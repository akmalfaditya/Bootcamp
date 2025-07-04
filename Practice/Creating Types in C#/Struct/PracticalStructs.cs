using System;

namespace StructDemo
{
    /// <summary>
    /// Color struct - represents RGB color values
    /// Perfect example of a struct: small, immutable, value-like semantics
    /// </summary>
    public readonly struct Color
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
        
        /// <summary>
        /// Factory method for creating colors from RGB values
        /// Provides a clean API for color creation
        /// </summary>
        public static Color FromRgb(byte r, byte g, byte b)
        {
            return new Color(r, g, b);
        }
        
        /// <summary>
        /// Convert to hexadecimal representation
        /// Useful for web development and debugging
        /// </summary>
        public string ToHex()
        {
            return $"#{R:X2}{G:X2}{B:X2}";
        }
        
        /// <summary>
        /// Calculate brightness (luminance) of the color
        /// Uses standard RGB to luminance formula
        /// </summary>
        public double Brightness()
        {
            return (0.299 * R + 0.587 * G + 0.114 * B) / 255.0;
        }
        
        /// <summary>
        /// Blend this color with another color
        /// Demonstrates how structs can have complex operations
        /// </summary>
        public Color BlendWith(Color other, double ratio)
        {
            // Clamp ratio between 0 and 1
            ratio = Math.Max(0, Math.Min(1, ratio));
            
            byte newR = (byte)(R * (1 - ratio) + other.R * ratio);
            byte newG = (byte)(G * (1 - ratio) + other.G * ratio);
            byte newB = (byte)(B * (1 - ratio) + other.B * ratio);
            
            return new Color(newR, newG, newB);
        }
        
        // Common colors as static properties
        public static Color Red => new Color(255, 0, 0);
        public static Color Green => new Color(0, 255, 0);
        public static Color Blue => new Color(0, 0, 255);
        public static Color White => new Color(255, 255, 255);
        public static Color Black => new Color(0, 0, 0);
        
        public override string ToString()
        {
            return $"Color(R:{R}, G:{G}, B:{B}) [{ToHex()}]";
        }
        
