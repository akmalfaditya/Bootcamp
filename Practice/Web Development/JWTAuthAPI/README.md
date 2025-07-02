# JWT Authentication API with Microsoft Identity

A comprehensive **JWT Authentication System** built with ASP.NET Core Web API and Microsoft Identity, demonstrating enterprise-grade authentication and authorization using industry best practices.

## Project Overview

This project showcases a complete JWT authentication system that leverages Microsoft Identity for user management, providing a robust foundation for secure API development.

### Key Features
- **Microsoft Identity Integration**: Enterprise-grade user management with built-in security features
- **JWT Token Authentication**: Stateless authentication perfect for APIs and microservices
- **Role-Based Authorization**: Protecting endpoints based on user roles (Admin, Manager, User)
- **Security Best Practices**: Password hashing, account lockouts, and secure token validation
- **Comprehensive Error Handling**: Proper error responses and logging
- **API Documentation**: Swagger integration with JWT authentication support
- **Database Integration**: Entity Framework Core with SQLite and automatic migrations

---

## **Quick Start - Run the Existing Project**

If you want to run this existing project immediately:

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Running the Application
```bash
# 1. Navigate to the project directory
cd "c:\Users\Formulatrix\Documents\Bootcamp\Practice\Web Development\JWTAuthAPI"

# 2. Restore NuGet packages
dotnet restore

# 3. Apply database migrations (creates SQLite database)
dotnet ef database update

# 4. Build and run the application
dotnet run
```

The API will be available at `https://localhost:7001` (HTTPS) and `http://localhost:5184` (HTTP).

## **Step-by-Step Tutorial: Building JWT Authentication API from Scratch**

This comprehensive tutorial will guide you through creating a complete JWT Authentication API using ASP.NET Core 8.0. You will learn to implement user registration, authentication, authorization, and secure API endpoints using modern development practices.

### **Learning Objectives**

By completing this tutorial, you will:
- Understand JWT token-based authentication and authorization
- Implement secure user registration and login functionality
- Create protected API endpoints with role-based access control
- Work with Entity Framework Core and SQL Server/SQLite
- Apply industry best practices for API security
- Implement proper error handling and validation
- Create comprehensive API documentation with Swagger

### **Step 1: Environment Setup and Project Creation**

#### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio Code](https://code.visualstudio.com/) with C# extension or [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Postman](https://www.postman.com/) for API testing (optional)

#### Create New Web API Project
```bash
# Create the project directory
mkdir JWTAuthAPI
cd JWTAuthAPI

# Create a new Web API project
dotnet new webapi -n JWTAuthAPI

# Navigate to the project folder
cd JWTAuthAPI
```

#### Install Required NuGet Packages
```bash
# Authentication and Authorization packages
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# Entity Framework Core packages
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design

# Additional utility packages
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package BCrypt.Net-Next
```

### **Step 2: Project Structure and Architecture**

Create the following folder structure:
```
JWTAuthAPI/
├── Controllers/
├── Models/
│   ├── Domain/
│   ├── DTOs/
│   └── Responses/
├── Data/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Middlewares/
├── Extensions/
└── Configurations/
```

### **Step 3: Domain Models**

#### Create User Entity
Create `Models/Domain/User.cs`:
```csharp
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthAPI.Models.Domain
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
```

