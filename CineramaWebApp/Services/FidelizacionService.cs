using CineramaWebApp.Repositories;

namespace CineramaWebApp.Services
{
    public class FidelizacionService : IFidelizacionService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public FidelizacionService(IVentaRepository ventaRepository, IUsuarioRepository usuarioRepository)
        {
            _ventaRepository = ventaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<object>> ObtenerHistorialComprasAsync(int idUsuario)
        {
            return await _ventaRepository.ObtenerHistorialComprasAsync(idUsuario);
        }

        public async Task<int> ObtenerPuntosUsuarioAsync(int idUsuario)
        {
            return await _usuarioRepository.ObtenerPuntosUsuarioAsync(idUsuario);
        }
    }
}