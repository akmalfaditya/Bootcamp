# Student Management MVC Application

A comprehensive Student Management System built with **ASP.NET Core MVC** and **Entity Framework Core**, demonstrating modern web development practices and clean architecture principles.

## Project Overview

This application showcases a complete CRUD (Create, Read, Update, Delete) system for managing students and their academic records. It is designed as both a learning resource and a foundation for more complex educational management systems.

### Key Features
- **Student Management**: Complete profile management with validation
- **Grade Tracking**: Academic performance monitoring with automatic letter grade calculation  
- **Advanced Search**: Multi-field search capabilities across student data
- **Responsive Design**: Mobile-friendly interface using Bootstrap 5
- **Database Integration**: SQLite database with Entity Framework Core
- **Professional UI**: Clean, intuitive user interface with modern design patterns

---

## **Quick Start - Run the Existing Project**

If you want to run this existing project immediately:

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Running the Application
```bash
# 1. Navigate to the project directory
cd "c:\Users\Formulatrix\Documents\Bootcamp\Practice\Web Development\Basic MVC"

# 2. Restore NuGet packages
dotnet restore

# 3. Apply database migrations (creates SQLite database)
dotnet ef database update

# 4. Build and run the application
dotnet run
```

### Accessing the Application
- Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`
- The application will automatically create a SQLite database file named `studentmanagement.db`
- Sample data will be seeded automatically for testing

---

## **Project Structure Overview**

Before building from scratch, here is what the completed project contains:

```
StudentManagementMVC/
├── Controllers/              # HTTP request handlers
│   ├── HomeController.cs       # Home page controller
│   └── StudentController.cs    # Student CRUD operations
├── Models/                   # Data entities and view models
│   ├── Student.cs             # Student entity with validation
│   ├── Grade.cs               # Grade entity with relationships
│   └── ErrorViewModel.cs      # Error handling model
├── Views/                    # Razor view templates
│   ├── Home/                  # Home page views
│   ├── Student/               # Student management views
│   │   ├── Index.cshtml         # Student list with search
│   │   ├── Create.cshtml        # Add new student form
│   │   ├── Edit.cshtml          # Edit student form
│   │   ├── Details.cshtml       # Student details view
│   │   ├── Delete.cshtml        # Delete confirmation
│   │   ├── Grades.cshtml        # Grade management
│   │   └── AddGrade.cshtml      # Add new grade form
│   └── Shared/                # Shared layout components
├── Data/                     # Database context and configuration
│   └── ApplicationDbContext.cs # EF Core database context
├── Services/                 # Business logic layer
│   └── StudentService.cs      # Student business operations
├── Migrations/               # Entity Framework migrations
├── wwwroot/                  # Static files (CSS, JS, images)
├── Program.cs                # Application startup configuration
├── appsettings.json          # Configuration settings
└── StudentManagementMVC.csproj # Project file with dependencies
```

## **Technology Stack**

### Backend Technologies
- **ASP.NET Core 8.0**: Microsoft's modern web framework
- **Entity Framework Core 9.0.0**: Object-Relational Mapping (ORM)
- **SQLite**: Lightweight, serverless database
- **C# 12**: Latest language features

### Frontend Technologies  
- **Razor Pages**: Server-side rendering with C# integration
- **Bootstrap 5**: Responsive CSS framework
- **jQuery**: JavaScript library for enhanced interactivity
- **Font Awesome**: Professional icon library

### Development Tools
- **Entity Framework CLI**: Database migration management
- **Visual Studio Code/Visual Studio**: IDE support
- **NuGet Package Manager**: Dependency management

---

## **Step-by-Step Tutorial: Build This Project From Scratch**

This comprehensive tutorial will guide you through creating the entire Student Management MVC application from the ground up. Perfect for learning ASP.NET Core MVC!

### Prerequisites for Building from Scratch
- Visual Studio 2022 or VS Code
- .NET 8.0 SDK or later
- Basic understanding of C# programming
- Familiarity with HTML/CSS (helpful but not required)

---

## **Phase 1: Project Setup & Foundation**

### Step 1: Create New ASP.NET Core MVC Project
```bash
# Create new MVC project
dotnet new mvc -n StudentManagementMVC
cd StudentManagementMVC

# Verify the project runs
dotnet run
```

### Step 2: Install Required NuGet Packages
```bash
# Install Entity Framework Core for SQLite (specific versions for compatibility)
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0  
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0

# Install Entity Framework CLI tools globally (if not already installed)
dotnet tool install --global dotnet-ef
```

Your `.csproj` file should now include:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
```

### Step 2.1: Create Project Folders
```bash
# Create necessary directories for organized code structure
mkdir Models
mkdir Data  
mkdir Services
mkdir DTOs
mkdir Repositories/Interfaces
mkdir Repositories/Implementations
mkdir Mappings
```

---

## **Phase 2: Data Models & Database Setup**

### Step 3: Create the Student Model
Create `/Models/Student.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace StudentManagementMVC.Models
{
    public class Student
    {
        // Primary key - Entity Framework recognizes this pattern
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Other)$", 
            ErrorMessage = "Gender must be Male, Female, or Other")]
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

        [Required(ErrorMessage = "Enrollment date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        // Navigation property for one-to-many relationship
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
```

