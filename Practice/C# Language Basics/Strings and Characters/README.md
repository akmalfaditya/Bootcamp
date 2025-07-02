# C# Strings and Characters

This project provides a comprehensive exploration of text handling in C#, focusing on the `char` and `string` data types. Understanding these types is fundamental for processing text data, user input, and creating user interfaces.

## Objectives

This demonstration covers the essential concepts for working with textual data in C#, including character manipulation, string operations, and performance considerations for text processing.

## Core Concepts

The following essential topics are covered in this project:

### 1. Character Type (`char`)

The `char` data type in C# represents a single character and serves as the fundamental building block for text processing. Understanding character manipulation is essential for developing applications that handle textual data effectively.

**Unicode Representation:**
C# characters are stored as 16-bit unsigned integers that represent Unicode code points, specifically using the UTF-16 encoding format. This design provides comprehensive support for international character sets, including Latin alphabets, Asian scripts, mathematical symbols, and special characters.

```csharp
char englishLetter = 'A';          // Basic Latin character
char unicodeChar = '\u03B1';       // Greek letter alpha (α)
char emojiChar = '\u1F600';        // Unicode emoji (😀)
char chineseChar = '中';            // Chinese character
```

The Unicode system ensures that applications can handle text in virtually any language, making C# suitable for global software development.

**Character Literals:**
Character literals are created using single quotes and can be expressed in several formats:

- **Direct Character**: `'A'`, `'5'`, `'@'`
- **Escape Sequences**: `'\n'` (newline), `'\t'` (tab), `'\\'` (backslash)
- **Unicode Escape**: `'\u0041'` (represents 'A')
- **Hexadecimal Escape**: `'\x41'` (represents 'A')

```csharp
char letter = 'C';
char newline = '\n';
char tab = '\t';
char backslash = '\\';
char quote = '\'';
char unicodeA = '\u0041';
char hexA = '\x41';
```

**Character Operations:**
The `char` type provides extensive static methods for character classification, conversion, and manipulation:

**Classification Methods:**
```csharp
char testChar = 'A';
bool isLetter = char.IsLetter(testChar);        // true
bool isDigit = char.IsDigit(testChar);          // false
bool isUpper = char.IsUpper(testChar);          // true
bool isLower = char.IsLower(testChar);          // false
bool isWhiteSpace = char.IsWhiteSpace(' ');     // true
bool isPunctuation = char.IsPunctuation('.');   // true
bool isSymbol = char.IsSymbol('$');             // true
bool isControl = char.IsControl('\n');         // true
```

**Conversion Methods:**
```csharp
char upperCase = char.ToUpper('a');             // 'A'
char lowerCase = char.ToLower('A');             // 'a'
char upperInvariant = char.ToUpperInvariant('a'); // Culture-independent conversion
char lowerInvariant = char.ToLowerInvariant('A'); // Culture-independent conversion
```

**Escape Sequences:**
Escape sequences provide a way to represent characters that cannot be easily typed or displayed:

- `\n` - Line feed (newline)
- `\r` - Carriage return
- `\t` - Horizontal tab
- `\"` - Double quote
- `\'` - Single quote
- `\\` - Backslash
- `\0` - Null character
- `\a` - Alert (bell)
- `\b` - Backspace
- `\f` - Form feed
- `\v` - Vertical tab

Understanding these escape sequences is crucial for properly formatting text output and handling special characters in string processing.

### 2. String Type Fundamentals

Strings in C# are reference types that represent sequences of characters. Understanding the fundamental characteristics of strings is essential for effective text processing and memory-efficient programming.

**Immutability:**
String immutability is one of the most important concepts in C# string handling. Once a string object is created, its content cannot be modified. Any operation that appears to change a string actually creates a new string object and returns a reference to it.

```csharp
string original = "Hello";
string modified = original + " World";  // Creates new string object
// 'original' still contains "Hello"
// 'modified' contains "Hello World"

string text = "Initial";
text = text.Replace("I", "i");  // Creates new string, assigns to 'text'
```

This immutability provides several benefits:
- **Thread Safety**: Multiple threads can safely read the same string without synchronization
- **Predictable Behavior**: String values cannot be unexpectedly modified by other code
- **Optimization Opportunities**: Enables string interning and other compiler optimizations

However, immutability also means that operations like concatenation in loops can be inefficient because they create many temporary string objects.

**Memory Management:**
Strings are allocated on the managed heap and are subject to garbage collection. The .NET runtime manages string memory automatically, but understanding the allocation patterns helps in writing efficient code.

```csharp
// Each concatenation creates a new string object
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += i.ToString();  // Creates 1000 intermediate string objects
}

// More efficient approach for multiple concatenations
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append(i);  // Modifies internal buffer, no new allocations
}
string efficient = sb.ToString();  // Creates final string once
```

**String Interning:**
String interning is an optimization technique where identical string literals are stored in a shared memory pool, ensuring that multiple references to the same string value point to the same object in memory.

