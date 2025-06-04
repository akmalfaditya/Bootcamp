# JWT Authentication API with Microsoft Identity

A comprehensive JWT authentication system built with ASP.NET Core Web API and Microsoft Identity. This project demonstrates enterprise-grade authentication and authorization using industry best practices.

## What This Project Demonstrates

This isn't just another "hello world" authentication example. This is a production-ready authentication system that shows you:

- **Microsoft Identity Integration**: Using ASP.NET Core Identity for user management instead of rolling your own
- **JWT Token Authentication**: Stateless authentication perfect for APIs and microservices
- **Role-Based Authorization**: Protecting endpoints based on user roles
- **Security Best Practices**: Password hashing, account lockouts, and secure token validation
- **Entity Framework with SQLite**: Database operations with migrations
- **Comprehensive Error Handling**: Proper error responses and logging
- **API Documentation**: Swagger integration with JWT authentication support

## Why Use Microsoft Identity?

Instead of building custom user management from scratch, this project leverages Microsoft Identity which provides:

- ✅ Built-in password hashing and validation
- ✅ Account lockout protection against brute force attacks
- ✅ Email confirmation and password reset capabilities
- ✅ Two-factor authentication support (ready to extend)
- ✅ Role and claims management
- ✅ Security stamp validation
- ✅ Proven, battle-tested security implementations

## Project Architecture

```
JWTAuthAPI/
├── Controllers/
│   └── AuthController.cs          # Authentication endpoints
├── Data/
│   └── AuthDbContext.cs           # Database context with Identity
├── DTOs/
│   └── AuthDTOs.cs               # Data transfer objects
├── Models/
│   └── User.cs                   # ApplicationUser extending IdentityUser
├── Services/
│   └── JwtTokenService.cs        # JWT token generation and validation
├── Program.cs                    # Application configuration
├── appsettings.json             # Configuration including JWT settings
└── README.md                    # This file
```

## Step-by-Step Project Creation Guide

This section will walk you through creating this JWT Authentication API project from scratch, explaining each step and the reasoning behind it.

### Step 1: Create the Project Foundation

1. **Create a new Web API project**
   ```bash
   # Create a new directory for your project
   mkdir JWTAuthAPI
   cd JWTAuthAPI
   
   # Create a new Web API project
   dotnet new webapi -n JWTAuthAPI
   cd JWTAuthAPI
   ```

2. **Verify the project structure**
   ```bash
   # Check if the project was created successfully
   dotnet build
   dotnet run
   ```
   You should see the default WeatherForecast API running.

### Step 2: Install Required NuGet Packages

Install the necessary packages for JWT authentication and Microsoft Identity:

```bash
# Microsoft Identity with Entity Framework - Core authentication system
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.11

# JWT Bearer authentication - For token-based authentication
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.11

# Entity Framework with SQLite - Database operations
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.11

# BCrypt for password hashing (optional, Identity has built-in hashing)
dotnet add package BCrypt.Net-Next --version 4.0.3
```

**Why these packages?**
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`: Provides complete user management system
- `Microsoft.AspNetCore.Authentication.JwtBearer`: Handles JWT token validation
- `Microsoft.EntityFrameworkCore.Sqlite`: Lightweight database for development
- `BCrypt.Net-Next`: Additional password security (Identity already includes secure hashing)

### Step 3: Create the Data Models

1. **Create the ApplicationUser model** (`Models/User.cs`):
   ```csharp
   using Microsoft.AspNetCore.Identity;
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

### Step 8: Configure the Application

Update `Program.cs`:

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
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
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

