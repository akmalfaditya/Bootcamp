# File Handling Project 

## **Training Objective**
Learn advanced file handling techniques in C#. This project elevates your skills from basic file operations to production-level file management, including compression, advanced FileStream concepts, and real-world scenarios that you'll encounter in professional development.

## 📚 **What You'll Learn**

### **Advanced FileStream Mastery**
Think of FileStream as your **Swiss Army knife** for file operations. While File.ReadAllText() is convenient, FileStream gives you **surgical precision** and **complete control**.

```csharp
// FileStream constructor - every parameter matters
new FileStream(
    path: "data.bin",           // Which file?
    mode: FileMode.OpenOrCreate, // What to do if file exists/doesn't exist?
    access: FileAccess.ReadWrite, // What operations are allowed?
    share: FileShare.Read        // Can other processes access this file?
);
```

### **File Modes - Your File Strategy**

#### **Understanding Each Mode**
```csharp
// FileMode.Create - "Start fresh, overwrite everything"
using (var fs = new FileStream("log.txt", FileMode.Create))
{
    // File is created new, existing content is lost
    // Perfect for: New reports, fresh data exports
}

// FileMode.CreateNew - "Only if it doesn't exist"
try
{
    using (var fs = new FileStream("unique.txt", FileMode.CreateNew))
    {
        // Throws exception if file already exists
        // Perfect for: Preventing accidental overwrites
    }
}
catch (IOException)
{
    Console.WriteLine("File already exists - safety first!");
}

// FileMode.Append - "Add to the end"
using (var fs = new FileStream("application.log", FileMode.Append))
{
    // Always writes to the end, perfect for logging
    // Position automatically set to end of file
}

// FileMode.Open - "File must exist"
using (var fs = new FileStream("config.xml", FileMode.Open))
{
    // Throws exception if file doesn't exist
    // Perfect for: Reading configuration files
}
```

### **File Access Control - Security and Performance**

#### **FileAccess - What Can You Do?**
```csharp
// Read-only access - Safe for shared files
using (var fs = new FileStream("shared_data.txt", FileMode.Open, FileAccess.Read))
{
    // Can only read, can't modify
    // Multiple processes can read simultaneously
}

// Write-only access - Perfect for logs
using (var fs = new FileStream("audit.log", FileMode.Append, FileAccess.Write))
{
    // Can only write, can't read back
    // Slight performance benefit
}

// ReadWrite - Full control
using (var fs = new FileStream("database.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite))
{
    // Can read and write, modify in place
    // Most flexible but requires careful handling
}
```

#### **FileShare - Multi-Process Coordination**
```csharp
// FileShare.None - "This file is mine!"
using (var fs = new FileStream("exclusive.dat", FileMode.Create, FileAccess.ReadWrite, FileShare.None))
{
    // No other process can access this file
    // Perfect for: Critical operations, database files
}

// FileShare.Read - "Others can look, but not touch"
using (var fs = new FileStream("report.pdf", FileMode.Open, FileAccess.Write, FileShare.Read))
{
    // Other processes can read while you write
    // Perfect for: Reports being generated while users view them
}

// FileShare.ReadWrite - "Everyone can access"
using (var fs = new FileStream("config.ini", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
{
    // Multiple processes can read and write
    // Requires careful coordination to avoid corruption
}
```

### **File Class - Your High-Level Assistant**

Think of the File class as your **personal assistant** for common file operations:

```csharp
// Quick text operations - One line of code!
string content = File.ReadAllText("document.txt");              // Read entire file
File.WriteAllText("output.txt", "Hello World");                // Create/overwrite with text
File.AppendAllText("log.txt", $"{DateTime.Now}: Event occurred\n"); // Add to end

// Line-based operations - Perfect for structured data
string[] lines = File.ReadAllLines("data.csv");                // Each line as array element
File.WriteAllLines("output.csv", new[] { "Name,Age", "John,25" }); // Write array as lines

// Binary operations - For any file type
byte[] fileBytes = File.ReadAllBytes("image.jpg");             // Entire file as byte array
File.WriteAllBytes("copy.jpg", fileBytes);                     // Write byte array to file

// File management operations
File.Copy("source.txt", "backup.txt");                         // Create copy
File.Move("old_name.txt", "new_name.txt");                    // Rename/move file
File.Delete("temporary.tmp");                                  // Remove file
bool exists = File.Exists("config.xml");                      // Check if file exists
```

### **Compression Streams - Space-Saving Techniques**

#### **GZip Compression - The Standard Choice**
```csharp
// Compressing data - Making files smaller
string originalText = "This text will be compressed to save space!";
byte[] originalBytes = Encoding.UTF8.GetBytes(originalText);

using (FileStream fs = new FileStream("compressed.gz", FileMode.Create))
using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress))
{
    gzipStream.Write(originalBytes, 0, originalBytes.Length);
    // File is automatically compressed!
}

// Decompressing data - Getting your data back
using (FileStream fs = new FileStream("compressed.gz", FileMode.Open))
using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress))
using (StreamReader reader = new StreamReader(gzipStream))
{
    string decompressedText = reader.ReadToEnd();
    // Original text is restored!
}
```

#### **Deflate vs Brotli - Choosing the Right Algorithm**
```csharp
// Deflate - Faster compression, smaller headers
using (var deflateStream = new DeflateStream(outputStream, CompressionMode.Compress))
{
    // Good balance of speed and compression ratio
    // Perfect for: Network data, temporary compression
}

// Brotli - Better compression, slower processing
using (var brotliStream = new BrotliStream(outputStream, CompressionMode.Compress))
{
    // Best compression ratio, but uses more CPU
    // Perfect for: Long-term storage, web content
}
```

