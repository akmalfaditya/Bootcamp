# String and Text Handling in C#

## 🎯 Learning Objectives

Master the fundamental skill of text manipulation! String handling is at the core of almost every application - from user input validation to data processing, API communication, and file operations.

## 📚 What You'll Learn

### Core Concepts Covered:

1. **Character and String Fundamentals**
   - **`char` type**: Unicode character representation
   - **Character categories**: Letters, digits, symbols, whitespace
   - **String immutability**: Why strings can't be changed
   - **String internment**: Memory optimization for literals

2. **String Operations**
   - **Searching**: `Contains()`, `IndexOf()`, `StartsWith()`, `EndsWith()`
   - **Manipulation**: `Substring()`, `Replace()`, `Remove()`, `Insert()`
   - **Transformation**: `ToUpper()`, `ToLower()`, `Trim()`, `PadLeft()`
   - **Splitting and joining**: `Split()`, `Join()`, array to string conversion

3. **String Formatting and Interpolation**
   - **String interpolation**: `$"Hello {name}!"` (modern approach)
   - **Composite formatting**: `string.Format()` and placeholders
   - **Format specifiers**: Numbers, dates, custom formats
   - **Culture-specific formatting**: Localization considerations

4. **String Comparison**
   - **Ordinal comparison**: Binary character comparison
   - **Cultural comparison**: Language-aware comparison
   - **Case sensitivity**: `StringComparison` enumeration
   - **Equality vs comparison**: Performance implications

5. **StringBuilder for Performance**
   - When to use `StringBuilder` vs string concatenation
   - Capacity management and performance optimization
   - Method chaining and fluent API
   - Memory allocation patterns

6. **Text Encoding**
   - **UTF-8, UTF-16, ASCII**: Character encoding schemes
   - **Byte arrays**: Converting strings to/from bytes
   - **Encoding class**: Handling different text formats
   - **BOM (Byte Order Mark)**: Unicode encoding detection

## 🚀 Key Features Demonstrated

### String Interpolation (Modern C#):
```csharp
string name = "Alice";
int age = 30;
decimal salary = 75000.50m;

// String interpolation - clean and readable
string message = $"Employee {name} is {age} years old and earns {salary:C}";

// With expressions and formatting
string report = $"Status: {DateTime.Now:yyyy-MM-dd} - {(age >= 18 ? "Adult" : "Minor")}";
```

### String Builder for Performance:
```csharp
// Inefficient - creates many string objects
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += $"Item {i}, "; // Creates new string each time!
}

// Efficient - uses mutable buffer
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append($"Item {i}, ");
}
string result = sb.ToString();
```

### String Comparison Best Practices:
```csharp
// For user interface text (case-insensitive, culture-aware)
if (userInput.Equals(expectedValue, StringComparison.CurrentCultureIgnoreCase))
{
    // Handle user input
}

// For internal identifiers (fast, exact comparison)
if (fileName.Equals(targetFile, StringComparison.Ordinal))
{
    // Process file
}

// For file paths (case-insensitive on Windows)
if (path1.Equals(path2, StringComparison.OrdinalIgnoreCase))
{
    // Same file path
}
```

### Text Processing Patterns:
```csharp
// Parsing CSV data
string csvLine = "Alice,30,Engineer,75000";
string[] fields = csvLine.Split(',');

// Building CSV data
string[] values = { "Bob", "25", "Designer", "65000" };
string csvOutput = string.Join(",", values);

// Cleaning user input
string userInput = "  Hello World!  ";
string cleaned = userInput.Trim().Replace("  ", " ");
```

## 💡 Trainer Tips

> **Immutability Impact**: Every string operation that seems to "modify" a string actually creates a new string object. For multiple concatenations, use StringBuilder or string interpolation with arrays.

> **Culture Awareness**: Use `CurrentCulture` for user-facing text and `InvariantCulture` for internal data. This prevents bugs when your application runs in different locales.

> **Performance Rule**: Use StringBuilder when you have more than 3-4 concatenations in a loop, or when building strings dynamically based on conditions.

## 🔍 What to Focus On

