using System;

namespace Interfaces
{
    /// <summary>
    /// Interfaces and Boxing - Performance Considerations
    /// When you cast a struct to an interface, boxing occurs.
    /// This means the struct gets copied from stack to heap.
    /// Important to understand for performance-critical code!
    /// </summary>

    // Simple interface for our boxing demo
    interface IBoxingDemo
    {
        void Foo();
    }

    /// <summary>
    /// A struct that implements an interface
    /// Structs are value types - they normally live on the stack
    /// </summary>
    struct MyStruct : IBoxingDemo
    {
        public int Value { get; set; }

        public MyStruct(int value)
        {
            Value = value;
        }

        public void Foo()
        {
            Console.WriteLine($"MyStruct.Foo called - Value: {Value}");
        }
    }

    /// <summary>
    /// Boxing demonstration class
    /// </summary>
    public static class BoxingDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Boxing with Interfaces Demo ===\n");

            MyStruct s = new MyStruct(42);
            
            Console.WriteLine("1. Calling method directly on struct (NO boxing):");
            s.Foo(); // Direct call - struct stays on stack
            
            Console.WriteLine("\n2. Casting struct to interface (BOXING occurs here!):");
            IBoxingDemo i = s; // BOXING! - struct copied to heap
            i.Foo(); // Called on the boxed copy
            
            Console.WriteLine("\n3. Demonstrating the copy behavior:");
            s.Value = 100; // Change original struct
            Console.WriteLine("After changing original struct value to 100:");
            Console.WriteLine("Original struct:");
            s.Foo(); // Shows 100
            Console.WriteLine("Boxed interface:");
            i.Foo(); // Still shows 42 (it's a separate copy!)

            Console.WriteLine("\nKey takeaway: The interface holds a COPY, not a reference!");
            Console.WriteLine("In performance-critical code, be mindful of boxing with value types.");
        }
    }
}
