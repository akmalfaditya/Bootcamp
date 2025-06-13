using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryMVC.Models
{
    /// <summary>
    /// Grade model represents individual academic achievements for students
    /// This demonstrates how models can have relationships with other models
    /// Each grade belongs to a specific student and subject
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Primary key for the grade record
        /// Every grade entry needs its own unique identifier
        /// </summary>
        public int GradeID { get; set; }

        /// <summary>
        /// Foreign key linking this grade to a specific student
        /// This creates the relationship between Grade and Student models
        /// Foreign keys are how we connect related data in relational databases
        /// </summary>
        [Required(ErrorMessage = "Student ID is required")]
        public int StudentID { get; set; }

        /// <summary>
        /// Subject name for this grade
        /// Examples: Mathematics, Physics, Computer Science, etc.
        /// </summary>
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// The actual grade value - we're using decimal for precision
        /// Range validation ensures grades are between 0 and 100
        /// Column attribute specifies the database column type for precision
        /// </summary>
        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal GradeValue { get; set; }

        /// <summary>
        /// Letter grade representation (A, B, C, D, F)
        /// This could be calculated based on GradeValue, but we're storing it for flexibility
        /// </summary>
        [StringLength(2, ErrorMessage = "Letter grade cannot exceed 2 characters")]
        public string? LetterGrade { get; set; }

        /// <summary>
        /// When this grade was recorded
        /// DataType.Date ensures proper date handling in forms and views
        /// </summary>
        [Required(ErrorMessage = "Grade date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Grade Date")]
        public DateTime GradeDate { get; set; }

        /// <summary>
        /// Optional comments about the grade
        /// Teachers might want to add notes about the assessment
        /// </summary>
        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string? Comments { get; set; }

        /// <summary>
        /// Navigation property back to the Student
        /// This allows us to easily access student information from a grade record
        /// Virtual keyword enables lazy loading - EF will load the student data when needed
        /// </summary>
        public virtual Student? Student { get; set; }

        /// <summary>
        /// Helper method to calculate letter grade based on numeric value
        /// This demonstrates business logic within the model
        /// You could also implement this as a computed property
        /// </summary>
        public string CalculateLetterGrade()
        {
            return GradeValue switch
            {
                >= 90 => "A",
                >= 80 => "B", 
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            };
        }
    }
}
