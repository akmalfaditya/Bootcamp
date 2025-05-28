using System.Text; // For StringBuilder class

namespace StringsAndCharactersDemo
{
    class Program
    {
        // Main entry point - orchestrates all demonstrations
        // Each method focuses on a specific aspect of string/character handling
        static void Main()
        {
            Console.WriteLine("=== STRINGS AND CHARACTERS IN C# - COMPLETE GUIDE ===\n");

            // Character fundamentals - single Unicode characters
            DemonstrateCharacterBasics();

            // Escape sequences - handling special characters
            DemonstrateEscapeSequences();

            // Character conversions - working with Unicode values
            DemonstrateCharacterConversions();

            // String fundamentals - sequences of characters
            DemonstrateStringBasics();

            // String equality - value vs reference comparison
            DemonstrateStringEquality();

            // String escape sequences - special characters in strings
            DemonstrateStringEscapeSequences();

            // Verbatim strings - literal string handling
            DemonstrateVerbatimStrings();

            // Raw string literals - modern approach to complex strings
            DemonstrateRawStringLiterals();

            // String concatenation - joining strings together
            DemonstrateStringConcatenation();

            // String interpolation - embedding expressions in strings
            DemonstrateStringInterpolation();

            // String comparison - equality and ordering
            DemonstrateStringComparisons();

            // UTF-8 strings - performance-optimized text handling
            DemonstrateUTF8Strings();

            // Real-world examples - practical applications
            DemonstratePracticalExamples();

            Console.WriteLine("\n=== END OF DEMONSTRATION ===");
        }

        // Character type fundamentals
        // The char type stores a single Unicode character (16-bit UTF-16)
        // Remember: char uses single quotes, string uses double quotes
        static void DemonstrateCharacterBasics()
        {
            Console.WriteLine("1. CHARACTER TYPE BASICS");
            Console.WriteLine("========================");

            // Basic character declaration - always use single quotes
            char letter = 'A';
            char digit = '7';
            char symbol = '$';
            char space = ' ';

            Console.WriteLine($"Letter: '{letter}'");
            Console.WriteLine($"Digit: '{digit}'");
            Console.WriteLine($"Symbol: '{symbol}'");
            Console.WriteLine($"Space: '{space}' (invisible but there!)");

            // Unicode characters - C# supports the full Unicode range
            char heart = '♥';           // Direct Unicode input
            char smiley = '☺';          // Another Unicode symbol
            char chinese = '中';         // Chinese character
            char arabic = 'ع';          // Arabic letter

            Console.WriteLine($"Heart: {heart}");
            Console.WriteLine($"Smiley: {smiley}");
            Console.WriteLine($"Chinese: {chinese}");
            Console.WriteLine($"Arabic: {arabic}");

            // Character storage details
            Console.WriteLine($"\nCharacter storage info:");
            Console.WriteLine($"Size of char: {sizeof(char)} bytes");
            Console.WriteLine($"Min char value: {(int)char.MinValue}");
            Console.WriteLine($"Max char value: {(int)char.MaxValue}");

            Console.WriteLine();
        }

        // Escape sequences - representing special characters
        // When you need characters that are hard to type or have special meaning
        static void DemonstrateEscapeSequences()
        {
            Console.WriteLine("2. ESCAPE SEQUENCES");
            Console.WriteLine("===================");

            // Common escape sequences - memorize these, you'll use them often
            char singleQuote = '\'';    // Escape single quote inside char
            char doubleQuote = '\"';    // Escape double quote
            char backslash = '\\';      // Escape backslash itself
            char nullChar = '\0';       // Null character (string terminator in C)
            char newLine = '\n';        // New line (line break)
            char tab = '\t';            // Horizontal tab (indentation)

            Console.WriteLine("Common escape sequences:");
            Console.WriteLine($"Single quote: {singleQuote}");
            Console.WriteLine($"Double quote: {doubleQuote}");
            Console.WriteLine($"Backslash: {backslash}");
            Console.WriteLine($"Null character: '{nullChar}' (usually invisible)");
            Console.WriteLine($"After this comes a new line:{newLine}See? New line worked!");
            Console.WriteLine($"Tab example:{tab}This text is tabbed over");

            // Unicode escape sequences - specify characters by their Unicode code point
            // Format: \uXXXX where XXXX is the 4-digit hex Unicode value
            char copyrightSymbol = '\u00A9';  // © symbol
            char omegaSymbol = '\u03A9';      // Ω (Greek letter Omega)
            char euroSymbol = '\u20AC';       // € symbol
            char checkMark = '\u2713';        // ✓ symbol

            Console.WriteLine("\nUnicode escape sequences:");
            Console.WriteLine($"Copyright: {copyrightSymbol} (\\u00A9)");
            Console.WriteLine($"Omega: {omegaSymbol} (\\u03A9)");
            Console.WriteLine($"Euro: {euroSymbol} (\\u20AC)");
            Console.WriteLine($"Check mark: {checkMark} (\\u2713)");

            Console.WriteLine();
        }

