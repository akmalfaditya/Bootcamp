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

This comprehensive guide will walk you through creating the BloggieMVC project from scratch. Each step includes detailed explanations to ensure understanding of the underlying concepts and implementation details.

### Phase 1: Project Foundation and Initial Setup

#### Step 1: Environment Preparation and Solution Creation

**Prerequisites Verification:**
Before beginning, ensure your development environment meets the following requirements:
- .NET 7.0 SDK or later installed
- SQL Server or SQL Server Express available
- Visual Studio 2022 or Visual Studio Code with C# extension
- Command-line interface (PowerShell/Terminal) access

**Create the Solution Structure:**
Execute the following commands to establish the project foundation:

```powershell
# Navigate to your desired development directory
cd C:\Development  # Adjust path according to your preference

# Create the main project directory
mkdir BloggieMVC
cd BloggieMVC

# Initialize the solution file - this will contain multiple projects
dotnet new sln -n Bloggie

# Create the main web application project using MVC template
dotnet new mvc -n Bloggie.Web

# Associate the web project with the solution for centralized management
dotnet sln add Bloggie.Web/Bloggie.Web.csproj

# Verify the solution structure
dotnet sln list
```

**Understanding the Structure:**
- **Solution file (.sln)**: Manages multiple related projects
- **MVC project**: Contains controllers, views, models, and configuration
- **Project file (.csproj)**: Defines dependencies and build settings

#### Step 2: Package Dependencies Installation and Configuration

Navigate to the web project directory and install the required NuGet packages. Each package serves a specific purpose in our application architecture:

```powershell
# Change to the web project directory
cd Bloggie.Web

# Entity Framework Core packages for SQL Server integration
# These packages enable database operations and migrations
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.3
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.3

# ASP.NET Core Identity for authentication and authorization
# Provides user management, role-based security, and authentication
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 7.0.3

# Cloudinary SDK for cloud-based image management
# Enables image upload, storage, and optimization
dotnet add package CloudinaryDotNet --version 1.20.0

# Verify package installation
dotnet list package
```

**Package Purpose Explanation:**
- **EntityFrameworkCore.SqlServer**: Enables SQL Server database connectivity
- **EntityFrameworkCore.Tools**: Provides migration and scaffolding commands
- **Identity.EntityFrameworkCore**: Integrates ASP.NET Core Identity with EF Core
- **CloudinaryDotNet**: Facilitates cloud-based image storage and manipulation

#### Step 3: Project Structure Organization

Create the necessary directory structure to organize your code effectively:

```powershell
# Create domain model directories
mkdir Models\Domain
mkdir Models\ViewModels

# Create data access layer directories
mkdir Data
mkdir Repositories

# Verify directory structure
tree /F
```

**Architecture Explanation:**
- **Models/Domain**: Contains business entities that represent database tables
- **Models/ViewModels**: Contains data transfer objects for view-controller communication
- **Data**: Houses database context classes and configurations
- **Repositories**: Implements data access patterns for clean separation of concerns

### Phase 2: Domain Models and Database Architecture

#### Step 4: Create Domain Models with Detailed Entity Relationships

Create comprehensive domain models that represent your application's core business entities. Each model includes detailed property annotations and relationship mappings.

**Create BlogPost.cs in Models/Domain/ directory:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.Domain
{
    /// <summary>
    /// Represents a blog post entity with all associated metadata and relationships
    /// This is the core content entity of the blogging platform
    /// </summary>
    public class BlogPost
    {
        /// <summary>
        /// Unique identifier for the blog post - using GUID for distributed systems compatibility
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Main heading/title displayed prominently on the blog post
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Heading { get; set; }

        /// <summary>
        /// SEO-optimized page title for browser tabs and search engines
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PageTitle { get; set; }

        /// <summary>
        /// Main content body of the blog post - supports HTML markup
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Brief description for previews and meta descriptions
        /// </summary>
        [Required]
        [StringLength(500)]
        public string ShortDescription { get; set; }

        /// <summary>
        /// URL to the featured image - stored in Cloudinary
        /// </summary>
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// SEO-friendly URL handle for the blog post (e.g., "my-first-blog-post")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UrlHandle { get; set; }

        /// <summary>
        /// Publication date and time for the blog post
        /// </summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// Author name or identifier for the blog post
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        /// <summary>
        /// Determines if the blog post is visible to public users
        /// </summary>
        public bool Visible { get; set; }

        // Navigation properties for Entity Framework relationships
        
        /// <summary>
        /// Collection of tags associated with this blog post (Many-to-Many relationship)
        /// </summary>
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Collection of likes received by this blog post (One-to-Many relationship)
        /// </summary>
        public ICollection<BlogPostLike> Likes { get; set; } = new List<BlogPostLike>();

        /// <summary>
        /// Collection of comments on this blog post (One-to-Many relationship)
        /// </summary>
        public ICollection<BlogPostComment> Comments { get; set; } = new List<BlogPostComment>();
    }
}
```

**Create Tag.cs in Models/Domain/ directory:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.Domain
{
    /// <summary>
    /// Represents a categorization tag that can be associated with multiple blog posts
    /// Implements Many-to-Many relationship with BlogPost entities
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Unique identifier for the tag
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Internal name of the tag (used in URLs and backend processing)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// User-friendly display name shown in the UI
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Collection of blog posts associated with this tag (Many-to-Many relationship)
        /// </summary>
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
```

**Create BlogPostLike.cs in Models/Domain/ directory:**

```csharp
namespace Bloggie.Web.Models.Domain
{
    /// <summary>
    /// Represents a user's like action on a specific blog post
    /// Tracks user engagement and enables like/unlike functionality
    /// </summary>
    public class BlogPostLike
    {
        /// <summary>
        /// Unique identifier for the like record
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Foreign key reference to the associated blog post
        /// </summary>
        public Guid BlogPostId { get; set; }

        /// <summary>
        /// Foreign key reference to the user who liked the post
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Timestamp when the like was recorded
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to the associated blog post
        /// </summary>
        public BlogPost? BlogPost { get; set; }
    }
}
```