### Step 9: Configure Application Settings

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=jwtauth.db"
  },
  "JWT": {
    "SecretKey": "your-very-secure-secret-key-here-make-it-long-and-complex",
    "Issuer": "JWTAuthAPI",
    "Audience": "JWTAuthAPIUsers",
    "ExpirationInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Step 10: Create and Apply Database Migrations

```bash
# Create the initial migration
dotnet ef migrations add InitialCreate

# Apply the migration to create the database
dotnet ef database update
```

### Step 11: Create Test Files

Create `test-endpoints.http` for manual testing:

```http
### 1. Register a new user
POST http://localhost:5195/api/auth/register
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "StrongPassword123!",
  "firstName": "John",
  "lastName": "Doe"
}

### 2. Login with the registered user
POST http://localhost:5195/api/auth/login
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "StrongPassword123!"
}

### 3. Get user profile (requires authentication)
GET http://localhost:5195/api/auth/profile
Authorization: Bearer {{token}}

### 4. Get all users (Admin only)
GET http://localhost:5195/api/auth/users
Authorization: Bearer {{admin_token}}
```

### Step 12: Test the Application

```bash
# Run the application
dotnet run

# The API will be available at:
# - http://localhost:5195
# - Swagger UI: http://localhost:5195/swagger
```

**Default admin credentials for testing:**
- Email: `admin@jwtauth.com`
- Password: `Admin123!`

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code
- Basic understanding of C# and REST APIs

### Step-by-Step Setup

1. **Clone or Create the Project**
   ```bash
   # If starting fresh
   dotnet new webapi -n JWTAuthAPI
   cd JWTAuthAPI
   ```

2. **Install Required Packages**
   ```bash
   # Microsoft Identity with Entity Framework
   dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.11
   
   # JWT Bearer authentication
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.11
   
   # Entity Framework with SQLite
   dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.11
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.11
   ```

3. **Configure Your Database Connection**
   
   Update `appsettings.json` with your database connection:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=jwtauth.db"
     },
     "JWT": {
       "SecretKey": "YourVerySecretKeyHere",
       "Issuer": "YourAppName",
       "Audience": "YourAppUsers",
       "ExpirationInMinutes": 60
     }
   }
   ```

4. **Create and Run Database Migrations**
   ```bash
   # Create initial migration
   dotnet ef migrations add InitialCreate
   
   # Apply migration to database
   dotnet ef database update
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

6. **Test the API**
   
   Open your browser to `https://localhost:7xxx/swagger` to see the interactive API documentation.

## API Endpoints

### Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register a new user | No |
| POST | `/api/auth/login` | Login and get JWT token | No |
| GET | `/api/auth/profile` | Get current user profile | Yes |
| GET | `/api/auth/admin-only` | Admin-only test endpoint | Yes (Admin role) |
| GET | `/api/auth/users` | Get all users | Yes (Admin role) |

### Example Usage

**1. Register a New User:**
```bash
POST /api/auth/register
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "StrongPassword123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**2. Login:**
```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "StrongPassword123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "john@example.com",
  "fullName": "John Doe",
  "roles": ["User"],
  "expiresAt": "2025-06-04T11:30:00Z"
}
```

**3. Access Protected Endpoint:**
```bash
GET /api/auth/profile
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Key Features Explained

### Microsoft Identity Integration

This project uses `IdentityDbContext` instead of regular `DbContext`, which automatically provides:
- User management tables (Users, Roles, UserRoles, etc.)
- Password hashing with built-in security
- Account lockout mechanisms
- Role-based access control

### JWT Token Structure

The JWT tokens include these claims:
- `sub`: User ID
- `email`: User's email address
- `role`: User roles for authorization
- `exp`: Token expiration time
- Custom claims: first name, last name, full name

### Security Features

1. **Password Requirements**: Configurable in `Program.cs`
2. **Account Lockout**: Protects against brute force attacks
3. **Token Validation**: Comprehensive JWT validation
4. **Role-Based Authorization**: Different access levels
5. **HTTPS Enforcement**: Secure communication

### Database Seeding

The application automatically creates:
- Default roles: Admin, User, Manager
- Default admin user: admin@jwtauth.com / Admin123!

## Testing the API

### Using Swagger UI

1. Run the application: `dotnet run`
2. Open browser to `http://localhost:5195/swagger`
3. Test endpoints directly in the browser interface

