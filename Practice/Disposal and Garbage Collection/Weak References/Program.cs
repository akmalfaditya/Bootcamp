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
            Console.WriteLine("Learn how to observe objects without rooting them for the GC\n");

            // Run all demonstrations
            DemonstrateBasicWeakReference();
            Console.WriteLine();

            DemonstrateStrongVsWeakReferences();
            Console.WriteLine();

            DemonstrateSafetyPrecautions();
            Console.WriteLine();

            DemonstrateWidgetTracking();
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
            Console.WriteLine("How WeakReference works with the GC\n");

            // Example from material: basic weak reference usage
            var sb = new StringBuilder("this is a test");
            var weak = new WeakReference(sb);
            
            Console.WriteLine($"Original object: {sb}");
            Console.WriteLine($"Weak reference target: {weak.Target}");
            Console.WriteLine($"Is target alive? {weak.IsAlive}");

            // Clear the strong reference - now only weak reference exists
            Console.WriteLine("\nClearing strong reference...");
            sb = null!;

            // Check before garbage collection
            Console.WriteLine($"Before GC - Is target alive? {weak.IsAlive}");
            Console.WriteLine($"Before GC - Target: {weak.Target}");

            // The material's example with GetWeakRef method
            Console.WriteLine("\nDemonstrating with method that returns weak reference:");
            var weakFromMethod = GetWeakRef();
            Console.WriteLine($"Before GC - Method weak ref alive? {weakFromMethod.IsAlive}");
            
            // Force garbage collection (demonstration only - never do this in production!)
            Console.WriteLine("\nForcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect(); // Second collection to ensure everything is cleaned up

            // Check after garbage collection
            Console.WriteLine($"After GC - Original weak ref alive? {weak.IsAlive}");
            Console.WriteLine($"After GC - Target: {weak.Target ?? "null (object was collected)"}");
            Console.WriteLine($"After GC - Method weak ref alive? {weakFromMethod.IsAlive}");
            Console.WriteLine($"After GC - Method target: {weakFromMethod.Target ?? "null (object was collected)"}");
        }

        // Helper method from the material
        static WeakReference GetWeakRef() => new WeakReference(new StringBuilder("weak"));

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
            Console.WriteLine("DEMO 5: Cache Implementation with Weak References");
            Console.WriteLine("=================================================");
            Console.WriteLine("Cache large objects without preventing GC\n");

            // Simple weak reference cache example from material
            Console.WriteLine("Simple weak reference cache pattern:");
            WeakReference? _weakCache = null;
            
            // First access - create and cache
            var cache = _weakCache?.Target as ExpensiveObject;
            if (cache == null)
            {
                Console.WriteLine("Cache miss - creating new expensive object");
                cache = new ExpensiveObject("Large cached data");
                _weakCache = new WeakReference(cache);
            }
            Console.WriteLine($"Got cached object: {cache.Data}");
            
            // Keep a reference temporarily
            var tempRef = cache;
            
            // Second access - should hit cache
            cache = _weakCache?.Target as ExpensiveObject;
            if (cache != null)
            {
                Console.WriteLine($"Cache hit: {cache.Data}");
            }
            else
            {
                Console.WriteLine("Cache miss - object was collected");
            }
            
            // Clear strong reference
            tempRef = null;
            cache = null;
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            // Third access - should miss because object was collected
            cache = _weakCache?.Target as ExpensiveObject;
            if (cache == null)
            {
                Console.WriteLine("Cache miss after GC - object was collected");
                Console.WriteLine("This demonstrates the limitation: you have limited control over when GC runs");
            }
            
            Console.WriteLine("\nAdvanced cache with proper cleanup:");
            var advancedCache = new WeakReferenceCache();
            
            // Add some items
            var item1 = new ExpensiveObject("Item 1");
            var item2 = new ExpensiveObject("Item 2");
            
            advancedCache.Add("key1", item1);
            advancedCache.Add("key2", item2);
            
            Console.WriteLine($"Cache size: {advancedCache.Count}");
            
            // Clear one reference
            item2 = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            // The cache automatically cleans up dead references
            Console.WriteLine($"Cache size after GC: {advancedCache.Count}");
            
            var retrieved = advancedCache.Get("key1");
            Console.WriteLine($"Retrieved: {retrieved?.Data ?? "null"}");
            
            retrieved = advancedCache.Get("key2");
            Console.WriteLine($"Retrieved: {retrieved?.Data ?? "null"}");
        }

        static void DemonstrateWeakEventPattern()
        {
            Console.WriteLine("DEMO 6: Weak Event Pattern with WeakDelegate");
            Console.WriteLine("============================================");
            Console.WriteLine("Events that don't prevent subscribers from being collected\n");

            var publisher = new WeakEventPublisher();
            
            // Create subscriber - it will be referenced only by the weak event
            var subscriber = new SimpleEventSubscriber("TestSubscriber");
            
            // Subscribe to the event - this uses WeakDelegate internally
            publisher.Click += subscriber.HandleEvent;
            
            Console.WriteLine("Subscriber created and subscribed to event");
            Console.WriteLine($"Active subscribers: {publisher.SubscriberCount}");
            
            // Trigger the event - subscriber should receive it
            Console.WriteLine("\nTriggering event with subscriber alive:");
            publisher.TriggerEvent();
            
            // Clear the strong reference to the subscriber
            subscriber = null;
            Console.WriteLine("\nCleared strong reference to subscriber");
            
            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect(); // Second collection to ensure everything is cleaned up
            
            Console.WriteLine("\nTriggering event after subscriber collected:");
            publisher.TriggerEvent();
            Console.WriteLine($"Active subscribers after GC: {publisher.SubscriberCount}");
            
            Console.WriteLine("\nThis demonstrates the weak event pattern - subscribers can be");
            Console.WriteLine("collected even while subscribed, preventing memory leaks!");
        }

        // Simple event subscriber for WeakDelegate demo
        class SimpleEventSubscriber
        {
            public string Name { get; }
            
            public SimpleEventSubscriber(string name)
            {
                Name = name;
                Console.WriteLine($"  Subscriber '{name}' created");
            }
            
            ~SimpleEventSubscriber()
            {
                Console.WriteLine($"  Subscriber '{Name}' finalized");
            }
            
            public void HandleEvent(object? sender, EventArgs e)
            {
                Console.WriteLine($"  {Name} received event from {sender?.GetType().Name}");
            }
        }

        // Event publisher using WeakDelegate
        class WeakEventPublisher
        {
            private readonly WeakDelegate<EventHandler> _click = new WeakDelegate<EventHandler>();

            public event EventHandler Click
            {
                add { _click.Combine(value); }
                remove { _click.Remove(value); }
            }

            public void TriggerEvent()
            {
                Console.WriteLine("  Publishing event...");
                _click.Target?.Invoke(this, EventArgs.Empty);
            }

            public int SubscriberCount => _click.Target?.GetInvocationList().Length ?? 0;
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

        // DEMO: Safety precautions when using weak references
        static void DemonstrateSafetyPrecautions()
        {
            Console.WriteLine("DEMO 3: Safety Precautions with Weak References");
            Console.WriteLine("===============================================");
            Console.WriteLine("Always assign Target to a local variable to prevent race conditions!\n");

            var sb = new StringBuilder("Safety Demo");
            var weakRef = new WeakReference(sb);
            
            // BAD PRACTICE: Don't do this!
            Console.WriteLine("BAD: Checking Target multiple times (race condition risk)");
            if (weakRef.Target != null)
            {
                // Object could be collected between this check and the next line!
                // Console.WriteLine($"Bad: {weakRef.Target}"); // Don't do this!
            }

            // GOOD PRACTICE: Always assign to local variable first
            Console.WriteLine("GOOD: Assign Target to local variable first");
            var localRef = weakRef.Target as StringBuilder;
            if (localRef != null)
            {
                // Now localRef is a strong reference, object is safe to use
                Console.WriteLine($"Safe: {localRef}");
                // localRef keeps the object alive until it goes out of scope
            }
            
            sb = null; // Clear original reference
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            Console.WriteLine("After GC:");
            var safeCheck = weakRef.Target as StringBuilder;
            if (safeCheck != null)
            {
                Console.WriteLine($"Object survived: {safeCheck}");
            }
            else
            {
                Console.WriteLine("Object was collected (as expected)");
            }
        }

        // DEMO: Widget tracking without preventing collection
        static void DemonstrateWidgetTracking()
        {
            Console.WriteLine("DEMO 4: Widget Tracking with Weak References");
            Console.WriteLine("============================================");
            Console.WriteLine("Track objects without preventing their collection\n");

            // Create several widgets - they automatically register themselves
            var w1 = new Widget("Calculator");
            var w2 = new Widget("TextEditor");
            var w3 = new Widget("MediaPlayer");
            
            Console.WriteLine("All widgets created:");
            Widget.ListAllWidgets();
            Console.WriteLine($"Total tracked widgets: {Widget.TrackedWidgetCount}");

            // Clear some strong references
            w2 = null;
            w3 = null;
            Console.WriteLine("\nCleared references to TextEditor and MediaPlayer");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            Console.WriteLine("\nAfter garbage collection:");
            Widget.ListAllWidgets();
            Console.WriteLine($"Tracked widgets (with dead references): {Widget.TrackedWidgetCount}");
            
            // Demonstrate the cleanup strategy mentioned in the material
            Console.WriteLine("\nCleaning up dead references:");
            Widget.CleanupDeadReferences();
            Widget.ListAllWidgets();
        }
    }
}
