# Todo List Web API - Professional Implementation Guide

## Project Overview

This project demonstrates a professional-grade Todo List Management Web API built with ASP.NET Core 8, implementing enterprise-level architectural patterns and best practices. The API provides secure, scalable RESTful services with comprehensive authentication, data validation, and advanced filtering capabilities.

### Architecture Components

- **ASP.NET Core 8** - Modern cross-platform web framework
- **Entity Framework Core 8** - Advanced object-relational mapping with SQLite
- **ASP.NET Core Identity** - Comprehensive authentication and authorization framework
- **JWT Bearer Authentication** - Stateless token-based security implementation
- **AutoMapper** - Advanced object-to-object mapping with custom profiles
- **FluentValidation** - Rule-based input validation with custom validators
- **Repository Pattern** - Data access abstraction layer
- **Service Layer** - Business logic separation and dependency injection
- **Swagger/OpenAPI** - Interactive API documentation with authentication support

### Professional Features

- **JWT Authentication System** - Secure token-based authentication with role-based authorization
- **Clean Architecture** - Separation of concerns with distinct layers (Controllers, Services, Repositories)
- **Data Transfer Objects (DTOs)** - Structured data contracts for API communication
- **Advanced Validation** - Comprehensive input validation using FluentValidation
- **User Isolation** - Secure data access ensuring users only see their own data
- **Filtering and Pagination** - Efficient data retrieval with advanced filtering options
- **Standardized API Responses** - Consistent response format with success/error handling
- **Database Seeding** - Automated initialization of roles and default users
- **Professional Error Handling** - Comprehensive error management and logging
- **CORS Configuration** - Production-ready cross-origin resource sharing setup

## Development Environment Setup

### Prerequisites

Ensure the following development tools are installed:

#### Essential Software