```csharp
string literal1 = "Hello";
string literal2 = "Hello";
bool sameReference = ReferenceEquals(literal1, literal2);  // true

// Compile-time constants are also interned
string combined = "Hel" + "lo";
bool alsoInterned = ReferenceEquals(literal1, combined);   // true

// Runtime-created strings are not automatically interned
string runtime = new string("Hello".ToCharArray());
bool notInterned = ReferenceEquals(literal1, runtime);     // false

// Manual interning
string interned = string.Intern(runtime);
bool nowInterned = ReferenceEquals(literal1, interned);    // true
```

**Default Values:**
String variables that are declared but not initialized have a default value of `null`, which is different from an empty string.

```csharp
string uninitializedString;        // null
string emptyString = "";           // empty string (length 0)
string nullString = null;          // explicitly null

bool isNull = (uninitializedString == null);           // true
bool isEmpty = string.IsNullOrEmpty(emptyString);      // true
bool isNullOrEmpty = string.IsNullOrEmpty(nullString); // true
```

Understanding the distinction between `null` and empty strings is crucial for proper error handling and validation in applications.

### 3. String Creation and Literals

C# provides multiple ways to create and represent string literals, each serving different purposes and offering various advantages for different scenarios.

**Standard Literals:**
Standard string literals are the most common way to create strings in C#. They are enclosed in double quotes and support escape sequences for special characters.

```csharp
string basicString = "Hello, World!";
string withEscapes = "Line 1\nLine 2\tTabbed content";
string quotedText = "She said, \"Hello there!\"";
string filePath = "C:\\Users\\Documents\\file.txt";
```

Standard literals process escape sequences, which means backslashes and certain character combinations have special meaning and must be escaped when used literally.

**Verbatim Strings:**
Verbatim strings, prefixed with the `@` symbol, treat most characters literally, eliminating the need for escape sequences except for double quotes, which are escaped by doubling.

```csharp
string verbatimPath = @"C:\Users\Documents\file.txt";  // No need to escape backslashes
string multiLineVerbatim = @"This string
spans multiple lines
without requiring \n escape sequences";

string quotesInVerbatim = @"This string contains ""quoted"" text";  // Double quotes escaped
```

Verbatim strings are particularly useful for:
- File paths (avoiding double backslashes)
- Regular expressions (reducing escape complexity)
- Multi-line strings (preserving formatting)
- SQL queries and other text with many backslashes

