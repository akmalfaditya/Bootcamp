# Operator Overloading

## Learning Objectives

By learning this project, you will:
- **Understand Operator Overloading**: Learn how to define custom behavior for operators
- **Master Arithmetic Operators**: Implement +, -, *, / for custom types with mathematical meaning
- **Handle Equality and Comparison**: Override ==, !=, <, >, <=, >= for consistent comparisons
- **Implement Conversion Operators**: Create implicit and explicit type conversions
- **Use Unary Operators**: Define behavior for +, -, !, ++, -- with custom types
- **Work with Checked Operators**: Implement overflow-safe arithmetic (C# 11+)
- **Create Three-Valued Logic**: Build types with true/false operators for conditional logic
- **Follow Best Practices**: Ensure operator consistency, null safety, and logical behavior
- **Apply Real-World Patterns**: Build mathematical types, domain models, and fluent APIs

## Core Concepts

### What is Operator Overloading?

Operator overloading is a feature in C# that allows you to define custom behavior for operators when they are applied to user-defined types. This capability enables your custom classes and structs to integrate seamlessly with C#'s built-in operator syntax, making your types feel like first-class citizens in the language.

The primary benefit of operator overloading is enhanced readability and intuitive usage. Instead of requiring users to call specific methods, operator overloading allows for natural mathematical and logical expressions.

```csharp
// Without operator overloading - verbose and unnatural
Vector sum = vector1.Add(vector2);
bool isEqual = vector1.Equals(vector2);
Vector scaled = vector1.MultiplyByScalar(2.5);

// With operator overloading - natural and intuitive
Vector sum = vector1 + vector2;
bool isEqual = vector1 == vector2;
Vector scaled = vector1 * 2.5;
```

**When to Use Operator Overloading:**
- Types that represent mathematical concepts (vectors, matrices, complex numbers)
- Types that have natural arithmetic relationships (money, measurements, coordinates)
- Types where operators enhance readability without obscuring meaning
- Value types that behave like primitive types

**When NOT to Use Operator Overloading:**
- Operations that are not intuitive or universally understood
- Complex business logic that would be clearer as named methods
- Operations that might have unexpected side effects
- Reference types where identity vs equality semantics could be confusing

### Overloadable vs Non-Overloadable Operators

Understanding which operators can be overloaded is crucial for effective operator overloading implementation.

**Overloadable Operators:**
- **Unary**: `+`, `-`, `!`, `~`, `++`, `--`, `true`, `false`
- **Binary Arithmetic**: `+`, `-`, `*`, `/`, `%`
- **Binary Bitwise**: `&`, `|`, `^`, `<<`, `>>`
- **Comparison**: `==`, `!=`, `<`, `>`, `<=`, `>=`

**Non-Overloadable Operators:**
- **Assignment**: `=` (assignment is handled by the type system)
- **Logical**: `&&`, `||` (these use short-circuit evaluation based on `true`/`false` operators)
- **Null-related**: `??`, `?.` (these require special compiler support)
- **Type operations**: `is`, `as`, `typeof`, `sizeof`
- **Lambda**: `=>` (this is syntax, not an operator)

**Important Note:** Some operators are automatically derived. For example, when you overload `+`, the compound assignment operator `+=` becomes available automatically. Similarly, `&&` and `||` can work with your types if you implement the bitwise operators (`&`, `|`) and the `true`/`false` operators.

### Operator Function Syntax

Operator functions follow a specific syntax pattern that distinguishes them from regular methods:

```csharp
public struct Vector2D
{
    public double X { get; set; }
    public double Y { get; set; }
    
    // Binary operator: requires two parameters
    public static Vector2D operator +(Vector2D left, Vector2D right)
    {
        return new Vector2D
        {
            X = left.X + right.X,
            Y = left.Y + right.Y
        };
    }
    
    // Unary operator: requires one parameter
    public static Vector2D operator -(Vector2D vector)
    {
        return new Vector2D
        {
            X = -vector.X,
            Y = -vector.Y
        };
    }
    
    // Conversion operator: special syntax
    public static implicit operator string(Vector2D vector)
    {
        return $"({vector.X}, {vector.Y})";
    }
}
```

**Key Requirements:**
1. **Must be static**: Operator functions cannot be instance methods
2. **Must be public**: Private operators cannot be called by external code
3. **Parameter rules**: At least one parameter must be of the containing type
4. **Return type**: Can be any type, but should be logical for the operation
5. **Naming**: Use the `operator` keyword followed by the operator symbol

## Key Features

### 1. **Arithmetic Operators**

Arithmetic operators are the most commonly overloaded operators because they provide natural mathematical operations for custom types. These operators should behave consistently with mathematical expectations and maintain properties like commutativity where appropriate.

**Design Principles:**
- Operations should be mathematically meaningful for your type
- Results should be predictable and consistent with user expectations
- Consider whether operations should be commutative (a + b = b + a)
- Handle edge cases like division by zero or overflow conditions
- Maintain mathematical properties like associativity and distributivity where applicable

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
    
    // Addition: Combines monetary values of the same currency
    // Validates currency compatibility to prevent logical errors
    public static Money operator +(Money left, Money right)
    {
        if (left.currency != right.currency)
            throw new InvalidOperationException("Cannot add different currencies");
        
        return new Money(left.amount + right.amount, left.currency);
    }
    
    // Subtraction: Calculates difference between monetary values
    // Also ensures currency compatibility for meaningful results
    public static Money operator -(Money left, Money right)
    {
        if (left.currency != right.currency)
            throw new InvalidOperationException("Cannot subtract different currencies");
        
        return new Money(left.amount - right.amount, left.currency);
    }
    
    // Scalar multiplication: Scales monetary value by a factor
    // Useful for calculations like taxes, discounts, or projections
    public static Money operator *(Money money, decimal factor) =>
        new Money(money.amount * factor, money.currency);
    
    // Scalar division: Splits monetary value proportionally
    // Guards against division by zero to prevent runtime errors
    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Cannot divide money by zero");
        return new Money(money.amount / divisor, money.currency);
    }
}

