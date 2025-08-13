# Todo List Web API - Complete Setup Guide

## Introduction

This project is a comprehensive **Todo List Management Web API** built with modern ASP.NET Core 8 technologies. It provides a secure, scalable RESTful API with JWT authentication, complete CRUD operations, advanced filtering, and comprehensive documentation.

### Technologies Used

- **ASP.NET Core 8** - Modern web framework for building APIs
- **Entity Framework Core 8** - Object-relational mapping (ORM) for data access
- **SQLite** - Lightweight, file-based database for development
- **ASP.NET Core Identity** - Authentication and authorization framework
- **JWT Bearer Authentication** - Secure token-based authentication
- **AutoMapper** - Object-to-object mapping library
- **FluentValidation** - Input validation library
- **Swagger/OpenAPI** - API documentation and testing interface

### Key Features

- **JWT Authentication** - Secure token-based user authentication
- **Role-based Authorization** - Admin and User roles with different permissions
- **Todo CRUD Operations** - Complete Create, Read, Update, Delete functionality
- **User Isolation** - Users can only access their own todos
- **Advanced Filtering** - Filter todos by completion status, priority, category
- **Pagination** - Efficient data loading with customizable page sizes
- **Input Validation** - Comprehensive validation using FluentValidation
- **Statistics API** - Analytics and insights about user's todos
- **Swagger Documentation** - Interactive API documentation with authentication
- **CORS Support** - Ready for frontend integration
- **Standardized Responses** - Consistent API response format
- **Database Seeding** - Automatic creation of default roles and users

## Prerequisites

Before starting, ensure you have the following software installed:

### Required Software

