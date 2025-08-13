# Todo List Management System

## Introduction

This is a comprehensive Todo List Management System built with modern web technologies. The application provides a robust platform for personal task management with advanced administrative capabilities.

### Technology Stack

- **ASP.NET Core 8 MVC** - Modern web framework for building scalable web applications
- **Entity Framework Core 8** - Object-relational mapping (ORM) framework for database operations
- **SQLite Database** - Lightweight, serverless database engine
- **ASP.NET Core Identity** - Complete authentication and authorization system
- **AutoMapper** - Object-to-object mapping library
- **FluentValidation** - Validation library for .NET
- **Bootstrap 5** - Modern CSS framework for responsive UI
- **FontAwesome** - Icon library for enhanced user interface

### Key Features

- **User Authentication & Authorization**
  - Secure user registration and login
  - Role-based access control (Admin/User)
  - Password security with customizable policies

- **Todo Management**
  - Create, read, update, and delete todos
  - Set due dates and priorities
  - Mark todos as completed/pending
  - Advanced filtering (All, Completed, Pending, Overdue)
  - Pagination for large datasets

- **Admin Panel**
  - User management with role assignment
  - View all users and their todo statistics
  - Cross-user todo management
  - Administrative dashboard with insights

- **Responsive Design**
  - Mobile-friendly interface
  - Modern and intuitive user experience
  - Real-time status updates

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

### Step 2: Database Configuration

#### A. Create ApplicationDbContext

Create `Data/ApplicationDbContext.cs`:

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListMVC.Models;

namespace TodoListMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TodoItem entity
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UserId).IsRequired();

                // Configure relationship with ApplicationUser
                entity.HasOne(e => e.User)
                      .WithMany(u => u.TodoItems)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
```

#### B. Update Connection String

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

#### C. Run Database Migrations

```bash
# Add initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Step 3: Create Models

#### A. ApplicationUser Model

Create `Models/ApplicationUser.cs`:

```csharp
using Microsoft.AspNetCore.Identity;

namespace TodoListMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}
```

#### B. TodoItem Model

Create `Models/TodoItem.cs`:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListMVC.Models
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
        
        // Computed property
        public bool IsOverdue => !IsCompleted && DueDate.HasValue && DueDate.Value < DateTime.UtcNow;

        // Navigation property
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
    }
}
```

### Step 4: Authentication & Authorization Setup

#### A. Configure Services in Program.cs

Update `Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using TodoListMVC.Data;
using TodoListMVC.Models;
using TodoListMVC.Services;
using TodoListMVC.Repositories;

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

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Register repositories and services
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add controllers and views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
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

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

#### B. Create Data Seeding

Create `Data/SeedData.cs`:

```csharp
using Microsoft.AspNetCore.Identity;
using TodoListMVC.Models;

namespace TodoListMVC.Data
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
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
```

### Step 5: Implement Todo List Features

#### A. Create DTOs

Create data transfer objects in the `DTOs/` folder:

- `TodoItemDto.cs`
- `CreateTodoItemDto.cs`
- `UpdateTodoItemDto.cs`
- `TodoItemFilterDto.cs`
- `UserDto.cs`
- `RegisterDto.cs`
- `LoginDto.cs`

#### B. Create Validators

Create validation classes in the `Validators/` folder using FluentValidation.

#### C. Create Repositories

Implement the Repository pattern in the `Repositories/` folder:

- `ITodoItemRepository.cs`
- `TodoItemRepository.cs`

#### D. Create Services

Implement business logic in the `Services/` folder:

- `ITodoItemService.cs`
- `TodoItemService.cs`
- `IUserService.cs`
- `UserService.cs`

#### E. Create Controllers

Create MVC controllers in the `Controllers/` folder:

- `HomeController.cs` - Landing page
- `AccountController.cs` - Authentication
- `TodoController.cs` - Todo management
- `AdminController.cs` - Admin panel

#### F. Create Views

Create Razor views in the `Views/` folder with responsive Bootstrap design.

### Step 6: Admin Panel Implementation

The admin panel provides comprehensive user and todo management capabilities:

- **User Management**: View all users, assign roles, view user statistics
- **Todo Management**: View and manage todos across all users
- **Dashboard**: Administrative insights and system overview

## Running the Project

### Development Environment

1. **Start the application**:
   ```bash
   dotnet run
   ```

2. **Default URLs**:
   - HTTP: `http://localhost:5111`
   - HTTPS: `https://localhost:7111`

3. **Access the application**:
   - Open your web browser
   - Navigate to the application URL
   - You should see the landing page

### First-Time Setup

1. **Create a user account**:
   - Click "Register" to create a new account
   - Fill in the required information
   - Login with your credentials

2. **Admin Access**:
   - **Email**: `admin@todolist.com`
   - **Password**: `Admin123!`
   - After login, access admin features via the navigation menu

3. **Sample User Access** (Optional):
   - **Email**: `user@todolist.com`
   - **Password**: `User123!`
   - Regular user account for testing

4. **Create your first todo**:
   - Navigate to "My Todos"
   - Click "Create New Todo"
   - Fill in the task details
   - Save and manage your todos

### Database Management

- **Database file**: `todolist.db` (SQLite)
- **View database**: Use DB Browser for SQLite or similar tools
- **Reset database**: Delete `todolist.db` and run `dotnet ef database update`

### Development Commands

```bash
# Run the application
dotnet run

# Build the project
dotnet build

# Run tests (if implemented)
dotnet test

# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

## Project Features Walkthrough

### User Features

1. **Authentication**
   - Secure registration and login
   - Password requirements enforcement
   - Session management

2. **Todo Management**
   - Create todos with titles, descriptions, due dates
   - Mark todos as completed
   - Filter by status (All, Completed, Pending, Overdue)
   - Pagination for large lists
   - Update dates are tracked automatically

3. **Dashboard**
   - Personal todo statistics
   - Quick overview of pending tasks
   - Recent activity

### Admin Features

1. **User Management**
   - View all registered users
   - See user registration dates
   - View todo statistics per user
   - Manage user roles

2. **System Overview**
   - Total users count
   - System-wide todo statistics
   - Administrative dashboard

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Ensure SQLite is properly installed
   - Check connection string in `appsettings.json`
   - Run `dotnet ef database update`

2. **Migration Issues**
   - Delete `Migrations/` folder
   - Delete `todolist.db` file
   - Run `dotnet ef migrations add InitialCreate`
   - Run `dotnet ef database update`

3. **Package Restore Issues**
   - Run `dotnet restore`
   - Clear NuGet cache: `dotnet nuget locals all --clear`

4. **Permission Issues**
   - Ensure proper file permissions
   - Run Visual Studio as Administrator if needed

### Getting Help

- Check the official documentation: https://docs.microsoft.com/aspnet/core
- ASP.NET Core Community: https://github.com/dotnet/aspnetcore
- Entity Framework Core: https://docs.microsoft.com/ef/core



