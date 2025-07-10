using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// Comprehensive demonstration of generational garbage collection concepts
    /// mentioned in the training material. Shows how the generational hypothesis
    /// works in practice and what factors influence GC behavior.
    /// </summary>
    public static class GenerationalGCDemo
    {
        /// <summary>
        /// Demonstrates the complete generational garbage collection system
        /// mentioned in the training material, including all factors that influence
        /// when collections occur.
        /// </summary>
        public static void DemoGenerationalSystem()
        {
            Console.WriteLine("=== GENERATIONAL GARBAGE COLLECTION SYSTEM ===");
            Console.WriteLine("The GC doesn't collect all eligible garbage with every cycle");
            Console.WriteLine("Objects are divided into generations for optimization");
            Console.WriteLine();
            
            DemoGenerationalHypothesis();
            DemoGenerationPromotionProcess();
            DemoCollectionFactors();
            DemoGenerationFrequencies();
        }
        
        /// <summary>
        /// Demonstrates the "generational hypothesis" mentioned in the training material:
        /// - Most objects are short-lived
        /// - If an object survives initial collection, it's likely to live longer
        /// </summary>
        private static void DemoGenerationalHypothesis()
        {
            Console.WriteLine("1. THE GENERATIONAL HYPOTHESIS");
            Console.WriteLine("==============================");
            Console.WriteLine("Key assumptions:");
            Console.WriteLine("- Most objects are short-lived");
            Console.WriteLine("- Objects that survive initial collection tend to live longer");
            Console.WriteLine();
            
            Console.WriteLine("Creating 1000 short-lived objects...");
            var shortLivedStart = DateTime.Now;
            
            // Create many short-lived objects
            for (int i = 0; i < 1000; i++)
            {
                // These objects become unreachable immediately
                new SampleObject($"ShortLived_{i}");
            }
            
            var shortLivedTime = DateTime.Now - shortLivedStart;
            Console.WriteLine($"Short-lived objects created in {shortLivedTime.TotalMilliseconds:F1}ms");
            Console.WriteLine("These objects are immediately eligible for collection");
            
            Console.WriteLine("\nCreating long-lived objects...");
            var longLivedObjects = CreateLongLivedObjects(100);
            
            Console.WriteLine($"Created {longLivedObjects.Count} long-lived objects");
            Console.WriteLine("These objects will survive multiple collection cycles");
            
            // Show generation behavior
            Console.WriteLine("\nInitial generation distribution:");
            ShowGenerationDistribution(longLivedObjects);
            
            Console.WriteLine("\nForcing Gen 0 collection...");
            GC.Collect(0);
            
            Console.WriteLine("After Gen 0 collection:");
            ShowGenerationDistribution(longLivedObjects);
            
            Console.WriteLine("\nKey insight: Short-lived objects are collected quickly from Gen 0");
            Console.WriteLine("Long-lived objects get promoted to higher generations");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Shows how objects are promoted through generations.
        /// </summary>
        private static void DemoGenerationPromotionProcess()
        {
            Console.WriteLine("2. GENERATION PROMOTION PROCESS");
            Console.WriteLine("================================");
            Console.WriteLine("Generation 0: Newly allocated, short-lived objects");
            Console.WriteLine("Generation 1: Objects that survived Gen 0 collection");
            Console.WriteLine("Generation 2: Objects that survived Gen 1 collection");
            Console.WriteLine();
            
            var testObjects = CreateTestObjects();
            Console.WriteLine($"Created {testObjects.Count} test objects");
            
            // Show promotion through generations
            for (int cycle = 0; cycle < 3; cycle++)
            {
                Console.WriteLine($"\n--- Collection Cycle {cycle + 1} ---");
                
                // Create some new Gen 0 objects
                CreateGenerationZeroObjects(200);
                
                Console.WriteLine("Before collection:");
                ShowGenerationDistribution(testObjects);
                
                // Collect Gen 0 only
                GC.Collect(0);
                
                Console.WriteLine("After Gen 0 collection:");
                ShowGenerationDistribution(testObjects);
                
                Console.WriteLine("Objects that survived were promoted to higher generation");
            }
            
            Console.WriteLine("\nKey insight: Objects prove their longevity by surviving collections");
            Console.WriteLine("Each survival earns promotion to a higher, less-frequently-collected generation");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates the factors that influence when garbage collection occurs,
        /// as mentioned in the training material.
        /// </summary>
        private static void DemoCollectionFactors()
        {
            Console.WriteLine("3. FACTORS THAT TRIGGER GARBAGE COLLECTION");
            Console.WriteLine("==========================================");
            Console.WriteLine("The GC dynamically self-tunes based on:");
            Console.WriteLine("1. Available Memory (is memory becoming scarce?)");
            Console.WriteLine("2. Amount of Memory Allocation (significant new allocations?)");
            Console.WriteLine("3. Time Since Last Collection (balancing collection frequency)");
            Console.WriteLine();
            
            // Factor 1: Available Memory
            DemoMemoryPressureFactor();
            
            // Factor 2: Amount of Memory Allocation
            DemoAllocationPressureFactor();
            
            // Factor 3: Time Since Last Collection
            DemoTimingFactor();
        }
        
        /// <summary>
        /// Shows how available memory affects collection frequency.
        /// </summary>
        private static void DemoMemoryPressureFactor()
        {
            Console.WriteLine("Factor 1: Available Memory Pressure");
            Console.WriteLine("-----------------------------------");
            
            // Get baseline
            var initialCollections = GetCollectionCounts();
            Console.WriteLine("Creating memory pressure by allocating large objects...");
            
            var memoryHogs = new List<byte[]>();
            
            // Allocate increasingly large amounts of memory
            for (int i = 1; i <= 10; i++)
            {
                // Each iteration allocates more memory
                int allocationSize = i * 1_000_000; // 1MB, 2MB, 3MB, etc.
                memoryHogs.Add(new byte[allocationSize]);
                
                if (i % 3 == 0)
                {
                    var currentCollections = GetCollectionCounts();
                    Console.WriteLine($"After {i * 1_000_000 / 1_000_000}MB allocated:");
                    Console.WriteLine($"  Gen 0: {currentCollections.Gen0 - initialCollections.Gen0} collections");
                    Console.WriteLine($"  Gen 1: {currentCollections.Gen1 - initialCollections.Gen1} collections");
                    Console.WriteLine($"  Gen 2: {currentCollections.Gen2 - initialCollections.Gen2} collections");
                }
            }
            
            Console.WriteLine("Memory pressure triggers more frequent collections");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Shows how allocation rate affects collection frequency.
        /// </summary>
        private static void DemoAllocationPressureFactor()
        {
            Console.WriteLine("Factor 2: Allocation Rate Pressure");
            Console.WriteLine("----------------------------------");
            
            var initialCollections = GetCollectionCounts();
            Console.WriteLine("Creating allocation pressure through rapid object creation...");
            
            // Rapid allocation of many small objects
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                // Create objects that immediately become unreachable
                new SampleObject($"Pressure_{i}");
                
                if (i % 2000 == 0 && i > 0)
                {
                    var currentCollections = GetCollectionCounts();
                    Console.WriteLine($"After {i} rapid allocations:");
                    Console.WriteLine($"  Gen 0: {currentCollections.Gen0 - initialCollections.Gen0} collections");
                }
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Rapid allocation completed in {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine("High allocation rate triggers frequent Gen 0 collections");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Shows how time since last collection affects GC behavior.
        /// </summary>
        private static void DemoTimingFactor()
        {
            Console.WriteLine("Factor 3: Time Since Last Collection");
            Console.WriteLine("------------------------------------");
            
            var initialCollections = GetCollectionCounts();
            Console.WriteLine("The GC balances collection frequency to avoid excessive pauses");
            Console.WriteLine("It won't collect too frequently (wasteful) or too infrequently (memory pressure)");
            Console.WriteLine();
            
            // Show current timing
            Console.WriteLine($"Current collection counts: Gen0={initialCollections.Gen0}, Gen1={initialCollections.Gen1}, Gen2={initialCollections.Gen2}");
            Console.WriteLine("The GC has been tuning itself based on application behavior");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates the different collection frequencies for each generation.
        /// </summary>
        private static void DemoGenerationFrequencies()
        {
            Console.WriteLine("4. GENERATION COLLECTION FREQUENCIES");
            Console.WriteLine("====================================");
            Console.WriteLine("Generation 0: Collected most frequently (short-lived objects)");
            Console.WriteLine("Generation 1: Collected less frequently (intermediate objects)");
            Console.WriteLine("Generation 2: Collected least frequently (long-lived objects)");
            Console.WriteLine();
            
            var before = GetCollectionCounts();
            
            // Create workload that will trigger collections
            Console.WriteLine("Creating workload to trigger collections...");
            
            // Keep some objects alive across collections
            var survivors = new List<SampleObject>();
            
            for (int iteration = 0; iteration < 5; iteration++)
            {
                Console.WriteLine($"\nIteration {iteration + 1}:");
                
                // Create many Gen 0 objects
                for (int i = 0; i < 1000; i++)
                {
                    var obj = new SampleObject($"Iteration_{iteration}_Object_{i}");
                    
                    // Keep some objects alive to become survivors
                    if (i % 100 == 0)
                    {
                        survivors.Add(obj);
                    }
                }
                
                // Show collection pattern
                var current = GetCollectionCounts();
                Console.WriteLine($"  Gen 0 collections: {current.Gen0 - before.Gen0}");
                Console.WriteLine($"  Gen 1 collections: {current.Gen1 - before.Gen1}");
                Console.WriteLine($"  Gen 2 collections: {current.Gen2 - before.Gen2}");
                Console.WriteLine($"  Survivor objects: {survivors.Count}");
            }
            
            var after = GetCollectionCounts();
            Console.WriteLine($"\nFinal collection pattern:");
            Console.WriteLine($"Gen 0 collections: {after.Gen0 - before.Gen0} (most frequent)");
            Console.WriteLine($"Gen 1 collections: {after.Gen1 - before.Gen1} (less frequent)");
            Console.WriteLine($"Gen 2 collections: {after.Gen2 - before.Gen2} (least frequent)");
            
            Console.WriteLine("\nThis pattern optimizes performance by:");
            Console.WriteLine("- Quickly collecting short-lived objects from Gen 0");
            Console.WriteLine("- Rarely disturbing long-lived objects in Gen 2");
            Console.WriteLine("- Minimizing work required for each collection");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Creates objects that will be used to test generation behavior.
        /// </summary>
        private static List<SampleObject> CreateTestObjects()
        {
            var objects = new List<SampleObject>();
            
            for (int i = 0; i < 50; i++)
            {
                objects.Add(new SampleObject($"TestObject_{i}"));
            }
            
            return objects;
        }
        
        /// <summary>
        /// Creates objects that will survive multiple collection cycles.
        /// </summary>
        private static List<SampleObject> CreateLongLivedObjects(int count)
        {
            var objects = new List<SampleObject>();
            
            for (int i = 0; i < count; i++)
            {
                objects.Add(new SampleObject($"LongLived_{i}"));
            }
            
            return objects;
        }
        
        /// <summary>
        /// Creates objects that will be allocated in Generation 0.
        /// </summary>
        private static void CreateGenerationZeroObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                // Create objects without keeping references
                new SampleObject($"Gen0_Temp_{i}");
            }
        }
        
        /// <summary>
        /// Shows what generation each object is currently in.
        /// </summary>
        private static void ShowGenerationDistribution(List<SampleObject> objects)
        {
            var gen0Count = objects.Count(obj => GC.GetGeneration(obj) == 0);
            var gen1Count = objects.Count(obj => GC.GetGeneration(obj) == 1);
            var gen2Count = objects.Count(obj => GC.GetGeneration(obj) == 2);
            
            Console.WriteLine($"  Gen 0: {gen0Count} objects");
            Console.WriteLine($"  Gen 1: {gen1Count} objects");
            Console.WriteLine($"  Gen 2: {gen2Count} objects");
        }
        
        /// <summary>
        /// Gets current collection counts for all generations.
        /// </summary>
        private static (int Gen0, int Gen1, int Gen2) GetCollectionCounts()
        {
            return (
                GC.CollectionCount(0),
                GC.CollectionCount(1),
                GC.CollectionCount(2)
            );
        }
        
        /// <summary>
        /// Demonstrates the indeterminate delay mentioned in the training material.
        /// Shows that there's a delay between eligibility and actual collection.
        /// </summary>
        public static void DemoIndeterminateDelay()
        {
            Console.WriteLine("=== INDETERMINATE DELAY IN GARBAGE COLLECTION ===");
            Console.WriteLine("There's an indeterminate delay between object eligibility and collection");
            Console.WriteLine("This delay can range from nanoseconds to much longer periods");
            Console.WriteLine();
            
            var timings = new List<long>();
            
            Console.WriteLine("Measuring delay between eligibility and collection...");
            
            for (int test = 0; test < 5; test++)
            {
                var stopwatch = Stopwatch.StartNew();
                
                // Create objects that become immediately eligible
                CreateEligibleObjects();
                
                // Force collection to see when it actually happens
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                
                stopwatch.Stop();
                timings.Add(stopwatch.ElapsedMilliseconds);
                
                Console.WriteLine($"Test {test + 1}: {stopwatch.ElapsedMilliseconds}ms from eligibility to collection");
            }
            
            Console.WriteLine($"\nAverage delay: {timings.Average():F1}ms");
            Console.WriteLine($"Min delay: {timings.Min()}ms");
            Console.WriteLine($"Max delay: {timings.Max()}ms");
            
            Console.WriteLine("\nKey insight: Collection timing depends on:");
            Console.WriteLine("- System load and available resources");
            Console.WriteLine("- GC heuristics and self-tuning algorithms");
            Console.WriteLine("- Other applications competing for memory");
            Console.WriteLine("- Application's specific memory access patterns");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Creates objects that become immediately eligible for collection.
        /// </summary>
        private static void CreateEligibleObjects()
        {
            for (int i = 0; i < 100; i++)
            {
                new SampleObject($"Eligible_{i}");
            }
        }
    }
}
