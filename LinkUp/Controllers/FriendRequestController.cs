using LinkUp.Core.Application.Interfaces;
using LinkUp.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkUp.Controllers
{
    [Authorize]
    public class FriendRequestController : Controller
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IAccountServiceForWebApp _userService;

        public FriendRequestController(IFriendshipService friendshipService, IAccountServiceForWebApp userService)
        {
            _friendshipService = friendshipService;
            _userService = userService;
        }

        private string GetUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // Lista de solicitudes recibidas y enviadas
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var received = await _friendshipService.GetPendingRequestsReceived(userId);
            var sent = await _friendshipService.GetPendingRequestsSent(userId);

            ViewBag.Received = received;
            ViewBag.Sent = sent;
            return View();
        }

        // Confirmar aceptar solicitud
        [HttpGet]
        public async Task<IActionResult> Accept(int id)
        {
            var friendship = await _friendshipService.GetByIdAsync(id);
            if (friendship == null) return NotFound();
            ViewBag.FriendshipId = id;
            ViewBag.SenderName = friendship.IdUserOne;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AcceptConfirmed(int id)
        {
            await _friendshipService.AcceptFriendRequest(id);
            return RedirectToAction("Index");
        }

        // Confirmar rechazar solicitud
        [HttpGet]
        public async Task<IActionResult> Reject(int id)
        {
            var friendship = await _friendshipService.GetByIdAsync(id);
            if (friendship == null) return NotFound();
            ViewBag.FriendshipId = id;
            ViewBag.SenderName = friendship.IdUserOne;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RejectConfirmed(int id)
        {
            await _friendshipService.RejectFriendRequest(id);
            return RedirectToAction("Index");
        }

        // Confirmar eliminar solicitud enviada
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var friendship = await _friendshipService.GetByIdAsync(id);
            if (friendship == null) return NotFound();
            ViewBag.FriendshipId = id;
            ViewBag.ReceiverName = friendship.IdUserTwo;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _friendshipService.DeleteFriendship(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> New(string search = "")
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Index", "Login");

            var allUsers = await _userService.GetAllUsersAsync();

            var availableUsers = new List<LinkUp.Core.Application.ViewModels.User.UserViewModel>();

            foreach (var u in allUsers)
            {
                if (u.Id == userId) continue;
                if (!string.IsNullOrEmpty(search) &&
                !(u.UserName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)) continue;

                var hasRelation = await _friendshipService.AlreadyFriendsOrPending(userId, u.Id ?? "");
                if (!hasRelation) availableUsers.Add(u);
            }

            ViewBag.Search = search;
            return View(availableUsers);
        }

        [HttpPost]
        public async Task<IActionResult> Send(string receiverId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(receiverId))
            {
                TempData["Error"] = "Debe seleccionar un usuario.";
                return RedirectToAction("New");
            }
            await _friendshipService.SendFriendRequest(userId, receiverId);
            return RedirectToAction("Index");
        }
    }
}