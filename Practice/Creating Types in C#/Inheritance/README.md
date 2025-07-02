# C# Inheritance: A Comprehensive Guide to Object-Oriented Programming

## Overview

This project provides a thorough exploration of inheritance, one of the four fundamental pillars of object-oriented programming in C#. Inheritance enables the creation of new classes based on existing classes, promoting code reuse, establishing hierarchical relationships, and enabling polymorphic behavior. This guide presents practical, well-documented examples that demonstrate every major inheritance concept, from basic class derivation to advanced topics like abstract classes, virtual methods, and modern C# inheritance features.

## Learning Objectives

By studying and executing this project, you will develop a comprehensive understanding of:

- **Inheritance Fundamentals**: How to create derived classes that inherit from base classes and understand the "is-a" relationship.
- **Polymorphism**: How a single interface can represent different underlying forms through inheritance and virtual dispatch.
- **Casting and Type Conversion**: Safe techniques for converting between base and derived types using explicit casting, the `as` operator, and the `is` operator.
- **Virtual Methods and Override**: How to design extensible class hierarchies using virtual methods and override implementations.
- **Abstract Classes and Members**: Creating contracts through abstract base classes that force derived classes to implement specific functionality.
- **Member Hiding vs. Overriding**: Understanding the critical difference between `new` (hiding) and `override` (polymorphism).
- **The `base` Keyword**: Accessing base class functionality from derived classes and implementing constructor chaining.
- **Constructor Inheritance**: How constructors work in inheritance hierarchies and proper initialization patterns.
- **Modern Inheritance Features**: Leveraging C# 11 required members, C# 12 primary constructors, and sealed classes for robust design.

## Project Structure

The project is organized into focused files, each demonstrating specific inheritance concepts. The `Program.cs` file serves as the central demonstration runner that executes all examples in a logical learning sequence.

```
Inheritance/
├── Program.cs                 # Main entry point that orchestrates all demonstrations
├── BasicAssets.cs            # Fundamental inheritance concepts with Asset base class
├── PolymorphismDemo.cs       # Polymorphism and base class references to derived objects
├── CastingAndConversions.cs  # Type casting, upcasting, downcasting, is/as operators
├── VirtualOverrideAssets.cs  # Virtual methods, override implementations, covariant returns
├── AbstractAssets.cs         # Abstract classes, abstract members, and concrete implementations
├── MemberHidingAndBase.cs    # Member hiding with 'new', base keyword usage, enhanced classes
├── ConstructorInheritance.cs # Constructor chaining, base constructor calls, initialization order
├── ModernFeatures.cs         # C# 11 required members and C# 12 primary constructors
├── SealedConcepts.cs         # Sealed classes and sealed method overrides
└── README.md                 # This comprehensive documentation file
```

## Core Inheritance Concepts Demonstrated

### 1. **Basic Inheritance**: Foundation of Object-Oriented Design

Inheritance allows a new class (derived class or subclass) to acquire properties and methods from an existing class (base class or superclass). This establishes an "is-a" relationship where the derived class is a specialized version of the base class.

```csharp
// From BasicAssets.cs
public class Asset  // Base class
{
    public string Name;
}

public class Stock : Asset  // Derived class - Stock "is-a" Asset
{
    public long SharesOwned;
}

public class House : Asset  // Another derived class - House "is-a" Asset
{
    public decimal Mortgage;
}
```

**Key Principles:**
- C# supports single inheritance: a class can inherit from only one base class
- Derived classes automatically inherit all accessible members from their base class
- Derived classes can add their own unique members and functionality
- The colon (`:`) syntax establishes the inheritance relationship

### 2. **Polymorphism**: One Interface, Many Forms

Polymorphism enables a variable of a base class type to refer to objects of any derived class type. This allows methods to work with objects at different levels of the inheritance hierarchy without knowing their specific types.

```csharp
// From PolymorphismDemo.cs
public static void DisplayAsset(Asset asset)  // Accepts any Asset or its subclasses
{
    Console.WriteLine($"Asset: {asset.Name}");
}

Stock stock = new Stock { Name = "MSFT", SharesOwned = 1000 };
House house = new House { Name = "Family Home", Mortgage = 250000 };

DisplayAsset(stock);  // Works with Stock
DisplayAsset(house);  // Works with House
```

**Key Benefits:**
- Code reusability: write once, work with many types
- Extensibility: new derived classes work with existing polymorphic code
- Maintainability: changes to derived classes do not affect polymorphic methods

### 3. **Casting and Reference Conversions**: Safe Type Navigation

Object references can be converted between compatible types in an inheritance hierarchy. Understanding these conversions is crucial for working safely with polymorphic code.

#### Upcasting (Implicit and Always Safe)
```csharp
// From CastingAndConversions.cs
Stock stock = new Stock();
Asset asset = stock;  // Upcasting: derived to base (automatic)
```

#### Downcasting (Explicit and Potentially Unsafe)
```csharp
Asset asset = new Stock();
Stock stock = (Stock)asset;  // Downcasting: base to derived (explicit cast required)
```

#### Safe Casting with `as` Operator
```csharp
Asset asset = new House();
Stock stock = asset as Stock;  // Returns null if conversion fails
if (stock != null)
{
    // Safe to use stock
}
```

#### Type Checking with `is` Operator
```csharp
Asset asset = new Stock();
if (asset is Stock stockVariable)  // Pattern variable (C# 7+)
{
    Console.WriteLine(stockVariable.SharesOwned);  // stockVariable is automatically cast
}
```

### 4. **Virtual Methods and Override**: Enabling Extensible Design

Virtual methods provide a mechanism for base classes to define methods that derived classes can customize while maintaining polymorphic behavior.

```csharp
// From VirtualOverrideAssets.cs
public class Asset
{
    public virtual decimal Liability => 0;  // Virtual property with default implementation
}

public class House : Asset
{
    public decimal Mortgage;
    public override decimal Liability => Mortgage;  // Override with house-specific logic
}
```

