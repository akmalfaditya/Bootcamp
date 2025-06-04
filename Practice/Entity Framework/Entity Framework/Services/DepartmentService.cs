
using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services
{
    /// <summary>
    /// DepartmentService handles all department-related operations
    /// This demonstrates working with hierarchical data and complex relationships
    /// </summary>
    public class DepartmentService
    {
        private readonly CompanyDbContext _context;

        public DepartmentService(CompanyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new department
        /// Includes validation to ensure business rules are followed
        /// </summary>
        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            // Business rule: Department names must be unique
            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == department.Name);

            if (existingDepartment != null)
                throw new InvalidOperationException($"Department '{department.Name}' already exists");

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return department;
        }

        /// <summary>
        /// Get all departments with their employees
        /// Demonstrates eager loading of related data
        /// </summary>
        public async Task<List<Department>> GetAllDepartmentsWithEmployeesAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Manager)  // Include manager information
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Get department by ID with full details
        /// </summary>
        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        /// <summary>
        /// Assign a manager to a department
        /// Demonstrates updating related entities
        /// </summary>
        public async Task<bool> AssignManagerAsync(int departmentId, int employeeId)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            var employee = await _context.Employees.FindAsync(employeeId);

            if (department == null || employee == null)
                return false;

            // Business rule: Manager must be from the same department
            if (employee.DepartmentId != departmentId)
                throw new InvalidOperationException("Manager must be an employee of the same department");

            department.ManagerId = employeeId;
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Get departments with budget above a certain threshold
        /// Demonstrates filtering and ordering
        /// </summary>
        public async Task<List<Department>> GetDepartmentsWithHighBudgetAsync(decimal minBudget)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .Where(d => d.Budget >= minBudget)
                .OrderByDescending(d => d.Budget)
                .ToListAsync();
        }

        /// <summary>
        /// Update department information
        /// </summary>
        public async Task<Department?> UpdateDepartmentAsync(int id, Department updatedDepartment)
        {
            var existingDepartment = await _context.Departments.FindAsync(id);
            
            if (existingDepartment == null)
                return null;

            existingDepartment.Name = updatedDepartment.Name;
            existingDepartment.Description = updatedDepartment.Description;
            existingDepartment.Budget = updatedDepartment.Budget;

            await _context.SaveChangesAsync();
            return existingDepartment;
        }

        /// <summary>
        /// Delete a department
        /// Includes checks to prevent deletion of departments with employees
        /// </summary>
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);
            
            if (department == null)
                return false;

            // Business rule: Cannot delete department with active employees
            if (department.Employees.Any(e => e.IsActive))
                throw new InvalidOperationException("Cannot delete department with active employees");

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
