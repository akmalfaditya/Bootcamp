# AutoMapper MVC - Complete Student Management System

## Project Overview

Welcome to the **AutoMapper MVC Student Management System** - a comprehensive ASP.NET Core MVC application that demonstrates the power of automated object mapping in enterprise web development. This project serves as a complete educational resource for understanding how AutoMapper transforms the traditionally complex and error-prone task of object-to-object mapping into an elegant, maintainable solution.

## What You Will Learn

By completing this tutorial, you will master:

- **AutoMapper Fundamentals**: Deep understanding of mapping concepts, profiles, and configuration
- **DTOs vs Entities**: Proper separation between domain models and data transfer objects
- **Advanced Mapping Scenarios**: Complex mappings, custom resolvers, and business logic integration
- **Performance Optimization**: Efficient database projections and mapping strategies
- **Enterprise Patterns**: Repository pattern, service layer architecture, and dependency injection
- **Security Best Practices**: Preventing over-posting attacks and data exposure
- **Testing Strategies**: Unit testing mapping configurations and integration testing
- **Production Deployment**: Configuration management and performance considerations

## Project Architecture & Design Philosophy

### Clean Architecture Principles
This project implements clean architecture with distinct layers:

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                       │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐ │
│  │   Controllers   │  │      Views      │  │  ViewModels │ │
│  │    (MVC)        │  │   (Razor)       │  │    (DTOs)   │ │
│  └─────────────────┘  └─────────────────┘  └─────────────┘ │
└─────────────────────────────────────────────────────────────┘
                               │
                    AutoMapper Profiles
                               │
┌─────────────────────────────────────────────────────────────┐
│                     Business Layer                          │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐ │
│  │    Services     │  │  Business Logic │  │  Validation │ │
│  │  (Use Cases)    │  │     Rules       │  │   Rules     │ │
│  └─────────────────┘  └─────────────────┘  └─────────────┘ │
└─────────────────────────────────────────────────────────────┘
                               │
┌─────────────────────────────────────────────────────────────┐
│                      Data Layer                             │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐ │
│  │   Repositories  │  │   DbContext     │  │   Entities  │ │
│  │ (Data Access)   │  │ (EF Core)       │  │  (Models)   │ │
│  └─────────────────┘  └─────────────────┘  └─────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

### AutoMapper Integration Strategy
AutoMapper serves as the translation layer between:
- **Domain Entities** → **Data Transfer Objects (DTOs)** for API responses
- **Create/Update DTOs** → **Domain Entities** for data persistence
- **Complex Business Objects** → **Simplified View Models** for UI presentation

## Comprehensive Step-by-Step Tutorial

This tutorial is structured to provide progressive learning, from basic concepts to advanced implementation. Each phase builds upon the previous one, ensuring complete understanding before advancing to complex scenarios.

## Project Overview

This project demonstrates the comprehensive implementation of **AutoMapper** in an ASP.NET Core MVC application for building a student management system. AutoMapper is a powerful object-to-object mapping library that eliminates the need for manual mapping between entities and DTOs (Data Transfer Objects), resulting in cleaner, more maintainable code.

## Architecture & Key Concepts

### 1. Understanding Models vs DTOs
- **Models (Entities)**: Represent the complete database structure with all internal fields, relationships, and business logic
- **DTOs (Data Transfer Objects)**: Simplified objects designed specifically for data transfer, exposing only the necessary fields for client consumption

### 2. AutoMapper Benefits Demonstrated
- **Code Reduction**: Eliminates repetitive manual mapping code
- **Maintainability**: Centralized mapping configuration in profiles
- **Security**: Controls data exposure by mapping only required fields
- **Productivity**: Automatic property mapping with intelligent conventions
- **Consistency**: Uniform mapping patterns across the entire application
- **Type Safety**: Compile-time checking of mapping configurations

### 3. Clean Architecture Implementation
- **Separation of Concerns**: Clear boundaries between data access, business logic, and presentation
- **Dependency Injection**: Loose coupling through service abstractions
- **Repository Pattern**: Abstracted data access layer
- **Service Layer**: Business logic encapsulation with AutoMapper integration

## Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQLite with Entity Framework Core 9.0
- **Mapping**: AutoMapper 12.0.1 with Microsoft DI Extensions
- **Architecture**: Clean Architecture with Repository and Service patterns
- **UI**: Razor Views with Bootstrap for responsive design

## Prerequisites

Before building this project, ensure you have:

1. **.NET 8.0 SDK** or later installed
2. **Visual Studio 2022** (recommended) or **Visual Studio Code** with C# extension
3. **Entity Framework Tools** (installed globally):
   ```powershell
   dotnet tool install --global dotnet-ef
   ```

## Complete Step-by-Step Implementation Guide

This comprehensive tutorial will guide you through building a complete AutoMapper MVC application from the ground up. Each step includes detailed explanations, code examples, and best practices to ensure thorough understanding.

### Phase 1: Project Foundation and Environment Setup

#### Step 1: Initialize the Project Structure

Begin by creating a new ASP.NET Core MVC project and installing the necessary dependencies:

```powershell
# Create a new solution directory
mkdir AutoMapperMVC-Tutorial
cd AutoMapperMVC-Tutorial

# Create new MVC project with specific framework version
dotnet new mvc -n AutoMapperMVC -f net8.0
cd AutoMapperMVC

# Install AutoMapper with Microsoft Dependency Injection extensions
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

# Install Entity Framework Core with SQLite provider
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

# Install Entity Framework Design tools for migrations
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# Verify package installation
dotnet list package
```

