# Bloggie.Web - ASP.NET Core MVC Blog Application

Welcome to the **Bloggie.Web** project! This is a feature-rich blogging platform built with ASP.NET Core MVC that demonstrates modern web development practices. 📚✨

## 🎯 Project Overview

This ASP.NET Core MVC application serves as a complete blogging platform with both public-facing blog functionality and administrative capabilities. It's designed to teach core web development concepts while building a real-world application.

## 🏗️ Project Architecture

### Domain Models
The application is built around these core entities:

```csharp
📄 BlogPost     // Main blog content with title, content, author, etc.
🏷️ Tag          // Categorization system for blog posts
❤️ BlogPostLike  // User interactions (likes/unlikes)
💬 BlogPostComment // User comments on blog posts
👤 User         // ASP.NET Core Identity users with roles
```

### Folder Structure
```
Bloggie.Web/
├── Controllers/         # MVC Controllers
│   ├── AdminBlogPostsController.cs    # Admin: Manage blog posts
│   ├── AdminTagsController.cs         # Admin: Manage tags
│   ├── AdminUsersController.cs        # Admin: Manage users
│   ├── AccountController.cs           # Authentication
│   ├── BlogsController.cs             # Public blog views
│   ├── BlogPostLikeController.cs      # Like/Unlike API
│   └── HomeController.cs              # Landing page
├── Data/                # Database Contexts
│   ├── AuthDbContext.cs               # Identity database
│   └── BloggieDbContext.cs            # Application database
├── Models/              # Data Models & ViewModels
│   ├── Domain/                        # Entity models
│   └── ViewModels/                    # UI-specific models
├── Repositories/        # Data Access Layer
│   ├── Interfaces/                    # Repository contracts
│   └── Implementations/               # Repository implementations
├── Views/               # Razor Templates
│   ├── Admin*/                        # Admin panel views
│   ├── Account/                       # Auth views
│   ├── Blogs/                         # Public blog views
│   ├── Home/                          # Landing pages
│   └── Shared/                        # Layout and partials
├── wwwroot/             # Static Assets
├── Migrations/          # EF Core Migrations
└── Program.cs           # Application entry point
```

## ⚡ Key Features

### 🌐 Public Features
- **Blog Reading**: Browse and read blog posts
- **Search & Filter**: Find posts by tags and content
- **User Interaction**: Like/unlike posts and add comments
- **Responsive Design**: Mobile-friendly interface
- **User Registration**: Create accounts and manage profiles

### 🔧 Administrative Features
- **Content Management**: Full CRUD operations for blog posts
- **Tag Management**: Create and organize tags
- **User Management**: Manage user accounts and roles
- **Media Upload**: Image upload with Cloudinary integration
- **Role-based Access**: Admin and SuperAdmin roles

## 🛠️ Technology Implementation

### Database Strategy
```csharp
// Two separate DbContexts for separation of concerns
AuthDbContext      // ASP.NET Core Identity (Users, Roles, Claims)
BloggieDbContext   // Application data (Posts, Tags, Likes, Comments)
```

### Repository Pattern
```csharp
// Clean data access with dependency injection
IRepository<T> → Repository<T>
IBlogPostRepository → BlogPostRepository
ITagRepository → TagRepository
// ... and more for each entity
```

### Authentication & Authorization
```csharp
// Role-based security
[Authorize(Roles = "Admin")]           // Admin only
[Authorize(Roles = "Admin,SuperAdmin")] // Multiple roles
[Authorize]                            // Any authenticated user
```

## 🚀 Getting Started

### 1. Database Setup
```bash
# Update both database contexts
dotnet ef database update --context BloggieDbContext
dotnet ef database update --context AuthDbContext
```

### 2. Configuration
Update your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "BloggieDbConnectionString": "Your SQL Server connection",
    "BloggieAuthDbConnectionString": "Your Auth database connection"
  },
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

### 3. Run the Application
```bash
dotnet run
```

### 4. Default Access
- **Public**: Browse to `/` to see the blog
- **Admin**: Register a user, then manually assign Admin role in database
- **Super Admin**: Manually assign SuperAdmin role in database

## 📋 Core Concepts Demonstrated

### 1. MVC Pattern
- **Models**: Domain entities and ViewModels
- **Views**: Razor templates with strongly-typed models
- **Controllers**: Handle HTTP requests and coordinate between Model and View

### 2. Entity Framework Core
```csharp
// Code-first approach with migrations
public class BlogPost
{
    public Guid Id { get; set; }
    public string Heading { get; set; }
    // Navigation properties
    public ICollection<Tag> Tags { get; set; }
    public ICollection<BlogPostLike> Likes { get; set; }
}
```

