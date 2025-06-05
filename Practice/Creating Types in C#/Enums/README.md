# C# Enums - Named Constants and Type Safety

## Learning Objectives

Master C# enums to create type-safe, readable code with named constants instead of magic numbers, and learn advanced enum patterns for professional development.

**What you'll master:**
- Creating and using basic enums for type-safe constants
- Understanding enum underlying types and custom values
- Implementing flags enums for combinable options
- Performing safe enum conversions and validations
- Using enum extension methods and advanced techniques
- Applying enums in real-world scenarios and design patterns
- Following enum best practices for maintainable code

## Core Concepts Covered

### Enum Fundamentals
- **Named Constants**: Replacing magic numbers with meaningful names
- **Underlying Types**: Understanding byte, int, long backing stores
- **Automatic Numbering**: How compiler assigns sequential values
- **Explicit Values**: Manually controlling enum numeric values
- **Type Safety**: Compile-time checking prevents invalid values

### Flags Enums
- **Combinable Values**: Using bitwise operations for multiple selections
- **Power of Two Values**: Ensuring proper bit manipulation
- **HasFlag Method**: Safe checking for flag combinations
- **Bitwise Operations**: OR, AND, XOR for flag manipulation
- **None and All Values**: Common patterns for flag initialization

### Conversions and Validation
- **Explicit Casting**: Converting between enums and integers
- **Enum.Parse**: Converting strings to enum values
- **Enum.TryParse**: Safe string parsing without exceptions
- **Enum.IsDefined**: Validating enum values from external sources
- **Enum.GetValues**: Retrieving all possible enum values

## Key Features with Examples

### Basic Enum Declaration and Usage
```csharp
public enum OrderStatus
{
    Pending,     // 0
    Processing,  // 1
    Shipped,     // 2
    Delivered,   // 3
    Cancelled    // 4
}

// Usage with type safety
OrderStatus currentStatus = OrderStatus.Processing;
Console.WriteLine($"Order is: {currentStatus}");

// Switch expressions work beautifully with enums
string message = currentStatus switch
{
    OrderStatus.Pending => "Waiting for processing",
    OrderStatus.Processing => "Order is being prepared",
    OrderStatus.Shipped => "Order is on the way",
    OrderStatus.Delivered => "Order has arrived",
    OrderStatus.Cancelled => "Order was cancelled",
    _ => "Unknown status"
};
```

### Custom Underlying Types and Values
```csharp
public enum Priority : byte  // Uses byte instead of int
{
    Low = 1,
    Medium = 5,
    High = 10,
    Critical = 20
}

public enum HttpStatusCode : int
{
    OK = 200,
    NotFound = 404,
    InternalServerError = 500
}

// Explicit values allow semantic meaning
Priority taskPriority = Priority.High;
int priorityWeight = (int)taskPriority; // 10
```

### Flags Enums for Combinable Options
```csharp
[Flags]
public enum FilePermissions
{
    None = 0,
    Read = 1,      // 0001
    Write = 2,     // 0010
    Execute = 4,   // 0100
    All = Read | Write | Execute  // 0111
}

// Combining permissions
FilePermissions userPermissions = FilePermissions.Read | FilePermissions.Write;
Console.WriteLine($"User permissions: {userPermissions}");

// Checking for specific permissions
if (userPermissions.HasFlag(FilePermissions.Write))
{
    Console.WriteLine("User can write files");
}

// Adding permission
userPermissions |= FilePermissions.Execute;

// Removing permission
userPermissions &= ~FilePermissions.Write;
```

### Safe Enum Parsing and Validation
```csharp
// Safe string parsing
string statusText = "Processing";
if (Enum.TryParse<OrderStatus>(statusText, out OrderStatus parsedStatus))
{
    Console.WriteLine($"Parsed status: {parsedStatus}");
}

// Validation for external data
int statusFromDatabase = 99; // Invalid value
if (Enum.IsDefined(typeof(OrderStatus), statusFromDatabase))
{
    var status = (OrderStatus)statusFromDatabase;
    ProcessOrder(status);
}
else
{
    Console.WriteLine("Invalid status received from database");
}

// Getting all enum values
foreach (OrderStatus status in Enum.GetValues<OrderStatus>())
{
    Console.WriteLine($"Status: {status} = {(int)status}");
}
```