#### Create Role Entity
Create `Models/Domain/Role.cs`:
```csharp
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthAPI.Models.Domain
{
    public class Role : IdentityRole
    {
        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

### **Step 4: Data Transfer Objects (DTOs)**

#### Authentication DTOs
Create `Models/DTOs/RegisterRequestDto.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace JWTAuthAPI.Models.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
```

Create `Models/DTOs/LoginRequestDto.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace JWTAuthAPI.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
```

Create `Models/DTOs/UserDto.cs`:
```csharp
namespace JWTAuthAPI.Models.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
```

#### Response DTOs
Create `Models/Responses/AuthResponseDto.cs`:
```csharp
namespace JWTAuthAPI.Models.Responses
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public UserDto? User { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
```

Create `Models/Responses/ApiResponseDto.cs`:
```csharp
namespace JWTAuthAPI.Models.Responses
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public int StatusCode { get; set; }

        public static ApiResponseDto<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 200
            };
        }

        public static ApiResponseDto<T> ErrorResponse(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                StatusCode = statusCode
            };
        }
    }
}
```

### **Step 5: Database Context**

Create `Data/AuthDbContext.cs`:
```csharp
using JWTAuthAPI.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthAPI.Data
{
    public class AuthDbContext : IdentityDbContext<User, Role, string>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Role entity
            builder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Seed default roles
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator with full access",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "Regular user with limited access",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
```

### **Step 6: Service Interfaces**

Create `Services/Interfaces/IAuthService.cs`:
```csharp
using JWTAuthAPI.Models.DTOs;
using JWTAuthAPI.Models.Responses;

namespace JWTAuthAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<ApiResponseDto<UserDto>> GetUserProfileAsync(string userId);
        Task<ApiResponseDto<List<UserDto>>> GetAllUsersAsync();
        Task<ApiResponseDto<bool>> AssignRoleAsync(string userId, string role);
    }
}
```

Create `Services/Interfaces/ITokenService.cs`:
```csharp
using JWTAuthAPI.Models.Domain;
using System.Security.Claims;

namespace JWTAuthAPI.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(User user);
        ClaimsPrincipal? ValidateToken(string token);
        Task<string> GenerateRefreshTokenAsync();
    }
}
```

### **Step 7: Service Implementations**

Create `Services/Implementations/TokenService.cs`:
```csharp
using JWTAuthAPI.Models.Domain;
using JWTAuthAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWTAuthAPI.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new("firstName", user.FirstName),
                new("lastName", user.LastName)
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["Jwt:ExpireHours"] ?? "24")),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }
    }
}
```

### **Step 8: Continue Implementation**

The tutorial continues with detailed implementation of the AuthService, Controllers, Program.cs configuration, database migrations, testing procedures, advanced features, security considerations, deployment guidelines, troubleshooting, and best practices.

This comprehensive approach ensures that developers understand not just what to implement, but why each component exists and how they work together to create a secure, production-ready JWT authentication system.

### **Available API Endpoints**

#### Authentication Endpoints
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login user and get JWT token
- `GET /api/auth/profile` - Get current user profile (requires authentication)

#### Admin Endpoints
- `GET /api/auth/users` - Get all users (Admin only)
- `POST /api/auth/assign-role` - Assign role to user (Admin only)

#### Test Endpoints
- `GET /api/auth/test-auth` - Test JWT authentication
- `GET /api/secure/public` - Public data (requires authentication)
- `GET /api/secure/admin` - Admin data (Admin role required)
- `GET /api/secure/user` - User data (User or Admin role required)

### **Testing the API**

#### Using Swagger UI
1. Navigate to `https://localhost:7001` when the application is running
2. Use the interactive Swagger interface to test endpoints
3. Click "Authorize" button and enter `Bearer YOUR_TOKEN_HERE` after login

#### Using Postman or cURL

**1. Register a New User**
```http
POST https://localhost:7001/api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "Password@123",
  "confirmPassword": "Password@123"
}
```

**2. Login**
```http
POST https://localhost:7001/api/auth/login
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "Password@123"
}
```

**3. Access Protected Endpoint**
```http
GET https://localhost:7001/api/auth/profile
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

### **Default Admin Credentials**
- **Email**: admin@jwtauth.com
- **Password**: Admin@123

### **Project Architecture**

#### Domain Models
- **User**: Extended IdentityUser with additional properties
- **Role**: Extended IdentityRole with description

#### DTOs (Data Transfer Objects)
- **RegisterRequestDto**: User registration data
- **LoginRequestDto**: User login credentials
- **UserDto**: User information response
- **AuthResponseDto**: Authentication response with token

#### Services
- **IAuthService / AuthService**: User authentication and management
- **ITokenService / TokenService**: JWT token generation and validation

#### Database
- Uses Entity Framework Core with Identity
- Supports both SQL Server and SQLite
- Automatic migrations and data seeding

### **Security Features**

- **JWT Authentication**: Stateless token-based authentication
- **Role-based Authorization**: Admin and User roles
- **Password Security**: BCrypt hashing with strong password requirements
- **Input Validation**: Comprehensive data validation
- **Error Handling**: Secure error responses without sensitive information
- **CORS Support**: Configurable cross-origin requests

### **Configuration**

Key configuration sections in `appsettings.json`:

#### JWT Settings
```json
"Jwt": {
  "Key": "Your-Secret-Key-Here",
  "Issuer": "JWTAuthAPI",
  "Audience": "JWTAuthAPI-Users",
  "ExpireHours": "24"
}
```

#### Database Connection
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JWTAuthDB;Trusted_Connection=true",
  "SqliteConnection": "Data Source=jwtauth.db"
}
```

