using EcommerceApp.Backend.Data;
using EcommerceApp.Backend.Dtos.Auth;
using EcommerceApp.Backend.Models;
using EcommerceApp.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Backend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already registered.");

            var user = new User
            {
                Email = request.Email,
                Role = request.Role
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _tokenService.CreateToken(user);

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Role = user.Role,
                UserId = user.Id
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new Exception("Invalid credentials.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials.");

            var token = _tokenService.CreateToken(user);

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Role = user.Role,
                UserId = user.Id
            };
        }
    }
}
