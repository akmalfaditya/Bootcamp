using System;

namespace Classes
{
    /// <summary>
    /// Demonstrates finalizers (destructors) - cleanup code that runs before garbage collection
    /// IMPORTANT: In real code, you rarely need finalizers! Use IDisposable pattern instead.
    /// This is just to show how they work conceptually.
    /// </summary>
    public class ResourceManager
    {
        private string resourceName;
        private bool disposed = false;

        /// <summary>
        /// Constructor - simulates acquiring a resource
        /// </summary>
        /// <param name="name">Name of the resource being managed</param>
        public ResourceManager(string name)
        {
            resourceName = name;
            Console.WriteLine($"ResourceManager: Acquired resource '{resourceName}'");
        }

        /// <summary>
        /// This is the finalizer (destructor) - note the ~ symbol
        /// The garbage collector calls this before destroying the object
        /// Finalizers are non-deterministic - you don't know when they'll run!
        /// </summary>
        ~ResourceManager()
        {
            Console.WriteLine($"FINALIZER: Cleaning up resource '{resourceName}'");
            
            // In real scenarios, you'd clean up unmanaged resources here
            // But be careful - finalizers run on a separate thread!
            Cleanup();
        }

        /// <summary>
        /// Method to manually release resources
        /// In practice, implement IDisposable instead of relying on finalizers
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                Console.WriteLine($"DISPOSE: Manually releasing resource '{resourceName}'");
                Cleanup();
                disposed = true;
                
                // Tell GC not to call finalizer since we've already cleaned up
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Common cleanup logic used by both Dispose and finalizer
        /// </summary>
        private void Cleanup()
        {
            if (!disposed)
            {
                // Simulate releasing the resource
                Console.WriteLine($"Cleanup: Resource '{resourceName}' has been released");
                disposed = true;
            }
        }

        /// <summary>
        /// Method to use the resource
        /// </summary>
        public void UseResource()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(ResourceManager));
            
            Console.WriteLine($"Using resource: {resourceName}");
        }

        /// <summary>
        /// Property to check if resource is still available
        /// </summary>
        public bool IsDisposed => disposed;
    }

    /// <summary>
    /// Better example - class that implements IDisposable pattern
    /// This is the recommended approach for resource management
    /// </summary>
    public class BetterResourceManager : IDisposable
    {
        private string resourceName;
        private bool disposed = false;

        public BetterResourceManager(string name)
        {
            resourceName = name;
            Console.WriteLine($"BetterResourceManager: Acquired resource '{resourceName}'");
        }

        /// <summary>
        /// Public dispose method - implements IDisposable
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method - the actual cleanup logic
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Clean up managed resources here
                    Console.WriteLine($"BetterResourceManager: Disposing managed resources for '{resourceName}'");
                }

                // Clean up unmanaged resources here
                Console.WriteLine($"BetterResourceManager: Cleaning up '{resourceName}'");
                disposed = true;
            }
        }

        /// <summary>
        /// Finalizer as backup - only if Dispose wasn't called
        /// </summary>
        ~BetterResourceManager()
        {
            Console.WriteLine($"FINALIZER: BetterResourceManager finalizing '{resourceName}' (Dispose wasn't called!)");
            Dispose(false);
        }

        public void UseResource()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(BetterResourceManager));
            
            Console.WriteLine($"Using resource: {resourceName}");
        }
    }
}
