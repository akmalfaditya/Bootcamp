using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TaskDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Task-Based Asynchronous Programming Demo ===");
            Console.WriteLine("This demo shows how Tasks solve the limitations of direct thread usage\n");

            // Show the problems with direct thread usage first
            DirectThreadLimitationsDemo();
            
            // Basic Task concepts - addressing thread limitations
            await BasicTaskDemo();
            
            // Task creation patterns and thread pool integration
            await TaskCreationDemo();
            
            // Returning values - solving the thread return value problem
            await TaskReturnValueDemo();
            
            // Exception handling - solving thread exception propagation
            await TaskExceptionHandlingDemo();
            
            // Continuations - solving the "what happens next" problem
            await TaskContinuationDemo();
            
            // TaskCompletionSource - manual task control
            await TaskCompletionSourceDemo();
            
            // Task.Delay - non-blocking delays
            await TaskDelayDemo();
            
            // Advanced scenarios
            await AdvancedTaskScenariosDemo();
            
            Console.WriteLine("\n=== Task Demo Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DirectThreadLimitationsDemo()
        {
            Console.WriteLine("1. DIRECT THREAD LIMITATIONS DEMO");
            Console.WriteLine("==================================");
            Console.WriteLine("Let's see the problems with direct thread usage:");
            
            // Problem 1: No return values
            Console.WriteLine("\n  Problem 1: No return values from threads");
            string sharedResult = "";
            var thread1 = new Thread(() => {
                Thread.Sleep(100);
                sharedResult = "Thread completed"; // Must use shared field
            });
            thread1.Start();
            thread1.Join(); // Must block to wait
            Console.WriteLine($"  Result from thread (via shared field): {sharedResult}");
            
            // Problem 2: Exception handling is difficult
            Console.WriteLine("\n  Problem 2: Exception propagation is problematic");
            var thread2 = new Thread(() => {
                throw new Exception("Thread exception"); // This will crash the app!
            });
            // We can't easily catch this exception in the main thread
            Console.WriteLine("  (We can't demonstrate this safely - it would crash the app!)");
            
            // Problem 3: No built-in continuation mechanism
            Console.WriteLine("\n  Problem 3: No built-in way to chain operations");
            Console.WriteLine("  With threads, you must manually coordinate 'what happens next'");
            
            Console.WriteLine("\n  Solution: Use Task instead of Thread!\n");
        }

        static async Task BasicTaskDemo()
        {
            Console.WriteLine("2. BASIC TASK OPERATIONS");
            Console.WriteLine("========================");
            Console.WriteLine("Tasks solve the thread limitations we just saw:");

            // Tasks are "hot" - they start immediately
            Console.WriteLine("\n  Creating and starting tasks (they start immediately):");
            Task simpleTask = Task.Run(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine($"  Simple task completed on thread: {Thread.CurrentThread.ManagedThreadId}");
            });

            // Check task status
            Console.WriteLine($"  Task status after creation: {simpleTask.Status}");
            
            // Wait for completion (non-blocking in async context)
            await simpleTask;
            Console.WriteLine($"  Task status after completion: {simpleTask.Status}");
            
            // Tasks use thread pool by default (efficient resource management)
            Console.WriteLine("\n  Tasks use thread pool threads by default:");
            var poolTasks = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                int taskId = i;
                poolTasks[i] = Task.Run(() => 
                {
                    Console.WriteLine($"  Task {taskId} on thread {Thread.CurrentThread.ManagedThreadId} (IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread})");
                });
            }
            await Task.WhenAll(poolTasks);
            Console.WriteLine();
        }

        static async Task TaskCreationDemo()
        {
            Console.WriteLine("3. TASK CREATION PATTERNS");
            Console.WriteLine("=========================");
            Console.WriteLine("Different ways to create tasks for different scenarios:");

            // Task.Run - most common, uses thread pool
            Console.WriteLine("\n  Task.Run() - Most common, uses thread pool:");
            var task1 = Task.Run(() => {
                Thread.Sleep(200);
                Console.WriteLine("  Task.Run completed");
            });

            // Task.Factory.StartNew - more control over task creation
            Console.WriteLine("\n  Task.Factory.StartNew() - More control over creation:");
            var task2 = Task.Factory.StartNew(() => {
                Thread.Sleep(200);
                Console.WriteLine("  Factory.StartNew completed");
            });

            // Long-running tasks - prevents thread pool starvation
            Console.WriteLine("\n  Long-running tasks (prevent thread pool starvation):");
            var longTask = Task.Factory.StartNew(() => {
                Thread.Sleep(200);
                Console.WriteLine($"  Long-running task on thread {Thread.CurrentThread.ManagedThreadId} (IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread})");
            }, TaskCreationOptions.LongRunning);

            // Task.FromResult - already completed task
            Console.WriteLine("\n  Task.FromResult() - Already completed task:");
            var completedTask = Task.FromResult("Immediate result");
            Console.WriteLine($"  Immediate result: {await completedTask}");

            await Task.WhenAll(task1, task2, longTask);
            Console.WriteLine();
        }

        static async Task TaskReturnValueDemo()
        {
            Console.WriteLine("4. TASK RETURN VALUES");
            Console.WriteLine("=====================");
            Console.WriteLine("Tasks can return values - solving the thread limitation:");

            // Generic Task<T> can return values
            Console.WriteLine("\n  Task<T> can return values:");
            Task<int> calculationTask = Task.Run(() => {
                Thread.Sleep(500);
                Console.WriteLine("  Performing calculation...");
                return 3 + 2;
            });

            // You can do other work while the task runs
            Console.WriteLine("  Doing other work while task runs...");
            
            // Get the result (will await completion if needed)
            int result = await calculationTask;
            Console.WriteLine($"  Calculation result: {result}");

            // Multiple tasks returning different types
            Console.WriteLine("\n  Multiple tasks with different return types:");
            var stringTask = Task.Run(() => {
                Thread.Sleep(200);
                return "Hello from task";
            });
            
            var boolTask = Task.Run(() => {
                Thread.Sleep(300);
                return true;
            });

            string stringResult = await stringTask;
            bool boolResult = await boolTask;
            Console.WriteLine($"  String result: {stringResult}");
            Console.WriteLine($"  Boolean result: {boolResult}");
            Console.WriteLine();
        }

        static async Task TaskExceptionHandlingDemo()
        {
            Console.WriteLine("5. TASK EXCEPTION HANDLING");
            Console.WriteLine("===========================");
            Console.WriteLine("Tasks properly propagate exceptions - solving another thread limitation:");

            // Single task exception handling
            Console.WriteLine("\n  Single task exception handling:");
            try
            {
                await Task.Run(() => {
                    Thread.Sleep(200);
                    throw new InvalidOperationException("Something went wrong in the task!");
                });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  Caught exception: {ex.Message}");
            }

            // Multiple tasks with exceptions (AggregateException)
            Console.WriteLine("\n  Multiple tasks with exceptions:");
            var tasks = new[]
            {
                Task.Run(() => { Thread.Sleep(100); return "Success 1"; }),
                Task.Run(() => { Thread.Sleep(150); throw new ArgumentException("Error in task 2"); }),
                Task.Run(() => { Thread.Sleep(200); return "Success 3"; }),
                Task.Run(() => { Thread.Sleep(250); throw new NullReferenceException("Error in task 4"); })
            };

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  First exception caught: {ex.Message}");
                Console.WriteLine("  Checking all task states:");
                
                for (int i = 0; i < tasks.Length; i++)
                {
                    var task = tasks[i];
                    if (task.IsFaulted)
                    {
                        Console.WriteLine($"    Task {i}: FAULTED - {task.Exception?.GetBaseException().Message}");
                    }
                    else if (task.IsCompletedSuccessfully)
                    {
                        // We need to cast to Task<string> to access Result
                        if (task is Task<string> stringTask)
                            Console.WriteLine($"    Task {i}: SUCCESS - {stringTask.Result}");
                        else
                            Console.WriteLine($"    Task {i}: SUCCESS");
                    }
                    else if (task.IsCanceled)
                    {
                        Console.WriteLine($"    Task {i}: CANCELED");
                    }
                }
            }
            Console.WriteLine();
        }

        static async Task TaskContinuationDemo()
        {
            Console.WriteLine("6. TASK CONTINUATIONS");
            Console.WriteLine("=====================");
            Console.WriteLine("Continuations solve the 'what happens next' problem:");

            // Method 1: GetAwaiter().OnCompleted() - used by async/await
            Console.WriteLine("\n  Method 1: GetAwaiter().OnCompleted() (used by async/await):");
            var primeTask = Task.Run(() => {
                Console.WriteLine("  Calculating prime numbers...");
                Thread.Sleep(500);
                return Enumerable.Range(2, 1000).Count(n => 
                    Enumerable.Range(2, (int)Math.Sqrt(n)).All(i => n % i != 0));
            });

            var awaiter = primeTask.GetAwaiter();
            awaiter.OnCompleted(() => {
                try
                {
                    int result = awaiter.GetResult(); // Direct exception propagation
                    Console.WriteLine($"  Prime count result: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Exception in continuation: {ex.Message}");
                }
            });

            // Wait for the continuation to complete
            await primeTask;

            // Method 2: ContinueWith() - more explicit control
            Console.WriteLine("\n  Method 2: ContinueWith() - explicit control:");
            var continuationChain = Task.Run(() => {
                Console.WriteLine("  First task executing...");
                Thread.Sleep(200);
                return 10;
            }).ContinueWith(antecedent => {
                Console.WriteLine($"  Second task received: {antecedent.Result}");
                return antecedent.Result * 2;
            }).ContinueWith(antecedent => {
                Console.WriteLine($"  Third task received: {antecedent.Result}");
                return antecedent.Result + 5;
            });

            int finalResult = await continuationChain;
            Console.WriteLine($"  Final chained result: {finalResult}");
            Console.WriteLine();
        }

        static async Task TaskCompletionSourceDemo()
        {
            Console.WriteLine("7. TASKCOMPLETIONSOURCE");
            Console.WriteLine("=======================");
            Console.WriteLine("Manual task control for I/O-bound operations:");

            // Example 1: Timer-based completion (no thread blocking)
            Console.WriteLine("\n  Example 1: Timer-based task completion:");
            var timerTask = CreateTimerTask(2000);
            Console.WriteLine("  Timer task created, doing other work...");
            
            // Do other work while waiting
            await Task.Delay(500);
            Console.WriteLine("  Still doing other work...");
            
            string timerResult = await timerTask;
            Console.WriteLine($"  Timer task result: {timerResult}");

            // Example 2: Manual completion with different outcomes
            Console.WriteLine("\n  Example 2: Manual completion scenarios:");
            await DemonstrateManualCompletion();
            Console.WriteLine();
        }

        static Task<string> CreateTimerTask(int delayMs)
        {
            var tcs = new TaskCompletionSource<string>();
            
            // Use System.Timers.Timer for demonstration
            var timer = new System.Timers.Timer(delayMs) { AutoReset = false };
            timer.Elapsed += (sender, e) => {
                timer.Dispose();
                tcs.SetResult($"Timer completed after {delayMs}ms");
            };
            timer.Start();
            
            return tcs.Task;
        }

        static async Task DemonstrateManualCompletion()
        {
            // Success scenario
            var successTcs = new TaskCompletionSource<string>();
            _ = Task.Run(async () => {
                await Task.Delay(200);
                successTcs.SetResult("Manual success!");
            });
            
            // Exception scenario
            var exceptionTcs = new TaskCompletionSource<string>();
            _ = Task.Run(async () => {
                await Task.Delay(300);
                exceptionTcs.SetException(new InvalidOperationException("Manual exception!"));
            });
            
            // Cancellation scenario
            var cancellationTcs = new TaskCompletionSource<string>();
            _ = Task.Run(async () => {
                await Task.Delay(400);
                cancellationTcs.SetCanceled();
            });

            // Handle success
            try
            {
                string result = await successTcs.Task;
                Console.WriteLine($"  Success result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Unexpected exception: {ex.Message}");
            }

            // Handle exception
            try
            {
                await exceptionTcs.Task;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  Expected exception: {ex.Message}");
            }

            // Handle cancellation
            try
            {
                await cancellationTcs.Task;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("  Task was cancelled as expected");
            }
        }

        static async Task TaskDelayDemo()
        {
            Console.WriteLine("8. TASK.DELAY - NON-BLOCKING DELAYS");
            Console.WriteLine("===================================");
            Console.WriteLine("Task.Delay is the async equivalent of Thread.Sleep:");

            Console.WriteLine("\n  Traditional Thread.Sleep blocks the thread:");
            var stopwatch = Stopwatch.StartNew();
            // Thread.Sleep(1000); // This would block!
            Console.WriteLine("  (We skip Thread.Sleep to avoid blocking)");

            Console.WriteLine("\n  Task.Delay doesn't block - it returns immediately:");
            stopwatch.Restart();
            var delayTask = Task.Delay(1000);
            Console.WriteLine($"  Task.Delay returned immediately after {stopwatch.ElapsedMilliseconds}ms");
            
            // You can do other work while waiting
            Console.WriteLine("  Doing other work while delay runs...");
            await Task.Delay(200);
            Console.WriteLine("  Still doing work...");
            
            // Now wait for the original delay to complete
            await delayTask;
            Console.WriteLine($"  Delay completed after total {stopwatch.ElapsedMilliseconds}ms");

            // Chaining delays with continuations
            Console.WriteLine("\n  Chaining delays with continuations:");
            var chainedDelays = Task.Delay(500)
                .ContinueWith(_ => {
                    Console.WriteLine("  First delay completed");
                    return Task.Delay(300);
                })
                .Unwrap() // Unwrap the nested task
                .ContinueWith(_ => Console.WriteLine("  Second delay completed"));

            await chainedDelays; // Wait for the chain to complete
            Console.WriteLine();
        }

        static async Task AdvancedTaskScenariosDemo()
        {
            Console.WriteLine("9. ADVANCED TASK SCENARIOS");
            Console.WriteLine("===========================");
            Console.WriteLine("Advanced patterns and best practices:");

            // Parallel execution with different completion times
            Console.WriteLine("\n  Parallel execution with Task.WhenAll:");
            var parallelTasks = new[]
            {
                CreateWorkTask("Task A", 800),
                CreateWorkTask("Task B", 600),
                CreateWorkTask("Task C", 1000)
            };

            var sw = Stopwatch.StartNew();
            await Task.WhenAll(parallelTasks);
            sw.Stop();
            Console.WriteLine($"  All parallel tasks completed in {sw.ElapsedMilliseconds}ms");

            // First to complete with Task.WhenAny
            Console.WriteLine("\n  First to complete with Task.WhenAny:");
            var raceTasks = new[]
            {
                CreateWorkTask("Racer 1", 400),
                CreateWorkTask("Racer 2", 300),
                CreateWorkTask("Racer 3", 500)
            };

            sw.Restart();
            var firstCompleted = await Task.WhenAny(raceTasks);
            sw.Stop();
            Console.WriteLine($"  First task completed: {firstCompleted.Result} in {sw.ElapsedMilliseconds}ms");

            // Wait for all racers to finish
            await Task.WhenAll(raceTasks);

            // Cancellation support
            Console.WriteLine("\n  Cancellation support:");
            await CancellationDemo();
            Console.WriteLine();
        }

        static Task<string> CreateWorkTask(string taskName, int delayMs)
        {
            return Task.Run(async () => {
                await Task.Delay(delayMs);
                Console.WriteLine($"  {taskName} completed after {delayMs}ms");
                return taskName;
            });
        }

        static async Task CancellationDemo()
        {
            using var cts = new CancellationTokenSource();
            
            var cancellableTask = Task.Run(async () => {
                for (int i = 0; i < 10; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"  Working... step {i + 1}");
                    await Task.Delay(200, cts.Token);
                }
                return "Work completed";
            }, cts.Token);

            // Cancel after 800ms
            cts.CancelAfter(800);

            try
            {
                string result = await cancellableTask;
                Console.WriteLine($"  Task result: {result}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Task was cancelled before completion");
            }
        }
    }
}
