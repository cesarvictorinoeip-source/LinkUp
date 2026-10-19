using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship.Friendship;
using LinkUp.Core.Domain.Entities;

namespace LinkUp.Core.Application.Mappings
{
    public class FriendshipProfile : Profile
    {
        public FriendshipProfile()
        {
            CreateMap<Friendship, FriendshipDto>().ReverseMap();
        }
    }
}