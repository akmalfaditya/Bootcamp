using AutoMapperMVC.Data;
using AutoMapperMVC.Models;
using AutoMapperMVC.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperMVC.Services
{
    /// <summary>
    /// StudentService acts as our business layer with AutoMapper integration
    /// This demonstrates how AutoMapper simplifies the service layer by handling
    /// the conversion between entities and DTOs automatically
    /// Notice how much cleaner the code becomes when we don't need manual mapping
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor injection - we now receive both the database context and AutoMapper
        /// This shows the power of dependency injection - we can easily add new dependencies
        /// without changing our controller code
        /// </summary>
        public StudentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all students and return them as DTOs
        /// AutoMapper handles the conversion from Student entities to StudentDTOs
        /// This protects our internal data structure and only exposes what clients need
        /// </summary>
        public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
        {
            var students = await _context.Students
                .Include(s => s.Grades)  // Load grades with each student
                .OrderBy(s => s.Name)    // Sort by name for consistent ordering
                .ToListAsync();

            // AutoMapper converts the entire collection in one call
            // This is much cleaner than manually mapping each student
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        /// <summary>
        /// Get a specific student by ID and return as DTO
        /// AutoMapper handles the conversion, including nested Grade objects
        /// </summary>
        public async Task<StudentDTO?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentID == id);

            // AutoMapper handles null values gracefully
            return student != null ? _mapper.Map<StudentDTO>(student) : null;
        }

        /// <summary>
        /// Create a new student from CreateDTO
        /// AutoMapper converts the DTO to an entity, then we save it
        /// Finally, we return the created student as a DTO
        /// </summary>
        public async Task<StudentDTO> CreateStudentAsync(StudentCreateDTO createDto)
        {
            // AutoMapper converts CreateDTO to Student entity
            var student = _mapper.Map<Student>(createDto);
            
            // Business logic: set enrollment date to today if not provided
            if (student.EnrollmentDate == default(DateTime))
            {
                student.EnrollmentDate = DateTime.Today;
            }
            
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Convert the saved entity (now with ID) back to DTO for return
            return _mapper.Map<StudentDTO>(student);
        }

        /// <summary>
        /// Update an existing student using DTO data
        /// This demonstrates a more complex mapping scenario
        /// </summary>
        public async Task<bool> UpdateStudentAsync(int id, StudentDTO studentDto)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
                return false;

            // AutoMapper can update an existing object with data from another object
            // This preserves relationships and other properties not in the DTO
            _mapper.Map(studentDto, existingStudent);
            
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExistsAsync(id))
                {
                    return false;
                }
                throw;
            }
        }

        /// <summary>
        /// Delete a student - this doesn't need DTOs since we're just removing data
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Check if a student exists - simple operation that doesn't need DTOs
        /// </summary>
        public async Task<bool> StudentExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.StudentID == id);
        }

        /// <summary>
        /// Get grades for a specific student and return as DTOs
        /// Notice how AutoMapper includes student name in the GradeDTO automatically
        /// </summary>
        public async Task<IEnumerable<GradeDTO>> GetStudentGradesAsync(int studentId)
        {
            var grades = await _context.Grades
                .Include(g => g.Student)  // Include student for name mapping
                .Where(g => g.StudentID == studentId)
                .OrderByDescending(g => g.GradeDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<GradeDTO>>(grades);
        }

        /// <summary>
        /// Add a new grade using CreateDTO
        /// AutoMapper handles the conversion and we can add custom logic
        /// </summary>
        public async Task<GradeDTO> AddGradeAsync(GradeCreateDTO createDto)
        {
            var grade = _mapper.Map<Grade>(createDto);
            
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

            // Reload with student information for proper DTO mapping
            var savedGrade = await _context.Grades
                .Include(g => g.Student)
                .FirstAsync(g => g.GradeID == grade.GradeID);

            return _mapper.Map<GradeDTO>(savedGrade);
        }

        /// <summary>
        /// Search students by name and return as DTOs
        /// This shows how AutoMapper works seamlessly with LINQ queries
        /// </summary>
        public async Task<IEnumerable<StudentDTO>> SearchStudentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllStudentsAsync();
            }

            var students = await _context.Students
                .Include(s => s.Grades)
                .Where(s => s.Name.Contains(searchTerm) || 
                           s.Email.Contains(searchTerm) ||
                           s.Branch.Contains(searchTerm) ||
                           s.Section.Contains(searchTerm))
                .OrderBy(s => s.Name)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }
    }

    /// <summary>
    /// Updated interface to work with DTOs instead of entities
    /// This provides a clean separation between our internal data model and external API
    /// Using interfaces makes our code more testable and follows SOLID principles
    /// </summary>
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudentsAsync();
        Task<StudentDTO?> GetStudentByIdAsync(int id);
        Task<StudentDTO> CreateStudentAsync(StudentCreateDTO createDto);
        Task<bool> UpdateStudentAsync(int id, StudentDTO studentDto);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> StudentExistsAsync(int id);
        Task<IEnumerable<GradeDTO>> GetStudentGradesAsync(int studentId);
        Task<GradeDTO> AddGradeAsync(GradeCreateDTO createDto);
        Task<IEnumerable<StudentDTO>> SearchStudentsAsync(string searchTerm);
    }
}
