using System;
using System.Collections.Generic;

namespace ITNews.Core.Domain
{
    public class Post : EntityBase
    {
        public string PostName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public User CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public PostType PostType { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public List<PostTag> PostTags { get; set; }
        public ICollection<PostRating> Ratings { get; set; }
        public int? averageRating { get; set; }
        //public IEnumerable<Like> Likes { get; set; }
        public Post()
        {
            Photos = new List<Photo>();
            Comments = new List<Comment>();
            PostTags = new List<PostTag>();
            Ratings = new List<PostRating>();
        }

        public bool IsForCurrentUser(string userId)
        {
            return (string.Equals(CreatedById, userId, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
