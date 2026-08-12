using CineramaWebApp.Models.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CineramaWebApp.Repositories
{
    public class CineRepository : ICineRepository
    {
        private readonly string _connectionString;

        public CineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CineramaConnection")!;
        }

        public async Task<IEnumerable<Cine>> ListarCinesAsync(string? ciudad = null)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Cine>(
                "sp_ListarCines",
                new { ciudad },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<string>> ListarCiudadesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<string>(
                "sp_ListarCiudades",
                commandType: CommandType.StoredProcedure);
        }
    }
}