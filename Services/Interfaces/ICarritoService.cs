
using Frontend_AprendeYa.Models;

namespace Frontend_AprendeYa.Services
{
    public interface ICarritoService
    {
        Task<RespuestaCarrito> AgregarAlCarritoAsync(int idUsuario, int idCurso);
        Task<CarritoCompras> ObtenerCarritoAsync(int idUsuario);
        Task<RespuestaCarrito> EliminarDelCarritoAsync(int idUsuario, int idDetalle);
        Task<RespuestaCarrito> PagarCarritoAsync(int idUsuario);
    }
}