**Raw String Literals (C# 11+):**
Raw string literals provide the ultimate flexibility for complex string content. They start and end with at least three double quotes and can contain any characters, including quotes, without escaping.

```csharp
string rawString = """
    This is a raw string literal.
    It can contain "quotes" and \backslashes
    without any escaping required.
    """;

string jsonData = """
    {
        "name": "John Doe",
        "age": 30,
        "address": {
            "street": "123 Main St",
            "city": "New York"
        }
    }
    """;

string regexPattern = """^\d{3}-\d{3}-\d{4}$""";  // Phone number pattern
```

Raw string literals automatically handle indentation by removing common leading whitespace, making them ideal for embedded code, configuration files, and complex text templates.

**String Interpolation:**
String interpolation, introduced with the `$` prefix, allows embedding expressions directly within string literals, providing a readable and efficient way to construct formatted strings.

```csharp
string name = "Alice";
int age = 25;
decimal salary = 75000.50m;

// Basic interpolation
string greeting = $"Hello, {name}!";
string info = $"{name} is {age} years old.";

// Format specifiers
string currency = $"Salary: {salary:C}";                    // Currency format
string percentage = $"Growth: {0.15:P}";                    // Percentage format
string padded = $"ID: {42:D6}";                            // Zero-padded number
string scientific = $"Value: {123456.789:E}";              // Scientific notation

// Alignment and width
string table = $"{"Name",-15} {"Age",3} {"Salary",10:C}";
```

**Advanced Interpolation Features:**
```csharp
// Conditional expressions
bool isVip = true;
string status = $"Status: {(isVip ? "VIP Customer" : "Regular Customer")}";

// Method calls in interpolation
DateTime now = DateTime.Now;
string timestamp = $"Generated at: {now.ToString("yyyy-MM-dd HH:mm:ss")}";

// Complex expressions
var items = new[] { "apple", "banana", "cherry" };
string summary = $"Found {items.Length} items: {string.Join(", ", items)}";
```

String interpolation is compiled into efficient code that uses `StringBuilder` internally when appropriate, making it both readable and performant for most use cases.

### 4. String Operations

String operations in C# encompass a wide range of methods for manipulating, analyzing, and transforming text data. Understanding these operations is essential for effective text processing in applications.

**Concatenation:**
String concatenation combines multiple strings into a single string. C# provides several methods for concatenation, each with different performance characteristics and use cases.

**Operator Concatenation (`+`):**
```csharp
string first = "Hello";
string second = "World";
string combined = first + " " + second;  // "Hello World"

// Works with different types (automatic ToString() conversion)
string result = "Count: " + 42 + " items";  // "Count: 42 items"
```

**String.Concat Method:**
```csharp
string result1 = string.Concat("Hello", " ", "World");
string result2 = string.Concat("Values: ", 1, 2, 3);  // Handles multiple types
string[] parts = { "Apple", "Banana", "Cherry" };
string result3 = string.Concat(parts);  // Concatenates array elements
```

**String.Join Method:**
```csharp
string[] words = { "Apple", "Banana", "Cherry" };
string joined = string.Join(", ", words);  // "Apple, Banana, Cherry"
string numbered = string.Join(" | ", Enumerable.Range(1, 5));  // "1 | 2 | 3 | 4 | 5"
```

**Substring Operations:**
Substring operations extract portions of existing strings based on position and length criteria.

```csharp
string text = "Hello, World!";

// Extract substring starting at index
string hello = text.Substring(0, 5);      // "Hello"
string world = text.Substring(7, 5);      // "World"
string ending = text.Substring(7);        // "World!" (from index to end)

// Modern span-based approach (C# 8+)
ReadOnlySpan<char> span = text.AsSpan(0, 5);  // Zero-allocation substring
string helloFromSpan = text[0..5];             // Range operator syntax
```

**Search and Replace:**
Search and replace operations locate and modify specific content within strings.

**Search Operations:**
```csharp
string text = "The quick brown fox jumps over the lazy dog";

// Basic search methods
bool contains = text.Contains("fox");           // true
bool startsWith = text.StartsWith("The");      // true
bool endsWith = text.EndsWith("dog");          // true

// Index-based search
int index = text.IndexOf("fox");               // 16
int lastIndex = text.LastIndexOf("the");       // 31
int notFound = text.IndexOf("elephant");       // -1

// Case-insensitive search
bool containsIgnoreCase = text.Contains("FOX", StringComparison.OrdinalIgnoreCase);
```

**Replace Operations:**
```csharp
string original = "The quick brown fox";
string replaced = original.Replace("fox", "wolf");     // "The quick brown wolf"
string removedSpaces = original.Replace(" ", "");      // "Thequickbrownfox"

// Case-sensitive replacement
string caseReplaced = original.Replace("Fox", "Wolf"); // No change (case mismatch)
```

**Case Conversion:**
Case conversion operations modify the capitalization of string content.

```csharp
string mixed = "Hello World";

string upper = mixed.ToUpper();                    // "HELLO WORLD"
string lower = mixed.ToLower();                    // "hello world"
string upperInvariant = mixed.ToUpperInvariant();  // Culture-independent
string lowerInvariant = mixed.ToLowerInvariant();  // Culture-independent

// Title case (requires TextInfo)
string title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mixed.ToLower());
```

**String Splitting and Trimming:**
```csharp
string data = "  apple,banana,cherry  ";

// Splitting
string[] fruits = data.Split(',');                     // [" apple", "banana", "cherry "]
string[] trimmedFruits = data.Split(',', StringSplitOptions.TrimEntries);  // ["apple", "banana", "cherry"]

// Trimming
string trimmed = data.Trim();                          // "apple,banana,cherry"
string leftTrimmed = data.TrimStart();                 // "apple,banana,cherry  "
string rightTrimmed = data.TrimEnd();                  // "  apple,banana,cherry"
string customTrimmed = data.Trim(' ', ',');            // "apple,banana,cherry"
```

These operations form the foundation for most text processing tasks and are essential for data validation, formatting, and user interface development.

### 5. String Comparison

String comparison in C# involves multiple considerations including case sensitivity, cultural context, and performance requirements. Understanding these nuances is crucial for building applications that handle text correctly across different scenarios and locales.

**Equality Comparison:**
String equality can be evaluated using different approaches, each with specific use cases and performance characteristics.

**Operator Equality (`==`):**
```csharp
string str1 = "Hello";
string str2 = "Hello";
string str3 = new string("Hello".ToCharArray());

bool equal1 = (str1 == str2);    // true (value equality)
bool equal2 = (str1 == str3);    // true (value equality)
```

**Equals Method:**
```csharp
bool equals1 = str1.Equals(str2);                                    // true
bool equals2 = str1.Equals(str3);                                    // true
bool equalsIgnoreCase = str1.Equals("HELLO", StringComparison.OrdinalIgnoreCase);  // true
```

**Reference Equality:**
```csharp
bool referenceEqual1 = ReferenceEquals(str1, str2);    // true (string interning)
bool referenceEqual2 = ReferenceEquals(str1, str3);    // false (different objects)
```

**Ordinal Comparison:**
Ordinal comparison performs binary comparison of character codes without considering cultural or linguistic rules. This method is fast and consistent across different cultures.

```csharp
// Ordinal comparison
string value1 = "apple";
string value2 = "Apple";

bool ordinalEqual = string.Equals(value1, value2, StringComparison.Ordinal);                    // false
bool ordinalIgnoreCase = string.Equals(value1, value2, StringComparison.OrdinalIgnoreCase);    // true

// Ordinal comparison for sorting
string[] items = { "zebra", "Apple", "banana" };
Array.Sort(items, StringComparer.Ordinal);           // Result: ["Apple", "banana", "zebra"]
Array.Sort(items, StringComparer.OrdinalIgnoreCase); // Result: ["Apple", "banana", "zebra"]
```

Ordinal comparison is recommended for:
- Configuration keys and identifiers
- File paths and URLs
- Programming language tokens
- Any scenario where consistent, culture-independent behavior is required

**Cultural Comparison:**
Cultural comparison considers language-specific rules, character equivalences, and sorting orders. This is important for user-facing content and internationalized applications.

```csharp
// Current culture comparison
string german1 = "Straße";
string german2 = "STRASSE";

bool currentCulture = string.Equals(german1, german2, StringComparison.CurrentCulture);              // false
bool currentCultureIgnoreCase = string.Equals(german1, german2, StringComparison.CurrentCultureIgnoreCase); // true (in German culture)

// Invariant culture comparison
bool invariantCulture = string.Equals(german1, german2, StringComparison.InvariantCulture);         // false
bool invariantIgnoreCase = string.Equals(german1, german2, StringComparison.InvariantCultureIgnoreCase);    // false
```

**Case Sensitivity Options:**
C# provides explicit control over case sensitivity in string operations.

```csharp
string searchText = "The Quick Brown Fox";
string pattern = "quick";

// Case-sensitive search
bool caseSensitive = searchText.Contains(pattern);                                           // false
bool caseInsensitive = searchText.Contains(pattern, StringComparison.OrdinalIgnoreCase);    // true

// StartsWith and EndsWith with case control
bool startsWithCase = searchText.StartsWith("the");                                         // false
bool startsWithIgnoreCase = searchText.StartsWith("the", StringComparison.OrdinalIgnoreCase); // true
```

**Advanced Comparison Scenarios:**
```csharp
public class StringComparisonExamples
{
    // Configuration key comparison - use Ordinal for performance and consistency
    public bool IsConfigurationKey(string key, string expectedKey)
    {
        return string.Equals(key, expectedKey, StringComparison.Ordinal);
    }
    
    // User input comparison - use OrdinalIgnoreCase for culture independence
    public bool IsUserCommand(string input, string command)
    {
        return string.Equals(input?.Trim(), command, StringComparison.OrdinalIgnoreCase);
    }
    
    // Display name sorting - use CurrentCulture for proper localization
    public IEnumerable<string> SortDisplayNames(IEnumerable<string> names)
    {
        return names.OrderBy(name => name, StringComparer.CurrentCulture);
    }
    
    // File extension check - use OrdinalIgnoreCase
    public bool HasImageExtension(string fileName)
    {
        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        return imageExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }
}
```

**Performance Considerations:**
- **Ordinal comparisons** are fastest and should be used when cultural considerations are not relevant
- **Cultural comparisons** are slower but necessary for proper internationalization
- **Case-insensitive comparisons** require additional processing but provide better user experience
- **String.Intern()** can improve performance for frequently compared strings but should be used carefully due to memory implications

### 6. Performance Considerations

Understanding performance implications of string operations is critical for building efficient applications, especially when dealing with large amounts of text data or frequent string manipulations.

**StringBuilder for Efficient String Building:**
The `StringBuilder` class is specifically designed for scenarios requiring multiple string modifications. Unlike regular string concatenation, `StringBuilder` maintains an internal buffer that can be modified without creating new objects.

```csharp
// Inefficient: Creates multiple intermediate string objects
string inefficient = "";
for (int i = 0; i < 1000; i++)
{
    inefficient += $"Item {i}\n";  // Creates 1000 string objects
}

// Efficient: Uses internal buffer, minimal allocations
var efficient = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    efficient.AppendLine($"Item {i}");  // Modifies buffer in place
}
string result = efficient.ToString();  // Creates final string once
```

**StringBuilder Best Practices:**
```csharp
// Pre-allocate capacity when size is predictable
var sb = new StringBuilder(capacity: 10000);  // Avoids buffer resizing

// Use appropriate methods for different data types
sb.Append("Text");              // For strings
sb.Append(42);                  // For numbers (avoids ToString() call)
sb.AppendLine();                // For line breaks
sb.AppendFormat("Value: {0}", value);  // For formatted output

// Clear buffer for reuse
sb.Clear();  // Resets length but keeps capacity
```

**String Allocation and Garbage Collection:**
String operations can create significant memory pressure due to immutability. Understanding allocation patterns helps optimize performance.

```csharp
// High allocation scenario
public string ProcessItems(IEnumerable<Item> items)
{
    string result = "Processing: ";
    foreach (var item in items)
    {
        result += item.Name + ", ";  // Creates new string each iteration
    }
    return result.TrimEnd(',', ' ');
}

// Optimized version
public string ProcessItemsOptimized(IEnumerable<Item> items)
{
    return "Processing: " + string.Join(", ", items.Select(i => i.Name));
}

// Or using StringBuilder for complex scenarios
public string ProcessItemsComplex(IEnumerable<Item> items)
{
    var sb = new StringBuilder("Processing: ");
    var first = true;
    
    foreach (var item in items)
    {
        if (!first) sb.Append(", ");
        sb.Append(item.Name);
        if (item.IsUrgent) sb.Append(" (URGENT)");
        first = false;
    }
    
    return sb.ToString();
}
```

**Memory Optimization Techniques:**
```csharp
// Use Span<char> for substring operations without allocation
public ReadOnlySpan<char> ExtractDomain(ReadOnlySpan<char> email)
{
    int atIndex = email.IndexOf('@');
    return atIndex >= 0 ? email.Slice(atIndex + 1) : ReadOnlySpan<char>.Empty;
}

// String interning for frequently used values
private static readonly ConcurrentDictionary<string, string> _internedStrings = new();

public string GetInternedString(string value)
{
    return _internedStrings.GetOrAdd(value, string.Intern);
}

// Use string.Empty instead of "" for consistency
string empty = string.Empty;  // References single static instance
```

**Performance Measurement and Analysis:**
```csharp
public class StringPerformanceAnalysis
{
    public void CompareStringOperations()
    {
        const int iterations = 10000;
        var stopwatch = new Stopwatch();
        
        // Test string concatenation
        stopwatch.Start();
        string concat = "";
        for (int i = 0; i < iterations; i++)
        {
            concat += i.ToString();
        }
        stopwatch.Stop();
        Console.WriteLine($"String concatenation: {stopwatch.ElapsedMilliseconds}ms");
        
        // Test StringBuilder
        stopwatch.Restart();
        var sb = new StringBuilder();
        for (int i = 0; i < iterations; i++)
        {
            sb.Append(i);
        }
        string sbResult = sb.ToString();
        stopwatch.Stop();
        Console.WriteLine($"StringBuilder: {stopwatch.ElapsedMilliseconds}ms");
        
        // Test string.Join
        stopwatch.Restart();
        var numbers = Enumerable.Range(0, iterations).Select(i => i.ToString());
        string joined = string.Join("", numbers);
        stopwatch.Stop();
        Console.WriteLine($"String.Join: {stopwatch.ElapsedMilliseconds}ms");
    }
}
```

**Modern Performance Features:**
```csharp
// Span<T> for zero-allocation string processing (C# 7.2+)
public bool IsValidEmailFormat(ReadOnlySpan<char> email)
{
    int atIndex = email.IndexOf('@');
    if (atIndex <= 0 || atIndex == email.Length - 1) return false;
    
    ReadOnlySpan<char> localPart = email.Slice(0, atIndex);
    ReadOnlySpan<char> domainPart = email.Slice(atIndex + 1);
    
    return localPart.Length > 0 && domainPart.Contains('.');
}

// String interpolation with format providers for performance
public string FormatCurrency(decimal amount)
{
    return $"{amount:C}";  // Uses current culture
}

public string FormatCurrencyInvariant(decimal amount)
{
    return string.Create(CultureInfo.InvariantCulture, $"{amount:C}");
}
```

Understanding these performance considerations enables developers to make informed decisions about string handling strategies based on their specific use cases and performance requirements.
## Comprehensive Examples and Practical Applications

### Character Operations and Classification

**Advanced Character Processing:**
```

```csharp
public class CharacterProcessor
{
    public string ProcessPassword(string input)
    {
        var result = new StringBuilder();
        
        foreach (char c in input)
        {
            if (char.IsLetter(c))
            {
                // Rotate letters for simple encoding
                if (char.IsUpper(c))
                {
                    result.Append((char)(((c - 'A' + 13) % 26) + 'A'));
                }
                else
                {
                    result.Append((char)(((c - 'a' + 13) % 26) + 'a'));
                }
            }
            else if (char.IsDigit(c))
            {
                // Reverse digits
                result.Append((char)('9' - c + '0'));
            }
            else
            {
                // Keep special characters as-is
                result.Append(c);
            }
        }
        
        return result.ToString();
    }
    
    public ValidationResult ValidateIdentifier(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
            return ValidationResult.Failure("Identifier cannot be empty");
        
        if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
            return ValidationResult.Failure("Identifier must start with letter or underscore");
        
        for (int i = 1; i < identifier.Length; i++)
        {
            char c = identifier[i];
            if (!char.IsLetterOrDigit(c) && c != '_')
                return ValidationResult.Failure($"Invalid character '{c}' at position {i}");
        }
        
        return ValidationResult.Success();
    }
}
```

### Advanced String Manipulation Techniques

**String Parsing and Tokenization:**
```csharp
public class TextParser
{
    public Dictionary<string, string> ParseKeyValuePairs(string input, char separator = '=', char delimiter = ';')
    {
        var result = new Dictionary<string, string>();
        
        if (string.IsNullOrWhiteSpace(input))
            return result;
        
        var pairs = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string pair in pairs)
        {
            var parts = pair.Split(separator, 2, StringSplitOptions.TrimEntries);
            if (parts.Length == 2)
            {
                result[parts[0]] = parts[1];
            }
        }
        
        return result;
    }
    
    public List<string> ExtractQuotedStrings(string input)
    {
        var results = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;
        bool escapeNext = false;
        
        foreach (char c in input)
        {
            if (escapeNext)
            {
                current.Append(c);
                escapeNext = false;
            }
            else if (c == '\\')
            {
                escapeNext = true;
            }
            else if (c == '"')
            {
                if (inQuotes)
                {
                    results.Add(current.ToString());
                    current.Clear();
                }
                inQuotes = !inQuotes;
            }
            else if (inQuotes)
            {
                current.Append(c);
            }
        }
        
        return results;
    }
}
```
```csharp
// Basic string literals
string greeting = "Hello, World!";
string empty = "";
string nullString = null;

