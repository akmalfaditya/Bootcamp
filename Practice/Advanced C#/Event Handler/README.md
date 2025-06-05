# Event Handlers in C#

## Learning Objectives

Events are built on top of delegates and provide a safe, standardized way to implement notifications and loosely-coupled communication between objects.

## What You'll Learn

### Core Concepts Covered:

1. **Event Fundamentals**
   - What events are and how they differ from delegates
   - The publisher-subscriber (observer) pattern
   - Event declaration and usage
   - Event safety and encapsulation

2. **Standard Event Pattern**
   - `EventHandler` and `EventHandler<T>` delegates
   - `EventArgs` and custom event argument classes
   - Sender parameter conventions
   - Best practices for event design

3. **Event Features**
   - Event accessors (add/remove)
   - Thread-safe event handling
   - Event registration and deregistration
   - Null conditional operator with events (`?.`)

4. **Advanced Event Concepts**
   - Custom event accessors
   - Event interface design
   - Weak event patterns
   - Memory leak prevention

5. **Real-World Applications**
   - UI event handling patterns
   - Business event notifications
   - Domain event architecture
   - Asynchronous event handling

## Key Features Demonstrated

### Basic Event Pattern:
```csharp
public class Publisher
{
    // Event declaration - note the 'event' keyword
    public event Action<string> MessageReceived;
    
    // Safe event invocation
    public void SendMessage(string message)
    {
        MessageReceived?.Invoke(message);
    }
}

// Usage
var publisher = new Publisher();
publisher.MessageReceived += msg => Console.WriteLine($"Received: {msg}");
```

### Standard Event Pattern:
```csharp
public class FileProcessor
{
    public event EventHandler<FileProcessedEventArgs> FileProcessed;
    
    protected virtual void OnFileProcessed(string fileName)
    {
        FileProcessed?.Invoke(this, new FileProcessedEventArgs(fileName));
    }
}

public class FileProcessedEventArgs : EventArgs
{
    public string FileName { get; }
    public DateTime ProcessedAt { get; }
    
    public FileProcessedEventArgs(string fileName)
    {
        FileName = fileName;
        ProcessedAt = DateTime.Now;
    }
}
```

## Tips

> **Events vs Delegates**: Events are a special kind of delegate that can only be triggered from within the class that declares them. This provides better encapsulation - external classes can subscribe but can't directly invoke the event.

> **Memory Leak Warning**: Event subscriptions create strong references. Always unsubscribe (`-=`) when you're done, especially with long-lived publishers and short-lived subscribers!

> **Thread Safety**: Events themselves aren't automatically thread-safe. Use proper synchronization when multiple threads might subscribe/unsubscribe or when event handlers might run concurrently.

## What to Focus On

1. **Encapsulation**: Events can only be triggered by their declaring class
2. **Standard patterns**: Follow .NET conventions for consistency
3. **Memory management**: Prevent leaks by unsubscribing appropriately
4. **Exception handling**: One handler's exception shouldn't break others

## Run the Project

```bash
dotnet run
```

The demo includes:
- Basic event declaration and usage
- Standard event pattern implementation
- Thread-safety considerations
- Custom event accessors
- Real-world scenarios and patterns

## Best Practices

1. **Use standard event pattern** (`EventHandler<T>`) for consistency
2. **Always null-check** before invoking: `MyEvent?.Invoke(...)`
3. **Derive from EventArgs** for custom event data
4. **Use weak references** for long-lived event scenarios
5. **Unsubscribe in Dispose()** to prevent memory leaks
6. **Handle exceptions** in event handlers appropriately

## Real-World Applications

### Common Event Scenarios:
- **UI Events**: Button clicks, form submissions, window events
- **File Operations**: File created, modified, deleted notifications
- **Business Logic**: Order processed, payment completed, inventory updated
- **System Events**: Configuration changed, service status updates
- **Game Development**: Player actions, collision detection, game state changes

### Architectural Patterns:
- **Domain Events**: Business rule violations, state changes
- **Message Bus**: Decoupled component communication
- **Observer Pattern**: UI updates based on model changes
- **Publish-Subscribe**: Microservice communication

## Event Design Guidelines

**Good Event Design:**
- Events represent notifications of something that happened
- Event names use past tense (OrderProcessed, not ProcessOrder)
- Include relevant context in EventArgs
- Follow standard sender/args pattern

**Avoid:**
- Using events for command-like operations
- Directly assigning to events outside the declaring class
- Forgetting to unsubscribe from long-lived objects
- Exposing events that change object state

## Advanced Patterns

### Weak Event Pattern:
```csharp
// For scenarios where subscriber lifetime is uncertain
WeakEventManager.AddHandler(publisher, "PropertyChanged", handler);
```

### Async Event Handling:
```csharp
public event Func<string, Task> AsyncEventHandler;

// Invocation
await AsyncEventHandler?.Invoke("data");
```

Remember: Events are about communication and notification. They enable loose coupling by allowing objects to communicate without knowing about each other directly. Master events, and you'll build more maintainable, flexible applications!
