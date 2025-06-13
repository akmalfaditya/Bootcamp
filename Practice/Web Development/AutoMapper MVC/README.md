# AutoMapper MVC Demo Project

## Project Overview

This project demonstrates the implementation and benefits of AutoMapper in an ASP.NET Core MVC application. AutoMapper is a powerful object-to-object mapping library that eliminates the need for manual mapping between entities and DTOs (Data Transfer Objects).

## Key Concepts Demonstrated

### 1. Models vs DTOs
- **Models (Entities)**: Represent the database structure with all internal fields
- **DTOs**: Simplified objects designed for data transfer, exposing only necessary fields

### 2. AutoMapper Benefits
- Reduces code duplication
- Improves maintainability
- Increases development productivity
- Provides consistent mapping across the application
- Enhances security by controlling data exposure

## Project Structure

```
AutoMapper MVC/
├── Controllers/
│   ├── HomeController.cs
│   └── StudentController.cs          # Updated to work with DTOs
├── Data/
│   └── ApplicationDbContext.cs       # Entity Framework context
├── DTOs/                            # Data Transfer Objects
│   ├── StudentDTO.cs                # For displaying student data
│   ├── StudentCreateDTO.cs          # For creating new students
│   ├── GradeDTO.cs                  # For displaying grade data
│   └── GradeCreateDTO.cs            # For creating new grades
├── MappingProfiles/                 # AutoMapper configuration
│   ├── StudentMappingProfile.cs     # Student entity mappings
│   └── GradeMappingProfile.cs       # Grade entity mappings
├── Models/                          # Entity models
│   ├── Student.cs
│   ├── Grade.cs
│   └── ErrorViewModel.cs
├── Services/
│   └── StudentService.cs            # Business logic with AutoMapper
├── Views/                           # Updated to work with DTOs
│   └── Student/
└── Program.cs                       # AutoMapper registration
```

## Setting Up AutoMapper from Scratch

### Step 1: Install AutoMapper Package

```bash
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

### Step 2: Create DTOs

Create DTOs that represent the data structure you want to expose to clients:

```csharp
// StudentDTO.cs - For displaying student information
public class StudentDTO
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Department { get; set; }
    // ... other properties
}

// StudentCreateDTO.cs - For creating new students
public class StudentCreateDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Branch { get; set; }
    // ... validation attributes and properties
}
```

### Step 3: Create Mapping Profiles

AutoMapper uses profiles to define mapping rules:

```csharp
public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        // Map Student entity to StudentDTO
        CreateMap<Student, StudentDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Branch));

        // Map StudentCreateDTO to Student entity
        CreateMap<StudentCreateDTO, Student>()
            .ForMember(dest => dest.StudentID, opt => opt.Ignore());
    }
}
```

### Step 4: Register AutoMapper in Program.cs

```csharp
// Add AutoMapper with all profiles in the assembly
builder.Services.AddAutoMapper(typeof(Program).Assembly);
```

### Step 5: Update Services to Use AutoMapper

```csharp
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
        var students = await _context.Students.ToListAsync();
        return _mapper.Map<IEnumerable<StudentDTO>>(students);
    }
}
```

### Step 6: Update Controllers to Work with DTOs

```csharp
[HttpPost]
public async Task<IActionResult> Create(StudentCreateDTO createDto)
{
    if (ModelState.IsValid)
    {
        var studentDto = await _studentService.CreateStudentAsync(createDto);
        return RedirectToAction(nameof(Index));
    }
    return View(createDto);
}
```

### Step 7: Update Views to Use DTOs

```html
@model AutoMapperMVC.DTOs.StudentDTO

<h1>@Model.FullName</h1>
<p>Department: @Model.Department</p>
```

## Key AutoMapper Features Demonstrated

### 1. Basic Mapping
```csharp
CreateMap<Student, StudentDTO>();
```

### 2. Property Name Mapping
```csharp
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
```

### 3. Ignoring Properties
```csharp
CreateMap<StudentCreateDTO, Student>()
    .ForMember(dest => dest.StudentID, opt => opt.Ignore());
```

### 4. Custom Logic During Mapping
```csharp
CreateMap<GradeCreateDTO, Grade>()
    .AfterMap((src, dest) =>
    {
        if (string.IsNullOrEmpty(dest.LetterGrade))
        {
            dest.LetterGrade = dest.CalculateLetterGrade();
        }
    });
```

### 5. Collection Mapping
```csharp
var studentDTOs = _mapper.Map<IEnumerable<StudentDTO>>(students);
```

## Building and Running the Project

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Steps to Run

1. **Clone/Navigate to the project directory**
   ```bash
   cd "AutoMapper MVC"
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Build the project**
   ```bash
   dotnet build
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access the application**
   - Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

## Database Migrations

The project uses Entity Framework Core with SQLite. The database will be created automatically when you run the application for the first time.

### To add new migrations:
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Testing the AutoMapper Implementation

### 1. Create a Student
- Navigate to `/Student/Create`
- Fill in the form using the `StudentCreateDTO`
- Notice how only necessary fields are exposed

### 2. View Student List
- Navigate to `/Student`
- Observe how data is displayed using `StudentDTO` properties
- Search functionality works with mapped properties

### 3. View Student Details
- Click on any student to see details
- Notice how related grades are automatically mapped

### 4. Add Grades
- From student details, add grades using `GradeCreateDTO`
- See how AutoMapper handles the conversion and includes business logic

## Benefits Observed in This Implementation

### Before AutoMapper (Manual Mapping)
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

### After AutoMapper
```csharp
var studentDTO = _mapper.Map<StudentDTO>(student);
```

### Key Improvements
1. **Reduced Code**: 90% less mapping code
2. **Better Maintainability**: Changes to mappings are centralized
3. **Fewer Errors**: Automatic mapping reduces human error
4. **Enhanced Security**: DTOs prevent over-posting attacks
5. **Cleaner Architecture**: Clear separation between entities and data transfer

## Advanced AutoMapper Scenarios

### 1. Conditional Mapping
```csharp
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => 
        string.IsNullOrEmpty(src.Name) ? "Unknown" : src.Name));
