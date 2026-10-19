using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Dtos.Battleship.Ship;
using LinkUp.Core.Application.Dtos.Battleship.Playershot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LinkUp.Infrastructure.Identity.Services;

namespace LinkUp.Controllers
{
    [Authorize]
    public class BattleshipController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IPlayerShotService _playerShotService;
        private readonly IFriendshipService _friendshipService;
        private readonly IAccountServiceForWebApp _userService;

        public BattleshipController(
            IGameService gameService,
            IPlayerShotService playerShotService,
            IFriendshipService friendshipService,
            IAccountServiceForWebApp userService)
        {
            _gameService = gameService;
            _playerShotService = playerShotService;
            _friendshipService = friendshipService;
            _userService = userService;
        }

        private string GetUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // Lista de juegos activos e historial
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var activeGames = await _gameService.GetActiveGames(userId);
            var history = await _gameService.GetGameHistory(userId);
            var friends = await _friendshipService.GetUserFriends(userId);

            ViewBag.ActiveGames = activeGames;
            ViewBag.History = history;
            ViewBag.Friends = friends;
            ViewBag.CurrentUserId = userId;
            return View();
        }

        // Crear nueva partida
        [HttpPost]
        public async Task<IActionResult> Create(string opponentId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(opponentId))
            {
                TempData["Error"] = "Debe seleccionar un oponente.";
                return RedirectToAction("Index");
            }
            var game = await _gameService.CreateGameAsync(userId, opponentId);
            return RedirectToAction("PlaceShips", new { gameId = game.Id });
        }

        // Vista para colocar barcos
        [HttpGet]
        public async Task<IActionResult> PlaceShips(int gameId)
        {
            var userId = GetUserId();
            var game = await _gameService.GetByIdAsync(gameId);
            if (game == null) return NotFound();

            ViewBag.GameId = gameId;
            ViewBag.UserId = userId;
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceShips(int gameId, [FromForm] List<ShipDto> ships)
        {
            var userId = GetUserId();
            foreach (var ship in ships)
            {
                await _gameService.PlaceShipAsync(gameId, userId, ship);
            }
            return RedirectToAction("WaitingRoom", new { gameId });
        
        }

        // Sala de espera
        [HttpGet]
        public async Task<IActionResult> WaitingRoom(int gameId)
        {
            var game = await _gameService.GetByIdAsync(gameId);
            if (game == null) return NotFound();

            var bothReady = await _gameService.BothPlayersReadyAsync(gameId);
            ViewBag.BothReady = bothReady;
            return View(game);
        }

        // Vista principal del juego
        [HttpGet]
        public async Task<IActionResult> Play(int gameId)
        {
            var userId = GetUserId();
            var game = await _gameService.GetByIdAsync(gameId);
            if (game == null) return NotFound();

            var shots = await _playerShotService.GetShotsByGameAsync(gameId);
            ViewBag.Shots = shots;
            ViewBag.CurrentUserId = userId;
            ViewBag.IsMyTurn = game.CurrentTurnPlayerId == userId;
            return View(game);
        }

        // Atacar
        [HttpPost]
        public async Task<IActionResult> Attack(int gameId, int row, int column)
        {
            var userId = GetUserId();
            var coordinate = $"{row},{column}";
            await _gameService.AttackAsync(gameId, userId, coordinate);
            return RedirectToAction("Play", new { gameId });
        }

        // Rendirse
        [HttpPost]
        public async Task<IActionResult> Surrender(int gameId)
        {
            var userId = GetUserId();
            await _gameService.SurrenderAsync(gameId, userId);
            return RedirectToAction("Index");
        }

        // Historial detallado
        [HttpGet]
        public async Task<IActionResult> GameDetail(int gameId)
        {
            var game = await _gameService.GetByIdAsync(gameId);
            if (game == null) return NotFound();
            var shots = await _playerShotService.GetShotsByGameAsync(gameId);
            ViewBag.Shots = shots;
            return View(game);
        }
    }
}