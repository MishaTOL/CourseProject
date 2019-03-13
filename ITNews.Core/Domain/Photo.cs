using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Core.Domain
{
    public class Photo : EntityBase
    {
        public string FileName { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