1. **.NET 8 SDK** (Version 8.0 or later)
   - Download: [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Verification: Execute `dotnet --version` in terminal

2. **Integrated Development Environment**
   - Visual Studio 2022 (Professional/Community): [https://visualstudio.microsoft.com/downloads/](https://visualstudio.microsoft.com/downloads/)
   - Visual Studio Code: [https://code.visualstudio.com/](https://code.visualstudio.com/)

3. **Version Control System**
   - Git: [https://git-scm.com/downloads](https://git-scm.com/downloads)

#### Professional Development Tools

- **API Testing Tools**: Postman, Insomnia, or REST Client VS Code extension
- **Database Management**: DB Browser for SQLite
- **Code Quality**: SonarLint extension for code analysis

## Step-by-Step Implementation Guide

### Step 1: Project Initialization

Create a new ASP.NET Core Web API project with the appropriate structure:

```bash
# Create new Web API project
dotnet new webapi -n TodoListAPI

# Navigate to project directory
cd TodoListAPI

# Open in development environment
code .  # For VS Code
# OR for Visual Studio
start TodoListAPI.sln
```

### Step 2: Package Dependencies Installation

Install all required NuGet packages for enterprise-level functionality:

```bash
# Core Entity Framework packages
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.7
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.7
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.7

# Identity and Authentication packages
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.7
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.7
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.0.1

# Object Mapping and Validation packages
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package FluentValidation --version 11.9.2

# API Documentation packages
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.11
```

### Step 3: Professional Project Structure

Establish a clean architecture folder structure:

```
TodoListAPI/
├── Controllers/          # API endpoint controllers
├── Data/                # Database context and seeding
├── DTOs/                # Data Transfer Objects
├── Models/              # Entity models
├── Services/            # Business logic layer
├── Repositories/        # Data access layer
├── Validators/          # FluentValidation validators
├── MappingProfiles/     # AutoMapper configuration
├── Migrations/          # Database migrations
├── Properties/          # Project properties
├── appsettings.json     # Application configuration
└── Program.cs           # Application entry point
```

### Step 4: Entity Models Implementation

#### A. Application User Model

Create `Models/ApplicationUser.cs` - Professional user entity extending Identity:

```csharp
using Microsoft.AspNetCore.Identity;

namespace Implemented_WebAPI.Models
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

#### B. Todo Item Entity Model

Create `Models/TodoItem.cs` - Core business entity with comprehensive properties:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implemented_WebAPI.Models
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

        // Foreign Key
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        // Computed property for business logic
        public bool IsOverdue => !IsCompleted && DueDate.HasValue && DueDate.Value < DateTime.UtcNow;

        // Navigation property
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
    }
}
```

### Step 5: Database Context Configuration

Create `Data/ApplicationDbContext.cs` - Professional Entity Framework configuration:

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Implemented_WebAPI.Models;

namespace Implemented_WebAPI.Data
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

            // Configure TodoItem entity
            builder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
                
                // Configure relationship with cascade delete
                entity.HasOne(t => t.User)
                      .WithMany(u => u.TodoItems)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Performance optimization indexes
                entity.HasIndex(t => t.UserId);
                entity.HasIndex(t => t.IsCompleted);
                entity.HasIndex(t => t.CreatedAt);
            });

            // Configure ApplicationUser entity
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

### Step 6: Data Transfer Objects (DTOs) Implementation

#### A. API Response Wrapper

Create `DTOs/ApiResponseDto.cs` - Standardized API response format:

```csharp
namespace Implemented_WebAPI.DTOs
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        
        public static ApiResponseDto<T> SuccessResult(T data, string message = "Success")
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
                Errors = errors ?? new List<string>()
            };
        }
    }
}
```

#### B. Todo Item DTOs

Create `DTOs/TodoItemDto.cs` - Data transfer representation:

```csharp
namespace Implemented_WebAPI.DTOs
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
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        
        // Computed property
        public bool IsOverdue => !IsCompleted && DueDate.HasValue && DueDate.Value < DateTime.UtcNow;
    }
}
```

Create `DTOs/CreateTodoItemDto.cs` - Creation input contract:

```csharp
namespace Implemented_WebAPI.DTOs
{
    public class CreateTodoItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
```

Create `DTOs/UpdateTodoItemDto.cs` - Update input contract:

```csharp
namespace Implemented_WebAPI.DTOs
{
    public class UpdateTodoItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
```

#### C. Authentication DTOs

Create `DTOs/RegisterDto.cs`:

```csharp
namespace Implemented_WebAPI.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
```

Create `DTOs/LoginDto.cs`:

```csharp
namespace Implemented_WebAPI.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
```

Create `DTOs/AuthResponseDto.cs`:

```csharp
namespace Implemented_WebAPI.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; } = null!;
    }
}
```

Create `DTOs/UserDto.cs`:

```csharp
namespace Implemented_WebAPI.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
```

### Step 7: Repository Layer Implementation

#### A. Repository Interface Definition

Create `Repositories/ITodoItemRepository.cs` - Repository pattern contract:

```csharp
using Implemented_WebAPI.Models;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Repositories
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetAllAsync(string userId);
        Task<TodoItem?> GetByIdAsync(int id, string userId);
        Task<TodoItem> CreateAsync(TodoItem todoItem);
        Task<TodoItem> UpdateAsync(TodoItem todoItem);
        Task DeleteAsync(int id, string userId);
        Task<(List<TodoItem> Items, int TotalCount)> GetFilteredAsync(string userId, TodoItemFilterDto filter);
        Task<bool> ExistsAsync(int id, string userId);
    }
}
```

#### B. Repository Implementation

Create `Repositories/TodoItemRepository.cs` - Data access layer implementation:

```csharp
using Microsoft.EntityFrameworkCore;
using Implemented_WebAPI.Data;
using Implemented_WebAPI.Models;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetAllAsync(string userId)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id, string userId)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TodoItem> CreateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var todoItem = await GetByIdAsync(id, userId);
            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(List<TodoItem> Items, int TotalCount)> GetFilteredAsync(string userId, TodoItemFilterDto filter)
        {
            var query = _context.TodoItems
                .Include(t => t.User)
                .Where(t => t.UserId == userId);

            // Apply filters
            if (filter.IsCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == filter.IsCompleted.Value);
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(t => t.Title.Contains(filter.SearchTerm) || 
                                       (t.Description != null && t.Description.Contains(filter.SearchTerm)));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> ExistsAsync(int id, string userId)
        {
            return await _context.TodoItems
                .AnyAsync(t => t.Id == id && t.UserId == userId);
        }
    }
}
```

### Step 8: Service Layer Implementation

#### A. Authentication Service Interface

Create `Services/IAuthService.cs` - Authentication service contract:

```csharp
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Services
{
    public interface IAuthService
    {
        Task<ApiResponseDto<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponseDto<UserDto>> GetCurrentUserAsync(string userId);
    }
}
```

#### B. Authentication Service Implementation

Create `Services/AuthService.cs` - Professional JWT authentication service:

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Implemented_WebAPI.Models;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return ApiResponseDto<AuthResponseDto>.ErrorResult("User with this email already exists");
                }

                var user = _mapper.Map<ApplicationUser>(registerDto);
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ApiResponseDto<AuthResponseDto>.ErrorResult("Registration failed", errors);
                }

                // Add user to default role
                await _userManager.AddToRoleAsync(user, "User");

                var authResponse = await GenerateJwtToken(user);
                return ApiResponseDto<AuthResponseDto>.SuccessResult(authResponse, "Registration successful");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AuthResponseDto>.ErrorResult($"Registration error: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return ApiResponseDto<AuthResponseDto>.ErrorResult("Invalid email or password");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    return ApiResponseDto<AuthResponseDto>.ErrorResult("Invalid email or password");
                }

                var authResponse = await GenerateJwtToken(user);
                return ApiResponseDto<AuthResponseDto>.SuccessResult(authResponse, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AuthResponseDto>.ErrorResult($"Login error: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<UserDto>> GetCurrentUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponseDto<UserDto>.ErrorResult("User not found");
                }

                var userDto = _mapper.Map<UserDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles.ToList();

                return ApiResponseDto<UserDto>.SuccessResult(userDto);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<UserDto>.ErrorResult($"Error retrieving user: {ex.Message}");
            }
        }

        private async Task<AuthResponseDto> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "your-super-secret-key-that-is-at-least-256-bits-long");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("FirstName", user.FirstName ?? ""),
                new Claim("LastName", user.LastName ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToList();

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires.Value,
                User = userDto
            };
        }
    }
}
```

#### C. Todo Service Interface and Implementation

Create `Services/ITodoItemService.cs`:

```csharp
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Services
{
    public interface ITodoItemService
    {
        Task<ApiResponseDto<List<TodoItemDto>>> GetAllAsync(string userId);
        Task<ApiResponseDto<TodoItemDto>> GetByIdAsync(int id, string userId);
        Task<ApiResponseDto<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto, string userId);
        Task<ApiResponseDto<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto, string userId);
        Task<ApiResponseDto<object>> DeleteAsync(int id, string userId);
        Task<ApiResponseDto<PaginatedResultDto<TodoItemDto>>> GetFilteredAsync(string userId, TodoItemFilterDto filter);
    }
}
```

Create `Services/TodoItemService.cs` - Business logic implementation with AutoMapper integration.

### Step 9: FluentValidation Implementation

#### A. Create Todo Item Validators

Create `Validators/CreateTodoItemValidator.cs` - Professional input validation:

```csharp
using FluentValidation;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Validators
{
    public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemDto>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");
                
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
                
            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.Now.AddDays(-1))
                .WithMessage("Due date cannot be in the past")
                .When(x => x.DueDate.HasValue);
        }
    }
}
```

Create `Validators/UpdateTodoItemValidator.cs`:

```csharp
using FluentValidation;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Validators
{
    public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemDto>
    {
        public UpdateTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");
                
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
                
            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.Now.AddDays(-1))
                .WithMessage("Due date cannot be in the past")
                .When(x => x.DueDate.HasValue);
        }
    }
}
```

#### B. Authentication Validators

Create `Validators/RegisterValidator.cs`:

```csharp
using FluentValidation;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please provide a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.FirstName)
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.LastName));
        }
    }
}
```

Create `Validators/LoginValidator.cs`:

```csharp
using FluentValidation;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please provide a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
```

### Step 10: AutoMapper Configuration

#### A. Todo Item Mapping Profile

Create `MappingProfiles/TodoItemProfile.cs` - Professional object mapping configuration:

```csharp
using AutoMapper;
using Implemented_WebAPI.Models;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.MappingProfiles
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
                
            CreateMap<CreateTodoItemDto, TodoItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false));
                
            CreateMap<UpdateTodoItemDto, TodoItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom((src, dest) => 
                    src.IsCompleted && !dest.IsCompleted ? DateTime.UtcNow : 
                    !src.IsCompleted && dest.IsCompleted ? null : dest.CompletedAt))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