**Create BlogPostComment.cs in Models/Domain/ directory:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.Domain
{
    /// <summary>
    /// Represents a user comment on a specific blog post
    /// Enables interactive discussion functionality
    /// </summary>
    public class BlogPostComment
    {
        /// <summary>
        /// Unique identifier for the comment
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Content of the user's comment
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Foreign key reference to the associated blog post
        /// </summary>
        public Guid BlogPostId { get; set; }

        /// <summary>
        /// Foreign key reference to the user who posted the comment
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Timestamp when the comment was posted
        /// </summary>
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Username of the commenter for display purposes
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        /// <summary>
        /// Navigation property to the associated blog post
        /// </summary>
        public BlogPost? BlogPost { get; set; }
    }
}
```

#### Step 5: Create Database Contexts with Advanced Configuration

Implement two separate database contexts following the separation of concerns principle. This approach isolates application data from authentication data, providing better security and maintainability.

**Create BloggieDbContext.cs in Data/ directory:**

```csharp
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    /// <summary>
    /// Primary database context for application-specific data
    /// Manages blog posts, tags, comments, and likes
    /// Separated from authentication context for security and maintainability
    /// </summary>
    public class BloggieDbContext : DbContext
    {
        /// <summary>
        /// Constructor accepting DbContextOptions for dependency injection
        /// Enables configuration of database connection and provider settings
        /// </summary>
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet representing the BlogPosts table in the database
        /// Provides CRUD operations for blog post entities
        /// </summary>
        public DbSet<BlogPost> BlogPosts { get; set; }

        /// <summary>
        /// DbSet representing the Tags table in the database
        /// Manages tag entities for blog categorization
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// DbSet representing the BlogPostLike table in the database
        /// Tracks user likes for blog posts
        /// </summary>
        public DbSet<BlogPostLike> BlogPostLike { get; set; }

        /// <summary>
        /// DbSet representing the BlogPostComment table in the database
        /// Manages user comments on blog posts
        /// </summary>
        public DbSet<BlogPostComment> BlogPostComment { get; set; }

        /// <summary>
        /// Configures entity relationships and database constraints
        /// Called automatically by Entity Framework during model creation
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Many-to-Many relationship between BlogPost and Tag
            modelBuilder.Entity<BlogPost>()
                .HasMany(bp => bp.Tags)
                .WithMany(t => t.BlogPosts)
                .UsingEntity(j => j.ToTable("BlogPostTags"));

            // Configure One-to-Many relationship between BlogPost and BlogPostLike
            modelBuilder.Entity<BlogPostLike>()
                .HasOne(bpl => bpl.BlogPost)
                .WithMany(bp => bp.Likes)
                .HasForeignKey(bpl => bpl.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure One-to-Many relationship between BlogPost and BlogPostComment
            modelBuilder.Entity<BlogPostComment>()
                .HasOne(bpc => bpc.BlogPost)
                .WithMany(bp => bp.Comments)
                .HasForeignKey(bpc => bpc.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure unique constraint for UrlHandle to prevent duplicates
            modelBuilder.Entity<BlogPost>()
                .HasIndex(bp => bp.UrlHandle)
                .IsUnique();

            // Configure unique constraint for Tag Name to prevent duplicates
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}
```

**Create AuthDbContext.cs in Data/ directory:**

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    /// <summary>
    /// Authentication-specific database context extending IdentityDbContext
    /// Manages user accounts, roles, and authentication-related data
    /// Includes pre-seeded administrative accounts and roles
    /// </summary>
    public class AuthDbContext : IdentityDbContext
    {
        /// <summary>
        /// Constructor accepting DbContextOptions for dependency injection
        /// Enables configuration of authentication database connection
        /// </summary>
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures entity relationships and seeds initial data
        /// Creates default roles and administrative user account
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define role identifiers for consistent referencing
            var adminRoleId = "37cc67e1-41ca-461c-bf34-2b5e62dbae32";
            var superAdminRoleId = "3cfd9eee-08cb-4da3-9e6f-c3166b50d3b0";
            var userRoleId = "a0cab2c3-6558-4a1c-be81-dfb39180da3d";

            // Seed application roles for role-based authorization
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Create default SuperAdmin user account
            var superAdminId = "472ba632-6133-44a1-b158-6c10bd7d850d";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "SUPERADMIN@BLOGGIE.COM",
                NormalizedUserName = "SUPERADMIN@BLOGGIE.COM",
                Id = superAdminId,
                EmailConfirmed = true
            };

            // Hash the password using ASP.NET Core Identity's password hasher
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Assign all roles to the SuperAdmin user
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

**Understanding the Database Design:**
- **Separation of Concerns**: Authentication data is isolated from application data
- **Role-Based Security**: Three-tier role system (User, Admin, SuperAdmin)
- **Entity Relationships**: Properly configured foreign keys and cascade behaviors
- **Data Integrity**: Unique constraints prevent duplicate URLs and tag names
- **Seeded Data**: Default administrative account for immediate system access

### Phase 3: Repository Pattern Implementation

The Repository pattern provides a clean abstraction layer between your business logic and data access code. This implementation enhances testability, maintainability, and follows SOLID principles.

#### Step 6: Create Repository Interfaces

Repository interfaces define contracts for data access operations, enabling dependency injection and facilitating unit testing through mock implementations.

**Create ITagRepository.cs in Repositories/ directory:**

```csharp
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    /// <summary>
    /// Repository interface for Tag entity operations
    /// Defines contract for tag-related data access methods
    /// Enables dependency injection and facilitates unit testing
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Retrieves all tags from the database asynchronously
        /// </summary>
        /// <returns>Collection of all tag entities</returns>
        Task<IEnumerable<Tag>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific tag by its unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the tag</param>
        /// <returns>Tag entity if found, null otherwise</returns>
        Task<Tag?> GetAsync(Guid id);

        /// <summary>
        /// Creates a new tag in the database
        /// </summary>
        /// <param name="tag">Tag entity to be created</param>
        /// <returns>Created tag entity with generated ID</returns>
        Task<Tag> AddAsync(Tag tag);

        /// <summary>
        /// Updates an existing tag in the database
        /// </summary>
        /// <param name="tag">Tag entity with updated information</param>
        /// <returns>Updated tag entity if successful, null if tag not found</returns>
        Task<Tag?> UpdateAsync(Tag tag);

        /// <summary>
        /// Deletes a tag from the database by its identifier
        /// </summary>
        /// <param name="id">Unique identifier of the tag to delete</param>
        /// <returns>Deleted tag entity if successful, null if tag not found</returns>
        Task<Tag?> DeleteAsync(Guid id);

        /// <summary>
        /// Checks if a tag with the specified name already exists
        /// </summary>
        /// <param name="name">Tag name to check for uniqueness</param>
        /// <returns>True if tag exists, false otherwise</returns>
        Task<bool> ExistsAsync(string name);
    }
}
```

**Create IBlogPostRepository.cs in Repositories/ directory:**

```csharp
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    /// <summary>
    /// Repository interface for BlogPost entity operations
    /// Provides comprehensive data access methods for blog post management
    /// Includes specialized methods for public blog viewing and admin management
    /// </summary>
    public interface IBlogPostRepository
    {
        /// <summary>
        /// Retrieves all blog posts with associated tags, likes, and comments
        /// Used primarily for administrative purposes
        /// </summary>
        /// <returns>Collection of all blog post entities with navigation properties</returns>
        Task<IEnumerable<BlogPost>> GetAllAsync();

        /// <summary>
        /// Retrieves only visible blog posts for public display
        /// Excludes draft or unpublished posts
        /// </summary>
        /// <returns>Collection of publicly visible blog posts</returns>
        Task<IEnumerable<BlogPost>> GetAllVisibleAsync();

        /// <summary>
        /// Retrieves a specific blog post by its unique identifier
        /// Includes all related entities (tags, likes, comments)
        /// </summary>
        /// <param name="id">Unique identifier of the blog post</param>
        /// <returns>Blog post entity if found, null otherwise</returns>
        Task<BlogPost?> GetAsync(Guid id);

        /// <summary>
        /// Retrieves a blog post by its SEO-friendly URL handle
        /// Used for public blog post viewing via clean URLs
        /// </summary>
        /// <param name="urlHandle">SEO-friendly URL handle</param>
        /// <returns>Blog post entity if found, null otherwise</returns>
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        /// <summary>
        /// Creates a new blog post in the database
        /// </summary>
        /// <param name="blogPost">Blog post entity to be created</param>
        /// <returns>Created blog post entity with generated ID</returns>
        Task<BlogPost> AddAsync(BlogPost blogPost);

        /// <summary>
        /// Updates an existing blog post in the database
        /// </summary>
        /// <param name="blogPost">Blog post entity with updated information</param>
        /// <returns>Updated blog post entity if successful, null if not found</returns>
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        /// <summary>
        /// Deletes a blog post from the database by its identifier
        /// </summary>
        /// <param name="id">Unique identifier of the blog post to delete</param>
        /// <returns>Deleted blog post entity if successful, null if not found</returns>
        Task<BlogPost?> DeleteAsync(Guid id);

        /// <summary>
        /// Checks if a blog post with the specified URL handle already exists
        /// </summary>
        /// <param name="urlHandle">URL handle to check for uniqueness</param>
        /// <returns>True if URL handle exists, false otherwise</returns>
        Task<bool> UrlHandleExistsAsync(string urlHandle);
    }
}
```

**Create additional repository interfaces for comprehensive functionality:**

**IBlogPostLikeRepository.cs:**

```csharp
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    /// <summary>
    /// Repository interface for BlogPostLike entity operations
    /// Manages user like interactions with blog posts
    /// </summary>
    public interface IBlogPostLikeRepository
    {
        /// <summary>
        /// Adds a like to a blog post
        /// </summary>
        /// <param name="blogPostLike">Like entity to be added</param>
        /// <returns>Created like entity</returns>
        Task<BlogPostLike> AddLikeAsync(BlogPostLike blogPostLike);

        /// <summary>
        /// Removes a like from a blog post
        /// </summary>
        /// <param name="blogPostId">ID of the blog post</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>True if like was removed, false otherwise</returns>
        Task<bool> RemoveLikeAsync(Guid blogPostId, Guid userId);

        /// <summary>
        /// Gets total likes count for a blog post
        /// </summary>
        /// <param name="blogPostId">ID of the blog post</param>
        /// <returns>Number of likes</returns>
        Task<int> GetLikesCountAsync(Guid blogPostId);

        /// <summary>
        /// Checks if a user has liked a specific blog post
        /// </summary>
        /// <param name="blogPostId">ID of the blog post</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>True if user has liked the post, false otherwise</returns>
        Task<bool> HasUserLikedAsync(Guid blogPostId, Guid userId);
    }
}
```

**IBlogPostCommentRepository.cs:**

```csharp
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    /// <summary>
    /// Repository interface for BlogPostComment entity operations
    /// Manages user comments on blog posts
    /// </summary>
    public interface IBlogPostCommentRepository
    {
        /// <summary>
        /// Adds a comment to a blog post
        /// </summary>
        /// <param name="comment">Comment entity to be added</param>
        /// <returns>Created comment entity</returns>
        Task<BlogPostComment> AddCommentAsync(BlogPostComment comment);

        /// <summary>
        /// Gets all comments for a specific blog post
        /// </summary>
        /// <param name="blogPostId">ID of the blog post</param>
        /// <returns>Collection of comments for the blog post</returns>
        Task<IEnumerable<BlogPostComment>> GetCommentsAsync(Guid blogPostId);

        /// <summary>
        /// Gets total comments count for a blog post
        /// </summary>
        /// <param name="blogPostId">ID of the blog post</param>
        /// <returns>Number of comments</returns>
        Task<int> GetCommentsCountAsync(Guid blogPostId);
    }
}
```

#### Step 7: Implement Repository Classes with Comprehensive Error Handling

Create concrete repository implementations that handle database operations efficiently and include proper error handling and logging.

**Create TagRepository.cs in Repositories/ directory:**

```csharp
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    /// <summary>
    /// Concrete implementation of ITagRepository
    /// Provides data access operations for Tag entities using Entity Framework Core
    /// Implements error handling and performance optimizations
    /// </summary>
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext _context;
        private readonly ILogger<TagRepository> _logger;

        /// <summary>
        /// Constructor with dependency injection for database context and logging
        /// </summary>
        /// <param name="context">Database context for data operations</param>
        /// <param name="logger">Logger for error tracking and debugging</param>
        public TagRepository(BloggieDbContext context, ILogger<TagRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all tags from the database with optimized querying
        /// </summary>
        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all tags from database");
                return await _context.Tags
                    .OrderBy(t => t.DisplayName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all tags");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific tag by ID with null safety
        /// </summary>
        public async Task<Tag?> GetAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving tag with ID: {TagId}", id);
                return await _context.Tags
                    .Include(t => t.BlogPosts)
                    .FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving tag with ID: {TagId}", id);
                throw;
            }
        }

        /// <summary>
        /// Creates a new tag with validation and conflict checking
        /// </summary>
        public async Task<Tag> AddAsync(Tag tag)
        {
            try
            {
                _logger.LogInformation("Adding new tag: {TagName}", tag.Name);
                
                // Check for existing tag with same name
                if (await ExistsAsync(tag.Name))
                {
                    throw new InvalidOperationException($"Tag with name '{tag.Name}' already exists");
                }

                tag.Id = Guid.NewGuid();
                await _context.Tags.AddAsync(tag);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully added tag with ID: {TagId}", tag.Id);
                return tag;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding tag: {TagName}", tag.Name);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing tag with optimistic concurrency handling
        /// </summary>
        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            try
            {
                _logger.LogInformation("Updating tag with ID: {TagId}", tag.Id);
                
                var existingTag = await _context.Tags.FindAsync(tag.Id);
                if (existingTag == null)
                {
                    _logger.LogWarning("Tag with ID {TagId} not found for update", tag.Id);
                    return null;
                }

                // Update properties
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully updated tag with ID: {TagId}", tag.Id);
                return existingTag;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating tag with ID: {TagId}", tag.Id);
                throw;
            }
        }

        /// <summary>
        /// Deletes a tag with cascade handling for related blog posts
        /// </summary>
        public async Task<Tag?> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting tag with ID: {TagId}", id);
                
                var tag = await _context.Tags
                    .Include(t => t.BlogPosts)
                    .FirstOrDefaultAsync(t => t.Id == id);
                
                if (tag == null)
                {
                    _logger.LogWarning("Tag with ID {TagId} not found for deletion", id);
                    return null;
                }

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully deleted tag with ID: {TagId}", id);
                return tag;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting tag with ID: {TagId}", id);
                throw;
            }
        }

        /// <summary>
        /// Checks if a tag with the specified name exists (case-insensitive)
        /// </summary>
        public async Task<bool> ExistsAsync(string name)
        {
            try
            {
                return await _context.Tags
                    .AnyAsync(t => t.Name.ToLower() == name.ToLower());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking tag existence for name: {TagName}", name);
                throw;
            }
        }
    }
}
```

**Create BlogPostRepository.cs in Repositories/ directory:**

```csharp
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    /// <summary>
    /// Concrete implementation of IBlogPostRepository
    /// Provides comprehensive data access operations for BlogPost entities
    /// Includes performance optimizations and relationship management
    /// </summary>
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext _context;
        private readonly ILogger<BlogPostRepository> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public BlogPostRepository(BloggieDbContext context, ILogger<BlogPostRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all blog posts with related entities for admin view
        /// </summary>
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all blog posts for admin view");
                return await _context.BlogPosts
                    .Include(bp => bp.Tags)
                    .Include(bp => bp.Likes)
                    .Include(bp => bp.Comments)
                    .OrderByDescending(bp => bp.PublishedDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all blog posts");
                throw;
            }
        }

        /// <summary>
        /// Retrieves only visible blog posts for public display
        /// </summary>
        public async Task<IEnumerable<BlogPost>> GetAllVisibleAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving visible blog posts for public view");
                return await _context.BlogPosts
                    .Include(bp => bp.Tags)
                    .Include(bp => bp.Likes)
                    .Include(bp => bp.Comments)
                    .Where(bp => bp.Visible)
                    .OrderByDescending(bp => bp.PublishedDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving visible blog posts");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a blog post by ID with all related entities
        /// </summary>
        public async Task<BlogPost?> GetAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving blog post with ID: {BlogPostId}", id);
                return await _context.BlogPosts
                    .Include(bp => bp.Tags)
                    .Include(bp => bp.Likes)
                    .Include(bp => bp.Comments.OrderByDescending(c => c.DateAdded))
                    .FirstOrDefaultAsync(bp => bp.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving blog post with ID: {BlogPostId}", id);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a blog post by URL handle for SEO-friendly routing
        /// </summary>
        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            try
            {
                _logger.LogInformation("Retrieving blog post with URL handle: {UrlHandle}", urlHandle);
                return await _context.BlogPosts
                    .Include(bp => bp.Tags)
                    .Include(bp => bp.Likes)
                    .Include(bp => bp.Comments.OrderByDescending(c => c.DateAdded))
                    .FirstOrDefaultAsync(bp => bp.UrlHandle == urlHandle && bp.Visible);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving blog post with URL handle: {UrlHandle}", urlHandle);
                throw;
            }
        }

        /// <summary>
        /// Creates a new blog post with validation
        /// </summary>
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            try
            {
                _logger.LogInformation("Adding new blog post: {BlogPostTitle}", blogPost.Heading);
                
                // Check for URL handle uniqueness
                if (await UrlHandleExistsAsync(blogPost.UrlHandle))
                {
                    throw new InvalidOperationException($"URL handle '{blogPost.UrlHandle}' already exists");
                }

                blogPost.Id = Guid.NewGuid();
                blogPost.PublishedDate = DateTime.UtcNow;
                
                await _context.BlogPosts.AddAsync(blogPost);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully added blog post with ID: {BlogPostId}", blogPost.Id);
                return blogPost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding blog post: {BlogPostTitle}", blogPost.Heading);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing blog post with tag relationship management
        /// </summary>
        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            try
            {
                _logger.LogInformation("Updating blog post with ID: {BlogPostId}", blogPost.Id);
                
                var existingBlogPost = await _context.BlogPosts
                    .Include(bp => bp.Tags)
                    .FirstOrDefaultAsync(bp => bp.Id == blogPost.Id);
                
                if (existingBlogPost == null)
                {
                    _logger.LogWarning("Blog post with ID {BlogPostId} not found for update", blogPost.Id);
                    return null;
                }

                // Update properties
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.Visible = blogPost.Visible;

                // Update tag relationships
                existingBlogPost.Tags.Clear();
                if (blogPost.Tags != null && blogPost.Tags.Any())
                {
                    foreach (var tag in blogPost.Tags)
                    {
                        var existingTag = await _context.Tags.FindAsync(tag.Id);
                        if (existingTag != null)
                        {
                            existingBlogPost.Tags.Add(existingTag);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully updated blog post with ID: {BlogPostId}", blogPost.Id);
                return existingBlogPost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating blog post with ID: {BlogPostId}", blogPost.Id);
                throw;
            }
        }

        /// <summary>
        /// Deletes a blog post with cascade handling
        /// </summary>
        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting blog post with ID: {BlogPostId}", id);
                
                var blogPost = await _context.BlogPosts
                    .Include(bp => bp.Tags)
                    .Include(bp => bp.Likes)
                    .Include(bp => bp.Comments)
                    .FirstOrDefaultAsync(bp => bp.Id == id);
                
                if (blogPost == null)
                {
                    _logger.LogWarning("Blog post with ID {BlogPostId} not found for deletion", id);
                    return null;
                }

                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully deleted blog post with ID: {BlogPostId}", id);
                return blogPost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting blog post with ID: {BlogPostId}", id);
                throw;
            }
        }

        /// <summary>
        /// Checks if URL handle exists for uniqueness validation
        /// </summary>
        public async Task<bool> UrlHandleExistsAsync(string urlHandle)
        {
            try
            {
                return await _context.BlogPosts
                    .AnyAsync(bp => bp.UrlHandle.ToLower() == urlHandle.ToLower());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking URL handle existence: {UrlHandle}", urlHandle);
                throw;
            }
        }
    }
}
```

**Understanding Repository Implementation:**
- **Error Handling**: Comprehensive try-catch blocks with logging
- **Performance**: Optimized queries with appropriate includes
- **Validation**: Business rule enforcement (uniqueness, existence checks)
- **Logging**: Detailed logging for debugging and monitoring
- **Relationships**: Proper handling of entity relationships and cascade operations

### Phase 4: ViewModels and Data Transfer Objects

ViewModels serve as intermediary objects between controllers and views, providing a clean separation between domain models and presentation logic. They enable proper data validation and ensure security by controlling what data is exposed to the client.

#### Step 8: Create Comprehensive ViewModels

Create ViewModels in `Models/ViewModels/` directory that facilitate secure data transfer and validation:

**Create AddBlogPostRequest.cs:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for creating new blog posts
    /// Provides validation and data binding for blog post creation form
    /// Separates form data from domain model for security and validation
    /// </summary>
    public class AddBlogPostRequest
    {
        /// <summary>
        /// Main heading of the blog post
        /// </summary>
        [Required(ErrorMessage = "Heading is required")]
        [StringLength(200, ErrorMessage = "Heading cannot exceed 200 characters")]
        [Display(Name = "Blog Post Heading")]
        public string Heading { get; set; } = string.Empty;

        /// <summary>
        /// SEO-optimized page title for browser and search engines
        /// </summary>
        [Required(ErrorMessage = "Page title is required")]
        [StringLength(100, ErrorMessage = "Page title cannot exceed 100 characters")]
        [Display(Name = "Page Title")]
        public string PageTitle { get; set; } = string.Empty;

        /// <summary>
        /// Main content of the blog post
        /// </summary>
        [Required(ErrorMessage = "Content is required")]
        [Display(Name = "Blog Content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Brief description for previews and meta descriptions
        /// </summary>
        [Required(ErrorMessage = "Short description is required")]
        [StringLength(500, ErrorMessage = "Short description cannot exceed 500 characters")]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } = string.Empty;

        /// <summary>
        /// URL to the featured image
        /// </summary>
        [Display(Name = "Featured Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// SEO-friendly URL handle
        /// </summary>
        [Required(ErrorMessage = "URL handle is required")]
        [StringLength(100, ErrorMessage = "URL handle cannot exceed 100 characters")]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "URL handle can only contain lowercase letters, numbers, and hyphens")]
        [Display(Name = "URL Handle")]
        public string UrlHandle { get; set; } = string.Empty;

        /// <summary>
        /// Author name for the blog post
        /// </summary>
        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
        [Display(Name = "Author")]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Visibility status of the blog post
        /// </summary>
        [Display(Name = "Visible to Public")]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Comma-separated list of tag names
        /// </summary>
        [Display(Name = "Tags (comma-separated)")]
        public string? Tags { get; set; }
    }
}
```