**Package Explanations:**
- **AutoMapper.Extensions.Microsoft.DependencyInjection**: Provides seamless integration with ASP.NET Core's built-in dependency injection container
- **Microsoft.EntityFrameworkCore.Sqlite**: SQLite database provider for Entity Framework Core
- **Microsoft.EntityFrameworkCore.Tools**: Command-line tools for Entity Framework operations
- **Microsoft.EntityFrameworkCore.Design**: Design-time components for Entity Framework

#### Step 2: Create Organized Project Directory Structure

Establish a well-organized folder structure that follows clean architecture principles:

```powershell
# Navigate to the project root directory
cd AutoMapperMVC

# Create Data Transfer Objects (DTOs) structure
mkdir DTOs
mkdir DTOs\Student
mkdir DTOs\Grade
mkdir DTOs\Common

# Create mapping configuration folder
mkdir MappingProfiles

# Create service layer folders
mkdir Services
mkdir Interfaces

# Create data access layer folder
mkdir Data

# Verify the structure has been created correctly
tree /F
```

**Expected Project Structure:**
```
AutoMapperMVC/
├── Controllers/          # MVC Controllers (existing)
├── Data/                # Database context and configurations
├── DTOs/                # Data Transfer Objects
│   ├── Common/          # Shared DTOs and base classes
│   ├── Grade/           # Grade-related DTOs
│   └── Student/         # Student-related DTOs
├── Interfaces/          # Service and repository interfaces
├── MappingProfiles/     # AutoMapper profile configurations
├── Models/              # Entity models (existing)
├── Services/            # Business logic services
├── Views/               # Razor views (existing)
└── wwwroot/            # Static files (existing)
```

**Architecture Reasoning:**
- **DTOs folder**: Separates data transfer concerns from domain models
- **MappingProfiles folder**: Centralizes AutoMapper configuration
- **Services folder**: Implements business logic and data operations
- **Interfaces folder**: Defines contracts for dependency injection

### Phase 2: Domain Model Implementation

#### Step 3: Create the Student Entity Model

Create the primary domain entity that represents a student in the database:

**Create `Models/Student.cs`:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.Models
{
    /// <summary>
    /// Represents a student entity in the database
    /// This is the domain model containing all business rules and database relationships
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Primary key for the student entity
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        /// Student's full name
        /// Required field with length validation
        /// </summary>
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender with restricted values
        /// </summary>
        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic department or major
        /// </summary>
        [Required(ErrorMessage = "Branch is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Branch must be between 2 and 50 characters")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Class section identifier
        /// </summary>
        [Required(ErrorMessage = "Section is required")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Section must be between 1 and 10 characters")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Contact email address with validation
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional phone number
        /// </summary>
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Date when the student enrolled
        /// </summary>
        [Required(ErrorMessage = "Enrollment date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Timestamp for record creation (audit field)
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp for last modification (audit field)
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property for related grades
        /// Virtual keyword enables lazy loading
        /// </summary>
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

        /// <summary>
        /// Calculated property for average grade
        /// This demonstrates business logic within the entity
        /// </summary>
        public decimal? CalculateAverageGrade()
        {
            if (Grades == null || !Grades.Any())
                return null;

            return Grades.Average(g => g.GradeValue);
        }
    }
}
```

**Key Design Decisions Explained:**
- **Data Annotations**: Provide both client-side and server-side validation
- **Navigation Properties**: Enable Entity Framework relationships
- **Audit Fields**: CreatedAt and UpdatedAt for tracking changes
- **Business Logic**: CalculateAverageGrade method encapsulates domain logic
- **Nullable Properties**: PhoneNumber is optional, reflecting real-world scenarios
```

#### Step 4: Create the Grade Entity Model

Develop the Grade entity to demonstrate one-to-many relationships and complex business logic:

**Create `Models/Grade.cs`:**

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMapperMVC.Models
{
    /// <summary>
    /// Represents a grade record for a specific student and subject
    /// Demonstrates complex entity relationships and business logic
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Primary key for the grade entity
        /// </summary>
        public int GradeID { get; set; }

        /// <summary>
        /// Foreign key reference to the Student entity
        /// </summary>
        [Required(ErrorMessage = "Student ID is required")]
        [ForeignKey("Student")]
        public int StudentID { get; set; }

        /// <summary>
        /// Subject name for this grade
        /// </summary>
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Subject name must be between 2 and 100 characters")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Numeric grade value with precise decimal handling
        /// </summary>
        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal GradeValue { get; set; }

        /// <summary>
        /// Letter grade representation (calculated or manually assigned)
        /// </summary>
        [StringLength(2, ErrorMessage = "Letter grade cannot exceed 2 characters")]
        public string? LetterGrade { get; set; }

        /// <summary>
        /// Date when the grade was recorded
        /// </summary>
        [Required(ErrorMessage = "Grade date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Grade Date")]
        public DateTime GradeDate { get; set; }

        /// <summary>
        /// Optional teacher comments about the grade
        /// </summary>
        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        [Display(Name = "Teacher Comments")]
        public string? Comments { get; set; }

        /// <summary>
        /// Navigation property back to the Student entity
        /// </summary>
        public virtual Student? Student { get; set; }

        /// <summary>
        /// Business logic method to calculate letter grade from numeric value
        /// This demonstrates domain logic encapsulation
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
                >= 63 => "D",
                >= 60 => "D-",
                _ => "F"
            };
        }

        /// <summary>
        /// Determines if the grade represents a passing score
        /// </summary>
        /// <returns>True if grade is passing, false otherwise</returns>
        public bool IsPassingGrade()
        {
            return GradeValue >= 60;
        }

        /// <summary>
        /// Calculates grade points for GPA computation
        /// </summary>
        /// <returns>Grade points on a 4.0 scale</returns>
        public decimal CalculateGradePoints()
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
                >= 63 => 1.0m,
                >= 60 => 0.7m,
                _ => 0.0m
            };
        }
    }
}
```

**Entity Relationship Design:**
- **One-to-Many Relationship**: One Student can have multiple Grades
- **Foreign Key Constraint**: StudentID links to Student.StudentID
- **Navigation Properties**: Enable Entity Framework to handle relationships automatically
- **Business Logic Methods**: Encapsulate grade calculation rules within the entity
```

