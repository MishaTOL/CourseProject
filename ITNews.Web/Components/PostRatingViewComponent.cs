using ITNews.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Web.Components
{
    public class PostRatingViewComponent : ViewComponent
    {
        private readonly IPostService postService;

        public PostRatingViewComponent(IPostService postService)
        {
            this.postService = postService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int postId)
        {
            return View(await postService.GetPostRating(postId));
        }
    }
}
