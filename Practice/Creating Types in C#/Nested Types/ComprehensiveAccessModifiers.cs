using System;

namespace NestedTypes
{
    /// <summary>
    /// Comprehensive access modifiers demonstration for nested types
    /// Unlike non-nested types, nested types can use the FULL range of access modifiers:
    /// public, internal, private, protected, protected internal, private protected, file
    /// This offers fine-grained control over their visibility
    /// </summary>
    public class FullAccessModifierDemo
    {
        private string privateData = "Secret company data";
        protected string protectedData = "Department information";
        internal string internalData = "Assembly-wide data";
        public string publicData = "Everyone can see this";

        // PUBLIC nested type - accessible from anywhere
        public class PublicNested
        {
            public void ShowInfo()
            {
                Console.WriteLine("✓ I'm public - accessible from any assembly");
                var parent = new FullAccessModifierDemo();
                Console.WriteLine($"I can access all parent data: {parent.privateData}");
            }
        }

        // PRIVATE nested type - only accessible within the enclosing type
        private class PrivateNested
        {
            public void ShowInfo()
            {
                Console.WriteLine("✓ I'm private - only my enclosing type can use me");
                var parent = new FullAccessModifierDemo();
                Console.WriteLine($"Private access to: {parent.privateData}");
            }
        }

        // PROTECTED nested type - accessible in derived classes
        protected class ProtectedNested
        {
            public void ShowInfo()
            {
                Console.WriteLine("✓ I'm protected - derived classes can access me");
                var parent = new FullAccessModifierDemo();
                Console.WriteLine($"Protected access to: {parent.protectedData}");
            }
        }

        // INTERNAL nested type - accessible within the same assembly
        internal class InternalNested
        {
            public void ShowInfo()
            {
                Console.WriteLine("✓ I'm internal - same assembly can access me");
                var parent = new FullAccessModifierDemo();
                Console.WriteLine($"Internal access to: {parent.internalData}");
            }
        }

        // PROTECTED INTERNAL nested type - accessible in derived classes OR same assembly
        protected internal class ProtectedInternalNested
        {
            public void ShowInfo()
            {
                Console.WriteLine("✓ I'm protected internal - derived classes OR same assembly");
                var parent = new FullAccessModifierDemo();
                Console.WriteLine($"Protected internal access to: {parent.publicData}");
            }
        }

        // PRIVATE PROTECTED nested type - accessible in derived classes within the same assembly
        private protected class PrivateProtectedNested
        {
            public void ShowInfo()
            {
                Console.WriteLine("✓ I'm private protected - derived classes within same assembly only");
                var parent = new FullAccessModifierDemo();
                Console.WriteLine($"Private protected access to: {parent.protectedData}");
            }
        }

        /// <summary>
        /// Demonstrate all access levels from within the enclosing type
        /// </summary>
        public void DemonstrateAllAccessLevels()
        {
            Console.WriteLine("=== Full Range of Access Modifiers for Nested Types ===");
            
            // All nested types are accessible from within the enclosing type
            new PublicNested().ShowInfo();
            new PrivateNested().ShowInfo();
            new ProtectedNested().ShowInfo();
            new InternalNested().ShowInfo();
            new ProtectedInternalNested().ShowInfo();
            new PrivateProtectedNested().ShowInfo();
            
            Console.WriteLine("✓ Enclosing type can access ALL its nested types regardless of access modifier\n");
        }
    }

    /// <summary>
    /// Derived class to test protected access to nested types
    /// This demonstrates inheritance-based access to protected nested types
    /// </summary>
    public class DerivedFullAccessDemo : FullAccessModifierDemo
    {
        public void TestInheritedAccess()
        {
            Console.WriteLine("=== Derived Class Access to Protected Nested Types ===");
            
            // Can access public nested types
            new PublicNested().ShowInfo();
            
            // Can access protected nested types - this is the key feature!
            new ProtectedNested().ShowInfo();
            
            // Can access internal nested types (same assembly)
            new InternalNested().ShowInfo();
            
            // Can access protected internal nested types
            new ProtectedInternalNested().ShowInfo();
            
            // Can access private protected nested types (derived + same assembly)
            new PrivateProtectedNested().ShowInfo();
            
            // CANNOT access private nested types - they're truly private
            // new PrivateNested(); // This would cause a compilation error
            
            Console.WriteLine("✓ Derived classes inherit access to protected nested types\n");
        }
    }

    /// <summary>
    /// External class to test access limitations
    /// This shows what external classes can and cannot access
    /// </summary>
    public class ExternalFullAccessTest
    {
        public void TestExternalAccess()
        {
            Console.WriteLine("=== External Access Limitations ===");
            
            // Can access public nested types with qualified name
            var publicNested = new FullAccessModifierDemo.PublicNested();
            publicNested.ShowInfo();
            
            // Can access internal nested types (same assembly)
            var internalNested = new FullAccessModifierDemo.InternalNested();
            internalNested.ShowInfo();
            
            // Can access protected internal nested types (same assembly)
            var protectedInternalNested = new FullAccessModifierDemo.ProtectedInternalNested();
            protectedInternalNested.ShowInfo();
            
            // CANNOT access private, protected, or private protected nested types
            // These would cause compilation errors:
            // new FullAccessModifierDemo.PrivateNested();
            // new FullAccessModifierDemo.ProtectedNested();
            // new FullAccessModifierDemo.PrivateProtectedNested();
            
            Console.WriteLine("✓ External classes have limited access based on access modifiers");
            Console.WriteLine("✓ Must use qualified names for accessible nested types\n");
        }
    }

    /// <summary>
    /// Comparison with non-nested types to highlight the difference
    /// Non-nested types default to internal and can only be public or internal
    /// Nested types default to private and can use all access modifiers
    /// </summary>
    internal class NonNestedComparison // internal by default for non-nested types
    {
        // This class demonstrates that non-nested types have limited access options
        // They can only be public or internal, never private, protected, etc.
    }

    public class NestedVsNonNestedDemo
    {
        // This nested class can be private - something non-nested types cannot be
        private class PrivateHelper
        {
            public void DoInternalWork()
            {
                Console.WriteLine("I'm a private nested class - non-nested types can't be private!");
            }
        }

        public static void DemonstrateTheDifference()
        {
            Console.WriteLine("=== Nested vs Non-Nested Type Access ===");
            Console.WriteLine("Non-nested types: Can only be public or internal (default)");
            Console.WriteLine("Nested types: Can use ALL access modifiers, default to private");
            
            var demo = new NestedVsNonNestedDemo();
            var helper = new PrivateHelper();
            helper.DoInternalWork();
            
            Console.WriteLine("✓ Nested types offer much more flexible access control\n");
        }
    }
}
