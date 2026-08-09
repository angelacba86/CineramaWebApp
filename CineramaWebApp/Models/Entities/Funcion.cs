namespace CineramaWebApp.Models.Entities
{
    public class Funcion
    {
        public int IdFuncion { get; set; }
        public int IdPelicula { get; set; }
        public int IdSala { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