**Create EditBlogPostRequest.cs:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for editing existing blog posts
    /// Includes ID for update operations and maintains validation rules
    /// </summary>
    public class EditBlogPostRequest
    {
        /// <summary>
        /// Unique identifier of the blog post being edited
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Main heading of the blog post
        /// </summary>
        [Required(ErrorMessage = "Heading is required")]
        [StringLength(200, ErrorMessage = "Heading cannot exceed 200 characters")]
        [Display(Name = "Blog Post Heading")]
        public string Heading { get; set; } = string.Empty;

        /// <summary>
        /// SEO-optimized page title
        /// </summary>
        [Required(ErrorMessage = "Page title is required")]
        [StringLength(100, ErrorMessage = "Page title cannot exceed 100 characters")]
        [Display(Name = "Page Title")]
        public string PageTitle { get; set; } = string.Empty;

        /// <summary>
        /// Main content of the blog post
        /// </summary>
        [Required(ErrorMessage = "Content is required")]
        [Display(Name = "Blog Content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Brief description for previews
        /// </summary>
        [Required(ErrorMessage = "Short description is required")]
        [StringLength(500, ErrorMessage = "Short description cannot exceed 500 characters")]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } = string.Empty;

        /// <summary>
        /// URL to the featured image
        /// </summary>
        [Display(Name = "Featured Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// SEO-friendly URL handle
        /// </summary>
        [Required(ErrorMessage = "URL handle is required")]
        [StringLength(100, ErrorMessage = "URL handle cannot exceed 100 characters")]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "URL handle can only contain lowercase letters, numbers, and hyphens")]
        [Display(Name = "URL Handle")]
        public string UrlHandle { get; set; } = string.Empty;

        /// <summary>
        /// Author name for the blog post
        /// </summary>
        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
        [Display(Name = "Author")]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Visibility status of the blog post
        /// </summary>
        [Display(Name = "Visible to Public")]
        public bool Visible { get; set; }

        /// <summary>
        /// Comma-separated list of tag names
        /// </summary>
        [Display(Name = "Tags (comma-separated)")]
        public string? Tags { get; set; }

        /// <summary>
        /// Publication date for reference
        /// </summary>
        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; }
    }
}
```

**Create authentication ViewModels:**

**LoginViewModel.cs:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for user authentication
    /// Provides validation for login form data
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// User's email address for authentication
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's password for authentication
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Remember user login option
        /// </summary>
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Return URL after successful login
        /// </summary>
        public string? ReturnUrl { get; set; }
    }
}
```

