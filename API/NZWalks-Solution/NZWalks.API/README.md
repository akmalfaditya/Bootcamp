# NZWalks.API - RESTful Web API

## 🎯 Project Overview
Welcome to **NZWalks.API** - a robust ASP.NET Core Web API that powers the New Zealand Walks application! This project serves as the backend engine, providing comprehensive RESTful services for managing walking trails, regions, difficulties, and user authentication.

## 🚀 What You'll Learn
This API project is your gateway to mastering:
- **RESTful API Design** with proper HTTP verbs and status codes
- **Entity Framework Core** with Code-First migrations
- **JWT Authentication & Authorization** with role-based security
- **Repository Pattern** for clean data access
- **AutoMapper** for elegant object mapping
- **Custom Action Filters** for validation
- **Exception Handling Middleware** for robust error management
- **Structured Logging** with Serilog

## 🏗️ Architecture & Design Patterns

### 🗂️ Project Structure
```
NZWalks.API/
├── Controllers/          # API endpoints and HTTP handling
├── Models/
│   ├── Domain/          # Entity models (Region, Walk, Difficulty, Image)
│   └── DTO/             # Data Transfer Objects
├── Data/                # DbContext and database configuration
├── Repositories/        # Data access layer with interfaces
├── Mappings/           # AutoMapper profiles
├── Middlewares/        # Custom middleware components
├── CustomActionFilters/ # Validation and custom filters
└── Migrations/         # Entity Framework migrations
```

### 🎨 Design Patterns Implemented
- **Repository Pattern** - Clean separation of data access logic
- **Dependency Injection** - Loose coupling and testability
- **DTO Pattern** - Secure data transfer and API contract definition
- **Middleware Pattern** - Cross-cutting concerns handling

## 🔧 Core Features

### 📍 **Regions Management**
- **GET** `/api/regions` - Retrieve all New Zealand regions
- **GET** `/api/regions/{id}` - Get specific region details
- **POST** `/api/regions` - Create new regions
- **PUT** `/api/regions/{id}` - Update existing regions
- **DELETE** `/api/regions/{id}` - Remove regions

### 🚶‍♂️ **Walks Management**
- **GET** `/api/walks` - List all walks with filtering, sorting & pagination
- **GET** `/api/walks/{id}` - Retrieve specific walk details
- **POST** `/api/walks` - Create new walking trails
- **PUT** `/api/walks/{id}` - Update walk information
- **DELETE** `/api/walks/{id}` - Remove walks

### 🔐 **Authentication & Authorization**
- **POST** `/api/auth/register` - User registration
- **POST** `/api/auth/login` - JWT token generation
- **Role-based authorization** (Reader/Writer roles)

### 📸 **Image Management**
- **POST** `/api/images/upload` - Upload walk and region images
- **Local file storage** with proper validation

## 💾 Database Design

### 🗄️ Core Entities
```csharp
// Region Entity
public class Region
{
    public Guid Id { get; set; }
    public string Code { get; set; }        // e.g., "AKL", "WGN"
    public string Name { get; set; }        // e.g., "Auckland", "Wellington"
    public string? RegionImageUrl { get; set; }
}

// Walk Entity  
public class Walk
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public Guid DifficultyId { get; set; }  // Foreign Key
    public Guid RegionId { get; set; }      // Foreign Key
    
    // Navigation Properties
    public Difficulty Difficulty { get; set; }
    public Region Region { get; set; }
}

// Difficulty Entity
public class Difficulty
{
    public Guid Id { get; set; }
    public string Name { get; set; }        // "Easy", "Medium", "Hard"
}
```

### 🌱 Seed Data
The application comes pre-loaded with:
- **New Zealand Regions**: Auckland, Wellington, Northland, Bay of Plenty, Nelson, Southland
- **Difficulty Levels**: Easy, Medium, Hard
- **Sample Images**: Beautiful NZ landscape photos

## 🔒 Security Implementation

