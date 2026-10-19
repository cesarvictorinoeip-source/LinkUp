namespace LinkUp.Core.Application.Dtos.Battleship.Email
{
    public class EmailRequest
    {
        public required string To { get; set; }      // A quién va el correo
        public required string Subject { get; set; } // El asunto
        public required string Body { get; set; }    // El contenido (HTML)
    }
}