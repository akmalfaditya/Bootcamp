# FluentValidation MVC Project

## Overview

This project demonstrates the implementation of **FluentValidation** in ASP.NET Core MVC as a powerful alternative to Data Annotations for model validation. The project showcases a Student Management System with comprehensive validation rules using FluentValidation's fluent API.

## What is FluentValidation?

FluentValidation is a .NET library for building strongly-typed validation rules using a fluent interface. It provides a clean way to separate validation logic from your domain models, making your code more maintainable and testable.

## Benefits of FluentValidation over Data Annotations

### 1. **Separation of Concerns**
- **Data Annotations**: Validation logic is mixed with model properties
- **FluentValidation**: Validation logic is separated into dedicated validator classes

### 2. **Complex Validation Rules**
- **Data Annotations**: Limited to simple validation attributes
- **FluentValidation**: Supports complex conditional logic, cross-field validation, and custom rules

### 3. **Testability**
- **Data Annotations**: Difficult to unit test validation logic
- **FluentValidation**: Validators are separate classes that can be easily unit tested

### 4. **Reusability**
- **Data Annotations**: Validation rules are tied to specific models
- **FluentValidation**: Validators can be reused and composed

### 5. **Better Error Messages**
- **Data Annotations**: Limited error message customization
- **FluentValidation**: Rich error message formatting with placeholders and conditional messages

### 6. **Conditional Validation**
- **Data Annotations**: Basic conditional validation with limited options
- **FluentValidation**: Powerful `When()` and `Unless()` methods for complex conditions

## Project Structure

```
FluentValidationMVC/
├── Controllers/
│   ├── HomeController.cs          # Main controller
│   └── StudentController.cs       # Student CRUD operations
├── Models/
│   ├── Student.cs                 # Student model (clean, no validation attributes)
│   ├── Grade.cs                   # Grade model (clean, no validation attributes)
│   └── ErrorViewModel.cs          # Error handling model
├── Validators/
│   ├── StudentValidator.cs        # FluentValidation rules for Student
│   └── GradeValidator.cs          # FluentValidation rules for Grade
├── Data/
│   └── ApplicationDbContext.cs    # Entity Framework context
├── Services/
│   └── StudentService.cs          # Business logic service
└── Views/
    ├── Home/                      # Home views
    └── Student/                   # Student management views
```

## Key Features Demonstrated

### 1. **Student Validation**
- **Name**: Required, length validation (2-50 characters)
- **Gender**: Must be "Male" or "Female"
- **Branch**: Required, length validation (2-100 characters)
- **Section**: Required, single character A-Z
- **Email**: Required, valid email format, university domain restriction
- **Phone Number**: Optional, valid format when provided
- **Enrollment Date**: Cannot be in the future

### 2. **Grade Validation**
- **Student ID**: Must exist in database
- **Subject**: Required, length validation
- **Grade Value**: 0-100 range
- **Letter Grade**: Must match numeric grade (A=90-100, B=80-89, etc.)
- **Grade Date**: Cannot be in the future
- **Comments**: Optional, length limit when provided

### 3. **Cross-Field Validation**
- Letter grade must correspond to the numeric grade value
- Complex business rules with custom validation methods

## Implementation Guide

### Step 1: Install FluentValidation Package

```bash
dotnet add package FluentValidation.AspNetCore
```

### Step 2: Create Validator Classes

Create validators that inherit from `AbstractValidator<T>`:

```csharp
public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.")
            .Must(email => email.EndsWith("@university.edu"))
            .WithMessage("Email must be a university domain (@university.edu).");
    }
}
```

### Step 3: Register FluentValidation in Program.cs

```csharp
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>();
```

### Step 4: Clean Your Models

Remove Data Annotation validation attributes, keep only UI-related attributes:

```csharp
public class Student
{
    public int StudentID { get; set; }
    
    [Display(Name = "Full Name")]
    public string Name { get; set; }
    
    [Display(Name = "Email Address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    // ... other properties without validation attributes
}
```

## Step-by-Step Implementation Guide

