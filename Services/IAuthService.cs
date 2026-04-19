using Frontend_AprendeYa.Models;

namespace Frontend_AprendeYa.Services
{
    public interface IAuthService
    {
        Task<UsuarioSesion> LoginAsync(LoginRequest request);
    }
}