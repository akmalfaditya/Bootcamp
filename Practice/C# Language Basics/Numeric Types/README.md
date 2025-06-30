# Numeric Types - Understanding C#'s Number System

Welcome to the Numeric Types project! This comprehensive demonstration covers C#'s complete set of numeric types and their behaviors. Understanding numeric types is essential for any application dealing with calculations, measurements, financial data, or scientific computations.

## What This Project Demonstrates

This project provides hands-on examples of:

### 1. Integral Types (Whole Numbers)
- **Signed integers**: `sbyte`, `short`, `int`, `long` - can store negative numbers
- **Unsigned integers**: `byte`, `ushort`, `uint`, `ulong` - only positive numbers, larger positive range
- Size differences and when to use each type
- Min/Max value constants and range limitations

### 2. Real Types (Decimal Numbers)
- **float**: 32-bit floating point (~7 digits precision) - for graphics and games
- **double**: 64-bit floating point (~15-17 digits precision) - for scientific calculations
- **decimal**: 128-bit decimal (28-29 digits precision) - for financial calculations
- Precision vs performance trade-offs

### 3. Numeric Literals and Notations
- **Decimal literals**: `123`, `1_000_000` (with underscores for readability)
- **Hexadecimal literals**: `0xFF`, `0x1A2B` (useful for bit manipulation)
- **Binary literals**: `0b1010`, `0b1111_0000` (clear bit patterns)
- **Scientific notation**: `1.23e6`, `4.56E-3` (exponential form)

### 4. Type Inference and Suffixes
- How C# automatically determines literal types
- Explicit type suffixes: `L` (long), `UL` (ulong), `F` (float), `D` (double), `M` (decimal)
- When suffixes are required vs optional

### 5. Numeric Conversions
- **Implicit conversions**: Safe, automatic conversions (no data loss)
- **Explicit conversions**: Manual casting when data might be lost
- Conversion rules between integral and floating-point types
- Special decimal conversion requirements

### 6. Arithmetic Operations
- Basic operators: `+`, `-`, `*`, `/`, `%`
- Integer vs floating-point division behavior
- Increment (`++`) and decrement (`--`) operators (prefix vs postfix)
- 8-bit and 16-bit type promotion to `int`

### 7. Overflow and Underflow
- Default "wrap-around" behavior when numbers exceed type limits
- `checked` context - throws exceptions on overflow
- `unchecked` context - explicitly allows overflow
- Performance implications of overflow checking

### 8. Special Floating-Point Values
- **Infinity**: Results from dividing by zero
- **NaN (Not a Number)**: Results from undefined operations like 0.0/0.0
- **Negative Zero**: Special case of zero with a sign
- **Epsilon**: Smallest representable positive value

### 9. Precision and Rounding Errors
- Why `float` and `double` can't represent decimal fractions exactly
- Accumulating rounding errors in calculations
- When to use `decimal` for exact decimal arithmetic
- Financial calculation accuracy comparison

### 10. Bitwise Operations
- Bitwise operators: `&` (AND), `|` (OR), `^` (XOR), `~` (NOT)
- Shift operators: `<<` (left), `>>` (right), `>>>` (unsigned right)
- Practical applications: flags, permissions, bit manipulation

### 11. Small Integer Type Promotion
- How `byte`, `sbyte`, `short`, `ushort` are promoted to `int` for operations
- Compilation issues when assigning back to smaller types
- Mixed-size arithmetic behavior

## Running the Project

Navigate to the project directory and run:

```powershell
dotnet run
```

The program executes 14 comprehensive demonstrations, each clearly labeled and explained.

## Code Structure

The project is organized into logical demonstration methods:

1. **Integral Types Demo** - All signed and unsigned integer types
2. **Real Types Demo** - float, double, decimal with precision examples
3. **Numeric Literals Demo** - All notation formats and readability features
4. **Type Inference Demo** - How C# determines literal types
5. **Numeric Suffixes Demo** - Explicit type specification
6. **Numeric Conversions Demo** - Safe and unsafe conversions
7. **Arithmetic Operators Demo** - Basic math and division behavior
8. **Increment/Decrement Demo** - Prefix vs postfix operators
9. **Overflow/Underflow Demo** - Boundary behavior and checking
10. **Practical Examples Demo** - Real-world usage scenarios
11. **Special Float Values Demo** - NaN, Infinity, and special cases
12. **Double vs Decimal Demo** - Precision comparison for financial use
13. **Bitwise Operations Demo** - Bit manipulation and flags
14. **Small Integer Promotion Demo** - Type promotion rules

## Key Learning Points

### Type Selection Guidelines
- **Use `int`** for most integer operations (it's the default and most efficient)
- **Use `long`** for large numbers like timestamps or file sizes
- **Use `decimal`** for financial calculations requiring exact precision
- **Use `double`** for scientific and mathematical calculations
- **Use `float`** only for graphics/games where speed matters more than precision
- **Use `byte`/`short`** only for memory optimization or interoperability

### Performance Considerations
- `int` operations are fastest (native processor support)
- `decimal` is ~10x slower than `double` but provides exact decimal arithmetic
- Overflow checking (`checked`) has a small performance cost
- Smaller integer types don't improve performance due to promotion to `int`

### Precision Awareness
- Floating-point types (`float`, `double`) use binary representation
- Decimal fractions like 0.1 cannot be represented exactly in binary
- This leads to rounding errors that accumulate over many operations
- `decimal` uses base-10 representation for exact decimal arithmetic

### Common Pitfalls to Avoid
1. **Never use `float` or `double` for money** - use `decimal` instead
2. **Be aware of integer overflow** - use `checked` in critical applications
3. **Remember type promotion** - `byte + byte` results in `int`, not `byte`
4. **Don't ignore precision limits** - even `decimal` has precision limits
5. **Use appropriate suffixes** - `3.14F` for float, `3.14M` for decimal

## Questions to Consider

As you study the code, ask yourself:
1. When would you choose `float` over `double`?
2. Why does `0.1 + 0.2` not exactly equal `0.3` with floating-point types?
3. How does integer overflow behave by default, and when should you use `checked`?
4. Why are small integer types promoted to `int` for arithmetic?
5. What are the practical applications of bitwise operations?

## Real-World Applications

### Financial Systems
- Banking: Account balances, transaction amounts
- E-commerce: Product prices, tax calculations
- Accounting: Ledger entries, financial reports

### Scientific Computing
- Physics simulations: Forces, velocities, accelerations
- Statistical analysis: Data processing, probability calculations
- Mathematical modeling: Equations, algorithms

### Graphics and Games
- Coordinate systems: Screen positions, 3D transformations
- Animation: Frame timing, interpolation
- Physics engines: Collision detection, movement

### System Programming
- Memory management: Buffer sizes, offsets
- Network protocols: Packet headers, checksums
- File operations: Sizes, timestamps

## Next Steps

After mastering numeric types, you'll be ready to explore:
- String manipulation and character encoding
- Boolean logic and conditional statements
- Arrays and collections for storing multiple values
- Object-oriented programming with custom types

Understanding numeric types is fundamental to all advanced C# programming concepts!