This section provides a comprehensive, step-by-step guide on how to implement FluentValidation from scratch in an ASP.NET Core MVC application. Follow these steps to transform your application from Data Annotations to FluentValidation.

### Phase 1: Project Setup and Package Installation

#### Step 1: Install FluentValidation Packages
FluentValidation requires specific NuGet packages for ASP.NET Core integration:

```bash
# Install the core FluentValidation package for ASP.NET Core
dotnet add package FluentValidation.AspNetCore

# Alternative: Install individual packages for more control
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions
```

**Package Explanation:**
- `FluentValidation.AspNetCore`: Main package that includes automatic validation, client-side validation adapters, and dependency injection extensions
- `FluentValidation`: Core library with validation rules and engine
- `FluentValidation.DependencyInjectionExtensions`: Provides automatic validator discovery and registration

#### Step 2: Create Validators Folder Structure
Organize your validation logic in a dedicated folder structure:

```bash
# Create validators folder
mkdir Validators

# Optional: Create subfolders for organization
mkdir Validators/Student
mkdir Validators/Grade
mkdir Validators/Common
```

**Folder Structure Best Practices:**
```
Validators/
├── Common/                    # Shared validation logic
│   ├── BaseValidator.cs       # Common validation methods
│   └── ValidationExtensions.cs # Custom validation extensions
├── Student/
│   ├── StudentValidator.cs    # Main student validation
│   └── StudentCreateValidator.cs # Create-specific validation
└── Grade/
    ├── GradeValidator.cs      # Grade validation rules
    └── GradeUpdateValidator.cs # Update-specific validation
```

### Phase 2: Analyze and Clean Existing Models

#### Step 3: Identify Current Validation Logic
Before implementing FluentValidation, analyze your existing models to understand current validation requirements:

**Example of Data Annotations Model:**
```csharp
// BEFORE: Model with Data Annotations
public class Student
{
    public int StudentID { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2-50 characters")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Gender is required")]
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be Male or Female")]
    public string Gender { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [RegularExpression(@".*@university\.edu$", ErrorMessage = "Must use university email")]
    public string Email { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }
    
    [Required(ErrorMessage = "Branch is required")]
    [StringLength(100, MinimumLength = 2)]
    public string Branch { get; set; }
    
    [Required(ErrorMessage = "Section is required")]
    [RegularExpression("^[A-Z]$", ErrorMessage = "Section must be a single letter A-Z")]
    public string Section { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime EnrollmentDate { get; set; }
}
```

#### Step 4: Clean Your Models
Remove validation attributes and keep only UI-related attributes:

```csharp
// AFTER: Clean Model without validation attributes
using System.ComponentModel.DataAnnotations;

namespace FluentValidationMVC.Models
{
    /// <summary>
    /// Student model - clean domain entity without validation logic
    /// Validation is handled separately by FluentValidation validators
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Unique identifier for the student
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        /// Student's full name
        /// </summary>
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender
        /// </summary>
        [Display(Name = "Gender")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic branch/department
        /// </summary>
        [Display(Name = "Branch")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Class section identifier
        /// </summary>
        [Display(Name = "Section")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Student's email address
        /// </summary>
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]  // UI hint only
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional phone number
        /// </summary>
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]  // UI hint only
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Date when student enrolled
        /// </summary>
        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]  // UI hint only
        public DateTime EnrollmentDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Navigation property for related grades
        /// </summary>
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
```

**Key Points:**
- Remove ALL validation attributes (`[Required]`, `[StringLength]`, `[RegularExpression]`, etc.)
- Keep UI-related attributes (`[Display]`, `[DataType]`) for proper form rendering
- Add XML documentation for better code clarity
- Initialize collections and strings to avoid null reference issues

### Phase 3: Create Basic Validators

#### Step 5: Create Your First Validator
Start with a basic validator for the Student model:

```csharp
// Validators/StudentValidator.cs
using FluentValidation;
using FluentValidationMVC.Models;

namespace FluentValidationMVC.Validators
{
    /// <summary>
    /// FluentValidation validator for Student model
    /// Contains all validation rules for student data
    /// </summary>
    public class StudentValidator : AbstractValidator<Student>
    {
        /// <summary>
        /// Constructor where all validation rules are defined
        /// </summary>
        public StudentValidator()
        {
            // Configure validation rules in constructor
            ConfigureNameValidation();
            ConfigureGenderValidation();
            ConfigureBranchValidation();
            ConfigureSectionValidation();
            ConfigureEmailValidation();
            ConfigurePhoneValidation();
            ConfigureDateValidation();
        }

        /// <summary>
        /// Configure name validation rules
        /// </summary>
        private void ConfigureNameValidation()
        {
            RuleFor(student => student.Name)
                .NotEmpty()
                .WithMessage("Student name is required.")
                .Length(2, 50)
                .WithMessage("Name must be between 2 and 50 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name can only contain letters and spaces.");
        }

        /// <summary>
        /// Configure gender validation rules
        /// </summary>
        private void ConfigureGenderValidation()
        {
            RuleFor(student => student.Gender)
                .NotEmpty()
                .WithMessage("Gender is required.")
                .Must(BeValidGender)
                .WithMessage("Gender must be either 'Male' or 'Female'.");
        }

        /// <summary>
        /// Configure branch validation rules
        /// </summary>
        private void ConfigureBranchValidation()
        {
            RuleFor(student => student.Branch)
                .NotEmpty()
                .WithMessage("Branch is required.")
                .Length(2, 100)
                .WithMessage("Branch must be between 2 and 100 characters.");
        }

        /// <summary>
        /// Configure section validation rules
        /// </summary>
        private void ConfigureSectionValidation()
        {
            RuleFor(student => student.Section)
                .NotEmpty()
                .WithMessage("Section is required.")
                .Matches(@"^[A-Z]$")
                .WithMessage("Section must be a single uppercase letter (A-Z).");
        }

        /// <summary>
        /// Configure email validation rules
        /// </summary>
        private void ConfigureEmailValidation()
        {
            RuleFor(student => student.Email)
                .NotEmpty()
                .WithMessage("Email address is required.")
                .EmailAddress()
                .WithMessage("Please enter a valid email address.")
                .Must(BeUniversityEmail)
                .WithMessage("Email must be a university domain (@university.edu).");
        }

        /// <summary>
        /// Configure phone number validation (optional field)
        /// </summary>
        private void ConfigurePhoneValidation()
        {
            RuleFor(student => student.PhoneNumber)
                .Matches(@"^\+?[\d\s\-\(\)]+$")
                .WithMessage("Please enter a valid phone number format.")
                .When(student => !string.IsNullOrEmpty(student.PhoneNumber));
        }

        /// <summary>
        /// Configure enrollment date validation
        /// </summary>
        private void ConfigureDateValidation()
        {
            RuleFor(student => student.EnrollmentDate)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Enrollment date cannot be in the future.");
        }

        /// <summary>
        /// Custom validation method for gender
        /// </summary>
        /// <param name="gender">Gender value to validate</param>
        /// <returns>True if valid gender</returns>
        private static bool BeValidGender(string gender)
        {
            return gender?.ToLower() is "male" or "female";
        }

        /// <summary>
        /// Custom validation method for university email
        /// </summary>
        /// <param name="email">Email to validate</param>
        /// <returns>True if university email</returns>
        private static bool BeUniversityEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
                
            return email.EndsWith("@university.edu", StringComparison.OrdinalIgnoreCase);
        }
    }
}
```

#### Step 6: Create Grade Validator
Create a validator for the Grade model with cross-field validation:

```csharp
// Validators/GradeValidator.cs
using FluentValidation;
using FluentValidationMVC.Models;

namespace FluentValidationMVC.Validators
{
    /// <summary>
    /// FluentValidation validator for Grade model
    /// Demonstrates cross-field validation and complex business rules
    /// </summary>
    public class GradeValidator : AbstractValidator<Grade>
    {
        /// <summary>
        /// Constructor with validation rules
        /// </summary>
        public GradeValidator()
        {
            ConfigureStudentValidation();
            ConfigureSubjectValidation();
            ConfigureGradeValueValidation();
            ConfigureLetterGradeValidation();
            ConfigureDateValidation();
            ConfigureCommentsValidation();
        }

        /// <summary>
        /// Configure student ID validation
        /// </summary>
        private void ConfigureStudentValidation()
        {
            RuleFor(grade => grade.StudentID)
                .GreaterThan(0)
                .WithMessage("A valid student must be selected.");
        }

        /// <summary>
        /// Configure subject validation
        /// </summary>
        private void ConfigureSubjectValidation()
        {
            RuleFor(grade => grade.Subject)
                .NotEmpty()
                .WithMessage("Subject is required.")
                .Length(2, 50)
                .WithMessage("Subject must be between 2 and 50 characters.");
        }

        /// <summary>
        /// Configure numeric grade validation
        /// </summary>
        private void ConfigureGradeValueValidation()
        {
            RuleFor(grade => grade.GradeValue)
                .InclusiveBetween(0, 100)
                .WithMessage("Grade must be between 0 and 100.")
                .ScalePrecision(2, 5)
                .WithMessage("Grade can have at most 2 decimal places.");
        }

        /// <summary>
        /// Configure letter grade validation with cross-field validation
        /// </summary>
        private void ConfigureLetterGradeValidation()
        {
            RuleFor(grade => grade.LetterGrade)
                .Must((grade, letterGrade) => BeValidLetterGrade(letterGrade))
                .WithMessage("Letter grade must be A, B, C, D, or F.")
                .When(grade => !string.IsNullOrEmpty(grade.LetterGrade));

            // Cross-field validation: letter grade must match numeric grade
            RuleFor(grade => grade.LetterGrade)
                .Must((grade, letterGrade) => HaveMatchingLetterGrade(grade.GradeValue, letterGrade))
                .WithMessage("Letter grade must match the numeric grade value.")
                .When(grade => !string.IsNullOrEmpty(grade.LetterGrade));
        }

        /// <summary>
        /// Configure grade date validation
        /// </summary>
        private void ConfigureDateValidation()
        {
            RuleFor(grade => grade.GradeDate)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Grade date cannot be in the future.");
        }

        /// <summary>
        /// Configure comments validation (optional field)
        /// </summary>
        private void ConfigureCommentsValidation()
        {
            RuleFor(grade => grade.Comments)
                .MaximumLength(500)
                .WithMessage("Comments cannot exceed 500 characters.")
                .When(grade => !string.IsNullOrEmpty(grade.Comments));
        }

        /// <summary>
        /// Validates if letter grade is one of the accepted values
        /// </summary>
        /// <param name="letterGrade">Letter grade to validate</param>
        /// <returns>True if valid letter grade</returns>
        private static bool BeValidLetterGrade(string? letterGrade)
        {
            if (string.IsNullOrEmpty(letterGrade))
                return true; // Optional field

            var validGrades = new[] { "A", "B", "C", "D", "F" };
            return validGrades.Contains(letterGrade.ToUpper());
        }

        /// <summary>
        /// Cross-field validation: ensures letter grade matches numeric grade
        /// </summary>
        /// <param name="numericGrade">Numeric grade value</param>
        /// <param name="letterGrade">Letter grade</param>
        /// <returns>True if letter grade matches numeric grade</returns>
        private static bool HaveMatchingLetterGrade(decimal numericGrade, string? letterGrade)
        {
            if (string.IsNullOrEmpty(letterGrade))
                return true; // Optional field

            return letterGrade.ToUpper() switch
            {
                "A" => numericGrade >= 90,
                "B" => numericGrade >= 80 && numericGrade < 90,
                "C" => numericGrade >= 70 && numericGrade < 80,
                "D" => numericGrade >= 60 && numericGrade < 70,
                "F" => numericGrade < 60,
                _ => false
            };
        }
    }
}
```

### Phase 4: Configure FluentValidation in Your Application

