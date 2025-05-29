# 🔤 Strings and Characters in C#

## 🎯 Learning Objectives
By the end of this module, you will master:
- **Character type** (`char`) and Unicode fundamentals
- **String type** immutability and reference semantics
- **String literals** including verbatim and raw string formats
- **String interpolation** and formatting techniques
- **String comparison** methods and cultural considerations
- **Escape sequences** for special characters
- **Performance optimization** with StringBuilder and Span<char>
- **UTF-8 strings** and modern text handling (C# 11+)

## 📚 Core Concepts Covered

### 1. Character Fundamentals
- **Unicode support**: 16-bit UTF-16 character representation
- **Character literals**: Single quotes and escape sequences
- **Character operations**: Conversion, comparison, and classification
- **ASCII vs Unicode**: Understanding character encoding differences

### 2. String Fundamentals
- **Immutability**: Strings cannot be modified after creation
- **Reference type**: String objects stored on heap
- **String interning**: Automatic optimization for string literals
- **UTF-16 encoding**: Internal representation and memory usage

### 3. String Creation and Manipulation
- **Literal syntax**: Various ways to create string constants
- **Concatenation**: Joining strings with different operators
- **Interpolation**: Embedding expressions within strings
- **StringBuilder**: Mutable string building for performance

### 4. String Comparison and Equality
- **Value equality**: Content-based comparison
- **Reference equality**: Object identity comparison
- **Cultural sensitivity**: Locale-aware string operations
- **Case sensitivity**: Options for different comparison needs

## 🚀 Key Features & Examples

### Character Basics and Operations
```csharp
// Character literals and Unicode
char letter = 'A';              // Basic character literal
char unicode = '\u0041';        // Unicode escape sequence (A)
char escaped = '\'';            // Escaped single quote
char newline = '\n';            // Escape sequence for newline

// Character operations and properties
char digit = '5';
bool isDigit = char.IsDigit(digit);           // true
bool isLetter = char.IsLetter(digit);         // false
bool isUpper = char.IsUpper('A');             // true
char lower = char.ToLower('A');               // 'a'
char upper = char.ToUpper('a');               // 'A'

// Character arithmetic and conversion
char ch = 'A';
int asciiValue = (int)ch;                     // 65
char nextChar = (char)(ch + 1);               // 'B'

// Character classification methods
bool isWhitespace = char.IsWhiteSpace(' ');   // true
bool isPunctuation = char.IsPunctuation('!'); // true
bool isSymbol = char.IsSymbol('$');           // true
```

### String Literals and Creation
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
    // ❌ Inefficient for many concatenations
    string result = "";
    foreach (string part in parts)
    {
        result += part; // Creates new string each time
    }
    
    // ✅ Efficient for many concatenations
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

## 💡 Trainer Tips

### Performance Considerations
- **String immutability** means each concatenation creates a new object
- **StringBuilder** is essential for multiple string operations
- **String interning** optimizes memory for literals but isn't automatic for runtime strings
- **Span<char>** provides zero-allocation string slicing for performance-critical code

```csharp
// ✅ Efficient string building
StringBuilder sb = new StringBuilder(capacity: 1000); // Pre-allocate capacity
foreach (var item in largeCollection)
{
    sb.AppendLine($"Processing {item}");
}

// ✅ Use Span<char> for string manipulation without allocation
string text = "Hello, World!";
ReadOnlySpan<char> span = text.AsSpan(7, 5); // "World" - no allocation

// ✅ String interning for frequently used strings
string frequentString = string.Intern(computedString);
```

### Memory Management
```csharp
// ✅ Understand string pooling
string literal1 = "Hello";     // Goes to string pool
string literal2 = "Hello";     // References same pooled string
string computed = "Hel" + "lo"; // Also goes to pool (compile-time constant)
string runtime = GetString();   // New string object each time

// ✅ Use string.Empty instead of ""
string empty1 = string.Empty;  // References static field
string empty2 = "";            // Literal (also pooled)

// ✅ Be careful with string operations in loops
// ❌ Inefficient
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += i.ToString(); // Creates 1000 string objects
}

// ✅ Efficient
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append(i);
}
string result = sb.ToString();
```

### Cultural Considerations
```csharp
// ✅ Be explicit about string comparison culture
bool IsValidEmail(string email)
{
    return email.EndsWith("@company.com", StringComparison.OrdinalIgnoreCase);
}

// ✅ Use appropriate comparison for sorting
var sortedNames = names.OrderBy(n => n, StringComparer.CurrentCulture);

// ✅ Consider Turkish I problem and other cultural issues
string turkish = "İstanbul";
bool problem = turkish.ToUpper() == "ISTANBUL"; // May be false in Turkish culture
bool correct = string.Equals(turkish, "istanbul", StringComparison.OrdinalIgnoreCase);
```

## 🎓 Best Practices & Guidelines

### 1. String Creation and Initialization
```csharp
// ✅ Use string interpolation for readability
string message = $"Welcome, {user.Name}! You have {user.MessageCount} messages.";

// ✅ Use verbatim strings for paths and regex
string filePath = @"C:\Users\Documents\MyFile.txt";
string regexPattern = @"\d{3}-\d{3}-\d{4}"; // Phone number pattern

// ✅ Use raw strings for complex literals (C# 11+)
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
    // ✅ Use appropriate StringComparison for your use case
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
    // ✅ Pre-allocate capacity when size is known
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
    
    // ✅ Use StringBuilder for conditional building
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

## 🔧 Real-World Applications

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

## 🎯 Mastery Checklist

### Fundamental Level
- [ ] Understand char vs string differences
- [ ] Use basic string literals and escape sequences
- [ ] Perform simple string concatenation
- [ ] Compare strings for equality

### Intermediate Level
- [ ] Master string interpolation and formatting
- [ ] Use StringBuilder for performance optimization
- [ ] Apply appropriate string comparison methods
- [ ] Work with verbatim and raw string literals

### Advanced Level
- [ ] Optimize string operations for performance
- [ ] Handle Unicode and cultural considerations
- [ ] Implement complex string parsing algorithms
- [ ] Use Span<char> for zero-allocation operations

### Expert Level
- [ ] Design high-performance text processing systems
- [ ] Create custom string formatting solutions
- [ ] Implement advanced string matching algorithms
- [ ] Build culture-aware text processing applications

## 💼 Industry Applications

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

## 🔗 Integration with Other Concepts

### C# Language Features
- **Regular Expressions**: Pattern matching in strings
- **LINQ**: String query and manipulation operations
- **Async/Await**: Asynchronous string I/O operations
- **Nullable Reference Types**: Null-safe string handling

### .NET Framework
- **Globalization**: Cultural string handling and localization
- **Encoding**: Character set conversion and handling
- **IO Operations**: File and stream text processing
- **Serialization**: String-based data format handling

### Performance Optimization
- **Memory Management**: String allocation and garbage collection
- **Span<T>**: Zero-allocation string slicing
- **String Interning**: Memory optimization for repeated strings
- **StringBuilder Pooling**: Reusable StringBuilder instances

---

*Master strings and characters to handle all text processing needs in C# applications. These skills are fundamental for user interfaces, data processing, and communication systems.*
