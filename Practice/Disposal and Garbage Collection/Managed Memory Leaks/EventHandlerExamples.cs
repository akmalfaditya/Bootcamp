// Event Handler Memory Leak Examples
// These classes show how event subscriptions can cause memory leaks

namespace ManagedMemoryLeaks
{
    // The event publisher - represents objects that raise events
    public class EventPublisher
    {
        // This event holds references to all subscribers
        public event EventHandler<string>? DataReceived;
        
        private int _eventCount = 0;
        
        public void TriggerEvent(string data)
        {
            _eventCount++;
            DataReceived?.Invoke(this, data);
        }
        
        public int SubscriberCount => DataReceived?.GetInvocationList().Length ?? 0;
        
        public void ShowSubscriberInfo()
        {
            Console.WriteLine($"Event publisher has {SubscriberCount} subscribers");
            Console.WriteLine($"Events triggered: {_eventCount}");
        }
    }
    
    // BAD EXAMPLE: This subscriber doesn't unsubscribe - causes memory leak!
    public class LeakyEventSubscriber
    {
        private readonly int _id;
        private readonly EventPublisher _publisher;
        private readonly byte[] _largeData; // Make object larger to see memory impact
        
        public LeakyEventSubscriber(EventPublisher publisher, int id)
        {
            _id = id;
            _publisher = publisher;
            _largeData = new byte[10000]; // 10KB per subscriber
            
            // Subscribe to event - this creates a reference from publisher to this object
            _publisher.DataReceived += OnDataReceived;
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
    }
    
    // GOOD EXAMPLE: This subscriber properly unsubscribes
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
        public void Dispose()
        {
            if (!_disposed)
            {
                // Unsubscribe from the event - this removes the reference
                _publisher.DataReceived -= OnDataReceived;
                _disposed = true;
            }
        }
    }
}
