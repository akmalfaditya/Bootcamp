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
    }
}
