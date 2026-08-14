namespace CineramaWebApp.Models.ViewModels
{
    public class MiCuentaViewModel
    {
        public string Usuario { get; set; } = string.Empty;
        public string PuntosAcumulados { get; set; } = "0";
        public List<CompraHistorialViewModel> Compras { get; set; } = new();
    }

    public class CompraHistorialViewModel
    {
        public int IdVenta { get; set; }
        public string? CodigoQR { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Pelicula { get; set; } = string.Empty;
        public string Cine { get; set; } = string.Empty;
        public DateTime Funcion { get; set; }
    }
}