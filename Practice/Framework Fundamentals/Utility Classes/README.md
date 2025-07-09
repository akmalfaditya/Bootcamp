# Utility Classes

## Learning Objectives

By completing this project, you will acquire the following competencies:
- **Master Console Operations**: Implement comprehensive input/output handling, text formatting, color management, and cursor positioning techniques
- **Access Environment Information**: Retrieve and utilize system details, environment variables, and runtime configuration data
- **Manage External Processes**: Launch, monitor, and establish communication with external applications and system processes
- **Handle Application Context**: Work with feature switches, data directories, and application-wide configuration management
- **Build Interactive Applications**: Design and implement user-friendly console interfaces with proper user experience considerations
- **Implement System Integration**: Establish secure and efficient connections between your applications and the operating system
- **Apply Diagnostic Techniques**: Develop comprehensive system information gathering capabilities for troubleshooting and monitoring purposes

## Core Concepts and Implementation Details

### Console Class: Foundation of Terminal-Based User Interaction

The Console class serves as the primary interface between your application and the terminal environment. It provides a comprehensive set of methods and properties that enable sophisticated user interaction patterns commonly required in enterprise applications.

#### Input Operations and Data Collection
The Console class offers multiple input methods, each designed for specific use cases:

**ReadLine Method**: Captures complete lines of user input, making it ideal for collecting textual data such as names, addresses, or configuration values. This method blocks execution until the user presses Enter, providing a synchronous input experience.

**ReadKey Method**: Captures individual keystrokes without requiring the Enter key. This method is particularly valuable for creating interactive menus, confirmation dialogs, or real-time input scenarios where immediate response is required.

**Read Method**: Captures single characters as integer values, providing low-level input control for specialized applications that require character-by-character processing.

```csharp
// Comprehensive input demonstration
Console.Write("Enter your full name: ");
string fullName = Console.ReadLine() ?? string.Empty;

Console.WriteLine("Press any key to continue (Y/N): ");
ConsoleKeyInfo keyInfo = Console.ReadKey(true);
bool confirmed = keyInfo.Key == ConsoleKey.Y;
```

#### Output Operations and Formatting
Console output operations support sophisticated formatting capabilities that enable professional presentation of information:

**Composite Formatting**: Utilizes placeholder syntax similar to string.Format, allowing for precise control over how data is presented. This approach supports culture-specific formatting for numbers, dates, and other data types.

**String Interpolation**: Provides a modern, readable approach to embedding expressions directly within string literals, enhancing code maintainability and readability.

**Formatted Output**: Supports standard and custom format specifiers for precise control over numeric, date, and string presentation.

```csharp
// Professional output formatting examples
int taskCount = 150;
double completionRate = 0.847;
DateTime completionTime = DateTime.Now.AddHours(2.5);

// Composite formatting with culture awareness
Console.WriteLine("Tasks completed: {0:N0} out of {1:N0} ({2:P2})", 
    taskCount * completionRate, taskCount, completionRate);

// String interpolation for readability
Console.WriteLine($"Estimated completion: {completionTime:yyyy-MM-dd HH:mm:ss}");

// Custom formatting for professional output
Console.WriteLine("Progress report: {0} items processed at {1:F2} items per minute", 
    taskCount, taskCount / 60.0);
```

#### Console Appearance and Visual Presentation
Professional console applications require sophisticated visual presentation capabilities:

**Color Management**: The Console class provides comprehensive color control through ForegroundColor and BackgroundColor properties. These capabilities enable the creation of visually distinct message categories such as errors, warnings, and informational messages.

**Window Properties**: Applications can query and modify console window dimensions, including width, height, and buffer sizes. This information is crucial for creating responsive layouts that adapt to different terminal configurations.

**Cursor Positioning**: Precise cursor control enables the creation of dynamic user interfaces, progress indicators, and data update scenarios without requiring complete screen refreshes.

```csharp
// Professional color scheme implementation
public static class ConsoleTheme
{
    public static void WriteError(string message)
    {
        ConsoleColor original = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {message}");
        Console.ForegroundColor = original;
    }
    
    public static void WriteSuccess(string message)
    {
        ConsoleColor original = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"SUCCESS: {message}");
        Console.ForegroundColor = original;
    }
    
    public static void WriteWarning(string message)
    {
        ConsoleColor original = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"WARNING: {message}");
        Console.ForegroundColor = original;
    }
}
```

#### Stream Redirection and Output Management
Console stream redirection provides powerful capabilities for logging, testing, and output management:

**Output Redirection**: The Console.SetOut method enables redirection of console output to files, memory streams, or custom TextWriter implementations. This capability is essential for creating logging systems and capturing program output for analysis.

**Input Redirection**: Similarly, Console.SetIn allows redirection of input sources, enabling automated testing scenarios and batch processing capabilities.

**Dual Output Scenarios**: Professional applications often require simultaneous output to multiple destinations, such as both console and log files.

```csharp
// Professional logging implementation with redirection
public static class OutputManager
{
    public static void ExecuteWithLogging(string logFile, Action operation)
    {
        TextWriter originalOutput = Console.Out;
        
        try
        {
            using (StreamWriter fileWriter = new StreamWriter(logFile, append: true))
            using (var dualWriter = new DualOutputWriter(originalOutput, fileWriter))
            {
                Console.SetOut(dualWriter);
                operation();
            }
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }
}
```

### Environment Class: System Information and Configuration Management

The Environment class provides comprehensive access to system-level information and configuration data. This class is fundamental for creating applications that adapt to different execution environments and provide detailed diagnostic information.

#### System Information Retrieval
The Environment class exposes extensive system information that is crucial for application diagnostics and environment-specific behavior:

**Hardware Information**: Properties such as ProcessorCount provide insights into system capabilities, enabling applications to optimize performance based on available resources.

**Operating System Details**: OSVersion, Platform, and related properties provide detailed information about the execution environment, enabling platform-specific optimizations and compatibility checks.

**User Context**: UserName, UserDomainName, and UserInteractive properties provide information about the current user context, essential for security and personalization features.

```csharp
// Comprehensive system information gathering
public static class SystemDiagnostics
{
    public static SystemInfo GatherSystemInformation()
    {
        return new SystemInfo
        {
            MachineName = Environment.MachineName,
            OperatingSystem = Environment.OSVersion.ToString(),
            Platform = Environment.OSVersion.Platform.ToString(),
            ProcessorCount = Environment.ProcessorCount,
            Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
            Is64BitProcess = Environment.Is64BitProcess,
            UserName = Environment.UserName,
            UserDomainName = Environment.UserDomainName,
            SystemDirectory = Environment.SystemDirectory,
            CurrentDirectory = Environment.CurrentDirectory,
            WorkingSetMemory = Environment.WorkingSet,
            TickCount = Environment.TickCount,
            Version = Environment.Version.ToString()
        };
    }
}
```

#### Environment Variables and Configuration
Environment variables serve as a primary mechanism for external configuration in professional applications:

**Variable Access**: GetEnvironmentVariable and GetEnvironmentVariables methods provide access to system and user-defined configuration values.

**Variable Management**: SetEnvironmentVariable enables dynamic configuration changes during application execution.

**Security Considerations**: Environment variables often contain sensitive information such as database connection strings, API keys, and authentication tokens, requiring careful handling.

```csharp
// Professional environment variable management
public static class ConfigurationManager
{
    private static readonly Dictionary<string, string> DefaultValues = new()
    {
        { "LOG_LEVEL", "Information" },
        { "CONNECTION_TIMEOUT", "30" },
        { "MAX_RETRY_ATTEMPTS", "3" }
    };
    
    public static string GetConfigurationValue(string key, string defaultValue = null)
    {
        string value = Environment.GetEnvironmentVariable(key);
        
        if (!string.IsNullOrEmpty(value))
            return value;
            
        if (DefaultValues.TryGetValue(key, out string fallback))
            return fallback;
            
        return defaultValue ?? throw new ConfigurationException($"Required configuration value '{key}' not found");
    }
    
    public static T GetConfigurationValue<T>(string key, T defaultValue = default) where T : IConvertible
    {
        string stringValue = GetConfigurationValue(key, defaultValue?.ToString());
        
        try
        {
            return (T)Convert.ChangeType(stringValue, typeof(T));
        }
        catch (Exception ex)
        {
            throw new ConfigurationException($"Invalid configuration value for '{key}': {stringValue}", ex);
        }
    }
}
```

#### Special Folders and Path Management
The Environment class provides cross-platform access to system-defined folders:

**GetFolderPath Method**: Returns paths to special folders such as Application Data, User Profile, and Program Files. This approach ensures compatibility across different operating systems and user configurations.

**Path Construction**: Combining special folder paths with application-specific subdirectories creates robust file storage solutions that respect operating system conventions.

