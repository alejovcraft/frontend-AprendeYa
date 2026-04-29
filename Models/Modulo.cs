using System.ComponentModel.DataAnnotations;

namespace Frontend_AprendeYa.Models
{
    public class Modulo
    {
        public int IdModulo { get; set; }
        public int IdCurso { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es necesaria")]
        public string Descripcion { get; set; }

        public int Orden { get; set; }
    }
}
