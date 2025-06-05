# Type Basics in C#

## Learning Objectives
By the end of this module, you will learn:
- **C# type system** fundamentals and categorization
- **Value types vs reference types** and their behavioral differences
- **Built-in types** (predefined) and custom type creation
- **Type conversion** mechanisms (implicit, explicit, custom)
- **Object instantiation** and memory allocation patterns
- **Static vs instance members** and their appropriate usage
- **Type safety** principles and compile-time verification
- **Boxing and unboxing** concepts and performance implications

## Core Concepts Covered

### 1. C# Type System Hierarchy
- **System.Object**: Universal base type for all C# types
- **Value types**: Data stored directly (structs, enums, built-in types)
- **Reference types**: Data stored on heap with reference on stack
- **Nullable types**: Value types that can represent null
- **Pointer types**: Unsafe code memory manipulation (advanced)

### 2. Type Categories
- **Built-in types**: Predefined by C# language (int, string, bool)
- **User-defined types**: Custom classes, structs, interfaces, enums
- **Generic types**: Type-safe containers and methods
- **Anonymous types**: Compiler-generated types for temporary data
- **Dynamic types**: Runtime type resolution

### 3. Memory Management
- **Stack allocation**: Value types and references
- **Heap allocation**: Object instances and reference type data
- **Garbage collection**: Automatic memory cleanup
- **Object lifecycle**: Creation, usage, and disposal

## Key Features & Examples

### Value Types vs Reference Types
```csharp
// Value type (struct) - stored on stack
public struct Point
{
    public int X, Y;
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

// Reference type (class) - stored on heap
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

// Demonstrating the difference
void DemonstrateValueVsReference()
{
    // Value type behavior - copies the actual data
    Point p1 = new Point(10, 20);
    Point p2 = p1; // Creates a copy of p1's data
    p2.X = 30;     // Only changes p2, p1 remains unchanged
    
    Console.WriteLine($"p1: ({p1.X}, {p1.Y})"); // Output: p1: (10, 20)
    Console.WriteLine($"p2: ({p2.X}, {p2.Y})"); // Output: p2: (30, 20)
    
    // Reference type behavior - copies the reference
    Person person1 = new Person("Alice", 25);
    Person person2 = person1; // Copies the reference, not the object
    person2.Age = 30;         // Changes the shared object
    
    Console.WriteLine($"person1: {person1.Age}"); // Output: person1: 30
    Console.WriteLine($"person2: {person2.Age}"); // Output: person2: 30
}
```

### Built-in Types and Their Characteristics
```csharp
// Integral types
byte smallNumber = 255;           // 8-bit unsigned: 0 to 255
sbyte signedSmall = -128;         // 8-bit signed: -128 to 127
short shortNumber = 32767;        // 16-bit signed: -32,768 to 32,767
ushort unsignedShort = 65535;     // 16-bit unsigned: 0 to 65,535
int number = 2147483647;          // 32-bit signed: -2.1B to 2.1B
uint unsignedNumber = 4294967295; // 32-bit unsigned: 0 to 4.3B
long bigNumber = 9223372036854775807;     // 64-bit signed
ulong unsignedBig = 18446744073709551615; // 64-bit unsigned

// Floating-point types
float precision = 3.14159f;       // 32-bit: ~7 digits precision
double doublePrecision = 3.141592653589793; // 64-bit: ~15-17 digits
decimal money = 123.456789m;      // 128-bit: 28-29 digits (financial)

// Character and text types
char letter = 'A';                // Single Unicode character
string text = "Hello, World!";    // Immutable string of characters

// Boolean type
bool isActive = true;             // true or false only

// Special types
object anything = "Can hold any type"; // Universal base type
dynamic runtime = "Resolved at runtime"; // Dynamic typing
```

### Type Conversions
```csharp
// Implicit conversions (safe, no data loss)
int intValue = 42;
long longValue = intValue;        // int to long (widening)
double doubleValue = intValue;    // int to double

// Explicit conversions (potential data loss)
double sourceDouble = 123.456;
int truncated = (int)sourceDouble; // Result: 123 (decimal part lost)

// Parsing from strings
string numberText = "42";
int parsed = int.Parse(numberText);
bool success = int.TryParse("invalid", out int result);

// Custom conversion operators
public struct Temperature
{
    public double Celsius { get; }
    
    public Temperature(double celsius) => Celsius = celsius;
    
    // Implicit conversion from double
    public static implicit operator Temperature(double celsius)
        => new Temperature(celsius);
    
    // Explicit conversion to double
    public static explicit operator double(Temperature temp)
        => temp.Celsius;
}

// Usage of custom conversions
Temperature temp = 25.0;           // Implicit conversion
double celsius = (double)temp;     // Explicit conversion
```