### Using the Test File

The project includes `test-endpoints.http` file with sample requests:

1. **Register a new user** - Test user registration
2. **Login** - Get your JWT token
3. **Access protected endpoints** - Use the token in Authorization header
4. **Test admin functions** - Use admin credentials

### Default Admin Account

For testing admin features:
- **Email**: `admin@jwtauth.com`
- **Password**: `Admin123!`

### Step-by-Step Testing Guide

1. **Start the application**
   ```bash
   dotnet run
   ```

2. **Test user registration**
   ```bash
   POST http://localhost:5195/api/auth/register
   {
     "email": "test@example.com",
     "password": "TestPassword123!",
     "firstName": "Test",
     "lastName": "User"
   }
   ```

3. **Login and get token**
   ```bash
   POST http://localhost:5195/api/auth/login
   {
     "email": "test@example.com",
     "password": "TestPassword123!"
   }
   ```

4. **Use token for protected endpoints**
   ```bash
   GET http://localhost:5195/api/auth/profile
   Authorization: Bearer YOUR_TOKEN_HERE
   ```

## Project Structure

```
JWTAuthAPI/
├── Controllers/
│   └── AuthController.cs          # Authentication endpoints
├── Data/
│   └── AuthDbContext.cs           # Database context with Identity
├── DTOs/
│   └── AuthDTOs.cs               # Data transfer objects
├── Models/
│   └── User.cs                   # ApplicationUser model
├── Services/
│   └── JwtTokenService.cs        # JWT token generation/validation
├── Program.cs                    # App configuration
├── appsettings.json             # Configuration settings
└── test-endpoints.http          # API testing file
```

## Troubleshooting

### Common Issues

1. **Database Connection Errors**
   - Ensure SQLite package is installed
   - Check connection string in appsettings.json
   - Run `dotnet ef database update`

2. **JWT Token Issues**
   - Verify JWT configuration in appsettings.json
   - Check token expiration time
   - Ensure Bearer token format: `Bearer YOUR_TOKEN`

