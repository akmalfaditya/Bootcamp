# Working With Numbers

## Learning Objectives

By learning this project, you will:
- **Master Numeric Conversions**: Safely parse and convert between different number types
- **Handle Number Bases**: Work with binary, octal, hexadecimal, and custom base systems
- **Use Mathematical Operations**: Leverage the Math class for complex calculations
- **Work with Big Numbers**: Handle arbitrarily large integers and complex numbers
- **Generate Random Numbers**: Create cryptographically secure and pseudo-random numbers
- **Perform Bit Operations**: Manipulate individual bits for optimization and flags
- **Apply Real-World Patterns**: Build financial calculators, scientific applications, and data processing systems

## Core Concepts

### Safe Number Parsing
Always use `TryParse` methods for user input to avoid exceptions and handle invalid data gracefully.

```csharp
// Safe approach - recommended
if (int.TryParse(userInput, out int result))
{
    Console.WriteLine($"Valid number: {result}");
}
else
{
    Console.WriteLine("Invalid input - please enter a number");
}

// Unsafe approach - can throw exceptions
int result = int.Parse(userInput); // FormatException if invalid
```

### Number Base Conversions
C# supports parsing numbers in different bases (2-36) using the `Convert` class.

```csharp
// Hexadecimal (base 16) - common in programming
int hexValue = Convert.ToInt32("FF", 16);        // 255
int hexColor = Convert.ToInt32("A0B1C2", 16);    // 10531778

// Binary (base 2) - useful for bit operations
int binaryValue = Convert.ToInt32("1010", 2);    // 10
int flags = Convert.ToInt32("11110000", 2);      // 240

// Octal (base 8) - used in Unix file permissions
int filePermissions = Convert.ToInt32("755", 8); // 493 (rwxr-xr-x)
```

### Lossless vs Lossy Conversions
Understanding when data can be lost during type conversions is crucial for accuracy.

```csharp
// Lossless conversions (smaller to larger types)
int intValue = 42;
long longValue = intValue;     // No data loss
double doubleValue = intValue; // No data loss

// Lossy conversions (larger to smaller types)  
double pi = 3.14159;
int truncated = (int)pi;       // 3 - decimal part lost!
float floatPi = (float)pi;     // Possible precision loss
```

## Key Features

### 1. **Comprehensive Number Parsing**
```csharp
public static class NumberParser
{
    public static T SafeParse<T>(string input, TryParseDelegate<T> tryParse, T defaultValue = default)
    {
        return tryParse(input, out T result) ? result : defaultValue;
    }
    
    // Usage examples
    int number = SafeParse(userInput, int.TryParse, 0);
    double percentage = SafeParse(userInput, double.TryParse, 0.0);
    decimal money = SafeParse(userInput, decimal.TryParse, 0m);
}

public delegate bool TryParseDelegate<T>(string input, out T result);
```

### 2. **Mathematical Operations with Math Class**
```csharp
public static class AdvancedMath
{
    public static double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }
    
    public static double CalculateCompoundInterest(double principal, double rate, int times, int years)
    {
        return principal * Math.Pow(1 + (rate / times), times * years);
    }
    
    public static bool IsPrime(int number)
    {
        if (number < 2) return false;
        
        int sqrt = (int)Math.Sqrt(number);
        for (int i = 2; i <= sqrt; i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
}
```

### 3. **Big Integer Operations**
```csharp
using System.Numerics;

public static class BigNumberCalculations
{
    // Calculate factorial of very large numbers
    public static BigInteger Factorial(int n)
    {
        BigInteger result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }
    
    // Fibonacci sequence with large numbers
    public static BigInteger Fibonacci(int n)
    {
        if (n <= 1) return n;
        
        BigInteger prev = 0, curr = 1;
        for (int i = 2; i <= n; i++)
        {
            BigInteger next = prev + curr;
            prev = curr;
            curr = next;
        }
        return curr;
    }
    
    // Work with numbers beyond long.MaxValue
    public static void DemonstrateHugeNumbers()
    {
        BigInteger googol = BigInteger.Pow(10, 100);  // 10^100
        BigInteger factorial100 = Factorial(100);      // 100!
        
        Console.WriteLine($"Googol: {googol}");
        Console.WriteLine($"100! = {factorial100}");
    }
}
```

