# Operator Overloading ⚡

## 🎯 Learning Objectives

By mastering this project, you will:
- **Understand Operator Overloading**: Learn how to define custom behavior for operators
- **Master Arithmetic Operators**: Implement +, -, *, / for custom types with mathematical meaning
- **Handle Equality and Comparison**: Override ==, !=, <, >, <=, >= for consistent comparisons
- **Implement Conversion Operators**: Create implicit and explicit type conversions
- **Use Unary Operators**: Define behavior for +, -, !, ++, -- with custom types
- **Follow Best Practices**: Ensure operator consistency, null safety, and logical behavior
- **Apply Real-World Patterns**: Build mathematical types, domain models, and fluent APIs

## 📚 Core Concepts

### What is Operator Overloading?
Operator overloading allows you to define custom behavior for operators when used with your custom types. Instead of calling methods, users can use natural mathematical notation.

```csharp
// Without operator overloading
Vector sum = vector1.Add(vector2);

// With operator overloading  
Vector sum = vector1 + vector2;  // Much more natural!
```

### Overloadable vs Non-Overloadable Operators
**Overloadable**: `+`, `-`, `*`, `/`, `%`, `==`, `!=`, `<`, `>`, `<=`, `>=`, `!`, `~`, `++`, `--`, `true`, `false`

**Non-Overloadable**: `=`, `&&`, `||`, `??`, `?.`, `=>`, `is`, `as`, `new`, `typeof`, `sizeof`

### Operator Syntax
Operators are static methods with the `operator` keyword:

```csharp
public struct Vector2D
{
    public double X { get; set; }
    public double Y { get; set; }
    
    // Binary operator overload
    public static Vector2D operator +(Vector2D left, Vector2D right)
    {
        return new Vector2D(left.X + right.X, left.Y + right.Y);
    }
    
    // Unary operator overload
    public static Vector2D operator -(Vector2D vector)
    {
        return new Vector2D(-vector.X, -vector.Y);
    }
}
```

## 🚀 Key Features

### 1. **Arithmetic Operators**
```csharp
public struct Money
{
    private decimal amount;
    private string currency;
    
    public Money(decimal amount, string currency)
    {
        this.amount = amount;
        this.currency = currency;
    }
    
    // Addition
    public static Money operator +(Money left, Money right)
    {
        if (left.currency != right.currency)
            throw new InvalidOperationException("Cannot add different currencies");
        
        return new Money(left.amount + right.amount, left.currency);
    }
    
    // Subtraction
    public static Money operator -(Money left, Money right)
    {
        if (left.currency != right.currency)
            throw new InvalidOperationException("Cannot subtract different currencies");
        
        return new Money(left.amount - right.amount, left.currency);
    }
    
    // Multiplication by scalar
    public static Money operator *(Money money, decimal factor) =>
        new Money(money.amount * factor, money.currency);
    
    // Division by scalar
    public static Money operator /(Money money, decimal divisor) =>
        new Money(money.amount / divisor, money.currency);
}

// Usage
var price1 = new Money(100.50m, "USD");
var price2 = new Money(50.25m, "USD");
var total = price1 + price2;          // $150.75 USD
var discounted = total * 0.9m;        // 10% discount
```

### 2. **Equality and Comparison Operators**
```csharp
public struct Note : IComparable<Note>, IEquatable<Note>
{
    private int semitonesFromA;
    
    public Note(int semitones) => semitonesFromA = semitones;
    
    // Equality operators (must implement both)
    public static bool operator ==(Note left, Note right) =>
        left.semitonesFromA == right.semitonesFromA;
    
    public static bool operator !=(Note left, Note right) =>
        !(left == right);
    
    // Comparison operators (implement all or none)
    public static bool operator <(Note left, Note right) =>
        left.semitonesFromA < right.semitonesFromA;
    
    public static bool operator >(Note left, Note right) =>
        left.semitonesFromA > right.semitonesFromA;
    
    public static bool operator <=(Note left, Note right) =>
        left.semitonesFromA <= right.semitonesFromA;
    
    public static bool operator >=(Note left, Note right) =>
        left.semitonesFromA >= right.semitonesFromA;
    
    // Override Equals and GetHashCode for consistency
    public override bool Equals(object obj) =>
        obj is Note other && this == other;
    
    public bool Equals(Note other) => this == other;
    
    public override int GetHashCode() => semitonesFromA.GetHashCode();
    
    public int CompareTo(Note other) => semitonesFromA.CompareTo(other.semitonesFromA);
}
```