**Covariant Return Types (C# 9+):**
```csharp
public class Asset
{
    public virtual Asset Clone() => new Asset { Name = Name };
}

public class House : Asset
{
    public override House Clone() => new House { Name = Name, Mortgage = Mortgage };
    // Return type is more specific (covariant)
}
```

**Key Characteristics:**
- Virtual methods enable runtime polymorphism
- Override methods must have matching signatures and accessibility
- Covariant return types allow more specific return types in overrides
- Virtual method calls are resolved at runtime based on the actual object type

### 5. **Abstract Classes and Members**: Enforcing Implementation Contracts

Abstract classes serve as blueprints that cannot be instantiated directly. They can define abstract members that derived classes must implement, ensuring consistent interfaces across the inheritance hierarchy.

```csharp
// From AbstractAssets.cs
public abstract class AbstractAsset  // Cannot be instantiated
{
    public string Name;
    public abstract decimal NetValue { get; }  // Must be implemented by derived classes
    public abstract void CalculateRisk();     // Abstract method
}

public class ConcreteStock : AbstractAsset
{
    public override decimal NetValue => SharesOwned * CurrentPrice;  // Required implementation
    public override void CalculateRisk() 
    { 
        // Stock-specific risk calculation
    }
}
```

**Abstract Class Rules:**
- Cannot be instantiated with `new`
- Can contain both abstract and concrete members
- Derived classes must implement all abstract members unless they are also abstract
- Provides a way to enforce implementation contracts while sharing common functionality

### 6. **Member Hiding vs. Override**: Understanding the Critical Difference

One of the most important concepts in inheritance is understanding when method resolution happens at compile-time (hiding) versus runtime (overriding).

#### Method Hiding with `new` Keyword
```csharp
// From MemberHidingAndBase.cs
public class BaseClass
{
    public virtual void DoSomething() { Console.WriteLine("Base implementation"); }
}

public class DerivedClass : BaseClass
{
    public new void DoSomething() { Console.WriteLine("Derived implementation"); }  // Hiding
}

BaseClass obj = new DerivedClass();
obj.DoSomething();  // Calls Base implementation (compile-time binding)
```

#### Method Overriding with `override` Keyword
```csharp
public class DerivedClass : BaseClass
{
    public override void DoSomething() { Console.WriteLine("Derived implementation"); }  // Overriding
}

BaseClass obj = new DerivedClass();
obj.DoSomething();  // Calls Derived implementation (runtime binding)
```

**Critical Differences:**
- **Hiding (`new`)**: Method called depends on the compile-time type of the reference
- **Overriding (`override`)**: Method called depends on the runtime type of the object
- **Polymorphism**: Only works with virtual/override, not with hiding

### 7. **The `base` Keyword**: Accessing Base Class Functionality

The `base` keyword provides access to base class members from derived classes, enabling extension of base functionality rather than complete replacement.

```csharp
// From MemberHidingAndBase.cs
public class SmartHouse : House
{
    public decimal SmartDevicesCost;
    
    public override decimal Liability => base.Liability + SmartDevicesCost;  // Extend base logic
    
    public override void DisplayInfo()
    {
        base.DisplayInfo();  // Call base implementation first
        Console.WriteLine($"Smart Devices: ${SmartDevicesCost:N2}");
    }
}
```

**Constructor Chaining with `base`:**
```csharp
public class EnhancedAsset : Asset
{
    public string Category;
    
    public EnhancedAsset(string name, string category) : base(name)  // Call base constructor
    {
        Category = category;
    }
}
```

### 8. **Constructor Inheritance**: Proper Object Initialization

Constructors are not inherited, but derived classes must ensure base class constructors are called for proper initialization.

```csharp
// From ConstructorInheritance.cs
public class BaseExample
{
    public int X;
    public BaseExample() { X = 1; }
    public BaseExample(int x) { X = x; }
}

public class DerivedExample : BaseExample
{
    public int Y;
    
    public DerivedExample() : base()  // Explicit call to base()
    {
        Y = 20;
    }
    
    public DerivedExample(int x) : base(x)  // Call parameterized base constructor
    {
        Y = x * 2;
    }
}
```

**Initialization Order:**
1. Derived class fields are initialized
2. Base constructor arguments are evaluated
3. Base class fields are initialized
4. Base class constructor executes
5. Derived class constructor executes

### 9. **Modern C# Inheritance Features**

#### Required Members (C# 11)
```csharp
// From ModernFeatures.cs
public class ModernAsset
{
    public required string Name;      // Must be set during object creation
    public required string AssetType; // Compiler enforces initialization
}

var asset = new ModernAsset { Name = "Gold", AssetType = "Commodity" };  // Required
```

#### Primary Constructors (C# 12)
```csharp
public class ModernBaseClass(int x)  // Primary constructor syntax
{
    public int X { get; } = x;
}

public class ModernDerivedClass(int x, int y) : ModernBaseClass(x)  // Chain to base
{
    public int Y { get; } = y;
}
```

### 10. **Sealed Classes and Methods**: Preventing Further Inheritance

The `sealed` keyword prevents inheritance or further overriding, providing finalization of class hierarchies.

```csharp
// From SealedConcepts.cs
public sealed class FinalClass  // Cannot be inherited
{
    public void DoSomething() { }
}

public class House : Asset
{
    public sealed override decimal Liability => Mortgage;  // Cannot be overridden further
}
```

## How to Run This Project

1. **Prerequisites**: Ensure you have .NET 8.0 SDK or later installed on your system.
2. **Navigate to Project**: Open a terminal and navigate to the `Inheritance` project directory.
3. **Build the Project**: Execute `dotnet build` to compile the code and verify there are no errors.
4. **Run Demonstrations**: Execute `dotnet run` to see all inheritance concepts demonstrated with clear output.

## Educational Progression

The program follows a carefully structured learning path:

1. **Foundation**: Basic inheritance syntax and relationships
2. **Behavior**: Polymorphism and method dispatch
3. **Safety**: Type casting and conversion techniques
4. **Design**: Virtual methods and abstract classes
5. **Advanced**: Member hiding, base keyword usage
6. **Architecture**: Constructor patterns and initialization
7. **Modern**: Contemporary C# inheritance features

## Best Practices Demonstrated

- **Favor composition over inheritance** when "has-a" relationships are more appropriate than "is-a"
- **Use virtual methods judiciously** - only when you expect derived classes to customize behavior
- **Prefer abstract classes over interfaces** when you need to share implementation code
- **Always call base constructors explicitly** for clarity and maintainability
- **Use sealed appropriately** to prevent unintended inheritance
- **Leverage modern C# features** like required members and primary constructors for cleaner code

## Key Takeaways

- **Inheritance establishes "is-a" relationships** and enables code reuse through class hierarchies
- **Polymorphism allows uniform treatment** of objects at different levels of the inheritance hierarchy
- **Virtual and override enable extensibility** while maintaining polymorphic behavior
- **Abstract classes enforce implementation contracts** while providing shared functionality
- **The difference between hiding and overriding** is fundamental to understanding method dispatch
- **Constructor chaining ensures proper initialization** of the entire object hierarchy
- **Modern C# features enhance inheritance** with better syntax and compile-time safety
- **Sealed classes and methods provide control** over inheritance chains and prevent unwanted extensions

Understanding inheritance thoroughly is essential for designing robust, maintainable object-oriented applications in C#. This project provides the foundation for applying these concepts effectively in real-world software development.
using System;

namespace Inheritance
{
    /// <summary>
    /// Base class representing any type of financial asset
    /// This demonstrates the foundation of inheritance
    /// </summary>
    public class Asset
    {
        public string Name { get; set; }
        
        /// <summary>
        /// Virtual property that can be overridden by derived classes
        /// Default implementation returns 0 for liability
        /// </summary>
        public virtual decimal Liability => 0;
        
        /// <summary>
        /// Virtual method that can be overridden to provide asset-specific information
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Asset: {Name}");
            Console.WriteLine($"Liability: ${Liability:F2}");
        }
    }

    /// <summary>
    /// Stock class inherits from Asset
    /// Represents ownership shares in a company
    /// </summary>
    public class Stock : Asset
    {
        public long SharesOwned { get; set; }
        public decimal CurrentPrice { get; set; }
        
        /// <summary>
        /// Calculated property showing total value of stock holdings
        /// </summary>
        public decimal TotalValue => SharesOwned * CurrentPrice;
        
        /// <summary>
        /// Override the base class DisplayInfo to show stock-specific information
        /// </summary>
        public override void DisplayInfo()
        {
            Console.WriteLine($"Stock: {Name}");
            Console.WriteLine($"Shares Owned: {SharesOwned:N0}");
            Console.WriteLine($"Current Price: ${CurrentPrice:F2}");
            Console.WriteLine($"Total Value: ${TotalValue:F2}");
            Console.WriteLine($"Liability: ${Liability:F2}");
        }
    }

    /// <summary>
    /// House class also inherits from Asset
    /// Represents real estate property
    /// </summary>
    public class House : Asset
    {
        public decimal Mortgage { get; set; }
        public decimal EstimatedValue { get; set; }
        
        /// <summary>
        /// Override the Liability property - houses have mortgage debt
        /// </summary>
        public override decimal Liability => Mortgage;
        
        /// <summary>
        /// Calculated property showing net equity in the house
        /// </summary>
        public decimal Equity => EstimatedValue - Mortgage;
        
        /// <summary>
        /// Override DisplayInfo to show house-specific information
        /// </summary>
        public override void DisplayInfo()
        {
            Console.WriteLine($"House: {Name}");
            Console.WriteLine($"Estimated Value: ${EstimatedValue:F2}");
            Console.WriteLine($"Mortgage: ${Mortgage:F2}");
            Console.WriteLine($"Equity: ${Equity:F2}");
            Console.WriteLine($"Liability: ${Liability:F2}");
        }
    }
}
```

### Step 3: Create Abstract Classes and Members

Create a new file called `AbstractAssets.cs` to demonstrate abstract classes:

```csharp
using System;

