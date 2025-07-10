using System;

namespace DisposalPatternDemo
{
    /// <summary>
    /// This class demonstrates clearing sensitive data during disposal.
    /// This is a security best practice - you don't want sensitive data
    /// hanging around in memory longer than necessary.
    /// </summary>
    public class SecureCache : IDisposable
    {
        private byte[]? _sensitiveData;
        private string? _secretKey;
        private bool _disposed = false;

        public SecureCache()
        {
            Console.WriteLine("🔐 SecureCache: Created for storing sensitive data");
        }

        /// <summary>
        /// Stores sensitive data in memory.
        /// </summary>
        public void StoreSecret(string secret)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(SecureCache), 
                    "Cannot store secrets in a disposed cache");
            }

            _secretKey = secret;
            _sensitiveData = System.Text.Encoding.UTF8.GetBytes(secret);
            
            Console.WriteLine($"🔒 SecureCache: Stored sensitive data ({_sensitiveData.Length} bytes)");
        }

        /// <summary>
        /// This is where we demonstrate security-conscious disposal.
        /// We explicitly clear sensitive data from memory.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                Console.WriteLine("🔄 SecureCache: Dispose() called again - safely ignored");
                return;
            }

            // Clear sensitive data from memory
            if (_sensitiveData != null)
            {
                // Zero out the byte array
                Array.Clear(_sensitiveData, 0, _sensitiveData.Length);
                _sensitiveData = null;
                Console.WriteLine("🧹 SecureCache: Sensitive byte array cleared and nullified");
            }

            if (_secretKey != null)
            {
                // We can't "clear" a string (they're immutable), but we can null the reference
                _secretKey = null;
                Console.WriteLine("🧹 SecureCache: Secret key reference cleared");
            }

            _disposed = true;
            Console.WriteLine("✅ SecureCache: All sensitive data cleared from memory");
            
            GC.SuppressFinalize(this);
        }
    }
}