        // Character conversions - chars are basically 16-bit numbers
        // Understanding this helps with text processing and character manipulation
        static void DemonstrateCharacterConversions()
        {
            Console.WriteLine("3. CHARACTER CONVERSIONS");
            Console.WriteLine("========================");

            // Implicit conversion from char to larger numeric types
            // This works because char fits into larger types without data loss
            char letterA = 'A';
            int asciiValue = letterA;        // Implicit conversion - no cast needed
            long longValue = letterA;        // Also implicit
            double doubleValue = letterA;    // Also implicit

            Console.WriteLine($"Character 'A' conversions:");
            Console.WriteLine($"As char: {letterA}");
            Console.WriteLine($"As int: {asciiValue}");
            Console.WriteLine($"As long: {longValue}");
            Console.WriteLine($"As double: {doubleValue}");

            // Explicit conversion from numbers back to char
            // Need casting because larger types might not fit in char
            int number = 66;
            char letterFromNumber = (char)number;  // Explicit cast required
            Console.WriteLine($"\nNumber {number} as char: {letterFromNumber}");

            // Working with character ranges - useful for validation
            char[] alphabet = new char[26];
            for (int i = 0; i < 26; i++)
            {
                alphabet[i] = (char)('A' + i);  // Generate A-Z
            }
            Console.WriteLine($"Generated alphabet: {string.Join(", ", alphabet)}");

            // Character classification - these methods are incredibly useful
            char[] testChars = { 'A', 'a', '5', ' ', '#', '\n' };
            Console.WriteLine("\nCharacter classification:");
            foreach (char c in testChars)
            {
                string displayChar = c == '\n' ? "\\n" : c.ToString();
                Console.WriteLine($"'{displayChar}': " +
                    $"Letter={char.IsLetter(c)}, " +
                    $"Digit={char.IsDigit(c)}, " +
                    $"WhiteSpace={char.IsWhiteSpace(c)}, " +
                    $"Upper={char.IsUpper(c)}, " +
                    $"Lower={char.IsLower(c)}");
            }

            Console.WriteLine();
        }

