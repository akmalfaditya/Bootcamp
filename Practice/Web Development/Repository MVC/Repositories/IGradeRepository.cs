using RepositoryMVC.Models;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Grade Repository Interface - Specialized Repository for Grade Entity
    /// 
    /// This interface extends the generic repository to add Grade-specific operations.
    /// Grades are tightly related to Students, so many of these methods involve
    /// querying grades in relation to specific students or academic criteria.
    /// 
    /// Grade-specific operations often involve:
    /// 1. Student-centric queries (grades for a specific student)
    /// 2. Subject-based queries (all grades for a specific subject)
    /// 3. Date-based queries (grades within a semester or academic year)
    /// 4. Performance analysis (average grades, grade distributions)
    /// </summary>
    public interface IGradeRepository : IGenericRepository<Grade>
    {
        /// <summary>
        /// Get all grades for a specific student
        /// Essential for student transcripts and performance tracking
        /// </summary>
        /// <param name="studentId">ID of the student</param>
        /// <returns>All grades for the specified student</returns>
        Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId);

        /// <summary>
        /// Get all grades for a specific subject
        /// Useful for subject-based performance analysis and grading reports
        /// </summary>
        /// <param name="subject">Name of the subject</param>
        /// <returns>All grades for the specified subject</returns>
        Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject);

        /// <summary>
        /// Get grades within a specific date range
        /// Essential for academic period reporting (semester, quarter, academic year)
        /// </summary>
        /// <param name="startDate">Start date of the period</param>
        /// <param name="endDate">End date of the period</param>
        /// <returns>Grades recorded within the date range</returns>
        Task<IEnumerable<Grade>> GetGradesByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get recent grades for a student
        /// Useful for displaying latest academic performance
        /// </summary>
        /// <param name="studentId">ID of the student</param>
        /// <param name="count">Number of recent grades to retrieve</param>
        /// <returns>Most recent grades for the student</returns>
        Task<IEnumerable<Grade>> GetRecentGradesByStudentAsync(int studentId, int count);

        /// <summary>
        /// Calculate average grade for a student
        /// Essential for GPA calculations and academic standing
        /// </summary>
        /// <param name="studentId">ID of the student</param>
        /// <returns>Average grade value, or null if no grades exist</returns>
        Task<decimal?> GetAverageGradeByStudentAsync(int studentId);

        /// <summary>
        /// Calculate average grade for a subject across all students
        /// Useful for course difficulty analysis and grade distribution reports
        /// </summary>
        /// <param name="subject">Name of the subject</param>
        /// <returns>Average grade for the subject</returns>
        Task<decimal?> GetAverageGradeBySubjectAsync(string subject);

        /// <summary>
        /// Get grade distribution for a subject
        /// Shows how many students got each letter grade (A, B, C, D, F)
        /// Essential for academic reporting and grade curve analysis
        /// </summary>
        /// <param name="subject">Name of the subject</param>
        /// <returns>Dictionary with letter grades as keys and counts as values</returns>
        Task<Dictionary<string, int>> GetGradeDistributionBySubjectAsync(string subject);

        /// <summary>
        /// Find grades within a specific range
        /// Useful for identifying students who need help or recognition
        /// </summary>
        /// <param name="minGrade">Minimum grade value</param>
        /// <param name="maxGrade">Maximum grade value</param>
        /// <returns>Grades within the specified range</returns>
        Task<IEnumerable<Grade>> GetGradesByRangeAsync(decimal minGrade, decimal maxGrade);

        /// <summary>
        /// Get all subjects that have grades recorded
        /// Useful for populating dropdown lists and academic reports
        /// </summary>
        /// <returns>List of unique subject names</returns>
        Task<IEnumerable<string>> GetAllSubjectsAsync();

        /// <summary>
        /// Get grades for a student in a specific subject
        /// Useful for subject-specific student performance tracking
        /// </summary>
        /// <param name="studentId">ID of the student</param>
        /// <param name="subject">Name of the subject</param>
        /// <returns>Grades for the student in the specified subject</returns>
        Task<IEnumerable<Grade>> GetGradesByStudentAndSubjectAsync(int studentId, string subject);

        /// <summary>
        /// Check if a grade exists for a student in a subject on a specific date
        /// Prevents duplicate grade entries for the same assessment
        /// </summary>
        /// <param name="studentId">ID of the student</param>
        /// <param name="subject">Name of the subject</param>
        /// <param name="gradeDate">Date of the grade</param>
        /// <returns>True if grade exists, false otherwise</returns>
        Task<bool> IsGradeExistsAsync(int studentId, string subject, DateTime gradeDate);
    }
}
