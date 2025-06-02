using System;
using System.IO;
using Serialization.Models;
using Serialization.Services;
using Serialization.Examples;

namespace Serialization
{
    /// <summary>
    /// Main program that demonstrates all three types of serialization covered in the material:
    /// 1. Binary Serialization - for performance and exact object state preservation
    /// 2. XML Serialization - for human-readable, cross-platform data exchange
    /// 3. JSON Serialization - for modern web applications and APIs
    /// 
    /// This program follows the step-by-step processes outlined in the training material.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== COMPREHENSIVE SERIALIZATION DEMONSTRATION ===");
            Console.WriteLine("This program demonstrates the three main types of serialization in C#:");
            Console.WriteLine("1. Binary Serialization");
            Console.WriteLine("2. XML Serialization");
            Console.WriteLine("3. JSON Serialization");
            Console.WriteLine();
            Console.WriteLine("Press any key to start the demonstrations...");
            Console.ReadKey();
            Console.Clear();
              try
            {
                // Start with the basic example from the training material
                BasicSerializationExample.RunExample();
                
                Console.WriteLine("Press any key to continue to advanced demonstrations...");
                Console.ReadKey();
                Console.Clear();
                
                // Ensure our data directories exist
                EnsureDataDirectoriesExist();
                
                // Create service instances for each serialization type
                var binaryService = new BinarySerializationService();
                var xmlService = new XmlSerializationService();
                var jsonService = new JsonSerializationService();
                
                // Demonstrate Binary Serialization
                // This is the most compact format but not human-readable
                binaryService.DemonstrateBinarySerialization();
                
                // Wait for user input between demonstrations
                Console.WriteLine("Press any key to continue to XML serialization...");
                Console.ReadKey();
                Console.Clear();
                
                // Demonstrate XML Serialization
                // This creates human-readable XML files that work across platforms
                xmlService.DemonstrateXmlSerialization();
                
                // Wait for user input
                Console.WriteLine("Press any key to continue to JSON serialization...");
                Console.ReadKey();
                Console.Clear();
                
                // Demonstrate JSON Serialization (synchronous)
                // This is the modern standard for web applications
                jsonService.DemonstrateJsonSerialization();
                
                // Wait for user input
                Console.WriteLine("Press any key to see async JSON operations...");
                Console.ReadKey();
                Console.Clear();
                
                // Demonstrate Async JSON Serialization
                // Shows how to handle serialization in modern async applications
                await jsonService.DemonstrateAsyncJsonSerialization();
                
                // Performance comparison
                Console.WriteLine("Press any key to see performance comparison...");
                Console.ReadKey();
                Console.Clear();
                
                PerformanceComparison(binaryService, xmlService, jsonService);
                
                // Show final summary
                ShowFilesSummary();
                
                Console.WriteLine("=== SERIALIZATION DEMONSTRATION COMPLETE ===");
                Console.WriteLine("All serialization types have been demonstrated successfully!");
                Console.WriteLine("Check the Data folder to see the generated files.");
                Console.WriteLine();
                Console.WriteLine("Key takeaways:");
                Console.WriteLine("• Binary: Fastest, smallest files, but not human-readable");
                Console.WriteLine("• XML: Human-readable, cross-platform, larger files");
                Console.WriteLine("• JSON: Modern standard, web-friendly, good balance of size and readability");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during the demonstration: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
        
        /// <summary>
        /// Ensure all data directories exist before we start serialization operations.
        /// Good practice to check for directory existence in file operations.
        /// </summary>
        private static void EnsureDataDirectoriesExist()
        {
            string baseDataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            
            // Create directories if they don't exist
            Directory.CreateDirectory(Path.Combine(baseDataDir, "Binary"));
            Directory.CreateDirectory(Path.Combine(baseDataDir, "XML"));
            Directory.CreateDirectory(Path.Combine(baseDataDir, "JSON"));
            
            Console.WriteLine($"Data directories prepared at: {baseDataDir}");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Compare the performance characteristics of different serialization methods.
        /// This helps understand when to use each approach.
        /// </summary>
        private static void PerformanceComparison(
            BinarySerializationService binaryService,
            XmlSerializationService xmlService,
            JsonSerializationService jsonService)
        {
            Console.WriteLine("=== PERFORMANCE AND FILE SIZE COMPARISON ===");
            Console.WriteLine("Comparing serialization methods with the same student object...\n");
            
            // Create a test student with substantial data
            var testStudent = new Student(999, "Performance Test Student", 25, "test@university.edu", 3.95m);
            testStudent.Courses.AddRange(new[] 
            { 
                "Advanced Data Structures",
                "Computer Graphics",
                "Machine Learning",
                "Distributed Systems",
                "Cybersecurity Fundamentals"
            });
            
            Console.WriteLine("Test Student Data:");
            Console.WriteLine(testStudent);
            
            // Measure Binary Serialization
            var binaryStopwatch = System.Diagnostics.Stopwatch.StartNew();
            binaryService.SerializeStudent(testStudent, "performance_test.dat");
            binaryStopwatch.Stop();
            
            // Measure XML Serialization
            var xmlStopwatch = System.Diagnostics.Stopwatch.StartNew();
            xmlService.SerializeStudent(testStudent, "performance_test.xml");
            xmlStopwatch.Stop();
            
            // Measure JSON Serialization
            var jsonStopwatch = System.Diagnostics.Stopwatch.StartNew();
            jsonService.SerializeStudent(testStudent, "performance_test.json");
            jsonStopwatch.Stop();
            
            // Get file sizes for comparison
            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            long binarySize = GetFileSize(Path.Combine(dataDir, "Binary", "performance_test.dat"));
            long xmlSize = GetFileSize(Path.Combine(dataDir, "XML", "performance_test.xml"));
            long jsonSize = GetFileSize(Path.Combine(dataDir, "JSON", "performance_test.json"));
            
            // Display comparison results
            Console.WriteLine("PERFORMANCE COMPARISON RESULTS:");
            Console.WriteLine($"Binary: {binaryStopwatch.ElapsedMilliseconds}ms, {binarySize} bytes");
            Console.WriteLine($"XML:    {xmlStopwatch.ElapsedMilliseconds}ms, {xmlSize} bytes");
            Console.WriteLine($"JSON:   {jsonStopwatch.ElapsedMilliseconds}ms, {jsonSize} bytes");
            Console.WriteLine();
            
            // Determine the winner in each category
            var fastestTime = Math.Min(Math.Min(binaryStopwatch.ElapsedMilliseconds, xmlStopwatch.ElapsedMilliseconds), jsonStopwatch.ElapsedMilliseconds);
            var smallestSize = Math.Min(Math.Min(binarySize, xmlSize), jsonSize);
            
            Console.WriteLine("ANALYSIS:");
            if (binaryStopwatch.ElapsedMilliseconds == fastestTime) Console.WriteLine("• Binary is fastest for serialization");
            if (xmlStopwatch.ElapsedMilliseconds == fastestTime) Console.WriteLine("• XML is fastest for serialization");
            if (jsonStopwatch.ElapsedMilliseconds == fastestTime) Console.WriteLine("• JSON is fastest for serialization");
            
            if (binarySize == smallestSize) Console.WriteLine("• Binary produces smallest files");
            if (xmlSize == smallestSize) Console.WriteLine("• XML produces smallest files");
            if (jsonSize == smallestSize) Console.WriteLine("• JSON produces smallest files");
            
            Console.WriteLine("\n" + new string('=', 60) + "\n");
        }
        
        /// <summary>
        /// Get the size of a file safely, returning 0 if the file doesn't exist.
        /// </summary>
        private static long GetFileSize(string filePath)
        {
            try
            {
                return File.Exists(filePath) ? new FileInfo(filePath).Length : 0;
            }
            catch
            {
                return 0;
            }
        }
        
        /// <summary>
        /// Show a summary of all files created during the demonstration.
        /// This helps students see the concrete results of serialization.
        /// </summary>
        private static void ShowFilesSummary()
        {
            Console.WriteLine("=== GENERATED FILES SUMMARY ===");
            Console.WriteLine("The following files were created during this demonstration:\n");
            
            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            
            ShowDirectoryContents("Binary Files", Path.Combine(dataDir, "Binary"));
            ShowDirectoryContents("XML Files", Path.Combine(dataDir, "XML"));
            ShowDirectoryContents("JSON Files", Path.Combine(dataDir, "JSON"));
            
            Console.WriteLine("You can examine these files to see how each serialization format");
            Console.WriteLine("represents the same data in different ways.\n");
            Console.WriteLine(new string('=', 60) + "\n");
        }
        
        /// <summary>
        /// Helper method to display contents of a directory.
        /// </summary>
        private static void ShowDirectoryContents(string title, string directoryPath)
        {
            Console.WriteLine($"{title}:");
            if (Directory.Exists(directoryPath))
            {
                var files = Directory.GetFiles(directoryPath);
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        Console.WriteLine($"  • {fileInfo.Name} ({fileInfo.Length} bytes)");
                    }
                }
                else
                {
                    Console.WriteLine("  No files found");
                }
            }
            else
            {
                Console.WriteLine("  Directory not found");
            }
            Console.WriteLine();
        }
    }
}
