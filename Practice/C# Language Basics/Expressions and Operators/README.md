# C# Expressions and Operators

This project explores expressions and operators, which are the fundamental building blocks for creating calculations, comparisons, and logical operations in C# programs. Understanding these concepts is essential for writing effective and efficient code.

## Objectives

This demonstration covers the various types of expressions and operators available in C#, showing how to combine values and variables to create meaningful computations and decision-making logic.

## Core Concepts

The following essential topics are covered in this project with detailed explanations and practical examples:

### 1. Understanding Expressions

Expressions are the fundamental building blocks of C# programs, representing computations that produce values.

**Expression Definition:**
An expression is a combination of operators, operands, and method calls that evaluates to a single value. Every expression has a type and produces a result that can be used in larger computations.

**Primary Expressions:**
These are the simplest forms of expressions that serve as building blocks for more complex operations:

```csharp
// Literal expressions
int number = 42;           // Integer literal
string text = "Hello";     // String literal
bool flag = true;          // Boolean literal
double pi = 3.14159;       // Double literal

// Variable expressions
int result = number;       // Variable reference
string message = text;     // Variable reference

// Method call expressions
int length = text.Length;  // Property access
string upper = text.ToUpper(); // Method invocation
```

**Complex Expressions:**
Primary expressions combine to create sophisticated operations:

```csharp
// Arithmetic expression combining multiple operations
int complexResult = (number * 2) + (length - 1);

// Boolean expression with multiple conditions
bool isValid = (number > 0) && (text != null) && (text.Length > 0);

// Method chain expression
string processed = text.Trim().ToUpper().Substring(0, 3);
```

**Expression Evaluation Order:**
Understanding evaluation order prevents logical errors and ensures predictable results:

```csharp
// Parentheses control evaluation order
int result1 = 1 + 2 * 3;      // Result: 7 (multiplication first)
int result2 = (1 + 2) * 3;    // Result: 9 (addition first)

// Method calls evaluate left to right
string result = GetFirstName() + " " + GetLastName();
```

### 2. Arithmetic Operators

Arithmetic operators perform mathematical calculations and are fundamental to numerical computations.

**Binary Arithmetic Operators:**
These operators work with two operands to perform mathematical operations:

```csharp
int a = 12, b = 5;

int sum = a + b;        // Addition: 17
int difference = a - b;  // Subtraction: 7
int product = a * b;     // Multiplication: 60
int quotient = a / b;    // Integer division: 2 (truncated)
int remainder = a % b;   // Modulo (remainder): 2
```

**Important Considerations:**
- **Integer Division**: When both operands are integers, the result is truncated (not rounded)
- **Division by Zero**: Throws `DivideByZeroException` for integer types, produces `Infinity` for floating-point types
- **Overflow Behavior**: Results exceeding type limits wrap around unless checked context is used

**Unary Arithmetic Operators:**
These operators work with single operands:

```csharp
int value = 10;

int positive = +value;  // Unary plus: 10 (rarely used)
int negative = -value;  // Unary minus: -10
int preIncrement = ++value;   // Pre-increment: 11, value becomes 11
int postIncrement = value++;  // Post-increment: 11, then value becomes 12
int preDecrement = --value;   // Pre-decrement: 11, value becomes 11
int postDecrement = value--;  // Post-decrement: 11, then value becomes 10
```

**Increment and Decrement Behavior:**
Understanding the difference between pre and post operators is crucial:

```csharp
int counter = 5;
Console.WriteLine(++counter); // Outputs 6, counter is now 6
Console.WriteLine(counter++); // Outputs 6, counter becomes 7
Console.WriteLine(counter);   // Outputs 7
```

**Operator Precedence in Arithmetic:**
Mathematical operations follow standard precedence rules:
1. Parentheses: `()`
2. Unary operators: `+`, `-`, `++`, `--`
3. Multiplicative: `*`, `/`, `%`
4. Additive: `+`, `-`

### 3. Assignment Operators

Assignment operators store values in variables and can be combined with arithmetic operations for efficiency.

**Simple Assignment:**
The basic assignment operator (`=`) stores a value in a variable:

```csharp
int number = 42;           // Assigns 42 to number
string name = "Alice";     // Assigns "Alice" to name
bool isActive = true;      // Assigns true to isActive
```

