# C# Statements

This project provides a comprehensive exploration of statements in C#, which are the fundamental building blocks that control program execution flow. Statements define the actions that a program performs and the order in which those actions occur.

## Objectives

This demonstration covers the various types of statements available in C#, showing how to control program flow, make decisions, handle repetition, and manage resources effectively.

## Core Concepts

The following essential topics are covered in this project with comprehensive explanations and practical examples:

### 1. Declaration Statements

Declaration statements establish storage locations for data and define constants within your program. These statements form the foundation of variable management and scope control.

**Variable Declarations:**
Variable declarations create named storage locations that can hold values of a specific type. The general syntax combines type specification, identifier naming, and optional initialization.

```csharp
// Basic variable declarations with explicit types
string customerName = "John Doe";        // String variable with initial value
int orderQuantity = 5;                   // Integer variable initialized
double unitPrice = 29.99;                // Double-precision floating point
bool isProcessed = false;                // Boolean variable

// Multiple variables of the same type
int x = 10, y = 20, z = 30;            // Three integers in one statement
bool isActive = true, isVerified = false; // Two booleans in one statement
```

**Constant Declarations:**
Constants are immutable named values that cannot be changed after declaration. They provide a way to define fixed values that remain constant throughout program execution.

```csharp
// Constants for configuration values
const double PI = 3.14159265359;         // Mathematical constant
const int MAX_RETRY_ATTEMPTS = 3;        // Configuration constant
const string APPLICATION_NAME = "OrderProcessor"; // String constant

// Constants improve code readability and maintainability
if (attempts >= MAX_RETRY_ATTEMPTS)
{
    throw new InvalidOperationException("Maximum retries exceeded");
}
```

**Local Variable Scope:**
Scope determines where declared variables can be accessed within your code. C# uses block-level scoping, where variables are accessible only within the code block where they are declared.

```csharp
public void DemonstrateScope()
{
    int outerVariable = 100;  // Accessible throughout the method
    
    if (someCondition)
    {
        int innerVariable = 200;  // Only accessible within this block
        Console.WriteLine(outerVariable);  // Can access outer variable
    }
    
    // innerVariable is not accessible here - would cause compile error
    Console.WriteLine(outerVariable);  // Still accessible
}
```

**Key Characteristics:**
- **Type Safety**: Variables must be declared with specific types
- **Initialization**: Best practice to initialize variables at declaration
- **Scope Rules**: Variables are accessible only within their declared scope
- **Constant Immutability**: Constants cannot be modified after declaration

### 2. Expression Statements

Expression statements perform actions that change the state of your program. They evaluate expressions and often produce side effects such as variable assignments, method calls, or object creation.

**Assignment Statements:**
Assignment statements store values in variables, properties, or fields. The assignment operator `=` evaluates the right-hand expression and stores the result in the left-hand location.

```csharp
// Basic assignments
int result = 10 + 5;           // Expression evaluation and assignment
string message = GetMessage(); // Method call result assignment

// Compound assignments
counter += 5;                  // Equivalent to: counter = counter + 5
balance *= 1.05;              // Equivalent to: balance = balance * 1.05
```

**Method Call Statements:**
Method calls execute functions or procedures, potentially with arguments, and may return values. Method calls can be used for their side effects or return values.

```csharp
// Method calls for side effects
Console.WriteLine("Processing order");     // Void method call
list.Add(newItem);                        // Collection modification

// Method calls with return values
string upperText = input.ToUpper();       // String transformation
bool success = TryProcessOrder(order);    // Boolean result
```

**Increment and Decrement Operations:**
These operators provide convenient ways to increase or decrease numeric values by one. They come in prefix and postfix variations with different evaluation timing.

```csharp
int counter = 5;

// Postfix increment: use current value, then increment
int current = counter++;  // current = 5, counter becomes 6

// Prefix increment: increment first, then use new value
int next = ++counter;     // counter becomes 7, next = 7

// Same pattern applies to decrement
int before = counter--;   // before = 7, counter becomes 6
int after = --counter;    // counter becomes 5, after = 5
```

