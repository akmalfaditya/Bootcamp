# Utility Classes 🛠️

## Learning Objectives

By learning this project, you will:
- **Master Console Operations**: Handle input/output, formatting, colors, and cursor positioning
- **Access Environment Information**: Retrieve system details, environment variables, and runtime data
- **Manage External Processes**: Launch, monitor, and communicate with other applications
- **Handle Application Context**: Work with switches, data directories, and configuration
- **Build Interactive Applications**: Create user-friendly console interfaces
- **Implement System Integration**: Connect your applications with the operating system
- **Apply Diagnostic Techniques**: Gather system information for troubleshooting and monitoring

## Core Concepts

### Console Class - Your Gateway to User Interaction
The Console class provides everything you need for terminal-based user interaction, from simple input/output to advanced formatting and appearance control.

```csharp
// Basic input/output
Console.Write("Enter your name: ");
string name = Console.ReadLine();
Console.WriteLine($"Hello, {name}!");

// Formatted output
Console.WriteLine("Progress: {0:P2}", 0.75);  // Progress: 75.00%
Console.WriteLine($"Temperature: {temp:F1}°C"); // Temperature: 23.5°C
```

### Environment Class - System Information Hub
Access comprehensive system information including hardware details, user context, and environment variables.

```csharp
// System information
Console.WriteLine($"Machine: {Environment.MachineName}");
Console.WriteLine($"OS: {Environment.OSVersion}");
Console.WriteLine($"Processors: {Environment.ProcessorCount}");
Console.WriteLine($"Memory: {Environment.WorkingSet:N0} bytes");

// Environment variables
string path = Environment.GetEnvironmentVariable("PATH");
string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
```

### Process Class - External Application Management
Launch, monitor, and communicate with external processes and applications.

```csharp
// Start a process
Process process = Process.Start("notepad.exe", "sample.txt");

// Monitor process
Console.WriteLine($"Process ID: {process.Id}");
Console.WriteLine($"Started: {process.StartTime}");

// Wait for completion
process.WaitForExit();
Console.WriteLine($"Exit code: {process.ExitCode}");
```

## Key Features

### 1. **Advanced Console Operations**
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
```

### 2. **Environment Information Gathering**
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
```

### 3. **Process Management and Monitoring**
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
```

### 4. **Application Context Management**
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
```

### 5. **Console Redirection and Logging**
```csharp
public static class ConsoleRedirection
{
    public static void RedirectToFile(string filePath, Action action)
    {
        TextWriter originalOut = Console.Out;
        
        try
        {
            using var fileWriter = new StreamWriter(filePath);
            Console.SetOut(fileWriter);
            action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
    
    public static void DualOutput(string filePath, Action action)
    {
        TextWriter originalOut = Console.Out;
        
        try
        {
            using var fileWriter = new StreamWriter(filePath);
            var dualWriter = new DualTextWriter(originalOut, fileWriter);
            Console.SetOut(dualWriter);
            action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}

public class DualTextWriter : TextWriter
{
    private readonly TextWriter writer1;
    private readonly TextWriter writer2;
    
    public DualTextWriter(TextWriter writer1, TextWriter writer2)
    {
        this.writer1 = writer1;
        this.writer2 = writer2;
    }
    
    public override Encoding Encoding => writer1.Encoding;
    
    public override void Write(char value)
    {
        writer1.Write(value);
        writer2.Write(value);
    }
    
    public override void WriteLine(string value)
    {
        writer1.WriteLine(value);
        writer2.WriteLine(value);
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            writer2?.Dispose();
        }
        base.Dispose(disposing);
    }
}
```

## Tips

### Console Best Practices
- **Use colored output sparingly**: Too many colors can be overwhelming and hard to read
- **Handle console resizing**: Check `Console.WindowWidth` and `Console.WindowHeight` before positioning
- **Implement input validation**: Always validate user input and provide clear error messages
- **Consider accessibility**: Provide text alternatives to color-based information

### Environment Variable Security
- **Never log sensitive variables**: Environment variables may contain passwords or API keys
- **Validate paths**: Always check if paths from environment variables exist and are accessible
- **Handle missing variables**: Use default values when environment variables aren't set
- **Use specific folder APIs**: Prefer `Environment.GetFolderPath()` over hardcoded paths

### Process Management Safety
- **Always dispose processes**: Use `using` statements or call `Dispose()` explicitly
- **Handle access denied**: Some processes can't be accessed due to permissions
- **Set timeouts**: Use `WaitForExit(timeout)` to prevent hanging on stuck processes
- **Monitor resource usage**: Launching many processes can consume system resources quickly

## Real-World Applications