3. **Migration Errors**
   ```bash
   # Remove and recreate migrations
   dotnet ef migrations remove
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Package Version Conflicts**
   - Ensure all Entity Framework packages use same version (8.0.11)
   - Clear NuGet cache: `dotnet nuget locals all --clear`

### Performance Tips

1. **Token Caching**: Consider caching tokens for better performance
2. **Database Indexing**: Add indexes on frequently queried fields
3. **Logging**: Use structured logging for production monitoring

## Production Deployment

### Security Checklist

- [ ] Change default JWT secret key
- [ ] Use strong passwords for admin accounts
- [ ] Enable HTTPS in production
- [ ] Configure proper CORS policies
- [ ] Set up proper logging and monitoring
- [ ] Use secure database connection strings
- [ ] Implement rate limiting for API endpoints

### Environment Configuration

Create separate `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your-Production-Database-URL"
  },
  "JWT": {
    "SecretKey": "Your-Very-Secure-Production-Key-Here",
    "Issuer": "YourProductionApp",
    "Audience": "ProductionUsers",
    "ExpirationInMinutes": 30
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

## Advanced Features & Extensibility

### Adding Two-Factor Authentication (2FA)

To extend this project with 2FA support:

```csharp
// In your controller, add 2FA endpoints
[HttpPost("enable-2fa")]
[Authorize]
public async Task<IActionResult> EnableTwoFactorAuthentication()
{
    var user = await GetCurrentUserAsync();
    var key = await _userManager.GetAuthenticatorKeyAsync(user);
    
    if (string.IsNullOrEmpty(key))
    {
        await _userManager.ResetAuthenticatorKeyAsync(user);
        key = await _userManager.GetAuthenticatorKeyAsync(user);
    }
    
    return Ok(new { qrCodeKey = key });
}
```

### Email Confirmation

Add email confirmation by integrating with an email service:

```csharp
// Configure email service in Program.cs
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});

// Add email service
builder.Services.AddTransient<IEmailSender, EmailSender>();
```

### Refresh Token Implementation

For long-lived authentication, implement refresh tokens:

```csharp
public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public string UserId { get; set; } = string.Empty;
}
```

## Best Practices Implemented

### 1. **Separation of Concerns**
- Controllers handle HTTP requests/responses
- Services handle business logic
- DTOs separate API contracts from internal models
- Repository pattern through Entity Framework

### 2. **Security Best Practices**
- Password hashing with Identity's built-in security
- JWT token validation with proper claims
- Role-based authorization
- Account lockout protection
- HTTPS enforcement

### 3. **Error Handling**
- Comprehensive try-catch blocks
- Structured logging
- Consistent error response format
- Input validation with model attributes

### 4. **Code Organization**
- Clean architecture principles
- Dependency injection
- Interface-based design
- Async/await patterns

### 5. **Testing Strategy**
- HTTP test files for manual testing
- PowerShell automation scripts
- Swagger integration for interactive testing
- Clear test scenarios and expected outcomes

## Learning Resources

### Understanding JWT Tokens
- **What is JWT?** JSON Web Tokens are a secure way to transmit information between parties
- **Why use JWT?** Stateless authentication perfect for APIs and microservices
- **Token Structure:** Header.Payload.Signature format
- **Claims:** Information stored in the token (user ID, roles, expiration)

### Microsoft Identity Framework
- **UserManager:** Handles user operations (create, find, update)
- **SignInManager:** Manages user sign-in operations
- **RoleManager:** Manages application roles
- **IdentityDbContext:** Database context with built-in Identity tables

### Entity Framework Patterns
- **Code-First Migrations:** Database schema from code models
- **Data Seeding:** Pre-populating database with default data
- **Dependency Injection:** Service registration and resolution

## Deployment Options

### 1. **Azure App Service**
```bash
# Deploy to Azure
az webapp up --sku F1 --name your-jwt-api-name
```

### 2. **Docker Containerization**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "JWTAuthAPI.dll"]
```

### 3. **IIS Deployment**
- Publish as Framework-dependent deployment
- Configure IIS with ASP.NET Core Module
- Set up HTTPS certificates

## Common Extensions

### Rate Limiting
```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("AuthPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
    });
});
```

### API Versioning
```csharp
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});
```

### Caching
```csharp
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});
```

## Performance Considerations

### 1. **Database Optimization**
- Index frequently queried fields (Email, Username)
- Use async operations for all database calls
- Consider connection pooling for high-traffic applications

### 2. **Token Management**
- Keep JWT payload minimal
- Consider shorter expiration times with refresh tokens
- Implement token blacklisting for logout functionality

### 3. **Caching Strategy**
- Cache user roles and permissions
- Use distributed caching for multi-instance deployments
- Implement cache invalidation strategies

## Monitoring & Observability

### Application Insights Integration
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

### Health Checks
```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AuthDbContext>();
```

### Structured Logging
```csharp
builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
    builder.AddApplicationInsights();
});
```

## Summary

This JWT Authentication API provides:

✅ **Complete user authentication system** using Microsoft Identity
✅ **JWT token-based authentication** with secure token generation
✅ **Role-based authorization** (Admin, User, Manager roles)
✅ **SQLite database** with automatic migrations
✅ **Comprehensive API documentation** with Swagger
✅ **Security best practices** including password policies and account lockout
✅ **Ready-to-use endpoints** for registration, login, and user management
✅ **Production-ready configuration** with proper error handling
✅ **Extensible architecture** ready for additional features
✅ **Comprehensive documentation** with step-by-step creation guide

The project demonstrates modern .NET 8 authentication patterns and can be easily extended or integrated into larger applications. Whether you're building a small API or an enterprise-scale application, this foundation provides the security and scalability you need.



This project serves as both a learning resource and a solid foundation for building secure .NET APIs.
