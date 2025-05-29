using System;

namespace Classes
{
    /// <summary>
    /// Demonstrates properties - the "smart" way to control access to data
    /// Properties look like fields from the outside, but can have logic inside
    /// </summary>
    public class Stock
    {
        // Private field to store the actual price
        // This is the "backing field" for our property
        private decimal currentPrice;
        
        // Private field for shares outstanding
        private long sharesOutstanding;

        /// <summary>
        /// Automatic property - compiler creates the backing field for us
        /// Use this when you don't need custom logic in get/set
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Manual property with validation in the setter
        /// This is where properties shine - you can add logic!
        /// </summary>
        public decimal CurrentPrice
        {
            get 
            { 
                // You could add logging here, or calculate the value
                return currentPrice; 
            }
            set 
            { 
                // Validation - price can't be negative
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative!");
                
                // You could add more logic here - logging, notifications, etc.
                Console.WriteLine($"Price changed from ${currentPrice} to ${value}");
                currentPrice = value; 
            }
        }

        /// <summary>
        /// Property with only a getter - read-only from outside
        /// The value is calculated based on other properties
        /// </summary>
        public decimal MarketCap
        {
            get { return CurrentPrice * SharesOutstanding; }
            // No setter - this is calculated, not stored
        }

        /// <summary>
        /// Property with private setter - can only be set from within the class
        /// Useful when you want controlled access
        /// </summary>
        public long SharesOutstanding
        {
            get { return sharesOutstanding; }
            private set 
            { 
                if (value <= 0)
                    throw new ArgumentException("Shares outstanding must be positive!");
                sharesOutstanding = value; 
            }
        }

        /// <summary>
        /// Auto-property with different access levels
        /// Public getter, private setter
        /// </summary>
        public DateTime LastUpdated { get; private set; }

        /// <summary>
        /// Expression-bodied property - for simple calculated properties
        /// </summary>
        public string DisplayName => $"{Symbol} - ${CurrentPrice:F2}";

        /// <summary>
        /// Constructor to initialize the stock
        /// </summary>
        public Stock()
        {
            Symbol = "N/A";
            currentPrice = 0;
            sharesOutstanding = 1000000; // Default 1 million shares
            LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Method to update shares outstanding (since property setter is private)
        /// </summary>
        /// <param name="shares">New number of shares</param>
        public void UpdateSharesOutstanding(long shares)
        {
            SharesOutstanding = shares;
            LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Method to update price and timestamp together
        /// </summary>
        /// <param name="newPrice">New stock price</param>
        public void UpdatePrice(decimal newPrice)
        {
            CurrentPrice = newPrice; // This uses the property setter with validation
            LastUpdated = DateTime.Now;
        }
    }
}
