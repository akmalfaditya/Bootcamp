using System;
using System.Collections;
using System.Collections.Generic;

namespace EnumeratorAndIterators
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ENUMERATION AND ITERATORS DEMONSTRATION ===\n");

            // 1. Basic enumeration with foreach
            DemonstrateBasicEnumeration();

            // 2. Manual enumeration (what foreach does under the hood)
            DemonstrateManualEnumeration();

            // 3. Custom enumerable and enumerator
            DemonstrateCustomEnumerable();

            // 4. Collection initializers
            DemonstrateCollectionInitializers();

            // 5. Collection expressions (C# 12+)
            DemonstrateCollectionExpressions();

            // 6. Iterator methods with yield
            DemonstrateIterators();

            // 7. Composing sequences
            DemonstrateSequenceComposition();

            // 8. Iterator with try-finally
            DemonstrateIteratorWithFinally();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Enumeration Examples

        static void DemonstrateBasicEnumeration()
        {
            Console.WriteLine("--- 1. Basic Enumeration with foreach ---");
            
            // The foreach statement is the high-level way to iterate
            // It works with any type that implements IEnumerable<T> or has GetEnumerator() method
            string word = "beer";
            Console.WriteLine($"Iterating through the string '{word}':");
            
            foreach (char c in word)
            {
                Console.WriteLine($"  Character: {c}");
            }
            Console.WriteLine();
        }

        static void DemonstrateManualEnumeration()
        {
            Console.WriteLine("--- 2. Manual Enumeration (what foreach does behind the scenes) ---");
            
            string word = "beer";
            Console.WriteLine($"Manually iterating through '{word}' using GetEnumerator():");
            
            // This is what the compiler generates for foreach statements
            using (var enumerator = word.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var element = enumerator.Current;
                    Console.WriteLine($"  Character: {element}");
                }
            } // Dispose is called automatically due to 'using'
            Console.WriteLine();
        }

        #endregion

        #region Custom Enumerable Implementation

        static void DemonstrateCustomEnumerable()
        {
            Console.WriteLine("--- 3. Custom Enumerable and Enumerator ---");
            
            var countdown = new CountdownSequence(5);
            Console.WriteLine("Custom countdown sequence from 5 to 1:");
            
            foreach (int number in countdown)
            {
                Console.WriteLine($"  Count: {number}");
            }
            Console.WriteLine();
        }

        #endregion

        #region Collection Initializers and Expressions

        static void DemonstrateCollectionInitializers()
        {
            Console.WriteLine("--- 4. Collection Initializers ---");
            
            // Collection initializer syntax - syntactic sugar for calling Add() method
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine("List created with collection initializer:");
            foreach (int num in numbers)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine();
            
            // Dictionary with collection initializer
            var colors = new Dictionary<string, string>
            {
                { "red", "#FF0000" },
                { "green", "#00FF00" },
                { "blue", "#0000FF" }
            };
            
            // Dictionary with indexer syntax
            var moreColors = new Dictionary<string, string>
            {
                ["yellow"] = "#FFFF00",
                ["purple"] = "#800080",
                ["orange"] = "#FFA500"
            };
            
            Console.WriteLine("Dictionary colors:");
            foreach (var kvp in colors)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        static void DemonstrateCollectionExpressions()
        {
            Console.WriteLine("--- 5. Collection Expressions (C# 12+) ---");
            
            // Collection expressions use square brackets and are target-typed
            List<int> list = [1, 2, 3, 4, 5];
            int[] array = [10, 20, 30];
            
            Console.WriteLine("List created with collection expression:");
            foreach (int num in list)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine();
            
            Console.WriteLine("Array created with collection expression:");
            foreach (int num in array)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine("\n");
        }

        #endregion

        #region Iterator Methods

        static void DemonstrateIterators()
        {
            Console.WriteLine("--- 6. Iterator Methods with yield ---");
            
            Console.WriteLine("Fibonacci sequence (first 8 numbers):");
            foreach (int fib in GenerateFibonacci(8))
            {
                Console.Write($"{fib} ");
            }
            Console.WriteLine();
            
            Console.WriteLine("\nSquares of numbers 1-5:");
            foreach (int square in GenerateSquares(5))
            {
                Console.Write($"{square} ");
            }
            Console.WriteLine("\n");
        }

        // Iterator method that generates Fibonacci sequence
        // Notice: this method doesn't execute immediately when called
        // It returns an IEnumerable that produces values on-demand
        static IEnumerable<int> GenerateFibonacci(int count)
        {
            if (count <= 0) yield break; // Early termination with yield break
            
            int previous = 0, current = 1;
            
            for (int i = 0; i < count; i++)
            {
                yield return current; // Yields current value and pauses execution
                
                // Calculate next Fibonacci number
                int next = previous + current;
                previous = current;
                current = next;
            }
        }

        // Another iterator example
        static IEnumerable<int> GenerateSquares(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                yield return i * i; // Yields the square of i
            }
        }

        #endregion

        #region Sequence Composition

        static void DemonstrateSequenceComposition()
        {
            Console.WriteLine("--- 7. Composing Sequences ---");
            
            // Chain iterators together for powerful data processing
            var fibonacci = GenerateFibonacci(15);
            var evenFibs = FilterEvenNumbers(fibonacci);
            var limitedEvenFibs = TakeFirst(evenFibs, 4);
            
            Console.WriteLine("First 4 even Fibonacci numbers:");
            foreach (int num in limitedEvenFibs)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine("\n");
        }

        // Iterator that filters for even numbers only
        static IEnumerable<int> FilterEvenNumbers(IEnumerable<int> source)
        {
            foreach (int number in source)
            {
                if (number % 2 == 0)
                {
                    yield return number; // Only yield even numbers
                }
            }
        }

        // Iterator that takes only the first N elements
        static IEnumerable<T> TakeFirst<T>(IEnumerable<T> source, int count)
        {
            int taken = 0;
            foreach (T item in source)
            {
                if (taken >= count)
                    yield break; // Stop when we've taken enough
                
                yield return item;
                taken++;
            }
        }

        #endregion

        #region Iterator with Resource Management

        static void DemonstrateIteratorWithFinally()
        {
            Console.WriteLine("--- 8. Iterator with try-finally (Resource Management) ---");
            
            Console.WriteLine("Processing numbers with cleanup:");
            foreach (string result in ProcessNumbersWithCleanup([1, 2, 3]))
            {
                Console.WriteLine($"  {result}");
            }
            Console.WriteLine();
        }

        // Iterator that demonstrates proper resource management
        // The finally block executes when enumeration completes or is disposed
        static IEnumerable<string> ProcessNumbersWithCleanup(IEnumerable<int> numbers)
        {
            Console.WriteLine("  Setup: Initializing resources...");
            
            try
            {
                foreach (int number in numbers)
                {
                    // yield return can appear in try block with finally
                    yield return $"Processed: {number * 2}";
                }
            }
            finally
            {
                // This executes when enumeration ends or enumerator is disposed
                Console.WriteLine("  Cleanup: Resources released");
            }
        }

        #endregion
    }

    #region Custom Enumerable Implementation

    // Custom enumerable class that demonstrates the enumeration pattern
    public class CountdownSequence : IEnumerable<int>
    {
        private readonly int _start;

        public CountdownSequence(int start)
        {
            _start = start;
        }

        // GetEnumerator() returns an enumerator for this sequence
        public IEnumerator<int> GetEnumerator()
        {
            return new CountdownEnumerator(_start);
        }

        // Non-generic version required by IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // Custom enumerator that implements the cursor logic
    public class CountdownEnumerator : IEnumerator<int>
    {
        private readonly int _start;
        private int _current;
        private bool _started = false;

        public CountdownEnumerator(int start)
        {
            _start = start;
            _current = start + 1; // Start one above so first MoveNext() gives correct value
        }

        // Current element at cursor position
        public int Current { get; private set; }

        // Non-generic version
        object IEnumerator.Current => Current;

        // Move cursor to next position, return true if successful
        public bool MoveNext()
        {
            if (!_started)
            {
                _started = true;
                _current = _start;
            }
            else
            {
                _current--;
            }

            if (_current >= 1)
            {
                Current = _current;
                return true;
            }
            
            return false; // End of sequence reached
        }

        // Reset cursor to beginning (optional for most scenarios)
        public void Reset()
        {
            _current = _start + 1;
            _started = false;
        }

        // Clean up resources when enumeration is done
        public void Dispose()
        {
            // In this simple example, no cleanup needed
            // But this is where you'd release resources like file handles, database connections, etc.
        }
    }

    #endregion
}
