using CineramaWebApp.Models.DTOs;

namespace CineramaWebApp.Services
{
    public interface IAuthService
    {
        Task<UsuarioSesionDTO?> LoginAsync(LoginDTO loginDto);
        Task<int> RegistrarClienteAsync(string nombre, string apellido, string email, string password, string? telefono, DateTime fechaNacimiento);
    }
}