**RegisterViewModel.cs:**

```csharp
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for user registration
    /// Includes comprehensive validation for new user accounts
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// User's email address - also serves as username
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's chosen password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Password confirmation for validation
        /// </summary>
        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
```

**Create HomeViewModel.cs for homepage data aggregation:**

```csharp
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for the homepage
    /// Aggregates data for the main landing page display
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Collection of blog posts to display on homepage
        /// </summary>
        public IEnumerable<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

        /// <summary>
        /// Collection of available tags for filtering
        /// </summary>
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Current search/filter term if any
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Selected tag for filtering if any
        /// </summary>
        public string? SelectedTag { get; set; }
    }
}
```

**Create BlogDetailsViewModel.cs for single blog post view:**

```csharp
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for individual blog post details page
    /// Includes blog post data and user interaction information
    /// </summary>
    public class BlogDetailsViewModel
    {
        /// <summary>
        /// The blog post being displayed
        /// </summary>
        public BlogPost BlogPost { get; set; } = new BlogPost();

        /// <summary>
        /// Total number of likes for this blog post
        /// </summary>
        public int TotalLikes { get; set; }

        /// <summary>
        /// Indicates if the current user has liked this post
        /// </summary>
        public bool HasUserLiked { get; set; }

        /// <summary>
        /// Total number of comments for this blog post
        /// </summary>
        public int TotalComments { get; set; }

        /// <summary>
        /// Comments for this blog post
        /// </summary>
        public IEnumerable<BlogPostComment> Comments { get; set; } = new List<BlogPostComment>();

        /// <summary>
        /// New comment being added (if user is authenticated)
        /// </summary>
        public string? NewComment { get; set; }

        /// <summary>
        /// Indicates if user is authenticated and can interact
        /// </summary>
        public bool IsAuthenticated { get; set; }
    }
}
```

**Understanding ViewModel Benefits:**
- **Security**: Prevents over-posting attacks by controlling exposed properties
- **Validation**: Centralized validation rules with user-friendly error messages
- **Separation**: Clean separation between domain models and presentation logic
- **Flexibility**: Different views can have specialized data requirements
- **Maintainability**: Changes to domain models don't directly affect views

#### Step 8: Implement Controllers
Create controllers for different functional areas:
- **HomeController** - Public homepage and blog listing
- **AdminBlogPostsController** - Blog management (Admin only)
- **AdminTagsController** - Tag management (Admin only)
- **AccountController** - User authentication
- **BlogsController** - Public blog viewing
- **ImagesController** - Image upload handling

### Phase 5: Configuration and Services

#### Step 9: Configure Services and Dependency Injection

Configure all services, database contexts, and dependencies in `Program.cs`. This centralizes application configuration and enables proper dependency injection throughout the application.

```csharp
using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// Configure MVC with comprehensive view support
builder.Services.AddControllersWithViews();

// Database Context Configuration
// Configure primary application database for blog data
builder.Services.AddDbContext<BloggieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

// Configure authentication database for user management
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

// ASP.NET Core Identity Configuration
// Configure authentication and authorization services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Configure password policy requirements
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Configure user account requirements
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false; // Set to true for production

    // Configure account lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// Cookie Authentication Configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// Repository Dependency Injection
// Register repository interfaces with their concrete implementations
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>();
builder.Services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();

// Additional Services
// Configure Cloudinary for image management (if using cloud storage)
// builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
// builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();

// Configure logging services
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Add session services for temporary data storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
// Handle different environments appropriately
if (!app.Environment.IsDevelopment())
{
    // Production error handling
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}
else
{
    // Development error page for detailed debugging
    app.UseDeveloperExceptionPage();
}

// Configure middleware pipeline in correct order
app.UseHttpsRedirection();      // Redirect HTTP to HTTPS
app.UseStaticFiles();           // Serve static files (CSS, JS, images)
app.UseRouting();               // Enable routing
app.UseSession();               // Enable session state
app.UseAuthentication();        // Enable authentication
app.UseAuthorization();         // Enable authorization

// Configure routing patterns
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ensure database is created and seeded (development only)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var authContext = services.GetRequiredService<AuthDbContext>();
            var bloggieContext = services.GetRequiredService<BloggieDbContext>();
            
            // Ensure databases are created
            authContext.Database.EnsureCreated();
            bloggieContext.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}

app.Run();
```

**Understanding Service Configuration:**
- **Database Contexts**: Two separate contexts for application and authentication data
- **Identity Configuration**: Comprehensive user authentication and authorization setup
- **Repository Pattern**: Dependency injection for repository interfaces
- **Security Configuration**: Password policies, cookie settings, and lockout policies
- **Middleware Pipeline**: Correctly ordered middleware for optimal security and performance
- **Environment-Specific**: Different configurations for development and production
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

#### Step 10: Create Application Configuration

Configure the application settings in `appsettings.json` and `appsettings.Development.json` to support different environments and external services.

**Update appsettings.json:**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "BloggieDbConnectionString": "Server=(localdb)\\mssqllocaldb;Database=BloggieDb;Trusted_Connection=True;TrustServerCertificate=Yes;MultipleActiveResultSets=true",
    "BloggieAuthDbConnectionString": "Server=(localdb)\\mssqllocaldb;Database=BloggieAuthDb;Trusted_Connection=True;TrustServerCertificate=Yes;MultipleActiveResultSets=true"
  },
  "Cloudinary": {
    "CloudName": "your-cloud-name-here",
    "ApiKey": "your-api-key-here",
    "ApiSecret": "your-api-secret-here"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderPassword": "your-app-password"
  }
}
```

**Create appsettings.Development.json for development-specific settings:**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "ConnectionStrings": {
    "BloggieDbConnectionString": "Server=(localdb)\\mssqllocaldb;Database=BloggieDb_Dev;Trusted_Connection=True;TrustServerCertificate=Yes;MultipleActiveResultSets=true",
    "BloggieAuthDbConnectionString": "Server=(localdb)\\mssqllocaldb;Database=BloggieAuthDb_Dev;Trusted_Connection=True;TrustServerCertificate=Yes;MultipleActiveResultSets=true"
  }
}
```

**Configuration Explanation:**
- **Logging Levels**: Different verbosity for development vs production
- **Database Connections**: Separate databases for development and production
- **External Services**: Cloudinary for image storage, email settings for notifications
- **Security**: Connection strings include appropriate security settings

### Phase 5: Database Migration and Setup

Database migrations provide version control for your database schema, enabling systematic updates and deployment across different environments.

#### Step 11: Create and Apply Database Migrations

Execute the following commands to create database migrations and apply them to your development environment:

```powershell
# Ensure you're in the project directory
cd Bloggie.Web

# Create initial migration for the main application database
# This will analyze your DbContext and create migration files
dotnet ef migrations add "InitialCreate" --project Bloggie.Web --context BloggieDbContext

# Create initial migration for the authentication database
# Using a separate output directory to organize auth-related migrations
dotnet ef migrations add "CreateAuthDatabase" --project Bloggie.Web --context AuthDbContext --output-dir Migrations/AuthDb

# Apply migrations to create the main application database
# This will execute the migration scripts against your database
dotnet ef database update --project Bloggie.Web --context BloggieDbContext

# Apply migrations to create the authentication database
# This will create tables for users, roles, and authentication data
dotnet ef database update --project Bloggie.Web --context AuthDbContext

# Verify database creation
# Connect to your SQL Server instance and verify that both databases exist:
# - BloggieDb (or BloggieDb_Dev for development)
# - BloggieAuthDb (or BloggieAuthDb_Dev for development)
```

**Migration Process Explanation:**
1. **Migration Files**: Generated code that represents database schema changes
2. **Up Methods**: Define changes to apply to the database
3. **Down Methods**: Define how to reverse the changes if needed
4. **Migration History**: Tracked in the database to know which migrations have been applied

**Troubleshooting Migration Issues:**

If you encounter migration problems, try these solutions:

```powershell
# If LocalDB is not available, check SQL Server services
services.msc
# Look for "SQL Server (MSSQLSERVER)" or "SQL Server (SQLEXPRESS)"

# Alternative connection string for SQL Server Express
# Update appsettings.json if LocalDB is not available:
"BloggieDbConnectionString": "Server=.\\SQLEXPRESS;Database=BloggieDb;Trusted_Connection=True;TrustServerCertificate=Yes"

# Drop and recreate databases if needed (development only)
dotnet ef database drop --project Bloggie.Web --context BloggieDbContext --force
dotnet ef database drop --project Bloggie.Web --context AuthDbContext --force

# Remove migration files if you need to start over
Remove-Item -Path "Migrations" -Recurse -Force

# Then recreate migrations and databases
dotnet ef migrations add "InitialCreate" --context BloggieDbContext
dotnet ef migrations add "CreateAuthDatabase" --context AuthDbContext --output-dir Migrations/AuthDb
dotnet ef database update --context BloggieDbContext
dotnet ef database update --context AuthDbContext
```

**Database Verification Steps:**

After successful migration, verify your database structure:

1. **Connect to SQL Server Management Studio** or use Visual Studio SQL Server Object Explorer
2. **Check BloggieDb database** contains these tables:
   - BlogPosts
   - Tags
   - BlogPostTags (junction table for many-to-many relationship)
   - BlogPostLike
   - BlogPostComment
   - __EFMigrationsHistory

3. **Check BloggieAuthDb database** contains these tables:
   - AspNetUsers
   - AspNetRoles
   - AspNetUserRoles
   - AspNetUserClaims
   - AspNetUserLogins
   - AspNetUserTokens
   - AspNetRoleClaims
   - __EFMigrationsHistory

4. **Verify seeded data** in BloggieAuthDb:
   - Three roles: User, Admin, SuperAdmin
   - One user: superadmin@bloggie.com with all three roles assigned

### Phase 6: Controllers Implementation

Controllers handle HTTP requests, coordinate between models and views, and implement the application's business logic flow. Each controller focuses on a specific domain area.

#### Step 12: Create Comprehensive Controllers

**Create HomeController.cs in Controllers/ directory:**

```csharp
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bloggie.Web.Controllers
{
    /// <summary>
    /// Handles requests for the public-facing homepage and blog listing
    /// Provides the main entry point for visitors to the blog
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public HomeController(
            ILogger<HomeController> logger, 
            IBlogPostRepository blogPostRepository, 
            ITagRepository tagRepository)
        {
            _logger = logger;
            _blogPostRepository = blogPostRepository;
            _tagRepository = tagRepository;
        }

        /// <summary>
        /// Displays the homepage with visible blog posts
        /// Supports filtering by tags and search terms
        /// </summary>
        public async Task<IActionResult> Index(string? searchTerm, string? tag)
        {
            try
            {
                _logger.LogInformation("Loading homepage with search: {SearchTerm}, tag: {Tag}", searchTerm, tag);

                // Get all visible blog posts
                var blogPosts = await _blogPostRepository.GetAllVisibleAsync();
                
                // Apply search filter if provided
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    blogPosts = blogPosts.Where(bp => 
                        bp.Heading.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        bp.ShortDescription.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        bp.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                }

                // Apply tag filter if provided
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    blogPosts = blogPosts.Where(bp => 
                        bp.Tags.Any(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase)));
                }

                // Get all tags for the filter dropdown
                var tags = await _tagRepository.GetAllAsync();

                // Create view model
                var viewModel = new HomeViewModel
                {
                    BlogPosts = blogPosts.OrderByDescending(bp => bp.PublishedDate),
                    Tags = tags.OrderBy(t => t.DisplayName),
                    SearchTerm = searchTerm,
                    SelectedTag = tag
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading homepage");
                return View("Error");
            }
        }

        /// <summary>
        /// Displays privacy policy page
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Handles application errors
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
```

**Create BlogsController.cs for public blog viewing:**

```csharp
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bloggie.Web.Controllers
{
    /// <summary>
    /// Handles public blog post viewing and interaction
    /// Provides functionality for reading blog posts, commenting, and liking
    /// </summary>
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;
        private readonly IBlogPostCommentRepository _blogPostCommentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<BlogsController> _logger;

        public BlogsController(
            IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            IBlogPostCommentRepository blogPostCommentRepository,
            UserManager<IdentityUser> userManager,
            ILogger<BlogsController> logger)
        {
            _blogPostRepository = blogPostRepository;
            _blogPostLikeRepository = blogPostLikeRepository;
            _blogPostCommentRepository = blogPostCommentRepository;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Displays a specific blog post by URL handle
        /// Includes like and comment functionality for authenticated users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(urlHandle))
                {
                    return NotFound();
                }

                _logger.LogInformation("Loading blog post with URL handle: {UrlHandle}", urlHandle);

                var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
                
                if (blogPost == null)
                {
                    _logger.LogWarning("Blog post not found for URL handle: {UrlHandle}", urlHandle);
                    return NotFound();
                }

                var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
                var userId = isAuthenticated ? Guid.Parse(_userManager.GetUserId(User) ?? Guid.Empty.ToString()) : Guid.Empty;

                // Get interaction data
                var totalLikes = await _blogPostLikeRepository.GetLikesCountAsync(blogPost.Id);
                var hasUserLiked = isAuthenticated && await _blogPostLikeRepository.HasUserLikedAsync(blogPost.Id, userId);
                var comments = await _blogPostCommentRepository.GetCommentsAsync(blogPost.Id);

                var viewModel = new BlogDetailsViewModel
                {
                    BlogPost = blogPost,
                    TotalLikes = totalLikes,
                    HasUserLiked = hasUserLiked,
                    Comments = comments,
                    TotalComments = comments.Count(),
                    IsAuthenticated = isAuthenticated
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading blog post: {UrlHandle}", urlHandle);
                return View("Error");
            }
        }

        /// <summary>
        /// Handles adding a comment to a blog post
        /// Requires user authentication
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(string urlHandle, string comment)
        {
            try
            {
                if (!User.Identity?.IsAuthenticated ?? true)
                {
                    return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", new { urlHandle }) });
                }

                if (string.IsNullOrWhiteSpace(comment))
                {
                    TempData["Error"] = "Comment cannot be empty.";
                    return RedirectToAction("Index", new { urlHandle });
                }

                var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
                if (blogPost == null)
                {
                    return NotFound();
                }

                var userId = Guid.Parse(_userManager.GetUserId(User) ?? Guid.Empty.ToString());
                var user = await _userManager.FindByIdAsync(userId.ToString());

                var blogComment = new BlogPostComment
                {
                    BlogPostId = blogPost.Id,
                    Description = comment.Trim(),
                    UserId = userId,
                    Username = user?.UserName ?? "Anonymous",
                    DateAdded = DateTime.UtcNow
                };

                await _blogPostCommentRepository.AddCommentAsync(blogComment);
                
                TempData["Success"] = "Comment added successfully!";
                return RedirectToAction("Index", new { urlHandle });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding comment to blog post: {UrlHandle}", urlHandle);
                TempData["Error"] = "An error occurred while adding your comment.";
                return RedirectToAction("Index", new { urlHandle });
            }
        }
    }
}
```

**Create AdminBlogPostsController.cs for administrative blog management:**

