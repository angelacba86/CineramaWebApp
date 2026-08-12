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

        // GET: /Cartelera/Detalle/1
        [HttpGet]
        public async Task<IActionResult> Detalle(int id, int idCine = 0)
        {
            var pelicula = await _carteleraRepository.ObtenerDetallePeliculaAsync(id);
            if (pelicula == null)
            {
                return RedirectToAction("Index");
            }

            var funciones = await _carteleraRepository.ObtenerFuncionesPorPeliculaAsync(id, idCine);
            ViewBag.Funciones = funciones;

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