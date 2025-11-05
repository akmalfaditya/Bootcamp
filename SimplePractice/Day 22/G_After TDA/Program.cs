using System;

public class Rectangle
{
    public int Width { private get; set; }
    public int Height { private get; set; }

    public int CalculateArea()
    {
        return Width*Height;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var rectangle = new Rectangle { Width = 10, Height = 5 };

        int area = rectangle.CalculateArea();
        Console.WriteLine("The area of the rectangle is: " + area);
    }
}
