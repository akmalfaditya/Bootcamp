using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.Models
{
    /// <summary>
    /// Student model represents the core data structure for student information
    /// This is the 'M' in MVC - it handles data and business logic
    /// Think of this as your data blueprint that defines what a student looks like in your system
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Primary key for the student - every student needs a unique identifier
        /// This is what we use to distinguish one student from another in our database
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        /// Student's full name - we're using validation attributes here
        /// Required means this field cannot be empty when creating a student
        /// StringLength limits the name to 100 characters max
        /// </summary>
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender - using validation to ensure only valid values are accepted
        /// RegularExpression ensures only "Male", "Female", or "Other" are valid inputs
        /// </summary>
        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic branch or major the student belongs to
        /// Examples: Computer Science, Engineering, Business Administration
        /// </summary>
        [Required(ErrorMessage = "Branch is required")]
        [StringLength(50, ErrorMessage = "Branch cannot exceed 50 characters")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Section within the branch - helps organize students into smaller groups
        /// Examples: Section A, Section B, etc.
        /// </summary>
        [Required(ErrorMessage = "Section is required")]
        [StringLength(10, ErrorMessage = "Section cannot exceed 10 characters")]
        public string Section { get; set; } = string.Empty;        /// <summary>
        /// Email address for communication - notice the EmailAddress validation
        /// This ensures the input follows proper email format
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Student's phone number - optional contact information
        /// Added to demonstrate Entity Framework migrations workflow
        /// </summary>
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Enrollment date - when the student joined the institution
        /// We're using DataType.Date to ensure proper date handling in views
        /// </summary>
        [Required(ErrorMessage = "Enrollment date is required")]
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
