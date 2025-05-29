using System;

namespace NestedTypes
{
    /// <summary>
    /// Access modifier demonstrations with nested types
    /// This is where nested types really show their power - different levels of access!
    /// Think of it like a building with different security clearances for different floors
    /// </summary>
    public class AccessModifierDemo
    {
        private string secretCode = "TOP_SECRET_123";
        protected string departmentInfo = "Engineering Department";
        internal string companyInfo = "Formulatrix Corp";
        public string publicInfo = "Welcome to our company!";

        // PUBLIC nested class - everyone can see and use this
        // Like the lobby of a building - open to all visitors
        public class PublicNested
        {
            public void ShowPublicAccess()
            {
                Console.WriteLine("\n=== Public Nested Class ===");
                Console.WriteLine("✓ I'm accessible from anywhere!");
                Console.WriteLine("✓ Any assembly can create and use me");
                
                AccessModifierDemo parent = new AccessModifierDemo();
                // Can access all members of the outer class
                Console.WriteLine($"Secret code: {parent.secretCode}");
                Console.WriteLine($"Department: {parent.departmentInfo}");
                Console.WriteLine($"Company: {parent.companyInfo}");
                Console.WriteLine($"Public info: {parent.publicInfo}");
            }
        }

        // PRIVATE nested class - only the outer class can see this
        // Like the CEO's private office - invitation only!
        private class PrivateNested
        {
            private string privateMessage = "I'm hidden from the world!";

            public void DoSecretWork()
            {
                Console.WriteLine("\n=== Private Nested Class ===");
                Console.WriteLine("🔒 Only my outer class can create me");
                Console.WriteLine("🔒 I handle the most sensitive operations");
                
                AccessModifierDemo parent = new AccessModifierDemo();
                Console.WriteLine($"Accessing secret code: {parent.secretCode}");
                Console.WriteLine($"My private message: {privateMessage}");
            }
        }

        // PROTECTED nested class - only this class and its children can see this
        // Like a family meeting room - relatives only
        protected class ProtectedNested
        {
            public void ShowFamilySecrets()
            {
                Console.WriteLine("\n=== Protected Nested Class ===");
                Console.WriteLine("👨‍👩‍👧‍👦 Only family members (inheritance) can see me");
                
                AccessModifierDemo parent = new AccessModifierDemo();
                Console.WriteLine($"Department info: {parent.departmentInfo}");
                Console.WriteLine($"Secret code: {parent.secretCode}");
            }
        }

        // INTERNAL nested class - only classes in the same assembly can see this
        // Like an employee break room - company staff only
        internal class InternalNested
        {
            public void ShowCompanySecrets()
            {
                Console.WriteLine("\n=== Internal Nested Class ===");
                Console.WriteLine("🏢 Only classes in the same assembly can use me");
                
                AccessModifierDemo parent = new AccessModifierDemo();
                Console.WriteLine($"Company info: {parent.companyInfo}");
                Console.WriteLine($"Secret code: {parent.secretCode}");
            }
        }

        // PROTECTED INTERNAL - combination of protected AND internal
        // Like a management meeting room - managers and family members
        protected internal class ProtectedInternalNested
        {
            public void ShowExecutiveAccess()
            {
                Console.WriteLine("\n=== Protected Internal Nested Class ===");
                Console.WriteLine("🤝 Accessible by inheritance OR same assembly");
                
                AccessModifierDemo parent = new AccessModifierDemo();
                Console.WriteLine($"All access granted: {parent.secretCode}");
            }
        }

        // Method to demonstrate using private nested class
        public void UsePrivateNested()
        {
            // Only I can create instances of my private nested class
            PrivateNested secret = new PrivateNested();
            secret.DoSecretWork();
        }

        public void DemonstrateAllAccess()
        {
            Console.WriteLine("=== Access Modifier Demonstration ===");
            
            // We can use public nested class easily
            PublicNested publicInstance = new PublicNested();
            publicInstance.ShowPublicAccess();
            
            // We can use our private nested class (because we're the outer class)
            UsePrivateNested();
            
            // We can use protected nested class (we're in the same class)
            ProtectedNested protectedInstance = new ProtectedNested();
            protectedInstance.ShowFamilySecrets();
            
            // We can use internal nested class (same assembly)
            InternalNested internalInstance = new InternalNested();
            internalInstance.ShowCompanySecrets();
            
            // We can use protected internal nested class
            ProtectedInternalNested execInstance = new ProtectedInternalNested();
            execInstance.ShowExecutiveAccess();
        }
    }

