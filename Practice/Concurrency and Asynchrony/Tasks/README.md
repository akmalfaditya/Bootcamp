# Task-Based Asynchronous Programming

## Learning Objectives
Master the **Task Parallel Library (TPL)** and understand how to build responsive, scalable applications using task-based asynchronous programming. Learn to orchestrate complex asynchronous operations, handle errors gracefully, and optimize performance through parallel execution.

## What You'll Learn

### Core Task Concepts
- **Task Fundamentals**: Understanding tasks as the building blocks of async programming
- **Task vs Thread**: Why tasks are superior to raw threads for most scenarios
- **Task<T> Generic**: Working with tasks that return values
- **Task Lifecycle**: Created, Running, Completed, Faulted, and Cancelled states

### Task Creation Patterns
- **Task.Run()**: The most common way to start CPU-bound work
- **Task.Factory.StartNew()**: Advanced task creation with custom options
- **Manual Task Creation**: Using constructors for fine-grained control
- **Task.FromResult()**: Creating already-completed tasks for optimization

### Advanced Task Operations
- **Task Continuation**: Chaining operations with ContinueWith()
- **Parallel Execution**: Running multiple tasks simultaneously
- **Task Synchronization**: Coordinating multiple concurrent operations
- **Error Handling**: Managing exceptions in asynchronous contexts

### Cancellation and Timeouts
- **CancellationToken**: Cooperative cancellation for long-running operations
- **Timeout Handling**: Setting time limits on asynchronous operations
- **Graceful Shutdown**: Cleanly terminating work when cancellation is requested
- **Resource Cleanup**: Proper disposal in cancellation scenarios

## Key Features Demonstrated

### 1. **Basic Task Operations**
```csharp
// CPU-bound work on a background thread
Task<int> computeTask = Task.Run(() =>
{
    // Expensive calculation
    return PerformComplexCalculation();
});

int result = await computeTask;
```

### 2. **Parallel Task Execution**
```csharp
// Run multiple operations concurrently
var tasks = new[]
{
    ProcessFileAsync("file1.txt"),
    ProcessFileAsync("file2.txt"),
    ProcessFileAsync("file3.txt")
};

// Wait for all to complete
string[] results = await Task.WhenAll(tasks);
```

### 3. **Task Continuation Chains**
```csharp
var pipeline = Task.Run(() => LoadData())
    .ContinueWith(t => ProcessData(t.Result))
    .ContinueWith(t => SaveResults(t.Result));

await pipeline;
```

### 4. **Cancellation Support**
```csharp
using var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(30)); // 30-second timeout

try
{
    await LongRunningOperationAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled or timed out");
}
```

## Tips

### **Task vs Thread Decision Matrix**
- **Use Tasks When**: I/O operations, CPU-bound work, composition needed
- **Use Threads When**: Very specific thread requirements, legacy interop
- **Default Choice**: Tasks with async/await for 99% of scenarios

### **Performance Optimization**
```csharp
// Good: Efficient parallel processing
var tasks = urls.Select(async url => await DownloadAsync(url));
var results = await Task.WhenAll(tasks);

// Bad: Sequential processing disguised as async
var results = new List<string>();
foreach (var url in urls)
{
    results.Add(await DownloadAsync(url)); // Blocking!
}
```

### **Error Handling Best Practices**
```csharp
// Comprehensive error handling
try
{
    var tasks = new[] { Task1(), Task2(), Task3() };
    await Task.WhenAll(tasks);
}
catch (Exception)
{
    // Check individual task results
    foreach (var task in tasks)
    {
        if (task.IsFaulted)
        {
            // Log specific error
            logger.LogError(task.Exception);
        }
    }
}
```

### **Common Pitfalls**
- **Don't**: Use Task.Wait() or .Result in UI applications (causes deadlocks)
- **Do**: Use await throughout your async call chain
- **Don't**: Create unnecessary tasks for already-synchronous operations
- **Do**: Use Task.FromResult() for immediate values in async methods

