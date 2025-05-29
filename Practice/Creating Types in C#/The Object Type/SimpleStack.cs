using System;

namespace TheObjectType
{
    /// <summary>
    /// A simple stack implementation using object as storage type
    /// This is how collections worked before generics were introduced!
    /// Demonstrates the power and flexibility of object as universal container
    /// </summary>
    public class SimpleStack
    {
        // Private fields to manage our stack
        private int position;                    // Current top position
        private object[] data = new object[10];  // Fixed-size array for simplicity
        
        /// <summary>
        /// Push any object onto the stack
        /// The beauty: we can store ANY type because everything inherits from object!
        /// </summary>
        /// <param name="obj">Any object you want to store</param>
        public void Push(object obj)
        {
            // Safety check - prevent stack overflow
            if (position >= data.Length)
            {
                throw new InvalidOperationException("Stack is full!");
            }
            
            // Store the object and increment position
            data[position++] = obj;
            Console.WriteLine($"Pushed: {obj} (Type: {obj?.GetType().Name ?? "null"})");
        }
        
        /// <summary>
        /// Pop the most recent object from the stack (LIFO)
        /// Returns object type - caller must cast to expected type
        /// </summary>
        /// <returns>The object that was on top of the stack</returns>
        public object Pop()
        {
            // Safety check - prevent underflow
            if (position <= 0)
            {
                throw new InvalidOperationException("Stack is empty!");
            }
            
            // Get the object and decrement position
            object result = data[--position];
            data[position] = null;  // Clear reference to help GC
            
            Console.WriteLine($"Popped: {result} (Type: {result?.GetType().Name ?? "null"})");
            return result;
        }
        
        /// <summary>
        /// Peek at the top object without removing it
        /// </summary>
        public object Peek()
        {
            if (position <= 0)
            {
                throw new InvalidOperationException("Stack is empty!");
            }
            
            return data[position - 1];
        }
        
        /// <summary>
        /// Check if the stack is empty
        /// </summary>
        public bool IsEmpty => position == 0;
        
        /// <summary>
        /// Get current number of items in stack
        /// </summary>
        public int Count => position;
        
        /// <summary>
        /// Display all items currently in the stack (for debugging)
        /// </summary>
        public void DisplayContents()
        {
            Console.WriteLine($"Stack contents ({Count} items):");
            for (int i = position - 1; i >= 0; i--)
            {
                Console.WriteLine($"  [{i}]: {data[i]} ({data[i]?.GetType().Name})");
            }
        }
    }
}