// Practical usage demonstrating natural mathematical syntax
var price1 = new Money(100.50m, "USD");
var price2 = new Money(50.25m, "USD");
var total = price1 + price2;          // $150.75 USD
var discounted = total * 0.9m;        // 10% discount = $135.68 USD
var split = total / 3;                // Split among 3 people = $50.25 USD each
```

**Common Patterns:**
- Always validate preconditions (same currency, non-zero divisors)
- Return the same type when the operation preserves the type's meaning
- Consider both directions for non-commutative operations (Money * decimal vs decimal * Money)
- Use expression-bodied syntax for simple operations to improve readability

### 2. **Equality and Comparison Operators**

Equality and comparison operators are critical for enabling your types to work seamlessly with collections, sorting algorithms, and conditional logic. The C# compiler enforces specific rules to ensure consistency and prevent logical errors.

**Mandatory Pairing Rules:**
- If you override `==`, you must also override `!=`
- If you override `<`, you must also override `>`
- If you override `<=`, you must also override `>=`
- You must override `Equals(object)` and `GetHashCode()` when implementing equality operators

**Consistency Requirements:**
The framework expects that equality operations are consistent across different access methods. This means `obj1 == obj2`, `obj1.Equals(obj2)`, and hash-based collections should all produce consistent results.

```csharp
public struct Note : IComparable<Note>, IEquatable<Note>
{
    private readonly int semitonesFromA;
    
    public Note(int semitones) => semitonesFromA = semitones;
    
    // Equality operators: Must implement both == and != as a pair
    // The compiler enforces this to prevent logical inconsistencies
    public static bool operator ==(Note left, Note right) =>
        left.semitonesFromA == right.semitonesFromA;
    
    public static bool operator !=(Note left, Note right) =>
        !(left == right);  // Defined in terms of == for consistency
    
    // Comparison operators: Implement all four or none
    // This enables sorting, range checking, and ordered collections
    public static bool operator <(Note left, Note right) =>
        left.semitonesFromA < right.semitonesFromA;
    
    public static bool operator >(Note left, Note right) =>
        left.semitonesFromA > right.semitonesFromA;
    
    public static bool operator <=(Note left, Note right) =>
        left.semitonesFromA <= right.semitonesFromA;
    
    public static bool operator >=(Note left, Note right) =>
        left.semitonesFromA >= right.semitonesFromA;
    
    // Object.Equals override: Required for consistency with == operator
    // This ensures that boxing/unboxing scenarios work correctly
    public override bool Equals(object obj) =>
        obj is Note other && this == other;
    
    // IEquatable<T>.Equals: Provides type-safe equality without boxing
    // More efficient than Object.Equals for value types
    public bool Equals(Note other) => this == other;
    
    // GetHashCode override: Critical for hash-based collections
    // Objects that are equal must have the same hash code
    public override int GetHashCode() => semitonesFromA.GetHashCode();
    
    // IComparable<T>.CompareTo: Enables sorting and ordered operations
    // Should be consistent with comparison operators
    public int CompareTo(Note other) => semitonesFromA.CompareTo(other.semitonesFromA);
}
```

**Hash Code Considerations:**
The `GetHashCode()` method is crucial for performance in hash-based collections like `Dictionary<T,U>` and `HashSet<T>`. Two objects that are equal must always return the same hash code, though objects with the same hash code need not be equal.

**Sorting Integration:**
Implementing `IComparable<T>` allows your type to work with framework sorting methods like `Array.Sort()`, `List<T>.Sort()`, and LINQ's `OrderBy()`. The comparison logic should be consistent with your comparison operators.

### 3. **Conversion Operators**

Conversion operators enable seamless transformation between your custom types and other types. They provide a way to integrate your types naturally with existing code while maintaining type safety. The choice between implicit and explicit conversions communicates intent and prevents accidental data loss.

**Implicit vs Explicit Conversions:**
- **Implicit conversions**: Should only be used when the conversion is guaranteed to succeed and no information will be lost. The compiler performs these automatically without requiring a cast.
- **Explicit conversions**: Should be used when the conversion might fail, might lose precision, or when the conversion is not commonly needed. These require an explicit cast from the developer.

**Design Guidelines:**
- Implicit conversions should be safe, obvious, and commonly used
- Explicit conversions may lose data or require computational overhead
- Consider the principle of least surprise - conversions should behave as users expect
- Avoid conversion chains that could lead to ambiguous or unexpected behavior

```csharp
public struct Temperature
{
    private readonly double celsius;
    
