# Product Catalog Web API - Complete REST API Implementation

## Project Overview

This project demonstrates the comprehensive implementation of a **RESTful Web API** using ASP.NET Core, showcasing modern web development practices, clean architecture principles, and industry-standard patterns for building production-ready APIs.

## Architecture & Key Concepts

### 1. RESTful API Design Principles
- **Resource-based URLs**: `/api/products`, `/api/categories`
- **HTTP Verbs**: GET, POST, PUT, DELETE for different operations
- **Status Codes**: Proper HTTP status codes (200, 201, 404, 400, 500)
- **Stateless Operations**: Each request contains all necessary information
- **JSON Communication**: Standard data exchange format

### 2. Clean Architecture Implementation
- **Separation of Concerns**: Controllers, Services, Data Access, and DTOs
- **Dependency Injection**: Loose coupling through interfaces
- **Repository Pattern**: Abstracted data access layer
- **Service Layer**: Business logic encapsulation
- **DTO Pattern**: Data transfer objects for API contracts

### 3. Entity Framework Core Integration
- **Code First Approach**: Define models in C#, generate database schema
- **Migration System**: Version control for database schema changes
- **Relationship Management**: One-to-Many relationships between entities
- **Query Optimization**: Efficient database queries with Include and Select

## Technology Stack

- **Framework**: ASP.NET Core 8.0 Web API
- **Database**: SQLite with Entity Framework Core 9.0
- **Documentation**: Swagger/OpenAPI for API documentation
- **Architecture**: Clean Architecture with Repository and Service patterns
- **Validation**: Data Annotations for model validation
- **Serialization**: System.Text.Json for JSON handling

## Prerequisites

Before building this project, ensure you have:

1. **.NET 8.0 SDK** or later installed
2. **Visual Studio 2022** (recommended) or **Visual Studio Code** with C# extension
3. **Entity Framework Tools** (installed globally):
   ```powershell
   dotnet tool install --global dotnet-ef
   ```
4. **Basic understanding** of C#, HTTP protocols, and REST principles
5. **Git** for version control (optional but recommended)

## Complete Step-by-Step Build Guide

### Phase 1: Project Setup and Structure

#### Step 1: Create the Web API Project
```powershell
# Create project directory
mkdir ProductCatalogAPI
cd ProductCatalogAPI

# Create new Web API project with controllers
dotnet new webapi -n ProductCatalogAPI --use-controllers

# Navigate to project directory
cd ProductCatalogAPI

# Test initial build
dotnet build
```

#### Step 2: Install Required NuGet Packages
```powershell
# Install Entity Framework Core packages
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.5
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.5
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.5

# Verify packages are installed
dotnet list package
```

#### Step 3: Create Project Structure
Create the following directories:
```
ProductCatalogAPI/
├── Controllers/
├── Data/
├── DTOs/
├── Models/
├── Services/
├── Migrations/ (will be created by EF)
└── Properties/
```

### Phase 2: Domain Models (Entities)

#### Step 4: Create the Category Entity
Create `Models/Category.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
```

#### Step 5: Create the Product Entity
Create `Models/Product.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
```

### Phase 3: Data Transfer Objects (DTOs)

#### Step 6: Create Product DTOs
Create `DTOs/ProductDtos.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.DTOs
{
    // DTO for creating new products
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number")]
        public int CategoryId { get; set; }
    }

    // DTO for updating existing products
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number")]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // DTO for returning product data
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public CategoryDto Category { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    // DTO for product summaries in list views
    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
```

#### Step 7: Create Category DTOs
Create `DTOs/CategoryDtos.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.DTOs
{
    // DTO for creating new categories
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }
    }

    // DTO for updating categories
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // DTO for returning category data
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProductCount { get; set; }
    }

    // Extended DTO with product information
    public class CategoryWithProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductSummaryDto> Products { get; set; } = new List<ProductSummaryDto>();
    }
}
```

### Phase 4: Database Context and Configuration

