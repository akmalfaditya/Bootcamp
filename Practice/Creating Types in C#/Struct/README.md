# C# Structs - Value Types and Performance

## 🎯 Learning Objectives

Master C# structs to create efficient value types that provide better performance for small data structures while understanding their unique behavior and constraints.

**What you'll master:**
- Understanding value type vs reference type semantics
- Creating efficient structs for small, immutable data
- Using readonly structs and readonly methods for immutability
- Working with ref structs for stack-only scenarios
- Understanding struct constructor and initialization rules
- Optimizing performance with struct design patterns
- Knowing when to use structs vs classes

## 📚 Core Concepts Covered

### 📦 Value Type Fundamentals
- **Value Semantics**: Understanding copy behavior vs reference behavior
- **Stack Allocation**: How structs are stored and managed in memory
- **Immutability Patterns**: Designing structs for thread safety and predictability
- **Default Initialization**: How struct fields are automatically initialized
- **Assignment Behavior**: Copy semantics and their implications

### 🏗️ Struct Design Principles
- **Size Considerations**: Keeping structs small for optimal performance
- **Immutability**: Designing immutable structs for safer code
- **Interface Implementation**: How structs can implement interfaces
- **No Inheritance**: Understanding struct limitations and alternatives
- **Constructor Rules**: Struct-specific constructor requirements

### ⚡ Performance Optimizations
- **Memory Layout**: Understanding struct memory footprint
- **Readonly Structs**: Preventing defensive copying for better performance
- **Readonly Methods**: Enabling readonly struct method calls
- **Ref Structs**: Stack-only types for zero-allocation scenarios
- **Boxing Avoidance**: Keeping structs on the stack

## 🚀 Key Features with Examples

### Value Type vs Reference Type Behavior
```csharp
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

// Value semantics - copying behavior
Point point1 = new Point(10, 20);
Point point2 = point1;  // Creates a complete copy
point2.X = 99;

Console.WriteLine($"point1.X = {point1.X}"); // Still 10!
Console.WriteLine($"point2.X = {point2.X}"); // Now 99

// Each variable contains its own copy of the data
```

### Readonly Structs for Immutability
```csharp
public readonly struct ImmutablePoint
{
    public readonly int X;
    public readonly int Y;
    
    public ImmutablePoint(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    // All methods are implicitly readonly
    public double DistanceFromOrigin()
    {
        return Math.Sqrt(X * X + Y * Y);
    }
    
    // Methods can return new instances for "mutations"
    public ImmutablePoint Move(int deltaX, int deltaY)
    {
        return new ImmutablePoint(X + deltaX, Y + deltaY);
    }
}

// Usage - immutable and thread-safe
ImmutablePoint point = new ImmutablePoint(3, 4);
ImmutablePoint movedPoint = point.Move(1, 1); // Returns new instance
Console.WriteLine($"Original: ({point.X}, {point.Y})");
Console.WriteLine($"Moved: ({movedPoint.X}, {movedPoint.Y})");
```

### Readonly Methods for Mixed Mutability
```csharp
public struct Rectangle
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }
    
    // Readonly method - can be called on readonly references
    public readonly int CalculateArea()
    {
        return Width * Height; // No defensive copying needed
    }
    
    // Readonly method with complex calculation
    public readonly bool IsSquare()
    {
        return Width == Height;
    }
    
    // Mutating method - modifies the struct
    public void Scale(double factor)
    {
        Width = (int)(Width * factor);
        Height = (int)(Height * factor);
    }
}

// Readonly reference can call readonly methods efficiently
static void ProcessRectangle(in Rectangle rect)
{
    int area = rect.CalculateArea(); // No copying!
    bool isSquare = rect.IsSquare(); // No copying!
    // rect.Scale(2.0); // Compile error - can't call mutating method
}
```

