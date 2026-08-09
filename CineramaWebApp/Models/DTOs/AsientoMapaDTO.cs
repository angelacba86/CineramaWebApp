namespace CineramaWebApp.Models.DTOs
{
    public class AsientoMapaDTO
    {
        public int IdAsiento { get; set; }
        public string Fila { get; set; } = string.Empty;
        public int Numero { get; set; }
        public bool Ocupado { get; set; }
    }
}