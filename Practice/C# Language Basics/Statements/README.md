# 📋 Statements in C#

## 🎯 Learning Objectives
By the end of this module, you will master:
- **Declaration statements** for variable and constant creation
- **Expression statements** for executing operations and assignments
- **Selection statements** (if, switch) for conditional branching
- **Iteration statements** (for, while, foreach) for repetitive operations
- **Jump statements** (break, continue, goto, return) for flow control
- **Exception handling statements** (try, catch, finally, throw)
- **Statement blocks** and scope management
- **Modern C# statement features** and syntax improvements

## 📚 Core Concepts Covered

### 1. Statement Categories
- **Declaration statements**: Create variables, constants, and local functions
- **Expression statements**: Execute expressions and assignments
- **Selection statements**: Conditional execution paths
- **Iteration statements**: Repetitive execution patterns
- **Jump statements**: Control flow modifications
- **Synchronization statements**: Lock and using statements

### 2. Control Flow Fundamentals
- **Sequential execution**: Default top-to-bottom flow
- **Conditional branching**: Decision-based execution paths
- **Iterative execution**: Repeated code execution
- **Exception handling**: Error management and recovery
- **Resource management**: Automatic cleanup patterns

### 3. Scope and Lifetime
- **Block scope**: Variable visibility within statement blocks
- **Local variable lifetime**: Memory management principles
- **Resource disposal**: Using statements and IDisposable
- **Variable shadowing**: Inner scope variable hiding

## 🚀 Key Features & Examples

### Declaration Statements
```csharp
// Variable declarations with initialization
string userName = "Alice";
int age = 25;
bool isActive = true;

// Multiple variable declarations
int x = 10, y = 20, z = 30;

// Array declarations
int[] numbers = { 1, 2, 3, 4, 5 };
string[] names = new string[3];

// Object initialization
var user = new User { Name = "Bob", Email = "bob@example.com" };

// Constant declarations
const double PI = 3.14159;
const string APPLICATION_NAME = "MyApp";

// Local function declarations (C# 7.0+)
int CalculateSum(int a, int b)
{
    return a + b;
}
```

### Selection Statements
```csharp
// If-else statements
if (age >= 18)
{
    Console.WriteLine("Adult");
}
else if (age >= 13)
{
    Console.WriteLine("Teenager");
}
else
{
    Console.WriteLine("Child");
}

// Switch statements (traditional)
switch (dayOfWeek)
{
    case DayOfWeek.Monday:
        Console.WriteLine("Start of work week");
        break;
    case DayOfWeek.Friday:
        Console.WriteLine("TGIF!");
        break;
    default:
        Console.WriteLine("Regular day");
        break;
}

// Switch expressions (C# 8.0+)
string dayType = dayOfWeek switch
{
    DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
    DayOfWeek.Monday => "Monday Blues",
    _ => "Weekday"
};

// Pattern matching in switch (C# 8.0+)
string DescribeObject(object obj) => obj switch
{
    int i when i > 0 => "Positive integer",
    int i when i < 0 => "Negative integer",
    string s when s.Length > 10 => "Long string",
    string s => "Short string",
    null => "Null value",
    _ => "Unknown type"
};
```

### Iteration Statements
```csharp
// For loop - counter-based iteration
for (int i = 0; i < 10; i++)
{
    Console.WriteLine($"Iteration: {i}");
}

// While loop - condition-based iteration
int count = 0;
while (count < 5)
{
    Console.WriteLine($"Count: {count}");
    count++;
}

// Do-while loop - execute at least once
int attempts = 0;
do
{
    Console.WriteLine("Attempting operation...");
    attempts++;
} while (attempts < 3 && !TryOperation());

// Foreach loop - collection iteration
string[] names = { "Alice", "Bob", "Charlie" };
foreach (string name in names)
{
    Console.WriteLine($"Hello, {name}!");
}

// Enhanced foreach with index (C# 8.0+)
foreach (var (item, index) in names.Select((name, i) => (name, i)))
{
    Console.WriteLine($"{index}: {item}");
}

// Foreach with pattern matching
foreach (var item in mixedCollection)
{
    switch (item)
    {
        case string s:
            Console.WriteLine($"String: {s}");
            break;
        case int i:
            Console.WriteLine($"Integer: {i}");
            break;
    }
}
```

