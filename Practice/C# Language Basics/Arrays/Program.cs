
// Complete guide to arrays in C# - everything you need to know
// This program demonstrates all array concepts from the fundamentals to advanced features
// We'll cover: declaration, initialization, indexing, multidimensional arrays,
// indices & ranges, bounds checking, and practical real-world examples

using System;

namespace ArraysDemo
{
    // Simple Point class to demonstrate reference types in arrays
    // We'll use this to show the difference between value and reference type arrays
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public Point() : this(0, 0) { }
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public override string ToString() => $"({X}, {Y})";
    }

    class Program
    {
        // Main method - your guided tour through array mastery
        // Each demonstration builds on the previous one, so follow along step by step
        static void Main()
        {
            Console.WriteLine("=== ARRAYS IN C# - COMPLETE MASTERY GUIDE ===\n");

            // Foundation concepts - you must understand these before moving forward
            DemonstrateArrayDeclaration();
            DemonstrateArrayInitialization();
            DemonstrateArrayAccess();
            
            // Default behavior - critical to understand for avoiding bugs
            DemonstrateDefaultValues();
            DemonstrateValueVsReferenceTypes();
            
            // Modern C# features - these will make your code cleaner and more readable
            DemonstrateIndicesAndRanges();
            
            // Multidimensional arrays - essential for matrix operations and game boards
            DemonstrateRectangularArrays();
            DemonstrateJaggedArrays();
            
            // Shortcuts and best practices - write less code, achieve more
            DemonstrateInitializationShortcuts();
            
            // Safety features - understanding bounds checking prevents runtime crashes
            DemonstrateBoundsChecking();
            
            // Real-world applications - putting it all together
            DemonstratePracticalExamples();

            Console.WriteLine("\n=== CONGRATULATIONS! YOU'VE MASTERED ARRAYS ===");
        }

        // Array declaration fundamentals
        // Think of arrays as containers - you need to specify what type of items you'll store
        static void DemonstrateArrayDeclaration()
        {
            Console.WriteLine("1. ARRAY DECLARATION FUNDAMENTALS");
            Console.WriteLine("==================================");

            // Basic declaration syntax: type[] arrayName = new type[size];
            // The square brackets [] tell C# this is an array type
            char[] vowels = new char[5];  // Creates space for 5 characters
            int[] numbers = new int[10];  // Creates space for 10 integers
            string[] names = new string[3];  // Creates space for 3 string references

            Console.WriteLine($"Created vowels array with {vowels.Length} elements");
            Console.WriteLine($"Created numbers array with {numbers.Length} elements");
            Console.WriteLine($"Created names array with {names.Length} elements");

            // Key insight: At this point, arrays exist but contain default values
            Console.WriteLine($"Default char value: '{vowels[0]}' (appears empty but it's \\0)");
            Console.WriteLine($"Default int value: {numbers[0]}");
            Console.WriteLine($"Default string value: {names[0] ?? "null"}");

            // Pro tip: Always remember that Length gives you the total capacity,
            // not the number of meaningful elements you've added
            Console.WriteLine($"\nImportant: Length = {vowels.Length} means indices 0 to {vowels.Length - 1}");

            Console.WriteLine();
        }

        // Array initialization - putting actual data into your arrays
        // There are several ways to do this, each with its own use case
        static void DemonstrateArrayInitialization()
        {
            Console.WriteLine("2. ARRAY INITIALIZATION TECHNIQUES");
            Console.WriteLine("===================================");

            // Method 1: Initialize with specific values (most common)
            // Use this when you know exactly what data you want to store
            char[] vowels = {'a', 'e', 'i', 'o', 'u'};
            Console.WriteLine("Method 1 - Direct initialization:");
            Console.WriteLine($"Vowels: {string.Join(", ", vowels)}");

            // Method 2: C# 12 collection expressions (modern syntax)
            // This is the newest way - cleaner and more flexible
            char[] consonants = ['b', 'c', 'd', 'f', 'g'];
            Console.WriteLine($"Consonants (C# 12 syntax): {string.Join(", ", consonants)}");

            // Method 3: Declare first, then assign elements individually
            // Use this when you need to calculate or input values dynamically
            int[] fibonacci = new int[7];
            fibonacci[0] = 0;
            fibonacci[1] = 1;
            // Generate fibonacci sequence
            for (int i = 2; i < fibonacci.Length; i++)
            {
                fibonacci[i] = fibonacci[i-1] + fibonacci[i-2];
            }
            Console.WriteLine($"Fibonacci sequence: {string.Join(", ", fibonacci)}");

            // Method 4: Array.Fill for identical values
            // Perfect for initialization scenarios
            int[] scores = new int[5];
            Array.Fill(scores, 100);  // Set all elements to 100
            Console.WriteLine($"All perfect scores: {string.Join(", ", scores)}");

            // Method 5: Using loops for pattern-based initialization
            double[] powers = new double[6];
            for (int i = 0; i < powers.Length; i++)
            {
                powers[i] = Math.Pow(2, i);  // Powers of 2: 1, 2, 4, 8, 16, 32
            }
            Console.WriteLine($"Powers of 2: {string.Join(", ", powers)}");

            Console.WriteLine();
        }

        // Array access patterns - the bread and butter of array operations
        // Master these patterns and you'll handle 90% of array tasks
        static void DemonstrateArrayAccess()
        {
            Console.WriteLine("3. ARRAY ACCESS PATTERNS");
            Console.WriteLine("=========================");

            string[] cities = {"New York", "London", "Tokyo", "Sydney", "Mumbai"};
            
            Console.WriteLine("Our cities array:");
            for (int i = 0; i < cities.Length; i++)
            {
                Console.WriteLine($"Index {i}: {cities[i]}");
            }

            // Pattern 1: Forward iteration (most common)
            Console.WriteLine("\nPattern 1 - Forward iteration:");
            for (int i = 0; i < cities.Length; i++)
            {
                Console.Write($"{cities[i]} ");
            }
            Console.WriteLine();

            // Pattern 2: Reverse iteration
            Console.WriteLine("Pattern 2 - Reverse iteration:");
            for (int i = cities.Length - 1; i >= 0; i--)
            {
                Console.Write($"{cities[i]} ");
            }
            Console.WriteLine();

            // Pattern 3: Enhanced for loop (foreach)
            // Use this when you don't need the index
            Console.WriteLine("Pattern 3 - Foreach (when you don't need index):");
            foreach (string city in cities)
            {
                Console.Write($"{city} ");
            }
            Console.WriteLine();

            // Pattern 4: Conditional access
            Console.WriteLine("\nPattern 4 - Finding specific elements:");
            string searchCity = "Tokyo";
            for (int i = 0; i < cities.Length; i++)
            {
                if (cities[i] == searchCity)
                {
                    Console.WriteLine($"Found {searchCity} at index {i}");
                    break;
                }
            }

            // Pattern 5: Modification during iteration
            Console.WriteLine("Pattern 5 - Modifying elements:");
            string[] uppercaseCities = new string[cities.Length];
            for (int i = 0; i < cities.Length; i++)
            {
                uppercaseCities[i] = cities[i].ToUpper();
            }
            Console.WriteLine($"Uppercase: {string.Join(", ", uppercaseCities)}");

            Console.WriteLine();
        }

        // Default values - understanding what happens when you create arrays
        // This knowledge prevents many common beginner mistakes
        static void DemonstrateDefaultValues()
        {
            Console.WriteLine("4. DEFAULT VALUES BEHAVIOR");
            Console.WriteLine("===========================");

            // Value types get zero-equivalent defaults
            Console.WriteLine("Value type defaults:");
            int[] integers = new int[3];
            bool[] booleans = new bool[3];
            char[] characters = new char[3];
            double[] doubles = new double[3];

            Console.WriteLine($"int array: [{string.Join(", ", integers)}]");
            Console.WriteLine($"bool array: [{string.Join(", ", booleans)}]");
            Console.WriteLine($"char array: ['{string.Join("', '", characters)}'] (empty chars are \\0)");
            Console.WriteLine($"double array: [{string.Join(", ", doubles)}]");

            // Reference types get null defaults
            Console.WriteLine("\nReference type defaults:");
            string[] strings = new string[3];
            Point[] points = new Point[3];

            Console.WriteLine("string array contents:");
            for (int i = 0; i < strings.Length; i++)
            {
                Console.WriteLine($"  Index {i}: {strings[i] ?? "null"}");
            }

            Console.WriteLine("Point array contents:");
            for (int i = 0; i < points.Length; i++)
            {
                Console.WriteLine($"  Index {i}: {points[i] ?? null}");
            }

            // Critical insight: you must initialize reference type elements
            Console.WriteLine("\nInitializing reference type elements:");
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point(i, i * 2);
            }

            Console.WriteLine("After initialization:");
            for (int i = 0; i < points.Length; i++)
            {
                Console.WriteLine($"  Index {i}: {points[i]}");
            }

            Console.WriteLine();
        }

        // Value vs reference types in arrays - this is where many developers get confused
        // Understanding this concept is crucial for preventing bugs
        static void DemonstrateValueVsReferenceTypes()
        {
            Console.WriteLine("5. VALUE TYPES vs REFERENCE TYPES IN ARRAYS");
            Console.WriteLine("============================================");

            // Value types: each array element contains the actual value
            Console.WriteLine("Value types - each element stores the actual data:");
            int[] numbers1 = {10, 20, 30};
            int[] numbers2 = new int[numbers1.Length];
            
            // Copying value type array
            for (int i = 0; i < numbers1.Length; i++)
            {
                numbers2[i] = numbers1[i];
            }
            
            Console.WriteLine($"Original: [{string.Join(", ", numbers1)}]");
            Console.WriteLine($"Copy:     [{string.Join(", ", numbers2)}]");
            
            // Modify the copy
            numbers2[0] = 999;
            Console.WriteLine("After modifying copy[0] = 999:");
            Console.WriteLine($"Original: [{string.Join(", ", numbers1)}] (unchanged!)");
            Console.WriteLine($"Copy:     [{string.Join(", ", numbers2)}] (changed)");

            // Reference types: each array element contains a reference to an object
            Console.WriteLine("\nReference types - each element stores a reference:");
            Point[] points1 = {new Point(1, 1), new Point(2, 2), new Point(3, 3)};
            Point[] points2 = new Point[points1.Length];
            
            // Copying reference type array (shallow copy)
            for (int i = 0; i < points1.Length; i++)
            {
                points2[i] = points1[i];  // Copying references, not objects!
            }
            
            Console.WriteLine($"Original: [{points1}]");
            Console.WriteLine($"Copy:     [{points2}]");
            
            // Modify through the copy
            points2[0].X = 999;
            Console.WriteLine("After modifying copy[0].X = 999:");
            Console.WriteLine($"Original: [{points1}] (also changed!)");
            Console.WriteLine($"Copy:     [{points2}] (changed)");
            Console.WriteLine("Both arrays point to the same objects!");

            // To create independent copies of reference types, you need deep copying
            Console.WriteLine("\nCreating independent copies (deep copy):");
            Point[] points3 = new Point[points1.Length];
            for (int i = 0; i < points1.Length; i++)
            {
                points3[i] = new Point(points1[i].X, points1[i].Y);  // New objects
            }
            points3[0].X = 777;
            Console.WriteLine($"Original: [{points1}] (unchanged)");
            Console.WriteLine($"Deep copy: [{points3}] (changed independently)");

            Console.WriteLine();
        }

        // Indices and ranges - modern C# features that make array manipulation elegant
        // These features reduce bugs and make code more readable
        static void DemonstrateIndicesAndRanges()
        {
            Console.WriteLine("6. INDICES AND RANGES (C# 8+)");
            Console.WriteLine("===============================");

            string[] colors = {"Red", "Green", "Blue", "Yellow", "Orange", "Purple", "Pink"};
            Console.WriteLine($"Colors array: [{string.Join(", ", colors)}]");
            Console.WriteLine($"Array length: {colors.Length}, valid indices: 0 to {colors.Length - 1}");

            // Traditional indexing vs modern indices
            Console.WriteLine("\nTraditional vs Modern indexing:");
            Console.WriteLine($"First element - Traditional: colors[0] = {colors[0]}");
            Console.WriteLine($"Last element  - Traditional: colors[{colors.Length - 1}] = {colors[colors.Length - 1]}");
            Console.WriteLine($"Last element  - Modern: colors[^1] = {colors[^1]}");
            Console.WriteLine($"Second to last - Modern: colors[^2] = {colors[^2]}");

            // Index type for more explicit code
            Console.WriteLine("\nUsing Index type:");
            Index first = 0;
            Index last = ^1;
            Index secondToLast = ^2;
            Console.WriteLine($"First: {colors[first]}");
            Console.WriteLine($"Last: {colors[last]}");
            Console.WriteLine($"Second to last: {colors[secondToLast]}");

            // Ranges - extracting subarrays
            Console.WriteLine("\nRange operations:");
            
            // From start
            string[] firstThree = colors[..3];  // Equivalent to colors[0..3]
            Console.WriteLine($"First 3: [{string.Join(", ", firstThree)}]");
            
            // To end
            string[] lastThree = colors[^3..];  // Last 3 elements
            Console.WriteLine($"Last 3: [{string.Join(", ", lastThree)}]");
            
            // Middle section
            string[] middle = colors[2..5];  // Elements at indices 2, 3, 4
            Console.WriteLine($"Middle (indices 2-4): [{string.Join(", ", middle)}]");
            
            // Single element as array
            string[] singleElement = colors[3..4];
            Console.WriteLine($"Single element as array: [{string.Join(", ", singleElement)}]");

            // Range type for reusable ranges
            Console.WriteLine("\nUsing Range type:");
            Range firstHalf = 0..(colors.Length / 2);
            Range secondHalf = (colors.Length / 2)..;
            
            string[] part1 = colors[firstHalf];
            string[] part2 = colors[secondHalf];
            Console.WriteLine($"First half: [{string.Join(", ", part1)}]");
            Console.WriteLine($"Second half: [{string.Join(", ", part2)}]");

            // Practical example: getting all but first and last
            string[] middleElements = colors[1..^1];
            Console.WriteLine($"All except first and last: [{string.Join(", ", middleElements)}]");

            Console.WriteLine();
        }

        // Rectangular arrays - think spreadsheets or game boards
        // Perfect when you need consistent row and column sizes
        static void DemonstrateRectangularArrays()
        {
            Console.WriteLine("7. RECTANGULAR ARRAYS (2D AND BEYOND)");
            Console.WriteLine("======================================");

            // 2D array declaration and initialization
            Console.WriteLine("Creating a 3x3 game board:");
            char[,] ticTacToe = new char[3, 3];
            
            // Initialize with empty spaces
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    ticTacToe[row, col] = '-';
                }
            }

            // Make some moves
            ticTacToe[0, 0] = 'X';  // Top-left
            ticTacToe[1, 1] = 'O';  // Center
            ticTacToe[2, 2] = 'X';  // Bottom-right
            ticTacToe[0, 2] = 'O';  // Top-right

            Console.WriteLine("Game board after some moves:");
            PrintTicTacToeBoard(ticTacToe);

            // Array with initial values
            Console.WriteLine("\nInitialized 2D array (multiplication table):");
            int[,] multiplicationTable = new int[,]
            {
                {1, 2, 3, 4, 5},
                {2, 4, 6, 8, 10},
                {3, 6, 9, 12, 15},
                {4, 8, 12, 16, 20},
                {5, 10, 15, 20, 25}
            };

            PrintMultiplicationTable(multiplicationTable);

            // Array dimensions and lengths
            Console.WriteLine($"\nArray information:");
            Console.WriteLine($"Dimensions: {multiplicationTable.Rank}");
            Console.WriteLine($"Total elements: {multiplicationTable.Length}");
            Console.WriteLine($"Rows: {multiplicationTable.GetLength(0)}");
            Console.WriteLine($"Columns: {multiplicationTable.GetLength(1)}");

            // 3D array example (rare but sometimes useful)
            Console.WriteLine("\n3D Array example (RGB cube):");
            int[,,] rgbCube = new int[2, 2, 3];  // 2x2 pixels, 3 color channels
            
            // Set pixel colors
            rgbCube[0, 0, 0] = 255; rgbCube[0, 0, 1] = 0;   rgbCube[0, 0, 2] = 0;   // Red
            rgbCube[0, 1, 0] = 0;   rgbCube[0, 1, 1] = 255; rgbCube[0, 1, 2] = 0;   // Green
            rgbCube[1, 0, 0] = 0;   rgbCube[1, 0, 1] = 0;   rgbCube[1, 0, 2] = 255; // Blue
            rgbCube[1, 1, 0] = 255; rgbCube[1, 1, 1] = 255; rgbCube[1, 1, 2] = 255; // White

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int r = rgbCube[i, j, 0];
                    int g = rgbCube[i, j, 1];
                    int b = rgbCube[i, j, 2];
                    Console.WriteLine($"Pixel [{i},{j}]: RGB({r}, {g}, {b})");
                }
            }

            Console.WriteLine();
        }

        // Jagged arrays - arrays of arrays with different lengths
        // More flexible than rectangular arrays, perfect for irregular data
        static void DemonstrateJaggedArrays()
        {
            Console.WriteLine("8. JAGGED ARRAYS (ARRAYS OF ARRAYS)");
            Console.WriteLine("====================================");

            // Declaration and manual initialization
            Console.WriteLine("Creating a jagged array manually:");
            int[][] triangularArray = new int[4][];  // 4 rows, lengths to be determined
            
            // Initialize each row with different lengths
            for (int i = 0; i < triangularArray.Length; i++)
            {
                triangularArray[i] = new int[i + 1];  // Row 0: 1 element, Row 1: 2 elements, etc.
                
                // Fill with values
                for (int j = 0; j < triangularArray[i].Length; j++)
                {
                    triangularArray[i][j] = (i + 1) * 10 + (j + 1);
                }
            }

            Console.WriteLine("Triangular array:");
            for (int i = 0; i < triangularArray.Length; i++)
            {
                Console.WriteLine($"Row {i}: [{string.Join(", ", triangularArray[i])}]");
            }

            // Direct initialization with different row lengths
            Console.WriteLine("\nDirect initialization - monthly sales data:");
            int[][] monthlySales = new int[][]
            {
                new int[] {100, 150, 200},           // Q1 - 3 months
                new int[] {180, 220, 190, 250},     // Q2 - 4 months (includes bonus month)
                new int[] {300, 280},               // Q3 - 2 months (partial data)
                new int[] {400, 450, 500, 520, 600} // Q4 - 5 months (includes year-end sales)
            };

            for (int quarter = 0; quarter < monthlySales.Length; quarter++)
            {
                int total = 0;
                foreach (int sales in monthlySales[quarter])
                {
                    total += sales;
                }
                Console.WriteLine($"Q{quarter + 1}: {monthlySales[quarter].Length} months, " +
                                $"Sales: [{string.Join(", ", monthlySales[quarter])}], Total: {total}");
            }

            // Practical example: storing words grouped by length
            Console.WriteLine("\nPractical example - words grouped by length:");
            string[] words = {"C#", "is", "fun", "and", "powerful", "programming", "language"};
            
            // Find maximum word length
            int maxLength = 0;
            foreach (string word in words)
            {
                if (word.Length > maxLength)
                    maxLength = word.Length;
            }

            // Create jagged array to group words by length
            string[][] wordsByLength = new string[maxLength + 1][];
            
            // Count words of each length
            int[] counts = new int[maxLength + 1];
            foreach (string word in words)
            {
                counts[word.Length]++;
            }

            // Initialize inner arrays
            for (int i = 0; i <= maxLength; i++)
            {
                wordsByLength[i] = new string[counts[i]];
            }

            // Reset counts for indexing
            Array.Fill(counts, 0);

            // Fill the arrays
            foreach (string word in words)
            {
                int length = word.Length;
                wordsByLength[length][counts[length]] = word;
                counts[length]++;
            }

            // Display results
            for (int length = 1; length <= maxLength; length++)
            {
                if (wordsByLength[length].Length > 0)
                {
                    Console.WriteLine($"Length {length}: [{string.Join(", ", wordsByLength[length])}]");
                }
            }

            Console.WriteLine();
        }

        // Initialization shortcuts - write less code, be more productive
        // These techniques make your code cleaner and more maintainable
        static void DemonstrateInitializationShortcuts()
        {
            Console.WriteLine("9. INITIALIZATION SHORTCUTS");
            Console.WriteLine("============================");

            // Implicit typing with var
            Console.WriteLine("Using 'var' for type inference:");
            var numbers = new[] {1, 2, 3, 4, 5};  // Compiler infers int[]
            var names = new[] {"Alice", "Bob", "Charlie"};  // Compiler infers string[]
            var mixed = new object[] {1, "hello", 3.14, true};  // Explicit object[] needed for mixed types

            Console.WriteLine($"Numbers: {numbers.GetType().Name} = [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Names: {names.GetType().Name} = [{string.Join(", ", names)}]");
            Console.WriteLine($"Mixed: {mixed.GetType().Name} = [{string.Join(", ", mixed)}]");

            // Multidimensional array shortcuts
            Console.WriteLine("\nMultidimensional shortcuts:");
            var matrix2D = new[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };

            var matrix3D = new[,,] {
                {{1, 2}, {3, 4}},
                {{5, 6}, {7, 8}}
            };

            Console.WriteLine("2D Matrix:");
            for (int i = 0; i < matrix2D.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2D.GetLength(1); j++)
                {
                    Console.Write($"{matrix2D[i, j],3}");
                }
                Console.WriteLine();
            }

            // Collection expressions (C# 12)
            Console.WriteLine("\nCollection expressions (C# 12):");
            int[] modernArray = [10, 20, 30, 40, 50];
            string[] modernStrings = ["Hello", "Modern", "C#"];
            
            Console.WriteLine($"Modern int array: [{string.Join(", ", modernArray)}]");
            Console.WriteLine($"Modern string array: [{string.Join(", ", modernStrings)}]");

            // Spreading arrays (C# 12)
            int[] firstHalf = [1, 2, 3];
            int[] secondHalf = [4, 5, 6];
            int[] combined = [..firstHalf, ..secondHalf];  // Spread operator
            int[] withExtra = [0, ..combined, 7, 8, 9];
            
            Console.WriteLine($"Combined: [{string.Join(", ", combined)}]");
            Console.WriteLine($"With extra: [{string.Join(", ", withExtra)}]");

            Console.WriteLine();
        }

        // Bounds checking - C#'s safety net that prevents memory corruption
        // Understanding this helps you write safer code and debug issues
        static void DemonstrateBoundsChecking()
        {
            Console.WriteLine("10. BOUNDS CHECKING AND SAFETY");
            Console.WriteLine("===============================");

            int[] safeArray = {10, 20, 30, 40, 50};
            Console.WriteLine($"Array: [{string.Join(", ", safeArray)}]");
            Console.WriteLine($"Valid indices: 0 to {safeArray.Length - 1}");

            // Safe array access
            Console.WriteLine("\nSafe array access:");
            for (int i = 0; i < safeArray.Length; i++)
            {
                Console.WriteLine($"safeArray[{i}] = {safeArray[i]}");
            }

            // Demonstrating bounds checking (controlled exception)
            Console.WriteLine("\nDemonstrating bounds checking:");
            try
            {
                Console.WriteLine($"Attempting to access index {safeArray.Length} (out of bounds)...");
                int invalidValue = safeArray[safeArray.Length];  // This will throw
                Console.WriteLine($"Value: {invalidValue}");  // This line won't execute
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
                Console.WriteLine("C# prevented potential memory corruption!");
            }

            // Safe array access patterns
            Console.WriteLine("\nSafe access patterns:");
            
            // Pattern 1: Check bounds explicitly
            int indexToCheck = 10;
            if (indexToCheck >= 0 && indexToCheck < safeArray.Length)
            {
                Console.WriteLine($"safeArray[{indexToCheck}] = {safeArray[indexToCheck]}");
            }
            else
            {
                Console.WriteLine($"Index {indexToCheck} is out of bounds");
            }

            // Pattern 2: Using Array.IndexOf for searching
            int searchValue = 30;
            int foundIndex = Array.IndexOf(safeArray, searchValue);
            if (foundIndex >= 0)
            {
                Console.WriteLine($"Found {searchValue} at index {foundIndex}");
            }
            else
            {
                Console.WriteLine($"{searchValue} not found in array");
            }

            // Pattern 3: Safe enumeration with foreach
            Console.WriteLine("Safe enumeration with foreach:");
            foreach (int value in safeArray)
            {
                Console.Write($"{value} ");
            }
            Console.WriteLine();

            // Performance note about bounds checking
            Console.WriteLine("\nPerformance note:");
            Console.WriteLine("Bounds checking has minimal overhead in modern .NET");
            Console.WriteLine("The safety benefits far outweigh the tiny performance cost");
            Console.WriteLine("Trust the runtime - don't try to disable bounds checking!");

            Console.WriteLine();
        }

        // Real-world practical examples
        // These show how arrays are used in actual applications
        static void DemonstratePracticalExamples()
        {
            Console.WriteLine("11. PRACTICAL REAL-WORLD EXAMPLES");
            Console.WriteLine("==================================");

            // Example 1: Grade management system
            Console.WriteLine("Example 1: Student Grade Management");
            DemonstrateGradeManagement();

            // Example 2: Image processing (simplified)
            Console.WriteLine("\nExample 2: Simple Image Processing");
            DemonstrateImageProcessing();

            // Example 3: Game inventory system
            Console.WriteLine("\nExample 3: Game Inventory System");
            DemonstrateInventorySystem();

            // Example 4: Data analysis
            Console.WriteLine("\nExample 4: Sales Data Analysis");
            DemonstrateSalesAnalysis();

            Console.WriteLine();
        }

        // Helper method: Student grade management
        static void DemonstrateGradeManagement()
        {
            string[] students = {"Alice", "Bob", "Charlie", "Diana", "Eve"};
            double[] grades = {85.5, 92.0, 78.5, 96.0, 88.5};
            char[] letterGrades = new char[students.Length];

            // Calculate letter grades
            for (int i = 0; i < grades.Length; i++)
            {
                letterGrades[i] = grades[i] >= 90 ? 'A' :
                                 grades[i] >= 80 ? 'B' :
                                 grades[i] >= 70 ? 'C' :
                                 grades[i] >= 60 ? 'D' : 'F';
            }

            // Display results
            Console.WriteLine("Student Grade Report:");
            for (int i = 0; i < students.Length; i++)
            {
                Console.WriteLine($"{students[i],-10}: {grades[i],5:F1} ({letterGrades[i]})");
            }

            // Calculate statistics
            double sum = 0;
            double max = grades[0];
            double min = grades[0];

            foreach (double grade in grades)
            {
                sum += grade;
                if (grade > max) max = grade;
                if (grade < min) min = grade;
            }

            double average = sum / grades.Length;
            Console.WriteLine($"\nClass Statistics:");
            Console.WriteLine($"Average: {average:F2}");
            Console.WriteLine($"Highest: {max:F1}");
            Console.WriteLine($"Lowest: {min:F1}");
        }

        // Helper method: Simple image processing
        static void DemonstrateImageProcessing()
        {
            // Simulate a small grayscale image (8x8 pixels)
            byte[,] image = {
                {0, 50, 100, 150, 200, 255, 200, 150},
                {25, 75, 125, 175, 225, 200, 175, 125},
                {50, 100, 150, 200, 250, 175, 150, 100},
                {75, 125, 175, 225, 200, 150, 125, 75},
                {100, 150, 200, 250, 175, 125, 100, 50},
                {125, 175, 225, 200, 150, 100, 75, 25},
                {150, 200, 250, 175, 125, 75, 50, 0},
                {175, 225, 200, 150, 100, 50, 25, 0}
            };

            Console.WriteLine("Original image (grayscale values):");
            PrintImageArray(image);

            // Apply simple brightness adjustment
            byte[,] brightened = new byte[8, 8];
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    int newValue = image[row, col] + 30;
                    brightened[row, col] = (byte)Math.Min(255, newValue);
                }
            }

            Console.WriteLine("Brightened image (+30):");
            PrintImageArray(brightened);
        }

        // Helper method: Game inventory system
        static void DemonstrateInventorySystem()
        {
            string[] itemNames = {"Sword", "Shield", "Potion", "Key", "Gem"};
            int[] itemCounts = {1, 1, 5, 3, 2};
            double[] itemValues = {150.0, 75.0, 25.0, 10.0, 100.0};

            Console.WriteLine("Player Inventory:");
            Console.WriteLine("Item        Count  Value   Total");
            Console.WriteLine("--------------------------------");

            double totalValue = 0;
            for (int i = 0; i < itemNames.Length; i++)
            {
                double itemTotal = itemCounts[i] * itemValues[i];
                totalValue += itemTotal;
                Console.WriteLine($"{itemNames[i],-10} {itemCounts[i],5} {itemValues[i],6:F0} {itemTotal,7:F0}");
            }

            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Total Inventory Value: {totalValue:F0} gold");

            // Find most valuable item
            int mostValuableIndex = 0;
            for (int i = 1; i < itemValues.Length; i++)
            {
                if (itemValues[i] > itemValues[mostValuableIndex])
                {
                    mostValuableIndex = i;
                }
            }

            Console.WriteLine($"Most valuable item: {itemNames[mostValuableIndex]} ({itemValues[mostValuableIndex]:F0} gold)");
        }

        // Helper method: Sales data analysis
        static void DemonstrateSalesAnalysis()
        {
            string[] months = {"Jan", "Feb", "Mar", "Apr", "May", "Jun"};
            double[] sales = {15000, 18000, 22000, 19000, 25000, 28000};

            Console.WriteLine("Monthly Sales Report:");
            for (int i = 0; i < months.Length; i++)
            {
                Console.WriteLine($"{months[i]}: ${sales[i]:N0}");
            }

            // Calculate growth rates
            Console.WriteLine("\nMonth-over-month growth:");
            for (int i = 1; i < sales.Length; i++)
            {
                double growth = ((sales[i] - sales[i-1]) / sales[i-1]) * 100;
                Console.WriteLine($"{months[i-1]} to {months[i]}: {growth:+0.0;-0.0}%");
            }

            // Find best and worst months
            int bestMonth = 0, worstMonth = 0;
            for (int i = 1; i < sales.Length; i++)
            {
                if (sales[i] > sales[bestMonth]) bestMonth = i;
                if (sales[i] < sales[worstMonth]) worstMonth = i;
            }

            Console.WriteLine($"\nBest month: {months[bestMonth]} (${sales[bestMonth]:N0})");
            Console.WriteLine($"Worst month: {months[worstMonth]} (${sales[worstMonth]:N0})");

            // Calculate average
            double total = 0;
            foreach (double sale in sales)
            {
                total += sale;
            }
            double average = total / sales.Length;
            Console.WriteLine($"Average monthly sales: ${average:N0}");
        }

        // Helper method: Print tic-tac-toe board
        static void PrintTicTacToeBoard(char[,] board)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.Write(board[row, col]);
                    if (col < 2) Console.Write("|");
                }
                Console.WriteLine();
                if (row < 2) Console.WriteLine("-----");
            }
        }

        // Helper method: Print multiplication table
        static void PrintMultiplicationTable(int[,] table)
        {
            Console.Write("   ");
            for (int col = 0; col < table.GetLength(1); col++)
            {
                Console.Write($"{col + 1,4}");
            }
            Console.WriteLine();

            for (int row = 0; row < table.GetLength(0); row++)
            {
                Console.Write($"{row + 1,2}:");
                for (int col = 0; col < table.GetLength(1); col++)
                {
                    Console.Write($"{table[row, col],4}");
                }
                Console.WriteLine();
            }
        }

        // Helper method: Print image array
        static void PrintImageArray(byte[,] image)
        {
            for (int row = 0; row < image.GetLength(0); row++)
            {
                for (int col = 0; col < image.GetLength(1); col++)
                {
                    Console.Write($"{image[row, col],4}");
                }
                Console.WriteLine();
            }
        }
    }
}
