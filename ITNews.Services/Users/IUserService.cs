using ITNews.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITNews.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByUserNameAsync(string username);
        Task<string> GetCurrentUserIdAsync();
        Task<string> GetCurrentUserNameAsync();
        Task<IQueryable<User>> GetAllUsersExceptCurrentUser();
        Task<string> GetUserProfilePictureAsync();
        Task<bool> IsProfilePageForCurrentUserAsync(string username);
    }
}
