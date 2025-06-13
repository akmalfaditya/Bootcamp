using Microsoft.AspNetCore.Mvc;
using RepositoryMVC.Models;
using RepositoryMVC.Services;

namespace RepositoryMVC.Controllers
{
    /// <summary>
    /// StudentController is the 'C' in MVC - the Controller
    /// This is where we handle HTTP requests and coordinate between the Model and View
    /// Controllers are responsible for:
    /// 1. Receiving user input (HTTP requests)
    /// 2. Processing that input (using services/business logic)
    /// 3. Selecting and returning the appropriate view with data
    /// 
    /// Notice how we inherit from Controller base class - this gives us access to
    /// MVC functionality like View(), RedirectToAction(), etc.
    /// </summary>
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        /// <summary>
        /// Constructor injection - we receive our service through dependency injection
        /// This is a key principle in modern web development
        /// It makes our controller testable and loosely coupled from data access logic
        /// </summary>
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Index action - this handles GET requests to /Student or /Student/Index
        /// This demonstrates the basic controller action pattern:
        /// 1. Get data from the service layer
        /// 2. Pass that data to a view
        /// 3. Return the view to be rendered
        /// </summary>
        public async Task<IActionResult> Index(string searchTerm)
        {
            // Get student data - notice we're using the service layer, not direct database access
            // This follows separation of concerns - controllers don't know about databases
            var students = await _studentService.SearchStudentsAsync(searchTerm ?? string.Empty);
            
            // Pass search term to view so it can be displayed in the search box
            ViewBag.SearchTerm = searchTerm;
            
            // Return the view with our student data
            // The view will receive this as its model
            return View(students);
        }

        /// <summary>
        /// Details action - shows information for a specific student
        /// This demonstrates how controllers handle parameters from the URL
        /// URL like /Student/Details/5 will pass 5 as the id parameter
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            // Validate input - if no ID provided, return bad request
            if (id == null)
            {
                return BadRequest("Student ID is required");
            }

            // Get the student data
            var student = await _studentService.GetStudentByIdAsync(id.Value);
            
            // If student doesn't exist, return 404 Not Found
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found");
            }

            // Return the details view with the student data
            return View(student);
        }

        /// <summary>
        /// Create action (GET) - displays the form for creating a new student
        /// This action just returns an empty form
        /// HTTP GET is used for displaying forms, HTTP POST for submitting them
        /// </summary>
        public IActionResult Create()
        {
            // Return view with a new, empty student object
            // This provides the form with default values
            return View(new Student());
        }

        /// <summary>
        /// Create action (POST) - processes the form submission for creating a student
        /// HttpPost attribute means this action only responds to POST requests
        /// ValidateAntiForgeryToken protects against CSRF attacks
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Gender,Branch,Section,Email,EnrollmentDate")] Student student)
        {
            // ModelState.IsValid checks all validation attributes on the Student model
            // If any validation fails, we redisplay the form with error messages
            if (ModelState.IsValid)
            {
                try
                {
                    // Create the student using our service layer
                    await _studentService.CreateStudentAsync(student);
                    
                    // Redirect to the Index action to show all students
                    // This follows the POST-Redirect-GET pattern to prevent duplicate submissions
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // If something goes wrong, add an error message and redisplay the form
                    ModelState.AddModelError("", $"Error creating student: {ex.Message}");
                }
            }

            // If we got here, something went wrong - redisplay the form
            return View(student);
        }

        /// <summary>
        /// Edit action (GET) - displays the form for editing an existing student
        /// Similar to Details, but returns an editable form instead of read-only display
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
        /// This demonstrates how to handle updates in MVC
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,Name,Gender,Branch,Section,Email,EnrollmentDate")] Student student)
        {
            // Security check - make sure the ID in the URL matches the student being edited
            if (id != student.StudentID)
            {
                return BadRequest("ID mismatch");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _studentService.UpdateStudentAsync(student);
                    if (!success)
                    {
                        return NotFound($"Student with ID {id} not found");
                    }
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating student: {ex.Message}");
                }
            }
            return View(student);
        }

        /// <summary>
        /// Delete action (GET) - displays confirmation page before deleting
        /// We show the student details and ask for confirmation
        /// This prevents accidental deletions
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
        /// We use a different action name to avoid conflicts with the GET Delete action
        /// ActionName attribute maps this to "Delete" in the URL routing
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
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // If deletion fails, redirect back with an error message
                TempData["ErrorMessage"] = $"Error deleting student: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Grades action - shows all grades for a specific student
        /// This demonstrates how to create specialized views for related data
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

            // Pass student info to the view so we can display student name, etc.
            ViewBag.StudentName = student.Name;
            ViewBag.StudentID = student.StudentID;

            var grades = await _studentService.GetStudentGradesAsync(id.Value);
            return View(grades);
        }

        /// <summary>
        /// AddGrade action (GET) - displays form for adding a new grade to a student
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

            // Pre-populate the grade with student information
            var grade = new Grade
            {
                StudentID = studentId.Value,
                GradeDate = DateTime.Today
            };

            ViewBag.StudentName = student.Name;
            return View(grade);
        }

        /// <summary>
        /// AddGrade action (POST) - processes the form submission for adding a grade
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGrade([Bind("StudentID,Subject,GradeValue,GradeDate,Comments")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.AddGradeAsync(grade);
                    return RedirectToAction(nameof(Grades), new { id = grade.StudentID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error adding grade: {ex.Message}");
                }
            }

            // If we got here, something went wrong - reload student name and redisplay form
            var student = await _studentService.GetStudentByIdAsync(grade.StudentID);
            ViewBag.StudentName = student?.Name ?? "Unknown Student";
            
            return View(grade);
        }
    }
}
