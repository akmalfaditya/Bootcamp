# Professional Todo List Management System - Complete Implementation Guide

## Project Overview

This project demonstrates a professional-grade Todo List Management System built with ASP.NET Core 8 MVC, implementing enterprise-level architectural patterns and best practices. The application provides comprehensive task management capabilities with robust authentication, role-based authorization, and advanced administrative features.

### Enterprise Architecture Components

- **ASP.NET Core 8 MVC** - Modern web framework with Model-View-Controller pattern
- **Entity Framework Core 8** - Advanced object-relational mapping with SQLite database
- **ASP.NET Core Identity** - Comprehensive authentication and authorization framework
- **JWT Bearer Authentication** - Token-based authentication for API endpoints
- **AutoMapper** - Object-to-object mapping with custom configuration profiles
- **FluentValidation** - Rule-based input validation with custom validators
- **Repository Pattern** - Data access abstraction for maintainable code architecture
- **Service Layer** - Business logic separation with dependency injection
- **Bootstrap 5** - Professional responsive UI framework
- **FontAwesome** - Comprehensive icon library for enhanced user experience

### Professional Features

- **Authentication & Authorization System**
  - Secure user registration and login with password policies
  - Role-based access control with Admin and User roles
  - Session management with configurable cookie settings
  - Access denied handling and security measures

- **Advanced Todo Management**
  - Complete CRUD operations with professional validation
  - Due date tracking with overdue task identification
  - Task completion tracking with timestamp management
  - Advanced filtering capabilities (All, Completed, Pending, Overdue)
  - Pagination system for efficient data handling

- **Administrative Panel**
  - Comprehensive user management with role assignment
  - Cross-user todo management and oversight
  - System-wide statistics and analytics dashboard
  - Administrative insights and reporting capabilities

- **Professional UI/UX Design**
  - Responsive design with mobile-first approach
  - Intuitive navigation with role-based menu systems
  - Real-time feedback with notification systems
  - Professional styling with consistent branding

## Prerequisites

Before you begin, ensure you have the following software installed on your development machine:

### Required Software

1. **.NET 8 SDK**
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   - Verify installation: `dotnet --version`

2. **Development Environment** (Choose one):
   - **Visual Studio 2022** (Recommended)
     - Download from: https://visualstudio.microsoft.com/downloads/
     - Include ASP.NET and web development workload
   - **Visual Studio Code**
     - Download from: https://code.visualstudio.com/
     - Install C# extension

3. **Git**
   - Download from: https://git-scm.com/downloads
   - Verify installation: `git --version`

4. **Database Tools** (Optional but recommended):
   - **DB Browser for SQLite**: https://sqlitebrowser.org/
   - Or any SQLite database viewer

## Installation & Setup Guide

### Step 1: Create New Project

#### A. Initialize the Project

```bash
# Create a new directory for your project
mkdir TodoListApp
cd TodoListApp

# Create a new ASP.NET Core MVC project
dotnet new mvc -n TodoListMVC
cd TodoListMVC
```

#### B. Add Required NuGet Packages

```bash
# Entity Framework Core with SQLite
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.7
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.7
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.7

# ASP.NET Core Identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.7

# AutoMapper for object mapping
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

# FluentValidation for input validation
dotnet add package FluentValidation --version 11.9.2
dotnet add package FluentValidation.AspNetCore --version 11.3.0

# JWT Bearer Authentication (for API endpoints)
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.7
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.0.1
```

#### C. Project Structure

After setup, your project structure should look like this:

```
TodoListMVC/
├── Controllers/
├── Data/
├── DTOs/
├── MappingProfiles/
├── Models/
├── Repositories/
├── Services/
├── Validators/
├── Views/
├── wwwroot/
├── Migrations/
├── Program.cs
├── appsettings.json
└── TodoListMVC.csproj
```

### Step 2: Entity Models Implementation

#### A. Application User Model

Create `Models/ApplicationUser.cs` - Professional user entity extending ASP.NET Core Identity:

```csharp
using Microsoft.AspNetCore.Identity;

namespace Implemented_MVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties for entity relationships
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}
```

#### B. Todo Item Entity Model

Create `Models/TodoItem.cs` - Core business entity with comprehensive properties:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implemented_MVC.Models
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

        // Foreign Key for user relationship
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

