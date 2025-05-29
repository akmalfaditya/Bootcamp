using System;

namespace AccessModifiers
{
    /// <summary>
    /// Base class demonstrating protected access modifier
    /// Protected = "family inheritance only" - this class and derived classes can access
    /// Think of it as giving your children the keys to the family business
    /// </summary>
    public class BaseProtectedDemo
    {
        // Protected field - accessible in this class and derived classes
        protected string ProtectedField;
        
        // Protected property - accessible in this class and derived classes
        protected int ProtectedProperty { get; set; }
        
        // Public constructor
        public BaseProtectedDemo()
        {
            ProtectedField = "Protected family secret";
            ProtectedProperty = 100;
        }
        
        // Protected method - only this class and derived classes can call
        protected void ProtectedMethod()
        {
            Console.WriteLine("Protected method called from base class");
            Console.WriteLine($"Protected field: {ProtectedField}");
        }
        
        // Public method that uses protected members
        public void PublicMethod()
        {
            Console.WriteLine("Base class public method");
            ProtectedMethod();  // Can call protected method from within same class
        }
        
        // Protected virtual method - meant to be overridden by derived classes
        protected virtual void ProtectedVirtualMethod()
        {
            Console.WriteLine("Base implementation of protected virtual method");
        }
    }
    
    /// <summary>
    /// Derived class that can access protected members from base class
    /// This demonstrates the inheritance aspect of protected access
    /// </summary>
    public class DerivedProtectedDemo : BaseProtectedDemo
    {
        public DerivedProtectedDemo()
        {
            // Can access protected members from base class
            ProtectedField = "Modified by derived class";
            ProtectedProperty = 200;
        }
        
        // Public method that demonstrates access to protected base class members
        public void AccessProtectedMembers()
        {
            Console.WriteLine("Derived class accessing protected members:");
            
            // Can access protected field from base class
            Console.WriteLine($"Protected field from base: {ProtectedField}");
            
            // Can access protected property from base class
            Console.WriteLine($"Protected property from base: {ProtectedProperty}");
            
            // Can call protected method from base class
            ProtectedMethod();
            
            // Can call virtual protected method
            ProtectedVirtualMethod();
        }
        
        // Override protected virtual method
        protected override void ProtectedVirtualMethod()
        {
            Console.WriteLine("Derived class override of protected virtual method");
            base.ProtectedVirtualMethod();  // Can still call base implementation
        }
        
        // New protected method in derived class
        protected void DerivedProtectedMethod()
        {
            Console.WriteLine("New protected method in derived class");
        }
    }
    
    /// <summary>
    /// Class demonstrating protected internal access modifier
    /// Protected Internal = "family OR same assembly" - union of protected and internal
    /// Accessible from derived classes OR within same assembly
    /// </summary>
    public class ProtectedInternalDemo
    {
        // Protected internal field - accessible in same assembly OR derived classes
        protected internal string ProtectedInternalField;
        
        // Protected internal property - accessible in same assembly OR derived classes  
        protected internal int ProtectedInternalProperty { get; set; }
        
        public ProtectedInternalDemo()
        {
            ProtectedInternalField = "Protected internal data";
            ProtectedInternalProperty = 300;
        }
        
        // Protected internal method - accessible in same assembly OR derived classes
        protected internal void ProtectedInternalMethod()
        {
            Console.WriteLine("Protected internal method called");
            Console.WriteLine("Accessible in same assembly OR derived classes");
        }
        
        // Public method for demonstration
        public void PublicMethod()
        {
            Console.WriteLine("Public method in ProtectedInternalDemo");
            ProtectedInternalMethod();
        }
    }
    
    /// <summary>
    /// Derived class demonstrating access to protected internal members
    /// </summary>
    public class DerivedProtectedInternalDemo : ProtectedInternalDemo
    {
        public void AccessProtectedInternalMembers()
        {
            Console.WriteLine("Derived class accessing protected internal members:");
            
            // Can access because it's protected (inheritance)
            ProtectedInternalField = "Modified by derived class";
            ProtectedInternalProperty = 400;
            
            Console.WriteLine($"Field: {ProtectedInternalField}");
            Console.WriteLine($"Property: {ProtectedInternalProperty}");
            
            // Can call because it's protected (inheritance)
            ProtectedInternalMethod();
        }
    }
    
    /// <summary>
    /// Class demonstrating private protected access modifier  
    /// Private Protected = "family inheritance AND same assembly only"
    /// More restrictive than protected internal - intersection not union
    /// </summary>
    public class PrivateProtectedDemo
    {
        // Private protected field - only derived classes in same assembly
        private protected string PrivateProtectedField;
        
        // Private protected property - only derived classes in same assembly
        private protected int PrivateProtectedProperty { get; set; }
        
        public PrivateProtectedDemo()
        {
            PrivateProtectedField = "Private protected secret";
            PrivateProtectedProperty = 500;
        }
        
        // Private protected method - only derived classes in same assembly
        private protected void PrivateProtectedMethod()
        {
            Console.WriteLine("Private protected method called");
            Console.WriteLine("Only accessible in derived classes within same assembly");
        }
        
        // Public method for demonstration
        public void PublicMethod()
        {
            Console.WriteLine("Public method in PrivateProtectedDemo");
            PrivateProtectedMethod();  // Can call from within same class
        }
    }
    
    /// <summary>
    /// Derived class demonstrating access to private protected members
    /// This works because it's a derived class in the same assembly
    /// </summary>
    public class DerivedPrivateProtectedDemo : PrivateProtectedDemo
    {
        public void AccessPrivateProtectedMembers()
        {
            Console.WriteLine("Derived class accessing private protected members:");
            
            // Can access because it's a derived class in same assembly
            PrivateProtectedField = "Modified by derived in same assembly";
            PrivateProtectedProperty = 600;
            
            Console.WriteLine($"Field: {PrivateProtectedField}");
            Console.WriteLine($"Property: {PrivateProtectedProperty}");
            
            // Can call because it's a derived class in same assembly
            PrivateProtectedMethod();
        }
    }
}
