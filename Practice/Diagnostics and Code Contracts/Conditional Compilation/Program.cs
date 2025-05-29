// Define symbols at the file level - these take priority over project-level definitions
#define DEBUG_MODE
#define TESTING_ENABLED
// #define LEGACY_SUPPORT  // Commented out to show how to disable features

using System;
using System.Diagnostics;
using System.IO;

namespace Conditional_Compilation
{
    /// <summary>
    /// This project demonstrates various conditional compilation techniques in C#
    /// We'll explore preprocessor directives, conditional attributes, and runtime flags
    /// </summary>
    class Program
    {
        // Runtime flag - can be changed during execution
        // This is different from compile-time symbols as it can be modified at runtime
        private static bool EnableRuntimeLogging = true;
        
        // Static configuration that could come from config files or environment variables
        private static readonly bool IsProductionEnvironment = false;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Conditional Compilation and Debugging Demo ===\n");
            
            // Demonstrate basic conditional compilation with preprocessor directives
            DemonstratePreprocessorDirectives();
            
            // Show conditional attributes in action
            DemonstrateConditionalAttributes();
            
            // Compare compile-time vs runtime decision making
            DemonstrateRuntimeFlags();
            
            // Show different API versions using conditional compilation
            DemonstrateApiVersioning();
            
            // Performance measurement example
            DemonstratePerformanceMetrics();
            
