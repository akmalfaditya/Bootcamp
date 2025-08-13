# NZWalks Solution - Complete Step-by-Step Tutorial

## Overview

Welcome to the **NZWalks Solution** - a comprehensive demonstration of modern .NET 8 development practices, showcasing the implementation of both a robust Web API and an interactive web application for managing New Zealand walking trails and regions.

## Project Architecture

This solution exemplifies enterprise-level architecture patterns and serves as an educational platform for understanding full-stack .NET development. The solution consists of two interconnected projects that work together to provide a complete web application ecosystem.

### **NZWalks.API** - RESTful Web API
A professionally architected ASP.NET Core Web API serving as the backend infrastructure for managing New Zealand walking trails, geographical regions, and difficulty classifications. This API demonstrates industry-standard practices for building scalable, secure, and maintainable web services.

### **NZWalks.UI** - Web Application Frontend
An ASP.NET Core MVC web application providing an intuitive, responsive user interface for region management and seamless interaction with the backend API. This component showcases modern frontend development techniques within the .NET ecosystem.

## Key Features and Capabilities

- **Complete CRUD Operations** for Regions, Walks, and Difficulties with full data validation
- **JWT-based Authentication** with comprehensive role-based authorization system
- **Entity Framework Core** integration with SQL Server and advanced migration management
- **AutoMapper** implementation for efficient object-to-object mapping
- **Image Upload Functionality** with secure local storage and file management
- **Comprehensive Logging System** using Serilog with structured logging capabilities
- **RESTful API Design** following industry standards with proper HTTP status codes
- **Custom Action Filters** for request validation and response handling
- **Responsive UI Design** with Bootstrap 5 integration for optimal user experience
- **Error Handling Middleware** with centralized exception management
- **API Documentation** with Swagger/OpenAPI specifications

## Technology Stack

- **.NET 8.0** - Latest long-term support framework with enhanced performance
- **ASP.NET Core Web API** - High-performance web API framework
- **ASP.NET Core MVC** - Model-View-Controller pattern implementation
- **Entity Framework Core** - Object-relational mapping and database management
- **SQL Server** - Enterprise-grade relational database management system
- **AutoMapper** - Sophisticated object-to-object mapping library
- **JWT (JSON Web Tokens)** - Secure, stateless authentication mechanism
- **Serilog** - Structured logging framework with multiple output sinks
- **Bootstrap 5** - Modern CSS framework for responsive design
- **Swagger/OpenAPI** - API documentation and testing interface

## Domain Model Architecture

The application manages four core business entities with well-defined relationships:

### **1. Regions**
- **Purpose**: Represents New Zealand geographical regions (Auckland, Wellington, Canterbury, etc.)
- **Properties**: Unique identifier, region code, descriptive name, optional image URL
- **Relationships**: One-to-many with Walks

### **2. Walks**
- **Purpose**: Individual walking trails with comprehensive details
- **Properties**: Name, description, length, difficulty level, region association, image management
- **Relationships**: Many-to-one with Region, many-to-one with Difficulty

### **3. Difficulties**
- **Purpose**: Standardized trail difficulty classifications
- **Properties**: Unique identifier, difficulty name (Easy, Medium, Hard, Extreme)
- **Relationships**: One-to-many with Walks

### **4. Images**
- **Purpose**: File upload management for walks and regions
- **Properties**: File metadata, storage paths, content validation
- **Functionality**: Secure upload, validation, and retrieval mechanisms

## Learning Objectives and Outcomes

This comprehensive solution serves as an exceptional educational resource for mastering:

### **Backend Development Mastery**
- **RESTful API Development** with industry best practices
- **Clean Architecture** principles and implementation patterns
- **Entity Framework Core** advanced features including migrations, relationships, and performance optimization
- **Authentication & Authorization** using modern JWT implementation
- **Dependency Injection** and inversion of control principles
- **Middleware Development** for cross-cutting concerns

