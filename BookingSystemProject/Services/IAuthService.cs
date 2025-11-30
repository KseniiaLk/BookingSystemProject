using BookingSystemProject.DTOs;

namespace BookingSystemProject.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task SeedAdminIfNeededAsync();
}
