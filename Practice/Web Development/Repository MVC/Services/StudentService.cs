using RepositoryMVC.Models;
using RepositoryMVC.Repositories;

namespace RepositoryMVC.Services
{    /// <summary>
    /// StudentService with Repository Pattern Implementation
    /// 
    /// This service now demonstrates the Repository Design Pattern in action.
    /// Instead of directly accessing the Entity Framework DbContext, it uses
    /// the Unit of Work pattern to coordinate operations across multiple repositories.
    /// 
    /// Key Benefits of Repository Pattern Implementation:
    /// 1. Separation of Concerns - Service logic is separated from data access logic
    /// 2. Testability - Can easily mock repositories for unit testing
    /// 3. Flexibility - Can change data access implementation without affecting business logic
    /// 4. Consistency - All data access follows the same pattern
    /// 5. Transaction Management - Unit of Work handles complex multi-entity operations
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor injection with Unit of Work
        /// 
        /// Instead of receiving a DbContext directly, we now receive a Unit of Work.
        /// This gives us access to all repositories while maintaining transaction consistency.
        /// The Unit of Work coordinates operations between Student and Grade repositories.
        /// </summary>
        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }        /// <summary>
        /// Get all students using Repository Pattern
        /// 
        /// Notice how this method now uses the repository instead of direct DbContext access.
        /// The repository handles the Include() operation for loading grades, and we don't
        /// need to worry about the underlying Entity Framework details here.
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _unitOfWork.Students.GetAllStudentsWithGradesAsync();
        }        /// <summary>
        /// Get student by ID using Repository Pattern
        /// 
        /// The repository provides a specialized method that includes grades,
        /// making our service method simpler and more focused on business logic.
        /// </summary>
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _unitOfWork.Students.GetStudentWithGradesAsync(id);
        }        /// <summary>
        /// Create a new student using Repository Pattern
        /// 
        /// This method demonstrates the Unit of Work pattern in action.
        /// We use the repository to add the student and the Unit of Work to save changes.
        /// This ensures that the operation is atomic and can be rolled back if needed.
        /// </summary>
        public async Task<Student> CreateStudentAsync(Student student)
        {
            // Business logic: set enrollment date if not provided
            if (student.EnrollmentDate == default(DateTime))
            {
                student.EnrollmentDate = DateTime.Today;
            }

            // Business validation: check if email already exists
            if (await _unitOfWork.Students.IsEmailExistsAsync(student.Email))
            {
                throw new InvalidOperationException($"A student with email {student.Email} already exists.");
            }

            // Use repository to add the student
            await _unitOfWork.Students.AddAsync(student);
            
            // Use Unit of Work to save changes - this commits the transaction
            await _unitOfWork.SaveChangesAsync();
            
            return student;
        }        /// <summary>
        /// Update student using Repository Pattern
        /// 
        /// This method shows how the Repository Pattern simplifies update operations.
        /// The repository handles the Entity Framework specifics, while we focus on
        /// business logic like email validation.
        /// </summary>
        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                // Business validation: check if email is taken by another student
                if (await _unitOfWork.Students.IsEmailExistsAsync(student.Email, student.StudentID))
                {
                    throw new InvalidOperationException($"Email {student.Email} is already taken by another student.");
                }

                // Use repository to update the student
                _unitOfWork.Students.Update(student);
                
                // Use Unit of Work to save changes
                await _unitOfWork.SaveChangesAsync();
                
                return true;
            }
            catch (Exception)
            {
                // In a real application, you might want to log the exception
                // and handle different types of exceptions differently
                return false;
            }
        }        /// <summary>
        /// Delete student using Repository Pattern
        /// 
        /// This method demonstrates how the Repository Pattern handles entity deletion.
        /// The repository method abstracts the complexity of finding and removing the entity.
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            // First, get the student to delete
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
            {
                return false;
            }

            // Use repository to remove the student
            // Note: Cascade delete will automatically remove related grades
            _unitOfWork.Students.Remove(student);
            
            // Use Unit of Work to save changes
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }        /// <summary>
        /// Check if student exists using Repository Pattern
        /// 
        /// The repository provides an efficient method for existence checking
        /// without loading the entire entity.
        /// </summary>
        public async Task<bool> StudentExistsAsync(int id)
        {
            return await _unitOfWork.Students.AnyAsync(s => s.StudentID == id);
        }        /// <summary>
        /// Get student grades using Repository Pattern
        /// 
        /// This method now uses the Grade repository instead of querying grades directly.
        /// This demonstrates how different repositories can be used together through
        /// the Unit of Work pattern.
        /// </summary>
        public async Task<IEnumerable<Grade>> GetStudentGradesAsync(int studentId)
        {
            return await _unitOfWork.Grades.GetGradesByStudentIdAsync(studentId);
        }        /// <summary>
        /// Add grade using Repository Pattern
        /// 
        /// This method demonstrates coordinated operations across multiple repositories.
        /// We validate the student exists using the Student repository, then add the grade
        /// using the Grade repository, all coordinated through the Unit of Work.
        /// </summary>
        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            // Business validation: ensure the student exists
            if (!await _unitOfWork.Students.AnyAsync(s => s.StudentID == grade.StudentID))
            {
                throw new InvalidOperationException($"Student with ID {grade.StudentID} does not exist.");
            }

            // Business validation: check for duplicate grades on the same date
            if (await _unitOfWork.Grades.IsGradeExistsAsync(grade.StudentID, grade.Subject, grade.GradeDate))
            {
                throw new InvalidOperationException($"A grade for {grade.Subject} already exists for this student on {grade.GradeDate:yyyy-MM-dd}.");
            }

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

            // Use Grade repository to add the grade
            await _unitOfWork.Grades.AddAsync(grade);
            
            // Use Unit of Work to save changes
            await _unitOfWork.SaveChangesAsync();
            
            return grade;
        }        /// <summary>
        /// Search students using Repository Pattern
        /// 
        /// This method now delegates to the repository's search functionality,
        /// which provides a more comprehensive search across multiple fields.
        /// </summary>
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            return await _unitOfWork.Students.SearchStudentsAsync(searchTerm);
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
