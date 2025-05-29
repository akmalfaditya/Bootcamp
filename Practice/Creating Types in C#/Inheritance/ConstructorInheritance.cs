using System;

namespace Inheritance
{
    /// <summary>
    /// Base class for demonstrating constructor inheritance patterns
    /// Shows different constructor scenarios and how they work with inheritance
    /// </summary>
    public class BaseExample
    {
        public int X;
        public string Info;

        /// <summary>
        /// Parameterless constructor
        /// This gets called implicitly if derived class doesn't specify otherwise
        /// </summary>
        public BaseExample()
        {
            X = 10;
            Info = "Default base";
            Console.WriteLine("BaseExample parameterless constructor called");
        }

        /// <summary>
        /// Parameterized constructor
        /// Derived classes must explicitly call this with : base(value)
        /// </summary>
        public BaseExample(int x)
        {
            X = x;
            Info = $"Base with X={x}";
            Console.WriteLine($"BaseExample parameterized constructor called with X={x}");
        }

        /// <summary>
        /// Constructor with multiple parameters
        /// Shows more complex constructor inheritance
        /// </summary>
        public BaseExample(int x, string info)
        {
            X = x;
            Info = info;
            Console.WriteLine($"BaseExample full constructor called with X={x}, Info={info}");
        }
    }

    /// <summary>
    /// Derived class showing explicit base constructor calls
    /// Demonstrates how to choose which base constructor to call
    /// </summary>
    public class DerivedExample : BaseExample
    {
        public int Y;

        /// <summary>
        /// Constructor that explicitly calls base constructor with parameter
        /// The : base(x) tells it which base constructor to use
        /// </summary>
        public DerivedExample(int x) : base(x)
        {
            Y = x * 2;
            Console.WriteLine($"DerivedExample constructor called, Y set to {Y}");
        }

        /// <summary>
        /// Parameterless constructor that implicitly calls base()
        /// Since we don't specify : base(...), the parameterless base constructor is called
        /// </summary>
        public DerivedExample()
        {
            Y = 20;
            Console.WriteLine($"DerivedExample parameterless constructor, Y set to {Y}");
        }

        /// <summary>
        /// Constructor showing call to different base constructor
        /// </summary>
        public DerivedExample(int x, string info, int y) : base(x, info)
        {
            Y = y;
            Console.WriteLine($"DerivedExample full constructor, Y set to {Y}");
        }

        /// <summary>
        /// Method to display values from both base and derived class
        /// </summary>
        public void DisplayValues()
        {
            Console.WriteLine($"X (from base): {X}, Y (from derived): {Y}, Info: {Info}");
        }
    }

    /// <summary>
    /// Example showing constructor chaining within the same class
    /// The : this() syntax calls another constructor in the same class
    /// </summary>
    public class ConstructorChainExample
    {
        public string Name;
        public int Value;
        public bool IsActive;

        /// <summary>
        /// Most complete constructor - others will delegate to this one
        /// This is a common pattern - one "master" constructor that does all the work
        /// </summary>
        public ConstructorChainExample(string name, int value, bool isActive)
        {
            Name = name;
            Value = value;
            IsActive = isActive;
            Console.WriteLine($"Master constructor: {name}, {value}, {isActive}");
        }

        /// <summary>
        /// Constructor that delegates to the full constructor
        /// : this(name, 0, true) calls the three-parameter constructor above
        /// </summary>
        public ConstructorChainExample(string name) : this(name, 0, true)
        {
            Console.WriteLine("Single-parameter constructor delegated to master");
        }

        /// <summary>
        /// Another delegating constructor
        /// Shows how you can provide different default values
        /// </summary>
        public ConstructorChainExample(string name, int value) : this(name, value, false)
        {
            Console.WriteLine("Two-parameter constructor delegated to master");
        }

        /// <summary>
        /// Parameterless constructor also delegates
        /// </summary>
        public ConstructorChainExample() : this("Default", 100, true)
        {
            Console.WriteLine("Parameterless constructor delegated to master");
        }
    }

    /// <summary>
    /// Class that inherits from ConstructorChainExample
    /// Shows how constructor chaining works across inheritance levels
    /// </summary>
    public class InheritedChainExample : ConstructorChainExample
    {
        public DateTime CreatedDate;

        /// <summary>
        /// Constructor that calls specific base constructor
        /// The chain becomes: this -> base(name, value) -> base master constructor
        /// </summary>
        public InheritedChainExample(string name, int value) : base(name, value)
        {
            CreatedDate = DateTime.Now;
            Console.WriteLine($"InheritedChainExample constructor, created at {CreatedDate:HH:mm:ss}");
        }

        /// <summary>
        /// Constructor that calls different base constructor
        /// </summary>
        public InheritedChainExample(string name) : base(name)
        {
            CreatedDate = DateTime.Now;
            Console.WriteLine($"InheritedChainExample single-param constructor, created at {CreatedDate:HH:mm:ss}");
        }
    }

    /// <summary>
    /// Example showing field initialization order with inheritance
    /// This demonstrates the precise order of operations during object creation
    /// </summary>
    public class BaseWithFieldInitialization
    {
        // Field initialization happens before constructor body
        public int BaseField = InitializeBaseField();
        
        public BaseWithFieldInitialization(int value)
        {
            Console.WriteLine($"BaseWithFieldInitialization constructor, BaseField = {BaseField}");
        }

        private static int InitializeBaseField()
        {
            Console.WriteLine("Base field initialization");
            return 100;
        }
    }

    /// <summary>
    /// Derived class showing field initialization order
    /// Order: derived fields -> base constructor -> base fields -> derived constructor
    /// </summary>
    public class DerivedWithFieldInitialization : BaseWithFieldInitialization
    {
        // This field is initialized first!
        public int DerivedField = InitializeDerivedField();

        public DerivedWithFieldInitialization(int value) : base(value)
        {
            Console.WriteLine($"DerivedWithFieldInitialization constructor, DerivedField = {DerivedField}");
        }

        private static int InitializeDerivedField()
        {
            Console.WriteLine("Derived field initialization (happens first!)");
            return 200;
        }

        /// <summary>
        /// Method to demonstrate the initialization order
        /// Call this to see the complete sequence of events
        /// </summary>
        public void ShowInitializationOrder()
        {
            Console.WriteLine("=== Initialization Order Demo ===");
            Console.WriteLine("1. Derived field initialization");
            Console.WriteLine("2. Base constructor call");
            Console.WriteLine("3. Base field initialization");
            Console.WriteLine("4. Base constructor body");
            Console.WriteLine("5. Derived constructor body");
            Console.WriteLine($"Final values: BaseField={BaseField}, DerivedField={DerivedField}");
        }
    }
}
