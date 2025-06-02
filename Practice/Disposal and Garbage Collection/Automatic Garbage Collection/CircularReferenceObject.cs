using System;

namespace AutomaticGarbageCollection
{
    /// <summary>
    /// A class specifically designed to demonstrate circular references and how
    /// the garbage collector handles them. In older garbage collectors (like reference counting),
    /// circular references could cause memory leaks. .NET's tracing GC handles them correctly.
    /// </summary>
    public class CircularReferenceObject
    {
        /// <summary>
        /// The name of this object for identification during demos.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Reference to another CircularReferenceObject.
        /// This can create circular reference chains that would confuse simpler GC algorithms.
        /// </summary>
        public CircularReferenceObject? Reference { get; set; }

        /// <summary>
        /// Some data to make the object consume memory and make GC effects visible.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// A timestamp of when this object was created, useful for tracking object lifetime.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Creates a new CircularReferenceObject with the specified name.
        /// </summary>
        /// <param name="name">Name to identify this object instance</param>
        public CircularReferenceObject(string name)
        {
            Name = name;
            CreatedAt = DateTime.Now;
            
            // Allocate some memory to make the object's memory footprint visible
            Data = new byte[2000]; // 2KB per object
            
            // Fill with a pattern to ensure memory is actually allocated
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = (byte)((i + name.GetHashCode()) % 256);
            }
        }

        /// <summary>
        /// Creates a circular reference chain of the specified length.
        /// The last object will reference the first, creating a complete circle.
        /// </summary>
        /// <param name="chainLength">Number of objects in the circular chain</param>
        /// <param name="baseName">Base name for objects in the chain</param>
        /// <returns>The first object in the chain (which has no external references after method returns)</returns>
        public static CircularReferenceObject CreateCircularChain(int chainLength, string baseName)
        {
            if (chainLength <= 0)
                throw new ArgumentException("Chain length must be positive", nameof(chainLength));

            // Create all objects first
            var objects = new CircularReferenceObject[chainLength];
            for (int i = 0; i < chainLength; i++)
            {
                objects[i] = new CircularReferenceObject($"{baseName}_{i}");
            }

            // Link them in a circle
            for (int i = 0; i < chainLength; i++)
            {
                int nextIndex = (i + 1) % chainLength;
                objects[i].Reference = objects[nextIndex];
            }

            // Return the first object - but note that we're not keeping any references
            // to the array, so the entire circle becomes unreachable when this method returns
            return objects[0];
        }

        /// <summary>
        /// Traces through the circular reference chain and counts objects.
        /// This method is careful to avoid infinite loops by tracking visited objects.
        /// </summary>
        /// <returns>The number of objects in the circular reference chain</returns>
        public int CountCircularChain()
        {
            var visited = new System.Collections.Generic.HashSet<CircularReferenceObject>();
            var current = this;

            while (current != null && visited.Add(current))
            {
                current = current.Reference;
            }

            return visited.Count;
        }

        /// <summary>
        /// Checks if this object is part of a circular reference by seeing if we can
        /// reach ourselves by following the Reference chain.
        /// </summary>
        /// <returns>True if this object is part of a circular reference</returns>
        public bool IsPartOfCircle()
        {
            var visited = new System.Collections.Generic.HashSet<CircularReferenceObject>();
            var current = this.Reference;

            while (current != null && visited.Add(current))
            {
                if (current == this)
                    return true;
                
                current = current.Reference;
            }

            return false;
        }

        /// <summary>
        /// Finalizer to help demonstrate when circular reference objects are cleaned up.
        /// Note: Finalizers add overhead and should be avoided in production code unless necessary.
        /// </summary>
        ~CircularReferenceObject()
        {
            // Finalizer runs when GC is cleaning up the object
            // We can't reliably use Console.WriteLine here as console might be finalized
            // This is just for demonstration purposes
        }

        /// <summary>
        /// Provides a string representation of this object.
        /// </summary>
        /// <returns>String representation including name and creation time</returns>
        public override string ToString()
        {
            return $"CircularReferenceObject: {Name} (Created: {CreatedAt:HH:mm:ss.fff}, Data: {Data.Length} bytes)";
        }

        /// <summary>
        /// Breaks the circular reference chain by setting all Reference properties to null.
        /// This is sometimes done manually to help the GC, though it's not necessary in .NET.
        /// </summary>
        public void BreakCircularChain()
        {
            var visited = new System.Collections.Generic.HashSet<CircularReferenceObject>();
            var current = this;

            while (current != null && visited.Add(current))
            {
                var next = current.Reference;
                current.Reference = null; // Break the link
                current = next;
            }
        }
    }
}
