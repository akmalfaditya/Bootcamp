# Conditional Compilation in C#

## Learning Objectives
Learn **conditional compilation** in C# to build applications that adapt to different environments, debug scenarios, and feature sets without runtime overhead. Learn to use preprocessor directives, conditional attributes, and compilation symbols to create flexible, maintainable code.

## What You'll Learn

### Preprocessor Directives
- **#define and #undef**: Creating and removing compilation symbols
- **#if, #elif, #else, #endif**: Conditional code inclusion
- **#warning and #error**: Compile-time messages and errors
- **#region and #endregion**: Code organization and folding

### Conditional Attributes
- **[Conditional] Attribute**: Methods that exist only in specific builds
- **ConditionalAttribute Benefits**: Zero runtime cost when disabled
- **Debug vs Release**: Automatically adapting behavior to build configuration
- **Custom Compilation Symbols**: Creating domain-specific conditional behavior

### Advanced Compilation Patterns
- **Multi-Target Frameworks**: Code that works across different .NET versions
- **Platform-Specific Code**: Adapting to different operating systems
- **Feature Flags**: Compile-time enablement of experimental features
- **API Versioning**: Supporting multiple API versions in the same codebase

## Key Features Demonstrated

### 1. **Basic Preprocessor Directives**
```csharp
#define DEBUG_MODE
#define FEATURE_ADVANCED_LOGGING

#if DEBUG_MODE
    Console.WriteLine("Debug mode is enabled");
#elif RELEASE
    Console.WriteLine("Release mode is enabled");
#else
    Console.WriteLine("Unknown build configuration");
#endif

#if FEATURE_ADVANCED_LOGGING
    // This code only exists when the symbol is defined
    LogDetailed("Advanced logging is available");
#endif
```

### 2. **Conditional Methods**
```csharp
[Conditional("DEBUG")]
public static void DebugLog(string message)
{
    Console.WriteLine($"DEBUG: {message}");
}

[Conditional("TRACE")]
public static void TraceOperation(string operation)
{
    Console.WriteLine($"TRACE: {operation} at {DateTime.Now}");
}

// These methods are completely removed in release builds
// if the symbols aren't defined
```

### 3. **Platform-Specific Code**
```csharp
#if WINDOWS
    private static void WindowsSpecificOperation()
    {
        // Windows-only implementation
    }
#elif LINUX
    private static void LinuxSpecificOperation()
    {
        // Linux-only implementation
    }
#else
    private static void CrossPlatformOperation()
    {
        // Fallback implementation
    }
#endif
```

### 4. **API Versioning**
```csharp
public class ApiService
{
#if API_V1
    public string GetData() => "Version 1 Data";
#elif API_V2
    public DataResponse GetData() => new DataResponse { Data = "Version 2", Version = 2 };
#else
    public async Task<DataResponse> GetDataAsync() => new DataResponse { Data = "Version 3", Version = 3 };
#endif
}
```

## Tips

### **Compilation Symbols Sources**
1. **Project File**: `<DefineConstants>DEBUG;TRACE;CUSTOM_SYMBOL</DefineConstants>`
2. **File Level**: `#define SYMBOL_NAME` at the top of .cs files
3. **Command Line**: `dotnet build -p:DefineConstants="SYMBOL1;SYMBOL2"`
4. **IDE Settings**: Visual Studio project properties

### **Best Practices**
```csharp
// Good: Clear, descriptive symbol names
#define ENABLE_EXPERIMENTAL_FEATURES
#define SUPPORT_LEGACY_API

// Bad: Unclear or confusing names
#define FLAG1
#define TEMP_CODE

// Good: Document what symbols control
/// <summary>
/// Enable this symbol to include advanced debugging features
/// Note: This may impact performance in production
/// </summary>
#define ADVANCED_DEBUGGING
```

### **Performance Considerations**
- Conditional methods have **zero runtime cost** when disabled
- Use for expensive debugging/logging operations
- Avoid conditional compilation for business logic
- Prefer dependency injection for runtime feature toggles