### **Frontend Development Excellence**
- **MVC Pattern Implementation** with proper separation of concerns
- **Responsive Web Design** using Bootstrap framework
- **API Integration** from frontend applications
- **Form Handling and Validation** with server-side integration
- **User Experience Design** principles and implementation

### **Full-Stack Integration**
- **End-to-end application development** workflow
- **API consumption** from web applications
- **Security implementation** across application layers
- **Error handling strategies** for robust applications
- **Modern web development** patterns and practices

## Prerequisites and Environment Setup

Before beginning this tutorial, ensure you have the following development environment configured:

### **Required Software Components**
1. **.NET 8.0 SDK** or later - [Download from Microsoft](https://dotnet.microsoft.com/download)
2. **SQL Server** (Express/LocalDB acceptable for development) - [Download SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
3. **Visual Studio 2022** (Community/Professional/Enterprise) or **Visual Studio Code** with C# extension
4. **SQL Server Management Studio** (optional but recommended for database management)
5. **Postman** or similar API testing tool (optional but helpful for API testing)

### **Recommended Development Tools**
- **Git** for version control
- **Windows Terminal** or **PowerShell** for command-line operations
- **Browser Developer Tools** for frontend debugging
- **Entity Framework Core Tools** for migration management

## Complete Step-by-Step Build Guide

### Phase 1: Solution and Project Creation

#### Step 1: Create Solution Structure
Begin by establishing the foundational solution architecture that will house both API and UI projects.

```powershell
# Create main solution directory
mkdir NZWalks-Solution
cd NZWalks-Solution

# Create blank solution file
dotnet new sln -n NZWalks

# Create Web API project
dotnet new webapi -n NZWalks.API

# Create MVC web application project
dotnet new mvc -n NZWalks.UI

# Add projects to solution
dotnet sln add NZWalks.API/NZWalks.API.csproj
dotnet sln add NZWalks.UI/NZWalks.UI.csproj
```

**Understanding the Structure:**
- The solution file (`.sln`) acts as a container for multiple related projects
- The Web API project will serve as our backend service layer
- The MVC project will provide our user-facing web application
- This separation allows for independent development, testing, and deployment

#### Step 2: Configure NuGet Packages for API Project
Navigate to the API project and install essential dependencies that form the foundation of our enterprise-grade web API.

```powershell
cd NZWalks.API

# Entity Framework Core for SQL Server integration
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.6
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.6

# AutoMapper for object-to-object mapping
dotnet add package AutoMapper --version 13.0.1

# JWT Authentication packages
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.6
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.6
dotnet add package Microsoft.IdentityModel.Tokens --version 7.6.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.6.0

# Serilog for comprehensive logging
dotnet add package Serilog --version 4.0.0
dotnet add package Serilog.AspNetCore --version 8.0.1
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0

# Swagger for API documentation
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.6
```

**Package Explanation:**
- **Entity Framework Core**: Provides object-relational mapping and database management capabilities
- **AutoMapper**: Eliminates boilerplate code for object mapping between different models
- **JWT Authentication**: Implements secure, stateless authentication mechanism
- **Serilog**: Offers structured logging with multiple output destinations
- **Swagger**: Generates interactive API documentation and testing interface

### Phase 2: Domain Models and Data Layer

#### Step 3: Create Domain Models
Establish the core business entities that represent the application's data structure and business logic.

**Create Models/Domain/Region.cs:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Domain
{
    /// <summary>
    /// Represents a geographical region in New Zealand
    /// Contains information about distinct areas where walks can be located
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Unique identifier for the region
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Short code identifier for the region (e.g., "AKL" for Auckland)
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 2)]
        public string Code { get; set; }

        /// <summary>
        /// Full descriptive name of the region (e.g., "Auckland Region")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Optional URL to an image representing the region
        /// </summary>
        [Url]
        public string? RegionImageUrl { get; set; }

        /// <summary>
        /// Navigation property for walks located in this region
        /// </summary>
        public virtual ICollection<Walk> Walks { get; set; } = new List<Walk>();
    }
}
```

**Create Models/Domain/Difficulty.cs:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Domain
{
    /// <summary>
    /// Represents the difficulty level classification for walking trails
    /// Provides standardized difficulty ratings for user guidance
    /// </summary>
    public class Difficulty
    {
        /// <summary>
        /// Unique identifier for the difficulty level
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the difficulty level (Easy, Medium, Hard, Extreme)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Navigation property for walks with this difficulty level
        /// </summary>
        public virtual ICollection<Walk> Walks { get; set; } = new List<Walk>();
    }
}
```