// Escape sequences in strings
string path = "C:\\Users\\Documents\\file.txt";
string quote = "She said, \"Hello!\"";
string multiLine = "Line 1\nLine 2\nLine 3";
string tab = "Column1\tColumn2\tColumn3";

// Verbatim strings - ignore escape sequences
string verbatimPath = @"C:\Users\Documents\file.txt";
string verbatimMultiLine = @"This string
spans multiple
lines without escape sequences";

// Raw string literals (C# 11+) - ultimate flexibility
string rawString = """
    This is a raw string literal.
    It can contain "quotes" and \backslashes
    without any escaping needed.
    """;

string jsonExample = """
    {
        "name": "John Doe",
        "age": 30,
        "city": "New York"
    }
    """;
```

### String Interpolation and Formatting
```csharp
// Basic string interpolation
string name = "Alice";
int age = 25;
string message = $"Hello, {name}! You are {age} years old.";

// Complex expressions in interpolation
decimal price = 99.95m;
string formatted = $"Price: {price:C}"; // Currency formatting
string dateFormatted = $"Today: {DateTime.Now:yyyy-MM-dd}";

// Multi-line interpolated strings
string report = $@"
Customer Report
===============
Name: {name}
Age: {age}
Registration Date: {DateTime.Now:d}
Status: {"Active"}
";

