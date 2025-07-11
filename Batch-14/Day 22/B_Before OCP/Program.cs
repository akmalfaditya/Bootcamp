using System;

public class Calculator
{
    public double Calculate(double a, double b, string operation)
    {
        switch (operation)
        {
            case "add":
                return a + b;
            case "subtract":
                return a - b;
            case "multiply":
                return a * b;
            case "divide":
                return a / b;
            default:
                throw new InvalidOperationException("Invalid operation");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var calculator = new Calculator();
        Console.WriteLine("Addition: " + calculator.Calculate(5, 3, "add"));
        Console.WriteLine("Subtraction: " + calculator.Calculate(5, 3, "subtract"));
        Console.WriteLine("Multiplication: " + calculator.Calculate(5, 3, "multiply"));
        Console.WriteLine("Division: " + calculator.Calculate(6, 3, "divide"));
    }
}
