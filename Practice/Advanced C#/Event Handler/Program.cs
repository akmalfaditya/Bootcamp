using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Events in C# - Complete Demonstration ===\n");

            // Run all demonstrations
            BasicEventDemo();
            StandardEventPatternDemo();
            ThreadSafetyDemo();
            CustomEventAccessorsDemo();
            RealWorldScenarioDemo();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Event Demonstrations

        static void BasicEventDemo()
        {
            Console.WriteLine("1. BASIC EVENT DEMONSTRATION");
            Console.WriteLine("============================");

            // Create broadcaster and subscribers
            var newsAgency = new NewsAgency();
            var subscriber1 = new NewsSubscriber("CNN");
            var subscriber2 = new NewsSubscriber("BBC");

            // Subscribe to the event - this is where the magic happens
            newsAgency.BreakingNews += subscriber1.OnBreakingNews;
            newsAgency.BreakingNews += subscriber2.OnBreakingNews;

            // Trigger the event - both subscribers will be notified
            newsAgency.PublishNews("Major earthquake hits California!");

            // Unsubscribe one subscriber
            newsAgency.BreakingNews -= subscriber1.OnBreakingNews;
            
            Console.WriteLine("\nAfter CNN unsubscribed:");
            newsAgency.PublishNews("Stock market reaches new high!");

            Console.WriteLine();
        }

        #endregion

        #region Standard Event Pattern Demonstrations

        static void StandardEventPatternDemo()
        {
            Console.WriteLine("2. STANDARD EVENT PATTERN DEMONSTRATION");
            Console.WriteLine("=======================================");

            // Create a stock and some investors
            var appleStock = new Stock("AAPL", 150.00m);
            var investor1 = new Investor("Warren Buffett");
            var investor2 = new Investor("Elon Musk");

            // Subscribe investors to price changes
            appleStock.PriceChanged += investor1.OnPriceChanged;
            appleStock.PriceChanged += investor2.OnPriceChanged;

            Console.WriteLine("Setting initial stock price...");
            Console.WriteLine($"Stock: {appleStock.Symbol}, Initial Price: ${appleStock.Price}");
            Console.WriteLine();

            // Change prices - this will trigger events
            Console.WriteLine("Price changes occurring:");
            appleStock.Price = 155.50m;
            appleStock.Price = 148.75m;
            appleStock.Price = 162.20m;

            Console.WriteLine();
        }

        #endregion

        #region Thread Safety Demonstrations

        static void ThreadSafetyDemo()
        {
            Console.WriteLine("3. THREAD SAFETY DEMONSTRATION");
            Console.WriteLine("===============================");

            var server = new WebServer();
            var logger = new RequestLogger();
            var analytics = new AnalyticsService();

            // Subscribe multiple handlers
            server.RequestReceived += logger.LogRequest;
            server.RequestReceived += analytics.TrackRequest;

            Console.WriteLine("Simulating concurrent web requests...");

            // Simulate multiple threads making requests
            Task.Run(() => server.SimulateRequest("GET /api/users"));
            Task.Run(() => server.SimulateRequest("POST /api/orders"));
            Task.Run(() => server.SimulateRequest("GET /api/products"));

            // Give threads time to complete
            Thread.Sleep(100);
            
            Console.WriteLine("All requests processed safely in multithreaded environment.\n");
        }

        #endregion

        #region Custom Event Accessors Demonstrations

        static void CustomEventAccessorsDemo()
        {
            Console.WriteLine("4. CUSTOM EVENT ACCESSORS DEMONSTRATION");
            Console.WriteLine("=======================================");

            var chatRoom = new ChatRoom("General Discussion");
            var user1 = new ChatUser("Alice");
            var user2 = new ChatUser("Bob");

            // Subscribe users to chat messages
            chatRoom.MessageReceived += user1.OnMessageReceived;
            chatRoom.MessageReceived += user2.OnMessageReceived;

            Console.WriteLine($"Users joined chat room: {chatRoom.RoomName}");
            Console.WriteLine($"Active subscribers: {chatRoom.SubscriberCount}");
            Console.WriteLine();

            // Send messages
            chatRoom.SendMessage("Alice", "Hello everyone!");
            chatRoom.SendMessage("Bob", "Hey Alice, how's it going?");

            // Unsubscribe a user
            chatRoom.MessageReceived -= user1.OnMessageReceived;
            Console.WriteLine($"\nAfter Alice left - Active subscribers: {chatRoom.SubscriberCount}");
            
            chatRoom.SendMessage("Bob", "Alice left the chat...");

            Console.WriteLine();
        }

        #endregion

        #region Real World Scenario

        static void RealWorldScenarioDemo()
        {
            Console.WriteLine("5. REAL WORLD SCENARIO - E-COMMERCE SYSTEM");
            Console.WriteLine("==========================================");

            var orderProcessor = new OrderProcessor();
            var emailService = new EmailNotificationService();
            var inventoryService = new InventoryManagementService();
            var loyaltyService = new LoyaltyPointsService();

            // Wire up all the services to listen to order events
            orderProcessor.OrderPlaced += emailService.OnOrderPlaced;
            orderProcessor.OrderPlaced += inventoryService.OnOrderPlaced;
            orderProcessor.OrderPlaced += loyaltyService.OnOrderPlaced;

            orderProcessor.OrderCancelled += emailService.OnOrderCancelled;
            orderProcessor.OrderCancelled += inventoryService.OnOrderCancelled;

            // Process some orders
            Console.WriteLine("Processing customer orders...\n");
            
            var order1 = new Order(1001, "john@email.com", 299.99m);
            var order2 = new Order(1002, "jane@email.com", 149.50m);
            
            orderProcessor.PlaceOrder(order1);
            Console.WriteLine();
            
            orderProcessor.PlaceOrder(order2);
            Console.WriteLine();

            orderProcessor.CancelOrder(order1);
            Console.WriteLine();
        }

        #endregion
    }

    #region Basic Event Classes

    // Simple delegate for basic events
    public delegate void BreakingNewsHandler(string newsContent);

    // Broadcaster class - publishes news
    public class NewsAgency
    {
        // Declare the event using our custom delegate
        public event BreakingNewsHandler BreakingNews;

        public void PublishNews(string news)
        {
            Console.WriteLine($"News Agency: Publishing -> {news}");
            
            // Fire the event - notify all subscribers
            BreakingNews?.Invoke(news);
        }
    }

    // Subscriber class - receives news
    public class NewsSubscriber
    {
        private string _channelName;

        public NewsSubscriber(string channelName)
        {
            _channelName = channelName;
        }

        // Event handler method - must match delegate signature
        public void OnBreakingNews(string news)
        {
            Console.WriteLine($"  {_channelName} received: {news}");
        }
    }

    #endregion

    #region Standard Event Pattern Classes

    // Custom EventArgs class for price changes
    public class PriceChangedEventArgs : EventArgs
    {
        public string Symbol { get; }
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }
        public decimal Change => NewPrice - OldPrice;
        public decimal PercentChange => OldPrice == 0 ? 0 : (Change / OldPrice) * 100;

        public PriceChangedEventArgs(string symbol, decimal oldPrice, decimal newPrice)
        {
            Symbol = symbol;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }

    // Stock class using standard event pattern
    public class Stock
    {
        private decimal _price;

        public string Symbol { get; }

        // Using the standard EventHandler<T> pattern
        public event EventHandler<PriceChangedEventArgs> PriceChanged;

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    decimal oldPrice = _price;
                    _price = value;
                    
                    // Fire the event with proper EventArgs
                    OnPriceChanged(new PriceChangedEventArgs(Symbol, oldPrice, value));
                }
            }
        }

        public Stock(string symbol, decimal initialPrice)
        {
            Symbol = symbol;
            _price = initialPrice;
        }

        // Protected virtual method to fire the event
        // This is the standard pattern - allows derived classes to override
        protected virtual void OnPriceChanged(PriceChangedEventArgs e)
        {
            // Thread-safe event firing using null-conditional operator
            PriceChanged?.Invoke(this, e);
        }
    }

    // Investor class that reacts to stock price changes
    public class Investor
    {
        public string Name { get; }

        public Investor(string name)
        {
            Name = name;
        }

        // Event handler that follows the standard pattern
        public void OnPriceChanged(object sender, PriceChangedEventArgs e)
        {
            var direction = e.Change >= 0 ? "↑" : "↓";
            var color = e.Change >= 0 ? "green" : "red";
            
            Console.WriteLine($"  {Name}: {e.Symbol} moved {direction} from ${e.OldPrice:F2} to ${e.NewPrice:F2} " +
                            $"(Change: ${e.Change:F2}, {e.PercentChange:F1}%)");
        }
    }

    #endregion

    #region Thread Safety Classes

    public class RequestEventArgs : EventArgs
    {
        public string Method { get; }
        public string Path { get; }
        public DateTime Timestamp { get; }
        public int ThreadId { get; }

        public RequestEventArgs(string method, string path)
        {
            Method = method;
            Path = path;
            Timestamp = DateTime.Now;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }
    }

    public class WebServer
    {
        // Thread-safe event declaration
        public event EventHandler<RequestEventArgs> RequestReceived;

        public void SimulateRequest(string request)
        {
            var parts = request.Split(' ');
            var method = parts[0];
            var path = parts[1];

            Console.WriteLine($"  Server processing: {request} on thread {Thread.CurrentThread.ManagedThreadId}");

            // Create event args
            var eventArgs = new RequestEventArgs(method, path);

            // Thread-safe event firing
            OnRequestReceived(eventArgs);
        }

        protected virtual void OnRequestReceived(RequestEventArgs e)
        {
            // The ?. operator ensures thread safety
            // If another thread removes all handlers between the check and invoke,
            // this won't throw a NullReferenceException
            RequestReceived?.Invoke(this, e);
        }
    }

    public class RequestLogger
    {
        public void LogRequest(object sender, RequestEventArgs e)
        {
            Console.WriteLine($"  Logger: [{e.Timestamp:HH:mm:ss.fff}] {e.Method} {e.Path} (Thread: {e.ThreadId})");
        }
    }

    public class AnalyticsService
    {
        public void TrackRequest(object sender, RequestEventArgs e)
        {
            Console.WriteLine($"  Analytics: Tracking {e.Method} request to {e.Path}");
        }
    }

    #endregion

    #region Custom Event Accessors Classes

    public class MessageEventArgs : EventArgs
    {
        public string Sender { get; }
        public string Message { get; }
        public DateTime Timestamp { get; }

        public MessageEventArgs(string sender, string message)
        {
            Sender = sender;
            Message = message;
            Timestamp = DateTime.Now;
        }
    }

    // Chat room with custom event accessors
    public class ChatRoom
    {
        private EventHandler<MessageEventArgs> _messageReceived;
        private int _subscriberCount = 0;

        public string RoomName { get; }
        public int SubscriberCount => _subscriberCount;

        public ChatRoom(string roomName)
        {
            RoomName = roomName;
        }

        // Custom event with explicit add/remove accessors
        public event EventHandler<MessageEventArgs> MessageReceived
        {
            add
            {
                _messageReceived += value;
                _subscriberCount++;
                Console.WriteLine($"  User joined chat room. Total subscribers: {_subscriberCount}");
            }
            remove
            {
                _messageReceived -= value;
                _subscriberCount--;
                Console.WriteLine($"  User left chat room. Total subscribers: {_subscriberCount}");
            }
        }

        public void SendMessage(string sender, string message)
        {
            Console.WriteLine($"  {sender}: {message}");
            
            var eventArgs = new MessageEventArgs(sender, message);
            OnMessageReceived(eventArgs);
        }

        protected virtual void OnMessageReceived(MessageEventArgs e)
        {
            _messageReceived?.Invoke(this, e);
        }
    }

    public class ChatUser
    {
        public string Username { get; }

        public ChatUser(string username)
        {
            Username = username;
        }

        public void OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Sender != Username) // Don't notify yourself
            {
                Console.WriteLine($"    {Username} saw message from {e.Sender}");
            }
        }
    }

    #endregion

    #region Real World E-Commerce Classes

    public class Order
    {
        public int OrderId { get; }
        public string CustomerEmail { get; }
        public decimal Amount { get; }
        public DateTime OrderDate { get; }

        public Order(int orderId, string customerEmail, decimal amount)
        {
            OrderId = orderId;
            CustomerEmail = customerEmail;
            Amount = amount;
            OrderDate = DateTime.Now;
        }
    }

    public class OrderEventArgs : EventArgs
    {
        public Order Order { get; }

        public OrderEventArgs(Order order)
        {
            Order = order;
        }
    }

    // Main order processor - the broadcaster
    public class OrderProcessor
    {
        public event EventHandler<OrderEventArgs> OrderPlaced;
        public event EventHandler<OrderEventArgs> OrderCancelled;

        public void PlaceOrder(Order order)
        {
            Console.WriteLine($"Order #{order.OrderId} placed for ${order.Amount} by {order.CustomerEmail}");
            
            // Fire the event - all subscribers will be notified
            OnOrderPlaced(new OrderEventArgs(order));
        }

        public void CancelOrder(Order order)
        {
            Console.WriteLine($"Order #{order.OrderId} cancelled");
            
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

    // Email service - subscriber
    public class EmailNotificationService
    {
        public void OnOrderPlaced(object sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Email Service: Sending confirmation email to {e.Order.CustomerEmail}");
            Console.WriteLine($"    Subject: Order #{e.Order.OrderId} confirmed - ${e.Order.Amount}");
        }

        public void OnOrderCancelled(object sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Email Service: Sending cancellation email to {e.Order.CustomerEmail}");
            Console.WriteLine($"    Subject: Order #{e.Order.OrderId} has been cancelled");
        }
    }

    // Inventory service - subscriber
    public class InventoryManagementService
    {
        public void OnOrderPlaced(object sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Inventory Service: Reserving items for order #{e.Order.OrderId}");
            Console.WriteLine($"    Processing inventory allocation...");
        }

        public void OnOrderCancelled(object sender, OrderEventArgs e)
        {
            Console.WriteLine($"  Inventory Service: Releasing reserved items for order #{e.Order.OrderId}");
            Console.WriteLine($"    Items returned to available inventory");
        }
    }

    // Loyalty service - subscriber
    public class LoyaltyPointsService
    {
        public void OnOrderPlaced(object sender, OrderEventArgs e)
        {
            int pointsEarned = (int)(e.Order.Amount / 10); // 1 point per $10
            Console.WriteLine($"  Loyalty Service: Adding {pointsEarned} points to {e.Order.CustomerEmail}");
            Console.WriteLine($"    Customer reward points updated");
        }
    }

    #endregion
}