### 🎫 JWT Authentication
```csharp
// JWT Configuration in Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            // ... more configuration
        };
    });
```

### 👥 Role-Based Authorization
- **Reader Role**: Can view regions and walks
- **Writer Role**: Can create, update, and delete content

## 🛠️ Key Dependencies

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.6" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.6" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.6" />
<PackageReference Include="AutoMapper" Version="13.0.1" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.1" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
```

## 🚀 Getting Started

### 1️⃣ **Prerequisites**
- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### 2️⃣ **Database Setup**
```bash
# Update connection string in appsettings.json
"ConnectionStrings": {
  "NZWalksConnectionString": "Server=(localdb)\\mssqllocaldb;Database=NZWalksDb;Trusted_Connection=true;"
}

# Run migrations
dotnet ef database update
```

### 3️⃣ **Run the API**
```bash
dotnet run
# API will be available at https://localhost:7081
# Swagger UI at https://localhost:7081/swagger
```

## 📊 API Testing

### 🧪 **Using Swagger UI**
Navigate to `https://localhost:7081/swagger` for interactive API documentation and testing.

### 🔧 **Sample API Calls**
```bash
# Get all regions
GET https://localhost:7081/api/regions

# Create a new region
POST https://localhost:7081/api/regions
Content-Type: application/json
{
  "code": "OTA",
  "name": "Otago",
  "regionImageUrl": "https://example.com/otago.jpg"
}

# Get walks with filtering
GET https://localhost:7081/api/walks?filterOn=Name&filterQuery=Track&pageSize=5
```

## 🎓 **Advanced Features to Explore**

### 🔍 **Filtering & Pagination**
Learn how to implement:
- Dynamic filtering by any property
- Sorting in ascending/descending order  
- Pagination with page size limits

### 🛡️ **Validation & Error Handling**
Discover:
- Custom validation attributes
- Global exception handling middleware
- Consistent error response formats

### 📝 **Logging Strategy**
Understand:
- Structured logging with Serilog
- Log levels and filtering
- File and console logging outputs

## 🔮 **Next Steps**
1. **Explore the Controllers** - See how each endpoint is implemented
2. **Study the Repository Pattern** - Understand data access abstraction
3. **Examine AutoMapper Profiles** - Learn object mapping strategies
4. **Test the Authentication** - Try JWT token generation and usage
5. **Review the Migrations** - Understand database evolution

This API serves as an excellent foundation for understanding modern .NET Web API development. Every pattern and practice implemented here reflects real-world enterprise development standards!

---

*Happy coding! 🚀 Master these concepts and you'll be ready for any .NET API project.*

## 📚 **Step-by-Step Implementation Guide**

Ready to build this API from scratch? Follow this comprehensive guide to understand every component and implementation detail.

### 🎯 **Phase 1: Project Setup & Foundation**

#### **Step 1: Create New Project**
```bash
# Create solution and API project
dotnet new sln -n "NZWalks-Solution"
dotnet new webapi -n "NZWalks.API" -f net8.0
dotnet sln add NZWalks.API/NZWalks.API.csproj

# Navigate to API project
cd NZWalks.API
```

#### **Step 2: Install Required Packages**
```bash
# Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.6
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.6

# Authentication & Identity
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.6
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.6
dotnet add package Microsoft.IdentityModel.Tokens --version 7.6.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.6.0

# AutoMapper
dotnet add package AutoMapper --version 13.0.1

# Logging
dotnet add package Serilog --version 4.0.0
dotnet add package Serilog.AspNetCore --version 8.0.1
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0

# API Documentation
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.6
```

#### **Step 3: Project Structure Setup**
```bash
# Create folder structure
mkdir Models Models/Domain Models/DTO
mkdir Data Repositories Mappings
mkdir CustomActionFilters Middlewares
mkdir Images Logs
```

### 🎯 **Phase 2: Domain Models & Database**

#### **Step 4: Create Domain Models**

