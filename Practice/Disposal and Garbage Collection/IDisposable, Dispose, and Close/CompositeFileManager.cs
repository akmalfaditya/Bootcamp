using System;
using System.Collections.Generic;
using System.IO;

namespace DisposalPatternDemo
{
    /// <summary>
    /// This class demonstrates GOLDEN RULE 3: Chained Disposal.
    /// When an object "owns" other disposable objects, it should dispose them too.
    /// 
    /// Think of this as a parent that's responsible for cleaning up after its children.
    /// </summary>
    public class CompositeFileManager : IDisposable
    {
        private List<FileManager> _fileManagers;
        private bool _disposed = false;

        public CompositeFileManager(params string[] filePaths)
        {
            _fileManagers = new List<FileManager>();
            
            // Create and "own" multiple FileManager instances
            foreach (var filePath in filePaths)
            {
                // Create the file if it doesn't exist
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, $"Log file created at {DateTime.Now}");
                }
                
                var fileManager = new FileManager(filePath);
                _fileManagers.Add(fileManager);
            }
            
            Console.WriteLine($"📁 CompositeFileManager: Created and owns {_fileManagers.Count} FileManager instances");
        }

        /// <summary>
        /// This method writes to all the log files we manage.
        /// </summary>
        public void WriteToLogs(string message)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(CompositeFileManager), 
                    "Cannot write to logs - CompositeFileManager has been disposed");
            }

            foreach (var fileManager in _fileManagers)
            {
                // We'll just read the content to show activity
                fileManager.ReadContent();
            }
            
            Console.WriteLine($"📝 Wrote message to {_fileManagers.Count} log files: {message}");
        }

        /// <summary>
        /// This is where GOLDEN RULE 3 comes into play.
        /// Since we "own" the FileManager instances, WE are responsible for disposing them.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                Console.WriteLine("🔄 CompositeFileManager: Dispose() called again - safely ignored");
                return;
            }

            Console.WriteLine("🧹 CompositeFileManager: Starting chained disposal...");
            
            // Dispose all the FileManager instances we own
            foreach (var fileManager in _fileManagers)
            {
                fileManager.Dispose(); // This demonstrates chained disposal
            }
            
            // Clear our list
            _fileManagers.Clear();
            
            // Mark as disposed
            _disposed = true;
            
            Console.WriteLine("✅ CompositeFileManager: Chained disposal complete - all owned objects disposed");
            
            GC.SuppressFinalize(this);
        }
    }
}
