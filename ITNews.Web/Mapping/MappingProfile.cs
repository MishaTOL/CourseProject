using AutoMapper;
using ITNews.Core.Domain;
using ITNews.Web.ViewModels.Account;
using ITNews.Web.ViewModels.Posts;
using ITNews.Web.ViewModels.Profile;

namespace ITNews.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, ProfileViewModel>();
            CreateMap<CreatePostViewModel, Post>();
            CreateMap<Post, EditPostViewModel>();
            CreateMap<Post, PostViewModel>();
        }
    }
}
