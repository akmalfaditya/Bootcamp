# Debug and Trace Classes in C# - Comprehensive Guide

This project demonstrates all the essential concepts for debugging and logging using C#'s Debug and Trace classes. Think of this as your complete toolkit for diagnosing issues and monitoring application behavior.

## What You'll Learn

### 1. Fundamental Differences: Debug vs Trace
- **Debug**: Only works in DEBUG builds, completely removed in Release
- **Trace**: Works in both DEBUG and RELEASE builds
- **Key Insight**: Use Debug for development-only diagnostics, Trace for production monitoring

### 2. Basic Logging Methods
```csharp
// Debug methods (development only)
Debug.Write("Message");           // Write without newline
Debug.WriteLine("Message");       // Write with newline
Debug.WriteIf(condition, "msg");  // Conditional writing

// Trace methods (development + production)
Trace.Write("Message");           // Write without newline
Trace.WriteLine("Message");       // Write with newline
Trace.WriteIf(condition, "msg");  // Conditional writing
```

### 3. Trace Message Levels
```csharp
Trace.TraceInformation("General info");     // Informational messages
Trace.TraceWarning("Something's off");      // Warnings - not critical
Trace.TraceError("Something went wrong");   // Errors - needs attention
```

### 4. Assertions - Your Safety Net
```csharp
Debug.Assert(condition, "Error message");           // Breaks into debugger if false
Debug.Assert(condition, "Message", "Details");      // With detailed info
Debug.Fail("Explicit failure message");             // Always fails
```

**Tips**: Assertions help catch bugs early by validating your assumptions about program state.

### 5. Trace Listeners - Where Your Logs Go

#### Built-in Listeners
- **TextWriterTraceListener**: Writes to files or console
- **DelimitedListTraceListener**: Creates CSV-style structured logs
- **XmlWriterTraceListener**: Outputs XML format for structured analysis
- **EventLogTraceListener**: Writes to Windows Event Log

#### Setting Up Listeners
```csharp
// File logging
Trace.Listeners.Add(new TextWriterTraceListener("app.log"));

// CSV logging
var csvListener = new DelimitedListTraceListener("data.csv");
csvListener.Delimiter = ",";
Trace.Listeners.Add(csvListener);

// XML logging
Trace.Listeners.Add(new XmlWriterTraceListener("trace.xml"));
```

### 6. Filtering - Control What Gets Logged
```csharp
// Only log warnings and errors
var listener = new TextWriterTraceListener("errors.log");
listener.Filter = new EventTypeFilter(SourceLevels.Warning | SourceLevels.Error);
Trace.Listeners.Add(listener);
```

**Available Filter Levels**:
- `SourceLevels.All` - Everything
- `SourceLevels.Information` - Info and above
- `SourceLevels.Warning` - Warnings and errors only
- `SourceLevels.Error` - Errors only
- `SourceLevels.Critical` - Critical issues only

### 7. Trace Output Options
Add context to your log messages:
```csharp
listener.TraceOutputOptions = TraceOptions.DateTime |      // Timestamp
                             TraceOptions.ProcessId |     // Process ID
                             TraceOptions.ThreadId |      // Thread ID
                             TraceOptions.Callstack;      // Call stack
```

### 8. Resource Management
Always clean up properly:
```csharp
Trace.Flush();  // Ensure all messages are written
Trace.Close();  // Close all listeners (do this at app shutdown)

// Or enable auto-flush for real-time output
Trace.AutoFlush = true;
```

## Real-World Patterns

### Performance Monitoring
```csharp
var stopwatch = Stopwatch.StartNew();
// ... do work ...
stopwatch.Stop();
Trace.TraceInformation($"Operation completed in {stopwatch.ElapsedMilliseconds}ms");
```

### Error Handling
```csharp
try
{
    // Risky operation
}
catch (Exception ex)
{
    Trace.TraceError($"Operation failed: {ex.Message}");
    Debug.WriteLine($"Stack trace: {ex.StackTrace}");
}
```

### Security Logging
```csharp
if (authenticationSuccessful)
{
    Trace.TraceInformation($"User login successful: {username}");
}
else
{
    Trace.TraceWarning($"Failed login attempt: {username}");
}
```

## Configuration Strategies

### Development Setup
- Enable all trace levels
- Include call stacks and detailed context
- Use multiple output formats for analysis

### Production Setup
- Filter to warnings and errors only
- Include timestamps and process IDs
- Log to files and possibly event log
- Avoid performance-impacting features

### Testing Setup
- Comprehensive logging for troubleshooting
- Include performance metrics
- Use structured formats for automated analysis

## Best Practices

1. **Use Debug for development diagnostics** - It disappears in release builds
2. **Use Trace for production monitoring** - Available in all builds
3. **Set up logging early** - Configure listeners at application startup
4. **Use appropriate message levels** - Information < Warning < Error < Critical
5. **Filter intelligently** - Don't overwhelm yourself with too much data
6. **Include context** - Timestamps, thread IDs, and process IDs help debugging
7. **Clean up resources** - Always flush and close listeners
8. **Test your logging** - Make sure logs work in all build configurations

## File Structure in This Project

- `Program.cs` - Main demonstration with comprehensive examples
- `DiagnosticUtilities.cs` - Advanced patterns and custom listeners
- `logs/` - Generated log files (created when you run the project)

## Running the Project

1. **Debug Build**: `dotnet run` - Shows both Debug and Trace output
2. **Release Build**: `dotnet run -c Release` - Shows only Trace output
3. **Check the logs**: Look in the `logs/` directory for generated files

## Common Gotchas

- **Debug messages disappear in Release** - By design! Use Trace for production logging
- **Forgetting to flush** - Your last messages might not appear without Trace.Flush()
- **Too much logging** - Filter appropriately to avoid information overload
- **Not handling exceptions in logging** - Don't let logging failures crash your app

## When to Use What

- **Debug.WriteLine()**: Development diagnostics that should disappear in production
- **Trace.TraceInformation()**: General application flow information
- **Trace.TraceWarning()**: Something's not quite right, but not critical
- **Trace.TraceError()**: Something went wrong that needs attention
- **Debug.Assert()**: Validate assumptions during development
