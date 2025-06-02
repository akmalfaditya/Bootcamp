// Advanced GC Examples - Specialized scenarios and edge cases
// These classes demonstrate more complex GC behavior patterns

using System.Runtime;

namespace HowTheGCWorks
{
    // This class shows server vs workstation GC configuration impact
    public static class GCConfigurationDemo
    {
        public static void DemonstrateServerVsWorkstationGC()
        {
            Console.WriteLine("GC Configuration Analysis");
            Console.WriteLine("========================");
            
            // Show current GC configuration
            Console.WriteLine($"Server GC: {GCSettings.IsServerGC}");
            Console.WriteLine($"Concurrent GC: {!GCSettings.IsServerGC || Environment.ProcessorCount > 1}");
            Console.WriteLine($"Processor Count: {Environment.ProcessorCount}");
            
            if (GCSettings.IsServerGC)
            {
                Console.WriteLine("Server GC is active:");
                Console.WriteLine("- Multiple GC threads (one per core)");
                Console.WriteLine("- Higher memory usage but better throughput");
                Console.WriteLine("- Best for server applications with multiple cores");
            }
            else
            {
                Console.WriteLine("Workstation GC is active:");
                Console.WriteLine("- Single GC thread");
                Console.WriteLine("- Lower memory usage");
                Console.WriteLine("- Better for client applications");
            }
            
            // Demonstrate memory allocation patterns under current GC mode
            TestAllocationPerformance();
        }
        
        private static void TestAllocationPerformance()
        {
            Console.WriteLine("\nTesting allocation performance under current GC mode...");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var objects = new List<object>();
            
            // Allocate many objects to stress-test the GC
            for (int i = 0; i < 100000; i++)
            {
                objects.Add(new AllocatedObject(i));
                
                if (i % 10000 == 0)
                {
                    Console.WriteLine($"Allocated {i + 1} objects...");
                }
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Allocation completed in: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Memory used: {GC.GetTotalMemory(false):N0} bytes");
            
            GC.KeepAlive(objects);
        }
    }
    
    // Simple class for allocation testing
    public class AllocatedObject
    {
        public int Id { get; }
        public string Data { get; }
        public DateTime CreatedAt { get; }
        
        public AllocatedObject(int id)
        {
            Id = id;
            Data = $"Object data for {id}";
            CreatedAt = DateTime.Now;
        }
    }
    
    // This class demonstrates weak references and their GC behavior
    public class WeakReferenceDemo
    {
        public static void DemonstrateWeakReferences()
        {
            Console.WriteLine("Weak Reference Demonstration");
            Console.WriteLine("============================");
            
            // Create a regular (strong) reference
            var strongRef = new ExpensiveObject("Strong Reference Object");
            
            // Create weak references to the same object
            var weakRef = new WeakReference(strongRef);
            var weakRefTrackResurrection = new WeakReference(strongRef, true);
            
            Console.WriteLine($"Strong reference alive: {strongRef != null}");
            Console.WriteLine($"Weak reference alive: {weakRef.IsAlive}");
            Console.WriteLine($"Weak reference (track resurrection) alive: {weakRefTrackResurrection.IsAlive}");
            
            // Clear the strong reference - object becomes eligible for collection
            Console.WriteLine("\nClearing strong reference...");
            strongRef = null;
            
            // Force collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Console.WriteLine($"After GC - Weak reference alive: {weakRef.IsAlive}");
            Console.WriteLine($"After GC - Weak reference (track resurrection) alive: {weakRefTrackResurrection.IsAlive}");
            
            if (weakRef.Target != null)
            {
                Console.WriteLine("Weak reference target still accessible");
            }
            else
            {
                Console.WriteLine("Weak reference target has been collected");
            }
        }
    }
    
    // Object with finalizer to demonstrate weak reference behavior
    public class ExpensiveObject
    {
        public string Name { get; }
        
        public ExpensiveObject(string name)
        {
            Name = name;
            Console.WriteLine($"ExpensiveObject '{name}' created");
        }
        
        ~ExpensiveObject()
        {
            Console.WriteLine($"ExpensiveObject '{Name}' finalized");
        }
    }
    
    // This class demonstrates pinned memory and its GC implications
    public class PinnedMemoryDemo
    {
        public static unsafe void DemonstratePinnedMemory()
        {
            Console.WriteLine("Pinned Memory Demonstration");
            Console.WriteLine("============================");
            
            // Allocate a managed array
            byte[] managedArray = new byte[10000];
            
            Console.WriteLine("Created managed array - GC can move this around");
            Console.WriteLine($"Initial memory: {GC.GetTotalMemory(false):N0} bytes");
            
            // Pin the array in memory
            var handle = System.Runtime.InteropServices.GCHandle.Alloc(managedArray, 
                System.Runtime.InteropServices.GCHandleType.Pinned);
            
            try
            {
                Console.WriteLine("Array is now pinned - GC cannot move it");
                Console.WriteLine("This can cause heap fragmentation if overused");
                
                // Get the pinned address
                IntPtr pinnedAddress = handle.AddrOfPinnedObject();
                Console.WriteLine($"Pinned address: 0x{pinnedAddress:X}");
                
                // Demonstrate that GC still works, but the pinned object won't move
                Console.WriteLine("\nAllocating more objects while array is pinned...");
                var moreObjects = new List<byte[]>();
                
                for (int i = 0; i < 1000; i++)
                {
                    moreObjects.Add(new byte[1000]);
                }
                
                Console.WriteLine("Forcing GC while array is pinned...");
                GC.Collect();
                
                Console.WriteLine($"Memory after GC: {GC.GetTotalMemory(false):N0} bytes");
                Console.WriteLine("Pinned array remained at the same address during GC");
                
                GC.KeepAlive(moreObjects);
            }
            finally
            {
                // Always free pinned handles!
                handle.Free();
                Console.WriteLine("Pinned handle freed - array can now be moved by GC");
            }
        }
    }
    
