using System;
using System.Threading;

namespace DisposalPatternDemo
{
    /// <summary>
    /// Demonstrates advanced disposal scenarios including thread safety and sensitive data clearing.
    /// This class shows patterns you'd use in production systems handling sensitive information.
    /// </summary>
    public class SecureResourceManager : IDisposable
    {
        private readonly object _lockObject = new object();
        private volatile bool _disposed = false;
        private byte[]? _sensitiveData;
        private Timer? _backgroundTimer;
        private readonly string _resourceId;

        public SecureResourceManager(string resourceId)
        {
            _resourceId = resourceId ?? throw new ArgumentNullException(nameof(resourceId));
            
            // Simulate allocating sensitive data (like encryption keys, passwords, etc.)
            _sensitiveData = new byte[32];
            new Random().NextBytes(_sensitiveData);
            
            // Start a background timer to simulate ongoing work
            _backgroundTimer = new Timer(DoBackgroundWork, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
            
            Console.WriteLine($"🔐 SecureResourceManager '{_resourceId}': Created with sensitive data");
        }

        /// <summary>
        /// Simulates background work that might be happening
        /// </summary>
        private void DoBackgroundWork(object? state)
        {
            // Check if we've been disposed before doing work
            if (_disposed)
                return;

            lock (_lockObject)
            {
                if (_disposed || _sensitiveData == null)
                    return;

                Console.WriteLine($"⚙ SecureResourceManager '{_resourceId}': Background work executing...");
            }
        }

        /// <summary>
        /// Performs some operation with the sensitive data
        /// </summary>
        public void ProcessSensitiveData()
        {
            lock (_lockObject)
            {
                ThrowIfDisposed();

                if (_sensitiveData == null)
                {
                    throw new InvalidOperationException("No sensitive data available");
                }

                // Simulate processing sensitive data
                Console.WriteLine($"🔒 SecureResourceManager '{_resourceId}': Processing {_sensitiveData.Length} bytes of sensitive data");
                Thread.Sleep(100); // Simulate work
                Console.WriteLine($"✅ SecureResourceManager '{_resourceId}': Processing complete");
            }
        }

        /// <summary>
        /// Gets the status of this resource manager
        /// </summary>
        public string GetStatus()
        {
            lock (_lockObject)
            {
                ThrowIfDisposed();
                return $"Active with {_sensitiveData?.Length ?? 0} bytes of data";
            }
        }

        /// <summary>
        /// Thread-safe disposal implementation
        /// </summary>
        public void Dispose()
        {
            // Use lock to ensure thread-safe disposal
            lock (_lockObject)
            {
                if (!_disposed)
                {
                    Console.WriteLine($"🧹 SecureResourceManager '{_resourceId}': Starting secure disposal...");
                    
                    // Stop the background timer first
                    _backgroundTimer?.Dispose();
                    _backgroundTimer = null;
                    Console.WriteLine($"  ⏹ Background timer stopped");

                    // CRITICAL: Clear sensitive data by overwriting with zeros
                    if (_sensitiveData != null)
                    {
                        Array.Clear(_sensitiveData, 0, _sensitiveData.Length);
                        _sensitiveData = null;
                        Console.WriteLine($"  🛡 Sensitive data securely cleared");
                    }

                    _disposed = true;
                    Console.WriteLine($"✅ SecureResourceManager '{_resourceId}': Disposal complete");
                }
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer as a safety net
        /// </summary>
        ~SecureResourceManager()
        {
            Console.WriteLine($"⚠ SecureResourceManager '{_resourceId}': Finalizer called - sensitive data may have leaked!");
            
            // In finalizer, we can't use locks safely, so we do minimal cleanup
            _backgroundTimer?.Dispose();
            
            // Try to clear sensitive data if it still exists
            if (_sensitiveData != null)
            {
                Array.Clear(_sensitiveData, 0, _sensitiveData.Length);
                Console.WriteLine($"  🛡 Attempted to clear sensitive data in finalizer");
            }
        }

        /// <summary>
        /// Helper method to check disposal state
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException($"SecureResourceManager-{_resourceId}",
                    "Cannot perform operations on a disposed secure resource manager");
            }
        }

        /// <summary>
        /// Property to check disposal state (thread-safe)
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                lock (_lockObject)
                {
                    return _disposed;
                }
            }
        }

        /// <summary>
        /// Gets the resource identifier
        /// </summary>
        public string ResourceId => _resourceId;
    }

    /// <summary>
    /// Demonstrates a disposable object that doesn't actually need disposal.
    /// Some classes implement IDisposable only because their base class does,
    /// but they don't have any resources to clean up themselves.
    /// </summary>
    public class LightweightWrapper : IDisposable
    {
        private bool _disposed = false;
        private readonly string _data;

        public LightweightWrapper(string data)
        {
            _data = data ?? string.Empty;
            Console.WriteLine($"📝 LightweightWrapper: Created with data '{_data}'");
        }

        /// <summary>
        /// Gets the wrapped data
        /// </summary>
        public string GetData()
        {
            ThrowIfDisposed();
            return _data;
        }

        /// <summary>
        /// Disposal implementation - nothing much to clean up here
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Console.WriteLine($"🧹 LightweightWrapper: Disposed (no resources to clean up)");
                _disposed = true;
            }
            
            // No finalizer needed since we don't have unmanaged resources
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(LightweightWrapper),
                    "Cannot use disposed LightweightWrapper");
            }
        }

        public bool IsDisposed => _disposed;
    }
}
