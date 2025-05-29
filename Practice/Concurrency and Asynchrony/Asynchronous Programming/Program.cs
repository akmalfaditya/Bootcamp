using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgrammingMasterclass
{
    class Program
    {
        // This demo showcases async programming from basic concepts to advanced patterns
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Asynchronous Programming Masterclass ===");
            Console.WriteLine("This comprehensive demo covers async/await, cancellation, progress, and task combinators\n");

            await RunDemo("1. Synchronous vs Asynchronous Operations", DemoSyncVsAsync);
            await RunDemo("2. Basic Async/Await Patterns", DemoBasicAsyncAwait);
            await RunDemo("3. I/O-Bound vs Compute-Bound Operations", DemoIOVsCompute);
            await RunDemo("4. Continuations and Task Chaining", DemoContinuations);
            await RunDemo("5. Capturing Local State", DemoLocalState);
            await RunDemo("6. Parallel Asynchronous Execution", DemoParallelAsync);
            await RunDemo("7. Asynchronous Streams (C# 8+)", DemoAsyncStreams);
            await RunDemo("8. Cancellation with CancellationToken", DemoCancellation);
            await RunDemo("9. Task Combinators - WhenAny", DemoWhenAny);
            await RunDemo("10. Task Combinators - WhenAll", DemoWhenAll);
            await RunDemo("11. Progress Reporting", DemoProgressReporting);
            await RunDemo("12. Exception Handling in Async", DemoAsyncExceptions);

            Console.WriteLine("\n=== Async Masterclass Complete ===");
            Console.WriteLine("You now understand the fundamentals of asynchronous programming!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Helper method to run demonstrations with clear separation
        static async Task RunDemo(string title, Func<Task> demoMethod)
        {
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));
            
            try
            {
                await demoMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Demo error: {ex.Message}");
            }
            
            Console.WriteLine("\nPress Enter to continue to next demo...");
            Console.ReadLine();
        }

        // Demo 1: Shows the fundamental difference between sync and async operations
        static async Task DemoSyncVsAsync()
        {
            Console.WriteLine("Let's compare synchronous and asynchronous operations:");
            
            // Synchronous operation - blocks the thread
            Console.WriteLine("\n--- Synchronous Operation (Thread Blocked) ---");
            var syncStart = DateTime.Now;
            Console.WriteLine($"Starting synchronous operation at {syncStart:HH:mm:ss.fff}");
            
            // This blocks the calling thread for 3 seconds
            Thread.Sleep(3000);
            Console.WriteLine($"Synchronous operation completed at {DateTime.Now:HH:mm:ss.fff}");
            Console.WriteLine("Notice: The thread was completely blocked and couldn't do anything else");
            
            // Asynchronous operation - doesn't block the thread
            Console.WriteLine("\n--- Asynchronous Operation (Thread Not Blocked) ---");
            var asyncStart = DateTime.Now;
            Console.WriteLine($"Starting asynchronous operation at {asyncStart:HH:mm:ss.fff}");
            
            // Start async operation but don't await immediately
            var delayTask = DelayWithProgressAsync(3000);
            
            // While the delay is happening, we can do other work
            for (int i = 1; i <= 6; i++)
            {
                Console.WriteLine($"Main thread doing other work: {i}/6");
                await Task.Delay(500); // Non-blocking delay
            }
            
            // Now wait for the original operation to complete
            await delayTask;
            Console.WriteLine($"Asynchronous operation completed at {DateTime.Now:HH:mm:ss.fff}");
            Console.WriteLine("Notice: The thread remained responsive and could do other work!");
        }

        // Helper method for demonstrating async with progress
        static async Task DelayWithProgressAsync(int milliseconds)
        {
            Console.WriteLine($"[Async Task] Starting {milliseconds}ms delay...");
            await Task.Delay(milliseconds);
            Console.WriteLine($"[Async Task] Delay completed!");
        }

        // Demo 2: Basic async/await patterns and how they work
        static async Task DemoBasicAsyncAwait()
        {
            Console.WriteLine("Understanding async/await fundamentals:");
            
            // Simple async method call
            Console.WriteLine("\n--- Simple Async Method Call ---");
            int result = await CalculatePrimesAsync(1000);
            Console.WriteLine($"Found {result} prime numbers under 1000");
            
            // Method without async (returns Task directly)
            Console.WriteLine("\n--- Method Returning Task Directly ---");
            string message = await GetMessageAsync();
            Console.WriteLine($"Received message: {message}");
            
            // Demonstrating await behavior
            Console.WriteLine("\n--- Demonstrating Await Behavior ---");
            Console.WriteLine("Before await - this executes immediately");
            
            await Task.Delay(1000); // This pauses execution but doesn't block the thread
            
            Console.WriteLine("After await - this executes after the delay");
            Console.WriteLine("The method was paused at 'await' but the thread wasn't blocked");
        }

        // Async method that performs CPU-bound work on a background thread
        static async Task<int> CalculatePrimesAsync(int maxNumber)
        {
            Console.WriteLine($"[CalculatePrimesAsync] Starting calculation for numbers up to {maxNumber}");
            
            // Use Task.Run to move CPU-bound work to a background thread
            int primeCount = await Task.Run(() =>
            {
                Console.WriteLine($"[Background Thread] Calculating primes...");
                return Enumerable.Range(2, maxNumber - 1).Count(IsPrime);
            });
            
            Console.WriteLine($"[CalculatePrimesAsync] Calculation completed");
            return primeCount;
        }

        // Simple method that returns a Task directly (without async)
        static Task<string> GetMessageAsync()
        {
            Console.WriteLine("[GetMessageAsync] Preparing message...");
            
            // This method doesn't use async but still returns a Task
            return Task.FromResult("Hello from async world!");
        }

        // Helper method to check if a number is prime
        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        // Demo 3: I/O-bound vs Compute-bound operations
        static async Task DemoIOVsCompute()
        {
            Console.WriteLine("Comparing I/O-bound and Compute-bound async operations:");
            
            // I/O-bound operation - typically doesn't consume threads while waiting
            Console.WriteLine("\n--- I/O-Bound Operation (File Reading Simulation) ---");
            string fileContent = await SimulateFileReadAsync("data.txt");
            Console.WriteLine($"File content: {fileContent}");
            
            // Compute-bound operation - uses background threads
            Console.WriteLine("\n--- Compute-Bound Operation (Heavy Calculation) ---");
            long fibonacci = await CalculateFibonacciAsync(35);
            Console.WriteLine($"Fibonacci(35) = {fibonacci}");
            
            Console.WriteLine("\nKey Difference:");
            Console.WriteLine("- I/O-bound: Thread is released while waiting for external resources");
            Console.WriteLine("- Compute-bound: Uses background thread to avoid blocking UI thread");
        }

        // Simulates an I/O-bound operation (like reading a file)
        static async Task<string> SimulateFileReadAsync(string filename)
        {
            Console.WriteLine($"[I/O Operation] Starting to read file: {filename}");
            
            // Simulate network/disk delay - this doesn't block a thread
            await Task.Delay(2000);
            
            Console.WriteLine($"[I/O Operation] File read completed");
            return $"Content of {filename}: Sample data from file";
        }

        // Compute-bound operation moved to background thread
        static async Task<long> CalculateFibonacciAsync(int n)
        {
            Console.WriteLine($"[Compute Operation] Starting Fibonacci calculation for n={n}");
            
            // Use Task.Run to move CPU-intensive work to background thread
            long result = await Task.Run(() =>
            {
                Console.WriteLine($"[Background Thread] Computing Fibonacci({n})...");
                return CalculateFibonacci(n);
            });
            
            Console.WriteLine($"[Compute Operation] Fibonacci calculation completed");
            return result;
        }

        // Recursive Fibonacci (intentionally inefficient for demo purposes)
        static long CalculateFibonacci(int n)
        {
            if (n <= 1) return n;
            return CalculateFibonacci(n - 1) + CalculateFibonacci(n - 2);
        }

        // Demo 4: Continuations and task chaining
        static async Task DemoContinuations()
        {
            Console.WriteLine("Demonstrating continuations and task chaining:");
            
            // Method 1: Using ContinueWith (older pattern)
            Console.WriteLine("\n--- Using ContinueWith (Traditional Approach) ---");
            
            Task<int> primeTask = CalculatePrimesAsync(500);
            
            // Attach continuation using ContinueWith
            Task continuationTask = primeTask.ContinueWith(task =>
            {
                Console.WriteLine($"[Continuation] Prime count task completed with result: {task.Result}");
                Console.WriteLine($"[Continuation] Task status: {task.Status}");
            });
            
            await continuationTask;
            
            // Method 2: Using async/await (modern approach)
            Console.WriteLine("\n--- Using async/await (Modern Approach) ---");
            await ProcessDataAsync();
        }

        // Demonstrates modern async/await chaining
        static async Task ProcessDataAsync()
        {
            Console.WriteLine("[ProcessData] Starting data processing pipeline...");
            
            // Step 1: Fetch data
            var data = await FetchDataAsync();
            Console.WriteLine($"[ProcessData] Fetched: {data}");
            
            // Step 2: Transform data
            var processedData = await TransformDataAsync(data);
            Console.WriteLine($"[ProcessData] Transformed: {processedData}");
            
            // Step 3: Save data
            await SaveDataAsync(processedData);
            Console.WriteLine("[ProcessData] Data processing pipeline completed!");
        }

        static async Task<string> FetchDataAsync()
        {
            Console.WriteLine("[FetchData] Simulating data fetch...");
            await Task.Delay(1000);
            return "Raw data from source";
        }

        static async Task<string> TransformDataAsync(string input)
        {
            Console.WriteLine("[TransformData] Processing data...");
            await Task.Delay(800);
            return $"Processed: {input.ToUpper()}";
        }

        static async Task SaveDataAsync(string data)
        {
            Console.WriteLine("[SaveData] Saving to database...");
            await Task.Delay(600);
            Console.WriteLine($"[SaveData] Saved: {data}");
        }

        // Demo 5: How async methods capture local state
        static async Task DemoLocalState()
        {
            Console.WriteLine("Demonstrating local state capture in async methods:");
            
            // Local variables are captured and preserved across await points
            string userName = "John Doe";
            int userId = 12345;
            
            Console.WriteLine($"Before async operation - User: {userName}, ID: {userId}");
            
            // Even though we await here, local variables are preserved
            await Task.Delay(1000);
            
            Console.WriteLine($"After delay - User: {userName}, ID: {userId}");
            Console.WriteLine("Local variables were preserved across the await!");
            
            // Demonstrating state capture in loops
            Console.WriteLine("\n--- State Capture in Loops ---");
            
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"Loop iteration {i} - before await");
                
                // The value of 'i' is captured for each iteration
                await Task.Delay(500);
                
                Console.WriteLine($"Loop iteration {i} - after await");
                Console.WriteLine($"Variable 'i' maintained its value: {i}");
            }
        }

        // Demo 6: Running multiple async operations in parallel
        static async Task DemoParallelAsync()
        {
            Console.WriteLine("Demonstrating parallel asynchronous execution:");
            
            // Sequential execution (one after another)
            Console.WriteLine("\n--- Sequential Execution ---");
            var sequentialStart = DateTime.Now;
            
            await SimulateWorkAsync("Task A", 1000);
            await SimulateWorkAsync("Task B", 1500);
            await SimulateWorkAsync("Task C", 800);
            
            var sequentialTime = DateTime.Now - sequentialStart;
            Console.WriteLine($"Sequential total time: {sequentialTime.TotalMilliseconds:F0}ms");
            
            // Parallel execution (all at once)
            Console.WriteLine("\n--- Parallel Execution ---");
            var parallelStart = DateTime.Now;
            
            // Start all tasks but don't await them immediately
            Task taskA = SimulateWorkAsync("Task A", 1000);
            Task taskB = SimulateWorkAsync("Task B", 1500);
            Task taskC = SimulateWorkAsync("Task C", 800);
            
            // Now await all of them
            await taskA;
            await taskB;
            await taskC;
            
            var parallelTime = DateTime.Now - parallelStart;
            Console.WriteLine($"Parallel total time: {parallelTime.TotalMilliseconds:F0}ms");
            Console.WriteLine($"Time saved: {(sequentialTime - parallelTime).TotalMilliseconds:F0}ms");
        }

        static async Task SimulateWorkAsync(string taskName, int duration)
        {
            Console.WriteLine($"[{taskName}] Starting work (duration: {duration}ms)");
            await Task.Delay(duration);
            Console.WriteLine($"[{taskName}] Work completed");
        }

        // Demo 7: Asynchronous streams (C# 8 feature)
        static async Task DemoAsyncStreams()
        {
            Console.WriteLine("Demonstrating asynchronous streams (IAsyncEnumerable):");
            Console.WriteLine("Async streams allow you to iterate over data that arrives asynchronously");
            
            Console.WriteLine("\n--- Processing Streaming Data ---");
            
            // Consume an async stream
            await foreach (var number in GenerateNumbersAsync(5, 800))
            {
                Console.WriteLine($"Received number: {number}");
                
                // You can perform async operations while processing each item
                await ProcessNumberAsync(number);
            }
            
            Console.WriteLine("All streaming data processed!");
        }

        // Async enumerable that yields values over time
        static async IAsyncEnumerable<int> GenerateNumbersAsync(int count, int delayMs)
        {
            Console.WriteLine($"[Generator] Starting to generate {count} numbers with {delayMs}ms delay");
            
            for (int i = 1; i <= count; i++)
            {
                // Simulate async data generation (like reading from a network stream)
                await Task.Delay(delayMs);
                
                Console.WriteLine($"[Generator] Yielding number {i}");
                yield return i * i; // Return square of the number
            }
            
            Console.WriteLine("[Generator] Finished generating numbers");
        }

        static async Task ProcessNumberAsync(int number)
        {
            // Simulate some async processing of each number
            await Task.Delay(200);
            Console.WriteLine($"  -> Processed {number} (result: {number * 2})");
        }

        // Demo 8: Cancellation using CancellationToken
        static async Task DemoCancellation()
        {
            Console.WriteLine("Demonstrating cancellation with CancellationToken:");
            
            // Create a cancellation token source
            using var cancellationSource = new CancellationTokenSource();
            
            // Set automatic cancellation after 3 seconds
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(3));
            
            Console.WriteLine("Starting a long-running task that will be cancelled after 3 seconds...");
            
            try
            {
                await LongRunningTaskAsync(cancellationSource.Token);
                Console.WriteLine("Task completed successfully");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was cancelled!");
            }
            
            // Manual cancellation example
            Console.WriteLine("\n--- Manual Cancellation Example ---");
            
            using var manualCancellation = new CancellationTokenSource();
            
            // Start a task
            var task = CountdownTaskAsync(10, manualCancellation.Token);
            
            // Cancel after 2 seconds
            await Task.Delay(2000);
            Console.WriteLine("Requesting cancellation...");
            manualCancellation.Cancel();
            
            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Countdown was cancelled!");
            }
        }

        static async Task LongRunningTaskAsync(CancellationToken cancellationToken)
        {
            for (int i = 1; i <= 10; i++)
            {
                // Check for cancellation before each iteration
                cancellationToken.ThrowIfCancellationRequested();
                
                Console.WriteLine($"Long running task - step {i}/10");
                
                // Use cancellation token with Task.Delay
                await Task.Delay(1000, cancellationToken);
            }
        }

        static async Task CountdownTaskAsync(int count, CancellationToken cancellationToken)
        {
            for (int i = count; i >= 1; i--)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                Console.WriteLine($"Countdown: {i}");
                await Task.Delay(500, cancellationToken);
            }
        }

        // Demo 9: Task.WhenAny for handling first completion
        static async Task DemoWhenAny()
        {
            Console.WriteLine("Demonstrating Task.WhenAny - waiting for first completion:");
            
            // Example 1: Race between multiple operations
            Console.WriteLine("\n--- Racing Multiple Operations ---");
            
            Task<string> fastTask = SimulateNetworkCallAsync("Fast Server", 1000);
            Task<string> mediumTask = SimulateNetworkCallAsync("Medium Server", 2000);
            Task<string> slowTask = SimulateNetworkCallAsync("Slow Server", 3000);
            
            // Wait for the first one to complete
            Task<string> winner = await Task.WhenAny(fastTask, mediumTask, slowTask);
            string result = await winner;
            
            Console.WriteLine($"Winner: {result}");
            Console.WriteLine("Other tasks are still running in the background!");
            
            // Example 2: Implementing timeout with WhenAny
            Console.WriteLine("\n--- Implementing Timeout ---");
            
            try
            {
                string timeoutResult = await WithTimeoutAsync(
                    SimulateNetworkCallAsync("Timeout Test Server", 4000),
                    TimeSpan.FromSeconds(2)
                );
                Console.WriteLine($"Result: {timeoutResult}");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Operation timed out!");
            }
        }

        static async Task<string> SimulateNetworkCallAsync(string serverName, int delayMs)
        {
            Console.WriteLine($"[{serverName}] Starting request...");
            await Task.Delay(delayMs);
            Console.WriteLine($"[{serverName}] Request completed!");
            return $"Data from {serverName}";
        }

        // Helper method to implement timeout using WhenAny
        static async Task<T> WithTimeoutAsync<T>(Task<T> task, TimeSpan timeout)
        {
            Task delayTask = Task.Delay(timeout);
            Task winner = await Task.WhenAny(task, delayTask);
            
            if (winner == delayTask)
            {
                throw new TimeoutException();
            }
            
            return await task;
        }

        // Demo 10: Task.WhenAll for waiting for all completions
        static async Task DemoWhenAll()
        {
            Console.WriteLine("Demonstrating Task.WhenAll - waiting for all completions:");
            
            // Example 1: Processing multiple items in parallel
            Console.WriteLine("\n--- Processing Multiple Items in Parallel ---");
            
            string[] urls = { "api/users", "api/products", "api/orders" };
            
            // Create tasks for all API calls
            Task<string>[] apiTasks = urls.Select(url => 
                FetchDataFromApiAsync(url)).ToArray();
            
            Console.WriteLine("All API calls started in parallel...");
            
            // Wait for all to complete
            string[] results = await Task.WhenAll(apiTasks);
            
            Console.WriteLine("All API calls completed:");
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine($"  {urls[i]}: {results[i]}");
            }
            
            // Example 2: Exception handling with WhenAll
            Console.WriteLine("\n--- Exception Handling with WhenAll ---");
            
            try
            {
                await Task.WhenAll(
                    SuccessfulTaskAsync("Task 1"),
                    FailingTaskAsync("Task 2"),
                    SuccessfulTaskAsync("Task 3")
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"One or more tasks failed: {ex.Message}");
                Console.WriteLine("Note: WhenAll stops on first exception but other tasks continue");
            }
        }

        static async Task<string> FetchDataFromApiAsync(string endpoint)
        {
            Console.WriteLine($"[API] Fetching data from {endpoint}...");
            
            // Simulate different response times
            Random random = new Random();
            int delay = random.Next(500, 2000);
            await Task.Delay(delay);
            
            Console.WriteLine($"[API] Data fetched from {endpoint}");
            return $"Data from {endpoint} (took {delay}ms)";
        }

        static async Task<string> SuccessfulTaskAsync(string taskName)
        {
            Console.WriteLine($"[{taskName}] Starting...");
            await Task.Delay(1000);
            Console.WriteLine($"[{taskName}] Completed successfully");
            return $"{taskName} result";
        }

        static async Task<string> FailingTaskAsync(string taskName)
        {
            Console.WriteLine($"[{taskName}] Starting...");
            await Task.Delay(800);
            Console.WriteLine($"[{taskName}] About to fail...");
            throw new InvalidOperationException($"{taskName} failed!");
        }

        // Demo 11: Progress reporting in async operations
        static async Task DemoProgressReporting()
        {
            Console.WriteLine("Demonstrating progress reporting in async operations:");
            
            // Create a progress reporter
            var progress = new Progress<ProgressInfo>(progressInfo =>
            {
                Console.WriteLine($"Progress: {progressInfo.Percentage:F1}% - {progressInfo.Message}");
                
                // Update a progress bar (simulated)
                int barLength = 20;
                int filledLength = (int)(progressInfo.Percentage / 100.0 * barLength);
                string bar = new string('█', filledLength) + new string('░', barLength - filledLength);
                Console.WriteLine($"[{bar}] {progressInfo.Percentage:F1}%");
            });
            
            Console.WriteLine("Starting data processing with progress reporting...");
            
            await ProcessLargeDatasetAsync(progress);
            
            Console.WriteLine("Data processing completed!");
        }        // Custom progress info class
        public class ProgressInfo
        {
            public double Percentage { get; set; }
            public string Message { get; set; } = string.Empty;
        }

        static async Task ProcessLargeDatasetAsync(IProgress<ProgressInfo> progress)
        {
            const int totalItems = 10;
            
            for (int i = 1; i <= totalItems; i++)
            {
                // Simulate processing each item
                await Task.Delay(500);
                
                // Report progress
                double percentage = (double)i / totalItems * 100;
                progress?.Report(new ProgressInfo
                {
                    Percentage = percentage,
                    Message = $"Processed item {i} of {totalItems}"
                });
            }
        }

        // Demo 12: Exception handling in async operations
        static async Task DemoAsyncExceptions()
        {
            Console.WriteLine("Demonstrating exception handling in async operations:");
            
            // Example 1: Basic exception handling
            Console.WriteLine("\n--- Basic Exception Handling ---");
            
            try
            {
                await MethodThatThrowsAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }
            
            // Example 2: Multiple async operations with exceptions
            Console.WriteLine("\n--- Handling Multiple Async Exceptions ---");
            
            Task[] tasks = {
                SafeAsyncOperation("Operation 1"),
                FaultyAsyncOperation("Operation 2"),
                SafeAsyncOperation("Operation 3")
            };
            
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                Console.WriteLine("At least one operation failed. Checking individual results:");
                
                foreach (var task in tasks)
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine($"  Task failed: {task.Exception?.InnerException?.Message}");
                    }
                    else if (task.IsCompletedSuccessfully)
                    {
                        Console.WriteLine($"  Task succeeded");
                    }
                }
            }
            
            // Example 3: Exception handling with async streams
            Console.WriteLine("\n--- Exception Handling in Async Streams ---");
            
            try
            {
                await foreach (var item in GenerateItemsWithErrorAsync())
                {
                    Console.WriteLine($"Processed: {item}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stream processing failed: {ex.Message}");
            }
        }

        static async Task MethodThatThrowsAsync()
        {
            Console.WriteLine("Starting method that will throw...");
            await Task.Delay(500);
            throw new InvalidOperationException("Something went wrong in async method!");
        }

        static async Task SafeAsyncOperation(string operationName)
        {
            Console.WriteLine($"[{operationName}] Starting safe operation...");
            await Task.Delay(1000);
            Console.WriteLine($"[{operationName}] Completed successfully");
        }

        static async Task FaultyAsyncOperation(string operationName)
        {
            Console.WriteLine($"[{operationName}] Starting faulty operation...");
            await Task.Delay(800);
            throw new InvalidOperationException($"{operationName} encountered an error!");
        }

        static async IAsyncEnumerable<int> GenerateItemsWithErrorAsync()
        {
            for (int i = 1; i <= 5; i++)
            {
                await Task.Delay(300);
                
                if (i == 3)
                {
                    throw new InvalidOperationException("Error occurred while generating item 3!");
                }
                
                yield return i;
            }
        }
    }
}