### Phase 3: Data Transfer Objects (DTOs) Implementation

#### Step 5: Create Student Data Transfer Objects

Design DTOs that separate external data representation from internal domain models:

**Create `DTOs/Student/StudentDTO.cs`:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs.Student
{
    /// <summary>
    /// Data Transfer Object for displaying student information
    /// This DTO exposes only the data that should be visible to clients
    /// Property names may differ from entity properties to provide better API contracts
    /// </summary>
    public class StudentDTO
    {
        /// <summary>
        /// Student identifier (mapped from StudentID in entity)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Student's display name (mapped from Name in entity)
        /// </summary>
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender
        /// </summary>
        [Display(Name = "Gender")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic department (mapped from Branch in entity)
        /// This demonstrates property name transformation during mapping
        /// </summary>
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Class section
        /// </summary>
        [Display(Name = "Section")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Contact email address
        /// </summary>
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional phone number
        /// </summary>
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Enrollment date
        /// </summary>
        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Collection of related grades
        /// This will be mapped from the Student.Grades navigation property
        /// </summary>
        public List<GradeDTO> Grades { get; set; } = new List<GradeDTO>();

        /// <summary>
        /// Calculated average grade for display purposes
        /// This can be computed during mapping or retrieved from entity
        /// </summary>
        [Display(Name = "Average Grade")]
        [DisplayFormat(DataFormatString = "{0:F2}", NullDisplayText = "No grades")]
        public decimal? AverageGrade { get; set; }

        /// <summary>
        /// Formatted display text for enrollment information
        /// </summary>
        public string EnrollmentDisplayText => $"Enrolled on {EnrollmentDate:MMMM dd, yyyy}";
    }
}
```

**Create `DTOs/Student/StudentCreateDTO.cs`:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs.Student
{
    /// <summary>
    /// Data Transfer Object for creating new students
    /// Contains only the fields necessary for student creation
    /// Includes comprehensive validation rules for data integrity
    /// </summary>
    public class StudentCreateDTO
    {
        /// <summary>
        /// Student's full name with validation rules
        /// </summary>
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender with restricted options
        /// </summary>
        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic branch or department
        /// </summary>
        [Required(ErrorMessage = "Branch is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Branch must be between 2 and 50 characters")]
        [Display(Name = "Academic Branch")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Class section with specific format requirements
        /// </summary>
        [Required(ErrorMessage = "Section is required")]
        [RegularExpression("^[A-Z]$", ErrorMessage = "Section must be a single uppercase letter (A-Z)")]
        [Display(Name = "Section")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Email address with format validation
        /// </summary>
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional phone number with format validation
        /// </summary>
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Enrollment date with default value
        /// </summary>
        [Required(ErrorMessage = "Enrollment date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Custom validation method to ensure enrollment date is not in the future
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EnrollmentDate > DateTime.Today)
            {
                yield return new ValidationResult(
                    "Enrollment date cannot be in the future",
                    new[] { nameof(EnrollmentDate) });
            }

            if (EnrollmentDate < DateTime.Today.AddYears(-10))
            {
                yield return new ValidationResult(
                    "Enrollment date cannot be more than 10 years ago",
                    new[] { nameof(EnrollmentDate) });
            }
        }
    }
}
```

**Create `DTOs/Student/StudentUpdateDTO.cs`:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs.Student
{
    /// <summary>
    /// Data Transfer Object for updating existing students
    /// Contains ID for identification and editable fields
    /// Some fields like enrollment date may be restricted from editing
    /// </summary>
    public class StudentUpdateDTO
    {
        /// <summary>
        /// Student identifier (required for update operations)
        /// </summary>
        [Required(ErrorMessage = "Student ID is required")]
        public int Id { get; set; }

        /// <summary>
        /// Updated student name
        /// </summary>
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Updated academic branch
        /// </summary>
        [Required(ErrorMessage = "Branch is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Branch must be between 2 and 50 characters")]
        [Display(Name = "Academic Branch")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Updated section
        /// </summary>
        [Required(ErrorMessage = "Section is required")]
        [RegularExpression("^[A-Z]$", ErrorMessage = "Section must be a single uppercase letter (A-Z)")]
        [Display(Name = "Section")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Updated email address
        /// </summary>
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Updated phone number
        /// </summary>
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        // Note: Gender and EnrollmentDate are typically not editable after creation
        // This design decision reflects business rules about immutable student data
    }
}
```

```csharp
// DTOs/Grade/GradeDTO.cs - For displaying grade information
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs.Grade
{
    /// <summary>
    /// DTO for displaying grade information
    /// Contains computed fields and formatted display properties
    /// </summary>
    public class GradeDTO
    {
        /// <summary>
        /// Grade identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Associated student's name for display purposes
        /// This will be populated through AutoMapper from navigation properties
        /// </summary>
        [Display(Name = "Student Name")]
        public string StudentName { get; set; } = string.Empty;

        /// <summary>
        /// Subject name
        /// </summary>
        [Display(Name = "Subject")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Numeric grade value
        /// </summary>
        [Display(Name = "Grade")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = false)]
        public decimal GradeValue { get; set; }

        /// <summary>
        /// Letter grade representation
        /// </summary>
        [Display(Name = "Letter Grade")]
        public string? LetterGrade { get; set; }

        /// <summary>
        /// Date when grade was recorded
        /// </summary>
        [Display(Name = "Grade Date")]
        [DataType(DataType.Date)]
        public DateTime GradeDate { get; set; }

        /// <summary>
        /// Teacher comments
        /// </summary>
        [Display(Name = "Comments")]
        public string? Comments { get; set; }

        /// <summary>
        /// Indicates if this is a passing grade
        /// This property will be computed during mapping
        /// </summary>
        [Display(Name = "Status")]
        public bool IsPassingGrade { get; set; }

        /// <summary>
        /// Grade points for GPA calculation
        /// </summary>
        [Display(Name = "Grade Points")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = false)]
        public decimal GradePoints { get; set; }

        /// <summary>
        /// Formatted display string for the grade
        /// </summary>
        public string GradeDisplayText => $"{GradeValue:F1} ({LetterGrade})";
    }
}
```

**Create `DTOs/Grade/GradeCreateDTO.cs`:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs.Grade
{
    /// <summary>
    /// Data Transfer Object for creating new grades
    /// Contains validation rules and required fields for grade creation
    /// </summary>
    public class GradeCreateDTO
    {
        /// <summary>
        /// Student identifier for the grade
        /// </summary>
        [Required(ErrorMessage = "Student selection is required")]
        [Display(Name = "Student")]
        public int StudentID { get; set; }

        /// <summary>
        /// Subject name with validation
        /// </summary>
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Subject must be between 2 and 100 characters")]
        [Display(Name = "Subject")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Numeric grade value with range validation
        /// </summary>
        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Display(Name = "Grade Value")]
        public decimal GradeValue { get; set; }

        /// <summary>
        /// Date when grade was recorded
        /// </summary>
        [Required(ErrorMessage = "Grade date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Grade Date")]
        public DateTime GradeDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Optional teacher comments
        /// </summary>
        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        [Display(Name = "Teacher Comments")]
        public string? Comments { get; set; }
    }
}
```

### Phase 4: Database Context and Configuration

#### Step 7: Create Database Context
Create `Data/ApplicationDbContext.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using AutoMapperMVC.Models;

namespace AutoMapperMVC.Data
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

            // Configure Student-Grade relationship (One-to-Many)
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision for GradeValue
            modelBuilder.Entity<Grade>()
                .Property(g => g.GradeValue)
                .HasColumnType("decimal(5,2)");

            // Seed sample data
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentID = 1,
                    Name = "John Doe",
                    Gender = "Male",
                    Branch = "Computer Science",
                    Section = "A",
                    Email = "john.doe@example.com",
                    PhoneNumber = "123-456-7890",
                    EnrollmentDate = new DateTime(2023, 1, 15)
                }
            );

            modelBuilder.Entity<Grade>().HasData(
                new Grade
                {
                    GradeID = 1,
                    StudentID = 1,
                    Subject = "Mathematics",
                    GradeValue = 85.5m,
                    LetterGrade = "B",
                    GradeDate = new DateTime(2023, 6, 15),
                    Comments = "Good performance"
                }
            );
        }
    }
}
```

### Phase 5: AutoMapper Configuration

#### Step 8: Create Mapping Profiles
Create `MappingProfiles/StudentMappingProfile.cs`:
```csharp
using AutoMapper;
using AutoMapperMVC.DTOs;
using AutoMapperMVC.Models;

namespace AutoMapperMVC.MappingProfiles
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Branch))
                .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.Grades));

            // CreateDTO to Entity mapping
            CreateMap<StudentCreateDTO, Student>()
                .ForMember(dest => dest.StudentID, opt => opt.Ignore())
                .ForMember(dest => dest.Grades, opt => opt.Ignore());

            // DTO to Entity mapping for updates
            CreateMap<StudentDTO, Student>()
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Grades, opt => opt.Ignore());
        }
    }
}
```

Create `MappingProfiles/GradeMappingProfile.cs`:
```csharp
using AutoMapper;
using AutoMapperMVC.DTOs.Grade;
using AutoMapperMVC.Models;

namespace AutoMapperMVC.MappingProfiles
{
    public class GradeMappingProfile : Profile
    {
        public GradeMappingProfile()
        {
            // Entity to DTO mapping
            CreateMap<Grade, GradeDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GradeID))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => 
                    src.Student != null ? src.Student.Name : "Unknown Student"))
                .AfterMap((src, dest) =>
                {
                    // Ensure letter grade is calculated if missing
                    if (string.IsNullOrEmpty(dest.LetterGrade))
                    {
                        dest.LetterGrade = CalculateLetterGrade(dest.GradeValue);
                    }
                });

            // CreateDTO to Entity mapping
            CreateMap<GradeCreateDTO, Grade>()
                .ForMember(dest => dest.GradeID, opt => opt.Ignore()) // Auto-generated
                .ForMember(dest => dest.Student, opt => opt.Ignore()) // Loaded separately
                .AfterMap((src, dest) =>
                {
                    // Auto-calculate letter grade if not provided
                    if (string.IsNullOrEmpty(dest.LetterGrade))
                    {
                        dest.LetterGrade = CalculateLetterGrade(dest.GradeValue);
                    }
                    
                    // Set grade date to today if not specified
                    if (dest.GradeDate == default)
                    {
                        dest.GradeDate = DateTime.Today;
                    }
                });

            // Reverse mapping for editing
            CreateMap<Grade, GradeCreateDTO>()
                .ForMember(dest => dest.GradeDate, opt => opt.MapFrom(src => src.GradeDate.Date));
        }

        /// <summary>
        /// Business logic: Calculate letter grade from numeric value
        /// </summary>
        /// <param name="gradeValue">Numeric grade (0-100)</param>
        /// <returns>Letter grade (A, B, C, D, F)</returns>
        private static string CalculateLetterGrade(decimal gradeValue)
        {
            return gradeValue switch
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

### Phase 6: Service Layer Implementation

#### Step 9: Create Service Interfaces
Create `Interfaces/IStudentService.cs`:
```csharp
using AutoMapperMVC.DTOs;

namespace AutoMapperMVC.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudentsAsync();
        Task<IEnumerable<StudentDTO>> SearchStudentsAsync(string searchTerm);
        Task<StudentDTO?> GetStudentByIdAsync(int id);
        Task<StudentDTO> CreateStudentAsync(StudentCreateDTO createDto);
        Task<bool> UpdateStudentAsync(int id, StudentDTO studentDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
```

#### Step 10: Implement Service Classes
Create `Services/StudentService.cs`:
```csharp
using AutoMapperMVC.Data;
using AutoMapperMVC.Models;
using AutoMapperMVC.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperMVC.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
        {
            var students = await _context.Students
                .Include(s => s.Grades)
                .OrderBy(s => s.Name)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<IEnumerable<StudentDTO>> SearchStudentsAsync(string searchTerm)
        {
            var query = _context.Students.Include(s => s.Grades).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Name.Contains(searchTerm) ||
                                   s.Branch.Contains(searchTerm) ||
                                   s.Email.Contains(searchTerm));
            }

            var students = await query.OrderBy(s => s.Name).ToListAsync();
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<StudentDTO?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentID == id);

            return student != null ? _mapper.Map<StudentDTO>(student) : null;
        }

        public async Task<StudentDTO> CreateStudentAsync(StudentCreateDTO createDto)
        {
            var student = _mapper.Map<Student>(createDto);
            
            if (student.EnrollmentDate == default(DateTime))
            {
                student.EnrollmentDate = DateTime.Today;
            }
            
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDTO studentDto)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
                return false;

            _mapper.Map(studentDto, existingStudent);
            
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

### Phase 7: Controller Implementation

#### Step 11: Create Controllers with DTO Integration
Create `Controllers/StudentController.cs`:
```csharp
using Microsoft.AspNetCore.Mvc;
using AutoMapperMVC.Services;
using AutoMapperMVC.DTOs;

namespace AutoMapperMVC.Controllers
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
            if (id == null)
                return BadRequest("Student ID is required");

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null)
                return NotFound($"Student with ID {id} not found");

            return View(student);
        }

        public IActionResult Create()
        {
            return View(new StudentCreateDTO 
            { 
                EnrollmentDate = DateTime.Today 
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateDTO createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var studentDto = await _studentService.CreateStudentAsync(createDto);
                    TempData["SuccessMessage"] = $"Student {studentDto.FullName} created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating student: {ex.Message}");
                }
            }

            return View(createDto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest("Student ID is required");

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null)
                return NotFound($"Student with ID {id} not found");

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentDTO studentDto)
        {
            if (id != studentDto.Id)
                return BadRequest("ID mismatch");

            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _studentService.UpdateStudentAsync(id, studentDto);
                    if (!success)
                        return NotFound($"Student with ID {id} not found");
                    
                    TempData["SuccessMessage"] = $"Student {studentDto.FullName} updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating student: {ex.Message}");
                }
            }

            return View(studentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _studentService.DeleteStudentAsync(id);
            if (!success)
                return NotFound($"Student with ID {id} not found");

            TempData["SuccessMessage"] = "Student deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
```

### Phase 8: Dependency Injection and Configuration

#### Step 12: Configure Services in Program.cs
```csharp
using Microsoft.EntityFrameworkCore;
using AutoMapperMVC.Data;
using AutoMapperMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=studentmanagement.db"));

// Register AutoMapper with all profiles in the assembly
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register business services
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply database migrations automatically
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline
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

#### Step 13: Configure Application Settings
Update `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=studentmanagement.db"
  }
}
```

### Phase 9: Database Migration and Views

#### Step 14: Create and Apply Migrations
```powershell
# Add initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

