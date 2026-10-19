namespace LinkUp.Core.Application.Dtos.Email
{
    public class EmailRequestDto
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        // Si en tu servicio usas HtmlBody, cámbialo aquí también
        public string? HtmlBody { get; set; }
    }
}