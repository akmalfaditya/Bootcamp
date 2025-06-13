using System.ComponentModel.DataAnnotations;

namespace AutoMapperMVC.DTOs
{
    /// <summary>
    /// Data Transfer Object for Student information
    /// This represents what we want to expose to the client - notice how we exclude sensitive data
    /// and rename some properties to be more client-friendly
    /// DTOs help us control exactly what data leaves our application
    /// </summary>
    public class StudentDTO
    {
        /// <summary>
        /// Student identifier - we keep this simple for client consumption
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Student's display name - renamed from 'Name' for better client understanding
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Student's gender information
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Academic department - renamed from 'Branch' for clarity
        /// </summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Class section information
        /// </summary>
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Contact email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number - optional field
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// When the student enrolled - formatted for display
        /// </summary>
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// List of grades for this student - we'll map this from the Grade entities
        /// </summary>
        public List<GradeDTO> Grades { get; set; } = new List<GradeDTO>();
    }
}
