# C# Expressions and Operators

This project explores expressions and operators, which are the fundamental building blocks for creating calculations, comparisons, and logical operations in C# programs. Understanding these concepts is essential for writing effective and efficient code.

## Objectives

This demonstration covers the various types of expressions and operators available in C#, showing how to combine values and variables to create meaningful computations and decision-making logic.

## Core Concepts

The following essential topics are covered in this project:

### 1. Understanding Expressions
- **Expression Definition**: A combination of operators, operands, and method calls that evaluates to a single value
- **Primary Expressions**: The simplest forms of expressions, including literals, variables, and method calls
- **Complex Expressions**: How simple expressions combine to create more sophisticated operations
- **Expression Evaluation**: The order in which expressions are processed

### 2. Arithmetic Operators
- **Binary Arithmetic**: Basic mathematical operations (`+`, `-`, `*`, `/`, `%`)
- **Unary Operators**: Operations on single operands (`+`, `-`, `++`, `--`)
- **Increment and Decrement**: Pre-increment/decrement vs post-increment/decrement behavior
- **Operator Precedence**: The order in which arithmetic operations are performed

### 3. Assignment Operators
- **Simple Assignment**: Using `=` to store values in variables
- **Compound Assignment**: Combining arithmetic and assignment (`+=`, `-=`, `*=`, `/=`, `%=`)
- **Assignment Expressions**: How assignments can be used as expressions that return values
- **Multiple Assignment**: Using tuples and deconstruction for complex assignments

### 4. Comparison and Equality Operators
- **Relational Operators**: Comparing magnitude (`<`, `>`, `<=`, `>=`)
- **Equality Operators**: Testing for equality and inequality (`==`, `!=`)
- **Reference vs Value Equality**: Understanding different types of equality comparisons
- **Type Compatibility**: How comparison works with different but compatible types

### 5. Logical Operators
- **Boolean Logic**: Combining Boolean values (`&&`, `||`, `!`)
- **Short-Circuit Evaluation**: How logical operators optimize performance and provide safety
- **Conditional Operator**: The ternary operator (`?:`) for concise conditional expressions
- **Bitwise Operations**: Working with individual bits for advanced scenarios

### 6. Modern C# Operators
- **Null-Conditional Operators**: Safe navigation (`?.`, `?[]`) to prevent null reference exceptions
- **Null-Coalescing Operators**: Providing default values (`??`, `??=`) for null scenarios
- **Pattern Matching**: Using `is` and `switch` expressions for advanced type checking
- **Range and Index Operators**: Modern syntax (`^`, `..`) for working with sequences
int x = 10;
int y = 3;

// Basic arithmetic
int sum = x + y;        // 13
int difference = x - y; // 7
int product = x * y;    // 30
int quotient = x / y;   // 3 (integer division)
int remainder = x % y;  // 1 (modulo)

// Compound assignment
x += 5;  // x = x + 5; (now x is 15)
y *= 2;  // y = y * 2; (now y is 6)

// Increment/decrement
int a = 5;
int b = ++a; // Pre-increment: a becomes 6, b is 6
int c = a++; // Post-increment: c is 6, a becomes 7
```

### Logical Operations:
```csharp
bool isLoggedIn = true;
bool hasPermission = false;

// Short-circuit evaluation
if (isLoggedIn && CheckPermissions()) // CheckPermissions only called if isLoggedIn is true
{
    // User has access
}

// Conditional operator (ternary)
string status = isLoggedIn ? "Welcome!" : "Please log in";

// Null-coalescing
string userName = GetUserName() ?? "Guest"; // Use "Guest" if GetUserName() returns null
```

### Modern Null-Safe Operations:
```csharp
User user = GetUser();

// Traditional null checking
if (user != null && user.Profile != null)
{
    Console.WriteLine(user.Profile.Name);
}

// Modern null-conditional operators
Console.WriteLine(user?.Profile?.Name ?? "Unknown");

// Null-coalescing assignment (C# 8+)
user.Profile ??= new UserProfile(); // Assign new profile if null
```

### Pattern Matching and Type Testing:
```csharp
object value = GetSomeValue();

// Traditional type testing
if (value is string)
{
    string str = (string)value;
    Console.WriteLine($"String length: {str.Length}");
}

// Modern pattern matching
if (value is string str && str.Length > 0)
{
    Console.WriteLine($"Non-empty string: {str}");
}

// Switch expressions (C# 8+)
string description = value switch
{
    int i when i > 0 => "Positive integer",
    int i when i < 0 => "Negative integer",
    int => "Zero",
    string s => $"String with {s.Length} characters",
    null => "Null value",
    _ => "Unknown type"
};
```

### Index and Range Operations (C# 8+):
```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Index from end
int lastElement = numbers[^1];     // 5 (last element)
int secondLast = numbers[^2];      // 4 (second from end)

