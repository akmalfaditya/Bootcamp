using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates casting and reference conversions in inheritance hierarchies.
    /// We'll cover upcasting, downcasting, and the 'is' and 'as' operators.
    /// 
    /// From material: "Object references can be converted between compatible types in a hierarchy 
    /// through reference conversions. This doesn't alter the underlying object; it just changes 
    /// the 'view' that the reference variable has of that object."
    /// </summary>

    // Using our existing Asset, Stock, and House classes for casting demonstrations
    public static class CastingDemo
    {
        public static void DemonstrateCasting()
        {
            Console.WriteLine("=== Casting and Reference Conversions ===");

            // Create some objects for our demonstrations
            var stock = new Stock("MSFT", 1000);
            var house = new House("Family Home", 350000m);

            DemonstrateUpcasting(stock, house);
            DemonstrateDowncasting();
            DemonstrateAsOperator();
            DemonstrateIsOperator();
            DemonstratePatternVariable();
        }

        /// <summary>
        /// Demonstrates upcasting - derived to base class reference
        /// From material: "An upcast operation creates a base class reference from a subclass reference.
        /// This conversion is always implicit and always succeeds"
        /// </summary>
        static void DemonstrateUpcasting(Stock stock, House house)
        {
            Console.WriteLine("\n--- Upcasting (Derived to Base) ---");
            
            // Upcasting - always safe and implicit
            Asset assetFromStock = stock;  // Upcast: Stock to Asset
            Asset assetFromHouse = house;  // Upcast: House to Asset

            Console.WriteLine($"Original stock name: {stock.Name}");
            Console.WriteLine($"Upcast asset name: {assetFromStock.Name}");
            Console.WriteLine($"Are they the same object? {object.ReferenceEquals(stock, assetFromStock)}");

            // After upcast, we have restricted view - can only access Asset members
            Console.WriteLine($"Asset name (upcast view): {assetFromStock.Name}");
            // Console.WriteLine($"Shares: {assetFromStock.SharesOwned}"); // This would cause compile error!
            
            Console.WriteLine("Upcasting is always safe - no exceptions possible");
        }

        /// <summary>
        /// Demonstrates downcasting - base to derived class reference
        /// From material: "A downcast operation creates a subclass reference from a base class reference.
        /// This conversion requires an explicit cast because it can potentially fail at runtime"
        /// </summary>
        static void DemonstrateDowncasting()
        {
            Console.WriteLine("\n--- Downcasting (Base to Derived) ---");

            // Create objects and upcast them
            Stock originalStock = new Stock("TSLA", 500);
            Asset asset = originalStock;  // Upcast

            // Downcast back to Stock - requires explicit cast
            // From material: "This conversion requires an explicit cast"
            Stock downcastStock = (Stock)asset;
            
            Console.WriteLine($"Original stock shares: {originalStock.SharesOwned}");
            Console.WriteLine($"Downcast stock shares: {downcastStock.SharesOwned}");
            Console.WriteLine($"Are they the same? {object.ReferenceEquals(originalStock, downcastStock)}");

            // Demonstrate invalid downcast
            Console.WriteLine("\nTrying invalid downcast:");
            House someHouse = new House("Test House", 200000m);
            Asset assetHouse = someHouse;  // Upcast House to Asset

            try
            {
                // This will throw InvalidCastException at runtime
                // From material: "If a downcast is attempted on an object that is not of the target type,
                // an InvalidCastException is thrown at runtime"
                Stock invalidStock = (Stock)assetHouse;  // Trying to cast House to Stock!
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Caught expected exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates the 'as' operator for safe casting
        /// From material: "The as operator performs a downcast but provides a safer alternative.
        /// If the downcast fails, it evaluates to null instead of throwing an InvalidCastException"
        /// </summary>
        static void DemonstrateAsOperator()
        {
            Console.WriteLine("\n--- The 'as' Operator (Safe Casting) ---");

            // Create some test objects
            Asset[] assets = 
            {
                new Stock("AAPL", 100),
                new House("Vacation Home", 500000m),
                new Asset("Generic Asset")
            };

            foreach (Asset asset in assets)
            {
                Console.WriteLine($"\nTesting asset: {asset.Name}");

                // Try to cast to Stock using 'as' operator
                Stock? stockResult = asset as Stock;
                if (stockResult != null)
                {
                    Console.WriteLine($"  Successfully cast to Stock. Shares: {stockResult.SharesOwned}");
                }
                else
                {
                    Console.WriteLine("  Not a Stock - 'as' returned null (no exception)");
                }

                // Try to cast to House using 'as' operator
                House? houseResult = asset as House;
                if (houseResult != null)
                {
                    Console.WriteLine($"  Successfully cast to House. Mortgage: ${houseResult.Mortgage:F2}");
                }
                else
                {
                    Console.WriteLine("  Not a House - 'as' returned null (no exception)");
                }
            }

            Console.WriteLine("\nCaution: Always check for null after using 'as' operator!");
        }

        /// <summary>
        /// Demonstrates the 'is' operator for type checking
        /// From material: "The is operator tests whether a variable matches a pattern.
        /// It checks if a reference conversion would succeed"
        /// </summary>
        static void DemonstrateIsOperator()
        {
            Console.WriteLine("\n--- The 'is' Operator (Type Checking) ---");

            // From material example: "Asset a = new Stock(); // 'a' actually holds a Stock object"
            Asset asset = new Stock("NVDA", 200);

            // From material: "if (a is Stock) // Checks if 'a' can be successfully cast to Stock"
            if (asset is Stock)
            {
                Console.WriteLine("Asset is a Stock - safe to cast");
                // From material: "Console.WriteLine(((Stock)a).SharesOwned); // This block executes"
                Console.WriteLine($"Shares: {((Stock)asset).SharesOwned}");
            }

            if (asset is House)
            {
                Console.WriteLine("Asset is a House");
            }
            else
            {
                Console.WriteLine("Asset is not a House");
            }

            // Test with different object types
            Asset[] testAssets = 
            {
                new Stock("META", 150),
                new House("Downtown Condo", 400000m),
                new Asset("Bond")
            };

            foreach (Asset testAsset in testAssets)
            {
                Console.WriteLine($"\nTesting {testAsset.Name}:");
                Console.WriteLine($"  Is Stock? {testAsset is Stock}");
                Console.WriteLine($"  Is House? {testAsset is House}");
                Console.WriteLine($"  Is Asset? {testAsset is Asset}"); // Always true for objects in hierarchy
            }
        }

        /// <summary>
        /// Demonstrates pattern variables with 'is' operator (C# 7+)
        /// From material: "From C# 7, you can combine the is operator with a variable declaration"
        /// </summary>
        static void DemonstratePatternVariable()
        {
            Console.WriteLine("\n--- Pattern Variables (C# 7+) ---");

            Asset[] portfolio = 
            {
                new Stock("AMZN", 75),
                new House("Mountain Cabin", 300000m),
                new Stock("NFLX", 200)
            };

            foreach (Asset asset in portfolio)
            {
                Console.WriteLine($"\nProcessing {asset.Name}:");

                // From material: "if (a is Stock s) // If 'a' is a Stock, assign it to a new 's' variable"
                if (asset is Stock s)
                {
                    // From material: "Console.WriteLine(s.SharesOwned); // 's' is now available and correctly typed"
                    Console.WriteLine($"  This is a stock with {s.SharesOwned} shares");
                    s.BuyShares(10); // Can call Stock-specific methods
                }
                else if (asset is House h)
                {
                    Console.WriteLine($"  This is a house with ${h.Mortgage:F2} mortgage");
                    h.MakeMortgagePayment(1000); // Can call House-specific methods
                }
                else
                {
                    Console.WriteLine($"  This is a generic asset");
                }
            }

            Console.WriteLine("\nPattern variables remain in scope and are properly typed!");
        }
    }
}
