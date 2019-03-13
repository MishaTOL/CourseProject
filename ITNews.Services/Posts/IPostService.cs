using ITNews.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ITNews.Services.Posts
{
    public interface IPostService
    {
        Task AddPostAsync(Post post);
        Task AddPostTagAsync(PostTag postTag);
        void RemovePost(Post post);
        Task AddCommentToPostAsync(Comment comment);
        Task AddRatingToPostAsync(PostRating postRating);
        Task<Post> GetPostByIdAsync(int postId);
        Task<Comment> GetCommentByIdAsync(int commentId);
        int GetCommentLikesCount(int commentId);
        Post GetPostById(int postId);
        IEnumerable<Post> GetAllPosts();
        IEnumerable<Post> GetAllPostsOrderedByRating();
        IEnumerable<Post> GetUserPosts(string userId);
        IEnumerable<Post> GetUserPostsByUserName(string userName);
        IEnumerable<Comment> GetCommentsByPostId(int postId);
        IEnumerable<PostRating> GetRatingsByPost(Post post);
        IEnumerable<Tag> GetAllPostTags(Post post);
        Task LikeCommentAsync(int postId);
        Task UnLikeCommentAsync(int postId);
        Task<int> GetTotalCommentLikesAsync(int postId);
        Task<bool> HasUserLikeCommentAsync(int postId);
        Task<bool> HasUserRatePostAsync(int postId);
        Task<int> GetUserTotalPostsAsync(string userId);
        Task<int> GetTotalCommentsForPostAsync(int postId);
        void RemoveAllUserComments(string userId);
        Task<int> GetPostRating(int postId);
        Task<int> GetUserRating(string userId);
        Task<IEnumerable<Post>> GetPostsByNameAsync(string postName);
        Task<IEnumerable<PostTag>> GetPostTagsByTagIdAsync(int tagId);
    }
}
