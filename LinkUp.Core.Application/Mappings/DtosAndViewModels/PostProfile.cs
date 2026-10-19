using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship.Post;
using LinkUp.Core.Application.ViewModels.Comment;
using LinkUp.Core.Application.ViewModels.Post;
using LinkUp.Core.Domain.Entities;

namespace LinkUp.Core.Application.Mappings
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostViewModel>()
                .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.User != null
                    ? $"{s.User.Name} {s.User.LastName}"  // ✅ Name en vez de FirstName
                    : "Usuario Desconocido"))
                .ForMember(d => d.CommentsCount, opt => opt.MapFrom(s => s.Comments != null
                    ? s.Comments.Count : 0))
                .ReverseMap();

            CreateMap<Post, PostDto>().ReverseMap();

            CreateMap<SavePostViewModel, Post>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.User, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Comment, CommentViewModel>().ReverseMap();
            CreateMap<SaveCommentViewModel, Comment>().ReverseMap();
        }
    }
}