### **Deployment**

#### Environment Variables (Production)
```bash
JWT__Key=your-production-jwt-secret-key
ConnectionStrings__DefaultConnection=your-production-connection-string
```

#### Docker Support
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "JWTAuthAPI.dll"]
```

### **Development Guidelines**

#### Adding New Endpoints
1. Create DTOs in `Models/DTOs/`
2. Add business logic to services
3. Create controller actions
4. Add authorization attributes as needed
5. Update Swagger documentation

#### Database Changes
```bash
# Add new migration
dotnet ef migrations add YourMigrationName

# Update database
dotnet ef database update
```

### **Dependencies**

#### Main Packages
- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT authentication
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` - Identity framework
- `Microsoft.EntityFrameworkCore.SqlServer` - SQL Server support
- `Microsoft.EntityFrameworkCore.Sqlite` - SQLite support
- `System.IdentityModel.Tokens.Jwt` - JWT token handling
- `BCrypt.Net-Next` - Password hashing

### **Learning Resources**

- [JWT.io](https://jwt.io/) - JWT token debugging
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Security](https://docs.microsoft.com/en-us/aspnet/core/security/)

### **Troubleshooting**

#### Common Issues

**Database Connection**
```bash
# Check connection string in appsettings.json
# Ensure SQL Server/SQLite is accessible
# Verify Entity Framework tools are installed
```

**JWT Token Issues**
```bash
# Verify JWT configuration
# Check token expiration
# Ensure Bearer token format in requests
```

**CORS Issues**
```bash
# Check CORS policy in Program.cs
# Verify allowed origins for frontend applications
```

#### Error Codes
- **400 Bad Request**: Invalid input data
- **401 Unauthorized**: Invalid credentials or missing token
- **403 Forbidden**: Insufficient permissions
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server-side error

### **Future Enhancements**

- Refresh token implementation
- Email verification system
- Password reset functionality
- Two-factor authentication (2FA)
- Social media login integration
- API rate limiting
- Comprehensive logging and monitoring
- API versioning
- OpenAPI 3.0 specification
- Unit and integration tests

### **Tips for Success**

1. **Security First**: Always validate input and handle errors securely
2. **Use HTTPS**: Never transmit JWT tokens over HTTP in production
3. **Token Expiration**: Keep JWT token expiration times reasonable
4. **Password Policies**: Enforce strong password requirements
5. **Regular Updates**: Keep dependencies updated for security patches
6. **Monitoring**: Implement proper logging and monitoring in production
7. **Testing**: Write comprehensive tests for authentication flows

This JWT Authentication API provides a solid foundation for building secure, scalable authentication systems in .NET applications.
- **Microsoft.AspNetCore.Authentication.JwtBearer 8.0.11**: JWT token authentication
- **Entity Framework Core 8.0.11**: Object-Relational Mapping with SQLite
- **BCrypt.Net-Next 4.0.3**: Additional password security
- **Swashbuckle.AspNetCore 6.6.2**: API documentation with Swagger

### Key Security Features
- **Password Hashing**: Built-in secure password storage
- **Account Lockout**: Brute force protection
- **Role-Based Access Control**: Fine-grained authorization
- **JWT Token Security**: Stateless authentication with configurable expiration
- **Input Validation**: Comprehensive data validation

---

## **Build from Scratch - Complete Guide**

Follow these step-by-step instructions to create this JWT Authentication API from scratch.

### **Prerequisites for Building from Scratch**
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Entity Framework Core CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- Basic knowledge of C# and ASP.NET Core Web API

---

## **Phase 1: Project Foundation Setup**

### Step 1: Create New Web API Project
```bash
# Create a new Web API project
dotnet new webapi -n JWTAuthAPI
cd JWTAuthAPI

# Verify the project runs
dotnet run
```

### Step 2: Install Required NuGet Packages
```bash
# Microsoft Identity with Entity Framework - Core authentication system
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.11

# JWT Bearer authentication - For token-based authentication
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.11

# Entity Framework with SQLite - Database operations
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.11

# BCrypt for additional password security (optional, Identity has built-in hashing)
dotnet add package BCrypt.Net-Next --version 4.0.3

# Install Entity Framework CLI tools globally (if not already installed)
dotnet tool install --global dotnet-ef
```

Your `.csproj` file should now include:
```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.11" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.11" />
```

### Step 3: Create Project Folders
```bash
# Create necessary directories for organized code structure
mkdir Models
mkdir DTOs
mkdir Data
mkdir Services
```

---

## **Phase 2: Create Domain Models and DTOs**

### Step 4: Create ApplicationUser Model
Create `/Models/User.cs`:
```csharp
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthAPI.Models
{
    /// <summary>
    /// User entity extending IdentityUser to leverage Microsoft Identity's built-in features
    /// This gives us password hashing, email confirmation, lockout, and more out of the box
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Full name property for convenience
        /// Combines first and last name without storing redundant data
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
```

### Step 5: Create Data Transfer Objects
Create `/DTOs/AuthDTOs.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace JWTAuthAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for user registration
    /// </summary>
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data Transfer Object for user login
    /// </summary>
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response object for successful authentication
    /// </summary>
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public DateTime ExpiresAt { get; set; }
    }

    /// <summary>
    /// User profile information (without sensitive data)
    /// </summary>
    public class UserProfileDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
```

---

## **Phase 3: Database Configuration**

### Step 6: Create Database Context
Create `/Data/AuthDbContext.cs`:
```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JWTAuthAPI.Models;

namespace JWTAuthAPI.Data
{
    /// <summary>
    /// Database context using Microsoft Identity
    /// Inherits from IdentityDbContext to get all Identity tables automatically
    /// </summary>
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        /// <summary>
        /// Configure database relationships and seed initial data
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser properties
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FirstName).HasMaxLength(100);
                entity.Property(u => u.LastName).HasMaxLength(100);
            });

            // Seed default roles
            var adminRoleId = "2301D884-221A-4E7D-B509-0113DCC043E1";
            var userRoleId = "2301D884-221A-4E7D-B509-0113DCC043E2";
            var managerRoleId = "2301D884-221A-4E7D-B509-0113DCC043E3";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = managerRoleId
                }
            );

            // Create default admin user
            var adminUserId = "2301D884-221A-4E7D-B509-0113DCC043A1";
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@jwtauth.com",
                NormalizedUserName = "ADMIN@JWTAUTH.COM",
                Email = "admin@jwtauth.com",
                NormalizedEmail = "ADMIN@JWTAUTH.COM",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = adminUserId
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign admin role to default admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );
        }
    }
}
```

### Step 7: Configure Application Settings
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=jwtauth.db"
  },
  "JWT": {
    "SecretKey": "MyVerySecretKeyThatShouldBeAtLeast32CharactersLongForHMACSHA256ToWorkProperly",
    "Issuer": "JWTAuthAPI",
    "Audience": "JWTAuthAPI-Users",
    "ExpirationInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## **Phase 4: JWT Token Service**

### Step 8: Create JWT Token Service
Create `/Services/JwtTokenService.cs`:
```csharp
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTAuthAPI.Models;

namespace JWTAuthAPI.Services
{
    /// <summary>
    /// JWT Token Service - handles creating and validating JWT tokens
    /// </summary>
    public interface IJwtTokenService
    {
        string GenerateToken(ApplicationUser user, List<string> roles);
        ClaimsPrincipal? ValidateToken(string token);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Generate a JWT token for authenticated user
        /// </summary>
        public string GenerateToken(ApplicationUser user, List<string> roles)
        {
            try
            {
                // Create claims - information about the user
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("first_name", user.FirstName),
                    new Claim("last_name", user.LastName),
                    new Claim("full_name", user.FullName)
                };

                // Add role claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Get signing key
                var secretKey = _configuration["JWT:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Token expiration
                var expirationTime = DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["JWT:ExpirationInMinutes"] ?? "60"));

                // Create the token
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: expirationTime,
                    signingCredentials: credentials
                );

                _logger.LogInformation("JWT token generated successfully for user {Email}", user.Email);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user {Email}", user.Email);
                throw;
            }
        }

        /// <summary>
        /// Validate a JWT token and extract user information
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration["JWT:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured");
                var key = Encoding.UTF8.GetBytes(secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return null;
            }
        }
    }
}
```

---

## **Phase 5: Authentication Controller**

### Step 9: Create Authentication Controller
Create `/Controllers/AuthController.cs`:
```csharp
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
    /// Handles user registration, login, and profile management
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
        /// Register a new user account
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email is already registered." });
                }

                // Create new user
                var newUser = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    EmailConfirmed = true
                };

                // Identity handles password validation and hashing
                var result = await _userManager.CreateAsync(newUser, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { message = "Registration failed", errors });
                }

                // Assign default "User" role
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
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", loginDto.Email);
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                // Validate password with built-in security features
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed login attempt for user: {Email}", loginDto.Email);
                    
                    if (result.IsLockedOut)
                        return Unauthorized(new { message = "Account is locked out." });
                    
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                // Get user roles
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
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", loginDto.Email);
                return StatusCode(500, new { message = "Login failed due to server error" });
            }
        }

        /// <summary>
        /// Get current user profile information (requires authentication)
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
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

                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new UserProfileDTO
                {
                    Id = user.Id,
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
        /// Manager or Admin endpoint
        /// </summary>
        [HttpGet("manager-area")]
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult ManagerArea()
        {
            var userName = User.Identity?.Name;
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);
            
            return Ok(new { 
                message = "Welcome to the manager area!", 
                user = userName,
                roles = roles,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
```

---

## **Phase 6: Application Configuration**

### Step 10: Configure Program.cs
Replace the content of `Program.cs`:
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JWTAuthAPI.Data;
using JWTAuthAPI.Models;
using JWTAuthAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework with SQLite
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Microsoft Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password requirements
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // Lockout settings - protect against brute force attacks
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ?? 
                throw new InvalidOperationException("JWT SecretKey not configured"))),
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Register custom services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Configure Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "JWT Authentication API", Version = "v1" });
    
    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication middleware MUST come before Authorization
