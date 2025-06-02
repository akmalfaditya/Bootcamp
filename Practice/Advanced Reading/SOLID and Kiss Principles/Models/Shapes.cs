namespace SOLID_and_Kiss_Principles.Models;

/// <summary>
/// Base shape class for demonstrating Open/Closed Principle
/// Abstract because we don't want to instantiate a generic "shape"
/// </summary>
public abstract class Shape
{
    public abstract double Area();
    public abstract string GetShapeInfo();
}

/// <summary>
/// Rectangle implementation - simple and focused
/// Each shape knows how to calculate its own area
/// </summary>
public class Rectangle : Shape
{
    public double Height { get; set; }
    public double Width { get; set; }

    public Rectangle(double height, double width)
    {
        Height = height;
        Width = width;
    }

    public override double Area()
    {
        return Height * Width;
    }

    public override string GetShapeInfo()
    {
        return $"Rectangle: {Width} x {Height}, Area: {Area()}";
    }
}

/// <summary>
/// Circle implementation - follows the same pattern
/// Notice how adding this didn't require changing AreaCalculator
/// </summary>
public class Circle : Shape
{
    public double Radius { get; set; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public override double Area()
    {
        return Math.PI * Radius * Radius;
    }

    public override string GetShapeInfo()
    {
        return $"Circle: radius {Radius}, Area: {Area():F2}";
    }
}

/// <summary>
/// Triangle implementation - added later without breaking existing code
/// This is the Open/Closed Principle in action!
/// </summary>
public class Triangle : Shape
{
    public double Base { get; set; }
    public double Height { get; set; }

    public Triangle(double baseLength, double height)
    {
        Base = baseLength;
        Height = height;
    }

    public override double Area()
    {
        return 0.5 * Base * Height;
    }

    public override string GetShapeInfo()
    {
        return $"Triangle: base {Base}, height {Height}, Area: {Area():F2}";
    }
}
