// WeakReferenceCache.cs
// This demonstrates a practical use case for weak references: caching
// Objects can be cached but still be garbage collected when memory pressure occurs

namespace WeakReferences
{
    // A cache that uses weak references so cached objects can still be collected
    // This is useful when you want to cache expensive objects but not prevent GC
    // 
    // LIMITATION: This strategy can be only mildly effective because you have limited 
    // control over when the GC runs and which generation it collects.
    // For robust caching, consider a two-level cache (strong + weak references)
    public class WeakReferenceCache
    {
        private readonly Dictionary<string, WeakReference> _cache;

        public WeakReferenceCache()
        {
            _cache = new Dictionary<string, WeakReference>();
        }

        // Add an item to the cache using a weak reference
        // The object can still be garbage collected even while cached
        public void Add(string key, ExpensiveObject item)
        {
            _cache[key] = new WeakReference(item);
            Console.WriteLine($"  Added to cache: {key} -> {item.Data}");
        }

        // Try to get an item from cache
        // Returns null if the object was garbage collected
        // SAFETY: Always assign Target to a local variable to prevent collection during use
        public ExpensiveObject? Get(string key)
        {
            if (_cache.TryGetValue(key, out var weakRef))
            {
                // SAFETY: Always assign Target to local variable first!
                var target = weakRef.Target as ExpensiveObject;
                if (target != null)
                {
                    Console.WriteLine($"  Cache HIT: {key} -> {target.Data}");
                    return target;
                }
                else
                {
                    // Object was collected, remove the dead weak reference
                    Console.WriteLine($"  Cache MISS (collected): {key}");
                    _cache.Remove(key);
                    return null;
                }
            }
            
            Console.WriteLine($"  Cache MISS (not found): {key}");
            return null;
        }

        // Clean up dead weak references and return count of live ones
        public int Count
        {
            get
            {
                // Remove dead weak references
                var deadKeys = new List<string>();
                foreach (var kvp in _cache)
                {
                    if (!kvp.Value.IsAlive)
                    {
                        deadKeys.Add(kvp.Key);
                    }
                }

                foreach (var key in deadKeys)
                {
                    _cache.Remove(key);
                }

                return _cache.Count;
            }
        }

        // Remove an item from cache explicitly
        public bool Remove(string key)
        {
            return _cache.Remove(key);
        }

        // Clear all cached items
        public void Clear()
        {
            _cache.Clear();
            Console.WriteLine("  Cache cleared");
        }

        // Get all currently alive cached objects
        public IEnumerable<ExpensiveObject> GetAliveObjects()
        {
            var aliveObjects = new List<ExpensiveObject>();
            
            foreach (var kvp in _cache.ToList()) // ToList to avoid modification during enumeration
            {
                var target = kvp.Value.Target as ExpensiveObject;
                if (target != null)
                {
                    aliveObjects.Add(target);
                }
                else
                {
                    // Clean up dead reference
                    _cache.Remove(kvp.Key);
                }
            }

            return aliveObjects;
        }
    }
}
