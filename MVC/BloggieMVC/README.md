# BloggieMVC - Complete Blog Management Platform

Welcome to **BloggieMVC** - a comprehensive blog management platform built with ASP.NET Core MVC, demonstrating modern web development practices and enterprise-level architecture patterns.

## Project Overview

This solution serves as a complete blogging platform featuring administrative capabilities, user authentication, content management, and interactive features. It implements the Model-View-Controller (MVC) architectural pattern with clean separation of concerns, making it an excellent learning resource for ASP.NET Core development.

## Architecture & Features

### Core Architecture
- **ASP.NET Core MVC 7.0** with clean architecture principles
- **Repository Pattern** for data access abstraction
- **Entity Framework Core** with Code First approach
- **ASP.NET Core Identity** for authentication and authorization
- **Dependency Injection** for loose coupling
- **Two-Database Architecture** (Application + Authentication)

### Key Features
- **Blog Management**: Complete CRUD operations for blog posts
- **Tag System**: Categorization and filtering capabilities
- **User Management**: Registration, login, and role-based access
- **Interactive Features**: Like/unlike posts and commenting system
- **Image Management**: Cloud-based image upload via Cloudinary
- **Admin Panel**: Comprehensive administrative interface
- **Responsive Design**: Bootstrap 5 integration for mobile-friendly UI

## Technology Stack

- **Backend**: ASP.NET Core 7.0 MVC
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Cloud Storage**: Cloudinary API
- **Frontend**: Razor Views with Bootstrap 5
- **Architecture**: Repository Pattern with Dependency Injection

## Prerequisites

Before building this project, ensure you have the following installed:

1. **.NET 7.0 SDK** or later
2. **SQL Server** (Express/LocalDB acceptable for development)
3. **Visual Studio 2022** or **Visual Studio Code** with C# extension
4. **SQL Server Management Studio** (optional, for database management)
5. **Cloudinary Account** (free tier available for image uploads)

## Complete Step-by-Step Build Guide

### Phase 1: Project Setup and Structure

#### Step 1: Create Solution and Project Structure
```powershell
# Create solution directory
mkdir BloggieMVC
cd BloggieMVC

# Create solution file
dotnet new sln -n Bloggie

# Create ASP.NET Core MVC project
dotnet new mvc -n Bloggie.Web

# Add project to solution
dotnet sln add Bloggie.Web/Bloggie.Web.csproj
```

#### Step 2: Install Required NuGet Packages
Navigate to the `Bloggie.Web` project directory and install dependencies:
```powershell
cd Bloggie.Web

# Entity Framework Core for SQL Server
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.3
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.3

# ASP.NET Core Identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 7.0.3

# Cloudinary for image management
dotnet add package CloudinaryDotNet --version 1.20.0
```

### Phase 2: Domain Models and Data Layer

#### Step 3: Create Domain Models
Create the following models in `Models/Domain/` directory:

**BlogPost.cs** - Main blog post entity:
```csharp
namespace Bloggie.Web.Models.Domain
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        // Navigation properties
        public ICollection<Tag> Tags { get; set; }
        public ICollection<BlogPostLike> Likes { get; set; }
        public ICollection<BlogPostComment> Comments { get; set; }
    }
}
```

**Tag.cs** - Blog categorization:
```csharp
namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
```

**BlogPostLike.cs** - User likes tracking:
```csharp
namespace Bloggie.Web.Models.Domain
{
    public class BlogPostLike
    {
        public Guid Id { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
```

**BlogPostComment.cs** - Comments system:
```csharp
namespace Bloggie.Web.Models.Domain
{
    public class BlogPostComment
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
```

#### Step 4: Create Database Contexts
Create two separate contexts in `Data/` directory:

**BloggieDbContext.cs** - Application data context:
```csharp
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        public DbSet<BlogPostComment> BlogPostComment { get; set; }
    }
}
```

**AuthDbContext.cs** - Authentication context with seeded data:
```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles (User, Admin, SuperAdmin)
            var adminRoleId = "37cc67e1-41ca-461c-bf34-2b5e62dbae32";
            var superAdminRoleId = "3cfd9eee-08cb-4da3-9e6f-c3166b50d3b0";
            var userRoleId = "a0cab2c3-6558-4a1c-be81-dfb39180da3d";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Seed SuperAdminUser
            var superAdminId = "472ba632-6133-44a1-b158-6c10bd7d850d";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Add All roles to SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
```

### Phase 3: Repository Pattern Implementation

#### Step 5: Create Repository Interfaces
Create repository interfaces in `Repositories/` directory:

**ITagRepository.cs**:
```csharp
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tag);
        Task<Tag?> DeleteAsync(Guid id);
    }
}
```

**IBlogPostRepository.cs**:
```csharp
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
```

#### Step 6: Implement Repository Classes
Create concrete implementations for each repository interface with full CRUD operations and Entity Framework integration.

### Phase 4: ViewModels and Controllers