```csharp
// Professional path management implementation
public static class ApplicationPaths
{
    public static string GetApplicationDataPath(string applicationName)
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string applicationPath = Path.Combine(appDataPath, applicationName);
        
        if (!Directory.Exists(applicationPath))
        {
            Directory.CreateDirectory(applicationPath);
        }
        
        return applicationPath;
    }
    
    public static string GetLogFilePath(string applicationName, string logFileName = null)
    {
        string applicationPath = GetApplicationDataPath(applicationName);
        string fileName = logFileName ?? $"{applicationName}_{DateTime.Now:yyyyMMdd}.log";
        return Path.Combine(applicationPath, "Logs", fileName);
    }
    
    public static string GetConfigurationFilePath(string applicationName, string configFileName = "app.config")
    {
        string applicationPath = GetApplicationDataPath(applicationName);
        return Path.Combine(applicationPath, configFileName);
    }
}
```

### Process Class: External Application Management and System Integration

The Process class provides comprehensive capabilities for launching, monitoring, and communicating with external applications and system processes. This functionality is essential for creating applications that integrate with existing system tools and services.

#### Process Creation and Lifecycle Management
Professional process management requires careful attention to configuration, monitoring, and resource cleanup:

**ProcessStartInfo Configuration**: This class provides detailed control over process execution parameters, including working directory, environment variables, and security context.

**Stream Redirection**: Capturing standard input, output, and error streams enables sophisticated communication patterns between your application and external processes.

**Lifecycle Monitoring**: Properties such as StartTime, ExitTime, and ExitCode provide detailed information about process execution for logging and debugging purposes.

```csharp
// Professional process management implementation
public static class ProcessManager
{
    public static async Task<ProcessExecutionResult> ExecuteProcessAsync(
        string fileName, 
        string arguments = "", 
        string workingDirectory = null, 
        int timeoutMs = 30000)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            CreateNoWindow = true
        };
        
        using var process = new Process { StartInfo = processInfo };
        var executionResult = new ProcessExecutionResult
        {
            StartTime = DateTime.Now
        };
        
        try
        {
            process.Start();
            executionResult.ProcessId = process.Id;
            
            // Capture output and error streams asynchronously
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();
            
            // Wait for completion with timeout
            bool completed = await Task.Run(() => process.WaitForExit(timeoutMs));
            
            if (!completed)
            {
                process.Kill();
                executionResult.TimedOut = true;
                executionResult.ErrorMessage = "Process execution timed out";
            }
            else
            {
                executionResult.ExitCode = process.ExitCode;
                executionResult.StandardOutput = await outputTask;
                executionResult.StandardError = await errorTask;
            }
            
            executionResult.EndTime = DateTime.Now;
            executionResult.ExecutionDuration = executionResult.EndTime - executionResult.StartTime;
            
            return executionResult;
        }
        catch (Exception ex)
        {
            executionResult.ErrorMessage = ex.Message;
            executionResult.EndTime = DateTime.Now;
            return executionResult;
        }
    }
}

public class ProcessExecutionResult
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan ExecutionDuration { get; set; }
    public int ProcessId { get; set; }
    public int ExitCode { get; set; }
    public string StandardOutput { get; set; } = string.Empty;
    public string StandardError { get; set; } = string.Empty;
    public bool TimedOut { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage) && !TimedOut && ExitCode == 0;
}
```

#### Advanced Stream Management
Professional applications require sophisticated handling of process communication streams to prevent deadlocks and ensure reliable data transfer:

**Asynchronous Stream Processing**: Reading from multiple streams simultaneously prevents blocking scenarios that can cause application hangs.

**Buffer Management**: Large output volumes require careful buffer management to prevent memory exhaustion.

**Error Handling**: Comprehensive error handling ensures graceful recovery from process failures and communication errors.

```csharp
// Advanced stream management for complex process communication
public static class AdvancedProcessCommunication
{
    public static async Task<string> ExecuteWithInputAsync(
        string fileName, 
        string arguments, 
        string inputData, 
        CancellationToken cancellationToken = default)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        
        using var process = new Process { StartInfo = processInfo };
        process.Start();
        
        // Send input data asynchronously
        var inputTask = SendInputAsync(process.StandardInput, inputData, cancellationToken);
        
        // Read output streams asynchronously
        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        
        // Wait for all operations to complete
        await Task.WhenAll(inputTask, outputTask, errorTask);
        await process.WaitForExitAsync(cancellationToken);
        
        string output = await outputTask;
        string errors = await errorTask;
        
        if (!string.IsNullOrEmpty(errors))
        {
            throw new ProcessExecutionException($"Process execution failed: {errors}");
        }
        
        return output;
    }
    
    private static async Task SendInputAsync(
        StreamWriter inputStream, 
        string inputData, 
        CancellationToken cancellationToken)
    {
        try
        {
            await inputStream.WriteAsync(inputData);
            await inputStream.FlushAsync();
            inputStream.Close();
        }
        catch (Exception ex)
        {
            throw new ProcessCommunicationException("Failed to send input data to process", ex);
        }
    }
}
```

#### Process Monitoring and System Diagnostics
The Process class enables comprehensive system monitoring capabilities essential for performance analysis and resource management:

**System Process Enumeration**: GetProcesses method provides access to all running processes, enabling system monitoring and resource analysis.

**Performance Metrics**: Properties such as WorkingSet64, PagedMemorySize64, and ProcessorTime provide detailed performance information.

**Security Considerations**: Process access is subject to operating system security restrictions, requiring appropriate error handling for access denied scenarios.

```csharp
// Comprehensive system monitoring implementation
public static class SystemMonitor
{
    public static List<ProcessInfo> GetSystemProcessInfo()
    {
        var processInfoList = new List<ProcessInfo>();
        
        foreach (var process in Process.GetProcesses())
        {
            try
            {
                var processInfo = new ProcessInfo
                {
                    ProcessId = process.Id,
                    ProcessName = process.ProcessName,
                    StartTime = process.StartTime,
                    WorkingSetMemory = process.WorkingSet64,
                    PagedMemory = process.PagedMemorySize64,
                    VirtualMemory = process.VirtualMemorySize64,
                    ThreadCount = process.Threads.Count,
                    HandleCount = process.HandleCount
                };
                
                processInfoList.Add(processInfo);
            }
            catch (Exception ex)
            {
                // Handle access denied and other exceptions
                var limitedInfo = new ProcessInfo
                {
                    ProcessId = process.Id,
                    ProcessName = process.ProcessName,
                    ErrorMessage = ex.Message
                };
                
                processInfoList.Add(limitedInfo);
            }
            finally
            {
                process.Dispose();
            }
        }
        
        return processInfoList.OrderByDescending(p => p.WorkingSetMemory).ToList();
    }
}
```

### AppContext Class: Application Configuration and Feature Management

The AppContext class provides essential functionality for managing application-wide configuration, feature switches, and runtime context information. This class is particularly important in modern applications that require flexible configuration management and feature toggle capabilities.

#### Application Context Information
The AppContext class provides fundamental information about the application's execution environment:

**BaseDirectory Property**: Returns the absolute path to the directory containing the application's executable files. This information is crucial for locating configuration files, resource files, and dependent assemblies.

**TargetFrameworkName Property**: Provides information about the target framework version specified during application compilation. This property is valuable for compatibility checks and framework-specific optimizations.

```csharp
// Professional application context management
public static class ApplicationContext
{
    public static ApplicationInfo GetApplicationInfo()
    {
        return new ApplicationInfo
        {
            BaseDirectory = AppContext.BaseDirectory,
            TargetFramework = AppContext.TargetFrameworkName,
            ApplicationName = Assembly.GetExecutingAssembly().GetName().Name,
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
            Location = Assembly.GetExecutingAssembly().Location,
            ConfigurationDirectory = GetConfigurationDirectory(),
            DataDirectory = GetDataDirectory()
        };
    }
    
    private static string GetConfigurationDirectory()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "Configuration");
        Directory.CreateDirectory(configPath);
        return configPath;
    }
    
    private static string GetDataDirectory()
    {
        string dataPath = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataPath);
        return dataPath;
    }
}
```

#### Feature Switch Management
Feature switches provide a powerful mechanism for controlling application behavior at runtime without requiring code changes or redeployment:

**SetSwitch Method**: Enables dynamic configuration of boolean feature flags that can control application behavior, feature availability, and experimental functionality.

**TryGetSwitch Method**: Provides safe access to feature switch values with explicit handling of undefined switches, preventing runtime errors and providing graceful fallback behavior.

**Library Integration**: Feature switches enable library authors to provide backward compatibility options and allow consumers to opt into new behaviors gradually.

