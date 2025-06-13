using Microsoft.AspNetCore.Mvc;
using AutoMapperMVC.Models;
using AutoMapperMVC.Services;
using AutoMapperMVC.DTOs;

namespace AutoMapperMVC.Controllers
{
    /// <summary>
    /// StudentController with AutoMapper integration
    /// This demonstrates how AutoMapper simplifies controller code by working with DTOs
    /// Notice how we no longer expose our internal entity structure to the views
    /// Instead, we use DTOs that are specifically designed for data transfer
    /// </summary>
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        /// <summary>
        /// Constructor injection - we receive our service through dependency injection
        /// The service layer now handles all AutoMapper operations, keeping the controller clean
        /// </summary>
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Index action - displays list of students using DTOs
        /// The service returns StudentDTOs which contain only the data we want to expose
        /// This provides better security and cleaner separation of concerns
        /// </summary>
        public async Task<IActionResult> Index(string searchTerm)
        {
            // Service returns DTOs instead of entities - much cleaner!
            var students = await _studentService.SearchStudentsAsync(searchTerm ?? string.Empty);
            
            ViewBag.SearchTerm = searchTerm;
            return View(students);
        }

        /// <summary>
        /// Details action - shows information for a specific student
        /// Working with DTOs means we only expose the data that's appropriate for display
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("Student ID is required");
            }

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found");
            }

            return View(student);
        }

        /// <summary>
        /// Create action (GET) - displays the form for creating a new student
        /// We use StudentCreateDTO which contains only the fields needed for creation
        /// This prevents over-posting attacks and keeps the form focused
        /// </summary>
        public IActionResult Create()
        {
            // Use CreateDTO instead of full entity - much more secure!
            return View(new StudentCreateDTO 
            { 
                EnrollmentDate = DateTime.Today // Set default enrollment date
            });
        }

        /// <summary>
        /// Create action (POST) - processes the form submission using DTOs
        /// Notice how clean this is - we receive a CreateDTO and service returns a DTO
        /// AutoMapper handles all the conversion logic behind the scenes
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateDTO createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Service handles the AutoMapper conversion internally
                    var studentDto = await _studentService.CreateStudentAsync(createDto);
                    
                    TempData["SuccessMessage"] = $"Student {studentDto.FullName} created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating student: {ex.Message}");
                }
            }

            return View(createDto);
        }

        /// <summary>
        /// Edit action (GET) - displays the form for editing an existing student
        /// We return a DTO that contains the current student data
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Student ID is required");
            }

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found");
            }

            return View(student);
        }

        /// <summary>
        /// Edit action (POST) - processes the form submission for updating a student
        /// We work with DTOs throughout - much cleaner than working with entities
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentDTO studentDto)
        {
            if (id != studentDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _studentService.UpdateStudentAsync(id, studentDto);
                    if (!success)
                    {
                        return NotFound($"Student with ID {id} not found");
                    }
                    
                    TempData["SuccessMessage"] = $"Student {studentDto.FullName} updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating student: {ex.Message}");
                }
            }
            return View(studentDto);
        }

        /// <summary>
        /// Delete action (GET) - displays confirmation page before deleting
        /// Shows student information using DTOs for confirmation
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Student ID is required");
            }

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found");
            }

            return View(student);
        }

        /// <summary>
        /// DeleteConfirmed action (POST) - actually performs the deletion
        /// Simple deletion doesn't need DTOs since we're just removing data
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _studentService.DeleteStudentAsync(id);
                if (!success)
                {
                    return NotFound($"Student with ID {id} not found");
                }
                
                TempData["SuccessMessage"] = "Student deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting student: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Grades action - shows all grades for a specific student using DTOs
        /// GradeDTOs include student name automatically thanks to AutoMapper
        /// </summary>
        public async Task<IActionResult> Grades(int? id)
        {
            if (id == null)
            {
                return BadRequest("Student ID is required");
            }

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found");
            }

            ViewBag.StudentName = student.FullName;
            ViewBag.StudentID = student.Id;

            var grades = await _studentService.GetStudentGradesAsync(id.Value);
            return View(grades);
        }

        /// <summary>
        /// AddGrade action (GET) - displays form for adding a new grade to a student
        /// Using GradeCreateDTO keeps the form focused on the necessary fields
        /// </summary>
        public async Task<IActionResult> AddGrade(int? studentId)
        {
            if (studentId == null)
            {
                return BadRequest("Student ID is required");
            }

            var student = await _studentService.GetStudentByIdAsync(studentId.Value);
            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found");
            }

            // Use CreateDTO with default values
            var gradeCreateDto = new GradeCreateDTO
            {
                StudentID = studentId.Value,
                GradeDate = DateTime.Today
            };

            ViewBag.StudentName = student.FullName;
            return View(gradeCreateDto);
        }

        /// <summary>
        /// AddGrade action (POST) - processes the form submission for adding a grade
        /// Clean DTO-based approach with AutoMapper handling the conversion
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGrade(GradeCreateDTO gradeCreateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var gradeDto = await _studentService.AddGradeAsync(gradeCreateDto);
                    TempData["SuccessMessage"] = $"Grade for {gradeDto.Subject} added successfully!";
                    return RedirectToAction(nameof(Grades), new { id = gradeCreateDto.StudentID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error adding grade: {ex.Message}");
                }
            }

            // If we got here, something went wrong - reload student name and redisplay form
            var student = await _studentService.GetStudentByIdAsync(gradeCreateDto.StudentID);
            ViewBag.StudentName = student?.FullName ?? "Unknown Student";
            
            return View(gradeCreateDto);
        }
    }
}
