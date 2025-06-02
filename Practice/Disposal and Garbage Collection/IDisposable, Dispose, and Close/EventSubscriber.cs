using System;

namespace DisposalPatternDemo
{
    /// <summary>
    /// Demonstrates event publishing for the disposal pattern demo.
    /// This class publishes events that other objects can subscribe to.
    /// </summary>
    public class EventPublisher
    {
        /// <summary>
        /// Event that gets triggered when something interesting happens
        /// </summary>
        public event Action<string>? SomethingHappened;

        /// <summary>
        /// Triggers the event to notify all subscribers
        /// </summary>
        public void TriggerEvent()
        {
            string message = $"Event triggered at {DateTime.Now:HH:mm:ss}";
            Console.WriteLine($"📢 Publisher: {message}");
            
            // Notify all subscribers
            SomethingHappened?.Invoke(message);
        }

        /// <summary>
        /// Gets the number of subscribers currently listening to the event
        /// </summary>
        public int SubscriberCount => SomethingHappened?.GetInvocationList().Length ?? 0;
    }

    /// <summary>
    /// Demonstrates proper event unsubscription during disposal.
    /// This is crucial to prevent memory leaks and unexpected behavior.
    /// </summary>
    public class EventSubscriber : IDisposable
    {
        private EventPublisher? _publisher;
        private bool _disposed = false;
        private readonly string _subscriberName;

        public EventSubscriber(EventPublisher publisher)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _subscriberName = $"Subscriber-{Guid.NewGuid().ToString()[..8]}";
            
            // Subscribe to the event
            _publisher.SomethingHappened += OnSomethingHappened;
            Console.WriteLine($"👂 {_subscriberName}: Subscribed to events");
        }

        /// <summary>
        /// Event handler that gets called when the publisher triggers an event
        /// </summary>
        private void OnSomethingHappened(string message)
        {
            if (_disposed)
            {
                Console.WriteLine($"⚠ {_subscriberName}: Received event after disposal! This shouldn't happen.");
                return;
            }

            Console.WriteLine($"🎯 {_subscriberName}: Received event - {message}");
        }

        /// <summary>
        /// Performs some work that might be called by external code
        /// </summary>
        public void DoWork()
        {
            ThrowIfDisposed();
            Console.WriteLine($"⚙ {_subscriberName}: Doing some work...");
        }

        /// <summary>
        /// Implementation of IDisposable.Dispose()
        /// This demonstrates the critical importance of unsubscribing from events
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method that handles the actual cleanup
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // CRITICAL: Unsubscribe from events to prevent memory leaks
                    if (_publisher != null)
                    {
                        _publisher.SomethingHappened -= OnSomethingHappened;
                        Console.WriteLine($"🔇 {_subscriberName}: Unsubscribed from events during disposal");
                        _publisher = null;
                    }
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer to catch cases where Dispose wasn't called
        /// </summary>
        ~EventSubscriber()
        {
            Console.WriteLine($"⚠ {_subscriberName}: Finalizer called - events may not be properly unsubscribed!");
            Dispose(disposing: false);
        }

        /// <summary>
        /// Helper method to check disposal state
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(_subscriberName,
                    "Cannot perform operations on a disposed EventSubscriber");
            }
        }

        /// <summary>
        /// Property to check if this subscriber has been disposed
        /// </summary>
        public bool IsDisposed => _disposed;

        /// <summary>
        /// Gets the name of this subscriber for identification
        /// </summary>
        public string Name => _subscriberName;
    }
}
