using System;
using System.Collections.Generic;
using System.Linq;

namespace NestedTypes
{
    /// <summary>
    /// Real-world scenarios demonstrating practical uses of nested types
    /// These examples show how nested types solve real problems in production code
    /// Think of these as battle-tested patterns you'll actually use in your career!
    /// </summary>
    /// 
    /// <summary>
    /// Configuration management system with nested types
    /// This pattern is common in enterprise applications where configuration has multiple layers
    /// </summary>
    public class DatabaseConfig
    {
        private string connectionString;
        private int timeoutSeconds;
        private bool enableRetry;
        private ConnectionPool pool;

        public DatabaseConfig(string server, string database, string username, string password)
        {
            connectionString = $"Server={server};Database={database};User={username};Password={password};";
            timeoutSeconds = 30;
            enableRetry = true;
            pool = new ConnectionPool(this);
            
            Console.WriteLine($"Database config created for {database} on {server}");
        }

        /// <summary>
        /// Nested class for connection pool management
        /// This is perfect as a nested type because it's tightly coupled to the database config
        /// and needs access to connection details
        /// </summary>
        public class ConnectionPool
        {
            private DatabaseConfig config;
            private List<Connection> availableConnections;
            private List<Connection> activeConnections;
            private int maxPoolSize;

            internal ConnectionPool(DatabaseConfig config)
            {
                this.config = config;
                this.availableConnections = new List<Connection>();
                this.activeConnections = new List<Connection>();
                this.maxPoolSize = 10;
                
                Console.WriteLine("Connection pool initialized");
            }

            public Connection GetConnection()
            {
                if (availableConnections.Any())
                {
                    Connection conn = availableConnections.First();
                    availableConnections.Remove(conn);
                    activeConnections.Add(conn);
                    Console.WriteLine($"Reused connection from pool (Active: {activeConnections.Count})");
                    return conn;
                }
                
                if (activeConnections.Count < maxPoolSize)
                {
                    // Access parent's private connection string!
                    Connection newConn = new Connection(config.connectionString, config.timeoutSeconds);
                    activeConnections.Add(newConn);
                    Console.WriteLine($"Created new connection (Active: {activeConnections.Count})");
                    return newConn;
                }
                
                throw new InvalidOperationException("Connection pool exhausted");
            }

            public void ReturnConnection(Connection connection)
            {
                if (activeConnections.Remove(connection))
                {
                    availableConnections.Add(connection);
                    Console.WriteLine($"Connection returned to pool (Available: {availableConnections.Count})");
                }
            }

            public void ShowPoolStatus()
            {
                Console.WriteLine($"\n=== Connection Pool Status ===");
                Console.WriteLine($"Max Pool Size: {maxPoolSize}");
                Console.WriteLine($"Active Connections: {activeConnections.Count}");
                Console.WriteLine($"Available Connections: {availableConnections.Count}");
                Console.WriteLine($"Total Connections: {activeConnections.Count + availableConnections.Count}");
            }
        }

        /// <summary>
        /// Nested class representing individual database connections
        /// Another example of tight coupling that benefits from nested type organization
        /// </summary>
        public class Connection : IDisposable
        {
            private string connectionString;
            private int timeoutSeconds;
            private bool isOpen;
            private DateTime lastUsed;

            internal Connection(string connectionString, int timeoutSeconds)
            {
                this.connectionString = connectionString;
                this.timeoutSeconds = timeoutSeconds;
                this.isOpen = false;
                this.lastUsed = DateTime.Now;
            }

            public void Open()
            {
                if (!isOpen)
                {
                    isOpen = true;
                    lastUsed = DateTime.Now;
                    Console.WriteLine($"Database connection opened (Timeout: {timeoutSeconds}s)");
                }
            }

            public void ExecuteQuery(string sql)
            {
                if (!isOpen)
                    throw new InvalidOperationException("Connection not open");
                
                lastUsed = DateTime.Now;
                Console.WriteLine($"Executing query: {sql.Substring(0, Math.Min(50, sql.Length))}...");
                
                // Simulate query execution
                System.Threading.Thread.Sleep(100);
                Console.WriteLine("Query completed successfully");
            }

