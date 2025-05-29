using System;

namespace AccessModifiers
{
    /// <summary>
    /// Public class - accessible from anywhere, any assembly
    /// Think of this as your "front door" - everyone can see and use it
    /// Perfect for APIs and classes you want to share with the world
    /// </summary>
    public class PublicDemo
    {
        // Public field - directly accessible (though properties are usually better)
        public string PublicField;
        
        // Public property - the preferred way to expose data
        public int PublicProperty { get; set; }
        
        // Public method - callable from anywhere
        public void PublicMethod()
        {
            Console.WriteLine("Public method called - accessible from anywhere!");
        }
        
        // Public constructor - anyone can create instances
        public PublicDemo()
        {
            PublicField = "Default public value";
            PublicProperty = 0;
        }
        
        // Public static method - callable without creating instance
        public static void StaticPublicMethod()
        {
            Console.WriteLine("Static public method - no instance needed!");
        }
    }
    
    /// <summary>
    /// Internal class - only accessible within the same assembly
    /// Think of this as "family only" - same project can use it, others can't
    /// Perfect for implementation details you don't want to expose externally
    /// </summary>
    internal class InternalDemo
    {
        // Internal field - accessible within same assembly
        internal string InternalField;
        
        // Internal property - accessible within same assembly
        internal int InternalProperty { get; set; }
        
        // Internal method - callable within same assembly
        internal void InternalMethod()
        {
            Console.WriteLine("Internal method called - same assembly only!");
        }
        
        // You can mix access modifiers within the same class
        public void PublicMethodInInternalClass()
        {
            Console.WriteLine("Public method in internal class");
            Console.WriteLine("Note: Still limited by class accessibility!");
        }
        
        // Internal constructor
        internal InternalDemo()
        {
            InternalField = "Internal secret";
            InternalProperty = 0;
        }
    }
    
    /// <summary>
    /// Class demonstrating private access modifier
    /// Private = "keep out!" - only this class can access private members
    /// </summary>
    public class PrivateDemo
    {
        // Private field - only accessible within this class
        private string _privateField;
        
        // Private property - only accessible within this class
        private int PrivateProperty { get; set; }
        
        // Public constructor that initializes private members
        public PrivateDemo()
        {
            _privateField = "Top secret data";
            PrivateProperty = 42;
        }
        
        // Private method - only callable from within this class
        private void PrivateMethod()
        {
            Console.WriteLine("Private method called internally");
            Console.WriteLine($"Private field value: {_privateField}");
        }
        
        // Public method that uses private members internally
        public void DoSomethingPublic()
        {
            Console.WriteLine("Public method executing...");
            PrivateMethod();  // Can call private method from within same class
            
            // Can access private members within same class
            _privateField = "Modified internally";
            PrivateProperty = 100;
            
            Console.WriteLine($"Private property: {PrivateProperty}");
        }
        
        // Private static method - class-level private functionality
        private static void PrivateStaticHelper()
        {
            Console.WriteLine("Private static helper method");
        }
        
        // Public method that demonstrates encapsulation
        public string GetSafeInformation()
        {
            // We can safely expose processed private data
            return $"Safe info based on private data: {_privateField.Length} characters";
        }
    }
}
