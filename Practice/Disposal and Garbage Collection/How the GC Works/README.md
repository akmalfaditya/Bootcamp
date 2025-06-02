# How the Garbage Collector Works in C# - Training Project

## Overview

This project gives you hands-on experience with the .NET Garbage Collector. I've built this to show you **exactly** how the GC behaves in real applications - not just theory, but actual memory allocations, collections, and performance impacts. You'll see the marking, sweeping, and compacting phases in action, understand generational collection, and learn to optimize your applications.

## What You'll Master

After running through this code, you'll understand:

1. **GC Fundamentals** - Marking, sweeping, compacting phases
2. **Generational Collection** - Why Gen0 is fast and Gen2 is expensive
3. **Large Object Heap (LOH)** - When objects bypass normal collection
4. **Forced Collection** - When and how to manually trigger GC
5. **Memory Pressure** - How to inform GC about unmanaged memory
6. **Array Pooling** - Reducing GC pressure with object reuse
7. **GC Latency Modes** - Balancing responsiveness vs throughput
8. **Background Collection** - How work continues during GC
9. **Advanced Patterns** - Weak references, pinned memory, fragmentation

## Project Structure

```
How the GC Works/
├── Program.cs              # Main demonstrations (8 core scenarios)
├── GCAdvancedExamples.cs   # Advanced GC patterns and edge cases
├── How the GC Works.csproj # Project configuration
└── README.md               # This training guide
```

## Core Demonstrations

### Demo 1: Basic GC Behavior
- Shows marking, sweeping, and compacting in action
- **Real measurement**: Memory before/after allocation and collection
- **Key insight**: Objects don't disappear immediately when unreachable

### Demo 2: Generational Collection
- Demonstrates Gen0, Gen1, Gen2 promotion
- Shows collection counts for each generation
- **Trainer tip**: Gen0 collections are microseconds, Gen2 can be 100ms+

### Demo 3: Large Object Heap (LOH)
- Objects ≥85KB go to special heap
- Demonstrates LOH compaction (normally doesn't compact)
- **Performance impact**: LOH fragmentation is a real problem in long-running apps

### Demo 4: Forced Collection
- When to use `GC.Collect()` (spoiler: almost never!)
- Proper pattern with `GC.WaitForPendingFinalizers()`
- **Warning**: Forced collection usually hurts performance

### Demo 5: Memory Pressure
- Using `GC.AddMemoryPressure()` for unmanaged memory
- Why the GC needs to know about native allocations
- **Real scenario**: COM objects, native handles, GPU memory

### Demo 6: Array Pooling
- `ArrayPool<T>` reduces allocation pressure
- **Measured benefit**: Often 50%+ performance improvement
- **Best practice**: Use pooling for frequently allocated arrays

### Demo 7: GC Latency Modes
- Interactive vs LowLatency vs Batch modes
- **Trade-offs**: Responsiveness vs throughput
- **When to use**: Games need LowLatency, batch processing uses Batch

### Demo 8: Background Collection
- How GC runs concurrently with your application
- **Key point**: Background GC reduces pause times significantly

## Advanced Examples

### Server vs Workstation GC
- Shows current GC configuration
- Demonstrates allocation performance differences
- **Decision point**: Server GC for multi-core, Workstation for client apps

### Weak References
- Objects you can reference but don't keep alive
- Perfect for caches and event handlers
- **Pattern**: Weak event pattern prevents memory leaks

### Pinned Memory
- `GCHandle.Alloc(Pinned)` prevents object movement
- **Danger**: Overuse causes heap fragmentation
- **Use case**: Interop with native code

### Generation Promotion
- Detailed tracking of objects through generations
- Shows how object lifetime affects GC performance
- **Optimization**: Keep objects either very short-lived or very long-lived

### Heap Fragmentation
- Demonstrates fragmentation and compaction
- Shows memory reclamation through compaction
- **Real impact**: Fragmentation can waste significant memory

## How to Run

1. **Build the project**:
   ```bash
   dotnet build
   ```

2. **Run the main demonstration**:
   ```bash
   dotnet run
   ```

3. **Watch the output carefully** - each demo shows real memory measurements

## What to Watch For

Pay attention to:
- **Timing differences** between operations
- **Memory usage** before and after operations
- **Collection counts** changing during demonstrations
- **Performance patterns** that emerge

## Trainer's Professional Tips

### ✅ Best Practices:
- Let the GC do its job - don't micromanage
- Use `using` statements for `IDisposable` objects
- Pool frequently allocated objects (especially arrays)
- Understand your application's allocation patterns

### ⚠️ Warning Signs:
- Frequent Gen2 collections (check LOH usage)
- High allocation rates (measure with profiler)
- Manual `GC.Collect()` calls in production code
- Pinned objects left unpinned

### 🎯 Performance Insights:
- Gen0 collection: ~1ms
- Gen1 collection: ~10ms  
- Gen2 collection: ~100ms
- LOH fragmentation can waste 30%+ memory

## Real-World Applications

This knowledge helps you:
- **Debug memory issues** in production applications
- **Optimize allocation patterns** for better performance
- **Choose the right GC configuration** for your application type
- **Write GC-friendly code** that performs well under load

## Memory Profiling

To see this in action with real applications:
- Use **dotMemory** or **PerfView** to visualize GC behavior
- Monitor **Gen2 collection frequency** in production
- Watch for **LOH growth** in long-running applications
- Profile **allocation rates** during peak load

## Common Scenarios

### Web Applications:
- Use Server GC for better throughput
- Pool objects for high-traffic endpoints
- Monitor Gen2 collections under load

### Desktop Applications:
- Use Workstation GC for better responsiveness
- Consider LowLatency mode for real-time features
- Pool UI-related objects that allocate frequently

### Background Services:
- Server GC for multi-threaded processing
- Batch mode for throughput-oriented work
- Careful memory pressure management

## Next Steps

After mastering this material:
1. **Profile a real application** to see GC patterns
2. **Implement array pooling** in performance-critical code
3. **Measure GC impact** in your applications
4. **Learn memory debugging** with diagnostic tools

---

**Remember**: The GC is incredibly sophisticated and self-tuning. Your job isn't to outsmart it, but to understand it well enough to write code that works **with** it, not against it. Most performance problems come from fighting the GC rather than working with its design!