```

### 2. Nested Object Mapping
```csharp
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.Grades));
```

### 3. Custom Value Resolvers
```csharp
CreateMap<Grade, GradeDTO>()
    .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => 
        src.Student != null ? src.Student.Name : "Unknown"));
```

## Troubleshooting

### Common Issues and Solutions

1. **Missing Mapping Configuration**
   - Error: "Missing type map configuration"
   - Solution: Ensure you have created a mapping profile for the types

2. **Null Reference Exceptions**
   - Error: Object reference not set to an instance
   - Solution: Handle null values in mapping profiles

3. **Property Name Mismatches**
   - Error: Properties not mapping correctly
   - Solution: Use `ForMember` to explicitly map properties

### Debugging AutoMapper
```csharp
// In development environment, validate mappings
var config = new MapperConfiguration(cfg => {
    cfg.AddProfile<StudentMappingProfile>();
});
config.AssertConfigurationIsValid();
```

## Performance Considerations

### Best Practices
1. **Use Projection for Queries**
   ```csharp
   var studentDTOs = await _context.Students
       .ProjectTo<StudentDTO>(_mapper.ConfigurationProvider)
       .ToListAsync();
   ```

2. **Avoid N+1 Queries**
   ```csharp
   var students = await _context.Students
       .Include(s => s.Grades)
       .ToListAsync();
   ```

3. **Use Compile-Time Checks**
   ```csharp
   CreateMap<Student, StudentDTO>()
       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
       .ValidateMemberList(MemberList.Destination);
   ```

## Conclusion

This project demonstrates how AutoMapper significantly improves the development experience in ASP.NET Core MVC applications. By providing automatic object-to-object mapping, it reduces boilerplate code, improves maintainability, and enhances application architecture.

The implementation shows practical use cases including:
- Entity to DTO mapping for data display
- DTO to entity mapping for data creation
- Complex mapping scenarios with custom logic
- Integration with Entity Framework and MVC patterns

AutoMapper proves to be an essential tool for modern web application development, providing both immediate productivity gains and long-term maintainability benefits.

## Next Steps

To further enhance this implementation, consider:
1. Adding validation attributes to DTOs
2. Implementing API endpoints alongside MVC views
3. Adding unit tests for mapping configurations
4. Exploring AutoMapper's advanced features like custom type converters
5. Implementing caching strategies for frequently mapped objects

## Resources

- [AutoMapper Documentation](https://docs.automapper.org/)
- [ASP.NET Core MVC Documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

## Step-by-Step Implementation Guide

This comprehensive guide will walk you through implementing AutoMapper in an ASP.NET Core MVC application from scratch. Follow these detailed steps to transform your application from manual object mapping to automated, maintainable mapping using AutoMapper.

### Phase 1: Understanding AutoMapper Concepts and Setup

#### Step 1: Install AutoMapper Packages
AutoMapper requires specific NuGet packages for ASP.NET Core integration:

```bash
# Install the main AutoMapper package with DI extensions
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

# Alternative: Install packages individually for more control
dotnet add package AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

**Package Explanation:**
- `AutoMapper`: Core mapping engine and functionality
- `AutoMapper.Extensions.Microsoft.DependencyInjection`: Provides seamless integration with ASP.NET Core's dependency injection container
- Optional: `AutoMapper.Collection` for advanced collection mapping scenarios

#### Step 2: Analyze Your Current Models
Before implementing AutoMapper, understand what you're mapping between:

**Example Domain Model (Entity):**
```csharp
// Models/Student.cs - Database entity with all fields
public class Student
{
    public int StudentID { get; set; }                    // Primary key
    public string Name { get; set; } = string.Empty;     // Full name
    public string Gender { get; set; } = string.Empty;   // Male/Female
    public string Branch { get; set; } = string.Empty;   // Department
    public string Section { get; set; } = string.Empty;  // Class section
    public string Email { get; set; } = string.Empty;    // Email address
    public string? PhoneNumber { get; set; }             // Optional phone
    public DateTime EnrollmentDate { get; set; }         // Enrollment date
    public DateTime CreatedAt { get; set; }              // Internal tracking
    public DateTime UpdatedAt { get; set; }              // Internal tracking
    public string? InternalNotes { get; set; }           // Admin only field
    
    // Navigation properties
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
```

**Problems with Direct Entity Usage:**
- Exposes internal fields (`CreatedAt`, `UpdatedAt`, `InternalNotes`)
- Security risk (over-posting attacks)
- Tightly couples UI to database structure
- Difficult to maintain when schema changes

#### Step 3: Create Project Structure for AutoMapper
Organize your AutoMapper-related code in a logical structure:

```bash
# Create DTOs folder structure
mkdir DTOs
mkdir DTOs/Student
mkdir DTOs/Grade
mkdir DTOs/Common

# Create MappingProfiles folder
mkdir MappingProfiles

# Optional: Create ViewModels folder for complex UI scenarios
mkdir ViewModels
```

**Recommended Folder Structure:**
```
AutoMapperMVC/
├── DTOs/                           # Data Transfer Objects
│   ├── Common/                     # Shared DTOs
│   │   ├── BaseDTO.cs             # Common properties
│   │   └── PaginationDTO.cs       # Pagination support
│   ├── Student/
│   │   ├── StudentDTO.cs          # Display/Read operations
│   │   ├── StudentCreateDTO.cs    # Create operations
│   │   ├── StudentUpdateDTO.cs    # Update operations
│   │   └── StudentListItemDTO.cs  # List view optimization
│   └── Grade/
│       ├── GradeDTO.cs
│       ├── GradeCreateDTO.cs
│       └── GradeUpdateDTO.cs
├── MappingProfiles/                # AutoMapper configuration
│   ├── StudentMappingProfile.cs
│   ├── GradeMappingProfile.cs
│   └── CommonMappingProfile.cs
└── ViewModels/                     # Complex UI models (optional)
    ├── StudentDetailsViewModel.cs
    └── DashboardViewModel.cs
```

### Phase 2: Create Data Transfer Objects (DTOs)

#### Step 4: Design Your DTOs
Create DTOs that represent exactly what data should be transferred for each operation:

```csharp
// DTOs/Student/StudentDTO.cs - For displaying student information
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs.Student
{
    /// <summary>
    /// DTO for displaying student information to clients
    /// Contains only fields that should be exposed publicly
    /// </summary>
    public class StudentDTO
    {
        /// <summary>
        /// Student identifier for client use
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Student's display name
        /// </summary>
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Academic department
        /// </summary>
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Class section
        /// </summary>
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Contact email address
        /// </summary>
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional phone number
        /// </summary>
        [Display(Name = "Phone")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Formatted enrollment date
        /// </summary>
        [Display(Name = "Enrolled On")]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Student's grades (for details view)
        /// </summary>
        public List<GradeDTO> Grades { get; set; } = new();

        /// <summary>
        /// Calculated average grade
        /// </summary>
        [Display(Name = "Average Grade")]
        public decimal? AverageGrade { get; set; }
    }
}
```

```csharp
// DTOs/Student/StudentCreateDTO.cs - For creating new students
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs.Student
{
    /// <summary>
    /// DTO for creating new students
    /// Contains validation rules and only required fields for creation
    /// </summary>
    public class StudentCreateDTO
    {
        /// <summary>
        /// Student's full name
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender
        /// </summary>
        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be Male or Female")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic branch/department
        /// </summary>
        [Required(ErrorMessage = "Branch is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Branch must be between 2 and 100 characters")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Class section
        /// </summary>
        [Required(ErrorMessage = "Section is required")]
        [RegularExpression("^[A-Z]$", ErrorMessage = "Section must be a single letter A-Z")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Email address
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional phone number
        /// </summary>
        [Phone(ErrorMessage = "Invalid phone number format")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Enrollment date (defaults to today)
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Today;
    }
}
```

```csharp
// DTOs/Student/StudentUpdateDTO.cs - For updating existing students
using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs.Student
{
    /// <summary>
    /// DTO for updating existing students
    /// Includes ID and allows modification of specific fields
    /// </summary>
    public class StudentUpdateDTO
    {
        /// <summary>
        /// Student ID (required for updates)
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Updated name
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Updated branch
        /// </summary>
        [Required(ErrorMessage = "Branch is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Updated section
        /// </summary>
        [Required(ErrorMessage = "Section is required")]
        [RegularExpression("^[A-Z]$")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Updated email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Updated phone number
        /// </summary>
        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        // Note: Gender and EnrollmentDate typically shouldn't be editable
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
    /// </summary>
    public class GradeDTO
    {
        public int Id { get; set; }

        [Display(Name = "Student")]
        public string StudentName { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        [Display(Name = "Grade")]
        public decimal GradeValue { get; set; }

        [Display(Name = "Letter Grade")]
        public string? LetterGrade { get; set; }

        [Display(Name = "Date")]
        public DateTime GradeDate { get; set; }

        public string? Comments { get; set; }
    }
}
```

#### Step 5: Create Optimized DTOs for Different Scenarios
Create specialized DTOs for specific use cases:

```csharp
// DTOs/Student/StudentListItemDTO.cs - Optimized for list displays
namespace AutoMapperMVC.DTOs.Student
{
    /// <summary>
    /// Lightweight DTO for student list displays
    /// Contains only essential information for performance
    /// </summary>
    public class StudentListItemDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        
        /// <summary>
        /// Count of grades for this student
        /// </summary>
        public int GradeCount { get; set; }
        
        /// <summary>
        /// Average grade if available
        /// </summary>
        public decimal? AverageGrade { get; set; }
    }
}
```

### Phase 3: Create AutoMapper Profiles

#### Step 6: Create Basic Mapping Profiles
AutoMapper uses profiles to organize and configure mappings:

```csharp
// MappingProfiles/StudentMappingProfile.cs
using AutoMapper;
using AutoMapperMVC.DTOs.Student;
using AutoMapperMVC.Models;

namespace AutoMapperMVC.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for Student entity mappings
    /// Defines how Student entities map to/from various DTOs
    /// </summary>
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            // Configure all student-related mappings
            CreateStudentToDisplayMappings();
            CreateStudentToCreateMappings();
            CreateStudentToUpdateMappings();
            CreateStudentToListMappings();
        }

        /// <summary>
        /// Configure mappings for displaying student data
        /// </summary>
        private void CreateStudentToDisplayMappings()
        {
            // Entity to DTO mapping for display
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Branch))
                .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.Grades))
                .ForMember(dest => dest.AverageGrade, opt => opt.MapFrom(src => 
                    src.Grades.Any() ? src.Grades.Average(g => g.GradeValue) : (decimal?)null))
                .ReverseMap() // Allows mapping back from DTO to Entity
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InternalNotes, opt => opt.Ignore());
        }

        /// <summary>
        /// Configure mappings for creating students
        /// </summary>
        private void CreateStudentToCreateMappings()
        {
            // CreateDTO to Entity mapping
            CreateMap<StudentCreateDTO, Student>()
                .ForMember(dest => dest.StudentID, opt => opt.Ignore()) // Auto-generated
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.InternalNotes, opt => opt.Ignore())
                .ForMember(dest => dest.Grades, opt => opt.Ignore()); // Will be added separately

            // Entity to CreateDTO mapping (for edit forms)
            CreateMap<Student, StudentCreateDTO>()
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrollmentDate.Date));
        }

        /// <summary>
        /// Configure mappings for updating students
        /// </summary>
        private void CreateStudentToUpdateMappings()
        {
            // UpdateDTO to Entity mapping
            CreateMap<StudentUpdateDTO, Student>()
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Gender, opt => opt.Ignore()) // Don't update gender
                .ForMember(dest => dest.EnrollmentDate, opt => opt.Ignore()) // Don't update enrollment date
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Preserve creation date
                .ForMember(dest => dest.InternalNotes, opt => opt.Ignore()) // Preserve internal notes
                .ForMember(dest => dest.Grades, opt => opt.Ignore()); // Handle separately

            // Entity to UpdateDTO mapping (for populating edit forms)
            CreateMap<Student, StudentUpdateDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID));
        }

        /// <summary>
        /// Configure mappings for list displays
        /// </summary>
        private void CreateStudentToListMappings()
        {
            // Entity to ListItemDTO mapping (optimized for lists)
            CreateMap<Student, StudentListItemDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Branch))
                .ForMember(dest => dest.GradeCount, opt => opt.MapFrom(src => src.Grades.Count))
                .ForMember(dest => dest.AverageGrade, opt => opt.MapFrom(src => 
                    src.Grades.Any() ? src.Grades.Average(g => g.GradeValue) : (decimal?)null));
        }
    }
}
```

#### Step 7: Create Grade Mapping Profile with Complex Logic
Create mappings for grades with business logic:

```csharp
// MappingProfiles/GradeMappingProfile.cs
using AutoMapper;
using AutoMapperMVC.DTOs.Grade;
using AutoMapperMVC.Models;

namespace AutoMapperMVC.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for Grade entity mappings
    /// Demonstrates complex mapping scenarios and business logic
    /// </summary>
    public class GradeMappingProfile : Profile
    {
        public GradeMappingProfile()
        {
            CreateGradeDisplayMappings();
            CreateGradeCreateMappings();
        }

        /// <summary>
        /// Configure mappings for displaying grades
        /// </summary>
        private void CreateGradeDisplayMappings()
        {
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
        }

        /// <summary>
        /// Configure mappings for creating grades
        /// </summary>
        private void CreateGradeCreateMappings()
        {
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

### Phase 4: Configure AutoMapper in Your Application

#### Step 8: Register AutoMapper in Program.cs
Configure AutoMapper services in your dependency injection container:

```csharp
// Program.cs
using AutoMapper;
using AutoMapperMVC.Data;
using AutoMapperMVC.MappingProfiles;
using AutoMapperMVC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MVC services
builder.Services.AddControllersWithViews();

