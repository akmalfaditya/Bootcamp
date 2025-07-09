# Plugging in Equality and Order in C#

## Overview
This project provides a comprehensive demonstration of custom equality and comparison operations in C#. You'll learn how to implement plug-in protocols that allow you to define custom behaviors for equality comparison, hashing, and ordering - essential skills for building robust collection-based applications.

## Learning Objectives
Master the implementation of custom comparers, understand how HashSet, Dictionary, and sorted collections use equality and comparison logic, and build robust collection operations that work exactly how your business logic requires.

## Core Concepts Explained

### 1. **Understanding the Equality Problem**

In C#, by default, reference types use reference equality, meaning two objects are considered equal only if they point to the same memory location. This behavior often conflicts with business logic requirements where objects should be considered equal based on their data content rather than their memory addresses.

**The Challenge:**
```csharp
var customer1 = new Customer("Smith", "John");
var customer2 = new Customer("Smith", "John");
Console.WriteLine(customer1.Equals(customer2)); // False - different memory addresses
```

Even though both customers have identical data, they are not considered equal because they are separate objects in memory. This creates problems when using these objects as keys in collections like Dictionary or HashSet, where logical equality is typically desired.

**Business Impact:**
- Dictionary lookups fail when searching with logically equivalent objects
- HashSet allows duplicate entries for objects that should be considered identical
- Sorting algorithms may produce inconsistent results based on memory addresses rather than meaningful data

### 2. **IEqualityComparer<T> Implementation**

The IEqualityComparer<T> interface provides a mechanism to define custom equality logic that collections can use instead of the default equality behavior. This interface requires implementation of two critical methods that work together to maintain consistency in hash-based collections.

**Core Interface Definition:**
```csharp
public interface IEqualityComparer<T>
{
    bool Equals(T x, T y);      // Defines equality logic
    int GetHashCode(T obj);     // Provides hash code for object
}
```

**The Hash Code Contract:**
The relationship between Equals and GetHashCode is fundamental to the correct operation of hash-based collections. If two objects are considered equal by the Equals method, they must produce identical hash codes. This contract ensures that hash-based collections can locate objects correctly.

**Implementation Strategy:**
When implementing custom equality comparers, extend the abstract EqualityComparer<T> class rather than implementing the interface directly. This approach provides better performance and handles non-generic compatibility automatically.

### 3. **EqualityComparer<T>.Default Behavior**

The EqualityComparer<T>.Default property provides an intelligent default equality comparer that automatically selects the most appropriate equality comparison method based on the type T. This selection process follows a specific hierarchy to ensure optimal performance and correctness.

**Selection Algorithm:**
1. If T implements IEquatable<T>, uses that implementation for type-safe, non-boxing equality
2. If T overrides Object.Equals, uses that implementation
3. Falls back to reference equality for reference types
4. Uses bitwise comparison for value types

**Performance Benefits:**
For value types, EqualityComparer<T>.Default avoids boxing operations that would occur with Object.Equals, resulting in significantly better performance. For reference types that implement IEquatable<T>, it provides type-safe comparison without casting.

**Generic Method Usage:**
This comparer is particularly valuable in generic methods where the specific type is unknown at compile time, allowing the method to use the most appropriate equality logic for any given type.

### 4. **ReferenceEqualityComparer (.NET 5+)**

ReferenceEqualityComparer provides a way to enforce reference-only equality comparison, regardless of whether a type has overridden Equals or implemented IEquatable<T>. This comparer is essential when object identity (same instance) is more important than object equality (same data).

**Use Cases:**
- Object lifetime tracking in caches or dependency injection containers
- Preventing memory leaks by ensuring weak references point to specific instances
- Debugging scenarios where you need to distinguish between object copies
- Performance optimization where reference comparison is faster than value comparison

**Behavior with Value Types:**
For value types, ReferenceEqualityComparer will always return false for Equals (since value types cannot share references) and provides a hash code based on the object's identity rather than its value.

### 5. **IComparer<T> for Custom Ordering**

The IComparer<T> interface enables definition of custom sorting logic for collections and algorithms. Unlike equality comparison, ordering comparison establishes a relationship between objects that determines their relative position in sorted collections.