### Step 4: Create the Grade Model
Create `/Models/Grade.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace StudentManagementMVC.Models
{
    public class Grade
    {
        public int GradeID { get; set; }

        // Foreign key to Student
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Display(Name = "Grade (%)")]
        public decimal GradeValue { get; set; }

        [StringLength(2, ErrorMessage = "Letter grade cannot exceed 2 characters")]
        [Display(Name = "Letter Grade")]
        public string? LetterGrade { get; set; }

        [Required(ErrorMessage = "Grade date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Recorded")]
        public DateTime GradeDate { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string? Comments { get; set; }

        // Navigation property back to Student
        public virtual Student Student { get; set; } = null!;
    }
}
```

### Step 5: Create Database Context
Create `/Data/ApplicationDbContext.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using StudentManagementMVC.Models;

namespace StudentManagementMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties represent tables in your database
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision for grades
            modelBuilder.Entity<Grade>()
                .Property(g => g.GradeValue)
                .HasColumnType("decimal(5,2)");

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Students
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
                },
                new Student
                {
                    StudentID = 2,
                    Name = "Sarah Johnson",
                    Gender = "Female",
                    Branch = "Engineering",
                    Section = "B",
                    Email = "sarah.johnson@university.edu",
                    EnrollmentDate = new DateTime(2023, 9, 1)
                },
                new Student
                {
                    StudentID = 3,
                    Name = "Mike Davis",
                    Gender = "Male",
                    Branch = "Business Administration",
                    Section = "A",
                    Email = "mike.davis@university.edu",
                    EnrollmentDate = new DateTime(2024, 1, 15)
                }
            );

            // Seed Grades
            modelBuilder.Entity<Grade>().HasData(
                new Grade
                {
                    GradeID = 1,
                    StudentID = 1,
                    Subject = "Data Structures",
                    GradeValue = 95.5m,
                    LetterGrade = "A",
                    GradeDate = new DateTime(2024, 3, 15),
                    Comments = "Excellent understanding of algorithms"
                },
                new Grade
                {
                    GradeID = 2,
                    StudentID = 1,
                    Subject = "Web Development",
                    GradeValue = 88.0m,
                    LetterGrade = "B",
                    GradeDate = new DateTime(2024, 4, 20),
                    Comments = "Good work on MVC project"
                },
                new Grade
                {
                    GradeID = 3,
                    StudentID = 2,
                    Subject = "Calculus",
                    GradeValue = 92.0m,
                    LetterGrade = "A",
                    GradeDate = new DateTime(2024, 3, 10),
                    Comments = "Strong mathematical foundation"
                }
            );
        }
    }
}
```

---

## **Phase 3: Business Logic Layer**

### Step 6: Create Service Interface and Implementation
Create `/Services/StudentService.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using StudentManagementMVC.Data;
using StudentManagementMVC.Models;

namespace StudentManagementMVC.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);
        Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId);
        Task<Grade> AddGradeAsync(Grade grade);
    }

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.Grades)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentID == id);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            if (student.EnrollmentDate == default(DateTime))
                student.EnrollmentDate = DateTime.Today;

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllStudentsAsync();

            return await _context.Students
                .Include(s => s.Grades)
                .Where(s => s.Name.Contains(searchTerm) || 
                           s.Branch.Contains(searchTerm) || 
                           s.Section.Contains(searchTerm))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            return await _context.Grades
                .Where(g => g.StudentID == studentId)
                .OrderBy(g => g.Subject)
                .ToListAsync();
        }

        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            // Auto-calculate letter grade
            grade.LetterGrade = CalculateLetterGrade(grade.GradeValue);
            
            if (grade.GradeDate == default(DateTime))
                grade.GradeDate = DateTime.Today;

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        private string CalculateLetterGrade(decimal gradeValue)
        {
            return gradeValue switch
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
                >= 60 => "D",
                _ => "F"
            };
        }
    }
}
```

---

## **Phase 4: Configure Services & Database**

### Step 7: Update Program.cs
Replace the content of `Program.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using StudentManagementMVC.Data;
using StudentManagementMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework and configure SQLite database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=studentmanagement.db"));

// Register business layer service
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

### Step 8: Create and Apply Database Migration
```bash
# Create the initial migration (this generates the database schema)
dotnet ef migrations add InitialCreate

# Apply the migration to create the SQLite database
dotnet ef database update
```

**What happens during migration:**
- A `Migrations` folder is created with migration files
- A SQLite database file `studentmanagement.db` is created in your project root
- All tables (Students, Grades) are created with proper relationships
- Sample data is automatically seeded for testing

**Troubleshooting Migration Issues:**
```bash
# If you encounter migration errors, try:
dotnet ef database drop --force        # Delete existing database
dotnet ef migrations remove           # Remove last migration
dotnet ef migrations add InitialCreate # Recreate migration
dotnet ef database update            # Apply migration
```

---

## **Phase 5: Controller Layer**

### Step 9: Create Student Controller
Create `/Controllers/StudentController.cs`:
```csharp
using Microsoft.AspNetCore.Mvc;
using StudentManagementMVC.Models;
using StudentManagementMVC.Services;

