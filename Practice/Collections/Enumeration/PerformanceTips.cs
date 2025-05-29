using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Enumeration
{
    /// <summary>
    /// Demonstrates performance considerations and best practices for enumeration
    /// Understanding these concepts helps you write efficient, maintainable code
    /// </summary>
    public static class PerformanceTips
    {
        /// <summary>
        /// Shows performance differences between different enumeration approaches
        /// </summary>
        public static void DemonstratePerformanceConsiderations()
        {
            Console.WriteLine("10. Performance Tips and Best Practices");
            Console.WriteLine("=".PadRight(40, '='));
            
            Console.WriteLine("Key Performance Tips:");
            Console.WriteLine("1. Use generic collections (List<T>) over non-generic (ArrayList)");
            Console.WriteLine("2. Prefer foreach over manual indexing for enumeration");
            Console.WriteLine("3. Use LINQ methods appropriately (Any() vs Count() > 0)");
            Console.WriteLine("4. Be aware of deferred execution in LINQ and yield");
            Console.WriteLine("5. Avoid multiple enumeration of IEnumerable");
            Console.WriteLine("6. Use yield return for memory-efficient custom iterators");
            
            // Simple demonstration of deferred execution
            Console.WriteLine("\nDeferred Execution Example:");
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            
            // This doesn't execute immediately - it's deferred!
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            Console.WriteLine("Query created (not executed yet)");
            
            // Now it executes when we enumerate
            Console.WriteLine("Even numbers:");
            foreach (int even in evenNumbers)
            {
                Console.WriteLine($"  {even}");
            }
            
            Console.WriteLine("\nBest Practice: Use appropriate LINQ methods");
            Console.WriteLine($"  Has any items? {numbers.Any()}");
            Console.WriteLine($"  First even number: {numbers.FirstOrDefault(n => n % 2 == 0)}");
            
            // Demonstrate the difference between Count property and Count() method
            var list = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine($"  List.Count property: {list.Count} (O(1) - fast!)");
            
            var enumerable = list.Where(n => n > 0);
            Console.WriteLine($"  IEnumerable.Count() method: {enumerable.Count()} (O(n) - slower)");
            
            // Show yield in action
            Console.WriteLine("\nYield demonstration - memory efficient:");
            foreach (int square in GetSquares(5))
            {
                Console.WriteLine($"  Square: {square}");
            }
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates yield return for memory-efficient iteration
        /// Values are computed on-demand, not stored in memory
        /// </summary>
        private static IEnumerable<int> GetSquares(int count)
        {
            Console.WriteLine("    GetSquares called - starting to generate...");
            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine($"    Computing square of {i}");
                yield return i * i;
            }
            Console.WriteLine("    GetSquares finished");
        }
    }
}
