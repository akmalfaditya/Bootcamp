# Interfaces in C#

## Learning Objectives

Master the art of creating flexible, testable, and maintainable code! Interfaces are contracts that define what a class can do without specifying how it does it, enabling powerful design patterns and loose coupling.

## What You'll Learn

### Core Concepts Covered:

1. **Interface Fundamentals**
   - What interfaces are and why they're essential
   - Interface declaration and implementation
   - Contract-based programming
   - Separation of "what" from "how"

2. **Interface Implementation**
   - **Implicit implementation**: Standard approach
   - **Explicit implementation**: Resolving conflicts
   - **Multiple interface implementation**: One class, many contracts
   - **Interface inheritance**: Building interface hierarchies

3. **Modern Interface Features (C# 8+)**
   - **Default interface members**: Shared implementations
   - **Static interface members**: Utility methods in interfaces
   - **Interface constraints**: Generic interface requirements

4. **Advanced Interface Patterns**
   - **Polymorphism**: Runtime type flexibility
   - **Dependency injection**: Loose coupling with IoC
   - **Strategy pattern**: Pluggable algorithms
   - **Repository pattern**: Data access abstraction

5. **Interface Design Principles**
   - Interface Segregation Principle (ISP)
   - Liskov Substitution Principle (LSP)
   - Designing for testability
   - API evolution strategies

## Key Features Demonstrated

### Basic Interface Pattern:
```csharp
// Interface defines the contract
public interface IDrawable
{
    void Draw();
    bool IsVisible { get; set; }
}

// Multiple implementations of the same contract
public class Circle : IDrawable
{
    public void Draw() => Console.WriteLine("Drawing a circle");
    public bool IsVisible { get; set; } = true;
}

public class Square : IDrawable
{
    public void Draw() => Console.WriteLine("Drawing a square");
    public bool IsVisible { get; set; } = true;
}
```

### Multiple Interfaces:
```csharp
public interface IMoveable
{
    void Move(int x, int y);
}

public interface IResizable
{
    void Resize(double factor);
}

// One class can implement multiple interfaces
public class GameObject : IDrawable, IMoveable, IResizable
{
    public void Draw() { /* implementation */ }
    public void Move(int x, int y) { /* implementation */ }
    public void Resize(double factor) { /* implementation */ }
    public bool IsVisible { get; set; }
}
```

### Explicit Implementation:
```csharp
public interface IPrinter
{
    void Print();
}

public interface IScanner
{
    void Print(); // Same method name, different purpose
}

public class AllInOneDevice : IPrinter, IScanner
{
    // Explicit implementation resolves naming conflicts
    void IPrinter.Print() => Console.WriteLine("Printing document");
    void IScanner.Print() => Console.WriteLine("Printing scan job");
}
```

### Modern Interface Features (C# 8+):
```csharp
public interface ILogger
{
    void Log(string message);
    
    // Default implementation - all implementers get this for free
    void LogError(string message) => Log($"ERROR: {message}");
    
    // Static method - utility functionality
    static string FormatMessage(string message) => $"[{DateTime.Now}] {message}";
}
```

## Tips

> **Think Contracts, Not Implementation**: An interface is like a job description - it tells you what capabilities someone must have, but not how they acquired those skills. This abstraction is incredibly powerful!

> **Interface Segregation**: Many small, focused interfaces are better than one large interface. It's easier to implement `IReadable` and `IWritable` than `IFileOperations` with 20 methods.

> **Dependency Injection Magic**: Interfaces enable dependency injection, making your code testable and flexible. Instead of `new SqlRepository()`, use `IRepository repository` and inject the implementation.

## What to Focus On

1. **Contract definition**: Interfaces define capabilities, not implementation
2. **Polymorphism**: Treat different objects uniformly through interfaces
3. **Testability**: Interfaces make unit testing much easier
4. **Flexibility**: Swap implementations without changing client code

## Run the Project

```bash
dotnet run
```

The demo includes:
- Basic interface implementation patterns
- Multiple interface scenarios
- Explicit implementation examples
- Modern C# 8+ interface features
- Real-world design patterns
- Polymorphism in action

## Best Practices

1. **Keep interfaces small and focused** (Interface Segregation Principle)
2. **Use explicit implementation** only when necessary (naming conflicts)
3. **Design for evolution** - interfaces are hard to change once published
4. **Prefer composition over inheritance** using interfaces
5. **Use meaningful names** that describe capabilities (`IDisposable`, `IComparable`)
6. **Document interface contracts** clearly

## Real-World Applications

### Essential .NET Interfaces:
- **`IDisposable`**: Resource cleanup pattern
- **`IEnumerable<T>`**: Iteration support
- **`IComparable<T>`**: Object comparison
- **`IEquatable<T>`**: Value equality
- **`ICloneable`**: Object copying

### Design Patterns with Interfaces:
- **Repository Pattern**: `IRepository<T>` for data access
- **Strategy Pattern**: `IPaymentProcessor` for different payment methods
- **Factory Pattern**: `IFactory<T>` for object creation
- **Observer Pattern**: `IObserver<T>` for notifications
- **Command Pattern**: `ICommand` for encapsulating operations

### Architecture Benefits:
- **Dependency Injection**: Loose coupling between components
- **Unit Testing**: Mock interfaces for isolated testing
- **Plugin Architecture**: Load implementations at runtime
- **API Design**: Stable contracts for library consumers

## When to Use Interfaces

**Perfect for:**
- Defining contracts between components
- Enabling dependency injection
- Creating testable code
- Building plugin architectures
- Implementing design patterns

**Consider alternatives when:**
- You need shared implementation (use abstract classes)
- The relationship is clearly "is-a" (use inheritance)
- You're building simple, internal-only code
- Performance is absolutely critical

## Advanced Interface Concepts

### Interface Hierarchies:
```csharp
public interface IShape
{
    double Area { get; }
}

public interface IColoredShape : IShape
{
    Color Color { get; set; }
}

public interface I3DShape : IShape
{
    double Volume { get; }
}
```

### Generic Interfaces:
```csharp
public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task SaveAsync(T entity);
    Task DeleteAsync(int id);
}
```