### Static vs Instance Members
```csharp
public class Calculator
{
    // Instance field - each Calculator object has its own
    private double _lastResult;
    
    // Static field - shared by all Calculator instances
    private static int _calculationCount;
    
    // Instance property
    public double LastResult => _lastResult;
    
    // Static property
    public static int TotalCalculations => _calculationCount;
    
    // Instance method - operates on specific object
    public double Add(double a, double b)
    {
        _lastResult = a + b;
        _calculationCount++; // Can access static members
        return _lastResult;
    }
    
    // Static method - operates on class level
    public static double Multiply(double a, double b)
    {
        _calculationCount++; // Can only access static members
        return a * b;
    }
    
    // Static constructor - runs once when class is first used
    static Calculator()
    {
        _calculationCount = 0;
        Console.WriteLine("Calculator class initialized");
    }
}

// Usage demonstration
Calculator calc1 = new Calculator();
Calculator calc2 = new Calculator();

double result1 = calc1.Add(5, 3);        // Instance method call
double result2 = Calculator.Multiply(4, 2); // Static method call

Console.WriteLine($"Calc1 last result: {calc1.LastResult}"); // 8
Console.WriteLine($"Calc2 last result: {calc2.LastResult}"); // 0
Console.WriteLine($"Total calculations: {Calculator.TotalCalculations}"); // 2
```

### Boxing and Unboxing
```csharp
// Boxing - converting value type to reference type
int value = 42;
object boxed = value;              // Boxing: int → object
Console.WriteLine(boxed.GetType()); // System.Int32

// Unboxing - converting reference type back to value type
object boxedValue = 123;
int unboxed = (int)boxedValue;     // Unboxing: object → int

// Performance implications
void DemonstrateBoxingPerformance()
{
    // Inefficient: Boxing occurs in each iteration
    for (int i = 0; i < 1000000; i++)
    {
        object boxed = i; // Boxing allocation on heap
    }
    
    // Efficient: No boxing with generic collections
    List<int> numbers = new List<int>();
    for (int i = 0; i < 1000000; i++)
    {
        numbers.Add(i); // No boxing required
    }
}
```

## Tips

### Performance Considerations
- **Value types** are generally faster for small data structures
- **Reference types** are better for larger objects and when you need identity semantics
- **Boxing/unboxing** creates heap allocations - avoid in performance-critical code
- **struct vs class** choice impacts performance and semantics

```csharp
// Use structs for small, immutable data
public readonly struct Vector3
{
    public readonly float X, Y, Z;
    
    public Vector3(float x, float y, float z)
    {
        X = x; Y = y; Z = z;
    }
}

// Use classes for entities and mutable objects
public class Customer
{
    public string Name { get; set; }
    public List<Order> Orders { get; set; } = new();
}

// Avoid boxing in hot paths
List<int> numbers = new List<int>(); // Generic - no boxing
ArrayList list = new ArrayList();    // Non-generic - boxing occurs
```

### Memory Management
```csharp
// Understand object lifecycle
public class ResourceManager
{
    public void ProcessData()
    {
        // Stack allocated - automatically cleaned up
        int localVar = 42;
        
        // Heap allocated - garbage collected when no references remain
        var data = new LargeDataSet();
        
        // Process data...
        
    } // localVar goes out of scope, data eligible for GC
}

// Use using statements for disposable resources
using (var file = new FileStream("data.txt", FileMode.Open))
{
    // File automatically closed and disposed
}

// Be aware of reference cycles
public class Parent
{
    public List<Child> Children { get; set; } = new();
}

public class Child
{
    public Parent Parent { get; set; } // Can create reference cycles
}
```

### Type Safety Best Practices
```csharp
// Use specific types instead of object when possible
List<Customer> customers = new(); // Type-safe
// ArrayList customers = new();   // Not type-safe

// Validate type conversions
public T SafeCast<T>(object value) where T : class
{
    return value as T; // Returns null if cast fails
}

// Use pattern matching for type checks
public void ProcessValue(object value)
{
    switch (value)
    {
        case int i:
            Console.WriteLine($"Integer: {i}");
            break;
        case string s:
            Console.WriteLine($"String: {s}");
            break;
        case null:
            Console.WriteLine("Null value");
            break;
        default:
            Console.WriteLine($"Unknown type: {value.GetType()}");
            break;
    }
}
```

