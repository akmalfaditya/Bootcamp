# How the Garbage Collector Works in C#

## Overview

This project gives you hands-on experience with the .NET Garbage Collector. 

## What You'll Learn

This comprehensive training covers the complete spectrum of .NET garbage collection concepts:

### Core Garbage Collection Theory

**Automatic Memory Management**: The .NET garbage collector employs a tracing system that automatically manages memory allocation and deallocation. Unlike manual memory management systems, the GC periodically examines all objects in memory to determine which are still reachable from application code and which can be safely reclaimed.

**Root Object Analysis**: The garbage collector begins its work by identifying root objects, which include local variables on thread stacks, static class members, CPU registers, and other memory locations that serve as entry points into the object graph. From these roots, the GC traces all object references to build a complete map of reachable objects.

**Mark-and-Compact Algorithm**: The standard CLR garbage collector uses a mark-and-compact approach consisting of three distinct phases: marking reachable objects, segregating live objects from garbage, and compacting memory to eliminate fragmentation.

### Fundamental Concepts You Will Master

1. **GC Process Mechanics** - The three-phase collection process and how object reachability is determined
2. **Generational Collection Theory** - The generational hypothesis and its performance implications
3. **Memory Heap Organization** - How the managed heap is structured and optimized
4. **Large Object Management** - Special handling for objects that exceed normal size thresholds
5. **Collection Triggering** - When and why garbage collections occur
6. **Unmanaged Resource Coordination** - Integrating managed and unmanaged memory systems
7. **Performance Optimization** - Techniques for minimizing GC overhead
8. **Latency Management** - Balancing collection frequency with application responsiveness
9. **Concurrent Collection** - How modern GC systems minimize application pauses
10. **Advanced Memory Patterns** - Specialized techniques for complex scenarios

## Project Structure

```
How the GC Works/
├── Program.cs              # Main demonstrations (8 core scenarios)
├── GCAdvancedExamples.cs   # Advanced GC patterns and edge cases
├── How the GC Works.csproj # Project configuration
└── README.md               # This training guide
```

## Core Demonstrations

### Demo 1: Fundamental GC Process
**Theoretical Foundation**: This demonstration illustrates the core three-phase garbage collection process that forms the foundation of .NET memory management.

**Marking Phase**: The garbage collector begins by identifying all root references in the application. These roots include local variables on thread stacks, static fields, CPU registers, and other memory locations that serve as entry points into the managed heap. From these roots, the GC traverses the entire object graph, following each reference to mark every reachable object as "live."

**Segregation Phase**: Once marking is complete, the GC categorizes all objects into two groups: those that were marked as reachable (live objects) and those that were not marked (garbage objects). Objects with finalizers that have not yet executed are placed in a special finalization queue and temporarily kept alive until their cleanup methods can be run.

**Compaction Phase**: The final phase involves physically moving all live objects to the beginning of the heap, eliminating gaps left by collected objects. This compaction serves two critical purposes: it prevents memory fragmentation that could lead to allocation failures, and it enables extremely fast allocation of new objects by simply incrementing a pointer to the end of the used memory region.

**Practical Insights**: Objects do not disappear immediately when they become unreachable. The collection process is triggered by allocation pressure or explicit requests, and the timing of collections is optimized for overall application performance rather than immediate cleanup.

### Demo 2: Generational Collection System
**Theoretical Foundation**: The generational collection system is based on the empirically-verified generational hypothesis, which states that most objects have very short lifetimes, while objects that survive longer tend to have much longer lifetimes.

**Generation 0 Characteristics**: Generation 0 contains newly allocated objects and is kept relatively small (typically hundreds of kilobytes to a few megabytes). Collections of Generation 0 occur frequently but complete very quickly, often in less than one millisecond. The vast majority of short-lived objects are collected at this level, making Gen0 collections highly efficient.

**Generation 1 Purpose**: Generation 1 serves as an intermediate buffer between the frequently collected Generation 0 and the expensive Generation 2. Objects that survive a Gen0 collection are promoted to Gen1, where they undergo further evaluation. Gen1 collections are less frequent than Gen0 but still relatively fast.

**Generation 2 Behavior**: Generation 2 contains long-lived objects that have survived multiple collection cycles. Collections of Generation 2 are "full collections" that examine the entire managed heap and are consequently the most time-consuming. These collections occur infrequently but can take tens of milliseconds or more to complete.

