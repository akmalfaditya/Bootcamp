using System.Numerics;
using System.Security.Cryptography;

// Working with Numbers Demonstration
// This project covers numeric conversions, mathematical operations, and advanced number handling

namespace WorkingWithNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== WORKING WITH NUMBERS DEMONSTRATION ===\n");

            // Let's explore all aspects of number handling in C#
            DemonstrateNumericConversions();
            DemonstrateBaseNumberParsing();
            DemonstrateLosslessConversions();
            DemonstrateRoundingConversions();
            DemonstrateMathClass();
            DemonstrateBigInteger();
            DemonstrateComplexNumbers();
            DemonstrateRandomNumbers();
            DemonstrateBitOperations();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== END OF DEMONSTRATION ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DemonstrateNumericConversions()
        {
            Console.WriteLine("1. NUMERIC CONVERSIONS - SAFE AND UNSAFE");
            Console.WriteLine("=========================================");

            // Basic parsing operations - the foundation of handling user input
            Console.WriteLine("Basic string to number parsing:");

            string[] numberStrings = { "42", "3.14159", "-123", "0", "999999999" };
            
            foreach (string numberStr in numberStrings)
            {
                Console.WriteLine($"\nParsing \"{numberStr}\":");
                
                // Safe parsing with TryParse - always prefer this for user input
                if (int.TryParse(numberStr, out int intResult))
                {
                    Console.WriteLine($"  ✓ As int: {intResult}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Cannot parse as int");
                }

                if (double.TryParse(numberStr, out double doubleResult))
                {
                    Console.WriteLine($"  ✓ As double: {doubleResult}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Cannot parse as double");
                }

                if (decimal.TryParse(numberStr, out decimal decimalResult))
                {
                    Console.WriteLine($"  ✓ As decimal: {decimalResult}");
                }
            }

            // Demonstrating the difference between Parse and TryParse
            Console.WriteLine("\nDifference between Parse() and TryParse():");
            
            string invalidNumber = "not_a_number";
            
            // TryParse approach - safe, no exceptions
            if (int.TryParse(invalidNumber, out int safeResult))
            {
                Console.WriteLine($"  TryParse succeeded: {safeResult}");
            }
            else
            {
                Console.WriteLine($"  TryParse failed gracefully for \"{invalidNumber}\"");
            }

            // Parse approach - can throw exceptions
            try
            {
                int unsafeResult = int.Parse(invalidNumber);
                Console.WriteLine($"  Parse succeeded: {unsafeResult}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"  Parse threw exception: {ex.Message}");
            }

            Console.WriteLine();
        }

        static void DemonstrateBaseNumberParsing()
        {
            Console.WriteLine("2. BASE NUMBER PARSING - BINARY, OCTAL, HEXADECIMAL");
            Console.WriteLine("====================================================");

            // Working with different number bases is crucial for systems programming
            Console.WriteLine("Parsing numbers in different bases:");

            // Hexadecimal parsing - common in color codes, memory addresses
            string[] hexNumbers = { "FF", "1E", "A0", "DEADBEEF" };
            
            Console.WriteLine("\nHexadecimal numbers:");
            foreach (string hex in hexNumbers)
            {
                try
                {
                    if (hex.Length <= 8) // Standard int range
                    {
                        int hexValue = Convert.ToInt32(hex, 16);
                        Console.WriteLine($"  0x{hex} -> {hexValue} (decimal)");
                    }
                    else // Large hex values
                    {
                        long hexValue = Convert.ToInt64(hex, 16);
                        Console.WriteLine($"  0x{hex} -> {hexValue} (decimal)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  0x{hex} -> Error: {ex.Message}");
                }
            }

            // Binary parsing - useful for flags and bit manipulation
            string[] binaryNumbers = { "1010", "11110000", "101010101010" };
            
            Console.WriteLine("\nBinary numbers:");
            foreach (string binary in binaryNumbers)
            {
                try
                {
                    int binaryValue = Convert.ToInt32(binary, 2);
                    Console.WriteLine($"  {binary} (binary) -> {binaryValue} (decimal)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  {binary} -> Error: {ex.Message}");
                }
            }

            // Octal parsing - less common but still used in Unix permissions
            string[] octalNumbers = { "755", "644", "777" };
            
            Console.WriteLine("\nOctal numbers (Unix permissions example):");
            foreach (string octal in octalNumbers)
            {
                int octalValue = Convert.ToInt32(octal, 8);
                Console.WriteLine($"  {octal} (octal) -> {octalValue} (decimal) -> {ConvertToPermissionString(octalValue)}");
            }

            // Converting numbers back to different bases
            Console.WriteLine("\nConverting decimal to different bases:");
            int decimalNumber = 255;
            
            Console.WriteLine($"  Decimal {decimalNumber}:");
            Console.WriteLine($"    Binary: {Convert.ToString(decimalNumber, 2)}");
            Console.WriteLine($"    Octal: {Convert.ToString(decimalNumber, 8)}");
            Console.WriteLine($"    Hexadecimal: {Convert.ToString(decimalNumber, 16).ToUpper()}");

            // Formatting numbers to hexadecimal with ToString
            Console.WriteLine("\nFormatting to hexadecimal:");
            int[] numbers = { 15, 255, 4095 };
            
            foreach (int num in numbers)
            {
                Console.WriteLine($"  {num} -> 0x{num:X} (uppercase) or 0x{num:x} (lowercase)");
            }

            Console.WriteLine();
        }

        static void DemonstrateLosslessConversions()
        {
            Console.WriteLine("3. LOSSLESS CONVERSIONS - IMPLICIT VS EXPLICIT");
            Console.WriteLine("==============================================");

            // Understanding when data loss can occur is crucial for reliable applications
            Console.WriteLine("Implicit conversions (no data loss):");

            // These conversions are safe - smaller to larger type
            byte byteValue = 100;
            short shortValue = byteValue;      // byte -> short (safe)
            int intValue = shortValue;         // short -> int (safe)
            long longValue = intValue;         // int -> long (safe)
            float floatValue = longValue;      // long -> float (safe for most values)
            double doubleValue = floatValue;   // float -> double (safe)

            Console.WriteLine($"  byte {byteValue} -> short {shortValue} -> int {intValue}");
            Console.WriteLine($"  -> long {longValue} -> float {floatValue} -> double {doubleValue}");

            // Explicit conversions (potential data loss)
            Console.WriteLine("\nExplicit conversions (potential data loss):");

            double largeDouble = 123.456789;
            float fromDouble = (float)largeDouble;      // May lose precision
            int fromFloat = (int)fromDouble;            // Loses decimal part
            short fromInt = (short)fromFloat;           // May lose data if too large
            byte fromShort = (byte)fromInt;             // May lose data if too large

            Console.WriteLine($"  double {largeDouble} -> float {fromDouble}");
            Console.WriteLine($"  -> int {fromInt} -> short {fromShort} -> byte {fromShort}");

            // Demonstrating precision loss
            Console.WriteLine("\nPrecision loss examples:");
            
            double preciseValue = 1.23456789012345;
            float lessePrecise = (float)preciseValue;
            
            Console.WriteLine($"  Original double: {preciseValue:F14}");
            Console.WriteLine($"  As float: {lessePrecise:F14}");
            Console.WriteLine($"  Precision lost: {Math.Abs(preciseValue - lessePrecise):E3}");

            // Overflow examples
            Console.WriteLine("\nOverflow examples:");
            
            try
            {
                int maxInt = int.MaxValue;
                Console.WriteLine($"  int.MaxValue: {maxInt:N0}");
                
                // This will overflow silently unless checked
                checked
                {
                    int overflow = maxInt + 1;
                    Console.WriteLine($"  MaxValue + 1: {overflow}");
                }
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"  Overflow detected: {ex.Message}");
            }

            Console.WriteLine();
        }

        static void DemonstrateRoundingConversions()
        {
            Console.WriteLine("4. ROUNDING CONVERSIONS - REAL TO INTEGRAL");
            Console.WriteLine("==========================================");

            // Different rounding strategies can significantly impact calculations
            double[] testValues = { 2.3, 2.5, 2.7, 3.5, 4.5, -2.5, -3.5 };

            Console.WriteLine("Comparing different rounding methods:");
            Console.WriteLine("Value     Truncate  Convert   Round     Floor     Ceiling");
            Console.WriteLine("-----     --------  -------   -----     -----     -------");

            foreach (double value in testValues)
            {
                int truncated = (int)value;                                    // Simple cast truncates
                int converted = Convert.ToInt32(value);                        // Banker's rounding
                int rounded = (int)Math.Round(value, MidpointRounding.AwayFromZero); // Round away from zero
                int floored = (int)Math.Floor(value);                          // Always round down
                int ceiled = (int)Math.Ceiling(value);                         // Always round up

                Console.WriteLine($"{value,5:F1}     {truncated,8}  {converted,7}   {rounded,5}     {floored,5}     {ceiled,7}");
            }

            // Banker's rounding explanation
            Console.WriteLine("\nBanker's rounding (used by Convert.ToInt32):");
            Console.WriteLine("  - Rounds .5 to the nearest even number");
            Console.WriteLine("  - Reduces bias in large datasets");
            Console.WriteLine("  - 2.5 -> 2, 3.5 -> 4, 4.5 -> 4, 5.5 -> 6");

            // Practical example: Currency calculations
            Console.WriteLine("\nPractical example - Currency rounding:");
            
            double[] prices = { 19.995, 29.985, 15.125, 8.875 };
            
            Console.WriteLine("Original   Truncate   Banker's   Away from Zero");
            Console.WriteLine("--------   --------   --------   --------------");
            
            foreach (double price in prices)
            {
                double truncated = Math.Truncate(price * 100) / 100;
                double bankers = Math.Round(price, 2, MidpointRounding.ToEven);
                double awayFromZero = Math.Round(price, 2, MidpointRounding.AwayFromZero);
                
                Console.WriteLine($"${price:F3}     ${truncated:F2}      ${bankers:F2}       ${awayFromZero:F2}");
            }

            Console.WriteLine();
        }

        static void DemonstrateMathClass()
        {
            Console.WriteLine("5. MATH CLASS - MATHEMATICAL OPERATIONS");
            Console.WriteLine("=======================================");

            // The Math class is your toolbox for mathematical calculations
            Console.WriteLine("Basic Math operations:");

            double[] values = { -10.7, -5.5, 0, 5.5, 10.7 };
            
            Console.WriteLine("Value     Abs       Floor     Ceiling   Round     Sign");
            Console.WriteLine("-----     ---       -----     -------   -----     ----");
            
            foreach (double value in values)
            {
                Console.WriteLine($"{value,5:F1}     {Math.Abs(value),3:F1}       {Math.Floor(value),5}     {Math.Ceiling(value),7}   {Math.Round(value),5}     {Math.Sign(value),4}");
            }

            // Min/Max operations
            Console.WriteLine("\nMin/Max operations:");
            int[] numbers = { 15, 8, 23, 4, 42, 16 };
            
            Console.WriteLine($"  Numbers: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"  Min: {numbers.Min()}");
            Console.WriteLine($"  Max: {numbers.Max()}");
            Console.WriteLine($"  Math.Min(15, 8): {Math.Min(15, 8)}");
            Console.WriteLine($"  Math.Max(15, 8): {Math.Max(15, 8)}");

            // Power and root operations
            Console.WriteLine("\nPower and root operations:");
            
            double baseNumber = 2;
            double[] exponents = { 2, 3, 0.5, -1 };
            
            foreach (double exp in exponents)
            {
                double result = Math.Pow(baseNumber, exp);
                string description = exp switch
                {
                    2 => "(squared)",
                    3 => "(cubed)",
                    0.5 => "(square root)",
                    -1 => "(reciprocal)",
                    _ => ""
                };
                Console.WriteLine($"  {baseNumber}^{exp} = {result:F3} {description}");
            }

            Console.WriteLine($"  Square root of 16: {Math.Sqrt(16)}");
            Console.WriteLine($"  Cube root of 27: {Math.Pow(27, 1.0/3):F3}");

            // Logarithmic operations
            Console.WriteLine("\nLogarithmic operations:");
            
            double[] logValues = { 1, 10, 100, Math.E };
            
            foreach (double val in logValues)
            {
                Console.WriteLine($"  ln({val:F3}) = {Math.Log(val):F3}");
                if (val > 0)
                {
                    Console.WriteLine($"  log10({val:F3}) = {Math.Log10(val):F3}");
                }
            }

            // Trigonometric operations
            Console.WriteLine("\nTrigonometric operations (angles in radians):");
            
            double[] angles = { 0, Math.PI / 6, Math.PI / 4, Math.PI / 3, Math.PI / 2 };
            string[] angleNames = { "0°", "30°", "45°", "60°", "90°" };
            
            for (int i = 0; i < angles.Length; i++)
            {
                double angle = angles[i];
                Console.WriteLine($"  {angleNames[i],3}: sin={Math.Sin(angle):F3}, cos={Math.Cos(angle):F3}, tan={Math.Tan(angle):F3}");
            }

            // Practical example: Distance calculation
            Console.WriteLine("\nPractical example - Distance between two points:");
            
            (double x1, double y1) = (3, 4);
            (double x2, double y2) = (6, 8);
            
            double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            Console.WriteLine($"  Point 1: ({x1}, {y1})");
            Console.WriteLine($"  Point 2: ({x2}, {y2})");
            Console.WriteLine($"  Distance: {distance:F2}");

            Console.WriteLine();
        }

        static void DemonstrateBigInteger()
        {
            Console.WriteLine("6. BIGINTEGER - ARBITRARILY LARGE NUMBERS");
            Console.WriteLine("==========================================");

            // BigInteger handles numbers larger than long.MaxValue
            Console.WriteLine("Working with very large numbers:");

            // Creating BigInteger values
            BigInteger small = new BigInteger(123456789);
            BigInteger fromString = BigInteger.Parse("987654321012345678901234567890");
            BigInteger googol = BigInteger.Pow(10, 100); // 10^100
            
            Console.WriteLine($"  Small BigInteger: {small:N0}");
            Console.WriteLine($"  From string: {fromString}");
            Console.WriteLine($"  Googol (10^100): {googol}");

            // Arithmetic operations with BigInteger
            Console.WriteLine("\nBigInteger arithmetic:");
            
            BigInteger a = BigInteger.Parse("12345678901234567890");
            BigInteger b = BigInteger.Parse("98765432109876543210");
            
            Console.WriteLine($"  a = {a}");
            Console.WriteLine($"  b = {b}");
            Console.WriteLine($"  a + b = {a + b}");
            Console.WriteLine($"  a * b = {a * b}");
            Console.WriteLine($"  a^2 = {BigInteger.Pow(a, 2)}");

            // Factorials - impossible with regular integers for large values
            Console.WriteLine("\nFactorial calculations:");
            
            for (int n = 10; n <= 50; n += 10)
            {
                BigInteger factorial = CalculateFactorial(n);
                Console.WriteLine($"  {n}! = {factorial}");
                Console.WriteLine($"       ({factorial.ToString().Length} digits)");
            }

            // Fibonacci sequence with large numbers
            Console.WriteLine("\nLarge Fibonacci numbers:");
            
            BigInteger fib1 = 0, fib2 = 1;
            Console.WriteLine($"  F(0) = {fib1}");
            Console.WriteLine($"  F(1) = {fib2}");
            
            for (int i = 2; i <= 100; i += 10)
            {
                for (int j = (i == 2 ? 2 : i - 8); j <= i; j++)
                {
                    BigInteger fibNext = fib1 + fib2;
                    fib1 = fib2;
                    fib2 = fibNext;
                }
                Console.WriteLine($"  F({i}) = {fib2}");
            }

            // Precision loss when converting to double
            Console.WriteLine("\nPrecision loss with double conversion:");
            
            BigInteger hugNumber = BigInteger.Pow(2, 100);
            double asDouble = (double)hugNumber;
            BigInteger backToBig = (BigInteger)asDouble;
            
            Console.WriteLine($"  Original: {hugNumber}");
            Console.WriteLine($"  As double: {asDouble:E3}");
            Console.WriteLine($"  Back to BigInteger: {backToBig}");
            Console.WriteLine($"  Precision lost: {hugNumber != backToBig}");

            Console.WriteLine();
        }

        static void DemonstrateComplexNumbers()
        {
            Console.WriteLine("7. COMPLEX NUMBERS - REAL AND IMAGINARY PARTS");
            Console.WriteLine("==============================================");

            // Complex numbers are essential for advanced mathematics and engineering
            Console.WriteLine("Creating and working with complex numbers:");

            // Creating complex numbers
            Complex c1 = new Complex(3, 4);        // 3 + 4i
            Complex c2 = new Complex(1, -2);       // 1 - 2i
            Complex real = new Complex(5, 0);      // Real number
            Complex imaginary = new Complex(0, 3); // Pure imaginary number

            Console.WriteLine($"  c1 = {c1}");
            Console.WriteLine($"  c2 = {c2}");
            Console.WriteLine($"  real = {real}");
            Console.WriteLine($"  imaginary = {imaginary}");

            // Properties of complex numbers
            Console.WriteLine("\nComplex number properties:");
            
            Console.WriteLine($"  c1 real part: {c1.Real}");
            Console.WriteLine($"  c1 imaginary part: {c1.Imaginary}");
            Console.WriteLine($"  c1 magnitude: {c1.Magnitude:F3}");
            Console.WriteLine($"  c1 phase (radians): {c1.Phase:F3}");
            Console.WriteLine($"  c1 phase (degrees): {c1.Phase * 180 / Math.PI:F1}°");

            // Complex arithmetic
            Console.WriteLine("\nComplex arithmetic:");
            
            Complex sum = c1 + c2;
            Complex difference = c1 - c2;
            Complex product = c1 * c2;
            Complex quotient = c1 / c2;
            
            Console.WriteLine($"  {c1} + {c2} = {sum}");
            Console.WriteLine($"  {c1} - {c2} = {difference}");
            Console.WriteLine($"  {c1} * {c2} = {product}");
            Console.WriteLine($"  {c1} / {c2} = {quotient}");

            // Complex conjugate and other operations
            Console.WriteLine("\nAdvanced complex operations:");
            
            Complex conjugate = Complex.Conjugate(c1);
            Complex reciprocal = Complex.Reciprocal(c1);
            
            Console.WriteLine($"  Conjugate of {c1}: {conjugate}");
            Console.WriteLine($"  Reciprocal of {c1}: {reciprocal}");
            Console.WriteLine($"  |{c1}|² = {c1.Magnitude * c1.Magnitude:F3}");
            Console.WriteLine($"  {c1} * conjugate = {c1 * conjugate}");

            // Practical example: AC circuit analysis (simplified)
            Console.WriteLine("\nPractical example - AC circuit impedance:");
            
            // Impedance = Resistance + j*Reactance
            Complex impedance1 = new Complex(100, 50);  // 100Ω resistance, 50Ω inductive reactance
            Complex impedance2 = new Complex(75, -25);  // 75Ω resistance, 25Ω capacitive reactance
            
            Complex totalImpedance = impedance1 + impedance2;
            
            Console.WriteLine($"  Z1 = {impedance1} Ω");
            Console.WriteLine($"  Z2 = {impedance2} Ω");
            Console.WriteLine($"  Total impedance = {totalImpedance} Ω");
            Console.WriteLine($"  Magnitude = {totalImpedance.Magnitude:F1} Ω");
            Console.WriteLine($"  Phase = {totalImpedance.Phase * 180 / Math.PI:F1}°");

            Console.WriteLine();
        }

        static void DemonstrateRandomNumbers()
        {
            Console.WriteLine("8. RANDOM NUMBERS - PSEUDORANDOM AND CRYPTOGRAPHIC");
            Console.WriteLine("==================================================");

            // Random numbers are crucial for simulations, games, and security
            Console.WriteLine("Basic Random class usage:");

            Random random = new Random(42); // Fixed seed for reproducible results
            
            Console.WriteLine("  Random integers:");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"    Next(): {random.Next()}");
                Console.WriteLine($"    Next(100): {random.Next(100)}");
                Console.WriteLine($"    Next(10, 50): {random.Next(10, 50)}");
            }

            Console.WriteLine("\n  Random floating-point numbers:");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"    NextDouble(): {random.NextDouble():F4}");
                Console.WriteLine($"    Range [5.0, 10.0): {random.NextDouble() * 5 + 5:F2}");
            }

            // Random bytes
            Console.WriteLine("\n  Random bytes:");
            byte[] randomBytes = new byte[10];
            random.NextBytes(randomBytes);
            Console.WriteLine($"    Byte array: [{string.Join(", ", randomBytes)}]");

            // Demonstrating the importance of seed
            Console.WriteLine("\nSeed importance:");
            
            Random seeded1 = new Random(123);
            Random seeded2 = new Random(123);
            Random unseeded = new Random();
            
            Console.WriteLine("  Same seed produces same sequence:");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"    Seeded1: {seeded1.Next(100),3}, Seeded2: {seeded2.Next(100),3}, Unseeded: {unseeded.Next(100),3}");
            }

            // Practical examples
            Console.WriteLine("\nPractical examples:");
            
            // Dice simulation
            Console.WriteLine("  Dice roll simulation (6-sided die, 1000 rolls):");
            Random diceRandom = new Random();
            int[] diceCounts = new int[6];
            
            for (int i = 0; i < 1000; i++)
            {
                int roll = diceRandom.Next(1, 7);
                diceCounts[roll - 1]++;
            }
            
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine($"    {i + 1}: {diceCounts[i],3} times ({diceCounts[i] / 10.0:F1}%)");
            }

            // Random sampling from a list
            Console.WriteLine("\n  Random sampling from a list:");
            string[] names = { "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank" };
            
            Console.WriteLine("    Random names:");
            for (int i = 0; i < 3; i++)
            {
                string randomName = names[diceRandom.Next(names.Length)];
                Console.WriteLine($"      {randomName}");
            }

            // Cryptographically secure random numbers
            Console.WriteLine("\nCryptographically secure random numbers:");
            
            using (RandomNumberGenerator cryptoRandom = RandomNumberGenerator.Create())
            {
                byte[] secureBytes = new byte[16];
                cryptoRandom.GetBytes(secureBytes);
                
                Console.WriteLine($"  Secure random bytes: [{string.Join(", ", secureBytes.Take(8))}...]");
                
                // Generate secure random integer
                byte[] intBytes = new byte[4];
                cryptoRandom.GetBytes(intBytes);
                int secureInt = BitConverter.ToInt32(intBytes, 0);
                Console.WriteLine($"  Secure random int: {secureInt}");
            }

            Console.WriteLine();
        }

        static void DemonstrateBitOperations()
        {
            Console.WriteLine("9. BIT OPERATIONS - LOW-LEVEL MANIPULATION");
            Console.WriteLine("==========================================");

            // Bit operations are essential for performance optimization and low-level programming
            Console.WriteLine("Basic bitwise operations:");

            int a = 12;  // Binary: 1100
            int b = 10;  // Binary: 1010
            
            Console.WriteLine($"  a = {a} (binary: {Convert.ToString(a, 2).PadLeft(8, '0')})");
            Console.WriteLine($"  b = {b} (binary: {Convert.ToString(b, 2).PadLeft(8, '0')})");
            Console.WriteLine($"  a & b = {a & b} (AND)");
            Console.WriteLine($"  a | b = {a | b} (OR)");
            Console.WriteLine($"  a ^ b = {a ^ b} (XOR)");
            Console.WriteLine($"  ~a = {~a} (NOT)");
            Console.WriteLine($"  a << 1 = {a << 1} (left shift)");
            Console.WriteLine($"  a >> 1 = {a >> 1} (right shift)");

            // BitOperations class methods
            Console.WriteLine("\nBitOperations class methods:");
            
            int[] testNumbers = { 16, 17, 32, 255, 1024 };
            
            foreach (int num in testNumbers)
            {
                Console.WriteLine($"\n  Number: {num} (binary: {Convert.ToString(num, 2)})");
                Console.WriteLine($"    Is power of 2: {BitOperations.IsPow2((uint)num)}");
                Console.WriteLine($"    Leading zero count: {BitOperations.LeadingZeroCount((uint)num)}");
                Console.WriteLine($"    Trailing zero count: {BitOperations.TrailingZeroCount((uint)num)}");
                Console.WriteLine($"    Population count (1s): {BitOperations.PopCount((uint)num)}");
                
                if (num > 0)
                {
                    Console.WriteLine($"    Log2: {BitOperations.Log2((uint)num)}");
                }
            }

            // Bit rotation
            Console.WriteLine("\nBit rotation operations:");
            
            uint rotateValue = 0b11110000_00000000_00000000_00001111;
            Console.WriteLine($"  Original: 0x{rotateValue:X8}");
            
            uint leftRotated = BitOperations.RotateLeft(rotateValue, 4);
            uint rightRotated = BitOperations.RotateRight(rotateValue, 4);
            
            Console.WriteLine($"  Rotate left 4:  0x{leftRotated:X8}");
            Console.WriteLine($"  Rotate right 4: 0x{rightRotated:X8}");

            // Practical examples
            Console.WriteLine("\nPractical bit manipulation examples:");
            
            // Flags enumeration simulation
            Console.WriteLine("  File permissions (using bit flags):");
            const int READ = 1;    // 001
            const int WRITE = 2;   // 010
            const int EXECUTE = 4; // 100
            
            int permissions = READ | WRITE; // Set read and write
            Console.WriteLine($"    Initial permissions: {permissions} (binary: {Convert.ToString(permissions, 2).PadLeft(3, '0')})");
            Console.WriteLine($"    Has read: {(permissions & READ) != 0}");
            Console.WriteLine($"    Has write: {(permissions & WRITE) != 0}");
            Console.WriteLine($"    Has execute: {(permissions & EXECUTE) != 0}");
            
            permissions |= EXECUTE; // Add execute permission
            Console.WriteLine($"    After adding execute: {permissions} (binary: {Convert.ToString(permissions, 2).PadLeft(3, '0')})");
            
            permissions &= ~WRITE; // Remove write permission
            Console.WriteLine($"    After removing write: {permissions} (binary: {Convert.ToString(permissions, 2).PadLeft(3, '0')})");

            // Fast power of 2 operations
            Console.WriteLine("\n  Fast power-of-2 operations:");
            
            int value = 100;
            Console.WriteLine($"    Multiply {value} by 8 (2^3): {value << 3}");
            Console.WriteLine($"    Divide {value} by 4 (2^2): {value >> 2}");
            Console.WriteLine($"    Check if {value} is even: {(value & 1) == 0}");

            // Bit field extraction
            Console.WriteLine("\n  Color component extraction (RGB):");
            
            uint color = 0xFF8040; // RGB color
            uint red = (color >> 16) & 0xFF;
            uint green = (color >> 8) & 0xFF;
            uint blue = color & 0xFF;
            
            Console.WriteLine($"    Color: 0x{color:X6}");
            Console.WriteLine($"    Red: {red} (0x{red:X2})");
            Console.WriteLine($"    Green: {green} (0x{green:X2})");
            Console.WriteLine($"    Blue: {blue} (0x{blue:X2})");

            Console.WriteLine();
        }

        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("10. REAL-WORLD SCENARIOS");
            Console.WriteLine("=========================");

            // Scenario 1: Financial calculations with precision
            Console.WriteLine("Scenario 1: Financial calculations");
            
            decimal principal = 10000m;
            decimal annualRate = 0.05m; // 5%
            int years = 10;
            
            // Compound interest calculation
            decimal compoundAmount = principal * (decimal)Math.Pow((double)(1 + annualRate), years);
            decimal simpleInterest = principal * (1 + annualRate * years);
            
            Console.WriteLine($"  Principal: {principal:C}");
            Console.WriteLine($"  Annual rate: {annualRate:P}");
            Console.WriteLine($"  Years: {years}");
            Console.WriteLine($"  Compound interest: {compoundAmount:C}");
            Console.WriteLine($"  Simple interest: {simpleInterest:C}");
            Console.WriteLine($"  Difference: {compoundAmount - simpleInterest:C}");

            // Loan payment calculation
            decimal loanAmount = 250000m;
            decimal monthlyRate = 0.04m / 12; // 4% annual / 12 months
            int months = 30 * 12; // 30 years
            
            decimal monthlyPayment = loanAmount * (monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), months)) 
                                   / ((decimal)Math.Pow((double)(1 + monthlyRate), months) - 1);
            
            Console.WriteLine($"\n  Loan calculation:");
            Console.WriteLine($"    Loan amount: {loanAmount:C}");
            Console.WriteLine($"    Monthly payment: {monthlyPayment:C}");
            Console.WriteLine($"    Total paid: {monthlyPayment * months:C}");
            Console.WriteLine($"    Total interest: {(monthlyPayment * months) - loanAmount:C}");

            // Scenario 2: Scientific calculations
            Console.WriteLine("\n\nScenario 2: Scientific calculations");
            
            // Physics: Projectile motion
            double initialVelocity = 50; // m/s
            double angle = 45; // degrees
            double gravity = 9.81; // m/s²
            
            double angleRadians = angle * Math.PI / 180;
            double maxHeight = Math.Pow(initialVelocity * Math.Sin(angleRadians), 2) / (2 * gravity);
            double range = Math.Pow(initialVelocity, 2) * Math.Sin(2 * angleRadians) / gravity;
            double flightTime = 2 * initialVelocity * Math.Sin(angleRadians) / gravity;
            
            Console.WriteLine($"  Projectile motion:");
            Console.WriteLine($"    Initial velocity: {initialVelocity} m/s");
            Console.WriteLine($"    Launch angle: {angle}°");
            Console.WriteLine($"    Maximum height: {maxHeight:F2} m");
            Console.WriteLine($"    Range: {range:F2} m");
            Console.WriteLine($"    Flight time: {flightTime:F2} s");

            // Chemistry: Ideal gas law calculations
            double pressure = 101325; // Pa (1 atm)
            double volume = 0.0224; // m³ (22.4 L)
            double gasConstant = 8.314; // J/(mol·K)
            double temperature = pressure * volume / gasConstant; // Kelvin
            
            Console.WriteLine($"\n  Ideal gas law (PV = nRT, assuming 1 mole):");
            Console.WriteLine($"    Pressure: {pressure:N0} Pa");
            Console.WriteLine($"    Volume: {volume:F4} m³");
            Console.WriteLine($"    Temperature: {temperature:F2} K ({temperature - 273.15:F2}°C)");

            // Scenario 3: Data analysis and statistics
            Console.WriteLine("\n\nScenario 3: Statistical analysis");
            
            double[] dataset = { 23.5, 18.2, 31.7, 22.1, 28.9, 19.8, 26.4, 33.2, 21.6, 29.3 };
            
            double mean = dataset.Average();
            double variance = dataset.Select(x => Math.Pow(x - mean, 2)).Average();
            double standardDeviation = Math.Sqrt(variance);
            double median = dataset.OrderBy(x => x).Skip(dataset.Length / 2).First();
            
            Console.WriteLine($"  Dataset: [{string.Join(", ", dataset.Select(x => x.ToString("F1")))}]");
            Console.WriteLine($"  Mean: {mean:F2}");
            Console.WriteLine($"  Median: {median:F1}");
            Console.WriteLine($"  Standard deviation: {standardDeviation:F2}");
            Console.WriteLine($"  Min: {dataset.Min():F1}");
            Console.WriteLine($"  Max: {dataset.Max():F1}");

            // Normal distribution probability (approximation)
            double value = 25.0;
            double zScore = (value - mean) / standardDeviation;
            double probability = 0.5 * (1 + Erf(zScore / Math.Sqrt(2))); // Cumulative probability
            
            Console.WriteLine($"\n  Normal distribution analysis for value {value}:");
            Console.WriteLine($"    Z-score: {zScore:F2}");
            Console.WriteLine($"    Cumulative probability: {probability:F3} ({probability * 100:F1}%)");

            // Scenario 4: Cryptographic and security applications
            Console.WriteLine("\n\nScenario 4: Cryptographic operations");
            
            // Generate a simple hash (for demonstration - not cryptographically secure)
            string data = "Hello, World!";
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
            uint simpleHash = 0;
            
            foreach (byte b in dataBytes)
            {
                simpleHash = simpleHash * 31 + b;
            }
            
            Console.WriteLine($"  Data: \"{data}\"");
            Console.WriteLine($"  Simple hash: 0x{simpleHash:X8}");

            // RSA key size calculation
            int[] rsaKeySizes = { 1024, 2048, 3072, 4096 };
            
            Console.WriteLine("\n  RSA key strength estimates:");
            foreach (int keySize in rsaKeySizes)
            {
                // Very rough estimate of security level in bits
                double securityBits = keySize / 3.0; 
                BigInteger keySpace = BigInteger.Pow(2, keySize);
                
                Console.WriteLine($"    {keySize}-bit RSA: ~{securityBits:F0} bits security");
                Console.WriteLine($"      Key space: 2^{keySize} ≈ 10^{Math.Log10((double)keySpace):F0}");
            }

            Console.WriteLine();
        }

        // Helper methods
        static string ConvertToPermissionString(int octalValue)
        {
            // Convert octal permission to rwx format
            string[] permissions = { "---", "--x", "-w-", "-wx", "r--", "r-x", "rw-", "rwx" };
            
            int owner = (octalValue >> 6) & 7;
            int group = (octalValue >> 3) & 7;
            int other = octalValue & 7;
            
            return permissions[owner] + permissions[group] + permissions[other];
        }

        static BigInteger CalculateFactorial(int n)
        {
            BigInteger result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }

        // Error function approximation for normal distribution
        static double Erf(double x)
        {
            // Abramowitz and Stegun approximation
            double a1 =  0.254829592;
            double a2 = -0.284496736;
            double a3 =  1.421413741;
            double a4 = -1.453152027;
            double a5 =  1.061405429;
            double p  =  0.3275911;

            int sign = x < 0 ? -1 : 1;
            x = Math.Abs(x);

            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }
    }
}