app.UseAuthentication(); // "Who are you?" - validates JWT tokens
app.UseAuthorization();  // "What can you do?" - checks roles and permissions

app.MapControllers();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
```

---

## **Phase 7: Build and Test**

### Step 11: Create Database Migration
```bash
# Add Entity Framework migration
dotnet ef migrations add InitialCreate

# Apply the migration to create the SQLite database
dotnet ef database update
```

### Step 12: Build and Run the Application
```bash
# Build the project to check for compilation errors
dotnet build

# Run the application
dotnet run
```

### Step 13: Test the API
1. **Navigate to `https://localhost:5001/swagger`** to access Swagger UI
2. **Test Registration**:
   - Use `/api/Auth/register` endpoint
   - Create a new user account
3. **Test Login**:
   - Use `/api/Auth/login` endpoint
   - Login with created credentials or default admin (`admin@jwtauth.com` / `Admin123!`)
   - Copy the returned JWT token
4. **Test Protected Endpoints**:
   - Click "Authorize" in Swagger UI
   - Enter: `Bearer [your-jwt-token]`
   - Test `/api/Auth/profile`, `/api/Auth/admin-only`, `/api/Auth/manager-area`

### Step 14: Create Test Files (Optional)
Create `test-endpoints.http` for easy testing:
```http
### Register a new user
POST https://localhost:5001/api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "User123!",
  "firstName": "John",
  "lastName": "Doe"
}

### Login
POST https://localhost:5001/api/Auth/login
Content-Type: application/json

{
  "email": "admin@jwtauth.com",
  "password": "Admin123!"
}

### Get profile (requires token)
GET https://localhost:5001/api/Auth/profile
Authorization: Bearer YOUR_JWT_TOKEN_HERE

### Admin only endpoint
GET https://localhost:5001/api/Auth/admin-only
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

---
   using System.ComponentModel.DataAnnotations;

   namespace JWTAuthAPI.Models
   {
       public class ApplicationUser : IdentityUser
       {
           [Required]
           [StringLength(50)]
           public string FirstName { get; set; } = string.Empty;

           [Required]
           [StringLength(50)]
           public string LastName { get; set; } = string.Empty;

           public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

           public string FullName => $"{FirstName} {LastName}".Trim();
       }
   }
   ```

   **Why extend IdentityUser?**
   - Gets all built-in properties (Email, Password, etc.)
   - Adds custom properties (FirstName, LastName)
   - Integrates seamlessly with Identity system

