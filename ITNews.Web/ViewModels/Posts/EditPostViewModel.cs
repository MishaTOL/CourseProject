using ITNews.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Web.ViewModels.Posts
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public string CreatedById { get; set; }
        public PostType PostType { get; set; }
    }
}
