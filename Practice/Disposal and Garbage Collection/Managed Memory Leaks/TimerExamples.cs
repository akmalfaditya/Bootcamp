// Timer Memory Leak Examples
// These classes show how different timer types can cause memory leaks

using System.Timers;

namespace ManagedMemoryLeaks
{
    // BAD EXAMPLE: Timer keeps object alive - memory leak!
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
        }
        
        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _tickCount++;
            
            if (_id % 20 == 0) // Only print occasionally
            {
                Console.WriteLine($"  Leaky timer {_id} tick #{_tickCount}");
            }
        }
        
        // NO DISPOSE METHOD!
        // The timer holds a reference to this object through the Elapsed event
        // Even when this object goes out of scope, the timer keeps it alive
        // The timer itself is also never disposed, so it continues running
    }
    
    // GOOD EXAMPLE: Properly disposes timer
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
        }
        
        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (_disposed) return;
            
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
                // Stop and dispose the timer - this is crucial!
                _timer.Stop();
                _timer.Elapsed -= OnTimerElapsed; // Unsubscribe from event
                _timer.Dispose(); // Dispose the timer itself
                
                _disposed = true;
            }
        }
    }
    
    // Threading Timer Example - behaves differently
    public class ThreadingTimerExample
    {
        public static void DemonstrateThreadingTimerBehavior()
        {
            Console.WriteLine("Threading Timer Behavior:");
            Console.WriteLine("- References callback delegate directly");
            Console.WriteLine("- Can be finalized automatically if no strong reference");
            Console.WriteLine("- But still best practice to dispose properly");
            
            // Example 1: Timer without keeping reference (might get collected)
            CreateThreadingTimerWithoutReference();
            
            Thread.Sleep(1000);
            
            // Example 2: Timer with proper disposal
            CreateThreadingTimerWithDisposal();
        }
        
        private static void CreateThreadingTimerWithoutReference()
        {
            // This timer might get collected in release mode because no strong reference is kept
            var timer = new System.Threading.Timer(
                callback: TimerCallback,
                state: "No Reference Timer",
                dueTime: 100,
                period: 500);
            
            // Timer reference goes out of scope - unpredictable behavior
            Console.WriteLine("Threading timer created without keeping reference");
        }
        
        private static void CreateThreadingTimerWithDisposal()
        {
            using (var timer = new System.Threading.Timer(
                callback: TimerCallback,
                state: "Properly Managed Timer",
                dueTime: 100,
                period: 500))
            {
                Console.WriteLine("Threading timer created with using statement");
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
