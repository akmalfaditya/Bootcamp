using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.DTOs
{
    /// <summary>
    /// DTO for creating a new category.
    /// Keep it simple - categories don't need complex validation.
    /// </summary>
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO for updating categories.
    /// </summary>
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO for returning category data.
    /// Notice we don't include the Products collection by default to avoid circular references
    /// and huge payloads. If clients need products, they should call the products endpoint.
    /// </summary>
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Simple count of products in this category.
        /// This gives clients useful information without loading all the product data.
        /// </summary>
        public int ProductCount { get; set; }
    }

    /// <summary>
    /// Extended category DTO that includes product information.
    /// Use this sparingly and only when clients specifically need the related data.
    /// </summary>
    public class CategoryWithProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Use the summary DTO to avoid exposing too much data.
        /// </summary>
        public List<ProductSummaryDto> Products { get; set; } = new List<ProductSummaryDto>();
    }
}
