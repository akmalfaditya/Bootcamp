# Try Statements and Exceptions in C#

## Learning Objectives

Exception handling is crucial for building robust applications that can recover from errors, provide meaningful feedback to users, and maintain system stability.

## What You'll Learn

### Core Concepts Covered:

1. **Exception Fundamentals**
   - What exceptions are and when they occur
   - Exception hierarchy in .NET
   - The cost of exceptions and when to use them
   - Exception vs return codes comparison

2. **Try-Catch-Finally Structure**
   - **`try` block**: Code that might throw exceptions
   - **`catch` block**: Exception handling logic
   - **`finally` block**: Cleanup code that always runs
   - **Multiple catch blocks**: Handling different exception types

3. **Exception Types and Hierarchy**
   - **System.Exception**: Base class for all exceptions
   - **SystemException**: Runtime errors
   - **ApplicationException**: Application-specific errors
   - **Custom exceptions**: Creating your own exception types

4. **Advanced Exception Handling**
   - **Exception filters**: `when` clause for conditional catching
   - **Rethrowing exceptions**: Preserving stack traces
   - **Exception properties**: Message, StackTrace, InnerException
   - **Aggregated exceptions**: Handling multiple exceptions

5. **Modern Exception Patterns**
   - **`using` statements**: Automatic resource disposal
   - **Try-parse patterns**: Safe conversion without exceptions
   - **Result pattern**: Returning success/failure without exceptions
   - **Exception guidelines**: When to throw, when to catch

## Key Features Demonstrated

### Basic Try-Catch Pattern:
```csharp
try
{
    // Code that might throw an exception
    int result = DivideNumbers(10, 0);
    Console.WriteLine($"Result: {result}");
}
catch (DivideByZeroException ex)
{
    // Handle specific exception type
    Console.WriteLine($"Cannot divide by zero: {ex.Message}");
}
catch (Exception ex)
{
    // Handle any other exception
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
finally
{
    // Cleanup code that always runs
    Console.WriteLine("Operation completed");
}
```

### Multiple Catch Blocks:
```csharp
try
{
    ProcessUserInput(userInput);
}
catch (ArgumentNullException ex)
{
    LogError("Null argument provided", ex);
    ShowUserError("Please provide valid input");
}
catch (ArgumentException ex)
{
    LogError("Invalid argument", ex);
    ShowUserError("Input format is incorrect");
}
catch (InvalidOperationException ex)
{
    LogError("Invalid operation", ex);
    ShowUserError("Operation not allowed in current state");
}
catch (Exception ex)
{
    LogError("Unexpected error", ex);
    ShowUserError("An unexpected error occurred");
}
```

### Exception Filters (C# 6+):
```csharp
try
{
    ProcessWebRequest(url);
}
catch (HttpException ex) when (ex.StatusCode == 404)
{
    // Handle not found specifically
    ShowNotFoundMessage();
}
catch (HttpException ex) when (ex.StatusCode >= 500)
{
    // Handle server errors
    RetryOperation();
}
catch (HttpException ex)
{
    // Handle other HTTP errors
    LogHttpError(ex);
}
```

### Custom Exception Types:
```csharp
// Custom exception for business logic violations
public class InsufficientFundsException : Exception
{
    public decimal AttemptedAmount { get; }
    public decimal AvailableBalance { get; }
    
    public InsufficientFundsException(decimal attempted, decimal available)
        : base($"Insufficient funds. Attempted: {attempted:C}, Available: {available:C}")
    {
        AttemptedAmount = attempted;
        AvailableBalance = available;
    }
}

// Usage in business logic
public void WithdrawMoney(decimal amount)
{
    if (amount > _balance)
    {
        throw new InsufficientFundsException(amount, _balance);
    }
    _balance -= amount;
}
```

### Using Statements for Resource Management:
```csharp
// Traditional try-finally
FileStream file = null;
try
{
    file = new FileStream("data.txt", FileMode.Open);
    // Use file
}
finally
{
    file?.Dispose();
}

// Modern using statement
using (var file = new FileStream("data.txt", FileMode.Open))
{
    // Use file - automatically disposed
}

// C# 8+ using declaration
using var file = new FileStream("data.txt", FileMode.Open);
// File automatically disposed at end of scope
```

### Try-Parse Pattern (Exception-Free):
```csharp
// Instead of exception-prone parsing
try
{
    int number = int.Parse(userInput);
    ProcessNumber(number);
}
catch (FormatException)
{
    ShowInvalidInputMessage();
}

// Use try-parse pattern
if (int.TryParse(userInput, out int number))
{
    ProcessNumber(number);
}
else
{
    ShowInvalidInputMessage();
}
```