```csharp
// Professional feature management system
public static class FeatureManager
{
    private static readonly Dictionary<string, FeatureDefinition> KnownFeatures = new()
    {
        { "Application.NewUserInterface", new FeatureDefinition("Enables the new user interface components", false) },
        { "Application.AdvancedLogging", new FeatureDefinition("Enables detailed logging and diagnostics", true) },
        { "Application.BetaFeatures", new FeatureDefinition("Enables experimental beta functionality", false) },
        { "Application.PerformanceOptimizations", new FeatureDefinition("Enables performance optimization features", true) },
        { "Application.SecurityEnhancements", new FeatureDefinition("Enables additional security measures", true) }
    };
    
    public static void InitializeFeatures()
    {
        foreach (var feature in KnownFeatures)
        {
            AppContext.SetSwitch(feature.Key, feature.Value.DefaultValue);
        }
        
        // Load feature overrides from configuration
        LoadFeatureOverrides();
    }
    
    public static bool IsFeatureEnabled(string featureName)
    {
        if (AppContext.TryGetSwitch(featureName, out bool isEnabled))
        {
            return isEnabled;
        }
        
        // Return default value if feature is not explicitly set
        if (KnownFeatures.TryGetValue(featureName, out FeatureDefinition feature))
        {
            return feature.DefaultValue;
        }
        
        // Unknown features are disabled by default
        return false;
    }
    
    public static void EnableFeature(string featureName, bool enabled = true)
    {
        AppContext.SetSwitch(featureName, enabled);
        LogFeatureChange(featureName, enabled);
    }
    
    private static void LoadFeatureOverrides()
    {
        string configFile = Path.Combine(AppContext.BaseDirectory, "features.config");
        
        if (File.Exists(configFile))
        {
            foreach (string line in File.ReadAllLines(configFile))
            {
                if (TryParseFeatureLine(line, out string featureName, out bool enabled))
                {
                    AppContext.SetSwitch(featureName, enabled);
                }
            }
        }
    }
    
    private static bool TryParseFeatureLine(string line, out string featureName, out bool enabled)
    {
        featureName = string.Empty;
        enabled = false;
        
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            return false;
            
        var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
            return false;
            
        featureName = parts[0].Trim();
        return bool.TryParse(parts[1].Trim(), out enabled);
    }
    
    private static void LogFeatureChange(string featureName, bool enabled)
    {
        Console.WriteLine($"Feature '{featureName}' {(enabled ? "enabled" : "disabled")}");
    }
}

public record FeatureDefinition(string Description, bool DefaultValue);
```

#### Data Directory Management and Configuration
Professional applications require sophisticated data directory management for storing configuration files, user data, and application state:

**Configuration File Location**: Establishing consistent patterns for configuration file storage ensures predictable application behavior across different deployment environments.

**User Data Management**: Separating user-specific data from application files enables proper multi-user support and data isolation.

**Backup and Migration**: Implementing data directory management patterns facilitates application updates and data migration scenarios.

```csharp
// Comprehensive data directory management
public static class DataDirectoryManager
{
    private static readonly string ApplicationName = "UtilityClassesDemo";
    
    public static void InitializeDirectories()
    {
        var directories = new[]
        {
            GetConfigurationDirectory(),
            GetLogsDirectory(),
            GetTempDirectory(),
            GetBackupDirectory(),
            GetUserDataDirectory()
        };
        
        foreach (string directory in directories)
        {
            Directory.CreateDirectory(directory);
        }
    }
    
    public static string GetConfigurationDirectory()
    {
        return Path.Combine(AppContext.BaseDirectory, "Configuration");
    }
    
    public static string GetLogsDirectory()
    {
        return Path.Combine(AppContext.BaseDirectory, "Logs");
    }
    
    public static string GetTempDirectory()
    {
        return Path.Combine(AppContext.BaseDirectory, "Temp");
    }
    
    public static string GetBackupDirectory()
    {
        return Path.Combine(AppContext.BaseDirectory, "Backups");
    }
    
    public static string GetUserDataDirectory()
    {
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(userProfile, ApplicationName);
    }
    
    public static void CleanupTempDirectory(TimeSpan maxAge)
    {
        string tempDir = GetTempDirectory();
        var cutoffTime = DateTime.Now - maxAge;
        
        foreach (string file in Directory.GetFiles(tempDir))
        {
            var fileInfo = new FileInfo(file);
            if (fileInfo.CreationTime < cutoffTime)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete temporary file {file}: {ex.Message}");
                }
            }
        }
    }
}
```

## Implementation Patterns and Best Practices

### Error Handling and Exception Management

Professional utility class implementations require comprehensive error handling strategies that provide meaningful feedback while maintaining application stability:

#### Graceful Degradation Patterns
When working with system resources and external processes, applications must handle various failure scenarios gracefully:

```csharp
public static class RobustSystemOperations
{
    public static SystemOperationResult<T> ExecuteWithFallback<T>(
        Func<T> primaryOperation,
        Func<T> fallbackOperation,
        string operationName)
    {
        try
        {
            T result = primaryOperation();
            return SystemOperationResult<T>.Success(result);
        }
        catch (Exception primaryEx)
        {
            try
            {
                T fallbackResult = fallbackOperation();
                return SystemOperationResult<T>.SuccessWithWarning(
                    fallbackResult, 
                    $"Primary operation failed, used fallback: {primaryEx.Message}");
            }
            catch (Exception fallbackEx)
            {
                return SystemOperationResult<T>.Failure(
                    $"Both primary and fallback operations failed for {operationName}. " +
                    $"Primary: {primaryEx.Message}, Fallback: {fallbackEx.Message}");
            }
        }
    }
}

public class SystemOperationResult<T>
{
    public bool IsSuccess { get; private set; }
    public T Value { get; private set; }
    public string ErrorMessage { get; private set; }
    public string WarningMessage { get; private set; }
    
    public static SystemOperationResult<T> Success(T value) =>
        new() { IsSuccess = true, Value = value };
        
    public static SystemOperationResult<T> SuccessWithWarning(T value, string warning) =>
        new() { IsSuccess = true, Value = value, WarningMessage = warning };
        
    public static SystemOperationResult<T> Failure(string error) =>
        new() { IsSuccess = false, ErrorMessage = error };
}
```

#### Resource Management and Cleanup
Proper resource management is crucial when working with processes, streams, and system resources:

```csharp
public static class ResourceManager
{
    public static async Task<TResult> ExecuteWithManagedResourcesAsync<TResource, TResult>(
        Func<TResource> resourceFactory,
        Func<TResource, Task<TResult>> operation,
        TimeSpan timeout = default)
        where TResource : IDisposable
    {
        if (timeout == default)
            timeout = TimeSpan.FromMinutes(5);
            
        using var cancellationTokenSource = new CancellationTokenSource(timeout);
        TResource resource = default;
        
        try
        {
            resource = resourceFactory();
            return await operation(resource).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException($"Operation timed out after {timeout.TotalSeconds} seconds");
        }
        finally
        {
            resource?.Dispose();
        }
    }
}
```

### Performance Optimization Strategies

When working with utility classes, performance considerations become critical for enterprise applications:

#### Efficient Process Communication
Large-scale applications require optimized process communication patterns:

```csharp
public static class OptimizedProcessCommunication
{
    private static readonly ObjectPool<StringBuilder> StringBuilderPool = 
        new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());
    
    public static async Task<string> ExecuteCommandOptimizedAsync(
        string command, 
        string arguments,
        int bufferSize = 4096)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        
        var outputBuilder = StringBuilderPool.Get();
        var errorBuilder = StringBuilderPool.Get();
        
        try
        {
            var buffer = new char[bufferSize];
            
            // Use streaming read for large outputs
            var outputTask = ReadStreamAsync(process.StandardOutput, outputBuilder, buffer);
            var errorTask = ReadStreamAsync(process.StandardError, errorBuilder, buffer);
            
            await Task.WhenAll(outputTask, errorTask);
            await process.WaitForExitAsync();
            
            if (process.ExitCode != 0)
            {
                throw new ProcessExecutionException(
                    $"Process exited with code {process.ExitCode}: {errorBuilder}");
            }
            
            return outputBuilder.ToString();
        }
        finally
        {
            StringBuilderPool.Return(outputBuilder);
            StringBuilderPool.Return(errorBuilder);
        }
    }
    
    private static async Task ReadStreamAsync(
        StreamReader reader, 
        StringBuilder builder, 
        char[] buffer)
    {
        int bytesRead;
        while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            builder.Append(buffer, 0, bytesRead);
        }
    }
}
```

#### Memory-Efficient System Information Collection
Large-scale monitoring applications require efficient data collection patterns:

