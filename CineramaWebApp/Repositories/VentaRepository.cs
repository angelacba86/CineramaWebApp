using System.Data;
using CineramaWebApp.Models.DTOs;
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

        public async Task<VentaResponseDTO> FinalizarCompraAsync(VentaRequestDTO ventaDto)
        {
            using var connection = new SqlConnection(_connectionString);

            var asientosJson = System.Text.Json.JsonSerializer.Serialize(ventaDto.ListaAsientos);
            var urlPDF = $"/boletos/ticket_{Guid.NewGuid():N}.pdf";

            var parametros = new DynamicParameters();
            parametros.Add("@idFuncion", ventaDto.IdFuncion);
            parametros.Add("@idUsuario", ventaDto.IdUsuario);
            parametros.Add("@asientosJson", asientosJson);
            parametros.Add("@urlPDF", urlPDF);
            parametros.Add("@idVentaResultado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                var resultado = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_FinalizarCompra",
                    parametros,
                    commandType: CommandType.StoredProcedure);

                return new VentaResponseDTO
                {
                    Exito = true,
                    Mensaje = "Compra procesada exitosamente.",
                    IdVenta = resultado?.idVenta ?? 0,
                    CodigoTicket = resultado?.codigoTicket ?? "",
                    UrlPDF = resultado?.urlPDF ?? ""
                };
            }
            catch (Exception ex)
            {
                return new VentaResponseDTO
                {
                    Exito = false,
                    Mensaje = ex.Message
                };
            }
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