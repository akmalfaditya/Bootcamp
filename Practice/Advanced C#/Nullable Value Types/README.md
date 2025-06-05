# Nullable Value Types in C#

## Learning Objectives
By the end of this module, you will learn:
- **Nullable value types** (`T?`) syntax and semantics
- **HasValue and Value properties** for safe nullable access
- **Implicit and explicit conversions** between nullable and non-nullable types
- **Operator lifting** and how operators work with nullable types
- **Boxing and unboxing** behavior with nullable types
- **Null-coalescing operators** (`??` and `??=`) with nullable values
- **Performance implications** and best practices
- **Real-world scenarios** where nullable types are essential

## Core Concepts Covered

### 1. Nullable Type Fundamentals
- **Syntax**: `T?` is shorthand for `Nullable<T>`
- **Value types only**: Only value types can be made nullable
- **Three-state logic**: null, true/false for bool?, or actual value
- **Memory overhead**: Additional byte to track null state

### 2. Nullable<T> Structure
- **HasValue property**: Boolean indicating if value exists
- **Value property**: Actual value (throws if HasValue is false)
- **GetValueOrDefault()**: Safe value retrieval with fallback
- **Equals() and GetHashCode()**: Proper equality semantics

### 3. Operator Lifting
- **Automatic lifting**: Most operators work with nullable operands
- **Null propagation**: Operations with null operands return null
- **Boolean logic**: Special rules for nullable bool operations
- **Comparison operators**: Handle null values appropriately

### 4. Conversion Mechanics
- **Implicit conversion**: From T to T? is always safe
- **Explicit conversion**: From T? to T requires null checking
- **Boxing behavior**: Nullable types box to null or boxed value
- **Type compatibility**: Integration with generic constraints

## Key Features & Examples

### Basic Nullable Type Usage
```csharp
// Declaration and initialization
int? nullableInt = null;        // Can be null
int? withValue = 42;           // Can have value
bool? nullableBool = null;     // Three-state logic: null, true, false
DateTime? nullableDate = new DateTime(2023, 12, 25);

// Checking for values
if (nullableInt.HasValue)
{
    Console.WriteLine($"Value is: {nullableInt.Value}");
}
else
{
    Console.WriteLine("No value present");
}

// Safe value access
int defaultValue = nullableInt.GetValueOrDefault();    // 0 if null
int customDefault = nullableInt.GetValueOrDefault(10); // 10 if null

// Using null-coalescing operator
int finalValue = nullableInt ?? 0;  // 0 if null
int? result = nullableInt ?? withValue ?? 100; // First non-null value
```

### Operator Lifting Examples
```csharp
// Arithmetic operators with nullable types
int? a = 10;
int? b = 20;
int? c = null;

int? sum = a + b;        // 30
int? nullSum = a + c;    // null (null propagates)
int? product = a * b;    // 200
int? nullProduct = b * c; // null

// Comparison operators
bool? greater = a > b;    // false
bool? nullCompare = a > c; // null (can't compare with null)

// Equality operators have special behavior
bool equalNull = (c == null);    // true
bool notEqualNull = (c != null); // false
bool equalValue = (a == 10);     // true

// Boolean logical operators with three-state logic
bool? x = true;
bool? y = false;
bool? z = null;

bool? andResult = x & y;  // false
bool? orResult = x | y;   // true
bool? nullAnd = x & z;    // null
bool? nullOr = x | z;     // true (true OR anything is true)
```

### Conversion Between Nullable and Non-Nullable
```csharp
// Implicit conversion: T to T? (always safe)
int regularInt = 42;
int? nullableInt = regularInt;  // Implicit conversion

// Explicit conversion: T? to T (requires null check)
int? sourceNullable = 100;
if (sourceNullable.HasValue)
{
    int converted = sourceNullable.Value;  // Explicit access
    // Or using cast operator
    int converted2 = (int)sourceNullable; // Throws if null
}

// Safe conversion with null-coalescing
int safeConversion = sourceNullable ?? 0;

// Generic method with nullable types
T GetValueOrDefault<T>(T? nullable, T defaultValue) where T : struct
{
    return nullable ?? defaultValue;
}

// Usage
int result = GetValueOrDefault(nullableInt, 0);
DateTime date = GetValueOrDefault(nullableDate, DateTime.Now);
```

