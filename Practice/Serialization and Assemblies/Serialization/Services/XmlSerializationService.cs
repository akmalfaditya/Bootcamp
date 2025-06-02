using System;
using System.IO;
using System.Xml.Serialization;
using Serialization.Models;

namespace Serialization.Services
{
    /// <summary>
    /// XML serialization service - converts objects to XML format.
    /// XML serialization creates human-readable text files that are cross-platform.
    /// Only serializes public properties and fields, making it more secure than binary.
    /// </summary>
    public class XmlSerializationService
    {
        private readonly string _dataDirectory;
        
        public XmlSerializationService()
        {
            // Create a dedicated directory for XML files
            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "XML");
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }
        
        /// <summary>
        /// Serialize a student object to XML format.
        /// This creates a readable XML file that can be opened in any text editor.
        /// </summary>
        public void SerializeStudent(Student student, string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                // Create a file stream to write the XML data
                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                
                // Create an XML serializer specifically for the Student type
                // The serializer needs to know the exact type it's working with
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Student));
                
                // Serialize the student object and write it to the file stream
                // This converts the object properties into XML elements and attributes
                xmlSerializer.Serialize(fileStream, student);
                
                Console.WriteLine($"✓ Successfully serialized student to XML file: {fileName}");
                Console.WriteLine($"  File location: {filePath}");
                Console.WriteLine($"  File size: {new FileInfo(filePath).Length} bytes");
                
                // Let's show a preview of the XML content
                ShowXmlPreview(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during XML serialization: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Deserialize a student object from an XML file.
        /// This reads the XML file and reconstructs the original object.
        /// </summary>
        public Student? DeserializeStudent(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"✗ XML file not found: {fileName}");
                    return null;
                }
                
                // Create a file stream to read the XML data
                using FileStream fileStream = new FileStream(filePath, FileMode.Open);
                
                // Create an XML serializer for the Student type
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Student));
                
                // Deserialize the XML back into a Student object
                // The serializer reads the XML and creates a new Student instance
                Student? student = (Student?)xmlSerializer.Deserialize(fileStream);
                
                Console.WriteLine($"✓ Successfully deserialized student from XML file: {fileName}\n");
                return student;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during XML deserialization: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Serialize a course object to demonstrate XML serialization with different object types.
        /// </summary>
        public void SerializeCourse(Course course, string fileName)
        {
            try
            {
                string filePath = Path.Combine(_dataDirectory, fileName);
                
                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Course));
                xmlSerializer.Serialize(fileStream, course);
                
                Console.WriteLine($"✓ Successfully serialized course to XML file: {fileName}");
                ShowXmlPreview(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error during course XML serialization: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Show a preview of the XML content to help understand the serialization format.
        /// </summary>
        private void ShowXmlPreview(string filePath)
        {
            try
            {
                string xmlContent = File.ReadAllText(filePath);
                Console.WriteLine("  XML Content Preview:");
                
                // Show first few lines of the XML file
                string[] lines = xmlContent.Split('\n');
                int linesToShow = Math.Min(5, lines.Length);
                
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
            catch (Exception ex)
            {
                Console.WriteLine($"  Could not show XML preview: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Demonstrate the complete XML serialization cycle.
        /// This shows the advantages of XML format - human readable and cross-platform.
        /// </summary>
        public void DemonstrateXmlSerialization()
        {
            Console.WriteLine("=== XML SERIALIZATION DEMONSTRATION ===");
            Console.WriteLine("XML serialization converts objects to human-readable XML format.");
            Console.WriteLine("This format is cross-platform and can be easily inspected or modified.\n");
            
            // Create a sample student
            var originalStudent = new Student(201, "Bob Smith", 22, "bob.smith@university.edu", 3.50m);
            originalStudent.Courses.AddRange(new[] { "Data Structures", "Algorithms", "Database Systems" });
            
            Console.WriteLine("Original Student Data:");
            Console.WriteLine(originalStudent);
            
            // Serialize to XML
            SerializeStudent(originalStudent, "student.xml");
            
            // Deserialize from XML
            var deserializedStudent = DeserializeStudent("student.xml");
            
            if (deserializedStudent != null)
            {
                Console.WriteLine("Deserialized Student Data:");
                Console.WriteLine(deserializedStudent);
            }
            
            // Also demonstrate course serialization
            Console.WriteLine("--- Course Serialization Example ---");
            var course = new Course("CS101", "Introduction to Computer Science", 3, "Dr. Jane Doe");
            SerializeCourse(course, "course.xml");
            
            Console.WriteLine("\n" + new string('=', 60) + "\n");
        }
    }
}