### 3. **Conversion Operators**
```csharp
public struct Temperature
{
    private double celsius;
    
    public Temperature(double celsius) => this.celsius = celsius;
    
    // Implicit conversion from double (safe, no data loss)
    public static implicit operator Temperature(double celsius) =>
        new Temperature(celsius);
    
    // Implicit conversion to double (safe, common operation)
    public static implicit operator double(Temperature temp) =>
        temp.celsius;
    
    // Explicit conversion to Fahrenheit (explicit because it's less common)
    public static explicit operator double(Temperature temp) =>
        temp.celsius * 9.0 / 5.0 + 32.0;
}

// Usage
Temperature temp = 25.0;           // Implicit from double
double celsius = temp;             // Implicit to double
double fahrenheit = (double)temp;  // Explicit to Fahrenheit
```

### 4. **Unary Operators**
```csharp
public struct Complex
{
    public double Real { get; set; }
    public double Imaginary { get; set; }
    
    public Complex(double real, double imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }
    
    // Unary plus (identity)
    public static Complex operator +(Complex c) => c;
    
    // Unary minus (negation)
    public static Complex operator -(Complex c) =>
        new Complex(-c.Real, -c.Imaginary);
    
    // Logical NOT (conjugate for complex numbers)
    public static Complex operator !(Complex c) =>
        new Complex(c.Real, -c.Imaginary);
    
    // Increment (add 1 to real part)
    public static Complex operator ++(Complex c) =>
        new Complex(c.Real + 1, c.Imaginary);
    
    // Decrement (subtract 1 from real part)
    public static Complex operator --(Complex c) =>
        new Complex(c.Real - 1, c.Imaginary);
}
```

### 5. **Compound Assignment (Automatic)**
```csharp
// When you define basic operators, compound assignments work automatically
var vector1 = new Vector2D(1, 2);
var vector2 = new Vector2D(3, 4);

vector1 += vector2;  // Automatically uses operator +
vector1 -= vector2;  // Automatically uses operator -
vector1 *= 2.0;      // Automatically uses operator *
```

### 6. **True/False Operators**
```csharp
public struct Probability
{
    private double value;
    
    public Probability(double value)
    {
        if (value < 0 || value > 1)
            throw new ArgumentException("Probability must be between 0 and 1");
        this.value = value;
    }
    
    // True operator (for if statements)
    public static bool operator true(Probability p) => p.value > 0.5;
    
    // False operator (must implement both)
    public static bool operator false(Probability p) => p.value <= 0.5;
    
    // Logical AND
    public static Probability operator &(Probability left, Probability right) =>
        new Probability(left.value * right.value);
    
    // Logical OR
    public static Probability operator |(Probability left, Probability right) =>
        new Probability(left.value + right.value - (left.value * right.value));
}

// Usage
var prob = new Probability(0.7);
if (prob)  // Uses operator true
{
    Console.WriteLine("High probability event");
}
```

## 💡 Trainer Tips

### Consistency Rules
- **Implement pairs**: If you override `==`, also override `!=`
- **Comparison set**: Implement all comparison operators (`<`, `>`, `<=`, `>=`) or none
- **Equals/GetHashCode**: Always override `Equals()` and `GetHashCode()` with equality operators
- **Symmetric operations**: `a + b` should equal `b + a` when mathematically appropriate

### Safety Considerations
- **Null checks**: Handle null arguments appropriately
- **Overflow protection**: Check for arithmetic overflow in calculations
- **Type validation**: Ensure operands are compatible types
- **Exception handling**: Throw meaningful exceptions for invalid operations

### Performance Optimization
- **Struct types**: Use structs for mathematical types to avoid heap allocation
- **In parameters**: Use `in` modifier for large structs to avoid copying
- **Expression-bodied members**: Use for simple operators to improve readability

