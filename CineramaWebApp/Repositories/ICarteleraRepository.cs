using CineramaWebApp.Models.DTOs;

namespace CineramaWebApp.Repositories
{
    public interface ICarteleraRepository
    {
        Task<PeliculaDTO?> ObtenerDetallePeliculaAsync(int idPelicula);
        Task<IEnumerable<CarteleraDTO>> ListarCarteleraPorCineAsync(int? idCine, string? ciudad = null);
        Task<IEnumerable<FuncionDisponibleDTO>> ObtenerFuncionesPorPeliculaAsync(int idPelicula, int idCine = 0, DateTime? fecha = null);
        Task<IEnumerable<AsientoMapaDTO>> ObtenerMapaAsientosAsync(int idFuncion);
    }
}