**Object Creation Statements:**
Object instantiation creates new instances of classes or structures using constructors. These statements allocate memory and initialize object state.

```csharp
// Constructor calls with assignment
StringBuilder builder = new StringBuilder();
List<string> names = new List<string>();

// Constructor calls with parameters
DateTime appointmentTime = new DateTime(2024, 12, 25);
Customer customer = new Customer("John Doe", "john@example.com");

// Object creation without assignment (immediate use)
new StringBuilder().Append("Hello").Append(" World").ToString();
```

### 3. Selection Statements

Selection statements enable conditional execution of code blocks based on Boolean expressions. They form the decision-making logic of programs, allowing different execution paths based on runtime conditions.

**If Statements:**
The fundamental conditional statement executes code blocks when Boolean expressions evaluate to true. If statements provide the basis for all conditional logic.

```csharp
// Basic if statement
if (temperature > 30)
{
    Console.WriteLine("It's hot today!");
    airConditioner.TurnOn();
}

// If-else for binary decisions
if (user.IsAuthenticated)
{
    DisplayDashboard();
}
else
{
    RedirectToLogin();
}
```

**If-Else Chains:**
Multiple if-else statements can be chained to handle mutually exclusive conditions. Each condition is evaluated in sequence until one matches or the final else is reached.

```csharp
// Grade calculation with if-else chain
if (score >= 90)
{
    grade = "A";
    Console.WriteLine("Excellent performance!");
}
else if (score >= 80)
{
    grade = "B";
    Console.WriteLine("Good work!");
}
else if (score >= 70)
{
    grade = "C";
    Console.WriteLine("Satisfactory performance.");
}
else if (score >= 60)
{
    grade = "D";
    Console.WriteLine("Needs improvement.");
}
else
{
    grade = "F";
    Console.WriteLine("Failing grade - please retake.");
}
```

**Switch Statements:**
Switch statements provide efficient multi-way branching when comparing a single expression against multiple constant values. They offer better performance than long if-else chains for many conditions.

```csharp
// Traditional switch statement
switch (dayOfWeek)
{
    case DayOfWeek.Monday:
        Console.WriteLine("Start of work week");
        break;
    case DayOfWeek.Tuesday:
    case DayOfWeek.Wednesday:
    case DayOfWeek.Thursday:
        Console.WriteLine("Midweek days");
        break;
    case DayOfWeek.Friday:
        Console.WriteLine("TGIF!");
        break;
    case DayOfWeek.Saturday:
    case DayOfWeek.Sunday:
        Console.WriteLine("Weekend!");
        break;
    default:
        Console.WriteLine("Unknown day");
        break;
}
```