namespace Inheritance
{
    /// <summary>
    /// Abstract base class for financial instruments
    /// Cannot be instantiated directly - serves as a blueprint
    /// </summary>
    public abstract class FinancialInstrument
    {
        public string Symbol { get; set; }
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Abstract property - must be implemented by derived classes
        /// </summary>
        public abstract decimal CurrentValue { get; }
        
        /// <summary>
        /// Abstract method - derived classes must provide implementation
        /// </summary>
        public abstract decimal CalculateRisk();
        
        /// <summary>
        /// Concrete method - shared by all financial instruments
        /// Can be overridden if needed
        /// </summary>
        public virtual void PrintSummary()
        {
            Console.WriteLine($"Financial Instrument: {Symbol}");
            Console.WriteLine($"Created: {CreatedDate:yyyy-MM-dd}");
            Console.WriteLine($"Current Value: ${CurrentValue:F2}");
            Console.WriteLine($"Risk Factor: {CalculateRisk():P2}");
        }
        
        /// <summary>
        /// Protected method - only accessible by derived classes
        /// </summary>
        protected void LogActivity(string activity)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {Symbol}: {activity}");
        }
    }

    /// <summary>
    /// Bond class inherits from the abstract FinancialInstrument
    /// Must implement all abstract members
    /// </summary>
    public class Bond : FinancialInstrument
    {
        public decimal FaceValue { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime MaturityDate { get; set; }
        
        /// <summary>
        /// Implementation of abstract CurrentValue property
        /// Simplified calculation for demonstration
        /// </summary>
        public override decimal CurrentValue => FaceValue * (1 + InterestRate);
        
        /// <summary>
        /// Implementation of abstract CalculateRisk method
        /// Bonds generally have lower risk
        /// </summary>
        public override decimal CalculateRisk()
        {
            var yearsToMaturity = (MaturityDate - DateTime.Now).Days / 365.0;
            return (decimal)(0.02 + (yearsToMaturity * 0.01)); // Simple risk calculation
        }
        
        /// <summary>
        /// Bond-specific method
        /// </summary>
        public void ProcessCouponPayment()
        {
            LogActivity("Coupon payment processed");
            var payment = FaceValue * InterestRate / 4; // Quarterly payment
            Console.WriteLine($"Quarterly coupon payment: ${payment:F2}");
        }
    }

    /// <summary>
    /// Derivative class for more complex financial instruments
    /// </summary>
    public class Derivative : FinancialInstrument
    {
        public string UnderlyingAsset { get; set; }
        public decimal Leverage { get; set; }
        public decimal BaseValue { get; set; }
        
        /// <summary>
        /// Implementation of abstract CurrentValue property
        /// Derivatives can amplify gains/losses through leverage
        /// </summary>
        public override decimal CurrentValue => BaseValue * Leverage;
        
        /// <summary>
        /// Implementation of abstract CalculateRisk method
        /// Derivatives typically have higher risk due to leverage
        /// </summary>
        public override decimal CalculateRisk()
        {
            return Math.Min(0.95m, 0.15m * Leverage); // Risk increases with leverage
        }
        
        /// <summary>
        /// Override PrintSummary to include derivative-specific information
        /// </summary>
        public override void PrintSummary()
        {
            base.PrintSummary(); // Call the base implementation first
            Console.WriteLine($"Underlying Asset: {UnderlyingAsset}");
            Console.WriteLine($"Leverage: {Leverage:F1}x");
        }
    }
}
```

### Step 4: Demonstrate Virtual Override Behavior

Create `VirtualOverrideAssets.cs` to show polymorphism in action:

```csharp
using System;

namespace Inheritance
{
    /// <summary>
    /// Base class demonstrating virtual methods and polymorphism
    /// </summary>
    public class Investment
    {
        public string Name { get; set; }
        public decimal InitialAmount { get; set; }
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Virtual method that can be overridden by derived classes
        /// </summary>
        public virtual decimal CalculateReturns()
        {
            // Simple 3% annual return for base investment
            var years = (DateTime.Now - StartDate).Days / 365.0;
            return InitialAmount * (decimal)(Math.Pow(1.03, years));
        }
        
        /// <summary>
        /// Virtual method for calculating risk
        /// </summary>
        public virtual string GetRiskLevel()
        {
            return "Low";
        }
        