#### Step 15: Create Razor Views
Create views in `Views/Student/` directory:
- `Index.cshtml` - Student listing with search
- `Details.cshtml` - Student details view
- `Create.cshtml` - Student creation form
- `Edit.cshtml` - Student editing form

Example `Views/Student/Index.cshtml`:
```html
@model IEnumerable<AutoMapperMVC.DTOs.StudentDTO>

@{
    ViewData["Title"] = "Students";
}

<h2>Student Management</h2>

<div class="row mb-3">
    <div class="col-md-6">
        <a asp-action="Create" class="btn btn-primary">Add New Student</a>
    </div>
    <div class="col-md-6">
        <form asp-action="Index" method="get" class="d-flex">
            <input name="searchTerm" value="@ViewBag.SearchTerm" class="form-control me-2" placeholder="Search students..." />
            <button type="submit" class="btn btn-outline-secondary">Search</button>
        </form>
    </div>
</div>

<div class="table-responsive">
    <table class="table table-striped">
        <thead>
            <tr>
                <th>@Html.DisplayNameFor(model => model.FullName)</th>
                <th>@Html.DisplayNameFor(model => model.Department)</th>
                <th>@Html.DisplayNameFor(model => model.Section)</th>
                <th>@Html.DisplayNameFor(model => model.Email)</th>
                <th>@Html.DisplayNameFor(model => model.EnrollmentDate)</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var student in Model)
            {
                <tr>
                    <td>@student.FullName</td>
                    <td>@student.Department</td>
                    <td>@student.Section</td>
                    <td>@student.Email</td>
                    <td>@student.EnrollmentDate.ToShortDateString()</td>
                    <td>
                        <a asp-action="Details" asp-route-id="@student.Id" class="btn btn-sm btn-info">Details</a>
                        <a asp-action="Edit" asp-route-id="@student.Id" class="btn btn-sm btn-warning">Edit</a>
                        <form asp-action="Delete" asp-route-id="@student.Id" method="post" class="d-inline" 
                              onsubmit="return confirm('Are you sure you want to delete this student?')">
                            <button type="submit" class="btn btn-sm btn-danger">Delete</button>
                        </form>
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>

@if (!Model.Any())
{
    <div class="alert alert-info">
        <h4>No students found</h4>
        <p>@(string.IsNullOrEmpty(ViewBag.SearchTerm) ? "No students have been added yet." : $"No students match your search term '{ViewBag.SearchTerm}'.")</p>
    </div>
}
```

