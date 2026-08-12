using CineramaWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CineramaWebApp.Controllers
{
    public class CarteleraController : Controller
    {
        private readonly ICineRepository _cineRepository;
        private readonly ICarteleraRepository _carteleraRepository;

        public CarteleraController(ICineRepository cineRepository, ICarteleraRepository carteleraRepository)
        {
            _cineRepository = cineRepository;
            _carteleraRepository = carteleraRepository;
        }

        // GET: /Cartelera/Index
        [HttpGet]
        public async Task<IActionResult> Index(int? idCine, string? ciudad)
        {
            // 1. Cargar la lista completa de ciudades
            var ciudades = await _cineRepository.ListarCiudadesAsync();

            // 2. Cargar SOLO los cines de la ciudad seleccionada (si ciudad es null o vacio, trae todos)
            var cines = await _cineRepository.ListarCinesAsync(ciudad);

            ViewBag.Ciudades = ciudades;
            ViewBag.Cines = cines;
            ViewBag.CiudadSeleccionada = ciudad;
            ViewBag.IdCineSeleccionado = idCine;

            // 3. Cargar las películas que coincidan con la búsqueda
            var cartelera = await _carteleraRepository.ListarCarteleraPorCineAsync(idCine, ciudad);
            return View(cartelera);
        }
        // GET: /Cartelera/Detalle/1?idCine=2&ciudad=Lima&fecha=2026-08-12
        [HttpGet]
        public async Task<IActionResult> Detalle(int id, int idCine = 0, string? ciudad = null, DateTime? fecha = null)
        {
            var pelicula = await _carteleraRepository.ObtenerDetallePeliculaAsync(id);
            if (pelicula == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.IdCineSeleccionado = idCine;
            ViewBag.CiudadSeleccionada = ciudad;

            // --- PASO 1: SELECCIONAR CINE (Si no se especificó un cine) ---
            if (idCine == 0)
            {
                var cinesDisponibles = await _carteleraRepository.ObtenerCinesPorPeliculaAsync(id);
                ViewBag.CinesDisponibles = cinesDisponibles;
                return View(pelicula);
            }

            // --- PASO 2: CÁLCULO DINÁMICO DE FECHAS (HOY HASTA EL PRÓXIMO MIÉRCOLES) ---
            DateTime hoy = DateTime.Today;

            // Calculamos cuántos días faltan desde hoy hasta el próximo miércoles
            int diasHastaMiercoles = ((int)DayOfWeek.Wednesday - (int)hoy.DayOfWeek + 7) % 7;

            // Generamos las fechas desde hoy hasta el miércoles de la semana cinematográfica actual
            var fechasDisponibles = Enumerable.Range(0, diasHastaMiercoles + 1)
                .Select(i => hoy.AddDays(i))
                .ToList();

            // Validamos que la fecha seleccionada esté dentro del rango permitido; si no, usaremos 'hoy'
            DateTime fechaSeleccionada = fecha ?? hoy;
            if (!fechasDisponibles.Any(f => f.Date == fechaSeleccionada.Date))
            {
                fechaSeleccionada = hoy;
            }

            // Obtener funciones del cine y fecha seleccionada
            var funciones = await _carteleraRepository.ObtenerFuncionesPorPeliculaAsync(id, idCine, fechaSeleccionada);
            var cines = await _carteleraRepository.ObtenerCinesPorPeliculaAsync(id);
            var cineActual = cines.FirstOrDefault(c => c.IdCine == idCine);

            ViewBag.CineActual = cineActual;
            ViewBag.Funciones = funciones;
            ViewBag.FechasDisponibles = fechasDisponibles;
            ViewBag.FechaSeleccionada = fechaSeleccionada;

            return View(pelicula);
        }

        // GET: /Cartelera/ObtenerCinesPorCiudad?ciudad=Lima
        [HttpGet]
        public async Task<IActionResult> ObtenerCinesPorCiudad(string? ciudad)
        {
            var cines = await _cineRepository.ListarCinesAsync(ciudad);
            return Json(cines);
        }


    }
}