### Advanced Enum Extension Methods
```csharp
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
    
    public static T Next<T>(this T value) where T : struct, Enum
    {
        T[] values = Enum.GetValues<T>();
        int index = Array.IndexOf(values, value);
        return values[(index + 1) % values.Length];
    }
}

// Usage with extension methods
[Description("Order is being processed")]
Processing,

OrderStatus status = OrderStatus.Processing;
Console.WriteLine(status.GetDescription()); // "Order is being processed"
OrderStatus nextStatus = status.Next(); // OrderStatus.Shipped
```

## Tips

### Design Best Practices
- **Meaningful Names**: Use descriptive enum and value names that express intent
- **Consistent Naming**: Follow PascalCase for enum types and values
- **Logical Ordering**: Arrange enum values in logical progression when possible
- **Default Values**: Consider what value 0 represents - often "None" or "Default"

### Flags Enum Guidelines
- **Power of Two**: Always use powers of 2 for flags enum values (1, 2, 4, 8, 16...)
- **None Value**: Include a None = 0 value for empty state
- **All Value**: Consider an All value combining all flags for convenience
- **Flags Attribute**: Always mark with [Flags] attribute for proper ToString() behavior

### Performance Considerations
- **Underlying Types**: Use smaller types (byte, short) when range allows to save memory
- **Avoid IsDefined**: It's slow due to reflection - cache results if called frequently
- **Boxing Avoidance**: Use generic methods where possible to avoid boxing enum values
- **Switch vs Dictionary**: Switch statements are faster than dictionary lookups for enums

### Common Pitfalls
- **Invalid Casts**: Always validate enum values from external sources
- **Flags Combinations**: Ensure proper power-of-2 values for flags enums
- **ToString Performance**: Enum.ToString() is slow - cache or use constants for hot paths
- **Default Values**: Be aware that default(EnumType) is always 0

## Real-World Applications

### Game State Management
```csharp
public enum GameState
{
    MainMenu,
    Loading,
    Playing,
    Paused,
    GameOver,
    Settings
}

public class GameManager
{
    public GameState CurrentState { get; private set; }
    
    public void ChangeState(GameState newState)
    {
        var previousState = CurrentState;
        CurrentState = newState;
        
        OnStateChanged(previousState, newState);
    }
    
    private void OnStateChanged(GameState from, GameState to)
    {
        (from, to) switch
        {
            (GameState.Loading, GameState.Playing) => StartGame(),
            (GameState.Playing, GameState.Paused) => PauseGame(),
            (GameState.Paused, GameState.Playing) => ResumeGame(),
            _ => { /* Handle other transitions */ }
        };
    }
}
```

### Medical System Status Tracking
```csharp
[Flags]
public enum PatientCondition
{
    None = 0,
    Stable = 1,
    Critical = 2,
    Infectious = 4,
    Allergic = 8,
    Diabetic = 16,
    Hypertensive = 32
}

public class Patient
{
    public PatientCondition Conditions { get; set; }
    
    public bool RequiresIsolation => Conditions.HasFlag(PatientCondition.Infectious);
    public bool RequiresSpecialDiet => Conditions.HasFlag(PatientCondition.Diabetic) || 
                                      Conditions.HasFlag(PatientCondition.Allergic);
    
    public void AddCondition(PatientCondition condition)
    {
        Conditions |= condition;
        LogConditionChange($"Added condition: {condition}");
    }
    
    public void RemoveCondition(PatientCondition condition)
    {
        Conditions &= ~condition;
        LogConditionChange($"Removed condition: {condition}");
    }
}
```

### API Response Handling
```csharp
public enum ApiResponseStatus
{
    Success = 200,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServerError = 500,
    ServiceUnavailable = 503
}

public class ApiResponse<T>
{
    public ApiResponseStatus Status { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    
    public bool IsSuccess => Status == ApiResponseStatus.Success;
    public bool IsClientError => (int)Status >= 400 && (int)Status < 500;
    public bool IsServerError => (int)Status >= 500;
}

// Usage in error handling
public async Task<ApiResponse<UserData>> GetUserAsync(int userId)
{
    try
    {
        var user = await userService.GetUserAsync(userId);
        return new ApiResponse<UserData>
        {
            Status = ApiResponseStatus.Success,
            Data = user
        };
    }
    catch (UserNotFoundException)
    {
        return new ApiResponse<UserData>
        {
            Status = ApiResponseStatus.NotFound,
            Message = "User not found"
        };
    }
}
```

## Industry Impact

### Code Quality Benefits
- **Readability**: Self-documenting code that clearly expresses intent
- **Maintainability**: Changes to enum values automatically propagate throughout codebase
- **Type Safety**: Compile-time checking prevents invalid constant usage
- **Refactoring Safety**: IDEs can safely rename and refactor enum usage



