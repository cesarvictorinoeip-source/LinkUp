using LinkUp.Core.Application.Interfaces;
using LinkUp.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkUp.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IPostService _postService;
        private readonly IAccountServiceForWebApp _userService;

        public FriendController(IFriendshipService friendshipService, IPostService postService, IAccountServiceForWebApp userService)
        {
            _friendshipService = friendshipService;
            _postService = postService;
            _userService = userService;
        }

        private string GetUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // Lista de amigos y sus publicaciones
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var friendIds = await _friendshipService.GetFriendIdsAsync(userId);
            var friends = await _friendshipService.GetUserFriends(userId);
            var posts = await _postService.GetAllForHome(friendIds);

            ViewBag.Friends = friends;
            ViewBag.CurrentUserId = userId;
            return View(posts);
        }

        // Ver publicaciones de un amigo específico
        public async Task<IActionResult> FriendPosts(string friendId)
        {
            var posts = await _postService.GetAllForHome(new List<string> { friendId });
            var friend = await _userService.GetUserByIdAsync(friendId);
            ViewBag.FriendName = friend?.UserName ?? friendId;
            return View(posts);
        }

        // Confirmar eliminar amigo
        [HttpGet]
        public async Task<IActionResult> Remove(int friendshipId)
        {
            var friendship = await _friendshipService.GetByIdAsync(friendshipId);
            if (friendship == null) return NotFound();
            ViewBag.FriendshipId = friendshipId;
            ViewBag.FriendName = friendship.UserTwoName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConfirmed(int friendshipId)
        {
            await _friendshipService.DeleteFriendship(friendshipId);
            return RedirectToAction("Index");
        }
    }
}