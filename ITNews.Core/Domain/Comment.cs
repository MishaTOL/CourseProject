using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Core.Domain
{
    public class Comment : EntityBase
    {
        public User CommentBy { get; set; }
        public string CommentById { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; private set; } = DateTime.Now;
        public IEnumerable<Like> Likes { get; set; }
        public Comment()
        {
            Likes = new List<Like>();
        }
    }
}
