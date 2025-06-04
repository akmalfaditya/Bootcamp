using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.Models
{
    /// <summary>
    /// Represents a product category in our system.
    /// Categories help organize products and make the API more useful for filtering and searching.
    /// This demonstrates a one-to-many relationship where one category can have many products.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Primary key for the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Category name with appropriate validation.
        /// Each category should have a unique, meaningful name.
        /// </summary>
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional description for the category.
        /// This helps API consumers understand what types of products belong in this category.
        /// </summary>
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property representing all products in this category.
        /// This is the "many" side of the one-to-many relationship.
        /// Virtual enables lazy loading, and we initialize it to avoid null reference issues.
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// When this category was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Whether this category is still active.
        /// Inactive categories might be hidden from API responses but preserved for historical data.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