### 3. Dependency Injection
```csharp
// Program.cs configuration
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
```

### 4. Repository Pattern
```csharp
public interface IBlogPostRepository
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    Task<BlogPost?> GetAsync(Guid id);
    Task<BlogPost> AddAsync(BlogPost blogPost);
    // ... more methods
}
```

## 🎓 Learning Opportunities

### For Beginners
1. **Start with Models**: Understand the domain entities
2. **Explore Controllers**: See how HTTP requests are handled
3. **Study Views**: Learn Razor syntax and HTML helpers
4. **Database Basics**: Understand Entity Framework concepts

### For Intermediate Developers
1. **Repository Pattern**: Study the data access layer
2. **Authentication Flow**: Understand ASP.NET Core Identity
3. **Authorization**: Learn role-based security
4. **AJAX Integration**: Check the like/unlike functionality

### For Advanced Developers
1. **Architecture Decisions**: Analyze the separation of concerns
2. **Performance**: Study async/await patterns
3. **Security**: Review authentication and authorization
4. **Cloud Integration**: Understand Cloudinary integration

## 🔍 Key Files to Study

### Entry Point
- `Program.cs` - Application configuration and dependency injection

### Data Layer
- `BloggieDbContext.cs` - Entity Framework configuration
- `AuthDbContext.cs` - Identity framework setup

### Controllers (Study Order)
1. `HomeController.cs` - Simple controller basics
2. `BlogsController.cs` - Public functionality
3. `AdminBlogPostsController.cs` - CRUD operations
4. `AccountController.cs` - Authentication logic

### Models
- `Models/Domain/` - Core business entities
- `Models/ViewModels/` - UI-specific data transfer objects

## 🚨 Common Learning Challenges

### Database Relationships
```csharp
// Many-to-Many: BlogPost ↔ Tag
public ICollection<Tag> Tags { get; set; }

// One-to-Many: BlogPost → BlogPostLike
public ICollection<BlogPostLike> Likes { get; set; }
```

### Async Programming
```csharp
// Always use async/await for database operations
public async Task<IActionResult> Details(Guid id)
{
    var blogPost = await blogPostRepository.GetAsync(id);
    return View(blogPost);
}
```

## 💡 Next Steps for Learning

1. **Add Unit Tests**: Learn testing with xUnit
2. **API Development**: Add Web API controllers
3. **Real-time Features**: Implement SignalR for live comments
4. **Caching**: Add Redis or memory caching
5. **Logging**: Implement structured logging with Serilog

## 🏗️ Build This Project From Scratch - Step by Step Guide

Ready to build this entire project yourself? Follow this comprehensive guide to create the BloggieMVC application from the ground up!

### Phase 1: Project Setup and Foundation

#### Step 1: Create the Solution and Project
```bash
# Create solution
dotnet new sln -n BloggieMVC

# Create ASP.NET Core MVC project
dotnet new mvc -n Bloggie.Web

# Add project to solution
dotnet sln add Bloggie.Web/Bloggie.Web.csproj

# Navigate to project directory
cd Bloggie.Web
```

#### Step 2: Install Required NuGet Packages
```bash
# Entity Framework Core packages
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Identity packages
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI

# Cloudinary for image upload
dotnet add package CloudinaryDotNet

# Additional utilities
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
```

#### Step 3: Update Project File Structure
Create the following folders in your project:
```
Bloggie.Web/
├── Data/
├── Models/
│   ├── Domain/
│   └── ViewModels/
├── Repositories/
└── Migrations/
```

### Phase 2: Domain Models Creation

#### Step 4: Create Domain Models

**Create `Models/Domain/Tag.cs`:**
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

**Create `Models/Domain/BlogPost.cs`:**
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

        public ICollection<Tag> Tags { get; set; }
        public ICollection<BlogPostLike> Likes { get; set; }
        public ICollection<BlogPostComment> Comments { get; set; }
    }
}
```

**Create `Models/Domain/BlogPostLike.cs`:**
```csharp
namespace Bloggie.Web.Models.Domain
{
    public class BlogPostLike
    {
        public Guid Id { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
        
        public BlogPost BlogPost { get; set; }
    }
}
```

**Create `Models/Domain/BlogPostComment.cs`:**
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
        
        public BlogPost BlogPost { get; set; }
    }
}
```

### Phase 3: Database Context Setup

#### Step 5: Create Database Contexts

**Create `Data/BloggieDbContext.cs`:**
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

**Create `Data/AuthDbContext.cs`:**
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
            var adminRoleId = "cba30f97-38d8-4d7a-b4f5-6b3e62b3e123";
            var superAdminRoleId = "bcd83dd6-5a9b-4d7f-a5ce-8e2f4c7b9876";
            var userRoleId = "dfe82cc4-7b4a-4e8f-b7da-9f1a3d6c5432";

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

