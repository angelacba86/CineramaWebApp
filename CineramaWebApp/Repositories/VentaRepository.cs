using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CineramaWebApp.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly string _connectionString;

        public VentaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CineramaConnection")!;
        }

        public async Task<IEnumerable<object>> ObtenerHistorialComprasAsync(int idUsuario)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync(
                "sp_ObtenerHistorialCompras",
                new { idUsuario },
                commandType: CommandType.StoredProcedure);
        }
    }
}