#### Step 7: Create ViewModels
Create ViewModels in `Models/ViewModels/` directory for:
- `AddBlogPostRequest` - Blog post creation
- `EditBlogPostRequest` - Blog post editing
- `LoginViewModel` - User authentication
- `RegisterViewModel` - User registration
- `HomeViewModel` - Homepage data aggregation

#### Step 8: Implement Controllers
Create controllers for different functional areas:
- **HomeController** - Public homepage and blog listing
- **AdminBlogPostsController** - Blog management (Admin only)
- **AdminTagsController** - Tag management (Admin only)
- **AccountController** - User authentication
- **BlogsController** - Public blog viewing
- **ImagesController** - Image upload handling

### Phase 5: Configuration and Services

#### Step 9: Configure Services in Program.cs
```csharp
using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Database contexts
builder.Services.AddDbContext<BloggieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

// Identity configuration
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

// Password policy configuration
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Repository dependency injection
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRespository, CloudinaryImageRepository>();
builder.Services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>();
builder.Services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

#### Step 10: Configure Application Settings
Create `appsettings.json` with database connections and Cloudinary configuration:
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
    "BloggieDbConnectionString": "Server=(localdb)\\mssqllocaldb;Database=BloggieDb;Trusted_Connection=True;TrustServerCertificate=Yes",
    "BloggieAuthDbConnectionString": "Server=(localdb)\\mssqllocaldb;Database=BloggieAuthDb;Trusted_Connection=True;TrustServerCertificate=Yes"
  },
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

### Phase 6: Database Migration and Setup

#### Step 11: Create and Apply Migrations
```powershell
# Create initial migration for main database
dotnet ef migrations add "Initial Migration" --project Bloggie.Web

# Create migration for authentication database
dotnet ef migrations add "Creating Auth Db" --project Bloggie.Web --context AuthDbContext --output-dir Migrations/AuthDb

# Apply migrations to create databases
dotnet ef database update --project Bloggie.Web
dotnet ef database update --project Bloggie.Web --context AuthDbContext
```

### Phase 7: Views and User Interface

#### Step 12: Create Razor Views
Implement Razor views for all functionality:
- **Home/Index.cshtml** - Homepage with blog listing
- **AdminBlogPosts/** - Administrative blog management views
- **AdminTags/** - Tag management interface
- **Account/** - Login/Register forms
- **Blogs/** - Public blog viewing
- **Shared/_Layout.cshtml** - Common layout with navigation

### Phase 8: Testing and Deployment

#### Step 13: Test the Application
```powershell
# Run the application
dotnet run --project Bloggie.Web

# Access the application at https://localhost:7xxx
# Default admin credentials:
# Email: superadmin@bloggie.com
# Password: Superadmin@123
```

#### Step 14: Verify Core Functionality
Test the following features:
1. **User Registration and Login**
2. **Admin Panel Access** (Admin/SuperAdmin roles)
3. **Blog Post Creation, Editing, and Deletion**
4. **Tag Management**
5. **Image Upload** (requires Cloudinary configuration)
6. **Public Blog Viewing and Commenting**
7. **Like/Unlike Functionality**

## Database Schema Overview

The application uses two separate databases:

### BloggieDb (Application Data)
- **BlogPosts** - Main content entities
- **Tags** - Categorization system
- **BlogPostLike** - User interaction tracking
- **BlogPostComment** - Comment system

### BloggieAuthDb (Authentication)
- **AspNetUsers** - User accounts
- **AspNetRoles** - Role definitions (User, Admin, SuperAdmin)
- **AspNetUserRoles** - User-role relationships

## Security Features

- **Role-based Authorization** with three levels (User, Admin, SuperAdmin)
- **Password Policy Enforcement** with complexity requirements
- **Authentication via ASP.NET Core Identity**
- **CSRF Protection** on all forms
- **Secure Password Hashing** using Identity framework

## Cloudinary Integration Setup

1. **Create Cloudinary Account** at https://cloudinary.com
2. **Obtain API Credentials** from your dashboard
3. **Update appsettings.json** with your credentials
4. **Image Upload** will automatically work in admin panels

## Default Admin Account

The application seeds a default SuperAdmin account:
- **Email**: superadmin@bloggie.com
- **Password**: Superadmin@123
- **Roles**: User, Admin, SuperAdmin

## Learning Objectives and Outcomes

### Beginner Level Concepts
- **MVC Architecture Pattern**: Understanding separation of concerns between Models, Views, and Controllers
- **Razor View Engine**: Server-side rendering with C# embedded in HTML
- **Entity Framework Basics**: Code-first approach and database interactions
- **Dependency Injection**: Understanding IoC container and service registration
- **Basic CRUD Operations**: Create, Read, Update, Delete functionality

### Intermediate Level Concepts
- **Repository Pattern**: Abstraction layer for data access operations
- **Authentication & Authorization**: ASP.NET Core Identity implementation
- **Database Relationships**: One-to-many and many-to-many relationships
- **ViewModels and Data Transfer**: Separating domain models from view models
- **Form Handling and Validation**: Server-side validation and model binding

### Advanced Level Concepts
- **Multi-Database Architecture**: Separate contexts for different concerns
- **Role-Based Security**: Hierarchical access control implementation
- **Cloud Service Integration**: External API integration (Cloudinary)
- **Migration Management**: Database schema versioning and deployment
- **Clean Architecture Principles**: Maintainable and testable code structure

## Common Issues and Troubleshooting

### Database Connection Issues
```powershell
# If LocalDB is not available, use SQL Server Express
# Update connection strings to use SQL Server instance:
"Server=.\\SQLEXPRESS;Database=BloggieDb;Trusted_Connection=True;TrustServerCertificate=Yes"
```

### Migration Problems
```powershell
# If migrations fail, try dropping and recreating databases
dotnet ef database drop --project Bloggie.Web --force
dotnet ef database drop --project Bloggie.Web --context AuthDbContext --force