// Advanced formatting with alignment and format specifiers
string table = $@"
{"Name",-10} {"Age",3} {"Score",8:F2}
{"-".PadRight(10, '-'),-10} {"-".PadRight(3, '-'),3} {"-".PadRight(8, '-'),8}
{"Alice",-10} {25,3} {95.67,8:F2}
{"Bob",-10} {30,3} {87.23,8:F2}
";

// Conditional expressions in interpolation
bool isVip = true;
string status = $"Customer Status: {(isVip ? "VIP" : "Regular")}";
```

### String Concatenation Methods
```csharp
// Operator concatenation
string first = "Hello";
string second = "World";
string combined = first + " " + second; // "Hello World"

// String.Concat method
string result1 = string.Concat(first, " ", second);
string result2 = string.Concat("Values: ", 1, 2, 3); // Handles different types

// String.Join for collections
string[] words = { "Apple", "Banana", "Cherry" };
string joined = string.Join(", ", words); // "Apple, Banana, Cherry"

// StringBuilder for multiple operations
StringBuilder sb = new StringBuilder();
sb.Append("Building ");
sb.Append("a string ");
sb.AppendLine("with StringBuilder");
sb.AppendFormat("Number: {0}", 42);
string built = sb.ToString();

// Performance comparison demonstration
void CompareStringBuilding(string[] parts)
{
    // Inefficient for many concatenations
    string result = "";
    foreach (string part in parts)
    {
        result += part; // Creates new string each time
    }
    
    // Efficient for many concatenations
    StringBuilder sb = new StringBuilder();
    foreach (string part in parts)
    {
        sb.Append(part); // Modifies existing buffer
    }
    string efficient = sb.ToString();
}
```

### String Comparison and Equality
```csharp
// Reference vs value equality
string str1 = "Hello";
string str2 = "Hello";
string str3 = new string("Hello".ToCharArray());

