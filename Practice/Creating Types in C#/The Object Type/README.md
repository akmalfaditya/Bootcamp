# The Object Type 🏛️

## 🎯 Learning Objectives

By mastering this project, you will:
- **Understand the Universal Base Class**: Learn how `object` is the root of all types in .NET
- **Master Boxing and Unboxing**: Handle value type to reference type conversions efficiently
- **Implement Type Checking**: Use runtime type inspection for safe casting and validation
- **Leverage Object Members**: Utilize inherited methods like `ToString()`, `Equals()`, and `GetHashCode()`
- **Build Type-Safe Collections**: Create flexible containers using the object type system
- **Optimize Performance**: Avoid common pitfalls with boxing and understand memory implications
- **Apply Polymorphism**: Use object as a universal container for mixed-type scenarios

## 📚 Core Concepts

### The Universal Base Class
The `object` type is the ultimate base class for all types in .NET. Every class, struct, interface, and built-in type inherits from `object`, creating a unified type system.

```csharp
// Everything inherits from object
object number = 42;           // int → object
object text = "Hello";        // string → object  
object date = DateTime.Now;   // DateTime → object
object array = new int[5];    // array → object
```

### Boxing and Unboxing
Boxing converts value types to reference types by wrapping them in an object on the heap. Unboxing extracts the value back to the stack.

```csharp
// Boxing: value type → heap allocation → object reference
int value = 42;
object boxed = value;  // Boxing creates new heap object

// Unboxing: object reference → extract value → stack value
int unboxed = (int)boxed;  // Must cast to exact original type
```

### Type Checking and Safety
Runtime type checking ensures safe conversions when working with object references.

```csharp
// Pattern matching (C# 7+)
if (mysteryObject is string text)
{
    Console.WriteLine($"It's a string: {text}");
}

// Safe casting with 'as' operator
int? maybeNumber = mysteryObject as int?;
if (maybeNumber != null)
{
    Console.WriteLine($"It's a number: {maybeNumber}");
}
```

## 🚀 Key Features

### 1. **Universal Container Pattern**
```csharp
// Store any type in the same container
object[] mixedBag = { 42, "Hello", DateTime.Now, new Person("John", 25) };

foreach (object item in mixedBag)
{
    Console.WriteLine($"Type: {item.GetType().Name}, Value: {item}");
}
```

### 2. **Generic Stack Implementation**
```csharp
public class SimpleStack
{
    private object[] items = new object[10];
    private int count = 0;
    
    public void Push(object item) => items[count++] = item;
    public object Pop() => items[--count];
}

// Usage with mixed types
var stack = new SimpleStack();
stack.Push("Text");
stack.Push(100);
stack.Push(DateTime.Now);
```

### 3. **Type Inspection**
```csharp
// GetType() - runtime type of instance
Console.WriteLine(person.GetType().Name);  // "Person"

// typeof() - compile-time type
Console.WriteLine(typeof(Person).Name);   // "Person"

// Detailed type information
Type type = person.GetType();
Console.WriteLine($"Namespace: {type.Namespace}");
Console.WriteLine($"Base Type: {type.BaseType?.Name}");
```

### 4. **Object Member Overrides**
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Override ToString() for meaningful string representation
    public override string ToString() => $"{Name} (Age: {Age})";
    
    // Override Equals() for value equality
    public override bool Equals(object obj) =>
        obj is Person other && Name == other.Name && Age == other.Age;
    
    // Override GetHashCode() for hash-based collections
    public override int GetHashCode() => HashCode.Combine(Name, Age);
}
```

### 5. **Performance Considerations**
```csharp
// ❌ Avoid: Boxing in loops creates many heap allocations
for (int i = 0; i < 1000; i++)
{
    object boxed = i;  // Boxing on each iteration
    list.Add(boxed);
}