**Switch Expressions (Modern C#):**
Switch expressions provide a concise syntax for value-based selection, introduced in C# 8.0. They return values directly and use pattern matching.

```csharp
// Switch expression for value calculation
decimal shippingCost = package.Weight switch
{
    <= 1.0 => 5.00m,
    <= 5.0 => 10.00m,
    <= 10.0 => 15.00m,
    _ => 20.00m
};

// Switch expression with pattern matching
string describeValue = input switch
{
    int i when i > 0 => "Positive integer",
    int i when i < 0 => "Negative integer",
    int => "Zero",
    string s when s.Length > 10 => "Long string",
    string s => $"Short string: {s}",
    null => "Null value",
    _ => "Unknown type"
};
```

### 4. Iteration Statements

Iteration statements enable repeated execution of code blocks, forming the foundation for processing collections, implementing algorithms, and handling repetitive tasks efficiently.

**For Loops:**
For loops provide counter-based iteration with explicit initialization, condition checking, and increment/decrement operations. They offer precise control over iteration parameters.

```csharp
// Basic for loop structure
for (int i = 0; i < 10; i++)
{
    Console.WriteLine($"Iteration: {i}");
    ProcessItem(i);
}

// For loop with custom increment
for (int i = 0; i < 100; i += 10)
{
    Console.WriteLine($"Value: {i}");  // Prints: 0, 10, 20, 30...
}

// Countdown loop
for (int countdown = 10; countdown > 0; countdown--)
{
    Console.WriteLine($"T-minus {countdown}");
}
Console.WriteLine("Launch!");
```

**While Loops:**
While loops continue execution as long as a specified condition remains true. They provide condition-based iteration where the number of iterations is not predetermined.

```csharp
// Basic while loop
int attempts = 0;
while (attempts < maxAttempts && !operationSuccessful)
{
    operationSuccessful = TryOperation();
    attempts++;
    
    if (!operationSuccessful)
    {
        Console.WriteLine($"Attempt {attempts} failed, retrying...");
        Thread.Sleep(1000);  // Wait before retry
    }
}

// Input validation loop
string userInput;
while (true)
{
    Console.Write("Enter a positive number: ");
    userInput = Console.ReadLine();
    
    if (int.TryParse(userInput, out int number) && number > 0)
    {
        Console.WriteLine($"Valid input: {number}");
        break;
    }
    
    Console.WriteLine("Invalid input. Please try again.");
}
```

**Do-While Loops:**
Do-while loops execute the code block at least once before checking the condition. They guarantee minimum execution and are useful for scenarios requiring initial execution.

```csharp
// Menu system with do-while
string choice;
do
{
    Console.WriteLine("\n=== MENU ===");
    Console.WriteLine("1. View Orders");
    Console.WriteLine("2. Create Order"); 
    Console.WriteLine("3. Exit");
    Console.Write("Select option: ");
    
    choice = Console.ReadLine();
    
    switch (choice)
    {
        case "1":
            DisplayOrders();
            break;
        case "2":
            CreateNewOrder();
            break;
        case "3":
            Console.WriteLine("Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
} while (choice != "3");
```

**Foreach Loops:**
Foreach loops provide simplified iteration over collections and enumerable sequences. They handle iteration mechanics automatically and improve code readability.

```csharp
// Basic foreach with arrays
string[] fruits = { "apple", "banana", "cherry", "date" };
foreach (string fruit in fruits)
{
    Console.WriteLine($"Processing: {fruit}");
    ProcessFruit(fruit);
}

// Foreach with collections
List<Customer> customers = GetCustomers();
foreach (Customer customer in customers)
{
    if (customer.IsActive)
    {
        SendNotification(customer);
    }
}

// Foreach with dictionaries
Dictionary<string, int> inventory = GetInventory();
foreach (KeyValuePair<string, int> item in inventory)
{
    Console.WriteLine($"{item.Key}: {item.Value} units");
}

// Simplified foreach with var keyword
foreach (var order in activeOrders)
{
    ProcessOrder(order);
    UpdateOrderStatus(order);
}
```

### 5. Jump Statements

Jump statements alter the normal sequential execution flow by transferring control to different parts of the program. They provide mechanisms for early exit, iteration control, and unconditional branching.

**Break Statements:**
Break statements immediately exit loops or switch statements, transferring control to the statement following the terminated construct.

```csharp
// Break in for loop - search operation
int[] numbers = { 5, 12, 8, 23, 7, 15 };
int target = 23;
int foundIndex = -1;

for (int i = 0; i < numbers.Length; i++)
{
    if (numbers[i] == target)
    {
        foundIndex = i;
        break;  // Exit loop immediately when found
    }
}

// Break in while loop - input processing
while (true)
{
    string input = Console.ReadLine();
    
    if (input == "quit")
    {
        break;  // Exit infinite loop
    }
    
    ProcessInput(input);
}
```

**Continue Statements:**
Continue statements skip the remaining code in the current iteration and jump to the next iteration of the enclosing loop.

```csharp
// Continue in for loop - skip even numbers
for (int i = 1; i <= 10; i++)
{
    if (i % 2 == 0)
    {
        continue;  // Skip even numbers
    }
    
    Console.WriteLine($"Odd number: {i}");  // Only prints 1, 3, 5, 7, 9
}

// Continue in foreach - process valid items only
foreach (var item in items)
{
    if (!item.IsValid)
    {
        LogError($"Invalid item: {item.Id}");
        continue;  // Skip to next item
    }
    
    ProcessValidItem(item);
}
```

**Return Statements:**
Return statements exit methods immediately, optionally returning a value to the caller. They provide early exit mechanisms and value communication.

```csharp
// Return with value
public int FindMaxValue(int[] numbers)
{
    if (numbers == null || numbers.Length == 0)
    {
        return -1;  // Early return for invalid input
    }
    
    int max = numbers[0];
    for (int i = 1; i < numbers.Length; i++)
    {
        if (numbers[i] > max)
        {
            max = numbers[i];
        }
    }
    
    return max;  // Return computed result
}

// Return without value (void method)
public void ProcessOrder(Order order)
{
    if (order == null)
    {
        LogError("Null order received");
        return;  // Early exit
    }
    
    if (!order.IsValid)
    {
        LogError($"Invalid order: {order.Id}");
        return;  // Early exit
    }
    
    // Main processing logic here
    ExecuteOrderProcessing(order);
}
```

**Goto Statements:**
Goto statements provide unconditional jumps to labeled statements. While powerful, they should be used sparingly as they can make code difficult to understand and maintain.

```csharp
// Goto with retry logic (use sparingly)
int retryCount = 0;

retryOperation:
try
{
    PerformRiskyOperation();
    Console.WriteLine("Operation successful");
}
catch (TemporaryException ex)
{
    retryCount++;
    if (retryCount < 3)
    {
        Console.WriteLine($"Retry attempt {retryCount}");
        Thread.Sleep(1000);
        goto retryOperation;  // Jump back to retry
    }
    
    Console.WriteLine("Maximum retries exceeded");
    throw;
}
```

### 6. Exception Handling Statements

Exception handling statements provide structured mechanisms for dealing with runtime errors and exceptional conditions. They enable graceful error recovery and resource cleanup.

**Try-Catch Blocks:**
Try-catch blocks isolate risky code and provide handlers for specific exception types. They prevent unhandled exceptions from crashing applications.

```csharp
// Basic try-catch structure
try
{
    string input = Console.ReadLine();
    int number = int.Parse(input);
    int result = 100 / number;
    Console.WriteLine($"Result: {result}");
}
catch (FormatException ex)
{
    Console.WriteLine($"Invalid number format: {ex.Message}");
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Cannot divide by zero: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

**Finally Blocks:**
Finally blocks contain code that executes regardless of whether exceptions occur. They ensure cleanup operations are performed even when exceptions are thrown.

```csharp
// Try-catch-finally with resource cleanup
FileStream fileStream = null;
try
{
    fileStream = new FileStream("data.txt", FileMode.Open);
    // File operations here
    ProcessFile(fileStream);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"File not found: {ex.Message}");
}
catch (IOException ex)
{
    Console.WriteLine($"I/O error: {ex.Message}");
}
finally
{
    // Cleanup always executes
    fileStream?.Close();
    Console.WriteLine("File handling completed");
}
```

**Using Statements:**
Using statements provide automatic resource disposal for objects implementing `IDisposable`. They ensure proper cleanup without explicit finally blocks.

```csharp
// Using statement for automatic disposal
using (var fileReader = new StreamReader("config.txt"))
{
    string content = fileReader.ReadToEnd();
    ProcessConfiguration(content);
}  // StreamReader automatically disposed here

