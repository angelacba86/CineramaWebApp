using System.Security.Claims;
using CineramaWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineramaWebApp.Controllers
{
    [Authorize] // Requiere que el usuario esté autenticado con su sesión activa
    public class CuentaController : Controller
    {
        private readonly IFidelizacionService _fidelizacionService;

        public CuentaController(IFidelizacionService fidelizacionService)
        {
            _fidelizacionService = fidelizacionService;
        }

        // GET: /Cuenta/MiCuenta
        [HttpGet]
        public async Task<IActionResult> MiCuenta()
        {
            // Obtener el ID del usuario desde las Cookies de Autenticación
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idClaim, out int idUsuario))
            {
                return RedirectToAction("Login", "Account");
            }

            // Consultar historial de compras y puntos vía Dapper
            var historial = await _fidelizacionService.ObtenerHistorialComprasAsync(idUsuario);

            // Obtener puntos acumulados guardados en la Claim de sesión
            string puntos = User.FindFirst("Puntos")?.Value ?? "0";
            ViewBag.PuntosAcumulados = puntos;

            return Json(new
            {
                usuario = User.Identity?.Name,
                puntosAcumulados = puntos,
                compras = historial
            });
        }
    }
}