using System;
using System.IO;
using System.Threading;

namespace Finalizers
{
    /// <summary>
    /// A basic example of finalizer implementation.
    /// This shows the fundamental syntax and behavior of finalizers in C#.
    /// 
    /// Key points about finalizers:
    /// - Declared with the tilde (~) syntax
    /// - Cannot be public, private, protected, or static
    /// - Cannot have parameters
    /// - Cannot call base class finalizers directly
    /// - Called automatically by GC when object is being collected
    /// </summary>
    public class BasicFinalizerExample
    {
        private readonly string _name;
        private readonly byte[] _someData;

        /// <summary>
        /// Constructor that initializes the object with a name and some data.
        /// The data simulates resources that need cleanup.
        /// </summary>
        public BasicFinalizerExample(string name)
        {
            _name = name;
            _someData = new byte[1000]; // Simulate some resource usage
            Console.WriteLine($"  → Created {_name}");
        }

        /// <summary>
        /// Finalizer (destructor) - called by the garbage collector.
        /// This is the ~ClassName() syntax for declaring a finalizer.
        /// 
        /// Important characteristics:
        /// - Runs on a separate finalizer thread
        /// - Execution timing is unpredictable
        /// - Should execute quickly and not block
        /// - Should not throw exceptions
        /// - Should not access other managed objects
        /// </summary>
        ~BasicFinalizerExample()
        {
            // This runs on the finalizer thread, not your main thread
            Console.WriteLine($"  ⚠️  Finalizer called for {_name}");

            // In a real scenario, you'd clean up unmanaged resources here
            // For example: closing file handles, releasing memory, etc.
            
            // Important: Keep finalizer logic simple and fast!
            // The GC waits for this to complete before continuing
        }

        /// <summary>
        /// A simple method to demonstrate the object is functional.
        /// </summary>
        public void DoSomething()
        {
            Console.WriteLine($"{_name} is doing some work...");
        }
    }

    /// <summary>
    /// Demonstrates the multi-phase garbage collection process.
    /// Shows how objects with finalizers are handled differently by the GC.
    /// </summary>
    public class GCPhaseDemo
    {
        private readonly string _name;
        private readonly IntPtr _unmanagedResource;

        public GCPhaseDemo(string name)
        {
            _name = name;
            _unmanagedResource = new IntPtr(42); // Simulate unmanaged resource
            Console.WriteLine($"  → Created {_name}");
        }

        /// <summary>
        /// Finalizer that demonstrates the GC phases.
        /// The object goes through multiple phases:
        /// 1. Marked for collection
        /// 2. Moved to finalization queue
        /// 3. Finalizer executes
        /// 4. Next GC cycle frees memory
        /// </summary>
        ~GCPhaseDemo()
        {
            Console.WriteLine($"  📋 Phase 3: Finalizer executing for {_name}");
            Console.WriteLine($"     Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            
            // Simulate cleanup of unmanaged resource
            if (_unmanagedResource != IntPtr.Zero)
            {
                Console.WriteLine($"     Cleaning up unmanaged resource for {_name}");
                // In real code: release the actual unmanaged resource
            }
        }
    }

    /// <summary>
    /// Demonstrates finalizer timing by tracking when objects are created vs finalized.
    /// This helps illustrate the unpredictable nature of finalizer execution.
    /// </summary>
    public class TimedFinalizerExample
    {
        private readonly string _name;
        private readonly DateTime _createdAt;

        public TimedFinalizerExample(string name)
        {
            _name = name;
            _createdAt = DateTime.Now;
            Console.WriteLine($"  → {_name} created at {_createdAt:HH:mm:ss.fff}");
        }