// ✅ Better: Use generics to avoid boxing
var numbers = new List<int>();
for (int i = 0; i < 1000; i++)
{
    numbers.Add(i);  // No boxing needed
}
```

## 💡 Trainer Tips

### Boxing Performance Impact
- **Boxing creates heap objects**: Each boxing operation allocates memory on the heap
- **Use generics when possible**: `List<T>` avoids boxing compared to `ArrayList`
- **Watch for implicit boxing**: Some operations box values without obvious syntax

### Type Safety Best Practices
- **Always check types**: Use `is`, `as`, or pattern matching before casting
- **Cast to exact type**: Unboxing requires casting to the original boxed type
- **Handle InvalidCastException**: Wrap risky casts in try-catch blocks

### Object Member Implementation
- **Override ToString()**: Provide meaningful string representations for debugging
- **Implement Equals() and GetHashCode()**: Essential for collections and comparisons
- **Follow equality contract**: If `Equals()` returns true, `GetHashCode()` must return same value

## 🎓 Real-World Applications

### Legacy Code Integration
```csharp
// Working with older APIs that use object
public void ProcessLegacyData(object[] data)
{
    foreach (object item in data)
    {
        switch (item)
        {
            case string text:
                ProcessText(text);
                break;
            case int number:
                ProcessNumber(number);
                break;
            case DateTime date:
                ProcessDate(date);
                break;
            default:
                LogUnknownType(item);
                break;
        }
    }
}
```

### Configuration Systems
```csharp
// Flexible configuration storage
public class ConfigurationManager
{
    private Dictionary<string, object> settings = new();
    
    public void Set<T>(string key, T value) => settings[key] = value;
    
    public T Get<T>(string key)
    {
        if (settings.TryGetValue(key, out object value) && value is T result)
            return result;
        return default(T);
    }
}
```

### Serialization Scenarios
```csharp
// JSON deserialization to object first, then convert
public T Deserialize<T>(string json)
{
    object parsed = JsonSerializer.Deserialize<object>(json);
    
    if (parsed is JsonElement element)
    {
        return JsonSerializer.Deserialize<T>(element.GetRawText());
    }
    
    throw new InvalidOperationException("Invalid JSON format");
}
```

## 🎯 Mastery Checklist

### Beginner Level
- [ ] Store different types in object variables
- [ ] Understand boxing and unboxing basics
- [ ] Use `GetType()` to inspect object types
- [ ] Cast objects back to original types
- [ ] Handle `InvalidCastException` properly

### Intermediate Level
- [ ] Implement custom `ToString()` methods
- [ ] Use pattern matching for type checking
- [ ] Create heterogeneous collections with object
- [ ] Understand reference vs value equality
- [ ] Override `Equals()` and `GetHashCode()`

### Advanced Level
- [ ] Optimize code to minimize boxing
- [ ] Implement object-based generic containers
- [ ] Use reflection with Type objects
- [ ] Handle complex inheritance scenarios
- [ ] Design type-safe APIs using object judiciously

## 💼 Industry Impact

Understanding the object type system is crucial for:

**Framework Development**: Building flexible APIs that can handle any type
**Legacy System Maintenance**: Working with older codebases that predate generics
**Serialization Libraries**: Converting between different type representations
**ORM Development**: Mapping database values to strongly-typed objects
**Plugin Architectures**: Loading and working with unknown types at runtime

## 🔗 Integration with Modern C#

**Pattern Matching (C# 7+)**:
```csharp
string ProcessObject(object obj) => obj switch
{
    string s => $"String: {s}",
    int i => $"Integer: {i}",
    DateTime dt => $"Date: {dt:yyyy-MM-dd}",
    _ => "Unknown type"
};
```

**Records and Pattern Matching (C# 9+)**:
```csharp
public record PersonRecord(string Name, int Age);

// Pattern matching with records
if (obj is PersonRecord { Age: > 18 } adult)
{
    Console.WriteLine($"{adult.Name} is an adult");
}
```

---

*The object type is the foundation of .NET's unified type system. Master it to understand how all types relate and to build flexible, robust applications that can handle any data type with confidence!* 🏛️✨