            // Seed SuperAdmin User
            var superAdminId = "e8f3c2a1-9b7d-4f5e-a3c8-1d6b9e4f2a7c";
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

            // Add all roles to SuperAdmin User
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

### Phase 4: Repository Pattern Implementation

#### Step 6: Create Repository Interfaces

**Create `Repositories/ITagRepository.cs`:**
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

**Create `Repositories/IBlogPostRepository.cs`:**
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

#### Step 7: Implement Repository Classes

**Create `Repositories/TagRepository.cs`:**
```csharp
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                bloggieDbContext.Tags.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await bloggieDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }
    }
}
```

**Create `Repositories/BlogPostRepository.cs`:**
```csharp
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await bloggieDbContext.BlogPosts.FindAsync(id);

            if (existingBlogPost != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlogPost);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await bloggieDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost != null)
            {
                existingBlogPost.Id = blogPost.Id;
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.Visible = blogPost.Visible;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Tags = blogPost.Tags;

                await bloggieDbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }
    }
}
```

### Phase 5: ViewModels Creation

#### Step 8: Create ViewModels

**Create `Models/ViewModels/AddTagRequest.cs`:**
```csharp
namespace Bloggie.Web.Models.ViewModels
{
    public class AddTagRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
```

**Create `Models/ViewModels/EditTagRequest.cs`:**
```csharp
namespace Bloggie.Web.Models.ViewModels
{
    public class EditTagRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
```

**Create `Models/ViewModels/AddBlogPostRequest.cs`:**
```csharp
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        // Display tags
        public IEnumerable<SelectListItem> Tags { get; set; }

        // Collect Tag
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
```

### Phase 6: Program.cs Configuration

#### Step 9: Configure Services in Program.cs
```csharp
using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BloggieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### Phase 7: Controllers Implementation

#### Step 10: Create Admin Controllers

**Create `Controllers/AdminTagsController.cs`:**
```csharp
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                // Show success notification
            }
            else
            {
                // Show error notification
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }

            // Show error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
```

### Phase 8: Database Migration and Setup

#### Step 11: Update appsettings.json
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
    "BloggieDbConnectionString": "Server=(localdb)\\mssqllocaldb;Database=BloggieDb;Trusted_Connection=true;MultipleActiveResultSets=true;",
    "BloggieAuthDbConnectionString": "Server=(localdb)\\mssqllocaldb;Database=BloggieAuthDb;Trusted_Connection=true;MultipleActiveResultSets=true;"
  },
  "Cloudinary": {
    "CloudName": "",
    "ApiKey": "",
    "ApiSecret": ""
  }
}
```

#### Step 12: Create and Run Migrations
```bash
# Create initial migration for BloggieDbContext
dotnet ef migrations add "Initial Migration" --context BloggieDbContext

# Create initial migration for AuthDbContext
dotnet ef migrations add "Creating Auth Db" --context AuthDbContext

# Update databases
dotnet ef database update --context BloggieDbContext
dotnet ef database update --context AuthDbContext
```

### Phase 9: Views and UI Implementation

#### Step 13: Create Basic Views Structure

Continue implementing:
1. **Admin Views**: Create views for tag management, blog post management
2. **Public Views**: Create views for displaying blog posts
3. **Authentication Views**: Login, Register pages
4. **Layout and Shared Views**: Master layout, navigation
5. **Additional Features**: Like functionality, comments, image upload

### Phase 10: Testing and Refinement

#### Step 14: Test Your Application
```bash
# Run the application
dotnet run

# Access admin features at:
# https://localhost:xxxx/AdminTags
# https://localhost:xxxx/AdminBlogPosts

# Login with:
# Email: superadmin@bloggie.com
# Password: Superadmin@123
```

## 🎯 Key Learning Points from This Build

1. **Project Structure**: Understand how to organize an MVC application
2. **Entity Framework**: Code-first approach with relationships
3. **Repository Pattern**: Clean data access layer implementation
4. **Identity Framework**: Authentication and authorization setup
5. **Dependency Injection**: Service registration and usage
6. **MVC Pattern**: Controllers, Views, and Models working together

This step-by-step guide gives you a solid foundation. Continue building upon this base by adding the remaining controllers, views, and features!

