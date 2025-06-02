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

