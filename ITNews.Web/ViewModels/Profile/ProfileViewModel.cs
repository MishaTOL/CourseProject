using ITNews.Core.Domain;
using System.Collections.Generic;

namespace ITNews.Web.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfileCoverPictureUrl { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Lives { get; set; }
        public string From { get; set; }
        public string WorkAt { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
