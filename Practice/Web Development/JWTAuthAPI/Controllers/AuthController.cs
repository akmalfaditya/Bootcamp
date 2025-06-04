using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JWTAuthAPI.DTOs;
using JWTAuthAPI.Models;
using JWTAuthAPI.Services;

namespace JWTAuthAPI.Controllers
{
    /// <summary>
    /// Authentication Controller using Microsoft Identity
    /// This handles user registration, login, and profile management
    /// Think of this as your security checkpoint - it decides who gets in and what they can do
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenService tokenService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user account using Microsoft Identity's built-in user management
        /// This is like signing up for a new account with enterprise-grade security
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                // Check if user already exists using Identity's UserManager
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email is already registered." });
                }

                // Create new user with Identity's built-in validation and security
                var newUser = new ApplicationUser
                {
                    UserName = registerDto.Email, // Identity uses UserName for login
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    EmailConfirmed = true // For demo purposes, auto-confirm email
                };

                // Identity handles password hashing, validation, and security automatically
                var result = await _userManager.CreateAsync(newUser, registerDto.Password);

                if (!result.Succeeded)
                {
                    // Return Identity's validation errors
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { message = "Registration failed", errors });
                }

                // Assign default "User" role to new registrations
                await _userManager.AddToRoleAsync(newUser, "User");

                _logger.LogInformation("User {Email} registered successfully", registerDto.Email);

                return CreatedAtAction(nameof(GetProfile), new { }, new
                {
                    message = "User registered successfully",
                    userId = newUser.Id,
                    email = newUser.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for {Email}", registerDto.Email);
                return StatusCode(500, new { message = "Registration failed due to server error" });
            }
        }

        /// <summary>
        /// Login endpoint using Microsoft Identity's SignInManager
        /// This validates credentials and issues JWT tokens with Identity's security features
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                // Find user by email using Identity's UserManager
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", loginDto.Email);
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                // Use Identity's SignInManager to validate password with built-in security features
                // This handles lockouts, two-factor auth, and other security policies
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed login attempt for user: {Email}", loginDto.Email);
                    
                    if (result.IsLockedOut)
                        return Unauthorized(new { message = "Account is locked out." });
                    
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                // Get user roles using Identity's UserManager
                var roles = await _userManager.GetRolesAsync(user);

                // Generate JWT token
                var token = _tokenService.GenerateToken(user, roles.ToList());

                _logger.LogInformation("User {Email} logged in successfully", user.Email);

                return Ok(new AuthResponseDTO
                {
                    Token = token,
                    Email = user.Email ?? "",
                    FullName = user.FullName,
                    Roles = roles.ToList(),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60) // Should match token expiration
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", loginDto.Email);
                return StatusCode(500, new { message = "Login failed due to server error" });
            }
        }

        /// <summary>
        /// Get current user profile information
        /// This endpoint requires authentication (JWT token)
        /// </summary>
        [HttpGet("profile")]
        [Authorize] // This attribute requires a valid JWT token
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // Get current user from JWT claims using Identity
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return Unauthorized(new { message = "Invalid token." });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);                return Ok(new UserProfileDTO
                {
                    Id = user.Id, // Use string ID directly
                    Email = user.Email ?? "",
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = user.CreatedAt,
                    Roles = roles.ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user profile");
                return StatusCode(500, new { message = "Failed to retrieve profile" });
            }
        }

        /// <summary>
        /// Admin-only endpoint to demonstrate role-based authorization
        /// This shows how to protect endpoints based on user roles
        /// </summary>
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            var userName = User.Identity?.Name;
            return Ok(new { 
                message = "Welcome to the admin area!", 
                user = userName,
                timestamp = DateTime.UtcNow 
            });
        }

        /// <summary>
        /// Get all registered users (Admin only)
        /// This demonstrates how to use UserManager for user management operations
        /// </summary>
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = _userManager.Users.ToList();
                var userProfiles = new List<UserProfileDTO>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);                    userProfiles.Add(new UserProfileDTO
                    {
                        Id = user.Id, // Use string ID directly
                        Email = user.Email ?? "",
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CreatedAt = user.CreatedAt,
                        Roles = roles.ToList()
                    });
                }

                return Ok(userProfiles);
            }
            catch (Exception ex)
            {                _logger.LogError(ex, "Error retrieving all users");
                return StatusCode(500, new { message = "Failed to retrieve users" });
            }
        }
    }
}
