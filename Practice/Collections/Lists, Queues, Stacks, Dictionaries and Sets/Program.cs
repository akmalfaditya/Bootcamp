using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lists__Queues__Stacks__Dictionaries_and_Sets
{
    /// <summary>
    /// Comprehensive demonstration of .NET collections focusing on practical usage patterns.
    /// This covers List<T>, ArrayList, LinkedList<T>, Queue<T>, Stack<T>, HashSet<T>, 
    /// SortedSet<T>, and BitArray with real-world examples and performance insights.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Lists, Queues, Stacks, Dictionaries and Sets: Essential Concrete Collections ===");
            Console.WriteLine("Demonstrating the most commonly used collection types in .NET development\n");

            // Core collection demonstrations
            DemonstrateDynamicArrays();
            DemonstrateLinkedLists();
            DemonstrateQueues();
            DemonstrateStacks();
            DemonstrateSetCollections();
            DemonstrateBitArrays();

            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("COLLECTION SELECTION GUIDE & PERFORMANCE INSIGHTS");
            Console.WriteLine(new string('=', 70));
            
            CollectionUtilities.ShowPerformanceComparisons();
            CollectionUtilities.ShowConstructorOptions();
            CollectionUtilities.DemonstrateConversionPatterns();
            CollectionUtilities.ShowRealWorldScenarios();
            CollectionUtilities.AnalyzeMemoryAndPerformance();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates List<T> and ArrayList - the dynamically sized array collections.
        /// List<T> is the generic powerhouse for most scenarios, while ArrayList is the legacy non-generic version.
        /// Shows internal mechanism, performance characteristics, and when to use each.
        /// </summary>
        static void DemonstrateDynamicArrays()
        {
            Console.WriteLine("=== 1. List<T> and ArrayList: Dynamically Sized Arrays ===");
            Console.WriteLine("The workhorses of .NET collections - understanding when and how to use them effectively\n");

            // List<T> - The modern generic choice
            Console.WriteLine("1.1 List<T> - Generic Dynamic Array (Your go-to choice)");
            Console.WriteLine("    • Implements IList<T>, IReadOnlyList<T>, and IList");
            Console.WriteLine("    • Maintains internal array that grows automatically");
            Console.WriteLine("    • Type-safe, no boxing for value types\n");

            // Different constructor options
            var emptyList = new List<string>();
            var initializedList = new List<string> { "apple", "banana", "cherry" };
            var fromCollection = new List<string>(new[] { "dog", "elephant", "fox" });
            var withCapacity = new List<string>(10); // Reduces reallocations if you know approximate size

            Console.WriteLine("Constructor examples:");
            Console.WriteLine($"  Empty list: Count = {emptyList.Count}");
            Console.WriteLine($"  Initialized: [{string.Join(", ", initializedList)}]");
            Console.WriteLine($"  From array: [{string.Join(", ", fromCollection)}]");
            Console.WriteLine($"  With capacity: Count = {withCapacity.Count}, Capacity = {withCapacity.Capacity}\n");

            // Core operations demonstration
            var fruits = new List<string>();
            
            // Adding elements - Add() is generally efficient (amortized O(1))
            fruits.Add("apple");
            fruits.Add("banana");
            fruits.AddRange(new[] { "cherry", "date", "elderberry" });
            Console.WriteLine($"After adding: [{string.Join(", ", fruits)}]");

            // Inserting elements - can be slow for large lists (O(n) due to shifting)
            fruits.Insert(2, "blueberry");
            Console.WriteLine($"After inserting 'blueberry' at index 2: [{string.Join(", ", fruits)}]");

            // Removing elements - also involves shifting for mid-list operations
            fruits.Remove("banana"); // Remove by value
            Console.WriteLine($"After removing 'banana': [{string.Join(", ", fruits)}]");

            fruits.RemoveAt(0); // Remove by index
            Console.WriteLine($"After removing item at index 0: [{string.Join(", ", fruits)}]");

            // RemoveAll with predicate - very powerful for conditional removal
            fruits.AddRange(new[] { "nectarine", "orange", "nutmeg" });
            Console.WriteLine($"Before conditional remove: [{string.Join(", ", fruits)}]");
            
            int removedCount = fruits.RemoveAll(f => f.StartsWith("n"));
            Console.WriteLine($"Removed {removedCount} items starting with 'n': [{string.Join(", ", fruits)}]");

            // Key List<T> members demonstration
            Console.WriteLine("\nKey List<T> operations:");
            
            // Indexing and access
            Console.WriteLine($"  First item: {fruits[0]}");
            Console.WriteLine($"  Last item: {fruits[fruits.Count - 1]}");
            
            // Search operations
            int cherryIndex = fruits.IndexOf("cherry");
            Console.WriteLine($"  Index of 'cherry': {cherryIndex}");
            Console.WriteLine($"  Contains 'apple': {fruits.Contains("apple")}");

            // Conversion operations
            string[] fruitsArray = fruits.ToArray();
            Console.WriteLine($"  Converted to array: [{string.Join(", ", fruitsArray)}]");

            // Capacity management
            Console.WriteLine($"  Current capacity: {fruits.Capacity}");
            fruits.TrimExcess(); // Reduces capacity to match count
            Console.WriteLine($"  After TrimExcess: {fruits.Capacity}");

            // ArrayList - The legacy non-generic version
            Console.WriteLine("\n1.2 ArrayList - Non-Generic Legacy Collection");
            Console.WriteLine("    • Stores everything as 'object' - requires casting");
            Console.WriteLine("    • Boxing/unboxing overhead for value types");
            Console.WriteLine("    • Mainly kept for backward compatibility\n");

            ArrayList al = new ArrayList();
            al.Add("hello");        // string - no boxing
            al.Add(42);             // int - boxing occurs here
            al.Add(3.14);           // double - boxing occurs here
            al.AddRange(new[] { 1, 5, 9 });

            Console.WriteLine("ArrayList with mixed types:");
            foreach (var item in al)
            {
                Console.WriteLine($"  {item} (Type: {item.GetType().Name})");
            }

            // The casting problem - runtime type checking required
            Console.WriteLine("\nCasting requirements (potential runtime errors):");
            string? first = (string?)al[0];  // Safe cast
            int second = (int)al[1]!;        // Must be certain of type (! suppresses nullable warning)
            Console.WriteLine($"  Retrieved: '{first}' and {second}");

            // Converting ArrayList to List<T> using LINQ
            // Safe conversion for specific range
            List<int> safeIntList = al.OfType<int>().ToList(); // Only gets int items
            Console.WriteLine($"  Integers from ArrayList: [{string.Join(", ", safeIntList)}]");

            Console.WriteLine("\nKey takeaway: Use List<T> for new code. ArrayList only when:");
            Console.WriteLine("  • Working with legacy code");
            Console.WriteLine("  • Need to store truly mixed types without common base");
            Console.WriteLine("  • Reflection scenarios (rare)\n");
        }

        /// <summary>
        /// Demonstrates LinkedList<T> - doubly linked list implementation.
        /// Shows when it excels (frequent insertions/deletions) and when it struggles (random access).
        /// </summary>
        static void DemonstrateLinkedLists()
        {
            Console.WriteLine("=== 2. LinkedList<T>: Doubly Linked Lists ===");
            Console.WriteLine("Excellent for frequent insertions/deletions, poor for random access\n");

            Console.WriteLine("Key characteristics:");
            Console.WriteLine("  • Each element wrapped in LinkedListNode<T>");
            Console.WriteLine("  • O(1) insertion/removal anywhere (if you have the node)");
            Console.WriteLine("  • O(n) searching - must traverse from beginning or end");
            Console.WriteLine("  • No indexed access (no list[index])");
            Console.WriteLine("  • Implements ICollection<T> but NOT IList<T>\n");

            var tune = new LinkedList<string>();
            
            // Building a musical scale
            tune.AddFirst("do");
            Console.WriteLine($"Added 'do': [{string.Join(" -> ", tune)}]");
            
            tune.AddLast("so");
            Console.WriteLine($"Added 'so' at end: [{string.Join(" -> ", tune)}]");
            
            // Adding relative to existing nodes (this is where LinkedList shines)
            if (tune.First != null)
            {
                tune.AddAfter(tune.First, "re");
                Console.WriteLine($"Added 're' after 'do': [{string.Join(" -> ", tune)}]");
            }

            if (tune.Last != null)
            {
                tune.AddBefore(tune.Last, "fa");
                Console.WriteLine($"Added 'fa' before 'so': [{string.Join(" -> ", tune)}]");
            }

            // Working with nodes directly
            var reNode = tune.First?.Next; // Get the "re" node
            if (reNode != null)
            {
                var miNode = tune.AddAfter(reNode, "mi");
                Console.WriteLine($"Added 'mi' after 're': [{string.Join(" -> ", tune)}]");
                
                // Remove specific node - O(1) operation!
                tune.Remove(miNode);
                Console.WriteLine($"Removed 'mi' node: [{string.Join(" -> ", tune)}]");
            }

            // Searching operations (O(n) - must traverse)
            var foundNode = tune.Find("re");
            Console.WriteLine($"Found 're': {foundNode != null}");
            
            var lastFaNode = tune.FindLast("fa");
            Console.WriteLine($"Found last 'fa': {lastFaNode != null}");

            // Navigation properties
            Console.WriteLine($"\nNavigation examples:");
            if (tune.First != null)
            {
                Console.WriteLine($"  First: {tune.First.Value}");
                Console.WriteLine($"  First->Next: {tune.First.Next?.Value ?? "null"}");
            }
            
            if (tune.Last != null)
            {
                Console.WriteLine($"  Last: {tune.Last.Value}");
                Console.WriteLine($"  Last->Previous: {tune.Last.Previous?.Value ?? "null"}");
            }

            Console.WriteLine($"\nTotal nodes: {tune.Count}");

            Console.WriteLine("\nWhen to use LinkedList<T>:");
            Console.WriteLine("  ✓ Frequent insertions/deletions in middle");
            Console.WriteLine("  ✓ Building queues, stacks, or custom data structures");
            Console.WriteLine("  ✓ When you maintain references to nodes");
            Console.WriteLine("  ✗ Random access by index needed");
            Console.WriteLine("  ✗ Frequent searching operations");
            Console.WriteLine("  ✗ Memory-conscious applications (extra node overhead)\n");
        }

        /// <summary>
        /// Demonstrates Queue<T> and Queue - First-In, First-Out (FIFO) collections.
        /// Shows practical usage patterns like task processing and breadth-first algorithms.
        /// </summary>
        static void DemonstrateQueues()
        {
            Console.WriteLine("=== 3. Queue<T> and Queue: First-In, First-Out (FIFO) ===");
            Console.WriteLine("Perfect for task processing, breadth-first search, and any 'first-come-first-served' scenario\n");

            Console.WriteLine("Key operations:");
            Console.WriteLine("  • Enqueue(item) - Add to tail (rear)");
            Console.WriteLine("  • Dequeue() - Remove from head (front)");
            Console.WriteLine("  • Peek() - Look at head without removing");
            Console.WriteLine("  • Generally O(1) operations, except during internal array resize\n");

            // Basic queue operations
            var customerQueue = new Queue<string>();
            
            Console.WriteLine("Customers arriving at service counter:");
            customerQueue.Enqueue("Alice");
            Console.WriteLine($"  Alice arrives. Queue: [{string.Join(", ", customerQueue)}]");
            
            customerQueue.Enqueue("Bob");
            Console.WriteLine($"  Bob arrives. Queue: [{string.Join(", ", customerQueue)}]");
            
            customerQueue.Enqueue("Charlie");
            Console.WriteLine($"  Charlie arrives. Queue: [{string.Join(", ", customerQueue)}]");

            Console.WriteLine($"\nNext customer to serve: {customerQueue.Peek()}");
            Console.WriteLine($"Queue size: {customerQueue.Count}");

            // Serving customers in FIFO order
            Console.WriteLine("\nServing customers:");
            while (customerQueue.Count > 0)
            {
                string customer = customerQueue.Dequeue();
                Console.WriteLine($"  Now serving: {customer} (Remaining: {customerQueue.Count})");
            }

            // Real-world example: Task processing system
            Console.WriteLine("\nReal-world example: Background task processor");
            var taskQueue = new Queue<WorkItem>();
            
            // Add work items
            taskQueue.Enqueue(new WorkItem { Id = 1, Task = "Process payment", Priority = "High" });
            taskQueue.Enqueue(new WorkItem { Id = 2, Task = "Send email", Priority = "Low" });
            taskQueue.Enqueue(new WorkItem { Id = 3, Task = "Update database", Priority = "Medium" });
            taskQueue.Enqueue(new WorkItem { Id = 4, Task = "Generate report", Priority = "Low" });

            Console.WriteLine($"Tasks queued: {taskQueue.Count}");

            // Process tasks in FIFO order
            while (taskQueue.Count > 0)
            {
                var workItem = taskQueue.Dequeue();
                Console.WriteLine($"  Processing: [{workItem.Priority}] {workItem.Task} (ID: {workItem.Id})");
                
                // Simulate processing time
                System.Threading.Thread.Sleep(50);
            }

            // Converting to/from arrays
            var numbers = new Queue<int>();
            numbers.Enqueue(10);
            numbers.Enqueue(20);
            numbers.Enqueue(30);

            int[] numberArray = numbers.ToArray();
            Console.WriteLine($"\nQueue as array: [{string.Join(", ", numberArray)}]");
            Console.WriteLine("Note: ToArray() preserves FIFO order (first enqueued = first in array)");

            Console.WriteLine("\nCommon Queue<T> usage patterns:");
            Console.WriteLine("  • Web request processing");
            Console.WriteLine("  • Print job queuing");
            Console.WriteLine("  • Breadth-first search algorithms");
            Console.WriteLine("  • Producer-consumer scenarios");
            Console.WriteLine("  • Undo/redo systems (with multiple queues)\n");
        }

        /// <summary>
        /// Demonstrates Stack<T> and Stack - Last-In, First-Out (LIFO) collections.
        /// Shows usage in undo operations, expression parsing, and recursive algorithms.
        /// </summary>
        static void DemonstrateStacks()
        {
            Console.WriteLine("=== 4. Stack<T> and Stack: Last-In, First-Out (LIFO) ===");
            Console.WriteLine("Essential for undo operations, parsing, recursion simulation, and reversing data\n");

            Console.WriteLine("Key operations:");
            Console.WriteLine("  • Push(item) - Add to top");
            Console.WriteLine("  • Pop() - Remove from top");
            Console.WriteLine("  • Peek() - Look at top without removing");
            Console.WriteLine("  • Generally O(1) operations\n");

            // Basic stack operations
            var plateStack = new Stack<string>();
            
            Console.WriteLine("Stacking plates (imagine a cafeteria):");
            plateStack.Push("Blue plate");
            Console.WriteLine($"  Added blue plate. Stack: [{string.Join(" | ", plateStack.Reverse())}] <- top");
            
            plateStack.Push("Red plate");
            Console.WriteLine($"  Added red plate. Stack: [{string.Join(" | ", plateStack.Reverse())}] <- top");
            
            plateStack.Push("Green plate");
            Console.WriteLine($"  Added green plate. Stack: [{string.Join(" | ", plateStack.Reverse())}] <- top");

            Console.WriteLine($"\nTop plate: {plateStack.Peek()}");
            Console.WriteLine($"Stack height: {plateStack.Count}");

            // Taking plates (LIFO order)
            Console.WriteLine("\nTaking plates:");
            while (plateStack.Count > 0)
            {
                string plate = plateStack.Pop();
                Console.WriteLine($"  Took: {plate} (Remaining: {plateStack.Count})");
            }

            // Real-world example: Undo system
            Console.WriteLine("\nReal-world example: Document editor undo system");
            var undoStack = new Stack<EditorAction>();
            
            // User performs actions
            undoStack.Push(new EditorAction { Type = "Insert", Description = "Added 'Hello'" });
            undoStack.Push(new EditorAction { Type = "Insert", Description = "Added ' World'" });
            undoStack.Push(new EditorAction { Type = "Format", Description = "Made text bold" });
            undoStack.Push(new EditorAction { Type = "Insert", Description = "Added '!'" });

            Console.WriteLine($"Actions in history: {undoStack.Count}");
            Console.WriteLine($"Last action: {undoStack.Peek().Description}");

            // User hits Ctrl+Z multiple times
            Console.WriteLine("\nUndo operations (Ctrl+Z):");
            while (undoStack.Count > 0)
            {
                var action = undoStack.Pop();
                Console.WriteLine($"  Undoing: {action.Description} (Remaining: {undoStack.Count})");
                
                // In real editor, you'd only undo one at a time
                if (undoStack.Count == 1) break;
            }

            // Another example: Expression evaluation (simplified)
            Console.WriteLine("\nExpression parsing example:");
            var operatorStack = new Stack<char>();
            string expression = "3 + 4 * 2 - 1";
            
            Console.WriteLine($"Parsing: {expression}");
            foreach (char c in expression)
            {
                if ("+-*/".Contains(c))
                {
                    operatorStack.Push(c);
                    Console.WriteLine($"  Pushed operator: {c}");
                }
            }

            Console.WriteLine("Operators in reverse order (LIFO):");
            while (operatorStack.Count > 0)
            {
                Console.WriteLine($"  {operatorStack.Pop()}");
            }

            // Practical stack usage for reversing
            var originalWords = new[] { "first", "second", "third", "fourth" };
            var reverseStack = new Stack<string>(originalWords);
            
            Console.WriteLine($"\nOriginal order: [{string.Join(", ", originalWords)}]");
            Console.WriteLine($"Reversed order: [{string.Join(", ", reverseStack)}]");

            Console.WriteLine("\nCommon Stack<T> usage patterns:");
            Console.WriteLine("  • Undo/redo functionality");
            Console.WriteLine("  • Expression/syntax parsing");
            Console.WriteLine("  • Depth-first search algorithms");
            Console.WriteLine("  • Function call simulation");
            Console.WriteLine("  • Reversing sequences");
            Console.WriteLine("  • Backtracking algorithms\n");
        }

        /// <summary>
        /// Demonstrates HashSet<T> and SortedSet<T> - collections that ensure uniqueness.
        /// Shows set operations and when to choose each type.
        /// </summary>
        static void DemonstrateSetCollections()
        {
            Console.WriteLine("=== 5. HashSet<T> and SortedSet<T>: Unique Collections ===");
            Console.WriteLine("Perfect for ensuring uniqueness and performing mathematical set operations\n");

            // HashSet<T> demonstration
            Console.WriteLine("5.1 HashSet<T> - Unordered Unique Elements");
            Console.WriteLine("    • Hash table implementation (O(1) average operations)");
            Console.WriteLine("    • No guaranteed order");
            Console.WriteLine("    • Fastest for Contains(), Add(), Remove()\n");

            // Initialize from string (IEnumerable<char>)
            var letters = new HashSet<char>("the quick brown fox");
            Console.WriteLine($"Unique letters in 'the quick brown fox': {string.Join("", letters.OrderBy(c => c))}");
            Console.WriteLine($"Original order in HashSet: {string.Join("", letters)}");

            // Fast membership testing
            Console.WriteLine($"\nMembership testing (very fast):");
            Console.WriteLine($"  Contains 't': {letters.Contains('t')}");
            Console.WriteLine($"  Contains 'j': {letters.Contains('j')}");

            // Adding duplicates (silently ignored)
            letters.Add('x');
            letters.Add('t'); // Duplicate - ignored
            Console.WriteLine($"After adding 'x' and 't' again: {string.Join("", letters.OrderBy(c => c))}");

            // Set operations - the real power of HashSet<T>
            Console.WriteLine("\nSet operations demonstration:");
            var vowels = new HashSet<char>("aeiou");
            var consonants = new HashSet<char>("bcdfghjklmnpqrstvwxyz");
            
            Console.WriteLine($"Vowels: {string.Join("", vowels)}");
            Console.WriteLine($"Consonants: {string.Join("", consonants.Take(10))}..."); // Show first 10

            // Find vowels in our text
            var textVowels = new HashSet<char>(letters);
            textVowels.IntersectWith(vowels);
            Console.WriteLine($"Vowels found in text: {string.Join("", textVowels.OrderBy(c => c))}");

            // Real-world example: User permissions
            Console.WriteLine("\nReal-world example: User permissions comparison");
            var adminPermissions = new HashSet<string> 
            { 
                "read", "write", "delete", "execute", "admin", "backup" 
            };
            var userPermissions = new HashSet<string> 
            { 
                "read", "write", "execute" 
            };

            Console.WriteLine($"Admin permissions: {string.Join(", ", adminPermissions)}");
            Console.WriteLine($"User permissions: {string.Join(", ", userPermissions)}");

            // Various set operations
            var extraPermissions = new HashSet<string>(adminPermissions);
            extraPermissions.ExceptWith(userPermissions);
            Console.WriteLine($"Extra admin permissions: {string.Join(", ", extraPermissions)}");

            Console.WriteLine($"User is subset of admin: {userPermissions.IsSubsetOf(adminPermissions)}");
            Console.WriteLine($"Sets overlap: {adminPermissions.Overlaps(userPermissions)}");

            // Demonstrate all ISet<T> operations
            Console.WriteLine("\nAll set operations available:");
            
            var set1 = new HashSet<string> { "apple", "banana", "cherry" };
            var set2 = new HashSet<string> { "banana", "date", "elderberry" };
            
            Console.WriteLine($"Set1: {string.Join(", ", set1)}");
            Console.WriteLine($"Set2: {string.Join(", ", set2)}");

            // UnionWith - adds all elements from other set
            var unionSet = new HashSet<string>(set1);
            unionSet.UnionWith(set2);
            Console.WriteLine($"Union (Set1 ∪ Set2): {string.Join(", ", unionSet)}");

            // IntersectWith - keeps only common elements
            var intersectSet = new HashSet<string>(set1);
            intersectSet.IntersectWith(set2);
            Console.WriteLine($"Intersection (Set1 ∩ Set2): {string.Join(", ", intersectSet)}");

            // ExceptWith - removes elements present in other set
            var exceptSet = new HashSet<string>(set1);
            exceptSet.ExceptWith(set2);
            Console.WriteLine($"Except (Set1 - Set2): {string.Join(", ", exceptSet)}");

            // SymmetricExceptWith - elements unique to either set
            var symmetricSet = new HashSet<string>(set1);
            symmetricSet.SymmetricExceptWith(set2);
            Console.WriteLine($"Symmetric difference (Set1 ⊕ Set2): {string.Join(", ", symmetricSet)}");

            // SortedSet<T> demonstration
            Console.WriteLine("\n5.2 SortedSet<T> - Ordered Unique Elements");
            Console.WriteLine("    • Red-black tree implementation (O(log n) operations)");
            Console.WriteLine("    • Maintains sorted order");
            Console.WriteLine("    • Additional range operations\n");

            var scores = new SortedSet<int> { 85, 92, 78, 92, 85, 88, 95 }; // Duplicates ignored
            Console.WriteLine($"Unique scores in sorted order: [{string.Join(", ", scores)}]");

            // SortedSet specific operations
            Console.WriteLine($"Minimum score: {scores.Min}");
            Console.WriteLine($"Maximum score: {scores.Max}");

            // Range operations
            var highScores = scores.GetViewBetween(90, 100);
            Console.WriteLine($"High scores (90-100): [{string.Join(", ", highScores)}]");

            // Reverse enumeration
            Console.WriteLine($"Scores in descending order: [{string.Join(", ", scores.Reverse())}]");

            // Custom comparison example
            var wordsByLength = new SortedSet<string>(
                new[] { "apple", "pie", "a", "wonderful", "day" },
                Comparer<string>.Create((x, y) => x.Length.CompareTo(y.Length))
            );
            Console.WriteLine($"Words sorted by length: [{string.Join(", ", wordsByLength)}]");

            // Performance comparison insight
            Console.WriteLine("\nChoosing between HashSet<T> and SortedSet<T>:");
            Console.WriteLine("HashSet<T> when:");
            Console.WriteLine("  ✓ Need fastest possible Contains/Add/Remove");
            Console.WriteLine("  ✓ Don't care about order");
            Console.WriteLine("  ✓ Working with large datasets");
            
            Console.WriteLine("SortedSet<T> when:");
            Console.WriteLine("  ✓ Need elements in sorted order");
            Console.WriteLine("  ✓ Need range operations (Min, Max, GetViewBetween)");
            Console.WriteLine("  ✓ Custom sorting requirements\n");
        }

        /// <summary>
        /// Demonstrates BitArray - memory-efficient storage for boolean values.
        /// Shows bitwise operations and memory efficiency compared to bool arrays.
        /// </summary>
        static void DemonstrateBitArrays()
        {
            Console.WriteLine("=== 6. BitArray: Compact Boolean Collection ===");
            Console.WriteLine("Highly memory-efficient boolean storage using only 1 bit per value\n");

            Console.WriteLine("Memory efficiency comparison:");
            Console.WriteLine("  • bool array: 1 byte per boolean (8 bits, 7 wasted)");
            Console.WriteLine("  • BitArray: 1 bit per boolean (8x more efficient)");
            Console.WriteLine("  • For 1000 booleans: bool[] = ~1000 bytes, BitArray = ~125 bytes\n");

            // Create BitArray for user permissions
            var permissions = new BitArray(8); // 8 different permission flags
            string[] permissionNames = 
            { 
                "Read", "Write", "Execute", "Delete", "Admin", "Backup", "Restore", "Audit" 
            };

            // Set specific permissions
            permissions[0] = true;  // Read
            permissions[2] = true;  // Execute  
            permissions[4] = true;  // Admin
            permissions[7] = true;  // Audit

            Console.WriteLine("User permissions (1=granted, 0=denied):");
            for (int i = 0; i < permissions.Count; i++)
            {
                Console.WriteLine($"  {permissionNames[i]}: {(permissions[i] ? "1" : "0")}");
            }

            Console.WriteLine($"\nPermissions as bit string: {GetBitString(permissions)}");

            // Bitwise operations - modify in place
            var adminPermissions = new BitArray(8, true);  // All permissions
            var guestPermissions = new BitArray(8, false); // No permissions
            guestPermissions[0] = true; // Except read

            Console.WriteLine($"Admin permissions:  {GetBitString(adminPermissions)}");
            Console.WriteLine($"Guest permissions:  {GetBitString(guestPermissions)}");
            Console.WriteLine($"User permissions:   {GetBitString(permissions)}");

            // Union operation (OR) - combine permissions
            var combinedPermissions = new BitArray(permissions);
            combinedPermissions.Or(guestPermissions);
            Console.WriteLine($"Combined (User OR Guest): {GetBitString(combinedPermissions)}");

            // Intersection operation (AND) - common permissions
            var commonPermissions = new BitArray(permissions);
            commonPermissions.And(adminPermissions);
            Console.WriteLine($"Common (User AND Admin): {GetBitString(commonPermissions)}");

            // XOR operation - exclusive permissions
            var exclusivePermissions = new BitArray(permissions);
            exclusivePermissions.Xor(guestPermissions);
            Console.WriteLine($"Exclusive (User XOR Guest): {GetBitString(exclusivePermissions)}");

            // NOT operation - flip all bits
            var invertedPermissions = new BitArray(permissions);
            invertedPermissions.Not();
            Console.WriteLine($"Inverted (NOT User): {GetBitString(invertedPermissions)}");

            // Practical example: Feature flags
            Console.WriteLine("\nPractical example: Application feature flags");
            var featureFlags = new BitArray(5);
            string[] features = { "DarkMode", "BetaFeatures", "Analytics", "Notifications", "CloudSync" };

            // Enable some features
            featureFlags[0] = true; // DarkMode
            featureFlags[3] = true; // Notifications

            Console.WriteLine("Enabled features:");
            for (int i = 0; i < featureFlags.Count; i++)
            {
                if (featureFlags[i])
                {
                    Console.WriteLine($"  ✓ {features[i]}");
                }
            }

            // Demonstrate real-world BitArray usage with bytes
            Console.WriteLine("\nWorking with bytes and BitArray:");
            byte[] bytes = { 0b10101010, 0b11110000 }; // Binary literals
            var bitsFromBytes = new BitArray(bytes);
            
            Console.WriteLine($"From bytes [{bytes[0]}, {bytes[1]}]:");
            Console.WriteLine($"BitArray: {GetBitString(bitsFromBytes)}");
            
            // Convert back to bytes
            byte[] resultBytes = new byte[2];
            bitsFromBytes.CopyTo(resultBytes, 0);
            Console.WriteLine($"Back to bytes: [{resultBytes[0]}, {resultBytes[1]}]");

            Console.WriteLine("\nWhen to use BitArray:");
            Console.WriteLine("  ✓ Large numbers of boolean flags");
            Console.WriteLine("  ✓ Memory-constrained environments");
            Console.WriteLine("  ✓ Bitwise operations needed");
            Console.WriteLine("  ✓ Implementing bitmap algorithms");
            Console.WriteLine("  ✗ Small number of booleans (overhead not worth it)");
            Console.WriteLine("  ✗ Need frequent individual access patterns\n");
        }

        /// <summary>
        /// Helper method to convert BitArray to readable binary string
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

    /// <summary>
    /// Simple work item class for queue demonstration
    /// </summary>
    public class WorkItem
    {
        public int Id { get; set; }
        public string Task { get; set; } = "";
        public string Priority { get; set; } = "";
    }

    /// <summary>
    /// Simple editor action class for stack demonstration
    /// </summary>
    public class EditorAction
    {
        public string Type { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