// Configure AutoMapper
ConfigureAutoMapper(builder.Services);

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
/// Configure AutoMapper services and validation
/// </summary>
/// <param name="services">Service collection</param>
static void ConfigureAutoMapper(IServiceCollection services)
{
    // Method 1: Automatic discovery of all profiles in assembly
    services.AddAutoMapper(typeof(Program).Assembly);

    // Method 2: Explicit profile registration (for more control)
    // services.AddAutoMapper(cfg =>
    // {
    //     cfg.AddProfile<StudentMappingProfile>();
    //     cfg.AddProfile<GradeMappingProfile>();
    // });

    // Method 3: Manual configuration with validation
    // services.AddSingleton(provider =>
    // {
    //     var configuration = new MapperConfiguration(cfg =>
    //     {
    //         cfg.AddProfile<StudentMappingProfile>();
    //         cfg.AddProfile<GradeMappingProfile>();
    //         
    //         // Add validation in development
    //         if (provider.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
    //         {
    //             cfg.AssertConfigurationIsValid();
    //         }
    //     });
    //     
    //     return configuration.CreateMapper();
    // });
}
```

#### Step 9: Alternative Configuration Methods
For more advanced scenarios, use these configuration approaches:

```csharp
// Advanced AutoMapper configuration
public static void ConfigureAdvancedAutoMapper(IServiceCollection services, IConfiguration configuration)
{
    // Configuration with custom settings
    services.AddAutoMapper(cfg =>
    {
        // Add all profiles from assembly
        cfg.AddMaps(typeof(Program).Assembly);
        
        // Global configuration
        cfg.ForAllMaps((typeMap, mappingExpression) =>
        {
            // Ignore all properties ending with "Internal"
            mappingExpression.ForAllMembers(opt =>
            {
                if (opt.DestinationMember.Name.EndsWith("Internal"))
                {
                    opt.Ignore();
                }
            });
        });
        
        // Custom naming conventions
        cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
        cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
        
        // Performance settings
        cfg.DisableConstructorMapping();
        cfg.ShouldMapProperty = propertyInfo => propertyInfo.GetMethod?.IsPublic == true;
        
        // Environment-specific settings
        if (configuration.GetValue<bool>("AutoMapper:ValidateConfiguration"))
        {
            cfg.AssertConfigurationIsValid();
        }
    });
}
```

### Phase 5: Update Services to Use AutoMapper

#### Step 10: Refactor Service Layer
Update your services to use AutoMapper for all object transformations:

```csharp
// Services/StudentService.cs
using AutoMapper;
using AutoMapperMVC.Data;
using AutoMapperMVC.DTOs.Student;
using AutoMapperMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperMVC.Services
{
    /// <summary>
    /// Student service that uses AutoMapper for all entity-DTO conversions
    /// Demonstrates clean separation between domain entities and data transfer
    /// </summary>
    public class StudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public StudentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all students as DTOs for list display
        /// Uses projection for optimal database queries
        /// </summary>
        public async Task<IEnumerable<StudentListItemDTO>> GetAllStudentsAsync()
        {
            // Method 1: Load entities then map (simple but less efficient)
            var students = await _context.Students
                .Include(s => s.Grades)
                .ToListAsync();
            
            return _mapper.Map<IEnumerable<StudentListItemDTO>>(students);

            // Method 2: Project directly to DTO (more efficient for large datasets)
            // return await _context.Students
            //     .ProjectTo<StudentListItemDTO>(_mapper.ConfigurationProvider)
            //     .ToListAsync();
        }

        /// <summary>
        /// Get student by ID with full details
        /// </summary>
        public async Task<StudentDTO?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
                return null;

            return _mapper.Map<StudentDTO>(student);
        }

        /// <summary>
        /// Create a new student from DTO
        /// Demonstrates DTO to entity mapping
        /// </summary>
        public async Task<StudentDTO> CreateStudentAsync(StudentCreateDTO createDto)
        {
            // Validate DTO (can be done with FluentValidation)
            if (string.IsNullOrWhiteSpace(createDto.Name))
                throw new ArgumentException("Student name is required");

            // Check for duplicate email
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == createDto.Email);
            
            if (existingStudent != null)
                throw new InvalidOperationException("A student with this email already exists");

            // Map DTO to entity
            var student = _mapper.Map<Student>(createDto);
            
            // Add business logic
            student.CreatedAt = DateTime.UtcNow;
            student.UpdatedAt = DateTime.UtcNow;

            // Save to database
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Return as DTO
            return _mapper.Map<StudentDTO>(student);
        }

        /// <summary>
        /// Update existing student
        /// </summary>
        public async Task<StudentDTO> UpdateStudentAsync(StudentUpdateDTO updateDto)
        {
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentID == updateDto.Id);

            if (existingStudent == null)
                throw new ArgumentException("Student not found");

            // Map DTO changes to existing entity
            _mapper.Map(updateDto, existingStudent);
            
            // Update timestamp
            existingStudent.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Return updated entity as DTO
            return _mapper.Map<StudentDTO>(existingStudent);
        }

        /// <summary>
        /// Delete student
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// Search students by criteria
        /// Demonstrates complex querying with DTO projection
        /// </summary>
        public async Task<IEnumerable<StudentListItemDTO>> SearchStudentsAsync(
            string? searchTerm = null, 
            string? department = null)
        {
            var query = _context.Students.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Name.Contains(searchTerm) || 
                                        s.Email.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(s => s.Branch == department);
            }

            // Include grades for calculation
            var students = await query
                .Include(s => s.Grades)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentListItemDTO>>(students);
        }

        /// <summary>
        /// Get student for editing (returns UpdateDTO)
        /// </summary>
        public async Task<StudentUpdateDTO?> GetStudentForEditAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            
            if (student == null)
                return null;

            return _mapper.Map<StudentUpdateDTO>(student);
        }

        /// <summary>
        /// Get students by department with statistics
        /// Demonstrates complex DTO mapping with calculated fields
        /// </summary>
        public async Task<IEnumerable<StudentDTO>> GetStudentsByDepartmentAsync(string department)
        {
            var students = await _context.Students
                .Where(s => s.Branch == department)
                .Include(s => s.Grades)
                .ToListAsync();

            var studentDtos = _mapper.Map<IEnumerable<StudentDTO>>(students);

            // Additional processing after mapping if needed
            foreach (var dto in studentDtos)
            {
                // Calculate additional statistics or formatting
                if (dto.Grades.Any())
                {
                    dto.AverageGrade = Math.Round(dto.AverageGrade ?? 0, 2);
                }
            }

            return studentDtos;
        }
    }
}
```

### Phase 6: Update Controllers to Work with DTOs

#### Step 11: Refactor Controllers for DTO Usage
Update your controllers to work exclusively with DTOs:

```csharp
// Controllers/StudentController.cs
using AutoMapperMVC.DTOs.Student;
using AutoMapperMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoMapperMVC.Controllers
{
    /// <summary>
    /// Student controller that works exclusively with DTOs
    /// Demonstrates clean separation between presentation and domain layers
    /// </summary>
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Display list of students
        /// </summary>
        public async Task<IActionResult> Index(string? searchTerm, string? department)
        {
            try
            {
                IEnumerable<StudentListItemDTO> students;

                if (!string.IsNullOrWhiteSpace(searchTerm) || !string.IsNullOrWhiteSpace(department))
                {
                    students = await _studentService.SearchStudentsAsync(searchTerm, department);
                }
                else
                {
                    students = await _studentService.GetAllStudentsAsync();
                }

                // Pass search parameters to view for form retention
                ViewBag.SearchTerm = searchTerm;
                ViewBag.Department = department;

                return View(students);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading students: " + ex.Message;
                return View(new List<StudentListItemDTO>());
            }
        }

        /// <summary>
        /// Display student details
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                
                if (student == null)
                {
                    TempData["Error"] = "Student not found";
                    return RedirectToAction(nameof(Index));
                }

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading student details: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Show create student form
        /// </summary>
        public IActionResult Create()
        {
            return View(new StudentCreateDTO());
        }

        /// <summary>
        /// Handle student creation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateDTO createDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createDto);
            }

            try
            {
                var createdStudent = await _studentService.CreateStudentAsync(createDto);
                TempData["Success"] = $"Student '{createdStudent.FullName}' created successfully!";
                return RedirectToAction(nameof(Details), new { id = createdStudent.Id });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(createDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating student: " + ex.Message);
                return View(createDto);
            }
        }

        /// <summary>
        /// Show edit student form
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var student = await _studentService.GetStudentForEditAsync(id);
                
                if (student == null)
                {
                    TempData["Error"] = "Student not found";
                    return RedirectToAction(nameof(Index));
                }

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading student for edit: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handle student update
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentUpdateDTO updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return View(updateDto);
            }

            try
            {
                var updatedStudent = await _studentService.UpdateStudentAsync(updateDto);
                TempData["Success"] = $"Student '{updatedStudent.FullName}' updated successfully!";
                return RedirectToAction(nameof(Details), new { id = updatedStudent.Id });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(updateDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating student: " + ex.Message);
                return View(updateDto);
            }
        }

        /// <summary>
        /// Show delete confirmation
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                
                if (student == null)
                {
                    TempData["Error"] = "Student not found";
                    return RedirectToAction(nameof(Index));
                }

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading student: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handle student deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var deleted = await _studentService.DeleteStudentAsync(id);
                
                if (deleted)
                {
                    TempData["Success"] = "Student deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Student not found";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting student: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// API endpoint that returns JSON DTOs
        /// Demonstrates AutoMapper in API scenarios
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStudentsJson(string? department = null)
        {
            try
            {
                IEnumerable<StudentListItemDTO> students;

                if (!string.IsNullOrWhiteSpace(department))
                {
                    students = await _studentService.SearchStudentsAsync(null, department);
                }
                else
                {
                    students = await _studentService.GetAllStudentsAsync();
                }

                return Json(students);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
```

### Phase 7: Update Views to Work with DTOs

#### Step 12: Update Razor Views
Modify your views to work with DTOs instead of entities:

```html
<!-- Views/Student/Index.cshtml -->
@model IEnumerable<AutoMapperMVC.DTOs.Student.StudentListItemDTO>

@{
    ViewData["Title"] = "Students";
}

<div class="container">
    <div class="row">
        <div class="col-md-12">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2>Student Management</h2>
                <a asp-action="Create" class="btn btn-primary">
                    <i class="fas fa-plus"></i> Add New Student
                </a>
            </div>

            @* Search and Filter Form *@
            <div class="card mb-4">
                <div class="card-body">
                    <form method="get" class="row g-3">
                        <div class="col-md-4">
                            <label for="searchTerm" class="form-label">Search</label>
                            <input type="text" class="form-control" id="searchTerm" name="searchTerm" 
                                   value="@ViewBag.SearchTerm" placeholder="Search by name or email...">
                        </div>
                        <div class="col-md-4">
                            <label for="department" class="form-label">Department</label>
                            <select class="form-select" id="department" name="department">
                                <option value="">All Departments</option>
                                <option value="Computer Science" selected="@(ViewBag.Department == "Computer Science")">Computer Science</option>
                                <option value="Engineering" selected="@(ViewBag.Department == "Engineering")">Engineering</option>
                                <option value="Business Administration" selected="@(ViewBag.Department == "Business Administration")">Business Administration</option>
                            </select>
                        </div>
                        <div class="col-md-4 d-flex align-items-end">
                            <button type="submit" class="btn btn-outline-primary me-2">Search</button>
                            <a href="@Url.Action("Index")" class="btn btn-outline-secondary">Clear</a>
                        </div>
                    </form>
                </div>
            </div>

            @* Student List Table *@
            @if (Model.Any())
            {
                <div class="card">
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover">
                                <thead class="table-dark">
                                    <tr>
                                        <th>@Html.DisplayNameFor(model => model.First().FullName)</th>
                                        <th>@Html.DisplayNameFor(model => model.First().Department)</th>
                                        <th>@Html.DisplayNameFor(model => model.First().Section)</th>
                                        <th>@Html.DisplayNameFor(model => model.First().Email)</th>
                                        <th>Grades</th>
                                        <th>Average</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    @foreach (var student in Model)
                                    {
                                        <tr>
                                            <td>
                                                <strong>@student.FullName</strong>
                                            </td>
                                            <td>@student.Department</td>
                                            <td>
                                                <span class="badge bg-primary">@student.Section</span>
                                            </td>
                                            <td>
                                                <a href="mailto:@student.Email">@student.Email</a>
                                            </td>
                                            <td>
                                                <span class="badge bg-info">@student.GradeCount grades</span>
                                            </td>
                                            <td>
                                                @if (student.AverageGrade.HasValue)
                                                {
                                                    <span class="badge @GetGradeBadgeClass(student.AverageGrade.Value)">
                                                        @student.AverageGrade.Value.ToString("F1")
                                                    </span>
                                                }
                                                else
                                                {
                                                    <span class="text-muted">No grades</span>
                                                }
                                            </td>
                                            <td>
                                                <div class="btn-group" role="group">
                                                    <a asp-action="Details" asp-route-id="@student.Id" 
                                                       class="btn btn-sm btn-outline-info" title="View Details">
                                                        <i class="fas fa-eye"></i>
                                                    </a>
                                                    <a asp-action="Edit" asp-route-id="@student.Id" 
                                                       class="btn btn-sm btn-outline-warning" title="Edit">
                                                        <i class="fas fa-edit"></i>
                                                    </a>
                                                    <a asp-action="Delete" asp-route-id="@student.Id" 
                                                       class="btn btn-sm btn-outline-danger" title="Delete">
                                                        <i class="fas fa-trash"></i>
                                                    </a>
                                                </div>
                                            </td>
                                        </tr>
                                    }
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            }
            else
            {
                <div class="alert alert-info">
                    <h4>No Students Found</h4>
                    <p>No students match your search criteria. <a asp-action="Create">Create a new student</a> to get started.</p>
                </div>
            }
        </div>
    </div>
</div>

@functions {
    private string GetGradeBadgeClass(decimal average)
    {
        return average switch
        {
            >= 90 => "bg-success",
            >= 80 => "bg-primary",
            >= 70 => "bg-warning",
            >= 60 => "bg-warning text-dark",
            _ => "bg-danger"
        };
    }
}
```

```html
<!-- Views/Student/Create.cshtml -->
@model AutoMapperMVC.DTOs.Student.StudentCreateDTO

@{
    ViewData["Title"] = "Create Student";
}

<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">Create New Student</h3>
                </div>
                <div class="card-body">
                    <form asp-action="Create" method="post" novalidate>
                        <div asp-validation-summary="ModelOnly" class="alert alert-danger" role="alert"></div>
                        
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="Name" class="form-label"></label>
                                <input asp-for="Name" class="form-control" />
                                <span asp-validation-for="Name" class="text-danger"></span>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label asp-for="Gender" class="form-label"></label>
                                <select asp-for="Gender" class="form-select">
                                    <option value="">-- Select Gender --</option>
                                    <option value="Male">Male</option>
                                    <option value="Female">Female</option>
                                </select>
                                <span asp-validation-for="Gender" class="text-danger"></span>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-8 mb-3">
                                <label asp-for="Branch" class="form-label"></label>
                                <input asp-for="Branch" class="form-control" list="branchOptions" />
                                <datalist id="branchOptions">
                                    <option value="Computer Science">
                                    <option value="Engineering">
                                    <option value="Business Administration">
                                    <option value="Mathematics">
                                    <option value="Physics">
                                </datalist>
                                <span asp-validation-for="Branch" class="text-danger"></span>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label asp-for="Section" class="form-label"></label>
                                <input asp-for="Section" class="form-control" maxlength="1" style="text-transform: uppercase;" />
                                <span asp-validation-for="Section" class="text-danger"></span>
                                <div class="form-text">Single letter (A-Z)</div>
                            </div>
                        </div>

                        <div class="mb-3">
                            <label asp-for="Email" class="form-label"></label>
                            <input asp-for="Email" class="form-control" type="email" />
                            <span asp-validation-for="Email" class="text-danger"></span>
                        </div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="PhoneNumber" class="form-label"></label>
                                <input asp-for="PhoneNumber" class="form-control" type="tel" />
                                <span asp-validation-for="PhoneNumber" class="text-danger"></span>
                                <div class="form-text">Optional</div>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label asp-for="EnrollmentDate" class="form-label"></label>
                                <input asp-for="EnrollmentDate" class="form-control" type="date" />
                                <span asp-validation-for="EnrollmentDate" class="text-danger"></span>
                            </div>
                        </div>

                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                            <a asp-action="Index" class="btn btn-secondary me-md-2">Cancel</a>
                            <button type="submit" class="btn btn-primary">Create Student</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    
    <script>
        // Auto-uppercase section input
        document.getElementById('Section').addEventListener('input', function(e) {
            e.target.value = e.target.value.toUpperCase();
        });
    </script>
}
```

### Phase 8: Testing and Validation

#### Step 13: Test Your AutoMapper Implementation

**1. Unit Test AutoMapper Profiles:**
```csharp
// Tests/MappingProfileTests.cs
using AutoMapper;
using AutoMapperMVC.DTOs.Student;
using AutoMapperMVC.MappingProfiles;
using AutoMapperMVC.Models;
using Xunit;

namespace AutoMapperMVC.Tests
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentMappingProfile>();
                cfg.AddProfile<GradeMappingProfile>();
            });

            configuration.AssertConfigurationIsValid();
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Should_Map_Student_To_StudentDTO()
        {
            // Arrange
            var student = new Student
            {
                StudentID = 1,
                Name = "John Doe",
                Branch = "Computer Science",
                Gender = "Male",
                Section = "A",
                Email = "john@university.edu",
                EnrollmentDate = DateTime.Today,
                Grades = new List<Grade>
                {
                    new Grade { GradeValue = 85 },
                    new Grade { GradeValue = 92 }
                }
            };

            // Act
            var dto = _mapper.Map<StudentDTO>(student);

            // Assert
            Assert.Equal(student.StudentID, dto.Id);
            Assert.Equal(student.Name, dto.FullName);
            Assert.Equal(student.Branch, dto.Department);
            Assert.Equal(88.5m, dto.AverageGrade); // (85 + 92) / 2
            Assert.Equal(2, dto.Grades.Count);
        }

        [Fact]
        public void Should_Map_StudentCreateDTO_To_Student()
        {
            // Arrange
            var createDto = new StudentCreateDTO
            {
                Name = "Jane Smith",
                Branch = "Engineering",
                Gender = "Female",
                Section = "B",
                Email = "jane@university.edu",
                EnrollmentDate = DateTime.Today
            };

            // Act
            var student = _mapper.Map<Student>(createDto);

            // Assert
            Assert.Equal(createDto.Name, student.Name);
            Assert.Equal(createDto.Branch, student.Branch);
            Assert.Equal(createDto.Gender, student.Gender);
            Assert.Equal(0, student.StudentID); // Should be ignored
            Assert.True(student.CreatedAt > DateTime.MinValue);
        }

        [Fact]
        public void Should_Handle_Null_Collections()
        {
            // Arrange
            var student = new Student
            {
                StudentID = 1,
                Name = "Test Student",
                Branch = "Test Branch",
                Grades = null
            };

            // Act & Assert - Should not throw exception
            var dto = _mapper.Map<StudentDTO>(student);
            Assert.NotNull(dto);
            Assert.Null(dto.AverageGrade);
        }
    }
}
```

**2. Integration Testing:**
```bash
# Run the application
dotnet run

