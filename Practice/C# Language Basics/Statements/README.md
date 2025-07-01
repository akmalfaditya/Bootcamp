# Statements in C#

Welcome to the control center of C# programming! Statements are the building blocks that tell your program exactly what to do and when to do it. Think of them as the instructions that transform your ideas into executable code.

## Why Statements Are Your Programming Foundation

**The fundamental truth**: Every meaningful program is a sequence of statements. Whether you're storing data, making decisions, repeating tasks, or handling errors - it all comes down to writing the right statements in the right order.

**Professional perspective**: Understanding statements deeply is what separates developers who can follow tutorials from those who can solve real problems. When you master statements, you master the art of program control flow.

## Your Learning Journey

Our comprehensive demonstration takes you through each type of statement with practical examples and real-world context:

### 1. **Declaration Statements** - Creating Your Data Foundation
Learn to create variables and constants that store your program's data. This is where all programming begins - you need somewhere to put your information before you can work with it.

**Key Insight**: Good variable declaration habits make your code readable and maintainable. Poor habits create bugs and confusion.

### 2. **Expression Statements** - Making Things Happen
Discover how expressions become statements that actually change your program's state. This is where calculations happen, assignments occur, and methods get called.

**Key Insight**: Expression statements are your action verbs - they're what make your program DO something rather than just define something.

### 3. **Selection Statements** - The Art of Decision Making
Master if statements and switch statements to create programs that respond intelligently to different conditions. This is where your program becomes smart.

**Key Insight**: Good selection logic makes programs that feel intelligent and responsive. Poor selection logic creates confusing, unpredictable behavior.

### 4. **Iteration Statements** - Embracing Repetition
Learn the various loop types to handle repetitive tasks efficiently. This is where you solve problems that involve multiple items or repeated actions.

**Key Insight**: Choosing the right loop type makes your code clearer and often more efficient. The wrong choice makes it harder to understand and maintain.

### 5. **Jump Statements** - Precise Flow Control
Understand how to break, continue, return, and redirect program flow with surgical precision. These are your escape hatches and shortcuts.

**Key Insight**: Jump statements are powerful but should be used thoughtfully. They can make code more efficient or more confusing, depending on how you use them.

### 6. **Miscellaneous Statements** - Professional Resource Management
Explore using statements and exception handling - the tools that make your code robust and resource-aware.

**Key Insight**: Professional code handles resources properly and recovers gracefully from errors. This is what separates hobby projects from production software.

## The Six Pillars of Statement Mastery

### **Pillar 1: Variable Declaration Strategy**
```csharp
// Strategic declaration - clear intent and scope
string customerName = "John Doe";    // Descriptive names
const int MAX_RETRIES = 3;           // Constants for magic numbers
int x = 0, y = 0;                    // Group related variables

// Proper scoping
{
    int temporaryValue = calculation(); // Limited scope when appropriate
    ProcessValue(temporaryValue);
}
// temporaryValue is no longer accessible here
```

### **Pillar 2: Smart Selection Logic**
```csharp
// Traditional if-else for complex logic
if (age >= 65)
{
    discount = seniorDiscount;
}
else if (age >= 18)
{
    discount = adultDiscount;
}
else
{
    discount = 0; // Minors don't get discounts
}

// Modern switch expressions for simple mappings
string priority = urgencyLevel switch
{
    1 => "Low",
    2 => "Medium", 
    3 => "High",
    _ => "Unknown"
};
```

### **Pillar 3: Efficient Iteration Patterns**
```csharp
// Use foreach for collections when you need every item
foreach (var customer in customers)
{
    ProcessCustomer(customer);
}

// Use for loops when you need precise control
for (int i = 0; i < maxAttempts; i++)
{
    if (TryOperation())
        break; // Exit early on success
}

// Use while for condition-based repetition
while (!IsComplete() && attempts < maxAttempts)
{
    AttemptOperation();
    attempts++;
}
```
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

## Tips

### Performance Considerations
- **Loop optimization**: Use appropriate loop types for different scenarios
- **Early returns**: Use return statements to avoid unnecessary processing
- **Switch vs if-else**: Switch expressions are often more performant for multiple conditions

```csharp
// Efficient: Early return pattern
public bool ValidateUser(User user)
{
    if (user == null) return false;
    if (string.IsNullOrEmpty(user.Name)) return false;
    if (string.IsNullOrEmpty(user.Email)) return false;
    
    return IsValidEmail(user.Email);
}

// Efficient: Use foreach for collections
foreach (var item in collection)
{
    // More readable and often faster than index-based loops
}

// Consider: Switch expressions for multiple conditions
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
// Avoid: Infinite loops
while (true)
{
    // Always ensure there's a break condition
    if (someCondition)
        break;
}

// Avoid: Complex nested conditions
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

// Better: Guard clauses or early returns
if (!condition1) return;
if (!condition2) return;
if (!condition3) return;

// Process here

// Avoid: Unnecessary goto usage
goto skipProcessing; // Usually indicates poor design

// Better: Structured control flow
if (shouldProcess)
{
    ProcessData();
}
```

### Modern C# Features
```csharp
// Use pattern matching in switch expressions
public decimal CalculateDiscount(Customer customer) => customer switch
{
    { IsPremium: true, YearsActive: > 5 } => 0.20m,
    { IsPremium: true } => 0.15m,
    { YearsActive: > 3 } => 0.10m,
    _ => 0.05m
};

// Use target-typed new expressions (C# 9.0+)
List<string> names = new(); // Type inferred
Dictionary<string, int> scores = new();

// Use using declarations for cleaner code
public void ProcessFile(string path)
{
    using var reader = new StreamReader(path);
    // File automatically closed at end of method
    var content = reader.ReadToEnd();
    ProcessContent(content);
}
```

## Best Practices & Guidelines

### 1. Clear and Readable Control Flow
```csharp
// Use meaningful variable names in loops
foreach (var customer in activeCustomers)
{
    ProcessCustomerOrder(customer);
}

// Keep loop bodies simple
foreach (var item in items)
{
    if (ShouldProcess(item))
    {
        ProcessItem(item);
    }
}

// Use early returns for validation
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
// Catch specific exceptions first
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

// Use using statements for resource management
public async Task<string> ReadFileAsync(string path)
{
    using var reader = new StreamReader(path);
    return await reader.ReadToEndAsync();
}
```

### 3. Switch Statement Modernization
```csharp
// Use switch expressions for simple mappings
public string GetStatusMessage(OrderStatus status) => status switch
{
    OrderStatus.Pending => "Your order is being processed",
    OrderStatus.Shipped => "Your order is on its way",
    OrderStatus.Delivered => "Your order has been delivered",
    OrderStatus.Cancelled => "Your order has been cancelled",
    _ => "Unknown status"
};

// Use pattern matching for complex conditions
public decimal CalculateShipping(Package package) => package switch
{
    { Weight: <= 1, IsExpress: true } => 15.00m,
    { Weight: <= 1, IsExpress: false } => 5.00m,
    { Weight: <= 5, IsExpress: true } => 25.00m,
    { Weight: <= 5, IsExpress: false } => 10.00m,
    _ => 20.00m + (package.Weight * 2.00m)
};
```

## Real-World Applications

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

## Industry Applications

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

