using System;
using System.Collections;
using System.Collections.Generic;

namespace Enumeration
{
    /// <summary>
    /// This class demonstrates the manual implementation of IEnumerator and IEnumerable
    /// as described in the training material. This is the most complex approach and is rarely
    /// necessary since yield return handles it automatically, but understanding it reveals
    /// how iterators work under the hood.
    /// 
    /// This example is based directly on the material's MyIntList example.
    /// </summary>
    public class MyIntList : IEnumerable<int>
    {
        // Simple internal storage - in real implementation you might use arrays or other structures
        private readonly int[] data = { 1, 2, 3 };

        /// <summary>
        /// Returns a generic enumerator for type safety and performance
        /// </summary>
        public IEnumerator<int> GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Explicit implementation for non-generic interface (required for backward compatibility)
        /// This is the pattern described in the material for hiding non-generic implementation
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Inner enumerator class that implements the actual enumeration logic
        /// This is where all the enumeration state management happens
        /// </summary>
        private class Enumerator : IEnumerator<int>
        {
            private readonly MyIntList collection;
            private int currentIndex = -1; // -1 means before first element (important!)

            public Enumerator(MyIntList items) => this.collection = items;

            /// <summary>
            /// Generic Current property - returns typed value
            /// This is what gives us type safety and performance benefits
            /// </summary>
            public int Current
            {
                get
                {
                    // Proper bounds checking as mentioned in the material
                    if (currentIndex < 0 || currentIndex >= collection.data.Length)
                        throw new InvalidOperationException("Enumerator is not positioned on a valid element");
                    
                    return collection.data[currentIndex];
                }
            }

            /// <summary>
            /// Non-generic Current - required by IEnumerator interface
            /// Notice how it just calls the generic version to avoid duplication
            /// </summary>
            object IEnumerator.Current => Current;

            /// <summary>
            /// Advances to the next element
            /// Returns true if successful, false if end is reached
            /// Must be called before accessing Current for the first time
            /// </summary>
            public bool MoveNext()
            {
                return ++currentIndex < collection.data.Length;
            }

            /// <summary>
            /// Resets enumerator to initial position
            /// As mentioned in the material, this is rarely used in modern C# and
            /// primarily exists for COM interoperability
            /// </summary>
            public void Reset() => currentIndex = -1;

            /// <summary>
            /// Implements IDisposable - critical for resource cleanup
            /// In this simple case, no cleanup is needed, but real enumerators
            /// might need to close files, database connections, etc.
            /// </summary>
            public void Dispose() 
            { 
                // No resources to dispose in this simple example
                // In real scenarios, you might close files, dispose connections, etc.
            }
        }
    }

    /// <summary>
    /// This class demonstrates the wrapper approach mentioned in the material
    /// This is the simplest implementation - just delegate to another collection's enumerator
    /// Simple but inflexible, as noted in the training material
    /// </summary>
    public class WrapperIntList : IEnumerable<int>
    {
        private readonly List<int> innerList = new List<int> { 10, 20, 30 };

        /// <summary>
        /// Just return the inner collection's enumerator
        /// This is simple but doesn't give you control over the enumeration process
        /// </summary>
        public IEnumerator<int> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        /// <summary>
        /// Standard explicit implementation for non-generic interface
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// This class demonstrates the yield return approach mentioned in the material
    /// This is the most common and recommended approach in modern C#
    /// The yield return statement instructs the compiler to automatically generate
    /// the complex IEnumerator implementation behind the scenes
    /// </summary>
    public class YieldIntList : IEnumerable<int>
    {
        private readonly int[] data = { 100, 200, 300 };

        /// <summary>
        /// Using yield return - the compiler generates all the enumeration logic for us!
        /// This is much simpler than the manual implementation above
        /// </summary>
        public IEnumerator<int> GetEnumerator()
        {
            // The magic happens here - yield return creates a state machine
            foreach (int value in data)
            {
                // You can add logic here - filtering, transformation, etc.
                yield return value; // Compiler generates the state machine
            }
        }

        /// <summary>
        /// Standard explicit implementation
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Static class to demonstrate iterator methods that return IEnumerable<T>
    /// without defining a full class, as mentioned in the material
    /// </summary>
    public static class IteratorMethods
    {
        /// <summary>
        /// Static iterator method that returns IEnumerable<int>
        /// This pattern is used extensively in LINQ
        /// </summary>
        public static IEnumerable<int> GetSomeIntegers()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        /// <summary>
        /// More complex iterator method with parameters
        /// Demonstrates lazy evaluation - values are generated only when requested
        /// </summary>
        public static IEnumerable<int> GetRange(int start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return start + i;
            }
        }

        /// <summary>
        /// Iterator method with filtering logic
        /// Shows how yield can be combined with conditions
        /// </summary>
        public static IEnumerable<int> GetEvenNumbersInRange(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                {
                    yield return i;
                }
            }
        }
    }
}
