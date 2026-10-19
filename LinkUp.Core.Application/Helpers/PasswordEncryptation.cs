using System.Security.Cryptography;
using System.Text;

namespace LinkUp.Core.Application.Helpers
{
    public static class PasswordEncryptation
    {
        public static string ComputeSha256Hash(string password)
        {
            // Creamos una instancia de SHA256
            using SHA256 sha256Hash = SHA256.Create();

            // Computamos el Hash recibiendo los bytes de la contraseña
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convertimos el array de bytes a una cadena hexadecimal
            StringBuilder sb = new();

            foreach (var item in bytes)
            {
                sb.Append(item.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}