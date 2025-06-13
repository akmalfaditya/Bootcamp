# Repository Design Pattern Demo - ASP.NET Core MVC

## Overview

This project demonstrates the implementation of the **Repository Design Pattern** in an ASP.NET Core MVC application. The Repository pattern is a design pattern that encapsulates the logic needed to access data sources, centralizing common data access functionality, and providing better maintainability and decoupling the infrastructure or technology used to access databases from the domain model layer.

## What is the Repository Design Pattern?

The Repository Design Pattern is a structural pattern that:

- **Encapsulates data access logic** - All database operations are contained within repository classes
- **Provides abstraction** - Controllers and services work with interfaces, not concrete implementations  
- **Enables testability** - Easy to mock repositories for unit testing
- **Promotes separation of concerns** - Business logic is separated from data access logic
- **Supports multiple data sources** - Can easily switch between different data storage technologies

## Project Architecture

### Traditional MVC vs Repository Pattern

**Before (Traditional MVC):**
```
Controller → Service → DbContext (Direct database access)
```

**After (Repository Pattern):**
```
Controller → Service → Repository Interface → Repository Implementation → DbContext
```

### Key Components

#### 1. Generic Repository (`IGenericRepository<T>`)
Provides common CRUD operations for any entity:
- `GetAllAsync()` - Retrieve all entities
- `GetByIdAsync(id)` - Get entity by ID
- `AddAsync(entity)` - Add new entity
- `UpdateAsync(entity)` - Update existing entity
- `DeleteAsync(id)` - Delete entity by ID

#### 2. Specific Repositories
Entity-specific repositories that inherit from generic repository and add specialized methods:
- `IStudentRepository` - Student-specific operations
- `IGradeRepository` - Grade-specific operations

#### 3. Unit of Work Pattern (`IUnitOfWork`)
Manages multiple repositories and coordinates database transactions:
- Groups related operations
- Ensures data consistency
- Manages database context lifecycle
- Provides transaction support

## Project Structure

```
Repository MVC/
├── Controllers/           # MVC Controllers
├── Models/               # Entity models (Student, Grade)
├── Data/                 # DbContext and database configuration
├── Repositories/         # Repository pattern implementation
│   ├── Interfaces/       # Repository interfaces
│   ├── Implementations/  # Repository concrete classes
│   ├── IGenericRepository.cs
│   ├── GenericRepository.cs
│   ├── IStudentRepository.cs
│   ├── StudentRepository.cs
│   ├── IGradeRepository.cs
│   ├── GradeRepository.cs
│   ├── IUnitOfWork.cs
│   └── UnitOfWork.cs
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

### 3. Unit of Work Interface

```csharp
public interface IUnitOfWork : IDisposable
{
    IStudentRepository Students { get; }
    IGradeRepository Grades { get; }
    Task<int> SaveChangesAsync();
}
```

### 4. Service Layer Integration

The service layer uses repositories through dependency injection:

```csharp
public class StudentService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public StudentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _unitOfWork.Students.GetAllAsync();
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
- Easy to mock `IUnitOfWork` and repository interfaces
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
3. **`Repositories/IUnitOfWork.cs`** - Unit of work interface
4. **`Repositories/UnitOfWork.cs`** - Unit of work implementation
5. **`Services/StudentService.cs`** - Service layer using repositories
6. **`Program.cs`** - Dependency injection configuration

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
builder.Services.AddScoped<IGenericRepository<Student>, GenericRepository<Student>>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
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

This section provides a detailed, step-by-step guide on how to implement the Repository Design Pattern from scratch. Follow these steps to transform a basic MVC application into one that uses the Repository pattern.

### Phase 1: Project Setup and Preparation

#### Step 1: Create Project Structure
First, create the necessary folder structure for organizing your repositories:

```bash
# Create the main Repositories folder
mkdir Repositories

# Create subfolders for better organization
mkdir Repositories/Interfaces
mkdir Repositories/Implementations
```

