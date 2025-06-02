// ThreadSafetyDemo.cs - Demonstrates thread safety issues and solutions with streams
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

public static class ThreadSafetyDemo
{
    public static async Task RunThreadSafetyDemo()
    {
        Console.WriteLine("=== Thread Safety Demo ===");
        Console.WriteLine("---------------------------");
        
        await DemonstrateUnsafeStreamAccess();
        await DemonstrateSafeStreamAccess();
        await DemonstrateStreamTimeouts();
        await DemonstrateCancellationTokens();
        
        Console.WriteLine();
    }
    
    // Show what happens when multiple threads access the same stream unsafely
    static async Task DemonstrateUnsafeStreamAccess()
    {
        Console.WriteLine("Unsafe Stream Access (potential issues):");
        
        string fileName = "unsafe_test.txt";
        
        try
        {
            using (var fileStream = File.Create(fileName))
            {
                Console.WriteLine("⚠️  Multiple threads writing to same stream without synchronization");
                
                // Create multiple tasks that write to the same stream
                var tasks = new Task[3];
                for (int i = 0; i < tasks.Length; i++)
                {
                    int taskId = i;
                    tasks[i] = Task.Run(async () =>
                    {
                        try
                        {
                            string data = $"Data from task {taskId}\n";
                            byte[] bytes = Encoding.UTF8.GetBytes(data);
                            
                            // This is unsafe - multiple threads writing to same stream
                            await fileStream.WriteAsync(bytes, 0, bytes.Length);
                            Console.WriteLine($"Task {taskId} attempted to write");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Task {taskId} failed: {ex.Message}");
                        }
                    });
                }
                
                await Task.WhenAll(tasks);
            }
            
            // Check what actually got written
            string result = await File.ReadAllTextAsync(fileName);
            Console.WriteLine($"Result may be corrupted: {result.Replace('\n', ' ')}");
            
            File.Delete(fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unsafe access demo error: {ex.Message}");
        }
    }
    
    // Show how to safely access streams from multiple threads
    static async Task DemonstrateSafeStreamAccess()
    {
        Console.WriteLine("\nSafe Stream Access (with synchronization):");
        
        string fileName = "safe_test.txt";
        
        try
        {
            using (var fileStream = File.Create(fileName))
            {
                // Wrap the stream in a thread-safe wrapper
                var synchronizedStream = Stream.Synchronized(fileStream);
                Console.WriteLine("✓ Created thread-safe wrapper using Stream.Synchronized()");
                
                var tasks = new Task[3];
                for (int i = 0; i < tasks.Length; i++)
                {
                    int taskId = i;
                    tasks[i] = Task.Run(async () =>
                    {
                        string data = $"Safe data from task {taskId}\n";
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        
                        // Now this is safe - synchronized wrapper ensures thread safety
                        await synchronizedStream.WriteAsync(bytes, 0, bytes.Length);
                        Console.WriteLine($"✓ Task {taskId} safely wrote data");
                    });
                }
                
                await Task.WhenAll(tasks);
            }
            
            string result = await File.ReadAllTextAsync(fileName);
            Console.WriteLine($"✓ Safe result: {result.Replace('\n', ' ')}");
            
            File.Delete(fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Safe access demo error: {ex.Message}");
        }
    }
    
    // Demonstrate timeout capabilities
    static async Task DemonstrateStreamTimeouts()
    {
        Console.WriteLine("\nStream Timeouts:");
        
        try
        {
            // FileStreams don't support timeouts, but we can show the concept
            using (var memoryStream = new MemoryStream())
            {
                Console.WriteLine($"MemoryStream CanTimeout: {memoryStream.CanTimeout}");
                
                if (memoryStream.CanTimeout)
                {
                    memoryStream.ReadTimeout = 1000;  // 1 second
                    memoryStream.WriteTimeout = 1000; // 1 second
                    Console.WriteLine("✓ Set read/write timeouts to 1 second");
                }
                else
                {
                    Console.WriteLine("ℹ️  MemoryStream does not support timeouts");
                }
            }
            
            // Network streams would support timeouts in real scenarios
            Console.WriteLine("ℹ️  NetworkStream would support timeouts for network I/O operations");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Timeout demo error: {ex.Message}");
        }
    }
    
    // Show how to use cancellation tokens with async operations
    static async Task DemonstrateCancellationTokens()
    {
        Console.WriteLine("\nCancellation Tokens (alternative to timeouts):");
        
        try
        {
            using (var cts = new CancellationTokenSource())
            {
                // Set cancellation after 2 seconds
                cts.CancelAfter(TimeSpan.FromSeconds(2));
                
                string fileName = "cancellation_test.txt";
                
                try
                {
                    // Simulate a slow operation that can be cancelled
                    await File.WriteAllTextAsync(fileName, "Test content", cts.Token);
                    
                    // Simulate reading with potential cancellation
                    string content = await File.ReadAllTextAsync(fileName, cts.Token);
                    Console.WriteLine($"✓ Operation completed: {content}");
                    
                    File.Delete(fileName);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("⚠️  Operation was cancelled due to timeout");
                }
            }
            
            // Demonstrate manual cancellation
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel(); // Cancel immediately
                
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes("This won't complete");
                        await ms.WriteAsync(data, 0, data.Length, cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("✓ Demonstrated immediate cancellation");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Cancellation demo error: {ex.Message}");
        }
    }
}
