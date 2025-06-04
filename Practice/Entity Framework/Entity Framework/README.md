# Entity Framework Core Demo Project

## Overview

Welcome to this comprehensive Entity Framework Core demonstration project! This hands-on example is designed to teach you the fundamentals of working with EF Core in .NET applications. Rather than just reading about concepts, you'll see them in action with real, working code.

## Step-by-Step Tutorial: Building This Project from Scratch

Follow this complete tutorial to create your own Entity Framework Core project from the ground up.

### Prerequisites

Before starting, ensure you have:
- **.NET 8.0 SDK** or later installed ([Download here](https://dotnet.microsoft.com/download))
- **Visual Studio Code** or **Visual Studio** IDE
- **SQLite Browser** (optional, for viewing database)

### Step 1: Create New Console Project

```powershell
# Create new directory for your project
mkdir "Entity Framework Demo"
cd "Entity Framework Demo"

# Create new console application
dotnet new console -n "Entity Framework"
cd "Entity Framework"
```

### Step 2: Install Required NuGet Packages

```powershell
# Install Entity Framework Core with SQLite provider
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

# Install EF Core Design package for migrations
dotnet add package Microsoft.EntityFrameworkCore.Design

# Install EF Core Tools globally (one-time setup)
dotnet tool install --global dotnet-ef
```

### Step 3: Create Entity Models

Create the `Models/` folder and add your entity classes:

**Models/Employee.cs**
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        
        public DateTime HireDate { get; set; }
        
        [MaxLength(100)]
        public string? Position { get; set; }
        
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        
        public bool IsActive { get; set; } = true;
    }
}
```

**Models/Department.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace Entity_Framework.Models
{
    public class Department
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public decimal Budget { get; set; }
        
        public int? ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }
        
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
```