```csharp
public static class EfficientSystemMonitoring
{
    private static readonly ConcurrentDictionary<int, ProcessSnapshot> ProcessCache = new();
    private static readonly Timer CacheCleanupTimer = new(CleanupCache, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
    
    public static IEnumerable<ProcessInfo> GetProcessInfoEfficient()
    {
        var currentTime = DateTime.UtcNow;
        var processes = Process.GetProcesses();
        
        foreach (var process in processes)
        {
            ProcessInfo info;
            
            try
            {
                // Use cached data if available and recent
                if (ProcessCache.TryGetValue(process.Id, out ProcessSnapshot cached) &&
                    currentTime - cached.LastUpdate < TimeSpan.FromSeconds(30))
                {
                    info = cached.ProcessInfo;
                }
                else
                {
                    info = CreateProcessInfo(process);
                    ProcessCache[process.Id] = new ProcessSnapshot
                    {
                        ProcessInfo = info,
                        LastUpdate = currentTime
                    };
                }
                
                yield return info;
            }
            catch (Exception ex)
            {
                yield return new ProcessInfo
                {
                    ProcessId = process.Id,
                    ProcessName = process.ProcessName,
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                process.Dispose();
            }
        }
    }
    
    private static ProcessInfo CreateProcessInfo(Process process)
    {
        return new ProcessInfo
        {
            ProcessId = process.Id,
            ProcessName = process.ProcessName,
            StartTime = process.StartTime,
            WorkingSetMemory = process.WorkingSet64,
            PagedMemory = process.PagedMemorySize64,
            VirtualMemory = process.VirtualMemorySize64,
            ThreadCount = process.Threads.Count,
            HandleCount = process.HandleCount,
            ProcessorTime = process.TotalProcessorTime
        };
    }
    
    private static void CleanupCache(object state)
    {
        var cutoffTime = DateTime.UtcNow - TimeSpan.FromMinutes(10);
        var keysToRemove = ProcessCache
            .Where(kvp => kvp.Value.LastUpdate < cutoffTime)
            .Select(kvp => kvp.Key)
            .ToList();
            
        foreach (int key in keysToRemove)
        {
            ProcessCache.TryRemove(key, out _);
        }
    }
}

public record ProcessSnapshot
{
    public ProcessInfo ProcessInfo { get; init; }
    public DateTime LastUpdate { get; init; }
}
```

### Security Considerations and Best Practices

Professional applications must implement comprehensive security measures when working with system resources:

#### Secure Environment Variable Handling
Environment variables often contain sensitive information requiring careful management:

```csharp
public static class SecureEnvironmentManager
{
    private static readonly string[] SensitiveVariablePatterns = 
    {
        "*PASSWORD*", "*SECRET*", "*KEY*", "*TOKEN*", "*CREDENTIAL*"
    };
    
    public static Dictionary<string, string> GetFilteredEnvironmentVariables()
    {
        var filtered = new Dictionary<string, string>();
        
        foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
        {
            string key = entry.Key.ToString();
            string value = entry.Value?.ToString() ?? string.Empty;
            
            if (IsSensitiveVariable(key))
            {
                filtered[key] = MaskSensitiveValue(value);
            }
            else
            {
                filtered[key] = value;
            }
        }
        
        return filtered;
    }
    
    private static bool IsSensitiveVariable(string variableName)
    {
        string upperName = variableName.ToUpperInvariant();
        return SensitiveVariablePatterns.Any(pattern => 
            upperName.Contains(pattern.Replace("*", string.Empty)));
    }
    
    private static string MaskSensitiveValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;
            
        if (value.Length <= 4)
            return new string('*', value.Length);
            
        return value.Substring(0, 2) + new string('*', value.Length - 4) + value.Substring(value.Length - 2);
    }
}
```

#### Safe Process Execution
Process execution requires careful validation and security considerations:

```csharp
public static class SecureProcessExecution
{
    private static readonly HashSet<string> AllowedExecutables = new(StringComparer.OrdinalIgnoreCase)
    {
        "cmd.exe", "powershell.exe", "git.exe", "dotnet.exe", "node.exe"
    };
    
    public static async Task<ProcessExecutionResult> ExecuteSecurelyAsync(
        string fileName, 
        string arguments,
        ProcessSecurityOptions options = null)
    {
        options ??= ProcessSecurityOptions.Default;
        
        // Validate executable
        if (!IsExecutableAllowed(fileName, options))
        {
            throw new UnauthorizedAccessException($"Execution of '{fileName}' is not permitted");
        }
        
        // Sanitize arguments
        string sanitizedArguments = SanitizeArguments(arguments, options);
        
        var processInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = sanitizedArguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = options.WorkingDirectory ?? Environment.CurrentDirectory
        };
        
        // Apply security restrictions
        ApplySecurityRestrictions(processInfo, options);
        
        return await ProcessManager.ExecuteProcessAsync(
            processInfo.FileName,
            processInfo.Arguments,
            processInfo.WorkingDirectory,
            options.TimeoutMs);
    }
    
    private static bool IsExecutableAllowed(string fileName, ProcessSecurityOptions options)
    {
        if (options.AllowAllExecutables)
            return true;
            
        string executableName = Path.GetFileName(fileName);
        return AllowedExecutables.Contains(executableName) || 
               options.AdditionalAllowedExecutables.Contains(executableName);
    }
    
    private static string SanitizeArguments(string arguments, ProcessSecurityOptions options)
    {
        if (string.IsNullOrEmpty(arguments))
            return arguments;
            
        // Remove potentially dangerous characters
        var dangerous = new[] { "|", "&", ";", ">", "<", "$(", "`" };
        string sanitized = arguments;
        
        foreach (string danger in dangerous)
        {
            if (sanitized.Contains(danger) && !options.AllowDangerousCharacters)
            {
                throw new ArgumentException($"Dangerous character sequence '{danger}' not allowed in arguments");
            }
        }
        
        return sanitized;
    }
    
    private static void ApplySecurityRestrictions(
        ProcessStartInfo processInfo, 
        ProcessSecurityOptions options)
    {
        // Apply environment variable restrictions
        if (options.RestrictEnvironmentVariables)
        {
            processInfo.UseShellExecute = false;
            processInfo.Environment.Clear();
            
            // Add only necessary environment variables
            foreach (var envVar in options.AllowedEnvironmentVariables)
            {
                string value = Environment.GetEnvironmentVariable(envVar);
                if (!string.IsNullOrEmpty(value))
                {
                    processInfo.Environment[envVar] = value;
                }
            }
        }
    }
}

public class ProcessSecurityOptions
{
    public static ProcessSecurityOptions Default => new();
    
    public bool AllowAllExecutables { get; set; } = false;
    public HashSet<string> AdditionalAllowedExecutables { get; set; } = new();
    public bool AllowDangerousCharacters { get; set; } = false;
    public bool RestrictEnvironmentVariables { get; set; } = true;
    public HashSet<string> AllowedEnvironmentVariables { get; set; } = new() { "PATH", "TEMP", "TMP" };
    public string WorkingDirectory { get; set; }
    public int TimeoutMs { get; set; } = 30000;
}
```
```csharp
public static class ConsoleHelper
{
    public static void WriteColoredText(string text, ConsoleColor color)
    {
        ConsoleColor original = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = original;
    }
    
    public static void ShowProgressBar(int percentage, int width = 50)
    {
        int filled = (percentage * width) / 100;
        string bar = new string('█', filled) + new string('░', width - filled);
        Console.Write($"\r[{bar}] {percentage}%");
    }
    
    public static string GetSecureInput(string prompt)
    {
        Console.Write(prompt);
        string input = "";
        ConsoleKeyInfo key;
        
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                input += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[0..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        
        Console.WriteLine();
        return input;
    }
    
    public static void ClearLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }
}
## Real-World Application Scenarios

### Enterprise System Monitoring Dashboard

Professional monitoring applications require comprehensive system oversight capabilities. The following implementation demonstrates how utility classes combine to create sophisticated monitoring solutions:

