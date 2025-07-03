using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates the most basic form of inheritance in C#.
    /// Think of inheritance as a "is-a" relationship. A Stock IS AN Asset, a House IS AN Asset.
    /// The key point here is code reuse - we define common properties once in the base class,
    /// and all derived classes automatically get those properties.
    /// </summary>

    // This is our base class - the foundation that other classes will build upon
    public class BasicAsset
    {
        // Every asset has a name - this will be inherited by all subclasses
        public string Name = string.Empty;
        
        // Let's add a creation date that all assets will have
        public DateTime AcquisitionDate { get; set; } = DateTime.Now;
        
        // A simple method that all assets can use
        public void DisplayBasicInfo()
        {
            Console.WriteLine($"Asset: {Name}, Acquired: {AcquisitionDate:yyyy-MM-dd}");
        }
    }

    // Stock class inherits from BasicAsset using the colon (:) syntax
    // This means Stock "is-a" BasicAsset, so it gets all BasicAsset's members plus its own
    public class BasicStock : BasicAsset
    {
        // Stock-specific properties that only Stock has
        public long SharesOwned { get; set; }
        public decimal CurrentPrice { get; set; }
        public string TickerSymbol { get; set; } = string.Empty;
        
        // Calculate the total value of this stock holding
        public decimal TotalValue => SharesOwned * CurrentPrice;
        
        // A method specific to stocks
        public void ShowStockDetails()
        {
            Console.WriteLine($"Stock Details: {TickerSymbol} - {SharesOwned} shares at ${CurrentPrice:F2} = ${TotalValue:F2}");
        }
    }

    // House class also inherits from BasicAsset
    // Like Stock, it gets BasicAsset's members but adds its own house-specific stuff
    public class BasicHouse : BasicAsset
    {
        // House-specific properties
        public decimal Mortgage { get; set; }
        public decimal MarketValue { get; set; }
        public string Address { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        
        // Calculate equity (what you actually own vs what you owe)
        public decimal Equity => MarketValue - Mortgage;
        
        // House-specific method
        public void ShowHouseDetails()
        {
            Console.WriteLine($"House: {Address}, Value: ${MarketValue:F2}, Mortgage: ${Mortgage:F2}, Equity: ${Equity:F2}");
        }
    }

    // Let's add one more type to show how inheritance scales
    public class BasicBond : BasicAsset
    {
        public decimal FaceValue { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime MaturityDate { get; set; }
        
        public decimal AnnualInterest => FaceValue * (InterestRate / 100);
        
        public void ShowBondDetails()
        {
            Console.WriteLine($"Bond: {Name}, Face Value: ${FaceValue:F2}, Rate: {InterestRate:F2}%, Annual Interest: ${AnnualInterest:F2}");
        }
    }

    /// <summary>
    /// Demonstration class to show basic inheritance in action
    /// </summary>
    public static class BasicInheritanceDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== BASIC INHERITANCE DEMONSTRATION ===\n");
            
            // Create instances of our derived classes
            var microsoftStock = new BasicStock
            {
                Name = "Microsoft Corporation",           // Inherited from BasicAsset
                TickerSymbol = "MSFT",                   // Stock-specific
                SharesOwned = 100,                       // Stock-specific
                CurrentPrice = 350.75m                   // Stock-specific
            };
            
            var dreamHouse = new BasicHouse
            {
                Name = "Dream House",                     // Inherited from BasicAsset
                Address = "123 Main Street, Seattle",    // House-specific
                MarketValue = 750000,                     // House-specific
                Mortgage = 400000,                        // House-specific
                Bedrooms = 4                              // House-specific
            };
            
            var treasuryBond = new BasicBond
            {
                Name = "US Treasury Bond",                // Inherited from BasicAsset
                FaceValue = 10000,                        // Bond-specific
                InterestRate = 3.5m,                      // Bond-specific
                MaturityDate = DateTime.Now.AddYears(10) // Bond-specific
            };
            
            // Notice how all objects can call methods from both base and derived classes
            Console.WriteLine("1. Inherited methods work on all objects:");
            microsoftStock.DisplayBasicInfo();  // This comes from Asset base class
            dreamHouse.DisplayBasicInfo();      // Same method, different object
            treasuryBond.DisplayBasicInfo();    // All three can use it!
            
            Console.WriteLine("\n2. Each class also has its own specific methods:");
            microsoftStock.ShowStockDetails();  // Only Stock has this
            dreamHouse.ShowHouseDetails();      // Only House has this  
            treasuryBond.ShowBondDetails();     // Only Bond has this
            
            // This is the key benefit: we wrote DisplayBasicInfo() once in Asset,
            // but all three derived classes can use it. That's code reuse through inheritance!
            
            Console.WriteLine("\n3. All derived classes have the same base properties:");
            Console.WriteLine($"Stock name: {microsoftStock.Name}");
            Console.WriteLine($"House name: {dreamHouse.Name}");
            Console.WriteLine($"Bond name: {treasuryBond.Name}");
            
            Console.WriteLine($"\nStock acquired: {microsoftStock.AcquisitionDate}");
            Console.WriteLine($"House acquired: {dreamHouse.AcquisitionDate}");
            Console.WriteLine($"Bond acquired: {treasuryBond.AcquisitionDate}");
        }
    }
}