**Performance Implications**: This generational approach dramatically improves performance by concentrating collection efforts on the areas of memory where the highest proportion of garbage is found. Applications that create many short-lived objects benefit significantly from this optimization.

### Demo 3: Large Object Heap Management
**Theoretical Foundation**: Objects that exceed a specific size threshold (currently 85,000 bytes) are allocated on a separate heap called the Large Object Heap (LOH) to avoid the prohibitive cost of moving large blocks of memory during compaction.

**Allocation Behavior**: Unlike the normal managed heap where allocation simply involves incrementing a pointer, LOH allocation requires searching through a linked list of free memory blocks to find a suitable location. This makes LOH allocation inherently slower than normal heap allocation.

**Compaction Considerations**: By default, the LOH is not compacted during garbage collection because moving large objects is expensive. This design choice prevents collection pauses that could significantly impact application responsiveness but creates the possibility of memory fragmentation.

**Fragmentation Impact**: LOH fragmentation occurs when large objects are freed, creating gaps that can only be filled by objects of similar or smaller size. In long-running applications, this fragmentation can lead to significant memory waste and even allocation failures despite available memory.

**Mitigation Strategies**: The demonstration shows how to force LOH compaction when necessary and introduces array pooling as a strategy to reduce LOH pressure by reusing large objects rather than repeatedly allocating and collecting them.

### Demo 4: Manual Collection Control
**Theoretical Foundation**: While the garbage collector is designed to operate autonomously, there are specific scenarios where manual intervention may be beneficial or necessary.

**Automatic Collection Triggers**: Under normal circumstances, garbage collection is triggered by allocation pressure when the system determines that collection would be beneficial. The GC system includes sophisticated algorithms to determine optimal collection timing based on allocation patterns and memory pressure.

**Manual Collection Scenarios**: Forced collection is appropriate in limited scenarios such as dormant applications that will not allocate memory for extended periods, testing scenarios where deterministic cleanup behavior is required, or specific performance optimization cases where the application has superior knowledge of memory usage patterns.

**Proper Collection Sequence**: When manual collection is necessary, the proper sequence involves calling GC.Collect(), waiting for pending finalizers to complete with GC.WaitForPendingFinalizers(), and potentially calling GC.Collect() again to handle objects that were finalized in the first cycle.

**Performance Warnings**: Manual collection typically degrades performance by disrupting the GC's self-tuning algorithms and potentially promoting objects to higher generations prematurely. Production applications should rely on automatic collection except in carefully considered scenarios.

### Demo 5: Memory Pressure Coordination
**Theoretical Foundation**: The garbage collector makes collection decisions based on managed heap pressure, but it has no visibility into unmanaged memory allocations that may be consuming system resources.

**Unmanaged Memory Challenge**: Applications that allocate significant unmanaged memory through mechanisms such as Platform Invoke (P/Invoke), COM interop, or direct memory allocation create a disconnect between actual memory usage and the GC's perception of memory pressure.

**Pressure Notification System**: The GC.AddMemoryPressure() and GC.RemoveMemoryPressure() methods allow applications to inform the garbage collector about unmanaged memory allocations and deallocations, enabling more informed collection decisions.

**Coordination Benefits**: Proper memory pressure notification helps the GC trigger collections at appropriate times to maintain overall system memory health, preventing scenarios where managed memory appears abundant while the system is actually under memory pressure.

**Implementation Guidelines**: Memory pressure notifications should be paired carefully, with each AddMemoryPressure() call matched by a corresponding RemoveMemoryPressure() call when the unmanaged memory is freed.

### Demo 6: Array Pooling Optimization
**Theoretical Foundation**: Array pooling addresses the performance overhead associated with frequent allocation and collection of large arrays, particularly those that end up on the Large Object Heap.

**Allocation Overhead**: Repeatedly allocating and discarding large arrays creates significant garbage collection pressure and can lead to LOH fragmentation. The overhead includes both the allocation cost and the collection cost when arrays become garbage.

**Pooling Mechanism**: ArrayPool<T> provides a centralized system for renting and returning arrays, allowing applications to reuse existing arrays rather than allocating new ones. The pool manager optimizes array sizes and maintains multiple arrays of different sizes to satisfy various allocation requests.

**Performance Benefits**: Array pooling can provide substantial performance improvements, often reducing allocation overhead by 50% or more in array-intensive applications. The benefits are most pronounced for large arrays and scenarios with high allocation frequencies.