### Jump Statements
```csharp
// Break - exit loop or switch
for (int i = 0; i < 100; i++)
{
    if (i == 50)
        break; // Exit the loop
    Console.WriteLine(i);
}

// Continue - skip to next iteration
for (int i = 0; i < 10; i++)
{
    if (i % 2 == 0)
        continue; // Skip even numbers
    Console.WriteLine($"Odd: {i}");
}

// Return - exit method
public int FindFirstPositive(int[] numbers)
{
    foreach (int num in numbers)
    {
        if (num > 0)
            return num; // Exit method immediately
    }
    return -1; // Not found
}

// Goto - direct jump (use sparingly)
int attempts = 0;
retry:
try
{
    // Some operation that might fail
    PerformOperation();
}
catch (Exception ex) when (attempts < 3)
{
    attempts++;
    Console.WriteLine($"Attempt {attempts} failed, retrying...");
    goto retry;
}
```

### Exception Handling Statements
```csharp
// Try-catch-finally
try
{
    // Risky operation
    int result = DivideNumbers(10, 0);
    Console.WriteLine($"Result: {result}");
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Division error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"General error: {ex.Message}");
}
finally
{
    Console.WriteLine("Cleanup operations");
}

// Using statements for resource management
using (var file = new FileStream("data.txt", FileMode.Open))
{
    // File operations
    // Automatically disposed when exiting block
}

// Using declarations (C# 8.0+)
using var connection = new SqlConnection(connectionString);
connection.Open();
// Automatically disposed at end of scope
```

## 💡 Trainer Tips

### Performance Considerations
- **Loop optimization**: Use appropriate loop types for different scenarios
- **Early returns**: Use return statements to avoid unnecessary processing
- **Switch vs if-else**: Switch expressions are often more performant for multiple conditions

```csharp
// ✅ Efficient: Early return pattern
public bool ValidateUser(User user)
{
    if (user == null) return false;
    if (string.IsNullOrEmpty(user.Name)) return false;
    if (string.IsNullOrEmpty(user.Email)) return false;
    
    return IsValidEmail(user.Email);
}

// ✅ Efficient: Use foreach for collections
foreach (var item in collection)
{
    // More readable and often faster than index-based loops
}

// ✅ Consider: Switch expressions for multiple conditions
string GetPriority(int level) => level switch
{
    1 => "Low",
    2 => "Medium", 
    3 => "High",
    _ => "Unknown"
};
```

### Common Pitfalls
```csharp
// ❌ Avoid: Infinite loops
while (true)
{
    // Always ensure there's a break condition
    if (someCondition)
        break;
}

// ❌ Avoid: Complex nested conditions
if (condition1)
{
    if (condition2)
    {
        if (condition3)
        {
            // Too deeply nested
        }
    }
}

// ✅ Better: Guard clauses or early returns
if (!condition1) return;
if (!condition2) return;
if (!condition3) return;

// Process here

// ❌ Avoid: Unnecessary goto usage
goto skipProcessing; // Usually indicates poor design

// ✅ Better: Structured control flow
if (shouldProcess)
{
    ProcessData();
}
```

### Modern C# Features
```csharp
// ✅ Use pattern matching in switch expressions
public decimal CalculateDiscount(Customer customer) => customer switch
{
    { IsPremium: true, YearsActive: > 5 } => 0.20m,
    { IsPremium: true } => 0.15m,
    { YearsActive: > 3 } => 0.10m,
    _ => 0.05m
};

// ✅ Use target-typed new expressions (C# 9.0+)
List<string> names = new(); // Type inferred
Dictionary<string, int> scores = new();

// ✅ Use using declarations for cleaner code
public void ProcessFile(string path)
{
    using var reader = new StreamReader(path);
    // File automatically closed at end of method
    var content = reader.ReadToEnd();
    ProcessContent(content);
}
```