## Real-World Applications

### **Web API Development**
```csharp
[HttpGet]
public async Task<ActionResult<List<Product>>> GetProductsAsync()
{
    var tasks = new[]
    {
        GetProductsFromDatabaseAsync(),
        GetPricingFromServiceAsync(),
        GetInventoryFromCacheAsync()
    };
    
    var results = await Task.WhenAll(tasks);
    
    return CombineResults(results[0], results[1], results[2]);
}
```

### **File Processing System**
```csharp
public async Task ProcessFilesBatchAsync(string[] filePaths)
{
    var semaphore = new SemaphoreSlim(Environment.ProcessorCount);
    
    var tasks = filePaths.Select(async filePath =>
    {
        await semaphore.WaitAsync();
        try
        {
            return await ProcessSingleFileAsync(filePath);
        }
        finally
        {
            semaphore.Release();
        }
    });
    
    var results = await Task.WhenAll(tasks);
    await SaveResultsAsync(results);
}
```

### **Real-Time Data Processing**
```csharp
public class DataStreamProcessor
{
    private readonly CancellationTokenSource _cts = new();
    
    public Task StartProcessingAsync()
    {
        var tasks = new[]
        {
            Task.Run(() => ReadDataStreamAsync(_cts.Token)),
            Task.Run(() => ProcessDataQueueAsync(_cts.Token)),
            Task.Run(() => WriteResultsAsync(_cts.Token))
        };
        
        return Task.WhenAll(tasks);
    }
    
    public void Stop() => _cts.Cancel();
}
```

### **Microservice Coordination**
```csharp
public async Task<OrderResult> ProcessOrderAsync(Order order)
{
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
    
    try
    {
        var tasks = new[]
        {
            ValidateInventoryAsync(order.Items, cts.Token),
            ProcessPaymentAsync(order.Payment, cts.Token),
            ReserveShippingAsync(order.Address, cts.Token)
        };
        
        var results = await Task.WhenAll(tasks);
        return new OrderResult { Success = true, OrderId = Guid.NewGuid() };
    }
    catch (OperationCanceledException)
    {
        await CompensateAsync(order); // Rollback any partial work
        throw new TimeoutException("Order processing timeout");
    }
}
```


## Integration with Modern C#

### **Async Streams (C# 8+)**
```csharp
public async IAsyncEnumerable<T> ProcessItemsAsync<T>(
    IEnumerable<T> items,
    [EnumeratorCancellation] CancellationToken ct = default)
{
    await foreach (var item in items.ToAsyncEnumerable().WithCancellation(ct))
    {
        yield return await ProcessItemAsync(item);
    }
}
```

### **Pattern Matching (C# 8+)**
```csharp
var result = await task switch
{
    { IsCompletedSuccessfully: true } => task.Result,
    { IsCanceled: true } => throw new OperationCanceledException(),
    { IsFaulted: true } => throw task.Exception!,
    _ => throw new InvalidOperationException()
};
```

### **Nullable Reference Types (C# 8+)**
```csharp
public async Task<string?> GetDataAsync(CancellationToken ct = default)
{
    var task = await Task.Run(() => FetchData(), ct);
    return task?.ToString();
}
```

### **Top-Level Programs (C# 9+)**
```csharp
// Program.cs can be async in modern C#
await ProcessDataAsync();
await Task.Delay(1000);
Console.WriteLine("Processing complete!");
```

## Industry Impact

Task-based programming is crucial because it:

- **Enables Scalability**: Applications can handle thousands of concurrent operations
- **Improves Responsiveness**: UI applications remain interactive during long operations
- **Optimizes Resources**: Better thread pool utilization and reduced context switching
- **Supports Cloud Architecture**: Essential for microservices and distributed systems
- **Powers Modern Frameworks**: ASP.NET Core, Entity Framework, and most .NET libraries use tasks

