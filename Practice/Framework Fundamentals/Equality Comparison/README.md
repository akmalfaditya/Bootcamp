# Equality Comparison in C#

## Learning Objectives
Learn the complex but crucial concept of **equality comparison** in C# - understanding when objects are considered equal, the different types of equality, and how to implement robust equality logic in your own types. This knowledge is essential for collections, LINQ operations, and creating reliable applications.

## What You'll Learn

### Fundamental Equality Concepts
- **Value vs Reference Equality**: Understanding when content matters vs when identity matters
- **Operator Overloading**: How `==` and `!=` operators work with different types
- **Equals() Method**: The virtual method that forms the foundation of equality
- **Object.ReferenceEquals()**: Testing for identical object references

### Advanced Equality Techniques
- **IEquatable<T> Interface**: Strongly-typed equality for better performance
- **Hash Code Contracts**: The critical relationship between equality and hash codes
- **Null Handling**: Safe equality comparisons that handle null values gracefully
- **String Equality**: Special considerations for string comparison and culture

### Custom Equality Implementation
- **Value Type Equality**: Implementing equality for structs and records
- **Reference Type Equality**: Creating semantic equality for classes
- **Composite Equality**: Handling objects with multiple properties
- **Performance Optimization**: Efficient equality checks for complex objects

## Key Features Demonstrated

### 1. **Basic Equality Patterns**
```csharp
// Reference equality - are they the same object?
bool sameReference = ReferenceEquals(obj1, obj2);

// Value equality - do they have the same content?
bool sameValue = obj1.Equals(obj2);

// Operator equality - depends on type implementation
bool operatorEqual = obj1 == obj2;
```

### 2. **String Equality Nuances**
```csharp
string s1 = "Hello";
string s2 = "HELLO";

// Case-sensitive comparison
bool exact = s1.Equals(s2);  // false

// Case-insensitive comparison
bool ignoreCase = s1.Equals(s2, StringComparison.OrdinalIgnoreCase);  // true
```

### 3. **Custom Equality Implementation**
```csharp
public class Person : IEquatable<Person>
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public bool Equals(Person other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Age == other.Age;
    }
    
    public override bool Equals(object obj) => Equals(obj as Person);
    
    public override int GetHashCode() => HashCode.Combine(Name, Age);
    
    public static bool operator ==(Person left, Person right) 
        => EqualityComparer<Person>.Default.Equals(left, right);
    
    public static bool operator !=(Person left, Person right) => !(left == right);
}
```

### 4. **Null-Safe Comparisons**
```csharp
// Safe comparison that handles nulls correctly
public static bool SafeEquals<T>(T left, T right) where T : class
{
    if (ReferenceEquals(left, right)) return true;
    if (left is null || right is null) return false;
    return left.Equals(right);
}
```

## Tips

### **The Golden Rules of Equality**
1. **Reflexive**: `x.Equals(x)` must always return `true`
2. **Symmetric**: If `x.Equals(y)` returns `true`, then `y.Equals(x)` must return `true`
3. **Transitive**: If `x.Equals(y)` and `y.Equals(z)` return `true`, then `x.Equals(z)` must return `true`
4. **Consistent**: Multiple calls to `x.Equals(y)` should return the same result
5. **Null Handling**: `x.Equals(null)` must return `false` for non-null `x`

### **Hash Code Contract**
```csharp
// CRITICAL: If two objects are equal, they MUST have the same hash code
if (obj1.Equals(obj2))
{
    Debug.Assert(obj1.GetHashCode() == obj2.GetHashCode());
}

// However, objects with the same hash code don't have to be equal
// (hash collisions are allowed and expected)
```

### **Performance Considerations**
- Implement `IEquatable<T>` to avoid boxing in generic collections
- Use short-circuit evaluation in equality methods
- Consider reference equality check first for reference types
- Cache hash codes for expensive-to-compute scenarios

### **Common Pitfalls**
- **Don't**: Forget to override `GetHashCode()` when overriding `Equals()`
- **Do**: Always implement both methods together
- **Don't**: Use mutable properties in hash code calculation
- **Do**: Base hash codes on immutable data when possible

## Real-World Applications

### **Entity Framework & ORM**
```csharp
public class Customer : IEquatable<Customer>
{
    public int Id { get; set; }
    public string Email { get; set; }
    
    // Business equality based on email, not database ID
    public bool Equals(Customer other) 
        => other != null && Email?.ToLower() == other.Email?.ToLower();
    
    public override int GetHashCode() 
        => Email?.ToLower().GetHashCode() ?? 0;
}
```

### **Caching Systems**
```csharp
// Cache keys need reliable equality and hash codes
public class CacheKey : IEquatable<CacheKey>
{
    public string Operation { get; }
    public string[] Parameters { get; }
    
    public bool Equals(CacheKey other)
    {
        return other != null && 
               Operation == other.Operation &&
               Parameters.SequenceEqual(other.Parameters);
    }
    
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Operation);
        foreach (string param in Parameters)
            hash.Add(param);
        return hash.ToHashCode();
    }
}
```

### **Financial Systems**
```csharp
public struct Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public bool Equals(Money other)
    {
        // Same currency and amount (within tolerance for floating point)
        return Currency == other.Currency && 
               Math.Abs(Amount - other.Amount) < 0.01m;
    }
}
```

### **Configuration Management**
```csharp
public class ConfigSection : IEquatable<ConfigSection>
{
    public string Name { get; set; }
    public Dictionary<string, string> Settings { get; set; }
    
    public bool Equals(ConfigSection other)
    {
        if (other == null || Name != other.Name) return false;
        
        // Deep comparison of dictionaries
        return Settings.Count == other.Settings.Count &&
               Settings.All(kvp => other.Settings.TryGetValue(kvp.Key, out string value) && 
                                  value == kvp.Value);
    }
}
```


## Integration with Modern C#

### **Records (C# 9+)**
```csharp
// Records provide automatic equality implementation
public record Person(string Name, int Age);

// Equivalent to a full equality implementation
var person1 = new Person("John", 30);
var person2 = new Person("John", 30);
Console.WriteLine(person1 == person2);  // True
```

### **Pattern Matching (C# 7+)**
```csharp
public bool IsSpecialCustomer(Customer customer) => customer switch
{
    { Status: "VIP", YearsActive: > 5 } => true,
    { TotalSpent: > 10000 } => true,
    _ => false
};
```

### **HashCode.Combine (C# 2.1+)**
```csharp
public override int GetHashCode() 
    => HashCode.Combine(Name, Age, Email, Department);
```

## Industry Impact

Equality comparison is crucial because it:

- **Powers Collections**: HashSet, Dictionary, and LINQ operations rely on proper equality
- **Enables Caching**: Cache systems need reliable equality to function correctly
- **Supports Testing**: Unit tests depend on equality for assertions and comparisons
- **Drives Performance**: Efficient equality implementations improve application speed
- **Ensures Correctness**: Proper equality prevents subtle bugs in business logic

