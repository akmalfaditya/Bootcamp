using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    /// <summary>
    /// Interface for category operations.
    /// Categories are simpler than products but still need proper service layer abstraction.
    /// </summary>
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

    /// <summary>
    /// Category service implementation.
    /// Similar patterns to ProductService but simpler business logic.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ProductCatalogDbContext _context;

        public CategoryService(ProductCatalogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all categories with product counts.
        /// The product count is useful information for API clients.
        /// </summary>
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
                    ProductCount = c.Products.Count(p => p.IsActive) // Only count active products
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets only active categories.
        /// Most API clients will want this version.
        /// </summary>
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

        /// <summary>
        /// Gets a single category by ID.
        /// </summary>
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            var productCount = await _context.Products
                .CountAsync(p => p.CategoryId == id && p.IsActive);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedDate = category.CreatedDate,
                ProductCount = productCount
            };
        }

        /// <summary>
        /// Gets a category with all its products.
        /// Use this endpoint sparingly as it can return large amounts of data.
        /// </summary>
        public async Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
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
                Products = category.Products
                    .Where(p => p.IsActive)
                    .Select(p => new ProductSummaryDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryName = category.Name,
                        IsActive = p.IsActive
                    })
                    .OrderBy(p => p.Name)
                    .ToList()
            };
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            // Check if category name already exists (case-insensitive)
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

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return null;

            // Check if the new name conflicts with existing categories (excluding current one)
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id != id && c.Name.ToLower() == updateCategoryDto.Name.ToLower());

            if (existingCategory != null)
            {
                throw new ArgumentException($"Category with name '{updateCategoryDto.Name}' already exists");
            }

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            category.IsActive = updateCategoryDto.IsActive;

            await _context.SaveChangesAsync();

            return await GetCategoryByIdAsync(id);
        }

        /// <summary>
        /// Hard delete a category.
        /// This will fail if there are products in the category due to foreign key constraints.
        /// </summary>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            // Check if category has products
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
            {
                throw new InvalidOperationException("Cannot delete category that contains products. Remove or reassign products first.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Soft delete - marks category as inactive.
        /// This also deactivates all products in the category.
        /// </summary>
        public async Task<bool> DeactivateCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            category.IsActive = false;

            // Also deactivate all products in this category
            foreach (var product in category.Products)
            {
                product.IsActive = false;
                product.LastModifiedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
