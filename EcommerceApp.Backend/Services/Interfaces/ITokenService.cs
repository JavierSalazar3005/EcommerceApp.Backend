using EcommerceApp.Backend.Models;

namespace EcommerceApp.Backend.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}