using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;
using Implemented_MVC.Services;
using FluentValidation;

namespace Implemented_MVC.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IValidator<CreateTodoItemDto> _createValidator;
        private readonly IValidator<UpdateTodoItemDto> _updateValidator;
        private readonly IValidator<TodoItemFilterDto> _filterValidator;

        public TodoController(
            ITodoItemService todoService,
            UserManager<ApplicationUser> userManager,
            IValidator<CreateTodoItemDto> createValidator,
            IValidator<UpdateTodoItemDto> updateValidator,
            IValidator<TodoItemFilterDto> filterValidator)
        {
            _todoService = todoService;
            _userManager = userManager;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _filterValidator = filterValidator;
        }

        // GET: Todo
        public async Task<IActionResult> Index(string? status = null, string? sortBy = null, string? sortOrder = null, int page = 1, int pageSize = 10)
        {
            var filter = new TodoItemFilterDto
            {
                Status = status ?? "all",
                SortBy = sortBy ?? "CreatedAt",
                SortOrder = sortOrder ?? "desc",
                Page = page,
                PageSize = pageSize
            };

            var validationResult = await _filterValidator.ValidateAsync(filter);
            if (!validationResult.IsValid)
            {
                TempData["ErrorMessage"] = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                filter = new TodoItemFilterDto(); // Reset to default
            }

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoService.GetFilteredAsync(filter, currentUserId!, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                ViewBag.TodoItems = new PaginatedResultDto<TodoItemDto> { Items = new List<TodoItemDto>() };
            }
            else
            {
                ViewBag.TodoItems = result.Data;
            }

            ViewBag.CurrentFilter = filter;
            ViewBag.IsAdmin = isAdmin;

            return View();
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoService.GetByIdAsync(id, currentUserId!, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTodoItemDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(createDto);
            }

            var currentUserId = _userManager.GetUserId(User);
            var result = await _todoService.CreateAsync(createDto, currentUserId!);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(createDto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoService.GetByIdAsync(id, currentUserId!, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateTodoItemDto
            {
                Title = result.Data!.Title,
                Description = result.Data.Description,
                IsCompleted = result.Data.IsCompleted,
                DueDate = result.Data.DueDate
            };

            ViewBag.TodoId = id;
            return View(updateDto);
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTodoItemDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.TodoId = id;
                return View(updateDto);
            }

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoService.UpdateAsync(id, updateDto, currentUserId!, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                ViewBag.TodoId = id;
                return View(updateDto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoService.GetByIdAsync(id, currentUserId!, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoService.DeleteAsync(id, currentUserId!, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Todo/ToggleComplete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            // Get current todo
            var currentTodo = await _todoService.GetByIdAsync(id, currentUserId!, isAdmin);
            if (!currentTodo.Success)
            {
                return Json(new { success = false, message = currentTodo.Message });
            }

            // Toggle completion status
            var updateDto = new UpdateTodoItemDto
            {
                Title = currentTodo.Data!.Title,
                Description = currentTodo.Data.Description,
                IsCompleted = !currentTodo.Data.IsCompleted,
                DueDate = currentTodo.Data.DueDate
            };

            var result = await _todoService.UpdateAsync(id, updateDto, currentUserId!, isAdmin);

            return Json(new { success = result.Success, message = result.Message });
        }

        // GET: Todo/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoService.GetDashboardStatsAsync(currentUserId!, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            ViewBag.IsAdmin = isAdmin;
            return View(result.Data);
        }
    }
}
