using System;
using System.Diagnostics;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// This class demonstrates the most fundamental concepts of automatic garbage collection
    /// mentioned in the training material. It shows how objects are allocated on the heap,
    /// how local variables reference them, and how they become eligible for collection
    /// when references go out of scope.
    /// </summary>
    public static class BasicMemoryExample
    {
        /// <summary>
        /// This is the exact example from the training material that shows how memory
        /// is allocated on the heap and becomes eligible for collection when the method exits.
        /// 
        /// Key concepts demonstrated:
        /// - Memory allocation on the managed heap
        /// - Local variables stored on the stack
        /// - Objects becoming "orphaned" when references go out of scope
        /// - Eligibility vs. immediate collection
        /// </summary>
        public static void Test()
        {
            // This mirrors the exact example from the training material
            byte[] myArray = new byte[1000]; // Memory allocated on the heap
            
            // At this point:
            // - 1000 bytes are allocated on the managed heap
            // - myArray (a reference) is stored on the method's local variable stack
            // - The array is reachable through the myArray reference
            
            Console.WriteLine($"Array allocated: {myArray.Length} bytes on the heap");
            Console.WriteLine("The myArray variable (reference) is on the stack");
            
            // ... use myArray ...
            // Here we could do something with the array, but for this demo we don't need to
            
        } // Method exits - myArray local variable pops out of scope
        
        // At this point (after method exit):
        // - The myArray local variable has been popped from the stack
        // - No active reference remains pointing to the array on the heap
        // - The array is now "orphaned" or "unreachable"
        // - The array becomes eligible for garbage collection
        // - IMPORTANT: Eligibility does NOT mean immediate collection!
        
        /// <summary>
        /// Demonstrates the key concept that eligibility for collection does NOT mean
        /// immediate collection. Shows the indeterminate nature of garbage collection.
        /// </summary>
        public static void DemoIndeterminateCollection()
        {
            Console.WriteLine("=== Indeterminate Nature of Garbage Collection ===");
            Console.WriteLine("Creating objects that become immediately eligible for collection...");
            
            // Track memory and time to show the delay
            long memoryBefore = GC.GetTotalMemory(false);
            var stopwatch = Stopwatch.StartNew();
            
            // Create objects that become immediately eligible
            for (int i = 0; i < 100; i++)
            {
                CreateOrphanedArray();
            }
            
            stopwatch.Stop();
            long memoryAfter = GC.GetTotalMemory(false);
            
            Console.WriteLine($"Created 100 orphaned arrays in {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Memory before: {memoryBefore:N0} bytes");
            Console.WriteLine($"Memory after: {memoryAfter:N0} bytes");
            Console.WriteLine($"Memory increase: {memoryAfter - memoryBefore:N0} bytes");
            Console.WriteLine();
            Console.WriteLine("Key insight: Objects are eligible but may not be collected yet!");
            Console.WriteLine("The GC decides when to collect based on:");
            Console.WriteLine("- Available memory (is memory becoming scarce?)");
            Console.WriteLine("- Amount of memory allocation (significant new allocations?)");
            Console.WriteLine("- Time since last collection (balancing collection frequency)");
            Console.WriteLine();
            Console.WriteLine("The delay can range from nanoseconds to much longer periods");
            Console.WriteLine("depending on system load and GC heuristics.");
        }
        
        /// <summary>
        /// Creates an array that immediately becomes orphaned (no reference stored).
        /// This demonstrates how objects can become eligible for collection instantly.
        /// </summary>
        private static void CreateOrphanedArray()
        {
            // Create an array but don't store the reference anywhere
            // This makes it immediately eligible for collection
            _ = new byte[1000];  // Using discard to avoid CS0201 error
            
            // The array is allocated on the heap, but since we don't store
            // the reference in any variable, it becomes unreachable immediately
            // when this method returns
        }
        
        /// <summary>
        /// Demonstrates the difference between Debug and Release builds regarding
        /// object lifetime, as mentioned in the training material.
        /// </summary>
        public static void DemoDebugVsReleaseBehavior()
        {
            Console.WriteLine("=== Debug vs Release Object Lifetime ===");
            
            #if DEBUG
            Console.WriteLine("Running in DEBUG mode:");
            Console.WriteLine("- Object lifetime may extend to end of enclosing code block");
            Console.WriteLine("- This is done to simplify debugging");
            Console.WriteLine("- Objects remain reachable longer than strictly necessary");
            #else
            Console.WriteLine("Running in RELEASE mode:");
            Console.WriteLine("- Objects become eligible at earliest possible point");
            Console.WriteLine("- More aggressive optimization");
            Console.WriteLine("- Objects collected as soon as no longer actively used");
            #endif
            
            Console.WriteLine();
            DemoVariableLifetime();
        }
        
        /// <summary>
        /// Shows how variable lifetime affects object reachability.
        /// </summary>
        private static void DemoVariableLifetime()
        {
            Console.WriteLine("Demonstrating variable lifetime and object reachability:");
            
            {
                // Create object within a scope block
                byte[] scopedArray = new byte[1000];
                Console.WriteLine("Array created within scope block");
                
                // In Debug builds, scopedArray might remain reachable until
                // the end of this scope block
                // In Release builds, it might become eligible sooner if not used
                
            } // Scope block ends
            
            Console.WriteLine("Scope block ended - array reference is definitely out of scope");
            Console.WriteLine("Object is now eligible for collection in both Debug and Release");
        }
        
        /// <summary>
        /// Demonstrates how the GC balances collection frequency with memory consumption.
        /// Shows that applications might temporarily consume more memory than necessary.
        /// </summary>
        public static void DemoMemoryConsumptionBalance()
        {
            Console.WriteLine("=== GC Memory Consumption Balance ===");
            Console.WriteLine("The GC balances collection time vs. memory consumption");
            Console.WriteLine("Applications may temporarily use more memory than strictly needed");
            Console.WriteLine();
            
            // Show current memory state
            long initialMemory = GC.GetTotalMemory(false);
            Console.WriteLine($"Initial memory: {initialMemory:N0} bytes");
            
            // Create temporary objects that remain eligible for collection
            Console.WriteLine("Creating temporary objects...");
            for (int i = 0; i < 500; i++)
            {
                CreateTemporaryObject();
            }
            
            long afterAllocation = GC.GetTotalMemory(false);
            Console.WriteLine($"After allocation: {afterAllocation:N0} bytes");
            Console.WriteLine($"Memory increase: {afterAllocation - initialMemory:N0} bytes");
            Console.WriteLine();
            
            Console.WriteLine("These objects are eligible for collection but may not be collected yet");
            Console.WriteLine("The GC waits for the right moment to minimize application pauses");
            Console.WriteLine("This is why memory consumption may appear higher than necessary");
            
            // Show what happens when we force collection
            Console.WriteLine("\nForcing collection (for demonstration only)...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            long afterCollection = GC.GetTotalMemory(false);
            Console.WriteLine($"After forced collection: {afterCollection:N0} bytes");
            Console.WriteLine($"Memory reclaimed: {afterAllocation - afterCollection:N0} bytes");
        }
        
        /// <summary>
        /// Creates a temporary object that becomes immediately eligible for collection.
        /// </summary>
        private static void CreateTemporaryObject()
        {
            // Create object but don't store reference
            new SampleObject($"Temporary_{DateTime.Now.Ticks}");
        }
        
        /// <summary>
        /// Demonstrates programmatic memory monitoring using performance counters,
        /// as mentioned in the training material.
        /// </summary>
        public static void DemoMemoryMonitoring()
        {
            Console.WriteLine("=== Programmatic Memory Monitoring ===");
            Console.WriteLine("Monitoring memory using performance counters...");
            
            try
            {
                // This mirrors the exact code from the training material
                string procName = Process.GetCurrentProcess().ProcessName;
                using PerformanceCounter pc = new PerformanceCounter("Process", "Private Bytes", procName);
                
                long privateBytes = (long)pc.NextValue();
                Console.WriteLine($"Private Bytes: {privateBytes:N0} bytes ({privateBytes / 1024 / 1024:N1} MB)");
                Console.WriteLine();
                Console.WriteLine("Private Bytes provides the best overall indication of memory consumption");
                Console.WriteLine("It excludes memory that the CLR has deallocated internally");
                Console.WriteLine("and is willing to return to the OS if another process needs it");
                
                // Compare with GC memory
                long gcMemory = GC.GetTotalMemory(false);
                Console.WriteLine($"GC Total Memory: {gcMemory:N0} bytes ({gcMemory / 1024 / 1024:N1} MB)");
                Console.WriteLine();
                Console.WriteLine("The difference shows memory managed by the CLR but not");
                Console.WriteLine("currently allocated to managed objects");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Performance counter access failed: {ex.Message}");
                Console.WriteLine("This is common in restricted environments");
                Console.WriteLine("Alternative: Use Process.GetCurrentProcess().PrivateMemorySize64");
                
                // Alternative approach
                var process = Process.GetCurrentProcess();
                Console.WriteLine($"Private Memory Size: {process.PrivateMemorySize64:N0} bytes");
                Console.WriteLine($"Working Set: {process.WorkingSet64:N0} bytes");
            }
        }
    }
}
