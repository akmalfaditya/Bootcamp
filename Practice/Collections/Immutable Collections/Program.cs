using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace ImmutableCollectionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Immutable Collections in C# - Complete Guide ===");
            Console.WriteLine("Understanding immutability and how it changes the way we work with data\n");

            // Demonstrate each type of immutable collection
            ImmutableListDemo();
            ImmutableArrayDemo();
            ImmutableDictionaryDemo();
            ImmutableHashSetDemo();
            ImmutableStackQueueDemo();
            BuilderPatternsDemo();
            PerformanceComparisonsDemo();
            RealWorldScenariosDemo();

            Console.WriteLine("\n=== All Immutable Collections Demonstrations Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Shows how ImmutableList works - the most commonly used immutable collection
        /// Think of it as a List<T> that never changes, always returns new copies
        /// </summary>
        static void ImmutableListDemo()
        {
            Console.WriteLine("=== 1. ImmutableList<T> - The Immutable Workhorse ===");
            Console.WriteLine("Every operation returns a NEW list, original stays untouched\n");

            // Create an immutable list - several ways to do this
            var originalList = ImmutableList.Create(10, 20, 30, 40);
            Console.WriteLine($"Original list: [{string.Join(", ", originalList)}]");

            // Adding items - notice we get a NEW list back
            var listWithNewItem = originalList.Add(50);
            Console.WriteLine($"After adding 50: [{string.Join(", ", listWithNewItem)}]");
            Console.WriteLine($"Original unchanged: [{string.Join(", ", originalList)}]");

            // Insert at specific position
            var listWithInsert = originalList.Insert(2, 25);
            Console.WriteLine($"After inserting 25 at index 2: [{string.Join(", ", listWithInsert)}]");

            // Remove items
            var listWithRemoval = originalList.Remove(20);
            Console.WriteLine($"After removing 20: [{string.Join(", ", listWithRemoval)}]");

            // AddRange for multiple items - much more efficient than multiple Add calls
            var listWithRange = originalList.AddRange(new[] { 60, 70, 80 });
            Console.WriteLine($"After adding range: [{string.Join(", ", listWithRange)}]");

            // Converting from regular collections
            var regularList = new List<int> { 100, 200, 300 };
            var immutableFromRegular = regularList.ToImmutableList();
            Console.WriteLine($"Converted from List<T>: [{string.Join(", ", immutableFromRegular)}]");

            Console.WriteLine("Key point: Every 'change' creates a new list - original is never modified!\n");
        }

        /// <summary>
        /// ImmutableArray is like a fixed-size array that can't be changed
        /// Best for scenarios where you rarely modify but read a lot
        /// </summary>
        static void ImmutableArrayDemo()
        {
            Console.WriteLine("=== 2. ImmutableArray<T> - Fixed-Size Immutable Storage ===");
            Console.WriteLine("Fastest for reads, but expensive for modifications\n");

            // Create an immutable array
            var originalArray = ImmutableArray.Create("apple", "banana", "cherry");
            Console.WriteLine($"Original array: [{string.Join(", ", originalArray)}]");

            // Adding to an array creates entirely new array (expensive!)
            var arrayWithNewItem = originalArray.Add("date");
            Console.WriteLine($"After adding 'date': [{string.Join(", ", arrayWithNewItem)}]");

            // Removing from array (also expensive)
            var arrayWithRemoval = originalArray.Remove("banana");
            Console.WriteLine($"After removing 'banana': [{string.Join(", ", arrayWithRemoval)}]");

            // SetItem - replace at specific index
            var arrayWithReplacement = originalArray.SetItem(1, "blueberry");
            Console.WriteLine($"After replacing index 1: [{string.Join(", ", arrayWithReplacement)}]");

            // Create from regular array
            string[] regularArray = { "x", "y", "z" };
            var immutableFromArray = regularArray.ToImmutableArray();
            Console.WriteLine($"From regular array: [{string.Join(", ", immutableFromArray)}]");

            // Check if empty properly
            var emptyArray = ImmutableArray<string>.Empty;
            Console.WriteLine($"Empty array count: {emptyArray.Length}");
            Console.WriteLine($"Is default: {emptyArray.IsDefault}"); // Important check!

            Console.WriteLine("Use ImmutableArray when you read often, modify rarely\n");
        }

        /// <summary>
        /// ImmutableDictionary for key-value pairs that don't change
        /// Perfect for configuration data, lookup tables, etc.
        /// </summary>
        static void ImmutableDictionaryDemo()
        {
            Console.WriteLine("=== 3. ImmutableDictionary<TKey, TValue> - Unchanging Key-Value Pairs ===");
            Console.WriteLine("Great for configuration, caching, and lookup tables\n");            // Create an immutable dictionary
            var dictBuilder = ImmutableDictionary.CreateBuilder<string, int>();
            dictBuilder.Add("Alice", 25);
            dictBuilder.Add("Bob", 30);
            dictBuilder.Add("Charlie", 35);
            var originalDict = dictBuilder.ToImmutable();

            Console.WriteLine("Original dictionary:");
            foreach (var kvp in originalDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            // Add new key-value pair
            var dictWithNewPair = originalDict.Add("Diana", 28);
            Console.WriteLine($"\nAfter adding Diana: {dictWithNewPair.Count} items");

            // Update existing value
            var dictWithUpdate = originalDict.SetItem("Bob", 31);
            Console.WriteLine($"Bob's age updated to: {dictWithUpdate["Bob"]}");
            Console.WriteLine($"Original Bob's age still: {originalDict["Bob"]}");

            // Remove a key
            var dictWithRemoval = originalDict.Remove("Charlie");
            Console.WriteLine($"After removing Charlie: {dictWithRemoval.Count} items");

            // Safe value retrieval
            if (originalDict.TryGetValue("Alice", out int aliceAge))
            {
                Console.WriteLine($"Alice's age: {aliceAge}");
            }

            // Create from regular dictionary
            var regularDict = new Dictionary<string, string>
            {
                ["red"] = "#FF0000",
                ["green"] = "#00FF00",
                ["blue"] = "#0000FF"
            };
            var immutableColors = regularDict.ToImmutableDictionary();
            Console.WriteLine($"Color dictionary has {immutableColors.Count} colors");

            Console.WriteLine("Immutable dictionaries are thread-safe and perfect for shared state\n");
        }

        /// <summary>
        /// ImmutableHashSet for unique collections that don't change
        /// Great for maintaining lists of unique items
        /// </summary>
        static void ImmutableHashSetDemo()
        {
            Console.WriteLine("=== 4. ImmutableHashSet<T> - Unique Elements Collection ===");
            Console.WriteLine("Perfect for maintaining unique items without duplicates\n");

            // Create immutable hash set
            var originalSet = ImmutableHashSet.Create("apple", "banana", "cherry");
            Console.WriteLine($"Original set: [{string.Join(", ", originalSet)}]");

            // Add new item
            var setWithNewItem = originalSet.Add("date");
            Console.WriteLine($"After adding 'date': [{string.Join(", ", setWithNewItem)}]");

            // Try to add duplicate (no effect, but returns new set)
            var setWithDuplicate = originalSet.Add("apple");
            Console.WriteLine($"After adding duplicate 'apple': {setWithDuplicate.Count} items (same as before)");

            // Remove item
            var setWithRemoval = originalSet.Remove("banana");
            Console.WriteLine($"After removing 'banana': [{string.Join(", ", setWithRemoval)}]");

            // Set operations - union, intersect, except
            var otherSet = ImmutableHashSet.Create("cherry", "date", "elderberry");
            var unionSet = originalSet.Union(otherSet);
            Console.WriteLine($"Union with other set: [{string.Join(", ", unionSet)}]");

            var intersectSet = originalSet.Intersect(otherSet);
            Console.WriteLine($"Intersection: [{string.Join(", ", intersectSet)}]");

            // Check membership
            Console.WriteLine($"Contains 'apple': {originalSet.Contains("apple")}");
            Console.WriteLine($"Contains 'grape': {originalSet.Contains("grape")}");

            Console.WriteLine("HashSets automatically handle uniqueness - no duplicates ever!\n");
        }

        /// <summary>
        /// ImmutableStack and ImmutableQueue - specialized LIFO and FIFO collections
        /// These are less common but useful for specific algorithms
        /// </summary>
        static void ImmutableStackQueueDemo()
        {
            Console.WriteLine("=== 5. ImmutableStack<T> & ImmutableQueue<T> - Specialized Collections ===");
            Console.WriteLine("Stack = Last In First Out, Queue = First In First Out\n");

            // ImmutableStack demo
            Console.WriteLine("ImmutableStack (LIFO):");
            var emptyStack = ImmutableStack<string>.Empty;
            var stack = emptyStack.Push("First").Push("Second").Push("Third");

            Console.WriteLine($"Stack contents (top to bottom): {string.Join(" -> ", stack)}");
            Console.WriteLine($"Peek at top: {stack.Peek()}");

            var stackAfterPop = stack.Pop();
            Console.WriteLine($"After popping: {string.Join(" -> ", stackAfterPop)}");
            Console.WriteLine($"Original stack unchanged: {string.Join(" -> ", stack)}");

            // ImmutableQueue demo
            Console.WriteLine("\nImmutableQueue (FIFO):");
            var emptyQueue = ImmutableQueue<string>.Empty;
            var queue = emptyQueue.Enqueue("First").Enqueue("Second").Enqueue("Third");

            Console.WriteLine($"Queue contents: {string.Join(" -> ", queue)}");
            Console.WriteLine($"Peek at front: {queue.Peek()}");

            var queueAfterDequeue = queue.Dequeue();
            Console.WriteLine($"After dequeuing: {string.Join(" -> ", queueAfterDequeue)}");

            Console.WriteLine("These collections are perfect for algorithms that need LIFO/FIFO behavior\n");
        }

        /// <summary>
        /// Builders are the secret to efficient immutable collection manipulation
        /// Use them when you need to make many changes before finalizing
        /// </summary>
        static void BuilderPatternsDemo()
        {
            Console.WriteLine("=== 6. Builder Patterns - Efficient Batch Operations ===");
            Console.WriteLine("When you need to make lots of changes, builders are your friend\n");

            // Without builder - inefficient for many operations
            Console.WriteLine("Without builder (inefficient for many changes):");
            var stopwatch = Stopwatch.StartNew();
            var inefficientList = ImmutableList<int>.Empty;
            for (int i = 0; i < 1000; i++)
            {
                inefficientList = inefficientList.Add(i); // Creates new list each time!
            }
            stopwatch.Stop();
            Console.WriteLine($"Adding 1000 items without builder: {stopwatch.ElapsedMilliseconds} ms");

            // With builder - much more efficient
            Console.WriteLine("\nWith builder (efficient for batch operations):");
            stopwatch.Restart();
            var builder = ImmutableList.CreateBuilder<int>();
            for (int i = 0; i < 1000; i++)
            {
                builder.Add(i); // Modifies internal mutable structure
            }
            var efficientList = builder.ToImmutable(); // Convert to immutable once
            stopwatch.Stop();
            Console.WriteLine($"Adding 1000 items with builder: {stopwatch.ElapsedMilliseconds} ms");

            // Builder operations look like regular collections
            var listBuilder = ImmutableList.CreateBuilder<string>();
            listBuilder.Add("First");
            listBuilder.Add("Second");
            listBuilder.Add("Third");
            listBuilder.Insert(1, "Middle");
            listBuilder.Remove("Second");

            var finalList = listBuilder.ToImmutable();
            Console.WriteLine($"Final list: [{string.Join(", ", finalList)}]");

            // Dictionary builder example
            var dictBuilder = ImmutableDictionary.CreateBuilder<string, int>();
            dictBuilder.Add("apples", 5);
            dictBuilder.Add("bananas", 3);
            dictBuilder.Add("cherries", 8);
            dictBuilder["bananas"] = 6; // Update value

            var finalDict = dictBuilder.ToImmutable();
            Console.WriteLine("Final dictionary:");
            foreach (var kvp in finalDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("Rule of thumb: Use builders for 3+ operations, direct methods for 1-2\n");
        }

        /// <summary>
        /// Performance comparison between immutable and mutable collections
        /// Understanding the trade-offs helps you make informed decisions
        /// </summary>
        static void PerformanceComparisonsDemo()
        {
            Console.WriteLine("=== 7. Performance Comparisons - Understanding the Trade-offs ===");
            Console.WriteLine("Immutable collections prioritize safety over raw speed\n");

            const int itemCount = 100000;

            // List<T> vs ImmutableList<T> - Read performance
            var mutableList = Enumerable.Range(1, itemCount).ToList();
            var immutableList = mutableList.ToImmutableList();

            // Read performance comparison
            var stopwatch = Stopwatch.StartNew();
            int sum1 = 0;
            for (int i = 0; i < mutableList.Count; i++)
            {
                sum1 += mutableList[i];
            }
            var mutableReadTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            int sum2 = 0;
            for (int i = 0; i < immutableList.Count; i++)
            {
                sum2 += immutableList[i];
            }
            var immutableReadTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("Read Performance (100,000 items):");
            Console.WriteLine($"  List<T>: {mutableReadTime} ms");
            Console.WriteLine($"  ImmutableList<T>: {immutableReadTime} ms");

            // Array vs ImmutableArray - Read performance
            var array = Enumerable.Range(1, itemCount).ToArray();
            var immutableArray = array.ToImmutableArray();

            stopwatch.Restart();
            int sum3 = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum3 += array[i];
            }
            var arrayReadTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            int sum4 = 0;
            for (int i = 0; i < immutableArray.Length; i++)
            {
                sum4 += immutableArray[i];
            }
            var immutableArrayReadTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("\nRead Performance - Arrays (100,000 items):");
            Console.WriteLine($"  Array: {arrayReadTime} ms");
            Console.WriteLine($"  ImmutableArray<T>: {immutableArrayReadTime} ms");

            Console.WriteLine("\nKey takeaways:");
            Console.WriteLine("- ImmutableArray is nearly as fast as arrays for reads");
            Console.WriteLine("- ImmutableList is slower but still acceptable for most scenarios");
            Console.WriteLine("- Choose based on your read/write patterns and safety requirements\n");
        }

        /// <summary>
        /// Real-world scenarios where immutable collections shine
        /// These examples show practical applications beyond toy examples
        /// </summary>
        static void RealWorldScenariosDemo()
        {
            Console.WriteLine("=== 8. Real-World Scenarios - Where Immutability Shines ===");
            Console.WriteLine("Practical examples of when immutable collections are the right choice\n");

            // Scenario 1: Configuration that shouldn't change during runtime
            Console.WriteLine("Scenario 1: Application Configuration");
            var configBuilder = ImmutableDictionary.CreateBuilder<string, string>();
            configBuilder["DatabaseConnection"] = "Server=localhost;Database=MyApp";
            configBuilder["ApiKey"] = "abc123def456";
            configBuilder["MaxRetries"] = "3";
            configBuilder["Timeout"] = "30";

            var appConfig = configBuilder.ToImmutable();
            Console.WriteLine("Configuration loaded:");
            foreach (var setting in appConfig)
            {
                Console.WriteLine($"  {setting.Key}: {setting.Value}");
            }
            Console.WriteLine("Configuration is immutable - prevents accidental changes during runtime");

            // Scenario 2: Event history that should never be modified
            Console.WriteLine("\nScenario 2: Event History Log");
            var eventHistory = ImmutableList<UserEvent>.Empty;
            eventHistory = eventHistory.Add(new UserEvent("UserLogin", DateTime.Now.AddMinutes(-30), "user123"));
            eventHistory = eventHistory.Add(new UserEvent("PageView", DateTime.Now.AddMinutes(-25), "user123"));
            eventHistory = eventHistory.Add(new UserEvent("Purchase", DateTime.Now.AddMinutes(-10), "user123"));

            Console.WriteLine("User event history:");
            foreach (var evt in eventHistory)
            {
                Console.WriteLine($"  {evt.Timestamp:HH:mm:ss} - {evt.Action} by {evt.UserId}");
            }
            Console.WriteLine("Event history is immutable - ensures audit trail integrity");

            // Scenario 3: Shared state in multi-threaded environment
            Console.WriteLine("\nScenario 3: Thread-Safe Shared Cache");
            var cache = ImmutableDictionary<string, object>.Empty;
            cache = cache.Add("user:123", new { Name = "Alice", Role = "Admin" });
            cache = cache.Add("user:456", new { Name = "Bob", Role = "User" });

            // Multiple threads can safely read from this cache without locks
            Console.WriteLine("Thread-safe cache contents:");
            foreach (var item in cache)
            {
                Console.WriteLine($"  {item.Key}: {item.Value}");
            }
            Console.WriteLine("Cache is immutable - safe for concurrent access without locks");

            // Scenario 4: Functional programming style transformations
            Console.WriteLine("\nScenario 4: Functional Data Transformations");
            var numbers = ImmutableList.Create(1, 2, 3, 4, 5);
            var doubled = numbers.Select(x => x * 2).ToImmutableList();
            var filtered = numbers.Where(x => x % 2 == 0).ToImmutableList();
            var aggregated = numbers.Aggregate(0, (acc, x) => acc + x);

            Console.WriteLine($"Original: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Doubled: [{string.Join(", ", doubled)}]");
            Console.WriteLine($"Even only: [{string.Join(", ", filtered)}]");
            Console.WriteLine($"Sum: {aggregated}");
            Console.WriteLine("Functional transformations work naturally with immutable collections");

            Console.WriteLine("\nWhen to choose immutable collections:");
            Console.WriteLine("✓ Configuration data that shouldn't change");
            Console.WriteLine("✓ Shared state in multi-threaded applications");
            Console.WriteLine("✓ Event sourcing and audit trails");
            Console.WriteLine("✓ Functional programming patterns");
            Console.WriteLine("✓ When you need to prevent accidental mutations");
            Console.WriteLine("✗ High-performance scenarios with frequent modifications");
            Console.WriteLine("✗ Large collections that change constantly\n");
        }
    }

    // Helper class for event history demo
    public class UserEvent
    {
        public string Action { get; }
        public DateTime Timestamp { get; }
        public string UserId { get; }

        public UserEvent(string action, DateTime timestamp, string userId)
        {
            Action = action;
            Timestamp = timestamp;
            UserId = userId;
        }
    }
}
