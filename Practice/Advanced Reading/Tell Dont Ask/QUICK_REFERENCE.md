# Tell, Don't Ask - Quick Reference Guide

## 🎯 The Core Principle

**Tell objects what to do, don't ask them for data to make decisions yourself.**

## ❌ Bad Examples (Ask Approach)

### Problem Pattern 1: Exposing Internal State
```csharp
// BAD: Object is just a data container
public class BankAccount 
{
    public decimal Balance { get; set; }
}

// External code has to know business rules
if (account.Balance >= withdrawAmount) 
{
    account.Balance -= withdrawAmount;
}
```

### Problem Pattern 2: Scattered Business Logic
```csharp
// BAD: Business logic scattered across the codebase
public class TemperatureMonitor 
{
    public int Temperature { get; set; }
    public int Limit { get; set; }
}

// Every place that uses the monitor must know the alarm logic
if (monitor.Temperature > monitor.Limit) 
{
    alarm.Trigger("Temperature too high!");
}
```

### Problem Pattern 3: Exposing Collections
```csharp
// BAD: Exposing internal collections
public class ShoppingCart 
{
    public List<CartItem> Items { get; set; }
}

// External code manipulates cart internals
cart.Items.Add(new CartItem(product, quantity));
decimal total = cart.Items.Sum(item => item.Price * item.Quantity);
```

## ✅ Good Examples (Tell Approach)

### Solution 1: Encapsulate Behavior
```csharp
// GOOD: Object handles its own business rules
public class BankAccount 
{
    private decimal _balance;
    
    public bool Withdraw(decimal amount) 
    {
        if (_balance >= amount) 
        {
            _balance -= amount;
            return true;
        }
        return false;
    }
}

// Simple, clean usage
bool success = account.Withdraw(withdrawAmount);
```

### Solution 2: Internal Logic Management
```csharp
// GOOD: Monitor handles its own alarm logic
public class TemperatureMonitor 
{
    private int _temperature;
    private readonly int _limit;
    private readonly Alarm _alarm;
    
    public void UpdateTemperature(int newTemp) 
    {
        _temperature = newTemp;
        if (_temperature > _limit) 
        {
            _alarm.Trigger("Temperature too high!");
        }
    }
}

// Clean usage - just tell it the new temperature
monitor.UpdateTemperature(35);
```

### Solution 3: Controlled Operations
```csharp
// GOOD: Cart provides specific operations
public class ShoppingCart 
{
    private readonly List<CartItem> _items = new();
    
    public void AddItem(Product product, int quantity) 
    {
        // Cart handles the logic internally
        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing != null) 
        {
            existing.Quantity += quantity;
        } 
        else 
        {
            _items.Add(new CartItem(product, quantity));
        }
    }
    
    public decimal GetTotal() 
    {
        return _items.Sum(item => item.Price * item.Quantity);
    }
}

// Clean usage
cart.AddItem(product, 2);
decimal total = cart.GetTotal();
```

## 🔍 Quick Identification Guide

### 🚨 Red Flags (Indicates Ask Approach)
- Methods that only get/set data without behavior
- Business logic scattered across multiple classes
- External code checking object state before acting
- Public setters on important business properties
- Methods that return collections for external manipulation
- If-statements based on object state in calling code

### ✅ Green Flags (Indicates Tell Approach)
- Methods that perform complete operations
- Business logic encapsulated within relevant objects
- Objects that protect their own invariants
- Minimal public getters, focused on essential information
- Methods that return results/status rather than raw data
- Objects that "do things" rather than just "hold things"

## 🛠️ Refactoring Checklist

When you see Ask patterns, ask yourself:

1. **Who should make this decision?** 
   - Move the decision logic to the object that owns the data

2. **What is this code really trying to do?** 
   - Create a method that captures the intent

3. **Is this logic repeated elsewhere?** 
   - Centralize it in the appropriate object

4. **Could this object protect itself better?** 
   - Add validation and business rules inside the object

5. **What would happen if the internal structure changed?** 
   - Design the interface to be stable regardless of internals

## 🎯 Benefits Summary

| Aspect | Ask Approach | Tell Approach |
|--------|-------------|---------------|
| **Coupling** | High - callers know internals | Low - callers use clean interface |
| **Maintainability** | Hard - logic scattered | Easy - logic centralized |
| **Testing** | Complex - must test interactions | Simple - test object behavior |
| **Reusability** | Low - context-dependent | High - self-contained |
| **Bug Risk** | High - easy to misuse | Low - object protects itself |

## 💡 Remember

- **Behavior belongs with data** - put methods where the data is
- **Objects should be active, not passive** - they should do things, not just hold things
- **Encapsulation is about behavior, not just data hiding** - hide the decisions, not just the variables
- **Think in terms of responsibilities** - what should this object be responsible for?

## 🚀 Next Steps

1. Review your current code for Ask patterns
2. Identify objects that are just data containers
3. Move business logic into the objects that own the data
4. Create meaningful methods that capture intent
5. Test that objects can protect their own invariants
6. Enjoy more maintainable, robust code!
