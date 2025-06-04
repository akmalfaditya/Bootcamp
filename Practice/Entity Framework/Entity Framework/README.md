# Entity Framework Core Demo Project

## Overview

Welcome to this comprehensive Entity Framework Core demonstration project! This hands-on example is designed to teach you the fundamentals of working with EF Core in .NET applications. Rather than just reading about concepts, you'll see them in action with real, working code.

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

This project is designed to be a comprehensive learning resource. Take your time exploring each concept, and don't hesitate to experiment with the code to deepen your understanding!
