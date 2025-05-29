using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Conditional_Compilation
{
    /// <summary>
    /// Advanced example showing conditional compilation in a service class
    /// This demonstrates real-world patterns you might use in larger applications
    /// </summary>
    public class DataProcessingService
    {
        // Runtime configuration - can be modified without recompilation
        private static readonly Dictionary<string, bool> FeatureFlags = new()
        {
            ["EnableCaching"] = true,
            ["UseAsyncProcessing"] = true,
            ["DetailedValidation"] = false
        };

        // Static cache that's only available in certain builds
        #if DEVELOPMENT || TESTING_ENABLED
        private static readonly Dictionary<string, object> DevCache = new();
        #endif

        /// <summary>
        /// Processes data with different behaviors based on compilation symbols
        /// Shows how conditional compilation can affect business logic
        /// </summary>
        public void ProcessData(string[] data)
        {
            LogOperation("Starting data processing");

            // Pre-processing validation - only in development builds
            #if DEVELOPMENT
            ValidateInputData(data);
            #endif

            // Different processing strategies based on build configuration
            #if LEGACY_SUPPORT
            ProcessDataLegacy(data);
            #else
            ProcessDataModern(data);
            #endif

            // Post-processing verification - conditional on testing builds
            #if TESTING_ENABLED
            VerifyProcessingResults(data);
            #endif

            LogOperation("Data processing completed");
        }

        /// <summary>
        /// Modern data processing implementation
        /// This version uses newer, more efficient algorithms
        /// </summary>
        #if !LEGACY_SUPPORT
        private void ProcessDataModern(string[] data)
        {
            LogOperation("Using modern data processing algorithm");
            
            // Simulate modern processing with performance metrics
            var stopwatch = Stopwatch.StartNew();
            
            foreach (var item in data)
            {
                // Modern processing logic here
                var processedItem = item.ToUpperInvariant().Trim();
                LogDebug($"Processed item: {processedItem}");
            }
            
            stopwatch.Stop();
            LogPerformance("Modern processing", stopwatch.ElapsedMilliseconds);
        }
        #endif

        /// <summary>
        /// Legacy data processing implementation
        /// Kept for backward compatibility with older systems
        /// </summary>
        #if LEGACY_SUPPORT
        private void ProcessDataLegacy(string[] data)
        {
            LogOperation("Using legacy data processing algorithm");
            
            // Legacy implementation - slower but more compatible
            for (int i = 0; i < data.Length; i++)
            {
                // Legacy processing logic here
                var processedItem = data[i].ToUpper().Trim();
                LogDebug($"Legacy processed item: {processedItem}");
                
                // Simulate legacy processing delay
                System.Threading.Thread.Sleep(10);
            }
        }
        #endif

        /// <summary>
        /// Input validation that's only compiled in development builds
        /// Removes validation overhead from production code
        /// </summary>
        #if DEVELOPMENT
        private void ValidateInputData(string[] data)
        {
            LogDebug("Performing detailed input validation");
            
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Data array cannot be null");
            
            if (data.Length == 0)
                LogDebug("Warning: Empty data array provided");
            
            foreach (var item in data)
            {
                if (string.IsNullOrWhiteSpace(item))
                    LogDebug($"Warning: Empty or whitespace item found in data");
            }
            
            LogDebug($"Validation completed. {data.Length} items validated.");
        }
        #endif

        /// <summary>
        /// Result verification for testing builds
        /// Helps ensure processing worked correctly during testing
        /// </summary>
        #if TESTING_ENABLED
        private void VerifyProcessingResults(string[] originalData)
        {
            LogDebug("Verifying processing results");
            
            // Simulate verification logic
            foreach (var item in originalData)
            {
                // Verify the item was processed correctly
                LogDebug($"Verified processing of: {item}");
            }
            
            LogDebug("Processing verification completed successfully");
        }
        #endif

        /// <summary>
        /// Caching mechanism that's only available in development or testing
        /// Production builds don't include caching to save memory
        /// </summary>
        #if DEVELOPMENT || TESTING_ENABLED
        public T GetFromCache<T>(string key) where T : class
        {
            if (DevCache.TryGetValue(key, out var value))
            {
                LogDebug($"Cache hit for key: {key}");
                return value as T;
            }
            
            LogDebug($"Cache miss for key: {key}");
            return null;
        }

        public void SetCache<T>(string key, T value) where T : class
        {
            DevCache[key] = value;
            LogDebug($"Cached value for key: {key}");
        }
        #endif

        // =====================================
        // CONDITIONAL LOGGING METHODS
        // =====================================

        /// <summary>
        /// General operation logging - included when LOGGING symbol is defined
        /// </summary>
        [Conditional("LOGGING")]
        private void LogOperation(string message)
        {
            Console.WriteLine($"   📋 [OPERATION] {message}");
        }

        /// <summary>
        /// Debug logging - only included in debug builds
        /// </summary>
        [Conditional("DEBUG_MODE")]
        [Conditional("DEVELOPMENT")]  // Multiple conditional attributes = OR logic
        private void LogDebug(string message)
        {
            Console.WriteLine($"   🔍 [DEBUG] {message}");
        }

        /// <summary>
        /// Performance logging - only when performance metrics are enabled
        /// </summary>
        [Conditional("PERFORMANCE_METRICS")]
        private void LogPerformance(string operation, long milliseconds)
        {
            Console.WriteLine($"   ⚡ [PERFORMANCE] {operation} took {milliseconds}ms");
        }

        // =====================================
        // RUNTIME FEATURE FLAGS
        // =====================================

        /// <summary>
        /// Example of runtime feature flags vs compile-time symbols
        /// These can be changed without recompilation
        /// </summary>
        public void DemonstrateRuntimeFlags()
        {
            Console.WriteLine("\n   🎛️  Runtime Feature Flags Demo:");

            if (FeatureFlags["EnableCaching"])
            {
                Console.WriteLine("   ✓ Caching is enabled (runtime decision)");
            }

            if (FeatureFlags["UseAsyncProcessing"])
            {
                Console.WriteLine("   ✓ Async processing is enabled (runtime decision)");
            }

            // You can toggle these at runtime
            FeatureFlags["DetailedValidation"] = !FeatureFlags["DetailedValidation"];
            Console.WriteLine($"   🔄 Toggled detailed validation to: {FeatureFlags["DetailedValidation"]}");
        }

        /// <summary>
        /// Shows the difference between compile-time and runtime decisions
        /// </summary>
        public void CompareDecisionTypes()
        {
            Console.WriteLine("\n   🆚 Compile-time vs Runtime Decisions:");

            // Compile-time decision - decided when code is compiled
            #if DEVELOPMENT
            Console.WriteLine("   ⚙️  Compile-time: Development features are ENABLED");
            #else
            Console.WriteLine("   ⚙️  Compile-time: Development features are DISABLED");
            #endif

            // Runtime decision - can be changed while program is running
            if (FeatureFlags["EnableCaching"])
            {
                Console.WriteLine("   🏃 Runtime: Caching is currently ENABLED");
            }
            else
            {
                Console.WriteLine("   🏃 Runtime: Caching is currently DISABLED");
            }

            Console.WriteLine("\n   💡 Key Differences:");
            Console.WriteLine("   • Compile-time: Zero runtime cost when disabled, requires recompilation to change");
            Console.WriteLine("   • Runtime: Small performance cost, can be changed without recompilation");
        }
    }
}
