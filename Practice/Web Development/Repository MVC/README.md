# Repository Pattern Student Management System

A comprehensive ASP.NET Core MVC application demonstrating the **Repository Design Pattern** with Entity Framework Core. This project serves as a complete educational resource for understanding professional software development practices including separation of concerns, dependency injection, and clean architecture principles.

## Table of Contents

- [Project Overview](#project-overview)
- [Learning Objectives](#learning-objectives)
- [Architecture Overview](#architecture-overview)
- [Prerequisites](#prerequisites)
- [Complete Build Tutorial](#complete-build-tutorial)
- [Project Structure](#project-structure)
- [Design Patterns Explained](#design-patterns-explained)
- [Database Schema](#database-schema)
- [Testing Guide](#testing-guide)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)
- [Advanced Features](#advanced-features)

## Project Overview

This project demonstrates the implementation of the **Repository Design Pattern** in an ASP.NET Core MVC application. The Repository pattern encapsulates data access logic, centralizes common database operations, and provides better maintainability while decoupling the infrastructure from the domain model layer.

### Key Benefits Demonstrated

- **Separation of Concerns**: Business logic clearly separated from data access logic
- **Testability**: Easy to mock repositories for comprehensive unit testing
- **Maintainability**: Centralized data access patterns reduce code duplication
- **Flexibility**: Can easily switch between different data storage technologies
- **Code Reusability**: Generic repository eliminates duplicate CRUD operations
- **Clean Architecture**: Well-defined layers with clear responsibilities

### Features Implemented

- **Student Management**: Complete CRUD operations for student records
- **Grade Management**: Manage student grades with automatic letter grade calculation
- **Search Functionality**: Advanced search capabilities across multiple fields
- **Data Validation**: Comprehensive server-side validation with user-friendly error messages
- **Responsive Design**: Modern Bootstrap-based responsive user interface
- **Database Migrations**: Automated database schema management and seeding

## Learning Objectives

Upon completion of this tutorial, you will understand:

1. **Repository Pattern Implementation**
   - How to create and implement generic repositories
   - Specific repository patterns for domain entities
   - Interface segregation and dependency inversion principles

2. **Clean Architecture Principles**
   - Layered architecture design and implementation
   - Separation of concerns across application layers
   - Dependency injection and inversion of control

3. **Entity Framework Core Integration**
   - Database context configuration and setup
   - Entity relationships and navigation properties
   - Migration management and database seeding

4. **ASP.NET Core MVC Best Practices**
   - Controller design and responsibility
   - Service layer implementation
   - View model patterns and data binding

## Architecture Overview

### Design Pattern Flow
```
HTTP Request → Controller → Service Layer → Repository Interface → Repository Implementation → Entity Framework Core → SQLite Database
```

### Layer Responsibilities

1. **Presentation Layer (Controllers/Views)**
   - Handle HTTP requests and responses
   - Coordinate user interactions
   - Manage view rendering and data binding

2. **Service Layer (Business Logic)**
   - Implement business rules and validation
   - Coordinate multiple repository operations
   - Handle transaction management

3. **Repository Layer (Data Access)**
   - Abstract data access operations
   - Implement specific query logic
   - Provide consistent data access patterns

4. **Domain Layer (Models)**
   - Define domain entities and relationships
   - Implement validation rules
   - Represent business concepts

5. **Infrastructure Layer (Data Context)**
   - Configure Entity Framework
   - Manage database connections
   - Handle migrations and seeding

## Prerequisites

### Required Software
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) with C# extension
- [Entity Framework Core CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

### Required Knowledge
- Basic understanding of C# programming language
- Familiarity with ASP.NET Core MVC pattern
- Basic knowledge of Entity Framework Core
- Understanding of dependency injection concepts

### Optional Tools
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/) or [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/) for database management
- [Postman](https://www.postman.com/) for API testing

## Complete Build Tutorial

This section provides a comprehensive, step-by-step guide to building the Repository Pattern Student Management System from scratch.

### Phase 1: Project Foundation Setup

#### Step 1: Create the Project Structure

```bash
# Create a new directory for the project
mkdir RepositoryMVC
cd RepositoryMVC

# Create a new ASP.NET Core MVC project
dotnet new mvc -n RepositoryMVC
cd RepositoryMVC

# Verify the project structure
dir # (Windows) or ls -la # (macOS/Linux)
```

#### Step 2: Install Required NuGet Packages

```bash
# Entity Framework Core with SQLite provider
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.0

# Entity Framework Tools for migrations
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

# Design-time services for EF Core
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# Additional validation attributes
dotnet add package System.ComponentModel.Annotations --version 5.0.0

# Verify packages were added
dotnet list package
```

#### Step 3: Create Project Directory Structure

```bash
# Create necessary directories
mkdir Models
mkdir Data
mkdir Repositories
mkdir Services
mkdir ViewModels
mkdir DTOs

# Create subdirectories for better organization
mkdir Repositories\Interfaces
mkdir Repositories\Implementations
mkdir Services\Interfaces
mkdir Services\Implementations
```

### Phase 2: Domain Models Implementation

#### Step 4: Create the Student Entity

Create `Models/Student.cs`:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryMVC.Models
{
    /// <summary>
    /// Student entity representing a student in the education system
    /// Includes validation attributes for data integrity
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Primary key for the Student entity
        /// </summary>
        [Key]
        public int StudentID { get; set; }

        /// <summary>
        /// Student's full name with validation rules
        /// </summary>
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender with predefined options
        /// </summary>
        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic branch or department
        /// </summary>
        [Required(ErrorMessage = "Branch is required")]
        [StringLength(50, ErrorMessage = "Branch cannot exceed 50 characters")]
        [Display(Name = "Academic Branch")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Class section identifier
        /// </summary>
        [Required(ErrorMessage = "Section is required")]
        [StringLength(10, ErrorMessage = "Section cannot exceed 10 characters")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Student's email address for communication
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional phone number for contact
        /// </summary>
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Date when the student enrolled in the institution
        /// </summary>
        [Required(ErrorMessage = "Enrollment date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Optional date of birth for age calculations
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Student's current address
        /// </summary>
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        /// <summary>
        /// Navigation property for the student's grades
        /// Represents the one-to-many relationship between Student and Grade
        /// </summary>
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

        /// <summary>
        /// Computed property to get the student's full display name
        /// </summary>
        [NotMapped]
        public string DisplayName => $"{Name} ({Email})";

        /// <summary>
        /// Computed property to calculate student's age if date of birth is provided
        /// </summary>
        [NotMapped]
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
```

#### Step 5: Create the Grade Entity

Create `Models/Grade.cs`:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryMVC.Models
{
    /// <summary>
    /// Grade entity representing a student's grade in a specific subject
    /// Includes automatic letter grade calculation and validation
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Primary key for the Grade entity
        /// </summary>
        [Key]
        public int GradeID { get; set; }

        /// <summary>
        /// Foreign key referencing the Student entity
        /// </summary>
        [Required(ErrorMessage = "Student ID is required")]
        [Display(Name = "Student")]
        public int StudentID { get; set; }

        /// <summary>
        /// Subject name for which the grade is assigned
        /// </summary>
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Numerical grade value (0-100)
        /// </summary>
        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Grade Value")]
        public decimal GradeValue { get; set; }

        /// <summary>
        /// Letter grade computed from numerical value
        /// </summary>
        [StringLength(2, ErrorMessage = "Letter grade cannot exceed 2 characters")]
        [Display(Name = "Letter Grade")]
        public string? LetterGrade { get; set; }

        /// <summary>
        /// Date when the grade was assigned
        /// </summary>
        [Required(ErrorMessage = "Grade date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Grade Date")]
        public DateTime GradeDate { get; set; }

        /// <summary>
        /// Optional comments about the grade
        /// </summary>
        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string? Comments { get; set; }

        /// <summary>
        /// Academic semester or term
        /// </summary>
        [StringLength(20, ErrorMessage = "Semester cannot exceed 20 characters")]
        public string? Semester { get; set; }

        /// <summary>
        /// Academic year for the grade
        /// </summary>
        [Range(2000, 3000, ErrorMessage = "Academic year must be between 2000 and 3000")]
        [Display(Name = "Academic Year")]
        public int? AcademicYear { get; set; }

        /// <summary>
        /// Navigation property to the associated Student
        /// </summary>
        [ForeignKey("StudentID")]
        public virtual Student? Student { get; set; }

        /// <summary>
        /// Calculated property to determine if the grade is passing
        /// </summary>
        [NotMapped]
        public bool IsPassing => GradeValue >= 60;

        /// <summary>
        /// Method to calculate letter grade based on numerical value
        /// </summary>
        /// <returns>Letter grade representation</returns>
        public string CalculateLetterGrade()
        {
            return GradeValue switch
            {
                >= 97 => "A+",
                >= 93 => "A",
                >= 90 => "A-",
                >= 87 => "B+",
                >= 83 => "B",
                >= 80 => "B-",
                >= 77 => "C+",
                >= 73 => "C",
                >= 70 => "C-",
                >= 67 => "D+",
                >= 65 => "D",
                >= 60 => "D-",
                _ => "F"
            };
        }

        /// <summary>
        /// Method to get grade point value for GPA calculation
        /// </summary>
        /// <returns>Grade point value (0.0 - 4.0)</returns>
        public decimal GetGradePoint()
        {
            return GradeValue switch
            {
                >= 97 => 4.0m,
                >= 93 => 4.0m,
                >= 90 => 3.7m,
                >= 87 => 3.3m,
                >= 83 => 3.0m,
                >= 80 => 2.7m,
                >= 77 => 2.3m,
                >= 73 => 2.0m,
                >= 70 => 1.7m,
                >= 67 => 1.3m,
                >= 65 => 1.0m,
                >= 60 => 0.7m,
                _ => 0.0m
            };
        }
    }
}
```

### Phase 3: Database Context and Configuration

#### Step 6: Create the Application Database Context

Create `Data/ApplicationDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Models;

namespace RepositoryMVC.Data
{
    /// <summary>
    /// Application database context for Entity Framework Core
    /// Configures database tables, relationships, and constraints
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor that receives database context options
        /// </summary>
        /// <param name="options">Database context configuration options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Students table representation
        /// </summary>
        public DbSet<Student> Students { get; set; } = null!;

        /// <summary>
        /// Grades table representation
        /// </summary>
        public DbSet<Grade> Grades { get; set; } = null!;

        /// <summary>
        /// Configures entity relationships and constraints using Fluent API
        /// </summary>
        /// <param name="modelBuilder">Model builder for configuration</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            ConfigureStudentEntity(modelBuilder);

            // Configure Grade entity
            ConfigureGradeEntity(modelBuilder);

            // Configure relationships
            ConfigureRelationships(modelBuilder);

            // Add seed data
            SeedData(modelBuilder);
        }

        /// <summary>
        /// Configures the Student entity properties and constraints
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        private static void ConfigureStudentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                // Primary key
                entity.HasKey(s => s.StudentID);

                // Property configurations
                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Gender)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(s => s.Branch)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(s => s.Section)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(s => s.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(s => s.Address)
                    .HasMaxLength(500);

                entity.Property(s => s.EnrollmentDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                // Indexes for performance
                entity.HasIndex(s => s.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Student_Email");

                entity.HasIndex(s => s.Branch)
                    .HasDatabaseName("IX_Student_Branch");

                entity.HasIndex(s => s.Section)
                    .HasDatabaseName("IX_Student_Section");

                entity.HasIndex(s => s.EnrollmentDate)
                    .HasDatabaseName("IX_Student_EnrollmentDate");

                // Table name
                entity.ToTable("Students");
            });
        }

        /// <summary>
        /// Configures the Grade entity properties and constraints
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        private static void ConfigureGradeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>(entity =>
            {
                // Primary key
                entity.HasKey(g => g.GradeID);

                // Property configurations
                entity.Property(g => g.Subject)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(g => g.GradeValue)
                    .IsRequired()
                    .HasColumnType("decimal(5,2)");

                entity.Property(g => g.LetterGrade)
                    .HasMaxLength(5);

                entity.Property(g => g.GradeDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(g => g.Semester)
                    .HasMaxLength(20);

                entity.Property(g => g.AcademicYear)
                    .IsRequired();

                entity.Property(g => g.Comments)
                    .HasMaxLength(500);

                // Indexes for performance
                entity.HasIndex(g => g.StudentID)
                    .HasDatabaseName("IX_Grade_StudentID");

                entity.HasIndex(g => g.Subject)
                    .HasDatabaseName("IX_Grade_Subject");

                entity.HasIndex(g => g.GradeDate)
                    .HasDatabaseName("IX_Grade_GradeDate");

                entity.HasIndex(g => new { g.StudentID, g.Subject, g.GradeDate })
                    .IsUnique()
                    .HasDatabaseName("IX_Grade_StudentSubjectDate");

                // Table name
                entity.ToTable("Grades");
            });
        }

        /// <summary>
        /// Configures entity relationships using Fluent API
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        private static void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Student-Grade relationship (One-to-Many)
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentID)
                .OnDelete(DeleteBehavior.Cascade) // Cascade delete grades when student is deleted
                .HasConstraintName("FK_Grade_Student");
        }

        /// <summary>
        /// Seeds initial data for testing and demonstration purposes
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentID = 1,
                    Name = "John Doe",
                    Gender = "Male",
                    Branch = "Computer Science",
                    Section = "A",
                    Email = "john.doe@university.edu",
                    PhoneNumber = "+1-555-0101",
                    EnrollmentDate = new DateTime(2023, 9, 1),
                    DateOfBirth = new DateTime(2000, 5, 15),
                    Address = "123 Main Street, University City"
                },
                new Student
                {
                    StudentID = 2,
                    Name = "Jane Smith",
                    Gender = "Female",
                    Branch = "Computer Science",
                    Section = "A",
                    Email = "jane.smith@university.edu",
                    PhoneNumber = "+1-555-0102",
                    EnrollmentDate = new DateTime(2023, 9, 1),
                    DateOfBirth = new DateTime(2001, 3, 22),
                    Address = "456 Oak Avenue, University City"
                },
                new Student
                {
                    StudentID = 3,
                    Name = "Mike Johnson",
                    Gender = "Male",
                    Branch = "Information Technology",
                    Section = "B",
                    Email = "mike.johnson@university.edu",
                    PhoneNumber = "+1-555-0103",
                    EnrollmentDate = new DateTime(2023, 9, 1),
                    DateOfBirth = new DateTime(2000, 8, 10),
                    Address = "789 Pine Road, University City"
                },
                new Student
                {
                    StudentID = 4,
                    Name = "Sarah Wilson",
                    Gender = "Female",
                    Branch = "Information Technology",
                    Section = "B",
                    Email = "sarah.wilson@university.edu",
                    PhoneNumber = "+1-555-0104",
                    EnrollmentDate = new DateTime(2023, 9, 1),
                    DateOfBirth = new DateTime(2001, 1, 5),
                    Address = "321 Elm Street, University City"
                },
                new Student
                {
                    StudentID = 5,
                    Name = "David Brown",
                    Gender = "Male",
                    Branch = "Computer Science",
                    Section = "C",
                    Email = "david.brown@university.edu",
                    PhoneNumber = "+1-555-0105",
                    EnrollmentDate = new DateTime(2023, 9, 1),
                    DateOfBirth = new DateTime(2000, 11, 18),
                    Address = "654 Maple Lane, University City"
                }
            );

            // Seed Grades
            modelBuilder.Entity<Grade>().HasData(
                // John Doe's grades
                new Grade
                {
                    GradeID = 1,
                    StudentID = 1,
                    Subject = "Programming Fundamentals",
                    GradeValue = 85.5m,
                    LetterGrade = "B",
                    GradeDate = new DateTime(2024, 1, 15),
                    Semester = "Fall 2023",
                    AcademicYear = 2023,
                    Comments = "Good understanding of basic concepts"
                },
                new Grade
                {
                    GradeID = 2,
                    StudentID = 1,
                    Subject = "Data Structures",
                    GradeValue = 92.0m,
                    LetterGrade = "A",
                    GradeDate = new DateTime(2024, 1, 20),
                    Semester = "Fall 2023",
                    AcademicYear = 2023,
                    Comments = "Excellent performance"
                },
                // Jane Smith's grades
                new Grade
                {
                    GradeID = 3,
                    StudentID = 2,
                    Subject = "Programming Fundamentals",
                    GradeValue = 88.0m,
                    LetterGrade = "B+",
                    GradeDate = new DateTime(2024, 1, 15),
                    Semester = "Fall 2023",
                    AcademicYear = 2023,
                    Comments = "Strong programming skills"
                },
                new Grade
                {
                    GradeID = 4,
                    StudentID = 2,
                    Subject = "Database Systems",
                    GradeValue = 95.5m,
                    LetterGrade = "A",
                    GradeDate = new DateTime(2024, 1, 25),
                    Semester = "Fall 2023",
                    AcademicYear = 2023,
                    Comments = "Outstanding database design skills"
                },
                // Mike Johnson's grades
                new Grade
                {
                    GradeID = 5,
                    StudentID = 3,
                    Subject = "Network Administration",
                    GradeValue = 78.5m,
                    LetterGrade = "C+",
                    GradeDate = new DateTime(2024, 1, 18),
                    Semester = "Fall 2023",
                    AcademicYear = 2023,
                    Comments = "Needs improvement in practical applications"
                },
                new Grade
                {
                    GradeID = 6,
                    StudentID = 3,
                    Subject = "System Analysis",
                    GradeValue = 82.0m,
                    LetterGrade = "B-",
                    GradeDate = new DateTime(2024, 1, 22),
                    Semester = "Fall 2023",
                    AcademicYear = 2023,
                    Comments = "Good analytical thinking"
                }
            );
        }
    }
}
```

### Phase 4: Repository Pattern Implementation

#### Step 7: Create Repository Interfaces

Create `Repositories/Interfaces/IGenericRepository.cs`:

```csharp
using System.Linq.Expressions;

namespace RepositoryMVC.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface defining common CRUD operations
    /// This interface can be used for any entity type to ensure consistency
    /// </summary>
    /// <typeparam name="T">The entity type that implements this repository</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities from the database asynchronously
        /// </summary>
        /// <returns>Collection of all entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves a single entity by its primary key asynchronously
        /// </summary>
        /// <param name="id">The primary key value</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Finds entities that match the specified predicate
        /// </summary>
        /// <param name="predicate">The condition to match</param>
        /// <returns>Collection of matching entities</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity to the database asynchronously
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds multiple entities to the database asynchronously
        /// </summary>
        /// <param name="entities">The entities to add</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task AddRangeAsync(IEnumerable<T> entities);

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
        /// Removes multiple entities from the database
        /// </summary>
        /// <param name="entities">The entities to remove</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Checks if any entity matches the specified predicate
        /// </summary>
        /// <param name="predicate">The condition to check</param>
        /// <returns>True if any entity matches, false otherwise</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Counts entities that match the specified predicate
        /// </summary>
        /// <param name="predicate">Optional condition to count specific entities</param>
        /// <returns>Number of matching entities</returns>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        /// <summary>
        /// Retrieves entities with paging support
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paged collection of entities</returns>
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves entities with paging and ordering support
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="orderBy">Ordering expression</param>
        /// <param name="ascending">True for ascending order, false for descending</param>
        /// <returns>Paged and ordered collection of entities</returns>
        Task<IEnumerable<T>> GetPagedAsync<TKey>(int pageNumber, int pageSize, 
            Expression<Func<T, TKey>> orderBy, bool ascending = true);
    }
}
```

Create `Repositories/Interfaces/IStudentRepository.cs`:

```csharp
using RepositoryMVC.Models;

namespace RepositoryMVC.Repositories.Interfaces
{
    /// <summary>
    /// Student-specific repository interface that extends the generic repository
    /// with student-specific operations and queries
    /// </summary>
    public interface IStudentRepository : IGenericRepository<Student>
    {
        /// <summary>
        /// Retrieves all students with their grades included (eager loading)
        /// </summary>
        /// <returns>Students with their grades loaded</returns>
        Task<IEnumerable<Student>> GetAllStudentsWithGradesAsync();

        /// <summary>
        /// Retrieves a specific student with all their grades included
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>Student with grades loaded, or null if not found</returns>
        Task<Student?> GetStudentWithGradesAsync(int studentId);

        /// <summary>
        /// Searches for students by name using partial matching
        /// </summary>
        /// <param name="name">The name or partial name to search for</param>
        /// <returns>Students whose names contain the search term</returns>
        Task<IEnumerable<Student>> FindStudentsByNameAsync(string name);

        /// <summary>
        /// Retrieves all students in a specific academic branch
        /// </summary>
        /// <param name="branch">The academic branch to filter by</param>
        /// <returns>Students in the specified branch</returns>
        Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch);

        /// <summary>
        /// Retrieves all students in a specific section
        /// </summary>
        /// <param name="section">The section to filter by</param>
        /// <returns>Students in the specified section</returns>
        Task<IEnumerable<Student>> GetStudentsBySectionAsync(string section);

        /// <summary>
        /// Performs a comprehensive search across multiple student fields
        /// </summary>
        /// <param name="searchTerm">The term to search for in name, email, branch, or section</param>
        /// <returns>Students matching the search criteria</returns>
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);

        /// <summary>
        /// Checks if a student email already exists in the database
        /// </summary>
        /// <param name="email">The email address to check</param>
        /// <param name="excludeStudentId">Optional student ID to exclude from the check (for updates)</param>
        /// <returns>True if email exists, false otherwise</returns>
        Task<bool> IsEmailExistsAsync(string email, int? excludeStudentId = null);

        /// <summary>
        /// Retrieves students enrolled in a specific date range
        /// </summary>
        /// <param name="startDate">Start date of enrollment range</param>
        /// <param name="endDate">End date of enrollment range</param>
        /// <returns>Students enrolled within the specified date range</returns>
        Task<IEnumerable<Student>> GetStudentsByEnrollmentDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets students with their average grade calculated
        /// </summary>
        /// <returns>Students with calculated average grades</returns>
        Task<IEnumerable<Student>> GetStudentsWithAverageGradeAsync();

        /// <summary>
        /// Retrieves students who have grades in a specific subject
        /// </summary>
        /// <param name="subject">The subject to filter by</param>
        /// <returns>Students who have grades in the specified subject</returns>
        Task<IEnumerable<Student>> GetStudentsBySubjectAsync(string subject);
    }
}
```

Create `Repositories/Interfaces/IGradeRepository.cs`:

```csharp
using RepositoryMVC.Models;

namespace RepositoryMVC.Repositories.Interfaces
{
    /// <summary>
    /// Grade-specific repository interface that extends the generic repository
    /// with grade-specific operations and queries
    /// </summary>
    public interface IGradeRepository : IGenericRepository<Grade>
    {
        /// <summary>
        /// Retrieves all grades for a specific student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>All grades belonging to the specified student</returns>
        Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId);

        /// <summary>
        /// Retrieves grades for a specific student in a particular subject
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <param name="subject">The subject to filter by</param>
        /// <returns>Grades for the student in the specified subject</returns>
        Task<IEnumerable<Grade>> GetGradesByStudentAndSubjectAsync(int studentId, string subject);

        /// <summary>
        /// Retrieves all grades for a specific subject across all students
        /// </summary>
        /// <param name="subject">The subject to filter by</param>
        /// <returns>All grades in the specified subject</returns>
        Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject);

        /// <summary>
        /// Checks if a grade already exists for a student in a specific subject and date
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <param name="subject">The subject name</param>
        /// <param name="gradeDate">The grade date</param>
        /// <returns>True if a grade already exists, false otherwise</returns>
        Task<bool> IsGradeExistsAsync(int studentId, string subject, DateTime gradeDate);

        /// <summary>
        /// Calculates the average grade for a specific student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>The average grade value, or null if no grades exist</returns>
        Task<decimal?> GetAverageGradeForStudentAsync(int studentId);

        /// <summary>
        /// Calculates the average grade for a specific subject across all students
        /// </summary>
        /// <param name="subject">The subject name</param>
        /// <returns>The average grade value for the subject</returns>
        Task<decimal?> GetAverageGradeForSubjectAsync(string subject);

        /// <summary>
        /// Retrieves grades within a specific date range
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>Grades assigned within the specified date range</returns>
        Task<IEnumerable<Grade>> GetGradesByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retrieves grades for a specific semester and academic year
        /// </summary>
        /// <param name="semester">The semester identifier</param>
        /// <param name="academicYear">The academic year</param>
        /// <returns>Grades for the specified semester and year</returns>
        Task<IEnumerable<Grade>> GetGradesBySemesterAsync(string semester, int academicYear);

        /// <summary>
        /// Retrieves the highest grade achieved by a student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>The highest grade for the student, or null if no grades exist</returns>
        Task<Grade?> GetHighestGradeForStudentAsync(int studentId);

        /// <summary>
        /// Retrieves the lowest grade achieved by a student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>The lowest grade for the student, or null if no grades exist</returns>
        Task<Grade?> GetLowestGradeForStudentAsync(int studentId);
    }
}
```

#### Step 8: Create Repository Implementation Classes

Create `Repositories/Implementations/GenericRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Repositories.Interfaces;
using System.Linq.Expressions;

namespace RepositoryMVC.Repositories.Implementations
{
    /// <summary>
    /// Generic repository implementation providing common CRUD operations
    /// This class serves as the base implementation for all entity repositories
    /// </summary>
    /// <typeparam name="T">The entity type that this repository manages</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor that receives the database context
        /// </summary>
        /// <param name="context">The application database context</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Retrieves all entities from the database asynchronously
        /// </summary>
        /// <returns>Collection of all entities</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving all entities of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Retrieves a single entity by its primary key asynchronously
        /// </summary>
        /// <param name="id">The primary key value</param>
        /// <returns>The entity if found, null otherwise</returns>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving entity of type {typeof(T).Name} with ID {id}", ex);
            }
        }

        /// <summary>
        /// Finds entities that match the specified predicate
        /// </summary>
        /// <param name="predicate">The condition to match</param>
        /// <returns>Collection of matching entities</returns>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error finding entities of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Adds a new entity to the database asynchronously
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public virtual async Task AddAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error adding entity of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Adds multiple entities to the database asynchronously
        /// </summary>
        /// <param name="entities">The entities to add</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                await _dbSet.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error adding multiple entities of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Updates an existing entity in the database
        /// </summary>
        /// <param name="entity">The entity to update</param>
        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating entity of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Removes an entity from the database
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        public virtual void Remove(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error removing entity of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Removes multiple entities from the database
        /// </summary>
        /// <param name="entities">The entities to remove</param>
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                _dbSet.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error removing multiple entities of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Checks if any entity matches the specified predicate
        /// </summary>
        /// <param name="predicate">The condition to check</param>
        /// <returns>True if any entity matches, false otherwise</returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking existence of entities of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Counts entities that match the specified predicate
        /// </summary>
        /// <param name="predicate">Optional condition to count specific entities</param>
        /// <returns>Number of matching entities</returns>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    return await _dbSet.CountAsync();
                
                return await _dbSet.CountAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error counting entities of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Retrieves entities with paging support
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paged collection of entities</returns>
        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1)
                    throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
                
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

                return await _dbSet
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving paged entities of type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Retrieves entities with paging and ordering support
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="orderBy">Ordering expression</param>
        /// <param name="ascending">True for ascending order, false for descending</param>
        /// <returns>Paged and ordered collection of entities</returns>
        public virtual async Task<IEnumerable<T>> GetPagedAsync<TKey>(int pageNumber, int pageSize, 
            Expression<Func<T, TKey>> orderBy, bool ascending = true)
        {
            try
            {
                if (pageNumber < 1)
                    throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
                
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

                if (orderBy == null)
                    throw new ArgumentNullException(nameof(orderBy));

                var query = ascending 
                    ? _dbSet.OrderBy(orderBy)
                    : _dbSet.OrderByDescending(orderBy);

                return await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving paged and ordered entities of type {typeof(T).Name}", ex);
            }
        }
    }
}
```

Create `Repositories/Implementations/StudentRepository.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Models;
using RepositoryMVC.Repositories.Interfaces;

namespace RepositoryMVC.Repositories.Implementations
{
    /// <summary>
    /// Student repository implementation providing student-specific data access operations
    /// Inherits from GenericRepository and implements IStudentRepository
    /// </summary>
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        /// <summary>
        /// Constructor that passes the database context to the base class
        /// </summary>
        /// <param name="context">The application database context</param>
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves all students with their grades included (eager loading)
        /// </summary>
        /// <returns>Students with their grades loaded</returns>
        public async Task<IEnumerable<Student>> GetAllStudentsWithGradesAsync()
        {
            try
            {
                return await _dbSet
                    .Include(s => s.Grades)
                    .OrderBy(s => s.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving students with grades", ex);
            }
        }

        /// <summary>
        /// Retrieves a specific student with all their grades included
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>Student with grades loaded, or null if not found</returns>
        public async Task<Student?> GetStudentWithGradesAsync(int studentId)
        {
            try
            {
                return await _dbSet
                    .Include(s => s.Grades.OrderByDescending(g => g.GradeDate))
                    .FirstOrDefaultAsync(s => s.StudentID == studentId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving student with ID {studentId} and their grades", ex);
            }
        }

        /// <summary>
        /// Searches for students by name using partial matching
        /// </summary>
        /// <param name="name">The name or partial name to search for</param>
        /// <returns>Students whose names contain the search term</returns>
        public async Task<IEnumerable<Student>> FindStudentsByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return await GetAllAsync();

                return await _dbSet
                    .Where(s => s.Name.ToLower().Contains(name.ToLower()))
                    .OrderBy(s => s.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error searching students by name '{name}'", ex);
            }
        }

        /// <summary>
        /// Retrieves all students in a specific academic branch
        /// </summary>
        /// <param name="branch">The academic branch to filter by</param>
        /// <returns>Students in the specified branch</returns>
        public async Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branch))
                    return await GetAllAsync();

                return await _dbSet
                    .Where(s => s.Branch.ToLower() == branch.ToLower())
                    .OrderBy(s => s.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving students by branch '{branch}'", ex);
            }
        }

        /// <summary>
        /// Retrieves all students in a specific section
        /// </summary>
        /// <param name="section">The section to filter by</param>
        /// <returns>Students in the specified section</returns>
        public async Task<IEnumerable<Student>> GetStudentsBySectionAsync(string section)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(section))
                    return await GetAllAsync();

                return await _dbSet
                    .Where(s => s.Section.ToLower() == section.ToLower())
                    .OrderBy(s => s.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving students by section '{section}'", ex);
            }
        }

        /// <summary>
        /// Performs a comprehensive search across multiple student fields
        /// </summary>
        /// <param name="searchTerm">The term to search for in name, email, branch, or section</param>
        /// <returns>Students matching the search criteria</returns>
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllStudentsWithGradesAsync();

                var lowerSearchTerm = searchTerm.ToLower();

                return await _dbSet
                    .Include(s => s.Grades)
                    .Where(s => s.Name.ToLower().Contains(lowerSearchTerm) ||
                               s.Email.ToLower().Contains(lowerSearchTerm) ||
                               s.Branch.ToLower().Contains(lowerSearchTerm) ||
                               s.Section.ToLower().Contains(lowerSearchTerm))
                    .OrderBy(s => s.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error searching students with term '{searchTerm}'", ex);
            }
        }

        /// <summary>
        /// Checks if a student email already exists in the database
        /// </summary>
        /// <param name="email">The email address to check</param>
        /// <param name="excludeStudentId">Optional student ID to exclude from the check (for updates)</param>
        /// <returns>True if email exists, false otherwise</returns>
        public async Task<bool> IsEmailExistsAsync(string email, int? excludeStudentId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                var query = _dbSet.Where(s => s.Email.ToLower() == email.ToLower());
                
                if (excludeStudentId.HasValue)
                    query = query.Where(s => s.StudentID != excludeStudentId.Value);

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking if email '{email}' exists", ex);
            }
        }

        /// <summary>
        /// Retrieves students enrolled in a specific date range
        /// </summary>
        /// <param name="startDate">Start date of enrollment range</param>
        /// <param name="endDate">End date of enrollment range</param>
        /// <returns>Students enrolled within the specified date range</returns>
        public async Task<IEnumerable<Student>> GetStudentsByEnrollmentDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    throw new ArgumentException("Start date cannot be greater than end date");

                return await _dbSet
                    .Where(s => s.EnrollmentDate >= startDate && s.EnrollmentDate <= endDate)
                    .OrderBy(s => s.EnrollmentDate)
                    .ThenBy(s => s.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving students by enrollment date range {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}", ex);
            }
        }

        /// <summary>
        /// Gets students with their average grade calculated
        /// </summary>
        /// <returns>Students with calculated average grades</returns>
        public async Task<IEnumerable<Student>> GetStudentsWithAverageGradeAsync()
        {
            try
            {
                return await _dbSet
                    .Include(s => s.Grades)
                    .Select(s => new Student
                    {
                        StudentID = s.StudentID,
                        Name = s.Name,
                        Gender = s.Gender,
                        Branch = s.Branch,
                        Section = s.Section,
                        Email = s.Email,
                        PhoneNumber = s.PhoneNumber,
                        EnrollmentDate = s.EnrollmentDate,
                        DateOfBirth = s.DateOfBirth,
                        Address = s.Address,
                        Grades = s.Grades
                    })
                    .OrderBy(s => s.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving students with average grades", ex);
            }
        }

        /// <summary>
        /// Retrieves students who have grades in a specific subject
        /// </summary>
        /// <param name="subject">The subject to filter by</param>
        /// <returns>Students who have grades in the specified subject</returns>
        public async Task<IEnumerable<Student>> GetStudentsBySubjectAsync(string subject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject))
                    return new List<Student>();

                return await _dbSet
                    .Include(s => s.Grades)
                    .Where(s => s.Grades.Any(g => g.Subject.ToLower() == subject.ToLower()))
                    .OrderBy(s => s.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving students by subject '{subject}'", ex);
            }
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

namespace RepositoryMVC.Repositories.Implementations
{
    /// <summary>
    /// Grade repository implementation providing grade-specific data access operations
    /// Inherits from GenericRepository and implements IGradeRepository
    /// </summary>
    public class GradeRepository : GenericRepository<Grade>, IGradeRepository
    {
        /// <summary>
        /// Constructor that passes the database context to the base class
        /// </summary>
        /// <param name="context">The application database context</param>
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves all grades for a specific student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>All grades belonging to the specified student</returns>
        public async Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            try
            {
                return await _dbSet
                    .Include(g => g.Student)
                    .Where(g => g.StudentID == studentId)
                    .OrderByDescending(g => g.GradeDate)
                    .ThenBy(g => g.Subject)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving grades for student ID {studentId}", ex);
            }
        }

        /// <summary>
        /// Retrieves grades for a specific student in a particular subject
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <param name="subject">The subject to filter by</param>
        /// <returns>Grades for the student in the specified subject</returns>
        public async Task<IEnumerable<Grade>> GetGradesByStudentAndSubjectAsync(int studentId, string subject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject))
                    return await GetGradesByStudentIdAsync(studentId);

                return await _dbSet
                    .Include(g => g.Student)
                    .Where(g => g.StudentID == studentId && 
                               g.Subject.ToLower() == subject.ToLower())
                    .OrderByDescending(g => g.GradeDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving grades for student ID {studentId} in subject '{subject}'", ex);
            }
        }

        /// <summary>
        /// Retrieves all grades for a specific subject across all students
        /// </summary>
        /// <param name="subject">The subject to filter by</param>
        /// <returns>All grades in the specified subject</returns>
        public async Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject))
                    return new List<Grade>();

                return await _dbSet
                    .Include(g => g.Student)
                    .Where(g => g.Subject.ToLower() == subject.ToLower())
                    .OrderByDescending(g => g.GradeValue)
                    .ThenBy(g => g.Student!.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving grades for subject '{subject}'", ex);
            }
        }

        /// <summary>
        /// Checks if a grade already exists for a student in a specific subject and date
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <param name="subject">The subject name</param>
        /// <param name="gradeDate">The grade date</param>
        /// <returns>True if a grade already exists, false otherwise</returns>
        public async Task<bool> IsGradeExistsAsync(int studentId, string subject, DateTime gradeDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject))
                    return false;

                return await _dbSet.AnyAsync(g => 
                    g.StudentID == studentId && 
                    g.Subject.ToLower() == subject.ToLower() && 
                    g.GradeDate.Date == gradeDate.Date);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking if grade exists for student ID {studentId}, subject '{subject}', date {gradeDate:yyyy-MM-dd}", ex);
            }
        }

        /// <summary>
        /// Calculates the average grade for a specific student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>The average grade value, or null if no grades exist</returns>
        public async Task<decimal?> GetAverageGradeForStudentAsync(int studentId)
        {
            try
            {
                var grades = await _dbSet
                    .Where(g => g.StudentID == studentId)
                    .ToListAsync();

                if (!grades.Any())
                    return null;

                return grades.Average(g => g.GradeValue);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating average grade for student ID {studentId}", ex);
            }
        }

        /// <summary>
        /// Calculates the average grade for a specific subject across all students
        /// </summary>
        /// <param name="subject">The subject name</param>
        /// <returns>The average grade value for the subject</returns>
        public async Task<decimal?> GetAverageGradeForSubjectAsync(string subject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject))
                    return null;

                var grades = await _dbSet
                    .Where(g => g.Subject.ToLower() == subject.ToLower())
                    .ToListAsync();

                if (!grades.Any())
                    return null;

                return grades.Average(g => g.GradeValue);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating average grade for subject '{subject}'", ex);
            }
        }

        /// <summary>
        /// Retrieves grades within a specific date range
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>Grades assigned within the specified date range</returns>
        public async Task<IEnumerable<Grade>> GetGradesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    throw new ArgumentException("Start date cannot be greater than end date");

                return await _dbSet
                    .Include(g => g.Student)
                    .Where(g => g.GradeDate >= startDate && g.GradeDate <= endDate)
                    .OrderByDescending(g => g.GradeDate)
                    .ThenBy(g => g.Student!.Name)
                    .ThenBy(g => g.Subject)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving grades by date range {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}", ex);
            }
        }

        /// <summary>
        /// Retrieves grades for a specific semester and academic year
        /// </summary>
        /// <param name="semester">The semester identifier</param>
        /// <param name="academicYear">The academic year</param>
        /// <returns>Grades for the specified semester and year</returns>
        public async Task<IEnumerable<Grade>> GetGradesBySemesterAsync(string semester, int academicYear)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(semester))
                    return new List<Grade>();

                return await _dbSet
                    .Include(g => g.Student)
                    .Where(g => g.Semester != null && 
                               g.Semester.ToLower() == semester.ToLower() && 
                               g.AcademicYear == academicYear)
                    .OrderBy(g => g.Student!.Name)
                    .ThenBy(g => g.Subject)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving grades for semester '{semester}' and academic year {academicYear}", ex);
            }
        }

        /// <summary>
        /// Retrieves the highest grade achieved by a student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>The highest grade for the student, or null if no grades exist</returns>
        public async Task<Grade?> GetHighestGradeForStudentAsync(int studentId)
        {
            try
            {
                return await _dbSet
                    .Include(g => g.Student)
                    .Where(g => g.StudentID == studentId)
                    .OrderByDescending(g => g.GradeValue)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving highest grade for student ID {studentId}", ex);
            }
        }

        /// <summary>
        /// Retrieves the lowest grade achieved by a student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>The lowest grade for the student, or null if no grades exist</returns>
        public async Task<Grade?> GetLowestGradeForStudentAsync(int studentId)
        {
            try
            {
                return await _dbSet
                    .Include(g => g.Student)
                    .Where(g => g.StudentID == studentId)
                    .OrderBy(g => g.GradeValue)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving lowest grade for student ID {studentId}", ex);
            }
        }
    }
}
```

### Phase 5: Service Layer Implementation

#### Step 9: Create Service Layer Interfaces

Create `Services/Interfaces/IStudentService.cs`:

```csharp
using RepositoryMVC.Models;

namespace RepositoryMVC.Services.Interfaces
{
    /// <summary>
    /// Student service interface defining business logic operations for student management
    /// This service coordinates between controllers and repositories while implementing business rules
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Retrieves all students with their associated grades
        /// </summary>
        /// <returns>Collection of students with grades</returns>
        Task<IEnumerable<Student>> GetAllStudentsAsync();

        /// <summary>
        /// Retrieves a specific student by their unique identifier
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>Student details with grades, or null if not found</returns>
        Task<Student?> GetStudentByIdAsync(int studentId);

        /// <summary>
        /// Creates a new student record with validation
        /// </summary>
        /// <param name="student">The student information to create</param>
        /// <returns>The created student with assigned ID</returns>
        Task<Student> CreateStudentAsync(Student student);

        /// <summary>
        /// Updates an existing student record with validation
        /// </summary>
        /// <param name="student">The updated student information</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateStudentAsync(Student student);

        /// <summary>
        /// Deletes a student and all associated grades
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>True if deletion was successful, false if student not found</returns>
        Task<bool> DeleteStudentAsync(int studentId);

        /// <summary>
        /// Searches for students using multiple criteria
        /// </summary>
        /// <param name="searchTerm">Search term to match against name, email, branch, or section</param>
        /// <returns>Students matching the search criteria</returns>
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);

        /// <summary>
        /// Validates student data before create or update operations
        /// </summary>
        /// <param name="student">The student data to validate</param>
        /// <param name="isUpdate">True if this is an update operation, false for create</param>
        /// <returns>List of validation error messages, empty if valid</returns>
        Task<List<string>> ValidateStudentAsync(Student student, bool isUpdate = false);

        /// <summary>
        /// Retrieves students by academic branch
        /// </summary>
        /// <param name="branch">The academic branch to filter by</param>
        /// <returns>Students in the specified branch</returns>
        Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch);

        /// <summary>
        /// Retrieves students by section
        /// </summary>
        /// <param name="section">The section to filter by</param>
        /// <returns>Students in the specified section</returns>
        Task<IEnumerable<Student>> GetStudentsBySectionAsync(string section);

        /// <summary>
        /// Checks if a student email is available for use
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <param name="excludeStudentId">Student ID to exclude from check (for updates)</param>
        /// <returns>True if email is available, false if already in use</returns>
        Task<bool> IsEmailAvailableAsync(string email, int? excludeStudentId = null);

        /// <summary>
        /// Calculates statistics for a student (average grade, total grades, etc.)
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>Student statistics object</returns>
        Task<StudentStatistics?> GetStudentStatisticsAsync(int studentId);
    }

    /// <summary>
    /// Data transfer object for student statistics
    /// </summary>
    public class StudentStatistics
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int TotalGrades { get; set; }
        public decimal? AverageGrade { get; set; }
        public decimal? HighestGrade { get; set; }
        public decimal? LowestGrade { get; set; }
        public string? HighestGradeSubject { get; set; }
        public string? LowestGradeSubject { get; set; }
        public int PassingGrades { get; set; }
        public int FailingGrades { get; set; }
        public double PassingPercentage { get; set; }
    }
}
```

Create `Services/Interfaces/IGradeService.cs`:

```csharp
using RepositoryMVC.Models;

namespace RepositoryMVC.Services.Interfaces
{
    /// <summary>
    /// Grade service interface defining business logic operations for grade management
    /// This service handles grade-related business rules and validation
    /// </summary>
    public interface IGradeService
    {
        /// <summary>
        /// Retrieves all grades with student information
        /// </summary>
        /// <returns>Collection of grades with associated student data</returns>
        Task<IEnumerable<Grade>> GetAllGradesAsync();

        /// <summary>
        /// Retrieves a specific grade by its unique identifier
        /// </summary>
        /// <param name="gradeId">The grade's unique identifier</param>
        /// <returns>Grade details with student information, or null if not found</returns>
        Task<Grade?> GetGradeByIdAsync(int gradeId);

        /// <summary>
        /// Creates a new grade record with validation and business rules
        /// </summary>
        /// <param name="grade">The grade information to create</param>
        /// <returns>The created grade with calculated letter grade</returns>
        Task<Grade> CreateGradeAsync(Grade grade);

        /// <summary>
        /// Updates an existing grade record with validation
        /// </summary>
        /// <param name="grade">The updated grade information</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateGradeAsync(Grade grade);

        /// <summary>
        /// Deletes a grade record
        /// </summary>
        /// <param name="gradeId">The grade's unique identifier</param>
        /// <returns>True if deletion was successful, false if grade not found</returns>
        Task<bool> DeleteGradeAsync(int gradeId);

        /// <summary>
        /// Retrieves all grades for a specific student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>All grades belonging to the specified student</returns>
        Task<IEnumerable<Grade>> GetGradesByStudentAsync(int studentId);

        /// <summary>
        /// Retrieves grades for a specific subject across all students
        /// </summary>
        /// <param name="subject">The subject name</param>
        /// <returns>All grades in the specified subject</returns>
        Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject);

        /// <summary>
        /// Validates grade data before create or update operations
        /// </summary>
        /// <param name="grade">The grade data to validate</param>
        /// <param name="isUpdate">True if this is an update operation, false for create</param>
        /// <returns>List of validation error messages, empty if valid</returns>
        Task<List<string>> ValidateGradeAsync(Grade grade, bool isUpdate = false);

        /// <summary>
        /// Calculates the average grade for a student
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <returns>Average grade value, or null if no grades exist</returns>
        Task<decimal?> CalculateStudentAverageAsync(int studentId);

        /// <summary>
        /// Calculates the average grade for a subject
        /// </summary>
        /// <param name="subject">The subject name</param>
        /// <returns>Average grade value for the subject</returns>
        Task<decimal?> CalculateSubjectAverageAsync(string subject);

        /// <summary>
        /// Retrieves grades within a specific date range
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>Grades assigned within the specified date range</returns>
        Task<IEnumerable<Grade>> GetGradesByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Checks if a grade already exists for the same student, subject, and date
        /// </summary>
        /// <param name="studentId">The student's unique identifier</param>
        /// <param name="subject">The subject name</param>
        /// <param name="gradeDate">The grade date</param>
        /// <returns>True if duplicate exists, false otherwise</returns>
        Task<bool> IsDuplicateGradeAsync(int studentId, string subject, DateTime gradeDate);

        /// <summary>
        /// Retrieves grade statistics for reporting purposes
        /// </summary>
        /// <returns>Grade statistics and analytics</returns>
        Task<GradeStatistics> GetGradeStatisticsAsync();
    }

    /// <summary>
    /// Data transfer object for grade statistics and analytics
    /// </summary>
    public class GradeStatistics
    {
        public int TotalGrades { get; set; }
        public int TotalStudents { get; set; }
        public int UniqueSubjects { get; set; }
        public decimal OverallAverage { get; set; }
        public decimal HighestGrade { get; set; }
        public decimal LowestGrade { get; set; }
        public int PassingGrades { get; set; }
        public int FailingGrades { get; set; }
        public double PassingPercentage { get; set; }
        public Dictionary<string, decimal> AveragesBySubject { get; set; } = new();
        public Dictionary<string, int> GradeDistribution { get; set; } = new();
    }
}
```

#### Step 10: Create Service Implementation Classes

Create `Services/Implementations/StudentService.cs`:

```csharp
using RepositoryMVC.Models;
using RepositoryMVC.Repositories.Interfaces;
using RepositoryMVC.Services.Interfaces;
using System.Text.RegularExpressions;

namespace RepositoryMVC.Services.Implementations
{
    /// <summary>
    /// Student service implementation providing business logic for student management
    /// Coordinates between controllers and repositories while enforcing business rules
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor with dependency injection
        ///
        /// </summary>
        public async Task<Student> CreateStudentAsync(Student student)
        {
            try
            {
                // Business validation
                if (student == null)
                    throw new ArgumentNullException(nameof(student), "Student cannot be null");

                if (string.IsNullOrWhiteSpace(student.Name))
                    throw new ArgumentException("Student name is required", nameof(student));

                if (string.IsNullOrWhiteSpace(student.Email))
                    throw new ArgumentException("Student email is required", nameof(student));

                // Check for duplicate email
                if (await _studentRepository.IsEmailExistsAsync(student.Email))
                {
                    _logger.LogWarning("Attempt to create student with existing email: {Email}", student.Email);
                    throw new InvalidOperationException($"A student with email '{student.Email}' already exists");
                }

                // Set default enrollment date if not provided
                if (student.EnrollmentDate == default(DateTime))
                {
                    student.EnrollmentDate = DateTime.Today;
                    _logger.LogInformation("Set default enrollment date for student: {Email}", student.Email);
                }

                // Validate enrollment date is not in the future
                if (student.EnrollmentDate > DateTime.Today)
                {
                    throw new ArgumentException("Enrollment date cannot be in the future", nameof(student));
                }

                _logger.LogInformation("Creating new student: {Email}", student.Email);

                // Add student to repository
                await _studentRepository.AddAsync(student);
                
                // Save changes to database
                await _studentRepository.SaveChangesAsync();
                
                _logger.LogInformation("Successfully created student with ID: {StudentId}", student.StudentID);
                return student;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student: {Email}", student?.Email);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Update student with validation
        /// </summary>
        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                if (student == null)
                    throw new ArgumentNullException(nameof(student), "Student cannot be null");

                // Check if student exists
                var existingStudent = await _studentRepository.GetByIdAsync(student.StudentID);
                if (existingStudent == null)
                {
                    _logger.LogWarning("Attempt to update non-existent student: {StudentId}", student.StudentID);
                    return false;
                }

                // Check for duplicate email (excluding current student)
                if (await _studentRepository.IsEmailExistsAsync(student.Email, student.StudentID))
                {
                    _logger.LogWarning("Attempt to update student with existing email: {Email}", student.Email);
                    throw new InvalidOperationException($"Email '{student.Email}' is already taken by another student");
                }

                // Validate enrollment date
                if (student.EnrollmentDate > DateTime.Today)
                {
                    throw new ArgumentException("Enrollment date cannot be in the future", nameof(student));
                }

                _logger.LogInformation("Updating student: {StudentId}", student.StudentID);

                // Update student
                _studentRepository.Update(student);
                await _studentRepository.SaveChangesAsync();
                
                _logger.LogInformation("Successfully updated student: {StudentId}", student.StudentID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student: {StudentId}", student?.StudentID);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Delete student and cascade delete grades
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid student ID provided for deletion: {StudentId}", id);
                    return false;
                }

                var student = await _studentRepository.GetByIdAsync(id);
                if (student == null)
                {
                    _logger.LogWarning("Attempt to delete non-existent student: {StudentId}", id);
                    return false;
                }

                _logger.LogInformation("Deleting student: {StudentId} - {Name}", id, student.Name);

                // Remove student (grades will be cascade deleted due to FK constraint)
                _studentRepository.Remove(student);
                await _studentRepository.SaveChangesAsync();
                
                _logger.LogInformation("Successfully deleted student: {StudentId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student: {StudentId}", id);
                throw new InvalidOperationException($"Failed to delete student with ID {id}", ex);
            }
        }

        /// <summary>
        /// Business logic: Search students with validation
        /// </summary>
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Searching students with term: {SearchTerm}", searchTerm);
                
                // If search term is null or empty, return all students
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllStudentsAsync();
                }

                // Trim and validate search term
                searchTerm = searchTerm.Trim();
                if (searchTerm.Length < 2)
                {
                    _logger.LogWarning("Search term too short: {SearchTerm}", searchTerm);
                    return await GetAllStudentsAsync();
                }

                return await _studentRepository.SearchStudentsAsync(searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching students with term: {SearchTerm}", searchTerm);
                throw new InvalidOperationException("Failed to search students", ex);
            }
        }

        /// <summary>
        /// Business logic: Check if email is available
        /// </summary>
        public async Task<bool> IsEmailAvailableAsync(string email, int? excludeStudentId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                var exists = await _studentRepository.IsEmailExistsAsync(email, excludeStudentId);
                return !exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email availability: {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Get students by branch
        /// </summary>
        public async Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branch))
                    return await GetAllStudentsAsync();

                _logger.LogInformation("Getting students by branch: {Branch}", branch);
                return await _studentRepository.GetStudentsByBranchAsync(branch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting students by branch: {Branch}", branch);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Get students with calculated statistics
        /// </summary>
        public async Task<IEnumerable<Student>> GetStudentsWithStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Getting students with statistics");
                return await _studentRepository.GetStudentsWithAverageGradeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting students with statistics");
                throw;
            }
        }
    }
}
```

Create `Services/Implementations/GradeService.cs`:

```csharp
using RepositoryMVC.Models;
using RepositoryMVC.Repositories.Interfaces;
using RepositoryMVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace RepositoryMVC.Services.Implementations
{
    /// <summary>
    /// Grade service implementation containing business logic for grade management
    /// </summary>
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly ILogger<GradeService> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public GradeService(IGradeRepository gradeRepository, ILogger<GradeService> logger)
        {
            _gradeRepository = gradeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Business logic: Get all grades
        /// </summary>
        public async Task<IEnumerable<Grade>> GetAllGradesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all grades with student information");
                return await _gradeRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all grades");
                throw new InvalidOperationException("Failed to retrieve grades", ex);
            }
        }

        /// <summary>
        /// Business logic: Get grade by ID
        /// </summary>
        public async Task<Grade?> GetGradeByIdAsync(int gradeId)
        {
            try
            {
                if (gradeId <= 0)
                {
                    _logger.LogWarning("Invalid grade ID provided: {GradeId}", gradeId);
                    return null;
                }

                _logger.LogInformation("Retrieving grade with ID: {GradeId}", gradeId);
                return await _gradeRepository.GetByIdAsync(gradeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grade with ID: {GradeId}", gradeId);
                throw new InvalidOperationException($"Failed to retrieve grade with ID {gradeId}", ex);
            }
        }

        /// <summary>
        /// Business logic: Create new grade
        /// </summary>
        public async Task<Grade> CreateGradeAsync(Grade grade)
        {
            try
            {
                // Business validation
                if (grade == null)
                    throw new ArgumentNullException(nameof(grade), "Grade cannot be null");

                if (grade.StudentID <= 0)
                    throw new ArgumentException("Invalid student ID", nameof(grade));

                if (string.IsNullOrWhiteSpace(grade.Subject))
                    throw new ArgumentException("Subject is required", nameof(grade));

                // Calculate letter grade if not provided
                if (string.IsNullOrEmpty(grade.LetterGrade))
                    grade.LetterGrade = grade.CalculateLetterGrade();

                // Set grade date if not provided
                if (grade.GradeDate == default(DateTime))
                    grade.GradeDate = DateTime.Today;

                // Add grade to repository
                await _gradeRepository.AddAsync(grade);
                
                // Save changes to database
                await _gradeRepository.SaveChangesAsync();
                
                _logger.LogInformation("Successfully created grade for student ID: {StudentId}, Subject: {Subject}", grade.StudentID, grade.Subject);
                return grade;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grade for student ID: {StudentId}", grade?.StudentID);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Update grade
        /// </summary>
        public async Task<bool> UpdateGradeAsync(Grade grade)
        {
            try
            {
                if (grade == null)
                    throw new ArgumentNullException(nameof(grade), "Grade cannot be null");

                // Check if grade exists
                var existingGrade = await _gradeRepository.GetByIdAsync(grade.GradeID);
                if (existingGrade == null)
                {
                    _logger.LogWarning("Attempt to update non-existent grade: {GradeId}", grade.GradeID);
                    return false;
                }

                // Update grade
                _gradeRepository.Update(grade);
                await _gradeRepository.SaveChangesAsync();
                
                _logger.LogInformation("Successfully updated grade for student ID: {StudentId}, Subject: {Subject}", grade.StudentID, grade.Subject);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grade for ID: {GradeId}", grade?.GradeID);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Delete grade
        /// </summary>
        public async Task<bool> DeleteGradeAsync(int gradeId)
        {
            try
            {
                if (gradeId <= 0)
                {
                    _logger.LogWarning("Invalid grade ID provided for deletion: {GradeId}", gradeId);
                    return false;
                }

                var grade = await _gradeRepository.GetByIdAsync(gradeId);
                if (grade == null)
                {
                    _logger.LogWarning("Attempt to delete non-existent grade: {GradeId}", gradeId);
                    return false;
                }

                _logger.LogInformation("Deleting grade ID: {GradeId} for student ID: {StudentId}", gradeId, grade.StudentID);

                // Remove grade
                _gradeRepository.Remove(grade);
                await _gradeRepository.SaveChangesAsync();
                
                _logger.LogInformation("Successfully deleted grade ID: {GradeId}", gradeId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grade: {GradeId}", gradeId);
                throw new InvalidOperationException($"Failed to delete grade with ID {gradeId}", ex);
            }
        }

        /// <summary>
        /// Business logic: Get grades for a student
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesByStudentAsync(int studentId)
        {
            try
            {
                return await _gradeRepository.GetGradesByStudentIdAsync(studentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grades for student ID: {StudentId}", studentId);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Get grades for a subject
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject)
        {
            try
            {
                return await _gradeRepository.GetGradesBySubjectAsync(subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grades for subject: {Subject}", subject);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Calculate student's average grade
        /// </summary>
        public async Task<decimal?> CalculateStudentAverageAsync(int studentId)
        {
            try
            {
                return await _gradeRepository.GetAverageGradeForStudentAsync(studentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating average grade for student ID: {StudentId}", studentId);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Calculate subject average grade
        /// </summary>
        public async Task<decimal?> CalculateSubjectAverageAsync(string subject)
        {
            try
            {
                return await _gradeRepository.GetAverageGradeForSubjectAsync(subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating average grade for subject: {Subject}", subject);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Check if grade exists
        /// </summary>
        public async Task<bool> IsDuplicateGradeAsync(int studentId, string subject, DateTime gradeDate)
        {
            try
            {
                return await _gradeRepository.IsGradeExistsAsync(studentId, subject, gradeDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking for duplicate grade for student ID: {StudentId}, subject: {Subject}, date: {GradeDate}", studentId, subject, gradeDate);
                throw;
            }
        }

        /// <summary>
        /// Business logic: Get grade statistics
        /// </summary>
        public async Task<GradeStatistics> GetGradeStatisticsAsync()
        {
            try
            {
                var grades = await _gradeRepository.GetAllAsync();

                var totalGrades = grades.Count();
                var totalStudents = grades.Select(g => g.StudentID).Distinct().Count();
                var uniqueSubjects = grades.Select(g => g.Subject).Distinct().Count();
                var overallAverage = grades.Average(g => g.GradeValue);
                var highestGrade = grades.Max(g => g.GradeValue);
                var lowestGrade = grades.Min(g => g.GradeValue);
                var passingGrades = grades.Count(g => g.GradeValue >= 60);
                var failingGrades = grades.Count(g => g.GradeValue < 60);
                var passingPercentage = (double)passingGrades / totalGrades * 100;

                var averagesBySubject = grades
                    .GroupBy(g => g.Subject)
                    .ToDictionary(g => g.Key, g => g.Average(x => x.GradeValue));

                var gradeDistribution = grades
                    .GroupBy(g => g.LetterGrade)
                    .ToDictionary(g => g.Key, g => g.Count());

                return new GradeStatistics
                {
                    TotalGrades = totalGrades,
                    TotalStudents = totalStudents,
                    UniqueSubjects = uniqueSubjects,
                    OverallAverage = overallAverage,
                    HighestGrade = highestGrade,
                    LowestGrade = lowestGrade,
                    PassingGrades = passingGrades,
                    FailingGrades = failingGrades,
                    PassingPercentage = passingPercentage,
                    AveragesBySubject = averagesBySubject,
                    GradeDistribution = gradeDistribution
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calculating grade statistics", ex);
            }
        }
    }
}
```

### Phase 6: Configure Dependency Injection

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
builder.Services.AddScoped<IGradeService, GradeService>();
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

## Phase 8: Create Controllers and Views

### Step 11: Create Student Controller

Create a comprehensive controller in `Controllers/StudentsController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using RepositoryMVCDemo.Services.Interfaces;
using RepositoryMVCDemo.Models;

namespace RepositoryMVCDemo.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving students");
                TempData["ErrorMessage"] = "Error loading students.";
                return View(new List<Student>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentService.GetStudentWithGradesAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentService.CreateStudentAsync(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
    }
}
```

### Step 12: Create Basic Views

Create the following Razor views in `Views/Students/`:

**Index.cshtml:**
```html
@model IEnumerable<Student>
@{
    ViewData["Title"] = "Students";
}

<h2>Student Management</h2>

<a asp-action="Create" class="btn btn-primary">Add New Student</a>

<table class="table">
    <thead>
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Enrollment Date</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var student in Model)
        {
            <tr>
                <td>@student.FullName</td>
                <td>@student.Email</td>
                <td>@student.EnrollmentDate.ToString("MM/dd/yyyy")</td>
                <td>
                    <a asp-action="Details" asp-route-id="@student.Id" class="btn btn-info btn-sm">Details</a>
                    <a asp-action="Edit" asp-route-id="@student.Id" class="btn btn-warning btn-sm">Edit</a>
                    <a asp-action="Delete" asp-route-id="@student.Id" class="btn btn-danger btn-sm">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

## Phase 9: Final Configuration and Testing

### Step 13: Complete Program.cs Configuration

Ensure your `Program.cs` includes all necessary services:

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryMVCDemo.Data;
using RepositoryMVCDemo.Repositories.Interfaces;
using RepositoryMVCDemo.Repositories.Implementations;
using RepositoryMVCDemo.Services.Interfaces;
using RepositoryMVCDemo.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IGradeService, GradeService>();

var app = builder.Build();

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
    pattern: "{controller=Students}/{action=Index}/{id?}");

// Create database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
```

### Step 14: Run and Test the Application

1. **Build the project:**
   ```bash
   dotnet build
   ```

2. **Run the application:**
   ```bash
   dotnet run
   ```

3. **Test functionality:**
   - Navigate to the Students page
   - Create new students
   - Verify all CRUD operations work
   - Test the repository pattern implementation

## Conclusion

This tutorial has demonstrated a complete implementation of the Repository Pattern in ASP.NET Core MVC. You have learned:

### Key Benefits Achieved

1. **Separation of Concerns**: Clean architecture with distinct layers
2. **Testability**: Easy unit testing with repository interfaces
3. **Maintainability**: Loosely coupled, modular code structure
4. **Flexibility**: Easy to switch data sources or add new features
5. **Code Reusability**: Generic repository reduces code duplication

### Best Practices Implemented

- **Generic Repository Pattern**: Provides common CRUD operations
- **Specific Repositories**: Handle entity-specific business logic
- **Service Layer**: Manages business rules and validation
- **Dependency Injection**: Proper IoC container configuration
- **Error Handling**: Comprehensive exception management
- **Async/Await**: Modern asynchronous programming patterns

The Repository Pattern provides a solid foundation for enterprise applications, ensuring maintainable, testable, and scalable code architecture.

