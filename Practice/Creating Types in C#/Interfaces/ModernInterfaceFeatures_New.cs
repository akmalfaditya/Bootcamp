using System;

namespace Interfaces
{
    /// <summary>
    /// Modern Interface Features (C# 8+ and C# 11+)
    /// These features revolutionized interfaces and made them much more powerful
    /// </summary>

    #region Default Interface Members (C# 8+)

    /// <summary>
    /// Default Interface Members - Game changer for API evolution!
    /// You can now provide default implementations in interfaces
    /// This lets you add new methods without breaking existing implementations
    /// </summary>
    interface ILogger
    {
        // Default implementation - classes don't HAVE to implement this
        void Log(string text) => Console.WriteLine(text);
        
        // Abstract method - classes MUST implement this  
        void LogError(string error);
    }

    /// <summary>
    /// This logger only implements the required method
    /// It will use the default implementation for Log()
    /// </summary>
    class SimpleLogger : ILogger
    {
        public void LogError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {error}");
            Console.ResetColor();
        }
        
        // Note: We don't implement Log() - we'll use the default
    }

    /// <summary>
    /// Advanced logger that implements both interfaces
    /// This allows us to demonstrate static interface members
    /// </summary>
    class AdvancedLogger : ILogger, ILoggerWithStatics
    {
        public void LogError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ADVANCED ERROR: {error}");
            Console.ResetColor();
        }
        
        // Note: Log() method comes from both interfaces with same signature
    }

    /// <summary>
    /// This logger provides its own implementation for both methods
    /// </summary>
    class CustomLogger : ILogger
    {
        public void Log(string text)
        {
            Console.WriteLine($"[CUSTOM] {text}");
        }

        public void LogError(string error)
        {
            Console.WriteLine($"[CUSTOM ERROR] {error}");
        }
    }

    #endregion

    #region Static Interface Members

    /// <summary>
    /// Static Non-Virtual Interface Members (C# 8+)
    /// Interfaces can now have static fields and methods
    /// These support default interface members
    /// </summary>
    interface ILoggerWithStatics
    {
        // Static field in interface!
        static string Prefix = "";

        // Default method using static field
        void Log(string text) => Console.WriteLine(Prefix + text);
    }

    /// <summary>
    /// Static Virtual/Abstract Interface Members (C# 11+)
    /// This enables static polymorphism - very advanced feature!
    /// Used for things like Generic Math in .NET
    /// </summary>
    interface ITypeDescribable
    {
        // Static abstract - MUST be implemented by the type
        static abstract string Description { get; }
        
        // Static virtual - CAN be overridden, has default
        static virtual string Category => "General";
    }

    /// <summary>
    /// Type implementing static abstract interface
    /// </summary>
    class CustomerTest : ITypeDescribable
    {
        // Required static implementation
        public static string Description => "Customer tests";
        
        // Optional static override
        public static string Category => "Unit Testing";
    }

    class ProductTest : ITypeDescribable
    {
        // Required static implementation
        public static string Description => "Product tests";
        
        // Using default Category ("General") - explicitly implementing it
        public static string Category => "General";
    }

    #endregion

    /// <summary>
    /// Demo class for modern interface features
    /// </summary>
    public static class ModernFeaturesDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Modern Interface Features Demo ===\n");

            // Default interface members demo
            Console.WriteLine("--- Default Interface Members ---");
            
            var simpleLogger = new SimpleLogger();
            var customLogger = new CustomLogger();
            var advancedLogger = new AdvancedLogger();

            // For default methods, you must cast to interface
            ((ILogger)simpleLogger).Log("Using default implementation");
            simpleLogger.LogError("Custom error implementation");

            customLogger.Log("Custom implementation");
            customLogger.LogError("Custom error implementation");

            // Static interface members demo
            Console.WriteLine("\n--- Static Interface Members ---");
            
            ILoggerWithStatics.Prefix = "[STATIC] ";
            ((ILoggerWithStatics)advancedLogger).Log("Message with static prefix");

            // Static abstract/virtual members
            Console.WriteLine("\n--- Static Abstract/Virtual Members ---");
            Console.WriteLine($"CustomerTest: {CustomerTest.Description} - {CustomerTest.Category}");
            Console.WriteLine($"ProductTest: {ProductTest.Description} - {ProductTest.Category}");

            // Generic method using static abstract constraint
            PrintTypeInfo<CustomerTest>();
            PrintTypeInfo<ProductTest>();
        }

        // This method can only be called with types that implement ITypeDescribable
        // It can access the static members of the type parameter!
        static void PrintTypeInfo<T>() where T : ITypeDescribable
        {
            Console.WriteLine($"Generic method - {typeof(T).Name}: {T.Description}");
        }
    }
}