namespace StudentManagementMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Student
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<Student> students;

            if (!string.IsNullOrEmpty(searchString))
            {
                students = await _studentService.SearchStudentsAsync(searchString);
                ViewData["CurrentFilter"] = searchString;
            }
            else
            {
                students = await _studentService.GetAllStudentsAsync();
            }

            return View(students);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound();

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentService.CreateStudentAsync(student);
                TempData["SuccessMessage"] = "Student created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.StudentID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.UpdateStudentAsync(student);
                    TempData["SuccessMessage"] = "Student updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _studentService.DeleteStudentAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Student deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting student.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Grades/5
        public async Task<IActionResult> Grades(int? id)
        {
            if (id == null) return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound();

            ViewBag.Student = student;
            var grades = await _studentService.GetGradesByStudentIdAsync(id.Value);
            return View(grades);
        }

        // GET: Student/AddGrade/5
        public async Task<IActionResult> AddGrade(int? id)
        {
            if (id == null) return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound();

            ViewBag.Student = student;
            var grade = new Grade { StudentID = id.Value };
            return View(grade);
        }

        // POST: Student/AddGrade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGrade(Grade grade)
        {
            if (ModelState.IsValid)
            {
                await _studentService.AddGradeAsync(grade);
                TempData["SuccessMessage"] = "Grade added successfully!";
                return RedirectToAction(nameof(Grades), new { id = grade.StudentID });
            }

            var student = await _studentService.GetStudentByIdAsync(grade.StudentID);
            ViewBag.Student = student;
            return View(grade);
        }
    }
}
```

---

## **Phase 6: View Layer - Create All Views**

### Step 10: Update Shared Layout
Update `/Views/Shared/_Layout.cshtml` to add navigation:
```html
<!-- Add this in the navbar section -->
<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="Student" asp-action="Index">Students</a>
</li>
```

### Step 11: Create Student Views

Create `/Views/Student/Index.cshtml`:
```html
@model IEnumerable<StudentManagementMVC.Models.Student>
@{
    ViewData["Title"] = "Students";
}

