using System.Data;
using System.Text.RegularExpressions;
using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Models.Entities;
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

        public async Task<IEnumerable<Cine>> ObtenerCinesPorPeliculaAsync(int idPelicula)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Cine>(
                "sp_ObtenerCinesPorPelicula",
                new { idPelicula },
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

            var filasCrudas = await connection.QueryAsync<dynamic>(
                "sp_ObtenerMapaAsientos",
                new { idFuncion },
                commandType: CommandType.StoredProcedure);

            var resultado = new List<AsientoMapaDTO>();
            var regex = new Regex(@"^([A-Za-z]+)(\d+)$");

            foreach (var fila in filasCrudas)
            {
                string codigo = fila.codigo?.ToString() ?? "";
                var match = regex.Match(codigo);

                resultado.Add(new AsientoMapaDTO
                {
                    IdAsiento = (int)fila.idAsiento,
                    Fila = match.Success ? match.Groups[1].Value : "?",
                    Numero = match.Success ? int.Parse(match.Groups[2].Value) : 0,
                    Ocupado = Convert.ToBoolean(fila.ocupado)
                });
            }

            return resultado
                .OrderBy(a => a.Fila)
                .ThenBy(a => a.Numero)
                .ToList();
        }
    }
}