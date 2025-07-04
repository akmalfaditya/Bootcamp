using System;

namespace NestedTypes
{
    /// <summary>
    /// Simple illustration from the course material - the foundation concept
    /// This shows the basic syntax and structure of nested types
    /// </summary>
    public class TopLevel // Enclosing class
    {
        // These are private members that normally can't be accessed from outside
        private static int x = 10;
        private string instanceData = "I'm private instance data";

        public class Nested { }               // Nested class
        public enum Color { Red, Blue, Tan }  // Nested enum

        /// <summary>
        /// Demonstration of the key feature: nested types can access private members
        /// of their enclosing type - this is what makes them special!
        /// </summary>
        class NestedWithAccess // Note: no access modifier = private by default
        {
            static void Foo()
            {
                // This is the magic - nested class can access private static member 'x'
                Console.WriteLine($"Accessing private static field from nested type: {TopLevel.x}");
                
                // We can also access instance members if we have an instance
                TopLevel instance = new TopLevel();
                Console.WriteLine($"Accessing private instance data: {instance.instanceData}");
            }

            public static void DemonstrateAccess()
            {
                Console.WriteLine("=== Nested Type Accessing Private Members ===");
                Foo();
                Console.WriteLine("✓ Nested types have special access privileges to private members!\n");
            }
        }

        /// <summary>
        /// Public method to demonstrate the nested type functionality
        /// </summary>
        public void ShowNestedAccess()
        {
            NestedWithAccess.DemonstrateAccess();
        }
    }

    /// <summary>
    /// Inheritance example showing access to protected nested types
    /// This demonstrates how protected nested types work with inheritance
    /// </summary>
    public class SubTopLevel : TopLevel
    {
        static void UseProtectedNested()
        {
            // If TopLevel had protected nested types, we could access them here
            new TopLevel.Nested(); // Public nested - accessible
            TopLevel.Color color = TopLevel.Color.Red; // Public nested enum - accessible
        }

        public static void DemonstrateInheritedAccess()
        {
            Console.WriteLine("=== Inherited Access to Nested Types ===");
            UseProtectedNested();
            Console.WriteLine("✓ Derived classes can access public nested types of their base class\n");
        }
    }

    /// <summary>
    /// External access demonstration - showing qualification requirement
    /// To access a nested type from outside its enclosing type, 
    /// you must qualify its name with the enclosing type's name
    /// </summary>
    class SimpleExternalAccessDemo
    {
        public static void DemonstrateQualifiedAccess()
        {
            Console.WriteLine("=== Qualified Access for External Use ===");
            
            // Must use qualified names to access nested types from outside
            TopLevel.Nested n = new TopLevel.Nested();
            TopLevel.Color color = TopLevel.Color.Red;
            
            Console.WriteLine($"Created nested type: {n.GetType().Name}");
            Console.WriteLine($"Using nested enum: {color}");
            Console.WriteLine("✓ External access requires qualified names (EnclosingType.NestedType)\n");
        }
    }

    /// <summary>
    /// Comprehensive demonstration of all types that can be nested
    /// All types in C# can be nested: classes, structs, interfaces, delegates, enums
    /// </summary>
    public class ComprehensiveNestingDemo
    {
        // Nested class
        public class NestedClass
        {
            public void DoWork() => Console.WriteLine("Nested class working");
        }

        // Nested struct
        public struct NestedStruct
        {
            public int Value;
            public NestedStruct(int value) => Value = value;
        }

        // Nested interface
        public interface INestedInterface
        {
            void ProcessData();
        }

        // Nested delegate
        public delegate void NestedDelegate(string message);

        // Nested enum
        public enum NestedEnum
        {
            Option1,
            Option2,
            Option3
        }

        /// <summary>
        /// Demonstrate using all nested types
        /// </summary>
        public static void DemonstrateAllNestedTypes()
        {
            Console.WriteLine("=== All Types Can Be Nested ===");

            // Using nested class
            var nestedClass = new NestedClass();
            nestedClass.DoWork();

            // Using nested struct
            var nestedStruct = new NestedStruct(42);
            Console.WriteLine($"Nested struct value: {nestedStruct.Value}");

            // Using nested enum
            NestedEnum option = NestedEnum.Option2;
            Console.WriteLine($"Nested enum value: {option}");

            // Using nested delegate
            NestedDelegate del = message => Console.WriteLine($"Delegate says: {message}");
            del("Hello from nested delegate!");

            // Implementation of nested interface would require a separate class
            Console.WriteLine("✓ Classes, structs, interfaces, delegates, and enums can all be nested\n");
        }
    }

    /// <summary>
    /// Default accessibility demonstration
    /// For members of a class or struct, the default accessibility is private
    /// This applies to nested types as well - they are private by default
    /// </summary>
    public class DefaultAccessibilityDemo
    {
        // No access modifier = private by default (for nested types)
        class PrivateByDefault
        {
            public void Work() => Console.WriteLine("I'm private by default!");
        }

        // Explicitly public
        public class ExplicitlyPublic
        {
            public void Work() => Console.WriteLine("I'm explicitly public!");
        }

        public static void DemonstrateDefaultAccess()
        {
            Console.WriteLine("=== Default Accessibility (Private) ===");
            
            // Can access private nested type from within the same class
            var privateNested = new PrivateByDefault();
            privateNested.Work();

            var publicNested = new ExplicitlyPublic();
            publicNested.Work();

            Console.WriteLine("✓ Nested types default to private accessibility");
            Console.WriteLine("✓ This differs from non-nested types which default to internal\n");
        }
    }
}
