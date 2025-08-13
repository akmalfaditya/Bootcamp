using Implemented_WebAPI.Models;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Repositories
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetAllAsync(string userId);
        Task<TodoItem?> GetByIdAsync(int id, string userId);
        Task<TodoItem> CreateAsync(TodoItem todoItem);
        Task<TodoItem> UpdateAsync(TodoItem todoItem);
        Task DeleteAsync(int id, string userId);
        Task<(List<TodoItem> Items, int TotalCount)> GetFilteredAsync(string userId, TodoItemFilterDto filter);
        Task<bool> ExistsAsync(int id, string userId);
    }
}
