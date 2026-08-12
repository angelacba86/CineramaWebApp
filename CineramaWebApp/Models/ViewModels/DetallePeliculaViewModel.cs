using CineramaWebApp.Models.DTOs;
using CineramaWebApp.Models.Entities;

namespace CineramaWebApp.Models.ViewModels
{
    public class DetallePeliculaViewModel
    {
        public PeliculaDTO Pelicula { get; set; } = default!;
        public int IdCineSeleccionado { get; set; }
        public string CiudadSeleccionada { get; set; } = string.Empty;
        public DateTime FechaSeleccionada { get; set; } = DateTime.Today;

        // Datos Paso 1
        public IEnumerable<Cine> CinesDisponibles { get; set; } = Enumerable.Empty<Cine>();

        // Datos Paso 2
        public Cine? CineActual { get; set; }
        public IEnumerable<DateTime> FechasDisponibles { get; set; } = Enumerable.Empty<DateTime>();
        public IEnumerable<FuncionDisponibleDTO> Funciones { get; set; } = Enumerable.Empty<FuncionDisponibleDTO>();
    }
}