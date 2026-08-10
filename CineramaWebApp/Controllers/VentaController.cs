using System.Security.Claims;
using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Repositories;
using CineramaWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineramaWebApp.Controllers
{
    public class VentaController : Controller
    {
        private readonly IVentaService _ventaService;
        private readonly IBoletoService _boletoService;
        private readonly ICarteleraRepository _carteleraRepository;

        public VentaController(
            IVentaService ventaService,
            IBoletoService boletoService,
            ICarteleraRepository carteleraRepository)
        {
            _ventaService = ventaService;
            _boletoService = boletoService;
            _carteleraRepository = carteleraRepository;
        }

        // GET: /Venta/MapaAsientos?idFuncion=1
        [HttpGet]
        public async Task<IActionResult> MapaAsientos(int idFuncion)
        {
            var asientos = await _carteleraRepository.ObtenerMapaAsientosAsync(idFuncion);
            return Json(asientos);
        }

        // POST: /Venta/ProcesarCompra
        [HttpPost]
        public async Task<IActionResult> ProcesarCompra([FromBody] VentaRequestDTO dto)
        {
            if (dto == null || dto.ListaAsientos == null || !dto.ListaAsientos.Any())
            {
                return Json(new { exito = false, mensaje = "Debe seleccionar al menos un asiento válido." });
            }

            // Si el usuario está autenticado, tomamos su ID real de la cookie de sesión
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(idClaim, out int idUsuarioSesion))
                {
                    dto.IdUsuario = idUsuarioSesion;
                }
            }
            else if (dto.IdUsuario <= 0)
            {
                // Usuario invitado / anónimo (ID predeterminado en BD para compras rápidas)
                dto.IdUsuario = 1;
            }

            // 1. Procesar la transacción en la Base de Datos (usp_FinalizarCompra)
            var resultadoVenta = await _ventaService.ProcesarVentaAsync(dto);

            if (!resultadoVenta.Exito)
            {
                return Json(new { exito = false, mensaje = resultadoVenta.Mensaje });
            }

            // 2. Generar el Ticket PDF físico con QuestPDF
            try
            {
                string clienteNombre = User.Identity?.Name ?? "Cliente Invitado";
                string listaAsientosTexto = string.Join(", ", dto.ListaAsientos);

                string urlPdfGenerado = await _boletoService.GenerarTicketPdfAsync(
                    idVenta: resultadoVenta.IdVenta,
                    codigoTicket: resultadoVenta.CodigoTicket,
                    cliente: clienteNombre,
                    pelicula: "Película Seleccionada",
                    cine: "Cinerama",
                    sala: "Sala 1",
                    fechaHora: DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    asientos: dto.ListaAsientos.Select(a => $"A-{a}").ToList(),
                    montoTotal: dto.ListaAsientos.Count * 15.00m
                );

                resultadoVenta.UrlPDF = urlPdfGenerado;
            }
            catch (Exception ex)
            {
                // Si falla el PDF, la venta en BD sigue siendo válida
                Console.WriteLine($"Advertencia al generar PDF: {ex.Message}");
            }

            return Json(new
            {
                exito = true,
                mensaje = "¡Compra realizada con éxito!",
                idVenta = resultadoVenta.IdVenta,
                codigoTicket = resultadoVenta.CodigoTicket,
                urlPdf = resultadoVenta.UrlPDF
            });
        }
    }
}