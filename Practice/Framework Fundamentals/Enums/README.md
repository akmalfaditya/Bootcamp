# C# Enums

## Learning Objectives

Learn the complete C# enum system, from basic usage to advanced flags operations and real-world applications. This comprehensive project covers every aspect of working with enumerations in modern C# development.

## What You'll Learn

### Core Enum Concepts
- **Enum fundamentals** - Understanding enums as named constants
- **Underlying types** - How enums relate to integral types (int, byte, long)
- **Value assignments** - Explicit vs implicit enum value assignment
- **Enum comparisons** - Ordering and equality operations

### Type Conversions and Parsing
- **Enum ↔ Integer** conversions with casting and safety
- **Enum ↔ String** conversions with formatting options
- **String parsing** with `Enum.Parse()` and `TryParse()` methods
- **Generic conversion** utilities for any enum type
- **Type safety** considerations and validation patterns

### Flags Enums (Bitwise Operations)
- **`[Flags]` attribute** and its significance
- **Bitwise operations** - OR, AND, XOR, NOT for combining values
- **Flag checking** with `HasFlag()` and bitwise techniques
- **Complex flag combinations** and manipulation patterns
- **Performance considerations** for flags operations

### Advanced Enum Techniques
- **Enum enumeration** - Getting all values and names
- **Reflection with enums** - Dynamic enum operations
- **Validation patterns** - Ensuring enum value integrity
- **Generic enum utilities** - Building reusable enum helpers
- **State machines** with enums for business logic

## Key Features Demonstrated

### 1. Basic Enum Operations
```csharp
public enum Priority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

// Usage examples
Priority taskPriority = Priority.High;
string description = taskPriority switch
{
    Priority.Low => "Can wait for next sprint",
    Priority.High => "Needs attention soon",
    // ... more cases
};
```

### 2. Comprehensive Type Conversions
```csharp
// Enum to integer
int priorityValue = (int)Priority.Critical;

// Integer to enum (with validation)
if (Enum.IsDefined(typeof(Priority), 3))
{
    Priority priority = (Priority)3;
}

// String parsing with error handling
if (Enum.TryParse<Priority>("High", true, out Priority result))
{
    // Successfully parsed
}
```

### 3. Flags Enum Mastery
```csharp
[Flags]
public enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4,
    Delete = 8,
    FullControl = Read | Write | Execute | Delete
}

// Combining and checking flags
var permissions = FilePermissions.Read | FilePermissions.Write;
bool canRead = permissions.HasFlag(FilePermissions.Read);
```

### 4. Real-World Applications
- **Logging systems** with log levels
- **HTTP status code** handling
- **Feature flag** management
- **State machines** for order processing
- **Configuration categories** for application settings

## Tips

### **Understanding Enum Fundamentals**
Enums are essentially named integer constants. Always remember that enum values have underlying numeric values, which affects comparisons, storage, and serialization.

### **Safe Conversion Patterns**
Never trust external input when converting to enums. Always use validation:
```csharp
// Dangerous - no validation
Priority priority = (Priority)userInput;

// Safe - with validation
if (Enum.IsDefined(typeof(Priority), userInput))
{
    Priority priority = (Priority)userInput;
}
```

### **Flags Design Guidelines**
- Use powers of 2 for flag values (1, 2, 4, 8, 16...)
- Always include a "None = 0" value
- Consider common combinations as named constants
- Use meaningful names that work together

### **Performance Considerations**
- `HasFlag()` is convenient but slower than bitwise operations
- For high-performance scenarios, use `(flags & target) != 0`
- Enum comparisons are fast - they're just integer comparisons

### **Common Pitfalls to Avoid**
1. **Invalid enum values** - Casting can create undefined enum values
2. **Arithmetic operations** - Adding/subtracting enums may not make logical sense
3. **Flag combinations** - Not all bit combinations may be valid business values
4. **Serialization issues** - Enum values can break if underlying values change

## Code Structure

```
Program.cs
├── Basic Enum Operations
├── Enum ↔ Integral Conversions
├── Generic Enum Conversions
├── Enum ↔ String Conversions
├── String to Enum Parsing
├── Enumerating Enum Values
├── Flags Enums Basics
├── Advanced Flags Operations
├── Enum Limitations & Pitfalls
└── Real-World Scenarios
    ├── Logging System
    ├── HTTP API Responses
    ├── Feature Flag Management
    ├── State Machine Implementation
    └── Configuration Management
```

## Getting Started

1. **Run the application** to see comprehensive enum demonstrations
2. **Study each section** to understand different enum capabilities
3. **Experiment with modifications** to see how changes affect behavior
4. **Focus on real-world scenarios** to understand practical applications

## Practical Applications

### Enterprise Development
- **Configuration management** with categorized settings
- **User permission systems** with flags for fine-grained control
- **Workflow state machines** for business process automation
- **Logging and monitoring** with structured severity levels

### API Development
- **Response status codes** with meaningful enum representations
- **Error categorization** for consistent error handling
- **Feature toggles** for gradual feature rollouts
- **Validation states** for input processing pipelines

### User Interface
- **Dropdown/select options** generated from enum values
- **Status indicators** with consistent visual representations
- **Theme and preference** management
- **Multi-select controls** using flags for option combinations


## Integration with Modern C#

### Pattern Matching (C# 8+)
```csharp
string GetPriorityAction(Priority priority) => priority switch
{
    Priority.Critical => "Immediate action required",
    Priority.High => "Schedule for today",
    Priority.Medium => "Handle this week",
    Priority.Low => "Backlog item",
    _ => "Unknown priority"
};
```

### Generic Constraints (C# 7.3+)
```csharp
public static T ParseEnum<T>(string value) where T : struct, Enum
{
    return Enum.Parse<T>(value, ignoreCase: true);
}
```

### Nullable Enums
```csharp
Priority? optionalPriority = null;
if (optionalPriority.HasValue)
{
    ProcessPriority(optionalPriority.Value);
}
```

## Industry Impact

Understanding enums deeply is crucial for:
- **Domain modeling** - Representing business concepts clearly
- **API design** - Creating intuitive and maintainable interfaces
- **Database design** - Storing categorical data efficiently
- **System integration** - Handling external system status codes and categories
- **Performance optimization** - Using enums instead of strings for better performance


