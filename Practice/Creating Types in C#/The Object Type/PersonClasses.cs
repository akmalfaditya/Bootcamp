namespace TheObjectType
{
    /// <summary>
    /// Example class that does NOT override ToString
    /// Shows what happens when you don't provide custom string representation
    /// </summary>
    public class PandaWithoutToString
    {
        public string Name { get; set; } = string.Empty;
        
        // Notice: NO ToString() override
        // Will use default object.ToString() which just returns type name
    }

    /// <summary>
    /// Example class that DOES override ToString
    /// Shows proper implementation of string representation
    /// </summary>
    public class PandaWithToString
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        /// <summary>
        /// Proper ToString override provides meaningful information
        /// Makes debugging and logging much easier
        /// </summary>
        public override string ToString()
        {
            return $"Panda named {Name}, {Age} years old";
        }
    }

    /// <summary>
    /// Employee class demonstrating formatted ToString output
    /// Shows how to create professional string representations
    /// </summary>
    public class Employee
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

        public Employee(string name, decimal salary, DateTime hireDate)
        {
            Name = name;
            Salary = salary;
            HireDate = hireDate;
        }

        /// <summary>
        /// Professional ToString with formatted output
        /// Includes multiple properties in readable format
        /// </summary>
        public override string ToString()
        {
            return $"Employee: {Name}\n" +
                   $"  Salary: {Salary:C}\n" +
                   $"  Hire Date: {HireDate:yyyy-MM-dd}\n" +
                   $"  Years of Service: {(DateTime.Now - HireDate).Days / 365.0:F1}";
        }
    }

    /// <summary>
    /// Configuration manager that stores different types as objects
    /// Demonstrates practical use of object type for flexible storage
    /// </summary>
    public class ConfigurationManager
    {
        private readonly Dictionary<string, object> _settings = new();

        /// <summary>
        /// Store any type of value in configuration
        /// Object type allows maximum flexibility
        /// </summary>
        public void SetValue(string key, object value)
        {
            _settings[key] = value;
        }

        /// <summary>
        /// Retrieve strongly-typed value from configuration
        /// Uses casting to convert from object back to specific type
        /// </summary>
        public T GetValue<T>(string key)
        {
            if (_settings.TryGetValue(key, out object? value))
            {
                return (T)value;
            }
            return default(T)!;
        }

        /// <summary>
        /// Display all configuration settings
        /// Demonstrates ToString() being called on different types
        /// </summary>
        public void DisplayAllSettings()
        {
            foreach (var setting in _settings)
            {
                Console.WriteLine($"{setting.Key}: {setting.Value} (Type: {setting.Value.GetType().Name})");
            }
        }
    }

    /// <summary>
    /// Simple logging system that accepts any object
    /// Shows practical application of object type flexibility
    /// </summary>
    public class SimpleLogger
    {
        private readonly List<LogEntry> _logs = new();

        /// <summary>
        /// Log any object - ToString() will be called automatically
        /// Object parameter allows logging anything
        /// </summary>
        public void Log(object message)
        {
            _logs.Add(new LogEntry(DateTime.Now, message));
        }

        /// <summary>
        /// Display all log entries
        /// Shows how object type enables flexible logging
        /// </summary>
        public void DisplayLogs()
        {
            foreach (var entry in _logs)
            {
                Console.WriteLine($"[{entry.Timestamp:HH:mm:ss}] {entry.Message} (Type: {entry.Message.GetType().Name})");
            }
        }

        /// <summary>
        /// Internal log entry structure
        /// Stores timestamp and message as object
        /// </summary>
        private class LogEntry
        {
            public DateTime Timestamp { get; }
            public object Message { get; }

            public LogEntry(DateTime timestamp, object message)
            {
                Timestamp = timestamp;
                Message = message;
            }
        }
    }
}
