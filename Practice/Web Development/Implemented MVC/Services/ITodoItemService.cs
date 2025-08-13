using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Services
{
    public interface ITodoItemService
    {
        Task<ApiResponseDto<TodoItemDto>> GetByIdAsync(int id, string currentUserId, bool isAdmin);
        Task<ApiResponseDto<PaginatedResultDto<TodoItemDto>>> GetFilteredAsync(TodoItemFilterDto filter, string currentUserId, bool isAdmin);
        Task<ApiResponseDto<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto, string userId);
        Task<ApiResponseDto<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto, string currentUserId, bool isAdmin);
        Task<ApiResponseDto<bool>> DeleteAsync(int id, string currentUserId, bool isAdmin);
        Task<ApiResponseDto<List<TodoItemDto>>> GetUserTodosAsync(string userId);
        Task<ApiResponseDto<object>> GetDashboardStatsAsync(string currentUserId, bool isAdmin);
    }
}
