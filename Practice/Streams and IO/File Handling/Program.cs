// File Handling Comprehensive Demo
// This project demonstrates all file handling concepts using System.IO namespace

using System;
using System.IO;
using System.Text;
using System.IO.Compression;

class Program
{    static void Main(string[] args)
    {
        Console.WriteLine("=== C# File Handling Comprehensive Demo ===\n");
        
        try
        {
            // Core file handling concepts
            RunFileStreamDemo();
            RunStreamWriterReaderDemo();
            RunFileClassDemo();
            RunCompressionDemo();
            
            // Advanced concepts
            FileStreamAdvancedDemo.RunAdvancedFileStreamDemo();
            
            // Practical real-world examples
            PracticalExamplesDemo.RunPracticalExamples();
            
            Console.WriteLine("\n=== All file handling demos completed successfully! ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ An unexpected error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
    
    // FileStream demo - Low level file operations with complete control
    static void RunFileStreamDemo()
    {
        Console.WriteLine("1. FileStream Operations Demo");
        Console.WriteLine("============================");
        
        // File paths for demonstration
        string filePath = "filestream_demo.txt";
        string content = "FileStream provides complete control over file operations!";
        
        DemoFileCreation(filePath);
        DemoFileWriting(filePath, content);
        DemoFileReading(filePath);
        DemoFileModesAndAccess();
        
        // Cleanup
        if (File.Exists(filePath))
            File.Delete(filePath);
            
        Console.WriteLine();
    }
    
    // Demo file creation using FileStream
    static void DemoFileCreation(string filePath)
    {
        Console.WriteLine("File Creation with FileStream:");
        
        try
        {
            // Create file using FileStream constructor
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                Console.WriteLine($"✓ File '{filePath}' created successfully");
                Console.WriteLine($"  Can Read: {fileStream.CanRead}");
                Console.WriteLine($"  Can Write: {fileStream.CanWrite}");
                Console.WriteLine($"  Can Seek: {fileStream.CanSeek}");
                
                // Important: Always close or use 'using' to release resources
                // fileStream.Close(); // Not needed with 'using' statement
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating file: {ex.Message}");
        }
    }
    
    // Demo writing to file using FileStream
    static void DemoFileWriting(string filePath, string content)
    {
        Console.WriteLine("\nFile Writing with FileStream:");
        
        try
        {
            // Open file for appending data
            using (FileStream fileStream = new FileStream(filePath, FileMode.Append))
            {
                // Convert string to byte array for writing
                byte[] data = Encoding.UTF8.GetBytes(content);
                
                // Write data to file
                fileStream.Write(data, 0, data.Length);
                
                // Ensure all data is written to disk (even if buffer isn't full)
                fileStream.Flush();
                
                Console.WriteLine($"✓ Written {data.Length} bytes to file");
                Console.WriteLine($"  Content: {content}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error writing to file: {ex.Message}");
        }
    }
    
    // Demo reading from file using FileStream
    static void DemoFileReading(string filePath)
    {
        Console.WriteLine("\nFile Reading with FileStream:");
        
        try
        {
            string readContent;
            
            // Open file for reading only
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Using StreamReader for easier text reading
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    readContent = reader.ReadToEnd();
                }
            }
            
            Console.WriteLine($"✓ Read content: {readContent}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("❌ File not found for reading");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error reading file: {ex.Message}");
        }
    }
    
    // Demo different FileMode, FileAccess, and FileShare options
    static void DemoFileModesAndAccess()
    {
        Console.WriteLine("\nFileMode, FileAccess & FileShare Demo:");
        
        string testFile = "modes_demo.txt";
        
        try
        {
            // FileMode.CreateNew - Fails if file exists
            Console.WriteLine("Testing FileMode options:");
            
            using (var fs1 = new FileStream(testFile, FileMode.CreateNew))
            {
                byte[] data = Encoding.UTF8.GetBytes("CreateNew mode test");
                fs1.Write(data, 0, data.Length);
                Console.WriteLine("✓ FileMode.CreateNew - Created new file");
            }
            
            // FileMode.Open - Opens existing file
            using (var fs2 = new FileStream(testFile, FileMode.Open, FileAccess.Read))
            {
                Console.WriteLine("✓ FileMode.Open - Opened existing file for reading");
            }
            
            // FileMode.Append - Opens for writing at end
            using (var fs3 = new FileStream(testFile, FileMode.Append))
            {
                byte[] appendData = Encoding.UTF8.GetBytes(" + appended text");
                fs3.Write(appendData, 0, appendData.Length);
                Console.WriteLine("✓ FileMode.Append - Appended data to file");
            }
            
            // FileMode.Truncate - Opens existing file and truncates to 0 bytes
            using (var fs4 = new FileStream(testFile, FileMode.Truncate))
            {
                Console.WriteLine("✓ FileMode.Truncate - Truncated file to 0 bytes");
            }
            
            // Clean up
            File.Delete(testFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in FileMode demo: {ex.Message}");
        }
    }
    
    // StreamWriter and StreamReader demo - Higher level text operations
    static void RunStreamWriterReaderDemo()
    {
        Console.WriteLine("2. StreamWriter & StreamReader Demo");
        Console.WriteLine("===================================");
        
        DemoStreamWriter();
        DemoStreamReader();
        DemoInteractiveWriting();
        DemoVariableDataSaving();
        
        Console.WriteLine();
    }
    
    // Demo StreamWriter for text writing
    static void DemoStreamWriter()
    {
        Console.WriteLine("StreamWriter Text Writing:");
        
        string filePath = "streamwriter_demo.txt";
        
        try
        {
            // StreamWriter makes text writing much easier than FileStream
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("First line using StreamWriter");
                writer.WriteLine("Second line with special characters: àáâãäå");
                writer.Write("Third line without newline");
                writer.Write(" - continued on same line");
                
                // AutoFlush property controls automatic flushing
                writer.AutoFlush = true;
                writer.WriteLine("\nFourth line with AutoFlush enabled");
                
                Console.WriteLine("✓ Written multiple lines using StreamWriter");
                Console.WriteLine($"  AutoFlush: {writer.AutoFlush}");
                Console.WriteLine($"  Encoding: {writer.Encoding}");
            }
            
            // Clean up
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ StreamWriter error: {ex.Message}");
        }
    }
    
    // Demo StreamReader for text reading
    static void DemoStreamReader()
    {
        Console.WriteLine("\nStreamReader Text Reading:");
        
        string filePath = "streamreader_demo.txt";
        
        try
        {
            // First, create some test content
            File.WriteAllLines(filePath, new[] 
            {
                "Line 1: StreamReader demo",
                "Line 2: Reading line by line",
                "Line 3: Last line of test file"
            });
            
            // Read using StreamReader
            using (StreamReader reader = new StreamReader(filePath))
            {
                Console.WriteLine($"✓ File properties:");
                Console.WriteLine($"  Encoding: {reader.CurrentEncoding}");
                Console.WriteLine($"  End of Stream: {reader.EndOfStream}");
                
                // Seek to beginning (though files start at beginning anyway)
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                  // Read line by line
                string? line;
                int lineNumber = 1;
                
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"  Line {lineNumber}: {line}");
                    lineNumber++;
                }
                
                Console.WriteLine($"✓ Finished reading - End of Stream: {reader.EndOfStream}");
            }
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ StreamReader error: {ex.Message}");
        }
    }
    