    public Temperature(double celsius) => this.celsius = celsius;
    
    // Implicit conversion from double: Safe and commonly expected
    // Users can write: Temperature temp = 25.0;
    // This is safe because any double value represents a valid temperature
    public static implicit operator Temperature(double celsius) =>
        new Temperature(celsius);
    
    // Implicit conversion to double: Returns the underlying value
    // Safe because it preserves the exact value without loss
    // Enables direct use in mathematical expressions
    public static implicit operator double(Temperature temp) =>
        temp.celsius;
    
    // Explicit conversion to Fahrenheit: Requires cast to show intent
    // Explicit because this is a less common operation and involves calculation
    // Usage: double fahrenheit = (double)temp;  // Wrong! This gets Celsius
    //        double fahrenheit = temp.ToFahrenheit(); // Better as a method
    public static explicit operator double(Temperature temp) =>
        temp.celsius * 9.0 / 5.0 + 32.0;
        
    // Better approach for specific conversions: Use named methods
    public double ToFahrenheit() => celsius * 9.0 / 5.0 + 32.0;
    public double ToKelvin() => celsius + 273.15;
}

// Practical usage examples
Temperature temp = 25.0;              // Implicit from double - natural and safe
double celsius = temp;                // Implicit to double - preserves exact value
double fahrenheit = temp.ToFahrenheit(); // Named method - clear intent

// Mathematical operations work naturally due to implicit conversion
Temperature warm = temp + 5.0;        // Equivalent to: temp + new Temperature(5.0)
Temperature doubled = temp * 2;       // Natural scaling operation
```

**Common Pitfalls:**
- Avoid multiple implicit conversion paths that could create ambiguity
- Be careful with precision loss in floating-point conversions
- Consider whether a named method might be clearer than an explicit operator
- Test conversion roundtrip behavior (convert to another type and back)

**Integration with Framework:**
Conversion operators work seamlessly with generic constraints, LINQ operations, and framework methods. However, they are ignored by `is` and `as` operators, which only consider inheritance and interface implementation.

### 4. **Unary Operators**

Unary operators operate on a single operand and are particularly useful for mathematical types, state transformations, and logical operations. They should represent natural, single-step transformations that users would intuitively expect.

**Common Unary Operators:**
- `+` (unary plus): Often used for identity or normalization operations
- `-` (unary minus): Negation or inverse operations
- `!` (logical NOT): Logical negation or complementary operations
- `~` (bitwise NOT): Bitwise complement for integral types
- `++` and `--`: Increment and decrement operations

**Design Considerations:**
- Unary operations should be mathematically or logically meaningful
- The result should be the same type or a closely related type
- Operations should be reversible where mathematically appropriate
- Consider whether the operation modifies the original value or returns a new value

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
    
    // Unary plus: Identity operation, returns the same value
    // Could be used for normalization or validation in more complex scenarios
    public static Complex operator +(Complex c) => c;
    
    // Unary minus: Mathematical negation of both components
    // Creates the additive inverse: c + (-c) = 0
    public static Complex operator -(Complex c) =>
        new Complex(-c.Real, -c.Imaginary);
    
    // Logical NOT: Complex conjugate operation
    // Mathematically meaningful operation that flips the sign of imaginary part
    public static Complex operator !(Complex c) =>
        new Complex(c.Real, -c.Imaginary);
    
    // Increment: Adds 1 to the real part
    // Provides natural progression for complex numbers
    public static Complex operator ++(Complex c) =>
        new Complex(c.Real + 1, c.Imaginary);
    
    // Decrement: Subtracts 1 from the real part
    // Provides natural regression for complex numbers
    public static Complex operator --(Complex c) =>
        new Complex(c.Real - 1, c.Imaginary);
        
    // Additional useful unary-style operations
    public double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);
    public Complex Normalized => Magnitude == 0 ? this : new Complex(Real / Magnitude, Imaginary / Magnitude);
}

// Usage examples demonstrating natural mathematical operations
var complex = new Complex(3, 4);
var negated = -complex;              // (-3, -4) - additive inverse
var conjugate = !complex;            // (3, -4) - complex conjugate
var incremented = ++complex;         // (4, 4) - shifted right on real axis

// Increment/decrement work with both prefix and postfix notation
var original = new Complex(5, 2);
var postIncrement = original++;      // Returns (5, 2), original becomes (6, 2)
var preIncrement = ++original;       // original becomes (7, 2), returns (7, 2)
```

**Increment and Decrement Behavior:**
C# automatically handles the difference between prefix (`++x`) and postfix (`x++`) operations. Your operator function defines the increment operation, and the compiler generates the appropriate behavior for both forms.

**Mathematical Properties:**
Well-designed unary operators should preserve mathematical properties:
- Idempotency where appropriate: `+x` should equal `x`
- Reversibility: `-(-x)` should equal `x`
- Consistency with binary operations: `x + (-x)` should equal zero

