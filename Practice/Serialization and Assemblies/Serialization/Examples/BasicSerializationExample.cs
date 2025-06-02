using System;
using System.IO;
using System.Xml.Serialization;

namespace Serialization.Examples
{
    /// <summary>
    /// This example follows the exact Tutorial class pattern from the training material.
    /// It demonstrates the basic XML serialization approach step by step.
    /// </summary>
    /// 
    // A class to serialize and deserialize - exactly like the material example
    [Serializable]
    public class Tutorial
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Basic serialization example that mirrors the material provided.
    /// This shows the fundamental concepts before moving to more complex scenarios.
    /// </summary>
    public class BasicSerializationExample
    {
        public static void RunExample()
        {
            Console.WriteLine("=== BASIC SERIALIZATION EXAMPLE (from Training Material) ===");
            Console.WriteLine("This example follows the exact pattern from your training material.\n");
            
            // Create an object of Tutorial class - Step 1 from material
            Tutorial t1 = new Tutorial();
            t1.ID = 1;
            t1.Name = ".Net";
            
            Console.WriteLine("Original Tutorial object:");
            Console.WriteLine($"ID: {t1.ID}");
            Console.WriteLine($"Name: {t1.Name}\n");

            // Ensure directory exists
            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Examples");
            Directory.CreateDirectory(dataDir);
            string filePath = Path.Combine(dataDir, "Example.xml");

            // Create a file stream to write the object - Step 2
            FileStream fs = new FileStream(filePath, FileMode.Create);

            // Create an XML serializer to serialize the object - Step 3
            XmlSerializer xs = new XmlSerializer(typeof(Tutorial));

            // Serialize the object and write it to the file stream - Step 4
            xs.Serialize(fs, t1);

            // Close the file stream - Step 5
            fs.Close();
            
            Console.WriteLine("✓ Serialization completed successfully!");
            Console.WriteLine($"File saved to: {filePath}\n");

            // Now demonstrate deserialization
            Console.WriteLine("Starting deserialization process...");

            // Create another file stream to read the object - Deserialization Step 1
            FileStream fs2 = new FileStream(filePath, FileMode.Open);

            // Create another XML serializer to deserialize the object - Step 2
            XmlSerializer xs2 = new XmlSerializer(typeof(Tutorial));            // Deserialize the object and cast it to Tutorial class - Step 3
            Tutorial? t2 = (Tutorial?)xs2.Deserialize(fs2);

            // Close the file stream - Step 4
            fs2.Close();

            // Print the properties of the deserialized object
            if (t2 != null)
            {
                Console.WriteLine("Deserialized Tutorial object:");
                Console.WriteLine("ID: {0}", t2.ID);
                Console.WriteLine("Name: {0}", t2.Name);
                
                // Verify data integrity
                bool dataMatches = t1.ID == t2.ID && t1.Name == t2.Name;
                Console.WriteLine($"\nData integrity check: {(dataMatches ? "✓ PASSED" : "✗ FAILED")}");
            }
            else
            {
                Console.WriteLine("✗ Failed to deserialize Tutorial object");
            }
            
            // Show the XML content that was created
            ShowXmlContent(filePath);
            
            Console.WriteLine("\n" + new string('=', 60) + "\n");
        }
        
        private static void ShowXmlContent(string filePath)
        {
            try
            {
                Console.WriteLine("\nGenerated XML content:");
                string xmlContent = File.ReadAllText(filePath);
                Console.WriteLine(xmlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not read XML file: {ex.Message}");
            }
        }
    }
}
