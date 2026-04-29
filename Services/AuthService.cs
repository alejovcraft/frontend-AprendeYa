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
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Hacemos la petición al backend
                var response = await _httpClient.PostAsync("api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // ESTO ES LA MAGIA: Le dice a C# que ignore las mayúsculas y minúsculas al leer el JSON de Postman
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var usuario = JsonSerializer.Deserialize<UsuarioSesion>(jsonString, options);

                    return usuario;
                }

                return null;
            }
            catch (Exception ex)
            {
                // Si el frontend no logra alcanzar al backend (ej. API apagada), caerá aquí
                Console.WriteLine("Error conectando a la API: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> RegistrarAsync(RegistroRequest request)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/registro", content);

            return response.IsSuccessStatusCode;
        }
    }
}