namespace CineramaWebApp.Models.DTOs
{
    public class VentaRequestDTO
    {
        public int IdFuncion { get; set; }
        public int IdUsuario { get; set; }
        public List<int> ListaAsientos { get; set; } = new List<int>();
    }

    public class VentaResponseDTO
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdVenta { get; set; }
        public string CodigoTicket { get; set; } = string.Empty;
        public string UrlPDF { get; set; } = string.Empty;
    }
}