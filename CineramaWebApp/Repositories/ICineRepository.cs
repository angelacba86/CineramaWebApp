// Repositories/ICineRepository.cs
using CineramaWebApp.Models.Entities;

namespace CineramaWebApp.Repositories
{
    public interface ICineRepository
    {
        Task<IEnumerable<Cine>> ListarCinesAsync(string? ciudad = null);
        Task<IEnumerable<string>> ListarCiudadesAsync();
    }
}