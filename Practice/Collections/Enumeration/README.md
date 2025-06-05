# Enumeration in C#

## Learning Objectives
Master the art of **enumeration** in C# - the foundation of working with collections and sequences. Learn how `foreach` works under the hood, implement custom enumerable types, and leverage advanced enumeration patterns including lazy evaluation and infinite sequences.

## What You'll Learn

### Core Enumeration Concepts
- **IEnumerable Interface**: The contract that makes foreach possible
- **IEnumerator Pattern**: The state machine behind collection traversal
- **Generic vs Non-Generic**: Type safety and performance differences
- **Foreach Loop Mechanics**: What really happens when you use foreach

### Advanced Enumeration Techniques
- **Yield Keyword**: Creating lazy, memory-efficient sequences
- **Custom Enumerables**: Building your own collection types
- **Iterator Methods**: Functions that return IEnumerable<T>
- **Infinite Sequences**: Collections that generate values on demand

### Enumeration Patterns
- **Deferred Execution**: Understanding when enumeration actually occurs
- **Multiple Enumeration**: Handling collections that can only be enumerated once
- **Reset Capability**: When and why enumerators support resetting
- **Disposal Pattern**: Proper cleanup of enumeration resources

## Key Features Demonstrated

### 1. **Basic Enumeration Mechanics**
```csharp
// What foreach actually does
IEnumerable<int> numbers = GetNumbers();
foreach (int number in numbers)
{
    Console.WriteLine(number);
}

// Equivalent manual enumeration
using (IEnumerator<int> enumerator = numbers.GetEnumerator())
{
    while (enumerator.MoveNext())
    {
        Console.WriteLine(enumerator.Current);
    }
}
```

### 2. **Custom Enumerable Implementation**
```csharp
public class NumberSequence : IEnumerable<int>
{
    private readonly int _start;
    private readonly int _count;
    
    public NumberSequence(int start, int count)
    {
        _start = start;
        _count = count;
    }
    
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return _start + i;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

### 3. **Yield-Based Iterator Methods**
```csharp
public static IEnumerable<T> TakeEvery<T>(IEnumerable<T> source, int step)
{
    int index = 0;
    foreach (T item in source)
    {
        if (index % step == 0)
            yield return item;
        index++;
    }
}

// Usage: Only takes every 3rd element
var everyThird = TakeEvery(numbers, 3);
```

### 4. **Infinite Sequences**
```csharp
public static IEnumerable<int> Fibonacci()
{
    int a = 0, b = 1;
    while (true)
    {
        yield return a;
        (a, b) = (b, a + b);
    }
}

// Take first 10 Fibonacci numbers
var firstTen = Fibonacci().Take(10);
```

## Tips

### **Deferred Execution Understanding**
```csharp
// This doesn't execute immediately!
var expensiveQuery = data
    .Where(x => ExpensiveOperation(x))
    .Select(x => TransformData(x));

// Execution happens here, when enumerated
foreach (var item in expensiveQuery)
{
    // Now the operations run
}
```

### **Multiple Enumeration Gotchas**
```csharp
// Dangerous: Enumerates the sequence twice
var results = GetExpensiveData();
var count = results.Count();           // First enumeration
var list = results.ToList();           // Second enumeration!

// Better: Materialize once
var results = GetExpensiveData().ToList();
var count = results.Count;             // No enumeration
var list = results;                    // No enumeration
```

### **Memory Efficiency with Yield**
```csharp
// Memory efficient: only one item in memory at a time
public static IEnumerable<string> ReadLargeFile(string path)
{
    using var reader = new StreamReader(path);
    string line;
    while ((line = reader.ReadLine()) != null)
    {
        yield return line;
    }
}

// Processes gigabyte files with minimal memory
foreach (var line in ReadLargeFile("huge-file.txt"))
{
    ProcessLine(line);
}
```

### **Performance Considerations**
- **Use yield** for lazy evaluation and memory efficiency
- **Avoid** multiple enumeration of expensive sequences
- **Materialize with ToList()** when you need random access
- **Don't** use IEnumerable for frequently accessed collections

## Real-World Applications

### **Data Processing Pipeline**
```csharp
public class DataProcessor
{
    public IEnumerable<ProcessedRecord> ProcessLargeDataset(string filePath)
    {
        return File.ReadLines(filePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => ParseRecord(line))
            .Where(record => IsValid(record))
            .Select(record => ProcessRecord(record));
    }
    
    // Processes millions of records with constant memory usage
}
```

### **Configuration System**
```csharp
public class ConfigurationProvider : IEnumerable<KeyValuePair<string, string>>
{
    private readonly List<IConfigSource> _sources;
    
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        foreach (var source in _sources)
        {
            foreach (var setting in source.GetSettings())
            {
                yield return setting;
            }
        }
    }
    
    // Lazy loading from multiple configuration sources
}
```

### **Game Development**
```csharp
public class GameObjectEnumerator : IEnumerable<GameObject>
{
    private readonly Scene _scene;
    
    public IEnumerable<T> OfType<T>() where T : Component
    {
        foreach (var gameObject in this)
        {
            var component = gameObject.GetComponent<T>();
            if (component != null)
                yield return component;
        }
    }
    
    // Efficiently find specific component types
}
```

### **API Response Processing**
```csharp
public async IAsyncEnumerable<T> GetPagedDataAsync<T>(string apiEndpoint)
{
    string nextUrl = apiEndpoint;
    
    while (!string.IsNullOrEmpty(nextUrl))
    {
        var response = await httpClient.GetAsync(nextUrl);
        var page = await response.Content.ReadFromJsonAsync<PagedResponse<T>>();
        
        foreach (var item in page.Items)
        {
            yield return item;
        }
        
        nextUrl = page.NextPageUrl;
    }
}
```


## Integration with Modern C#

### **Async Enumeration (C# 8+)**
```csharp
public async IAsyncEnumerable<string> ReadLinesAsync(string filePath)
{
    using var reader = new StreamReader(filePath);
    string line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        yield return line;
    }
}

// Usage with await foreach
await foreach (var line in ReadLinesAsync("data.txt"))
{
    await ProcessLineAsync(line);
}
```

### **Range and Index (C# 8+)**
```csharp
public static IEnumerable<T> GetRange<T>(IList<T> list, Range range)
{
    var (start, length) = range.GetOffsetAndLength(list.Count);
    for (int i = start; i < start + length; i++)
    {
        yield return list[i];
    }
}

// Usage: Get last 5 elements
var lastFive = GetRange(numbers, ^5..);
```

### **Pattern Matching (C# 7+)**
```csharp
public static IEnumerable<T> ProcessItems<T>(IEnumerable<T> items)
{
    foreach (var item in items)
    {
        yield return item switch
        {
            string s when s.Length > 10 => TransformLongString(s),
            int n when n > 100 => TransformLargeNumber(n),
            _ => item
        };
    }
}
```

### **Records and With Expressions (C# 9+)**
```csharp
public record DataPoint(DateTime Timestamp, double Value);

public static IEnumerable<DataPoint> AdjustTimestamps(
    IEnumerable<DataPoint> points, TimeSpan offset)
{
    foreach (var point in points)
    {
        yield return point with { Timestamp = point.Timestamp.Add(offset) };
    }
}
```

## Industry Impact

Enumeration mastery is essential because it:

- **Powers LINQ**: The foundation of all LINQ operations and query syntax
- **Enables Streaming**: Process large datasets without loading everything into memory
- **Supports Lazy Loading**: Defer expensive operations until actually needed
- **Facilitates Integration**: Standard interface for working with any collection type
- **Improves Performance**: Avoid unnecessary data copying and memory allocation

