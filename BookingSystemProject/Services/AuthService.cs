using BookingSystemProject.DTOs;
using BookingSystemProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BookingSystemProject.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingSystemProject.Services;

public class AuthService : IAuthService
{
    private readonly RestaurantDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(RestaurantDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var admin = await _context.Administrators
            .FirstOrDefaultAsync(a => a.Username == request.Username);

        if (admin == null)
            return null;

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
            return null;

        // Generate JWT token
        var token = GenerateJwtToken(admin);
        var expiresAt = DateTime.UtcNow.AddMinutes(
            int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60"));

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt
        };
    }

    public async Task SeedAdminIfNeededAsync()
    {
        if (!await _context.Administrators.AnyAsync())
        {
            var admin = new Administrator
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
            };

            _context.Administrators.Add(admin);
            await _context.SaveChangesAsync();
        }
    }

    private string GenerateJwtToken(Administrator admin)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim(ClaimTypes.Name, admin.Username),
            new Claim(ClaimTypes.Role, "Administrator")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