### **Real-World Scenarios**

#### **Application Logging - Production Pattern**
```csharp
public class Logger
{
    private readonly string logPath;
    
    public void WriteLog(string level, string message)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
        
        // Thread-safe append operation
        lock (this)
        {
            File.AppendAllText(logPath, logEntry + Environment.NewLine);
        }
        
        // Rotate log if it gets too large
        if (new FileInfo(logPath).Length > 10_000_000) // 10MB
        {
            RotateLogFile();
        }
    }
}
```

#### **CSV File Processing - Data Import/Export**
```csharp
public class CsvProcessor
{
    public void ExportToCsv<T>(IEnumerable<T> data, string filePath)
    {
        using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            // Write header
            var properties = typeof(T).GetProperties();
            writer.WriteLine(string.Join(",", properties.Select(p => p.Name)));
            
            // Write data rows
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? "");
                writer.WriteLine(string.Join(",", values));
            }
        }
    }
}
```

#### **Configuration File Management**
```csharp
public class ConfigManager
{
    public void SaveConfig(Dictionary<string, object> settings, string configPath)
    {
        try
        {
            // Use JSON for human-readable configuration
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            // Atomic write - write to temp file, then replace
            string tempPath = configPath + ".tmp";
            File.WriteAllText(tempPath, json);
            File.Move(tempPath, configPath); // Atomic operation
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save configuration: {ex.Message}", ex);
        }
    }
}
```

#### **File Backup and Restore**
```csharp
public class BackupManager
{
    public void CreateBackup(string sourceDir, string backupPath)
    {
        using (FileStream fs = new FileStream(backupPath, FileMode.Create))
        using (GZipStream gzip = new GZipStream(fs, CompressionMode.Compress))
        using (var archive = new TarArchive(gzip))
        {
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceDir, file);
                archive.AddFile(file, relativePath);
            }
        }
    }
}
```

## **Project Structure**

### **Program.cs** - Your Learning Laboratory
**Core Demonstrations:**
- **FileStream fundamentals** with all constructor combinations
- **StreamWriter/Reader** for text processing
- **File class methods** for quick operations
- **Compression streams** with performance comparisons

### **FileStreamAdvancedDemo.cs** - Expert Techniques
**Advanced Concepts:**
- **FileShare modes** and multi-process scenarios
- **Stream positioning** and seeking operations
- **Error handling patterns** for robust applications
- **Performance optimization** techniques

### **PracticalExamplesDemo.cs** - Real-World Applications
**Production Scenarios:**
- **Logging systems** with rotation and thread safety
- **CSV processing** for data import/export
- **Configuration management** with atomic updates
- **Backup/restore operations** with compression

## **How to Run This Training**

```powershell
# Navigate to the project directory
cd "File Handling"

# Run the comprehensive demonstration
dotnet run

# Build and check for compilation issues
dotnet build

# Run specific demos (modify Program.cs to enable)
dotnet run --configuration Release  # For performance testing
```

### **"When should I use FileStream vs File class methods?"**

**Use FileStream when:**
- Processing large files (> 100MB)
- Need precise control over file access
- Working with binary data
- Implementing streaming operations
- Multiple read/write operations on same file

**Use File class when:**
- Simple read/write operations
- Small to medium files (< 50MB)
- One-time operations
- Rapid prototyping
- Human-readable text files

### **"How do I prevent file corruption in multi-process scenarios?"**

```csharp
// DANGEROUS - Race condition possible
if (File.Exists("counter.txt"))
{
    int count = int.Parse(File.ReadAllText("counter.txt"));
    File.WriteAllText("counter.txt", (count + 1).ToString());
}

// SAFE - Exclusive access with proper locking
using (var fs = new FileStream("counter.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
using (var reader = new StreamReader(fs))
using (var writer = new StreamWriter(fs))
{
    fs.Seek(0, SeekOrigin.Begin);
    string content = reader.ReadToEnd();
    int count = string.IsNullOrEmpty(content) ? 0 : int.Parse(content);
    
    fs.SetLength(0); // Clear file
    writer.Write((count + 1).ToString());
    writer.Flush();
}
```

### **"How do I handle large files without running out of memory?"**

```csharp
// MEMORY KILLER - Loads entire file into memory
byte[] allData = File.ReadAllBytes("huge_file.dat"); // Could be 1GB+

// MEMORY EFFICIENT - Process in chunks
using (FileStream fs = new FileStream("huge_file.dat", FileMode.Open))
{
    byte[] buffer = new byte[4096]; // Only 4KB in memory
    int bytesRead;
    
    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
    {
        // Process this chunk
        ProcessChunk(buffer, 0, bytesRead);
    }
}
```

### **"What's the best compression algorithm for my use case?"**

| Algorithm | Speed | Compression Ratio | Best For |
|-----------|-------|-------------------|----------|
| **Deflate** | Fast | Good | Network data, temporary files |
| **GZip** | Medium | Good | Web content, general purpose |
| **Brotli** | Slow | Excellent | Long-term storage, static assets |

## **Performance Tips**

1. **Use BufferedStream** for many small operations
2. **Choose appropriate buffer sizes** (4KB-64KB typically optimal)
3. **Dispose resources properly** to avoid file handle leaks
4. **Use FileShare.Read** when multiple processes need access
5. **Implement file locking** for critical sections
6. **Monitor file system performance** in production