#### Step 8: Create Database Context
Create `Data/ProductCatalogDbContext.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Data
{
    public class ProductCatalogDbContext : DbContext
    {
        public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                // Set up Product-Category relationship
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure decimal precision for Price
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");

                // Add indexes for performance
                entity.HasIndex(p => p.CategoryId)
                      .HasDatabaseName("IX_Products_CategoryId");

                entity.HasIndex(p => p.IsActive)
                      .HasDatabaseName("IX_Products_IsActive");
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                // Unique constraint on category name
                entity.HasIndex(c => c.Name)
                      .IsUnique()
                      .HasDatabaseName("IX_Categories_Name_Unique");

                entity.HasIndex(c => c.IsActive)
                      .HasDatabaseName("IX_Categories_IsActive");
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            // Seed Categories
            var categories = new[]
            {
                new Category
                {
                    Id = 1,
                    Name = "Electronics",
                    Description = "Electronic devices and gadgets",
                    CreatedDate = baseDate.AddDays(1),
                    IsActive = true
                },
                new Category
                {
                    Id = 2,
                    Name = "Books",
                    Description = "Books, magazines, and reading materials",
                    CreatedDate = baseDate.AddDays(2),
                    IsActive = true
                },
                new Category
                {
                    Id = 3,
                    Name = "Clothing",
                    Description = "Apparel and fashion items",
                    CreatedDate = baseDate.AddDays(3),
                    IsActive = true
                },
                new Category
                {
                    Id = 4,
                    Name = "Home & Garden",
                    Description = "Home improvement and gardening supplies",
                    CreatedDate = baseDate.AddDays(4),
                    IsActive = true
                }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Seed Products
            var products = new[]
            {
                new Product
                {
                    Id = 1,
                    Name = "Smartphone X1",
                    Description = "Latest smartphone with advanced features",
                    Price = 699.99m,
                    StockQuantity = 50,
                    CategoryId = 1,
                    CreatedDate = baseDate.AddDays(10),
                    LastModifiedDate = baseDate.AddDays(10),
                    IsActive = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Laptop Pro 15",
                    Description = "High-performance laptop for professionals",
                    Price = 1299.99m,
                    StockQuantity = 25,
                    CategoryId = 1,
                    CreatedDate = baseDate.AddDays(11),
                    LastModifiedDate = baseDate.AddDays(11),
                    IsActive = true
                },
                new Product
                {
                    Id = 3,
                    Name = "Programming Fundamentals",
                    Description = "Learn the basics of programming",
                    Price = 49.99m,
                    StockQuantity = 100,
                    CategoryId = 2,
                    CreatedDate = baseDate.AddDays(12),
                    LastModifiedDate = baseDate.AddDays(12),
                    IsActive = true
                },
                new Product
                {
                    Id = 4,
                    Name = "Web Development Mastery",
                    Description = "Advanced web development techniques",
                    Price = 79.99m,
                    StockQuantity = 75,
                    CategoryId = 2,
                    CreatedDate = baseDate.AddDays(13),
                    LastModifiedDate = baseDate.AddDays(13),
                    IsActive = true
                },
                new Product
                {
                    Id = 5,
                    Name = "Cotton T-Shirt",
                    Description = "Comfortable cotton t-shirt",
                    Price = 19.99m,
                    StockQuantity = 200,
                    CategoryId = 3,
                    CreatedDate = baseDate.AddDays(14),
                    LastModifiedDate = baseDate.AddDays(14),
                    IsActive = true
                },
                new Product
                {
                    Id = 6,
                    Name = "Garden Tools Set",
                    Description = "Complete set of gardening tools",
                    Price = 89.99m,
                    StockQuantity = 30,
                    CategoryId = 4,
                    CreatedDate = baseDate.AddDays(15),
                    LastModifiedDate = baseDate.AddDays(15),
                    IsActive = true
                }
            };

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
```

### Phase 5: Service Layer Implementation