## 🎓 Real-World Applications

### Financial Calculations
```csharp
public struct Percentage
{
    private decimal value;
    
    public Percentage(decimal value) => this.value = value;
    
    // Apply percentage to money
    public static Money operator *(Money money, Percentage percent) =>
        money * (percent.value / 100m);
    
    // Add percentages
    public static Percentage operator +(Percentage left, Percentage right) =>
        new Percentage(left.value + right.value);
}

// Usage
var price = new Money(100m, "USD");
var tax = new Percentage(8.5m);
var total = price + (price * tax);  // $108.50
```

### 3D Graphics and Game Development
```csharp
public struct Vector3
{
    public float X, Y, Z;
    
    // Vector operations
    public static Vector3 operator +(Vector3 a, Vector3 b) =>
        new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    
    public static Vector3 operator -(Vector3 a, Vector3 b) =>
        new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    
    // Scalar multiplication
    public static Vector3 operator *(Vector3 v, float scalar) =>
        new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
    
    // Dot product
    public static float operator *(Vector3 a, Vector3 b) =>
        a.X * b.X + a.Y * b.Y + a.Z * b.Z;
}
```

### Time and Duration Calculations
```csharp
public struct Duration
{
    private TimeSpan timeSpan;
    
    public Duration(TimeSpan timeSpan) => this.timeSpan = timeSpan;
    
    // Add durations
    public static Duration operator +(Duration left, Duration right) =>
        new Duration(left.timeSpan + right.timeSpan);
    
    // Add duration to DateTime
    public static DateTime operator +(DateTime dateTime, Duration duration) =>
        dateTime + duration.timeSpan;
    
    // Multiply duration by factor
    public static Duration operator *(Duration duration, double factor) =>
        new Duration(TimeSpan.FromTicks((long)(duration.timeSpan.Ticks * factor)));
}
```

## 🎯 Mastery Checklist

### Beginner Level
- [ ] Implement basic arithmetic operators (+, -, *, /)
- [ ] Override equality operators (==, !=)
- [ ] Understand operator precedence and associativity
- [ ] Handle simple type conversions
- [ ] Write consistent Equals() and GetHashCode()

### Intermediate Level
- [ ] Implement all comparison operators consistently
- [ ] Create implicit and explicit conversion operators
- [ ] Use unary operators for domain-specific operations
- [ ] Handle null safety and edge cases
- [ ] Implement IComparable and IEquatable interfaces

### Advanced Level
- [ ] Design complex mathematical types with full operator support
- [ ] Implement true/false operators for conditional logic
- [ ] Create fluent APIs using operator overloading
- [ ] Optimize performance with struct types and in parameters
- [ ] Build domain-specific languages using operators

## 💼 Industry Impact

Operator overloading is crucial for:

**Game Development**: Vector math, matrix operations, physics calculations
**Financial Software**: Currency operations, percentage calculations, risk models
**Scientific Computing**: Mathematical expressions, unit conversions, statistical operations
**Graphics Programming**: 3D transformations, color blending, geometric calculations
**Domain-Specific Languages**: Creating intuitive APIs that feel like natural expressions

## 🔗 Integration with Modern C#

**Expression-Bodied Members (C# 6+)**:
```csharp
public static Vector2D operator +(Vector2D left, Vector2D right) =>
    new Vector2D(left.X + right.X, left.Y + right.Y);
```

**Pattern Matching with Operators (C# 7+)**:
```csharp
public static string Describe(object obj) => obj switch
{
    Vector2D v when v == Vector2D.Zero => "Zero vector",
    Vector2D v when v.Magnitude > 1 => "Unit vector",
    _ => "Unknown"
};
```

**Records with Operator Overloading (C# 9+)**:
```csharp
public record Point(double X, double Y)
{
    public static Point operator +(Point left, Point right) =>
        new Point(left.X + right.X, left.Y + right.Y);
}
```

---

*Operator overloading makes your types feel like first-class citizens in C#. Use it to create intuitive, mathematical, and domain-specific APIs that developers love to use!* ⚡✨
