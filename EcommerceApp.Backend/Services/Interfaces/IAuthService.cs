using EcommerceApp.Backend.Dtos.Auth;

namespace EcommerceApp.Backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}