<div class="container">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2><i class="fas fa-users"></i> Student Management</h2>
        <a asp-action="Create" class="btn btn-success">
            <i class="fas fa-plus"></i> Add New Student
        </a>
    </div>

    <!-- Search Form -->
    <form asp-action="Index" method="get" class="mb-4">
        <div class="input-group">
            <input type="text" name="searchString" value="@ViewData["CurrentFilter"]" 
                   class="form-control" placeholder="Search students by name, branch, or section..." />
            <button class="btn btn-outline-primary" type="submit">
                <i class="fas fa-search"></i> Search
            </button>
            <a asp-action="Index" class="btn btn-outline-secondary">
                <i class="fas fa-times"></i> Clear
            </a>
        </div>
    </form>

    <!-- Students Table -->
    <div class="card">
        <div class="card-body">
            <table class="table table-striped table-hover">
                <thead class="table-dark">
                    <tr>
                        <th>Name</th>
                        <th>Branch</th>
                        <th>Section</th>
                        <th>Email</th>
                        <th>Enrollment Date</th>
                        <th>Grades</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var student in Model)
                    {
                        <tr>
                            <td>@student.Name</td>
                            <td>@student.Branch</td>
                            <td>@student.Section</td>
                            <td>@student.Email</td>
                            <td>@student.EnrollmentDate.ToString("MMM dd, yyyy")</td>
                            <td>
                                <span class="badge bg-info">@student.Grades.Count() grades</span>
                            </td>
                            <td>
                                <div class="btn-group" role="group">
                                    <a asp-action="Details" asp-route-id="@student.StudentID" 
                                       class="btn btn-sm btn-outline-info">
                                        <i class="fas fa-eye"></i>
                                    </a>
                                    <a asp-action="Edit" asp-route-id="@student.StudentID" 
                                       class="btn btn-sm btn-outline-warning">
                                        <i class="fas fa-edit"></i>
                                    </a>
                                    <a asp-action="Grades" asp-route-id="@student.StudentID" 
                                       class="btn btn-sm btn-outline-success">
                                        <i class="fas fa-chart-bar"></i>
                                    </a>
                                    <a asp-action="Delete" asp-route-id="@student.StudentID" 
                                       class="btn btn-sm btn-outline-danger">
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
```

### Step 12: Create Student Create Form
Create `/Views/Student/Create.cshtml`:
```html
@model StudentManagementMVC.Models.Student
@{
    ViewData["Title"] = "Create Student";
}

<div class="container">
    <div class="row">
        <div class="col-md-8 offset-md-2">
            <div class="card">
                <div class="card-header bg-success text-white">
                    <h5><i class="fas fa-user-plus"></i> Add New Student</h5>
                </div>
                <div class="card-body">
                    <form asp-action="Create" method="post">
                        @Html.AntiForgeryToken()
                        
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label asp-for="Name" class="form-label">Full Name *</label>
                                    <input asp-for="Name" class="form-control" />
                                    <span asp-validation-for="Name" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="Gender" class="form-label">Gender *</label>
                                    <select asp-for="Gender" class="form-select">
                                        <option value="">-- Select Gender --</option>
                                        <option value="Male">Male</option>
                                        <option value="Female">Female</option>
                                        <option value="Other">Other</option>
                                    </select>
                                    <span asp-validation-for="Gender" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="Email" class="form-label">Email Address *</label>
                                    <input asp-for="Email" type="email" class="form-control" />
                                    <span asp-validation-for="Email" class="text-danger"></span>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label asp-for="Branch" class="form-label">Branch/Major *</label>
                                    <select asp-for="Branch" class="form-select">
                                        <option value="">-- Select Branch --</option>
                                        <option value="Computer Science">Computer Science</option>
                                        <option value="Engineering">Engineering</option>
                                        <option value="Business Administration">Business Administration</option>
                                        <option value="Mathematics">Mathematics</option>
                                    </select>
                                    <span asp-validation-for="Branch" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="Section" class="form-label">Section *</label>
                                    <select asp-for="Section" class="form-select">
                                        <option value="">-- Select Section --</option>
                                        <option value="A">Section A</option>
                                        <option value="B">Section B</option>
                                        <option value="C">Section C</option>
                                    </select>
                                    <span asp-validation-for="Section" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="EnrollmentDate" class="form-label">Enrollment Date *</label>
                                    <input asp-for="EnrollmentDate" type="date" class="form-control" />
                                    <span asp-validation-for="EnrollmentDate" class="text-danger"></span>
                                </div>
                            </div>
                        </div>

                        <div class="d-grid gap-2 d-md-flex justify-content-md-end mt-4">
                            <a asp-action="Index" class="btn btn-secondary me-md-2">Cancel</a>
                            <button type="submit" class="btn btn-success">Create Student</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

### Step 12: Create Student Edit Form
Create `/Views/Student/Edit.cshtml`:
```html
@model StudentManagementMVC.Models.Student
@{
    ViewData["Title"] = "Edit Student";
}

<div class="container">
    <div class="row">
        <div class="col-md-8 offset-md-2">
            <div class="card">
                <div class="card-header bg-warning text-dark">
                    <h5><i class="fas fa-user-edit"></i> Edit Student: @Model.Name</h5>
                </div>
                <div class="card-body">
                    <form asp-action="Edit" method="post">
                        @Html.AntiForgeryToken()
                        <input type="hidden" asp-for="StudentID" />
                        
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label asp-for="Name" class="form-label">Full Name *</label>
                                    <input asp-for="Name" class="form-control" />
                                    <span asp-validation-for="Name" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="Gender" class="form-label">Gender *</label>
                                    <select asp-for="Gender" class="form-select">
                                        <option value="">-- Select Gender --</option>
                                        <option value="Male">Male</option>
                                        <option value="Female">Female</option>
                                        <option value="Other">Other</option>
                                    </select>
                                    <span asp-validation-for="Gender" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="Email" class="form-label">Email Address *</label>
                                    <input asp-for="Email" type="email" class="form-control" />
                                    <span asp-validation-for="Email" class="text-danger"></span>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label asp-for="Branch" class="form-label">Branch/Major *</label>
                                    <select asp-for="Branch" class="form-select">
                                        <option value="">-- Select Branch --</option>
                                        <option value="Computer Science">Computer Science</option>
                                        <option value="Engineering">Engineering</option>
                                        <option value="Business Administration">Business Administration</option>
                                        <option value="Mathematics">Mathematics</option>
                                    </select>
                                    <span asp-validation-for="Branch" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="Section" class="form-label">Section *</label>
                                    <select asp-for="Section" class="form-select">
                                        <option value="">-- Select Section --</option>
                                        <option value="A">Section A</option>
                                        <option value="B">Section B</option>
                                        <option value="C">Section C</option>
                                    </select>
                                    <span asp-validation-for="Section" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="EnrollmentDate" class="form-label">Enrollment Date *</label>
                                    <input asp-for="EnrollmentDate" type="date" class="form-control" />
                                    <span asp-validation-for="EnrollmentDate" class="text-danger"></span>
                                </div>
                            </div>
                        </div>

                        <div class="d-grid gap-2 d-md-flex justify-content-md-end mt-4">
                            <a asp-action="Index" class="btn btn-secondary me-md-2">Cancel</a>
                            <a asp-action="Details" asp-route-id="@Model.StudentID" class="btn btn-info me-md-2">View Details</a>
                            <button type="submit" class="btn btn-warning">Update Student</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

### Step 13: Create Student Details View
Create `/Views/Student/Details.cshtml`:
```html
@model StudentManagementMVC.Models.Student
@{
    ViewData["Title"] = "Student Details";
}

<div class="container">
    <div class="row">
        <div class="col-md-8 offset-md-2">
            <div class="card">
                <div class="card-header bg-info text-white">
                    <h5><i class="fas fa-user"></i> Student Details</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <table class="table table-borderless">
                                <tr>
                                    <td><strong>Student ID:</strong></td>
                                    <td>@Model.StudentID</td>
                                </tr>
                                <tr>
                                    <td><strong>Full Name:</strong></td>
                                    <td>@Model.Name</td>
                                </tr>
                                <tr>
                                    <td><strong>Gender:</strong></td>
                                    <td>@Model.Gender</td>
                                </tr>
                                <tr>
                                    <td><strong>Email:</strong></td>
                                    <td><a href="mailto:@Model.Email">@Model.Email</a></td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-6">
                            <table class="table table-borderless">
                                <tr>
                                    <td><strong>Branch/Major:</strong></td>
                                    <td>@Model.Branch</td>
                                </tr>
                                <tr>
                                    <td><strong>Section:</strong></td>
                                    <td>@Model.Section</td>
                                </tr>
                                <tr>
                                    <td><strong>Enrollment Date:</strong></td>
                                    <td>@Model.EnrollmentDate.ToString("MMMM dd, yyyy")</td>
                                </tr>
                                <tr>
                                    <td><strong>Total Grades:</strong></td>
                                    <td><span class="badge bg-success">@Model.Grades.Count()</span></td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    @if (Model.Grades.Any())
                    {
                        <hr />
                        <h6><i class="fas fa-chart-bar"></i> Recent Grades</h6>
                        <div class="table-responsive">
                            <table class="table table-sm">
                                <thead class="table-light">
                                    <tr>
                                        <th>Subject</th>
                                        <th>Grade</th>
                                        <th>Letter</th>
                                        <th>Date</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    @foreach (var grade in Model.Grades.OrderByDescending(g => g.GradeDate).Take(5))
                                    {
                                        <tr>
                                            <td>@grade.Subject</td>
                                            <td>@grade.GradeValue.ToString("F1")%</td>
                                            <td><span class="badge bg-primary">@grade.LetterGrade</span></td>
                                            <td>@grade.GradeDate.ToString("MMM dd, yyyy")</td>
                                        </tr>
                                    }
                                </tbody>
                            </table>
                        </div>
                    }

                    <div class="d-grid gap-2 d-md-flex justify-content-md-end mt-4">
                        <a asp-action="Index" class="btn btn-secondary me-md-2">Back to List</a>
                        <a asp-action="Grades" asp-route-id="@Model.StudentID" class="btn btn-success me-md-2">
                            <i class="fas fa-chart-bar"></i> Manage Grades
                        </a>
                        <a asp-action="Edit" asp-route-id="@Model.StudentID" class="btn btn-warning me-md-2">
                            <i class="fas fa-edit"></i> Edit
                        </a>
                        <a asp-action="Delete" asp-route-id="@Model.StudentID" class="btn btn-danger">
                            <i class="fas fa-trash"></i> Delete
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```

### Step 14: Create Delete Confirmation View
Create `/Views/Student/Delete.cshtml`:
```html
@model StudentManagementMVC.Models.Student
@{
    ViewData["Title"] = "Delete Student";
}

<div class="container">
    <div class="row">
        <div class="col-md-6 offset-md-3">
            <div class="card border-danger">
                <div class="card-header bg-danger text-white">
                    <h5><i class="fas fa-exclamation-triangle"></i> Confirm Deletion</h5>
                </div>
                <div class="card-body">
                    <div class="alert alert-warning">
                        <strong>Warning:</strong> This action cannot be undone. All grades associated with this student will also be deleted.
                    </div>

                    <p>Are you sure you want to delete the following student?</p>

                    <div class="card">
                        <div class="card-body">
                            <h6 class="card-title">@Model.Name</h6>
                            <p class="card-text">
                                <strong>Branch:</strong> @Model.Branch<br/>
                                <strong>Section:</strong> @Model.Section<br/>
                                <strong>Email:</strong> @Model.Email<br/>
                                <strong>Enrollment Date:</strong> @Model.EnrollmentDate.ToString("MMMM dd, yyyy")<br/>
                                <strong>Associated Grades:</strong> @Model.Grades.Count()
                            </p>
                        </div>
                    </div>

                    <form asp-action="Delete" method="post" class="mt-3">
                        @Html.AntiForgeryToken()
                        <input type="hidden" asp-for="StudentID" />
                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                            <a asp-action="Index" class="btn btn-secondary me-md-2">Cancel</a>
                            <a asp-action="Details" asp-route-id="@Model.StudentID" class="btn btn-info me-md-2">View Details</a>
                            <button type="submit" class="btn btn-danger">
                                <i class="fas fa-trash"></i> Delete Student
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
```

### Step 15: Create Grade Management Views
Create `/Views/Student/Grades.cshtml`:
```html
@model IEnumerable<StudentManagementMVC.Models.Grade>
@{
    ViewData["Title"] = "Student Grades";
    var student = ViewBag.Student as StudentManagementMVC.Models.Student;
}

<div class="container">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
            <h2><i class="fas fa-chart-bar"></i> Grades for @student.Name</h2>
            <p class="text-muted">@student.Branch - Section @student.Section</p>
        </div>
        <a asp-action="AddGrade" asp-route-id="@student.StudentID" class="btn btn-success">
            <i class="fas fa-plus"></i> Add New Grade
        </a>
    </div>

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a asp-action="Index">Students</a></li>
            <li class="breadcrumb-item"><a asp-action="Details" asp-route-id="@student.StudentID">@student.Name</a></li>
            <li class="breadcrumb-item active" aria-current="page">Grades</li>
        </ol>
    </nav>

    @if (Model.Any())
    {
        <!-- Grade Statistics -->
        <div class="row mb-4">
            <div class="col-md-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">Total Grades</h5>
                        <h3 class="text-primary">@Model.Count()</h3>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">Average Grade</h5>
                        <h3 class="text-success">@Model.Average(g => g.GradeValue).ToString("F1")%</h3>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">Highest Grade</h5>
                        <h3 class="text-warning">@Model.Max(g => g.GradeValue).ToString("F1")%</h3>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">Subjects</h5>
                        <h3 class="text-info">@Model.Select(g => g.Subject).Distinct().Count()</h3>
                    </div>
                </div>
            </div>
        </div>

        <!-- Grades Table -->
        <div class="card">
            <div class="card-body">
                <table class="table table-striped table-hover">
                    <thead class="table-dark">
                        <tr>
                            <th>Subject</th>
                            <th>Grade (%)</th>
                            <th>Letter Grade</th>
                            <th>Date Recorded</th>
                            <th>Comments</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach (var grade in Model.OrderByDescending(g => g.GradeDate))
                        {
                            <tr>
                                <td><strong>@grade.Subject</strong></td>
                                <td>
                                    <span class="fs-5">@grade.GradeValue.ToString("F1")%</span>
                                </td>
                                <td>
                                    <span class="badge bg-@(grade.GradeValue >= 90 ? "success" : 
                                                          grade.GradeValue >= 80 ? "primary" : 
                                                          grade.GradeValue >= 70 ? "warning" : "danger") fs-6">
                                        @grade.LetterGrade
                                    </span>
                                </td>
                                <td>@grade.GradeDate.ToString("MMM dd, yyyy")</td>
                                <td>
                                    @if (!string.IsNullOrEmpty(grade.Comments))
                                    {
                                        <span class="text-muted">@(grade.Comments.Length > 50 ? grade.Comments.Substring(0, 50) + "..." : grade.Comments)</span>
                                    }
                                    else
                                    {
                                        <span class="text-muted">No comments</span>
                                    }
                                </td>
                                <td>
                                    <div class="btn-group" role="group">
                                        <button type="button" class="btn btn-sm btn-outline-info" 
                                                title="View Details" data-bs-toggle="tooltip">
                                            <i class="fas fa-eye"></i>
                                        </button>
                                        <button type="button" class="btn btn-sm btn-outline-warning" 
                                                title="Edit Grade" data-bs-toggle="tooltip">
                                            <i class="fas fa-edit"></i>
                                        </button>
                                        <button type="button" class="btn btn-sm btn-outline-danger" 
                                                title="Delete Grade" data-bs-toggle="tooltip">
                                            <i class="fas fa-trash"></i>
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        }
                    </tbody>
                </table>
            </div>
        </div>
    }
    else
    {
        <div class="alert alert-info text-center">
            <h4><i class="fas fa-info-circle"></i> No Grades Yet</h4>
            <p>This student does not have any grades recorded yet.</p>
            <a asp-action="AddGrade" asp-route-id="@student.StudentID" class="btn btn-success">
                <i class="fas fa-plus"></i> Add First Grade
            </a>
        </div>
    }

    <div class="mt-4">
        <a asp-action="Details" asp-route-id="@student.StudentID" class="btn btn-info me-2">
            <i class="fas fa-user"></i> Back to Student Details
        </a>
        <a asp-action="Index" class="btn btn-secondary">
            <i class="fas fa-list"></i> Back to Students List
        </a>
    </div>
