# Product Catalog Web API

## Learning Objectives

After completing this tutorial, you will understand:
- How to create ASP.NET Core Web API from scratch
- Proper REST API principles implementation
- Entity Framework Core usage for database operations
- Clean Architecture and Separation of Concerns
- Dependency Injection patterns
- Database migrations and seeding
- API documentation with Swagger/OpenAPI

## Prerequisites

Make sure you have installed:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Code editor (Visual Studio, VS Code, or JetBrains Rider)
- Git for version control
- Basic understanding of C#, HTTP, and REST concepts

## Step-by-Step Tutorial

### Step 1: Create New Project

```bash
# Open terminal/command prompt in your desired folder
cd "C:\Your\Development\Folder"

# Create new Web API project with controllers template
dotnet new webapi -n "ProductCatalogAPI" --use-controllers

# Navigate to project folder
cd ProductCatalogAPI

# Test if project can be built
dotnet build
```

**Explanation:**
- `dotnet new webapi` creates a Web API template project
- `--use-controllers` uses controller-based routing (not minimal APIs)
- `-n` specifies the project name

### Step 2: Install Dependencies

```bash
# Install Entity Framework Core packages
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design

# Verify packages are installed
dotnet list package
```

**Explanation:**
- `EntityFrameworkCore.Sqlite`: Database provider for SQLite
- `EntityFrameworkCore.Tools`: Command-line tools for migrations
- `EntityFrameworkCore.Design`: Design-time services for EF Core

### Step 3: Create Models (Domain Entities)

Create `Models` folder and add the following files:

**3.1. Models/Category.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.Models
{
    /// <summary>
    /// Represents a product category in our system.
    /// This is a domain entity that maps directly to the database table.
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation property: One category can have many products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
```

**3.2. Models/Product.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.Models
{
    /// <summary>
    /// Represents a product in our catalog.
    /// Contains validation rules and business logic for products.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        // Foreign key to Category
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation property: Many products belong to one category
        public virtual Category Category { get; set; } = null!;

        // Business logic methods
        public bool IsInStock() => StockQuantity > 0;
        public bool IsLowStock(int threshold = 10) => StockQuantity <= threshold && StockQuantity > 0;
    }
}
```

### Step 4: Create DTOs (Data Transfer Objects)

Create a `DTOs` folder to separate API contracts from internal models:

**4.1. DTOs/CategoryDtos.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.DTOs
{
    // DTO for creating new category
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }
    }

    // DTO for updating category
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // DTO for category response
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }

    // DTO for category with products
    public class CategoryWithProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<ProductSummaryDto> Products { get; set; } = new();
    }
}
```

**4.2. DTOs/ProductDtos.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.DTOs
{
    // DTO for creating new product
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }
    }

    // DTO for updating product
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // DTO for complete product response
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsInStock { get; set; }
        public bool IsLowStock { get; set; }
    }

    // DTO for product summary (used in lists)
    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsInStock { get; set; }
    }
}
```

### Step 5: Create Database Context

Create a `Data` folder and add:

**5.1. Data/ProductCatalogDbContext.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Data
{    /// <summary>
    /// Database context for Product Catalog API.
    /// This is the bridge between the C# application and the database.
    /// </summary>
    public class ProductCatalogDbContext : DbContext
    {
        public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options)
        {
        }

        // DbSet represents tables in the database
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                // Setup relationship between Product and Category
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure decimal precision for Price
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");

                // Indexes for performance
                entity.HasIndex(p => p.CategoryId);
                entity.HasIndex(p => p.IsActive);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                // Unique constraint on category name
                entity.HasIndex(c => c.Name)
                      .IsUnique();

                entity.HasIndex(c => c.IsActive);
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
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets", CreatedDate = baseDate.AddDays(1), IsActive = true },
                new Category { Id = 2, Name = "Books", Description = "Books, magazines, and reading materials", CreatedDate = baseDate.AddDays(2), IsActive = true },
                new Category { Id = 3, Name = "Clothing", Description = "Apparel and fashion items", CreatedDate = baseDate.AddDays(3), IsActive = true },
                new Category { Id = 4, Name = "Home & Garden", Description = "Home improvement and gardening supplies", CreatedDate = baseDate.AddDays(4), IsActive = true }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Seed Products
            var products = new[]
            {
                new Product { Id = 1, Name = "Smartphone X1", Description = "Latest smartphone with advanced features", Price = 699.99m, StockQuantity = 50, CategoryId = 1, CreatedDate = baseDate.AddDays(10), LastModifiedDate = baseDate.AddDays(10), IsActive = true },
                new Product { Id = 2, Name = "Laptop Pro 15", Description = "High-performance laptop for professionals", Price = 1299.99m, StockQuantity = 25, CategoryId = 1, CreatedDate = baseDate.AddDays(11), LastModifiedDate = baseDate.AddDays(11), IsActive = true },
                new Product { Id = 3, Name = "Programming Fundamentals", Description = "Learn the basics of programming", Price = 49.99m, StockQuantity = 100, CategoryId = 2, CreatedDate = baseDate.AddDays(12), LastModifiedDate = baseDate.AddDays(12), IsActive = true },
                new Product { Id = 4, Name = "Web Development Mastery", Description = "Advanced web development techniques", Price = 79.99m, StockQuantity = 75, CategoryId = 2, CreatedDate = baseDate.AddDays(13), LastModifiedDate = baseDate.AddDays(13), IsActive = true },
                new Product { Id = 5, Name = "Cotton T-Shirt", Description = "Comfortable cotton t-shirt", Price = 19.99m, StockQuantity = 200, CategoryId = 3, CreatedDate = baseDate.AddDays(14), LastModifiedDate = baseDate.AddDays(14), IsActive = true },
                new Product { Id = 6, Name = "Garden Tools Set", Description = "Complete set of gardening tools", Price = 89.99m, StockQuantity = 30, CategoryId = 4, CreatedDate = baseDate.AddDays(15), LastModifiedDate = baseDate.AddDays(15), IsActive = true }
            };

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
```

### Step 6: Create Service Layer

Create a `Services` folder for business logic:

**6.1. Services/CategoryService.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }

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
                .Where(c => c.IsActive)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate,
                    IsActive = c.IsActive
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id && c.IsActive)
                .FirstOrDefaultAsync();

            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                IsActive = category.IsActive
            };
        }

        public async Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products.Where(p => p.IsActive))
                .Where(c => c.Id == id && c.IsActive)
                .FirstOrDefaultAsync();

            if (category == null) return null;

            return new CategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                IsActive = category.IsActive,
                Products = category.Products.Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = category.Name,
                    IsInStock = p.IsInStock()
                }).ToList()
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                IsActive = category.IsActive
            };
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || !category.IsActive) return null;

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            category.IsActive = updateCategoryDto.IsActive;

            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                IsActive = category.IsActive
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            // Check if category has products
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id && p.IsActive);
            if (hasProducts) return false;

            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

**6.2. Services/ProductService.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductSummaryDto>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductSummaryDto>> SearchProductsAsync(string query);
        Task<IEnumerable<ProductSummaryDto>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly ProductCatalogDbContext _context;

        public ProductService(ProductCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductSummaryDto>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsInStock = p.StockQuantity > 0
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id && p.IsActive)
                .FirstOrDefaultAsync();

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive,
                IsInStock = product.IsInStock(),
                IsLowStock = product.IsLowStock()
            };
        }

        public async Task<IEnumerable<ProductSummaryDto>> SearchProductsAsync(string query)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && 
                           (p.Name.Contains(query) || 
                            (p.Description != null && p.Description.Contains(query))))
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsInStock = p.StockQuantity > 0
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductSummaryDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsInStock = p.StockQuantity > 0
                })
                .ToListAsync();
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
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

            // Load category for response
            await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive,
                IsInStock = product.IsInStock(),
                IsLowStock = product.IsLowStock()
            };
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null || !product.IsActive) return null;

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;
            product.CategoryId = updateProductDto.CategoryId;
            product.IsActive = updateProductDto.IsActive;
            product.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Reload category if changed
            await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive,
                IsInStock = product.IsInStock(),
                IsLowStock = product.IsLowStock()
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            // Soft delete
            product.IsActive = false;
            product.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

### Step 7: Create Controllers

Create controllers in the `Controllers` folder:

**7.1. Controllers/CategoriesController.cs**
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

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return Ok(category);
        }

        // GET: api/categories/5/products
        [HttpGet("{id}/products")]
        public async Task<ActionResult<CategoryWithProductsDto>> GetCategoryWithProducts(int id)
        {
            var category = await _categoryService.GetCategoryWithProductsAsync(id);
            
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating category: {ex.Message}");
            }
        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return Ok(category);
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            
            if (!result)
            {
                return NotFound($"Category with ID {id} not found or has associated products.");
            }

            return NoContent();
        }
    }
}
```

**7.2. Controllers/ProductsController.cs**
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

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSummaryDto>>> GetProducts(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var products = await _productService.GetAllProductsAsync(pageNumber, pageSize);
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }

        // GET: api/products/search?query=laptop
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductSummaryDto>>> SearchProducts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query is required.");
            }

            var products = await _productService.SearchProductsAsync(query);
            return Ok(products);
        }

        // GET: api/products/category/1
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductSummaryDto>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.CreateProductAsync(createProductDto);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating product: {ex.Message}");
            }
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.UpdateProductAsync(id, updateProductDto);
            
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            
            if (!result)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
