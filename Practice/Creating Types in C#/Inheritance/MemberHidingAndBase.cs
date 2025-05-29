using System;

namespace Inheritance
{
    /// <summary>
    /// Parent class for demonstrating member hiding
    /// Shows what happens when child classes define members with same names
    /// </summary>
    public class ParentClass
    {
        // Field that will be hidden in child class
        public int Counter = 1;

        /// <summary>
        /// Method that will be hidden (not overridden) in child class
        /// Notice there's no virtual keyword - this can't be overridden
        /// </summary>
        public void ShowMessage()
        {
            Console.WriteLine("Message from Parent class");
        }

        /// <summary>
        /// Virtual method for comparison
        /// This one CAN be overridden properly
        /// </summary>
        public virtual void VirtualMethod()
        {
            Console.WriteLine("Virtual method from Parent");
        }
    }

    /// <summary>
    /// Child class demonstrating member hiding with 'new' keyword
    /// The 'new' keyword tells compiler "yes, I know I'm hiding the parent member"
    /// </summary>
    public class ChildClass : ParentClass
    {
        // Hide the parent's Counter field
        // Without 'new', you'd get a compiler warning
        public new int Counter = 2;

        /// <summary>
        /// Hide the parent's ShowMessage method
        /// This is NOT overriding - it's hiding!
        /// </summary>
        public new void ShowMessage()
        {
            Console.WriteLine("Message from Child class (hidden method)");
        }

        /// <summary>
        /// Properly override the virtual method
        /// This IS polymorphic - it will be called even when referenced as Parent
        /// </summary>
        public override void VirtualMethod()
        {
            Console.WriteLine("Overridden virtual method from Child");
        }

        /// <summary>
        /// Method to demonstrate the difference between hiding and overriding
        /// </summary>
        public void DemonstrateHidingVsOverriding()
        {
            Console.WriteLine("=== Demonstrating Hiding vs Overriding ===");
            
            // Call our own methods
            ShowMessage();        // Child version (hidden)
            VirtualMethod();      // Child version (overridden)
            
            // Call parent methods using base
            base.ShowMessage();   // Parent version
            base.VirtualMethod(); // Parent version
        }
    }

    /// <summary>
    /// Demonstrates the 'base' keyword for accessing parent members
    /// The base keyword is your way to reach "up" the inheritance chain
    /// </summary>
    public class SmartHouse : AdvancedHouse
    {
        public decimal SmartDevicesCost;

        /// <summary>
        /// Override Liability but use base class calculation as starting point
        /// This is a common pattern - extend the base behavior
        /// </summary>
        public override decimal Liability
        {
            get
            {
                // Start with parent's liability calculation, then add our own costs
                return base.Liability + SmartDevicesCost;
            }
        }

        /// <summary>
        /// Override DisplayInfo but include parent information
        /// Another common pattern - augment rather than replace
        /// </summary>
        public override void DisplayInfo()
        {
            // Call the parent's display method first
            base.DisplayInfo();
            
            // Then add our own information
            Console.WriteLine($"  Smart Devices Cost: ${SmartDevicesCost:N2}");
            Console.WriteLine($"  Total Liability (including devices): ${Liability:N2}");
        }

        /// <summary>
        /// Method showing how to call parent's CalculateAnnualCosts and extend it
        /// </summary>
        public override decimal CalculateAnnualCosts()
        {
            // Get the house's base annual costs
            decimal baseCosts = base.CalculateAnnualCosts();
            
            // Add smart device maintenance costs (10% of device value annually)
            decimal deviceMaintenance = SmartDevicesCost * 0.10m;
            
            return baseCosts + deviceMaintenance;
        }
    }

    /// <summary>
    /// Example class showing base constructor calls
    /// Constructors don't inherit, but you can call parent constructors
    /// </summary>
    public class EnhancedAsset : Asset
    {
        public string Category;

        /// <summary>
        /// Constructor that explicitly calls base constructor
        /// The : base(name) syntax calls the parent's constructor with parameters
        /// </summary>
        public EnhancedAsset(string name, string category) : base(name)
        {
            Category = category;
            Console.WriteLine($"EnhancedAsset constructor called with category: {category}");
        }

        /// <summary>
        /// Default constructor that calls base default constructor implicitly
        /// Even though we don't see : base(), it happens automatically
        /// </summary>
        public EnhancedAsset()
        {
            Category = "Uncategorized";
            Console.WriteLine("EnhancedAsset default constructor called");
        }

        /// <summary>
        /// Method that demonstrates working with both inherited and own members
        /// </summary>
        public void DisplayFullInfo()
        {
            Console.WriteLine($"Enhanced Asset: {Name} (Category: {Category})");
        }
    }

    /// <summary>
    /// Class demonstrating multiple levels of inheritance and base usage
    /// Shows how base works through multiple inheritance levels
    /// </summary>
    public class PremiumAsset : EnhancedAsset
    {
        public string PremiumFeatures;

        /// <summary>
        /// Constructor calling base constructor
        /// Notice the chain: this calls EnhancedAsset, which calls Asset
        /// </summary>
        public PremiumAsset(string name, string category, string features) 
            : base(name, category)
        {
            PremiumFeatures = features;
            Console.WriteLine($"PremiumAsset constructor called with features: {features}");
        }

        /// <summary>
        /// Method that calls methods up the inheritance chain
        /// </summary>
        public void DisplayPremiumInfo()
        {
            // Call parent's method using base
            base.DisplayFullInfo();
            
            // Add our own information
            Console.WriteLine($"Premium Features: {PremiumFeatures}");
        }
    }
}