</div>

@section Scripts {
    <script>
        // Initialize Bootstrap tooltips
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl)
        })
    </script>
}
```

Create `/Views/Student/AddGrade.cshtml`:
```html
@model StudentManagementMVC.Models.Grade
@{
    ViewData["Title"] = "Add Grade";
    var student = ViewBag.Student as StudentManagementMVC.Models.Student;
}

<div class="container">
    <div class="row">
        <div class="col-md-8 offset-md-2">
            <div class="card">
                <div class="card-header bg-success text-white">
                    <h5><i class="fas fa-plus"></i> Add Grade for @student.Name</h5>
                    <small>@student.Branch - Section @student.Section</small>
                </div>
                <div class="card-body">
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a asp-action="Index">Students</a></li>
                            <li class="breadcrumb-item"><a asp-action="Details" asp-route-id="@student.StudentID">@student.Name</a></li>
                            <li class="breadcrumb-item"><a asp-action="Grades" asp-route-id="@student.StudentID">Grades</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Add Grade</li>
                        </ol>
                    </nav>

                    <form asp-action="AddGrade" method="post">
                        @Html.AntiForgeryToken()
                        <input type="hidden" asp-for="StudentID" />
                        
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label asp-for="Subject" class="form-label">Subject *</label>
                                    <select asp-for="Subject" class="form-select">
                                        <option value="">-- Select Subject --</option>
                                        <option value="Mathematics">Mathematics</option>
                                        <option value="Physics">Physics</option>
                                        <option value="Chemistry">Chemistry</option>
                                        <option value="Biology">Biology</option>
                                        <option value="Computer Science">Computer Science</option>
                                        <option value="Data Structures">Data Structures</option>
                                        <option value="Web Development">Web Development</option>
                                        <option value="Database Systems">Database Systems</option>
                                        <option value="Software Engineering">Software Engineering</option>
                                        <option value="English">English</option>
                                        <option value="Business Studies">Business Studies</option>
                                        <option value="Economics">Economics</option>
                                        <option value="History">History</option>
                                        <option value="Geography">Geography</option>
                                    </select>
                                    <span asp-validation-for="Subject" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label asp-for="GradeValue" class="form-label">Grade (0-100) *</label>
                                    <input asp-for="GradeValue" type="number" step="0.1" min="0" max="100" 
                                           class="form-control" placeholder="Enter grade percentage" />
                                    <span asp-validation-for="GradeValue" class="text-danger"></span>
                                    <div class="form-text">Enter a value between 0 and 100</div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label asp-for="GradeDate" class="form-label">Date Recorded *</label>
                                    <input asp-for="GradeDate" type="date" class="form-control" 
                                           value="@DateTime.Today.ToString("yyyy-MM-dd")" />
                                    <span asp-validation-for="GradeDate" class="text-danger"></span>
                                </div>

                                <div class="form-group mb-3">
                                    <label class="form-label">Estimated Letter Grade</label>
                                    <div class="form-control bg-light" id="letterGradePreview">
                                        Enter a grade to see letter grade
                                    </div>
                                    <div class="form-text">This will be calculated automatically</div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group mb-3">
                            <label asp-for="Comments" class="form-label">Comments (Optional)</label>
                            <textarea asp-for="Comments" class="form-control" rows="3" 
                                      placeholder="Add any additional comments about this grade..."></textarea>
                            <span asp-validation-for="Comments" class="text-danger"></span>
                        </div>

                        <div class="d-grid gap-2 d-md-flex justify-content-md-end mt-4">
                            <a asp-action="Grades" asp-route-id="@student.StudentID" class="btn btn-secondary me-md-2">Cancel</a>
                            <button type="submit" class="btn btn-success">Add Grade</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
    
    <script>
        // Real-time letter grade calculation
        document.getElementById('GradeValue').addEventListener('input', function() {
            var gradeValue = parseFloat(this.value);
            var letterGradeElement = document.getElementById('letterGradePreview');
            
            if (isNaN(gradeValue) || gradeValue < 0 || gradeValue > 100) {
                letterGradeElement.innerHTML = 'Enter a valid grade (0-100)';
                letterGradeElement.className = 'form-control bg-light';
                return;
            }
            
            var letterGrade = '';
            var badgeClass = '';
            
            if (gradeValue >= 97) {
                letterGrade = 'A+';
                badgeClass = 'bg-success';
            } else if (gradeValue >= 93) {
                letterGrade = 'A';
                badgeClass = 'bg-success';
            } else if (gradeValue >= 90) {
                letterGrade = 'A-';
                badgeClass = 'bg-success';
            } else if (gradeValue >= 87) {
                letterGrade = 'B+';
                badgeClass = 'bg-primary';
            } else if (gradeValue >= 83) {
                letterGrade = 'B';
                badgeClass = 'bg-primary';
            } else if (gradeValue >= 80) {
                letterGrade = 'B-';
                badgeClass = 'bg-primary';
            } else if (gradeValue >= 77) {
                letterGrade = 'C+';
                badgeClass = 'bg-warning';
            } else if (gradeValue >= 73) {
                letterGrade = 'C';
                badgeClass = 'bg-warning';
            } else if (gradeValue >= 70) {
                letterGrade = 'C-';
                badgeClass = 'bg-warning';
            } else if (gradeValue >= 67) {
                letterGrade = 'D+';
                badgeClass = 'bg-secondary';
            } else if (gradeValue >= 60) {
                letterGrade = 'D';
                badgeClass = 'bg-secondary';
            } else {
                letterGrade = 'F';
                badgeClass = 'bg-danger';
            }
            
            letterGradeElement.innerHTML = '<span class="badge ' + badgeClass + ' fs-6">' + letterGrade + '</span>';
            letterGradeElement.className = 'form-control bg-light d-flex align-items-center';
        });
    </script>
}
```

---

## **Phase 9: Configuration and Final Setup**

### Step 16: Update Connection String Configuration
Update `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=studentmanagement.db"
  },
  "AllowedHosts": "*"
}
```

### Step 17: Add Success/Error Message Display
Update `/Views/Shared/_Layout.cshtml` to include message display:
```html
<!-- Add this after the <main> tag opening and before @RenderBody() -->
@if (TempData["SuccessMessage"] != null)
{
    <div class="alert alert-success alert-dismissible fade show" role="alert">
        <i class="fas fa-check-circle"></i> @TempData["SuccessMessage"]
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
}

