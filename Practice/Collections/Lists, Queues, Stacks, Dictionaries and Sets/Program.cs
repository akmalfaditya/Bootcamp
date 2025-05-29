using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Collections_Demo
{
    class Program
    {        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Collections Comprehensive Demo ===");
            Console.WriteLine("This demo covers Lists, Queues, Stacks, Sets, and Dictionaries\n");

            // Demonstrate each collection type with real-world scenarios
            ListCollectionsDemo();
            QueueDemo();
            StackDemo();
            SetCollectionsDemo();
            DictionaryDemo();
            BitArrayDemo();
            SpecializedCollectionsDemo();

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("ADVANCED COLLECTION UTILITIES & PATTERNS");
            Console.WriteLine(new string('=', 60));
            
            // Performance comparisons
            CollectionUtilities.CompareListPerformance();
            
            // Advanced set operations
            CollectionUtilities.AdvancedSetOperations();
            
            // Practical dictionary patterns
            CollectionUtilities.PracticalDictionaryPatterns();
            
            // Collection selection guide
            CollectionUtilities.CollectionSelectionGuide();
            
            // Common mistakes and solutions
            CollectionUtilities.CommonMistakesAndSolutions();

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("COLLECTION TYPES DEMONSTRATION COMPLETE");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates List<T>, ArrayList, and LinkedList<T>
        /// These are the workhorses for most list-based operations
        /// </summary>
        static void ListCollectionsDemo()
        {
            Console.WriteLine("=== 1. List Collections Demo ===");
            
            // List<T> - The most commonly used dynamic array
            Console.WriteLine("\n1.1 List<T> - Generic Dynamic Array");
            Console.WriteLine("Perfect for when you need fast indexed access and don't know the size upfront");
            
            var words = new List<string>();
            words.Add("melon");
            words.Add("avocado");
            words.AddRange(new[] { "banana", "plum" });
            
            Console.WriteLine($"Initial list: [{string.Join(", ", words)}]");
            
            words.Insert(0, "lemon");  // Insert at beginning
            Console.WriteLine($"After inserting 'lemon' at start: [{string.Join(", ", words)}]");
            
            words.Remove("melon");     // Remove by value
            Console.WriteLine($"After removing 'melon': [{string.Join(", ", words)}]");
            
            words.RemoveAt(3);         // Remove by index
            Console.WriteLine($"After removing item at index 3: [{string.Join(", ", words)}]");
            
            // Real trainer tip: RemoveAll with lambda is super powerful
            words.AddRange(new[] { "nectarine", "nut", "orange" });
            Console.WriteLine($"Added some items: [{string.Join(", ", words)}]");
            
            words.RemoveAll(s => s.StartsWith("n"));
            Console.WriteLine($"After removing items starting with 'n': [{string.Join(", ", words)}]");
            
            // Convert to array when you need to pass to methods expecting arrays
            string[] wordsArray = words.ToArray();
            Console.WriteLine($"Converted to array: [{string.Join(", ", wordsArray)}]");
            
            // ArrayList - The old non-generic way (avoid in new code!)
            Console.WriteLine("\n1.2 ArrayList - Legacy Non-Generic Collection");
            Console.WriteLine("Only use this when working with old code or mixed types");
            
            ArrayList al = new ArrayList();
            al.Add("hello");
            al.Add(42);
            al.Add(3.14);
            
            Console.WriteLine("ArrayList contents (notice the mixed types):");
            foreach (var item in al)
            {
                Console.WriteLine($"  {item} (Type: {item.GetType().Name})");
            }
            
            // This is why ArrayList is problematic - casting required everywhere
            string first = (string)al[0];
            int second = (int)al[1];
            Console.WriteLine($"Had to cast: '{first}' and {second}");
              // LinkedList<T> - Great for frequent insertions/deletions
            Console.WriteLine("\n1.3 LinkedList<T> - Doubly Linked List");
            Console.WriteLine("Use when you frequently insert/remove in the middle");
            
            var tune = new LinkedList<string>();
            tune.AddFirst("do");
            tune.AddLast("so");
            
            if (tune.First != null)
            {
                tune.AddAfter(tune.First, "re");
            }
            if (tune.Last != null)
            {
                tune.AddBefore(tune.Last, "fa");
            }
            
            Console.WriteLine($"Musical notes: [{string.Join(" -> ", tune)}]");
            
            // LinkedList shines when you need to manipulate nodes directly
            var reNode = tune.First?.Next;
            if (reNode != null)
            {
                var miNode = tune.AddAfter(reNode, "mi");
                Console.WriteLine($"Added 'mi': [{string.Join(" -> ", tune)}]");
                
                tune.Remove(miNode);
                Console.WriteLine($"Removed 'mi' node: [{string.Join(" -> ", tune)}]");
            }
            
            Console.WriteLine("\nKey takeaway: List<T> for most cases, LinkedList<T> for heavy middle operations\n");
        }

        /// <summary>
        /// Demonstrates Queue<T> - First In, First Out operations
        /// Think of it like a line at the bank or processing tasks
        /// </summary>
        static void QueueDemo()
        {
            Console.WriteLine("=== 2. Queue<T> Demo - FIFO Operations ===");
            Console.WriteLine("Perfect for task processing, breadth-first algorithms, or any 'first-come-first-served' scenario");
            
            var customerQueue = new Queue<string>();
            
            // Customers arriving at the bank
            customerQueue.Enqueue("Alice");
            customerQueue.Enqueue("Bob");
            customerQueue.Enqueue("Charlie");
            customerQueue.Enqueue("Diana");
            
            Console.WriteLine($"Customers in queue: {customerQueue.Count}");
            Console.WriteLine($"Next customer to be served: {customerQueue.Peek()}");
            
            // Serve customers in order
            Console.WriteLine("\nServing customers:");
            while (customerQueue.Count > 0)
            {
                string customer = customerQueue.Dequeue();
                Console.WriteLine($"  Now serving: {customer} (Remaining: {customerQueue.Count})");
            }
            
            // Real-world example: Processing work items
            Console.WriteLine("\nReal example - Processing work items:");
            var workQueue = new Queue<int>();
            
            // Add some work items
            for (int i = 1; i <= 5; i++)
            {
                workQueue.Enqueue(i * 100);
                Console.WriteLine($"  Added work item: {i * 100}");
            }
            
            // Process them in order
            Console.WriteLine("Processing work items in FIFO order:");
            while (workQueue.Count > 0)
            {
                int workItem = workQueue.Dequeue();
                Console.WriteLine($"  Processing: {workItem}");
                // Simulate some work
                System.Threading.Thread.Sleep(100);
            }
            
            Console.WriteLine("Queue is particularly useful for breadth-first search and task scheduling\n");
        }

        /// <summary>
        /// Demonstrates Stack<T> - Last In, First Out operations
        /// Think undo operations, function call stack, or reversing data
        /// </summary>
        static void StackDemo()
        {
            Console.WriteLine("=== 3. Stack<T> Demo - LIFO Operations ===");
            Console.WriteLine("Essential for undo operations, parsing expressions, and recursive algorithms");
            
            var historyStack = new Stack<string>();
            
            // Simulate user actions that can be undone
            Console.WriteLine("User performs actions (can be undone):");
            historyStack.Push("Created document");
            Console.WriteLine("  Action: Created document");
            
            historyStack.Push("Added title");
            Console.WriteLine("  Action: Added title");
            
            historyStack.Push("Added paragraph");
            Console.WriteLine("  Action: Added paragraph");
            
            historyStack.Push("Changed font");
            Console.WriteLine("  Action: Changed font");
            
            Console.WriteLine($"\nActions in history: {historyStack.Count}");
            Console.WriteLine($"Last action (can undo): {historyStack.Peek()}");
            
            // Undo operations
            Console.WriteLine("\nUser clicks Undo:");
            while (historyStack.Count > 0)
            {
                string lastAction = historyStack.Pop();
                Console.WriteLine($"  Undoing: {lastAction} (Remaining: {historyStack.Count})");
                
                // In real app, you'd only undo one at a time
                if (historyStack.Count == 2) break;
            }
            
            // Another practical example: Expression evaluation
            Console.WriteLine("\nExpression evaluation example:");
            var operatorStack = new Stack<char>();
            string expression = "3 + 4 * 2";
            
            Console.WriteLine($"Parsing expression: {expression}");
            foreach (char c in expression)
            {
                if (c == '+' || c == '*' || c == '-' || c == '/')
                {
                    operatorStack.Push(c);
                    Console.WriteLine($"  Pushed operator: {c}");
                }
            }
            
            Console.WriteLine("Operators in reverse order:");
            while (operatorStack.Count > 0)
            {
                Console.WriteLine($"  {operatorStack.Pop()}");
            }
            
            Console.WriteLine("Stacks are perfect when you need to reverse order or track nested operations\n");
        }

        /// <summary>
        /// Demonstrates HashSet<T> and SortedSet<T>
        /// These are your go-to for unique collections and set operations
        /// </summary>
        static void SetCollectionsDemo()
        {
            Console.WriteLine("=== 4. Set Collections Demo ===");
            
            // HashSet<T> - Fast membership testing, no duplicates
            Console.WriteLine("4.1 HashSet<T> - Unordered Unique Elements");
            Console.WriteLine("Perfect for removing duplicates and fast 'contains' checks");
            
            var letters = new HashSet<char>("the quick brown fox");
            Console.WriteLine($"Unique letters in 'the quick brown fox': {string.Join("", letters)}");
            
            Console.WriteLine($"Contains 't': {letters.Contains('t')}");
            Console.WriteLine($"Contains 'j': {letters.Contains('j')}");
            
            // Set operations are incredibly powerful
            var vowels = new HashSet<char>("aeiou");
            Console.WriteLine($"Vowels: {string.Join("", vowels)}");
            
            // Find vowels that appear in our text
            letters.IntersectWith(vowels);
            Console.WriteLine($"Vowels found in text: {string.Join("", letters)}");
            
            // Real example: Finding common interests
            Console.WriteLine("\nReal example - Finding common interests:");
            var aliceInterests = new HashSet<string> { "reading", "hiking", "cooking", "programming" };
            var bobInterests = new HashSet<string> { "gaming", "cooking", "music", "programming" };
            
            Console.WriteLine($"Alice likes: {string.Join(", ", aliceInterests)}");
            Console.WriteLine($"Bob likes: {string.Join(", ", bobInterests)}");
            
            // Find common interests
            var commonInterests = new HashSet<string>(aliceInterests);
            commonInterests.IntersectWith(bobInterests);
            Console.WriteLine($"Common interests: {string.Join(", ", commonInterests)}");
            
            // SortedSet<T> - Maintains order while ensuring uniqueness
            Console.WriteLine("\n4.2 SortedSet<T> - Ordered Unique Elements");
            Console.WriteLine("Use when you need unique elements in sorted order");
            
            var scores = new SortedSet<int> { 85, 92, 78, 92, 85, 88, 95 };
            Console.WriteLine($"Unique scores in order: [{string.Join(", ", scores)}]");
            
            scores.Add(90);
            Console.WriteLine($"After adding 90: [{string.Join(", ", scores)}]");
            
            // SortedSet operations
            var topScores = new SortedSet<int> { 90, 95, 100 };
            scores.UnionWith(topScores);
            Console.WriteLine($"After union with top scores: [{string.Join(", ", scores)}]");
            
            Console.WriteLine("Sets are essential for removing duplicates and mathematical set operations\n");
        }

        /// <summary>
        /// Comprehensive dictionary demonstration
        /// Shows all the different dictionary types and when to use each
        /// </summary>
        static void DictionaryDemo()
        {
            Console.WriteLine("=== 5. Dictionary Collections Demo ===");
            
            // Dictionary<TKey, TValue> - The workhorse
            Console.WriteLine("5.1 Dictionary<TKey, TValue> - Hash Table Implementation");
            Console.WriteLine("Your go-to choice for fast key-value lookups");
            
            var studentGrades = new Dictionary<string, int>();
            studentGrades.Add("Alice", 95);
            studentGrades["Bob"] = 87;     // Alternative way to add
            studentGrades["Charlie"] = 92;
            studentGrades["Diana"] = 88;
            
            // Update existing value
            studentGrades["Bob"] = 90;
            
            Console.WriteLine("Student grades:");
            foreach (var grade in studentGrades)
            {
                Console.WriteLine($"  {grade.Key}: {grade.Value}");
            }
            
            // Safe value retrieval
            Console.WriteLine($"\nCharlie's grade: {studentGrades["Charlie"]}");
            Console.WriteLine($"Dictionary contains 'Alice': {studentGrades.ContainsKey("Alice")}");
            Console.WriteLine($"Dictionary contains grade 88: {studentGrades.ContainsValue(88)}");
            
            // Safe retrieval with TryGetValue
            if (studentGrades.TryGetValue("Eve", out int eveGrade))
            {
                Console.WriteLine($"Eve's grade: {eveGrade}");
            }
            else
            {
                Console.WriteLine("Eve not found in gradebook");
            }
            
            // Hashtable - The old non-generic way
            Console.WriteLine("\n5.2 Hashtable - Legacy Non-Generic Dictionary");
            Console.WriteLine("Avoid in new code - shown for legacy understanding");
            
            Hashtable phoneBook = new Hashtable();
            phoneBook.Add("John", "555-1234");
            phoneBook["Jane"] = "555-5678";
            
            Console.WriteLine($"John's number: {phoneBook["John"]}");
            // Notice the casting required - this is why generics are better
            
            // SortedDictionary<TKey, TValue> - Maintains key order
            Console.WriteLine("\n5.3 SortedDictionary<TKey, TValue> - Red-Black Tree");
            Console.WriteLine("Use when you need keys in sorted order");
            
            var cityPopulation = new SortedDictionary<string, int>();
            cityPopulation.Add("New York", 8_400_000);
            cityPopulation.Add("Los Angeles", 3_900_000);
            cityPopulation.Add("Chicago", 2_700_000);
            cityPopulation.Add("Houston", 2_300_000);
            
            Console.WriteLine("Cities by alphabetical order:");
            foreach (var city in cityPopulation)
            {
                Console.WriteLine($"  {city.Key}: {city.Value:N0}");
            }
            
            // SortedList<TKey, TValue> - Array-based sorted dictionary
            Console.WriteLine("\n5.4 SortedList<TKey, TValue> - Array-Based Sorted");
            Console.WriteLine("Better memory usage than SortedDictionary, allows index access");
            
            var monthlyTemps = new SortedList<string, double>();
            monthlyTemps.Add("January", 32.5);
            monthlyTemps.Add("March", 52.1);
            monthlyTemps.Add("February", 38.2);
            
            Console.WriteLine("Monthly temperatures (sorted by month):");
            for (int i = 0; i < monthlyTemps.Count; i++)
            {
                Console.WriteLine($"  [{i}] {monthlyTemps.Keys[i]}: {monthlyTemps.Values[i]:F1}°F");
            }
            
            // OrderedDictionary - Maintains insertion order
            Console.WriteLine("\n5.5 OrderedDictionary - Preserves Insertion Order");
            Console.WriteLine("Use when you need both key lookup and insertion order");
            
            var processSteps = new OrderedDictionary();
            processSteps.Add("Step1", "Initialize system");
            processSteps.Add("Step2", "Load configuration");
            processSteps.Add("Step3", "Connect to database");
            processSteps.Add("Step4", "Start services");
            
            Console.WriteLine("Process steps in order:");
            foreach (DictionaryEntry step in processSteps)
            {
                Console.WriteLine($"  {step.Key}: {step.Value}");
            }
            
            // Can also access by index
            Console.WriteLine($"First step: {processSteps[0]}");
            
            Console.WriteLine("Choose dictionary type based on your needs: speed, order, or both\n");
        }

        /// <summary>
        /// BitArray demonstration - efficient boolean storage
        /// Great for flags, bitmap operations, and memory-efficient boolean arrays
        /// </summary>
        static void BitArrayDemo()
        {
            Console.WriteLine("=== 6. BitArray Demo - Memory-Efficient Boolean Storage ===");
            Console.WriteLine("Perfect for large boolean arrays, flags, and bitmap operations");
            
            // Create a BitArray representing user permissions
            var permissions = new BitArray(8);  // 8 different permissions
            
            // Set some permissions (using realistic permission names in comments)
            permissions[0] = true;  // Read
            permissions[1] = false; // Write
            permissions[2] = true;  // Execute
            permissions[3] = false; // Delete
            permissions[4] = true;  // Admin
            permissions[5] = false; // Backup
            permissions[6] = false; // Restore
            permissions[7] = true;  // Audit
            
            Console.WriteLine("User permissions (1=granted, 0=denied):");
            string[] permissionNames = { "Read", "Write", "Execute", "Delete", "Admin", "Backup", "Restore", "Audit" };
            
            for (int i = 0; i < permissions.Count; i++)
            {
                Console.WriteLine($"  {permissionNames[i]}: {(permissions[i] ? "1" : "0")}");
            }
            
            // BitArray operations are very efficient
            var adminPermissions = new BitArray(8, true);  // Admin has all permissions
            var guestPermissions = new BitArray(8, false); // Guest has no permissions
            guestPermissions[0] = true; // Except read
            
            Console.WriteLine($"\nAdmin permissions: {GetBitString(adminPermissions)}");
            Console.WriteLine($"Guest permissions: {GetBitString(guestPermissions)}");
            Console.WriteLine($"User permissions:  {GetBitString(permissions)}");
            
            // Bitwise operations
            var combinedPermissions = new BitArray(permissions);
            combinedPermissions.Or(guestPermissions);  // Union of permissions
            Console.WriteLine($"Combined with guest: {GetBitString(combinedPermissions)}");
            
            // Memory efficiency demonstration
            Console.WriteLine($"\nMemory efficiency:");
            Console.WriteLine($"BitArray(1000): ~125 bytes");
            Console.WriteLine($"bool[1000]: ~1000 bytes");
            Console.WriteLine($"BitArray is 8x more memory efficient!");
            
            Console.WriteLine("BitArray is perfect for large boolean datasets and bitwise operations\n");
        }

        /// <summary>
        /// Demonstrates specialized collections like ListDictionary and HybridDictionary
        /// These are legacy collections but still useful in specific scenarios
        /// </summary>
        static void SpecializedCollectionsDemo()
        {
            Console.WriteLine("=== 7. Specialized Collections Demo ===");
            Console.WriteLine("Legacy collections with specific use cases - understanding for completeness");
            
            // ListDictionary - Simple linked list implementation
            Console.WriteLine("7.1 ListDictionary - Linked List Based Dictionary");
            Console.WriteLine("Efficient for small collections (< 10 items)");
            
            var configSettings = new ListDictionary();
            configSettings.Add("theme", "dark");
            configSettings.Add("language", "en-US");
            configSettings.Add("timeout", "30");
            configSettings.Add("debug", "false");
            
            Console.WriteLine("Configuration settings:");
            foreach (DictionaryEntry setting in configSettings)
            {
                Console.WriteLine($"  {setting.Key}: {setting.Value}");
            }
            
            // HybridDictionary - Automatically switches from ListDictionary to Hashtable
            Console.WriteLine("\n7.2 HybridDictionary - Adaptive Collection");
            Console.WriteLine("Starts as ListDictionary, converts to Hashtable when it grows");
            
            var userCache = new HybridDictionary();
            
            // Start small (uses ListDictionary internally)
            userCache.Add("user1", "Alice");
            userCache.Add("user2", "Bob");
            userCache.Add("user3", "Charlie");
            
            Console.WriteLine("Small cache (ListDictionary mode):");
            foreach (DictionaryEntry user in userCache)
            {
                Console.WriteLine($"  {user.Key}: {user.Value}");
            }
            
            // Add more items (will convert to Hashtable automatically)
            for (int i = 4; i <= 15; i++)
            {
                userCache.Add($"user{i}", $"User{i}");
            }
            
            Console.WriteLine($"\nLarge cache (converted to Hashtable): {userCache.Count} items");
            Console.WriteLine("HybridDictionary automatically optimized for larger size");
            
            Console.WriteLine("\nModern recommendation: Use Dictionary<TKey, TValue> instead");
            Console.WriteLine("These specialized collections are mainly for legacy support\n");
        }

        /// <summary>
        /// Helper method to convert BitArray to readable string
        /// </summary>
        static string GetBitString(BitArray bits)
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < bits.Count; i++)
            {
                result.Append(bits[i] ? "1" : "0");
            }
            return result.ToString();
        }
    }
}
