using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates virtual function members and method overriding.
    /// Virtual members allow base classes to provide default implementations
    /// that derived classes can choose to override with their own specialized versions.
    /// 
    /// From material: "Virtual function members allow a base class to define a method 
    /// that its subclasses can choose to override and provide their own specialized implementation."
    /// </summary>

    // Base class with virtual members
    public class AssetWithVirtual
    {
        public string Name;

        public AssetWithVirtual(string name)
        {
            Name = name;
        }

        // Virtual property with default implementation
        // From material: "public virtual decimal Liability => 0; // Virtual property with a default implementation"
        public virtual decimal Liability => 0;

        // Virtual method that can be overridden
        public virtual void DisplayAssetInfo()
        {
            Console.WriteLine($"Generic Asset: {Name}");
            Console.WriteLine($"Liability: ${Liability:F2}");
        }

        // Virtual method for calculating value
        public virtual decimal CalculateValue()
        {
            Console.WriteLine("Using base class value calculation");
            return 0; // Default implementation
        }
    }

    // Stock class demonstrating selective overriding
    public class StockWithVirtual : AssetWithVirtual
    {
        public long SharesOwned;
        public decimal CurrentPrice;

        public StockWithVirtual(string name, long shares, decimal price) : base(name)
        {
            SharesOwned = shares;
            CurrentPrice = price;
        }

        // From material: "Stock doesn't need to override Liability, it uses Asset's default (0)."
        // Notice we're NOT overriding Liability here - we use the base class default

        // Override the virtual DisplayAssetInfo method
        public override void DisplayAssetInfo()
        {
            Console.WriteLine($"Stock: {Name}");
            Console.WriteLine($"Shares Owned: {SharesOwned:N0}");
            Console.WriteLine($"Current Price: ${CurrentPrice:F2}");
            Console.WriteLine($"Total Value: ${CalculateValue():F2}");
            Console.WriteLine($"Liability: ${Liability:F2}"); // Uses base class implementation (0)
        }

        // Override the virtual CalculateValue method
        public override decimal CalculateValue()
        {
            Console.WriteLine("Using stock-specific value calculation");
            return SharesOwned * CurrentPrice;
        }
    }

    // House class demonstrating property override
    public class HouseWithVirtual : AssetWithVirtual
    {
        public decimal Mortgage;
        public decimal EstimatedValue;

        public HouseWithVirtual(string name, decimal mortgage, decimal value) : base(name)
        {
            Mortgage = mortgage;
            EstimatedValue = value;
        }

        // From material: "House overrides Liability"
        public override decimal Liability => Mortgage;

        // Override the virtual DisplayAssetInfo method
        public override void DisplayAssetInfo()
        {
            Console.WriteLine($"House: {Name}");
            Console.WriteLine($"Estimated Value: ${EstimatedValue:F2}");
            Console.WriteLine($"Mortgage: ${Mortgage:F2}");
            Console.WriteLine($"Equity: ${CalculateValue():F2}");
            Console.WriteLine($"Liability: ${Liability:F2}"); // Uses overridden implementation
        }

        // Override the virtual CalculateValue method
        public override decimal CalculateValue()
        {
            Console.WriteLine("Using house-specific value calculation (equity)");
            return EstimatedValue - Mortgage; // Calculate equity
        }
    }

    /// <summary>
    /// Demonstrates covariant return types (C# 9+)
    /// From material: "From C# 9, you can override a method and specify a more derived return type"
    /// </summary>
    public class AssetWithClone
    {
        public string Name;

        public AssetWithClone(string name)
        {
            Name = name;
        }

        // From material: "public virtual Asset Clone() => new Asset { Name = Name }; // Returns Asset"
        public virtual AssetWithClone Clone()
        {
            Console.WriteLine("Cloning base Asset");
            return new AssetWithClone(Name);
        }
    }

    public class HouseWithClone : AssetWithClone
    {
        public decimal Mortgage;

        public HouseWithClone(string name, decimal mortgage) : base(name)
        {
            Mortgage = mortgage;
        }

        // From material: Covariant return type - returning House instead of Asset
        // "public override House Clone() => new House { Name = Name, Mortgage = Mortgage };"
        public override HouseWithClone Clone()
        {
            Console.WriteLine("Cloning House with covariant return type");
            return new HouseWithClone(Name, Mortgage);
        }
    }

    /// <summary>
    /// Class to demonstrate virtual method behavior and polymorphism
    /// </summary>
    public static class VirtualMethodDemo
    {
        public static void DemonstrateVirtualMethods()
        {
            Console.WriteLine("=== Virtual Methods and Override ===");

            // Create instances
            var stock = new StockWithVirtual("MSFT", 1000, 350.75m);
            var house = new HouseWithVirtual("McMansion", 250000m, 750000m);

            Console.WriteLine("\n--- Direct method calls ---");
            stock.DisplayAssetInfo();
            Console.WriteLine();
            house.DisplayAssetInfo();

            Console.WriteLine("\n--- Polymorphic behavior demonstration ---");
            // From material example showing polymorphism with virtual methods
            // "House mansion = new House { Name = "McMansion", Mortgage = 250000 };
            // Asset a = mansion;"
            AssetWithVirtual mansion = new HouseWithVirtual("Luxury Mansion", 250000m, 1000000m);

            // From material: "Console.WriteLine(mansion.Liability); // Output: 250000 (House's overridden implementation)
            // Console.WriteLine(a.Liability); // Output: 250000 (Polymorphism: Calls House's implementation)"
            Console.WriteLine($"Direct house liability: ${((HouseWithVirtual)mansion).Liability:F2}");
            Console.WriteLine($"Polymorphic liability: ${mansion.Liability:F2}"); // Calls House's override

            Console.WriteLine("\n--- Virtual method calls through base reference ---");
            AssetWithVirtual[] portfolio = { stock, house, mansion };

            foreach (AssetWithVirtual asset in portfolio)
            {
                Console.WriteLine($"\nAsset: {asset.Name}");
                asset.DisplayAssetInfo(); // Calls appropriate override
                Console.WriteLine($"Value: ${asset.CalculateValue():F2}"); // Polymorphic method call
                Console.WriteLine();
            }

            Console.WriteLine("--- Covariant Return Types (C# 9+) ---");
            DemonstrateCovariantReturnTypes();
        }

        static void DemonstrateCovariantReturnTypes()
        {
            var originalHouse = new HouseWithClone("Original House", 200000m);
            
            // Clone returns HouseWithClone, not just AssetWithClone
            HouseWithClone clonedHouse = originalHouse.Clone();
            
            Console.WriteLine($"Original: {originalHouse.Name}, Mortgage: ${originalHouse.Mortgage:F2}");
            Console.WriteLine($"Clone: {clonedHouse.Name}, Mortgage: ${clonedHouse.Mortgage:F2}");
            
            // Can still assign to base type due to covariance
            AssetWithClone assetClone = originalHouse.Clone();
            Console.WriteLine($"Clone as Asset: {assetClone.Name}");
        }
    }
}
