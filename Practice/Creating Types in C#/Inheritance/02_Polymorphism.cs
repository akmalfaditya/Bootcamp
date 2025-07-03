using System;
using System.Collections.Generic;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates polymorphism - one of the most powerful features of inheritance.
    /// Polymorphism means "many forms" - it allows a base class reference to refer to objects
    /// of any derived class. This is incredibly useful for writing flexible, extensible code.
    /// 
    /// Key concept: You can treat different types of objects the same way if they share a common base class.
    /// </summary>

    // We'll use these classes for polymorphism demonstrations
    // Notice they're similar to our basic example but focused on showing polymorphic behavior
    
    public class FinancialAsset
    {
        public string Name { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        
        // Virtual method - this can be overridden by derived classes
        // This is key for polymorphism to work properly
        public virtual decimal GetCurrentValue()
        {
            return 0; // Base implementation returns zero
        }
        
        // Virtual method for risk assessment
        public virtual string GetRiskLevel()
        {
            return "Unknown";
        }
        
        // Non-virtual method - same implementation for all assets
        public void ShowOwnerInfo()
        {
            Console.WriteLine($"Owner: {Owner}");
        }
    }

    public class EquityStock : FinancialAsset
    {
        public int Shares { get; set; }
        public decimal PricePerShare { get; set; }
        public string Exchange { get; set; } = string.Empty;
        
        // Override the base class method to provide stock-specific calculation
        public override decimal GetCurrentValue()
        {
            return Shares * PricePerShare;
        }
        
        public override string GetRiskLevel()
        {
            // Simple risk assessment based on stock exchange
            return Exchange == "NASDAQ" ? "High" : "Medium";
        }
    }

    public class CorporateBond : FinancialAsset
    {
        public decimal FaceValue { get; set; }
        public decimal CouponRate { get; set; }
        public string Rating { get; set; } = string.Empty;
        
        public override decimal GetCurrentValue()
        {
            // Simplified bond valuation
            return FaceValue * (1 + CouponRate / 100);
        }
        
        public override string GetRiskLevel()
        {
            // Risk based on bond rating
            return Rating switch
            {
                "AAA" => "Low",
                "AA" => "Low",
                "A" => "Medium",
                "BBB" => "Medium",
                _ => "High"
            };
        }
    }

    public class RealEstateAsset : FinancialAsset
    {
        public decimal AppraisedValue { get; set; }
        public decimal RentalIncome { get; set; }
        public string PropertyType { get; set; } = string.Empty;
        
        public override decimal GetCurrentValue()
        {
            // Real estate value includes property value plus annual rental income
            return AppraisedValue + (RentalIncome * 12);
        }
        
        public override string GetRiskLevel()
        {
            return PropertyType switch
            {
                "Residential" => "Medium",
                "Commercial" => "High",
                "Industrial" => "High",
                _ => "Medium"
            };
        }
    }

    /// <summary>
    /// Portfolio class demonstrates how polymorphism makes code incredibly flexible.
    /// Notice how it doesn't need to know about specific asset types - it just works with FinancialAsset references.
    /// </summary>
    public class InvestmentPortfolio
    {
        private List<FinancialAsset> assets = new List<FinancialAsset>();
        
        // This method accepts ANY type that inherits from FinancialAsset
        // This is polymorphism in action - one method, many types
        public void AddAsset(FinancialAsset asset)
        {
            assets.Add(asset);
            Console.WriteLine($"Added {asset.GetType().Name}: {asset.Name}");
        }
        
        // This method works with any FinancialAsset, regardless of specific type
        public decimal GetTotalValue()
        {
            decimal total = 0;
            foreach (FinancialAsset asset in assets)
            {
                // Here's the magic: asset.GetCurrentValue() calls the correct overridden method
                // based on the actual object type, not the reference type
                total += asset.GetCurrentValue();
            }
            return total;
        }
        
        // Generate a portfolio summary - works with any asset type
        public void ShowPortfolioSummary()
        {
            Console.WriteLine("\n=== PORTFOLIO SUMMARY ===");
            decimal totalValue = 0;
            
            foreach (FinancialAsset asset in assets)
            {
                // These method calls demonstrate polymorphism:
                // - The correct GetCurrentValue() method is called based on actual object type
                // - The correct GetRiskLevel() method is called based on actual object type
                decimal value = asset.GetCurrentValue();
                string risk = asset.GetRiskLevel();
                totalValue += value;
                
                Console.WriteLine($"{asset.Name} ({asset.GetType().Name}): ${value:F2} - Risk: {risk}");
            }
            
            Console.WriteLine($"\nTotal Portfolio Value: ${totalValue:F2}");
        }
        
        // Filter assets by risk level - another example of polymorphic behavior
        public void ShowAssetsByRisk(string riskLevel)
        {
            Console.WriteLine($"\n=== ASSETS WITH {riskLevel.ToUpper()} RISK ===");
            
            foreach (FinancialAsset asset in assets)
            {
                // Polymorphic call to GetRiskLevel()
                if (asset.GetRiskLevel() == riskLevel)
                {
                    Console.WriteLine($"- {asset.Name}: ${asset.GetCurrentValue():F2}");
                }
            }
        }
    }

    /// <summary>
    /// Demonstration of polymorphism concepts
    /// </summary>
    public static class PolymorphismDemoClass
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== POLYMORPHISM DEMONSTRATION ===\n");
            
            // Create different types of financial assets
            var appleStock = new EquityStock
            {
                Name = "Apple Inc.",
                Owner = "John Investor",
                Shares = 50,
                PricePerShare = 175.25m,
                Exchange = "NASDAQ"
            };
            
            var corporateBond = new CorporateBond
            {
                Name = "Microsoft Corporate Bond",
                Owner = "Jane Saver",
                FaceValue = 10000,
                CouponRate = 4.5m,
                Rating = "AAA"
            };
            
            var rentalProperty = new RealEstateAsset
            {
                Name = "Downtown Apartment",
                Owner = "Bob Landlord",
                AppraisedValue = 300000,
                RentalIncome = 2500,
                PropertyType = "Residential"
            };
            
            // Now here's where polymorphism shines!
            Console.WriteLine("1. Polymorphic variable assignment:");
            
            // All these variables are of type FinancialAsset, but they refer to different object types
            FinancialAsset asset1 = appleStock;      // FinancialAsset reference to EquityStock object
            FinancialAsset asset2 = corporateBond;   // FinancialAsset reference to CorporateBond object  
            FinancialAsset asset3 = rentalProperty;  // FinancialAsset reference to RealEstateAsset object
            
            Console.WriteLine($"Asset1 actual type: {asset1.GetType().Name}");
            Console.WriteLine($"Asset2 actual type: {asset2.GetType().Name}");
            Console.WriteLine($"Asset3 actual type: {asset3.GetType().Name}");
            
            Console.WriteLine("\n2. Polymorphic method calls:");
            Console.WriteLine("Even though all variables are FinancialAsset references,");
            Console.WriteLine("the correct overridden methods are called based on actual object type:");
            
            Console.WriteLine($"Asset1 value: ${asset1.GetCurrentValue():F2} (calls EquityStock.GetCurrentValue())");
            Console.WriteLine($"Asset2 value: ${asset2.GetCurrentValue():F2} (calls CorporateBond.GetCurrentValue())");
            Console.WriteLine($"Asset3 value: ${asset3.GetCurrentValue():F2} (calls RealEstateAsset.GetCurrentValue())");
            
            Console.WriteLine("\n3. Polymorphism with method parameters:");
            DisplayAssetInfo(appleStock);     // Pass EquityStock as FinancialAsset
            DisplayAssetInfo(corporateBond);  // Pass CorporateBond as FinancialAsset
            DisplayAssetInfo(rentalProperty); // Pass RealEstateAsset as FinancialAsset
            
            Console.WriteLine("\n4. Polymorphism with collections:");
            var portfolio = new InvestmentPortfolio();
            
            // All different types, but all treated as FinancialAsset
            portfolio.AddAsset(appleStock);
            portfolio.AddAsset(corporateBond);
            portfolio.AddAsset(rentalProperty);
            
            // The portfolio methods work with any FinancialAsset type
            portfolio.ShowPortfolioSummary();
            portfolio.ShowAssetsByRisk("High");
            portfolio.ShowAssetsByRisk("Medium");
            portfolio.ShowAssetsByRisk("Low");
        }
        
        // This method accepts FinancialAsset, but works with any derived type
        // This is the power of polymorphism - write once, works with many types
        private static void DisplayAssetInfo(FinancialAsset asset)
        {
            Console.WriteLine($"Processing {asset.GetType().Name}: {asset.Name}");
            Console.WriteLine($"  Current Value: ${asset.GetCurrentValue():F2}");
            Console.WriteLine($"  Risk Level: {asset.GetRiskLevel()}");
            asset.ShowOwnerInfo(); // This method is the same for all types
            Console.WriteLine();
        }
    }
}
