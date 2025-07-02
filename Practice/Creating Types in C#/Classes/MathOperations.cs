using System;

namespace Classes
{
    /// <summary>
    /// Demonstrates different types of methods:
    /// - Regular methods with full body
    /// - Expression-bodied methods (concise syntax)
    /// - Methods with different return types
    /// </summary>
    public class MathOperations
    {
        /// <summary>
        /// Expression-bodied method - perfect for simple operations
        /// The => syntax is like saying "this method returns..."
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Sum of a and b</returns>
        public int Add(int a, int b) => a + b;

        /// <summary>
        /// Another expression-bodied method for multiplication
        /// Notice how clean and readable this is for simple operations
        /// </summary>
        public int Multiply(int a, int b) => a * b;

        /// <summary>
        /// Subtraction method - adding this for the demo
        /// </summary>
        public int Subtract(int a, int b) => a - b;

        /// <summary>
        /// Expression-bodied method that returns a boolean
        /// Great for simple true/false checks
        /// </summary>
        public bool IsEven(int number) => number % 2 == 0;

        /// <summary>
        /// Regular method with full body - use this when you need more complex logic
        /// Sometimes you need multiple statements, and that's where traditional methods shine
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Division result</returns>
        public double Divide(double a, double b)
        {
            // We need validation here, so expression-bodied wouldn't work well
            if (b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero!");
            }
            
            double result = a / b;
            Console.WriteLine($"Dividing {a} by {b} = {result}");
            return result;
        }

        /// <summary>
        /// Method that doesn't return anything (void)
        /// These are great for actions that just "do something"
        /// </summary>
        /// <param name="message">Message to display</param>
        public void DisplayMessage(string message)
        {
            Console.WriteLine($"[MathOperations] {message}");
        }

        /// <summary>
        /// Method with optional parameters
        /// Shows more advanced method features
        /// </summary>
        /// <param name="base">Base number</param>
        /// <param name="exponent">Power to raise to (default is 2)</param>
        /// <returns>Base raised to the exponent</returns>
        public double Power(double @base, int exponent = 2)
        {
            return Math.Pow(@base, exponent);
        }

        /// <summary>
        /// Method that demonstrates method overloading
        /// Same method name, different parameters
        /// </summary>
        /// <param name="numbers">Array of numbers to add</param>
        /// <returns>Sum of all numbers</returns>
        public int Add(params int[] numbers)
        {
            int sum = 0;
            foreach (int number in numbers)
            {
                sum += number;
            }
            return sum;
        }

        /// <summary>
        /// Method demonstrating 'out' parameters
        /// 'out' means the method MUST assign a value to the parameter
        /// Great for returning multiple values from one method
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="sum">Output: the sum</param>
        /// <param name="product">Output: the product</param>
        public void CalculateSumAndProduct(int a, int b, out int sum, out int product)
        {
            sum = a + b;        // Must assign to 'out' parameters
            product = a * b;    // Must assign to 'out' parameters
            Console.WriteLine($"Calculated sum: {sum}, product: {product}");
        }

        /// <summary>
        /// Method demonstrating 'ref' parameters
        /// 'ref' means the parameter is passed by reference - changes affect the original
        /// The variable must be initialized before calling this method
        /// </summary>
        /// <param name="value">Value to square (passed by reference)</param>
        public void SquareByReference(ref int value)
        {
            Console.WriteLine($"Original value: {value}");
            value = value * value;  // This changes the original variable!
            Console.WriteLine($"Squared value: {value}");
        }

        /// <summary>
        /// Method demonstrating local methods (methods inside methods)
        /// Local methods are great for helper functionality that's only used within one method
        /// </summary>
        /// <param name="n">Number to calculate factorial for</param>
        /// <returns>Factorial of n</returns>
        public long CalculateFactorial(int n)
        {
            // Input validation
            if (n < 0)
                throw new ArgumentException("Cannot calculate factorial of negative number");

            // Local method - only exists within this method
            // This is like having a private helper method, but even more localized
            long FactorialHelper(int number)
            {
                if (number <= 1)
                    return 1;
                return number * FactorialHelper(number - 1);
            }

            // Call our local method
            long result = FactorialHelper(n);
            Console.WriteLine($"Factorial of {n} = {result}");
            return result;
        }

        /// <summary>
        /// Method showing complex logic with local variables
        /// Demonstrates how to organize code within a method
        /// </summary>
        /// <param name="numbers">Array of numbers to analyze</param>
        public void AnalyzeNumbers(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                Console.WriteLine("No numbers to analyze!");
                return;
            }

            // Local variables to track our calculations
            int sum = 0;
            int min = numbers[0];
            int max = numbers[0];
            int evenCount = 0;

            // Process each number
            foreach (int number in numbers)
            {
                sum += number;
                
                if (number < min) min = number;
                if (number > max) max = number;
                if (number % 2 == 0) evenCount++;
            }

            // Calculate average
            double average = (double)sum / numbers.Length;

            // Display results
            Console.WriteLine($"Analysis of {numbers.Length} numbers:");
            Console.WriteLine($"  Sum: {sum}");
            Console.WriteLine($"  Average: {average:F2}");
            Console.WriteLine($"  Min: {min}");
            Console.WriteLine($"  Max: {max}");
            Console.WriteLine($"  Even numbers: {evenCount}");
        }

        /// <summary>
        /// Override ToString for nice string representation
        /// Every class should have a meaningful ToString method
        /// </summary>
        /// <returns>String description of this MathOperations instance</returns>
        public override string ToString()
        {
            return "MathOperations Calculator - Ready for mathematical adventures!";
        }
    }
}
