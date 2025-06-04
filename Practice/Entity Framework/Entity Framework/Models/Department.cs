
using System.ComponentModel.DataAnnotations;

namespace Entity_Framework.Models
{
    /// <summary>
    /// Department entity represents the departments table in our database.
    /// This is a perfect example of how EF Core maps C# classes to database tables.
    /// Each property here becomes a column in the database.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Primary key - EF Core automatically recognizes 'Id' as the primary key
        /// This will be auto-incremented in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Department name - we're using Data Annotations to enforce business rules
        /// Required attribute ensures this field cannot be null
        /// MaxLength prevents overly long department names
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Department description - optional field to provide more context
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Budget allocated to this department
        /// Using decimal for precise financial calculations
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Navigation property - this establishes the one-to-many relationship
        /// One department can have many employees
        /// EF Core uses this to understand table relationships
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        /// <summary>
        /// Manager of the department - this is a self-referencing relationship
        /// A department can have one manager who is also an employee
        /// This demonstrates how EF handles more complex relationships
        /// </summary>
        public int? ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }
    }
}
