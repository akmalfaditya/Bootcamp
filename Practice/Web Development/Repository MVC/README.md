# Repository Pattern Student Management System

A comprehensive ASP.NET Core MVC application demonstrating the **Repository Design Pattern** with Entity Framework Core. This project showcases professional software development practices including separation of concerns, dependency injection, and clean architecture principles.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Step-by-Step Setup](#step-by-step-setup)
- [Project Structure](#project-structure)
- [Design Patterns](#design-patterns)
- [Database Schema](#database-schema)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)

## Overview

This project demonstrates the implementation of the **Repository Design Pattern** in an ASP.NET Core MVC application. The Repository pattern encapsulates data access logic, centralizes common database operations, and provides better maintainability while decoupling the infrastructure from the domain model layer.

### Key Benefits

- **Separation of Concerns**: Business logic separated from data access logic
- **Testability**: Easy to mock repositories for unit testing
- **Maintainability**: Centralized data access patterns
- **Flexibility**: Can easily switch between different data storage technologies
- **Code Reusability**: Generic repository eliminates duplicate CRUD operations

## Features

- **Student Management**: Create, read, update, delete student records
- **Grade Management**: Manage student grades with automatic letter grade calculation
- **Search Functionality**: Search students by name, branch, or section
- **Data Validation**: Server-side validation with user-friendly error messages
- **Responsive Design**: Bootstrap-based responsive UI
- **Database Migrations**: Automated database schema management

## Architecture

### Design Pattern Flow
```
Browser Request → Controller → Service Layer → Repository Interface → Repository Implementation → Entity Framework → SQLite Database
```

### Layer Responsibilities

1. **Controllers**: Handle HTTP requests and responses
2. **Services**: Business logic and transaction management
3. **Repositories**: Data access abstraction
4. **Models**: Data entities and validation
5. **Views**: User interface presentation

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- Basic knowledge of C#, MVC pattern, and Entity Framework

## Step-by-Step Setup

### 1. Create New Project

```bash
# Create new ASP.NET Core MVC project
dotnet new mvc -n RepositoryMVC
cd RepositoryMVC

# Add Entity Framework packages
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### 2. Create Models

Create `Models/Student.cs`:

```csharp
using System.ComponentModel.DataAnnotations;

namespace RepositoryMVC.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Branch is required")]
        [StringLength(50, ErrorMessage = "Branch cannot exceed 50 characters")]
        public string Branch { get; set; } = string.Empty;

        [Required(ErrorMessage = "Section is required")]
        [StringLength(10, ErrorMessage = "Section cannot exceed 10 characters")]
        public string Section { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enrollment date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
```

Create `Models/Grade.cs`:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryMVC.Models
{
    public class Grade
    {
        public int GradeID { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal GradeValue { get; set; }

        [StringLength(2, ErrorMessage = "Letter grade cannot exceed 2 characters")]
        public string? LetterGrade { get; set; }

        [Required(ErrorMessage = "Grade date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Grade Date")]
        public DateTime GradeDate { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string? Comments { get; set; }

        public virtual Student? Student { get; set; }

        public string CalculateLetterGrade()
        {
            return GradeValue switch
            {
                >= 90 => "A",
                >= 80 => "B", 
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            };
        }
    }
}
```

### 3. Create Database Context

Create `Data/ApplicationDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Models;

namespace RepositoryMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision
            modelBuilder.Entity<Grade>()
                .Property(g => g.GradeValue)
                .HasColumnType("decimal(5,2)");

            // Seed data
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentID = 1,
                    Name = "John Smith",
                    Gender = "Male",
                    Branch = "Computer Science",
                    Section = "A",
                    Email = "john.smith@university.edu",
                    EnrollmentDate = new DateTime(2023, 9, 1)
                }
                // Add more seed data as needed
            );
        }
    }
}
```

### 4. Create Repository Interfaces

Create `Repositories/IGenericRepository.cs`:

```csharp
using System.Linq.Expressions;

namespace RepositoryMVC.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}
```

Create `Repositories/IStudentRepository.cs`:

```csharp
using RepositoryMVC.Models;

namespace RepositoryMVC.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<IEnumerable<Student>> GetAllStudentsWithGradesAsync();
        Task<Student?> GetStudentWithGradesAsync(int studentId);
        Task<IEnumerable<Student>> FindStudentsByNameAsync(string name);
        Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch);
        Task<IEnumerable<Student>> GetStudentsBySectionAsync(string section);
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);
        Task<bool> IsEmailExistsAsync(string email, int? excludeStudentId = null);
    }
}
```

### 5. Create Repository Implementations

Create `Repositories/GenericRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using System.Linq.Expressions;

namespace RepositoryMVC.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            return await _dbSet.CountAsync(predicate);
        }
    }
}
```

Create `Repositories/StudentRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Models;

namespace RepositoryMVC.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetAllStudentsWithGradesAsync()
        {
            return await _dbSet.Include(s => s.Grades).ToListAsync();
        }

        public async Task<Student?> GetStudentWithGradesAsync(int studentId)
        {
            return await _dbSet.Include(s => s.Grades)
                              .FirstOrDefaultAsync(s => s.StudentID == studentId);
        }

        public async Task<IEnumerable<Student>> FindStudentsByNameAsync(string name)
        {
            return await _dbSet.Where(s => s.Name.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch)
        {
            return await _dbSet.Where(s => s.Branch == branch).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsBySectionAsync(string section)
        {
            return await _dbSet.Where(s => s.Section == section).ToListAsync();
        }

        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllStudentsWithGradesAsync();

            return await _dbSet.Include(s => s.Grades)
                              .Where(s => s.Name.Contains(searchTerm) ||
                                         s.Branch.Contains(searchTerm) ||
                                         s.Section.Contains(searchTerm) ||
                                         s.Email.Contains(searchTerm))
                              .ToListAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeStudentId = null)
        {
            var query = _dbSet.Where(s => s.Email == email);
            if (excludeStudentId.HasValue)
                query = query.Where(s => s.StudentID != excludeStudentId.Value);
            
            return await query.AnyAsync();
        }
    }
}
```

### 6. Create Service Layer

Create `Services/StudentService.cs`:

```csharp
using RepositoryMVC.Models;
using RepositoryMVC.Repositories;
using RepositoryMVC.Data;

namespace RepositoryMVC.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);
    }

    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ApplicationDbContext _context;

        public StudentService(IStudentRepository studentRepository, ApplicationDbContext context)
        {
            _studentRepository = studentRepository;
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsWithGradesAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetStudentWithGradesAsync(id);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            if (student.EnrollmentDate == default(DateTime))
                student.EnrollmentDate = DateTime.Today;

            if (await _studentRepository.IsEmailExistsAsync(student.Email))
                throw new InvalidOperationException($"A student with email {student.Email} already exists.");

            await _studentRepository.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                if (await _studentRepository.IsEmailExistsAsync(student.Email, student.StudentID))
                    throw new InvalidOperationException($"Email {student.Email} is already taken by another student.");

                _studentRepository.Update(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return false;

            _studentRepository.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            return await _studentRepository.SearchStudentsAsync(searchTerm);
        }
    }
}
```

### 7. Configure Services in Program.cs

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Services;
using RepositoryMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=studentmanagement.db"));

// Register Repository Pattern Components
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### 8. Create Controllers

Create `Controllers/StudentController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using RepositoryMVC.Models;
using RepositoryMVC.Services;

namespace RepositoryMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var students = await _studentService.SearchStudentsAsync(searchTerm ?? string.Empty);
            ViewBag.SearchTerm = searchTerm;
            return View(students);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest("Student ID is required");
            
            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound($"Student with ID {id} not found");
            
            return View(student);
        }

        public IActionResult Create()
        {
            return View(new Student());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Gender,Branch,Section,Email,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.CreateStudentAsync(student);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating student: {ex.Message}");
                }
            }
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest("Student ID is required");
            
            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound($"Student with ID {id} not found");
            
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,Name,Gender,Branch,Section,Email,EnrollmentDate")] Student student)
        {
            if (id != student.StudentID) return BadRequest("ID mismatch");

            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _studentService.UpdateStudentAsync(student);
                    if (!success) return NotFound($"Student with ID {id} not found");
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating student: {ex.Message}");
                }
            }
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest("Student ID is required");
            
            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound($"Student with ID {id} not found");
            
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _studentService.DeleteStudentAsync(id);
                if (!success) return NotFound($"Student with ID {id} not found");
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting student: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
```

### 9. Run Migrations and Start Application

```bash
# Create and apply migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

# Run the application
dotnet run
```

## Project Structure

```
RepositoryMVC/
├── Controllers/
│   ├── HomeController.cs
│   └── StudentController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── Student.cs
│   ├── Grade.cs
│   └── ErrorViewModel.cs
├── Repositories/
│   ├── IGenericRepository.cs
│   ├── GenericRepository.cs
│   ├── IStudentRepository.cs
│   ├── StudentRepository.cs
│   ├── IGradeRepository.cs
│   └── GradeRepository.cs
├── Services/
│   └── StudentService.cs
├── Views/
│   ├── Home/
│   ├── Student/
│   └── Shared/
├── wwwroot/
├── Migrations/
├── Program.cs
├── appsettings.json
└── RepositoryMVC.csproj
```

## Design Patterns

### 1. Repository Pattern
- **Generic Repository**: Common CRUD operations for all entities
- **Specific Repositories**: Entity-specific business operations
- **Interface Segregation**: Separate interfaces for different concerns

### 2. Dependency Injection
- **Constructor Injection**: Dependencies injected through constructors
- **Interface-based**: Program against abstractions, not concretions
- **Scoped Lifetime**: Repository instances scoped to HTTP request

### 3. Service Layer Pattern
- **Business Logic**: Centralized business rules and validation
- **Transaction Management**: Coordinated operations across repositories
- **Exception Handling**: Consistent error handling strategy

## Database Schema

### Students Table
| Column | Type | Constraints |
|--------|------|-------------|
| StudentID | INTEGER | PRIMARY KEY, IDENTITY |
| Name | NVARCHAR(100) | NOT NULL |
| Gender | NVARCHAR(10) | NOT NULL, CHECK |
| Branch | NVARCHAR(50) | NOT NULL |
| Section | NVARCHAR(10) | NOT NULL |
| Email | NVARCHAR(255) | NOT NULL, UNIQUE |
| PhoneNumber | NVARCHAR(20) | NULL |
| EnrollmentDate | DATE | NOT NULL |

### Grades Table
| Column | Type | Constraints |
|--------|------|-------------|
| GradeID | INTEGER | PRIMARY KEY, IDENTITY |
| StudentID | INTEGER | FOREIGN KEY → Students.StudentID |
| Subject | NVARCHAR(100) | NOT NULL |
| GradeValue | DECIMAL(5,2) | NOT NULL, CHECK (0-100) |
| LetterGrade | NVARCHAR(2) | NULL |
| GradeDate | DATE | NOT NULL |
| Comments | NVARCHAR(500) | NULL |

### Relationships
- **One-to-Many**: Student → Grades (Cascade Delete)

## API Endpoints

### Student Management
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Student` | List all students with search |
| GET | `/Student/Details/{id}` | Get student details |
| GET | `/Student/Create` | Show create form |
| POST | `/Student/Create` | Create new student |
| GET | `/Student/Edit/{id}` | Show edit form |
| POST | `/Student/Edit/{id}` | Update student |
| GET | `/Student/Delete/{id}` | Show delete confirmation |
| POST | `/Student/Delete/{id}` | Delete student |
| GET | `/Student/Grades/{id}` | List student grades |
| GET | `/Student/AddGrade/{id}` | Show add grade form |
| POST | `/Student/AddGrade` | Add new grade |

## Testing

### Unit Testing Setup

1. **Install Testing Packages**:
```bash
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package xunit
dotnet add package Moq
```

2. **Repository Testing Example**:
```csharp
[Fact]
public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    using var context = new ApplicationDbContext(options);
    var repository = new StudentRepository(context);
    
    var students = new List<Student>
    {
        new Student { Name = "John Doe", Email = "john@test.com" },
        new Student { Name = "Jane Smith", Email = "jane@test.com" }
    };
    
    context.Students.AddRange(students);
    await context.SaveChangesAsync();

    // Act
    var result = await repository.GetAllAsync();

    // Assert
    Assert.Equal(2, result.Count());
}
```

3. **Service Testing with Mocks**:
```csharp
[Fact]
public async Task CreateStudentAsync_ShouldThrowException_WhenEmailExists()
{
    // Arrange
    var mockRepo = new Mock<IStudentRepository>();
    var mockContext = new Mock<ApplicationDbContext>();
    
    mockRepo.Setup(r => r.IsEmailExistsAsync("test@email.com", null))
           .ReturnsAsync(true);

    var service = new StudentService(mockRepo.Object, mockContext.Object);
    var student = new Student { Email = "test@email.com" };

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => service.CreateStudentAsync(student));
}
```

## Best Practices

### 1. Repository Pattern
- Keep repositories focused on data access only
- Use generic repository for common operations
- Create specific repositories for complex queries
- Always use interfaces for dependency injection

### 2. Service Layer
- Implement business logic in services
- Handle transactions at service level
- Validate business rules before data operations
- Use meaningful exception messages

### 3. Error Handling
- Use try-catch blocks in service methods
- Return meaningful error messages to users
- Log exceptions for debugging
- Use ModelState for validation errors

### 4. Performance
- Use `Include()` for eager loading when needed
- Implement pagination for large datasets
- Use `AnyAsync()` instead of `Count() > 0`
- Consider caching for frequently accessed data

### 5. Security
- Always validate user input
- Use parameterized queries (handled by EF)
- Implement authorization where needed
- Validate business rules server-side

## Troubleshooting

### Common Issues

1. **Database Connection Errors**:
   - Ensure SQLite file path is correct
   - Check database file permissions
   - Verify connection string format

2. **Migration Issues**:
   ```bash
   # Reset migrations
   dotnet ef database drop
   dotnet ef migrations remove
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

3. **Dependency Injection Errors**:
   - Verify all services are registered in `Program.cs`
   - Check interface and implementation names
   - Ensure correct lifetime scopes

4. **Model Validation Errors**:
   - Check all required fields have values
   - Verify data annotation constraints
   - Ensure proper data types

### Performance Optimization

1. **Database Queries**:
   - Use `Include()` judiciously
   - Implement proper indexing
   - Consider query optimization

2. **Memory Management**:
   - Dispose DbContext properly (handled by DI)
   - Use streaming for large datasets
   - Implement pagination

### Deployment Considerations

1. **Production Database**:
   - Switch from SQLite to SQL Server/PostgreSQL
   - Update connection strings
   - Plan migration strategy

2. **Configuration**:
   - Use environment-specific settings
   - Implement proper logging
   - Set up health checks

## Learning Resources

- [Repository Pattern Documentation](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core MVC Documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/)
- [Dependency Injection in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
- Handles business validation and rules
- Manages data persistence through DbContext
- Provides transaction support

## Project Structure

```
Repository MVC/
├── Controllers/           # MVC Controllers
├── Models/               # Entity models (Student, Grade)
├── Data/                 # DbContext and database configuration
├── Repositories/         # Repository pattern implementation
│   ├── IGenericRepository.cs
│   ├── GenericRepository.cs
│   ├── IStudentRepository.cs
│   ├── StudentRepository.cs
│   ├── IGradeRepository.cs
│   └── GradeRepository.cs
├── Services/             # Business logic layer
├── Views/               # Razor views
└── DTOs/                # Data Transfer Objects
```

## Implementation Details

### 1. Generic Repository Interface

```csharp
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}
```

### 2. Student Repository Interface

```csharp
public interface IStudentRepository : IGenericRepository<Student>
{
    Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch);
    Task<Student?> GetStudentWithGradesAsync(int studentId);
    Task<bool> StudentExistsAsync(int studentId);
}
```

### 3. Service Layer Integration

The service layer uses repositories directly through dependency injection:

```csharp
public class StudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly ApplicationDbContext _context;
    
    public StudentService(IStudentRepository studentRepository, 
                         IGradeRepository gradeRepository, 
                         ApplicationDbContext context)
    {
        _studentRepository = studentRepository;
        _gradeRepository = gradeRepository;
        _context = context;
    }
    
    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _studentRepository.GetAllStudentsWithGradesAsync();
    }
    
    public async Task<Student> CreateStudentAsync(Student student)
    {
        await _studentRepository.AddAsync(student);
        await _context.SaveChangesAsync();
        return student;
    }
}
```

## Benefits Demonstrated

### 1. **Separation of Concerns**
- Controllers handle HTTP requests/responses
- Services contain business logic
- Repositories handle data access
- Models represent domain entities

### 2. **Testability**
- Easy to mock repository interfaces directly
- Business logic can be tested without database
- Controllers can be unit tested with mocked services

### 3. **Maintainability**
- Changes to data access logic are isolated to repositories
- Database schema changes require minimal code updates
- Clear structure makes code easier to understand

### 4. **Flexibility**
- Can easily switch between different data sources
- Support for multiple database providers
- Easy to add caching, logging, or other cross-cutting concerns

## Setup Instructions

### Prerequisites
- .NET 8.0 SDK
- Visual Studio Code or Visual Studio
- SQLite (included with Entity Framework Core)

### Step 1: Clone and Build
```bash
# Navigate to the project directory
cd "Repository MVC"

# Restore dependencies
dotnet restore

# Build the project
dotnet build
```

### Step 2: Database Setup
```bash
# Create initial migration (if not exists)
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Step 3: Run the Application
```bash
# Start the application
dotnet run

# Navigate to http://localhost:5034
```

## Features Demonstrated

### Student Management
- **List Students** - View all students with repository pattern
- **Add Student** - Create new student using repository
- **Edit Student** - Update student information
- **Delete Student** - Remove student and related grades
- **View Details** - Display student information and grades

### Grade Management
- **Add Grades** - Assign grades to students
- **View Grades** - Display student grades
- **Manage Relationships** - Handle student-grade relationships

## Learning Objectives

After studying this project, you will understand:

1. **Repository Pattern Implementation**
   - How to create generic and specific repositories
   - Interface segregation and dependency inversion
   - Repository method design and implementation

2. **Unit of Work Pattern**
   - Managing multiple repositories
   - Transaction coordination
   - Resource management and disposal

3. **Dependency Injection**
   - Registering repositories and services
   - Constructor injection patterns
   - Service lifetime management

4. **Clean Architecture Principles**
   - Layered architecture design
   - Separation of concerns
   - Loose coupling and high cohesion

## Key Files to Study

1. **`Repositories/IGenericRepository.cs`** - Generic repository interface
2. **`Repositories/GenericRepository.cs`** - Generic repository implementation
3. **`Repositories/IStudentRepository.cs`** - Student-specific repository interface
4. **`Repositories/StudentRepository.cs`** - Student repository implementation
5. **`Repositories/IGradeRepository.cs`** - Grade-specific repository interface
6. **`Repositories/GradeRepository.cs`** - Grade repository implementation
7. **`Services/StudentService.cs`** - Service layer using repositories directly
8. **`Program.cs`** - Dependency injection configuration

## Best Practices Demonstrated

- **Interface-based programming** - All repositories use interfaces
- **Async/await patterns** - All data operations are asynchronous
- **Resource management** - Proper disposal of database contexts
- **Error handling** - Graceful handling of database operations
- **Code documentation** - Comprehensive comments explaining concepts

## Common Patterns Explained

### Repository Registration in DI Container
```csharp
// Program.cs
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IStudentService, StudentService>();
```

### Using Repository in Controllers
```csharp
public class StudentController : Controller
{
    private readonly StudentService _studentService;
    
    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }
    
    public async Task<IActionResult> Index()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return View(students);
    }
}
```

## Step-by-Step Implementation Guide

This section provides a detailed, step-by-step guide on how to implement the Repository Design Pattern from scratch. Follow these steps to transform a basic MVC application into one that uses the Repository pattern with direct repository usage.

### Phase 1: Project Setup and Preparation

#### Step 1: Create Project Structure
First, create the necessary folder structure for organizing your repositories:

```bash
# Create the main Repositories folder
mkdir Repositories
```

#### Step 2: Analyze Your Current Architecture
Before implementing the Repository pattern, identify:
- **Domain Models**: Your entity classes (Student, Grade, etc.)
- **Data Context**: Your DbContext class
- **Current Data Access**: Where you directly use DbContext
- **Services**: Business logic that needs to use repositories

### Phase 2: Create Repository Interfaces

#### Step 3: Create Generic Repository Interface
Create `Repositories/IGenericRepository.cs`:

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Generic repository interface that defines common CRUD operations
    /// This interface can be used for any entity type
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities from the database
        /// </summary>
        /// <returns>Collection of all entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves a single entity by its ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new entity to the database
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the database
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Removes an entity from the database
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        void Remove(T entity);

        /// <summary>
        /// Checks if any entity matches the given predicate
        /// </summary>
        /// <param name="predicate">The condition to check</param>
        /// <returns>True if any entity matches, false otherwise</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
```

#### Step 4: Create Specific Repository Interfaces
Create `Repositories/IStudentRepository.cs`:

```csharp
using RepositoryMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Student-specific repository interface
    /// Inherits common CRUD operations from IGenericRepository
    /// and adds student-specific methods
    /// </summary>
    public interface IStudentRepository : IGenericRepository<Student>
    {
        /// <summary>
        /// Gets all students with their grades included
        /// </summary>
        /// <returns>Students with grades loaded</returns>
        Task<IEnumerable<Student>> GetAllStudentsWithGradesAsync();

        /// <summary>
        /// Gets a student with all their grades included
        /// </summary>
        /// <param name="studentId">The student ID</param>
        /// <returns>Student with grades, or null if not found</returns>
        Task<Student?> GetStudentWithGradesAsync(int studentId);

        /// <summary>
        /// Checks if a student email already exists
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <param name="excludeStudentId">Student ID to exclude from check (for updates)</param>
        /// <returns>True if email exists, false otherwise</returns>
        Task<bool> IsEmailExistsAsync(string email, int? excludeStudentId = null);

        /// <summary>
        /// Searches students by name, email, or branch
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>Matching students</returns>
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);
    }
}
```

Create `Repositories/IGradeRepository.cs`:

```csharp
using RepositoryMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Grade-specific repository interface
    /// Inherits common CRUD operations and adds grade-specific methods
    /// </summary>
    public interface IGradeRepository : IGenericRepository<Grade>
    {
        /// <summary>
        /// Gets all grades for a specific student
        /// </summary>
        /// <param name="studentId">The student ID</param>
        /// <returns>All grades for the student</returns>
        Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId);

        /// <summary>
        /// Checks if a grade already exists for a student on a specific date and subject
        /// </summary>
        /// <param name="studentId">The student ID</param>
        /// <param name="subject">The subject</param>
        /// <param name="gradeDate">The grade date</param>
        /// <returns>True if grade exists, false otherwise</returns>
        Task<bool> IsGradeExistsAsync(int studentId, string subject, DateTime gradeDate);
    }
}
```

### Phase 3: Implement Repository Classes

#### Step 5: Create Generic Repository Implementation
Create `Repositories/GenericRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Generic repository implementation that provides common CRUD operations
    /// for any entity type. This eliminates code duplication across repositories.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor that receives the database context
        /// </summary>
        /// <param name="context">The database context</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Retrieves all entities asynchronously
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Retrieves a single entity by ID asynchronously
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Adds a new entity asynchronously
        /// </summary>
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Removes an entity
        /// </summary>
        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Checks if any entity matches the given predicate
        /// </summary>
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
```

#### Step 6: Create Specific Repository Implementations
Create `Repositories/StudentRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Student repository implementation that inherits common operations
    /// from GenericRepository and adds student-specific functionality
    /// </summary>
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        /// <summary>
        /// Constructor that passes context to base generic repository
        /// </summary>
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all students with their grades included
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllStudentsWithGradesAsync()
        {
            return await _dbSet
                .Include(s => s.Grades)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a student with all related grades included
        /// Uses Entity Framework's Include method for eager loading
        /// </summary>
        public async Task<Student?> GetStudentWithGradesAsync(int studentId)
        {
            return await _dbSet
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);
        }

        /// <summary>
        /// Checks if a student email already exists
        /// </summary>
        public async Task<bool> IsEmailExistsAsync(string email, int? excludeStudentId = null)
        {
            var query = _dbSet.Where(s => s.Email == email);
            
            if (excludeStudentId.HasValue)
            {
                query = query.Where(s => s.StudentID != excludeStudentId.Value);
            }
            
            return await query.AnyAsync();
        }

        /// <summary>
        /// Searches students by name, email, or branch
        /// </summary>
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllStudentsWithGradesAsync();
            }

            return await _dbSet
                .Include(s => s.Grades)
                .Where(s => s.Name.Contains(searchTerm) ||
                           s.Email.Contains(searchTerm) ||
                           s.Branch.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
```

Create `Repositories/GradeRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Grade repository implementation with grade-specific operations
    /// </summary>
    public class GradeRepository : GenericRepository<Grade>, IGradeRepository
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all grades for a specific student
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            return await _dbSet
                .Where(g => g.StudentID == studentId)
                .OrderByDescending(g => g.GradeDate)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a grade already exists for a student on a specific date and subject
        /// </summary>
        public async Task<bool> IsGradeExistsAsync(int studentId, string subject, DateTime gradeDate)
        {
            return await _dbSet.AnyAsync(g => 
                g.StudentID == studentId && 
                g.Subject == subject && 
                g.GradeDate.Date == gradeDate.Date);
        }
    }
}
```

### Phase 4: Configure Dependency Injection

#### Step 7: Register Repositories in DI Container
Update your `Program.cs` to register all repositories:

```csharp
// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register Specific Repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();

// Register Services
builder.Services.AddScoped<IStudentService, StudentService>();
```

### Phase 5: Create and Update Service Classes

#### Step 8: Create Service Classes to Use Repositories
Create or modify your `Services/StudentService.cs` to use repositories directly:

```csharp
using RepositoryMVC.Models;
using RepositoryMVC.Repositories;
using RepositoryMVC.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Services
{
    /// <summary>
    /// Student service that contains business logic and uses repositories
    /// for data access with direct DbContext for transaction management
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor with repository and context dependency injection
        /// </summary>
        public StudentService(IStudentRepository studentRepository, 
                             IGradeRepository gradeRepository, 
                             ApplicationDbContext context)
        {
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _context = context;
        }

        /// <summary>
        /// Business logic: Get all students with grades
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsWithGradesAsync();
        }

        /// <summary>
        /// Business logic: Get student by ID with grades
        /// </summary>
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetStudentWithGradesAsync(id);
        }

        /// <summary>
        /// Business logic: Create new student
        /// Includes validation and business rules
        /// </summary>
        public async Task<Student> CreateStudentAsync(Student student)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(student.Name))
                throw new ArgumentException("Student name is required");

            if (await _studentRepository.IsEmailExistsAsync(student.Email))
                throw new InvalidOperationException($"Email {student.Email} already exists");

            // Set enrollment date if not provided
            if (student.EnrollmentDate == default(DateTime))
                student.EnrollmentDate = DateTime.Today;

            // Use repository to add student
            await _studentRepository.AddAsync(student);
            
            // Save changes through DbContext
            await _context.SaveChangesAsync();
            
            return student;
        }

        /// <summary>
        /// Business logic: Update student
        /// </summary>
        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                // Check if email is taken by another student
                if (await _studentRepository.IsEmailExistsAsync(student.Email, student.StudentID))
                    throw new InvalidOperationException($"Email {student.Email} is already taken");

                // Update through repository
                _studentRepository.Update(student);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Business logic: Delete student
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return false;

            _studentRepository.Remove(student);
            await _context.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// Business logic: Search students
        /// </summary>
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            return await _studentRepository.SearchStudentsAsync(searchTerm);
        }

        /// <summary>
        /// Business logic: Add grade to student
        /// </summary>
        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            // Validate student exists
            if (!await _studentRepository.AnyAsync(s => s.StudentID == grade.StudentID))
                throw new InvalidOperationException($"Student with ID {grade.StudentID} does not exist");

            // Check for duplicate grades
            if (await _gradeRepository.IsGradeExistsAsync(grade.StudentID, grade.Subject, grade.GradeDate))
                throw new InvalidOperationException($"A grade for {grade.Subject} already exists for this date");

            // Business logic: calculate letter grade if not provided
            if (string.IsNullOrEmpty(grade.LetterGrade))
                grade.LetterGrade = grade.CalculateLetterGrade();

            // Set grade date if not provided
            if (grade.GradeDate == default(DateTime))
                grade.GradeDate = DateTime.Today;

            await _gradeRepository.AddAsync(grade);
            await _context.SaveChangesAsync();
            
            return grade;
        }

        /// <summary>
        /// Business logic: Get student grades
        /// </summary>
        public async Task<IEnumerable<Grade>> GetStudentGradesAsync(int studentId)
        {
            return await _gradeRepository.GetGradesByStudentIdAsync(studentId);
        }

        /// <summary>
        /// Business logic: Check if student exists
        /// </summary>
        public async Task<bool> StudentExistsAsync(int id)
        {
            return await _studentRepository.AnyAsync(s => s.StudentID == id);
        }
    }

    /// <summary>
    /// Interface for StudentService
    /// </summary>
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> StudentExistsAsync(int id);
        Task<IEnumerable<Grade>> GetStudentGradesAsync(int studentId);
        Task<Grade> AddGradeAsync(Grade grade);
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);
    }
}
```

### Phase 6: Update Controllers

#### Step 9: Refactor Controllers to Use Services
Update your controllers to use the service layer:

```csharp
using Microsoft.AspNetCore.Mvc;
using RepositoryMVC.Models;
using RepositoryMVC.Services;

namespace RepositoryMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                var students = await _studentService.SearchStudentsAsync(searchTerm ?? string.Empty);
                ViewBag.SearchTerm = searchTerm;
                return View(students);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error retrieving students: " + ex.Message;
                return View(new List<Student>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                    return NotFound();
                    
                return View(student);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error retrieving student: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.CreateStudentAsync(student);
                    TempData["Success"] = "Student created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(student);
        }
    }
}
```

### Phase 7: Testing and Validation

#### Step 10: Test Your Implementation
1. **Build the Project**:
   ```bash
   dotnet build
   ```

2. **Run the Application**:
   ```bash
   dotnet run
   ```

3. **Test Repository Methods**:
   - Create a few test students
   - Verify CRUD operations work
   - Test specific repository methods
   - Check that transactions work properly

## Summary of Changes

This implementation has been simplified by **removing the Unit of Work pattern**. Here are the key changes made:

### What Was Removed:
- `IUnitOfWork` interface
- `UnitOfWork` implementation class
- Unit of Work dependency injection registration

### What Was Changed:

#### 1. Service Layer (`StudentService.cs`)
- **Before**: Services received `IUnitOfWork` through dependency injection
- **After**: Services receive repositories and `ApplicationDbContext` directly
- **Transaction Management**: Now handled directly through `ApplicationDbContext.SaveChangesAsync()`

#### 2. Dependency Injection (`Program.cs`)
- **Before**: Registered Unit of Work as the coordinator
- **After**: Register repositories and services directly
- **Simpler Registration**: Fewer dependencies to manage

#### 3. Repository Usage
- **Before**: `_unitOfWork.Students.GetAllAsync()`
- **After**: `_studentRepository.GetAllAsync()`
- **Direct Access**: No intermediate abstraction layer

### Benefits of This Simplified Approach:

1. **Easier to Learn**: Fewer concepts to understand
2. **Simpler Architecture**: Direct repository usage without additional abstraction
3. **Better Performance**: No overhead from Unit of Work coordination
4. **Clearer Dependencies**: Explicit dependencies in constructor injection
5. **Flexible Transaction Control**: Can manage transactions at service level when needed

### When to Use This Pattern:

This simplified Repository pattern is perfect for:
- ✅ Learning projects and educational purposes
- ✅ Small to medium-sized applications
- ✅ Applications with straightforward data access needs
- ✅ Teams new to Repository pattern
- ✅ Projects where simplicity is preferred

### When You Might Need Unit of Work:

Consider adding Unit of Work back when you have:
- Complex multi-entity transactions
- Need for explicit transaction boundaries
- Multiple repositories that must be coordinated
- Advanced transaction rollback scenarios

---
