// WeakEventPattern.cs
// This demonstrates how to use weak references for event handling
// Prevents memory leaks where event subscribers keep publishers alive

namespace WeakReferences
{
    // Event subscriber that can be garbage collected even when subscribed
    public class EventSubscriber
    {
        public string Name { get; private set; }
        private static int _instanceCount = 0;
        private int _instanceId;

        public EventSubscriber(string name)
        {
            _instanceId = ++_instanceCount;
            Name = name;
            Console.WriteLine($"  EventSubscriber #{_instanceId} created: {name}");
        }

        // Method that handles events
        public void HandleEvent(string message)
        {
            Console.WriteLine($"  {Name} received event: {message}");
        }

        // Finalizer to show when subscribers are collected
        ~EventSubscriber()
        {
            Console.WriteLine($"  EventSubscriber #{_instanceId} finalized: {Name}");
        }

        public override string ToString()
        {
            return $"EventSubscriber #{_instanceId}: {Name}";
        }
    }

    // Event publisher that uses weak references for subscribers
    // This prevents the publisher from keeping subscribers alive
    public class WeakEventPublisher
    {
        private readonly List<WeakReference> _subscribers;

        public WeakEventPublisher()
        {
            _subscribers = new List<WeakReference>();
        }

        // Subscribe using weak reference - subscriber can still be collected
        public void Subscribe(EventSubscriber subscriber)
        {
            _subscribers.Add(new WeakReference(subscriber));
            Console.WriteLine($"  Subscribed: {subscriber.Name}");
        }

        // Publish event to all alive subscribers
        public void PublishEvent(string message)
        {
            Console.WriteLine($"  Publishing: {message}");

            // Clean up dead references and notify alive subscribers
            var deadReferences = new List<WeakReference>();

            foreach (var weakRef in _subscribers)
            {
                var subscriber = weakRef.Target as EventSubscriber;
                if (subscriber != null)
                {
                    // Subscriber is still alive, notify it
                    subscriber.HandleEvent(message);
                }
                else
                {
                    // Subscriber was collected, mark reference for removal
                    deadReferences.Add(weakRef);
                }
            }

            // Remove dead references
            foreach (var deadRef in deadReferences)
            {
                _subscribers.Remove(deadRef);
            }

            if (deadReferences.Count > 0)
            {
                Console.WriteLine($"  Cleaned up {deadReferences.Count} dead subscriber(s)");
            }
        }

        // Get count of currently alive subscribers
        public int SubscriberCount
        {
            get
            {
                var aliveCount = 0;
                var deadReferences = new List<WeakReference>();

                foreach (var weakRef in _subscribers)
                {
                    if (weakRef.IsAlive)
                    {
                        aliveCount++;
                    }
                    else
                    {
                        deadReferences.Add(weakRef);
                    }
                }

                // Clean up dead references
                foreach (var deadRef in deadReferences)
                {
                    _subscribers.Remove(deadRef);
                }

                return aliveCount;
            }
        }

        // Manually unsubscribe a specific subscriber
        public bool Unsubscribe(EventSubscriber subscriber)
        {
            for (int i = _subscribers.Count - 1; i >= 0; i--)
            {
                var target = _subscribers[i].Target as EventSubscriber;
                if (target == subscriber)
                {
                    _subscribers.RemoveAt(i);
                    Console.WriteLine($"  Unsubscribed: {subscriber.Name}");
                    return true;
                }
            }
            return false;
        }

        // Clear all subscribers
        public void ClearSubscribers()
        {
            _subscribers.Clear();
            Console.WriteLine("  All subscribers cleared");
        }
    }
}