        // String basics - sequences of characters
        // Strings are reference types but behave like value types in many ways
        static void DemonstrateStringBasics()
        {
            Console.WriteLine("4. STRING TYPE BASICS");
            Console.WriteLine("=====================");

            // String declaration - use double quotes (not single!)
            string greeting = "Hello, World!";
            string empty = "";              // Empty string (not null)
            string nullString = null;       // Null reference
            string fromChars = new string('X', 5);  // "XXXXX" - repeat character

            Console.WriteLine($"Greeting: {greeting}");
            Console.WriteLine($"Empty string: '{empty}' (length: {empty.Length})");
            Console.WriteLine($"Null string: {nullString ?? "null"}");
            Console.WriteLine($"From chars: {fromChars}");

            // String immutability - this is crucial to understand!
            string original = "Hello";
            string modified = original + " World";  // Creates NEW string
            Console.WriteLine($"\nImmutability demonstration:");
            Console.WriteLine($"Original: {original}");     // Still "Hello"
            Console.WriteLine($"Modified: {modified}");     // "Hello World"
            Console.WriteLine("Notice: original string didn't change!");

            // String properties and methods - these are essential
            string sample = "Learning C# is fun!";
            Console.WriteLine($"\nString analysis of '{sample}':");
            Console.WriteLine($"Length: {sample.Length}");
            Console.WriteLine($"First character: {sample[0]}");
            Console.WriteLine($"Last character: {sample[sample.Length - 1]}");
            Console.WriteLine($"Contains 'C#': {sample.Contains("C#")}");
            Console.WriteLine($"Starts with 'Learning': {sample.StartsWith("Learning")}");
            Console.WriteLine($"Ends with 'fun!': {sample.EndsWith("fun!")}");

            Console.WriteLine();
        }

        // String equality - unlike other reference types, strings compare by value
        // This special behavior makes string comparison intuitive
        static void DemonstrateStringEquality()
        {
            Console.WriteLine("5. STRING EQUALITY");
            Console.WriteLine("==================");

            // Value equality - strings with same content are equal
            string str1 = "hello";
            string str2 = "hello";
            string str3 = "Hello";  // Different case
            string str4 = str1;     // Same reference

            Console.WriteLine("String equality examples:");
            Console.WriteLine($"str1: '{str1}'");
            Console.WriteLine($"str2: '{str2}'");
            Console.WriteLine($"str3: '{str3}'");
            Console.WriteLine($"str4: '{str4}'");

            Console.WriteLine($"\nEquality comparisons:");
            Console.WriteLine($"str1 == str2: {str1 == str2}");  // True - same value
            Console.WriteLine($"str1 == str3: {str1 == str3}");  // False - different case
            Console.WriteLine($"str1 == str4: {str1 == str4}");  // True - same reference

            // Case-insensitive comparison - often needed in real applications
            Console.WriteLine($"\nCase-insensitive comparison:");
            Console.WriteLine($"str1.Equals(str3, StringComparison.OrdinalIgnoreCase): " +
                            $"{str1.Equals(str3, StringComparison.OrdinalIgnoreCase)}");

            // String interning - .NET optimizes identical string literals
            string literal1 = "cached";
            string literal2 = "cached";
            Console.WriteLine($"\nString interning (optimization):");
            Console.WriteLine($"ReferenceEquals(literal1, literal2): {ReferenceEquals(literal1, literal2)}");
            Console.WriteLine("Same string literals may share memory!");

            Console.WriteLine();
        }

        // Escape sequences in strings - same rules as chars but in string context
        static void DemonstrateStringEscapeSequences()
        {
            Console.WriteLine("6. STRING ESCAPE SEQUENCES");
            Console.WriteLine("===========================");

            // Common escape sequences in strings - you'll use these constantly
            string withQuotes = "He said, \"Hello there!\"";
            string withApostrophe = "It's a beautiful day";  // No escape needed
            string filePath = "C:\\Users\\Username\\Documents";  // Windows path
            string multipleEscapes = "Line 1\nLine 2\tTabbed\nLine 3";

            Console.WriteLine("Strings with escape sequences:");
            Console.WriteLine($"With quotes: {withQuotes}");
            Console.WriteLine($"With apostrophe: {withApostrophe}");
            Console.WriteLine($"File path: {filePath}");
            Console.WriteLine($"Multiple escapes:\n{multipleEscapes}");

            // Unicode in strings - mix different languages and symbols
            string international = "English, Español, 中文, العربية, हिन्दी";
            string symbols = "Math: α + β = γ, Currency: $€£¥, Arrows: ←→↑↓";

            Console.WriteLine($"\nInternational text: {international}");
            Console.WriteLine($"Symbols: {symbols}");

            // Null and empty string handling - important for robust code
            string nullStr = null;
            string emptyStr = "";
            string whitespaceStr = "   ";

            Console.WriteLine($"\nString state checking:");
            Console.WriteLine($"null check: {nullStr ?? "IS NULL"}");
            Console.WriteLine($"Empty check: '{emptyStr}' is empty: {string.IsNullOrEmpty(emptyStr)}");
            Console.WriteLine($"Whitespace check: '{whitespaceStr}' is whitespace: {string.IsNullOrWhiteSpace(whitespaceStr)}");

            Console.WriteLine();
        }

        // Verbatim strings - literal interpretation, no escape processing
        // Perfect for file paths, regex patterns, and multi-line text
        static void DemonstrateVerbatimStrings()
        {
            Console.WriteLine("7. VERBATIM STRINGS");
            Console.WriteLine("===================");

            // Regular string with escapes vs verbatim string
            string regularPath = "C:\\Program Files\\My Application\\config.txt";
            string verbatimPath = @"C:\Program Files\My Application\config.txt";

            Console.WriteLine("File path comparison:");
            Console.WriteLine($"Regular: {regularPath}");
            Console.WriteLine($"Verbatim: {verbatimPath}");
            Console.WriteLine("Both produce the same result, but verbatim is cleaner!");

            // Multi-line verbatim strings - great for templates and formatted text
            string multiLineVerbatim = @"
                SELECT FirstName, LastName, Email
                FROM Users
                WHERE Age > 18
                AND IsActive = 1
                ORDER BY LastName";

            Console.WriteLine($"\nMulti-line SQL query:{multiLineVerbatim}");

            // Verbatim strings with quotes - double the quotes to escape them
            string verbatimWithQuotes = @"She said, ""I love verbatim strings!""";
            Console.WriteLine($"Verbatim with quotes: {verbatimWithQuotes}");

            // Practical examples where verbatim strings shine
            string regexPattern = @"^\d{3}-\d{2}-\d{4}$";  // Social Security Number pattern
            string jsonTemplate = @"{
    ""name"": ""John Doe"",
    ""age"": 30,
    ""city"": ""New York""
}";

            Console.WriteLine($"\nRegex pattern: {regexPattern}");
            Console.WriteLine($"JSON template:{jsonTemplate}");

            Console.WriteLine();
        }

