# Conditional Compilation in C# - Comprehensive Demo

This project demonstrates all aspects of conditional compilation in C#, providing practical examples of how to use preprocessor directives, conditional attributes, and runtime flags effectively.

## What is Conditional Compilation?

Conditional compilation is a powerful mechanism that allows developers to selectively include or exclude sections of source code during the compilation process. This technique operates at compile-time, meaning decisions about which code to include are made when the source code is transformed into executable machine code, not when the program is running.

The fundamental difference between conditional compilation and runtime decisions lies in when the choice is made:

**Compile-time decisions (Conditional Compilation):**
- Code is either included in the final executable or completely removed
- No runtime performance overhead for excluded code
- Decisions cannot be changed without recompiling the application
- Perfect for features that will not change during execution

**Runtime decisions (Traditional if/else statements):**
- All code paths are included in the executable
- Performance overhead exists for condition checking
- Decisions can be changed during program execution
- Suitable for user-configurable features and dynamic behavior

This distinction makes conditional compilation particularly valuable for creating optimized production builds, managing different deployment environments, and maintaining multiple versions of code within a single codebase.

## Key Features Demonstrated

### 1. Preprocessor Directives

Preprocessor directives are special instructions that begin with the hash symbol (#) and are processed before the main compilation phase. These directives control which portions of code are included in the final compiled output.

**Core Directives:**
- `#if` - Tests whether a symbol is defined and includes the following code block if true
- `#else` - Provides an alternative code block when the `#if` condition is false
- `#elif` - Combines `#else` and `#if` to test additional conditions in sequence
- `#endif` - Marks the end of a conditional compilation block

**Symbol Management:**
- `#define` - Defines a preprocessor symbol within a source file
- `#undef` - Removes a previously defined symbol

**Logical Operations:**
Preprocessor directives support logical operators to create complex conditions:
- `&&` (AND) - Both conditions must be true
- `||` (OR) - Either condition must be true
- `!` (NOT) - Inverts the condition

**Symbol Definition Scope:**
- **File-level symbols**: Defined using `#define` at the top of individual source files
- **Project-level symbols**: Defined in the project file (.csproj) and apply to all source files
- **Command-line symbols**: Specified during the build process via compiler flags

### 2. Conditional Attributes

The `[Conditional]` attribute, located in the `System.Diagnostics` namespace, provides an elegant solution for conditional method inclusion. When applied to a method, this attribute instructs the compiler to eliminate all calls to that method if the specified symbol is not defined.

**Key Advantages:**
- **Clean syntax**: No need to wrap method calls in `#if/#endif` blocks
- **Argument evaluation bypass**: Method arguments are not evaluated when the symbol is undefined
- **Zero runtime cost**: Complete elimination of method calls when disabled
- **Multiple conditions**: A single method can have multiple `[Conditional]` attributes

**Common Use Cases:**
- Debug and trace logging methods
- Performance measurement code
- Development-only validation routines
- Testing and diagnostic utilities

**Important Considerations:**
- Conditional methods must have a `void` return type
- The attribute affects the calling code, not the method definition itself
- Method calls are eliminated even across assembly boundaries

### 3. Runtime vs Compile-time Decisions

Understanding when to use compile-time versus runtime decision-making is crucial for writing efficient and maintainable code.

**Compile-time Decisions (Conditional Compilation):**
- **Performance**: Zero runtime overhead when features are disabled
- **Code size**: Unused code is completely removed from the executable
- **Optimization**: Allows for different algorithm implementations based on build configuration
- **Limitations**: Requires recompilation to change behavior

**Runtime Decisions (Boolean Variables):**
- **Flexibility**: Can be changed during program execution
- **Configuration**: Supports user preferences and environment-specific settings
- **Overhead**: Small performance cost for condition evaluation
- **Deployment**: Same executable can behave differently based on configuration

**Hybrid Approaches:**
The most effective solutions often combine both techniques, using compile-time decisions for performance-critical features that will not change, and runtime decisions for user-configurable options.

### 4. Advanced Patterns

**Type Aliasing:**
Conditional compilation can be used to create type aliases that switch between different implementations based on defined symbols. This technique is particularly useful for API versioning and maintaining backward compatibility.

**API Versioning:**
By using conditional compilation, developers can maintain multiple versions of APIs within a single codebase, allowing for gradual migration and testing of new implementations.

**Performance Metrics:**
Conditional compilation enables the inclusion of performance measurement code that can be completely removed from production builds, ensuring zero overhead when not needed.

**Multi-configuration Build Support:**
Projects can be configured to produce different executables with varying feature sets, all from the same source code, by defining different sets of compilation symbols.

### 5. Practical Logging Solutions

Logging presents unique challenges in terms of performance and configurability. This project demonstrates several approaches to address these challenges.

**Traditional Logging Issues:**
- Method arguments are always evaluated, even when logging is disabled
- Runtime checks add overhead to every logging call
- Configuration changes require application restart

**Conditional Logging Solutions:**
- **Conditional attributes**: Eliminate logging calls entirely when disabled
- **Deferred evaluation**: Use `Func<T>` delegates to avoid expensive argument computation
- **Hybrid approaches**: Combine compile-time and runtime techniques for maximum flexibility

**Deferred Argument Evaluation:**
The `Func<T>` delegate approach allows for runtime logging control while avoiding the performance penalty of evaluating expensive arguments when logging is disabled. This technique uses lambda expressions to defer computation until it is actually needed.

## Project Structure

```
├── Program.cs                    # Main demonstration program
├── DataProcessingService.cs      # Advanced service class examples
├── RuntimeLoggingExample.cs      # Runtime logging with deferred evaluation
├── Conditional Compilation.csproj # Project file with symbol definitions
└── README.md                    # This documentation
```

## Symbol Definitions

Preprocessor symbols are identifiers that control conditional compilation behavior. These symbols act as flags that determine which code sections are included during compilation.

### Project-Level Symbols (defined in .csproj)

Project-level symbols are defined in the project file and apply to all source files within the project. This approach ensures consistent behavior across the entire application.

- `DEVELOPMENT` - Activates development-specific features including detailed validation, enhanced error messages, and additional debugging capabilities
- `LOGGING` - Enables the logging infrastructure throughout the application, including file output and console logging
- `PERFORMANCE_METRICS` - Includes performance measurement code that tracks execution times and system resource usage
- `LOGGINGMODE` - Specifically enables the LogStatus method calls for detailed operation logging

### File-Level Symbols (defined in Program.cs)

File-level symbols are defined using the `#define` directive at the top of individual source files. These symbols override project-level settings and provide file-specific control.

- `DEBUG_MODE` - Enables debug-specific features including verbose output, detailed error information, and additional validation checks
- `TESTING_ENABLED` - Activates testing-related functionality such as test data generation, verification routines, and extended logging
- `LOGGINGMODE` - Overrides the project-level logging setting for this specific file

### Conditional Symbols (demonstration purposes)

These symbols are commented out in the source code to demonstrate different compilation scenarios. Uncommenting them will activate their respective features.

- `LEGACY_SUPPORT` - Would enable backward compatibility features, including older API implementations and deprecated functionality
- `V2` - Would activate version 2 features, including modern type implementations and enhanced algorithms
- `TESTMODE` - Would enable test-specific functionality and mock implementations

### Symbol Definition Hierarchy

When symbols are defined at multiple levels, the following precedence applies:
1. Command-line definitions (highest priority)
2. File-level definitions (`#define` in source files)
3. Project-level definitions (in .csproj file)
4. Default framework definitions (lowest priority)

This hierarchy allows for flexible configuration management, where developers can override project settings for specific files or build scenarios.

## Running the Demo

1. **Build and run the project:**
   ```bash
   dotnet run
   ```

2. **Try different symbol combinations:**
   - Edit the `#define` statements in `Program.cs`
   - Modify the `<DefineConstants>` in the `.csproj` file
   - Build and run to see how behavior changes

3. **Experiment with build configurations:**
   ```bash
   dotnet build -c Debug    # Includes DEBUG_EXTRA symbols
   dotnet build -c Release  # Includes OPTIMIZE symbols
   ```

## Key Concepts Demonstrated

### 1. Basic Conditional Compilation

The fundamental concept of conditional compilation involves using preprocessor directives to include or exclude code blocks based on defined symbols. This example demonstrates the basic syntax:

```csharp
#if DEBUG_MODE
    Console.WriteLine("Debug mode is active");
#else
    Console.WriteLine("Production mode");
#endif
```

**How it works:**
- The preprocessor evaluates the `DEBUG_MODE` symbol during compilation
- If the symbol is defined, the first code block is included in the final executable
- If the symbol is not defined, the second code block (after `#else`) is included
- The code that is not selected is completely removed from the compiled output

**Practical implications:**
- Zero runtime performance impact from excluded code
- Smaller executable size when debug features are disabled
- Ability to have completely different behavior based on build configuration

### 2. Conditional Attributes

The `[Conditional]` attribute provides a cleaner alternative to wrapping method calls in preprocessor directives. This example shows its usage:

```csharp
[Conditional("LOGGINGMODE")]
static void LogStatus(string message)
{
    // Method call eliminated if LOGGINGMODE not defined
    Console.WriteLine($"[LOG] {message}");
}
```

**How it works:**
- The attribute is applied to the method definition
- When `LOGGINGMODE` is not defined, all calls to `LogStatus` are eliminated during compilation
- Method arguments are not evaluated when the call is eliminated
- This applies even when the calling code is in a different assembly

**Advantages over manual preprocessing:**
- Cleaner calling code without `#if/#endif` blocks
- Automatic argument evaluation bypass
- Consistent behavior across assembly boundaries
- Easier maintenance and readability

### 3. Type Aliasing

Conditional compilation can be used to create type aliases that switch between different implementations. This technique is particularly useful for API versioning:

```csharp
using TestType =
#if V2
    System.Collections.Generic.Dictionary<string, object>;
#else
    System.Collections.Hashtable;
#endif
```

**How it works:**
- The `using` statement creates a type alias called `TestType`
- The actual type depends on whether the `V2` symbol is defined
- All code using `TestType` will use the appropriate implementation
- This allows for seamless switching between different API versions

**Benefits:**
- Single codebase supports multiple implementations
- Gradual migration path between API versions
- Minimal code changes required to switch implementations
- Compile-time validation of type compatibility

### 4. Deferred Evaluation for Runtime Logging

For scenarios requiring runtime configurability, deferred evaluation using `Func<T>` delegates provides an elegant solution:

```csharp
// Traditional approach - argument always evaluated
LogStatus("Data: " + GetExpensiveData());

// Deferred approach - argument only evaluated if needed
LogStatus(() => "Data: " + GetExpensiveData());
```

**How it works:**
- The traditional approach always calls `GetExpensiveData()`, even when logging is disabled
- The deferred approach wraps the expression in a lambda function
- The lambda is only executed if logging is actually enabled
- This prevents unnecessary computation when logging is disabled

**Performance benefits:**
- Avoids expensive operations when logging is disabled
- Maintains runtime configurability
- Clean syntax using lambda expressions
- Suitable for user-configurable logging levels

## Performance Implications

Understanding the performance characteristics of different conditional compilation approaches is crucial for making informed architectural decisions.

### Compile-time Decisions

**Advantages:**
- **Zero runtime cost**: Code that is not selected during compilation is completely absent from the final executable
- **Optimal performance**: No conditional checks are performed during program execution
- **Reduced memory footprint**: Unused code paths do not consume memory
- **Compiler optimizations**: The compiler can optimize more aggressively when code paths are eliminated

**Disadvantages:**
- **Inflexibility**: Behavior cannot be changed without recompiling the entire application
- **Build complexity**: Multiple build configurations may be required for different deployment scenarios
- **Testing overhead**: All possible symbol combinations should be tested to ensure correctness

**Ideal use cases:**
- Performance-critical code paths where any overhead is unacceptable
- Features that will never change during program execution
- Platform-specific implementations
- Debug and diagnostic code that should not impact production performance

### Runtime Decisions

**Advantages:**
- **Flexibility**: Behavior can be modified during program execution without recompilation
- **Configuration support**: Settings can be loaded from files, environment variables, or user input
- **Dynamic behavior**: Responses to changing conditions during program execution
- **Simplified deployment**: Single executable can serve multiple purposes

**Disadvantages:**
- **Performance overhead**: Conditional checks consume CPU cycles during execution
- **Memory usage**: All code paths remain in memory even when not used
- **Complexity**: Additional logic required to manage runtime state
- **Security considerations**: Runtime flags may expose internal functionality

**Ideal use cases:**
- User-configurable features and preferences
- Environment-specific behavior (development, testing, production)
- Features that may need to be toggled during program execution
- A/B testing and feature rollouts

### Conditional Attributes

**Advantages:**
- **Clean syntax**: No need to wrap method calls in preprocessor directives
- **Argument evaluation bypass**: Method parameters are not computed when calls are eliminated
- **Zero cost when disabled**: Complete elimination of method calls and their overhead
- **Cross-assembly support**: Behavior is consistent even when calling code is in different assemblies

**Disadvantages:**
- **Compile-time only**: Cannot be changed during program execution
- **Limited to void methods**: Methods with return values cannot use conditional attributes
- **Symbol dependency**: Requires careful management of preprocessor symbols

**Ideal use cases:**
- Debug and trace logging systems
- Performance measurement and profiling code
- Development-only validation and diagnostic routines
- Testing utilities and mock implementations

### Performance Measurement Results

The project includes performance benchmarks that demonstrate the measurable differences between approaches:

- **Compile-time conditionals**: 0ms overhead when disabled (code eliminated)
- **Runtime conditionals**: 2ms overhead for 1,000,000 iterations (minimal but measurable)
- **Conditional method calls**: 0ms overhead when symbol not defined (calls eliminated)

These results illustrate that while runtime overhead is often minimal, it can become significant in performance-critical scenarios or when executed frequently within tight loops.

## Best Practices

Effective use of conditional compilation requires adherence to established patterns and practices that ensure maintainable, performant, and reliable code.

### 1. Appropriate Use of Compile-time Features

**Recommended scenarios:**
- Performance-critical code paths where any runtime overhead is unacceptable
- Platform-specific implementations that will never change during execution
- Debug and diagnostic code that should not impact production performance
- Features that are fundamentally different between build configurations

**Implementation guidelines:**
- Use descriptive symbol names that clearly indicate their purpose
- Document the behavior changes that each symbol enables
- Ensure that all symbol combinations are tested thoroughly
- Avoid complex nested conditional compilation that reduces readability

### 2. Runtime Configuration Strategy

**Recommended scenarios:**
- User-configurable features and application settings
- Environment-specific behavior that may vary between deployments
- Features that need to be toggled during program execution
- A/B testing and gradual feature rollouts

**Implementation guidelines:**
- Use configuration files or environment variables for runtime settings
- Implement proper validation for runtime configuration values
- Provide reasonable defaults for all configurable options
- Consider the security implications of exposing runtime controls

### 3. Hybrid Approach Implementation

**Combining techniques effectively:**
- Use compile-time decisions for foundational architectural choices
- Implement runtime configuration for user-facing features
- Apply conditional attributes for debugging and diagnostic code
- Maintain clear separation between different types of conditional logic

**Design considerations:**
- Minimize the complexity of interactions between compile-time and runtime decisions
- Ensure that runtime configuration cannot override critical compile-time security decisions
- Document the relationship between different conditional mechanisms
- Test all combinations of compile-time and runtime configurations

### 4. Conditional Attribute Usage

**Recommended practices:**
- Use conditional attributes for debug and trace logging instead of manual preprocessor directives
- Apply multiple conditional attributes to methods that should be available under different conditions
- Ensure that conditional methods have void return types and do not affect program flow
- Consider the performance implications of method argument evaluation

**Common patterns:**
- Logging methods with different verbosity levels
- Performance measurement and profiling code
- Development-only validation routines
- Testing utilities and mock implementations

### 5. Deferred Evaluation Techniques

**When to use Func<T> delegates:**
- Runtime logging scenarios where argument computation is expensive
- User-configurable features that may involve complex calculations
- Scenarios where the decision to execute code depends on runtime state
- Performance-sensitive code that benefits from lazy evaluation

**Implementation guidelines:**
- Use lambda expressions for clean, readable deferred evaluation
- Ensure that deferred operations are thread-safe when necessary
- Consider the memory allocation implications of delegate creation
- Document the performance characteristics of deferred operations

### 6. Symbol Management

**Naming conventions:**
- Use uppercase letters for preprocessor symbols (e.g., `DEBUG_MODE`, `FEATURE_ENABLED`)
- Choose descriptive names that clearly indicate functionality
- Group related symbols with consistent prefixes (e.g., `FEATURE_CACHING`, `FEATURE_LOGGING`)
- Avoid abbreviations that may be unclear to other developers

**Organization strategies:**
- Define project-wide symbols in the project file for consistency
- Use file-level symbols only for file-specific customizations
- Document all symbols and their effects in project documentation
- Maintain a central registry of symbols used across the project

### 7. Testing and Validation

**Comprehensive testing approach:**
- Test all major combinations of preprocessor symbols
- Validate that disabled features are completely eliminated from the executable
- Ensure that runtime configuration changes produce expected behavior
- Use automated testing to verify different build configurations

**Quality assurance practices:**
- Include conditional compilation testing in continuous integration pipelines
- Perform performance testing with different symbol combinations
- Validate that security-sensitive code is properly protected by conditional compilation
- Review code changes that modify conditional compilation logic carefully

## Common Use Cases

Conditional compilation serves numerous practical purposes in professional software development, addressing real-world challenges in deployment, maintenance, and performance optimization.

### Debug vs Release Builds

**Purpose:** Creating optimized production builds while maintaining comprehensive debugging capabilities during development.

**Implementation approach:**
- Debug builds include extensive logging, validation, and diagnostic code
- Release builds exclude debug overhead for optimal performance
- Conditional attributes eliminate debug method calls completely in release builds
- Type safety and functionality remain consistent across both build types

**Benefits:**
- Zero performance impact in production from debug code
- Comprehensive debugging capabilities during development
- Simplified deployment process with optimized executables
- Consistent behavior across different build configurations

### Multiple Deployment Environments

**Purpose:** Supporting different configurations for development, testing, staging, and production environments without code duplication.

**Implementation approach:**
- Environment-specific connection strings and configuration values
- Different logging levels and output destinations per environment
- Feature flags that enable testing capabilities in non-production environments
- Conditional inclusion of monitoring and diagnostic tools

**Benefits:**
- Single codebase serves multiple deployment scenarios
- Reduced maintenance overhead from environment-specific code branches
- Consistent core functionality across all environments
- Flexible configuration management for different operational requirements

### API Versioning

**Purpose:** Maintaining backward compatibility while introducing new features and improvements.

**Implementation approach:**
- Conditional type aliases for different API versions
- Method overloads that provide different functionality based on version symbols
- Gradual migration paths that allow testing new implementations alongside legacy code
- Version-specific documentation and examples

**Benefits:**
- Smooth transition between API versions
- Reduced risk of breaking existing client code
- Ability to test new implementations without affecting production systems
- Simplified maintenance of multiple API versions

### Legacy Compatibility

**Purpose:** Supporting older systems and frameworks while modernizing the codebase.

**Implementation approach:**
- Conditional compilation for framework-specific code
- Alternative implementations for deprecated APIs
- Compatibility layers that translate between old and new interfaces
- Progressive enhancement that adds modern features when available

**Benefits:**
- Gradual modernization without breaking existing functionality
- Support for diverse deployment targets
- Reduced code duplication between legacy and modern implementations
- Clear migration path for future updates

### Performance Profiling

**Purpose:** Including performance measurement code that can be completely removed from production builds.

**Implementation approach:**
- Conditional attributes for performance measurement methods
- Timing code that tracks execution duration and resource usage
- Memory profiling that monitors allocation patterns
- Conditional compilation of performance-critical code paths

**Benefits:**
- Detailed performance insights during development and testing
- Zero overhead in production builds
- Ability to identify performance bottlenecks without affecting end users
- Comprehensive profiling data for optimization decisions

### Feature Flags and A/B Testing

**Purpose:** Enabling controlled rollouts of new features and experimental functionality.

**Implementation approach:**
- Compile-time feature flags for foundational changes
- Runtime feature flags for user-facing functionality
- Conditional compilation for experimental algorithms
- Gradual feature activation based on user groups or deployment environments

**Benefits:**
- Risk mitigation through controlled feature rollouts
- Ability to quickly disable problematic features
- A/B testing capabilities for feature effectiveness
- Simplified feature management across different user segments

### Platform-Specific Code

**Purpose:** Supporting multiple operating systems and hardware platforms from a single codebase.

**Implementation approach:**
- Platform-specific conditional compilation blocks
- Alternative implementations for platform-dependent functionality
- Conditional inclusion of platform-specific libraries and APIs
- Cross-platform compatibility layers

**Benefits:**
- Single codebase supports multiple platforms
- Platform-specific optimizations where beneficial
- Reduced maintenance overhead compared to separate codebases
- Consistent core functionality across all supported platforms

### Security and Compliance

**Purpose:** Including security-sensitive code only in appropriate build configurations.

**Implementation approach:**
- Conditional compilation for security features that should not be present in all builds
- Debug-only code that might expose sensitive information
- Compliance-specific functionality for regulated environments
- Security auditing code that can be enabled for specific builds

**Benefits:**
- Enhanced security through conditional exclusion of sensitive code
- Compliance with industry regulations and standards
- Reduced attack surface in production deployments
- Flexible security posture based on deployment requirements

## Learning Objectives

This comprehensive demonstration project is designed to provide practical understanding of conditional compilation concepts through hands-on exploration and experimentation.

### Understanding Preprocessor Directives

**Objective:** Master the syntax and behavior of preprocessor directives for conditional code inclusion.

**Key learning points:**
- Recognize the difference between preprocessor directives and runtime conditional statements
- Understand how logical operators combine multiple conditions in preprocessor directives
- Learn the proper syntax for nested conditional compilation blocks
- Appreciate the compile-time nature of preprocessor evaluation

**Practical skills developed:**
- Writing complex conditional compilation logic using multiple symbols
- Debugging conditional compilation issues and understanding why certain code is excluded
- Optimizing code structure to minimize conditional compilation complexity
- Creating maintainable conditional code that is easy to understand and modify

### Choosing Between Compile-time and Runtime Approaches

**Objective:** Develop judgment for selecting the appropriate conditional mechanism for different scenarios.

**Key learning points:**
- Understand the performance implications of compile-time versus runtime decisions
- Recognize scenarios where compile-time decisions are more appropriate
- Identify situations where runtime flexibility is essential
- Learn to balance performance, maintainability, and flexibility requirements

**Practical skills developed:**
- Analyzing performance requirements to determine the best approach
- Designing systems that effectively combine compile-time and runtime decisions
- Evaluating the trade-offs between different conditional compilation strategies
- Creating flexible architectures that can adapt to changing requirements

### Implementing Elegant Logging Solutions

**Objective:** Create efficient, maintainable logging systems using conditional compilation techniques.

**Key learning points:**
- Understand the problems with traditional logging approaches
- Learn how conditional attributes eliminate method calls completely
- Master the use of deferred evaluation for runtime logging control
- Appreciate the benefits of argument evaluation bypass

**Practical skills developed:**
- Implementing logging systems that have zero performance impact when disabled
- Creating flexible logging configurations that support multiple environments
- Designing logging APIs that are easy to use and maintain
- Optimizing logging performance while maintaining functionality

### Managing Different Build Configurations

**Objective:** Configure and manage projects with multiple build scenarios and deployment targets.

**Key learning points:**
- Understand the relationship between project-level and file-level symbol definitions
- Learn how to create and manage multiple build configurations
- Master the use of command-line build options for different scenarios
- Appreciate the importance of testing different build configurations

**Practical skills developed:**
- Creating project files that support multiple build scenarios
- Configuring continuous integration systems for different build types
- Managing symbol definitions across complex project structures
- Troubleshooting build configuration issues and symbol conflicts

### Combining Conditional Compilation with Runtime Flags

**Objective:** Design systems that effectively integrate compile-time and runtime decision-making.

**Key learning points:**
- Understand when to use compile-time versus runtime approaches
- Learn how to create hybrid systems that leverage both techniques
- Master the design patterns for combining different conditional mechanisms
- Appreciate the complexity management aspects of hybrid approaches

**Practical skills developed:**
- Architecting systems that use both compile-time and runtime decisions appropriately
- Creating configuration systems that support both static and dynamic behavior
- Designing APIs that hide the complexity of conditional compilation from users
- Implementing systems that are both performant and flexible

### Understanding Performance Implications

**Objective:** Quantify and optimize the performance impact of different conditional compilation approaches.

**Key learning points:**
- Measure the actual performance differences between compile-time and runtime approaches
- Understand the memory implications of different conditional compilation strategies
- Learn how compiler optimizations interact with conditional compilation
- Appreciate the cumulative impact of conditional compilation decisions

**Practical skills developed:**
- Conducting performance benchmarks for conditional compilation scenarios
- Optimizing code for maximum performance using appropriate conditional techniques
- Analyzing the trade-offs between performance and maintainability
- Creating performance-conscious architectures that scale effectively

### Mastery Assessment

**Indicators of successful learning:**
- Ability to choose the appropriate conditional compilation technique for specific scenarios
- Skill in creating maintainable conditional code that is easy to understand and modify
- Understanding of the performance implications and trade-offs of different approaches
- Competence in managing complex build configurations and symbol definitions
- Proficiency in combining compile-time and runtime approaches effectively

**Advanced competencies:**
- Designing conditional compilation strategies for large-scale projects
- Creating reusable patterns and libraries that leverage conditional compilation
- Mentoring other developers in conditional compilation best practices
- Contributing to architectural decisions involving conditional compilation trade-offs

## Next Steps

1. Try modifying the symbol definitions and observe behavior changes
2. Experiment with different build configurations
3. Add your own conditional features to the service class
4. Implement conditional compilation in your own projects
5. Practice combining compile-time and runtime approaches
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
