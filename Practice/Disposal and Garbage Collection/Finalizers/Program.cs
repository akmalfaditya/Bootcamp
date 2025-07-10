using System;
using System.Diagnostics;
using System.Threading;

namespace Finalizers
{
    /// <summary>
    /// Comprehensive demonstration of finalizers in C#.
    /// This program covers all aspects of finalizer behavior, best practices, and common pitfalls.
    /// 
    /// Key takeaways:
    /// - Finalizers are a last-resort safety net for unmanaged resource cleanup
    /// - They introduce performance overhead and should be used sparingly
    /// - The Dispose pattern is the recommended approach for deterministic cleanup
    /// - Finalizer execution is unpredictable in timing and order
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Finalizers: Complete Guide ===\n");

            bool interactive = args.Length == 0 || !args[0].Equals("--non-interactive", StringComparison.OrdinalIgnoreCase);

            // Demo 1: Basic finalizer syntax and behavior
            Console.WriteLine("1. Basic Finalizer Syntax and Behavior:");
            DemoBasicFinalizer();
            if (interactive) WaitForUser();

            // Demo 2: How finalizers work with garbage collection phases
            Console.WriteLine("2. Finalizer Execution Phases:");
            DemoFinalizerPhases();
            if (interactive) WaitForUser();

            // Demo 3: Performance impact and object lifetime extension
            Console.WriteLine("3. Performance Impact and Object Lifetime:");
            DemoPerformanceImpact();
            if (interactive) WaitForUser();

            // Demo 4: Proper dispose pattern implementation
            Console.WriteLine("4. Proper Dispose Pattern with Finalizers:");
            DemoDisposePattern();
            if (interactive) WaitForUser();

            // Demo 5: Object resurrection scenarios
            Console.WriteLine("5. Object Resurrection:");
            DemoObjectResurrection();
            if (interactive) WaitForUser();

            // Demo 6: GC.ReRegisterForFinalize usage
            Console.WriteLine("6. Re-registering for Finalization:");
            DemoReRegisterForFinalize();
            if (interactive) WaitForUser();

            // Demo 7: Common finalizer mistakes and how to avoid them
            Console.WriteLine("7. Common Finalizer Mistakes:");
            DemoCommonMistakes();
            if (interactive) WaitForUser();

            // Demo 8: Finalizer order unpredictability
            Console.WriteLine("8. Finalizer Order Unpredictability:");
            DemoFinalizerOrder();
            if (interactive) WaitForUser();

            Console.WriteLine("=== Demo Complete ===");
            Console.WriteLine("Key takeaway: Use finalizers only when absolutely necessary!");
            Console.WriteLine("Always prefer the Dispose pattern for deterministic cleanup.");
            
