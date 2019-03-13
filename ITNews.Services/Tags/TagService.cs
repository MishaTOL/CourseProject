using ITNews.Core.Domain;
using ITNews.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITNews.Services.Tags
{
    public class TagService : ITagService
    {
        private readonly ITagRepository tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        public async Task AddTagAsync(Tag tag)
        {
            await tagRepository.AddAsync(tag);
        }
        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await tagRepository.Table.FirstOrDefaultAsync(t => t.Name == tagName);
        }
        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            var tags = await tagRepository.FindAll();
            return tags;
        }

        public async Task<IEnumerable<string>> GetAllTagNamesAsync()
        {
            var tagNames = await tagRepository.Table.Select(t => t.Name).ToListAsync();
            return tagNames;
        }
    }
}
