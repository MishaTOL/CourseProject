using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Core.Domain
{
    public class Tag : EntityBase
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Url { get; set; }
        public List<PostTag> PostTags { get; set; }

        public Tag()
        {
            PostTags = new List<PostTag>();
        }
    }
}