**Comparison Contract:**
The Compare method must return:
- A negative value if x is less than y
- Zero if x equals y
- A positive value if x is greater than y

**Transitivity Requirement:**
The comparison logic must be transitive: if A < B and B < C, then A < C. Violating this rule can cause sorting algorithms to behave unpredictably or enter infinite loops.

**Multi-Criteria Sorting:**
Complex business objects often require sorting by multiple criteria. The standard pattern is to compare the primary criterion first, and only if objects are equal on that criterion, compare secondary criteria. This approach ensures consistent and predictable sorting behavior.

### 6. **StringComparer Variations**

String comparison is more complex than it appears due to cultural, linguistic, and performance considerations. The StringComparer class provides several predefined comparison strategies, each optimized for specific scenarios.

**Ordinal Comparison:**
StringComparer.Ordinal performs byte-by-byte comparison of string characters, ignoring cultural rules. This approach is fastest and most predictable, making it ideal for internal identifiers, file paths, and other non-linguistic data.

**Cultural Comparison:**
StringComparer.CurrentCulture and InvariantCulture consider linguistic rules specific to cultures. These comparers handle accents, case conversion, and character equivalence according to cultural conventions, making them appropriate for user-visible data.

**Performance Considerations:**
Ordinal comparisons are significantly faster than cultural comparisons because they avoid complex linguistic processing. For high-performance scenarios or when cultural sensitivity is not required, ordinal comparison should be preferred.

**Case Sensitivity:**
Each comparison type offers both case-sensitive and case-insensitive variants. Case-insensitive comparison is commonly needed for user input scenarios where "Apple" and "apple" should be treated as equivalent.

### 7. **Structural Equality and Comparison**

Structural equality compares composite objects element by element rather than treating them as single units. This concept is particularly important for collections like arrays, where logical equality means all corresponding elements are equal.

**IStructuralEquatable Interface:**
This interface allows objects to define how they should be compared structurally, accepting an element comparer that defines how individual elements are compared. Arrays implement this interface to enable deep comparison of their contents.

**Element Comparer Integration:**
The power of structural comparison lies in its ability to accept custom element comparers. This flexibility allows complex scenarios like case-insensitive string array comparison or tolerance-based numeric array comparison.

**Performance Implications:**
Structural comparison is more expensive than reference comparison because it must examine every element. For large collections, consider whether full structural comparison is necessary or if a hash-based approach would be more efficient.

### 8. **Mutable Key Dangers**

One of the most critical concepts in hash-based collections is the immutability requirement for keys. Modifying an object's properties after it has been inserted into a hash-based collection can break the collection's internal structure and make objects unfindable.

**The Problem Mechanism:**
Hash-based collections like Dictionary use an object's hash code to determine where to store it internally. If the hash code changes after insertion (due to property modification), the collection will look for the object in the wrong location, effectively losing it.

**Consequences:**
- Objects become unretrievable from the collection
- The collection's Count property may not reflect the actual number of accessible items
- Memory leaks can occur as unretrievable objects remain in memory
- Collection enumeration may still find the object, creating inconsistent behavior

**Prevention Strategies:**
- Use immutable objects as keys whenever possible
- Base hash codes only on immutable properties
- Consider using readonly structs for simple key types
- Implement defensive copying when mutable objects must be used as keys

## Key Features Demonstrated

### Custom Equality Comparer
```csharp
public class LastFirstEqualityComparer : EqualityComparer<Customer>
{
    public override bool Equals(Customer? x, Customer? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        
        // Business logic: customers equal if names match
        return x.LastName == y.LastName && x.FirstName == y.FirstName;
    }

    public override int GetHashCode(Customer? obj)
    {
        if (obj is null) return 0;
        
        // CRITICAL: Hash must be consistent with Equals!
        return HashCode.Combine(obj.LastName, obj.FirstName);
    }
}
```

### Custom Sort Order
```csharp
public class PriorityComparer : Comparer<Wish>
{
    public override int Compare(Wish? x, Wish? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        // Sort by priority first, then by name
        int priorityComparison = x.Priority.CompareTo(y.Priority);
        if (priorityComparison != 0) return priorityComparison;

        return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
    }
}
```