bool referenceEqual = ReferenceEquals(str1, str2); // true (interned)
bool referenceEqual2 = ReferenceEquals(str1, str3); // false (different objects)
bool valueEqual = str1 == str2;                    // true (value equality)
bool valueEqual2 = str1.Equals(str3);              // true (value equality)

// Case-sensitive vs case-insensitive comparison
string upper = "HELLO";
string lower = "hello";

bool caseSensitive = upper == lower;                          // false
bool caseInsensitive = upper.Equals(lower, StringComparison.OrdinalIgnoreCase); // true

// Cultural comparison considerations
string german1 = "Straße";
string german2 = "STRASSE";
bool culturalEqual = string.Equals(german1, german2, StringComparison.CurrentCultureIgnoreCase);

// Comparison methods for sorting
string[] names = { "Alice", "bob", "Charlie" };
Array.Sort(names, StringComparer.OrdinalIgnoreCase);

// StartsWith, EndsWith, Contains
string text = "The quick brown fox";
bool startsWithThe = text.StartsWith("The");           // true
bool endsWithFox = text.EndsWith("fox");               // true
bool containsBrown = text.Contains("brown");           // true
bool containsIgnoreCase = text.Contains("BROWN", StringComparison.OrdinalIgnoreCase); // true
```

### Modern String Features (C# 11+)
```csharp
// UTF-8 string literals for performance
ReadOnlySpan<byte> utf8 = "Hello, World!"u8;

