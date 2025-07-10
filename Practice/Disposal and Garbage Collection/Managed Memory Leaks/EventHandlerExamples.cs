// Event Handler Memory Leak Examples
// These classes demonstrate how event subscriptions cause memory leaks
// They show the exact pattern described in the training material

namespace ManagedMemoryLeaks
{
    // The event publisher - represents objects that raise events
    // This class holds references to ALL subscribers through its event delegate list
    public class EventPublisher
    {
        // This event holds references to all subscribers
        // The event is essentially a multicast delegate that maintains a list of all subscribers
        public event EventHandler<string>? DataReceived;
        
        private int _eventCount = 0;
        
        public void TriggerEvent(string data)
        {
            _eventCount++;
            Console.WriteLine($"Publishing event #{_eventCount}: {data}");
            DataReceived?.Invoke(this, data);
        }
        
        // This property shows how many subscribers are still connected
        // Even if you lose references to subscriber objects, they remain in this list
        public int SubscriberCount => DataReceived?.GetInvocationList().Length ?? 0;
        
        public void ShowSubscriberInfo()
        {
            Console.WriteLine($"Event publisher has {SubscriberCount} active subscribers");
            Console.WriteLine($"Total events triggered: {_eventCount}");
            
            if (SubscriberCount > 0)
            {
                Console.WriteLine("These subscribers cannot be garbage collected!");
                Console.WriteLine("The event publisher holds strong references to them.");
            }
        }
    }
    
    // BAD EXAMPLE: This subscriber doesn't unsubscribe - causes memory leak!
    // This demonstrates the exact problem from the material
    public class LeakyEventSubscriber
    {
        private readonly int _id;
        private readonly EventPublisher _publisher;
        private readonly byte[] _largeData; // Make object larger to see memory impact
        
        public LeakyEventSubscriber(EventPublisher publisher, int id)
        {
            _id = id;
            _publisher = publisher;
            _largeData = new byte[10000]; // 10KB per subscriber to make memory usage visible
            
            // Subscribe to event - this creates a reference from publisher to this object
            // The publisher's event delegate list now contains a reference to this instance
            _publisher.DataReceived += OnDataReceived;
            
            // IMPORTANT: This creates a two-way reference:
            // 1. This object holds a reference to _publisher
            // 2. The _publisher holds a reference to this object via the event
            // But only #2 prevents garbage collection!
        }
        
        private void OnDataReceived(object? sender, string data)
        {
            // Process the event
            if (_id % 100 == 0) // Only print occasionally to avoid spam
            {
                Console.WriteLine($"  Leaky subscriber {_id} received: {data}");
            }
        }
        
        // NO DISPOSE METHOD - this is the problem!
        // When this object goes out of scope, it's still referenced by the event
        // The garbage collector cannot collect it because the publisher holds a reference
        // 
        // Even if the event never fires again, or if OnDataReceived does nothing,
        // this object will remain alive and consume memory, leading to a leak.
    }
    
    // GOOD EXAMPLE: This subscriber properly unsubscribes to prevent memory leaks
    // This demonstrates the solution from the material
    public class ProperEventSubscriber : IDisposable
    {
        private readonly int _id;
        private readonly EventPublisher _publisher;
        private readonly byte[] _largeData;
        private bool _disposed = false;
        
        public ProperEventSubscriber(EventPublisher publisher, int id)
        {
            _id = id;
            _publisher = publisher;
            _largeData = new byte[10000]; // 10KB per subscriber
            
            // Subscribe to event - same as the leaky version
            _publisher.DataReceived += OnDataReceived;
        }
        
        private void OnDataReceived(object? sender, string data)
        {
            if (_disposed) return; // Don't process if disposed
            
            if (_id % 100 == 0)
            {
                Console.WriteLine($"  Proper subscriber {_id} received: {data}");
            }
        }
        
        // Proper cleanup - this is what prevents the memory leak
        // This is the "most robust and common solution" mentioned in the material
        public void Dispose()
        {
            if (!_disposed)
            {
                // Crucial step: Unsubscribe from the event
                // This removes the reference from the publisher's delegate list
                _publisher.DataReceived -= OnDataReceived;
                
                // Now the publisher no longer holds a reference to this object
                // The object becomes eligible for garbage collection
                _disposed = true;
            }
        }
    }
    
    // Additional example: Weak event pattern for environments like WPF
    // This shows the alternative solution mentioned in the material
    public class SimpleWeakEventSubscriber
    {
        private readonly int _id;
        private readonly WeakReference _selfReference;
        
        public SimpleWeakEventSubscriber(int id)
        {
            _id = id;
            _selfReference = new WeakReference(this);
        }
        
        public void HandleEvent(object? sender, string data)
        {
            if (_selfReference.IsAlive)
            {
                Console.WriteLine($"  Weak subscriber {_id} received: {data}");
            }
        }
        
        // In a real weak event pattern, you would use something like:
        // WeakEventManager.AddHandler(publisher, "DataReceived", HandleEvent);
        // This allows the subscriber to be collected even if the publisher is still alive
    }
}
