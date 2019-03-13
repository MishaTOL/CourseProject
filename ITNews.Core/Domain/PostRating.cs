using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Core.Domain
{
    public class PostRating : EntityBase
    {
        public int Rating { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
