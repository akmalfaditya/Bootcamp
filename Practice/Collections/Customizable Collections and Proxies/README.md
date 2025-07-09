# Customizable Collections and Proxies

This project demonstrates the power of .NET's customizable collection classes found in `System.Collections.ObjectModel`. These classes act as proxies or wrappers around standard collections, providing virtual methods that serve as "gateways" for custom behavior.

## Why Customize Collections?

Standard collections like `List<T>` are perfect for direct instantiation, but they don't provide mechanisms to intercept operations like adding or removing items. In enterprise applications, you often need:

- **Event Firing**: Trigger events when items are added, removed, or changed
- **Property Updates**: Automatically update related properties or states
- **Validation**: Enforce business rules and detect illegal operations
- **Relationship Management**: Maintain parent-child relationships between objects

## Core Concepts and Architecture

### Understanding the Proxy Pattern in Collections

The .NET Framework employs the proxy pattern for customizable collections. A proxy acts as an intermediary that controls access to another object. In the context of collections, these proxy classes wrap standard collections like `List<T>` or `Dictionary<TKey, TValue>` and intercept method calls to provide customization opportunities.

The fundamental architecture involves:
- **Wrapper Classes**: These inherit from base classes that implement standard collection interfaces
- **Virtual Gateway Methods**: Protected methods that are called for specific operations
- **Base Implementation Forwarding**: The proxy forwards actual work to the underlying collection
- **Customization Points**: Developers override virtual methods to inject custom behavior

### The Virtual Method Pattern

All customizable collections use virtual methods as extension points. This pattern allows derived classes to:
- Execute custom logic before the operation
- Modify or validate parameters
- Execute custom logic after the operation
- Completely replace the default behavior if needed

The critical principle is that all public collection methods (Add, Remove, Clear, indexer assignment) internally call these virtual methods, ensuring your custom logic is always executed.

## Key Collection Classes Demonstrated

### 1. Collection<T> - The Foundation of Customizable Collections

`Collection<T>` serves as the primary base class for creating customizable collections. It acts as a wrapper around `List<T>` while providing four essential virtual gateway methods that correspond to the fundamental collection operations:

```csharp
protected virtual void ClearItems();           // Called by Clear()
protected virtual void InsertItem(int index, T item);  // Called by Add(), Insert()
protected virtual void RemoveItem(int index);          // Called by Remove(), RemoveAt()
protected virtual void SetItem(int index, T item);     // Called by indexer assignment
```

#### Key Characteristics:
- **Proxy Behavior**: Does not copy elements when wrapping an existing `IList<T>`
- **Gateway Methods**: All public operations funnel through virtual methods
- **Base Call Pattern**: Always call `base.Method()` to ensure the actual operation occurs
- **Protected Items Property**: Provides direct access to the underlying list when needed

#### Implementation Pattern:
```csharp
public class AnimalCollection : Collection<Animal>
{
    private readonly Zoo parentZoo;
    
    public AnimalCollection(Zoo zoo) 
    {
        this.parentZoo = zoo;
    }
    
    protected override void InsertItem(int index, Animal item)
    {
        // Custom logic before insertion
        ValidateAnimal(item);
        
        // Perform the actual insertion
        base.InsertItem(index, item);
        
        // Custom logic after insertion
        item.Zoo = parentZoo;
        NotifyZooOfNewAnimal(item);
    }
}
```

#### When to Use Collection<T>:
- Need to enforce business rules on collection modifications
- Require automatic relationship management between objects
- Want to implement validation or logging for collection operations
- Need to maintain consistency across related objects

### 2. KeyedCollection<TKey, TItem> - Dual-Access Collection Architecture

`KeyedCollection<TKey, TItem>` extends `Collection<T>` to provide both sequential (list-like) and associative (dictionary-like) access patterns. This hybrid approach is particularly valuable when you need to access items both by their position and by a unique identifier.

