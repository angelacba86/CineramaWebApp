namespace CineramaWebApp.Models.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string TipoUsuario { get; set; } = "REGISTRADO";
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}