#### C. Error View Model

Create `Models/ErrorViewModel.cs` - Standard error handling model:

```csharp
namespace Implemented_MVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
```

### Step 3: Database Context Configuration

Create `Data/ApplicationDbContext.cs` - Professional Entity Framework configuration:

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Implemented_MVC.Models;

namespace Implemented_MVC.Data
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

            // Configure TodoItem entity with professional settings
            builder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
                
                // Configure entity relationships with cascade delete
                entity.HasOne(t => t.User)
                      .WithMany(u => u.TodoItems)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Add database indexes for performance optimization
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

### Step 4: Data Transfer Objects (DTOs) Implementation

#### A. API Response Wrapper

Create `DTOs/ApiResponseDto.cs` - Standardized API response format:

```csharp
namespace Implemented_MVC.DTOs
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
namespace Implemented_MVC.DTOs
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
namespace Implemented_MVC.DTOs
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
namespace Implemented_MVC.DTOs
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

Create authentication-related DTOs for user management:

`DTOs/RegisterDto.cs`:
```csharp
namespace Implemented_MVC.DTOs
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

`DTOs/LoginDto.cs`:
```csharp
namespace Implemented_MVC.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
```

`DTOs/UserDto.cs`:
```csharp
namespace Implemented_MVC.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public int TodoCount { get; set; }
        public int CompletedTodoCount { get; set; }
    }
}
```

### Step 5: FluentValidation Implementation

#### A. Todo Item Validators

Create `Validators/CreateTodoItemValidator.cs` - Professional input validation:

```csharp
using FluentValidation;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Validators
{
    public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemDto>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .When(x => x.DueDate.HasValue)
                .WithMessage("Due date cannot be in the past.");
        }
    }
}
```

Create `Validators/UpdateTodoItemValidator.cs`:

```csharp
using FluentValidation;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Validators
{
    public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemDto>
    {
        public UpdateTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .When(x => x.DueDate.HasValue)
                .WithMessage("Due date cannot be in the past.");
        }
    }
}
```

#### B. Authentication Validators

Create `Validators/RegisterValidator.cs`:

```csharp
using FluentValidation;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.FirstName)
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));
        }
    }
}
```

Create `Validators/LoginValidator.cs`:

```csharp
using FluentValidation;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
```

### Step 6: AutoMapper Configuration

#### A. Todo Item Mapping Profile

Create `MappingProfiles/TodoItemProfile.cs` - Professional object mapping configuration:

```csharp
using AutoMapper;
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.MappingProfiles
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
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.IsCompleted ? DateTime.UtcNow : (DateTime?)null))
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
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.MappingProfiles
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
                .ForMember(dest => dest.Roles, opt => opt.Ignore()) // Roles are set separately in service
                .ForMember(dest => dest.TodoCount, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.CompletedTodoCount, opt => opt.Ignore()); // Set in service
        }
    }
}
```

### Step 7: Repository Layer Implementation

#### A. Repository Interface Definition

Create `Repositories/ITodoItemRepository.cs` - Repository pattern contract:

```csharp
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Repositories
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
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetTotalCountAsync(string? userId = null);
        Task<int> GetCompletedCountAsync(string? userId = null);
        Task<int> GetPendingCountAsync(string? userId = null);
        Task<List<TodoItem>> GetOverdueTasksAsync(string? userId = null);
    }
}
```

#### B. Repository Implementation

Create `Repositories/TodoItemRepository.cs` - Data access layer implementation:

```csharp
using Microsoft.EntityFrameworkCore;
using Implemented_MVC.Data;
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem?> GetByIdWithUserAsync(int id)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TodoItem>> GetByUserIdAsync(string userId)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<PaginatedResultDto<TodoItem>> GetFilteredAsync(TodoItemFilterDto filter, string? userId = null)
        {
            var query = _context.TodoItems.Include(t => t.User).AsQueryable();

            // Apply user filter if specified
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserId == userId);
            }

            // Apply completion status filter
            if (filter.IsCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == filter.IsCompleted.Value);
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(t => t.Title.Contains(filter.SearchTerm) || 
                                       (t.Description != null && t.Description.Contains(filter.SearchTerm)));
            }

            // Apply overdue filter
            if (filter.ShowOverdue == true)
            {
                var now = DateTime.UtcNow;
                query = query.Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < now);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PaginatedResultDto<TodoItem>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize)
            };
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

        public async Task<bool> DeleteAsync(int id)
        {
            var todoItem = await GetByIdAsync(id);
            if (todoItem == null)
                return false;

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TodoItems.AnyAsync(t => t.Id == id);
        }

        public async Task<int> GetTotalCountAsync(string? userId = null)
        {
            var query = _context.TodoItems.AsQueryable();
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserId == userId);
            }
            return await query.CountAsync();
        }

        public async Task<int> GetCompletedCountAsync(string? userId = null)
        {
            var query = _context.TodoItems.Where(t => t.IsCompleted);
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserId == userId);
            }
            return await query.CountAsync();
        }

        public async Task<int> GetPendingCountAsync(string? userId = null)
        {
            var query = _context.TodoItems.Where(t => !t.IsCompleted);
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserId == userId);
            }
            return await query.CountAsync();
        }

        public async Task<List<TodoItem>> GetOverdueTasksAsync(string? userId = null)
        {
            var now = DateTime.UtcNow;
            var query = _context.TodoItems
                .Include(t => t.User)
                .Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < now);

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserId == userId);
            }

            return await query.OrderBy(t => t.DueDate).ToListAsync();
        }
    }
}
```

### Step 8: Service Layer Implementation

#### A. Todo Item Service Interface

Create `Services/ITodoItemService.cs` - Business logic contract:

```csharp
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Services
{
    public interface ITodoItemService
    {
        Task<ApiResponseDto<TodoItemDto>> GetByIdAsync(int id, string currentUserId, bool isAdmin);
        Task<ApiResponseDto<PaginatedResultDto<TodoItemDto>>> GetFilteredAsync(TodoItemFilterDto filter, string currentUserId, bool isAdmin);
        Task<ApiResponseDto<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto, string userId);
        Task<ApiResponseDto<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto, string currentUserId, bool isAdmin);
        Task<ApiResponseDto<bool>> DeleteAsync(int id, string currentUserId, bool isAdmin);
        Task<ApiResponseDto<List<TodoItemDto>>> GetUserTodosAsync(string userId);
        Task<ApiResponseDto<object>> GetDashboardStatsAsync(string currentUserId, bool isAdmin);
    }
}
```

#### B. Todo Item Service Implementation

Create `Services/TodoItemService.cs` - Business logic implementation with AutoMapper:

```csharp
using AutoMapper;
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;
using Implemented_MVC.Repositories;

namespace Implemented_MVC.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _repository;
        private readonly IMapper _mapper;

        public TodoItemService(ITodoItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<TodoItemDto>> GetByIdAsync(int id, string currentUserId, bool isAdmin)
        {
            try
            {
                var todoItem = await _repository.GetByIdWithUserAsync(id);
                
                if (todoItem == null)
                {
                    return ApiResponseDto<TodoItemDto>.ErrorResult("Todo item not found.");
                }

                // Check authorization
                if (!isAdmin && todoItem.UserId != currentUserId)
                {
                    return ApiResponseDto<TodoItemDto>.ErrorResult("Access denied.");
                }

                var todoDto = _mapper.Map<TodoItemDto>(todoItem);
                return ApiResponseDto<TodoItemDto>.SuccessResult(todoDto);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TodoItemDto>.ErrorResult($"Error retrieving todo: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResultDto<TodoItemDto>>> GetFilteredAsync(TodoItemFilterDto filter, string currentUserId, bool isAdmin)
        {
            try
            {
                var userId = isAdmin ? null : currentUserId;
                var result = await _repository.GetFilteredAsync(filter, userId);
                
                var todoDtos = _mapper.Map<List<TodoItemDto>>(result.Items);
                
                var paginatedResult = new PaginatedResultDto<TodoItemDto>
                {
                    Items = todoDtos,
                    TotalCount = result.TotalCount,
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalPages = result.TotalPages
                };

                return ApiResponseDto<PaginatedResultDto<TodoItemDto>>.SuccessResult(paginatedResult);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResultDto<TodoItemDto>>.ErrorResult($"Error retrieving todos: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto, string userId)
        {
            try
            {
                var todoItem = _mapper.Map<TodoItem>(createDto);
                todoItem.UserId = userId;
                todoItem.CreatedAt = DateTime.UtcNow;

                var createdTodo = await _repository.CreateAsync(todoItem);
                var todoDto = _mapper.Map<TodoItemDto>(createdTodo);

                return ApiResponseDto<TodoItemDto>.SuccessResult(todoDto, "Todo created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TodoItemDto>.ErrorResult($"Error creating todo: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto, string currentUserId, bool isAdmin)
        {
            try
            {
                var existingTodo = await _repository.GetByIdAsync(id);
                
                if (existingTodo == null)
                {
                    return ApiResponseDto<TodoItemDto>.ErrorResult("Todo item not found.");
                }

                // Check authorization
                if (!isAdmin && existingTodo.UserId != currentUserId)
                {
                    return ApiResponseDto<TodoItemDto>.ErrorResult("Access denied.");
                }

                // Update properties using AutoMapper
                _mapper.Map(updateDto, existingTodo);
                existingTodo.UpdatedAt = DateTime.UtcNow;

                // Set completion timestamp
                if (updateDto.IsCompleted && !existingTodo.IsCompleted)
                {
                    existingTodo.CompletedAt = DateTime.UtcNow;
                }
                else if (!updateDto.IsCompleted && existingTodo.IsCompleted)
                {
                    existingTodo.CompletedAt = null;
                }

                var updatedTodo = await _repository.UpdateAsync(existingTodo);
                var todoDto = _mapper.Map<TodoItemDto>(updatedTodo);

                return ApiResponseDto<TodoItemDto>.SuccessResult(todoDto, "Todo updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TodoItemDto>.ErrorResult($"Error updating todo: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteAsync(int id, string currentUserId, bool isAdmin)
        {
            try
            {
                var todoItem = await _repository.GetByIdAsync(id);
                
                if (todoItem == null)
                {
                    return ApiResponseDto<bool>.ErrorResult("Todo item not found.");
                }

                // Check authorization
                if (!isAdmin && todoItem.UserId != currentUserId)
                {
                    return ApiResponseDto<bool>.ErrorResult("Access denied.");
                }

                var deleted = await _repository.DeleteAsync(id);
                
                if (deleted)
                {
                    return ApiResponseDto<bool>.SuccessResult(true, "Todo deleted successfully.");
                }
                
                return ApiResponseDto<bool>.ErrorResult("Failed to delete todo.");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResult($"Error deleting todo: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<List<TodoItemDto>>> GetUserTodosAsync(string userId)
        {
            try
            {
                var todos = await _repository.GetByUserIdAsync(userId);
                var todoDtos = _mapper.Map<List<TodoItemDto>>(todos);

                return ApiResponseDto<List<TodoItemDto>>.SuccessResult(todoDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<List<TodoItemDto>>.ErrorResult($"Error retrieving user todos: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<object>> GetDashboardStatsAsync(string currentUserId, bool isAdmin)
        {
            try
            {
                var userId = isAdmin ? null : currentUserId;
                
                var totalCount = await _repository.GetTotalCountAsync(userId);
                var completedCount = await _repository.GetCompletedCountAsync(userId);
                var pendingCount = await _repository.GetPendingCountAsync(userId);
                var overdueTasks = await _repository.GetOverdueTasksAsync(userId);

                var stats = new
                {
                    TotalTodos = totalCount,
                    CompletedTodos = completedCount,
                    PendingTodos = pendingCount,
                    OverdueTodos = overdueTasks.Count,
                    CompletionRate = totalCount > 0 ? Math.Round((double)completedCount / totalCount * 100, 1) : 0
                };

                return ApiResponseDto<object>.SuccessResult(stats);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<object>.ErrorResult($"Error retrieving dashboard stats: {ex.Message}");
            }
        }
    }
}
```

#### C. User Service Interface and Implementation

Create `Services/IUserService.cs`:

```csharp
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Services
{
    public interface IUserService
    {
        Task<ApiResponseDto<LoginResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponseDto<bool>> LogoutAsync();
        Task<ApiResponseDto<UserDto>> GetCurrentUserAsync(string userId);
        Task<ApiResponseDto<List<UserDto>>> GetAllUsersAsync();
        Task<ApiResponseDto<UserDto>> GetUserByIdAsync(string userId);
    }
}
```

### Step 9: Database Seeding and Configuration

