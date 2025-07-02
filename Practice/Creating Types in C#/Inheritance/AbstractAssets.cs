using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates abstract classes and abstract members.
    /// Abstract classes cannot be instantiated directly - they serve as blueprints.
    /// 
    /// From material: "An abstract class is a base class that cannot be instantiated directly. 
    /// It serves as a blueprint for other classes and can define abstract members."
    /// </summary>

    // From material: "public abstract class Asset // Abstract class - cannot be instantiated"
    public abstract class AbstractAsset
    {
        public string Name;

        // Constructor for abstract class
        public AbstractAsset(string name)
        {
            Name = name;
            Console.WriteLine($"AbstractAsset constructor called for: {name}");
        }

        // From material: "public abstract decimal NetValue { get; } // Abstract property - no implementation here"
        public abstract decimal NetValue { get; }

        // Abstract method - must be implemented by derived classes
        public abstract void CalculateRisk();

        // Concrete method - can be used by all derived classes
        public virtual void DisplayBasicInfo()
        {
            Console.WriteLine($"Asset Name: {Name}");
            Console.WriteLine($"Net Value: ${NetValue:F2}");
        }

        // Another concrete method with implementation
        public void PrintAssetType()
        {
            Console.WriteLine($"Asset Type: {GetType().Name}");
        }
    }

    // From material: Concrete implementation of abstract class
    public class ConcreteStock : AbstractAsset
    {
        public long SharesOwned;
        public decimal CurrentPrice;

        public ConcreteStock(string name, long shares, decimal price) : base(name)
        {
            SharesOwned = shares;
            CurrentPrice = price;
        }

        // From material: "Must provide implementation" for abstract members
        // "public override decimal NetValue => CurrentPrice * SharesOwned;"
        public override decimal NetValue => CurrentPrice * SharesOwned;

        // Must implement the abstract method
        public override void CalculateRisk()
        {
            decimal riskFactor = 0.15m; // 15% risk for stocks
            decimal riskAmount = NetValue * riskFactor;
            Console.WriteLine($"Stock Risk Analysis for {Name}:");
            Console.WriteLine($"  Risk Factor: {riskFactor:P}");
            Console.WriteLine($"  Potential Loss: ${riskAmount:F2}");
        }

        // Override the virtual method to add stock-specific info
        public override void DisplayBasicInfo()
        {
            base.DisplayBasicInfo(); // Call base implementation first
            Console.WriteLine($"Shares: {SharesOwned:N0}");
            Console.WriteLine($"Price per Share: ${CurrentPrice:F2}");
        }
    }

    public class ConcreteBond : AbstractAsset
    {
        public decimal FaceValue;
        public decimal InterestRate;
        public DateTime MaturityDate;

        public ConcreteBond(string name, decimal faceValue, decimal rate, DateTime maturity) : base(name)
        {
            FaceValue = faceValue;
            InterestRate = rate;
            MaturityDate = maturity;
        }

        // Implementation of abstract property
        public override decimal NetValue
        {
            get
            {
                // Simple present value calculation
                var yearsToMaturity = (MaturityDate - DateTime.Now).TotalDays / 365.0;
                if (yearsToMaturity <= 0) return FaceValue;
                
                var futureValue = FaceValue * (1 + InterestRate);
                return (decimal)futureValue; // Simplified calculation
            }
        }

        // Implementation of abstract method
        public override void CalculateRisk()
        {
            decimal riskFactor = 0.05m; // 5% risk for bonds (lower than stocks)
            decimal riskAmount = NetValue * riskFactor;
            Console.WriteLine($"Bond Risk Analysis for {Name}:");
            Console.WriteLine($"  Risk Factor: {riskFactor:P}");
            Console.WriteLine($"  Credit Risk: ${riskAmount:F2}");
        }

        // Override to add bond-specific information
        public override void DisplayBasicInfo()
        {
            base.DisplayBasicInfo();
            Console.WriteLine($"Face Value: ${FaceValue:F2}");
            Console.WriteLine($"Interest Rate: {InterestRate:P}");
            Console.WriteLine($"Maturity: {MaturityDate:yyyy-MM-dd}");
        }
    }

    /// <summary>
    /// Another abstract class demonstrating abstract methods
    /// </summary>
    public abstract class FinancialInstrument
    {
        public string Symbol;
        public DateTime CreatedDate;

        protected FinancialInstrument(string symbol)
        {
            Symbol = symbol;
            CreatedDate = DateTime.Now;
        }

        // Abstract methods - no implementation in base class
        public abstract decimal GetCurrentValue();
        public abstract string GetRiskLevel();
        public abstract void ProcessTransaction(decimal amount);

        // Concrete method that uses abstract methods
        public void GenerateReport()
        {
            Console.WriteLine($"\n=== Financial Instrument Report ===");
            Console.WriteLine($"Symbol: {Symbol}");
            Console.WriteLine($"Created: {CreatedDate:yyyy-MM-dd}");
            Console.WriteLine($"Current Value: ${GetCurrentValue():F2}");
            Console.WriteLine($"Risk Level: {GetRiskLevel()}");
        }
    }

    public class TradableStock : FinancialInstrument
    {
        public decimal Price;
        public long Volume;

        public TradableStock(string symbol, decimal price) : base(symbol)
        {
            Price = price;
            Volume = 0;
        }

        // Implementations of abstract methods
        public override decimal GetCurrentValue()
        {
            return Price * Volume;
        }

        public override string GetRiskLevel()
        {
            // Simple risk assessment based on price volatility
            if (Price > 1000) return "High";
            if (Price > 100) return "Medium";
            return "Low";
        }

        public override void ProcessTransaction(decimal amount)
        {
            long sharesToBuy = (long)(amount / Price);
            Volume += sharesToBuy;
            Console.WriteLine($"Bought {sharesToBuy} shares of {Symbol} at ${Price:F2} each");
            Console.WriteLine($"Total volume now: {Volume:N0}");
        }
    }

    // Additional concrete implementations for demonstration
    public class RealStock : AbstractAsset
    {
        public long SharesOwned;
        public decimal CurrentPrice;
        
        public RealStock(string name) : base(name) { }
        
        public RealStock() : base("Unknown Stock") { } // Parameterless constructor
        
        public override decimal NetValue => SharesOwned * CurrentPrice;
        
        public override void CalculateRisk()
        {
            Console.WriteLine($"Calculating stock risk for {Name}: Market volatility analysis");
        }
        
        public string GetDescription()
        {
            return $"Stock investment: {SharesOwned:N0} shares at ${CurrentPrice:N2} each";
        }
    }
    
    public class RealEstate : AbstractAsset
    {
        public decimal PurchasePrice;
        public decimal CurrentValue;
        
        public RealEstate(string name) : base(name) { }
        
        public RealEstate() : base("Unknown Property") { } // Parameterless constructor
        
        public override decimal NetValue => CurrentValue;
        
        public override void CalculateRisk()
        {
            Console.WriteLine($"Calculating real estate risk for {Name}: Location and market analysis");
        }
        
        public string GetDescription()
        {
            return $"Real estate: Purchased for ${PurchasePrice:N2}, current value ${CurrentValue:N2}";
        }
    }
    
    // Common GetDescription method for all AbstractAsset instances
    public static class AbstractAssetExtensions
    {
        public static string GetDescription(this AbstractAsset asset)
        {
            return asset switch
            {
                RealStock stock => stock.GetDescription(),
                RealEstate estate => estate.GetDescription(),
                _ => $"Abstract asset: {asset.Name}, Net Value: ${asset.NetValue:N2}"
            };
        }
    }

    /// <summary>
    /// Class to demonstrate abstract class usage
    /// </summary>
    public static class AbstractDemo
    {
        public static void DemonstrateAbstractClasses()
        {
            Console.WriteLine("=== Abstract Classes and Members ===");

            // Cannot instantiate abstract class:
            // var asset = new AbstractAsset("Test"); // This would cause compilation error!

            Console.WriteLine("\n--- Creating concrete implementations ---");
            
            var stock = new ConcreteStock("AAPL", 100, 175.25m);
            var bond = new ConcreteBond("US Treasury", 10000m, 0.03m, DateTime.Now.AddYears(5));

            Console.WriteLine("\n--- Using inherited concrete methods ---");
            stock.PrintAssetType();
            bond.PrintAssetType();

            Console.WriteLine("\n--- Using overridden abstract implementations ---");
            stock.DisplayBasicInfo();
            stock.CalculateRisk();
            
            Console.WriteLine();
            bond.DisplayBasicInfo();
            bond.CalculateRisk();

            Console.WriteLine("\n--- Polymorphic behavior with abstract base ---");
            AbstractAsset[] portfolio = { stock, bond };
            
            foreach (AbstractAsset asset in portfolio)
            {
                Console.WriteLine($"\nProcessing: {asset.Name}");
                asset.DisplayBasicInfo();  // Calls overridden version
                asset.CalculateRisk();     // Calls concrete implementation
            }

            Console.WriteLine("\n--- Financial Instrument Demo ---");
            var tradableStock = new TradableStock("MSFT", 350.75m);
            tradableStock.ProcessTransaction(5000m); // Buy $5000 worth
            tradableStock.GenerateReport();
        }
    }
}
