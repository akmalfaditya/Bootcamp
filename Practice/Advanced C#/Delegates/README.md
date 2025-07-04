# Delegates in C#

## Overview

Delegates represent one of the most powerful features in C#, providing type-safe function pointers that enable flexible and maintainable code design. This project demonstrates comprehensive delegate usage patterns essential for modern C# development.

## Core Concepts

### 1. Basic Delegate Usage

A delegate is a type that defines the signature of methods it can reference. Think of it as a contract specifying the return type and parameters that compatible methods must have.

**Key Points:**
- Delegates provide indirection between method callers and implementations
- Method assignment uses implicit conversion: `Transformer t = Square;`
- Invocation syntax matches regular method calls: `int result = t(5);`
- Explicit invocation is also supported: `t.Invoke(5);`

**Demonstration:** The project shows basic delegate declaration, assignment, and invocation with different methods (Square, Cube) using the same delegate type.

### 2. Plugin Methods with Delegates

Delegates enable the creation of higher-order functions that accept other functions as parameters, facilitating plugin-based architectures.

**Key Points:**
- Methods can accept delegates as parameters to customize behavior
- The same algorithm can work with different transformation logic
- Lambda expressions can be passed as inline plugins
- This pattern supports the Strategy design pattern implementation

**Demonstration:** The Transform method accepts any compatible delegate, allowing different transformations (square, cube, addition) to be applied to arrays.

### 3. Instance and Static Method Targets

Delegates can reference both static methods and instance methods, with important differences in how they store references.

**Key Points:**
- Static method delegates store only the method reference
- Instance method delegates store both the method and object instance
- The Target property reveals the referenced object (null for static methods)
- Instance delegates keep objects alive through strong references

**Demonstration:** Shows delegation to static methods and instance methods, displaying how delegates maintain object references.

### 4. Multicast Delegates

Multicast capability allows a single delegate to reference multiple methods, executing them sequentially when invoked.

**Key Points:**
- Multiple methods are combined using += operator
- Methods are removed using -= operator  
- Delegates are immutable; operators create new instances
- Return values: only the last method's return value is preserved
- Best suited for void-returning methods (like event handlers)

**Demonstration:** Progress reporting system where multiple logging methods are combined and executed together.

### 5. Generic Delegate Types

Generic delegates provide maximum reusability by working with any specified type, eliminating the need for multiple delegate declarations.

**Key Points:**
- Type parameters enable delegates to work with various data types
- Single delegate definition supports multiple use cases
- Generic constraints can be applied for type safety
- Performance equivalent to non-generic versions after compilation

**Demonstration:** A generic Transformer delegate works with integers, strings, and other types, showing versatility in a Transform utility method.

### 6. Func and Action Delegates

The .NET Framework provides built-in generic delegates that cover most common scenarios, reducing the need for custom delegate declarations.

**Key Points:**
- **Func delegates:** Return values with up to 16 parameters
- **Action delegates:** Void return with up to 16 parameters
- Standardized approach across the .NET ecosystem
- Preferred over custom delegates in most situations

**Demonstration:** Various Func and Action usages including parameterless, single-parameter, and multi-parameter scenarios.

### 7. Delegates versus Interfaces

Understanding when to use delegates versus interfaces is crucial for proper architectural decisions.

**Delegates are preferred when:**
- Single method contracts are sufficient
- Multicast capability is needed
- Multiple implementations from one class are required
- Functional programming approaches are desired

**Interfaces are preferred when:**
- Multiple related methods need grouping
- Object-oriented inheritance relationships exist
- Complex contracts with properties and events are needed

**Demonstration:** Side-by-side comparison showing equivalent functionality implemented with both approaches.

### 8. Delegate Compatibility

Delegates follow strict type compatibility rules while supporting variance in specific scenarios.

**Key Points:**
- Delegate types with identical signatures are still incompatible
- Explicit construction enables conversion between delegate types
- Delegate equality depends on method targets and invocation order
- Multicast delegates with different method chains are not equal

**Demonstration:** Shows type safety enforcement and equality comparison behaviors.

### 9. Parameter Compatibility (Contravariance)

Contravariance allows delegates to accept methods with more general parameter types than specified.

**Key Points:**
- Method parameters can be more general than delegate parameters
- String-specific delegate can point to object-accepting method
- Enables flexible event handling and callback scenarios
- Arguments are implicitly upcast during invocation

