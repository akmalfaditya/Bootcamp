using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates the fundamental concepts of inheritance in C#.
    /// We'll start with a simple Asset base class and build derived classes from it.
    /// Remember: inheritance represents an "is-a" relationship.
    /// 
    /// From the material: "Stock and House are derived classes, and Asset is the base class. 
    /// Both Stock and House automatically get the Name field from Asset, in addition to their own unique members."
    /// </summary>

    // Base class - this is our foundation
    // Every asset has a name, that's common to all types of assets
    public class Asset
    {
        // Simple field that all derived classes will inherit
        // Notice this is exactly like the material example - just a public field
        public string Name = string.Empty;

        // Default constructor
        public Asset()
        {
            Console.WriteLine("Asset base constructor called");
        }

        // Constructor with name parameter
        public Asset(string name)
        {
            Name = name;
            Console.WriteLine($"Asset constructor called for: {name}");
        }
    }

    // Stock inherits from Asset using the colon (:) syntax
    // A Stock "is-a" Asset, so inheritance makes sense here
    // From material: "Stock inherits from Asset"
    public class Stock : Asset
    {
        // Stock-specific field - exactly as shown in material
        public long SharesOwned;

        // Default constructor - implicitly calls base constructor
        public Stock()
        {
            Console.WriteLine("Stock constructor called");
        }

        // Constructor with parameters
        public Stock(string name, long shares) : base(name)
        {
            SharesOwned = shares;
            Console.WriteLine($"Stock created: {shares} shares of {name}");
        }

        // Method specific to Stock - demonstrates behavior specific to this derived class
        public void BuyShares(long additionalShares)
        {
            SharesOwned += additionalShares;
            Console.WriteLine($"Bought {additionalShares} shares of {Name}. Total: {SharesOwned}");
        }

        public void SellShares(long sharesToSell)
        {
            if (sharesToSell <= SharesOwned)
            {
                SharesOwned -= sharesToSell;
                Console.WriteLine($"Sold {sharesToSell} shares of {Name}. Remaining: {SharesOwned}");
            }
            else
            {
                Console.WriteLine($"Cannot sell {sharesToSell} shares - only have {SharesOwned}");
            }
        }
    }

    // House also inherits from Asset
    // A House "is-a" Asset as well
    // From material: "House also inherits from Asset"
    public class House : Asset
    {
        // House-specific field - exactly as shown in material
        public decimal Mortgage;

        // Default constructor
        public House()
        {
            Console.WriteLine("House constructor called");
        }

        // Constructor with parameters
        public House(string name, decimal mortgage) : base(name)
        {
            Mortgage = mortgage;
            Console.WriteLine($"House created: {name} with ${mortgage:C} mortgage");
        }

        // Method specific to House
        public void MakeMortgagePayment(decimal amount)
        {
            if (amount > 0 && amount <= Mortgage)
            {
                Mortgage -= amount;
                Console.WriteLine($"Mortgage payment of ${amount:C} made on {Name}. Remaining: ${Mortgage:C}");
            }
            else
            {
                Console.WriteLine($"Invalid payment amount: ${amount:C}");
            }
        }
    }
}
