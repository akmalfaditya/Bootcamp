using System;

namespace Interfaces
{
    /// <summary>
    /// Logger interface with default implementation (C# 8+)
    /// Default methods let you add new functionality without breaking existing implementations
    /// Think of it as "here's a sensible default, but feel free to customize"
    /// </summary>
    public interface ILogger
    {
        // Abstract method - must be implemented
        void Log(string message);
        
        // Default implementation - optional to override
        void LogError(string error) 
        {
            Log($"ERROR: {error}");
        }
        
        // Another default method with more sophisticated logic
        void LogWithTimestamp(string message)
        {
            Log($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }
        
        // Default property
        string LoggerName => "DefaultLogger";
    }

    /// <summary>
    /// File logger that uses default implementations
    /// Shows how you can implement just the required methods
    /// </summary>
    public class FileLogger : ILogger
    {
        private string _fileName;
        
        public FileLogger(string fileName = "app.log")
        {
            _fileName = fileName;
        }
        
        // Only implementing the required abstract method
        public void Log(string message)
        {
            Console.WriteLine($"FileLogger: Writing to {_fileName}: {message}");
            // In real implementation, would write to actual file
        }
        
        // Inherits default implementations of LogError and LogWithTimestamp
        // Can still override if needed
    }

    /// <summary>
    /// Database logger that customizes some default behavior
    /// Shows how you can selectively override default implementations
    /// </summary>
    public class DatabaseLogger : ILogger
    {
        private string _connectionString;
        
        public DatabaseLogger(string connectionString = "Server=localhost;Database=Logs")
        {
            _connectionString = connectionString;
        }
        
        // Required implementation
        public void Log(string message)
        {
            Console.WriteLine($"DatabaseLogger: Inserting to database: {message}");
        }
        
        // Custom implementation of default method
        public void LogError(string error)
        {
            Console.WriteLine($"DatabaseLogger: CRITICAL ERROR - alerting admin: {error}");
            Log($"ADMIN_ALERT: {error}");
        }
        
        // Using default implementation of LogWithTimestamp
        // Overriding default property
        public string LoggerName => "ProductionDatabaseLogger";
    }

    /// <summary>
    /// Type description interface with static members (C# 11+)
    /// Static interface members enable type-level contracts
    /// Perfect for factory patterns and generic constraints
    /// </summary>
    public interface ITypeDescribable
    {
        // Static abstract - must be implemented by the type
        static abstract string Description { get; }
        
        // Static virtual - can be overridden but has default
        static virtual string Category => "General";
        
        // Static method with default implementation
        static virtual int DefaultPriority => 1;
        
        // Regular instance method for comparison
        string GetInstanceInfo();
    }

    /// <summary>
    /// Product class implementing static interface
    /// Shows how types can have static contracts
    /// </summary>
    public class Product : ITypeDescribable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        // Required static abstract implementation
        public static string Description => "Physical or digital item for sale";
        
        // Override static virtual property
        public static string Category => "Merchandise";
        
        // Override static virtual method
        public static int DefaultPriority => 5;
        
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        
        // Instance method implementation
        public string GetInstanceInfo()
        {
            return $"Product: {Name} - ${Price:F2}";
        }
    }

    /// <summary>
    /// Service class with different static implementations
    /// Demonstrates how different types can fulfill static contracts differently
    /// </summary>
    public class Service : ITypeDescribable
    {
        public string ServiceName { get; set; }
        public TimeSpan Duration { get; set; }
        
        // Different static implementation
        public static string Description => "Professional service offering";
        
        // Different category
        public static string Category => "Professional Services";
        
        // Using default priority (inherited from interface)
        // public static int DefaultPriority => 1; // This is inherited
        
        public Service(string serviceName, TimeSpan duration)
        {
            ServiceName = serviceName;
            Duration = duration;
        }
        
        public string GetInstanceInfo()
        {
            return $"Service: {ServiceName} - {Duration.TotalHours:F1} hours";
        }
    }
}
