using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Enumeration
{
    /// <summary>
    /// Demonstrates read-only collections and interfaces
    /// Read-only collections are important for encapsulation and preventing unwanted modifications
    /// </summary>
    public static class ReadOnlyCollectionDemo
    {
        /// <summary>
        /// Shows different ways to create and use read-only collections
        /// </summary>
        public static void DemonstrateReadOnlyCollections()
        {
            Console.WriteLine("6. Read-Only Collections Demo");
            Console.WriteLine("=".PadRight(40, '='));

            // Create a modifiable list first
            var modifiableList = new List<string> { "Apple", "Banana", "Cherry" };
            
            // Wrap it in a read-only collection
            IReadOnlyList<string> readOnlyList = modifiableList.AsReadOnly();
            
            Console.WriteLine("Read-only list contents:");
            for (int i = 0; i < readOnlyList.Count; i++)
            {
                Console.WriteLine($"[{i}]: {readOnlyList[i]}");
            }
            
            // You can still enumerate over read-only collections
            Console.WriteLine("\nEnumerating read-only collection:");
            foreach (string fruit in readOnlyList)
            {
                Console.WriteLine($"- {fruit}");
            }
            
            // Demonstrate that changes to original list affect read-only view
            Console.WriteLine("\nAdding 'Date' to original list...");
            modifiableList.Add("Date");
            
            Console.WriteLine("Read-only list now contains:");
            foreach (string fruit in readOnlyList)
            {
                Console.WriteLine($"- {fruit}");
            }
            
            // Show ReadOnlyCollection<T>
            var readOnlyCollection = new ReadOnlyCollection<string>(modifiableList);
            Console.WriteLine($"\nReadOnlyCollection Count: {readOnlyCollection.Count}");
            Console.WriteLine($"Contains 'Apple': {readOnlyCollection.Contains("Apple")}");
            
            Console.WriteLine();
        }

        /// <summary>
        /// Shows how to expose collections safely from a class
        /// This is a common pattern in API design
        /// </summary>
        public static void DemonstrateProperCollectionExposure()
        {
            Console.WriteLine("7. Proper Collection Exposure Pattern");
            Console.WriteLine("=".PadRight(40, '='));
            
            var shoppingCart = new ShoppingCart();
            shoppingCart.AddItem("Laptop");
            shoppingCart.AddItem("Mouse");
            shoppingCart.AddItem("Keyboard");
            
            // The Items property returns IReadOnlyList<string>
            // This prevents external code from modifying the internal collection
            IReadOnlyList<string> items = shoppingCart.Items;
            
            Console.WriteLine($"Shopping cart has {items.Count} items:");
            foreach (string item in items)
            {
                Console.WriteLine($"- {item}");
            }
            
            // This would cause a compile error - good!
            // items.Add("Monitor"); // Cannot do this with IReadOnlyList
            
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Example class showing proper encapsulation of collections
    /// This is a common pattern in real-world applications
    /// </summary>
    public class ShoppingCart
    {
        // Private backing field - internal implementation detail
        private readonly List<string> _items = new List<string>();

        /// <summary>
        /// Public read-only access to the items
        /// External code can read and enumerate, but cannot modify
        /// </summary>
        public IReadOnlyList<string> Items => _items.AsReadOnly();

        /// <summary>
        /// Controlled way to add items
        /// The class maintains control over its internal state
        /// </summary>
        public void AddItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                throw new ArgumentException("Item cannot be null or empty", nameof(item));
            
            _items.Add(item);
            Console.WriteLine($"Added '{item}' to shopping cart");
        }

        /// <summary>
        /// Controlled way to remove items
        /// </summary>
        public bool RemoveItem(string item)
        {
            bool removed = _items.Remove(item);
            if (removed)
                Console.WriteLine($"Removed '{item}' from shopping cart");
            else
                Console.WriteLine($"'{item}' not found in shopping cart");
            
            return removed;
        }

        /// <summary>
        /// Clear all items from the cart
        /// </summary>
        public void Clear()
        {
            int count = _items.Count;
            _items.Clear();
            Console.WriteLine($"Cleared {count} items from shopping cart");
        }

        /// <summary>
        /// Get total count of items
        /// </summary>
        public int Count => _items.Count;
    }
}