// Using declaration (C# 8.0+)
public void ProcessDataFile(string filename)
{
    using var reader = new StreamReader(filename);
    using var writer = new StreamWriter("output.txt");
    
    string line;
    while ((line = reader.ReadLine()) != null)
    {
        writer.WriteLine(ProcessLine(line));
    }
}  // Both reader and writer disposed at method end
```

**Throw Statements:**
Throw statements explicitly raise exceptions, either new exceptions or re-throwing caught exceptions. They enable custom error signaling and exception propagation.

```csharp
// Throwing custom exceptions
public void ValidateAge(int age)
{
    if (age < 0)
    {
        throw new ArgumentException("Age cannot be negative", nameof(age));
    }
    
    if (age > 150)
    {
        throw new ArgumentException("Age seems unrealistic", nameof(age));
    }
}

// Re-throwing exceptions
try
{
    PerformOperation();
}
catch (SpecificException ex)
{
    LogError(ex);
    throw;  // Re-throw original exception
}
```

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
### **Pillar 4: Resource Management Excellence**
```csharp
// Using statements for automatic cleanup
using (var fileStream = new FileStream("data.txt", FileMode.Open))
{
    // File processing logic
    ProcessFile(fileStream);
} // Automatically disposed here

// Using declarations (C# 8.0+)
using var connection = new SqlConnection(connectionString);
using var command = new SqlCommand(query, connection);
// Both disposed at end of scope

