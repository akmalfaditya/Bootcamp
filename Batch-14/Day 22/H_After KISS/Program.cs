using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> squaredEvenNumbers = GetSquaredEvenNumbers(numbers);

        Console.WriteLine("Squared even numbers:");
        foreach (int number in squaredEvenNumbers)
        {
            Console.WriteLine(number);
        }
    }

    public static List<int> GetSquaredEvenNumbers(List<int> numbers)
    {
        List<int> squaredEvenNumbers = new List<int>();

        foreach (int number in numbers)
        {
            if (number % 2 == 0)
            {
                squaredEvenNumbers.Add(number * number);
            }
        }

        return squaredEvenNumbers;
    }
}