**Create Models/Domain/Walk.cs:**
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    /// <summary>
    /// Represents an individual walking trail with comprehensive details
    /// Central entity containing walk information, location, and difficulty
    /// </summary>
    public class Walk
    {
        /// <summary>
        /// Unique identifier for the walk
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the walking trail
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Detailed description of the walk, including features and highlights
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Length of the walk in kilometers
        /// </summary>
        [Required]
        [Range(0.1, 1000.0)]
        public double LengthInKm { get; set; }

        /// <summary>
        /// Optional URL to an image of the walk
        /// </summary>
        [Url]
        public string? WalkImageUrl { get; set; }

        /// <summary>
        /// Foreign key reference to the difficulty level
        /// </summary>
        [Required]
        public Guid DifficultyId { get; set; }

        /// <summary>
        /// Foreign key reference to the region where the walk is located
        /// </summary>
        [Required]
        public Guid RegionId { get; set; }

        /// <summary>
        /// Navigation property to the associated difficulty level
        /// </summary>
        [ForeignKey("DifficultyId")]
        public virtual Difficulty Difficulty { get; set; }

        /// <summary>
        /// Navigation property to the associated region
        /// </summary>
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }
    }
}
```

**Create Models/Domain/Image.cs:**
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    /// <summary>
    /// Represents uploaded image files with metadata
    /// Manages file storage and retrieval for walks and regions
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Unique identifier for the image
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Original filename as uploaded by the user
        /// </summary>
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        /// <summary>
        /// Description or alt text for the image
        /// </summary>
        [StringLength(500)]
        public string? FileDescription { get; set; }

        /// <summary>
        /// File extension (e.g., .jpg, .png)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string FileExtension { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        [Required]
        public long FileSizeInBytes { get; set; }

        /// <summary>
        /// Physical file path on the server
        /// </summary>
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        /// <summary>
        /// Date and time when the image was uploaded
        /// </summary>
        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
    }
}
```

#### Step 4: Create Data Transfer Objects (DTOs)
DTOs provide a clean separation between internal domain models and external API contracts, ensuring data security and API versioning flexibility.

**Create Models/DTO/RegionDto.cs:**
```csharp
namespace NZWalks.API.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for Region entity
    /// Used for API responses to expose only necessary data
    /// </summary>
    public class RegionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
```

