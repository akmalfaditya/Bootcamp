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

            // Basic character creation
            char letter = 'A';
            char newLine = '\n';
            char tab = '\t';

            Console.WriteLine($"Basic character: {letter}");
            Console.WriteLine($"Special characters: newline and tab exist but aren't visible here");

            // Character manipulation methods
            Console.WriteLine($"Uppercase 'c': {char.ToUpper('c')}");
            Console.WriteLine($"Lowercase 'C': {char.ToLower('C')}");
            Console.WriteLine($"Is tab whitespace? {char.IsWhiteSpace('\t')}");

            // Culture-invariant methods - important for international applications
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

            // Different ways to create strings
            string literal = "Hello World";
            string multiline = "First Line\r\nSecond Line";
            string repeated = new string('*', 10);
            char[] charArray = { 'H', 'e', 'l', 'l', 'o' };
            string fromArray = new string(charArray);

            Console.WriteLine($"Literal string: {literal}");
            Console.WriteLine($"Multiline string:\n{multiline}");
            Console.WriteLine($"Repeated character: {repeated}");
            Console.WriteLine($"From char array: {fromArray}");

            // Null and empty string handling - critical for robust applications
            string empty = "";
            string alsoEmpty = string.Empty;
            string nullString = null;

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

            // Basic equality comparison
            Console.WriteLine($"'{str1}' == '{str3}': {str1 == str3}");
            Console.WriteLine($"'{str1}' == '{str2}': {str1 == str2}");

            // Case-insensitive comparison - very important for user input
            bool caseInsensitive = string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"Case-insensitive '{str1}' equals '{str2}': {caseInsensitive}");

            // Ordinal comparison for sorting
            string[] cities = { "Boston", "Austin", "Chicago", "Denver" };
            Console.WriteLine("\nOriginal order:");
            foreach (string city in cities)
                Console.Write($"{city} ");

            Array.Sort(cities);
            Console.WriteLine("\nSorted order:");
            foreach (string city in cities)
                Console.Write($"{city} ");

            // CompareTo method for custom sorting logic
            Console.WriteLine($"\n'Boston'.CompareTo('Austin'): {string.Compare("Boston", "Austin")}");
            Console.WriteLine($"'Boston'.CompareTo('Boston'): {string.Compare("Boston", "Boston")}");
            Console.WriteLine($"'Boston'.CompareTo('Chicago'): {string.Compare("Boston", "Chicago")}");

            Console.WriteLine();
        }

        static void DemonstrateStringBuilder()
        {
            Console.WriteLine("8. STRINGBUILDER DEMONSTRATION");
            Console.WriteLine("==============================");

            // StringBuilder for efficient string building
            // When you need to build strings in loops, StringBuilder is much faster
            StringBuilder sb = new StringBuilder();
            
            Console.WriteLine("Building a large string with StringBuilder:");
            for (int i = 0; i < 10; i++)
            {
                sb.Append($"Item {i}, ");
            }
            
            Console.WriteLine(sb.ToString());

            // StringBuilder methods
            sb.Clear();
            sb.AppendLine("First line");
            sb.AppendLine("Second line");
            sb.Insert(0, "Header: ");
            sb.Replace("First", "Primary");
            
            Console.WriteLine("StringBuilder after various operations:");
            Console.WriteLine(sb.ToString());

            // Performance comparison hint
            Console.WriteLine("Note: StringBuilder is significantly faster when building large strings");
            Console.WriteLine("Use regular string concatenation for simple cases, StringBuilder for loops");

            Console.WriteLine();
        }

        static void DemonstrateTextEncoding()
        {
            Console.WriteLine("9. TEXT ENCODING DEMONSTRATION");
            Console.WriteLine("==============================");

            string originalText = "Hello, World! 🌍";
            Console.WriteLine($"Original text: {originalText}");

            // UTF-8 encoding - most common for web and file storage
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(originalText);
            Console.WriteLine($"UTF-8 byte count: {utf8Bytes.Length}");
            Console.WriteLine($"UTF-8 bytes: {BitConverter.ToString(utf8Bytes)}");

            // UTF-16 encoding - used internally by .NET
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(originalText);
            Console.WriteLine($"UTF-16 byte count: {utf16Bytes.Length}");

            // Decoding back to string
            string decodedFromUtf8 = Encoding.UTF8.GetString(utf8Bytes);
            string decodedFromUtf16 = Encoding.Unicode.GetString(utf16Bytes);
            
            Console.WriteLine($"Decoded from UTF-8: {decodedFromUtf8}");
            Console.WriteLine($"Decoded from UTF-16: {decodedFromUtf16}");
            Console.WriteLine($"Decoding successful: {originalText == decodedFromUtf8}");

            // ASCII encoding - limited to English characters
            try
            {
                byte[] asciiBytes = Encoding.ASCII.GetBytes("Hello World");
                string asciiDecoded = Encoding.ASCII.GetString(asciiBytes);
                Console.WriteLine($"ASCII works for English: {asciiDecoded}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ASCII encoding error: {ex.Message}");
            }

            Console.WriteLine("\nEncoding is crucial when working with files, databases, and web APIs");
            Console.WriteLine();
        }
    }
}
