using System;

public abstract class Shape
{
    public abstract int GetArea();
}

public class Rectangle : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public override int GetArea()
    {
        return Width * Height;
    }
}

public class Square : Shape
{
    public int Side { get; set; }

    public override int GetArea()
    {
        return Side * Side;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Shape shape = new Rectangle { Width = 5, Height = 3 };
        Console.WriteLine("Rectangle area: " + shape.GetArea()); // Correct result: 15

        shape = new Square { Side = 4 };
        Console.WriteLine("Square area: " + shape.GetArea()); // Correct result: 16
    }
}