### Boxing and Unboxing Behavior
```csharp
// Boxing nullable types
int? nullableValue = 42;
int? nullValue = null;

object boxedValue = nullableValue;  // Boxes to int (42), not Nullable<int>
object boxedNull = nullValue;       // Boxes to null reference

Console.WriteLine(boxedValue?.GetType()); // System.Int32
Console.WriteLine(boxedNull == null);     // true

// Unboxing behavior
object boxedInt = 42;
int? unboxed = (int?)boxedInt;     // Valid: boxes int to int?
int? unboxedNull = null as int?;   // Valid: null to int?

// Demonstration of boxing differences
void DemonstrateBoxing()
{
    int? value = 42;
    int? nullRef = null;
    
    // These create different boxed representations
    object box1 = value;    // Boxes to System.Int32 with value 42
    object box2 = nullRef;  // Boxes to null reference
    
    // Type checking
    Console.WriteLine(box1 is int);      // true
    Console.WriteLine(box1 is int?);     // true
    Console.WriteLine(box2 is int);      // false
    Console.WriteLine(box2 is int?);     // true (null is valid int?)
}
```

### Nullable Bool Three-State Logic
```csharp
// Nullable bool represents three states: true, false, null
bool? a = true;
bool? b = false; 
bool? c = null;

// Logical AND (&) truth table
Console.WriteLine($"true & true = {a & a}");     // true
Console.WriteLine($"true & false = {a & b}");    // false
Console.WriteLine($"true & null = {a & c}");     // null
Console.WriteLine($"false & null = {b & c}");    // false (false AND anything is false)

// Logical OR (|) truth table
Console.WriteLine($"true | false = {a | b}");    // true
Console.WriteLine($"true | null = {a | c}");     // true (true OR anything is true)
Console.WriteLine($"false | null = {b | c}");    // null

// Practical application in conditionals
bool? userPreference = GetUserPreference(); // Could return null
bool? systemDefault = GetSystemDefault();   // Could return null

// Using three-state logic for decision making
bool finalSetting = (userPreference ?? systemDefault) ?? false;
```

### Working with Collections and LINQ
```csharp
// Nullable types in collections
List<int?> nullableNumbers = new List<int?> { 1, null, 3, null, 5 };

// LINQ operations with nullable types
var validNumbers = nullableNumbers.Where(n => n.HasValue).Select(n => n.Value);
var sum = nullableNumbers.Sum(); // Ignores null values
var average = nullableNumbers.Average(); // Returns nullable double

// Filtering and processing
var nonNullValues = nullableNumbers.Where(x => x != null);
var greaterThanTwo = nullableNumbers.Where(x => x > 2); // Null propagation

// Aggregation with null handling
int totalSum = nullableNumbers.Sum(x => x ?? 0);
double? avgValue = nullableNumbers.Any(x => x.HasValue) 
    ? nullableNumbers.Where(x => x.HasValue).Average(x => x.Value)
    : null;
```

## Tips

### Performance Considerations
- **Memory overhead**: Nullable<T> uses extra byte for HasValue flag
- **Boxing optimization**: null nullable types box to null, not Nullable<T>
- **Method calls**: HasValue and Value access have minimal overhead
- **Generic constraints**: Use `where T : struct` for nullable type parameters

```csharp
// ✅ Efficient: Check HasValue before accessing Value
if (nullableInt.HasValue)
{
    ProcessValue(nullableInt.Value);
}

// ✅ Efficient: Use null-coalescing for defaults
int value = nullableInt ?? 0;

// ⚠️ Consider: Multiple Value accesses
// ❌ Less efficient
if (nullableInt.HasValue)
{
    DoSomething(nullableInt.Value);
    DoSomethingElse(nullableInt.Value);
}

// ✅ More efficient
if (nullableInt.HasValue)
{
    int val = nullableInt.Value;
    DoSomething(val);
    DoSomethingElse(val);
}
```

### Common Pitfalls
```csharp
// ❌ Avoid: Accessing Value without checking HasValue
int? nullable = null;
// int bad = nullable.Value; // Throws InvalidOperationException

// ✅ Better: Always check or use safe access
int safe = nullable ?? 0;
if (nullable.HasValue)
{
    int good = nullable.Value;
}

// ❌ Avoid: Unnecessary boxing
object boxed = (object)(int?)42; // Creates unnecessary Nullable<int> box

// ✅ Better: Direct boxing
object boxed = 42; // Boxes directly to int

// ❌ Avoid: Confusing equality with reference types
object obj = 42;
int? nullable = 42;
// bool equal = nullable == obj; // Compilation error

// ✅ Better: Explicit casting or comparison
bool equal = nullable == (int?)obj;
```

