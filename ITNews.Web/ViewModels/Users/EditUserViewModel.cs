using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Web.ViewModels.Users
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Description { get; set; }
        public string Lives { get; set; }
        public string From { get; set; }
        public string WorkAt { get; set; }
    }
}
