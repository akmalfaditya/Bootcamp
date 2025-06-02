// FileHelperDemo.cs - Demonstrates File helper methods and different FileMode options
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public static class FileHelperDemo
{
    public static async Task RunAllFileHelperDemos()
    {
        Console.WriteLine("=== File Helper Methods Demo ===");
        Console.WriteLine("---------------------------------");
        
        await DemonstrateFileHelperMethods();
        await DemonstrateFileModes();
        await DemonstratePathOperations();
        
        Console.WriteLine();
    }
    
    // Show all the convenient File.* helper methods
    static async Task DemonstrateFileHelperMethods()
    {
        Console.WriteLine("File Helper Methods:");
        
        string textFile = "helper_text.txt";
        string binaryFile = "helper_binary.bin";
        
        try
        {
            // Text operations
            string content = "Line 1\nLine 2\nLine 3";
            await File.WriteAllTextAsync(textFile, content);
            Console.WriteLine("✓ File.WriteAllText - wrote text content");
            
            string readContent = await File.ReadAllTextAsync(textFile);
            Console.WriteLine($"✓ File.ReadAllText - read: {readContent.Replace('\n', ' ')}");
            
            string[] lines = { "First line", "Second line", "Third line" };
            await File.WriteAllLinesAsync(textFile, lines);
            Console.WriteLine("✓ File.WriteAllLines - wrote array of lines");
            
            string[] readLines = await File.ReadAllLinesAsync(textFile);
            Console.WriteLine($"✓ File.ReadAllLines - read {readLines.Length} lines");
            
            // Binary operations
            byte[] binaryData = { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello" in ASCII
            await File.WriteAllBytesAsync(binaryFile, binaryData);
            Console.WriteLine("✓ File.WriteAllBytes - wrote binary data");
            
            byte[] readBinary = await File.ReadAllBytesAsync(binaryFile);
            string binaryAsText = Encoding.ASCII.GetString(readBinary);
            Console.WriteLine($"✓ File.ReadAllBytes - read binary as text: {binaryAsText}");
            
            // Append operation
            await File.AppendAllTextAsync(textFile, "\nAppended line");
            Console.WriteLine("✓ File.AppendAllText - appended additional content");
            
            // Clean up
            File.Delete(textFile);
            File.Delete(binaryFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in file helper demo: {ex.Message}");
        }
    }
    
    // Demonstrate different FileMode options
    static async Task DemonstrateFileModes()
    {
        Console.WriteLine("\nFileMode Options:");
        
        string fileName = "filemode_demo.txt";
        
        try
        {
            // Create - creates new file, overwrites if exists
            using (var fs1 = new FileStream(fileName, FileMode.Create))
            {
                byte[] data = Encoding.UTF8.GetBytes("Original content");
                await fs1.WriteAsync(data, 0, data.Length);
            }
            Console.WriteLine("✓ FileMode.Create - created file with original content");
            
            // Check file content
            string content1 = await File.ReadAllTextAsync(fileName);
            Console.WriteLine($"  Content: {content1}");
            
            // CreateNew - would fail if file exists, so we delete first
            File.Delete(fileName);
            using (var fs2 = new FileStream(fileName, FileMode.CreateNew))
            {
                byte[] data = Encoding.UTF8.GetBytes("CreateNew content");
                await fs2.WriteAsync(data, 0, data.Length);
            }
            Console.WriteLine("✓ FileMode.CreateNew - created new file");
            
            // Open - opens existing file
            using (var fs3 = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs3.Length];
                await fs3.ReadAsync(buffer, 0, buffer.Length);
                string content = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"✓ FileMode.Open - read existing file: {content}");
            }
            
            // Append - opens for writing at the end
            using (var fs4 = new FileStream(fileName, FileMode.Append))
            {
                byte[] data = Encoding.UTF8.GetBytes(" + appended");
                await fs4.WriteAsync(data, 0, data.Length);
            }
            Console.WriteLine("✓ FileMode.Append - appended to existing file");
            
            string finalContent = await File.ReadAllTextAsync(fileName);
            Console.WriteLine($"  Final content: {finalContent}");
            
            File.Delete(fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in FileMode demo: {ex.Message}");
        }
    }
    
    // Demonstrate path operations and filename handling
    static async Task DemonstratePathOperations()
    {
        Console.WriteLine("\nPath Operations:");
        
        try
        {
            // Absolute vs relative paths
            string relativePath = "data.txt";
            string absolutePath = Path.GetFullPath(relativePath);
            
            Console.WriteLine($"✓ Relative path: {relativePath}");
            Console.WriteLine($"✓ Absolute path: {absolutePath}");
            
            // Path manipulation
            string directory = Path.GetDirectoryName(absolutePath);
            string fileName = Path.GetFileName(absolutePath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(absolutePath);
            string extension = Path.GetExtension(absolutePath);
            
            Console.WriteLine($"✓ Directory: {directory}");
            Console.WriteLine($"✓ Filename: {fileName}");
            Console.WriteLine($"✓ Name without extension: {fileNameWithoutExt}");
            Console.WriteLine($"✓ Extension: {extension}");
            
            // Combine paths safely
            string subDir = "subfolder";
            string subFile = "subfile.dat";
            string combinedPath = Path.Combine(directory, subDir, subFile);
            Console.WriteLine($"✓ Combined path: {combinedPath}");
            
            // Temporary file operations
            string tempFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFile, "Temporary content");
            Console.WriteLine($"✓ Created temp file: {tempFile}");
            
            // Check if file exists
            bool exists = File.Exists(tempFile);
            Console.WriteLine($"✓ Temp file exists: {exists}");
            
            File.Delete(tempFile);
            Console.WriteLine("✓ Cleaned up temp file");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in path operations demo: {ex.Message}");
        }
    }
}
