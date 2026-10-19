using LinkUp.Core.Application.ViewModels.User;
using LinkUp.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;

namespace LinkUp.WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IAccountServiceForWebApp _userService;

        public UserController(IAccountServiceForWebApp userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        #region Mi Perfil
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Index", "Login");

            var vm = await _userService.GetUserByIdAsync(userId);
            if (vm == null) return NotFound();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(SaveUserViewModel vm, IFormFile? ProfileImageFile)
        {
            // Obtener el ID del usuario logueado directamente del claim
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            vm.Id = userId; // ✅ Forzar el ID correcto

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("UserName");
            ModelState.Remove("Email");

            if (!ModelState.IsValid) return View(vm);

            if (ProfileImageFile != null && ProfileImageFile.Length > 0)
                vm.ProfileImage = await UploadFile(ProfileImageFile);

            if (string.IsNullOrEmpty(vm.ProfileImage))
            {
                var current = await _userService.GetUserByIdAsync(userId ?? "");
                vm.ProfileImage = current?.ProfileImage;
            }

            await _userService.UpdateUserAsync(vm);
            return RedirectToAction("Index", "Home");
        
        }
        #endregion

        #region Crear Usuario
        [HttpGet]
        public IActionResult Create()
        {
            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.ProfileImageFile != null)
                vm.ProfileImage = await UploadFile(vm.ProfileImageFile);

            string result = await _userService.RegisterUserAsync(vm);

            if (!string.IsNullOrEmpty(result) && Guid.TryParse(result, out _))
                return RedirectToAction("Index");

            vm.Error = result;
            return View(vm);
        }
        #endregion

        #region Edición
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");
            var vm = await _userService.GetUserByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.Password))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (!ModelState.IsValid) return View(vm);

            if (vm.ProfileImageFile != null)
                vm.ProfileImage = await UploadFile(vm.ProfileImageFile);

            await _userService.UpdateUserAsync(vm);
            return RedirectToAction("Index");
        }
        #endregion

        #region Eliminación
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");
            var vm = await _userService.GetUserByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private async Task<string> UploadFile(IFormFile file)
        {
            string basePath = "/images/users/";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + basePath);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return basePath + fileName;
        }
        #endregion
    }
}