# Test scenarios:
# 1. Create a new student - verify DTO validation works
# 2. View student list - check that only appropriate fields are displayed
# 3. Edit student - ensure update DTO works correctly
# 4. Check that internal fields are not exposed in views
```

#### Step 14: Performance Testing and Optimization
Test and optimize AutoMapper performance:

```csharp
// Performance testing and optimization
public class AutoMapperPerformanceTests
{
    [Fact]
    public void Benchmark_Mapping_Performance()
    {
        // Setup
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<StudentMappingProfile>());
        var mapper = configuration.CreateMapper();
        
        var students = GenerateTestStudents(1000);
        
        // Benchmark
        var stopwatch = Stopwatch.StartNew();
        
        var dtos = mapper.Map<List<StudentDTO>>(students);
        
        stopwatch.Stop();
        
        // Assert performance is acceptable
        Assert.True(stopwatch.ElapsedMilliseconds < 100, 
            $"Mapping took {stopwatch.ElapsedMilliseconds}ms, expected < 100ms");
    }

    [Fact]
    public void Compare_ProjectTo_vs_Map_Performance()
    {
        // Test ProjectTo vs Map for database queries
        // ProjectTo should be faster for large datasets
    }
}
```

### Common Pitfalls and Solutions

#### Pitfall 1: Missing Mappings
**Problem**: "Missing type map configuration" errors

**Solution**:
```csharp
// Ensure all mappings are configured
CreateMap<Source, Destination>();
CreateMap<Destination, Source>(); // If reverse mapping needed

