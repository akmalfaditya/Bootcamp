using System.Globalization;
using System.Text;

// String and Text Handling Demonstration
// This project covers all fundamental concepts of working with strings and characters in C#

namespace StringAndTextHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== STRING AND TEXT HANDLING DEMONSTRATION ===\n");

            // Let's explore each concept step by step
            DemonstrateCharacterType();
            DemonstrateStringBasics();
            DemonstrateStringSearching();
            DemonstrateStringManipulation();
            DemonstrateStringSplittingAndJoining();
            DemonstrateStringInterpolationAndFormatting();
            DemonstrateStringComparison();
            DemonstrateStringBuilder();
            DemonstrateTextEncoding();

            Console.WriteLine("\n=== END OF DEMONSTRATION ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DemonstrateCharacterType()
        {
            Console.WriteLine("1. CHARACTER TYPE DEMONSTRATION");
            Console.WriteLine("================================");

            // Basic character creation - char is System.Char, represents 16-bit Unicode
            char letter = 'A';
            char newLine = '\n';
            char tab = '\t';
            char unicodeChar = '\u0041'; // Unicode escape sequence for 'A'

            Console.WriteLine($"Basic character: {letter}");
            Console.WriteLine($"Unicode character \\u0041: {unicodeChar}");
            Console.WriteLine($"Special characters exist: newline={newLine}, tab={tab}");

            // Character manipulation methods
            Console.WriteLine($"Uppercase 'c': {char.ToUpper('c')}");
            Console.WriteLine($"Lowercase 'C': {char.ToLower('C')}");
            Console.WriteLine($"Is tab whitespace? {char.IsWhiteSpace(tab)}");

            // Culture-invariant methods - critical for international applications
            // Turkish example: 'i' -> 'İ' in Turkish culture vs 'I' in invariant
            Console.WriteLine($"Culture-invariant uppercase 'i': {char.ToUpperInvariant('i')}");
            Console.WriteLine($"Regular uppercase 'i': {char.ToUpper('i')}");

            // Character categorization - very useful for data validation
            Console.WriteLine($"Is 'A' a letter? {char.IsLetter('A')}");
            Console.WriteLine($"Is '5' a digit? {char.IsDigit('5')}");
            Console.WriteLine($"Is '!' punctuation? {char.IsPunctuation('!')}");
            Console.WriteLine($"Is ' ' whitespace? {char.IsWhiteSpace(' ')}");

            // Unicode categorization for advanced text processing
            char testChar = 'A';
            Console.WriteLine($"Unicode category of '{testChar}': {char.GetUnicodeCategory(testChar)}");

            Console.WriteLine();
        }

        static void DemonstrateStringBasics()
        {
            Console.WriteLine("2. STRING BASICS DEMONSTRATION");
            Console.WriteLine("==============================");

            // Different ways to create strings - understanding construction options
            string literal = "Hello World";
            string multiline = "First Line\r\nSecond Line";
            string verbatim = @"C:\Path\File.txt";          // Verbatim string literal - no escape sequences
            string repeated = new string('*', 10);          // Repeat character constructor
            char[] charArray = { 'H', 'e', 'l', 'l', 'o' };
            string fromArray = new string(charArray);       // From char array constructor
            string fromSubset = new string(charArray, 1, 3); // From char array subset (start, count)

            Console.WriteLine($"Literal string: {literal}");
            Console.WriteLine($"Multiline string:\n{multiline}");
            Console.WriteLine($"Verbatim string: {verbatim}");
            Console.WriteLine($"Repeated character: {repeated}");
            Console.WriteLine($"From char array: {fromArray}");
            Console.WriteLine($"From char subset: {fromSubset}");

            // Null and empty string handling - critical for robust applications
            string empty = "";
            string alsoEmpty = string.Empty;
            string? nullString = null; // Explicitly nullable for modern C# nullable reference types

            Console.WriteLine($"Empty string == \"\": {empty == ""}");
            Console.WriteLine($"Empty string == string.Empty: {empty == string.Empty}");
            Console.WriteLine($"Empty string length: {empty.Length}");

            // Safe null checking - prevents NullReferenceException
            Console.WriteLine($"Is null string null? {nullString == null}");
            Console.WriteLine($"Is null string empty? {string.IsNullOrEmpty(nullString)}");
            Console.WriteLine($"Is empty string null or empty? {string.IsNullOrEmpty(empty)}");

            // Accessing characters within strings
            string sample = "Programming";
            Console.WriteLine($"Character at index 0: {sample[0]}");
            Console.WriteLine($"Character at index 4: {sample[4]}");

            // Iterating through string characters
            Console.Write("Characters in '123': ");
            foreach (char c in "123")
            {
                Console.Write($"{c},");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        static void DemonstrateStringSearching()
        {
            Console.WriteLine("3. STRING SEARCHING DEMONSTRATION");
            Console.WriteLine("=================================");

            string text = "The quick brown fox jumps over the lazy dog";

            // Basic search methods
            Console.WriteLine($"Text: '{text}'");
            Console.WriteLine($"Starts with 'The': {text.StartsWith("The")}");
            Console.WriteLine($"Ends with 'dog': {text.EndsWith("dog")}");
            Console.WriteLine($"Contains 'brown': {text.Contains("brown")}");

            // Finding positions - useful for text parsing
            Console.WriteLine($"Index of 'fox': {text.IndexOf("fox")}");
            Console.WriteLine($"Index of 'cat' (not found): {text.IndexOf("cat")}");
            Console.WriteLine($"Last index of 'the': {text.LastIndexOf("the")}");

            // Finding any of multiple characters - great for parsing delimited data
            string sample = "apple,banana;orange:grape";
            char[] delimiters = { ',', ';', ':' };
            int firstDelimiter = sample.IndexOfAny(delimiters);
            Console.WriteLine($"Sample: '{sample}'");
            Console.WriteLine($"First delimiter position: {firstDelimiter}");

            Console.WriteLine();
        }

        static void DemonstrateStringManipulation()
        {
            Console.WriteLine("4. STRING MANIPULATION DEMONSTRATION");
            Console.WriteLine("====================================");

            // Remember: strings are immutable - each operation creates a new string
            string original = "Hello World";

            // Substring extraction
            string left5 = original.Substring(0, 5);
            string right5 = original.Substring(6);
            Console.WriteLine($"Original: '{original}'");
            Console.WriteLine($"Left 5 characters: '{left5}'");
            Console.WriteLine($"From index 6 to end: '{right5}'");

            // Insert and remove operations
            string inserted = original.Insert(5, ",");
            string removed = inserted.Remove(5, 1);
            Console.WriteLine($"After inserting comma: '{inserted}'");
            Console.WriteLine($"After removing comma: '{removed}'");

            // Padding - useful for formatting output
            string number = "123";
            Console.WriteLine($"Right-padded: '{number.PadRight(10, '*')}'");
            Console.WriteLine($"Left-padded: '{number.PadLeft(10, '0')}'");

            // Trimming whitespace - essential for user input processing
            string messy = "   Hello World   \t\r\n";
            Console.WriteLine($"Original length: {messy.Length}");
            Console.WriteLine($"Trimmed length: {messy.Trim().Length}");
            Console.WriteLine($"Trimmed result: '{messy.Trim()}'");

            // String replacement
            string sentence = "I like cats and cats like me";
            string replaced = sentence.Replace("cats", "dogs");
            Console.WriteLine($"Original: '{sentence}'");
            Console.WriteLine($"Replaced: '{replaced}'");

            Console.WriteLine();
        }

        static void DemonstrateStringSplittingAndJoining()
        {
            Console.WriteLine("5. STRING SPLITTING AND JOINING DEMONSTRATION");
            Console.WriteLine("=============================================");

            // Splitting strings - fundamental for data processing
            string sentence = "The quick brown fox jumps";
            string[] words = sentence.Split();
            
            Console.WriteLine($"Original sentence: '{sentence}'");
            Console.Write("Words: ");
            foreach (string word in words)
            {
                Console.Write($"'{word}' ");
            }
            Console.WriteLine();

            // Splitting with custom delimiters
            string csvData = "apple,banana,cherry,date";
            string[] fruits = csvData.Split(',');
            Console.WriteLine($"CSV data: '{csvData}'");
            Console.WriteLine($"Number of fruits: {fruits.Length}");

            // Joining strings back together
            string rejoined = string.Join(" ", words);
            string csvRejoined = string.Join(" | ", fruits);
            
            Console.WriteLine($"Rejoined with spaces: '{rejoined}'");
            Console.WriteLine($"Fruits joined with pipes: '{csvRejoined}'");

            Console.WriteLine();
        }

        static void DemonstrateStringInterpolationAndFormatting()
        {
            Console.WriteLine("6. STRING INTERPOLATION AND FORMATTING DEMONSTRATION");
            Console.WriteLine("====================================================");

            // String interpolation - modern and readable way to build strings
            string name = "Alice";
            int age = 25;
            DateTime today = DateTime.Now;

            string interpolated = $"Hello, my name is {name} and I'm {age} years old.";
            string withDate = $"Today is {today.DayOfWeek}, {today:yyyy-MM-dd}";
            
            Console.WriteLine(interpolated);
            Console.WriteLine(withDate);

            // Traditional string formatting - still useful for complex scenarios
            string template = "It's {0} degrees in {1} on this {2} morning";
            string formatted = string.Format(template, 25, "Jakarta", today.DayOfWeek);
            Console.WriteLine(formatted);

            // Format specifiers for numbers and dates
            double price = 19.99;
            Console.WriteLine($"Price: {price:C}"); // Currency format
            Console.WriteLine($"Percentage: {0.85:P}"); // Percentage format
            Console.WriteLine($"Date: {today:dddd, MMMM dd, yyyy}"); // Long date format

            Console.WriteLine();
        }

        static void DemonstrateStringComparison()
        {
            Console.WriteLine("7. STRING COMPARISON DEMONSTRATION");
            Console.WriteLine("==================================");

            string str1 = "Hello";
            string str2 = "hello";
            string str3 = "Hello";

            // Default equality comparison - ordinal, case-sensitive
            Console.WriteLine("=== EQUALITY COMPARISON ===");
            Console.WriteLine($"'{str1}' == '{str3}': {str1 == str3}");
            Console.WriteLine($"'{str1}' == '{str2}': {str1 == str2}");
            Console.WriteLine($"'{str1}'.Equals('{str2}'): {str1.Equals(str2)}");

            // StringComparison enum - gives you full control over comparison behavior
            Console.WriteLine("\n=== STRING COMPARISON OPTIONS ===");
            Console.WriteLine($"Ordinal (default): {string.Equals(str1, str2, StringComparison.Ordinal)}");
            Console.WriteLine($"OrdinalIgnoreCase: {string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase)}");
            Console.WriteLine($"CurrentCulture: {string.Equals(str1, str2, StringComparison.CurrentCulture)}");
            Console.WriteLine($"CurrentCultureIgnoreCase: {string.Equals(str1, str2, StringComparison.CurrentCultureIgnoreCase)}");
            Console.WriteLine($"InvariantCulture: {string.Equals(str1, str2, StringComparison.InvariantCulture)}");
            Console.WriteLine($"InvariantCultureIgnoreCase: {string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase)}");

            // Order comparison - for sorting and alphabetical ordering
            Console.WriteLine("\n=== ORDER COMPARISON ===");
            string[] words = { "apple", "Banana", "cherry", "Date" };
            Console.WriteLine("Original order: " + string.Join(", ", words));

            // Default culture-sensitive comparison
            Array.Sort(words, string.Compare);
            Console.WriteLine("Culture sort: " + string.Join(", ", words));

            // Reset array
            words = new[] { "apple", "Banana", "cherry", "Date" };
            
            // Ordinal comparison - treats characters as their numeric Unicode values
            Array.Sort(words, StringComparer.Ordinal);
            Console.WriteLine("Ordinal sort: " + string.Join(", ", words));

            // Case-insensitive ordinal comparison
            Array.Sort(words, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("Ordinal ignore case: " + string.Join(", ", words));

            // CompareTo examples - returns negative, zero, or positive
            Console.WriteLine("\n=== COMPARETO EXAMPLES ===");
            Console.WriteLine($"'Boston'.CompareTo('Austin'): {string.Compare("Boston", "Austin")}");
            Console.WriteLine($"'Boston'.CompareTo('Boston'): {string.Compare("Boston", "Boston")}");
            Console.WriteLine($"'Boston'.CompareTo('Chicago'): {string.Compare("Boston", "Chicago")}");
            
            // Ordinal vs Culture demonstration
            Console.WriteLine("\n=== ORDINAL VS CULTURE COMPARISON ===");
            string a = "Atom";
            string b = "atom";
            Console.WriteLine($"Ordinal: '{a}' vs '{b}' = {string.Compare(a, b, StringComparison.Ordinal)}");
            Console.WriteLine($"Culture: '{a}' vs '{b}' = {string.Compare(a, b, StringComparison.CurrentCulture)}");
            Console.WriteLine("Note: Ordinal treats 'A' (65) and 'a' (97) by Unicode values");
            Console.WriteLine("Culture comparison considers language rules for proper alphabetical ordering");

            Console.WriteLine();
        }

        static void DemonstrateStringBuilder()
        {
            Console.WriteLine("8. STRINGBUILDER DEMONSTRATION");
            Console.WriteLine("==============================");

            // StringBuilder for efficient string building - mutable strings
            // Critical when you need to build strings in loops or with many operations
            Console.WriteLine("=== BASIC STRINGBUILDER OPERATIONS ===");
            
            StringBuilder sb = new StringBuilder();
            Console.WriteLine($"Initial capacity: {sb.Capacity}");
            Console.WriteLine($"Initial length: {sb.Length}");
            
            // Building strings efficiently
            for (int i = 0; i < 10; i++)
            {
                sb.Append($"Item {i}, ");
            }
            
            Console.WriteLine($"After appending 10 items:");
            Console.WriteLine($"Length: {sb.Length}, Capacity: {sb.Capacity}");
            Console.WriteLine($"Content: {sb.ToString()}");

            // StringBuilder with initial capacity - performance optimization
            Console.WriteLine("\n=== CAPACITY MANAGEMENT ===");
            StringBuilder sbWithCapacity = new StringBuilder(100); // Pre-allocate capacity
            Console.WriteLine($"StringBuilder with initial capacity 100: {sbWithCapacity.Capacity}");

            // Various StringBuilder methods
            Console.WriteLine("\n=== STRINGBUILDER METHODS ===");
            sb.Clear();
            sb.AppendLine("First line");
            sb.AppendLine("Second line");
            sb.Insert(0, "Header: ");
            sb.Replace("First", "Primary");
            sb.AppendFormat("Formatted number: {0:N2}", 12345.67);
            sb.AppendLine();
            
            Console.WriteLine("StringBuilder after various operations:");
            Console.WriteLine(sb.ToString());

            // Writable indexer - you can modify individual characters
            Console.WriteLine("\n=== MUTABLE CHARACTER ACCESS ===");
            StringBuilder demo = new StringBuilder("Hello World");
            Console.WriteLine($"Original: {demo}");
            demo[6] = 'w'; // Change 'W' to 'w'
            Console.WriteLine($"After changing index 6: {demo}");

            // Performance comparison demonstration
            Console.WriteLine("\n=== PERFORMANCE INSIGHTS ===");
            Console.WriteLine("StringBuilder vs String Concatenation:");
            Console.WriteLine("- String: Creates new object for each concatenation");
            Console.WriteLine("- StringBuilder: Modifies internal buffer, much more efficient");
            Console.WriteLine("- Use StringBuilder when building strings in loops");
            Console.WriteLine("- Use string concatenation for simple, one-time operations");
            
            // Method chaining with StringBuilder
            StringBuilder chained = new StringBuilder()
                .Append("Method ")
                .Append("chaining ")
                .Append("works ")
                .AppendLine("great!");
            Console.WriteLine($"Method chaining result: {chained}");

            Console.WriteLine();
        }

        static void DemonstrateTextEncoding()
        {
            Console.WriteLine("9. TEXT ENCODING DEMONSTRATION");
            Console.WriteLine("==============================");

            // Text encoding is crucial for file I/O, network communication, and data storage
            string originalText = "Hello, World! 🌍 Café résumé";
            Console.WriteLine($"Original text: {originalText}");
            Console.WriteLine($"Character count: {originalText.Length}");

            Console.WriteLine("\n=== DIFFERENT ENCODING SCHEMES ===");

            // UTF-8: Variable-length encoding (1-4 bytes per character)
            // Most common for web and file storage, ASCII-compatible
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(originalText);
            Console.WriteLine($"UTF-8 byte count: {utf8Bytes.Length}");
            Console.WriteLine($"UTF-8 first 20 bytes: {BitConverter.ToString(utf8Bytes.Take(20).ToArray())}...");

            // UTF-16: Variable-length encoding (2 or 4 bytes per character)
            // Used internally by .NET for char and string
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(originalText);
            Console.WriteLine($"UTF-16 (Unicode) byte count: {utf16Bytes.Length}");
            Console.WriteLine($"UTF-16 first 20 bytes: {BitConverter.ToString(utf16Bytes.Take(20).ToArray())}...");

            // UTF-32: Fixed-length encoding (4 bytes per character)
            // Least space-efficient but allows easy random access
            byte[] utf32Bytes = Encoding.UTF32.GetBytes(originalText);
            Console.WriteLine($"UTF-32 byte count: {utf32Bytes.Length}");

            // ASCII: Limited to first 128 Unicode characters
            Console.WriteLine("\n=== ASCII ENCODING (LIMITED) ===");
            string asciiText = "Hello World 123";
            byte[] asciiBytes = Encoding.ASCII.GetBytes(asciiText);
            Console.WriteLine($"ASCII text: '{asciiText}' -> {asciiBytes.Length} bytes");

            // ASCII can't handle extended characters
            try
            {
                byte[] asciiWithEmoji = Encoding.ASCII.GetBytes("Hello 🌍");
                string asciiDecoded = Encoding.ASCII.GetString(asciiWithEmoji);
                Console.WriteLine($"ASCII with emoji: '{asciiDecoded}' (emoji lost!)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ASCII encoding issue: {ex.Message}");
            }

            Console.WriteLine("\n=== ENCODING/DECODING ROUNDTRIP ===");
            
            // Demonstrate encoding and decoding roundtrip
            string[] testEncodings = { "UTF-8", "UTF-16", "UTF-32" };
            Encoding[] encodings = { Encoding.UTF8, Encoding.Unicode, Encoding.UTF32 };

            for (int i = 0; i < testEncodings.Length; i++)
            {
                byte[] encoded = encodings[i].GetBytes(originalText);
                string decoded = encodings[i].GetString(encoded);
                bool isIdentical = originalText == decoded;
                Console.WriteLine($"{testEncodings[i]}: {encoded.Length} bytes, roundtrip successful: {isIdentical}");
            }

            Console.WriteLine("\n=== PRACTICAL FILE I/O EXAMPLE ===");
            // Demonstrate how encoding affects file operations
            string tempFile = Path.GetTempFileName();
            
            try
            {
                // Write with UTF-8 (default)
                File.WriteAllText(tempFile, originalText);
                var utf8FileInfo = new FileInfo(tempFile);
                Console.WriteLine($"UTF-8 file size: {utf8FileInfo.Length} bytes");

                // Write with UTF-16
                File.WriteAllText(tempFile, originalText, Encoding.Unicode);
                var utf16FileInfo = new FileInfo(tempFile);
                Console.WriteLine($"UTF-16 file size: {utf16FileInfo.Length} bytes");

                // Read back and verify
                string readBack = File.ReadAllText(tempFile, Encoding.Unicode);
                Console.WriteLine($"Read back successfully: {originalText == readBack}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            Console.WriteLine("\n=== KEY ENCODING CONCEPTS ===");
            Console.WriteLine("• Character Set: Assignment of characters to numeric codes (Unicode)");
            Console.WriteLine("• Text Encoding: Mapping from character codes to binary representation");
            Console.WriteLine("• UTF-8: Most common, ASCII-compatible, variable length (1-4 bytes)");
            Console.WriteLine("• UTF-16: .NET internal format, variable length (2-4 bytes)");
            Console.WriteLine("• UTF-32: Fixed length (4 bytes), rarely used");
            Console.WriteLine("• ASCII: Legacy, limited to English alphabet (128 characters)");
            Console.WriteLine("• Always specify encoding for file/network operations to avoid data corruption");

            Console.WriteLine();
        }
    }
}
