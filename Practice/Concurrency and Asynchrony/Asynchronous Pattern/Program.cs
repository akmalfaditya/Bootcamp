using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousPattern
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Asynchronous Patterns Demo ===");
            Console.WriteLine("This demo covers the essential patterns for building robust async applications\n");

            // 1. Cancellation patterns
            await CancellationPatternsDemo();
            
            // 2. Progress reporting patterns
            await ProgressReportingDemo();
            
            // 3. Task-based Asynchronous Pattern (TAP)
            await TaskBasedAsyncPatternDemo();
            
            // 4. Task combinators
            await TaskCombinatorsDemo();
            
            Console.WriteLine("\n=== Asynchronous Patterns Demo Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region Cancellation Patterns

        static async Task CancellationPatternsDemo()
        {
            Console.WriteLine("1. CANCELLATION PATTERNS");
            Console.WriteLine("========================");
            Console.WriteLine("Cancellation is critical for responsive applications and resource management");

            // Basic cancellation with CancellationTokenSource
            await BasicCancellationDemo();
            
            // Timeout-based cancellation
            await TimeoutCancellationDemo();
            
            // Cancellation with cleanup
            await CancellationWithCleanupDemo();
            
            // Cancellation callbacks
            await CancellationCallbacksDemo();
            
            Console.WriteLine();
        }

        static async Task BasicCancellationDemo()
        {
            Console.WriteLine("\n  Basic Cancellation Pattern:");
            Console.WriteLine("  CancellationTokenSource initiates cancellation");
            Console.WriteLine("  CancellationToken monitors for cancellation requests");

            using var cts = new CancellationTokenSource();
            
            // Start a long-running operation
            var longRunningTask = LongRunningOperationAsync(cts.Token);
            
            // Cancel after 2 seconds to demonstrate cancellation
            _ = Task.Delay(2000).ContinueWith(_ => {
                Console.WriteLine("  Initiating cancellation...");
                cts.Cancel();
            });

            try
            {
                await longRunningTask;
                Console.WriteLine("  Operation completed successfully");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Operation was cancelled as expected");
            }
        }

        static async Task LongRunningOperationAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("  Starting long-running operation...");
            
            for (int i = 0; i < 10; i++)
            {
                // Check for cancellation at regular intervals
                cancellationToken.ThrowIfCancellationRequested();
                
                Console.WriteLine($"  Processing step {i + 1}/10");
                
                // Task.Delay accepts CancellationToken and will throw OperationCanceledException
                // if cancellation is requested during the delay
                await Task.Delay(500, cancellationToken);
            }
            
            Console.WriteLine("  Long-running operation completed");
        }

        static async Task TimeoutCancellationDemo()
        {
            Console.WriteLine("\n  Timeout-based Cancellation:");
            Console.WriteLine("  CancellationTokenSource can automatically cancel after a time limit");

            // Create a cancellation token with a 2-second timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            
            try
            {
                await SimulateSlowOperationAsync(cts.Token);
                Console.WriteLine("  Operation completed within timeout");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Operation timed out and was cancelled");
            }
        }

        static async Task SimulateSlowOperationAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("  Simulating slow operation (3 seconds)...");
            await Task.Delay(3000, cancellationToken); // This will timeout
            Console.WriteLine("  Slow operation completed");
        }

        static async Task CancellationWithCleanupDemo()
        {
            Console.WriteLine("\n  Cancellation with Resource Cleanup:");
            Console.WriteLine("  Proper resource cleanup is essential when operations are cancelled");

            using var cts = new CancellationTokenSource();
            
            // Cancel after 1 second
            cts.CancelAfter(1000);
            
            try
            {
                await OperationWithResourcesAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Operation cancelled, but resources were cleaned up properly");
            }
        }

        static async Task OperationWithResourcesAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("  Acquiring resources...");
            
            // Simulate resource acquisition
            using var resource = new DisposableResource("Critical Resource");
            
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine($"  Using resource for operation {i + 1}");
                    await Task.Delay(300, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Cancellation detected, cleaning up resources...");
                throw; // Re-throw to maintain cancellation semantics
            }
            // The using statement ensures resource disposal even if cancelled
        }

        static async Task CancellationCallbacksDemo()
        {
            Console.WriteLine("\n  Cancellation Callbacks:");
            Console.WriteLine("  Register callbacks to execute when cancellation occurs");

            using var cts = new CancellationTokenSource();
            
            // Register a callback that executes when cancellation is requested
            cts.Token.Register(() => {
                Console.WriteLine("  Cancellation callback executed - performing cleanup");
            });
            
            // Start operation and cancel after 800ms
            var operation = Task.Run(async () => {
                for (int i = 0; i < 10; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    await Task.Delay(200, cts.Token);
                    Console.WriteLine($"  Background work {i + 1}");
                }
            }, cts.Token);
            
            // Cancel the operation
            await Task.Delay(800);
            cts.Cancel();
            
            try
            {
                await operation;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Background operation was cancelled");
            }
        }

        #endregion

        #region Progress Reporting

        static async Task ProgressReportingDemo()
        {
            Console.WriteLine("2. PROGRESS REPORTING PATTERNS");
            Console.WriteLine("===============================");
            Console.WriteLine("Progress reporting keeps users informed during long-running operations");

            // Basic progress reporting
            await BasicProgressReportingDemo();
            
            // Progress reporting with custom data
            await CustomProgressReportingDemo();
            
            // Progress reporting with cancellation
            await ProgressWithCancellationDemo();
            
            Console.WriteLine();
        }

        static async Task BasicProgressReportingDemo()
        {
            Console.WriteLine("\n  Basic Progress Reporting:");
            Console.WriteLine("  IProgress<T> provides thread-safe progress reporting");
            
            // Create a Progress<T> instance that captures the current synchronization context
            var progress = new Progress<int>(percentage => {
                Console.WriteLine($"  Progress: {percentage}% complete");
            });
            
            await SimulateWorkWithProgressAsync(progress);
            Console.WriteLine("  Work completed successfully");
        }

        static async Task SimulateWorkWithProgressAsync(IProgress<int> progress)
        {
            Console.WriteLine("  Starting work simulation...");
            
            // Simulate CPU-bound work that reports progress
            await Task.Run(() => {
                for (int i = 0; i <= 100; i += 10)
                {
                    // Simulate work
                    Thread.Sleep(200);
                    
                    // Report progress - Progress<T> ensures this is called on the correct thread
                    progress?.Report(i);
                }
            });
        }

        static async Task CustomProgressReportingDemo()
        {
            Console.WriteLine("\n  Custom Progress Reporting:");
            Console.WriteLine("  Use custom types for detailed progress information");
            
            var progress = new Progress<WorkProgress>(info => {
                Console.WriteLine($"  {info.CurrentOperation} - {info.PercentComplete}% ({info.ItemsProcessed}/{info.TotalItems})");
            });
            
            await ProcessItemsWithProgressAsync(progress);
            Console.WriteLine("  All items processed successfully");
        }

        static async Task ProcessItemsWithProgressAsync(IProgress<WorkProgress> progress)
        {
            const int totalItems = 20;
            
            for (int i = 0; i < totalItems; i++)
            {
                // Simulate processing an item
                await Task.Delay(100);
                
                var progressInfo = new WorkProgress
                {
                    CurrentOperation = $"Processing item {i + 1}",
                    ItemsProcessed = i + 1,
                    TotalItems = totalItems,
                    PercentComplete = (int)((i + 1) * 100.0 / totalItems)
                };
                
                progress?.Report(progressInfo);
            }
        }

        static async Task ProgressWithCancellationDemo()
        {
            Console.WriteLine("\n  Progress Reporting with Cancellation:");
            Console.WriteLine("  Combining progress reporting with cancellation support");
            
            using var cts = new CancellationTokenSource();
            
            var progress = new Progress<int>(percentage => {
                Console.WriteLine($"  Download progress: {percentage}%");
            });
            
            // Cancel after 2 seconds
            cts.CancelAfter(2000);
            
            try
            {
                await SimulateDownloadAsync(progress, cts.Token);
                Console.WriteLine("  Download completed successfully");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Download was cancelled by user or timeout");
            }
        }

        static async Task SimulateDownloadAsync(IProgress<int> progress, CancellationToken cancellationToken)
        {
            Console.WriteLine("  Starting download...");
            
            for (int i = 0; i <= 100; i += 5)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                // Simulate download chunk
                await Task.Delay(100, cancellationToken);
                
                progress?.Report(i);
            }
        }

        #endregion

        #region Task-based Asynchronous Pattern (TAP)

        static async Task TaskBasedAsyncPatternDemo()
        {
            Console.WriteLine("3. TASK-BASED ASYNCHRONOUS PATTERN (TAP)");
            Console.WriteLine("=========================================");
            Console.WriteLine("TAP is the recommended pattern for asynchronous operations in .NET");

            // TAP method characteristics
            await TAPMethodCharacteristicsDemo();
            
            // TAP with cancellation and progress
            await TAPWithCancellationAndProgressDemo();
            
            Console.WriteLine();
        }

        static async Task TAPMethodCharacteristicsDemo()
        {
            Console.WriteLine("\n  TAP Method Characteristics:");
            Console.WriteLine("  - Returns Task or Task<TResult>");
            Console.WriteLine("  - Method name ends with 'Async'");
            Console.WriteLine("  - Returns quickly to caller");
            Console.WriteLine("  - Efficient for I/O-bound operations");
            
            // Demonstrate a properly designed TAP method
            string result = await DownloadStringAsync("https://httpbin.org/delay/1");
            Console.WriteLine($"  Downloaded content length: {result.Length} characters");
        }

        // This is a TAP-compliant method
        static async Task<string> DownloadStringAsync(string url)
        {
            Console.WriteLine($"  Initiating download from: {url}");
            
            // Method returns quickly - the actual work is asynchronous
            using var client = new HttpClient();
            return await client.GetStringAsync(url);
        }

        static async Task TAPWithCancellationAndProgressDemo()
        {
            Console.WriteLine("\n  TAP with Cancellation and Progress:");
            Console.WriteLine("  TAP methods should provide overloads for cancellation and progress");
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            
            var progress = new Progress<int>(p => 
                Console.WriteLine($"  Processing progress: {p}%"));
            
            try
            {
                var result = await ProcessDataAsync(1000, progress, cts.Token);
                Console.WriteLine($"  Processing completed. Result: {result}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Processing was cancelled");
            }
        }

        // TAP method with all recommended overloads
        static async Task<int> ProcessDataAsync(int itemCount, IProgress<int>? progress = null, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"  Processing {itemCount} items...");
            
            int processed = 0;
            
            for (int i = 0; i < itemCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                // Simulate processing
                if (i % 100 == 0)
                {
                    await Task.Delay(50, cancellationToken);
                    progress?.Report((i * 100) / itemCount);
                }
                
                processed++;
            }
            
            progress?.Report(100);
            return processed;
        }

        #endregion

        #region Task Combinators

        static async Task TaskCombinatorsDemo()
        {
            Console.WriteLine("4. TASK COMBINATORS");
            Console.WriteLine("===================");
            Console.WriteLine("Task combinators enable powerful composition of asynchronous operations");

            // Task.WhenAny demonstrations
            await TaskWhenAnyDemo();
            
            // Task.WhenAll demonstrations  
            await TaskWhenAllDemo();
            
            // Practical applications
            await PracticalCombinatorsDemo();
            
            Console.WriteLine();
        }

        static async Task TaskWhenAnyDemo()
        {
            Console.WriteLine("\n  Task.WhenAny - First to Complete:");
            Console.WriteLine("  Returns when ANY of the provided tasks completes");
            
            // Create tasks with different completion times
            var task1 = DelayWithResultAsync(1000, "Task 1");
            var task2 = DelayWithResultAsync(2000, "Task 2");
            var task3 = DelayWithResultAsync(3000, "Task 3");
            
            var stopwatch = Stopwatch.StartNew();
            
            // Wait for the first task to complete
            Task<string> winningTask = await Task.WhenAny(task1, task2, task3);
            
            stopwatch.Stop();
            
            string result = await winningTask;
            Console.WriteLine($"  First to complete: {result} (after {stopwatch.ElapsedMilliseconds}ms)");
            
            // Note: Other tasks continue running in the background
            await Task.WhenAll(task1, task2, task3); // Wait for all to complete
            Console.WriteLine("  All tasks have now completed");
        }

        static async Task TaskWhenAllDemo()
        {
            Console.WriteLine("\n  Task.WhenAll - All Must Complete:");
            Console.WriteLine("  Returns when ALL provided tasks complete");
            
            // Create multiple tasks
            var task1 = DelayWithResultAsync(500, "Result 1");
            var task2 = DelayWithResultAsync(800, "Result 2");
            var task3 = DelayWithResultAsync(1200, "Result 3");
            
            var stopwatch = Stopwatch.StartNew();
            
            // Wait for all tasks to complete and collect results
            string[] results = await Task.WhenAll(task1, task2, task3);
            
            stopwatch.Stop();
            
            Console.WriteLine($"  All tasks completed after {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Results: [{string.Join(", ", results)}]");
        }

        static async Task PracticalCombinatorsDemo()
        {
            Console.WriteLine("\n  Practical Combinator Applications:");
            
            // Timeout pattern using Task.WhenAny
            await TimeoutPatternDemo();
            
            // Parallel processing with Task.WhenAll
            await ParallelProcessingDemo();
            
            // Exception handling with combinators
            await ExceptionHandlingDemo();
        }

        static async Task TimeoutPatternDemo()
        {
            Console.WriteLine("\n  Timeout Pattern with Task.WhenAny:");
            Console.WriteLine("  Implement timeout for operations that don't support it natively");
            
            var longOperation = DelayWithResultAsync(3000, "Long operation completed");
            var timeout = Task.Delay(2000);
            
            var completedTask = await Task.WhenAny(longOperation, timeout);
            
            if (completedTask == timeout)
            {
                Console.WriteLine("  Operation timed out after 2 seconds");
            }
            else
            {
                string result = await longOperation;
                Console.WriteLine($"  Operation completed: {result}");
            }
        }

        static async Task ParallelProcessingDemo()
        {
            Console.WriteLine("\n  Parallel Processing with Task.WhenAll:");
            Console.WriteLine("  Download multiple URLs concurrently");
            
            // URLs to download (using httpbin for reliable testing)
            string[] urls = {
                "https://httpbin.org/delay/1",
                "https://httpbin.org/json",
                "https://httpbin.org/uuid"
            };
            
            var stopwatch = Stopwatch.StartNew();
            
            // Create tasks for parallel downloads
            var downloadTasks = urls.Select(async url => {
                using var client = new HttpClient();
                var content = await client.GetStringAsync(url);
                return content.Length;
            });
            
            // Wait for all downloads to complete
            int[] sizes = await Task.WhenAll(downloadTasks);
            
            stopwatch.Stop();
            
            Console.WriteLine($"  Downloaded {sizes.Length} URLs in {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Total size: {sizes.Sum()} characters");
        }

        static async Task ExceptionHandlingDemo()
        {
            Console.WriteLine("\n  Exception Handling with Combinators:");
            Console.WriteLine("  How exceptions are handled in Task.WhenAll");
            
            // Create tasks, some of which will fail
            var task1 = DelayWithResultAsync(500, "Success 1");
            var task2 = DelayWithExceptionAsync(800, "Failure 2");
            var task3 = DelayWithResultAsync(1000, "Success 3");
            var task4 = DelayWithExceptionAsync(1200, "Failure 4");
            
            try
            {
                // Task.WhenAll will throw the first exception encountered
                await Task.WhenAll(task1, task2, task3, task4);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  First exception caught: {ex.Message}");
                
                // Examine individual task states
                Console.WriteLine("  Individual task states:");
                var tasks = new[] { task1, task2, task3, task4 };
                
                for (int i = 0; i < tasks.Length; i++)
                {
                    var task = tasks[i];
                    if (task.IsCompletedSuccessfully)
                    {
                        Console.WriteLine($"    Task {i + 1}: Success - {task.Result}");
                    }
                    else if (task.IsFaulted)
                    {
                        Console.WriteLine($"    Task {i + 1}: Faulted - {task.Exception?.GetBaseException().Message}");
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        static async Task<string> DelayWithResultAsync(int milliseconds, string result)
        {
            await Task.Delay(milliseconds);
            return result;
        }

        static async Task<string> DelayWithExceptionAsync(int milliseconds, string message)
        {
            await Task.Delay(milliseconds);
            throw new InvalidOperationException(message);
        }

        #endregion

        #region Helper Classes

        // Custom progress reporting class
        public class WorkProgress
        {
            public string CurrentOperation { get; set; } = string.Empty;
            public int ItemsProcessed { get; set; }
            public int TotalItems { get; set; }
            public int PercentComplete { get; set; }
        }

        // Disposable resource for demonstrating cleanup
        public class DisposableResource : IDisposable
        {
            private readonly string _name;
            private bool _disposed = false;

            public DisposableResource(string name)
            {
                _name = name;
                Console.WriteLine($"  Resource '{_name}' acquired");
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    Console.WriteLine($"  Resource '{_name}' disposed");
                    _disposed = true;
                }
            }
        }

        #endregion
    }
}
