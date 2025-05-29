using System;
using System.Collections.Generic;
using System.Linq;

namespace Interfaces
{
    /// <summary>
    /// Payment processor interface - real-world polymorphism example
    /// Same contract, completely different payment methods behind the scenes
    /// Perfect example of how interfaces enable flexible, extensible design
    /// </summary>
    public interface IPaymentProcessor
    {
        bool ValidatePayment(decimal amount);
        void ProcessPayment(decimal amount);
        string GetPaymentMethodName();
    }

    /// <summary>
    /// Credit card payment processor
    /// One way to handle payments
    /// </summary>
    public class CreditCardProcessor : IPaymentProcessor
    {
        public bool ValidatePayment(decimal amount)
        {
            // Credit card validation logic
            bool isValid = amount > 0 && amount <= 10000; // $10k limit
            Console.WriteLine($"CreditCard: Validating ${amount:F2} - {(isValid ? "Valid" : "Invalid")}");
            return isValid;
        }

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"CreditCard: Processing ${amount:F2}");
            Console.WriteLine("CreditCard: Contacting bank... Approved! ✅");
        }

        public string GetPaymentMethodName() => "Credit Card";
    }

    /// <summary>
    /// PayPal payment processor
    /// Different implementation, same interface
    /// </summary>
    public class PayPalProcessor : IPaymentProcessor
    {
        public bool ValidatePayment(decimal amount)
        {
            bool isValid = amount > 0 && amount <= 5000; // $5k limit
            Console.WriteLine($"PayPal: Validating ${amount:F2} - {(isValid ? "Valid" : "Invalid")}");
            return isValid;
        }

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"PayPal: Processing ${amount:F2}");
            Console.WriteLine("PayPal: Redirecting to PayPal... Payment confirmed! ✅");
        }

        public string GetPaymentMethodName() => "PayPal";
    }

    /// <summary>
    /// Bank transfer processor
    /// Yet another way to handle payments
    /// </summary>
    public class BankTransferProcessor : IPaymentProcessor
    {
        public bool ValidatePayment(decimal amount)
        {
            bool isValid = amount > 0 && amount <= 50000; // $50k limit
            Console.WriteLine($"BankTransfer: Validating ${amount:F2} - {(isValid ? "Valid" : "Invalid")}");
            return isValid;
        }

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"BankTransfer: Processing ${amount:F2}");
            Console.WriteLine("BankTransfer: Initiating ACH transfer... Transfer scheduled! ⏰");
        }

        public string GetPaymentMethodName() => "Bank Transfer";
    }

    /// <summary>
    /// Cryptocurrency processor
    /// Modern payment method, same old interface
    /// </summary>
    public class CryptocurrencyProcessor : IPaymentProcessor
    {
        public bool ValidatePayment(decimal amount)
        {
            bool isValid = amount > 0 && amount <= 100000; // $100k limit
            Console.WriteLine($"Crypto: Validating ${amount:F2} - {(isValid ? "Valid" : "Invalid")}");
            return isValid;
        }

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Crypto: Processing ${amount:F2}");
            Console.WriteLine("Crypto: Broadcasting to blockchain... Transaction confirmed! 🔗");
        }

        public string GetPaymentMethodName() => "Cryptocurrency";
    }

    // =================== REPOSITORY PATTERN ===================

    /// <summary>
    /// User entity for our repository example
    /// Simple data class representing a user
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// User repository interface - data access contract
    /// This is the Repository Pattern - abstract away data storage details
    /// Your business logic doesn't care if data comes from database, file, or memory
    /// </summary>
    public interface IUserRepository
    {
        void AddUser(User user);
        User? GetUser(int id);
        IEnumerable<User> GetAllUsers();
        void UpdateUser(User user);
        void DeleteUser(int id);
    }

    /// <summary>
    /// Database implementation of user repository
    /// In real app, this would talk to SQL Server, PostgreSQL, etc.
    /// For demo purposes, we'll simulate database operations
    /// </summary>
    public class DatabaseUserRepository : IUserRepository
    {
        // Simulating database with in-memory collection
        private List<User> _users = new();

        public void AddUser(User user)
        {
            Console.WriteLine($"DatabaseRepo: INSERT INTO Users... Adding {user.Name}");
            _users.Add(user);
        }

        public User? GetUser(int id)
        {
            Console.WriteLine($"DatabaseRepo: SELECT * FROM Users WHERE Id = {id}");
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            Console.WriteLine("DatabaseRepo: SELECT * FROM Users");
            return _users.ToList();
        }

        public void UpdateUser(User user)
        {
            Console.WriteLine($"DatabaseRepo: UPDATE Users SET... Updating {user.Name}");
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.Name = user.Name;
                existing.Email = user.Email;
            }
        }

        public void DeleteUser(int id)
        {
            Console.WriteLine($"DatabaseRepo: DELETE FROM Users WHERE Id = {id}");
            _users.RemoveAll(u => u.Id == id);
        }
    }

    /// <summary>
    /// In-memory implementation - perfect for testing!
    /// Same interface, completely different storage mechanism
    /// This is why interfaces are so powerful for testing
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private Dictionary<int, User> _users = new();

        public void AddUser(User user)
        {
            Console.WriteLine($"MemoryRepo: Storing {user.Name} in memory");
            _users[user.Id] = user;
        }

        public User? GetUser(int id)
        {
            Console.WriteLine($"MemoryRepo: Looking up user {id} in memory");
            return _users.TryGetValue(id, out User? user) ? user : null;
        }

        public IEnumerable<User> GetAllUsers()
        {
            Console.WriteLine("MemoryRepo: Returning all users from memory");
            return _users.Values;
        }

        public void UpdateUser(User user)
        {
            Console.WriteLine($"MemoryRepo: Updating {user.Name} in memory");
            _users[user.Id] = user;
        }

        public void DeleteUser(int id)
        {
            Console.WriteLine($"MemoryRepo: Removing user {id} from memory");
            _users.Remove(id);
        }
    }

    // =================== NOTIFICATION SYSTEM ===================

    /// <summary>
    /// Notification service interface
    /// Contract for sending notifications through different channels
    /// </summary>
    public interface INotificationService
    {
        void SendNotification(string message, string recipient);
        bool IsServiceAvailable();
    }

    /// <summary>
    /// Email notification service
    /// One way to notify users
    /// </summary>
    public class EmailNotificationService : INotificationService
    {
        public void SendNotification(string message, string recipient)
        {
            Console.WriteLine($"📧 Email: Sending to {recipient}");
            Console.WriteLine($"📧 Email: Subject: Notification");
            Console.WriteLine($"📧 Email: Body: {message}");
        }

        public bool IsServiceAvailable() => true; // Email is usually available
    }

    /// <summary>
    /// SMS notification service
    /// Different channel, same interface
    /// </summary>
    public class SmsNotificationService : INotificationService
    {
        public void SendNotification(string message, string recipient)
        {
            Console.WriteLine($"📱 SMS: Sending to {recipient}");
            Console.WriteLine($"📱 SMS: {message}");
        }

        public bool IsServiceAvailable() => DateTime.Now.Hour >= 8 && DateTime.Now.Hour <= 22; // Only during reasonable hours
    }

    /// <summary>
    /// Push notification service
    /// Modern notification method
    /// </summary>
    public class PushNotificationService : INotificationService
    {
        public void SendNotification(string message, string recipient)
        {
            Console.WriteLine($"🔔 Push: Sending notification to {recipient}");
            Console.WriteLine($"🔔 Push: {message}");
        }

        public bool IsServiceAvailable() => true; // Push notifications usually work
    }

    /// <summary>
    /// Notification manager that coordinates multiple notification services
    /// Shows how interfaces enable flexible composition of services
    /// </summary>
    public class NotificationManager
    {
        private List<INotificationService> _services = new();

        public void AddService(INotificationService service)
        {
            _services.Add(service);
            Console.WriteLine($"NotificationManager: Added {service.GetType().Name}");
        }

        public void SendNotification(string message, string recipient)
        {
            Console.WriteLine($"\nNotificationManager: Broadcasting message to all services...");
            
            foreach (var service in _services)
            {
                if (service.IsServiceAvailable())
                {
                    service.SendNotification(message, recipient);
                }
                else
                {
                    Console.WriteLine($"⚠️  {service.GetType().Name} is not available right now");
                }
            }
        }
    }

    // =================== CACHE SYSTEM ===================

    /// <summary>
    /// Cache interface - contract for caching systems
    /// Could be Redis, Memcached, in-memory, or file-based
    /// </summary>
    public interface ICache<T>
    {
        void Set(string key, T value, TimeSpan? expiration = null);
        T? Get(string key);
        bool Remove(string key);
        void Clear();
    }

    /// <summary>
    /// Simple in-memory cache implementation
    /// Shows how generic interfaces work
    /// </summary>
    public class MemoryCache<T> : ICache<T>
    {
        private Dictionary<string, (T Value, DateTime Expiration)> _cache = new();

        public void Set(string key, T value, TimeSpan? expiration = null)
        {
            var exp = expiration.HasValue ? DateTime.Now.Add(expiration.Value) : DateTime.MaxValue;
            _cache[key] = (value, exp);
            Console.WriteLine($"MemoryCache: Cached '{key}' until {exp:HH:mm:ss}");
        }

        public T? Get(string key)
        {
            if (_cache.TryGetValue(key, out var item))
            {
                if (DateTime.Now <= item.Expiration)
                {
                    Console.WriteLine($"MemoryCache: Cache hit for '{key}'");
                    return item.Value;
                }
                else
                {
                    _cache.Remove(key);
                    Console.WriteLine($"MemoryCache: Cache expired for '{key}'");
                }
            }
            
            Console.WriteLine($"MemoryCache: Cache miss for '{key}'");
            return default(T);
        }

        public bool Remove(string key)
        {
            bool removed = _cache.Remove(key);
            Console.WriteLine($"MemoryCache: {(removed ? "Removed" : "Failed to remove")} '{key}'");
            return removed;
        }

        public void Clear()
        {
            _cache.Clear();
            Console.WriteLine("MemoryCache: Cleared all entries");
        }
    }
}
