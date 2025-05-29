using System;

namespace Classes
{
    /// <summary>
    /// Demonstrates constants - values that are set at compile time and never change
    /// Constants are like "universal truths" in your code
    /// </summary>
    public class MathConstants
    {
        // Constants must be assigned at declaration
        // They're evaluated at compile time, not runtime
        public const double PI = 3.14159265359;
        public const double E = 2.71828182846;
        public const long SPEED_OF_LIGHT = 299792458; // meters per second
        
        // You can also have string constants
        public const string COMPANY_NAME = "Tech Bootcamp Inc.";
        
        // Constants are implicitly static - you access them via the class name
        // like MathConstants.PI, not through an instance

        /// <summary>
        /// Method that uses constants in calculations
        /// Shows how constants are typically used in practice
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <returns>Circle area</returns>
        public static double CalculateCircleArea(double radius)
        {
            // Using our constant here
            return PI * radius * radius;
        }

        /// <summary>
        /// Another example using constants
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <returns>Circle circumference</returns>
        public static double CalculateCircleCircumference(double radius)
        {
            return 2 * PI * radius;
        }
    }
}
