using Microsoft.EntityFrameworkCore;
using Implemented_MVC.Data;
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem?> GetByIdWithUserAsync(int id)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TodoItem>> GetByUserIdAsync(string userId)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<PaginatedResultDto<TodoItem>> GetFilteredAsync(TodoItemFilterDto filter, string? userId = null)
        {
            var query = _context.TodoItems.Include(t => t.User).AsQueryable();

            // Filter by user if specified
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserId == userId);
            }

            // Filter by status
            if (!string.IsNullOrEmpty(filter.Status) && filter.Status.ToLower() != "all")
            {
                var isCompleted = filter.Status.ToLower() == "completed";
                query = query.Where(t => t.IsCompleted == isCompleted);
            }

            // Sorting
            query = filter.SortBy?.ToLower() switch
            {
                "duedate" => filter.SortOrder?.ToLower() == "asc" 
                    ? query.OrderBy(t => t.DueDate ?? DateTime.MaxValue)
                    : query.OrderByDescending(t => t.DueDate ?? DateTime.MinValue),
                "title" => filter.SortOrder?.ToLower() == "asc"
                    ? query.OrderBy(t => t.Title)
                    : query.OrderByDescending(t => t.Title),
                _ => filter.SortOrder?.ToLower() == "asc"
                    ? query.OrderBy(t => t.CreatedAt)
                    : query.OrderByDescending(t => t.CreatedAt)
            };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PaginatedResultDto<TodoItem>
            {
                Items = items,
                TotalItems = totalItems,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = totalPages,
                HasPreviousPage = filter.Page > 1,
                HasNextPage = filter.Page < totalPages
            };
        }

        public async Task<TodoItem> CreateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem todoItem)
        {
            todoItem.UpdatedAt = DateTime.UtcNow;
            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todoItem = await GetByIdAsync(id);
            if (todoItem == null) return false;

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TodoItems.AnyAsync(t => t.Id == id);
        }

        public async Task<int> GetTotalCountAsync(string? userId = null)
        {
            var query = _context.TodoItems.AsQueryable();
            if (!string.IsNullOrEmpty(userId))
                query = query.Where(t => t.UserId == userId);
            
            return await query.CountAsync();
        }

        public async Task<int> GetCompletedCountAsync(string? userId = null)
        {
            var query = _context.TodoItems.Where(t => t.IsCompleted);
            if (!string.IsNullOrEmpty(userId))
                query = query.Where(t => t.UserId == userId);
            
            return await query.CountAsync();
        }

        public async Task<int> GetPendingCountAsync(string? userId = null)
        {
            var query = _context.TodoItems.Where(t => !t.IsCompleted);
            if (!string.IsNullOrEmpty(userId))
                query = query.Where(t => t.UserId == userId);
            
            return await query.CountAsync();
        }

        public async Task<List<TodoItem>> GetOverdueTasksAsync(string? userId = null)
        {
            var today = DateTime.Today;
            var query = _context.TodoItems
                .Include(t => t.User)
                .Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value.Date < today);
            
            if (!string.IsNullOrEmpty(userId))
                query = query.Where(t => t.UserId == userId);
            
            return await query.ToListAsync();
        }
    }
}
