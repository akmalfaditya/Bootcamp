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

#### **Step 5: Create Data Transfer Objects (DTOs)**

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

**Create Walk DTOs similarly:**
- `Models/DTO/WalkDto.cs`
- `Models/DTO/AddWalkRequestDto.cs`
- `Models/DTO/UpdateWalkRequestDto.cs`
- `Models/DTO/DifficultyDto.cs`

#### **Step 6: Database Context Setup**

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

### 🎯 **Phase 3: Repository Pattern Implementation**

#### **Step 7: Create Repository Interfaces**

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

**Create `Repositories/IWalkRepository.cs`:**
```csharp
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
```

#### **Step 8: Implement Repository Classes**

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

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
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

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
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
    }
}
```

### 🎯 **Phase 4: AutoMapper Configuration**

#### **Step 9: AutoMapper Profile Setup**

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

#### **Step 10: Model Validation Filter**

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

### 🎯 **Phase 6: Controllers Implementation**

#### **Step 11: Regions Controller**

**Create `Controllers/RegionsController.cs`:**
```csharp
using AutoMapper;
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
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        // GET SINGLE REGION
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        // POST To Create New Region
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // Update region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
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

### 🎯 **Phase 7: Authentication & Identity Setup**

#### **Step 12: Identity DbContext**

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

#### **Step 13: JWT Token Repository**

**Create `Repositories/ITokenRepository.cs`:**
```csharp
using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
```

### 🎯 **Phase 8: Program.cs Configuration**

#### **Step 14: Complete Program.cs Setup**

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

### 🎯 **Phase 9: Configuration & Migration**

#### **Step 15: Update appsettings.json**

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

#### **Step 16: Create and Run Migrations**

```bash
# Install EF CLI tools (if not already installed)
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add "Initial Migration" --context NZWalksDbContext

# Create seeding migration
dotnet ef migrations add "Seeding data for Difficulties and Regions" --context NZWalksDbContext

# Create auth migration
dotnet ef migrations add "Creating Auth Database" --context NZWalksAuthDbContext

# Update databases
dotnet ef database update --context NZWalksDbContext
dotnet ef database update --context NZWalksAuthDbContext
```

### 🎯 **Phase 10: Testing & Validation**

#### **Step 17: Run and Test the API**

```bash
# Build the project
dotnet build

# Run the API
dotnet run

# API will be available at https://localhost:7081
# Swagger UI at https://localhost:7081/swagger
```

#### **Step 18: Test Key Endpoints**

```bash
# Test GET all regions
GET https://localhost:7081/api/regions

# Test POST create region
POST https://localhost:7081/api/regions
Content-Type: application/json
{
  "code": "CHC",
  "name": "Christchurch",
  "regionImageUrl": "https://example.com/christchurch.jpg"
}

# Test GET single region
GET https://localhost:7081/api/regions/{id}
```

## 🎓 **Implementation Tips**

### ✅ **Best Practices Applied**
1. **Separation of Concerns** - Clear layer separation
2. **Dependency Injection** - Proper IoC container usage
3. **Async/Await** - Non-blocking operations
4. **Model Validation** - Input validation at API level
5. **Error Handling** - Proper HTTP status codes
6. **Security** - JWT authentication and authorization

### 🔧 **Common Issues & Solutions**
1. **Migration Errors** - Ensure connection strings are correct
2. **JWT Errors** - Verify JWT configuration in appsettings.json
3. **CORS Issues** - Add CORS policy if needed for frontend
4. **File Upload** - Ensure Images folder has proper permissions

### 🚀 **Next Implementation Steps**
1. Complete the Walks controller with filtering and pagination
2. Implement the Authentication controller
3. Add Images controller for file uploads
4. Create custom middleware for exception handling
5. Add comprehensive logging throughout the application

This step-by-step guide provides a solid foundation for understanding modern .NET API development patterns and practices!

