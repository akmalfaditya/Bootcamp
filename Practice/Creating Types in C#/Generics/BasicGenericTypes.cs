using System;
using System.Text;

namespace Generics
{
    /// <summary>
    /// Our custom generic stack - the classic example of generics in action
    /// This single class can store ANY type safely and efficiently
    /// No boxing/unboxing, no casting, no runtime errors!
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the stack</typeparam>
    public class CustomStack<T>
    {
        private T[] data;
        private int position;
        private readonly int maxSize;

        public CustomStack(int capacity = 100)
        {
            maxSize = capacity;
            data = new T[maxSize];
            position = 0;
        }

        /// <summary>
        /// Gets the current number of items in the stack
        /// </summary>
        public int Count => position;

        /// <summary>
        /// Checks if the stack is empty
        /// </summary>
        public bool IsEmpty => position == 0;

        /// <summary>
        /// Checks if the stack is full
        /// </summary>
        public bool IsFull => position == maxSize;

        /// <summary>
        /// Push an item onto the stack
        /// Notice how we accept T - whatever type was specified when creating the stack
        /// </summary>
        public void Push(T item)
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Stack overflow! Cannot push more items.");
            }

            data[position++] = item;
            Console.WriteLine($"  Pushed: {item}");
        }

        /// <summary>
        /// Pop an item from the stack
        /// Returns T - the exact same type that was pushed, no casting needed!
        /// </summary>
        public T Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Stack underflow! Cannot pop from empty stack.");
            }

            T item = data[--position];
            // Clear the reference to help GC (important for reference types)
            data[position] = default(T)!;
            
            Console.WriteLine($"  Popped: {item}");
            return item;
        }

        /// <summary>
        /// Peek at the top item without removing it
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot peek at empty stack.");
            }

            return data[position - 1];
        }

        /// <summary>
        /// Show detailed information about the stack's current state
        /// This method works regardless of what type T actually is
        /// </summary>
        public void ShowStackInfo()
        {
            Console.WriteLine($"  Stack<{typeof(T).Name}> - Count: {Count}, Capacity: {maxSize}");
            
            if (!IsEmpty)
            {
                Console.WriteLine($"  Top item: {Peek()}");
                Console.WriteLine("  All items (top to bottom):");
                for (int i = position - 1; i >= 0; i--)
                {
                    Console.WriteLine($"    [{i}]: {data[i]}");
                }
            }
            else
            {
                Console.WriteLine("  Stack is empty");
            }
        }

        /// <summary>
        /// Clear all items from the stack
        /// </summary>
        public void Clear()
        {
            // For reference types, clear references to help GC
            for (int i = 0; i < position; i++)
            {
                data[i] = default(T)!;
            }
            position = 0;
            Console.WriteLine("  Stack cleared");
        }
    }

    /// <summary>
    /// A simple Person class to demonstrate generics with custom types
    /// Nothing special here - just a regular class that can be used with our generic stack
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        /// <summary>
        /// Parameterless constructor for constraints that require 'new()'
        /// </summary>
        public Person() : this("Unknown", 0) { }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return $"{Name} (Age: {Age})";
        }

        /// <summary>
        /// Override Equals for demonstration purposes
        /// This allows our generic search methods to work properly
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Person other)
            {
                return Name == other.Name && Age == other.Age;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Age);
        }
    }

    /// <summary>
    /// Generic list implementation to show more complex generic scenarios
    /// This demonstrates how generics can be used for dynamic collections
    /// </summary>
    /// <typeparam name="T">The type of elements in the list</typeparam>
    public class CustomList<T>
    {
        private T[] items;
        private int count;
        private int capacity;

        public CustomList(int initialCapacity = 4)
        {
            capacity = initialCapacity;
            items = new T[capacity];
            count = 0;
        }

        public int Count => count;
        
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();
                return items[index];
            }
            set
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();
                items[index] = value;
            }
        }

        /// <summary>
        /// Add an item to the list
        /// Automatically grows the internal array when needed
        /// </summary>
        public void Add(T item)
        {
            if (count == capacity)
            {
                Resize();
            }
            
            items[count++] = item;
        }

        /// <summary>
        /// Remove an item from the list
        /// Returns true if the item was found and removed
        /// </summary>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove item at specific index
        /// </summary>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            // Shift elements left to fill the gap
            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }
            
            // Clear the last element and decrement count
            items[--count] = default(T)!;
        }

        /// <summary>
        /// Find the index of an item
        /// Uses the type's Equals method for comparison
        /// </summary>
        public int IndexOf(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(items[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Check if the list contains an item
        /// </summary>
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Private method to resize the internal array
        /// This is why generics are better than object[] - no boxing!
        /// </summary>
        private void Resize()
        {
            capacity *= 2;
            T[] newItems = new T[capacity];
            Array.Copy(items, newItems, count);
            items = newItems;
        }

        /// <summary>
        /// Convert the list to a string representation
        /// </summary>
        public override string ToString()
        {
            if (count == 0) return "[]";

            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < count; i++)
            {
                sb.Append(items[i]);
                if (i < count - 1) sb.Append(", ");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