Create `Data/SeedData.cs` - Professional data seeding:

```csharp
using Microsoft.AspNetCore.Identity;
using Implemented_MVC.Models;

namespace Implemented_MVC.Data
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
            var adminEmail = "admin@todolist.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine($"Admin user created: {adminEmail}");
                    Console.WriteLine("Default password: Admin123!");
                }
            }

            // Create sample user
            var userEmail = "user@todolist.com";
            var sampleUser = await userManager.FindByEmailAsync(userEmail);
            
            if (sampleUser == null)
            {
                sampleUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "Sample",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(sampleUser, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(sampleUser, "User");
                    Console.WriteLine($"Sample user created: {userEmail}");
                    Console.WriteLine("Default password: User123!");
                }
            }
        }
    }
}
```

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=todolist.db"
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

### Step 10: Professional Program.cs Configuration

Replace the content of `Program.cs` with comprehensive enterprise configuration:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Implemented_MVC.Data;
using Implemented_MVC.Models;
using Implemented_MVC.Services;
using Implemented_MVC.Repositories;
using Implemented_MVC.DTOs;
using Implemented_MVC.Validators;

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

// Configure authentication cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// Add AutoMapper for object-to-object mapping
builder.Services.AddAutoMapper(typeof(Program));

// Add FluentValidation services
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoItemValidator>();

// Add Repository Layer
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

// Add Service Layer
builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add MVC Controllers with Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed database with default roles and users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configure admin routes
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

// Configure default routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### Step 11: Controller Implementation

#### A. Account Controller with Authentication

Create `Controllers/AccountController.cs` - Professional authentication controller:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Implemented_MVC.Models;
using Implemented_MVC.DTOs;
using Implemented_MVC.Services;
using FluentValidation;

namespace Implemented_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AccountController(
            IUserService userService,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator)
        {
            _userService = userService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        // GET: Account/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Todo");
            }
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Todo");
            }

            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(registerDto);
            }

            var result = await _userService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(registerDto);
            }

            TempData["SuccessMessage"] = result.Message + " You can now login.";
            return RedirectToAction(nameof(Login));
        }

        // GET: Account/Login
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Todo");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Todo");
            }

            ViewData["ReturnUrl"] = returnUrl;

            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(loginDto);
            }

            var result = await _userService.LoginAsync(loginDto);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(loginDto);
            }

            TempData["SuccessMessage"] = $"Welcome back, {result.Data?.User?.FirstName}!";

            // Redirect to return URL or default
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Todo");
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var result = await _userService.LogoutAsync();
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "You have been logged out successfully.";
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/AccessDenied
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Account/Profile
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _userService.GetCurrentUserAsync(userId);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index", "Todo");
            }

            return View(result.Data);
        }
    }
}
```

#### B. Todo Controller with CRUD Operations

Create `Controllers/TodoController.cs` - Professional todo management:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Implemented_MVC.DTOs;
using Implemented_MVC.Services;
using FluentValidation;

namespace Implemented_MVC.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IValidator<CreateTodoItemDto> _createValidator;
        private readonly IValidator<UpdateTodoItemDto> _updateValidator;

        public TodoController(
            ITodoItemService todoItemService,
            IValidator<CreateTodoItemDto> createValidator,
            IValidator<UpdateTodoItemDto> updateValidator)
        {
            _todoItemService = todoItemService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: Todo
        public async Task<IActionResult> Index(TodoItemFilterDto filter)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            // Set default values for filter
            filter.PageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            filter.PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

            var result = await _todoItemService.GetFilteredAsync(filter, userId, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new PaginatedResultDto<TodoItemDto>());
            }

            ViewBag.Filter = filter;
            return View(result.Data);
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoItemService.GetByIdAsync(id, userId, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTodoItemDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(createDto);
            }

            var userId = GetCurrentUserId();
            var result = await _todoItemService.CreateAsync(createDto, userId);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(createDto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoItemService.GetByIdAsync(id, userId, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateTodoItemDto
            {
                Title = result.Data.Title,
                Description = result.Data.Description,
                DueDate = result.Data.DueDate,
                IsCompleted = result.Data.IsCompleted
            };

            return View(updateDto);
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTodoItemDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(updateDto);
            }

            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoItemService.UpdateAsync(id, updateDto, userId, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(updateDto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoItemService.GetByIdAsync(id, userId, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoItemService.DeleteAsync(id, userId, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Todo/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _todoItemService.GetDashboardStatsAsync(userId, isAdmin);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}
```

