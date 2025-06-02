// ResurrectableObject.cs
// This demonstrates object resurrection and how weak references behave
// during the finalization process

namespace WeakReferences
{
    // An object that resurrects itself during finalization
    // This shows advanced GC behavior with weak references
    public class ResurrectableObject
    {
        public string Data { get; private set; }
        private static int _instanceCount = 0;
        private int _instanceId;

        // Static field to hold resurrected instance
        public static ResurrectableObject? ResurrectedInstance { get; private set; }

        public ResurrectableObject(string data)
        {
            _instanceId = ++_instanceCount;
            Data = data;
            Console.WriteLine($"  ResurrectableObject #{_instanceId} created: {data}");
        }

        // Finalizer that resurrects the object
        ~ResurrectableObject()
        {
            Console.WriteLine($"  ResurrectableObject #{_instanceId} finalizer called - attempting resurrection!");
            
            // Resurrect by storing reference in static field
            // This prevents the object from being collected this time
            if (ResurrectedInstance == null)
            {
                ResurrectedInstance = this;
                Console.WriteLine($"  Object #{_instanceId} resurrected successfully!");
                
                // Re-register for finalization to show what happens next time
                GC.ReRegisterForFinalize(this);
            }
            else
            {
                Console.WriteLine($"  Object #{_instanceId} finally being collected (second finalization)");
            }
        }

        public void DoSomething()
        {
            Console.WriteLine($"ResurrectableObject #{_instanceId} is doing something: {Data}");
        }

        // Helper method to clear the resurrected instance
        public static void ClearResurrectedInstance()
        {
            if (ResurrectedInstance != null)
            {
                Console.WriteLine("  Clearing resurrected instance");
                ResurrectedInstance = null;
            }
        }

        public override string ToString()
        {
            return $"ResurrectableObject #{_instanceId}: {Data}";
        }
    }

    // Demonstrates different weak reference behaviors
    public static class WeakReferenceAdvancedExamples
    {
        // Show how weak references behave with different tracking options
        public static void DemonstrateTrackingOptions()
        {
            Console.WriteLine("Advanced: Weak Reference Tracking Options");
            Console.WriteLine("========================================");

            var obj = new ExpensiveObject("Tracking Demo");

            // Create weak references with different tracking behaviors
            var weakRefNoTracking = new WeakReference(obj, trackResurrection: false);
            var weakRefWithTracking = new WeakReference(obj, trackResurrection: true);

            Console.WriteLine($"Before collection - No tracking alive: {weakRefNoTracking.IsAlive}");
            Console.WriteLine($"Before collection - With tracking alive: {weakRefWithTracking.IsAlive}");

            // Clear strong reference
            obj = null!;

            // Force collection
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"After collection - No tracking alive: {weakRefNoTracking.IsAlive}");
            Console.WriteLine($"After collection - With tracking alive: {weakRefWithTracking.IsAlive}");

            // Second collection cycle
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"After second collection - No tracking alive: {weakRefNoTracking.IsAlive}");
            Console.WriteLine($"After second collection - With tracking alive: {weakRefWithTracking.IsAlive}");
        }

        // Demonstrate weak reference with large objects
        public static void DemonstrateLargeObjectBehavior()
        {
            Console.WriteLine("\nAdvanced: Weak References with Large Objects");
            Console.WriteLine("============================================");

            // Large objects (>85KB) go to Large Object Heap
            var largeData = new byte[100000]; // 100KB
            var weakRefLarge = new WeakReference(largeData);

            Console.WriteLine($"Large object created - Weak ref alive: {weakRefLarge.IsAlive}");

            // Clear reference
            largeData = null!;

            // Large Object Heap is collected less frequently
            Console.WriteLine("Forcing collection of Large Object Heap...");
            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"After LOH collection - Weak ref alive: {weakRefLarge.IsAlive}");
        }
    }
}
