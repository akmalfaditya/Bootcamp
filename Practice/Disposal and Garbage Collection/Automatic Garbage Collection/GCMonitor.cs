using System;
using System.Diagnostics;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// A utility class for monitoring garbage collection performance and memory usage.
    /// This class provides helper methods to track GC behavior and memory patterns
    /// in a more detailed way for advanced analysis.
    /// </summary>
    public static class GCMonitor
    {
        /// <summary>
        /// Captures a snapshot of current GC and memory state.
        /// This is useful for comparing states before and after operations.
        /// </summary>
        public static GCSnapshot TakeSnapshot(string description = "")
        {
            return new GCSnapshot
            {
                Description = description,
                Timestamp = DateTime.Now,
                Gen0Collections = GC.CollectionCount(0),
                Gen1Collections = GC.CollectionCount(1),
                Gen2Collections = GC.CollectionCount(2),
                TotalMemory = GC.GetTotalMemory(false),
                WorkingSet = Process.GetCurrentProcess().WorkingSet64,
                PrivateMemory = Process.GetCurrentProcess().PrivateMemorySize64
            };
        }

        /// <summary>
        /// Compares two GC snapshots and shows the differences.
        /// This helps analyze what happened between two points in time.
        /// </summary>
        public static void CompareSnapshots(GCSnapshot before, GCSnapshot after)
        {
            Console.WriteLine($"GC Analysis: {before.Description} → {after.Description}");
            Console.WriteLine($"Time elapsed: {(after.Timestamp - before.Timestamp).TotalMilliseconds:F1} ms");
            Console.WriteLine($"Gen 0 collections: {before.Gen0Collections} → {after.Gen0Collections} (+{after.Gen0Collections - before.Gen0Collections})");
            Console.WriteLine($"Gen 1 collections: {before.Gen1Collections} → {after.Gen1Collections} (+{after.Gen1Collections - before.Gen1Collections})");
            Console.WriteLine($"Gen 2 collections: {before.Gen2Collections} → {after.Gen2Collections} (+{after.Gen2Collections - before.Gen2Collections})");
            
            long memoryDelta = after.TotalMemory - before.TotalMemory;
            Console.WriteLine($"Total memory: {before.TotalMemory:N0} → {after.TotalMemory:N0} ({memoryDelta:+#,0;-#,0;0} bytes)");
            
            long workingSetDelta = after.WorkingSet - before.WorkingSet;
            Console.WriteLine($"Working set: {before.WorkingSet / 1024 / 1024:N0} MB → {after.WorkingSet / 1024 / 1024:N0} MB ({workingSetDelta / 1024 / 1024:+#,0;-#,0;0} MB)");
        }

        /// <summary>
        /// Executes an action while monitoring its impact on garbage collection.
        /// Returns a report of what happened during the action execution.
        /// </summary>
        public static GCImpactReport MonitorAction(string actionName, Action action)
        {
            var before = TakeSnapshot($"Before {actionName}");
            
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            
            var after = TakeSnapshot($"After {actionName}");

            return new GCImpactReport
            {
                ActionName = actionName,
                ExecutionTime = stopwatch.Elapsed,
                BeforeSnapshot = before,
                AfterSnapshot = after,
                Gen0CollectionsTriggered = after.Gen0Collections - before.Gen0Collections,
                Gen1CollectionsTriggered = after.Gen1Collections - before.Gen1Collections,
                Gen2CollectionsTriggered = after.Gen2Collections - before.Gen2Collections,
                MemoryAllocated = Math.Max(0, after.TotalMemory - before.TotalMemory),
                MemoryReclaimed = Math.Max(0, before.TotalMemory - after.TotalMemory)
            };
        }

        /// <summary>
        /// Prints a detailed report of garbage collection impact.
        /// </summary>
        public static void PrintImpactReport(GCImpactReport report)
        {
            Console.WriteLine($"=== GC Impact Report: {report.ActionName} ===");
            Console.WriteLine($"Execution time: {report.ExecutionTime.TotalMilliseconds:F1} ms");
            Console.WriteLine($"Collections triggered:");
            Console.WriteLine($"  Gen 0: {report.Gen0CollectionsTriggered}");
            Console.WriteLine($"  Gen 1: {report.Gen1CollectionsTriggered}");
            Console.WriteLine($"  Gen 2: {report.Gen2CollectionsTriggered}");
            
            if (report.MemoryAllocated > 0)
                Console.WriteLine($"Memory allocated: {report.MemoryAllocated:N0} bytes");
            
            if (report.MemoryReclaimed > 0)
                Console.WriteLine($"Memory reclaimed: {report.MemoryReclaimed:N0} bytes");
                
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Represents a snapshot of garbage collection and memory state at a specific point in time.
    /// </summary>
    public class GCSnapshot
    {
        public string Description { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public int Gen0Collections { get; set; }
        public int Gen1Collections { get; set; }
        public int Gen2Collections { get; set; }
        public long TotalMemory { get; set; }
        public long WorkingSet { get; set; }
        public long PrivateMemory { get; set; }
    }

    /// <summary>
    /// Represents the garbage collection impact of executing a specific action.
    /// </summary>
    public class GCImpactReport
    {
        public string ActionName { get; set; } = "";
        public TimeSpan ExecutionTime { get; set; }
        public GCSnapshot BeforeSnapshot { get; set; } = new();
        public GCSnapshot AfterSnapshot { get; set; } = new();
        public int Gen0CollectionsTriggered { get; set; }
        public int Gen1CollectionsTriggered { get; set; }
        public int Gen2CollectionsTriggered { get; set; }
        public long MemoryAllocated { get; set; }
        public long MemoryReclaimed { get; set; }
    }
}