#### Step 2: Analyze Your Current Architecture
Before implementing the Repository pattern, identify:
- **Domain Models**: Your entity classes (Student, Grade, etc.)
- **Data Context**: Your DbContext class
- **Current Data Access**: Where you directly use DbContext
- **Services**: Business logic that needs to use repositories

### Phase 2: Create Repository Interfaces

#### Step 3: Create Generic Repository Interface
Create `Repositories/Interfaces/IGenericRepository.cs`:

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories.Interfaces
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
        /// <returns>The updated entity</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
}
```

#### Step 4: Create Specific Repository Interfaces
Create `Repositories/Interfaces/IStudentRepository.cs`:

```csharp
using RepositoryMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories.Interfaces
{
    /// <summary>
    /// Student-specific repository interface
    /// Inherits common CRUD operations from IGenericRepository
    /// and adds student-specific methods
    /// </summary>
    public interface IStudentRepository : IGenericRepository<Student>
    {
        /// <summary>
        /// Gets all students from a specific branch
        /// </summary>
        /// <param name="branch">The branch name</param>
        /// <returns>Students in the specified branch</returns>
        Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch);

        /// <summary>
        /// Gets a student with all their grades included
        /// </summary>
        /// <param name="studentId">The student ID</param>
        /// <returns>Student with grades, or null if not found</returns>
        Task<Student?> GetStudentWithGradesAsync(int studentId);

        /// <summary>
        /// Checks if a student exists in the database
        /// </summary>
        /// <param name="studentId">The student ID to check</param>
        /// <returns>True if student exists, false otherwise</returns>
        Task<bool> StudentExistsAsync(int studentId);
    }
}
```

Create `Repositories/Interfaces/IGradeRepository.cs`:

```csharp
using RepositoryMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories.Interfaces
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
        /// Gets all grades for a specific subject
        /// </summary>
        /// <param name="subject">The subject name</param>
        /// <returns>All grades for the subject</returns>
        Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject);

        /// <summary>
        /// Gets the average grade for a student
        /// </summary>
        /// <param name="studentId">The student ID</param>
        /// <returns>Average grade value, or null if no grades</returns>
        Task<decimal?> GetAverageGradeAsync(int studentId);
    }
}
```

### Phase 3: Create Unit of Work Interface

#### Step 5: Create Unit of Work Interface
Create `Repositories/Interfaces/IUnitOfWork.cs`:

```csharp
using System;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories.Interfaces
{
    /// <summary>
    /// Unit of Work interface that coordinates multiple repositories
    /// and manages database transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Student repository instance
        /// </summary>
        IStudentRepository Students { get; }

        /// <summary>
        /// Grade repository instance
        /// </summary>
        IGradeRepository Grades { get; }

        /// <summary>
        /// Saves all changes made through repositories to the database
        /// This coordinates transactions across multiple repositories
        /// </summary>
        /// <returns>Number of affected records</returns>
        Task<int> SaveChangesAsync();
    }
}
```

### Phase 4: Implement Repository Classes

#### Step 6: Create Generic Repository Implementation
Create `Repositories/Implementations/GenericRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories.Implementations
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
        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        /// <summary>
        /// Deletes an entity by ID asynchronously
        /// </summary>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            return false;
        }
    }
}
```

#### Step 7: Create Specific Repository Implementations
Create `Repositories/Implementations/StudentRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Models;
using RepositoryMVC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories.Implementations
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
        /// Gets students filtered by branch
        /// </summary>
        public async Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch)
        {
            return await _dbSet
                .Where(s => s.Branch.ToLower() == branch.ToLower())
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
        /// Checks if a student exists without loading the entire entity
        /// </summary>
        public async Task<bool> StudentExistsAsync(int studentId)
        {
            return await _dbSet.AnyAsync(s => s.StudentID == studentId);
        }
    }
}
```

Create `Repositories/Implementations/GradeRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Models;
using RepositoryMVC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories.Implementations
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
        /// Gets all grades for a specific subject across all students
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject)
        {
            return await _dbSet
                .Include(g => g.Student)
                .Where(g => g.Subject.ToLower() == subject.ToLower())
                .ToListAsync();
        }

        /// <summary>
        /// Calculates the average grade for a student
        /// </summary>
        public async Task<decimal?> GetAverageGradeAsync(int studentId)
        {
            var grades = await _dbSet
                .Where(g => g.StudentID == studentId)
                .Select(g => g.GradeValue)
                .ToListAsync();

            return grades.Any() ? grades.Average() : null;
        }
    }
}
```

### Phase 5: Implement Unit of Work

#### Step 8: Create Unit of Work Implementation
Create `Repositories/Implementations/UnitOfWork.cs`:

```csharp
using RepositoryMVC.Data;
using RepositoryMVC.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace RepositoryMVC.Repositories.Implementations
{
    /// <summary>
    /// Unit of Work implementation that coordinates multiple repositories
    /// and manages the database context lifecycle
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IStudentRepository? _studentRepository;
        private IGradeRepository? _gradeRepository;

        /// <summary>
        /// Constructor that receives the database context
        /// </summary>
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lazy-loaded Student repository property
        /// Creates the repository only when first accessed
        /// </summary>
        public IStudentRepository Students
        {
            get
            {
                _studentRepository ??= new StudentRepository(_context);
                return _studentRepository;
            }
        }

        /// <summary>
        /// Lazy-loaded Grade repository property
        /// </summary>
        public IGradeRepository Grades
        {
            get
            {
                _gradeRepository ??= new GradeRepository(_context);
                return _gradeRepository;
            }
        }

        /// <summary>
        /// Saves all changes made through any repository to the database
        /// This ensures all operations are part of the same transaction
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the database context when the Unit of Work is disposed
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
```

### Phase 6: Configure Dependency Injection

#### Step 9: Register Repositories in DI Container
Update your `Program.cs` to register all repositories and the Unit of Work:

```csharp
// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Generic Repository (if needed separately)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register Specific Repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Services (if you have them)
builder.Services.AddScoped<StudentService>();
```

### Phase 7: Refactor Services

#### Step 10: Update Service Classes to Use Repositories
Modify your `Services/StudentService.cs` to use the Unit of Work instead of direct DbContext:

```csharp
using RepositoryMVC.Models;
using RepositoryMVC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryMVC.Services
{
    /// <summary>
    /// Student service that contains business logic and uses repositories
    /// for data access instead of directly accessing the database context
    /// </summary>
    public class StudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor with Unit of Work dependency injection
        /// </summary>
        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Business logic: Get all students
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _unitOfWork.Students.GetAllAsync();
        }

        /// <summary>
        /// Business logic: Get student by ID
        /// </summary>
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _unitOfWork.Students.GetByIdAsync(id);
        }

        /// <summary>
        /// Business logic: Get student with grades
        /// </summary>
        public async Task<Student?> GetStudentWithGradesAsync(int id)
        {
            return await _unitOfWork.Students.GetStudentWithGradesAsync(id);
        }

        /// <summary>
        /// Business logic: Add new student
        /// Includes validation and business rules
        /// </summary>
        public async Task<Student> CreateStudentAsync(Student student)
        {
            // Add any business validation here
            if (string.IsNullOrWhiteSpace(student.Name))
                throw new ArgumentException("Student name is required");

            if (string.IsNullOrWhiteSpace(student.Email))
                throw new ArgumentException("Student email is required");

            // Use repository to add student
            var addedStudent = await _unitOfWork.Students.AddAsync(student);
            
            // Save changes through Unit of Work
            await _unitOfWork.SaveChangesAsync();
            
            return addedStudent;
        }

        /// <summary>
        /// Business logic: Update student
        /// </summary>
        public async Task<Student> UpdateStudentAsync(Student student)
        {
            // Check if student exists
            var existingStudent = await _unitOfWork.Students.GetByIdAsync(student.StudentID);
            if (existingStudent == null)
                throw new ArgumentException("Student not found");

            // Update through repository
            var updatedStudent = await _unitOfWork.Students.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();
            
            return updatedStudent;
        }

        /// <summary>
        /// Business logic: Delete student
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var deleted = await _unitOfWork.Students.DeleteAsync(id);
            if (deleted)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            return deleted;
        }

        /// <summary>
        /// Business logic: Get students by branch
        /// </summary>
        public async Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch)
        {
            return await _unitOfWork.Students.GetStudentsByBranchAsync(branch);
        }
    }
}
```

### Phase 8: Update Controllers

#### Step 11: Refactor Controllers to Use Services
Update your controllers to use the service layer instead of direct repository access:

```csharp
using Microsoft.AspNetCore.Mvc;
using RepositoryMVC.Models;
using RepositoryMVC.Services;
using System.Threading.Tasks;