        // Raw string literals - C# 11 feature for ultimate string flexibility
        // Perfect for complex content with lots of quotes and special characters
        static void DemonstrateRawStringLiterals()
        {
            Console.WriteLine("8. RAW STRING LITERALS (C# 11+)");
            Console.WriteLine("=================================");

            // Raw strings use triple quotes - no escaping needed at all!
            string xmlContent = """
            <configuration>
                <appSettings>
                    <add key="DatabasePath" value="C:\Data\mydb.sqlite" />
                    <add key="LogLevel" value="Debug" />
                </appSettings>
            </configuration>
            """;

            Console.WriteLine("XML configuration:");
            Console.WriteLine(xmlContent);

            // Raw strings are perfect for JSON without escape nightmare
            string jsonConfig = """
{
    "server": {
        "host": "localhost",
        "port": 8080,
        "ssl": true
    },
    "database": {
        "connectionString": "Server=localhost;Database=MyDB;Trusted_Connection=true;"
    }
}
""";

            Console.WriteLine($"\nJSON configuration:{jsonConfig}");

            // Complex content that would be painful with regular strings
            string csharpCode = """
public class Example
{
    public string GetMessage()
    {
        return "Hello, \"World\"!";
    }
}
""";

            Console.WriteLine($"\nC# code snippet:{csharpCode}");

            // Raw strings can even contain triple quotes by using more quotes
            string rawWithTripleQuotes = """"
This is a raw string that contains """ triple quotes """ inside it!
And it still works perfectly fine.
"""";

            Console.WriteLine($"\nRaw string with triple quotes: {rawWithTripleQuotes}");

            Console.WriteLine();
        }

