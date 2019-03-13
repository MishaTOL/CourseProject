using ITNews.Core;
using ITNews.Core.Domain;
using ITNews.Core.Repository;
using ITNews.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> postRepository;
        private readonly IUserService userService;
        private readonly IRepository<Comment> commentRepository;
        private readonly IRepository<Like> likeRepository;
        private readonly IRepository<PostTag> postTagRepository;
        private readonly IRepository<PostRating> postRatingRepository;
        private readonly IUnitOfWork unitOfWork;

        public PostService(
            IRepository<Post> postRepository,
            IRepository<Comment> commentRepository,
            IRepository<Like> likeRepository,
            IUserService userService,
            IRepository<PostTag> postTagRepository,
            IRepository<PostRating> postRatingRepository,
            IUnitOfWork unitOfWork)
        {
            this.postRepository = postRepository;
            this.userService = userService;
            this.commentRepository = commentRepository;
            this.likeRepository = likeRepository;
            this.postTagRepository = postTagRepository;
            this.postRatingRepository = postRatingRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task AddPostAsync(Post post)
        {
            await postRepository.AddAsync(post);
        }

        public async Task AddPostTagAsync(PostTag postTag)
        {
            await postTagRepository.AddAsync(postTag);
        }
        public void RemovePost(Post post)
        {
           postRepository.Remove(post);
        }

        public async Task AddCommentToPostAsync(Comment comment)
        {
            await commentRepository.AddAsync(comment);
        }

        public async Task AddRatingToPostAsync(PostRating postRating)
        {
            await postRatingRepository.AddAsync(postRating);
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await postRepository.Table
                .Include(p => p.CreatedBy)
                .Include(p => p.PostTags)
                .ThenInclude(postTag => postTag.Tag)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }
        public IEnumerable<PostRating> GetRatingsByPost(Post post)
        {
            var ratings = postRatingRepository.Table
                .Include(c => c.Post)
                .Include(c => c.User)
                .Where(c => c.PostId == post.Id)
                .ToList();

            return ratings;
        }

        public async Task<int> GetPostRating(int postId)
        {
            var post = await GetPostByIdAsync(postId);
            var ratings = GetRatingsByPost(post);
            if(ratings != null && ratings.Count() != 0)
            {
                var sumRating = 0;
                foreach (var postRating in post.Ratings)
                {
                    sumRating += postRating.Rating;
                }
                post.averageRating = sumRating / (post.Ratings.Count());
                await unitOfWork.CompleteAsync();
                return sumRating / (post.Ratings.Count());
            }
            post.averageRating = 0;
            await unitOfWork.CompleteAsync();
            return 0;
        }

        public async Task<int> GetUserRating(string userId)
        {
            var posts = GetUserPosts(userId);
            var sumRating = 0;
            foreach(var post in posts)
            {
                sumRating += await GetPostRating(post.Id);
            }
            if(sumRating > 0)
            {
                return sumRating / posts.Count();
            }
            return 0;
            
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await commentRepository.Table.FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public int GetCommentLikesCount(int commentId)
        {
            return likeRepository.Table.Where(l => l.CommentId == commentId).Count();
        }

        public async Task<Comment> GetCommentByUserIdAsync(string userId)
        {
            return await commentRepository.Table.FirstOrDefaultAsync(c => c.CommentById == userId);
        }
        public Post GetPostById(int postId)
        {
            return postRepository.Table.Include(p => p.CreatedBy).FirstOrDefault(p => p.Id == postId);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var posts = postRepository.Table
                .Include(p => p.Comments)
                .Include(p => p.Photos)
                .Include(p => p.CreatedBy)
                .Include(p => p.PostTags)
                .ThenInclude(postTag => postTag.Tag)
                .OrderByDescending(p => p.CreatedOn)
                .ToList();
            return posts;
        }

        public IEnumerable<Post> GetAllPostsOrderedByRating()
        {
            var posts = postRepository.Table
                .Include(p => p.Comments)
                .Include(p => p.Photos)
                .Include(p => p.CreatedBy)
                .Include(p => p.PostTags)
                .ThenInclude(postTag => postTag.Tag)
                .OrderByDescending(p => p.averageRating)
                .ToList();
            return posts;
        }

        public IEnumerable<Post> GetUserPosts(string userId)
        {
            var userPosts = postRepository.Table
                .Include(p => p.Comments)
                .Include(p => p.Photos)
                .Include(p => p.CreatedBy)
                .Include(p => p.PostTags)
                .ThenInclude(postTag => postTag.Tag)
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.CreatedById == userId)
                .ToList();

            return userPosts;
        }

        public async Task<IEnumerable<PostTag>> GetPostTagsByTagIdAsync(int tagId)
        {
            var postTags = await postTagRepository.Table.Where(pt => pt.TagId == tagId).ToListAsync();
            
            return postTags;
        }

        public async Task<IEnumerable<Post>> GetPostsByNameAsync(string postName)
        {
            var posts = await postRepository.Table
                .Include(p => p.Comments)
                .Include(p => p.Photos)
                .Include(p => p.CreatedBy)
                .Include(p => p.PostTags)
                .ThenInclude(postTag => postTag.Tag)
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.PostName.Contains(postName))
                .ToListAsync();

            return posts;
        }

        public IEnumerable<Post> GetUserPostsByUserName(string userName)
        {
            var userPosts = postRepository.Table
                .Include(p => p.Comments)
                .Include(p => p.Photos)
                .Include(p => p.CreatedBy)
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.CreatedBy.UserName == userName)
                .ToList();

            return userPosts;
        }


        public IEnumerable<Comment> GetCommentsByPostId(int postId)
        {
            var comments = commentRepository.Table
                .Include(c => c.Post)
                .Include(c => c.CommentBy)
                .Include(c => c.Likes)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedOn)
                .ToList();

            return comments;
        }

        public IEnumerable<Tag> GetAllPostTags(Post post)
        {
            var tags = post.PostTags.Select(pt => pt.Tag).ToList();
            return tags;
        }

        public async Task LikeCommentAsync(int commentId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            var like = likeRepository.Table.FirstOrDefault(l => l.LikeById == userId && l.CommentId == commentId);

            if (like == null)
            {
                like = new Like
                {
                    LikeById = userId,
                    CommentId = commentId,
                    CreatedOn = DateTime.Now
                };

                await likeRepository.AddAsync(like);
            }

        }

        public async Task UnLikeCommentAsync(int commentId)
        {
            var userId = await userService.GetCurrentUserIdAsync();

            var like = likeRepository.Table.FirstOrDefault(l => l.CommentId == commentId && l.LikeById == userId);

            if (like != null)
            {
                likeRepository.Remove(like);
            }
        }

        public async Task<int> GetTotalCommentLikesAsync(int commentId)
        {
            return await likeRepository.Table.Where(l => l.CommentId == commentId).CountAsync();
        }

        public async Task<bool> HasUserLikeCommentAsync(int commentId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            return await likeRepository.Table.AnyAsync(l => l.CommentId == commentId && l.LikeById == userId);
        }

        public async Task<bool> HasUserRatePostAsync(int postId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            return await postRatingRepository.Table.AnyAsync(r => r.PostId == postId && r.UserId == userId);
        }

        public async Task<int> GetUserTotalPostsAsync(string userId)
        {
            return await postRepository.Table.Where(p => p.CreatedById == userId).CountAsync();
        }

        public async Task<int> GetTotalCommentsForPostAsync(int postId)
        {
            return await commentRepository.Table.Where(c => c.PostId == postId).CountAsync();
        }

        public void RemoveAllUserComments(string userId)
        {
            var userComments = commentRepository.Table.Where(c => c.CommentById == userId);
            foreach(var comment in userComments)
            {
                commentRepository.Remove(comment);
            }
        }
    }
}