```csharp
public class EnterpriseSystemMonitor
{
    private readonly Timer monitoringTimer;
    private readonly ISystemMetricsCollector metricsCollector;
    private readonly IAlertingService alertingService;
    private readonly CancellationTokenSource cancellationTokenSource;
    
    public EnterpriseSystemMonitor(
        ISystemMetricsCollector metricsCollector,
        IAlertingService alertingService)
    {
        this.metricsCollector = metricsCollector;
        this.alertingService = alertingService;
        this.cancellationTokenSource = new CancellationTokenSource();
        
        // Initialize monitoring with configurable interval
        var monitoringInterval = TimeSpan.FromSeconds(
            ConfigurationManager.GetConfigurationValue("MonitoringIntervalSeconds", 30));
            
        this.monitoringTimer = new Timer(
            CollectAndAnalyzeMetrics, 
            null, 
            TimeSpan.Zero, 
            monitoringInterval);
    }
    
    private async void CollectAndAnalyzeMetrics(object state)
    {
        try
        {
            var metrics = await CollectSystemMetricsAsync();
            await AnalyzeMetricsAndTriggerAlertsAsync(metrics);
            DisplayMetricsToConsole(metrics);
        }
        catch (Exception ex)
        {
            ConsoleTheme.WriteError($"Monitoring cycle failed: {ex.Message}");
        }
    }
    
    private async Task<SystemMetrics> CollectSystemMetricsAsync()
    {
        var tasks = new[]
        {
            Task.Run(() => CollectProcessMetrics()),
            Task.Run(() => CollectEnvironmentMetrics()),
            Task.Run(() => CollectApplicationMetrics())
        };
        
        await Task.WhenAll(tasks);
        
        return new SystemMetrics
        {
            Timestamp = DateTime.UtcNow,
            ProcessMetrics = await tasks[0],
            EnvironmentMetrics = await tasks[1],
            ApplicationMetrics = await tasks[2]
        };
    }
    
    private ProcessMetrics CollectProcessMetrics()
    {
        var processInfos = EfficientSystemMonitoring.GetProcessInfoEfficient().ToList();
        
        return new ProcessMetrics
        {
            TotalProcesses = processInfos.Count,
            TotalMemoryUsage = processInfos.Sum(p => p.WorkingSetMemory),
            HighMemoryProcesses = processInfos
                .Where(p => p.WorkingSetMemory > 100 * 1024 * 1024) // > 100MB
                .OrderByDescending(p => p.WorkingSetMemory)
                .Take(10)
                .ToList(),
            ProcessorTime = processInfos.Sum(p => p.ProcessorTime.TotalMilliseconds)
        };
    }
    
    private EnvironmentMetrics CollectEnvironmentMetrics()
    {
        return new EnvironmentMetrics
        {
            MachineName = Environment.MachineName,
            ProcessorCount = Environment.ProcessorCount,
            OSVersion = Environment.OSVersion.ToString(),
            WorkingSet = Environment.WorkingSet,
            SystemUptime = TimeSpan.FromMilliseconds(Environment.TickCount),
            AvailableDiskSpace = GetAvailableDiskSpace(),
            NetworkConnectivity = TestNetworkConnectivity()
        };
    }
    
    private ApplicationMetrics CollectApplicationMetrics()
    {
        var currentProcess = Process.GetCurrentProcess();
        
        return new ApplicationMetrics
        {
            ApplicationName = Assembly.GetExecutingAssembly().GetName().Name,
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
            StartTime = currentProcess.StartTime,
            RuntimeVersion = Environment.Version.ToString(),
            MemoryUsage = currentProcess.WorkingSet64,
            ThreadCount = currentProcess.Threads.Count,
            HandleCount = currentProcess.HandleCount,
            FeatureFlags = GetActiveFeatureFlags()
        };
    }
    
    private void DisplayMetricsToConsole(SystemMetrics metrics)
    {
        Console.Clear();
        Console.WriteLine("========== ENTERPRISE SYSTEM MONITOR ==========");
        Console.WriteLine($"Last Update: {metrics.Timestamp:yyyy-MM-dd HH:mm:ss UTC}");
        Console.WriteLine();
        
        // Display process metrics
        ConsoleTheme.WriteSection("PROCESS METRICS");
        Console.WriteLine($"Total Processes: {metrics.ProcessMetrics.TotalProcesses:N0}");
        Console.WriteLine($"Total Memory Usage: {metrics.ProcessMetrics.TotalMemoryUsage / (1024.0 * 1024.0 * 1024.0):F2} GB");
        
        // Display environment metrics
        ConsoleTheme.WriteSection("ENVIRONMENT METRICS");
        Console.WriteLine($"Machine: {metrics.EnvironmentMetrics.MachineName}");
        Console.WriteLine($"OS: {metrics.EnvironmentMetrics.OSVersion}");
        Console.WriteLine($"Processors: {metrics.EnvironmentMetrics.ProcessorCount}");
        Console.WriteLine($"Uptime: {metrics.EnvironmentMetrics.SystemUptime:dd\\.hh\\:mm\\:ss}");
        
        // Display application metrics
        ConsoleTheme.WriteSection("APPLICATION METRICS");
        Console.WriteLine($"Application: {metrics.ApplicationMetrics.ApplicationName} v{metrics.ApplicationMetrics.Version}");
        Console.WriteLine($"Runtime: .NET {metrics.ApplicationMetrics.RuntimeVersion}");
        Console.WriteLine($"Memory: {metrics.ApplicationMetrics.MemoryUsage / (1024.0 * 1024.0):F1} MB");
        Console.WriteLine($"Threads: {metrics.ApplicationMetrics.ThreadCount}");
    }
}
```

### Automated Deployment and DevOps Integration

Modern deployment scenarios require sophisticated process management and environment coordination:

```csharp
public class DeploymentOrchestrator
{
    private readonly ILogger logger;
    private readonly DeploymentConfiguration configuration;
    
    public DeploymentOrchestrator(ILogger logger, DeploymentConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
    }
    
    public async Task<DeploymentResult> ExecuteDeploymentAsync(
        string applicationPath, 
        string targetEnvironment)
    {
        var deployment = new DeploymentContext
        {
            ApplicationPath = applicationPath,
            TargetEnvironment = targetEnvironment,
            DeploymentId = Guid.NewGuid(),
            StartTime = DateTime.UtcNow
        };
        
        try
        {
            logger.LogInformation($"Starting deployment {deployment.DeploymentId} to {targetEnvironment}");
            
            // Pre-deployment validation
            await ValidateDeploymentPrerequisitesAsync(deployment);
            
            // Stop existing services
            await StopApplicationServicesAsync(deployment);
            
            // Backup current version
            await CreateBackupAsync(deployment);
            
            // Deploy new version
            await DeployApplicationAsync(deployment);
            
            // Start services
            await StartApplicationServicesAsync(deployment);
            
            // Verify deployment
            await VerifyDeploymentAsync(deployment);
            
            deployment.Status = DeploymentStatus.Successful;
            deployment.EndTime = DateTime.UtcNow;
            
            logger.LogInformation($"Deployment {deployment.DeploymentId} completed successfully");
            
            return new DeploymentResult(deployment);
        }
        catch (Exception ex)
        {
            deployment.Status = DeploymentStatus.Failed;
            deployment.ErrorMessage = ex.Message;
            deployment.EndTime = DateTime.UtcNow;
            
            logger.LogError(ex, $"Deployment {deployment.DeploymentId} failed");
            
            // Attempt rollback
            await AttemptRollbackAsync(deployment);
            
            throw new DeploymentException($"Deployment failed: {ex.Message}", ex, deployment);
        }
    }
    
    private async Task ValidateDeploymentPrerequisitesAsync(DeploymentContext deployment)
    {
        // Validate source application
        if (!Directory.Exists(deployment.ApplicationPath))
        {
            throw new DeploymentException($"Application path does not exist: {deployment.ApplicationPath}");
        }
        
        // Validate target environment
        string targetPath = GetTargetPath(deployment.TargetEnvironment);
        if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
        }
        
        // Check disk space
        var driveInfo = new DriveInfo(Path.GetPathRoot(targetPath));
        long requiredSpace = GetDirectorySize(deployment.ApplicationPath);
        
        if (driveInfo.AvailableFreeSpace < requiredSpace * 2) // 2x for backup
        {
            throw new DeploymentException(
                $"Insufficient disk space. Required: {requiredSpace / (1024 * 1024)} MB, " +
                $"Available: {driveInfo.AvailableFreeSpace / (1024 * 1024)} MB");
        }
        
        // Validate environment configuration
        await ValidateEnvironmentConfigurationAsync(deployment);
    }
    
    private async Task<ProcessExecutionResult> ExecuteDeploymentCommandAsync(
        string command, 
        string arguments,
        DeploymentContext deployment)
    {
        var securityOptions = new ProcessSecurityOptions
        {
            WorkingDirectory = deployment.ApplicationPath,
            TimeoutMs = configuration.CommandTimeoutMs,
            AllowedEnvironmentVariables = new HashSet<string>
            {
                "PATH", "TEMP", "TMP", "DEPLOYMENT_ENV", "APPLICATION_PATH"
            }
        };
        
        // Set deployment-specific environment variables
        Environment.SetEnvironmentVariable("DEPLOYMENT_ENV", deployment.TargetEnvironment);
        Environment.SetEnvironmentVariable("APPLICATION_PATH", deployment.ApplicationPath);
        
        try
        {
            var result = await SecureProcessExecution.ExecuteSecurelyAsync(
                command, arguments, securityOptions);
                
            logger.LogInformation($"Command executed: {command} {arguments}. Exit code: {result.ExitCode}");
            
            if (!result.IsSuccess)
            {
                logger.LogError($"Command failed: {result.ErrorMessage}");
            }
            
            return result;
        }
        finally
        {
            // Clean up environment variables
            Environment.SetEnvironmentVariable("DEPLOYMENT_ENV", null);
            Environment.SetEnvironmentVariable("APPLICATION_PATH", null);
        }
    }
    
    private async Task VerifyDeploymentAsync(DeploymentContext deployment)
    {
        // Health check endpoints
        var healthChecks = configuration.HealthCheckEndpoints;
        var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        
        for (int attempt = 1; attempt <= configuration.HealthCheckRetries; attempt++)
        {
            bool allChecksPass = true;
            
            foreach (string endpoint in healthChecks)
            {
                try
                {
                    var response = await httpClient.GetAsync(endpoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        allChecksPass = false;
                        logger.LogWarning($"Health check failed for {endpoint}: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    allChecksPass = false;
                    logger.LogWarning($"Health check exception for {endpoint}: {ex.Message}");
                }
            }
            
            if (allChecksPass)
            {
                logger.LogInformation("All health checks passed");
                return;
            }
            
            if (attempt < configuration.HealthCheckRetries)
            {
                await Task.Delay(TimeSpan.FromSeconds(configuration.HealthCheckDelaySeconds));
            }
        }
        
        throw new DeploymentException("Health checks failed after all retry attempts");
    }
}
```

