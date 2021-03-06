﻿using ITNews.Core.Domain;
using ITNews.Core.Repository;
using ITNews.Data.EFContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Data.Repository
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(NewsDbContext context)
            : base(context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }
    }
}