**Create Models/DTO/AddRegionRequestDto.cs:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for creating new regions
    /// Contains validation attributes for input data
    /// </summary>
    public class AddRegionRequestDto
    {
        [Required(ErrorMessage = "Region code is required")]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "Code must be between 2 and 3 characters")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Region name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? RegionImageUrl { get; set; }
    }
}
```

**Create similar DTOs for other entities following the same pattern...**

### Phase 3: Database Context and Configuration

#### Step 5: Create Database Context
The database context serves as the primary class responsible for Entity Framework functionality, including querying, change tracking, and persisting data.

**Create Data/NZWalksDbContext.cs:**
```csharp
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    /// <summary>
    /// Primary database context for NZWalks application
    /// Manages entity relationships and database configuration
    /// </summary>
    public class NZWalksDbContext : DbContext
    {
        /// <summary>
        /// Constructor accepting DbContextOptions for configuration
        /// </summary>
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet for Region entities
        /// </summary>
        public DbSet<Region> Regions { get; set; }

        /// <summary>
        /// DbSet for Walk entities
        /// </summary>
        public DbSet<Walk> Walks { get; set; }

        /// <summary>
        /// DbSet for Difficulty entities
        /// </summary>
        public DbSet<Difficulty> Difficulties { get; set; }

        /// <summary>
        /// DbSet for Image entities
        /// </summary>
        public DbSet<Image> Images { get; set; }

        /// <summary>
        /// Configures entity relationships and database constraints
        /// Seeds initial data for Difficulties and Regions
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships
            ConfigureEntityRelationships(modelBuilder);

            // Seed initial data
            SeedInitialData(modelBuilder);
        }

        /// <summary>
        /// Configures entity relationships and constraints
        /// </summary>
        private void ConfigureEntityRelationships(ModelBuilder modelBuilder)
        {
            // Configure Walk-Region relationship
            modelBuilder.Entity<Walk>()
                .HasOne(w => w.Region)
                .WithMany(r => r.Walks)
                .HasForeignKey(w => w.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Walk-Difficulty relationship
            modelBuilder.Entity<Walk>()
                .HasOne(w => w.Difficulty)
                .WithMany(d => d.Walks)
                .HasForeignKey(w => w.DifficultyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure unique constraints
            modelBuilder.Entity<Region>()
                .HasIndex(r => r.Code)
                .IsUnique();

            modelBuilder.Entity<Difficulty>()
                .HasIndex(d => d.Name)
                .IsUnique();
        }

        /// <summary>
        /// Seeds initial data for application startup
        /// </summary>
        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            // Seed Difficulties
            var difficulties = new List<Difficulty>
            {
                new Difficulty { Id = Guid.Parse("54466f17-02af-48e7-8ed3-5a4a8bfacf6f"), Name = "Easy" },
                new Difficulty { Id = Guid.Parse("ea294873-7a8c-4c0f-bfa7-a2eb492cbf8c"), Name = "Medium" },
                new Difficulty { Id = Guid.Parse("f808ddcd-b5e5-4d80-b732-1ca523e48434"), Name = "Hard" }
            };

            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Code = "AKL",
                    Name = "Auckland",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Code = "NTL",
                    Name = "Northland",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Code = "BOP",
                    Name = "Bay Of Plenty",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Code = "WGN",
                    Name = "Wellington",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Code = "NSN",
                    Name = "Nelson",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Code = "STL",
                    Name = "Southland",
                    RegionImageUrl = null
                }
            };

            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
```

### Phase 4: Authentication and Authorization Setup

#### Step 6: Create Authentication DbContext
Implement a separate context for authentication to maintain security isolation between business data and user credentials.

**Create Data/NZWalksAuthDbContext.cs:**
```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    /// <summary>
    /// Authentication-specific database context
    /// Manages user accounts, roles, and authentication data
    /// Separated from main application context for security
    /// </summary>
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Create roles
            var readerRoleId = "8e445865-a24d-4543-a6c6-9443d048cdb9";
            var writerRoleId = "6313179f-7837-473a-a4d5-a5571b43e6a6";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
```

### Phase 5: Repository Pattern Implementation

#### Step 7: Implement Repository Pattern
The Repository pattern abstracts data access logic and provides a consistent interface for data operations, promoting testability and maintainability.

**Create Repositories/IRegionRepository.cs:**
```csharp
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    /// <summary>
    /// Repository interface for Region entity operations
    /// Defines contract for region-related data access methods
    /// </summary>
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> CodeExistsAsync(string code, Guid? excludeId = null);
    }
}
```

**Create Repositories/SQLRegionRepository.cs:**
```csharp
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    /// <summary>
    /// SQL Server implementation of Region repository
    /// Provides concrete data access operations using Entity Framework Core
    /// </summary>
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all regions from the database
        /// </summary>
        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific region by ID with related walks
        /// </summary>
        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions
                .Include(r => r.Walks)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Creates a new region in the database
        /// </summary>
        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        /// <summary>
        /// Updates an existing region
        /// </summary>
        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();
            return existingRegion;
        }

        /// <summary>
        /// Deletes a region from the database
        /// </summary>
        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRegion == null)
            {
                return null;
            }

            _dbContext.Regions.Remove(existingRegion);
            await _dbContext.SaveChangesAsync();
            return existingRegion;
        }

        /// <summary>
        /// Checks if a region exists with the given ID
        /// </summary>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Regions.AnyAsync(r => r.Id == id);
        }

        /// <summary>
        /// Checks if a region code already exists (for uniqueness validation)
        /// </summary>
        public async Task<bool> CodeExistsAsync(string code, Guid? excludeId = null)
        {
            var query = _dbContext.Regions.Where(r => r.Code == code);
            if (excludeId.HasValue)
            {
                query = query.Where(r => r.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }
    }
}
```

### Phase 6: Service Layer and Business Logic

#### Step 8: Create AutoMapper Profiles
AutoMapper profiles define the mapping configuration between domain models and DTOs, ensuring clean data transformation.

**Create Mappings/AutoMapperProfiles.cs:**
```csharp
using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    /// <summary>
    /// AutoMapper profile configuration for entity-to-DTO mappings
    /// Defines how domain models should be mapped to Data Transfer Objects
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();

            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
