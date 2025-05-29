using System;

namespace Inheritance
{
    /// <summary>
    /// Class demonstrating sealed methods
    /// Sealed methods prevent further overriding in derived classes
    /// Use sealed when you want to "lock down" a method implementation
    /// </summary>
    public class SealedHouse : AdvancedHouse
    {
        /// <summary>
        /// Sealed override of Liability property
        /// No class that inherits from SealedHouse can override this again
        /// It's the "final" implementation in this inheritance chain
        /// </summary>
        public sealed override decimal Liability => Mortgage;

        /// <summary>
        /// Sealed override of DisplayInfo method
        /// This prevents any further customization of how house info is displayed
        /// </summary>
        public sealed override void DisplayInfo()
        {
            Console.WriteLine($"Sealed House: {Name}");
            Console.WriteLine($"  Mortgage (Final Liability): ${Liability:N2}");
            Console.WriteLine($"  Estimated Value: ${EstimatedValue:N2}");
            Console.WriteLine("  [This display method is sealed - cannot be overridden further]");
        }

        /// <summary>
        /// Regular virtual method - can still be overridden by subclasses
        /// Only the sealed methods are locked down
        /// </summary>
        public virtual void PerformMaintenance()
        {
            Console.WriteLine($"Performing maintenance on {Name}");
        }
    }

    /// <summary>
    /// Sealed class - cannot be inherited from at all
    /// Use sealed classes when you want to prevent any further inheritance
    /// Common examples: String, DateTime, and many .NET types are sealed
    /// </summary>
    public sealed class SealedClass
    {
        private string data;

        /// <summary>
        /// Constructor for sealed class
        /// Works normally - sealed only affects inheritance
        /// </summary>
        public SealedClass(string data)
        {
            this.data = data;
            Console.WriteLine($"SealedClass created with data: {data}");
        }

        /// <summary>
        /// Regular method in sealed class
        /// Since class is sealed, no one can override this anyway
        /// </summary>
        public void DoSomething()
        {
            Console.WriteLine($"SealedClass doing something with: {data}");
        }

        /// <summary>
        /// Virtual method in sealed class
        /// This is pointless since no one can inherit from this class!
        /// Compiler will warn you about this
        /// </summary>
        public virtual void VirtualMethod()
        {
            Console.WriteLine("This virtual method can't be overridden - class is sealed!");
        }

        /// <summary>
        /// Property in sealed class
        /// </summary>
        public string Data 
        { 
            get => data; 
            set 
            { 
                data = value;
                Console.WriteLine($"Data updated to: {data}");
            } 
        }
    }

    /// <summary>
    /// This would cause a compiler error if uncommented:
    /// Sealed classes cannot be inherited from
    /// </summary>
    /*
    public class CannotInheritFromSealed : SealedClass
    {
        // Compiler error: cannot derive from sealed type 'SealedClass'
    }
    */

    /// <summary>
    /// Attempting to inherit from SealedHouse to show sealed method limitations
    /// This class CAN inherit from SealedHouse, but cannot override sealed members
    /// </summary>
    public class ExtendedSealedHouse : SealedHouse
    {
        /// <summary>
        /// This would cause compiler error if uncommented:
        /// Cannot override sealed method
        /// </summary>
        /*
        public override decimal Liability => base.Liability + 1000;
        */

        /// <summary>
        /// This would also cause compiler error:
        /// Cannot override sealed method
        /// </summary>
        /*
        public override void DisplayInfo()
        {
            Console.WriteLine("Trying to override sealed method");
        }
        */

        /// <summary>
        /// But we CAN override non-sealed virtual methods
        /// PerformMaintenance wasn't sealed, so this works fine
        /// </summary>
        public override void PerformMaintenance()
        {
            Console.WriteLine($"Extended maintenance on {Name} - checking smart systems");
            base.PerformMaintenance(); // Call the base implementation too
        }

        /// <summary>
        /// We can also add new methods to the class
        /// Sealed doesn't prevent adding new functionality
        /// </summary>
        public void AdvancedFeature()
        {
            Console.WriteLine($"Advanced feature for {Name}");
        }
    }

    /// <summary>
    /// Example showing practical use of sealed
    /// Configuration classes are often sealed to prevent tampering
    /// </summary>
    public sealed class DatabaseConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int MaxConnections { get; set; } = 100;
        public TimeSpan CommandTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Validate configuration settings
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ConnectionString) 
                   && MaxConnections > 0 
                   && CommandTimeout.TotalSeconds > 0;
        }

        /// <summary>
        /// Get configuration summary
        /// </summary>
        public string GetSummary()
        {
            return $"DB Config: {MaxConnections} max connections, {CommandTimeout.TotalSeconds}s timeout";
        }
    }

    /// <summary>
    /// Another practical sealed class example
    /// Utility classes are often sealed
    /// </summary>
    public sealed class MathHelper
    {
        /// <summary>
        /// Calculate compound interest
        /// </summary>
        public static decimal CalculateCompoundInterest(decimal principal, decimal rate, int periods)
        {
            return principal * (decimal)Math.Pow((double)(1 + rate), periods);
        }

        /// <summary>
        /// Convert percentage to decimal
        /// </summary>
        public static decimal PercentageToDecimal(decimal percentage)
        {
            return percentage / 100m;
        }

        /// <summary>
        /// Private constructor prevents instantiation
        /// Combined with sealed, this makes it a pure utility class
        /// </summary>
        private MathHelper()
        {
            // No instances allowed
        }
    }

    /// <summary>
    /// Class showing when NOT to use sealed
    /// Business logic classes should usually allow inheritance for extensibility
    /// </summary>
    public class ExtensibleBusinessClass
    {
        /// <summary>
        /// Virtual method that subclasses can customize
        /// Don't seal this - business requirements change!
        /// </summary>
        public virtual decimal CalculateDiscount(decimal amount)
        {
            return amount * 0.05m; // 5% default discount
        }

        /// <summary>
        /// Virtual method for validation
        /// Different business units might have different validation rules
        /// </summary>
        public virtual bool ValidateTransaction(decimal amount)
        {
            return amount > 0 && amount <= 10000; // Basic validation
        }
    }

    /// <summary>
    /// Premium business class that extends the base business logic
    /// This shows why sealed can be limiting for business classes
    /// </summary>
    public class PremiumBusinessClass : ExtensibleBusinessClass
    {
        /// <summary>
        /// Override discount calculation for premium customers
        /// </summary>
        public override decimal CalculateDiscount(decimal amount)
        {
            // Premium customers get better discounts
            return amount * 0.10m; // 10% discount
        }

        /// <summary>
        /// Override validation for premium customers
        /// </summary>
        public override bool ValidateTransaction(decimal amount)
        {
            // Premium customers have higher limits
            return amount > 0 && amount <= 50000;
        }
    }
}
