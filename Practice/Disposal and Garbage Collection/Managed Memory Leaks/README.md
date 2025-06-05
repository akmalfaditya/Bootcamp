# Managed Memory Leaks in C# 

## What This Project Teaches You

**Welcome to the dangerous world of managed memory leaks!** 

Yes, you heard right - even in the "safe" managed world of C#, you can still leak memory. The garbage collector is smart, but it's not magic. It can only clean up objects that have NO references pointing to them. When you accidentally keep references alive, you've just created a memory leak.

This project will teach you the **6 most common ways** developers accidentally leak memory in C# applications, and more importantly, how to avoid them.

## Why Should You Care?

Memory leaks in production applications are **expensive**:
- Applications slow down over time (memory pressure)
- Servers crash with OutOfMemoryExceptions
- Users complain about poor performance
- Your company loses money

The worst part? These leaks often don't show up during development - they only appear under real-world load after running for hours or days.

## Project Structure

```
├── Program.cs                           # Main demonstration runner
├── EventHandlerExamples.cs             # Event subscription leaks
├── TimerExamples.cs                     # Timer disposal issues
├── StaticAndWeakReferenceExamples.cs    # Static reference problems
└── ProperCleanupExamples.cs             # Best practices
```

## The 6 Memory Leak Scenarios

### 1. Event Handler Memory Leaks
**The Problem:** When you subscribe to events but forget to unsubscribe, the event publisher keeps your object alive.

**Real World Example:** A UI form subscribes to a service's events but doesn't unsubscribe when closing. The form stays in memory forever.

**What You'll Learn:**
- Why event subscriptions create strong references
- How to properly unsubscribe from events
- Using weak event patterns for automatic cleanup

### 2. Timer Memory Leaks
**The Problem:** Timers hold references to their callback methods, keeping entire objects alive.

**Real World Example:** A background service creates timer objects for periodic tasks but doesn't dispose them. Each timer keeps its parent object alive.

**What You'll Learn:**
- Difference between `System.Timers.Timer` and `System.Threading.Timer`
- Why timers need explicit disposal
- Proper timer lifecycle management

### 3. Threading Timer Behavior
**The Problem:** Threading timers behave differently and can keep objects alive in unexpected ways.

**Real World Example:** Using threading timers in object instances without understanding their lifetime behavior.

**What You'll Learn:**
- How threading timers differ from regular timers
- Reference behavior in multi-threaded scenarios
- Best practices for timer disposal in threading contexts

### 4. Static Reference Leaks
**The Problem:** Static collections grow forever because nothing tells them when objects should be removed.

**Real World Example:** A static cache that stores user session data but never removes expired sessions.

**What You'll Learn:**
- Why static references prevent garbage collection
- How to implement proper cache eviction
- When to use static collections safely

### 5. Weak References as Solutions
**The Problem:** Sometimes you need to reference objects without keeping them alive.

**Real World Example:** Observer patterns where you want to notify subscribers but allow them to be garbage collected.

**What You'll Learn:**
- What weak references are and when to use them
- Implementing weak event patterns
- Trade-offs between strong and weak references

### 6. Proper Cleanup Patterns
**The Problem:** Not following established disposal patterns leads to resource leaks.

**Real World Example:** Database connections, file handles, and network resources that aren't properly disposed.

**What You'll Learn:**
- Implementing IDisposable correctly
- Using `using` statements effectively
- Resource pooling for expensive objects

## How to Run the Demonstration

```bash
# Navigate to the project directory
cd "Managed Memory Leaks"

# Build the project
dotnet build

# Run the demonstration
dotnet run
```

The program will run each memory leak scenario and show you:
- **Memory before** creating objects
- **Memory after** creating objects
- **Memory after** attempting cleanup
- **Analysis** of what happened and why

## What You'll See in the Output

Each demonstration shows memory usage in bytes and explains:

1. **The Setup** - What objects are being created
2. **The Problem** - Why memory isn't being freed
3. **The Solution** - How to fix the leak
4. **The Proof** - Memory measurements showing the difference

## Key Learning Points

### Bad Patterns (Memory Leaks)
```csharp
// Subscribing without unsubscribing
publisher.SomeEvent += MyHandler;
// Object can't be garbage collected!

// Creating timers without disposal
var timer = new System.Timers.Timer(1000);
// Timer keeps callbacks alive forever!

// Growing static collections
static List<object> cache = new List<object>();
cache.Add(someObject); // Never removed!
```

### Good Patterns (Proper Cleanup)
```csharp
// Always unsubscribe
publisher.SomeEvent += MyHandler;
// Later...
publisher.SomeEvent -= MyHandler;

// Dispose timers properly
using var timer = new System.Timers.Timer(1000);
// Automatically disposed!

// Implement proper cache eviction
// Remove expired items regularly
// Use weak references when appropriate
```

## Tips

1. **Use Memory Profilers**: Tools like dotMemory or PerfView will show you exactly what's keeping objects alive.

2. **Watch Static Collections**: Any static `List<>`, `Dictionary<>`, or custom collection is a potential leak source.

3. **Events Are Dangerous**: The biggest source of memory leaks in C# applications. Always pair subscribe with unsubscribe.

4. **Timers Need Love**: Both `System.Timers.Timer` and `System.Threading.Timer` need explicit disposal.

5. **Weak References**: Use them for observer patterns and caches where you don't want to control object lifetime.

## Common Mistakes to Avoid

1. **Forgetting to unsubscribe from events** - especially in UI applications
2. **Not disposing timers** - they keep running even when you don't need them
3. **Growing static collections indefinitely** - implement size limits and eviction
4. **Assuming garbage collection will handle everything** - it only cleans up unreferenced objects
5. **Not understanding object lifetime** - know when objects should die



