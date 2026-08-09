// Repositories/ICarteleraRepository.cs
using CineramaWebApp.Models.DTOs;

namespace CineramaWebApp.Repositories
{
    public interface ICarteleraRepository
    {
        Task<IEnumerable<CarteleraDTO>> ListarCarteleraPorCineAsync(int? idCine, DateTime? fecha = null);
        Task<IEnumerable<FuncionDisponibleDTO>> ObtenerFuncionesPorPeliculaAsync(int idPelicula, int idCine, DateTime? fecha = null);
        Task<IEnumerable<AsientoMapaDTO>> ObtenerMapaAsientosAsync(int idFuncion);
    }
}