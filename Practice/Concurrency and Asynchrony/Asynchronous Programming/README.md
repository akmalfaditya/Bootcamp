# Asynchronous Programming in C#

## Learning Objectives

Asynchronous programming is essential for building scalable, responsive applications that can handle I/O operations, web requests, and long-running tasks without blocking the user interface or other operations.

## What You'll Learn

### Core Concepts Covered:

1. **Async/Await Fundamentals**
   - What asynchronous programming solves
   - `async` and `await` keywords
   - `Task` and `Task<T>` return types
   - Async method signatures and conventions

2. **I/O vs CPU-Bound Operations**
   - **I/O-bound**: File operations, web requests, database calls
   - **CPU-bound**: Mathematical calculations, data processing
   - When to use async vs parallel processing
   - Thread pool behavior with async operations

3. **Task Management**
   - Creating and starting tasks
   - Task continuations and chaining
   - Capturing local state and closures
   - Task lifecycle and status

4. **Advanced Async Patterns**
   - **Parallel async execution**: Multiple operations simultaneously
   - **Task combinators**: `Task.WhenAll()`, `Task.WhenAny()`
   - **Cancellation**: `CancellationToken` for cooperative cancellation
   - **Progress reporting**: `IProgress<T>` for operation updates

5. **Modern Async Features**
   - **Async streams** (C# 8+): `IAsyncEnumerable<T>`
   - **Async disposable**: `IAsyncDisposable` interface
   - **ConfigureAwait**: Context switching control
   - Exception handling in async operations

## Key Features Demonstrated

### Basic Async/Await Pattern:
```csharp
// Async method that returns a Task
public async Task<string> FetchDataAsync(string url)
{
    using var client = new HttpClient();
    string result = await client.GetStringAsync(url);
    return result.ToUpper(); // Additional processing
}

// Usage - the calling code can await the result
string data = await FetchDataAsync("https://api.example.com/data");
```

### Parallel Async Operations:
```csharp
// Execute multiple async operations concurrently
async Task<string[]> FetchMultipleUrlsAsync(string[] urls)
{
    // Start all operations simultaneously
    Task<string>[] tasks = urls.Select(url => FetchDataAsync(url)).ToArray();
    
    // Wait for all to complete
    string[] results = await Task.WhenAll(tasks);
    return results;
}
```

### Task Combinators:
```csharp
// WhenAny - complete when first task finishes
Task<string> firstCompleted = await Task.WhenAny(tasks);
string result = await firstCompleted;

// WhenAll - complete when all tasks finish
string[] allResults = await Task.WhenAll(tasks);
```

### Cancellation Support:
```csharp
public async Task<string> ProcessWithCancellationAsync(CancellationToken cancellationToken)
{
    for (int i = 0; i < 1000; i++)
    {
        // Check for cancellation request
        cancellationToken.ThrowIfCancellationRequested();
        
        await Task.Delay(100, cancellationToken); // Cancellable delay
        // Process item i
    }
    return "Processing complete";
}

// Usage with timeout
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
try
{
    await ProcessWithCancellationAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled");
}
```

### Async Streams (C# 8+):
```csharp
// Async enumerable for streaming data
public async IAsyncEnumerable<int> GenerateNumbersAsync()
{
    for (int i = 0; i < 100; i++)
    {
        await Task.Delay(100); // Simulate async work
        yield return i;
    }
}

// Consuming async streams
await foreach (int number in GenerateNumbersAsync())
{
    Console.WriteLine(number);
}
```

## Tips

> **Golden Rule**: Use async for I/O-bound operations (file access, network calls, database queries) and parallel processing for CPU-bound operations (calculations, data processing).

> **Async All the Way**: Once you go async, stay async throughout your call chain. Mixing sync and async can lead to deadlocks, especially in UI applications.

> **ConfigureAwait(false)**: In library code, use `ConfigureAwait(false)` to avoid capturing the synchronization context, improving performance and avoiding deadlocks.

## What to Focus On

1. **When to use async**: I/O operations, web calls, file access
2. **Avoiding deadlocks**: Don't block on async code with `.Result` or `.Wait()`
3. **Exception handling**: How exceptions propagate in async operations
4. **Performance implications**: Async overhead vs. responsiveness benefits

## Run the Project

```bash
dotnet run
```

The demo includes:
- Sync vs async performance comparisons
- Real I/O-bound and CPU-bound examples
- Task combinators in action
- Cancellation and timeout scenarios
- Progress reporting implementations
- Async streams demonstrations
- Exception handling patterns

## Best Practices

1. **Use async/await consistently** - don't mix with blocking calls
2. **Prefer `Task.ConfigureAwait(false)`** in library code
3. **Use `CancellationToken`** for long-running operations
4. **Handle `OperationCanceledException`** appropriately
5. **Don't use `async void`** except for event handlers
6. **Use `Task.WhenAll()`** for parallel async operations
7. **Implement `IAsyncDisposable`** for async cleanup

## Real-World Applications

### Common Async Scenarios:
- **Web API calls**: HTTP requests to external services
- **File operations**: Reading/writing large files
- **Database access**: Entity Framework queries
- **Image processing**: Async image manipulation
- **Email sending**: SMTP operations
- **Cloud storage**: Uploading/downloading files

### Application Types:
- **Web Applications**: Non-blocking request handling
- **Desktop Apps**: Responsive UI during long operations
- **Mobile Apps**: Network calls without freezing UI
- **Microservices**: Scalable service-to-service communication
- **Background Services**: Processing queues and scheduled tasks

## When to Use Async

**Perfect for:**
- File I/O operations
- Network requests (HTTP, TCP, etc.)
- Database calls
- External API integrations
- Long-running background tasks

**Avoid for:**
- CPU-intensive calculations (use parallel processing instead)
- Very short operations (async overhead isn't worth it)
- Synchronous-only APIs
- Simple property access or basic calculations

## Advanced Async Patterns

### Custom Awaitable Types:
```csharp
public class DelayedResult<T>
{
    public TaskAwaiter<T> GetAwaiter() => GetResultAsync().GetAwaiter();
    private async Task<T> GetResultAsync() { /* implementation */ }
}
```

### Async Lazy Initialization:
```csharp
private readonly AsyncLazy<ExpensiveResource> _resource = 
    new AsyncLazy<ExpensiveResource>(InitializeResourceAsync);

public async Task<string> UseResourceAsync()
{
    var resource = await _resource;
    return resource.DoSomething();
}
```

### ValueTask for Performance:
```csharp
// ValueTask avoids allocation when result is immediately available
public ValueTask<int> GetCachedValueAsync(string key)
{
    if (_cache.TryGetValue(key, out int value))
        return new ValueTask<int>(value); // Synchronous completion
    
    return new ValueTask<int>(FetchFromDatabaseAsync(key)); // Async completion
}
```

## Common Pitfalls to Avoid

**Don't:**
- Block on async code with `.Result` or `.Wait()`
- Use `async void` except for event handlers
- Forget to await async operations
- Ignore cancellation tokens in long-running operations

**Do:**
- Use async all the way through your call chain
- Handle exceptions properly in async operations
- Use ConfigureAwait(false) in library code
- Implement proper cancellation support