        // String concatenation - joining strings together
        // Multiple approaches with different performance characteristics
        static void DemonstrateStringConcatenation()
        {
            Console.WriteLine("9. STRING CONCATENATION");
            Console.WriteLine("=======================");

            // Simple concatenation with + operator - fine for small amounts
            string firstName = "John";
            string lastName = "Doe";
            string fullName = firstName + " " + lastName;
            Console.WriteLine($"Simple concatenation: {fullName}");

            // Concatenation with different types - automatic ToString() calls
            int age = 25;
            double salary = 50000.50;
            string info = "Employee: " + fullName + ", Age: " + age + ", Salary: $" + salary;
            Console.WriteLine($"Mixed types: {info}");

            // String.Concat method - explicit concatenation
            string concatResult = string.Concat("A", "B", "C", "D");
            Console.WriteLine($"String.Concat: {concatResult}");

            // StringBuilder - efficient for multiple concatenations
            StringBuilder sb = new StringBuilder();
            sb.Append("Building ");
            sb.Append("a ");
            sb.Append("string ");
            sb.Append("efficiently!");
            string sbResult = sb.ToString();
            Console.WriteLine($"StringBuilder: {sbResult}");

            // Performance comparison demonstration
            Console.WriteLine("\nPerformance considerations:");
            Console.WriteLine("+ operator: Good for 2-3 strings");
            Console.WriteLine("String.Concat: Good for known number of strings");
            Console.WriteLine("StringBuilder: Best for loops and many operations");

            // String.Join - excellent for arrays and collections
            string[] words = { "This", "is", "a", "joined", "sentence" };
            string sentence = string.Join(" ", words);
            Console.WriteLine($"String.Join: {sentence}");

            int[] numbers = { 1, 2, 3, 4, 5 };
            string numberList = string.Join(", ", numbers);
            Console.WriteLine($"Joining numbers: {numberList}");

            Console.WriteLine();
        }

        // String interpolation - embedding expressions directly in strings
        // Modern, readable way to format strings with variables and expressions
        static void DemonstrateStringInterpolation()
        {
            Console.WriteLine("10. STRING INTERPOLATION");
            Console.WriteLine("========================");

            // Basic interpolation - much cleaner than concatenation
            string name = "Alice";
            int age = 28;
            string basicInterpolation = $"Hello, my name is {name} and I am {age} years old.";
            Console.WriteLine($"Basic: {basicInterpolation}");

            // Expressions in interpolation - you can put any expression inside { }
            int x = 4;
            string expressionExample = $"A square has {x} sides and {x * x} total area units.";
            Console.WriteLine($"Expression: {expressionExample}");

            // Formatting with interpolation - control how values appear
            double price = 19.99;
            DateTime now = DateTime.Now;
            int number = 255;

            Console.WriteLine($"Formatting examples:");
            Console.WriteLine($"Price: {price:C}");           // Currency format
            Console.WriteLine($"Date: {now:yyyy-MM-dd}");     // Custom date format
            Console.WriteLine($"Time: {now:HH:mm:ss}");       // Time format
            Console.WriteLine($"Hex: {number:X2}");           // Hexadecimal
            Console.WriteLine($"Percentage: {0.75:P}");       // Percentage

            // Complex expressions with conditional operator
            bool isSunny = true;
            int temperature = 22;
            string weatherReport = $"Today is {temperature}°C and {(isSunny ? "sunny" : "cloudy")}. " +
                                 $"{(temperature > 20 ? "Perfect for outdoor activities!" : "Better stay inside.")}";
            Console.WriteLine($"Complex: {weatherReport}");

            // Method calls in interpolation
            string methodExample = $"Current time: {DateTime.Now.ToString("F")}";
            Console.WriteLine($"Method call: {methodExample}");

            // Alignment and padding in interpolation
            string[] items = { "Apple", "Banana", "Cherry" };
            decimal[] prices = { 1.20m, 0.80m, 2.50m };

            Console.WriteLine("\nAlignment example:");
            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine($"{items[i],-10} : {prices[i],6:C}");
            }

            Console.WriteLine();
        }

