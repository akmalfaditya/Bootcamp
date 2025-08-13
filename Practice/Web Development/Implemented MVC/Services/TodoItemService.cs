using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;
using Implemented_MVC.Repositories;

namespace Implemented_MVC.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoItemService(ITodoItemRepository todoRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ApiResponseDto<TodoItemDto>> GetByIdAsync(int id, string currentUserId, bool isAdmin)
        {
            try
            {
                var todoItem = await _todoRepository.GetByIdWithUserAsync(id);
                
                if (todoItem == null)
                {
                    return new ApiResponseDto<TodoItemDto>
                    {
                        Success = false,
                        Message = "Todo item not found"
                    };
                }

                // Check authorization: admin can see all, user can only see their own
                if (!isAdmin && todoItem.UserId != currentUserId)
                {
                    return new ApiResponseDto<TodoItemDto>
                    {
                        Success = false,
                        Message = "Access denied"
                    };
                }

                var todoDto = _mapper.Map<TodoItemDto>(todoItem);
                
                return new ApiResponseDto<TodoItemDto>
                {
                    Success = true,
                    Data = todoDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TodoItemDto>
                {
                    Success = false,
                    Message = $"Error retrieving todo item: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<PaginatedResultDto<TodoItemDto>>> GetFilteredAsync(TodoItemFilterDto filter, string currentUserId, bool isAdmin)
        {
            try
            {
                // If not admin, filter by current user
                var userId = isAdmin ? null : currentUserId;
                
                var paginatedResult = await _todoRepository.GetFilteredAsync(filter, userId);
                
                var todoItemDtos = _mapper.Map<List<TodoItemDto>>(paginatedResult.Items);
                
                var result = new PaginatedResultDto<TodoItemDto>
                {
                    Items = todoItemDtos,
                    TotalItems = paginatedResult.TotalItems,
                    Page = paginatedResult.Page,
                    PageSize = paginatedResult.PageSize,
                    TotalPages = paginatedResult.TotalPages,
                    HasPreviousPage = paginatedResult.HasPreviousPage,
                    HasNextPage = paginatedResult.HasNextPage
                };

                return new ApiResponseDto<PaginatedResultDto<TodoItemDto>>
                {
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<PaginatedResultDto<TodoItemDto>>
                {
                    Success = false,
                    Message = $"Error retrieving todo items: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto, string userId)
        {
            try
            {
                var todoItem = _mapper.Map<TodoItem>(createDto);
                todoItem.UserId = userId;
                todoItem.CreatedAt = DateTime.UtcNow;

                var createdTodo = await _todoRepository.CreateAsync(todoItem);
                var todoDto = _mapper.Map<TodoItemDto>(createdTodo);

                return new ApiResponseDto<TodoItemDto>
                {
                    Success = true,
                    Data = todoDto,
                    Message = "Todo item created successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TodoItemDto>
                {
                    Success = false,
                    Message = $"Error creating todo item: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto, string currentUserId, bool isAdmin)
        {
            try
            {
                var existingTodo = await _todoRepository.GetByIdAsync(id);
                
                if (existingTodo == null)
                {
                    return new ApiResponseDto<TodoItemDto>
                    {
                        Success = false,
                        Message = "Todo item not found"
                    };
                }

                // Check authorization
                if (!isAdmin && existingTodo.UserId != currentUserId)
                {
                    return new ApiResponseDto<TodoItemDto>
                    {
                        Success = false,
                        Message = "Access denied"
                    };
                }

                // Map updates
                _mapper.Map(updateDto, existingTodo);
                
                // Handle completion timestamp
                if (updateDto.IsCompleted && !existingTodo.IsCompleted)
                {
                    existingTodo.CompletedAt = DateTime.UtcNow;
                }
                else if (!updateDto.IsCompleted && existingTodo.IsCompleted)
                {
                    existingTodo.CompletedAt = null;
                }

                var updatedTodo = await _todoRepository.UpdateAsync(existingTodo);
                var todoDto = _mapper.Map<TodoItemDto>(updatedTodo);

                return new ApiResponseDto<TodoItemDto>
                {
                    Success = true,
                    Data = todoDto,
                    Message = "Todo item updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TodoItemDto>
                {
                    Success = false,
                    Message = $"Error updating todo item: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteAsync(int id, string currentUserId, bool isAdmin)
        {
            try
            {
                var existingTodo = await _todoRepository.GetByIdAsync(id);
                
                if (existingTodo == null)
                {
                    return new ApiResponseDto<bool>
                    {
                        Success = false,
                        Message = "Todo item not found"
                    };
                }

                // Check authorization
                if (!isAdmin && existingTodo.UserId != currentUserId)
                {
                    return new ApiResponseDto<bool>
                    {
                        Success = false,
                        Message = "Access denied"
                    };
                }

                var deleted = await _todoRepository.DeleteAsync(id);

                return new ApiResponseDto<bool>
                {
                    Success = deleted,
                    Data = deleted,
                    Message = deleted ? "Todo item deleted successfully" : "Failed to delete todo item"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = $"Error deleting todo item: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<List<TodoItemDto>>> GetUserTodosAsync(string userId)
        {
            try
            {
                var todoItems = await _todoRepository.GetByUserIdAsync(userId);
                var todoItemDtos = _mapper.Map<List<TodoItemDto>>(todoItems);

                return new ApiResponseDto<List<TodoItemDto>>
                {
                    Success = true,
                    Data = todoItemDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<List<TodoItemDto>>
                {
                    Success = false,
                    Message = $"Error retrieving user todos: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<object>> GetDashboardStatsAsync(string currentUserId, bool isAdmin)
        {
            try
            {
                var userId = isAdmin ? null : currentUserId;
                
                var totalCount = await _todoRepository.GetTotalCountAsync(userId);
                var completedCount = await _todoRepository.GetCompletedCountAsync(userId);
                var pendingCount = await _todoRepository.GetPendingCountAsync(userId);
                var overdueTasks = await _todoRepository.GetOverdueTasksAsync(userId);

                var stats = new
                {
                    TotalTodos = totalCount,
                    CompletedTodos = completedCount,
                    PendingTodos = pendingCount,
                    OverdueTodos = overdueTasks.Count,
                    CompletionRate = totalCount > 0 ? Math.Round((double)completedCount / totalCount * 100, 1) : 0,
                    OverdueTasksList = _mapper.Map<List<TodoItemDto>>(overdueTasks.Take(5)) // Show only 5 recent overdue tasks
                };

                return new ApiResponseDto<object>
                {
                    Success = true,
                    Data = stats
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<object>
                {
                    Success = false,
                    Message = $"Error retrieving dashboard stats: {ex.Message}"
                };
            }
        }
    }
}