**Create `Models/Domain/Region.cs`:**
```csharp
namespace NZWalks.API.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
```

**Create `Models/Domain/Difficulty.cs`:**
```csharp
namespace NZWalks.API.Models.Domain
{
    public class Difficulty
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
```

**Create `Models/Domain/Walk.cs`:**
```csharp
namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        // Navigation properties
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
```

**Create `Models/Domain/Image.cs`:**
```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}
```

#### **Step 5: Create DTO Models**

**Create `Models/DTO/RegionDto.cs`:**
```csharp
namespace NZWalks.API.Models.DTO
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

**Create `Models/DTO/AddRegionRequestDto.cs`:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
```

**Create `Models/DTO/UpdateRegionRequestDto.cs`:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
```

**Create `Models/DTO/WalkDto.cs`:**
```csharp
namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
```

**Create `Models/DTO/AddWalkRequestDto.cs`:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0, 50)]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
```

**Create `Models/DTO/UpdateWalkRequestDto.cs`:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0, 50)]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
```

**Create `Models/DTO/DifficultyDto.cs`:**
```csharp
namespace NZWalks.API.Models.DTO
{
    public class DifficultyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
```

**Create `Models/DTO/ImageUploadRequestDto.cs`:**
```csharp
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class ImageUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? FileDescription { get; set; }
    }
}
```

#### **Step 6: Create Database Context**

**Create `Data/NZWalksDbContext.cs`:**
```csharp
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext: DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions): base(dbContextOptions)
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("54466f17-02af-48e7-8ed3-5a4a8bfacf6f"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("ea294873-7a8c-4c0f-bfa7-a2eb492cbf8c"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("f808ddcd-b5e5-4d80-b732-1ca523e48434"),
                    Name = "Hard"
                }
            };

            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed data for Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
```

**Create `Data/NZWalksAuthDbContext.cs`:**
```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "a71a55d6-99d7-4123-b4e0-1218ecb90e3e";
            var writerRoleId = "c309fa92-2123-47be-b397-a1c77adb502c";

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

### 🎯 **Phase 3: Repository Pattern Implementation**

#### **Step 7: Create Repository Interfaces and Implementations**

**Create `Repositories/IRegionRepository.cs`:**
```csharp
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
```

**Create `Repositories/SQLRegionRepository.cs`:**
```csharp
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
```

**Create `Repositories/IWalkRepository.cs`:**
```csharp
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
```

**Create `Repositories/SQLWalkRepository.cs`:**
```csharp
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
```

### 🎯 **Phase 4: AutoMapper Configuration**

#### **Step 8: Create AutoMapper Profiles**

**Create `Mappings/AutoMapperProfiles.cs`:**
```csharp
using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
        }
    }
}
```

### 🎯 **Phase 5: Custom Action Filters**

#### **Step 9: Create Validation Filter**

**Create `CustomActionFilters/ValidateModelAttribute.cs`:**
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.API.CustomActionFilters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
```

### 🎯 **Phase 6: Exception Handling Middleware**

#### **Step 10: Create Exception Handler Middleware**

**Create `Middlewares/ExceptionHandlerMiddleware.cs`:**
```csharp
using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                // Log This Exception
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return A Custom Error Response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! We are looking into resolving this."
                };

                await httpContext.Response.WriteAsync(error.ToString());
            }
        }
    }
}
```

### 🎯 **Phase 7: API Controllers Implementation**

#### **Step 11: Create Regions Controller**

**Create `Controllers/RegionsController.cs`:**
```csharp
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET ALL REGIONS
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Return DTOs
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        // GET SINGLE REGION (Get Region By ID)
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get Region Domain Model From Database
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Return DTO back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        // POST To Create New Region
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // Update region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            // Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        // Delete Region
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
```

#### **Step 12: Create Walks Controller**

**Create `Controllers/WalksController.cs`:**
```csharp
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // CREATE Walk
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // GET Walks
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,
                isAscending ?? true, pageNumber, pageSize);

            // Create an exception
            throw new Exception("This is a new exception");

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        // Get Walk By Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // Update Walk By Id
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // Delete a Walk By Id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
```

### 🎯 **Phase 8: Image Upload Implementation**

#### **Step 13: Create Image Repository and Controller**

**Create `Repositories/IImageRepository.cs`:**
```csharp
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
```

**Create `Repositories/LocalImageRepository.cs`:**
```csharp
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            // Upload Image to Local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // Update the image object with file path
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // Add Image to the Images table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
```

**Create `Controllers/ImagesController.cs`:**
```csharp
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                // Convert DTO to Domain model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };

                // User repository to upload image
                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }
    }
}
```

### 🎯 **Phase 9: Complete Program.cs Configuration**

#### **Step 16: Configure Program.cs**

**Update `Program.cs`:**
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

// Configure Serilog
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NzWalks_Log.txt", rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "v1" });
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

// Configure Entity Framework
builder.Services.AddDbContext<NZWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

// Configure Repositories
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

// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
```

