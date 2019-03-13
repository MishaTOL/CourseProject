using ITNews.Core.Domain;
using ITNews.Core.Repository;
using ITNews.Data.EFContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Data.Repository
{
    public class PostRatingRepository : Repository<PostRating>, IPostRatingRepository
    {
        public PostRatingRepository(NewsDbContext context)
            : base(context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }
    }
}