**Usage Considerations**: Proper array pooling requires careful attention to array lifecycle management. Arrays must be returned to the pool promptly, and applications must not use arrays after returning them to avoid data corruption or security issues.

### Demo 7: Latency Mode Configuration
**Theoretical Foundation**: Different applications have varying requirements for the balance between garbage collection latency (responsiveness) and throughput (overall performance).

**Latency Mode Options**: The GC provides several latency modes that optimize for different scenarios. Interactive mode balances responsiveness and throughput for typical applications. LowLatency mode prioritizes minimal pause times for real-time applications. Batch mode maximizes throughput at the expense of responsiveness for background processing scenarios.

**Trade-off Analysis**: Lower latency modes typically achieve reduced pause times by performing more frequent, smaller collections. This approach maintains application responsiveness but may reduce overall throughput due to increased collection overhead.

**Application Matching**: The choice of latency mode should align with application requirements. Games and real-time applications benefit from low latency modes, while batch processing systems can optimize for throughput using batch mode.

**Dynamic Adjustment**: Latency modes can be changed at runtime, allowing applications to adjust their GC behavior based on current operational requirements or user interaction patterns.

### Demo 8: Background Collection System
**Theoretical Foundation**: Background collection represents a significant advancement in garbage collection technology, allowing Generation 2 collections to proceed concurrently with application execution.

**Concurrent Collection Mechanics**: Background collection enables the garbage collector to perform expensive Generation 2 collections on a separate thread while the application continues to execute. This concurrency dramatically reduces the perception of collection pauses from the application's perspective.

**Pause Reduction**: While Generation 0 and Generation 1 collections are typically fast enough not to require background processing, Generation 2 collections can take significant time. Background collection allows these expensive operations to proceed without blocking application threads.

**Resource Trade-offs**: Background collection consumes additional CPU and memory resources to maintain the concurrent collection infrastructure. However, the improvement in application responsiveness typically justifies this overhead in interactive applications.

**Coordination Challenges**: The background collector must coordinate with ongoing application activity, including new allocations and object modifications that occur during collection. Sophisticated algorithms ensure collection correctness while maintaining concurrency benefits.

## Advanced Examples

### Server versus Workstation Garbage Collection
**Architectural Differences**: The .NET runtime provides two fundamentally different garbage collection modes optimized for different deployment scenarios. Workstation GC uses a single managed heap with a single garbage collection thread, optimized for client-side applications where user responsiveness is paramount. Server GC creates multiple managed heaps (typically one per CPU core) with dedicated garbage collection threads, optimized for server applications that prioritize throughput over individual operation latency.

**Performance Characteristics**: Server GC can significantly improve throughput in multi-threaded applications by parallelizing both allocation and collection across multiple heaps. However, this comes at the cost of higher memory usage and CPU overhead. Workstation GC provides lower memory overhead and better responsiveness for single-threaded or lightly-threaded applications.

**Selection Criteria**: The choice between Server and Workstation GC should be based on application characteristics and deployment environment. Multi-core server applications with high allocation rates benefit from Server GC, while desktop applications and services that prioritize responsiveness should use Workstation GC.

### Weak Reference Implementation
**Reference Management Theory**: Weak references provide a mechanism to maintain a reference to an object without preventing the garbage collector from collecting that object when it becomes otherwise unreachable. This capability is essential for implementing caches, event handlers, and other scenarios where strong references would create memory leaks.

**Cache Implementation**: Weak references are particularly valuable in caching scenarios where the cache should not prevent objects from being collected when memory pressure increases. The cache can maintain weak references to expensive-to-create objects while allowing the GC to reclaim them when necessary.

**Event Handler Patterns**: The weak event pattern uses weak references to prevent event subscriptions from creating strong references that would keep event sources alive indefinitely. This pattern is crucial for preventing memory leaks in publish-subscribe scenarios.

**Resurrection Handling**: Objects referenced by weak references can be "resurrected" during finalization, creating complex scenarios that require careful handling. The demonstration shows how to properly work with weak references while avoiding these pitfalls.

### Pinned Memory Management
**Memory Pinning Theory**: Pinned memory represents a special category of managed memory that is prevented from being moved during garbage collection compaction. This capability is essential for interoperability with unmanaged code that requires stable memory addresses.

