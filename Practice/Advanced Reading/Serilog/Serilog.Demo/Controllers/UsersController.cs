using Microsoft.AspNetCore.Mvc;
using Serilog.Demo.Models;
using Serilog.Demo.Services;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Serilog.Demo.Controllers;

/// <summary>
/// User controller demonstrating Serilog usage in API endpoints
/// Shows how to log HTTP operations, request/response data, and API-specific concerns
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Get user by ID
    /// Demonstrates logging API requests with parameters and responses
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<User>>> GetUser(int id)
    {
        // Add request context to all logs in this operation
        using (LogContext.PushProperty("UserId", id))
        using (LogContext.PushProperty("RequestPath", Request.Path))
        using (LogContext.PushProperty("RequestMethod", Request.Method))
        {
            _logger.LogInformation("API request: GET /api/users/{UserId}", id);

            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found, returning 404", id);
                    return NotFound(new ApiResponse<User>
                    {
                        Success = false,
                        Message = $"User with ID {id} not found"
                    });
                }

                _logger.LogInformation("Successfully retrieved user {Username} for API request", user.Username);

                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Data = user,
                    Message = "User retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user {UserId}: {ErrorMessage}", 
                    id, ex.Message);

                return StatusCode(500, new ApiResponse<User>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }

    /// <summary>
    /// Create a new user
    /// Shows logging for POST operations and input validation
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<User>>> CreateUser(User user)
    {
        // Generate request ID for this operation
        var requestId = Guid.NewGuid().ToString("N")[..8];
        
        using (LogContext.PushProperty("RequestId", requestId))
        using (LogContext.PushProperty("Username", user.Username))
        {
            _logger.LogInformation("API request: POST /api/users for username {Username}", user.Username);

            try
            {
                // Log the incoming request data (be careful with sensitive information)
                _logger.LogDebug("Creating user with data: {@UserData}", new
                {
                    user.Username,
                    user.Email,
                    user.FirstName,
                    user.LastName
                    // Note: We intentionally exclude any sensitive data
                });

                var createdUser = await _userService.CreateUserAsync(user);

                _logger.LogInformation("User {Username} created successfully via API with ID {UserId}", 
                    createdUser.Username, createdUser.UserId);

                return CreatedAtAction(
                    nameof(GetUser),
                    new { id = createdUser.UserId },
                    new ApiResponse<User>
                    {
                        Success = true,
                        Data = createdUser,
                        Message = "User created successfully"
                    });
            }
            catch (InvalidOperationException ex)
            {
                // Business logic exceptions (like duplicate username)
                _logger.LogWarning("User creation failed due to business rule violation: {ErrorMessage}", ex.Message);

                return Conflict(new ApiResponse<User>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user creation: {ErrorMessage}", ex.Message);

                return StatusCode(500, new ApiResponse<User>
                {
                    Success = false,
                    Message = "An unexpected error occurred while creating the user",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }

    /// <summary>
    /// User login endpoint
    /// Demonstrates security-related logging
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
    {
        var requestId = Guid.NewGuid().ToString("N")[..8];
        
        using (LogContext.PushProperty("RequestId", requestId))
        using (LogContext.PushProperty("Username", request.Username))
        using (LogContext.PushProperty("ClientIP", HttpContext.Connection.RemoteIpAddress?.ToString()))
        using (LogContext.PushProperty("UserAgent", Request.Headers.UserAgent.ToString()))
        {
            _logger.LogInformation("Login attempt for username {Username} from IP {ClientIP}", 
                request.Username, HttpContext.Connection.RemoteIpAddress?.ToString());

            try
            {
                var isValid = await _userService.ValidateUserCredentialsAsync(request.Username, request.Password);

                if (isValid)
                {
                    // In a real application, you'd generate a JWT token here
                    var token = $"mock-jwt-token-{requestId}";
                    
                    _logger.LogInformation("Successful login for username {Username}", request.Username);

                    return Ok(new ApiResponse<string>
                    {
                        Success = true,
                        Data = token,
                        Message = "Login successful"
                    });
                }
                else
                {
                    // Security: Log failed login attempts but don't reveal too much information
                    _logger.LogWarning("Failed login attempt for username {Username}", request.Username);

                    return Unauthorized(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process for username {Username}: {ErrorMessage}", 
                    request.Username, ex.Message);

                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred during login",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }

    /// <summary>
    /// Deactivate user account
    /// Shows logging for DELETE/deactivation operations
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeactivateUser(int id)
    {
        using (LogContext.PushProperty("UserId", id))
        {
            _logger.LogInformation("API request: DELETE /api/users/{UserId} (deactivation)", id);

            try
            {
                var result = await _userService.DeactivateUserAsync(id);

                if (result)
                {
                    _logger.LogInformation("User {UserId} deactivated successfully via API", id);

                    return Ok(new ApiResponse<bool>
                    {
                        Success = true,
                        Data = true,
                        Message = "User deactivated successfully"
                    });
                }
                else
                {
                    _logger.LogWarning("Failed to deactivate user {UserId} - user not found", id);

                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Data = false,
                        Message = $"User with ID {id} not found"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deactivating user {UserId}: {ErrorMessage}", 
                    id, ex.Message);

                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "An error occurred while deactivating the user",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