        /// <summary>
        /// Virtual method that demonstrates polymorphic behavior
        /// </summary>
        public virtual void DisplayPerformance()
        {
            Console.WriteLine($"Investment: {Name}");
            Console.WriteLine($"Initial Amount: ${InitialAmount:F2}");
            Console.WriteLine($"Current Value: ${CalculateReturns():F2}");
            Console.WriteLine($"Risk Level: {GetRiskLevel()}");
        }
    }

    /// <summary>
    /// Aggressive investment strategy with higher returns and risk
    /// </summary>
    public class AggressiveInvestment : Investment
    {
        public decimal VolatilityFactor { get; set; } = 1.5m;
        
        /// <summary>
        /// Override to provide higher but more volatile returns
        /// </summary>
        public override decimal CalculateReturns()
        {
            var years = (DateTime.Now - StartDate).Days / 365.0;
            var baseReturn = Math.Pow(1.08, years); // 8% base return
            var volatility = Math.Sin(years * Math.PI) * 0.2; // Add some volatility
            return InitialAmount * (decimal)(baseReturn * (1 + volatility));
        }
        
        /// <summary>
        /// Override risk level for aggressive investments
        /// </summary>
        public override string GetRiskLevel()
        {
            return "High";
        }
        
        /// <summary>
        /// Override to show additional aggressive investment information
        /// </summary>
        public override void DisplayPerformance()
        {
            base.DisplayPerformance(); // Call base implementation
            Console.WriteLine($"Volatility Factor: {VolatilityFactor:F1}");
            Console.WriteLine($"Strategy: Aggressive Growth");
        }
    }

    /// <summary>
    /// Conservative investment with lower returns but stable growth
    /// </summary>
    public class ConservativeInvestment : Investment
    {
        public bool IsInsured { get; set; } = true;
        
        /// <summary>
        /// Override to provide steady, lower returns
        /// </summary>
        public override decimal CalculateReturns()
        {
            var years = (DateTime.Now - StartDate).Days / 365.0;
            var rate = IsInsured ? 0.025 : 0.035; // 2.5% if insured, 3.5% if not
            return InitialAmount * (decimal)Math.Pow(1 + rate, years);
        }
        
        /// <summary>
        /// Override risk level for conservative investments
        /// </summary>
        public override string GetRiskLevel()
        {
            return IsInsured ? "Very Low" : "Low";
        }
        
        /// <summary>
        /// Override to show conservative investment specific information
        /// </summary>
        public override void DisplayPerformance()
        {
            base.DisplayPerformance();
            Console.WriteLine($"Insured: {(IsInsured ? "Yes" : "No")}");
            Console.WriteLine($"Strategy: Capital Preservation");
        }
    }
}
```

### Step 5: Demonstrate Constructor Inheritance

Create `ConstructorInheritance.cs`:

```csharp
using System;

namespace Inheritance
{
    /// <summary>
    /// Base class demonstrating constructor inheritance patterns
    /// </summary>
    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string VIN { get; set; }
        
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Vehicle()
        {
            VIN = Guid.NewGuid().ToString("N")[..8].ToUpper();
            Console.WriteLine($"Vehicle created with VIN: {VIN}");
        }
        
        /// <summary>
        /// Constructor with basic parameters
        /// </summary>
        public Vehicle(string make, string model) : this()
        {
            Make = make;
            Model = model;
            Year = DateTime.Now.Year;
            Console.WriteLine($"Vehicle constructor: {make} {model}");
        }
        
        /// <summary>
        /// Full constructor with all parameters
        /// </summary>
        public Vehicle(string make, string model, int year) : this(make, model)
        {
            Year = year;
            Console.WriteLine($"Vehicle full constructor: {year} {make} {model}");
        }
        
        /// <summary>
        /// Virtual method for displaying vehicle information
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"{Year} {Make} {Model} (VIN: {VIN})");
        }
    }

    /// <summary>
    /// Car class demonstrating constructor chaining with base class
    /// </summary>
    public class Car : Vehicle
    {
        public int NumberOfDoors { get; set; }
        public string FuelType { get; set; }
        
        /// <summary>
        /// Default constructor that calls base constructor
        /// </summary>
        public Car() : base()
        {
            NumberOfDoors = 4;
            FuelType = "Gasoline";
            Console.WriteLine("Car default constructor executed");
        }
        
        /// <summary>
        /// Constructor that chains to base constructor with parameters
        /// </summary>
        public Car(string make, string model) : base(make, model)
        {
            NumberOfDoors = 4;
            FuelType = "Gasoline";
            Console.WriteLine($"Car constructor: {make} {model}");
        }
        
        /// <summary>
        /// Full constructor with car-specific parameters
        /// </summary>
        public Car(string make, string model, int year, int doors, string fuelType) 
            : base(make, model, year)
        {
            NumberOfDoors = doors;
            FuelType = fuelType;
            Console.WriteLine($"Car full constructor: {doors}-door {fuelType}");
        }
        
        /// <summary>
        /// Override DisplayInfo to include car-specific information
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo(); // Call base implementation
            Console.WriteLine($"Doors: {NumberOfDoors}, Fuel: {FuelType}");
        }
    }

    /// <summary>
    /// Motorcycle class with different constructor requirements
    /// </summary>
    public class Motorcycle : Vehicle
    {
        public int EngineSize { get; set; }
        public bool HasSidecar { get; set; }
        
        /// <summary>
        /// Motorcycle requires engine size, so no parameterless constructor
        /// </summary>
        public Motorcycle(string make, string model, int engineSize) : base(make, model)
        {
            EngineSize = engineSize;
            HasSidecar = false;
            Console.WriteLine($"Motorcycle constructor: {engineSize}cc");
        }
        
        /// <summary>
        /// Full constructor with all motorcycle-specific options
        /// </summary>
        public Motorcycle(string make, string model, int year, int engineSize, bool hasSidecar) 
            : base(make, model, year)
        {
            EngineSize = engineSize;
            HasSidecar = hasSidecar;
            Console.WriteLine($"Motorcycle full constructor: {engineSize}cc, Sidecar: {hasSidecar}");
        }
        
        /// <summary>
        /// Override DisplayInfo for motorcycle-specific information
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Engine: {EngineSize}cc, Sidecar: {(HasSidecar ? "Yes" : "No")}");
        }
    }
}
```

### Step 6: Demonstrate Member Hiding and Base Keyword

Create `MemberHidingAndBase.cs`:

```csharp
using System;

namespace Inheritance
{
    /// <summary>
    /// Base class demonstrating member hiding vs overriding
    /// </summary>
    public class Employee
    {
        public string Name { get; set; }
        protected decimal baseSalary;
        
        public Employee(string name, decimal salary)
        {
            Name = name;
            baseSalary = salary;
        }
        
        /// <summary>
        /// Virtual method that can be overridden
        /// </summary>
        public virtual decimal CalculatePay()
        {
            Console.WriteLine("Employee.CalculatePay() called");
            return baseSalary;
        }
        