#### Step 9: Create Service Interfaces and Implementations
Create `Services/ProductService.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    // Service interface for dependency injection
    public interface IProductService
    {
        Task<IEnumerable<ProductSummaryDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductSummaryDto>> GetActiveProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductSummaryDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductSummaryDto>> SearchProductsAsync(string searchTerm);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> DeactivateProductAsync(int id);
    }

    // Service implementation
    public class ProductService : IProductService
    {
        private readonly ProductCatalogDbContext _context;

        public ProductService(ProductCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductSummaryDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsActive = p.IsActive
                })
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductSummaryDto>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Category.IsActive)
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsActive = p.IsActive
                })
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                Category = new CategoryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                    Description = product.Category.Description,
                    IsActive = product.Category.IsActive,
                    CreatedDate = product.Category.CreatedDate,
                    ProductCount = 0
                }
            };
        }

        public async Task<IEnumerable<ProductSummaryDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive && p.Category.IsActive)
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsActive = p.IsActive
                })
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductSummaryDto>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetActiveProductsAsync();

            var lowerSearchTerm = searchTerm.ToLower();

            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Category.IsActive &&
                           (p.Name.ToLower().Contains(lowerSearchTerm) ||
                            p.Description!.ToLower().Contains(lowerSearchTerm) ||
                            p.Category.Name.ToLower().Contains(lowerSearchTerm)))
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsActive = p.IsActive
                })
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            // Verify the category exists and is active
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == createProductDto.CategoryId && c.IsActive);

            if (category == null)
            {
                throw new ArgumentException($"Category with ID {createProductDto.CategoryId} not found or inactive");
            }

            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                CategoryId = createProductDto.CategoryId,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return (await GetProductByIdAsync(product.Id))!;
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;

            // Verify the category exists and is active if it's being changed
            if (product.CategoryId != updateProductDto.CategoryId)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == updateProductDto.CategoryId && c.IsActive);

                if (category == null)
                {
                    throw new ArgumentException($"Category with ID {updateProductDto.CategoryId} not found or inactive");
                }
            }

            // Update the product properties
            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;
            product.CategoryId = updateProductDto.CategoryId;
            product.IsActive = updateProductDto.IsActive;
            product.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetProductByIdAsync(id);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            product.IsActive = false;
            product.LastModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

#### Step 9B: Create Category Service Implementation
Create `Services/CategoryService.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    // Service interface for dependency injection
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> DeactivateCategoryAsync(int id);
    }

    // Service implementation
    public class CategoryService : ICategoryService
    {
        private readonly ProductCatalogDbContext _context;

        public CategoryService(ProductCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate,
                    ProductCount = c.Products.Count(p => p.IsActive)
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate,
                    ProductCount = c.Products.Count(p => p.IsActive)
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedDate = category.CreatedDate,
                ProductCount = category.Products.Count(p => p.IsActive)
            };
        }

        public async Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products.Where(p => p.IsActive))
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            return new CategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedDate = category.CreatedDate,
                Products = category.Products.Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = category.Name,
                    IsActive = p.IsActive
                }).ToList()
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            // Check if category name already exists
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == createCategoryDto.Name.ToLower());

            if (existingCategory != null)
            {
                throw new ArgumentException($"Category with name '{createCategoryDto.Name}' already exists");
            }

            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return (await GetCategoryByIdAsync(category.Id))!;
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return null;

            // Check if new name conflicts with existing category
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id != id && c.Name.ToLower() == updateCategoryDto.Name.ToLower());

            if (existingCategory != null)
            {
                throw new ArgumentException($"Category with name '{updateCategoryDto.Name}' already exists");
            }

            // Update category properties
            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            category.IsActive = updateCategoryDto.IsActive;

            await _context.SaveChangesAsync();

            return await GetCategoryByIdAsync(id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            // Prevent deletion if category has products
            if (category.Products.Any())
            {
                throw new InvalidOperationException("Cannot delete category with associated products. Remove or reassign products first.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

### Phase 6: Controller Implementation

#### Step 10: Create Products Controller
Create `Controllers/ProductsController.cs`:
```csharp
using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Services;

namespace ProductCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSummaryDto>>> GetProducts(
            [FromQuery] bool activeOnly = true,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? search = null)
        {
            try
            {
                IEnumerable<ProductSummaryDto> products;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    products = await _productService.SearchProductsAsync(search);
                }
                else if (categoryId.HasValue)
                {
                    products = await _productService.GetProductsByCategoryAsync(categoryId.Value);
                }
                else if (activeOnly)
                {
                    products = await _productService.GetActiveProductsAsync();
                }
                else
                {
                    products = await _productService.GetAllProductsAsync();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving products", error = ex.Message });
            }
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                
                if (product == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the product", error = ex.Message });
            }
        }

        // POST /api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdProduct = await _productService.CreateProductAsync(createProductDto);

                return CreatedAtAction(
                    nameof(GetProduct),
                    new { id = createdProduct.Id },
                    createdProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the product", error = ex.Message });
            }
        }

        // PUT /api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, updateProductDto);
                
                if (updatedProduct == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                return Ok(updatedProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the product", error = ex.Message });
            }
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                
                if (!result)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the product", error = ex.Message });
            }
        }

        // POST /api/products/{id}/deactivate
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateProduct(int id)
        {
            try
            {
                var result = await _productService.DeactivateProductAsync(id);
                
                if (!result)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                return Ok(new { message = $"Product with ID {id} has been deactivated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deactivating the product", error = ex.Message });
            }
        }
    }
}
```

Create `Controllers/CategoriesController.cs`:
```csharp
using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Services;

namespace ProductCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET /api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving categories", error = ex.Message });
            }
        }

        // GET /api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                
                if (category == null)
                {
                    return NotFound(new { message = $"Category with ID {id} not found" });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the category", error = ex.Message });
            }
        }

        // GET /api/categories/{id}/products
        [HttpGet("{id}/products")]
        public async Task<ActionResult<CategoryWithProductsDto>> GetCategoryWithProducts(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryWithProductsAsync(id);
                
                if (category == null)
                {
                    return NotFound(new { message = $"Category with ID {id} not found" });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the category with products", error = ex.Message });
            }
        }

        // POST /api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryDto);

                return CreatedAtAction(
                    nameof(GetCategory),
                    new { id = createdCategory.Id },
                    createdCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the category", error = ex.Message });
            }
        }

        // PUT /api/categories/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
                
                if (updatedCategory == null)
                {
                    return NotFound(new { message = $"Category with ID {id} not found" });
                }

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the category", error = ex.Message });
            }
        }

        // DELETE /api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                
                if (!result)
                {
                    return NotFound(new { message = $"Category with ID {id} not found or has associated products" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the category", error = ex.Message });
            }
        }
    }
}
```

### Phase 7: Dependency Injection and Configuration

#### Step 11: Configure Services in Program.cs
Update `Program.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework with SQLite
builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
    options.UseSqlite("Data Source=ProductCatalog.db"));

// Register services for dependency injection
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Add controllers with JSON options configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization to handle reference loops
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        // Use camelCase for property names in JSON responses
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Configure API documentation with Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product Catalog API",
        Version = "v1",
        Description = "A comprehensive ASP.NET Core Web API demonstrating REST principles, Entity Framework, and clean architecture patterns."
    });
});

// Configure CORS for web client integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure database is created and seeded at startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();
    context.Database.EnsureCreated();
}

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger only in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at application root
    });
}

// Enable CORS
app.UseCors("AllowAll");

// Enable HTTPS redirection for security
app.UseHttpsRedirection();

// Enable authorization (although not using authentication in this demo)
app.UseAuthorization();

// Map controller routes
app.MapControllers();

app.Run();
```

### Phase 8: Testing and Validation

#### Step 12: Build and Run the Application
```powershell
# Build the application
dotnet build

# Run the application
dotnet run
```

#### Step 13: Test API Endpoints
1. **Navigate to** `https://localhost:7xxx` to access Swagger UI
2. **Test the following endpoints**:
   - `GET /api/products` - Get all products
   - `GET /api/products/{id}` - Get specific product
   - `POST /api/products` - Create new product
   - `PUT /api/products/{id}` - Update product
   - `DELETE /api/products/{id}` - Delete product
   - `GET /api/categories` - Get all categories

#### Step 14: Test with HTTP Client
Create `ProductCatalogAPI.http` for testing:
```http
### Get all products
GET https://localhost:7xxx/api/products

### Get product by ID
GET https://localhost:7xxx/api/products/1

### Search products
GET https://localhost:7xxx/api/products?search=smartphone

### Create new product
POST https://localhost:7xxx/api/products
Content-Type: application/json

{
  "name": "New Product",
  "description": "Product description",
  "price": 99.99,
  "stockQuantity": 10,
  "categoryId": 1
}

### Update product
PUT https://localhost:7xxx/api/products/1
Content-Type: application/json

{
  "name": "Updated Product",
  "description": "Updated description",
  "price": 129.99,
  "stockQuantity": 15,
  "categoryId": 1,
  "isActive": true
}
```

## Advanced Features and Best Practices

### 1. Error Handling and Validation
The API implements comprehensive error handling:
- **Model Validation**: Data annotations ensure input validity
- **Business Logic Validation**: Service layer validates business rules
- **Exception Handling**: Controllers catch and handle exceptions appropriately
- **Consistent Error Responses**: Standardized error message format

### 2. Performance Optimization Techniques
- **Async/Await Pattern**: All database operations are asynchronous
- **Database Indexing**: Strategic indexes for frequently queried columns
- **Projection Queries**: Select only required data to reduce payload size
- **Eager Loading**: Use Include() to avoid N+1 query problems

### 3. Security Considerations
- **Input Validation**: Prevent injection attacks through validation
- **Parameter Binding**: Use [FromBody] and [FromQuery] explicitly
- **CORS Configuration**: Controlled cross-origin resource sharing
- **HTTPS Enforcement**: Secure communication protocols

### 4. API Design Patterns
- **Resource-Based URLs**: Clear, intuitive endpoint structure
- **HTTP Status Codes**: Proper status codes for different scenarios
- **RESTful Principles**: Stateless operations with standard HTTP verbs
- **Content Negotiation**: JSON as primary content type

### 5. Testing Strategies

#### Unit Testing Example
```csharp
[Test]
public async Task GetProductByIdAsync_ExistingProduct_ReturnsProduct()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ProductCatalogDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    using var context = new ProductCatalogDbContext(options);
    var service = new ProductService(context);

    var category = new Category { Id = 1, Name = "Electronics", IsActive = true };
    var product = new Product 
    { 
        Id = 1, 
        Name = "Test Product", 
        Price = 100.00m, 
        CategoryId = 1, 
        IsActive = true 
    };

    context.Categories.Add(category);
    context.Products.Add(product);
    await context.SaveChangesAsync();

    // Act
    var result = await service.GetProductByIdAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test Product", result.Name);
    Assert.Equal(100.00m, result.Price);
}
```

#### Integration Testing with WebApplicationFactory
```csharp
public class ProductsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ProductsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task GetProducts_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
    }
}
```

### 6. Database Migration Management

#### Creating Migrations
```powershell
# Add new migration
dotnet ef migrations add AddProductTables

# Update database
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Remove last migration (if not applied)
dotnet ef migrations remove
```

#### Production Migration Strategy
```csharp
// In Program.cs for production
if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();
    
    // Apply pending migrations
    context.Database.Migrate();
}
```

### 7. Logging and Monitoring

#### Structured Logging with Serilog
```csharp
// Install: dotnet add package Serilog.AspNetCore
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
```

#### Application Insights Integration
```csharp
// Install: dotnet add package Microsoft.ApplicationInsights.AspNetCore
builder.Services.AddApplicationInsightsTelemetry();
```

### 8. Caching Strategies

#### In-Memory Caching
```csharp
// In Program.cs
builder.Services.AddMemoryCache();

// In ProductService
private readonly IMemoryCache _cache;

public async Task<ProductDto?> GetProductByIdAsync(int id)
{
    string cacheKey = $"product_{id}";
    
    if (_cache.TryGetValue(cacheKey, out ProductDto cachedProduct))
    {
        return cachedProduct;
    }

    var product = await GetProductFromDatabase(id);
    
    if (product != null)
    {
        _cache.Set(cacheKey, product, TimeSpan.FromMinutes(15));
    }

    return product;
}
```

#### Redis Distributed Caching
```csharp
// Install: dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});
```

### 9. API Versioning

#### URL Path Versioning
```csharp
// Install: dotnet add package Microsoft.AspNetCore.Mvc.Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProductsController : ControllerBase
{
    // Implementation
}
```

### 10. Authentication and Authorization

#### JWT Bearer Authentication
```csharp
// Install: dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-issuer",
            ValidAudience = "your-audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"))
        };
    });

[ApiController]
[Authorize] // Require authentication for all actions
public class ProductsController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous] // Allow anonymous access to this action
    public async Task<ActionResult<IEnumerable<ProductSummaryDto>>> GetProducts()
    {
        // Implementation
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Require specific role
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        // Implementation
    }
}
```

## Deployment Strategies

### 1. Docker Containerization
Create `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProductCatalogAPI.csproj", "./"]
RUN dotnet restore "ProductCatalogAPI.csproj"
COPY . .
RUN dotnet build "ProductCatalogAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductCatalogAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalogAPI.dll"]
```

Create `docker-compose.yml`:
```yaml
version: '3.8'

services:
  api:
    build: .
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Data Source=/app/data/ProductCatalog.db
    volumes:
      - ./data:/app/data

  redis:
    image: redis:alpine
    ports:
      - "6379:6379"
```

### 2. Azure App Service Deployment
```powershell
# Install Azure CLI and login
az login

# Create resource group
az group create --name ProductCatalogRG --location "East US"

# Create App Service plan
az appservice plan create --name ProductCatalogPlan --resource-group ProductCatalogRG --sku B1

# Create web app
az webapp create --resource-group ProductCatalogRG --plan ProductCatalogPlan --name ProductCatalogAPI --runtime "DOTNET|8.0"

# Deploy from local folder
az webapp deployment source config-zip --resource-group ProductCatalogRG --name ProductCatalogAPI --src ./publish.zip
```

### 3. CI/CD with GitHub Actions
Create `.github/workflows/deploy.yml`:
```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v2

    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 8.0.x

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore

    - name: Test
      run: dotnet test --no-build --verbosity normal

    - name: Publish
      run: dotnet publish -c Release -o ./publish

    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'ProductCatalogAPI'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
```

## Troubleshooting Common Issues

### 1. Database Connection Issues
```csharp
// Check connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String: {connectionString}");

// Test database connection
try
{
    using var context = new ProductCatalogDbContext(options);
    await context.Database.CanConnectAsync();
    Console.WriteLine("Database connection successful");
}
catch (Exception ex)
{
    Console.WriteLine($"Database connection failed: {ex.Message}");
}
```

### 2. CORS Issues
```csharp
// More specific CORS policy for production
builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins("https://yourfrontend.com")
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .WithHeaders("Content-Type", "Authorization");
    });
});
```

### 3. Swagger Not Loading
```csharp
// Ensure Swagger is only enabled in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API v1");
        c.RoutePrefix = string.Empty;
    });
}
```

## Performance Monitoring

### 1. Application Performance Monitoring
```csharp
// Custom middleware for request timing
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        await _next(context);
        
        stopwatch.Stop();
        
        if (stopwatch.ElapsedMilliseconds > 1000) // Log slow requests
        {
            _logger.LogWarning("Slow request: {Method} {Path} took {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

// Register middleware
app.UseMiddleware<RequestTimingMiddleware>();
```

### 2. Health Checks
```csharp
// Install: dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ProductCatalogDbContext>();

app.MapHealthChecks("/health");
```

## Extensions and Improvements

### Potential Enhancements
1. **GraphQL Integration**: Flexible query capabilities for clients
2. **Real-time Updates**: SignalR for live inventory updates
3. **Background Services**: Hosted services for inventory management
4. **Rate Limiting**: Protect API from abuse
5. **Response Compression**: Reduce bandwidth usage
6. **OpenTelemetry**: Distributed tracing and metrics
7. **Feature Flags**: Toggle features without deployment
8. **Audit Logging**: Track all data changes
9. **Bulk Operations**: Efficient batch processing
10. **Advanced Search**: Full-text search with Elasticsearch

### Advanced API Features
1. **HATEOAS**: Hypermedia as the Engine of Application State
2. **ETags**: Optimistic concurrency control
3. **Conditional Requests**: If-Modified-Since headers
4. **Content Negotiation**: Support multiple response formats
5. **API Gateway Integration**: Centralized API management

## Related Learning Resources

### Official Documentation
- [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [RESTful API Design](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)

### Advanced Topics
- **Clean Architecture**: Robert C. Martin's architectural principles
- **Domain-Driven Design**: Complex business logic organization
- **CQRS Pattern**: Command Query Responsibility Segregation
- **Event Sourcing**: Event-driven architecture patterns
- **Microservices**: Distributed system architecture

---

This comprehensive Product Catalog Web API project demonstrates modern web development practices, RESTful design principles, and production-ready implementation patterns. The step-by-step guide ensures developers can recreate and understand every aspect of building a robust, scalable Web API from scratch.

