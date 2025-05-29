using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Collections_Demo
{
    /// <summary>
    /// Advanced collection utilities and performance demonstrations
    /// These are the kind of helper methods you'll build in real projects
    /// </summary>
    public static class CollectionUtilities
    {
        /// <summary>
        /// Demonstrates performance differences between List and LinkedList
        /// This shows why choosing the right collection matters
        /// </summary>
        public static void CompareListPerformance()
        {
            Console.WriteLine("=== Collection Performance Comparison ===");
            const int itemCount = 50000;
            
            // Test List<T> performance
            var stopwatch = Stopwatch.StartNew();
            var list = new List<int>();
            
            // Insert at beginning (worst case for List<T>)
            for (int i = 0; i < itemCount; i++)
            {
                list.Insert(0, i);
            }
            stopwatch.Stop();
            long listTime = stopwatch.ElapsedMilliseconds;
            
            // Test LinkedList<T> performance
            stopwatch.Restart();
            var linkedList = new LinkedList<int>();
            
            // Insert at beginning (best case for LinkedList<T>)
            for (int i = 0; i < itemCount; i++)
            {
                linkedList.AddFirst(i);
            }
            stopwatch.Stop();
            long linkedListTime = stopwatch.ElapsedMilliseconds;
            
            Console.WriteLine($"Inserting {itemCount:N0} items at beginning:");
            Console.WriteLine($"  List<T>:       {listTime:N0} ms");
            Console.WriteLine($"  LinkedList<T>: {linkedListTime:N0} ms");
            Console.WriteLine($"  LinkedList is {(double)listTime / linkedListTime:F1}x faster");
            
            // Now test random access (List<T> advantage)
            stopwatch.Restart();
            int sum1 = 0;
            for (int i = 0; i < Math.Min(1000, list.Count); i++)
            {
                sum1 += list[i * 10];  // Random access
            }
            stopwatch.Stop();
            long listAccessTime = stopwatch.ElapsedMilliseconds;
            
            stopwatch.Restart();
            int sum2 = 0;
            int counter = 0;
            foreach (var item in linkedList)
            {
                if (counter % 10 == 0) sum2 += item;
                counter++;
                if (counter >= 10000) break;
            }
            stopwatch.Stop();
            long linkedListAccessTime = stopwatch.ElapsedMilliseconds;
            
            Console.WriteLine($"\nRandom access comparison:");
            Console.WriteLine($"  List<T> indexed access:     {listAccessTime} ms");
            Console.WriteLine($"  LinkedList<T> enumeration:  {linkedListAccessTime} ms");
            Console.WriteLine("Lesson: Use List<T> for random access, LinkedList<T> for frequent insertions\n");
        }

        /// <summary>
        /// Demonstrates efficient set operations for real-world scenarios
        /// Shows how sets can solve common programming problems elegantly
        /// </summary>
        public static void AdvancedSetOperations()
        {
            Console.WriteLine("=== Advanced Set Operations ===");
            
            // Scenario: Finding duplicate entries across multiple data sources
            var database1Users = new HashSet<string> 
            { 
                "alice@email.com", "bob@email.com", "charlie@email.com", "diana@email.com" 
            };
            
            var database2Users = new HashSet<string> 
            { 
                "bob@email.com", "eve@email.com", "frank@email.com", "diana@email.com" 
            };
            
            var csvImport = new HashSet<string> 
            { 
                "diana@email.com", "grace@email.com", "henry@email.com" 
            };
            
            Console.WriteLine("Finding duplicate users across systems:");
            Console.WriteLine($"Database 1: {string.Join(", ", database1Users)}");
            Console.WriteLine($"Database 2: {string.Join(", ", database2Users)}");
            Console.WriteLine($"CSV Import: {string.Join(", ", csvImport)}");
            
            // Find users in all three systems
            var allSystems = new HashSet<string>(database1Users);
            allSystems.IntersectWith(database2Users);
            allSystems.IntersectWith(csvImport);
            Console.WriteLine($"\nUsers in ALL systems: {string.Join(", ", allSystems)}");
            
            // Find users unique to database1
            var uniqueToDb1 = new HashSet<string>(database1Users);
            uniqueToDb1.ExceptWith(database2Users);
            uniqueToDb1.ExceptWith(csvImport);
            Console.WriteLine($"Unique to Database 1: {string.Join(", ", uniqueToDb1)}");
            
            // Find all unique users across all systems
            var allUsers = new HashSet<string>(database1Users);
            allUsers.UnionWith(database2Users);
            allUsers.UnionWith(csvImport);
            Console.WriteLine($"All unique users: {string.Join(", ", allUsers)}");
            Console.WriteLine($"Total unique users: {allUsers.Count}");
            
            Console.WriteLine("\nSet operations are incredibly powerful for data deduplication and analysis\n");
        }

        /// <summary>
        /// Demonstrates practical dictionary patterns you'll use constantly
        /// These are the bread-and-butter operations for real applications
        /// </summary>
        public static void PracticalDictionaryPatterns()
        {
            Console.WriteLine("=== Practical Dictionary Patterns ===");
            
            // Pattern 1: Grouping data
            var employees = new List<(string Name, string Department, int Salary)>
            {
                ("Alice", "Engineering", 85000),
                ("Bob", "Sales", 65000),
                ("Charlie", "Engineering", 92000),
                ("Diana", "Marketing", 58000),
                ("Eve", "Sales", 72000),
                ("Frank", "Engineering", 78000)
            };
            
            Console.WriteLine("1. Grouping employees by department:");
            var employeesByDept = new Dictionary<string, List<string>>();
            
            foreach (var emp in employees)
            {
                if (!employeesByDept.ContainsKey(emp.Department))
                {
                    employeesByDept[emp.Department] = new List<string>();
                }
                employeesByDept[emp.Department].Add(emp.Name);
            }
            
            foreach (var dept in employeesByDept)
            {
                Console.WriteLine($"  {dept.Key}: {string.Join(", ", dept.Value)}");
            }
            
            // Pattern 2: Caching expensive calculations
            Console.WriteLine("\n2. Caching pattern (memoization):");
            var fibonacciCache = new Dictionary<int, long>();
            
            long CalculateFibonacci(int n)
            {
                if (n <= 1) return n;
                
                if (fibonacciCache.ContainsKey(n))
                {
                    Console.WriteLine($"  Cache hit for fib({n})");
                    return fibonacciCache[n];
                }
                
                Console.WriteLine($"  Calculating fib({n})");
                long result = CalculateFibonacci(n - 1) + CalculateFibonacci(n - 2);
                fibonacciCache[n] = result;
                return result;
            }
            
            Console.WriteLine($"Fibonacci(10) = {CalculateFibonacci(10)}");
            Console.WriteLine($"Cache size: {fibonacciCache.Count} entries");
            
            // Pattern 3: Configuration management
            Console.WriteLine("\n3. Configuration management:");
            var config = new Dictionary<string, object>
            {
                ["ConnectionString"] = "Server=localhost;Database=MyApp",
                ["MaxConnections"] = 100,
                ["EnableLogging"] = true,
                ["CacheTimeout"] = TimeSpan.FromMinutes(30)
            };
            
            // Safe configuration retrieval with defaults
            T GetConfigValue<T>(string key, T defaultValue = default)
            {
                if (config.TryGetValue(key, out var value) && value is T typedValue)
                {
                    return typedValue;
                }
                return defaultValue;
            }
            
            Console.WriteLine($"  Connection String: {GetConfigValue<string>("ConnectionString")}");
            Console.WriteLine($"  Max Connections: {GetConfigValue<int>("MaxConnections")}");
            Console.WriteLine($"  Debug Mode: {GetConfigValue<bool>("DebugMode", false)}"); // Uses default
            
            Console.WriteLine("Dictionaries are essential for grouping, caching, and configuration\n");
        }

        /// <summary>
        /// Shows how to choose the right collection for your specific needs
        /// This decision tree helps in real-world scenarios
        /// </summary>
        public static void CollectionSelectionGuide()
        {
            Console.WriteLine("=== Collection Selection Guide ===");
            Console.WriteLine("Choose your collection based on these criteria:\n");
            
            Console.WriteLine("📋 SEQUENTIAL ACCESS (List-like):");
            Console.WriteLine("  • List<T>        → Random access, dynamic size, most common choice");
            Console.WriteLine("  • LinkedList<T>  → Frequent insertions/deletions in middle");
            Console.WriteLine("  • ArrayList      → Legacy code only (avoid in new projects)");
            
            Console.WriteLine("\n🔄 ORDERED PROCESSING:");
            Console.WriteLine("  • Queue<T>       → First-in-first-out (task processing, BFS)");
            Console.WriteLine("  • Stack<T>       → Last-in-first-out (undo, DFS, expression parsing)");
            
            Console.WriteLine("\n🎯 UNIQUE ELEMENTS:");
            Console.WriteLine("  • HashSet<T>     → Fast lookup, no duplicates, unordered");
            Console.WriteLine("  • SortedSet<T>   → Unique elements in sorted order");
            
            Console.WriteLine("\n🗝️ KEY-VALUE PAIRS:");
            Console.WriteLine("  • Dictionary<K,V>       → Fast lookup by key (most common)");
            Console.WriteLine("  • SortedDictionary<K,V> → Keys in sorted order");
            Console.WriteLine("  • SortedList<K,V>       → Sorted keys + index access");
            Console.WriteLine("  • OrderedDictionary     → Preserves insertion order");
            
            Console.WriteLine("\n🏷️ SPECIAL CASES:");
            Console.WriteLine("  • BitArray              → Memory-efficient boolean arrays");
            Console.WriteLine("  • ListDictionary        → Very small collections (<10 items)");
            Console.WriteLine("  • HybridDictionary      → Adaptive (legacy, use Dictionary<K,V>)");
            
            Console.WriteLine("\n💡 DECISION TREE:");
            Console.WriteLine("  1. Need key-value? → Dictionary<K,V>");
            Console.WriteLine("  2. Need unique items? → HashSet<T>");
            Console.WriteLine("  3. Need ordering? → Queue<T> (FIFO) or Stack<T> (LIFO)");
            Console.WriteLine("  4. Need indexed access? → List<T>");
            Console.WriteLine("  5. Frequent middle insertions? → LinkedList<T>");
            Console.WriteLine("  6. Large boolean array? → BitArray");
            
            Console.WriteLine("\n🚀 PERFORMANCE QUICK REFERENCE:");
            Console.WriteLine("  Operation              List<T>  Dict<K,V>  HashSet<T>  Queue<T>  Stack<T>");
            Console.WriteLine("  ───────────────────────────────────────────────────────────────────────");
            Console.WriteLine("  Add to end            O(1)     O(1)       O(1)        O(1)      O(1)");
            Console.WriteLine("  Insert at beginning   O(n)     N/A        N/A         O(1)      O(1)");
            Console.WriteLine("  Random access         O(1)     O(1)       N/A         N/A       N/A");
            Console.WriteLine("  Contains check        O(n)     O(1)       O(1)        O(n)      O(n)");
            Console.WriteLine("  Remove by value       O(n)     O(1)       O(1)        N/A       N/A");
            
            Console.WriteLine("\nRemember: Start with the simplest collection that meets your needs!");
            Console.WriteLine("You can always refactor to a more specialized collection later.\n");
        }

        /// <summary>
        /// Demonstrates common collection anti-patterns and how to avoid them
        /// Learn from these mistakes so you don't repeat them
        /// </summary>
        public static void CommonMistakesAndSolutions()
        {
            Console.WriteLine("=== Common Collection Mistakes & Solutions ===");
            
            Console.WriteLine("❌ MISTAKE 1: Using List<T> for uniqueness checks");
            Console.WriteLine("Don't do this:");
            var badList = new List<string> { "apple", "banana", "apple", "cherry" };
            Console.WriteLine($"  List with duplicates: [{string.Join(", ", badList)}]");
            
            // Inefficient way to check uniqueness
            var stopwatch = Stopwatch.StartNew();
            var hasDuplicates = badList.Count != badList.Distinct().Count();
            stopwatch.Stop();
            Console.WriteLine($"  Checking duplicates with LINQ: {stopwatch.ElapsedTicks} ticks");
            
            Console.WriteLine("✅ Do this instead:");
            stopwatch.Restart();
            var goodSet = new HashSet<string>(badList);
            var wasUnique = goodSet.Count == badList.Count;
            stopwatch.Stop();
            Console.WriteLine($"  Using HashSet: {stopwatch.ElapsedTicks} ticks (much faster!)");
            
            Console.WriteLine("\n❌ MISTAKE 2: Wrong collection for frequent insertions");
            Console.WriteLine("Don't insert at beginning of List<T> repeatedly:");
            var inefficientList = new List<int>();
            stopwatch.Restart();
            for (int i = 0; i < 1000; i++)
            {
                inefficientList.Insert(0, i);  // O(n) each time!
            }
            stopwatch.Stop();
            Console.WriteLine($"  1000 insertions at beginning of List<T>: {stopwatch.ElapsedMilliseconds} ms");
            
            Console.WriteLine("✅ Use LinkedList<T> or reverse the process:");
            var efficientLinkedList = new LinkedList<int>();
            stopwatch.Restart();
            for (int i = 0; i < 1000; i++)
            {
                efficientLinkedList.AddFirst(i);  // O(1) each time!
            }
            stopwatch.Stop();
            Console.WriteLine($"  1000 insertions at beginning of LinkedList<T>: {stopwatch.ElapsedMilliseconds} ms");
            
            Console.WriteLine("\n❌ MISTAKE 3: Not using TryGetValue with dictionaries");
            var dictionary = new Dictionary<string, int> { ["apple"] = 5, ["banana"] = 3 };
            
            Console.WriteLine("Don't do this (can throw exception):");
            Console.WriteLine("  // var count = dictionary[\"orange\"]; // KeyNotFoundException!");
            
            Console.WriteLine("✅ Do this instead:");
            if (dictionary.TryGetValue("orange", out int count))
            {
                Console.WriteLine($"  Orange count: {count}");
            }
            else
            {
                Console.WriteLine("  Orange not found (safe!)");
            }
            
            Console.WriteLine("\n❌ MISTAKE 4: Using ArrayList or Hashtable in new code");
            Console.WriteLine("These are legacy collections - avoid them:");
            Console.WriteLine("  • ArrayList → Use List<T>");
            Console.WriteLine("  • Hashtable → Use Dictionary<TKey, TValue>");
            Console.WriteLine("  • Generics provide type safety and better performance");
            
            Console.WriteLine("\n💡 GOLDEN RULES:");
            Console.WriteLine("  1. Choose collections based on primary operations");
            Console.WriteLine("  2. Use HashSet<T> for uniqueness, not List<T>.Distinct()");
            Console.WriteLine("  3. Always use TryGetValue with dictionaries");
            Console.WriteLine("  4. Prefer generic collections over non-generic ones");
            Console.WriteLine("  5. Consider LINQ for complex queries, but watch performance");
            Console.WriteLine("  6. When in doubt, start with List<T> or Dictionary<K,V>\n");
        }
    }
}