#### Step 7: Register FluentValidation in Program.cs
Configure FluentValidation services in your `Program.cs` file:

```csharp
// Program.cs
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidationMVC.Data;
using FluentValidationMVC.Services;
using FluentValidationMVC.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MVC services
builder.Services.AddControllersWithViews();

// Configure FluentValidation
ConfigureFluentValidation(builder.Services);

// Add application services
builder.Services.AddScoped<StudentService>();

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/// <summary>
/// Configure FluentValidation services and options
/// </summary>
/// <param name="services">Service collection</param>
static void ConfigureFluentValidation(IServiceCollection services)
{
    // Add FluentValidation services
    services.AddFluentValidationAutoValidation(options =>
    {
        // Disable automatic validation for properties with [BindNever] attribute
        options.DisableDataAnnotationsValidation = false;
        
        // Configure implicit validation for child properties
        options.ImplicitlyValidateChildProperties = true;
    });

    // Add client-side validation adapters for better user experience
    services.AddFluentValidationClientsideAdapters();

    // Register all validators from the current assembly
    services.AddValidatorsFromAssemblyContaining<StudentValidator>();

    // Alternative: Register validators individually for more control
    // services.AddScoped<IValidator<Student>, StudentValidator>();
    // services.AddScoped<IValidator<Grade>, GradeValidator>();
}
```

**Configuration Options Explained:**
1. **`AddFluentValidationAutoValidation()`**: Enables automatic validation integration with MVC model binding
2. **`DisableDataAnnotationsValidation = false`**: Allows both FluentValidation and Data Annotations to work together (useful during migration)
3. **`ImplicitlyValidateChildProperties = true`**: Automatically validates nested objects
4. **`AddFluentValidationClientsideAdapters()`**: Enables client-side validation generation
5. **`AddValidatorsFromAssemblyContaining<T>()`**: Automatically discovers and registers all validators in the assembly

#### Step 8: Alternative Registration Methods
For more control over validator registration, use these approaches:

```csharp
// Method 1: Individual registration
services.AddScoped<IValidator<Student>, StudentValidator>();
services.AddScoped<IValidator<Grade>, GradeValidator>();

// Method 2: Assembly scanning with filtering
services.AddValidatorsFromAssembly(typeof(StudentValidator).Assembly, includeInternalTypes: false);

// Method 3: Multiple assemblies
services.AddValidatorsFromAssemblies(new[] 
{ 
    typeof(StudentValidator).Assembly,
    typeof(SomeOtherValidator).Assembly 
});

// Method 4: With lifetime specification
services.AddValidatorsFromAssemblyContaining<StudentValidator>(ServiceLifetime.Scoped);
```

### Phase 5: Update Controllers and Views

#### Step 9: Update Controller Actions
Your controllers don't need major changes, but you can enhance error handling:

```csharp
// Controllers/StudentController.cs
using Microsoft.AspNetCore.Mvc;
using FluentValidationMVC.Models;
using FluentValidationMVC.Services;
using FluentValidation;

namespace FluentValidationMVC.Controllers
{
    /// <summary>
    /// Student controller with FluentValidation integration
    /// </summary>
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private readonly IValidator<Student> _studentValidator;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public StudentController(StudentService studentService, IValidator<Student> studentValidator)
        {
            _studentService = studentService;
            _studentValidator = studentValidator;
        }

        /// <summary>
        /// Create student - GET action
        /// </summary>
        public IActionResult Create()
        {
            return View(new Student());
        }

        /// <summary>
        /// Create student - POST action with enhanced validation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            // Manual validation (optional - automatic validation happens via model binding)
            var validationResult = await _studentValidator.ValidateAsync(student);
            
            if (!validationResult.IsValid)
            {
                // Add validation errors to ModelState (if not already added automatically)
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                
                return View(student);
            }

            // Alternative: Check ModelState (automatic validation)
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                await _studentService.CreateStudentAsync(student);
                TempData["Success"] = "Student created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating student: " + ex.Message);
                return View(student);
            }
        }

        /// <summary>
        /// Edit student - POST action
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            // ModelState is automatically populated by FluentValidation
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                await _studentService.UpdateStudentAsync(student);
                TempData["Success"] = "Student updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating student: " + ex.Message);
                return View(student);
            }
        }
    }
}
```

