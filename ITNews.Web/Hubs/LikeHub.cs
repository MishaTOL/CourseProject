using ITNews.Core;
using ITNews.Core.Domain;
using ITNews.Services.Posts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Web.Hubs
{
    public class LikeHub : Hub
    {
        private readonly IPostService postService;
        private readonly IUnitOfWork unitOfWork;

        public LikeHub(IPostService postService, IUnitOfWork unitOfWork)
        {
            this.postService = postService;
            this.unitOfWork = unitOfWork;
        }

        public async Task LikeComment(int commentId)
        {
            if(await postService.HasUserLikeCommentAsync(commentId))
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
    }
}
