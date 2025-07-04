using System;

namespace Interfaces
{
    /// <summary>
    /// Explicit Interface Implementation - solving the name collision problem
    /// What happens when two interfaces have methods with the same name?
    /// You get a collision! Explicit implementation is the solution.
    /// </summary>

    // Interface I1 has a Foo method that returns void
    interface I1 
    { 
        void Foo(); 
    }

    // Interface I2 has a Foo method that returns int
    // Same name, different return type - classic collision!
    interface I2 
    { 
        int Foo(); 
    }

    /// <summary>
    /// Widget implements both interfaces with conflicting method names
    /// We solve this using explicit interface implementation
    /// </summary>
    public class Widget : I1, I2
    {
        // Implicit implementation for I1.Foo()
        // This is the "default" Foo that you can call directly on Widget
        public void Foo()
        {
            Console.WriteLine("Widget's implementation of I1.Foo");
        }

        // Explicit implementation for I2.Foo()
        // Notice: No access modifier (not public, not private, nothing!)
        // Notice: Interface name before the method name
        // This can ONLY be called when cast to I2
        int I2.Foo()
        {
            Console.WriteLine("Widget's implementation of I2.Foo");
            return 42;
        }
    }

    /// <summary>
    /// Demo class to show how explicit implementation works
    /// </summary>
    public static class ExplicitImplementationDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Explicit Interface Implementation Demo ===");
            
            Widget w = new Widget();
            
            // This calls the implicit (public) implementation
            w.Foo(); // Output: Widget's implementation of I1.Foo
            
            // To call explicit implementations, you MUST cast to the interface
            ((I1)w).Foo(); // Output: Widget's implementation of I1.Foo
            
            int result = ((I2)w).Foo(); // Output: Widget's implementation of I2.Foo
            Console.WriteLine($"I2.Foo() returned: {result}");
            
            // This won't compile - no direct access to explicit implementation:
            // int result2 = w.I2.Foo(); // COMPILER ERROR!
        }
    }
}