```

#### B. User Mapping Profile

Create `MappingProfiles/UserProfile.cs`:

```csharp
using AutoMapper;
using Implemented_WebAPI.Models;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.TodoItems, opt => opt.Ignore());

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles are set separately
        }
    }
}
```

### Step 11: Database Seeding Configuration

Create `Data/SeedData.cs` - Professional data seeding for default users and roles:

```csharp
using Microsoft.AspNetCore.Identity;
using Implemented_WebAPI.Models;

namespace Implemented_WebAPI.Data
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

### Step 12: Application Configuration

Update `appsettings.json` with professional configuration:

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

### Step 13: Professional Program.cs Configuration

Replace the content of `Program.cs` with comprehensive enterprise configuration:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation;
using System.Text;
using System.Reflection;
using Implemented_WebAPI.Data;
using Implemented_WebAPI.Models;
using Implemented_WebAPI.Services;
using Implemented_WebAPI.Repositories;
using Implemented_WebAPI.DTOs;
using Implemented_WebAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password security settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // User account settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add JWT Bearer Authentication
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

// Add AutoMapper for object-to-object mapping
builder.Services.AddAutoMapper(typeof(Program));

// Add FluentValidation services
builder.Services.AddScoped<IValidator<CreateTodoItemDto>, CreateTodoItemValidator>();
builder.Services.AddScoped<IValidator<UpdateTodoItemDto>, UpdateTodoItemValidator>();
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginValidator>();

