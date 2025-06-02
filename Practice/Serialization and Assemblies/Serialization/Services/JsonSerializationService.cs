using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serialization.Models;

namespace Serialization.Services
{
    /// <summary>
    /// JSON serialization service - converts objects to JSON format.
    /// JSON is lightweight, human-readable, and widely used in web applications.
    /// Perfect for REST APIs and configuration files.
    /// </summary>
    public class JsonSerializationService
    {
        private readonly string _dataDirectory;
        private readonly JsonSerializerOptions _jsonOptions;
        
        public JsonSerializationService()
        {
            // Create a dedicated directory for JSON files
            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "JSON");
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
            
            // Configure JSON serialization options for better readability
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true, // Makes JSON human-readable with proper indentation
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Uses camelCase for property names
                Converters = { new JsonStringEnumConverter() } // Converts enums to strings instead of numbers
            };
        }
        
        /// <summary>
        /// Serialize a student object to JSON format.
        /// JSON serialization is the most common format for modern web applications.
        /// </summary>
        public async Task SerializeStudentAsync(Student student, string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                // Convert the student object to JSON string
                // JsonSerializer.Serialize() is the modern .NET approach for JSON serialization
                string jsonString = JsonSerializer.Serialize(student, _jsonOptions);
                
                // Write the JSON string to a file asynchronously
                // Using async methods is good practice for file I/O operations
                await File.WriteAllTextAsync(filePath, jsonString);
                
                Console.WriteLine($"✓ Successfully serialized student to JSON file: {fileName}");
                Console.WriteLine($"  File location: {filePath}");
                Console.WriteLine($"  File size: {new FileInfo(filePath).Length} bytes");
                
                // Show JSON preview
                ShowJsonPreview(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during JSON serialization: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Synchronous version of JSON serialization for simpler usage.
        /// Sometimes you don't need async operations, especially in console applications.
        /// </summary>
        public void SerializeStudent(Student student, string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                // Serialize to JSON string
                string jsonString = JsonSerializer.Serialize(student, _jsonOptions);
                
                // Write to file synchronously
                File.WriteAllText(filePath, jsonString);
                
                Console.WriteLine($"✓ Successfully serialized student to JSON file: {fileName}");
                ShowJsonPreview(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during JSON serialization: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Deserialize a student object from a JSON file.
        /// JSON deserialization converts the JSON text back into a .NET object.
        /// </summary>
        public async Task<Student?> DeserializeStudentAsync(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"✗ JSON file not found: {fileName}");
                    return null;
                }
                
                // Read the JSON file content asynchronously
                string jsonString = await File.ReadAllTextAsync(filePath);
                
                // Deserialize the JSON string back to a Student object
                Student? student = JsonSerializer.Deserialize<Student>(jsonString, _jsonOptions);
                
                Console.WriteLine($"✓ Successfully deserialized student from JSON file: {fileName}\n");
                return student;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during JSON deserialization: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Synchronous version of JSON deserialization.
        /// </summary>
        public Student? DeserializeStudent(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"✗ JSON file not found: {fileName}");
                    return null;
                }
                
                string jsonString = File.ReadAllText(filePath);
                Student? student = JsonSerializer.Deserialize<Student>(jsonString, _jsonOptions);
                
                Console.WriteLine($"✓ Successfully deserialized student from JSON file: {fileName}\n");
                return student;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during JSON deserialization: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Serialize a course object to demonstrate JSON with different object types.
        /// </summary>
        public void SerializeCourse(Course course, string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                string jsonString = JsonSerializer.Serialize(course, _jsonOptions);
                File.WriteAllText(filePath, jsonString);
                
                Console.WriteLine($"✓ Successfully serialized course to JSON file: {fileName}");
                ShowJsonPreview(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during course JSON serialization: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Show a preview of the JSON content to help understand the format.
        /// </summary>
        private void ShowJsonPreview(string jsonString)
        {
            Console.WriteLine("  JSON Content Preview:");
            
            // Show first few lines of the JSON
            string[] lines = jsonString.Split('\n');
            int linesToShow = Math.Min(10, lines.Length);
            
            for (int i = 0; i < linesToShow; i++)
            {
                Console.WriteLine($"    {lines[i]}");
            }
            
            if (lines.Length > linesToShow)
            {
                Console.WriteLine("    ... (truncated)");
            }
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrate the complete JSON serialization cycle.
        /// JSON is the most popular format for web APIs and configuration files.
        /// </summary>
        public void DemonstrateJsonSerialization()
        {
            Console.WriteLine("=== JSON SERIALIZATION DEMONSTRATION ===");
            Console.WriteLine("JSON serialization converts objects to lightweight, human-readable JSON format.");
            Console.WriteLine("This is the standard for web APIs and modern data exchange.\n");
            
            // Create a sample student
            var originalStudent = new Student(301, "Carol Williams", 21, "carol.williams@university.edu", 3.85m);
            originalStudent.Courses.AddRange(new[] { "Web Development", "Mobile Apps", "Cloud Computing" });
            
            Console.WriteLine("Original Student Data:");
            Console.WriteLine(originalStudent);
            
            // Serialize to JSON
            SerializeStudent(originalStudent, "student.json");
            
            // Deserialize from JSON
            var deserializedStudent = DeserializeStudent("student.json");
            
            if (deserializedStudent != null)
            {
                Console.WriteLine("Deserialized Student Data:");
                Console.WriteLine(deserializedStudent);
            }
            
            // Also demonstrate course serialization
            Console.WriteLine("--- Course JSON Serialization Example ---");
            var course = new Course("WEB101", "Web Development Fundamentals", 4, "Prof. John Smith");
            SerializeCourse(course, "course.json");
            
            Console.WriteLine("\n" + new string('=', 60) + "\n");
        }
        
        /// <summary>
        /// Demonstrate async JSON operations.
        /// In real applications, async operations prevent blocking the UI thread.
        /// </summary>
        public async Task DemonstrateAsyncJsonSerialization()
        {
            Console.WriteLine("=== ASYNC JSON SERIALIZATION DEMONSTRATION ===");
            Console.WriteLine("Asynchronous operations are essential for responsive applications.\n");
            
            var student = new Student(401, "David Brown", 23, "david.brown@university.edu", 3.60m);
            student.Courses.AddRange(new[] { "Software Engineering", "System Design" });
            
            Console.WriteLine("Performing async JSON serialization...");
            await SerializeStudentAsync(student, "student_async.json");
            
            Console.WriteLine("Performing async JSON deserialization...");
            var deserializedStudent = await DeserializeStudentAsync("student_async.json");
            
            if (deserializedStudent != null)
            {
                Console.WriteLine("Async Deserialized Student Data:");
                Console.WriteLine(deserializedStudent);
            }
            
            Console.WriteLine("\n" + new string('=', 60) + "\n");
        }
    }
}