```

### Step 8: Configure Program.cs

Update `Program.cs` for dependency injection and configuration:

**8.1. Program.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework with SQLite
// In real applications, connection strings are usually stored in appsettings.json
builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
    options.UseSqlite("Data Source=ProductCatalog.db"));

// Register services for dependency injection
// This follows the dependency inversion principle - controllers depend on abstractions
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Add controllers with JSON configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {        // Configure JSON serialization to handle reference loops
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
// In production, use migrations through deployment pipelines
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

**Program.cs Explanation:**
- **Dependency Injection**: All services are registered in the DI container
- **Entity Framework**: Database configuration with SQLite
- **JSON Options**: Serialization settings for API responses
- **Swagger**: Interactive API documentation
- **CORS**: Allow cross-origin requests
- **Database Setup**: Auto-create and seed database at startup

### Step 9: Create and Run Migrations

```bash
# Create first migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update

# Verify migration succeeded
dotnet ef migrations list
```

**Migrations Explanation:**
- **InitialCreate**: First migration that creates all tables
- **Auto-generated**: EF Core generates SQL based on model classes
- **Version Control**: Migrations can be tracked in git for deployment

### Step 10: Testing API

```bash
# Run the application
dotnet run

# Application will open at:
# - HTTPS: https://localhost:7128
# - HTTP: http://localhost:5011
# - Swagger UI: https://localhost:7128 (opens automatically)
```

**Testing with Swagger UI:**
1. Open browser to `https://localhost:7128`
2. Use Swagger interface to test endpoints
3. Try all CRUD operations for Products and Categories
4. Notice response codes and data structure

**Testing with curl:**
```bash
# Get all products
curl -X GET "https://localhost:7128/api/products" -H "accept: application/json"

# Get product by ID
curl -X GET "https://localhost:7128/api/products/1" -H "accept: application/json"

# Create new product
curl -X POST "https://localhost:7128/api/products" \
     -H "accept: application/json" \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Test Product",
       "description": "A test product",
       "price": 99.99,
       "stockQuantity": 10,
       "categoryId": 1
     }'
```

## Key Learning Concepts