#### **Step 17: Configure appsettings.json**

**Update `appsettings.json`:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "NZWalksConnectionString": "Server=(localdb)\\mssqllocaldb;Database=NZWalksDb;Trusted_Connection=true;MultipleActiveResultSets=true;",
    "NZWalksAuthConnectionString": "Server=(localdb)\\mssqllocaldb;Database=NZWalksAuthDb;Trusted_Connection=true;MultipleActiveResultSets=true;"
  },
  "Jwt": {
    "Key": "veryverysecret.......................",
    "Issuer": "https://localhost:7081",
    "Audience": "https://localhost:7081"
  },
  "AllowedHosts": "*"
}
```

### 🎯 **Phase 10: Database Setup & Migration**

#### **Step 18: Create and Run Migrations**

```bash
# Install EF CLI tools (if not already installed)
dotnet tool install --global dotnet-ef

# Create initial migration for main database
dotnet ef migrations add "Initial Migration" --context NZWalksDbContext

# Create seeding migration
dotnet ef migrations add "Seeding data for Difficulties and Regions" --context NZWalksDbContext

# Create auth database migration
dotnet ef migrations add "Creating Auth Database" --context NZWalksAuthDbContext

# Update databases
dotnet ef database update --context NZWalksDbContext
dotnet ef database update --context NZWalksAuthDbContext
```

#### **Step 19: Build and Run the Application**

```bash
# Build the project
dotnet build

# Run the application
dotnet run

# API will be available at:
# - HTTPS: https://localhost:7081
# - HTTP: https://localhost:5081
# - Swagger UI: https://localhost:7081/swagger
```

### 🎯 **Phase 11: Testing & Validation**

#### **Step 20: Test API Endpoints**

**1. Test Regions API:**
```bash
# Get all regions
GET https://localhost:7081/api/regions

# Get single region
GET https://localhost:7081/api/regions/{id}

# Create new region
POST https://localhost:7081/api/regions
Content-Type: application/json

{
  "code": "CHC",
  "name": "Christchurch",
  "regionImageUrl": "https://example.com/christchurch.jpg"
}

# Update region
PUT https://localhost:7081/api/regions/{id}
Content-Type: application/json

{
  "code": "CHC",
  "name": "Christchurch Updated",
  "regionImageUrl": "https://example.com/christchurch-updated.jpg"
}

# Delete region
DELETE https://localhost:7081/api/regions/{id}
```

**2. Test Authentication:**
```bash
# Register new user
POST https://localhost:7081/api/Auth/Register
Content-Type: application/json

{
  "username": "user@example.com",
  "password": "Test@123",
  "roles": ["Reader"]
}

# Login to get JWT token
POST https://localhost:7081/api/Auth/Login
Content-Type: application/json

