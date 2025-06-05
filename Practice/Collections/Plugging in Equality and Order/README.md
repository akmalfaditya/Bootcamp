# Plugging in Equality and Order in C#

## Learning Objectives
 Learn to implement custom comparers, understand how HashSet, Dictionary, and sorted collections use equality and comparison logic, and build robust collection operations that work exactly how your business logic requires.

## What You'll Learn

### Core Comparison Concepts
- **IEqualityComparer<T>**: Custom equality logic for HashSet and Dictionary
- **IComparer<T>**: Custom ordering logic for sorted collections
- **Structural vs Reference Equality**: When to compare content vs identity
- **Hash Code Consistency**: The critical relationship between equality and hash codes

### Built-in Comparer Types
- **EqualityComparer<T>.Default**: Understanding default equality behavior
- **Comparer<T>.Default**: Default ordering for comparable types
- **StringComparer**: Culture-aware and ordinal string comparison
- **StructuralComparisons**: Comparing arrays and tuples element-by-element

### Advanced Comparison Patterns
- **Composite Comparers**: Combining multiple comparison criteria
- **Case-Insensitive Comparisons**: Handling text data flexibly
- **Custom Business Logic**: Implementing domain-specific equality rules
- **Performance Optimization**: Creating efficient comparison operations

## Key Features Demonstrated

### 1. **Custom Equality Comparer**
```csharp
public class CustomerEqualityComparer : IEqualityComparer<Customer>
{
    public bool Equals(Customer x, Customer y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        
        // Business rule: customers are equal if they have same name
        return x.FirstName == y.FirstName && x.LastName == y.LastName;
    }
    
    public int GetHashCode(Customer obj)
    {
        return HashCode.Combine(obj.FirstName, obj.LastName);
    }
}

// Usage in collections
var uniqueCustomers = new HashSet<Customer>(new CustomerEqualityComparer());
```

### 2. **Custom Ordering Comparer**
```csharp
public class ProductPriorityComparer : IComparer<Product>
{
    public int Compare(Product x, Product y)
    {
        // First compare by priority (high to low)
        int priorityComparison = y.Priority.CompareTo(x.Priority);
        if (priorityComparison != 0) return priorityComparison;
        
        // Then by name (alphabetical)
        return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
}

// Usage in sorted collections
var sortedProducts = new SortedSet<Product>(new ProductPriorityComparer());
```

### 3. **String Comparer Variations**
```csharp
// Case-sensitive ordinal comparison (fastest)
var caseSensitive = new HashSet<string>(StringComparer.Ordinal);

// Case-insensitive ordinal comparison
var caseInsensitive = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

// Culture-aware comparison (for UI display)
var cultureAware = new HashSet<string>(StringComparer.CurrentCulture);

// Invariant culture (for data storage)
var invariant = new HashSet<string>(StringComparer.InvariantCulture);
```

### 4. **Structural Equality for Collections**
```csharp
// Compare arrays element by element
int[] array1 = { 1, 2, 3 };
int[] array2 = { 1, 2, 3 };

bool areEqual = StructuralComparisons.StructuralEqualityComparer
    .Equals(array1, array2);  // true

// Works with nested arrays and complex structures
object[,] matrix1 = { { 1, 2 }, { 3, 4 } };
object[,] matrix2 = { { 1, 2 }, { 3, 4 } };
bool matricesEqual = StructuralComparisons.StructuralEqualityComparer
    .Equals(matrix1, matrix2);  // true
```

## Tips

### **The Hash Code Contract**
```csharp
// CRITICAL RULE: If two objects are equal, they MUST have the same hash code
public class Person : IEquatable<Person>
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public bool Equals(Person other)
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

### **Performance Considerations**
- **Hash codes should be fast to compute** - avoid expensive operations
- **Use immutable properties** for hash code calculation when possible
- **StringComparer.Ordinal** is fastest for exact string matching
- **Cache hash codes** for expensive-to-compute scenarios

### **Common Pitfalls**
```csharp
// BAD: Using mutable properties in equality
public class BadExample
{
    public List<string> Items { get; set; } = new();
    
    public override int GetHashCode()
    {
        // DON'T DO THIS - hash changes when list changes!
        return Items.GetHashCode();
    }
}

// GOOD: Using immutable or stable properties
public class GoodExample
{
    public string Id { get; }  // Immutable
    public List<string> Items { get; set; } = new();
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();  // Stable hash code
    }
}
```

### **Choosing the Right Comparer**
- **Default Equality**: Use when built-in behavior is sufficient
- **Custom IEqualityComparer**: For business-specific equality rules
- **StringComparer**: Always use for string operations - don't roll your own
- **Structural Comparisons**: For arrays, tuples, and composite data structures

## Real-World Applications

### **Customer Management System**
```csharp
public class CustomerService
{
    // Use custom equality to prevent duplicate customers
    private readonly HashSet<Customer> _customers = new(new CustomerEqualityComparer());
    
    public bool AddCustomer(Customer customer)
    {
        // Automatically prevents duplicates based on business rules
        return _customers.Add(customer);
    }
    
    public Customer FindCustomer(string firstName, string lastName)
    {
        var searchCustomer = new Customer(firstName, lastName);
        _customers.TryGetValue(searchCustomer, out Customer found);
        return found;
    }
}

