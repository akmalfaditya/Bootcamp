# Customizable Collections and Proxies 🔧

## 🎓 Learning Objectives
Learn to extend and customize .NET's collection framework by creating **collection proxies** and **custom collections** that add specialized behavior while maintaining compatibility with existing code. Master the art of hooking into collection operations to implement validation, logging, change tracking, and business rules.

## 🔍 What You'll Learn

### Collection Extensibility Patterns
- **Collection<T> Class**: The foundation for customizable generic collections
- **KeyedCollection<TKey, TItem>**: Collections with built-in key-based lookup
- **CollectionBase Class**: Legacy base class for strongly-typed collections
- **ReadOnlyCollection<T>**: Creating immutable collection views

### Proxy and Wrapper Patterns
- **Operation Interception**: Hooking into add, remove, and update operations
- **Validation Logic**: Enforcing business rules at the collection level
- **Change Notification**: Implementing collection change events
- **Lazy Loading**: Collections that load data on demand

### Advanced Collection Techniques
- **Composite Collections**: Combining multiple collections into unified views
- **Filtered Collections**: Collections that automatically filter content
- **Synchronized Collections**: Thread-safe collection wrappers
- **Observable Collections**: Collections that notify of changes for data binding

## 🚀 Key Features Demonstrated

### 1. **Collection<T> Customization**
```csharp
public class Zoo
{
    private AnimalCollection _animals = new AnimalCollection();
    public AnimalCollection Animals => _animals;
    
    public class AnimalCollection : Collection<Animal>
    {
        protected override void InsertItem(int index, Animal item)
        {
            item.Zoo = this;  // Automatically assign zoo reference
            base.InsertItem(index, item);
        }
        
        protected override void SetItem(int index, Animal item)
        {
            item.Zoo = this;
            base.SetItem(index, item);
        }
    }
}
```

### 2. **KeyedCollection Implementation**
```csharp
public class PersonCollection : KeyedCollection<string, Person>
{
    protected override string GetKeyForItem(Person item) => item.Email;
    
    // Automatic lookup by key
    public Person FindByEmail(string email) => this[email];
}
```

### 3. **Validation and Business Rules**
```csharp
public class ValidatedList<T> : Collection<T> where T : IValidatable
{
    protected override void InsertItem(int index, T item)
    {
        if (!item.IsValid())
            throw new ArgumentException("Item failed validation");
            
        base.InsertItem(index, item);
    }
}
```

### 4. **ReadOnly Collection Wrapper**
```csharp
public class DatabaseEntityCollection<T> : ReadOnlyCollection<T>
{
    public DatabaseEntityCollection(IList<T> list) : base(list) { }
    
    // Expose only read operations to external code
    public T FindById(int id) => this.FirstOrDefault(x => x.Id == id);
}
```

## 💡 Trainer Tips

### **When to Use Each Base Class**
- **Collection<T>**: Most versatile, use for general-purpose customizable collections
- **KeyedCollection<TKey, TItem>**: When you need both indexed and keyed access
- **CollectionBase**: Legacy scenarios or when you need non-generic base behavior
- **ReadOnlyCollection<T>**: When you want to expose internal collections safely

### **Performance Considerations**
```csharp
// Override multiple methods to maintain efficiency
public class OptimizedCollection<T> : Collection<T>
{
    protected override void ClearItems()
    {
        // Custom cleanup logic before clearing
        foreach (var item in this)
        {
            CleanupItem(item);
        }
        base.ClearItems();
    }
    
    protected override void RemoveItem(int index)
    {
        var item = this[index];
        CleanupItem(item);
        base.RemoveItem(index);
    }
}
```

### **Thread Safety Patterns**
```csharp
public class ThreadSafeCollection<T> : Collection<T>
{
    private readonly object _lock = new object();
    
    protected override void InsertItem(int index, T item)
    {
        lock (_lock)
        {
            base.InsertItem(index, item);
        }
    }
    
    // Implement all other operations with locking
}
```

### **Change Notification Implementation**
```csharp
public class NotifyingCollection<T> : Collection<T>, INotifyCollectionChanged
{
    public event NotifyCollectionChangedEventHandler CollectionChanged;
    
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction.Add, item, index));
    }
}
```

## 🌍 Real-World Applications

### **Business Entity Management**
```csharp
public class OrderLineCollection : KeyedCollection<int, OrderLine>
{
    private Order _parentOrder;
    
    public OrderLineCollection(Order parent)
    {
        _parentOrder = parent;
    }
    
    protected override int GetKeyForItem(OrderLine item) => item.ProductId;
    
    protected override void InsertItem(int index, OrderLine item)
    {
        item.Order = _parentOrder;  // Maintain parent relationship
        _parentOrder.RecalculateTotal();  // Update calculated fields
        base.InsertItem(index, item);
    }
    
    protected override void RemoveItem(int index)
    {
        base.RemoveItem(index);
        _parentOrder.RecalculateTotal();
    }
}
```