1. **.NET 8 SDK** (Latest version)
   - Download from: [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Verify installation: `dotnet --version`

2. **Visual Studio 2022** or **Visual Studio Code**
   - Visual Studio 2022: [https://visualstudio.microsoft.com/downloads/](https://visualstudio.microsoft.com/downloads/)
   - VS Code: [https://code.visualstudio.com/](https://code.visualstudio.com/)

3. **Git** (for version control)
   - Download from: [https://git-scm.com/downloads](https://git-scm.com/downloads)

### Optional but Recommended

- **Postman** or **Insomnia** for API testing
- **DB Browser for SQLite** for database inspection
- **REST Client extension** for VS Code (to use .http files)

## Installation & Setup

### Step 1: Create New Web API Project

Open your terminal or command prompt and execute the following commands:

```bash
# Create a new Web API project
dotnet new webapi -n TodoListAPI

# Navigate to the project directory
cd TodoListAPI

# Open in your preferred editor
code .  # For VS Code
# OR
start TodoListAPI.sln  # For Visual Studio
```

### Step 2: Install Required NuGet Packages

Add all necessary packages to your project:

```bash
# Entity Framework Core packages
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.7
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.7
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.7

# Identity packages
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.7

# JWT Authentication
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.7
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.0.1

# AutoMapper for object mapping
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

# FluentValidation for input validation
dotnet add package FluentValidation --version 11.9.2

# Swagger/OpenAPI documentation
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.11
```

### Step 3: Project Structure Setup

Create the following folder structure in your project:

```
TodoListAPI/
├── Controllers/
├── Data/
├── DTOs/
├── Models/
├── Services/
├── Repositories/
├── Validators/
├── MappingProfiles/
├── Migrations/
├── Properties/
├── appsettings.json
├── appsettings.Development.json
└── Program.cs
```

### Step 4: Database Configuration

#### A. Create Models

Create `Models/ApplicationUser.cs`:

```csharp
using Microsoft.AspNetCore.Identity;

namespace TodoListAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}
```

Create `Models/TodoItem.cs`:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListAPI.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        public DateTime? DueDate { get; set; }

        [Required]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High
        
        public string? Category { get; set; }

        // Foreign Key
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        // Computed property
        public bool IsOverdue => !IsCompleted && DueDate.HasValue && DueDate.Value < DateTime.UtcNow;

        // Navigation property
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
    }
}
```

#### B. Create Database Context

Create `Data/ApplicationDbContext.cs`:

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;

namespace TodoListAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure TodoItem
            builder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
                
                // Configure relationship
                entity.HasOne(t => t.User)
                      .WithMany(u => u.TodoItems)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Index for better performance
                entity.HasIndex(t => t.UserId);
                entity.HasIndex(t => t.IsCompleted);
                entity.HasIndex(t => t.CreatedAt);
            });

            // Configure ApplicationUser
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FirstName).HasMaxLength(100);
                entity.Property(u => u.LastName).HasMaxLength(100);
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("datetime('now')");
            });
        }
    }
}
```

#### C. Configure Connection String

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=todoapi.db"
  },
  "JwtSettings": {
    "Secret": "your-super-secret-key-that-is-at-least-256-bits-long-for-jwt-token-generation",
    "ExpirationInDays": 7
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

### Step 5: Create DTOs (Data Transfer Objects)

Create `DTOs/ApiResponseDto.cs`:

```csharp
namespace TodoListAPI.DTOs
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponseDto<T> SuccessResult(T data, string message = "Operation successful")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponseDto<T> ErrorResult(string message, List<string>? errors = null)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
```

Create `DTOs/TodoItemDto.cs`:

```csharp
namespace TodoListAPI.DTOs
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = "Medium";
        public string? Category { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        
        // Computed property
        public bool IsOverdue => !IsCompleted && DueDate.HasValue && DueDate.Value < DateTime.UtcNow;
    }
}
```

Create additional DTOs: `CreateTodoItemDto.cs`, `UpdateTodoItemDto.cs`, `RegisterDto.cs`, `LoginDto.cs`, `AuthResponseDto.cs`, etc.

### Step 6: Create Repositories

Create `Repositories/ITodoItemRepository.cs`:

```csharp
using TodoListAPI.Models;
using TodoListAPI.DTOs;

namespace TodoListAPI.Repositories
{
    public interface ITodoItemRepository
    {
        Task<TodoItem?> GetByIdAsync(int id);
        Task<TodoItem?> GetByIdWithUserAsync(int id);
        Task<List<TodoItem>> GetAllAsync();
        Task<List<TodoItem>> GetByUserIdAsync(string userId);
        Task<PaginatedResultDto<TodoItem>> GetFilteredAsync(TodoItemFilterDto filter, string? userId = null);
        Task<TodoItem> CreateAsync(TodoItem todoItem);
        Task<TodoItem> UpdateAsync(TodoItem todoItem);
        Task DeleteAsync(int id);
        Task<TodoStatsDto> GetStatsAsync(string userId);
    }
}
```

Create `Repositories/TodoItemRepository.cs` with implementation.

### Step 7: Create Services

Create `Services/IAuthService.cs`:

```csharp
using TodoListAPI.DTOs;

namespace TodoListAPI.Services
{
    public interface IAuthService
    {
        Task<ApiResponseDto<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponseDto<UserDto>> GetCurrentUserAsync(string userId);
        string GenerateJwtToken(ApplicationUser user, IList<string> roles);
    }
}
```

Create `Services/AuthService.cs` with JWT token generation logic.

### Step 8: Add Validation

Create `Validators/CreateTodoItemValidator.cs`:

```csharp
using FluentValidation;
using TodoListAPI.DTOs;

namespace TodoListAPI.Validators
{
    public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemDto>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .Length(3, 200).WithMessage("Title must be between 3 and 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.Priority)
                .Must(p => new[] { "Low", "Medium", "High" }.Contains(p))
                .WithMessage("Priority must be Low, Medium, or High");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.DueDate.HasValue)
                .WithMessage("Due date must be in the future");
        }
    }
}
```

### Step 9: Create AutoMapper Profiles

Create `MappingProfiles/TodoItemProfile.cs`:

```csharp
using AutoMapper;
using TodoListAPI.Models;
using TodoListAPI.DTOs;

namespace TodoListAPI.MappingProfiles
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : ""))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : ""));

            CreateMap<CreateTodoItemDto, TodoItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateTodoItemDto, TodoItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
```

### Step 10: Create Data Seeding

Create `Data/SeedData.cs`:

```csharp
using Microsoft.AspNetCore.Identity;
using TodoListAPI.Models;

namespace TodoListAPI.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles
            string[] roleNames = { "Admin", "User" };
            
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user
            var adminEmail = "admin@todoapi.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "API",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine($"Admin user created successfully with email: {adminEmail}");
                    Console.WriteLine("Default password: Admin123!");
                }
            }

            // Create sample user
            var userEmail = "user@todoapi.com";
            var sampleUser = await userManager.FindByEmailAsync(userEmail);
            
            if (sampleUser == null)
            {
                sampleUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "API",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(sampleUser, "User123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(sampleUser, "User");
                    Console.WriteLine($"Sample user created successfully with email: {userEmail}");
                    Console.WriteLine("Default password: User123!");
                }
            }
        }
    }
}
```

### Step 11: Configure Program.cs

Replace the content of `Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation;
using System.Text;
using System.Reflection;
using TodoListAPI.Data;
using TodoListAPI.Models;
using TodoListAPI.Services;
using TodoListAPI.Repositories;
using TodoListAPI.DTOs;
using TodoListAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "your-super-secret-key-that-is-at-least-256-bits-long");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add FluentValidation
builder.Services.AddScoped<IValidator<CreateTodoItemDto>, CreateTodoItemValidator>();
builder.Services.AddScoped<IValidator<UpdateTodoItemDto>, UpdateTodoItemValidator>();
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginValidator>();

// Add Repositories
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

// Add Services
builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add controllers
builder.Services.AddControllers();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Todo List API", 
        Version = "v1",
        Description = "A comprehensive Todo List Management API with authentication and CRUD operations"
    });
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo List API v1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

### Step 12: Create Controllers

Create `Controllers/AuthController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TodoListAPI.Services;
using TodoListAPI.DTOs;

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto<UserDto>>> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponseDto<UserDto>.ErrorResult("Invalid token"));
            }
            
            var result = await _authService.GetCurrentUserAsync(userId);
            return Ok(result);
        }
    }
}
```

Create `Controllers/TodosController.cs` with all CRUD operations.

### Step 13: Database Migration

Run the following commands to create and apply the database migration:

```bash
# Add initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update

