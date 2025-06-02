using System;
using System.Xml.Serialization;
using System.Text.Json.Serialization;

namespace Serialization.Models
{
    /// <summary>
    /// Student model class that demonstrates different serialization approaches.
    /// This class is designed to work with Binary, XML, and JSON serialization.
    /// </summary>
    [Serializable] // Required for binary serialization - tells the runtime this class can be serialized
    public class Student
    {
        // Primary key for our student records
        public int Id { get; set; }
        
        // Student's full name
        public string Name { get; set; } = string.Empty;
        
        // Student's age - demonstrating numeric serialization
        public int Age { get; set; }
        
        // Student's email address
        public string Email { get; set; } = string.Empty;
        
        // Enrollment date - demonstrating DateTime serialization
        public DateTime EnrollmentDate { get; set; }
        
        // Student's current GPA - demonstrating decimal serialization
        public decimal GPA { get; set; }
        
        // List of courses - demonstrating collection serialization
        public List<string> Courses { get; set; } = new List<string>();
        
        // Default constructor - required for XML serialization
        // XML serializer needs a parameterless constructor to work properly
        public Student()
        {
        }
        
        // Convenience constructor for creating student objects quickly
        public Student(int id, string name, int age, string email, decimal gpa)
        {
            Id = id;
            Name = name;
            Age = age;
            Email = email;
            GPA = gpa;
            EnrollmentDate = DateTime.Now;
            Courses = new List<string>();
        }
        
        // Override ToString for easy display of student information
        public override string ToString()
        {
            return $"Student ID: {Id}\n" +
                   $"Name: {Name}\n" +
                   $"Age: {Age}\n" +
                   $"Email: {Email}\n" +
                   $"Enrollment Date: {EnrollmentDate:yyyy-MM-dd}\n" +
                   $"GPA: {GPA:F2}\n" +
                   $"Courses: {string.Join(", ", Courses)}\n";
        }
    }
}
