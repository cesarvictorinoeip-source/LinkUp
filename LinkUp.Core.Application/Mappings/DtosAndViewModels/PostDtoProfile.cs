using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship.Post;
using LinkUp.Core.Domain.Entities;

namespace LinkUp.Core.Application.Mappings
{
    public class PostDtoProfile : Profile
    {
        public PostDtoProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
        }
    }
}