### Step 12: Database Migration and Build

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

### JWT Authentication Integration (Optional API Endpoints)

While this is primarily an MVC application, JWT authentication can be integrated for API endpoints:

- **Token Generation**: Secure JWT token creation with claims-based authentication
- **Token Validation**: Comprehensive validation middleware for API requests
- **Role-Based Access**: Integration with ASP.NET Core Identity roles
- **Hybrid Authentication**: Support for both cookie and JWT authentication

### Data Transfer Objects (DTOs) Architecture

The DTO implementation ensures clean separation of concerns:

- **Input DTOs**: `CreateTodoItemDto`, `UpdateTodoItemDto` for user input
- **Output DTOs**: `TodoItemDto`, `UserDto` for API responses
- **Filter DTOs**: `TodoItemFilterDto` for advanced search and filtering
- **Response Wrappers**: `ApiResponseDto<T>` for consistent API responses
- **Pagination DTOs**: `PaginatedResultDto<T>` for efficient data handling

### AutoMapper Professional Configuration

Advanced object-to-object mapping implementation:

- **Custom Mapping Logic**: Specialized mappings for complex business scenarios
- **Conditional Mappings**: Context-aware property mapping rules
- **Performance Optimization**: Efficient mapping with minimal overhead
- **Null Safety**: Proper handling of nullable properties and relationships
- **Business Logic Integration**: Computed properties and timestamp management

### Repository Pattern Implementation

Clean data access layer with enterprise patterns:

- **Interface Segregation**: Well-defined contracts for data operations
- **Separation of Concerns**: Data access logic isolated from business logic
- **Query Optimization**: Efficient database queries with proper indexing
- **User Authorization**: Built-in security for user-specific data access
- **Advanced Filtering**: Complex query building with dynamic filters

### Service Layer Architecture

Professional business logic implementation:

- **Dependency Injection**: Proper IoC container configuration
- **Error Handling**: Comprehensive exception management and logging
- **Business Validation**: Complex business rules beyond basic validation
- **Transaction Management**: Appropriate handling of database transactions
- **Authorization Logic**: Role-based access control implementation

### FluentValidation Integration

Enterprise-level input validation system:

- **Rule-Based Validation**: Declarative validation rules with custom messages
- **Conditional Validation**: Context-aware validation based on business rules
- **Custom Validators**: Business-specific validation logic implementation
- **Integration with MVC**: Seamless integration with model binding and validation
- **Error Messaging**: User-friendly validation error messages with localization support

## Application Execution and Testing

### Development Environment Setup

```bash
# Start the application in development mode
dotnet run

# The application will be accessible at:
# HTTP: http://localhost:5111
# HTTPS: https://localhost:7111
```

### Default Authentication Accounts

The application automatically creates default accounts for testing:

**Administrator Account:**
- Email: `admin@todolist.com`
- Password: `Admin123!`
- Role: Admin
- Access: Full system administration capabilities

**Sample User Account:**
- Email: `user@todolist.com`
- Password: `User123!`
- Role: User
- Access: Personal todo management only

### Professional Testing Approach

#### User Workflow Testing

1. **Registration Process**
   - Navigate to registration page
   - Test validation rules and error handling
   - Verify successful account creation

2. **Authentication Flow**
   - Test login with valid and invalid credentials
   - Verify role-based access control
   - Test session management and logout

3. **Todo Management**
   - Create todos with various data combinations
   - Test CRUD operations with validation
   - Verify user isolation and security

#### Administrative Testing

1. **Admin Panel Access**
   - Login with admin credentials
   - Verify admin-only functionality access
   - Test cross-user data management

2. **User Management**
   - View all users and their statistics
   - Test role assignment functionality
   - Verify system-wide analytics

### Database Management

- **Database File**: `todolist.db` (SQLite database)
- **Schema Management**: Entity Framework Core migrations
- **Data Seeding**: Automatic role and user creation
- **Performance**: Optimized indexes for efficient querying

This implementation demonstrates professional-grade ASP.NET Core MVC development with enterprise-level architectural patterns, comprehensive security, clean code principles, and maintainable structure suitable for production environments.
