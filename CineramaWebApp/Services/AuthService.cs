using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Models.Entities;
using CineramaWebApp.Repositories;

namespace CineramaWebApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioSesionDTO?> LoginAsync(LoginDTO loginDto)
        {
            // Para fines de la entrega usaremos la clave directa/hash proporcionado
            return await _usuarioRepository.AutenticarUsuarioAsync(loginDto.Email, loginDto.Password);
        }

        public async Task<int> RegistrarClienteAsync(string nombre, string apellido, string email, string password, string? telefono)
        {
            var usuario = new Usuario
            {
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                PasswordHash = password,
                Telefono = telefono
            };

            return await _usuarioRepository.RegistrarUsuarioClienteAsync(usuario);
        }
    }
}