    /// <summary>
    /// Derived class to demonstrate protected nested type access
    /// This shows how inheritance affects nested type visibility
    /// </summary>
    public class DerivedAccessDemo : AccessModifierDemo
    {
        public void TestInheritedAccess()
        {
            Console.WriteLine("\n=== Derived Class Access Test ===");
            
            // We can access public nested types - no surprise there
            PublicNested publicNested = new PublicNested();
            Console.WriteLine("✓ Can access public nested type");
            
            // We can access protected nested types because we inherited them
            ProtectedNested protectedNested = new ProtectedNested();
            protectedNested.ShowFamilySecrets();
            Console.WriteLine("✓ Can access protected nested type through inheritance");
            
            // We can access internal nested types (same assembly)
            InternalNested internalNested = new InternalNested();
            Console.WriteLine("✓ Can access internal nested type (same assembly)");
            
            // We can access protected internal nested types (both conditions met)
            ProtectedInternalNested execNested = new ProtectedInternalNested();
            Console.WriteLine("✓ Can access protected internal nested type");
            
            // But we CANNOT access private nested types - they're truly private!
            // PrivateNested privateNested = new PrivateNested(); // This would cause a compile error!
            Console.WriteLine("❌ Cannot access private nested type (as expected)");
        }
    }

    /// <summary>
    /// External class to show what's accessible from outside
    /// This represents a "stranger" trying to access our nested types
    /// </summary>
    public class ExternalAccessDemo
    {
        public void TestExternalAccess()
        {
            Console.WriteLine("\n=== External Access Test ===");
            
            // We can access public nested types from anywhere
            AccessModifierDemo.PublicNested publicNested = new AccessModifierDemo.PublicNested();
            publicNested.ShowPublicAccess();
            Console.WriteLine("✓ External class can access public nested type");
            
            // We can access internal nested types (same assembly)
            AccessModifierDemo.InternalNested internalNested = new AccessModifierDemo.InternalNested();
            internalNested.ShowCompanySecrets();
            Console.WriteLine("✓ External class can access internal nested type (same assembly)");
            
            // We can access protected internal nested types (internal condition met)
            AccessModifierDemo.ProtectedInternalNested execNested = new AccessModifierDemo.ProtectedInternalNested();
            execNested.ShowExecutiveAccess();
            Console.WriteLine("✓ External class can access protected internal nested type");
            
            // But we CANNOT access protected or private nested types
            // AccessModifierDemo.ProtectedNested protectedNested = new AccessModifierDemo.ProtectedNested(); // Compile error!
            // AccessModifierDemo.PrivateNested privateNested = new AccessModifierDemo.PrivateNested(); // Compile error!
            Console.WriteLine("❌ Cannot access protected or private nested types (as expected)");
        }
    }

    /// <summary>
    /// Static nested types demonstration
    /// Static nested types don't need an instance of the outer class - they're independent!
    /// Think of them like a utility shed in your backyard - it belongs to your property but works on its own
    /// </summary>
    public class StaticNestedDemo
    {
        private static string sharedResource = "I'm shared by all!";
        private string instanceResource = "I belong to an instance";

        // Static nested class - doesn't need an outer instance
        public static class StaticNested
        {
            public static void DoStaticWork()
            {
                Console.WriteLine("\n=== Static Nested Class ===");
                Console.WriteLine("⚡ I don't need an instance of the outer class!");
                Console.WriteLine("⚡ I'm like a utility class that happens to live here");
                
                // Can access static members of outer class
                Console.WriteLine($"Shared resource: {sharedResource}");
                
                // But CANNOT access instance members directly
                // Console.WriteLine($"Instance resource: {instanceResource}"); // This would fail!
                Console.WriteLine("❌ Cannot access instance members without an instance");
            }

            public static void WorkWithInstance()
            {
                // But we can work with instances if we create them
                StaticNestedDemo instance = new StaticNestedDemo();
                Console.WriteLine($"Instance resource via instance: {instance.instanceResource}");
            }
        }

        public void DemonstrateStaticNested()
        {
            Console.WriteLine("=== Static Nested Demonstration ===");
            
            // Call static nested method without creating outer instance
            StaticNested.DoStaticWork();
            StaticNested.WorkWithInstance();
        }
    }
}
