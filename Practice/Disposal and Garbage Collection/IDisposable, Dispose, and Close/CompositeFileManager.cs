using System;
using System.Collections.Generic;
using System.IO;

namespace DisposalPatternDemo
{
    /// <summary>
    /// Demonstrates disposal of nested disposable objects.
    /// This class manages multiple FileManager instances and shows how to dispose them properly.
    /// The key rule: if you own disposable objects, you must dispose them when you're disposed.
    /// </summary>
    public class CompositeFileManager : IDisposable
    {
        private readonly List<FileManager> _fileManagers;
        private bool _disposed = false;
        private readonly string _identifier;

        public CompositeFileManager(params string[] filePaths)
        {
            if (filePaths == null || filePaths.Length == 0)
                throw new ArgumentException("At least one file path must be provided", nameof(filePaths));

            _identifier = $"Composite-{Guid.NewGuid().ToString()[..8]}";
            _fileManagers = new List<FileManager>();

            Console.WriteLine($"📁 {_identifier}: Creating composite manager for {filePaths.Length} files");

            // Create FileManager instances for each file path
            foreach (string filePath in filePaths)
            {
                try
                {
                    var fileManager = new FileManager(filePath);
                    _fileManagers.Add(fileManager);
                }
                catch (Exception ex)
                {
                    // If we fail to create any FileManager, we need to dispose the ones we already created
                    Console.WriteLine($"❌ Failed to create FileManager for '{filePath}': {ex.Message}");
                    
                    // Clean up any FileManagers we've already created
                    foreach (var existingManager in _fileManagers)
                    {
                        existingManager.Dispose();
                    }
                    _fileManagers.Clear();
                    throw;
                }
            }

            Console.WriteLine($"✅ {_identifier}: Successfully created {_fileManagers.Count} file managers");
        }

        /// <summary>
        /// Processes all files managed by this composite manager
        /// </summary>
        public void ProcessFiles()
        {
            ThrowIfDisposed();

            Console.WriteLine($"⚙ {_identifier}: Processing all files...");

            for (int i = 0; i < _fileManagers.Count; i++)
            {
                try
                {
                    Console.WriteLine($"  📄 Processing file {i + 1}/{_fileManagers.Count}:");
                    _fileManagers[i].ReadContent();
                    
                    long fileSize = _fileManagers[i].GetFileSize();
                    Console.WriteLine($"    📏 File size: {fileSize} bytes");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ❌ Error processing file {i + 1}: {ex.Message}");
                }
            }

            Console.WriteLine($"✅ {_identifier}: Finished processing all files");
        }

        /// <summary>
        /// Gets the total number of files being managed
        /// </summary>
        public int FileCount
        {
            get
            {
                ThrowIfDisposed();
                return _fileManagers.Count;
            }
        }

        /// <summary>
        /// Gets the total size of all managed files
        /// </summary>
        public long TotalFileSize
        {
            get
            {
                ThrowIfDisposed();
                
                long totalSize = 0;
                foreach (var fileManager in _fileManagers)
                {
                    try
                    {
                        if (!fileManager.IsDisposed)
                        {
                            totalSize += fileManager.GetFileSize();
                        }
                    }
                    catch (Exception)
                    {
                        // Skip files that can't be accessed
                    }
                }
                return totalSize;
            }
        }

        /// <summary>
        /// Implementation of IDisposable.Dispose()
        /// This demonstrates the critical pattern of disposing nested disposable objects
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method that handles nested object disposal
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine($"🧹 {_identifier}: Starting disposal of {_fileManagers.Count} nested objects...");
                    
                    // CRITICAL: Dispose all owned disposable objects
                    int disposedCount = 0;
                    foreach (var fileManager in _fileManagers)
                    {
                        try
                        {
                            if (!fileManager.IsDisposed)
                            {
                                fileManager.Dispose();
                                disposedCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"⚠ Error disposing FileManager: {ex.Message}");
                        }
                    }

                    _fileManagers.Clear();
                    Console.WriteLine($"✅ {_identifier}: Disposed {disposedCount} file managers");
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer to catch improper disposal
        /// </summary>
        ~CompositeFileManager()
        {
            Console.WriteLine($"⚠ {_identifier}: Finalizer called - nested objects may not be properly disposed!");
            Dispose(disposing: false);
        }

        /// <summary>
        /// Helper method to check disposal state
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(_identifier,
                    "Cannot perform operations on a disposed CompositeFileManager");
            }
        }

        /// <summary>
        /// Property to check if this manager has been disposed
        /// </summary>
        public bool IsDisposed => _disposed;

        /// <summary>
        /// Gets the identifier for this composite manager
        /// </summary>
        public string Identifier => _identifier;
    }
}
