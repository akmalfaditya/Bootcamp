// AdvancedStreamDemo.cs - Demonstrates more advanced stream concepts
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

public static class AdvancedStreamDemo
{
    public static async Task RunAdvancedStreamDemos()
    {
        Console.WriteLine("=== Advanced Stream Concepts ===");
        Console.WriteLine("---------------------------------");
        
        await DemonstrateStreamComposition();
        await DemonstrateCustomStream();
        await DemonstrateStreamPerformance();
        
        Console.WriteLine();
    }
    
    // Show how decorator streams can be composed together
    static async Task DemonstrateStreamComposition()
    {
        Console.WriteLine("Stream Composition (Decorator Pattern):");
        
        string fileName = "composed_stream.gz";
        string originalText = "This text will be compressed and buffered for optimal performance!";
        
        try
        {
            // Writing: FileStream -> GZipStream -> BufferedStream
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
            using (var bufferedStream = new BufferedStream(gzipStream))
            {
                byte[] data = Encoding.UTF8.GetBytes(originalText);
                await bufferedStream.WriteAsync(data, 0, data.Length);
                Console.WriteLine("✓ Written data through: FileStream -> GZipStream -> BufferedStream");
            }
            
            // Reading: FileStream -> GZipStream -> BufferedStream
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var bufferedStream = new BufferedStream(gzipStream))
            {
                using (var reader = new StreamReader(bufferedStream))
                {
                    string decompressed = await reader.ReadToEndAsync();
                    Console.WriteLine($"✓ Read back: {decompressed}");
                    Console.WriteLine($"✓ Matches original: {decompressed == originalText}");
                }
            }
            
            // Show file size difference
            var originalSize = Encoding.UTF8.GetBytes(originalText).Length;
            var compressedSize = new FileInfo(fileName).Length;
            Console.WriteLine($"✓ Original size: {originalSize} bytes, Compressed: {compressedSize} bytes");
            
            File.Delete(fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Stream composition error: {ex.Message}");
        }
    }
    
    // Create a simple custom stream for demonstration
    static async Task DemonstrateCustomStream()
    {
        Console.WriteLine("\nCustom Stream Implementation:");
        
        try
        {
            using (var customStream = new LoggingStream(new MemoryStream()))
            {
                Console.WriteLine("✓ Created custom LoggingStream wrapper");
                
                byte[] data = Encoding.UTF8.GetBytes("Testing custom stream");
                await customStream.WriteAsync(data, 0, data.Length);
                
                customStream.Position = 0;
                byte[] readBuffer = new byte[data.Length];
                int bytesRead = await customStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                
                string result = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                Console.WriteLine($"✓ Custom stream result: {result}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Custom stream error: {ex.Message}");
        }
    }
    
    // Compare performance of different stream approaches
    static async Task DemonstrateStreamPerformance()
    {
        Console.WriteLine("\nStream Performance Comparison:");
        
        try
        {
            const int iterations = 1000;
            byte[] testData = Encoding.UTF8.GetBytes("Performance test data");
            
            // Test 1: Direct FileStream
            string file1 = "perf_direct.tmp";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            using (var fs = new FileStream(file1, FileMode.Create))
            {
                for (int i = 0; i < iterations; i++)
                {
                    await fs.WriteAsync(testData, 0, testData.Length);
                }
            }
            
            stopwatch.Stop();
            long directTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"✓ Direct FileStream: {directTime}ms for {iterations} writes");
            
            // Test 2: Buffered FileStream
            string file2 = "perf_buffered.tmp";
            stopwatch.Restart();
            
            using (var fs = new FileStream(file2, FileMode.Create))
            using (var buffered = new BufferedStream(fs))
            {
                for (int i = 0; i < iterations; i++)
                {
                    await buffered.WriteAsync(testData, 0, testData.Length);
                }
            }
            
            stopwatch.Stop();
            long bufferedTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"✓ Buffered FileStream: {bufferedTime}ms for {iterations} writes");
            
            // Calculate improvement
            if (bufferedTime > 0)
            {
                double improvement = ((double)(directTime - bufferedTime) / bufferedTime) * 100;
                Console.WriteLine($"✓ Performance improvement: {improvement:F1}%");
            }
            
            // Clean up
            File.Delete(file1);
            File.Delete(file2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Performance demo error: {ex.Message}");
        }
    }
}

// Custom stream that logs all operations
public class LoggingStream : Stream
{
    private readonly Stream _baseStream;
    
    public LoggingStream(Stream baseStream)
    {
        _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
    }
    
    public override bool CanRead => _baseStream.CanRead;
    public override bool CanSeek => _baseStream.CanSeek;
    public override bool CanWrite => _baseStream.CanWrite;
    public override long Length => _baseStream.Length;
    
    public override long Position
    {
        get => _baseStream.Position;
        set => _baseStream.Position = value;
    }
    
    public override void Flush()
    {
        Console.WriteLine("  [LoggingStream] Flush called");
        _baseStream.Flush();
    }
    
    public override int Read(byte[] buffer, int offset, int count)
    {
        int bytesRead = _baseStream.Read(buffer, offset, count);
        Console.WriteLine($"  [LoggingStream] Read {bytesRead} bytes");
        return bytesRead;
    }
    
    public override long Seek(long offset, SeekOrigin origin)
    {
        long newPosition = _baseStream.Seek(offset, origin);
        Console.WriteLine($"  [LoggingStream] Seek to position {newPosition}");
        return newPosition;
    }
    
    public override void SetLength(long value)
    {
        Console.WriteLine($"  [LoggingStream] SetLength to {value}");
        _baseStream.SetLength(value);
    }
    
    public override void Write(byte[] buffer, int offset, int count)
    {
        Console.WriteLine($"  [LoggingStream] Write {count} bytes");
        _baseStream.Write(buffer, offset, count);
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Console.WriteLine("  [LoggingStream] Disposing");
            _baseStream?.Dispose();
        }
        base.Dispose(disposing);
    }
}
