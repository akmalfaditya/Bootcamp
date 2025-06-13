using RepositoryMVC.Models;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Student Repository Interface - Specialized Repository for Student Entity
    /// 
    /// This interface extends the generic repository to add Student-specific operations.
    /// While the generic repository handles basic CRUD operations, this interface
    /// defines business-specific queries that are unique to the Student entity.
    /// 
    /// This follows the Interface Segregation Principle - clients only depend on
    /// methods they actually use. Controllers working with students will use this
    /// interface instead of the generic one.
    /// 
    /// Benefits of specific repository interfaces:
    /// 1. Type safety - methods return Student objects, not generic entities
    /// 2. Business-focused - method names reflect business operations, not technical ones
    /// 3. Extensibility - can add Student-specific operations without affecting other entities
    /// 4. Clear contracts - developers know exactly what operations are available for Students
    /// </summary>
    public interface IStudentRepository : IGenericRepository<Student>
    {
        /// <summary>
        /// Get all students with their related grade data loaded
        /// This demonstrates eager loading - loading related data in a single query
        /// More efficient than lazy loading when you know you'll need the grades
        /// </summary>
        /// <returns>Students with their grades included</returns>
        Task<IEnumerable<Student>> GetAllStudentsWithGradesAsync();

        /// <summary>
        /// Get a specific student with all their grades loaded
        /// Essential for displaying student details where grades are shown
        /// </summary>
        /// <param name="studentId">ID of the student to retrieve</param>
        /// <returns>Student with grades, or null if not found</returns>
        Task<Student?> GetStudentWithGradesAsync(int studentId);

        /// <summary>
        /// Search students by name - A common business requirement
        /// This encapsulates the search logic and keeps it maintainable
        /// Could be extended to search multiple fields or use full-text search
        /// </summary>
        /// <param name="name">Partial or full name to search for</param>
        /// <returns>Students whose names contain the search term</returns>
        Task<IEnumerable<Student>> FindStudentsByNameAsync(string name);

        /// <summary>
        /// Get students by branch - Useful for academic reporting
        /// Example: Get all Computer Science students
        /// </summary>
        /// <param name="branch">Academic branch/department</param>
        /// <returns>Students in the specified branch</returns>
        Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch);

        /// <summary>
        /// Get students by section - Useful for class management
        /// Example: Get all students in Section A
        /// </summary>
        /// <param name="section">Class section</param>
        /// <returns>Students in the specified section</returns>
        Task<IEnumerable<Student>> GetStudentsBySectionAsync(string section);

        /// <summary>
        /// Get students enrolled in a specific date range
        /// Useful for cohort analysis or enrollment reports
        /// </summary>
        /// <param name="startDate">Start of enrollment period</param>
        /// <param name="endDate">End of enrollment period</param>
        /// <returns>Students enrolled within the date range</returns>
        Task<IEnumerable<Student>> GetStudentsByEnrollmentDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Search students across multiple fields
        /// This provides a comprehensive search functionality
        /// Could search in name, branch, section, or email
        /// </summary>
        /// <param name="searchTerm">Term to search for across multiple fields</param>
        /// <returns>Students matching the search criteria</returns>
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);

        /// <summary>
        /// Check if a student email already exists
        /// Important for validation - emails should be unique
        /// More specific than the generic AnyAsync method
        /// </summary>
        /// <param name="email">Email address to check</param>
        /// <param name="excludeStudentId">Optional: Student ID to exclude from check (useful for updates)</param>
        /// <returns>True if email exists, false otherwise</returns>
        Task<bool> IsEmailExistsAsync(string email, int? excludeStudentId = null);

        /// <summary>
        /// Get students with no grades recorded
        /// Useful for identifying students who need academic attention
        /// Demonstrates complex queries that join multiple tables
        /// </summary>
        /// <returns>Students who have no grade records</returns>
        Task<IEnumerable<Student>> GetStudentsWithoutGradesAsync();

        /// <summary>
        /// Get top performing students based on average grades
        /// Example of business logic in the repository layer
        /// Could be used for honor roll, scholarships, etc.
        /// </summary>
        /// <param name="count">Number of top students to return</param>
        /// <returns>Top performing students ordered by average grade</returns>
        Task<IEnumerable<Student>> GetTopPerformingStudentsAsync(int count);
    }
}
