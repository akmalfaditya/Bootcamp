using System;

namespace StructDemo
{
    /// <summary>
    /// Basic Point struct demonstrating fundamental struct behavior
    /// This is the classic example - simple, small, value-like data
    /// </summary>
    public struct Point
    {
        // Public fields - direct access, no properties needed for simple cases
        public int X;
        public int Y;
        
        /// <summary>
        /// Custom constructor for Point
        /// Note: structs always have an implicit parameterless constructor too
        /// </summary>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        /// <summary>
        /// Override ToString for meaningful display
        /// Always a good practice for any type!
        /// </summary>
        public override string ToString()
        {
            return $"Point({X}, {Y})";
        }
        
        /// <summary>
        /// Calculate distance from origin
        /// Demonstrates instance methods in structs
        /// </summary>
        public double DistanceFromOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        
        /// <summary>
        /// Static method for calculating distance between two points
        /// Shows how structs can have static methods too
        /// </summary>
        public static double Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
    
    /// <summary>
    /// Equivalent class for comparison with struct behavior
    /// Same data, but reference semantics instead of value semantics
    /// </summary>
    public class PointClass
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public override string ToString()
        {
            return $"PointClass({X}, {Y})";
        }
    }
    
    /// <summary>
    /// Interface that structs can implement
    /// This is the ONLY form of "inheritance" structs support
    /// </summary>
    public interface IMovable
    {
        void Move(int deltaX, int deltaY);
    }
    
    /// <summary>
    /// Point struct implementing an interface
    /// Shows that structs CAN implement interfaces (but not inherit from classes)
    /// </summary>
    public struct MovablePoint : IMovable
    {
        public int X;
        public int Y;
        
        public MovablePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        /// <summary>
        /// Implementation of interface method
        /// Note: this modifies the struct, so be careful with copying behavior
        /// </summary>
        public void Move(int deltaX, int deltaY)
        {
            X += deltaX;
            Y += deltaY;
        }
        
        public override string ToString()
        {
            return $"MovablePoint({X}, {Y})";
        }
    }
    
    /// <summary>
    /// Simple shape struct demonstrating regular (non-virtual) methods
    /// No virtual, abstract, or protected members allowed in structs
    /// </summary>
    public struct SimpleShape
    {
        public int Width;
        public int Height;
        
        /// <summary>
        /// Regular method - not virtual, can't be overridden
        /// Structs keep things simple!
        /// </summary>
        public int CalculateArea()
        {
            return Width * Height;
        }
        
        /// <summary>
        /// Another regular method
        /// </summary>
        public int CalculatePerimeter()
        {
            return 2 * (Width + Height);
        }
        
        public override string ToString()
        {
            return $"Shape({Width}x{Height})";
        }
    }
    
    /// <summary>
    /// Modern struct with field initializers (C# 10+)
    /// Shows the difference between 'new' and 'default'
    /// </summary>
    public struct ModernPoint
    {
        // Field initializers - these run when using 'new'
        public int X = 1;  // Default value when using constructor
        public int Y;
        
        /// <summary>
        /// Custom parameterless constructor (C# 10+ feature)
        /// This runs when you call 'new ModernPoint()'
        /// </summary>
        public ModernPoint()
        {
            Y = 1;  // Set Y to 1 when using new
            // X already gets 1 from field initializer
        }
        
        public ModernPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public override string ToString()
        {
            return $"ModernPoint({X}, {Y})";
        }
    }
}
