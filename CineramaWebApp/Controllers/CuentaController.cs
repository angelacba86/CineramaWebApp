using System.Security.Claims;
using CineramaWebApp.Models.ViewModels;
using CineramaWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineramaWebApp.Controllers
{
    [Authorize]
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
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idClaim, out int idUsuario))
            {
                return RedirectToAction("Login", "Account");
            }

            var historialRaw = await _fidelizacionService.ObtenerHistorialComprasAsync(idUsuario);
            int puntosDB = await _fidelizacionService.ObtenerPuntosUsuarioAsync(idUsuario);
            string puntos = puntosDB.ToString();

            var compras = new List<CompraHistorialViewModel>();
            foreach (dynamic item in historialRaw)
            {
                compras.Add(new CompraHistorialViewModel
                {
                    IdVenta = item.idVenta,
                    CodigoQR = item.codigoQR,
                    Fecha = item.fecha,
                    Monto = item.monto,
                    Pelicula = item.pelicula,
                    Cine = item.cine,
                    Funcion = item.funcion
                });
            }

            var modelo = new MiCuentaViewModel
            {
                Usuario = User.Identity?.Name ?? "Usuario",
                PuntosAcumulados = puntos,
                Compras = compras
            };

            return View(modelo);
        }
    }
}