#### Step 16: Test Core Functionality
1. **Navigate to** `https://localhost:7xxx/Student`
2. **Test CRUD Operations**:
   - Create new students
   - View student details
   - Edit existing students
   - Delete students
   - Search functionality
3. **Verify AutoMapper Integration**:
   - Check that field mappings work correctly (Name → FullName, Branch → Department)
   - Ensure DTOs are being used throughout the application
   - Validate that only appropriate data is exposed to views
## Advanced AutoMapper Features Demonstrated

### 1. Property Name Mapping
```csharp
// Mapping properties with different names
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
    .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Branch));
```

### 2. Ignoring Properties
```csharp
// Ignore properties during mapping
CreateMap<StudentCreateDTO, Student>()
    .ForMember(dest => dest.StudentID, opt => opt.Ignore())
    .ForMember(dest => dest.Grades, opt => opt.Ignore());
```

### 3. Custom Logic During Mapping
```csharp
// Execute custom logic after mapping
CreateMap<GradeCreateDTO, Grade>()
    .AfterMap((src, dest) =>
    {
        if (string.IsNullOrEmpty(dest.LetterGrade))
        {
            dest.LetterGrade = dest.CalculateLetterGrade();
        }
    });
```

### 4. Collection Mapping
```csharp
// AutoMapper automatically handles collections
var studentDTOs = _mapper.Map<IEnumerable<StudentDTO>>(students);
var grades = _mapper.Map<List<GradeDTO>>(student.Grades);
```

