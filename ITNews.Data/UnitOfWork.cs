using ITNews.Core;
using ITNews.Data.EFContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ITNews.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsDbContext _context;

        public UnitOfWork(NewsDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
