# Formatting and Parsing in C#

## Learning Objectives
Learn the essential art of **data conversion** in C# - transforming objects to strings for display and parsing strings back to typed data. You'll learn culture-aware formatting, custom format providers, and robust parsing techniques that handle real-world data scenarios gracefully.

## What You'll Learn

### Core Conversion Concepts
- **ToString() Mastery**: Converting any object to its string representation
- **Parse() Methods**: Converting strings back to strongly-typed data
- **TryParse() Safety**: Exception-free parsing with validation
- **Culture Awareness**: Handling international data formats correctly

### Advanced Formatting Techniques
- **Standard Format Strings**: Built-in formatting patterns for common scenarios
- **Custom Format Strings**: Creating your own formatting rules
- **Composite Formatting**: String interpolation and format specifiers
- **IFormatProvider Interface**: Custom formatting behavior for specialized needs

### Parsing Strategies
- **Robust Input Handling**: Dealing with malformed, empty, or unexpected data
- **Culture-Specific Parsing**: Numbers, dates, and currencies in different locales
- **Performance Considerations**: Choosing the right parsing method for your scenario
- **Error Recovery**: Graceful handling of conversion failures

## Key Features Demonstrated

### 1. **Basic Conversion Operations**
```csharp
// Object to string - the foundation of data display
int number = 42;
string text = number.ToString();

// String to object - essential for user input processing
string input = "123";
int parsed = int.Parse(input);
```

### 2. **Safe Parsing with TryParse**
```csharp
// Exception-free parsing - preferred for user input
if (int.TryParse(userInput, out int result))
{
    // Success - use result
}
else
{
    // Handle invalid input gracefully
}
```

### 3. **Culture-Aware Formatting**
```csharp
decimal price = 1234.56m;
// US format: $1,234.56
string usDollar = price.ToString("C", CultureInfo.GetCultureInfo("en-US"));
// German format: 1.234,56 €
string euro = price.ToString("C", CultureInfo.GetCultureInfo("de-DE"));
```

### 4. **Custom Format Providers**
```csharp
// Create specialized formatting for domain-specific needs
public class CustomNumberFormatter : IFormatProvider, ICustomFormatter
{
    // Implement custom formatting logic
}
```

## Tips

### **Performance Best Practices**
- Use `StringBuilder` for multiple string concatenations during formatting
- Cache `CultureInfo` objects instead of creating them repeatedly
- Prefer `TryParse()` over `Parse()` with try-catch for user input
- Use format strings instead of string concatenation for complex formatting

### **Culture Handling Strategies**
```csharp
// Always specify culture for data storage/retrieval
string dataValue = number.ToString(CultureInfo.InvariantCulture);
int parsed = int.Parse(dataValue, CultureInfo.InvariantCulture);

// Use current culture for user display
string displayValue = number.ToString("N2", CultureInfo.CurrentCulture);
```

### **Input Validation Patterns**
```csharp
// Robust parsing with multiple validation layers
public static bool TryParseQuantity(string input, out int quantity)
{
    quantity = 0;
    
    if (string.IsNullOrWhiteSpace(input))
        return false;
        
    if (!int.TryParse(input.Trim(), out quantity))
        return false;
        
    return quantity >= 0; // Business rule validation
}
```

### **Common Formatting Pitfalls**
- **Don't**: Use `ToString()` without format specifiers for data storage
- **Do**: Always specify culture and format for consistent results
- **Don't**: Ignore culture settings in international applications
- **Do**: Use invariant culture for data persistence, current culture for display

## Real-World Applications

### **Financial Systems**
```csharp
// Currency formatting for international e-commerce
decimal orderTotal = 1299.99m;
var cultures = new[] { "en-US", "en-GB", "de-DE", "ja-JP" };

foreach (string cultureName in cultures)
{
    var culture = CultureInfo.GetCultureInfo(cultureName);
    string formatted = orderTotal.ToString("C", culture);
    // US: $1,299.99, UK: £1,299.99, Germany: 1.299,99 €, Japan: ¥1,300
}
```

### **Data Import/Export**
```csharp
// CSV file processing with culture-aware parsing
public class DataImporter
{
    public Product ParseProductLine(string csvLine, CultureInfo culture)
    {
        string[] fields = csvLine.Split(',');
        
        decimal.TryParse(fields[2], NumberStyles.Currency, culture, out decimal price);
        DateTime.TryParse(fields[3], culture, DateTimeStyles.None, out DateTime date);
        
        return new Product(fields[0], fields[1], price, date);
    }
}
```

### **User Interface Development**
```csharp
// Dynamic formatting based on user preferences
public string FormatValue(object value, string formatType, CultureInfo userCulture)
{
    return formatType switch
    {
        "currency" => ((decimal)value).ToString("C", userCulture),
        "percentage" => ((double)value).ToString("P2", userCulture),
        "shortDate" => ((DateTime)value).ToString("d", userCulture),
        _ => value.ToString()
    };
}
```

### **Configuration Management**
```csharp
// Safe configuration value parsing
public class ConfigManager
{
    public T GetValue<T>(string key, T defaultValue) where T : IParsable<T>
    {
        string configValue = GetConfigString(key);
        
        if (T.TryParse(configValue, CultureInfo.InvariantCulture, out T result))
            return result;
            
        return defaultValue;
    }
}
```

## Integration with Modern C#

### **String Interpolation (C# 6+)**
```csharp
decimal price = 42.99m;
string message = $"Price: {price:C} (as of {DateTime.Now:d})";
```

### **Raw String Literals (C# 11+)**
```csharp
string jsonTemplate = """
{
    "price": "{0:F2}",
    "date": "{1:yyyy-MM-dd}"
}
""";
```

### **Generic Math (C# 11+)**
```csharp
public static T ParseNumber<T>(string input) where T : IParsable<T>
{
    return T.Parse(input, CultureInfo.InvariantCulture);
}
```

### **UTF-8 String Literals (C# 11+)**
```csharp
ReadOnlySpan<byte> utf8Json = "{"price": 42.99}"u8;
```

## Impact

Formatting and parsing are critical because they:

- **Enable User Interfaces**: Every application needs to display data to users
- **Power Data Integration**: APIs, file imports, and system communication rely on conversion
- **Support Globalization**: International applications require culture-aware formatting
- **Ensure Data Quality**: Robust parsing prevents corrupt data from entering systems
- **Drive Business Logic**: Financial calculations, reporting, and analytics depend on accurate conversion

