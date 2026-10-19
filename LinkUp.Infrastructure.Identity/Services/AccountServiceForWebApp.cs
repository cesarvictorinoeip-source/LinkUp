using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LinkUp.Core.Application.ViewModels.User;
using LinkUp.Core.Domain.Entities;
using LinkUp.Core.Application.Dtos.Battleship.User;
using LinkUp.Core.Application.Dtos.Battleship.Email;
using LinkUp.Core.Application.Interfaces;

namespace LinkUp.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApp : IAccountServiceForWebApp
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountServiceForWebApp(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        #region Autenticación y Registro

        public async Task<AuthenticationResponse> AuthenticateAsync(LoginViewModel loginDto)
        {
            AuthenticationResponse response = new()
            {
                HasError = false,
                Errors = new List<string>()
            };

            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"Usuario {loginDto.UserName} no encontrado.");
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                loginDto.Password,
                false,
                true
            );

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Errors.Add("Contraseña incorrecta.");
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email!;
            response.UserName = user.UserName!;
            response.Roles = (await _userManager.GetRolesAsync(user)).ToList();

            return response;
        }

        public async Task<string> RegisterUserAsync(SaveUserViewModel vm)
        {
            var existingUser = await _userManager.FindByNameAsync(vm.UserName);
            if (existingUser != null)
                return "El nombre de usuario ya está registrado.";

            var existingEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (existingEmail != null)
                return "El correo electrónico ya está registrado.";

            AppUser user = new()
            {
                UserName = vm.UserName,
                Email = vm.Email,
                Name = vm.Name,
                LastName = vm.LastName,
                PhoneNumber = vm.Phone,
                ProfileImage = vm.ProfileImage,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
                return result.Errors.FirstOrDefault()?.Description ?? "Error al crear el usuario.";

            await _userManager.AddToRoleAsync(user, "User");

            // Enviar correo de confirmación
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var confirmUrl = $"https://localhost:7162/Login/ConfirmEmail?userId={user.Id}&token={encodedToken}";

            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email!,
                Subject = "Confirma tu cuenta en LinkUp",
                Body = $"<h3>Bienvenido a LinkUp</h3><p>Haz clic en el enlace para activar tu cuenta:</p><a href='{confirmUrl}'>Activar cuenta</a>"
            });

            return user.Id;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion

        #region Métodos CRUD

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var list = new List<UserViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                list.Add(new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName ?? "",
                    Email = u.Email ?? "",
                    Name = u.Name,
                    LastName = u.LastName,
                    ProfileImage = u.ProfileImage,
                    Role = roles.FirstOrDefault() ?? "User"
                });
            }

            return list;
        }

        public async Task<SaveUserViewModel?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return new SaveUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                ProfileImage = user.ProfileImage
            };
        }

        public async Task UpdateUserAsync(SaveUserViewModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id ?? "");
            if (user == null) return;

            user.Name = vm.Name;
            user.LastName = vm.LastName;
            user.PhoneNumber = vm.Phone;

            if (!string.IsNullOrEmpty(vm.Email))
                user.Email = vm.Email;
            if (!string.IsNullOrEmpty(vm.UserName))
                user.UserName = vm.UserName;
            if (!string.IsNullOrEmpty(vm.ProfileImage))
                user.ProfileImage = vm.ProfileImage;

            if (!string.IsNullOrEmpty(vm.Password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, vm.Password);
            }

            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }

        #endregion

        #region Password y Confirmación

        public async Task ForgotPasswordAsync(ForgotPasswordRequestViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email ?? "");
            if (user == null) return;

            // Desactivar usuario
            user.EmailConfirmed = false;
            await _userManager.UpdateAsync(user);

            // Generar token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var resetUrl = $"https://localhost:7162/Login/ResetPassword?token={encodedToken}&email={user.Email}";

            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email!,
                Subject = "Restablecer contraseña - LinkUp",
                Body = $"<h3>Restablecer contraseña</h3><p>Haz clic en el enlace para restablecer tu contraseña:</p><a href='{resetUrl}'>Restablecer contraseña</a>"
            });
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email ?? "");
            if (user == null) return;

            var decodedToken = Uri.UnescapeDataString(request.Token ?? "");
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password);

            if (result.Succeeded)
            {
                // Reactivar usuario
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "Usuario no encontrado.";

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            return result.Succeeded ? "Cuenta activada correctamente." : "Error al activar la cuenta.";
        }

        #endregion

        #region Métodos requeridos por interfaz

        public async Task<IEnumerable<AuthenticationResponse>> GetAllUser(bool isActive)
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new AuthenticationResponse
            {
                Id = u.Id,
                UserName = u.UserName ?? "",
                Email = u.Email ?? ""
            });
        }

        #endregion
    }
}