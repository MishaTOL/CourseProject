using System;

namespace ITNews.Core.Domain
{
    public class Like : EntityBase
    {
        public Comment Comment { get; set; }
        public int CommentId { get; set; }
        public User LikeBy { get; set; }
        public string LikeById { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
