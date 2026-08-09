namespace CineramaWebApp.Repositories
{
    public interface IVentaRepository
    {
        Task<IEnumerable<object>> ObtenerHistorialComprasAsync(int idUsuario);
    }
}