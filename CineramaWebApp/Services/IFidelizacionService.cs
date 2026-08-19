namespace CineramaWebApp.Services
{
    public interface IFidelizacionService
    {
        Task<IEnumerable<object>> ObtenerHistorialComprasAsync(int idUsuario);
        Task<int> ObtenerPuntosUsuarioAsync(int idUsuario);
    }
}