### String Normalization
```csharp
public class SurnameComparer : Comparer<string>
{
    string Normalize(string? s)
    {
        if (s == null) return "";
        
        s = s.Trim().ToUpper();
        // Normalize "Mc" to "Mac" for consistent sorting
        if (s.StartsWith("MC")) 
            s = "MAC" + s.Substring(2);
        
        return s;
    }

    public override int Compare(string? x, string? y)
    {
        return Normalize(x).CompareTo(Normalize(y));
    }
}
```

### Structural Equality
```csharp
// Compare arrays element by element
int[] a1 = { 1, 2, 3 };
int[] a2 = { 1, 2, 3 };

IStructuralEquatable se1 = a1;
bool areEqual = se1.Equals(a2, EqualityComparer<int>.Default); // True

// Case-insensitive string array comparison
string[] sArr1 = "the quick brown fox".Split();
string[] sArr2 = "THE QUICK BROWN FOX".Split();

IStructuralEquatable seArr1 = sArr1;
bool caseInsensitiveEqual = seArr1.Equals(sArr2, StringComparer.InvariantCultureIgnoreCase);
```

## Critical Rules and Best Practices

### The Hash Code Contract
**FUNDAMENTAL RULE**: If `Equals(x, y)` returns `true`, then `GetHashCode(x)` MUST equal `GetHashCode(y)`

```csharp
public class Person : IEquatable<Person>
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public bool Equals(Person? other)
    {
        return other != null && Name == other.Name && Age == other.Age;
    }
    
    public override int GetHashCode()
    {
        // Must be consistent with Equals method
        return HashCode.Combine(Name, Age);
    }
}
```

### Mutable Key Danger Zone
```csharp
// DANGEROUS: Don't do this!
var customer = new Customer("Smith", "John");
var dict = new Dictionary<Customer, string>(new CustomerComparer());
dict[customer] = "Data";

// DANGER: This breaks the hash contract!
customer.LastName = "Johnson";  // Object now unfindable in dictionary!

// SAFE: Use immutable properties for keys
public class SafeCustomer
{
    public string LastName { get; }  // Immutable
    public string FirstName { get; }  // Immutable
    
    public SafeCustomer(string lastName, string firstName)
    {
        LastName = lastName;
        FirstName = firstName;
    }
}
```

### Performance Guidelines
- **StringComparer.Ordinal** is fastest for exact matching
- **StringComparer.OrdinalIgnoreCase** for case-insensitive scenarios
- **Avoid expensive operations** in GetHashCode() - it's called frequently
- **Cache hash codes** for expensive-to-compute scenarios
- **Use immutable properties** for hash calculation when possible

## Implementation Patterns and Best Practices

### Design Patterns for Equality Comparers

**Null Handling Pattern:**
All custom equality comparers must handle null values consistently. The standard pattern checks for reference equality first (which handles the case where both objects are the same instance), then checks for null values, and finally performs the actual comparison logic.

```csharp
public override bool Equals(Customer? x, Customer? y)
{
    if (ReferenceEquals(x, y)) return true;  // Same instance or both null
    if (x is null || y is null) return false;  // One is null, other is not
    
    // Actual business logic comparison
    return x.LastName == y.LastName && x.FirstName == y.FirstName;
}
```

**Hash Code Calculation Strategy:**
Hash codes should be calculated using immutable properties only and should distribute values evenly across the hash space. The HashCode.Combine method provides an efficient way to combine multiple values while maintaining good distribution properties.

### Performance Optimization Techniques

**Caching Hash Codes:**
For objects with expensive-to-calculate hash codes, consider caching the hash value after first calculation. This optimization is particularly valuable for objects that are frequently used as keys in collections.

**Comparer Selection Guidelines:**
- Use StringComparer.Ordinal for maximum performance when cultural sensitivity is not required
- Use StringComparer.OrdinalIgnoreCase for case-insensitive scenarios without cultural rules
- Reserve cultural comparers for user-facing data that requires linguistic accuracy
- Consider creating custom comparers for domain-specific optimization needs

### Thread Safety Considerations

**Immutable Comparer Design:**
Equality and ordering comparers should be stateless and immutable, making them inherently thread-safe. Avoid storing mutable state within comparer instances, as this can lead to race conditions in multi-threaded environments.