namespace RepositoryMVC.Controllers
{
    /// <summary>
    /// Student controller that uses the service layer
    /// Controllers should contain minimal logic and delegate to services
    /// </summary>
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;

        /// <summary>
        /// Constructor with service dependency injection
        /// </summary>
        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Display list of all students
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return View(students);
            }
            catch (Exception ex)
            {
                // Handle errors appropriately
                TempData["Error"] = "Error retrieving students: " + ex.Message;
                return View(new List<Student>());
            }
        }

        /// <summary>
        /// Display student details with grades
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var student = await _studentService.GetStudentWithGradesAsync(id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error retrieving student details: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Create new student - POST action
        /// </summary>
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
                    ModelState.AddModelError("", "Error creating student: " + ex.Message);
                }
            }
            return View(student);
        }
    }
}
```

### Phase 9: Testing and Validation

#### Step 12: Test Your Implementation
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

4. **Verify Dependency Injection**:
   - Ensure all repositories are properly injected
   - Check that Unit of Work coordinates multiple repositories
   - Validate that services receive repositories correctly

### Phase 10: Advanced Patterns (Optional)

#### Step 13: Add Specification Pattern (Advanced)
For complex queries, consider implementing the Specification pattern:

```csharp
// Create base specification interface
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
}

// Example specification for students
public class StudentsByBranchSpecification : ISpecification<Student>
{
    public StudentsByBranchSpecification(string branch)
    {
        Criteria = s => s.Branch == branch;
    }