// Exception handling with proper cleanup
try
{
    PerformRiskyOperation();
}
catch (SpecificException ex)
{
    LogError(ex);
    HandleError(ex);
}
finally
{
    CleanupResources(); // Always executes
}
```

### **Pillar 5: Smart Jump Control**
```csharp
// Strategic use of break for early exit
foreach (var item in largeCollection)
{
    if (ProcessItem(item) && ShouldStopProcessing())
    {
        break; // Exit when condition met
    }
}

// Continue for filtering
foreach (var record in records)
{
    if (!record.IsValid)
    {
        LogInvalidRecord(record);
        continue; // Skip invalid records
    }
    
    ProcessValidRecord(record);
}

// Early returns for validation
public bool ValidateInput(string input)
{
    if (string.IsNullOrEmpty(input)) return false;
    if (input.Length < 3) return false;
    if (ContainsInvalidCharacters(input)) return false;
    
    return true; // All validations passed
}
```

### **Pillar 6: Exception Handling Mastery**
```csharp
// Specific exception handling
public void ProcessPayment(Payment payment)
{
    try
    {
        ValidatePayment(payment);
        ProcessTransaction(payment);
    }
    catch (InsufficientFundsException ex)
    {
        NotifyCustomer("Insufficient funds");
        LogError(ex);
    }
    catch (PaymentGatewayException ex)
    {
        ScheduleRetry(payment);
        LogError(ex);
    }
    catch (Exception ex)
    {
        LogCriticalError(ex);
        throw; // Re-throw unexpected exceptions
    }
}
```

## Comprehensive Code Examples

### Declaration Statements in Practice
```csharp
public class OrderProcessor
{
    // Field declarations at class level
    private readonly ILogger _logger;
    private const int MAX_BATCH_SIZE = 1000;
    
    public void ProcessOrders()
    {
        // Local variable declarations
        int processedCount = 0;              // Counter initialization
        var startTime = DateTime.Now;        // Type inference with var
        bool hasErrors = false;              // Boolean flag
        
        // Multiple declarations of same type
        string errorMessage = null, successMessage = "Processing complete";
        
        // Constant for local use
        const double TAX_RATE = 0.08;
        
        // Demonstrate scope
        if (processedCount == 0)
        {
            string statusMessage = "Starting processing"; // Block scope
            _logger.LogInformation(statusMessage);
        }
        // statusMessage not accessible here
    }
}
```

### Selection Statements in Practice
```csharp
public class CustomerService
{
    public string DetermineCustomerTier(Customer customer)
    {
        // Complex if-else chain
        if (customer.YearsActive >= 10 && customer.TotalSpent >= 50000)
        {
            return "Platinum";
        }
        else if (customer.YearsActive >= 5 && customer.TotalSpent >= 20000)
        {
            return "Gold";
        }
        else if (customer.YearsActive >= 2 && customer.TotalSpent >= 5000)
        {
            return "Silver";
        }
        else
        {
            return "Bronze";
        }
    }
    
