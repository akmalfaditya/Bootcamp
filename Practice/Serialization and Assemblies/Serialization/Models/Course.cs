using System;
using System.Xml.Serialization;

namespace Serialization.Models
{
    /// <summary>
    /// Course model to demonstrate complex object serialization.
    /// Shows how related objects can be serialized together.
    /// </summary>
    [Serializable]
    public class Course
    {
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Instructor { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        // Default constructor for XML serialization
        public Course()
        {
        }
        
        public Course(string courseCode, string courseName, int credits, string instructor)
        {
            CourseCode = courseCode;
            CourseName = courseName;
            Credits = credits;
            Instructor = instructor;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(4); // Typical semester length
        }
        
        public override string ToString()
        {
            return $"Course: {CourseCode} - {CourseName}\n" +
                   $"Credits: {Credits}\n" +
                   $"Instructor: {Instructor}\n" +
                   $"Duration: {StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}";
        }
    }
}
