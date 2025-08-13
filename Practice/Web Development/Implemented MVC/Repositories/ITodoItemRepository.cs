using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Repositories
{
    public interface ITodoItemRepository
    {
        Task<TodoItem?> GetByIdAsync(int id);
        Task<TodoItem?> GetByIdWithUserAsync(int id);
        Task<List<TodoItem>> GetAllAsync();
        Task<List<TodoItem>> GetByUserIdAsync(string userId);
        Task<PaginatedResultDto<TodoItem>> GetFilteredAsync(TodoItemFilterDto filter, string? userId = null);
        Task<TodoItem> CreateAsync(TodoItem todoItem);
        Task<TodoItem> UpdateAsync(TodoItem todoItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetTotalCountAsync(string? userId = null);
        Task<int> GetCompletedCountAsync(string? userId = null);
        Task<int> GetPendingCountAsync(string? userId = null);
        Task<List<TodoItem>> GetOverdueTasksAsync(string? userId = null);
    }
}