    // This class demonstrates generational promotion in detail
    public class GenerationPromotionDemo
    {
        public static void DemonstrateGenerationPromotion()
        {
            Console.WriteLine("Generation Promotion Detailed Demonstration");
            Console.WriteLine("==========================================");
            
            // Create objects that we'll track through generations
            var trackedObjects = new List<TrackableObject>();
            
            Console.WriteLine("Creating trackable objects...");
            for (int i = 0; i < 100; i++)
            {
                trackedObjects.Add(new TrackableObject(i));
            }
            
            // Check initial generation
            CheckObjectGenerations(trackedObjects, "Initial allocation");
            
            // Force Gen0 collection to promote survivors
            Console.WriteLine("\nForcing Gen0 collection...");
            GC.Collect(0);
            CheckObjectGenerations(trackedObjects, "After Gen0 collection");
            
            // Force Gen1 collection to promote to Gen2
            Console.WriteLine("\nForcing Gen1 collection...");
            GC.Collect(1);
            CheckObjectGenerations(trackedObjects, "After Gen1 collection");
            
            // Show what happens with new objects
            Console.WriteLine("\nCreating new objects (these will be Gen0)...");
            var newObjects = new List<TrackableObject>();
            for (int i = 0; i < 50; i++)
            {
                newObjects.Add(new TrackableObject(i + 1000));
            }
            
            CheckObjectGenerations(newObjects, "New objects");
            CheckObjectGenerations(trackedObjects, "Original objects (should still be Gen2)");
            
            GC.KeepAlive(trackedObjects);
            GC.KeepAlive(newObjects);
        }
        
        private static void CheckObjectGenerations(List<TrackableObject> objects, string label)
        {
            var gen0Count = 0;
            var gen1Count = 0;
            var gen2Count = 0;
            
            foreach (var obj in objects.Take(10)) // Check first 10 to avoid spam
            {
                var generation = GC.GetGeneration(obj);
                switch (generation)
                {
                    case 0: gen0Count++; break;
                    case 1: gen1Count++; break;
                    case 2: gen2Count++; break;
                }
            }
            
            Console.WriteLine($"{label} (first 10 objects):");
            Console.WriteLine($"  Gen0: {gen0Count}, Gen1: {gen1Count}, Gen2: {gen2Count}");
        }
    }
    
    // Simple class to track through generations
    public class TrackableObject
    {
        public int Id { get; }
        public byte[] Data { get; }
        
        public TrackableObject(int id)
        {
            Id = id;
            Data = new byte[1024]; // 1KB to make tracking easier
        }
    }
    
    // This class demonstrates memory fragmentation and compaction
    public class FragmentationDemo
    {
        public static void DemonstrateHeapFragmentation()
        {
            Console.WriteLine("Heap Fragmentation Demonstration");
            Console.WriteLine("=================================");
            
            long initialMemory = GC.GetTotalMemory(false);
            Console.WriteLine($"Initial memory: {initialMemory:N0} bytes");
            
            // Create a pattern that causes fragmentation
            var objects = new List<object>();
            
            Console.WriteLine("\nCreating fragmented allocation pattern...");
            
            // Allocate objects of different sizes
            for (int i = 0; i < 1000; i++)
            {
                if (i % 3 == 0)
                {
                    objects.Add(new byte[1000]);    // Small objects
                }
                else if (i % 3 == 1)
                {
                    objects.Add(new byte[5000]);    // Medium objects  
                }
                else
                {
                    objects.Add(new byte[10000]);   // Large objects
                }
            }
            
            long afterAllocation = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after allocation: {afterAllocation:N0} bytes");
            
            // Now remove every other object to create gaps
            Console.WriteLine("\nRemoving every other object to create fragmentation...");
            for (int i = 1; i < objects.Count; i += 2)
            {
                objects[i] = null;
            }
            
            long afterFragmentation = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after creating gaps: {afterFragmentation:N0} bytes");
            
            // Force collection to see compaction in action
            Console.WriteLine("\nForcing garbage collection (compaction should occur)...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long afterCompaction = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after compaction: {afterCompaction:N0} bytes");
            Console.WriteLine($"Memory reclaimed by compaction: {afterFragmentation - afterCompaction:N0} bytes");
            
            Console.WriteLine("\nCompaction eliminates fragmentation gaps!");
            
            GC.KeepAlive(objects);
        }
    }
}