@if (TempData["ErrorMessage"] != null)
{
    <div class="alert alert-danger alert-dismissible fade show" role="alert">
        <i class="fas fa-exclamation-circle"></i> @TempData["ErrorMessage"]
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
}

@if (TempData["InfoMessage"] != null)
{
    <div class="alert alert-info alert-dismissible fade show" role="alert">
        <i class="fas fa-info-circle"></i> @TempData["InfoMessage"]
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
}
```

### Step 18: Add Font Awesome Icons
Update `/Views/Shared/_Layout.cshtml` to include Font Awesome:
```html
<!-- Add this in the <head> section -->
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
```

### Step 19: Final Build and Test
```bash
# Clean any previous builds
dotnet clean

# Restore all packages
dotnet restore

# Build the application
dotnet build

# Apply migrations to ensure database is up to date
dotnet ef database update

# Run the application
dotnet run
```

---

## **Phase 10: Testing and Validation**

### Step 20: Comprehensive Testing Checklist

**Database and Models Testing:**
1. Verify SQLite database is created in project root
2. Check that sample data is seeded properly
3. Test model validation by entering invalid data
4. Verify relationships between Students and Grades work correctly

**CRUD Operations Testing:**
1. **Create Student**: Test adding new students with various data combinations
2. **Read Students**: Verify student list displays correctly with pagination
3. **Update Student**: Test editing student information
4. **Delete Student**: Test deletion with confirmation dialog

**Grade Management Testing:**
1. **Add Grades**: Test grade entry with letter grade calculation
2. **View Grades**: Verify grade statistics and display
3. **Grade Validation**: Test grade boundaries (0-100)
4. **Grade History**: Check chronological grade display

**Search and Navigation Testing:**
1. **Search Functionality**: Test searching by name, branch, section
2. **Navigation**: Test all breadcrumb and button navigation
3. **Responsive Design**: Test on different screen sizes
4. **Error Handling**: Test invalid URLs and missing data

**User Interface Testing:**
1. **Form Validation**: Test client-side and server-side validation
2. **Success Messages**: Verify TempData messages display correctly
3. **Bootstrap Components**: Test all UI components work properly
4. **Accessibility**: Check form labels and navigation

### Step 21: Performance and Security Considerations

**Performance Optimizations:**
- Entity Framework uses efficient Include() statements for related data
- Queries are optimized to select only necessary columns
- Async/await patterns prevent UI blocking
- Database indexes are automatically created for foreign keys

**Security Features:**
- Anti-forgery tokens prevent CSRF attacks
- Input validation prevents malicious data entry
- Parameterized queries prevent SQL injection
- HTML encoding prevents XSS attacks

**Best Practices Implemented:**
- Service layer separates business logic from controllers
- Repository pattern concepts through Entity Framework
- Dependency injection for loose coupling
- Proper error handling and logging

---

## **Phase 11: Advanced Features and Enhancements**

### Optional Enhancements You Can Add

**1. Photo Upload for Students**
```csharp
// Add to Student model
[Display(Name = "Profile Photo")]
public string? PhotoPath { get; set; }

