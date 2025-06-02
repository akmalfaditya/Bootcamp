# Tell, Don't Ask Principle Demo

Hey there! Welcome to this hands-on demonstration of the "Tell, Don't Ask" principle in C#.

## What This Project Is About

This isn't just another theory lesson - it's a practical exploration of one of the most important principles in object-oriented programming. You'll see exactly why "Tell, Don't Ask" makes your code cleaner, more maintainable, and easier to understand.

## The Problem We're Solving

Ever written code where you constantly ask objects for their data, then make decisions based on that data somewhere else? That's the "Ask" approach, and it leads to:
- Scattered logic across your codebase
- Objects that are just data containers
- Code that's hard to maintain and extend
- Tight coupling between components

## Project Structure

```
├── BadExamples/           # The "ask" approach - what NOT to do
├── GoodExamples/          # The "tell" approach - what TO do
├── Models/                # Our domain objects and supporting classes
├── Scenarios/             # Real-world scenarios demonstrating the principle
└── Program.cs            # Interactive demonstration runner
```

## What You'll Learn

### Core Concept
Instead of asking an object for its data and making decisions externally, **tell** the object what you want it to do and let it handle its own logic internally.

### Bad (Ask) Approach:
```csharp
// This is problematic - we're asking for data and making decisions outside
if (monitor.GetValue() > monitor.GetLimit()) 
{
    monitor.GetAlarm().Warn(monitor.GetName() + " too high");
}
```

### Good (Tell) Approach:
```csharp
// This is better - we tell the object what to do, it handles the logic
monitor.SetValue(newValue); // Monitor decides internally if alarm should trigger
```

## Real-World Examples Included

1. **Monitor System** - The classic example from the material
2. **Bank Account** - Managing deposits, withdrawals, and overdraft protection
3. **Shopping Cart** - Handling item additions, discounts, and checkout logic
4. **Security System** - Access control and breach detection
5. **Temperature Controller** - HVAC system management

## How to Run

```bash
dotnet run
```

The program will walk you through each example, showing the "ask" approach first, then the improved "tell" approach. Pay attention to how the responsibilities shift from external controllers to the objects themselves.

## Key Benefits You'll See

- **Encapsulation** - Data and behavior stay together where they belong
- **Maintainability** - Changes to logic happen in one place
- **Readability** - Code reads more naturally and expresses intent clearly
- **Testability** - Objects can be tested in isolation more easily
- **Flexibility** - Easier to extend and modify object behavior

## When to Use This Principle

✅ **Good candidates:**
- Business logic that operates on an object's data
- State validation and rule enforcement
- Calculations based on internal properties
- Behavior that naturally belongs to the object

❌ **Not always applicable:**
- Simple data transfer objects (DTOs)
- When you genuinely need read-only access to data
- Integration with external systems that require specific data formats

## Pro Tips

1. **Start with behavior** - Ask yourself "what should this object be able to do?"
2. **Think in commands** - Use verbs for method names (deposit, withdraw, activate)
3. **Avoid getter chains** - If you're chaining getters, you might be violating the principle
4. **Favor composition** - Build complex behavior by combining simpler objects

## Remember

> "Objects should be responsible for their own behavior, not just hold data for others to manipulate"

This principle isn't about eliminating all getters - it's about moving behavior closer to the data it operates on. The goal is more cohesive, maintainable code.

Ready to see the difference? Run the demo and watch how "Tell, Don't Ask" transforms complex, scattered logic into clean, encapsulated behavior!

---
*Built with ❤️ for developers who want to write better object-oriented code*
