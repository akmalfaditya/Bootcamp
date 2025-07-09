using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Array_Class
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Array Class Comprehensive Demo ===\n");

            // Start with basic array concepts
            BasicArrayDeclarationDemo();
            
            // Show memory representation differences
            MemoryRepresentationDemo();
            
            // Demonstrate array equality concepts
            ArrayEqualityDemo();
            
            // Demonstrate array cloning (shallow vs deep copy)
            ArrayCloningDemo();
            
            // Show array resizing capabilities
            ArrayResizingDemo();
            
            // Different ways to access array elements
            ArrayAccessDemo();
            
            // Multidimensional arrays
            MultidimensionalArrayDemo();
            
            // Array searching and sorting operations
            SearchingAndSortingDemo();
            
            // Various enumeration techniques
            ArrayEnumerationDemo();
            
            // Advanced array operations and best practices
            AdvancedArrayOperationsDemo();
            
            // Advanced array creation techniques
            AdvancedArrayCreationDemo();
            
            // Demonstrate utility methods
            ArrayUtilitiesDemo();
            
            // Performance comparisons and edge cases
            ArrayPerformanceDemo.RunPerformanceComparisons();
            
            Console.WriteLine("\n=== Array Demo Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Demonstrates basic array declaration and initialization patterns
        /// Arrays are reference types, even when containing value types
        /// This covers the fundamental nature of Array as the implicit base class
        /// </summary>
        static void BasicArrayDeclarationDemo()
        {
            Console.WriteLine("1. Basic Array Declaration and Initialization");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Most common way - array literal initialization
            // The CLR implicitly creates a pseudotype that inherits from Array
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Array literal: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Type: {numbers.GetType().Name} (pseudotype inheriting from Array)");
            
            // Explicit size declaration with new keyword
            string[] names = new string[3];
            names[0] = "Alice";
            names[1] = "Bob";
            names[2] = "Charlie";
            Console.WriteLine($"Explicit size: [{string.Join(", ", names)}]");
            
            // Alternative initialization syntax
            double[] prices = new double[] { 19.99, 25.50, 12.75 };
            Console.WriteLine($"Alternative syntax: [{string.Join(", ", prices)}]");
            
            // Array.CreateInstance - demonstrates dynamic array creation
            // This is the underlying mechanism that C# syntax uses
            Array dynamicArray = Array.CreateInstance(typeof(int), 4);
            for (int i = 0; i < dynamicArray.Length; i++)
            {
                dynamicArray.SetValue(i * 10, i);
            }
            Console.WriteLine($"Dynamic creation: [{string.Join(", ", dynamicArray.Cast<int>())}]");
            
            // Prove that all arrays inherit from System.Array
            Console.WriteLine($"\nArray inheritance proof:");
            Console.WriteLine($"numbers is Array: {numbers is Array}");
            Console.WriteLine($"names is Array: {names is Array}");
            Console.WriteLine($"dynamicArray is Array: {dynamicArray is Array}");
            
            // Important: Arrays are reference types!
            int[] original = { 1, 2, 3 };
            int[] reference = original; // This doesn't copy - both point to same array!
            reference[0] = 999;
            Console.WriteLine($"\nReference semantics demonstration:");
            Console.WriteLine($"Original after reference change: [{string.Join(", ", original)}]");
            Console.WriteLine("Notice: Both arrays changed because they reference the same memory!");
            
            // Demonstrate that arrays implement generic interfaces automatically
            Console.WriteLine($"\nAutomatic interface implementation:");
            Console.WriteLine($"numbers implements IList<int>: {numbers is IList<int>}");
            Console.WriteLine($"names implements IEnumerable<string>: {names is IEnumerable<string>}");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows how different types are stored in array memory
        /// Value types store data directly, reference types store pointers
        /// Demonstrates the contiguous memory allocation principle
        /// </summary>
        static void MemoryRepresentationDemo()
        {
            Console.WriteLine("2. Memory Representation and Contiguous Allocation");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Array of value types - data stored directly in contiguous memory
            long[] valueTypeArray = new long[3];
            valueTypeArray[0] = 12345L;
            valueTypeArray[1] = 54321L;
            valueTypeArray[2] = 98765L;
            
            Console.WriteLine("Value Type Array (long[]):");
            Console.WriteLine("Memory layout: [data][data][data] - values stored directly");
            Console.WriteLine($"Each long takes 8 bytes, total array memory: {valueTypeArray.Length * 8} bytes");
            for (int i = 0; i < valueTypeArray.Length; i++)
            {
                Console.WriteLine($"  Index {i}: {valueTypeArray[i]} (stored as actual 64-bit value)");
            }
            
            // Array of reference types - stores references to objects
            StringBuilder[] referenceTypeArray = new StringBuilder[3];
            referenceTypeArray[0] = new StringBuilder("First Builder");
            referenceTypeArray[1] = new StringBuilder("Second Builder");
            referenceTypeArray[2] = new StringBuilder("Third Builder");
            
            Console.WriteLine("\nReference Type Array (StringBuilder[]):");
            Console.WriteLine("Memory layout: [ref][ref][ref] -> [object][object][object]");
            Console.WriteLine($"Each reference takes {IntPtr.Size} bytes on this system");
            for (int i = 0; i < referenceTypeArray.Length; i++)
            {
                Console.WriteLine($"  Index {i}: '{referenceTypeArray[i]}' (stored as reference)");
                Console.WriteLine($"    Object hash: {referenceTypeArray[i].GetHashCode()}");
            }
            
            // Demonstrate the efficiency of contiguous memory
            Console.WriteLine("\nContiguous Memory Benefits:");
            Console.WriteLine("- O(1) indexing: array[index] = base_address + (index * element_size)");
            Console.WriteLine("- CPU cache friendly: accessing adjacent elements is very fast");
            Console.WriteLine("- Memory locality: better for modern processor architectures");
            
            // Show that null references can be stored
            referenceTypeArray[1] = null!; // This is allowed for reference types
            Console.WriteLine($"\nAfter setting index 1 to null:");
            for (int i = 0; i < referenceTypeArray.Length; i++)
            {
                var value = referenceTypeArray[i];
                Console.WriteLine($"  Index {i}: {value?.ToString() ?? "null"}");
            }
            
            Console.WriteLine("\nKey Point: Reference types in arrays share objects when assigned!");
            Console.WriteLine("This is why shallow copying can be tricky with reference types.\n");
        }

        /// <summary>
        /// Demonstrates array cloning - shallow copy behavior
        /// Critical to understand the difference between copying array structure vs content
        /// Shows why Clone() is insufficient for reference types
        /// </summary>
        static void ArrayCloningDemo()
        {
            Console.WriteLine("4. Array Cloning and Copy Behavior");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Clone with value types - works as expected
            int[] originalNumbers = { 10, 20, 30 };
            int[] clonedNumbers = (int[])originalNumbers.Clone();
            
            Console.WriteLine("Cloning Value Type Array:");
            Console.WriteLine($"Original: [{string.Join(", ", originalNumbers)}]");
            Console.WriteLine($"Cloned:   [{string.Join(", ", clonedNumbers)}]");
            Console.WriteLine($"Same reference? {ReferenceEquals(originalNumbers, clonedNumbers)}");
            
            // Modify cloned array
            clonedNumbers[0] = 999;
            Console.WriteLine("After modifying cloned array:");
            Console.WriteLine($"Original: [{string.Join(", ", originalNumbers)}] (unchanged)");
            Console.WriteLine($"Cloned:   [{string.Join(", ", clonedNumbers)}] (changed)");
            
            // Clone with reference types - shallow copy behavior
            StringBuilder[] originalBuilders = new StringBuilder[3];
            originalBuilders[0] = new StringBuilder("Builder A");
            originalBuilders[1] = new StringBuilder("Builder B");
            originalBuilders[2] = new StringBuilder("Builder C");
            
            StringBuilder[] clonedBuilders = (StringBuilder[])originalBuilders.Clone();
            
            Console.WriteLine("\nCloning Reference Type Array (Shallow Copy):");
            Console.WriteLine($"Same array reference? {ReferenceEquals(originalBuilders, clonedBuilders)}");
            Console.WriteLine($"Same object references? {ReferenceEquals(originalBuilders[0], clonedBuilders[0])}");
            
            Console.WriteLine("Original builders:");
            for (int i = 0; i < originalBuilders.Length; i++)
            {
                Console.WriteLine($"  [{i}]: '{originalBuilders[i]}'");
            }
            
            // Modify the content of objects in cloned array
            clonedBuilders[0].Append(" - Modified!");
            
            Console.WriteLine("\nAfter modifying content through cloned array:");
            Console.WriteLine("Original builders (also changed!):");
            for (int i = 0; i < originalBuilders.Length; i++)
            {
                Console.WriteLine($"  [{i}]: '{originalBuilders[i]}'");
            }
            
            Console.WriteLine("\nKey Learning: Clone() creates new array but same object references!");
            Console.WriteLine("For true deep copy, you need to clone each object individually.");
            
            // Demonstrate other copying methods
            Console.WriteLine("\nOther Array Copying Methods:");
            
            // CopyTo method
            int[] sourceArray = { 1, 2, 3, 4, 5 };
            int[] destinationArray = new int[8];
            sourceArray.CopyTo(destinationArray, 2); // Copy starting at index 2
            Console.WriteLine($"CopyTo result: [{string.Join(", ", destinationArray)}]");
            
            // Array.Copy static method
            int[] copyResult = new int[3];
            Array.Copy(sourceArray, 1, copyResult, 0, 3); // Copy 3 elements starting from index 1
            Console.WriteLine($"Array.Copy result: [{string.Join(", ", copyResult)}]");
            
            // ConstrainedCopy - atomic operation
            try
            {
                int[] constrainedResult = new int[5];
                Array.ConstrainedCopy(sourceArray, 0, constrainedResult, 0, 5);
                Console.WriteLine($"ConstrainedCopy result: [{string.Join(", ", constrainedResult)}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ConstrainedCopy failed: {ex.Message}");
            }
            
            // Demonstrate deep copy approach
            Console.WriteLine("\nDeep Copy Implementation:");
            StringBuilder[] deepCopy = new StringBuilder[originalBuilders.Length];
            for (int i = 0; i < originalBuilders.Length; i++)
            {
                // Create new StringBuilder with same content
                deepCopy[i] = new StringBuilder(originalBuilders[i].ToString());
            }
            
            // Modify deep copy
            deepCopy[0].Append(" - Deep Copy Modification");
            Console.WriteLine("After deep copy modification:");
            Console.WriteLine($"Original[0]: '{originalBuilders[0]}'");
            Console.WriteLine($"Deep copy[0]: '{deepCopy[0]}'");
            Console.WriteLine("Deep copy: manually copy each object's content.\n");
        }

        /// <summary>
        /// Shows how Array.Resize works and its implications
        /// Important: Resize creates new array, doesn't modify existing one
        /// </summary>
        static void ArrayResizingDemo()
        {
            Console.WriteLine("4. Array Resizing with Array.Resize");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Start with a small array
            int[] numbers = { 1, 2, 3 };
            int[] originalReference = numbers; // Keep reference to original
            
            Console.WriteLine($"Original array: [{string.Join(", ", numbers)}] (Length: {numbers.Length})");
            Console.WriteLine($"Original reference points to same array: {ReferenceEquals(numbers, originalReference)}");
            
            // Resize to larger array
            Console.WriteLine("\nResizing array from 3 to 6 elements...");
            Array.Resize(ref numbers, 6);
            
            Console.WriteLine($"After resize: [{string.Join(", ", numbers)}] (Length: {numbers.Length})");
            Console.WriteLine($"Is it the same array object? {ReferenceEquals(numbers, originalReference)}");
            Console.WriteLine($"Original reference still: [{string.Join(", ", originalReference)}]");
            
            // Add values to new positions
            numbers[3] = 4;
            numbers[4] = 5;
            numbers[5] = 6;
            Console.WriteLine($"With new values: [{string.Join(", ", numbers)}]");
            
            // Resize to smaller array
            Console.WriteLine("\nResizing down to 4 elements...");
            Array.Resize(ref numbers, 4);
            Console.WriteLine($"After shrinking: [{string.Join(", ", numbers)}] (Length: {numbers.Length})");
            
            Console.WriteLine("\nImportant Notes:");
            Console.WriteLine("- Array.Resize creates a NEW array and copies old elements");
            Console.WriteLine("- Original array references are NOT updated automatically");
            Console.WriteLine("- Use 'ref' parameter to update the variable reference");
            Console.WriteLine("- For frequent resizing, consider List<T> instead of arrays\n");
        }

        /// <summary>
        /// Different ways to access array elements
        /// Shows both direct indexing and Array class methods
        /// Demonstrates GetValue/SetValue for dynamic access
        /// </summary>
        static void ArrayAccessDemo()
        {
            Console.WriteLine("6. Array Element Access Methods");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Standard array with mixed data
            string[] fruits = { "Apple", "Banana", "Cherry", "Date", "Elderberry" };
            
            Console.WriteLine("Direct Index Access (C# syntax):");
            Console.WriteLine($"First fruit: {fruits[0]}");
            Console.WriteLine($"Last fruit: {fruits[fruits.Length - 1]}");
            Console.WriteLine($"Middle fruit: {fruits[fruits.Length / 2]}");
            
            // Using Array class methods for dynamic access
            Console.WriteLine("\nUsing Array Class Methods:");
            Array fruitArray = fruits; // Cast to Array for generic methods
            
            Console.WriteLine($"GetValue(0): {fruitArray.GetValue(0)}");
            Console.WriteLine($"GetValue(2): {fruitArray.GetValue(2)}");
            
            // Set values using Array methods
            fruitArray.SetValue("Dragonfruit", 3);
            Console.WriteLine($"After SetValue(\"Dragonfruit\", 3): {fruits[3]}");
            
            // More dynamic approach with unknown types
            Console.WriteLine("\nDynamic Array Creation and Access:");
            Array dynamicStringArray = Array.CreateInstance(typeof(string), 3);
            dynamicStringArray.SetValue("First", 0);
            dynamicStringArray.SetValue("Second", 1);
            dynamicStringArray.SetValue("Third", 2);
            
            Console.WriteLine("Dynamic array contents:");
            for (int i = 0; i < dynamicStringArray.Length; i++)
            {
                object? value = dynamicStringArray.GetValue(i);
                Console.WriteLine($"  [{i}]: {value} (Type: {value?.GetType().Name ?? "null"})");
            }
            
            // Demonstrate casting compatibility
            Console.WriteLine("\nCasting Compatibility:");
            string[] castArray = (string[])dynamicStringArray; // Valid for zero-based arrays
            Console.WriteLine($"Successfully cast to string[]: {castArray[0]}");
            
            // Array bounds and dimension information
            Console.WriteLine("\nArray Bounds and Dimension Information:");
            Console.WriteLine($"Length: {fruits.Length}");
            Console.WriteLine($"LongLength: {fruits.LongLength}");
            Console.WriteLine($"Rank (dimensions): {fruits.Rank}");
            Console.WriteLine($"Lower bound of dimension 0: {fruits.GetLowerBound(0)}");
            Console.WriteLine($"Upper bound of dimension 0: {fruits.GetUpperBound(0)}");
            Console.WriteLine($"Length of dimension 0: {fruits.GetLength(0)}");
            
            // Array element type information
            Console.WriteLine("\nArray Type Information:");
            Console.WriteLine($"Element type: {fruits.GetType().GetElementType()}");
            Console.WriteLine($"Is fixed size: {((IList)fruits).IsFixedSize}");
            Console.WriteLine($"Is read-only: {((IList)fruits).IsReadOnly}");
            Console.WriteLine($"Is synchronized: {((IList)fruits).IsSynchronized}");
            
            // Demonstrate Array.Clear
            Console.WriteLine("\nArray.Clear demonstration:");
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Before Clear: [{string.Join(", ", numbers)}]");
            Array.Clear(numbers, 1, 3); // Clear 3 elements starting at index 1
            Console.WriteLine($"After Clear(1, 3): [{string.Join(", ", numbers)}]");
            
            // Default value initialization
            Console.WriteLine("\nDefault Value Initialization:");
            bool[] booleans = new bool[3]; // Automatically initialized to false
            Console.WriteLine($"New bool array: [{string.Join(", ", booleans)}]");
            
            DateTime[] dates = new DateTime[2]; // Automatically initialized to DateTime.MinValue
            Console.WriteLine($"New DateTime array: [{string.Join(", ", dates)}]");
            
            string[] strings = new string[2]; // Automatically initialized to null
            Console.WriteLine($"New string array: [{string.Join(", ", strings.Select(s => s ?? "null"))}]");
            
            Console.WriteLine("\nBest Practice Guidelines:");
            Console.WriteLine("- Use direct indexing (array[index]) for known types and compile-time access");
            Console.WriteLine("- Use Array methods (GetValue/SetValue) for dynamic/runtime scenarios");
            Console.WriteLine("- Array.CreateInstance when you need to create arrays of unknown types");
            Console.WriteLine("- Remember that elements are auto-initialized to their default values\n");
        }

        /// <summary>
        /// Demonstrates multidimensional arrays - 2D and 3D examples
        /// Shows different types: rectangular vs jagged arrays
        /// </summary>
        static void MultidimensionalArrayDemo()
        {
            Console.WriteLine("6. Multidimensional Arrays");
            Console.WriteLine("=".PadRight(50, '='));
            
            // 2D Rectangular Array (matrix)
            Console.WriteLine("2D Rectangular Array (Matrix):");
            int[,] matrix = new int[3, 3];
            
            // Fill the matrix
            int value = 1;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    matrix[row, col] = value++;
                }
            }
            
            // Display the matrix
            Console.WriteLine("3x3 Matrix:");
            for (int row = 0; row < 3; row++)
            {
                Console.Write("  ");
                for (int col = 0; col < 3; col++)
                {
                    Console.Write($"{matrix[row, col],3}");
                }
                Console.WriteLine();
            }
            
            // Alternative initialization
            int[,] gameBoard = {
                { 1, 0, 1 },
                { 0, 1, 0 },
                { 1, 0, 1 }
            };
            
            Console.WriteLine("\nGame Board (initialized directly):");
            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                Console.Write("  ");
                for (int j = 0; j < gameBoard.GetLength(1); j++)
                {
                    Console.Write($"{gameBoard[i, j],3}");
                }
                Console.WriteLine();
            }
            
            // 3D Array example
            Console.WriteLine("\n3D Array (Cube):");
            int[,,] cube = new int[2, 2, 2];
            int cubeValue = 1;
            
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        cube[x, y, z] = cubeValue++;
                    }
                }
            }
            
            Console.WriteLine("2x2x2 Cube contents:");
            for (int x = 0; x < 2; x++)
            {
                Console.WriteLine($"  Layer {x}:");
                for (int y = 0; y < 2; y++)
                {
                    Console.Write("    ");
                    for (int z = 0; z < 2; z++)
                    {
                        Console.Write($"{cube[x, y, z],3}");
                    }
                    Console.WriteLine();
                }
            }
            
            // Jagged Arrays (arrays of arrays)
            Console.WriteLine("\nJagged Arrays (Array of Arrays):");
            int[][] jaggedArray = new int[3][];
            jaggedArray[0] = new int[] { 1, 2 };           // 2 elements
            jaggedArray[1] = new int[] { 3, 4, 5, 6 };     // 4 elements
            jaggedArray[2] = new int[] { 7, 8, 9 };        // 3 elements
            
            Console.WriteLine("Jagged array with different row lengths:");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                Console.Write($"  Row {i}: ");
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    Console.Write($"{jaggedArray[i][j]} ");
                }
                Console.WriteLine();
            }
            
            Console.WriteLine("\nKey Differences:");
            Console.WriteLine("- Rectangular arrays: [,] - fixed dimensions, single memory block");
            Console.WriteLine("- Jagged arrays: [][] - variable row lengths, multiple memory blocks\n");
        }

        /// <summary>
        /// Demonstrates searching and sorting operations on arrays
        /// Shows built-in methods: BinarySearch, IndexOf, Find, FindAll, Exists, TrueForAll
        /// Covers both single array and paired array sorting
        /// </summary>
        static void SearchingAndSortingDemo()
        {
            Console.WriteLine("8. Array Searching and Sorting");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Single Array Sorting demonstration
            Console.WriteLine("Single Array Sorting:");
            int[] unsortedNumbers = { 64, 34, 25, 12, 22, 11, 90 };
            Console.WriteLine($"Original: [{string.Join(", ", unsortedNumbers)}]");
            
            // Create copy for sorting
            int[] sortedNumbers = (int[])unsortedNumbers.Clone();
            Array.Sort(sortedNumbers);
            Console.WriteLine($"Sorted:   [{string.Join(", ", sortedNumbers)}]");
            
            // Reverse array elements
            Array.Reverse(sortedNumbers);
            Console.WriteLine($"Reversed: [{string.Join(", ", sortedNumbers)}]");
            
            // Paired Array Sorting - sort two arrays based on keys
            Console.WriteLine("\nPaired Array Sorting:");
            int[] keys = { 3, 1, 4, 1, 5 };
            string[] values = { "three", "one", "four", "ONE", "five" };
            
            Console.WriteLine($"Keys before:   [{string.Join(", ", keys)}]");
            Console.WriteLine($"Values before: [{string.Join(", ", values)}]");
            
            Array.Sort(keys, values); // Sort both arrays based on keys
            
            Console.WriteLine($"Keys after:    [{string.Join(", ", keys)}]");
            Console.WriteLine($"Values after:  [{string.Join(", ", values)}]");
            
            // String array sorting (lexicographic)
            string[] names = { "Charlie", "Alice", "Bob", "David", "Eve" };
            Console.WriteLine($"\nOriginal names: [{string.Join(", ", names)}]");
            Array.Sort(names);
            Console.WriteLine($"Sorted names:   [{string.Join(", ", names)}]");
            
            // Custom sorting with Comparison delegate
            Console.WriteLine("\nCustom Sorting (odd numbers first):");
            int[] customSortNumbers = { 1, 2, 3, 4, 5, 6, 7, 8 };
            Console.WriteLine($"Before: [{string.Join(", ", customSortNumbers)}]");
            
            Array.Sort(customSortNumbers, (x, y) => 
            {
                // Sort odd numbers first, then even numbers
                if (x % 2 != y % 2)
                    return x % 2 == 1 ? -1 : 1; // Odd numbers come first
                return x.CompareTo(y); // Within same parity, sort normally
            });
            Console.WriteLine($"After:  [{string.Join(", ", customSortNumbers)}]");
            
            // Binary Search (requires sorted array)
            Console.WriteLine("\nBinary Search (on sorted array):");
            int[] searchArray = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
            Console.WriteLine($"Search array: [{string.Join(", ", searchArray)}]");
            
            int searchValue = 11;
            int index = Array.BinarySearch(searchArray, searchValue);
            Console.WriteLine($"BinarySearch for {searchValue}: index {index}");
            
            searchValue = 6;
            index = Array.BinarySearch(searchArray, searchValue);
            Console.WriteLine($"BinarySearch for {searchValue}: {(index < 0 ? "not found" : $"index {index}")}");
            if (index < 0)
            {
                Console.WriteLine($"  Would be inserted at index: {~index}");
            }
            
            // Linear search methods for unsorted arrays
            Console.WriteLine("\nLinear Search Methods (work on unsorted arrays):");
            int[] mixedNumbers = { 45, 12, 78, 34, 89, 23, 67 };
            Console.WriteLine($"Mixed array: [{string.Join(", ", mixedNumbers)}]");
            
            // IndexOf and LastIndexOf
            Console.WriteLine($"IndexOf(78): {Array.IndexOf(mixedNumbers, 78)}");
            Console.WriteLine($"IndexOf(99): {Array.IndexOf(mixedNumbers, 99)}"); // Not found = -1
            
            int[] duplicateNumbers = { 1, 2, 3, 2, 4, 2, 5 };
            Console.WriteLine($"Array with duplicates: [{string.Join(", ", duplicateNumbers)}]");
            Console.WriteLine($"IndexOf(2): {Array.IndexOf(duplicateNumbers, 2)}"); // First occurrence
            Console.WriteLine($"LastIndexOf(2): {Array.LastIndexOf(duplicateNumbers, 2)}"); // Last occurrence
            
            // Predicate-based search methods
            Console.WriteLine("\nPredicate-based Search Methods:");
            
            // Find first element matching condition
            int found = Array.Find(mixedNumbers, x => x > 40);
            Console.WriteLine($"First number > 40: {found}");
            
            // Find index of first element matching condition
            int foundIndex = Array.FindIndex(mixedNumbers, x => x > 40);
            Console.WriteLine($"Index of first number > 40: {foundIndex}");
            
            // Find last element matching condition
            int lastFound = Array.FindLast(mixedNumbers, x => x > 40);
            Console.WriteLine($"Last number > 40: {lastFound}");
            
            // Find index of last element matching condition
            int lastFoundIndex = Array.FindLastIndex(mixedNumbers, x => x > 40);
            Console.WriteLine($"Index of last number > 40: {lastFoundIndex}");
            
            // Find all elements matching condition
            int[] allFound = Array.FindAll(mixedNumbers, x => x > 40);
            Console.WriteLine($"All numbers > 40: [{string.Join(", ", allFound)}]");
            
            // Boolean condition checks
            bool hasLarge = Array.Exists(mixedNumbers, x => x > 80);
            Console.WriteLine($"Has any number > 80: {hasLarge}");
            
            bool allPositive = Array.TrueForAll(mixedNumbers, x => x > 0);
            Console.WriteLine($"All numbers positive: {allPositive}");
            
            bool allLarge = Array.TrueForAll(mixedNumbers, x => x > 100);
            Console.WriteLine($"All numbers > 100: {allLarge}");
            
            // Demonstrate with strings and lambda expressions
            Console.WriteLine("\nString Array Predicate Examples:");
            string[] words = { "apple", "banana", "cherry", "date", "elderberry" };
            
            string? longWord = Array.Find(words, w => w.Length > 6);
            Console.WriteLine($"First word longer than 6 chars: {longWord ?? "none found"}");
            
            string[] longWords = Array.FindAll(words, w => w.Length > 5);
            Console.WriteLine($"All words longer than 5 chars: [{string.Join(", ", longWords)}]");
            
            bool hasWordWithA = Array.Exists(words, w => w.Contains('a'));
            Console.WriteLine($"Has word containing 'a': {hasWordWithA}");
            
            bool allStartWithLetter = Array.TrueForAll(words, w => char.IsLetter(w[0]));
            Console.WriteLine($"All words start with letter: {allStartWithLetter}");
            
            Console.WriteLine("\nPerformance Characteristics:");
            Console.WriteLine("- BinarySearch: O(log n) - requires sorted array, very fast");
            Console.WriteLine("- IndexOf/LastIndexOf: O(n) - linear scan, works on unsorted");
            Console.WriteLine("- Find methods: O(n) - linear scan with predicate evaluation");
            Console.WriteLine("- Sort: O(n log n) - efficient quicksort implementation\n");
        }

        /// <summary>
        /// Shows different ways to iterate through arrays
        /// Covers for, foreach, Array.ForEach, and manual enumerators
        /// Demonstrates the IEnumerable implementation of arrays
        /// </summary>
        static void ArrayEnumerationDemo()
        {
            Console.WriteLine("9. Array Enumeration Techniques");
            Console.WriteLine("=".PadRight(50, '='));
            
            string[] colors = { "Red", "Green", "Blue", "Yellow", "Purple" };
            
            // Traditional for loop - gives you index access
            Console.WriteLine("1. Traditional for loop (with index access):");
            for (int i = 0; i < colors.Length; i++)
            {
                Console.WriteLine($"  [{i}]: {colors[i]}");
            }
            
            // foreach loop - cleaner for simple iteration
            // Arrays automatically implement IEnumerable<T>
            Console.WriteLine("\n2. foreach loop (IEnumerable<T> implementation):");
            foreach (string color in colors)
            {
                Console.WriteLine($"  Color: {color}");
            }
            
            // Array.ForEach static method - functional approach
            Console.WriteLine("\n3. Array.ForEach static method:");
            Array.ForEach(colors, color => Console.WriteLine($"  Processing: {color}"));
            
            // Using Array.ForEach with more complex operations
            Console.WriteLine("\n4. Array.ForEach with complex operations:");
            int[] numbers = { 1, 2, 3, 4, 5 };
            Array.ForEach(numbers, number => 
            {
                int square = number * number;
                Console.WriteLine($"  {number}² = {square}");
            });
            
            // Array.ForEach equivalent to LINQ but returns void
            Console.WriteLine("\n5. Array.ForEach vs LINQ comparison:");
            Console.WriteLine("Array.ForEach performs action on each element:");
            var processedCount = 0;
            Array.ForEach(numbers, n => { processedCount++; });
            Console.WriteLine($"  Processed {processedCount} elements");
            
            // LINQ enumeration methods (deferred execution)
            Console.WriteLine("\nLINQ enumeration (deferred execution):");
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            Console.WriteLine($"  Even numbers query created (not executed yet)");
            Console.WriteLine($"  Even numbers: [{string.Join(", ", evenNumbers)}]"); // Now executed
            
            var doubled = numbers.Select(n => n * 2);
            Console.WriteLine($"  Doubled: [{string.Join(", ", doubled)}]");
            
            // Manual enumerator (demonstrates IEnumerable implementation)
            Console.WriteLine("\n6. Manual enumerator (IEnumerable implementation):");
            var enumerator = colors.GetEnumerator();
            Console.WriteLine("  Using manual enumerator:");
            while (enumerator.MoveNext())
            {
                Console.WriteLine($"    Current: {enumerator.Current}");
            }
            
            // IEnumerable interface demonstration
            Console.WriteLine("\n7. IEnumerable interface demonstration:");
            IEnumerable enumerable = colors; // Arrays implement IEnumerable
            IEnumerator nonGenericEnumerator = enumerable.GetEnumerator();
            Console.WriteLine("  Non-generic enumerator:");
            while (nonGenericEnumerator.MoveNext())
            {
                Console.WriteLine($"    Current: {nonGenericEnumerator.Current}");
            }
            
            // Generic IEnumerable<T>
            IEnumerable<string> genericEnumerable = colors;
            Console.WriteLine("  Generic IEnumerable<string>:");
            foreach (string item in genericEnumerable)
            {
                Console.WriteLine($"    Item: {item}");
            }
            
            // Demonstrate that arrays support both IList and IEnumerable
            Console.WriteLine("\n8. Array interface implementations:");
            Console.WriteLine($"colors is IEnumerable: {colors is IEnumerable}");
            Console.WriteLine($"colors is IEnumerable<string>: {colors is IEnumerable<string>}");
            Console.WriteLine($"colors is IList: {colors is IList}");
            Console.WriteLine($"colors is IList<string>: {colors is IList<string>}");
            Console.WriteLine($"colors is ICollection: {colors is ICollection}");
            Console.WriteLine($"colors is ICollection<string>: {colors is ICollection<string>}");
            
            Console.WriteLine("\nPerformance and Usage Guidelines:");
            Console.WriteLine("- for loop: Best for index-based access and reverse iteration");
            Console.WriteLine("- foreach: Best for simple forward iteration (compiler optimized)");
            Console.WriteLine("- Array.ForEach: Good for short operations, functional style");
            Console.WriteLine("- LINQ: Powerful but adds overhead, deferred execution");
            Console.WriteLine("- Manual enumerators: Rarely needed, mainly for custom iteration\n");
        }

        /// <summary>
        /// Advanced array operations and best practices
        /// Covers copying, conversion, resizing, and readonly wrappers
        /// Demonstrates Array.ConvertAll and AsReadOnly functionality
        /// </summary>
        static void AdvancedArrayOperationsDemo()
        {
            Console.WriteLine("10. Advanced Array Operations and Best Practices");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Array copying methods comparison
            Console.WriteLine("Array Copying Methods:");
            int[] source = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Method 1: Array.Copy
            int[] dest1 = new int[5];
            Array.Copy(source, 2, dest1, 0, 5); // Copy 5 elements starting from index 2
            Console.WriteLine($"Array.Copy (elements 2-6): [{string.Join(", ", dest1)}]");
            
            // Method 2: CopyTo instance method
            int[] dest2 = new int[source.Length + 3];
            source.CopyTo(dest2, 3); // Copy all elements starting at index 3
            Console.WriteLine($"CopyTo (at index 3): [{string.Join(", ", dest2)}]");
            
            // Method 3: Buffer.BlockCopy (fastest for primitives)
            int[] dest3 = new int[source.Length];
            Buffer.BlockCopy(source, 0, dest3, 0, source.Length * sizeof(int));
            Console.WriteLine($"Buffer.BlockCopy: [{string.Join(", ", dest3)}]");
            
            // Array.ConvertAll - type conversion
            Console.WriteLine("\nArray Type Conversion with Array.ConvertAll:");
            string[] stringNumbers = { "1", "2", "3", "4", "5" };
            Console.WriteLine($"String array: [{string.Join(", ", stringNumbers)}]");
            
            // Convert using Array.ConvertAll with method group
            int[] convertedNumbers = Array.ConvertAll(stringNumbers, int.Parse);
            Console.WriteLine($"Converted to int: [{string.Join(", ", convertedNumbers)}]");
            
            // Convert with lambda expression
            string[] hexNumbers = Array.ConvertAll(convertedNumbers, x => $"0x{x:X2}");
            Console.WriteLine($"Converted to hex: [{string.Join(", ", hexNumbers)}]");
            
            // Convert floats to integers with rounding
            float[] reals = { 1.3f, 1.5f, 1.8f, 2.2f, 2.7f };
            int[] rounded = Array.ConvertAll(reals, x => (int)Math.Round(x));
            Console.WriteLine($"Float array: [{string.Join(", ", reals)}]");
            Console.WriteLine($"Rounded to int: [{string.Join(", ", rounded)}]");
            
            // Array.Resize demonstration
            Console.WriteLine("\nArray.Resize Operations:");
            int[] resizableArray = { 1, 2, 3 };
            int[] originalReference = resizableArray;
            
            Console.WriteLine($"Original: [{string.Join(", ", resizableArray)}]");
            Console.WriteLine($"Same reference before resize: {ReferenceEquals(resizableArray, originalReference)}");
            
            Array.Resize(ref resizableArray, 6); // Increase size
            resizableArray[3] = 4;
            resizableArray[4] = 5;
            resizableArray[5] = 6;
            
            Console.WriteLine($"After resize to 6: [{string.Join(", ", resizableArray)}]");
            Console.WriteLine($"Same reference after resize: {ReferenceEquals(resizableArray, originalReference)}");
            Console.WriteLine($"Original reference still: [{string.Join(", ", originalReference)}]");
            
            // Array.AsReadOnly - readonly wrapper
            Console.WriteLine("\nArray.AsReadOnly - Readonly Wrapper:");
            int[] mutableArray = { 10, 20, 30, 40, 50 };
            var readOnlyWrapper = Array.AsReadOnly(mutableArray);
            
            Console.WriteLine($"Original array: [{string.Join(", ", mutableArray)}]");
            Console.WriteLine($"ReadOnly wrapper: [{string.Join(", ", readOnlyWrapper)}]");
            Console.WriteLine($"ReadOnly wrapper type: {readOnlyWrapper.GetType().Name}");
            Console.WriteLine($"Is readonly: {((IList)readOnlyWrapper).IsReadOnly}");
            
            // Modify original array - readonly wrapper reflects changes
            mutableArray[0] = 999;
            Console.WriteLine($"After modifying original: [{string.Join(", ", readOnlyWrapper)}]");
            
            // Array clearing and filling
            Console.WriteLine("\nArray Clearing and Filling:");
            int[] workArray = { 1, 2, 3, 4, 5, 6, 7, 8 };
            Console.WriteLine($"Original: [{string.Join(", ", workArray)}]");
            
            // Clear portion of array (sets to default values)
            Array.Clear(workArray, 2, 3); // Clear 3 elements starting at index 2
            Console.WriteLine($"After Clear(2,3): [{string.Join(", ", workArray)}]");
            
            // Fill array with value (C# 2.1+)
            Array.Fill(workArray, 99, 1, 4); // Fill with 99, starting at index 1, count 4
            Console.WriteLine($"After Fill(99,1,4): [{string.Join(", ", workArray)}]");
            
            // Working with Array as IList
            Console.WriteLine("\nArray as IList Interface:");
            string[] fruits = { "Apple", "Banana", "Cherry" };
            IList fruitList = fruits;
            
            Console.WriteLine($"IList.Count: {fruitList.Count}");
            Console.WriteLine($"IList[1]: {fruitList[1]}");
            Console.WriteLine($"IList.Contains(\"Banana\"): {fruitList.Contains("Banana")}");
            Console.WriteLine($"IList.IndexOf(\"Cherry\"): {fruitList.IndexOf("Cherry")}");
            Console.WriteLine($"IList.IsFixedSize: {fruitList.IsFixedSize}");
            Console.WriteLine($"IList.IsReadOnly: {fruitList.IsReadOnly}");
            
            // Array bounds safety demonstration
            Console.WriteLine("\nArray Safety and Bounds Checking:");
            try
            {
                int[] safeArray = { 1, 2, 3 };
                Console.WriteLine($"Valid access [1]: {safeArray[1]}");
                
                // This would throw IndexOutOfRangeException
                Console.WriteLine("Attempting invalid access [5]...");
                // var invalid = safeArray[5]; // Commented out to prevent crash
                Console.WriteLine("Bounds checking prevents invalid access automatically");
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }
            
            // Demonstrate Array.ConstrainedCopy vs Array.Copy
            Console.WriteLine("\nArray.ConstrainedCopy vs Array.Copy:");
            object[] mixedSource = { "string", 123, DateTime.Now };
            object[] mixedDest = new object[3];
            
            try
            {
                Array.ConstrainedCopy(mixedSource, 0, mixedDest, 0, 3);
                Console.WriteLine("ConstrainedCopy succeeded for compatible types");
                Console.WriteLine($"Result: [{string.Join(", ", mixedDest)}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ConstrainedCopy failed: {ex.Message}");
            }
            
            Console.WriteLine("\nPerformance and Best Practice Guidelines:");
            Console.WriteLine("1. Pre-allocate arrays when size is known");
            Console.WriteLine("2. Use Array.Copy/Buffer.BlockCopy for bulk operations");
            Console.WriteLine("3. Consider ArrayPool<T> for temporary arrays (not shown here)");
            Console.WriteLine("4. Use ReadOnlySpan<T> for high-performance scenarios");
            Console.WriteLine("5. Prefer arrays over List<T> for fixed-size collections");
            Console.WriteLine("6. Array.ConvertAll is efficient for type transformations");
            Console.WriteLine("7. Array.AsReadOnly provides protection without copying");
            
            Console.WriteLine("\nKey Architectural Points:");
            Console.WriteLine("- Arrays are reference types with value semantics for elements");
            Console.WriteLine("- Fixed size after creation (use List<T> for dynamic sizing)");
            Console.WriteLine("- Contiguous memory allocation enables O(1) indexing");
            Console.WriteLine("- Rich set of static methods in Array class");
            Console.WriteLine("- Foundation for most other collection types in .NET");
            Console.WriteLine("- Automatic implementation of generic collection interfaces\n");
        }

        /// <summary>
        /// Demonstrates advanced Array.CreateInstance features
        /// Shows non-zero-based arrays and custom bounds (rarely used but educational)
        /// </summary>
        static void AdvancedArrayCreationDemo()
        {
            Console.WriteLine("11. Advanced Array Creation with Array.CreateInstance");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Standard zero-based array creation
            Console.WriteLine("Standard Array Creation:");
            Array standardArray = Array.CreateInstance(typeof(int), 5);
            for (int i = 0; i < standardArray.Length; i++)
            {
                standardArray.SetValue(i * 10, i);
            }
            Console.WriteLine($"Standard array: [{string.Join(", ", standardArray.Cast<int>())}]");
            Console.WriteLine($"Lower bound: {standardArray.GetLowerBound(0)}");
            Console.WriteLine($"Upper bound: {standardArray.GetUpperBound(0)}");
            
            // Non-zero-based array (rarely used, not CLS compliant)
            Console.WriteLine("\nNon-Zero-Based Array (Educational - Not Recommended):");
            try
            {
                // Create array with bounds from 10 to 14 (5 elements)
                Array nonZeroArray = Array.CreateInstance(typeof(string), new int[] { 5 }, new int[] { 10 });
                
                Console.WriteLine($"Array length: {nonZeroArray.Length}");
                Console.WriteLine($"Lower bound: {nonZeroArray.GetLowerBound(0)}");
                Console.WriteLine($"Upper bound: {nonZeroArray.GetUpperBound(0)}");
                
                // Set values using non-zero indices
                for (int i = nonZeroArray.GetLowerBound(0); i <= nonZeroArray.GetUpperBound(0); i++)
                {
                    nonZeroArray.SetValue($"Item{i}", i);
                }
                
                Console.WriteLine("Non-zero array contents:");
                for (int i = nonZeroArray.GetLowerBound(0); i <= nonZeroArray.GetUpperBound(0); i++)
                {
                    Console.WriteLine($"  Index {i}: {nonZeroArray.GetValue(i)}");
                }
                
                // Cannot cast to C# array syntax
                Console.WriteLine("Cannot cast to C# array - not CLS compliant");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Non-zero array error: {ex.Message}");
            }
            
            // Multi-dimensional array creation
            Console.WriteLine("\nMulti-dimensional Array Creation:");
            
            // 2D array with Array.CreateInstance
            Array matrix2D = Array.CreateInstance(typeof(double), 3, 4); // 3x4 matrix
            
            Console.WriteLine($"2D Array - Rank: {matrix2D.Rank}");
            Console.WriteLine($"Dimension 0 length: {matrix2D.GetLength(0)}");
            Console.WriteLine($"Dimension 1 length: {matrix2D.GetLength(1)}");
            Console.WriteLine($"Total length: {matrix2D.Length}");
            
            // Fill 2D array
            for (int i = 0; i < matrix2D.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2D.GetLength(1); j++)
                {
                    matrix2D.SetValue(i * 10 + j, i, j);
                }
            }
            
            // Display 2D array
            Console.WriteLine("2D Array contents:");
            for (int i = 0; i < matrix2D.GetLength(0); i++)
            {
                Console.Write("  ");
                for (int j = 0; j < matrix2D.GetLength(1); j++)
                {
                    Console.Write($"{matrix2D.GetValue(i, j),4}");
                }
                Console.WriteLine();
            }
            
            // 3D array creation
            Array cube3D = Array.CreateInstance(typeof(int), 2, 2, 2); // 2x2x2 cube
            Console.WriteLine($"\n3D Array - Rank: {cube3D.Rank}, Total elements: {cube3D.Length}");
            
            // Type information
            Console.WriteLine("\nArray Type Information:");
            Console.WriteLine($"Standard array type: {standardArray.GetType()}");
            Console.WriteLine($"2D array type: {matrix2D.GetType()}");
            Console.WriteLine($"3D array type: {cube3D.GetType()}");
            
            // Element type vs array type
            Console.WriteLine($"Element type of int[]: {typeof(int[]).GetElementType()}");
            Console.WriteLine($"Element type of double[,]: {typeof(double[,]).GetElementType()}");
            
            Console.WriteLine("\nKey Points about Array.CreateInstance:");
            Console.WriteLine("- Allows runtime type specification");
            Console.WriteLine("- Supports custom lower bounds (not recommended)");
            Console.WriteLine("- Can create multi-dimensional arrays dynamically");
            Console.WriteLine("- Non-zero-based arrays are not CLS compliant");
            Console.WriteLine("- Use C# syntax when types are known at compile time\n");
        }

        /// <summary>
        /// Demonstrates array equality - referential vs structural
        /// Critical concept: arrays are reference types, so == checks references, not content
        /// </summary>
        static void ArrayEqualityDemo()
        {
            Console.WriteLine("3.5. Array Equality - Referential vs Structural");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Create two arrays with identical content
            object[] a1 = { "string", 123, true };
            object[] a2 = { "string", 123, true };
            
            Console.WriteLine("Two arrays with identical content:");
            Console.WriteLine($"a1: [{string.Join(", ", a1)}]");
            Console.WriteLine($"a2: [{string.Join(", ", a2)}]");
            
            // Referential equality checks
            Console.WriteLine("\nReferential Equality (default behavior):");
            Console.WriteLine($"a1 == a2: {a1 == a2}"); // False - different array instances
            Console.WriteLine($"a1.Equals(a2): {a1.Equals(a2)}"); // False - object.Equals checks references
            Console.WriteLine($"ReferenceEquals(a1, a2): {ReferenceEquals(a1, a2)}"); // False
            
            // Same reference comparison
            object[] a3 = a1; // Same reference
            Console.WriteLine($"\nSame reference comparison:");
            Console.WriteLine($"a1 == a3: {a1 == a3}"); // True - same reference
            Console.WriteLine($"ReferenceEquals(a1, a3): {ReferenceEquals(a1, a3)}"); // True
            
            // Structural equality using IStructuralEquatable
            Console.WriteLine("\nStructural Equality (content comparison):");
            IStructuralEquatable se1 = a1;
            bool structurallyEqual = se1.Equals(a2, StructuralComparisons.StructuralEqualityComparer);
            Console.WriteLine($"Structural comparison: {structurallyEqual}"); // True - same content
            
            // Demonstrate with nested arrays
            object[] nested1 = { new int[] { 1, 2 }, "test" };
            object[] nested2 = { new int[] { 1, 2 }, "test" };
            
            Console.WriteLine("\nNested arrays comparison:");
            IStructuralEquatable nestedSe1 = nested1;
            bool nestedEqual = nestedSe1.Equals(nested2, StructuralComparisons.StructuralEqualityComparer);
            Console.WriteLine($"Nested structural comparison: {nestedEqual}"); // True - deep content comparison
            
            // Hash code considerations
            Console.WriteLine("\nHash Code Comparison:");
            IStructuralEquatable hashable1 = a1;
            IStructuralEquatable hashable2 = a2;
            int hash1 = hashable1.GetHashCode(StructuralComparisons.StructuralEqualityComparer);
            int hash2 = hashable2.GetHashCode(StructuralComparisons.StructuralEqualityComparer);
            Console.WriteLine($"Structural hash codes equal: {hash1 == hash2}"); // Should be true for equal content
            
            Console.WriteLine("\nKey Takeaways:");
            Console.WriteLine("- Arrays use referential equality by default");
            Console.WriteLine("- Use IStructuralEquatable for content comparison");
            Console.WriteLine("- Important for collections, dictionaries, and comparisons\n");
        }
        
        /// <summary>
        /// Demonstrates advanced array utility methods from our ArrayUtilities class
        /// These show practical real-world array manipulation techniques
        /// </summary>
        static void ArrayUtilitiesDemo()
        {
            Console.WriteLine("12. Advanced Array Utilities Demo");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Test data for demonstrations
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] duplicates = { 1, 2, 2, 3, 3, 3, 4, 5, 5 };
            string[] words = { "apple", "banana", "cherry", "date" };
            
            Console.WriteLine("Original array: [" + string.Join(", ", numbers) + "]");
            
            // Find maximum demonstration
            Console.WriteLine("\n1. Find Maximum:");
            int maxValue = ArrayUtilities.FindMaximum(numbers);
            Console.WriteLine($"Maximum value in array: {maxValue}");
            
            // Array rotation (in-place)
            Console.WriteLine("\n2. Array Rotation:");
            int[] rotationDemo = (int[])numbers.Clone(); // Create copy for demo
            Console.WriteLine($"Before rotation: [{string.Join(", ", rotationDemo)}]");
            ArrayUtilities.RotateLeft(rotationDemo, 3);
            Console.WriteLine($"After rotating left by 3: [{string.Join(", ", rotationDemo)}]");
            
            // Merge arrays
            Console.WriteLine("\n3. Merge Arrays:");
            int[] array1 = { 1, 3, 5 };
            int[] array2 = { 2, 4, 6 };
            int[] merged = ArrayUtilities.MergeArrays(array1, array2);
            Console.WriteLine($"Merged arrays: [{string.Join(", ", merged)}]");
            
            // Array partitioning demonstration
            Console.WriteLine("\n4. Array Partitioning:");
            int[] partitionDemo = { 3, 7, 1, 9, 2, 8, 5 };
            Console.WriteLine($"Before partition: [{string.Join(", ", partitionDemo)}]");
            int pivotIndex = ArrayUtilities.PartitionArray(partitionDemo, 0, partitionDemo.Length - 1);
            Console.WriteLine($"After partition (pivot at {pivotIndex}): [{string.Join(", ", partitionDemo)}]");
            
            // Chunk array
            Console.WriteLine("\n5. Chunk Array:");
            var chunks = ArrayUtilities.ChunkArray(numbers, 3);
            for (int i = 0; i < chunks.Length; i++)
            {
                Console.WriteLine($"Chunk {i + 1}: [{string.Join(", ", chunks[i])}]");
            }
            
            // Flatten nested arrays
            Console.WriteLine("\n6. Flatten Arrays:");
            int[][] nested = { new[] { 1, 2 }, new[] { 3, 4, 5 }, new[] { 6 } };
            int[] flattened = ArrayUtilities.FlattenJaggedArray(nested);
            Console.WriteLine($"Flattened: [{string.Join(", ", flattened)}]");
            
            // Remove duplicates
            Console.WriteLine("\n7. Remove Duplicates:");
            Console.WriteLine($"With duplicates: [{string.Join(", ", duplicates)}]");
            int[] unique = ArrayUtilities.RemoveDuplicates(duplicates);
            Console.WriteLine($"Unique values: [{string.Join(", ", unique)}]");
            
            // Matrix transpose
            Console.WriteLine("\n8. Matrix Transpose:");
            int[,] matrix = { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] transposed = ArrayUtilities.TransposeMatrix(matrix);
            
            Console.WriteLine("Original matrix:");
            ArrayUtilities.Print2DArray(matrix);
            Console.WriteLine("Transposed matrix:");
            ArrayUtilities.Print2DArray(transposed);
            
            // Generic operations with strings and deep copy demo
            Console.WriteLine("\n9. Generic Operations with Strings:");
            string[] rotatedWords = (string[])words.Clone();
            ArrayUtilities.RotateLeft(rotatedWords, 1);
            Console.WriteLine($"Original words: [{string.Join(", ", words)}]");
            Console.WriteLine($"After left rotation: [{string.Join(", ", rotatedWords)}]");
            
            var wordChunks = ArrayUtilities.ChunkArray(words, 2);
            for (int i = 0; i < wordChunks.Length; i++)
            {
                Console.WriteLine($"Word chunk {i + 1}: [{string.Join(", ", wordChunks[i])}]");
            }
            
            // Deep copy demonstration with cloneable objects
            Console.WriteLine("\n10. Deep Copy with Custom Objects:");
            string[] stringArray = { "Hello", "World", "Array", "Demo" };
            // Note: strings are immutable, so clone behavior is equivalent to shallow copy
            Console.WriteLine($"String array operations work with all utility methods");
            string[] mergedStrings = ArrayUtilities.MergeArrays(words, new[] { "extra", "words" });
            Console.WriteLine($"Merged string arrays: [{string.Join(", ", mergedStrings)}]");
            
            Console.WriteLine("\nUtility Methods Summary:");
            Console.WriteLine("- Generic methods work with any type");
            Console.WriteLine("- Focus on immutability (return new arrays)");
            Console.WriteLine("- Handle edge cases gracefully");
            Console.WriteLine("- Demonstrate real-world array manipulation patterns\n");
        }
    }
}
