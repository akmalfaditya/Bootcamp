using System;
using System.Diagnostics.CodeAnalysis;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates constructor inheritance patterns and required members in C#.
    /// Constructors are not inherited, so derived classes must explicitly define how to initialize
    /// both their own members and their base class members.
    /// 
    /// Key concepts:
    /// 1. Constructor chaining with 'base' keyword
    /// 2. Implicit calling of parameterless base constructor
    /// 3. Constructor and field initialization order
    /// 4. Required members (C# 11+)
    /// 5. Primary constructors (C# 12+)
    /// </summary>

    // Base class demonstrating various constructor patterns
    public class Computer
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime ManufactureDate { get; set; }
        
        // Field that gets initialized during construction
        private readonly string _serialNumber;
        
        // Field with initializer - this runs before constructor body
        public int WarrantyMonths = 12;
        
        // Parameterless constructor
        public Computer()
        {
            Console.WriteLine("Computer parameterless constructor executing...");
            _serialNumber = GenerateSerialNumber();
            ManufactureDate = DateTime.Now;
            Console.WriteLine($"Computer initialized with serial: {_serialNumber}");
        }
        
        // Constructor with basic parameters
        public Computer(string brand, string model) : this() // Chain to parameterless constructor
        {
            Console.WriteLine("Computer(brand, model) constructor executing...");
            Brand = brand;
            Model = model;
            Console.WriteLine($"Computer brand and model set: {Brand} {Model}");
        }
        
        // Constructor with all parameters
        public Computer(string brand, string model, decimal price, int warrantyMonths)
        {
            Console.WriteLine("Computer(all parameters) constructor executing...");
            _serialNumber = GenerateSerialNumber();
            ManufactureDate = DateTime.Now;
            Brand = brand;
            Model = model;
            Price = price;
            WarrantyMonths = warrantyMonths;
            Console.WriteLine($"Computer fully initialized: {Brand} {Model} - ${Price}");
        }
        
        private string GenerateSerialNumber()
        {
            return $"PC{DateTime.Now.Ticks % 100000000}";
        }
        
        public virtual void DisplaySpecs()
        {
            Console.WriteLine($"Computer: {Brand} {Model}");
            Console.WriteLine($"Price: ${Price:F2}");
            Console.WriteLine($"Serial: {_serialNumber}");
            Console.WriteLine($"Manufactured: {ManufactureDate:yyyy-MM-dd}");
            Console.WriteLine($"Warranty: {WarrantyMonths} months");
        }
        
        public string GetSerialNumber() => _serialNumber;
    }

    // First derived class - Laptop
    public class Laptop : Computer
    {
        public double ScreenSize { get; set; }
        public int BatteryLifeHours { get; set; }
        public bool HasTouchscreen { get; set; }
        
        // Field initializer - runs before constructor
        public bool IsPortable = true;
        
        // If we don't define any constructors, the compiler creates a default one
        // that calls the base parameterless constructor
        // Let's define explicit constructors to show the chaining
        
        // Parameterless constructor - must explicitly call base constructor
        public Laptop() : base() // This is actually optional - base() is called implicitly
        {
            Console.WriteLine("Laptop parameterless constructor executing...");
            ScreenSize = 15.6;
            BatteryLifeHours = 8;
            HasTouchscreen = false;
            Console.WriteLine("Laptop default values set");
        }
        
        // Constructor that takes basic laptop info
        public Laptop(string brand, string model, double screenSize)
            : base(brand, model) // Call the base constructor with brand and model
        {
            Console.WriteLine("Laptop(brand, model, screenSize) constructor executing...");
            ScreenSize = screenSize;
            BatteryLifeHours = 8; // Default value
            HasTouchscreen = false; // Default value
            Console.WriteLine($"Laptop screen size set to {ScreenSize} inches");
        }
        
        // Full constructor
        public Laptop(string brand, string model, decimal price, double screenSize, int batteryLife, bool touchscreen)
            : base(brand, model, price, 24) // Laptops get 24-month warranty by default
        {
            Console.WriteLine("Laptop full constructor executing...");
            ScreenSize = screenSize;
            BatteryLifeHours = batteryLife;
            HasTouchscreen = touchscreen;
            Console.WriteLine($"Laptop fully configured: {ScreenSize}\" screen, {BatteryLifeHours}h battery");
        }
        
        public override void DisplaySpecs()
        {
            base.DisplaySpecs(); // Call base implementation first
            Console.WriteLine($"Screen Size: {ScreenSize} inches");
            Console.WriteLine($"Battery Life: {BatteryLifeHours} hours");
            Console.WriteLine($"Touchscreen: {HasTouchscreen}");
            Console.WriteLine($"Portable: {IsPortable}");
        }
    }

    // Second derived class - Desktop
    public class Desktop : Computer
    {
        public string TowerType { get; set; } = "Mid Tower";
        public int PowerSupplyWatts { get; set; }
        public bool HasDedicatedGPU { get; set; }
        
        // Field initializer
        public bool RequiresMonitor = true;
        
        // Constructor that shows different base constructor calling patterns
        public Desktop() // Implicitly calls base() - the parameterless constructor
        {
            Console.WriteLine("Desktop parameterless constructor executing...");
            PowerSupplyWatts = 500;
            HasDedicatedGPU = false;
            Console.WriteLine("Desktop default configuration set");
        }
        
        // Constructor that demonstrates argument evaluation order
        public Desktop(string brand, string model, int powerWatts)
            : base(brand, model, CalculateDesktopPrice(powerWatts), 36) // Arguments evaluated before base call
        {
            Console.WriteLine("Desktop(brand, model, powerWatts) constructor executing...");
            PowerSupplyWatts = powerWatts;
            HasDedicatedGPU = powerWatts > 600; // High wattage suggests dedicated GPU
            TowerType = powerWatts > 750 ? "Full Tower" : "Mid Tower";
            Console.WriteLine($"Desktop configured: {PowerSupplyWatts}W PSU, GPU: {HasDedicatedGPU}");
        }
        
        // Static method used in constructor argument - this gets called before base constructor
        private static decimal CalculateDesktopPrice(int powerWatts)
        {
            Console.WriteLine($"Calculating desktop price for {powerWatts}W system...");
            decimal basePrice = 800;
            decimal powerBonus = powerWatts * 0.5m; // More power = higher price
            return basePrice + powerBonus;
        }
        
        public override void DisplaySpecs()
        {
            base.DisplaySpecs();
            Console.WriteLine($"Tower Type: {TowerType}");
            Console.WriteLine($"Power Supply: {PowerSupplyWatts}W");
            Console.WriteLine($"Dedicated GPU: {HasDedicatedGPU}");
            Console.WriteLine($"Requires Monitor: {RequiresMonitor}");
        }
    }

    // Demonstrating required members (C# 11+)
    public class ModernComputer
    {
        // Required fields - must be set via object initializer or constructor
        public required string ProcessorModel { get; set; }
        public required int RAM_GB { get; set; }
        public required string StorageType { get; set; }
        
        // Optional properties
        public string OperatingSystem { get; set; } = "Windows 11";
        public decimal Price { get; set; }
        
        // Parameterless constructor - required fields still need to be set by caller
        public ModernComputer()
        {
            Console.WriteLine("ModernComputer parameterless constructor - required fields must be set via object initializer");
        }
        
        // Constructor that satisfies required members
        [SetsRequiredMembers]
        public ModernComputer(string processor, int ram, string storage)
        {
            Console.WriteLine("ModernComputer constructor satisfying required members");
            ProcessorModel = processor;
            RAM_GB = ram;
            StorageType = storage;
        }
        
        // Full constructor
        [SetsRequiredMembers]
        public ModernComputer(string processor, int ram, string storage, string os, decimal price)
        {
            Console.WriteLine("ModernComputer full constructor");
            ProcessorModel = processor;
            RAM_GB = ram;
            StorageType = storage;
            OperatingSystem = os;
            Price = price;
        }
        
        public void DisplayConfiguration()
        {
            Console.WriteLine($"Modern Computer Configuration:");
            Console.WriteLine($"  Processor: {ProcessorModel}");
            Console.WriteLine($"  RAM: {RAM_GB}GB");
            Console.WriteLine($"  Storage: {StorageType}");
            Console.WriteLine($"  OS: {OperatingSystem}");
            Console.WriteLine($"  Price: ${Price:F2}");
        }
    }

    // Derived class from ModernComputer
    public class GamingPC : ModernComputer
    {
        public required string GraphicsCard { get; set; } // Additional required member
        public int CoolingFans { get; set; } = 3;
        public bool HasRGBLighting { get; set; } = true;
        
        // Constructor that must handle both base and derived required members
        [SetsRequiredMembers]
        public GamingPC(string processor, int ram, string storage, string graphicsCard)
            : base(processor, ram, storage) // Base constructor handles base required members
        {
            Console.WriteLine("GamingPC constructor - setting gaming-specific required members");
            GraphicsCard = graphicsCard; // Handle derived required member
        }
        
        // Full constructor
        [SetsRequiredMembers]
        public GamingPC(string processor, int ram, string storage, string graphicsCard, int fans, bool rgb, decimal price)
            : base(processor, ram, storage, "Windows 11 Gaming Edition", price)
        {
            GraphicsCard = graphicsCard;
            CoolingFans = fans;
            HasRGBLighting = rgb;
            Console.WriteLine("GamingPC fully configured for gaming");
        }
        
        public void DisplayGamingSpecs()
        {
            DisplayConfiguration(); // Call base method
            Console.WriteLine($"  Graphics Card: {GraphicsCard}");
            Console.WriteLine($"  Cooling Fans: {CoolingFans}");
            Console.WriteLine($"  RGB Lighting: {HasRGBLighting}");
        }
    }

    // Demonstrating Primary Constructors (C# 12+)
    public class SimpleDevice(string name, string type, decimal price)
    {
        // Primary constructor parameters are automatically available as private fields
        public string Name => name;
        public string Type => type;
        public decimal Price => price;
        
        // Additional properties can be defined normally
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        
        public void DisplayDevice()
        {
            Console.WriteLine($"Device: {name} ({type}) - ${price:F2}");
            Console.WriteLine($"Purchased: {PurchaseDate:yyyy-MM-dd}");
        }
    }

    // Derived class using primary constructor
    public class SmartDevice(string name, string type, decimal price, string connectivity)
        : SimpleDevice(name, type, price) // Call base primary constructor
    {
        public string Connectivity => connectivity;
        public bool IsConnected { get; set; } = false;
        
        public void Connect()
        {
            IsConnected = true;
            Console.WriteLine($"{Name} connected via {connectivity}");
        }
    }

    /// <summary>
    /// Demonstration class for constructor inheritance patterns
    /// </summary>
    public static class ConstructorInheritanceDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== CONSTRUCTOR INHERITANCE DEMONSTRATION ===\n");
            
            // 1. DEMONSTRATE BASIC CONSTRUCTOR CHAINING
            Console.WriteLine("1. BASIC CONSTRUCTOR CHAINING:");
            Console.WriteLine("Watch the order of constructor execution\n");
            
            Console.WriteLine("--- Creating laptop with parameterless constructor ---");
            var basicLaptop = new Laptop();
            Console.WriteLine();
            
            Console.WriteLine("--- Creating laptop with partial parameters ---");
            var gamingLaptop = new Laptop("ASUS", "ROG Strix", 17.3);
            Console.WriteLine();
            
            Console.WriteLine("--- Creating laptop with full parameters ---");
            var premiumLaptop = new Laptop("Apple", "MacBook Pro", 2499.99m, 16.0, 12, true);
            Console.WriteLine();
            
            // 2. DEMONSTRATE INITIALIZATION ORDER
            Console.WriteLine("2. INITIALIZATION ORDER:");
            Console.WriteLine("Fields initialize before constructor bodies execute\n");
            
            Console.WriteLine("--- Creating desktop (shows argument evaluation order) ---");
            var workstation = new Desktop("Dell", "Precision", 850);
            Console.WriteLine();
            
            // 3. DEMONSTRATE IMPLICIT BASE CONSTRUCTOR CALLS
            Console.WriteLine("3. IMPLICIT BASE CONSTRUCTOR CALLS:");
            Console.WriteLine("When no base() is specified, parameterless base constructor is called\n");
            
            var simpleDesktop = new Desktop();
            Console.WriteLine();
            
            // 4. DISPLAY ALL COMPUTER SPECS
            Console.WriteLine("4. DISPLAYING COMPUTER SPECIFICATIONS:");
            Console.WriteLine("Virtual methods work correctly through inheritance\n");
            
            Console.WriteLine("--- Basic Laptop ---");
            basicLaptop.DisplaySpecs();
            Console.WriteLine();
            
            Console.WriteLine("--- Gaming Laptop ---");
            gamingLaptop.DisplaySpecs();
            Console.WriteLine();
            
            Console.WriteLine("--- Premium Laptop ---");
            premiumLaptop.DisplaySpecs();
            Console.WriteLine();
            
            Console.WriteLine("--- Workstation Desktop ---");
            workstation.DisplaySpecs();
            Console.WriteLine();
            
            // 5. DEMONSTRATE REQUIRED MEMBERS (C# 11+)
            Console.WriteLine("5. REQUIRED MEMBERS (C# 11+):");
            Console.WriteLine("Required members must be set via object initializer or satisfied by constructor\n");
            
            // Using object initializer with parameterless constructor
            var modernPC1 = new ModernComputer
            {
                ProcessorModel = "Intel i7-13700K",
                RAM_GB = 32,
                StorageType = "1TB NVMe SSD",
                Price = 1899.99m
            };
            
            // Using constructor that satisfies required members
            var modernPC2 = new ModernComputer("AMD Ryzen 9 7900X", 64, "2TB NVMe SSD");
            
            // Using full constructor
            var modernPC3 = new ModernComputer("Intel i9-13900K", 128, "4TB NVMe SSD", "Windows 11 Pro", 3499.99m);
            
            Console.WriteLine("--- Modern PC with object initializer ---");
            modernPC1.DisplayConfiguration();
            Console.WriteLine();
            
            Console.WriteLine("--- Modern PC with basic constructor ---");
            modernPC2.DisplayConfiguration();
            Console.WriteLine();
            
            Console.WriteLine("--- Modern PC with full constructor ---");
            modernPC3.DisplayConfiguration();
            Console.WriteLine();
            
            // 6. GAMING PC WITH MULTIPLE REQUIRED MEMBERS
            Console.WriteLine("6. GAMING PC WITH INHERITED REQUIRED MEMBERS:");
            
            // Using object initializer with parameterless constructor - requires parameterless constructor
            var gamingPC1 = new GamingPC("Intel i9-13900KF", 64, "2TB NVMe SSD", "RTX 4090")
            {
                Price = 4999.99m
            };
            
            // Using constructor
            var gamingPC2 = new GamingPC("AMD Ryzen 9 7950X", 128, "4TB NVMe SSD", "RTX 4090", 6, true, 5999.99m);
            
            Console.WriteLine("--- Gaming PC with object initializer ---");
            gamingPC1.DisplayGamingSpecs();
            Console.WriteLine();
            
            Console.WriteLine("--- Gaming PC with full constructor ---");
            gamingPC2.DisplayGamingSpecs();
            Console.WriteLine();
            
            // 7. PRIMARY CONSTRUCTORS (C# 12+)
            Console.WriteLine("7. PRIMARY CONSTRUCTORS (C# 12+):");
            Console.WriteLine("Concise syntax for simple constructor scenarios\n");
            
            var tablet = new SimpleDevice("iPad Pro", "Tablet", 1099.99m);
            var smartWatch = new SmartDevice("Apple Watch", "Wearable", 399.99m, "Bluetooth");
            
            Console.WriteLine("--- Simple Device ---");
            tablet.DisplayDevice();
            Console.WriteLine();
            
            Console.WriteLine("--- Smart Device ---");
            smartWatch.DisplayDevice();
            smartWatch.Connect();
            Console.WriteLine();
            
            // 8. POLYMORPHIC USAGE
            Console.WriteLine("8. POLYMORPHIC USAGE WITH CONSTRUCTORS:");
            Computer[] computers = { basicLaptop, premiumLaptop, workstation };
            
            foreach (Computer computer in computers)
            {
                Console.WriteLine($"Computer Serial: {computer.GetSerialNumber()}");
                computer.DisplaySpecs();
                Console.WriteLine("---");
            }
        }
    }
}
