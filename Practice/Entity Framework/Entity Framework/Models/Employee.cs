
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models
{
    /// <summary>
    /// Employee entity - this is where we demonstrate various EF Core features
    /// including relationships, data annotations, and different data types
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Primary key with auto-increment
        /// EF Core convention: properties named 'Id' or '{ClassName}Id' are automatically primary keys
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee's full name - required field with reasonable length limit
        /// This shows how we can enforce business rules at the model level
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email address - must be unique across all employees
        /// We'll configure this constraint in our DbContext
        /// </summary>
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Employee's salary - using decimal for financial accuracy
        /// Column attribute shows how we can customize database column properties
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        /// <summary>
        /// Date when employee was hired
        /// Useful for calculating tenure and other business logic
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Employee's position/job title
        /// Optional field that provides context about their role
        /// </summary>
        [MaxLength(100)]
        public string? Position { get; set; }

        /// <summary>
        /// Foreign key to Department table
        /// This creates the many-to-one relationship (many employees belong to one department)
        /// EF Core recognizes this pattern automatically
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Navigation property to Department
        /// This allows us to access the full Department object from an Employee
        /// Virtual keyword enables lazy loading (loads data when accessed)
        /// </summary>
        public virtual Department Department { get; set; } = null!;

        /// <summary>
        /// Collection of projects this employee is working on
        /// This demonstrates many-to-many relationships
        /// One employee can work on multiple projects
        /// </summary>
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        /// <summary>
        /// Boolean field to track employee status
        /// Shows how EF handles different data types
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