            // Advanced conditional compilation patterns
            DemonstrateAdvancedPatterns();
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Shows how preprocessor directives work for including/excluding code blocks
        /// This is decided at compile time and cannot be changed during runtime
        /// </summary>
        static void DemonstratePreprocessorDirectives()
        {
            Console.WriteLine("1. Preprocessor Directives Demo:");
            
            // Basic conditional compilation
            #if DEBUG_MODE
            Console.WriteLine("   ✓ Debug mode is ACTIVE - extra diagnostics enabled");
            Console.WriteLine("   ✓ Memory usage tracking enabled");
            Console.WriteLine("   ✓ Verbose error messages enabled");
            #else
            Console.WriteLine("   ✗ Debug mode is DISABLED - production mode");
            #endif
            
            // Multiple conditions using logical operators
            #if DEBUG_MODE && TESTING_ENABLED
            Console.WriteLine("   ✓ Both DEBUG and TESTING are enabled - running comprehensive checks");
            #elif DEBUG_MODE
            Console.WriteLine("   ✓ Debug only - limited testing features");
            #elif TESTING_ENABLED
            Console.WriteLine("   ✓ Testing only - no debug features");
            #else
            Console.WriteLine("   ✗ No special modes enabled");
            #endif
            
            // Conditional compilation for legacy support
            #if LEGACY_SUPPORT
            Console.WriteLine("   ⚠️  Legacy API support is enabled");
            LegacyApiCall();
            #else
            Console.WriteLine("   ✓ Using modern API implementation");
            ModernApiCall();
            #endif
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates the Conditional attribute which eliminates method calls entirely
        /// when the specified symbol is not defined. This is more elegant than wrapping
        /// every method call in #if/#endif blocks
        /// </summary>
        static void DemonstrateConditionalAttributes()
        {
            Console.WriteLine("2. Conditional Attributes Demo:");
            
            // These method calls will only be included in compilation if their respective symbols are defined
            LogDebugInfo("Starting conditional attributes demonstration");
            LogPerformanceMetric("Method execution time", 150);
            LogDevelopmentInfo("This message only appears in development builds");
            
            // This call will always execute because it doesn't have a conditional attribute
            Console.WriteLine("   ✓ Regular method call always executes");
            
            // Simulate some business logic
            int result = CalculateBusinessValue(10, 20);
            LogDebugInfo($"Business calculation result: {result}");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Shows the difference between compile-time and runtime decision making
        /// Runtime flags are useful when you need flexibility without recompiling
        /// </summary>
        static void DemonstrateRuntimeFlags()
        {
            Console.WriteLine("3. Runtime Flags vs Compile-time Symbols:");
            
            // Runtime decision - can be changed during execution
            if (EnableRuntimeLogging)
            {
                Console.WriteLine("   ✓ Runtime logging is ENABLED");
                Console.WriteLine("   ✓ This can be toggled without recompiling");
            }
            
            // Let's toggle it to show the difference
            Console.WriteLine("   → Toggling runtime flag...");
            EnableRuntimeLogging = !EnableRuntimeLogging;
            
            if (EnableRuntimeLogging)
            {
                Console.WriteLine("   ✓ Runtime logging is now ENABLED");
            }
            else
            {
                Console.WriteLine("   ✗ Runtime logging is now DISABLED");
            }
            
            // Environment-based configuration
            if (IsProductionEnvironment)
            {
                Console.WriteLine("   🚀 Production environment detected - optimizations active");
            }
            else
            {
                Console.WriteLine("   🔧 Development environment - debug features available");
            }
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates how conditional compilation can be used for API versioning
        /// This is useful when migrating between different versions of libraries
        /// </summary>
        static void DemonstrateApiVersioning()
        {
            Console.WriteLine("4. API Versioning with Conditional Compilation:");
            
            #if LEGACY_SUPPORT
            Console.WriteLine("   📜 Using legacy API implementation");
            #else
            Console.WriteLine("   🆕 Using modern API implementation");
            #endif
            
            // Simulate calling different API versions
            var data = GetUserData("john_doe");
            Console.WriteLine($"   📊 Retrieved user data: {data}");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Shows how to implement performance metrics that can be conditionally compiled
        /// This is crucial for production builds where you don't want measurement overhead
        /// </summary>
        static void DemonstratePerformanceMetrics()
        {
            Console.WriteLine("5. Performance Metrics Demo:");
            
            var stopwatch = Stopwatch.StartNew();
            
            // Simulate some work
            System.Threading.Thread.Sleep(100);
            
            stopwatch.Stop();
            
            // This will only be included if PERFORMANCE_METRICS symbol is defined
            LogPerformanceMetric("Simulated work", stopwatch.ElapsedMilliseconds);
            
            Console.WriteLine("   ✓ Work completed (performance metrics may or may not be logged)");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates advanced conditional compilation patterns using a service class
        /// This shows how real-world applications might use these techniques
        /// </summary>
        static void DemonstrateAdvancedPatterns()
        {
            Console.WriteLine("6. Advanced Conditional Compilation Patterns:");
            
            var service = new DataProcessingService();
            
            // Process some sample data - behavior changes based on compilation symbols
            string[] sampleData = { "  Hello  ", "  World  ", "  C#  ", "  Conditional  " };
            service.ProcessData(sampleData);
            
            // Show runtime vs compile-time decisions
            service.CompareDecisionTypes();
            
            // Demonstrate runtime feature flags
            service.DemonstrateRuntimeFlags();
            
            Console.WriteLine();
        }
        
        // =========================
        // CONDITIONAL METHODS
        // =========================
        
        /// <summary>
        /// This method will only be called if DEVELOPMENT symbol is defined
        /// If the symbol is not defined, calls to this method are completely removed from the compiled code
        /// </summary>
        [Conditional("DEVELOPMENT")]
        static void LogDevelopmentInfo(string message)
        {
            string logEntry = $"[DEV {DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine($"   🔧 {logEntry}");
            
            // In a real application, you might write this to a file
            // File.AppendAllText("development.log", logEntry + Environment.NewLine);
        }
        
        /// <summary>
        /// Debug logging that only occurs when DEBUG_MODE is defined
        /// This is perfect for detailed troubleshooting information
        /// </summary>
        [Conditional("DEBUG_MODE")]
        static void LogDebugInfo(string message)
        {
            string logEntry = $"[DEBUG {DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine($"   🐛 {logEntry}");
            
            // Write to debug log file
            try
            {
                File.AppendAllText("debug.log", logEntry + Environment.NewLine);
            }
            catch
            {
                // Ignore file write errors in debug logging
            }
        }
        
        /// <summary>
        /// Performance metrics logging - only included when PERFORMANCE_METRICS is defined
        /// This prevents performance measurement overhead in production builds
        /// </summary>
        [Conditional("PERFORMANCE_METRICS")]
        static void LogPerformanceMetric(string operation, long milliseconds)
        {
            string logEntry = $"[PERF {DateTime.Now:HH:mm:ss}] {operation}: {milliseconds}ms";
            Console.WriteLine($"   ⏱️  {logEntry}");
            
            // In production, you might send this to a monitoring service
            // File.AppendAllText("performance.log", logEntry + Environment.NewLine);
        }
        
        // =========================
        // BUSINESS LOGIC METHODS
        // =========================
        
        /// <summary>
        /// Simulates a business calculation with optional debug logging
        /// </summary>
        static int CalculateBusinessValue(int input1, int input2)
        {
            LogDebugInfo($"Calculating business value for inputs: {input1}, {input2}");
            
            int result = input1 * input2 + 50;
            
            LogDebugInfo($"Business calculation completed with result: {result}");
            return result;
        }
        
        /// <summary>
        /// Simulates different API implementations based on conditional compilation
        /// </summary>
        static string GetUserData(string userId)
        {
            #if LEGACY_SUPPORT
            // Legacy implementation - might use older, slower methods
            LogDebugInfo("Using legacy user data retrieval method");
            return $"Legacy_User_{userId}_Data";
            #else
            // Modern implementation - faster, more efficient
            LogDebugInfo("Using modern user data retrieval method");
            return $"Modern_User_{userId}_Data_Optimized";
            #endif
        }
        
        /// <summary>
        /// Legacy API method - only compiled when LEGACY_SUPPORT is defined
        /// </summary>
        #if LEGACY_SUPPORT
        static void LegacyApiCall()
        {
            Console.WriteLine("   📜 Executing legacy API call (slower, but compatible)");
            LogDebugInfo("Legacy API call executed");
        }
        #endif
        
        /// <summary>
        /// Modern API method - used when LEGACY_SUPPORT is not defined
        /// </summary>
        #if !LEGACY_SUPPORT
        static void ModernApiCall()
        {
            Console.WriteLine("   🚀 Executing modern API call (faster, optimized)");
            LogDebugInfo("Modern API call executed");
        }
        #endif
        
        /// <summary>
        /// Conditional method to demonstrate advanced patterns - included only in DEBUG builds
        /// This method showcases how to use multiple conditional symbols and nesting
        /// </summary>
        [Conditional("DEBUG_MODE")]
        static void ConditionalMethodDemo()
        {
            Console.WriteLine("   ✓ Conditional method demo executed - only in DEBUG_MODE");
            
            #if TESTING_ENABLED
            Console.WriteLine("   ✓ Additional testing features are ENABLED in this build");
            #endif
        }
    }
}
