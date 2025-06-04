using Microsoft.EntityFrameworkCore;
using StudentManagementMVC.Models;

namespace StudentManagementMVC.Data
{
    /// <summary>
    /// ApplicationDbContext is our database context class
    /// This is where we configure our database connection and define our data sets
    /// Think of this as the bridge between your C# objects and the SQLite database
    /// 
    /// DbContext is part of Entity Framework - it's the main class that coordinates 
    /// Entity Framework functionality for your data model
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor that accepts DbContextOptions
        /// This allows dependency injection to configure our database connection
        /// The options parameter contains configuration like connection string, database provider, etc.
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet represents a table in our database
        /// Students DbSet corresponds to the Students table
        /// Each Student object represents a row in that table
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Grades DbSet corresponds to the Grades table
        /// This will hold all grade records for all students
        /// </summary>
        public DbSet<Grade> Grades { get; set; }

        /// <summary>
        /// OnModelCreating is where we configure our database schema
        /// This method is called when EF is creating the model for our database
        /// Here we can define relationships, constraints, and other database-specific configurations
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Student and Grade
            // This tells EF that one student can have many grades
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)           // Each grade has one student
                .WithMany(s => s.Grades)          // Each student has many grades
                .HasForeignKey(g => g.StudentID)  // Foreign key is StudentID
                .OnDelete(DeleteBehavior.Cascade); // If student is deleted, delete their grades too

            // Configure decimal precision for GradeValue
            // This ensures proper storage of decimal values in SQLite
            modelBuilder.Entity<Grade>()
                .Property(g => g.GradeValue)
                .HasColumnType("decimal(5,2)");

            // Add some sample data for testing
            // This is called "seeding" - we're providing initial data for our application
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentID = 1,
                    Name = "John Smith",
                    Gender = "Male",
                    Branch = "Computer Science",
                    Section = "A",
                    Email = "john.smith@university.edu",
                    EnrollmentDate = new DateTime(2023, 9, 1)
                },
                new Student
                {
                    StudentID = 2,
                    Name = "Sarah Johnson",
                    Gender = "Female", 
                    Branch = "Engineering",
                    Section = "B",
                    Email = "sarah.johnson@university.edu",
                    EnrollmentDate = new DateTime(2023, 9, 1)
                },
                new Student
                {
                    StudentID = 3,
                    Name = "Mike Davis",
                    Gender = "Male",
                    Branch = "Business Administration",
                    Section = "A",
                    Email = "mike.davis@university.edu",
                    EnrollmentDate = new DateTime(2024, 1, 15)
                }
            );

            // Seed some sample grades
            modelBuilder.Entity<Grade>().HasData(
                new Grade
                {
                    GradeID = 1,
                    StudentID = 1,
                    Subject = "Data Structures",
                    GradeValue = 95.5m,
                    LetterGrade = "A",
                    GradeDate = new DateTime(2024, 3, 15),
                    Comments = "Excellent understanding of algorithms"
                },
                new Grade
                {
                    GradeID = 2,
                    StudentID = 1,
                    Subject = "Web Development",
                    GradeValue = 88.0m,
                    LetterGrade = "B",
                    GradeDate = new DateTime(2024, 4, 20),
                    Comments = "Good work on MVC project"
                },
                new Grade
                {
                    GradeID = 3,
                    StudentID = 2,
                    Subject = "Calculus",
                    GradeValue = 92.0m,
                    LetterGrade = "A",
                    GradeDate = new DateTime(2024, 3, 10),
                    Comments = "Strong mathematical foundation"
                }
            );
        }
    }
}
