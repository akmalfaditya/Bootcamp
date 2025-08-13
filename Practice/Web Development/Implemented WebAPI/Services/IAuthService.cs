using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Services
{
    public interface IAuthService
    {
        Task<ApiResponseDto<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponseDto<UserDto>> GetCurrentUserAsync(string userId);
    }
}
