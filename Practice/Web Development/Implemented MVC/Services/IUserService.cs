using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Services
{
    public interface IUserService
    {
        Task<ApiResponseDto<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponseDto<bool>> LogoutAsync();
        Task<ApiResponseDto<UserDto>> GetCurrentUserAsync(string userId);
        Task<ApiResponseDto<List<UserDto>>> GetAllUsersAsync();
        Task<ApiResponseDto<bool>> DeleteUserAsync(string userId);
        Task<ApiResponseDto<bool>> AssignRoleAsync(string userId, string roleName);
    }
}
