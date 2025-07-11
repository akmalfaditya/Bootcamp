# Build Configurations Guide - Conditional Compilation

This comprehensive guide explains how to configure conditional compilation symbols for different build scenarios, demonstrating all concepts from the training material.

## Understanding Conditional Compilation

Conditional compilation allows you to include or exclude code at **compile time** based on preprocessor symbols. This is different from runtime decisions - the code is either included in the final executable or completely removed.

## Current Project Configuration

The project is configured with symbols in the `.csproj` file:
```xml
<DefineConstants>DEVELOPMENT;LOGGING;PERFORMANCE_METRICS;LOGGINGMODE</DefineConstants>
```

### Project-Level Symbol Definitions
These symbols affect **all source files** in the project:
- `DEVELOPMENT` - Enables development-specific features and validation
- `LOGGING` - Enables general logging capabilities
- `PERFORMANCE_METRICS` - Includes performance measurement code
- `LOGGINGMODE` - Enables the LogStatus method calls

### File-Level Symbol Definitions
In `Program.cs`, these symbols are defined with `#define` (file-specific):
- `DEBUG_MODE` - Enables debug-specific features
- `TESTING_ENABLED` - Enables testing features
- `LOGGINGMODE` - Overrides project-level setting for this file

## Build Configuration Scenarios

### 1. Full Development Build
**Purpose:** Maximum features for development and debugging
```xml
<DefineConstants>DEVELOPMENT;LOGGING;PERFORMANCE_METRICS;LOGGINGMODE;TESTING_ENABLED</DefineConstants>
```
**Features Enabled:**
- Complete input validation
- Detailed debug logging
- Performance metrics collection
- Testing and verification features
- Development-only caching

### 2. Production Build
**Purpose:** Optimized for performance with minimal overhead
```xml
<DefineConstants>LOGGING</DefineConstants>
```
**Features Enabled:**
- Basic logging only
- No development overhead
- No performance metrics
- No testing features

### 3. Testing Build
**Purpose:** Optimized for testing scenarios
```xml
<DefineConstants>DEVELOPMENT;LOGGING;TESTING_ENABLED;PERFORMANCE_METRICS</DefineConstants>
```
**Features Enabled:**
- Development features
- Testing and verification
- Performance monitoring
- Detailed logging

### 4. Legacy Support Build
**Purpose:** Backward compatibility with older systems
```xml
<DefineConstants>LEGACY_SUPPORT;LOGGING;DEVELOPMENT</DefineConstants>
```
**Features Enabled:**
- Legacy API implementations
- Older algorithm versions
- Backward compatibility features
- Basic logging

### 5. Version 2 Build
**Purpose:** Modern features and optimizations
```xml
<DefineConstants>V2;LOGGING;PERFORMANCE_METRICS;DEVELOPMENT</DefineConstants>
```
**Features Enabled:**
- Modern type aliases (Dictionary vs Hashtable)
- Latest API implementations
- Enhanced performance features
- Modern algorithms

### 6. Minimal Build
**Purpose:** Absolute minimum for embedded/resource-constrained environments
```xml
<DefineConstants></DefineConstants>
```
**Features Enabled:**
- Core functionality only
- No logging overhead
- No development features
- Maximum performance

## Using Build Configurations

### Method 1: Project File Configuration
Edit the `.csproj` file directly:
```xml
<PropertyGroup>
    <DefineConstants>DEVELOPMENT;LOGGING;PERFORMANCE_METRICS</DefineConstants>
</PropertyGroup>
```

### Method 2: Configuration-Specific Symbols
Different symbols for Debug vs Release:
```xml
<PropertyGroup Condition="'$(Configuration)' == 'Debug'">
    <DefineConstants>$(DefineConstants);DEBUG_EXTRA;DETAILED_LOGGING</DefineConstants>
</PropertyGroup>

<PropertyGroup Condition="'$(Configuration)' == 'Release'">
    <DefineConstants>$(DefineConstants);OPTIMIZE;PRODUCTION</DefineConstants>
</PropertyGroup>
```

