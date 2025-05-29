# C# Access Modifiers - Controlling Code Visibility

## 🎯 Learning Objectives

Master C# access modifiers to control code visibility and build well-encapsulated, maintainable object-oriented applications with proper information hiding.

**What you'll master:**
- Understanding all seven access modifiers in C#
- Choosing the right access level for different scenarios
- Implementing proper encapsulation and information hiding
- Using access modifiers for API design and security
- Understanding assembly boundaries and cross-project visibility
- Modern access modifiers (file-scoped, private protected)
- Best practices for access control in real-world applications

## 📚 Core Concepts Covered

### 🔐 Access Modifier Hierarchy
- **Public**: Accessible from anywhere - the most permissive
- **Internal**: Accessible within the same assembly only
- **Protected**: Accessible to the class and its derived classes
- **Private**: Accessible only within the same class - the most restrictive
- **Protected Internal**: Accessible within assembly OR to derived classes
- **Private Protected**: Accessible to derived classes within same assembly
- **File** (C# 11+): Accessible only within the same source file

### 🏗️ Encapsulation Principles
- **Information Hiding**: Controlling what internal details are exposed
- **API Surface**: Defining the public interface of your classes
- **Implementation Details**: Keeping internal workings private
- **Inheritance Access**: Controlling what derived classes can access

### 🎯 Strategic Access Design
- **Principle of Least Privilege**: Giving minimum necessary access
- **API Evolution**: Designing for future changes and extensibility
- **Security Boundaries**: Using access modifiers for security design
- **Assembly Boundaries**: Understanding cross-project visibility rules

## 🚀 Key Features with Examples

### Public Access - The Open Door
```csharp
public class PublicDemo
{
    public string PublicField = "Accessible everywhere";
    public int PublicProperty { get; set; }
    
    public void PublicMethod()
    {
        Console.WriteLine("Public method called from anywhere!");
    }
}

// Usage from anywhere
var demo = new PublicDemo();
demo.PublicField = "Modified from external assembly";
demo.PublicMethod(); // Always accessible
```

### Internal Access - Assembly Boundaries
```csharp
internal class InternalDemo
{
    internal string InternalField = "Same assembly only";
    internal int InternalProperty { get; set; }
    
    internal void InternalMethod()
    {
        Console.WriteLine("Only visible within this assembly");
    }
}

// Perfect for implementation details that shouldn't leak
internal static class InternalUtilities
{
    internal static void ProcessData() { /* Helper method */ }
}
```

### Protected Access - Family Secrets
```csharp
public class BaseClass
{
    protected string familySecret = "Only family knows this";
    protected virtual void FamilyMethod()
    {
        Console.WriteLine("Inherited by derived classes");
    }
}

public class DerivedClass : BaseClass
{
    public void AccessFamilyMembers()
    {
        familySecret = "Derived class can modify this";
        FamilyMethod(); // Can call protected methods
    }
}
```

### Private Protected - Modern Precision
```csharp
public class ModernBase
{
    private protected string restrictedData = "Very limited access";
    
    private protected void RestrictedMethod()
    {
        // Only derived classes within same assembly
        Console.WriteLine("Ultra-restricted access");
    }
}

// In same assembly
public class SameAssemblyDerived : ModernBase
{
    public void CanAccess()
    {
        restrictedData = "Accessible here"; // ✅ Works
        RestrictedMethod(); // ✅ Works
    }
}
```

### File-Scoped Access (C# 11+)
```csharp
file class FileOnlyUtility
{
    private static string fileSecret = "Only in this file";
    
    public static void ProcessFileData()
    {
        Console.WriteLine("File-scoped processing");
    }
}

// This utility class is invisible to other files
// Perfect for source-generated code and file-specific helpers
```

## 💡 Trainer Tips

### Access Modifier Selection Strategy
- **Start with Private**: Begin with most restrictive, then open up as needed
- **Public API Design**: Carefully consider what becomes public - it's hard to take back
- **Internal for Infrastructure**: Use internal for classes that support your public API
- **Protected for Extensibility**: Use protected when you want inheritance but not general access

### Common Anti-Patterns
- **Public Everything**: Making everything public defeats encapsulation
- **Friend Assemblies Abuse**: Overusing InternalsVisibleTo breaks assembly boundaries
- **Protected Instead of Private**: Don't use protected unless inheritance is intended
- **Access Modifier Soup**: Using too many different access levels in one class

### Performance Considerations
- **JIT Optimizations**: Private and internal methods can be inlined more aggressively
- **Assembly Loading**: Internal types don't need cross-assembly metadata
- **Security**: Access modifiers are not security boundaries - use proper security measures

## 🎓 Real-World Applications

### 🏦 Banking System Design
```csharp
public class BankAccount
{
    private decimal _balance; // Private - critical data protection
    public string AccountNumber { get; private set; } // Public get, private set
    protected decimal MinimumBalance { get; set; } = 0; // For derived account types
    
    public decimal GetBalance() // Public API
    {
        return _balance;
    }
    
    internal void SystemDeposit(decimal amount) // Bank system only
    {
        _balance += amount;
    }
    
    private bool ValidateTransaction(decimal amount) // Internal validation
    {
        return amount > 0 && _balance >= amount;
    }
}

public class SavingsAccount : BankAccount
{
    protected override decimal MinimumBalance { get; set; } = 100; // Override minimum
    
    public bool CanWithdraw(decimal amount)
    {
        return GetBalance() - amount >= MinimumBalance; // Use protected member
    }
}
```

### 🎮 Game Engine Architecture
```csharp
public class GameObject
{
    public string Name { get; set; } // Public for editor access
    internal Vector3 Position { get; set; } // Internal for engine systems
    protected List<Component> Components { get; set; } // Protected for inheritance
    private bool _isDestroyed; // Private state
    
    public void Destroy() // Public API
    {
        _isDestroyed = true;
        OnDestroyed(); // Call protected virtual
    }
    
    protected virtual void OnDestroyed() // Override in derived classes
    {
        // Cleanup logic for derived classes
    }
    
    internal void SystemUpdate() // Only engine can call
    {
        if (!_isDestroyed)
            UpdateComponents();
    }
    
    private void UpdateComponents() // Implementation detail
    {
        foreach (var component in Components)
            component.Update();
    }
}
```

### 📚 Library Design Pattern
```csharp
// Public API surface
public class DataProcessor
{
    public Task<ProcessResult> ProcessAsync(DataInput input)
    {
        return InternalProcessAsync(input);
    }
    
    // Internal implementation
    internal async Task<ProcessResult> InternalProcessAsync(DataInput input)
    {
        var validated = await ValidateInput(input);
        return await ProcessValidatedData(validated);
    }
    
    // Private implementation details
    private async Task<ValidatedInput> ValidateInput(DataInput input)
    {
        // Complex validation logic
        return new ValidatedInput(input);
    }
    
    private async Task<ProcessResult> ProcessValidatedData(ValidatedInput input)
    {
        // Core processing logic
        return new ProcessResult();
    }
}

// Internal supporting classes - not exposed to library users
internal class ValidatedInput
{
    public ValidatedInput(DataInput input) { /* ... */ }
}

internal class ProcessResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
```

## 🎯 Mastery Checklist

### Beginner Level
- [ ] Understand public, private, and internal access modifiers
- [ ] Use appropriate access modifiers for fields and properties
- [ ] Implement basic encapsulation with private fields and public properties
- [ ] Understand assembly boundaries and internal visibility
- [ ] Use protected access for inheritance scenarios

### Intermediate Level
- [ ] Design clean public APIs with appropriate access levels
- [ ] Use protected internal and private protected effectively
- [ ] Implement proper encapsulation in complex class hierarchies
- [ ] Understand access modifier rules and restrictions
- [ ] Use file-scoped access modifiers (C# 11+)

### Advanced Level
- [ ] Design extensible class hierarchies with strategic access control
- [ ] Create internal framework components with proper encapsulation
- [ ] Balance API surface area with implementation flexibility
- [ ] Use access modifiers for security and maintainability
- [ ] Handle access control in generic types and constraints

### Expert Level
- [ ] Design comprehensive API surface areas for large libraries
- [ ] Create secure access patterns for sensitive operations
- [ ] Balance performance and encapsulation in high-performance scenarios
- [ ] Implement complex inheritance patterns with multiple access levels
- [ ] Use access modifiers strategically in domain-driven design

## 💼 Industry Impact

### Software Design Benefits
- **Maintainability**: Clear access boundaries make code easier to maintain and modify
- **Security**: Access control provides first line of defense against misuse
- **API Stability**: Well-designed access modifiers enable stable public APIs
- **Team Collaboration**: Clear access boundaries improve team development workflows

### Career Applications
- **Library Development**: Creating reusable components with clean APIs
- **Enterprise Applications**: Building secure, maintainable business systems
- **Framework Design**: Creating extensible frameworks with proper encapsulation
- **Open Source Projects**: Designing public APIs that evolve gracefully

## 🔗 Integration with Other Technologies

### Modern C# Features
- **Record Types**: Access modifiers work with records and positional parameters
- **Init-Only Properties**: Combining with access modifiers for immutable design
- **Pattern Matching**: Access-controlled properties in pattern expressions
- **Nullable Reference Types**: Access modifiers with nullable annotations

### Design Patterns
- **Factory Pattern**: Using internal constructors with public factory methods
- **Builder Pattern**: Progressive access revelation through method chaining
- **Template Method**: Protected virtual methods for customization points
- **Facade Pattern**: Public interface hiding internal complexity

---

**🎖️ Professional Insight**: Access modifiers are the foundation of good object-oriented design. They're not just about hiding code - they're about:

- **Communication**: Telling other developers (and your future self) what's intended for external use
- **Evolution**: Allowing your code to change internally without breaking external consumers
- **Security**: Preventing accidental misuse of internal implementation details
- **Performance**: Enabling compiler optimizations through restricted visibility

Master access modifiers to write code that's not just functional, but professional, maintainable, and secure!
