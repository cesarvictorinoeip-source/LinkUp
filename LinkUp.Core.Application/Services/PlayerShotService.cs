using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.Battleship.Playershot;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class PlayerShotService
        : GenericService<PlayerShot, PlayerShotDto>, IPlayerShotService
    {
        private readonly IPlayerShotRepository _playerShotRepository;
        private readonly IMapper _mapper;

        public PlayerShotService(
            IPlayerShotRepository playerShotRepository,
            IMapper mapper)
            : base(playerShotRepository, mapper)
        {
            _playerShotRepository = playerShotRepository;
            _mapper = mapper;
        }

        public override async Task<PlayerShotDto?> GetByIdAsync(int id)
        {
            var entity = await _playerShotRepository
                .GetAllQueryWithInclude(new List<string> { "Game", "Player" })
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : _mapper.Map<PlayerShotDto>(entity);
        }

        public async Task<List<PlayerShotDto>> GetShotsByGameAsync(int gameId)
        {
            return await _playerShotRepository
                .GetAllQuery()
                .Where(s => s.GameId == gameId)
                .ProjectTo<PlayerShotDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}