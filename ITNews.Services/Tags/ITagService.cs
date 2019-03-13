using ITNews.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ITNews.Services.Tags
{
    public interface ITagService
    {
        Task AddTagAsync(Tag tag);
        Task<Tag> GetTagByNameAsync(string tagName);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<IEnumerable<string>> GetAllTagNamesAsync();
    }
}