**Assignment as Expression:**
Assignments are expressions that return the assigned value:

```csharp
int x, y;
int result = (x = 10) + (y = 20); // x=10, y=20, result=30

// Chained assignments work right-to-left
int a, b, c;
a = b = c = 100; // All variables get 100
```

**Compound Assignment Operators:**
These operators combine arithmetic and assignment for cleaner, more efficient code:

```csharp
int value = 20;

value += 10;  // Equivalent to: value = value + 10; (now 30)
value -= 5;   // Equivalent to: value = value - 5;  (now 25)
value *= 2;   // Equivalent to: value = value * 2;  (now 50)
value /= 3;   // Equivalent to: value = value / 3;  (now 16)
value %= 7;   // Equivalent to: value = value % 7;  (now 2)
```

**String Concatenation Assignment:**
```csharp
string message = "Hello";
message += " World";  // message is now "Hello World"
message += "!";       // message is now "Hello World!"
```

**Performance Benefits:**
Compound assignment operators can be more efficient because they:
- Evaluate the left-hand side only once
- May use optimized CPU instructions
- Reduce code verbosity and potential errors

### 4. Comparison and Equality Operators

These operators compare values and return boolean results, forming the foundation of conditional logic.

**Relational Operators:**
Compare the magnitude or ordering of values:

```csharp
int x = 10, y = 20;

bool isLess = x < y;           // true: 10 is less than 20
bool isLessOrEqual = x <= y;   // true: 10 is less than or equal to 20
bool isGreater = x > y;        // false: 10 is not greater than 20
bool isGreaterOrEqual = x >= y; // false: 10 is not greater than or equal to 20
```

**Equality Operators:**
Test for equality and inequality:

```csharp
int a = 5, b = 5, c = 10;

bool areEqual = a == b;      // true: both are 5
bool areNotEqual = a != c;   // true: 5 is not equal to 10
```

**Reference vs Value Equality:**
Understanding the difference is crucial for correct comparisons:

```csharp
// Value types: compare actual values
int num1 = 42, num2 = 42;
bool sameValue = num1 == num2; // true: same value

// Reference types: compare references by default
string str1 = "Hello";
string str2 = "Hello";
bool sameReference = ReferenceEquals(str1, str2); // May be true due to string interning
bool sameContent = str1 == str2; // true: string overrides == for content comparison

// Custom objects: compare references unless overridden
var obj1 = new Person("Alice");
var obj2 = new Person("Alice");
bool sameObject = obj1 == obj2; // false: different objects (unless == is overridden)
```

**Type Compatibility:**
C# allows comparisons between compatible types with automatic conversions:

```csharp
int intValue = 42;
double doubleValue = 42.0;
bool areEqual = intValue == doubleValue; // true: int is converted to double
```

### 5. Logical Operators

Logical operators combine boolean expressions and provide short-circuit evaluation for performance and safety.

**Boolean Logic Operators:**
```csharp
bool condition1 = true;
bool condition2 = false;

bool andResult = condition1 && condition2; // false: both must be true
bool orResult = condition1 || condition2;  // true: at least one must be true
bool notResult = !condition1;              // false: negates the value
```

**Short-Circuit Evaluation:**
This optimization provides both performance benefits and logical safety:

```csharp
// AND short-circuit: if first condition is false, second is not evaluated
if (user != null && user.IsActive)
{
    // user.IsActive only checked if user is not null
    // Prevents NullReferenceException
}

// OR short-circuit: if first condition is true, second is not evaluated
if (isAdmin || HasPermission("read"))
{
    // HasPermission only called if isAdmin is false
    // Saves unnecessary method call
}
```

**Conditional Operator (Ternary):**
Provides concise conditional expressions:

```csharp
int age = 17;
string status = age >= 18 ? "Adult" : "Minor";

// Equivalent to:
string status;
if (age >= 18)
    status = "Adult";
else
    status = "Minor";
```

**Bitwise Logical Operators:**
Work with individual bits for advanced scenarios:

```csharp
byte flags = 0b1010_1100; // Binary literal
byte mask =  0b1111_0000; // Binary mask

byte result1 = (byte)(flags & mask);  // Bitwise AND
byte result2 = (byte)(flags | mask);  // Bitwise OR
byte result3 = (byte)(flags ^ mask);  // Bitwise XOR
byte result4 = (byte)~flags;          // Bitwise NOT
```