#### Step 10: Update Views for Better Validation Display
Enhance your Razor views to work optimally with FluentValidation:

```html
<!-- Views/Student/Create.cshtml -->
@model FluentValidationMVC.Models.Student

@{
    ViewData["Title"] = "Create Student";
}

<div class="container">
    <div class="row">
        <div class="col-md-8">
            <h2>Create New Student</h2>
            
            @* Display validation summary *@
            <div asp-validation-summary="ModelOnly" class="alert alert-danger" role="alert"></div>
            
            <form asp-action="Create" method="post" novalidate>
                <div class="form-group mb-3">
                    <label asp-for="Name" class="form-label"></label>
                    <input asp-for="Name" class="form-control" />
                    <span asp-validation-for="Name" class="text-danger"></span>
                </div>

                <div class="form-group mb-3">
                    <label asp-for="Gender" class="form-label"></label>
                    <select asp-for="Gender" class="form-select">
                        <option value="">-- Select Gender --</option>
                        <option value="Male">Male</option>
                        <option value="Female">Female</option>
                    </select>
                    <span asp-validation-for="Gender" class="text-danger"></span>
                </div>

                <div class="form-group mb-3">
                    <label asp-for="Branch" class="form-label"></label>
                    <input asp-for="Branch" class="form-control" />
                    <span asp-validation-for="Branch" class="text-danger"></span>
                </div>

                <div class="form-group mb-3">
                    <label asp-for="Section" class="form-label"></label>
                    <input asp-for="Section" class="form-control" maxlength="1" />
                    <span asp-validation-for="Section" class="text-danger"></span>
                    <div class="form-text">Enter a single letter (A-Z)</div>
                </div>

                <div class="form-group mb-3">
                    <label asp-for="Email" class="form-label"></label>
                    <input asp-for="Email" class="form-control" type="email" />
                    <span asp-validation-for="Email" class="text-danger"></span>
                    <div class="form-text">Must use university email (@university.edu)</div>
                </div>

                <div class="form-group mb-3">
                    <label asp-for="PhoneNumber" class="form-label"></label>
                    <input asp-for="PhoneNumber" class="form-control" type="tel" />
                    <span asp-validation-for="PhoneNumber" class="text-danger"></span>
                    <div class="form-text">Optional - Enter valid phone number</div>
                </div>

                <div class="form-group mb-3">
                    <label asp-for="EnrollmentDate" class="form-label"></label>
                    <input asp-for="EnrollmentDate" class="form-control" type="date" />
                    <span asp-validation-for="EnrollmentDate" class="text-danger"></span>
                </div>

                <div class="form-group">
                    <button type="submit" class="btn btn-primary">Create Student</button>
                    <a asp-action="Index" class="btn btn-secondary">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</div>

@* Client-side validation scripts *@
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    
    <script>
        // Optional: Custom client-side enhancements
        $(document).ready(function() {
            // Auto-format phone number
            $('#PhoneNumber').on('input', function() {
                // Custom formatting logic if needed
            });
            
            // Auto-uppercase section
            $('#Section').on('input', function() {
                this.value = this.value.toUpperCase();
            });
        });
    </script>
}
```

### Phase 6: Advanced FluentValidation Features

#### Step 11: Implement Conditional Validation
Add complex conditional validation rules:

```csharp
// Advanced StudentValidator with conditional validation
public class AdvancedStudentValidator : AbstractValidator<Student>
{
    public AdvancedStudentValidator()
    {
        // Basic rules
        RuleFor(x => x.Name).NotEmpty().Length(2, 50);
        
        // Conditional validation - phone required for international students
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required for international students.")
            .When(x => IsInternationalStudent(x));
        
        // Different email rules based on student type
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(BeValidStudentEmail)
            .WithMessage("Email must be valid student email format.")
            .When(x => !IsInternationalStudent(x));
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(BeValidInternationalEmail)
            .WithMessage("International students must use their home institution email.")
            .When(x => IsInternationalStudent(x));
    }
    
    private bool IsInternationalStudent(Student student)
    {
        // Example logic: international students have sections starting with 'I'
        return student.Section?.StartsWith("I") == true;
    }
    
    private bool BeValidStudentEmail(string email)
    {
        return email?.EndsWith("@university.edu") == true;
    }
    
    private bool BeValidInternationalEmail(string email)
    {
        // Allow various international domains
        var validDomains = new[] { ".edu", ".ac.uk", ".edu.au", ".edu.in" };
        return validDomains.Any(domain => email?.EndsWith(domain) == true);
    }
}
```

#### Step 12: Create Custom Validation Extensions
Build reusable validation extensions:

```csharp
// Validators/Extensions/ValidationExtensions.cs
using FluentValidation;

namespace FluentValidationMVC.Validators.Extensions
{
    /// <summary>
    /// Custom validation extensions for reusable validation logic
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Validates that a string contains only letters and spaces
        /// </summary>
        public static IRuleBuilderOptions<T, string> OnlyLettersAndSpaces<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("'{PropertyName}' can only contain letters and spaces.");
        }

        /// <summary>
        /// Validates university email format
        /// </summary>
        public static IRuleBuilderOptions<T, string> UniversityEmail<T>(
            this IRuleBuilder<T, string> ruleBuilder, string domain = "@university.edu")
        {
            return ruleBuilder
                .EmailAddress()
                .Must(email => email?.EndsWith(domain, StringComparison.OrdinalIgnoreCase) == true)
                .WithMessage($"Email must end with {domain}");
        }

        /// <summary>
        /// Validates that date is not in the future
        /// </summary>
        public static IRuleBuilderOptions<T, DateTime> NotInFuture<T>(
            this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("'{PropertyName}' cannot be in the future.");
        }

        /// <summary>
        /// Validates positive integer
        /// </summary>
        public static IRuleBuilderOptions<T, int> PositiveInteger<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("'{PropertyName}' must be a positive number.");
        }
    }
}

// Usage in validator
public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(2, 50)
            .OnlyLettersAndSpaces(); // Custom extension
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .UniversityEmail(); // Custom extension
            
        RuleFor(x => x.EnrollmentDate)
            .NotInFuture(); // Custom extension
    }
}
```

#### Step 13: Implement Async Validation
Add asynchronous validation for database checks:

```csharp
// Validators/AsyncStudentValidator.cs
using FluentValidation;
using FluentValidationMVC.Data;
using FluentValidationMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FluentValidationMVC.Validators
{
    /// <summary>
    /// Student validator with async validation for database checks
    /// </summary>
    public class AsyncStudentValidator : AbstractValidator<Student>
    {
        private readonly ApplicationDbContext _context;

        public AsyncStudentValidator(ApplicationDbContext context)
        {
            _context = context;
            
            // Synchronous rules
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 50);
            
            // Asynchronous email uniqueness check
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(BeUniqueEmail)
                .WithMessage("Email address is already in use.")
                .When(x => x.StudentID == 0); // Only for new students
        }

        /// <summary>
        /// Async validation to check email uniqueness
        /// </summary>
        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower(), cancellationToken);

            return existingStudent == null;
        }

        /// <summary>
        /// Async validation for edit scenarios (check email uniqueness excluding current student)
        /// </summary>
        private async Task<bool> BeUniqueEmailForEdit(Student student, string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower() && s.StudentID != student.StudentID, 
                    cancellationToken);

            return existingStudent == null;
        }
    }
}
```

**Register async validator:**
```csharp
// Program.cs
services.AddScoped<IValidator<Student>, AsyncStudentValidator>();
```

### Phase 7: Testing and Validation

#### Step 14: Test Your FluentValidation Implementation

