using Frontend_AprendeYa.Models;
using System.Text;
using System.Text.Json;

namespace Frontend_AprendeYa.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        // Inyectamos HttpClient
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsuarioSesion> LoginAsync(LoginRequest request)
        {
            // 1. Convertimos los datos del login a JSON
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 2. Hacemos el POST a la URL de tu API
            var response = await _httpClient.PostAsync("api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                // 3. Si es exitoso (200 OK), leemos el JSON de respuesta y extraemos el Token
                var responseString = await response.Content.ReadAsStringAsync();

                // Usamos esta opción para que ignore mayúsculas/minúsculas al leer el JSON
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var usuario = JsonSerializer.Deserialize<UsuarioSesion>(responseString, options);

                return usuario;
            }

            // Si falla (Ej. 401 Unauthorized), devolvemos nulo
            return null;
        }
    }
}