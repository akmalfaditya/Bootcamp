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

