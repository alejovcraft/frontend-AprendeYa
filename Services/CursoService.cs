using Frontend_AprendeYa.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Frontend_AprendeYa.Services
{
    public class CursoService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CursoService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método para obtener el Token guardado por tu compañero
        private void AñadirToken()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Curso>> GetCursosAsync()
        {
            var response = await _httpClient.GetAsync("api/Curso");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Curso>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Curso>();
        }

        public async Task<Curso> GetCursoByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Curso/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Curso>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<bool> InsertarAsync(Curso curso)
        {
            AñadirToken();
            var json = JsonSerializer.Serialize(curso);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Curso", content);
            return response.IsSuccessStatusCode;
        }

        internal async Task<bool> UpdateAsync(Curso curso)
        {
            AñadirToken();

            var json = JsonSerializer.Serialize(curso);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Llama a la API usando PUT
            var response = await _httpClient.PutAsync("api/Curso", content);

            return response.IsSuccessStatusCode;
        }

        internal async Task<bool> DeleteAsync(int id)
        {
            AñadirToken();

            // Llama a la API enviando el ID en la URL
            var response = await _httpClient.DeleteAsync($"api/Curso/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