### 5. **Compound Assignment Operators (Automatic)**

Compound assignment operators are one of the most convenient features of operator overloading because they are automatically generated by the compiler when you define the corresponding basic operators. This automatic behavior ensures consistency and reduces code duplication.

**Automatic Generation Rules:**
- `+=` is automatically available when you define `+`
- `-=` is automatically available when you define `-`
- `*=` is automatically available when you define `*`
- `/=` is automatically available when you define `/`
- `%=` is automatically available when you define `%`
- `&=`, `|=`, `^=`, `<<=`, `>>=` follow the same pattern for bitwise operators

**How It Works:**
When you write `a += b`, the compiler translates this to `a = a + b`, using your overloaded `+` operator. This means the compound assignment inherits all the behavior and validation logic from your basic operator implementation.

```csharp
// Example with Vector2D to demonstrate automatic compound assignment
public struct Vector2D
{
    public double X { get; set; }
    public double Y { get; set; }
    
    public Vector2D(double x, double y)
    {
        X = x;
        Y = y;
    }
    
    // Define basic operators - compound assignments come for free
    public static Vector2D operator +(Vector2D left, Vector2D right) =>
        new Vector2D(left.X + right.X, left.Y + right.Y);
    
    public static Vector2D operator -(Vector2D left, Vector2D right) =>
        new Vector2D(left.X - right.X, left.Y - right.Y);
    
    public static Vector2D operator *(Vector2D vector, double scalar) =>
        new Vector2D(vector.X * scalar, vector.Y * scalar);
    
    public static Vector2D operator /(Vector2D vector, double divisor) =>
        new Vector2D(vector.X / divisor, vector.Y / divisor);
}

// Usage: All compound assignments work automatically
var vector1 = new Vector2D(1, 2);
var vector2 = new Vector2D(3, 4);

vector1 += vector2;    // Automatically uses operator + → vector1 = (4, 6)
vector1 -= vector2;    // Automatically uses operator - → vector1 = (1, 2)
vector1 *= 2.0;        // Automatically uses operator * → vector1 = (2, 4)
vector1 /= 2.0;        // Automatically uses operator / → vector1 = (1, 2)

// The compound assignment operators inherit all validation and behavior
// from the basic operators, including exception handling and edge cases
```

**Benefits of Automatic Generation:**
1. **Consistency**: Compound assignments always behave identically to their expanded form
2. **Maintenance**: Changes to basic operators automatically apply to compound assignments
3. **Performance**: The compiler can optimize compound assignments effectively
4. **Completeness**: You get a full set of assignment operators with minimal code

**Important Considerations:**
- Compound assignments modify the left operand in place (for mutable types)
- For immutable value types, compound assignment creates a new value and assigns it back
- The left operand must be a variable (lvalue) that can be assigned to
- All validation and exception handling from your basic operators applies automatically

**Performance Notes:**
For value types, compound assignment can be more efficient than separate operations because it avoids creating intermediate temporary objects in some scenarios. However, for immutable types, the behavior is logically equivalent to `a = a + b`.

### 6. **Checked Operators (C# 11+)**

Checked operators represent one of the most significant recent additions to operator overloading, introduced in C# 11. They provide a mechanism to define separate behavior for operations performed in checked versus unchecked contexts, enabling fine-grained control over overflow handling and arithmetic safety.

**The Problem Checked Operators Solve:**
In financial, scientific, or safety-critical applications, arithmetic overflow can lead to incorrect results or security vulnerabilities. Traditional operator overloading could not distinguish between contexts where overflow should be detected versus where wraparound behavior is acceptable.

**How Checked Operators Work:**
When an operation is performed within a `checked` context (either a checked block or when overflow checking is enabled globally), the compiler will preferentially call the checked version of an operator if it exists. Otherwise, it falls back to the regular operator.

```csharp
public struct SafeNumber
{
    public int Value { get; }
    
    public SafeNumber(int value) => Value = value;
    
    // Regular (unchecked) addition: Allows overflow with wraparound behavior
    // This is the default behavior that maintains backward compatibility
    public static SafeNumber operator +(SafeNumber x, SafeNumber y) =>
        new SafeNumber(x.Value + y.Value);
    
    // Checked addition: Throws OverflowException on overflow
    // Only called when the operation is in a checked context
    public static SafeNumber operator checked +(SafeNumber x, SafeNumber y) =>
        new SafeNumber(checked(x.Value + y.Value));
    
    // Regular subtraction
    public static SafeNumber operator -(SafeNumber x, SafeNumber y) =>
        new SafeNumber(x.Value - y.Value);
    
    // Checked subtraction: Prevents underflow as well as overflow
    public static SafeNumber operator checked -(SafeNumber x, SafeNumber y) =>
        new SafeNumber(checked(x.Value - y.Value));
    
    // Checked multiplication: Prevents overflow in multiplication
    public static SafeNumber operator checked *(SafeNumber x, SafeNumber y) =>
        new SafeNumber(checked(x.Value * y.Value));
    
    // You can mix checked and unchecked operators as needed
    // Not all operators need checked versions
    public static SafeNumber operator *(SafeNumber x, SafeNumber y) =>
        new SafeNumber(x.Value * y.Value);
}

// Usage examples demonstrating different contexts
var num1 = new SafeNumber(int.MaxValue - 1);  // 2,147,483,646
var num2 = new SafeNumber(5);

// Unchecked context: Uses regular operator, allows overflow
var uncheckedResult = num1 + num2;            // Overflows to negative number

// Checked context: Uses checked operator, throws on overflow
try
{
    var checkedResult = checked(num1 + num2);  // Throws OverflowException
}
catch (OverflowException ex)
{
    Console.WriteLine($"Overflow detected: {ex.Message}");
}

// You can also use checked blocks for multiple operations
checked
{
    var result1 = num1 + num2;                 // Uses checked operator
    var result2 = result1 * 2;                 // Uses checked operator
}
```