### Configuration Management and Environment Adaptation

Enterprise applications require sophisticated configuration management that adapts to different environments:

```csharp
public class ConfigurationOrchestrator
{
    private readonly Dictionary<string, IConfigurationProvider> providers;
    private readonly IConfigurationValidator validator;
    
    public ConfigurationOrchestrator()
    {
        providers = new Dictionary<string, IConfigurationProvider>
        {
            { "Environment", new EnvironmentVariableProvider() },
            { "File", new FileConfigurationProvider() },
            { "Registry", new RegistryConfigurationProvider() },
            { "CommandLine", new CommandLineConfigurationProvider() },
            { "AppContext", new AppContextConfigurationProvider() }
        };
        
        validator = new ConfigurationValidator();
    }
    
    public async Task<ApplicationConfiguration> LoadConfigurationAsync()
    {
        var configuration = new ApplicationConfiguration();
        var errors = new List<string>();
        
        // Load from providers in priority order
        foreach (var provider in providers.Values.OrderBy(p => p.Priority))
        {
            try
            {
                await provider.LoadConfigurationAsync(configuration);
            }
            catch (Exception ex)
            {
                errors.Add($"Provider {provider.GetType().Name} failed: {ex.Message}");
            }
        }
        
        // Validate final configuration
        var validationResult = await validator.ValidateAsync(configuration);
        if (!validationResult.IsValid)
        {
            errors.AddRange(validationResult.Errors);
        }
        
        if (errors.Any())
        {
            throw new ConfigurationException(
                $"Configuration loading failed with {errors.Count} errors:\n" +
                string.Join("\n", errors));
        }
        
        // Apply environment-specific overrides
        ApplyEnvironmentOverrides(configuration);
        
        return configuration;
    }
    
    private void ApplyEnvironmentOverrides(ApplicationConfiguration configuration)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") 
                           ?? Environment.GetEnvironmentVariable("ENVIRONMENT") 
                           ?? "Production";
        
        switch (environment.ToLowerInvariant())
        {
            case "development":
                ApplyDevelopmentOverrides(configuration);
                break;
            case "testing":
                ApplyTestingOverrides(configuration);
                break;
            case "staging":
                ApplyStagingOverrides(configuration);
                break;
            case "production":
                ApplyProductionOverrides(configuration);
                break;
        }
        
        // Set feature flags based on environment
        ConfigureEnvironmentFeatures(environment, configuration);
    }
    
    private void ConfigureEnvironmentFeatures(string environment, ApplicationConfiguration configuration)
    {
        var featureConfig = new Dictionary<string, bool>();
        
        switch (environment.ToLowerInvariant())
        {
            case "development":
                featureConfig["DetailedErrorPages"] = true;
                featureConfig["DeveloperTools"] = true;
                featureConfig["PerformanceCounters"] = true;
                featureConfig["DatabaseSeeding"] = true;
                break;
                
            case "testing":
                featureConfig["DetailedErrorPages"] = true;
                featureConfig["TestDataGeneration"] = true;
                featureConfig["MockServices"] = true;
                break;
                
            case "staging":
                featureConfig["DetailedErrorPages"] = false;
                featureConfig["PerformanceMonitoring"] = true;
                featureConfig["LoadTesting"] = true;
                break;
                
            case "production":
                featureConfig["DetailedErrorPages"] = false;
                featureConfig["PerformanceMonitoring"] = true;
                featureConfig["SecurityAudit"] = true;
                featureConfig["AutoScaling"] = true;
                break;
        }
        
        foreach (var feature in featureConfig)
        {
            AppContext.SetSwitch($"Application.{feature.Key}", feature.Value);
        }
    }
}
```
```csharp
public static class SystemInfo
{
    public static void DisplaySystemDetails()
    {
        Console.WriteLine("=== SYSTEM INFORMATION ===");
        Console.WriteLine($"Machine Name: {Environment.MachineName}");
        Console.WriteLine($"User Name: {Environment.UserName}");
        Console.WriteLine($"Domain: {Environment.UserDomainName}");
        Console.WriteLine($"OS Version: {Environment.OSVersion}");
        Console.WriteLine($"Platform: {Environment.OSVersion.Platform}");
        Console.WriteLine($"Processor Count: {Environment.ProcessorCount}");
        Console.WriteLine($"System Directory: {Environment.SystemDirectory}");
        Console.WriteLine($"Current Directory: {Environment.CurrentDirectory}");
        Console.WriteLine($"Working Set: {Environment.WorkingSet:N0} bytes");
    }
    
    public static void DisplaySpecialFolders()
    {
        var folders = new[]
        {
            Environment.SpecialFolder.Desktop,
            Environment.SpecialFolder.MyDocuments,
            Environment.SpecialFolder.ApplicationData,
            Environment.SpecialFolder.LocalApplicationData,
            Environment.SpecialFolder.ProgramFiles,
            Environment.SpecialFolder.System,
            Environment.SpecialFolder.Windows
        };
        
        Console.WriteLine("\n=== SPECIAL FOLDERS ===");
        foreach (var folder in folders)
        {
            string path = Environment.GetFolderPath(folder);
            Console.WriteLine($"{folder}: {path}");
        }
    }
    
    public static Dictionary<string, string> GetEnvironmentVariables()
    {
        var variables = new Dictionary<string, string>();
        
        foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
        {
            variables[entry.Key.ToString()] = entry.Value?.ToString() ?? "";
        }
        
        return variables;
    }
    
    public static void DisplayImportantEnvironmentVariables()
    {
        var important = new[] { "PATH", "TEMP", "USERNAME", "COMPUTERNAME", "OS" };
        
        Console.WriteLine("\n=== ENVIRONMENT VARIABLES ===");
        foreach (string variable in important)
        {
            string value = Environment.GetEnvironmentVariable(variable);
            Console.WriteLine($"{variable}: {value ?? "Not Set"}");
        }
    }
}
## Professional Development Guidelines

### Code Quality and Maintainability Standards

When implementing utility class functionality in enterprise environments, adherence to professional development standards is crucial for long-term maintainability and team collaboration.

#### Documentation and Code Comments
Professional utility implementations require comprehensive documentation that explains not only what the code does, but why specific approaches were chosen:

```csharp
/// <summary>
/// Provides secure and efficient process execution capabilities with comprehensive
/// error handling, resource management, and security validation.
/// </summary>
/// <remarks>
/// This class implements industry best practices for process management including:
/// - Asynchronous execution to prevent UI blocking
/// - Comprehensive timeout handling to prevent hanging operations
/// - Secure argument validation to prevent injection attacks
/// - Resource pooling to optimize memory usage
/// - Structured logging for operational monitoring
/// 
/// Usage in production environments should consider:
/// - Network latency when setting timeout values
/// - Memory constraints when processing large output streams
/// - Security implications of allowing external process execution
/// </remarks>
public static class EnterpriseProcessManager
{
    // Implementation details...
}
```

#### Error Handling Patterns
Professional applications implement consistent error handling patterns that provide meaningful information for debugging while maintaining security:

```csharp
public static class ErrorHandlingPatterns
{
    /// <summary>
    /// Executes an operation with comprehensive error handling and structured logging
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the operation</typeparam>
    /// <param name="operation">The operation to execute</param>
    /// <param name="operationName">A descriptive name for logging purposes</param>
    /// <param name="logger">The logger instance for recording operation details</param>
    /// <returns>An operation result indicating success or failure with detailed information</returns>
    public static async Task<OperationResult<TResult>> ExecuteWithErrorHandlingAsync<TResult>(
        Func<Task<TResult>> operation,
        string operationName,
        ILogger logger)
    {
        var stopwatch = Stopwatch.StartNew();
        var operationId = Guid.NewGuid();
        
        logger.LogInformation("Starting operation {OperationName} with ID {OperationId}", 
            operationName, operationId);
        
        try
        {
            TResult result = await operation();
            stopwatch.Stop();
            
            logger.LogInformation("Operation {OperationName} completed successfully in {Duration}ms", 
                operationName, stopwatch.ElapsedMilliseconds);
                
            return OperationResult<TResult>.Success(result, stopwatch.Elapsed);
        }
        catch (TimeoutException ex)
        {
            stopwatch.Stop();
            logger.LogWarning("Operation {OperationName} timed out after {Duration}ms: {Error}", 
                operationName, stopwatch.ElapsedMilliseconds, ex.Message);
                
            return OperationResult<TResult>.Timeout(ex.Message, stopwatch.Elapsed);
        }
        catch (SecurityException ex)
        {
            stopwatch.Stop();
            logger.LogError("Security violation in operation {OperationName}: {Error}", 
                operationName, ex.Message);
                
            return OperationResult<TResult>.SecurityViolation(ex.Message, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, "Operation {OperationName} failed after {Duration}ms", 
                operationName, stopwatch.ElapsedMilliseconds);
                
            return OperationResult<TResult>.Failure(ex.Message, stopwatch.Elapsed, ex);
        }
    }
}
```

#### Testing Considerations
Professional utility implementations include comprehensive testing strategies that cover both success and failure scenarios:

```csharp
[TestClass]
public class ProcessManagerTests
{
    [TestMethod]
    public async Task ExecuteProcessAsync_WithValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var processManager = new ProcessManager();
        var testCommand = "echo";
        var testArguments = "Hello World";
        