// Add to controller
[HttpPost]
public async Task<IActionResult> UploadPhoto(int id, IFormFile photo)
{
    // Implement photo upload logic
}
```

**2. Grade Statistics Dashboard**
```csharp
// Add to StudentService
public async Task<GradeStatistics> GetGradeStatisticsAsync(int studentId)
{
    var grades = await _context.Grades
        .Where(g => g.StudentID == studentId)
        .ToListAsync();
    
    return new GradeStatistics
    {
        AverageGrade = grades.Average(g => g.GradeValue),
        HighestGrade = grades.Max(g => g.GradeValue),
        LowestGrade = grades.Min(g => g.GradeValue),
        TotalSubjects = grades.Select(g => g.Subject).Distinct().Count()
    };
}
```

**3. Export Functionality**
```csharp
// Add export methods
public async Task<IActionResult> ExportToExcel()
{
    // Implement Excel export using EPPlus
}

public async Task<IActionResult> ExportToPdf(int studentId)
{
    // Implement PDF export using iTextSharp
}
```

**4. Advanced Search Filters**
```html
<!-- Add advanced search form -->
<form asp-action="Index" method="get">
    <div class="row">
        <div class="col-md-3">
            <select name="branchFilter" class="form-select">
                <option value="">All Branches</option>
                <option value="Computer Science">Computer Science</option>
                <!-- More options -->
            </select>
        </div>
        <div class="col-md-3">
            <select name="sectionFilter" class="form-select">
                <option value="">All Sections</option>
                <!-- Section options -->
            </select>
        </div>
        <div class="col-md-3">
            <input type="date" name="enrollmentDateFrom" class="form-control" />
        </div>
        <div class="col-md-3">
            <button type="submit" class="btn btn-primary">Filter</button>
        </div>
    </div>
