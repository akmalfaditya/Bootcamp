using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new product.
    /// DTOs are essential in Web APIs because they:
    /// 1. Decouple the API contract from internal data models
    /// 2. Allow us to control exactly what data is exposed
    /// 3. Provide validation specific to the API operation
    /// 4. Prevent over-posting attacks (where clients send extra data)
    /// </summary>
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

    /// <summary>
    /// DTO for updating an existing product.
    /// Notice this is similar to CreateProductDto but might have different validation rules.
    /// In some cases, you might want to allow partial updates.
    /// </summary>
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

        /// <summary>
        /// Allow clients to activate/deactivate products through the API.
        /// This is a common pattern instead of hard deletes.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO for returning product data to clients.
    /// This controls exactly what information is exposed to API consumers.
    /// Notice we include category information but not internal timestamps that clients don't need.
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// We include category information so clients don't need to make separate API calls.
        /// This reduces the number of HTTP requests (following REST efficiency principles).
        /// </summary>
        public CategoryDto Category { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    /// <summary>
    /// Simple product DTO for list views where we don't need all the details.
    /// This improves API performance by reducing payload size.
    /// </summary>
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
