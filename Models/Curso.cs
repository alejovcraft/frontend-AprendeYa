using System.ComponentModel.DataAnnotations;

namespace Frontend_AprendeYa.Models
{
    public class Curso
    {
        public int IdCurso { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede pasar de 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es necesaria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debes ingresar un precio")]
        [Range(0.01, 9999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }
        public int IdNivel { get; set; }
        public int IdInstructor { get; set; }
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Debes subir una imagen")]
        public string ImagenUrl { get; set; }

        public string Estado { get; set; }
    }
}