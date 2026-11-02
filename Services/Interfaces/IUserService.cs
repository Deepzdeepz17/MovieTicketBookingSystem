
using MovieTicketBookingSystem.DTOs.Auth;

namespace MovieTicketBookingSystem.Services
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