### Ref Structs for Zero-Allocation Scenarios
```csharp
public ref struct StackOnlySpan<T>
{
    private readonly Span<T> _span;
    
    public StackOnlySpan(Span<T> span)
    {
        _span = span;
    }
    
    public int Length => _span.Length;
    
    public ref T this[int index] => ref _span[index];
    
    // Can return refs to elements without allocation
    public ref T GetReference(int index)
    {
        return ref _span[index];
    }
}

// Ref structs can only live on the stack
static void ProcessData()
{
    Span<int> data = stackalloc int[100]; // Stack allocation
    var span = new StackOnlySpan<int>(data);
    
    // Process data without any heap allocations
    for (int i = 0; i < span.Length; i++)
    {
        span[i] = i * 2;
    }
}
```

### Interface Implementation
```csharp
public interface IMovable
{
    void Move(int deltaX, int deltaY);
}

public struct MovablePoint : IMovable
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public MovablePoint(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public void Move(int deltaX, int deltaY)
    {
        X += deltaX;
        Y += deltaY;
    }
    
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

// Interface usage (be careful of boxing!)
IMovable movable = new MovablePoint(1, 2); // Boxing occurs here!
movable.Move(3, 4); // Works, but on a boxed copy
```

## 💡 Trainer Tips

### When to Use Structs
- **Small Data**: Use for small, simple data structures (typically ≤ 16 bytes)
- **Value Semantics**: When you want copying behavior rather than reference sharing
- **Immutable Data**: Excellent for immutable value types like coordinates, colors, dates
- **Performance Critical**: When you need to avoid heap allocation and garbage collection
- **Frequently Copied**: When the data is copied frequently and you want value semantics

### When NOT to Use Structs
- **Large Data**: Avoid for data structures larger than ~16 bytes (copying cost)
- **Complex Behavior**: When you need inheritance, virtual methods, or complex polymorphism
- **Mutable Shared State**: When you need multiple variables to reference the same object
- **Null References**: When you need null semantics (use nullable structs instead)

### Performance Best Practices
- **Keep Small**: Large structs are expensive to copy - consider classes instead
- **Readonly When Possible**: Use readonly structs to prevent defensive copying
- **Avoid Boxing**: Don't cast structs to object or interfaces in hot paths
- **Pass by Reference**: Use `in` parameters for large structs to avoid copying
- **Immutable Design**: Immutable structs are safer and often more performant

### Common Pitfalls
- **Defensive Copying**: Calling methods on readonly references can cause copying
- **Boxing Performance**: Interface usage causes boxing and heap allocation
- **Assignment Semantics**: Remember that assignment always copies the entire struct
- **Field Initialization**: All fields must be initialized in custom constructors

## 🎓 Real-World Applications

### 🎮 Game Development - Vector Mathematics
```csharp
public readonly struct Vector3
{
    public readonly float X, Y, Z;
    
    public Vector3(float x, float y, float z)
    {
        X = x; Y = y; Z = z;
    }
    
    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    
    public static Vector3 operator *(Vector3 v, float scalar)
    {
        return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
    }
    
    public readonly float Magnitude()
    {
        return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
    }
    
    public readonly Vector3 Normalized()
    {
        float mag = Magnitude();
        return mag > 0 ? this * (1f / mag) : this;
    }
}

// High-performance vector operations without heap allocation
Vector3 position = new Vector3(1, 2, 3);
Vector3 velocity = new Vector3(0.1f, 0, 0);
Vector3 newPosition = position + velocity * Time.deltaTime;
```

### 💰 Financial Calculations - Money Type
```csharp
public readonly struct Money
{
    private readonly decimal _amount;
    private readonly string _currency;
    
    public Money(decimal amount, string currency)
    {
        _amount = amount;
        _currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }
    
    public decimal Amount => _amount;
    public string Currency => _currency;
    
    public static Money operator +(Money a, Money b)
    {
        if (a._currency != b._currency)
            throw new InvalidOperationException("Cannot add different currencies");
        
        return new Money(a._amount + b._amount, a._currency);
    }
    
    public override string ToString()
    {
        return $"{_amount:C} {_currency}";
    }
    
    public override bool Equals(object obj)
    {
        return obj is Money other && 
               _amount == other._amount && 
               _currency == other._currency;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(_amount, _currency);
    }
}

// Immutable money calculations
Money price = new Money(29.99m, "USD");
Money tax = new Money(2.40m, "USD");
Money total = price + tax; // New instance, originals unchanged
```

