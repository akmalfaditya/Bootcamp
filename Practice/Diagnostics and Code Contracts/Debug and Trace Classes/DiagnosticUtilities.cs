using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Debug_and_Trace_Classes
{
    /// <summary>
    /// Custom trace listener that demonstrates advanced logging scenarios
    /// This shows how you can create specialized listeners for different needs
    /// </summary>
    public class CustomDiagnosticListener : TraceListener
    {
        private readonly StreamWriter _writer;
        private readonly string _loggerName;
        private bool _disposed = false;

        public CustomDiagnosticListener(string filename, string name) : base(name)
        {
            _loggerName = name;
            _writer = new StreamWriter(filename, append: true);
            _writer.AutoFlush = true; // Ensure immediate writing
        }        /// <summary>
        /// Write method - handles simple write operations
        /// This is called by Debug.Write() and Trace.Write()
        /// </summary>
        public override void Write(string? message)
        {
            if (_disposed) return;
            
            _writer.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{_loggerName}] {message ?? "null"}");
        }

        /// <summary>
        /// WriteLine method - handles line-based write operations
        /// This is called by Debug.WriteLine() and Trace.WriteLine()
        /// </summary>
        public override void WriteLine(string? message)
        {
            if (_disposed) return;
            
            _writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{_loggerName}] {message ?? "null"}");
        }        /// <summary>
        /// TraceEvent method - handles structured trace events with categories
        /// This gives you more control over how different types of messages are formatted
        /// </summary>
        public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
        {
            if (_disposed) return;

            var sb = new StringBuilder();
            sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
            sb.Append($" [{_loggerName}]");
            sb.Append($" [{eventType.ToString().ToUpper()}]");
            
            if (!string.IsNullOrEmpty(source))
                sb.Append($" [{source}]");
            
            if (id > 0)
                sb.Append($" [ID:{id}]");
            
            sb.Append($" {message ?? "null"}");

            // Add additional context if TraceOutputOptions are set
            if ((TraceOutputOptions & TraceOptions.ProcessId) != 0)
                sb.Append($" [PID:{eventCache?.ProcessId}]");
            
            if ((TraceOutputOptions & TraceOptions.ThreadId) != 0)
                sb.Append($" [TID:{eventCache?.ThreadId}]");
            
            if ((TraceOutputOptions & TraceOptions.Callstack) != 0)
                sb.AppendLine().Append($"Callstack: {eventCache?.Callstack}");

            _writer.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Properly dispose of resources when the listener is no longer needed
        /// This is crucial for preventing resource leaks
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _writer?.Flush();
                _writer?.Close();
                _writer?.Dispose();
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Flush any buffered output
        /// Important for ensuring all log data is written
        /// </summary>
        public override void Flush()
        {
            if (!_disposed)
                _writer?.Flush();
        }
    }

    /// <summary>
    /// Utility class for setting up common trace listener configurations
    /// This demonstrates real-world logging setup patterns
    /// </summary>
    public static class TraceListenerSetup
    {
        /// <summary>
        /// Sets up a comprehensive logging configuration for a production application
        /// This is the kind of setup you'd use in real enterprise applications
        /// </summary>
        public static void ConfigureProductionLogging()
        {
            // Clear any existing listeners to start fresh
            Trace.Listeners.Clear();

            // 1. Console listener for immediate feedback during development
            var consoleListener = new TextWriterTraceListener(Console.Out, "console");
            consoleListener.TraceOutputOptions = TraceOptions.None; // Keep console output clean
            Trace.Listeners.Add(consoleListener);

            // 2. General application log - everything goes here
            var generalLogListener = new TextWriterTraceListener("logs/application.log", "general");
            generalLogListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId;
            Trace.Listeners.Add(generalLogListener);

            // 3. Error-only log - critical issues for quick review
            var errorLogListener = new TextWriterTraceListener("logs/errors.log", "errors");
            errorLogListener.Filter = new EventTypeFilter(SourceLevels.Error);
            errorLogListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack;
            Trace.Listeners.Add(errorLogListener);

            // 4. Performance log - timing and performance metrics
            var perfListener = new CustomDiagnosticListener("logs/performance.log", "performance");
            perfListener.Filter = new EventTypeFilter(SourceLevels.Information);
            perfListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId | TraceOptions.ThreadId;
            Trace.Listeners.Add(perfListener);

            // 5. CSV log for data analysis - structured format for tools like Excel
            var csvListener = new DelimitedListTraceListener("logs/trace-data.csv", "csv");
            csvListener.Delimiter = ",";
            csvListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ThreadId;
            Trace.Listeners.Add(csvListener);

            // Enable auto-flush to ensure real-time logging
            Trace.AutoFlush = true;

            // Log the setup completion
            Trace.TraceInformation("Production logging configuration completed successfully");
        }

        /// <summary>
        /// Sets up a development-friendly logging configuration
        /// More verbose and includes debug information
        /// </summary>
        public static void ConfigureDevelopmentLogging()
        {
            Trace.Listeners.Clear();

            // Console output with detailed information
            var consoleListener = new TextWriterTraceListener(Console.Out, "console");
            consoleListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ThreadId;
            Trace.Listeners.Add(consoleListener);

            // Detailed debug log with full context
            var debugListener = new CustomDiagnosticListener("logs/debug.log", "debug");
            debugListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId | 
                                             TraceOptions.ThreadId | TraceOptions.Callstack;
            Trace.Listeners.Add(debugListener);

            // XML output for structured analysis
            var xmlListener = new XmlWriterTraceListener("logs/trace-output.xml", "xml");
            Trace.Listeners.Add(xmlListener);

            Trace.AutoFlush = true;
            Trace.TraceInformation("Development logging configuration completed");
        }

        /// <summary>
        /// Creates the logs directory if it doesn't exist
        /// Essential for file-based logging to work properly
        /// </summary>
        public static void EnsureLogDirectoryExists()
        {
            string logDirectory = "logs";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
                Console.WriteLine($"Created log directory: {logDirectory}");
            }
        }

        /// <summary>
        /// Demonstrates different filtering options
        /// Shows how to control what gets logged where
        /// </summary>
        public static void DemonstrateAdvancedFiltering()
        {
            Console.WriteLine("\n=== Advanced Filtering Demo ===");

            // Create listeners with different filter levels
            var infoListener = new TextWriterTraceListener("logs/info-only.log", "info");
            infoListener.Filter = new EventTypeFilter(SourceLevels.Information);

            var warningListener = new TextWriterTraceListener("logs/warnings-and-errors.log", "warnings");
            warningListener.Filter = new EventTypeFilter(SourceLevels.Warning | SourceLevels.Error);

            var criticalListener = new TextWriterTraceListener("logs/critical-only.log", "critical");
            criticalListener.Filter = new EventTypeFilter(SourceLevels.Critical);

            Trace.Listeners.Add(infoListener);
            Trace.Listeners.Add(warningListener);
            Trace.Listeners.Add(criticalListener);

            // Test different message levels
            Trace.TraceInformation("This goes to info-only log and general logs");
            Trace.TraceWarning("This goes to warnings-and-errors log and general logs");
            Trace.TraceError("This goes to warnings-and-errors log and general logs");
            
            // Note: Critical level requires using TraceSource for full support
            Console.WriteLine("✓ Filtering demonstration completed - check individual log files");
        }
    }

    /// <summary>
    /// Demonstrates practical debugging scenarios you'll encounter in real applications
    /// These are patterns every developer should know
    /// </summary>
    public static class DebuggingScenarios
    {
        /// <summary>
        /// Demonstrates debugging a data processing pipeline
        /// Shows how to trace data flow through your application
        /// </summary>
        public static void SimulateDataProcessingPipeline()
        {
            Console.WriteLine("\n=== Data Processing Pipeline Debug Demo ===");

            string[] sampleData = { "John,25,Engineer", "Jane,30,Manager", "Bob,35,Developer", "Alice,28,Designer" };

            Trace.TraceInformation("Starting data processing pipeline");
            Debug.WriteLine($"Processing {sampleData.Length} records");

            foreach (var record in sampleData)
            {
                ProcessRecord(record);
            }

            Trace.TraceInformation("Data processing pipeline completed");
        }

        private static void ProcessRecord(string record)
        {
            Debug.WriteLine($"Processing record: {record}");

            string[] parts = record.Split(',');
            
            // Assert that we have the expected data format
            Debug.Assert(parts.Length == 3, $"Expected 3 parts in record, got {parts.Length}", $"Record: {record}");

            if (parts.Length != 3)
            {
                Trace.TraceError($"Invalid record format: {record}");
                return;
            }

            string name = parts[0];
            if (!int.TryParse(parts[1], out int age))
            {
                Trace.TraceWarning($"Invalid age format in record: {record}");
                return;
            }
            string role = parts[2];

            // Validate business rules
            Debug.Assert(age >= 18, "Employee age must be at least 18", $"Age: {age}");
            Debug.Assert(!string.IsNullOrEmpty(name), "Employee name cannot be empty");

            Trace.TraceInformation($"Successfully processed employee: {name}, Age: {age}, Role: {role}");
        }

        /// <summary>
        /// Demonstrates debugging exception scenarios
        /// Shows proper error logging and diagnostic information
        /// </summary>
        public static void SimulateExceptionHandling()        {
            Console.WriteLine("\n=== Exception Handling Debug Demo ===");

            string?[] testCases = { "valid-file.txt", "", null, "missing-file.txt" };

            foreach (var filename in testCases)
            {
                try
                {
                    ProcessFile(filename);
                }
                catch (Exception ex)
                {
                    // Log the full exception details for debugging
                    Trace.TraceError($"Exception in ProcessFile: {ex.GetType().Name}: {ex.Message}");
                    Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    
                    // In production, you might also log to a separate error tracking system
                    Console.WriteLine($"   ❌ Error processing {filename ?? "null"}: {ex.Message}");
                }
            }
        }        private static void ProcessFile(string? filename)
        {
            Debug.WriteLine($"Attempting to process file: {filename ?? "null"}");

            // Validate input parameters with assertions
            // Debug.Assert(!string.IsNullOrEmpty(filename), "Filename cannot be null or empty");
            Debug.WriteLineIf(string.IsNullOrEmpty(filename), "   🔍 Debug note: Filename validation failed");

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("Filename cannot be null or empty", nameof(filename));
            }

            if (filename == "missing-file.txt")
            {
                throw new FileNotFoundException($"File not found: {filename}");
            }

            Trace.TraceInformation($"File processed successfully: {filename}");
        }

        /// <summary>
        /// Demonstrates performance debugging techniques
        /// Essential for identifying bottlenecks in your applications
        /// </summary>
        public static void SimulatePerformanceDebugging()
        {
            Console.WriteLine("\n=== Performance Debugging Demo ===");

            // Simulate different operations with varying performance characteristics
            MeasureOperation("Fast Operation", () => System.Threading.Thread.Sleep(50));
            MeasureOperation("Medium Operation", () => System.Threading.Thread.Sleep(150));
            MeasureOperation("Slow Operation", () => System.Threading.Thread.Sleep(300));
            
            // Simulate a memory-intensive operation
            MeasureMemoryUsage("Memory Test", () => {
                var data = new int[1000000]; // Allocate some memory
                for (int i = 0; i < data.Length; i++)
                    data[i] = i;
            });
        }

        private static void MeasureOperation(string operationName, Action operation)
        {
            var stopwatch = Stopwatch.StartNew();
            
            Debug.WriteLine($"Starting {operationName}");
            Trace.TraceInformation($"Performance test started: {operationName}");

            operation();

            stopwatch.Stop();
            long elapsedMs = stopwatch.ElapsedMilliseconds;

            Trace.TraceInformation($"Performance test completed: {operationName} took {elapsedMs}ms");
            
            // Log performance warnings for slow operations
            if (elapsedMs > 200)
            {
                Trace.TraceWarning($"Slow operation detected: {operationName} took {elapsedMs}ms");
            }

            Console.WriteLine($"   ⏱️  {operationName}: {elapsedMs}ms");
        }

        private static void MeasureMemoryUsage(string operationName, Action operation)
        {
            long memoryBefore = GC.GetTotalMemory(forceFullCollection: true);
            
            Debug.WriteLine($"Memory before {operationName}: {memoryBefore:N0} bytes");

            operation();

            long memoryAfter = GC.GetTotalMemory(forceFullCollection: false);
            long memoryUsed = memoryAfter - memoryBefore;

            Trace.TraceInformation($"Memory usage for {operationName}: {memoryUsed:N0} bytes");
            Console.WriteLine($"   💾 {operationName}: {memoryUsed:N0} bytes allocated");
        }
    }
}
