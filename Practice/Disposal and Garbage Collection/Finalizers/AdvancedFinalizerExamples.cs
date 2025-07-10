using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Finalizers
{
    /// <summary>
    /// Demonstrates the proper Dispose pattern combined with finalizers.
    /// This is the recommended approach when you need both deterministic cleanup (Dispose)
    /// and a safety net for cleanup (finalizer).
    /// 
    /// The pattern follows these principles:
    /// 1. Implement IDisposable for deterministic cleanup
    /// 2. Use finalizer as safety net if Dispose isn't called
    /// 3. Call GC.SuppressFinalize(this) when Dispose is called properly
    /// 4. Use protected virtual Dispose(bool disposing) for the actual cleanup
    /// </summary>
    public class ProperDisposalExample : IDisposable
    {
        private readonly string _name;
        private IntPtr _unmanagedResource;
        private IDisposable? _managedResource;
        private bool _disposed = false;

        public ProperDisposalExample(string name)
        {
            _name = name;
            _unmanagedResource = new IntPtr(54321); // Simulated unmanaged resource
            _managedResource = new MemoryStream(); // Managed resource example
            Console.WriteLine($"  → {_name} created with managed and unmanaged resources");
        }

        /// <summary>
        /// Finalizer that works as a safety net.
        /// This will only run if Dispose() wasn't called properly.
        /// It's a backup mechanism to ensure unmanaged resources get cleaned up.
        /// </summary>
        ~ProperDisposalExample()
        {
            Console.WriteLine($"  🛡️  Safety net finalizer called for {_name}");
            Console.WriteLine($"     This means Dispose() wasn't called properly!");
            
            // Call Dispose with disposing = false
            // This tells Dispose to only clean up unmanaged resources
            Dispose(false);
        }

        /// <summary>
        /// Public Dispose method for deterministic cleanup.
        /// Call this explicitly when you're done with the object.
        /// This follows the standard IDisposable pattern.
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine($"  🧹 Dispose() called for {_name}");
            
            // Call Dispose with disposing = true
            // This tells Dispose it can clean up both managed and unmanaged resources
            Dispose(true);
            
            // Tell the GC it doesn't need to call our finalizer
            // This is important for performance - object can be collected in one GC cycle!
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected virtual dispose method that does the actual cleanup.
        /// This is the heart of the Dispose pattern implementation.
        /// </summary>
        /// <param name="disposing">
        /// True if called from Dispose() method (can safely clean up managed resources)
        /// False if called from finalizer (only clean up unmanaged resources)
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Clean up managed resources
                    // Only do this if called from Dispose(), not from finalizer
                    // When called from finalizer, managed objects might already be finalized
                    Console.WriteLine($"     Cleaning up managed resources for {_name}");
                    _managedResource?.Dispose();
                    _managedResource = null!;
                }

                // Always clean up unmanaged resources
                // This happens whether called from Dispose() or finalizer
                if (_unmanagedResource != IntPtr.Zero)
                {
                    Console.WriteLine($"     Cleaning up unmanaged resources for {_name}");
                    // In real code: close file handles, free unmanaged memory, etc.
                    _unmanagedResource = IntPtr.Zero;
                }

                _disposed = true;
                Console.WriteLine($"     Disposal completed for {_name}");
            }
        }

        /// <summary>
        /// Helper method to check if object has been disposed.
        /// Throws an exception if you try to use a disposed object.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(_name);
        }

        /// <summary>
        /// Example method that uses the object's resources.
        /// Always check if disposed before using resources.
        /// </summary>
        public void DoWork()
        {
            ThrowIfDisposed();
            Console.WriteLine($"     {_name} is doing work with its resources");
        }
    }

    /// <summary>
    /// Demonstrates object resurrection - when a finalizer makes an object reachable again.
    /// This example shows a temp file that tries to delete itself in the finalizer,
    /// but if deletion fails, it resurrects itself by adding to a static collection.
    /// 
    /// Key concepts:
    /// - Resurrection occurs when finalizer makes object reachable again
    /// - Object survives current GC cycle and won't be collected until next cycle
    /// - Useful for error handling and retry mechanisms
    /// </summary>
    public class TempFileRef
    {
        // Static collection to hold objects whose deletion failed
        // This acts as a root reference, keeping failed objects alive
        public static readonly ConcurrentQueue<TempFileRef> FailedDeletions = new ConcurrentQueue<TempFileRef>();
        
        public readonly string FilePath;
        public Exception? DeletionError { get; private set; } // Stores the error if deletion fails

        public TempFileRef(string filePath)
        {
            FilePath = filePath;
            
            // Create a dummy file for demonstration
            try
            {
                File.WriteAllText(FilePath, "Temporary file for finalizer demo");
                Console.WriteLine($"  → Created temp file: {FilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠️  Failed to create temp file: {ex.Message}");
            }
        }

        /// <summary>
        /// Finalizer that attempts to delete the temp file.
        /// If deletion fails, it resurrects the object by adding it to FailedDeletions.
        /// </summary>
        ~TempFileRef()
        {
            Console.WriteLine($"  🗑️  Finalizer trying to delete: {FilePath}");
            
            try
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    Console.WriteLine($"     Successfully deleted: {FilePath}");
                }
                else
                {
                    Console.WriteLine($"     File doesn't exist: {FilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"     ⚡ RESURRECTION! Failed to delete: {FilePath}");
                Console.WriteLine($"     Error: {ex.Message}");
                
                // RESURRECTION: Add this object to static collection
                // This makes the object reachable again, so it won't be collected
                DeletionError = ex;
                FailedDeletions.Enqueue(this);
                
                // Object is now "alive" again because FailedDeletions holds a reference
                // It will survive this GC cycle and remain in memory
            }
        }

        /// <summary>
        /// Static method to process objects that failed to delete.
        /// This could be called periodically to retry failed deletions.
        /// </summary>
        public static void ProcessFailedDeletions()
        {
            while (FailedDeletions.TryDequeue(out var failedFile))
            {
                Console.WriteLine($"  🔄 Processing failed deletion: {failedFile.FilePath}");
                Console.WriteLine($"     Original error: {failedFile.DeletionError?.Message}");
                
                try
                {
                    if (File.Exists(failedFile.FilePath))
                    {
                        File.Delete(failedFile.FilePath);
                        Console.WriteLine($"     ✅ Successfully deleted on retry: {failedFile.FilePath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"     ❌ Retry also failed: {ex.Message}");
                    // Could re-enqueue for another retry, or give up
                }
            }
        }
    }

    /// <summary>
    /// Demonstrates GC.ReRegisterForFinalize for retry scenarios.
    /// This shows how to make a finalizer run multiple times by re-registering the object.
    /// 
    /// Key concepts:
    /// - By default, finalizers only run once per object
    /// - GC.ReRegisterForFinalize allows finalizer to run again in future GC cycles
    /// - Useful for retry mechanisms with limited attempts
    /// - Be careful not to create infinite loops
    /// </summary>
    public class RetryTempFileRef
    {
        public readonly string FilePath;
        private int _deleteAttempt = 0; // Counter for retry attempts
        private const int MaxRetries = 3;

        public RetryTempFileRef(string filePath)
        {
            FilePath = filePath;
            
            // Create a dummy file that we'll make hard to delete
            try
            {
                File.WriteAllText(FilePath, "File that might fail to delete");
                Console.WriteLine($"  → Created retry temp file: {FilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠️  Failed to create file: {ex.Message}");
            }
        }

        /// <summary>
        /// Finalizer that attempts to delete the file with retry logic.
        /// Uses GC.ReRegisterForFinalize to try again if deletion fails.
        /// </summary>
        ~RetryTempFileRef()
        {
            _deleteAttempt++;
            Console.WriteLine($"  🔄 Finalizer attempt #{_deleteAttempt} for: {FilePath}");
            
            try
            {
                if (File.Exists(FilePath))
                {
                    // Simulate occasional failure for demonstration
                    if (_deleteAttempt == 1 && Random.Shared.Next(100) < 70) // 70% chance of failure on first try
                    {
                        throw new IOException("Simulated file access failure");
                    }
                    
                    File.Delete(FilePath);
                    Console.WriteLine($"     ✅ Successfully deleted on attempt #{_deleteAttempt}: {FilePath}");
                }
                else
                {
                    Console.WriteLine($"     File doesn't exist: {FilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"     ❌ Attempt #{_deleteAttempt} failed: {ex.Message}");
                
                if (_deleteAttempt < MaxRetries)
                {
                    Console.WriteLine($"     🔄 Re-registering for finalization (attempt {_deleteAttempt + 1}/{MaxRetries})");
                    
                    // Re-register this object for finalization
                    // This means the finalizer will run again in a future GC cycle
                    GC.ReRegisterForFinalize(this);
                }
                else
                {
                    Console.WriteLine($"     💀 Giving up after {MaxRetries} attempts");
                    // After max retries, we give up and let the object be collected
                    // In real scenarios, you might log this or add to a failure queue
                }
            }
        }
    }

    /// <summary>
    /// Demonstrates finalizer order unpredictability issues.
    /// These classes show why you shouldn't depend on finalizer execution order.
    /// </summary>
    public class FinalizerContainer
    {
        private readonly string _name;
        private readonly List<FinalizerItem> _items = new List<FinalizerItem>();

        public FinalizerContainer(string name)
        {
            _name = name;
            Console.WriteLine($"  → Created container: {_name}");
        }

        public void AddItem(FinalizerItem item)
        {
            _items.Add(item);
            Console.WriteLine($"     Added {item.Name} to {_name}");
        }

        /// <summary>
        /// Container finalizer that tries to access its items.
        /// This is DANGEROUS because the items might already be finalized!
        /// </summary>
        ~FinalizerContainer()
        {
            Console.WriteLine($"  📦 Container {_name} finalizer executing");
            
            try
            {
                // DANGEROUS: Trying to access other finalizable objects
                // These items might already be finalized and in unpredictable state
                Console.WriteLine($"     Container had {_items.Count} items");
                
                // This could crash or behave unpredictably!
                foreach (var item in _items)
                {
                    // Don't do this in real code!
                    Console.WriteLine($"     Item: {item.Name} (this might fail!)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"     ❌ Error accessing items: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Item class to demonstrate finalizer order issues.
    /// </summary>
    public class FinalizerItem
    {
        public string Name { get; }

        public FinalizerItem(string name)
        {
            Name = name;
            Console.WriteLine($"  → Created item: {Name}");
        }

        /// <summary>
        /// Item finalizer - might run before or after container finalizer.
        /// The order is unpredictable!
        /// </summary>
        ~FinalizerItem()
        {
            Console.WriteLine($"  🔧 Item {Name} finalizer executing");
            
            // Simulate cleanup that might make the object unusable
            // If this runs before the container finalizer, accessing this object becomes dangerous
        }
    }
}