        /// <summary>
        /// Regular method that can be hidden (but not overridden)
        /// </summary>
        public void DisplayEmployeeInfo()
        {
            Console.WriteLine($"Employee: {Name}");
            Console.WriteLine($"Base Salary: ${baseSalary:F2}");
        }
        
        /// <summary>
        /// Protected method accessible to derived classes
        /// </summary>
        protected virtual void LogPayCalculation(decimal amount)
        {
            Console.WriteLine($"Pay calculated for {Name}: ${amount:F2}");
        }
    }

    /// <summary>
    /// Manager class demonstrating method overriding
    /// </summary>
    public class Manager : Employee
    {
        public decimal Bonus { get; set; }
        public int TeamSize { get; set; }
        
        public Manager(string name, decimal salary, decimal bonus) : base(name, salary)
        {
            Bonus = bonus;
            TeamSize = 0;
        }
        
        /// <summary>
        /// Override the virtual CalculatePay method
        /// This demonstrates polymorphism - the correct method will be called
        /// based on the actual object type, not the reference type
        /// </summary>
        public override decimal CalculatePay()
        {
            Console.WriteLine("Manager.CalculatePay() called");
            var totalPay = base.CalculatePay() + Bonus + (TeamSize * 500); // Team bonus
            
            // Call protected base method
            base.LogPayCalculation(totalPay);
            return totalPay;
        }
        
        /// <summary>
        /// New method that hides the base class method
        /// This is method hiding, not overriding
        /// </summary>
        public new void DisplayEmployeeInfo()
        {
            Console.WriteLine($"Manager: {Name}");
            Console.WriteLine($"Base Salary: ${baseSalary:F2}");
            Console.WriteLine($"Bonus: ${Bonus:F2}");
            Console.WriteLine($"Team Size: {TeamSize}");
            Console.WriteLine($"Total Pay: ${CalculatePay():F2}");
        }
    }

    /// <summary>
    /// Developer class demonstrating another override implementation
    /// </summary>
    public class Developer : Employee
    {
        public string ProgrammingLanguage { get; set; }
        public int ProjectsCompleted { get; set; }
        
        public Developer(string name, decimal salary, string language) : base(name, salary)
        {
            ProgrammingLanguage = language;
            ProjectsCompleted = 0;
        }
        
        /// <summary>
        /// Override CalculatePay with developer-specific logic
        /// </summary>
        public override decimal CalculatePay()
        {
            Console.WriteLine("Developer.CalculatePay() called");
            var projectBonus = ProjectsCompleted * 1000; // $1000 per completed project
            var totalPay = base.CalculatePay() + projectBonus;
            
            base.LogPayCalculation(totalPay);
            return totalPay;
        }
        
        /// <summary>
        /// Override the protected LogPayCalculation method
        /// </summary>
        protected override void LogPayCalculation(decimal amount)
        {
            base.LogPayCalculation(amount); // Call base implementation
            Console.WriteLine($"Developer {Name} completed {ProjectsCompleted} projects");
        }
    }

    /// <summary>
    /// Contractor class that hides CalculatePay instead of overriding it
    /// This demonstrates the difference between hiding and overriding
    /// </summary>
    public class Contractor : Employee
    {
        public decimal HourlyRate { get; set; }
        public int HoursWorked { get; set; }
        
        public Contractor(string name, decimal hourlyRate) : base(name, 0)
        {
            HourlyRate = hourlyRate;
            HoursWorked = 0;
        }
        
        /// <summary>
        /// Hide the base CalculatePay method using 'new' keyword
        /// This is method hiding, not overriding
        /// </summary>
        public new decimal CalculatePay()
        {
            Console.WriteLine("Contractor.CalculatePay() called (hidden method)");
            return HourlyRate * HoursWorked;
        }
    }
}
```

### Step 7: Demonstrate Sealed Classes and Methods

Create `SealedConcepts.cs`:

```csharp
using System;

namespace Inheritance
{
    /// <summary>
    /// Base class with virtual methods that can be sealed by derived classes
    /// </summary>
    public class SecuritySystem
    {
        public string SystemName { get; set; }
        
        public SecuritySystem(string name)
        {
            SystemName = name;
        }
        
        /// <summary>
        /// Virtual method that can be overridden
        /// </summary>
        public virtual bool Authenticate(string credentials)
        {
            Console.WriteLine($"Basic authentication in {SystemName}");
            return !string.IsNullOrEmpty(credentials);
        }
        
        /// <summary>
        /// Virtual method for authorization
        /// </summary>
        public virtual bool Authorize(string user, string resource)
        {
            Console.WriteLine($"Basic authorization in {SystemName}");
            return true; // Basic system allows all access
        }
        
        /// <summary>
        /// Virtual method for logging security events
        /// </summary>
        public virtual void LogSecurityEvent(string eventDescription)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {SystemName}: {eventDescription}");
        }
    }

    /// <summary>
    /// Enhanced security system that overrides and seals some methods
    /// </summary>
    public class EnhancedSecuritySystem : SecuritySystem
    {
        public int MaxFailedAttempts { get; set; } = 3;
        private int failedAttempts = 0;
        
        public EnhancedSecuritySystem(string name) : base(name)
        {
        }
        
        /// <summary>
        /// Override and seal the Authenticate method
        /// Subclasses of EnhancedSecuritySystem cannot override this method
        /// </summary>
        public sealed override bool Authenticate(string credentials)
        {
            LogSecurityEvent("Enhanced authentication attempt");
            
            if (failedAttempts >= MaxFailedAttempts)
            {
                LogSecurityEvent("Account locked due to too many failed attempts");
                return false;
            }
            
            bool isValid = credentials?.Length >= 8; // Simple validation
            
            if (!isValid)
            {
                failedAttempts++;
                LogSecurityEvent($"Failed authentication attempt {failedAttempts}");
            }
            else
            {
                failedAttempts = 0; // Reset on successful auth
                LogSecurityEvent("Successful authentication");
            }
            
            return isValid;
        }
        
        /// <summary>
        /// Override authorization with enhanced logic
        /// This method can still be overridden by subclasses
        /// </summary>
        public override bool Authorize(string user, string resource)
        {
            LogSecurityEvent($"Enhanced authorization check for {user} accessing {resource}");
            
            // Enhanced authorization logic
            if (user == "admin") return true;
            if (resource == "public") return true;
            if (user?.Length > 0 && resource?.Length > 0) return true;
            
            return false;
        }
    }

    /// <summary>
    /// Attempt to inherit from EnhancedSecuritySystem
    /// Cannot override the sealed Authenticate method
    /// </summary>
    public class BiometricSecuritySystem : EnhancedSecuritySystem
    {
        public bool BiometricEnabled { get; set; } = true;
        
        public BiometricSecuritySystem(string name) : base(name)
        {
        }
        
        // This would cause a compilation error because Authenticate is sealed:
        // public override bool Authenticate(string credentials) { ... }
        
        /// <summary>
        /// Can still override non-sealed methods
        /// </summary>
        public override bool Authorize(string user, string resource)
        {
            LogSecurityEvent("Biometric authorization check");
            
            if (BiometricEnabled)
            {
                LogSecurityEvent("Using biometric verification");
                // Simulate biometric check
                return user?.Contains("bio_verified") == true;
            }
            
            // Fall back to base implementation
            return base.Authorize(user, resource);
        }
        
        /// <summary>
        /// Add biometric-specific functionality
        /// </summary>
        public bool ScanBiometric(string biometricData)
        {
            LogSecurityEvent("Biometric scan initiated");
            return biometricData?.Length > 10; // Simulate biometric validation
        }
    }

    /// <summary>
    /// Sealed class - cannot be inherited from
    /// </summary>
    public sealed class FinalSecuritySystem : SecuritySystem
    {
        public FinalSecuritySystem(string name) : base(name)
        {
        }
        
        /// <summary>
        /// Override with final implementation
        /// </summary>
        public override bool Authenticate(string credentials)
        {
            LogSecurityEvent("Final security system authentication");
            return credentials == "ultimate_password_123";
        }
        
        /// <summary>
        /// Final authorization implementation
        /// </summary>
        public override bool Authorize(string user, string resource)
        {
            LogSecurityEvent("Final authorization check");
            return user == "superuser";
        }
    }

    // This would cause a compilation error because FinalSecuritySystem is sealed:
    // public class AttemptedInheritance : FinalSecuritySystem { }
}
```

### Step 8: Create Modern Features with C# 12

Create `ModernFeatures.cs`:

```csharp
using System;