```

#### Step 9: Create Custom Action Filters
Action filters provide cross-cutting concerns such as validation, logging, and error handling.

**Create CustomActionFilters/ValidateModelAttribute.cs:**
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.API.CustomActionFilters
{
    /// <summary>
    /// Custom action filter for model validation
    /// Automatically returns BadRequest for invalid models
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
```

### Phase 7: API Controllers Implementation

#### Step 10: Create RESTful Controllers
Controllers handle HTTP requests and orchestrate the interaction between the presentation layer and business logic.

**Create Controllers/RegionsController.cs:**
```csharp
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    /// <summary>
    /// RESTful API controller for managing geographical regions
    /// Provides comprehensive CRUD operations with proper HTTP semantics
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepository regionRepository, 
                               IMapper mapper, 
                               ILogger<RegionsController> logger)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/regions
        /// Retrieves all regions
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("GetAll Regions method was invoked");

                var regionsDomain = await _regionRepository.GetAllAsync();
                var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);

                _logger.LogInformation($"Finished GetAll Regions request with data: {regionsDto}");

                return Ok(regionsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all regions");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// GET: api/regions/{id}
        /// Retrieves a specific region by ID
        /// </summary>
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var regionDomain = await _regionRepository.GetByIdAsync(id);

                if (regionDomain == null)
                {
                    return NotFound($"Region with ID {id} was not found");
                }

                var regionDto = _mapper.Map<RegionDto>(regionDomain);
                return Ok(regionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting region by ID: {RegionId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// POST: api/regions
        /// Creates a new region
        /// </summary>
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            try
            {
                // Check if region code already exists
                if (await _regionRepository.CodeExistsAsync(addRegionRequestDto.Code))
                {
                    ModelState.AddModelError("Code", "A region with this code already exists");
                    return BadRequest(ModelState);
                }

                var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
                regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating region");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// PUT: api/regions/{id}
        /// Updates an existing region
        /// </summary>
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            try
            {
                // Check if region code already exists for a different region
                if (await _regionRepository.CodeExistsAsync(updateRegionRequestDto.Code, id))
                {
                    ModelState.AddModelError("Code", "A region with this code already exists");
                    return BadRequest(ModelState);
                }

                var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
                regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound($"Region with ID {id} was not found");
                }

                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating region with ID: {RegionId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// DELETE: api/regions/{id}
        /// Deletes a region
        /// </summary>
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var regionDomainModel = await _regionRepository.DeleteAsync(id);

                if (regionDomainModel == null)
                {
                    return NotFound($"Region with ID {id} was not found");
                }

                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting region with ID: {RegionId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
```

### Phase 8: Program.cs Configuration and Dependency Injection

#### Step 11: Configure Application Services
The Program.cs file serves as the application's entry point and dependency injection container configuration.