            public void Close()
            {
                if (isOpen)
                {
                    isOpen = false;
                    Console.WriteLine("Database connection closed");
                }
            }

            public void Dispose()
            {
                Close();
            }
        }

        public ConnectionPool GetConnectionPool() => pool;

        public void TestDatabaseOperations()
        {
            Console.WriteLine("\n=== Testing Database Operations ===");
            
            var connection1 = pool.GetConnection();
            connection1.Open();
            connection1.ExecuteQuery("SELECT * FROM Users WHERE Active = 1");
            
            var connection2 = pool.GetConnection();
            connection2.Open();
            connection2.ExecuteQuery("SELECT COUNT(*) FROM Orders WHERE Date >= '2024-01-01'");
            
            pool.ShowPoolStatus();
            
            pool.ReturnConnection(connection1);
            pool.ReturnConnection(connection2);
            
            pool.ShowPoolStatus();
        }
    }

    /// <summary>
    /// Pizza builder pattern with nested types
    /// This shows how nested types can create fluent, type-safe APIs
    /// Perfect example of the builder pattern with domain-specific nested types
    /// </summary>
    public class Pizza
    {
        private List<string> toppings;
        private CrustType crust;
        private SizeType size;
        private List<string> specialInstructions;
        private decimal basePrice;

        private Pizza()
        {
            toppings = new List<string>();
            specialInstructions = new List<string>();
            crust = CrustType.Regular;
            size = SizeType.Medium;
            basePrice = 12.99m;
        }

        /// <summary>
        /// Nested enum for crust types - perfectly scoped to Pizza
        /// </summary>
        public enum CrustType
        {
            Thin,
            Regular,
            Thick,
            Stuffed,
            Gluten_Free
        }

        /// <summary>
        /// Nested enum for sizes - another perfect use case
        /// </summary>
        public enum SizeType
        {
            Personal,
            Small,
            Medium,
            Large,
            ExtraLarge
        }

        /// <summary>
        /// Nested builder class - implements fluent interface pattern
        /// This is a classic use case where the builder is tightly coupled to the product
        /// </summary>
        public class Builder
        {
            private Pizza pizza;

            public Builder()
            {
                pizza = new Pizza();
                Console.WriteLine("🍕 Starting pizza order...");
            }

            public Builder WithSize(SizeType size)
            {
                pizza.size = size;
                pizza.basePrice = size switch
                {
                    SizeType.Personal => 8.99m,
                    SizeType.Small => 10.99m,
                    SizeType.Medium => 12.99m,
                    SizeType.Large => 15.99m,
                    SizeType.ExtraLarge => 18.99m,
                    _ => 12.99m
                };
                Console.WriteLine($"Size set to {size} (${pizza.basePrice:C})");
                return this;
            }

            public Builder WithCrust(CrustType crust)
            {
                pizza.crust = crust;
                Console.WriteLine($"Crust set to {crust}");
                return this;
            }

            public Builder AddTopping(string topping)
            {
                pizza.toppings.Add(topping);
                Console.WriteLine($"Added topping: {topping}");
                return this;
            }

            public Builder AddToppings(params string[] toppings)
            {
                foreach (string topping in toppings)
                {
                    AddTopping(topping);
                }
                return this;
            }

            public Builder WithSpecialInstructions(string instructions)
            {
                pizza.specialInstructions.Add(instructions);
                Console.WriteLine($"Special instructions: {instructions}");
                return this;
            }

            /// <summary>
            /// Nested class for pre-configured pizza recipes
            /// Shows how nested types can provide convenient factory methods
            /// </summary>
            public class Recipes
            {
                public static Builder Margherita(Builder builder)
                {
                    return builder
                        .WithCrust(CrustType.Thin)
                        .AddToppings("Mozzarella", "Tomato Sauce", "Fresh Basil")
                        .WithSpecialInstructions("Extra virgin olive oil drizzle");
                }

                public static Builder Supreme(Builder builder)
                {
                    return builder
                        .WithCrust(CrustType.Regular)
                        .AddToppings("Pepperoni", "Sausage", "Bell Peppers", "Onions", "Mushrooms", "Olives");
                }