**Models/Project.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace Entity_Framework.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public decimal Budget { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } = "Active";
        
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
```

### Step 4: Create DbContext

Create the `Data/` folder and add your DbContext:

**Data/CompanyDbContext.cs**
```csharp
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Data
{
    public class CompanyDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }
        public CompanyDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=CompanyDatabase.db");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(d => d.Name).IsUnique();
                entity.HasOne(d => d.Manager)
                      .WithMany()
                      .HasForeignKey(d => d.ManagerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure many-to-many relationship
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Projects)
                .WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeProject",
                    j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                    j => j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "ProjectId");
                        j.ToTable("EmployeeProjects");
                    });
        }
    }
}
```

### Step 5: Create Service Layer

Create the `Services/` folder for business logic:

**Services/EmployeeService.cs**
```csharp
using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services
{
    public class EmployeeService
    {
        private readonly CompanyDbContext _context;

        public EmployeeService(CompanyDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == employee.Email);

            if (existingEmployee != null)
                throw new InvalidOperationException($"Employee with email {employee.Email} already exists");

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        // READ
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Projects)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // UPDATE
        public async Task<Employee?> UpdateEmployeeAsync(int id, Employee updatedEmployee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null) return null;

            existingEmployee.Name = updatedEmployee.Name;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.Salary = updatedEmployee.Salary;
            existingEmployee.Position = updatedEmployee.Position;

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        // DELETE (Soft Delete)
        public async Task<bool> DeactivateEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            employee.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

### Step 6: Setup Database with Migrations

```powershell
# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migrations to database
dotnet ef database update
```

### Step 7: Create Seeding Method

Add to your `Program.cs`:

```csharp
static async Task SeedDatabaseAsync(CompanyDbContext context)
{
    if (await context.Departments.AnyAsync()) return;

    var departments = new[]
    {
        new Department { Name = "Engineering", Description = "Software development", Budget = 500000m },
        new Department { Name = "Sales", Description = "Revenue generation", Budget = 300000m },
        // ... more departments
    };
    
    context.Departments.AddRange(departments);
    await context.SaveChangesAsync();

    // Add employees and projects similarly...
}
```

### Step 8: Create Main Program

Update your `Program.cs`:

```csharp
using Entity_Framework.Data;
using Entity_Framework.Services;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Entity Framework Core Demo ===");

            using var context = new CompanyDbContext();
            
            // Apply migrations
            await context.Database.MigrateAsync();
            
            // Seed data
            await SeedDatabaseAsync(context);

            // Initialize services
            var employeeService = new EmployeeService(context);

            // Demo CRUD operations
            await DemonstrateCrudOperationsAsync(employeeService);
        }

        static async Task DemonstrateCrudOperationsAsync(EmployeeService service)
        {
            // Your demo code here...
        }
    }
}
```

### Step 9: Setup .gitignore

Create `.gitignore` file:

```gitignore
# SQLite database files
*.db
*.db-shm
*.db-wal

# .NET build outputs
bin/
obj/
*.user
*.suo
```

### Step 10: Run and Test

```powershell
# Build the project
dotnet build

# Run the application
dotnet run
```

## What You'll Learn

This project covers all the essential EF Core concepts through practical examples:

### Core Concepts
- **DbContext**: The heart of EF Core that manages your database connection and entity tracking
- **Entities**: How C# classes map to database tables
- **LINQ Queries**: Writing database queries using familiar C# syntax
- **Relationships**: Handling one-to-many and many-to-many relationships between entities

### CRUD Operations
- **Create**: Adding new records to the database
- **Read**: Querying data with various filtering and sorting options
- **Update**: Modifying existing records efficiently
- **Delete**: Both hard deletes and soft deletes (marking as inactive)

### Advanced Features
- **Eager Loading**: Loading related data in a single query using `Include()`
- **Aggregations**: Performing calculations like averages, sums, and counts
- **Complex Queries**: Combining multiple conditions and joins
- **Data Seeding**: Populating your database with initial data

## Project Structure

```
Models/
├── Employee.cs      # Employee entity with relationships to Department and Projects
├── Department.cs    # Department entity with employee collection and manager reference
└── Project.cs       # Project entity for many-to-many relationship demonstration

Data/
└── CompanyDbContext.cs  # Main DbContext with configuration and seeding

Services/
├── EmployeeService.cs    # Complete CRUD operations for employees
└── DepartmentService.cs  # Department management operations

Program.cs           # Main application with comprehensive demos
```

## Database Schema

The project creates a SQLite database with the following structure:

- **Departments**: Company departments with budget information
- **Employees**: Staff members with personal and job information
- **Projects**: Work projects that employees can be assigned to
- **EmployeeProjects**: Junction table for many-to-many relationships

## Key Features Demonstrated

### 1. Entity Relationships
- **One-to-Many**: Department → Employees
- **Many-to-Many**: Employees ↔ Projects
- **Self-Referencing**: Department → Manager (Employee)

### 2. Data Validation
- Required fields using Data Annotations
- Unique constraints (email addresses, department names)
- Business rule enforcement in service classes

### 3. Query Patterns
```csharp
// Simple filtering
var engineerEmployees = await context.Employees
    .Where(e => e.Department.Name == "Engineering")
    .ToListAsync();

// Complex aggregations
var departmentStats = await context.Employees
    .GroupBy(e => e.Department.Name)
    .Select(g => new {
        Department = g.Key,
        EmployeeCount = g.Count(),
        AverageSalary = g.Average(e => e.Salary)
    })
    .ToListAsync();
```

### 4. Change Tracking
EF Core automatically tracks changes to your entities, so you only need to call `SaveChanges()` to persist modifications to the database.

## Running the Project

1. **Prerequisites**: Ensure you have .NET 8.0 or later installed
2. **Restore packages**: The project will automatically restore NuGet packages
3. **Run the application**: Execute `dotnet run` in the project directory
4. **Explore the database**: After running, you'll find `CompanyDatabase.db` in your project folder

## Learning Path

The `Program.cs` file runs several demonstration methods in sequence:

1. **CRUD Operations Demo**: Basic create, read, update, delete operations
2. **Complex Queries Demo**: Advanced filtering and relationships
3. **Department Operations Demo**: Working with hierarchical data
4. **Advanced LINQ Demo**: Aggregations and complex projections
5. **Many-to-Many Demo**: Managing project assignments

Each demo is thoroughly commented to explain what's happening and why.

## Database Tools

Since this project uses SQLite, you can examine the created database using tools like:
- **SQLite Browser**: Free, cross-platform SQLite database viewer
- **VS Code SQLite Extension**: View and query your database directly in VS Code
- **Command Line**: Use `sqlite3 CompanyDatabase.db` to interact with the database

## Real-World Applications

The patterns demonstrated here are used in production applications for:
- **HR Management Systems**: Employee and department tracking
- **Project Management**: Assigning staff to projects
- **Financial Systems**: Budget tracking and salary management
- **Reporting**: Generating statistical reports from business data

## Best Practices Demonstrated

- **Service Layer Pattern**: Separating business logic from data access
- **Async/Await**: Using asynchronous programming for database operations
- **Error Handling**: Proper exception handling and validation
- **Resource Management**: Using `using` statements for proper disposal
- **Business Rules**: Enforcing constraints through code rather than just database

## Next Steps

After working through this demo, you'll be ready to:
- Build your own EF Core applications
- Design effective database schemas
- Write efficient LINQ queries
- Handle complex business relationships
- Implement proper error handling and validation

Remember: The best way to learn EF Core is by building something! Try extending this project with additional entities or business rules to practice what you've learned.

## Common Troubleshooting

**Database locked errors**: Make sure only one instance of the application is running
**Missing data**: Check that the seeding process completed successfully
**Query performance**: Use `Include()` for eager loading when you need related data

## Advanced Topics & Tips

### Working with Migrations

This project uses EF Core Migrations for database schema management. Here are essential commands:

```powershell
# Create a new migration
dotnet ef migrations add MigrationName

# Apply pending migrations
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Remove last unapplied migration
dotnet ef migrations remove

# Generate SQL script for migrations
dotnet ef migrations script

# List all migrations
dotnet ef migrations list
```

### Migration Best Practices

1. **Always backup your database** before applying migrations in production
2. **Test migrations** on a copy of production data first
3. **Use meaningful migration names** that describe the change
4. **Review generated migration code** before applying
5. **Never edit applied migrations** - create new ones instead

### Performance Tips

```csharp
// Use projection to select only needed fields
var employeeNames = await context.Employees
    .Select(e => new { e.Id, e.Name })
    .ToListAsync();

// Use AsNoTracking() for read-only queries
var readOnlyEmployees = await context.Employees
    .AsNoTracking()
    .ToListAsync();

// Batch operations for better performance
context.Employees.AddRange(multipleEmployees);
await context.SaveChangesAsync();
```

### Connection String Configuration

For production applications, store connection strings in configuration:

**appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=CompanyDatabase.db"
  }
}
```

**Program.cs (with DI)**
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CompanyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### Testing Your EF Code

```csharp
// Example unit test setup
[Test]
public async Task CreateEmployee_WithValidData_ShouldSucceed()
{
    // Arrange
    var options = new DbContextOptionsBuilder<CompanyDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    using var context = new CompanyDbContext(options);
    var service = new EmployeeService(context);

    // Act & Assert
    var employee = new Employee 
    { 
        Name = "Test User", 
        Email = "test@example.com",
        DepartmentId = 1 
    };
    
    var result = await service.CreateEmployeeAsync(employee);
    Assert.IsNotNull(result);
    Assert.AreEqual("Test User", result.Name);
}
```

## Deployment Considerations

### Production Database Setup

1. **Use a proper database server** (SQL Server, PostgreSQL, MySQL) instead of SQLite
2. **Configure connection pooling** for better performance
3. **Enable logging** for monitoring and debugging
4. **Set up automated backups**
5. **Use environment-specific configurations**

### Security Best Practices

```csharp
// Never expose DbContext directly in APIs
// Always use service layer for data access

