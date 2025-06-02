using Microsoft.AspNetCore.Mvc;
using StructuredLogging.Demo.Models;
using StructuredLogging.Demo.Services;

namespace StructuredLogging.Demo.Controllers
{
    /// <summary>
    /// User controller demonstrating structured logging in API endpoints
    /// This controller shows how to log HTTP requests, responses, and user interactions
    /// with proper correlation IDs and request context
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// User login endpoint demonstrating structured logging for authentication
        /// This method shows how to log HTTP requests with security context
        /// and proper handling of sensitive information
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest request)
        {
            // Generate a correlation ID for tracking this request through the system
            var correlationId = Guid.NewGuid().ToString();
            
            // Get client IP address for security logging
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            
            // Log the incoming request with structured data
            // Notice: We don't log the password for security reasons
            _logger.LogInformation("Login request received. Username: {Username}, IP: {ClientIp}, " +
                "UserAgent: {UserAgent}, CorrelationId: {CorrelationId}",
                request.Username, clientIp, userAgent, correlationId);

            try
            {
                // Add IP address to the request for service layer logging
                request.IpAddress = clientIp;
                request.UserAgent = userAgent;

                // Call the service layer
                var result = await _userService.LoginAsync(request);

                if (result.Success)
                {
                    // Log successful authentication with user context
                    _logger.LogInformation("Login successful for user {Username} (ID: {UserId}). " +
                        "Session: {SessionId}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        result.User?.Username, result.User?.Id, result.SessionId, clientIp, correlationId);
                    
                    return Ok(result);
                }
                else
                {
                    // Log failed authentication attempt with failure context
                    _logger.LogWarning("Login failed for user {Username}. Reason: {FailureReason}, " +
                        "IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        request.Username, result.FailureReason, clientIp, correlationId);
                    
                    return Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                // Log system errors during authentication
                _logger.LogError(ex, "System error during login for user {Username}, " +
                    "IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    request.Username, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get user by ID endpoint with structured logging for data retrieval
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var correlationId = Guid.NewGuid().ToString();
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            _logger.LogInformation("Get user request received. UserId: {UserId}, IP: {ClientIp}, " +
                "CorrelationId: {CorrelationId}", id, clientIp, correlationId);

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                
                if (user != null)
                {
                    // Log successful data retrieval
                    _logger.LogInformation("User data retrieved successfully. Username: {Username} (ID: {UserId}), " +
                        "Role: {Role}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        user.Username, user.Id, user.Role, clientIp, correlationId);
                    
                    return Ok(user);
                }
                else
                {
                    // Log when requested resource is not found
                    _logger.LogWarning("User not found. UserId: {UserId}, IP: {ClientIp}, " +
                        "CorrelationId: {CorrelationId}", id, clientIp, correlationId);
                    
                    return NotFound(new { message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user. UserId: {UserId}, IP: {ClientIp}, " +
                    "CorrelationId: {CorrelationId}", id, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Create new user endpoint with structured logging for user management
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            var correlationId = Guid.NewGuid().ToString();
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            _logger.LogInformation("Create user request received. Username: {Username}, Email: {Email}, " +
                "Role: {Role}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                user.Username, user.Email, user.Role, clientIp, correlationId);

            try
            {
                var createdUser = await _userService.CreateUserAsync(user);
                
                // Log successful user creation
                _logger.LogInformation("User created successfully. Username: {Username} (ID: {UserId}), " +
                    "Email: {Email}, Role: {Role}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    createdUser.Username, createdUser.Id, createdUser.Email, 
                    createdUser.Role, clientIp, correlationId);
                
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                // Log business rule violations
                _logger.LogWarning("User creation failed due to business rule violation. " +
                    "Username: {Username}, Email: {Email}, Error: {ErrorMessage}, " +
                    "IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    user.Username, user.Email, ex.Message, clientIp, correlationId);
                
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user. Username: {Username}, Email: {Email}, " +
                    "IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    user.Username, user.Email, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
