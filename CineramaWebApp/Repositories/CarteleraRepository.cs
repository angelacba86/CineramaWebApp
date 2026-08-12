using System.Data;
using CineramaWebApp.Models.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CineramaWebApp.Repositories
{
    public class CarteleraRepository : ICarteleraRepository
    {
        private readonly string _connectionString;

        public CarteleraRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CineramaConnection")!;
        }

        public async Task<IEnumerable<CarteleraDTO>> ListarCarteleraPorCineAsync(int? idCine, string? ciudad = null)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<CarteleraDTO>(
                "sp_ListarCarteleraPorCine",
                new { idCine, ciudad },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<PeliculaDTO?> ObtenerDetallePeliculaAsync(int idPelicula)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<PeliculaDTO>(
                "sp_ObtenerDetallePelicula",
                new { idPelicula },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<FuncionDisponibleDTO>> ObtenerFuncionesPorPeliculaAsync(int idPelicula, int idCine = 0, DateTime? fecha = null)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<FuncionDisponibleDTO>(
                "usp_ObtenerFuncionesPorPelicula",
                new { idPelicula, idCine, fecha },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AsientoMapaDTO>> ObtenerMapaAsientosAsync(int idFuncion)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<AsientoMapaDTO>(
                "sp_ObtenerMapaAsientos",
                new { idFuncion },
                commandType: CommandType.StoredProcedure);
        }
    }
}