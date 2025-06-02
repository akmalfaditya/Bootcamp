using System;
using System.Diagnostics;
using System.Threading;

namespace Finalizers
{
    /// <summary>
    /// This program demonstrates how finalizers work in C# and their impact on garbage collection.
    /// We'll explore proper finalizer implementation, performance implications, and best practices.
    /// Remember: finalizers should be used sparingly and only when absolutely necessary!
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Finalizers Demonstration ===\n");

            // Demo 1: Basic finalizer behavior
            Console.WriteLine("1. Basic Finalizer Behavior:");
            DemoBasicFinalizer();
            Console.WriteLine();

            // Demo 2: Finalizer execution timing
            Console.WriteLine("2. Finalizer Execution Timing:");
            DemoFinalizerTiming();
            Console.WriteLine();

            // Demo 3: Performance impact of finalizers
            Console.WriteLine("3. Performance Impact of Finalizers:");
            DemoPerformanceImpact();
            Console.WriteLine();

            // Demo 4: Finalizer best practices
            Console.WriteLine("4. Finalizer Best Practices:");
            DemoBestPractices();
            Console.WriteLine();            // Demo 5: Common finalizer mistakes
            Console.WriteLine("5. Common Finalizer Mistakes (What NOT to do):");
            DemoCommonMistakes();
            Console.WriteLine();

            // Demo 6: Finalizers with Dispose pattern
            Console.WriteLine("6. Finalizers with Dispose Pattern:");
            DemoAdvancedPattern();
            Console.WriteLine();

            // Demo 7: Finalizer order and dependencies
            Console.WriteLine("7. Finalizer Order and Dependencies:");
            DemoFinalizerOrder();
            Console.WriteLine();

