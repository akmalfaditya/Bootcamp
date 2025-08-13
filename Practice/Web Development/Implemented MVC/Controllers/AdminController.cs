using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Implemented_MVC.Services;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITodoItemService _todoService;

        public AdminController(IUserService userService, ITodoItemService todoService)
        {
            _userService = userService;
            _todoService = todoService;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            // Get dashboard stats for admin
            var statsResult = await _todoService.GetDashboardStatsAsync("", true);
            var usersResult = await _userService.GetAllUsersAsync();

            ViewBag.Stats = statsResult.Success ? statsResult.Data : null;
            ViewBag.Users = usersResult.Success ? usersResult.Data : new List<UserDto>();

            if (!statsResult.Success)
            {
                TempData["ErrorMessage"] = statsResult.Message;
            }

            if (!usersResult.Success)
            {
                TempData["ErrorMessage"] = usersResult.Message;
            }

            return View();
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var result = await _userService.GetAllUsersAsync();

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<UserDto>());
            }

            return View(result.Data);
        }

        // GET: Admin/AllTodos
        public async Task<IActionResult> AllTodos(string? userId = null, string? status = null, string? sortBy = null, string? sortOrder = null, int page = 1, int pageSize = 20)
        {
            var filter = new TodoItemFilterDto
            {
                Status = status ?? "all",
                SortBy = sortBy ?? "CreatedAt",
                SortOrder = sortOrder ?? "desc",
                Page = page,
                PageSize = pageSize
            };

            var result = await _todoService.GetFilteredAsync(filter, userId ?? "", true); // Empty userId for admin means get all

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
            ViewBag.SelectedUserId = userId;
            return View();
        }

        // POST: Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Users));
        }

        // POST: Admin/AssignRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var result = await _userService.AssignRoleAsync(userId, roleName);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Users));
        }

        // POST: Admin/DeleteTodo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTodo(int todoId)
        {
            var result = await _todoService.DeleteAsync(todoId, "", true); // Empty userId for admin

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(AllTodos));
        }
    }
}
