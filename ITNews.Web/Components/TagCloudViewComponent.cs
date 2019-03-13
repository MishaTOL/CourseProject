using ITNews.Services.Tags;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Web.Components
{
    public class TagCloudViewComponent : ViewComponent
    {
        private readonly ITagService tagService;

        public TagCloudViewComponent(ITagService tagService)
        {
            this.tagService = tagService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await tagService.GetAllTagsAsync());
        }
    }
}
