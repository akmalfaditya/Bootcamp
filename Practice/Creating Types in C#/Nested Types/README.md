# Nested Types 🪆

## 🎯 Learning Objectives

By mastering this project, you will:
- **Understand Type Nesting**: Learn how to define types inside other types for better organization
- **Master Access Control**: Use access modifiers to control nested type visibility
- **Leverage Private Access**: Exploit nested types' ability to access outer class private members
- **Apply Inheritance Patterns**: Use protected nested types in inheritance hierarchies
- **Build Clean APIs**: Create sophisticated designs with simple external interfaces
- **Implement Design Patterns**: Use nested types in Builder, State Machine, and Factory patterns
- **Optimize Code Organization**: Group related functionality while maintaining encapsulation

## 📚 Core Concepts

### What Are Nested Types?
Nested types are types defined inside other types. Think of them as "Russian dolls" - types within types. Any type can contain nested types: classes, structs, interfaces, enums, and delegates.

```csharp
public class OuterClass
{
    // Nested class
    public class NestedClass
    {
        public void DoSomething() { }
    }
    
    // Nested enum
    public enum Status { Active, Inactive }
    
    // Nested struct
    public struct Point { public int X, Y; }
}

// Usage
var nested = new OuterClass.NestedClass();
```

### Access Modifiers for Nested Types
Nested types can use all access modifiers, giving you fine-grained control over visibility:

```csharp
public class Container
{
    public class PublicNested { }        // Anyone can access
    private class PrivateNested { }      // Only Container can access
    protected class ProtectedNested { }  // Container and derived classes
    internal class InternalNested { }    // Same assembly only
    protected internal class MixedNested { } // Protected OR internal
}
```

### Private Member Access
The superpower of nested types: they can access ALL private members of their containing type:

```csharp
public class BankAccount
{
    private decimal balance;  // Private field
    private string accountId; // Private field
    
    public class Transaction  // Nested class
    {
        public void ProcessPayment(BankAccount account, decimal amount)
        {
            // Can directly access private members!
            account.balance -= amount;
            Console.WriteLine($"Account {account.accountId} charged ${amount}");
        }
    }
}
```

## 🚀 Key Features

### 1. **Basic Nested Type Declaration**
```csharp
public class OuterClass
{
    private string outerField = "Outer data";
    
    public class NestedClass
    {
        public void AccessOuter(OuterClass outer)
        {
            // Can access private members of outer class
            Console.WriteLine(outer.outerField);
        }
    }
    
    public void UseNested()
    {
        var nested = new NestedClass();
        nested.AccessOuter(this);
    }
}
```

### 2. **Protected Nested Types with Inheritance**
```csharp
public class Employee
{
    protected string employeeId;
    protected decimal salary;
    
    protected class PayrollCalculator
    {
        public decimal CalculateBonus(Employee emp, decimal percentage)
        {
            // Access protected members in derived classes
            return emp.salary * percentage;
        }
    }
}

public class Manager : Employee
{
    public void ProcessBonuses()
    {
        var calculator = new PayrollCalculator();  // Can access protected nested type
        decimal bonus = calculator.CalculateBonus(this, 0.15m);
    }
}
```

### 3. **Builder Pattern with Nested Builder**
```csharp
public class Pizza
{
    public enum SizeType { Small, Medium, Large }
    public enum CrustType { Thin, Thick, Stuffed }
    
    private SizeType size;
    private CrustType crust;
    private List<string> toppings = new();
    
    private Pizza() { } // Private constructor
    
    public static Builder CreateBuilder() => new Builder();
    
    public class Builder  // Nested builder class
    {
        private Pizza pizza = new Pizza();
        
        public Builder WithSize(SizeType size)
        {
            pizza.size = size;
            return this;
        }
        
        public Builder WithCrust(CrustType crust)
        {
            pizza.crust = crust;
            return this;
        }
        
        public Builder AddToppings(params string[] toppings)
        {
            pizza.toppings.AddRange(toppings);
            return this;
        }
        
        public Pizza Build() => pizza;
    }
}

// Usage
var pizza = Pizza.CreateBuilder()
    .WithSize(Pizza.SizeType.Large)
    .WithCrust(Pizza.CrustType.Thin)
    .AddToppings("Pepperoni", "Mushrooms")
    .Build();
```

### 4. **State Machine Pattern**
```csharp
public class OrderProcessor
{
    private string orderId;
    private IOrderState currentState;
    
    public OrderProcessor(string orderId)
    {
        this.orderId = orderId;
        this.currentState = new PendingState();
    }
    
    private interface IOrderState
    {
        void ProcessOrder(OrderProcessor processor);
        void ShipOrder(OrderProcessor processor);
        string GetStatus();
    }
    
    private class PendingState : IOrderState
    {
        public void ProcessOrder(OrderProcessor processor)
        {
            processor.currentState = new ProcessingState();
            Console.WriteLine("Order moved to processing");
        }
        
        public void ShipOrder(OrderProcessor processor) =>
            throw new InvalidOperationException("Cannot ship pending order");
        
        public string GetStatus() => "Pending";
    }
    
    private class ProcessingState : IOrderState
    {
        public void ProcessOrder(OrderProcessor processor) =>
            Console.WriteLine("Order already processing");
        
        public void ShipOrder(OrderProcessor processor)
        {
            processor.currentState = new ShippedState();
            Console.WriteLine("Order shipped");
        }
        
        public string GetStatus() => "Processing";
    }
    
    private class ShippedState : IOrderState
    {
        public void ProcessOrder(OrderProcessor processor) =>
            Console.WriteLine("Order already processed");
        
        public void ShipOrder(OrderProcessor processor) =>
            Console.WriteLine("Order already shipped");
        
        public string GetStatus() => "Shipped";
    }
}
```

