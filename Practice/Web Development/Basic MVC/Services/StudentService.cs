using StudentManagementMVC.Data;
using StudentManagementMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementMVC.Services
{
    /// <summary>
    /// StudentService acts as our business layer
    /// This is where we implement business logic and data access operations
    /// Following the separation of concerns principle - controllers shouldn't directly access the database
    /// Instead, they use this service layer which handles the complexity
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor injection - we receive the database context through dependency injection
        /// This is a key pattern in modern application development
        /// It makes our code testable and loosely coupled
        /// </summary>
        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all students from the database
        /// Include() method loads related data (grades) - this is called "eager loading"
        /// Without Include(), the Grades collection would be empty
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.Grades)  // Load grades with each student
                .OrderBy(s => s.Name)    // Sort by name for consistent ordering
                .ToListAsync();
        }

        /// <summary>
        /// Get a specific student by ID
        /// This method demonstrates error handling - we return null if student doesn't exist
        /// The controller will handle this appropriately (show 404 error, etc.)
        /// </summary>
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentID == id);
        }

        /// <summary>
        /// Create a new student record
        /// This method encapsulates the logic for adding a student to the database
        /// We set the enrollment date to today if not provided
        /// </summary>
        public async Task<Student> CreateStudentAsync(Student student)
        {
            // Business logic: set enrollment date if not provided
            if (student.EnrollmentDate == default(DateTime))
            {
                student.EnrollmentDate = DateTime.Today;
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        /// <summary>
        /// Update an existing student record
        /// This method handles the complexity of updating entity state
        /// Entity Framework tracks changes and updates only modified fields
        /// </summary>
        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                _context.Entry(student).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts - multiple users editing same record
                if (!await StudentExistsAsync(student.StudentID))
                {
                    return false; // Student was deleted by another user
                }
                throw; // Re-throw if it's a different concurrency issue
            }
        }

        /// <summary>
        /// Delete a student and all their grades
        /// The cascade delete is configured in our DbContext, so grades are automatically deleted
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Check if a student exists - useful for validation
        /// This is a helper method used by other methods in this service
        /// </summary>
        public async Task<bool> StudentExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.StudentID == id);
        }

        /// <summary>
        /// Get all grades for a specific student
        /// This method demonstrates querying related data
        /// </summary>
        public async Task<IEnumerable<Grade>> GetStudentGradesAsync(int studentId)
        {
            return await _context.Grades
                .Where(g => g.StudentID == studentId)
                .OrderByDescending(g => g.GradeDate)  // Most recent grades first
                .ToListAsync();
        }

        /// <summary>
        /// Add a grade for a student
        /// This method includes business logic for calculating letter grades
        /// </summary>
        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            // Business logic: calculate letter grade if not provided
            if (string.IsNullOrEmpty(grade.LetterGrade))
            {
                grade.LetterGrade = grade.CalculateLetterGrade();
            }

            // Set grade date to today if not provided
            if (grade.GradeDate == default(DateTime))
            {
                grade.GradeDate = DateTime.Today;
            }

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        /// <summary>
        /// Search students by name or branch
        /// This demonstrates how to implement search functionality
        /// Using LINQ to build dynamic queries
        /// </summary>
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllStudentsAsync();
            }

            return await _context.Students
                .Include(s => s.Grades)
                .Where(s => s.Name.Contains(searchTerm) || 
                           s.Branch.Contains(searchTerm) ||
                           s.Section.Contains(searchTerm))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }
    }

    /// <summary>
    /// Interface for StudentService
    /// This defines the contract for our business layer
    /// Using interfaces makes our code more testable and follows SOLID principles
    /// We can easily mock this interface for unit testing
    /// </summary>
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> StudentExistsAsync(int id);
        Task<IEnumerable<Grade>> GetStudentGradesAsync(int studentId);
        Task<Grade> AddGradeAsync(Grade grade);
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);
    }
}
