using System;

namespace Classes
{
    /// <summary>
    /// MathUtilities static class demonstrating static class concepts
    /// Static classes cannot be instantiated - they exist purely to group related functionality
    /// Think of them as utility libraries - you use the class name directly, never create instances
    /// Perfect for helper methods, constants, and extension methods
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Static field to track how many calculations we've performed
        /// Static fields belong to the type itself, not to any instance
        /// </summary>
        private static int _calculationCount = 0;

        /// <summary>
        /// Static property to expose the calculation count
        /// </summary>
        public static int CalculationCount => _calculationCount;

        /// <summary>
        /// Static property to check if utilities are initialized
        /// </summary>
        public static bool IsInitialized { get; private set; } = true;

        /// <summary>
        /// Static constructor - runs once when the type is first accessed
        /// You cannot control when this runs - it's automatic
        /// Perfect for one-time initialization of static data
        /// </summary>
        static MathUtilities()
        {
            Console.WriteLine($"  🔧 MathUtilities static constructor called - initializing utility class");
            _calculationCount = 0;
        }

        /// <summary>
        /// Static method to calculate the greatest common divisor
        /// Static methods can only access static members - they have no 'this' reference
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Greatest common divisor</returns>
        public static int GreatestCommonDivisor(int a, int b)
        {
            _calculationCount++;
            Console.WriteLine($"  🧮 Calculating GCD of {a} and {b} (calculation #{_calculationCount})");
            
            // Euclidean algorithm
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            
            Console.WriteLine($"  ✅ GCD result: {a}");
            return a;
        }

        /// <summary>
        /// Static method to calculate the least common multiple
        /// Shows how static methods can call other static methods
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Least common multiple</returns>
        public static int LeastCommonMultiple(int a, int b)
        {
            _calculationCount++;
            Console.WriteLine($"  🧮 Calculating LCM of {a} and {b} (calculation #{_calculationCount})");
            
            // LCM(a,b) = |a*b| / GCD(a,b)
            int gcd = GreatestCommonDivisor(a, b); // Calling another static method
            int lcm = Math.Abs(a * b) / gcd;
            
            Console.WriteLine($"  ✅ LCM result: {lcm}");
            return lcm;
        }

        /// <summary>
        /// Static method to check if a number is prime
        /// </summary>
        /// <param name="number">Number to check</param>
        /// <returns>True if prime, false otherwise</returns>
        public static bool IsPrime(int number)
        {
            _calculationCount++;
            Console.WriteLine($"  🔍 Checking if {number} is prime (calculation #{_calculationCount})");
            
            if (number < 2)
            {
                Console.WriteLine($"  ❌ {number} is not prime (less than 2)");
                return false;
            }
            
            if (number == 2)
            {
                Console.WriteLine($"  ✅ {number} is prime (it's 2!)");
                return true;
            }
            
            if (number % 2 == 0)
            {
                Console.WriteLine($"  ❌ {number} is not prime (even number)");
                return false;
            }
            
            // Check odd divisors up to sqrt(number)
            for (int i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0)
                {
                    Console.WriteLine($"  ❌ {number} is not prime (divisible by {i})");
                    return false;
                }
            }
            
            Console.WriteLine($"  ✅ {number} is prime!");
            return true;
        }

        /// <summary>
        /// Static method to calculate factorial
        /// Shows recursive static methods
        /// </summary>
        /// <param name="n">Number to calculate factorial for</param>
        /// <returns>Factorial of n</returns>
        public static long Factorial(int n)
        {
            _calculationCount++;
            
            if (n < 0)
            {
                throw new ArgumentException("Cannot calculate factorial of negative number");
            }
            
            if (n <= 1)
            {
                Console.WriteLine($"  🧮 Factorial base case: {n}! = 1 (calculation #{_calculationCount})");
                return 1;
            }
            
            Console.WriteLine($"  🧮 Calculating {n}! (calculation #{_calculationCount})");
            long result = n * Factorial(n - 1); // Recursive call
            Console.WriteLine($"  ✅ {n}! = {result}");
            return result;
        }

        /// <summary>
        /// Static method to display calculation statistics
        /// </summary>
        public static void DisplayStatistics()
        {
            Console.WriteLine($"  📊 MathUtilities Statistics:");
            Console.WriteLine($"      Total calculations performed: {_calculationCount}");
            Console.WriteLine($"      Static class is ready for more calculations!");
        }

        /// <summary>
        /// Static method to reset calculation count
        /// Useful for testing or demonstration purposes
        /// </summary>
        public static void ResetCalculationCount()
        {
            int oldCount = _calculationCount;
            _calculationCount = 0;
            Console.WriteLine($"  🔄 Reset calculation count from {oldCount} to 0");
        }

        /// <summary>
        /// Simple Add method for compatibility
        /// </summary>
        public static double Add(double a, double b)
        {
            _calculationCount++;
            return a + b;
        }

        /// <summary>
        /// Square root method
        /// </summary>
        public static double SquareRoot(double number)
        {
            _calculationCount++;
            if (number < 0)
                throw new ArgumentException("Cannot calculate square root of negative number");
            return Math.Sqrt(number);
        }
    }
}
