using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ImmutableCollectionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Immutable Collections in C# - Complete Training Guide ===");
            Console.WriteLine("Understanding immutability and how it changes the way we work with data");
            Console.WriteLine("Key principle: Once created, immutable collections NEVER change - they return NEW collections instead\n");

            // Core concepts and benefits
            ImmutabilityBenefitsDemo();
            
            // All collection types with internal structure explanations
            ImmutableListDemo();
            ImmutableArrayDemo();
            ImmutableDictionaryDemo();
            ImmutableHashSetDemo();
            ImmutableSortedCollectionsDemo();
            ImmutableStackQueueDemo();
            
            // Advanced patterns and optimization
            CreationPatternsDemo();
            BuilderPatternsDemo();
            StructuralSharingDemo();
            PerformanceComparisonsDemo();
            ThreadSafetyDemo();
            RealWorldScenariosDemo();
            
            Console.WriteLine("\n=== Training Complete - You now understand immutable collections! ===");
            Console.WriteLine("Remember: Immutability = safety, predictability, and thread-safety");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// ImmutableList<T> - Uses AVL Tree internally for balanced performance
        /// This is your go-to immutable collection for most scenarios
        /// </summary>
        static void ImmutableListDemo()
        {
            Console.WriteLine("=== 2. ImmutableList<T> - AVL Tree Based Collection ===");
            Console.WriteLine("Internal Structure: AVL Tree (self-balancing binary search tree)");
            Console.WriteLine("Performance: O(log n) for add/remove, O(log n) for access by index");
            Console.WriteLine("Best for: Balanced read/write scenarios with structural sharing benefits\n");

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
        /// ImmutableArray<T> - Uses Array internally for maximum read performance
        /// Trade-off: Very fast reads, very slow writes (no structural sharing)
        /// </summary>
        static void ImmutableArrayDemo()
        {
            Console.WriteLine("=== 3. ImmutableArray<T> - Array Based Collection ===");
            Console.WriteLine("Internal Structure: Plain Array");
            Console.WriteLine("Performance: O(1) for reads, O(n) for add/remove (copies entire array)");
            Console.WriteLine("Best for: Read-heavy scenarios where modifications are rare\n");

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
        /// ImmutableSortedDictionary and ImmutableSortedSet - AVL Tree based sorted collections
        /// These maintain sort order automatically while providing immutability
        /// </summary>
        static void ImmutableSortedCollectionsDemo()
        {
            Console.WriteLine("=== 6. ImmutableSortedDictionary & ImmutableSortedSet - Ordered Collections ===");
            Console.WriteLine("Internal Structure: AVL Tree (maintains sort order)");
            Console.WriteLine("Performance: O(log n) for all operations");
            Console.WriteLine("Best for: When you need both immutability AND sorted order\n");

            // ImmutableSortedDictionary demo
            Console.WriteLine("ImmutableSortedDictionary - Keys always sorted:");
            var sortedDict = ImmutableSortedDictionary<string, int>.Empty;
            sortedDict = sortedDict.Add("zebra", 26)
                                   .Add("apple", 1)
                                   .Add("banana", 2)
                                   .Add("cherry", 3);

            Console.WriteLine("Dictionary contents (automatically sorted by key):");
            foreach (var kvp in sortedDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            // ImmutableSortedSet demo
            Console.WriteLine("\nImmutableSortedSet - Values always sorted:");
            var sortedSet = ImmutableSortedSet<int>.Empty;
            sortedSet = sortedSet.Add(50)
                                 .Add(10)
                                 .Add(30)
                                 .Add(20)
                                 .Add(40);

            Console.WriteLine($"Set contents (automatically sorted): [{string.Join(", ", sortedSet)}]");

            // Custom comparer example
            var reverseSet = ImmutableSortedSet.Create(Comparer<string>.Create((x, y) => y.CompareTo(x)),
                "apple", "banana", "cherry");
            Console.WriteLine($"Reverse sorted strings: [{string.Join(", ", reverseSet)}]");

            Console.WriteLine("Use these when you need guaranteed sort order with immutability benefits\n");
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
        /// Performance comparison showing the specific trade-offs mentioned in training material
        /// Understanding these numbers helps you choose the right collection type
        /// </summary>
        static void PerformanceComparisonsDemo()
        {
            Console.WriteLine("=== 10. Performance Comparisons - Understanding the Trade-offs ===");
            Console.WriteLine("Real numbers to help you make informed decisions\n");

            Console.WriteLine("Performance Characteristics Summary:");
            Console.WriteLine("| Type                  | Read Performance | Add Performance | Internal Structure |");
            Console.WriteLine("|----------------------|------------------|-----------------|-------------------|");
            Console.WriteLine("| ImmutableList<T>     | Slow (O(log n)) | Slow (O(log n)) | AVL Tree          |");
            Console.WriteLine("| ImmutableArray<T>    | Very Fast (O(1))| Very Slow (O(n))| Array             |");
            Console.WriteLine("| List<T>              | Very Fast (O(1))| Fast (O(1)*)    | Array             |");
            Console.WriteLine("| Array                | Very Fast (O(1))| N/A (fixed)     | Array             |\n");

            const int itemCount = 100000;
            const int iterations = 1000;

            // Setup test data
            var mutableList = Enumerable.Range(1, itemCount).ToList();
            var immutableList = mutableList.ToImmutableList();
            var array = mutableList.ToArray();
            var immutableArray = array.ToImmutableArray();

            Console.WriteLine($"Testing with {itemCount:N0} items, {iterations:N0} iterations\n");

            // Read performance comparison
            Console.WriteLine("READ PERFORMANCE TEST:");
            
            // List<T> read test
            var stopwatch = Stopwatch.StartNew();
            long sum1 = 0;
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < mutableList.Count; i++)
                {
                    sum1 += mutableList[i];
                }
            }
            var mutableListReadTime = stopwatch.ElapsedMilliseconds;

            // ImmutableList<T> read test
            stopwatch.Restart();
            long sum2 = 0;
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < immutableList.Count; i++)
                {
                    sum2 += immutableList[i];
                }
            }
            var immutableListReadTime = stopwatch.ElapsedMilliseconds;

            // Array read test
            stopwatch.Restart();
            long sum3 = 0;
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    sum3 += array[i];
                }
            }
            var arrayReadTime = stopwatch.ElapsedMilliseconds;

            // ImmutableArray<T> read test
            stopwatch.Restart();
            long sum4 = 0;
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < immutableArray.Length; i++)
                {
                    sum4 += immutableArray[i];
                }
            }
            var immutableArrayReadTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"List<T>:           {mutableListReadTime,4} ms (baseline)");
            Console.WriteLine($"ImmutableList<T>:  {immutableListReadTime,4} ms ({(double)immutableListReadTime/mutableListReadTime:F1}x slower)");
            Console.WriteLine($"Array:             {arrayReadTime,4} ms ({(double)arrayReadTime/mutableListReadTime:F1}x vs List)");
            Console.WriteLine($"ImmutableArray<T>: {immutableArrayReadTime,4} ms ({(double)immutableArrayReadTime/mutableListReadTime:F1}x vs List)");

            // Add performance comparison
            Console.WriteLine("\nADD PERFORMANCE TEST (1000 additions):");
            const int addCount = 1000;

            // List<T> add test
            stopwatch.Restart();
            var testList = new List<int>(mutableList);
            for (int i = 0; i < addCount; i++)
            {
                testList.Add(i);
            }
            var mutableListAddTime = stopwatch.ElapsedMilliseconds;

            // ImmutableList<T> add test (without builder)
            stopwatch.Restart();
            var testImmutableList = immutableList;
            for (int i = 0; i < addCount; i++)
            {
                testImmutableList = testImmutableList.Add(i);
            }
            var immutableListAddTime = stopwatch.ElapsedMilliseconds;

            // ImmutableArray<T> add test (very expensive)
            stopwatch.Restart();
            var testImmutableArray = ImmutableArray.Create(1, 2, 3); // Start small
            for (int i = 0; i < 100; i++) // Only 100 iterations - would be too slow otherwise
            {
                testImmutableArray = testImmutableArray.Add(i);
            }
            var immutableArrayAddTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"List<T> (1000 adds):           {mutableListAddTime,4} ms");
            Console.WriteLine($"ImmutableList<T> (1000 adds):  {immutableListAddTime,4} ms ({(double)immutableListAddTime/mutableListAddTime:F0}x slower)");
            Console.WriteLine($"ImmutableArray<T> (100 adds):  {immutableArrayAddTime,4} ms (extremely slow - creates new array each time)");

            // Builder performance
            Console.WriteLine("\nBUILDER PERFORMANCE (1000 additions):");
            stopwatch.Restart();
            var builder = ImmutableList.CreateBuilder<int>();
            for (int i = 0; i < addCount; i++)
            {
                builder.Add(i);
            }
            var builderResult = builder.ToImmutable();
            var builderAddTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"ImmutableList.Builder:  {builderAddTime,4} ms (much faster than individual adds)");

            Console.WriteLine("\nKEY INSIGHTS:");
            Console.WriteLine($"• ImmutableList can be {(double)immutableListReadTime/mutableListReadTime:F0}-{(double)immutableListReadTime/mutableListReadTime + 5:F0}x slower for reads (but often acceptable)");
            Console.WriteLine($"• ImmutableList can be {(double)immutableListAddTime/Math.Max(mutableListAddTime, 1):F0}+ times slower for individual adds");
            Console.WriteLine("• ImmutableArray has excellent read performance (nearly identical to arrays)");
            Console.WriteLine("• ImmutableArray has terrible add performance (avoid frequent modifications)");
            Console.WriteLine("• Builders dramatically improve performance for batch operations");
            Console.WriteLine("• Choose based on your read/write ratio and concurrency needs\n");
        }

        /// <summary>
        /// Real-world scenarios where immutable collections solve actual business problems
        /// These examples show practical applications you'll encounter in professional development
        /// </summary>
        static void RealWorldScenariosDemo()
        {
            Console.WriteLine("=== 12. Real-World Scenarios - Solving Business Problems ===");
            Console.WriteLine("Practical examples where immutability provides real value\n");

            // Scenario 1: Configuration management
            Console.WriteLine("Scenario 1: Application Configuration Management");
            var configBuilder = ImmutableDictionary.CreateBuilder<string, object>();
            configBuilder["DatabaseConnectionString"] = "Server=prod-db;Database=MyApp;Integrated Security=true";
            configBuilder["ApiTimeout"] = TimeSpan.FromSeconds(30);
            configBuilder["MaxRetries"] = 3;
            configBuilder["EnableLogging"] = true;
            configBuilder["CacheExpirationMinutes"] = 60;

            var appConfig = configBuilder.ToImmutable();
            Console.WriteLine("✓ Configuration loaded and locked - cannot be accidentally modified");
            Console.WriteLine($"  Database: {appConfig["DatabaseConnectionString"]}");
            Console.WriteLine($"  Timeout: {appConfig["ApiTimeout"]}");
            Console.WriteLine($"  Retries: {appConfig["MaxRetries"]}");
            Console.WriteLine("  Benefit: Thread-safe access across entire application, no defensive copying needed");

            // Scenario 2: Event sourcing system
            Console.WriteLine("\nScenario 2: Event Sourcing / Audit Trail System");
            var eventStore = new EventStore();
            eventStore.AppendEvent(new DomainEvent("OrderCreated", DateTime.Now.AddMinutes(-30), "order-123", "{ orderId: 123, customerId: 456 }"));
            eventStore.AppendEvent(new DomainEvent("PaymentProcessed", DateTime.Now.AddMinutes(-25), "order-123", "{ amount: 99.99, method: 'CreditCard' }"));
            eventStore.AppendEvent(new DomainEvent("OrderShipped", DateTime.Now.AddMinutes(-10), "order-123", "{ trackingNumber: 'TRK789' }"));

            var recentEvents = eventStore.GetEventsAfter(DateTime.Now.AddMinutes(-35));
            Console.WriteLine("✓ Event history preserved immutably:");
            foreach (var evt in recentEvents)
            {
                Console.WriteLine($"  {evt.Timestamp:HH:mm:ss} - {evt.EventType} for {evt.AggregateId}");
            }
            Console.WriteLine("  Benefit: Audit trail integrity guaranteed, safe concurrent access");

            // Scenario 3: State management in UI applications
            Console.WriteLine("\nScenario 3: UI State Management (Redux-style pattern)");
            var initialState = new ApplicationState(
                ImmutableList<User>.Empty,
                ImmutableDictionary<string, string>.Empty,
                false
            );

            // Simulate state transitions
            var withUsers = initialState.AddUser(new User("alice", "Alice Smith", "alice@example.com"));
            var withMoreUsers = withUsers.AddUser(new User("bob", "Bob Jones", "bob@example.com"));
            var withNotification = withMoreUsers.SetNotification("users-loaded", "Users loaded successfully");

            Console.WriteLine($"✓ State evolution tracked immutably:");
            Console.WriteLine($"  Initial state: {initialState.Users.Count} users");
            Console.WriteLine($"  After adding Alice: {withUsers.Users.Count} users");
            Console.WriteLine($"  Final state: {withMoreUsers.Users.Count} users, {withNotification.Notifications.Count} notifications");
            Console.WriteLine("  Benefit: Predictable state changes, easy undo/redo, time-travel debugging");

            // Scenario 4: Caching with snapshots
            Console.WriteLine("\nScenario 4: Thread-Safe Caching with Snapshots");
            var cache = new ThreadSafeCache<string, object>();
            cache.Set("user:123", new { Name = "Alice", Department = "Engineering" });
            cache.Set("user:456", new { Name = "Bob", Department = "Sales" });
            cache.Set("config:timeout", 30);

            var snapshot1 = cache.CreateSnapshot();
            Console.WriteLine($"✓ Snapshot 1 created: {snapshot1.Count} items");

            cache.Set("user:789", new { Name = "Charlie", Department = "Marketing" });
            cache.Remove("config:timeout");

            var snapshot2 = cache.CreateSnapshot();
            Console.WriteLine($"✓ Snapshot 2 created: {snapshot2.Count} items");
            Console.WriteLine($"  Snapshot 1 still has: {snapshot1.Count} items (unchanged)");
            Console.WriteLine("  Benefit: Instant snapshots, safe to share across threads, no copying overhead");

            // Scenario 5: Functional programming style data processing
            Console.WriteLine("\nScenario 5: Functional Data Processing Pipeline");
            var salesData = ImmutableList.Create(
                new SaleRecord("2024-01-01", "Electronics", 1500m),
                new SaleRecord("2024-01-02", "Clothing", 750m),
                new SaleRecord("2024-01-03", "Electronics", 2200m),
                new SaleRecord("2024-01-04", "Books", 300m),
                new SaleRecord("2024-01-05", "Electronics", 1800m)
            );

            // Functional pipeline - each step returns new immutable collection
            var processedSales = salesData
                .Where(sale => sale.Amount > 1000m)  // Filter high-value sales
                .GroupBy(sale => sale.Category)       // Group by category
                .Select(group => new CategorySummary(
                    group.Key,
                    group.Count(),
                    group.Sum(s => s.Amount)
                ))
                .ToImmutableList();

            Console.WriteLine("✓ Sales data processed functionally:");
            foreach (var summary in processedSales)
            {
                Console.WriteLine($"  {summary.Category}: {summary.Count} sales, ${summary.TotalAmount:N2}");
            }
            Console.WriteLine("  Benefit: No side effects, composable operations, safe parallel processing");

            Console.WriteLine("\n=== When to Choose Immutable Collections ===");
            Console.WriteLine("✓ Configuration data that shouldn't change during runtime");
            Console.WriteLine("✓ Shared state in multi-threaded applications");
            Console.WriteLine("✓ Event sourcing and audit trail systems");
            Console.WriteLine("✓ UI state management (Redux, MVVM patterns)");
            Console.WriteLine("✓ Functional programming and data transformation pipelines");
            Console.WriteLine("✓ Caching systems that need snapshots");
            Console.WriteLine("✓ When you need to prevent accidental mutations");
            Console.WriteLine("✗ High-performance scenarios with frequent large-scale modifications");
            Console.WriteLine("✗ Very large collections that change constantly");
            Console.WriteLine("✗ Memory-constrained environments where object creation overhead matters\n");
        }

        /// <summary>
        /// Demonstrates the core benefits of immutability - why this approach matters
        /// This is the foundation that makes everything else worth learning
        /// </summary>
        static void ImmutabilityBenefitsDemo()
        {
            Console.WriteLine("=== 1. Why Immutability Matters - The Core Benefits ===");
            Console.WriteLine("Understanding the 'why' before the 'how'\n");

            // Benefit 1: Reduced bugs through predictable state
            Console.WriteLine("Benefit 1: Reduced Bugs (Predictable State)");
            var originalData = ImmutableList.Create("Important", "Data", "Here");
            
            // Simulate passing data to methods
            var processedData = SomeMethodThatMightModifyData(originalData);
            var anotherProcessedData = AnotherMethodThatMightModifyData(originalData);
            
            Console.WriteLine($"Original data after processing: [{string.Join(", ", originalData)}]");
            Console.WriteLine("^ Original data is GUARANTEED to be unchanged - no defensive programming needed!");
            
            // Benefit 2: Thread safety
            Console.WriteLine("\nBenefit 2: Thread Safety (No Locks Required)");
            var sharedState = ImmutableDictionary.Create<string, int>()
                .Add("counter1", 0)
                .Add("counter2", 0);
                
            Console.WriteLine("Multiple threads can safely read and 'modify' without locks");
            Console.WriteLine("Each thread gets its own version - no race conditions possible");
            
            // Benefit 3: Easier reasoning and debugging
            Console.WriteLine("\nBenefit 3: Easier Code Reasoning");
            Console.WriteLine("If you have an immutable object, you know:");
            Console.WriteLine("  ✓ It will never change unexpectedly");
            Console.WriteLine("  ✓ You can safely return it from methods");
            Console.WriteLine("  ✓ You can store it without worrying about external changes");
            Console.WriteLine("  ✓ Debugging is simpler - state doesn't change behind your back");
            
            // Benefit 4: No defensive copying
            Console.WriteLine("\nBenefit 4: No Defensive Copying Overhead");
            Console.WriteLine("With mutable collections, you often need to copy to protect yourself:");
            var mutableList = new List<string> { "item1", "item2" };
            var defensiveCopy = new List<string>(mutableList); // Expensive copy operation
            
            Console.WriteLine("With immutable collections, you can safely share the original:");
            var immutableList = ImmutableList.Create("item1", "item2");
            // No copying needed - safe to share directly!
            
            Console.WriteLine("\nThe trade-off: Slower individual modifications, but safer overall architecture");
            Console.WriteLine("Perfect for: Configuration, shared state, event logs, undo systems\n");
        }

        /// <summary>
        /// Helper methods to demonstrate data safety
        /// </summary>
        static ImmutableList<string> SomeMethodThatMightModifyData(ImmutableList<string> data)
        {
            // Even if we try to "modify", we can only return a new version
            return data.Add("Modified");
        }
        
        static ImmutableList<string> AnotherMethodThatMightModifyData(ImmutableList<string> data)
        {
            return data.Remove("Data");
        }

        /// <summary>
        /// Shows all the different ways to create immutable collections
        /// Understanding creation patterns is crucial for efficient usage
        /// </summary>
        static void CreationPatternsDemo()
        {
            Console.WriteLine("=== 7. Creation Patterns - Different Ways to Create Immutable Collections ===");
            Console.WriteLine("Choose the right creation method for your scenario\n");

            // Method 1: Static Create methods
            Console.WriteLine("Method 1: Static Create() methods");
            var listFromCreate = ImmutableList.Create(1, 2, 3, 4, 5);
            var arrayFromCreate = ImmutableArray.Create("a", "b", "c");
            var dictFromCreate = ImmutableDictionary.Create<string, int>()
                .Add("one", 1)
                .Add("two", 2);
            Console.WriteLine($"List: [{string.Join(", ", listFromCreate)}]");
            Console.WriteLine($"Array: [{string.Join(", ", arrayFromCreate)}]");
            Console.WriteLine($"Dictionary: {dictFromCreate.Count} items");

            // Method 2: CreateRange methods
            Console.WriteLine("\nMethod 2: CreateRange() from existing IEnumerable");
            var existingData = new[] { 10, 20, 30, 40, 50 };
            var listFromRange = ImmutableList.CreateRange(existingData);
            var arrayFromRange = ImmutableArray.CreateRange(existingData);
            Console.WriteLine($"List from range: [{string.Join(", ", listFromRange)}]");
            Console.WriteLine($"Array from range: [{string.Join(", ", arrayFromRange)}]");

            // Method 3: Extension methods (ToImmutableXXX)
            Console.WriteLine("\nMethod 3: ToImmutableXXX() extension methods");
            var regularList = new List<string> { "convert", "me", "to", "immutable" };
            var immutableFromList = regularList.ToImmutableList();
            var immutableArrayFromList = regularList.ToImmutableArray();
            Console.WriteLine($"From List<T>: [{string.Join(", ", immutableFromList)}]");
            Console.WriteLine($"As Array: [{string.Join(", ", immutableArrayFromList)}]");

            // Method 4: Dictionary-specific creation
            Console.WriteLine("\nMethod 4: Dictionary creation patterns");
            var keyValuePairs = new[]
            {
                new KeyValuePair<string, int>("first", 1),
                new KeyValuePair<string, int>("second", 2),
                new KeyValuePair<string, int>("third", 3)
            };
            var dictFromPairs = ImmutableDictionary.CreateRange(keyValuePairs);
            
            // From existing dictionary
            var regularDict = new Dictionary<string, string>
            {
                ["red"] = "#FF0000",
                ["green"] = "#00FF00",
                ["blue"] = "#0000FF"
            };
            var immutableDict = regularDict.ToImmutableDictionary();
            Console.WriteLine($"From KeyValuePairs: {dictFromPairs.Count} items");
            Console.WriteLine($"From Dictionary: {immutableDict.Count} items");

            // Method 5: Empty collections
            Console.WriteLine("\nMethod 5: Starting with empty collections");
            var emptyList = ImmutableList<int>.Empty;
            var emptyArray = ImmutableArray<int>.Empty;
            var emptyDict = ImmutableDictionary<string, int>.Empty;
            Console.WriteLine($"Empty list count: {emptyList.Count}");
            Console.WriteLine($"Empty array length: {emptyArray.Length}");
            Console.WriteLine($"Empty dict count: {emptyDict.Count}");

            Console.WriteLine("\nRule of thumb: Use Create() for small sets, CreateRange() for existing data, ToImmutableXXX() for conversions\n");
        }

        /// <summary>
        /// Demonstrates how structural sharing works - the secret to immutable collection efficiency
        /// This is why immutable collections aren't as expensive as you might think
        /// </summary>
        static void StructuralSharingDemo()
        {
            Console.WriteLine("=== 9. Structural Sharing - The Secret to Efficiency ===");
            Console.WriteLine("How immutable collections avoid copying everything every time\n");

            // Demonstrate structural sharing concept
            Console.WriteLine("Structural Sharing Concept:");
            Console.WriteLine("When you 'modify' an immutable collection, only the changed parts are new");
            Console.WriteLine("Unchanged parts are shared between old and new versions\n");

            // Example with lists
            var originalList = ImmutableList.Create(1, 2, 3, 4, 5);
            var modifiedList = originalList.Insert(2, 99); // Insert at index 2
            var anotherModified = originalList.Add(6);     // Add at end

            Console.WriteLine($"Original:  [{string.Join(", ", originalList)}]");
            Console.WriteLine($"Modified:  [{string.Join(", ", modifiedList)}]");
            Console.WriteLine($"Another:   [{string.Join(", ", anotherModified)}]");
            Console.WriteLine("Memory-wise: These three collections share most of their internal structure");

            // Memory efficiency demonstration
            Console.WriteLine("\nMemory Efficiency Example:");
            var baseList = ImmutableList<int>.Empty;
            var versions = new List<ImmutableList<int>>();

            // Create multiple versions by adding items
            for (int i = 0; i < 10; i++)
            {
                baseList = baseList.Add(i);
                versions.Add(baseList);
            }

            Console.WriteLine($"Created {versions.Count} versions of the list");
            Console.WriteLine($"Version 1: [{string.Join(", ", versions[0])}]");
            Console.WriteLine($"Version 5: [{string.Join(", ", versions[4])}]");
            Console.WriteLine($"Version 10: [{string.Join(", ", versions[9])}]");
            Console.WriteLine("All versions share internal structure where possible - minimal memory overhead");

            // Contrast with arrays (no structural sharing)
            Console.WriteLine("\nContrast with ImmutableArray (no structural sharing):");
            var originalArray = ImmutableArray.Create(1, 2, 3, 4, 5);
            var modifiedArray = originalArray.Add(6);
            Console.WriteLine($"Original array: [{string.Join(", ", originalArray)}]");
            Console.WriteLine($"Modified array: [{string.Join(", ", modifiedArray)}]");
            Console.WriteLine("Array modification creates a complete copy - no sharing possible");

            Console.WriteLine("\nKey insight: This is why ImmutableList is often better than ImmutableArray for frequent modifications");
            Console.WriteLine("The AVL tree structure allows efficient sharing, plain arrays don't\n");
        }

        /// <summary>
        /// Demonstrates thread safety benefits - no locks needed with immutable collections
        /// This is one of the biggest advantages in multi-threaded applications
        /// </summary>
        static void ThreadSafetyDemo()
        {
            Console.WriteLine("=== 11. Thread Safety - The Concurrency Advantage ===");
            Console.WriteLine("Immutable collections are inherently thread-safe - no locks required!\n");

            // Shared state example
            var sharedData = ImmutableList.Create(1, 2, 3, 4, 5);
            var results = new List<ImmutableList<int>>();
            var lockObject = new object();

            Console.WriteLine("Demonstrating concurrent access to shared immutable data:");
            Console.WriteLine($"Starting data: [{string.Join(", ", sharedData)}]");

            // Simulate multiple threads working with the same data
            var tasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                int threadId = i;
                tasks[i] = Task.Run(() =>
                {
                    // Each thread can safely read and create new versions
                    var localVersion = sharedData.Add(threadId * 10);
                    
                    // Safe to add to results (though we still need to synchronize the list itself)
                    lock (lockObject)
                    {
                        results.Add(localVersion);
                    }
                    
                    Console.WriteLine($"Thread {threadId} created: [{string.Join(", ", localVersion)}]");
                });
            }

            Task.WaitAll(tasks);

            Console.WriteLine($"\nOriginal data after all threads: [{string.Join(", ", sharedData)}]");
            Console.WriteLine("^ Notice: Original data is completely unchanged!");
            Console.WriteLine($"Created {results.Count} different versions, all sharing structure with the original");

            // Compare with thread-safe scenario for caching
            Console.WriteLine("\nThread-Safe Caching Example:");
            ThreadSafeCacheExample();

            // Parallel processing example
            Console.WriteLine("\nParallel Processing Example:");
            ParallelProcessingExample();

            Console.WriteLine("Key advantage: No race conditions, no locks needed for the immutable data itself\n");
        }

        static void ThreadSafeCacheExample()
        {
            var cache = ImmutableDictionary<string, string>.Empty;
            
            // Simulate multiple threads updating cache
            var updateTasks = Enumerable.Range(0, 10).Select(i => Task.Run(() =>
            {
                var key = $"key{i}";
                var value = $"value{i}";
                
                // This would typically use Interlocked.Exchange in real scenarios
                // Here we're just showing the concept
                var newCache = cache.Add(key, value);
                Console.WriteLine($"Thread created cache version with {newCache.Count} items");
                return newCache;
            })).ToArray();

            Task.WaitAll(updateTasks);
            Console.WriteLine("All cache updates completed safely");
        }

        static void ParallelProcessingExample()
        {
            var data = ImmutableList.CreateRange(Enumerable.Range(1, 1000));
            
            // Parallel processing with no synchronization needed
            var processed = data.AsParallel()
                               .Select(x => x * x)  // Square each number
                               .Where(x => x % 2 == 0)  // Keep even results
                               .ToImmutableList();

            Console.WriteLine($"Processed {data.Count} items in parallel");
            Console.WriteLine($"Result: {processed.Count} even squares");
            Console.WriteLine($"Sample results: [{string.Join(", ", processed.Take(10))}]...");
        }
    }

    // Helper classes for the real-world scenarios

    // Original helper class
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

    // Event sourcing classes
    public class DomainEvent
    {
        public string EventType { get; }
        public DateTime Timestamp { get; }
        public string AggregateId { get; }
        public string Data { get; }

        public DomainEvent(string eventType, DateTime timestamp, string aggregateId, string data)
        {
            EventType = eventType;
            Timestamp = timestamp;
            AggregateId = aggregateId;
            Data = data;
        }
    }

    public class EventStore
    {
        private ImmutableList<DomainEvent> _events = ImmutableList<DomainEvent>.Empty;

        public void AppendEvent(DomainEvent domainEvent)
        {
            // In real implementation, you'd use thread-safe operations like Interlocked.Exchange
            _events = _events.Add(domainEvent);
        }

        public ImmutableList<DomainEvent> GetEventsAfter(DateTime timestamp)
        {
            return _events.Where(e => e.Timestamp >= timestamp).ToImmutableList();
        }

        public ImmutableList<DomainEvent> GetAllEvents()
        {
            return _events;
        }
    }

    // UI State management classes
    public class User
    {
        public string Id { get; }
        public string Name { get; }
        public string Email { get; }

        public User(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }

    public class ApplicationState
    {
        public ImmutableList<User> Users { get; }
        public ImmutableDictionary<string, string> Notifications { get; }
        public bool IsLoading { get; }

        public ApplicationState(ImmutableList<User> users, ImmutableDictionary<string, string> notifications, bool isLoading)
        {
            Users = users;
            Notifications = notifications;
            IsLoading = isLoading;
        }

        public ApplicationState AddUser(User user)
        {
            return new ApplicationState(Users.Add(user), Notifications, IsLoading);
        }

        public ApplicationState SetNotification(string key, string message)
        {
            return new ApplicationState(Users, Notifications.SetItem(key, message), IsLoading);
        }

        public ApplicationState SetLoading(bool loading)
        {
            return new ApplicationState(Users, Notifications, loading);
        }
    }

    // Caching classes
    public class ThreadSafeCache<TKey, TValue> where TKey : notnull
    {
        private volatile ImmutableDictionary<TKey, TValue> _cache = ImmutableDictionary<TKey, TValue>.Empty;

        public void Set(TKey key, TValue value)
        {
            // Thread-safe update using Interlocked
            ImmutableDictionary<TKey, TValue> current, updated;
            do
            {
                current = _cache;
                updated = current.SetItem(key, value);
            } while (Interlocked.CompareExchange(ref _cache, updated, current) != current);
        }

        public void Remove(TKey key)
        {
            ImmutableDictionary<TKey, TValue> current, updated;
            do
            {
                current = _cache;
                updated = current.Remove(key);
            } while (Interlocked.CompareExchange(ref _cache, updated, current) != current);
        }

        public ImmutableDictionary<TKey, TValue> CreateSnapshot()
        {
            return _cache; // Instant snapshot - no copying needed!
        }

        public bool TryGetValue(TKey key, out TValue? value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }

    // Sales data processing classes
    public class SaleRecord
    {
        public string Date { get; }
        public string Category { get; }
        public decimal Amount { get; }

        public SaleRecord(string date, string category, decimal amount)
        {
            Date = date;
            Category = category;
            Amount = amount;
        }
    }

    public class CategorySummary
    {
        public string Category { get; }
        public int Count { get; }
        public decimal TotalAmount { get; }

        public CategorySummary(string category, int count, decimal totalAmount)
        {
            Category = category;
            Count = count;
            TotalAmount = totalAmount;
        }
    }
}
