using System;
using System.IO;

namespace DisposalPatternDemo
{
    /// <summary>
    /// Demonstrates a proper implementation of IDisposable pattern.
    /// This class manages a FileStream resource and shows how to clean it up properly.
    /// </summary>
    public class FileManager : IDisposable
    {
        private FileStream? _fileStream;
        private bool _disposed = false; // Flag to track disposal state

        public FileManager(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            try
            {
                _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                Console.WriteLine($"📂 FileManager: Opened file '{Path.GetFileName(filePath)}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to open file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Reads and displays the content of the managed file.
        /// This method demonstrates checking disposal state before operations.
        /// </summary>
        public void ReadContent()
        {
            // Always check if object has been disposed before performing operations
            ThrowIfDisposed();

            if (_fileStream == null)
            {
                Console.WriteLine("⚠ No file stream available to read from");
                return;
            }

            try
            {
                _fileStream.Position = 0; // Reset to beginning
                using var reader = new StreamReader(_fileStream, leaveOpen: true);
                string content = reader.ReadToEnd();
                Console.WriteLine($"📖 Content: {content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error reading file: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the length of the file in bytes.
        /// Another example of checking disposal state.
        /// </summary>
        public long GetFileSize()
        {
            ThrowIfDisposed();
            return _fileStream?.Length ?? 0;
        }

        /// <summary>
        /// Implementation of IDisposable.Dispose().
        /// This follows the standard disposal pattern.
        /// </summary>
        public void Dispose()
        {
            // Dispose of managed and unmanaged resources
            Dispose(disposing: true);
            
            // Tell the garbage collector that finalization is not needed
            // since we've already cleaned up everything
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method that does the actual cleanup.
        /// This pattern allows derived classes to override disposal behavior.
        /// </summary>
        /// <param name="disposing">True if disposing managed resources, false if called from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _fileStream?.Dispose();
                    Console.WriteLine("🧹 FileManager: FileStream disposed properly");
                }

                // If we had unmanaged resources, we'd clean them up here
                // (both when disposing=true AND disposing=false)

                _fileStream = null;
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer (destructor) - only needed if we have unmanaged resources.
        /// This is a safety net in case someone forgets to call Dispose().
        /// </summary>
        ~FileManager()
        {
            Console.WriteLine("⚠ FileManager finalizer called - someone forgot to dispose!");
            Dispose(disposing: false);
        }

        /// <summary>
        /// Helper method to check if the object has been disposed.
        /// Throws ObjectDisposedException if it has been disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(FileManager), 
                    "Cannot perform operations on a disposed FileManager");
            }
        }

        /// <summary>
        /// Property to check if the object has been disposed.
        /// Useful for defensive programming.
        /// </summary>
        public bool IsDisposed => _disposed;
    }
}
