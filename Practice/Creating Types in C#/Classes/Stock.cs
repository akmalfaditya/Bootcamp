using System;

namespace Classes
{
    /// <summary>
    /// Stock class demonstrating different types of properties
    /// Properties are the modern way to control access to object data
    /// Think of them as smart fields that can validate data and control access
    /// </summary>
    public class Stock
    {
        private decimal _price;
        private string _symbol = "";
        private int _shares;

        /// <summary>
        /// Auto-implemented property - compiler creates the backing field automatically
        /// This is the most common type of property for simple get/set scenarios
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Property with custom logic and validation
        /// Notice how we can control what happens when price is set
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Stock price cannot be negative!");
                _price = value;
                Console.WriteLine($"  💰 Stock price updated to ${value:F2}");
            }
        }

        /// <summary>
        /// Property with different access levels
        /// Public getter, public setter - allowing external code to set symbol
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value?.ToUpper() ?? ""; } // Always store in uppercase
        }

        /// <summary>
        /// Property for company name
        /// </summary>
        public string CompanyName { get; set; } = "";

        /// <summary>
        /// Property with custom logic and validation
        /// Notice how we can control what happens when price is set
        /// </summary>
        public decimal CurrentPrice
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Stock price cannot be negative!");
                _price = value;
                Console.WriteLine($"  💰 Stock price updated to ${value:F2}");
            }
        }

        /// <summary>
        /// Property for shares owned
        /// </summary>
        public int SharesOwned
        {
            get { return _shares; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Cannot own negative shares!");
                
                int oldShares = _shares;
                _shares = value;
                
                if (oldShares != value)
                {
                    Console.WriteLine($"  📊 Shares changed from {oldShares} to {value}");
                    Console.WriteLine($"  💵 Portfolio value: ${TotalValue:F2}");
                }
            }
        }

        /// <summary>
        /// Property for creation date
        /// </summary>
        public DateTime CreatedDate { get; init; } = DateTime.Now;

        /// <summary>
        /// Computed property - calculates value rather than storing it
        /// No backing field needed since it's calculated from other properties
        /// </summary>
        public decimal TotalValue => CurrentPrice * SharesOwned;

        /// <summary>
        /// Property with init-only setter (C# 9 feature)
        /// Can be set during object initialization but not after
        /// </summary>
        public DateTime ListingDate { get; init; }

        /// <summary>
        /// Expression-bodied property for read-only scenarios
        /// Perfect for simple calculated values
        /// </summary>
        public bool IsExpensive => CurrentPrice > 100;

        /// <summary>
        /// Property with complex logic
        /// </summary>
        public int Shares
        {
            get { return _shares; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Cannot own negative shares!");
                
                int oldShares = _shares;
                _shares = value;
                
                if (oldShares != value)
                {
                    Console.WriteLine($"  📊 Shares changed from {oldShares} to {value}");
                    Console.WriteLine($"  💵 Portfolio value: ${TotalValue:F2}");
                }
            }
        }

        /// <summary>
        /// Default constructor to initialize the stock
        /// </summary>
        public Stock()
        {
            Symbol = "UNKNOWN";
            Name = "Unknown Company";
            CompanyName = "Unknown Company";
            CurrentPrice = 0;
            SharesOwned = 0;
            CreatedDate = DateTime.Now;
            
            Console.WriteLine($"  📈 Created stock: {Symbol} ({Name})");
        }

        /// <summary>
        /// Method to buy shares
        /// </summary>
        /// <param name="sharesToBuy">Number of shares to purchase</param>
        public void BuyShares(int sharesToBuy)
        {
            if (sharesToBuy <= 0)
            {
                Console.WriteLine("  ❌ Cannot buy zero or negative shares!");
                return;
            }

            SharesOwned += sharesToBuy; // Uses the property setter
        }

        /// <summary>
        /// Method to sell shares
        /// </summary>
        /// <param name="sharesToSell">Number of shares to sell</param>
        public void SellShares(int sharesToSell)
        {
            if (sharesToSell <= 0)
            {
                Console.WriteLine("  ❌ Cannot sell zero or negative shares!");
                return;
            }

            if (sharesToSell > SharesOwned)
            {
                Console.WriteLine($"  ❌ Cannot sell {sharesToSell} shares - only own {SharesOwned}!");
                return;
            }

            SharesOwned -= sharesToSell; // Uses the property setter
        }

        /// <summary>
        /// Indexer demonstration - access stock data by string key
        /// Makes the object work like a dictionary for certain properties
        /// </summary>
        /// <param name="property">Property name</param>
        /// <returns>Property value as object</returns>
        public object this[string property]
        {
            get
            {
                return property.ToLower() switch
                {
                    "symbol" => Symbol,
                    "name" => Name,
                    "price" => CurrentPrice,
                    "shares" => SharesOwned,
                    "value" => TotalValue,
                    _ => "Property not found"
                };
            }
        }

        /// <summary>
        /// Override ToString for nice display
        /// </summary>
        public override string ToString()
        {
            return $"{Symbol}: {Name} - ${CurrentPrice:F2} x {SharesOwned} shares = ${TotalValue:F2}";
        }
    }
}
