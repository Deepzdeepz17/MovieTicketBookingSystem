
using Microsoft.AspNetCore.Identity;
using MovieTicketBookingSystem.DTOs.Auth;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(
            RegisterRequestDto request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            var existingUser =
                await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "Email already registered.");
            }

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = email,
                Role = "Customer"
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user, request.Password);

            await _userRepository.AddAsync(user);

            string token = _jwtService.GenerateToken(
                user.UserId,
                user.Email,
                user.Role);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(
            LoginRequestDto request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            string token = _jwtService.GenerateToken(
                user.UserId,
                user.Email,
                user.Role);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }
    }
}
