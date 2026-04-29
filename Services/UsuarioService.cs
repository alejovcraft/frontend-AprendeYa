using Frontend_AprendeYa.Models;
using System.Net.Http.Json; // Importante para usar los métodos AsJsonAsync

namespace Frontend_AprendeYa.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _httpClient;

        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UsuarioAdmin>> ObtenerUsuariosAsync()
        {
            try
            {
                // Hace un GET a nuestra API
                var response = await _httpClient.GetFromJsonAsync<List<UsuarioAdmin>>("api/Usuario");
                return response ?? new List<UsuarioAdmin>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
                return new List<UsuarioAdmin>();
            }
        }

        public async Task<UsuarioAdmin> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<UsuarioAdmin>($"api/Usuario/{id}");
        }

        public async Task<bool> CrearUsuarioAsync(UsuarioAdmin usuario)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Usuario", usuario);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarUsuarioAsync(UsuarioAdmin usuario)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Usuario/{usuario.IdUsuario}", usuario);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            // Recuerda que en el Backend esto es una baja lógica (Update estado = 0)
            var response = await _httpClient.DeleteAsync($"api/Usuario/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> MatricularAlumnoAsync(int idUsuario, int idCurso)
        {
            // Hacemos el POST a la ruta que acabamos de probar en Postman
            var response = await _httpClient.PostAsync($"api/Usuario/{idUsuario}/Matricular/{idCurso}", null);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<CursoMatriculado>> ObtenerCursosDeAlumnoAsync(int idUsuario)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CursoMatriculado>>($"api/Usuario/{idUsuario}/MisCursos");
            return response ?? new List<CursoMatriculado>();
        }
    }
}