using System;

namespace Array_Class
{
    /// <summary>
    /// Utility class demonstrating advanced array operations and algorithms
    /// These are real-world examples you might encounter in professional development
    /// </summary>
    public static class ArrayUtilities
    {
        /// <summary>
        /// Demonstrates custom deep copy for arrays containing reference types
        /// This is what you need when Clone() isn't sufficient
        /// </summary>
        public static T[] DeepCopyArray<T>(T[] source) where T : ICloneable
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            T[] copy = new T[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                // Use ICloneable interface to clone each element
                copy[i] = source[i] != null ? (T)source[i].Clone() : default(T);
            }
            return copy;
        }

        /// <summary>
        /// Generic method to find the maximum element in an array
        /// Shows how Array class methods can be extended with generics
        /// </summary>
        public static T FindMaximum<T>(T[] array) where T : IComparable<T>
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array cannot be null or empty");

            T max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i].CompareTo(max) > 0)
                {
                    max = array[i];
                }
            }
            return max;
        }

        /// <summary>
        /// Demonstrates array rotation algorithm
        /// Useful for understanding array manipulation at a deeper level
        /// </summary>
        public static void RotateLeft<T>(T[] array, int positions)
        {
            if (array == null || array.Length <= 1)
                return;

            // Normalize positions to array length
            positions = positions % array.Length;
            if (positions == 0)
                return;

            // Store elements that will be moved
            T[] temp = new T[positions];
            Array.Copy(array, 0, temp, 0, positions);

            // Shift remaining elements left
            Array.Copy(array, positions, array, 0, array.Length - positions);

            // Place stored elements at the end
            Array.Copy(temp, 0, array, array.Length - positions, positions);
        }

        /// <summary>
        /// Efficient array merging algorithm
        /// Shows how to combine multiple arrays efficiently
        /// </summary>
        public static T[] MergeArrays<T>(params T[][] arrays)
        {
            if (arrays == null)
                throw new ArgumentNullException(nameof(arrays));

            // Calculate total length
            int totalLength = 0;
            foreach (var array in arrays)
            {
                if (array != null)
                    totalLength += array.Length;
            }

            // Create result array
            T[] result = new T[totalLength];
            int currentIndex = 0;

            // Copy each array
            foreach (var array in arrays)
            {
                if (array != null)
                {
                    Array.Copy(array, 0, result, currentIndex, array.Length);
                    currentIndex += array.Length;
                }
            }

            return result;
        }

        /// <summary>
        /// Demonstrates custom array partitioning (similar to quicksort partition)
        /// Educational example of how sorting algorithms work with arrays
        /// </summary>
        public static int PartitionArray(int[] array, int low, int high)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            int pivot = array[high]; // Use last element as pivot
            int i = low - 1; // Index of smaller element

            for (int j = low; j < high; j++)
            {
                // If current element is smaller than or equal to pivot
                if (array[j] <= pivot)
                {
                    i++;
                    // Swap elements
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }

            // Place pivot in correct position
            int temp2 = array[i + 1];
            array[i + 1] = array[high];
            array[high] = temp2;

            return i + 1; // Return partition index
        }

        /// <summary>
        /// Demonstrates chunking an array into smaller arrays
        /// Useful for batch processing or pagination scenarios
        /// </summary>
        public static T[][] ChunkArray<T>(T[] source, int chunkSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (chunkSize <= 0)
                throw new ArgumentException("Chunk size must be positive", nameof(chunkSize));

            int chunkCount = (int)Math.Ceiling((double)source.Length / chunkSize);
            T[][] chunks = new T[chunkCount][];

            for (int i = 0; i < chunkCount; i++)
            {
                int startIndex = i * chunkSize;
                int actualChunkSize = Math.Min(chunkSize, source.Length - startIndex);
                
                chunks[i] = new T[actualChunkSize];
                Array.Copy(source, startIndex, chunks[i], 0, actualChunkSize);
            }

            return chunks;
        }

        /// <summary>
        /// Demonstrates flattening a jagged array into a single-dimensional array
        /// Common operation when working with multidimensional data
        /// </summary>
        public static T[] FlattenJaggedArray<T>(T[][] jaggedArray)
        {
            if (jaggedArray == null)
                throw new ArgumentNullException(nameof(jaggedArray));

            // Calculate total length
            int totalLength = 0;
            foreach (var subArray in jaggedArray)
            {
                if (subArray != null)
                    totalLength += subArray.Length;
            }

            // Create flattened array
            T[] flattened = new T[totalLength];
            int currentIndex = 0;

            // Copy all sub-arrays
            foreach (var subArray in jaggedArray)
            {
                if (subArray != null)
                {
                    Array.Copy(subArray, 0, flattened, currentIndex, subArray.Length);
                    currentIndex += subArray.Length;
                }
            }

            return flattened;
        }

        /// <summary>
        /// Demonstrates removing duplicates from an array while preserving order
        /// Shows practical array manipulation techniques
        /// </summary>
        public static T[] RemoveDuplicates<T>(T[] source) where T : IEquatable<T>
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // Use a temporary list to build result
            var uniqueItems = new System.Collections.Generic.List<T>();

            foreach (T item in source)
            {
                if (!uniqueItems.Contains(item))
                {
                    uniqueItems.Add(item);
                }
            }

            return uniqueItems.ToArray();
        }

        /// <summary>
        /// Demonstrates matrix transposition for 2D arrays
        /// Educational example of multidimensional array manipulation
        /// </summary>
        public static T[,] TransposeMatrix<T>(T[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            T[,] transposed = new T[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    transposed[j, i] = matrix[i, j];
                }
            }

            return transposed;
        }

        /// <summary>
        /// Utility method to print 2D arrays in a readable format
        /// Helpful for debugging and visualization
        /// </summary>
        public static void Print2DArray<T>(T[,] array, string title = "2D Array")
        {
            if (array == null)
            {
                Console.WriteLine($"{title}: null");
                return;
            }

            Console.WriteLine($"{title}:");
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                Console.Write("  ");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{array[i, j],6}");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
