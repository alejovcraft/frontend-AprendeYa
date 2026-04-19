
using Frontend_AprendeYa.Models;

namespace Frontend_AprendeYa.Interfaces
{
    public interface ICursoService
    {
        List<Curso> GetCursos();
        Curso GetCursoById(int idCurso);
        bool InsertCurso(Curso curso);
        bool UpdateCurso(Curso curso);
        bool DeleteCurso(int idCurso);

        //Filtro
        Task<IEnumerable<Curso>> FiltrarCursos(int? nivel, int? categoria);
    }
}