# Then reapply migrations
dotnet ef database update --project Bloggie.Web
dotnet ef database update --project Bloggie.Web --context AuthDbContext
```

### Cloudinary Configuration
- Image upload will work without Cloudinary but images won't be saved
- For production deployment, Cloudinary credentials are required
- Free tier provides sufficient storage for development and testing

## Deployment Considerations

### Environment Configuration
- **Development**: Use LocalDB or SQL Server Express
- **Production**: Configure Azure SQL Database or dedicated SQL Server
- **Secrets Management**: Use Azure Key Vault or environment variables for sensitive data

### Performance Optimization
- **Database Indexing**: Add indexes for frequently queried columns
- **Caching**: Implement response caching for static content
- **Image Optimization**: Use Cloudinary's automatic optimization features

## Extensions and Improvements

### Potential Enhancements
1. **Rich Text Editor**: Integrate WYSIWYG editor for blog content
2. **Search Functionality**: Implement full-text search across blog posts
3. **Email Notifications**: Send notifications for comments and likes
4. **Social Media Integration**: Share buttons and social login
5. **Analytics Dashboard**: Track views, likes, and user engagement
6. **REST API**: Create API endpoints for mobile app integration
7. **Real-time Features**: SignalR for live comments and notifications

### Performance Improvements
1. **Pagination**: Implement paging for large datasets
2. **Image Lazy Loading**: Optimize page load times
3. **Database Optimization**: Query optimization and indexing
4. **Caching Strategy**: Redis or in-memory caching implementation

## Related Learning Resources

### Official Documentation
- [ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)

### Additional Learning Topics
- **Clean Architecture**: Robert C. Martin's architectural principles
- **SOLID Principles**: Object-oriented design principles
- **Unit Testing**: xUnit and Moq for testing ASP.NET Core applications
- **Async Programming**: Understanding async/await patterns in web applications

## Contributing and Development

### Code Style Guidelines
- Follow C# naming conventions
- Use meaningful variable and method names
- Implement proper error handling and logging
- Write unit tests for business logic
- Comment complex algorithms and business rules

### Development Best Practices
- **Version Control**: Use Git with meaningful commit messages
- **Code Reviews**: Implement peer review process
- **Testing Strategy**: Unit tests, integration tests, and end-to-end tests
- **Documentation**: Keep README and inline documentation updated
- **Security**: Regular security audits and dependency updates

---

This project serves as a comprehensive learning resource for ASP.NET Core MVC development, demonstrating real-world application architecture and implementation patterns. The step-by-step guide ensures that developers can recreate this project from scratch while understanding each component's purpose and implementation details.

## Project File Structure
```
BloggieMVC/
├── Bloggie.sln
└── Bloggie.Web/
    ├── Controllers/
    │   ├── AccountController.cs
    │   ├── AdminBlogPostsController.cs
    │   ├── AdminTagsController.cs
    │   ├── AdminUsersController.cs
    │   ├── BlogPostLikeController.cs
    │   ├── BlogsController.cs
    │   ├── HomeController.cs
    │   └── ImagesController.cs
    ├── Data/
    │   ├── AuthDbContext.cs
    │   └── BloggieDbContext.cs
    ├── Models/
    │   ├── Domain/
    │   │   ├── BlogPost.cs
    │   │   ├── BlogPostComment.cs
    │   │   ├── BlogPostLike.cs
    │   │   └── Tag.cs
    │   └── ViewModels/
    │       ├── AddBlogPostRequest.cs
    │       ├── EditBlogPostRequest.cs
    │       ├── LoginViewModel.cs
    │       └── RegisterViewModel.cs
    ├── Repositories/
    │   ├── Interfaces/
    │   └── Implementations/
    ├── Views/
    │   ├── Home/
    │   ├── AdminBlogPosts/
    │   ├── AdminTags/
    │   ├── Account/
    │   ├── Blogs/
    │   └── Shared/
    ├── Migrations/
    ├── wwwroot/
    ├── Program.cs
    ├── appsettings.json
    └── Bloggie.Web.csproj
```


