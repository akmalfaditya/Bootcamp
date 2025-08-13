using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Services
{
    public interface ITodoItemService
    {
        Task<ApiResponseDto<List<TodoItemDto>>> GetAllAsync(string userId);
        Task<ApiResponseDto<TodoItemDto>> GetByIdAsync(int id, string userId);
        Task<ApiResponseDto<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto, string userId);
        Task<ApiResponseDto<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto, string userId);
        Task<ApiResponseDto<bool>> DeleteAsync(int id, string userId);
        Task<ApiResponseDto<PaginatedResultDto<TodoItemDto>>> GetFilteredAsync(string userId, TodoItemFilterDto filter);
        Task<ApiResponseDto<TodoStatsDto>> GetStatsAsync(string userId);
    }
}
