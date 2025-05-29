using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Enumeration
{
    class Program
    {        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Enumeration and Collections Demo ===\n");

            // Let's start with basic enumeration concepts
            BasicEnumerationDemo();
            
            // Show how IEnumerator works under the hood
            ManualEnumeratorDemo();
            
            // Demonstrate generic vs non-generic collections
            GenericVsNonGenericDemo();
            
            // Custom collection implementations
            CustomCollectionDemo();
            
            // Advanced enumeration patterns
            AdvancedEnumerationDemo();
            
            // Read-only collections demonstration
            ReadOnlyCollectionDemo.DemonstrateReadOnlyCollections();
            ReadOnlyCollectionDemo.DemonstrateProperCollectionExposure();
            
            // Yield patterns and custom enumerables
            YieldDemo.DemonstrateYieldPatterns();
              // Custom enumerable class demonstration
            DemonstrateCustomEnumerableClass();
            
            // Performance tips and best practices
            PerformanceTips.DemonstratePerformanceConsiderations();
            
            Console.WriteLine("\n=== Demo Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Shows basic enumeration using foreach - this is what you'll use 99% of the time
        /// </summary>
        static void BasicEnumerationDemo()
        {
            Console.WriteLine("1. Basic Enumeration with foreach");
            Console.WriteLine("=".PadRight(40, '='));
            
            // String implements IEnumerable, so we can iterate over characters
            string greeting = "Hello World";
            Console.Write("Characters in '{0}': ", greeting);
            foreach (char c in greeting)
            {
                Console.Write(c + " ");
            }
            Console.WriteLine("\n");
            
            // Arrays naturally support enumeration
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.Write("Numbers in array: ");
            foreach (int num in numbers)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Demonstrates how enumeration works behind the scenes using IEnumerator directly
        /// You rarely need to do this manually, but it's good to understand the mechanics
        /// </summary>
        static void ManualEnumeratorDemo()
        {
            Console.WriteLine("2. Manual Enumeration with IEnumerator");
            Console.WriteLine("=".PadRight(40, '='));
            
            string text = "Demo";
            
            // This is essentially what foreach does behind the scenes
            Console.WriteLine("Using IEnumerator manually (what foreach does internally):");
            IEnumerator enumerator = text.GetEnumerator();
            
            Console.Write("Characters: ");
            while (enumerator.MoveNext())
            {
                char currentChar = (char)enumerator.Current;
                Console.Write(currentChar + " ");
            }
            Console.WriteLine();
            
            // Always dispose of enumerators when done (foreach handles this automatically)
            if (enumerator is IDisposable disposable)
                disposable.Dispose();
            
            Console.WriteLine("Much easier with foreach, right?\n");
        }

        /// <summary>
        /// Shows the difference between generic and non-generic collections
        /// Generic collections are type-safe and perform better
        /// </summary>
        static void GenericVsNonGenericDemo()
        {
            Console.WriteLine("3. Generic vs Non-Generic Collections");
            Console.WriteLine("=".PadRight(40, '='));
            
            // Non-generic ArrayList - can hold any type of object
            ArrayList nonGenericList = new ArrayList();
            nonGenericList.Add(1);
            nonGenericList.Add("Hello");
            nonGenericList.Add(3.14);
            
            Console.WriteLine("Non-generic ArrayList (not recommended):");
            foreach (object item in nonGenericList)
            {
                // Need to cast or check type - not type-safe!
                Console.WriteLine($"Item: {item} (Type: {item.GetType().Name})");
            }
            
            // Generic List<T> - type-safe and performant
            List<int> genericList = new List<int> { 1, 2, 3, 4, 5 };
            
            Console.WriteLine("\nGeneric List<int> (recommended):");
            foreach (int number in genericList)
            {
                // No casting needed - compiler knows it's an int
                Console.WriteLine($"Number: {number} (squared: {number * number})");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates our custom collection implementations
        /// </summary>
        static void CustomCollectionDemo()
        {
            Console.WriteLine("4. Custom Collection Implementations");
            Console.WriteLine("=".PadRight(40, '='));
            
            // Test our custom collection
            var customCollection = new CustomCollection<string>();
            customCollection.Add("First");
            customCollection.Add("Second");
            customCollection.Add("Third");
            
            Console.WriteLine($"Custom Collection has {customCollection.Count} items:");
            foreach (string item in customCollection)
            {
                Console.WriteLine($"- {item}");
            }
            
            // Test our custom list
            var customList = new CustomList<int>();
            customList.Add(10);
            customList.Add(20);
            customList.Insert(1, 15); // Insert 15 between 10 and 20
            
            Console.WriteLine($"\nCustom List has {customList.Count} items:");
            for (int i = 0; i < customList.Count; i++)
            {
                Console.WriteLine($"[{i}]: {customList[i]}");
            }
            
            // Test some list operations
            Console.WriteLine($"Index of 15: {customList.IndexOf(15)}");
            Console.WriteLine($"Contains 20: {customList.Contains(20)}");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows some advanced enumeration patterns and scenarios
        /// </summary>
        static void AdvancedEnumerationDemo()
        {
            Console.WriteLine("5. Advanced Enumeration Patterns");
            Console.WriteLine("=".PadRight(40, '='));
            
            // Using yield return for custom enumeration
            Console.WriteLine("Custom enumeration with yield return:");
            foreach (int evenNumber in GetEvenNumbers(1, 10))
            {
                Console.Write(evenNumber + " ");
            }
            Console.WriteLine();
            
            // LINQ works because collections implement IEnumerable<T>
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Console.WriteLine("\nUsing LINQ (works because of IEnumerable<T>):");
            var evenSquares = numbers.Where(n => n % 2 == 0)
                                    .Select(n => n * n);
            
            Console.Write("Even squares: ");
            foreach (int square in evenSquares)
            {
                Console.Write(square + " ");
            }
            Console.WriteLine("\n");
        }        /// <summary>
        /// Demonstrates yield return - a powerful way to create custom enumerations
        /// The method returns IEnumerable<int> but uses yield to generate values on-demand
        /// </summary>
        static IEnumerable<int> GetEvenNumbers(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                {
                    yield return i; // This makes the method into an iterator
                }
            }
        }

        /// <summary>
        /// Demonstrates our custom enumerable class with NumberRange
        /// </summary>
        static void DemonstrateCustomEnumerableClass()
        {
            Console.WriteLine("9. Custom Enumerable Class Demo");
            Console.WriteLine("=".PadRight(40, '='));
            
            // Create a custom range that's enumerable
            var range = new NumberRange(1, 10, 2); // odd numbers from 1 to 10
            
            Console.WriteLine("Custom NumberRange (1 to 10, step 2):");
            foreach (int number in range)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
            
            // Use LINQ with our custom enumerable
            Console.WriteLine("\nSquares using LINQ:");
            var squares = range.Where(n => n > 3).Select(n => n * n);
            foreach (int square in squares)
            {
                Console.Write(square + " ");
            }
            Console.WriteLine();
            
            // Use our custom GetSquares method
            Console.WriteLine("\nSquares using custom method:");
            foreach (int square in range.GetSquares())
            {
                Console.Write(square + " ");
            }
            Console.WriteLine("\n");
        }
    }
}
