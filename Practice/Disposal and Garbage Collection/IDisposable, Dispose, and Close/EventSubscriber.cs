using System;

namespace DisposalPatternDemo
{
    /// <summary>
    /// This class demonstrates why event unsubscription is crucial in Dispose().
    /// Without proper unsubscription, the event source holds references to the subscriber,
    /// preventing garbage collection and causing memory leaks.
    /// </summary>
    public class EventPublisher
    {
        // This event will hold references to any subscribers
        public event Action<string>? TestEvent;

        public void RaiseTestEvent()
        {
            // Only raise the event if there are subscribers
            if (TestEvent != null)
            {
                TestEvent("Test event fired!");
                Console.WriteLine("🔔 EventPublisher: Event raised and delivered to subscribers");
            }
            else
            {
                Console.WriteLine("🔕 EventPublisher: Event raised but no subscribers (this is what we want after disposal)");
            }
        }
    }

    /// <summary>
    /// This class shows how to properly unsubscribe from events in Dispose().
    /// This is one of the most common causes of memory leaks in .NET applications.
    /// </summary>
    public class EventSubscriber : IDisposable
    {
        private EventPublisher? _publisher;
        private bool _disposed = false;

        public EventSubscriber(EventPublisher publisher)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            
            // Subscribe to the event
            _publisher.TestEvent += OnTestEvent;
            
            Console.WriteLine("📻 EventSubscriber: Subscribed to TestEvent");
        }

        /// <summary>
        /// This is our event handler method.
        /// </summary>
        private void OnTestEvent(string message)
        {
            Console.WriteLine($"📨 EventSubscriber: Received event - {message}");
        }

        /// <summary>
        /// This is where the magic happens - proper event unsubscription.
        /// If we don't unsubscribe, the EventPublisher will hold a reference to this object,
        /// preventing it from being garbage collected even after disposal.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                Console.WriteLine("🔄 EventSubscriber: Dispose() called again - safely ignored");
                return;
            }

            // CRITICAL: Unsubscribe from events to prevent memory leaks
            if (_publisher != null)
            {
                _publisher.TestEvent -= OnTestEvent;
                Console.WriteLine("🔌 EventSubscriber: Unsubscribed from TestEvent");
                _publisher = null; // Clear the reference
            }

            _disposed = true;
            Console.WriteLine("🧹 EventSubscriber: Disposed - no more event handling");
            
            GC.SuppressFinalize(this);
        }
    }
}