### 5. Conditional Mapping
```csharp
// Map only when condition is met
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.PhoneNumber, opt => 
        opt.MapFrom(src => !string.IsNullOrEmpty(src.PhoneNumber) ? src.PhoneNumber : "Not provided"));
```

### 6. Flattening and Unflattening
```csharp
// Flatten nested objects
CreateMap<Student, StudentSummaryDTO>()
    .ForMember(dest => dest.GradeCount, opt => opt.MapFrom(src => src.Grades.Count))
    .ForMember(dest => dest.AverageGrade, opt => opt.MapFrom(src => src.Grades.Average(g => g.GradeValue)));
```

## Database Schema Overview

### Students Table
- **StudentID** (Primary Key) - Auto-incrementing identifier
- **Name** - Student's full name (max 100 characters)
- **Gender** - Male, Female, or Other
- **Branch** - Academic department/major (max 50 characters)
- **Section** - Class section (max 10 characters)
- **Email** - Contact email (validated format)
- **PhoneNumber** - Optional contact number (max 20 characters)
- **EnrollmentDate** - Date student enrolled

### Grades Table
- **GradeID** (Primary Key) - Auto-incrementing identifier
- **StudentID** (Foreign Key) - References Students.StudentID
- **Subject** - Subject name (max 100 characters)
- **GradeValue** - Numeric grade (0-100, decimal precision)
- **LetterGrade** - Letter representation (A, B, C, D, F)
- **GradeDate** - Date grade was recorded
- **Comments** - Optional teacher comments (max 500 characters)

### Relationships
- **One-to-Many**: One Student can have many Grades
- **Cascade Delete**: Deleting a student removes all their grades

## AutoMapper vs Manual Mapping Comparison

### Manual Mapping (Before AutoMapper)
```csharp
public StudentDTO MapToDTO(Student student)
{
    return new StudentDTO
    {
        Id = student.StudentID,
        FullName = student.Name,
        Department = student.Branch,
        // ... manual mapping for each property
    };
}
```

### AutoMapper (After Implementation)
```csharp
// Configuration once in Profile
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
    .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Branch));

// Usage everywhere - just one line!
var studentDTO = _mapper.Map<StudentDTO>(student);
var studentDTOs = _mapper.Map<IEnumerable<StudentDTO>>(students);
```

**Key Benefits Demonstrated:**

1. **Code Reduction**: 90% less mapping code required
2. **Centralized Configuration**: All mapping rules defined once in profiles
3. **Automatic Null Handling**: AutoMapper handles null objects gracefully
4. **Collection Support**: Automatically maps collections without loops
5. **Type Safety**: Compile-time validation of mapping configurations
6. **Performance**: Expression compilation for optimized runtime performance

### Complex Mapping Examples