                public static Builder Vegetarian(Builder builder)
                {
                    return builder
                        .WithCrust(CrustType.Regular)
                        .AddToppings("Mushrooms", "Bell Peppers", "Onions", "Tomatoes", "Spinach", "Feta Cheese");
                }

                public static Builder MeatLovers(Builder builder)
                {
                    return builder
                        .WithCrust(CrustType.Thick)
                        .AddToppings("Pepperoni", "Sausage", "Ham", "Bacon", "Ground Beef");
                }
            }

            public Pizza Build()
            {
                Console.WriteLine("🍕 Pizza order completed!");
                return pizza;
            }
        }

        public decimal CalculatePrice()
        {
            decimal price = basePrice;
            price += toppings.Count * 1.50m; // $1.50 per topping
            
            // Crust price modifier
            price += crust switch
            {
                CrustType.Stuffed => 3.00m,
                CrustType.Gluten_Free => 2.50m,
                _ => 0m
            };

            return price;
        }

        public void ShowOrder()
        {
            Console.WriteLine("\n🍕 === Pizza Order Summary ===");
            Console.WriteLine($"Size: {size}");
            Console.WriteLine($"Crust: {crust}");
            Console.WriteLine($"Toppings ({toppings.Count}): {string.Join(", ", toppings)}");
            
            if (specialInstructions.Any())
            {
                Console.WriteLine($"Special Instructions:");
                foreach (string instruction in specialInstructions)
                {
                    Console.WriteLine($"  • {instruction}");
                }
            }
            
            Console.WriteLine($"Total Price: ${CalculatePrice():C}");
        }

        // Static factory method to start building
        public static Builder CreateBuilder() => new Builder();
    }

    /// <summary>
    /// Order processing system with nested state machine
    /// This demonstrates how nested types can implement complex state patterns
    /// </summary>
    public class OrderProcessor
    {
        private string orderId;
        private DateTime orderDate;
        private List<OrderItem> items;
        private OrderState currentState;
        private List<string> statusHistory;

        public OrderProcessor(string orderId)
        {
            this.orderId = orderId;
            this.orderDate = DateTime.Now;
            this.items = new List<OrderItem>();
            this.statusHistory = new List<string>();
            this.currentState = new PendingState(this);
            
            AddStatusHistory("Order created");
            Console.WriteLine($"Order {orderId} created");
        }

        /// <summary>
        /// Nested abstract base class for state pattern
        /// Perfect use of nested types for closely related state management
        /// </summary>
        public abstract class OrderState
        {
            protected OrderProcessor processor;

            protected OrderState(OrderProcessor processor)
            {
                this.processor = processor;
            }

            public abstract void Process();
            public abstract void Cancel();
            public abstract void Ship();
            public abstract void Complete();
            public abstract string GetStatusName();
        }

        /// <summary>
        /// Concrete state implementations as nested classes
        /// Each state knows how to handle specific operations
        /// </summary>
        public class PendingState : OrderState
        {
            public PendingState(OrderProcessor processor) : base(processor) { }

            public override void Process()
            {
                processor.TransitionTo(new ProcessingState(processor));
                processor.AddStatusHistory("Order processing started");
                Console.WriteLine($"Order {processor.orderId} is now being processed");
            }

            public override void Cancel()
            {
                processor.TransitionTo(new CancelledState(processor));
                processor.AddStatusHistory("Order cancelled while pending");
                Console.WriteLine($"Order {processor.orderId} was cancelled");
            }

            public override void Ship()
            {
                throw new InvalidOperationException("Cannot ship pending order");
            }

            public override void Complete()
            {
                throw new InvalidOperationException("Cannot complete pending order");
            }

            public override string GetStatusName() => "Pending";
        }

        public class ProcessingState : OrderState
        {
            public ProcessingState(OrderProcessor processor) : base(processor) { }

            public override void Process()
            {
                Console.WriteLine($"Order {processor.orderId} is already being processed");
            }

            public override void Cancel()
            {
                processor.TransitionTo(new CancelledState(processor));
                processor.AddStatusHistory("Order cancelled during processing");
                Console.WriteLine($"Order {processor.orderId} was cancelled during processing");
            }