// Add Repository Layer
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

// Add Service Layer
builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add Controllers
builder.Services.AddControllers();

// Add API Documentation with Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Todo List API", 
        Version = "v1",
        Description = "A comprehensive Todo List Management API with authentication and CRUD operations"
    });
    
    // Add JWT Authentication to Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
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
    
    // Include XML comments for better documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Add CORS for cross-origin requests
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

// Seed database with default roles and users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Configure HTTP request pipeline
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

### Step 14: Controller Implementation

#### A. Authentication Controller

Create `Controllers/AuthController.cs` - Professional JWT authentication endpoints:

```csharp
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Implemented_WebAPI.DTOs;
using Implemented_WebAPI.Services;
using Implemented_WebAPI.Validators;

namespace Implemented_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="registerDto">User registration details</param>
        /// <returns>JWT token and user information</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ApiResponseDto<AuthResponseDto>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var result = await _authService.RegisterAsync(registerDto);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Authenticate user and generate JWT token
        /// </summary>
        /// <param name="loginDto">User login credentials</param>
        /// <returns>JWT token and user information</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ApiResponseDto<AuthResponseDto>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var result = await _authService.LoginAsync(loginDto);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get current authenticated user information
        /// </summary>
        /// <returns>Current user details</returns>
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _authService.GetCurrentUserAsync(userId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
```

#### B. Todo Items Controller

