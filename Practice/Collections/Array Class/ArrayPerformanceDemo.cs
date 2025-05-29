using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Array_Class
{
    /// <summary>
    /// Demonstrates performance characteristics and edge cases of Array operations
    /// Important for understanding when to use arrays vs other collections
    /// </summary>
    public static class ArrayPerformanceDemo
    {
        /// <summary>
        /// Compares performance of different array operations
        /// Helps understand the cost of various operations
        /// </summary>
        public static void RunPerformanceComparisons()
        {
            Console.WriteLine("=== Array Performance Demonstrations ===\n");

            // Array vs List performance for known size
            CompareArrayVsList();

            // Different copying methods performance
            CompareCopyingMethods();

            // Search performance comparison
            CompareSearchMethods();

            // Memory allocation patterns
            DemonstrateMemoryPatterns();

            // Edge cases and gotchas
            DemonstrateEdgeCases();
        }

        /// <summary>
        /// Compares array vs List<T> for scenarios where size is known
        /// Shows when arrays are preferred
        /// </summary>
        private static void CompareArrayVsList()
        {
            Console.WriteLine("1. Array vs List<T> Performance (Known Size)");
            Console.WriteLine("=".PadRight(50, '='));

            const int size = 1000000;
            const int iterations = 10;

            // Array performance
            var stopwatch = Stopwatch.StartNew();
            for (int iter = 0; iter < iterations; iter++)
            {
                int[] array = new int[size];
                for (int i = 0; i < size; i++)
                {
                    array[i] = i;
                }
                // Force some usage to prevent optimization
                int sum = 0;
                for (int i = 0; i < size; i++)
                {
                    sum += array[i];
                }
            }
            stopwatch.Stop();
            long arrayTime = stopwatch.ElapsedMilliseconds;

            // List performance
            stopwatch.Restart();
            for (int iter = 0; iter < iterations; iter++)
            {
                var list = new List<int>(size); // Pre-allocate capacity
                for (int i = 0; i < size; i++)
                {
                    list.Add(i);
                }
                // Force some usage
                int sum = 0;
                for (int i = 0; i < size; i++)
                {
                    sum += list[i];
                }
            }
            stopwatch.Stop();
            long listTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"Array time:     {arrayTime}ms");
            Console.WriteLine($"List<T> time:   {listTime}ms");
            Console.WriteLine($"Performance:    Array is {(double)listTime / arrayTime:F1}x faster");
            Console.WriteLine($"Memory:         Array uses less memory overhead\n");
        }

        /// <summary>
        /// Compares different array copying methods
        /// Important for understanding bulk operations
        /// </summary>
        private static void CompareCopyingMethods()
        {
            Console.WriteLine("2. Array Copying Methods Performance");
            Console.WriteLine("=".PadRight(50, '='));

            const int size = 100000;
            const int iterations = 1000;
            int[] source = new int[size];
            for (int i = 0; i < size; i++)
            {
                source[i] = i;
            }

            // Array.Copy performance
            var stopwatch = Stopwatch.StartNew();
            for (int iter = 0; iter < iterations; iter++)
            {
                int[] dest = new int[size];
                Array.Copy(source, dest, size);
            }
            stopwatch.Stop();
            long arrayCopyTime = stopwatch.ElapsedMilliseconds;

            // Buffer.BlockCopy performance (for primitives)
            stopwatch.Restart();
            for (int iter = 0; iter < iterations; iter++)
            {
                int[] dest = new int[size];
                Buffer.BlockCopy(source, 0, dest, 0, size * sizeof(int));
            }
            stopwatch.Stop();
            long bufferCopyTime = stopwatch.ElapsedMilliseconds;

            // Manual loop copy
            stopwatch.Restart();
            for (int iter = 0; iter < iterations; iter++)
            {
                int[] dest = new int[size];
                for (int i = 0; i < size; i++)
                {
                    dest[i] = source[i];
                }
            }
            stopwatch.Stop();
            long loopCopyTime = stopwatch.ElapsedMilliseconds;

            // Clone method
            stopwatch.Restart();
            for (int iter = 0; iter < iterations; iter++)
            {
                int[] dest = (int[])source.Clone();
            }
            stopwatch.Stop();
            long cloneTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"Array.Copy:       {arrayCopyTime}ms");
            Console.WriteLine($"Buffer.BlockCopy: {bufferCopyTime}ms (fastest for primitives)");
            Console.WriteLine($"Manual loop:      {loopCopyTime}ms");
            Console.WriteLine($"Clone():          {cloneTime}ms");
            Console.WriteLine($"Best choice:      Buffer.BlockCopy for primitives, Array.Copy for objects\n");
        }

        /// <summary>
        /// Compares linear vs binary search performance
        /// Shows importance of choosing right algorithm
        /// </summary>
        private static void CompareSearchMethods()
        {
            Console.WriteLine("3. Search Methods Performance");
            Console.WriteLine("=".PadRight(50, '='));

            const int size = 100000;
            const int searches = 1000;
            
            // Create sorted array for testing
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = i * 2; // Even numbers
            }

            Random random = new Random(42); // Fixed seed for reproducible results
            int[] searchValues = new int[searches];
            for (int i = 0; i < searches; i++)
            {
                searchValues[i] = random.Next(0, size * 2);
            }

            // Linear search using Array.Find
            var stopwatch = Stopwatch.StartNew();
            int linearFound = 0;
            for (int i = 0; i < searches; i++)
            {
                int value = searchValues[i];
                int found = Array.Find(array, x => x == value);
                if (found != 0) linearFound++;
            }
            stopwatch.Stop();
            long linearTime = stopwatch.ElapsedMilliseconds;

            // Binary search
            stopwatch.Restart();
            int binaryFound = 0;
            for (int i = 0; i < searches; i++)
            {
                int index = Array.BinarySearch(array, searchValues[i]);
                if (index >= 0) binaryFound++;
            }
            stopwatch.Stop();
            long binaryTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"Linear search:  {linearTime}ms (found {linearFound}/{searches})");
            Console.WriteLine($"Binary search:  {binaryTime}ms (found {binaryFound}/{searches})");
            Console.WriteLine($"Performance:    Binary search is {(double)linearTime / binaryTime:F1}x faster");
            Console.WriteLine($"Requirement:    Binary search needs sorted array\n");
        }

        /// <summary>
        /// Demonstrates memory allocation patterns with arrays
        /// Important for understanding memory usage
        /// </summary>
        private static void DemonstrateMemoryPatterns()
        {
            Console.WriteLine("4. Memory Allocation Patterns");
            Console.WriteLine("=".PadRight(50, '='));

            // Show memory usage difference between value and reference types
            Console.WriteLine("Memory usage comparison:");
            
            // Array of value types
            long initialMemory = GC.GetTotalMemory(true);
            int[] valueArray = new int[100000];
            long afterValueArray = GC.GetTotalMemory(false);
            
            // Array of reference types
            string[] refArray = new string[100000];
            for (int i = 0; i < 1000; i++) // Only fill some to show the difference
            {
                refArray[i] = $"String {i}";
            }
            long afterRefArray = GC.GetTotalMemory(false);

            Console.WriteLine($"Initial memory:           {initialMemory:N0} bytes");
            Console.WriteLine($"After int[100000]:        {afterValueArray:N0} bytes (+{afterValueArray - initialMemory:N0})");
            Console.WriteLine($"After string[100000]:     {afterRefArray:N0} bytes (+{afterRefArray - afterValueArray:N0})");
            Console.WriteLine($"Value type array:         Stores data directly");
            Console.WriteLine($"Reference type array:     Stores references + object data\n");

            // Demonstrate array pooling concept
            Console.WriteLine("Array pooling pattern (advanced):");
            Console.WriteLine("- For temporary arrays, consider ArrayPool<T>");
            Console.WriteLine("- Reduces garbage collection pressure");
            Console.WriteLine("- Important for high-performance scenarios\n");
        }

        /// <summary>
        /// Demonstrates edge cases and common pitfalls
        /// Important for robust programming
        /// </summary>
        private static void DemonstrateEdgeCases()
        {
            Console.WriteLine("5. Edge Cases and Common Pitfalls");
            Console.WriteLine("=".PadRight(50, '='));

            // Null vs empty arrays
            Console.WriteLine("Null vs Empty Arrays:");
            int[] nullArray = null;
            int[] emptyArray = new int[0];
            
            Console.WriteLine($"Null array:       {(nullArray == null ? "null" : "not null")}");
            Console.WriteLine($"Empty array:      Length = {emptyArray.Length}");
            Console.WriteLine($"Empty is valid:   Can safely iterate (no elements)");
            Console.WriteLine($"Null throws:      NullReferenceException on access\n");

            // Array bounds
            Console.WriteLine("Array Bounds Checking:");
            int[] smallArray = { 1, 2, 3 };
            Console.WriteLine($"Array: [{string.Join(", ", smallArray)}]");
            Console.WriteLine($"Valid indices: 0 to {smallArray.Length - 1}");
            Console.WriteLine($"GetLowerBound(0): {smallArray.GetLowerBound(0)}");
            Console.WriteLine($"GetUpperBound(0): {smallArray.GetUpperBound(0)}");
            
            try
            {
                // This would throw IndexOutOfRangeException
                // int invalid = smallArray[10];
                Console.WriteLine("Index bounds are automatically checked\n");
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Bounds violation: {ex.Message}\n");
            }

            // Covariance in arrays (dangerous with reference types)
            Console.WriteLine("Array Covariance (Reference Types Only):");
            string[] strings = { "Hello", "World" };
            object[] objects = strings; // This is allowed!
            
            Console.WriteLine("string[] can be assigned to object[]");
            Console.WriteLine("But runtime type checking still applies:");
            
            try
            {
                // This compiles but throws at runtime!
                // objects[0] = 123; // ArrayTypeMismatchException
                Console.WriteLine("Adding wrong type throws ArrayTypeMismatchException\n");
            }
            catch (ArrayTypeMismatchException ex)
            {
                Console.WriteLine($"Type mismatch: {ex.Message}\n");
            }

            // Multidimensional array gotchas
            Console.WriteLine("Multidimensional Array Considerations:");
            int[,] rect = new int[2, 3];
            int[][] jagged = new int[2][];
            jagged[0] = new int[3];
            jagged[1] = new int[3];
            
            Console.WriteLine($"Rectangular [,]:  Single object, fixed dimensions");
            Console.WriteLine($"Jagged []:[]      Multiple objects, variable dimensions");
            Console.WriteLine($"Performance:      Rectangular faster, Jagged more flexible\n");

            // Best practices summary
            Console.WriteLine("Best Practices Summary:");
            Console.WriteLine("✓ Use arrays for fixed-size, high-performance scenarios");
            Console.WriteLine("✓ Pre-allocate when size is known");
            Console.WriteLine("✓ Check for null before accessing");
            Console.WriteLine("✓ Use Array.Copy for bulk operations");
            Console.WriteLine("✓ Consider List<T> for dynamic sizing");
            Console.WriteLine("✓ Use appropriate search method (linear vs binary)");
            Console.WriteLine("✗ Don't resize arrays frequently");
            Console.WriteLine("✗ Don't rely on array covariance for mutations\n");
        }
    }
}
