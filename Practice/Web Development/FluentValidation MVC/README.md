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

## Advanced FluentValidation Features

### 1. **Conditional Validation**
```csharp
RuleFor(x => x.PhoneNumber)
    .NotEmpty()
    .When(x => x.IsContactRequired)
    .WithMessage("Phone number is required for this student type.");
```

### 2. **Custom Validation Methods**
```csharp
RuleFor(x => x.Gender)
    .Must(BeValidGender)
    .WithMessage("Gender must be either 'Male' or 'Female'.");

private bool BeValidGender(string gender)
{
    return gender == "Male" || gender == "Female";
}
```

### 3. **Cross-Field Validation**
```csharp
RuleFor(x => x.LetterGrade)
    .Must((grade, letterGrade) => HaveMatchingLetterGrade(grade.GradeValue, letterGrade))
    .WithMessage("Letter grade must match the numeric grade value.");
```

### 4. **Async Validation**
```csharp
RuleFor(x => x.Email)
    .MustAsync(async (email, cancellation) => 
    {
        return await IsEmailUniqueAsync(email);
    })
    .WithMessage("Email address must be unique.");
```

## Running the Project

### Prerequisites
- .NET 8.0 SDK
- Visual Studio Code or Visual Studio

### Setup Steps

1. **Clone/Navigate to the project directory**
   ```bash
   cd "FluentValidation MVC"
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Update database**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open in browser**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

## Testing FluentValidation

### 1. **Create a New Student**
- Try submitting empty forms to see validation messages
- Enter invalid email formats
- Test the university domain restriction
- Try invalid gender values

### 2. **Grade Management**
- Add grades with mismatched letter grades and numeric values
- Test future dates
- Try invalid grade ranges

### 3. **Client-Side Validation**
- Notice how validation messages appear without form submission
- FluentValidation automatically generates client-side validation

## Comparison: Before and After

### Before (Data Annotations)
```csharp
public class Student
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    [RegularExpression(@".*@university\.edu$", 
        ErrorMessage = "Must be university email")]
    public string Email { get; set; }
}
```

**Issues:**
- Validation logic mixed with model
- Limited conditional validation
- Hard to unit test
- Complex rules are difficult to express

### After (FluentValidation)
```csharp
// Clean Model
public class Student
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// Separate Validator
public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 50);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(email => email.EndsWith("@university.edu"))
            .WithMessage("Email must be a university domain.");
    }
}
```

**Benefits:**
- Clean separation of concerns
- Easy to unit test
- Powerful conditional logic
- Reusable validation rules
- Better error message control

## Validation Features Demonstrated

### StudentValidator Features
```csharp
public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        // Required fields with custom messages
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Student name is required.")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces.");

        // Custom validation methods
        RuleFor(x => x.Gender)
            .Must(BeValidGender).WithMessage("Gender must be either 'Male' or 'Female'.");

        // Complex email validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.")
            .Must(email => email.EndsWith("@university.edu"))
            .WithMessage("Email must be a university domain (@university.edu).");

        // Conditional validation
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[\d\s\-\(\)]+$")
            .WithMessage("Please enter a valid phone number format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        // Date validation
        RuleFor(x => x.EnrollmentDate)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Enrollment date cannot be in the future.");
    }

    private bool BeValidGender(string gender)
    {
        return gender == "Male" || gender == "Female";
    }
}
```

### GradeValidator Features
```csharp
public class GradeValidator : AbstractValidator<Grade>
{
    public GradeValidator()
    {
        // Numeric range validation
        RuleFor(x => x.GradeValue)
            .InclusiveBetween(0, 100)
            .WithMessage("Grade must be between 0 and 100.");

        // Cross-field validation
        RuleFor(x => x.LetterGrade)
            .Must((grade, letterGrade) => HaveMatchingLetterGrade(grade.GradeValue, letterGrade))
            .WithMessage("Letter grade must match the numeric grade value.")
            .When(x => !string.IsNullOrEmpty(x.LetterGrade));

        // Conditional length validation
        RuleFor(x => x.Comments)
            .MaximumLength(500)
            .WithMessage("Comments cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comments));
    }

    private bool HaveMatchingLetterGrade(decimal numericGrade, string letterGrade)
    {
        return letterGrade switch
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
```

## Key Takeaways

1. **FluentValidation provides better separation of concerns** by moving validation logic out of your models
2. **Complex validation scenarios** are much easier to implement and maintain
3. **Unit testing** becomes straightforward with dedicated validator classes
4. **Error messages** can be more descriptive and context-aware
5. **Conditional validation** is more powerful and flexible
6. **Client-side validation** is automatically generated from your server-side rules

## Learn More

- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [ASP.NET Core Integration](https://docs.fluentvalidation.net/en/latest/aspnet.html)
- [Advanced Validation Scenarios](https://docs.fluentvalidation.net/en/latest/advanced.html)

This project serves as a comprehensive example of how to implement FluentValidation in ASP.NET Core MVC, demonstrating its advantages over traditional Data Annotations for complex validation scenarios.