```csharp
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    /// <summary>
    /// Administrative controller for blog post management
    /// Restricted to users with Admin or SuperAdmin roles
    /// Provides full CRUD operations for blog posts
    /// </summary>
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<AdminBlogPostsController> _logger;

        public AdminBlogPostsController(
            IBlogPostRepository blogPostRepository,
            ITagRepository tagRepository,
            ILogger<AdminBlogPostsController> logger)
        {
            _blogPostRepository = blogPostRepository;
            _tagRepository = tagRepository;
            _logger = logger;
        }

        /// <summary>
        /// Displays list of all blog posts for administrative management
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                _logger.LogInformation("Loading blog posts list for admin");
                var blogPosts = await _blogPostRepository.GetAllAsync();
                return View(blogPosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading blog posts list");
                TempData["Error"] = "An error occurred while loading blog posts.";
                return View(new List<BlogPost>());
            }
        }

        /// <summary>
        /// Displays form for creating a new blog post
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                // Load tags for selection
                var tags = await _tagRepository.GetAllAsync();
                ViewBag.Tags = tags;
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading add blog post form");
                TempData["Error"] = "An error occurred while loading the form.";
                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// Processes the creation of a new blog post
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var tags = await _tagRepository.GetAllAsync();
                    ViewBag.Tags = tags;
                    return View(addBlogPostRequest);
                }

                // Check for URL handle uniqueness
                if (await _blogPostRepository.UrlHandleExistsAsync(addBlogPostRequest.UrlHandle))
                {
                    ModelState.AddModelError("UrlHandle", "This URL handle already exists. Please choose a different one.");
                    var tags = await _tagRepository.GetAllAsync();
                    ViewBag.Tags = tags;
                    return View(addBlogPostRequest);
                }

                // Map view model to domain model
                var blogPost = new BlogPost
                {
                    Heading = addBlogPostRequest.Heading,
                    PageTitle = addBlogPostRequest.PageTitle,
                    Content = addBlogPostRequest.Content,
                    ShortDescription = addBlogPostRequest.ShortDescription,
                    FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                    UrlHandle = addBlogPostRequest.UrlHandle.ToLower(),
                    Author = addBlogPostRequest.Author,
                    Visible = addBlogPostRequest.Visible,
                    PublishedDate = DateTime.UtcNow
                };

                // Process tags if provided
                if (!string.IsNullOrWhiteSpace(addBlogPostRequest.Tags))
                {
                    var tagNames = addBlogPostRequest.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(t => t.Trim().ToLower())
                                                        .Where(t => !string.IsNullOrEmpty(t))
                                                        .Distinct();

                    var selectedTags = new List<Tag>();
                    foreach (var tagName in tagNames)
                    {
                        var existingTag = (await _tagRepository.GetAllAsync())
                                        .FirstOrDefault(t => t.Name.ToLower() == tagName);
                        
                        if (existingTag != null)
                        {
                            selectedTags.Add(existingTag);
                        }
                    }
                    blogPost.Tags = selectedTags;
                }

                await _blogPostRepository.AddAsync(blogPost);
                
                TempData["Success"] = "Blog post created successfully!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating blog post: {Title}", addBlogPostRequest.Heading);
                TempData["Error"] = "An error occurred while creating the blog post.";
                
                var tags = await _tagRepository.GetAllAsync();
                ViewBag.Tags = tags;
                return View(addBlogPostRequest);
            }
        }

        /// <summary>
        /// Displays form for editing an existing blog post
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var blogPost = await _blogPostRepository.GetAsync(id);
                if (blogPost == null)
                {
                    TempData["Error"] = "Blog post not found.";
                    return RedirectToAction("List");
                }

                var tags = await _tagRepository.GetAllAsync();
                ViewBag.Tags = tags;

                // Map domain model to view model
                var editRequest = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    PublishedDate = blogPost.PublishedDate,
                    Tags = string.Join(", ", blogPost.Tags.Select(t => t.Name))
                };

                return View(editRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading edit form for blog post: {Id}", id);
                TempData["Error"] = "An error occurred while loading the blog post.";
                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// Processes the update of an existing blog post
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var tags = await _tagRepository.GetAllAsync();
                    ViewBag.Tags = tags;
                    return View(editBlogPostRequest);
                }

                // Map view model to domain model
                var blogPost = new BlogPost
                {
                    Id = editBlogPostRequest.Id,
                    Heading = editBlogPostRequest.Heading,
                    PageTitle = editBlogPostRequest.PageTitle,
                    Content = editBlogPostRequest.Content,
                    ShortDescription = editBlogPostRequest.ShortDescription,
                    FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                    UrlHandle = editBlogPostRequest.UrlHandle.ToLower(),
                    Author = editBlogPostRequest.Author,
                    Visible = editBlogPostRequest.Visible,
                    PublishedDate = editBlogPostRequest.PublishedDate
                };

                // Process tags
                if (!string.IsNullOrWhiteSpace(editBlogPostRequest.Tags))
                {
                    var tagNames = editBlogPostRequest.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                         .Select(t => t.Trim().ToLower())
                                                         .Where(t => !string.IsNullOrEmpty(t))
                                                         .Distinct();

                    var selectedTags = new List<Tag>();
                    foreach (var tagName in tagNames)
                    {
                        var existingTag = (await _tagRepository.GetAllAsync())
                                        .FirstOrDefault(t => t.Name.ToLower() == tagName);
                        
                        if (existingTag != null)
                        {
                            selectedTags.Add(existingTag);
                        }
                    }
                    blogPost.Tags = selectedTags;
                }

                var updatedBlogPost = await _blogPostRepository.UpdateAsync(blogPost);
                if (updatedBlogPost == null)
                {
                    TempData["Error"] = "Blog post not found.";
                    return RedirectToAction("List");
                }

                TempData["Success"] = "Blog post updated successfully!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating blog post: {Id}", editBlogPostRequest.Id);
                TempData["Error"] = "An error occurred while updating the blog post.";
                
                var tags = await _tagRepository.GetAllAsync();
                ViewBag.Tags = tags;
                return View(editBlogPostRequest);
            }
        }

        /// <summary>
        /// Deletes a blog post
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deletedBlogPost = await _blogPostRepository.DeleteAsync(id);
                if (deletedBlogPost == null)
                {
                    TempData["Error"] = "Blog post not found.";
                }
                else
                {
                    TempData["Success"] = "Blog post deleted successfully!";
                }

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting blog post: {Id}", id);
                TempData["Error"] = "An error occurred while deleting the blog post.";
                return RedirectToAction("List");
            }
        }
    }
}
```

**Understanding Controller Architecture:**
- **Separation of Concerns**: Each controller handles a specific domain area
- **Dependency Injection**: Controllers receive dependencies through constructor injection
- **Error Handling**: Comprehensive try-catch blocks with user-friendly error messages
- **Authorization**: Role-based access control for administrative functions
- **Logging**: Detailed logging for debugging and monitoring
- **Model Validation**: Server-side validation with proper error handling
- **User Experience**: Success and error messages displayed to users via TempData

### Phase 7: Razor Views Implementation

The Views represent the presentation layer of the MVC pattern, responsible for rendering the user interface. This section covers the creation of comprehensive Razor views for all application features.

#### Step 12: Create Shared Layout and Common Views

**Create _Layout.cshtml in Views/Shared/ directory:**

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Bloggie</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/Bloggie.Web.styles.css" asp-append-version="true" />
    <link href="https://cdn.jsdelivr.net/npm/summernote@0.8.18/dist/summernote.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark border-bottom box-shadow mb-3">
            <div class="container-fluid">
                <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">
                    <i class="fas fa-blog"></i> Bloggie
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" 
                        aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link" asp-area="" asp-controller="Home" asp-action="Index">
                                <i class="fas fa-home"></i> Home
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" asp-area="" asp-controller="Blogs" asp-action="Index">
                                <i class="fas fa-blog"></i> Blogs
                            </a>
                        </li>
                        @if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
                        {
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="#" id="adminDropdown" role="button" 
                                   data-bs-toggle="dropdown" aria-expanded="false">
                                    <i class="fas fa-cog"></i> Admin
                                </a>
                                <ul class="dropdown-menu" aria-labelledby="adminDropdown">
                                    <li><a class="dropdown-item" asp-controller="AdminBlogPosts" asp-action="Add">
                                        <i class="fas fa-plus"></i> Add Blog Post</a></li>
                                    <li><a class="dropdown-item" asp-controller="AdminBlogPosts" asp-action="List">
                                        <i class="fas fa-list"></i> All Blog Posts</a></li>
                                    <li><a class="dropdown-item" asp-controller="AdminTags" asp-action="Add">
                                        <i class="fas fa-tag"></i> Add Tag</a></li>
                                    <li><a class="dropdown-item" asp-controller="AdminTags" asp-action="List">
                                        <i class="fas fa-tags"></i> All Tags</a></li>
                                    @if (User.IsInRole("SuperAdmin"))
                                    {
                                        <li><hr class="dropdown-divider"></li>
                                        <li><a class="dropdown-item" asp-controller="AdminUsers" asp-action="List">
                                            <i class="fas fa-users"></i> Manage Users</a></li>
                                    }
                                </ul>
                            </li>
                        }
                    </ul>
                    <ul class="navbar-nav">
                        @if (User.Identity.IsAuthenticated)
                        {
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" 
                                   data-bs-toggle="dropdown" aria-expanded="false">
                                    <i class="fas fa-user"></i> @User.Identity.Name
                                </a>
                                <ul class="dropdown-menu" aria-labelledby="userDropdown">
                                    <li><a class="dropdown-item" asp-controller="Account" asp-action="Logout">
                                        <i class="fas fa-sign-out-alt"></i> Logout</a></li>
                                </ul>
                            </li>
                        }
                        else
                        {
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Account" asp-action="Register">
                                    <i class="fas fa-user-plus"></i> Register
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Account" asp-action="Login">
                                    <i class="fas fa-sign-in-alt"></i> Login
                                </a>
                            </li>
                        }
                    </ul>
                </div>
            </div>
        </nav>
    </header>

    <div class="container-fluid">
        <main role="main" class="pb-3">
            @if (TempData["Success"] != null)
            {
                <div class="alert alert-success alert-dismissible fade show" role="alert">
                    <i class="fas fa-check-circle"></i> @TempData["Success"]
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            }
            @if (TempData["Error"] != null)
            {
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                    <i class="fas fa-exclamation-circle"></i> @TempData["Error"]
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            }
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted bg-dark text-light">
        <div class="container">
            <div class="row py-4">
                <div class="col-md-6">
                    <h5><i class="fas fa-blog"></i> Bloggie</h5>
                    <p>A modern blog platform built with ASP.NET Core MVC, featuring user authentication, 
                       role-based access control, and content management capabilities.</p>
                </div>
                <div class="col-md-6">
                    <h6>Quick Links</h6>
                    <ul class="list-unstyled">
                        <li><a asp-controller="Home" asp-action="Index" class="text-light">Home</a></li>
                        <li><a asp-controller="Blogs" asp-action="Index" class="text-light">All Blogs</a></li>
                        @if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                        {
                            <li><a asp-controller="AdminBlogPosts" asp-action="List" class="text-light">Admin Panel</a></li>
                        }
                    </ul>
                </div>
            </div>
            <hr class="border-light">
            <div class="text-center">
                &copy; @DateTime.Now.Year - Bloggie. Built with ASP.NET Core MVC.
            </div>
        </div>
    </footer>

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    <script src="https://cdn.jsdelivr.net/npm/summernote@0.8.18/dist/summernote.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

