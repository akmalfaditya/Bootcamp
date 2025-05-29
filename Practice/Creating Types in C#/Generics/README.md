# Generics in C#

## 🎯 Learning Objectives

Welcome to the world of type-safe, reusable code! Generics are one of C#'s most powerful features, allowing you to write code once and use it with any type while maintaining compile-time type safety.

## 📚 What You'll Learn

### Core Concepts Covered:

1. **Generic Fundamentals**
   - What generics are and why they exist
   - Type parameters and type arguments
   - Generic classes, interfaces, and methods
   - Compile-time type safety benefits

2. **Generic Types**
   - **Generic classes**: `List<T>`, `Dictionary<TKey, TValue>`
   - **Generic interfaces**: `IEnumerable<T>`, `IComparer<T>`
   - **Generic structs**: Value types with type parameters
   - **Generic delegates**: `Action<T>`, `Func<T, TResult>`

3. **Generic Constraints**
   - **Reference type constraints**: `where T : class`
   - **Value type constraints**: `where T : struct`
   - **Constructor constraints**: `where T : new()`
   - **Base class constraints**: `where T : BaseClass`
   - **Interface constraints**: `where T : IInterface`
   - **Multiple constraints**: Combining constraint types

4. **Variance in Generics**
   - **Covariance**: `out T` - producing T (read-only)
   - **Contravariance**: `in T` - consuming T (write-only)
   - **Invariance**: Default behavior for mutable types
   - Safe variance assignment rules

5. **Advanced Generic Features**
   - Generic method type inference
   - Nested generic types
   - Generic type testing and casting
   - Default values for type parameters

## 🚀 Key Features Demonstrated

### Generic Class Example:
```csharp
// One class definition works with ANY type
public class Stack<T>
{
    private T[] items = new T[10];
    private int count = 0;
    
    public void Push(T item) => items[count++] = item;
    public T Pop() => items[--count];
    public bool IsEmpty => count == 0;
}

// Usage with different types
var intStack = new Stack<int>();        // Stack of integers
var stringStack = new Stack<string>();  // Stack of strings
var personStack = new Stack<Person>();  // Stack of custom objects
```

### Generic Constraints:
```csharp
// Only reference types that implement IComparable
public class SortedList<T> where T : class, IComparable<T>
{
    public void Add(T item) { /* type-safe sorting */ }
}

// Only value types with parameterless constructor
public class Repository<T> where T : struct, new()
{
    public T CreateDefault() => new T();
}

// Multiple constraints
public class Manager<T> where T : Employee, IManageable, new()
{
    public T CreateEmployee() => new T();
}
```

### Variance Examples:
```csharp
// Covariance - can return more derived types
IEnumerable<string> strings = new List<string>();
IEnumerable<object> objects = strings; // Safe - reading only

// Contravariance - can accept less derived types
Action<object> objectAction = obj => Console.WriteLine(obj);
Action<string> stringAction = objectAction; // Safe - writing only
```

## 💡 Trainer Tips

> **Think of Generics as Templates**: You're creating a template that the compiler fills in with specific types. This happens at compile time, so you get full type safety with zero runtime cost!

> **Constraint Strategy**: Start with no constraints and add them only when you need specific capabilities. Each constraint limits which types can be used but unlocks specific functionality.

> **Variance Memory Aid**: 
> - **Co**variant = **Co**nsumer can **Co**me **O**ut (producer)
> - **Contra**variant = **Contra**ry direction, goes **In** (consumer)

## 🔍 What to Focus On

1. **Type safety**: Generics catch type errors at compile time
2. **Performance**: No boxing/unboxing with value types
3. **Code reuse**: Write once, use with many types
4. **Constraint design**: Balance flexibility with functionality

## 🏃‍♂️ Run the Project

```bash
dotnet run
```

The demo showcases:
- Basic generic types and methods
- All constraint types with examples
- Variance scenarios and safety
- Real-world generic implementations
- Performance comparisons

## 🎓 Best Practices

1. **Use meaningful type parameter names**: `T` for single type, `TKey`/`TValue` for pairs
2. **Add constraints only when needed**: Don't over-constrain your generics
3. **Prefer composition over inheritance** with generics
4. **Use generic collections** instead of non-generic ones
5. **Consider variance** when designing interfaces
6. **Document constraint requirements** clearly

## 🔧 Real-World Applications

### Common Generic Patterns:
- **Repository Pattern**: `IRepository<T>` for data access
- **Factory Pattern**: `IFactory<T>` for object creation
- **Command Pattern**: `ICommand<T>` for parameterized commands
- **Strategy Pattern**: `IStrategy<T>` for algorithmic flexibility
- **Observer Pattern**: `IObserver<T>` for type-safe notifications

### Industry Examples:
- **Collections**: `List<T>`, `Dictionary<TKey, TValue>`
- **LINQ**: `IEnumerable<T>`, `IQueryable<T>`
- **Async Programming**: `Task<T>`, `TaskCompletionSource<T>`
- **Dependency Injection**: `IServiceProvider`, `IOptions<T>`
- **Entity Framework**: `DbSet<T>`, `IQueryable<T>`

## 🎯 When to Use Generics

✅ **Perfect for:**
- Collections and data structures
- Algorithms that work with multiple types
- Type-safe wrapper classes
- Generic interfaces for abstraction
- Performance-critical code with value types

❌ **Avoid when:**
- Logic is truly type-specific
- Constraints would be too complex
- Simple inheritance would be clearer
- Only used with one type (unless future expansion planned)

## 🔮 Advanced Generic Concepts

Coming up in your journey:
- **Generic type inference**: Method calls without explicit types
- **Reflection with generics**: Runtime type manipulation
- **Expression trees**: Generics in LINQ providers
- **Compiler optimizations**: How generics improve performance

## 🎯 Mastery Checklist

After this project, you should confidently:
- ✅ Create generic classes, interfaces, and methods
- ✅ Apply appropriate constraints for your scenarios
- ✅ Understand and use variance correctly
- ✅ Design type-safe APIs using generics
- ✅ Choose between different constraint options
- ✅ Optimize performance using generic collections
- ✅ Implement common generic patterns

## 💼 Career Impact

Generics are everywhere in professional C# development:
- **Framework APIs**: Most .NET APIs use generics extensively
- **Library Design**: Essential for creating reusable components
- **Performance Optimization**: Critical for high-performance applications
- **Type Safety**: Reduces runtime errors and improves code quality

## 🔗 Connection to Other Concepts

Generics work seamlessly with:
- **Collections**: Type-safe data structures
- **LINQ**: Generic extension methods and queries
- **Async/Await**: `Task<T>` and generic async patterns
- **Dependency Injection**: Generic service registration
- **Reflection**: Runtime generic type manipulation

Remember: Generics are about writing flexible, type-safe, and reusable code. They're not just a language feature - they're a design philosophy that leads to better software architecture!
