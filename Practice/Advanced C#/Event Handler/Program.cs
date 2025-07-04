using System;
using System.Threading;
using System.Collections.Generic;

namespace EventHandlerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Events in C# - Complete Demonstration ===\n");

            // Run all demonstrations to show event concepts in action
            BasicEventDeclarationDemo();
            EventVsDelegateDemo();
            StandardEventPatternDemo();
            EventAccessorsDemo();
            EventModifiersDemo();
            ThreadSafeEventHandlingDemo();
            RealWorldECommerceDemo();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Event Declaration

        static void BasicEventDeclarationDemo()
        {
            Console.WriteLine("1. BASIC EVENT DECLARATION - BROADCASTER/SUBSCRIBER PATTERN");
            Console.WriteLine("==========================================================");

            // Create a basic broadcaster using custom delegate
            var priceMonitor = new BasicPriceMonitor("AAPL");
            
            // Create subscribers - these are just methods that match our delegate signature
            void Trader1Handler(decimal oldPrice, decimal newPrice) => 
                Console.WriteLine($"  Trader 1: Price changed from ${oldPrice} to ${newPrice}");
            
            void Trader2Handler(decimal oldPrice, decimal newPrice) => 
                Console.WriteLine($"  Trader 2: Significant move! ${oldPrice} -> ${newPrice}");

            // Subscribe to the event - notice we can only use += and -= from outside the class
            priceMonitor.PriceChanged += Trader1Handler;
            priceMonitor.PriceChanged += Trader2Handler;

            Console.WriteLine("Subscribed two traders to price changes");
            Console.WriteLine("Triggering price changes...\n");

            // Trigger some price changes - this will notify all subscribers
            priceMonitor.UpdatePrice(150.00m);
            priceMonitor.UpdatePrice(155.50m);

            // Remove one subscriber
            priceMonitor.PriceChanged -= Trader1Handler;
            Console.WriteLine("\nTrader 1 unsubscribed. Only Trader 2 should receive this update:");
            priceMonitor.UpdatePrice(152.75m);

            Console.WriteLine();
        }

        #endregion

        #region Event vs Delegate Demonstration

        static void EventVsDelegateDemo()
        {
            Console.WriteLine("2. EVENT VS DELEGATE - SAFETY COMPARISON");
            Console.WriteLine("========================================");

            var eventPublisher = new EventPublisher();
            var delegatePublisher = new DelegatePublisher();

            // Subscribe to both
            eventPublisher.SafeNotification += msg => Console.WriteLine($"  Event received: {msg}");
            delegatePublisher.UnsafeNotification += msg => Console.WriteLine($"  Delegate received: {msg}");

            Console.WriteLine("Both subscribed successfully");

            // These work fine for both
            eventPublisher.TriggerEvent("Hello from event");
            delegatePublisher.TriggerEvent("Hello from delegate");

            Console.WriteLine("\nTesting safety differences:");

            // Try to do dangerous things - these will show the difference
            // eventPublisher.SafeNotification = null;        // Compile error - can't assign to event
            // eventPublisher.SafeNotification("hack");       // Compile error - can't invoke from outside

            // But with delegate, these dangerous operations are possible:
            Console.WriteLine("Delegate allows dangerous operations:");
            delegatePublisher.UnsafeNotification = null; // Wipes out all subscribers!
            delegatePublisher.TriggerEvent("This won't be received by anyone");

            Console.WriteLine("Event safety prevents subscriber interference\n");
        }

        #endregion

        #region Standard Event Pattern

        static void StandardEventPatternDemo()
        {
            Console.WriteLine("3. STANDARD EVENT PATTERN - PROPER .NET CONVENTION");
            Console.WriteLine("==================================================");

            // Create a stock using the standard pattern
            var stock = new Stock("MSFT");
            var portfolio = new Portfolio("Tech Stocks");

            // Subscribe using the standard EventHandler<T> pattern
            stock.PriceChanged += portfolio.OnPriceChanged;
            stock.PriceChanged += (sender, e) => 
            {
                if (Math.Abs(e.PercentChange) > 5)
                    Console.WriteLine($"  ALERT: Large price movement detected!");
            };

            Console.WriteLine($"Created stock: {stock.Symbol}");
            Console.WriteLine("Subscribed portfolio manager and alert system");
            Console.WriteLine("Making price changes...\n");

            // Set initial price and then change it
            stock.Price = 300.00m;
            stock.Price = 315.50m;  // 5.17% increase
            stock.Price = 298.25m;  // 5.47% decrease
            stock.Price = 298.25m;  // No change - won't trigger event

            Console.WriteLine();
        }

        #endregion

        #region Event Accessors

        static void EventAccessorsDemo()
        {
            Console.WriteLine("4. CUSTOM EVENT ACCESSORS - ADVANCED CONTROL");
            Console.WriteLine("============================================");

            var smartNotifier = new SmartNotificationSystem();
            
            void Handler1(string msg) => Console.WriteLine($"  Handler 1: {msg}");
            void Handler2(string msg) => Console.WriteLine($"  Handler 2: {msg}");
            void Handler3(string msg) => Console.WriteLine($"  Handler 3: {msg}");

            // The custom accessors will track subscription changes
            Console.WriteLine("Adding subscribers (watch the custom accessor behavior):");
            smartNotifier.MessageReceived += Handler1;
            smartNotifier.MessageReceived += Handler2;
            smartNotifier.MessageReceived += Handler3;

            Console.WriteLine($"\nTotal subscribers: {smartNotifier.SubscriberCount}");
            smartNotifier.SendMessage("Hello everyone!");

            Console.WriteLine("\nRemoving one subscriber:");
            smartNotifier.MessageReceived -= Handler2;
            Console.WriteLine($"Remaining subscribers: {smartNotifier.SubscriberCount}");
            smartNotifier.SendMessage("Handler 2 should be gone");

            Console.WriteLine();
        }

        #endregion

        #region Event Modifiers

        static void EventModifiersDemo()
        {
            Console.WriteLine("5. EVENT MODIFIERS - VIRTUAL, OVERRIDE, STATIC");
            Console.WriteLine("===============================================");

            // Demonstrate static events
            Console.WriteLine("Static event demonstration:");
            SystemMonitor.SystemAlert += (msg) => Console.WriteLine($"  System Alert: {msg}");
            SystemMonitor.TriggerAlert("High CPU usage detected");

            // Demonstrate virtual/override events
            Console.WriteLine("\nVirtual/Override event demonstration:");
            BaseService baseService = new EnhancedService();
            
            baseService.StatusChanged += (sender, status) => 
                Console.WriteLine($"  Status update from {sender.GetType().Name}: {status}");

            // This will use the overridden event behavior
            baseService.ChangeStatus("Enhanced service is running");

            Console.WriteLine();
        }

        #endregion

        #region Thread-Safe Event Handling

        static void ThreadSafeEventHandlingDemo()
        {
            Console.WriteLine("6. THREAD-SAFE EVENT HANDLING");
            Console.WriteLine("=============================");

            var processor = new DataProcessor();
            var logger = new EventLogger();

            // Subscribe to events
            processor.DataProcessed += logger.LogDataProcessed;
            processor.DataProcessed += (sender, e) => 
                Console.WriteLine($"  Processing complete: {e.ItemsProcessed} items in {e.Duration.TotalMilliseconds}ms");

            Console.WriteLine("Starting concurrent data processing...");

            // Simulate multiple threads processing data
            var threads = new Thread[3];
            for (int i = 0; i < threads.Length; i++)
            {
                int threadId = i;
                threads[i] = new Thread(() => 
                {
                    processor.ProcessData($"Dataset-{threadId}", 100 + threadId * 50);
                });
                threads[i].Start();
            }

            // Wait for all threads to complete
            foreach (var thread in threads)
                thread.Join();

            Console.WriteLine("All processing completed safely\n");
        }

        #endregion

        #region Real World E-Commerce Scenario

        static void RealWorldECommerceDemo()
        {
            Console.WriteLine("7. REAL-WORLD SCENARIO - E-COMMERCE ORDER SYSTEM");
            Console.WriteLine("=================================================");

            // Set up the order processing system
            var orderSystem = new OrderProcessingSystem();
            var emailService = new EmailService();
            var inventorySystem = new InventorySystem();
            var auditLogger = new AuditLogger();
            var loyaltyProgram = new LoyaltyProgram();

            // Wire up all the event handlers - this is the beauty of events
            // Each service can independently subscribe to the events it cares about
            orderSystem.OrderPlaced += emailService.OnOrderPlaced;
            orderSystem.OrderPlaced += inventorySystem.OnOrderPlaced;
            orderSystem.OrderPlaced += auditLogger.OnOrderPlaced;
            orderSystem.OrderPlaced += loyaltyProgram.OnOrderPlaced;

            orderSystem.OrderCancelled += emailService.OnOrderCancelled;
            orderSystem.OrderCancelled += inventorySystem.OnOrderCancelled;
            orderSystem.OrderCancelled += auditLogger.OnOrderCancelled;

            // Process some orders - watch how all systems respond automatically
            Console.WriteLine("Processing customer orders...\n");

            var order1 = new CustomerOrder(1001, "john.doe@email.com", 299.99m, "Laptop");
            var order2 = new CustomerOrder(1002, "jane.smith@email.com", 89.50m, "Wireless Mouse");

            orderSystem.PlaceOrder(order1);
            Console.WriteLine();

            orderSystem.PlaceOrder(order2);
            Console.WriteLine();

            // Cancel an order
            orderSystem.CancelOrder(order1);
            Console.WriteLine();
        }

        #endregion
    }

    #region Basic Event Classes

    // Custom delegate for price changes - this defines the event signature
    public delegate void PriceChangedHandler(decimal oldPrice, decimal newPrice);

    // Basic broadcaster class - demonstrates fundamental event concepts
    public class BasicPriceMonitor
    {
        private decimal _currentPrice;
        
        public string Symbol { get; }

        // Event declaration - this is the key difference from a regular delegate field
        // Outside classes can only use += and -= on this event
        public event PriceChangedHandler? PriceChanged;

        public BasicPriceMonitor(string symbol)
        {
            Symbol = symbol;
            _currentPrice = 0;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (_currentPrice != newPrice)
            {
                decimal oldPrice = _currentPrice;
                _currentPrice = newPrice;

                // Inside the class, we can invoke the event like a regular delegate
                // This will call ALL subscribed methods in the order they were added
                PriceChanged?.Invoke(oldPrice, newPrice);
            }
        }
    }

    #endregion

    #region Event vs Delegate Safety Demo

    // Class using proper event (safe)
    public class EventPublisher
    {
        public event Action<string>? SafeNotification;

        public void TriggerEvent(string message)
        {
            Console.WriteLine($"Event publisher sending: {message}");
            SafeNotification?.Invoke(message);
        }
    }

    // Class using raw delegate (unsafe)
    public class DelegatePublisher
    {
        // This is just a public delegate field - dangerous!
        public Action<string>? UnsafeNotification;

        public void TriggerEvent(string message)
        {
            Console.WriteLine($"Delegate publisher sending: {message}");
            UnsafeNotification?.Invoke(message);
        }
    }

    #endregion

    #region Standard Event Pattern Classes

    // Proper EventArgs subclass following .NET conventions
    public class PriceChangedEventArgs : EventArgs
    {
        public string Symbol { get; }
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }
        public decimal PriceChange => NewPrice - OldPrice;
        public decimal PercentChange => OldPrice == 0 ? 0 : (PriceChange / OldPrice) * 100;
        public DateTime Timestamp { get; }

        public PriceChangedEventArgs(string symbol, decimal oldPrice, decimal newPrice)
        {
            Symbol = symbol;
            OldPrice = oldPrice;
            NewPrice = newPrice;
            Timestamp = DateTime.Now;
        }
    }

    // Stock class implementing the standard .NET event pattern
    public class Stock
    {
        private decimal _price;

        public string Symbol { get; }
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    decimal oldPrice = _price;
                    _price = value;
                    
                    // Create proper EventArgs and fire the event
                    OnPriceChanged(new PriceChangedEventArgs(Symbol, oldPrice, value));
                }
            }
        }

        // Standard event using EventHandler<T> - this is the preferred approach
        public event EventHandler<PriceChangedEventArgs>? PriceChanged;

        public Stock(string symbol)
        {
            Symbol = symbol;
        }

        // Protected virtual "On" method - this is standard .NET pattern
        // Allows derived classes to override event firing behavior
        protected virtual void OnPriceChanged(PriceChangedEventArgs e)
        {
            // Thread-safe event invocation using null-conditional operator
            // This prevents NullReferenceException if the last subscriber unsubscribes
            // between the null check and the invocation
            PriceChanged?.Invoke(this, e);
        }
    }

    // Subscriber class that handles stock price changes
    public class Portfolio
    {
        public string Name { get; }

        public Portfolio(string name)
        {
            Name = name;
        }

        // Event handler following standard pattern: object sender, EventArgs e
        public void OnPriceChanged(object? sender, PriceChangedEventArgs e)
        {
            var direction = e.PriceChange >= 0 ? "↑" : "↓";
            
            Console.WriteLine($"  Portfolio '{Name}': {e.Symbol} {direction} " +
                            $"${e.OldPrice:F2} → ${e.NewPrice:F2} " +
                            $"({e.PercentChange:+0.00;-0.00}%)");
        }
    }

    #endregion

    #region Custom Event Accessors

    // Class demonstrating explicit event accessors
    public class SmartNotificationSystem
    {
        // Private delegate field - the compiler normally generates this automatically
        private Action<string>? _messageReceived;
        private int _subscriberCount = 0;

        public int SubscriberCount => _subscriberCount;

        // Explicit event accessors - we control what happens during add/remove
        public event Action<string> MessageReceived
        {
            add
            {
                Console.WriteLine($"  Adding subscriber (current count: {_subscriberCount})");
                _messageReceived += value;
                _subscriberCount++;
            }
            remove
            {
                Console.WriteLine($"  Removing subscriber (current count: {_subscriberCount})");
                _messageReceived -= value;
                _subscriberCount--;
            }
        }

        public void SendMessage(string message)
        {
            Console.WriteLine($"Sending message: {message}");
            // Invoke the private delegate field directly
            _messageReceived?.Invoke(message);
        }
    }

    #endregion

    #region Event Modifiers Demo

    // Static events example
    public static class SystemMonitor
    {
        // Static event belongs to the type, not an instance
        public static event Action<string>? SystemAlert;

        public static void TriggerAlert(string alertMessage)
        {
            SystemAlert?.Invoke(alertMessage);
        }
    }

    // Base class with virtual event
    public class BaseService
    {
        // Virtual event can be overridden in derived classes
        public virtual event Action<object, string>? StatusChanged;

        public virtual void ChangeStatus(string status)
        {
            OnStatusChanged(status);
        }

        protected virtual void OnStatusChanged(string status)
        {
            StatusChanged?.Invoke(this, status);
        }
    }

    // Derived class overriding the virtual event
    public class EnhancedService : BaseService
    {
        // Override the virtual event with enhanced behavior
        public override event Action<object, string>? StatusChanged;

        protected override void OnStatusChanged(string status)
        {
            // Enhanced logging before firing the event
            Console.WriteLine($"  Enhanced service logging: Status changing to '{status}'");
            StatusChanged?.Invoke(this, $"[ENHANCED] {status}");
        }
    }

    #endregion

    #region Thread-Safe Event Handling

    public class DataProcessedEventArgs : EventArgs
    {
        public string DatasetName { get; }
        public int ItemsProcessed { get; }
        public TimeSpan Duration { get; }
        public int ThreadId { get; }

        public DataProcessedEventArgs(string datasetName, int itemsProcessed, TimeSpan duration)
        {
            DatasetName = datasetName;
            ItemsProcessed = itemsProcessed;
            Duration = duration;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }
    }

    public class DataProcessor
    {
        // Thread-safe event declaration
        public event EventHandler<DataProcessedEventArgs>? DataProcessed;

        public void ProcessData(string datasetName, int itemCount)
        {
            var startTime = DateTime.Now;
            
            Console.WriteLine($"  Thread {Thread.CurrentThread.ManagedThreadId}: Processing {datasetName}...");
            
            // Simulate processing time
            Thread.Sleep(100 + new Random().Next(100));
            
            var duration = DateTime.Now - startTime;
            
            // Fire the event - this is thread-safe due to the null-conditional operator
            OnDataProcessed(new DataProcessedEventArgs(datasetName, itemCount, duration));
        }

        protected virtual void OnDataProcessed(DataProcessedEventArgs e)
        {
            // The ?. operator ensures thread safety even if subscribers change
            // between threads
            DataProcessed?.Invoke(this, e);
        }
    }

    public class EventLogger
    {
        public void LogDataProcessed(object? sender, DataProcessedEventArgs e)
        {
            Console.WriteLine($"  Logger: {e.DatasetName} completed on thread {e.ThreadId} " +
                            $"({e.ItemsProcessed} items, {e.Duration.TotalMilliseconds:F0}ms)");
        }
    }

    #endregion

    #region Real-World E-Commerce Classes

    // Order data structure
    public class CustomerOrder
    {
        public int OrderId { get; }
        public string CustomerEmail { get; }
        public decimal Amount { get; }
        public string ProductName { get; }
        public DateTime OrderTime { get; }

        public CustomerOrder(int orderId, string customerEmail, decimal amount, string productName)
        {
            OrderId = orderId;
            CustomerEmail = customerEmail;
            Amount = amount;
            ProductName = productName;
            OrderTime = DateTime.Now;
        }
    }

    // EventArgs for order events
    public class OrderEventArgs : EventArgs
    {
        public CustomerOrder Order { get; }
        public DateTime EventTime { get; }

        public OrderEventArgs(CustomerOrder order)
        {
            Order = order;
            EventTime = DateTime.Now;
        }
    }

    // Main order processing system - the event broadcaster
    public class OrderProcessingSystem
    {
        // Multiple events for different business scenarios
        public event EventHandler<OrderEventArgs>? OrderPlaced;
        public event EventHandler<OrderEventArgs>? OrderCancelled;

        public void PlaceOrder(CustomerOrder order)
        {
            Console.WriteLine($"Order System: Processing order #{order.OrderId} for {order.CustomerEmail}");
            Console.WriteLine($"  Product: {order.ProductName}, Amount: ${order.Amount}");
            
            // Fire the event - all interested systems will be notified automatically
            OnOrderPlaced(new OrderEventArgs(order));
        }

        public void CancelOrder(CustomerOrder order)
        {
            Console.WriteLine($"Order System: Cancelling order #{order.OrderId}");
            
            OnOrderCancelled(new OrderEventArgs(order));
        }

        protected virtual void OnOrderPlaced(OrderEventArgs e)
        {
            OrderPlaced?.Invoke(this, e);
        }

        protected virtual void OnOrderCancelled(OrderEventArgs e)
        {
            OrderCancelled?.Invoke(this, e);
        }
    }

    // Email service - independent subscriber
    public class EmailService
    {
        public void OnOrderPlaced(object? sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Email Service: Sending confirmation to {e.Order.CustomerEmail}");
            Console.WriteLine($"    'Your order #{e.Order.OrderId} has been confirmed'");
        }

        public void OnOrderCancelled(object? sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Email Service: Sending cancellation notice to {e.Order.CustomerEmail}");
            Console.WriteLine($"    'Your order #{e.Order.OrderId} has been cancelled'");
        }
    }

    // Inventory system - another independent subscriber
    public class InventorySystem
    {
        public void OnOrderPlaced(object? sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Inventory System: Reserving {e.Order.ProductName} for order #{e.Order.OrderId}");
            Console.WriteLine($"    Inventory levels updated");
        }

        public void OnOrderCancelled(object? sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Inventory System: Releasing reserved {e.Order.ProductName}");
            Console.WriteLine($"    Item returned to available inventory");
        }
    }

    // Audit logging - tracks all order activities
    public class AuditLogger
    {
        public void OnOrderPlaced(object? sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Audit Logger: ORDER_PLACED - ID:{e.Order.OrderId}, " +
                            $"Customer:{e.Order.CustomerEmail}, Amount:${e.Order.Amount}");
        }

        public void OnOrderCancelled(object? sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Audit Logger: ORDER_CANCELLED - ID:{e.Order.OrderId}");
        }
    }

    // Loyalty program - calculates reward points
    public class LoyaltyProgram
    {
        public void OnOrderPlaced(object? sender, OrderEventArgs e)
        {
            int points = (int)(e.Order.Amount / 10); // 1 point per $10 spent
            Console.WriteLine($"  Loyalty Program: Adding {points} points to {e.Order.CustomerEmail}");
            Console.WriteLine($"    Customer rewards balance updated");
        }
    }

    #endregion
}