// Use parameterized queries (EF Core does this automatically)
var user = await context.Employees
    .FirstOrDefaultAsync(e => e.Email == userEmail); // Safe from SQL injection

// Implement proper authorization
// Validate input data
// Use HTTPS in production
```

## Extending This Project

Here are ideas to expand your learning:

### 1. Add More Entities
- **TimeSheet**: Track employee work hours
- **Salary**: Historical salary information
- **Office**: Multiple office locations
- **Benefits**: Employee benefits tracking

### 2. Implement Advanced Features
- **Audit Trail**: Track who changed what and when
- **Soft Delete**: Mark records as deleted instead of removing them
- **Multi-tenancy**: Support multiple companies in one database
- **Caching**: Implement caching for frequently accessed data

### 3. Add Web API
Convert this console app to a REST API:

```csharp
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    [HttpGet]
    public async Task<ActionResult<List<Employee>>> GetEmployees()
    {
        return await _employeeService.GetAllEmployeesAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
    {
        var created = await _employeeService.CreateEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetEmployees), new { id = created.Id }, created);
    }
}
```

### 4. Add Authentication & Authorization
```csharp
// Install Microsoft.AspNetCore.Authentication.JwtBearer
// Configure JWT authentication
// Add [Authorize] attributes to controllers
// Implement role-based access control
```

## Troubleshooting Guide

### Common Issues and Solutions

| Problem | Solution |
|---------|----------|
| `Database is locked` | Ensure no other applications are accessing the SQLite file |
| `Migration already applied` | Use `dotnet ef migrations remove` or create a new migration |
| `Table already exists` | Delete the database file and run migrations again |
| `Foreign key constraint failed` | Check that referenced entities exist before creating relationships |
| `Unique constraint violation` | Ensure unique fields (like email) don't have duplicates |

### Debug Tips

```csharp
// Enable detailed logging in development
optionsBuilder.EnableSensitiveDataLogging()
             .EnableDetailedErrors()
             .LogTo(Console.WriteLine, LogLevel.Information);

// Use debugger to inspect generated SQL
// Check the Migrations folder for schema changes
// Use database browser tools to verify data
```

## Resources for Further Learning

### Official Documentation
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [EF Core Relationships](https://docs.microsoft.com/en-us/ef/core/modeling/relationships)

### Recommended Books
- "Entity Framework Core in Action" by Jon P Smith
- "Programming Entity Framework Core" by Julia Lerman

### Video Tutorials
- Microsoft Learn EF Core modules
- YouTube: .NET Foundation EF Core videos

### Practice Projects
- Build a blog system with posts, comments, and users
- Create an e-commerce system with products, orders, and customers
- Develop a library management system with books, authors, and borrowers
