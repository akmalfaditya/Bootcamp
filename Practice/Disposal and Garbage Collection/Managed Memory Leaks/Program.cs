// Managed Memory Leaks in C# - Training Project
// This project demonstrates how memory leaks occur in managed code
// Even with garbage collection, you can still leak memory!

using System.Timers;

namespace ManagedMemoryLeaks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== MANAGED MEMORY LEAKS TRAINING ===");
            Console.WriteLine("=====================================\n");
            
            Console.WriteLine("IMPORTANT: In unmanaged languages like C++, developers manually manage memory.");
            Console.WriteLine("In .NET, the CLR's Garbage Collector prevents traditional memory leaks.");
            Console.WriteLine("However, large .NET applications can still exhibit memory leaks!");
            Console.WriteLine();
            Console.WriteLine("CAUSE: Unused objects stay alive due to forgotten or unintended references.");
            Console.WriteLine("The GC cannot collect objects if there's still a 'root' pointing to them.\n");
            
            // Show initial memory state
            long initialMemory = GC.GetTotalMemory(true);
            Console.WriteLine($"Starting memory usage: {initialMemory:N0} bytes\n");
            
            // Run all demonstrations
            DemonstrateExactMaterialExamples();
            DemonstrateEventHandlerLeaks();
            DemonstrateTimerLeaks();
            DemonstrateThreadingTimerBehavior();
            DemonstrateStaticReferenceLeaks();
            DemonstrateWeakReferencesSolution();
            DemonstrateProperCleanupPatterns();
            DemonstrateMemoryDiagnostics();
            
            Console.WriteLine("\n=== TRAINING COMPLETE ===");
            Console.WriteLine("You've learned the most common managed memory leak patterns!");
            Console.WriteLine("Remember: The GC is smart, but it's not magic - it needs your help!");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        
        static void DemonstrateExactMaterialExamples()
        {
            Console.WriteLine("=== EXACT EXAMPLES FROM TRAINING MATERIAL ===\n");
            
            // Run the exact Host/Client example from the material
            MaterialExampleRunner.RunHostClientExample();
            
            // Run the exact Timer example from the material
            MaterialExampleRunner.RunTimerExample();
            
            // Run the exact Threading Timer example from the material
            ThreadingTimerExamples.DemonstrateThreadingTimerBehavior();
            ThreadingTimerExamples.DemonstrateProperThreadingTimer();
            
            Console.WriteLine("=== END OF EXACT MATERIAL EXAMPLES ===\n");
        }
        
        static void DemonstrateEventHandlerLeaks()
        {
            Console.WriteLine("DEMO 1: Event Handler Memory Leaks");
            Console.WriteLine("===================================");
            
            Console.WriteLine("THE PROBLEM: Event handlers create references from publisher to subscriber.");
            Console.WriteLine("Unless the handler is static, the publisher holds a strong reference to the subscriber.");
            Console.WriteLine("This is THE most common cause of memory leaks in C# applications!\n");
            
            // Show memory before creating objects
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before creating event subscribers: {memoryBefore:N0} bytes");
            
            // Create a static host (acting as a root) - this simulates the problem described in the material
            var eventHost = new EventPublisher();
            
            // Create multiple subscribers - this demonstrates the exact scenario from the material
            Console.WriteLine("\nCreating 1000 event subscribers (like the material example)...");
            var leakySubscribers = CreateLeakyEventSubscribers(eventHost, 1000);
            
            long memoryAfterCreation = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after creating subscribers: {memoryAfterCreation:N0} bytes");
            Console.WriteLine($"Memory used by subscribers: {memoryAfterCreation - memoryBefore:N0} bytes");
            
            // Clear our local references to subscribers
            Console.WriteLine("\nClearing local subscriber references...");
            Console.WriteLine("(But they're still subscribed to events - this is the leak!)");
            leakySubscribers.Clear();
            leakySubscribers = null;
            
            // Force garbage collection - subscribers won't be collected!
            Console.WriteLine("\nForcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after GC: {memoryAfterGC:N0} bytes");
            Console.WriteLine($"Memory NOT reclaimed: {memoryAfterGC - memoryBefore:N0} bytes");
            Console.WriteLine("WHY? The event publisher still holds references to all subscribers!");
            
            // Show subscriber count
            eventHost.ShowSubscriberInfo();
            
            Console.WriteLine("\n--- SOLUTION: Proper Event Unsubscription ---");
            
            // Now demonstrate proper cleanup using IDisposable
            Console.WriteLine("\nCreating properly disposable subscribers...");
            var goodSubscribers = CreateProperEventSubscribers(eventHost, 1000);
            
            long memoryAfterGoodCreation = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after creating disposable subscribers: {memoryAfterGoodCreation:N0} bytes");
            
            // Dispose them properly - this will unsubscribe from events
            Console.WriteLine("\nDisposing subscribers properly (this unsubscribes from events)...");
            foreach (var subscriber in goodSubscribers)
            {
                subscriber.Dispose();
            }
            goodSubscribers.Clear();
            
            // Force collection again
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterProperCleanup = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after proper cleanup: {memoryAfterProperCleanup:N0} bytes");
            Console.WriteLine($"Memory successfully reclaimed: {memoryAfterGoodCreation - memoryAfterProperCleanup:N0} bytes");
            
            // Show subscriber count after cleanup
            eventHost.ShowSubscriberInfo();
            
            Console.WriteLine("\nKEY LESSON: Always implement IDisposable and unsubscribe in Dispose()!");
            Console.WriteLine("This is the most robust solution for event handler memory leaks.");
            Console.WriteLine();
        }
        
        static List<LeakyEventSubscriber> CreateLeakyEventSubscribers(EventPublisher host, int count)
        {
            // This creates subscribers that DON'T unsubscribe - memory leak!
            // Each subscriber will hold a reference to the host AND
            // the host will hold a reference to each subscriber via the event
            var subscribers = new List<LeakyEventSubscriber>();
            
            for (int i = 0; i < count; i++)
            {
                subscribers.Add(new LeakyEventSubscriber(host, i));
            }
            
            return subscribers;
        }
        
        static List<ProperEventSubscriber> CreateProperEventSubscribers(EventPublisher host, int count)
        {
            // This creates subscribers that can properly unsubscribe via IDisposable
            var subscribers = new List<ProperEventSubscriber>();
            
            for (int i = 0; i < count; i++)
            {
                subscribers.Add(new ProperEventSubscriber(host, i));
            }
            
            return subscribers;
        }
        
        static void DemonstrateTimerLeaks()
        {
            Console.WriteLine("DEMO 2: System.Timers.Timer Memory Leaks");
            Console.WriteLine("=========================================");
            
            Console.WriteLine("THE PROBLEM: System.Timers.Timer creates a strong reference chain:");
            Console.WriteLine("1. .NET runtime holds references to active timers");
            Console.WriteLine("2. Timer keeps callback object alive via event subscription");
            Console.WriteLine("3. This creates a circular reference that prevents garbage collection!\n");
            
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before creating timer objects: {memoryBefore:N0} bytes");
            
            // Create objects with timers that aren't disposed - memory leak!
            Console.WriteLine("\nCreating 100 objects with undisposed System.Timers.Timer...");
            Console.WriteLine("Each object subscribes to its timer's Elapsed event");
            CreateLeakyTimerObjects(100);
            
            long memoryAfterLeakyTimers = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after creating timer objects: {memoryAfterLeakyTimers:N0} bytes");
            
            // Try to collect - objects won't be collected because timers hold references
            Console.WriteLine("\nAttempting garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after GC: {memoryAfterGC:N0} bytes");
            Console.WriteLine($"Memory leaked: {memoryAfterGC - memoryBefore:N0} bytes");
            Console.WriteLine("WHY? Runtime keeps timers alive, timers keep callback objects alive!");
            
            Console.WriteLine("\n--- SOLUTION: Implement IDisposable and Dispose Timers ---");
            
            // Now create properly disposable timer objects
            Console.WriteLine("\nCreating 100 objects with properly managed timers...");
            var properTimerObjects = CreateProperTimerObjects(100);
            
            long memoryAfterProperCreation = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after creating disposable timer objects: {memoryAfterProperCreation:N0} bytes");
            
            // Dispose them properly
            Console.WriteLine("\nDisposing timer objects properly...");
            Console.WriteLine("This calls timer.Dispose() which breaks the reference chain");
            foreach (var obj in properTimerObjects)
            {
                obj.Dispose();
            }
            properTimerObjects.Clear();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterProperDisposal = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after proper disposal: {memoryAfterProperDisposal:N0} bytes");
            Console.WriteLine($"Memory properly reclaimed: {memoryAfterProperCreation - memoryAfterProperDisposal:N0} bytes");
            
            Console.WriteLine("\nKEY LESSON: System.Timers.Timer implements IDisposable for a reason!");
            Console.WriteLine("Rule of thumb: If any field implements IDisposable, your class should too!");
            Console.WriteLine();
        }
        
        static void CreateLeakyTimerObjects(int count)
        {
            // These objects will never be collected because their timers hold references
            // This demonstrates the exact problem described in the material
            for (int i = 0; i < count; i++)
            {
                var leakyObject = new LeakyTimerObject(i);
                // Object reference goes out of scope but timer keeps it alive - LEAK!
                // The runtime holds the timer, the timer holds the object
            }
        }
        
        static List<ProperTimerObject> CreateProperTimerObjects(int count)
        {
            // These objects can be properly disposed to break the reference chain
            var objects = new List<ProperTimerObject>();
            
            for (int i = 0; i < count; i++)
            {
                objects.Add(new ProperTimerObject(i));
            }
            
            return objects;
        }
        
        static void DemonstrateThreadingTimerBehavior()
        {
            Console.WriteLine("DEMO 3: System.Threading.Timer Behavior");
            Console.WriteLine("=======================================");
            
            Console.WriteLine("THE DIFFERENCE: System.Threading.Timer behaves differently!");
            Console.WriteLine("- .NET doesn't hold strong references to threading timers");
            Console.WriteLine("- Instead, it references their callback delegates directly");
            Console.WriteLine("- This means timers can be finalized if no strong reference exists");
            Console.WriteLine("- But this creates unpredictable behavior!\n");
            
            // Demonstrate the exact scenario from the material
            Console.WriteLine("Reproducing the exact example from the material:");
            Console.WriteLine("Creating threading timer without keeping reference...");
            
            // This simulates the Main() method example from the material
            var timerWithoutReference = new System.Threading.Timer(
                callback: (state) => Console.WriteLine("  Timer tick (might stop unexpectedly)"),
                state: null,
                dueTime: 1000,
                period: 1000);
            
            Console.WriteLine("Timer created, but local variable might be eligible for GC in release mode");
            
            // Clear the reference (simulating end of scope)
            timerWithoutReference = null;
            
            // Force collection as in the material example
            Console.WriteLine("\nForcing garbage collection (like in the material)...");
            GC.Collect();
            
            Console.WriteLine("Waiting to see if timer continues ticking...");
            Thread.Sleep(3000);
            
            Console.WriteLine("In release mode, the timer might have stopped by now!");
            
            Console.WriteLine("\n--- SOLUTION: Use 'using' Statement for Predictable Behavior ---");
            
            // Now demonstrate the proper way using 'using' statement
            Console.WriteLine("\nCreating threading timer with proper 'using' statement...");
            using (var timer = new System.Threading.Timer(
                callback: (state) => Console.WriteLine("  Timer tick (properly managed)"),
                state: null,
                dueTime: 1000,
                period: 1000))
            {
                Console.WriteLine("Timer created with using statement - guaranteed to stay alive");
                Console.WriteLine("Letting timer run for 3 seconds...");
                Thread.Sleep(3000);
            } // Timer automatically disposed here
            
            Console.WriteLine("Timer automatically disposed when leaving using block");
            
            Console.WriteLine("\nKEY LESSON: Even though threading timers can be finalized,");
            Console.WriteLine("you should NEVER rely on this behavior!");
            Console.WriteLine("Always use 'using' statements or explicit disposal for predictable behavior.");
            Console.WriteLine();
        }
        
        static void DemonstrateStaticReferenceLeaks()
        {
            Console.WriteLine("DEMO 4: Static Reference Memory Leaks");
            Console.WriteLine("=====================================");
            
            Console.WriteLine("THE PROBLEM: Static references act as permanent 'roots' in the GC graph");
            Console.WriteLine("Objects referenced by static collections can NEVER be garbage collected");
            Console.WriteLine("This is because static fields live for the entire application lifetime!\n");
            
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before creating objects: {memoryBefore:N0} bytes");
            
            // Create objects and add them to static collection - they'll never be collected!
            Console.WriteLine("\nAdding 1000 objects to static collection...");
            Console.WriteLine("Each object contains string data that will be held forever");
            for (int i = 0; i < 1000; i++)
            {
                StaticReferenceLeakDemo.AddToStaticCollection($"Object {i} with lots of data to make memory usage visible - this string will never be collected because it's in a static collection");
            }
            
            long memoryAfterStatic = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after adding to static collection: {memoryAfterStatic:N0} bytes");
            Console.WriteLine($"Memory used by static collection: {memoryAfterStatic - memoryBefore:N0} bytes");
            
            // Try to collect - won't work because static collection holds references
            Console.WriteLine("\nAttempting garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after GC: {memoryAfterGC:N0} bytes");
            Console.WriteLine($"Memory NOT reclaimed: {memoryAfterGC - memoryBefore:N0} bytes");
            Console.WriteLine("WHY? Static collections act as 'roots' - GC can't collect referenced objects!");
            
            Console.WriteLine($"\nStatic collection contains: {StaticReferenceLeakDemo.StaticCollectionCount} items");
            
            Console.WriteLine("\n--- SOLUTION: Explicit Static Collection Management ---");
            
            // Clear the static collection to release memory
            Console.WriteLine("\nManually clearing static collection...");
            StaticReferenceLeakDemo.ClearStaticCollection();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterClear = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after clearing static collection: {memoryAfterClear:N0} bytes");
            Console.WriteLine($"Memory successfully reclaimed: {memoryAfterGC - memoryAfterClear:N0} bytes");
            
            Console.WriteLine("\nKEY LESSON: Static collections grow forever unless you explicitly manage them!");
            Console.WriteLine("Solutions: Size limits, LRU eviction, weak references, or regular cleanup");
            Console.WriteLine();
        }
        
        static void DemonstrateWeakReferencesSolution()
        {
            Console.WriteLine("DEMO 5: Weak References as Memory Leak Solution");
            Console.WriteLine("===============================================");
            
            Console.WriteLine("Weak references allow objects to be collected even when referenced");
            
            // Create an object and hold it with weak reference
            var strongRef = new ExpensiveDataObject("Strong Reference Data");
            var weakRef = new WeakReference(strongRef);
            
            Console.WriteLine($"Strong reference exists: {strongRef != null}");
            Console.WriteLine($"Weak reference target alive: {weakRef.IsAlive}");
            Console.WriteLine($"Can access via weak reference: {weakRef.Target != null}");
            
            // Clear strong reference
            Console.WriteLine("\nClearing strong reference...");
            strongRef = null;
            
            // Object might still be alive
            Console.WriteLine($"Weak reference target alive before GC: {weakRef.IsAlive}");
            
            // Force collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Console.WriteLine($"Weak reference target alive after GC: {weakRef.IsAlive}");
            
            if (weakRef.Target == null)
            {
                Console.WriteLine("Object was successfully collected despite weak reference");
            }
            
            // Demonstrate weak event pattern
            Console.WriteLine("\nDemonstrating weak event pattern...");
            DemonstrateWeakEventPattern();
            Console.WriteLine();
        }
        
        static void DemonstrateWeakEventPattern()
        {
            var publisher = new WeakEventPublisher();
            var subscriber = new WeakEventSubscriber();
            
            // Subscribe using weak reference pattern
            WeakEventManager.Subscribe(publisher, subscriber);
            
            Console.WriteLine("Subscriber created and subscribed with weak reference");
            
            // Trigger event
            publisher.TriggerEvent("Test message");
            
            // Clear strong reference to subscriber
            subscriber = null;
            
            // Force collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Console.WriteLine("Subscriber reference cleared and GC forced");
            
            // Try to trigger event again - subscriber should be gone
            publisher.TriggerEvent("Second message (subscriber should be collected)");
        }
        
        static void DemonstrateProperCleanupPatterns()
        {
            Console.WriteLine("DEMO 6: Proper Cleanup Patterns Summary");
            Console.WriteLine("=======================================");
            
            Console.WriteLine("Best practices for preventing memory leaks:");
            Console.WriteLine("1. Always implement IDisposable for resource-holding classes");
            Console.WriteLine("2. Use 'using' statements for automatic disposal");
            Console.WriteLine("3. Unsubscribe from events in Dispose methods");
            Console.WriteLine("4. Be careful with static references");
            Console.WriteLine("5. Consider weak references for event patterns");
            
            Console.WriteLine("\nDemonstrating proper cleanup pattern...");
            
            // Demonstrate using statement
            using (var resource = new ProperlyDisposableResource("Test Resource"))
            {
                resource.DoWork();
                Console.WriteLine("Resource is being used...");
                // Resource will be automatically disposed when leaving this block
            }
            
            Console.WriteLine("Resource automatically disposed by using statement");
            
            // Demonstrate manual disposal
            var manualResource = new ProperlyDisposableResource("Manual Resource");
            try
            {
                manualResource.DoWork();
                Console.WriteLine("Manual resource used");
            }
            finally
            {
                manualResource.Dispose();
                Console.WriteLine("Manual resource explicitly disposed");
            }
            
            Console.WriteLine();
        }
        
        static void DemonstrateMemoryDiagnostics()
        {
            Console.WriteLine("DEMO 7: Memory Diagnostics and Monitoring");
            Console.WriteLine("=========================================");
            
            Console.WriteLine("Learning how to diagnose memory leaks proactively:");
            Console.WriteLine("1. Monitor memory usage during development");
            Console.WriteLine("2. Use profiling tools for deep analysis");
            Console.WriteLine("3. Write unit tests that verify cleanup");
            Console.WriteLine("4. Use performance counters for production monitoring\n");
            
            // Demonstrate memory monitoring
            Console.WriteLine("Memory monitoring example:");
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before allocation: {memoryBefore:N0} bytes");
            
            // Allocate some objects
            var objects = new List<byte[]>();
            for (int i = 0; i < 100; i++)
            {
                objects.Add(new byte[10000]); // 10KB each
            }
            
            long memoryAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after allocation: {memoryAfter:N0} bytes");
            Console.WriteLine($"Memory increase: {memoryAfter - memoryBefore:N0} bytes");
            
            // Clear objects
            objects.Clear();
            
            // Force collection with 'true' for more accurate reading
            long memoryAfterCleanup = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory after cleanup (with GC): {memoryAfterCleanup:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {memoryAfter - memoryAfterCleanup:N0} bytes");
            
            Console.WriteLine("\nKey diagnostic tools:");
            Console.WriteLine("- Visual Studio Diagnostic Tools");
            Console.WriteLine("- dotMemory or PerfView for detailed heap analysis");
            Console.WriteLine("- Windows Performance Toolkit for advanced scenarios");
            Console.WriteLine("- Custom memory monitoring in production code");
            
            Console.WriteLine("\nBest practices for memory leak prevention:");
            Console.WriteLine("1. Implement IDisposable for resource-holding classes");
            Console.WriteLine("2. Use 'using' statements for automatic cleanup");
            Console.WriteLine("3. Always unsubscribe from events you subscribe to");
            Console.WriteLine("4. Be cautious with static collections - they live forever");
            Console.WriteLine("5. Consider weak references for observer patterns");
            Console.WriteLine("6. Profile your application under realistic load");
            
            Console.WriteLine();
        }
    }
}
