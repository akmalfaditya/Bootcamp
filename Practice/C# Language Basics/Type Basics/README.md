# Understanding C# Type System

This project provides a comprehensive introduction to the C# type system, which forms the foundation of all C# programming. A thorough understanding of types is essential for writing efficient, safe, and maintainable code.

## Objectives

This demonstration explores the fundamental concepts of how C# organizes and manages different kinds of data through its type system. Understanding these concepts is crucial for effective C# development.

## Core Concepts

The following essential topics are covered in this project:

### 1. Type System Fundamentals

**Type Definition:**
A type in C# is a blueprint that defines the structure, behavior, and memory layout of data. Every piece of data in C# belongs to a specific type, which determines what operations can be performed on that data, how much memory it occupies, and how it is stored and accessed. Types serve as contracts that specify what properties and methods are available for objects of that type.

**Type Safety:**
C# is a statically typed language, meaning that type checking occurs at compile time rather than runtime. This static typing provides several advantages:
- **Compile-time Error Detection**: Many programming errors are caught during compilation, preventing runtime failures
- **IntelliSense Support**: Development environments can provide accurate code completion and suggestions
- **Performance Optimization**: The compiler can optimize code more effectively when types are known in advance
- **Code Documentation**: Types serve as self-documenting contracts that make code easier to understand and maintain

**Type Hierarchy:**
The C# type system is organized in a hierarchical structure with `System.Object` at the root. All types in C# ultimately derive from this base type, either directly or indirectly. This hierarchy enables polymorphism, where objects of derived types can be treated as instances of their base types. The type hierarchy consists of:
- **Value Types**: Derived from `System.ValueType` (which inherits from `System.Object`)
- **Reference Types**: Directly inherit from `System.Object` or other reference types
- **Special Types**: Such as interfaces, delegates, and arrays that have their own inheritance rules

### 2. Value Types vs Reference Types

The distinction between value types and reference types is fundamental to understanding how C# manages memory and handles data operations.

**Value Types:**
Value types store their data directly in the memory location where the variable is declared. When you assign a value type to another variable, the actual data is copied, creating two independent instances. Value types are typically allocated on the stack, which provides fast access and automatic cleanup when the variable goes out of scope.

**Characteristics of Value Types:**
- Store data directly in their memory location
- Assignment creates a complete copy of the data
- Each variable has its own independent copy of the data
- Generally allocated on the stack (though can be on the heap when part of reference types)
- Cannot be null (unless using nullable value types with `?` syntax)
- Inherit from `System.ValueType`

**Reference Types:**
Reference types store a reference (memory address) to the actual data, which is located elsewhere in memory (typically on the heap). When you assign a reference type to another variable, only the reference is copied, not the actual data. This means both variables point to the same object in memory.

**Characteristics of Reference Types:**
- Store a reference to the actual data location
- Assignment copies the reference, not the data
- Multiple variables can reference the same object
- Allocated on the managed heap
- Can be null
- Inherit directly from `System.Object` or other reference types

**Memory Allocation:**
- **Stack Memory**: Fast, automatically managed, limited in size, used for value types and method parameters
- **Heap Memory**: Larger, managed by garbage collector, used for reference types and dynamic allocation

**Assignment Behavior:**
The copying behavior differs significantly between value and reference types, which has important implications for program logic and performance.