**Design Guidelines for Checked Operators:**
1. **Provide both versions**: For critical arithmetic operations, implement both checked and unchecked versions
2. **Consistent behavior**: Checked operators should perform the same logical operation but with overflow detection
3. **Performance consideration**: Checked operations may be slightly slower due to overflow checking
4. **Exception handling**: Checked operators should throw appropriate exceptions (typically `OverflowException`)
5. **Documentation**: Clearly document which operators support checked behavior

**Real-World Applications:**
- **Financial calculations**: Preventing overflow in monetary calculations
- **Scientific computing**: Ensuring precision in mathematical operations
- **Security-critical code**: Preventing integer overflow vulnerabilities
- **Embedded systems**: Detecting arithmetic errors in resource-constrained environments

**Compiler Integration:**
The choice between checked and unchecked operators is made at compile time based on the context. This means there is no runtime performance penalty for the decision itself, only for the overflow checking within the checked operator implementation.

### 7. **True/False Operators (Three-Valued Logic)**

The `true` and `false` operators represent one of the most specialized forms of operator overloading in C#. They enable custom types to participate in conditional expressions and logical operations, creating sophisticated Boolean-like behavior that goes beyond simple true/false semantics.

**Understanding Three-Valued Logic:**
Traditional Boolean logic has two states: true and false. However, many real-world scenarios require a third state representing "unknown" or "null". This is particularly common in database systems where NULL values are distinct from both true and false.

**Key Requirements:**
- Both `true` and `false` operators must be implemented as a pair
- They return `bool` (not your custom type)
- They enable your type to work with `if` statements, `while` loops, and conditional operators
- When combined with bitwise operators (`&`, `|`), they enable short-circuit logical operators (`&&`, `||`)

**How It Works:**
When your type appears in a conditional context, C# calls the appropriate operator:
- In `if (value)`, the `true` operator determines if the condition is met
- The `false` operator is used in certain logical operations and optimizations
- The logical AND (`&&`) and OR (`||`) operators can work with your type if you implement both bitwise operators and true/false operators

```csharp
public struct SqlBoolean
{
    private readonly byte value;
    
    // Three distinct states with clear numeric representations
    public static readonly SqlBoolean Null = new SqlBoolean(0);
    public static readonly SqlBoolean False = new SqlBoolean(1);
    public static readonly SqlBoolean True = new SqlBoolean(2);
    
    private SqlBoolean(byte value) => this.value = value;
    
    // True operator: Determines when the value should evaluate to true in conditionals
    // Only returns true when the value is explicitly True (not Null)
    public static bool operator true(SqlBoolean x) =>
        x.value == True.value;
    
    // False operator: Determines when the value should evaluate to false in conditionals
    // Only returns true when the value is explicitly False (not Null)
    public static bool operator false(SqlBoolean x) =>
        x.value == False.value;
    
    // Logical operations implementing three-valued logic rules
    // These work with && and || when true/false operators are defined
    
    // AND operation: Result is False if either operand is False
    // Result is Null if either operand is Null (but neither is False)
    // Result is True only if both operands are True
    public static SqlBoolean operator &(SqlBoolean x, SqlBoolean y)
    {
        if (x.value == False.value || y.value == False.value) return False;
        if (x.value == Null.value || y.value == Null.value) return Null;
        return True;
    }
    
    // OR operation: Result is True if either operand is True
    // Result is Null if either operand is Null (but neither is True)
    // Result is False only if both operands are False
    public static SqlBoolean operator |(SqlBoolean x, SqlBoolean y)
    {
        if (x.value == True.value || y.value == True.value) return True;
        if (x.value == Null.value || y.value == Null.value) return Null;
        return False;
    }
    
    // NOT operation: True becomes False, False becomes True
    // Null remains Null (unknown negated is still unknown)
    public static SqlBoolean operator !(SqlBoolean x)
    {
        if (x.value == Null.value) return Null;
        return x.value == False.value ? True : False;
    }
    
    // Standard equality and conversion operators
    public static bool operator ==(SqlBoolean x, SqlBoolean y) => x.value == y.value;
    public static bool operator !=(SqlBoolean x, SqlBoolean y) => x.value != y.value;
    
    // Explicit conversion to bool (throws for Null to prevent silent errors)
    public static explicit operator bool(SqlBoolean x)
    {
        if (x.value == Null.value)
            throw new InvalidOperationException("Cannot convert SqlBoolean.Null to bool");
        return x.value == True.value;
    }
    
    // Implicit conversion from bool (safe and intuitive)
    public static implicit operator SqlBoolean(bool value) => value ? True : False;
    
    public override string ToString()
    {
        if (value == Null.value) return "Null";
        if (value == False.value) return "False";
        return "True";
    }
}

// Usage examples demonstrating three-valued logic in action
SqlBoolean unknown = SqlBoolean.Null;
SqlBoolean confirmed = SqlBoolean.True;
SqlBoolean denied = SqlBoolean.False;

// Conditional statements use the true operator
if (confirmed)        // true operator returns true
{
    // This block executes
}

if (unknown)          // true operator returns false (unknown is not true)
{
    // This block does NOT execute
}

if (!unknown)         // NOT of unknown is still unknown; true operator returns false
{
    // This block does NOT execute either
}

// Three-valued logic operations
var result1 = confirmed & unknown;   // True AND Null = Null
var result2 = confirmed | unknown;   // True OR Null = True
var result3 = denied & unknown;      // False AND Null = False
var result4 = denied | unknown;      // False OR Null = Null

// Short-circuit operations work when both bitwise and true/false operators exist
var shortCircuit1 = confirmed && unknown;   // Uses & operator with short-circuit behavior
var shortCircuit2 = denied || unknown;      // Uses | operator with short-circuit behavior
```