## 🎓 Best Practices & Guidelines

### 1. Choosing Between Value and Reference Types
```csharp
// Use structs when:
// - Data is small (typically < 16 bytes)
// - Immutable by design
// - No inheritance needed
public readonly struct Point2D
{
    public readonly int X, Y;
    public Point2D(int x, int y) => (X, Y) = (x, y);
}

// Use classes when:
// - Object has identity
// - Mutable state
// - Inheritance relationships
public class BankAccount
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }
    
    public void Deposit(decimal amount) => Balance += amount;
}
```

### 2. Static Member Design
```csharp
public class MathUtilities
{
    // Static for utility methods that don't need state
    public static double CalculateDistance(Point2D p1, Point2D p2)
    {
        int dx = p1.X - p2.X;
        int dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    // Static properties for class-level configuration
    public static double DefaultPrecision { get; set; } = 0.001;
    
    // Static constructor for one-time initialization
    static MathUtilities()
    {
        DefaultPrecision = LoadPrecisionFromConfig();
    }
}
```

### 3. Type Conversion Strategies
```csharp
public class ConversionUtilities
{
    // Safe conversion with TryParse pattern
    public static bool TryConvertToInt(string input, out int result)
    {
        return int.TryParse(input, out result);
    }
    
    // Custom conversion with validation
    public static Temperature ParseTemperature(string input)
    {
        if (!double.TryParse(input, out double celsius))
        {
            throw new FormatException($"Invalid temperature: {input}");
        }
        
        if (celsius < -273.15)
        {
            throw new ArgumentOutOfRangeException(nameof(input), 
                "Temperature cannot be below absolute zero");
        }
        
        return new Temperature(celsius);
    }
}
```

## Real-World Applications

### 1. Data Transfer Objects (DTOs)
```csharp
// Using structs for lightweight data transfer
public readonly struct CustomerDto
{
    public readonly int Id;
    public readonly string Name;
    public readonly string Email;
    
    public CustomerDto(int id, string name, string email)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}

// Using classes for complex entities
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Order> Orders { get; set; } = new();
    public CustomerProfile Profile { get; set; }
    
    public CustomerDto ToDto() => new(Id, Name, Email);
}
```

### 2. Configuration Management
```csharp
public static class AppConfig
{
    // Static properties for application-wide settings
    public static string DatabaseConnectionString { get; private set; }
    public static int MaxRetryAttempts { get; private set; }
    public static TimeSpan RequestTimeout { get; private set; }
    
    // Static constructor for initialization
    static AppConfig()
    {
        LoadConfiguration();
    }
    
    private static void LoadConfiguration()
    {
        DatabaseConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") 
            ?? "Data Source=localhost;Initial Catalog=MyApp";
        MaxRetryAttempts = int.Parse(Environment.GetEnvironmentVariable("MAX_RETRIES") ?? "3");
        RequestTimeout = TimeSpan.FromSeconds(30);
    }
}
```

### 3. Type-Safe Enumerations
```csharp
public class OrderStatus
{
    private readonly string _value;
    private readonly int _priority;
    
    private OrderStatus(string value, int priority)
    {
        _value = value;
        _priority = priority;
    }
    
    public static readonly OrderStatus Pending = new("Pending", 1);
    public static readonly OrderStatus Processing = new("Processing", 2);
    public static readonly OrderStatus Shipped = new("Shipped", 3);
    public static readonly OrderStatus Delivered = new("Delivered", 4);
    public static readonly OrderStatus Cancelled = new("Cancelled", 0);
    
    public override string ToString() => _value;
    
    public bool CanTransitionTo(OrderStatus newStatus)
    {
        return newStatus._priority > _priority || newStatus == Cancelled;
    }
}
```

## Industry Applications

### Software Architecture
- **Domain Modeling**: Designing type-safe business entities
- **API Design**: DTOs for service boundaries
- **Configuration Systems**: Type-safe application settings
- **State Management**: Type-based state representations

### Performance Engineering
- **Memory Optimization**: Strategic value vs reference type usage
- **Boxing Avoidance**: Generic collections and algorithms
- **Cache-Friendly Design**: Struct layout optimization
- **Allocation Reduction**: Stack vs heap allocation strategies

### Enterprise Development
- **Data Access Layers**: Type-safe database mappings
- **Service Boundaries**: Contract-based type definitions
- **Validation Systems**: Type-based validation rules
- **Serialization**: Type-aware data exchange formats

