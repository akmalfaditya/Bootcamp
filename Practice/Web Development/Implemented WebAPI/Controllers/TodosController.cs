using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Implemented_WebAPI.DTOs;
using Implemented_WebAPI.Services;
using System.Security.Claims;

namespace Implemented_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IValidator<CreateTodoItemDto> _createValidator;
        private readonly IValidator<UpdateTodoItemDto> _updateValidator;

        public TodosController(
            ITodoItemService todoItemService,
            IValidator<CreateTodoItemDto> createValidator,
            IValidator<UpdateTodoItemDto> updateValidator)
        {
            _todoItemService = todoItemService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Get all todo items for the current user
        /// </summary>
        /// <returns>List of todo items</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.GetAllAsync(userId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get filtered and paginated todo items
        /// </summary>
        /// <param name="filter">Filter criteria</param>
        /// <returns>Paginated todo items</returns>
        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered([FromQuery] TodoItemFilterDto filter)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.GetFilteredAsync(userId, filter);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get a specific todo item by ID
        /// </summary>
        /// <param name="id">Todo item ID</param>
        /// <returns>Todo item details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.GetByIdAsync(id, userId);
            
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Create a new todo item
        /// </summary>
        /// <param name="createDto">Todo item data</param>
        /// <returns>Created todo item</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemDto createDto)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ApiResponseDto<TodoItemDto>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var result = await _todoItemService.CreateAsync(createDto, userId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// Update an existing todo item
        /// </summary>
        /// <param name="id">Todo item ID</param>
        /// <param name="updateDto">Updated todo item data</param>
        /// <returns>Updated todo item</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoItemDto updateDto)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ApiResponseDto<TodoItemDto>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var result = await _todoItemService.UpdateAsync(id, updateDto, userId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete a todo item
        /// </summary>
        /// <param name="id">Todo item ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.DeleteAsync(id, userId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get todo statistics for the current user
        /// </summary>
        /// <returns>Todo statistics</returns>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.GetStatsAsync(userId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
