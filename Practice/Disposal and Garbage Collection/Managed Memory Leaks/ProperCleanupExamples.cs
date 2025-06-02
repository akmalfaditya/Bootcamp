// Proper Cleanup Pattern Examples
// These classes demonstrate best practices for resource management and memory leak prevention

namespace ManagedMemoryLeaks
{
    // Example of a properly disposable resource that manages multiple cleanup concerns
    public class ProperlyDisposableResource : IDisposable
    {
        private readonly string _name;
        private readonly System.Timers.Timer _timer;
        private readonly FileStream? _fileStream;
        private readonly List<EventHandler> _eventHandlers;
        private bool _disposed = false;
        
        public ProperlyDisposableResource(string name)
        {
            _name = name;
            _eventHandlers = new List<EventHandler>();
            
            // Initialize timer
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            
            // Initialize file stream (if possible)
            try
            {
                var tempFile = Path.GetTempFileName();
                _fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create file stream: {ex.Message}");
                _fileStream = null;
            }
            
            Console.WriteLine($"Created properly disposable resource: {_name}");
        }
        
        public void DoWork()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ProperlyDisposableResource));
            
            Console.WriteLine($"{_name} is doing work...");
            
            // Start timer if not already running
            if (!_timer.Enabled)
            {
                _timer.Start();
            }
            
            // Write to file if available
            if (_fileStream != null)
            {
                var data = System.Text.Encoding.UTF8.GetBytes($"Work done by {_name}\n");
                _fileStream.Write(data, 0, data.Length);
                _fileStream.Flush();
            }
        }
        
        public void SubscribeToEvent(EventHandler handler)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ProperlyDisposableResource));
            
            _eventHandlers.Add(handler);
            // In a real scenario, you'd subscribe to some external event here
        }
        
        private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_disposed)
            {
                Console.WriteLine($"  {_name} timer tick");
            }
        }
        
        // Proper implementation of IDisposable pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Tell GC not to call finalizer
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    Console.WriteLine($"Disposing {_name}...");
                    
                    // Stop and dispose timer
                    _timer.Stop();
                    _timer.Elapsed -= OnTimerElapsed;
                    _timer.Dispose();
                    
                    // Dispose file stream
                    _fileStream?.Dispose();
                    
                    // Clear event handlers
                    _eventHandlers.Clear();
                }
                
                // Free unmanaged resources (if any)
                // In this example, we don't have unmanaged resources
                
                _disposed = true;
                Console.WriteLine($"  {_name} disposed successfully");
            }
        }
        
        // Finalizer - only needed if you have unmanaged resources
        ~ProperlyDisposableResource()
        {
            Console.WriteLine($"Finalizer called for {_name} - this shouldn't happen if Dispose was called!");
            Dispose(false);
        }
    }
    
    // Example showing common disposal patterns
    public static class DisposalPatternExamples
    {
        public static void DemonstrateUsingStatement()
        {
            Console.WriteLine("Demonstrating 'using' statement pattern:");
            
            // Using statement automatically calls Dispose when block exits
            using (var resource = new ProperlyDisposableResource("Using Statement Resource"))
            {
                resource.DoWork();
                // Dispose() is automatically called here, even if exception occurs
            }
        }
        
        public static void DemonstrateTryFinallyPattern()
        {
            Console.WriteLine("\nDemonstrating try-finally pattern:");
            
            ProperlyDisposableResource? resource = null;
            try
            {
                resource = new ProperlyDisposableResource("Try-Finally Resource");
                resource.DoWork();
            }
            finally
            {
                // Ensure disposal even if exception occurs
                resource?.Dispose();
            }
        }
        
        public static void DemonstrateUsingDeclaration()
        {
            Console.WriteLine("\nDemonstrating using declaration pattern (C# 8+):");
            
            // Using declaration - resource is disposed at end of scope
            using var resource = new ProperlyDisposableResource("Using Declaration Resource");
            resource.DoWork();
            
            // More code can go here...
            Console.WriteLine("Resource will be disposed at end of method");
            
            // Dispose() is automatically called at end of scope
        }
    }
    
    // Example of resource pooling to reduce allocation pressure
    public class ResourcePool<T> : IDisposable where T : class, IDisposable, new()
    {
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly object _lock = new object();
        private bool _disposed = false;
        
        public T Rent()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ResourcePool<T>));
            
            lock (_lock)
            {
                if (_pool.Count > 0)
                {
                    return _pool.Dequeue();
                }
            }
            
            // Create new instance if pool is empty
            return new T();
        }
        
        public void Return(T item)
        {
            if (_disposed || item == null)
                return;
            
            lock (_lock)
            {
                _pool.Enqueue(item);
            }
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                lock (_lock)
                {
                    // Dispose all pooled items
                    while (_pool.Count > 0)
                    {
                        var item = _pool.Dequeue();
                        item.Dispose();
                    }
                }
                
                _disposed = true;
            }
        }
    }
}
