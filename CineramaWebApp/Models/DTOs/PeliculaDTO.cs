namespace CineramaWebApp.Models.DTOs
{
    public class PeliculaDTO
{
    public int IdPelicula { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Sinopsis { get; set; } = string.Empty;
    public int DuracionMinutos { get; set; }
    public string Clasificacion { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
}
}