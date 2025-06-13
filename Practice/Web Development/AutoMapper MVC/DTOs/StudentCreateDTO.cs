using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating new students
    /// This contains only the fields needed for student creation
    /// Notice how we don't include the ID (auto-generated) or Grades (added separately)
    /// </summary>
    public class StudentCreateDTO
    {
        /// <summary>
        /// Student's full name - required for creation
        /// </summary>
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender - must be valid option
        /// </summary>
        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic branch/department
        /// </summary>
        [Required(ErrorMessage = "Branch is required")]
        [StringLength(50, ErrorMessage = "Branch cannot exceed 50 characters")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Class section
        /// </summary>
        [Required(ErrorMessage = "Section is required")]
        [StringLength(10, ErrorMessage = "Section cannot exceed 10 characters")]
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Student's email address
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional phone number
        /// </summary>
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Enrollment date - when the student joined
        /// </summary>
        [Required(ErrorMessage = "Enrollment date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
    }
}