**Real-World Applications:**
- **Database integration**: Handling SQL NULL values in business logic
- **Validation systems**: Representing pending, passed, and failed states
- **Configuration systems**: Supporting enabled, disabled, and default states
- **State machines**: Modeling states that can be unknown or transitional

**Performance Considerations:**
Three-valued logic types are typically implemented as value types (structs) to avoid heap allocation. The operations should be lightweight since they may be called frequently in conditional expressions and loops.
## Best Practices and Design Guidelines

### Consistency Rules

Operator overloading success depends heavily on maintaining consistency across related operations. These rules are not just recommendations but are often enforced by the C# compiler to prevent logical errors and ensure predictable behavior.

**Mandatory Pairing Requirements:**
- **Equality operators**: If you override `==`, you must also override `!=`. The compiler will generate errors if you implement only one.
- **Comparison operators**: Implement all comparison operators (`<`, `>`, `<=`, `>=`) or none. Partial implementation leads to inconsistent behavior.
- **Object overrides**: When implementing equality operators, you must override `Equals(object)` and `GetHashCode()` to ensure consistency between operator-based and method-based equality checks.

**Mathematical Symmetry:**
Operations should behave consistently with mathematical expectations:
- **Commutativity**: Where mathematically appropriate, `a + b` should equal `b + a`
- **Associativity**: For operations like addition, `(a + b) + c` should equal `a + (b + c)`
- **Identity elements**: Operations should respect mathematical identity (e.g., `x + 0 = x`, `x * 1 = x`)
- **Inverse operations**: If you implement `+`, consider whether `-` should be the inverse operation

**Logical Consistency:**
- Equality operators should be reflexive (`a == a` is always true), symmetric (`a == b` implies `b == a`), and transitive
- Comparison operators should define a total ordering where possible
- Hash codes must be consistent with equality (equal objects must have equal hash codes)

### Safety Considerations

Robust operator overloading requires careful attention to edge cases, error conditions, and potential misuse scenarios.

**Input Validation:**
```csharp
public static Money operator +(Money left, Money right)
{
    // Validate preconditions before performing operations
    if (left.Currency != right.Currency)
        throw new InvalidOperationException(
            $"Cannot add {left.Currency} and {right.Currency}. Currencies must match.");
    
    // Check for overflow in financial calculations
    try
    {
        return new Money(checked(left.Amount + right.Amount), left.Currency);
    }
    catch (OverflowException)
    {
        throw new OverflowException(
            $"Addition of {left.Amount} and {right.Amount} would cause overflow.");
    }
}
```

**Null Handling:**
For reference types, always consider null inputs:
```csharp
public static bool operator ==(Money left, Money right)
{
    // Handle null references properly
    if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
    if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
    
    return left.Amount == right.Amount && left.Currency == right.Currency;
}
```

**Exception Handling:**
- Throw appropriate, descriptive exceptions for invalid operations
- Use standard exception types where possible (`ArgumentException`, `InvalidOperationException`, `OverflowException`)
- Provide clear error messages that help developers understand what went wrong
- Consider whether operations should fail fast or handle edge cases gracefully

### Performance Optimization

Operator overloading can impact performance, especially for frequently used mathematical types. Consider these optimization strategies:

**Struct vs Class Decision:**
```csharp
// Prefer structs for mathematical types to avoid heap allocation
public struct Point2D  // Good: No heap allocation, passed by value
{
    public double X { get; }
    public double Y { get; }
    
    // Operators work on stack-allocated values
    public static Point2D operator +(Point2D left, Point2D right) =>
        new Point2D(left.X + right.X, left.Y + right.Y);
}

// Use classes only when reference semantics are needed
public class ComplexMatrix  // Appropriate: Large objects benefit from reference semantics
{
    private double[,] values;
    
    // Reference semantics avoid copying large data structures
    public static ComplexMatrix operator +(ComplexMatrix left, ComplexMatrix right)
    {
        // Implementation works with references, not copies
    }
}
```

**Parameter Optimization:**
```csharp
// For large structs, consider using 'in' parameters to avoid copying
public static Matrix4x4 operator +(in Matrix4x4 left, in Matrix4x4 right)
{
    // Work with references to avoid copying 64 bytes per parameter
    return new Matrix4x4(/* implementation */);
}

// Expression-bodied members for simple operations
public static Vector2D operator +(Vector2D left, Vector2D right) =>
    new Vector2D(left.X + right.X, left.Y + right.Y);  // Concise and efficient
```

**Memory Allocation Considerations:**
- Minimize object allocation in frequently called operators
- Consider object pooling for complex mathematical operations
- Use stackalloc for temporary arrays in performance-critical code
- Prefer immutable types to avoid defensive copying

## Real-World Applications

Operator overloading shines in domain-specific scenarios where mathematical or logical operations are central to the problem domain. Understanding these applications helps developers recognize when and how to apply operator overloading effectively.

### Financial and Business Calculations

Financial applications require precise arithmetic with domain-specific validation and business rules. Operator overloading makes financial calculations more readable and less error-prone.

```csharp
public struct Money
{
    private readonly decimal amount;
    private readonly string currency;
    
    public decimal Amount => amount;
    public string Currency => currency;
    
    public Money(decimal amount, string currency)
    {
        this.amount = amount;
        this.currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }
    
    // Business rule: Only same currencies can be combined
    public static Money operator +(Money left, Money right)
    {
        ValidateSameCurrency(left, right, "addition");
        return new Money(left.amount + right.amount, left.currency);
    }
    
    public static Money operator -(Money left, Money right)
    {
        ValidateSameCurrency(left, right, "subtraction");
        return new Money(left.amount - right.amount, left.currency);
    }
    
    // Scaling operations for calculations like taxes, discounts
    public static Money operator *(Money money, decimal factor) =>
        new Money(money.amount * factor, money.currency);
    
    public static Money operator *(decimal factor, Money money) =>
        money * factor;  // Commutative multiplication
    
    // Percentage calculations
    public static Money operator *(Money money, Percentage percent) =>
        money * (percent.Value / 100m);
    
    private static void ValidateSameCurrency(Money left, Money right, string operation)
    {
        if (left.currency != right.currency)
            throw new InvalidOperationException(
                $"Cannot perform {operation} on {left.currency} and {right.currency}");
    }
}

public struct Percentage
{
    public decimal Value { get; }
    public Percentage(decimal value) => Value = value;
    
    // Natural percentage operations
    public static Percentage operator +(Percentage left, Percentage right) =>
        new Percentage(left.Value + right.Value);
}

// Usage in business scenarios
var salary = new Money(5000m, "USD");
var bonus = new Money(1000m, "USD");
var taxRate = new Percentage(22m);

var grossIncome = salary + bonus;           // $6,000 USD
var tax = grossIncome * taxRate;            // $1,320 USD (22% of gross)
var netIncome = grossIncome - tax;          // $4,680 USD
var yearlyProjection = netIncome * 12;      // $56,160 USD
```

### Mathematical and Scientific Computing

Scientific applications often involve complex mathematical operations where operator overloading provides natural expression of mathematical formulas.

```csharp
public struct Vector3D
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    
    public Vector3D(double x, double y, double z) => (X, Y, Z) = (x, y, z);
    
    // Vector arithmetic mirrors mathematical notation
    public static Vector3D operator +(Vector3D a, Vector3D b) =>
        new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    
    public static Vector3D operator -(Vector3D a, Vector3D b) =>
        new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    
    // Scalar multiplication
    public static Vector3D operator *(Vector3D v, double scalar) =>
        new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
    
    public static Vector3D operator *(double scalar, Vector3D v) => v * scalar;
    
    // Dot product (returns scalar)
    public static double operator *(Vector3D a, Vector3D b) =>
        a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    
    // Cross product using ^ operator (less common but intuitive in 3D contexts)
    public static Vector3D operator ^(Vector3D a, Vector3D b) =>
        new Vector3D(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    
    public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);
    public Vector3D Normalized => this / Magnitude;
    
    public static Vector3D operator /(Vector3D v, double scalar) =>
        new Vector3D(v.X / scalar, v.Y / scalar, v.Z / scalar);
}

// Usage in physics simulations
var velocity = new Vector3D(10, 0, 5);      // m/s
var acceleration = new Vector3D(0, -9.8, 0); // gravity
var timeStep = 0.1; // seconds

// Physics calculations using natural mathematical notation
var newVelocity = velocity + acceleration * timeStep;
var displacement = velocity * timeStep + 0.5 * acceleration * timeStep * timeStep;

// Vector operations
var force1 = new Vector3D(100, 0, 0);
var force2 = new Vector3D(0, 50, 0);
var resultantForce = force1 + force2;       // Vector addition
var work = force1 * displacement;           // Dot product for work calculation
```

