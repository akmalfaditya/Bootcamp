using System;

namespace Classes
{
    /// <summary>
    /// Rectangle class demonstrating deconstructors and value tuples
    /// Deconstructors allow you to "unpack" an object into individual variables
    /// Think of it as the opposite of a constructor - instead of building an object from parts,
    /// you're breaking an object down into its component parts
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// Properties for rectangle dimensions
        /// </summary>
        public float Width { get; set; }
        public float Height { get; set; }

        /// <summary>
        /// Computed properties for additional rectangle information
        /// </summary>
        public float Area => Width * Height;
        public float Perimeter => 2 * (Width + Height);
        public bool IsSquare => Math.Abs(Width - Height) < 0.001f;

        /// <summary>
        /// Constructor to create a rectangle
        /// </summary>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        public Rectangle(float width, float height)
        {
            Width = width;
            Height = height;
            Console.WriteLine($"  📐 Created rectangle: {width} x {height} (Area: {Area:F2})");
        }

        /// <summary>
        /// Primary deconstructor - the most common pattern
        /// This allows: var (width, height) = rectangle;
        /// The 'out' keyword is required for deconstructors
        /// </summary>
        /// <param name="width">Deconstructed width</param>
        /// <param name="height">Deconstructed height</param>
        public void Deconstruct(out float width, out float height)
        {
            width = Width;
            height = Height;
            Console.WriteLine($"  🔓 Deconstructed rectangle into width: {width}, height: {height}");
        }

        /// <summary>
        /// Alternative deconstructor with more information
        /// You can have multiple deconstructors with different signatures
        /// This allows: var (width, height, area) = rectangle;
        /// </summary>
        /// <param name="width">Deconstructed width</param>
        /// <param name="height">Deconstructed height</param>
        /// <param name="area">Calculated area</param>
        public void Deconstruct(out float width, out float height, out float area)
        {
            width = Width;
            height = Height;
            area = Area;
            Console.WriteLine($"  🔓 Deconstructed rectangle into width: {width}, height: {height}, area: {area:F2}");
        }

        /// <summary>
        /// Full deconstructor with all rectangle properties
        /// This allows: var (width, height, area, perimeter, isSquare) = rectangle;
        /// </summary>
        /// <param name="width">Deconstructed width</param>
        /// <param name="height">Deconstructed height</param>
        /// <param name="area">Calculated area</param>
        /// <param name="perimeter">Calculated perimeter</param>
        /// <param name="isSquare">Whether it's a square</param>
        public void Deconstruct(out float width, out float height, out float area, out float perimeter, out bool isSquare)
        {
            width = Width;
            height = Height;
            area = Area;
            perimeter = Perimeter;
            isSquare = IsSquare;
            Console.WriteLine($"  🔓 Full deconstruction: w={width}, h={height}, area={area:F2}, perimeter={perimeter:F2}, square={isSquare}");
        }

        /// <summary>
        /// Method to resize the rectangle
        /// </summary>
        /// <param name="newWidth">New width</param>
        /// <param name="newHeight">New height</param>
        public void Resize(float newWidth, float newHeight)
        {
            Console.WriteLine($"  📏 Resizing from {Width}x{Height} to {newWidth}x{newHeight}");
            Width = newWidth;
            Height = newHeight;
        }

        /// <summary>
        /// Method to scale the rectangle by a factor
        /// </summary>
        /// <param name="scaleFactor">Factor to scale by</param>
        public void Scale(float scaleFactor)
        {
            Console.WriteLine($"  🔍 Scaling rectangle by factor {scaleFactor}");
            Width *= scaleFactor;
            Height *= scaleFactor;
        }

        /// <summary>
        /// Method to display rectangle information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  📊 Rectangle Info:");
            Console.WriteLine($"      Dimensions: {Width} x {Height}");
            Console.WriteLine($"      Area: {Area:F2}");
            Console.WriteLine($"      Perimeter: {Perimeter:F2}");
            Console.WriteLine($"      Is Square: {(IsSquare ? "Yes" : "No")}");
        }

        /// <summary>
        /// Static method to demonstrate multiple deconstructors
        /// </summary>
        /// <param name="rect">Rectangle to demonstrate with</param>
        public static void DemonstrateDeconstruction(Rectangle rect)
        {
            Console.WriteLine($"  🎭 Demonstrating deconstruction with {rect}");

            // Basic deconstruction (2 values)
            var (w1, h1) = rect;
            Console.WriteLine($"    Basic: width={w1}, height={h1}");

            // Extended deconstruction (3 values)
            var (w2, h2, area) = rect;
            Console.WriteLine($"    Extended: width={w2}, height={h2}, area={area:F2}");

            // Full deconstruction (5 values)
            var (w3, h3, fullArea, perimeter, isSquare) = rect;
            Console.WriteLine($"    Full: w={w3}, h={h3}, area={fullArea:F2}, perimeter={perimeter:F2}, square={isSquare}");
        }

        /// <summary>
        /// Override ToString for nice string representation
        /// </summary>
        /// <returns>String description of the rectangle</returns>
        public override string ToString()
        {
            return $"Rectangle({Width} x {Height})";
        }
    }
}
