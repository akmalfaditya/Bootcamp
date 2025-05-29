using System;
using System.Diagnostics;
using System.IO;

namespace Debug_and_Trace_Classes
{
    /// <summary>
    /// This program demonstrates the comprehensive use of Debug and Trace classes in C#
    /// We'll explore different logging methods, assertions, listeners, and output configurations
    /// Think of this as your diagnostic toolkit - essential for any serious development work
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Debug and Trace Classes Comprehensive Demo ===\n");
            
            // Set up our trace listeners first - this is like configuring your logging infrastructure
            SetupTraceListeners();
            
            // Basic logging demonstrations
            DemonstrateBasicLogging();
            
            // Show different trace message levels
            DemonstrateTraceLevels();
            
            // Assert and Fail methods - your safety net for catching bugs
            DemonstrateAssertions();
            
            // Working with custom listeners for different output targets
            DemonstrateCustomListeners();
            
            // Filtering and conditional logging
            DemonstrateFiltering();
            
            // Performance tracking with timestamps
            DemonstratePerformanceTracking();
            
            // Simulate real-world scenarios
            SimulateRealWorldScenarios();
            
            // Demonstrate advanced logging configurations
            DemonstrateAdvancedConfigurations();
            
            // Proper cleanup - always important in production code
            CleanupAndFlush();
            
            Console.WriteLine("\n=== Demo Complete ===");
            Console.WriteLine("Check the generated log files for trace output!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Sets up our initial trace listeners - this is like configuring your logging pipeline
        /// In real applications, you'd typically do this in your startup/configuration code
        /// </summary>
        static void SetupTraceListeners()
        {
            Console.WriteLine("1. Setting up Trace Listeners");
            Console.WriteLine("   Think of listeners as different output channels for your diagnostic messages\n");
            
            // The default listener writes to the debugger output window
            // In Visual Studio, you'd see this in the Output window -> Debug
            Console.WriteLine("   ✓ Default debugger listener is already active");
            
            // Add a console listener so we can see trace messages in the console too
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out, "console"));
            Console.WriteLine("   ✓ Added console listener for immediate feedback");
            
