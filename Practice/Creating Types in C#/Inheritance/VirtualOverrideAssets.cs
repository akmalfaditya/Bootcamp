using System;

namespace Inheritance
{
    /// <summary>
    /// Advanced Asset class demonstrating virtual members
    /// Virtual members can be overridden by subclasses - that's the key difference
    /// Think of virtual as "you can customize this behavior in your subclass if you want"
    /// </summary>
    public class AdvancedAsset
    {
        public string Name = string.Empty;

        /// <summary>
        /// Virtual property - subclasses can provide their own implementation
        /// Default implementation returns 0, but subclasses can override
        /// </summary>
        public virtual decimal Liability => 0;

        /// <summary>
        /// Virtual method - can be overridden in derived classes
        /// This is the foundation of polymorphism
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Asset: {Name}, Liability: ${Liability:N2}");
        }

        /// <summary>
        /// Virtual method for calculating annual costs
        /// Base implementation assumes no ongoing costs
        /// </summary>
        public virtual decimal CalculateAnnualCosts()
        {
            return 0; // Base assets have no annual costs
        }

        /// <summary>
        /// Non-virtual method - cannot be overridden
        /// Use this when you want consistent behavior across all subclasses
        /// </summary>
        public void PrintAssetType()
        {
            Console.WriteLine($"This is an asset of type: {GetType().Name}");
        }
    }

    /// <summary>
    /// Advanced Stock class showing override in action
    /// Notice how we override virtual members to provide Stock-specific behavior
    /// </summary>
    public class AdvancedStock : AdvancedAsset
    {
        public long SharesOwned;
        public decimal CurrentPrice;

        /// <summary>
        /// Override the Liability property
        /// For stocks, liability could be potential losses
        /// </summary>
        public override decimal Liability
        {
            get
            {
                // Simple risk calculation - 20% of current value
                return (CurrentPrice * SharesOwned) * 0.20m;
            }
        }

        /// <summary>
        /// Override the DisplayInfo method
        /// We provide Stock-specific information
        /// </summary>
        public override void DisplayInfo()
        {
            Console.WriteLine($"Stock: {Name}");
            Console.WriteLine($"  Shares: {SharesOwned:N0}");
            Console.WriteLine($"  Current Price: ${CurrentPrice:N2}");
            Console.WriteLine($"  Total Value: ${CurrentPrice * SharesOwned:N2}");
            Console.WriteLine($"  Risk (Liability): ${Liability:N2}");
        }

        /// <summary>
        /// Override annual costs calculation
        /// Stocks might have trading fees, etc.
        /// </summary>
        public override decimal CalculateAnnualCosts()
        {
            // Assume $10 per year in trading fees per stock position
            return 10.0m;
        }

        /// <summary>
        /// Stock-specific method not in base class
        /// </summary>
        public decimal GetCurrentValue()
        {
            return CurrentPrice * SharesOwned;
        }
    }

    /// <summary>
    /// Advanced House class showing different override implementations
    /// Same base class, completely different override behavior
    /// </summary>
    public class AdvancedHouse : AdvancedAsset
    {
        public decimal Mortgage;
        public decimal EstimatedValue;

        /// <summary>
        /// Override Liability - for houses, it's the mortgage amount
        /// This makes perfect sense for real estate
        /// </summary>
        public override decimal Liability => Mortgage;

        /// <summary>
        /// Override DisplayInfo with house-specific details
        /// </summary>
        public override void DisplayInfo()
        {
            Console.WriteLine($"House: {Name}");
            Console.WriteLine($"  Estimated Value: ${EstimatedValue:N2}");
            Console.WriteLine($"  Mortgage: ${Mortgage:N2}");
            Console.WriteLine($"  Equity: ${EstimatedValue - Mortgage:N2}");
            Console.WriteLine($"  Liability: ${Liability:N2}");
        }

        /// <summary>
        /// Override annual costs - houses have maintenance, taxes, etc.
        /// </summary>
        public override decimal CalculateAnnualCosts()
        {
            // Rough estimate: 2% of home value per year
            return EstimatedValue * 0.02m;
        }

        /// <summary>
        /// House-specific methods
        /// </summary>
        public decimal GetEquityPercentage()
        {
            if (EstimatedValue <= 0) return 0;
            return ((EstimatedValue - Mortgage) / EstimatedValue) * 100;
        }

        public bool IsUnderwaterMortgage()
        {
            return Mortgage > EstimatedValue;
        }
    }
}
