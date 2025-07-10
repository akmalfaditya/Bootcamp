// How the Garbage Collector Works - Comprehensive Training Project
// This project demonstrates all aspects of .NET garbage collection behavior
// Run this to see GC in action with real memory allocations and collections

using System.Buffers;
using System.Runtime;

namespace HowTheGCWorks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== .NET Garbage Collector Comprehensive Training ===");
            Console.WriteLine("This training covers the standard CLR generational mark-and-compact GC");
            Console.WriteLine("You'll see how the GC traces object graphs from roots to determine reachability\n");
            
            // Core GC process demonstrations - these show the fundamental mechanics
            // Every .NET developer needs to understand these concepts thoroughly
            RunDemo1_BasicGCBehavior();
            RunDemo2_GenerationalCollection();
            RunDemo3_LargeObjectHeap();
            RunDemo4_ForcedCollection();
            RunDemo5_MemoryPressure();
            RunDemo6_ArrayPooling();
            RunDemo7_GCLatencyModes();
            RunDemo8_BackgroundCollection();
            
            // Advanced GC optimizations - these separate good developers from great ones
            Console.WriteLine("\n=== Advanced GC Optimization Techniques ===");
            GCConfigurationDemo.DemonstrateServerVsWorkstationGC();
            WeakReferenceDemo.DemonstrateWeakReferences();
            PinnedMemoryDemo.DemonstratePinnedMemory();
            GenerationPromotionDemo.DemonstrateGenerationPromotion();
            FragmentationDemo.DemonstrateHeapFragmentation();
            GCNotificationDemo.DemonstrateGCNotifications();
            NoGCRegionDemo.DemonstrateNoGCRegion();
            
            // Memory management best practices
            Console.WriteLine("\n=== Memory Management Best Practices ===");
            MemoryPressureDemo.DemonstrateMemoryPressure();
            FinalizationQueueDemo.DemonstrateFinalizationQueue();
            ObjectResurrectionDemo.DemonstrateObjectResurrection();
            
            Console.WriteLine("\n=== Training Complete - You Now Understand GC Internals ===");
            Console.WriteLine("Remember: Let the GC do its job unless you have specific performance requirements");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        static void RunDemo1_BasicGCBehavior()
        {
            Console.WriteLine("DEMO 1: Core GC Process - Marking, Segregation, and Compaction");
            Console.WriteLine("==============================================================");
            Console.WriteLine("The GC performs three critical phases during each collection:");
            Console.WriteLine("1. MARKING: Traces object graph from roots to find reachable objects");
            Console.WriteLine("2. SEGREGATION: Separates live objects from garbage, handles finalizers");
            Console.WriteLine("3. COMPACTION: Moves live objects together to eliminate fragmentation\n");
            
            // Show current memory state - this represents the managed heap before allocation
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before allocation: {memoryBefore:N0} bytes");
            
            // Create objects that will become garbage - these won't be reachable from roots
            CreateTemporaryObjects();
            
            long memoryAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after allocation: {memoryAfter:N0} bytes");
            Console.WriteLine($"Memory increase: {memoryAfter - memoryBefore:N0} bytes");
            Console.WriteLine("^^ These objects are now unreachable and eligible for collection");
            
            // Force collection to demonstrate the three-phase process
            Console.WriteLine("\nInitiating garbage collection to demonstrate GC phases...");
            Console.WriteLine("Phase 1: MARKING - GC traces from roots, finds no references to our objects");
            Console.WriteLine("Phase 2: SEGREGATION - Objects marked as garbage (no finalizers in this demo)");
            Console.WriteLine("Phase 3: COMPACTION - Remaining objects moved together, heap defragmented");
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect(); // Second collect handles any objects that were finalized
            
            long memoryAfterGC = GC.GetTotalMemory(false);
            Console.WriteLine($"\nMemory after GC: {memoryAfterGC:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {memoryAfter - memoryAfterGC:N0} bytes");
            Console.WriteLine("Result: Memory compacted, fragmentation eliminated, allocation pointer reset");
            Console.WriteLine();
        }
        
        static void CreateTemporaryObjects()
        {
            // These objects will go out of scope and become eligible for collection
            // This demonstrates the marking phase - these won't be reachable from roots
            var temporaryList = new List<string>();
            
            for (int i = 0; i < 1000; i++)
            {
                temporaryList.Add($"Temporary string {i} - this will become garbage");
            }
            
            // When this method ends, temporaryList becomes unreachable (garbage)
        }
        
        static void RunDemo2_GenerationalCollection()
        {
            Console.WriteLine("DEMO 2: Generational Hypothesis - The Foundation of GC Efficiency");
            Console.WriteLine("================================================================");
            Console.WriteLine("The generational hypothesis: Most objects die young, survivors live long");
            Console.WriteLine("This enables the GC to focus efforts where most garbage is found\n");
            
            Console.WriteLine("Generation Structure:");
            Console.WriteLine("- Gen0: New objects (100KB-few MB), collected frequently (<1ms)");
            Console.WriteLine("- Gen1: Survived one collection, buffer for Gen2");
            Console.WriteLine("- Gen2: Long-lived objects, full collections (expensive)\n");
            
            // Show initial generation counts
            ShowGenerationCounts("Initial state - before any allocations");
            
            // Create short-lived objects that demonstrate the generational hypothesis
            Console.WriteLine("\nCreating ephemeral objects (will prove the generational hypothesis)...");
            CreateShortLivedObjects();
            ShowGenerationCounts("After ephemeral object creation");
            
            // Force Gen0 collection to show how most objects die young
            Console.WriteLine("\nForcing Gen0 collection - demonstrating 'most objects die young'...");
            GC.Collect(0);
            ShowGenerationCounts("After Gen0 collection - notice most objects were collected");
            
            // Create objects that survive to demonstrate promotion behavior
            Console.WriteLine("\nCreating survivor objects and forcing promotion...");
            var survivors = CreateSurvivorObjects();
            
            // Multiple collections to demonstrate promotion through generations
            Console.WriteLine("Forcing multiple collections to show generation promotion...");
            GC.Collect(0); // Gen0 collection - survivors move to Gen1
            ShowGenerationCounts("After first collection - survivors promoted to Gen1");
            
            GC.Collect(1); // Gen1 collection - survivors move to Gen2
            ShowGenerationCounts("After second collection - survivors promoted to Gen2");
            
            Console.WriteLine("Result: Objects that survive multiple collections become long-lived");
            Console.WriteLine("The GC now treats these as Gen2 objects, collecting them infrequently");
            
            // Keep survivors alive to prevent them from being collected
            GC.KeepAlive(survivors);
            Console.WriteLine();
        }
        
        static object CreateSurvivorObjects()
        {
            // These objects will be kept alive and demonstrate generation promotion
            var survivors = new List<object>();
            
            for (int i = 0; i < 100; i++)
            {
                survivors.Add($"Survivor object {i} - will be promoted through generations");
            }
            
            return survivors;
        }
        
        static void CreateShortLivedObjects()
        {
            // These objects are created and immediately become eligible for collection
            // Perfect candidates for Gen0 collection - very short lifetime
            for (int i = 0; i < 1000; i++)
            {
                var temp = new byte[1024]; // 1KB objects
                // temp goes out of scope immediately - becomes Gen0 garbage
            }
        }
        
        static List<object> CreateSurvivingObjects()
        {
            // These objects will be returned and kept alive longer
            // They'll survive Gen0 collection and get promoted to higher generations
            var survivors = new List<object>();
            
            for (int i = 0; i < 100; i++)
            {
                survivors.Add(new byte[2048]); // 2KB objects that will survive
            }
            
            return survivors;
        }
        
        static void ShowGenerationCounts(string label)
        {
            Console.WriteLine($"{label}:");
            Console.WriteLine($"  Gen0 collections: {GC.CollectionCount(0)}");
            Console.WriteLine($"  Gen1 collections: {GC.CollectionCount(1)}");
            Console.WriteLine($"  Gen2 collections: {GC.CollectionCount(2)}");
        }
        
        static void RunDemo3_LargeObjectHeap()
        {
            Console.WriteLine("DEMO 3: Large Object Heap (LOH) - Objects >= 85,000 bytes");
            Console.WriteLine("============================================================");
            
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before LOH allocation: {memoryBefore:N0} bytes");
            
            // Allocate objects larger than 85,000 bytes - these go to LOH
            Console.WriteLine("\nAllocating large objects (>85KB each)...");
            var largeObjects = new List<byte[]?>();
            
            for (int i = 0; i < 10; i++)
            {
                // Each array is 100KB - definitely goes to LOH
                largeObjects.Add(new byte[100_000]);
                Console.WriteLine($"Allocated large object {i + 1} (100KB)");
            }
            
            long memoryAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"\nMemory after LOH allocation: {memoryAfter:N0} bytes");
            Console.WriteLine($"LOH memory used: {memoryAfter - memoryBefore:N0} bytes");
            
            // Demonstrate LOH compaction (normally LOH is not compacted)
            Console.WriteLine("\nSetting LOH to compact on next collection...");
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            
            // Clear some references to create fragmentation
            largeObjects[2] = null;
            largeObjects[5] = null;
            largeObjects[8] = null;
            
            Console.WriteLine("Forcing GC with LOH compaction...");
            GC.Collect();
            
            long memoryAfterCompaction = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after LOH compaction: {memoryAfterCompaction:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {memoryAfter - memoryAfterCompaction:N0} bytes");
            
            GC.KeepAlive(largeObjects);
            Console.WriteLine();
        }
        
        static void RunDemo4_ForcedCollection()
        {
            Console.WriteLine("DEMO 4: Forced Garbage Collection - When and How");
            Console.WriteLine("=================================================");
            
            ShowGenerationCounts("Before forced collections");
            
            // Demonstrate different collection levels
            Console.WriteLine("\nForcing Gen0 collection only...");
            GC.Collect(0);
            ShowGenerationCounts("After Gen0 collection");
            
            Console.WriteLine("\nForcing Gen1 collection (includes Gen0)...");
            GC.Collect(1);
            ShowGenerationCounts("After Gen1 collection");
            
            Console.WriteLine("\nForcing full collection (all generations)...");
            GC.Collect();
            ShowGenerationCounts("After full collection");
            
            // Demonstrate proper pattern for forcing collection with finalizers
            Console.WriteLine("\nProper forced collection pattern (with finalizers)...");
            CreateFinalizableObjects();
            
            // The recommended pattern when you need to force collection
            GC.Collect();                    // Collect objects
            GC.WaitForPendingFinalizers();   // Wait for finalizers to run
            GC.Collect();                    // Collect objects whose finalizers ran
            
            Console.WriteLine("Complete forced collection cycle finished");
            Console.WriteLine();
        }
        
        static void CreateFinalizableObjects()
        {
            // Create objects with finalizers to demonstrate the complete collection cycle
            for (int i = 0; i < 100; i++)
            {
                var obj = new FinalizableObject(i);
                // Objects go out of scope, but finalizers need to run
            }
        }
        
        static void RunDemo5_MemoryPressure()
        {
            Console.WriteLine("DEMO 5: Memory Pressure - Informing GC about unmanaged memory");
            Console.WriteLine("==============================================================");
            
            ShowGenerationCounts("Before memory pressure");
            
            // Simulate allocating unmanaged memory
            // In real scenarios, this might be native API calls, COM objects, etc.
            const long unmangedMemorySize = 50_000_000; // 50MB
            
            Console.WriteLine($"Simulating {unmangedMemorySize:N0} bytes of unmanaged memory allocation...");
            
            // Tell the GC about unmanaged memory pressure
            GC.AddMemoryPressure(unmangedMemorySize);
            
            Console.WriteLine("Memory pressure added - GC may collect more aggressively now");
            
            // Allocate some managed objects to trigger collection
            var managedObjects = new List<byte[]>();
            for (int i = 0; i < 1000; i++)
            {
                managedObjects.Add(new byte[10_000]); // 10KB each
            }
            
            ShowGenerationCounts("After allocation with memory pressure");
            
            // Important: Remove memory pressure when unmanaged memory is freed
            Console.WriteLine("\nSimulating unmanaged memory deallocation...");
            GC.RemoveMemoryPressure(unmangedMemorySize);
            Console.WriteLine("Memory pressure removed");
            
            GC.KeepAlive(managedObjects);
            Console.WriteLine();
        }
        
        static void RunDemo6_ArrayPooling()
        {
            Console.WriteLine("DEMO 6: Array Pooling - Reducing GC pressure with ArrayPool");
            Console.WriteLine("============================================================");
            
            // First, demonstrate the problem: frequent array allocation
            Console.WriteLine("Without Array Pooling (creates GC pressure):");
            ShowGenerationCounts("Before frequent allocations");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Allocate and discard arrays frequently - bad for GC
            for (int i = 0; i < 10000; i++)
            {
                var array = new int[1000]; // Each allocation creates garbage
                // Use array briefly
                array[0] = i;
                // Array becomes garbage when method continues
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Time without pooling: {stopwatch.ElapsedMilliseconds}ms");
            ShowGenerationCounts("After frequent allocations");
            
            // Now demonstrate the solution: array pooling
            Console.WriteLine("\nWith Array Pooling (reduces GC pressure):");
            ShowGenerationCounts("Before pooled allocations");
            
            stopwatch.Restart();
            
            // Use array pool to reuse arrays - good for GC
            var pool = ArrayPool<int>.Shared;
            
            for (int i = 0; i < 10000; i++)
            {
                var array = pool.Rent(1000); // Rent from pool
                try
                {
                    // Use array
                    array[0] = i;
                }
                finally
                {
                    pool.Return(array); // Return to pool for reuse
                }
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Time with pooling: {stopwatch.ElapsedMilliseconds}ms");
            ShowGenerationCounts("After pooled allocations");
            
            Console.WriteLine("\nArray pooling significantly reduces garbage collection pressure!");
            Console.WriteLine();
        }
        
        static void RunDemo7_GCLatencyModes()
        {
            Console.WriteLine("DEMO 7: GC Latency Modes - Balancing responsiveness vs throughput");
            Console.WriteLine("===================================================================");
            
            // Show current latency mode
            Console.WriteLine($"Current GC Latency Mode: {GCSettings.LatencyMode}");
            
            // Demonstrate different latency modes
            var originalMode = GCSettings.LatencyMode;
            
            try
            {
                // Low Latency Mode - prioritizes responsiveness
                Console.WriteLine("\nSetting to LowLatency mode (prioritizes responsiveness)...");
                GCSettings.LatencyMode = GCLatencyMode.LowLatency;
                
                PerformWorkWithGCMeasurement("LowLatency");
                
                // Batch Mode - prioritizes throughput
                Console.WriteLine("\nSetting to Batch mode (prioritizes throughput)...");
                GCSettings.LatencyMode = GCLatencyMode.Batch;
                
                PerformWorkWithGCMeasurement("Batch");
                
                // Interactive Mode - balanced approach
                Console.WriteLine("\nSetting to Interactive mode (balanced)...");
                GCSettings.LatencyMode = GCLatencyMode.Interactive;
                
                PerformWorkWithGCMeasurement("Interactive");
            }
            finally
            {
                // Always restore original mode
                GCSettings.LatencyMode = originalMode;
                Console.WriteLine($"\nRestored original latency mode: {originalMode}");
            }
            
            Console.WriteLine();
        }
        
        static void PerformWorkWithGCMeasurement(string modeName)
        {
            var initialCounts = new int[]
            {
                GC.CollectionCount(0),
                GC.CollectionCount(1),
                GC.CollectionCount(2)
            };
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Perform memory-intensive work
            var objects = new List<byte[]?>();
            for (int i = 0; i < 5000; i++)
            {
                objects.Add(new byte[5000]); // 5KB each
                if (i % 1000 == 0)
                {
                    // Periodically clear some objects to trigger collection
                    for (int j = 0; j < objects.Count / 2; j++)
                    {
                        objects[j] = null;
                    }
                }
            }
            
            stopwatch.Stop();
            
            var finalCounts = new int[]
            {
                GC.CollectionCount(0),
                GC.CollectionCount(1),
                GC.CollectionCount(2)
            };
            
            Console.WriteLine($"{modeName} mode results:");
            Console.WriteLine($"  Time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Gen0 collections: {finalCounts[0] - initialCounts[0]}");
            Console.WriteLine($"  Gen1 collections: {finalCounts[1] - initialCounts[1]}");
            Console.WriteLine($"  Gen2 collections: {finalCounts[2] - initialCounts[2]}");
            
            GC.KeepAlive(objects);
        }
        
        static void RunDemo8_BackgroundCollection()
        {
            Console.WriteLine("DEMO 8: Background Collection - Concurrent GC behavior");
            Console.WriteLine("======================================================");
            
            Console.WriteLine($"Concurrent GC enabled: {GCSettings.IsServerGC}");
            Console.WriteLine($"Server GC mode: {GCSettings.IsServerGC}");
            
            // Background collection is enabled by default and runs concurrently
            // This demo shows how work continues during collection
            
            Console.WriteLine("\nDemonstrating background collection behavior...");
            ShowGenerationCounts("Before background collection demo");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var workCompleted = 0;
            
            // Start memory-intensive work that will trigger background collection
            var backgroundWork = Task.Run(() =>
            {
                var localObjects = new List<object>();
                
                for (int i = 0; i < 10000; i++)
                {
                    localObjects.Add(new byte[10000]); // 10KB each
                    
                    if (i % 100 == 0)
                    {
                        // Clear some objects to encourage collection
                        localObjects.Clear();
                        Interlocked.Increment(ref workCompleted);
                    }
                }
                
                GC.KeepAlive(localObjects);
            });
            
            // Meanwhile, do other work to show application continues running
            var mainWork = Task.Run(() =>
            {
                while (!backgroundWork.IsCompleted)
                {
                    // Simulate application work continuing during GC
                    Thread.Sleep(10);
                    Interlocked.Increment(ref workCompleted);
                }
            });
            
            Task.WaitAll(backgroundWork, mainWork);
            stopwatch.Stop();
            
            Console.WriteLine($"Work completed during collection: {workCompleted} units");
            Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}ms");
            ShowGenerationCounts("After background collection demo");
            
            Console.WriteLine("Background collection allows work to continue during GC!");
            Console.WriteLine();
        }
    }
    
    // Supporting class to demonstrate finalizers in forced collection demo
    public class FinalizableObject
    {
        private readonly int _id;
        
        public FinalizableObject(int id)
        {
            _id = id;
        }
        
        // Finalizer - will be called during garbage collection
        ~FinalizableObject()
        {
            // In real code, you'd cleanup unmanaged resources here
            // For demo purposes, we'll just track that finalization happened
            if (_id % 20 == 0) // Only print every 20th to avoid spam
            {
                Console.WriteLine($"  Finalizer called for object {_id}");
            }
        }
    }
}
