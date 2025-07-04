using System;

namespace NestedTypes
{
    /// <summary>
    /// Basic nested types demonstration - where it all begins!
    /// A nested type is a type (class, struct, interface, enum, or delegate) 
    /// declared inside the scope of another class or struct (the enclosing type).
    /// Think of it like Russian dolls - one type living inside another!
    /// </summary>
    public class OuterClass
    {
        private int privateValue = 42;
        protected string protectedValue = "I'm protected!";
        public static int instanceCounter = 0;

        public OuterClass()
        {
            instanceCounter++;
            Console.WriteLine($"OuterClass instance #{instanceCounter} created");
        }

        // Here's the magic - a nested class that lives inside our outer class
        // It's like having a specialized assistant that knows all your secrets
        public class NestedClass
        {
            private string nestedField = "I'm nested!";

            /// <summary>
            /// This is where nested types shine - accessing private members of the outer class
            /// It's like being part of the family - you get access to private stuff!
            /// </summary>
            public void ShowOuterPrivateValue()
            {
                // Look at this! We can create an instance of the outer class
                // and access its private members directly - no getters needed!
                OuterClass outer = new OuterClass();
                Console.WriteLine($"Accessing outer's private value: {outer.privateValue}");
                Console.WriteLine($"Accessing outer's protected value: {outer.protectedValue}");
                
                // We can even access static members easily
                Console.WriteLine($"Total instances created: {instanceCounter}");
            }

            public void ShowNestedInfo()
            {
                Console.WriteLine($"I'm a nested class! My field value: {nestedField}");
                Console.WriteLine($"My full type name is: {this.GetType().FullName}");
            }
        }

        // Multiple nested types are totally fine - think of them as different rooms
        public struct NestedStruct
        {
            public int X;
            public int Y;

            public NestedStruct(int x, int y)
            {
                X = x;
                Y = y;
                Console.WriteLine($"Nested struct created at ({X}, {Y})");
            }

            public double DistanceFromOrigin()
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        // Nested enums are perfect for values that only make sense in this context
        public enum OperationMode
        {
            Idle,
            Processing,
            Complete,
            Error
        }

        // Even nested interfaces! Though less common, they're useful for callbacks
        public interface INestedCallback
        {
            void OnComplete();
            void OnError(string message);
        }

        // A method that uses our nested types
        public void DemonstrateNestedTypes()
        {
            Console.WriteLine("\n=== Demonstrating Basic Nested Types ===");
            
            // Using nested class
            NestedClass nested = new NestedClass();
            nested.ShowNestedInfo();
            nested.ShowOuterPrivateValue();
            
            // Using nested struct
            NestedStruct point = new NestedStruct(3, 4);
            Console.WriteLine($"Distance from origin: {point.DistanceFromOrigin():F2}");
            
            // Using nested enum
            OperationMode mode = OperationMode.Processing;
            Console.WriteLine($"Current operation mode: {mode}");
        }
    }

    /// <summary>
    /// Another example showing how to use nested types from outside
    /// This is like visiting someone's house - you need permission to enter certain rooms
    /// </summary>
    public class NestedTypeConsumer
    {
        public void UseNestedTypes()
        {
            Console.WriteLine("\n=== Using Nested Types from Outside ===");
            
            // To access nested types from outside, we use the full qualified name
            // It's like giving someone your full address: "John's House, Living Room"
            OuterClass.NestedClass nested = new OuterClass.NestedClass();
            nested.ShowNestedInfo();
            
            // Same pattern for structs and enums
            OuterClass.NestedStruct point = new OuterClass.NestedStruct(5, 12);
            Console.WriteLine($"Point distance: {point.DistanceFromOrigin():F2}");
            
            OuterClass.OperationMode mode = OuterClass.OperationMode.Complete;
            Console.WriteLine($"Mode from outside: {mode}");
        }
    }

    /// <summary>
    /// Generic nested types - because sometimes your nested types need to be flexible too!
    /// It's like having a room that can change its purpose based on what you put in it
    /// </summary>
    public class GenericOuter<T>
    {
        private T outerValue;

        public GenericOuter(T value)
        {
            outerValue = value;
        }

        // A nested class that can work with the outer class's generic type
        public class GenericNested
        {
            public void ProcessOuterValue()
            {
                GenericOuter<T> outer = new GenericOuter<T>(default(T)!);
                Console.WriteLine($"Processing value of type: {typeof(T).Name}");
                Console.WriteLine($"Outer value: {outer.outerValue}");
            }
        }

        // A nested class with its own generic parameter - double the flexibility!
        public class DoubleGenericNested<U>
        {
            public void ProcessBothTypes(U nestedValue)
            {
                Console.WriteLine($"Outer type: {typeof(T).Name}, Nested type: {typeof(U).Name}");
                Console.WriteLine($"Nested value: {nestedValue}");
            }
        }
    }
}