            if (interactive)
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\nDemo completed in non-interactive mode.");
            }
        }

        static void WaitForUser()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates basic finalizer behavior and execution.
        /// Shows the fundamental syntax and when finalizers get called.
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
        /// Demonstrates the multi-phase garbage collection process with finalizers.
        /// Shows how GC handles objects with finalizers differently.
        /// </summary>
        static void DemoFinalizerPhases()
        {
            Console.WriteLine("Understanding GC phases with finalizers:");
            Console.WriteLine("Phase 1: Mark objects for collection");
            Console.WriteLine("Phase 2: Objects with finalizers go to finalization queue");
            Console.WriteLine("Phase 3: Finalizer thread runs finalizers");
            Console.WriteLine("Phase 4: Next GC cycle actually frees memory");
            Console.WriteLine();

            Console.WriteLine("Creating objects to demonstrate phases...");
            for (int i = 0; i < 3; i++)
            {
                new GCPhaseDemo($"Phase_Object_{i}");
            }

            Console.WriteLine("\nStep 1: First GC.Collect() - objects marked for finalization");
            GC.Collect();
            
            Console.WriteLine("\nStep 2: WaitForPendingFinalizers() - finalizers execute");
            GC.WaitForPendingFinalizers();
            
            Console.WriteLine("\nStep 3: Second GC.Collect() - memory actually freed");
            GC.Collect();
            
            Console.WriteLine("\nThis two-phase process is why finalizers have performance overhead!");
        }

        /// <summary>
        /// Demonstrates the performance impact of having finalizers.
        /// Shows how objects with finalizers live longer and impact GC performance.
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

            Console.WriteLine($"Performance difference: {timeWithFinalizers - timeWithoutFinalizers} ms slower");
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
        /// Demonstrates the proper Dispose pattern with finalizers.
        /// Shows how finalizers act as a safety net when Dispose isn't called.
        /// </summary>
        static void DemoDisposePattern()
        {
            Console.WriteLine("Demonstrating the Dispose pattern with finalizers:");
            Console.WriteLine();

            // Scenario 1: Proper disposal - finalizer suppressed
            Console.WriteLine("Scenario 1: Proper disposal");
            using (var properDisposal = new ProperDisposalExample("ProperlyDisposed"))
            {
                properDisposal.DoWork();
            } // Dispose called automatically here
            
            Console.WriteLine();

            // Scenario 2: Improper disposal - finalizer runs as safety net
            Console.WriteLine("Scenario 2: Improper disposal (finalizer as safety net)");
            var improperDisposal = new ProperDisposalExample("ImproperlyDisposed");
            improperDisposal.DoWork();
            improperDisposal = null; // Not disposed properly!
            
            Console.WriteLine("Forcing GC to show finalizer safety net...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// Demonstrates object resurrection - when finalizers make objects reachable again.
        /// Shows how objects can "come back to life" during finalization.
        /// </summary>
        static void DemoObjectResurrection()
        {
            Console.WriteLine("Demonstrating object resurrection:");
            Console.WriteLine("When finalizers make objects reachable again...");
            Console.WriteLine();

            // Create a temporary file that might fail to delete
            var tempFile = new TempFileRef("test_file.tmp");
            tempFile = null; // Make it eligible for collection

            Console.WriteLine("Object eligible for collection...");
            Console.WriteLine("Forcing GC - finalizer will try to delete file...");
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Check if any objects were resurrected
            Console.WriteLine($"Objects in failed deletion queue: {TempFileRef.FailedDeletions.Count}");
            
            // Try to process failed deletions
            Console.WriteLine("Processing failed deletions...");
            TempFileRef.ProcessFailedDeletions();
        }

        /// <summary>
        /// Demonstrates GC.ReRegisterForFinalize for retry scenarios.
        /// Shows how to make finalizers run multiple times.
        /// </summary>
        static void DemoReRegisterForFinalize()
        {
            Console.WriteLine("Demonstrating GC.ReRegisterForFinalize:");
            Console.WriteLine("Making finalizers run multiple times for retry scenarios...");
            Console.WriteLine();

            var retryFile = new RetryTempFileRef("retry_file.tmp");
            retryFile = null; // Make it eligible for collection

            Console.WriteLine("Forcing multiple GC cycles to show retry mechanism...");
            
            // Force multiple GC cycles to demonstrate retry
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"GC cycle {i + 1}:");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                Thread.Sleep(100); // Small delay between cycles
            }
        }

        /// <summary>
        /// Demonstrates common mistakes people make with finalizers.
        /// Shows what NOT to do when implementing finalizers.
        /// </summary>
        static void DemoCommonMistakes()
        {
            Console.WriteLine("WARNING: The following examples show BAD practices!");
            Console.WriteLine("We're demonstrating what NOT to do with finalizers");
            Console.WriteLine();

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
        /// Demonstrates issues with finalizer execution order and object dependencies.
        /// Shows why finalizers shouldn't access other finalizable objects.
        /// </summary>
        static void DemoFinalizerOrder()
        {
            Console.WriteLine("Demonstrating finalizer order unpredictability:");
            Console.WriteLine("You cannot predict the order in which finalizers execute!");
            Console.WriteLine();

            // Create interdependent objects to show order issues
            var container = new FinalizerContainer("Container");
            var item1 = new FinalizerItem("Item1");
            var item2 = new FinalizerItem("Item2");
            var item3 = new FinalizerItem("Item3");

            container.AddItem(item1);
            container.AddItem(item2);
            container.AddItem(item3);

            // Clear references
            container = null;
            item1 = null;
            item2 = null;
            item3 = null;

            Console.WriteLine("All references cleared - watch the finalization order:");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("Notice: The order is unpredictable!");
            Console.WriteLine("This is why finalizers shouldn't depend on each other.");
        }
    }

    /// <summary>
    /// Simple object without finalizer for performance comparison.
    /// Used to demonstrate the performance difference between objects with and without finalizers.
    /// </summary>
    public class SimpleObject
    {
        private readonly string _name;

        public SimpleObject(string name)
        {
            _name = name;
        }
    }
}
