using Frontend_AprendeYa.Models;

namespace Frontend_AprendeYa.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioAdmin>> ObtenerUsuariosAsync();
        Task<UsuarioAdmin> ObtenerUsuarioPorIdAsync(int id);
        Task<List<CursoMatriculado>> ObtenerCursosDeAlumnoAsync(int idUsuario);
        Task<bool> CrearUsuarioAsync(UsuarioAdmin usuario);
        Task<bool> ActualizarUsuarioAsync(UsuarioAdmin usuario);
        Task<bool> EliminarUsuarioAsync(int id);

        Task<bool> MatricularAlumnoAsync(int idUsuario, int idCurso);
    }
}