using System;

namespace StructDemo
{
    /// <summary>
    /// Color struct - represents RGB color values
    /// Perfect example of a struct: small, immutable, value-like semantics
    /// </summary>
    public readonly struct Color
    {
        public readonly byte R;  // Red component (0-255)
        public readonly byte G;  // Green component (0-255)
        public readonly byte B;  // Blue component (0-255)
        
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
        
        /// <summary>
        /// Create color from hex string
        /// Example: Color.FromHex("#FF0000") for red
        /// </summary>
        public static Color FromHex(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);
            
            if (hex.Length != 6)
                throw new ArgumentException("Hex color must be 6 characters");
            
            byte r = Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = Convert.ToByte(hex.Substring(4, 2), 16);
            
            return new Color(r, g, b);
        }
        
        /// <summary>
        /// Convert to hex string
        /// </summary>
        public string ToHex()
        {
            return $"#{R:X2}{G:X2}{B:X2}";
        }
        
        /// <summary>
        /// Blend two colors together
        /// Returns a new color - immutable operation
        /// </summary>
        public Color Blend(Color other, float ratio)
        {
            if (ratio < 0 || ratio > 1)
                throw new ArgumentException("Ratio must be between 0 and 1");
            
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
        
        public override bool Equals(object obj)
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
    /// Simple DateTime struct (like the built-in DateTime)
    /// Shows how structs are perfect for date/time values
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
            // Simple validation
            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be 1-12");
            if (day < 1 || day > 31)
                throw new ArgumentException("Day must be 1-31");
            if (hour < 0 || hour > 23)
                throw new ArgumentException("Hour must be 0-23");
            if (minute < 0 || minute > 59)
                throw new ArgumentException("Minute must be 0-59");
            
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
        }
        
        /// <summary>
        /// Add days to date (returns new instance)
        /// </summary>
        public SimpleDateTime AddDays(int days)
        {
            // Simplified - real implementation would handle month/year rollover
            return new SimpleDateTime(Year, Month, Day + days, Hour, Minute);
        }
        
        /// <summary>
        /// Add hours to datetime (returns new instance)
        /// </summary>
        public SimpleDateTime AddHours(int hours)
        {
            int newHour = Hour + hours;
            int dayOffset = newHour / 24;
            newHour = newHour % 24;
            
            return new SimpleDateTime(Year, Month, Day + dayOffset, newHour, Minute);
        }
        
        /// <summary>
        /// Format as string
        /// </summary>
        public string ToString(string format)
        {
            return format switch
            {
                "date" => $"{Year:D4}-{Month:D2}-{Day:D2}",
                "time" => $"{Hour:D2}:{Minute:D2}",
                "full" => $"{Year:D4}-{Month:D2}-{Day:D2} {Hour:D2}:{Minute:D2}",
                _ => ToString()
            };
        }
        
        public override string ToString()
        {
            return $"{Year:D4}-{Month:D2}-{Day:D2} {Hour:D2}:{Minute:D2}";
        }
    }
    
    /// <summary>
    /// Money struct - perfect for financial calculations
    /// Demonstrates decimal precision and currency handling
    /// </summary>
    public readonly struct Money
    {
        public readonly decimal Amount;
        public readonly string Currency;
        
        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
        
        /// <summary>
        /// Add money (same currency only)
        /// </summary>
        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot add {Currency} and {other.Currency}");
            
            return new Money(Amount + other.Amount, Currency);
        }
        
        /// <summary>
        /// Subtract money (same currency only)
        /// </summary>
        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot subtract {Currency} and {other.Currency}");
            
            return new Money(Amount - other.Amount, Currency);
        }
        
        /// <summary>
        /// Multiply by scalar (for calculations like tax, discounts)
        /// </summary>
        public Money Multiply(decimal factor)
        {
            return new Money(Amount * factor, Currency);
        }
        
        /// <summary>
        /// Apply percentage (e.g., 10% tax = 0.10)
        /// </summary>
        public Money ApplyPercentage(decimal percentage)
        {
            return new Money(Amount * (1 + percentage), Currency);
        }
        
        // Operator overloading for convenience
        public static Money operator +(Money left, Money right)
        {
            return left.Add(right);
        }
        
        public static Money operator -(Money left, Money right)
        {
            return left.Subtract(right);
        }
        
        public static Money operator *(Money money, decimal factor)
        {
            return money.Multiply(factor);
        }
        
        public static bool operator >(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot compare different currencies");
            return left.Amount > right.Amount;
        }
        
        public static bool operator <(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot compare different currencies");
            return left.Amount < right.Amount;
        }
        
        public override string ToString()
        {
            return $"{Amount:F2} {Currency}";
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Money other)
                return Amount == other.Amount && Currency == other.Currency;
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
        
        public static bool operator ==(Money left, Money right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Money left, Money right)
        {
            return !left.Equals(right);
        }
    }
    
    /// <summary>
    /// Range struct - represents a range of values
    /// Similar to System.Range in .NET
    /// </summary>
    public readonly struct Range
    {
        public readonly int Start;
        public readonly int End;
        
        public Range(int start, int end)
        {
            if (start > end)
                throw new ArgumentException("Start cannot be greater than end");
            
            Start = start;
            End = end;
        }
        
        /// <summary>
        /// Length of the range
        /// </summary>
        public int Length => End - Start;
        
        /// <summary>
        /// Check if value is within range
        /// </summary>
        public bool Contains(int value)
        {
            return value >= Start && value <= End;
        }
        
        /// <summary>
        /// Check if ranges overlap
        /// </summary>
        public bool Overlaps(Range other)
        {
            return Start <= other.End && End >= other.Start;
        }
        
        /// <summary>
        /// Get intersection of two ranges
        /// </summary>
        public Range? Intersect(Range other)
        {
            if (!Overlaps(other))
                return null;
            
            int intersectStart = Math.Max(Start, other.Start);
            int intersectEnd = Math.Min(End, other.End);
            
            return new Range(intersectStart, intersectEnd);
        }
        
        public override string ToString()
        {
            return $"Range[{Start}..{End}] (Length: {Length})";
        }
    }
}