### 6. Modern C# Operators

Contemporary C# versions introduce powerful operators that improve code safety and expressiveness.

**Null-Conditional Operators (`?.` and `?[]`):**
Safely navigate object hierarchies without null reference exceptions:

```csharp
User user = GetUser();

// Traditional approach (verbose and error-prone)
if (user != null && user.Profile != null && user.Profile.Settings != null)
{
    string theme = user.Profile.Settings.Theme;
}

// Modern null-conditional approach (concise and safe)
string theme = user?.Profile?.Settings?.Theme;

// Array/indexer null-conditional access
string firstName = user?.Addresses?[0]?.Street;
```

**Null-Coalescing Operators (`??` and `??=`):**
Provide default values for null scenarios:

```csharp
string userName = GetUserName() ?? "Guest";           // Use "Guest" if null
int maxItems = config?.MaxItems ?? 100;              // Use 100 if null

// Null-coalescing assignment (C# 8+)
user.Profile ??= new UserProfile();                  // Assign if null
cache ??= new Dictionary<string, object>();          // Initialize if null
```

**Pattern Matching with `is` Operator:**
Safely test types and extract values:

```csharp
object value = GetSomeValue();

// Traditional approach
if (value is string)
{
    string str = (string)value;
    if (str.Length > 0)
    {
        Console.WriteLine($"Non-empty string: {str}");
    }
}

// Modern pattern matching
if (value is string str && str.Length > 0)
{
    Console.WriteLine($"Non-empty string: {str}");
}

// Advanced pattern matching
string description = value switch
{
    int i when i > 0 => "Positive integer",
    int i when i < 0 => "Negative integer",
    int => "Zero",
    string s when !string.IsNullOrEmpty(s) => $"String: {s}",
    string => "Empty string",
    null => "Null value",
    _ => "Unknown type"
};
```

**Range and Index Operators (`^` and `..`):**
Modern syntax for working with sequences:

```csharp
int[] numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

// Index from end operator (^)
int lastElement = numbers[^1];    // 10 (last element)
int secondLast = numbers[^2];     // 9 (second from end)

// Range operator (..)
int[] firstThree = numbers[0..3]; // {1, 2, 3} (elements 0, 1, 2)
int[] lastThree = numbers[^3..];  // {8, 9, 10} (last 3 elements)
int[] middle = numbers[2..^2];    // {3, 4, 5, 6, 7, 8} (excluding first 2 and last 2)
int[] allElements = numbers[..];  // Complete copy of array
```
## Operator Precedence and Associativity

Understanding operator precedence and associativity is crucial for writing predictable and maintainable code.

### Precedence Hierarchy

Operators are evaluated in a specific order, from highest to lowest precedence:

**1. Primary Operators (Highest Precedence):**
```csharp
obj.member          // Member access
obj?.member         // Null-conditional member access
obj[index]          // Element access
obj?[index]         // Null-conditional element access
obj()               // Method invocation
obj++, obj--        // Post-increment/decrement
typeof(type)        // Type information
nameof(expression)  // Member name
```

**2. Unary Operators:**
```csharp
+x, -x              // Unary plus and minus
!x                  // Logical negation
~x                  // Bitwise complement
++x, --x            // Pre-increment/decrement
(type)x             // Cast operator
```

**3. Multiplicative Operators:**
```csharp
x * y               // Multiplication
x / y               // Division
x % y               // Remainder
```

**4. Additive Operators:**
```csharp
x + y               // Addition
x - y               // Subtraction
```

**5. Shift Operators:**
```csharp
x << y              // Left shift
x >> y              // Right shift
```

**6. Relational and Type-Testing:**
```csharp
x < y, x > y        // Less than, greater than
x <= y, x >= y      // Less than or equal, greater than or equal
x is type           // Type testing
x as type           // Type conversion
```

**7. Equality Operators:**
```csharp
x == y              // Equality
x != y              // Inequality
```

**8. Logical AND:**
```csharp
x & y               // Bitwise AND
```

**9. Logical XOR:**
```csharp
x ^ y               // Bitwise XOR
```

**10. Logical OR:**
```csharp
x | y               // Bitwise OR
```

**11. Conditional AND:**
```csharp
x && y              // Logical AND (short-circuit)
```