#### 1. Conditional Property Mapping
```csharp
// Map different values based on conditions
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => 
        string.IsNullOrEmpty(src.Name) ? "Unknown Student" : src.Name))
    .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => 
        !string.IsNullOrEmpty(src.PhoneNumber) ? src.PhoneNumber : src.Email));
```

#### 2. Computed Properties
```csharp
// Calculate values during mapping
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.AverageGrade, opt => opt.MapFrom(src => 
        src.Grades.Any() ? src.Grades.Average(g => g.GradeValue) : (decimal?)null))
    .ForMember(dest => dest.GradeCount, opt => opt.MapFrom(src => src.Grades.Count))
    .ForMember(dest => dest.EnrollmentDuration, opt => opt.MapFrom(src => 
        (DateTime.Now - src.EnrollmentDate).Days));
```

#### 3. Nested Object Flattening
```csharp
// Flatten complex hierarchies
CreateMap<Student, StudentSummaryDTO>()
    .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
    .ForMember(dest => dest.HighestGradeValue, opt => opt.MapFrom(src => 
        src.Grades.Any() ? src.Grades.Max(g => g.GradeValue) : 0))
    .ForMember(dest => dest.HighestGradeSubject, opt => opt.MapFrom(src => 
        src.Grades.OrderByDescending(g => g.GradeValue).FirstOrDefault().Subject));
```

#### 4. Business Logic Integration
```csharp
// Execute business rules during mapping
CreateMap<GradeCreateDTO, Grade>()
    .AfterMap((src, dest, context) =>
    {
        // Auto-calculate letter grade
        dest.LetterGrade = CalculateLetterGrade(dest.GradeValue);
        
        // Set audit fields
        dest.CreatedAt = DateTime.UtcNow;
        dest.CreatedBy = context.Items["CurrentUser"]?.ToString() ?? "System";
        
        // Apply business validation
        if (dest.GradeValue > 100)
            throw new ArgumentException("Grade cannot exceed 100");
    });
```

### Performance Optimization Techniques

#### 1. Database Projection with ProjectTo
```csharp
// Efficient database queries - only select needed columns
public async Task<IEnumerable<StudentDTO>> GetAllStudentsOptimized()
{
    return await _context.Students
        .ProjectTo<StudentDTO>(_mapper.ConfigurationProvider)
        .ToListAsync();
}

// This generates efficient SQL like:
// SELECT s.StudentID as Id, s.Name as FullName, s.Branch as Department 
// FROM Students s
```

#### 2. Lazy Loading with Conditional Mapping
```csharp
// Only load related data when needed
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => 
        src.Grades != null ? src.Grades : new List<Grade>()))
    .ForMember(dest => dest.GradeCount, opt => opt.MapFrom(src => 
        src.Grades?.Count ?? 0));
```

#### 3. Custom Value Resolvers for Complex Logic
```csharp
// Reusable mapping logic
public class GradeAverageResolver : IValueResolver<Student, StudentDTO, decimal?>
{
    public decimal? Resolve(Student source, StudentDTO destination, 
        decimal? destMember, ResolutionContext context)
    {
        if (source.Grades == null || !source.Grades.Any())
            return null;
            
        return source.Grades.Average(g => g.GradeValue);
    }
}

// Usage in profile
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.AverageGrade, opt => opt.MapFrom<GradeAverageResolver>());
```

## Real-World Implementation Benefits

### Before AutoMapper Implementation
```csharp
// Manual mapping method (repetitive and error-prone)
public class StudentController : Controller
{
    public async Task<IActionResult> Index()
    {
        var students = await _context.Students.Include(s => s.Grades).ToListAsync();
        
        var studentDTOs = new List<StudentDTO>();
        foreach (var student in students)
        {
            var dto = new StudentDTO
            {
                Id = student.StudentID,
                FullName = student.Name,
                Department = student.Branch,
                Section = student.Section,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                EnrollmentDate = student.EnrollmentDate,
                Grades = new List<GradeDTO>()
            };
            
            // Manual grade mapping
            foreach (var grade in student.Grades)
            {
                dto.Grades.Add(new GradeDTO
                {
                    Id = grade.GradeID,
                    Subject = grade.Subject,
                    GradeValue = grade.GradeValue,
                    LetterGrade = grade.LetterGrade,
                    GradeDate = grade.GradeDate,
                    Comments = grade.Comments,
                    StudentName = student.Name
                });
            }
            
            // Calculate average manually
            if (dto.Grades.Any())
            {
                dto.AverageGrade = dto.Grades.Average(g => g.GradeValue);
            }
            
            studentDTOs.Add(dto);
        }
        
        return View(studentDTOs);
    }
}
```

### After AutoMapper Implementation
```csharp
// Clean, maintainable code with AutoMapper
public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    
    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    
    public async Task<IActionResult> Index()
    {
        var studentDTOs = await _studentService.GetAllStudentsAsync();
        return View(studentDTOs);
    }
}

// Service layer with AutoMapper
public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
    {
        var students = await _context.Students
            .Include(s => s.Grades)
            .ToListAsync();
            
        return _mapper.Map<IEnumerable<StudentDTO>>(students);
    }
}
```

**Lines of Code Comparison:**
- **Manual Mapping**: 45+ lines per method
- **AutoMapper**: 3-5 lines per method
- **Maintenance**: Changes require updating one profile vs. multiple methods

### Error Reduction Examples

#### Common Manual Mapping Errors (Fixed by AutoMapper)

1. **Null Reference Exceptions**:
```csharp
// Manual (error-prone)
dto.StudentName = student.Name; // Crashes if student is null

// AutoMapper (safe)
var dto = _mapper.Map<GradeDTO>(grade); // Handles nulls automatically
```

