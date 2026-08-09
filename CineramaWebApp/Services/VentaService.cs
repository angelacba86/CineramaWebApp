using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Repositories;

namespace CineramaWebApp.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;

        public VentaService(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        public async Task<VentaResponseDTO> ProcesarVentaAsync(VentaRequestDTO ventaDto)
        {
            if (ventaDto.ListaAsientos == null || !ventaDto.ListaAsientos.Any())
            {
                return new VentaResponseDTO
                {
                    Exito = false,
                    Mensaje = "Debe seleccionar al menos un asiento."
                };
            }

            return await _ventaRepository.FinalizarCompraAsync(ventaDto);
        }
    }
}