#### Architecture Overview:
- **Inherits from Collection<T>**: All customization capabilities are preserved
- **Abstract Key Extraction**: Requires implementation of `GetKeyForItem(T item)`
- **Internal Dictionary**: Created on-demand for fast key-based lookups
- **Dual Access Patterns**: Supports both indexed and keyed access simultaneously

#### Core Abstract Method:
```csharp
protected abstract TKey GetKeyForItem(TItem item);
```

This method defines how to extract the unique key from each item. The collection uses this to maintain an internal dictionary for efficient lookups.

#### Key Features and Methods:
```csharp
public TItem this[TKey key] { get; }                    // Key-based access
protected IDictionary<TKey, TItem> Dictionary { get; }   // Internal dictionary access
protected void ChangeItemKey(TItem item, TKey newKey);   // Key change notification
```

#### Implementation Pattern:
```csharp
public class BookCollection : KeyedCollection<string, Book>
{
    protected override string GetKeyForItem(Book item)
    {
        return item.ISBN; // ISBN serves as the unique key
    }
    
    // Optional: Override Collection<T> methods for additional behavior
    protected override void InsertItem(int index, Book item)
    {
        ValidateISBN(item.ISBN);
        base.InsertItem(index, item);
        LogBookAdded(item);
    }
}
```

#### Key Change Management:
When an item's key changes after being added to the collection, you must notify the collection:

```csharp
public class Book
{
    private string isbn;
    public string ISBN 
    { 
        get => isbn;
        set 
        {
            if (ParentCollection != null)
                ParentCollection.ChangeItemKey(this, value);
            isbn = value;
        }
    }
}
```

#### Performance Characteristics:
- **Dictionary Creation**: Internal dictionary is created when the first item is added
- **Threshold Control**: Can specify creation threshold in constructor
- **O(1) Key Access**: Key-based lookups are constant time
- **O(n) Index Access**: Index-based access maintains list semantics

#### When to Use KeyedCollection<TKey, TItem>:
- Need both positional and key-based access to items
- Items have natural, unique identifiers
- Performance requirements demand fast key lookups
- Want to maintain insertion order while supporting efficient searches

### 3. ReadOnlyCollection<T> - Controlled Access and Encapsulation

`ReadOnlyCollection<T>` implements a critical pattern for data encapsulation: providing read-only access to internal collections while preserving the ability to modify them internally. This class addresses a common architectural challenge where you need to expose collection data publicly without allowing external modification.

#### Core Design Principles:
- **Wrapper Pattern**: Acts as a proxy to an existing `IList<T>`
- **Live Reference**: Maintains a permanent reference to the wrapped collection
- **No Data Copying**: Changes to the underlying collection are immediately visible
- **Immutable Interface**: Throws `NotSupportedException` for any modification attempts

#### Implementation Architecture:
```csharp
public class GameManager
{
    private readonly List<int> scores;           // Internal mutable collection
    public ReadOnlyCollection<int> Scores { get; }  // Public read-only view
    
    public GameManager()
    {
        scores = new List<int>();
        Scores = new ReadOnlyCollection<int>(scores); // Create live wrapper
    }
    
    // Internal methods can modify the underlying collection
    public void AddScore(int score)
    {
        scores.Add(score); // Changes are immediately visible through Scores property
    }
}
```

#### Security and Robustness:
Unlike simply exposing `IReadOnlyList<T>`, `ReadOnlyCollection<T>` provides stronger guarantees:

```csharp
// This approach can be bypassed at runtime
public IReadOnlyList<string> Items => internalList; // Dangerous

// This can be downcast in some scenarios
if (manager.Items is IList<string> mutableList)
    mutableList.Add("Bypassed!"); // Might succeed

// ReadOnlyCollection prevents this
public ReadOnlyCollection<string> Items => readOnlyWrapper; // Safe

// This will always throw NotSupportedException
((IList<string>)manager.Items).Add("Will fail"); // Runtime exception
```

#### Usage Patterns:
- **Configuration Management**: Expose configuration items without allowing external changes
- **Entity Relationships**: Expose child entities in domain models
- **API Design**: Provide collection data in public APIs
- **Event Aggregation**: Expose event history without allowing manipulation

