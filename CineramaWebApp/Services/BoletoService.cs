using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CineramaWebApp.Services
{
    public class BoletoService : IBoletoService
    {
        private readonly IWebHostEnvironment _env;

        public BoletoService(IWebHostEnvironment env)
        {
            _env = env;
            // Configurar la licencia gratuita Community de QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<string> GenerarTicketPdfAsync(
            int idVenta,
            string codigoTicket,
            string cliente,
            string pelicula,
            string cine,
            string sala,
            string fechaHora,
            List<string> asientos,
            decimal montoTotal)
        {
            // 1. Carpeta destino dentro de wwwroot/boletos
            string folderPath = Path.Combine(_env.WebRootPath, "boletos");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"ticket_{idVenta}_{Guid.NewGuid():N}.pdf";
            string filePath = Path.Combine(folderPath, fileName);

            // 2. Construcción de la estructura del documento QuestPDF
            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A6); // Formato Ticket Pequeño
                    page.Margin(15);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .AlignCenter()
                        .Text("CINERAMA - BOLETO DE ENTRADA")
                        .Bold().FontSize(12).FontColor(Colors.Red.Medium);

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Spacing(5);

                        col.Item().Text($"Código Ticket: {codigoTicket}").Bold();
                        col.Item().Text($"Cliente: {cliente}");
                        col.Item().Text($"Cine: {cine}");
                        col.Item().Text($"Película: {pelicula}").Bold();
                        col.Item().Text($"Sala: {sala}");
                        col.Item().Text($"Fecha / Hora: {fechaHora}");
                        col.Item().Text($"Asientos: {string.Join(", ", asientos)}").Bold();

                        col.Item().PaddingTop(5).LineHorizontal(1);

                        col.Item().AlignRight()
                            .Text($"Total Pagado: S/ {montoTotal:F2}")
                            .Bold().FontSize(11);
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("¡Gracias por su compra! Presente este boleto en la entrada.")
                        .FontSize(8).Italic();
                });
            });

            // 3. CAMBIO CLAVE: Usar FileStream con 'using' para asegurar que el archivo SE CIERRE inmediatamente
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                doc.GeneratePdf(stream);
            }

            await Task.CompletedTask;

            // Retorna la URL relativa pública para descargar el PDF
            return $"/boletos/{fileName}";
        }
    }
}