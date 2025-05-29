using System;
using System.Collections.Generic;

namespace Generics
{
    /// <summary>
    /// Demonstrates generic constraints - how to add requirements to your generic types
    /// Constraints make generics more powerful by ensuring the type parameter has certain capabilities
    /// Think of them as "rules" that T must follow to be used with your generic class/method
    /// </summary>
    public static class ConstrainedGenerics
    {
        /// <summary>
        /// The classic constraint example - comparable types only
        /// This constraint ensures T has a CompareTo method so we can compare values
        /// Works with int, string, DateTime, decimal, and any type implementing IComparable<T>
        /// </summary>
        /// <typeparam name="T">Type that can be compared to itself</typeparam>
        /// <param name="first">First value</param>
        /// <param name="second">Second value</param>
        /// <returns>The larger of the two values</returns>
        public static T GetMaximum<T>(T first, T second) where T : IComparable<T>
        {
            Console.WriteLine($"  Comparing {typeof(T).Name} values: {first} vs {second}");
            
            // Because of the constraint, we KNOW T has CompareTo method
            int comparison = first.CompareTo(second);
            T result = comparison > 0 ? first : second;
            
            Console.WriteLine($"  Result: {result}");
            return result;
        }

        /// <summary>
        /// Find minimum in an array - also uses IComparable constraint
        /// This shows how constraints enable you to write algorithms
        /// </summary>
        /// <typeparam name="T">Comparable type</typeparam>
        /// <param name="values">Array of values</param>
        /// <returns>Minimum value in the array</returns>
        public static T FindMinimum<T>(T[] values) where T : IComparable<T>
        {
            if (values.Length == 0)
                throw new ArgumentException("Array cannot be empty");

            T minimum = values[0];
            foreach (T value in values)
            {
                if (value.CompareTo(minimum) < 0)
                {
                    minimum = value;
                }
            }

            return minimum;
        }

        /// <summary>
        /// Generic sorting method using IComparable constraint
        /// This is how the built-in Array.Sort works internally
        /// </summary>
        /// <typeparam name="T">Comparable type</typeparam>
        /// <param name="array">Array to sort</param>
        public static void QuickSort<T>(T[] array) where T : IComparable<T>
        {
            QuickSort(array, 0, array.Length - 1);
        }

        private static void QuickSort<T>(T[] array, int low, int high) where T : IComparable<T>
        {
            if (low < high)
            {
                int partitionIndex = Partition(array, low, high);
                QuickSort(array, low, partitionIndex - 1);
                QuickSort(array, partitionIndex + 1, high);
            }
        }

        private static int Partition<T>(T[] array, int low, int high) where T : IComparable<T>
        {
            T pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (array[j].CompareTo(pivot) <= 0)
                {
                    i++;
                    GenericUtilities.Swap(ref array[i], ref array[j]);
                }
            }

