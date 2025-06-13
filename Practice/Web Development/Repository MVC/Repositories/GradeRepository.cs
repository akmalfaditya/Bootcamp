using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Models;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Grade Repository Implementation - Specialized Data Access for Grades
    /// 
    /// This class extends the GenericRepository to provide Grade-specific data access methods.
    /// It focuses on grade-related queries that are essential for academic management systems.
    /// 
    /// Key Features:
    /// 1. Student-centric grade queries for transcripts and performance tracking
    /// 2. Subject-based analysis for course management and curriculum planning
    /// 3. Statistical operations for academic reporting and analytics
    /// 4. Validation methods to prevent duplicate or invalid grade entries
    /// </summary>
    public class GradeRepository : GenericRepository<Grade>, IGradeRepository
    {
        /// <summary>
        /// Constructor - Inherits basic CRUD operations from GenericRepository
        /// </summary>
        /// <param name="context">Entity Framework database context</param>
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all grades for a specific student with student information loaded
        /// 
        /// This method demonstrates eager loading with Include() to load the related Student entity.
        /// Ordering by GradeDate descending shows the most recent grades first,
        /// which is typically what users want to see.
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            return await _dbSet
                .Include(g => g.Student)  // Load student information with each grade
                .Where(g => g.StudentID == studentId)
                .OrderByDescending(g => g.GradeDate)  // Most recent grades first
                .ToListAsync();
        }

        /// <summary>
        /// Get all grades for a specific subject across all students
        /// 
        /// Useful for analyzing course performance, identifying difficult subjects,
        /// and generating subject-specific reports. Including Student data allows
        /// for comprehensive reporting.
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject)
        {
            return await _dbSet
                .Include(g => g.Student)
                .Where(g => g.Subject == subject)
                .OrderByDescending(g => g.GradeDate)
                .ThenBy(g => g.Student!.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Get grades within a specific date range
        /// 
        /// Essential for academic period reporting such as semester grades,
        /// quarterly assessments, or academic year summaries.
        /// Date range queries are fundamental in educational systems.
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(g => g.Student)
                .Where(g => g.GradeDate >= startDate && g.GradeDate <= endDate)
                .OrderByDescending(g => g.GradeDate)
                .ToListAsync();
        }

        /// <summary>
        /// Get the most recent grades for a student
        /// 
        /// This method uses Take() to limit results, which is efficient for getting
        /// just the latest few grades. Useful for dashboard widgets or quick performance views.
        /// </summary>
        public async Task<IEnumerable<Grade>> GetRecentGradesByStudentAsync(int studentId, int count)
        {
            return await _dbSet
                .Include(g => g.Student)
                .Where(g => g.StudentID == studentId)
                .OrderByDescending(g => g.GradeDate)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Calculate average grade for a student
        /// 
        /// This method demonstrates LINQ aggregation functions.
        /// Returns null if no grades exist, which the calling code can handle appropriately.
        /// Essential for GPA calculations and academic standing determinations.
        /// </summary>
        public async Task<decimal?> GetAverageGradeByStudentAsync(int studentId)
        {
            var grades = await _dbSet
                .Where(g => g.StudentID == studentId)
                .ToListAsync();

            return grades.Any() ? grades.Average(g => g.GradeValue) : null;
        }

        /// <summary>
        /// Calculate average grade for a subject across all students
        /// 
        /// Useful for course difficulty analysis and curriculum planning.
        /// If a subject has consistently low averages, it might indicate
        /// the need for curriculum review or additional support resources.
        /// </summary>
        public async Task<decimal?> GetAverageGradeBySubjectAsync(string subject)
        {
            var grades = await _dbSet
                .Where(g => g.Subject == subject)
                .ToListAsync();

            return grades.Any() ? grades.Average(g => g.GradeValue) : null;
        }

        /// <summary>
        /// Get grade distribution for a subject
        /// 
        /// This method demonstrates complex aggregation using GroupBy.
        /// Returns a dictionary showing how many students received each letter grade.
        /// Essential for academic reporting and understanding grade curves.
        /// 
        /// Example result: {"A": 15, "B": 20, "C": 10, "D": 3, "F": 2}
        /// </summary>
        public async Task<Dictionary<string, int>> GetGradeDistributionBySubjectAsync(string subject)
        {
            var grades = await _dbSet
                .Where(g => g.Subject == subject && !string.IsNullOrEmpty(g.LetterGrade))
                .ToListAsync();

            return grades
                .GroupBy(g => g.LetterGrade!)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        /// <summary>
        /// Find grades within a specific numeric range
        /// 
        /// Useful for identifying students who need help (low grades) or
        /// recognition (high grades). The range can be adjusted based on
        /// institutional grading standards.
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesByRangeAsync(decimal minGrade, decimal maxGrade)
        {
            return await _dbSet
                .Include(g => g.Student)
                .Where(g => g.GradeValue >= minGrade && g.GradeValue <= maxGrade)
                .OrderByDescending(g => g.GradeValue)
                .ThenBy(g => g.Student!.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Get all unique subjects that have grades recorded
        /// 
        /// This method demonstrates the Distinct() operation for getting unique values.
        /// Useful for populating dropdown lists, generating subject-based reports,
        /// and understanding the curriculum scope.
        /// </summary>
        public async Task<IEnumerable<string>> GetAllSubjectsAsync()
        {
            return await _dbSet
                .Select(g => g.Subject)
                .Distinct()
                .OrderBy(subject => subject)
                .ToListAsync();
        }

        /// <summary>
        /// Get grades for a specific student in a specific subject
        /// 
        /// This method combines student and subject filtering for detailed analysis.
        /// Useful for tracking a student's progress in a particular subject over time.
        /// Essential for subject-specific academic counseling.
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesByStudentAndSubjectAsync(int studentId, string subject)
        {
            return await _dbSet
                .Include(g => g.Student)
                .Where(g => g.StudentID == studentId && g.Subject == subject)
                .OrderByDescending(g => g.GradeDate)
                .ToListAsync();
        }

        /// <summary>
        /// Check if a grade already exists for a student, subject, and date combination
        /// 
        /// This validation method prevents duplicate grade entries for the same assessment.
        /// Important for maintaining data integrity in academic systems where students
        /// shouldn't have multiple grades for the same subject on the same date.
        /// </summary>
        public async Task<bool> IsGradeExistsAsync(int studentId, string subject, DateTime gradeDate)
        {
            return await _dbSet
                .AnyAsync(g => g.StudentID == studentId && 
                              g.Subject == subject && 
                              g.GradeDate.Date == gradeDate.Date);
        }
    }
}