**Interop Requirements**: Platform Invoke (P/Invoke) operations and other interop scenarios often require passing memory addresses to unmanaged code. Since the garbage collector normally moves objects during compaction, these scenarios require pinning to ensure address stability.

**Fragmentation Consequences**: Pinned objects create holes in the managed heap that cannot be compacted, leading to memory fragmentation. Excessive use of pinned memory can significantly degrade allocation performance and waste memory.

**Best Practices**: Pinned memory should be used sparingly and for the shortest possible duration. The demonstration shows proper techniques for minimizing fragmentation while satisfying interop requirements.

### Generation Promotion Mechanics
**Promotion Algorithm**: Objects are promoted between generations based on their survival through garbage collection cycles. Objects that survive a Generation 0 collection are promoted to Generation 1, and objects that survive a Generation 1 collection are promoted to Generation 2.

**Lifetime Optimization**: Understanding generation promotion enables developers to optimize object lifetime patterns. Objects should be designed to either have very short lifetimes (collected in Generation 0) or very long lifetimes (remain in Generation 2 throughout application execution).

**Performance Impact**: Objects with intermediate lifetimes that get promoted to Generation 1 or Generation 2 but are then collected create suboptimal performance patterns. The demonstration shows how to measure and optimize these patterns.

**Collection Frequency**: Different generations are collected at different frequencies, with Generation 0 collections occurring most frequently and Generation 2 collections occurring least frequently. This frequency difference makes generation assignment a critical performance factor.

### Heap Fragmentation Analysis
**Fragmentation Causes**: Memory fragmentation occurs when objects are allocated and deallocated in patterns that create unusable gaps in the heap. While compaction typically prevents fragmentation in the normal managed heap, the Large Object Heap and pinned objects can create fragmentation scenarios.

**Compaction Benefits**: The compaction process eliminates fragmentation by moving all live objects to the beginning of the heap, creating a contiguous block of free memory. This process ensures that allocation can proceed efficiently using simple pointer arithmetic.

**Measurement Techniques**: The demonstration shows how to measure fragmentation and its impact on application performance. Fragmentation can be detected through memory usage patterns and allocation failure scenarios.

**Mitigation Strategies**: Fragmentation can be mitigated through careful object lifetime management, appropriate use of object pooling, and strategic use of LOH compaction when necessary.

## Recent Enhancements

### Memory Pressure and Finalization Management
**Memory Pressure Coordination**: The MemoryPressureDemo demonstrates the critical coordination between managed and unmanaged memory systems. When applications allocate significant unmanaged resources through COM interop, Platform Invoke, or direct memory allocation, the garbage collector lacks visibility into this memory usage. The memory pressure notification system allows applications to inform the GC about unmanaged allocations, enabling more accurate collection timing decisions.

**Finalization Queue Processing**: The FinalizationQueueDemo illustrates the two-phase collection process required for objects with finalizers. During the first garbage collection cycle, objects with finalizers are moved to a special finalization queue rather than being immediately collected. The finalizer thread then executes the cleanup code, after which a subsequent collection cycle can actually reclaim the object's memory. This process ensures that cleanup operations complete before memory is reclaimed but adds overhead to the collection process.

**Object Resurrection Mechanics**: The ObjectResurrectionDemo explores the advanced technique where finalizers can make objects reachable again during the finalization process. While this capability exists for completeness, it creates complex lifecycle scenarios and is generally discouraged in production applications due to its potential for creating confusing object lifetime behavior.

### Advanced Garbage Collection Control Features
**GC Notification System**: The GCNotificationDemo presents enterprise-level garbage collection coordination for server farm scenarios. This system allows applications to receive advance notification before expensive Generation 2 collections occur, enabling sophisticated load balancing strategies where servers can temporarily redirect traffic to other instances during collection cycles.

**No-GC Region Implementation**: The NoGCRegionDemo demonstrates the ability to temporarily suspend garbage collection for ultra-low latency scenarios. Applications can request that the GC avoid collections within specific code regions, guaranteeing that critical operations complete without interruption. This feature is valuable for real-time applications such as high-frequency trading systems or audio processing where collection pauses are unacceptable.

### Configuration and Optimization Options
**Project Configuration Integration**: The enhanced project file includes comprehensive garbage collection configuration options that demonstrate the relationship between compile-time settings and runtime behavior. These settings allow experimentation with different GC modes without requiring code changes.

