using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingDemonstration
{
    class Program
    {
        // Shared state for demonstrating thread safety
        private static int sharedCounter = 0;
        private static readonly object lockObject = new object();
        
        // Signal for demonstrating thread communication
        private static ManualResetEvent signal = new ManualResetEvent(false);
        
        // Flag for controlling thread execution
        private static bool shouldStop = false;

        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Threading Demonstration ===\n");
            
            // Demo 1: Basic Thread Creation and Management
            Console.WriteLine("1. Basic Thread Creation and Lifecycle");
            DemoBasicThreading();
            
            Console.WriteLine("\n" + new string('-', 50) + "\n");
            
            // Demo 2: Thread Safety and Race Conditions
            Console.WriteLine("2. Thread Safety and Race Conditions");
            DemoThreadSafety();
            
            Console.WriteLine("\n" + new string('-', 50) + "\n");
            
            // Demo 3: Passing Data to Threads
            Console.WriteLine("3. Passing Data to Threads");
            DemoDataPassing();
            
            Console.WriteLine("\n" + new string('-', 50) + "\n");
            
            // Demo 4: Thread Priority
            Console.WriteLine("4. Thread Priority");
            DemoThreadPriority();
            
            Console.WriteLine("\n" + new string('-', 50) + "\n");
            
            // Demo 5: Thread Signaling
            Console.WriteLine("5. Thread Signaling with ManualResetEvent");
            DemoThreadSignaling();
            
            Console.WriteLine("\n" + new string('-', 50) + "\n");
            
            // Demo 6: Thread Pool and Tasks
            Console.WriteLine("6. Thread Pool and Tasks");
            DemoThreadPool();
            
            Console.WriteLine("\n" + new string('-', 50) + "\n");
            
            // Demo 7: Exception Handling in Threads
            Console.WriteLine("7. Exception Handling in Threads");
            DemoExceptionHandling();
            
            Console.WriteLine("\n" + new string('-', 50) + "\n");
            
            // Demo 8: Background vs Foreground Threads
            Console.WriteLine("8. Background vs Foreground Threads");
            DemoBackgroundThreads();
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Demo 1: Shows basic thread creation, starting, joining, and naming
        static void DemoBasicThreading()
        {
            Console.WriteLine("Creating and starting a new thread...");
            
            // Create a thread that will execute the PrintNumbers method
            Thread workerThread = new Thread(PrintNumbers)
            {
                Name = "NumberPrinter",  // Naming threads helps with debugging
                IsBackground = false     // Foreground thread (default)
            };
            
            Console.WriteLine($"Thread created. IsAlive: {workerThread.IsAlive}");
            
            // Start the thread execution
            workerThread.Start();
            Console.WriteLine($"Thread started. IsAlive: {workerThread.IsAlive}");
            
            // Main thread does some work while worker thread runs in parallel
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"Main({i}) ");
                Thread.Sleep(100);  // Simulate some work
            }
            
            // Wait for the worker thread to complete before continuing
            Console.WriteLine("\nWaiting for worker thread to finish...");
            workerThread.Join();
            
            Console.WriteLine($"Worker thread completed. IsAlive: {workerThread.IsAlive}");
        }

        // Helper method that will be executed by the worker thread
        static void PrintNumbers()
        {
            Console.WriteLine($"\nWorker thread '{Thread.CurrentThread.Name}' starting...");
            
            for (int i = 1; i <= 5; i++)
            {
                Console.Write($"Worker({i}) ");
                Thread.Sleep(150);  // Simulate work that takes time
            }
            
            Console.WriteLine($"\nWorker thread '{Thread.CurrentThread.Name}' finishing...");
        }

        // Demo 2: Demonstrates race conditions and how to fix them with locks
        static void DemoThreadSafety()
        {
            Console.WriteLine("Demonstrating race conditions and thread safety...");
            
            // Reset shared counter
            sharedCounter = 0;
            
            Console.WriteLine("\nFirst, let's see unsafe increment (race condition):");
            
            // Create multiple threads that increment the same counter unsafely
            Thread[] unsafeThreads = new Thread[3];
            for (int i = 0; i < unsafeThreads.Length; i++)
            {
                unsafeThreads[i] = new Thread(UnsafeIncrement);
                unsafeThreads[i].Start();
            }
            
            // Wait for all unsafe threads to complete
            foreach (Thread t in unsafeThreads)
                t.Join();
            
            Console.WriteLine($"Unsafe result: {sharedCounter} (should be 3000, but likely isn't)");
            
            // Reset counter for safe demonstration
            sharedCounter = 0;
            
            Console.WriteLine("\nNow let's see safe increment (with lock):");
            
            // Create threads that increment safely using locks
            Thread[] safeThreads = new Thread[3];
            for (int i = 0; i < safeThreads.Length; i++)
            {
                safeThreads[i] = new Thread(SafeIncrement);
                safeThreads[i].Start();
            }
            
            // Wait for all safe threads to complete
            foreach (Thread t in safeThreads)
                t.Join();
            
            Console.WriteLine($"Safe result: {sharedCounter} (should be exactly 3000)");
        }

        // Unsafe increment - can cause race conditions
        static void UnsafeIncrement()
        {
            for (int i = 0; i < 1000; i++)
            {
                // This is NOT thread-safe: multiple threads can read the same value
                // and increment it, causing lost updates
                sharedCounter++;
            }
        }

        // Safe increment - uses lock to prevent race conditions
        static void SafeIncrement()
        {
            for (int i = 0; i < 1000; i++)
            {
                // Lock ensures only one thread can access the critical section at a time
                lock (lockObject)
                {
                    sharedCounter++;
                }
            }
        }

        // Demo 3: Shows different ways to pass data to threads
        static void DemoDataPassing()
        {
            Console.WriteLine("Passing data to threads using different methods...");
            
            // Method 1: Using lambda expressions (most common and flexible)
            string message1 = "Hello from lambda thread!";
            Thread lambdaThread = new Thread(() => ProcessMessage(message1));
            lambdaThread.Start();
            lambdaThread.Join();
            
            // Method 2: Using ParameterizedThreadStart (requires object casting)
            Thread paramThread = new Thread(ProcessMessageWithParameter);
            paramThread.Start("Hello from parameterized thread!");
            paramThread.Join();
            
            // Method 3: Capturing variables in closures (be careful with this!)
            for (int i = 0; i < 3; i++)
            {
                int capturedValue = i;  // Capture the value, not the variable
                Thread closureThread = new Thread(() => 
                    Console.WriteLine($"Closure thread: {capturedValue}"));
                closureThread.Start();
                closureThread.Join();
            }
        }

        static void ProcessMessage(string message)
        {
            Console.WriteLine($"Processing: {message}");
        }

        static void ProcessMessageWithParameter(object messageObj)
        {
            // Need to cast the object parameter to the expected type
            string message = (string)messageObj;
            Console.WriteLine($"Processing: {message}");
        }

        // Demo 4: Shows how thread priority affects execution
        static void DemoThreadPriority()
        {
            Console.WriteLine("Demonstrating thread priority...");
            Console.WriteLine("Watch how different priorities affect execution order");
            
            // Create threads with different priorities
            Thread lowPriorityThread = new Thread(() => PrintWithPriority("LOW", 20))
            {
                Priority = ThreadPriority.Lowest,
                Name = "LowPriority"
            };
            
            Thread normalPriorityThread = new Thread(() => PrintWithPriority("NORMAL", 20))
            {
                Priority = ThreadPriority.Normal,
                Name = "NormalPriority"
            };
            
            Thread highPriorityThread = new Thread(() => PrintWithPriority("HIGH", 20))
            {
                Priority = ThreadPriority.Highest,
                Name = "HighPriority"
            };
            
            // Start all threads at roughly the same time
            lowPriorityThread.Start();
            normalPriorityThread.Start();
            highPriorityThread.Start();
            
            // Wait for all to complete
            lowPriorityThread.Join();
            normalPriorityThread.Join();
            highPriorityThread.Join();
            
            Console.WriteLine("\nNote: Priority effects may vary based on OS scheduler");
        }

        static void PrintWithPriority(string label, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write($"{label} ");
                // Small delay to make priority effects more visible
                Thread.SpinWait(1000000);
            }
        }

        // Demo 5: Shows thread signaling using ManualResetEvent
        static void DemoThreadSignaling()
        {
            Console.WriteLine("Demonstrating thread signaling...");
            
            // Reset the signal
            signal.Reset();
            
            // Start a worker thread that waits for a signal
            Thread waiterThread = new Thread(WaitForSignal)
            {
                Name = "SignalWaiter"
            };
            waiterThread.Start();
            
            // Main thread does some work before sending the signal
            Console.WriteLine("Main thread working for 2 seconds...");
            Thread.Sleep(2000);
            
            Console.WriteLine("Main thread sending signal...");
            signal.Set();  // This will unblock the waiting thread
            
            waiterThread.Join();
            Console.WriteLine("Signal demonstration complete.");
        }

        static void WaitForSignal()
        {
            Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' waiting for signal...");
            
            // This will block until another thread calls signal.Set()
            signal.WaitOne();
            
            Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' received signal!");
        }

        // Demo 6: Shows thread pool usage and Task.Run
        static void DemoThreadPool()
        {
            Console.WriteLine("Demonstrating Thread Pool and Tasks...");
            
            // Method 1: Using ThreadPool.QueueUserWorkItem (older approach)
            Console.WriteLine("\nUsing ThreadPool.QueueUserWorkItem:");
            for (int i = 0; i < 3; i++)
            {
                int workItem = i;
                ThreadPool.QueueUserWorkItem(state => 
                {
                    Console.WriteLine($"ThreadPool work item {workItem} executing on thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(500);
                });
            }
            
            // Give thread pool tasks time to complete
            Thread.Sleep(1000);
            
            // Method 2: Using Task.Run (modern approach)
            Console.WriteLine("\nUsing Task.Run:");
            Task[] tasks = new Task[3];
            for (int i = 0; i < tasks.Length; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(() => 
                {
                    Console.WriteLine($"Task {taskId} executing on thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(500);
                    return taskId;
                });
            }
            
            // Wait for all tasks to complete
            Task.WaitAll(tasks);
            Console.WriteLine("All tasks completed.");
            
            // Show thread pool info
            int workerThreads, completionPortThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"Available worker threads: {workerThreads}");
        }

        // Demo 7: Shows proper exception handling in threads
        static void DemoExceptionHandling()
        {
            Console.WriteLine("Demonstrating exception handling in threads...");
            
            // Example 1: Unhandled exception in thread (bad practice)
            Console.WriteLine("\nCreating thread with proper exception handling:");
            Thread exceptionThread = new Thread(MethodWithException);
            exceptionThread.Start();
            exceptionThread.Join();
            
            // Example 2: Using Task for better exception handling
            Console.WriteLine("\nUsing Task for exception handling:");
            Task faultyTask = Task.Run(() => 
            {
                throw new InvalidOperationException("Something went wrong in task!");
            });
            
            try
            {
                faultyTask.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"Task exception caught: {ex.InnerException?.Message}");
            }
        }

        static void MethodWithException()
        {
            try
            {
                Console.WriteLine("Thread starting work...");
                
                // Simulate some work that might fail
                Random random = new Random();
                if (random.Next(2) == 0)
                {
                    throw new InvalidOperationException("Random failure occurred!");
                }
                
                Console.WriteLine("Thread completed successfully.");
            }
            catch (Exception ex)
            {
                // Always handle exceptions within threads
                Console.WriteLine($"Exception in thread: {ex.Message}");
            }
        }

        // Demo 8: Shows the difference between foreground and background threads
        static void DemoBackgroundThreads()
        {
            Console.WriteLine("Demonstrating background vs foreground threads...");
            
            // Foreground thread - will keep the application alive
            Thread foregroundThread = new Thread(LongRunningWork)
            {
                Name = "ForegroundWorker",
                IsBackground = false  // This is the default
            };
            
            // Background thread - won't prevent application from exiting
            Thread backgroundThread = new Thread(LongRunningWork)
            {
                Name = "BackgroundWorker",
                IsBackground = true   // This makes it a background thread
            };
            
            Console.WriteLine("Starting both foreground and background threads...");
            Console.WriteLine("The background thread won't prevent the app from exiting.");
            
            foregroundThread.Start();
            backgroundThread.Start();
            
            // Give threads a moment to start
            Thread.Sleep(1000);
            
            Console.WriteLine("Main thread finishing...");
            Console.WriteLine("Foreground thread will complete, background thread may be terminated.");
            
            // We'll join the foreground thread so we can see it complete
            // The background thread may or may not complete depending on timing
            foregroundThread.Join();
        }

        static void LongRunningWork()
        {
            string threadType = Thread.CurrentThread.IsBackground ? "Background" : "Foreground";
            Console.WriteLine($"{threadType} thread '{Thread.CurrentThread.Name}' starting...");
            
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"{threadType} thread working... {i + 1}/5");
                Thread.Sleep(500);
            }
            
            Console.WriteLine($"{threadType} thread '{Thread.CurrentThread.Name}' completed.");
        }
    }
}