**Update Program.cs:**
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using NZWalks.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for comprehensive logging
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NzWalks_Log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "NZ Walks API", 
        Version = "v1",
        Description = "A comprehensive API for managing New Zealand walking trails",
        Contact = new OpenApiContact
        {
            Name = "NZ Walks Support",
            Email = "support@nzwalks.com"
        }
    });
    
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Configure Entity Framework DbContext
builder.Services.AddDbContext<NZWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

// Configure Repository Pattern dependencies
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Configure Identity
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
});

app.UseAuthentication();
app.UseAuthorization();

// Configure static file serving for images
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
```

### Phase 9: Database Migration and Setup

#### Step 12: Configure Connection Strings and Database Migration
Configure database connections and create the database schema using Entity Framework migrations.

**Update appsettings.json:**
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
    "NZWalksConnectionString": "Server=(localdb)\\mssqllocaldb;Database=NZWalksDb;Trusted_Connection=True;TrustServerCertificate=Yes",
    "NZWalksAuthConnectionString": "Server=(localdb)\\mssqllocaldb;Database=NZWalksAuthDb;Trusted_Connection=True;TrustServerCertificate=Yes"
  },
  "Jwt": {
    "Key": "veryverysceret....veryveryverysecret....veryverysecret",
    "Issuer": "https://localhost:7020",
    "Audience": "https://localhost:7020"
  }
}
```

**Create Database Migrations:**
```powershell
# Navigate to API project directory
cd NZWalks.API

# Create migration for main database
dotnet ef migrations add "Initial Migration" --context NZWalksDbContext

# Create migration for authentication database
dotnet ef migrations add "Creating Auth Database" --context NZWalksAuthDbContext

# Apply migrations to create databases
dotnet ef database update --context NZWalksDbContext
dotnet ef database update --context NZWalksAuthDbContext
```

### Phase 10: Frontend Web Application (NZWalks.UI)

#### Step 13: Configure UI Project Dependencies
Set up the MVC web application that will consume the API and provide user interface.

```powershell
cd ../NZWalks.UI

# Install required packages for HTTP client and JSON handling
dotnet add package Newtonsoft.Json --version 13.0.3
```

#### Step 14: Create Models for UI
Create models that represent the data structures used in the UI application.

**Create Models/RegionDto.cs:**
```csharp
namespace NZWalks.UI.Models
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
```

**Create Models/AddRegionViewModel.cs:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models
{
    public class AddRegionViewModel
    {
        [Required(ErrorMessage = "Region code is required")]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "Code must be between 2 and 3 characters")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Region name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? RegionImageUrl { get; set; }
    }
}
```

#### Step 15: Create Region Controller for UI
Implement the controller that handles user interactions and API communication.

**Create Controllers/RegionsController.cs:**
```csharp
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using Newtonsoft.Json;
using System.Text;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            try
            {
                // Get All Regions from Web API
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7020/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
                // Log Exception
                throw;
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7020/api/regions"),
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

                if (response is not null)
                {
                    return RedirectToAction("Index", "Regions");
                }
            }
            catch (Exception ex)
            {
                // Log Exception
                ViewBag.Error = "Something went wrong!";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7020/api/regions/{id.ToString()}");
                
                if (response is not null)
                {
                    return View(response);
                }
            }
            catch (Exception ex)
            {
                // Log Exception
                throw;
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7020/api/regions/{request.Id}"),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

                if (response is not null)
                {
                    return RedirectToAction("Index", "Regions");
                }
            }
            catch (Exception ex)
            {
                // Log Exception
                ViewBag.Error = "Something went wrong!";
            }

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7020/api/regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {
                // Log Exception
                ViewBag.Error = "Something went wrong!";
            }

            return View("Edit");
        }
    }
}
```

#### Step 16: Create Razor Views
Implement the user interface components using Razor syntax and Bootstrap styling.

**Create Views/Regions/Index.cshtml:**
```html
@model IEnumerable<NZWalks.UI.Models.RegionDto>
@{
    ViewData["Title"] = "Regions Management";
}

