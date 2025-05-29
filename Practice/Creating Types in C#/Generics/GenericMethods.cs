using System;
using System.Collections.Generic;

namespace Generics
{
    /// <summary>
    /// A collection of generic utility methods
    /// These methods work with ANY type - the power of generic methods in action!
    /// Notice how we don't need to specify the type parameter when calling these -
    /// the compiler is smart enough to figure it out automatically
    /// </summary>
    public static class GenericUtilities
    {
        /// <summary>
        /// Generic swap method - works with ANY type that supports assignment
        /// This is the classic example of generic methods
        /// The compiler infers T from the arguments you pass
        /// </summary>
        /// <typeparam name="T">The type of values to swap</typeparam>
        /// <param name="a">First value (passed by reference)</param>
        /// <param name="b">Second value (passed by reference)</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            Console.WriteLine($"  Swapping {typeof(T).Name} values...");
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Print any array to console - works with arrays of any type
        /// Uses the object's ToString() method for display
        /// </summary>
        /// <typeparam name="T">The type of array elements</typeparam>
        /// <param name="array">The array to print</param>
        public static void PrintArray<T>(T[] array)
        {
            Console.Write($"  {typeof(T).Name}[]: [");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]);
                if (i < array.Length - 1) Console.Write(", ");
            }
            Console.WriteLine("]");
        }

        /// <summary>
        /// Find the index of an item in an array
        /// Uses the default equality comparer for type T
        /// Returns -1 if not found, just like List<T>.IndexOf()
        /// </summary>
        /// <typeparam name="T">The type of array elements</typeparam>
        /// <param name="array">The array to search</param>
        /// <param name="item">The item to find</param>
        /// <returns>Index of the item, or -1 if not found</returns>
        public static int FindIndex<T>(T[] array, T item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                // Use the default equality comparer - this handles nulls properly
                if (EqualityComparer<T>.Default.Equals(array[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Generic binary search - works with any comparable type
        /// The array must be sorted for this to work correctly
        /// This demonstrates how generic methods can have implicit type requirements
        /// </summary>
        /// <typeparam name="T">Type that implements IComparable<T></typeparam>
        /// <param name="sortedArray">A sorted array to search</param>
        /// <param name="item">The item to find</param>
        /// <returns>Index of the item, or -1 if not found</returns>
        public static int BinarySearch<T>(T[] sortedArray, T item) where T : IComparable<T>
        {
            int left = 0;
            int right = sortedArray.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int comparison = sortedArray[mid].CompareTo(item);

                if (comparison == 0)
                {
                    return mid;  // Found it!
                }
                else if (comparison < 0)
                {
                    left = mid + 1;  // Search right half
                }
                else
                {
                    right = mid - 1;  // Search left half
                }
            }

            return -1;  // Not found
        }

        /// <summary>
        /// Generic sorting using bubble sort (simple but inefficient)
        /// This shows how generic methods can implement algorithms
        /// Works with any type that can be compared
        /// </summary>
        /// <typeparam name="T">Type that implements IComparable<T></typeparam>
        /// <param name="array">Array to sort in-place</param>
        public static void BubbleSort<T>(T[] array) where T : IComparable<T>
        {
            Console.WriteLine($"  Sorting {typeof(T).Name} array using bubble sort...");
            
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (array[j].CompareTo(array[j + 1]) > 0)
                    {
                        // Use our own generic swap method!
                        Swap(ref array[j], ref array[j + 1]);
                    }
                }
            }
            
            Console.WriteLine("  Sorting complete!");
        }

        /// <summary>
        /// Create a copy of an array with a transformation applied
        /// This demonstrates generic methods that work with different input/output types
        /// </summary>
        /// <typeparam name="TInput">The input array element type</typeparam>
        /// <typeparam name="TOutput">The output array element type</typeparam>
        /// <param name="source">Source array</param>
        /// <param name="transform">Function to transform each element</param>
        /// <returns>New array with transformed elements</returns>
        public static TOutput[] Transform<TInput, TOutput>(TInput[] source, Func<TInput, TOutput> transform)
        {
            Console.WriteLine($"  Transforming {typeof(TInput).Name}[] to {typeof(TOutput).Name}[]...");
            
            TOutput[] result = new TOutput[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                result[i] = transform(source[i]);
            }
            
            return result;
        }

        /// <summary>
        /// Filter an array based on a predicate
        /// Returns a new array containing only elements that match the condition
        /// </summary>
        /// <typeparam name="T">The array element type</typeparam>
        /// <param name="source">Source array</param>
        /// <param name="predicate">Function that returns true for elements to keep</param>
        /// <returns>New array with filtered elements</returns>
        public static T[] Filter<T>(T[] source, Func<T, bool> predicate)
        {
            Console.WriteLine($"  Filtering {typeof(T).Name} array...");
            
            // First pass: count matching elements
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                {
                    count++;
                }
            }

            // Second pass: copy matching elements
            T[] result = new T[count];
            int resultIndex = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                {
                    result[resultIndex++] = source[i];
                }
            }

            Console.WriteLine($"  Filtered from {source.Length} to {result.Length} elements");
            return result;
        }

        /// <summary>
        /// Reduce an array to a single value using an accumulator function
        /// This is like LINQ's Aggregate method
        /// </summary>
        /// <typeparam name="T">The array element type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="source">Source array</param>
        /// <param name="seed">Initial accumulator value</param>
        /// <param name="accumulator">Function to combine accumulator with each element</param>
        /// <returns>Final accumulated value</returns>
        public static TResult Reduce<T, TResult>(T[] source, TResult seed, Func<TResult, T, TResult> accumulator)
        {
            Console.WriteLine($"  Reducing {typeof(T).Name}[] to {typeof(TResult).Name}...");
            
            TResult result = seed;
            foreach (T item in source)
            {
                result = accumulator(result, item);
            }
            
            return result;
        }

        /// <summary>
        /// Generic method to check if all elements in an array satisfy a condition
        /// </summary>
        /// <typeparam name="T">The array element type</typeparam>
        /// <param name="source">Source array</param>
        /// <param name="predicate">Condition to check</param>
        /// <returns>True if all elements satisfy the condition</returns>
        public static bool All<T>(T[] source, Func<T, bool> predicate)
        {
            foreach (T item in source)
            {
                if (!predicate(item))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Generic method to check if any element in an array satisfies a condition
        /// </summary>
        /// <typeparam name="T">The array element type</typeparam>
        /// <param name="source">Source array</param>
        /// <param name="predicate">Condition to check</param>
        /// <returns>True if any element satisfies the condition</returns>
        public static bool Any<T>(T[] source, Func<T, bool> predicate)
        {
            foreach (T item in source)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// More advanced generic utility methods demonstrating different scenarios
    /// These show how generics can be used in more complex situations
    /// </summary>
    public static class AdvancedGenericUtilities
    {
        /// <summary>
        /// Generic method with multiple type parameters
        /// Creates a dictionary from two arrays (keys and values)
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="keys">Array of keys</param>
        /// <param name="values">Array of values</param>
        /// <returns>Dictionary created from the arrays</returns>
        public static Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>(TKey[] keys, TValue[] values)
            where TKey : notnull  // C# 8.0+ nullable reference types
        {
            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Keys and values arrays must have the same length");
            }

            var dictionary = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Length; i++)
            {
                dictionary[keys[i]] = values[i];
            }

            return dictionary;
        }

        /// <summary>
        /// Generic method that works with nullable value types
        /// Shows how to handle null values in generic methods
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="values">Array of nullable values</param>
        /// <returns>First non-null value, or null if all are null</returns>
        public static T? FindFirstNonNull<T>(T?[] values) where T : struct
        {
            foreach (T? value in values)
            {
                if (value.HasValue)
                {
                    return value;
                }
            }
            return null;
        }

        /// <summary>
        /// Generic method with delegate parameter
        /// Shows how generics work with delegates and events
        /// </summary>
        /// <typeparam name="T">The type to process</typeparam>
        /// <param name="items">Items to process</param>
        /// <param name="processor">Action to perform on each item</param>
        public static void ProcessEach<T>(IEnumerable<T> items, Action<T> processor)
        {
            foreach (T item in items)
            {
                processor(item);
            }
        }
    }
}