#### When to Use ReadOnlyCollection<T>:
- Need to expose internal collections publicly
- Want to prevent accidental external modifications
- Require stronger guarantees than `IReadOnlyList<T>`
- Building APIs where data integrity is critical

### 4. Legacy Collection Classes - Historical Context and Migration Guidance

The .NET Framework includes two legacy base classes that predate the generic collection era. Understanding these classes is important for maintaining existing code and appreciating the evolution of collection design patterns.

#### CollectionBase - Non-Generic Collection Foundation

`CollectionBase` was the original base class for creating strongly-typed collections before generics were introduced in .NET 2.0. It provides similar functionality to `Collection<T>` but with significant limitations.

**Key Limitations:**
- **No Type Safety**: All operations work with `object` references
- **Boxing/Unboxing**: Value types incur performance penalties
- **Verbose Implementation**: Requires manual implementation of typed methods
- **Hook Method Pairs**: Uses before/after method pairs instead of single gateway methods

**Hook Method Pattern:**
```csharp
protected virtual void OnInsert(int index, object value);
protected virtual void OnInsertComplete(int index, object value);
protected virtual void OnRemove(int index, object value);
protected virtual void OnRemoveComplete(int index, object value);
protected virtual void OnSet(int index, object oldValue, object newValue);
protected virtual void OnSetComplete(int index, object oldValue, object newValue);
```

**Implementation Requirements:**
```csharp
public class TaskCollection : CollectionBase
{
    // Must implement strongly-typed methods manually
    public void Add(Task task) => List.Add(task);
    public Task this[int index] 
    {
        get => (Task)List[index];
        set => List[index] = value;
    }
    
    // Must implement validation
    protected override void OnValidate(object value)
    {
        if (value is not Task)
            throw new ArgumentException("Value must be a Task");
    }
}
```

#### DictionaryBase - Non-Generic Dictionary Foundation

`DictionaryBase` provides the legacy equivalent of keyed collections with similar limitations to `CollectionBase`.

**Characteristics:**
- **Implements IDictionary**: Direct dictionary interface implementation
- **Non-Generic**: Keys and values are untyped `object` references
- **Hook Method Pairs**: Before/after patterns for all operations
- **Manual Type Implementation**: Requires custom strongly-typed interface

**Implementation Pattern:**
```csharp
public class PhoneBook : DictionaryBase
{
    public void Add(string name, string phone) => Dictionary.Add(name, phone);
    public string this[string name]
    {
        get => (string)Dictionary[name];
        set => Dictionary[name] = value;
    }
    
    protected override void OnValidate(object key, object value)
    {
        if (key is not string || value is not string)
            throw new ArgumentException("Keys and values must be strings");
    }
}
```

#### Why Legacy Classes Should Be Avoided:

1. **Type Safety**: Generic alternatives provide compile-time type checking
2. **Performance**: Avoid boxing/unboxing overhead for value types
3. **Maintainability**: Less code required with modern alternatives
4. **Developer Experience**: Better IntelliSense and debugging support
5. **Modern Language Features**: Integration with LINQ, async/await, etc.

#### Migration Strategy:
- **CollectionBase → Collection<T>**: Direct replacement with generic equivalent
- **DictionaryBase → KeyedCollection<TKey, TItem>**: For collections with natural keys
- **DictionaryBase → Dictionary<TKey, TValue>**: For pure key-value scenarios

#### When You Might Encounter Legacy Classes:
- **Existing Codebases**: Maintenance of older applications
- **Third-Party Libraries**: Legacy APIs that haven't been updated
- **Framework Integration**: Some older .NET Framework APIs still use these patterns
- **Backward Compatibility**: When supporting older .NET Framework versions

## Advanced Implementation Patterns

### Understanding Proxy Behavior and Reference Semantics

The proxy pattern in customizable collections creates important behavioral characteristics that developers must understand:

#### Live Reference Behavior
When `Collection<T>` wraps an existing `IList<T>`, it maintains a permanent reference rather than creating a copy:

```csharp
var originalList = new List<string> { "A", "B", "C" };
var wrappedCollection = new Collection<string>(originalList);

// Changes through wrapper affect original
wrappedCollection.Add("D");
Console.WriteLine(originalList.Count); // Output: 4

// Changes to original are visible through wrapper
originalList.Add("E");
Console.WriteLine(wrappedCollection.Count); // Output: 5
```

#### Virtual Method Bypass
Direct modifications to the underlying collection bypass the virtual gateway methods:

```csharp
public class LoggingCollection<T> : Collection<T>
{
    protected override void InsertItem(int index, T item)
    {
        Console.WriteLine($"Adding: {item}");
        base.InsertItem(index, item);
    }
}

var list = new List<string>();
var wrapper = new LoggingCollection<string>(list);

wrapper.Add("Test");     // Triggers logging
list.Add("Direct");      // NO logging - bypasses virtual method
```

### Key Change Management in KeyedCollection

Proper key change handling is critical for maintaining collection integrity:

#### The Challenge
When an item's key changes after being added to a `KeyedCollection`, the internal dictionary becomes inconsistent:

```csharp
var animals = new AnimalKeyedCollection();
var kangaroo = new Animal("Kangaroo", 85);
animals.Add(kangaroo);

// Direct property change breaks the collection
kangaroo.Name = "Mr Roo"; // Internal dictionary still has "Kangaroo" key

// This will fail because the key mapping is broken
var found = animals["Mr Roo"]; // KeyNotFoundException
```

#### The Solution
Implement proper key change notification:

```csharp
public class Animal
{
    private string name;
    public string Name
    {
        get => name;
        set
        {
            // Notify collection before changing the value
            if (ParentCollection != null)
                ParentCollection.ChangeItemKey(this, value);
            name = value;
        }
    }
    
    internal AnimalKeyedCollection ParentCollection { get; set; }
}

public class AnimalKeyedCollection : KeyedCollection<string, Animal>
{
    protected override void InsertItem(int index, Animal item)
    {
        base.InsertItem(index, item);
        item.ParentCollection = this; // Establish back-reference
    }
    
    protected override void RemoveItem(int index)
    {
        this[index].ParentCollection = null; // Clear back-reference
        base.RemoveItem(index);
    }
    
    internal void NotifyKeyChange(Animal item, string newKey)
    {
        this.ChangeItemKey(item, newKey);
    }
}
```

## Running the Demo

The project includes comprehensive demonstrations of all concepts:

1. **Collection<T>** - Basic customization with Zoo/Animal example
2. **KeyedCollection basics** - Library/Book example with dual access
3. **KeyedCollection advanced** - Key change notifications
4. **CollectionBase** - Legacy approach (avoid)
5. **DictionaryBase** - Legacy dictionary approach (avoid)
6. **ReadOnlyCollection<T>** - Safe public exposure
7. **Proxy behavior** - Live wrapper demonstration

Each demo includes detailed console output explaining what's happening and why these patterns are useful.

## Design Principles and Best Practices

### Virtual Method Implementation Guidelines

#### Always Call Base Methods
The most critical rule when overriding virtual methods is to call the base implementation:

```csharp
protected override void InsertItem(int index, T item)
{
    // Custom logic before
    ValidateItem(item);
    
    // ALWAYS call base - this performs the actual operation
    base.InsertItem(index, item);
    
    // Custom logic after
    NotifyItemAdded(item);
}
```

#### Order of Operations Matters
For insertion and setting operations, call base first, then apply custom logic:

```csharp
protected override void InsertItem(int index, T item)
{
    base.InsertItem(index, item);  // Add to collection first
    item.Parent = this;            // Then establish relationships
}
```

For removal operations, apply custom logic first, then call base:

```csharp
protected override void RemoveItem(int index)
{
    var item = this[index];        // Get reference before removal
    item.Parent = null;            // Clean up relationships first
    base.RemoveItem(index);        // Then remove from collection
}
```

### Thread Safety Considerations

Customizable collections are not thread-safe by default. For concurrent access scenarios:

#### Option 1: External Synchronization
```csharp
private readonly object lockObject = new object();

public void SafeAdd(T item)
{
    lock (lockObject)
    {
        this.Add(item);
    }
}
```

#### Option 2: Override Virtual Methods with Locking
```csharp
public class ThreadSafeCollection<T> : Collection<T>
{
    private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
    
    protected override void InsertItem(int index, T item)
    {
        rwLock.EnterWriteLock();
        try
        {
            base.InsertItem(index, item);
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }
    
    // Implement read operations with read locks
    public T SafeGet(int index)
    {
        rwLock.EnterReadLock();
        try
        {
            return this[index];
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }
}
```

### Performance Optimization Strategies

#### Batch Operations
When performing multiple operations, consider overriding methods to optimize batch scenarios:

```csharp
public class OptimizedCollection<T> : Collection<T>
{
    private bool suppressNotifications = false;
    
    public void AddRange(IEnumerable<T> items)
    {
        suppressNotifications = true;
        try
        {
            foreach (var item in items)
                Add(item);
        }
        finally
        {
            suppressNotifications = false;
            OnCollectionChanged(); // Single notification for all changes
        }
    }
    
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        if (!suppressNotifications)
            OnItemAdded(item);
    }
}
```

#### Lazy Initialization
Defer expensive operations until they are actually needed:

```csharp
public class LazyValidationCollection<T> : Collection<T> where T : IValidatable
{
    private bool validationEnabled = false;
    
    public void EnableValidation()
    {
        validationEnabled = true;
        // Validate existing items
        foreach (var item in this)
            ValidateItem(item);
    }
    
    protected override void InsertItem(int index, T item)
    {
        if (validationEnabled)
            ValidateItem(item);
        base.InsertItem(index, item);
    }
}
```

### Error Handling and Validation

#### Comprehensive Validation Strategy
```csharp
public class ValidatedCollection<T> : Collection<T> where T : class, IValidatable
{
    protected override void InsertItem(int index, T item)
    {
        // Null check
        ArgumentNullException.ThrowIfNull(item);
        
        // Range validation
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        // Business rule validation
        if (!item.IsValid(out string validationError))
            throw new InvalidOperationException($"Item validation failed: {validationError}");
        
        // Check for duplicates if required
        if (this.Any(existingItem => existingItem.Id == item.Id))
            throw new InvalidOperationException("Duplicate item detected");
        
        base.InsertItem(index, item);
    }
}
```

## Real-World Application Scenarios

### Enterprise Domain Modeling

#### Order Management System
```csharp
public class Order
{
    public OrderLineCollection Lines { get; }
    public decimal Total { get; private set; }
    
    public Order()
    {
        Lines = new OrderLineCollection(this);
    }
    
    internal void RecalculateTotal()
    {
        Total = Lines.Sum(line => line.ExtendedPrice);
    }
}

public class OrderLineCollection : Collection<OrderLine>
{
    private readonly Order parentOrder;
    
    public OrderLineCollection(Order order)
    {
        parentOrder = order;
    }
    
    protected override void InsertItem(int index, OrderLine item)
    {
        item.Order = parentOrder;
        base.InsertItem(index, item);
        parentOrder.RecalculateTotal(); // Automatic total updates
    }
    
    protected override void RemoveItem(int index)
    {
        this[index].Order = null;
        base.RemoveItem(index);
        parentOrder.RecalculateTotal();
    }
}
```

