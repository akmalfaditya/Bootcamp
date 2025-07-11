// File-level symbol definitions - these take priority over project-level definitions
// These symbols are only active within this specific file
#define DEBUG_MODE
#define TESTING_ENABLED
#define LOGGINGMODE
// #define LEGACY_SUPPORT  // Commented out to demonstrate disabling features
// #define V2  // Version 2 features - uncomment to enable new features

using System;
using System.Diagnostics;
using System.IO;

// Conditional type alias - demonstrates switching between different implementations
using TestType =
#if V2
    System.Collections.Generic.Dictionary<string, object>;  // V2 uses Dictionary
#else
    System.Collections.Hashtable;  // V1 uses Hashtable
#endif

namespace Conditional_Compilation
{
    /// <summary>
    /// This project demonstrates comprehensive conditional compilation techniques in C#
    /// We'll explore preprocessor directives, conditional attributes, runtime flags,
    /// and practical applications including type aliasing and version management
    /// </summary>
    class Program
    {
        // Runtime flag - can be changed during execution
        // This demonstrates the difference between compile-time and runtime decisions
        private static bool EnableRuntimeLogging = true;
        
        // Static configuration that could come from config files or environment variables
        private static readonly bool IsProductionEnvironment = false;
        
        // Example of runtime feature flags - these can be toggled without recompilation
        private static bool EnableComplexProcessing = false;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Comprehensive Conditional Compilation Demo ===\n");
            
            // 1. Basic preprocessor directives with logical operators
            DemonstratePreprocessorDirectives();
            
            // 2. Conditional attributes - elegant method call elimination
            DemonstrateConditionalAttributes();
            
            // 3. Runtime flags vs compile-time symbols comparison
            DemonstrateRuntimeVsCompileTime();
            
            // 4. Type aliasing and version management
            DemonstrateTypeAliasing();
            
            // 5. Practical logging scenarios
            DemonstratePracticalLogging();
            
            // 6. Advanced conditional compilation patterns
            DemonstrateAdvancedPatterns();
            
            // 7. Performance impact demonstration
            DemonstratePerformanceImpact();
            
            // 8. Runtime logging with deferred evaluation
            RuntimeLoggingExample.DemonstrateRuntimeLogging();
            
            // 9. Hybrid approach (compile-time + runtime)
            RuntimeLoggingExample.DemonstrateHybridApproach();
            
