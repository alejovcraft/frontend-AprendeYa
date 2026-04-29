using Frontend_AprendeYa.Models;

namespace Frontend_AprendeYa.Services
{
    public interface IVentaService
    {
        Task<List<ReporteVenta>> ObtenerReporteVentasAsync();
    }
}