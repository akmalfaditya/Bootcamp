using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates polymorphism - the ability for a base class reference
    /// to point to derived class objects and call their specific implementations.
    /// 
    /// From the material: "Polymorphism means 'many forms.' It refers to the ability 
    /// of a variable of a base class type to refer to an object of any of its derived classes."
    /// </summary>

    // Enhanced Asset class with virtual members for polymorphism demonstration
    public class AssetWithPolymorphism
    {
        public string Name;
        
        // Virtual property - derived classes can override this
        // From material: "Virtual function members allow a base class to define a method 
        // that its subclasses can choose to override"
        public virtual decimal Liability => 0; // Default implementation returns 0

        public AssetWithPolymorphism(string name)
        {
            Name = name;
        }

        // Virtual method that can be overridden
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Asset: {Name}");
            Console.WriteLine($"Liability: ${Liability:F2}");
        }

        // Static method for polymorphism demonstration
        // From material: "A method that accepts an Asset parameter can work with any object 
        // that is an Asset, including Stock and House instances"
        public static void Display(AssetWithPolymorphism asset)
        {
            Console.WriteLine($"Displaying asset: {asset.Name}");
            // This will call the appropriate overridden method based on actual object type
            asset.DisplayInfo();
        }
    }

    // Stock with polymorphic behavior
    public class StockWithPolymorphism : AssetWithPolymorphism
    {
        public long SharesOwned;
        public decimal CurrentPrice;

        public StockWithPolymorphism(string name, long shares, decimal price) : base(name)
        {
            SharesOwned = shares;
            CurrentPrice = price;
        }

        // Stock doesn't override Liability - uses base class default (0)
        // This demonstrates that overriding is optional for virtual members

        // Override the virtual DisplayInfo method
        public override void DisplayInfo()
        {
            Console.WriteLine($"Stock: {Name}");
            Console.WriteLine($"Shares: {SharesOwned:N0}");
            Console.WriteLine($"Price: ${CurrentPrice:F2}");
            Console.WriteLine($"Total Value: ${SharesOwned * CurrentPrice:F2}");
            Console.WriteLine($"Liability: ${Liability:F2}");
        }

        public decimal GetTotalValue()
        {
            return SharesOwned * CurrentPrice;
        }
    }

    // House with polymorphic behavior
    public class HouseWithPolymorphism : AssetWithPolymorphism
    {
        public decimal Mortgage;

        public HouseWithPolymorphism(string name, decimal mortgage) : base(name)
        {
            Mortgage = mortgage;
        }

        // Override the Liability property
        // From material: "House overrides Liability"
        public override decimal Liability => Mortgage;

        // Override the virtual DisplayInfo method
        public override void DisplayInfo()
        {
            Console.WriteLine($"House: {Name}");
            Console.WriteLine($"Mortgage: ${Mortgage:F2}");
            Console.WriteLine($"Liability: ${Liability:F2}");
        }
    }

    /// <summary>
    /// Class to demonstrate polymorphic behavior in action
    /// Shows how the same method call can result in different behaviors
    /// </summary>
    public static class PolymorphismDemo
    {
        public static void DemonstratePolymorphism()
        {
            Console.WriteLine("=== Polymorphism Demonstration ===");
            
            // Create different types of assets
            var stock = new StockWithPolymorphism("MSFT", 1000, 350.75m);
            var house = new HouseWithPolymorphism("Mansion", 250000m);

            // From material example: "For instance, a method that accepts an Asset parameter 
            // can work with any object that is an Asset"
            Console.WriteLine("\nUsing polymorphic method calls:");
            AssetWithPolymorphism.Display(stock);    // Calls Display with a Stock object
            Console.WriteLine();
            AssetWithPolymorphism.Display(house);   // Calls Display with a House object

            Console.WriteLine("\n--- Direct polymorphic calls ---");
            
            // Create array of base class references pointing to derived objects
            AssetWithPolymorphism[] portfolio = 
            {
                new StockWithPolymorphism("AAPL", 500, 175.25m),
                new HouseWithPolymorphism("Beach House", 750000m),
                new StockWithPolymorphism("GOOGL", 100, 2750.50m)
            };

            // Demonstrate polymorphism - same method call, different implementations
            Console.WriteLine("Portfolio contents (polymorphic behavior):");
            foreach (AssetWithPolymorphism asset in portfolio)
            {
                asset.DisplayInfo(); // Calls the correct overridden method
                Console.WriteLine($"Liability: ${asset.Liability:F2}"); // Polymorphic property access
                Console.WriteLine();
            }
        }
    }
}
