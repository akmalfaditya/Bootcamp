using System;

namespace Finalizers
{
    /// <summary>
    /// Advanced example showing how finalizers work together with the Dispose pattern.
    /// This is the recommended approach when you need both deterministic cleanup (Dispose)
    /// and a safety net for cleanup (finalizer).
    /// </summary>
    public class AdvancedFinalizerExample : IDisposable
    {
        private string _name;
        private IntPtr _unmanagedResource;
        private byte[] _managedResource;
        private bool _disposed = false;

        public AdvancedFinalizerExample(string name)
        {
            _name = name;
            _unmanagedResource = new IntPtr(54321); // Simulated unmanaged resource
            _managedResource = new byte[5000]; // Managed resource
            Console.WriteLine($"  → {_name} created with managed and unmanaged resources");
        }

        /// <summary>
        /// Finalizer that works as a safety net.
        /// This will only run if Dispose() wasn't called properly.
        /// It's a backup mechanism to ensure unmanaged resources get cleaned up.
        /// </summary>
        ~AdvancedFinalizerExample()
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
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine($"  🧹 Dispose() called for {_name}");
            
            // Call Dispose with disposing = true
            // This tells Dispose it can clean up both managed and unmanaged resources
            Dispose(true);
            
            // Tell the GC it doesn't need to call our finalizer
            // This is important for performance!
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected virtual dispose method that does the actual cleanup.
        /// This is the standard Dispose pattern implementation.
        /// </summary>
        /// <param name="disposing">
        /// True if called from Dispose() method (can clean up managed resources)
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
                    Console.WriteLine($"     Cleaning up managed resources for {_name}");
                    _managedResource = null!;
                }

                // Always clean up unmanaged resources
                // This happens whether called from Dispose() or finalizer
                if (_unmanagedResource != IntPtr.Zero)
                {
                    Console.WriteLine($"     Cleaning up unmanaged resources for {_name}");
                    _unmanagedResource = IntPtr.Zero;
                }

                _disposed = true;
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
        /// Shows how to check for disposal before using resources.
        /// </summary>
        public void DoWork()
        {
            ThrowIfDisposed();
            Console.WriteLine($"{_name} is working with its resources");
        }

        /// <summary>
        /// Creates multiple objects to demonstrate different disposal scenarios.
        /// </summary>
        public static void DemonstrateDisposalPatterns()
        {
            Console.WriteLine("Demonstrating proper disposal vs finalizer fallback:");

            // Scenario 1: Proper disposal using 'using' statement
            Console.WriteLine("\nScenario 1: Proper disposal with 'using'");
            using (var properlyDisposed = new AdvancedFinalizerExample("ProperlyDisposed"))
            {
                properlyDisposed.DoWork();
                // Dispose() is called automatically when leaving the using block
            }

            // Scenario 2: Manual disposal
            Console.WriteLine("\nScenario 2: Manual disposal");
            var manuallyDisposed = new AdvancedFinalizerExample("ManuallyDisposed");
            manuallyDisposed.DoWork();
            manuallyDisposed.Dispose(); // Explicitly call Dispose

            // Scenario 3: Forgotten disposal (finalizer will run)
            Console.WriteLine("\nScenario 3: Forgotten disposal (bad practice)");
            var forgottenDisposal = new AdvancedFinalizerExample("ForgottenDisposal");
            forgottenDisposal.DoWork();
            forgottenDisposal = null!; // Just remove reference without disposing

            Console.WriteLine("\nForcing GC to show finalizer behavior...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("\nNotice:");
            Console.WriteLine("- Objects 1 & 2: No finalizer ran (good!)");
            Console.WriteLine("- Object 3: Finalizer ran as safety net (not ideal)");
            Console.WriteLine("Always call Dispose() to avoid finalizer overhead!");
        }
    }

    /// <summary>
    /// Example showing potential issues with finalizer order and dependencies.
    /// This demonstrates why you shouldn't reference other finalizable objects in finalizers.
    /// </summary>
    public class FinalizerOrderExample
    {
        private string _name;
        private FinalizerOrderExample? _dependency;

        public FinalizerOrderExample(string name, FinalizerOrderExample? dependency = null)
        {
            _name = name;
            _dependency = dependency;
            Console.WriteLine($"  → Created {_name}" + (dependency != null ? $" (depends on {dependency._name})" : ""));
        }

        /// <summary>
        /// Finalizer that demonstrates the unpredictable order of finalization.
        /// This shows why you shouldn't access other finalizable objects in finalizers.
        /// </summary>
        ~FinalizerOrderExample()
        {
            Console.WriteLine($"  🔄 Finalizer called for {_name}");

            // This is problematic! The dependency might already be finalized
            if (_dependency != null)
            {
                Console.WriteLine($"     {_name} trying to access dependency {_dependency._name}");
                // In real scenarios, _dependency might be in an unpredictable state
                // This is why you should avoid accessing other objects in finalizers
            }
        }

        /// <summary>
        /// Demonstrates the unpredictable nature of finalizer execution order.
        /// </summary>
        public static void DemonstrateFinalizationOrder()
        {
            Console.WriteLine("Creating objects with dependencies to show finalizer order issues:");

            var parent = new FinalizerOrderExample("Parent");
            var child = new FinalizerOrderExample("Child", parent);
            var grandchild = new FinalizerOrderExample("Grandchild", child);

            // Clear references
            grandchild = null!;
            child = null!;
            parent = null!;

            Console.WriteLine("All references cleared - forcing GC...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("Notice: Finalizer order is unpredictable!");
            Console.WriteLine("Dependencies might be finalized before objects that need them.");
            Console.WriteLine("This is why finalizers should only clean up unmanaged resources.");
        }
    }
}
