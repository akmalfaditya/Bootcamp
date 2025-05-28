namespace NumericTypesDemo // Define namespace to organize our classes
{
    class Program // Main class that contains our demonstration methods
    {
        // Main method - entry point of the application
        static void Main()
        {
            // Display title for the demonstration
            Console.WriteLine("=== C# NUMERIC TYPES COMPREHENSIVE DEMONSTRATION ===\n");

            // Call each demonstration method to show different aspects of numeric types
            // Each method focuses on a specific concept from the material

            // 1. Show all integral types (whole numbers) - both signed and unsigned
            DemonstrateIntegralTypes();

            // 2. Show real types (numbers with decimal points) - float, double, decimal
            DemonstrateRealTypes();

            // 3. Show different ways to write numeric values (decimal, hex, binary)
            DemonstrateNumericLiterals();

            // 4. Show how C# automatically determines the type of numeric literals
            DemonstrateTypeInference();

            // 5. Show how suffixes (F, M, L, etc.) specify the exact type
            DemonstrateNumericSuffixes();

            // 6. Show how to convert between different numeric types
            DemonstrateNumericConversions();

            // 7. Show basic arithmetic operations (+, -, *, /, %)
            DemonstrateArithmeticOperators();

            // 8. Show increment (++) and decrement (--) operators
            DemonstrateIncrementDecrement();

            // 9. Show what happens when numbers get too large or small for their type
            DemonstrateOverflowUnderflow();

            // 10. Show real-world examples of using different numeric types
            DemonstratePracticalExamples();

            // End of demonstration
            Console.WriteLine("\n=== END OF DEMONSTRATION ===");
        }        // Method to demonstrate all integral (whole number) types in C#
        // Integral types store whole numbers without decimal points
        static void DemonstrateIntegralTypes()
        {
            Console.WriteLine("1. INTEGRAL TYPES (SIGNED AND UNSIGNED)");
            Console.WriteLine("========================================");

            // SIGNED INTEGRAL TYPES (can store positive and negative numbers)
            Console.WriteLine("Signed Integral Types:");
            
            // sbyte: smallest signed integer type (8 bits)
            // Range: -128 to 127 (can store negative numbers)
            sbyte sb = -128; // Using minimum value to show range
            
            // short: 16-bit signed integer
            // Range: -32,768 to 32,767
            short s = -32768; // Using minimum value
            
            // int: most commonly used integer type (32 bits)
            // Range: -2,147,483,648 to 2,147,483,647
            int i = -2147483648; // Using minimum value
            
            // long: largest standard integer type (64 bits)
            // Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
            // Note the 'L' suffix to indicate this is a long literal
            long l = -9223372036854775808L;

            // Display the values and their ranges
            Console.WriteLine($"sbyte: {sb} (Range: -128 to 127, Size: 8 bits)");
            Console.WriteLine($"short: {s} (Range: -32,768 to 32,767, Size: 16 bits)");
            Console.WriteLine($"int: {i} (Range: -2,147,483,648 to 2,147,483,647, Size: 32 bits)");
            Console.WriteLine($"long: {l} (Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807, Size: 64 bits)");

            // UNSIGNED INTEGRAL TYPES (can only store positive numbers, but larger positive range)
            Console.WriteLine("\nUnsigned Integral Types:");
            
            // byte: unsigned 8-bit integer (most common for binary data)
            // Range: 0 to 255 (no negative numbers, but twice the positive range of sbyte)
            byte b = 255; // Using maximum value
            
            // ushort: unsigned 16-bit integer
            // Range: 0 to 65,535
            ushort us = 65535; // Using maximum value
            
            // uint: unsigned 32-bit integer
            // Range: 0 to 4,294,967,295
            // Note the 'U' suffix to indicate this is an unsigned literal
            uint ui = 4294967295U;
            
            // ulong: unsigned 64-bit integer (largest standard integer type)
            // Range: 0 to 18,446,744,073,709,551,615
            // Note the 'UL' suffix (unsigned long)
            ulong ul = 18446744073709551615UL;

            // Display the unsigned values and their ranges
            Console.WriteLine($"byte: {b} (Range: 0 to 255, Size: 8 bits)");
            Console.WriteLine($"ushort: {us} (Range: 0 to 65,535, Size: 16 bits)");
            Console.WriteLine($"uint: {ui} (Range: 0 to 4,294,967,295, Size: 32 bits)");
            Console.WriteLine($"ulong: {ul} (Range: 0 to 18,446,744,073,709,551,615, Size: 64 bits)");

            // DEMONSTRATE MIN AND MAX VALUES using built-in constants
            // These constants are useful for boundary checking and validation
            Console.WriteLine("\nMin and Max Values:");
            Console.WriteLine($"int.MinValue: {int.MinValue}"); // Smallest possible int value
            Console.WriteLine($"int.MaxValue: {int.MaxValue}"); // Largest possible int value
            Console.WriteLine($"uint.MinValue: {uint.MinValue}"); // Always 0 for unsigned types
            Console.WriteLine($"uint.MaxValue: {uint.MaxValue}"); // Largest possible uint value

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate real number types (numbers with decimal points)
        // Real types are used for scientific calculations, measurements, and financial data
        static void DemonstrateRealTypes()
        {
            Console.WriteLine("2. REAL TYPES (FLOATING POINT AND DECIMAL)");
            Console.WriteLine("===========================================");

            // FLOAT TYPE (32-bit floating point)
            // Used for: Graphics, games, scientific calculations where speed > precision
            // Note the 'F' suffix - without it, C# would treat this as a double
            float f = 3.14159F;
            Console.WriteLine($"float: {f} (Size: 32 bits, Range: ±1.5 × 10^−45 to ±3.4 × 10^38)");
            // Demonstrate limited precision of float (about 7 decimal digits)
            Console.WriteLine($"float precision demo: {1.0F / 3.0F}");

            // DOUBLE TYPE (64-bit floating point)
            // Default type for decimal literals in C#
            // Used for: Most scientific and mathematical calculations
            double d = 3.141592653589793;
            Console.WriteLine($"double: {d} (Size: 64 bits, Range: ±5.0 × 10^−324 to ±1.7 × 10^308)");
            // Demonstrate higher precision of double (about 15-17 decimal digits)
            Console.WriteLine($"double precision demo: {1.0 / 3.0}");

            // DECIMAL TYPE (128-bit decimal)
            // Used for: Financial calculations where exact precision is required
            // Note the 'M' suffix - this tells C# it's a decimal, not double
            decimal m = 3.141592653589793238462643383M;
            Console.WriteLine($"decimal: {m} (Size: 128 bits, Range: ±1.0 × 10^−28 to ±7.9 × 10^28)");
            // Demonstrate highest precision of decimal (28-29 decimal digits)
            Console.WriteLine($"decimal precision demo: {1.0M / 3.0M}");

            // PRECISION COMPARISON
            // This shows why choosing the right type matters for accuracy
            Console.WriteLine("\nPrecision Comparison:");
            // Float has rounding errors due to binary representation
            Console.WriteLine($"float calculation: {0.1F + 0.2F}"); // May not equal exactly 0.3
            // Double has better precision but still uses binary floating point
            Console.WriteLine($"double calculation: {0.1 + 0.2}"); // Still may have tiny rounding error
            // Decimal uses decimal representation, so decimal arithmetic is exact
            Console.WriteLine($"decimal calculation: {0.1M + 0.2M}"); // Exactly 0.3

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate different ways to write numeric values
        // C# supports decimal, hexadecimal, binary notations and scientific notation
        static void DemonstrateNumericLiterals()
        {
            Console.WriteLine("3. NUMERIC LITERALS AND REPRESENTATIONS");
            Console.WriteLine("========================================");

            // DECIMAL NOTATION (Base 10) - most common way to write numbers
            // This is how we normally write numbers (0, 1, 2, 3, 4, 5, 6, 7, 8, 9)
            int decimal_notation = 127;
            Console.WriteLine($"Decimal notation: {decimal_notation}");

            // HEXADECIMAL NOTATION (Base 16) - uses 0-9 and A-F
            // Prefix with '0x' to indicate hexadecimal
            // Useful for memory addresses, colors, bit manipulation
            // 0x7F in hex = 7*16 + 15 = 127 in decimal
            long hex_notation = 0x7F;
            Console.WriteLine($"Hexadecimal notation (0x7F): {hex_notation}");

            // BINARY NOTATION (Base 2) - uses only 0 and 1
            // Prefix with '0b' to indicate binary
            // Useful for understanding bit patterns and flags
            // Underscores can be used to group digits for readability
            var binary_notation = 0b1010_1011_1100_1101;
            Console.WriteLine($"Binary notation (0b1010_1011_1100_1101): {binary_notation}");

            // USING UNDERSCORES FOR READABILITY
            // Underscores can be placed anywhere in numeric literals (except at start/end)
            // They don't affect the value, just make large numbers easier to read
            int million = 1_000_000; // Much easier to read than 1000000
            long billion = 1_000_000_000L; // The L suffix makes it a long
            Console.WriteLine($"Readable numbers: {million:N0} and {billion:N0}");

            // FLOATING-POINT LITERALS
            // Scientific notation uses 'E' or 'e' for "times 10 to the power of"
            double scientific_notation = 1E06; // 1 × 10^6 = 1,000,000
            double exponential = 2.5E-3; // 2.5 × 10^-3 = 0.0025
            Console.WriteLine($"Scientific notation (1E06): {scientific_notation:N0}");
            Console.WriteLine($"Exponential notation (2.5E-3): {exponential}");

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate how C# automatically determines the type of numeric literals
        // When you write a number, C# looks at its format to decide what type it should be
        static void DemonstrateTypeInference()
        {
            Console.WriteLine("4. NUMERIC LITERAL TYPE INFERENCE");
            Console.WriteLine("==================================");

            // TYPE INFERENCE EXAMPLES
            // C# uses the format and value of literals to determine their type
            
            // Any literal with a decimal point becomes 'double' by default
            Console.WriteLine($"1.0.GetType(): {1.0.GetType()}"); // System.Double
            
            // Scientific notation (with E) also becomes 'double'
            Console.WriteLine($"1E06.GetType(): {1E06.GetType()}"); // System.Double
            
            // Whole numbers without suffixes become the smallest type that can hold them
            // For most common values, this is 'int' (Int32)
            Console.WriteLine($"1.GetType(): {1.GetType()}"); // System.Int32 (int)
            
            // Large hexadecimal values may become 'uint' if they exceed int range
            // 0xF0000000 = 4,026,531,840 which is larger than int.MaxValue
            Console.WriteLine($"0xF0000000.GetType(): {0xF0000000.GetType()}"); // System.UInt32 (uint)

            // AUTOMATIC TYPE SELECTION based on value size
            // C# picks the smallest type that can hold the value
            var smallInt = 100; // Fits in int, so becomes int
            var largeInt = 3000000000; // Too large for int, becomes uint
            Console.WriteLine($"var smallInt = 100; Type: {smallInt.GetType()}");
            Console.WriteLine($"var largeInt = 3000000000; Type: {largeInt.GetType()}");

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate numeric suffixes that explicitly specify the type
        // Suffixes override C#'s default type inference
        static void DemonstrateNumericSuffixes()
        {
            Console.WriteLine("5. NUMERIC SUFFIXES");
            Console.WriteLine("===================");

            // FLOAT SUFFIX (F or f)
            // Without the F suffix, 1.0 would be treated as double
            // The suffix forces it to be float
            float f1 = 1.0F;
            Console.WriteLine($"1.0F is type: {f1.GetType()}");

            // DOUBLE SUFFIX (D or d) - optional since double is default for decimals
            // This suffix is rarely used because decimals are double by default
            double d1 = 1.0D;
            Console.WriteLine($"1.0D is type: {d1.GetType()}");

            // DECIMAL SUFFIX (M or m) - required for decimal literals
            // Without the M suffix, C# would treat this as double and cause compile error
            decimal m1 = 1.0M;
            Console.WriteLine($"1.0M is type: {m1.GetType()}");

            // UNSIGNED SUFFIX (U or u) - forces integer to be unsigned
            // Without suffix, large values might be interpreted as long instead of uint
            uint ui1 = 1000U;
            Console.WriteLine($"1000U is type: {ui1.GetType()}");

            // LONG SUFFIX (L or l) - forces integer to be long
            // Lowercase 'l' looks like '1', so uppercase 'L' is preferred
            long l1 = 1000L;
            Console.WriteLine($"1000L is type: {l1.GetType()}");

            // UNSIGNED LONG SUFFIX (UL or ul) - forces to unsigned long
            // Combines both unsigned (U) and long (L) suffixes
            ulong ul1 = 1000UL;
            Console.WriteLine($"1000UL is type: {ul1.GetType()}");

            // DEMONSTRATION: What happens without suffixes
            // C# uses its default inference rules
            var defaultFloat = 1.0; // Becomes double (not float!)
            var defaultInt = 1000; // Becomes int
            Console.WriteLine($"1.0 (no suffix) is type: {defaultFloat.GetType()}");
            Console.WriteLine($"1000 (no suffix) is type: {defaultInt.GetType()}");

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate how to convert between different numeric types
        // Conversions can be either implicit (automatic) or explicit (manual casting)
        static void DemonstrateNumericConversions()
        {
            Console.WriteLine("6. NUMERIC CONVERSIONS");
            Console.WriteLine("======================");

            // IMPLICIT CONVERSIONS (Safe - no data loss possible)
            // These happen automatically when converting to a larger type
            Console.WriteLine("Implicit Conversions (Safe):");
            
            int x = 12345;
            // int to long is always safe because long can hold all int values
            long y = x; // Automatic conversion - no casting needed
            Console.WriteLine($"int {x} → long {y}");

            byte b = 100;
            // byte to int is always safe because int can hold all byte values
            int i = b; // Automatic conversion
            Console.WriteLine($"byte {b} → int {i}");

            // EXPLICIT CONVERSIONS (Potentially unsafe - may lose data)
            // These require manual casting because data might be lost
            Console.WriteLine("\nExplicit Conversions (Potential Data Loss):");
            
            long bigNumber = 123456789L;
            // long to int might lose data if the long value is too large
            int smallerNumber = (int)bigNumber; // Manual cast required
            Console.WriteLine($"long {bigNumber} → int {smallerNumber}");

            // CONVERSION WITH ACTUAL DATA LOSS
            // This demonstrates what happens when the source value is too large
            long tooLarge = 5000000000L; // This exceeds int.MaxValue (2.1 billion)
            int truncated = (int)tooLarge; // Data will be lost!
            Console.WriteLine($"long {tooLarge} → int {truncated} (Data loss occurred!)");

            // FLOATING-POINT TO INTEGRAL CONVERSIONS
            Console.WriteLine("\nFloating-Point to Integral Conversions:");
            
            int intValue = 1;
            // int to float is implicit (though precision might be lost for very large ints)
            float floatValue = intValue;
            Console.WriteLine($"int {intValue} → float {floatValue}");

            double doubleValue = 5.9;
            // double to int requires explicit cast and TRUNCATES (doesn't round!)
            int truncatedInt = (int)doubleValue; // 5.9 becomes 5, not 6
            Console.WriteLine($"double {doubleValue} → int {truncatedInt} (Fraction truncated)");

            // DECIMAL CONVERSIONS
            // decimal has special conversion rules
            Console.WriteLine("\nDecimal Conversions:");
            
            int intForDecimal = 100;
            // All integral types can be implicitly converted to decimal
            decimal decimalFromInt = intForDecimal;
            Console.WriteLine($"int {intForDecimal} → decimal {decimalFromInt}");

            decimal decimalValue = 123.45M;
            // decimal to integral types requires explicit cast
            int intFromDecimal = (int)decimalValue; // Fraction is truncated
            Console.WriteLine($"decimal {decimalValue} → int {intFromDecimal}");

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate basic arithmetic operations in C#
        // Shows how different operators work and special behaviors with small integer types
        static void DemonstrateArithmeticOperators()
        {
            Console.WriteLine("7. ARITHMETIC OPERATORS");
            Console.WriteLine("========================");

            // BASIC ARITHMETIC OPERATIONS
            // These are the fundamental mathematical operations
            int a = 10, b = 3;
            Console.WriteLine($"a = {a}, b = {b}");
            
            // Addition: combines two numbers
            Console.WriteLine($"Addition (a + b): {a + b}");
            
            // Subtraction: finds the difference between numbers
            Console.WriteLine($"Subtraction (a - b): {a - b}");
            
            // Multiplication: repeated addition
            Console.WriteLine($"Multiplication (a * b): {a * b}");
            
            // Division: how many times one number fits into another
            Console.WriteLine($"Division (a / b): {a / b}"); // Integer division - result is 3, not 3.33
            
            // Modulo (remainder): what's left after division
            Console.WriteLine($"Remainder/Modulo (a % b): {a % b}"); // 10 ÷ 3 = 3 remainder 1

            // FLOATING-POINT VS INTEGER DIVISION
            // Important difference between integer and floating-point division
            double da = 10.0, db = 3.0;
            Console.WriteLine($"\nFloating-point division: {da} / {db} = {da / db}");

            // Compare integer vs floating-point division results
            Console.WriteLine($"Integer division: {10} / {3} = {10 / 3}"); // Truncates to 3
            Console.WriteLine($"Floating-point division: {10.0} / {3.0} = {10.0 / 3.0}"); // True result

            // SPECIAL BEHAVIOR WITH SMALL INTEGER TYPES
            // byte, sbyte, short, ushort are promoted to int before arithmetic
            Console.WriteLine("\n8-bit and 16-bit arithmetic (auto-promoted to int):");
            byte b1 = 100, b2 = 50;
            // Even though we're adding bytes, the result type is int!
            var result = b1 + b2; // result is int, not byte
            Console.WriteLine($"byte {b1} + byte {b2} = {result} (type: {result.GetType()})");

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate increment (++) and decrement (--) operators
        // These operators add or subtract 1 from a variable
        static void DemonstrateIncrementDecrement()
        {
            Console.WriteLine("8. INCREMENT AND DECREMENT OPERATORS");
            Console.WriteLine("=====================================");

            int x = 5, y = 5;
            Console.WriteLine($"Initial values: x = {x}, y = {y}");

            // POSTFIX OPERATORS (variable++ and variable--)
            // Return the current value, THEN modify the variable
            Console.WriteLine("\nPostfix operators:");
            // x++ returns the current value of x (5), then increments x to 6
            Console.WriteLine($"x++ returns: {x++}, x is now: {x}");
            // y-- returns the current value of y (5), then decrements y to 4
            Console.WriteLine($"y-- returns: {y--}, y is now: {y}");

            // Reset values for next demonstration
            x = 5; y = 5;
            Console.WriteLine($"\nReset values: x = {x}, y = {y}");

            // PREFIX OPERATORS (++variable and --variable)
            // Modify the variable FIRST, then return the new value
            Console.WriteLine("\nPrefix operators:");
            // ++x increments x to 6 first, then returns the new value (6)
            Console.WriteLine($"++x returns: {++x}, x is now: {x}");
            // --y decrements y to 4 first, then returns the new value (4)
            Console.WriteLine($"--y returns: {--y}, y is now: {y}");

            // PRACTICAL EXAMPLE showing the difference in a loop context
            Console.WriteLine("\nPractical example in a loop:");
            int counter = 0;
            for (int i = 0; i < 3; i++) // i++ is postfix here
            {
                // ++counter is prefix here - increment before using the value
                Console.WriteLine($"Loop iteration {i + 1}, counter before increment: {counter}, after: {++counter}");
            }

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate overflow and underflow behavior
        // Shows what happens when arithmetic results exceed the type's range
        static void DemonstrateOverflowUnderflow()
        {
            Console.WriteLine("9. OVERFLOW AND UNDERFLOW");
            Console.WriteLine("==========================");

            // DEFAULT BEHAVIOR (No exception thrown)
            // By default, C# allows overflow and the result "wraps around"
            Console.WriteLine("Default behavior (overflow wraps around):");
            int maxInt = int.MaxValue; // 2,147,483,647
            Console.WriteLine($"int.MaxValue: {maxInt}");
            // Adding 1 to the maximum value wraps to the minimum value
            int overflowed = maxInt + 1; // Becomes -2,147,483,648 (int.MinValue)
            Console.WriteLine($"int.MaxValue + 1: {overflowed} (wrapped to minimum value)");

            // UNCHECKED CONTEXT (Explicit permission for overflow)
            // The unchecked keyword explicitly allows overflow behavior
            Console.WriteLine("\nUnchecked context:");
            int uncheckedResult = unchecked(int.MaxValue + 1);
            Console.WriteLine($"unchecked(int.MaxValue + 1): {uncheckedResult}");

            // CHECKED CONTEXT (Throws exception on overflow)
            // The checked keyword makes overflow throw an OverflowException
            Console.WriteLine("\nChecked context (will throw OverflowException):");
            try
            {
                // This will throw an OverflowException
                int checkedResult = checked(int.MaxValue + 1);
                Console.WriteLine($"This line won't be reached: {checkedResult}");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"OverflowException caught: {ex.Message}");
            }

            // UNDERFLOW EXAMPLE (going below minimum value)
            // Similar to overflow, but going below the minimum value
            Console.WriteLine("\nUnderflow example:");
            uint minUint = 0; // uint.MinValue is always 0
            Console.WriteLine($"uint.MinValue: {minUint}");
            
            // Demonstrate underflow by subtracting from minimum value
            uint underflowed = 0;
            unchecked // Use unchecked block to avoid compile-time error
            {
                underflowed = minUint - 1; // 0 - 1 wraps to uint.MaxValue
            }
            Console.WriteLine($"uint.MinValue - 1 (unchecked): {underflowed} (wrapped to maximum value)");

            Console.WriteLine(); // Add spacing for readability
        }        // Method to demonstrate real-world applications of different numeric types
        // Shows when to use each type and provides practical scenarios
        static void DemonstratePracticalExamples()
        {
            Console.WriteLine("10. PRACTICAL EXAMPLES");
            Console.WriteLine("=======================");

            // FINANCIAL CALCULATION USING DECIMAL
            // Use decimal for money calculations to avoid rounding errors
            Console.WriteLine("Financial Calculation (using decimal for precision):");
            decimal price = 19.99M; // Price in dollars
            decimal tax = 0.08M; // 8% tax rate
            decimal total = price * (1 + tax); // Calculate total with tax
            Console.WriteLine($"Price: ${price:F2}"); // Format as currency
            Console.WriteLine($"Tax: {tax:P0}"); // Format as percentage
            Console.WriteLine($"Total: ${total:F2}"); // Exact result without rounding errors

            // SCIENTIFIC CALCULATION USING DOUBLE
            // Use double for mathematical and scientific calculations
            Console.WriteLine("\nScientific Calculation (using double):");
            double radius = 5.0; // Circle radius in units
            double area = Math.PI * radius * radius; // Calculate area using π
            Console.WriteLine($"Circle radius: {radius}");
            Console.WriteLine($"Circle area: {area:F6}"); // Show 6 decimal places

            // PERFORMANCE COUNTER USING LONG
            // Use long for large numbers like timestamps or counters
            Console.WriteLine("\nPerformance Counter (using long for large numbers):");
            long ticks = DateTime.Now.Ticks; // Ticks since January 1, 0001
            Console.WriteLine($"Current ticks: {ticks:N0}"); // Format with thousands separators

            // MEMORY SIZE CALCULATION
            // Demonstrates when different integer types are appropriate
            Console.WriteLine("\nMemory Size Calculation:");
            int kilobyte = 1024; // 1 KB = 1024 bytes
            int megabyte = kilobyte * kilobyte; // 1 MB = 1024 KB
            // Need long for GB because the result exceeds int range
            long gigabyte = (long)megabyte * kilobyte; // Cast to prevent overflow
            Console.WriteLine($"1 KB = {kilobyte:N0} bytes");
            Console.WriteLine($"1 MB = {megabyte:N0} bytes");
            Console.WriteLine($"1 GB = {gigabyte:N0} bytes");

            // TEMPERATURE CONVERSION
            // Common mathematical formula using floating-point arithmetic
            Console.WriteLine("\nTemperature Conversion:");
            double celsius = 25.0; // Temperature in Celsius
            // Formula: F = (C × 9/5) + 32
            double fahrenheit = (celsius * 9.0 / 5.0) + 32.0;
            Console.WriteLine($"{celsius}°C = {fahrenheit}°F");

            // BITWISE OPERATIONS WITH INTEGRAL TYPES
            // Demonstrates working with individual bits using byte type
            Console.WriteLine("\nBitwise Operations:");
            byte flags = 0b0000_1010; // Binary representation: 10 in decimal
            Console.WriteLine($"Original flags: {flags} (binary: {Convert.ToString(flags, 2).PadLeft(8, '0')})");
            
            // Use OR operation to set additional bits
            flags |= 0b0000_0101; // Set bits at positions 0 and 2
            Console.WriteLine($"After OR operation: {flags} (binary: {Convert.ToString(flags, 2).PadLeft(8, '0')})");

            Console.WriteLine(); // Add spacing for readability
        }
    }
}