### Step 4: Create Data Transfer Objects (DTOs)

Create `DTOs/AuthDTOs.cs` with all required DTOs:

```csharp
using System.ComponentModel.DataAnnotations;

namespace JWTAuthAPI.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
    }

    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public DateTime ExpiresAt { get; set; }
    }

    public class UserProfileDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
```

**Why use DTOs?**
- Separates internal models from API contracts
- Provides data validation
- Controls what data is exposed to clients

### Step 5: Create the Database Context

Create `Data/AuthDbContext.cs`:

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JWTAuthAPI.Models;

namespace JWTAuthAPI.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed default roles
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();
            var managerRoleId = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                }
            );

            // Seed default admin user
            var adminUserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@jwtauth.com",
                NormalizedUserName = "ADMIN@JWTAUTH.COM",
                Email = "admin@jwtauth.com",
                NormalizedEmail = "ADMIN@JWTAUTH.COM",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                CreatedAt = DateTime.UtcNow
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");
            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign admin role to admin user
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );
        }
    }
}
```

**Why IdentityDbContext?**
- Automatically creates all Identity tables
- Handles user, role, and permission relationships
- Provides data seeding capabilities

### Step 6: Create the JWT Token Service

Create `Services/JwtTokenService.cs`:

```csharp
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTAuthAPI.Models;

