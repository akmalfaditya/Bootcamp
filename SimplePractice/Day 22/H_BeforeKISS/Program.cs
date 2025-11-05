using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
        List<int> squareNumbers = evenNumbers.Select(n => n * n).ToList();

        Console.WriteLine("Squared even numbers:");
        foreach (int number in squareNumbers)
        {
            Console.WriteLine(number);
        }
    }
}