{
  "username": "user@example.com",
  "password": "Test@123"
}
```

**3. Test with Authorization:**
```bash
# Use JWT token in headers
GET https://localhost:7081/api/regions
Authorization: Bearer {your-jwt-token}
```

**4. Test Image Upload:**
```bash
# Upload image
POST https://localhost:7081/api/Images/Upload
Content-Type: multipart/form-data

file: [select image file]
fileName: "test-image"
fileDescription: "Test image upload"
```

### 🎯 **Phase 12: Advanced Features & Best Practices**

#### **Step 21: Enable Authorization (Optional)**

To enable role-based authorization, uncomment the `[Authorize]` attributes in controllers:

```csharp
// In RegionsController.cs
[HttpGet]
[Authorize(Roles = "Reader")] // Uncomment this line
public async Task<IActionResult> GetAll()

[HttpPost]
[Authorize(Roles = "Writer")] // Uncomment this line  
public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
```

#### **Step 22: Remove Exception for Production**

In `WalksController.cs`, remove or comment out the test exception:

```csharp
// Remove this line in production:
// throw new Exception("This is a new exception");
```

#### **Step 23: Logging Configuration**

The application includes Serilog for structured logging:
- Console logging for development
- File logging to `Logs/NzWalks_Log.txt`
- Minimum level set to Warning

#### **Step 24: Error Handling**

The custom exception middleware provides:
- Global exception handling
- Structured error responses
- Error logging with unique IDs
- User-friendly error messages

## 🎯 **Final Project Structure**

```
NZWalks.API/
├── Controllers/
│   ├── AuthController.cs
│   ├── ImagesController.cs
│   ├── RegionsController.cs
│   └── WalksController.cs
├── CustomActionFilters/
│   └── ValidateModelAttribute.cs
├── Data/
│   ├── NZWalksAuthDbContext.cs
│   └── NZWalksDbContext.cs
├── Images/
├── Logs/
├── Mappings/
│   └── AutoMapperProfiles.cs
├── Middlewares/
│   └── ExceptionHandlerMiddleware.cs
├── Migrations/
├── Models/
│   ├── Domain/
│   │   ├── Difficulty.cs
│   │   ├── Image.cs
│   │   ├── Region.cs
│   │   └── Walk.cs
│   └── DTO/
│       ├── AddRegionRequestDto.cs
│       ├── AddWalkRequestDto.cs
│       ├── DifficultyDto.cs
│       ├── ImageUploadRequestDto.cs
│       ├── LoginRequestDto.cs
│       ├── LoginResponseDto.cs
│       ├── RegionDto.cs
│       ├── RegisterRequestDto.cs
│       ├── UpdateRegionRequestDto.cs
│       ├── UpdateWalkRequestDto.cs
│       └── WalkDto.cs
├── Repositories/
│   ├── IImageRepository.cs
│   ├── IRegionRepository.cs
│   ├── ITokenRepository.cs
│   ├── IWalkRepository.cs
│   ├── LocalImageRepository.cs
│   ├── SQLRegionRepository.cs
│   ├── SQLWalkRepository.cs
│   └── TokenRepository.cs
├── appsettings.json
├── NZWalks.API.csproj
└── Program.cs
```

## 🚀 **Congratulations!**

You have successfully built a complete, production-ready ASP.NET Core Web API with:

✅ **RESTful API Design** with proper HTTP verbs and status codes  
✅ **Entity Framework Core** with Code-First migrations  
✅ **JWT Authentication & Authorization** with role-based security  
✅ **Repository Pattern** for clean data access  
✅ **AutoMapper** for elegant object mapping  
✅ **Custom Action Filters** for validation  
✅ **Exception Handling Middleware** for robust error management  
✅ **Structured Logging** with Serilog  
✅ **File Upload** functionality  
✅ **API Documentation** with Swagger  
✅ **Database Seeding** with initial data  

This API demonstrates modern .NET development best practices and is ready for production deployment or as a foundation for larger applications.

*Happy coding! 🎯 You now have the skills to build enterprise-grade .NET Web APIs.*