2. **Property Name Mismatches**:
```csharp
// Manual (typo-prone)
dto.Fullname = student.Name; // Wrong property name - runtime error

// AutoMapper (compile-time safe)
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
// Compile-time validation catches errors
```

3. **Collection Mapping Inconsistencies**:
```csharp
// Manual (inconsistent handling)
foreach (var grade in student.Grades ?? new List<Grade>())
{
    // Different null handling in different places
}

// AutoMapper (consistent)
// Handles collections uniformly across all mappings
```

## Advanced AutoMapper Features in Practice

### 1. Custom Type Converters
```csharp
// Convert between different date formats
public class DateToStringConverter : ITypeConverter<DateTime, string>
{
    public string Convert(DateTime source, string destination, ResolutionContext context)
    {
        return source.ToString("MMMM dd, yyyy");
    }
}

// Registration in profile
CreateMap<DateTime, string>().ConvertUsing<DateToStringConverter>();
```

### 2. Conditional Object Creation
```csharp
// Create different DTOs based on user roles
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom((src, dest, destMember, context) =>
    {
        var userRole = context.Items["UserRole"]?.ToString();
        return userRole == "Admin" ? src.PhoneNumber : "***-***-****";
    }));
```

### 3. Multi-Step Mapping Pipelines
```csharp
// Chain multiple mappings
public async Task<StudentReportDTO> GenerateStudentReport(int studentId)
{
    var student = await _context.Students
        .Include(s => s.Grades)
        .FirstOrDefaultAsync(s => s.StudentID == studentId);
    
    // Step 1: Entity to DTO
    var studentDTO = _mapper.Map<StudentDTO>(student);
    
    // Step 2: DTO to Report DTO with calculations
    var reportDTO = _mapper.Map<StudentReportDTO>(studentDTO);
    
    return reportDTO;
}
```

### 4. Dynamic Mapping Configuration
```csharp
// Configure mappings at runtime
public void ConfigureDynamicMapping(string userRole)
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<StudentMappingProfile>();
        
        if (userRole == "Student")
        {
            cfg.CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore());
        }
    });
    
    var mapper = config.CreateMapper();
}
```

## Testing AutoMapper Configurations

### Unit Testing Mapping Profiles
```csharp
[TestClass]
public class StudentMappingProfileTests
{
    private IMapper _mapper;
    
    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => 
        {
            cfg.AddProfile<StudentMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }
    
    [TestMethod]
    public void Should_Map_Student_To_StudentDTO_Correctly()
    {
        // Arrange
        var student = new Student
        {
            StudentID = 1,
            Name = "John Doe",
            Branch = "Computer Science",
            Email = "john@example.com",
            Grades = new List<Grade>
            {
                new Grade { GradeValue = 85.5m, Subject = "Math" },
                new Grade { GradeValue = 92.0m, Subject = "Physics" }
            }
        };
        
        // Act
        var result = _mapper.Map<StudentDTO>(student);
        
        // Assert
        Assert.AreEqual(student.StudentID, result.Id);
        Assert.AreEqual(student.Name, result.FullName);
        Assert.AreEqual(student.Branch, result.Department);
        Assert.AreEqual(2, result.Grades.Count);
        Assert.AreEqual(88.75m, result.AverageGrade); // (85.5 + 92.0) / 2
    }
    
    [TestMethod]
    public void Should_Handle_Null_Student_Gracefully()
    {
        // Act
        var result = _mapper.Map<StudentDTO>((Student)null);
        
        // Assert
        Assert.IsNull(result);
    }
    
    [TestMethod]
    public void AutoMapper_Configuration_Should_Be_Valid()
    {
        // Assert - This will throw if configuration is invalid
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
```

### Integration Testing with Database
```csharp
[TestClass]
public class StudentServiceIntegrationTests
{
    private ApplicationDbContext _context;
    private IMapper _mapper;
    private StudentService _service;
    
    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<StudentMappingProfile>());
        _mapper = config.CreateMapper();
        
        _service = new StudentService(_context, _mapper);
    }
    
    [TestMethod]
    public async Task GetAllStudentsAsync_Should_Return_Mapped_DTOs()
    {
        // Arrange
        _context.Students.AddRange(
            new Student { Name = "John", Branch = "CS", Email = "john@test.com" },
            new Student { Name = "Jane", Branch = "IT", Email = "jane@test.com" }
        );
        await _context.SaveChangesAsync();
        
        // Act
        var results = await _service.GetAllStudentsAsync();
        
        // Assert
        Assert.AreEqual(2, results.Count());
        Assert.IsTrue(results.All(s => !string.IsNullOrEmpty(s.FullName)));
        Assert.IsTrue(results.All(s => !string.IsNullOrEmpty(s.Department)));
    }
}
```

## Common Issues and Troubleshooting

### 1. Missing Type Maps
**Problem**: `AutoMapperMappingException: Missing type map configuration`

**Solution**:
```csharp
// Ensure all required mappings are configured
CreateMap<Student, StudentDTO>();
CreateMap<StudentCreateDTO, Student>();
CreateMap<Grade, GradeDTO>();

// Or use ReverseMap for bidirectional mapping
CreateMap<Student, StudentDTO>().ReverseMap();
```

### 2. Circular Reference Issues
**Problem**: Stack overflow with navigation properties

**Solution**:
```csharp
// Limit mapping depth
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.Grades))
    .MaxDepth(2);

// Or ignore problematic properties
CreateMap<Grade, GradeDTO>()
    .ForMember(dest => dest.Student, opt => opt.Ignore());
```