**Demonstration:** Action<string> delegate successfully references a method accepting object parameters.

### 10. Return Type Compatibility (Covariance)

Covariance permits delegates to reference methods returning more specific types than declared.

**Key Points:**
- Method return types can be more specific than delegate return types
- Returned values are implicitly upcast to delegate's return type
- Supports factory patterns and polymorphic scenarios
- Maintains type safety through inheritance relationships

**Demonstration:** Func<object> delegate references methods returning strings and other specific types.

### 11. Real-World Implementation

The file processing system demonstrates practical delegate usage in a business context.

**Key Points:**
- Strategy pattern implementation using delegates
- Event notifications through multicast delegates
- Plugin architecture for different processing algorithms
- Separation of concerns between processing logic and business rules

**Demonstration:** File processor that accepts different processing strategies and reports progress through events.

## Implementation Examples

### Basic Delegate Declaration and Usage
```csharp
// Custom delegate type definition
delegate int Transformer(int x);

// Method assignment and invocation
Transformer t = Square;
int result = t(3);  // Calls Square(3)
```

### Plugin Architecture with Delegates
```csharp
// Higher-order function accepting delegates
static void Transform(int[] values, Transformer t)
{
    for (int i = 0; i < values.Length; i++)
        values[i] = t(values[i]);
}

// Usage with different strategies
Transform(numbers, Square);     // Apply square transformation
Transform(numbers, x => x * 2); // Apply doubling transformation
```

### Multicast Delegate Operations
```csharp
// Combining multiple methods
ProgressReporter reporter = WriteToConsole;
reporter += WriteToFile;    // Add second method
reporter += SendAlert;      // Add third method

reporter(50);  // Executes all three methods in sequence
```

### Generic Delegates for Reusability
```csharp
// Generic delegate supporting any type
delegate TResult Transformer<TArg, TResult>(TArg arg);

// Type-specific instantiations
Transformer<int, int> intSquarer = x => x * x;
Transformer<string, int> stringLength = s => s.Length;
```

### Built-in Func and Action Delegates
```csharp
// Func delegates (with return values)
Func<int, int> squareFunc = x => x * x;
Func<int, int, int> addFunc = (a, b) => a + b;

// Action delegates (void return)
Action<string> logger = Console.WriteLine;
Action<int, string> complexAction = (num, text) => 
    Console.WriteLine($"Number: {num}, Text: {text}");
```

## Execution

To run the demonstration:
```bash
dotnet run
```

## Learning Outcomes

Upon completing this project, developers will understand:

1. **Type Safety Benefits:** How delegates provide compile-time checking while enabling runtime flexibility
2. **Architectural Patterns:** Implementation of Strategy pattern and plugin architectures using delegates
3. **Memory Management:** How delegate references affect object lifetime and garbage collection
4. **Performance Considerations:** When delegate overhead is acceptable versus direct method calls
5. **Variance Rules:** How contravariance and covariance enable flexible delegate assignments
6. **Best Practices:** When to prefer delegates over interfaces and vice versa

## Advanced Concepts Demonstrated

### Delegate Type Compatibility
```csharp
delegate void D1();
delegate void D2();

D1 d1 = Method;
D2 d2 = new D2(d1);  // Explicit conversion required
```

### Parameter Contravariance
```csharp
void ProcessObject(object obj) { }
Action<string> stringAction = ProcessObject;  // Valid assignment
```

### Return Type Covariance
```csharp
string GetString() => "text";
Func<object> objectGetter = GetString;  // Valid assignment
```

## Professional Applications

Delegates are fundamental to:
- **Event-driven programming:** Foundation for C# events and notifications
- **Asynchronous programming:** Callback mechanisms in async operations
- **Functional programming:** Higher-order functions and LINQ operations
- **Framework design:** Extensibility points in libraries and frameworks
- **Plugin architectures:** Runtime behavior customization without inheritance

## Best Practices

1. **Prefer built-in delegates** (Action, Func) over custom declarations when signatures match
2. **Always perform null checks** before delegate invocation to prevent runtime exceptions
3. **Use meaningful names** for delegate types that clearly indicate their purpose
4. **Consider memory implications** when storing delegates that reference instance methods
5. **Document variance behavior** when creating generic delegates with in/out parameters
6. **Favor composition over inheritance** when delegates can provide the same flexibility
