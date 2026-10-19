using LinkUp.Core.Application.ViewModels.User;
using LinkUp.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LinkUp.Core.Domain.Entities;
using AppUser = LinkUp.Core.Domain.Entities.AppUser; // ✅ alias explícito// ✅ en vez de LinkUp.Infrastructure.Identity.Entities

namespace LinkUp.WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAccountServiceForWebApp _userService;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(IAccountServiceForWebApp userService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        #region Login & Logout
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return await RedirectByUserRole();
            }

            return View(new LoginViewModel() { UserName = "", Password = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _userService.AuthenticateAsync(vm);

            if (result != null && !result.HasError)
            {
                return await RedirectByUserRole();
            }

            vm.Error = result?.Errors.FirstOrDefault() ?? "Usuario o contraseña incorrectos.";
            vm.Password = "";
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Registro (Create)
        [HttpGet]
        public IActionResult Register()
        {
            // Inicialización completa de campos vacíos para evitar errores en la vista
            return View(new SaveUserViewModel()
            {
                Name = "",
                LastName = "",
                Email = "",
                UserName = "",
                Password = "",
                ConfirmPassword = "",
                Phone = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            string result = await _userService.RegisterUserAsync(vm);

            if (!string.IsNullOrEmpty(result) && Guid.TryParse(result, out _))
            {
                return RedirectToAction("Index", "Login");
            }

            vm.Error = result;
            return View(vm);
        }
        #endregion

        #region Vistas de Soporte (Password & Email)
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordRequestViewModel() { Email = "" });
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _userService.ForgotPasswordAsync(vm);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordRequestViewModel() { Token = token, Email = email, Password = "", ConfirmPassword = "" });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _userService.ResetPasswordAsync(vm);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var response = await _userService.ConfirmAccountAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

        #region Helper
        private async Task<IActionResult> RedirectByUserRole()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin")) return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}