        /// <summary>
        /// Finalizer that shows timing information.
        /// This demonstrates that finalizers don't run immediately when objects
        /// become unreachable - they run later when GC decides to process them.
        /// </summary>
        ~TimedFinalizerExample()
        {
            var finalizedAt = DateTime.Now;
            var timeDifference = finalizedAt - _createdAt;
            
            Console.WriteLine($"  ⏱️  {_name} finalized at {finalizedAt:HH:mm:ss.fff} " +
                             $"(lived for {timeDifference.TotalMilliseconds:F0} ms)");
        }
    }

    /// <summary>
    /// Example of a properly implemented finalizer following best practices.
    /// This shows how to write finalizers that are efficient and safe.
    /// </summary>
    public class GoodFinalizerExample
    {
        private readonly string _name;
        private IntPtr _unmanagedResource; // Simulated unmanaged resource
        private bool _disposed = false;

        public GoodFinalizerExample(string name)
        {
            _name = name;
            _unmanagedResource = new IntPtr(12345); // Simulate unmanaged resource
            Console.WriteLine($"  → {_name} created with unmanaged resource");
        }

        /// <summary>
        /// Proper finalizer implementation following best practices:
        /// - Executes quickly
        /// - Handles exceptions properly
        /// - Only cleans up unmanaged resources
        /// - Doesn't access other managed objects
        /// </summary>
        ~GoodFinalizerExample()
        {
            try
            {
                Console.WriteLine($"  ✅ Good finalizer executing for {_name}");
                
                // Check if already cleaned up
                if (_disposed)
                {
                    Console.WriteLine($"     Already cleaned up - skipping");
                    return;
                }

                // Only clean up unmanaged resources
                if (_unmanagedResource != IntPtr.Zero)
                {
                    Console.WriteLine($"     Releasing unmanaged resource for {_name}");
                    _unmanagedResource = IntPtr.Zero;
                }

                _disposed = true;
                Console.WriteLine($"     Finalizer completed successfully for {_name}");
            }
            catch (Exception ex)
            {
                // NEVER let exceptions escape from finalizers!
                // This would crash the entire application
                Console.WriteLine($"     Error in finalizer for {_name}: {ex.Message}");
            }
        }

        /// <summary>
        /// Example method that uses the object's resources.
        /// </summary>
        public void DoSomeWork()
        {
            if (_disposed)
                throw new ObjectDisposedException(_name);
                
            Console.WriteLine($"{_name} is working with unmanaged resource");
        }
    }

    /// <summary>
    /// Example of BAD finalizer implementation.
    /// This demonstrates what NOT to do when writing finalizers.
    /// WARNING: This is for educational purposes only!
    /// </summary>
    public class BadFinalizerExample
    {
        private readonly string _name;
        private static readonly object _lock = new object();

        public BadFinalizerExample(string name)
        {
            _name = name;
            Console.WriteLine($"  → Created {_name} (with BAD finalizer)");
        }

        /// <summary>
        /// BAD finalizer implementation that demonstrates common mistakes.
        /// DO NOT write finalizers like this in real code!
        /// </summary>
        ~BadFinalizerExample()
        {
            Console.WriteLine($"  ❌ BAD finalizer starting for {_name}");
            
            try
            {
                // MISTAKE 1: Taking too long to execute
                Console.WriteLine($"     Simulating slow operation...");
                Thread.Sleep(100); // This is BAD! Don't do this!
                
                // MISTAKE 2: Acquiring locks (potential deadlock)
                lock (_lock)
                {
                    Console.WriteLine($"     Acquired lock in finalizer (BAD!)");
                }
                
                // MISTAKE 3: Complex operations that might fail
                var tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, "Finalizer was here");
                File.Delete(tempFile);
                
                // MISTAKE 4: Accessing other objects (they might be finalized)
                // (We're not doing this here, but it's another common mistake)
                
                Console.WriteLine($"     BAD finalizer completed for {_name}");
            }
            catch (Exception ex)
            {
                // At least we're catching exceptions, but the damage is done
                Console.WriteLine($"     Exception in BAD finalizer: {ex.Message}");
            }
        }
    }
}
