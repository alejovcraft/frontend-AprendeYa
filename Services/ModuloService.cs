using Frontend_AprendeYa.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Frontend_AprendeYa.Services
{
    public class ModuloService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ModuloService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método para adjuntar el Token de seguridad
        private void AñadirToken()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // GET: Obtener módulos por Curso
        public async Task<List<Modulo>> GetModulosByCursoAsync(int idCurso)
        {
            AñadirToken();
            var response = await _httpClient.GetAsync($"api/Modulo/PorCurso/{idCurso}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Modulo>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Modulo>();
        }

        // POST: Crear un nuevo módulo
        public async Task<bool> InsertarAsync(Modulo modulo)
        {
            AñadirToken();
            var json = JsonSerializer.Serialize(modulo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Modulo", content);
            return response.IsSuccessStatusCode;
        }

        // GET: Obtener un módulo por ID (para Editar)
        public async Task<Modulo> GetModuloByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Modulo/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Modulo>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        // PUT: Actualizar módulo existente
        public async Task<bool> UpdateAsync(Modulo modulo)
        {
            AñadirToken();
            var json = JsonSerializer.Serialize(modulo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/Modulo", content);
            return response.IsSuccessStatusCode;
        }

        // DELETE: Eliminar módulo
        public async Task<bool> DeleteAsync(int id)
        {
            AñadirToken();
            var response = await _httpClient.DeleteAsync($"api/Modulo/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}