            Console.WriteLine("\n=== Demo Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Demonstrates preprocessor directives for conditional compilation
        /// Shows how code blocks are included or excluded at compile time
        /// These decisions are made during compilation, not at runtime
        /// </summary>
        static void DemonstratePreprocessorDirectives()
        {
            Console.WriteLine("1. Preprocessor Directives Demo:");
            Console.WriteLine("   (These decisions are made at COMPILE TIME)\n");
            
            // Basic #if directive - simplest form of conditional compilation
            #if DEBUG_MODE
            Console.WriteLine("   ✓ DEBUG_MODE is ACTIVE");
            Console.WriteLine("     - Extra diagnostics enabled");
            Console.WriteLine("     - Memory usage tracking enabled");
            Console.WriteLine("     - Verbose error messages enabled");
            #else
            Console.WriteLine("   ✗ DEBUG_MODE is DISABLED - production mode");
            #endif
            
            // Logical operators with multiple conditions
            // && (AND), || (OR), ! (NOT) can be used to combine symbols
            #if DEBUG_MODE && TESTING_ENABLED
            Console.WriteLine("   ✓ Both DEBUG_MODE AND TESTING_ENABLED are active");
            Console.WriteLine("     - Running comprehensive test suite");
            Console.WriteLine("     - Enhanced debugging output");
            #elif DEBUG_MODE && !TESTING_ENABLED
            Console.WriteLine("   ✓ DEBUG_MODE only (testing disabled)");
            #elif !DEBUG_MODE && TESTING_ENABLED
            Console.WriteLine("   ✓ TESTING_ENABLED only (debug disabled)");
            #else
            Console.WriteLine("   ✗ Neither DEBUG_MODE nor TESTING_ENABLED are active");
            #endif
            
            // Conditional compilation for different feature sets
            #if LEGACY_SUPPORT
            Console.WriteLine("   📜 LEGACY_SUPPORT is enabled");
            Console.WriteLine("     - Backward compatibility features active");
            LegacyApiCall();
            #else
            Console.WriteLine("   🚀 Using MODERN implementation");
            Console.WriteLine("     - Latest features and optimizations");
            ModernApiCall();
            #endif
            
            // Complex conditional logic
            #if (DEBUG_MODE || TESTING_ENABLED) && !LEGACY_SUPPORT
            Console.WriteLine("   🔧 Advanced development mode active");
            Console.WriteLine("     - Modern debugging tools available");
            Console.WriteLine("     - Enhanced testing capabilities");
            #endif
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates the [Conditional] attribute - a more elegant solution than #if/#endif
        /// The [Conditional] attribute eliminates method calls entirely when the symbol is not defined
        /// This is cleaner than wrapping every method call in preprocessor directives
        /// </summary>
        static void DemonstrateConditionalAttributes()
        {
            Console.WriteLine("2. Conditional Attributes Demo:");
            Console.WriteLine("   (Methods with [Conditional] attribute are eliminated if symbol not defined)\n");
            
            // These method calls will only be included in compilation if their respective symbols are defined
            // Notice how clean the code looks - no #if/#endif blocks needed!
            LogStatus("Starting conditional attributes demonstration");
            LogDebugInfo("This is a debug message that may or may not appear");
            LogPerformanceMetric("Demo startup time", 25);
            
            // Example showing the power of conditional attributes
            // These calls look normal but may be completely eliminated from the compiled code
            Console.WriteLine("   📞 Making calls to conditional methods...");
            
            // Expensive operation that we only want to log in certain builds
            string complexData = GetComplexMessageHeaders();
            LogStatus($"Complex data processed: {complexData}");
            
            // The beauty of [Conditional] - arguments are NOT evaluated if symbol not defined
            // This means GetComplexMessageHeaders() won't run if LOGGINGMODE is not defined
            LogStatus(() => "Expensive operation result: " + GetExpensiveComputationResult());
            
            // Simulate some business logic
            int result = CalculateBusinessValue(10, 20);
            LogDebugInfo($"Business calculation result: {result}");
            
            Console.WriteLine("   ✓ Conditional method calls completed");
            Console.WriteLine("     (Some calls may have been eliminated at compile time)\n");
            
            // Show the difference between [Conditional] and regular method calls
            Console.WriteLine("   🔍 [Conditional] vs Regular Methods:");
            Console.WriteLine("     - Regular methods: Always compiled, always called");
            Console.WriteLine("     - [Conditional] methods: Only compiled/called if symbol defined");
            Console.WriteLine("     - Arguments to [Conditional] methods: Not evaluated if symbol not defined");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates the difference between compile-time and runtime decision making
        /// Shows when to use each approach and their respective benefits/drawbacks
        /// </summary>
        static void DemonstrateRuntimeVsCompileTime()
        {
            Console.WriteLine("3. Runtime vs Compile-time Decision Making:");
            Console.WriteLine("   (Understanding when to use each approach)\n");
            
            // COMPILE-TIME DECISIONS (using preprocessor directives)
            Console.WriteLine("   ⚙️  COMPILE-TIME decisions:");
            #if DEBUG_MODE
            Console.WriteLine("     ✓ DEBUG_MODE is enabled (decided at compile time)");
            Console.WriteLine("     ✓ Zero runtime performance cost");
            Console.WriteLine("     ✓ Code is completely eliminated if not needed");
            #else
            Console.WriteLine("     ✗ DEBUG_MODE is disabled (decided at compile time)");
            #endif
            
            // RUNTIME DECISIONS (using boolean variables)
            Console.WriteLine("\n   🏃 RUNTIME decisions:");
            if (EnableRuntimeLogging)
            {
                Console.WriteLine("     ✓ Runtime logging is ENABLED");
                Console.WriteLine("     ✓ Can be changed without recompilation");
                Console.WriteLine("     ✓ Configurable through files/environment variables");
            }
            else
            {
                Console.WriteLine("     ✗ Runtime logging is DISABLED");
            }
            
            // Demonstrate runtime flexibility
            Console.WriteLine("\n   🔄 Demonstrating runtime flexibility:");
            Console.WriteLine($"     Current EnableRuntimeLogging: {EnableRuntimeLogging}");
            
            EnableRuntimeLogging = !EnableRuntimeLogging;
            Console.WriteLine($"     After toggle: {EnableRuntimeLogging}");
            
            EnableRuntimeLogging = !EnableRuntimeLogging; // Toggle back
            Console.WriteLine($"     Toggled back: {EnableRuntimeLogging}");
            
            // Show environment-based decisions
            Console.WriteLine("\n   🌍 Environment-based decisions:");
            if (IsProductionEnvironment)
            {
                Console.WriteLine("     🚀 Production environment - performance optimized");
            }
            else
            {
                Console.WriteLine("     🔧 Development environment - debugging features available");
            }
            
            Console.WriteLine("\n   💡 When to use each approach:");
            Console.WriteLine("     • Compile-time: Performance critical, features won't change");
            Console.WriteLine("     • Runtime: Configuration flexibility, feature toggles");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates how conditional compilation can be used for API versioning
        /// This is useful when migrating between different versions of libraries
        /// </summary>
        /// <summary>
        /// Demonstrates advanced conditional compilation patterns using a service class
        /// This shows how real-world applications might use these techniques
        /// </summary>
        static void DemonstrateAdvancedPatterns()
        {
            Console.WriteLine("7. Advanced Conditional Compilation Patterns:");
            Console.WriteLine("   (Real-world service class example)\n");
            
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
        
        // =========================
        // MISSING DEMONSTRATION METHODS
        // =========================
        
        /// <summary>
        /// Demonstrates type aliasing with conditional compilation
        /// Shows how to switch between different implementations based on version flags
        /// </summary>
        static void DemonstrateTypeAliasing()
        {
            Console.WriteLine("4. Type Aliasing and Version Management:");
            Console.WriteLine("   (Switching between different implementations)\n");
            
            // Create instance using conditional type alias
            var testCollection = new TestType();
            
            #if V2
            Console.WriteLine("   🆕 Using V2 implementation (Dictionary<string, object>)");
            // V2 specific operations
            testCollection["key1"] = "Modern value";
            testCollection["key2"] = 42;
            Console.WriteLine($"     V2 Count: {testCollection.Count}");
            Console.WriteLine($"     V2 Type: {testCollection.GetType().Name}");
            #else
            Console.WriteLine("   📜 Using V1 implementation (Hashtable)");
            // V1 specific operations
            testCollection["key1"] = "Legacy value";
            testCollection["key2"] = 42;
            Console.WriteLine($"     V1 Count: {testCollection.Count}");
            Console.WriteLine($"     V1 Type: {testCollection.GetType().Name}");
            #endif
            
            // Show how conditional compilation enables different namespaces
            Console.WriteLine("\n   📦 Conditional namespace usage:");
            #if V2
            Console.WriteLine("     - System.Collections.Generic namespace active");
            Console.WriteLine("     - Modern collection features available");
            #else
            Console.WriteLine("     - System.Collections namespace active");
            Console.WriteLine("     - Legacy collection compatibility");
            #endif
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates practical logging scenarios with conditional compilation
        /// Shows elegant solutions to common logging challenges
        /// </summary>
        static void DemonstratePracticalLogging()
        {
            Console.WriteLine("5. Practical Logging Scenarios:");
            Console.WriteLine("   (Real-world logging challenges and solutions)\n");
            
            // Problem: We want to log expensive operations, but only in debug builds
            // Solution: Use [Conditional] attribute to eliminate calls completely
            
            Console.WriteLine("   🔍 Testing expensive logging scenarios:");
            
            // This looks like it might be expensive, but if LOGGINGMODE is not defined,
            // the method call AND argument evaluation is completely eliminated
            LogStatus("Starting expensive operation");
            LogStatus(() => "Result of expensive computation: " + GetExpensiveComputationResult());
            
            // Demonstrate the difference between approaches
            Console.WriteLine("\n   ⚖️  Comparing logging approaches:");
            
            // Approach 1: Manual #if blocks (tedious)
            #if LOGGINGMODE
            Console.WriteLine("     📝 Manual #if approach: This message shows");
            #endif
            
            // Approach 2: Runtime flag (always evaluates arguments)
            if (EnableRuntimeLogging)
            {
                Console.WriteLine("     🏃 Runtime flag approach: This message shows");
            }
            
            // Approach 3: [Conditional] attribute (elegant, zero cost when disabled)
            LogStatus("Conditional attribute approach: This message may show");
            
            Console.WriteLine("\n   💡 Best practices:");
            Console.WriteLine("     • Use [Conditional] for debug/trace logging");
            Console.WriteLine("     • Use runtime flags for user-configurable logging");
            Console.WriteLine("     • Avoid expensive operations in log arguments");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates performance impact of different conditional compilation approaches
        /// Shows measurable differences between compile-time and runtime decisions
        /// </summary>
        static void DemonstratePerformanceImpact()
        {
            Console.WriteLine("6. Performance Impact Analysis:");
            Console.WriteLine("   (Measuring the cost of different approaches)\n");
            
            const int iterations = 1000000;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Test 1: Compile-time conditional (zero cost when disabled)
            Console.WriteLine("   🚀 Testing compile-time conditional performance:");
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                #if PERFORMANCE_TEST
                // This code is completely eliminated if PERFORMANCE_TEST is not defined
                var dummy = i * 2;
                #endif
            }
            stopwatch.Stop();
            Console.WriteLine($"     Compile-time conditional: {stopwatch.ElapsedMilliseconds}ms");
            
            // Test 2: Runtime conditional (always has cost)
            Console.WriteLine("\n   🏃 Testing runtime conditional performance:");
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (EnableComplexProcessing)
                {
                    var dummy = i * 2;
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"     Runtime conditional: {stopwatch.ElapsedMilliseconds}ms");
            
            // Test 3: [Conditional] method calls
            Console.WriteLine("\n   📞 Testing [Conditional] method call performance:");
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                LogPerformanceTest($"Iteration {i}");
            }
            stopwatch.Stop();
            Console.WriteLine($"     [Conditional] method calls: {stopwatch.ElapsedMilliseconds}ms");
            
            Console.WriteLine("\n   📊 Performance Summary:");
            Console.WriteLine("     • Compile-time: Zero cost when disabled");
            Console.WriteLine("     • Runtime: Always has condition check cost");
            Console.WriteLine("     • [Conditional]: Zero cost when symbol not defined");
            Console.WriteLine();
        }
        
        // =========================
        // ADDITIONAL CONDITIONAL METHODS
        // =========================
        
        /// <summary>
        /// Practical logging method using [Conditional] attribute
        /// This demonstrates the elegant solution to logging challenges
        /// </summary>
        [Conditional("LOGGINGMODE")]
        static void LogStatus(string message)
        {
            string logEntry = $"[LOG {DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine($"   📝 {logEntry}");
            
            // In real applications, you might write to a file
            try
            {
                File.AppendAllText("application.log", logEntry + Environment.NewLine);
            }
            catch
            {
                // Ignore file write errors in logging
            }
        }
        
        /// <summary>
        /// Overloaded LogStatus method that accepts a function to defer argument evaluation
        /// This prevents expensive operations when logging is disabled
        /// </summary>
        [Conditional("LOGGINGMODE")]
        static void LogStatus(Func<string> messageFactory)
        {
            // The messageFactory is only called if LOGGINGMODE is defined
            string message = messageFactory();
            string logEntry = $"[LOG {DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine($"   📝 {logEntry}");
            
            try
            {
                File.AppendAllText("application.log", logEntry + Environment.NewLine);
            }
            catch
            {
                // Ignore file write errors in logging
            }
        }
        
        /// <summary>
        /// Performance testing method - only included when PERFORMANCE_METRICS is defined
        /// </summary>
        [Conditional("PERFORMANCE_METRICS")]
        static void LogPerformanceTest(string operation)
        {
            // This method call is eliminated entirely if PERFORMANCE_METRICS is not defined
            // No performance impact whatsoever
        }
        
        /// <summary>
        /// Simulates getting complex message headers - expensive operation
        /// Used to demonstrate argument evaluation with conditional methods
        /// </summary>
        static string GetComplexMessageHeaders()
        {
            // Simulate expensive operation
            System.Threading.Thread.Sleep(10);
            return $"Complex-Headers-{DateTime.Now.Ticks}";
        }
        
        /// <summary>
        /// Simulates an expensive computation
        /// Used to demonstrate deferred evaluation with Func parameters
        /// </summary>
        static string GetExpensiveComputationResult()
        {
            // Simulate expensive computation
            System.Threading.Thread.Sleep(5);
            var result = DateTime.Now.Ticks % 1000;
            return $"Computed-{result}";
        }
    }
}