### Design Guidelines
```csharp
// ✅ Use nullable types for optional parameters
public void ConfigureSystem(string name, int? timeout = null, bool? enableLogging = null)
{
    var actualTimeout = timeout ?? 30; // Default 30 seconds
    var actualLogging = enableLogging ?? true; // Default enabled
}

// ✅ Return nullable types for operations that might fail
public int? FindFirstIndex(int[] array, int target)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] == target)
            return i;
    }
    return null; // Not found
}

// ✅ Use nullable types for database fields that allow NULL
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? LastLoginDate { get; set; } // Can be null for new users
    public int? PreferredContactMethod { get; set; } // Optional preference
}
```

## Best Practices & Guidelines

### 1. API Design with Nullable Types
```csharp
public class UserService
{
    // ✅ Use nullable return types for operations that might not find results
    public User? FindUserById(int id)
    {
        return _repository.FindById(id); // Returns null if not found
    }
    
    // ✅ Use nullable parameters for optional values
    public void UpdateUser(int id, string? newName = null, DateTime? lastLogin = null)
    {
        var user = FindUserById(id);
        if (user != null)
        {
            user.Name = newName ?? user.Name;
            user.LastLogin = lastLogin ?? user.LastLogin;
        }
    }
    
    // ✅ Return nullable types for calculations that might be undefined
    public double? CalculateAverageAge(IEnumerable<User> users)
    {
        var ages = users.Where(u => u.Age.HasValue).Select(u => u.Age.Value);
        return ages.Any() ? ages.Average() : null;
    }
}
```

### 2. Database Integration Patterns
```csharp
public class DatabaseMapper
{
    // ✅ Map database NULL values to nullable types
    public Customer MapFromDataReader(IDataReader reader)
    {
        return new Customer
        {
            Id = reader.GetInt32("Id"),
            Name = reader.GetString("Name"),
            Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
            LastOrderDate = reader.IsDBNull("LastOrderDate") 
                ? null 
                : reader.GetDateTime("LastOrderDate"),
            PreferredPaymentMethod = reader.IsDBNull("PreferredPaymentMethod")
                ? null
                : reader.GetInt32("PreferredPaymentMethod")
        };
    }
    
    // ✅ Handle nullable types in database operations
    public void SaveCustomer(Customer customer)
    {
        using var command = new SqlCommand(
            "UPDATE Customers SET Email = @Email, LastOrderDate = @LastOrderDate WHERE Id = @Id");
        
        command.Parameters.AddWithValue("@Id", customer.Id);
        command.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@LastOrderDate", 
            customer.LastOrderDate ?? (object)DBNull.Value);
    }
}
```

### 3. Validation and Business Logic
```csharp
public class BusinessRules
{
    // ✅ Use nullable types for optional business rules
    public ValidationResult ValidateOrder(Order order)
    {
        var errors = new List<string>();
        
        // Required fields
        if (string.IsNullOrEmpty(order.CustomerName))
            errors.Add("Customer name is required");
        
        // Optional fields with business rules
        if (order.DiscountPercentage.HasValue)
        {
            if (order.DiscountPercentage.Value < 0 || order.DiscountPercentage.Value > 100)
                errors.Add("Discount percentage must be between 0 and 100");
        }
        
        if (order.DeliveryDate.HasValue)
        {
            if (order.DeliveryDate.Value < DateTime.Today)
                errors.Add("Delivery date cannot be in the past");
        }
        
        return errors.Any() 
            ? ValidationResult.Failure(errors) 
            : ValidationResult.Success();
    }
    
    // ✅ Use nullable types for conditional calculations
    public decimal CalculateTotal(Order order)
    {
        decimal subtotal = order.Items.Sum(i => i.Price * i.Quantity);
        
        // Apply discount if present
        if (order.DiscountPercentage.HasValue)
        {
            subtotal *= (1 - order.DiscountPercentage.Value / 100);
        }
        
        // Add shipping if required
        decimal shipping = order.RequiresShipping 
            ? (order.ShippingCost ?? CalculateDefaultShipping(order))
            : 0;
        
        return subtotal + shipping;
    }
}
```

## Real-World Applications