            public override void Ship()
            {
                processor.TransitionTo(new ShippedState(processor));
                processor.AddStatusHistory("Order shipped");
                Console.WriteLine($"Order {processor.orderId} has been shipped");
            }

            public override void Complete()
            {
                throw new InvalidOperationException("Cannot complete order that hasn't been shipped");
            }

            public override string GetStatusName() => "Processing";
        }

        public class ShippedState : OrderState
        {
            public ShippedState(OrderProcessor processor) : base(processor) { }

            public override void Process()
            {
                throw new InvalidOperationException("Cannot process shipped order");
            }

            public override void Cancel()
            {
                throw new InvalidOperationException("Cannot cancel shipped order");
            }

            public override void Ship()
            {
                Console.WriteLine($"Order {processor.orderId} is already shipped");
            }

            public override void Complete()
            {
                processor.TransitionTo(new CompletedState(processor));
                processor.AddStatusHistory("Order completed");
                Console.WriteLine($"Order {processor.orderId} has been completed");
            }

            public override string GetStatusName() => "Shipped";
        }

        public class CompletedState : OrderState
        {
            public CompletedState(OrderProcessor processor) : base(processor) { }

            public override void Process()
            {
                throw new InvalidOperationException("Cannot process completed order");
            }

            public override void Cancel()
            {
                throw new InvalidOperationException("Cannot cancel completed order");
            }

            public override void Ship()
            {
                throw new InvalidOperationException("Cannot ship completed order");
            }

            public override void Complete()
            {
                Console.WriteLine($"Order {processor.orderId} is already completed");
            }

            public override string GetStatusName() => "Completed";
        }

        public class CancelledState : OrderState
        {
            public CancelledState(OrderProcessor processor) : base(processor) { }

            public override void Process()
            {
                throw new InvalidOperationException("Cannot process cancelled order");
            }

            public override void Cancel()
            {
                Console.WriteLine($"Order {processor.orderId} is already cancelled");
            }

            public override void Ship()
            {
                throw new InvalidOperationException("Cannot ship cancelled order");
            }

            public override void Complete()
            {
                throw new InvalidOperationException("Cannot complete cancelled order");
            }

            public override string GetStatusName() => "Cancelled";
        }

        /// <summary>
        /// Nested class for order items
        /// Tightly coupled to the order, perfect for nested type
        /// </summary>
        public class OrderItem
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice => Quantity * UnitPrice;

            public OrderItem(string productId, string productName, int quantity, decimal unitPrice)
            {
                ProductId = productId;
                ProductName = productName;
                Quantity = quantity;
                UnitPrice = unitPrice;
            }

            public override string ToString()
            {
                return $"{ProductName} (ID: {ProductId}) - Qty: {Quantity} x ${UnitPrice:C} = ${TotalPrice:C}";
            }
        }

        private void TransitionTo(OrderState newState)
        {
            currentState = newState;
        }

        private void AddStatusHistory(string status)
        {
            statusHistory.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {status}");
        }

        public void AddItem(string productId, string productName, int quantity, decimal unitPrice)
        {
            items.Add(new OrderItem(productId, productName, quantity, unitPrice));
            Console.WriteLine($"Added item to order: {productName}");
        }

        public void ProcessOrder() => currentState.Process();
        public void CancelOrder() => currentState.Cancel();
        public void ShipOrder() => currentState.Ship();
        public void CompleteOrder() => currentState.Complete();

        public void ShowOrderDetails()
        {
            Console.WriteLine($"\n=== Order Details: {orderId} ===");
            Console.WriteLine($"Order Date: {orderDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Current Status: {currentState.GetStatusName()}");
            Console.WriteLine($"Items ({items.Count}):");
            
            foreach (var item in items)
            {
                Console.WriteLine($"  • {item}");
            }
            
            decimal total = items.Sum(i => i.TotalPrice);
            Console.WriteLine($"Order Total: ${total:C}");
            
            Console.WriteLine("\nStatus History:");
            foreach (string status in statusHistory)
            {
                Console.WriteLine($"  • {status}");
            }
        }
    }
}
