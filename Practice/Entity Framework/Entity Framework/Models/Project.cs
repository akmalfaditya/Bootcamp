
using System.ComponentModel.DataAnnotations;

namespace Entity_Framework.Models
{
    /// <summary>
    /// Project entity demonstrates many-to-many relationships in EF Core
    /// Multiple employees can work on multiple projects
    /// This is a common business scenario that EF handles elegantly
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Primary key for the Project entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Project name - required and reasonably sized
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of what the project entails
        /// </summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// When the project started
        /// Useful for tracking project timeline and duration
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Expected or actual completion date
        /// Nullable because some projects might not have a defined end date yet
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Project budget - important for financial tracking
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Project status - could be Active, Completed, On Hold, etc.
        /// Using an enum would be even better, but keeping it simple for this demo
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Navigation property for the many-to-many relationship
        /// This collection contains all employees assigned to this project
        /// EF Core handles the join table automatically
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
