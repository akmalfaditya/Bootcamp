// FileStreamAdvancedDemo.cs - Advanced FileStream concepts and error handling
using System;
using System.IO;
using System.Text;

public static class FileStreamAdvancedDemo
{
    public static void RunAdvancedFileStreamDemo()
    {
        Console.WriteLine("=== Advanced FileStream Concepts ===");
        Console.WriteLine("====================================");
        
        DemonstrateFileShareModes();
        DemonstrateFileStreamPositioning();
        DemonstrateFileStreamErrorHandling();
        DemonstrateFileStreamPerformance();
        
        Console.WriteLine();
    }
    
    // Demo FileShare enumeration values
    static void DemonstrateFileShareModes()
    {
        Console.WriteLine("FileShare Modes Demo:");
        
        string filePath = "share_demo.txt";
        
        try
        {
            // Create initial file
            File.WriteAllText(filePath, "Testing file sharing modes");
            
            // FileShare.None - Exclusive access
            Console.WriteLine("Testing FileShare modes:");
            
            using (var fs1 = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Console.WriteLine("✓ First stream opened with FileShare.Read");
                
                try
                {
                    // This should work - we're allowing read sharing
                    using (var fs2 = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Console.WriteLine("✓ Second stream opened successfully (Read sharing allowed)");
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("❌ Second stream failed due to sharing restrictions");
                }
                
                try
                {
                    // This might fail - trying to write while another stream has read access
                    using (var fs3 = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
                    {
                        Console.WriteLine("⚠️  Write access opened (might conflict depending on system)");
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"✓ Expected: Write access blocked - {ex.Message.Split('.')[0]}");
                }
            }
            
            // Demonstrate FileShare.ReadWrite
            using (var fs4 = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Console.WriteLine("✓ Opened with FileShare.ReadWrite - allows concurrent access");
                
                // Multiple streams can now access the file
                using (var fs5 = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Console.WriteLine("✓ Concurrent read access successful");
                }
            }
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ FileShare demo error: {ex.Message}");
        }
    }
    
    // Demo file positioning and seeking
    static void DemonstrateFileStreamPositioning()
    {
        Console.WriteLine("\nFileStream Positioning & Seeking:");
        
        string filePath = "positioning_demo.txt";
        
        try
        {
            // Create file with known content
            string content = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            File.WriteAllText(filePath, content);
            
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                Console.WriteLine($"Initial position: {fs.Position}");
                Console.WriteLine($"File length: {fs.Length}");
                
                // Seek to different positions
                fs.Seek(10, SeekOrigin.Begin);
                Console.WriteLine($"After Seek(10, Begin): Position = {fs.Position}");
                Console.WriteLine($"Character at position: '{(char)fs.ReadByte()}'");
                
                fs.Seek(-5, SeekOrigin.Current);
                Console.WriteLine($"After Seek(-5, Current): Position = {fs.Position}");
                Console.WriteLine($"Character at position: '{(char)fs.ReadByte()}'");
                
                fs.Seek(-10, SeekOrigin.End);
                Console.WriteLine($"After Seek(-10, End): Position = {fs.Position}");
                Console.WriteLine($"Character at position: '{(char)fs.ReadByte()}'");
                
                // Demonstrate writing at specific position
                fs.Seek(5, SeekOrigin.Begin);
                fs.WriteByte((byte)'*');
                Console.WriteLine("✓ Wrote '*' at position 5");
                
                // Read entire file to see modification
                fs.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string modifiedContent = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"Modified content: {modifiedContent}");
            }
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Positioning demo error: {ex.Message}");
        }
    }
    
    // Demo comprehensive error handling
    static void DemonstrateFileStreamErrorHandling()
    {
        Console.WriteLine("\nFileStream Error Handling:");
        
        try
        {
            // Test 1: File not found
            try
            {
                using (var fs = new FileStream("nonexistent_file.txt", FileMode.Open))
                {
                    // This won't execute
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"✓ Caught FileNotFoundException: {ex.Message.Split('\n')[0]}");
            }
            
            // Test 2: Unauthorized access
            try
            {
                string readOnlyFile = "readonly_test.txt";
                File.WriteAllText(readOnlyFile, "Read-only content");
                File.SetAttributes(readOnlyFile, FileAttributes.ReadOnly);
                
                using (var fs = new FileStream(readOnlyFile, FileMode.Open, FileAccess.Write))
                {
                    // This should fail
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"✓ Caught UnauthorizedAccessException: Access denied to read-only file");
                
                // Clean up
                string readOnlyFile = "readonly_test.txt";
                if (File.Exists(readOnlyFile))
                {
                    File.SetAttributes(readOnlyFile, FileAttributes.Normal);
                    File.Delete(readOnlyFile);
                }
            }
            
            // Test 3: Invalid file mode combination
            try
            {
                string testFile = "mode_test.txt";
                File.WriteAllText(testFile, "test");
                
                using (var fs = new FileStream(testFile, FileMode.CreateNew)) // Should fail - file exists
                {
                    // This won't execute
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"✓ Caught IOException: {ex.Message.Split('.')[0]}");
                
                // Clean up
                if (File.Exists("mode_test.txt"))
                    File.Delete("mode_test.txt");
            }
            
            Console.WriteLine("✓ Error handling demonstration completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error in error handling demo: {ex.Message}");
        }
    }
    
    // Demo performance considerations
    static void DemonstrateFileStreamPerformance()
    {
        Console.WriteLine("\nFileStream Performance Considerations:");
        
        string filePath = "performance_test.txt";
        
        try
        {
            const int iterations = 1000;
            const int dataSize = 100;
            byte[] testData = new byte[dataSize];
            new Random().NextBytes(testData);
            
            // Test 1: Writing without buffering
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                for (int i = 0; i < iterations; i++)
                {
                    fs.Write(testData, 0, testData.Length);
                    // fs.Flush(); // Uncomment to force immediate disk writes
                }
            }
            
            stopwatch.Stop();
            long unbufferedTime = stopwatch.ElapsedMilliseconds;
            long unbufferedSize = new FileInfo(filePath).Length;
            
            Console.WriteLine($"✓ Direct FileStream writing:");
            Console.WriteLine($"  Time: {unbufferedTime}ms");
            Console.WriteLine($"  Size: {unbufferedSize} bytes");
            
            File.Delete(filePath);
            
            // Test 2: Writing with BufferedStream
            stopwatch.Restart();
            
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var buffered = new BufferedStream(fs, bufferSize: 4096))
            {
                for (int i = 0; i < iterations; i++)
                {
                    buffered.Write(testData, 0, testData.Length);
                }
            } // BufferedStream.Dispose() automatically flushes
            
            stopwatch.Stop();
            long bufferedTime = stopwatch.ElapsedMilliseconds;
            long bufferedSize = new FileInfo(filePath).Length;
            
            Console.WriteLine($"✓ BufferedStream writing:");
            Console.WriteLine($"  Time: {bufferedTime}ms");
            Console.WriteLine($"  Size: {bufferedSize} bytes");
            
            // Performance comparison
            if (bufferedTime > 0)
            {
                double improvement = ((double)(unbufferedTime - bufferedTime) / Math.Max(bufferedTime, 1)) * 100;
                Console.WriteLine($"✓ Performance improvement: {improvement:F1}%");
            }
            
            Console.WriteLine("ℹ️  Performance benefits vary by:");
            Console.WriteLine("   - Operation size (smaller operations benefit more from buffering)");
            Console.WriteLine("   - Storage type (HDD vs SSD vs Network)");
            Console.WriteLine("   - System load and available memory");
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Performance demo error: {ex.Message}");
        }
    }
}
