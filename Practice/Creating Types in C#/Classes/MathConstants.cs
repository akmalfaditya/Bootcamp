using System;

namespace Classes
{
    /// <summary>
    /// Demonstrates constants vs static readonly fields
    /// This class shows when to use const vs static readonly and their differences
    /// Think of const as "compile-time constants" and static readonly as "runtime constants"
    /// </summary>
    public static class MathConstants
    {
        // Constants - evaluated at compile time, values baked into consuming assemblies
        // Use these for truly universal constants that will NEVER change
        public const double PI = 3.14159265358979323846;
        public const double E = 2.71828182845904523536;
        public const double SPEED_OF_LIGHT = 299792458; // meters per second
        public const int AVOGADRO_NUMBER_POWER = 23; // 6.022 × 10^23 (just the power for demo)
        
        // String constants are also allowed
        public const string MATH_LIBRARY_VERSION = "1.0.0";
        public const string AUTHOR = "C# Training Team";

        // Static readonly fields - evaluated at runtime, can be different each program run
        // Use these for values that are "constant" but determined at runtime
        public static readonly DateTime ApplicationStartTime = DateTime.Now;
        public static readonly int RandomSeed = new Random().Next(1000, 9999);
        public static readonly string ComputerName = Environment.MachineName;
        public static readonly string CurrentUser = Environment.UserName;
        
        // Static readonly can be initialized with method calls or complex expressions
        public static readonly string TempDirectory = System.IO.Path.GetTempPath();
        public static readonly double GoldenRatio = (1 + Math.Sqrt(5)) / 2;
        
        // Multiple constants can be declared together if they're the same type
        public const int SECONDS_PER_MINUTE = 60, MINUTES_PER_HOUR = 60, HOURS_PER_DAY = 24;

        /// <summary>
        /// Static constructor - runs once when the class is first accessed
        /// Perfect place to initialize complex static readonly fields
        /// </summary>
        static MathConstants()
        {
            Console.WriteLine($"  🧮 MathConstants static constructor executed at {ApplicationStartTime:HH:mm:ss.fff}");
            Console.WriteLine($"  🎲 Random seed for this session: {RandomSeed}");
        }

        /// <summary>
        /// Demonstrates local constants within a method
        /// Local constants are scoped to the method and compile-time evaluated
        /// </summary>
        public static void DemonstrateLocalConstants()
        {
            // Local constants - only visible within this method
            const int LOCAL_MAX_ITERATIONS = 1000;
            const string CALCULATION_TYPE = "Fibonacci";
            const bool ENABLE_DEBUG = false;

            Console.WriteLine($"  📊 Starting {CALCULATION_TYPE} calculation");
            Console.WriteLine($"  🔢 Max iterations: {LOCAL_MAX_ITERATIONS}");
            Console.WriteLine($"  � Debug mode: {(ENABLE_DEBUG ? "Enabled" : "Disabled")}");

            // Simple Fibonacci calculation using the local constant
            int a = 0, b = 1;
            for (int i = 0; i < LOCAL_MAX_ITERATIONS && b < 1000000; i++)
            {
                int temp = a + b;
                a = b;
                b = temp;
            }
            
            Console.WriteLine($"  ✅ Fibonacci calculation complete. Result: {b}");
        }

        /// <summary>
        /// Show the difference between const and static readonly at runtime
        /// </summary>
        public static void ShowConstVsStaticReadonly()
        {
            Console.WriteLine($"  📐 Const values (compile-time):");
            Console.WriteLine($"      PI: {PI}");
            Console.WriteLine($"      E: {E}");
            Console.WriteLine($"      Speed of Light: {SPEED_OF_LIGHT:N0} m/s");
            Console.WriteLine($"      Library Version: {MATH_LIBRARY_VERSION}");
            
            Console.WriteLine($"  ⏰ Static readonly values (runtime):");
            Console.WriteLine($"      App Start Time: {ApplicationStartTime:yyyy-MM-dd HH:mm:ss.fff}");
            Console.WriteLine($"      Random Seed: {RandomSeed}");
            Console.WriteLine($"      Computer: {ComputerName}");
            Console.WriteLine($"      User: {CurrentUser}");
            Console.WriteLine($"      Temp Directory: {TempDirectory}");
            Console.WriteLine($"      Golden Ratio: {GoldenRatio:F10}");
        }

        /// <summary>
        /// Calculate area using const PI
        /// This shows how constants are used in calculations
        /// </summary>
        public static double CalculateCircleArea(double radius)
        {
            // Using the const PI - this gets replaced with the actual value at compile time
            return PI * radius * radius;
        }

        /// <summary>
        /// Get a timestamp based on the application start time
        /// This shows how static readonly fields can be used in calculations
        /// </summary>
        public static string GetElapsedSinceStart()
        {
            TimeSpan elapsed = DateTime.Now - ApplicationStartTime;
            return $"{elapsed.TotalSeconds:F2} seconds since application start";
        }
    }
}