// Range operations
int[] firstThree = numbers[0..3];  // { 1, 2, 3 }
int[] lastTwo = numbers[^2..];     // { 4, 5 }
int[] middle = numbers[1..^1];     // { 2, 3, 4 }
```

## Tips

> **Operator Precedence Matters**: When in doubt, use parentheses! `a + b * c` is different from `(a + b) * c`. Clear code is more important than memorizing precedence rules.

> **Short-Circuit Magic**: Use `&&` and `||` strategically. `user != null && user.IsActive` prevents null reference exceptions because the second condition only evaluates if the first is true.

> **Performance Insight**: The null-conditional operator (`?.`) is not just syntax sugar - it's also more efficient than manual null checks in many cases because it only evaluates the left side once.

## What to Focus On

1. **Operator precedence**: Understanding evaluation order
2. **Short-circuit evaluation**: Performance and safety benefits
3. **Null safety**: Using modern operators to prevent exceptions
4. **Type safety**: Pattern matching for safe type conversions

## Run the Project

```bash
dotnet run
```

The demo includes:
- All operator categories with practical examples
- Precedence and associativity demonstrations
- Performance comparisons between approaches
- Real-world usage patterns
- Common pitfalls and solutions

## Best Practices

1. **Use parentheses** for clarity when operator precedence might be unclear
2. **Leverage null-conditional operators** (`?.`) to prevent null reference exceptions
3. **Use pattern matching** instead of explicit type casting when possible
4. **Prefer compound assignment** (`+=`) for readability and potential performance benefits
5. **Use meaningful variable names** even in complex expressions
6. **Consider performance** implications of short-circuit evaluation

## Real-World Applications

### Validation Logic:
```csharp
// Input validation with short-circuiting
bool IsValidEmail(string email) =>
    !string.IsNullOrWhiteSpace(email) &&
    email.Contains('@') &&
    email.Length >= 5 &&
    email.Length <= 254;

// Safe navigation in data processing
decimal? GetDiscountRate(Customer customer) =>
    customer?.Membership?.Level switch
    {
        "Gold" => 0.15m,
        "Silver" => 0.10m,
        "Bronze" => 0.05m,
        _ => null
    };
```

### Mathematical Calculations:
```csharp
// Financial calculations with proper precedence
decimal CalculateCompoundInterest(decimal principal, decimal rate, int years) =>
    principal * (decimal)Math.Pow((double)(1 + rate), years);

// Safe division with null-coalescing
double SafeDivide(double numerator, double denominator) =>
    denominator != 0 ? numerator / denominator : 0;
```

### Collection Operations:
```csharp
// Array slicing for data processing
T[] GetPage<T>(T[] data, int pageSize, int pageNumber)
{
    int start = pageNumber * pageSize;
    int end = Math.Min(start + pageSize, data.Length);
    return data[start..end];
}
```

## Operator Precedence Quick Reference

From highest to lowest precedence:
1. **Primary**: `x.y`, `x?.y`, `x[y]`, `x++`, `x--`
2. **Unary**: `+x`, `-x`, `!x`, `~x`, `++x`, `--x`, `(T)x`
3. **Multiplicative**: `*`, `/`, `%`
4. **Additive**: `+`, `-`
5. **Shift**: `<<`, `>>`
6. **Relational**: `<`, `>`, `<=`, `>=`, `is`, `as`
7. **Equality**: `==`, `!=`
8. **Logical AND**: `&`
9. **Logical XOR**: `^`
10. **Logical OR**: `|`
11. **Conditional AND**: `&&`
12. **Conditional OR**: `||`
13. **Null-coalescing**: `??`
14. **Conditional**: `?:`
15. **Assignment**: `=`, `+=`, `-=`, etc.

## Advanced Expression Patterns

### Expression-bodied Members:
```csharp
public class Calculator
{
    // Expression-bodied property
    public bool IsValid => _value >= 0;
    
    // Expression-bodied method
    public double Square(double x) => x * x;
    
    // Expression-bodied indexer
    public int this[int index] => _values[index];
}
```

### Local Functions with Expressions:
```csharp
public int ProcessData(int[] data)
{
    // Local function using expressions
    bool IsValid(int value) => value > 0 && value < 1000;
    
    return data.Where(IsValid).Sum();
}
```


## Industry Applications

Operators are fundamental to:
- **Business Logic**: Calculations, validations, decision making
- **Data Processing**: Filtering, transforming, aggregating data
- **API Development**: Request validation, response transformation
- **Game Development**: Physics calculations, collision detection
- **Financial Software**: Interest calculations, risk assessments
- **Scientific Computing**: Mathematical modeling, statistical analysis