**Runtime Configuration Examples**: The project provides examples of runtime configuration through JSON files, showing how containerized applications can optimize garbage collection behavior for their specific deployment environments and resource constraints.

### Essential Learning Points

#### The Three-Phase Garbage Collection Process
**Phase 1 - Marking**: The garbage collector begins each collection cycle by identifying all root objects that serve as entry points into the managed heap. These roots include local variables on thread stacks, static class members, CPU registers, and other locations that directly reference managed objects. From these roots, the GC performs a graph traversal, following each object reference to mark every reachable object as live. This marking phase is crucial because it determines which objects remain accessible to the application and which have become unreachable garbage.

**Phase 2 - Segregation**: After marking is complete, the garbage collector segregates objects into categories based on their reachability and finalization requirements. Objects that were not marked during the traversal are identified as garbage and become candidates for collection. However, objects with finalizers that have not yet executed are placed in a special finalization queue and temporarily kept alive until their cleanup methods can run. This segregation phase ensures that resource cleanup occurs before memory reclamation.

**Phase 3 - Compaction**: The final phase involves physically relocating all live objects to create a contiguous block of memory at the beginning of the heap. This compaction process serves two essential purposes: it eliminates fragmentation that could prevent future allocations despite available memory, and it enables extremely efficient allocation of new objects by simply incrementing a pointer to the end of the used memory region. The compaction phase is what distinguishes the .NET GC from simple mark-and-sweep collectors.

#### Generational Hypothesis and Its Implementation
**Empirical Foundation**: The generational garbage collection system is based on extensive empirical research showing that the vast majority of objects in typical applications have very short lifetimes, while objects that survive longer tend to have much longer lifetimes. This observation, known as the generational hypothesis, enables significant performance optimizations by focusing collection efforts on areas where the highest concentration of garbage is found.

**Generation 0 Efficiency**: Generation 0 collections are extremely efficient because they operate on a small memory region containing mostly garbage. These collections typically complete in microseconds and reclaim the majority of allocated memory. The high garbage-to-live object ratio in Generation 0 means that collection effort yields maximum benefit with minimal overhead.

**Generation Promotion Strategy**: Objects that survive Generation 0 collections are promoted to Generation 1, indicating that they may have longer lifetimes. Generation 1 serves as a buffer that prevents premature promotion to Generation 2 while still allowing efficient collection of medium-lived objects. Objects that survive Generation 1 collections are promoted to Generation 2, where they are presumed to be long-lived and are collected infrequently.

**Performance Optimization**: This generational approach enables the GC to focus collection efforts where they provide the greatest benefit while avoiding expensive operations on objects that are likely to remain live for extended periods.

#### Large Object Heap Special Considerations
**Size Threshold Rationale**: Objects that exceed 85,000 bytes are allocated on the Large Object Heap because moving such large blocks of memory during compaction would be prohibitively expensive. The LOH threshold represents a balance between the benefits of compaction and the costs of moving large objects.

**Allocation Performance Impact**: LOH allocation is inherently slower than normal heap allocation because the GC must search through a free list to find suitable memory blocks rather than simply incrementing an allocation pointer. This search process becomes more expensive as the LOH becomes fragmented.

**Fragmentation Challenges**: The lack of compaction in the LOH means that freed objects create gaps that can only be filled by objects of similar or smaller size. In long-running applications, this fragmentation can lead to significant memory waste and potentially cause allocation failures despite available memory.

**Mitigation Approaches**: LOH fragmentation can be addressed through careful object lifetime management, strategic use of object pooling, and occasional forced compaction when application knowledge indicates it would be beneficial.

#### Appropriate Manual Garbage Collection Scenarios
**Autonomous Operation Principle**: The garbage collector is designed to operate autonomously with sophisticated algorithms that optimize collection timing based on allocation patterns, memory pressure, and application behavior. Manual intervention typically disrupts these optimizations and should be avoided in normal operation.

**Legitimate Manual Collection Cases**: Manual garbage collection is appropriate in specific scenarios such as dormant applications that will not allocate memory for extended periods, testing environments where deterministic cleanup behavior is required, or specialized performance optimization cases where the application has superior knowledge of memory usage patterns.

**Proper Manual Collection Sequence**: When manual collection is necessary, the correct sequence involves calling GC.Collect() to initiate collection, calling GC.WaitForPendingFinalizers() to ensure finalization completes, and potentially calling GC.Collect() again to handle objects that were finalized during the first cycle.