```csharp
// Value type (struct) - stored on stack
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

### 3. Built-in Types

C# provides a comprehensive set of built-in types that cover the most common data representation needs. These types are optimized for performance and provide the foundation for more complex data structures.

**Numeric Types:**
Numeric types are designed to handle mathematical operations and come in several categories based on the range of values they can represent and their precision requirements.

**Integer Types:**
- **Signed Integers**: Can represent positive and negative whole numbers
  - `sbyte`: 8-bit signed (-128 to 127)
  - `short`: 16-bit signed (-32,768 to 32,767)
  - `int`: 32-bit signed (-2,147,483,648 to 2,147,483,647) - most commonly used
  - `long`: 64-bit signed (very large range for big numbers)

- **Unsigned Integers**: Can only represent positive whole numbers but with larger positive range
  - `byte`: 8-bit unsigned (0 to 255)
  - `ushort`: 16-bit unsigned (0 to 65,535)
  - `uint`: 32-bit unsigned (0 to 4,294,967,295)
  - `ulong`: 64-bit unsigned (0 to very large positive numbers)

**Floating-Point Types:**
- `float`: 32-bit single precision, approximately 7 decimal digits of precision
- `double`: 64-bit double precision, approximately 15-17 decimal digits of precision
- `decimal`: 128-bit high precision, 28-29 decimal digits, designed for financial calculations

**Text Types:**
- `char`: Represents a single Unicode character (16-bit)
- `string`: Represents an immutable sequence of Unicode characters

**Boolean Type:**
- `bool`: Represents logical values, can only be `true` or `false`

**Default Values:**
Every type in C# has a default value that is automatically assigned when a variable is declared but not explicitly initialized:
- Numeric types default to `0`
- `bool` defaults to `false`
- `char` defaults to `\0` (null character)
- Reference types default to `null`
- `DateTime` defaults to `01/01/0001 00:00:00`

Understanding default values is crucial for avoiding unexpected behavior in applications, especially when working with arrays or class fields that are not explicitly initialized.

### 4. Custom Types

C# allows developers to create custom types that model domain-specific concepts and combine multiple pieces of related data and behavior into cohesive units.

**Classes:**
Classes are reference types that serve as blueprints for creating objects. They encapsulate data (fields and properties) and behavior (methods) into a single unit, supporting the object-oriented programming paradigm.

**Key Characteristics of Classes:**
- Reference type semantics (stored on heap, assignment copies references)
- Support inheritance and polymorphism
- Can contain fields, properties, methods, events, and nested types
- Support access modifiers to control visibility
- Can implement interfaces and inherit from other classes
- Support virtual methods and method overriding

**Structures:**
Structures (structs) are value types designed for lightweight data containers. They are ideal for representing simple data that does not require reference semantics or inheritance.

**Key Characteristics of Structures:**
- Value type semantics (stored on stack when possible, assignment copies data)
- Cannot inherit from other structs or classes (except implicitly from `ValueType`)
- Cannot be inherited by other types
- All fields must be initialized before the struct can be used
- Recommended for immutable data types smaller than 16 bytes
- Automatically provide value equality semantics

**When to Use Classes vs Structures:**
- Use classes for entities with identity, mutable state, or complex behavior
- Use structs for small, immutable data that represents a single value
- Use classes when you need inheritance or reference semantics
- Use structs when you want value semantics and performance optimization

**Constructors:**
Constructors are special methods responsible for initializing new instances of types. They ensure that objects are created in a valid state and allow for parameterized initialization.

**Types of Constructors:**
- **Instance Constructors**: Initialize individual instances of classes or structs
- **Static Constructors**: Initialize static members and run once per type
- **Default Constructors**: Provided automatically if no constructors are defined
- **Parameterized Constructors**: Accept parameters to customize initialization

**Constructor Characteristics:**
- Must have the same name as the containing type
- Do not have return types
- Can be overloaded with different parameter signatures
- Can call other constructors using `this()` or `base()` syntax
- Static constructors are called automatically before first use of the type

### 5. Type Conversions

Type conversions allow data to be transformed from one type to another, enabling interoperability between different data types while maintaining type safety.

**Implicit Conversions:**
Implicit conversions are performed automatically by the compiler when no data loss can occur. These conversions are considered safe because the target type can accommodate all possible values from the source type.

**Characteristics of Implicit Conversions:**
- No explicit syntax required
- Guaranteed to succeed without data loss
- Commonly used for numeric widening (smaller to larger types)
- Include conversions from derived types to base types
- Cannot result in exceptions

**Common Implicit Conversions:**
- `int` to `long`, `float`, `double`, or `decimal`
- `float` to `double`
- Derived class to base class
- Any type to `object`

**Explicit Conversions:**
Explicit conversions require explicit syntax and may result in data loss or runtime exceptions. They are used when the conversion might not preserve all information or when the compiler cannot guarantee safety.

**Characteristics of Explicit Conversions:**
- Require cast syntax `(targetType)value`
- May result in data loss or exceptions
- Used for narrowing conversions (larger to smaller types)
- Include conversions between unrelated types
- Programmer takes responsibility for safety

**Type Compatibility:**
Understanding when types are compatible for conversion is essential for writing robust applications:

- **Assignment Compatibility**: When one type can be assigned to another
- **Conversion Compatibility**: When explicit conversion is possible
- **Inheritance Compatibility**: Relationships based on class hierarchy
- **Interface Compatibility**: When types implement common interfaces

**Conversion Methods:**
C# provides several mechanisms for type conversion:
- **Parse Methods**: Convert strings to other types (`int.Parse()`)
- **TryParse Methods**: Safe conversion with success indication
- **Convert Class**: Provides conversions between base types
- **Custom Operators**: User-defined implicit and explicit operators
- **Type Casting**: Direct casting using parentheses syntax

### 6. Static vs Instance Members

Understanding the distinction between static and instance members is crucial for proper object-oriented design and memory management in C# applications.

**Instance Members:**
Instance members belong to specific instances of a class or struct. Each object has its own copy of instance fields and can invoke instance methods on its own data.

**Characteristics of Instance Members:**
- Associated with individual object instances
- Each object maintains its own copy of instance fields
- Can access both instance and static members of the class
- Require an object instance to be invoked
- Have access to the `this` keyword
- Can be virtual and participate in polymorphism

**Types of Instance Members:**
- **Instance Fields**: Store data specific to each object
- **Instance Properties**: Provide controlled access to instance data
- **Instance Methods**: Operate on instance data and can modify object state
- **Instance Events**: Allow objects to notify subscribers of state changes

**Static Members:**
Static members belong to the type itself rather than to any specific instance. They are shared across all instances of the type and exist independently of object creation.

**Characteristics of Static Members:**
- Associated with the type rather than instances
- Shared across all instances of the type
- Can only access other static members directly
- Do not require object instantiation to be accessed
- Cannot use the `this` keyword
- Cannot be virtual or overridden

**Types of Static Members:**
- **Static Fields**: Store data shared across all instances
- **Static Properties**: Provide access to static data
- **Static Methods**: Utility methods that don't require instance data
- **Static Constructors**: Initialize static data when type is first used
- **Static Events**: Type-level notifications

**Memory and Lifecycle Considerations:**
- **Instance Members**: Memory allocated per object, lifetime tied to object lifecycle
- **Static Members**: Memory allocated once per type, lifetime spans application execution
- **Initialization**: Static members initialized before first use, instance members initialized during object creation

**Design Guidelines:**
- Use static members for utility functions and shared data
- Use instance members for object-specific state and behavior
- Avoid excessive static state to maintain testability
- Consider thread safety for static members in multi-threaded applications

## Detailed Examples and Practical Applications

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

Boxing and unboxing are fundamental concepts in C# that describe the conversion between value types and reference types, specifically when value types are treated as objects.

**Boxing:**
Boxing is the process of converting a value type to the `object` type or to any interface type implemented by the value type. When boxing occurs, the value type data is wrapped in an object and stored on the managed heap.

**Boxing Process:**
1. Memory is allocated on the heap for the boxed value
2. The value type data is copied to the heap location
3. A reference to the heap location is returned
4. The original value type remains unchanged on the stack

**Unboxing:**
Unboxing is the explicit conversion from an object type back to a value type. This process extracts the value type data from the object wrapper and copies it back to the stack.

**Unboxing Process:**
1. Verify that the object contains a value of the target type
2. Copy the value from the heap object to the stack
3. Return the extracted value type

**Performance Implications:**
Boxing and unboxing operations have significant performance costs:
- **Memory Allocation**: Boxing allocates memory on the heap
- **Garbage Collection**: Boxed values become eligible for garbage collection
- **Type Checking**: Unboxing requires runtime type verification
- **Data Copying**: Both operations involve copying data between stack and heap

**Common Scenarios Where Boxing Occurs:**
- Adding value types to non-generic collections (`ArrayList`)
- Calling `Object` methods on value types (`ToString()`, `GetHashCode()`)
- Passing value types to methods expecting `object` parameters
- Using value types with interfaces they implement

**Avoiding Boxing:**
- Use generic collections instead of non-generic collections
- Use generic methods when possible
- Be aware of implicit boxing in method calls
- Consider the performance impact in high-frequency operations

## Best Practices and Guidelines

### Type Selection Criteria

**Choosing Between Value and Reference Types:**
The decision between value types and reference types should be based on the intended use, performance requirements, and semantic meaning of the data.

**Use Value Types (Structs) When:**
- Data represents a single value or small collection of related values
- Immutability is desired or required
- The type size is small (generally less than 16 bytes)
- Value semantics are more appropriate than reference semantics
- No inheritance hierarchy is needed
- Performance is critical and you want to avoid heap allocation

**Use Reference Types (Classes) When:**
- The type represents a complex entity with identity
- Inheritance relationships are needed
- The object may be large or contain many fields
- Reference semantics are required
- Polymorphic behavior is needed
- The object maintains mutable state over time

**Performance Optimization Guidelines:**

**Memory Efficiency:**
- Consider the total memory footprint of your types
- Be aware of padding and alignment in struct layouts
- Use readonly structs for immutable value types
- Minimize boxing operations in performance-critical code

**Allocation Patterns:**
- Prefer stack allocation for short-lived data
- Use object pooling for frequently created objects
- Consider using `stackalloc` for small temporary arrays
- Be mindful of large object heap allocations (>85KB)

**Type Safety Best Practices:**

**Null Safety:**
- Use nullable reference types to express null intent clearly
- Validate parameters and handle null cases appropriately
- Consider using null-conditional operators (`?.`, `??`)
- Implement proper null checking in public APIs

**Conversion Safety:**
- Prefer TryParse methods over Parse methods for user input
- Use the `is` operator for type checking before casting
- Implement custom conversion operators judiciously
- Document conversion behavior and potential exceptions

**Design Principles:**

**Encapsulation:**
- Keep internal implementation details private
- Use properties instead of public fields
- Provide meaningful constructors for initialization
- Consider immutability for thread safety

**Interface Design:**
- Design types to be easy to use correctly
- Follow the principle of least surprise
- Provide clear and consistent naming
- Consider the long-term evolution of your types

### Advanced Type Concepts

**Generic Type Constraints:**
Understanding how to constrain generic types ensures type safety while maintaining flexibility.

```csharp
// Constraint examples
public class Repository<T> where T : class, IEntity, new()
{
    // T must be a reference type, implement IEntity, and have parameterless constructor
}

