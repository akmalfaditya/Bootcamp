using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UtilityClasses
{
    /// <summary>
    /// Comprehensive demonstration of C# utility classes
    /// This project covers Console, Environment, Process, and AppContext classes
    /// Each class provides essential functionality for system interaction and management
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Utility Classes Demonstration ===");
            Console.WriteLine("This demo shows practical usage of Console, Environment, Process, and AppContext classes");
            Console.WriteLine();

            // Demonstrate each utility class with practical examples
            DemonstrateConsoleClass();
            DemonstrateEnvironmentClass();
            DemonstrateProcessClass();
            DemonstrateAppContextClass();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Console Class Demonstration
        /// Shows input/output operations, formatting, console manipulation, and redirection
        /// The Console class is your primary tool for terminal-based user interaction
        /// </summary>
        static void DemonstrateConsoleClass()
        {
            Console.WriteLine("=== CONSOLE CLASS DEMONSTRATION ===");
            
            // Basic input and output operations
            Console.WriteLine("1. Basic Input/Output Operations:");
            Console.Write("Enter your name: ");
            string userName = Console.ReadLine() ?? "Anonymous";
            Console.WriteLine($"Hello, {userName}! Welcome to the utility classes demo.");
            
            // Demonstrate different input methods
            Console.WriteLine("\n2. Different Input Methods:");
            Console.WriteLine("Press any key to continue...");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); // true = don't display the key
            Console.WriteLine($"You pressed: {keyInfo.Key}");
            
            // Composite formatting - this is how we format output in the real world
            Console.WriteLine("\n3. Composite Formatting:");
            int progress = 75;
            double percentage = 87.5;
            Console.WriteLine("Task progress: {0}% completed", progress);
            Console.WriteLine("Overall completion: {0:F2}%", percentage);
            Console.WriteLine("Status: {0} out of {1} items processed", 15, 20);
            
            // Console appearance and cursor manipulation
            Console.WriteLine("\n4. Console Appearance and Cursor Control:");
            DemonstrateConsoleAppearanceControl();
            
            // Cursor positioning - useful for creating interactive interfaces
            Console.WriteLine("\n5. Advanced Cursor Manipulation:");
            DemonstrateAdvancedCursorControl();
            
            // Console redirection demonstration
            Console.WriteLine("\n6. Console Stream Redirection:");
            DemonstrateConsoleRedirection();
            
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates console output redirection
        /// In real applications, this is used for logging and capturing program output
        /// </summary>
        static void DemonstrateConsoleRedirection()
        {
            // Save the current console output
            TextWriter originalOut = Console.Out;
            
            try
            {
                // Create a temporary file for demonstration
                string tempFile = Path.GetTempFileName();
                
                using (TextWriter fileWriter = File.CreateText(tempFile))
                {
                    // Redirect console output to file
                    Console.SetOut(fileWriter);
                    Console.WriteLine("This message goes to the file, not the console");
                    Console.WriteLine("Timestamp: " + DateTime.Now);
                    Console.WriteLine("This is useful for logging application output");
                }
                
                // Restore original console output
                Console.SetOut(originalOut);
                
                // Read and display what was written to the file
                string fileContent = File.ReadAllText(tempFile);
                Console.WriteLine("Content written to file:");
                Console.WriteLine(fileContent);
                
                // Clean up
                File.Delete(tempFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during redirection demo: {ex.Message}");
            }
        }

        /// <summary>
        /// Environment Class Demonstration
        /// Shows system information retrieval and environment variable access
        /// Essential for applications that need to adapt to different environments
        /// </summary>
        static void DemonstrateEnvironmentClass()
        {
            Console.WriteLine("=== ENVIRONMENT CLASS DEMONSTRATION ===");
            
            // System information - crucial for diagnostics and support
            Console.WriteLine("1. System Information:");
            Console.WriteLine($"Machine Name: {Environment.MachineName}");
            Console.WriteLine($"Operating System: {Environment.OSVersion}");
            Console.WriteLine($"Current User: {Environment.UserName}");
            Console.WriteLine($"System Uptime: {Environment.TickCount} milliseconds");
            Console.WriteLine($"Processor Count: {Environment.ProcessorCount}");
            Console.WriteLine($"Working Set: {Environment.WorkingSet:N0} bytes");
            
            // Framework and runtime information
            Console.WriteLine("\n2. Runtime Information:");
            Console.WriteLine($".NET Version: {Environment.Version}");
            Console.WriteLine($"Is 64-bit OS: {Environment.Is64BitOperatingSystem}");
            Console.WriteLine($"Is 64-bit Process: {Environment.Is64BitProcess}");
            Console.WriteLine($"System Directory: {Environment.SystemDirectory}");
            
            // Special folders - these paths change between different OS versions
            Console.WriteLine("\n3. Special Folders:");
            Console.WriteLine($"User Profile: {Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}");
            Console.WriteLine($"Desktop: {Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}");
            Console.WriteLine($"My Documents: {Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
            Console.WriteLine($"Application Data: {Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}");
            Console.WriteLine($"Program Files: {Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}");
            
            // Environment variables - essential for configuration
            Console.WriteLine("\n4. Environment Variables:");
            Console.WriteLine("Important system environment variables:");
            
            string[] importantVars = { "PATH", "TEMP", "USERNAME", "COMPUTERNAME", "OS" };
            foreach (string varName in importantVars)
            {
                string? value = Environment.GetEnvironmentVariable(varName);
                // Truncate PATH because it's usually very long
                if (varName == "PATH" && value != null && value.Length > 100)
                {
                    value = value.Substring(0, 100) + "... (truncated)";
                }
                Console.WriteLine($"  {varName}: {value ?? "Not found"}");
            }
            
            // Command line arguments
            Console.WriteLine("\n5. Command Line Arguments:");
            string[] commandArgs = Environment.GetCommandLineArgs();
            Console.WriteLine($"Application started with {commandArgs.Length} arguments:");
            for (int i = 0; i < commandArgs.Length; i++)
            {
                Console.WriteLine($"  Arg[{i}]: {commandArgs[i]}");
            }
            
            // Environment manipulation
            Console.WriteLine("\n6. Environment Manipulation:");
            Environment.SetEnvironmentVariable("DEMO_VAR", "This is a demo value");
            string? demoValue = Environment.GetEnvironmentVariable("DEMO_VAR");
            Console.WriteLine($"Set and retrieved demo variable: {demoValue ?? "Failed to retrieve"}");
            
            Console.WriteLine();
        }

        /// <summary>
        /// Process Class Demonstration
        /// Shows process creation, management, and communication
        /// Critical for applications that need to interact with external programs
        /// </summary>
        static void DemonstrateProcessClass()
        {
            Console.WriteLine("=== PROCESS CLASS DEMONSTRATION ===");
            
            // Current process information
            Console.WriteLine("1. Current Process Information:");
            Process currentProcess = Process.GetCurrentProcess();
            Console.WriteLine($"Process Name: {currentProcess.ProcessName}");
            Console.WriteLine($"Process ID: {currentProcess.Id}");
            Console.WriteLine($"Start Time: {currentProcess.StartTime}");
            Console.WriteLine($"Working Set: {currentProcess.WorkingSet64:N0} bytes");
            
            // Running system commands with output capture
            Console.WriteLine("\n2. Running System Commands:");
            RunCommandWithOutput("ipconfig", "/all");
            
            // Running commands with error handling
            Console.WriteLine("\n3. Command Execution with Error Handling:");
            RunCommandWithErrorHandling("dir", "C:\\");
            
            // Process enumeration - useful for monitoring
            Console.WriteLine("\n4. Process Enumeration:");
            ListRunningProcesses();
            
            // Launching applications
            Console.WriteLine("\n5. Launching Applications:");
            LaunchApplicationDemo();
            
            // Advanced process handling demonstration
            Console.WriteLine("\n6. Advanced Process Handling with Proper Stream Management:");
            DemonstrateAdvancedProcessHandling();
            
            Console.WriteLine();
        }

        /// <summary>
        /// Runs a command and captures its output
        /// This pattern is very common in DevOps and automation scenarios
        /// </summary>
        static void RunCommandWithOutput(string command, string arguments)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? process = Process.Start(psi))
                {
                    if (process == null)
                    {
                        Console.WriteLine($"Failed to start process: {command}");
                        return;
                    }

                    // Read output asynchronously to prevent deadlocks
                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();
                    
                    process.WaitForExit();
                    
                    Console.WriteLine($"Command: {command} {arguments}");
                    Console.WriteLine($"Exit Code: {process.ExitCode}");
                    
                    if (!string.IsNullOrEmpty(output))
                    {
                        // Show only first few lines to keep demo manageable
                        string[] lines = output.Split('\n');
                        int linesToShow = Math.Min(5, lines.Length);
                        Console.WriteLine("Output (first 5 lines):");
                        for (int i = 0; i < linesToShow; i++)
                        {
                            Console.WriteLine($"  {lines[i]}");
                        }
                        if (lines.Length > 5)
                        {
                            Console.WriteLine($"  ... and {lines.Length - 5} more lines");
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(errors))
                    {
                        Console.WriteLine($"Errors: {errors}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running command '{command}': {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates command execution with comprehensive error handling
        /// Production applications should always handle process errors gracefully
        /// </summary>
        static void RunCommandWithErrorHandling(string command, string arguments)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command} {arguments}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? process = Process.Start(psi))
                {
                    if (process == null)
                    {
                        Console.WriteLine("Failed to start cmd.exe");
                        return;
                    }

                    // Set up async reading to prevent blocking
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            Console.WriteLine($"  Output: {e.Data}");
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            Console.WriteLine($"  Error: {e.Data}");
                        }
                    };

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Wait for process to complete with timeout
                    bool exited = process.WaitForExit(5000); // 5 second timeout
                    
                    if (!exited)
                    {
                        Console.WriteLine("Process timed out and will be terminated");
                        process.Kill();
                    }
                    else
                    {
                        Console.WriteLine($"Process completed with exit code: {process.ExitCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to execute command: {ex.Message}");
            }
        }

        /// <summary>
        /// Advanced process execution with proper handling of interleaved output and error streams
        /// This is the production-ready pattern mentioned in the material for handling both output and error streams
        /// </summary>
        static (string output, string errors) RunAdvancedProcess(string exePath, string args = "")
        {
            try
            {
                using var process = Process.Start(new ProcessStartInfo(exePath, args)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false, // Essential for stream redirection
                    CreateNoWindow = true
                })!;

                if (process == null)
                {
                    return ("", "Failed to start process");
                }

                var errors = new System.Text.StringBuilder();
                
                // Handle error stream asynchronously to prevent interleaving issues
                process.ErrorDataReceived += (sender, errorArgs) =>
                {
                    if (errorArgs.Data != null) 
                    {
                        lock (errors) // Thread-safe access to StringBuilder
                        {
                            errors.AppendLine(errorArgs.Data);
                        }
                    }
                };
                
                process.BeginErrorReadLine(); // Start asynchronous read for errors

                // Read output synchronously
                string output = process.StandardOutput.ReadToEnd();
                
                process.WaitForExit(); // Wait for the process to exit
                
                return (output, errors.ToString());
            }
            catch (Exception ex)
            {
                return ("", $"Process execution failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates the advanced process pattern with real examples
        /// Shows how to properly handle complex command execution scenarios
        /// </summary>
        static void DemonstrateAdvancedProcessHandling()
        {
            Console.WriteLine("\n6. Advanced Process Handling with Proper Stream Management:");
            
            // Test with a command that produces both output and potentially errors
            Console.WriteLine("Running system information command with advanced handling:");
            
            var (output, errors) = RunAdvancedProcess("cmd.exe", "/c systeminfo");
            
            if (!string.IsNullOrEmpty(output))
            {
                // Show first few lines of output
                string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                int linesToShow = Math.Min(8, lines.Length);
                
                Console.WriteLine("System Information (first 8 lines):");
                for (int i = 0; i < linesToShow; i++)
                {
                    Console.WriteLine($"  {lines[i].Trim()}");
                }
                
                if (lines.Length > 8)
                {
                    Console.WriteLine($"  ... and {lines.Length - 8} more lines");
                }
            }
            
            if (!string.IsNullOrEmpty(errors))
            {
                Console.WriteLine($"Errors encountered: {errors}");
            }
            
            // Demonstrate timeout handling
            Console.WriteLine("\nDemonstrating process timeout handling:");
            DemonstrateProcessTimeout();
        }

        /// <summary>
        /// Shows how to handle process timeouts gracefully
        /// Critical for production applications that can't hang indefinitely
        /// </summary>
        static void DemonstrateProcessTimeout()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c timeout /t 3 >nul", // Command that takes 3 seconds
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? process = Process.Start(psi))
                {
                    if (process == null)
                    {
                        Console.WriteLine("Failed to start timeout demo process");
                        return;
                    }

                    Console.WriteLine("Starting process with 2-second timeout (process needs 3 seconds)...");
                    bool finished = process.WaitForExit(2000); // 2 second timeout
                    
                    if (finished)
                    {
                        Console.WriteLine($"Process completed within timeout. Exit code: {process.ExitCode}");
                    }
                    else
                    {
                        Console.WriteLine("Process exceeded timeout and will be terminated");
                        process.Kill();
                        process.WaitForExit(); // Wait for kill to complete
                        Console.WriteLine("Process terminated successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Timeout demonstration failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Lists currently running processes
        /// Useful for system monitoring and diagnostics
        /// </summary>
        static void ListRunningProcesses()
        {
            try
            {
                Process[] processes = Process.GetProcesses();
                Console.WriteLine($"Found {processes.Length} running processes. Showing first 10:");
                
                // Sort by memory usage and show top 10
                var topProcesses = processes
                    .Where(p => !p.HasExited)
                    .OrderByDescending(p => 
                    {
                        try { return p.WorkingSet64; }
                        catch { return 0; }
                    })
                    .Take(10);

                Console.WriteLine("Top 10 processes by memory usage:");
                Console.WriteLine("PID\tName\t\t\tMemory (MB)");
                Console.WriteLine("".PadRight(50, '-'));

                foreach (Process proc in topProcesses)
                {
                    try
                    {
                        long memoryMB = proc.WorkingSet64 / 1024 / 1024;
                        string name = proc.ProcessName.Length > 15 
                            ? proc.ProcessName.Substring(0, 12) + "..." 
                            : proc.ProcessName;
                        Console.WriteLine($"{proc.Id}\t{name.PadRight(15)}\t\t{memoryMB}");
                    }
                    catch
                    {
                        // Some processes might not be accessible
                        Console.WriteLine($"{proc.Id}\t{proc.ProcessName.PadRight(15)}\t\tN/A");
                    }
                }

                // Clean up
                foreach (Process proc in processes)
                {
                    proc.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing processes: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates launching external applications
        /// Common in applications that need to open files or integrate with other tools
        /// </summary>
        static void LaunchApplicationDemo()
        {
            Console.WriteLine("Application launching demo:");
            Console.WriteLine("Note: In a real scenario, you might launch:");
            Console.WriteLine("- Text editors: Process.Start(\"notepad.exe\", \"filename.txt\")");
            Console.WriteLine("- Web browsers: Process.Start(\"https://www.example.com\")");
            Console.WriteLine("- File explorers: Process.Start(\"explorer.exe\", \"C:\\\\\")");
            Console.WriteLine("- Other applications with specific files");
            
            // Demonstrate launching a simple process (that won't actually open anything disruptive)
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c echo Demo process launched successfully",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? proc = Process.Start(psi))
                {
                    if (proc == null)
                    {
                        Console.WriteLine("Failed to start demo process");
                        return;
                    }

                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    Console.WriteLine($"Demo launch result: {output.Trim()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Launch demo failed: {ex.Message}");
            }
        }

        /// <summary>
        /// AppContext Class Demonstration
        /// Shows runtime information and feature switch management
        /// Essential for modern .NET applications that need configuration flexibility
        /// </summary>
        static void DemonstrateAppContextClass()
        {
            Console.WriteLine("=== APPCONTEXT CLASS DEMONSTRATION ===");
            
            // Basic application context information
            Console.WriteLine("1. Application Context Information:");
            Console.WriteLine($"Base Directory: {AppContext.BaseDirectory}");
            Console.WriteLine($"Target Framework: {AppContext.TargetFrameworkName ?? "Unknown"}");
            
            // Feature switches - this is how you control features at runtime
            Console.WriteLine("\n2. Feature Switch Management:");
            DemonstrateFeatureSwitches();
            
            // Data directory management
            Console.WriteLine("\n3. Data Directory Management:");
            string dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
            Console.WriteLine($"Application data directory: {dataDir}");
            Console.WriteLine($"Directory exists: {Directory.Exists(dataDir)}");
            
            // Configuration and settings scenarios
            Console.WriteLine("\n4. Real-world Usage Scenarios:");
            DemonstrateRealWorldScenarios();
            
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates feature switch functionality
        /// Feature switches are crucial for A/B testing and gradual feature rollouts
        /// </summary>
        static void DemonstrateFeatureSwitches()
        {
            // Set up feature switches for different application features
            AppContext.SetSwitch("MyApp.NewUIEnabled", true);
            AppContext.SetSwitch("MyApp.AdvancedLogging", false);
            AppContext.SetSwitch("MyApp.BetaFeatures", true);
            AppContext.SetSwitch("MyApp.DebugMode", true);
            
            Console.WriteLine("Feature switches configured:");
            
            // Check and act on feature switches
            string[] features = { 
                "MyApp.NewUIEnabled", 
                "MyApp.AdvancedLogging", 
                "MyApp.BetaFeatures", 
                "MyApp.DebugMode",
                "MyApp.NonExistentFeature" 
            };
            
            foreach (string feature in features)
            {
                if (AppContext.TryGetSwitch(feature, out bool isEnabled))
                {
                    Console.WriteLine($"  {feature}: {(isEnabled ? "ENABLED" : "DISABLED")}");
                    
                    // Simulate feature-specific behavior
                    switch (feature)
                    {
                        case "MyApp.NewUIEnabled" when isEnabled:
                            Console.WriteLine("    → Loading new UI components");
                            break;
                        case "MyApp.AdvancedLogging" when isEnabled:
                            Console.WriteLine("    → Enabling detailed logging");
                            break;
                        case "MyApp.BetaFeatures" when isEnabled:
                            Console.WriteLine("    → Unlocking beta functionality");
                            break;
                        case "MyApp.DebugMode" when isEnabled:
                            Console.WriteLine("    → Debug mode active - extra diagnostics available");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"  {feature}: NOT SET (using default behavior)");
                }
            }
        }

        /// <summary>
        /// Shows real-world scenarios where utility classes are essential
        /// These patterns appear frequently in production applications
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("Common real-world scenarios:");
            
            // Scenario 1: Application diagnostics
            Console.WriteLine("\nScenario 1: Application Diagnostics");
            GenerateDiagnosticReport();
            
            // Scenario 2: Configuration management
            Console.WriteLine("\nScenario 2: Environment-based Configuration");
            DemonstrateEnvironmentBasedConfig();
            
            // Scenario 3: External tool integration
            Console.WriteLine("\nScenario 3: External Tool Integration");
            DemonstrateExternalToolIntegration();
        }

        /// <summary>
        /// Generates a comprehensive diagnostic report
        /// Essential for troubleshooting production issues
        /// </summary>
        static void GenerateDiagnosticReport()
        {
            Console.WriteLine("Generating diagnostic report...");
            
            var report = new
            {
                Timestamp = DateTime.Now,
                Application = new
                {
                    BaseDirectory = AppContext.BaseDirectory,
                    Framework = AppContext.TargetFrameworkName,
                    Arguments = Environment.GetCommandLineArgs()
                },
                System = new
                {
                    MachineName = Environment.MachineName,
                    OSVersion = Environment.OSVersion.ToString(),
                    ProcessorCount = Environment.ProcessorCount,
                    Is64Bit = Environment.Is64BitOperatingSystem,
                    UserName = Environment.UserName,
                    WorkingSet = Environment.WorkingSet
                },
                Process = new
                {
                    Id = Process.GetCurrentProcess().Id,
                    Name = Process.GetCurrentProcess().ProcessName,
                    StartTime = Process.GetCurrentProcess().StartTime,
                    Memory = Process.GetCurrentProcess().WorkingSet64
                }
            };
            
            Console.WriteLine("Diagnostic report generated successfully:");
            Console.WriteLine($"  Generated at: {report.Timestamp}");
            Console.WriteLine($"  Running on: {report.System.MachineName} ({report.System.OSVersion})");
            Console.WriteLine($"  User: {report.System.UserName}");
            Console.WriteLine($"  Process: {report.Process.Name} (PID: {report.Process.Id})");
            Console.WriteLine($"  Memory usage: {report.Process.Memory:N0} bytes");
            Console.WriteLine($"  Framework: {report.Application.Framework ?? "Unknown"}");
        }

        /// <summary>
        /// Demonstrates environment-based configuration
        /// Critical for applications that run in different environments (dev, test, prod)
        /// </summary>
        static void DemonstrateEnvironmentBasedConfig()
        {
            // Check for environment-specific settings
            string environment = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Development";
            string dbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "localhost";
            string logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "Information";
            
            Console.WriteLine($"Current environment: {environment}");
            Console.WriteLine($"Database connection: {dbConnection}");
            Console.WriteLine($"Log level: {logLevel}");
            
            // Feature switches based on environment
            bool isDevelopment = environment.Equals("Development", StringComparison.OrdinalIgnoreCase);
            bool isProduction = environment.Equals("Production", StringComparison.OrdinalIgnoreCase);
            
            if (isDevelopment)
            {
                AppContext.SetSwitch("MyApp.DetailedErrors", true);
                AppContext.SetSwitch("MyApp.PerformanceMetrics", true);
                Console.WriteLine("Development mode: Detailed errors and metrics enabled");
            }
            else if (isProduction)
            {
                AppContext.SetSwitch("MyApp.DetailedErrors", false);
                AppContext.SetSwitch("MyApp.PerformanceMetrics", false);
                Console.WriteLine("Production mode: Error details and metrics disabled for security");
            }
        }

        /// <summary>
        /// Demonstrates integration with external tools
        /// Common in build systems, deployment scripts, and automation
        /// </summary>
        static void DemonstrateExternalToolIntegration()
        {
            Console.WriteLine("External tool integration examples:");
            
            // Example 1: Version control integration
            Console.WriteLine("\n1. Version Control Integration:");
            CheckGitRepository();
            
            // Example 2: File system operations
            Console.WriteLine("\n2. File System Operations:");
            DemonstrateFileOperations();
            
            // Example 3: System information gathering
            Console.WriteLine("\n3. System Information Gathering:");
            GatherSystemInfo();
        }

        /// <summary>
        /// Checks if the current directory is a Git repository
        /// Common pattern in build and deployment scripts
        /// </summary>
        static void CheckGitRepository()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "rev-parse --git-dir",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? process = Process.Start(psi))
                {
                    if (process == null)
                    {
                        Console.WriteLine("Failed to start git process");
                        return;
                    }

                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    
                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine("Git repository detected");
                        // Could get branch, commit info, etc.
                    }
                    else
                    {
                        Console.WriteLine("Not a Git repository or Git not available");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Git check failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates file operations using Environment paths
        /// Shows how to work with files in a cross-platform way
        /// </summary>
        static void DemonstrateFileOperations()
        {
            try
            {
                string tempDir = Path.GetTempPath();
                string demoFile = Path.Combine(tempDir, $"demo_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                
                // Write some data
                File.WriteAllText(demoFile, $"Demo file created at {DateTime.Now}\nApplication: {AppContext.BaseDirectory}");
                
                Console.WriteLine($"Created demo file: {demoFile}");
                Console.WriteLine($"File size: {new FileInfo(demoFile).Length} bytes");
                
                // Clean up
                File.Delete(demoFile);
                Console.WriteLine("Demo file cleaned up");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File operation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Gathers comprehensive system information
        /// Useful for system monitoring and capacity planning
        /// </summary>
        static void GatherSystemInfo()
        {
            Console.WriteLine("System Information Summary:");
            Console.WriteLine($"  Platform: {Environment.OSVersion.Platform}");
            Console.WriteLine($"  Architecture: {(Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit")}");
            Console.WriteLine($"  Processors: {Environment.ProcessorCount}");
            Console.WriteLine($"  .NET Version: {Environment.Version}");
            Console.WriteLine($"  Current Directory: {Environment.CurrentDirectory}");
            Console.WriteLine($"  System Directory: {Environment.SystemDirectory}");
            Console.WriteLine($"  Uptime: {TimeSpan.FromMilliseconds(Environment.TickCount):dd\\.hh\\:mm\\:ss}");
            
            // Memory information
            long workingSet = Environment.WorkingSet;
            Console.WriteLine($"  Working Set: {workingSet:N0} bytes ({workingSet / 1024.0 / 1024.0:F1} MB)");
        }

        /// <summary>
        /// Demonstrates console appearance control including colors and window properties
        /// Essential for creating user-friendly console applications
        /// </summary>
        static void DemonstrateConsoleAppearanceControl()
        {
            // Save original settings to restore later
            ConsoleColor originalForeground = Console.ForegroundColor;
            ConsoleColor originalBackground = Console.BackgroundColor;
            
            Console.WriteLine("Console color demonstrations:");
            
            // Different message types with appropriate colors
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SUCCESS: Operation completed successfully");
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: Something went wrong");
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: Proceed with caution");
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("INFO: General information message");
            
            // Background color demonstration
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Highlighted message with background color");
            
            // Reset to original colors
            Console.ForegroundColor = originalForeground;
            Console.BackgroundColor = originalBackground;
            
            // Window properties (be careful with these in production)
            try
            {
                Console.WriteLine($"\nCurrent console window properties:");
                Console.WriteLine($"  Window width: {Console.WindowWidth} characters");
                Console.WriteLine($"  Window height: {Console.WindowHeight} characters");
                Console.WriteLine($"  Buffer width: {Console.BufferWidth} characters");
                Console.WriteLine($"  Buffer height: {Console.BufferHeight} characters");
                Console.WriteLine($"  Largest possible window: {Console.LargestWindowWidth} x {Console.LargestWindowHeight}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Console window properties not available: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrates advanced cursor positioning and manipulation
        /// Useful for creating interactive console interfaces and progress indicators
        /// </summary>
        static void DemonstrateAdvancedCursorControl()
        {
            try
            {
                // Save current position
                int startLeft = Console.CursorLeft;
                int startTop = Console.CursorTop;
                
                Console.WriteLine("Cursor positioning demonstration:");
                
                // Progress bar simulation using cursor manipulation
                Console.Write("Progress: [");
                int progressBarStart = Console.CursorLeft;
                int progressBarTop = Console.CursorTop;
                Console.Write("]   0%");
                
                // Simulate progress updates
                for (int i = 0; i <= 10; i++)
                {
                    // Move cursor to progress bar position
                    Console.SetCursorPosition(progressBarStart, progressBarTop);
                    
                    // Draw progress bar
                    string progress = new string('█', i) + new string('░', 10 - i);
                    Console.Write(progress);
                    
                    // Move cursor to percentage position
                    Console.SetCursorPosition(progressBarStart + 12, progressBarTop);
                    Console.Write($"{i * 10,3}%");
                    
                    // Small delay for visual effect
                    System.Threading.Thread.Sleep(200);
                }
                
                Console.WriteLine(); // Move to next line
                
                // Demonstrate text replacement using cursor positioning
                Console.Write("Status: Initializing");
                int statusLeft = Console.CursorLeft - 12; // Position of "Initializing"
                int statusTop = Console.CursorTop;
                
                System.Threading.Thread.Sleep(500);
                Console.SetCursorPosition(statusLeft, statusTop);
                Console.Write("Processing  "); // Extra spaces to clear old text
                
                System.Threading.Thread.Sleep(500);
                Console.SetCursorPosition(statusLeft, statusTop);
                Console.Write("Complete    ");
                
                Console.WriteLine(); // Move to next line
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cursor manipulation failed: {ex.Message}");
                Console.WriteLine("This might happen in some console environments or when output is redirected");
            }
        }
    }
}
