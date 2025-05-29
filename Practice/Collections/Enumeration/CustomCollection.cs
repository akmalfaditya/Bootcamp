using System;
using System.Collections;
using System.Collections.Generic;

namespace Enumeration
{
    /// <summary>
    /// This is a custom implementation of ICollection<T> to show how enumeration works.
    /// In real projects, you'd typically use List<T>, but this demonstrates the underlying concepts.
    /// 
    /// ICollection<T> provides:
    /// - Count: number of elements
    /// - Add/Remove: basic modification operations
    /// - Contains: membership testing
    /// - Enumeration: because it extends IEnumerable<T>
    /// </summary>
    public class CustomCollection<T> : ICollection<T>
    {
        // We'll use a List<T> internally to store our items
        // In a real implementation, you might use an array or other data structure
        private readonly List<T> _items = new List<T>();

        #region ICollection<T> Properties

        /// <summary>
        /// Returns the number of elements in the collection
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Indicates whether the collection is read-only
        /// Our implementation allows modifications, so this returns false
        /// </summary>
        public bool IsReadOnly => false;

        #endregion

        #region ICollection<T> Methods

        /// <summary>
        /// Adds an item to the collection
        /// </summary>
        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Cannot add null item to collection");
            
            _items.Add(item);
            Console.WriteLine($"Added '{item}' to collection");
        }

        /// <summary>
        /// Removes all items from the collection
        /// </summary>
        public void Clear()
        {
            int originalCount = _items.Count;
            _items.Clear();
            Console.WriteLine($"Cleared {originalCount} items from collection");
        }

        /// <summary>
        /// Checks whether the collection contains a specific item
        /// </summary>
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copies the collection elements to an array, starting at a particular index
        /// This is useful for interoperability with array-based APIs
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            
            if (array.Length - arrayIndex < _items.Count)
                throw new ArgumentException("Destination array is too small");

            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the collection
        /// Returns true if the item was found and removed, false otherwise
        /// </summary>
        public bool Remove(T item)
        {
            bool removed = _items.Remove(item);
            if (removed)
                Console.WriteLine($"Removed '{item}' from collection");
            else
                Console.WriteLine($"'{item}' not found in collection");
            
            return removed;
        }

        #endregion

        #region IEnumerable<T> Implementation

        /// <summary>
        /// Returns a generic enumerator that iterates through the collection
        /// This is what makes foreach work with our custom collection!
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            // We delegate to our internal List's enumerator
            // In a custom implementation, you might implement your own enumerator logic
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Returns a non-generic enumerator (required by IEnumerable interface)
        /// This is for backward compatibility with older .NET code
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Additional Utility Methods

        /// <summary>
        /// Adds multiple items to the collection at once
        /// This shows how you can extend basic collection functionality
        /// </summary>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (T item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Returns a string representation of the collection
        /// Useful for debugging and display purposes
        /// </summary>
        public override string ToString()
        {
            return $"CustomCollection<{typeof(T).Name}> with {Count} items: [{string.Join(", ", _items)}]";
        }

        #endregion
    }
}
