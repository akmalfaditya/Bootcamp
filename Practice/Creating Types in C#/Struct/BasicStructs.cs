using System;

namespace StructDemo
{
    /// <summary>
    /// Basic Point struct demonstrating fundamental struct behavior
    /// This is the classic example - simple, small, value-like data
    /// </summary>
    public struct BasicPoint
    {
        // Public fields - direct access for simplicity
        public int X;
        public int Y;
        
        /// <summary>
        /// Custom constructor for Point
        /// Note: structs always have an implicit parameterless constructor too
        /// </summary>
        public BasicPoint(int x, int y)
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
        public static double Distance(BasicPoint p1, BasicPoint p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }

    /// <summary>
    /// Point class for comparison with struct behavior
    /// Reference type - objects live on heap, variables hold references
    /// </summary>
    public class PointClass
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public PointClass(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public override string ToString()
        {
            return $"PointClass({X}, {Y})";
        }
    }

    /// <summary>
    /// Modern Point struct demonstrating field initializers and modern constructor features
    /// Available from C# 10 and later
    /// </summary>
    public struct ModernPoint
    {
        // Field initializers in structs (C# 10+)
        public int X = 1;    // Default value
        public int Y = 1;    // Default value
        
        /// <summary>
        /// Custom parameterless constructor (C# 10+)
        /// Both this and the implicit default constructor are available
        /// </summary>
        public ModernPoint()
        {
            X = 5;  // Custom default
            Y = 5;  // Custom default
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

    /// <summary>
    /// Advanced Point struct demonstrating field initializers and custom constructors
    /// Shows mixed initialization patterns
    /// </summary>
    public struct AdvancedPoint
    {
        // Field initializers provide defaults
        public int X = 0;
        public int Y = 0;
        public string Label = "Point";
        
        public AdvancedPoint(int x, int y)
        {
            X = x;
            Y = y;
            // Label keeps its field initializer value
        }
        
        public override string ToString()
        {
            return $"{Label}({X}, {Y})";
        }
    }

    /// <summary>
    /// Readonly struct enforcing immutability
    /// All fields are implicitly readonly
    /// </summary>
    public readonly struct ReadOnlyPoint
    {
        // All fields in readonly struct are implicitly readonly
        public readonly int X;
        public readonly int Y;
        
        public ReadOnlyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        /// <summary>
        /// Methods in readonly struct are implicitly readonly
        /// Cannot modify any fields
        /// </summary>
        public double DistanceFromOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        
        public override string ToString()
        {
            return $"ReadOnlyPoint({X}, {Y})";
        }
        
        /// <summary>
        /// Readonly structs can have static methods
        /// Static methods don't operate on instance state anyway
        /// </summary>
        public static ReadOnlyPoint Origin => new ReadOnlyPoint(0, 0);
    }

    /// <summary>
    /// Mutable struct with readonly methods
    /// Demonstrates selective immutability
    /// </summary>
    public struct MutablePoint
    {
        public int X;
        public int Y;
        
        public MutablePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        /// <summary>
        /// Readonly method cannot modify struct fields
        /// Compiler enforces this restriction
        /// </summary>
        public readonly double CalculateDistance()
        {
            return Math.Sqrt(X * X + Y * Y);
            // X = 0; // Would cause compile error in readonly method
        }
        
        /// <summary>
        /// Regular method can modify struct state
        /// Note: this only affects the current instance
        /// </summary>
        public void Reset()
        {
            X = 0;
            Y = 0;
        }
        
        public override string ToString()
        {
            return $"MutablePoint({X}, {Y})";
        }
    }

    /// <summary>
    /// Rectangle struct for demonstrating readonly references
    /// Contains calculated properties
    /// </summary>
    public struct Rectangle
    {
        public int Width;
        public int Height;
        
        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }
        
        /// <summary>
        /// Calculated property - demonstrates readonly behavior
        /// </summary>
        public readonly int Area => Width * Height;
        
        public override string ToString()
        {
            return $"Rectangle({Width}x{Height})";
        }
    }

    /// <summary>
    /// Ref struct that can only exist on the stack
    /// Demonstrates advanced performance optimization scenarios
    /// </summary>
    public ref struct StackOnlyPoint
    {
        public int X;
        public int Y;
        
        public StackOnlyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public override string ToString()
        {
            return $"StackOnlyPoint({X}, {Y})";
        }
        
        /// <summary>
        /// Ref structs can have methods like regular structs
        /// But they're restricted to stack-only scenarios
        /// </summary>
        public double DistanceFromOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        
        // Ref structs cannot:
        // - Be stored in arrays (arrays live on heap)
        // - Be boxed (boxing puts on heap)
        // - Be fields of non-ref structs
        // - Be used in async methods
        // - Implement interfaces (interface boxing)
    }

    /// <summary>
    /// Interface for demonstrating struct interface implementation
    /// Shows that structs can participate in polymorphism through interfaces
    /// </summary>
    public interface IDrawable
    {
        void Draw();
    }

    /// <summary>
    /// Struct implementing an interface
    /// Demonstrates that structs can provide polymorphic behavior
    /// </summary>
    public struct DrawablePoint : IDrawable
    {
        public int X;
        public int Y;
        public string Color;
        
        public DrawablePoint(int x, int y, string color = "Black")
        {
            X = x;
            Y = y;
            Color = color;
        }
        
        public void Draw()
        {
            Console.WriteLine($"Drawing {Color} point at ({X}, {Y})");
        }
        
        public override string ToString()
        {
            return $"DrawablePoint({X}, {Y}, {Color})";
        }
    }

    /// <summary>
    /// Vector2D struct for mathematical operations
    /// Shows practical use of operator overloading
    /// </summary>
    public readonly struct Vector2D
    {
        public readonly float X;
        public readonly float Y;
        
        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);
        
        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X + b.X, a.Y + b.Y);
        }
        
        public override string ToString()
        {
            return $"Vector2D({X:F1}, {Y:F1})";
        }
    }

    /// <summary>
    /// Simple DateTime struct for demonstration
    /// Shows how structs can represent time-based values
    /// </summary>
    public readonly struct SimpleDateTime
    {
        public readonly int Year;
        public readonly int Month;
        public readonly int Day;
        public readonly int Hour;
        public readonly int Minute;
        
        public SimpleDateTime(int year, int month, int day, int hour = 0, int minute = 0)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
        }
        
        public override string ToString()
        {
            return $"{Year:D4}-{Month:D2}-{Day:D2} {Hour:D2}:{Minute:D2}";
        }
    }
}
