using Microsoft.AspNetCore.Identity;
using LinkUp.Core.Domain.Entities; // ✅
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace LinkUp.Infrastructure.Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser defaultUser = new()
            {
                Name = "Ramon",
                LastName = "Santana",
                Email = "admin@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "admin"
            };

            // Verificamos si el usuario ya existe por Email para evitar duplicados
            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                // Importante: La contraseña debe cumplir con los requisitos de Identity 
                // (Por defecto: 6 caracteres, Mayúscula, Número y Símbolo)
                // Si te da error al crear, usa una clave como "123Pa$$word!"
                var result = await userManager.CreateAsync(defaultUser, "123Pa$$word!");

                if (result.Succeeded)
                {
                    
                }
            }
        }
    }
}