using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Models.Entities;
namespace CineramaWebApp.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UsuarioSesionDTO?> AutenticarUsuarioAsync(string email, string passwordHash);
        Task<int> RegistrarUsuarioClienteAsync(Usuario usuario);
        Task<int> ObtenerPuntosUsuarioAsync(int idUsuario);
    }
}
