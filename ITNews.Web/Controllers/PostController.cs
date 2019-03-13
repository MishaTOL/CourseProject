using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Core;
using ITNews.Core.Domain;
using ITNews.Services.Photos;
using ITNews.Services.Posts;
using ITNews.Services.Tags;
using ITNews.Services.Users;
using ITNews.Web.ViewModels;
using ITNews.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITNews.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IPostService postService;
        private readonly ITagService tagService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IPhotoService photoService;

        public PostController(IUserService userService, IMapper mapper, IPostService postService, ITagService tagService, IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IPhotoService photoService)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.postService = postService;
            this.tagService = tagService;
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
            this.photoService = photoService;
        }
        public async Task<IActionResult> Index(int postId)
        {
            var post = await postService.GetPostByIdAsync(postId);
            var model = mapper.Map<Post, PostViewModel>(post);
            model.CreatedBy = await userService.GetUserByIdAsync(post.CreatedById);
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            var user = await userService.GetUserByUserNameAsync(User.Identity.Name);
            var post = mapper.Map<CreatePostViewModel, Post>(model);
            post.CreatedBy = user;
            post.CreatedById = user.Id;
            post.CreatedOn = DateTime.Now;
            await postService.AddPostAsync(post);
            await unitOfWork.CompleteAsync();

            if(model.Tags != null)
            {
                String[] TagsStringArray = model.Tags.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var tagName in TagsStringArray)
                {
                    var existedTag = await tagService.GetTagByNameAsync(tagName);
                    if(existedTag != null)
                    {
                        existedTag.Weight += 1;
                        await postService.AddPostTagAsync(new PostTag { TagId = existedTag.Id, PostId = post.Id });
                    }
                    else
                    {
                        var newTag = new Tag { Name = tagName };
                        await tagService.AddTagAsync(newTag);
                        await postService.AddPostTagAsync(new PostTag { TagId = newTag.Id, PostId = post.Id });
                    }                
                }
                await unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(ProfileController.Index), "Profile", user.UserName);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var model = mapper.Map<Post, EditPostViewModel>(post);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = await postService.GetPostByIdAsync(model.Id);
                if (post != null)
                {
                    post.PostName = model.PostName;
                    post.Description = model.Description;
                    post.Content = model.Content;
                    post.CreatedOn = DateTime.Now.Date;
                }
                var currentPostTags = post.PostTags.Select(x => x.Tag.Name);
                if (model.Tags != null)
                {
                    String[] TagsStringArray = model.Tags.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tagName in TagsStringArray)
                    {
                        var existedTag = await tagService.GetTagByNameAsync(tagName);
                        if (existedTag != null && !(currentPostTags.Contains(tagName)))
                        {
                            existedTag.Weight += 1;
                            await postService.AddPostTagAsync(new PostTag { TagId = existedTag.Id, PostId = post.Id });
                        }
                        else if(!(currentPostTags.Contains(tagName)))
                        {
                            var newTag = new Tag { Name = tagName };
                            await tagService.AddTagAsync(newTag);
                            await postService.AddPostTagAsync(new PostTag { TagId = newTag.Id, PostId = post.Id });
                        }
                    }
                    await unitOfWork.CompleteAsync();
                }

                await unitOfWork.CompleteAsync();

                return RedirectToAction(nameof(ProfileController.Index), "Profile");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = postService.GetPostById(id);
            if (post != null)
            {
                postService.RemovePost(post);
                await unitOfWork.CompleteAsync();
            }          
            return RedirectToAction(nameof(ProfileController.Index), "Profile", post.CreatedBy.UserName);
        }

        [HttpPost]
        public async Task<string> SaveFile(IFormFile file)
        {
            return await photoService.UploadAsync(file);
        }

        public async Task<JsonResult> TagComplete(string term)
        {
            var tags = await tagService.GetAllTagsAsync();
            var tagNames = tags.Where(t => t.Name.Contains(term))
                .Select(x => new AutoCompleteViewModel
                {
                    Id = x.Id,
                    Label = x.Name,
                    Value = x.Name
                }).ToList();

            return new JsonResult(tagNames);
        }

        public async Task<IActionResult> PostsByName(string searchWord)
        {
            var posts = await postService.GetPostsByNameAsync(searchWord);
            return View(posts);
        }

        public async Task<IActionResult> PostsByTag(int tagId)
        {
            var postTags = await postService.GetPostTagsByTagIdAsync(tagId);
            return View(postTags);
        }
    }
}