        // Act
        var result = await processManager.ExecuteProcessAsync(testCommand, testArguments);
        
        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(0, result.ExitCode);
        Assert.IsTrue(result.StandardOutput.Contains("Hello World"));
        Assert.IsTrue(result.ExecutionDuration > TimeSpan.Zero);
    }
    
    [TestMethod]
    public async Task ExecuteProcessAsync_WithTimeout_ReturnsTimeoutResult()
    {
        // Arrange
        var processManager = new ProcessManager();
        var testCommand = "ping";
        var testArguments = "-t localhost"; // Infinite ping
        var shortTimeout = 1000; // 1 second
        
        // Act
        var result = await processManager.ExecuteProcessAsync(testCommand, testArguments, timeoutMs: shortTimeout);
        
        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.TimedOut);
        Assert.IsTrue(result.ExecutionDuration.TotalMilliseconds >= shortTimeout);
    }
    
    [TestMethod]
    public void GetEnvironmentVariable_WithSensitiveData_ReturnsMaskedValue()
    {
        // Arrange
        var originalValue = Environment.GetEnvironmentVariable("TEST_PASSWORD");
        Environment.SetEnvironmentVariable("TEST_PASSWORD", "secret123");
        
        try
        {
            // Act
            var result = SecureEnvironmentManager.GetEnvironmentVariable("TEST_PASSWORD");
            
            // Assert
            Assert.AreNotEqual("secret123", result);
            Assert.IsTrue(result.Contains("***"));
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("TEST_PASSWORD", originalValue);
        }
    }
}
```

### Performance Optimization Guidelines

Professional applications require careful attention to performance characteristics, especially when dealing with system resources and external processes.

#### Memory Management Best Practices
Efficient memory usage is crucial for long-running applications:

```csharp
public static class MemoryOptimizedOperations
{
    private static readonly ObjectPool<StringBuilder> StringBuilderPool = 
        new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());
    
    private static readonly ArrayPool<char> CharArrayPool = ArrayPool<char>.Shared;
    
    /// <summary>
    /// Processes large text streams efficiently using object pooling and streaming operations
    /// </summary>
    public static async Task<string> ProcessLargeTextStreamAsync(Stream inputStream)
    {
        var stringBuilder = StringBuilderPool.Get();
        var buffer = CharArrayPool.Rent(4096);
        
        try
        {
            using var reader = new StreamReader(inputStream);
            
            int bytesRead;
            while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                stringBuilder.Append(buffer, 0, bytesRead);
                
                // Prevent excessive memory usage
                if (stringBuilder.Length > 50 * 1024 * 1024) // 50MB limit
                {
                    throw new InvalidOperationException("Input stream exceeds maximum allowed size");
                }
            }
            
            return stringBuilder.ToString();
        }
        finally
        {
            StringBuilderPool.Return(stringBuilder);
            CharArrayPool.Return(buffer);
        }
    }
}
```

#### Asynchronous Operation Patterns
Modern applications require non-blocking operations that don't compromise user experience:

```csharp
public class AsyncOperationManager
{
    private readonly SemaphoreSlim concurrencyLimiter;
    private readonly CancellationTokenSource shutdownTokenSource;
    
    public AsyncOperationManager(int maxConcurrency = 10)
    {
        concurrencyLimiter = new SemaphoreSlim(maxConcurrency, maxConcurrency);
        shutdownTokenSource = new CancellationTokenSource();
    }
    
    /// <summary>
    /// Executes multiple operations concurrently while respecting resource limits
    /// </summary>
    public async Task<TResult[]> ExecuteConcurrentOperationsAsync<TResult>(
        IEnumerable<Func<CancellationToken, Task<TResult>>> operations,
        CancellationToken cancellationToken = default)
    {
        using var combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, shutdownTokenSource.Token);
        
        var tasks = operations.Select(async operation =>
        {
            await concurrencyLimiter.WaitAsync(combinedTokenSource.Token);
            
            try
            {
                return await operation(combinedTokenSource.Token);
            }
            finally
            {
                concurrencyLimiter.Release();
            }
        });
        
        return await Task.WhenAll(tasks);
    }
    
    public void Dispose()
    {
        shutdownTokenSource.Cancel();
        concurrencyLimiter?.Dispose();
        shutdownTokenSource?.Dispose();
    }
}
```

## Integration with Modern Development Practices

### Dependency Injection and Service Architecture

Modern applications utilize dependency injection patterns that promote testability and maintainability:

```csharp
public interface ISystemResourceManager
{
    Task<SystemResourceInfo> GetResourceInfoAsync();
    Task<ProcessExecutionResult> ExecuteProcessAsync(string command, string arguments);
    Task<Dictionary<string, string>> GetEnvironmentVariablesAsync();
}

public class SystemResourceManager : ISystemResourceManager
{
    private readonly ILogger<SystemResourceManager> logger;
    private readonly IConfiguration configuration;
    private readonly ISecurityValidator securityValidator;
    
    public SystemResourceManager(
        ILogger<SystemResourceManager> logger,
        IConfiguration configuration,
        ISecurityValidator securityValidator)
    {
        this.logger = logger;
        this.configuration = configuration;
        this.securityValidator = securityValidator;
    }
    
    public async Task<SystemResourceInfo> GetResourceInfoAsync()
    {
        logger.LogInformation("Collecting system resource information");
        
        return await Task.Run(() => new SystemResourceInfo
        {
            MachineName = Environment.MachineName,
            ProcessorCount = Environment.ProcessorCount,
            OSVersion = Environment.OSVersion.ToString(),
            WorkingSet = Environment.WorkingSet,
            AvailableMemory = GC.GetTotalMemory(false),
            ApplicationUptime = DateTime.Now - Process.GetCurrentProcess().StartTime
        });
    }
    
    public async Task<ProcessExecutionResult> ExecuteProcessAsync(string command, string arguments)
    {
        // Validate security constraints
        await securityValidator.ValidateProcessExecutionAsync(command, arguments);
        
        // Execute with configured timeout
        int timeoutMs = configuration.GetValue<int>("ProcessExecution:TimeoutMs", 30000);
        
        return await SecureProcessExecution.ExecuteSecurelyAsync(
            command, arguments, new ProcessSecurityOptions { TimeoutMs = timeoutMs });
    }
    
    public async Task<Dictionary<string, string>> GetEnvironmentVariablesAsync()
    {
        return await Task.Run(() => SecureEnvironmentManager.GetFilteredEnvironmentVariables());
    }
}
```

### Configuration and Options Pattern

Professional applications implement the options pattern for configuration management:

```csharp
public class UtilityClassesOptions
{
    public const string SectionName = "UtilityClasses";
    
    public ProcessExecutionOptions ProcessExecution { get; set; } = new();
    public ConsoleOptions Console { get; set; } = new();
    public MonitoringOptions Monitoring { get; set; } = new();
    public SecurityOptions Security { get; set; } = new();
}

public class ProcessExecutionOptions
{
    public int DefaultTimeoutMs { get; set; } = 30000;
    public int MaxConcurrentProcesses { get; set; } = 10;
    public List<string> AllowedExecutables { get; set; } = new();
    public bool EnableDetailedLogging { get; set; } = true;
}

// Service registration in Program.cs or Startup.cs
services.Configure<UtilityClassesOptions>(configuration.GetSection(UtilityClassesOptions.SectionName));
services.AddScoped<ISystemResourceManager, SystemResourceManager>();
```

### Logging and Observability

Professional applications implement comprehensive logging and observability:

```csharp
public static class LoggingExtensions
{
    public static void LogProcessExecution(this ILogger logger, string command, string arguments, TimeSpan duration, int exitCode)
    {
        logger.LogInformation(
            "Process executed: {Command} {Arguments} | Duration: {Duration}ms | Exit Code: {ExitCode}",
            command, arguments, duration.TotalMilliseconds, exitCode);
    }
    
