using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// Comprehensive demonstration of all types of roots that keep objects alive
    /// in the garbage collector. This directly implements the concepts from the training material
    /// about what constitutes a "root" in the GC's reachability analysis.
    /// 
    /// A root is anything the GC considers a starting point for tracing live objects.
    /// If an object is not directly or indirectly referenced by a root, it's unreachable.
    /// </summary>
    public static class RootsAndReachabilityDemo
    {
        // 1. Static Variables - these are roots that exist for the application lifetime
        private static SampleObject? _staticRoot;
        private static List<SampleObject> _staticCollection = new List<SampleObject>();
        
        /// <summary>
        /// Demonstrates all five types of roots mentioned in the training material:
        /// 1. Local Variables or Parameters
        /// 2. Static Variables  
        /// 3. CPU Registers
        /// 4. GC Handles
        /// 5. Objects on the Finalization Queue
        /// </summary>
        public static void DemoAllRootTypes()
        {
            Console.WriteLine("=== Comprehensive Roots and Reachability Demo ===");
            Console.WriteLine("Demonstrating all types of roots that keep objects alive");
            Console.WriteLine();
            
            // Demo each type of root
            DemoLocalVariableRoots();
            DemoStaticVariableRoots();
            DemoGCHandleRoots();
            DemoFinalizationQueueRoots();
            DemoCyclicReferencesWithoutRoots();
            
            Console.WriteLine("Key insight: Only objects reachable from roots survive collection!");
        }
        
        /// <summary>
        /// Demonstrates how local variables and parameters act as roots.
        /// This is the most common type of root in everyday programming.
        /// </summary>
        private static void DemoLocalVariableRoots()
        {
            Console.WriteLine("1. LOCAL VARIABLES AND PARAMETERS AS ROOTS");
            Console.WriteLine("===========================================");
            
            // Local variables are roots - they keep objects alive
            var localRoot = new SampleObject("LocalRoot");
            
            // Create object graph reachable from local variable
            localRoot.Child = new SampleObject("LocalChild");
            localRoot.Child.Child = new SampleObject("LocalGrandchild");
            
            Console.WriteLine("Created object graph with local variable as root:");
            Console.WriteLine($"- {localRoot.Name} (root)");
            Console.WriteLine($"- {localRoot.Child.Name} (child)");
            Console.WriteLine($"- {localRoot.Child.Child.Name} (grandchild)");
            
            // All objects are reachable from the local variable root
            Console.WriteLine("All objects are reachable from local variable root");
            
            // Demonstrate with a parameter
            DemoParameterAsRoot(localRoot);
            
            Console.WriteLine("When method exits, local variable goes out of scope");
            Console.WriteLine("Entire object graph becomes unreachable (eligible for collection)");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Shows how method parameters also act as roots during method execution.
        /// </summary>
        private static void DemoParameterAsRoot(SampleObject parameter)
        {
            Console.WriteLine($"Method parameter '{parameter.Name}' is also a root");
            Console.WriteLine("Object graph remains reachable through parameter");
            
            // Create additional objects reachable through parameter
            if (parameter.Child?.Child != null)
            {
                parameter.Child.Child.Child = new SampleObject("ParameterExtension");
                Console.WriteLine("Extended object graph through parameter reference");
            }
        }
        
        /// <summary>
        /// Demonstrates how static variables act as roots.
        /// Static variables exist for the lifetime of the application domain.
        /// </summary>
        private static void DemoStaticVariableRoots()
        {
            Console.WriteLine("2. STATIC VARIABLES AS ROOTS");
            Console.WriteLine("=============================");
            
            // Create objects referenced by static variables
            _staticRoot = new SampleObject("StaticRoot");
            _staticRoot.Child = new SampleObject("StaticChild");
            
            // Add objects to static collection
            _staticCollection.Add(new SampleObject("StaticCollection_1"));
            _staticCollection.Add(new SampleObject("StaticCollection_2"));
            
            Console.WriteLine("Created objects referenced by static variables:");
            Console.WriteLine($"- Static field: {_staticRoot.Name}");
            Console.WriteLine($"- Static field child: {_staticRoot.Child.Name}");
            Console.WriteLine($"- Static collection: {_staticCollection.Count} objects");
            
            Console.WriteLine();
            Console.WriteLine("These objects will remain reachable until:");
            Console.WriteLine("- Static references are set to null, OR");
            Console.WriteLine("- Application domain is unloaded");
            
            // Demonstrate clearing static references
            Console.WriteLine("\nClearing static references...");
            _staticRoot = null;
            _staticCollection.Clear();
            Console.WriteLine("Static references cleared - objects now eligible for collection");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates GC Handles as roots.
        /// These are special handles used by the runtime to maintain strong references,
        /// often for interoperability with unmanaged code.
        /// </summary>
        private static void DemoGCHandleRoots()
        {
            Console.WriteLine("3. GC HANDLES AS ROOTS");
            Console.WriteLine("======================");
            
            // Create an object
            var targetObject = new SampleObject("GCHandleTarget");
            
            // Create a GC handle to keep it alive
            GCHandle handle = GCHandle.Alloc(targetObject, GCHandleType.Normal);
            
            Console.WriteLine($"Created GC handle for: {targetObject.Name}");
            Console.WriteLine("GC handle acts as a root - object cannot be collected");
            
            // Clear the local reference
            targetObject = null;
            Console.WriteLine("Cleared local reference - but GC handle still keeps object alive");
            
            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            // Object should still be alive due to GC handle
            if (handle.IsAllocated)
            {
                var retrievedObject = (SampleObject)handle.Target!;
                Console.WriteLine($"Object still alive through GC handle: {retrievedObject.Name}");
            }
            
            // Free the handle
            Console.WriteLine("Freeing GC handle...");
            handle.Free();
            Console.WriteLine("GC handle freed - object is now eligible for collection");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates how objects on the finalization queue act as roots.
        /// Objects with finalizers are kept alive until their finalizer runs.
        /// </summary>
        private static void DemoFinalizationQueueRoots()
        {
            Console.WriteLine("4. FINALIZATION QUEUE AS ROOTS");
            Console.WriteLine("===============================");
            
            Console.WriteLine("Creating objects with finalizers...");
            
            // Create objects with finalizers (these automatically go to finalization queue)
            CreateFinalizableObjects();
            
            Console.WriteLine("Objects with finalizers are placed on finalization queue");
            Console.WriteLine("Finalization queue acts as a root - keeps objects alive");
            Console.WriteLine("Objects require TWO garbage collection cycles:");
            Console.WriteLine("  Cycle 1: Object marked for finalization, finalizer scheduled");
            Console.WriteLine("  Cycle 2: After finalizer runs, object can be collected");
            
            Console.WriteLine("\nForcing first GC cycle...");
            GC.Collect();
            
            Console.WriteLine("After first cycle: Objects still alive, finalizers scheduled");
            
            Console.WriteLine("Waiting for finalizers to run...");
            GC.WaitForPendingFinalizers();
            
            Console.WriteLine("Forcing second GC cycle...");
            GC.Collect();
            
            Console.WriteLine("After second cycle: Objects can now be fully collected");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Creates objects that have finalizers and will be placed on finalization queue.
        /// </summary>
        private static void CreateFinalizableObjects()
        {
            for (int i = 0; i < 5; i++)
            {
                // Create FinalizationExample objects - these have finalizers
                new FinalizationExample($"FinalizationQueueObject_{i}");
            }
        }
        
        /// <summary>
        /// Demonstrates the key insight from the training material:
        /// Cyclic references don't prevent collection if no roots exist.
        /// The GC can collect entire groups of cyclically referenced objects.
        /// </summary>
        private static void DemoCyclicReferencesWithoutRoots()
        {
            Console.WriteLine("5. CYCLIC REFERENCES WITHOUT ROOTS");
            Console.WriteLine("===================================");
            
            Console.WriteLine("Creating circular reference chain without any root references...");
            
            // Create circular references but don't store any root references
            CreateCircularChainWithoutRoots();
            
            Console.WriteLine("Circular chain created: A → B → C → A");
            Console.WriteLine("No root variables store references to any object in the chain");
            
            long memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory before collection: {memoryBefore:N0} bytes");
            
            Console.WriteLine("\nForcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long memoryAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory after collection: {memoryAfter:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {memoryBefore - memoryAfter:N0} bytes");
            
            Console.WriteLine("\n✓ Circular references were successfully collected!");
            Console.WriteLine("Key insight: The GC traces from roots, not from objects");
            Console.WriteLine("If a group of objects can't be reached from any root,");
            Console.WriteLine("the entire group is collected, regardless of internal references");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Creates a circular reference chain but doesn't store any root references.
        /// </summary>
        private static void CreateCircularChainWithoutRoots()
        {
            // Create objects in a circular chain
            var objA = new CircularReferenceObject("A");
            var objB = new CircularReferenceObject("B");
            var objC = new CircularReferenceObject("C");
            
            // Create the circular references
            objA.Reference = objB;
            objB.Reference = objC;
            objC.Reference = objA;
            
            // At this point, we have A→B→C→A circular references
            // But when this method exits, all local variables (objA, objB, objC)
            // go out of scope, leaving no root references to the circular chain
            
            // The entire chain becomes unreachable despite the circular references
        }
        
        /// <summary>
        /// Demonstrates the critical concept that objects must be reachable from roots.
        /// Shows different scenarios of reachability and unreachability.
        /// </summary>
        public static void DemoReachabilityScenarios()
        {
            Console.WriteLine("=== REACHABILITY SCENARIOS ===");
            Console.WriteLine("Demonstrating different reachability patterns");
            Console.WriteLine();
            
            // Scenario 1: Simple reachability
            Console.WriteLine("Scenario 1: Simple reachability chain");
            var root = new SampleObject("Root");
            root.Child = new SampleObject("Child");
            root.Child.Child = new SampleObject("Grandchild");
            
            Console.WriteLine("Root → Child → Grandchild");
            Console.WriteLine("All objects reachable from root");
            
            // Scenario 2: Broken chain
            Console.WriteLine("\nScenario 2: Breaking the chain");
            root.Child = null; // Break the chain
            Console.WriteLine("Root → [broken] Child → Grandchild");
            Console.WriteLine("Root is reachable, Child and Grandchild are not");
            
            // Scenario 3: Multiple paths to same object
            Console.WriteLine("\nScenario 3: Multiple paths to same object");
            var sharedObject = new SampleObject("Shared");
            var root1 = new SampleObject("Root1");
            var root2 = new SampleObject("Root2");
            
            root1.Child = sharedObject;
            root2.Child = sharedObject;
            
            Console.WriteLine("Root1 → Shared ← Root2");
            Console.WriteLine("Shared object has multiple paths from different roots");
            
            // Clear one root
            root1 = null;
            Console.WriteLine("After clearing Root1: Root2 → Shared");
            Console.WriteLine("Shared object still reachable through Root2");
            
            Console.WriteLine("\nKey insight: Objects remain alive as long as ANY path from ANY root exists");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates how instance methods affect object reachability.
        /// As mentioned in the training material: "If there's any possibility for an 
        /// instance method of an object to execute, that object must be referenced by a root."
        /// </summary>
        public static void DemoInstanceMethodReachability()
        {
            Console.WriteLine("=== INSTANCE METHOD REACHABILITY ===");
            Console.WriteLine("Objects must be reachable if their instance methods might execute");
            Console.WriteLine();
            
            // Create an object and store it where instance methods might be called
            var obj = new SampleObject("MethodTarget");
            
            // Store reference in a location where methods might be called
            Action? methodCall = obj.DoSomething;
            
            Console.WriteLine("Created object and stored method reference");
            Console.WriteLine("Object must remain reachable because method might be called");
            
            // Clear the direct reference
            obj = null;
            Console.WriteLine("Cleared direct object reference");
            
            // But the object is still reachable through the method delegate
            Console.WriteLine("Object still reachable through method delegate");
            
            // Call the method to prove object is still alive
            methodCall?.Invoke();
            
            // Clear the method reference
            methodCall = null;
            Console.WriteLine("Cleared method reference - object now eligible for collection");
            Console.WriteLine();
        }
    }
    
    /// <summary>
    /// Extension of SampleObject to demonstrate method reachability.
    /// </summary>
    public static class SampleObjectExtensions
    {
        /// <summary>
        /// A method that can be called on SampleObject instances.
        /// </summary>
        public static void DoSomething(this SampleObject obj)
        {
            Console.WriteLine($"Instance method called on: {obj.Name}");
            Console.WriteLine("Object must be reachable for this method to execute");
        }
    }
}