// Raw string interpolation
string name = "World";
string rawInterpolated = $$"""
    Hello, {{name}}!
    This is a raw string with interpolation.
    """;

// List patterns with strings (C# 11)
string ProcessCommand(string[] args) => args switch
{
    ["help"] => "Displaying help...",
    ["create", var name] => $"Creating {name}...",
    ["delete", var name, "confirm"] => $"Deleting {name}...",
    _ => "Unknown command"
};
```

## Tips

### Performance Considerations
- **String immutability** means each concatenation creates a new object
- **StringBuilder** is essential for multiple string operations
- **String interning** optimizes memory for literals but isn't automatic for runtime strings
- **Span<char>** provides zero-allocation string slicing for performance-critical code

```csharp
// Efficient string building
StringBuilder sb = new StringBuilder(capacity: 1000); // Pre-allocate capacity
foreach (var item in largeCollection)
{
    sb.AppendLine($"Processing {item}");
}

// Use Span<char> for string manipulation without allocation
string text = "Hello, World!";
ReadOnlySpan<char> span = text.AsSpan(7, 5); // "World" - no allocation

// String interning for frequently used strings
string frequentString = string.Intern(computedString);
```

### Memory Management
```csharp
// Understand string pooling
string literal1 = "Hello";     // Goes to string pool
string literal2 = "Hello";     // References same pooled string
string computed = "Hel" + "lo"; // Also goes to pool (compile-time constant)
string runtime = GetString();   // New string object each time

// Use string.Empty instead of ""
string empty1 = string.Empty;  // References static field
string empty2 = "";            // Literal (also pooled)

// Be careful with string operations in loops
// Inefficient
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += i.ToString(); // Creates 1000 string objects
}

// Efficient
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append(i);
}
string result = sb.ToString();
```

### Cultural Considerations
```csharp
// Be explicit about string comparison culture
bool IsValidEmail(string email)
{
    return email.EndsWith("@company.com", StringComparison.OrdinalIgnoreCase);
}

// Use appropriate comparison for sorting
var sortedNames = names.OrderBy(n => n, StringComparer.CurrentCulture);

// Consider Turkish I problem and other cultural issues
string turkish = "İstanbul";
bool problem = turkish.ToUpper() == "ISTANBUL"; // May be false in Turkish culture
bool correct = string.Equals(turkish, "istanbul", StringComparison.OrdinalIgnoreCase);
```

## Best Practices & Guidelines

### 1. String Creation and Initialization
```csharp
// Use string interpolation for readability
string message = $"Welcome, {user.Name}! You have {user.MessageCount} messages.";

// Use verbatim strings for paths and regex
string filePath = @"C:\Users\Documents\MyFile.txt";
string regexPattern = @"\d{3}-\d{3}-\d{4}"; // Phone number pattern

// Use raw strings for complex literals (C# 11+)
string sqlQuery = """
    SELECT u.Name, u.Email, COUNT(o.Id) as OrderCount
    FROM Users u
    LEFT JOIN Orders o ON u.Id = o.UserId
    WHERE u.IsActive = 1
    GROUP BY u.Id, u.Name, u.Email
    """;
```

### 2. String Comparison Best Practices
```csharp
public class StringComparisonExamples
{
    // Use appropriate StringComparison for your use case
    public bool IsConfigValue(string key, string expectedValue)
    {
        // Ordinal for configuration keys (performance)
        return string.Equals(key, expectedValue, StringComparison.Ordinal);
    }
    
    public bool IsUserInputMatch(string input, string target)
    {
        // OrdinalIgnoreCase for user input (culture-independent)
        return string.Equals(input, target, StringComparison.OrdinalIgnoreCase);
    }
    