**Static Comparer Instances:**
Consider providing static instances of frequently used comparers to avoid repeated object allocation and to ensure consistent behavior across an application.

## Real-World Applications

### Product Catalog with Case-Insensitive SKUs
```csharp
var catalog = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase);
catalog["LAPTOP-001"] = new Product("Gaming Laptop", 1299.99m);

// Customer can search with any case variation
bool found = catalog.TryGetValue("laptop-001", out var product);  // True
```

### Employee Sorting by Department and Salary
```csharp
public class EmployeeComparer : Comparer<Employee>
{
    public override int Compare(Employee? x, Employee? y)
    {
        // Sort by department first
        int deptComparison = string.Compare(x.Department, y.Department);
        if (deptComparison != 0) return deptComparison;

        // Then by salary (descending)
        return y.Salary.CompareTo(x.Salary);
    }
}
```

### Intelligent Cache with Parameter-Order Independence
```csharp
public class CacheKey
{
    public string Resource { get; set; }
    public string[] Parameters { get; set; }
}

public class CacheKeyComparer : EqualityComparer<CacheKey>
{
    public override bool Equals(CacheKey? x, CacheKey? y)
    {
        if (x?.Resource != y?.Resource) return false;
        
        // Parameter order shouldn't matter
        var xSorted = x.Parameters.OrderBy(p => p);
        var ySorted = y.Parameters.OrderBy(p => p);
        return xSorted.SequenceEqual(ySorted);
    }

    public override int GetHashCode(CacheKey? obj)
    {
        // Hash must be order-independent
        var sortedParams = obj.Parameters.OrderBy(p => p);
        return HashCode.Combine(obj.Resource, string.Join("|", sortedParams));
    }
}
```

## Critical Rules and Best Practices

### The Hash Code Contract
**FUNDAMENTAL RULE**: If `Equals(x, y)` returns `true`, then `GetHashCode(x)` MUST equal `GetHashCode(y)`

```csharp
public class Person : IEquatable<Person>
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public bool Equals(Person? other)
    {
        return other != null && Name == other.Name && Age == other.Age;
    }
    
    public override int GetHashCode()
    {
        // Must be consistent with Equals method
        return HashCode.Combine(Name, Age);
    }
}
```

### Mutable Key Danger Zone
```csharp
// DANGEROUS: Don't do this!
var customer = new Customer("Smith", "John");
var dict = new Dictionary<Customer, string>(new CustomerComparer());
dict[customer] = "Data";

// DANGER: This breaks the hash contract!
customer.LastName = "Johnson";  // Object now unfindable in dictionary!

// SAFE: Use immutable properties for keys
public class SafeCustomer
{
    public string LastName { get; }  // Immutable
    public string FirstName { get; }  // Immutable
    
    public SafeCustomer(string lastName, string firstName)
    {
        LastName = lastName;
        FirstName = firstName;
    }
}
```

### Performance Guidelines
- **StringComparer.Ordinal** is fastest for exact matching
- **StringComparer.OrdinalIgnoreCase** for case-insensitive scenarios
- **Avoid expensive operations** in GetHashCode() - it's called frequently
- **Cache hash codes** for expensive-to-compute scenarios
- **Use immutable properties** for hash calculation when possible

## Implementation Patterns and Best Practices

### Design Patterns for Equality Comparers

**Null Handling Pattern:**
All custom equality comparers must handle null values consistently. The standard pattern checks for reference equality first (which handles the case where both objects are the same instance), then checks for null values, and finally performs the actual comparison logic.

```csharp
public override bool Equals(Customer? x, Customer? y)
{
    if (ReferenceEquals(x, y)) return true;  // Same instance or both null
    if (x is null || y is null) return false;  // One is null, other is not
    
    // Actual business logic comparison
    return x.LastName == y.LastName && x.FirstName == y.FirstName;
}
```

**Hash Code Calculation Strategy:**
Hash codes should be calculated using immutable properties only and should distribute values evenly across the hash space. The HashCode.Combine method provides an efficient way to combine multiple values while maintaining good distribution properties.

### Performance Optimization Techniques

**Caching Hash Codes:**
For objects with expensive-to-calculate hash codes, consider caching the hash value after first calculation. This optimization is particularly valuable for objects that are frequently used as keys in collections.

