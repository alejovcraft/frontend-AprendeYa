
using Frontend_AprendeYa.Models;

namespace Frontend_AprendeYa.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly HttpClient _httpClient;

        public CarritoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RespuestaCarrito> AgregarAlCarritoAsync(int idUsuario, int idCurso)
        {
            try
            {
                // Hacemos el POST (sin body, todo va en la URL)
                var response = await _httpClient.PostAsync($"api/Carrito/Agregar/{idUsuario}/Curso/{idCurso}", null);

                // Leemos la respuesta (ya sea 200 OK o 400 Bad Request, el Backend manda el mismo objeto)
                return await response.Content.ReadFromJsonAsync<RespuestaCarrito>();
            }
            catch (Exception ex)
            {
                return new RespuestaCarrito { Exito = false, Mensaje = "Error de conexión con el servidor: " + ex.Message };
            }
        }
        public async Task<CarritoCompras> ObtenerCarritoAsync(int idUsuario)
        {
            // Sin try-catch: si algo falla, queremos que el sistema nos grite en la cara cuál es el error
            var response = await _httpClient.GetFromJsonAsync<CarritoCompras>($"api/Carrito/{idUsuario}");
            return response ?? new CarritoCompras();
        }
        public async Task<RespuestaCarrito> EliminarDelCarritoAsync(int idUsuario, int idDetalle)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Carrito/Eliminar/{idUsuario}/Detalle/{idDetalle}");
                return await response.Content.ReadFromJsonAsync<RespuestaCarrito>();
            }
            catch (Exception ex)
            {
                return new RespuestaCarrito { Exito = false, Mensaje = "Error de conexión: " + ex.Message };
            }
        }
        public async Task<RespuestaCarrito> PagarCarritoAsync(int idUsuario)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/Carrito/Pagar/{idUsuario}", null);
                return await response.Content.ReadFromJsonAsync<RespuestaCarrito>();
            }
            catch (Exception ex)
            {
                return new RespuestaCarrito { Exito = false, Mensaje = "Error de conexión: " + ex.Message };
            }
        }
    }
}