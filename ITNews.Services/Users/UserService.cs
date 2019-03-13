using ITNews.Core;
using ITNews.Core.Domain;
using ITNews.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor accessor;
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IHttpContextAccessor accessor, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            this.accessor = accessor;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            var user = await userManager.GetUserAsync(accessor.HttpContext.User);

            return await userManager.GetUserIdAsync(user);
        }

        public async Task<string> GetCurrentUserNameAsync()
        {
            var user = await userManager.GetUserAsync(accessor.HttpContext.User);

            return await userManager.GetUserNameAsync(user);
        }

        public async Task<IQueryable<User>> GetAllUsersExceptCurrentUser()
        {
            var currentUserId = await GetCurrentUserIdAsync();

            return userManager.Users.Where(u => u.Id != currentUserId);
        }

        public async Task<string> GetUserProfilePictureAsync()
        {
            var user = await GetUserByIdAsync(await GetCurrentUserIdAsync());

            return user.GetProfilePicture();
        }

        public async Task<bool> IsProfilePageForCurrentUserAsync(string username)
        {
            var currentUser = await GetUserByIdAsync(await GetCurrentUserIdAsync());

            return string.Equals(currentUser.UserName, username, StringComparison.OrdinalIgnoreCase);
        }
    }
}
