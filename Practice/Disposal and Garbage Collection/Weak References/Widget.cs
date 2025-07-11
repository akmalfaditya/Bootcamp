using System;
using System.Collections.Generic;

namespace WeakReferences
{
    // Widget class demonstrates tracking objects without preventing their collection
    // This is a classic use case for weak references - we want to track instances
    // but don't want our tracking mechanism to keep them alive
    // 
    // In this system, the _allWidgets static list holds weak references to Widget objects.
    // If a Widget instance is no longer strongly referenced elsewhere, it can be collected
    // by the GC, and its corresponding WeakReference.Target will become null.
    public class Widget
    {
        // Static list stores weak references to all Widget instances
        // This allows objects to be collected even though we're "tracking" them
        static List<WeakReference> _allWidgets = new List<WeakReference>();
        
        public readonly string Name;

        public Widget(string name)
        {
            Name = name;
            // Add a weak reference to this instance - not the instance itself
            // This is crucial: we're not preventing collection by storing a weak reference
            _allWidgets.Add(new WeakReference(this));
            Console.WriteLine($"  Widget '{name}' created");
        }

        ~Widget()
        {
            Console.WriteLine($"  Widget '{Name}' finalized");
        }

        public static void ListAllWidgets()
        {
            Console.WriteLine("Active widgets:");
            foreach (WeakReference weak in _allWidgets)
            {
                // Always assign Target to a local variable first!
                // This creates a temporary strong reference to prevent collection
                // while we're working with the object
                Widget? w = weak.Target as Widget;
                if (w != null) 
                {
                    Console.WriteLine($"  - {w.Name}");
                }
            }
        }

        // This demonstrates the cleanup strategy mentioned in the material
        // Over time, dead weak references accumulate and need to be cleaned up
        public static void CleanupDeadReferences()
        {
            Console.WriteLine("Cleaning up dead widget references...");
            // Remove all weak references where Target is null (object was collected)
            _allWidgets.RemoveAll(wr => wr.Target == null);
            Console.WriteLine($"Remaining tracked widgets: {_allWidgets.Count}");
        }

        public static int TrackedWidgetCount => _allWidgets.Count;
    }
}
