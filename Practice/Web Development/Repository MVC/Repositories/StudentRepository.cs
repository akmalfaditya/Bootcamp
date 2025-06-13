using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Models;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Student Repository Implementation - Specialized Data Access for Students
    /// 
    /// This class extends the GenericRepository to provide Student-specific data access methods.
    /// It inherits all the basic CRUD operations from GenericRepository and adds business-specific
    /// queries that are unique to the Student entity.
    /// 
    /// Key Benefits of This Approach:
    /// 1. Code Reuse - Inherits common operations from GenericRepository
    /// 2. Specialization - Adds Student-specific business queries
    /// 3. Performance - Uses Entity Framework's advanced features like Include() for efficient loading
    /// 4. Maintainability - All Student data access logic is centralized in one place
    /// 5. Testability - Easy to mock for unit testing
    /// </summary>
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        /// <summary>
        /// Constructor - Passes the DbContext to the base GenericRepository
        /// This gives us access to all the generic CRUD operations plus our specialized methods
        /// </summary>
        /// <param name="context">Entity Framework database context</param>
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all students with their grades - Demonstrates Eager Loading
        /// 
        /// Eager loading loads related data (grades) in a single database query using JOIN.
        /// This is more efficient than lazy loading when you know you'll need the related data.
        /// The Include() method tells EF to load the Grades collection for each student.
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllStudentsWithGradesAsync()
        {
            return await _dbSet
                .Include(s => s.Grades)  // Load grades with each student
                .OrderBy(s => s.Name)    // Sort students by name for consistent ordering
                .ToListAsync();
        }

        /// <summary>
        /// Get a specific student with their grades loaded
        /// Essential for student detail pages where grades need to be displayed
        /// </summary>
        public async Task<Student?> GetStudentWithGradesAsync(int studentId)
        {
            return await _dbSet
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);
        }

        /// <summary>
        /// Find students by name - Case-insensitive partial matching
        /// 
        /// This method demonstrates how to implement flexible search functionality.
        /// Contains() method generates a SQL LIKE query for partial matching.
        /// The string.IsNullOrWhiteSpace check prevents unnecessary database calls.
        /// </summary>
        public async Task<IEnumerable<Student>> FindStudentsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<Student>();
            }

            return await _dbSet
                .Where(s => s.Name.ToLower().Contains(name.ToLower()))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Get students by academic branch
        /// Useful for department-specific reports and class management
        /// </summary>
        public async Task<IEnumerable<Student>> GetStudentsByBranchAsync(string branch)
        {
            return await _dbSet
                .Where(s => s.Branch == branch)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Get students by section
        /// Essential for classroom management and section-specific operations
        /// </summary>
        public async Task<IEnumerable<Student>> GetStudentsBySectionAsync(string section)
        {
            return await _dbSet
                .Where(s => s.Section == section)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Get students by enrollment date range
        /// 
        /// This method demonstrates date range queries, which are common in business applications.
        /// Useful for cohort analysis, enrollment reports, and academic planning.
        /// The DateTime comparison generates efficient SQL with date range conditions.
        /// </summary>
        public async Task<IEnumerable<Student>> GetStudentsByEnrollmentDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(s => s.EnrollmentDate >= startDate && s.EnrollmentDate <= endDate)
                .OrderBy(s => s.EnrollmentDate)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }        /// <summary>
        /// Comprehensive search across multiple student fields
        /// 
        /// This method provides a user-friendly search experience by checking multiple fields.
        /// Users can search by name, branch, section, or email with a single search term.
        /// This is more intuitive than requiring users to know which field they're searching.
        /// </summary>
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If no search term, return all students with grades included
                return await GetAllStudentsWithGradesAsync();
            }

            var lowerSearchTerm = searchTerm.ToLower();
            
            return await _dbSet
                .Include(s => s.Grades)  // Include grades for complete student information
                .Where(s => s.Name.ToLower().Contains(lowerSearchTerm) ||
                           s.Branch.ToLower().Contains(lowerSearchTerm) ||
                           s.Section.ToLower().Contains(lowerSearchTerm) ||
                           s.Email.ToLower().Contains(lowerSearchTerm))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Check if an email address is already registered
        /// 
        /// This method is crucial for data validation and maintaining email uniqueness.
        /// The excludeStudentId parameter allows checking during updates - we don't want
        /// to flag a student's own email as a duplicate when they're updating their profile.
        /// </summary>
        public async Task<bool> IsEmailExistsAsync(string email, int? excludeStudentId = null)
        {
            var query = _dbSet.Where(s => s.Email.ToLower() == email.ToLower());
            
            if (excludeStudentId.HasValue)
            {
                query = query.Where(s => s.StudentID != excludeStudentId.Value);
            }
            
            return await query.AnyAsync();
        }

        /// <summary>
        /// Find students who have no grades recorded
        /// 
        /// This demonstrates a LEFT JOIN query to find students without related records.
        /// Useful for identifying students who need academic attention or haven't been assessed yet.
        /// The !s.Grades.Any() generates a NOT EXISTS query in SQL.
        /// </summary>
        public async Task<IEnumerable<Student>> GetStudentsWithoutGradesAsync()
        {
            return await _dbSet
                .Include(s => s.Grades)
                .Where(s => !s.Grades.Any())
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Get top performing students based on average grades
        /// 
        /// This method demonstrates complex LINQ queries with aggregation.
        /// It calculates the average grade for each student and returns the top performers.
        /// This kind of query is essential for academic reporting, honor rolls, and scholarships.
        /// </summary>
        public async Task<IEnumerable<Student>> GetTopPerformingStudentsAsync(int count)
        {
            return await _dbSet
                .Include(s => s.Grades)
                .Where(s => s.Grades.Any())  // Only students with grades
                .OrderByDescending(s => s.Grades.Average(g => g.GradeValue))
                .Take(count)
                .ToListAsync();
        }
    }
}
