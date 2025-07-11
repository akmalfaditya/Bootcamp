using System;
using System.IO;
using System.Diagnostics;

namespace Conditional_Compilation
{
    /// <summary>
    /// Demonstrates runtime logging control with deferred argument evaluation
    /// This example shows how to avoid expensive operations when logging is disabled
    /// Based on the material about alternatives to [Conditional] attribute
    /// </summary>
    public class RuntimeLoggingExample
    {
        // Runtime flag - can be toggled without recompilation
        // This is different from compile-time symbols
        public static bool EnableLogging = true;
        
        /// <summary>
        /// Traditional logging method - arguments are always evaluated
        /// This can be expensive if the arguments involve complex operations
        /// </summary>
        public static void LogStatusTraditional(string message)
        {
            if (EnableLogging)
            {
                string logEntry = $"[TRADITIONAL {DateTime.Now:HH:mm:ss}] {message}";
                Console.WriteLine($"   📝 {logEntry}");
                File.AppendAllText("runtime.log", logEntry + Environment.NewLine);
            }
        }
        
        /// <summary>
        /// Elegant runtime logging with deferred argument evaluation
        /// Uses Func<T> delegate to avoid expensive operations when logging is disabled
        /// This is the solution suggested in the material for runtime control
        /// </summary>
        public static void LogStatus(Func<string> messageFactory)
        {
            if (EnableLogging)
            {
                // The messageFactory() is only invoked if logging is enabled
                // This means expensive operations in the message are avoided when logging is off
                string message = messageFactory();
                string logEntry = $"[RUNTIME {DateTime.Now:HH:mm:ss}] {message}";
                Console.WriteLine($"   📝 {logEntry}");
                File.AppendAllText("runtime.log", logEntry + Environment.NewLine);
            }
        }
        
        /// <summary>
        /// Demonstrates the difference between traditional and deferred evaluation approaches
        /// </summary>
        public static void DemonstrateRuntimeLogging()
        {
            Console.WriteLine("8. Runtime Logging with Deferred Evaluation:");
            Console.WriteLine("   (Alternative to [Conditional] for runtime control)\n");
            
            // Enable logging first
            EnableLogging = true;
            Console.WriteLine("   ✅ Logging ENABLED - both approaches will log");
            
            // Traditional approach - argument always evaluated
            LogStatusTraditional("Traditional logging: " + GetExpensiveData());
            
            // Deferred approach - argument only evaluated if logging enabled
            LogStatus(() => "Deferred logging: " + GetExpensiveData());
            
            Console.WriteLine();
            
            // Disable logging
            EnableLogging = false;
            Console.WriteLine("   ❌ Logging DISABLED - watch the difference");
            
            // Traditional approach - GetExpensiveData() still runs!
            Console.WriteLine("     Traditional approach: GetExpensiveData() will still run");
            LogStatusTraditional("Traditional logging: " + GetExpensiveData());
            
            // Deferred approach - GetExpensiveData() won't run!
            Console.WriteLine("     Deferred approach: GetExpensiveData() won't run");
            LogStatus(() => "Deferred logging: " + GetExpensiveData());
            
            Console.WriteLine();
            
            // Re-enable for cleanup
            EnableLogging = true;
            
            Console.WriteLine("   💡 Key Benefits of Func<T> approach:");
            Console.WriteLine("     • Runtime configurability (no recompilation needed)");
            Console.WriteLine("     • Deferred evaluation (expensive operations avoided)");
            Console.WriteLine("     • Clean syntax with lambda expressions");
            Console.WriteLine("     • Perfect for user-configurable logging levels");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Simulates an expensive operation that we don't want to run unnecessarily
        /// This demonstrates why deferred evaluation is important
        /// </summary>
        private static string GetExpensiveData()
        {
            Console.WriteLine("       🔄 GetExpensiveData() is running (this should be avoided when logging is off)");
            
            // Simulate expensive operation
            System.Threading.Thread.Sleep(50);
            
            // Simulate complex computation
            var result = DateTime.Now.Ticks % 10000;
            return $"ExpensiveResult-{result}";
        }
        
        /// <summary>
        /// Demonstrates combining compile-time and runtime approaches
        /// Shows how to use both techniques together for maximum flexibility
        /// </summary>
        public static void DemonstrateHybridApproach()
        {
            Console.WriteLine("9. Hybrid Approach (Compile-time + Runtime):");
            Console.WriteLine("   (Combining the best of both worlds)\n");
            
            // Compile-time decision for debug features
            #if DEBUG_MODE
            Console.WriteLine("   🔧 Debug mode features are compiled in");
            
            // Runtime decision for logging level
            if (EnableLogging)
            {
                Console.WriteLine("   📝 Runtime logging is enabled");
                LogDebugDetails();
            }
            else
            {
                Console.WriteLine("   📝 Runtime logging is disabled");
            }
            #else
            Console.WriteLine("   🚀 Production mode - debug features excluded at compile time");
            #endif
            
            Console.WriteLine("\n   🎯 Best Practice Guidelines:");
            Console.WriteLine("     • Use compile-time for features that won't change");
            Console.WriteLine("     • Use runtime for user-configurable options");
            Console.WriteLine("     • Combine both for maximum flexibility and performance");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Debug logging that's only compiled in debug builds
        /// Shows combining [Conditional] with runtime flags
        /// </summary>
        [Conditional("DEBUG_MODE")]
        private static void LogDebugDetails()
        {
            Console.WriteLine("   🐛 Debug details logged (compile-time conditional)");
            
            // Even within a conditional method, we can use runtime flags
            if (EnableLogging)
            {
                Console.WriteLine("   📊 Additional runtime-configurable debug info");
            }
        }
    }
}
