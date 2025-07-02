namespace Inheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Inheritance Concepts Demo ===\n");

            // Basic inheritance and subclassing
            DemonstrateBasicInheritance();
            
            // Polymorphism in action
            DemonstratePolymorphism();
            
            // Casting and reference conversions
            DemonstrateCasting();
            
            // Virtual and overridden members
            DemonstrateVirtualOverride();
            
            // Abstract classes and members
            DemonstrateAbstractClasses();
            
            // Member hiding with 'new' keyword
            DemonstrateMemberHiding();
            
            // Using the 'base' keyword
            DemonstrateBaseKeyword();
            
            // Constructor inheritance patterns
            DemonstrateConstructorInheritance();
            
            // Required members (C# 11 feature)
            DemonstrateRequiredMembers();
            
            // Sealed classes and methods
            DemonstrateSealedConcepts();
            
            // Primary constructors (C# 12 feature)
            DemonstratePrimaryConstructors();

            Console.WriteLine("\n=== Inheritance Demo Complete ===");
            Console.ReadKey();
        }

        static void DemonstrateBasicInheritance()
        {
            Console.WriteLine("1. Basic Inheritance:");
            
            // Creating instances of derived classes
            var msftStock = new Stock { Name = "Microsoft", SharesOwned = 1000 };
            var myHouse = new House { Name = "Family Home", Mortgage = 250000 };
            
            // Both have inherited the Name property from Asset
            Console.WriteLine($"Stock: {msftStock.Name}, Shares: {msftStock.SharesOwned}");
            Console.WriteLine($"House: {myHouse.Name}, Mortgage: ${myHouse.Mortgage:N0}");
            Console.WriteLine();
        }

        static void DemonstratePolymorphism()
        {
            Console.WriteLine("2. Polymorphism:");
            
            // Same method works with different types - that's polymorphism!
            var stock = new Stock { Name = "Apple", SharesOwned = 500 };
            var house = new House { Name = "Beach House", Mortgage = 180000 };
            
            DisplayAsset(stock);  // Asset method works with Stock
            DisplayAsset(house);  // Asset method works with House
            
            // Array of different asset types - polymorphism at work
            Asset[] portfolio = { stock, house };
            Console.WriteLine("Portfolio summary:");
            foreach (Asset asset in portfolio)
            {
                DisplayAsset(asset);
            }
            Console.WriteLine();
        }

        static void DisplayAsset(Asset asset)
        {
            // This method accepts any Asset or its subclasses
            Console.WriteLine($"Asset: {asset.Name}");
        }

        static void DemonstrateCasting()
        {
            Console.WriteLine("3. Casting and Reference Conversions:");
            
            // Upcasting - automatic and safe
            Stock originalStock = new Stock { Name = "Tesla", SharesOwned = 200 };
            Asset assetReference = originalStock;  // Upcasting happens automatically
            Console.WriteLine($"Upcast asset name: {assetReference.Name}");
            
            // Downcasting - explicit and potentially dangerous
            if (assetReference is Stock downcastStock)
            {
                Console.WriteLine($"Downcast successful! Shares: {downcastStock.SharesOwned}");
            }
            
            // Using 'as' operator for safe casting
            Asset someAsset = new House { Name = "Condo", Mortgage = 120000 };
            Stock? maybeStock = someAsset as Stock;  // Returns null if cast fails
            
            if (maybeStock != null)
            {
                Console.WriteLine($"It's a stock: {maybeStock.SharesOwned}");
            }
            else
            {
                Console.WriteLine("Not a stock - cast returned null");
            }
            
            // Pattern matching with 'is' - modern C# way
            if (someAsset is House { Mortgage: > 100000 } expensiveHouse)
            {
                Console.WriteLine($"Expensive house found: {expensiveHouse.Name}");
            }
            Console.WriteLine();
        }

        static void DemonstrateVirtualOverride()
        {
            Console.WriteLine("4. Virtual and Override:");
            
            var portfolio = new AdvancedAsset[] 
            {
                new AdvancedStock { Name = "Amazon", SharesOwned = 100, CurrentPrice = 150.50m },
                new AdvancedHouse { Name = "Mansion", Mortgage = 800000, EstimatedValue = 1200000 }
            };
            
            foreach (var asset in portfolio)
            {
                // Each subclass provides its own implementation
                Console.WriteLine($"{asset.Name}: Liability = ${asset.Liability:N2}");
                asset.DisplayInfo();  // Virtual method called
            }
            Console.WriteLine();
        }

        static void DemonstrateAbstractClasses()
        {
            Console.WriteLine("5. Abstract Classes:");
            
            // Can't instantiate AbstractAsset directly - it's abstract!
            // AbstractAsset asset = new AbstractAsset(); // This would cause compiler error
            
            var investments = new AbstractAsset[]
            {
                new RealStock("Google") { SharesOwned = 50, CurrentPrice = 2800.75m },
                new RealEstate("Office Building") { PurchasePrice = 2000000, CurrentValue = 2500000 }
            };
            
            foreach (var investment in investments)
            {
                // NetValue is abstract - each subclass MUST implement it
                Console.WriteLine($"{investment.Name}: Net Value = ${investment.NetValue:N2}");
                Console.WriteLine($"Description: {investment.GetDescription()}");
            }
            Console.WriteLine();
        }

        static void DemonstrateMemberHiding()
        {
            Console.WriteLine("6. Member Hiding with 'new':");
            
            var child = new ChildClass();
            var parent = new ParentClass();
            
            // Direct calls - each class uses its own version
            parent.ShowMessage();  // Parent version
            child.ShowMessage();   // Child version (hidden)
            
            // Reference as parent type - parent version is called
            ParentClass childAsParent = new ChildClass();
            childAsParent.ShowMessage();  // Still parent version!
            
            Console.WriteLine($"Parent counter: {parent.Counter}");
            Console.WriteLine($"Child counter: {child.Counter}");
            Console.WriteLine();
        }

        static void DemonstrateBaseKeyword()
        {
            Console.WriteLine("7. Using 'base' Keyword:");
            
            var smartHouse = new SmartHouse 
            { 
                Name = "Tech House", 
                Mortgage = 500000, 
                SmartDevicesCost = 25000 
            };
            
            // This calls the overridden method that uses base.Liability
            Console.WriteLine($"Smart House Total Liability: ${smartHouse.Liability:N2}");
            
            // Demonstrate base constructor call
            var enhancedAsset = new EnhancedAsset("Investment Property", "Real Estate Portfolio");
            enhancedAsset.DisplayFullInfo();
            Console.WriteLine();
        }

        static void DemonstrateConstructorInheritance()
        {
            Console.WriteLine("8. Constructor Inheritance:");
            
            // Different constructor patterns
            var baseExample = new BaseExample();  // Uses parameterless constructor
            var derivedExample1 = new DerivedExample(42);  // Calls base constructor explicitly
            var derivedExample2 = new DerivedExample();  // Uses parameterless, calls base implicitly
            
            Console.WriteLine($"Base X: {baseExample.X}");
            Console.WriteLine($"Derived1 X: {derivedExample1.X}, Y: {derivedExample1.Y}");
            Console.WriteLine($"Derived2 X: {derivedExample2.X}, Y: {derivedExample2.Y}");
            Console.WriteLine();
        }

        static void DemonstrateRequiredMembers()
        {
            Console.WriteLine("9. Required Members (C# 11):");
            
            // These compilations work because required members are provided
            var modernAsset1 = new ModernAsset { Name = "Bitcoin", AssetType = "Cryptocurrency" };
            var modernAsset2 = new ModernAsset { Name = "Gold", AssetType = "Precious Metal" };
            
            Console.WriteLine($"Modern Asset 1: {modernAsset1.Name} ({modernAsset1.AssetType})");
            Console.WriteLine($"Modern Asset 2: {modernAsset2.Name} ({modernAsset2.AssetType})");
            
            // This would cause compilation error:
            // var invalidAsset = new ModernAsset(); // Missing required members!
            Console.WriteLine();
        }

        static void DemonstrateSealedConcepts()
        {
            Console.WriteLine("10. Sealed Classes and Methods:");
            
            var sealedHouse = new SealedHouse 
            { 
                Name = "Final House", 
                Mortgage = 300000 
            };
            
            Console.WriteLine($"Sealed house liability: ${sealedHouse.Liability:N2}");
            sealedHouse.DisplayInfo();  // This method is sealed - can't be overridden further
            
            // SealedClass cannot be inherited
            var sealedObject = new SealedClass("Sealed Object");
            sealedObject.DoSomething();
            Console.WriteLine();
        }

        static void DemonstratePrimaryConstructors()
        {
            Console.WriteLine("11. Primary Constructors (C# 12):");
            
            var modernBase = new ModernBaseClass(100);
            var modernDerived = new ModernDerivedClass(200, 300);
            
            Console.WriteLine($"Modern Base X: {modernBase.X}");
            Console.WriteLine($"Modern Derived X: {modernDerived.X}, Y: {modernDerived.Y}");
            
            // Clean syntax - no boilerplate constructor code needed!
            Console.WriteLine();
        }
    }
}