**12. Conditional OR:**
```csharp
x || y              // Logical OR (short-circuit)
```

**13. Null-Coalescing:**
```csharp
x ?? y              // Null coalescing
```

**14. Conditional (Ternary):**
```csharp
condition ? x : y   // Conditional expression
```

**15. Assignment Operators (Lowest Precedence):**
```csharp
x = y               // Simple assignment
x += y, x -= y      // Compound assignment
x *= y, x /= y      // Compound assignment
x %= y              // Compound assignment
x ??= y             // Null-coalescing assignment
```

### Associativity Rules

**Left-to-Right Associativity (Most Operators):**
```csharp
int result = 20 / 4 / 2;        // Evaluated as: (20 / 4) / 2 = 2
int difference = 15 - 5 - 3;    // Evaluated as: (15 - 5) - 3 = 7
```

**Right-to-Left Associativity (Assignment and Conditional):**
```csharp
int a, b, c;
a = b = c = 42;                 // Evaluated as: a = (b = (c = 42))

int x = 5;
int y = 10;
int result = x > 0 ? y > 0 ? 1 : 2 : 3; // Evaluated as: x > 0 ? (y > 0 ? 1 : 2) : 3
```

### Practical Precedence Examples

**Common Precedence Scenarios:**
```csharp
// Example 1: Mixed arithmetic operations
int result1 = 2 + 3 * 4;        // Result: 14 (not 20)
int result2 = (2 + 3) * 4;      // Result: 20 (parentheses change order)

// Example 2: Logical operations with comparisons
bool result3 = x > 0 && y < 10; // Comparison operators have higher precedence
bool result4 = x > 0 || y < 10 && z != 0; // Evaluated as: x > 0 || (y < 10 && z != 0)

// Example 3: Assignment in conditional
int value = 0;
if ((value = GetValue()) > 0)   // Assignment happens first, then comparison
{
    Console.WriteLine($"Positive value: {value}");
}
```

**Best Practices for Precedence:**
1. **Use parentheses for clarity** when precedence might be ambiguous
2. **Break complex expressions** into smaller, named parts
3. **Follow the principle of least surprise** - write code that others can easily understand

```csharp
// Instead of this complex expression:
double result = principal * Math.Pow(1 + rate / compoundsPerYear, compoundsPerYear * years);

// Consider breaking it down:
double ratePerPeriod = rate / compoundsPerYear;
double totalPeriods = compoundsPerYear * years;
double growthFactor = Math.Pow(1 + ratePerPeriod, totalPeriods);
double result = principal * growthFactor;
```

## Performance Considerations and Optimization

Understanding the performance implications of different operators and expressions helps write efficient code.

### Operator Performance Characteristics

**Arithmetic Operations:**
- **Addition/Subtraction**: Fastest operations, single CPU instruction
- **Multiplication**: Slightly slower than addition, but still very fast on modern CPUs
- **Division**: Slower than multiplication, especially for integer division
- **Modulo**: Often implemented as division, so similar performance cost

```csharp
// Performance comparison (from fastest to slowest)
int fast = a + b;           // Fastest
int alsoPad = a * b;        // Very fast
int slower = a / b;         // Slower
int slowest = a % b;        // Slowest (often requires division)
```

**Short-Circuit Evaluation Benefits:**
```csharp
// Performance optimization through short-circuiting
if (cheapCondition && expensiveCondition())
{
    // expensiveCondition() only called if cheapCondition is true
}

// Order conditions by cost (cheapest first)
if (user != null && user.IsActive && user.HasPermission("read"))
{
    // Null check is fastest, so do it first
}
```

**Null-Conditional Operator Efficiency:**
```csharp
// Traditional null checking (multiple evaluations)
string result1 = user != null && user.Profile != null ? user.Profile.Name : null;

// Null-conditional operator (single evaluation of each part)
string result2 = user?.Profile?.Name; // More efficient and safer
```

### Memory Allocation Considerations

**Value vs Reference Type Operations:**
```csharp
// Value types: operations on stack (fast)
int value1 = 10;
int value2 = 20;
int sum = value1 + value2; // No heap allocation

// Reference types: potential heap allocations
string str1 = "Hello";
string str2 = "World";
string combined = str1 + str2; // Creates new string object on heap
```

