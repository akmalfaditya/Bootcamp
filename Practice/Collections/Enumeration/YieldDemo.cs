using System;
using System.Collections;
using System.Collections.Generic;

namespace Enumeration
{
    /// <summary>
    /// Demonstrates advanced enumeration patterns using yield return
    /// Yield is a powerful C# feature for creating iterators without implementing IEnumerator manually
    /// </summary>
    public static class YieldDemo
    {
        /// <summary>
        /// Demonstrates various yield patterns
        /// </summary>
        public static void DemonstrateYieldPatterns()
        {
            Console.WriteLine("8. Yield Return Patterns");
            Console.WriteLine("=".PadRight(40, '='));

            // Fibonacci sequence using yield
            Console.WriteLine("First 10 Fibonacci numbers:");
            foreach (int fib in GetFibonacci(10))
            {
                Console.Write(fib + " ");
            }
            Console.WriteLine();

            // Filtered enumeration
            Console.WriteLine("\nEven numbers from 1 to 20:");
            foreach (int even in GetEvenNumbers(1, 20))
            {
                Console.Write(even + " ");
            }
            Console.WriteLine();

            // Infinite sequence (be careful with these!)
            Console.WriteLine("\nFirst 5 powers of 2:");
            int count = 0;
            foreach (int power in GetPowersOfTwo())
            {
                Console.Write(power + " ");
                if (++count >= 5) break; // Important: break out of infinite sequence!
            }
            Console.WriteLine();

            // Tree traversal using yield
            Console.WriteLine("\nTree traversal example:");
            var tree = CreateSampleTree();
            Console.WriteLine("Depth-first traversal:");
            foreach (string node in TraverseTreeDepthFirst(tree))
            {
                Console.Write(node + " ");
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Generates Fibonacci sequence using yield return
        /// This is lazy evaluation - numbers are generated only when requested
        /// </summary>
        public static IEnumerable<int> GetFibonacci(int count)
        {
            if (count <= 0) yield break; // yield break stops the enumeration

            int a = 0, b = 1;
            
            yield return a; // First Fibonacci number
            if (count == 1) yield break;
            
            yield return b; // Second Fibonacci number
            if (count == 2) yield break;

            // Generate remaining numbers
            for (int i = 2; i < count; i++)
            {
                int next = a + b;
                yield return next;
                a = b;
                b = next;
            }
        }

        /// <summary>
        /// Filters and returns even numbers in a range
        /// Shows how yield can be used for filtering
        /// </summary>
        public static IEnumerable<int> GetEvenNumbers(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                {
                    yield return i;
                }
                // Note: odd numbers are never yielded, so they're effectively filtered out
            }
        }

        /// <summary>
        /// Generates an infinite sequence of powers of 2
        /// Demonstrates infinite sequences with yield
        /// WARNING: This will run forever unless you break out of the loop!
        /// </summary>
        public static IEnumerable<int> GetPowersOfTwo()
        {
            int power = 1;
            while (true) // Infinite loop!
            {
                yield return power;
                power *= 2;
                
                // Safety check to prevent integer overflow
                if (power < 0) yield break;
            }
        }

        /// <summary>
        /// Creates a simple tree structure for demonstration
        /// </summary>
        private static TreeNode CreateSampleTree()
        {
            return new TreeNode("Root",
                new TreeNode("Child1",
                    new TreeNode("Grandchild1"),
                    new TreeNode("Grandchild2")),
                new TreeNode("Child2",
                    new TreeNode("Grandchild3")));
        }

        /// <summary>
        /// Traverses a tree depth-first using yield return
        /// This shows how yield can be used for complex data structure traversal
        /// </summary>
        public static IEnumerable<string> TraverseTreeDepthFirst(TreeNode node)
        {
            if (node == null) yield break;

            // Yield current node
            yield return node.Value;

            // Recursively yield all children
            foreach (TreeNode child in node.Children)
            {
                foreach (string childValue in TraverseTreeDepthFirst(child))
                {
                    yield return childValue;
                }
            }
        }
    }

    /// <summary>
    /// Simple tree node class for demonstration
    /// </summary>
    public class TreeNode
    {
        public string Value { get; }
        public List<TreeNode> Children { get; }

        public TreeNode(string value, params TreeNode[] children)
        {
            Value = value;
            Children = new List<TreeNode>(children);
        }
    }

    /// <summary>
    /// Demonstrates creating a custom enumerable class using yield
    /// This shows how to make any class enumerable
    /// </summary>
    public class NumberRange : IEnumerable<int>
    {
        private readonly int _start;
        private readonly int _end;
        private readonly int _step;

        public NumberRange(int start, int end, int step = 1)
        {
            _start = start;
            _end = end;
            _step = step;
        }

        /// <summary>
        /// Implementation using yield return
        /// This makes the class work with foreach loops
        /// </summary>
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = _start; i <= _end; i += _step)
            {
                yield return i;
            }
        }

        /// <summary>
        /// Non-generic version for backward compatibility
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Additional method that can be chained with LINQ
        /// </summary>
        public IEnumerable<int> GetSquares()
        {
            foreach (int number in this)
            {
                yield return number * number;
            }
        }
    }
}
