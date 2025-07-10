// Memory Pressure and Finalization Examples
// These classes demonstrate how to work with unmanaged memory and the finalization queue

using System.Runtime.InteropServices;

namespace HowTheGCWorks
{
    // This class demonstrates memory pressure notifications to the GC
    public static class MemoryPressureDemo
    {
        public static void DemonstrateMemoryPressure()
        {
            Console.WriteLine("Memory Pressure - Informing GC About Unmanaged Memory");
            Console.WriteLine("====================================================");
            Console.WriteLine("When your app allocates unmanaged memory, the GC doesn't know about it");
            Console.WriteLine("Memory pressure helps GC make informed collection decisions\n");
            
            long managedBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Managed memory before: {managedBefore:N0} bytes");
            
            // Simulate allocation of unmanaged memory (e.g., via P/Invoke, COM)
            long unmanagedBytes = 50 * 1024 * 1024; // 50MB of "unmanaged" memory
            
            Console.WriteLine($"Simulating allocation of {unmanagedBytes:N0} bytes of unmanaged memory");
            Console.WriteLine("Without memory pressure, GC doesn't know about this allocation");
            
            // Tell the GC about our unmanaged memory allocation
            Console.WriteLine("Adding memory pressure to inform GC about unmanaged allocation...");
            GC.AddMemoryPressure(unmanagedBytes);
            
            // Show GC behavior with memory pressure
            int gen0Before = GC.CollectionCount(0);
            int gen1Before = GC.CollectionCount(1);
            int gen2Before = GC.CollectionCount(2);
            
            // Allocate some managed memory to trigger potential collections
            Console.WriteLine("Allocating managed memory to see GC behavior with memory pressure...");
            var managedObjects = new List<byte[]>();
            for (int i = 0; i < 100; i++)
            {
                managedObjects.Add(new byte[100_000]); // 100KB each
            }
            
            int gen0After = GC.CollectionCount(0);
            int gen1After = GC.CollectionCount(1);
            int gen2After = GC.CollectionCount(2);
            
            Console.WriteLine($"GC collections triggered:");
            Console.WriteLine($"  Gen0: {gen0After - gen0Before}");
            Console.WriteLine($"  Gen1: {gen1After - gen1Before}");
            Console.WriteLine($"  Gen2: {gen2After - gen2Before}");
            
            // Simulate freeing the unmanaged memory
            Console.WriteLine($"\nSimulating release of {unmanagedBytes:N0} bytes of unmanaged memory");
            GC.RemoveMemoryPressure(unmanagedBytes);
            Console.WriteLine("Memory pressure removed - GC now knows memory was freed");
            
            long managedAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"Managed memory after: {managedAfter:N0} bytes");
            
            Console.WriteLine("\nAlways pair AddMemoryPressure with RemoveMemoryPressure!");
            Console.WriteLine("This helps GC make optimal collection timing decisions");
            