**String Concatenation Performance:**
```csharp
// Inefficient: Creates multiple intermediate strings
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += i.ToString(); // Each iteration creates new string
}

// Efficient: Single allocation with capacity planning
var builder = new StringBuilder(estimatedLength);
for (int i = 0; i < 1000; i++)
{
    builder.Append(i);
}
string result = builder.ToString();
```

**Boxing and Unboxing Costs:**
```csharp
// Avoid boxing when possible
int value = 42;
object boxed = value;       // Boxing: heap allocation
int unboxed = (int)boxed;   // Unboxing: type check + copy

// Use generics to avoid boxing
List<int> numbers = new List<int>(); // No boxing
numbers.Add(42);            // Direct storage
```

## Advanced Expression Patterns

### Expression-Bodied Members

Modern C# allows concise expression syntax for various member types:

```csharp
public class Calculator
{
    private double _value;
    
    // Expression-bodied property
    public bool IsPositive => _value > 0;
    
    // Expression-bodied method
    public double Square(double x) => x * x;
    
    // Expression-bodied constructor
    public Calculator(double value) => _value = value;
    
    // Expression-bodied indexer
    public double this[int power] => Math.Pow(_value, power);
    
    // Expression-bodied finalizer
    ~Calculator() => Console.WriteLine("Calculator disposed");
}
```

### Local Functions with Expressions

```csharp
public double ProcessData(double[] data, Func<double, bool> filter)
{
    // Local function using expression body
    bool IsValid(double value) => value > 0 && value < 1000;
    
    // Combine with provided filter
    return data.Where(x => IsValid(x) && filter(x)).Sum();
}
```

### Pattern Matching Expressions

```csharp
// Switch expressions for concise conditional logic
public string GetDayType(DayOfWeek day) => day switch
{
    DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
    DayOfWeek.Monday => "Start of week",
    DayOfWeek.Friday => "End of week",
    _ => "Weekday"
};

// Property patterns
public string DescribePoint(Point point) => point switch
{
    { X: 0, Y: 0 } => "Origin",
    { X: 0 } => "On Y-axis",
    { Y: 0 } => "On X-axis",
    { X: var x, Y: var y } when x == y => "On diagonal",
    _ => "General point"
};
```

### LINQ Integration

Expressions integrate seamlessly with LINQ for data processing:

```csharp
var results = data
    .Where(item => item.IsActive && item.Score > 0)
    .Select(item => new 
    { 
        item.Name, 
        Grade = item.Score >= 90 ? "A" : 
                item.Score >= 80 ? "B" : 
                item.Score >= 70 ? "C" : "F",
        Status = item.IsActive ? "Active" : "Inactive"
    })
    .OrderByDescending(item => item.Grade)
    .ToList();
```

## Key Learning Points

### Fundamental Principles

1. **Expression Evaluation**: Every expression produces a value and has a specific type
2. **Operator Precedence**: Understanding evaluation order prevents logical errors and unexpected results
3. **Short-Circuit Logic**: Logical operators (`&&`, `||`) optimize performance and provide safety through conditional evaluation
4. **Type Safety**: C#'s type system ensures operations are performed on compatible types
5. **Null Safety**: Modern operators (`?.`, `??`, `??=`) prevent null reference exceptions

### Critical Performance Insights

**Operator Efficiency Guidelines:**
- Arithmetic operations: Addition and subtraction are fastest, division and modulo are slowest
- Short-circuit evaluation: Order conditions from least to most expensive
- String operations: Use StringBuilder for multiple concatenations
- Null-conditional operators: More efficient than manual null checking chains

**Memory Allocation Awareness:**
- Value type operations typically occur on the stack (faster)
- Reference type operations may involve heap allocation (slower)
- String concatenation creates new objects; consider StringBuilder for loops
- Boxing/unboxing adds overhead; use generics when possible

### Safety and Reliability Features

**Compile-Time Safety:**
- Type checking prevents incompatible operations
- Definite assignment ensures variables are initialized
- Null reference analysis (when enabled) catches potential null access

**Runtime Safety:**
- Overflow checking can be enabled with `checked` context
- Division by zero handling varies by type (exception for integers, infinity for floating-point)
- Bounds checking for array access prevents buffer overflows

## Comprehensive Best Practices

### Code Clarity and Maintainability