        // String comparison - equality, ordering, and culture considerations
        // Critical for sorting, searching, and user input validation
        static void DemonstrateStringComparisons()
        {
            Console.WriteLine("11. STRING COMPARISONS");
            Console.WriteLine("======================");

            // Basic equality comparison
            string str1 = "apple";
            string str2 = "Apple";
            string str3 = "banana";

            Console.WriteLine("Basic comparisons:");
            Console.WriteLine($"'{str1}' == '{str2}': {str1 == str2}");  // False - case sensitive
            Console.WriteLine($"'{str1}' == '{str3}': {str1 == str3}");  // False - different words

            // Case-insensitive comparison - often what you actually want
            Console.WriteLine($"\nCase-insensitive comparison:");
            Console.WriteLine($"'{str1}'.Equals('{str2}', OrdinalIgnoreCase): " +
                            $"{str1.Equals(str2, StringComparison.OrdinalIgnoreCase)}");

            // Ordering comparisons with CompareTo
            Console.WriteLine($"\nOrdering comparisons (CompareTo):");
            Console.WriteLine($"'{str1}'.CompareTo('{str2}'): {str1.CompareTo(str2)}");  // Positive (a > A in ordinal)
            Console.WriteLine($"'{str1}'.CompareTo('{str3}'): {str1.CompareTo(str3)}");  // Negative (apple < banana)

            // String comparison options - important for international applications
            string german1 = "Straße";
            string german2 = "STRASSE";

            Console.WriteLine($"\nCulture-aware comparison:");
            Console.WriteLine($"German comparison (CurrentCulture): " +
                            $"{string.Compare(german1, german2, StringComparison.CurrentCultureIgnoreCase)}");

            // Practical comparison methods
            string[] fruits = { "Apple", "banana", "Cherry", "apple" };
            Array.Sort(fruits, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine($"Sorted fruits (case-insensitive): {string.Join(", ", fruits)}");

            // StartsWith and EndsWith - common pattern matching
            string filename = "document.pdf";
            Console.WriteLine($"\nPattern matching:");
            Console.WriteLine($"Is PDF file: {filename.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)}");
            Console.WriteLine($"Starts with 'doc': {filename.StartsWith("doc", StringComparison.OrdinalIgnoreCase)}");

            Console.WriteLine();
        }

        // UTF-8 strings - performance optimization for byte-based operations
        // C# 11 feature for scenarios where you work with UTF-8 encoded data
        static void DemonstrateUTF8Strings()
        {
            Console.WriteLine("12. UTF-8 STRINGS (C# 11+)");
            Console.WriteLine("===========================");

            // UTF-8 string literals - compile directly to UTF-8 bytes
            ReadOnlySpan<byte> utf8Hello = "Hello, World!"u8;
            ReadOnlySpan<byte> utf8Json = """{"name": "John", "age": 30}"""u8;

            Console.WriteLine($"UTF-8 string length (bytes): {utf8Hello.Length}");
            Console.WriteLine($"UTF-8 JSON length (bytes): {utf8Json.Length}");

            // Converting between UTF-8 and regular strings
            string regularString = "Performance matters!";
            ReadOnlySpan<byte> utf8FromString = System.Text.Encoding.UTF8.GetBytes(regularString);
            string backToString = System.Text.Encoding.UTF8.GetString(utf8FromString);

            Console.WriteLine($"Original: {regularString}");
            Console.WriteLine($"UTF-8 bytes: {utf8FromString.Length}");
            Console.WriteLine($"Back to string: {backToString}");

            // When to use UTF-8 strings
            Console.WriteLine("\nWhen to use UTF-8 strings:");
            Console.WriteLine("- JSON API responses");
            Console.WriteLine("- File I/O operations");
            Console.WriteLine("- Network protocols");
            Console.WriteLine("- Memory-constrained scenarios");
            Console.WriteLine("- Interop with C libraries");

            // Performance benefit explanation
            Console.WriteLine("\nPerformance benefits:");
            Console.WriteLine("- No UTF-16 to UTF-8 conversion needed");
            Console.WriteLine("- Reduced memory allocations");
            Console.WriteLine("- Direct byte-level operations");

            Console.WriteLine();
        }

        // Real-world practical examples combining multiple concepts
        // These demonstrate how to use string/char features in actual applications
        static void DemonstratePracticalExamples()
        {
            Console.WriteLine("13. PRACTICAL EXAMPLES");
            Console.WriteLine("======================");

            // Example 1: Input validation and formatting
            Console.WriteLine("Example 1: User Input Processing");
            string userInput = "  John.Doe@Email.com  ";
            string cleanedEmail = ProcessEmailInput(userInput);
            Console.WriteLine($"Original input: '{userInput}'");
            Console.WriteLine($"Processed email: '{cleanedEmail}'");

            // Example 2: Configuration file parsing
            Console.WriteLine($"\nExample 2: Configuration Parsing");
            string configLine = "database.host=localhost;database.port=5432";
            var config = ParseConfigLine(configLine);
            Console.WriteLine($"Config line: {configLine}");
            Console.WriteLine($"Parsed: Host={config.host}, Port={config.port}");

            // Example 3: Text template processing
            Console.WriteLine($"\nExample 3: Email Template");
            string emailTemplate = GenerateEmailTemplate("Alice Johnson", "Premium", DateTime.Now.AddDays(30));
            Console.WriteLine(emailTemplate);

            // Example 4: File path manipulation
            Console.WriteLine($"\nExample 4: File Path Operations");
            string[] filePaths = {
                @"C:\Users\John\Documents\report.pdf",
                @"C:\Projects\MyApp\src\Program.cs",
                @"C:\Temp\backup_2024.zip"
            };

            foreach (string path in filePaths)
            {
                var pathInfo = AnalyzeFilePath(path);
                Console.WriteLine($"File: {pathInfo.fileName}, Extension: {pathInfo.extension}, Directory: {pathInfo.directory}");
            }

            // Example 5: Simple CSV parsing
            Console.WriteLine($"\nExample 5: CSV Data Processing");
            string csvData = "John,Doe,25,Engineer\nJane,Smith,30,Designer\nBob,Johnson,28,Developer";
            var employees = ParseSimpleCSV(csvData);

            foreach (var emp in employees)
            {
                Console.WriteLine($"Employee: {emp.firstName} {emp.lastName}, Age: {emp.age}, Role: {emp.role}");
            }

            Console.WriteLine();
        }

        // Helper method: Email input processing
        static string ProcessEmailInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Clean up the input: trim, convert to lowercase
            string cleaned = input.Trim().ToLowerInvariant();

            // Basic validation (just for demonstration)
            if (cleaned.Contains("@") && cleaned.Contains("."))
                return cleaned;

            return "INVALID_EMAIL";
        }