1. **String immutability**: Understanding why strings can't be changed
2. **Performance implications**: When to use StringBuilder vs concatenation
3. **Comparison strategies**: Choosing the right StringComparison
4. **Encoding awareness**: Handling text in different formats

## 🏃‍♂️ Run the Project

```bash
dotnet run
```

The demo showcases:
- Character type operations and Unicode handling
- All major string manipulation techniques
- Performance comparisons between approaches
- Formatting and interpolation examples
- StringBuilder optimization patterns
- Text encoding demonstrations

## 🎓 Best Practices

1. **Use string interpolation** (`$""`) for readable formatting
2. **Choose StringComparison explicitly** - don't rely on defaults
3. **Use StringBuilder** for multiple concatenations
4. **Validate input strings** before processing
5. **Consider memory allocation** in high-performance scenarios
6. **Use `string.IsNullOrEmpty()`** and `string.IsNullOrWhiteSpace()`
7. **Be explicit about encoding** when reading/writing files

## 🔧 Real-World Applications

### Common String Scenarios:
- **User input validation**: Email, phone number, password validation
- **Data parsing**: CSV, JSON, XML processing
- **Template processing**: Generating HTML, emails, reports
- **File operations**: Path manipulation, file name processing
- **API communication**: Building URLs, parsing responses
- **Logging**: Structured log message formatting

### Industry Examples:
```csharp
// Email validation pattern
bool IsValidEmail(string email) => 
    !string.IsNullOrWhiteSpace(email) && 
    email.Contains('@') && 
    email.IndexOf('@') < email.LastIndexOf('.');

// Safe file path building
string BuildSafePath(params string[] parts) => 
    Path.Combine(parts.Where(p => !string.IsNullOrWhiteSpace(p)).ToArray());

// SQL injection prevention
string SafeQuery(string userInput) => 
    userInput.Replace("'", "''").Replace(";", "");
```

## 🎯 Performance Guidelines

### String Operations Complexity:
| Operation | Time Complexity | Best Practice |
|-----------|----------------|---------------|
| Concatenation (+) | O(n) per operation | Use StringBuilder for loops |
| String.Join() | O(n) total | Preferred for joining arrays |
| StringBuilder.Append() | O(1) amortized | Best for building strings |
| String.Replace() | O(n) | Efficient for single replacements |
| String.Split() | O(n) | Consider span-based alternatives |

### Memory Considerations:
- **String literals**: Automatically interned (shared in memory)
- **Runtime strings**: Created on heap, garbage collected
- **StringBuilder**: Uses internal buffer, resizes as needed
- **Span<char>**: Stack-allocated, zero-copy string operations

## 🔮 Advanced String Features

### Span<char> for Performance (Modern C#):
```csharp
// Zero-allocation string slicing
ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path)
{
    int lastSlash = path.LastIndexOf('/');
    return lastSlash >= 0 ? path.Slice(lastSlash + 1) : path;
}
```

### String Interpolation with IFormattable:
```csharp
// Custom formatting support
public class Temperature : IFormattable
{
    public double Celsius { get; }
    public string ToString(string format, IFormatProvider provider) =>
        format?.ToUpper() switch
        {
            "F" => $"{Celsius * 9 / 5 + 32:F1}°F",
            "K" => $"{Celsius + 273.15:F1}K",
            _ => $"{Celsius:F1}°C"
        };
}
```

## 🎯 Mastery Checklist

After this project, you should confidently:
- ✅ Choose the right string operation for performance needs
- ✅ Handle text encoding and Unicode correctly
- ✅ Use string comparison appropriately for your use case
- ✅ Build efficient string processing algorithms
- ✅ Validate and sanitize user input safely
- ✅ Format numbers, dates, and custom objects properly
- ✅ Debug string-related performance issues

## 💼 Industry Applications

String handling is critical in:
- **Web Development**: URL parsing, form validation, template processing
- **Data Processing**: Log analysis, CSV/JSON parsing, text mining
- **Security**: Input sanitization, SQL injection prevention
- **Internationalization**: Multi-language support, cultural formatting
- **File Processing**: Path manipulation, content parsing
- **API Development**: Request/response formatting, data serialization

Remember: Efficient string handling is fundamental to application performance and user experience. Master these techniques to build responsive, reliable applications that handle text data effectively!
