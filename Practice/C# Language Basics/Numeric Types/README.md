# C# Numeric Types

This project provides a comprehensive overview of numeric types in C#, which are fundamental for mathematical calculations, financial applications, and scientific computing. Understanding the characteristics and appropriate usage of each numeric type is essential for effective C# programming.

## Objectives

This demonstration explores the various numeric data types available in C#, their characteristics, limitations, and best practices for their usage in different scenarios.

## Core Concepts

The following essential topics are covered in this project:

### 1. Integral Types
These types represent whole numbers without fractional parts:
- **Signed Integer Types**: `sbyte`, `short`, `int`, `long` - can represent both positive and negative values
- **Unsigned Integer Types**: `byte`, `ushort`, `uint`, `ulong` - represent only non-negative values
- **Range and Storage**: Understanding the memory footprint and value ranges of each type
- **Overflow Behavior**: How operations behave when values exceed the type's range

### 2. Floating-Point Types
These types represent real numbers with fractional parts:
- **`float`**: Single-precision (32-bit) floating-point type with approximately 7 decimal digits of precision
- **`double`**: Double-precision (64-bit) floating-point type with approximately 15-17 decimal digits of precision
- **`decimal`**: High-precision (128-bit) decimal type with 28-29 decimal digits of precision

### 3. Numeric Literals
Various ways to express numeric values in source code:
- **Decimal Notation**: Standard base-10 representation (e.g., `123`, `1_000_000`)
- **Hexadecimal Notation**: Base-16 representation using `0x` prefix (e.g., `0xFF`)
- **Binary Notation**: Base-2 representation using `0b` prefix (e.g., `0b1010`)
- **Scientific Notation**: Exponential format for very large or small numbers (e.g., `1.23e6`)

### 4. Type Suffixes and Inference
- **Literal Type Determination**: How C# automatically determines the type of numeric literals
- **Explicit Type Suffixes**: Using suffixes like `L`, `F`, `D`, `M` to specify exact types
- **Type Safety**: Ensuring literals match their intended types

### 5. Numeric Conversions
- **Implicit Conversions**: Automatic conversions that preserve all information (widening conversions)
- **Explicit Conversions**: Manual conversions that may result in data loss (narrowing conversions)
- **Overflow Checking**: Using `checked` and `unchecked` contexts to control overflow behavior
int million = 1_000_000;           // Underscores for readability
int hex = 0xFF_AA_BB;              // Hexadecimal
int binary = 0b1111_0000_1010;     // Binary
float scientific = 1.23e6F;        // Scientific notation
```

### Conversion Safety:
```csharp
// Implicit (safe) conversions
int i = 123;
long l = i;        // int to long is safe
double d = i;      // int to double is safe

// Explicit (potentially unsafe) conversions
long big = 1000000000000;
int smaller = (int)big;    // May lose data!

// Checked arithmetic
checked
{
    int result = int.MaxValue + 1;  // Throws OverflowException
}
```

## Tips

> **Float vs Double vs Decimal**: Use `float` for graphics/games, `double` for scientific calculations, and `decimal` for financial calculations. Each has different precision and performance characteristics!

> **Overflow Behavior**: By default, C# allows integer overflow to wrap around silently. Use `checked` context when you need to detect overflows, especially in financial applications.

> **Performance Hierarchy**: `int` > `long` > `double` > `float` > `decimal` (fastest to slowest). Choose based on your precision needs, not just performance.

## What to Focus On

1. **Choosing the right type**: Match your precision needs with performance requirements
2. **Understanding overflow**: Know when and how numeric operations can fail
3. **Conversion rules**: Implicit vs explicit conversions and when they're safe
4. **Literal syntax**: Use modern C# features for readable code

## Run the Project

```bash
dotnet run
```

The demo showcases:
- All numeric types with their ranges
- Precision differences between real types
- Modern literal syntax examples
- Conversion scenarios and safety
- Overflow behavior demonstration

## Best Practices

1. **Use `int` as default** for whole numbers unless you need larger range
2. **Use `decimal` for money** - it's designed for financial calculations
3. **Use `double` for scientific** calculations requiring precision
4. **Use underscores in large numbers** for readability: `1_000_000`
5. **Be explicit with suffixes** when type matters: `3.14F` vs `3.14`
6. **Use `checked` context** for overflow-sensitive operations

## Real-World Applications

### By Type:
- **`int`**: Counters, IDs, array indices, general arithmetic
- **`long`**: Timestamps, large counters, file sizes
- **`float`**: Graphics coordinates, game physics (when speed > precision)
- **`double`**: Scientific calculations, mathematical algorithms
- **`decimal`**: Financial calculations, currency, accounting
- **`byte`**: Binary data, image processing, network protocols

## Common Pitfalls to Avoid

**Don't:**
- Use `float` for financial calculations (precision loss)
- Ignore integer overflow in critical applications
- Mix `float` and `double` without understanding precision implications
- Use `double` for exact decimal calculations

**Do:**
- Choose types based on your precision requirements
- Use `checked` context for overflow-sensitive code
- Be explicit about numeric literal types when precision matters
- Understand the performance implications of your choices

## Industry Applications

- **Financial Systems**: Precise decimal calculations for accounting
- **Game Development**: Fast floating-point for physics simulations
- **Scientific Computing**: High-precision calculations for research
- **Data Analysis**: Statistical computations on large datasets
- **Embedded Systems**: Optimized numeric types for resource constraints

Remember: The right numeric type can make the difference between a bug-free financial application and a costly calculation error. Choose wisely based on your precision and performance requirements!