**Create Home Views:**

**Views/Home/Index.cshtml:**

```html
@model IEnumerable<Bloggie.Web.Models.Domain.BlogPost>
@{
    ViewData["Title"] = "Welcome to Bloggie";
}

<div class="container-fluid p-0">
    <!-- Hero Section -->
    <section class="bg-primary text-white py-5 mb-5">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-8">
                    <h1 class="display-4 fw-bold mb-3">
                        <i class="fas fa-blog"></i> Welcome to Bloggie
                    </h1>
                    <p class="lead mb-4">
                        Discover amazing stories, tutorials, and insights from our community of writers. 
                        Join us on a journey of knowledge sharing and creative expression.
                    </p>
                    <div class="d-flex gap-3">
                        <a asp-controller="Blogs" asp-action="Index" class="btn btn-light btn-lg">
                            <i class="fas fa-book-open"></i> Explore Blogs
                        </a>
                        @if (!User.Identity.IsAuthenticated)
                        {
                            <a asp-controller="Account" asp-action="Register" class="btn btn-outline-light btn-lg">
                                <i class="fas fa-user-plus"></i> Join Community
                            </a>
                        }
                    </div>
                </div>
                <div class="col-lg-4 text-center">
                    <i class="fas fa-laptop-code display-1 text-white-50"></i>
                </div>
            </div>
        </div>
    </section>

    <!-- Featured Posts Section -->
    <div class="container">
        <div class="row mb-5">
            <div class="col-12">
                <h2 class="text-center mb-4">
                    <i class="fas fa-star text-warning"></i> Featured Posts
                </h2>
                <p class="text-center text-muted mb-5">Check out our latest and most popular blog posts</p>
            </div>
        </div>

        @if (Model != null && Model.Any())
        {
            <div class="row">
                @foreach (var blogPost in Model.Take(6))
                {
                    <div class="col-lg-4 col-md-6 mb-4">
                        <div class="card h-100 shadow-sm border-0">
                            @if (!string.IsNullOrWhiteSpace(blogPost.FeaturedImageUrl))
                            {
                                <img src="@blogPost.FeaturedImageUrl" class="card-img-top" 
                                     alt="@blogPost.Heading" style="height: 200px; object-fit: cover;">
                            }
                            else
                            {
                                <div class="card-img-top bg-light d-flex align-items-center justify-content-center" 
                                     style="height: 200px;">
                                    <i class="fas fa-image text-muted fa-3x"></i>
                                </div>
                            }
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title fw-bold">@blogPost.Heading</h5>
                                <p class="card-text text-muted flex-grow-1">
                                    @(blogPost.ShortDescription.Length > 100 ? 
                                      blogPost.ShortDescription.Substring(0, 100) + "..." : 
                                      blogPost.ShortDescription)
                                </p>
                                <div class="d-flex justify-content-between align-items-center mt-auto">
                                    <small class="text-muted">
                                        <i class="fas fa-user"></i> @blogPost.Author
                                    </small>
                                    <small class="text-muted">
                                        <i class="fas fa-calendar"></i> @blogPost.PublishedDate.ToString("MMM dd, yyyy")
                                    </small>
                                </div>
                                <div class="mt-2">
                                    @if (blogPost.Tags != null && blogPost.Tags.Any())
                                    {
                                        @foreach (var tag in blogPost.Tags.Take(3))
                                        {
                                            <span class="badge bg-secondary me-1">@tag.Name</span>
                                        }
                                    }
                                </div>
                                <a asp-controller="Blogs" asp-action="Index" asp-route-urlHandle="@blogPost.UrlHandle" 
                                   class="btn btn-primary mt-3">
                                    <i class="fas fa-arrow-right"></i> Read More
                                </a>
                            </div>
                        </div>
                    </div>
                }
            </div>

            <div class="text-center mt-4">
                <a asp-controller="Blogs" asp-action="Index" class="btn btn-outline-primary btn-lg">
                    <i class="fas fa-list"></i> View All Posts
                </a>
            </div>
        }
        else
        {
            <div class="text-center py-5">
                <i class="fas fa-blog fa-5x text-muted mb-3"></i>
                <h3 class="text-muted">No Posts Available</h3>
                <p class="text-muted">Be the first to create a blog post!</p>
                @if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
                {
                    <a asp-controller="AdminBlogPosts" asp-action="Add" class="btn btn-primary">
                        <i class="fas fa-plus"></i> Create First Post
                    </a>
                }
            </div>
        }

        <!-- Features Section -->
        <div class="row mt-5 py-5 bg-light rounded">
            <div class="col-12">
                <h3 class="text-center mb-5">Why Choose Bloggie?</h3>
            </div>
            <div class="col-lg-4 text-center mb-4">
                <i class="fas fa-shield-alt fa-3x text-primary mb-3"></i>
                <h5>Secure & Reliable</h5>
                <p class="text-muted">Built with enterprise-grade security and reliable hosting infrastructure.</p>
            </div>
            <div class="col-lg-4 text-center mb-4">
                <i class="fas fa-users fa-3x text-primary mb-3"></i>
                <h5>Community Driven</h5>
                <p class="text-muted">Connect with like-minded writers and readers in our vibrant community.</p>
            </div>
            <div class="col-lg-4 text-center mb-4">
                <i class="fas fa-mobile-alt fa-3x text-primary mb-3"></i>
                <h5>Mobile Friendly</h5>
                <p class="text-muted">Responsive design ensures great experience across all devices.</p>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        // Add any home page specific JavaScript here
        $(document).ready(function() {
            // Animate featured posts on scroll
            $(window).scroll(function() {
                $('.card').each(function() {
                    var elementTop = $(this).offset().top;
                    var elementBottom = elementTop + $(this).outerHeight();
                    var viewportTop = $(window).scrollTop();
                    var viewportBottom = viewportTop + $(window).height();
                    
                    if (elementBottom > viewportTop && elementTop < viewportBottom) {
                        $(this).addClass('animate__animated animate__fadeInUp');
                    }
                });
            });
        });
    </script>
}
```

**Create Blog Views:**

**Views/Blogs/Index.cshtml:**

```html
@model IEnumerable<Bloggie.Web.Models.Domain.BlogPost>
@{
    ViewData["Title"] = "All Blog Posts";
}

<div class="container">
    <div class="row">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h1 class="display-5 fw-bold">
                        <i class="fas fa-blog text-primary"></i> All Blog Posts
                    </h1>
                    <p class="text-muted">Discover amazing content from our community</p>
                </div>
                @if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
                {
                    <a asp-controller="AdminBlogPosts" asp-action="Add" class="btn btn-primary">
                        <i class="fas fa-plus"></i> New Post
                    </a>
                }
            </div>
        </div>
    </div>

    @if (Model != null && Model.Any())
    {
        <div class="row">
            @foreach (var blogPost in Model)
            {
                <div class="col-lg-6 col-xl-4 mb-4">
                    <article class="card h-100 shadow-sm border-0 blog-card">
                        @if (!string.IsNullOrWhiteSpace(blogPost.FeaturedImageUrl))
                        {
                            <div class="position-relative">
                                <img src="@blogPost.FeaturedImageUrl" class="card-img-top" 
                                     alt="@blogPost.Heading" style="height: 250px; object-fit: cover;">
                                <div class="position-absolute top-0 start-0 m-3">
                                    @if (blogPost.Tags != null && blogPost.Tags.Any())
                                    {
                                        <span class="badge bg-primary">@blogPost.Tags.First().Name</span>
                                    }
                                </div>
                            </div>
                        }
                        else
                        {
                            <div class="card-img-top bg-gradient-primary d-flex align-items-center justify-content-center text-white" 
                                 style="height: 250px;">
                                <div class="text-center">
                                    <i class="fas fa-blog fa-3x mb-2"></i>
                                    <p class="mb-0 fw-bold">@blogPost.Heading.Take(2).ToArray()</p>
                                </div>
                            </div>
                        }
                        
                        <div class="card-body d-flex flex-column">
                            <div class="mb-3">
                                <h5 class="card-title fw-bold mb-2">
                                    <a asp-controller="Blogs" asp-action="Index" 
                                       asp-route-urlHandle="@blogPost.UrlHandle" 
                                       class="text-decoration-none text-dark">
                                        @blogPost.Heading
                                    </a>
                                </h5>
                                <p class="card-text text-muted">
                                    @(blogPost.ShortDescription.Length > 120 ? 
                                      blogPost.ShortDescription.Substring(0, 120) + "..." : 
                                      blogPost.ShortDescription)
                                </p>
                            </div>
                            
                            <div class="mt-auto">
                                <div class="d-flex justify-content-between align-items-center mb-3">
                                    <div class="d-flex align-items-center">
                                        <i class="fas fa-user-circle fa-lg text-primary me-2"></i>
                                        <div>
                                            <small class="fw-bold">@blogPost.Author</small><br>
                                            <small class="text-muted">@blogPost.PublishedDate.ToString("MMM dd, yyyy")</small>
                                        </div>
                                    </div>
                                    <div class="text-end">
                                        @if (blogPost.Likes != null)
                                        {
                                            <small class="text-muted">
                                                <i class="fas fa-heart text-danger"></i> @blogPost.Likes.Count()
                                            </small>
                                        }
                                    </div>
                                </div>
                                
                                @if (blogPost.Tags != null && blogPost.Tags.Any())
                                {
                                    <div class="mb-3">
                                        @foreach (var tag in blogPost.Tags.Take(4))
                                        {
                                            <span class="badge bg-light text-dark border me-1 mb-1">
                                                <i class="fas fa-tag"></i> @tag.Name
                                            </span>
                                        }
                                    </div>
                                }
                                
                                <a asp-controller="Blogs" asp-action="Index" 
                                   asp-route-urlHandle="@blogPost.UrlHandle" 
                                   class="btn btn-primary w-100">
                                    <i class="fas fa-book-open"></i> Read Full Article
                                </a>
                            </div>
                        </div>
                    </article>
                </div>
            }
        </div>
        
        <!-- Pagination would go here if implemented -->
        <div class="row">
            <div class="col-12 text-center mt-4">
                <p class="text-muted">Showing @Model.Count() blog posts</p>
            </div>
        </div>
    }
    else
    {
        <div class="text-center py-5">
            <i class="fas fa-blog fa-5x text-muted mb-4"></i>
            <h2 class="text-muted mb-3">No Blog Posts Found</h2>
            <p class="text-muted mb-4">
                It looks like there are no blog posts available at the moment. 
                Be the first to share your thoughts!
            </p>
            @if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
            {
                <a asp-controller="AdminBlogPosts" asp-action="Add" class="btn btn-primary btn-lg">
                    <i class="fas fa-plus"></i> Create Your First Post
                </a>
            }
            else
            {
                <a asp-controller="Account" asp-action="Register" class="btn btn-outline-primary btn-lg">
                    <i class="fas fa-user-plus"></i> Join Our Community
                </a>
            }
        </div>
    }
</div>

@section Scripts {
    <script>
        $(document).ready(function() {
            // Add hover effects to blog cards
            $('.blog-card').hover(
                function() {
                    $(this).addClass('shadow-lg').css('transform', 'translateY(-5px)');
                },
                function() {
                    $(this).removeClass('shadow-lg').css('transform', 'translateY(0)');
                }
            );
        });
    </script>
}

<style>
    .blog-card {
        transition: all 0.3s ease;
        cursor: pointer;
    }
    
    .bg-gradient-primary {
        background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
    }
    
    .card-title a:hover {
        color: #007bff !important;
    }
</style>
```