public void ProcessValue<T>(T value) where T : struct
{
    // T must be a value type
}
```

**Nullable Value Types:**
Nullable value types allow value types to represent the absence of a value.

```csharp
int? nullableInt = null;        // Can hold int values or null
DateTime? optionalDate = null;   // Useful for optional dates

// Safe navigation
if (nullableInt.HasValue)
{
    int actualValue = nullableInt.Value;
}

// Null-coalescing operator
int result = nullableInt ?? 0;  // Use 0 if null
```

**Type Inference and Anonymous Types:**
The compiler can infer types in many contexts, reducing verbosity while maintaining type safety.

```csharp
// Type inference with var
var customer = new Customer("John", "Doe");  // Type inferred as Customer
var numbers = new List<int>();               // Type inferred as List<int>

// Anonymous types for temporary data structures
var productInfo = new 
{ 
    Name = "Laptop", 
    Price = 999.99m, 
    InStock = true 
};
```

## Running the Project

To execute this demonstration project and observe the type system concepts in action, use the following command in your terminal or command prompt:

```bash
dotnet run
```

This command will compile and run the project, demonstrating all the type system concepts covered in this documentation. The program will show practical examples of value types, reference types, type conversions, boxing and unboxing, and the behavior differences between static and instance members.

## Learning Outcomes

After studying and running this project, you should be able to:

**Fundamental Understanding:**
- Explain the difference between value types and reference types
- Understand how memory allocation differs between stack and heap
- Identify when boxing and unboxing occur and their performance implications
- Distinguish between static and instance members and their appropriate uses

**Practical Application:**
- Choose appropriate built-in types for different data requirements
- Design custom types using classes and structs effectively
- Implement type conversions safely and efficiently
- Apply best practices for type design and usage

**Advanced Concepts:**
- Understand the C# type hierarchy and inheritance relationships
- Recognize type safety benefits and limitations
- Optimize code for better memory usage and performance
- Design APIs that are both flexible and type-safe

**Professional Development:**
- Write code that follows industry best practices for type usage
- Create maintainable and efficient applications using appropriate type choices
- Debug type-related issues and understand compiler error messages
- Evaluate trade-offs between different type design approaches

## Code Examples and Demonstrations

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

