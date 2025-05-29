# Collections: Lists, Queues, Stacks, Dictionaries and Sets

## 🎯 Learning Objectives

Master the essential data structures that power modern applications! Collections are the workhorses of programming - understanding when and how to use each type is crucial for writing efficient, readable code.

## 📚 What You'll Learn

### Core Collection Types:

1. **Lists (`List<T>`)**
   - Dynamic arrays with automatic resizing
   - Random access by index (O(1))
   - Insertion and deletion operations
   - Searching and sorting capabilities

2. **Queues (`Queue<T>`)**
   - First-In-First-Out (FIFO) principle
   - `Enqueue()` and `Dequeue()` operations
   - Perfect for scheduling and buffering
   - Real-world applications

3. **Stacks (`Stack<T>`)**
   - Last-In-First-Out (LIFO) principle
   - `Push()` and `Pop()` operations
   - Undo functionality and expression evaluation
   - Call stack simulation

4. **Sets (`HashSet<T>`, `SortedSet<T>`)**
   - Unique element collections
   - Set operations: union, intersection, difference
   - Fast membership testing (O(1) average)
   - Mathematical set behavior

5. **Dictionaries (`Dictionary<TKey, TValue>`)**
   - Key-value pair storage
   - Fast lookups by key (O(1) average)
   - `TryGetValue()` for safe access
   - Custom key types and equality

### Advanced Collections:
- **SortedDictionary<TKey, TValue>**: Ordered key-value pairs
- **ConcurrentDictionary<TKey, TValue>**: Thread-safe dictionaries
- **BitArray**: Compact boolean arrays
- **Specialized collections**: NameValueCollection, StringCollection

## 🚀 Key Features Demonstrated

### List Operations:
```csharp
var numbers = new List<int> { 1, 2, 3 };
numbers.Add(4);                    // Add single item
numbers.AddRange(new[] { 5, 6 });  // Add multiple items
numbers.Insert(0, 0);              // Insert at specific index
numbers.Remove(3);                 // Remove first occurrence
numbers.RemoveAt(1);               // Remove by index
```

### Queue (FIFO) Operations:
```csharp
var taskQueue = new Queue<string>();
taskQueue.Enqueue("First Task");   // Add to back
taskQueue.Enqueue("Second Task");
string current = taskQueue.Dequeue(); // Remove from front
string next = taskQueue.Peek();       // Look without removing
```

### Stack (LIFO) Operations:
```csharp
var undoStack = new Stack<string>();
undoStack.Push("Action 1");        // Add to top
undoStack.Push("Action 2");
string lastAction = undoStack.Pop(); // Remove from top
string topAction = undoStack.Peek(); // Look without removing
```

### Set Operations:
```csharp
var set1 = new HashSet<int> { 1, 2, 3, 4 };
var set2 = new HashSet<int> { 3, 4, 5, 6 };

set1.UnionWith(set2);          // Combine sets
set1.IntersectWith(set2);      // Common elements
set1.ExceptWith(set2);         // Elements in set1 but not set2
bool contains = set1.Contains(3); // Fast membership test
```

### Dictionary Operations:
```csharp
var userScores = new Dictionary<string, int>
{
    ["Alice"] = 100,
    ["Bob"] = 95
};

userScores["Charlie"] = 87;        // Add or update
bool found = userScores.TryGetValue("Alice", out int score);
userScores.Remove("Bob");          // Remove by key
```

## 💡 Trainer Tips

> **Choose the Right Tool**: Lists for ordered data with frequent access, Sets for uniqueness, Dictionaries for lookups, Queues for processing order, Stacks for reversal operations.

> **Performance Awareness**: Dictionary and HashSet are O(1) for lookups, List is O(n) for searching but O(1) for index access. Choose based on your primary operations!

> **Capacity Considerations**: Lists double in size when they grow. If you know the approximate size, set initial capacity to avoid reallocations.

## 🔍 What to Focus On

1. **Performance characteristics**: Big O notation for each operation
2. **Use case patterns**: When to choose each collection type
3. **Memory efficiency**: How collections manage memory
4. **Thread safety**: Which collections are safe for concurrent access

## 🏃‍♂️ Run the Project

```bash
dotnet run
```

The demo includes:
- Comprehensive examples of all collection types
- Performance comparisons
- Real-world usage scenarios
- Advanced operations and utilities
- Best practices for each collection

## 🎓 Best Practices

1. **Use specific collection interfaces** (`IList<T>`, `ISet<T>`) for parameters
2. **Set initial capacity** when size is predictable
3. **Use `TryGetValue()`** instead of checking `ContainsKey()` then accessing
4. **Consider `ConcurrentCollections`** for multi-threaded scenarios
5. **Use `foreach` over `for`** when you don't need the index
6. **Prefer `HashSet<T>` over `List<T>`** for membership testing

## 🔧 Real-World Applications

### By Collection Type:

**Lists:**
- Shopping cart items
- Search results
- User activity logs
- File system entries

**Queues:**
- Task processing systems
- Print job management
- Web request handling
- Breadth-first search algorithms

**Stacks:**
- Undo/redo functionality
- Expression evaluation
- Backtracking algorithms
- Call stack simulation

**Sets:**
- Unique user preferences
- Tag systems
- Permission management
- Data deduplication

**Dictionaries:**
- User sessions (ID → User)
- Configuration settings
- Caching systems
- Database result mapping

## 🎯 Performance Comparison

| Operation | List<T> | Queue<T> | Stack<T> | HashSet<T> | Dictionary<TKey,TValue> |
|-----------|---------|----------|----------|------------|-------------------------|
| Add/Insert | O(1)* | O(1) | O(1) | O(1)* | O(1)* |
| Remove | O(n) | O(1) | O(1) | O(1)* | O(1)* |
| Search | O(n) | N/A | N/A | O(1)* | O(1)* |
| Access by Index | O(1) | N/A | N/A | N/A | N/A |

*Amortized time complexity

## 🔮 Advanced Collection Patterns

### Custom Equality:
```csharp
var customSet = new HashSet<Person>(new PersonEqualityComparer());
var customDict = new Dictionary<Person, string>(new PersonEqualityComparer());
```

### Collection Initialization:
```csharp
var fruits = new List<string> { "apple", "banana", "orange" };
var scores = new Dictionary<string, int> 
{
    ["player1"] = 100,
    ["player2"] = 95
};
```

### LINQ with Collections:
```csharp
var evenNumbers = numbers.Where(x => x % 2 == 0).ToList();
var grouped = people.GroupBy(p => p.Department).ToDictionary(g => g.Key, g => g.ToList());
```

## 🎯 Mastery Checklist

After this project, you should confidently:
- ✅ Choose the appropriate collection type for any scenario
- ✅ Understand performance implications of collection operations
- ✅ Use set operations for data analysis
- ✅ Implement efficient lookup systems with dictionaries
- ✅ Handle thread-safety requirements with concurrent collections
- ✅ Optimize collection performance through proper initialization
- ✅ Apply LINQ effectively with collections

## 💼 Industry Impact

Collections are fundamental to:
- **Web Development**: Session management, caching, request processing
- **Game Development**: Entity management, collision detection, AI pathfinding
- **Data Analysis**: Processing large datasets, statistical operations
- **Enterprise Software**: Business rule engines, workflow management
- **System Programming**: Resource management, scheduling algorithms

Remember: Collections are the building blocks of data manipulation. Master them, and you'll be able to solve complex problems efficiently while writing clean, maintainable code!