**Comparer Selection Guidelines:**
- Use StringComparer.Ordinal for maximum performance when cultural sensitivity is not required
- Use StringComparer.OrdinalIgnoreCase for case-insensitive scenarios without cultural rules
- Reserve cultural comparers for user-facing data that requires linguistic accuracy
- Consider creating custom comparers for domain-specific optimization needs

### Thread Safety Considerations

**Immutable Comparer Design:**
Equality and ordering comparers should be stateless and immutable, making them inherently thread-safe. Avoid storing mutable state within comparer instances, as this can lead to race conditions in multi-threaded environments.

**Static Comparer Instances:**
Consider providing static instances of frequently used comparers to avoid repeated object allocation and to ensure consistent behavior across an application.

## Real-World Applications

### Product Catalog with Case-Insensitive SKUs
```csharp
var catalog = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase);
catalog["LAPTOP-001"] = new Product("Gaming Laptop", 1299.99m);

// Customer can search with any case variation
bool found = catalog.TryGetValue("laptop-001", out var product);  // True
```

### Employee Sorting by Department and Salary
```csharp
public class EmployeeComparer : Comparer<Employee>
{
    public override int Compare(Employee? x, Employee? y)
    {
        // Sort by department first
        int deptComparison = string.Compare(x.Department, y.Department);
        if (deptComparison != 0) return deptComparison;

        // Then by salary (descending)
        return y.Salary.CompareTo(x.Salary);
    }
}
```

### Intelligent Cache with Parameter-Order Independence
```csharp
public class CacheKey
{
    public string Resource { get; set; }
    public string[] Parameters { get; set; }
}

public class CacheKeyComparer : EqualityComparer<CacheKey>
{
    public override bool Equals(CacheKey? x, CacheKey? y)
    {
        if (x?.Resource != y?.Resource) return false;
        
        // Parameter order shouldn't matter
        var xSorted = x.Parameters.OrderBy(p => p);
        var ySorted = y.Parameters.OrderBy(p => p);
        return xSorted.SequenceEqual(ySorted);
    }

    public override int GetHashCode(CacheKey? obj)
    {
        // Hash must be order-independent
        var sortedParams = obj.Parameters.OrderBy(p => p);
        return HashCode.Combine(obj.Resource, string.Join("|", sortedParams));
    }
}
```

## Summary and Conclusion

### Key Design Principles

**Immutability First:**
Design objects used as keys with immutable properties whenever possible. This approach eliminates the risk of breaking hash-based collections through inadvertent property modification.

**Consistency Above All:**
Maintain strict consistency between equality and hash code implementations. Violating this relationship causes subtle but critical failures in collection operations.

**Performance Awareness:**
Choose comparison strategies appropriate for the use case. Optimize for the common case in your application while maintaining correctness for edge cases.

**Business Logic Alignment:**
Ensure that technical equality and ordering implementations accurately reflect business requirements and domain concepts.

### Common Implementation Pitfalls

**Mutable Key Usage:**
Never modify properties used in equality comparison or hash code calculation after an object has been inserted into a hash-based collection. This violation makes objects unfindable and can cause memory leaks.

**Inconsistent Hash Codes:**
Implementing GetHashCode methods that are inconsistent with Equals methods breaks the fundamental contract required by hash-based collections.

**Expensive Comparison Operations:**
Avoid computationally expensive operations in frequently-called comparison methods, as these can significantly degrade application performance.

**Inadequate Null Handling:**
Properly handle null values in all comparison logic to prevent runtime exceptions and ensure robust application behavior.

**Custom String Comparison:**
Use the predefined StringComparer classes rather than implementing custom string comparison logic, as the framework implementations are optimized and handle cultural complexities correctly.

### Professional Development Impact

This project provides essential foundation knowledge for building robust, efficient collection-based applications where custom equality and ordering logic is critical for correct business behavior. The concepts demonstrated here are fundamental to enterprise application development and appear frequently in real-world software engineering scenarios.

Understanding these patterns enables developers to create applications that behave correctly according to business rules while maintaining optimal performance characteristics. This knowledge is particularly valuable in domains such as financial systems, inventory management, customer relationship management, and any application where object identity and ordering are central to business operations.