### System Monitoring Dashboard
```csharp
public class SystemMonitor
{
    private readonly Timer timer;
    
    public SystemMonitor()
    {
        timer = new Timer(DisplayStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }
    
    private void DisplayStatus(object state)
    {
        Console.Clear();
        Console.WriteLine("=== SYSTEM MONITOR ===");
        Console.WriteLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"Uptime: {TimeSpan.FromMilliseconds(Environment.TickCount)}");
        Console.WriteLine($"Memory: {Environment.WorkingSet / (1024 * 1024)} MB");
        Console.WriteLine($"Processors: {Environment.ProcessorCount}");
        
        // Display top processes
        ProcessManager.MonitorProcesses();
        
        Console.WriteLine("\nPress any key to exit...");
    }
    
    public void Stop()
    {
        timer?.Dispose();
    }
}
```

### Deployment Script Helper
```csharp
public class DeploymentHelper
{
    public async Task<bool> DeployApplicationAsync(string appPath, string targetServer)
    {
        try
        {
            Console.WriteLine($"Starting deployment to {targetServer}...");
            
            // Check if target directory exists
            string targetPath = Environment.GetEnvironmentVariable("DEPLOY_PATH") ?? @"C:\Deploy";
            
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
                ConsoleHelper.WriteColoredText($"Created directory: {targetPath}", ConsoleColor.Green);
            }
            
            // Copy files
            ConsoleHelper.ShowProgressBar(25);
            await CopyApplicationFiles(appPath, targetPath);
            
            // Start service
            ConsoleHelper.ShowProgressBar(75);
            var result = await ProcessManager.RunProcessAsync("sc", $"start MyService");
            
            ConsoleHelper.ShowProgressBar(100);
            ConsoleHelper.WriteColoredText("Deployment completed successfully!", ConsoleColor.Green);
            
            return result.ExitCode == 0;
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteColoredText($"Deployment failed: {ex.Message}", ConsoleColor.Red);
            return false;
        }
    }
    
    private async Task CopyApplicationFiles(string source, string target)
    {
        // Implementation for copying files with progress updates
        await Task.Run(() =>
        {
            var files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
            int processed = 0;
            
            foreach (string file in files)
            {
                string relativePath = Path.GetRelativePath(source, file);
                string targetFile = Path.Combine(target, relativePath);
                
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                File.Copy(file, targetFile, true);
                
                processed++;
                int progress = (processed * 50) / files.Length; // 50% of total progress
                ConsoleHelper.ShowProgressBar(25 + progress);
            }
        });
    }
}
```

### Configuration Manager
```csharp
public class ConfigurationManager
{
    private readonly Dictionary<string, string> settings;
    
    public ConfigurationManager()
    {
        settings = new Dictionary<string, string>();
        LoadFromEnvironment();
        LoadFromFile();
    }
    
    private void LoadFromEnvironment()
    {
        // Load specific environment variables
        var envVars = new[] { "DATABASE_URL", "API_KEY", "LOG_LEVEL" };
        
        foreach (string varName in envVars)
        {
            string value = Environment.GetEnvironmentVariable(varName);
            if (!string.IsNullOrEmpty(value))
            {
                settings[varName] = value;
            }
        }
    }
    
    private void LoadFromFile()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "app.config");
        if (File.Exists(configPath))
        {
            var lines = File.ReadAllLines(configPath);
            foreach (string line in lines)
            {
                if (line.Contains('='))
                {
                    var parts = line.Split('=', 2);
                    settings[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }
    }
    
    public string GetSetting(string key, string defaultValue = "")
    {
        return settings.TryGetValue(key, out string value) ? value : defaultValue;
    }
    
    public void DisplayConfiguration()
    {
        Console.WriteLine("=== APPLICATION CONFIGURATION ===");
        foreach (var setting in settings.OrderBy(s => s.Key))
        {
            // Mask sensitive values
            string displayValue = setting.Key.ToLower().Contains("password") || 
                                 setting.Key.ToLower().Contains("key") 
                                 ? "***MASKED***" 
                                 : setting.Value;
            
            Console.WriteLine($"{setting.Key}: {displayValue}");
        }
    }
}
```


## Impact

Utility class is essential for:

**DevOps and Automation**: System monitoring, deployment scripts, environment management
**System Administration**: Process monitoring, configuration management, diagnostic tools
**Enterprise Applications**: Logging systems, health checks, environment-specific configurations
**Development Tools**: Build systems, testing frameworks, code generation utilities
**Cloud Applications**: Resource monitoring, auto-scaling triggers, deployment pipelines

## Integration with Modern C#

**Global Using Statements (C# 10+)**:
```csharp
global using static System.Console;
global using static System.Environment;

// Now you can use without qualification
WriteLine("Hello World!");
string user = UserName;
```

**File-Scoped Namespaces (C# 10+)**:
```csharp
namespace UtilityClasses;

class SystemHelper
{
    public static void ShowInfo() => WriteLine($"OS: {OSVersion}");
}
```

**Pattern Matching with Environment Data (C# 7+)**:
```csharp
string GetOSCategory(PlatformID platform) => platform switch
{
    PlatformID.Win32NT => "Windows",
    PlatformID.Unix => "Unix/Linux",
    PlatformID.MacOSX => "macOS",
    _ => "Unknown"
};
```

---


