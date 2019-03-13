using ITNews.Core.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ITNews.Services.Photos
{
    public interface IPhotoService
    {
        Task UploadAsync(IFormFileCollection files, int postId);
        Task<string> UploadAsync(IFormFile file);
        Task<IEnumerable<Photo>> GetUserPhotosAsync(string userId);
        Task<int> GetUserTotalPhotosAsync(string userId);
        void RemovePhotoFromDisk(string profilePictureUrl);
    }
}
