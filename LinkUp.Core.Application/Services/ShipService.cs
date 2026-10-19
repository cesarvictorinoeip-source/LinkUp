using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.Battleship.Ship;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using LinkUp.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class ShipService : GenericService<Ship, ShipDto>, IShipService
    {
        private readonly IShipRepository _shipRepository;
        private readonly IMapper _mapper;

        public ShipService(IShipRepository shipRepository, IMapper mapper) : base(shipRepository, mapper)
        {
            _shipRepository = shipRepository;
            _mapper = mapper;
        }

        public async Task<List<ShipDto>> GetPlayerShipsInGameAsync(int gameId, string userId)
        {
            try
            {
                return await _shipRepository.GetAllQuery()
                    .Where(s => s.GameId == gameId && s.PlayerId == userId)
                    .ProjectTo<ShipDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            catch (Exception) { return []; }
        }
    }
}