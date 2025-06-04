using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    /// <summary>
    /// Interface defining the contract for product operations.
    /// Interfaces are crucial in Web APIs because they:
    /// 1. Enable dependency injection and loose coupling
    /// 2. Make the code more testable (we can mock this interface)
    /// 3. Define clear contracts for what operations are available
    /// 4. Allow multiple implementations (e.g., database vs. cache vs. external API)
    /// </summary>
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

    /// <summary>
    /// Implementation of the product service.
    /// This is where we implement the actual business logic for product operations.
    /// Notice how we depend on the DbContext interface - this follows dependency injection principles.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ProductCatalogDbContext _context;

        /// <summary>
        /// Constructor injection. The DI container will provide the DbContext.
        /// This is a fundamental pattern in modern Web APIs.
        /// </summary>
        public ProductService(ProductCatalogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all products (including inactive ones).
        /// We return DTOs instead of entities to control what data is exposed through the API.
        /// Using async/await is essential for Web APIs to maintain scalability under load.
        /// </summary>
        public async Task<IEnumerable<ProductSummaryDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category) // Load related category data
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

        /// <summary>
        /// Gets only active products. This is probably what most API clients want.
        /// By having separate methods, we give API consumers clear choices.
        /// </summary>
        public async Task<IEnumerable<ProductSummaryDto>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Category.IsActive) // Only show products from active categories
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

        /// <summary>
        /// Gets a single product by ID with full details.
        /// Returns null if not found - the controller will convert this to a 404 response.
        /// </summary>
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
                    ProductCount = 0 // We don't calculate this here to avoid performance issues
                }
            };
        }

        /// <summary>
        /// Gets products by category. This is a common API operation.
        /// Notice we filter by active status - most clients don't want inactive products.
        /// </summary>
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

        /// <summary>
        /// Search functionality. Essential for any product catalog API.
        /// We search across multiple fields to give users flexible search capabilities.
        /// </summary>
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

        /// <summary>
        /// Creates a new product from the DTO.
        /// Notice how we validate the category exists and set audit fields.
        /// This is business logic that belongs in the service layer, not the controller.
        /// </summary>
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

            // Return the created product with full details
            return (await GetProductByIdAsync(product.Id))!;
        }

        /// <summary>
        /// Updates an existing product.
        /// Returns null if the product doesn't exist, which the controller converts to 404.
        /// </summary>
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

        /// <summary>
        /// Hard delete a product. Use with caution!
        /// In most production systems, you'd use soft delete (DeactivateProductAsync) instead.
        /// </summary>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Soft delete - marks product as inactive instead of removing it.
        /// This preserves historical data and is safer for production systems.
        /// </summary>
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