            Console.WriteLine("=== Demo Complete ===");
            Console.WriteLine("Key takeaway: Use finalizers only when absolutely necessary!");
            Console.WriteLine("Always prefer the Dispose pattern for deterministic cleanup.");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates basic finalizer behavior and execution.
        /// Shows how finalizers are called during garbage collection.
        /// </summary>
        static void DemoBasicFinalizer()
        {
            Console.WriteLine("Creating objects with finalizers...");

            // Create some objects with finalizers in a local scope
            CreateFinalizableObjects();

            Console.WriteLine("Objects went out of scope - now eligible for collection");
            Console.WriteLine("But finalizers haven't run yet...");

            // Force garbage collection to trigger finalizers
            Console.WriteLine("Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers(); // Wait for finalizers to complete
            GC.Collect(); // Second collection to actually free the memory

            Console.WriteLine("Finalizers should have executed by now");
        }

        /// <summary>
        /// Helper method that creates objects with finalizers in a local scope.
        /// When this method exits, the objects become eligible for garbage collection.
        /// </summary>
        static void CreateFinalizableObjects()
        {
            var obj1 = new BasicFinalizerExample("Object1");
            var obj2 = new BasicFinalizerExample("Object2");
            var obj3 = new BasicFinalizerExample("Object3");

            Console.WriteLine("Created 3 objects with finalizers");
            // Objects become unreachable when method exits
        }

        /// <summary>
        /// Demonstrates the timing and unpredictability of finalizer execution.
        /// Shows that you cannot control when finalizers run.
        /// </summary>
        static void DemoFinalizerTiming()
        {
            Console.WriteLine("Creating objects and monitoring finalizer timing...");

            // Create objects with timestamps to track finalizer delay
            for (int i = 0; i < 5; i++)
            {
                new TimedFinalizerExample($"TimedObject_{i}");
            }

            Console.WriteLine("All objects created and abandoned");
            Console.WriteLine("Waiting to see when finalizers execute...");

            // Wait a bit without forcing GC to show natural timing
            Thread.Sleep(1000);
            Console.WriteLine("After 1 second - checking if finalizers ran naturally...");

            // Now force GC to see the difference
            Console.WriteLine("Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Notice: Finalizers don't run immediately when objects become unreachable");
            Console.WriteLine("They only run when the GC decides to collect, which you can't control");
        }

        /// <summary>
        /// Demonstrates the performance impact of having finalizers.
        /// Shows how objects with finalizers take longer to be fully collected.
        /// </summary>
        static void DemoPerformanceImpact()
        {
            Console.WriteLine("Comparing performance: objects with vs without finalizers");

            // Measure time to create and collect objects WITHOUT finalizers
            var stopwatch = Stopwatch.StartNew();
            CreateObjectsWithoutFinalizers(1000);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            stopwatch.Stop();

            long timeWithoutFinalizers = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Time for 1000 objects WITHOUT finalizers: {timeWithoutFinalizers} ms");

            // Measure time to create and collect objects WITH finalizers
            stopwatch.Restart();
            CreateObjectsWithFinalizers(1000);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            stopwatch.Stop();

            long timeWithFinalizers = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Time for 1000 objects WITH finalizers: {timeWithFinalizers} ms");

            Console.WriteLine($"Performance difference: {timeWithFinalizers - timeWithoutFinalizers} ms slower with finalizers");
            Console.WriteLine("Objects with finalizers require TWO garbage collection cycles!");
        }

        /// <summary>
        /// Creates objects without finalizers for performance comparison.
        /// </summary>
        static void CreateObjectsWithoutFinalizers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                new SimpleObject($"NoFinalizer_{i}");
            }
        }

        /// <summary>
        /// Creates objects with finalizers for performance comparison.
        /// </summary>
        static void CreateObjectsWithFinalizers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                new BasicFinalizerExample($"WithFinalizer_{i}");
            }
        }

        /// <summary>
        /// Demonstrates best practices for implementing finalizers.
        /// Shows how to write efficient, safe finalizers.
        /// </summary>
        static void DemoBestPractices()
        {
            Console.WriteLine("Demonstrating proper finalizer implementation...");

            // Show good finalizer implementation
            var goodExample = new GoodFinalizerExample("ProperImplementation");
            goodExample.DoSomeWork();

            // Clear the reference
            goodExample = null!;

            Console.WriteLine("Object reference cleared - finalizer will run during GC");

            // Force collection to show proper finalizer behavior
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("Notice how the good finalizer:");
            Console.WriteLine("- Executes quickly");
            Console.WriteLine("- Doesn't throw exceptions");
            Console.WriteLine("- Only cleans up unmanaged resources");
            Console.WriteLine("- Uses try-catch for safety");
        }

        /// <summary>
        /// Demonstrates common mistakes people make with finalizers.
        /// Shows what NOT to do when implementing finalizers.
        /// </summary>
        static void DemoCommonMistakes()
        {
            Console.WriteLine("WARNING: The following examples show BAD practices!");
            Console.WriteLine("We're demonstrating what NOT to do with finalizers");

            // Note: In a real application, you would never implement finalizers like this
            Console.WriteLine("Creating object with poorly implemented finalizer...");

            var badExample = new BadFinalizerExample("BadImplementation");
            badExample = null!;

            Console.WriteLine("Forcing GC to show problems with bad finalizer implementation...");

            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during finalization: {ex.Message}");
            }

            Console.WriteLine("The bad finalizer demonstrates common mistakes:");
            Console.WriteLine("- Taking too long to execute");
            Console.WriteLine("- Potentially throwing exceptions");
            Console.WriteLine("- Trying to access other objects");
            Console.WriteLine("- Performing complex operations");
        }

        /// <summary>
        /// Demonstrates the advanced finalizer pattern combined with IDisposable.
        /// Shows how finalizers work as a safety net when Dispose isn't called.
        /// </summary>
        static void DemoAdvancedPattern()
        {
            Console.WriteLine("Demonstrating finalizer + Dispose pattern...");
            AdvancedFinalizerExample.DemonstrateDisposalPatterns();
            
            Console.WriteLine("\nKey insights about the Dispose + Finalizer pattern:");
            Console.WriteLine("- Finalizer acts as a safety net if Dispose() isn't called");
            Console.WriteLine("- Dispose() calls GC.SuppressFinalize() to skip the finalizer");
            Console.WriteLine("- This provides both deterministic cleanup AND a backup mechanism");
            Console.WriteLine("- Always prefer calling Dispose() explicitly");
        }

        /// <summary>
        /// Demonstrates issues with finalizer execution order and object dependencies.
        /// Shows why finalizers shouldn't access other finalizable objects.
        /// </summary>
        static void DemoFinalizerOrder()
        {
            Console.WriteLine("Demonstrating finalizer order unpredictability...");
            FinalizerOrderExample.DemonstrateFinalizationOrder();
            
            Console.WriteLine("\nKey insights about finalizer order:");
            Console.WriteLine("- Finalizer execution order is NOT guaranteed");
            Console.WriteLine("- Don't access other finalizable objects in finalizers");
            Console.WriteLine("- Dependencies might be finalized before objects that need them");
            Console.WriteLine("- Keep finalizers simple and self-contained");
        }
    }
}
