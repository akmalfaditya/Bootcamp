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