### 🔢 High-Performance Computing - Complex Numbers
```csharp
public readonly struct Complex
{
    public readonly double Real;
    public readonly double Imaginary;
    
    public Complex(double real, double imaginary = 0)
    {
        Real = real;
        Imaginary = imaginary;
    }
    
    public readonly double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);
    public readonly double Phase => Math.Atan2(Imaginary, Real);
    
    public static Complex operator +(Complex a, Complex b)
    {
        return new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);
    }
    
    public static Complex operator *(Complex a, Complex b)
    {
        return new Complex(
            a.Real * b.Real - a.Imaginary * b.Imaginary,
            a.Real * b.Imaginary + a.Imaginary * b.Real
        );
    }
    
    public readonly Complex Conjugate()
    {
        return new Complex(Real, -Imaginary);
    }
}

// Mathematical computations without heap allocation
Complex z1 = new Complex(3, 4);
Complex z2 = new Complex(1, -2);
Complex result = z1 * z2.Conjugate();
```

## 🎯 Mastery Checklist

### Beginner Level
- [ ] Understand value type vs reference type semantics
- [ ] Create basic structs with properties and methods
- [ ] Implement struct constructors correctly
- [ ] Use structs for simple data containers
- [ ] Understand struct assignment and copying behavior

### Intermediate Level
- [ ] Design readonly structs for immutability
- [ ] Use readonly methods to prevent defensive copying
- [ ] Implement interfaces on structs appropriately
- [ ] Understand when to use `in` parameters with structs
- [ ] Override Equals, GetHashCode, and ToString properly

### Advanced Level
- [ ] Create ref structs for zero-allocation scenarios
- [ ] Design high-performance struct hierarchies
- [ ] Implement operator overloading for mathematical structs
- [ ] Understand boxing implications and avoidance strategies
- [ ] Use structs in generic constraints and collections

### Expert Level
- [ ] Design domain-specific value types for complex systems
- [ ] Optimize struct layouts for performance-critical applications
- [ ] Create custom span-like types using ref structs
- [ ] Build lock-free algorithms using struct atomicity
- [ ] Design struct-based APIs for high-performance libraries

## 💼 Industry Impact

### Performance Benefits
- **Memory Efficiency**: Stack allocation reduces garbage collection pressure
- **Cache Locality**: Better CPU cache performance for small, frequently accessed data
- **Copy Semantics**: Predictable behavior in multi-threaded scenarios
- **Zero Allocation**: Ref structs enable zero-allocation high-performance scenarios

### Critical Applications
- **Game Engines**: Vector math, physics calculations, rendering data
- **Financial Systems**: Money types, decimal calculations, trading algorithms
- **Scientific Computing**: Mathematical operations, statistical calculations
- **Embedded Systems**: Resource-constrained environments requiring efficiency

## 🔗 Integration with Modern Technologies

### .NET Performance Features
- **Span<T> and Memory<T>**: Modern zero-allocation buffer management
- **System.Numerics**: High-performance mathematical structs
- **Unsafe Code**: Advanced struct manipulation with pointers
- **SIMD**: Vector types for parallel mathematical operations

### Language Integration
- **Pattern Matching**: Structs work with switch expressions and patterns
- **Nullable Value Types**: T? syntax for optional struct values
- **Tuple Types**: Built-in struct types for multiple return values
- **Record Structs**: Modern immutable struct syntax (C# 10+)

---

**🎖️ Professional Insight**: Structs are essential for high-performance .NET development. They're not just "lightweight classes" - they're fundamentally different with value semantics. Master structs to:

- **Build Efficient APIs**: Create value types that don't burden the garbage collector
- **Design Domain Models**: Model immutable value objects in domain-driven design
- **Optimize Performance**: Use for data that's copied frequently or needs stack allocation
- **Understand .NET**: Grasp how the runtime handles value vs reference types

The key is knowing when struct semantics serve your design better than class semantics!