namespace Inheritance
{
    /// <summary>
    /// Base class using modern C# features including required members
    /// </summary>
    public class ModernAsset
    {
        public required string Name { get; init; }
        public required decimal InitialValue { get; init; }
        public DateTime CreatedDate { get; init; } = DateTime.Now;
        
        /// <summary>
        /// Virtual property using expression body
        /// </summary>
        public virtual decimal CurrentValue => InitialValue;
        
        /// <summary>
        /// Virtual method using expression body
        /// </summary>
        public virtual string GetAssetType() => "Generic Asset";
        
        /// <summary>
        /// Method demonstrating switch expression with inheritance
        /// </summary>
        public string GetAssetCategory() => this switch
        {
            ModernStock => "Equity",
            ModernBond => "Fixed Income",
            ModernRealEstate => "Real Estate",
            _ => "Other"
        };
    }

    /// <summary>
    /// Modern stock class using primary constructor (C# 12)
    /// </summary>
    public class ModernStock(string symbol, decimal price, long shares) : ModernAsset
    {
        public string Symbol { get; } = symbol;
        public decimal SharePrice { get; } = price;
        public long SharesOwned { get; } = shares;
        
        // Required properties must be satisfied
        public required string Name { get; init; }
        public required decimal InitialValue { get; init; }
        
        /// <summary>
        /// Override using expression body
        /// </summary>
        public override decimal CurrentValue => SharePrice * SharesOwned;
        
        /// <summary>
        /// Override using expression body
        /// </summary>
        public override string GetAssetType() => $"Stock ({Symbol})";
        
        /// <summary>
        /// Deconstruction method for modern pattern matching
        /// </summary>
        public void Deconstruct(out string symbol, out decimal value, out long shares)
        {
            symbol = Symbol;
            value = CurrentValue;
            shares = SharesOwned;
        }
    }

    /// <summary>
    /// Modern bond class with primary constructor
    /// </summary>
    public class ModernBond(string issuer, decimal faceValue, decimal rate, DateTime maturity) : ModernAsset
    {
        public string Issuer { get; } = issuer;
        public decimal FaceValue { get; } = faceValue;
        public decimal InterestRate { get; } = rate;
        public DateTime MaturityDate { get; } = maturity;
        
        // Required properties
        public required string Name { get; init; }
        public required decimal InitialValue { get; init; }
        
        /// <summary>
        /// Calculate current bond value based on time to maturity
        /// </summary>
        public override decimal CurrentValue
        {
            get
            {
                var yearsRemaining = (MaturityDate - DateTime.Now).TotalDays / 365.0;
                if (yearsRemaining <= 0) return FaceValue;
                
                var accruedInterest = FaceValue * InterestRate * (decimal)yearsRemaining;
                return FaceValue + accruedInterest;
            }
        }
        
        public override string GetAssetType() => $"Bond ({Issuer})";
    }

    /// <summary>
    /// Real estate class with modern C# features
    /// </summary>
    public class ModernRealEstate : ModernAsset
    {
        public required string Address { get; init; }
        public required decimal SquareFootage { get; init; }
        public decimal PricePerSquareFoot { get; init; }
        
        /// <summary>
        /// Using init-only properties with calculated value
        /// </summary>
        public override decimal CurrentValue => SquareFootage * PricePerSquareFoot;
        
        public override string GetAssetType() => "Real Estate";
        
        /// <summary>
        /// Method using pattern matching with inheritance
        /// </summary>
        public string GetPropertyType() => SquareFootage switch
        {
            < 1000 => "Compact",
            >= 1000 and < 2500 => "Medium",
            >= 2500 and < 5000 => "Large",
            _ => "Estate"
        };
    }

    /// <summary>
    /// Portfolio class demonstrating modern collection handling
    /// </summary>
    public class ModernPortfolio
    {
        private readonly List<ModernAsset> assets = new();
        
        public string Name { get; init; } = "Default Portfolio";
        
        /// <summary>
        /// Add asset using modern null checking
        /// </summary>
        public void AddAsset(ModernAsset asset)
        {
            ArgumentNullException.ThrowIfNull(asset);
            assets.Add(asset);
        }
        
        /// <summary>
        /// Get total value using LINQ and polymorphism
        /// </summary>
        public decimal TotalValue => assets.Sum(asset => asset.CurrentValue);
        
        /// <summary>
        /// Get assets by type using pattern matching
        /// </summary>
        public IEnumerable<T> GetAssetsByType<T>() where T : ModernAsset
        {
            return assets.OfType<T>();
        }
        
        /// <summary>
        /// Display portfolio using modern string interpolation
        /// </summary>
        public void DisplayPortfolio()
        {
            Console.WriteLine($"Portfolio: {Name}");
            Console.WriteLine($"Total Assets: {assets.Count}");
            Console.WriteLine($"Total Value: ${TotalValue:F2}");
            Console.WriteLine();
            
            foreach (var asset in assets)
            {
                Console.WriteLine($"- {asset.GetAssetType()}: {asset.Name} = ${asset.CurrentValue:F2}");
            }
        }
    }
}
```

### Step 9: Create the Main Program

Now update your `Program.cs` file to demonstrate all the inheritance concepts:

```csharp
using System;

