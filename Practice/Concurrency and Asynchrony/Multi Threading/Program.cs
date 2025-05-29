using System;
using System.Threading;

namespace MultiThreadingMasterclass
{
    class Program
    {
        // Shared variables for demonstrating synchronization
        private static int sharedCounter = 0;
        private static readonly object lockObject = new object();
        
        // Objects for deadlock demonstration
        private static readonly object lock1 = new object();
        private static readonly object lock2 = new object();
        
        // Events for thread signaling
        private static AutoResetEvent autoEvent = new AutoResetEvent(false);
        
        // Semaphore for controlling concurrent access
        private static Semaphore semaphore = new Semaphore(2, 2);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Multithreading Masterclass ===");
            Console.WriteLine("This demo covers all essential threading concepts in C#\n");

            RunDemo("1. Sequential vs Concurrent Execution", DemoSequentialVsConcurrent);
            RunDemo("2. Thread Creation and Naming", DemoThreadCreationAndNaming);
            RunDemo("3. Thread Delegates and Lambdas", DemoThreadDelegatesAndLambdas);
            RunDemo("4. Thread Joining and IsAlive", DemoThreadJoiningAndIsAlive);
            RunDemo("5. Foreground vs Background Threads", DemoForegroundVsBackground);
            RunDemo("6. Thread Priority (Use with Caution)", DemoThreadPriority);
            RunDemo("7. Race Conditions and Locks", DemoRaceConditionsAndLocks);
            RunDemo("8. Deadlock Prevention", DemoDeadlockPrevention);
            RunDemo("9. Monitor and Advanced Locking", DemoMonitorAndAdvancedLocking);
            RunDemo("10. Mutex for Inter-Process Sync", DemoMutex);
            RunDemo("11. Semaphore for Resource Limiting", DemoSemaphore);
            RunDemo("12. AutoResetEvent for Signaling", DemoAutoResetEvent);
            RunDemo("13. Thread Pool Efficiency", DemoThreadPool);

            Console.WriteLine("\n=== Masterclass Complete ===");
            Console.WriteLine("You've seen all the major threading concepts in action!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Helper method to run demonstrations with clear separation
        static void RunDemo(string title, Action demoMethod)
        {
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));
            
            try
            {
                demoMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Demo error: {ex.Message}");
            }
            
            Console.WriteLine("\nPress Enter to continue to next demo...");
            Console.ReadLine();
        }

        // Demo 1: Shows the difference between sequential and concurrent execution
        static void DemoSequentialVsConcurrent()
        {
            Console.WriteLine("First, let's see sequential execution:");
            var startTime = DateTime.Now;
            
            // Sequential execution - one method after another
            Method1();
            Method2();
            Method3();
            
            var sequentialTime = DateTime.Now - startTime;
            Console.WriteLine($"Sequential execution took: {sequentialTime.TotalSeconds:F2} seconds");
            
            Console.WriteLine("\nNow let's see concurrent execution:");
            startTime = DateTime.Now;
            
            // Concurrent execution - all methods run simultaneously
            Thread t1 = new Thread(Method1);
            Thread t2 = new Thread(Method2);
            Thread t3 = new Thread(Method3);
            
            t1.Start();
            t2.Start();
            t3.Start();
            
            // Wait for all threads to complete
            t1.Join();
            t2.Join();
            t3.Join();
            
            var concurrentTime = DateTime.Now - startTime;
            Console.WriteLine($"Concurrent execution took: {concurrentTime.TotalSeconds:F2} seconds");
            Console.WriteLine("Notice how concurrent execution is faster when methods can run in parallel!");
        }

