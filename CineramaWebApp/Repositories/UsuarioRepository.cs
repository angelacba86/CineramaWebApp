using System.Data;
using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Models.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CineramaWebApp.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CineramaConnection")!;
        }

        public async Task<UsuarioSesionDTO?> AutenticarUsuarioAsync(string email, string passwordHash)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<UsuarioSesionDTO>(
                "sp_AutenticarUsuario",
                new { email, passwordHash },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> RegistrarUsuarioClienteAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "sp_RegistrarUsuarioCliente",
                new
                {
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Email,
                    usuario.PasswordHash,
                    usuario.Telefono
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