## Tips

> **Exception Performance**: Exceptions are expensive! They're designed for exceptional circumstances, not regular control flow. Use TryParse methods for expected failures like user input validation.

> **Catch Specific Types**: Always catch the most specific exception types first, then more general ones. The order matters - C# checks catch blocks from top to bottom.

> **Preserve Stack Traces**: Use `throw;` (not `throw ex;`) to rethrow exceptions and preserve the original stack trace for better debugging.

## What to Focus On

1. **When to use exceptions**: Exceptional conditions vs expected failures
2. **Exception hierarchy**: Understanding inheritance relationships
3. **Resource cleanup**: Using `finally` and `using` statements properly
4. **Exception safety**: Writing code that handles errors gracefully

## Run the Project

```bash
dotnet run
```

The demo includes:
- Basic try-catch-finally patterns
- Multiple exception type handling
- Exception filters and conditional catching
- Custom exception creation
- Resource management patterns
- Real-world exception scenarios

## Best Practices

1. **Catch specific exceptions** rather than generic `Exception`
2. **Use `finally` or `using`** for cleanup that must happen
3. **Don't catch and ignore** - always log or handle appropriately
4. **Throw meaningful exceptions** with descriptive messages
5. **Use TryParse methods** for expected conversion failures
6. **Document exceptions** in XML comments for public APIs
7. **Fail fast** - don't hide or mask errors unnecessarily

## Real-World Applications

### File Operations:
```csharp
public async Task<string> ReadFileContentAsync(string filePath)
{
    try
    {
        using var reader = new StreamReader(filePath);
        return await reader.ReadToEndAsync();
    }
    catch (FileNotFoundException)
    {
        // Return default content or create file
        return string.Empty;
    }
    catch (UnauthorizedAccessException)
    {
        throw new ApplicationException($"Access denied to file: {filePath}");
    }
    catch (IOException ex)
    {
        throw new ApplicationException($"Error reading file: {filePath}", ex);
    }
}
```

### Database Operations:
```csharp
public async Task<User> GetUserAsync(int userId)
{
    try
    {
        return await _dbContext.Users.FindAsync(userId);
    }
    catch (SqlException ex) when (ex.Number == 2) // Timeout
    {
        throw new TimeoutException("Database operation timed out", ex);
    }
    catch (SqlException ex) when (ex.Number == 18456) // Login failed
    {
        throw new UnauthorizedAccessException("Database access denied", ex);
    }
    catch (SqlException ex)
    {
        throw new DataException("Database error occurred", ex);
    }
}
```

### API Integration:
```csharp
public async Task<T> CallApiAsync<T>(string endpoint)
{
    try
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json);
    }
    catch (HttpRequestException ex)
    {
        throw new ServiceUnavailableException($"API call failed: {endpoint}", ex);
    }
    catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
    {
        throw new TimeoutException($"API call timed out: {endpoint}", ex);
    }
    catch (JsonException ex)
    {
        throw new DataException("Invalid API response format", ex);
    }
}
```

## When to Use Exceptions vs Alternatives

**Use Exceptions for:**
- Unexpected error conditions
- Programming errors (null reference, index out of bounds)
- External service failures
- Security violations
- Resource exhaustion

**Consider Alternatives for:**
- Expected validation failures (use validation methods)
- User input errors (use TryParse patterns)
- Business rule violations (use Result patterns)
- Performance-critical code (use return codes)

## Advanced Exception Patterns

### Result Pattern (Exception-Free):
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
    
    private Result(bool success, T value, string error)
    {
        IsSuccess = success;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}

// Usage
public Result<decimal> CalculateDiscount(decimal amount, string customerType)
{
    if (amount <= 0)
        return Result<decimal>.Failure("Amount must be positive");
        
    var discount = customerType switch
    {
        "Premium" => amount * 0.2m,
        "Regular" => amount * 0.1m,
        _ => 0
    };
    
    return Result<decimal>.Success(discount);
}
```


## Industry Applications

Exception handling is critical for:
- **Web Applications**: Graceful error pages, API error responses
- **Desktop Software**: User-friendly error messages, crash recovery
- **Microservices**: Circuit breakers, retry policies, error propagation
- **Financial Systems**: Transaction rollback, audit logging
- **Real-time Systems**: Fault tolerance, degraded mode operation


Remember: Good exception handling is about building resilient systems that can recover gracefully from problems while providing clear feedback about what went wrong and why!
