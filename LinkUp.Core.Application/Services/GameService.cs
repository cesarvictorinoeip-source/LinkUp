using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.Battleship.Game;
using LinkUp.Core.Application.Dtos.Battleship.Ship;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class GameService : GenericService<Game, GameDto>, IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IShipRepository _shipRepository;
        private readonly IPlayerShotRepository _playerShotRepository;
        private readonly IMapper _mapper;

        public GameService(
            IGameRepository gameRepository,
            IShipRepository shipRepository,
            IPlayerShotRepository playerShotRepository,
            IMapper mapper) : base(gameRepository, mapper)
        {
            _gameRepository = gameRepository;
            _shipRepository = shipRepository;
            _playerShotRepository = playerShotRepository;
            _mapper = mapper;
        }

        public async Task<List<GameDto>> GetActiveGames(string userId)
        {
            var games = await _gameRepository.GetAllQuery()
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Where(g => (g.Player1Id == userId || g.Player2Id == userId) && !g.IsFinished)
                .ToListAsync();
            return _mapper.Map<List<GameDto>>(games);
        
        }

        public async Task<List<GameDto>> GetGameHistory(string userId)
        {
            var games = await _gameRepository.GetGameHistory(userId);
            return _mapper.Map<List<GameDto>>(games);
        }

        public override async Task<GameDto?> GetByIdAsync(int id)
        {
            var game = await _gameRepository.GetGameWithDetails(id);
            return game == null ? null : _mapper.Map<GameDto>(game);
        }

        public async Task<GameDto> CreateGameAsync(string player1Id, string player2Id)
        {
            var game = new Game
            {
                Player1Id = player1Id,
                Player2Id = player2Id,
                PlayerTurnId = player1Id,
                IsFinished = false,
                CreatedAt = DateTime.Now,
                LastMove = DateTime.Now
            };
            await _gameRepository.AddAsync(game);
            return _mapper.Map<GameDto>(game);
        }

        public async Task<bool> PlaceShipAsync(int gameId, string playerId, ShipDto shipDto)
        {
            var ships = await _shipRepository.GetShipsByPlayer(gameId, playerId);
            var ship = _mapper.Map<Ship>(shipDto);
            ship.GameId = gameId;
            ship.PlayerId = playerId;

            foreach (var existing in ships)
            {
                if (existing.StartX == ship.StartX && existing.StartY == ship.StartY)
                    return false;
            }

            await _shipRepository.AddAsync(ship);
            return true;
        }

        public async Task<bool> BothPlayersReadyAsync(int gameId)
        {
            var game = await _gameRepository.GetGameWithDetails(gameId);
            if (game == null) return false;

            var p1Ready = await _shipRepository.AllShipsPlaced(gameId, game.Player1Id);
            var p2Ready = await _shipRepository.AllShipsPlaced(gameId, game.Player2Id);
            return p1Ready && p2Ready;
        }

        public async Task<bool> AttackAsync(int gameId, string attackerId, string coordinate)
        {
            var game = await _gameRepository.GetGameWithDetails(gameId);
            if (game == null || game.IsFinished) return false;
            if (game.PlayerTurnId != attackerId) return false;

            var parts = coordinate.Split(',');
            int row = int.Parse(parts[0]);
            int col = int.Parse(parts[1]);

            var alreadyAttacked = await _playerShotRepository.CellAlreadyAttacked(gameId, attackerId, row, col);
            if (alreadyAttacked) return false;

            var opponentId = game.Player1Id == attackerId ? game.Player2Id : game.Player1Id;
            var opponentShips = game.Ships.Where(s => s.PlayerId == opponentId).ToList();

            bool isHit = false;
            foreach (var ship in opponentShips)
            {
                for (int i = 0; i < ship.Size; i++)
                {
                    int shipRow = ship.IsVertical ? ship.StartX + i : ship.StartX;
                    int shipCol = ship.IsVertical ? ship.StartY : ship.StartY + i;
                    if (shipRow == row && shipCol == col)
                    {
                        isHit = true;
                        ship.Hits++;
                        break;
                    }
                }
            }

            var shot = new PlayerShot
            {
                GameId = gameId,
                PlayerId = attackerId,
                Row = row,
                Column = col,
                IsHit = isHit,
                CreatedAt = DateTime.Now
            };
            await _playerShotRepository.AddAsync(shot);

            game.PlayerTurnId = game.Player1Id == attackerId ? game.Player2Id : game.Player1Id;
            game.LastMove = DateTime.Now;

            if (opponentShips.All(s => s.IsSunk))
            {
                game.IsFinished = true;
                game.WinnerId = attackerId;
                game.FinishedAt = DateTime.Now;
            }

            await _gameRepository.UpdateAsync(game, game.Id);
            return true;
        }

        public async Task SurrenderAsync(int gameId, string playerId)
        {
            var game = await _gameRepository.GetGameWithDetails(gameId);
            if (game == null) return;

            game.IsFinished = true;
            game.WinnerId = game.Player1Id == playerId ? game.Player2Id : game.Player1Id;
            game.FinishedAt = DateTime.Now;

            await _gameRepository.UpdateAsync(game, game.Id);
        }

        public async Task CheckAbandonedGamesAsync()
        {
            var activeGames = _gameRepository.GetAllQuery()
                .Where(g => !g.IsFinished && g.LastMove < DateTime.Now.AddHours(-48))
                .ToList();

            foreach (var game in activeGames)
            {
                game.IsFinished = true;
                game.WinnerId = game.Player1Id == game.PlayerTurnId
                    ? game.Player2Id
                    : game.Player1Id;
                game.FinishedAt = DateTime.Now;
                await _gameRepository.UpdateAsync(game, game.Id);
            }
        }
    }
}