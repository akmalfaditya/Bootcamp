using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;
using Implemented_MVC.Data;

namespace Implemented_MVC.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponseDto<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return new ApiResponseDto<UserDto>
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    };
                }

                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    return new ApiResponseDto<UserDto>
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                // Assign default role
                await _userManager.AddToRoleAsync(user, "User");

                var userDto = _mapper.Map<UserDto>(user);
                
                return new ApiResponseDto<UserDto>
                {
                    Success = true,
                    Data = userDto,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<UserDto>
                {
                    Success = false,
                    Message = $"Error during registration: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return new ApiResponseDto<LoginResponseDto>
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    return new ApiResponseDto<LoginResponseDto>
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<UserDto>(user);

                var loginResponse = new LoginResponseDto
                {
                    User = userDto,
                    Roles = roles.ToList()
                };

                return new ApiResponseDto<LoginResponseDto>
                {
                    Success = true,
                    Data = loginResponse,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<LoginResponseDto>
                {
                    Success = false,
                    Message = $"Error during login: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<bool>> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                
                return new ApiResponseDto<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Logout successful"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = $"Error during logout: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<UserDto>> GetCurrentUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponseDto<UserDto>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var userDto = _mapper.Map<UserDto>(user);
                
                return new ApiResponseDto<UserDto>
                {
                    Success = true,
                    Data = userDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<UserDto>
                {
                    Success = false,
                    Message = $"Error retrieving user: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await Task.FromResult(_userManager.Users.ToList());
                var userDtos = _mapper.Map<List<UserDto>>(users);

                // Add todo statistics for each user
                foreach (var userDto in userDtos)
                {
                    var todos = await _context.TodoItems
                        .Where(t => t.UserId == userDto.Id)
                        .ToListAsync();

                    userDto.TodoStats = new TodoStatsDto
                    {
                        Total = todos.Count,
                        Completed = todos.Count(t => t.IsCompleted),
                        Pending = todos.Count(t => !t.IsCompleted),
                        Overdue = todos.Count(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < DateTime.Now)
                    };

                    // Get user roles
                    var user = await _userManager.FindByIdAsync(userDto.Id);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        userDto.Roles = roles.ToList();
                    }
                }

                return new ApiResponseDto<List<UserDto>>
                {
                    Success = true,
                    Data = userDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<List<UserDto>>
                {
                    Success = false,
                    Message = $"Error retrieving users: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponseDto<bool>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return new ApiResponseDto<bool>
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                return new ApiResponseDto<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "User deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = $"Error deleting user: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<bool>> AssignRoleAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponseDto<bool>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    return new ApiResponseDto<bool>
                    {
                        Success = false,
                        Message = "Role does not exist"
                    };
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (!result.Succeeded)
                {
                    return new ApiResponseDto<bool>
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                return new ApiResponseDto<bool>
                {
                    Success = true,
                    Data = true,
                    Message = $"Role '{roleName}' assigned successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = $"Error assigning role: {ex.Message}"
                };
            }
        }
    }
}
