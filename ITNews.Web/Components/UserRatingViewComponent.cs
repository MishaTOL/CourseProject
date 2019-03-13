using ITNews.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Web.Components
{
    public class UserRatingViewComponent : ViewComponent
    {
        private readonly IPostService postService;

        public UserRatingViewComponent(IPostService postService)
        {
            this.postService = postService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            return View(await postService.GetUserRating(userId));
        }
    }
}
