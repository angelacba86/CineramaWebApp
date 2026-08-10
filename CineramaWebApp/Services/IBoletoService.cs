namespace CineramaWebApp.Services
{
    public interface IBoletoService
    {
        Task<string> GenerarTicketPdfAsync(int idVenta, string codigoTicket, string cliente, string pelicula, string cine, string sala, string fechaHora, List<string> asientos, decimal montoTotal);
    }
}