<div class="container-fluid">
    <div class="row">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h1 class="display-5 fw-bold">
                        <i class="fas fa-map-marked-alt text-primary"></i> Regions Management
                    </h1>
                    <p class="text-muted">Manage New Zealand geographical regions</p>
                </div>
                <a asp-controller="Regions" asp-action="Add" class="btn btn-primary btn-lg">
                    <i class="fas fa-plus"></i> Add New Region
                </a>
            </div>
        </div>
    </div>

    @if (Model != null && Model.Any())
    {
        <div class="card border-0 shadow">
            <div class="card-header bg-light">
                <h5 class="mb-0">
                    <i class="fas fa-list"></i> All Regions (@Model.Count() total)
                </h5>
            </div>
            <div class="card-body p-0">
                <div class="table-responsive">
                    <table class="table table-hover mb-0">
                        <thead class="table-dark">
                            <tr>
                                <th scope="col">
                                    <i class="fas fa-image"></i> Image
                                </th>
                                <th scope="col">
                                    <i class="fas fa-code"></i> Code
                                </th>
                                <th scope="col">
                                    <i class="fas fa-map"></i> Name
                                </th>
                                <th scope="col" class="text-center">
                                    <i class="fas fa-tools"></i> Actions
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var region in Model)
                            {
                                <tr>
                                    <td>
                                        @if (!string.IsNullOrWhiteSpace(region.RegionImageUrl))
                                        {
                                            <img src="@region.RegionImageUrl" alt="@region.Name" 
                                                 class="rounded" style="width: 80px; height: 60px; object-fit: cover;">
                                        }
                                        else
                                        {
                                            <div class="bg-light rounded d-flex align-items-center justify-content-center" 
                                                 style="width: 80px; height: 60px;">
                                                <i class="fas fa-image text-muted fa-2x"></i>
                                            </div>
                                        }
                                    </td>
                                    <td>
                                        <span class="badge bg-primary fs-6">@region.Code</span>
                                    </td>
                                    <td>
                                        <h6 class="mb-0 fw-bold">@region.Name</h6>
                                    </td>
                                    <td class="text-center">
                                        <div class="btn-group" role="group">
                                            <a asp-controller="Regions" asp-action="Edit" 
                                               asp-route-id="@region.Id" 
                                               class="btn btn-sm btn-warning" 
                                               title="Edit Region">
                                                <i class="fas fa-edit"></i> Edit
                                            </a>
                                            <button type="button" class="btn btn-sm btn-danger" 
                                                    data-bs-toggle="modal" 
                                                    data-bs-target="#deleteModal-@region.Id" 
                                                    title="Delete Region">
                                                <i class="fas fa-trash"></i> Delete
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
            <i class="fas fa-map-marked-alt fa-5x text-muted mb-4"></i>
            <h2 class="text-muted mb-3">No Regions Found</h2>
            <p class="text-muted mb-4">Start by adding your first region to begin managing New Zealand walking areas.</p>
            <a asp-controller="Regions" asp-action="Add" class="btn btn-primary btn-lg">
                <i class="fas fa-plus"></i> Add Your First Region
            </a>
        </div>
    }
</div>

