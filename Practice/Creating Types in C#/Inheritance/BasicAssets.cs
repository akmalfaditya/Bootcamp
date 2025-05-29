using System;

namespace Inheritance
{
    /// <summary>
    /// Basic Asset class - the foundation of our inheritance hierarchy
    /// This is your typical base class - simple and focused
    /// Every asset has a name, that's the common behavior we want to share
    /// </summary>
    public class Asset
    {
        // Simple field that all assets will inherit
        // Notice how we don't need virtual here - it's just data
        public string Name = string.Empty;

        /// <summary>
        /// Basic constructor for Asset
        /// Keep base constructors simple when possible
        /// </summary>
        public Asset()
        {
            Console.WriteLine("Asset constructor called");
        }

        /// <summary>
        /// Constructor with name parameter
        /// Shows how to provide different initialization options
        /// </summary>
        public Asset(string name)
        {
            Name = name;
            Console.WriteLine($"Asset constructor called with name: {name}");
        }
    }

    /// <summary>
    /// Stock class inherits from Asset
    /// This demonstrates basic inheritance - we get Name for free
    /// and add our own Stock-specific behavior
    /// </summary>
    public class Stock : Asset
    {
        // Stock-specific field
        // This is what makes Stock different from other assets
        public long SharesOwned;

        /// <summary>
        /// Default constructor
        /// Notice we don't explicitly call base() - it happens automatically
        /// </summary>
        public Stock()
        {
            SharesOwned = 0;
            Console.WriteLine("Stock constructor called");
        }

        /// <summary>
        /// Method specific to stocks
        /// Base class doesn't have this - it's Stock's special behavior
        /// </summary>
        public void BuyShares(long shares)
        {
            SharesOwned += shares;
            Console.WriteLine($"Bought {shares} shares of {Name}. Total: {SharesOwned}");
        }

        /// <summary>
        /// Another stock-specific method
        /// </summary>
        public void SellShares(long shares)
        {
            if (shares <= SharesOwned)
            {
                SharesOwned -= shares;
                Console.WriteLine($"Sold {shares} shares of {Name}. Remaining: {SharesOwned}");
            }
            else
            {
                Console.WriteLine($"Cannot sell {shares} shares - only have {SharesOwned}");
            }
        }
    }

    /// <summary>
    /// House class also inherits from Asset
    /// Same inheritance pattern as Stock, but different specialized behavior
    /// </summary>
    public class House : Asset
    {
        // House-specific field
        public decimal Mortgage;

        /// <summary>
        /// Default constructor
        /// </summary>
        public House()
        {
            Mortgage = 0;
            Console.WriteLine("House constructor called");
        }

        /// <summary>
        /// House-specific method for making payments
        /// </summary>
        public void MakePayment(decimal amount)
        {
            if (amount > 0 && amount <= Mortgage)
            {
                Mortgage -= amount;
                Console.WriteLine($"Made payment of ${amount:N2} on {Name}. Remaining: ${Mortgage:N2}");
            }
            else
            {
                Console.WriteLine($"Invalid payment amount: ${amount:N2}");
            }
        }

        /// <summary>
        /// Calculate how much is paid off
        /// </summary>
        public decimal GetEquityPercentage(decimal originalValue)
        {
            if (originalValue <= 0) return 0;
            return ((originalValue - Mortgage) / originalValue) * 100;
        }
    }
}