            // Configure some basic options for better trace output
            Trace.AutoFlush = true; // Ensures messages appear immediately
            Console.WriteLine("   ✓ Enabled AutoFlush for real-time output\n");
        }
        
        /// <summary>
        /// Demonstrates the fundamental difference between Debug and Trace
        /// This is crucial to understand - Debug disappears in Release builds, Trace stays
        /// </summary>
        static void DemonstrateBasicLogging()
        {
            Console.WriteLine("2. Basic Debug vs Trace Logging");
            Console.WriteLine("   Debug = Development only, Trace = Development AND Production\n");
            
            int x = 15, y = 10;
            
            // Debug methods - these only work in DEBUG builds
            // In Release mode, these calls are completely removed by the compiler
            Debug.Write("   [DEBUG] Writing x value: ");
            Debug.WriteLine(x);
            Debug.WriteIf(x > y, $"   [DEBUG] Conditional: x ({x}) is greater than y ({y})");
            
            // Trace methods - these work in both DEBUG and RELEASE builds
            // Perfect for production logging where you need ongoing diagnostics
            Trace.Write("   [TRACE] Writing calculation result: ");
            Trace.WriteLine(x * y);
            Trace.WriteIf(x > y, $"   [TRACE] Conditional: x ({x}) is indeed greater than y ({y})");
            
            Console.WriteLine("   💡 Key insight: Debug messages only appear in debug builds");
            Console.WriteLine("       Trace messages appear in both debug and release builds\n");
        }
        
        /// <summary>
        /// Shows the different severity levels available with Trace
        /// This helps categorize your diagnostic messages by importance
        /// </summary>
        static void DemonstrateTraceLevels()
        {
            Console.WriteLine("3. Trace Message Levels (Information, Warning, Error)");
            Console.WriteLine("   Use these to categorize your diagnostic messages by severity\n");
            
            // Information - general diagnostic info
            Trace.TraceInformation("Application started successfully at {0}", DateTime.Now);
            
            // Warning - something's not quite right, but not critical
            Trace.TraceWarning("Configuration file not found, using default settings");
            
            // Error - something went wrong that needs attention
            Trace.TraceError("Failed to connect to database, retrying...");
            
            // You can also use the generic method with your own categories
            Trace.WriteLine("Custom category message", "PERFORMANCE");
            
            Console.WriteLine("   💡 Different levels help filter and prioritize log messages\n");
        }
        
        /// <summary>
        /// Demonstrates Debug.Assert and Debug.Fail - your early warning system
        /// These are essential for catching bugs during development
        /// </summary>
        static void DemonstrateAssertions()
        {
            Console.WriteLine("4. Assertions and Fail Methods");
            Console.WriteLine("   Your early warning system for catching bugs and invalid states\n");            // Let's test some conditions with Assert
            int userAge = 25;
            double accountBalance = 150.75;
            
            // Assert checks a condition - if false, it triggers the debugger
            // This is your safety net for catching assumptions that turn out to be wrong
            Debug.Assert(userAge >= 0, "User age cannot be negative", $"Received age: {userAge}");
            Debug.Assert(accountBalance >= 0, "Account balance cannot be negative");
            Debug.Assert(File.Exists("Program.cs"), "Program.cs should exist in current directory");
            
            // Let's demonstrate what happens with a failing assertion (commented out to avoid breaking)
            // Debug.Assert(userAge > 100, "This will fail and show in debugger");
            
            // Conditional assertions - only check when certain conditions are met
            Debug.WriteLineIf(userAge < 18, "   ⚠️  Warning: User is a minor");
            Debug.WriteLineIf(accountBalance < 100, "   ⚠️  Warning: Low account balance");
            
            Console.WriteLine("   ✓ All assertions passed - no bugs detected here!");
            Console.WriteLine("   💡 Assertions help catch bugs early in development\n");
        }
        
        /// <summary>
        /// Shows how to work with different types of trace listeners
        /// This is where you route your diagnostic messages to different destinations
        /// </summary>
        static void DemonstrateCustomListeners()
        {
            Console.WriteLine("5. Custom Trace Listeners");
            Console.WriteLine("   Route your diagnostic messages to files, event logs, or custom destinations\n");
            
            // Create a file listener - great for persistent logging
            string logFilePath = "application.log";
            var fileListener = new TextWriterTraceListener(logFilePath, "fileLogger");
            fileListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId;
            Trace.Listeners.Add(fileListener);
            
            Console.WriteLine($"   ✓ Added file listener: {logFilePath}");
            
            // Create a delimited trace listener - useful for structured logs
            var csvListener = new DelimitedListTraceListener("trace-data.csv", "csvLogger");
            csvListener.Delimiter = ",";
            csvListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ThreadId;
            Trace.Listeners.Add(csvListener);
            
            Console.WriteLine("   ✓ Added CSV listener for structured logging");
            
            // Test our new listeners with some sample data
            Trace.TraceInformation("Testing file and CSV listeners");
            Trace.TraceWarning("This message goes to console, file, and CSV");
            
            // Create an XML trace listener for structured XML output
            var xmlListener = new XmlWriterTraceListener("trace-data.xml", "xmlLogger");
            Trace.Listeners.Add(xmlListener);
            
            Console.WriteLine("   ✓ Added XML listener for structured XML output");
            Trace.TraceInformation("Testing XML output format");
            
            Console.WriteLine("   💡 Multiple listeners let you send logs to different destinations simultaneously\n");
        }
        
        /// <summary>
        /// Demonstrates trace filtering and conditional output
        /// This helps manage the volume of diagnostic output
        /// </summary>
        static void DemonstrateFiltering()
        {
            Console.WriteLine("6. Trace Filtering and Conditional Output");
            Console.WriteLine("   Control when and what gets logged to avoid information overload\n");
            
            // Set up a listener with filtering
            var filteredListener = new TextWriterTraceListener("filtered.log", "filtered");
            filteredListener.Filter = new EventTypeFilter(SourceLevels.Warning | SourceLevels.Error);
            Trace.Listeners.Add(filteredListener);
            
            Console.WriteLine("   ✓ Added filtered listener (warnings and errors only)");
            
            // Test different message types - only warnings and errors should go to filtered log
            Trace.TraceInformation("This info message won't appear in filtered log");
            Trace.TraceWarning("This warning WILL appear in filtered log");
            Trace.TraceError("This error WILL appear in filtered log");
            
            // Conditional tracing based on runtime conditions
            bool isDebugMode = true;  // In real apps, this might come from config
            bool isPerformanceTest = false;
            
            Trace.WriteLineIf(isDebugMode, "   🔧 Debug mode is enabled - extra diagnostics active");
            Trace.WriteLineIf(isPerformanceTest, "   ⚡ Performance testing mode active");
            
            Console.WriteLine("   💡 Filtering helps focus on the messages that matter most\n");
        }
        
        /// <summary>
        /// Shows how to use trace for performance monitoring
        /// Essential for identifying bottlenecks in your applications
        /// </summary>
        static void DemonstratePerformanceTracking()
        {
            Console.WriteLine("7. Performance Tracking with Trace");
            Console.WriteLine("   Monitor execution times and identify bottlenecks\n");
            
            // Set up a performance-specific listener
            var perfListener = new TextWriterTraceListener("performance.log", "performance");
            perfListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Timestamp;
            Trace.Listeners.Add(perfListener);
            
            // Simulate some operations with timing
            TrackOperationPerformance("Database Connection", () => {
                System.Threading.Thread.Sleep(100); // Simulate DB connection time
            });
            
            TrackOperationPerformance("Data Processing", () => {
                System.Threading.Thread.Sleep(250); // Simulate processing time
            });
            
            TrackOperationPerformance("File I/O Operation", () => {
                System.Threading.Thread.Sleep(50); // Simulate file operation
            });
            
            Console.WriteLine("   💡 Performance tracking helps optimize your application\n");
        }
        
        /// <summary>
        /// Simulates real-world logging scenarios you'd encounter in production
        /// This shows practical applications of Debug and Trace
        /// </summary>
        static void SimulateRealWorldScenarios()
        {
            Console.WriteLine("8. Real-World Scenarios");
            Console.WriteLine("   Practical examples of Debug and Trace in action\n");
            
            // Scenario 1: User authentication
            SimulateUserAuthentication("john.doe@example.com", "correctPassword");
            SimulateUserAuthentication("hacker@evil.com", "wrongPassword");
            
            // Scenario 2: File processing
            SimulateFileProcessing("important-data.txt");
            SimulateFileProcessing("missing-file.txt");
            
            // Scenario 3: API calls
            SimulateApiCall("https://api.example.com/users", 200);
            SimulateApiCall("https://api.example.com/orders", 404);
            SimulateApiCall("https://api.example.com/products", 500);
            
            Console.WriteLine("   💡 Real applications generate rich diagnostic information\n");
        }
        
        /// <summary>
        /// Demonstrates proper cleanup of trace listeners
        /// Important for preventing resource leaks and ensuring all data is written
        /// </summary>
        static void CleanupAndFlush()
        {
            Console.WriteLine("9. Cleanup and Resource Management");
            Console.WriteLine("   Properly closing listeners prevents data loss and resource leaks\n");
            
            Console.WriteLine("   🔄 Flushing all listeners to ensure data is written...");
            Trace.Flush(); // Make sure all buffered data is written
            
            // In a real application, you'd typically do this in a finally block or using statement
            Console.WriteLine("   ✅ All trace data has been flushed to outputs");
            
            // Note: We're not calling Trace.Close() here because it would prevent
            // any further trace output. In a real app, you'd call this during shutdown.
        }
          /// <summary>
        /// Demonstrates advanced logging configurations
        /// This includes things like custom formatting, dynamic log levels, and external configuration
        /// </summary>
        static void DemonstrateAdvancedConfigurations()
        {
            Console.WriteLine("10. Advanced Logging Configurations");
            Console.WriteLine("    Customizing your logging setup for flexibility and control\n");
            
            // Example 1: Dynamic filtering demonstration
            Console.WriteLine("    1. Dynamic filtering with multiple listeners");
            
            // Create a listener that only logs warnings and errors
            var warningListener = new TextWriterTraceListener("warnings-only.log", "warningListener");
            warningListener.Filter = new EventTypeFilter(SourceLevels.Warning | SourceLevels.Error);
            Trace.Listeners.Add(warningListener);
            
            // Create another listener for all information
            var infoListener = new TextWriterTraceListener("all-info.log", "infoListener");
            infoListener.Filter = new EventTypeFilter(SourceLevels.All);
            Trace.Listeners.Add(infoListener);
            
            Console.WriteLine("       ✓ Added warning-only listener and all-info listener");
            
            // Test the different filters
            Trace.TraceInformation("This info message goes only to all-info log");
            Trace.TraceWarning("This warning goes to both warning-only and all-info logs");
            Trace.TraceError("This error goes to both warning-only and all-info logs");
            
            // Example 2: Custom formatting with a delimited listener
            Console.WriteLine("\n    2. Custom formatting with a delimited listener");
            var customCsvListener = new DelimitedListTraceListener("custom-format-log.csv", "customCsvLogger");
            customCsvListener.Delimiter = ";"; // Use semicolon as delimiter
            customCsvListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId;
            Trace.Listeners.Add(customCsvListener);
            
            Trace.TraceInformation("This message goes to the custom CSV listener with semicolon delimiter");
            
            // Example 3: Using our custom diagnostic utilities
            Console.WriteLine("\n    3. Demonstrating advanced diagnostic utilities");
            
            // Set up advanced logging configurations
            TraceListenerSetup.EnsureLogDirectoryExists();
            DebuggingScenarios.SimulateDataProcessingPipeline();
            DebuggingScenarios.SimulateExceptionHandling();
            DebuggingScenarios.SimulatePerformanceDebugging();
            
            Console.WriteLine("    💡 Advanced configurations provide flexibility for complex scenarios\n");
        }
        
        // ==============================
        // HELPER METHODS
        // ==============================
        
        /// <summary>
        /// Helper method to track the performance of operations
        /// This is a common pattern for monitoring execution times
        /// </summary>
        static void TrackOperationPerformance(string operationName, Action operation)
        {
            var stopwatch = Stopwatch.StartNew();
            
            Trace.TraceInformation($"Starting operation: {operationName}");
            
            try
            {
                operation();
                stopwatch.Stop();
                
                Trace.TraceInformation($"Operation '{operationName}' completed in {stopwatch.ElapsedMilliseconds}ms");
                Console.WriteLine($"   ⏱️  {operationName}: {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Trace.TraceError($"Operation '{operationName}' failed after {stopwatch.ElapsedMilliseconds}ms: {ex.Message}");
                Console.WriteLine($"   ❌ {operationName}: Failed after {stopwatch.ElapsedMilliseconds}ms");
            }
        }
        
        /// <summary>
        /// Simulates user authentication with appropriate logging
        /// Shows how you'd trace security-related events
        /// </summary>
        static void SimulateUserAuthentication(string email, string password)
        {
            Debug.WriteLine($"Attempting authentication for user: {email}");
            
            if (password == "correctPassword")
            {
                Trace.TraceInformation($"User authentication successful: {email}");
                Console.WriteLine($"   ✅ Authentication successful: {email}");
            }
            else
            {
                Trace.TraceWarning($"Authentication failed for user: {email}");
                Console.WriteLine($"   ❌ Authentication failed: {email}");
                
                // In production, you might want to track failed login attempts
                Debug.Assert(false, $"Failed login attempt detected for {email}");
            }
        }
        
        /// <summary>
        /// Simulates file processing with error handling and logging
        /// Common scenario in most business applications
        /// </summary>
        static void SimulateFileProcessing(string filename)
        {
            Debug.WriteLine($"Starting file processing: {filename}");
            
            if (filename.Contains("missing"))
            {
                Trace.TraceError($"File not found: {filename}");
                Console.WriteLine($"   📄❌ File processing failed: {filename}");
            }
            else
            {
                Trace.TraceInformation($"File processed successfully: {filename}");
                Console.WriteLine($"   📄✅ File processed: {filename}");
            }
        }
        
        /// <summary>
        /// Simulates API calls with different response codes
        /// Shows how to log external service interactions
        /// </summary>
        static void SimulateApiCall(string url, int statusCode)
        {
            Debug.WriteLine($"Making API call to: {url}");
            
            switch (statusCode)
            {
                case 200:
                    Trace.TraceInformation($"API call successful: {url} (Status: {statusCode})");
                    Console.WriteLine($"   🌐✅ API Success: {url}");
                    break;
                case 404:
                    Trace.TraceWarning($"API endpoint not found: {url} (Status: {statusCode})");
                    Console.WriteLine($"   🌐⚠️  API Not Found: {url}");
                    break;
                case 500:
                    Trace.TraceError($"API server error: {url} (Status: {statusCode})");
                    Console.WriteLine($"   🌐❌ API Error: {url}");
                    break;
                default:
                    Trace.WriteLine($"API call completed with status {statusCode}: {url}", "API");
                    Console.WriteLine($"   🌐❓ API Status {statusCode}: {url}");
                    break;
            }
        }
    }
}