#### Configuration Management System
```csharp
public class ConfigurationSection : KeyedCollection<string, ConfigurationItem>
{
    private readonly IConfigurationStore store;
    private readonly string sectionName;
    
    public ConfigurationSection(string name, IConfigurationStore configStore)
    {
        sectionName = name;
        store = configStore;
    }
    
    protected override string GetKeyForItem(ConfigurationItem item) => item.Key;
    
    protected override void InsertItem(int index, ConfigurationItem item)
    {
        ValidateConfigurationItem(item);
        base.InsertItem(index, item);
        store.SaveConfiguration(sectionName, this.ToArray());
        LogConfigurationChange("Added", item);
    }
    
    protected override void SetItem(int index, ConfigurationItem item)
    {
        var oldItem = this[index];
        ValidateConfigurationItem(item);
        base.SetItem(index, item);
        store.SaveConfiguration(sectionName, this.ToArray());
        LogConfigurationChange("Modified", item, oldItem);
    }
    
    private void ValidateConfigurationItem(ConfigurationItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Key))
            throw new ArgumentException("Configuration key cannot be empty");
        
        if (item.Value == null)
            throw new ArgumentException("Configuration value cannot be null");
    }
}
```

### User Interface Framework Integration

#### Control Collections for UI Frameworks
```csharp
public class ControlCollection : Collection<UIControl>
{
    private readonly ContainerControl parent;
    
    public ControlCollection(ContainerControl parentControl)
    {
        parent = parentControl;
    }
    
    protected override void InsertItem(int index, UIControl item)
    {
        if (item.Parent != null)
            throw new InvalidOperationException("Control already has a parent");
        
        base.InsertItem(index, item);
        item.Parent = parent;
        parent.OnChildControlAdded(item);
        item.Invalidate(); // Trigger repaint
    }
    
    protected override void RemoveItem(int index)
    {
        var control = this[index];
        base.RemoveItem(index);
        control.Parent = null;
        parent.OnChildControlRemoved(control);
        parent.Invalidate(); // Trigger parent repaint
    }
}
```

### Data Access and ORM Integration

#### Entity Collection with Change Tracking
```csharp
public class EntityCollection<T> : Collection<T> where T : class, IEntity
{
    private readonly HashSet<T> addedItems = new HashSet<T>();
    private readonly HashSet<T> removedItems = new HashSet<T>();
    private readonly IEntityContext context;
    
    public EntityCollection(IEntityContext entityContext)
    {
        context = entityContext;
    }
    
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        
        if (removedItems.Contains(item))
            removedItems.Remove(item); // Item was re-added
        else
            addedItems.Add(item); // New item
        
        context.MarkAsModified(this);
    }
    
    protected override void RemoveItem(int index)
    {
        var item = this[index];
        base.RemoveItem(index);
        
        if (addedItems.Contains(item))
            addedItems.Remove(item); // Remove from added list
        else
            removedItems.Add(item); // Track as removed
        
        context.MarkAsModified(this);
    }
    
    public void AcceptChanges()
    {
        addedItems.Clear();
        removedItems.Clear();
    }
    
    public IEnumerable<T> GetAddedItems() => addedItems.ToArray();
    public IEnumerable<T> GetRemovedItems() => removedItems.ToArray();
}
```

### Caching and Performance Optimization

#### Cached Collection with Automatic Invalidation
```csharp
public class CachedCollection<T> : Collection<T> where T : ICacheableEntity
{
    private readonly ICache cache;
    private readonly TimeSpan cacheExpiration;
    
    public CachedCollection(ICache cacheProvider, TimeSpan expiration)
    {
        cache = cacheProvider;
        cacheExpiration = expiration;
    }
    
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        cache.Set(GenerateCacheKey(item), item, cacheExpiration);
        InvalidateCollectionCache();
    }
    
    protected override void RemoveItem(int index)
    {
        var item = this[index];
        base.RemoveItem(index);
        cache.Remove(GenerateCacheKey(item));
        InvalidateCollectionCache();
    }
    
    public T FindCached(string id)
    {
        var cacheKey = $"entity:{typeof(T).Name}:{id}";
        return cache.Get<T>(cacheKey) ?? this.FirstOrDefault(x => x.Id == id);
    }
    
    private string GenerateCacheKey(T item) => $"entity:{typeof(T).Name}:{item.Id}";
    private void InvalidateCollectionCache() => cache.Remove($"collection:{typeof(T).Name}");
}
```

## Integration with Modern Development Practices

### Dependency Injection and IoC Containers

