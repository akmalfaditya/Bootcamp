using System;

namespace DisposalPatternDemo
{
    /// <summary>
    /// This class perfectly demonstrates the difference between Close() and Dispose().
    /// It simulates a database connection, similar to how SqlConnection works.
    /// 
    /// Key lesson: Close() is temporary, Dispose() is permanent.
    /// </summary>
    public class DatabaseConnection : IDisposable
    {
        private string? _connectionString;
        private bool _isOpen = false;
        private bool _disposed = false;

        // This property lets us check the connection state from outside
        public string State => _disposed ? "Disposed" : (_isOpen ? "Open" : "Closed");

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            Console.WriteLine($"🔗 DatabaseConnection: Created with connection string");
        }

        /// <summary>
        /// Opens the database connection.
        /// This can be called multiple times if the connection is closed but not disposed.
        /// </summary>
        public void Open()
        {
            // Always check if disposed first
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DatabaseConnection), 
                    "Cannot open a disposed connection. Once disposed, it's gone forever!");
            }

            if (_isOpen)
            {
                Console.WriteLine("ℹ Connection is already open");
                return;
            }

            // Simulate opening a connection
            _isOpen = true;
            Console.WriteLine("✅ Database connection opened");
        }

        /// <summary>
        /// This is the key method that demonstrates the difference from Dispose().
        /// Close() means "pause" - you can reopen the connection later.
        /// Think of it like putting your phone to sleep vs. turning it off completely.
        /// </summary>
        public void Close()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DatabaseConnection), 
                    "Cannot close a disposed connection.");
            }

            if (!_isOpen)
            {
                Console.WriteLine("ℹ Connection is already closed");
                return;
            }

            // Simulate closing the connection
            _isOpen = false;
            Console.WriteLine("⏸ Database connection closed (but can be reopened)");
        }

        /// <summary>
        /// This is where the permanent shutdown happens.
        /// Unlike Close(), once you call Dispose(), the connection is permanently unusable.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                Console.WriteLine("🔄 Dispose() called again - safely ignored");
                return;
            }

            // If the connection is still open, close it first
            if (_isOpen)
            {
                _isOpen = false;
                Console.WriteLine("⏸ Closing open connection during disposal");
            }

            // Clear the connection string - this is permanent
            _connectionString = null;
            
            // Mark as disposed
            _disposed = true;
            
            Console.WriteLine("🧹 DatabaseConnection: Permanently disposed - cannot be reopened");
            
            GC.SuppressFinalize(this);
        }
    }
}
