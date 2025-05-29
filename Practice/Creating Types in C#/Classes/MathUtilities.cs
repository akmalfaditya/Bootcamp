using System;

namespace Classes
{
    /// <summary>
    /// Static class demonstration - cannot be instantiated, all members must be static
    /// Use static classes for utility functions that don't need object state
    /// Think of Math class in .NET - you don't create a Math object, you just call Math.Sqrt()
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Simple addition using expression-bodied syntax
        /// Notice how clean static utility methods can be
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Sum of a and b</returns>
        public static double Add(double a, double b) => a + b;

        /// <summary>
        /// Subtraction method
        /// </summary>
        public static double Subtract(double a, double b) => a - b;

        /// <summary>
        /// Multiplication method
        /// </summary>
        public static double Multiply(double a, double b) => a * b;

        /// <summary>
        /// Division with error checking - shows why static classes are great for utilities
        /// </summary>
        /// <param name="a">Dividend</param>
        /// <param name="b">Divisor</param>
        /// <returns>Result of division</returns>
        public static double Divide(double a, double b)
        {
            if (Math.Abs(b) < double.Epsilon) // Better than b == 0 for doubles
                throw new DivideByZeroException("Cannot divide by zero!");
            
            return a / b;
        }

        /// <summary>
        /// Calculate square root - wraps Math.Sqrt with additional validation
        /// </summary>
        /// <param name="number">Number to find square root of</param>
        /// <returns>Square root</returns>
        public static double SquareRoot(double number)
        {
            if (number < 0)
                throw new ArgumentException("Cannot calculate square root of negative number!");
            
            return Math.Sqrt(number);
        }

        /// <summary>
        /// Calculate percentage
        /// Great example of a utility function that doesn't need object state
        /// </summary>
        /// <param name="part">The part value</param>
        /// <param name="whole">The whole value</param>
        /// <returns>Percentage</returns>
        public static double CalculatePercentage(double part, double whole)
        {
            if (whole == 0)
                return 0;
            
            return (part / whole) * 100;
        }

        /// <summary>
        /// Check if a number is prime
        /// Another good utility function example
        /// </summary>
        /// <param name="number">Number to check</param>
        /// <returns>True if prime, false otherwise</returns>
        public static bool IsPrime(int number)
        {
            if (number < 2) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            // Check odd divisors up to square root
            for (int i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0)
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// Convert temperature from Celsius to Fahrenheit
        /// Static methods are perfect for conversion utilities
        /// </summary>
        /// <param name="celsius">Temperature in Celsius</param>
        /// <returns>Temperature in Fahrenheit</returns>
        public static double CelsiusToFahrenheit(double celsius) => (celsius * 9.0 / 5.0) + 32;

        /// <summary>
        /// Convert temperature from Fahrenheit to Celsius
        /// </summary>
        /// <param name="fahrenheit">Temperature in Fahrenheit</param>
        /// <returns>Temperature in Celsius</returns>
        public static double FahrenheitToCelsius(double fahrenheit) => (fahrenheit - 32) * 5.0 / 9.0;
    }
}