```csharp
public class ServiceCollection : KeyedCollection<Type, IServiceDescriptor>
{
    protected override Type GetKeyForItem(IServiceDescriptor item) => item.ServiceType;
    
    protected override void InsertItem(int index, IServiceDescriptor item)
    {
        ValidateServiceDescriptor(item);
        base.InsertItem(index, item);
        NotifyContainerOfChange();
    }
    
    public void RegisterSingleton<TInterface, TImplementation>()
        where TImplementation : class, TInterface
    {
        var descriptor = new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), ServiceLifetime.Singleton);
        this.Add(descriptor);
    }
}
```

### Event-Driven Architecture

```csharp
public class EventSourcedCollection<T> : Collection<T> where T : IEventSourced
{
    public event EventHandler<CollectionChangedEventArgs<T>> ItemAdded;
    public event EventHandler<CollectionChangedEventArgs<T>> ItemRemoved;
    
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        OnItemAdded(new CollectionChangedEventArgs<T>(item, index));
    }
    
    protected override void RemoveItem(int index)
    {
        var item = this[index];
        base.RemoveItem(index);
        OnItemRemoved(new CollectionChangedEventArgs<T>(item, index));
    }
    
    protected virtual void OnItemAdded(CollectionChangedEventArgs<T> e)
    {
        ItemAdded?.Invoke(this, e);
    }
    
    protected virtual void OnItemRemoved(CollectionChangedEventArgs<T> e)
    {
        ItemRemoved?.Invoke(this, e);
    }
}
```

## Summary and Decision Framework

### Choosing the Right Collection Base Class

Use this decision matrix to select the appropriate base class for your requirements:

| Requirement | Recommended Class | Rationale |
|-------------|------------------|-----------|
| Basic customization needs | `Collection<T>` | Simple virtual methods, broad compatibility |
| Need both indexed and keyed access | `KeyedCollection<TKey, TItem>` | Combines list and dictionary semantics |
| Expose internal collection safely | `ReadOnlyCollection<T>` | Prevents external modification while allowing internal changes |
| Legacy codebase maintenance | `CollectionBase/DictionaryBase` | Only when required for backward compatibility |
| High-performance scenarios | Custom implementation | Consider implementing `IList<T>` directly for maximum control |

### Key Architectural Benefits

1. **Separation of Concerns**: Collection behavior is separated from business logic
2. **Single Responsibility**: Each virtual method handles one specific operation
3. **Open/Closed Principle**: Collections are open for extension but closed for modification
4. **Liskov Substitution**: Custom collections can replace standard collections seamlessly
5. **Interface Segregation**: Clients depend only on the collection interfaces they need

### Common Anti-Patterns to Avoid

1. **Forgetting Base Calls**: Always call `base.Method()` in overrides
2. **Inconsistent State**: Ensure all virtual methods maintain object invariants
3. **Performance Neglect**: Consider the impact of custom logic in tight loops
4. **Thread Safety Assumptions**: Customizable collections are not thread-safe by default
5. **Circular Dependencies**: Avoid situations where collection changes trigger infinite updates

### Testing Strategies

When implementing customizable collections, ensure comprehensive testing:

```csharp
[Test]
public void InsertItem_ShouldMaintainRelationships()
{
    var zoo = new Zoo();
    var animal = new Animal("Lion", 95);
    
    zoo.Animals.Add(animal);
    
    Assert.That(animal.Zoo, Is.EqualTo(zoo));
    Assert.That(zoo.Animals.Count, Is.EqualTo(1));
    Assert.That(zoo.Animals[0], Is.EqualTo(animal));
}

[Test]
public void RemoveItem_ShouldCleanupRelationships()
{
    var zoo = new Zoo();
    var animal = new Animal("Lion", 95);
    zoo.Animals.Add(animal);
    
    zoo.Animals.Remove(animal);
    
    Assert.That(animal.Zoo, Is.Null);
    Assert.That(zoo.Animals.Count, Is.EqualTo(0));
}
```

Understanding these customizable collection patterns enables you to build robust, maintainable applications that properly encapsulate collection behavior while maintaining clean separation between data structures and business logic.
