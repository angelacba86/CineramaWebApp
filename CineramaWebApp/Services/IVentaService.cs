using CineramaWebApp.Models.DTOs;

namespace CineramaWebApp.Services
{
    public interface IVentaService
    {
        Task<VentaResponseDTO> ProcesarVentaAsync(VentaRequestDTO ventaDto);
    }
}