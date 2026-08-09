using System.Security.Claims;
using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CineramaWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return Content("Backend de Autenticación de Cinerama activo y listo.");
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return Json(new { exito = false, mensaje = "Datos de ingreso inválidos." });

            var usuario = await _authService.LoginAsync(dto);

            if (usuario == null)
                return Json(new { exito = false, mensaje = "Correo o contraseña incorrectos." });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario),
                new Claim("Puntos", usuario.PuntosAcumulados.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return Json(new { exito = true, mensaje = "Bienvenido a Cinerama", usuario });
        }

        // POST: /Account/Registrar
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] RegistroDTO dto)
        {
            try
            {
                int idNuevo = await _authService.RegistrarClienteAsync(dto.Nombre, dto.Apellido, dto.Email, dto.Password, dto.Telefono);
                return Json(new { exito = true, mensaje = "Registro completado exitosamente.", idUsuario = idNuevo });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error al registrar: " + ex.Message });
            }
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(new { exito = true });
        }
    }

    public class RegistroDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}