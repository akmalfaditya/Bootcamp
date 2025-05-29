using System;
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
        /// </summary>
        static void BasicArrayDeclarationDemo()
        {
            Console.WriteLine("1. Basic Array Declaration and Initialization");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Most common way - array literal initialization
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Array literal: [{string.Join(", ", numbers)}]");
            
            // Explicit size declaration with new keyword
            string[] names = new string[3];
            names[0] = "Alice";
            names[1] = "Bob";
            names[2] = "Charlie";
            Console.WriteLine($"Explicit size: [{string.Join(", ", names)}]");
            
            // Alternative initialization syntax
            double[] prices = new double[] { 19.99, 25.50, 12.75 };
            Console.WriteLine($"Alternative syntax: [{string.Join(", ", prices)}]");
            
            // Array.CreateInstance - more dynamic approach
            Array dynamicArray = Array.CreateInstance(typeof(int), 4);
            for (int i = 0; i < dynamicArray.Length; i++)
            {
                dynamicArray.SetValue(i * 10, i);
            }
            Console.WriteLine($"Dynamic creation: [{string.Join(", ", dynamicArray.Cast<int>())}]");
            
            // Important: Arrays are reference types!
            int[] original = { 1, 2, 3 };
            int[] reference = original; // This doesn't copy - both point to same array!
            reference[0] = 999;
            Console.WriteLine($"Original after reference change: [{string.Join(", ", original)}]");
            Console.WriteLine("Notice: Both arrays changed because they reference the same memory!\n");
        }

        /// <summary>
        /// Shows how different types are stored in array memory
        /// Value types store data directly, reference types store pointers
        /// </summary>
        static void MemoryRepresentationDemo()
        {
            Console.WriteLine("2. Memory Representation of Arrays");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Array of value types - data stored directly in array
            long[] valueTypeArray = new long[3];
            valueTypeArray[0] = 12345L;
            valueTypeArray[1] = 54321L;
            valueTypeArray[2] = 98765L;
            
            Console.WriteLine("Value Type Array (long[]):");
            Console.WriteLine("Memory layout: [data][data][data] - values stored directly");
            for (int i = 0; i < valueTypeArray.Length; i++)
            {
                Console.WriteLine($"  Index {i}: {valueTypeArray[i]} (stored as actual value)");
            }
            
            // Array of reference types - stores references to objects
            StringBuilder[] referenceTypeArray = new StringBuilder[3];
            referenceTypeArray[0] = new StringBuilder("First Builder");
            referenceTypeArray[1] = new StringBuilder("Second Builder");
            referenceTypeArray[2] = new StringBuilder("Third Builder");
            
            Console.WriteLine("\nReference Type Array (StringBuilder[]):");
            Console.WriteLine("Memory layout: [ref][ref][ref] -> [object][object][object]");
            for (int i = 0; i < referenceTypeArray.Length; i++)
            {
                Console.WriteLine($"  Index {i}: '{referenceTypeArray[i]}' (stored as reference)");
                Console.WriteLine($"    Object hash: {referenceTypeArray[i].GetHashCode()}");
            }
            
            // Demonstrate the difference when assigning
            referenceTypeArray[3] = referenceTypeArray[0]; // Would share same StringBuilder object
            Console.WriteLine("\nKey Point: Reference types in arrays share objects when assigned!");
            Console.WriteLine("This is why shallow copying can be tricky with reference types.\n");
        }

        /// <summary>
        /// Demonstrates array cloning - shallow copy behavior
        /// Critical to understand the difference between copying array structure vs content
        /// </summary>
        static void ArrayCloningDemo()
        {
            Console.WriteLine("3. Array Cloning (Shallow Copy Behavior)");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Clone with value types - works as expected
            int[] originalNumbers = { 10, 20, 30 };
            int[] clonedNumbers = (int[])originalNumbers.Clone();
            
            Console.WriteLine("Cloning Value Type Array:");
            Console.WriteLine($"Original: [{string.Join(", ", originalNumbers)}]");
            Console.WriteLine($"Cloned:   [{string.Join(", ", clonedNumbers)}]");
            
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
            Console.WriteLine("For true deep copy, you need to clone each object individually.\n");
            
            // Demonstrate deep copy approach
            StringBuilder[] deepCopy = new StringBuilder[originalBuilders.Length];
            for (int i = 0; i < originalBuilders.Length; i++)
            {
                deepCopy[i] = new StringBuilder(originalBuilders[i].ToString());
            }
            Console.WriteLine("Deep copy approach: manually copy each object's content.\n");
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
        /// </summary>
        static void ArrayAccessDemo()
        {
            Console.WriteLine("5. Array Element Access Methods");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Standard array with mixed data
            string[] fruits = { "Apple", "Banana", "Cherry", "Date", "Elderberry" };
            
            Console.WriteLine("Direct Index Access:");
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
            
            // Bounds checking
            Console.WriteLine("\nArray Bounds Information:");
            Console.WriteLine($"Lower bound: {fruits.GetLowerBound(0)}");
            Console.WriteLine($"Upper bound: {fruits.GetUpperBound(0)}");
            Console.WriteLine($"Length: {fruits.Length}");
            Console.WriteLine($"Rank (dimensions): {fruits.Rank}");
            
            Console.WriteLine("\nBest Practice: Use direct indexing for known types,");
            Console.WriteLine("Array methods for generic/dynamic scenarios.\n");
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
        /// Shows both built-in methods and custom approaches
        /// </summary>
        static void SearchingAndSortingDemo()
        {
            Console.WriteLine("7. Array Searching and Sorting");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Sorting demonstration
            Console.WriteLine("Array Sorting:");
            int[] unsortedNumbers = { 64, 34, 25, 12, 22, 11, 90 };
            Console.WriteLine($"Original: [{string.Join(", ", unsortedNumbers)}]");
            
            // Create copy for sorting
            int[] sortedNumbers = (int[])unsortedNumbers.Clone();
            Array.Sort(sortedNumbers);
            Console.WriteLine($"Sorted:   [{string.Join(", ", sortedNumbers)}]");
            
            // Reverse sorting
            Array.Reverse(sortedNumbers);
            Console.WriteLine($"Reversed: [{string.Join(", ", sortedNumbers)}]");
            
            // String array sorting
            string[] names = { "Charlie", "Alice", "Bob", "David", "Eve" };
            Console.WriteLine($"\nOriginal names: [{string.Join(", ", names)}]");
            Array.Sort(names);
            Console.WriteLine($"Sorted names:   [{string.Join(", ", names)}]");
            
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
            
            // Linear search using Array.Find and Array.FindIndex
            Console.WriteLine("\nLinear Search Methods:");
            int[] numbers = { 10, 25, 30, 45, 50, 65, 70 };
            Console.WriteLine($"Array: [{string.Join(", ", numbers)}]");
            
            // Find first element matching condition
            int found = Array.Find(numbers, x => x > 40);
            Console.WriteLine($"First number > 40: {found}");
            
            // Find index of first element matching condition
            int foundIndex = Array.FindIndex(numbers, x => x > 40);
            Console.WriteLine($"Index of first number > 40: {foundIndex}");
            
            // Find all elements matching condition
            int[] allFound = Array.FindAll(numbers, x => x > 40);
            Console.WriteLine($"All numbers > 40: [{string.Join(", ", allFound)}]");
            
            // Check if any/all elements match condition
            bool hasLarge = Array.Exists(numbers, x => x > 60);
            Console.WriteLine($"Has any number > 60: {hasLarge}");
            
            bool allPositive = Array.TrueForAll(numbers, x => x > 0);
            Console.WriteLine($"All numbers positive: {allPositive}");
            
            Console.WriteLine("\nPerformance Note:");
            Console.WriteLine("- BinarySearch: O(log n) - requires sorted array");
            Console.WriteLine("- Linear search: O(n) - works on unsorted arrays\n");
        }

        /// <summary>
        /// Shows different ways to iterate through arrays
        /// Covers for, foreach, and Array.ForEach approaches
        /// </summary>
        static void ArrayEnumerationDemo()
        {
            Console.WriteLine("8. Array Enumeration Techniques");
            Console.WriteLine("=".PadRight(50, '='));
            
            string[] colors = { "Red", "Green", "Blue", "Yellow", "Purple" };
            
            // Traditional for loop - gives you index access
            Console.WriteLine("1. Traditional for loop (with index access):");
            for (int i = 0; i < colors.Length; i++)
            {
                Console.WriteLine($"  [{i}]: {colors[i]}");
            }
            
            // foreach loop - cleaner for simple iteration
            Console.WriteLine("\n2. foreach loop (cleaner syntax):");
            foreach (string color in colors)
            {
                Console.WriteLine($"  Color: {color}");
            }
            
            // Array.ForEach static method
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
            
            // Enumerating with LINQ (extension methods)
            Console.WriteLine("\n5. LINQ enumeration methods:");
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            Console.WriteLine($"  Even numbers: [{string.Join(", ", evenNumbers)}]");
            
            var doubled = numbers.Select(n => n * 2);
            Console.WriteLine($"  Doubled: [{string.Join(", ", doubled)}]");
            
            // Manual enumerator (rarely needed)
            Console.WriteLine("\n6. Manual enumerator (advanced):");
            var enumerator = colors.GetEnumerator();
            Console.WriteLine("  Using manual enumerator:");
            while (enumerator.MoveNext())
            {
                Console.WriteLine($"    Current: {enumerator.Current}");
            }
            
            // Performance considerations
            Console.WriteLine("\nPerformance Tips:");
            Console.WriteLine("- for loop: Best for index-based access");
            Console.WriteLine("- foreach: Best for simple iteration (optimized by compiler)");
            Console.WriteLine("- Array.ForEach: Good for short operations");
            Console.WriteLine("- LINQ: Powerful but adds overhead\n");
        }

        /// <summary>
        /// Advanced array operations and best practices
        /// Covers copying, conversion, and performance considerations
        /// </summary>
        static void AdvancedArrayOperationsDemo()
        {
            Console.WriteLine("9. Advanced Array Operations and Best Practices");
            Console.WriteLine("=".PadRight(50, '='));
            
            // Array copying methods
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
            
            // Array conversion
            Console.WriteLine("\nArray Type Conversion:");
            string[] stringNumbers = { "1", "2", "3", "4", "5" };
            Console.WriteLine($"String array: [{string.Join(", ", stringNumbers)}]");
            
            // Convert using Array.ConvertAll
            int[] convertedNumbers = Array.ConvertAll(stringNumbers, int.Parse);
            Console.WriteLine($"Converted to int: [{string.Join(", ", convertedNumbers)}]");
            
            // Convert back with custom converter
            string[] backToStrings = Array.ConvertAll(convertedNumbers, x => $"#{x:D3}");
            Console.WriteLine($"Custom format: [{string.Join(", ", backToStrings)}]");
            
            // Array clearing and filling
            Console.WriteLine("\nArray Clearing and Filling:");
            int[] workArray = { 1, 2, 3, 4, 5, 6, 7, 8 };
            Console.WriteLine($"Original: [{string.Join(", ", workArray)}]");
            
            // Clear portion of array (sets to default values)
            Array.Clear(workArray, 2, 3); // Clear 3 elements starting at index 2
            Console.WriteLine($"After Clear(2,3): [{string.Join(", ", workArray)}]");
            
            // Fill array with value (C# doesn't have Array.Fill, but we can simulate)
            Array.Fill(workArray, 99, 1, 4); // Fill with 99, starting at index 1, count 4
            Console.WriteLine($"After Fill(99,1,4): [{string.Join(", ", workArray)}]");
            
            // Working with Array as IList
            Console.WriteLine("\nArray as IList Interface:");
            string[] fruits = { "Apple", "Banana", "Cherry" };
            System.Collections.IList fruitList = fruits;
            
            Console.WriteLine($"IList.Count: {fruitList.Count}");
            Console.WriteLine($"IList[1]: {fruitList[1]}");
            Console.WriteLine($"IList.Contains(\"Banana\"): {fruitList.Contains("Banana")}");
            Console.WriteLine($"IList.IndexOf(\"Cherry\"): {fruitList.IndexOf("Cherry")}");
            
            // Performance and memory considerations
            Console.WriteLine("\nPerformance Best Practices:");
            Console.WriteLine("1. Pre-allocate arrays when size is known");
            Console.WriteLine("2. Use Array.Copy for bulk operations");
            Console.WriteLine("3. Consider ArrayPool<T> for temporary arrays");
            Console.WriteLine("4. Use spans for high-performance scenarios");
            Console.WriteLine("5. Prefer arrays over List<T> for fixed-size collections");
            
            // Demonstrate array bounds and safety
            Console.WriteLine("\nArray Safety:");
            try
            {
                int[] safeArray = { 1, 2, 3 };
                Console.WriteLine($"Valid access [1]: {safeArray[1]}");
                
                // This will throw IndexOutOfRangeException
                // Console.WriteLine($"Invalid access [5]: {safeArray[5]}");
                Console.WriteLine("Bounds checking prevents invalid access automatically");
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }
              Console.WriteLine("\nKey Takeaways:");
            Console.WriteLine("- Arrays are reference types with value semantics for elements");
            Console.WriteLine("- Fixed size after creation (use List<T> for dynamic sizing)");
            Console.WriteLine("- Excellent performance for indexed access");
            Console.WriteLine("- Rich set of static methods in Array class");
            Console.WriteLine("- Foundation for most other collection types in .NET\n");
        }

        /// <summary>
        /// Demonstrates advanced array utility methods from our ArrayUtilities class
        /// These show practical real-world array manipulation techniques
        /// </summary>
        static void ArrayUtilitiesDemo()
        {
            Console.WriteLine("=== 9. Advanced Array Utilities Demo ===");
            
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
