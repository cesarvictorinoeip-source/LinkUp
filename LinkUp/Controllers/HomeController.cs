using System.Security.Claims;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Dtos.Battleship.Post;
using LinkUp.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly IFriendshipService _friendshipService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IPostService postService, IFriendshipService friendshipService, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
            _friendshipService = friendshipService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null) return RedirectToAction("Index", "Login");
            string userId = userClaim.Value;

            var friendIds = await _friendshipService.GetFriendIdsAsync(userId);
            if (!friendIds.Contains(userId))
                friendIds.Add(userId);

            var posts = await _postService.GetAllForHome(friendIds);
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto vm, IFormFile? ImageFile)
        {
            var userClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim != null && !string.IsNullOrEmpty(vm.Content))
            {
                vm.UserId = userClaim.Value;
                vm.CreatedAt = DateTime.Now;

                // Subir imagen si existe
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string basePath = "/images/posts/";
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + basePath);
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    vm.ImageUrl = basePath + fileName;
                }

                await _postService.AddAsync(vm);
            }

            return RedirectToAction("Index");
        }
    }
}