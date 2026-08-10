using CineramaWebApp.Repositories;

namespace CineramaWebApp.Services
{
    public class FidelizacionService : IFidelizacionService
    {
        private readonly IVentaRepository _ventaRepository;

        public FidelizacionService(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        public async Task<IEnumerable<object>> ObtenerHistorialComprasAsync(int idUsuario)
        {
            return await _ventaRepository.ObtenerHistorialComprasAsync(idUsuario);
        }
    }
}