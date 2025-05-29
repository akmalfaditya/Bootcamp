namespace NestedTypes
{
    /// <summary>
    /// Think of nested types as "types within types" - like Russian dolls!
    /// They're perfect when you want to keep related functionality tightly bundled together
    /// and control who gets to see and use your inner workings
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Nested Types: Types Within Types ===\n");

            // Let's explore all aspects of nested types step by step
            DemonstrateBasicNestedTypes();
            DemonstrateAccessModifiers();
            DemonstrateAccessingPrivateMembers();
            DemonstrateProtectedNestedTypes();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== Nested Types Demo Complete! ===");
            Console.ReadKey();
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
    }
}
