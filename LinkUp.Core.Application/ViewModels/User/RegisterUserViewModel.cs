using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LinkUp.Core.Application.ViewModels.User
{
    namespace LinkUp.Core.Application.ViewModels.User
    {
        public class RegisterUserViewModel
        {
            public string Name { get; set; } = string.Empty; // Asegúrate que se llame Name, no FirstName
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public required object Username { get;  set; }
            public string Password { get; set; } = string.Empty;
            public string? Phone { get; set; }
            public string? ProfileImage { get; set; } // Verifica que coincida con tu saveDto.ProfileImage
            public string? Role { get; set; }
        }
    }
}