<!-- Delete Confirmation Modals -->
@if (Model != null && Model.Any())
{
    @foreach (var region in Model)
    {
        <div class="modal fade" id="deleteModal-@region.Id" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <i class="fas fa-exclamation-triangle text-warning"></i> Confirm Deletion
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <p>Are you sure you want to delete the region:</p>
                        <strong>"@region.Name" (@region.Code)</strong>
                        <p class="text-danger mt-2">This action cannot be undone.</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                            <i class="fas fa-times"></i> Cancel
                        </button>
                        <form asp-controller="Regions" asp-action="Delete" method="post" class="d-inline">
                            <input type="hidden" asp-for="@region.Id" />
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
```

### Phase 11: Testing and Validation

#### Step 17: Configure and Test the Complete Application
Complete the setup by configuring both projects to work together and testing all functionality.

**Update NZWalks.UI Program.cs:**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient for API communication
builder.Services.AddHttpClient();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

#### Step 18: Run and Test the Application
```powershell
# Start the API project (in one terminal)
cd NZWalks.API
dotnet run

# Start the UI project (in another terminal)
cd ../NZWalks.UI
dotnet run
```

**Testing Checklist:**
1. **API Endpoints**: Test all CRUD operations using Swagger UI at `https://localhost:7020/swagger`
2. **Database Connectivity**: Verify data is persisting correctly in SQL Server
3. **Authentication**: Test JWT token generation and authorization
4. **UI Functionality**: Test region management through the web interface
5. **API Integration**: Verify UI successfully communicates with API
6. **Error Handling**: Test error scenarios and validate proper error messages
7. **Logging**: Check log files for proper operation tracking

## Project Structure Overview

```
NZWalks-Solution/
├── NZWalks.sln
├── NZWalks.API/                    # Backend Web API
│   ├── Controllers/                # API Controllers
│   ├── Models/
│   │   ├── Domain/                # Domain entities
│   │   └── DTO/                   # Data Transfer Objects
│   ├── Data/                      # Database contexts
│   ├── Repositories/              # Repository pattern implementation
│   ├── Mappings/                  # AutoMapper profiles
│   ├── CustomActionFilters/       # Custom action filters
│   ├── Middlewares/               # Custom middleware
│   ├── Migrations/                # EF Core migrations
│   ├── Images/                    # Image storage
│   ├── Logs/                      # Application logs
│   └── Program.cs                 # Application entry point
└── NZWalks.UI/                    # Frontend Web Application
    ├── Controllers/               # MVC Controllers
    ├── Models/                    # View models
    ├── Views/                     # Razor views
    ├── wwwroot/                   # Static files
    └── Program.cs                 # Application entry point
```

## Advanced Features and Extensions

### Security Enhancements
- **JWT Refresh Tokens**: Implement token refresh mechanism
- **Role-based Authorization**: Extend with more granular permissions
- **Rate Limiting**: Add API rate limiting for production
- **CORS Configuration**: Fine-tune CORS policies

### Performance Optimizations
- **Caching**: Implement Redis caching for frequently accessed data
- **Pagination**: Add pagination for large datasets
- **Database Indexing**: Optimize database queries with proper indexing
- **API Versioning**: Implement API versioning strategy

### Additional Features
- **File Upload Validation**: Enhanced image upload with validation
- **Search and Filtering**: Advanced search capabilities
- **Real-time Updates**: SignalR integration for real-time notifications
- **API Documentation**: Enhanced Swagger documentation with examples

## Troubleshooting Guide

### Common Database Issues
```powershell
# If migrations fail, reset database
dotnet ef database drop --context NZWalksDbContext --force
dotnet ef database drop --context NZWalksAuthDbContext --force
dotnet ef database update --context NZWalksDbContext
dotnet ef database update --context NZWalksAuthDbContext
```

### Connection String Issues
- Ensure SQL Server is running
- Verify LocalDB installation: `sqllocaldb info`
- Update connection strings for your SQL Server instance

### CORS Issues
- Verify CORS configuration in Program.cs
- Check browser console for CORS errors
- Ensure API and UI are running on correct ports

## Learning Outcomes

Upon completing this tutorial, trainees will have mastered:

### **Backend Development**
- RESTful API design and implementation
- Entity Framework Core with Code First approach
- Repository pattern and dependency injection
- JWT authentication and authorization
- Structured logging with Serilog
- AutoMapper for object mapping
- Custom middleware and action filters

### **Frontend Development**
- ASP.NET Core MVC architecture
- HTTP client integration for API consumption
- Razor view development with Bootstrap
- Form handling and model validation
- Responsive web design principles

### **Full-Stack Integration**
- End-to-end application development
- Database design and migration management
- Security implementation across layers
- Error handling and logging strategies
- Testing and debugging techniques

This comprehensive tutorial provides a solid foundation for building enterprise-level web applications using modern .NET technologies and best practices. 



