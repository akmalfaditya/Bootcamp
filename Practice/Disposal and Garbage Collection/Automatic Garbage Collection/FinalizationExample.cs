using System;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// Demonstrates the relationship between garbage collection and finalization.
    /// Shows how finalizers interact with the GC and why they should be used carefully.
    /// This class implements both IDisposable and finalization to show best practices.
    /// </summary>
    public class FinalizationExample : IDisposable
    {
        private bool _disposed = false;
        private byte[] _managedResource;
        private IntPtr _unmanagedResource; // Simulated unmanaged resource
        
        public string Name { get; private set; }

        /// <summary>
        /// Creates a new instance that holds both managed and simulated unmanaged resources.
        /// </summary>
        /// <param name="name">Name for identification during demonstrations</param>
        public FinalizationExample(string name)
        {
            Name = name;
            _managedResource = new byte[5000]; // Some managed memory
            _unmanagedResource = new IntPtr(12345); // Simulated unmanaged resource
            
            Console.WriteLine($"✓ Created {Name} with managed and unmanaged resources");
        }

        /// <summary>
        /// Finalizer (destructor) - called by the garbage collector.
        /// This demonstrates the finalization process and its overhead.
        /// Note: Objects with finalizers take longer to collect!
        /// </summary>
        ~FinalizationExample()
        {
            Console.WriteLine($"🔧 Finalizer called for {Name}");
            
            // Only clean up unmanaged resources in finalizer
            // Managed resources will be handled by GC
            Dispose(false);
        }

        /// <summary>
        /// Public dispose method implementing IDisposable.
        /// This allows deterministic cleanup without waiting for finalization.
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine($"🧹 Dispose() called for {Name}");
            
            // Clean up both managed and unmanaged resources
            Dispose(true);
            
            // Tell GC not to call finalizer since we've already cleaned up
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected virtual dispose method implementing the dispose pattern.
        /// This is where the actual cleanup logic lives.
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Clean up managed resources
                    Console.WriteLine($"  → Cleaning up managed resources for {Name}");
                    _managedResource = null!; // Let GC handle the array
                }

                // Always clean up unmanaged resources
                Console.WriteLine($"  → Cleaning up unmanaged resources for {Name}");
                _unmanagedResource = IntPtr.Zero;

                _disposed = true;
            }
        }

        /// <summary>
        /// Creates multiple objects with finalizers to demonstrate their impact on GC.
        /// Shows how finalization adds overhead to garbage collection.
        /// </summary>
        /// <param name="count">Number of objects to create</param>
        /// <param name="baseName">Base name for the objects</param>
        public static void CreateFinalizableObjects(int count, string baseName)
        {
            Console.WriteLine($"Creating {count} objects with finalizers...");
            
            for (int i = 0; i < count; i++)
            {
                // Create objects but don't keep references
                // They'll be eligible for collection immediately
                new FinalizationExample($"{baseName}_{i}");
            }
            
            Console.WriteLine($"All {count} objects created and became eligible for collection");
            Console.WriteLine("Note: These objects require two GC cycles to be fully collected!");
            Console.WriteLine("Cycle 1: Object collected, finalizer scheduled");
            Console.WriteLine("Cycle 2: Finalizer runs, object memory fully reclaimed");
        }

        /// <summary>
        /// Demonstrates the difference between objects with and without finalizers.
        /// Shows the performance impact of finalization.
        /// </summary>
        public static void DemoFinalizationImpact()
        {
            Console.WriteLine("=== Finalization Impact Demo ===");
            
            // Show GC state before
            var beforeSnapshot = GCMonitor.TakeSnapshot("Before creating finalizable objects");
            
            // Create objects with finalizers
            CreateFinalizableObjects(100, "Finalizable");
            
            // Force first GC cycle
            Console.WriteLine("\nForcing first GC cycle...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            var afterFirstGC = GCMonitor.TakeSnapshot("After first GC cycle");
            
            // Force second GC cycle to complete finalization
            Console.WriteLine("Forcing second GC cycle to complete finalization...");
            GC.Collect();
            
            var afterSecondGC = GCMonitor.TakeSnapshot("After second GC cycle");
            
            // Show the comparison
            Console.WriteLine("\nImpact analysis:");
            GCMonitor.CompareSnapshots(beforeSnapshot, afterFirstGC);
            Console.WriteLine();
            GCMonitor.CompareSnapshots(afterFirstGC, afterSecondGC);
        }

        /// <summary>
        /// Demonstrates proper disposal vs relying on finalization.
        /// Shows the benefits of calling Dispose() explicitly.
        /// </summary>
        public static void DemoProperDisposal()
        {
            Console.WriteLine("\n=== Proper Disposal Demo ===");
            
            Console.WriteLine("Creating object and disposing properly:");
            using (var properlyDisposed = new FinalizationExample("ProperlyDisposed"))
            {
                // Object will be disposed when using block exits
                Console.WriteLine("Object is being used...");
            } // Dispose() called automatically here
            
            Console.WriteLine("\nCreating object without disposing:");
            var notDisposed = new FinalizationExample("NotDisposed");
            notDisposed = null!; // Remove reference without disposing
            
            Console.WriteLine("Object abandoned without disposal - will rely on finalizer");
            
            // Force GC to show the difference
            Console.WriteLine("\nForcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Console.WriteLine("\nKey insight: Proper disposal avoids finalization overhead!");
        }

        /// <summary>
        /// Checks if the object has been disposed.
        /// Throws an exception if operations are attempted on a disposed object.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// A method that requires the object to be undisposed.
        /// Demonstrates checking disposal state before operations.
        /// </summary>
        public void DoSomething()
        {
            ThrowIfDisposed();
            Console.WriteLine($"{Name} is doing something with its resources");
        }

        public override string ToString()
        {
            return $"FinalizationExample: {Name} (Disposed: {_disposed})";
        }
    }
}