</form>
```

**5. Real-time Grade Analytics**
```javascript
// Add Chart.js for grade visualization
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<script>
// Implement grade trend charts
var ctx = document.getElementById('gradeChart').getContext('2d');
var gradeChart = new Chart(ctx, {
    type: 'line',
    data: {
        // Grade data over time
    },
    options: {
        responsive: true,
        // Chart configuration
    }
});
</script>
```

---

## Entity Framework Migrations

### Understanding Database Schema Management
This project demonstrates professional database management using Entity Framework Core migrations instead of the simpler `EnsureCreated()` approach.

### Migration Benefits
1. **Version Control**: Database schema changes are tracked in source control
2. **Team Collaboration**: Multiple developers can safely apply schema changes
3. **Production Deployment**: Safe, incremental database updates
4. **Rollback Capability**: Ability to revert database changes if needed

### Migration Commands Used in This Project
```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migrations to database
dotnet ef database update

# View migration status
dotnet ef migrations list

# Rollback to previous migration (if needed)
dotnet ef database update PreviousMigrationName

# Remove last migration (if not applied to database)
dotnet ef migrations remove
```

### Migration File Structure
After running the migration commands, you will see:
```
Migrations/
├── 20240702010000_InitialCreate.cs          # Migration implementation
├── 20240702010000_InitialCreate.Designer.cs # Migration metadata
└── ApplicationDbContextModelSnapshot.cs     # Current model state
```

### What the Migration Contains
1. **Schema Creation**: Tables, columns, constraints, and indexes
2. **Data Seeding**: Sample data for testing and demonstration
3. **Relationships**: Foreign key constraints and navigation properties
4. **Rollback Logic**: Down() method for undoing changes

### Production Deployment Considerations
```csharp
// In Program.cs - automatically applies migrations on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Safe for production
}
```

This approach ensures that:
- Database is always up-to-date with the latest schema
- Migrations are applied consistently across environments
- No data loss occurs during schema updates
- Multiple developers can work on database changes simultaneously

### Migration Troubleshooting
If you encounter migration issues:

**Problem**: Migration fails to apply
```bash
# Solution: Check migration status
dotnet ef migrations list

# Remove problematic migration and recreate
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Problem**: Database schema conflicts
```bash
# Solution: Reset database completely
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Problem**: Seed data conflicts
```bash
# Solution: Clear database and reapply migrations
rm studentmanagement.db
dotnet ef database update
```

---
