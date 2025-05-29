namespace AccessModifiers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Access Modifiers: Controlling Code Visibility ===\n");

            // Let's explore each access modifier with practical examples
            DemonstratePublicAccess();
            DemonstrateInternalAccess();
            DemonstratePrivateAccess();
            DemonstrateProtectedAccess();
            DemonstrateProtectedInternalAccess();
            DemonstratePrivateProtectedAccess();
            DemonstrateFileAccess();
            DemonstrateRealWorldScenarios();
            DemonstrateAccessModifierRules();

            Console.WriteLine("\n=== Access Modifiers Demo Complete! ===");
            Console.ReadKey();
        }

        /// <summary>
        /// Public = "Everyone welcome!"
        /// Most permissive - accessible from anywhere, any assembly
        /// Use for APIs you want to expose to the world
        /// </summary>
        static void DemonstratePublicAccess()
        {
            Console.WriteLine("1. Public Access - The Open Door:");
            
            // Public class can be accessed from anywhere
            var publicDemo = new PublicDemo();
            
            // All public members are accessible
            publicDemo.PublicField = "Hello World";
            publicDemo.PublicProperty = 42;
            publicDemo.PublicMethod();
            
            Console.WriteLine($"Public field: {publicDemo.PublicField}");
            Console.WriteLine($"Public property: {publicDemo.PublicProperty}");
            
            Console.WriteLine("\nPublic is perfect for:");
            Console.WriteLine("✅ API methods you want others to use");
            Console.WriteLine("✅ Properties that need external access");
            Console.WriteLine("✅ Classes designed for public consumption");
            Console.WriteLine("⚠️  Be careful: once public, always public (breaking changes!)");
            Console.WriteLine();
        }

        /// <summary>
        /// Internal = "Family only" (same assembly)
        /// Visible within the same project/assembly but not outside
        /// Great for implementation details you don't want to expose
        /// </summary>
        static void DemonstrateInternalAccess()
        {
            Console.WriteLine("2. Internal Access - The Family Secret:");
            
            // Internal class accessible within same assembly
            var internalDemo = new InternalDemo();
            
            // Internal members accessible within same assembly
            internalDemo.InternalField = "Internal secret";
            internalDemo.InternalProperty = 100;
            internalDemo.InternalMethod();
            
            Console.WriteLine($"Internal field: {internalDemo.InternalField}");
            Console.WriteLine($"Internal property: {internalDemo.InternalProperty}");
            
            Console.WriteLine("\nInternal is perfect for:");
            Console.WriteLine("✅ Helper classes within your project");
            Console.WriteLine("✅ Implementation details");
            Console.WriteLine("✅ Classes shared between project components");
            Console.WriteLine("✅ Default access level for top-level classes");
            Console.WriteLine();
        }

        /// <summary>
        /// Private = "Keep out!" 
        /// Only accessible within the same class
        /// Your secret implementation details
        /// </summary>
        static void DemonstratePrivateAccess()
        {
            Console.WriteLine("3. Private Access - The Locked Vault:");
            
            var privateDemo = new PrivateDemo();
            
            // Can only access public methods that might use private members
            privateDemo.DoSomethingPublic();
            
            // These would cause compiler errors:
            // privateDemo._privateField = "Can't touch this!";  // ERROR!
            // privateDemo.PrivateMethod();  // ERROR!
            
            Console.WriteLine("✅ Private members accessed internally through public methods");
            Console.WriteLine("❌ Direct access to private members blocked");
            
            Console.WriteLine("\nPrivate is perfect for:");
            Console.WriteLine("✅ Internal state/fields");
            Console.WriteLine("✅ Helper methods used only within the class");
            Console.WriteLine("✅ Implementation details that should stay hidden");
            Console.WriteLine("✅ Data that needs protection from external modification");
            Console.WriteLine();
        }

        /// <summary>
        /// Protected = "Family inheritance only"
        /// Accessible in the same class and derived classes
        /// Building blocks for inheritance hierarchies
        /// </summary>
        static void DemonstrateProtectedAccess()
        {
            Console.WriteLine("4. Protected Access - The Inheritance Chain:");
            
            var baseDemo = new BaseProtectedDemo();
            var derivedDemo = new DerivedProtectedDemo();
            
            // Can access public methods
            baseDemo.PublicMethod();
            derivedDemo.PublicMethod();
            derivedDemo.AccessProtectedMembers();
            
            // These would cause compiler errors:
            // baseDemo.ProtectedField = "Can't access";  // ERROR!
            // derivedDemo.ProtectedField = "Can't access";  // ERROR!
            
            Console.WriteLine("✅ Protected members accessed through derived class methods");
            Console.WriteLine("❌ Direct external access to protected members blocked");
            
            Console.WriteLine("\nProtected is perfect for:");
            Console.WriteLine("✅ Base class members meant for derived classes");
            Console.WriteLine("✅ Template method patterns");
            Console.WriteLine("✅ Shared functionality in inheritance hierarchies");
            Console.WriteLine("✅ Virtual/abstract members meant to be overridden");
            Console.WriteLine();
        }

        /// <summary>
        /// Protected Internal = "Family OR same assembly"
        /// Union of protected and internal access
        /// Accessible from derived classes OR same assembly
        /// </summary>
        static void DemonstrateProtectedInternalAccess()
        {
            Console.WriteLine("5. Protected Internal Access - The Extended Family:");
            
            var protectedInternalDemo = new ProtectedInternalDemo();
            var derivedDemo = new DerivedProtectedInternalDemo();
            
            // Accessible within same assembly (internal part)
            protectedInternalDemo.ProtectedInternalField = "Accessible in assembly";
            protectedInternalDemo.ProtectedInternalProperty = 500;
            protectedInternalDemo.ProtectedInternalMethod();
            
            // Also accessible in derived classes (protected part)
            derivedDemo.AccessProtectedInternalMembers();
            
            Console.WriteLine($"Field: {protectedInternalDemo.ProtectedInternalField}");
            Console.WriteLine($"Property: {protectedInternalDemo.ProtectedInternalProperty}");
            
            Console.WriteLine("\nProtected Internal is perfect for:");
            Console.WriteLine("✅ Framework/library internal APIs");
            Console.WriteLine("✅ Members shared within assembly AND inheritance chain");
            Console.WriteLine("✅ Extensibility points for derived classes");
            Console.WriteLine("✅ When you need both internal AND protected access");
            Console.WriteLine();
        }

        /// <summary>
        /// Private Protected = "Family inheritance AND same assembly only"
        /// Intersection of private and protected
        /// More restrictive than protected internal
        /// </summary>
        static void DemonstratePrivateProtectedAccess()
        {
            Console.WriteLine("6. Private Protected Access - The Exclusive Club:");
            
            var privateProtectedDemo = new PrivateProtectedDemo();
            var derivedDemo = new DerivedPrivateProtectedDemo();
            
            // Only accessible through derived class methods in same assembly
            derivedDemo.AccessPrivateProtectedMembers();
            
            // These would cause compiler errors:
            // privateProtectedDemo.PrivateProtectedField = "Can't access";  // ERROR!
            // derivedDemo.PrivateProtectedField = "Can't access";  // ERROR!
            
            Console.WriteLine("✅ Private protected members accessed through derived class methods");
            Console.WriteLine("❌ No direct external access, even within same assembly");
            
            Console.WriteLine("\nPrivate Protected is perfect for:");
            Console.WriteLine("✅ Very controlled inheritance scenarios");
            Console.WriteLine("✅ When you want inheritance but only within your assembly");
            Console.WriteLine("✅ Advanced framework design patterns");
            Console.WriteLine("✅ Preventing external assembly inheritance abuse");
            Console.WriteLine();
        }

        /// <summary>
        /// File access modifier (C# 11+) = "This file only"
        /// Most restrictive - only within the same source file
        /// Great for source generators and file-scoped utilities
        /// </summary>
        static void DemonstrateFileAccess()
        {
            Console.WriteLine("7. File Access (C# 11+) - The File-Scoped Secret:");
            
            // File-scoped class can only be used within this file
            // var fileDemo = new FileAccessDemo();
            // fileDemo.DoSomething();
            
            Console.WriteLine("✅ File-scoped class accessible within same file");
            Console.WriteLine("❌ Would be invisible to other files in same project");
            
            Console.WriteLine("\nFile access is perfect for:");
            Console.WriteLine("✅ Source generator utilities");
            Console.WriteLine("✅ File-specific helper classes");
            Console.WriteLine("✅ Preventing namespace pollution");
            Console.WriteLine("✅ Very localized implementation details");
            Console.WriteLine();
        }

        /// <summary>
        /// Real-world scenarios showing when to use each access modifier
        /// These patterns are what you'll see in production code
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("8. Real-World Scenarios:");
            
            // Banking system example
            var bankAccount = new BankAccount("John Doe", 1000.00m);
            
            // Public API for users
            Console.WriteLine($"Account holder: {bankAccount.AccountHolder}");
            Console.WriteLine($"Balance: ${bankAccount.GetBalance():F2}");
            
            // This works - controlled access
            bankAccount.Deposit(500.00m);
            bankAccount.Withdraw(200.00m);
            
            // These would be errors - protected data:
            // bankAccount._balance = 999999;  // ERROR!
            // bankAccount.ValidateTransaction(100);  // ERROR!
            
            Console.WriteLine($"New balance: ${bankAccount.GetBalance():F2}");
            
            // Configuration system example
            var config = new ConfigurationManager();
            config.LoadConfiguration();
            
            // Game entity example
            var player = new Player("Hero", 100);
            var enemy = new Enemy("Orc", 50);
            
            player.DisplayStatus();
            enemy.DisplayStatus();
            
            Console.WriteLine("\nReal-world access patterns:");
            Console.WriteLine("✅ Public: APIs, properties for external use");
            Console.WriteLine("✅ Private: Internal state, helper methods");
            Console.WriteLine("✅ Protected: Base class functionality for inheritance");
            Console.WriteLine("✅ Internal: Project-wide utilities, implementation details");
            Console.WriteLine();
        }

        /// <summary>
        /// Important rules and gotchas about access modifiers
        /// The stuff that trips up even experienced developers!
        /// </summary>
        static void DemonstrateAccessModifierRules()
        {
            Console.WriteLine("9. Important Rules and Gotchas:");
            
            Console.WriteLine("📋 Access Modifier Rules:");
            Console.WriteLine("1. Members can't be more accessible than their containing type");
            Console.WriteLine("2. Interfaces and enums are public by default");
            Console.WriteLine("3. Classes are internal by default");
            Console.WriteLine("4. Class members are private by default");
            Console.WriteLine("5. Nested classes can have any access modifier");
            
            Console.WriteLine("\n⚠️  Common Gotchas:");
            Console.WriteLine("• Protected members aren't accessible to external code");
            Console.WriteLine("• Internal types can't have public members visible outside assembly");
            Console.WriteLine("• Virtual members can have access restrictions in derived classes");
            Console.WriteLine("• Constructors can be private (singleton pattern)");
            
            Console.WriteLine("\n💡 Best Practices:");
            Console.WriteLine("• Start with most restrictive, open up as needed");
            Console.WriteLine("• Use properties instead of public fields");
            Console.WriteLine("• Document your public APIs clearly");
            Console.WriteLine("• Consider future extensibility when choosing modifiers");
            Console.WriteLine("• Use InternalsVisibleTo for testing assemblies");
            
            Console.WriteLine("\n🎯 Golden Rule:");
            Console.WriteLine("Expose the minimum necessary for functionality!");
            Console.WriteLine();
        }
    }
}
