using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluentValidationMVC.Models
{
    /// <summary>
    /// Grade model represents individual academic achievements for students
    /// In this FluentValidation demo, we've REMOVED validation attributes
    /// All validation logic will be handled by GradeValidator
    /// This keeps the model focused purely on data structure
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
        /// NO Required attribute - validation moved to FluentValidation
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        /// Subject name for this grade
        /// Examples: Mathematics, Physics, Computer Science, etc.        /// NO validation attributes - handled by GradeValidator
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// The actual grade value - we're using decimal for precision
        /// NO Range or Required attributes - validation moved to FluentValidation
        /// Column attribute kept because it's for database schema, not validation
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal GradeValue { get; set; }

        /// <summary>
        /// Letter grade representation (A, B, C, D, F)
        /// This could be calculated based on GradeValue, but we're storing it for flexibility
        /// NO StringLength attribute - validation moved to FluentValidation
        /// </summary>
        public string? LetterGrade { get; set; }

        /// <summary>
        /// When this grade was recorded
        /// DataType.Date kept for UI purposes, validation moved to FluentValidation
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Grade Date")]
        public DateTime GradeDate { get; set; }

        /// <summary>
        /// Optional comments about the grade
        /// Teachers might want to add notes about the assessment
        /// NO StringLength attribute - validation moved to FluentValidation
        /// </summary>
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