### 4. **Complex Number Mathematics**
```csharp
using System.Numerics;

public static class ComplexMath
{
    public static void DemonstrateComplexOperations()
    {
        // Create complex numbers
        Complex z1 = new Complex(3, 4);        // 3 + 4i
        Complex z2 = new Complex(1, -2);       // 1 - 2i
        
        // Basic operations
        Complex sum = z1 + z2;                 // 4 + 2i
        Complex product = z1 * z2;             // 11 - 2i
        Complex quotient = z1 / z2;            // -1 + 2i
        
        // Advanced operations
        double magnitude = z1.Magnitude;        // |z1| = 5
        double phase = z1.Phase;               // arg(z1) = 0.927 radians
        Complex conjugate = Complex.Conjugate(z1); // 3 - 4i
        
        // Exponential form (Euler's formula)
        Complex exponential = Complex.Exp(Complex.ImaginaryOne * Math.PI); // e^(iπ) = -1
    }
    
    public static Complex SolveQuadratic(double a, double b, double c)
    {
        double discriminant = b * b - 4 * a * c;
        if (discriminant >= 0)
        {
            // Real roots
            double root = (-b + Math.Sqrt(discriminant)) / (2 * a);
            return new Complex(root, 0);
        }
        else
        {
            // Complex roots
            double realPart = -b / (2 * a);
            double imaginaryPart = Math.Sqrt(-discriminant) / (2 * a);
            return new Complex(realPart, imaginaryPart);
        }
    }
}
```

### 5. **Secure Random Number Generation**
```csharp
public static class RandomNumberGenerator
{
    private static readonly Random pseudoRandom = new Random();
    private static readonly RandomNumberGenerator cryptoRandom = RandomNumberGenerator.Create();
    
    // Pseudo-random (fast, predictable seed)
    public static int NextInt(int min, int max) => pseudoRandom.Next(min, max);
    
    public static double NextDouble() => pseudoRandom.NextDouble();
    
    // Cryptographically secure random (slower, unpredictable)
    public static byte[] GenerateSecureBytes(int length)
    {
        byte[] bytes = new byte[length];
        cryptoRandom.GetBytes(bytes);
        return bytes;
    }
    
    public static int GenerateSecureInt(int min, int max)
    {
        byte[] bytes = GenerateSecureBytes(4);
        int value = BitConverter.ToInt32(bytes, 0);
        return Math.Abs(value % (max - min)) + min;
    }
    
    // Generate random numbers with specific distributions
    public static double NextGaussian(double mean = 0, double stdDev = 1)
    {
        // Box-Muller transform for normal distribution
        static double u1 = pseudoRandom.NextDouble();
        static double u2 = pseudoRandom.NextDouble();
        
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
```

### 6. **Bit Manipulation Operations**
```csharp
public static class BitOperations
{
    // Check if a specific bit is set
    public static bool IsBitSet(int value, int bitPosition)
    {
        return (value & (1 << bitPosition)) != 0;
    }
    
    // Set a specific bit
    public static int SetBit(int value, int bitPosition)
    {
        return value | (1 << bitPosition);
    }
    
    // Clear a specific bit
    public static int ClearBit(int value, int bitPosition)
    {
        return value & ~(1 << bitPosition);
    }
    
    // Toggle a specific bit
    public static int ToggleBit(int value, int bitPosition)
    {
        return value ^ (1 << bitPosition);
    }
    
    // Count number of set bits (population count)
    public static int CountSetBits(int value)
    {
        int count = 0;
        while (value != 0)
        {
            count++;
            value &= value - 1; // Remove the lowest set bit
        }
        return count;
    }
    
    // Find the next power of 2
    public static int NextPowerOf2(int value)
    {
        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        return value + 1;
    }
}
```

## Tips

### Parsing Best Practices
- **Always use TryParse**: Safer than Parse() methods, no exception handling needed
- **Validate ranges**: Check if parsed values are within expected bounds
- **Consider culture**: Use `CultureInfo` for international number formats
- **Handle edge cases**: Test with empty strings, null values, and extreme numbers