### 1. Configuration Management
```csharp
public class AppConfiguration
{
    // Nullable types for optional configuration values
    public string DatabaseConnection { get; set; }
    public int? ConnectionTimeout { get; set; }
    public bool? EnableLogging { get; set; }
    public double? CacheExpirationHours { get; set; }
    
    public void ApplyDefaults()
    {
        ConnectionTimeout ??= 30;
        EnableLogging ??= true;
        CacheExpirationHours ??= 24.0;
    }
    
    public static AppConfiguration LoadFromEnvironment()
    {
        return new AppConfiguration
        {
            DatabaseConnection = Environment.GetEnvironmentVariable("DB_CONNECTION"),
            ConnectionTimeout = ParseInt(Environment.GetEnvironmentVariable("DB_TIMEOUT")),
            EnableLogging = ParseBool(Environment.GetEnvironmentVariable("ENABLE_LOGGING")),
            CacheExpirationHours = ParseDouble(Environment.GetEnvironmentVariable("CACHE_HOURS"))
        };
    }
    
    private static int? ParseInt(string value) =>
        int.TryParse(value, out var result) ? result : null;
        
    private static bool? ParseBool(string value) =>
        bool.TryParse(value, out var result) ? result : null;
        
    private static double? ParseDouble(string value) =>
        double.TryParse(value, out var result) ? result : null;
}
```

### 2. Data Analysis and Statistics
```csharp
public class StatisticsCalculator
{
    public class StatisticsResult
    {
        public double? Mean { get; set; }
        public double? Median { get; set; }
        public double? StandardDeviation { get; set; }
        public int? Mode { get; set; }
        public int Count { get; set; }
    }
    
    public StatisticsResult CalculateStatistics(IEnumerable<int?> values)
    {
        var validValues = values.Where(v => v.HasValue).Select(v => v.Value).ToList();
        
        if (!validValues.Any())
        {
            return new StatisticsResult { Count = 0 };
        }
        
        return new StatisticsResult
        {
            Count = validValues.Count,
            Mean = validValues.Average(),
            Median = CalculateMedian(validValues),
            StandardDeviation = CalculateStandardDeviation(validValues),
            Mode = CalculateMode(validValues)
        };
    }
    
    private double? CalculateMedian(List<int> values)
    {
        if (!values.Any()) return null;
        
        values.Sort();
        int middle = values.Count / 2;
        
        return values.Count % 2 == 0
            ? (values[middle - 1] + values[middle]) / 2.0
            : values[middle];
    }
    
    private int? CalculateMode(List<int> values)
    {
        if (!values.Any()) return null;
        
        var frequency = values.GroupBy(v => v)
            .GroupBy(g => g.Count(), g => g.Key)
            .OrderByDescending(g => g.Key)
            .FirstOrDefault();
            
        return frequency?.Count() == 1 ? frequency.First() : null; // No mode if tie
    }
}
```

### 3. API Response Handling
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public int? ErrorCode { get; set; }
    public DateTime? Timestamp { get; set; }
    
    public static ApiResponse<T> CreateSuccess(T data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Timestamp = DateTime.UtcNow
        };
    }
    
    public static ApiResponse<T> CreateError(string message, int? errorCode = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            ErrorMessage = message,
            ErrorCode = errorCode,
            Timestamp = DateTime.UtcNow
        };
    }
}

public class WeatherService
{
    public async Task<ApiResponse<WeatherData>> GetWeatherAsync(string city)
    {
        try
        {
            var weather = await FetchWeatherData(city);
            return ApiResponse<WeatherData>.CreateSuccess(weather);
        }
        catch (HttpRequestException ex)
        {
            return ApiResponse<WeatherData>.CreateError(
                "Failed to fetch weather data", 500);
        }
        catch (ArgumentException ex)
        {
            return ApiResponse<WeatherData>.CreateError(
                "Invalid city name", 400);
        }
    }
}
```

## 💼 Industry Applications

### Data Processing
- **Database Integration**: Handling NULL values from databases
- **CSV/Excel Import**: Managing missing data in spreadsheets
- **API Integration**: Handling optional fields in external APIs
- **Data Analysis**: Statistical calculations with incomplete datasets

### Enterprise Applications
- **Configuration Systems**: Optional application settings
- **User Preferences**: Optional user-defined values
- **Reporting Systems**: Handling missing or optional report parameters
- **Workflow Management**: Optional steps and conditional processing

### Web Development
- **Form Processing**: Optional form fields and validation
- **API Design**: Optional parameters and response fields
- **Session Management**: Optional user session data
- **Feature Flags**: Optional feature enablement

---

*Master nullable value types to handle optional data elegantly and build robust applications that gracefully handle missing or undefined values in real-world scenarios.*
