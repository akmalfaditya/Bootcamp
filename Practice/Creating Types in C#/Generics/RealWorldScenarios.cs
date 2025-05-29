using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Generics
{
    /// <summary>
    /// Generic Repository Pattern - the most common real-world use of generics
    /// This pattern provides CRUD operations for any entity type
    /// Used in almost every business application for data access
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class GenericRepository<T> where T : class
    {
        private readonly List<T> entities = new List<T>();
        private readonly string repositoryName;

        public GenericRepository()
        {
            repositoryName = $"{typeof(T).Name}Repository";
            Console.WriteLine($"  📁 Created {repositoryName}");
        }

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entities.Add(entity);
            Console.WriteLine($"  ➕ {repositoryName}: Added {entity}");
        }

        /// <summary>
        /// Add multiple entities at once
        /// </summary>
        /// <param name="newEntities">Entities to add</param>
        public void AddRange(IEnumerable<T> newEntities)
        {
            foreach (T entity in newEntities)
            {
                Add(entity);
            }
        }

        /// <summary>
        /// Remove an entity
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        /// <returns>True if removed, false if not found</returns>
        public bool Remove(T entity)
        {
            bool removed = entities.Remove(entity);
            if (removed)
            {
                Console.WriteLine($"  ➖ {repositoryName}: Removed {entity}");
            }
            return removed;
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>All entities in the repository</returns>
        public IReadOnlyList<T> GetAll()
        {
            return entities.AsReadOnly();
        }

        /// <summary>
        /// Find entities matching a condition
        /// This is where generics really shine - works with any property of T
        /// </summary>
        /// <param name="predicate">Condition to match</param>
        /// <returns>Matching entities</returns>
        public List<T> FindWhere(Func<T, bool> predicate)
        {
            List<T> results = new List<T>();
            foreach (T entity in entities)
            {
                if (predicate(entity))
                {
                    results.Add(entity);
                }
            }
            
            Console.WriteLine($"  🔍 {repositoryName}: Found {results.Count} matching entities");
            return results;
        }

        /// <summary>
        /// Find first entity matching condition
        /// </summary>
        /// <param name="predicate">Condition to match</param>
        /// <returns>First matching entity or null</returns>
        public T? FindFirst(Func<T, bool> predicate)
        {
            foreach (T entity in entities)
            {
                if (predicate(entity))
                {
                    return entity;
                }
            }
            return null;
        }

        /// <summary>
        /// Update entities matching a condition
        /// </summary>
        /// <param name="predicate">Condition to match</param>
        /// <param name="updateAction">Action to perform on matching entities</param>
        /// <returns>Number of entities updated</returns>
        public int UpdateWhere(Func<T, bool> predicate, Action<T> updateAction)
        {
            int updateCount = 0;
            foreach (T entity in entities)
            {
                if (predicate(entity))
                {
                    updateAction(entity);
                    updateCount++;
                }
            }
            
            Console.WriteLine($"  ✏️ {repositoryName}: Updated {updateCount} entities");
            return updateCount;
        }

        /// <summary>
        /// Get count of entities
        /// </summary>
        public int Count => entities.Count;

        /// <summary>
        /// Check if repository contains an entity
        /// </summary>
        /// <param name="entity">Entity to check</param>
        /// <returns>True if found</returns>
        public bool Contains(T entity)
        {
            return entities.Contains(entity);
        }

        /// <summary>
        /// Clear all entities
        /// </summary>
        public void Clear()
        {
            int oldCount = entities.Count;
            entities.Clear();
            Console.WriteLine($"  🗑️ {repositoryName}: Cleared {oldCount} entities");
        }
    }

    /// <summary>
    /// Generic Caching System - another common real-world pattern
    /// Provides type-safe caching with TTL (time-to-live) support
    /// Used for performance optimization in web applications
    /// </summary>
    /// <typeparam name="TKey">Type of cache keys</typeparam>
    /// <typeparam name="TValue">Type of cached values</typeparam>
    public class GenericCache<TKey, TValue> where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, CacheItem<TValue>> cache;
        private readonly TimeSpan defaultTtl;

        public GenericCache(TimeSpan? defaultTtl = null)
        {
            this.cache = new ConcurrentDictionary<TKey, CacheItem<TValue>>();
            this.defaultTtl = defaultTtl ?? TimeSpan.FromMinutes(30);
            
            Console.WriteLine($"  💾 Created cache for <{typeof(TKey).Name}, {typeof(TValue).Name}>");
            Console.WriteLine($"  ⏰ Default TTL: {this.defaultTtl.TotalMinutes} minutes");
        }

        /// <summary>
        /// Set a value in the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to cache</param>
        /// <param name="ttl">Custom time-to-live (optional)</param>
        public void Set(TKey key, TValue value, TimeSpan? ttl = null)
        {
            TimeSpan actualTtl = ttl ?? defaultTtl;
            CacheItem<TValue> item = new CacheItem<TValue>(value, DateTime.UtcNow.Add(actualTtl));
            
            cache.AddOrUpdate(key, item, (k, oldItem) => item);
            Console.WriteLine($"  💾 Cached [{key}] = {value} (expires in {actualTtl.TotalMinutes:F1} min)");
        }

        /// <summary>
        /// Get a value from the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cached value or null if not found/expired</returns>
        public TValue? Get(TKey key)
        {
            if (cache.TryGetValue(key, out CacheItem<TValue>? item))
            {
                if (item.IsExpired)
                {
                    // Remove expired item
                    cache.TryRemove(key, out _);
                    Console.WriteLine($"  ⏰ Cache miss [{key}] - expired");
                    return default(TValue);
                }
                
                Console.WriteLine($"  ✅ Cache hit [{key}] = {item.Value}");
                return item.Value;
            }
            
            Console.WriteLine($"  ❌ Cache miss [{key}] - not found");
            return default(TValue);
        }

        /// <summary>
        /// Get or set pattern - common caching scenario
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="valueFactory">Function to create value if not cached</param>
        /// <param name="ttl">Custom TTL</param>
        /// <returns>Cached or newly created value</returns>
        public TValue GetOrSet(TKey key, Func<TValue> valueFactory, TimeSpan? ttl = null)
        {
            TValue? cachedValue = Get(key);
            if (cachedValue != null)
            {
                return cachedValue;
            }
            
            // Not in cache, create and cache the value
            TValue newValue = valueFactory();
            Set(key, newValue, ttl);
            Console.WriteLine($"  🏭 Created and cached new value for [{key}]");
            return newValue;
        }

        /// <summary>
        /// Remove a specific key from cache
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>True if removed</returns>
        public bool Remove(TKey key)
        {
            bool removed = cache.TryRemove(key, out _);
            if (removed)
            {
                Console.WriteLine($"  🗑️ Removed [{key}] from cache");
            }
            return removed;
        }

        /// <summary>
        /// Clear expired items from cache
        /// </summary>
        /// <returns>Number of items removed</returns>
        public int ClearExpired()
        {
            int removedCount = 0;
            DateTime now = DateTime.UtcNow;
            
            List<TKey> expiredKeys = new List<TKey>();
            foreach (var kvp in cache)
            {
                if (kvp.Value.IsExpired)
                {
                    expiredKeys.Add(kvp.Key);
                }
            }
            
            foreach (TKey key in expiredKeys)
            {
                if (cache.TryRemove(key, out _))
                {
                    removedCount++;
                }
            }
            
            Console.WriteLine($"  🧹 Cleared {removedCount} expired items from cache");
            return removedCount;
        }

        /// <summary>
        /// Get cache statistics
        /// </summary>
        public void ShowStatistics()
        {
            int totalItems = cache.Count;
            int expiredItems = 0;
            
            foreach (var kvp in cache)
            {
                if (kvp.Value.IsExpired)
                {
                    expiredItems++;
                }
            }
            
            Console.WriteLine($"  📊 Cache Statistics:");
            Console.WriteLine($"      Total items: {totalItems}");
            Console.WriteLine($"      Valid items: {totalItems - expiredItems}");
            Console.WriteLine($"      Expired items: {expiredItems}");
        }
    }

    /// <summary>
    /// Cache item wrapper with expiration
    /// </summary>
    /// <typeparam name="T">Type of cached value</typeparam>
    public class CacheItem<T>
    {
        public T Value { get; }
        public DateTime ExpiresAt { get; }
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        public CacheItem(T value, DateTime expiresAt)
        {
            Value = value;
            ExpiresAt = expiresAt;
        }
    }

    /// <summary>
    /// Generic Event System - publish/subscribe pattern with generics
    /// Allows type-safe event handling for different event types
    /// Used in messaging systems, UI frameworks, and domain events
    /// </summary>
    public class GenericEventBus
    {
        private readonly Dictionary<Type, List<object>> subscribers;

        public GenericEventBus()
        {
            subscribers = new Dictionary<Type, List<object>>();
            Console.WriteLine("  📡 Created Generic Event Bus");
        }

        /// <summary>
        /// Subscribe to events of a specific type
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="handler">Handler function</param>
        public void Subscribe<T>(Action<T> handler)
        {
            Type eventType = typeof(T);
            
            if (!subscribers.ContainsKey(eventType))
            {
                subscribers[eventType] = new List<object>();
            }
            
            subscribers[eventType].Add(handler);
            Console.WriteLine($"  📝 Subscribed to {eventType.Name} events");
        }

        /// <summary>
        /// Unsubscribe from events
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="handler">Handler to remove</param>
        public void Unsubscribe<T>(Action<T> handler)
        {
            Type eventType = typeof(T);
            
            if (subscribers.ContainsKey(eventType))
            {
                subscribers[eventType].Remove(handler);
                Console.WriteLine($"  📝 Unsubscribed from {eventType.Name} events");
            }
        }

        /// <summary>
        /// Publish an event to all subscribers
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="eventData">Event data</param>
        public void Publish<T>(T eventData)
        {
            Type eventType = typeof(T);
            Console.WriteLine($"  📢 Publishing {eventType.Name} event: {eventData}");
            
            if (subscribers.ContainsKey(eventType))
            {
                foreach (object subscriber in subscribers[eventType])
                {
                    if (subscriber is Action<T> typedHandler)
                    {
                        try
                        {
                            typedHandler(eventData);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"  ⚠️ Error in event handler: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"  📭 No subscribers for {eventType.Name} events");
            }
        }

        /// <summary>
        /// Publish event asynchronously
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="eventData">Event data</param>
        public async Task PublishAsync<T>(T eventData)
        {
            Type eventType = typeof(T);
            Console.WriteLine($"  📢 Publishing {eventType.Name} event asynchronously: {eventData}");
            
            if (subscribers.ContainsKey(eventType))
            {
                List<Task> tasks = new List<Task>();
                
                foreach (object subscriber in subscribers[eventType])
                {
                    if (subscriber is Action<T> typedHandler)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            try
                            {
                                typedHandler(eventData);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"  ⚠️ Error in async event handler: {ex.Message}");
                            }
                        }));
                    }
                }
                
                await Task.WhenAll(tasks);
                Console.WriteLine($"  ✅ All async handlers completed for {eventType.Name}");
            }
        }

        /// <summary>
        /// Get subscriber count for an event type
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <returns>Number of subscribers</returns>
        public int GetSubscriberCount<T>()
        {
            Type eventType = typeof(T);
            return subscribers.ContainsKey(eventType) ? subscribers[eventType].Count : 0;
        }
    }

    // Sample entity classes for repository demonstration

    /// <summary>
    /// User entity for repository demonstration
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public User(int id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        public override string ToString()
        {
            return $"User[{Id}]: {Username} ({Email}) - {(IsActive ? "Active" : "Inactive")}";
        }

        public override bool Equals(object? obj)
        {
            return obj is User other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    /// <summary>
    /// Product entity for repository demonstration
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }

        public Product(int id, string name, decimal price, int stockQuantity = 0, string category = "General")
        {
            Id = id;
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
            Category = category;
        }

        public override string ToString()
        {
            return $"Product[{Id}]: {Name} - ${Price:F2} (Stock: {StockQuantity}) [{Category}]";
        }

        public override bool Equals(object? obj)
        {
            return obj is Product other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    // Sample event types for event bus demonstration

    /// <summary>
    /// User logged in event
    /// </summary>
    public class UserLoggedIn
    {
        public string Username { get; }
        public DateTime Timestamp { get; }
        public string IpAddress { get; }

        public UserLoggedIn(string username, DateTime timestamp, string ipAddress = "127.0.0.1")
        {
            Username = username;
            Timestamp = timestamp;
            IpAddress = ipAddress;
        }

        public override string ToString()
        {
            return $"{Username} logged in at {Timestamp:HH:mm:ss} from {IpAddress}";
        }
    }

    /// <summary>
    /// Order placed event
    /// </summary>
    public class OrderPlaced
    {
        public int OrderId { get; }
        public decimal Amount { get; }
        public DateTime Timestamp { get; }
        public string CustomerEmail { get; }

        public OrderPlaced(int orderId, decimal amount, DateTime timestamp, string customerEmail = "customer@example.com")
        {
            OrderId = orderId;
            Amount = amount;
            Timestamp = timestamp;
            CustomerEmail = customerEmail;
        }

        public override string ToString()
        {
            return $"Order #{OrderId} for ${Amount:F2} placed at {Timestamp:HH:mm:ss} by {CustomerEmail}";
        }
    }

    /// <summary>
    /// Product updated event
    /// </summary>
    public class ProductUpdated
    {
        public int ProductId { get; }
        public string PropertyName { get; }
        public object OldValue { get; }
        public object NewValue { get; }
        public DateTime Timestamp { get; }

        public ProductUpdated(int productId, string propertyName, object oldValue, object newValue)
        {
            ProductId = productId;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Product #{ProductId}.{PropertyName} changed from '{OldValue}' to '{NewValue}' at {Timestamp:HH:mm:ss}";
        }
    }
}