## 🎓 Best Practices & Guidelines

### 1. Clear and Readable Control Flow
```csharp
// ✅ Use meaningful variable names in loops
foreach (var customer in activeCustomers)
{
    ProcessCustomerOrder(customer);
}

// ✅ Keep loop bodies simple
foreach (var item in items)
{
    if (ShouldProcess(item))
    {
        ProcessItem(item);
    }
}

// ✅ Use early returns for validation
public void ProcessOrder(Order order)
{
    if (order == null)
    {
        throw new ArgumentNullException(nameof(order));
    }
    
    if (!order.IsValid)
    {
        return; // Early return for invalid orders
    }
    
    // Main processing logic here
    ExecuteOrderProcessing(order);
}
```

### 2. Exception Handling Best Practices
```csharp
// ✅ Catch specific exceptions first
try
{
    ProcessPayment(order);
}
catch (InsufficientFundsException ex)
{
    Logger.LogWarning($"Insufficient funds for order {order.Id}");
    NotifyCustomer(order.Customer, "Payment failed: insufficient funds");
}
catch (PaymentGatewayException ex)
{
    Logger.LogError(ex, $"Payment gateway error for order {order.Id}");
    ScheduleRetry(order);
}
catch (Exception ex)
{
    Logger.LogError(ex, $"Unexpected error processing order {order.Id}");
    throw; // Re-throw unexpected exceptions
}

// ✅ Use using statements for resource management
public async Task<string> ReadFileAsync(string path)
{
    using var reader = new StreamReader(path);
    return await reader.ReadToEndAsync();
}
```

### 3. Switch Statement Modernization
```csharp
// ✅ Use switch expressions for simple mappings
public string GetStatusMessage(OrderStatus status) => status switch
{
    OrderStatus.Pending => "Your order is being processed",
    OrderStatus.Shipped => "Your order is on its way",
    OrderStatus.Delivered => "Your order has been delivered",
    OrderStatus.Cancelled => "Your order has been cancelled",
    _ => "Unknown status"
};

// ✅ Use pattern matching for complex conditions
public decimal CalculateShipping(Package package) => package switch
{
    { Weight: <= 1, IsExpress: true } => 15.00m,
    { Weight: <= 1, IsExpress: false } => 5.00m,
    { Weight: <= 5, IsExpress: true } => 25.00m,
    { Weight: <= 5, IsExpress: false } => 10.00m,
    _ => 20.00m + (package.Weight * 2.00m)
};
```

## 🔧 Real-World Applications

### 1. Data Processing Pipeline
```csharp
public class DataProcessor
{
    public async Task ProcessDataBatch(IEnumerable<DataRecord> records)
    {
        var successCount = 0;
        var errorCount = 0;
        
        foreach (var record in records)
        {
            try
            {
                // Validation
                if (!ValidateRecord(record))
                {
                    continue; // Skip invalid records
                }
                
                // Transform data
                var transformedData = TransformRecord(record);
                
                // Save to database
                await SaveRecord(transformedData);
                successCount++;
                
                // Break if we've processed enough
                if (successCount >= MaxBatchSize)
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                errorCount++;
                LogError(ex, record);
                
                // Stop processing if too many errors
                if (errorCount > MaxErrorThreshold)
                {
                    throw new BatchProcessingException($"Too many errors: {errorCount}");
                }
            }
        }
        
        Console.WriteLine($"Processed: {successCount}, Errors: {errorCount}");
    }
}
```