    public static void LogSystemResourceAccess(this ILogger logger, string resourceType, string resourceName, bool success)
    {
        if (success)
        {
            logger.LogInformation("Successfully accessed {ResourceType}: {ResourceName}", resourceType, resourceName);
        }
        else
        {
            logger.LogWarning("Failed to access {ResourceType}: {ResourceName}", resourceType, resourceName);
        }
    }
    
    public static void LogEnvironmentVariableAccess(this ILogger logger, string variableName, bool isSensitive)
    {
        if (isSensitive)
        {
            logger.LogInformation("Accessed sensitive environment variable: {VariableName}", variableName);
        }
        else
        {
            logger.LogDebug("Accessed environment variable: {VariableName}", variableName);
        }
    }
}
```
```csharp
public static class ProcessManager
{
    public static Process StartProcess(string fileName, string arguments = "")
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        
        return Process.Start(processInfo);
    }
    
    public static async Task<ProcessResult> RunProcessAsync(string fileName, string arguments = "")
    {
        using var process = StartProcess(fileName, arguments);
        
        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        
        await process.WaitForExitAsync();
        
        return new ProcessResult
        {
            ExitCode = process.ExitCode,
            Output = await outputTask,
            Error = await errorTask,
            ExecutionTime = process.ExitTime - process.StartTime
        };
    }
    
    public static void MonitorProcesses()
    {
        Console.WriteLine("=== RUNNING PROCESSES ===");
        var processes = Process.GetProcesses()
            .Where(p => !string.IsNullOrEmpty(p.ProcessName))
            .OrderByDescending(p => p.WorkingSet64)
            .Take(10);
        
        Console.WriteLine($"{"Process Name",-20} {"PID",-8} {"Memory (MB)",-12} {"Start Time",-20}");
        Console.WriteLine(new string('-', 70));
        
        foreach (var process in processes)
        {
            try
            {
                double memoryMB = process.WorkingSet64 / (1024.0 * 1024.0);
                string startTime = process.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                
                Console.WriteLine($"{process.ProcessName,-20} {process.Id,-8} {memoryMB,-12:F1} {startTime,-20}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{process.ProcessName,-20} {process.Id,-8} {"Access Denied",-12} {"Unknown",-20}");
            }
        }
    }
    
    public static bool IsProcessRunning(string processName)
    {
        return Process.GetProcessesByName(processName).Length > 0;
    }
}

public record ProcessResult
{
    public int ExitCode { get; init; }
    public string Output { get; init; } = "";
    public string Error { get; init; } = "";
    public TimeSpan ExecutionTime { get; init; }
}
## Summary and Professional Impact

### Technical Competencies Developed

Upon completion of this comprehensive utility classes implementation, developers will have acquired essential technical competencies that are fundamental to professional software development:

**System Integration Expertise**: The ability to create applications that seamlessly interact with operating system resources, external processes, and system services. This expertise is crucial for building enterprise applications that must operate within complex system environments.

**Resource Management Proficiency**: Understanding of proper resource lifecycle management, including memory allocation, process spawning, stream handling, and cleanup operations. These skills prevent memory leaks, resource exhaustion, and system instability in production environments.

**Security-Conscious Development**: Implementation of secure coding practices for system-level operations, including input validation, privilege management, and sensitive data handling. These practices are essential for maintaining security compliance in enterprise environments.

**Performance Optimization Skills**: Knowledge of efficient algorithms, resource pooling, asynchronous operations, and performance monitoring techniques that enable applications to scale effectively under production loads.

**Error Handling and Resilience**: Comprehensive understanding of error handling patterns, graceful degradation strategies, and recovery mechanisms that ensure application stability in adverse conditions.

### Industry Applications and Career Relevance

The utility classes demonstrated in this project form the foundation for numerous critical industry applications:

**DevOps and Site Reliability Engineering**: Modern DevOps practices rely heavily on automation tools that interact with system processes, monitor resource usage, and manage deployment pipelines. The process management and system monitoring capabilities developed in this project are directly applicable to creating sophisticated deployment orchestration systems, performance monitoring dashboards, and automated testing frameworks.

**Enterprise System Integration**: Large organizations require applications that can integrate with existing systems, legacy applications, and third-party tools. The process execution and environment management skills developed here enable developers to create robust integration solutions that can adapt to diverse enterprise environments.

**Cloud and Microservices Architecture**: Modern cloud applications require sophisticated configuration management, resource monitoring, and inter-service communication capabilities. The environment variable handling, feature flag management, and system resource monitoring demonstrated in this project are essential for building cloud-native applications that can scale dynamically and adapt to changing conditions.

**Application Performance Monitoring**: Professional applications require comprehensive monitoring and diagnostics capabilities. The system information gathering and process monitoring techniques demonstrated here are fundamental to creating application performance monitoring solutions, health check systems, and diagnostic tools.

**Security and Compliance**: Enterprise applications must implement robust security measures and maintain compliance with industry regulations. The secure process execution, environment variable handling, and access control patterns demonstrated in this project provide the foundation for building security-conscious applications that meet enterprise security requirements.

### Advanced Technical Concepts Mastered

**Asynchronous Programming Patterns**: Implementation of non-blocking operations using async/await patterns, task coordination, and cancellation token handling that enables responsive user interfaces and efficient resource utilization.

**Cross-Platform Compatibility**: Understanding of platform-specific considerations, environment abstraction, and portable code design that enables applications to run consistently across different operating systems and environments.

**Configuration Management**: Implementation of flexible configuration systems that support environment-specific settings, feature flags, and runtime configuration changes without requiring application restarts.

**Structured Logging and Observability**: Creation of comprehensive logging systems that provide detailed operational insights while maintaining performance and security requirements.

**Test-Driven Development**: Implementation of comprehensive testing strategies that cover both success and failure scenarios, ensuring code reliability and maintainability.

### Professional Development Outcomes

Developers who master these utility class concepts will be well-prepared for senior-level positions that require:

**Technical Leadership**: The ability to design and implement complex system integration solutions, make architectural decisions, and guide technical teams in best practices implementation.

**Problem-Solving Excellence**: Skills in diagnosing complex system issues, implementing robust solutions, and optimizing application performance under real-world constraints.

**Security Expertise**: Understanding of security implications in system-level programming and the ability to implement secure coding practices that protect sensitive data and system resources.

**Operational Excellence**: Knowledge of building applications that are not only functional but also maintainable, monitorable, and scalable in production environments.

The comprehensive nature of this utility classes implementation provides a solid foundation for career advancement in software engineering, system administration, DevOps engineering, and technical leadership roles. The skills developed here are directly applicable to solving real-world business problems and creating value through technology solutions.

These competencies represent essential building blocks for professional software development and are highly valued by employers seeking developers who can create robust, scalable, and secure applications that operate effectively in enterprise environments. The combination of technical depth, practical application, and professional development practices demonstrated in this project prepares developers for success in challenging and rewarding technology careers.
```csharp
public static class AppContextHelper
{
    public static void DisplayContextInformation()
    {
        Console.WriteLine("=== APPLICATION CONTEXT ===");
        Console.WriteLine($"Base Directory: {AppContext.BaseDirectory}");
        Console.WriteLine($"Target Framework: {AppContext.TargetFrameworkName}");
        
        // Display context switches
        DisplayContextSwitches();
        
        // Display context data
        DisplayContextData();
    }
    
    public static void SetContextSwitch(string switchName, bool value)
    {
        AppContext.SetSwitch(switchName, value);
        Console.WriteLine($"Set switch '{switchName}' to {value}");
    }
    
    public static bool TryGetSwitch(string switchName, out bool value)
    {
        return AppContext.TryGetSwitch(switchName, out value);
    }
    
    private static void DisplayContextSwitches()
    {
        Console.WriteLine("\n=== CONTEXT SWITCHES ===");
        var switches = new[] { "Switch.System.Globalization.NoAsyncCurrentCulture", 
                              "Switch.System.Net.DontEnableSchUseStrongCrypto" };
        
        foreach (string switchName in switches)
        {
            if (AppContext.TryGetSwitch(switchName, out bool value))
            {
                Console.WriteLine($"{switchName}: {value}");
            }
            else
            {
                Console.WriteLine($"{switchName}: Not Set");
            }
        }
    }
    
    private static void DisplayContextData()
    {
        Console.WriteLine("\n=== CONTEXT DATA ===");
        Console.WriteLine($"Application Domain: {AppDomain.CurrentDomain.FriendlyName}");
        Console.WriteLine($"Configuration File: {AppDomain.CurrentDomain.SetupInformation.ConfigurationFile}");
    }
}