// Or use ReverseMap()
CreateMap<Source, Destination>().ReverseMap();
```

#### Pitfall 2: Performance Issues
**Problem**: Slow mapping with large datasets

**Solution**:
```csharp
// Use ProjectTo for database queries
var dtos = await context.Students
    .ProjectTo<StudentDTO>(mapper.ConfigurationProvider)
    .ToListAsync();

// Instead of
var students = await context.Students.ToListAsync();
var dtos = mapper.Map<List<StudentDTO>>(students);
```

#### Pitfall 3: Null Reference Exceptions
**Problem**: Mapping fails when properties are null

**Solution**:
```csharp
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => 
        src.Student != null ? src.Student.Name : "Unknown"));
```

#### Pitfall 4: Circular References
**Problem**: Stack overflow with navigation properties

**Solution**:
```csharp
CreateMap<Student, StudentDTO>()
    .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.Grades))
    .MaxDepth(2); // Limit recursion depth

// Or use custom resolvers for complex scenarios
```

### Conclusion

Following this comprehensive step-by-step guide will help you successfully implement AutoMapper in your ASP.NET Core MVC application. The key benefits include:

- **Reduced Boilerplate Code**: 90% less manual mapping code
- **Better Security**: DTOs prevent over-posting and data exposure
- **Improved Maintainability**: Centralized mapping configuration
- **Enhanced Performance**: Efficient projection capabilities
- **Cleaner Architecture**: Clear separation between domain and presentation layers

Remember to:
1. Design DTOs based on specific use cases
2. Configure mappings thoroughly in profiles
3. Test mapping configurations
4. Use ProjectTo for database performance
5. Handle edge cases like null values
6. Validate mapping configuration in development

AutoMapper transforms object mapping from a tedious, error-prone task into a clean, maintainable aspect of your application architecture.
