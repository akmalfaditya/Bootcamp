// Static Reference and Weak Reference Examples
// These classes demonstrate memory leaks from static references and solutions using weak references

namespace ManagedMemoryLeaks
{
    // Demonstrates how static references can cause memory leaks
    public static class StaticReferenceLeakDemo
    {
        // This static collection holds references to objects forever!
        private static readonly List<string> StaticCollection = new List<string>();
        
        public static void AddToStaticCollection(string item)
        {
            // Once added here, objects can NEVER be garbage collected
            // because static references live for the entire application lifetime
            StaticCollection.Add(item);
        }
        
        public static void ClearStaticCollection()
        {
            // Only way to release the memory is to explicitly clear
            StaticCollection.Clear();
            Console.WriteLine($"Cleared static collection (had {StaticCollection.Count} items)");
        }
        
        public static int StaticCollectionCount => StaticCollection.Count;
    }
    
    // Example of expensive object that we want to be collectable
    public class ExpensiveDataObject
    {
        public string Data { get; }
        private readonly byte[] _largeBuffer;
        
        public ExpensiveDataObject(string data)
        {
            Data = data;
            _largeBuffer = new byte[100000]; // 100KB to make memory impact visible
            Console.WriteLine($"Created expensive object: {data}");
        }
        
        ~ExpensiveDataObject()
        {
            // Finalizer to show when object is collected
            Console.WriteLine($"Finalizing expensive object: {Data}");
        }
    }
    
    // Weak Event Pattern Implementation
    // This allows event handlers to be garbage collected even when subscribed
    public static class WeakEventManager
    {
        private static readonly List<WeakEventSubscription> Subscriptions = new List<WeakEventSubscription>();
        
        public static void Subscribe(WeakEventPublisher publisher, WeakEventSubscriber subscriber)
        {
            // Store weak reference to subscriber
            var subscription = new WeakEventSubscription(publisher, subscriber);
            Subscriptions.Add(subscription);
            
            // Subscribe to publisher's event
            publisher.WeakEvent += subscription.HandleEvent;
        }
        
        public static void CleanupDeadReferences()
        {
            // Remove subscriptions where subscriber has been collected
            for (int i = Subscriptions.Count - 1; i >= 0; i--)
            {
                if (!Subscriptions[i].IsSubscriberAlive)
                {
                    Subscriptions[i].Unsubscribe();
                    Subscriptions.RemoveAt(i);
                }
            }
        }
    }
    
    public class WeakEventSubscription
    {
        private readonly WeakReference _subscriberRef;
        private readonly WeakEventPublisher _publisher;
        
        public WeakEventSubscription(WeakEventPublisher publisher, WeakEventSubscriber subscriber)
        {
            _publisher = publisher;
            _subscriberRef = new WeakReference(subscriber);
        }
        
        public bool IsSubscriberAlive => _subscriberRef.IsAlive;
        
        public void HandleEvent(object? sender, string message)
        {
            // Try to get the subscriber
            if (_subscriberRef.Target is WeakEventSubscriber subscriber)
            {
                // Subscriber is still alive, forward the event
                subscriber.HandleWeakEvent(message);
            }
            else
            {
                // Subscriber has been collected, unsubscribe
                Console.WriteLine("Subscriber was collected, cleaning up subscription");
                Unsubscribe();
            }
        }
        
        public void Unsubscribe()
        {
            _publisher.WeakEvent -= HandleEvent;
        }
    }
    
    public class WeakEventPublisher
    {
        public event EventHandler<string>? WeakEvent;
        
        public void TriggerEvent(string message)
        {
            Console.WriteLine($"Publishing event: {message}");
            WeakEvent?.Invoke(this, message);
            
            // Clean up dead references
            WeakEventManager.CleanupDeadReferences();
        }
    }
    
    public class WeakEventSubscriber
    {
        private readonly string _name;
        private readonly byte[] _data;
        
        public WeakEventSubscriber()
        {
            _name = $"Subscriber_{GetHashCode()}";
            _data = new byte[10000]; // 10KB
            Console.WriteLine($"Created weak event subscriber: {_name}");
        }
        
        public void HandleWeakEvent(string message)
        {
            Console.WriteLine($"  {_name} received weak event: {message}");
        }
        
        ~WeakEventSubscriber()
        {
            Console.WriteLine($"Finalizing weak event subscriber: {_name}");
        }
    }
}
