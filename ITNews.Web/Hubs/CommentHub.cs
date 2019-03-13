using ITNews.Core;
using ITNews.Core.Domain;
using ITNews.Services.Posts;
using ITNews.Services.Users;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ITNews.Web.Hubs
{
    public class CommentHub : Hub
    {
        private readonly IUserService userService;
        private readonly IPostService postService;
        private readonly IUnitOfWork unitOfWork;
        public CommentHub(IUserService userService, IPostService postService, IUnitOfWork unitOfWork)
        {
            this.userService = userService;
            this.postService = postService;
            this.unitOfWork = unitOfWork;
        }
        public async Task SendComment(string comment, string userName, int postId)
        {
            var user = await userService.GetUserByUserNameAsync(userName);
            var post = await postService.GetPostByIdAsync(postId);
            var newComment = new Comment
            {
                Content = comment,
                CommentBy = user,
                CommentById = user.Id,
                Post = post,
                PostId = post.Id
            };
            await postService.AddCommentToPostAsync(newComment);
            await unitOfWork.CompleteAsync();
            var newCommentId = newComment.Id;
            await Clients.All.SendAsync("ReceiveComment");
        }

        public async Task LikeComment(int commentId)
        {
            if (await postService.HasUserLikeCommentAsync(commentId))
            {
                await postService.UnLikeCommentAsync(commentId);
                await unitOfWork.CompleteAsync();
            }
            else
            {
                await postService.LikeCommentAsync(commentId);
                await unitOfWork.CompleteAsync();
            }

            var likesCount = postService.GetCommentLikesCount(commentId);
            await Clients.All.SendAsync("ReceiveLike", likesCount);
        }

        public async Task SendRating(int rating, string userName, int postId)
        {
            if(await postService.HasUserRatePostAsync(postId))
            {
                var message = "You already rate this post.";
                await Clients.All.SendAsync("ReceiveRating", message);
            }
            else
            {
                var user = await userService.GetUserByUserNameAsync(userName);
                var post = await postService.GetPostByIdAsync(postId);
                var newRating = new PostRating
                {
                    Rating = rating,
                    Post = post,
                    PostId = post.Id,
                    User = user,
                    UserId = user.Id
                };
                await postService.AddRatingToPostAsync(newRating);
                await unitOfWork.CompleteAsync();

                var message = "You successfuly rate this post.";
                await Clients.All.SendAsync("ReceiveRating", message);
            }
        }
    }
}
