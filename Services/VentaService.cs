using Frontend_AprendeYa.Models;

namespace Frontend_AprendeYa.Services
{
    public class VentaService : IVentaService
    {
        private readonly HttpClient _httpClient;

        public VentaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ReporteVenta>> ObtenerReporteVentasAsync()
        {
            try
            {
                // Apuntamos a la ruta exacta de tu VentaController en el Backend
                var response = await _httpClient.GetFromJsonAsync<List<ReporteVenta>>("api/Venta/Reporte");
                return response ?? new List<ReporteVenta>();
            }
            catch
            {
                return new List<ReporteVenta>();
            }
        }
    }
}