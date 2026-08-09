using CineramaWebApp.Models.DTOs;

namespace CineramaWebApp.Repositories
{
    public interface IVentaRepository
    {
        Task<VentaResponseDTO> FinalizarCompraAsync(VentaRequestDTO ventaDto);
        Task<IEnumerable<object>> ObtenerHistorialComprasAsync(int idUsuario);
    }
}