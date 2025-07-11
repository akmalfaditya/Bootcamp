using System;
using System.Diagnostics;
using System.IO;

namespace Debug_and_Trace_Classes
{
    /// <summary>
    /// Advanced demonstration of Debug and Trace concepts
    /// This class shows enterprise-level logging patterns and best practices
    /// </summary>
    public static class AdvancedDebugTraceDemo
    {
        /// <summary>
        /// Demonstrates the complete range of TraceListener types
        /// This is what you'd use in a real enterprise application
        /// </summary>
        public static void DemonstrateAllListenerTypes()
        {
            Console.WriteLine("=== Advanced Listener Types Demo ===");
            Console.WriteLine("Real-world examples of different TraceListener implementations\n");
            
            // Clear existing listeners to start fresh
            Trace.Listeners.Clear();
            
            // 1. Console Listener - for immediate feedback during development
            var consoleListener = new ConsoleTraceListener();
            consoleListener.Name = "console";
            Trace.Listeners.Add(consoleListener);
            Console.WriteLine("✓ Added Console Listener");
            
            // 2. TextWriter Listener - for file-based logging
            var fileListener = new TextWriterTraceListener("debug-advanced.log", "fileLogger");
            fileListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId | TraceOptions.ThreadId;
            Trace.Listeners.Add(fileListener);
            Console.WriteLine("✓ Added File Listener with timestamps");
            
            // 3. Delimited List Listener - for structured data analysis
            var csvListener = new DelimitedListTraceListener("advanced-trace.csv", "csvLogger");
            csvListener.Delimiter = ",";
            csvListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack;
            Trace.Listeners.Add(csvListener);
            Console.WriteLine("✓ Added CSV Listener for data analysis");
            
            // 4. XML Writer Listener - for structured XML logging
            var xmlListener = new XmlWriterTraceListener("advanced-trace.xml", "xmlLogger");
            Trace.Listeners.Add(xmlListener);
            Console.WriteLine("✓ Added XML Listener for structured output");
            
            // 5. Windows Event Log Listener (Windows only)
            // Note: This requires administrative privileges to create event sources
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    // In production, you'd create the event source during application installation
                    Console.WriteLine("⚠️  Event Log listener requires Windows and admin privileges");
                    Console.WriteLine("   In production, create event sources during application installation");
                    Console.WriteLine("   Example: EventLogTraceListener(\"YourAppName\")");
                    
                    // Note: We're not actually creating the EventLog listener here
                    // because it requires additional NuGet packages and admin privileges
                    // In a real application, you would:
                    // 1. Add the System.Diagnostics.EventLog NuGet package
                    // 2. Create the event source during installation
                    // 3. Use EventLogTraceListener in your code
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️  Could not add Event Log listener: {ex.Message}");
            }
            
            // Test all listeners with different message types
            Console.WriteLine("\n📝 Testing all listeners with sample messages:");
            
            Trace.TraceInformation("Application started - this goes to all listeners");
            Trace.TraceWarning("Configuration file missing - using defaults");
            Trace.TraceError("Database connection failed - retrying");
            
            // Custom category messages
            Trace.WriteLine("Custom performance metric: 245ms", "PERFORMANCE");
            Trace.WriteLine("User authentication successful", "SECURITY");
            
            // Flush to ensure all data is written
            Trace.Flush();
            Console.WriteLine("✅ All listeners tested successfully\n");
        }
        
        /// <summary>
        /// Demonstrates advanced filtering techniques
        /// This shows how to control the volume and types of messages
        /// </summary>
        public static void DemonstrateAdvancedFiltering()
        {
            Console.WriteLine("=== Advanced Filtering Demo ===");
            Console.WriteLine("Control what gets logged where to manage information overload\n");
            
            // Clear and set up filtered listeners
            Trace.Listeners.Clear();
            
            // Console listener - show everything for demo purposes
            var consoleListener = new ConsoleTraceListener();
            consoleListener.Name = "console";
            Trace.Listeners.Add(consoleListener);
            
            // Error-only listener
            var errorListener = new TextWriterTraceListener("errors-only.log", "errorOnly");
            errorListener.Filter = new EventTypeFilter(SourceLevels.Error | SourceLevels.Critical);
            Trace.Listeners.Add(errorListener);
            Console.WriteLine("✓ Added Error-only listener");
            
            // Warning and above listener
            var warningListener = new TextWriterTraceListener("warnings-and-errors.log", "warningAndUp");
            warningListener.Filter = new EventTypeFilter(SourceLevels.Warning | SourceLevels.Error | SourceLevels.Critical);
            Trace.Listeners.Add(warningListener);
            Console.WriteLine("✓ Added Warning+ listener");
            
            // Information and above listener
            var infoListener = new TextWriterTraceListener("info-and-above.log", "infoAndUp");
            infoListener.Filter = new EventTypeFilter(SourceLevels.Information | SourceLevels.Warning | SourceLevels.Error | SourceLevels.Critical);
            Trace.Listeners.Add(infoListener);
            Console.WriteLine("✓ Added Info+ listener");
            
            // Test different message levels
            Console.WriteLine("\n📝 Testing filter behavior:");
            
            // This should only appear in console and info+ logs
            Trace.TraceInformation("Info message - routine application status");
            Console.WriteLine("   → Info message sent");
            
            // This should appear in console, warning+, and info+ logs
            Trace.TraceWarning("Warning message - something needs attention");
            Console.WriteLine("   → Warning message sent");
            
            // This should appear in all logs
            Trace.TraceError("Error message - something went wrong");
            Console.WriteLine("   → Error message sent");
            
            // Flush and check results
            Trace.Flush();
            Console.WriteLine("\n✅ Check the different log files to see filtering in action\n");
        }
        