        public override bool Equals(object? obj)
        {
            if (obj is Color other)
                return R == other.R && G == other.G && B == other.B;
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B);
        }
        
        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }
    }
    
    /// <summary>
    /// Money struct demonstrating financial calculations
    /// Immutable value type perfect for representing currency
    /// </summary>
    public readonly struct Money
    {
        public readonly decimal Amount;
        public readonly string Currency;
        
        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative");
            if (string.IsNullOrEmpty(currency))
                throw new ArgumentException("Currency cannot be null or empty");
                
            Amount = amount;
            Currency = currency.ToUpper();
        }
        
        /// <summary>
        /// Add two money values (same currency only)
        /// Demonstrates operator overloading in structs
        /// </summary>
        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException($"Cannot add {left.Currency} and {right.Currency}");
                
            return new Money(left.Amount + right.Amount, left.Currency);
        }
        
        /// <summary>
        /// Subtract two money values (same currency only)
        /// Shows how structs can enforce business rules
        /// </summary>
        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException($"Cannot subtract {left.Currency} and {right.Currency}");
                
            return new Money(left.Amount - right.Amount, left.Currency);
        }
        
        /// <summary>
        /// Multiply money by a scalar value
        /// Useful for calculations like tax, discount, etc.
        /// </summary>
        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money.Amount * multiplier, money.Currency);
        }
        
        public override string ToString()
        {
            return $"{Amount:C} {Currency}";
        }
        
        public override bool Equals(object? obj)
        {
            if (obj is Money other)
                return Amount == other.Amount && Currency == other.Currency;
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
    
    /// <summary>
    /// 2D Coordinate struct demonstrating practical geometry applications
    /// Immutable, mathematical value type
    /// </summary>
    public readonly struct Coordinate2D
    {
        public readonly double X;
        public readonly double Y;
        
        public Coordinate2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        /// <summary>
        /// Calculate distance to another coordinate
        /// Classic geometric operation
        /// </summary>
        public double DistanceTo(Coordinate2D other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        
        /// <summary>
        /// Calculate distance from origin (0,0)
        /// Useful for magnitude calculations
        /// </summary>
        public double DistanceFromOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        
        /// <summary>
        /// Get midpoint between this and another coordinate
        /// Returns a new coordinate (immutable operation)
        /// </summary>
        public Coordinate2D MidpointTo(Coordinate2D other)
        {
            return new Coordinate2D((X + other.X) / 2, (Y + other.Y) / 2);
        }
        
        public override string ToString()
        {
            return $"({X:F2}, {Y:F2})";
        }
        
        // Common coordinate constants
        public static Coordinate2D Origin => new Coordinate2D(0, 0);
        public static Coordinate2D UnitX => new Coordinate2D(1, 0);
        public static Coordinate2D UnitY => new Coordinate2D(0, 1);
    }
    
    /// <summary>
    /// Complex number struct for mathematical operations
    /// Demonstrates how structs excel at mathematical concepts
    /// </summary>
    public readonly struct Complex
    {
        public readonly double Real;
        public readonly double Imaginary;
        
        public Complex(double real, double imaginary = 0)
        {
            Real = real;
            Imaginary = imaginary;
        }
        
        /// <summary>
        /// Calculate magnitude (absolute value) of complex number
        /// |a + bi| = sqrt(a² + b²)
        /// </summary>
        public double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);
        
        /// <summary>
        /// Calculate phase angle in radians
        /// </summary>
        public double Phase => Math.Atan2(Imaginary, Real);
        
        /// <summary>
        /// Add two complex numbers
        /// (a + bi) + (c + di) = (a + c) + (b + d)i
        /// </summary>
        public static Complex operator +(Complex left, Complex right)
        {
            return new Complex(left.Real + right.Real, left.Imaginary + right.Imaginary);
        }
        
        /// <summary>
        /// Subtract two complex numbers
        /// </summary>
        public static Complex operator -(Complex left, Complex right)
        {
            return new Complex(left.Real - right.Real, left.Imaginary - right.Imaginary);
        }
        
        /// <summary>
        /// Multiply two complex numbers
        /// (a + bi)(c + di) = (ac - bd) + (ad + bc)i
        /// </summary>
        public static Complex operator *(Complex left, Complex right)
        {
            double real = left.Real * right.Real - left.Imaginary * right.Imaginary;
            double imaginary = left.Real * right.Imaginary + left.Imaginary * right.Real;
            return new Complex(real, imaginary);
        }
        
        public override string ToString()
        {
            if (Imaginary >= 0)
                return $"{Real:F2} + {Imaginary:F2}i";
            else
                return $"{Real:F2} - {Math.Abs(Imaginary):F2}i";
        }
        
        // Common complex number constants
        public static Complex Zero => new Complex(0, 0);
        public static Complex One => new Complex(1, 0);
        public static Complex I => new Complex(0, 1);
    }
    
    /// <summary>
    /// Time range struct for scheduling applications
    /// Demonstrates practical business logic in structs
    /// </summary>
    public readonly struct TimeRange
    {
        public readonly TimeSpan Start;
        public readonly TimeSpan End;
        
        public TimeRange(TimeSpan start, TimeSpan end)
        {
            if (start > end)
                throw new ArgumentException("Start time cannot be after end time");
                
            Start = start;
            End = end;
        }
        
        /// <summary>
        /// Duration of the time range
        /// </summary>
        public TimeSpan Duration => End - Start;
        
        /// <summary>
        /// Check if this range overlaps with another
        /// Useful for scheduling conflicts
        /// </summary>
        public bool OverlapsWith(TimeRange other)
        {
            return Start < other.End && End > other.Start;
        }
        
        /// <summary>
        /// Check if a specific time falls within this range
        /// </summary>
        public bool Contains(TimeSpan time)
        {
            return time >= Start && time <= End;
        }
        
        /// <summary>
        /// Get the intersection of two time ranges
        /// Returns null if no overlap exists
        /// </summary>
        public TimeRange? GetIntersection(TimeRange other)
        {
            var maxStart = Start > other.Start ? Start : other.Start;
            var minEnd = End < other.End ? End : other.End;
            
            if (maxStart <= minEnd)
                return new TimeRange(maxStart, minEnd);
                
            return null;
        }
        
        public override string ToString()
        {
            return $"{Start:hh\\:mm} - {End:hh\\:mm} ({Duration:hh\\:mm})";
        }
    }
}
