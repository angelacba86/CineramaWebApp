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

        public async Task<IEnumerable<CarteleraDTO>> ListarCarteleraPorCineAsync(int? idCine, DateTime? fecha = null)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<CarteleraDTO>(
                "sp_ListarCarteleraPorCine",
                new { idCine, fecha },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<FuncionDisponibleDTO>> ObtenerFuncionesPorPeliculaAsync(int idPelicula, int idCine, DateTime? fecha = null)
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
