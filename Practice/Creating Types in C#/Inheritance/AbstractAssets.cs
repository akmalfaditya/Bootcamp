using System;

namespace Inheritance
{
    /// <summary>
    /// Abstract Asset class - cannot be instantiated directly
    /// Use abstract when you want to define a contract that subclasses MUST follow
    /// Abstract is like saying "I'm defining the blueprint, but you fill in the details"
    /// </summary>
    public abstract class AbstractAsset
    {
        // Regular fields and properties work fine in abstract classes
        public string Name = string.Empty;
        public DateTime PurchaseDate = DateTime.Now;

        /// <summary>
        /// Abstract property - subclasses MUST implement this
        /// No default implementation provided - that's the point of abstract
        /// </summary>
        public abstract decimal NetValue { get; }

        /// <summary>
        /// Abstract method - subclasses must provide implementation
        /// </summary>
        public abstract decimal CalculateMonthlyReturn();

        /// <summary>
        /// Virtual method - subclasses CAN override if they want
        /// Notice the difference: abstract = must override, virtual = can override
        /// </summary>
        public virtual string GetDescription()
        {
            return $"{Name} purchased on {PurchaseDate:yyyy-MM-dd}";
        }

        /// <summary>
        /// Regular method - works normally in abstract classes
        /// This provides common functionality that all subclasses get
        /// </summary>
        public void PrintSummary()
        {
            Console.WriteLine($"Asset Summary for {Name}:");
            Console.WriteLine($"  Net Value: ${NetValue:N2}");
            Console.WriteLine($"  Monthly Return: ${CalculateMonthlyReturn():N2}");
            Console.WriteLine($"  Description: {GetDescription()}");
        }

        /// <summary>
        /// Another concrete method showing age calculation
        /// </summary>
        public int GetAgeInDays()
        {
            return (DateTime.Now - PurchaseDate).Days;
        }
    }

    /// <summary>
    /// RealStock implements the abstract AbstractAsset
    /// Must implement all abstract members - compiler enforces this
    /// </summary>
    public class RealStock : AbstractAsset
    {
        public long SharesOwned;
        public decimal CurrentPrice;

        /// <summary>
        /// MUST implement NetValue - it's abstract in base class
        /// This is the stock-specific way to calculate net value
        /// </summary>
        public override decimal NetValue => SharesOwned * CurrentPrice;

        /// <summary>
        /// MUST implement CalculateMonthlyReturn - also abstract
        /// Stock return calculation logic
        /// </summary>
        public override decimal CalculateMonthlyReturn()
        {
            // Simplified: assume 1% monthly return on stocks
            return NetValue * 0.01m;
        }

        /// <summary>
        /// Override the virtual GetDescription method
        /// We don't have to do this, but we can customize it
        /// </summary>
        public override string GetDescription()
        {
            return $"{base.GetDescription()} - {SharesOwned:N0} shares at ${CurrentPrice:N2} each";
        }

        /// <summary>
        /// Stock-specific method
        /// </summary>
        public decimal GetDividendYield(decimal annualDividend)
        {
            if (CurrentPrice <= 0) return 0;
            return (annualDividend / CurrentPrice) * 100;
        }
    }

    /// <summary>
    /// RealEstate also implements AbstractAsset
    /// Different implementation approach - that's the beauty of abstraction
    /// </summary>
    public class RealEstate : AbstractAsset
    {
        public decimal PurchasePrice;
        public decimal CurrentValue;

        /// <summary>
        /// Real estate version of NetValue calculation
        /// Completely different from stock calculation
        /// </summary>
        public override decimal NetValue => CurrentValue;

        /// <summary>
        /// Real estate monthly return calculation
        /// Different logic than stocks
        /// </summary>
        public override decimal CalculateMonthlyReturn()
        {
            // Real estate: assume 0.5% monthly appreciation
            return CurrentValue * 0.005m;
        }

        /// <summary>
        /// Customize the description for real estate
        /// </summary>
        public override string GetDescription()
        {
            decimal appreciation = CurrentValue - PurchasePrice;
            string appreciationText = appreciation >= 0 ? "gained" : "lost";
            return $"{base.GetDescription()} - {appreciationText} ${Math.Abs(appreciation):N2} in value";
        }

        /// <summary>
        /// Real estate specific methods
        /// </summary>
        public decimal GetAppreciationPercentage()
        {
            if (PurchasePrice <= 0) return 0;
            return ((CurrentValue - PurchasePrice) / PurchasePrice) * 100;
        }

        public bool HasAppreciated()
        {
            return CurrentValue > PurchasePrice;
        }
    }

    /// <summary>
    /// Another concrete implementation showing flexibility
    /// Cryptocurrency as an asset type
    /// </summary>
    public class Cryptocurrency : AbstractAsset
    {
        public decimal CoinsOwned;
        public decimal CurrentPricePerCoin;
        public string Symbol = string.Empty;

        /// <summary>
        /// Crypto-specific net value calculation
        /// </summary>
        public override decimal NetValue => CoinsOwned * CurrentPricePerCoin;

        /// <summary>
        /// Crypto monthly return - historically volatile!
        /// </summary>
        public override decimal CalculateMonthlyReturn()
        {
            // Crypto is volatile - could be +/- 10% monthly
            // For demo, let's say 3% average
            return NetValue * 0.03m;
        }

        /// <summary>
        /// Crypto-specific description
        /// </summary>
        public override string GetDescription()
        {
            return $"{base.GetDescription()} - {CoinsOwned:N4} {Symbol} at ${CurrentPricePerCoin:N2} each";
        }

        /// <summary>
        /// Check if we're holding whole coins or fractions
        /// </summary>
        public bool HasFractionalCoins()
        {
            return CoinsOwned % 1 != 0;
        }
    }
}
