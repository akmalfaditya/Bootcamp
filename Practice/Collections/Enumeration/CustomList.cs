using System;
using System.Collections;
using System.Collections.Generic;

namespace Enumeration
{
    /// <summary>
    /// Custom implementation of IList<T> to demonstrate ordered collections with index-based access.
    /// IList<T> extends ICollection<T> and adds:
    /// - Index-based access via this[int index]
    /// - Insert/RemoveAt operations at specific positions
    /// - IndexOf for finding element positions
    /// 
    /// This is the interface that List<T>, Array, and other ordered collections implement.
    /// </summary>
    public class CustomList<T> : IList<T>
    {
        // Internal storage - in a real implementation you might use a dynamic array
        private readonly List<T> _items = new List<T>();

        #region IList<T> Indexer

        /// <summary>
        /// Gets or sets the element at the specified index
        /// This indexer is what allows array-style access: myList[0], myList[1], etc.
        /// </summary>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), 
                        $"Index {index} is out of range. Valid range: 0 to {_items.Count - 1}");
                
                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), 
                        $"Index {index} is out of range. Valid range: 0 to {_items.Count - 1}");
                
                T oldValue = _items[index];
                _items[index] = value;
                Console.WriteLine($"Changed item at index {index} from '{oldValue}' to '{value}'");
            }
        }

        #endregion

        #region ICollection<T> Properties (inherited)

        /// <summary>
        /// Returns the number of elements in the list
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Indicates whether the list is read-only (false for our implementation)
        /// </summary>
        public bool IsReadOnly => false;

        #endregion

        #region IList<T> Methods

        /// <summary>
        /// Returns the zero-based index of the first occurrence of an item
        /// Returns -1 if the item is not found
        /// </summary>
        public int IndexOf(T item)
        {
            int index = _items.IndexOf(item);
            Console.WriteLine($"IndexOf('{item}'): {(index >= 0 ? index.ToString() : "not found")}");
            return index;
        }

        /// <summary>
        /// Inserts an item at the specified index
        /// All items at and after the index are shifted to the right
        /// </summary>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index), 
                    $"Insert index {index} is out of range. Valid range: 0 to {_items.Count}");

            _items.Insert(index, item);
            Console.WriteLine($"Inserted '{item}' at index {index}");
        }

        /// <summary>
        /// Removes the item at the specified index
        /// All items after the index are shifted to the left
        /// </summary>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index), 
                    $"RemoveAt index {index} is out of range. Valid range: 0 to {_items.Count - 1}");

            T removedItem = _items[index];
            _items.RemoveAt(index);
            Console.WriteLine($"Removed '{removedItem}' from index {index}");
        }

        #endregion

        #region ICollection<T> Methods (inherited)

        /// <summary>
        /// Adds an item to the end of the list
        /// </summary>
        public void Add(T item)
        {
            _items.Add(item);
            Console.WriteLine($"Added '{item}' to end of list (index {_items.Count - 1})");
        }

        /// <summary>
        /// Removes all items from the list
        /// </summary>
        public void Clear()
        {
            int originalCount = _items.Count;
            _items.Clear();
            Console.WriteLine($"Cleared all {originalCount} items from list");
        }

        /// <summary>
        /// Checks whether the list contains a specific item
        /// </summary>
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copies the list elements to an array, starting at a particular index
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
        /// Removes the first occurrence of a specific item from the list
        /// Returns true if the item was found and removed, false otherwise
        /// </summary>
        public bool Remove(T item)
        {
            int index = _items.IndexOf(item);
            if (index >= 0)
            {
                _items.RemoveAt(index);
                Console.WriteLine($"Removed '{item}' from index {index}");
                return true;
            }
            else
            {
                Console.WriteLine($"'{item}' not found in list");
                return false;
            }
        }

        #endregion

        #region IEnumerable<T> Implementation (inherited)

        /// <summary>
        /// Returns a generic enumerator that iterates through the list
        /// This enables foreach loops and LINQ operations
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Returns a non-generic enumerator (for backward compatibility)
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Additional Utility Methods

        /// <summary>
        /// Adds multiple items to the end of the list
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
        /// Finds the index of the last occurrence of an item
        /// Returns -1 if not found
        /// </summary>
        public int LastIndexOf(T item)
        {
            // Search from the end backwards
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (EqualityComparer<T>.Default.Equals(_items[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Reverses the order of elements in the list
        /// </summary>
        public void Reverse()
        {
            _items.Reverse();
            Console.WriteLine("Reversed the order of items in the list");
        }

        /// <summary>
        /// Returns a string representation of the list
        /// </summary>
        public override string ToString()
        {
            return $"CustomList<{typeof(T).Name}> with {Count} items: [{string.Join(", ", _items)}]";
        }

        #endregion
    }
}