### **Configuration Management**
```csharp
public class ConfigurationSection : KeyedCollection<string, ConfigurationItem>
{
    protected override string GetKeyForItem(ConfigurationItem item) => item.Key;
    
    protected override void SetItem(int index, ConfigurationItem item)
    {
        // Log configuration changes
        var oldItem = this[index];
        Logger.LogConfigChange(oldItem, item);
        
        // Validate new configuration
        ValidateConfiguration(item);
        
        base.SetItem(index, item);
    }
}
```

### **Caching Layer**
```csharp
public class CachedCollection<T> : Collection<T> where T : ICacheableEntity
{
    private readonly ICache _cache;
    
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        _cache.Set(item.CacheKey, item, TimeSpan.FromMinutes(30));
    }
    
    protected override void RemoveItem(int index)
    {
        var item = this[index];
        _cache.Remove(item.CacheKey);
        base.RemoveItem(index);
    }
}
```

### **Audit Trail Implementation**
```csharp
public class AuditableCollection<T> : Collection<T> where T : IAuditable
{
    private readonly IAuditLogger _auditLogger;
    
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        _auditLogger.LogAction("Added", item, DateTime.UtcNow);
    }
    
    protected override void RemoveItem(int index)
    {
        var item = this[index];
        base.RemoveItem(index);
        _auditLogger.LogAction("Removed", item, DateTime.UtcNow);
    }
}
```

## ✅ Mastery Checklist

### Beginner Level
- [ ] Understand the purpose of collection base classes
- [ ] Create a simple Collection<T> with custom behavior
- [ ] Override InsertItem and RemoveItem methods
- [ ] Use ReadOnlyCollection<T> to expose internal collections

### Intermediate Level
- [ ] Implement KeyedCollection<TKey, TItem> for dual access patterns
- [ ] Add validation logic to collection operations
- [ ] Implement change notification for data binding scenarios
- [ ] Create thread-safe collection wrappers

### Advanced Level
- [ ] Design complex business-rule-enforcing collections
- [ ] Implement performance-optimized collection operations
- [ ] Create lazy-loading collection proxies
- [ ] Build composite collections that combine multiple data sources

## 🔧 Integration with Modern C#

### **Generic Constraints (C# 2+)**
```csharp
public class ValidatedCollection<T> : Collection<T> 
    where T : class, IValidatable, new()
{
    // Use constraints to ensure type safety and capabilities
}
```

### **Pattern Matching (C# 7+)**
```csharp
protected override void InsertItem(int index, T item)
{
    var validationResult = item switch
    {
        IValidatable validatable => validatable.Validate(),
        _ => ValidationResult.Success
    };
    
    if (!validationResult.IsValid)
        throw new ValidationException(validationResult.ErrorMessage);
        
    base.InsertItem(index, item);
}
```

### **Nullable Reference Types (C# 8+)**
```csharp
public class SafeCollection<T> : Collection<T> where T : class?
{
    protected override void InsertItem(int index, T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        base.InsertItem(index, item);
    }
}
```

### **Record Types (C# 9+)**
```csharp
public record CollectionChangeEvent(string Action, object Item, DateTime Timestamp);

public class EventEmittingCollection<T> : Collection<T>
{
    public event Action<CollectionChangeEvent>? Changed;
    
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        Changed?.Invoke(new("Added", item, DateTime.UtcNow));
    }
}
```

## 🏆 Industry Impact

Customizable collections are essential because they:

- **Enforce Business Rules**: Automatically validate data at the collection level
- **Enable Framework Integration**: Provide hooks for ORMs, caching, and logging systems
- **Support Data Binding**: Essential for WPF, WinUI, and web UI frameworks
- **Improve Performance**: Add caching, lazy loading, and optimization strategies
- **Maintain Data Integrity**: Ensure consistent state across complex object graphs

## 📚 Advanced Topics to Explore

- **Concurrent Collections**: Building thread-safe collections with high performance
- **Reactive Collections**: Integrating with reactive programming frameworks
- **Persistence Collections**: Collections that automatically save changes to storage
- **Virtual Collections**: Collections that appear infinite but load data on demand
- **Projection Collections**: Collections that transform data on-the-fly

---

*Master customizable collections, and you'll be able to create robust, business-aware data structures that enforce rules, optimize performance, and integrate seamlessly with modern application architectures!* 🔧
