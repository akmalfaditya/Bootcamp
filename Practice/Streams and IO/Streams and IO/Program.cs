// Comprehensive Streams and I/O Demo
// This project demonstrates various stream types and I/O operations in C#

using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== C# Streams and I/O Comprehensive Demo ===\n");
          // Demo different types of streams and operations
        await RunFileStreamDemo();
        await RunMemoryStreamDemo();
        await RunBufferedStreamDemo();
        await RunTextAdapterDemo();
        await RunBinaryAdapterDemo();
        await RunNamedPipeDemo();
        await RunAnonymousPipeDemo();
        
        // Run additional comprehensive demos
        await FileHelperDemo.RunAllFileHelperDemos();
        await ThreadSafetyDemo.RunThreadSafetyDemo();
        await AdvancedStreamDemo.RunAdvancedStreamDemos();
        
        Console.WriteLine("\n=== All demos completed! ===");
        Console.ReadKey();
    }
    
    // FileStream demo - working with files on disk
    static async Task RunFileStreamDemo()
    {
        Console.WriteLine("1. FileStream Demo");
        Console.WriteLine("-----------------");
        
        string filePath = "demo.txt";
        string content = "Hello from FileStream! This is our test data.";
        
        try
        {
            // Writing to file using FileStream
            using (FileStream fs = File.Create(filePath))
            {
                byte[] data = Encoding.UTF8.GetBytes(content);
                await fs.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"✓ Written {data.Length} bytes to {filePath}");
            }
            
            // Reading from file using FileStream with proper chunk reading
            using (FileStream fs = File.OpenRead(filePath))
            {
                Console.WriteLine($"File size: {fs.Length} bytes");
                Console.WriteLine($"Can read: {fs.CanRead}, Can write: {fs.CanWrite}, Can seek: {fs.CanSeek}");
                
                // Proper way to read data in chunks
                byte[] buffer = new byte[1000];
                int bytesRead = 0;
                int chunkSize = 1;
                
                while (bytesRead < buffer.Length && chunkSize > 0)
                {
                    chunkSize = await fs.ReadAsync(buffer, bytesRead, buffer.Length - bytesRead);
                    bytesRead += chunkSize;
                }
                
                string result = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"✓ Read content: {result}");
                
                // Demonstrate seeking
                fs.Seek(0, SeekOrigin.Begin);
                int firstByte = fs.ReadByte();
                Console.WriteLine($"✓ First byte after seek: {firstByte} ('{(char)firstByte}')");
            }
            
            // Clean up
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in FileStream demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }
    
    // MemoryStream demo - working with data in memory
    static async Task RunMemoryStreamDemo()
    {
        Console.WriteLine("2. MemoryStream Demo");
        Console.WriteLine("-------------------");
        
        try
        {
            using (var ms = new MemoryStream())
            {
                string data = "MemoryStream stores data in RAM, not on disk!";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                
                // Write to memory stream
                await ms.WriteAsync(bytes, 0, bytes.Length);
                Console.WriteLine($"✓ Written {bytes.Length} bytes to MemoryStream");
                
                // Reset position to beginning
                ms.Position = 0;
                
                // Read back from memory stream
                byte[] readBuffer = new byte[ms.Length];
                int bytesRead = await ms.ReadAsync(readBuffer, 0, readBuffer.Length);
                string result = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                
                Console.WriteLine($"✓ Read back: {result}");
                Console.WriteLine($"Stream properties - Position: {ms.Position}, Length: {ms.Length}");
                
                // Demonstrate random access
                ms.Position = 12; // Jump to middle
                int byteAtPosition = ms.ReadByte();
                Console.WriteLine($"✓ Byte at position 12: {byteAtPosition} ('{(char)byteAtPosition}')");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in MemoryStream demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }
    
    // BufferedStream demo - adding buffering to improve performance
    static async Task RunBufferedStreamDemo()
    {
        Console.WriteLine("3. BufferedStream Demo");
        Console.WriteLine("---------------------");
        
        string filePath = "buffered_demo.txt";
        
        try
        {
            // Create a file with some data
            await File.WriteAllTextAsync(filePath, "This is test data for BufferedStream demonstration.");
            
            // Use BufferedStream to wrap FileStream
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var bufferedStream = new BufferedStream(fileStream, bufferSize: 1024))
            {
                Console.WriteLine("✓ Created BufferedStream wrapper around FileStream");
                Console.WriteLine($"Buffer size: 1024 bytes");
                
                // Read using buffered stream - more efficient for multiple small reads
                byte[] buffer = new byte[10];
                int totalBytesRead = 0;
                
                while (true)
                {
                    int bytesRead = await bufferedStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    
                    totalBytesRead += bytesRead;
                    string chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Chunk read: '{chunk}'");
                }
                
                Console.WriteLine($"✓ Total bytes read through buffer: {totalBytesRead}");
            }
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in BufferedStream demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }
    
    // Text adapters demo - StreamReader and StreamWriter
    static async Task RunTextAdapterDemo()
    {
        Console.WriteLine("4. Text Adapters Demo (StreamReader/StreamWriter)");
        Console.WriteLine("------------------------------------------------");
        
        string filePath = "text_demo.txt";
        
        try
        {
            // Writing text using StreamWriter
            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync("First line of text");
                await writer.WriteLineAsync("Second line with special chars: äöü");
                await writer.WriteLineAsync("Third line with numbers: 12345");
                Console.WriteLine("✓ Written multiple lines using StreamWriter");
            }
            
            // Reading text using StreamReader
            using (var reader = new StreamReader(filePath))
            {
                Console.WriteLine("Reading line by line:");
                string line;
                int lineNumber = 1;
                
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    Console.WriteLine($"  Line {lineNumber}: {line}");
                    lineNumber++;
                }
            }
            
            // Demonstrate StringReader/StringWriter for in-memory text operations
            using (var stringWriter = new StringWriter())
            {
                await stringWriter.WriteLineAsync("This is in-memory text");
                await stringWriter.WriteLineAsync("No file system involved!");
                
                string inMemoryText = stringWriter.ToString();
                Console.WriteLine($"✓ StringWriter result: {inMemoryText.Replace('\n', ' ').Replace('\r', ' ')}");
                  using (var stringReader = new StringReader(inMemoryText))
                {
                    string? firstLine = await stringReader.ReadLineAsync();
                    Console.WriteLine($"✓ StringReader first line: {firstLine ?? "null"}");
                }
            }
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in text adapter demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }
      // Binary adapters demo - BinaryReader and BinaryWriter
    static Task RunBinaryAdapterDemo()
    {
        Console.WriteLine("5. Binary Adapters Demo (BinaryReader/BinaryWriter)");
        Console.WriteLine("---------------------------------------------------");
        
        string filePath = "binary_demo.bin";
        
        try
        {
            // Writing binary data
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            using (var writer = new BinaryWriter(fileStream))
            {
                // Write different primitive types
                writer.Write(42);           // int
                writer.Write(3.14159);      // double
                writer.Write(true);         // bool
                writer.Write("Binary text"); // string
                writer.Write((byte)255);    // byte
                
                Console.WriteLine("✓ Written binary data: int, double, bool, string, byte");
            }
            
            // Reading binary data back
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            using (var reader = new BinaryReader(fileStream))
            {
                int intValue = reader.ReadInt32();
                double doubleValue = reader.ReadDouble();
                bool boolValue = reader.ReadBoolean();
                string stringValue = reader.ReadString();
                byte byteValue = reader.ReadByte();
                
                Console.WriteLine($"✓ Read back:");
                Console.WriteLine($"  Int: {intValue}");
                Console.WriteLine($"  Double: {doubleValue:F5}");
                Console.WriteLine($"  Bool: {boolValue}");
                Console.WriteLine($"  String: {stringValue}");
                Console.WriteLine($"  Byte: {byteValue}");
            }
            
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in binary adapter demo: {ex.Message}");
        }
        
        Console.WriteLine();
        return Task.CompletedTask;
    }
    
    // Named pipe demo - inter-process communication
    static async Task RunNamedPipeDemo()
    {
        Console.WriteLine("6. Named Pipe Demo");
        Console.WriteLine("-----------------");
        
        string pipeName = "demo_pipe";
        
        try
        {
            // Start server and client tasks concurrently
            var serverTask = RunPipeServer(pipeName);
            var clientTask = RunPipeClient(pipeName);
            
            await Task.WhenAll(serverTask, clientTask);
            Console.WriteLine("✓ Named pipe communication completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in named pipe demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }
    
    static async Task RunPipeServer(string pipeName)
    {
        using (var server = new NamedPipeServerStream(pipeName))
        {
            Console.WriteLine("Server: Waiting for client connection...");
            await server.WaitForConnectionAsync();
            Console.WriteLine("Server: Client connected!");
            
            // Send data to client
            server.WriteByte(100);
            Console.WriteLine("Server: Sent value 100 to client");
            
            // Receive response from client
            int response = server.ReadByte();
            Console.WriteLine($"Server: Received value {response} from client");
        }
    }
    
    static async Task RunPipeClient(string pipeName)
    {
        // Give server time to start
        await Task.Delay(100);
        
        using (var client = new NamedPipeClientStream(pipeName))
        {
            await client.ConnectAsync();
            Console.WriteLine("Client: Connected to server!");
            
            // Receive data from server
            int value = client.ReadByte();
            Console.WriteLine($"Client: Received value {value} from server");
            
            // Send response back
            client.WriteByte(200);
            Console.WriteLine("Client: Sent value 200 back to server");
        }
    }
    
    // Anonymous pipe demo - parent-child process communication
    static async Task RunAnonymousPipeDemo()
    {
        Console.WriteLine("7. Anonymous Pipe Demo");
        Console.WriteLine("---------------------");
        
        try
        {
            // Create anonymous pipe for one-way communication
            using (var pipeServer = new AnonymousPipeServerStream(PipeDirection.Out))
            {
                Console.WriteLine("Parent: Created anonymous pipe server");
                
                // In a real scenario, you'd pass the handle to a child process
                // For demo purposes, we'll simulate with a task
                string clientHandle = pipeServer.GetClientHandleAsString();
                Console.WriteLine($"Parent: Client handle: {clientHandle}");
                
                // Start "child" process simulation
                var childTask = SimulateChildProcess(clientHandle);
                
                // Parent sends data
                pipeServer.WriteByte(150);
                Console.WriteLine("Parent: Sent value 150 to child");
                
                // Release the local copy of client handle
                pipeServer.DisposeLocalCopyOfClientHandle();
                Console.WriteLine("Parent: Disposed local client handle");
                
                await childTask;
            }
            
            Console.WriteLine("✓ Anonymous pipe communication completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in anonymous pipe demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }
    
    static async Task SimulateChildProcess(string clientHandle)
    {
        await Task.Delay(50); // Simulate process startup time
        
        using (var pipeClient = new AnonymousPipeClientStream(PipeDirection.In, clientHandle))
        {
            Console.WriteLine("Child: Connected to anonymous pipe");
            
            // Receive data from parent
            int value = pipeClient.ReadByte();
            Console.WriteLine($"Child: Received value {value} from parent");
        }
    }
}
