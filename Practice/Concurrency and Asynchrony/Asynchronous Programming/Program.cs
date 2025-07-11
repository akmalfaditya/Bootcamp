using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Principles of Asynchronous Programming ===\n");
            Console.WriteLine("This comprehensive demonstration covers fundamental asynchronous programming concepts,");
            Console.WriteLine("from basic sync vs async operations to advanced patterns and language support.\n");

            // Demo 1: Fundamental difference between synchronous and asynchronous operations
            Console.WriteLine("1. Synchronous vs Asynchronous Operations");
            await DemoSynchronousVsAsynchronous();
            
            Console.WriteLine("\n" + new string('-', 70) + "\n");
            
            // Demo 2: What is asynchronous programming - design principles
            Console.WriteLine("2. Asynchronous Programming Design Principles");
            await DemoAsynchronousProgrammingPrinciples();
            
            Console.WriteLine("\n" + new string('-', 70) + "\n");
            
            // Demo 3: I/O-bound concurrency without thread blocking
            Console.WriteLine("3. I/O-Bound Concurrency Without Thread Blocking");
            await DemoIOBoundConcurrency();
            
            Console.WriteLine("\n" + new string('-', 70) + "\n");
            
            // Demo 4: Thread safety in rich-client applications
            Console.WriteLine("4. Simplified Thread Safety in Rich-Client Applications");
            await DemoRichClientThreadSafety();
            
            Console.WriteLine("\n" + new string('-', 70) + "\n");
            
            // Demo 5: Asynchronous programming and continuations
            Console.WriteLine("5. Asynchronous Programming and Continuations");
            await DemoAsynchronousAndContinuations();
            
            Console.WriteLine("\n" + new string('-', 70) + "\n");
            
            // Demo 6: Coarse-grained vs fine-grained concurrency
            Console.WriteLine("6. Coarse-Grained vs Fine-Grained Concurrency");
            await DemoCoarseVsFineGrainedConcurrency();
            
            Console.WriteLine("\n" + new string('-', 70) + "\n");
            
            // Demo 7: Why language support (async/await) is important
            Console.WriteLine("7. Why Language Support (async/await) is Important");
            await DemoLanguageSupport();
            
            Console.WriteLine("\n" + new string('-', 70) + "\n");
            
            // Demo 8: Manual state machine vs async/await
            Console.WriteLine("8. Manual State Machine vs async/await");
            await DemoManualStateMachineVsAsyncAwait();
            
            Console.WriteLine("\n=== Asynchronous Programming Demonstration Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Demo 1: Demonstrates the fundamental difference between synchronous and asynchronous operations
        static async Task DemoSynchronousVsAsynchronous()
        {
            Console.WriteLine("Understanding the core difference between synchronous and asynchronous operations:");
            
            // Synchronous operation example
            Console.WriteLine("\n--- Synchronous Operation ---");
            Console.WriteLine("A synchronous operation completes ALL its work before returning control to the caller.");
            Console.WriteLine("The calling thread remains BLOCKED until the operation concludes.");
            
            var syncStart = DateTime.Now;
            Console.WriteLine($"Starting synchronous operation at {syncStart:HH:mm:ss.fff}");
            
            // This is a synchronous operation - it blocks the calling thread
            Thread.Sleep(2000);  // Simulates work that takes 2 seconds
            
            Console.WriteLine($"Synchronous operation completed at {DateTime.Now:HH:mm:ss.fff}");
            Console.WriteLine("Notice: The calling thread was completely blocked and couldn't do anything else.");
            
            // Asynchronous operation example
            Console.WriteLine("\n--- Asynchronous Operation ---");
            Console.WriteLine("An asynchronous operation initiates its work and returns control BEFORE completion.");
            Console.WriteLine("The actual work continues in parallel, enabling non-blocking behavior.");
            
            var asyncStart = DateTime.Now;
            Console.WriteLine($"Starting asynchronous operation at {asyncStart:HH:mm:ss.fff}");
            
            // This is an asynchronous operation - it doesn't block the calling thread
            var asyncTask = DelayAsync(2000);  // Returns immediately
            
            Console.WriteLine("Asynchronous operation started - control returned immediately!");
            Console.WriteLine("Main thread can continue doing other work while waiting...");
            
            // Demonstrate that the thread is not blocked
            for (int i = 1; i <= 4; i++)
            {
                Console.WriteLine($"Main thread doing other work: step {i}");
                await Task.Delay(450);  // Non-blocking delay
            }
            
            // Wait for the async operation to complete
            await asyncTask;
            Console.WriteLine($"Asynchronous operation completed at {DateTime.Now:HH:mm:ss.fff}");
            Console.WriteLine("Key insight: The thread remained responsive throughout the operation!");
        }
        
        // Helper method that demonstrates an asynchronous operation
        static async Task DelayAsync(int milliseconds)
        {
            Console.WriteLine($"[Async Operation] Starting {milliseconds}ms operation...");
            await Task.Delay(milliseconds);
            Console.WriteLine($"[Async Operation] {milliseconds}ms operation completed");
        }

        // Demo 2: Demonstrates the fundamental principle of asynchronous programming
        static async Task DemoAsynchronousProgrammingPrinciples()
        {
            Console.WriteLine("The fundamental principle of asynchronous programming:");
            Console.WriteLine("Design long-running functions to be asynchronous from their INCEPTION,");
            Console.WriteLine("rather than wrapping synchronous functions externally.");
            
            // Traditional approach: Wrapping synchronous code externally
            Console.WriteLine("\n--- Traditional Approach (External Wrapping) ---");
            Console.WriteLine("Problem: Taking a synchronous method and wrapping it in Task.Run()");
            
            // This is NOT ideal - we're wrapping a synchronous operation
            var traditionalTask = Task.Run(() => {
                Console.WriteLine("[Traditional] Performing synchronous work on background thread");
                Thread.Sleep(1500);  // Synchronous operation
                Console.WriteLine("[Traditional] Synchronous work completed");
                return "Traditional result";
            });
            
            // Asynchronous approach: Concurrency initiated inside the function
            Console.WriteLine("\n--- Asynchronous Approach (Internal Concurrency) ---");
            Console.WriteLine("Solution: Design the function to be asynchronous from the start");
            
            // This is the proper approach - the function is asynchronous by design
            var asyncTask = ProcessDataAsynchronously();
            
            // Both approaches work, but the async approach is more scalable
            var traditionalResult = await traditionalTask;
            var asyncResult = await asyncTask;
            
            Console.WriteLine($"Traditional result: {traditionalResult}");
            Console.WriteLine($"Asynchronous result: {asyncResult}");
            
            Console.WriteLine("\nKey distinction: WHERE concurrency is initiated");
            Console.WriteLine("- Traditional: Concurrency initiated OUTSIDE the function");
            Console.WriteLine("- Asynchronous: Concurrency initiated INSIDE the function");
        }
        
        // Example of a properly designed asynchronous method
        static async Task<string> ProcessDataAsynchronously()
        {
            Console.WriteLine("[Async Method] Starting asynchronous processing...");
            
            // Simulate I/O-bound work (doesn't block threads)
            await Task.Delay(1500);
            
            Console.WriteLine("[Async Method] Asynchronous processing completed");
            return "Async result";
        }

        // Demo 3: I/O-bound concurrency without blocking threads
        static async Task DemoIOBoundConcurrency()
        {
            Console.WriteLine("Demonstrating I/O-bound concurrency without thread blocking:");
            Console.WriteLine("For I/O-heavy operations, async programming allows operations to proceed");
            Console.WriteLine("without tying up valuable threads, significantly improving scalability.");
            
            Console.WriteLine("\n--- Server-Side Application Scenario ---");
            Console.WriteLine("Simulating a web server handling multiple concurrent requests");
            
            // Simulate multiple concurrent I/O operations (like in a web server)
            var requests = new[]
            {
                SimulateWebRequest("GET /api/users", 800),
                SimulateWebRequest("GET /api/products", 1200),
                SimulateWebRequest("GET /api/orders", 600),
                SimulateWebRequest("POST /api/checkout", 1500),
                SimulateWebRequest("GET /api/inventory", 900)
            };
            
            Console.WriteLine("All requests initiated - no threads are blocked waiting for I/O!");
            
            // Process requests as they complete
            foreach (var request in requests)
            {
                var result = await request;
                Console.WriteLine($"Request completed: {result}");
            }
            
            Console.WriteLine("\nBenefit: Thousands of concurrent I/O operations can be handled");
            Console.WriteLine("without consuming thousands of threads - much more scalable!");
        }
        
        // Simulates an I/O-bound operation (network request, file access, database call)
        static async Task<string> SimulateWebRequest(string endpoint, int latencyMs)
        {
            Console.WriteLine($"[I/O Operation] Starting request to {endpoint}");
            
            // This represents I/O-bound work - the thread is released during the wait
            // In real scenarios, this would be actual network I/O, file I/O, or database calls
            await Task.Delay(latencyMs);
            
            Console.WriteLine($"[I/O Operation] Request to {endpoint} completed ({latencyMs}ms)");
            return $"Response from {endpoint}";
        }

        // Demo 4: Simplified thread safety in rich-client applications
        static async Task DemoRichClientThreadSafety()
        {
            Console.WriteLine("Demonstrating simplified thread safety in rich-client applications:");
            Console.WriteLine("Async programming minimizes code running on worker threads,");
            Console.WriteLine("keeping most logic on the main UI thread for simplified thread safety.");
            
            Console.WriteLine("\n--- Traditional Synchronous Approach ---");
            Console.WriteLine("Problem: Entire call graph must run on worker thread to avoid UI blocking");
            
            // Simulate traditional approach - entire operation on background thread
            await Task.Run(() => {
                Console.WriteLine("[Worker Thread] Starting complex UI operation");
                Console.WriteLine("[Worker Thread] Step 1: Validate user input");
                Thread.Sleep(200);
                Console.WriteLine("[Worker Thread] Step 2: Process business logic");
                Thread.Sleep(300);
                Console.WriteLine("[Worker Thread] Step 3: Calculate results");
                Thread.Sleep(400);
                Console.WriteLine("[Worker Thread] Step 4: Format output");
                Thread.Sleep(200);
                Console.WriteLine("[Worker Thread] All steps completed on worker thread");
                Console.WriteLine("[Worker Thread] Issue: UI updates require thread marshaling");
            });
            
            Console.WriteLine("\n--- Asynchronous Approach ---");
            Console.WriteLine("Solution: Only I/O operations use background threads, UI logic stays on main thread");
            
            // Simulate asynchronous approach - minimal worker thread usage
            await ProcessUIOperationAsync();
            
            Console.WriteLine("\nKey advantage: Fine-grained concurrency");
            Console.WriteLine("- Only the actual I/O operations use background threads");
            Console.WriteLine("- UI updates and business logic remain on the main thread");
            Console.WriteLine("- Thread safety is greatly simplified");
        }
        
        // Example of an async method that keeps most logic on the main thread
        static async Task ProcessUIOperationAsync()
        {
            Console.WriteLine("[Main Thread] Starting UI operation");
            Console.WriteLine("[Main Thread] Step 1: Validate user input");
            
            // Only the actual I/O operation uses a background thread
            var data = await LoadDataAsync();
            
            Console.WriteLine("[Main Thread] Step 2: Process business logic");
            var processedData = data.ToUpper();
            
            Console.WriteLine("[Main Thread] Step 3: Calculate results");
            var results = $"Processed: {processedData}";
            
            Console.WriteLine("[Main Thread] Step 4: Update UI");
            Console.WriteLine($"[Main Thread] UI Updated with: {results}");
            Console.WriteLine("[Main Thread] All UI operations completed on main thread - no marshaling needed!");
        }
        
        // This represents an I/O-bound operation that properly uses background threads
        static async Task<string> LoadDataAsync()
        {
            Console.WriteLine("[I/O Operation] Loading data from external source...");
            await Task.Delay(500);  // Simulate I/O latency
            Console.WriteLine("[I/O Operation] Data loaded successfully");
            return "Sample data from external source";
        }

        // Demo 5: Asynchronous programming and continuations
        static async Task DemoAsynchronousAndContinuations()
        {
            Console.WriteLine("Demonstrating asynchronous programming with continuations:");
            Console.WriteLine("The Task class is ideally suited for async programming due to its");
            Console.WriteLine("robust support for continuations - defining what happens after completion.");
            
            Console.WriteLine("\n--- TaskCompletionSource for I/O-Bound Operations ---");
            Console.WriteLine("For I/O-bound operations, TaskCompletionSource creates tasks that");
            Console.WriteLine("complete based on external events, not thread execution.");
            
            // Demonstrate TaskCompletionSource (the foundation of async I/O)
            await DemoTaskCompletionSource();
            
            Console.WriteLine("\n--- Task.Run for Compute-Bound Operations ---");
            Console.WriteLine("For compute-bound operations, Task.Run offloads work to background threads.");
            
            // Demonstrate Task.Run for CPU-bound work
            var computeResult = await PerformComputeWorkAsync();
            Console.WriteLine($"Compute result: {computeResult}");
            
            Console.WriteLine("\n--- Continuation Chaining ---");
            Console.WriteLine("Continuations allow you to chain operations without blocking threads.");
            
            // Demonstrate continuation chaining
            await DemoContinuationChaining();
        }
        
        // Demonstrates TaskCompletionSource for I/O-bound operations
        static async Task DemoTaskCompletionSource()
        {
            Console.WriteLine("[TaskCompletionSource] Creating a task that completes based on external event");
            
            // Create a TaskCompletionSource - this is how async I/O operations work internally
            var tcs = new TaskCompletionSource<string>();
            
            // Simulate an external event (like a network response) completing the task
            _ = Task.Run(async () => {
                Console.WriteLine("[External Event] Simulating external event (network response, file I/O, etc.)");
                await Task.Delay(1000);  // Simulate external latency
                Console.WriteLine("[External Event] External event completed");
                tcs.SetResult("Data from external event");
            });
            
            Console.WriteLine("[Main Thread] Task created, waiting for external event...");
            
            // Wait for the external event to complete the task
            var result = await tcs.Task;
            Console.WriteLine($"[Main Thread] Received result: {result}");
            Console.WriteLine("[Main Thread] No thread was blocked waiting for the external event!");
        }
        
        // Demonstrates Task.Run for compute-bound operations
        static async Task<int> PerformComputeWorkAsync()
        {
            Console.WriteLine("[Compute Operation] Starting CPU-intensive work on background thread");
            
            // Use Task.Run to offload CPU-bound work to thread pool
            return await Task.Run(() => {
                Console.WriteLine("[Background Thread] Performing CPU-intensive calculation");
                
                // Simulate CPU-intensive work
                int result = 0;
                for (int i = 0; i < 1000000; i++)
                {
                    result += i % 1000;
                }
                
                Console.WriteLine("[Background Thread] CPU-intensive calculation completed");
                return result;
            });
        }
        
        // Demonstrates continuation chaining
        static async Task DemoContinuationChaining()
        {
            Console.WriteLine("[Continuation Chain] Starting chained async operations");
            
            // Each operation in the chain can be async
            var step1Result = await Step1Async();
            var step2Result = await Step2Async(step1Result);
            var step3Result = await Step3Async(step2Result);
            
            Console.WriteLine($"[Continuation Chain] Final result: {step3Result}");
        }
        
        static async Task<string> Step1Async()
        {
            Console.WriteLine("[Step 1] Processing first step");
            await Task.Delay(300);
            return "Step1 completed";
        }
        
        static async Task<string> Step2Async(string input)
        {
            Console.WriteLine($"[Step 2] Processing second step with input: {input}");
            await Task.Delay(400);
            return $"Step2 processed: {input}";
        }
        
        static async Task<string> Step3Async(string input)
        {
            Console.WriteLine($"[Step 3] Processing final step with input: {input}");
            await Task.Delay(200);
            return $"Step3 finalized: {input}";
        }

        // Demo 6: Coarse-grained vs fine-grained concurrency
        static async Task DemoCoarseVsFineGrainedConcurrency()
        {
            Console.WriteLine("Demonstrating coarse-grained vs fine-grained concurrency:");
            Console.WriteLine("This shows the difference between traditional and async approaches");
            Console.WriteLine("using the prime calculation example from the material.");
            
            Console.WriteLine("\n--- Traditional Approach: Coarse-Grained Concurrency ---");
            Console.WriteLine("Problem: Entire call graph must run on background thread");
            
            // Traditional approach - wrap entire operation
            await Task.Run(() => DisplayPrimeCountsSync());
            
            Console.WriteLine("\n--- Asynchronous Approach: Fine-Grained Concurrency ---");
            Console.WriteLine("Solution: Only the actual computation uses background threads");
            
            // Asynchronous approach - concurrency initiated inside the compute method
            await DisplayPrimeCountsAsync();
            
            Console.WriteLine("\nKey insight: Async approach allows the calling method to remain");
            Console.WriteLine("on the main thread, with concurrency initiated only where needed.");
        }
        
        // Traditional synchronous approach - entire method blocks
        static void DisplayPrimeCountsSync()
        {
            Console.WriteLine("[Sync Method] Starting synchronous prime calculation");
            
            for (int i = 0; i < 3; i++)  // Reduced for demo purposes
            {
                Console.WriteLine($"[Sync Method] Calculating primes for range {i}...");
                
                // This blocks the calling thread
                int primeCount = GetPrimesCount(i * 10000 + 2, 10000);
                Console.WriteLine($"[Sync Method] {primeCount} primes found in range {i}");
            }
            
            Console.WriteLine("[Sync Method] Done - entire operation ran on background thread");
        }
        
        // Asynchronous approach - fine-grained concurrency
        static async Task DisplayPrimeCountsAsync()
        {
            Console.WriteLine("[Async Method] Starting asynchronous prime calculation");
            
            for (int i = 0; i < 3; i++)  // Reduced for demo purposes
            {
                Console.WriteLine($"[Async Method] Calculating primes for range {i}...");
                
                // This doesn't block - concurrency initiated inside GetPrimesCountAsync
                int primeCount = await GetPrimesCountAsync(i * 10000 + 2, 10000);
                Console.WriteLine($"[Async Method] {primeCount} primes found in range {i}");
            }
            
            Console.WriteLine("[Async Method] Done - main logic stayed on calling thread");
        }
        
        // Synchronous prime calculation (from the material)
        static int GetPrimesCount(int start, int count)
        {
            Console.WriteLine($"[Prime Calc] Computing primes from {start} to {start + count - 1}");
            
            // This is the CPU-intensive calculation from the material
            // Note: Simplified for demo performance
            return Enumerable.Range(start, Math.Min(count, 1000))
                .AsParallel()
                .Count(n => IsPrime(n));
        }
        
        // Asynchronous prime calculation (from the material)
        static async Task<int> GetPrimesCountAsync(int start, int count)
        {
            Console.WriteLine($"[Prime Calc Async] Starting async computation from {start} to {start + count - 1}");
            
            // This creates an asynchronous version by offloading to thread pool
            return await Task.Run(() => {
                Console.WriteLine($"[Background Thread] Computing primes from {start} to {start + count - 1}");
                
                // Same calculation as sync version, but on background thread
                return Enumerable.Range(start, Math.Min(count, 1000))
                    .AsParallel()
                    .Count(n => IsPrime(n));
            });
        }
        
        // Helper method to check if a number is prime
        static bool IsPrime(int n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            
            for (int i = 3; i <= Math.Sqrt(n); i += 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        // Demo 7: Why language support (async/await) is important
        static async Task DemoLanguageSupport()
        {
            Console.WriteLine("Demonstrating why async/await language support is crucial:");
            Console.WriteLine("Without async/await, even simple sequential async operations");
            Console.WriteLine("become complex and error-prone.");
            
            Console.WriteLine("\n--- The Problem: Manual Continuation Management ---");
            Console.WriteLine("Imagine trying to serialize async operations manually...");
            
            // Show the complexity of manual continuation management
            await DemoManualContinuations();
            
            Console.WriteLine("\n--- The Solution: async/await Syntax ---");
            Console.WriteLine("With async/await, complex async flows become simple and readable:");
            
            // Show how async/await simplifies the same operations
            await DemoSimpleAsyncAwait();
            
            Console.WriteLine("\nKey benefit: async/await allows you to write asynchronous code");
            Console.WriteLine("that looks and feels like synchronous code, but with async benefits!");
        }
        
        // Demonstrates the complexity of manual continuation management
        static async Task DemoManualContinuations()
        {
            Console.WriteLine("[Manual Continuations] This is what you would need to do manually:");
            Console.WriteLine("1. Create TaskCompletionSource for each step");
            Console.WriteLine("2. Manage state between continuations");
            Console.WriteLine("3. Handle exceptions in each continuation");
            Console.WriteLine("4. Chain continuations together");
            
            // This is intentionally complex to show the problem
            var tcs = new TaskCompletionSource<string>();
            
            // First operation
            var task1 = SimulateAsyncOperation("Operation 1", 500);
            _ = task1.ContinueWith(t1 => {
                if (t1.IsCompletedSuccessfully)
                {
                    Console.WriteLine($"[Manual] First operation completed: {t1.Result}");
                    
                    // Second operation
                    var task2 = SimulateAsyncOperation("Operation 2", 300);
                    _ = task2.ContinueWith(t2 => {
                        if (t2.IsCompletedSuccessfully)
                        {
                            Console.WriteLine($"[Manual] Second operation completed: {t2.Result}");
                            tcs.SetResult("Manual continuations completed");
                        }
                        else
                        {
                            tcs.SetException(t2.Exception ?? new Exception("Unknown error"));
                        }
                    });
                }
                else
                {
                    tcs.SetException(t1.Exception ?? new Exception("Unknown error"));
                }
            });
            
            var result = await tcs.Task;
            Console.WriteLine($"[Manual] Final result: {result}");
            Console.WriteLine("[Manual] Notice how complex this is for just 2 sequential operations!");
        }
        
        // Demonstrates how async/await simplifies the same operations
        static async Task DemoSimpleAsyncAwait()
        {
            Console.WriteLine("[async/await] Same operations with async/await:");
            
            // With async/await, this is simple and readable
            var result1 = await SimulateAsyncOperation("Operation 1", 500);
            Console.WriteLine($"[async/await] First operation completed: {result1}");
            
            var result2 = await SimulateAsyncOperation("Operation 2", 300);
            Console.WriteLine($"[async/await] Second operation completed: {result2}");
            
            Console.WriteLine("[async/await] Sequential async operations are now simple!");
            Console.WriteLine("[async/await] Exception handling, state management, and continuations");
            Console.WriteLine("[async/await] are all handled automatically by the compiler!");
        }
        
        // Helper method for async operation simulation
        static async Task<string> SimulateAsyncOperation(string operationName, int delayMs)
        {
            Console.WriteLine($"[{operationName}] Starting...");
            await Task.Delay(delayMs);
            Console.WriteLine($"[{operationName}] Completed");
            return $"Result from {operationName}";
        }

        // Demo 8: Manual state machine vs async/await
        static async Task DemoManualStateMachineVsAsyncAwait()
        {
            Console.WriteLine("Demonstrating manual state machine vs async/await:");
            Console.WriteLine("This shows the complexity that async/await hides from developers.");
            
            Console.WriteLine("\n--- What async/await does behind the scenes ---");
            Console.WriteLine("The compiler generates a state machine to manage async operations");
            Console.WriteLine("Here's a simplified example of what it looks like:");
            
            // Demonstrate manual state machine pattern
            await DemoManualStateMachine();
            
            Console.WriteLine("\n--- What you actually write with async/await ---");
            Console.WriteLine("The same logic expressed naturally:");
            
            // Show the equivalent async/await code
            await DemoEquivalentAsyncAwait();
            
            Console.WriteLine("\nThe async/await keywords are syntactic sugar that generate");
            Console.WriteLine("complex state machine code automatically, making async programming");
            Console.WriteLine("accessible to all developers without requiring deep understanding");
            Console.WriteLine("of continuation-passing style programming.");
        }
        
        // Demonstrates a manual state machine (simplified version of what compiler generates)
        static async Task DemoManualStateMachine()
        {
            Console.WriteLine("[Manual State Machine] Simulating compiler-generated state machine:");
            
            var stateMachine = new ManualStateMachine();
            await stateMachine.ExecuteAsync();
        }
        
        // Simplified manual state machine
        class ManualStateMachine
        {
            private int state = 0;
            private TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            
            public async Task ExecuteAsync()
            {
                Console.WriteLine("[State Machine] State 0: Starting");
                await MoveToNextState();
                await tcs.Task;
            }
            
            private async Task MoveToNextState()
            {
                switch (state)
                {
                    case 0:
                        Console.WriteLine("[State Machine] State 0: Performing first async operation");
                        state = 1;
                        var task1 = Task.Delay(500);
                        _ = task1.ContinueWith(_ => MoveToNextState());
                        await task1;
                        break;
                    
                    case 1:
                        Console.WriteLine("[State Machine] State 1: Performing second async operation");
                        state = 2;
                        var task2 = Task.Delay(300);
                        _ = task2.ContinueWith(_ => MoveToNextState());
                        await task2;
                        break;
                    
                    case 2:
                        Console.WriteLine("[State Machine] State 2: Completing");
                        tcs.SetResult(true);
                        break;
                }
            }
        }
        
        // Shows the equivalent async/await code
        static async Task DemoEquivalentAsyncAwait()
        {
            Console.WriteLine("[async/await] Starting equivalent async/await version:");
            
            Console.WriteLine("[async/await] Performing first async operation");
            await Task.Delay(500);
            
            Console.WriteLine("[async/await] Performing second async operation");
            await Task.Delay(300);
            
            Console.WriteLine("[async/await] Completing");
            
            Console.WriteLine("[async/await] Notice how much simpler this is!");
            Console.WriteLine("[async/await] The compiler handles all the state management automatically.");
        }
    }
}
