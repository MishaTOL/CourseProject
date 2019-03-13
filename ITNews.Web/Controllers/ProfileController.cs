using System.Threading.Tasks;
using AutoMapper;
using ITNews.Core.Domain;
using ITNews.Services.Photos;
using ITNews.Services.Posts;
using ITNews.Services.Users;
using ITNews.Web.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITNews.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly IPhotoService photoService;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public ProfileController(IPostService postService, IUserService userService, IPhotoService photoService, IMapper mapper, UserManager<User> userManager)
        {
            this.postService = postService;
            this.userService = userService;
            this.photoService = photoService;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(string username)
        {
            if(username != null)
            {
                var user = await userService.GetUserByUserNameAsync(username);
                var model = mapper.Map<User, ProfileViewModel>(user);
                if(model.ProfilePictureUrl == null)
                {
                    model.ProfilePictureUrl = "/uploads/profiles/default.jpg";
                }
                model.Posts = postService.GetUserPosts(user.Id);
                return View(model);
            }
            else
            {
                var user = await userService.GetUserByUserNameAsync(User.Identity.Name);
                var model = mapper.Map<User, ProfileViewModel>(user);
                if (model.ProfilePictureUrl == null)
                {
                    model.ProfilePictureUrl = "/uploads/profiles/default.jpg";
                }
                model.Posts = postService.GetUserPosts(user.Id);
                return View(model);
            }
        }

        public async Task<IActionResult> ChangeDescriptionField(string value)
        {
            var user = await userService.GetUserByUserNameAsync(User.Identity.Name);
            user.Description = value;

            await userManager.UpdateAsync(user);

            return Ok();
        }
        public async Task<IActionResult> ChangeFromField(string value)
        {
            var user = await userService.GetUserByUserNameAsync(User.Identity.Name);
            user.From = value;

            await userManager.UpdateAsync(user);

            return Ok();
        }
        public async Task<IActionResult> ChangeLivesField(string value)
        {
            var user = await userService.GetUserByUserNameAsync(User.Identity.Name);
            user.Lives = value;

            await userManager.UpdateAsync(user);

            return Ok();
        }
        public async Task<IActionResult> ChangeWorkAtField(string value)
        {
            var user = await userService.GetUserByUserNameAsync(User.Identity.Name);
            user.WorkAt = value;

            await userManager.UpdateAsync(user);

            return Ok();
        }
    }
}