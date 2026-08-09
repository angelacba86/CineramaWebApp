using System.Data;
using CineramaWebApp.Models.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CineramaWebApp.Repositories
{
    public class CineRepository : ICineRepository
    {
        private readonly string _connectionString;

        public CineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CineramaConnection")!;
        }

        public async Task<IEnumerable<Cine>> ListarCinesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Cine>(
                "usp_ListarCines",
                commandType: CommandType.StoredProcedure);
        }
    }
}