    public decimal CalculateDiscount(Customer customer)
    {
        // Modern switch expression with pattern matching
        return customer.Tier switch
        {
            "Platinum" => 0.20m,
            "Gold" => 0.15m,
            "Silver" => 0.10m,
            "Bronze" => 0.05m,
            _ => 0.00m
        };
    }
    
    public string GetShippingMethod(Order order)
    {
        // Switch statement with multiple cases
        switch (order.Priority)
        {
            case OrderPriority.Express:
            case OrderPriority.Overnight:
                return "Express Shipping";
                
            case OrderPriority.Standard:
                return order.Weight > 10 ? "Ground Freight" : "Standard Shipping";
                
            case OrderPriority.Economy:
                return "Economy Ground";
                
            default:
                throw new ArgumentException($"Unknown priority: {order.Priority}");
        }
    }
}

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

## Mastery Checklist

Before considering yourself proficient with C# statements, ensure you can:

### Core Competencies
- [ ] Declare variables with appropriate types and scope
- [ ] Use constants effectively to improve code maintainability
- [ ] Implement conditional logic using if-else and switch statements
- [ ] Choose appropriate loop types for different iteration scenarios
- [ ] Apply jump statements (break, continue, return) strategically
- [ ] Handle exceptions with try-catch-finally blocks
- [ ] Manage resources properly using using statements

### Advanced Applications
- [ ] Design complex control flow for business logic
- [ ] Optimize performance through proper statement selection
- [ ] Implement error handling strategies for robust applications
- [ ] Use modern C# features like switch expressions and pattern matching
- [ ] Apply defensive programming techniques
- [ ] Debug control flow issues effectively

### Professional Development
- [ ] Write clean, readable control structures
- [ ] Follow industry best practices for statement usage
- [ ] Design maintainable code with proper separation of concerns
- [ ] Handle edge cases and error conditions appropriately
- [ ] Document complex control flow logic clearly

## Summary

C# statements form the fundamental building blocks of program execution, providing the mechanisms for variable management, decision making, repetition, flow control, and error handling. Understanding these constructs is essential for writing effective, maintainable applications.

### Key Takeaways

**Declaration statements** establish the data foundation of your programs through variables and constants, managing scope and lifetime effectively.

**Expression statements** perform the work of your application through assignments, method calls, and object creation, changing program state through well-defined operations.

**Selection statements** implement decision-making logic through if-else chains and switch constructs, enabling programs to respond intelligently to different conditions and inputs.

**Iteration statements** handle repetitive tasks efficiently through for, while, do-while, and foreach loops, providing the mechanisms for processing collections and implementing algorithms.

**Jump statements** offer precise control over execution flow through break, continue, return, and goto, enabling early exits and specialized control patterns.

**Exception handling statements** ensure robust error management through try-catch-finally blocks and using statements, providing graceful recovery from exceptional conditions.

### Professional Impact

Mastering C# statements leads to:
- **Cleaner Code Architecture**: Well-structured control flow improves code organization
- **Better Performance**: Appropriate statement selection optimizes execution efficiency  
- **Enhanced Reliability**: Proper error handling prevents application crashes
- **Improved Maintainability**: Clear control structures make code easier to understand and modify
- **Reduced Debugging Time**: Systematic statement usage reduces logical errors

### Next Steps

To continue advancing your C# expertise:
1. **Practice Complex Scenarios**: Implement real-world business logic using combined statement types
2. **Performance Analysis**: Study the performance implications of different statement patterns
3. **Modern Features**: Explore pattern matching, switch expressions, and other contemporary C# features
4. **Code Review Focus**: Analyze statement usage in existing codebases for improvement opportunities
5. **Advanced Topics**: Study async/await patterns, LINQ query syntax, and advanced exception handling strategies

The mastery of statements provides the foundation for all advanced C# programming concepts, enabling you to build sophisticated, reliable, and maintainable applications.

---

