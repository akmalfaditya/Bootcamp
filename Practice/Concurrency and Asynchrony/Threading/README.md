# C# Threading

## Learning Objectives

Learn C# threading fundamentals and learn to create responsive, multi-threaded applications that can perform multiple operations simultaneously.

**What you'll master:**
- Creating and managing threads with the Thread class
- Understanding thread lifecycles and states
- Implementing thread safety and synchronization mechanisms
- Using thread signaling and communication techniques
- Managing shared resources with locks and synchronization primitives
- Optimizing thread performance and priorities
- Avoiding common threading pitfalls and race conditions

## Core Concepts Covered

### Thread Fundamentals
- **Thread Creation**: Learn to create threads using Thread class, delegates, and lambda expressions
- **Thread Lifecycle**: Understanding thread states (Unstarted, Running, Stopped, WaitSleepJoin)
- **Thread Management**: Joining threads, checking thread status, and graceful shutdown
- **Background vs Foreground**: Understanding when applications terminate based on thread types

### Thread Safety & Synchronization
- **Race Conditions**: Understanding and preventing data corruption in concurrent access
- **Lock Statement**: Using `lock` to ensure exclusive access to shared resources
- **Monitor Class**: Advanced locking mechanisms with timeout and try-enter patterns
- **Thread-Safe Operations**: Making critical sections atomic and safe

### Thread Communication
- **ManualResetEvent**: Signaling between threads for coordination
- **AutoResetEvent**: One-time signaling for thread synchronization
- **Thread Signaling**: Coordinating complex multi-threaded workflows
- **Shared State Management**: Safe ways to share data between threads

## Key Features with Examples

### Thread Creation and Management
```csharp
// Creating a new thread
Thread workerThread = new Thread(DoWork);
workerThread.Name = "Worker Thread";
workerThread.IsBackground = true;
workerThread.Start();

// Lambda expression threads
Thread lambdaThread = new Thread(() => {
    Console.WriteLine("Lambda thread working...");
});
lambdaThread.Start();
lambdaThread.Join(); // Wait for completion
```

### Thread Safety with Locks
```csharp
private static readonly object lockObject = new object();
private static int sharedCounter = 0;

static void SafeIncrement()
{
    lock (lockObject)
    {
        sharedCounter++; // Thread-safe operation
        Console.WriteLine($"Counter: {sharedCounter}");
    }
}
```

### Thread Signaling
```csharp
private static ManualResetEvent signal = new ManualResetEvent(false);

// Waiting thread
static void WaitingThread()
{
    Console.WriteLine("Waiting for signal...");
    signal.WaitOne(); // Block until signal is set
    Console.WriteLine("Signal received!");
}

// Signaling thread
static void SignalingThread()
{
    Thread.Sleep(2000);
    signal.Set(); // Release waiting threads
}
```

### Data Passing to Threads
```csharp
// Using ParameterizedThreadStart
static void ProcessData(object data)
{
    var info = (ProcessingInfo)data;
    Console.WriteLine($"Processing: {info.Name}");
}

// Start with parameter
var info = new ProcessingInfo { Name = "Data Set 1" };
Thread thread = new Thread(ProcessData);
thread.Start(info);
```

## Tips

### Performance Considerations
- **Thread Creation Cost**: Creating threads is expensive - consider thread pooling for short-lived tasks
- **Context Switching**: Too many threads can hurt performance due to context switching overhead
- **CPU-bound vs I/O-bound**: Choose appropriate threading strategies based on workload type
- **Lock Granularity**: Use fine-grained locks to minimize contention and improve throughput

### Common Pitfalls
- **Race Conditions**: Always protect shared state with appropriate synchronization
- **Deadlocks**: Avoid circular dependencies when using multiple locks
- **Thread Starvation**: Don't set too many high-priority threads
- **Resource Leaks**: Always clean up threads and synchronization objects

### Best Practices
- **Thread Naming**: Always name your threads for easier debugging
- **Exception Handling**: Handle exceptions within threads to prevent application crashes
- **Graceful Shutdown**: Implement clean shutdown mechanisms for long-running threads
- **Testing**: Thread issues are hard to reproduce - test thoroughly under load

## Real-World Applications

### Responsive UI Applications
```csharp
// Background processing without blocking UI
Thread backgroundWorker = new Thread(() => {
    // Long-running computation
    PerformHeavyCalculation();
}) { IsBackground = true };
backgroundWorker.Start();
```

### Producer-Consumer Patterns
```csharp
// Multiple threads processing a shared queue
private static Queue<WorkItem> workQueue = new Queue<WorkItem>();
private static object queueLock = new object();

static void ProducerThread()
{
    while (true)
    {
        var item = GenerateWork();
        lock (queueLock)
        {
            workQueue.Enqueue(item);
        }
    }
}

static void ConsumerThread()
{
    while (true)
    {
        WorkItem item = null;
        lock (queueLock)
        {
            if (workQueue.Count > 0)
                item = workQueue.Dequeue();
        }
        if (item != null)
            ProcessWork(item);
    }
}
```

### Parallel Data Processing
```csharp
// Processing data on multiple threads
static void ProcessDataInParallel(IList<DataItem> data)
{
    int threadCount = Environment.ProcessorCount;
    int itemsPerThread = data.Count / threadCount;
    Thread[] threads = new Thread[threadCount];

    for (int i = 0; i < threadCount; i++)
    {
        int start = i * itemsPerThread;
        int end = (i == threadCount - 1) ? data.Count : start + itemsPerThread;
        
        threads[i] = new Thread(() => {
            for (int j = start; j < end; j++)
            {
                ProcessSingleItem(data[j]);
            }
        });
        threads[i].Start();
    }

    // Wait for all threads to complete
    foreach (Thread thread in threads)
        thread.Join();
}
```

## Industry Impact

### Performance Benefits
- **Responsiveness**: Multi-threading keeps applications responsive during heavy operations
- **Throughput**: Parallel processing increases overall system throughput
- **Resource Utilization**: Better CPU and I/O utilization on multi-core systems
- **Scalability**: Multi-threaded applications scale better with hardware improvements

### Career Applications
- **Desktop Applications**: Building responsive WPF, WinForms, or console applications
- **Server Development**: Creating high-performance server applications and services
- **Game Development**: Managing game loops, physics, and AI on separate threads
- **System Programming**: Building operating system components and device drivers
