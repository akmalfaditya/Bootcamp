
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Data
{
    /// <summary>
    /// This is our DbContext - the main class that coordinates Entity Framework functionality
    /// Think of it as the bridge between your C# objects and the database
    /// It handles connection management, change tracking, and query translation
    /// </summary>
    public class CompanyDbContext : DbContext
    {
        /// <summary>
        /// DbSet properties represent tables in our database
        /// Each DbSet<T> corresponds to a table where T is the entity type
        /// These properties allow us to query and save instances of our entities
        /// </summary>
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// Constructor that accepts DbContextOptions
        /// This allows for dependency injection and configuration flexibility
        /// </summary>
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Parameterless constructor for scenarios where we configure the context manually
        /// This is useful for development and testing scenarios
        /// </summary>
        public CompanyDbContext()
        {
        }

        /// <summary>
        /// OnConfiguring method - this is where we set up the database connection
        /// In a real application, you'd typically configure this in your DI container
        /// but for learning purposes, we're doing it here
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if not already configured (allows for external configuration)
            if (!optionsBuilder.IsConfigured)
            {
                // Using SQLite for this demo - it's file-based and doesn't require installation
                // The database file will be created in the same directory as our executable
                optionsBuilder.UseSqlite("Data Source=CompanyDatabase.db");
                  // Enable sensitive data logging for development (don't do this in production!)
                optionsBuilder.EnableSensitiveDataLogging();
                
                // Log detailed information for learning purposes - commented out for cleaner console output
                // optionsBuilder.LogTo(Console.WriteLine);
            }
        }

        /// <summary>
        /// OnModelCreating is where we configure our model using Fluent API
        /// This gives us more control than Data Annotations alone
        /// Think of it as fine-tuning how EF maps our classes to database tables
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                // Ensure email addresses are unique across all employees
                entity.HasIndex(e => e.Email).IsUnique();
                
                // Configure the relationship between Employee and Department
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion of departments with employees
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                // Department names should be unique
                entity.HasIndex(d => d.Name).IsUnique();
                
                // Configure the manager relationship (self-referencing)
                entity.HasOne(d => d.Manager)
                      .WithMany()
                      .HasForeignKey(d => d.ManagerId)
                      .OnDelete(DeleteBehavior.SetNull); // If manager is deleted, set ManagerId to null
            });

            // Configure many-to-many relationship between Employee and Project
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Projects)
                .WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeProject", // Join table name
                    j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                    j => j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                    j =>
                    {                        j.HasKey("EmployeeId", "ProjectId"); // Composite primary key
                        j.ToTable("EmployeeProjects"); // Table name for the join table
                    });

            // Note: Seed data will be handled separately with migrations
            // SeedData(modelBuilder); // Commented out for migrations approach
        }

        /// <summary>
        /// This method seeds our database with some initial data
        /// In real applications, you might use migrations or separate seeding logic
        /// But for learning, this shows how EF can populate data automatically
        /// </summary>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Engineering", Description = "Software development and technical infrastructure", Budget = 500000m },
                new Department { Id = 2, Name = "Sales", Description = "Revenue generation and client acquisition", Budget = 300000m },
                new Department { Id = 3, Name = "Marketing", Description = "Brand promotion and market analysis", Budget = 200000m },
                new Department { Id = 4, Name = "Human Resources", Description = "Employee management and company culture", Budget = 150000m }
            );

            // Seed Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@company.com", Salary = 75000m, HireDate = new DateTime(2022, 1, 15), Position = "Senior Developer", DepartmentId = 1 },
                new Employee { Id = 2, Name = "Jane Smith", Email = "jane.smith@company.com", Salary = 68000m, HireDate = new DateTime(2022, 3, 10), Position = "Frontend Developer", DepartmentId = 1 },
                new Employee { Id = 3, Name = "Mike Johnson", Email = "mike.johnson@company.com", Salary = 82000m, HireDate = new DateTime(2021, 8, 20), Position = "Sales Manager", DepartmentId = 2 },
                new Employee { Id = 4, Name = "Sarah Wilson", Email = "sarah.wilson@company.com", Salary = 71000m, HireDate = new DateTime(2022, 5, 8), Position = "Marketing Specialist", DepartmentId = 3 },
                new Employee { Id = 5, Name = "David Brown", Email = "david.brown@company.com", Salary = 79000m, HireDate = new DateTime(2021, 11, 3), Position = "HR Manager", DepartmentId = 4 }
            );

            // Seed Projects
            modelBuilder.Entity<Project>().HasData(
                new Project { Id = 1, Name = "Website Redesign", Description = "Complete overhaul of company website", StartDate = new DateTime(2024, 1, 1), Budget = 50000m, Status = "Active" },
                new Project { Id = 2, Name = "Mobile App Development", Description = "Native mobile application for iOS and Android", StartDate = new DateTime(2024, 2, 15), Budget = 80000m, Status = "Active" },
                new Project { Id = 3, Name = "Sales Automation", Description = "CRM integration and sales process automation", StartDate = new DateTime(2023, 10, 1), EndDate = new DateTime(2024, 3, 31), Budget = 35000m, Status = "Completed" }
            );
        }
    }
}