namespace JWTAuthAPI.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user, IList<string> roles);
        ClaimsPrincipal? ValidateToken(string token);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not found");
            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? ""),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                new("fullName", user.FullName)
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            _logger.LogInformation("JWT token generated successfully for user {Email}", user.Email);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JWT");
                var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not found");
                var key = Encoding.UTF8.GetBytes(secretKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return null;
            }
        }
    }
}
```

**Why separate JWT service?**
- Single responsibility principle
- Easier to test and maintain
- Reusable across different controllers

### Step 7: Create the Authentication Controller

Create `Controllers/AuthController.cs`:

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JWTAuthAPI.DTOs;
using JWTAuthAPI.Models;
using JWTAuthAPI.Services;

namespace JWTAuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenService jwtTokenService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                    return Conflict(new { message = "User with this email already exists" });

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("User {Email} registered successfully", model.Email);
                    
                    return Ok(new { 
                        message = "User registered successfully",
                        email = user.Email,
                        fullName = user.FullName
                    });
                }

                return BadRequest(new { message = "User registration failed", errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new { message = "Registration failed" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Unauthorized(new { message = "Invalid email or password" });

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
                
                if (result.IsLockedOut)
                    return StatusCode(423, new { message = "Account is locked out" });

                if (!result.Succeeded)
                    return Unauthorized(new { message = "Invalid email or password" });

                var roles = await _userManager.GetRolesAsync(user);
                var token = await _jwtTokenService.GenerateTokenAsync(user, roles);

                _logger.LogInformation("User {Email} logged in successfully", model.Email);

                return Ok(new AuthResponseDTO
                {
                    Token = token,
                    Email = user.Email ?? "",
                    FullName = user.FullName,
                    Roles = roles.ToList(),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "Login failed" });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new UserProfileDTO
                {
                    Id = user.Id,
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
                    var roles = await _userManager.GetRolesAsync(user);
                    userProfiles.Add(new UserProfileDTO
                    {
                        Id = user.Id,
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
            {
                _logger.LogError(ex, "Error retrieving all users");
                return StatusCode(500, new { message = "Failed to retrieve users" });
            }
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                if (!await _roleManager.RoleExistsAsync(model.Role))
                    return BadRequest(new { message = "Role does not exist" });

                var result = await _userManager.AddToRoleAsync(user, model.Role);
                if (result.Succeeded)
                {
                    return Ok(new { message = $"Role {model.Role} assigned to user successfully" });
                }

                return BadRequest(new { message = "Failed to assign role", errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role");
                return StatusCode(500, new { message = "Failed to assign role" });
            }
        }

        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok(new { message = "You are authenticated!", user = User.Identity?.Name });
        }

        [HttpGet("test-admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdmin()
        {
            return Ok(new { message = "You have admin access!", user = User.Identity?.Name });
        }
    }

    public class AssignRoleDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
```

