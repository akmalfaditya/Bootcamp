// Managed Memory Leaks in C# - Training Project
// This project demonstrates common memory leak scenarios in managed code
// You'll see how objects stay alive due to forgotten references

using System.Timers;

namespace ManagedMemoryLeaks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Managed Memory Leaks Training Project ===\n");
            
            // These demonstrations show real memory leak scenarios you'll encounter
            // Pay attention to memory usage before and after each demo
            
            DemonstrateEventHandlerLeaks();
            DemonstrateTimerLeaks();
            DemonstrateThreadingTimerBehavior();
            DemonstrateStaticReferenceLeaks();
            DemonstrateWeakReferencesSolution();
            DemonstrateProperCleanupPatterns();
            
            Console.WriteLine("\n=== All Memory Leak Demonstrations Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        static void DemonstrateEventHandlerLeaks()
        {
            Console.WriteLine("DEMO 1: Event Handler Memory Leaks");
            Console.WriteLine("===================================");
            
            // Show memory before creating objects
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before creating event subscribers: {memoryBefore:N0} bytes");
            
            // Create the host that publishes events
            var eventHost = new EventPublisher();
            
            // Create multiple subscribers - this is where the leak happens
            Console.WriteLine("\nCreating 1000 event subscribers...");
            var leakySubscribers = CreateLeakyEventSubscribers(eventHost, 1000);
            
            long memoryAfterCreation = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after creating subscribers: {memoryAfterCreation:N0} bytes");
            
            // Clear our references to subscribers, but they're still alive!
            Console.WriteLine("\nClearing subscriber references (but they're still subscribed to events)...");
            leakySubscribers.Clear();
            leakySubscribers = null;
            
            // Force garbage collection - subscribers won't be collected!
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after GC (subscribers still alive): {memoryAfterGC:N0} bytes");
            Console.WriteLine($"Memory NOT reclaimed: {memoryAfterGC - memoryBefore:N0} bytes");
            
            // Now demonstrate proper cleanup
            Console.WriteLine("\nNow creating properly disposable subscribers...");
            var goodSubscribers = CreateProperEventSubscribers(eventHost, 1000);
            
            long memoryAfterGoodCreation = GC.GetTotalMemory(false);
            
            // Dispose them properly
            Console.WriteLine("Disposing subscribers properly...");
            foreach (var subscriber in goodSubscribers)
            {
                subscriber.Dispose();
            }
            goodSubscribers.Clear();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterProperCleanup = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after proper cleanup: {memoryAfterProperCleanup:N0} bytes");
            Console.WriteLine($"Memory properly reclaimed: {memoryAfterGoodCreation - memoryAfterProperCleanup:N0} bytes");
            Console.WriteLine();
        }
        
        static List<LeakyEventSubscriber> CreateLeakyEventSubscribers(EventPublisher host, int count)
        {
            // This creates subscribers that DON'T unsubscribe - memory leak!
            var subscribers = new List<LeakyEventSubscriber>();
            
            for (int i = 0; i < count; i++)
            {
                subscribers.Add(new LeakyEventSubscriber(host, i));
            }
            
            return subscribers;
        }
        
        static List<ProperEventSubscriber> CreateProperEventSubscribers(EventPublisher host, int count)
        {
            // This creates subscribers that properly unsubscribe
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
            
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before creating timer objects: {memoryBefore:N0} bytes");
            
            // Create objects with timers that aren't disposed - memory leak!
            Console.WriteLine("\nCreating 100 objects with undisposed timers...");
            CreateLeakyTimerObjects(100);
            
            long memoryAfterLeakyTimers = GC.GetTotalMemory(false);
            
            // Try to collect - objects won't be collected because timers hold references
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after GC (timers keep objects alive): {memoryAfterGC:N0} bytes");
            Console.WriteLine($"Memory leaked: {memoryAfterGC - memoryBefore:N0} bytes");
            
            // Now create properly disposable timer objects
            Console.WriteLine("\nCreating 100 objects with properly managed timers...");
            var properTimerObjects = CreateProperTimerObjects(100);
            
            long memoryAfterProperCreation = GC.GetTotalMemory(false);
            
            // Dispose them properly
            Console.WriteLine("Disposing timer objects properly...");
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
            Console.WriteLine();
        }
        
        static void CreateLeakyTimerObjects(int count)
        {
            // These objects will never be collected because their timers hold references
            for (int i = 0; i < count; i++)
            {
                var leakyObject = new LeakyTimerObject(i);
                // Object goes out of scope but timer keeps it alive - LEAK!
            }
        }
        
        static List<ProperTimerObject> CreateProperTimerObjects(int count)
        {
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
            
            Console.WriteLine("Threading timers behave differently - they can be collected automatically");
            Console.WriteLine("But it's still best practice to dispose them properly\n");
            
            // Demonstrate the difference in behavior
            Console.WriteLine("Creating threading timer without proper disposal...");
            CreateThreadingTimerWithoutDisposal();
            
            // Wait a moment
            Thread.Sleep(2000);
            
            // Force collection
            Console.WriteLine("Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Thread.Sleep(2000);
            
            Console.WriteLine("\nCreating threading timer with proper using statement...");
            CreateThreadingTimerWithProperDisposal();
            
            Console.WriteLine();
        }
        
        static void CreateThreadingTimerWithoutDisposal()
        {
            // This timer might get collected automatically in release mode
            // But it's unpredictable behavior - don't rely on it!
            var timer = new System.Threading.Timer(
                callback: (_) => Console.WriteLine("  Threading timer tick (might stop unexpectedly)"),
                state: null,
                dueTime: 500,
                period: 1000
            );
            
            Console.WriteLine("Threading timer created without disposal pattern");
            // Timer reference goes out of scope - unpredictable behavior
        }
        
        static void CreateThreadingTimerWithProperDisposal()
        {
            // This is the proper way - using statement ensures disposal
            using (var timer = new System.Threading.Timer(
                callback: (_) => Console.WriteLine("  Threading timer tick (properly managed)"),
                state: null,
                dueTime: 500,
                period: 1000))
            {
                Console.WriteLine("Threading timer created with using statement");
                Thread.Sleep(3000); // Let it tick a few times
            } // Timer automatically disposed here
            
            Console.WriteLine("Threading timer properly disposed");
        }
        
        static void DemonstrateStaticReferenceLeaks()
        {
            Console.WriteLine("DEMO 4: Static Reference Memory Leaks");
            Console.WriteLine("=====================================");
            
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before creating objects: {memoryBefore:N0} bytes");
            
            // Create objects and add them to static collection - they'll never be collected!
            Console.WriteLine("\nAdding 1000 objects to static collection...");
            for (int i = 0; i < 1000; i++)
            {
                StaticReferenceLeakDemo.AddToStaticCollection($"Object {i} with lots of data to make memory usage visible");
            }
            
            long memoryAfterStatic = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after adding to static collection: {memoryAfterStatic:N0} bytes");
            
            // Try to collect - won't work because static collection holds references
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterGC = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after GC (static references prevent collection): {memoryAfterGC:N0} bytes");
            Console.WriteLine($"Memory held by static references: {memoryAfterGC - memoryBefore:N0} bytes");
            
            // Clear the static collection to release memory
            Console.WriteLine("\nClearing static collection...");
            StaticReferenceLeakDemo.ClearStaticCollection();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfterClear = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after clearing static collection: {memoryAfterClear:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {memoryAfterGC - memoryAfterClear:N0} bytes");
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
    }
}
