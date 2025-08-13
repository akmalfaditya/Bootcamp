using Microsoft.EntityFrameworkCore;
using Implemented_WebAPI.Data;
using Implemented_WebAPI.Models;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetAllAsync(string userId)
        {
            return await _context.TodoItems
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id, string userId)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TodoItem> CreateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            
            // Load the user information
            await _context.Entry(todoItem)
                .Reference(t => t.User)
                .LoadAsync();
                
            return todoItem;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync();
            
            // Load the user information
            await _context.Entry(todoItem)
                .Reference(t => t.User)
                .LoadAsync();
                
            return todoItem;
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var todoItem = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
                
            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id, string userId)
        {
            return await _context.TodoItems
                .AnyAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<(List<TodoItem> Items, int TotalCount)> GetFilteredAsync(string userId, TodoItemFilterDto filter)
        {
            var query = _context.TodoItems
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filter.Status))
            {
                switch (filter.Status.ToLower())
                {
                    case "completed":
                        query = query.Where(t => t.IsCompleted);
                        break;
                    case "pending":
                        query = query.Where(t => !t.IsCompleted);
                        break;
                    case "overdue":
                        query = query.Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < DateTime.UtcNow);
                        break;
                }
            }

            if (filter.DueDateFrom.HasValue)
            {
                query = query.Where(t => t.DueDate >= filter.DueDateFrom.Value);
            }

            if (filter.DueDateTo.HasValue)
            {
                query = query.Where(t => t.DueDate <= filter.DueDateTo.Value);
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(t => t.Title.Contains(filter.SearchTerm) || 
                                        (t.Description != null && t.Description.Contains(filter.SearchTerm)));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = filter.SortBy.ToLower() switch
            {
                "title" => filter.SortDirection.ToLower() == "asc" 
                    ? query.OrderBy(t => t.Title) 
                    : query.OrderByDescending(t => t.Title),
                "duedate" => filter.SortDirection.ToLower() == "asc" 
                    ? query.OrderBy(t => t.DueDate) 
                    : query.OrderByDescending(t => t.DueDate),
                "iscompleted" => filter.SortDirection.ToLower() == "asc" 
                    ? query.OrderBy(t => t.IsCompleted) 
                    : query.OrderByDescending(t => t.IsCompleted),
                _ => filter.SortDirection.ToLower() == "asc" 
                    ? query.OrderBy(t => t.CreatedAt) 
                    : query.OrderByDescending(t => t.CreatedAt)
            };

            // Apply pagination
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
