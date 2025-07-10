// Timer Memory Leak Examples
// These classes demonstrate how different timer types can cause memory leaks
// They show the exact patterns described in the training material

using System.Timers;

namespace ManagedMemoryLeaks
{
    // BAD EXAMPLE: System.Timers.Timer keeps object alive - memory leak!
    // This demonstrates the exact problem described in the material
    public class LeakyTimerObject
    {
        private readonly int _id;
        private readonly System.Timers.Timer _timer;
        private readonly byte[] _largeData;
        private int _tickCount = 0;
        
        public LeakyTimerObject(int id)
        {
            _id = id;
            _largeData = new byte[50000]; // 50KB per object to make memory impact visible
            
            // Create timer and subscribe to event
            _timer = new System.Timers.Timer(2000); // 2 second interval
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
            
            // IMPORTANT: This creates a strong reference chain:
            // 1. .NET runtime holds references to active System.Timers.Timer instances
            // 2. _timer keeps this object alive via the Elapsed event subscription
            // 3. This creates a circular reference that prevents garbage collection
            
            Console.WriteLine($"Created LeakyTimerObject {_id} - timer started");
        }
        
        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _tickCount++;
            
            if (_id % 20 == 0) // Only print occasionally to avoid spam
            {
                Console.WriteLine($"  Leaky timer {_id} tick #{_tickCount}");
            }
        }
        
        // NO DISPOSE METHOD!
        // The timer holds a reference to this object through the Elapsed event
        // Even when this object goes out of scope, the timer keeps it alive
        // The timer itself is also never disposed, so it continues running
        // 
        // As stated in the material:
        // "The runtime keeps _timer alive (because it's an active timer)."
        // "_timer keeps the Foo instance alive via the tmr_Elapsed event handler."
    }
    
    // GOOD EXAMPLE: Properly disposes timer to break the reference chain
    // This demonstrates the solution from the material
    public class ProperTimerObject : IDisposable
    {
        private readonly int _id;
        private readonly System.Timers.Timer _timer;
        private readonly byte[] _largeData;
        private int _tickCount = 0;
        private bool _disposed = false;
        
        public ProperTimerObject(int id)
        {
            _id = id;
            _largeData = new byte[50000]; // 50KB per object
            
            _timer = new System.Timers.Timer(3000); // 3 second interval
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
            
            Console.WriteLine($"Created ProperTimerObject {_id} - timer started");
        }
        
        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (_disposed) return; // Don't process if disposed
            
            _tickCount++;
            
            if (_id % 20 == 0)
            {
                Console.WriteLine($"  Proper timer {_id} tick #{_tickCount}");
            }
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                Console.WriteLine($"Disposing ProperTimerObject {_id}");
                
                // Stop and dispose the timer - this is crucial!
                _timer.Stop();
                _timer.Elapsed -= OnTimerElapsed; // Unsubscribe from event
                _timer.Dispose(); // Dispose the timer itself
                
                // This breaks the reference chain:
                // 1. timer.Dispose() tells runtime to stop referencing the timer
                // 2. Unsubscribing removes the reference from timer to this object
                // 3. Now this object can be garbage collected
                
                _disposed = true;
            }
        }
    }
    
    // Material Example: Demonstrates the guideline from the training material
    public static class TimerLeakGuideline
    {
        // "A good rule of thumb is to implement IDisposable yourself if any field 
        // in your class is assigned an object that implements IDisposable."
        
        public static void DemonstrateGuideline()
        {
            Console.WriteLine("GUIDELINE: If any field implements IDisposable, your class should too!");
            Console.WriteLine("System.Timers.Timer implements IDisposable, so our class must too.");
            
            // Example of following the guideline
            using (var goodTimer = new ProperTimerObject(999))
            {
                Console.WriteLine("Using statement ensures proper disposal");
                Thread.Sleep(1000);
            } // Dispose() automatically called here
            
            Console.WriteLine("Timer properly disposed via using statement");
        }
    }
    
    // Threading Timer Example - behaves differently as described in material
    public class ThreadingTimerExample
    {
        public static void DemonstrateThreadingTimerBehavior()
        {
            Console.WriteLine("System.Threading.Timer Behavior:");
            Console.WriteLine("- .NET does NOT hold strong references to threading timers");
            Console.WriteLine("- Instead, it references their callback delegates directly");
            Console.WriteLine("- Timer can be finalized if no strong reference exists");
            Console.WriteLine("- But this creates unpredictable behavior!\n");
            
            // Example 1: Timer without keeping reference (might get collected)
            // This demonstrates the exact scenario from the material
            Console.WriteLine("Creating threading timer without keeping reference...");
            CreateThreadingTimerWithoutReference();
            
            Thread.Sleep(2000);
            
            // Force collection to show unpredictable behavior
            Console.WriteLine("Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Thread.Sleep(2000);
            Console.WriteLine("Timer might have stopped ticking (unpredictable behavior)");
            
            // Example 2: Timer with proper disposal
            Console.WriteLine("\nCreating threading timer with proper disposal...");
            CreateThreadingTimerWithDisposal();
        }
        
        private static void CreateThreadingTimerWithoutReference()
        {
            // This timer might get collected in release mode because no strong reference is kept
            // This replicates the exact problem described in the material
            var timer = new System.Threading.Timer(
                callback: TimerCallback,
                state: "No Reference Timer",
                dueTime: 100,
                period: 500);
            
            // Timer reference goes out of scope - unpredictable behavior
            Console.WriteLine("Threading timer created without keeping reference");
            Console.WriteLine("In release mode, this timer might be collected immediately!");
        }
        
        private static void CreateThreadingTimerWithDisposal()
        {
            // This is the robust solution from the material
            using (var timer = new System.Threading.Timer(
                callback: TimerCallback,
                state: "Properly Managed Timer",
                dueTime: 100,
                period: 500))
            {
                Console.WriteLine("Threading timer created with using statement");
                Console.WriteLine("The 'using' block ensures timer is considered 'used' until disposed");
                Thread.Sleep(2000); // Let it run for 2 seconds
            } // Automatically disposed here
            
            Console.WriteLine("Threading timer properly disposed");
        }
        
        private static void TimerCallback(object? state)
        {
            Console.WriteLine($"  Threading timer callback: {state}");
        }
    }
}