### **Common Patterns**
```csharp
// Debug-only validation
[Conditional("DEBUG")]
private static void ValidateArguments(object[] args)
{
    // Expensive validation that only runs in debug builds
    foreach (var arg in args)
    {
        if (arg == null)
            throw new ArgumentNullException(nameof(arg));
    }
}

// Feature toggle pattern
#if FEATURE_NEW_ALGORITHM
    private static int CalculateResult(int input) => NewAlgorithm(input);
#else
    private static int CalculateResult(int input) => LegacyAlgorithm(input);
#endif
```

## Real-World Applications

### **Logging Framework**
```csharp
public static class Logger
{
    [Conditional("DEBUG")]
    public static void Debug(string message) => WriteLog("DEBUG", message);
    
    [Conditional("TRACE")]
    public static void Trace(string message) => WriteLog("TRACE", message);
    
    // Always available
    public static void Error(string message) => WriteLog("ERROR", message);
    
    private static void WriteLog(string level, string message)
    {
        Console.WriteLine($"[{level}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}");
    }
}
```

### **Multi-Platform Application**
```csharp
public class PlatformService
{
#if WINDOWS
    public string GetPlatformInfo() => $"Windows {Environment.OSVersion}";
#elif LINUX
    public string GetPlatformInfo() => $"Linux {Environment.OSVersion}";
#elif MACOS
    public string GetPlatformInfo() => $"macOS {Environment.OSVersion}";
#else
    public string GetPlatformInfo() => "Unknown Platform";
#endif

#if MOBILE
    public void HandleTouchInput() { /* Mobile-specific touch handling */ }
#endif

#if DESKTOP
    public void HandleKeyboardShortcuts() { /* Desktop keyboard shortcuts */ }
#endif
}
```

### **Feature Development**
```csharp
public class PaymentService
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
#if FEATURE_NEW_PAYMENT_GATEWAY
        // New implementation being tested
        return await NewPaymentGateway.ProcessAsync(request);
#else
        // Stable existing implementation
        return await LegacyPaymentGateway.ProcessAsync(request);
#endif
    }
    
#if BETA_FEATURES
    [Conditional("BETA_FEATURES")]
    public void EnableBetaFeatures()
    {
        // Beta functionality only available to beta users
        BetaFeatureManager.EnableAll();
    }
#endif
}
```

### **Testing and QA**
```csharp
public class DatabaseService
{
    private readonly string _connectionString;
    
    public DatabaseService()
    {
#if TESTING
        _connectionString = "Server=test-db;Database=TestDb;";
#elif STAGING
        _connectionString = "Server=staging-db;Database=StagingDb;";
#else
        _connectionString = "Server=prod-db;Database=ProductionDb;";
#endif
    }
    
    [Conditional("DEBUG")]
    [Conditional("TESTING")]
    public void EnableQueryLogging()
    {
        // Only log SQL queries in debug/test builds
    }
}
```

## Integration with Modern C#

### **Target Framework Conditionals**
```csharp
#if NET6_0_OR_GREATER
    // Use new .NET 6+ features
    public string GetFileHash(string path) => 
        Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path)));
#else
    // Fallback for older frameworks
    public string GetFileHash(string path)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(File.ReadAllBytes(path));
        return BitConverter.ToString(hash).Replace("-", "");
    }
#endif
```

### **Nullable Reference Types (C# 8+)**
```csharp
#nullable enable
public class ModernService
{
#if LEGACY_SUPPORT
    public string? GetData(string? input) => input?.ToUpper();
#else
    public string GetData(string input) => input.ToUpper();
#endif
}
#nullable restore
```

### **Record Types (C# 9+)**
```csharp
#if NET5_0_OR_GREATER
public record ApiResponse(string Data, int StatusCode);
#else
public class ApiResponse
{
    public string Data { get; set; }
    public int StatusCode { get; set; }
}
#endif
```

## Industry Impact

Conditional compilation is crucial for:

- **Multi-Platform Development**: Single codebase supporting multiple platforms
- **Continuous Integration**: Different builds for different environments
- **Feature Management**: Safe deployment of experimental features
- **Performance Optimization**: Debug code that doesn't impact production
- **Legacy Support**: Maintaining compatibility across framework versions
