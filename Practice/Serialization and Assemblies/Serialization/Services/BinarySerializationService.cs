using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Serialization.Models;

namespace Serialization.Services
{
    /// <summary>
    /// Binary serialization service - handles converting objects to binary format.
    /// Binary serialization preserves the exact state of objects including private fields.
    /// Best for performance but not human-readable and platform-specific.
    /// </summary>
    public class BinarySerializationService
    {
        private readonly string _dataDirectory;
        
        public BinarySerializationService()
        {
            // Create a dedicated directory for binary files
            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Binary");
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }
          /// <summary>
        /// Serialize an object to binary format and save it to a file.
        /// This method follows the step-by-step process outlined in the material.
        /// NOTE: BinaryFormatter is obsolete in .NET 5+ for security reasons.
        /// </summary>
        public void SerializeStudent(Student student, string fileName)
        {
            try
            {
                Console.WriteLine("⚠️  IMPORTANT NOTE ABOUT BINARY SERIALIZATION:");
                Console.WriteLine("BinaryFormatter is obsolete and disabled in modern .NET for security reasons.");
                Console.WriteLine("In real projects, use alternatives like MessagePack, protobuf, or System.Text.Json.");
                Console.WriteLine("This demonstration shows the concept but uses a safe alternative.\n");
                
                // Alternative: Use JSON as a substitute to demonstrate the concept
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                // Convert student to JSON first (this simulates binary serialization process)
                string jsonData = System.Text.Json.JsonSerializer.Serialize(student);
                
                // Convert to bytes to simulate binary format
                byte[] binaryData = System.Text.Encoding.UTF8.GetBytes(jsonData);
                
                // Write binary data to file
                File.WriteAllBytes(filePath, binaryData);
                
                Console.WriteLine($"✓ Successfully serialized student to binary-like file: {fileName}");
                Console.WriteLine($"  File location: {filePath}");
                Console.WriteLine($"  File size: {new FileInfo(filePath).Length} bytes");
                Console.WriteLine($"  Note: Using JSON-to-binary conversion as BinaryFormatter substitute\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during binary serialization: {ex.Message}");
            }
        }
          /// <summary>
        /// Deserialize a student object from a binary file.
        /// This demonstrates the reverse process - getting our object back from storage.
        /// NOTE: Using safe alternative to BinaryFormatter.
        /// </summary>
        public Student? DeserializeStudent(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                // Check if file exists before attempting to deserialize
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"✗ Binary file not found: {fileName}");
                    return null;
                }
                
                // Read binary data from file
                byte[] binaryData = File.ReadAllBytes(filePath);
                
                // Convert bytes back to JSON string
                string jsonData = System.Text.Encoding.UTF8.GetString(binaryData);
                
                // Deserialize from JSON
                Student? student = System.Text.Json.JsonSerializer.Deserialize<Student>(jsonData);
                
                Console.WriteLine($"✓ Successfully deserialized student from binary-like file: {fileName}");
                Console.WriteLine($"  Note: Using binary-to-JSON conversion as BinaryFormatter substitute\n");
                return student;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during binary deserialization: {ex.Message}");
                return null;
            }
        }
          /// <summary>
        /// Demonstrate the complete binary serialization cycle.
        /// This shows both serialization and deserialization in action.
        /// </summary>
        public void DemonstrateBinarySerialization()
        {
            Console.WriteLine("=== BINARY SERIALIZATION DEMONSTRATION ===");
            Console.WriteLine("Binary serialization converts objects to a binary format that preserves");
            Console.WriteLine("the complete object state, including private fields and type information.");
            Console.WriteLine("NOTE: Traditional BinaryFormatter is obsolete for security reasons.\n");
            
            // Create a sample student object
            var originalStudent = new Student(101, "Alice Johnson", 20, "alice.johnson@university.edu", 3.75m);
            originalStudent.Courses.AddRange(new[] { "Computer Science 101", "Mathematics 201", "Physics 101" });
            
            Console.WriteLine("Original Student Data:");
            Console.WriteLine(originalStudent);
            
            // Serialize the student
            SerializeStudent(originalStudent, "student.dat");
            
            // Deserialize the student
            var deserializedStudent = DeserializeStudent("student.dat");
            
            if (deserializedStudent != null)
            {
                Console.WriteLine("Deserialized Student Data:");
                Console.WriteLine(deserializedStudent);
                
                // Verify the data integrity
                bool dataMatches = originalStudent.Id == deserializedStudent.Id &&
                                   originalStudent.Name == deserializedStudent.Name &&
                                   originalStudent.Age == deserializedStudent.Age &&
                                   originalStudent.Email == deserializedStudent.Email &&
                                   originalStudent.GPA == deserializedStudent.GPA;
                
                Console.WriteLine($"Data integrity check: {(dataMatches ? "✓ PASSED" : "✗ FAILED")}");
            }
            
            Console.WriteLine("\nModern Alternatives to BinaryFormatter:");
            Console.WriteLine("• MessagePack - High-performance binary serialization");
            Console.WriteLine("• Protocol Buffers (protobuf) - Cross-language serialization");
            Console.WriteLine("• System.Text.Json - Fast and secure JSON serialization");
            Console.WriteLine("• Custom binary writers for performance-critical scenarios");
            
            Console.WriteLine("\n" + new string('=', 60) + "\n");
        }
    }
}