### 5. **Secure Data Container**
```csharp
public class SecureDataContainer<T>
{
    private T[] data;
    private bool[] isEncrypted;
    private string securityKey;
    
    public SecureDataContainer(int capacity)
    {
        data = new T[capacity];
        isEncrypted = new bool[capacity];
        securityKey = GenerateSecurityKey();
    }
    
    public SecureAccessor GetSecureAccessor() => new SecureAccessor(this);
    
    public class SecureAccessor  // Nested class for controlled access
    {
        private SecureDataContainer<T> container;
        
        internal SecureAccessor(SecureDataContainer<T> container)
        {
            this.container = container;
        }
        
        public void StoreSecurely(int index, T value, bool encrypt)
        {
            // Direct access to private members
            container.data[index] = encrypt ? EncryptValue(value) : value;
            container.isEncrypted[index] = encrypt;
        }
        
        private T EncryptValue(T value)
        {
            // Access private security key
            Console.WriteLine($"Encrypting with key: {container.securityKey[..4]}...");
            return value; // Simplified encryption
        }
    }
}
```

## 💡 Trainer Tips

### When to Use Nested Types
- **Tight Coupling**: When the nested type only makes sense in context of the outer type
- **Implementation Hiding**: When you want to hide complex internal structures
- **Access Control**: When you need controlled access to private members
- **Logical Grouping**: When types are conceptually related and used together

### Access Modifier Strategy
- **Public**: For types that clients need to use directly
- **Private**: For internal helper classes and implementation details
- **Protected**: For types that derived classes should access
- **Internal**: For types used within the same assembly

### Performance Considerations
- **No Performance Penalty**: Nested types have same performance as regular types
- **Memory Layout**: Nested types don't affect outer class memory layout
- **Static vs Instance**: Nested types can be static (no outer instance needed)

## 🎓 Real-World Applications

### Configuration Classes
```csharp
public class DatabaseConfig
{
    private string connectionString;
    
    public class ConnectionPool
    {
        private DatabaseConfig config;
        
        public Connection GetConnection()
        {
            // Access private connection string
            return new Connection(config.connectionString);
        }
    }
    
    public class QueryBuilder
    {
        public string BuildSelectQuery(string table, string[] columns)
        {
            return $"SELECT {string.Join(", ", columns)} FROM {table}";
        }
    }
}
```

### Event Handling Systems
```csharp
public class EventManager
{
    private List<IEventHandler> handlers = new();
    
    public interface IEventHandler
    {
        void Handle(Event evt);
    }
    
    public class Event
    {
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
        public object Data { get; set; }
    }
    
    public class EventHandlerRegistry
    {
        private EventManager manager;
        
        public void RegisterHandler<T>(Action<T> handler) where T : Event
        {
            // Complex registration logic with access to private members
        }
    }
}
```

### API Response Wrappers
```csharp
public class ApiResponse<T>
{
    private T data;
    private bool success;
    private List<string> errors = new();
    
    public class Builder
    {
        private ApiResponse<T> response = new();
        
        public Builder WithData(T data)
        {
            response.data = data;
            response.success = true;
            return this;
        }
        
        public Builder WithError(string error)
        {
            response.errors.Add(error);
            response.success = false;
            return this;
        }
        
        public ApiResponse<T> Build() => response;
    }
}
```

## 🎯 Mastery Checklist

### Beginner Level
- [ ] Create basic nested classes and use them
- [ ] Understand public vs private nested types
- [ ] Access outer class members from nested types
- [ ] Use proper syntax for nested type instantiation
- [ ] Implement simple nested enumerations

### Intermediate Level
- [ ] Use protected nested types with inheritance
- [ ] Implement Builder pattern with nested builder
- [ ] Create nested interfaces and implement them
- [ ] Design secure APIs using private nested types
- [ ] Handle complex access modifier scenarios

### Advanced Level
- [ ] Implement State Machine pattern with nested states
- [ ] Create generic nested types with constraints
- [ ] Use nested types in event-driven architectures
- [ ] Design plugin systems with nested interfaces
- [ ] Optimize complex object hierarchies

## 💼 Industry Impact

Mastering nested types is valuable for:

**Framework Development**: Creating clean APIs that hide implementation complexity
**Design Patterns**: Implementing Builder, State Machine, and Factory patterns elegantly
**Library Design**: Providing controlled access to internal functionality
**Game Development**: Managing complex state machines and component systems
**Enterprise Applications**: Building secure, well-organized domain models

## 🔗 Integration with Modern C#

**Record Types (C# 9+)**:
```csharp
public record OuterRecord
{
    public record NestedRecord(string Name, int Value);
    
    public NestedRecord CreateNested(string name, int value) => 
        new NestedRecord(name, value);
}
```

**Pattern Matching (C# 8+)**:
```csharp
public void ProcessNestedType(object obj)
{
    var result = obj switch
    {
        OuterClass.NestedClass nested => ProcessNested(nested),
        OuterClass.Status status => ProcessStatus(status),
        _ => "Unknown type"
    };
}
```

---

*Nested types are the organizational powerhouse of C#. Use them to create clean, secure, and maintainable code that groups related functionality while maintaining strict access control!* 🪆✨
