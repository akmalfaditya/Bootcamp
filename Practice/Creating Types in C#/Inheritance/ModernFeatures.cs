using System;

namespace Inheritance
{
    /// <summary>
    /// Modern asset class demonstrating required members (C# 11 feature)
    /// Required members MUST be initialized when creating an object
    /// This prevents objects from being created in invalid states
    /// </summary>
    public class ModernAsset
    {
        // Required members must be set during object creation
        // Compiler will force you to initialize these
        public required string Name;
        public required string AssetType;

        // Optional members work as before
        public decimal Value;
        public DateTime PurchaseDate = DateTime.Now;

        /// <summary>
        /// Method to validate that required members are properly set
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(AssetType);
        }

        /// <summary>
        /// Display asset information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"Modern Asset: {Name} ({AssetType})");
            Console.WriteLine($"Value: ${Value:N2}, Purchased: {PurchaseDate:yyyy-MM-dd}");
        }
    }

    /// <summary>
    /// Derived class with additional required members
    /// Shows how required members work with inheritance
    /// </summary>
    public class ModernStock : ModernAsset
    {
        // Additional required member specific to stocks
        public required string TickerSymbol;
        
        // Optional stock-specific members
        public long SharesOwned;
        public decimal CurrentPrice;

        /// <summary>
        /// Calculate total stock value
        /// </summary>
        public decimal GetTotalValue()
        {
            return SharesOwned * CurrentPrice;
        }

        /// <summary>
        /// Override display to include stock-specific info
        /// </summary>
        public new void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Ticker: {TickerSymbol}, Shares: {SharesOwned:N0}, Price: ${CurrentPrice:N2}");
            Console.WriteLine($"Total Value: ${GetTotalValue():N2}");
        }
    }

    /// <summary>
    /// Class with required member and constructor interaction
    /// Shows how required members work with constructors
    /// </summary>
    public class ModernVehicle
    {
        public required string Make;
        public required string Model;
        public required int Year;
        
        public string? Color; // Nullable - not required
        public decimal Mileage;

        /// <summary>
        /// Constructor that works with required members
        /// Required members can be set via object initializer even with constructor
        /// </summary>
        public ModernVehicle()
        {
            Mileage = 0;
            Console.WriteLine("ModernVehicle constructor called");
        }

        /// <summary>
        /// Constructor that initializes some required members
        /// Remaining required members must still be set via object initializer
        /// </summary>
        public ModernVehicle(string make, string model)
        {
            Make = make;
            Model = model;
            Year = DateTime.Now.Year; // Default to current year
            Mileage = 0;
            Console.WriteLine($"ModernVehicle constructor called with {make} {model}");
        }

        public void DisplayVehicleInfo()
        {
            Console.WriteLine($"{Year} {Make} {Model}");
            Console.WriteLine($"Color: {Color ?? "Not specified"}, Mileage: {Mileage:N0}");
        }
    }

    /// <summary>
    /// Primary constructor example (C# 12 feature)
    /// Clean syntax for constructor parameters
    /// </summary>
    public class ModernBaseClass(int x)
    {
        // Property initialized from primary constructor parameter
        public int X = x;
        
        // Additional properties
        public string Description = $"Base class with X = {x}";

        public void DisplayBase()
        {
            Console.WriteLine($"ModernBaseClass: X = {X}, Description = {Description}");
        }
    }

    /// <summary>
    /// Derived class with primary constructor
    /// Shows inheritance with primary constructors
    /// </summary>
    public class ModernDerivedClass(int x, int y) : ModernBaseClass(x)
    {
        // Property initialized from primary constructor parameter
        public int Y = y;
        
        // Computed property using both parameters
        public int Sum = x + y;

        public void DisplayDerived()
        {
            DisplayBase(); // Call base method
            Console.WriteLine($"ModernDerivedClass: Y = {Y}, Sum = {Sum}");
        }
    }

    /// <summary>
    /// Advanced primary constructor with required members
    /// Combines C# 11 and C# 12 features
    /// </summary>
    public class ModernProduct(string name, decimal price)
    {
        // Required member (must be set via object initializer)
        public required string Category;
        
        // Properties from primary constructor
        public string Name = name;
        public decimal Price = price;
        
        // Optional properties
        public string? Description;
        public bool IsActive = true;

        /// <summary>
        /// Method using primary constructor parameters and properties
        /// </summary>
        public void DisplayProduct()
        {
            Console.WriteLine($"Product: {Name} ({Category})");
            Console.WriteLine($"Price: ${Price:N2}, Active: {IsActive}");
            if (!string.IsNullOrEmpty(Description))
            {
                Console.WriteLine($"Description: {Description}");
            }
        }

        /// <summary>
        /// Calculate price with tax
        /// </summary>
        public decimal GetPriceWithTax(decimal taxRate = 0.08m)
        {
            return Price * (1 + taxRate);
        }
    }

    /// <summary>
    /// Class demonstrating record-like behavior with inheritance
    /// Shows modern C# patterns for data classes
    /// </summary>
    public class ModernDataClass
    {
        public required string Id;
        public required string Name;
        public DateTime CreatedAt = DateTime.Now;
        public DateTime? UpdatedAt;

        /// <summary>
        /// Update method that tracks when object was modified
        /// </summary>
        public void Update(string newName)
        {
            Name = newName;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Check if object has been modified
        /// </summary>
        public bool IsModified => UpdatedAt.HasValue;

        /// <summary>
        /// Get object age
        /// </summary>
        public TimeSpan Age => DateTime.Now - CreatedAt;

        public override string ToString()
        {
            string modifiedInfo = IsModified ? $", Modified: {UpdatedAt:yyyy-MM-dd HH:mm}" : "";
            return $"{Name} (ID: {Id}, Created: {CreatedAt:yyyy-MM-dd HH:mm}{modifiedInfo})";
        }
    }
}
