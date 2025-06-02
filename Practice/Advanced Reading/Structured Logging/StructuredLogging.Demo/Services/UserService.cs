using StructuredLogging.Demo.Models;
using StructuredLogging.Demo.Services;

namespace StructuredLogging.Demo.Services
{
    /// <summary>
    /// User service implementation demonstrating structured logging patterns
    /// This class shows how to log user-related operations with proper context
    /// and structured data for better debugging and monitoring
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        
        // In-memory storage for demonstration purposes
        // In real applications, this would be replaced with database operations
        private readonly List<User> _users;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
            
            // Initialize with some demo users for testing
            _users = new List<User>
            {
                new User 
                { 
                    Id = 1, 
                    Username = "john_doe", 
                    Email = "john@example.com",
                    FirstName = "John", 
                    LastName = "Doe",
                    Role = "Customer",
                    CreatedAt = DateTime.Now.AddDays(-30),
                    IsActive = true
                },
                new User 
                { 
                    Id = 2, 
                    Username = "jane_smith", 
                    Email = "jane@example.com",
                    FirstName = "Jane", 
                    LastName = "Smith",
                    Role = "Admin",
                    CreatedAt = DateTime.Now.AddDays(-15),
                    IsActive = true
                }
            };
        }

        /// <summary>
        /// User login method demonstrating structured logging for authentication events
        /// This method shows how to log security-related events with proper context
        /// </summary>
        public async Task<LoginResult> LoginAsync(LoginRequest request)
        {
            // Start logging the login attempt with structured data
            // Notice how we include the username and IP address for security tracking
            _logger.LogInformation("Login attempt started for user {Username} from IP {IpAddress}", 
                request.Username, request.IpAddress);

            try
            {
                // Simulate some processing time
                await Task.Delay(100);

                var user = await GetUserByUsernameAsync(request.Username);
                
                if (user == null)
                {
                    // Log failed login with structured data - this is crucial for security monitoring
                    // We include the failure reason, username, and IP for security analysis
                    _logger.LogWarning("Login failed for user {Username} from IP {IpAddress}. Reason: {FailureReason}",
                        request.Username, request.IpAddress, "UserNotFound");
                    
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Invalid credentials",
                        FailureReason = LoginFailureReason.InvalidCredentials,
                        LoginTime = DateTime.UtcNow
                    };
                }

                if (!user.IsActive)
                {
                    // Log account disabled attempt with user context
                    _logger.LogWarning("Login attempt for disabled user {Username} (ID: {UserId}) from IP {IpAddress}",
                        request.Username, user.Id, request.IpAddress);
                    
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Account is disabled",
                        FailureReason = LoginFailureReason.AccountDisabled,
                        LoginTime = DateTime.UtcNow
                    };
                }

                // For demo purposes, we'll accept any password that's not empty
                // In real applications, you'd verify against hashed passwords
                if (string.IsNullOrEmpty(request.Password))
                {
                    // Log invalid password attempt
                    _logger.LogWarning("Login failed for user {Username} (ID: {UserId}) from IP {IpAddress}. Reason: {FailureReason}",
                        request.Username, user.Id, request.IpAddress, "InvalidPassword");
                    
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Invalid credentials",
                        FailureReason = LoginFailureReason.InvalidCredentials,
                        LoginTime = DateTime.UtcNow
                    };
                }

                // Update last login time
                await UpdateLastLoginAsync(user.Id);

                // Log successful login with comprehensive context
                // This includes user details, session info, and security context
                var sessionId = Guid.NewGuid().ToString();
                _logger.LogInformation("User {Username} (ID: {UserId}, Role: {Role}) logged in successfully from IP {IpAddress}. Session: {SessionId}",
                    user.Username, user.Id, user.Role, request.IpAddress, sessionId);

                return new LoginResult
                {
                    Success = true,
                    Message = "Login successful",
                    User = user,
                    SessionId = sessionId,
                    LoginTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                // Log system errors during login with full exception context
                // This helps with debugging system issues vs. user errors
                _logger.LogError(ex, "System error during login attempt for user {Username} from IP {IpAddress}",
                    request.Username, request.IpAddress);

                return new LoginResult
                {
                    Success = false,
                    Message = "System error occurred",
                    FailureReason = LoginFailureReason.SystemError,
                    LoginTime = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Get user by ID with structured logging for data access operations
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            // Log data access with query parameters
            _logger.LogDebug("Retrieving user by ID {UserId}", userId);

            try
            {
                await Task.Delay(50); // Simulate database access
                
                var user = _users.FirstOrDefault(u => u.Id == userId);
                
                if (user != null)
                {
                    // Log successful data retrieval with user context
                    _logger.LogDebug("User {Username} (ID: {UserId}) retrieved successfully", 
                        user.Username, user.Id);
                }
                else
                {
                    // Log when user is not found - this might indicate data integrity issues
                    _logger.LogWarning("User with ID {UserId} not found", userId);
                }

                return user;
            }
            catch (Exception ex)
            {
                // Log data access errors with context
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Get user by username with structured logging
        /// </summary>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            _logger.LogDebug("Retrieving user by username {Username}", username);

            try
            {
                await Task.Delay(50); // Simulate database access
                
                var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                
                if (user != null)
                {
                    _logger.LogDebug("User {Username} (ID: {UserId}) found by username search", 
                        user.Username, user.Id);
                }
                else
                {
                    _logger.LogDebug("No user found with username {Username}", username);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by username {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// Create new user with structured logging for user management operations
        /// </summary>
        public async Task<User> CreateUserAsync(User user)
        {
            // Log user creation attempt with user details
            _logger.LogInformation("Creating new user {Username} with email {Email}", 
                user.Username, user.Email);

            try
            {
                // Check for duplicate username
                var existingUser = await GetUserByUsernameAsync(user.Username);
                if (existingUser != null)
                {
                    // Log business rule violation
                    _logger.LogWarning("User creation failed: Username {Username} already exists (existing user ID: {ExistingUserId})",
                        user.Username, existingUser.Id);
                    
                    throw new InvalidOperationException($"Username {user.Username} already exists");
                }

                // Assign ID and set creation time
                user.Id = _users.Max(u => u.Id) + 1;
                user.CreatedAt = DateTime.UtcNow;
                
                _users.Add(user);

                // Log successful user creation with comprehensive context
                _logger.LogInformation("User created successfully: {Username} (ID: {UserId}, Email: {Email}, Role: {Role})",
                    user.Username, user.Id, user.Email, user.Role);

                return user;
            }
            catch (Exception ex)
            {
                // Log user creation errors
                _logger.LogError(ex, "Error creating user {Username} with email {Email}", 
                    user.Username, user.Email);
                throw;
            }
        }

        /// <summary>
        /// Update last login time with structured logging for audit trail
        /// </summary>
        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            _logger.LogDebug("Updating last login time for user {UserId}", userId);

            try
            {
                await Task.Delay(25); // Simulate database update
                
                var user = _users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    var previousLogin = user.LastLoginAt;
                    user.LastLoginAt = DateTime.UtcNow;
                    
                    // Log the update with before/after context for audit purposes
                    _logger.LogDebug("Last login updated for user {Username} (ID: {UserId}). Previous: {PreviousLogin}, Current: {CurrentLogin}",
                        user.Username, user.Id, previousLogin, user.LastLoginAt);
                    
                    return true;
                }
                else
                {
                    _logger.LogWarning("Cannot update last login: User {UserId} not found", userId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating last login for user {UserId}", userId);
                return false;
            }
        }
    }
}
