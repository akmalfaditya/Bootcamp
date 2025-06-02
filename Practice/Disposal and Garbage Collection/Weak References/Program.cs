// Weak References in C# - Training Project
// This project demonstrates how to use weak references to allow objects to be garbage collected
// while still maintaining a way to access them if they're still alive

using System.Text;

namespace WeakReferences
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Weak References Training Project ===\n");

            // Run all demonstrations
            DemonstrateBasicWeakReference();
            Console.WriteLine();

            DemonstrateStrongVsWeakReferences();
            Console.WriteLine();

            DemonstrateCacheWithWeakReferences();
            Console.WriteLine();

            DemonstrateWeakEventPattern();
            Console.WriteLine();

            DemonstrateResurrectionBehavior();
            Console.WriteLine();

            DemonstrateWeakReferenceGeneration();
            Console.WriteLine();

            Console.WriteLine("=== All Weak Reference Demonstrations Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DemonstrateBasicWeakReference()
        {
            Console.WriteLine("DEMO 1: Basic Weak Reference Behavior");
            Console.WriteLine("=====================================");

            // Create a normal object
            var stringBuilder = new StringBuilder("Hello from StringBuilder!");
            Console.WriteLine($"Original object: {stringBuilder}");

            // Create a weak reference to it
            // The object can still be collected even though we have this reference
            var weakRef = new WeakReference(stringBuilder);
            Console.WriteLine($"Weak reference target: {weakRef.Target}");
            Console.WriteLine($"Is target alive? {weakRef.IsAlive}");

            // Clear the strong reference - now only weak reference exists
            Console.WriteLine("\nClearing strong reference...");
            stringBuilder = null!;

            // Check before garbage collection
            Console.WriteLine($"Before GC - Is target alive? {weakRef.IsAlive}");
            Console.WriteLine($"Before GC - Target: {weakRef.Target}");

            // Force garbage collection
            // This is just for demonstration - never do this in production!
            Console.WriteLine("Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Check after garbage collection
            Console.WriteLine($"After GC - Is target alive? {weakRef.IsAlive}");
            Console.WriteLine($"After GC - Target: {weakRef.Target ?? "null (object was collected)"}");
        }

        static void DemonstrateStrongVsWeakReferences()
        {
            Console.WriteLine("DEMO 2: Strong vs Weak References Comparison");
            Console.WriteLine("============================================");

            // First, show how strong references prevent collection
            Console.WriteLine("Creating object with STRONG reference...");
            var strongObject = new ExpensiveObject("Strong Reference Data");
            var strongRef = strongObject; // This keeps the object alive
            var weakRefToStrong = new WeakReference(strongObject);

            strongObject = null!; // Clear original reference
            Console.WriteLine("Original reference cleared, but strong reference still exists");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"After GC with strong ref - Object alive? {weakRefToStrong.IsAlive}");

            // Now clear the strong reference too
            strongRef = null!;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"After clearing strong ref - Object alive? {weakRefToStrong.IsAlive}");

            Console.WriteLine("\nCreating object with WEAK reference only...");
            var weakOnlyObject = new ExpensiveObject("Weak Reference Only");
            var weakRefOnly = new WeakReference(weakOnlyObject);

            // Clear the only strong reference
            weakOnlyObject = null!;
            Console.WriteLine("Strong reference cleared, only weak reference remains");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"After GC with weak ref only - Object alive? {weakRefOnly.IsAlive}");
        }

        static void DemonstrateCacheWithWeakReferences()
        {
            Console.WriteLine("DEMO 3: Cache Implementation with Weak References");
            Console.WriteLine("=================================================");

            var cache = new WeakReferenceCache();

            // Add some items to cache
            var item1 = new ExpensiveObject("Cached Item 1");
            var item2 = new ExpensiveObject("Cached Item 2");
            var item3 = new ExpensiveObject("Cached Item 3");

            cache.Add("key1", item1);
            cache.Add("key2", item2);
            cache.Add("key3", item3);

            Console.WriteLine($"Cache size after adding 3 items: {cache.Count}");

            // Try to retrieve items
            var retrieved1 = cache.Get("key1");
            Console.WriteLine($"Retrieved item1: {retrieved1?.Data ?? "null"}");

            // Clear some strong references
            item2 = null!;
            item3 = null!;
            Console.WriteLine("\nCleared strong references to item2 and item3");

            // Force GC to see what happens to cached items
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Cache size after GC: {cache.Count}");
            Console.WriteLine($"Can still get item1: {cache.Get("key1")?.Data ?? "null"}");
            Console.WriteLine($"Can still get item2: {cache.Get("key2")?.Data ?? "null"}");
            Console.WriteLine($"Can still get item3: {cache.Get("key3")?.Data ?? "null"}");

            // Clear the last strong reference
            item1 = null!;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"\nFinal cache size after all items collected: {cache.Count}");
        }

        static void DemonstrateWeakEventPattern()
        {
            Console.WriteLine("DEMO 4: Weak Event Pattern");
            Console.WriteLine("===========================");

            var publisher = new WeakEventPublisher();
            
            // Create subscriber and subscribe to event
            var subscriber = new EventSubscriber("Subscriber1");
            publisher.Subscribe(subscriber);

            Console.WriteLine("Publishing event with subscriber alive...");
            publisher.PublishEvent("First message");

            Console.WriteLine($"Active subscriptions: {publisher.SubscriberCount}");

            // Clear strong reference to subscriber
            Console.WriteLine("\nClearing strong reference to subscriber...");
            subscriber = null!;

            // Force GC
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Publishing event after subscriber collected...");
            publisher.PublishEvent("Second message (subscriber should be gone)");

            Console.WriteLine($"Active subscriptions after GC: {publisher.SubscriberCount}");
        }

        static void DemonstrateResurrectionBehavior()
        {
            Console.WriteLine("DEMO 5: Object Resurrection with Weak References");
            Console.WriteLine("================================================");

            // This demonstrates how weak references behave during object resurrection
            ResurrectableObject.ClearResurrectedInstance();

            var obj = new ResurrectableObject("Resurrect Me!");
            var weakRef = new WeakReference(obj);

            Console.WriteLine($"Before collection - Object alive? {weakRef.IsAlive}");

            // Clear strong reference
            obj = null!;

            // Force GC - this will resurrect the object
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"After first GC - Object alive? {weakRef.IsAlive}");
            Console.WriteLine($"Object was resurrected: {ResurrectableObject.ResurrectedInstance != null}");

            // Second GC will actually collect it
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"After second GC - Object alive? {weakRef.IsAlive}");
            Console.WriteLine($"Object still resurrected: {ResurrectableObject.ResurrectedInstance != null}");
        }

        static void DemonstrateWeakReferenceGeneration()
        {
            Console.WriteLine("DEMO 6: Weak Reference Generation Tracking");
            Console.WriteLine("==========================================");

            // WeakReference can track which generation an object belongs to
            var shortLived = new ExpensiveObject("Short lived object");
            var weakRefShort = new WeakReference(shortLived, trackResurrection: false);

            var longLived = new ExpensiveObject("Long lived object");
            var weakRefLong = new WeakReference(longLived, trackResurrection: true);

            Console.WriteLine("Created objects with different weak reference tracking...");
            Console.WriteLine($"Short-lived object alive? {weakRefShort.IsAlive}");
            Console.WriteLine($"Long-lived object alive? {weakRefLong.IsAlive}");

            // Keep long-lived object alive but clear short-lived
            shortLived = null!;

            // Generate some garbage to age objects
            for (int i = 0; i < 3; i++)
            {
                var temp = new byte[1000000]; // 1MB allocation
                GC.Collect(0); // Collect generation 0
            }

            Console.WriteLine("\nAfter multiple generation 0 collections:");
            Console.WriteLine($"Short-lived object alive? {weakRefShort.IsAlive}");
            Console.WriteLine($"Long-lived object alive? {weakRefLong.IsAlive}");

            // Full collection
            longLived = null!;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\nAfter full collection:");
            Console.WriteLine($"Short-lived object alive? {weakRefShort.IsAlive}");
            Console.WriteLine($"Long-lived object alive? {weakRefLong.IsAlive}");
        }
    }
}