### 2. State Machine Implementation
```csharp
public class OrderStateMachine
{
    public void ProcessOrder(Order order, OrderEvent orderEvent)
    {
        var newStatus = (order.Status, orderEvent) switch
        {
            (OrderStatus.Created, OrderEvent.PaymentReceived) => OrderStatus.Paid,
            (OrderStatus.Paid, OrderEvent.InventoryAllocated) => OrderStatus.Confirmed,
            (OrderStatus.Confirmed, OrderEvent.Shipped) => OrderStatus.Shipped,
            (OrderStatus.Shipped, OrderEvent.Delivered) => OrderStatus.Delivered,
            (_, OrderEvent.Cancelled) => OrderStatus.Cancelled,
            _ => order.Status // No state change
        };
        
        if (newStatus != order.Status)
        {
            UpdateOrderStatus(order, newStatus);
            NotifyStatusChange(order, newStatus);
        }
    }
}
```

### 3. Configuration Parser
```csharp
public class ConfigurationParser
{
    public Dictionary<string, object> ParseConfiguration(string[] configLines)
    {
        var config = new Dictionary<string, object>();
        
        for (int i = 0; i < configLines.Length; i++)
        {
            var line = configLines[i].Trim();
            
            // Skip empty lines and comments
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
            {
                continue;
            }
            
            // Parse key-value pairs
            var parts = line.Split('=', 2);
            if (parts.Length != 2)
            {
                Console.WriteLine($"Skipping invalid line {i + 1}: {line}");
                continue;
            }
            
            var key = parts[0].Trim();
            var value = parts[1].Trim();
            
            // Type inference based on value format
            object parsedValue = value switch
            {
                var v when int.TryParse(v, out var intVal) => intVal,
                var v when bool.TryParse(v, out var boolVal) => boolVal,
                var v when double.TryParse(v, out var doubleVal) => doubleVal,
                _ => value // Default to string
            };
            
            config[key] = parsedValue;
        }
        
        return config;
    }
}
```

## 🎯 Mastery Checklist

### Fundamental Level
- [ ] Write basic variable declarations and assignments
- [ ] Use if-else statements for simple conditions
- [ ] Implement for and foreach loops
- [ ] Apply break and continue in loops

### Intermediate Level
- [ ] Master switch statements and expressions
- [ ] Implement try-catch-finally blocks
- [ ] Use using statements for resource management
- [ ] Apply pattern matching in switch expressions

### Advanced Level
- [ ] Design complex control flow with multiple statement types
- [ ] Implement state machines with switch expressions
- [ ] Use advanced pattern matching features
- [ ] Optimize statement performance in critical paths

### Expert Level
- [ ] Create domain-specific statement patterns
- [ ] Design statement-based DSLs (Domain Specific Languages)
- [ ] Implement advanced exception handling strategies
- [ ] Build high-performance statement execution engines

## 💼 Industry Applications

### Software Development
- **Control Flow Logic**: Core of all application decision-making
- **Data Processing**: ETL pipelines and batch processing
- **State Management**: State machines and workflow engines
- **Error Handling**: Robust application error management

### System Programming
- **Resource Management**: File, network, and memory handling
- **Performance Optimization**: Efficient loop and condition design
- **Configuration Processing**: System setting management
- **Event Processing**: Real-time event handling systems

### Algorithm Implementation
- **Search Algorithms**: Conditional and iterative logic
- **Sorting Algorithms**: Complex loop and swap logic
- **Graph Traversal**: Recursive and iterative approaches
- **Dynamic Programming**: Optimized conditional execution

## 🔗 Integration with Other Concepts

### C# Language Features
- **LINQ**: Statement-like query syntax
- **Async/Await**: Asynchronous statement execution
- **Pattern Matching**: Advanced conditional logic
- **Local Functions**: Statement-scoped function definitions

### .NET Framework
- **Collections**: Statement-based iteration patterns
- **Streams**: Using statements for resource management
- **Threading**: Synchronization statements
- **Reflection**: Dynamic statement generation

### Design Patterns
- **Strategy Pattern**: Switch-based algorithm selection
- **State Pattern**: Statement-based state transitions
- **Command Pattern**: Statement encapsulation
- **Template Method**: Statement sequence templates

---

*Master C# statements to build the foundation for all program logic and control flow. These constructs are the building blocks that transform simple instructions into powerful, intelligent applications.*
