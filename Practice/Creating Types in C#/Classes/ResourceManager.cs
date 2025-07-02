using System;

namespace Classes
{
    /// <summary>
    /// ResourceManager class demonstrating finalizers and IDisposable pattern
    /// Shows how to properly manage unmanaged resources in C#
    /// Think of this as a class that might hold file handles, database connections, etc.
    /// </summary>
    public class ResourceManager : IDisposable
    {
        private string _resourceName;
        private bool _isResourceOpen;
        private bool _disposed = false; // Track disposal status

        /// <summary>
        /// Constructor that "opens" a resource
        /// </summary>
        /// <param name="resourceName">Name of the resource to manage</param>
        public ResourceManager(string resourceName)
        {
            _resourceName = resourceName ?? "Unknown Resource";
            _isResourceOpen = true;
            Console.WriteLine($"  🔓 ResourceManager: Opened resource '{_resourceName}'");
        }

        /// <summary>
        /// Property to check if resource is open
        /// </summary>
        public bool IsResourceOpen => _isResourceOpen && !_disposed;

        /// <summary>
        /// Property to get resource name
        /// </summary>
        public string ResourceName => _resourceName;

        /// <summary>
        /// Method to use the resource
        /// </summary>
        /// <param name="operation">Operation to perform</param>
        public void UseResource(string operation)
        {
            // Check if disposed
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResourceManager), 
                    "Cannot use resource after disposal");
            }

            if (!_isResourceOpen)
            {
                Console.WriteLine($"  ❌ Cannot perform '{operation}' - resource '{_resourceName}' is not open");
                return;
            }

            Console.WriteLine($"  ⚙️ Performing '{operation}' on resource '{_resourceName}'");
        }

        /// <summary>
        /// Method to manually close the resource
        /// </summary>
        public void CloseResource()
        {
            if (_isResourceOpen)
            {
                _isResourceOpen = false;
                Console.WriteLine($"  🔒 ResourceManager: Manually closed resource '{_resourceName}'");
            }
        }

        /// <summary>
        /// IDisposable implementation - the preferred way to clean up
        /// This is called when you use 'using' statements or call Dispose() explicitly
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine($"  🧹 ResourceManager.Dispose() called for '{_resourceName}'");
            Dispose(true);
            
            // Tell the garbage collector not to call the finalizer
            // since we've already cleaned up
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected virtual dispose method - this is the pattern for proper disposal
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here
                    Console.WriteLine($"  🧹 Disposing managed resources for '{_resourceName}'");
                    CloseResource();
                }

                // Dispose unmanaged resources here (if any)
                Console.WriteLine($"  🧹 Disposing unmanaged resources for '{_resourceName}'");
                
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer (destructor) - this is called by the garbage collector
        /// Only implement if you have unmanaged resources to clean up
        /// Finalizers have performance overhead, so avoid them if possible
        /// </summary>
        ~ResourceManager()
        {
            Console.WriteLine($"  ⚰️ ResourceManager finalizer called for '{_resourceName}'");
            Console.WriteLine($"     ⚠️ Resource was not properly disposed! Use 'using' statements or call Dispose()");
            
            // Call Dispose with false to indicate we're in the finalizer
            Dispose(false);
        }

        /// <summary>
        /// Method to display resource status
        /// </summary>
        public void DisplayStatus()
        {
            Console.WriteLine($"  📊 ResourceManager Status:");
            Console.WriteLine($"      Resource Name: {_resourceName}");
            Console.WriteLine($"      Is Open: {_isResourceOpen}");
            Console.WriteLine($"      Is Disposed: {_disposed}");
        }

        /// <summary>
        /// Static method to demonstrate proper resource management
        /// </summary>
        public static void DemonstrateResourceManagement()
        {
            Console.WriteLine($"  🏭 Demonstrating Resource Management:");

            // Example 1: Using 'using' statement (recommended)
            Console.WriteLine($"  📝 Example 1: Using 'using' statement (automatic disposal)");
            using (var resource1 = new ResourceManager("Database Connection"))
            {
                resource1.UseResource("SELECT query");
                resource1.UseResource("UPDATE query");
                resource1.DisplayStatus();
                // Dispose() is automatically called when leaving this block
            }
            Console.WriteLine($"  ✅ Resource automatically disposed when leaving 'using' block");

            Console.WriteLine();

            // Example 2: Manual disposal (also good)
            Console.WriteLine($"  📝 Example 2: Manual disposal");
            var resource2 = new ResourceManager("File Handle");
            try
            {
                resource2.UseResource("Read file");
                resource2.UseResource("Write file");
                resource2.DisplayStatus();
            }
            finally
            {
                resource2.Dispose(); // Manually call Dispose
            }
            Console.WriteLine($"  ✅ Resource manually disposed in finally block");

            Console.WriteLine();

            // Example 3: Forgetting to dispose (bad practice)
            Console.WriteLine($"  📝 Example 3: Forgetting to dispose (will use finalizer)");
            var resource3 = new ResourceManager("Memory Buffer");
            resource3.UseResource("Process data");
            resource3.DisplayStatus();
            // Not disposing - finalizer will be called eventually by GC
            resource3 = null; // Remove reference

            Console.WriteLine($"  ⚠️ Resource not disposed - finalizer will handle cleanup later");
            Console.WriteLine($"     💡 This is inefficient and should be avoided!");

            // Force garbage collection to demonstrate finalizer
            Console.WriteLine($"  🗑️ Forcing garbage collection to show finalizer...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
