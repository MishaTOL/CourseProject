using ITNews.Core.Domain;
using Microsoft.AspNetCore.Http;

namespace ITNews.Web.ViewModels.Posts
{
    public class CreatePostViewModel
    {
        public string PostName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public string CreatedById { get; set; }
        public IFormFileCollection Files { get; set; }
        public PostType PostType { get; set; }
    }
}