        // These methods simulate different types of work
        static void Method1()
        {
            Console.WriteLine($"Method1 started on {GetThreadInfo()}");
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"Method1: Step {i}");
                Thread.Sleep(500); // Simulate some work
            }
            Console.WriteLine($"Method1 completed on {GetThreadInfo()}");
        }

        static void Method2()
        {
            Console.WriteLine($"Method2 started on {GetThreadInfo()}");
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"Method2: Step {i}");
                if (i == 2)
                {
                    Console.WriteLine("Method2: Simulating database operation...");
                    Thread.Sleep(2000); // Simulate long database operation
                    Console.WriteLine("Method2: Database operation completed");
                }
                else
                {
                    Thread.Sleep(300);
                }
            }
            Console.WriteLine($"Method2 completed on {GetThreadInfo()}");
        }

        static void Method3()
        {
            Console.WriteLine($"Method3 started on {GetThreadInfo()}");
            for (int i = 1; i <= 4; i++)
            {
                Console.WriteLine($"Method3: Processing item {i}");
                Thread.Sleep(400); // Simulate processing
            }
            Console.WriteLine($"Method3 completed on {GetThreadInfo()}");
        }

        // Demo 2: Thread creation with proper naming for debugging
        static void DemoThreadCreationAndNaming()
        {
            Console.WriteLine("Creating threads with meaningful names helps during debugging:");
            
            // Create threads with descriptive names
            Thread uiThread = new Thread(SimulateUIWork) { Name = "UI-UpdateThread" };
            Thread dataThread = new Thread(SimulateDataProcessing) { Name = "DataProcessing-Thread" };
            Thread logThread = new Thread(SimulateLogging) { Name = "Logging-Thread" };
            
            Console.WriteLine("Starting named threads...");
            uiThread.Start();
            dataThread.Start();
            logThread.Start();
            
            // Show main thread info
            Console.WriteLine($"Main thread: {Thread.CurrentThread.Name ?? "Main"}");
            
            // Wait for all to complete
            uiThread.Join();
            dataThread.Join();
            logThread.Join();
            
            Console.WriteLine("All named threads completed!");
        }

        static void SimulateUIWork()
        {
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Updating UI element {i}");
                Thread.Sleep(600);
            }
        }

        static void SimulateDataProcessing()
        {
            for (int i = 1; i <= 4; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Processing data chunk {i}");
                Thread.Sleep(400);
            }
        }

        static void SimulateLogging()
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Writing log entry {i}");
                Thread.Sleep(300);
            }
        }

        // Demo 3: Different ways to create threads using delegates and lambdas
        static void DemoThreadDelegatesAndLambdas()
        {
            Console.WriteLine("Exploring different thread creation patterns:");
            
            // Method 1: Traditional ThreadStart delegate (verbose but explicit)
            Console.WriteLine("\n1. Using ThreadStart delegate:");
            ThreadStart threadStart = new ThreadStart(DisplayNumbers);
            Thread delegateThread = new Thread(threadStart) { Name = "DelegateThread" };
            delegateThread.Start();
            delegateThread.Join();
            
            // Method 2: Direct method reference (cleaner)
            Console.WriteLine("\n2. Using method reference:");
            Thread methodRefThread = new Thread(DisplayNumbers) { Name = "MethodRefThread" };
            methodRefThread.Start();
            methodRefThread.Join();
            
            // Method 3: Lambda expression (most flexible)
            Console.WriteLine("\n3. Using lambda expression:");
            Thread lambdaThread = new Thread(() =>
            {
                Console.WriteLine($"Lambda thread {Thread.CurrentThread.Name} executing custom logic");
                for (int i = 1; i <= 3; i++)
                {
                    Console.WriteLine($"Lambda: {i}");
                    Thread.Sleep(200);
                }
            }) { Name = "LambdaThread" };
            lambdaThread.Start();
            lambdaThread.Join();
            
            // Method 4: Lambda with captured variables (closure)
            string message = "Hello from closure!";
            Thread closureThread = new Thread(() =>
            {
                Console.WriteLine($"Closure thread: {message}");
            }) { Name = "ClosureThread" };
            closureThread.Start();
            closureThread.Join();
        }

        static void DisplayNumbers()
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Number: {i}");
                Thread.Sleep(150);
            }
        }

        // Demo 4: Thread joining and checking thread status
        static void DemoThreadJoiningAndIsAlive()
        {
            Console.WriteLine("Demonstrating thread joining and IsAlive property:");
            
            Thread workerThread = new Thread(LongRunningTask) { Name = "WorkerThread" };
            
            Console.WriteLine($"Before Start - IsAlive: {workerThread.IsAlive}");
            Console.WriteLine($"Thread State: {workerThread.ThreadState}");
            
            workerThread.Start();
            Console.WriteLine($"After Start - IsAlive: {workerThread.IsAlive}");
            Console.WriteLine($"Thread State: {workerThread.ThreadState}");
            
            // Check status while running
            Thread.Sleep(1000);
            Console.WriteLine($"While Running - IsAlive: {workerThread.IsAlive}");
            Console.WriteLine($"Thread State: {workerThread.ThreadState}");
            
            Console.WriteLine("Main thread is now waiting for worker to finish (Join)...");
            workerThread.Join(); // This blocks until workerThread completes
            
            Console.WriteLine($"After Join - IsAlive: {workerThread.IsAlive}");
            Console.WriteLine($"Thread State: {workerThread.ThreadState}");
            Console.WriteLine("Worker thread has finished, main thread continues!");
        }

        static void LongRunningTask()
        {
            Console.WriteLine($"[{Thread.CurrentThread.Name}] Starting long task...");
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Working... {i}/5");
                Thread.Sleep(800);
            }
            Console.WriteLine($"[{Thread.CurrentThread.Name}] Task completed!");
        }

        // Demo 5: Foreground vs Background threads behavior
        static void DemoForegroundVsBackground()
        {
            Console.WriteLine("Comparing foreground and background thread behavior:");
            
            // Foreground thread (default) - keeps application alive
            Thread foregroundThread = new Thread(CountDown)
            {
                Name = "ForegroundCounter",
                IsBackground = false // This is the default
            };
            
            // Background thread - doesn't prevent app from closing
            Thread backgroundThread = new Thread(CountDown)
            {
                Name = "BackgroundCounter",
                IsBackground = true // This makes it a background thread
            };
            
            Console.WriteLine("Starting both threads...");
            Console.WriteLine("Foreground thread will keep the demo running");
            Console.WriteLine("Background thread will be terminated if no foreground threads remain");
            
            foregroundThread.Start();
            backgroundThread.Start();
            
            // Let them run for a bit
            Thread.Sleep(2000);
            
            Console.WriteLine("\nThread properties:");
            Console.WriteLine($"Foreground thread - IsBackground: {foregroundThread.IsBackground}");
            Console.WriteLine($"Background thread - IsBackground: {backgroundThread.IsBackground}");
            
            // Wait for foreground thread to complete
            // Background thread may or may not complete depending on timing
            foregroundThread.Join();
            
            Console.WriteLine("Foreground thread completed. Background thread may have been terminated.");
        }

        static void CountDown()
        {
            string threadType = Thread.CurrentThread.IsBackground ? "Background" : "Foreground";
            
            for (int i = 5; i >= 1; i--)
            {
                Console.WriteLine($"[{threadType}-{Thread.CurrentThread.Name}] Countdown: {i}");
                Thread.Sleep(800);
            }
            Console.WriteLine($"[{threadType}-{Thread.CurrentThread.Name}] Countdown finished!");
        }

        // Demo 6: Thread priority (use sparingly!)
        static void DemoThreadPriority()
        {
            Console.WriteLine("Demonstrating thread priority (use with extreme caution!):");
            Console.WriteLine("Priority affects scheduling but doesn't guarantee execution order");
            
            // Create threads with different priorities
            Thread lowPriorityThread = new Thread(PriorityWork)
            {
                Name = "LowPriority",
                Priority = ThreadPriority.Lowest
            };
            
            Thread normalPriorityThread = new Thread(PriorityWork)
            {
                Name = "NormalPriority",
                Priority = ThreadPriority.Normal
            };
            
            Thread highPriorityThread = new Thread(PriorityWork)
            {
                Name = "HighPriority",
                Priority = ThreadPriority.Highest
            };
            
            Console.WriteLine("Starting threads with different priorities...");
            
            // Start all threads
            lowPriorityThread.Start();
            normalPriorityThread.Start();
            highPriorityThread.Start();
            
            // Wait for completion
            lowPriorityThread.Join();
            normalPriorityThread.Join();
            highPriorityThread.Join();
            
            Console.WriteLine("\nRemember: Thread priority is a hint to the OS scheduler");
            Console.WriteLine("Actual behavior depends on system load and OS implementation");
        }

        static void PriorityWork()
        {
            string threadName = Thread.CurrentThread.Name;
            ThreadPriority priority = Thread.CurrentThread.Priority;
            
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"[{threadName}({priority})] Working: {i}");
                // Use SpinWait instead of Sleep to keep thread active and show priority effects
                Thread.SpinWait(10000000);
            }
        }

        // Demo 7: Race conditions and how to fix them with locks
        static void DemoRaceConditionsAndLocks()
        {
            Console.WriteLine("Demonstrating race conditions and their prevention:");
            
            // First, show unsafe increment (race condition)
            Console.WriteLine("\n--- Unsafe Increment (Race Condition) ---");
            sharedCounter = 0;
            
            Thread[] unsafeThreads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                unsafeThreads[i] = new Thread(UnsafeIncrement);
                unsafeThreads[i].Start();
            }
            
            // Wait for all unsafe threads
            foreach (Thread t in unsafeThreads)
                t.Join();
            
            Console.WriteLine($"Unsafe result: {sharedCounter} (should be 5000, but probably isn't!)");
            Console.WriteLine("This happens because multiple threads modify the same variable simultaneously");
            
            // Now show safe increment with lock
            Console.WriteLine("\n--- Safe Increment (With Lock) ---");
            sharedCounter = 0;
            
            Thread[] safeThreads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                safeThreads[i] = new Thread(SafeIncrement);
                safeThreads[i].Start();
            }
            
            // Wait for all safe threads
            foreach (Thread t in safeThreads)
                t.Join();
            
            Console.WriteLine($"Safe result: {sharedCounter} (should be exactly 5000)");
            Console.WriteLine("The lock ensures only one thread can modify the counter at a time");
        }

        static void UnsafeIncrement()
        {
            // This is NOT thread-safe - multiple threads can interfere with each other
            for (int i = 0; i < 1000; i++)
            {
                sharedCounter++; // Race condition here!
            }
        }

        static void SafeIncrement()
        {
            // This IS thread-safe - lock prevents race conditions
            for (int i = 0; i < 1000; i++)
            {
                lock (lockObject)
                {
                    sharedCounter++; // Protected by lock
                }
            }
        }

        // Demo 8: Deadlock demonstration and prevention
        static void DemoDeadlockPrevention()
        {
            Console.WriteLine("Understanding deadlocks and how to prevent them:");
            Console.WriteLine("A deadlock occurs when threads wait for each other indefinitely");
            
            Console.WriteLine("\nShowing proper lock ordering to prevent deadlocks:");
            
            // Start two threads that could deadlock, but won't due to consistent ordering
            Thread thread1 = new Thread(SafeLockingMethod1) { Name = "Thread1" };
            Thread thread2 = new Thread(SafeLockingMethod2) { Name = "Thread2" };
            
            thread1.Start();
            thread2.Start();
            
            thread1.Join();
            thread2.Join();
            
            Console.WriteLine("Both threads completed successfully - no deadlock!");
            Console.WriteLine("Key: Always acquire locks in the same order across all threads");
        }

        // These methods demonstrate safe locking patterns
        static void SafeLockingMethod1()
        {
            Console.WriteLine($"[{Thread.CurrentThread.Name}] Attempting to acquire lock1...");
            lock (lock1) // Always acquire lock1 first
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Acquired lock1");
                Thread.Sleep(100);
                
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Attempting to acquire lock2...");
                lock (lock2) // Then lock2
                {
                    Console.WriteLine($"[{Thread.CurrentThread.Name}] Acquired lock2 - doing work");
                    Thread.Sleep(100);
                }
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Released lock2");
            }
            Console.WriteLine($"[{Thread.CurrentThread.Name}] Released lock1");
        }

        static void SafeLockingMethod2()
        {
            Console.WriteLine($"[{Thread.CurrentThread.Name}] Attempting to acquire lock1...");
            lock (lock1) // Same order: lock1 first
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Acquired lock1");
                Thread.Sleep(100);
                
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Attempting to acquire lock2...");
                lock (lock2) // Then lock2
                {
                    Console.WriteLine($"[{Thread.CurrentThread.Name}] Acquired lock2 - doing work");
                    Thread.Sleep(100);
                }
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Released lock2");
            }
            Console.WriteLine($"[{Thread.CurrentThread.Name}] Released lock1");
        }

        // Demo 9: Monitor class for advanced locking scenarios
        static void DemoMonitorAndAdvancedLocking()
        {
            Console.WriteLine("Using Monitor for advanced locking scenarios:");
            Console.WriteLine("Monitor provides more control than the lock statement");
            
            Thread[] threads = new Thread[3];
            
            for (int i = 0; i < 3; i++)
            {
                int threadNum = i + 1;
                threads[i] = new Thread(() => MonitorExample(threadNum));
                threads[i].Start();
            }
            
            foreach (Thread t in threads)
                t.Join();
        }

        static void MonitorExample(int threadNumber)
        {
            Console.WriteLine($"[Thread{threadNumber}] Attempting to enter critical section...");
            
            // TryEnter allows timeout and non-blocking attempts
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(lockObject, TimeSpan.FromSeconds(2), ref lockTaken);
                
                if (lockTaken)
                {
                    Console.WriteLine($"[Thread{threadNumber}] Entered critical section");
                    Thread.Sleep(1000); // Simulate work
                    Console.WriteLine($"[Thread{threadNumber}] Completed work in critical section");
                }
                else
                {
                    Console.WriteLine($"[Thread{threadNumber}] Failed to enter critical section (timeout)");
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(lockObject);
                    Console.WriteLine($"[Thread{threadNumber}] Exited critical section");
                }
            }
        }

        // Demo 10: Mutex for inter-process synchronization
        static void DemoMutex()
        {
            Console.WriteLine("Demonstrating Mutex for inter-process synchronization:");
            Console.WriteLine("Mutex can coordinate between different applications");
            
            const string mutexName = "ThreadingDemo_Mutex";
            
            using (Mutex mutex = new Mutex(false, mutexName))
            {
                Console.WriteLine("Attempting to acquire mutex...");
                
                if (mutex.WaitOne(TimeSpan.FromSeconds(3)))
                {
                    try
                    {
                        Console.WriteLine("Mutex acquired! This code section is exclusive across all processes");
                        Console.WriteLine("Try running another instance of this program - it won't proceed here");
                        Thread.Sleep(2000);
                        Console.WriteLine("Releasing mutex...");
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
                else
                {
                    Console.WriteLine("Could not acquire mutex (another process may be holding it)");
                }
            }
        }

        // Demo 11: Semaphore for limiting concurrent access
        static void DemoSemaphore()
        {
            Console.WriteLine("Using Semaphore to limit concurrent resource access:");
            Console.WriteLine("This semaphore allows only 2 threads to access the resource simultaneously");
            
            Thread[] threads = new Thread[5];
            
            for (int i = 0; i < 5; i++)
            {
                int threadNum = i + 1;
                threads[i] = new Thread(() => AccessLimitedResource(threadNum));
                threads[i].Start();
            }
            
            foreach (Thread t in threads)
                t.Join();
        }

        static void AccessLimitedResource(int threadNumber)
        {
            Console.WriteLine($"[Thread{threadNumber}] Waiting for resource access...");
            
            semaphore.WaitOne(); // Wait for available slot
            
            try
            {
                Console.WriteLine($"[Thread{threadNumber}] Accessing limited resource");
                Thread.Sleep(2000); // Simulate resource usage
                Console.WriteLine($"[Thread{threadNumber}] Finished using resource");
            }
            finally
            {
                semaphore.Release(); // Release the slot for other threads
            }
        }

        // Demo 12: AutoResetEvent for thread signaling
        static void DemoAutoResetEvent()
        {
            Console.WriteLine("Using AutoResetEvent for thread signaling:");
            Console.WriteLine("One thread signals another when work is ready");
            
            // Start worker thread that waits for signals
            Thread workerThread = new Thread(WaitForWork) { Name = "WorkerThread" };
            workerThread.Start();
            
            // Main thread sends signals
            for (int i = 1; i <= 3; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"[MainThread] Sending work signal {i}...");
                autoEvent.Set(); // Signal the worker
            }
            
            Thread.Sleep(1000);
            Console.WriteLine("[MainThread] Stopping worker...");
            // Send stop signal by setting a flag and signaling
            autoEvent.Set();
            
            workerThread.Join();
        }

        static void WaitForWork()
        {
            int workCount = 0;
            
            while (workCount < 3)
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Waiting for work signal...");
                autoEvent.WaitOne(); // Block until signaled
                
                workCount++;
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Received work signal! Processing work {workCount}...");
                Thread.Sleep(500); // Simulate work
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Work {workCount} completed");
            }
            
            Console.WriteLine($"[{Thread.CurrentThread.Name}] All work completed, exiting");
        }

        // Demo 13: Thread Pool for efficient thread management
        static void DemoThreadPool()
        {
            Console.WriteLine("Demonstrating Thread Pool efficiency:");
            Console.WriteLine("Thread pool reuses threads instead of creating new ones");
            
            // Show thread pool info
            int workerThreads, completionPortThreads;
            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"Max worker threads: {workerThreads}");
            
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"Available worker threads: {workerThreads}");
            
            Console.WriteLine("\nQueueing work items to thread pool:");
            
            // Queue multiple work items
            for (int i = 1; i <= 5; i++)
            {
                int workItemId = i;
                ThreadPool.QueueUserWorkItem(ThreadPoolWork, workItemId);
            }
            
            // Give thread pool time to process
            Thread.Sleep(3000);
            
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"Available worker threads after work: {workerThreads}");
        }

        static void ThreadPoolWork(object state)
        {
            int workItemId = (int)state;
            Thread currentThread = Thread.CurrentThread;
            
            Console.WriteLine($"[WorkItem{workItemId}] Executing on thread {currentThread.ManagedThreadId}");
            Console.WriteLine($"[WorkItem{workItemId}] IsBackground: {currentThread.IsBackground}, IsThreadPoolThread: {currentThread.IsThreadPoolThread}");
            
            // Simulate work
            Thread.Sleep(1000);
            
            Console.WriteLine($"[WorkItem{workItemId}] Completed on thread {currentThread.ManagedThreadId}");
        }

        // Helper method to get thread information
        static string GetThreadInfo()
        {
            Thread current = Thread.CurrentThread;
            return $"Thread {current.ManagedThreadId} ({current.Name ?? "Unnamed"})";
        }
    }
}