**Production Environment Considerations**: Manual garbage collection should never be used in production hot paths because it can disrupt the GC's self-tuning algorithms, promote objects to higher generations prematurely, and create unpredictable performance characteristics that are difficult to diagnose and optimize.

## Execution and Learning Guidelines

### Project Execution Instructions

**Building the Project**: Execute `dotnet build` to compile all project components and verify that all dependencies are properly resolved. The build process will validate the code and create executable artifacts in the output directory.

**Running Comprehensive Demonstrations**: Execute `dotnet run` to launch the complete training sequence. The program will execute all demonstrations in a logical progression, providing detailed output with real-time memory measurements and performance statistics.

**Observation Guidelines**: Pay careful attention to the program output, which includes precise memory measurements before and after each operation, garbage collection counts by generation, and timing information that reveals the performance characteristics of different GC operations.

### Critical Observation Points

**Memory Usage Patterns**: Monitor the memory usage figures reported before and after each demonstration. These measurements reveal how different allocation patterns affect heap utilization and collection efficiency.

**Collection Count Analysis**: Observe how the collection counts for different generations change throughout the demonstrations. The frequency of collections at each generation level provides insight into the efficiency of the generational collection strategy.

**Timing Variations**: Note the dramatic timing differences between different types of operations. Generation 0 collections typically complete in microseconds, while Generation 2 collections may require tens of milliseconds, demonstrating the performance impact of collection scope.

**Performance Pattern Recognition**: Look for patterns that emerge across demonstrations, such as the relationship between object lifetime and collection efficiency, or the impact of allocation size on collection behavior.

### Professional Development Guidelines

**Best Practices for Production Systems**: 
The garbage collector operates most efficiently when allowed to manage memory autonomously. Production applications should avoid manual intervention except in carefully considered scenarios. Use disposable patterns (using statements) for resources that require deterministic cleanup. Implement object pooling for frequently allocated objects, particularly large arrays that would otherwise pressure the Large Object Heap. Develop an understanding of your application's allocation patterns to make informed architectural decisions.

**Performance Warning Indicators**: 
Monitor for signs of suboptimal garbage collection performance including frequent Generation 2 collections which may indicate Large Object Heap pressure or inappropriate object lifetime patterns. High allocation rates should be measured with profiling tools to identify hotspots. Manual GC.Collect() calls in production code typically indicate design problems that should be addressed architecturally. Pinned objects that remain pinned for extended periods can cause heap fragmentation and should be minimized.

**Quantitative Performance Expectations**: 
Understanding typical garbage collection performance characteristics helps in system design and optimization. Generation 0 collections typically complete in approximately one millisecond. Generation 1 collections generally require around ten milliseconds. Generation 2 collections can take one hundred milliseconds or more depending on heap size. Large Object Heap fragmentation can waste thirty percent or more of allocated memory in poorly designed systems.

### Real-World Application Context

**Production Debugging Capabilities**: 
The knowledge gained from this training enables effective debugging of memory-related issues in production applications. You will be able to optimize allocation patterns for better performance, choose appropriate garbage collection configurations for different application types, and write code that works efficiently with the garbage collection system rather than against it.

**Performance Monitoring and Profiling**: 
To observe these concepts in actual applications, use professional memory profilers such as dotMemory or PerfView to visualize garbage collection behavior in real-time. Monitor Generation 2 collection frequency in production environments as an indicator of system health. Watch for Large Object Heap growth patterns in long-running applications. Profile allocation rates during peak load conditions to identify optimization opportunities.

### Application-Specific Optimization Strategies

**Web Application Optimization**: 
Web applications benefit from Server GC configuration for improved throughput on multi-core systems. Implement object pooling for high-traffic endpoints that create many temporary objects. Monitor Generation 2 collection frequency under load as an early warning indicator of memory pressure or inefficient allocation patterns.

**Desktop Application Considerations**: 
Desktop applications should typically use Workstation GC for better user interface responsiveness. Consider Low Latency GC mode for applications with real-time features such as games or interactive graphics. Implement pooling for user interface-related objects that are frequently allocated and discarded.

**Background Service Optimization**: 
Background services and batch processing applications can benefit from Server GC for multi-threaded processing scenarios. Batch GC mode optimizes for throughput at the expense of responsiveness, which is appropriate for non-interactive workloads. Implement careful memory pressure management when coordinating with external systems or unmanaged resources.