        /// <summary>
        /// Demonstrates conditional compilation and method elimination
        /// Shows how Debug methods disappear in Release builds
        /// </summary>
        public static void DemonstrateConditionalCompilation()
        {
            Console.WriteLine("=== Conditional Compilation Demo ===");
            Console.WriteLine("Understanding how Debug and Trace methods are compiled\n");
            
            // Show current compilation symbols
            Console.WriteLine("Current compilation symbols:");
            
#if DEBUG
            Console.WriteLine("✅ DEBUG symbol is defined");
            Debug.WriteLine("   [DEBUG] This method call exists in the compiled code");
#else
            Console.WriteLine("❌ DEBUG symbol is not defined");
            // Debug.WriteLine calls would be completely eliminated here
#endif
            
#if TRACE
            Console.WriteLine("✅ TRACE symbol is defined");
            Trace.WriteLine("   [TRACE] This method call exists in the compiled code");
#else
            Console.WriteLine("❌ TRACE symbol is not defined");
            // Trace.WriteLine calls would be completely eliminated here
#endif
            
            // Demonstrate the performance implications
            Console.WriteLine("\n⚡ Performance demonstration:");
            
            int iterations = 100000;
            var stopwatch = Stopwatch.StartNew();
            
            // This loop demonstrates why conditional compilation matters
            for (int i = 0; i < iterations; i++)
            {
                // In Release builds, Debug calls are eliminated entirely
                Debug.WriteLineIf(i % 10000 == 0, $"Debug iteration: {i}");
                
                // In Release builds, Trace calls still exist (if TRACE is defined)
                Trace.WriteLineIf(i % 10000 == 0, $"Trace iteration: {i}");
            }
            
            stopwatch.Stop();
            Console.WriteLine($"   {iterations:N0} iterations completed in {stopwatch.ElapsedMilliseconds}ms");
            
            // Key insight about method elimination
            Console.WriteLine("\n💡 Key insights:");
            Console.WriteLine("   • Debug methods: Completely eliminated in Release builds");
            Console.WriteLine("   • Trace methods: Present in both Debug and Release builds");
            Console.WriteLine("   • Conditional methods have [Conditional] attribute");
            Console.WriteLine("   • Method arguments are NOT evaluated if symbol is undefined");
            
            // Demonstrate argument evaluation bypass
            Console.WriteLine("\n🔬 Argument evaluation test:");
            Debug.WriteLine($"Expensive calculation: {ExpensiveCalculation()}");
            Console.WriteLine("   ↑ ExpensiveCalculation() only runs if DEBUG is defined");
            
            Trace.WriteLine($"Another expensive calculation: {ExpensiveCalculation()}");
            Console.WriteLine("   ↑ ExpensiveCalculation() only runs if TRACE is defined\n");
        }
        
        /// <summary>
        /// Demonstrates assertion best practices
        /// Shows when to use Assert vs Fail and proper error handling
        /// </summary>
        public static void DemonstrateAssertionBestPractices()
        {
            Console.WriteLine("=== Assertion Best Practices Demo ===");
            Console.WriteLine("Understanding when and how to use Debug.Assert and Debug.Fail\n");
            
            // Example 1: Input validation vs assertion
            Console.WriteLine("1. Input validation vs assertion:");
            
            string userInput = "valid_input";
            
            // This is input validation - should throw exceptions
            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("   Input validation: Would throw ArgumentException");
                // throw new ArgumentException("User input cannot be null or empty");
            }
            
            // This is assertion - validates internal logic assumptions
            Debug.Assert(!string.IsNullOrEmpty(userInput), "At this point, userInput should be validated");
            Console.WriteLine("   ✅ Assertion passed: userInput is valid");
            
            // Example 2: Business logic validation
            Console.WriteLine("\n2. Business logic validation:");
            