### Game Development and Graphics

Game development extensively uses operator overloading for mathematical operations on positions, rotations, colors, and transformations.

```csharp
public struct Color
{
    public float R { get; }  // Red component (0-1)
    public float G { get; }  // Green component (0-1)
    public float B { get; }  // Blue component (0-1)
    public float A { get; }  // Alpha component (0-1)
    
    public Color(float r, float g, float b, float a = 1.0f) => (R, G, B, A) = (r, g, b, a);
    
    // Color blending operations
    public static Color operator +(Color c1, Color c2) =>
        new Color(
            Math.Min(c1.R + c2.R, 1.0f),
            Math.Min(c1.G + c2.G, 1.0f),
            Math.Min(c1.B + c2.B, 1.0f),
            Math.Min(c1.A + c2.A, 1.0f));
    
    // Color multiplication for lighting effects
    public static Color operator *(Color color, float intensity) =>
        new Color(color.R * intensity, color.G * intensity, color.B * intensity, color.A);
    
    // Color modulation
    public static Color operator *(Color c1, Color c2) =>
        new Color(c1.R * c2.R, c1.G * c2.G, c1.B * c2.B, c1.A * c2.A);
    
    // Predefined colors
    public static readonly Color Red = new Color(1, 0, 0);
    public static readonly Color Green = new Color(0, 1, 0);
    public static readonly Color Blue = new Color(0, 0, 1);
    public static readonly Color White = new Color(1, 1, 1);
}

// Usage in game rendering
var baseColor = Color.Red;
var lightIntensity = 0.8f;
var ambientLight = new Color(0.2f, 0.2f, 0.2f);

var finalColor = (baseColor * lightIntensity) + ambientLight;  // Lighting calculation
```

### Domain-Specific Languages and Fluent APIs

Operator overloading enables the creation of domain-specific languages that read naturally and express complex concepts concisely.

```csharp
public struct TimeSpan
{
    private readonly double totalSeconds;
    
    public TimeSpan(double seconds) => totalSeconds = seconds;
    
    public static TimeSpan operator +(TimeSpan left, TimeSpan right) =>
        new TimeSpan(left.totalSeconds + right.totalSeconds);
    
    public static TimeSpan operator *(TimeSpan timeSpan, double factor) =>
        new TimeSpan(timeSpan.totalSeconds * factor);
    
    // Conversion operators for fluent API
    public static implicit operator TimeSpan(double seconds) => new TimeSpan(seconds);
    
    public double TotalMinutes => totalSeconds / 60;
    public double TotalHours => totalSeconds / 3600;
}

// Extension methods for fluent syntax
public static class TimeExtensions
{
    public static TimeSpan Seconds(this int value) => new TimeSpan(value);
    public static TimeSpan Minutes(this int value) => new TimeSpan(value * 60);
    public static TimeSpan Hours(this int value) => new TimeSpan(value * 3600);
}

// Natural language-like expressions
var taskDuration = 2.Hours() + 30.Minutes() + 15.Seconds();
var totalTime = taskDuration * 3;  // Three iterations of the task
var breakTime = 15.Minutes();
var projectTime = totalTime + breakTime;
```

## Industry Impact

Operator overloading has become fundamental to modern software development across multiple industries:

**Game Development**: Vector mathematics, transformation matrices, physics calculations, and color operations rely heavily on operator overloading to maintain readable code in performance-critical scenarios.

**Financial Software**: Currency calculations, risk modeling, and statistical analysis benefit from operator overloading to ensure business rules are enforced consistently while maintaining mathematical notation.

**Scientific Computing**: Mathematical libraries, statistical analysis tools, and simulation software use operator overloading to create APIs that mirror mathematical expressions, reducing the cognitive load on domain experts.

**Graphics Programming**: 3D transformations, shader programming, and image processing operations become more intuitive and less error-prone when expressed through overloaded operators.

**Framework Development**: Major frameworks like Unity, Unreal Engine, and mathematical libraries like Math.NET rely extensively on operator overloading to provide developer-friendly APIs.

## Integration with Modern C# Features

Operator overloading continues to evolve with the C# language, integrating seamlessly with modern language features:

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

**Generic Math Interfaces (C# 11+)**:
```csharp
public struct Vector2D<T> where T : INumber<T>
{
    public T X { get; init; }
    public T Y { get; init; }
    
    public static Vector2D<T> operator +(Vector2D<T> left, Vector2D<T> right) =>
        new Vector2D<T> { X = left.X + right.X, Y = left.Y + right.Y };
}
```

---

**Conclusion**: Operator overloading transforms custom types into first-class citizens in the C# language ecosystem. When implemented thoughtfully with attention to consistency, safety, and performance, it creates intuitive APIs that feel natural to developers and integrate seamlessly with the broader .NET ecosystem. The key to successful operator overloading lies in understanding when operations are truly mathematical or logical in nature, and implementing them with the same rigor and attention to detail as the built-in operators they complement.
