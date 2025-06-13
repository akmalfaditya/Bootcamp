using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs
{
    /// <summary>
    /// Data Transfer Object for Grade information
    /// Simplified version of Grade entity for client consumption
    /// Notice how we exclude navigation properties and internal IDs where appropriate
    /// </summary>
    public class GradeDTO
    {
        /// <summary>
        /// Grade record identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Subject name for this grade
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Numeric grade value (0-100)
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// Letter grade representation (A, B, C, D, F)
        /// </summary>
        public string? LetterGrade { get; set; }

        /// <summary>
        /// When this grade was recorded
        /// </summary>
        public DateTime GradeDate { get; set; }

        /// <summary>
        /// Optional comments about the grade
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Student's name - included for convenience when displaying grade lists
        /// This shows how DTOs can include related data for better client experience
        /// </summary>
        public string StudentName { get; set; } = string.Empty;
    }
}
