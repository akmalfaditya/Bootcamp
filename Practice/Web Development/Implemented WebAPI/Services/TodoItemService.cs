using AutoMapper;
using Implemented_WebAPI.DTOs;
using Implemented_WebAPI.Models;
using Implemented_WebAPI.Repositories;

namespace Implemented_WebAPI.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _repository;
        private readonly IMapper _mapper;

        public TodoItemService(ITodoItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<List<TodoItemDto>>> GetAllAsync(string userId)
        {
            try
            {
                var todoItems = await _repository.GetAllAsync(userId);
                var todoItemDtos = _mapper.Map<List<TodoItemDto>>(todoItems);
                
                return ApiResponseDto<List<TodoItemDto>>.SuccessResult(todoItemDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<List<TodoItemDto>>.ErrorResult($"Error retrieving todo items: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<TodoItemDto>> GetByIdAsync(int id, string userId)
        {
            try
            {
                var todoItem = await _repository.GetByIdAsync(id, userId);
                
                if (todoItem == null)
                {
                    return ApiResponseDto<TodoItemDto>.ErrorResult("Todo item not found");
                }

                var todoItemDto = _mapper.Map<TodoItemDto>(todoItem);
                return ApiResponseDto<TodoItemDto>.SuccessResult(todoItemDto);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TodoItemDto>.ErrorResult($"Error retrieving todo item: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto, string userId)
        {
            try
            {
                var todoItem = _mapper.Map<TodoItem>(createDto);
                todoItem.UserId = userId;
                
                var createdTodoItem = await _repository.CreateAsync(todoItem);
                var todoItemDto = _mapper.Map<TodoItemDto>(createdTodoItem);
                
                return ApiResponseDto<TodoItemDto>.SuccessResult(todoItemDto, "Todo item created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TodoItemDto>.ErrorResult($"Error creating todo item: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto, string userId)
        {
            try
            {
                var existingTodoItem = await _repository.GetByIdAsync(id, userId);
                
                if (existingTodoItem == null)
                {
                    return ApiResponseDto<TodoItemDto>.ErrorResult("Todo item not found");
                }

                _mapper.Map(updateDto, existingTodoItem);
                
                var updatedTodoItem = await _repository.UpdateAsync(existingTodoItem);
                var todoItemDto = _mapper.Map<TodoItemDto>(updatedTodoItem);
                
                return ApiResponseDto<TodoItemDto>.SuccessResult(todoItemDto, "Todo item updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TodoItemDto>.ErrorResult($"Error updating todo item: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteAsync(int id, string userId)
        {
            try
            {
                var exists = await _repository.ExistsAsync(id, userId);
                
                if (!exists)
                {
                    return ApiResponseDto<bool>.ErrorResult("Todo item not found");
                }

                await _repository.DeleteAsync(id, userId);
                
                return ApiResponseDto<bool>.SuccessResult(true, "Todo item deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResult($"Error deleting todo item: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResultDto<TodoItemDto>>> GetFilteredAsync(string userId, TodoItemFilterDto filter)
        {
            try
            {
                var (items, totalCount) = await _repository.GetFilteredAsync(userId, filter);
                var todoItemDtos = _mapper.Map<List<TodoItemDto>>(items);
                
                var paginatedResult = new PaginatedResultDto<TodoItemDto>
                {
                    Data = todoItemDtos,
                    TotalCount = totalCount,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                
                return ApiResponseDto<PaginatedResultDto<TodoItemDto>>.SuccessResult(paginatedResult);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResultDto<TodoItemDto>>.ErrorResult($"Error retrieving filtered todo items: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<TodoStatsDto>> GetStatsAsync(string userId)
        {
            try
            {
                var todoItems = await _repository.GetAllAsync(userId);
                
                var stats = new TodoStatsDto
                {
                    Total = todoItems.Count,
                    Completed = todoItems.Count(t => t.IsCompleted),
                    Pending = todoItems.Count(t => !t.IsCompleted),
                    Overdue = todoItems.Count(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < DateTime.UtcNow)
                };
                
                return ApiResponseDto<TodoStatsDto>.SuccessResult(stats);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TodoStatsDto>.ErrorResult($"Error retrieving todo statistics: {ex.Message}");
            }
        }
    }
}
