using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Enumeration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Enumeration Fundamentals Demo ===\n");

            // Core enumeration concepts - building from ground up
            BasicEnumerationProtocolDemo();
            
            // Show the raw IEnumerator mechanics (rarely used directly)
            LowLevelEnumeratorDemo();
            
            // The power of foreach - C#'s syntactic sugar
            ForeachSyntacticSugarDemo();
            
            // Generic vs non-generic interfaces
            GenericVsNonGenericInterfacesDemo();
            
            // Understanding IDisposable in enumeration
            EnumerationAndDisposalDemo();
            
            // When to use non-generic interfaces
            NonGenericInterfaceUseCasesDemo();
            
            // Implementation approaches
            ImplementationApproachesDemo();
            
            // Advanced custom implementations
            AdvancedCustomImplementationsDemo();
            
            Console.WriteLine("\n=== Demo Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Demonstrates the basic enumeration protocol with IEnumerator and IEnumerable
        /// These are the fundamental interfaces that make foreach possible
        /// </summary>
        static void BasicEnumerationProtocolDemo()
        {
            Console.WriteLine("1. Basic Enumeration Protocol - IEnumerator & IEnumerable");
            Console.WriteLine("=".PadRight(60, '='));
            
            // Every enumerable type implements IEnumerable
            // IEnumerable provides GetEnumerator() method
            // IEnumerator provides MoveNext(), Current, and Reset()
            
            Console.WriteLine("The enumeration protocol consists of two key interfaces:");
            Console.WriteLine("- IEnumerable: Provides GetEnumerator() method");
            Console.WriteLine("- IEnumerator: Provides MoveNext(), Current, Reset()");
            Console.WriteLine();
            
            // String implements IEnumerable, so it can be enumerated
            string text = "Hello";
            Console.WriteLine($"Enumerating string '{text}' character by character:");
            
            // foreach is syntactic sugar for the enumeration protocol
            foreach (char c in text)
            {
                Console.Write(c + " ");
            }
            Console.WriteLine();
            
            // Arrays also implement IEnumerable
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine("Enumerating array:");
            foreach (int num in numbers)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Shows how enumeration works at the lowest level using IEnumerator directly
        /// This is what foreach does behind the scenes - but you rarely need to write this
        /// Based on the exact example from the training material
        /// </summary>
        static void LowLevelEnumeratorDemo()
        {
            Console.WriteLine("2. Low-Level Enumeration - Direct IEnumerator Usage");
            Console.WriteLine("=".PadRight(60, '='));
            
            Console.WriteLine("This demonstrates the exact pattern from the training material:");
            Console.WriteLine();
            
            // Example from the material - exact code pattern
            string s = "Hello"; // String implements IEnumerable
            IEnumerator rator = s.GetEnumerator();

            Console.WriteLine("Manual enumeration (what foreach does internally):");
            Console.Write("Output: ");
            while (rator.MoveNext()) // Move to the first, then second, etc.
            {
                char c = (char)rator.Current; // Current returns object, requires cast
                Console.Write(c + ".");
            }
            Console.WriteLine();
            Console.WriteLine("(This produces: H.e.l.l.o.)");
            
            // Important: Always dispose when done!
            if (rator is IDisposable disposable)
            {
                disposable.Dispose();
                Console.WriteLine("Enumerator disposed properly");
            }
            
            Console.WriteLine();
            Console.WriteLine("Key points from this example:");
            Console.WriteLine("- Must call MoveNext() before accessing Current");
            Console.WriteLine("- Current returns object (non-generic), requires casting");
            Console.WriteLine("- Always dispose enumerators when done");
            Console.WriteLine("- This is exactly what foreach does automatically!");
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates how foreach is syntactic sugar for the enumeration protocol
        /// Shows the equivalent manual implementation
        /// </summary>
        static void ForeachSyntacticSugarDemo()
        {
            Console.WriteLine("3. Foreach - C#'s Syntactic Sugar");
            Console.WriteLine("=".PadRight(60, '='));
            
            string text = "C#";
            
            Console.WriteLine("Using foreach (the easy way):");
            foreach (char c in text)
            {
                Console.Write(c + " ");
            }
            Console.WriteLine();
            
            Console.WriteLine("\nWhat foreach actually does behind the scenes:");
            
            // This is the equivalent of the foreach loop above
            // The compiler transforms foreach into something like this:
            using (var enumerator = text.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char c = (char)enumerator.Current;
                    Console.Write(c + " ");
                }
            }
            Console.WriteLine();
            
            Console.WriteLine("\nKey benefits of foreach:");
            Console.WriteLine("- Automatic disposal (using statement)");
            Console.WriteLine("- Cleaner, more readable code");
            Console.WriteLine("- Less error-prone than manual enumeration");
            Console.WriteLine("- Works with any type implementing IEnumerable");
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates the difference between generic and non-generic enumeration interfaces
        /// Shows type safety and performance benefits of generics
        /// </summary>
        static void GenericVsNonGenericInterfacesDemo()
        {
            Console.WriteLine("4. Generic vs Non-Generic Interfaces");
            Console.WriteLine("=".PadRight(60, '='));
            
            Console.WriteLine("IEnumerator vs IEnumerator<T> comparison:");
            Console.WriteLine();
            
            // Demonstrate the difference in type safety
            Console.WriteLine("Non-generic IEnumerator:");
            Console.WriteLine("- Current returns object (requires casting)");
            Console.WriteLine("- Boxing/unboxing overhead for value types");
            Console.WriteLine("- Runtime type checking only");
            Console.WriteLine();
            
            Console.WriteLine("Generic IEnumerator<T>:");
            Console.WriteLine("- Current returns T (strongly typed)");
            Console.WriteLine("- No boxing for value types");
            Console.WriteLine("- Compile-time type safety");
            Console.WriteLine("- Inherits from IDisposable");
            Console.WriteLine();
            
            // Demonstrate standard practice pattern from the material
            Console.WriteLine("Standard practice for collections:");
            int[] data = { 1, 2, 3 };
            
            // Getting the generic enumerator (type-safe by default)
            var genericEnumerator = ((IEnumerable<int>)data).GetEnumerator();
            Console.WriteLine("Generic enumerator (cast to IEnumerable<int>):");
            while (genericEnumerator.MoveNext())
            {
                int value = genericEnumerator.Current; // No casting needed!
                Console.Write(value + " ");
            }
            genericEnumerator.Dispose();
            Console.WriteLine();
            
            // The non-generic version is usually explicitly implemented
            Console.WriteLine("\nNote: Collections typically expose IEnumerable<T> by default");
            Console.WriteLine("and hide IEnumerable through explicit interface implementation");
            Console.WriteLine("foreach automatically chooses the generic version when available");
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates how IEnumerator<T> inherits from IDisposable
        /// Shows how foreach automatically handles disposal
        /// </summary>
        static void EnumerationAndDisposalDemo()
        {
            Console.WriteLine("5. Enumeration and IDisposable");
            Console.WriteLine("=".PadRight(60, '='));
            
            Console.WriteLine("IEnumerator<T> inherits from IDisposable because:");
            Console.WriteLine("- Enumerators may hold unmanaged resources");
            Console.WriteLine("- Examples: file handles, database connections");
            Console.WriteLine("- Disposal ensures proper cleanup");
            Console.WriteLine();
            
            // Create a custom disposable enumerator demo
            var disposableCollection = new DisposableCollection();
            
            Console.WriteLine("Using foreach with disposable enumerator:");
            foreach (string item in disposableCollection)
            {
                Console.WriteLine($"Processing: {item}");
            }
            
            Console.WriteLine("\nforeach automatically translates to:");
            Console.WriteLine("using (var enumerator = collection.GetEnumerator())");
            Console.WriteLine("{");
            Console.WriteLine("    while (enumerator.MoveNext())");
            Console.WriteLine("    {");
            Console.WriteLine("        // process enumerator.Current");
            Console.WriteLine("    }");
            Console.WriteLine("} // Dispose called automatically");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows when and why to use non-generic interfaces
        /// Demonstrates type unification scenarios
        /// </summary>
        static void NonGenericInterfaceUseCasesDemo()
        {
            Console.WriteLine("6. When to Use Non-Generic Interfaces");
            Console.WriteLine("=".PadRight(60, '='));
            
            Console.WriteLine("Non-generic IEnumerable is still useful for:");
            Console.WriteLine("- Type unification across different element types");
            Console.WriteLine("- Working with legacy code");
            Console.WriteLine("- Recursive operations on nested collections");
            Console.WriteLine();
            
            // Example: counting elements in nested collections of any type
            var nestedData = new object[]
            {
                new int[] { 1, 2, 3 },
                new string[] { "a", "b" },
                new List<double> { 1.1, 2.2, 3.3, 4.4 }
            };
            
            Console.WriteLine("Counting elements in nested collections:");
            foreach (var collection in nestedData)
            {
                if (collection is IEnumerable enumerable)
                {
                    int count = CountElements(enumerable);
                    Console.WriteLine($"Collection type: {collection.GetType().Name}, Count: {count}");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates the three main approaches to implementing enumeration
        /// </summary>
        static void ImplementationApproachesDemo()
        {
            Console.WriteLine("7. Implementation Approaches");
            Console.WriteLine("=".PadRight(60, '='));
            
            Console.WriteLine("Three ways to implement IEnumerable<T>:");
            Console.WriteLine("1. Wrapping another collection's enumerator");
            Console.WriteLine("2. Using yield return (recommended)");
            Console.WriteLine("3. Manual IEnumerator implementation");
            Console.WriteLine();
            
            // Approach 1: Wrapper collection (simple but inflexible)
            var wrapperCollection = new WrapperIntList();
            Console.WriteLine("1. Wrapper approach (delegates to inner collection):");
            foreach (int item in wrapperCollection)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            
            // Approach 2: Yield return (recommended - compiler generates state machine)
            Console.WriteLine("\n2. Yield return approach (compiler magic):");
            var yieldCollection = new YieldIntList();
            foreach (int item in yieldCollection)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            
            // Also demonstrate static iterator methods
            Console.WriteLine("\n   Static iterator methods:");
            foreach (int item in IteratorMethods.GetSomeIntegers())
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            
            // Approach 3: Manual implementation (complex but full control)
            var manualCollection = new MyIntList();
            Console.WriteLine("\n3. Manual implementation (full control, complex):");
            foreach (int item in manualCollection)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            
            Console.WriteLine("\nKey takeaway: Use yield return unless you need very specific control!");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows advanced custom implementations including proper disposal
        /// </summary>
        static void AdvancedCustomImplementationsDemo()
        {
            Console.WriteLine("8. Advanced Custom Implementations");
            Console.WriteLine("=".PadRight(60, '='));
            
            // Custom range with yield
            Console.WriteLine("Custom range enumerable:");
            var range = new NumberRange(1, 10, 2);
            foreach (int number in range)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
            
            // Infinite sequence (be careful!)
            Console.WriteLine("\nInfinite sequence (first 5 elements):");
            int count = 0;
            foreach (int fibonacci in GetFibonacciSequence())
            {
                Console.Write(fibonacci + " ");
                if (++count >= 5) break; // Important: break out of infinite sequence!
            }
            Console.WriteLine();
            
            // Tree traversal
            Console.WriteLine("\nTree traversal:");
            var tree = new TreeNode("Root",
                new TreeNode("Child1", new TreeNode("Leaf1")),
                new TreeNode("Child2", new TreeNode("Leaf2")));
            
            foreach (string node in YieldDemo.TraverseTreeDepthFirst(tree))
            {
                Console.Write(node + " ");
            }
            Console.WriteLine("\n");
        }

        #region Helper Methods and Classes

        /// <summary>
        /// Recursively counts elements in any enumerable, including nested collections
        /// This demonstrates the power of non-generic IEnumerable for type unification
        /// </summary>
        static int CountElements(IEnumerable enumerable)
        {
            int count = 0;
            foreach (object element in enumerable)
            {
                if (element is IEnumerable nestedEnumerable && !(element is string))
                {
                    // Recursively count nested collections (but not strings)
                    count += CountElements(nestedEnumerable);
                }
                else
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Example of yield return - creates enumerable without defining a class
        /// </summary>
        static IEnumerable<int> GetNumbersWithYield()
        {
            yield return 10;
            yield return 20;
            yield return 30;
        }

        /// <summary>
        /// Infinite Fibonacci sequence using yield return
        /// </summary>
        static IEnumerable<int> GetFibonacciSequence()
        {
            int a = 0, b = 1;
            while (true)
            {
                yield return a;
                int temp = a + b;
                a = b;
                b = temp;
            }
        }

        #endregion
    }

    #region Supporting Classes

    /// <summary>
    /// Example of wrapping another collection's enumerator
    /// Simple but inflexible approach
    /// </summary>
    public class WrapperCollection<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _innerCollection;

        public WrapperCollection(IEnumerable<T> innerCollection)
        {
            _innerCollection = innerCollection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Example of manual IEnumerator implementation
    /// Complex but gives full control
    /// </summary>
    public class ManualEnumerableCollection : IEnumerable<string>
    {
        private readonly string[] _items = { "First", "Second", "Third" };

        public IEnumerator<string> GetEnumerator()
        {
            return new ManualEnumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class ManualEnumerator : IEnumerator<string>
        {
            private readonly string[] _items;
            private int _currentIndex = -1;

            public ManualEnumerator(string[] items)
            {
                _items = items;
            }

            public string Current
            {
                get
                {
                    if (_currentIndex < 0 || _currentIndex >= _items.Length)
                        throw new InvalidOperationException("Invalid enumerator position");
                    return _items[_currentIndex];
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                return ++_currentIndex < _items.Length;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }

            public void Dispose()
            {
                // No resources to dispose in this example
            }
        }
    }

    /// <summary>
    /// Example of a collection that demonstrates disposal
    /// </summary>
    public class DisposableCollection : IEnumerable<string>
    {
        private readonly string[] _items = { "Item1", "Item2", "Item3" };

        public IEnumerator<string> GetEnumerator()
        {
            return new DisposableEnumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class DisposableEnumerator : IEnumerator<string>
        {
            private readonly string[] _items;
            private int _currentIndex = -1;
            private bool _disposed = false;

            public DisposableEnumerator(string[] items)
            {
                _items = items;
                Console.WriteLine("  Enumerator created");
            }

            public string Current
            {
                get
                {
                    if (_disposed)
                        throw new ObjectDisposedException(nameof(DisposableEnumerator));
                    if (_currentIndex < 0 || _currentIndex >= _items.Length)
                        throw new InvalidOperationException("Invalid enumerator position");
                    return _items[_currentIndex];
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DisposableEnumerator));
                return ++_currentIndex < _items.Length;
            }

            public void Reset()
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DisposableEnumerator));
                _currentIndex = -1;
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    Console.WriteLine("  Enumerator disposed");
                    _disposed = true;
                }
            }
        }
    }

    #endregion
}
