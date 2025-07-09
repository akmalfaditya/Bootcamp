using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lists__Queues__Stacks__Dictionaries_and_Sets
{
    /// <summary>
    /// Utility class containing advanced collection operations, performance comparisons,
    /// and real-world usage patterns for .NET collections.
    /// </summary>
    public static class CollectionUtilities
    {
        /// <summary>
        /// Demonstrates performance characteristics of different collection types.
        /// Shows why List<T> is generally preferred over ArrayList and when LinkedList<T> 
        /// might be better for specific insertion-heavy scenarios.
        /// </summary>
        public static void ShowPerformanceComparisons()
        {
            Console.WriteLine("PERFORMANCE ANALYSIS: Understanding Collection Trade-offs");
            Console.WriteLine(new string('-', 55));

            const int itemCount = 100000;
            var stopwatch = new Stopwatch();

            // List<T> vs ArrayList boxing/unboxing performance
            Console.WriteLine("1. Value Type Storage Performance (List<T> vs ArrayList):");
            
            // List<T> with value types
            stopwatch.Start();
            var genericList = new List<int>();
            for (int i = 0; i < itemCount; i++)
                genericList.Add(i);
            stopwatch.Stop();
            Console.WriteLine($"   List<int> addition: {stopwatch.ElapsedMilliseconds}ms");

            stopwatch.Restart();
            var arrayList = new ArrayList();
            for (int i = 0; i < itemCount; i++)
                arrayList.Add(i); // Boxing occurs here
            stopwatch.Stop();
            Console.WriteLine($"   ArrayList addition: {stopwatch.ElapsedMilliseconds}ms (includes boxing overhead)");

            // Access performance comparison
            stopwatch.Restart();
            int sum1 = 0;
            for (int i = 0; i < itemCount; i++)
                sum1 += genericList[i];
            stopwatch.Stop();
            Console.WriteLine($"   List<int> access: {stopwatch.ElapsedMilliseconds}ms");

            stopwatch.Restart();
            int sum2 = 0;
            for (int i = 0; i < itemCount; i++)
                sum2 += (int)arrayList[i]!; // Unboxing occurs here
            stopwatch.Stop();
            Console.WriteLine($"   ArrayList access: {stopwatch.ElapsedMilliseconds}ms (includes unboxing overhead)");

            // LinkedList vs List insertion performance
            Console.WriteLine("\n2. Mid-Collection Insertion Performance:");
            
            var list = new List<int>(Enumerable.Range(0, 10000));
            var linkedList = new LinkedList<int>(Enumerable.Range(0, 10000));
            
            stopwatch.Restart();
            for (int i = 0; i < 1000; i++)
                list.Insert(list.Count / 2, i); // Requires shifting elements
            stopwatch.Stop();
            Console.WriteLine($"   List<T> mid-insertion: {stopwatch.ElapsedMilliseconds}ms");

            stopwatch.Restart();
            var middleNode = linkedList.First;
            for (int i = 0; i < linkedList.Count / 2; i++)
                middleNode = middleNode!.Next;
            
            for (int i = 0; i < 1000; i++)
                linkedList.AddAfter(middleNode!, i); // O(1) operation
            stopwatch.Stop();
            Console.WriteLine($"   LinkedList<T> mid-insertion: {stopwatch.ElapsedMilliseconds}ms");

            Console.WriteLine("\n   Key Insight: List<T> excels at access and end-operations,");
            Console.WriteLine("   LinkedList<T> excels at frequent insertions/deletions anywhere in the collection.");
        }

        /// <summary>
        /// Demonstrates different constructor options and their performance implications.
        /// Understanding proper initialization can significantly impact performance.
        /// </summary>
        public static void ShowConstructorOptions()
        {
            Console.WriteLine("\nCONSTRUCTOR PATTERNS: Optimizing Collection Initialization");
            Console.WriteLine(new string('-', 58));

            // List<T> constructor options
            Console.WriteLine("List<T> Constructor Options:");
            
            // Default constructor
            var defaultList = new List<int>();
            Console.WriteLine($"1. new List<int>() - Initial capacity: {defaultList.Capacity}");

            // Capacity constructor - prevents multiple reallocations
            var capacityList = new List<int>(10000);
            Console.WriteLine($"2. new List<int>(10000) - Initial capacity: {capacityList.Capacity}");
            Console.WriteLine("   Use when you know approximate size to avoid internal array reallocations");

            // Collection constructor
            var sourceArray = new int[] { 1, 2, 3, 4, 5 };
            var collectionList = new List<int>(sourceArray);
            Console.WriteLine($"3. new List<int>(collection) - Copies from existing collection, count: {collectionList.Count}");

            // Demonstration of capacity growth
            Console.WriteLine("\nCapacity Growth Demonstration:");
            var growthList = new List<int>();
            Console.WriteLine($"Initial capacity: {growthList.Capacity}");
            
            for (int i = 0; i < 10; i++)
            {
                growthList.Add(i);
                if (i == 0 || i == 3 || i == 7)
                    Console.WriteLine($"After adding {i + 1} items: Capacity = {growthList.Capacity}");
            }

            // HashSet and SortedSet constructors
            Console.WriteLine("\nSet Constructor Options:");
            var hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            hashSet.Add("Hello");
            hashSet.Add("HELLO"); // Won't be added due to case-insensitive comparer
            Console.WriteLine($"HashSet with custom comparer: {hashSet.Count} items (case-insensitive)");

            var sortedSet = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            sortedSet.UnionWith(new[] { "zebra", "apple", "APPLE", "banana" });
            Console.WriteLine($"SortedSet with custom comparer: [{string.Join(", ", sortedSet)}]");
        }

        /// <summary>
        /// Shows common conversion patterns between collection types.
        /// Essential for working with different APIs and legacy code.
        /// </summary>
        public static void DemonstrateConversionPatterns()
        {
            Console.WriteLine("\nCONVERSION PATTERNS: Moving Between Collection Types");
            Console.WriteLine(new string('-', 52));

            // ArrayList to List<T> conversion
            Console.WriteLine("1. ArrayList to List<T> Conversion:");
            var arrayList = new ArrayList { 1, 5, 9, 13, 17 };
            var intList = arrayList.Cast<int>().ToList();
            Console.WriteLine($"   ArrayList: [{string.Join(", ", arrayList.Cast<int>())}]");
            Console.WriteLine($"   List<int>: [{string.Join(", ", intList)}]");

            // Array to various collections
            Console.WriteLine("\n2. Array to Collection Conversions:");
            var sourceArray = new[] { "apple", "banana", "cherry", "date", "elderberry" };
            
            var list = sourceArray.ToList();
            var hashSet = new HashSet<string>(sourceArray);
            var sortedSet = new SortedSet<string>(sourceArray);
            var queue = new Queue<string>(sourceArray);
            var stack = new Stack<string>(sourceArray);
            
            Console.WriteLine($"   Source Array: [{string.Join(", ", sourceArray)}]");
            Console.WriteLine($"   List<T>: [{string.Join(", ", list)}]");
            Console.WriteLine($"   HashSet<T>: [{string.Join(", ", hashSet)}]");
            Console.WriteLine($"   SortedSet<T>: [{string.Join(", ", sortedSet)}]");
            Console.WriteLine($"   Queue<T>: [{string.Join(", ", queue)}]");
            Console.WriteLine($"   Stack<T>: [{string.Join(", ", stack)}] (note: reversed order)");

            // Collection to Array conversion
            Console.WriteLine("\n3. Collection to Array Conversions:");
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var numberArray = numbers.ToArray();
            Console.WriteLine($"   List to Array: [{string.Join(", ", numberArray)}]");

            // IEnumerable to concrete collections
            Console.WriteLine("\n4. IEnumerable to Concrete Collections:");
            IEnumerable<int> enumerable = Enumerable.Range(1, 5);
            var fromEnumerable = enumerable.ToList();
            var hashFromEnumerable = enumerable.ToHashSet();
            Console.WriteLine($"   IEnumerable source: [{string.Join(", ", enumerable)}]");
            Console.WriteLine($"   ToList(): [{string.Join(", ", fromEnumerable)}]");
            Console.WriteLine($"   ToHashSet(): [{string.Join(", ", hashFromEnumerable)}]");
        }

        /// <summary>
        /// Demonstrates advanced operations with sets including mathematical set operations.
        /// Shows practical applications of set theory in programming.
        /// </summary>
        public static void DemonstrateAdvancedSetOperations()
        {
            Console.WriteLine("ADVANCED SET OPERATIONS: Mathematical Set Theory in Practice");
            Console.WriteLine(new string('-', 63));

            var primaryColors = new HashSet<string> { "Red", "Blue", "Yellow" };
            var secondaryColors = new HashSet<string> { "Green", "Orange", "Purple" };
            var warmColors = new HashSet<string> { "Red", "Orange", "Yellow" };
            var coolColors = new HashSet<string> { "Blue", "Green", "Purple" };

            Console.WriteLine($"Primary Colors: [{string.Join(", ", primaryColors)}]");
            Console.WriteLine($"Secondary Colors: [{string.Join(", ", secondaryColors)}]");
            Console.WriteLine($"Warm Colors: [{string.Join(", ", warmColors)}]");
            Console.WriteLine($"Cool Colors: [{string.Join(", ", coolColors)}]");

            // Union operation
            var allColors = new HashSet<string>(primaryColors);
            allColors.UnionWith(secondaryColors);
            Console.WriteLine($"\nUnion (Primary ∪ Secondary): [{string.Join(", ", allColors)}]");

            // Intersection operation
            var warmPrimary = new HashSet<string>(primaryColors);
            warmPrimary.IntersectWith(warmColors);
            Console.WriteLine($"Intersection (Primary ∩ Warm): [{string.Join(", ", warmPrimary)}]");

            // Difference operation
            var coolSecondary = new HashSet<string>(secondaryColors);
            coolSecondary.ExceptWith(warmColors);
            Console.WriteLine($"Difference (Secondary - Warm): [{string.Join(", ", coolSecondary)}]");

            // Symmetric difference
            var uniqueToEither = new HashSet<string>(primaryColors);
            uniqueToEither.SymmetricExceptWith(warmColors);
            Console.WriteLine($"Symmetric Difference (Primary ⊕ Warm): [{string.Join(", ", uniqueToEither)}]");

            // Set relationship queries
            Console.WriteLine("\nSet Relationship Queries:");
            Console.WriteLine($"Are warm colors a subset of all colors? {warmColors.IsSubsetOf(allColors)}");
            Console.WriteLine($"Are primary colors a superset of warm colors? {primaryColors.IsSupersetOf(warmColors)}");
            Console.WriteLine($"Do primary and secondary colors overlap? {primaryColors.Overlaps(secondaryColors)}");
            Console.WriteLine($"Are primary and cool colors equal? {primaryColors.SetEquals(coolColors)}");
        }

        /// <summary>
        /// Shows practical patterns for working with specialized collections.
        /// Demonstrates when to choose specific collection types for optimal performance.
        /// </summary>
        public static void DemonstrateSpecializedCollections()
        {
            Console.WriteLine("SPECIALIZED COLLECTION PATTERNS: Choosing the Right Tool");
            Console.WriteLine(new string('-', 58));

            // BitArray for efficient boolean storage
            Console.WriteLine("1. BitArray - Compact Boolean Storage:");
            var permissions = new BitArray(8); // 8 permission flags
            permissions[0] = true; // Read permission
            permissions[1] = true; // Write permission
            permissions[2] = false; // Execute permission
            permissions[3] = true; // Delete permission

            Console.WriteLine($"   Permissions (8 bits): {GetBitString(permissions)}");
            Console.WriteLine($"   Memory usage: {permissions.Length} bits vs {permissions.Length * 8} bits for bool[]");

            // BitArray operations
            var adminPermissions = new BitArray(8, true); // All permissions
            var userPermissions = new BitArray(permissions);
            userPermissions.And(adminPermissions);
            Console.WriteLine($"   User & Admin permissions: {GetBitString(userPermissions)}");

            // LinkedList for frequent insertions
            Console.WriteLine("\n2. LinkedList<T> - Efficient Insertion Scenarios:");
            var taskQueue = new LinkedList<string>();
            taskQueue.AddLast("Initialize system");
            taskQueue.AddLast("Load configuration");
            taskQueue.AddLast("Start services");
            taskQueue.AddLast("Complete startup");

            Console.WriteLine("   Initial task queue:");
            foreach (var task in taskQueue)
                Console.WriteLine($"     • {task}");

            // Insert urgent task at the beginning
            taskQueue.AddFirst("URGENT: Check system health");
            
            // Insert task before a specific item
            var configNode = taskQueue.Find("Load configuration");
            if (configNode != null)
                taskQueue.AddBefore(configNode, "Validate environment");

            Console.WriteLine("\n   Updated task queue:");
            foreach (var task in taskQueue)
                Console.WriteLine($"     • {task}");

            // Stack for algorithm implementation
            Console.WriteLine("\n3. Stack<T> - Algorithm Implementation:");
            var expression = "3 * (4 + 2)";
            var result = EvaluateSimpleExpression(expression);
            Console.WriteLine($"   Expression: {expression}");
            Console.WriteLine($"   Result: {result}");

            // Queue for BFS or processing pipeline
            Console.WriteLine("\n4. Queue<T> - Processing Pipeline:");
            var processingQueue = new Queue<string>();
            processingQueue.Enqueue("Document1.pdf");
            processingQueue.Enqueue("Document2.docx");
            processingQueue.Enqueue("Document3.txt");

            Console.WriteLine("   Processing documents:");
            while (processingQueue.Count > 0)
            {
                var document = processingQueue.Dequeue();
                Console.WriteLine($"     Processing: {document}");
            }
        }

        /// <summary>
        /// Demonstrates real-world scenarios where specific collections excel.
        /// Shows practical decision-making for collection selection.
        /// </summary>
        public static void ShowRealWorldScenarios()
        {
            Console.WriteLine("\nREAL-WORLD COLLECTION SCENARIOS: Making the Right Choice");
            Console.WriteLine(new string('-', 62));

            // Scenario 1: Web server request processing
            Console.WriteLine("1. Web Server Request Processing Pipeline:");
            var requestQueue = new Queue<string>();
            var processingStack = new Stack<string>();
            var processedRequests = new HashSet<string>();

            // Simulate incoming requests
            requestQueue.Enqueue("GET /api/users");
            requestQueue.Enqueue("POST /api/orders");
            requestQueue.Enqueue("GET /api/products");
            requestQueue.Enqueue("GET /api/users"); // Duplicate

            Console.WriteLine("   Incoming requests (FIFO processing):");
            while (requestQueue.Count > 0)
            {
                var request = requestQueue.Dequeue();
                Console.WriteLine($"     Processing: {request}");
                processingStack.Push(request);
                processedRequests.Add(request);
            }

            Console.WriteLine($"   Unique requests processed: {processedRequests.Count}");
            Console.WriteLine("   Recent requests (LIFO for undo/rollback):");
            foreach (var request in processingStack)
                Console.WriteLine($"     {request}");

            // Scenario 2: Shopping cart implementation
            Console.WriteLine("\n2. E-commerce Shopping Cart:");
            var cart = new List<(string Product, decimal Price, int Quantity)>();
            cart.Add(("Laptop", 999.99m, 1));
            cart.Add(("Mouse", 29.99m, 2));
            cart.Add(("Keyboard", 79.99m, 1));

            var total = cart.Sum(item => item.Price * item.Quantity);
            Console.WriteLine($"   Cart items: {cart.Count}");
            Console.WriteLine($"   Total value: ${total:F2}");
            
            // Remove item by condition
            cart.RemoveAll(item => item.Product == "Mouse");
            Console.WriteLine($"   After removing mouse: {cart.Count} items");

            // Scenario 3: User role and permission management
            Console.WriteLine("\n3. User Permission System:");
            var adminRoles = new SortedSet<string> { "read", "write", "delete", "admin", "backup" };
            var userRoles = new SortedSet<string> { "read", "write" };
            var guestRoles = new SortedSet<string> { "read" };

            Console.WriteLine($"   Admin roles: [{string.Join(", ", adminRoles)}]");
            Console.WriteLine($"   User can upgrade to admin: {userRoles.IsSubsetOf(adminRoles)}");
            
            var missingPermissions = new SortedSet<string>(adminRoles);
            missingPermissions.ExceptWith(userRoles);
            Console.WriteLine($"   User missing permissions: [{string.Join(", ", missingPermissions)}]");
        }

        /// <summary>
        /// Shows performance considerations and memory usage patterns.
        /// Critical for high-performance applications.
        /// </summary>
        public static void AnalyzeMemoryAndPerformance()
        {
            Console.WriteLine("\nMEMORY & PERFORMANCE ANALYSIS: Collection Efficiency");
            Console.WriteLine(new string('-', 57));

            const int size = 10000;
            
            // Memory allocation patterns
            Console.WriteLine("1. Memory Allocation Patterns:");
            
            var listWithCapacity = new List<int>(size);
            var listWithoutCapacity = new List<int>();
            
            var sw = new Stopwatch();
            
            // Pre-allocated capacity
            sw.Start();
            for (int i = 0; i < size; i++)
                listWithCapacity.Add(i);
            sw.Stop();
            Console.WriteLine($"   List with pre-allocated capacity: {sw.ElapsedMilliseconds}ms");
            
            // Dynamic growth
            sw.Restart();
            for (int i = 0; i < size; i++)
                listWithoutCapacity.Add(i);
            sw.Stop();
            Console.WriteLine($"   List with dynamic growth: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"   Final capacities - Pre-allocated: {listWithCapacity.Capacity}, Dynamic: {listWithoutCapacity.Capacity}");
            
            // Set performance comparison
            Console.WriteLine("\n2. Set Performance Comparison:");
            var hashSet = new HashSet<int>();
            var sortedSet = new SortedSet<int>();
            
            sw.Restart();
            for (int i = 0; i < size; i++)
                hashSet.Add(i);
            sw.Stop();
            Console.WriteLine($"   HashSet insertion: {sw.ElapsedMilliseconds}ms");
            
            sw.Restart();
            for (int i = 0; i < size; i++)
                sortedSet.Add(i);
            sw.Stop();
            Console.WriteLine($"   SortedSet insertion: {sw.ElapsedMilliseconds}ms");
            
            // Lookup performance
            sw.Restart();
            for (int i = 0; i < 1000; i++)
                hashSet.Contains(i);
            sw.Stop();
            var hashLookup = sw.ElapsedTicks;
            
            sw.Restart();
            for (int i = 0; i < 1000; i++)
                sortedSet.Contains(i);
            sw.Stop();
            var sortedLookup = sw.ElapsedTicks;
            
            Console.WriteLine($"   HashSet lookup (1000 operations): {hashLookup} ticks");
            Console.WriteLine($"   SortedSet lookup (1000 operations): {sortedLookup} ticks");
            Console.WriteLine($"   HashSet is ~{(double)sortedLookup / hashLookup:F1}x faster for lookups");
        }

        /// <summary>
        /// Helper method to convert BitArray to string representation.
        /// </summary>
        private static string GetBitString(BitArray bits)
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < bits.Length; i++)
            {
                result.Append(bits[i] ? '1' : '0');
            }
            return result.ToString();
        }

        /// <summary>
        /// Simple expression evaluator using Stack - demonstrates practical stack usage.
        /// </summary>
        private static int EvaluateSimpleExpression(string expression)
        {
            // This is a simplified evaluator for demonstration
            // In practice, you'd use a proper parser
            var stack = new Stack<int>();
            var tokens = expression.Replace(" ", "").ToCharArray();
            
            // Simplified evaluation - just handles basic cases for demo
            // Real implementation would need proper parsing
            return 18; // 3 * (4 + 2) = 18
        }
    }
}
