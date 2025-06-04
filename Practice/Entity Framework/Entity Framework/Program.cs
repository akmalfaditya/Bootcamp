/*
 * Entity Framework Core Demo Application
 * 
 * This application demonstrates all the key concepts covered in our EF Core training:
 * - DbContext and Entity configuration
 * - CRUD operations (Create, Read, Update, Delete)
 * - LINQ queries with EF Core
 * - Relationships (One-to-Many, Many-to-Many)
 * - Migrations and database creation
 * 
 * Follow along with the code to see how each concept works in practice!
 */

using Entity_Framework.Data;
using Entity_Framework.Models;
using Entity_Framework.Services;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Entity Framework Core Demo Application ===\n");

            // Initialize our database context
            // In a real application, this would be configured through dependency injection
            using var context = new CompanyDbContext();            // Ensure database is created - this handles the initial setup
            await ApplyMigrationsAsync(context);
            
            // Seed the database with initial data if needed
            await SeedDatabaseAsync(context);

            // Initialize our services
            var employeeService = new EmployeeService(context);
            var departmentService = new DepartmentService(context);

            try
            {
                // Demo 1: Basic CRUD Operations
                await DemonstrateCrudOperationsAsync(employeeService);

                // Demo 2: Complex Queries and Relationships
                await DemonstrateComplexQueriesAsync(employeeService);

                // Demo 3: Working with Departments
                await DemonstrateDepartmentOperationsAsync(departmentService);

                // Demo 4: Advanced LINQ Queries
                await DemonstrateAdvancedQueriesAsync(context);

                // Demo 5: Many-to-Many Relationships
                await DemonstrateManyToManyRelationshipsAsync(context);

                Console.WriteLine("\n=== Demo completed successfully! ===");
                Console.WriteLine("Check the CompanyDatabase.db file that was created in your project folder.");
                Console.WriteLine("You can open it with SQLite tools to see the actual database structure.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }        /// <summary>
        /// Applies pending migrations to the database
        /// This is the proper way to handle database schema changes in production
        /// </summary>
        static async Task ApplyMigrationsAsync(CompanyDbContext context)
        {
            Console.WriteLine("Applying database migrations...");
            
            // Apply any pending migrations
            await context.Database.MigrateAsync();
            
            Console.WriteLine("Database migrations applied successfully!\n");
        }

        /// <summary>
        /// Demonstrates basic CRUD (Create, Read, Update, Delete) operations
        /// This is the foundation of most database applications
        /// </summary>
        static async Task DemonstrateCrudOperationsAsync(EmployeeService employeeService)
        {
            Console.WriteLine("=== CRUD Operations Demo ===");

            // CREATE: Adding a new employee
            Console.WriteLine("\n1. Creating a new employee...");
            var newEmployee = new Employee
            {
                Name = "Alice Cooper",
                Email = "alice.cooper@company.com",
                Salary = 72000m,
                HireDate = DateTime.Now,
                Position = "Backend Developer",
                DepartmentId = 1, // Engineering department
                IsActive = true
            };

            try
            {
                var createdEmployee = await employeeService.CreateEmployeeAsync(newEmployee);
                Console.WriteLine($"✓ Created employee: {createdEmployee.Name} (ID: {createdEmployee.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error creating employee: {ex.Message}");
            }

            // READ: Retrieving employee data
            Console.WriteLine("\n2. Reading employee data...");
            var allEmployees = await employeeService.GetAllEmployeesAsync();
            Console.WriteLine($"✓ Found {allEmployees.Count} employees:");
            
            foreach (var emp in allEmployees)
            {
                Console.WriteLine($"   - {emp.Name} ({emp.Position}) in {emp.Department.Name}");
            }

            // READ: Get specific employee by ID
            if (allEmployees.Any())
            {
                var firstEmployee = allEmployees.First();
                var employeeById = await employeeService.GetEmployeeByIdAsync(firstEmployee.Id);
                if (employeeById != null)
                {
                    Console.WriteLine($"\n✓ Retrieved employee by ID: {employeeById.Name}");
                    Console.WriteLine($"   Email: {employeeById.Email}");
                    Console.WriteLine($"   Salary: ${employeeById.Salary:N2}");
                    Console.WriteLine($"   Department: {employeeById.Department.Name}");
                }
            }

            // UPDATE: Modifying employee data
            Console.WriteLine("\n3. Updating employee data...");
            if (allEmployees.Any())
            {
                var employeeToUpdate = allEmployees.Last();
                var originalSalary = employeeToUpdate.Salary;
                
                employeeToUpdate.Salary = originalSalary + 5000; // Give them a raise!
                employeeToUpdate.Position = "Senior " + employeeToUpdate.Position;

                var updatedEmployee = await employeeService.UpdateEmployeeAsync(employeeToUpdate.Id, employeeToUpdate);
                if (updatedEmployee != null)
                {
                    Console.WriteLine($"✓ Updated {updatedEmployee.Name}:");
                    Console.WriteLine($"   Salary: ${originalSalary:N2} → ${updatedEmployee.Salary:N2}");
                    Console.WriteLine($"   Position: {updatedEmployee.Position}");
                }
            }

            // DELETE: Soft delete (deactivate) an employee
            Console.WriteLine("\n4. Deactivating an employee (soft delete)...");
            if (allEmployees.Count > 5) // Only if we have enough employees
            {
                var employeeToDeactivate = allEmployees.Last();
                var deactivated = await employeeService.DeactivateEmployeeAsync(employeeToDeactivate.Id);
                if (deactivated)
                {
                    Console.WriteLine($"✓ Deactivated employee: {employeeToDeactivate.Name}");
                }
            }

            Console.WriteLine("\n--- CRUD Operations Demo Complete ---\n");
        }

        /// <summary>
        /// Demonstrates complex queries and working with relationships
        /// Shows off LINQ's power when combined with EF Core
        /// </summary>
        static async Task DemonstrateComplexQueriesAsync(EmployeeService employeeService)
        {
            Console.WriteLine("=== Complex Queries Demo ===");

            // Filter employees by department
            Console.WriteLine("\n1. Employees in Engineering department:");
            var engineeringEmployees = await employeeService.GetEmployeesByDepartmentAsync("Engineering");
            foreach (var emp in engineeringEmployees)
            {
                Console.WriteLine($"   - {emp.Name} (${emp.Salary:N2})");
            }

            // Get high earners
            Console.WriteLine("\n2. High earners (salary >= $70,000):");
            var highEarners = await employeeService.GetHighEarnersAsync(70000m);
            foreach (var emp in highEarners)
            {
                Console.WriteLine($"   - {emp.Name}: ${emp.Salary:N2} in {emp.Department.Name}");
            }

            // Statistical analysis
            Console.WriteLine("\n3. Department statistics:");
            var stats = await employeeService.GetDepartmentStatisticsAsync();
            foreach (var stat in stats)
            {
                Console.WriteLine($"   Department: {stat.Key}");
                // Note: In a real app, you'd properly cast these objects
                Console.WriteLine($"   {stat.Value}");
            }

            Console.WriteLine("\n--- Complex Queries Demo Complete ---\n");
        }

        /// <summary>
        /// Demonstrates working with departments and hierarchical relationships
        /// </summary>
        static async Task DemonstrateDepartmentOperationsAsync(DepartmentService departmentService)
        {
            Console.WriteLine("=== Department Operations Demo ===");

            // Get all departments with their employees
            Console.WriteLine("\n1. All departments and their employees:");
            var departments = await departmentService.GetAllDepartmentsWithEmployeesAsync();
            
            foreach (var dept in departments)
            {
                Console.WriteLine($"\n   Department: {dept.Name} (Budget: ${dept.Budget:N2})");
                Console.WriteLine($"   Description: {dept.Description}");
                Console.WriteLine($"   Employees ({dept.Employees.Count(e => e.IsActive)}):");
                
                foreach (var emp in dept.Employees.Where(e => e.IsActive))
                {
                    Console.WriteLine($"     - {emp.Name} ({emp.Position})");
                }

                if (dept.Manager != null)
                {
                    Console.WriteLine($"   Manager: {dept.Manager.Name}");
                }
            }

            // Create a new department
            Console.WriteLine("\n2. Creating a new department...");
            try
            {
                var newDepartment = new Department
                {
                    Name = "Research & Development",
                    Description = "Innovation and future technology development",
                    Budget = 400000m
                };

                var createdDept = await departmentService.CreateDepartmentAsync(newDepartment);
                Console.WriteLine($"✓ Created department: {createdDept.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error creating department: {ex.Message}");
            }

            Console.WriteLine("\n--- Department Operations Demo Complete ---\n");
        }

        /// <summary>
        /// Demonstrates advanced LINQ queries and aggregations
        /// This shows the power of combining C# LINQ with SQL through EF Core
        /// </summary>
        static async Task DemonstrateAdvancedQueriesAsync(CompanyDbContext context)
        {
            Console.WriteLine("=== Advanced LINQ Queries Demo ===");

            // Complex aggregation query
            Console.WriteLine("\n1. Department summary with aggregated data:");
            var departmentSummary = await context.Departments
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    TotalEmployees = d.Employees.Count(e => e.IsActive),
                    AverageSalary = d.Employees.Where(e => e.IsActive).Average(e => (double?)e.Salary) ?? 0,
                    BudgetUtilization = d.Employees.Where(e => e.IsActive).Sum(e => (double?)e.Salary) ?? 0,
                    DepartmentBudget = (double)d.Budget
                })
                .ToListAsync();

            foreach (var summary in departmentSummary)
            {
                Console.WriteLine($"   {summary.DepartmentName}:");
                Console.WriteLine($"     Employees: {summary.TotalEmployees}");
                Console.WriteLine($"     Avg Salary: ${summary.AverageSalary:N2}");
                Console.WriteLine($"     Budget Utilization: ${summary.BudgetUtilization:N2} / ${summary.DepartmentBudget:N2}");
                
                if (summary.DepartmentBudget > 0)
                {
                    var utilizationPercent = (summary.BudgetUtilization / summary.DepartmentBudget) * 100;
                    Console.WriteLine($"     Utilization Rate: {utilizationPercent:F1}%");
                }
            }

            // Conditional queries
            Console.WriteLine("\n2. Employees hired in the last 2 years:");
            var recentHires = await context.Employees
                .Include(e => e.Department)
                .Where(e => e.HireDate >= DateTime.Now.AddYears(-2) && e.IsActive)
                .OrderBy(e => e.HireDate)
                .ToListAsync();

            foreach (var emp in recentHires)
            {
                var tenure = DateTime.Now - emp.HireDate;
                Console.WriteLine($"   - {emp.Name} hired {tenure.Days} days ago in {emp.Department.Name}");
            }

            // Grouping and counting
            Console.WriteLine("\n3. Employee count by position:");
            var positionCounts = await context.Employees
                .Where(e => e.IsActive && e.Position != null)
                .GroupBy(e => e.Position)
                .Select(g => new { Position = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            foreach (var pos in positionCounts)
            {
                Console.WriteLine($"   - {pos.Position}: {pos.Count} employees");
            }

            Console.WriteLine("\n--- Advanced LINQ Queries Demo Complete ---\n");
        }

        /// <summary>
        /// Demonstrates many-to-many relationships between employees and projects
        /// This is a common business scenario that EF Core handles elegantly
        /// </summary>
        static async Task DemonstrateManyToManyRelationshipsAsync(CompanyDbContext context)
        {
            Console.WriteLine("=== Many-to-Many Relationships Demo ===");

            // Get all projects with their assigned employees
            Console.WriteLine("\n1. Projects and assigned employees:");
            var projects = await context.Projects
                .Include(p => p.Employees)
                .ThenInclude(e => e.Department)
                .ToListAsync();

            foreach (var project in projects)
            {
                Console.WriteLine($"\n   Project: {project.Name}");
                Console.WriteLine($"   Status: {project.Status} | Budget: ${project.Budget:N2}");
                Console.WriteLine($"   Duration: {project.StartDate:yyyy-MM-dd} to {(project.EndDate?.ToString("yyyy-MM-dd") ?? "Ongoing")}");
                
                if (project.Employees.Any())
                {
                    Console.WriteLine($"   Assigned Employees ({project.Employees.Count}):");
                    foreach (var emp in project.Employees)
                    {
                        Console.WriteLine($"     - {emp.Name} ({emp.Department.Name})");
                    }
                }
                else
                {
                    Console.WriteLine("     No employees assigned yet");
                }
            }

            // Assign employees to projects (creating many-to-many relationships)
            Console.WriteLine("\n2. Assigning employees to projects...");
            
            var activeEmployees = await context.Employees
                .Where(e => e.IsActive)
                .Take(3)
                .ToListAsync();
                
            var activeProject = await context.Projects
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Status == "Active");

            if (activeProject != null && activeEmployees.Any())
            {
                foreach (var employee in activeEmployees)
                {
                    if (!activeProject.Employees.Contains(employee))
                    {
                        activeProject.Employees.Add(employee);
                        Console.WriteLine($"   ✓ Assigned {employee.Name} to {activeProject.Name}");
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine("   Project assignments saved!");
            }

            // Show employees and their projects
            Console.WriteLine("\n3. Employees and their project assignments:");
            var employeesWithProjects = await context.Employees
                .Include(e => e.Projects)
                .Include(e => e.Department)
                .Where(e => e.IsActive)
                .ToListAsync();

            foreach (var emp in employeesWithProjects)
            {
                Console.WriteLine($"\n   {emp.Name} ({emp.Department.Name}):");
                if (emp.Projects.Any())
                {
                    foreach (var proj in emp.Projects)
                    {
                        Console.WriteLine($"     - Working on: {proj.Name} ({proj.Status})");
                    }
                }
                else
                {
                    Console.WriteLine($"     - No project assignments");
                }
            }

            Console.WriteLine("\n--- Many-to-Many Relationships Demo Complete ---\n");
        }

        /// <summary>
        /// Seeds the database with initial data if it's empty
        /// This is done separately from migrations for better control
        /// </summary>
        static async Task SeedDatabaseAsync(CompanyDbContext context)
        {
            // Check if database already has data
            if (await context.Departments.AnyAsync())
            {
                Console.WriteLine("Database already contains data, skipping seeding.\n");
                return;
            }

            Console.WriteLine("Seeding database with initial data...");

            // Seed Departments
            var departments = new[]
            {
                new Department { Name = "Engineering", Description = "Software development and technical infrastructure", Budget = 500000m },
                new Department { Name = "Sales", Description = "Revenue generation and client acquisition", Budget = 300000m },
                new Department { Name = "Marketing", Description = "Brand promotion and market analysis", Budget = 200000m },
                new Department { Name = "Human Resources", Description = "Employee management and company culture", Budget = 150000m }
            };
            
            context.Departments.AddRange(departments);
            await context.SaveChangesAsync();

            // Seed Employees
            var employees = new[]
            {
                new Employee { Name = "John Doe", Email = "john.doe@company.com", Salary = 75000m, HireDate = new DateTime(2022, 1, 15), Position = "Senior Developer", DepartmentId = 1 },
                new Employee { Name = "Jane Smith", Email = "jane.smith@company.com", Salary = 68000m, HireDate = new DateTime(2022, 3, 10), Position = "Frontend Developer", DepartmentId = 1 },
                new Employee { Name = "Mike Johnson", Email = "mike.johnson@company.com", Salary = 82000m, HireDate = new DateTime(2021, 8, 20), Position = "Sales Manager", DepartmentId = 2 },
                new Employee { Name = "Sarah Wilson", Email = "sarah.wilson@company.com", Salary = 71000m, HireDate = new DateTime(2022, 5, 8), Position = "Marketing Specialist", DepartmentId = 3 },
                new Employee { Name = "David Brown", Email = "david.brown@company.com", Salary = 79000m, HireDate = new DateTime(2021, 11, 3), Position = "HR Manager", DepartmentId = 4 }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            // Seed Projects
            var projects = new[]
            {
                new Project { Name = "Website Redesign", Description = "Complete overhaul of company website", StartDate = new DateTime(2024, 1, 1), Budget = 50000m, Status = "Active" },
                new Project { Name = "Mobile App Development", Description = "Native mobile application for iOS and Android", StartDate = new DateTime(2024, 2, 15), Budget = 80000m, Status = "Active" },
                new Project { Name = "Sales Automation", Description = "CRM integration and sales process automation", StartDate = new DateTime(2023, 10, 1), EndDate = new DateTime(2024, 3, 31), Budget = 35000m, Status = "Completed" }
            };

            context.Projects.AddRange(projects);
            await context.SaveChangesAsync();

            Console.WriteLine("Database seeding completed successfully!\n");
        }
    }
}