            double price = 99.99;
            double discount = 0.1; // 10% discount
            double finalPrice = price * (1 - discount);
            
            // Assert business logic constraints
            Debug.Assert(price > 0, "Price must be positive", $"Price: {price}");
            Debug.Assert(discount >= 0 && discount <= 1, "Discount must be between 0 and 1", $"Discount: {discount}");
            Debug.Assert(finalPrice <= price, "Final price cannot exceed original price", $"Final: {finalPrice}, Original: {price}");
            
            Console.WriteLine($"   ✅ Business logic assertions passed: ${price:F2} → ${finalPrice:F2}");
            
            // Example 3: State validation
            Console.WriteLine("\n3. State validation:");
            
            var items = new[] { "apple", "banana", "cherry" };
            
            // Assert expected state
            Debug.Assert(items.Length > 0, "Items array should not be empty at this point");
            Debug.Assert(items.All(item => !string.IsNullOrEmpty(item)), "All items should be valid");
            
            Console.WriteLine($"   ✅ State assertions passed: {items.Length} valid items");
            
            // Example 4: When to use Debug.Fail
            Console.WriteLine("\n4. When to use Debug.Fail:");
            
            string operationType = "READ";
            
            switch (operationType)
            {
                case "READ":
                case "WRITE":
                case "DELETE":
                    Console.WriteLine($"   ✅ Valid operation: {operationType}");
                    break;
                default:
                    // This should never happen if our validation is correct
                    Debug.Fail($"Invalid operation type: {operationType}");
                    Console.WriteLine($"   ❌ Invalid operation: {operationType}");
                    break;
            }
            
            Console.WriteLine("\n💡 Assertion guidelines:");
            Console.WriteLine("   • Use assertions for 'should never happen' scenarios");
            Console.WriteLine("   • Use exceptions for input validation");
            Console.WriteLine("   • Assertions help catch bugs during development");
            Console.WriteLine("   • Debug.Fail for unreachable code paths");
            Console.WriteLine("   • Always include descriptive messages\n");
        }
        
        /// <summary>
        /// Simulates an expensive calculation to demonstrate argument evaluation
        /// This helps show how conditional compilation affects performance
        /// </summary>
        private static int ExpensiveCalculation()
        {
            Console.WriteLine("      🔥 ExpensiveCalculation() is running!");
            
            // Simulate some expensive work
            int result = 0;
            for (int i = 0; i < 1000; i++)
            {
                result += i;
            }
            
            return result;
        }
        
        /// <summary>
        /// Demonstrates proper resource cleanup patterns
        /// Essential for production applications
        /// </summary>
        public static void DemonstrateResourceCleanup()
        {
            Console.WriteLine("=== Resource Cleanup Demo ===");
            Console.WriteLine("Proper patterns for managing TraceListener resources\n");
            
            // Pattern 1: Using try/finally
            Console.WriteLine("1. Try/finally pattern:");
            
            TraceListener? tempListener = null;
            try
            {
                tempListener = new TextWriterTraceListener("temp-log.txt", "tempLogger");
                Trace.Listeners.Add(tempListener);
                
                Trace.TraceInformation("Testing try/finally cleanup pattern");
                Console.WriteLine("   ✅ Listener added and used successfully");
            }
            finally
            {
                if (tempListener != null)
                {
                    Trace.Listeners.Remove(tempListener);
                    tempListener.Close();
                    tempListener.Dispose();
                    Console.WriteLine("   ✅ Listener properly cleaned up in finally block");
                }
            }
            
            // Pattern 2: Using using statement
            Console.WriteLine("\n2. Using statement pattern:");
            
            using (var usingListener = new TextWriterTraceListener("using-log.txt", "usingLogger"))
            {
                Trace.Listeners.Add(usingListener);
                Trace.TraceInformation("Testing using statement cleanup pattern");
                Console.WriteLine("   ✅ Listener added and used successfully");
                
                // Automatic cleanup happens here
                Trace.Listeners.Remove(usingListener);
            }
            Console.WriteLine("   ✅ Listener automatically cleaned up by using statement");
            
            // Pattern 3: Application shutdown cleanup
            Console.WriteLine("\n3. Application shutdown pattern:");
            Console.WriteLine("   // In your application's shutdown code:");
            Console.WriteLine("   try {");
            Console.WriteLine("       Trace.Flush();           // Ensure all data is written");
            Console.WriteLine("       Trace.Close();           // Close all listeners");
            Console.WriteLine("   } catch (Exception ex) {");
            Console.WriteLine("       // Log cleanup errors");
            Console.WriteLine("   }");
            
            // Final flush
            Trace.Flush();
            Console.WriteLine("\n✅ All cleanup patterns demonstrated\n");
        }
    }
}