### 1. REST API Principles
- **HTTP Verbs**: GET, POST, PUT, DELETE
- **Status Codes**: 200, 201, 400, 404, 500
- **Resource Naming**: `/api/products`, `/api/categories`
- **Stateless**: Each request is independent

### 2. Clean Architecture
- **Separation of Concerns**: Controllers, Services, Models, DTOs
- **Dependency Injection**: Loose coupling between components
- **Testability**: Business logic separated from framework

### 3. Entity Framework Core
- **Code First**: Model classes determine database schema
- **Migrations**: Version control for database changes
- **Relationships**: One-to-Many between Category and Product
- **Querying**: LINQ to Entities for database operations

### 4. Data Transfer Objects (DTOs)
- **API Contracts**: Data structure received/sent by API
- **Validation**: Data annotations for input validation
- **Versioning**: DTO changes don't affect database model

## Key Features Implemented

- **RESTful API Design** - Proper HTTP verbs, status codes, and resource naming
- **Entity Framework Core** - Database operations with SQLite
- **Clean Architecture** - Separation of concerns with models, DTOs, services, and controllers
- **Dependency Injection** - Loosely coupled components for maintainability
- **Data Validation** - Model validation with Data Annotations
- **API Documentation** - Interactive Swagger/OpenAPI documentation
- **Database Migrations** - Version-controlled database schema changes
- **Seed Data** - Pre-populated data for immediate testing

## Architecture Overview

```
ProductCatalogAPI/
├── Controllers/         # API Controllers (REST endpoints)
├── Models/             # Domain entities/models
├── DTOs/               # Data Transfer Objects (API contracts)
├── Services/           # Business logic layer
├── Data/               # Database context and configurations
├── Migrations/         # Entity Framework migrations
└── Program.cs          # Application configuration and DI setup
```

### Layer Responsibilities

- **Controllers**: Handle HTTP requests/responses, routing, and status codes
- **Services**: Contain business logic and data access operations
- **DTOs**: Define API contracts separate from internal models
- **Models**: Represent database entities and domain logic
- **Data**: Database context, configurations, and seed data

## Technologies Used

- **ASP.NET Core 8.0** - Modern web framework
- **Entity Framework Core** - Object-relational mapping (ORM)
- **SQLite** - Lightweight database for development
- **Swagger/OpenAPI** - API documentation and testing
- **Data Annotations** - Model validation
- **Dependency Injection** - Built-in IoC container

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Code editor (Visual Studio, VS Code, or JetBrains Rider)
- Basic understanding of C#, REST APIs, and HTTP

## Getting Started

### 1. Clone and Setup

```bash
# Navigate to the project directory
cd ProductCatalogAPI

# Restore NuGet packages
dotnet restore

# Build the project
dotnet build
```

### 2. Database Setup

The project uses Entity Framework Code First with migrations:

```bash
# Create migration (already done)
dotnet ef migrations add InitialCreate

# Apply migration to create database
dotnet ef database update
```

This creates a SQLite database file `ProductCatalog.db` with pre-seeded data.

### 3. Run the Application

```bash
# Start the development server
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:7139`
- **HTTP**: `http://localhost:5027`
- **Swagger UI**: `https://localhost:7139` (opens automatically)

## API Endpoints

### Products API

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/products` | Get all products with pagination |
| `GET` | `/api/products/{id}` | Get product by ID |
| `GET` | `/api/products/search?query={term}` | Search products |
| `GET` | `/api/products/category/{categoryId}` | Get products by category |
| `POST` | `/api/products` | Create new product |
| `PUT` | `/api/products/{id}` | Update existing product |
| `DELETE` | `/api/products/{id}` | Delete product (soft delete) |

### Categories API

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/categories` | Get all categories |
| `GET` | `/api/categories/{id}` | Get category by ID |
| `GET` | `/api/categories/{id}/products` | Get category with products |
| `POST` | `/api/categories` | Create new category |
| `PUT` | `/api/categories/{id}` | Update existing category |
| `DELETE` | `/api/categories/{id}` | Delete category |

