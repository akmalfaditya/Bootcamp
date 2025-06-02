using System;

namespace Finalizers
{
    /// <summary>
    /// A basic example of finalizer implementation.
    /// This shows the fundamental syntax and behavior of finalizers in C#.
    /// </summary>
    public class BasicFinalizerExample
    {
        private string _name;
        private byte[] _someData;

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
        /// Note the tilde (~) syntax - this is how you declare a finalizer.
        /// Key points about finalizers:
        /// - No parameters allowed
        /// - Cannot be public, private, protected, or static
        /// - Cannot call base class finalizers directly
        /// - Called automatically by GC when object is being collected
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
    /// Demonstrates finalizer timing by tracking when objects are created vs finalized.
    /// This helps illustrate the unpredictable nature of finalizer execution.
    /// </summary>
    public class TimedFinalizerExample
    {
        private string _name;
        private DateTime _createdAt;

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
        private string _name;
        private IntPtr _unmanagedResource; // Simulated unmanaged resource
        private bool _disposed = false;

        public GoodFinalizerExample(string name)
        {
            _name = name;
            _unmanagedResource = new IntPtr(12345); // Simulate unmanaged resource
            Console.WriteLine($"  → {_name} created with unmanaged resource");
        }

        /// <summary>
        /// Example of a GOOD finalizer implementation.
        /// This follows all the best practices for finalizer design.
        /// </summary>
        ~GoodFinalizerExample()
        {
            Console.WriteLine($"  ✅ Good finalizer starting for {_name}");

            try
            {
                // Best Practice 1: Keep it simple and fast
                // Only clean up unmanaged resources in finalizer
                if (!_disposed && _unmanagedResource != IntPtr.Zero)
                {
                    // Best Practice 2: Use try-catch to prevent exceptions
                    // from escaping the finalizer
                    Console.WriteLine($"     Cleaning up unmanaged resource for {_name}");
                    _unmanagedResource = IntPtr.Zero;
                    _disposed = true;
                }

                // Best Practice 3: Don't access other managed objects
                // They might already be finalized and in an unpredictable state

                // Best Practice 4: Execute quickly
                // Don't perform complex operations or lengthy tasks
            }
            catch (Exception ex)
            {
                // Best Practice 5: Handle exceptions gracefully
                // Never let exceptions escape from finalizers
                Console.WriteLine($"     Exception in finalizer for {_name}: {ex.Message}");
            }

            Console.WriteLine($"  ✅ Good finalizer completed for {_name}");
        }

        public void DoSomeWork()
        {
            if (_disposed)
                throw new ObjectDisposedException(_name);
                
            Console.WriteLine($"{_name} is working with its resources");
        }
    }

    /// <summary>
    /// Example of a POORLY implemented finalizer that violates best practices.
    /// This demonstrates what NOT to do when writing finalizers.
    /// Never implement finalizers like this in real code!
    /// </summary>
    public class BadFinalizerExample
    {
        private string _name;
        private static int _finalizerCallCount = 0;

        public BadFinalizerExample(string name)
        {
            _name = name;
            Console.WriteLine($"  → {_name} created (this will have a BAD finalizer)");
        }

        /// <summary>
        /// Example of a BAD finalizer implementation.
        /// This violates multiple best practices - DO NOT implement finalizers like this!
        /// </summary>
        ~BadFinalizerExample()
        {
            Console.WriteLine($"  ❌ Bad finalizer starting for {_name}");

            try
            {
                // MISTAKE 1: Taking too long to execute
                // Finalizers should be fast, but this one deliberately delays
                System.Threading.Thread.Sleep(100); // Don't do this!

                // MISTAKE 2: Performing complex operations
                // Finalizers should be simple, not complex calculations
                for (int i = 0; i < 1000; i++)
                {
                    Math.Sqrt(i); // Unnecessary complex work
                }

                // MISTAKE 3: Accessing static/global state unsafely
                // This can cause race conditions in multithreaded scenarios
                _finalizerCallCount++;

                // MISTAKE 4: Potentially throwing exceptions
                // This could happen in real scenarios and disrupt finalization
                if (_finalizerCallCount > 5)
                {
                    // Commented out to prevent actual exceptions in our demo
                    // throw new InvalidOperationException("Finalizer exception!");
                    Console.WriteLine($"     Would throw exception for {_name}");
                }

                // MISTAKE 5: Trying to access other objects
                // Other objects might already be finalized
                Console.WriteLine($"     Accessing potentially finalized objects for {_name}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"     Exception in bad finalizer: {ex.Message}");
                // MISTAKE 6: Not handling exceptions properly
                // In real code, you should never let exceptions escape finalizers
            }

            Console.WriteLine($"  ❌ Bad finalizer completed for {_name}");
        }
    }

    /// <summary>
    /// Simple object without a finalizer for performance comparison.
    /// Shows how objects behave when they don't have finalization overhead.
    /// </summary>
    public class SimpleObject
    {
        private string _name;
        private byte[] _someData;

        public SimpleObject(string name)
        {
            _name = name;
            _someData = new byte[1000]; // Same data size as finalizer example
            // Notice: No finalizer means this object will be collected faster
        }

        public void DoSomething()
        {
            Console.WriteLine($"{_name} is doing work (no finalizer)");
        }

        // No finalizer here - object will be collected in a single GC cycle
    }
}
