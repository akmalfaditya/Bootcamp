using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Task-Based Asynchronous Programming Demo ===\n");

            // Basic Task concepts
            await BasicTaskDemo();
            
            // Task creation patterns
            await TaskCreationDemo();
            
            // Task continuation and chaining
            await TaskContinuationDemo();
            
            // Parallel execution
            await ParallelTaskDemo();
            
            // Error handling in tasks
            await TaskErrorHandlingDemo();
            
            // Cancellation support
            await TaskCancellationDemo();
            
            // Task synchronization
            await TaskSynchronizationDemo();
            
            Console.WriteLine("\n=== Task Demo Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static async Task BasicTaskDemo()
        {
            Console.WriteLine("1. BASIC TASK OPERATIONS");
            Console.WriteLine("========================");

            // Create and start a simple task
            Task simpleTask = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("  Simple task completed on thread: " + Thread.CurrentThread.ManagedThreadId);
            });

            // Create a task that returns a value
            Task<int> valueTask = Task.Run(() =>
            {
                Thread.Sleep(500);
                return 42;
            });

            // Wait for tasks to complete
            await simpleTask;
            int result = await valueTask;
            Console.WriteLine($"  Value task returned: {result}\n");
        }

        static async Task TaskCreationDemo()
        {
            Console.WriteLine("2. TASK CREATION PATTERNS");
            Console.WriteLine("=========================");

            // Different ways to create tasks
            
            // Task.Run - most common for CPU-bound work
            var task1 = Task.Run(() => PerformWork("Task.Run"));
            
            // Task.Factory.StartNew - more control over creation
            var task2 = Task.Factory.StartNew(() => PerformWork("Factory.StartNew"), 
                TaskCreationOptions.LongRunning);
            
            // new Task() + Start() - manual control
            var task3 = new Task(() => PerformWork("Manual Start"));
            task3.Start();
            
            // Task.FromResult - already completed task
            var task4 = Task.FromResult("Immediate Result");
            
            await Task.WhenAll(task1, task2, task3);
            string immediateResult = await task4;
            Console.WriteLine($"  Immediate result: {immediateResult}\n");
        }

        static async Task TaskContinuationDemo()
        {
            Console.WriteLine("3. TASK CONTINUATION AND CHAINING");
            Console.WriteLine("==================================");

            var firstTask = Task.Run(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("  First task completed");
                return 10;
            });

            var continuationTask = firstTask.ContinueWith(antecedent =>
            {
                Console.WriteLine($"  Continuation received: {antecedent.Result}");
                return antecedent.Result * 2;
            });

            int finalResult = await continuationTask;
            Console.WriteLine($"  Final result: {finalResult}\n");
        }

        static async Task ParallelTaskDemo()
        {
            Console.WriteLine("4. PARALLEL TASK EXECUTION");
            Console.WriteLine("===========================");

            var stopwatch = Stopwatch.StartNew();

            // Run multiple tasks in parallel
            var tasks = new[]
            {
                Task.Run(() => SimulateWork("Task A", 1000)),
                Task.Run(() => SimulateWork("Task B", 1500)),
                Task.Run(() => SimulateWork("Task C", 800))
            };

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
            
            stopwatch.Stop();
            Console.WriteLine($"  All parallel tasks completed in {stopwatch.ElapsedMilliseconds}ms\n");
        }

        static async Task TaskErrorHandlingDemo()
        {
            Console.WriteLine("5. TASK ERROR HANDLING");
            Console.WriteLine("======================");

            try
            {
                var faultyTask = Task.Run(() =>
                {
                    Thread.Sleep(500);
                    throw new InvalidOperationException("Something went wrong!");
                });

                await faultyTask;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  Caught exception: {ex.Message}");
            }

            // Multiple tasks with potential errors
            var tasks = new[]
            {
                Task.Run(() => { Thread.Sleep(300); return "Success 1"; }),
                Task.Run(() => { Thread.Sleep(200); throw new Exception("Error in task 2"); }),
                Task.Run(() => { Thread.Sleep(400); return "Success 3"; })
            };

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                Console.WriteLine("  One or more tasks failed");
                foreach (var task in tasks)
                {
                    if (task.IsFaulted)
                        Console.WriteLine($"    Faulted task: {task.Exception?.GetBaseException().Message}");
                    else if (task.IsCompletedSuccessfully)
                        Console.WriteLine($"    Successful task: {task.Result}");
                }
            }
            Console.WriteLine();
        }

        static async Task TaskCancellationDemo()
        {
            Console.WriteLine("6. TASK CANCELLATION");
            Console.WriteLine("====================");

            using var cts = new CancellationTokenSource();
            
            var longRunningTask = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"  Working... step {i + 1}");
                    await Task.Delay(300, cts.Token);
                }
                return "Completed";
            }, cts.Token);

            // Cancel after 1 second
            cts.CancelAfter(1000);

            try
            {
                string result = await longRunningTask;
                Console.WriteLine($"  Task result: {result}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Task was cancelled");
            }
            Console.WriteLine();
        }

        static async Task TaskSynchronizationDemo()
        {
            Console.WriteLine("7. TASK SYNCHRONIZATION");
            Console.WriteLine("=======================");

            int sharedCounter = 0;
            var lockObject = new object();

            var incrementTasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                int taskId = i;
                incrementTasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        lock (lockObject)
                        {
                            sharedCounter++;
                        }
                    }
                    Console.WriteLine($"  Task {taskId} completed");
                });
            }

            await Task.WhenAll(incrementTasks);
            Console.WriteLine($"  Final counter value: {sharedCounter} (expected: 5000)\n");
        }

        static void PerformWork(string taskName)
        {
            Thread.Sleep(200);
            Console.WriteLine($"  {taskName} completed on thread: {Thread.CurrentThread.ManagedThreadId}");
        }

        static void SimulateWork(string taskName, int delayMs)
        {
            Thread.Sleep(delayMs);
            Console.WriteLine($"  {taskName} completed after {delayMs}ms");
        }
    }
}
