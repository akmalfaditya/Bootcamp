using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates member hiding vs overriding, and the 'base' keyword.
    /// It's crucial to understand the difference between 'new' (hiding) and 'override' (polymorphism).
    /// 
    /// From material: "It's crucial to understand the difference between new (hiding) and override (polymorphism)."
    /// </summary>

    // Field hiding examples
    public class ParentClass
    {
        public int Counter = 1;
        public virtual void ShowMessage()
        {
            Console.WriteLine("Parent message");
        }
    }

    public class ChildClass : ParentClass
    {
        public new int Counter = 2; // Hiding parent field
        public new void ShowMessage() // Hiding parent method
        {
            Console.WriteLine("Child message");
        }
    }

    // Enhanced asset classes for virtual/override demos
    public class AdvancedAsset : Asset
    {
        public virtual decimal Liability => 0;
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Asset: {Name}");
        }
    }

    public class AdvancedStock : AdvancedAsset
    {
        public long SharesOwned;
        public decimal CurrentPrice;
        
        public override decimal Liability => 0; // Stocks typically have no liability
        
        public override void DisplayInfo()
        {
            Console.WriteLine($"Stock: {Name}, Shares: {SharesOwned}, Price: ${CurrentPrice:N2}");
        }
    }

    public class AdvancedHouse : AdvancedAsset
    {
        public decimal Mortgage;
        public decimal EstimatedValue;
        
        public override decimal Liability => Mortgage;
        
        public override void DisplayInfo()
        {
            Console.WriteLine($"House: {Name}, Value: ${EstimatedValue:N2}, Mortgage: ${Mortgage:N2}");
        }
    }

    // Base keyword demonstration
    public class SmartHouse : AdvancedHouse
    {
        public decimal SmartDevicesCost;
        
        public override decimal Liability => base.Liability + SmartDevicesCost;
        
        public override void DisplayInfo()
        {
            base.DisplayInfo(); // Call parent method first
            Console.WriteLine($"  Smart Devices Cost: ${SmartDevicesCost:N2}");
            Console.WriteLine($"  Total Liability: ${Liability:N2}");
        }
    }

    // Enhanced asset with constructor base call
    public class EnhancedAsset : Asset
    {
        public string Category;
        
        public EnhancedAsset(string name, string category)
        {
            Name = name;
            Category = category;
        }
        
        public void DisplayFullInfo()
        {
            Console.WriteLine($"Enhanced Asset: {Name} ({Category})");
        }
    }
}
