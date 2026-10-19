using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship.Game;
using LinkUp.Core.Application.Dtos.Battleship.Playershot;
using LinkUp.Core.Application.Dtos.Battleship.Ship;
using LinkUp.Core.Domain.Entities;

namespace LinkUp.Core.Application.Mappings
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<Game, GameDto>()
                .ForMember(d => d.Player1Name, opt => opt.MapFrom(s => s.Player1 != null ? s.Player1.Name : ""))
                .ForMember(d => d.Player2Name, opt => opt.MapFrom(s => s.Player2 != null ? s.Player2.Name : ""))
                .ForMember(d => d.CurrentTurnPlayerId, opt => opt.MapFrom(s => s.PlayerTurnId))
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.StartedAt, opt => opt.MapFrom(s => s.StartedAt))
                .ForMember(d => d.FinishedAt, opt => opt.MapFrom(s => s.FinishedAt))
                .ReverseMap();

            CreateMap<Ship, ShipDto>().ReverseMap();
            CreateMap<PlayerShot, PlayerShotDto>().ReverseMap();
        }
    }
}