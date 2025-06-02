using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// This program demonstrates how automatic garbage collection works in .NET.
    /// We'll explore object lifecycles, generational collection, roots, reachability,
    /// and memory monitoring - all the essential concepts you need to understand
    /// to write memory-efficient C# applications.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Automatic Garbage Collection in C# Demo ===\n");

            // Demo 1: Basic object lifecycle and memory allocation
            Console.WriteLine("1. Object Lifecycle and Memory Allocation:");
            DemoObjectLifecycle();
            Console.WriteLine();

            // Demo 2: Understanding roots and reachability
            Console.WriteLine("2. Roots and Reachability:");
            DemoRootsAndReachability();
            Console.WriteLine();

            // Demo 3: Generational garbage collection
            Console.WriteLine("3. Generational Garbage Collection:");
            DemoGenerationalGC();
            Console.WriteLine();

            // Demo 4: Circular references and how GC handles them
            Console.WriteLine("4. Circular References:");
            DemoCircularReferences();
            Console.WriteLine();

            // Demo 5: Memory monitoring and performance counters
            Console.WriteLine("5. Memory Monitoring:");
            DemoMemoryMonitoring();
            Console.WriteLine();

            // Demo 6: Large object heap behavior
            Console.WriteLine("6. Large Object Heap (LOH):");
            DemoLargeObjectHeap();
            Console.WriteLine();

            // Demo 7: Memory pressure and collection triggers
            Console.WriteLine("7. Memory Pressure and GC Triggers:");
            DemoMemoryPressure();
            Console.WriteLine();            // Demo 8: Weak references and their behavior
            Console.WriteLine("8. Weak References:");
            DemoWeakReferences();
            Console.WriteLine();

            // Demo 9: Finalization and disposal patterns
            Console.WriteLine("9. Finalization and Disposal:");
            DemoFinalizationAndDisposal();
            Console.WriteLine();

            Console.WriteLine("=== Demo Complete ===");
            Console.WriteLine("Remember: In production code, never call GC.Collect() manually!");
            Console.WriteLine("The GC is smarter than us and knows when to run.");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates the basic lifecycle of objects in managed memory.
        /// Shows how objects are allocated on the heap and when they become eligible for collection.
        /// </summary>
        static void DemoObjectLifecycle()
        {
            Console.WriteLine("Creating objects and watching their lifecycle...");
            
            // Show memory before creating objects
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before object creation: {memoryBefore:N0} bytes");

            // Create objects in a method scope to demonstrate local variable lifecycle
            CreateAndAbandonObjects();

            // Show memory after objects go out of scope
            long memoryAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after objects go out of scope: {memoryAfter:N0} bytes");
            Console.WriteLine($"Memory difference: {memoryAfter - memoryBefore:N0} bytes");

            // The objects are now eligible for collection, but may not be collected yet
            Console.WriteLine("Objects are now eligible for collection (but may not be collected yet)");

            // Force collection to see the difference (only for demonstration!)
            Console.WriteLine("Forcing garbage collection (for demo purposes only)...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryAfterGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after forced GC: {memoryAfterGC:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {memoryAfter - memoryAfterGC:N0} bytes");
        }

        /// <summary>
        /// Helper method that creates objects and lets them go out of scope.
        /// This demonstrates how local variables become unreachable when the method exits.
        /// </summary>
        static void CreateAndAbandonObjects()
        {
            // Create some objects that will become unreachable when this method exits
            byte[] largeArray = new byte[10000]; // 10KB array
            string[] stringArray = new string[1000];
            
            // Fill the string array with data
            for (int i = 0; i < stringArray.Length; i++)
            {
                stringArray[i] = $"String number {i} with some content to take up memory";
            }

            Console.WriteLine($"Created array with {largeArray.Length} bytes and {stringArray.Length} strings");
            
            // When this method exits, both arrays become unreachable because
            // the local variables (largeArray and stringArray) go out of scope
        }

        /// <summary>
        /// Demonstrates the concept of roots and how they keep objects alive.
        /// Shows the difference between reachable and unreachable objects.
        /// </summary>
        static void DemoRootsAndReachability()
        {
            Console.WriteLine("Exploring roots and object reachability...");

            // Create an object and establish a root reference
            var rootObject = new SampleObject("Root Object");
            Console.WriteLine($"Created root object: {rootObject.Name}");

            // Create child objects referenced by the root
            rootObject.Child = new SampleObject("Child Object");
            rootObject.Child.Child = new SampleObject("Grandchild Object");
            
            Console.WriteLine("Created child and grandchild objects referenced by root");

            // All objects are reachable through the root
            Console.WriteLine("All objects are currently reachable through root reference");

            // Create an orphaned object (no root reference)
            CreateOrphanedObject();

            // The orphaned object is eligible for collection, but root chain is safe
            Console.WriteLine("Orphaned object created and immediately became unreachable");

            // Now remove the root reference
            Console.WriteLine("Setting root reference to null...");
            rootObject = null!;

            // Now the entire chain becomes unreachable
            Console.WriteLine("Entire object chain is now unreachable and eligible for collection");
        }

        /// <summary>
        /// Creates an object with no root reference, making it immediately eligible for GC.
        /// </summary>
        static void CreateOrphanedObject()
        {
            // This object has no variable storing its reference
            // It's immediately eligible for garbage collection
            new SampleObject("Orphaned Object");
            Console.WriteLine("Created orphaned object (no variable stores its reference)");
        }

        /// <summary>
        /// Demonstrates how the generational garbage collector works.
        /// Shows object promotion through generations and collection frequency.
        /// </summary>
        static void DemoGenerationalGC()
        {
            Console.WriteLine("Exploring generational garbage collection...");

            // Show initial collection counts
            Console.WriteLine("Initial GC collection counts:");
            PrintGCGenerationInfo();

            // Create short-lived objects (Generation 0 candidates)
            Console.WriteLine("\nCreating many short-lived objects...");
            CreateShortLivedObjects();

            Console.WriteLine("After creating short-lived objects:");
            PrintGCGenerationInfo();

            // Create long-lived objects
            Console.WriteLine("\nCreating long-lived objects...");
            var longLivedObjects = CreateLongLivedObjects();

            // Trigger multiple collections to show generational promotion
            Console.WriteLine("\nTriggering multiple collections to demonstrate promotion...");
            for (int i = 0; i < 3; i++)
            {
                CreateShortLivedObjects(); // Create more Gen 0 objects
                GC.Collect(0); // Collect only Generation 0
                Console.WriteLine($"After Gen 0 collection #{i + 1}:");
                PrintGCGenerationInfo();
            }

            // Keep long-lived objects alive to show they survive collections
            Console.WriteLine($"Long-lived objects still alive: {longLivedObjects.Count}");
        }

        /// <summary>
        /// Creates many short-lived objects that will quickly become eligible for collection.
        /// These typically stay in Generation 0.
        /// </summary>
        static void CreateShortLivedObjects()
        {
            for (int i = 0; i < 1000; i++)
            {
                // Create temporary objects that immediately become unreachable
                var temp = new byte[100];
                var tempString = $"Temporary string {i}";
                // No references stored - these become garbage immediately
            }
        }

        /// <summary>
        /// Creates objects that will survive multiple GC cycles and promote to higher generations.
        /// </summary>
        static List<SampleObject> CreateLongLivedObjects()
        {
            var objects = new List<SampleObject>();
            
            for (int i = 0; i < 100; i++)
            {
                objects.Add(new SampleObject($"Long-lived object {i}"));
            }
            
            return objects;
        }

        /// <summary>
        /// Shows information about garbage collection across all generations.
        /// </summary>
        static void PrintGCGenerationInfo()
        {
            Console.WriteLine($"  Gen 0 collections: {GC.CollectionCount(0)}");
            Console.WriteLine($"  Gen 1 collections: {GC.CollectionCount(1)}");
            Console.WriteLine($"  Gen 2 collections: {GC.CollectionCount(2)}");
            Console.WriteLine($"  Total memory: {GC.GetTotalMemory(false):N0} bytes");
        }

        /// <summary>
        /// Demonstrates how circular references are handled by the garbage collector.
        /// Shows that circular references don't prevent collection if no roots exist.
        /// </summary>
        static void DemoCircularReferences()
        {
            Console.WriteLine("Creating objects with circular references...");

            // Create objects that reference each other in a circle
            CreateCircularReferences();

            Console.WriteLine("Circular reference chain created and became unreachable");
            Console.WriteLine("Despite circular references, objects are eligible for collection");

            // Show that GC can handle circular references
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before GC: {memoryBefore:N0} bytes");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryAfter = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory after GC: {memoryAfter:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {memoryBefore - memoryAfter:N0} bytes");
            Console.WriteLine("✓ Circular references were successfully collected!");
        }

        /// <summary>
        /// Creates a chain of objects with circular references but no root reference.
        /// </summary>
        static void CreateCircularReferences()
        {
            var obj1 = new CircularReferenceObject("Object 1");
            var obj2 = new CircularReferenceObject("Object 2");
            var obj3 = new CircularReferenceObject("Object 3");

            // Create circular references
            obj1.Reference = obj2;
            obj2.Reference = obj3;
            obj3.Reference = obj1; // Completes the circle

            Console.WriteLine("Created circular reference: obj1 -> obj2 -> obj3 -> obj1");

            // No root variables store references after method exits
            // Despite circular references, all objects become unreachable
        }

        /// <summary>
        /// Demonstrates memory monitoring using process information.
        /// Shows how to track memory usage programmatically.
        /// </summary>
        static void DemoMemoryMonitoring()
        {
            Console.WriteLine("Monitoring memory usage...");

            // Get current process for monitoring
            Process currentProcess = Process.GetCurrentProcess();
            
            // Show initial memory state
            PrintMemoryInfo(currentProcess, "Initial state");

            // Allocate significant memory
            Console.WriteLine("Allocating large amounts of memory...");
            var memoryHogs = new List<byte[]>();
            
            for (int i = 0; i < 10; i++)
            {
                memoryHogs.Add(new byte[1_000_000]); // 1MB each
                if (i % 3 == 0)
                {
                    PrintMemoryInfo(currentProcess, $"After allocating {(i + 1)} MB");
                }
            }

            // Clear references and force collection
            Console.WriteLine("Clearing references and forcing garbage collection...");
            memoryHogs.Clear();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            PrintMemoryInfo(currentProcess, "After garbage collection");
        }

        /// <summary>
        /// Prints detailed memory information for the current process.
        /// </summary>
        static void PrintMemoryInfo(Process process, string context)
        {
            // Refresh process information
            process.Refresh();
            
            Console.WriteLine($"Memory info - {context}:");
            Console.WriteLine($"  Working Set: {process.WorkingSet64 / 1024 / 1024:N0} MB");
            Console.WriteLine($"  Private Memory: {process.PrivateMemorySize64 / 1024 / 1024:N0} MB");
            Console.WriteLine($"  GC Total Memory: {GC.GetTotalMemory(false) / 1024 / 1024:N0} MB");
        }

        /// <summary>
        /// Demonstrates the Large Object Heap (LOH) behavior.
        /// Objects >= 85KB go to LOH and have different collection characteristics.
        /// </summary>
        static void DemoLargeObjectHeap()
        {
            Console.WriteLine("Exploring Large Object Heap (LOH) behavior...");
            
            const int smallObjectSize = 1000;      // 1KB - goes to regular heap
            const int largeObjectSize = 100_000;   // 100KB - goes to LOH

            Console.WriteLine($"Creating small objects ({smallObjectSize} bytes each)...");
            var smallObjects = new List<byte[]>();
            
            for (int i = 0; i < 100; i++)
            {
                smallObjects.Add(new byte[smallObjectSize]);
            }

            Console.WriteLine($"Creating large objects ({largeObjectSize} bytes each)...");
            var largeObjects = new List<byte[]>();
            
            for (int i = 0; i < 20; i++)
            {
                largeObjects.Add(new byte[largeObjectSize]);
            }

            Console.WriteLine("Large objects (>=85KB) are allocated on the Large Object Heap");
            Console.WriteLine("LOH is collected less frequently and only during Gen 2 collections");

            PrintGCGenerationInfo();

            // Clear small objects first
            Console.WriteLine("\nClearing small objects and forcing Gen 0/1 collection...");
            smallObjects.Clear();
            GC.Collect(1); // Collect Gen 0 and 1 only

            Console.WriteLine("After Gen 0/1 collection (LOH objects should remain):");
            PrintGCGenerationInfo();

            // Clear large objects and force full collection
            Console.WriteLine("\nClearing large objects and forcing full collection...");
            largeObjects.Clear();
            GC.Collect(); // Full collection includes LOH

            Console.WriteLine("After full collection (LOH objects should be collected):");
            PrintGCGenerationInfo();
        }

        /// <summary>
        /// Demonstrates memory pressure and what triggers garbage collection.
        /// Shows how allocation patterns affect GC behavior.
        /// </summary>
        static void DemoMemoryPressure()
        {
            Console.WriteLine("Demonstrating memory pressure and GC triggers...");

            // Monitor collection counts
            int gen0Before = GC.CollectionCount(0);
            int gen1Before = GC.CollectionCount(1);
            int gen2Before = GC.CollectionCount(2);

            Console.WriteLine("Creating memory pressure through rapid allocation...");

            // Create memory pressure by rapidly allocating and abandoning objects
            for (int i = 0; i < 10000; i++)
            {
                // Rapid allocation creates pressure on Generation 0
                var tempArray = new byte[1000];
                var tempString = new string('x', 100);
                
                // Every 1000 iterations, show current state
                if (i % 2000 == 0 && i > 0)
                {
                    int gen0Current = GC.CollectionCount(0);
                    Console.WriteLine($"After {i} allocations: Gen 0 collections: {gen0Current - gen0Before}");
                }
            }

            // Show final collection counts
            Console.WriteLine("\nFinal garbage collection statistics:");
            Console.WriteLine($"Gen 0 collections triggered: {GC.CollectionCount(0) - gen0Before}");
            Console.WriteLine($"Gen 1 collections triggered: {GC.CollectionCount(1) - gen1Before}");
            Console.WriteLine($"Gen 2 collections triggered: {GC.CollectionCount(2) - gen2Before}");

            Console.WriteLine("\nKey insights:");
            Console.WriteLine("- Rapid allocation triggers frequent Gen 0 collections");
            Console.WriteLine("- Gen 1 collections happen less frequently");
            Console.WriteLine("- Gen 2 collections are rare and expensive");
        }

        /// <summary>
        /// Demonstrates weak references and their behavior with garbage collection.
        /// Shows how weak references don't keep objects alive.
        /// </summary>
        static void DemoWeakReferences()
        {
            Console.WriteLine("Exploring weak references and their behavior...");

            // Create an object with both strong and weak references
            var strongRef = new SampleObject("Weakly Referenced Object");
            var weakRef = new WeakReference(strongRef);

            Console.WriteLine($"Created object with strong and weak references");
            Console.WriteLine($"Strong reference: {strongRef.Name}");
            Console.WriteLine($"Weak reference target: {(weakRef.Target as SampleObject)?.Name ?? "null"}");
            Console.WriteLine($"Weak reference is alive: {weakRef.IsAlive}");

            // Remove strong reference but keep weak reference
            Console.WriteLine("\nRemoving strong reference...");
            strongRef = null!;

            // Object might still be alive (GC hasn't run yet)
            Console.WriteLine($"Weak reference is alive: {weakRef.IsAlive}");
            Console.WriteLine($"Weak reference target: {(weakRef.Target as SampleObject)?.Name ?? "null"}");

            // Force garbage collection
            Console.WriteLine("Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Now the weak reference should be dead
            Console.WriteLine($"After GC - Weak reference is alive: {weakRef.IsAlive}");
            Console.WriteLine($"Weak reference target: {(weakRef.Target as SampleObject)?.Name ?? "null"}");

            Console.WriteLine("\nKey insight: Weak references don't prevent garbage collection");
            Console.WriteLine("They're useful for caches and observers that shouldn't keep objects alive");
        }

        /// <summary>
        /// Demonstrates finalization patterns and the interaction between GC and IDisposable.
        /// Shows the overhead of finalization and benefits of proper disposal.
        /// </summary>
        static void DemoFinalizationAndDisposal()
        {
            Console.WriteLine("Exploring finalization and disposal patterns...");

            // Demo proper disposal
            FinalizationExample.DemoProperDisposal();

            // Demo finalization impact
            FinalizationExample.DemoFinalizationImpact();

            Console.WriteLine("\nKey insights about finalization:");
            Console.WriteLine("1. Objects with finalizers require TWO GC cycles to be fully collected");
            Console.WriteLine("2. Finalizers add significant overhead to garbage collection");
            Console.WriteLine("3. Always implement IDisposable for deterministic cleanup");
            Console.WriteLine("4. Use 'using' statements or call Dispose() explicitly");
            Console.WriteLine("5. Call GC.SuppressFinalize() in Dispose() to avoid finalizer overhead");
        }
    }
}