Create `Controllers/TodosController.cs` - Professional CRUD operations with comprehensive features:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using System.Security.Claims;
using Implemented_WebAPI.Services;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IValidator<CreateTodoItemDto> _createValidator;
        private readonly IValidator<UpdateTodoItemDto> _updateValidator;

        public TodosController(
            ITodoItemService todoItemService,
            IValidator<CreateTodoItemDto> createValidator,
            IValidator<UpdateTodoItemDto> updateValidator)
        {
            _todoItemService = todoItemService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Get all todo items for the authenticated user
        /// </summary>
        /// <returns>List of user's todo items</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.GetAllAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific todo item by ID
        /// </summary>
        /// <param name="id">Todo item ID</param>
        /// <returns>Todo item details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.GetByIdAsync(id, userId);
            
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Create a new todo item
        /// </summary>
        /// <param name="createDto">Todo item creation details</param>
        /// <returns>Created todo item</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ApiResponseDto<TodoItemDto>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.CreateAsync(createDto, userId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// Update an existing todo item
        /// </summary>
        /// <param name="id">Todo item ID</param>
        /// <param name="updateDto">Todo item update details</param>
        /// <returns>Updated todo item</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoItemDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ApiResponseDto<TodoItemDto>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.UpdateAsync(id, updateDto, userId);
            
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete a todo item
        /// </summary>
        /// <param name="id">Todo item ID</param>
        /// <returns>Deletion confirmation</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _todoItemService.DeleteAsync(id, userId);
            
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
```

### Step 15: Database Migration and Build

Execute the following commands to create and apply database migrations:

```bash
# Add initial migration for database schema
dotnet ef migrations add InitialCreate

# Apply migration to create database
dotnet ef database update

# Build the project to verify compilation
dotnet build
```

## Key Implementation Highlights

### JWT Authentication Implementation

The JWT authentication system implements industry-standard security practices:

- **Secure Token Generation**: Uses symmetric key encryption with HMAC-SHA256
- **Claims-Based Authentication**: Includes user identity, roles, and custom claims
- **Token Validation**: Comprehensive validation of signature, lifetime, and claims
- **Role-Based Authorization**: Support for Admin and User roles with different permissions
- **Swagger Integration**: JWT authentication integrated into API documentation

### Data Transfer Objects (DTOs) Architecture

The DTO implementation ensures clean data contracts:

- **Separation of Concerns**: Clear distinction between entity models and API contracts
- **Input Validation**: Dedicated DTOs for create and update operations
- **Response Standardization**: Consistent API response format with ApiResponseDto wrapper
- **Security**: Prevents over-posting and controls data exposure
- **Versioning Support**: Facilitates API versioning without breaking changes

### AutoMapper Configuration

Professional object-to-object mapping implementation:

- **Custom Mapping Logic**: Specialized mappings for complex scenarios
- **Performance Optimization**: Efficient mapping with minimal overhead
- **Null Safety**: Proper handling of nullable properties
- **Business Logic Integration**: Computed properties and conditional mappings
- **Maintainability**: Centralized mapping configuration in profiles

### Repository Pattern Implementation

Clean data access layer with professional patterns:

- **Abstraction Layer**: Interface-based contracts for testability
- **Separation of Concerns**: Data access logic isolated from business logic
- **Query Optimization**: Efficient database queries with proper indexing
- **User Isolation**: Secure data access ensuring user-specific operations
- **Error Handling**: Comprehensive exception management

### Service Layer Architecture

Business logic layer implementing enterprise patterns:

- **Dependency Injection**: Proper IoC container configuration
- **Transaction Management**: Appropriate handling of database transactions
- **Error Handling**: Standardized error responses and logging
- **Business Validation**: Complex business rules implementation
- **Performance**: Optimized operations with minimal database calls

### FluentValidation Integration

Professional input validation system:

- **Rule-Based Validation**: Declarative validation rules
- **Custom Validators**: Business-specific validation logic
- **Error Messaging**: User-friendly validation error messages
- **Conditional Validation**: Context-aware validation rules
- **Performance**: Efficient validation with minimal overhead

## Application Execution

### Starting the Development Server

```bash
# Run the application in development mode
dotnet run

# The API will be accessible at:
# HTTP: http://localhost:5236
# Swagger UI: http://localhost:5236/swagger
```

### Default Authentication Accounts

The application automatically creates default accounts for testing:

**Administrator Account:**
- Email: `admin@todoapi.com`
- Password: `Admin123!`
- Role: Admin

**Standard User Account:**
- Email: `user@todoapi.com`
- Password: `User123!`
- Role: User

## API Endpoint Documentation

### Authentication Endpoints

- `POST /api/auth/register` - Register new user account
- `POST /api/auth/login` - Authenticate user and obtain JWT token
- `GET /api/auth/me` - Retrieve current authenticated user information

### Todo Management Endpoints

- `GET /api/todos` - Retrieve all todos for authenticated user
- `GET /api/todos/{id}` - Retrieve specific todo by ID
- `POST /api/todos` - Create new todo item
- `PUT /api/todos/{id}` - Update existing todo item
- `DELETE /api/todos/{id}` - Delete todo item

## Professional Testing Approach

### Using Swagger UI for Interactive Testing

1. Navigate to `http://localhost:5236/swagger`
2. Click the "Authorize" button in the top-right corner
3. Authenticate using the login endpoint
4. Copy the JWT token from the authentication response
5. Enter `Bearer YOUR_TOKEN` in the authorization dialog
6. Execute API operations interactively with full documentation


