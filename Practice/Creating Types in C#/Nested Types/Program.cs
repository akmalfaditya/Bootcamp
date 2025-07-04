namespace NestedTypes
{
    /// <summary>
    /// Comprehensive Nested Types demonstration project
    /// 
    /// This project covers ALL aspects of nested types as presented in the course material:
    /// 
    /// 1. FOUNDATION CONCEPTS (from course material simple illustrations):
    ///    - Basic TopLevel class with nested types showing core syntax
    ///    - Private member access demonstration (the killer feature)
    ///    - Qualified access requirements for external usage
    ///    - Default accessibility behavior (private for nested, internal for non-nested)
    ///    - All types can be nested: classes, structs, interfaces, enums, delegates
    /// 
    /// 2. FULL ACCESS MODIFIER RANGE:
    ///    - Unlike non-nested types, nested types can use ALL 6 access modifiers
    ///    - public, private, protected, internal, protected internal, private protected
    ///    - Fine-grained control over nested type visibility
    ///    - Inheritance scenarios with protected nested types
    /// 
    /// 3. PRIVATE MEMBER ACCESS (the superpower):
    ///    - Nested types can access ALL private members of their enclosing type
    ///    - No need for getters/setters - direct field access
    ///    - Intimate collaboration between tightly coupled types
    ///    - Real examples: BankAccount with Transaction, SecureVault with Auditor
    /// 
    /// 4. WHEN TO USE (practical scenarios):
    ///    - Stronger Access Control: Helper types that shouldn't be global
    ///    - Intimate Collaboration: When nested type needs private access
    ///    - Logical Grouping: Conceptually related types kept together
    ///    - Avoiding Namespace Clutter: Specialized types stay localized
    /// 
    /// 5. REAL-WORLD PATTERNS:
    ///    - Builder patterns with nested builders
    ///    - State machines with nested states
    ///    - Configuration classes with nested validation
    ///    - Iterator implementations (what compiler generates)
    /// 
    /// Key Learning Points:
    /// - Nested types = "Russian dolls" - types within types
    /// - Special access privileges to private members of enclosing type
    /// - Default to private accessibility (unlike non-nested types)
    /// - External access requires qualified names (EnclosingType.NestedType)
    /// - Perfect for tight coupling scenarios where types truly belong together
    /// - Used extensively in .NET framework and advanced design patterns
    /// 
    /// This implementation demonstrates production-ready examples that you'll
    /// encounter in real-world C# development, from simple syntax to complex
    /// architectural patterns.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Nested Types: Complete Learning Guide ===\n");
            Console.WriteLine("Understanding nested types - types declared inside other types");
            Console.WriteLine("Key features: private access, full access modifiers, qualified names\n");

            // Start with the foundation concepts from the course material
            DemonstrateFoundationConcepts();
            
            // Explore all aspects of nested types step by step
            DemonstrateBasicNestedTypes();
            DemonstrateAccessModifiers();
            DemonstrateAccessingPrivateMembers();
            DemonstrateProtectedNestedTypes();
            DemonstrateRealWorldScenarios();
            
            // Show comprehensive access modifier capabilities
            DemonstrateFullAccessModifierRange();
            
            // Cover when to use nested types scenarios
            DemonstrateWhenToUseScenarios();

            Console.WriteLine("\n=== Complete Nested Types Guide Finished! ===");
            Console.WriteLine("Key takeaways:");
            Console.WriteLine("✓ Nested types can access private members of their enclosing type");
            Console.WriteLine("✓ They support the full range of access modifiers");
            Console.WriteLine("✓ Default to private accessibility (unlike non-nested types)");
            Console.WriteLine("✓ External access requires qualified names");
            Console.WriteLine("✓ Best used for: access control, intimate collaboration, logical grouping");
            Console.ReadKey();
        }

        /// <summary>
        /// Foundation concepts - the basic illustrations from the course material
        /// Shows the simple syntax and core features that make nested types special
        /// </summary>
        static void DemonstrateFoundationConcepts()
        {
            Console.WriteLine("=== FOUNDATION CONCEPTS ===");
            Console.WriteLine("Starting with the basic illustrations from the course material\n");
            
            // Simple TopLevel example from material
            var topLevel = new TopLevel();
            topLevel.ShowNestedAccess();
            
            // Inheritance access demonstration
            SubTopLevel.DemonstrateInheritedAccess();
            
            // External qualified access
            SimpleExternalAccessDemo.DemonstrateQualifiedAccess();
            
            // All types can be nested
            ComprehensiveNestingDemo.DemonstrateAllNestedTypes();
            
            // Default accessibility (private)
            DefaultAccessibilityDemo.DemonstrateDefaultAccess();
            
            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Basic nested types - the foundation concept
        /// Any type can live inside another type: classes, structs, enums, interfaces, delegates
        /// </summary>
        static void DemonstrateBasicNestedTypes()
        {
            Console.WriteLine("1. Basic Nested Types - Types Living Inside Other Types:");
            
            // Create instances of nested types - notice the syntax
            var outerInstance = new OuterClass();
            var nestedInstance = new OuterClass.NestedClass();
            
            Console.WriteLine($"Outer class type: {outerInstance.GetType().Name}");
            Console.WriteLine($"Nested class type: {nestedInstance.GetType().Name}");
            Console.WriteLine($"Nested class full name: {nestedInstance.GetType().FullName}");
            
            // Call methods to show they work normally
            nestedInstance.ShowNestedInfo();
            outerInstance.DemonstrateNestedTypes();
            
            // Demonstrate using nested types from outside
            var consumer = new NestedTypeConsumer();
            consumer.UseNestedTypes();
            
            Console.WriteLine("✅ Nested types work just like regular types, but with special access privileges!");
            Console.WriteLine();
        }

        /// <summary>
        /// Access modifiers determine who can see and use your nested types
        /// This is where nested types really shine - you control the visibility!
        /// </summary>
        static void DemonstrateAccessModifiers()
        {
            Console.WriteLine("2. Access Modifiers - Controlling Nested Type Visibility:");
            
            // Public nested type - anyone can access it
            var publicNested = new AccessModifierDemo.PublicNested();
            publicNested.ShowPublicAccess();
            
            // Internal nested type - only within this assembly
            var internalNested = new AccessModifierDemo.InternalNested();
            internalNested.ShowCompanySecrets();
            
            // Demonstrate all access levels
            var accessDemo = new AccessModifierDemo();
            accessDemo.DemonstrateAllAccess();
            
            // Show derived class access
            var derivedDemo = new DerivedAccessDemo();
            derivedDemo.TestInheritedAccess();
            
            // Show external access limitations
            var externalDemo = new ExternalAccessDemo();
            externalDemo.TestExternalAccess();
            
            Console.WriteLine("✅ Access modifiers give you fine-grained control over nested type visibility");
            Console.WriteLine("✅ Use public for types others need, private for internal helpers");
            Console.WriteLine();
        }

        /// <summary>
        /// The superpower of nested types: accessing private members of the outer class
        /// This is what makes nested types special - they're like family members with house keys!
        /// </summary>
        static void DemonstrateAccessingPrivateMembers()
        {
            Console.WriteLine("3. Accessing Private Members - The Nested Type Superpower:");
            
            var bankAccount = new BankAccount("ACC-123", 1000m);
            
            // The nested transaction class can access private fields of BankAccount
            bankAccount.Deposit(500m, "Salary deposit");
            bankAccount.Withdraw(200m, "ATM withdrawal");
            bankAccount.ChargeFee(5m, "Monthly maintenance fee");
            
            bankAccount.PrintStatement();
            bankAccount.RunSecurityAudit();
            
            // Demonstrate secure data container
            var container = new SecureDataContainer<string>(5);
            var accessor = container.GetSecureAccessor();
            
            accessor.StoreSecurely(0, "Public data", false);
            accessor.StoreSecurely(1, "Secret data", true);
            accessor.ShowSecurityInfo();
            
            Console.WriteLine("✅ Nested types can access ALL private members of their containing type");
            Console.WriteLine("✅ This creates tight coupling - use it when types truly belong together");
            Console.WriteLine();
        }

        /// <summary>
        /// Protected nested types work with inheritance - family access only!
        /// Child classes inherit access to parent's protected nested types
        /// </summary>
        static void DemonstrateProtectedNestedTypes()
        {
            Console.WriteLine("4. Protected Nested Types - Inheritance-Friendly Access:");
            
            // Basic employee
            var basicEmployee = new Employee("EMP001", "Alice Johnson", 50000);
            basicEmployee.ShowEmployeeInfo();
            
            // Manager inherits and extends functionality
            var manager = new Manager("MGR001", "Bob Smith", 75000, 0.15m);
            manager.ShowEmployeeInfo();
            manager.ManageTeam();
            
            // Add some team members
            manager.AddDirectReport(basicEmployee);
            manager.ConductTeamReview();
            
            // Executive further extends the hierarchy
            var executive = new Executive("EXE001", "Carol Davis", 120000, 0.25m, 50000, "Technology");
            executive.ShowEmployeeInfo();
            executive.HandleExecutiveResponsibilities();
            
            // Show external access limitations
            var externalHR = new ExternalHRSystem();
            externalHR.TryToAccessProtectedTypes();
            
            Console.WriteLine("✅ Protected nested types are perfect for inheritance hierarchies");
            Console.WriteLine("✅ Child classes get access to parent's protected nested types");
            Console.WriteLine();
        }

        /// <summary>
        /// Real-world examples showing practical applications of nested types
        /// These patterns are used in production systems every day!
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("5. Real-World Scenarios - Where Nested Types Excel:");
            
            // Scenario 1: Database configuration with connection pooling
            var dbConfig = new DatabaseConfig("localhost", "MyApp", "user", "password");
            dbConfig.TestDatabaseOperations();
            
            // Scenario 2: Builder pattern with nested builder
            var pizza = Pizza.CreateBuilder()
                .WithSize(Pizza.SizeType.Large)
                .WithCrust(Pizza.CrustType.Thin)
                .AddToppings("Pepperoni", "Mushrooms", "Extra Cheese")
                .Build();
            
            pizza.ShowOrder();
            
            // Scenario 3: State machine with nested states
            var orderProcessor = new OrderProcessor("ORD-12345");
            orderProcessor.AddItem("PROD-001", "Laptop", 1, 999.99m);
            orderProcessor.AddItem("PROD-002", "Mouse", 2, 29.99m);
            
            orderProcessor.ShowOrderDetails();
            orderProcessor.ProcessOrder();
            orderProcessor.ShipOrder();
            orderProcessor.CompleteOrder();
            orderProcessor.ShowOrderDetails();
            
            Console.WriteLine("✅ Nested types excel in builders, state machines, and configuration classes");
            Console.WriteLine("✅ They provide clean APIs while hiding implementation complexity");
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrate the full range of access modifiers available to nested types
        /// This shows the fine-grained control that nested types provide
        /// </summary>
        static void DemonstrateFullAccessModifierRange()
        {
            Console.WriteLine("=== FULL ACCESS MODIFIER RANGE ===");
            Console.WriteLine("Nested types can use ALL access modifiers - something non-nested types cannot do\n");
            
            // Demonstrate all access levels
            var fullDemo = new FullAccessModifierDemo();
            fullDemo.DemonstrateAllAccessLevels();
            
            // Show inheritance access
            var derivedDemo = new DerivedFullAccessDemo();
            derivedDemo.TestInheritedAccess();
            
            // Show external access limitations
            var externalDemo = new ExternalFullAccessTest();
            externalDemo.TestExternalAccess();
            
            // Compare with non-nested types
            NestedVsNonNestedDemo.DemonstrateTheDifference();
            
            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Demonstrate when to use nested types with real-world scenarios
        /// Shows practical applications and best practices
        /// </summary>
        static void DemonstrateWhenToUseScenarios()
        {
            Console.WriteLine("=== WHEN TO USE NESTED TYPES ===");
            Console.WriteLine("Real-world scenarios where nested types excel\n");
            
            // Run all the "when to use" scenarios
            WhenToUseDemo.DemonstrateAllScenarios();
            
            Console.WriteLine(new string('=', 70) + "\n");
        }
    }
}