---

## **Phase 6: Application Configuration**

### Step 10: Configure Program.cs
Replace the content of `Program.cs`:
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using JWTAuthAPI.Data;
using JWTAuthAPI.Models;
using JWTAuthAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Authentication API",
        Description = "A comprehensive JWT authentication system with Microsoft Identity"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure Entity Framework
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password requirements
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // Lockout settings - protect against brute force attacks
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JWT");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not found in configuration");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// Register custom services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

---

## **Phase 7: Build and Test**

### Step 11: Create Database Migration
```bash
# Add Entity Framework migration
dotnet ef migrations add InitialCreate

# Apply the migration to create the SQLite database
dotnet ef database update
```

### Step 12: Build and Run the Application
```bash
# Build the project to check for compilation errors
dotnet build

# Run the application
dotnet run
```

### Step 13: Test the API
1. **Navigate to `https://localhost:5001/swagger`** to access Swagger UI
2. **Test Registration**:
   - Use `/api/Auth/register` endpoint
   - Create a new user account
3. **Test Login**:
   - Use `/api/Auth/login` endpoint
   - Login with created credentials or default admin (`admin@jwtauth.com` / `Admin123!`)
   - Copy the returned JWT token
4. **Test Protected Endpoints**:
   - Click "Authorize" in Swagger UI
   - Enter: `Bearer [your-jwt-token]`
   - Test `/api/Auth/profile`, `/api/Auth/admin-only`, `/api/Auth/manager-area`

### Step 14: Create Test Files (Optional)
Create `test-endpoints.http` for easy testing:
```http
### Register a new user
POST https://localhost:5001/api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "User123!",
  "firstName": "John",
  "lastName": "Doe"
}

### Login
POST https://localhost:5001/api/Auth/login
Content-Type: application/json

{
  "email": "admin@jwtauth.com",
  "password": "Admin123!"
}

### Get profile (requires token)
GET https://localhost:5001/api/Auth/profile
Authorization: Bearer YOUR_JWT_TOKEN_HERE

### Admin only endpoint
GET https://localhost:5001/api/Auth/admin-only
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

---

## **Why Use Microsoft Identity?**

Instead of building custom user management from scratch, this project leverages Microsoft Identity which provides:

### **Built-in Security Features**
- **Password Hashing**: Secure password storage using industry-standard algorithms
- **Account Lockout**: Protection against brute force attacks with configurable lockout policies
- **Email Confirmation**: Built-in email verification system
- **Password Reset**: Secure password recovery mechanisms
- **Two-Factor Authentication**: Ready-to-extend 2FA support
- **Security Stamps**: Automatic token invalidation on security changes

### **Enterprise-Grade User Management**
- **Role and Claims Management**: Flexible authorization system
- **User Profile Management**: Extensible user data storage
- **Audit Trail**: Built-in logging and security event tracking
- **Proven Security**: Battle-tested in production environments worldwide

### **Developer Benefits**
- **Reduced Development Time**: No need to implement user management from scratch
- **Security Best Practices**: Follows OWASP and industry security standards
- **Extensibility**: Easy to customize and extend for specific needs
- **Integration**: Seamless integration with ASP.NET Core and Entity Framework

---

## **Security Features Implemented**

### **Authentication Security**
- **JWT Token Security**: Stateless authentication with configurable expiration
- **Secure Token Validation**: Signature verification and expiration checking
- **Role-Based Authorization**: Fine-grained access control based on user roles
- **Input Validation**: Comprehensive data validation for all endpoints

### **Password Security**
```csharp
// Password policy configuration
options.Password.RequireDigit = true;
options.Password.RequiredLength = 6;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequireUppercase = true;
options.Password.RequireLowercase = true;
```

### **Account Protection**
```csharp
// Lockout policy to prevent brute force attacks
options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
options.Lockout.MaxFailedAccessAttempts = 5;
options.Lockout.AllowedForNewUsers = true;
```

---

## **Troubleshooting Common Issues**

### **JWT Token Issues**
**Problem**: JWT tokens not being validated or "401 Unauthorized" errors

**Solutions**:
```bash
# 1. Check JWT configuration in appsettings.json
# Ensure SecretKey is at least 32 characters for HMAC SHA256

