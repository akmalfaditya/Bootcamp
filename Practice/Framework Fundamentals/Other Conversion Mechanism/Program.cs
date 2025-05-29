using System.ComponentModel;
using System.Drawing;
using System.Xml;

// Other Conversion Mechanisms Demonstration
// This project covers advanced conversion techniques beyond basic ToString() and Parse()

namespace OtherConversionMechanism
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== OTHER CONVERSION MECHANISMS DEMONSTRATION ===\n");

            // Let's explore the various conversion tools available in .NET
            DemonstrateConvertClass();
            DemonstrateRoundingConversions();
            DemonstrateBaseConversions();
            DemonstrateDynamicConversions();
            DemonstrateBase64Conversions();
            DemonstrateXmlConvert();
            DemonstrateTypeConverters();
            DemonstrateBitConverter();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== END OF DEMONSTRATION ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DemonstrateConvertClass()
        {
            Console.WriteLine("1. CONVERT CLASS - GENERAL PURPOSE CONVERSIONS");
            Console.WriteLine("==============================================");

            // The Convert class is your Swiss Army knife for type conversions
            // It handles a lot of edge cases that simple casting doesn't

            Console.WriteLine("Basic Convert class usage:");
            
            // Converting between common types
            string numberString = "42";
            string doubleString = "3.14159";
            string boolString = "true";

            int convertedInt = Convert.ToInt32(numberString);
            double convertedDouble = Convert.ToDouble(doubleString);
            bool convertedBool = Convert.ToBoolean(boolString);

            Console.WriteLine($"  String \"{numberString}\" -> int {convertedInt}");
            Console.WriteLine($"  String \"{doubleString}\" -> double {convertedDouble}");
            Console.WriteLine($"  String \"{boolString}\" -> bool {convertedBool}");

            // Converting between numeric types
            decimal money = 123.456m;
            float precision = 789.012f;
            long bigNumber = 9876543210L;

            Console.WriteLine("\nNumeric type conversions:");
            Console.WriteLine($"  decimal {money} -> int {Convert.ToInt32(money)}");
            Console.WriteLine($"  float {precision} -> int {Convert.ToInt32(precision)}");
            Console.WriteLine($"  long {bigNumber} -> int {Convert.ToInt32(bigNumber)}");

            // Handling null values gracefully
            Console.WriteLine("\nHandling null values:");
            string nullString = null;
            object nullObject = null;

            // Convert handles nulls better than direct parsing
            int defaultInt = Convert.ToInt32(nullString); // Returns 0, doesn't throw
            bool defaultBool = Convert.ToBoolean(nullObject); // Returns false

            Console.WriteLine($"  null string -> int: {defaultInt}");
            Console.WriteLine($"  null object -> bool: {defaultBool}");

            Console.WriteLine();
        }

        static void DemonstrateRoundingConversions()
        {
            Console.WriteLine("2. ROUNDING CONVERSIONS - BANKER'S ROUNDING");
            Console.WriteLine("===========================================");

            // Convert uses "banker's rounding" - rounds to nearest even number for .5 values
            // This reduces systematic bias in calculations

            double[] testValues = { 2.1, 2.5, 2.9, 3.5, 4.5, 5.5 };

            Console.WriteLine("Comparing casting vs Convert rounding:");
            Console.WriteLine("Value    Cast    Convert");
            Console.WriteLine("-----    ----    -------");

            foreach (double value in testValues)
            {
                int castResult = (int)value;           // Truncates (always rounds down)
                int convertResult = Convert.ToInt32(value); // Banker's rounding

                Console.WriteLine($"{value,-7}  {castResult,-6}  {convertResult}");
            }

            // Real-world example: Financial calculations
            Console.WriteLine("\nFinancial calculation example:");
            
            double[] payments = { 10.235, 15.675, 23.125, 8.945 };
            double castTotal = 0;
            double convertTotal = 0;

            Console.WriteLine("Payment amounts and rounding differences:");
            foreach (double payment in payments)
            {
                int castCents = (int)(payment * 100);
                int convertCents = Convert.ToInt32(payment * 100);
                
                castTotal += castCents / 100.0;
                convertTotal += convertCents / 100.0;

                Console.WriteLine($"  ${payment:F3} -> Cast: {castCents}¢, Convert: {convertCents}¢");
            }

            Console.WriteLine($"\nTotal with casting: ${castTotal:F2}");
            Console.WriteLine($"Total with Convert: ${convertTotal:F2}");
            Console.WriteLine($"Difference: ${Math.Abs(castTotal - convertTotal):F2}");

            Console.WriteLine();
        }

        static void DemonstrateBaseConversions()
        {
            Console.WriteLine("3. BASE CONVERSIONS - BINARY, OCTAL, HEXADECIMAL");
            Console.WriteLine("=================================================");

            // Convert class can parse numbers in different bases
            // Very useful for low-level programming and data processing

            Console.WriteLine("Parsing numbers in different bases:");

            // Hexadecimal (base 16) - commonly used in programming
            string hexValue = "FF";
            int fromHex = Convert.ToInt32(hexValue, 16);
            Console.WriteLine($"  Hex \"{hexValue}\" (base 16) -> decimal {fromHex}");

            // Binary (base 2) - useful for bit manipulation
            string binaryValue = "11110000";
            int fromBinary = Convert.ToInt32(binaryValue, 2);
            Console.WriteLine($"  Binary \"{binaryValue}\" (base 2) -> decimal {fromBinary}");

            // Octal (base 8) - less common but still used
            string octalValue = "755";
            int fromOctal = Convert.ToInt32(octalValue, 8);
            Console.WriteLine($"  Octal \"{octalValue}\" (base 8) -> decimal {fromOctal}");

            // Converting back to different bases
            Console.WriteLine("\nConverting decimal to different bases:");
            int decimalValue = 255;
            
            string toHex = Convert.ToString(decimalValue, 16).ToUpper();
            string toBinary = Convert.ToString(decimalValue, 2);
            string toOctal = Convert.ToString(decimalValue, 8);

            Console.WriteLine($"  Decimal {decimalValue}:");
            Console.WriteLine($"    -> Hex: {toHex}");
            Console.WriteLine($"    -> Binary: {toBinary}");
            Console.WriteLine($"    -> Octal: {toOctal}");

            // Practical example: Color values
            Console.WriteLine("\nPractical example - RGB color values:");
            string[] colorComponents = { "FF", "A0", "7F" }; // Red, Green, Blue in hex
            
            Console.Write("  RGB color components: ");
            for (int i = 0; i < colorComponents.Length; i++)
            {
                int component = Convert.ToInt32(colorComponents[i], 16);
                Console.Write($"{colorComponents[i]}({component})");
                if (i < colorComponents.Length - 1) Console.Write(", ");
            }
            Console.WriteLine();

            // File permissions example (Unix-style)
            Console.WriteLine("\nFile permissions example (Unix octal):");
            string[] permissions = { "644", "755", "777" };
            string[] descriptions = { "rw-r--r--", "rwxr-xr-x", "rwxrwxrwx" };

            for (int i = 0; i < permissions.Length; i++)
            {
                int octalPerm = Convert.ToInt32(permissions[i], 8);
                Console.WriteLine($"  {permissions[i]} (octal) = {octalPerm} (decimal) = {descriptions[i]}");
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

        static void DemonstrateBase64Conversions()
        {
            Console.WriteLine("5. BASE64 CONVERSIONS - BINARY DATA AS TEXT");
            Console.WriteLine("===========================================");

            // Base64 is essential for representing binary data as text
            // Used in email attachments, web APIs, embedded images, etc.

            Console.WriteLine("Basic Base64 encoding/decoding:");

            // Convert text to bytes, then to Base64
            string originalText = "Hello, World! 🌍";
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(originalText);
            string base64Text = Convert.ToBase64String(textBytes);

            Console.WriteLine($"  Original: \"{originalText}\"");
            Console.WriteLine($"  Base64: {base64Text}");

            // Decode back to original
            byte[] decodedBytes = Convert.FromBase64String(base64Text);
            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
            Console.WriteLine($"  Decoded: \"{decodedText}\"");

            // Working with binary data
            Console.WriteLine("\nEncoding binary data:");
            
            byte[] binaryData = { 0xFF, 0xA0, 0x7F, 0x00, 0x80, 0x40 };
            string binaryBase64 = Convert.ToBase64String(binaryData);
            
            Console.WriteLine($"  Binary data: [{string.Join(", ", binaryData.Select(b => $"0x{b:X2}"))}]");
            Console.WriteLine($"  Base64: {binaryBase64}");

            // Practical example: Simulating file embedding
            Console.WriteLine("\nPractical example - embedding small files:");
            
            // Simulate a small image file (just random bytes for demo)
            byte[] imageData = new byte[20];
            new Random().NextBytes(imageData);
            
            string embeddedImage = Convert.ToBase64String(imageData);
            Console.WriteLine($"  'Image' data ({imageData.Length} bytes): {embeddedImage}");
            
            // This is how you'd embed it in HTML/CSS
            Console.WriteLine($"  HTML img tag: <img src=\"data:image/png;base64,{embeddedImage}\" />");

            // Working with larger data and line breaks
            Console.WriteLine("\nBase64 with line breaks (for readability):");
            
            byte[] largeData = new byte[60];
            new Random(42).NextBytes(largeData); // Use seed for consistent output
            
            string base64NoBreaks = Convert.ToBase64String(largeData);
            string base64WithBreaks = Convert.ToBase64String(largeData, Base64FormattingOptions.InsertLineBreaks);
            
            Console.WriteLine($"  Without breaks: {base64NoBreaks}");
            Console.WriteLine($"  With breaks:\n{base64WithBreaks}");

            Console.WriteLine();
        }

        static void DemonstrateXmlConvert()
        {
            Console.WriteLine("6. XMLCONVERT - XML-SPECIFIC CONVERSIONS");
            Console.WriteLine("========================================");

            // XmlConvert ensures data is formatted according to XML standards
            // Important for XML serialization and web services

            Console.WriteLine("Basic XmlConvert operations:");

            // Boolean conversion - XML uses lowercase
            bool flag = true;
            string xmlBool = XmlConvert.ToString(flag);
            bool parsedBool = XmlConvert.ToBoolean(xmlBool);
            
            Console.WriteLine($"  Boolean {flag} -> XML: \"{xmlBool}\" -> parsed: {parsedBool}");

            // DateTime conversion with timezone handling
            DateTime now = DateTime.Now;
            DateTime utcNow = DateTime.UtcNow;
            
            Console.WriteLine("\nDateTime XML formatting:");
            
            string localXml = XmlConvert.ToString(now, XmlDateTimeSerializationMode.Local);
            string utcXml = XmlConvert.ToString(utcNow, XmlDateTimeSerializationMode.Utc);
            string unspecifiedXml = XmlConvert.ToString(now, XmlDateTimeSerializationMode.Unspecified);
            
            Console.WriteLine($"  Local time: {localXml}");
            Console.WriteLine($"  UTC time: {utcXml}");
            Console.WriteLine($"  Unspecified: {unspecifiedXml}");

            // Numeric conversions
            Console.WriteLine("\nNumeric XML conversions:");
            
            decimal price = 123.45m;
            double scientific = 1.23e-4;
            float singlePrecision = 456.789f;
            
            Console.WriteLine($"  Decimal: {XmlConvert.ToString(price)}");
            Console.WriteLine($"  Double: {XmlConvert.ToString(scientific)}");
            Console.WriteLine($"  Float: {XmlConvert.ToString(singlePrecision)}");

            // Special values handling
            Console.WriteLine("\nSpecial numeric values:");
            
            double positiveInfinity = double.PositiveInfinity;
            double negativeInfinity = double.NegativeInfinity;
            double notANumber = double.NaN;
            
            Console.WriteLine($"  Positive infinity: {XmlConvert.ToString(positiveInfinity)}");
            Console.WriteLine($"  Negative infinity: {XmlConvert.ToString(negativeInfinity)}");
            Console.WriteLine($"  NaN: {XmlConvert.ToString(notANumber)}");

            // Parsing XML date formats
            Console.WriteLine("\nParsing XML date strings:");
            
            string[] xmlDates = {
                "2024-05-29T14:30:00Z",           // UTC
                "2024-05-29T14:30:00+07:00",     // With timezone
                "2024-05-29T14:30:00.123Z"       // With milliseconds
            };

            foreach (string xmlDate in xmlDates)
            {
                try
                {
                    DateTime parsed = XmlConvert.ToDateTime(xmlDate, XmlDateTimeSerializationMode.RoundtripKind);
                    Console.WriteLine($"  \"{xmlDate}\" -> {parsed} (Kind: {parsed.Kind})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  \"{xmlDate}\" -> Error: {ex.Message}");
                }
            }

            Console.WriteLine();
        }

        static void DemonstrateTypeConverters()
        {
            Console.WriteLine("7. TYPE CONVERTERS - DESIGN-TIME CONVERSIONS");
            Console.WriteLine("============================================");

            // TypeConverters are mainly used in design environments and XAML
            // They provide intelligent string-to-object conversion

            Console.WriteLine("Color type converter examples:");

            try
            {
                TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));
                
                // Converting color names
                string[] colorNames = { "Red", "Blue", "Beige", "Transparent", "DarkSlateGray" };
                
                foreach (string colorName in colorNames)
                {
                    if (colorConverter.CanConvertFrom(typeof(string)))
                    {
                        Color color = (Color)colorConverter.ConvertFromString(colorName);
                        Console.WriteLine($"  \"{colorName}\" -> R:{color.R}, G:{color.G}, B:{color.B}, A:{color.A}");
                    }
                }

                // Converting RGB values
                Console.WriteLine("\nRGB string conversions:");
                string[] rgbStrings = { "255, 0, 0", "0, 255, 0", "0, 0, 255" };
                
                foreach (string rgb in rgbStrings)
                {
                    try
                    {
                        Color rgbColor = (Color)colorConverter.ConvertFromString(rgb);
                        Console.WriteLine($"  \"{rgb}\" -> {rgbColor.Name} (R:{rgbColor.R}, G:{rgbColor.G}, B:{rgbColor.B})");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  \"{rgb}\" -> Error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Color converter not available: {ex.Message}");
            }

            // Exploring other type converters
            Console.WriteLine("\nOther type converters:");
            
            Type[] typesToTest = { typeof(int), typeof(DateTime), typeof(bool), typeof(decimal) };
            
            foreach (Type type in typesToTest)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                Console.WriteLine($"  {type.Name}: Can convert from string: {converter.CanConvertFrom(typeof(string))}");
                
                if (converter.CanConvertFrom(typeof(string)))
                {
                    try
                    {
                        // Test with sample values
                        string testValue = type.Name switch
                        {
                            "Int32" => "42",
                            "DateTime" => "2024-05-29",
                            "Boolean" => "true",
                            "Decimal" => "123.45",
                            _ => "test"
                        };
                        
                        object converted = converter.ConvertFromString(testValue);
                        Console.WriteLine($"    Example: \"{testValue}\" -> {converted}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    Conversion failed: {ex.Message}");
                    }
                }
            }

            Console.WriteLine();
        }

        static void DemonstrateBitConverter()
        {
            Console.WriteLine("8. BITCONVERTER - LOW-LEVEL BINARY CONVERSIONS");
            Console.WriteLine("==============================================");

            // BitConverter works with the binary representation of data types
            // Essential for network protocols, file formats, and serialization

            Console.WriteLine("Converting values to byte arrays:");

            // Basic type conversions
            int intValue = 1234567890;
            float floatValue = 3.14159f;
            double doubleValue = 2.718281828459045;
            bool boolValue = true;

            byte[] intBytes = BitConverter.GetBytes(intValue);
            byte[] floatBytes = BitConverter.GetBytes(floatValue);
            byte[] doubleBytes = BitConverter.GetBytes(doubleValue);
            byte[] boolBytes = BitConverter.GetBytes(boolValue);

            Console.WriteLine($"  int {intValue}: [{string.Join(", ", intBytes)}]");
            Console.WriteLine($"  float {floatValue}: [{string.Join(", ", floatBytes)}]");
            Console.WriteLine($"  double {doubleValue:F6}: [{string.Join(", ", doubleBytes.Take(8))}]");
            Console.WriteLine($"  bool {boolValue}: [{string.Join(", ", boolBytes)}]");

            // Converting back from bytes
            Console.WriteLine("\nConverting byte arrays back to values:");
            
            int recoveredInt = BitConverter.ToInt32(intBytes, 0);
            float recoveredFloat = BitConverter.ToSingle(floatBytes, 0);
            double recoveredDouble = BitConverter.ToDouble(doubleBytes, 0);
            bool recoveredBool = BitConverter.ToBoolean(boolBytes, 0);

            Console.WriteLine($"  Recovered int: {recoveredInt} (match: {recoveredInt == intValue})");
            Console.WriteLine($"  Recovered float: {recoveredFloat} (match: {recoveredFloat == floatValue})");
            Console.WriteLine($"  Recovered double: {recoveredDouble:F6} (match: {recoveredDouble == doubleValue})");
            Console.WriteLine($"  Recovered bool: {recoveredBool} (match: {recoveredBool == boolValue})");

            // Endianness awareness
            Console.WriteLine($"\nSystem endianness: {(BitConverter.IsLittleEndian ? "Little Endian" : "Big Endian")}");

            // Practical example: Network packet structure
            Console.WriteLine("\nPractical example - Network packet simulation:");
            
            // Simulate a simple packet: [Header(4 bytes)][Length(4 bytes)][Data(variable)]
            uint packetHeader = 0x12345678;
            uint dataLength = 100;
            
            byte[] headerBytes = BitConverter.GetBytes(packetHeader);
            byte[] lengthBytes = BitConverter.GetBytes(dataLength);
            
            Console.WriteLine($"  Packet header 0x{packetHeader:X8}: [{string.Join(", ", headerBytes.Select(b => $"0x{b:X2}"))}]");
            Console.WriteLine($"  Data length {dataLength}: [{string.Join(", ", lengthBytes)}]");

            // DateTime binary serialization
            Console.WriteLine("\nDateTime binary serialization:");
            
            DateTime timestamp = DateTime.Now;
            long binaryTime = timestamp.ToBinary();
            byte[] timeBytes = BitConverter.GetBytes(binaryTime);
            
            Console.WriteLine($"  Original: {timestamp}");
            Console.WriteLine($"  Binary representation: {binaryTime}");
            Console.WriteLine($"  As bytes: [{string.Join(", ", timeBytes.Take(8))}]");
            
            // Recover the DateTime
            long recoveredBinary = BitConverter.ToInt64(timeBytes, 0);
            DateTime recoveredTime = DateTime.FromBinary(recoveredBinary);
            Console.WriteLine($"  Recovered: {recoveredTime}");

            Console.WriteLine();
        }

        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("9. REAL-WORLD SCENARIOS");
            Console.WriteLine("========================");

            // Scenario 1: Configuration file processing with different data types
            Console.WriteLine("Scenario 1: Processing mixed configuration data");
            
            var configData = new Dictionary<string, (string value, Type targetType)>
            {
                {"server_port", ("8080", typeof(int))},
                {"timeout_seconds", ("30.5", typeof(double))},
                {"enable_logging", ("true", typeof(bool))},
                {"max_connections", ("FF", typeof(int))}, // Hex value
                {"api_key", ("QWxhZGRpbjpvcGVuIHNlc2FtZQ==", typeof(string))}, // Base64
                {"start_date", ("2024-05-29T10:00:00Z", typeof(DateTime))}
            };

            foreach (var config in configData)
            {
                Console.WriteLine($"\n  Processing {config.Key}: \"{config.Value.value}\"");
                
                try
                {
                    object result;
                    
                    // Special handling for hex numbers
                    if (config.Key.Contains("connections") && config.Value.targetType == typeof(int))
                    {
                        result = Convert.ToInt32(config.Value.value, 16);
                        Console.WriteLine($"    Hex conversion: {result}");
                    }
                    // Special handling for Base64
                    else if (config.Key.Contains("key"))
                    {
                        byte[] decoded = Convert.FromBase64String(config.Value.value);
                        result = System.Text.Encoding.UTF8.GetString(decoded);
                        Console.WriteLine($"    Base64 decoded: {result}");
                    }
                    // Special handling for XML DateTime
                    else if (config.Value.targetType == typeof(DateTime))
                    {
                        result = XmlConvert.ToDateTime(config.Value.value, XmlDateTimeSerializationMode.RoundtripKind);
                        Console.WriteLine($"    XML DateTime: {result}");
                    }
                    // General conversion
                    else
                    {
                        result = Convert.ChangeType(config.Value.value, config.Value.targetType);
                        Console.WriteLine($"    Converted to {config.Value.targetType.Name}: {result}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    Conversion failed: {ex.Message}");
                }
            }

            // Scenario 2: Binary protocol implementation
            Console.WriteLine("\n\nScenario 2: Binary protocol message parsing");
            
            // Simulate a binary message: [Type(1 byte)][ID(4 bytes)][Timestamp(8 bytes)][Data length(4 bytes)][Data]
            byte messageType = 0x01;
            uint messageId = 12345;
            DateTime timestamp = DateTime.UtcNow;
            string messageData = "Hello, Binary World!";
            
            // Build the binary message
            List<byte> message = new List<byte>();
            message.Add(messageType);
            message.AddRange(BitConverter.GetBytes(messageId));
            message.AddRange(BitConverter.GetBytes(timestamp.ToBinary()));
            
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(messageData);
            message.AddRange(BitConverter.GetBytes((uint)dataBytes.Length));
            message.AddRange(dataBytes);
            
            Console.WriteLine($"  Built binary message ({message.Count} bytes)");
            
            // Parse the binary message
            byte[] messageArray = message.ToArray();
            int offset = 0;
            
            byte parsedType = messageArray[offset++];
            uint parsedId = BitConverter.ToUInt32(messageArray, offset); offset += 4;
            long parsedTimeBinary = BitConverter.ToInt64(messageArray, offset); offset += 8;
            DateTime parsedTime = DateTime.FromBinary(parsedTimeBinary);
            uint parsedDataLength = BitConverter.ToUInt32(messageArray, offset); offset += 4;
            string parsedData = System.Text.Encoding.UTF8.GetString(messageArray, offset, (int)parsedDataLength);
            
            Console.WriteLine($"  Parsed message:");
            Console.WriteLine($"    Type: 0x{parsedType:X2}");
            Console.WriteLine($"    ID: {parsedId}");
            Console.WriteLine($"    Timestamp: {parsedTime:yyyy-MM-dd HH:mm:ss.fff} UTC");
            Console.WriteLine($"    Data: \"{parsedData}\"");

            // Scenario 3: Web API data transformation
            Console.WriteLine("\n\nScenario 3: Web API response processing");
            
            // Simulate processing different API response formats
            var apiResponses = new[]
            {
                ("user_id", "123"),
                ("birth_date", "1990-06-15"),
                ("is_active", "true"),
                ("balance", "1234.56"),
                ("profile_image", "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg=="), // Tiny PNG
                ("permissions", "7F"), // Hex flags
                ("last_login", "2024-05-29T08:30:00.000Z")
            };

            Console.WriteLine("  Processing API response fields:");
            
            foreach (var (field, value) in apiResponses)
            {
                Console.WriteLine($"\n    {field}: \"{value}\"");
                
                try
                {
                    switch (field)
                    {
                        case "user_id":
                            int userId = Convert.ToInt32(value);
                            Console.WriteLine($"      -> User ID: {userId}");
                            break;
                            
                        case "birth_date":
                            DateTime birthDate = DateTime.Parse(value);
                            int age = DateTime.Now.Year - birthDate.Year;
                            Console.WriteLine($"      -> Birth Date: {birthDate:yyyy-MM-dd} (Age: {age})");
                            break;
                            
                        case "is_active":
                            bool isActive = Convert.ToBoolean(value);
                            Console.WriteLine($"      -> Active Status: {isActive}");
                            break;
                            
                        case "balance":
                            decimal balance = Convert.ToDecimal(value);
                            Console.WriteLine($"      -> Account Balance: {balance:C}");
                            break;
                            
                        case "profile_image":
                            byte[] imageData = Convert.FromBase64String(value);
                            Console.WriteLine($"      -> Image Data: {imageData.Length} bytes");
                            break;
                            
                        case "permissions":
                            int permissions = Convert.ToInt32(value, 16);
                            string binary = Convert.ToString(permissions, 2).PadLeft(8, '0');
                            Console.WriteLine($"      -> Permissions: 0x{permissions:X2} (binary: {binary})");
                            break;
                            
                        case "last_login":
                            DateTime lastLogin = XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind);
                            TimeSpan timeSince = DateTime.UtcNow - lastLogin;
                            Console.WriteLine($"      -> Last Login: {lastLogin:yyyy-MM-dd HH:mm:ss} UTC ({timeSince.TotalHours:F1} hours ago)");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"      -> Conversion error: {ex.Message}");
                }
            }

            Console.WriteLine();
        }

        // Helper method for dynamic conversion demonstration
        static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