    public Expression<Func<Student, bool>> Criteria { get; }
    public List<Expression<Func<Student, object>>> Includes { get; } = new();
}
```

#### Step 14: Add Repository Caching (Advanced)
Implement caching in your repositories for better performance:

```csharp
public class CachedStudentRepository : IStudentRepository
{
    private readonly IStudentRepository _repository;
    private readonly IMemoryCache _cache;

    public CachedStudentRepository(IStudentRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        const string cacheKey = "all_students";
        
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<Student> students))
        {
            students = await _repository.GetAllAsync();
            _cache.Set(cacheKey, students, TimeSpan.FromMinutes(5));
        }
        
        return students;
    }
}
```

### Common Pitfalls and Solutions

#### Pitfall 1: Forgetting to Save Changes
**Problem**: Adding/updating entities but forgetting to call `SaveChangesAsync()`

**Solution**: Always call `await _unitOfWork.SaveChangesAsync()` after repository operations

#### Pitfall 2: Memory Leaks
**Problem**: Not disposing the Unit of Work properly

**Solution**: Always implement `IDisposable` and use `using` statements or dependency injection

#### Pitfall 3: Circular Dependencies
**Problem**: Services referencing each other through repositories

**Solution**: Carefully design your service layer to avoid circular references

#### Pitfall 4: Over-Engineering
**Problem**: Creating too many specific repository methods

**Solution**: Use the generic repository for simple operations, only create specific methods when needed

### Conclusion

Following these steps will give you a clean, maintainable implementation of the Repository Design Pattern. The pattern provides:

- **Separation of Concerns**: Data access is separated from business logic
- **Testability**: Easy to mock repositories for unit testing
- **Maintainability**: Changes to data access are isolated
- **Flexibility**: Easy to switch data sources or add caching

Remember to start simple and add complexity only when needed. The Repository pattern is powerful but should not be over-engineered for simple applications.

By studying and understanding this implementation, you'll be well-equipped to apply the Repository pattern in your own ASP.NET Core applications.