        // Helper method: Configuration line parsing
        static (string host, int port) ParseConfigLine(string configLine)
        {
            string host = "localhost";
            int port = 5432;

            string[] pairs = configLine.Split(';');
            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    if (key == "database.host")
                        host = value;
                    else if (key == "database.port" && int.TryParse(value, out int parsedPort))
                        port = parsedPort;
                }
            }

            return (host, port);
        }

        // Helper method: Email template generation
        static string GenerateEmailTemplate(string customerName, string membershipType, DateTime expiryDate)
        {
            return $"""
            Dear {customerName},

            Thank you for being a valued {membershipType} member!

            Your membership details:
            - Type: {membershipType}
            - Renewal Date: {expiryDate:MMMM dd, yyyy}
            - Days Remaining: {(expiryDate - DateTime.Now).Days}

            Best regards,
            The Membership Team
            """;
        }

        // Helper method: File path analysis
        static (string fileName, string extension, string directory) AnalyzeFilePath(string fullPath)
        {
            int lastSlash = fullPath.LastIndexOf('\\');
            string fileName = lastSlash >= 0 ? fullPath.Substring(lastSlash + 1) : fullPath;
            string directory = lastSlash >= 0 ? fullPath.Substring(0, lastSlash) : "";

            int lastDot = fileName.LastIndexOf('.');
            string extension = lastDot >= 0 ? fileName.Substring(lastDot) : "";

            return (fileName, extension, directory);
        }

        // Helper method: Simple CSV parsing
        static List<(string firstName, string lastName, int age, string role)> ParseSimpleCSV(string csvData)
        {
            var employees = new List<(string, string, int, string)>();
            string[] lines = csvData.Split('\n');

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] fields = line.Split(',');
                if (fields.Length >= 4)
                {
                    string firstName = fields[0].Trim();
                    string lastName = fields[1].Trim();
                    int.TryParse(fields[2].Trim(), out int age);
                    string role = fields[3].Trim();

                    employees.Add((firstName, lastName, age, role));
                }
            }

            return employees;
        }
    }
}
