using System;

public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int GetArea()
    {
        return Width * Height;
    }
}

public class Square : Rectangle
{
    private int _side;

    public override int Width
    {
        get => _side;
        set => _side = value;
    }

    public override int Height
    {
        get => _side;
        set => _side = value;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Rectangle rect = new Square();
        rect.Width = 5;
        rect.Height = 3;

        Console.WriteLine("Area: " + rect.GetArea()); // Incorrect result: 9 instead of 15
    }
}
