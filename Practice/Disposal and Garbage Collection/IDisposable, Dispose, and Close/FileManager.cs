using System;
using System.IO;

namespace DisposalPatternDemo
{
    /// <summary>
    /// This is our basic example of implementing IDisposable correctly.
    /// Think of this as the "Hello World" of resource management.
    /// 
    /// This class wraps a FileStream (which holds an unmanaged file handle)
    /// and demonstrates the fundamental principles of disposal.
    /// </summary>
    public sealed class FileManager : IDisposable
    {
        private FileStream? _fileStream;
        private readonly string _fileName;
        private bool _disposed = false; // This flag tracks our disposal state

        public FileManager(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            _fileName = Path.GetFileName(filePath);
            
            // Here's where we acquire the unmanaged resource (file handle)
            // This is what makes us "disposable" - we're holding onto something
            // that the OS needs to get back
            _fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Console.WriteLine($"📂 FileManager: Opened file '{_fileName}'");
        }

        /// <summary>
        /// This method demonstrates the "irreversible disposal" rule.
        /// Once an object is disposed, it should throw ObjectDisposedException
        /// if someone tries to use it.
        /// </summary>
        public void ReadContent()
        {
            // GOLDEN RULE 1: Always check if disposed before doing work
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(FileManager), 
                    "Cannot read from a disposed FileManager. This object is 'dead' - no resurrection!");
            }

            if (_fileStream == null)
            {
                Console.WriteLine("⚠ No file stream available to read from");
                return;
            }

            try
            {
                _fileStream.Position = 0; // Reset to beginning
                if (_fileStream.Length > 0)
                {
                    int firstByte = _fileStream.ReadByte();
                    Console.WriteLine($"📖 Read first byte from '{_fileName}': {firstByte}");
                }
                else
                {
                    Console.WriteLine($"📖 File '{_fileName}' is empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error reading file: {ex.Message}");
            }
        }

        /// <summary>
        /// This is the heart of IDisposable - the Dispose method.
        /// This is where we release our unmanaged resources.
        /// 
        /// Notice how we implement GOLDEN RULE 2: Idempotent disposal.
        /// You can call this method multiple times safely.
        /// </summary>
        public void Dispose()
        {
            // GOLDEN RULE 2: Idempotent disposal
            // If we're already disposed, just return quietly
            if (_disposed)
            {
                // In a real application, you might not even log this
                // But for training purposes, let's show it's being called again
                Console.WriteLine($"🔄 Dispose() called again on '{_fileName}' - safely ignored");
                return;
            }

            // Here's where the actual cleanup happens
            if (_fileStream != null)
            {
                _fileStream.Close(); // This releases the file handle
                _fileStream = null;  // Clear the reference
                Console.WriteLine($"🧹 FileManager: Released file handle for '{_fileName}'");
            }

            // Mark as disposed - this is crucial for the irreversible disposal rule
            _disposed = true;
            
            // Tell the GC it doesn't need to run our finalizer (if we had one)
            GC.SuppressFinalize(this);
        }
    }
}
