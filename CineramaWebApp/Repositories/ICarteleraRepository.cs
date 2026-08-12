using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Models.Entities;

namespace CineramaWebApp.Repositories
{
    public interface ICarteleraRepository
    {
        Task<IEnumerable<CarteleraDTO>> ListarCarteleraPorCineAsync(int? idCine, string? ciudad = null);
        Task<IEnumerable<Cine>> ObtenerCinesPorPeliculaAsync(int idPelicula);
        Task<PeliculaDTO?> ObtenerDetallePeliculaAsync(int idPelicula);
        Task<IEnumerable<FuncionDisponibleDTO>> ObtenerFuncionesPorPeliculaAsync(int idPelicula, int idCine = 0, DateTime? fecha = null);
        Task<IEnumerable<AsientoMapaDTO>> ObtenerMapaAsientosAsync(int idFuncion);
    }
}