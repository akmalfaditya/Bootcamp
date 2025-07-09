using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Text;

// Other Conversion Mechanisms: Specialized Data Transformations in .NET
// Beyond the basic ToString() and Parse() methods, .NET provides several specialized
// conversion mechanisms for specific scenarios. Let's explore the professional toolkit!

namespace OtherConversionMechanism
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== OTHER CONVERSION MECHANISMS IN .NET ===");
            Console.WriteLine("Specialized tools for professional data transformation\n");

            // These conversion mechanisms solve specific problems that basic parsing can't handle
            DemonstrateConvertClass();
            DemonstrateRoundingBehavior();
            DemonstrateBaseConversions();
            DemonstrateDynamicConversions();
            DemonstrateBase64Operations();
            DemonstrateXmlConvertSpecialties();
            DemonstrateTypeConverterMagic();
            DemonstrateBitConverterOperations();
            DemonstrateAdvancedScenarios();

            Console.WriteLine("\n=== MASTERY COMPLETE ===");
            Console.WriteLine("You now understand .NET's full conversion toolkit!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DemonstrateConvertClass()
        {
            Console.WriteLine("1. CONVERT CLASS - THE SWISS ARMY KNIFE OF CONVERSIONS");
            Console.WriteLine("=======================================================");

            // Convert class handles conversions between .NET's "base types"
            // bool, char, string, DateTime, DateTimeOffset, and all numeric types
            // It's more robust than simple casting and handles edge cases gracefully

            Console.WriteLine("Basic Convert class capabilities:");
            
            // Converting between common types - more forgiving than Parse methods
            string numberString = "42";
            string doubleString = "3.14159";
            string boolString = "true";

            int convertedInt = Convert.ToInt32(numberString);
            double convertedDouble = Convert.ToDouble(doubleString);
            bool convertedBool = Convert.ToBoolean(boolString);

            Console.WriteLine($"  String \"{numberString}\" -> int {convertedInt}");
            Console.WriteLine($"  String \"{doubleString}\" -> double {convertedDouble}");
            Console.WriteLine($"  String \"{boolString}\" -> bool {convertedBool}");

            // Converting between numeric types with automatic handling
            decimal money = 123.456m;
            float precision = 789.012f;
            long reasonableNumber = 9876543; // Changed from too large number

            Console.WriteLine("\nNumeric type conversions:");
            Console.WriteLine($"  decimal {money} -> int {Convert.ToInt32(money)}");
            Console.WriteLine($"  float {precision} -> int {Convert.ToInt32(precision)}");
            Console.WriteLine($"  long {reasonableNumber} -> int {Convert.ToInt32(reasonableNumber)}");

            // The Convert class gracefully handles null values - a huge advantage
            Console.WriteLine("\nNull handling (Convert's superpower):");
            string? nullString = null;
            object? nullObject = null;

            // Convert returns default values instead of throwing exceptions
            int defaultInt = Convert.ToInt32(nullString); // Returns 0
            bool defaultBool = Convert.ToBoolean(nullObject); // Returns false
            double defaultDouble = Convert.ToDouble(nullString); // Returns 0.0

            Console.WriteLine($"  null string -> int: {defaultInt}");
            Console.WriteLine($"  null object -> bool: {defaultBool}");
            Console.WriteLine($"  null string -> double: {defaultDouble}");

            // Demonstrating Convert's advantage over direct parsing
            Console.WriteLine("\nWhy Convert is often better than Parse:");
            
            try
            {
                // This would throw an exception
                // int.Parse(null); 
                Console.WriteLine("  int.Parse(null) would throw NullReferenceException");
                
                // This gracefully returns 0
                int safeResult = Convert.ToInt32(null);
                Console.WriteLine($"  Convert.ToInt32(null) safely returns: {safeResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error: {ex.Message}");
            }

            Console.WriteLine();
        }

        static void DemonstrateRoundingBehavior()
        {
            Console.WriteLine("2. ROUNDING CONVERSIONS - BANKER'S ROUNDING EXPLAINED");
            Console.WriteLine("======================================================");

            // This is where Convert really shines - it uses banker's rounding
            // Standard casting truncates, Convert rounds intelligently
            // Banker's rounding reduces systematic bias in large datasets

            Console.WriteLine("Understanding the difference between casting and Convert:");
            Console.WriteLine("Casting truncates (always rounds toward zero)");
            Console.WriteLine("Convert uses banker's rounding (rounds to nearest even for .5 values)");
            Console.WriteLine();

            double[] criticalValues = { 2.1, 2.5, 2.9, 3.5, 4.5, 5.5, 6.5, 7.5 };

            Console.WriteLine("Value    Cast    Convert   Explanation");
            Console.WriteLine("-----    ----    -------   -----------");

            foreach (double value in criticalValues)
            {
                int castResult = (int)value;           // Truncates
                int convertResult = Convert.ToInt32(value); // Banker's rounding
                
                string explanation = value % 1 == 0.5 ? 
                    $"Midpoint -> rounds to nearest even ({convertResult})" : 
                    "Standard rounding";

                Console.WriteLine($"{value,-7}  {castResult,-6}  {convertResult,-7}   {explanation}");
            }

            // Real-world impact: Financial calculations
            Console.WriteLine("\nWhy banker's rounding matters in financial systems:");
            Console.WriteLine("It prevents systematic bias that could accumulate over many transactions");
            
            double[] transactions = { 10.125, 15.625, 23.375, 8.875, 12.125, 19.375 };
            double castTotal = 0;
            double convertTotal = 0;

            Console.WriteLine("\nTransaction processing comparison:");
            Console.WriteLine("Amount    Cast(¢)  Convert(¢)  Difference");
            Console.WriteLine("------    -------  ----------  ----------");

            foreach (double transaction in transactions)
            {
                // Convert to cents for processing
                int castCents = (int)(transaction * 100);
                int convertCents = Convert.ToInt32(transaction * 100);
                
                castTotal += castCents;
                convertTotal += convertCents;

                Console.WriteLine($"${transaction,-8:F3}  {castCents,-7}  {convertCents,-10}  {convertCents - castCents}");
            }

            Console.WriteLine($"\nTotal difference: {convertTotal - castTotal} cents");
            Console.WriteLine("Over thousands of transactions, this adds up!");

            // Demonstrating Math.Round for custom rounding control
            Console.WriteLine("\nCustom rounding with Math.Round:");
            double testValue = 3.5;
            
            Console.WriteLine($"Value: {testValue}");
            Console.WriteLine($"  Convert.ToInt32(): {Convert.ToInt32(testValue)} (banker's rounding)");
            Console.WriteLine($"  Math.Round(ToEven): {Math.Round(testValue, MidpointRounding.ToEven)}");
            Console.WriteLine($"  Math.Round(AwayFromZero): {Math.Round(testValue, MidpointRounding.AwayFromZero)}");
            Console.WriteLine($"  Math.Round(ToZero): {Math.Round(testValue, MidpointRounding.ToZero)}");
            Console.WriteLine($"  Math.Round(ToPositiveInfinity): {Math.Round(testValue, MidpointRounding.ToPositiveInfinity)}");

            Console.WriteLine();
        }

        static void DemonstrateBaseConversions()
        {
            Console.WriteLine("3. BASE CONVERSIONS - BEYOND DECIMAL NUMBERS");
            Console.WriteLine("=============================================");

            // Convert provides overloads for parsing numbers in different bases
            // Essential for low-level programming, embedded systems, and data analysis
            // Supports bases 2, 8, 10, and 16

            Console.WriteLine("Parsing numbers in different bases:");

            // Hexadecimal (base 16) - the programmer's friend
            Console.WriteLine("\nHexadecimal conversions (base 16):");
            string[] hexValues = { "FF", "1E", "A0", "DEADBEEF" };
            
            foreach (string hex in hexValues)
            {
                try
                {
                    if (hex.Length <= 8) // Fits in int
                    {
                        int fromHex = Convert.ToInt32(hex, 16);
                        Console.WriteLine($"  0x{hex} -> {fromHex} (decimal)");
                    }
                    else // Use long for larger values
                    {
                        long fromHexLong = Convert.ToInt64(hex, 16);
                        Console.WriteLine($"  0x{hex} -> {fromHexLong} (decimal, long)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  0x{hex} -> Error: {ex.Message}");
                }
            }

            // Binary (base 2) - understanding the machine
            Console.WriteLine("\nBinary conversions (base 2):");
            string[] binaryValues = { "1010", "11110000", "10101010" };
            
            foreach (string binary in binaryValues)
            {
                int fromBinary = Convert.ToInt32(binary, 2);
                Console.WriteLine($"  {binary}₂ -> {fromBinary} (decimal)");
            }

            // Octal (base 8) - Unix permissions and more
            Console.WriteLine("\nOctal conversions (base 8):");
            string[] octalValues = { "755", "644", "777" };
            
            foreach (string octal in octalValues)
            {
                int fromOctal = Convert.ToInt32(octal, 8);
                Console.WriteLine($"  {octal}₈ -> {fromOctal} (decimal)");
            }

            // Converting decimal back to different bases
            Console.WriteLine("\nConverting decimal to different bases:");
            int[] decimalValues = { 255, 170, 15 };
            
            Console.WriteLine("Decimal  Hex   Binary     Octal");
            Console.WriteLine("-------  ---   -------    -----");
            
            foreach (int value in decimalValues)
            {
                string toHex = Convert.ToString(value, 16).ToUpper();
                string toBinary = Convert.ToString(value, 2);
                string toOctal = Convert.ToString(value, 8);

                Console.WriteLine($"{value,-7}  {toHex,-4}  {toBinary,-9}  {toOctal}");
            }

            // Real-world application: RGB color values
            Console.WriteLine("\nPractical example - RGB color processing:");
            string hexColor = "#FF6A7F"; // Remove the # prefix for parsing
            string cleanHex = hexColor.TrimStart('#');
            
            // Parse RGB components
            int red = Convert.ToInt32(cleanHex.Substring(0, 2), 16);
            int green = Convert.ToInt32(cleanHex.Substring(2, 2), 16);
            int blue = Convert.ToInt32(cleanHex.Substring(4, 2), 16);
            
            Console.WriteLine($"  Color {hexColor}:");
            Console.WriteLine($"    Red:   {cleanHex.Substring(0, 2)} -> {red}");
            Console.WriteLine($"    Green: {cleanHex.Substring(2, 2)} -> {green}");
            Console.WriteLine($"    Blue:  {cleanHex.Substring(4, 2)} -> {blue}");

            // Unix file permissions example
            Console.WriteLine("\nUnix file permissions (octal notation):");
            var permissions = new[] { 
                ("644", "rw-r--r--", "Owner: read/write, Group: read, Others: read"),
                ("755", "rwxr-xr-x", "Owner: full, Group: read/execute, Others: read/execute"),
                ("777", "rwxrwxrwx", "Everyone: full permissions")
            };

            foreach (var (octal, symbolic, description) in permissions)
            {
                int decimal_perm = Convert.ToInt32(octal, 8);
                Console.WriteLine($"  {octal} (octal) = {decimal_perm} (decimal) = {symbolic} ({description})");
            }

            Console.WriteLine();
        }

        static void DemonstrateDynamicConversions()
        {
            Console.WriteLine("4. DYNAMIC CONVERSIONS - RUNTIME TYPE CONVERSION");
            Console.WriteLine("================================================");

            // Convert.ChangeType is powerful when you don't know the target type at compile time
            // Common in reflection, serialization, and generic programming scenarios

            Console.WriteLine("Dynamic type conversion examples:");

            // Simulating data from a configuration file or database
            object[] configValues = { "42", "3.14", "true", "2024-05-29" };
            Type[] targetTypes = { typeof(int), typeof(double), typeof(bool), typeof(DateTime) };
            string[] descriptions = { "Port number", "Tax rate", "Debug mode", "Release date" };

            for (int i = 0; i < configValues.Length; i++)
            {
                try
                {
                    object converted = Convert.ChangeType(configValues[i], targetTypes[i]);
                    Console.WriteLine($"  {descriptions[i]}: \"{configValues[i]}\" -> {converted} ({targetTypes[i].Name})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  {descriptions[i]}: Failed to convert - {ex.Message}");
                }
            }

            // Generic method example
            Console.WriteLine("\nUsing dynamic conversion in a generic method:");
            
            string intString = "123";
            string doubleString = "45.67";
            
            int convertedInt = ConvertValue<int>(intString);
            double convertedDouble = ConvertValue<double>(doubleString);
            
            Console.WriteLine($"  ConvertValue<int>(\"{intString}\") -> {convertedInt}");
            Console.WriteLine($"  ConvertValue<double>(\"{doubleString}\") -> {convertedDouble}");

            // Handling nullable types
            Console.WriteLine("\nWorking with nullable types:");
            
            Type nullableIntType = typeof(int?);
            Type underlyingType = Nullable.GetUnderlyingType(nullableIntType) ?? nullableIntType;
            
            object nullableResult = Convert.ChangeType("456", underlyingType);
            Console.WriteLine($"  Converting to nullable int: {nullableResult}");

            Console.WriteLine();
        }

        static void DemonstrateBase64Operations()
        {
            Console.WriteLine("5. BASE64 OPERATIONS - BINARY DATA AS TEXT");
            Console.WriteLine("===========================================");

            // Base64 encoding represents binary data in ASCII text format
            // Crucial for email attachments, JSON APIs, data URLs, and embedded content
            // Every web developer needs to understand this!

            Console.WriteLine("Why Base64 matters:");
            Console.WriteLine("- Embeds binary data in text-based formats (JSON, XML, HTML)");
            Console.WriteLine("- Safe for transmission over text-based protocols");
            Console.WriteLine("- Used in data URIs, email attachments, and web APIs");
            Console.WriteLine();

            // Basic text encoding
            Console.WriteLine("Basic text encoding example:");
            string originalText = "Hello, World! 🌍 Special chars: äöü";
            byte[] textBytes = Encoding.UTF8.GetBytes(originalText);
            string base64Text = Convert.ToBase64String(textBytes);

            Console.WriteLine($"  Original: \"{originalText}\"");
            Console.WriteLine($"  UTF-8 bytes: [{textBytes.Length} bytes]");
            Console.WriteLine($"  Base64: {base64Text}");

            // Decode back to verify
            byte[] decodedBytes = Convert.FromBase64String(base64Text);
            string decodedText = Encoding.UTF8.GetString(decodedBytes);
            Console.WriteLine($"  Decoded: \"{decodedText}\"");
            Console.WriteLine($"  Match: {originalText == decodedText}");

            // Binary data example
            Console.WriteLine("\nBinary data encoding:");
            byte[] binaryData = { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46 };
            string binaryBase64 = Convert.ToBase64String(binaryData);
            
            Console.WriteLine($"  Binary: [{string.Join(", ", binaryData.Select(b => $"0x{b:X2}"))}]");
            Console.WriteLine($"  Base64: {binaryBase64}");

            // Simulate file embedding (common in web development)
            Console.WriteLine("\nWeb development example - Data URIs:");
            
            // Simulate a tiny PNG image (just the header bytes)
            byte[] pngHeader = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string pngBase64 = Convert.ToBase64String(pngHeader);
            
            Console.WriteLine($"  PNG header bytes: {pngBase64}");
            Console.WriteLine($"  Data URI: data:image/png;base64,{pngBase64}");

            // Line breaks for readability (useful for large data)
            Console.WriteLine("\nFormatting options for large data:");
            byte[] largeData = new byte[100];
            new Random(12345).NextBytes(largeData); // Consistent seed for demo
            
            string compactBase64 = Convert.ToBase64String(largeData);
            string formattedBase64 = Convert.ToBase64String(largeData, Base64FormattingOptions.InsertLineBreaks);
            
            Console.WriteLine($"  Compact ({compactBase64.Length} chars):");
            Console.WriteLine($"    {compactBase64}");
            Console.WriteLine($"  With line breaks:");
            Console.WriteLine($"    {formattedBase64}");

            // Real-world scenario: API token encoding
            Console.WriteLine("\nAPI authentication example:");
            string apiKey = "user123";
            string apiSecret = "secret456";
            string credentials = $"{apiKey}:{apiSecret}";
            
            byte[] credentialBytes = Encoding.UTF8.GetBytes(credentials);
            string encodedCredentials = Convert.ToBase64String(credentialBytes);
            
            Console.WriteLine($"  Credentials: {credentials}");
            Console.WriteLine($"  Base64: {encodedCredentials}");
            Console.WriteLine($"  HTTP Header: Authorization: Basic {encodedCredentials}");

            // Handling invalid Base64 strings
            Console.WriteLine("\nError handling:");
            string[] invalidBase64 = { "invalid!", "almost=valid", "SGVsbG8gV29ybGQ!" };
            
            foreach (string invalid in invalidBase64)
            {
                try
                {
                    byte[] result = Convert.FromBase64String(invalid);
                    Console.WriteLine($"  \"{invalid}\" -> Valid ({result.Length} bytes)");
                }
                catch (FormatException)
                {
                    Console.WriteLine($"  \"{invalid}\" -> Invalid Base64 format");
                }
            }

            Console.WriteLine();
        }

        static void DemonstrateXmlConvertSpecialties()
        {
            Console.WriteLine("6. XMLCONVERT - XML STANDARDS COMPLIANCE");
            Console.WriteLine("=========================================");

            // XmlConvert ensures data conforms to XML/W3C standards
            // Different from standard .NET formatting in crucial ways
            // Essential for XML serialization, SOAP services, and web APIs

            Console.WriteLine("Why XmlConvert is different from standard ToString():");
            Console.WriteLine("- XML requires lowercase boolean values ('true'/'false' not 'True'/'False')");
            Console.WriteLine("- Culture-invariant by design (essential for data exchange)");
            Console.WriteLine("- Handles XML-specific datetime formats and timezone info");
            Console.WriteLine();

            // Boolean differences - this trips up many developers
            Console.WriteLine("Boolean conversion differences:");
            bool flag = true;
            
            Console.WriteLine($"  Standard ToString(): {flag.ToString()}");
            Console.WriteLine($"  XmlConvert.ToString(): {XmlConvert.ToString(flag)}");
            
            string xmlBool = XmlConvert.ToString(flag);
            bool parsedBool = XmlConvert.ToBoolean(xmlBool);
            Console.WriteLine($"  Round-trip test: {flag} -> \"{xmlBool}\" -> {parsedBool}");

            // DateTime handling - the real strength of XmlConvert
            Console.WriteLine("\nDateTime XML serialization modes:");
            DateTime now = DateTime.Now;
            DateTime utcNow = DateTime.UtcNow;
            DateTime unspecified = new DateTime(2024, 5, 29, 14, 30, 0, DateTimeKind.Unspecified);
            
            // Local mode - includes timezone offset
            string localXml = XmlConvert.ToString(now, XmlDateTimeSerializationMode.Local);
            Console.WriteLine($"  Local time with offset: {localXml}");
            
            // UTC mode - converts to UTC and adds 'Z'
            string utcXml = XmlConvert.ToString(utcNow, XmlDateTimeSerializationMode.Utc);
            Console.WriteLine($"  UTC time: {utcXml}");
            
            // Unspecified - strips timezone info
            string unspecifiedXml = XmlConvert.ToString(unspecified, XmlDateTimeSerializationMode.Unspecified);
            Console.WriteLine($"  Unspecified: {unspecifiedXml}");
            
            // RoundtripKind - preserves original DateTimeKind (safest option)
            string roundtripXml = XmlConvert.ToString(now, XmlDateTimeSerializationMode.RoundtripKind);
            Console.WriteLine($"  Roundtrip kind: {roundtripXml}");

            // Numeric conversions - culture invariant
            Console.WriteLine("\nNumeric XML conversions (always culture-invariant):");
            decimal price = 1234.56m;
            double scientific = 1.23e-4;
            float singlePrecision = 456.789f;
            
            Console.WriteLine($"  Decimal: {XmlConvert.ToString(price)}");
            Console.WriteLine($"  Scientific notation: {XmlConvert.ToString(scientific)}");
            Console.WriteLine($"  Float: {XmlConvert.ToString(singlePrecision)}");

            // Special floating-point values
            Console.WriteLine("\nSpecial numeric values (IEEE 754 compliance):");
            double[] specialValues = { 
                double.PositiveInfinity, 
                double.NegativeInfinity, 
                double.NaN 
            };
            
            foreach (double value in specialValues)
            {
                string xmlValue = XmlConvert.ToString(value);
                Console.WriteLine($"  {value} -> \"{xmlValue}\"");
            }

            // Parsing various XML date formats
            Console.WriteLine("\nParsing XML date strings:");
            string[] xmlDates = {
                "2024-05-29T14:30:00Z",           // UTC format
                "2024-05-29T14:30:00+07:00",     // With timezone offset
                "2024-05-29T14:30:00.123Z",      // With milliseconds
                "2024-05-29",                     // Date only
                "14:30:00"                        // Time only
            };

            foreach (string xmlDate in xmlDates)
            {
                try
                {
                    DateTime parsed = XmlConvert.ToDateTime(xmlDate, XmlDateTimeSerializationMode.RoundtripKind);
                    Console.WriteLine($"  \"{xmlDate}\" -> {parsed:yyyy-MM-dd HH:mm:ss.fff} (Kind: {parsed.Kind})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  \"{xmlDate}\" -> Parse error: {ex.Message}");
                }
            }

            // Demonstrating culture independence
            Console.WriteLine("\nCulture independence demonstration:");
            decimal testAmount = 1234.56m;
            
            // These will be the same regardless of system culture
            string xmlFormat = XmlConvert.ToString(testAmount);
            string invariantFormat = testAmount.ToString(System.Globalization.CultureInfo.InvariantCulture);
            
            Console.WriteLine($"  XmlConvert: {xmlFormat}");
            Console.WriteLine($"  InvariantCulture: {invariantFormat}");
            Console.WriteLine($"  Same result: {xmlFormat == invariantFormat}");

            Console.WriteLine();
        }

        static void DemonstrateTypeConverterMagic()
        {
            Console.WriteLine("7. TYPE CONVERTERS - DESIGN-TIME INTELLIGENCE");
            Console.WriteLine("==============================================");

            // TypeConverters are mainly used in Visual Studio designers and XAML
            // They provide context-aware string-to-object conversion
            // Much more flexible than standard parsing methods

            Console.WriteLine("What makes TypeConverters special:");
            Console.WriteLine("- Context-aware parsing (can infer format from content)");
            Console.WriteLine("- Used by Visual Studio property editors");
            Console.WriteLine("- XAML attribute parsing relies on these");
            Console.WriteLine("- Can provide design-time services (dropdown lists, etc.)");
            Console.WriteLine();

            // Color converter demonstration
            Console.WriteLine("Color TypeConverter examples:");

            try
            {
                TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));
                
                // Converting various color representations
                string[] colorInputs = { "Red", "Blue", "Beige", "Transparent", "DarkSlateGray", "255, 128, 0" };
                
                Console.WriteLine("Input String      -> R    G    B    A    Name");
                Console.WriteLine("------------      -  ---  ---  ---  ---  ----");
                
                foreach (string colorInput in colorInputs)
                {
                    if (colorConverter.CanConvertFrom(typeof(string)))
                    {
                        try
                        {
                            object? converted = colorConverter.ConvertFromString(colorInput);
                            if (converted is Color color)
                            {
                                Console.WriteLine($"{colorInput,-16}  -> {color.R,3}  {color.G,3}  {color.B,3}  {color.A,3}  {color.Name}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{colorInput,-16}  -> Error: {ex.Message}");
                        }
                    }
                }

                // Converting back to string
                Console.WriteLine("\nColor to string conversion:");
                Color testColor = Color.FromArgb(255, 128, 64);
                string colorString = colorConverter.ConvertToString(testColor) ?? "null";
                Console.WriteLine($"  Color(255,128,64) -> \"{colorString}\"");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Color converter not available: {ex.Message}");
                Console.WriteLine("(This is normal in some environments without System.Drawing)");
            }

            // Exploring other built-in type converters
            Console.WriteLine("\nOther built-in type converters:");
            
            Type[] typesToTest = { typeof(int), typeof(DateTime), typeof(bool), typeof(decimal), typeof(TimeSpan) };
            
            foreach (Type type in typesToTest)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                bool canConvertFromString = converter.CanConvertFrom(typeof(string));
                bool canConvertToString = converter.CanConvertTo(typeof(string));
                
                Console.WriteLine($"  {type.Name}:");
                Console.WriteLine($"    Can convert from string: {canConvertFromString}");
                Console.WriteLine($"    Can convert to string: {canConvertToString}");
                
                if (canConvertFromString)
                {
                    try
                    {
                        // Test with appropriate sample values
                        string testValue = type.Name switch
                        {
                            "Int32" => "42",
                            "DateTime" => "2024-05-29 14:30:00",
                            "Boolean" => "true",
                            "Decimal" => "123.45",
                            "TimeSpan" => "01:30:45",
                            _ => "test"
                        };
                        
                        object? converted = converter.ConvertFromString(testValue);
                        Console.WriteLine($"    Example: \"{testValue}\" -> {converted} ({converted?.GetType().Name})");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    Conversion failed: {ex.Message}");
                    }
                }
                Console.WriteLine();
            }

            // Demonstrating advanced TypeConverter features
            Console.WriteLine("Advanced TypeConverter capabilities:");
            
            TypeConverter intConverter = TypeDescriptor.GetConverter(typeof(int));
            
            // Check what types it can convert from
            Type[] sourceTypes = { typeof(string), typeof(double), typeof(decimal), typeof(bool) };
            Console.WriteLine("  Int32 converter can convert from:");
            
            foreach (Type sourceType in sourceTypes)
            {
                bool canConvert = intConverter.CanConvertFrom(sourceType);
                Console.WriteLine($"    {sourceType.Name}: {canConvert}");
                
                if (canConvert && sourceType != typeof(bool)) // Skip bool for cleaner output
                {
                    try
                    {
                        object testValue = sourceType.Name switch
                        {
                            "String" => "123",
                            "Double" => 456.789,
                            "Decimal" => 789.123m,
                            _ => "test"
                        };
                        
                        object? result = intConverter.ConvertFrom(testValue);
                        Console.WriteLine($"      Example: {testValue} -> {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"      Error: {ex.Message}");
                    }
                }
            }

            Console.WriteLine();
        }

        static void DemonstrateBitConverterOperations()
        {
            Console.WriteLine("8. BITCONVERTER - RAW BINARY DATA MANIPULATION");
            Console.WriteLine("===============================================");

            // BitConverter works at the byte level - the lowest level of data representation
            // Essential for: network protocols, file formats, serialization, cryptography
            // Understanding endianness is crucial for cross-platform compatibility

            Console.WriteLine("Understanding BitConverter:");
            Console.WriteLine("- Converts primitive types to/from byte arrays");
            Console.WriteLine("- Platform-dependent endianness (byte order)");
            Console.WriteLine("- Essential for binary protocols and file formats");
            Console.WriteLine($"- This system is: {(BitConverter.IsLittleEndian ? "Little Endian" : "Big Endian")}");
            Console.WriteLine();

            // Basic type to bytes conversion
            Console.WriteLine("Converting values to byte arrays:");
            
            var testValues = new (string name, object value, byte[] bytes)[]
            {
                ("bool true", true, BitConverter.GetBytes(true)),
                ("int 305419896", 305419896, BitConverter.GetBytes(305419896)),  // 0x12345678
                ("float π", (float)Math.PI, BitConverter.GetBytes((float)Math.PI)),
                ("double e", Math.E, BitConverter.GetBytes(Math.E)),
                ("long max", long.MaxValue, BitConverter.GetBytes(long.MaxValue))
            };

            Console.WriteLine("Type/Value           Bytes (hex)");
            Console.WriteLine("----------           -----------");
            
            foreach (var (name, value, bytes) in testValues)
            {
                string hexBytes = string.Join(" ", bytes.Take(8).Select(b => $"{b:X2}"));
                if (bytes.Length > 8) hexBytes += "...";
                Console.WriteLine($"{name,-19}  {hexBytes}");
            }

            // Converting back from bytes
            Console.WriteLine("\nConverting byte arrays back to values:");
            
            // Use the same test data
            int intValue = 305419896;
            float floatValue = (float)Math.PI;
            double doubleValue = Math.E;
            bool boolValue = true;

            byte[] intBytes = BitConverter.GetBytes(intValue);
            byte[] floatBytes = BitConverter.GetBytes(floatValue);
            byte[] doubleBytes = BitConverter.GetBytes(doubleValue);
            byte[] boolBytes = BitConverter.GetBytes(boolValue);

            // Reconstruct the values
            int recoveredInt = BitConverter.ToInt32(intBytes, 0);
            float recoveredFloat = BitConverter.ToSingle(floatBytes, 0);
            double recoveredDouble = BitConverter.ToDouble(doubleBytes, 0);
            bool recoveredBool = BitConverter.ToBoolean(boolBytes, 0);

            Console.WriteLine($"  int: {intValue} -> {recoveredInt} (match: {recoveredInt == intValue})");
            Console.WriteLine($"  float: {floatValue:F6} -> {recoveredFloat:F6} (match: {recoveredFloat == floatValue})");
            Console.WriteLine($"  double: {doubleValue:F15} -> {recoveredDouble:F15} (match: {recoveredDouble == doubleValue})");
            Console.WriteLine($"  bool: {boolValue} -> {recoveredBool} (match: {recoveredBool == boolValue})");

            // Endianness demonstration
            Console.WriteLine("\nEndianness demonstration:");
            int testInt = 0x12345678;
            byte[] testBytes = BitConverter.GetBytes(testInt);
            
            Console.WriteLine($"  Value: 0x{testInt:X8}");
            Console.WriteLine($"  Bytes: [{string.Join(", ", testBytes.Select(b => $"0x{b:X2}"))}]");
            Console.WriteLine($"  Order: {(BitConverter.IsLittleEndian ? "Least significant byte first" : "Most significant byte first")}");

            // Working with DateTime - requires special handling
            Console.WriteLine("\nDateTime binary serialization:");
            DateTime timestamp = new DateTime(2024, 5, 29, 14, 30, 45, DateTimeKind.Utc);
            
            // DateTime to binary using ToBinary()
            long binaryTime = timestamp.ToBinary();
            byte[] timeBytes = BitConverter.GetBytes(binaryTime);
            
            Console.WriteLine($"  Original: {timestamp:yyyy-MM-dd HH:mm:ss} {timestamp.Kind}");
            Console.WriteLine($"  Binary: {binaryTime}");
            Console.WriteLine($"  Bytes: [{string.Join(", ", timeBytes.Take(8).Select(b => $"0x{b:X2}"))}]");
            
            // Recover the DateTime
            long recoveredBinary = BitConverter.ToInt64(timeBytes, 0);
            DateTime recoveredTime = DateTime.FromBinary(recoveredBinary);
            Console.WriteLine($"  Recovered: {recoveredTime:yyyy-MM-dd HH:mm:ss} {recoveredTime.Kind}");

            // Decimal requires special handling too
            Console.WriteLine("\nDecimal binary representation:");
            decimal testDecimal = 123.456m;
            
            // Decimal uses int[] representation
            int[] decimalBits = decimal.GetBits(testDecimal);
            Console.WriteLine($"  Decimal: {testDecimal}");
            Console.WriteLine($"  Bits: [{string.Join(", ", decimalBits)}]");
            
            // Convert each int to bytes
            Console.Write("  Bytes: [");
            for (int i = 0; i < decimalBits.Length; i++)
            {
                byte[] intAsBytes = BitConverter.GetBytes(decimalBits[i]);
                Console.Write(string.Join(", ", intAsBytes.Select(b => $"0x{b:X2}")));
                if (i < decimalBits.Length - 1) Console.Write(", ");
            }
            Console.WriteLine("]");

            // Practical example: Network packet structure
            Console.WriteLine("\nPractical example - Binary protocol message:");
            
            // Create a simple message: [Type][ID][Timestamp][DataLength][Data]
            byte messageType = 0x01;
            uint messageId = 0x12345678;
            long timestamp_binary = DateTime.UtcNow.ToBinary();
            string messageData = "Hello, Binary World!";
            byte[] dataBytes = Encoding.UTF8.GetBytes(messageData);
            
            // Build the packet
            List<byte> packet = new List<byte>();
            packet.Add(messageType);                                    // 1 byte
            packet.AddRange(BitConverter.GetBytes(messageId));          // 4 bytes
            packet.AddRange(BitConverter.GetBytes(timestamp_binary));   // 8 bytes
            packet.AddRange(BitConverter.GetBytes((uint)dataBytes.Length)); // 4 bytes
            packet.AddRange(dataBytes);                                 // variable
            
            Console.WriteLine($"  Built packet: {packet.Count} bytes");
            Console.WriteLine($"  Header bytes: [{string.Join(" ", packet.Take(17).Select(b => $"{b:X2}"))}]");
            
            // Parse the packet
            byte[] packetBytes = packet.ToArray();
            int offset = 0;
            
            byte parsedType = packetBytes[offset++];
            uint parsedId = BitConverter.ToUInt32(packetBytes, offset); offset += 4;
            long parsedTimestamp = BitConverter.ToInt64(packetBytes, offset); offset += 8;
            uint parsedDataLength = BitConverter.ToUInt32(packetBytes, offset); offset += 4;
            string parsedData = Encoding.UTF8.GetString(packetBytes, offset, (int)parsedDataLength);
            
            Console.WriteLine($"  Parsed type: 0x{parsedType:X2}");
            Console.WriteLine($"  Parsed ID: 0x{parsedId:X8}");
            Console.WriteLine($"  Parsed timestamp: {DateTime.FromBinary(parsedTimestamp):yyyy-MM-dd HH:mm:ss.fff}");
            Console.WriteLine($"  Parsed data: \"{parsedData}\" ({parsedDataLength} bytes)");

            Console.WriteLine();
        }

        static void DemonstrateAdvancedScenarios()
        {
            Console.WriteLine("9. ADVANCED SCENARIOS - PROFESSIONAL APPLICATIONS");
            Console.WriteLine("==================================================");

            // These scenarios show how all the conversion mechanisms work together
            // in real enterprise software development situations

            // Scenario 1: Multi-format configuration system
            Console.WriteLine("Scenario 1: Enterprise configuration processor");
            Console.WriteLine("Processing configuration with mixed data types and formats:");
            
            var advancedConfig = new Dictionary<string, (string value, string format, Type targetType)>
            {
                {"server_port", ("8080", "decimal", typeof(int))},
                {"timeout_seconds", ("30.5", "decimal", typeof(double))},
                {"enable_ssl", ("true", "boolean", typeof(bool))},
                {"max_connections", ("FF", "hex", typeof(int))},
                {"buffer_size", ("10000000", "binary", typeof(int))}, // 128 in binary
                {"api_credentials", ("dXNlcjEyMzpzZWNyZXQ0NTY=", "base64", typeof(string))},
                {"maintenance_window", ("2024-05-29T02:00:00Z", "xml_datetime", typeof(DateTime))},
                {"license_key", ("12345678-ABCD-EFGH-IJKL-MNOPQRSTUVWX", "guid", typeof(Guid))},
                {"cpu_affinity", ("1010", "binary_flags", typeof(int))}
            };

            foreach (var config in advancedConfig)
            {
                Console.WriteLine($"\n  {config.Key} ({config.Value.format}): \"{config.Value.value}\"");
                
                try
                {
                    object result = config.Value.format switch
                    {
                        "decimal" => Convert.ChangeType(config.Value.value, config.Value.targetType),
                        "boolean" => Convert.ToBoolean(config.Value.value),
                        "hex" => Convert.ToInt32(config.Value.value, 16),
                        "binary" => Convert.ToInt32(config.Value.value, 2),
                        "base64" => ProcessBase64Config(config.Value.value),
                        "xml_datetime" => XmlConvert.ToDateTime(config.Value.value, XmlDateTimeSerializationMode.RoundtripKind),
                        "guid" => Guid.Parse(config.Value.value),
                        "binary_flags" => ProcessBinaryFlags(config.Value.value),
                        _ => config.Value.value
                    };
                    
                    Console.WriteLine($"    -> {config.Value.targetType.Name}: {result}");
                    
                    // Add interpretation for specific fields
                    if (config.Key == "cpu_affinity")
                    {
                        Console.WriteLine($"    -> CPU cores enabled: {string.Join(", ", GetEnabledCores((int)result))}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    -> Error: {ex.Message}");
                }
            }

            // Scenario 2: Binary protocol with multiple data types
            Console.WriteLine("\n\nScenario 2: Complex binary protocol implementation");
            Console.WriteLine("Building and parsing a structured binary message:");
            
            // Define a complex message structure:
            // [Magic(4)][Version(2)][Type(1)][Flags(1)][Length(4)][Timestamp(8)][User ID(16)][Data(variable)]
            
            uint magic = 0xDEADBEEF;
            ushort version = 0x0102; // v1.2
            byte messageType = 0x05;
            byte flags = 0b10110001; // Binary flags
            DateTime timestamp = DateTime.UtcNow;
            Guid userId = Guid.NewGuid();
            string payload = "Complex binary message payload with unicode: 你好世界! 🌍";
            
            // Build the message
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
            uint totalLength = (uint)(4 + 2 + 1 + 1 + 4 + 8 + 16 + payloadBytes.Length);
            
            List<byte> complexMessage = new List<byte>();
            complexMessage.AddRange(BitConverter.GetBytes(magic));
            complexMessage.AddRange(BitConverter.GetBytes(version));
            complexMessage.Add(messageType);
            complexMessage.Add(flags);
            complexMessage.AddRange(BitConverter.GetBytes(totalLength));
            complexMessage.AddRange(BitConverter.GetBytes(timestamp.ToBinary()));
            complexMessage.AddRange(userId.ToByteArray());
            complexMessage.AddRange(payloadBytes);
            
            Console.WriteLine($"  Built message: {complexMessage.Count} bytes");
            Console.WriteLine($"  Magic + Version: {string.Join(" ", complexMessage.Take(6).Select(b => $"{b:X2}"))}");
            
            // Parse the message back
            byte[] messageBytes = complexMessage.ToArray();
            int parseOffset = 0;
            
            uint parsedMagic = BitConverter.ToUInt32(messageBytes, parseOffset); parseOffset += 4;
            ushort parsedVersion = BitConverter.ToUInt16(messageBytes, parseOffset); parseOffset += 2;
            byte parsedType = messageBytes[parseOffset++];
            byte parsedFlags = messageBytes[parseOffset++];
            uint parsedLength = BitConverter.ToUInt32(messageBytes, parseOffset); parseOffset += 4;
            long parsedTimeBinary = BitConverter.ToInt64(messageBytes, parseOffset); parseOffset += 8;
            DateTime parsedTime = DateTime.FromBinary(parsedTimeBinary);
            
            byte[] userIdBytes = new byte[16];
            Array.Copy(messageBytes, parseOffset, userIdBytes, 0, 16);
            Guid parsedUserId = new Guid(userIdBytes); parseOffset += 16;
            
            int payloadLength = (int)(parsedLength - parseOffset);
            string parsedPayload = Encoding.UTF8.GetString(messageBytes, parseOffset, payloadLength);
            
            Console.WriteLine($"  Parsed results:");
            Console.WriteLine($"    Magic: 0x{parsedMagic:X8} (valid: {parsedMagic == magic})");
            Console.WriteLine($"    Version: {parsedVersion >> 8}.{parsedVersion & 0xFF}");
            Console.WriteLine($"    Type: 0x{parsedType:X2}");
            Console.WriteLine($"    Flags: 0b{Convert.ToString(parsedFlags, 2).PadLeft(8, '0')}");
            Console.WriteLine($"    Length: {parsedLength} bytes");
            Console.WriteLine($"    Timestamp: {parsedTime:yyyy-MM-dd HH:mm:ss.fff} UTC");
            Console.WriteLine($"    User ID: {parsedUserId}");
            Console.WriteLine($"    Payload: \"{parsedPayload}\"");

            // Scenario 3: Data export with multiple formats
            Console.WriteLine("\n\nScenario 3: Multi-format data export system");
            Console.WriteLine("Exporting the same data in different formats:");
            
            var exportData = new[]
            {
                new { Id = 1001, Name = "Alice Johnson", Score = 95.75, Active = true, LastLogin = DateTime.Now.AddDays(-2) },
                new { Id = 1002, Name = "Bob Smith", Score = 88.25, Active = false, LastLogin = DateTime.Now.AddDays(-10) },
                new { Id = 1003, Name = "Charlie Brown", Score = 92.50, Active = true, LastLogin = DateTime.Now.AddHours(-3) }
            };

            // CSV format (machine-readable, invariant culture)
            Console.WriteLine("\nCSV Export (InvariantCulture):");
            Console.WriteLine("ID,Name,Score,Active,LastLogin");
            foreach (var record in exportData)
            {
                string csvLine = string.Join(",",
                    record.Id.ToString(),
                    $"\"{record.Name}\"",
                    record.Score.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                    XmlConvert.ToString(record.Active),
                    XmlConvert.ToString(record.LastLogin, XmlDateTimeSerializationMode.RoundtripKind)
                );
                Console.WriteLine(csvLine);
            }

            // JSON-like format with Base64 encoded metadata
            Console.WriteLine("\nJSON-like Export with Base64 metadata:");
            foreach (var record in exportData)
            {
                // Create metadata
                var metadata = new { ExportTime = DateTime.UtcNow, Version = "1.0", Source = "DemoSystem" };
                string metadataJson = $"{{\"exportTime\":\"{XmlConvert.ToString(metadata.ExportTime, XmlDateTimeSerializationMode.Utc)}\",\"version\":\"{metadata.Version}\",\"source\":\"{metadata.Source}\"}}";
                string metadataBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(metadataJson));
                
                Console.WriteLine($"{{");
                Console.WriteLine($"  \"id\": {record.Id},");
                Console.WriteLine($"  \"name\": \"{record.Name}\",");
                Console.WriteLine($"  \"score\": {record.Score.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)},");
                Console.WriteLine($"  \"active\": {XmlConvert.ToString(record.Active)},");
                Console.WriteLine($"  \"lastLogin\": \"{XmlConvert.ToString(record.LastLogin, XmlDateTimeSerializationMode.RoundtripKind)}\",");
                Console.WriteLine($"  \"_meta\": \"{metadataBase64}\"");
                Console.WriteLine($"}},");
            }

            // Binary format for high-performance scenarios
            Console.WriteLine("\nBinary Export (compact representation):");
            foreach (var record in exportData)
            {
                List<byte> binaryRecord = new List<byte>();
                binaryRecord.AddRange(BitConverter.GetBytes(record.Id));
                
                byte[] nameBytes = Encoding.UTF8.GetBytes(record.Name);
                binaryRecord.Add((byte)nameBytes.Length);
                binaryRecord.AddRange(nameBytes);
                
                binaryRecord.AddRange(BitConverter.GetBytes((float)record.Score));
                binaryRecord.Add((byte)(record.Active ? 1 : 0));
                binaryRecord.AddRange(BitConverter.GetBytes(record.LastLogin.ToBinary()));
                
                string hexDump = string.Join(" ", binaryRecord.Select(b => $"{b:X2}"));
                Console.WriteLine($"  Record {record.Id}: {binaryRecord.Count} bytes - {hexDump}");
            }

            Console.WriteLine("\nKey Insights:");
            Console.WriteLine("1. Different scenarios require different conversion approaches");
            Console.WriteLine("2. Combine multiple conversion mechanisms for complex requirements");
            Console.WriteLine("3. Always consider culture, endianness, and data integrity");
            Console.WriteLine("4. Choose the right tool: Convert, XmlConvert, BitConverter, TypeConverter");
            Console.WriteLine("5. Plan for error handling and data validation in production systems");

            Console.WriteLine();
        }

        // Helper methods for advanced scenarios
        static string ProcessBase64Config(string base64Value)
        {
            byte[] decoded = Convert.FromBase64String(base64Value);
            return Encoding.UTF8.GetString(decoded);
        }

        static int ProcessBinaryFlags(string binaryValue)
        {
            return Convert.ToInt32(binaryValue, 2);
        }

        static List<int> GetEnabledCores(int flags)
        {
            var cores = new List<int>();
            for (int i = 0; i < 32; i++)
            {
                if ((flags & (1 << i)) != 0)
                {
                    cores.Add(i);
                }
            }
            return cores;
        }

        // Helper method for dynamic conversion demonstration
        static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