public class CustomerEqualityComparer : IEqualityComparer<Customer>
{
    public bool Equals(Customer x, Customer y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        
        // Business rule: same customer if same name (case-insensitive)
        return string.Equals(x.FirstName, y.FirstName, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(x.LastName, y.LastName, StringComparison.OrdinalIgnoreCase);
    }
    
    public int GetHashCode(Customer obj)
    {
        return HashCode.Combine(
            obj.FirstName?.ToUpperInvariant(),
            obj.LastName?.ToUpperInvariant()
        );
    }
}
```

### **File System Operations**
```csharp
public class FileManager
{
    // Case-insensitive file names on Windows, case-sensitive on Unix
    private readonly HashSet<string> _processedFiles = new(
        Environment.OSVersion.Platform == PlatformID.Win32NT 
            ? StringComparer.OrdinalIgnoreCase 
            : StringComparer.Ordinal
    );
    
    public bool MarkAsProcessed(string filePath)
    {
        return _processedFiles.Add(Path.GetFileName(filePath));
    }
    
    public bool IsAlreadyProcessed(string filePath)
    {
        return _processedFiles.Contains(Path.GetFileName(filePath));
    }
}
```

### **Priority Queue Implementation**
```csharp
public class TaskScheduler
{
    private readonly SortedSet<ScheduledTask> _tasks = new(new TaskPriorityComparer());
    
    public void ScheduleTask(ScheduledTask task)
    {
        _tasks.Add(task);
    }
    
    public ScheduledTask GetNextTask()
    {
        if (_tasks.Count == 0) return null;
        
        var nextTask = _tasks.Min;  // Highest priority task
        _tasks.Remove(nextTask);
        return nextTask;
    }
}

public class TaskPriorityComparer : IComparer<ScheduledTask>
{
    public int Compare(ScheduledTask x, ScheduledTask y)
    {
        // Higher priority values come first
        int priorityComparison = y.Priority.CompareTo(x.Priority);
        if (priorityComparison != 0) return priorityComparison;
        
        // Then by scheduled time (earlier first)
        int timeComparison = x.ScheduledTime.CompareTo(y.ScheduledTime);
        if (timeComparison != 0) return timeComparison;
        
        // Finally by ID for consistency
        return x.Id.CompareTo(y.Id);
    }
}
```

### **Configuration Management**
```csharp
public class ConfigurationComparer : IEqualityComparer<ConfigSection>
{
    public bool Equals(ConfigSection x, ConfigSection y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        
        // Compare section names (case-insensitive)
        if (!string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase))
            return false;
        
        // Compare all settings
        if (x.Settings.Count != y.Settings.Count) return false;
        
        foreach (var setting in x.Settings)
        {
            if (!y.Settings.TryGetValue(setting.Key, out string otherValue) ||
                !string.Equals(setting.Value, otherValue, StringComparison.Ordinal))
            {
                return false;
            }
        }
        
        return true;
    }
    
    public int GetHashCode(ConfigSection obj)
    {
        var hash = new HashCode();
        hash.Add(obj.Name?.ToUpperInvariant());
        
        // Include all settings in hash (order-independent)
        foreach (var setting in obj.Settings.OrderBy(kvp => kvp.Key))
        {
            hash.Add(setting.Key);
            hash.Add(setting.Value);
        }
        
        return hash.ToHashCode();
    }
}
```

## Integration with Modern C#

### **Records with Custom Equality (C# 9+)**
```csharp
public record Customer(string FirstName, string LastName)
{
    // Custom equality comparer as a static property
    public static IEqualityComparer<Customer> NameIgnoreCaseComparer { get; } = 
        new CustomerIgnoreCaseComparer();
        
    private class CustomerIgnoreCaseComparer : IEqualityComparer<Customer>
    {
        public bool Equals(Customer x, Customer y) =>
            StringComparer.OrdinalIgnoreCase.Equals(x.FirstName, y.FirstName) &&
            StringComparer.OrdinalIgnoreCase.Equals(x.LastName, y.LastName);
            
        public int GetHashCode(Customer obj) =>
            HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.FirstName),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.LastName)
            );
    }
}
```

### **Pattern Matching in Comparers (C# 8+)**
```csharp
public class SmartComparer<T> : IComparer<T>
{
    public int Compare(T x, T y) => (x, y) switch
    {
        (null, null) => 0,
        (null, _) => -1,
        (_, null) => 1,
        (IComparable<T> comparable, var other) => comparable.CompareTo(other),
        _ => Comparer<T>.Default.Compare(x, y)
    };
}
```

### **Generic Math (C# 11+)**
```csharp
public class NumericComparer<T> : IComparer<T> where T : INumber<T>
{
    public int Compare(T x, T y) => x.CompareTo(y);
}
```

## Industry Impact

Custom equality and ordering are essential for:

- **Data Deduplication**: Removing duplicates based on business rules, not memory addresses
- **Efficient Lookups**: HashMap and TreeMap performance depends on good equality/comparison
- **User Interface Sorting**: Displaying data in meaningful order for users
- **Database Operations**: Implementing custom comparison logic for in-memory operations
- **Caching Systems**: Proper key comparison for cache hit/miss detection

