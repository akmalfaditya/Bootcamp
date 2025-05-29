using System;

namespace StructDemo
{
    /// <summary>
    /// Immutable point struct - readonly struct
    /// Once created, this cannot be modified - great for thread safety!
    /// All fields must be readonly in a readonly struct
    /// </summary>
    public readonly struct ImmutablePoint
    {
        // All fields must be readonly
        public readonly int X;
        public readonly int Y;
        
        /// <summary>
        /// Constructor for immutable point
        /// This is the ONLY place where fields can be set
        /// </summary>
        public ImmutablePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        /// <summary>
        /// Immutable operation - returns a NEW instance instead of modifying this one
        /// This is the immutable pattern: operations return new objects
        /// </summary>
        public ImmutablePoint Move(int deltaX, int deltaY)
        {
            return new ImmutablePoint(X + deltaX, Y + deltaY);
        }
        
        /// <summary>
        /// Calculate distance - this method doesn't modify anything
        /// All methods in readonly structs are implicitly readonly
        /// </summary>
        public double DistanceFromOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        
        /// <summary>
        /// Static method for distance calculation
        /// </summary>
        public static double Distance(ImmutablePoint p1, ImmutablePoint p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        
        public override string ToString()
        {
            return $"ImmutablePoint({X}, {Y})";
        }
        
        /// <summary>
        /// Equality comparison for immutable types
        /// Since they can't change, equality is straightforward
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is ImmutablePoint other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        
        // Operator overloading works great with immutable structs
        public static bool operator ==(ImmutablePoint left, ImmutablePoint right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(ImmutablePoint left, ImmutablePoint right)
        {
            return !left.Equals(right);
        }
    }
    
    /// <summary>
    /// Rectangle struct demonstrating readonly methods
    /// The struct itself is mutable, but some methods promise not to modify it
    /// </summary>
    public struct Rectangle
    {
        public double Width;
        public double Height;
        
        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }
        
        /// <summary>
        /// Readonly method - promises not to modify any fields
        /// Can be called on readonly references without defensive copying
        /// </summary>
        public readonly double CalculateArea()
        {
            return Width * Height;
        }
        
        /// <summary>
        /// Another readonly method
        /// </summary>
        public readonly double CalculatePerimeter()
        {
            return 2 * (Width + Height);
        }
        
        /// <summary>
        /// Readonly method that returns information
        /// </summary>
        public readonly string GetInfo()
        {
            return $"Rectangle: {Width}x{Height}, Area: {CalculateArea():F2}";
        }
        
        /// <summary>
        /// Non-readonly method - can modify the struct
        /// </summary>
        public void Scale(double factor)
        {
            Width *= factor;
            Height *= factor;
        }
        
        /// <summary>
        /// This would be a compiler error in a readonly method:
        /// public readonly void BadMethod()
        /// {
        ///     Width = 10;  // Error: cannot modify fields in readonly method
        /// }
        /// </summary>
        
        public override string ToString()
        {
            return $"Rectangle({Width:F1}x{Height:F1})";
        }
    }
    
    /// <summary>
    /// Ref struct - MUST live on the stack only
    /// Cannot be boxed, cannot be array elements, cannot be class fields
    /// Perfect for high-performance scenarios where you need stack-only guarantees
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
        
        /// <summary>
        /// Process data efficiently on the stack
        /// No heap allocations, no GC pressure
        /// </summary>
        public void ProcessEfficiently()
        {
            // Imagine high-performance calculations here
            // All operations stay on the stack
            X = X * 2;
            Y = Y * 2;
        }
        
        public override string ToString()
        {
            return $"StackOnlyPoint({X}, {Y})";
        }
        
        // Note: These operations would cause compiler errors:
        // 1. Cannot box: object boxed = this;
        // 2. Cannot be array element: var array = new StackOnlyPoint[10];
        // 3. Cannot be class field: class Container { StackOnlyPoint point; }
        // 4. Cannot be used with generics that might box
    }
    
    /// <summary>
    /// Vector2D for mathematical operations
    /// Shows floating-point struct with operator overloading
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
        
        /// <summary>
        /// Calculate vector magnitude
        /// </summary>
        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);
        
        /// <summary>
        /// Normalize the vector (return unit vector)
        /// </summary>
        public Vector2D Normalize()
        {
            float mag = Magnitude;
            if (mag == 0) return new Vector2D(0, 0);
            return new Vector2D(X / mag, Y / mag);
        }
        
        // Operator overloading - very common with mathematical structs
        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X + b.X, a.Y + b.Y);
        }
        
        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X - b.X, a.Y - b.Y);
        }
        
        public static Vector2D operator *(Vector2D v, float scalar)
        {
            return new Vector2D(v.X * scalar, v.Y * scalar);
        }
        
        public override string ToString()
        {
            return $"Vector2D({X:F2}, {Y:F2})";
        }
    }
}
