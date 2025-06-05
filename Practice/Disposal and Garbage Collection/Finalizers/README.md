# Finalizers in C#

## Overview

This project provides a hands-on demonstration of finalizers (destructors) in C#.

## What You'll Learn

By running and studying this code, you'll understand:

1. **Basic Finalizer Syntax** - How to write them correctly
2. **Execution Timing** - Why finalizers are unpredictable
3. **Performance Impact** - Actual measurable costs (we'll show you 86ms vs 0ms!)
4. **Best Practices** - What professional code actually looks like
5. **Common Mistakes** - What NOT to do (and why)
6. **Advanced Patterns** - Dispose + Finalizer pattern done right
7. **Finalizer Order Issues** - Why dependencies can cause problems

## Project Structure

```
Finalizers/
├── Program.cs                      # Main demonstration with 7 scenarios
├── FinalizerExamples.cs           # Basic finalizer implementations
├── AdvancedFinalizerExamples.cs   # Advanced patterns and edge cases
├── Finalizers.csproj              # Project configuration
└── README.md                      # This file
```

## Key Demonstrations

### 1. Basic Finalizer Behavior (`BasicFinalizerExample`)
- Shows fundamental finalizer syntax
- Demonstrates when finalizers actually execute
- **Trainer Note**: Watch how finalizers don't run immediately when objects go out of scope!

### 2. Execution Timing (`TimedFinalizerExample`)
- Proves finalizers are unpredictable
- Shows multiple garbage collection cycles
- **Trainer Note**: This is why you NEVER rely on finalizers for critical cleanup!

### 3. Performance Impact Testing
- Compares 1000 objects with vs without finalizers
- Shows real timing differences (typically 80-100ms slower!)
- **Trainer Note**: Finalizers have a real cost - use them wisely!

### 4. Best Practices (`GoodFinalizerExample`)
- Demonstrates proper finalizer implementation
- Shows defensive programming techniques
- **Trainer Note**: Notice the try-catch and the simplicity!

### 5. Common Mistakes (`BadFinalizerExample`)
- Shows what NOT to do in finalizers
- Demonstrates dangerous patterns
- **Trainer Note**: These mistakes can crash your application!

### 6. Advanced Dispose Pattern (`AdvancedFinalizerExample`)
- Full IDisposable + Finalizer implementation
- Shows the pattern used in .NET Framework itself
- **Trainer Note**: This is the professional-grade pattern!

### 7. Finalizer Order Problems (`FinalizerOrderExample`)
- Demonstrates dependency issues between finalizers
- Shows why object relationships matter
- **Trainer Note**: Finalizer order is unpredictable - design accordingly!

## How to Run

1. **Open Terminal/Command Prompt**
2. **Navigate to the project folder**:
   ```bash
   cd "c:\Users\Formulatrix\Documents\Bootcamp\Practice\Disposal and Garbage Collection\Finalizers"
   ```
3. **Build the project**:
   ```bash
   dotnet build
   ```
4. **Run the demonstration**:
   ```bash
   dotnet run
   ```

## What to Watch For

When you run this program, pay attention to:

- **Timing differences** between finalizer vs non-finalizer objects
- **Unpredictable execution** of finalizers (they don't run immediately!)
- **Console output** showing when finalizers actually execute
- **Performance metrics** displayed for each scenario

## Trainer's Key Points

### Do This:
- Keep finalizers simple and fast
- Always use try-catch in finalizers
- Implement the full Dispose pattern when you need finalizers
- Test finalizer performance impact

### Don't Do This:
- Don't access other objects in finalizers (they might be finalized already!)
- Don't throw exceptions from finalizers
- Don't rely on finalizer execution timing
- Don't use finalizers unless you absolutely need them

### Tips:
In real applications, you'll rarely write finalizers yourself. Most come from:
- File handles (`FileStream`)
- Network connections (`Socket`)
- Database connections (`SqlConnection`)
- Unmanaged memory allocations

## Understanding the Output

When you run the program, you'll see output like:
```
=== Performance Impact Testing ===
Creating 1000 objects WITHOUT finalizers...
Time taken: 0 ms

Creating 1000 objects WITH finalizers...
Time taken: 86 ms
```

This shows the **real cost** of finalizers! They're not free.

## Real-World Applications

This knowledge helps you understand:
- Why `using` statements are so important
- How garbage collection performance is affected
- Why some .NET classes implement `IDisposable`
- When you might need to write your own finalizers (rare!)