using System.ComponentModel.DataAnnotations;

namespace FluentValidationMVC.Models
{
    /// <summary>
    /// Student model represents the core data structure for student information
    /// This is the 'M' in MVC - it handles data and business logic
    /// In this FluentValidation demo, we've REMOVED most Data Annotations
    /// Instead, validation rules will be defined using FluentValidation classes
    /// This separates validation logic from the model itself - cleaner architecture!
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Primary key for the student - every student needs a unique identifier
        /// This is what we use to distinguish one student from another in our database
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        /// Student's full name - NO validation attributes here!
        /// All validation will be handled by StudentValidator using FluentValidation
        /// This keeps the model clean and focused on data structure only
        /// </summary>
        public string Name { get; set; } = string.Empty;        /// <summary>
        /// Student's gender - NO RegularExpression attribute here!
        /// Gender validation will be handled in StudentValidator
        /// Acceptable values: "Male", "Female", "Other"
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic branch or major the student belongs to
        /// Examples: Computer Science, Engineering, Business Administration
        /// NO validation attributes - handled by FluentValidation
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Section within the branch - helps organize students into smaller groups
        /// Examples: Section A, Section B, etc.
        /// NO validation attributes - handled by FluentValidation
        /// </summary>
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Email address for communication
        /// NO EmailAddress attribute - email validation handled by FluentValidation
        /// This demonstrates the separation of concerns approach
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Student's phone number - optional contact information
        /// NO Phone attribute - phone validation handled by FluentValidation
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Enrollment date - when the student joined the institution
        /// We keep DataType and Display attributes as they're for UI purposes, not validation
        /// Date validation will be handled by FluentValidation
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Navigation property for grades - this creates a relationship between Student and Grade
        /// One student can have many grades (One-to-Many relationship)
        /// This is how Entity Framework handles relationships between tables
        /// </summary>
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