# Build the project to ensure everything compiles
dotnet build
```

## Running the Project

### Start the API Server

```bash
# Run the application
dotnet run

# The API will be available at:
# HTTP: http://localhost:5000
# HTTPS: https://localhost:5001
# Swagger UI: http://localhost:5000/swagger
```

### Default Test Accounts

The application automatically creates default accounts for testing:

**Admin Account:**
- Email: `admin@todoapi.com`
- Password: `Admin123!`
- Role: Admin

**User Account:**
- Email: `user@todoapi.com`
- Password: `User123!`
- Role: User

## API Endpoints

### Authentication Endpoints

- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `GET /api/auth/me` - Get current user info

### Todo Endpoints

- `GET /api/todos` - Get all todos (with filtering and pagination)
- `GET /api/todos/{id}` - Get specific todo
- `POST /api/todos` - Create new todo
- `PUT /api/todos/{id}` - Update todo
- `DELETE /api/todos/{id}` - Delete todo
- `GET /api/todos/stats` - Get todo statistics

## Testing the API

### Using Swagger UI

1. Navigate to `http://localhost:5000/swagger`
2. Click "Authorize" button
3. Login using the Auth endpoints
4. Copy the JWT token from the response
5. Enter `Bearer YOUR_TOKEN` in the authorization field
6. Test all endpoints interactively

### Using HTTP Files

Create a `test-endpoints.http` file:

```http
### Login
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "email": "admin@todoapi.com",
  "password": "Admin123!"
}

### Create Todo
POST http://localhost:5000/api/todos
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "title": "Complete API Development",
  "description": "Finish implementing the Todo Web API",
  "priority": "High",
  "category": "Work",
  "dueDate": "2024-12-31T17:00:00Z"
}

### Get All Todos
GET http://localhost:5000/api/todos
Authorization: Bearer YOUR_JWT_TOKEN
```

## Production Deployment Considerations

### Security

- Change default JWT secret in production
- Use secure connection strings
- Enable HTTPS in production
- Configure proper CORS policies
- Use environment variables for secrets

### Database

- Consider using SQL Server or PostgreSQL for production
- Implement proper backup strategies
- Configure connection pooling
- Add database health checks

### Performance

- Add caching (Redis, In-Memory)
- Implement rate limiting
- Add compression middleware
- Consider API versioning