            GenericUtilities.Swap(ref array[i + 1], ref array[high]);
            return i + 1;
        }
    }

    /// <summary>
    /// Generic repository demonstrating the 'class' constraint
    /// The 'where T : class' constraint means T must be a reference type (class, not struct)
    /// This is useful when you need to store null values or use reference equality
    /// </summary>
    /// <typeparam name="T">Must be a reference type</typeparam>
    public class Repository<T> where T : class
    {
        private readonly List<T> items = new List<T>();

        /// <summary>
        /// Add an item to the repository
        /// Because of 'class' constraint, we can safely check for null
        /// </summary>
        /// <param name="item">Item to add (can be null due to class constraint)</param>
        public void Add(T? item)
        {
            if (item != null)  // This null check is only valid because T : class
            {
                items.Add(item);
                Console.WriteLine($"  Added {typeof(T).Name}: {item}");
            }
            else
            {
                Console.WriteLine($"  Attempted to add null {typeof(T).Name} - skipped");
            }
        }

        /// <summary>
        /// Remove an item from the repository
        /// Uses reference equality (default for reference types)
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if removed, false if not found</returns>
        public bool Remove(T item)
        {
            bool removed = items.Remove(item);
            if (removed)
            {
                Console.WriteLine($"  Removed {typeof(T).Name}: {item}");
            }
            return removed;
        }

        /// <summary>
        /// Get all items in the repository
        /// </summary>
        /// <returns>Read-only list of all items</returns>
        public IReadOnlyList<T> GetAll()
        {
            return items.AsReadOnly();
        }

        /// <summary>
        /// Find items matching a predicate
        /// </summary>
        /// <param name="predicate">Condition to match</param>
        /// <returns>Matching items</returns>
        public List<T> FindWhere(Func<T, bool> predicate)
        {
            List<T> results = new List<T>();
            foreach (T item in items)
            {
                if (predicate(item))
                {
                    results.Add(item);
                }
            }
            return results;
        }

        /// <summary>
        /// Show all items in the repository
        /// </summary>
        public void ShowAll()
        {
            Console.WriteLine($"  Repository<{typeof(T).Name}> contains {items.Count} items:");
            foreach (T item in items)
            {
                Console.WriteLine($"    - {item}");
            }
        }
    }

    /// <summary>
    /// Factory class demonstrating the 'new()' constraint
    /// This constraint requires T to have a parameterless constructor
    /// Useful for creating instances without knowing the specific type
    /// </summary>
    /// <typeparam name="T">Type with parameterless constructor</typeparam>
    public class GenericFactory<T> where T : new()
    {
        /// <summary>
        /// Create a new instance of T using the parameterless constructor
        /// This is only possible because of the 'new()' constraint
        /// </summary>
        /// <returns>New instance of T</returns>
        public T CreateInstance()
        {
            Console.WriteLine($"  Creating new instance of {typeof(T).Name}...");
            T instance = new T();  // Only works because of 'new()' constraint
            Console.WriteLine($"  Created: {instance}");
            return instance;
        }

        /// <summary>
        /// Create multiple instances
        /// </summary>
        /// <param name="count">Number of instances to create</param>
        /// <returns>Array of new instances</returns>
        public T[] CreateInstances(int count)
        {
            Console.WriteLine($"  Creating {count} instances of {typeof(T).Name}...");
            T[] instances = new T[count];
            for (int i = 0; i < count; i++)
            {
                instances[i] = new T();
            }
            return instances;
        }

        /// <summary>
        /// Create and initialize instance using a setup action
        /// </summary>
        /// <param name="setup">Action to configure the new instance</param>
        /// <returns>Configured instance</returns>
        public T CreateAndSetup(Action<T> setup)
        {
            T instance = new T();
            setup(instance);
            return instance;
        }
    }

    /// <summary>
    /// Example class for demonstrating multiple constraints
    /// Shows how you can combine different constraint types
    /// </summary>
    /// <typeparam name="T">Must be a reference type that implements IComparable and has parameterless constructor</typeparam>
    public class AdvancedManager<T> where T : class, IComparable<T>, new()
    {
        private readonly List<T> managedItems = new List<T>();

        /// <summary>
        /// Process an item with full constraint capabilities
        /// - Can check for null (class constraint)
        /// - Can compare items (IComparable constraint)  
        /// - Can create new instances (new() constraint)
        /// </summary>
        /// <param name="item">Item to process</param>
        public void ProcessEmployee(T item)
        {
            Console.WriteLine($"  Processing {typeof(T).Name}...");

            // Use class constraint - can check for null
            if (item == null)
            {
                Console.WriteLine("  Item is null, creating new instance...");
                item = new T();  // Use new() constraint
            }

            managedItems.Add(item);

            // Use IComparable constraint - can sort
            managedItems.Sort();  // This works because T : IComparable<T>

            Console.WriteLine($"  Processed and sorted. Total items: {managedItems.Count}");
            ShowAllItems();
        }

        /// <summary>
        /// Create a new item and add it to management
        /// </summary>
        public T CreateAndAdd()
        {
            T newItem = new T();  // new() constraint
            managedItems.Add(newItem);
            managedItems.Sort();  // IComparable<T> constraint
            return newItem;
        }

        /// <summary>
        /// Find the maximum item using comparison
        /// </summary>
        /// <returns>Maximum item, or null if no items</returns>
        public T? GetMaximum()
        {
            if (managedItems.Count == 0) return null;  // class constraint allows null

            T maximum = managedItems[0];
            foreach (T item in managedItems)
            {
                if (item.CompareTo(maximum) > 0)  // IComparable<T> constraint
                {
                    maximum = item;
                }
            }
            return maximum;
        }

        private void ShowAllItems()
        {
            foreach (T item in managedItems)
            {
                Console.WriteLine($"    - {item}");
            }
        }
    }

    /// <summary>
    /// Employee class that satisfies multiple constraints
    /// Implements IComparable for sorting, has parameterless constructor, and is a reference type
    /// </summary>
    public class Employee : IComparable<Employee>
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Parameterless constructor required for 'new()' constraint
        /// </summary>
        public Employee()
        {
            Name = "New Employee";
            Department = "Unassigned";
            HireDate = DateTime.Now;
        }

        public Employee(string name, string department)
        {
            Name = name;
            Department = department;
            HireDate = DateTime.Now;
        }

        /// <summary>
        /// Implementation required for IComparable<Employee> constraint
        /// Sorts by name alphabetically
        /// </summary>
        /// <param name="other">Other employee to compare to</param>
        /// <returns>Comparison result</returns>
        public int CompareTo(Employee? other)
        {
            if (other == null) return 1;
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"{Name} ({Department}) - Hired: {HireDate:yyyy-MM-dd}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Employee other)
            {
                return Name == other.Name && Department == other.Department;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Department);
        }
    }

    /// <summary>
    /// Struct constraint example - requires T to be a value type
    /// This is useful for performance-critical scenarios or when you need value semantics
    /// </summary>
    /// <typeparam name="T">Must be a value type (struct)</typeparam>
    public class ValueTypeProcessor<T> where T : struct
    {
        /// <summary>
        /// Process nullable value types
        /// The struct constraint allows us to use T? (nullable value types)
        /// </summary>
        /// <param name="value">Nullable value to process</param>
        /// <returns>Processed value or default</returns>
        public T ProcessNullable(T? value)
        {
            Console.WriteLine($"  Processing nullable {typeof(T).Name}...");
            
            if (value.HasValue)
            {
                Console.WriteLine($"  Value present: {value.Value}");
                return value.Value;
            }
            else
            {
                T defaultValue = default(T);
                Console.WriteLine($"  No value, using default: {defaultValue}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Compare two value types for equality
        /// Value types have built-in equality comparison
        /// </summary>
        /// <param name="first">First value</param>
        /// <param name="second">Second value</param>
        /// <returns>True if equal</returns>
        public bool AreEqual(T first, T second)
        {
            return EqualityComparer<T>.Default.Equals(first, second);
        }
    }

    /// <summary>
    /// Demonstration of enum constraint (C# 7.3+)
    /// Shows how to work specifically with enumeration types
    /// </summary>
    /// <typeparam name="TEnum">Must be an enum type</typeparam>
    public static class EnumUtilities<TEnum> where TEnum : struct, Enum
    {
        /// <summary>
        /// Get all values of an enum type
        /// </summary>
        /// <returns>Array of all enum values</returns>
        public static TEnum[] GetAllValues()
        {
            return Enum.GetValues<TEnum>();
        }

        /// <summary>
        /// Parse a string to enum value safely
        /// </summary>
        /// <param name="value">String to parse</param>
        /// <param name="result">Parsed enum value if successful</param>
        /// <returns>True if parsing succeeded</returns>
        public static bool TryParse(string value, out TEnum result)
        {
            return Enum.TryParse<TEnum>(value, true, out result);
        }

        /// <summary>
        /// Check if an enum has a specific flag (for flags enums)
        /// </summary>
        /// <param name="value">Enum value to check</param>
        /// <param name="flag">Flag to check for</param>
        /// <returns>True if the flag is set</returns>
        public static bool HasFlag(TEnum value, TEnum flag)
        {
            return value.HasFlag(flag);
        }
    }
}