    // Demo interactive writing (simulating user input)
    static void DemoInteractiveWriting()
    {
        Console.WriteLine("\nInteractive Writing Demo:");
        
        string filePath = "user_input_demo.txt";
        
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Simulate user input scenarios
                string[] simulatedInputs = 
                {
                    "Hello from simulated user input!",
                    "This demonstrates interactive file writing",
                    "StreamWriter makes text handling easy"
                };
                
                foreach (string input in simulatedInputs)
                {
                    Console.WriteLine($"Simulated input: {input}");
                    writer.WriteLine(input);
                }
                
                // Clear all buffers to ensure data is written
                writer.Flush();
                
                Console.WriteLine("✓ Interactive writing completed");
            }
            
            // Verify content was written
            string content = File.ReadAllText(filePath);
            Console.WriteLine($"✓ File content verification: {content.Split('\n').Length - 1} lines written");
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Interactive writing error: {ex.Message}");
        }
    }
    
    // Demo saving variable data to file
    static void DemoVariableDataSaving()
    {
        Console.WriteLine("\nVariable Data Saving Demo:");
        
        string filePath = "calculation_result.txt";
        
        try
        {
            // Sample calculation variables
            int a = 25;
            int b = 17;
            int sum = a + b;
            double average = sum / 2.0;
            
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("=== Calculation Results ===");
                writer.WriteLine($"First number: {a}");
                writer.WriteLine($"Second number: {b}");
                writer.WriteLine($"Sum: {a} + {b} = {sum}");
                writer.WriteLine($"Average: {average:F2}");
                writer.WriteLine($"Calculation performed at: {DateTime.Now}");
            }
            
            Console.WriteLine("✓ Variable data saved to file:");
            Console.WriteLine($"  Numbers: {a}, {b}");
            Console.WriteLine($"  Result: {sum}");
            Console.WriteLine($"  File: {filePath}");
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Variable data saving error: {ex.Message}");
        }
    }
    
    // File class demo - High level file operations
    static void RunFileClassDemo()
    {
        Console.WriteLine("3. File Class Methods Demo");
        Console.WriteLine("=========================");
        
        DemoFileExistence();
        DemoFileReadWriteOperations();
        DemoFileCopyMoveDelete();
        DemoFileAttributes();
        
        Console.WriteLine();
    }
    
    // Demo File.Exists and basic file operations
    static void DemoFileExistence()
    {
        Console.WriteLine("File Existence and Basic Operations:");
        
        string filePath = "existence_test.txt";
        
        try
        {
            // Check if file exists before creating
            if (File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} already exists - deleting first");
                File.Delete(filePath);
            }
            
            // Create file using File.Create
            using (FileStream fs = File.Create(filePath))
            {
                // File.Create returns FileStream, so we can write immediately
                byte[] data = Encoding.UTF8.GetBytes("File created using File.Create method");
                fs.Write(data, 0, data.Length);
            }
            
            // Now check existence
            if (File.Exists(filePath))
            {
                Console.WriteLine($"✓ File {filePath} created and exists");
                
                // Get file information
                FileInfo info = new FileInfo(filePath);
                Console.WriteLine($"  Size: {info.Length} bytes");
                Console.WriteLine($"  Created: {info.CreationTime}");
                Console.WriteLine($"  Last Modified: {info.LastWriteTime}");
            }
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ File existence demo error: {ex.Message}");
        }
    }
    
    // Demo File class read/write operations
    static void DemoFileReadWriteOperations()
    {
        Console.WriteLine("\nFile Class Read/Write Operations:");
        
        string textFile = "file_operations.txt";
        string binaryFile = "file_operations.bin";
        
        try
        {
            // File.WriteAllText - Simple text writing
            string content = "This is written using File.WriteAllText method";
            File.WriteAllText(textFile, content);
            Console.WriteLine("✓ File.WriteAllText - wrote text content");
            
            // File.ReadAllText - Simple text reading
            string readContent = File.ReadAllText(textFile);
            Console.WriteLine($"✓ File.ReadAllText - read: {readContent}");
            
            // File.WriteAllLines - Array of strings
            string[] lines = 
            {
                "Line 1: File.WriteAllLines demo",
                "Line 2: Each array element becomes a line",
                "Line 3: Very convenient for structured data"
            };
            File.WriteAllLines(textFile, lines);
            Console.WriteLine("✓ File.WriteAllLines - wrote array of lines");
            
            // File.ReadAllLines - Read as array
            string[] readLines = File.ReadAllLines(textFile);
            Console.WriteLine($"✓ File.ReadAllLines - read {readLines.Length} lines:");
            for (int i = 0; i < readLines.Length; i++)
            {
                Console.WriteLine($"    [{i}]: {readLines[i]}");
            }
            
            // File.WriteAllBytes - Binary data
            byte[] binaryData = { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 }; // "Hello World"
            File.WriteAllBytes(binaryFile, binaryData);
            Console.WriteLine("✓ File.WriteAllBytes - wrote binary data");
            
            // File.ReadAllBytes - Read binary data
            byte[] readBinary = File.ReadAllBytes(binaryFile);
            string binaryAsText = Encoding.UTF8.GetString(readBinary);
            Console.WriteLine($"✓ File.ReadAllBytes - read binary as text: {binaryAsText}");
            
            // Clean up
            File.Delete(textFile);
            File.Delete(binaryFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ File operations error: {ex.Message}");
        }
    }
    
    // Demo file copy, move, and delete operations
    static void DemoFileCopyMoveDelete()
    {
        Console.WriteLine("\nFile Copy, Move & Delete Operations:");
        
        string sourceFile = "source_file.txt";
        string copyFile = "copied_file.txt";
        string moveFile = "moved_file.txt";
        
        try
        {
            // Create source file
            File.WriteAllText(sourceFile, "This file will be copied and moved");
            Console.WriteLine($"✓ Created source file: {sourceFile}");
            
            // File.Copy - Copy file to new location
            if (File.Exists(sourceFile))
            {
                File.Copy(sourceFile, copyFile);
                Console.WriteLine($"✓ File.Copy - copied to: {copyFile}");
                
                // Verify copy by reading content
                string copiedContent = File.ReadAllText(copyFile);
                Console.WriteLine($"  Copied content: {copiedContent}");
            }
            
            // File.Move - Move file to new location (essentially rename)
            if (File.Exists(copyFile))
            {
                File.Move(copyFile, moveFile);
                Console.WriteLine($"✓ File.Move - moved to: {moveFile}");
                Console.WriteLine($"  Original copy file exists: {File.Exists(copyFile)}");
                Console.WriteLine($"  Moved file exists: {File.Exists(moveFile)}");
            }
            
            // File.Delete - Remove files
            if (File.Exists(sourceFile))
            {
                File.Delete(sourceFile);
                Console.WriteLine($"✓ File.Delete - deleted: {sourceFile}");
            }
            
            if (File.Exists(moveFile))
            {
                File.Delete(moveFile);
                Console.WriteLine($"✓ File.Delete - deleted: {moveFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ File copy/move/delete error: {ex.Message}");
        }
    }
    
    // Demo file attributes and encryption (where supported)
    static void DemoFileAttributes()
    {
        Console.WriteLine("\nFile Attributes Demo:");
        
        string filePath = "attributes_test.txt";
        
        try
        {
            // Create file for testing
            File.WriteAllText(filePath, "File for testing attributes and encryption");
            
            if (File.Exists(filePath))
            {
                // Get file attributes
                FileAttributes attributes = File.GetAttributes(filePath);
                Console.WriteLine($"✓ File attributes: {attributes}");
                
                // Try to set file as read-only
                File.SetAttributes(filePath, FileAttributes.ReadOnly);
                Console.WriteLine("✓ Set file as ReadOnly");
                
                // Check new attributes
                attributes = File.GetAttributes(filePath);
                Console.WriteLine($"✓ Updated attributes: {attributes}");
                
                // Remove read-only to allow deletion
                File.SetAttributes(filePath, FileAttributes.Normal);
                Console.WriteLine("✓ Restored normal attributes");
                
                // Note: Encryption/Decryption is Windows-specific and requires NTFS
                // File.Encrypt(filePath);  // Would encrypt on supported systems
                // File.Decrypt(filePath);  // Would decrypt on supported systems
                Console.WriteLine("ℹ️  Encryption/Decryption available on Windows NTFS systems");
            }
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ File attributes error: {ex.Message}");
        }
    }
    
    // Compression streams demo
    static void RunCompressionDemo()
    {
        Console.WriteLine("4. Compression Streams Demo");
        Console.WriteLine("===========================");
        
        DemoGZipCompression();
        DemoDeflateCompression();
        DemoBrotliCompression();
        
        Console.WriteLine();
    }
    
    // Demo GZip compression
    static void DemoGZipCompression()
    {
        Console.WriteLine("GZip Compression Demo:");
        
        string originalFile = "original_data.txt";
        string compressedFile = "compressed_data.gz";
        string decompressedFile = "decompressed_data.txt";
        
        try
        {
            // Create original file with repetitive data (compresses well)
            string originalData = string.Join("\n", 
                new string('A', 100),
                new string('B', 100), 
                new string('C', 100),
                "This text will compress well due to repetition");
            
            File.WriteAllText(originalFile, originalData);
            long originalSize = new FileInfo(originalFile).Length;
            
            // Compress using GZipStream
            using (FileStream originalFileStream = File.OpenRead(originalFile))
            using (FileStream compressedFileStream = File.Create(compressedFile))
            using (GZipStream gzipStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
            {
                originalFileStream.CopyTo(gzipStream);
            }
            
            long compressedSize = new FileInfo(compressedFile).Length;
            double compressionRatio = (double)compressedSize / originalSize * 100;
            
            Console.WriteLine($"✓ GZip compression completed");
            Console.WriteLine($"  Original size: {originalSize} bytes");
            Console.WriteLine($"  Compressed size: {compressedSize} bytes");
            Console.WriteLine($"  Compression ratio: {compressionRatio:F1}%");
            
            // Decompress using GZipStream
            using (FileStream compressedFileStream = File.OpenRead(compressedFile))
            using (GZipStream gzipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            using (FileStream decompressedFileStream = File.Create(decompressedFile))
            {
                gzipStream.CopyTo(decompressedFileStream);
            }
            
            // Verify decompression
            string decompressedData = File.ReadAllText(decompressedFile);
            bool isIdentical = originalData == decompressedData;
            Console.WriteLine($"✓ GZip decompression completed");
            Console.WriteLine($"  Data integrity: {(isIdentical ? "Perfect" : "Corrupted")}");
            
            // Clean up
            File.Delete(originalFile);
            File.Delete(compressedFile);
            File.Delete(decompressedFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ GZip compression error: {ex.Message}");
        }
    }
    
    // Demo Deflate compression
    static void DemoDeflateCompression()
    {
        Console.WriteLine("\nDeflate Compression Demo:");
        
        try
        {
            string testData = "Deflate compression provides better compression than GZip but without headers";
            byte[] originalBytes = Encoding.UTF8.GetBytes(testData);
            
            // Compress using DeflateStream
            byte[] compressedBytes;
            using (MemoryStream compressedStream = new MemoryStream())
            using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress))
            {
                deflateStream.Write(originalBytes, 0, originalBytes.Length);
                deflateStream.Close(); // Important: Close to flush final data
                compressedBytes = compressedStream.ToArray();
            }
            
            Console.WriteLine($"✓ Deflate compression completed");
            Console.WriteLine($"  Original size: {originalBytes.Length} bytes");
            Console.WriteLine($"  Compressed size: {compressedBytes.Length} bytes");
            
            // Decompress using DeflateStream
            byte[] decompressedBytes;
            using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
            using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                deflateStream.CopyTo(decompressedStream);
                decompressedBytes = decompressedStream.ToArray();
            }
            
            string decompressedData = Encoding.UTF8.GetString(decompressedBytes);
            bool isIdentical = testData == decompressedData;
            
            Console.WriteLine($"✓ Deflate decompression completed");
            Console.WriteLine($"  Data integrity: {(isIdentical ? "Perfect" : "Corrupted")}");
            Console.WriteLine($"  Decompressed: {decompressedData}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Deflate compression error: {ex.Message}");
        }
    }
    
    // Demo Brotli compression
    static void DemoBrotliCompression()
    {
        Console.WriteLine("\nBrotli Compression Demo:");
        
        try
        {
            string testData = new string('X', 1000) + " Brotli provides excellent compression but is slower";
            byte[] originalBytes = Encoding.UTF8.GetBytes(testData);
            
            // Compress using BrotliStream
            byte[] compressedBytes;
            using (MemoryStream compressedStream = new MemoryStream())
            using (BrotliStream brotliStream = new BrotliStream(compressedStream, CompressionMode.Compress))
            {
                brotliStream.Write(originalBytes, 0, originalBytes.Length);
                brotliStream.Close(); // Important: Close to flush final data
                compressedBytes = compressedStream.ToArray();
            }
            
            double compressionRatio = (double)compressedBytes.Length / originalBytes.Length * 100;
            
            Console.WriteLine($"✓ Brotli compression completed");
            Console.WriteLine($"  Original size: {originalBytes.Length} bytes");
            Console.WriteLine($"  Compressed size: {compressedBytes.Length} bytes");
            Console.WriteLine($"  Compression ratio: {compressionRatio:F1}%");
            
            // Decompress using BrotliStream
            byte[] decompressedBytes;
            using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
            using (BrotliStream brotliStream = new BrotliStream(compressedStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                brotliStream.CopyTo(decompressedStream);
                decompressedBytes = decompressedStream.ToArray();
            }
            
            string decompressedData = Encoding.UTF8.GetString(decompressedBytes);
            bool isIdentical = testData == decompressedData;
            
            Console.WriteLine($"✓ Brotli decompression completed");
            Console.WriteLine($"  Data integrity: {(isIdentical ? "Perfect" : "Corrupted")}");
            Console.WriteLine($"  Note: Brotli is slower but achieves better compression ratios");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Brotli compression error: {ex.Message}");
        }
    }
}
