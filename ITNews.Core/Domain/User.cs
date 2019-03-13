using ITNews.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ITNews.Core.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfileCoverPictureUrl { get; set; }
        public string Description { get; set; }
        public string Lives { get; set; }
        public string From { get; set; }
        public string WorkAt { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<PostRating> Ratings { get; set; }

        public User()
        {
            Posts = new List<Post>();
            Comments = new List<Comment>();
            Likes = new List<Like>();
            Ratings = new List<PostRating>();
        }

        public string GetProfilePicture()
        {
            return ProfilePictureUrl ?? $"/assets/images/avatars/{CommonHelper.GenerateRandomValue(limit: 5)}.jpg";
        }


        public string GetProfileCoverPicture()
        {
            return ProfileCoverPictureUrl ?? "/assets/images/no-profile-cover.jpg";
        }
    }
}