**1. Unit Test Validators:**
```csharp
// Tests/StudentValidatorTests.cs
using FluentValidation.TestHelper;
using FluentValidationMVC.Models;
using FluentValidationMVC.Validators;
using Xunit;

namespace FluentValidationMVC.Tests
{
    public class StudentValidatorTests
    {
        private readonly StudentValidator _validator;

        public StudentValidatorTests()
        {
            _validator = new StudentValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var student = new Student { Name = "" };

            // Act & Assert
            var result = _validator.TestValidate(student);
            result.ShouldHaveValidationErrorFor(s => s.Name)
                  .WithErrorMessage("Student name is required.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_Is_Valid()
        {
            // Arrange
            var student = new Student { Name = "John Doe" };

            // Act & Assert
            var result = _validator.TestValidate(student);
            result.ShouldNotHaveValidationErrorFor(s => s.Name);
        }

        [Theory]
        [InlineData("test@university.edu")]
        [InlineData("student@university.edu")]
        public void Should_Not_Have_Error_When_Email_Is_University_Domain(string email)
        {
            // Arrange
            var student = new Student { Email = email };

            // Act & Assert
            var result = _validator.TestValidate(student);
            result.ShouldNotHaveValidationErrorFor(s => s.Email);
        }

        [Theory]
        [InlineData("test@gmail.com")]
        [InlineData("student@yahoo.com")]
        public void Should_Have_Error_When_Email_Is_Not_University_Domain(string email)
        {
            // Arrange
            var student = new Student { Email = email };

            // Act & Assert
            var result = _validator.TestValidate(student);
            result.ShouldHaveValidationErrorFor(s => s.Email)
                  .WithErrorMessage("Email must be a university domain (@university.edu).");
        }
    }
}
```

**2. Integration Testing:**
```bash
# Run the application
dotnet run

# Test scenarios:
# 1. Submit empty form - should show validation errors
# 2. Enter invalid email - should show university domain error
# 3. Enter invalid grade range - should show range error
# 4. Test client-side validation (errors before form submission)
```

#### Step 15: Performance Optimization and Best Practices

**1. Validator Caching:**
```csharp
// For better performance, validators are automatically cached by DI container
// But you can implement custom caching if needed
services.AddSingleton<IValidator<Student>, StudentValidator>();
```

**2. Conditional Validator Loading:**
```csharp
// Only register certain validators based on configuration
if (builder.Configuration.GetValue<bool>("Features:AdvancedValidation"))
{
    services.AddScoped<IValidator<Student>, AdvancedStudentValidator>();
}
else
{
    services.AddScoped<IValidator<Student>, StudentValidator>();
}
```

**3. Memory Management:**
```csharp
// For async validators, ensure proper disposal
public class AsyncStudentValidator : AbstractValidator<Student>, IDisposable
{
    private readonly ApplicationDbContext _context;
    
    public AsyncStudentValidator(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}
```

### Phase 8: Migration Strategy

#### Step 16: Gradual Migration from Data Annotations

**1. Hybrid Approach (During Migration):**
```csharp
// Program.cs - Allow both validation systems temporarily
services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = false; // Keep both systems
});
```

**2. Model-by-Model Migration:**
```csharp
// Start with one model at a time
// Week 1: Migrate Student model
// Week 2: Migrate Grade model
// Week 3: Remove Data Annotations
```

**3. Testing Strategy:**
```csharp
// Create comprehensive tests for each migrated validator
// Compare validation results between old and new systems
// Ensure no regression in validation behavior
```

### Common Pitfalls and Solutions

#### Pitfall 1: Client-Side Validation Not Working
**Problem**: Validation messages don't appear on the client side

**Solution**:
```html
<!-- Ensure validation scripts are included -->
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}

<!-- Use proper validation attributes in views -->
<span asp-validation-for="PropertyName" class="text-danger"></span>
```

#### Pitfall 2: Validators Not Being Discovered
**Problem**: Validators are not automatically registered

**Solution**:
```csharp
// Check assembly reference in Program.cs
services.AddValidatorsFromAssemblyContaining<StudentValidator>();

// Or register manually
services.AddScoped<IValidator<Student>, StudentValidator>();
```