            GC.KeepAlive(managedObjects);
            Console.WriteLine();
        }
    }

    // Demonstrates objects with finalizers and the finalization queue
    public class FinalizableResourceExample
    {
        private readonly int _id;
        private readonly IntPtr _unmanagedResource;
        private bool _disposed = false;

        public FinalizableResourceExample(int id)
        {
            _id = id;
            // Simulate allocation of unmanaged resource
            _unmanagedResource = Marshal.AllocHGlobal(1024); // 1KB unmanaged memory
            
            if (_id % 50 == 0) // Only print every 50th to avoid spam
            {
                Console.WriteLine($"    Allocated unmanaged resource for object {_id}");
            }
        }

        // Finalizer - called by finalizer thread during GC
        ~FinalizableResourceExample()
        {
            if (_id % 50 == 0) // Only print every 50th to avoid spam
            {
                Console.WriteLine($"    Finalizer: Cleaning up unmanaged resource for object {_id}");
            }
            
            if (!_disposed)
            {
                // Clean up unmanaged resources
                if (_unmanagedResource != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_unmanagedResource);
                }
                _disposed = true;
            }
        }

        // Proper disposal method (IDisposable pattern would be better)
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_unmanagedResource != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_unmanagedResource);
                }
                _disposed = true;
                
                // Remove from finalization queue since we handled cleanup
                GC.SuppressFinalize(this);
                
                if (_id % 50 == 0)
                {
                    Console.WriteLine($"    Disposed object {_id} - removed from finalization queue");
                }
            }
        }
    }

    public static class FinalizationQueueDemo
    {
        public static void DemonstrateFinalizationQueue()
        {
            Console.WriteLine("Finalization Queue - How Objects with Finalizers Are Handled");
            Console.WriteLine("==========================================================");
            Console.WriteLine("Objects with finalizers require special GC handling:");
            Console.WriteLine("1. First GC: Objects moved to finalization queue (kept alive)");
            Console.WriteLine("2. Finalizer thread executes finalizers");
            Console.WriteLine("3. Second GC: Objects finally collected (unless resurrected)\n");
            
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before creating finalizable objects: {memoryBefore:N0} bytes");
            
            // Create objects with finalizers
            Console.WriteLine("Creating 200 objects with finalizers (unmanaged resources)...");
            CreateFinalizableObjects();
            
            long memoryAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after creation: {memoryAfter:N0} bytes");
            Console.WriteLine($"Memory allocated: {memoryAfter - memoryBefore:N0} bytes");
            
            // First GC - objects moved to finalization queue
            Console.WriteLine("\nFirst GC: Objects become unreachable but moved to finalization queue...");
            GC.Collect();
            
            long memoryAfterFirstGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after first GC: {memoryAfterFirstGC:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {memoryAfter - memoryAfterFirstGC:N0} bytes");
            Console.WriteLine("Notice: Memory not fully reclaimed yet - objects in finalization queue");
            
            // Wait for finalizers to run
            Console.WriteLine("\nWaiting for finalizer thread to process finalization queue...");
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Finalizers completed - unmanaged resources cleaned up");
            
            // Second GC - finalized objects finally collected
            Console.WriteLine("\nSecond GC: Finalized objects can now be collected...");
            GC.Collect();
            
            long memoryAfterSecondGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after second GC: {memoryAfterSecondGC:N0} bytes");
            Console.WriteLine($"Additional memory reclaimed: {memoryAfterFirstGC - memoryAfterSecondGC:N0} bytes");
            Console.WriteLine($"Total memory reclaimed: {memoryAfter - memoryAfterSecondGC:N0} bytes");
            
            Console.WriteLine("\nResult: Two-phase collection ensures finalizers run before objects collected");
            Console.WriteLine("This is why finalizers add overhead - prefer IDisposable pattern!");
            Console.WriteLine();
        }

        private static void CreateFinalizableObjects()
        {
            // Create objects with finalizers that will go out of scope
            for (int i = 0; i < 200; i++)
            {
                var finalizableObj = new FinalizableResourceExample(i);
                
                // Some objects we dispose properly, others rely on finalizers
                if (i % 3 == 0)
                {
                    finalizableObj.Dispose(); // Proper cleanup - no finalizer needed
                }
                // Other objects will rely on finalizers when they go out of scope
            }
            // All objects become unreachable here
        }
    }

    // Demonstrates object resurrection - bringing objects back to life in finalizers
    public class ResurrectableObject
    {
        public static ResurrectableObject? ResurrectedInstance { get; private set; }
        private readonly int _id;

        public ResurrectableObject(int id)
        {
            _id = id;
        }

        ~ResurrectableObject()
        {
            Console.WriteLine($"Finalizer called for object {_id}");
            
            // RESURRECTION: Make object reachable again by storing reference
            ResurrectedInstance = this;
            Console.WriteLine($"Object {_id} has been RESURRECTED!");
            
            // Suppress further finalization since we're alive again
            GC.ReRegisterForFinalize(this);
        }

        public void Die()
        {
            ResurrectedInstance = null;
            Console.WriteLine($"Object {_id} is dying for real this time...");
        }
    }

    public static class ObjectResurrectionDemo
    {
        public static void DemonstrateObjectResurrection()
        {
            Console.WriteLine("Object Resurrection - Bringing Objects Back from the Dead");
            Console.WriteLine("========================================================");
            Console.WriteLine("Finalizers can 'resurrect' objects by making them reachable again");
            Console.WriteLine("This is possible but generally considered a bad practice\n");
            
            // Create an object that will resurrect itself
            Console.WriteLine("Creating a resurrectible object...");
            var zombie = new ResurrectableObject(666);
            
            Console.WriteLine("Making object unreachable (should trigger finalizer)...");
            zombie = null; // Object becomes unreachable
            
            // Force GC to trigger finalizer
            Console.WriteLine("Forcing GC to trigger finalization...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            // Check if object was resurrected
            if (ResurrectableObject.ResurrectedInstance != null)
            {
                Console.WriteLine("SUCCESS: Object was resurrected during finalization!");
                Console.WriteLine("The object is now reachable again through static reference");
                
                // Kill it for real this time
                Console.WriteLine("Now killing the resurrected object properly...");
                ResurrectableObject.ResurrectedInstance.Die();
                
                // Final cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect(); // Second collect to handle the final death
                
                Console.WriteLine("Object finally collected on second finalization cycle");
            }
            else
            {
                Console.WriteLine("Object was not resurrected");
            }
            
            Console.WriteLine("\nResurrection is powerful but dangerous - avoid in production code!");
            Console.WriteLine();
        }
    }
}