    public int CompareDisplayNames(string name1, string name2)
    {
        // CurrentCulture for display to users
        return string.Compare(name1, name2, StringComparison.CurrentCulture);
    }
}
```

### 3. StringBuilder Usage Guidelines
```csharp
public class StringBuilderBestPractices
{
    // Pre-allocate capacity when size is known
    public string BuildReport(IEnumerable<DataRow> rows)
    {
        var estimatedSize = rows.Count() * 50; // Estimate based on data
        var sb = new StringBuilder(estimatedSize);
        
        sb.AppendLine("Data Report");
        sb.AppendLine("==========");
        
        foreach (var row in rows)
        {
            sb.AppendLine($"{row.Id}: {row.Name} - {row.Value:C}");
        }
        
        return sb.ToString();
    }
    
    // Use StringBuilder for conditional building
    public string BuildSqlWhere(SearchCriteria criteria)
    {
        var sb = new StringBuilder("WHERE 1=1");
        
        if (!string.IsNullOrEmpty(criteria.Name))
        {
            sb.Append(" AND Name LIKE @Name");
        }
        
        if (criteria.MinAge.HasValue)
        {
            sb.Append(" AND Age >= @MinAge");
        }
        
        if (criteria.IsActive.HasValue)
        {
            sb.Append(" AND IsActive = @IsActive");
        }
        
        return sb.ToString();
    }
}
```

## Real-World Applications

### 1. Text Processing and Parsing
```csharp
public class CsvParser
{
    public List<Dictionary<string, string>> ParseCsv(string csvContent)
    {
        var result = new List<Dictionary<string, string>>();
        var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        if (lines.Length == 0) return result;
        
        // Parse header
        var headers = ParseCsvLine(lines[0]);
        
        // Parse data rows
        for (int i = 1; i < lines.Length; i++)
        {
            var values = ParseCsvLine(lines[i]);
            var row = new Dictionary<string, string>();
            
            for (int j = 0; j < Math.Min(headers.Length, values.Length); j++)
            {
                row[headers[j]] = values[j];
            }
            
            result.Add(row);
        }
        
        return result;
    }
    
    private string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var sb = new StringBuilder();
        bool inQuotes = false;
        
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(sb.ToString().Trim());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }
        
        result.Add(sb.ToString().Trim());
        return result.ToArray();
    }
}
```

### 2. Configuration and Template Processing
```csharp
public class TemplateProcessor
{
    public string ProcessTemplate(string template, Dictionary<string, object> variables)
    {
        var result = new StringBuilder(template);
        
        foreach (var variable in variables)
        {
            var placeholder = $"{{{variable.Key}}}";
            var value = variable.Value?.ToString() ?? "";
            result.Replace(placeholder, value);
        }
        
        return result.ToString();
    }
}

public class ConfigurationBuilder
{
    private readonly StringBuilder _config = new();
    
    public ConfigurationBuilder AddSection(string sectionName)
    {
        _config.AppendLine($"[{sectionName}]");
        return this;
    }
    
    public ConfigurationBuilder AddValue(string key, object value)
    {
        _config.AppendLine($"{key}={value}");
        return this;
    }
    
    public string Build() => _config.ToString();
}

// Usage
var config = new ConfigurationBuilder()
    .AddSection("Database")
    .AddValue("ConnectionString", "Server=localhost;Database=MyApp")
    .AddValue("Timeout", 30)
    .AddSection("Logging")
    .AddValue("Level", "Info")
    .Build();
```

### 3. Text Validation and Sanitization
```csharp
public class TextValidator
{
    public ValidationResult ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return ValidationResult.Failure("Email is required");
        }
        
        if (!email.Contains('@'))
        {
            return ValidationResult.Failure("Email must contain @ symbol");
        }
        
        var parts = email.Split('@');
        if (parts.Length != 2)
        {
            return ValidationResult.Failure("Email must have exactly one @ symbol");
        }
        
        if (string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
        {
            return ValidationResult.Failure("Email parts cannot be empty");
        }
        
        return ValidationResult.Success();
    }
    
    public string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
        
        var sb = new StringBuilder();
        
        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || ".,!?-".Contains(c))
            {
                sb.Append(c);
            }
        }
        
        return sb.ToString().Trim();
    }
}
```


## Industry Applications

### Web Development
- **URL Processing**: Path parsing and query string handling
- **Template Engines**: Dynamic content generation
- **Input Validation**: User data sanitization and validation
- **API Response Formatting**: JSON and XML string building

### Data Processing
- **CSV/TSV Parsing**: Structured data extraction
- **Log Analysis**: Text pattern matching and extraction
- **Report Generation**: Dynamic document creation
- **Data Transformation**: Format conversion and cleanup

### System Integration
- **Configuration Management**: Settings file processing
- **Protocol Implementation**: Text-based communication protocols
- **File Processing**: Text file reading, writing, and manipulation
- **Command Line Interfaces**: Argument parsing and response formatting

