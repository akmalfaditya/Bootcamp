using System;

public class Rectangle
{
    public int Width { get; set; }
    public int Height { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        var rectangle = new Rectangle { Width = 10, Height = 5 };

        int area = rectangle.Width * rectangle.Height;
        Console.WriteLine("The area of the rectangle is: " + area);
    }
}
