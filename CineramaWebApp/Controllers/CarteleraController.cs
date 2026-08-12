using CineramaWebApp.Models.ViewModels;
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
        [HttpGet]
        public async Task<IActionResult> Detalle(int id, int idCine = 0, string? ciudad = null, DateTime? fecha = null)
        {
            var pelicula = await _carteleraRepository.ObtenerDetallePeliculaAsync(id);
            if (pelicula == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new DetallePeliculaViewModel
            {
                Pelicula = pelicula,
                IdCineSeleccionado = idCine,
                CiudadSeleccionada = ciudad ?? string.Empty
            };

            // PASO 1: Si no hay cine seleccionado
            if (idCine == 0)
            {
                vm.CinesDisponibles = await _carteleraRepository.ObtenerCinesPorPeliculaAsync(id);
                return View(vm);
            }

            // PASO 2: Semana cinematográfica (Hoy hasta el próximo miércoles)
            DateTime hoy = DateTime.Today;
            int diasHastaMiercoles = ((int)DayOfWeek.Wednesday - (int)hoy.DayOfWeek + 7) % 7;

            vm.FechasDisponibles = Enumerable.Range(0, diasHastaMiercoles + 1)
                .Select(i => hoy.AddDays(i))
                .ToList();

            DateTime fechaSeleccionada = fecha ?? hoy;
            if (!vm.FechasDisponibles.Any(f => f.Date == fechaSeleccionada.Date))
            {
                fechaSeleccionada = hoy;
            }
            vm.FechaSeleccionada = fechaSeleccionada;

            vm.Funciones = await _carteleraRepository.ObtenerFuncionesPorPeliculaAsync(id, idCine, fechaSeleccionada);

            var cines = await _carteleraRepository.ObtenerCinesPorPeliculaAsync(id);
            vm.CineActual = cines.FirstOrDefault(c => c.IdCine == idCine);

            return View(vm);
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