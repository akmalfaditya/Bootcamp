using System;
using System.IO;
using System.Threading.Tasks;

namespace DisposalPatternDemo
{
    /// <summary>
    /// Demonstrates async disposal patterns introduced in .NET Core 3.0 and later.
    /// This is important for modern applications that work with async resources like
    /// network streams, database connections, or cloud services.
    /// </summary>
    public class AsyncResourceManager : IAsyncDisposable, IDisposable
    {
        private Stream? _networkStream;
        private bool _disposed = false;
        private readonly string _connectionId;

        public AsyncResourceManager(string connectionId)
        {
            _connectionId = connectionId ?? throw new ArgumentNullException(nameof(connectionId));
            
            // Simulate creating a network stream
            _networkStream = new MemoryStream();
            Console.WriteLine($"🌐 AsyncResourceManager '{_connectionId}': Created async resource");
        }

        /// <summary>
        /// Simulates async operations with the resource
        /// </summary>
        public async Task<string> ProcessDataAsync()
        {
            ThrowIfDisposed();

            if (_networkStream == null)
                throw new InvalidOperationException("Network stream not available");

            Console.WriteLine($"⚙ AsyncResourceManager '{_connectionId}': Starting async processing...");
            
            // Simulate async work
            await Task.Delay(500);
            
            // Simulate writing and reading data
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Processed data for {_connectionId}");
            await _networkStream.WriteAsync(data, 0, data.Length);
            
            Console.WriteLine($"✅ AsyncResourceManager '{_connectionId}': Async processing complete");
            return $"Processed {data.Length} bytes";
        }

        /// <summary>
        /// Implementation of IAsyncDisposable for async cleanup
        /// This is the preferred disposal method for async resources
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                Console.WriteLine($"🧹 AsyncResourceManager '{_connectionId}': Starting async disposal...");
                
                if (_networkStream != null)
                {
                    // Perform async cleanup operations
                    await _networkStream.FlushAsync();
                    await _networkStream.DisposeAsync();
                    _networkStream = null;
                    Console.WriteLine($"  🌊 Network stream flushed and disposed asynchronously");
                }

                _disposed = true;
                Console.WriteLine($"✅ AsyncResourceManager '{_connectionId}': Async disposal complete");
            }

            // Suppress finalizer since we've cleaned up properly
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Synchronous disposal implementation for IDisposable compatibility
        /// This provides fallback for code that doesn't use async disposal
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Console.WriteLine($"🧹 AsyncResourceManager '{_connectionId}': Synchronous disposal (fallback)");
                
                // For sync disposal, we have to dispose synchronously
                _networkStream?.Dispose();
                _networkStream = null;
                
                _disposed = true;
                Console.WriteLine($"✅ AsyncResourceManager '{_connectionId}': Sync disposal complete");
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer as safety net
        /// </summary>
        ~AsyncResourceManager()
        {
            Console.WriteLine($"⚠ AsyncResourceManager '{_connectionId}': Finalizer called - async cleanup not performed!");
            Dispose();
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException($"AsyncResourceManager-{_connectionId}",
                    "Cannot perform operations on disposed async resource");
            }
        }

        public bool IsDisposed => _disposed;
        public string ConnectionId => _connectionId;
    }
}
