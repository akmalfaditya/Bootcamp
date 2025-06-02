using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace DisposalPatternDemo
{
    /// <summary>
    /// This program demonstrates the proper implementation and usage of IDisposable interface,
    /// disposal patterns, and resource management in C#.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== IDisposable and Resource Management Demo ===\n");

            // Demo 1: Basic disposal pattern with FileStream
            Console.WriteLine("1. Basic Disposal Pattern Demo:");
            DemoBasicDisposal();
            Console.WriteLine();

            // Demo 2: Using statement demonstration
            Console.WriteLine("2. Using Statement Demo:");
            DemoUsingStatement();
            Console.WriteLine();

            // Demo 3: Database connection Close vs Dispose
            Console.WriteLine("3. Close vs Dispose Demo:");
            DemoCloseVsDispose();
            Console.WriteLine();

            // Demo 4: Proper disposal with event unsubscription
            Console.WriteLine("4. Event Unsubscription in Disposal:");
            DemoEventUnsubscription();
            Console.WriteLine();

            // Demo 5: Nested disposable objects
            Console.WriteLine("5. Nested Disposable Objects:");
            DemoNestedDisposableObjects();
            Console.WriteLine();

            // Demo 6: What happens when you forget to dispose
            Console.WriteLine("6. Consequences of Not Disposing:");
            DemoWithoutDisposal();
            Console.WriteLine();

            // Demo 7: Advanced disposal patterns with sensitive data
            Console.WriteLine("7. Advanced Disposal with Sensitive Data:");
            DemoAdvancedDisposal();
            Console.WriteLine();

            // Demo 8: Lightweight disposable objects
            Console.WriteLine("8. Lightweight Disposable Objects:");
            DemoLightweightDisposal();
            Console.WriteLine();

            // Demo 9: Async disposal patterns (modern .NET)
            Console.WriteLine("9. Async Disposal Patterns:");
            await DemoAsyncDisposal();
            Console.WriteLine();            Console.WriteLine("=== Demo Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates basic disposal pattern implementation
        /// </summary>
        static void DemoBasicDisposal()
        {
            try
            {
                // Create a temporary file for demonstration
                string tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, "This is sample content for disposal demo");

                // Manual disposal - you need to remember to call Dispose()
                var fileManager = new FileManager(tempFile);
                fileManager.ReadContent();
                fileManager.Dispose(); // Must call explicitly

                Console.WriteLine("✓ File disposed manually");

                // Clean up temp file
                File.Delete(tempFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in basic disposal demo: {ex.Message}");
            }
        }

        /// <summary>
        /// Shows the power of using statement for automatic disposal
        /// </summary>
        static void DemoUsingStatement()
        {
            try
            {
                string tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, "Content for using statement demo");

                // Using statement automatically calls Dispose() at the end of the block
                // Even if an exception occurs, Dispose() is guaranteed to be called
                using (var fileManager = new FileManager(tempFile))
                {
                    fileManager.ReadContent();
                    // Dispose() is called automatically here
                }

                Console.WriteLine("✓ File disposed automatically with using statement");

                // Clean up
                File.Delete(tempFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in using statement demo: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates the difference between Close() and Dispose() methods
        /// </summary>
        static void DemoCloseVsDispose()
        {
            try
            {
                using var connection = new DatabaseConnection("mock_connection_string");
                
                connection.Open();
                connection.ExecuteQuery("SELECT * FROM Users");
                
                // Close allows the connection to be reopened
                connection.Close();
                Console.WriteLine("✓ Connection closed - can be reopened");
                
                connection.Open();
                connection.ExecuteQuery("SELECT * FROM Products");
                
                // Dispose releases all resources permanently
                connection.Dispose();
                Console.WriteLine("✓ Connection disposed - cannot be reopened");
                
                // This would throw an ObjectDisposedException
                // connection.Open(); // Uncommenting this would cause an error
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Close vs Dispose demo: {ex.Message}");
            }
        }

        /// <summary>
        /// Shows proper event unsubscription during disposal
        /// </summary>
        static void DemoEventUnsubscription()
        {
            var publisher = new EventPublisher();
            var subscriber = new EventSubscriber(publisher);
            
            publisher.TriggerEvent();
            
            // Proper disposal unsubscribes from events
            subscriber.Dispose();
            Console.WriteLine("✓ Event unsubscribed during disposal");
            
            // This won't trigger the subscriber since it's been disposed
            publisher.TriggerEvent();
        }

        /// <summary>
        /// Demonstrates disposal of nested disposable objects
        /// </summary>
        static void DemoNestedDisposableObjects()
        {
            try
            {
                string tempFile1 = Path.GetTempFileName();
                string tempFile2 = Path.GetTempFileName();
                
                File.WriteAllText(tempFile1, "Content for first file");
                File.WriteAllText(tempFile2, "Content for second file");

                using (var compositeManager = new CompositeFileManager(tempFile1, tempFile2))
                {
                    compositeManager.ProcessFiles();
                    // All nested resources are disposed automatically
                }
                
                Console.WriteLine("✓ All nested disposable objects disposed properly");
                
                // Clean up
                File.Delete(tempFile1);
                File.Delete(tempFile2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in nested disposal demo: {ex.Message}");
            }
        }        /// <summary>
        /// Shows what happens when you don't dispose resources properly
        /// </summary>
        static void DemoWithoutDisposal()
        {
            Console.WriteLine("Creating objects without proper disposal...");
            var tempFiles = new List<string>();
            
            try
            {
                // This is what NOT to do - creates resources without disposing them
                for (int i = 0; i < 3; i++)
                {
                    string tempFile = Path.GetTempFileName();
                    tempFiles.Add(tempFile);
                    File.WriteAllText(tempFile, $"Temp content {i}");
                    
                    // BAD: Creating FileManager without disposing it
                    var badManager = new FileManager(tempFile);
                    badManager.ReadContent();
                    // No dispose call - this leaks resources!
                }
                
                Console.WriteLine("⚠ Created 3 FileManager instances without disposing them");
                Console.WriteLine("  The file handles are still open, preventing deletion...");
                
                // Try to delete files - this will fail because file handles are still open
                foreach (string tempFile in tempFiles)
                {
                    try
                    {
                        File.Delete(tempFile);
                        Console.WriteLine($"  ✗ Could not delete {Path.GetFileName(tempFile)} - file handle still open!");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"  ✗ IOException: {ex.Message}");
                    }
                }
                
                Console.WriteLine("  💡 This demonstrates why proper disposal is crucial!");
                
                // Force garbage collection to trigger finalizers
                Console.WriteLine("  🧹 Forcing garbage collection to run finalizers...");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect(); // Run GC again to clean up finalized objects
                
                Console.WriteLine("  ✓ Finalizers ran - now files can be deleted");
                
                // Now try to delete again - should work after finalizers ran
                foreach (string tempFile in tempFiles)
                {
                    try
                    {
                        if (File.Exists(tempFile))
                        {
                            File.Delete(tempFile);
                            Console.WriteLine($"  ✓ Successfully deleted {Path.GetFileName(tempFile)} after GC");
                        }
                    }
                    catch (IOException)
                    {
                        Console.WriteLine($"  ⚠ Still couldn't delete {Path.GetFileName(tempFile)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Unexpected error: {ex.Message}");
            }
            finally
            {
                // Final cleanup attempt
                foreach (string tempFile in tempFiles)
                {
                    try
                    {
                        if (File.Exists(tempFile))
                        {
                            File.Delete(tempFile);
                        }
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }

        /// <summary>
        /// Demonstrates advanced disposal patterns including thread safety and sensitive data clearing
        /// </summary>
        static void DemoAdvancedDisposal()
        {
            try
            {
                Console.WriteLine("Creating secure resource manager with sensitive data...");
                
                // Manual disposal of sensitive resource
                var secureManager = new SecureResourceManager("SecureResource-001");
                secureManager.ProcessSensitiveData();
                Console.WriteLine($"Status: {secureManager.GetStatus()}");
                
                // Give the background timer a chance to run
                System.Threading.Thread.Sleep(3000);
                
                secureManager.Dispose();
                Console.WriteLine("✓ Secure resource disposed manually");
                
                Console.WriteLine("\nUsing automatic disposal with using statement...");
                
                // Automatic disposal with using statement
                using (var autoSecureManager = new SecureResourceManager("SecureResource-002"))
                {
                    autoSecureManager.ProcessSensitiveData();
                    Console.WriteLine($"Status: {autoSecureManager.GetStatus()}");
                    
                    // Let background work run for a bit
                    System.Threading.Thread.Sleep(2000);
                    
                    // Disposal happens automatically here
                }
                
                Console.WriteLine("✓ Secure resource disposed automatically");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in advanced disposal demo: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates disposal of lightweight objects that don't have significant resources
        /// </summary>
        static void DemoLightweightDisposal()
        {
            try
            {
                Console.WriteLine("Working with lightweight disposable objects...");
                
                using (var lightObject = new LightweightWrapper("Sample data for lightweight object"))
                {
                    string data = lightObject.GetData();
                    Console.WriteLine($"Retrieved data: {data}");
                    
                    // Even though this object doesn't have significant resources,
                    // it's still good practice to dispose it properly
                }
                
                Console.WriteLine("✓ Lightweight object disposed (no significant cleanup needed)");
                
                // Demonstrate multiple lightweight objects
                Console.WriteLine("\nCreating multiple lightweight objects...");
                
                var lightObjects = new[]
                {
                    new LightweightWrapper("Data 1"),
                    new LightweightWrapper("Data 2"),
                    new LightweightWrapper("Data 3")
                };
                
                foreach (var obj in lightObjects)
                {
                    Console.WriteLine($"Data: {obj.GetData()}");
                    obj.Dispose();
                }
                
                Console.WriteLine("✓ All lightweight objects disposed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in lightweight disposal demo: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates async disposal patterns with modern .NET
        /// </summary>
        static async Task DemoAsyncDisposal()
        {
            try
            {
                Console.WriteLine("Working with async disposable resources...");
                
                // Manual async disposal
                var asyncManager = new AsyncResourceManager("AsyncConnection-001");
                string result = await asyncManager.ProcessDataAsync();
                Console.WriteLine($"Processing result: {result}");
                
                // Properly dispose using async disposal
                await asyncManager.DisposeAsync();
                Console.WriteLine("✓ Async resource disposed manually with DisposeAsync()");
                
                Console.WriteLine("\nUsing 'await using' for automatic async disposal...");
                
                // Automatic async disposal with 'await using'
                await using (var autoAsyncManager = new AsyncResourceManager("AsyncConnection-002"))
                {
                    string autoResult = await autoAsyncManager.ProcessDataAsync();
                    Console.WriteLine($"Auto processing result: {autoResult}");
                    
                    // DisposeAsync() is called automatically here
                }
                
                Console.WriteLine("✓ Async resource disposed automatically with 'await using'");
                
                // Demonstrate fallback to sync disposal
                Console.WriteLine("\nDemonstrating sync disposal fallback...");
                
                using (var syncFallback = new AsyncResourceManager("SyncFallback-003"))
                {
                    string syncResult = await syncFallback.ProcessDataAsync();
                    Console.WriteLine($"Sync fallback result: {syncResult}");
                    
                    // Regular Dispose() is called here (sync disposal)
                }
                
                Console.WriteLine("✓ Async resource disposed with sync fallback");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in async disposal demo: {ex.Message}");
            }
        }
    }
}
