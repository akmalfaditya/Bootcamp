using System;

namespace DisposalPatternDemo
{
    /// <summary>
    /// Simulates a database connection to demonstrate the difference between Close() and Dispose().
    /// This is similar to how SqlConnection works in real applications.
    /// </summary>
    public class DatabaseConnection : IDisposable
    {
        private string? _connectionString;
        private bool _isOpen = false;
        private bool _disposed = false;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            Console.WriteLine($"🔗 DatabaseConnection: Created with connection string (length: {connectionString.Length})");
        }

        /// <summary>
        /// Opens the database connection.
        /// Can be called multiple times if the connection is closed but not disposed.
        /// </summary>
        public void Open()
        {
            ThrowIfDisposed();

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
        /// Closes the connection but keeps it available for reopening.
        /// This is different from Dispose() - the connection can be reopened after Close().
        /// </summary>
        public void Close()
        {
            ThrowIfDisposed();

            if (!_isOpen)
            {
                Console.WriteLine("ℹ Connection is already closed");
                return;
            }

            _isOpen = false;
            Console.WriteLine("🔒 Database connection closed (can be reopened)");
        }

        /// <summary>
        /// Executes a query on the database.
        /// Requires the connection to be open.
        /// </summary>
        public void ExecuteQuery(string sql)
        {
            ThrowIfDisposed();

            if (!_isOpen)
            {
                throw new InvalidOperationException("Connection must be open to execute queries");
            }

            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("SQL query cannot be null or empty", nameof(sql));
            }

            // Simulate query execution
            Console.WriteLine($"📊 Executing query: {sql}");
            System.Threading.Thread.Sleep(100); // Simulate some work
            Console.WriteLine("✅ Query executed successfully");
        }

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        public string GetConnectionState()
        {
            ThrowIfDisposed();
            return _isOpen ? "Open" : "Closed";
        }

        /// <summary>
        /// Implementation of IDisposable.Dispose().
        /// This permanently releases all resources and makes the connection unusable.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method following the standard disposal pattern.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Close the connection if it's still open
                    if (_isOpen)
                    {
                        _isOpen = false;
                        Console.WriteLine("🔒 Connection closed during disposal");
                    }

                    // Clear the connection string
                    _connectionString = null;
                    Console.WriteLine("🧹 DatabaseConnection: All resources disposed");
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer - called by garbage collector if Dispose() wasn't called
        /// </summary>
        ~DatabaseConnection()
        {
            Console.WriteLine("⚠ DatabaseConnection finalizer called - dispose was not called explicitly!");
            Dispose(disposing: false);
        }

        /// <summary>
        /// Helper method to ensure operations aren't performed on disposed objects
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DatabaseConnection),
                    "Cannot perform operations on a disposed database connection");
            }
        }

        /// <summary>
        /// Property to check if the connection has been disposed
        /// </summary>
        public bool IsDisposed => _disposed;

        /// <summary>
        /// Property to check if the connection is currently open
        /// </summary>
        public bool IsOpen => !_disposed && _isOpen;
    }
}
