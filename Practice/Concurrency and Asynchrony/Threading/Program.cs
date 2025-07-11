using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingDemonstration
{
    class Program
    {
        // Shared state for demonstrating thread safety issues
        private static int sharedCounter = 0;
        private static bool sharedFlag = false;
        private static readonly object lockObject = new object();
        
        // Manual reset event for thread signaling demonstrations
        private static ManualResetEvent signal = new ManualResetEvent(false);
        
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Threading Comprehensive Demonstration ===\n");
            Console.WriteLine("This demo covers all key threading concepts from basic creation to advanced scenarios.\n");
            
            // Demo 1: The Classic Thread Example - Main Thread vs New Thread
            Console.WriteLine("1. Classic Threading Example - Interleaved Execution");
            DemoClassicThreading();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 2: Thread Properties and Lifecycle
            Console.WriteLine("2. Thread Properties and Lifecycle Management");
            DemoThreadProperties();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 3: Thread Synchronization - Join and Sleep
            Console.WriteLine("3. Thread Synchronization - Join and Sleep");
            DemoThreadSynchronization();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 4: I/O-bound vs Compute-bound Operations
            Console.WriteLine("4. I/O-bound vs Compute-bound Operations");
            DemoIOvsComputeBound();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 5: Local vs Shared State
            Console.WriteLine("5. Local vs Shared State");
            DemoLocalVsSharedState();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 6: Thread Safety and Locking
            Console.WriteLine("6. Thread Safety and Locking");
            DemoThreadSafety();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 7: Passing Data to Threads
            Console.WriteLine("7. Passing Data to Threads");
            DemoDataPassing();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 8: Exception Handling in Threads
            Console.WriteLine("8. Exception Handling in Threads");
            DemoExceptionHandling();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 9: Foreground vs Background Threads
            Console.WriteLine("9. Foreground vs Background Threads");
            DemoForegroundVsBackground();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 10: Thread Priority
            Console.WriteLine("10. Thread Priority");
            DemoThreadPriority();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 11: Thread Signaling
            Console.WriteLine("11. Thread Signaling with ManualResetEvent");
            DemoThreadSignaling();
            
            Console.WriteLine("\n" + new string('-', 60) + "\n");
            
            // Demo 12: Thread Pool
            Console.WriteLine("12. Thread Pool Usage");
            DemoThreadPool();
            
            Console.WriteLine("\n=== All Threading Demonstrations Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Demo 1: Classic Threading Example - recreates the textbook example
        static void DemoClassicThreading()
        {
            Console.WriteLine("Starting classic threading demo - you'll see interleaved x's and y's");
            
            // Create a new thread to run WriteY method
            Thread t = new Thread(WriteY);
            t.Start();  // Start the new thread
            
            // Meanwhile, main thread does its own work
            for (int i = 0; i < 1000; i++) 
                Console.Write("x");
            
            // Wait for the other thread to complete
            t.Join();
            Console.WriteLine("\nDemo complete - notice how x's and y's were mixed together");
        }
        
        static void WriteY()
        {
            for (int i = 0; i < 1000; i++) 
                Console.Write("y");
        }

        // Demo 2: Thread Properties and Lifecycle
        static void DemoThreadProperties()
        {
            Console.WriteLine("Examining thread properties and lifecycle...");
            
            // Create a thread but don't start it yet
            Thread worker = new Thread(DoSomeWork);
            worker.Name = "WorkerThread";  // Naming helps with debugging
            
            Console.WriteLine($"Before Start - IsAlive: {worker.IsAlive}");
            Console.WriteLine($"Thread Name: {worker.Name}");
            Console.WriteLine($"Is Background: {worker.IsBackground}");
            Console.WriteLine($"Thread State: {worker.ThreadState}");
            
            // Start the thread
            worker.Start();
            Console.WriteLine($"After Start - IsAlive: {worker.IsAlive}");
            Console.WriteLine($"Thread State: {worker.ThreadState}");
            
            // Check current thread info
            Console.WriteLine($"Current thread name: {Thread.CurrentThread.Name ?? "Main"}");
            Console.WriteLine($"Current thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
            
            // Wait for completion
            worker.Join();
            Console.WriteLine($"After Join - IsAlive: {worker.IsAlive}");
            Console.WriteLine($"Final Thread State: {worker.ThreadState}");
        }
        
        static void DoSomeWork()
        {
            Console.WriteLine($"Worker thread starting on thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);  // Simulate work
            Console.WriteLine("Worker thread finishing");
        }

        // Demo 3: Thread Synchronization with Join and Sleep
        static void DemoThreadSynchronization()
        {
            Console.WriteLine("Demonstrating Join and Sleep methods...");
            
            // Start a thread and immediately join it
            Thread quickWorker = new Thread(() => 
            {
                Console.WriteLine("Quick worker starting");
                Thread.Sleep(1000);
                Console.WriteLine("Quick worker done");
            });
            
            quickWorker.Start();
            Console.WriteLine("Main thread waiting for quick worker...");
            quickWorker.Join();  // Main thread blocks until quickWorker completes
            Console.WriteLine("Quick worker finished, main thread continues");
            
            // Demonstrate Join with timeout
            Thread slowWorker = new Thread(() => 
            {
                Console.WriteLine("Slow worker starting");
                Thread.Sleep(3000);
                Console.WriteLine("Slow worker done");
            });
            
            slowWorker.Start();
            Console.WriteLine("Main thread waiting for slow worker (max 1 second)...");
            bool finished = slowWorker.Join(1000);  // Wait max 1 second
            
            if (finished)
                Console.WriteLine("Slow worker finished in time");
            else
                Console.WriteLine("Slow worker didn't finish in time, continuing anyway");
            
            // Demonstrate different Sleep scenarios
            Console.WriteLine("\nDemonstrating Thread.Sleep variations:");
            Console.WriteLine("Sleeping for 500ms...");
            Thread.Sleep(500);
            
            Console.WriteLine("Sleeping with TimeSpan...");
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            
            Console.WriteLine("Yielding time slice (Sleep(0))...");
            Thread.Sleep(0);  // Immediately yield to other threads
            
            Console.WriteLine("Thread.Yield() - yield to threads on same processor");
            Thread.Yield();   // Similar to Sleep(0) but more specific
            
            slowWorker.Join();  // Make sure slow worker finishes
        }

        // Demo 4: I/O-bound vs Compute-bound Operations
        static void DemoIOvsComputeBound()
        {
            Console.WriteLine("Comparing I/O-bound vs Compute-bound operations...");
            
            // I/O-bound operation - waiting for external event
            Console.WriteLine("Starting I/O-bound operation (reading user input)...");
            Thread ioThread = new Thread(() => 
            {
                Console.WriteLine("I/O thread: Please type something and press Enter:");
                string input = Console.ReadLine() ?? "";  // This blocks waiting for I/O
                Console.WriteLine($"I/O thread: You entered: {input}");
            });
            ioThread.Start();
            
            // Compute-bound operation - CPU intensive work
            Console.WriteLine("Starting compute-bound operation (calculating prime numbers)...");
            Thread computeThread = new Thread(() => 
            {
                Console.WriteLine("Compute thread: Finding prime numbers up to 100000...");
                int primeCount = 0;
                for (int i = 2; i <= 100000; i++)
                {
                    if (IsPrime(i)) primeCount++;
                }
                Console.WriteLine($"Compute thread: Found {primeCount} prime numbers");
            });
            computeThread.Start();
            
            // Demonstrate blocking vs spinning
            Console.WriteLine("\nBlocking vs Spinning comparison:");
            
            // Blocking approach (efficient)
            var blockingStart = DateTime.Now;
            Thread.Sleep(1000);  // Blocks, releases CPU
            var blockingEnd = DateTime.Now;
            Console.WriteLine($"Blocking approach took: {(blockingEnd - blockingStart).TotalMilliseconds}ms");
            
            // Spinning approach (inefficient - don't do this for long waits!)
            Console.WriteLine("Spinning approach (brief demo only)...");
            var spinStart = DateTime.Now;
            var endTime = DateTime.Now.AddMilliseconds(100);  // Only spin for 100ms
            while (DateTime.Now < endTime) { }  // Continuous spinning - wastes CPU!
            var spinEnd = DateTime.Now;
            Console.WriteLine($"Spinning approach took: {(spinEnd - spinStart).TotalMilliseconds}ms");
            Console.WriteLine("Note: Spinning wastes CPU cycles - use blocking for longer waits!");
            
            computeThread.Join();
            ioThread.Join();
        }
        
        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        // Demo 5: Local vs Shared State
        static void DemoLocalVsSharedState()
        {
            Console.WriteLine("Demonstrating local vs shared state...");
            
            // Local state - each thread has its own copy
            Console.WriteLine("\nLocal state example - each thread has its own variables:");
            Thread thread1 = new Thread(WorkWithLocalState);
            Thread thread2 = new Thread(WorkWithLocalState);
            
            thread1.Start();
            thread2.Start();
            
            thread1.Join();
            thread2.Join();
            
            // Shared state - threads share the same variables
            Console.WriteLine("\nShared state example - threads share variables:");
            sharedFlag = false;  // Reset shared flag
            
            Thread sharedThread1 = new Thread(WorkWithSharedState);
            Thread sharedThread2 = new Thread(WorkWithSharedState);
            
            sharedThread1.Start();
            sharedThread2.Start();
            
            sharedThread1.Join();
            sharedThread2.Join();
        }
        
        static void WorkWithLocalState()
        {
            // Each thread gets its own copy of these variables
            for (int cycles = 0; cycles < 5; cycles++)
            {
                Console.Write($"Thread {Thread.CurrentThread.ManagedThreadId}: {cycles} ");
                Thread.Sleep(50);
            }
            Console.WriteLine();
        }
        
        static void WorkWithSharedState()
        {
            // Both threads share the same sharedFlag variable
            if (!sharedFlag)
            {
                sharedFlag = true;
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Set shared flag to true");
            }
            else
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Shared flag already true");
            }
        }

        // Demo 6: Thread Safety and Locking
        static void DemoThreadSafety()
        {
            Console.WriteLine("Demonstrating thread safety issues and solutions...");
            
            // First, show the problem - race condition
            Console.WriteLine("\n1. Race condition demonstration (unsafe):");
            sharedCounter = 0;
            
            Thread unsafe1 = new Thread(UnsafeIncrement);
            Thread unsafe2 = new Thread(UnsafeIncrement);
            
            unsafe1.Start();
            unsafe2.Start();
            
            unsafe1.Join();
            unsafe2.Join();
            
            Console.WriteLine($"Expected result: 2000, Actual result: {sharedCounter}");
            Console.WriteLine("Note: Result may vary due to race condition");
            
            // Now show the solution - using locks
            Console.WriteLine("\n2. Thread-safe solution using locks:");
            sharedCounter = 0;
            
            Thread safe1 = new Thread(SafeIncrement);
            Thread safe2 = new Thread(SafeIncrement);
            
            safe1.Start();
            safe2.Start();
            
            safe1.Join();
            safe2.Join();
            
            Console.WriteLine($"Expected result: 2000, Actual result: {sharedCounter}");
            Console.WriteLine("Note: Result should always be exactly 2000");
        }
        
        static void UnsafeIncrement()
        {
            for (int i = 0; i < 1000; i++)
            {
                // This is NOT thread-safe! Multiple threads can read/modify simultaneously
                sharedCounter++;  // Actually: read sharedCounter, increment, write back
            }
        }
        
        static void SafeIncrement()
        {
            for (int i = 0; i < 1000; i++)
            {
                // Lock ensures only one thread can execute this code at a time
                lock (lockObject)
                {
                    sharedCounter++;  // Now this is thread-safe
                }
            }
        }

        // Demo 7: Passing Data to Threads
        static void DemoDataPassing()
        {
            Console.WriteLine("Demonstrating various ways to pass data to threads...");
            
            // Method 1: Lambda expressions (most common and flexible)
            Console.WriteLine("\n1. Using lambda expressions:");
            string message = "Hello from lambda!";
            int number = 42;
            
            Thread lambdaThread = new Thread(() => ProcessData(message, number));
            lambdaThread.Start();
            lambdaThread.Join();
            
            // Method 2: ParameterizedThreadStart (single object parameter)
            Console.WriteLine("\n2. Using ParameterizedThreadStart:");
            Thread paramThread = new Thread(ProcessObjectData);
            paramThread.Start("Hello from ParameterizedThreadStart!");
            paramThread.Join();
            
            // Method 3: Captured variables (be careful with this!)
            Console.WriteLine("\n3. Variable capture in loops - common pitfall:");
            
            // Wrong way - all threads might print the same value
            Console.WriteLine("Wrong way (may print unexpected values):");
            for (int i = 0; i < 3; i++)
            {
                new Thread(() => Console.WriteLine($"Wrong: {i}")).Start();
            }
            Thread.Sleep(100);  // Give threads time to run
            
            // Right way - capture the value properly
            Console.WriteLine("Right way (captures value correctly):");
            for (int i = 0; i < 3; i++)
            {
                int temp = i;  // Create a copy for each iteration
                new Thread(() => Console.WriteLine($"Right: {temp}")).Start();
            }
            Thread.Sleep(100);  // Give threads time to run
            
            // Method 4: Using custom class to pass multiple parameters
            Console.WriteLine("\n4. Using custom class for multiple parameters:");
            var threadData = new ThreadData
            {
                Name = "CustomThread",
                Value = 100,
                Message = "Hello from custom class!"
            };
            
            Thread customThread = new Thread(ProcessCustomData);
            customThread.Start(threadData);
            customThread.Join();
        }
        
        static void ProcessData(string message, int number)
        {
            Console.WriteLine($"Processing: {message}, Number: {number}");
        }
        
        static void ProcessObjectData(object? data)
        {
            // Need to cast and null-check when using object parameter
            if (data is string message)
            {
                Console.WriteLine($"Processing: {message}");
            }
        }
        
        static void ProcessCustomData(object? data)
        {
            if (data is ThreadData threadData)
            {
                Console.WriteLine($"Name: {threadData.Name}, Value: {threadData.Value}, Message: {threadData.Message}");
            }
        }
        
        // Helper class for passing multiple parameters
        public class ThreadData
        {
            public string Name { get; set; } = "";
            public int Value { get; set; }
            public string Message { get; set; } = "";
        }

        // Demo 8: Exception Handling in Threads
        static void DemoExceptionHandling()
        {
            Console.WriteLine("Demonstrating exception handling in threads...");
            
            // Show that exceptions don't cross thread boundaries
            Console.WriteLine("\n1. Exception handling demonstration:");
            
            try
            {
                Thread faultyThread = new Thread(ThrowException);
                faultyThread.Start();
                faultyThread.Join();
                
                Console.WriteLine("Main thread continues after thread completed");
            }
            catch (Exception ex)
            {
                // This will NOT catch the exception from the other thread
                Console.WriteLine($"This won't be reached: {ex.Message}");
            }
            
            // Show proper exception handling within threads
            Console.WriteLine("\n2. Proper exception handling:");
            Thread safeThread = new Thread(SafeMethodWithException);
            safeThread.Start();
            safeThread.Join();
        }
        
        static void ThrowException()
        {
            try
            {
                Console.WriteLine("Thread about to throw exception...");
                throw new InvalidOperationException("Something went wrong in thread!");
            }
            catch (Exception ex)
            {
                // Exception must be handled within the thread
                Console.WriteLine($"Exception caught in thread: {ex.Message}");
            }
        }
        
        static void SafeMethodWithException()
        {
            try
            {
                Console.WriteLine("Safe thread starting work...");
                
                // Simulate some work that might fail
                Random random = new Random();
                if (random.Next(2) == 0)
                {
                    throw new InvalidOperationException("Random failure!");
                }
                
                Console.WriteLine("Safe thread completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Safe thread handled exception: {ex.Message}");
            }
        }

        // Demo 9: Foreground vs Background Threads
        static void DemoForegroundVsBackground()
        {
            Console.WriteLine("Demonstrating foreground vs background threads...");
            
            // Create a foreground thread (default behavior)
            Thread foregroundThread = new Thread(DoWork)
            {
                Name = "ForegroundWorker",
                IsBackground = false  // This is the default
            };
            
            // Create a background thread
            Thread backgroundThread = new Thread(DoWork)
            {
                Name = "BackgroundWorker",
                IsBackground = true   // This makes it a background thread
            };
            
            Console.WriteLine($"Foreground thread IsBackground: {foregroundThread.IsBackground}");
            Console.WriteLine($"Background thread IsBackground: {backgroundThread.IsBackground}");
            
            Console.WriteLine("\nStarting both threads...");
            foregroundThread.Start();
            backgroundThread.Start();
            
            // Give threads time to start
            Thread.Sleep(500);
            
            Console.WriteLine("Main thread ending...");
            Console.WriteLine("Foreground thread will keep app alive until it completes");
            Console.WriteLine("Background thread will be terminated when app exits");
            
            // Wait for foreground thread to complete
            foregroundThread.Join();
            
            // Background thread may or may not complete - depends on timing
            Console.WriteLine("Application will now exit");
        }
        
        static void DoWork()
        {
            string threadType = Thread.CurrentThread.IsBackground ? "Background" : "Foreground";
            Console.WriteLine($"{threadType} thread '{Thread.CurrentThread.Name}' starting...");
            
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{threadType} thread working... {i + 1}/3");
                Thread.Sleep(1000);
            }
            
            Console.WriteLine($"{threadType} thread completed");
        }

        // Demo 10: Thread Priority
        static void DemoThreadPriority()
        {
            Console.WriteLine("Demonstrating thread priority...");
            
            // Create threads with different priorities
            Thread lowPriority = new Thread(() => PrintWithPriority("LOW", 20))
            {
                Priority = ThreadPriority.Lowest,
                Name = "LowPriority"
            };
            
            Thread normalPriority = new Thread(() => PrintWithPriority("NORMAL", 20))
            {
                Priority = ThreadPriority.Normal,
                Name = "NormalPriority"
            };
            
            Thread highPriority = new Thread(() => PrintWithPriority("HIGH", 20))
            {
                Priority = ThreadPriority.Highest,
                Name = "HighPriority"
            };
            
            Console.WriteLine("Priority values:");
            Console.WriteLine($"Low: {lowPriority.Priority}");
            Console.WriteLine($"Normal: {normalPriority.Priority}");
            Console.WriteLine($"High: {highPriority.Priority}");
            
            Console.WriteLine("\nStarting threads with different priorities...");
            Console.WriteLine("Higher priority threads should get more CPU time:");
            
            // Start all threads
            lowPriority.Start();
            normalPriority.Start();
            highPriority.Start();
            
            // Wait for completion
            lowPriority.Join();
            normalPriority.Join();
            highPriority.Join();
            
            Console.WriteLine("\nNote: Priority effects depend on OS scheduler and system load");
        }
        
        static void PrintWithPriority(string label, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write($"{label} ");
                // Add some computation to make priority effects more visible
                Thread.SpinWait(500000);
            }
            Console.WriteLine();
        }

        // Demo 11: Thread Signaling
        static void DemoThreadSignaling()
        {
            Console.WriteLine("Demonstrating thread signaling with ManualResetEvent...");
            
            // Reset the signal to closed state
            signal.Reset();
            
            Console.WriteLine("Creating a thread that waits for signal...");
            Thread waiter = new Thread(WaitForSignal)
            {
                Name = "WaiterThread"
            };
            
            waiter.Start();
            
            Console.WriteLine("Main thread doing work for 3 seconds...");
            Thread.Sleep(3000);
            
            Console.WriteLine("Main thread sending signal...");
            signal.Set();  // Open the signal - releases waiting threads
            
            waiter.Join();
            
            // Show that signal remains open
            Console.WriteLine("\nSignal remains open - subsequent waits don't block:");
            Thread quickWaiter = new Thread(() => 
            {
                Console.WriteLine("Quick waiter: Checking signal...");
                signal.WaitOne();  // This won't block because signal is already set
                Console.WriteLine("Quick waiter: Signal was already open!");
            });
            
            quickWaiter.Start();
            quickWaiter.Join();
            
            // Reset signal for next use
            signal.Reset();
            Console.WriteLine("Signal reset for next demonstration");
        }
        
        static void WaitForSignal()
        {
            Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' waiting for signal...");
            signal.WaitOne();  // Block until signal is set
            Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' received signal and continuing!");
        }

        // Demo 12: Thread Pool
        static void DemoThreadPool()
        {
            Console.WriteLine("Demonstrating thread pool usage...");
            
            // Show thread pool information
            int workerThreads, completionPortThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            ThreadPool.GetMaxThreads(out int maxWorker, out int maxCompletion);
            
            Console.WriteLine($"Max worker threads: {maxWorker}");
            Console.WriteLine($"Available worker threads: {workerThreads}");
            Console.WriteLine($"Max completion port threads: {maxCompletion}");
            
            // Method 1: ThreadPool.QueueUserWorkItem (legacy approach)
            Console.WriteLine("\n1. Using ThreadPool.QueueUserWorkItem:");
            for (int i = 0; i < 5; i++)
            {
                int workItem = i;
                ThreadPool.QueueUserWorkItem(state => 
                {
                    Console.WriteLine($"ThreadPool work item {workItem} on thread {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine($"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
                    Thread.Sleep(1000);
                });
            }
            
            // Wait for thread pool work to complete
            Thread.Sleep(2000);
            
            // Method 2: Task.Run (modern approach)
            Console.WriteLine("\n2. Using Task.Run (preferred method):");
            Task[] tasks = new Task[5];
            for (int i = 0; i < tasks.Length; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(() => 
                {
                    Console.WriteLine($"Task {taskId} on thread {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine($"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
                    Thread.Sleep(1000);
                });
            }
            
            // Wait for all tasks to complete
            Task.WaitAll(tasks);
            
            Console.WriteLine("\nThread pool hygiene tips:");
            Console.WriteLine("- Keep work items short (< 100ms ideally)");
            Console.WriteLine("- Avoid blocking operations in thread pool threads");
            Console.WriteLine("- Thread pool threads are background threads");
            Console.WriteLine("- Cannot set names on thread pool threads");
            
            // Show final thread pool state
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"Available worker threads after work: {workerThreads}");
        }
    }
}