**Use Parentheses for Clarity:**
```csharp
// Unclear precedence
bool result = a > 0 && b < 10 || c == 5;

// Clear with parentheses
bool result = (a > 0 && b < 10) || (c == 5);
```

**Break Complex Expressions:**
```csharp
// Complex expression (hard to understand)
double payment = principal * (Math.Pow(1 + monthlyRate, months) * monthlyRate) / 
                (Math.Pow(1 + monthlyRate, months) - 1);

// Broken down (easier to understand and debug)
double factor = Math.Pow(1 + monthlyRate, months);
double numerator = principal * factor * monthlyRate;
double denominator = factor - 1;
double payment = numerator / denominator;
```

**Use Meaningful Variable Names:**
```csharp
// Poor naming
bool x = age >= 18 && income > 30000 && creditScore >= 650;

// Descriptive naming
bool isEligibleForLoan = age >= 18 && income > 30000 && creditScore >= 650;
```

### Performance Optimization Techniques

**Optimize Condition Ordering:**
```csharp
// Order from cheapest to most expensive
if (isEnabled && user != null && user.HasExpensivePermission())
{
    // HasExpensivePermission() only called if first two conditions pass
}
```

**Use Appropriate String Operations:**
```csharp
// For single concatenation
string result = firstName + " " + lastName;

// For multiple concatenations in loop
var builder = new StringBuilder();
foreach (var item in items)
{
    builder.AppendLine(item.ToString());
}
string result = builder.ToString();

// For interpolation
string message = $"Hello {name}, you have {count} messages";
```

**Leverage Modern Null-Safe Operators:**
```csharp
// Traditional (verbose and potentially inefficient)
string result = null;
if (user != null && user.Profile != null && user.Profile.Settings != null)
{
    result = user.Profile.Settings.Theme;
}
result = result ?? "Default";

// Modern (concise and efficient)
string result = user?.Profile?.Settings?.Theme ?? "Default";
```

### Error Prevention Strategies

**Validate Input Early:**
```csharp
public decimal CalculateDiscount(decimal price, decimal discountRate)
{
    if (price < 0)
        throw new ArgumentException("Price cannot be negative", nameof(price));
    
    if (discountRate < 0 || discountRate > 1)
        throw new ArgumentOutOfRangeException(nameof(discountRate), "Rate must be between 0 and 1");
    
    return price * discountRate;
}
```

**Use Safe Division:**
```csharp
// Safe division with validation
public double SafeDivide(double numerator, double denominator)
{
    return denominator != 0 ? numerator / denominator : 0;
}

// Or throw exception for invalid input
public double Divide(double numerator, double denominator)
{
    if (denominator == 0)
        throw new DivideByZeroException("Cannot divide by zero");
    
    return numerator / denominator;
}
```

**Handle Overflow Scenarios:**
```csharp
// Check for overflow in calculations
public int SafeMultiply(int a, int b)
{
    try
    {
        checked
        {
            return a * b;
        }
    }
    catch (OverflowException)
    {
        throw new InvalidOperationException($"Multiplication of {a} and {b} would overflow");
    }
}
```

### Modern C# Integration

**Utilize Pattern Matching:**
```csharp
// Instead of multiple if-else statements
public string ProcessValue(object value)
{
    return value switch
    {
        null => "No value",
        int i when i > 0 => $"Positive: {i}",
        int i when i < 0 => $"Negative: {i}",
        int => "Zero",
        string s when !string.IsNullOrWhiteSpace(s) => $"Text: {s}",
        string => "Empty text",
        double d => $"Decimal: {d:F2}",
        _ => $"Unknown: {value.GetType().Name}"
    };
}
```

**Combine with LINQ for Data Processing:**
```csharp
var validActiveUsers = users
    .Where(u => u != null && u.IsActive)
    .Where(u => !string.IsNullOrWhiteSpace(u.Email))
    .Select(u => new UserInfo 
    { 
        Name = u.Name, 
        Email = u.Email.ToLower(),
        Status = u.LastLogin > DateTime.Now.AddDays(-30) ? "Recent" : "Inactive"
    })
    .OrderBy(u => u.Name)
    .ToList();
```

### Debugging and Troubleshooting

**Common Expression Issues:**

**Issue 1: Operator Precedence Confusion**
```csharp
// Problem: Unexpected result due to precedence
int result = 10 + 5 * 2; // Result is 20, not 30

// Solution: Use parentheses
int result = (10 + 5) * 2; // Now result is 30
```

