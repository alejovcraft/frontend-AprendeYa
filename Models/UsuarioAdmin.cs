namespace Frontend_AprendeYa.Models // Ajusta el namespace si es distinto
{
    public class UsuarioAdmin
    {
        public int IdUsuario { get; set; }
        public int? IdPersona { get; set; }
        public int? IdRol { get; set; }
        public string? Username { get; set; }
        public string? ContrasenaLiteral { get; set; }
        public bool Estado { get; set; }

        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? NombreRol { get; set; }
    }
}