using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating new grades
    /// Contains only the fields necessary for grade creation
    /// StudentID comes from the route/form, so we include it here
    /// </summary>
    public class GradeCreateDTO
    {
        /// <summary>
        /// Which student this grade belongs to
        /// </summary>
        [Required(ErrorMessage = "Student ID is required")]
        public int StudentID { get; set; }

        /// <summary>
        /// Subject for this grade
        /// </summary>
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Numeric grade value (0-100)
        /// </summary>
        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        public decimal GradeValue { get; set; }

        /// <summary>
        /// Optional letter grade - can be calculated automatically
        /// </summary>
        [StringLength(2, ErrorMessage = "Letter grade cannot exceed 2 characters")]
        public string? LetterGrade { get; set; }

        /// <summary>
        /// When this grade was awarded
        /// </summary>
        [Required(ErrorMessage = "Grade date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Grade Date")]
        public DateTime GradeDate { get; set; }

        /// <summary>
        /// Optional teacher comments
        /// </summary>
        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string? Comments { get; set; }
    }
}
