
using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services
{
    /// <summary>
    /// EmployeeService demonstrates all CRUD operations using Entity Framework
    /// This is a common pattern - separating data access logic into service classes
    /// makes your code more maintainable and testable
    /// </summary>
    public class EmployeeService
    {
        private readonly CompanyDbContext _context;

        /// <summary>
        /// Constructor injection - we receive the DbContext through dependency injection
        /// This promotes loose coupling and makes testing easier
        /// </summary>
        public EmployeeService(CompanyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// CREATE operation - Adding a new employee to the database
        /// This demonstrates the basic pattern: Create object, Add to context, SaveChanges
        /// </summary>
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            // Validate the employee data before adding
            if (string.IsNullOrWhiteSpace(employee.Name))
                throw new ArgumentException("Employee name is required");

            if (string.IsNullOrWhiteSpace(employee.Email))
                throw new ArgumentException("Employee email is required");

            // Check if email already exists (business rule enforcement)
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == employee.Email);

            if (existingEmployee != null)
                throw new InvalidOperationException($"Employee with email {employee.Email} already exists");

            // Add the new employee to the context
            // At this point, it's only in memory - not yet in the database
            _context.Employees.Add(employee);

            // SaveChanges actually writes to the database
            // This is when EF generates and executes the SQL INSERT statement
            await _context.SaveChangesAsync();

            // The employee object now has its Id populated by the database
            return employee;
        }

        /// <summary>
        /// READ operations - Various ways to retrieve employee data
        /// This showcases different LINQ query patterns with EF Core
        /// </summary>
        /// 
        // Get all employees with their department information
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            // Include() is used for eager loading - loads related data in one query
            // Without Include(), Department would be null unless explicitly loaded
            return await _context.Employees
                .Include(e => e.Department)  // Joins with Department table
                .OrderBy(e => e.Name)        // Sort by name
                .ToListAsync();              // Execute query and return results
        }

        // Get employee by ID
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            // FirstOrDefaultAsync returns null if no employee found
            // This is safer than First() which throws an exception
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Projects)  // Also load the projects this employee works on
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Get employees by department - demonstrates filtering
        public async Task<List<Employee>> GetEmployeesByDepartmentAsync(string departmentName)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Department.Name == departmentName)  // WHERE clause in SQL
                .OrderBy(e => e.HireDate)
                .ToListAsync();
        }        // Get employees with salary above a certain amount - more complex filtering
        // Note: SQLite doesn't support ordering by decimal directly, so we work around this limitation
        public async Task<List<Employee>> GetHighEarnersAsync(decimal minSalary)
        {
            // First get the data without ordering (SQLite limitation workaround)
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Salary >= minSalary && e.IsActive)
                .ToListAsync();
                
            // Then sort in memory (client-side) to work around SQLite decimal ordering limitation
            return employees
                .OrderByDescending(e => e.Salary)
                .ToList();
        }

        /// <summary>
        /// UPDATE operation - Modifying existing employee data
        /// Shows how EF tracks changes and generates appropriate SQL
        /// </summary>
        public async Task<Employee?> UpdateEmployeeAsync(int id, Employee updatedEmployee)
        {
            // First, find the existing employee
            var existingEmployee = await _context.Employees.FindAsync(id);
            
            if (existingEmployee == null)
                return null;

            // Update the properties
            // EF Core's change tracker notices these modifications
            existingEmployee.Name = updatedEmployee.Name;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.Salary = updatedEmployee.Salary;
            existingEmployee.Position = updatedEmployee.Position;
            existingEmployee.DepartmentId = updatedEmployee.DepartmentId;
            existingEmployee.IsActive = updatedEmployee.IsActive;

            // Save changes - EF generates UPDATE SQL for only the changed fields
            await _context.SaveChangesAsync();

            return existingEmployee;
        }

        /// <summary>
        /// Alternative update method using Update() - updates all properties
        /// This is useful when you want to replace the entire entity
        /// </summary>
        public async Task<bool> UpdateEmployeeCompleteAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.Id);
            
            if (existingEmployee == null)
                return false;

            // Update() marks all properties as modified
            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
            
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// DELETE operation - Removing an employee from the database
        /// Demonstrates both soft and hard delete patterns
        /// </summary>
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            
            if (employee == null)
                return false;

            // Hard delete - completely removes the record
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// Soft delete - marks employee as inactive instead of deleting
        /// This is often preferred in business applications to maintain data history
        /// </summary>
        public async Task<bool> DeactivateEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            
            if (employee == null)
                return false;

            // Just mark as inactive - data remains in database
            employee.IsActive = false;
            await _context.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// Advanced query - demonstrates more complex LINQ operations
        /// Gets employees who joined in a specific year and are working on active projects
        /// </summary>
        public async Task<List<Employee>> GetEmployeesHiredInYearWithActiveProjectsAsync(int year)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Projects)
                .Where(e => e.HireDate.Year == year && 
                           e.IsActive && 
                           e.Projects.Any(p => p.Status == "Active"))
                .OrderBy(e => e.HireDate)
                .ToListAsync();
        }        /// <summary>
        /// Statistical query - demonstrates aggregation functions
        /// Gets department-wise employee statistics
        /// Note: SQLite has limitations with decimal aggregation, so we do client-side calculations
        /// </summary>
        public async Task<Dictionary<string, object>> GetDepartmentStatisticsAsync()
        {
            // First get all active employees with their departments
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.IsActive)
                .ToListAsync();

            // Then perform grouping and aggregation on client side to work around SQLite limitations
            var stats = employees
                .GroupBy(e => e.Department.Name)
                .ToDictionary(g => g.Key, g => (object)new
                {
                    DepartmentName = g.Key,
                    EmployeeCount = g.Count(),
                    AverageSalary = g.Average(e => e.Salary),
                    TotalSalaryBudget = g.Sum(e => e.Salary),
                    HighestSalary = g.Max(e => e.Salary),
                    LowestSalary = g.Min(e => e.Salary)
                });

            return stats;
        }
    }
}