## Testing the API

### Using Swagger UI

1. Run the application (`dotnet run`)
2. Open browser to `https://localhost:7139`
3. Use the interactive Swagger interface to test endpoints

### Using curl Examples

```bash
# Get all products
curl -X GET "https://localhost:7139/api/products" -H "accept: application/json"

# Get product by ID
curl -X GET "https://localhost:7139/api/products/1" -H "accept: application/json"

# Create new product
curl -X POST "https://localhost:7139/api/products" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "New Product",
    "description": "Product description",
    "price": 29.99,
    "stockQuantity": 10,
    "categoryId": 1
  }'

# Search products
curl -X GET "https://localhost:7139/api/products/search?query=laptop" -H "accept: application/json"
```

### Sample Request/Response

**GET /api/products/1**

Response:
```json
{
  "id": 1,
  "name": "Smartphone X1",
  "description": "Latest smartphone with advanced features",
  "price": 699.99,
  "stockQuantity": 50,
  "categoryId": 1,
  "categoryName": "Electronics",
  "createdDate": "2024-01-11T00:00:00Z",
  "lastModifiedDate": "2024-01-11T00:00:00Z",
  "isActive": true
}
```

## Database Schema

### Products Table
- `Id` (Primary Key, Auto-increment)
- `Name` (Required, Max 100 chars)
- `Description` (Optional, Max 500 chars)
- `Price` (Decimal 18,2, Required, > 0)
- `StockQuantity` (Integer, Required, >= 0)
- `CategoryId` (Foreign Key, Required)
- `CreatedDate` (DateTime, Required)
- `LastModifiedDate` (DateTime, Required)
- `IsActive` (Boolean, Required)

### Categories Table
- `Id` (Primary Key, Auto-increment)
- `Name` (Required, Max 50 chars, Unique)
- `Description` (Optional, Max 200 chars)
- `CreatedDate` (DateTime, Required)
- `IsActive` (Boolean, Required)

### Relationships
- One Category can have many Products (One-to-Many)
- Products cannot be deleted if referenced by Categories (Restrict)

## Key Learning Concepts Demonstrated

### 1. RESTful API Design
- Proper HTTP verb usage (GET, POST, PUT, DELETE)
- Meaningful HTTP status codes (200, 201, 400, 404, 500)
- Resource-based URL structure (`/api/products/{id}`)
- Query parameters for filtering and searching

### 2. Entity Framework Core
- Code First approach with migrations
- Relationships (One-to-Many)
- Indexes for performance optimization
- Seed data for testing
- Async/await patterns for database operations

### 3. Clean Architecture
- Separation of concerns across layers
- Dependency Injection for loose coupling
- DTOs for API contract isolation
- Service layer for business logic

### 4. Data Validation
- Model validation with Data Annotations
- Custom validation logic
- Proper error handling and response formatting

### 5. API Documentation
- Swagger/OpenAPI integration
- Interactive API testing interface
- Comprehensive endpoint documentation

## Development Notes

### Adding New Features

1. **New Model**: Add to `Models/` folder with proper validation attributes
2. **New DTO**: Create corresponding DTOs in `DTOs/` folder
3. **Service Layer**: Implement business logic in `Services/`
4. **Controller**: Add REST endpoints in `Controllers/`
5. **Database**: Create migration with `dotnet ef migrations add [Name]`

### Best Practices Implemented

- **Async/Await**: All database operations are asynchronous
- **Error Handling**: Comprehensive exception handling in controllers
- **Validation**: Multi-layer validation (model + business logic)
- **Status Codes**: Proper HTTP status code usage
- **Security**: CORS configuration for cross-origin requests
- **Performance**: Database indexes on frequently queried columns


## Additional Resources

- [ASP.NET Core Web API Tutorial](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [REST API Design Best Practices](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)
- [HTTP Status Codes Reference](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status)