**Issue 2: Integer Division Truncation**
```csharp
// Problem: Integer division truncates
double average = sum / count; // If both are int, result is truncated

// Solution: Cast to double
double average = (double)sum / count; // Now result includes decimals
```

**Issue 3: Null Reference in Chains**
```csharp
// Problem: Any null in chain causes exception
string result = user.Profile.Settings.Theme; // NullReferenceException if any is null

// Solution: Use null-conditional operators
string result = user?.Profile?.Settings?.Theme ?? "Default";
```

**Debugging Techniques:**
```csharp
// Use intermediate variables for complex expressions
public bool ValidateUser(User user)
{
    bool hasValidEmail = !string.IsNullOrWhiteSpace(user?.Email) && 
                        user.Email.Contains("@");
    
    bool hasValidAge = user?.Age > 0 && user.Age < 150;
    
    bool isActive = user?.IsActive == true;
    
    // Easy to debug each condition
    return hasValidEmail && hasValidAge && isActive;
}
```

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


## Summary and Mastery Framework

### Essential Competencies

After completing this module, you should be able to:

- [ ] **Expression Construction**: Build complex expressions from simple components using appropriate operators
- [ ] **Precedence Mastery**: Understand and apply operator precedence rules correctly
- [ ] **Type-Safe Operations**: Perform operations with proper type handling and conversions
- [ ] **Performance Optimization**: Choose efficient operators and expression patterns
- [ ] **Null Safety**: Use modern null-handling operators to prevent runtime exceptions
- [ ] **Pattern Matching**: Apply advanced pattern matching for type-safe conditional logic
- [ ] **Debugging Skills**: Identify and resolve common expression-related issues

### Integration with Advanced C# Concepts

Expressions and operators form the foundation for:

**Object-Oriented Programming:**
- Property expressions and computed properties
- Operator overloading for custom types
- Method chaining and fluent interfaces

**Functional Programming:**
- Lambda expressions and delegates
- LINQ query expressions
- Higher-order functions with expression parameters

**Asynchronous Programming:**
- Conditional awaiting based on expressions
- Task composition using logical operators
- Cancellation token expressions

**Generic Programming:**
- Type parameter constraints using operators
- Generic mathematical operations
- Expression trees for dynamic code generation

### Professional Development Impact

Mastering expressions and operators enables:

1. **Code Quality**: Writing clear, maintainable, and efficient expressions
2. **Problem Solving**: Breaking down complex logic into manageable components
3. **Performance Awareness**: Understanding the cost of different operations
4. **Safety**: Preventing common runtime errors through proper operator usage
5. **Modern C# Usage**: Leveraging contemporary language features effectively

### Real-World Application Domains

**Business Logic Implementation:**
```csharp
// Complex business rule evaluation
public bool IsEligibleForPromotion(Employee employee)
{
    return employee.YearsOfService >= 2 &&
           employee.PerformanceRating >= 4.0 &&
           employee.Department?.Budget > 0 &&
           !employee.HasActiveViolations;
}
```

**Financial Calculations:**
```csharp
// Compound interest with safety checks
public decimal CalculateCompoundInterest(decimal principal, decimal rate, int years)
{
    if (principal <= 0 || rate < 0 || years < 0)
        throw new ArgumentException("Invalid parameters");
    
    return principal * (decimal)Math.Pow(1 + (double)rate, years);
}
```

**Data Validation and Processing:**
```csharp
// Input validation pipeline
public ValidationResult ValidateInput(string input)
{
    return input switch
    {
        null or "" => ValidationResult.Error("Input cannot be empty"),
        string s when s.Length > 100 => ValidationResult.Error("Input too long"),
        string s when s.All(char.IsDigit) => ValidationResult.Success(int.Parse(s)),
        _ => ValidationResult.Error("Invalid format")
    };
}
```

**Algorithm Implementation:**
```csharp
// Search algorithm with modern operators
public T? FindElement<T>(T[] array, Predicate<T> condition) where T : class
{
    return array?.FirstOrDefault(item => item != null && condition(item));
}
```

Understanding expressions and operators thoroughly provides the foundation for all C# programming endeavors, from simple calculations to complex business logic implementation. These concepts are fundamental to writing efficient, maintainable, and robust applications in professional development environments.


