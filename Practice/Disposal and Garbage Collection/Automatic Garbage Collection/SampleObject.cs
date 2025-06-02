using System;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// A sample class to demonstrate object lifecycle and garbage collection behavior.
    /// This class includes properties and methods to help visualize how objects
    /// are created, referenced, and collected by the garbage collector.
    /// </summary>
    public class SampleObject
    {
        /// <summary>
        /// The name of this object instance for identification purposes.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Reference to another SampleObject to demonstrate object graphs and reachability.
        /// This allows us to create chains of objects for testing GC behavior.
        /// </summary>
        public SampleObject? Child { get; set; }

        /// <summary>
        /// Some data to make the object take up memory.
        /// This helps demonstrate memory allocation and reclamation.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Creates a new SampleObject with the specified name.
        /// </summary>
        /// <param name="name">The name to identify this object instance</param>
        public SampleObject(string name)
        {
            Name = name;
            // Allocate some memory to make the object's footprint visible
            Data = new byte[1000]; // 1KB of data per object
            
            // Fill with some pattern to ensure the memory is actually used
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = (byte)(i % 256);
            }
        }

        /// <summary>
        /// Finalizer to demonstrate when the garbage collector is cleaning up objects.
        /// Note: In real applications, finalizers should only be used when absolutely necessary
        /// as they add overhead and delay object cleanup.
        /// </summary>
        ~SampleObject()
        {
            // Note: We can't use Console.WriteLine in finalizer reliably
            // as the console might be finalized already
            // This is just for demonstration - avoid finalizers in real code unless needed
        }

        /// <summary>
        /// Override ToString to provide meaningful output when displaying object information.
        /// </summary>
        /// <returns>A string representation of this object</returns>
        public override string ToString()
        {
            return $"SampleObject: {Name} (Data: {Data.Length} bytes)";
        }

        /// <summary>
        /// Creates a deep object hierarchy for testing reachability through object graphs.
        /// This demonstrates how the GC traces through connected objects.
        /// </summary>
        /// <param name="depth">How many levels deep to create the hierarchy</param>
        /// <param name="baseName">Base name for the objects in the hierarchy</param>
        /// <returns>The root object of the hierarchy</returns>
        public static SampleObject CreateHierarchy(int depth, string baseName)
        {
            var root = new SampleObject($"{baseName}_Root");
            var current = root;

            for (int i = 1; i < depth; i++)
            {
                current.Child = new SampleObject($"{baseName}_Level_{i}");
                current = current.Child;
            }

            return root;
        }

        /// <summary>
        /// Gets the total count of objects in this hierarchy by following Child references.
        /// Useful for verifying object creation and traversal.
        /// </summary>
        /// <returns>The number of objects reachable from this object</returns>
        public int GetHierarchyCount()
        {
            int count = 1; // Count this object
            var current = Child;

            while (current != null)
            {
                count++;
                current = current.Child;
            }

            return count;
        }
    }
}
