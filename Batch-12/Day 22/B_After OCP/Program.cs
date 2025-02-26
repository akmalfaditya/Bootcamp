using System;

public interface IOperation
{
    double Calculate(double a, double b);
}

public class Addition : IOperation
{
    public double Calculate(double a, double b)
    {
        return a + b;
    }
}

public class Subtraction : IOperation
{
    public double Calculate(double a, double b)
    {
        return a - b;
    }
}

public class Multiplication : IOperation
{
    public double Calculate(double a, double b)
    {
        return a * b;
    }
}

public class Division : IOperation
{
    public double Calculate(double a, double b)
    {
        return a / b;
    }
}

public class Calculator
{
    public double Calculate(double a, double b, IOperation operation)
    {
        return operation.Calculate(a, b);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var calculator = new Calculator();

        Console.WriteLine("Addition: " + calculator.Calculate(5, 3, new Addition()));
        Console.WriteLine("Subtraction: " + calculator.Calculate(5, 3, new Subtraction()));
        Console.WriteLine("Multiplication: " + calculator.Calculate(5, 3, new Multiplication()));
        Console.WriteLine("Division: " + calculator.Calculate(6, 3, new Division()));
    }
}