#### Step 13: Create Admin Panel Views

**Create Views/AdminBlogPosts/List.cshtml:**

```html
@model IEnumerable<Bloggie.Web.Models.Domain.BlogPost>
@{
    ViewData["Title"] = "Manage Blog Posts";
}

<div class="container-fluid">
    <div class="row">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h1 class="display-6 fw-bold">
                        <i class="fas fa-cogs text-primary"></i> Manage Blog Posts
                    </h1>
                    <p class="text-muted">Create, edit, and manage all blog posts</p>
                </div>
                <a asp-controller="AdminBlogPosts" asp-action="Add" class="btn btn-primary btn-lg">
                    <i class="fas fa-plus"></i> Add New Post
                </a>
            </div>
        </div>
    </div>

    @if (Model != null && Model.Any())
    {
        <div class="card border-0 shadow">
            <div class="card-header bg-light">
                <div class="row align-items-center">
                    <div class="col">
                        <h5 class="mb-0">
                            <i class="fas fa-list"></i> All Blog Posts (@Model.Count() total)
                        </h5>
                    </div>
                    <div class="col-auto">
                        <div class="input-group">
                            <input type="text" id="searchInput" class="form-control" placeholder="Search posts...">
                            <button class="btn btn-outline-secondary" type="button">
                                <i class="fas fa-search"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body p-0">
                <div class="table-responsive">
                    <table class="table table-hover mb-0" id="blogPostsTable">
                        <thead class="table-dark">
                            <tr>
                                <th scope="col">
                                    <i class="fas fa-image"></i> Image
                                </th>
                                <th scope="col">
                                    <i class="fas fa-heading"></i> Title
                                </th>
                                <th scope="col">
                                    <i class="fas fa-user"></i> Author
                                </th>
                                <th scope="col">
                                    <i class="fas fa-calendar"></i> Published
                                </th>
                                <th scope="col">
                                    <i class="fas fa-eye"></i> Status
                                </th>
                                <th scope="col">
                                    <i class="fas fa-tags"></i> Tags
                                </th>
                                <th scope="col">
                                    <i class="fas fa-heart"></i> Likes
                                </th>
                                <th scope="col" class="text-center">
                                    <i class="fas fa-tools"></i> Actions
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var blogPost in Model)
                            {
                                <tr>
                                    <td>
                                        @if (!string.IsNullOrWhiteSpace(blogPost.FeaturedImageUrl))
                                        {
                                            <img src="@blogPost.FeaturedImageUrl" alt="@blogPost.Heading" 
                                                 class="rounded" style="width: 60px; height: 40px; object-fit: cover;">
                                        }
                                        else
                                        {
                                            <div class="bg-light rounded d-flex align-items-center justify-content-center" 
                                                 style="width: 60px; height: 40px;">
                                                <i class="fas fa-image text-muted"></i>
                                            </div>
                                        }
                                    </td>
                                    <td>
                                        <div>
                                            <h6 class="mb-1 fw-bold">@blogPost.Heading</h6>
                                            <small class="text-muted">@blogPost.UrlHandle</small>
                                        </div>
                                    </td>
                                    <td>
                                        <span class="badge bg-info">@blogPost.Author</span>
                                    </td>
                                    <td>
                                        <small>@blogPost.PublishedDate.ToString("MMM dd, yyyy")</small>
                                    </td>
                                    <td>
                                        @if (blogPost.Visible)
                                        {
                                            <span class="badge bg-success">
                                                <i class="fas fa-eye"></i> Visible
                                            </span>
                                        }
                                        else
                                        {
                                            <span class="badge bg-warning">
                                                <i class="fas fa-eye-slash"></i> Hidden
                                            </span>
                                        }
                                    </td>
                                    <td>
                                        @if (blogPost.Tags != null && blogPost.Tags.Any())
                                        {
                                            @foreach (var tag in blogPost.Tags.Take(2))
                                            {
                                                <span class="badge bg-secondary me-1">@tag.Name</span>
                                            }
                                            @if (blogPost.Tags.Count() > 2)
                                            {
                                                <span class="text-muted">+@(blogPost.Tags.Count() - 2) more</span>
                                            }
                                        }
                                        else
                                        {
                                            <span class="text-muted">No tags</span>
                                        }
                                    </td>
                                    <td>
                                        <span class="badge bg-danger">
                                            @(blogPost.Likes?.Count() ?? 0)
                                        </span>
                                    </td>
                                    <td class="text-center">
                                        <div class="btn-group" role="group">
                                            <a asp-controller="Blogs" asp-action="Index" 
                                               asp-route-urlHandle="@blogPost.UrlHandle" 
                                               class="btn btn-sm btn-outline-primary" 
                                               title="View Post" target="_blank">
                                                <i class="fas fa-external-link-alt"></i>
                                            </a>
                                            <a asp-controller="AdminBlogPosts" asp-action="Edit" 
                                               asp-route-id="@blogPost.Id" 
                                               class="btn btn-sm btn-outline-warning" 
                                               title="Edit Post">
                                                <i class="fas fa-edit"></i>
                                            </a>
                                            <button type="button" class="btn btn-sm btn-outline-danger" 
                                                    data-bs-toggle="modal" 
                                                    data-bs-target="#deleteModal-@blogPost.Id" 
                                                    title="Delete Post">
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
        </div>
    }
    else
    {
        <div class="text-center py-5">
            <i class="fas fa-blog fa-5x text-muted mb-4"></i>
            <h2 class="text-muted mb-3">No Blog Posts Found</h2>
            <p class="text-muted mb-4">Start creating amazing content for your blog!</p>
            <a asp-controller="AdminBlogPosts" asp-action="Add" class="btn btn-primary btn-lg">
                <i class="fas fa-plus"></i> Create Your First Post
            </a>
        </div>
    }
</div>

<!-- Delete Confirmation Modals -->
@if (Model != null && Model.Any())
{
    @foreach (var blogPost in Model)
    {
        <div class="modal fade" id="deleteModal-@blogPost.Id" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <i class="fas fa-exclamation-triangle text-warning"></i> Confirm Deletion
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <p>Are you sure you want to delete the blog post:</p>
                        <strong>"@blogPost.Heading"</strong>
                        <p class="text-danger mt-2">This action cannot be undone.</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                            <i class="fas fa-times"></i> Cancel
                        </button>
                        <form asp-controller="AdminBlogPosts" asp-action="Delete" 
                              asp-route-id="@blogPost.Id" method="post" class="d-inline">
                            @Html.AntiForgeryToken()
                            <button type="submit" class="btn btn-danger">
                                <i class="fas fa-trash"></i> Delete
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    }
}

@section Scripts {
    <script>
        $(document).ready(function() {
            // Search functionality
            $('#searchInput').on('keyup', function() {
                var value = $(this).val().toLowerCase();
                $('#blogPostsTable tbody tr').filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                });
            });

            // Auto-hide alerts after 5 seconds
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
}
```

### Phase 8: Testing and Deployment

#### Step 14: Test the Application
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


