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
        public async Task<IActionResult> Index(int? idCine, DateTime? fecha)
        {
            // Listar cines para el selector superior
            var cines = await _cineRepository.ListarCinesAsync();
            ViewBag.Cines = cines;
            ViewBag.IdCineSeleccionado = idCine;

            // Listar cartelera según filtros
            var cartelera = await _carteleraRepository.ListarCarteleraPorCineAsync(idCine, fecha);
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
    }
}