# 2. Verify middleware order in Program.cs
app.UseAuthentication(); # Must come BEFORE UseAuthorization
app.UseAuthorization();

# 3. Check token format in requests
# Header: Authorization: Bearer [your-jwt-token]

# 4. Verify token hasn't expired
# Check ExpirationInMinutes setting
```

### **Database Issues**
**Problem**: Database not created or migration errors

**Solutions**:
```bash
# Delete database and recreate
rm jwtauth.db
dotnet ef database update

# Or reset migrations completely
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### **Identity Configuration Issues**
**Problem**: User registration fails or password validation errors

**Solutions**:
```csharp
// Ensure proper service registration in Program.cs
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();
```

---

## **API Endpoints Reference**

### **Authentication Endpoints**

#### Register New User
```http
POST /api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "User123!",
  "firstName": "John",
  "lastName": "Doe"
}
```

#### User Login
```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "User123!"
}
```

#### Get User Profile
```http
GET /api/Auth/profile
Authorization: Bearer [jwt-token]
```

#### Admin Only Endpoint
```http
GET /api/Auth/admin-only
Authorization: Bearer [admin-jwt-token]
```

---

## **What You've Learned**

By completing this tutorial, you've built a comprehensive JWT authentication API demonstrating:

**Microsoft Identity Integration**: Enterprise-grade user management system  
**JWT Authentication**: Stateless token-based authentication  
**Role-Based Authorization**: Fine-grained access control  
**Security Best Practices**: Password policies, account lockout, secure tokens  
**API Development**: Professional API design and documentation  
**Database Integration**: Entity Framework with Identity tables  
**Configuration Management**: Secure configuration practices  
**Error Handling**: Professional error responses and logging  

This foundation prepares you for building secure, scalable web APIs and understanding enterprise authentication patterns!

---

## **Next Steps for Learning**

### **Beginner Extensions**
- Add password reset functionality
- Implement email confirmation
- Create user profile update endpoints
- Add role management endpoints for admins

### **Intermediate Extensions**
- Implement refresh token functionality
- Add two-factor authentication (2FA)
- Create user management dashboard
- Add API rate limiting and throttling

### **Advanced Extensions**
- Implement OAuth2/OpenID Connect
- Add microservices authentication patterns
- Create custom claims and policies
- Implement audit logging and compliance features

### **Frontend Integration Example**
```javascript
// Example React integration
const login = async (email, password) => {
  const response = await fetch('/api/Auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  
  const data = await response.json();
  localStorage.setItem('token', data.token);
  return data;
};

const getProfile = async () => {
  const token = localStorage.getItem('token');
  const response = await fetch('/api/Auth/profile', {
    headers: { 'Authorization': `Bearer ${token}` }
  });
  
  return await response.json();
};
```

This JWT Authentication API project provides a solid foundation for understanding modern web API security patterns and Microsoft Identity integration!