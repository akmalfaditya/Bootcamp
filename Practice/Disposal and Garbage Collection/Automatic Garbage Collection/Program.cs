using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// Comprehensive demonstration of automatic garbage collection in .NET.
    /// This program covers all essential concepts from the training material:
    /// - Object lifecycles and memory allocation on the heap
    /// - The indeterminate nature of garbage collection
    /// - Roots and reachability analysis
    /// - Generational garbage collection and the generational hypothesis
    /// - Circular references and why they don't cause memory leaks
    /// - Memory monitoring and programmatic analysis
    /// - Factors that trigger garbage collection
    /// - Large Object Heap (LOH) behavior
    /// - Weak references and their relationship to GC
    /// - Finalization and the interaction with disposal patterns
    /// 
    /// Each demonstration includes detailed explanations and practical examples
    /// to help you understand memory management in .NET applications.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== AUTOMATIC GARBAGE COLLECTION IN .NET ===");
            Console.WriteLine("Comprehensive demonstration of memory management concepts");
            Console.WriteLine("Based on the foundational principles of the .NET CLR");
            Console.WriteLine();
            
            // Fundamental concepts from the training material
            Console.WriteLine("=== PART 1: FUNDAMENTAL CONCEPTS ===");
            
            // Demo 1: The basic Test() method example from training material
            Console.WriteLine("1. Basic Memory Allocation and Object Lifecycle");
            Console.WriteLine("   (The Test() method example from training material)");
            DemoBasicMemoryAllocation();
            Console.WriteLine();
            
            // Demo 2: Indeterminate nature of garbage collection
            Console.WriteLine("2. The Indeterminate Nature of Garbage Collection");
            BasicMemoryExample.DemoIndeterminateCollection();
            Console.WriteLine();
            
            // Demo 3: Debug vs Release behavior
            Console.WriteLine("3. Debug vs Release Object Lifetime");
            BasicMemoryExample.DemoDebugVsReleaseBehavior();
            Console.WriteLine();
            
            // Demo 4: Memory consumption balance
            Console.WriteLine("4. GC Memory Consumption Balance");
            BasicMemoryExample.DemoMemoryConsumptionBalance();
            Console.WriteLine();
            
            // Demo 5: Programmatic memory monitoring
            Console.WriteLine("5. Programmatic Memory Monitoring");
            BasicMemoryExample.DemoMemoryMonitoring();
            Console.WriteLine();
            
            Console.WriteLine("=== PART 2: ROOTS AND REACHABILITY ===");
            
            // Demo 6: Comprehensive roots demonstration
            Console.WriteLine("6. All Types of Roots (Complete Coverage)");
            RootsAndReachabilityDemo.DemoAllRootTypes();
            Console.WriteLine();
            
            // Demo 7: Reachability scenarios
            Console.WriteLine("7. Reachability Scenarios");
            RootsAndReachabilityDemo.DemoReachabilityScenarios();
            Console.WriteLine();
            
            // Demo 8: Instance method reachability
            Console.WriteLine("8. Instance Method Reachability");
            RootsAndReachabilityDemo.DemoInstanceMethodReachability();
            Console.WriteLine();
            
            Console.WriteLine("=== PART 3: GENERATIONAL GARBAGE COLLECTION ===");
            
            // Demo 9: Complete generational system
            Console.WriteLine("9. Generational Garbage Collection System");
            GenerationalGCDemo.DemoGenerationalSystem();
            Console.WriteLine();
            
            // Demo 10: Indeterminate delay
            Console.WriteLine("10. Indeterminate Collection Delay");
            GenerationalGCDemo.DemoIndeterminateDelay();
            Console.WriteLine();
            
            Console.WriteLine("=== PART 4: ADVANCED CONCEPTS ===");
            
            // Demo 11: Circular references (from training material)
            Console.WriteLine("11. Circular References and GC");
            DemoCircularReferences();
            Console.WriteLine();
            
            // Demo 12: Large Object Heap
            Console.WriteLine("12. Large Object Heap (LOH) Behavior");
            DemoLargeObjectHeap();
            Console.WriteLine();
            
            // Demo 13: Memory pressure and triggers
            Console.WriteLine("13. Memory Pressure and Collection Triggers");
            DemoMemoryPressure();
            Console.WriteLine();
            
            // Demo 14: Weak references
            Console.WriteLine("14. Weak References and GC");
            DemoWeakReferences();
            Console.WriteLine();
            
            // Demo 15: Finalization and disposal
            Console.WriteLine("15. Finalization and Disposal Patterns");
            DemoFinalizationAndDisposal();
            Console.WriteLine();
            
            Console.WriteLine("=== DEMONSTRATION COMPLETE ===");
            Console.WriteLine();
            Console.WriteLine("KEY TAKEAWAYS FROM THIS DEMONSTRATION:");
            Console.WriteLine("• Memory management in .NET is fully automatic");
            Console.WriteLine("• Objects are allocated on the heap, references on the stack");
            Console.WriteLine("• Garbage collection is indeterminate - eligibility ≠ immediate collection");
            Console.WriteLine("• The GC traces from roots to determine reachability");
            Console.WriteLine("• Generational collection optimizes for object lifetime patterns");
            Console.WriteLine("• Circular references don't prevent collection in .NET");
            Console.WriteLine("• The GC self-tunes based on application behavior");
            Console.WriteLine("• Understanding these concepts helps write memory-efficient code");
            Console.WriteLine();
            Console.WriteLine("IMPORTANT: Never call GC.Collect() in production code!");
            Console.WriteLine("The GC is smarter than we are and knows when to run optimally.");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Demonstrates the exact Test() method example from the training material.
        /// This is the fundamental example that shows how automatic memory management works.
        /// </summary>
        static void DemoBasicMemoryAllocation()
        {
            Console.WriteLine("This demonstrates the exact Test() method from the training material:");
            Console.WriteLine("---");
            Console.WriteLine("public void Test()");
            Console.WriteLine("{");
            Console.WriteLine("    byte[] myArray = new byte[1000]; // Memory allocated on the heap");
            Console.WriteLine("    // ... use myArray ...");
            Console.WriteLine("} // Method exits");
            Console.WriteLine("---");
            Console.WriteLine();
            
            Console.WriteLine("Let's trace through what happens:");
            Console.WriteLine("1. Method executes, array allocated on managed heap");
            Console.WriteLine("2. myArray variable (reference) stored on local variable stack");
            Console.WriteLine("3. Array is reachable through myArray reference");
            Console.WriteLine("4. Method completes, myArray pops out of scope");
            Console.WriteLine("5. No active reference points to the array");
            Console.WriteLine("6. Array becomes 'orphaned' or 'unreachable'");
            Console.WriteLine("7. Array is now eligible for garbage collection");
            Console.WriteLine();
            
            Console.WriteLine("Executing the actual Test() method:");
            BasicMemoryExample.Test();
            Console.WriteLine("Method completed - array is now eligible for collection");
            Console.WriteLine();
            
            Console.WriteLine("Key insight: Eligibility ≠ immediate collection!");
            Console.WriteLine("The CLR performs collections periodically based on various factors.");
        }

        /// <summary>
        /// Demonstrates how circular references are handled by the garbage collector.
        /// Shows that circular references don't prevent collection if no roots exist.
        /// This directly addresses the common misconception mentioned in the training material.
        /// </summary>
        static void DemoCircularReferences()
        {
            Console.WriteLine("Creating objects with circular references...");
            Console.WriteLine("This addresses the common misconception that circular references prevent collection");
            Console.WriteLine();

            // Create objects that reference each other in a circle
            CreateCircularReferences();

            Console.WriteLine("Circular reference chain created and became unreachable");
            Console.WriteLine("Despite circular references, objects are eligible for collection");
            Console.WriteLine();
            Console.WriteLine("This is because .NET uses a TRACING garbage collector, not reference counting");
            Console.WriteLine("The GC traces from roots - if no path exists from any root, objects are collected");

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
            Console.WriteLine();
            Console.WriteLine("Key insight: Circular references are not a problem in .NET");
            Console.WriteLine("The GC only collects objects that cannot be reached from roots");
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

            Console.WriteLine("Created circular reference: obj1 → obj2 → obj3 → obj1");

            // No root variables store references after method exits
            // Despite circular references, all objects become unreachable
        }

        /// <summary>
        /// Demonstrates the Large Object Heap (LOH) behavior.
        /// Objects >= 85KB go to LOH and have different collection characteristics.
        /// </summary>
        static void DemoLargeObjectHeap()
        {
            Console.WriteLine("Exploring Large Object Heap (LOH) behavior...");
            Console.WriteLine("Objects >= 85KB are allocated on the Large Object Heap");
            Console.WriteLine("LOH has different collection characteristics than regular heap");
            Console.WriteLine();
            
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
            Console.WriteLine("This shows the three factors mentioned in the training material:");
            Console.WriteLine("1. Available Memory");
            Console.WriteLine("2. Amount of Memory Allocation");
            Console.WriteLine("3. Time Since Last Collection");
            Console.WriteLine();

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
                
                // Every 2000 iterations, show current state
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
            Console.WriteLine("- The GC dynamically self-tunes based on these patterns");
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
    }
}