### Precision and Accuracy
- **Use decimal for money**: Avoid floating-point errors in financial calculations
- **Understand floating-point limits**: `float` has ~7 digits, `double` has ~15-17 digits
- **Round appropriately**: Use `Math.Round()` with proper rounding mode
- **Check for overflow**: Use `checked` context for arithmetic operations

### Performance Considerations
- **Cache expensive calculations**: Store results of complex mathematical operations
- **Use appropriate types**: Don't use `BigInteger` when `long` suffices
- **Minimize boxing**: Prefer generic methods over object-based operations
- **Consider bit operations**: Faster than arithmetic for powers of 2

## Real-World Applications

### Financial Calculator
```csharp
public class FinancialCalculator
{
    public decimal CalculateMonthlyPayment(decimal principal, double annualRate, int years)
    {
        if (annualRate == 0) return principal / (years * 12);
        
        double monthlyRate = annualRate / 12 / 100;
        int totalPayments = years * 12;
        
        double factor = Math.Pow(1 + monthlyRate, totalPayments);
        decimal payment = (decimal)(principal * monthlyRate * factor / (factor - 1));
        
        return Math.Round(payment, 2);
    }
    
    public decimal CalculateFutureValue(decimal presentValue, double rate, int periods)
    {
        return presentValue * (decimal)Math.Pow(1 + rate, periods);
    }
}
```

### Scientific Calculations
```csharp
public class ScientificCalculator
{
    public double CalculateStandardDeviation(IEnumerable<double> values)
    {
        double mean = values.Average();
        double squaredDifferences = values.Sum(x => Math.Pow(x - mean, 2));
        return Math.Sqrt(squaredDifferences / values.Count());
    }
    
    public double CalculateCorrelation(IEnumerable<(double x, double y)> points)
    {
        var data = points.ToList();
        double meanX = data.Average(p => p.x);
        double meanY = data.Average(p => p.y);
        
        double numerator = data.Sum(p => (p.x - meanX) * (p.y - meanY));
        double denomX = Math.Sqrt(data.Sum(p => Math.Pow(p.x - meanX, 2)));
        double denomY = Math.Sqrt(data.Sum(p => Math.Pow(p.y - meanY, 2)));
        
        return numerator / (denomX * denomY);
    }
}
```

### Data Processing Pipeline
```csharp
public class NumberProcessor
{
    public static IEnumerable<T> ParseNumbers<T>(IEnumerable<string> inputs, 
        TryParseDelegate<T> parser) where T : struct
    {
        return inputs
            .Where(input => parser(input, out _))
            .Select(input => { parser(input, out T result); return result; });
    }
    
    public static Statistics CalculateStatistics(IEnumerable<double> numbers)
    {
        var data = numbers.ToList();
        if (!data.Any()) return new Statistics();
        
        return new Statistics
        {
            Count = data.Count,
            Sum = data.Sum(),
            Mean = data.Average(),
            Min = data.Min(),
            Max = data.Max(),
            StandardDeviation = CalculateStandardDeviation(data)
        };
    }
}
```

## Industry Impact

Working with numbers effectively is essential for:

**Financial Technology**: Currency calculations, interest computations, risk analysis
**Game Development**: Physics simulations, random generation, mathematical models
**Scientific Computing**: Statistical analysis, numerical methods, data modeling
**Cryptography**: Random number generation, large integer arithmetic, modular arithmetic
**Data Analytics**: Statistical calculations, trend analysis, machine learning algorithms

## Integration with Modern C#

**Pattern Matching with Numbers (C# 7+)**:
```csharp
string ClassifyNumber(object obj) => obj switch
{
    int i when i > 0 => "Positive integer",
    int i when i < 0 => "Negative integer",
    int => "Zero",
    double d when double.IsNaN(d) => "Not a number",
    double d when double.IsInfinity(d) => "Infinity",
    _ => "Not a number"
};
```

**Range and Index with Numbers (C# 8+)**:
```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
var lastThree = numbers[^3..];     // [8, 9, 10]
var firstHalf = numbers[..5];      // [1, 2, 3, 4, 5]
```

**Init-Only Properties with Numbers (C# 9+)**:
```csharp
public record Point(double X, double Y)
{
    public double Distance => Math.Sqrt(X * X + Y * Y);
}
```

---

