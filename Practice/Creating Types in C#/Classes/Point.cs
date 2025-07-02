using System;

namespace Classes
{
    /// <summary>
    /// Point class demonstrating primary constructors and additional features
    /// This class shows how primary constructors work with other C# features
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    public class Point(int x, int y)
    {
        /// <summary>
        /// Properties exposing the primary constructor parameters
        /// The parameters x and y are available throughout the class
        /// </summary>
        public int X => x;
        public int Y => y;

        /// <summary>
        /// Computed property using primary constructor parameters
        /// </summary>
        public double DistanceFromOrigin => Math.Sqrt(x * x + y * y);

        /// <summary>
        /// Method to calculate distance to another point
        /// </summary>
        /// <param name="other">Other point</param>
        /// <returns>Distance between points</returns>
        public double DistanceTo(Point other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
                
            int dx = x - other.X;  // Using properties instead of parameters
            int dy = y - other.Y;
            
            double distance = Math.Sqrt(dx * dx + dy * dy);
            Console.WriteLine($"  📏 Distance from ({x}, {y}) to ({other.X}, {other.Y}) = {distance:F2}");
            return distance;
        }

        /// <summary>
        /// Method to move the point (returns new point since this one is immutable)
        /// </summary>
        /// <param name="deltaX">Change in X</param>
        /// <param name="deltaY">Change in Y</param>
        /// <returns>New point at moved location</returns>
        public Point Move(int deltaX, int deltaY)
        {
            var newPoint = new Point(x + deltaX, y + deltaY);
            Console.WriteLine($"  🎯 Moved from ({x}, {y}) to ({newPoint.X}, {newPoint.Y})");
            return newPoint;
        }

        /// <summary>
        /// Method to check if point is at origin
        /// </summary>
        /// <returns>True if at origin (0,0)</returns>
        public bool IsAtOrigin() => x == 0 && y == 0;

        /// <summary>
        /// Method to get quadrant information
        /// </summary>
        /// <returns>Quadrant number (1-4) or 0 if on axis</returns>
        public int GetQuadrant()
        {
            if (x > 0 && y > 0) return 1;
            if (x < 0 && y > 0) return 2;
            if (x < 0 && y < 0) return 3;
            if (x > 0 && y < 0) return 4;
            return 0; // On axis
        }

        /// <summary>
        /// Deconstructor for the Point
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void Deconstruct(out int x, out int y)
        {
            x = this.X;  // Using properties
            y = this.Y;  // Using properties
        }

        /// <summary>
        /// Display point information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  📍 Point Information:");
            Console.WriteLine($"      Coordinates: ({x}, {y})");
            Console.WriteLine($"      Distance from origin: {DistanceFromOrigin:F2}");
            Console.WriteLine($"      Quadrant: {GetQuadrant()}");
            Console.WriteLine($"      At origin: {(IsAtOrigin() ? "Yes" : "No")}");
        }

        /// <summary>
        /// Override ToString using primary constructor parameters
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"Point({x}, {y})";
        }

        /// <summary>
        /// Override Equals for value comparison
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        /// <returns>True if equal</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Point other)
            {
                return x == other.X && y == other.Y;
            }
            return false;
        }

        /// <summary>
        /// Override GetHashCode
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        /// <summary>
        /// Static method to create point at origin
        /// </summary>
        /// <returns>Point at (0,0)</returns>
        public static Point Origin() => new Point(0, 0);

        /// <summary>
        /// Static method to demonstrate point operations
        /// </summary>
        public static void DemonstratePointOperations()
        {
            Console.WriteLine($"  🎯 Demonstrating Point Operations:");
            
            var point1 = new Point(3, 4);
            var point2 = new Point(-2, 1);
            
            point1.DisplayInfo();
            point2.DisplayInfo();
            
            // Calculate distance
            double distance = point1.DistanceTo(point2);
            
            // Move points
            var movedPoint = point1.Move(2, -1);
            
            // Deconstruction
            var (x, y) = point2;
            Console.WriteLine($"  🔓 Deconstructed point2 to x={x}, y={y}");
            
            // Origin point
            var origin = Point.Origin();
            Console.WriteLine($"  🏠 Origin point: {origin}");
        }
    }
}