namespace Inheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Inheritance Comprehensive Tutorial ===\n");
            
            // Demonstrate each concept in sequence
            DemonstrateBasicInheritance();
            DemonstratePolymorphism();
            DemonstrateAbstractClasses();
            DemonstrateVirtualOverride();
            DemonstrateConstructorInheritance();
            DemonstrateMemberHiding();
            DemonstrateSealedConcepts();
            DemonstrateModernFeatures();
            DemonstrateCastingAndTypeChecking();
            
            Console.WriteLine("\n=== Tutorial Complete ===");
            Console.WriteLine("You have successfully learned all major inheritance concepts in C#!");
        }

        static void DemonstrateBasicInheritance()
        {
            Console.WriteLine("1. Basic Inheritance Demonstration");
            Console.WriteLine("==================================");
            
            // Create instances of base and derived classes
            var asset = new Asset { Name = "Generic Asset" };
            var stock = new Stock 
            { 
                Name = "Microsoft", 
                SharesOwned = 100, 
                CurrentPrice = 350.75m 
            };
            var house = new House 
            { 
                Name = "Family Home", 
                EstimatedValue = 450000m, 
                Mortgage = 300000m 
            };
            
            Console.WriteLine("Base class instance:");
            asset.DisplayInfo();
            Console.WriteLine();
            
            Console.WriteLine("Derived class instances:");
            stock.DisplayInfo();
            Console.WriteLine();
            house.DisplayInfo();
            Console.WriteLine();
        }

        static void DemonstratePolymorphism()
        {
            Console.WriteLine("2. Polymorphism in Action");
            Console.WriteLine("========================");
            
            // Array of base class references pointing to derived class objects
            Asset[] portfolio = 
            {
                new Stock { Name = "Apple", SharesOwned = 50, CurrentPrice = 175.25m },
                new House { Name = "Vacation Home", EstimatedValue = 650000m, Mortgage = 400000m },
                new Stock { Name = "Google", SharesOwned = 25, CurrentPrice = 2750.50m }
            };
            
            Console.WriteLine("Polymorphic behavior - same method call, different implementations:");
            foreach (Asset asset in portfolio)
            {
                asset.DisplayInfo(); // Calls the correct overridden method
                Console.WriteLine($"Liability: ${asset.Liability:F2}"); // Polymorphic property access
                Console.WriteLine();
            }
        }

        static void DemonstrateAbstractClasses()
        {
            Console.WriteLine("3. Abstract Classes and Members");
            Console.WriteLine("==============================");
            
            // Cannot instantiate abstract class:
            // var instrument = new FinancialInstrument(); // Compilation error
            
            var bond = new Bond
            {
                Symbol = "US10Y",
                FaceValue = 10000m,
                InterestRate = 0.025m,
                MaturityDate = DateTime.Now.AddYears(10),
                CreatedDate = DateTime.Now
            };
            
            var derivative = new Derivative
            {
                Symbol = "MSFT-CALL",
                UnderlyingAsset = "MSFT",
                Leverage = 3.5m,
                BaseValue = 1000m,
                CreatedDate = DateTime.Now
            };
            
            Console.WriteLine("Bond (concrete implementation of abstract class):");
            bond.PrintSummary();
            bond.ProcessCouponPayment();
            Console.WriteLine();
            
            Console.WriteLine("Derivative (another concrete implementation):");
            derivative.PrintSummary();
            Console.WriteLine();
        }

        static void DemonstrateVirtualOverride()
        {
            Console.WriteLine("4. Virtual Methods and Override");
            Console.WriteLine("==============================");
            
            var investments = new Investment[]
            {
                new Investment 
                { 
                    Name = "Basic Savings", 
                    InitialAmount = 10000m, 
                    StartDate = DateTime.Now.AddYears(-2) 
                },
                new AggressiveInvestment 
                { 
                    Name = "Growth Fund", 
                    InitialAmount = 15000m, 
                    StartDate = DateTime.Now.AddYears(-3),
                    VolatilityFactor = 2.0m
                },
                new ConservativeInvestment 
                { 
                    Name = "Bond Fund", 
                    InitialAmount = 20000m, 
                    StartDate = DateTime.Now.AddYears(-1),
                    IsInsured = true
                }
            };
            
            Console.WriteLine("Virtual method calls - polymorphic behavior:");
            foreach (var investment in investments)
            {
                investment.DisplayPerformance();
                Console.WriteLine();
            }
        }

        static void DemonstrateConstructorInheritance()
        {
            Console.WriteLine("5. Constructor Inheritance");
            Console.WriteLine("=========================");
            
            Console.WriteLine("Creating vehicles with different constructor paths:");
            
            var car1 = new Car();
            car1.DisplayInfo();
            Console.WriteLine();
            
            var car2 = new Car("Toyota", "Camry");
            car2.DisplayInfo();
            Console.WriteLine();
            
            var car3 = new Car("BMW", "M3", 2023, 2, "Gasoline");
            car3.DisplayInfo();
            Console.WriteLine();
            
            var motorcycle = new Motorcycle("Harley-Davidson", "Street 750", 2024, 750, false);
            motorcycle.DisplayInfo();
            Console.WriteLine();
        }

        static void DemonstrateMemberHiding()
        {
            Console.WriteLine("6. Member Hiding vs Override");
            Console.WriteLine("============================");
            
            var employees = new Employee[]
            {
                new Manager("Alice Johnson", 80000m, 15000m) { TeamSize = 5 },
                new Developer("Bob Smith", 95000m, "C#") { ProjectsCompleted = 3 },
                new Contractor("Carol Williams", 125m) { HoursWorked = 160 }
            };
            
            Console.WriteLine("Polymorphic method calls (CalculatePay is virtual/override):");
            foreach (Employee emp in employees)
            {
                Console.WriteLine($"Pay for {emp.Name}: ${emp.CalculatePay():F2}");
                emp.DisplayEmployeeInfo(); // This calls base method for all except Manager
                Console.WriteLine();
            }
            
            Console.WriteLine("Direct calls showing method hiding:");
            if (employees[0] is Manager manager)
            {
                manager.DisplayEmployeeInfo(); // Calls Manager's hidden method
            }
            Console.WriteLine();
        }

        static void DemonstrateSealedConcepts()
        {
            Console.WriteLine("7. Sealed Classes and Methods");
            Console.WriteLine("============================");
            
            var systems = new SecuritySystem[]
            {
                new SecuritySystem("Basic System"),
                new EnhancedSecuritySystem("Enhanced System"),
                new BiometricSecuritySystem("Biometric System"),
                new FinalSecuritySystem("Final System")
            };
            
            Console.WriteLine("Testing authentication across different security systems:");
            
            string[] credentials = { "weak", "strong_password", "bio_verified_user" };
            
            foreach (var system in systems)
            {
                Console.WriteLine($"\nTesting {system.SystemName}:");
                foreach (var cred in credentials)
                {
                    bool result = system.Authenticate(cred);
                    Console.WriteLine($"  Credential '{cred}': {(result ? "SUCCESS" : "FAILED")}");
                }
            }
            Console.WriteLine();
        }

        static void DemonstrateModernFeatures()
        {
            Console.WriteLine("8. Modern C# Features with Inheritance");
            Console.WriteLine("======================================");
            
            var portfolio = new ModernPortfolio { Name = "Tech Portfolio" };
            
            // Using object initializers with required properties
            var modernStock = new ModernStock("AAPL", 175.25m, 100)
            {
                Name = "Apple Inc.",
                InitialValue = 15000m
            };
            
            var modernBond = new ModernBond("US Treasury", 10000m, 0.03m, DateTime.Now.AddYears(5))
            {
                Name = "5-Year Treasury",
                InitialValue = 10000m
            };
            
            var realEstate = new ModernRealEstate
            {
                Name = "Downtown Condo",
                InitialValue = 300000m,
                Address = "123 Main St",
                SquareFootage = 1200m,
                PricePerSquareFoot = 350m
            };
            
            portfolio.AddAsset(modernStock);
            portfolio.AddAsset(modernBond);
            portfolio.AddAsset(realEstate);
            
            portfolio.DisplayPortfolio();
            
            // Demonstrate pattern matching with inheritance
            Console.WriteLine("Asset categorization using pattern matching:");
            var assets = new ModernAsset[] { modernStock, modernBond, realEstate };
            foreach (var asset in assets)
            {
                Console.WriteLine($"{asset.Name}: {asset.GetAssetCategory()} - {asset.GetAssetType()}");
            }
            Console.WriteLine();
        }

        static void DemonstrateCastingAndTypeChecking()
        {
            Console.WriteLine("9. Type Casting and Checking");
            Console.WriteLine("============================");
            
            Asset[] assets = 
            {
                new Stock { Name = "Tesla", SharesOwned = 10, CurrentPrice = 800m },
                new House { Name = "Beach House", EstimatedValue = 750000m, Mortgage = 500000m },
                new Asset { Name = "Generic Asset" }
            };
            
            Console.WriteLine("Demonstrating safe casting and type checking:");
            
            foreach (Asset asset in assets)
            {
                Console.WriteLine($"Processing asset: {asset.Name}");
                
                // Using 'is' operator with pattern variable (C# 7+)
                if (asset is Stock stock)
                {
                    Console.WriteLine($"  This is a stock with {stock.SharesOwned} shares worth ${stock.TotalValue:F2}");
                }
                else if (asset is House house)
                {
                    Console.WriteLine($"  This is a house with ${house.Equity:F2} equity");
                }
                else
                {
                    Console.WriteLine($"  This is a generic asset");
                }
                
                // Using 'as' operator for safe casting
                Stock? possibleStock = asset as Stock;
                if (possibleStock != null)
                {
                    Console.WriteLine($"  Safe cast successful: Stock value = ${possibleStock.TotalValue:F2}");
                }
                
                Console.WriteLine();
            }
            
            // Demonstrate upcasting and downcasting
            Console.WriteLine("Upcasting and Downcasting:");
            Stock msft = new Stock { Name = "Microsoft", SharesOwned = 50, CurrentPrice = 350m };
            Asset assetRef = msft; // Upcast (implicit)
            
            Console.WriteLine($"Upcast successful: {assetRef.Name}");
            
            // Downcast (explicit)
            if (assetRef is Stock downcastStock)
            {
                Console.WriteLine($"Downcast successful: {downcastStock.SharesOwned} shares");
            }
            
            Console.WriteLine();
        }
    }
}
```

### Step 10: Build and Run the Project

1. Save all files in your project directory
2. Open a terminal in the project directory
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the project:
   ```bash
   dotnet run
   ```

## Expected Output

When you run the project, you should see comprehensive demonstrations of:
- Basic inheritance with Asset, Stock, and House classes
- Polymorphic behavior with virtual method calls
- Abstract classes that cannot be instantiated
- Constructor chaining through inheritance hierarchies
- The difference between method hiding and overriding
- Sealed classes and methods preventing further inheritance
- Modern C# features like primary constructors and required members
- Safe type casting and checking techniques

## Key Learning Points

After completing this tutorial, you will understand:

1. **Inheritance Hierarchy Design**: How to create logical "is-a" relationships between classes
2. **Polymorphism**: How virtual methods enable different behaviors for the same method call
3. **Abstract Classes**: When and how to use classes that cannot be instantiated
4. **Constructor Chaining**: How derived classes properly initialize their base classes
5. **Method Hiding vs Override**: The crucial difference between `new` and `override` keywords
6. **Sealed Members**: How to prevent further inheritance or overriding
7. **Type Safety**: Safe casting techniques using `is`, `as`, and pattern matching
8. **Modern C# Features**: Primary constructors, required members, and pattern matching

## Best Practices Learned

- Use inheritance for true "is-a" relationships, not convenience
- Make methods virtual if you expect them to be overridden
- Call base constructors explicitly when necessary
- Use abstract classes for shared implementation with required overrides
- Prefer composition over inheritance when the relationship is not clearly "is-a"
- Use sealed classes and methods judiciously to prevent unintended inheritance
- Always use safe casting techniques in production code

## Common Pitfalls to Avoid

1. **Calling virtual methods in constructors** - Can lead to unexpected behavior
2. **Deep inheritance hierarchies** - Make code difficult to understand and maintain  
3. **Using inheritance for code reuse** - Consider composition instead
4. **Forgetting to call base constructors** - Can lead to improper initialization
5. **Confusing method hiding with overriding** - They behave differently with polymorphism

## Extension Exercises

To further practice these concepts, try:

1. Create an animal hierarchy with mammals, birds, and fish
2. Build a geometric shape system with area and perimeter calculations
3. Design an employee management system with different types of workers
4. Implement a file system with files, directories, and symbolic links
5. Create a game entity system with players, enemies, and power-ups

## Troubleshooting

If you encounter issues:

- **Build errors**: Check that all using statements are correct
- **Method not found**: Ensure you are calling methods on the correct object type
- **InvalidCastException**: Use safe casting with `is` or `as` operators
- **Constructor errors**: Verify that base class constructors are properly called

## Additional Resources

- Microsoft C# Documentation on Inheritance
- Object-Oriented Programming Principles
- Design Patterns in C#
- SOLID Principles and Inheritance

This comprehensive tutorial provides a solid foundation for understanding inheritance in C#. The concepts learned here are fundamental to object-oriented programming and will be essential for building larger, more complex applications.
