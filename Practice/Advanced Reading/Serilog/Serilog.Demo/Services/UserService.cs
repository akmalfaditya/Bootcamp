using Serilog.Demo.Models;
using Serilog;
using Serilog.Context;

namespace Serilog.Demo.Services;

/// <summary>
/// User service implementation demonstrating structured logging with Serilog
/// This service shows various logging scenarios including:
/// - Method entry/exit logging
/// - Structured data logging with properties
/// - Error handling and exception logging
/// - Performance monitoring with timing
/// - Security-related logging (login attempts, etc.)
/// </summary>
public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    
    // Simulated in-memory data store for demo purposes
    // In a real application, this would be a database context
    private static readonly List<User> _users = new()
    {
        new User { UserId = 1, Username = "john_doe", Email = "john@example.com", 
                  FirstName = "John", LastName = "Doe", CreatedAt = DateTime.Now.AddDays(-30), IsActive = true },
        new User { UserId = 2, Username = "jane_smith", Email = "jane@example.com", 
                  FirstName = "Jane", LastName = "Smith", CreatedAt = DateTime.Now.AddDays(-15), IsActive = true },
        new User { UserId = 3, Username = "inactive_user", Email = "inactive@example.com", 
                  FirstName = "Inactive", LastName = "User", CreatedAt = DateTime.Now.AddDays(-60), IsActive = false }
    };

    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
        // Log service initialization - useful for tracking service lifecycle
        _logger.LogInformation("UserService initialized at {Timestamp}", DateTime.UtcNow);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        // This is a common pattern: log method entry with parameters
        // The {UserId} placeholder creates structured logging properties
        _logger.LogInformation("Attempting to retrieve user with ID {UserId}", userId);

        // Simulate async database call with delay
        await Task.Delay(Random.Shared.Next(10, 100));

        var user = _users.FirstOrDefault(u => u.UserId == userId);
        
        if (user != null)
        {
            // Log successful retrieval with multiple structured properties
            // Notice how we include both the input parameter and result data
            _logger.LogInformation("Successfully retrieved user {Username} (ID: {UserId}, Active: {IsActive})", 
                user.Username, user.UserId, user.IsActive);
            
            // Example of enriching log context for subsequent operations
            // Any logs within this scope will include the UserId property
            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("Username", user.Username))
            {
                _logger.LogDebug("User data loaded: {UserData}", new { 
                    user.Username, 
                    user.Email, 
                    user.FullName, 
                    user.CreatedAt,
                    DaysOld = (DateTime.Now - user.CreatedAt).Days 
                });
            }
        }
        else
        {
            // Log when data is not found - this is important for debugging
            // Use Warning level because this might indicate an issue
            _logger.LogWarning("User with ID {UserId} not found in the system", userId);
        }

        return user;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        // Validate input parameters and log validation issues
        if (string.IsNullOrWhiteSpace(username))
        {
            _logger.LogWarning("GetUserByUsername called with null or empty username");
            return null;
        }

        _logger.LogInformation("Searching for user with username {Username}", username);

        await Task.Delay(Random.Shared.Next(15, 80));

        var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        
        if (user != null)
        {
            _logger.LogInformation("Found user {Username} with ID {UserId}", username, user.UserId);
        }
        else
        {
            // This could be a security concern - log with appropriate level
            _logger.LogInformation("Username {Username} not found - possible invalid login attempt", username);
        }

        return user;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _logger.LogInformation("Creating new user account for {Username} ({Email})", 
            user.Username, user.Email);

        try
        {
            // Simulate validation
            if (_users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            {
                var message = $"Username {user.Username} is already taken";
                _logger.LogError("User creation failed: {ErrorMessage} for username {Username}", 
                    message, user.Username);
                throw new InvalidOperationException(message);
            }

            // Simulate database save operation
            await Task.Delay(Random.Shared.Next(50, 200));

            // Assign new user ID and set creation timestamp
            user.UserId = _users.Count + 1;
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            _users.Add(user);

            // Log successful creation with comprehensive details
            _logger.LogInformation("Successfully created user account {Username} with ID {UserId} at {CreatedAt}", 
                user.Username, user.UserId, user.CreatedAt);

            // Example of logging business metrics
            // This type of logging is valuable for business intelligence
            _logger.LogInformation("User creation metrics: {TotalUsers} total users in system", _users.Count);

            return user;
        }
        catch (Exception ex)
        {
            // Always log exceptions with full context
            // Include the operation details and user data (excluding sensitive info)
            _logger.LogError(ex, "Failed to create user account for {Username} ({Email}). Error: {ErrorMessage}", 
                user.Username, user.Email, ex.Message);
            throw; // Re-throw to maintain exception flow
        }
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        // Security logging: authentication attempts should always be logged
        // But be careful not to log the actual password!
        _logger.LogInformation("Authentication attempt for username {Username} from source {Source}", 
            username, "API");

        // Input validation
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Authentication failed: Missing username or password for user {Username}", 
                username ?? "[empty]");
            return false;
        }

        await Task.Delay(Random.Shared.Next(100, 300)); // Simulate authentication delay

        var user = await GetUserByUsernameAsync(username);
        
        if (user == null)
        {
            // Security logging: failed login attempts
            _logger.LogWarning("Authentication failed: User {Username} not found", username);
            return false;
        }

        if (!user.IsActive)
        {
            // Log account status issues
            _logger.LogWarning("Authentication failed: User {Username} account is inactive", username);
            return false;
        }

        // Simulate password verification (in real app, use proper hashing)
        bool isValidPassword = password == "password123"; // Simplified for demo

        if (isValidPassword)
        {
            // Success: log with user context
            using (LogContext.PushProperty("UserId", user.UserId))
            using (LogContext.PushProperty("Username", user.Username))
            {
                _logger.LogInformation("Successful authentication for user {Username} (ID: {UserId})", 
                    user.Username, user.UserId);
            }
            return true;
        }
        else
        {
            // Security: log failed password attempts
            _logger.LogWarning("Authentication failed: Invalid password for user {Username}", username);
            return false;
        }
    }

    public async Task<bool> DeactivateUserAsync(int userId)
    {
        _logger.LogInformation("Attempting to deactivate user with ID {UserId}", userId);

        try
        {
            var user = await GetUserByIdAsync(userId);
            
            if (user == null)
            {
                _logger.LogWarning("Cannot deactivate user: User with ID {UserId} not found", userId);
                return false;
            }

            if (!user.IsActive)
            {
                _logger.LogInformation("User {Username} (ID: {UserId}) is already inactive", 
                    user.Username, userId);
                return true;
            }

            // Simulate deactivation process
            await Task.Delay(Random.Shared.Next(50, 150));

            user.IsActive = false;

            // Log the business operation with context
            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("Username", user.Username))
            {
                _logger.LogInformation("Successfully deactivated user {Username} (ID: {UserId})", 
                    user.Username, userId);
                
                // Log business metrics
                var activeUserCount = _users.Count(u => u.IsActive);
                _logger.LogInformation("User deactivation completed. Active users remaining: {ActiveUserCount}", 
                    activeUserCount);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deactivating user {UserId}: {ErrorMessage}", 
                userId, ex.Message);
            return false;
        }
    }
}