### Method 3: Command Line Build
Override symbols via command line:
```bash
# Development build
dotnet build -p:DefineConstants="DEVELOPMENT;LOGGING;PERFORMANCE_METRICS;LOGGINGMODE"

# Production build
dotnet build -c Release -p:DefineConstants="LOGGING"

# Testing build
dotnet build -p:DefineConstants="DEVELOPMENT;TESTING_ENABLED;LOGGING"

# Legacy support
dotnet build -p:DefineConstants="LEGACY_SUPPORT;LOGGING"

# Minimal build
dotnet build -c Release -p:DefineConstants=""
```

### Method 4: Using the Demo Script
The project includes a PowerShell script for testing different configurations:
```powershell
# Build all configurations
.\build-configurations.ps1

# Interactive mode
.\build-configurations.ps1 -Interactive

# Show side-by-side comparisons
.\build-configurations.ps1 -ShowDifferences

# Use Release configuration
.\build-configurations.ps1 -Configuration Release
```

## File-Level Symbol Management

### Defining Symbols in Source Files
```csharp
// Must be at the top of the file, before any actual code
#define DEBUG_MODE
#define TESTING_ENABLED
#define LOGGINGMODE
// #define LEGACY_SUPPORT  // Commented out to disable
```

### Undefining Symbols
```csharp
// Remove a symbol that was defined at project level
#undef PERFORMANCE_METRICS
```

## Practical Applications

### 1. API Versioning
```csharp
using TestType =
#if V2
    System.Collections.Generic.Dictionary<string, object>;
#else
    System.Collections.Hashtable;
#endif
```

### 2. Platform-Specific Code
```csharp
#if WINDOWS
    // Windows-specific implementation
#elif LINUX
    // Linux-specific implementation
#else
    // Cross-platform implementation
#endif
```

### 3. Feature Flags
```csharp
#if EXPERIMENTAL_FEATURES
    // New, experimental code
#else
    // Stable, proven code
#endif
```

### 4. Debug vs Release Behavior
```csharp
#if DEBUG
    // Debug-specific validation and logging
#else
    // Optimized release code
#endif
```

## Best Practices

### 1. Symbol Naming Conventions
- Use UPPERCASE for symbols
- Use descriptive names (e.g., `LEGACY_SUPPORT` not `LEGACY`)
- Group related symbols with prefixes (e.g., `FEATURE_CACHING`, `FEATURE_LOGGING`)

### 2. Documentation
- Document what each symbol enables/disables
- Explain the purpose of each build configuration
- Provide examples of when to use each configuration

### 3. Testing
- Test all major build configurations
- Use the demo script to verify behavior changes
- Ensure critical features work in all relevant configurations

### 4. Performance Considerations
- Use compile-time decisions for performance-critical code
- Reserve runtime decisions for user-configurable features
- Measure the impact of different configurations

## Troubleshooting

### Common Issues
1. **Symbol not defined:** Check spelling and case sensitivity
2. **Wrong configuration:** Verify which symbols are actually defined
3. **File vs project symbols:** Remember file-level symbols override project-level

### Debugging Tips
- Use `#if DEBUG` to include debug-only code
- Add `#warning` messages to verify symbol definitions
- Use the demo script to test different configurations

## Learning Exercises

1. **Modify symbol definitions** and observe behavior changes
2. **Create custom build configurations** for specific scenarios
3. **Practice type aliasing** with different implementations
4. **Implement feature flags** for experimental features
5. **Combine compile-time and runtime** approaches effectively

This guide provides the foundation for understanding and implementing conditional compilation in real-world C# projects, following industry best practices and demonstrating all concepts from the training material.
3. Set different conditional compilation symbols for each

## Symbol Effects

| Symbol | Effect |
|--------|--------|
| `DEVELOPMENT` | Enables development logging and features |
| `DEBUG_MODE` | Enables detailed debug output and file logging |
| `LOGGING` | Enables general application logging |
| `PERFORMANCE_METRICS` | Includes performance measurement overhead |
| `TESTING_ENABLED` | Enables testing-specific features |
| `LEGACY_SUPPORT` | Uses older, compatible API implementations |

## Best Practices

1. **Use symbols sparingly** - Too many can make code hard to follow
2. **Document your symbols** - Always explain what each symbol does
3. **Test all configurations** - Make sure your app works with different symbol combinations
4. **Consider runtime flags** - For features that need to be toggled without recompilation
5. **Performance considerations** - Conditional attributes have zero runtime cost when disabled
