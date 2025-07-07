using System.Globalization;
using System.Text;

// Formatting and Parsing: Data Conversion in .NET
// This comprehensive demo covers all aspects of converting between strings and other data types
// We'll explore everything from basic ToString() to advanced custom format providers

namespace FormattingAndParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FORMATTING AND PARSING IN .NET ===");
            Console.WriteLine("Complete demonstration of data conversion techniques\n");

            // Start with the fundamentals - every developer needs to understand these
            DemonstrateBasicToStringAndParse();
            
            // Safety first - TryParse prevents those nasty exceptions
            DemonstrateTryParseMethod();
            
            // Culture matters - your app will be used worldwide
            DemonstrateCultureSensitiveParsing();
            
            // Taking control - format providers give you precision
            DemonstrateFormatProviders();
            
            // Numbers everywhere - master numeric formatting
            DemonstrateAdvancedNumberFormatting();
            
            // Time is money - DateTime formatting expertise
            DemonstrateAdvancedDateTimeFormatting();
            
            // String composition - building complex formatted strings
            DemonstrateCompositeFormatting();
            
            // Going custom - create your own formatting logic
            DemonstrateCustomFormatProvider();
            
            // Real world application - practical scenarios you'll encounter
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== DEMONSTRATION COMPLETE ===");
            Console.WriteLine("You now have the tools to handle any formatting challenge!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DemonstrateBasicToStringAndParse()
        {
            Console.WriteLine("1. BASIC TOSTRING() AND PARSE() FUNDAMENTALS");
            Console.WriteLine("=============================================");

            // ToString() is available on virtually all types in .NET
            // For primitive value types, it provides meaningful string output
            bool isActive = true;
            int quantity = 42;
            double price = 19.99;
            DateTime orderDate = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            Console.WriteLine("Converting values TO strings using ToString():");
            Console.WriteLine($"  Boolean: {isActive} -> \"{isActive.ToString()}\"");
            Console.WriteLine($"  Integer: {quantity} -> \"{quantity.ToString()}\"");
            Console.WriteLine($"  Double: {price} -> \"{price.ToString()}\"");
            Console.WriteLine($"  DateTime: {orderDate} -> \"{orderDate.ToString()}\"");
            Console.WriteLine($"  Guid: {transactionId} -> \"{transactionId.ToString()}\"");

            // Many types provide static Parse() methods for converting strings back
            // These methods throw FormatException if parsing fails
            string boolString = "True";
            string intString = "123";
            string doubleString = "45.67";
            string dateString = "2024-05-29";

            Console.WriteLine("\nConverting strings BACK to values using Parse():");
            
            try
            {
                bool parsedBool = bool.Parse(boolString);
                int parsedInt = int.Parse(intString);
                double parsedDouble = double.Parse(doubleString);
                DateTime parsedDate = DateTime.Parse(dateString);

                Console.WriteLine($"  \"{boolString}\" -> {parsedBool} (type: {parsedBool.GetType().Name})");
                Console.WriteLine($"  \"{intString}\" -> {parsedInt} (type: {parsedInt.GetType().Name})");
                Console.WriteLine($"  \"{doubleString}\" -> {parsedDouble} (type: {parsedDouble.GetType().Name})");
                Console.WriteLine($"  \"{dateString}\" -> {parsedDate:yyyy-MM-dd} (type: {parsedDate.GetType().Name})");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"  Parsing error: {ex.Message}");
            }

            // Here's what happens when Parse() encounters invalid input
            Console.WriteLine("\nWhat happens with invalid input using Parse():");
            try
            {
                int invalidNumber = int.Parse("not_a_number");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"  Attempting to parse 'not_a_number' as int throws: {ex.GetType().Name}");
                Console.WriteLine($"  Message: {ex.Message}");
            }

            Console.WriteLine();
        }

        static void DemonstrateTryParseMethod()
        {
            Console.WriteLine("2. TRYPARSE() - SAFE PARSING WITHOUT EXCEPTIONS");
            Console.WriteLine("================================================");

            // TryParse is safer and more efficient for error-prone scenarios
            // It returns bool indicating success/failure, with parsed value via out parameter
            // No exceptions are thrown on failure - much better for user input validation
            
            string[] testInputs = { "123", "45.67", "not_a_number", "", "999999999999999999999", "(123)" };

            Console.WriteLine("Testing various inputs with TryParse - notice no exceptions:");
            
            foreach (string input in testInputs)
            {
                Console.WriteLine($"\nTesting input: \"{input}\"");

                // Try parsing as integer
                bool intSuccess = int.TryParse(input, out int intResult);
                if (intSuccess)
                {
                    Console.WriteLine($"  ✓ Successfully parsed as int: {intResult}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Failed to parse as int (result defaulted to {intResult})");
                }

                // Try parsing as double
                bool doubleSuccess = double.TryParse(input, out double doubleResult);
                if (doubleSuccess)
                {
                    Console.WriteLine($"  ✓ Successfully parsed as double: {doubleResult}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Failed to parse as double (result defaulted to {doubleResult})");
                }
            }

            // Using discard pattern when you only care about success/failure
            Console.WriteLine("\nUsing discard pattern (when you don't need the parsed value):");
            string[] validationInputs = { "123", "abc", "456" };
            
            foreach (string input in validationInputs)
            {
                bool isValidInteger = int.TryParse(input, out int _); // Using discard '_'
                Console.WriteLine($"  \"{input}\" is valid integer: {isValidInteger}");
            }

            // Practical example: Age validation with business logic
            Console.WriteLine("\nPractical example - Age validation with business rules:");
            string[] ageInputs = { "25", "0", "-5", "150", "twenty-five", "18" };

            foreach (string ageInput in ageInputs)
            {
                if (int.TryParse(ageInput, out int age) && age >= 0 && age <= 120)
                {
                    Console.WriteLine($"  \"{ageInput}\" -> Valid age: {age}");
                }
                else if (int.TryParse(ageInput, out age))
                {
                    Console.WriteLine($"  \"{ageInput}\" -> Invalid age range: {age}");
                }
                else
                {
                    Console.WriteLine($"  \"{ageInput}\" -> Not a valid number");
                }
            }

            Console.WriteLine();
        }

        static void DemonstrateCultureSensitiveParsing()
        {
            Console.WriteLine("3. CULTURE SENSITIVITY - THE INTERNATIONAL CHALLENGE");
            Console.WriteLine("======================================================");

            // By default, Parse() and TryParse() respect local culture settings
            // This can lead to unexpected results in international applications!
            
            double number = 1234.56;
            DateTime date = new DateTime(2024, 5, 29, 14, 30, 0);

            // Demonstrate how different cultures format the same data differently
            CultureInfo usCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo germanCulture = CultureInfo.GetCultureInfo("de-DE");
            CultureInfo frenchCulture = CultureInfo.GetCultureInfo("fr-FR");
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            Console.WriteLine($"Number {number} formatted in different cultures:");
            Console.WriteLine($"  US (en-US): {number.ToString("N2", usCulture)} (comma thousands, dot decimal)");
            Console.WriteLine($"  German (de-DE): {number.ToString("N2", germanCulture)} (dot thousands, comma decimal)");
            Console.WriteLine($"  French (fr-FR): {number.ToString("N2", frenchCulture)} (space thousands, comma decimal)");
            Console.WriteLine($"  Invariant: {number.ToString("N2", invariantCulture)} (consistent everywhere)");

            Console.WriteLine($"\nDate {date:yyyy-MM-dd HH:mm} formatted differently:");
            Console.WriteLine($"  US: {date.ToString("d", usCulture)}");
            Console.WriteLine($"  German: {date.ToString("d", germanCulture)}");
            Console.WriteLine($"  French: {date.ToString("d", frenchCulture)}");

            // The parsing challenge - same number, different string representations
            Console.WriteLine("\nParsing culture-specific number formats:");
            
            string usNumber = "1,234.56";     // US format: comma thousands, dot decimal
            string germanNumber = "1.234,56"; // German format: dot thousands, comma decimal

            // Parse using appropriate cultures
            if (double.TryParse(usNumber, NumberStyles.Number, usCulture, out double usResult))
            {
                Console.WriteLine($"  US format \"{usNumber}\" -> {usResult}");
            }

            if (double.TryParse(germanNumber, NumberStyles.Number, germanCulture, out double germanResult))
            {
                Console.WriteLine($"  German format \"{germanNumber}\" -> {germanResult}");
            }

            // What happens if you use the wrong culture?
            Console.WriteLine("\nCulture mismatch problems:");
            if (double.TryParse(germanNumber, NumberStyles.Number, usCulture, out double wrongResult))
            {
                Console.WriteLine($"  German number with US culture: {wrongResult} (WRONG!)");
            }
            else
            {
                Console.WriteLine($"  German number with US culture: Parse failed (good protection)");
            }

            // InvariantCulture - your safety net for consistent behavior
            Console.WriteLine("\nUsing InvariantCulture for consistency:");
            string invariantNumber = "1234.56";
            double invariantResult = double.Parse(invariantNumber, CultureInfo.InvariantCulture);
            Console.WriteLine($"  \"{invariantNumber}\" (invariant) -> {invariantResult}");
            Console.WriteLine("  Always use InvariantCulture for data exchange, file formats, APIs!");

            Console.WriteLine();
        }

        static void DemonstrateFormatProviders()
        {
            Console.WriteLine("4. FORMAT PROVIDERS - GRANULAR CONTROL OVER FORMATTING");
            Console.WriteLine("=======================================================");

            // Format providers implement IFormatProvider interface
            // They interpret format strings and apply regional settings
            // Core interface: IFormattable.ToString(string format, IFormatProvider formatProvider)
            
            decimal amount = 1234.56m;

            // NumberFormatInfo - controls numeric formatting
            NumberFormatInfo customCurrency = new NumberFormatInfo();
            customCurrency.CurrencySymbol = "IDR ";
            customCurrency.CurrencyDecimalDigits = 0;
            customCurrency.CurrencyGroupSeparator = ".";
            customCurrency.CurrencyDecimalSeparator = ",";

            Console.WriteLine("Custom NumberFormatInfo for currency:");
            Console.WriteLine($"  Default: {amount:C}");
            Console.WriteLine($"  Indonesian Rupiah: {amount.ToString("C", customCurrency)}");

            // Modifying existing format providers via cloning
            NumberFormatInfo clonedFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            clonedFormat.NumberGroupSeparator = " ";  // Space instead of comma
            clonedFormat.NumberDecimalSeparator = ","; // Comma instead of dot

            int largeNumber = 1234567;
            Console.WriteLine($"\nCustom number grouping:");
            Console.WriteLine($"  Default: {largeNumber:N0}");
            Console.WriteLine($"  Space-separated: {largeNumber.ToString("N0", clonedFormat)}");

            // CultureInfo acts as an intermediary format provider
            CultureInfo ukCulture = CultureInfo.GetCultureInfo("en-GB");
            Console.WriteLine($"\nUsing CultureInfo as format provider:");
            Console.WriteLine($"  UK currency: {amount.ToString("C", ukCulture)}");

            // DateTimeFormatInfo for date/time formatting
            DateTimeFormatInfo customDateFormat = new DateTimeFormatInfo();
            customDateFormat.ShortDatePattern = "dd/MM/yyyy";
            customDateFormat.LongTimePattern = "HH:mm:ss";

            DateTime now = DateTime.Now;
            Console.WriteLine($"\nCustom DateTimeFormatInfo:");
            Console.WriteLine($"  Default: {now:G}");
            Console.WriteLine($"  Custom: {now.ToString("G", customDateFormat)}");

            Console.WriteLine();
        }

        static void DemonstrateAdvancedNumberFormatting()
        {
            Console.WriteLine("5. ADVANCED NUMBER FORMATTING MASTERY");
            Console.WriteLine("======================================");

            int integer = 42;
            double floating = 1234.5678;
            decimal money = 19.99m;

            // Standard numeric format strings - the building blocks
            Console.WriteLine("Standard format strings:");
            Console.WriteLine($"  Integer {integer}:");
            Console.WriteLine($"    Currency: {integer:C}");
            Console.WriteLine($"    Decimal: {integer:D5}");          // Pad with zeros
            Console.WriteLine($"    Exponential: {integer:E}");
            Console.WriteLine($"    Fixed-point: {integer:F2}");
            Console.WriteLine($"    General: {integer:G}");
            Console.WriteLine($"    Number: {integer:N}");
            Console.WriteLine($"    Percent: {integer:P}");
            Console.WriteLine($"    Hexadecimal: {integer:X}");

            Console.WriteLine($"\n  Double {floating}:");
            Console.WriteLine($"    Currency: {floating:C}");
            Console.WriteLine($"    Exponential: {floating:E2}");
            Console.WriteLine($"    Fixed-point: {floating:F2}");
            Console.WriteLine($"    Number: {floating:N2}");
            Console.WriteLine($"    Percent: {floating:P1}");

            Console.WriteLine($"\n  Decimal (money) {money}:");
            Console.WriteLine($"    Currency: {money:C}");
            Console.WriteLine($"    Fixed-point: {money:F4}");
            Console.WriteLine($"    Number: {money:N}");
            Console.WriteLine($"    Percent: {money:P2}");

            // Custom numeric format strings - unleash your creativity
            Console.WriteLine("\nCustom format strings:");
            Console.WriteLine($"  Phone number format: {1234567890:###-###-####}");
            Console.WriteLine($"  Padded number: {42:00000}");
            Console.WriteLine($"  Conditional format positive: {15:#;(#);zero}");
            Console.WriteLine($"  Conditional format negative: {-15:#;(#);zero}");
            Console.WriteLine($"  Conditional format zero: {0:#;(#);zero}");

            // NumberStyles for parsing - control what patterns are allowed
            Console.WriteLine("\nAdvanced parsing with NumberStyles:");
            
            // Parse numbers with parentheses (accounting style)
            int negativeNumber = int.Parse("(42)", NumberStyles.Integer | NumberStyles.AllowParentheses);
            Console.WriteLine($"  Parsed \"(42)\" with parentheses: {negativeNumber}");

            // Parse currency values
            decimal currencyValue = decimal.Parse("$1,234.56", NumberStyles.Currency, 
                CultureInfo.GetCultureInfo("en-US"));
            Console.WriteLine($"  Parsed \"$1,234.56\" as currency: {currencyValue}");

            // Parse with leading/trailing whitespace
            double trimmedNumber = double.Parse("  123.45  ", NumberStyles.Float | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
            Console.WriteLine($"  Parsed \"  123.45  \" with whitespace: {trimmedNumber}");

            Console.WriteLine();
        }

        static void DemonstrateAdvancedDateTimeFormatting()
        {
            Console.WriteLine("6. ADVANCED DATETIME FORMATTING EXPERTISE");
            Console.WriteLine("==========================================");

            DateTime now = DateTime.Now;
            DateTimeOffset nowOffset = DateTimeOffset.Now;

            // Standard DateTime format strings - know them all
            Console.WriteLine($"Current time: {now}");
            Console.WriteLine("\nStandard format strings:");
            Console.WriteLine($"  Short date: {now:d}");
            Console.WriteLine($"  Long date: {now:D}");
            Console.WriteLine($"  Full date/time (short): {now:f}");
            Console.WriteLine($"  Full date/time (long): {now:F}");
            Console.WriteLine($"  General (short): {now:g}");
            Console.WriteLine($"  General (long): {now:G}");
            Console.WriteLine($"  Month/day: {now:M}");
            Console.WriteLine($"  Round-trip: {now:O}");
            Console.WriteLine($"  RFC1123: {now:R}");
            Console.WriteLine($"  Sortable: {now:s}");
            Console.WriteLine($"  Short time: {now:t}");
            Console.WriteLine($"  Long time: {now:T}");
            Console.WriteLine($"  Universal sortable: {now:u}");
            Console.WriteLine($"  Universal full: {now:U}");
            Console.WriteLine($"  Year/month: {now:Y}");

            // Custom DateTime format strings for specific needs
            Console.WriteLine("\nCustom format strings for common scenarios:");
            Console.WriteLine($"  Database format: {now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Human readable: {now:dddd, MMMM dd, yyyy}");
            Console.WriteLine($"  Time only: {now:HH:mm:ss}");
            Console.WriteLine($"  12-hour format: {now:h:mm:ss tt}");
            Console.WriteLine($"  ISO 8601: {now:yyyy-MM-ddTHH:mm:ssZ}");
            Console.WriteLine($"  File-safe: {now:yyyy-MM-dd_HH-mm-ss}");

            // DateTimeOffset for timezone-aware applications
            Console.WriteLine($"\nDateTimeOffset formatting (timezone-aware):");
            Console.WriteLine($"  With offset: {nowOffset:yyyy-MM-dd HH:mm:ss zzz}");
            Console.WriteLine($"  UTC: {nowOffset.UtcDateTime:yyyy-MM-dd HH:mm:ss} UTC");

            // DateTimeStyles for parsing flexibility
            Console.WriteLine("\nAdvanced parsing with DateTimeStyles:");
            
            string[] dateFormats = { "2024-05-29", "29/05/2024", "May 29, 2024" };
            foreach (string dateStr in dateFormats)
            {
                if (DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
                {
                    Console.WriteLine($"  Parsed \"{dateStr}\" -> {parsed:yyyy-MM-dd}");
                }
            }

            Console.WriteLine();
        }

        static void DemonstrateCompositeFormatting()
        {
            Console.WriteLine("7. COMPOSITE FORMATTING - STRING.FORMAT AND INTERPOLATION");
            Console.WriteLine("==========================================================");

            // Composite formatting combines literal text with format items {}
            // These methods accept optional IFormatProvider for consistent formatting
            
            string customerName = "Alice Johnson";
            int orderNumber = 12345;
            decimal orderTotal = 156.78m;
            DateTime orderDate = DateTime.Now;

            // Traditional string.Format approach - still widely used
            string formatted1 = string.Format(
                "Order #{0} for {1} totaling {2:C} was placed on {3:d}",
                orderNumber, customerName, orderTotal, orderDate);
            
            Console.WriteLine("string.Format example:");
            Console.WriteLine($"  {formatted1}");

            // Using format providers with string.Format for consistent culture
            CultureInfo britishCulture = CultureInfo.GetCultureInfo("en-GB");
            string formatted2 = string.Format(britishCulture,
                "Order #{0} for {1} totaling {2:C} was placed on {3:d}",
                orderNumber, customerName, orderTotal, orderDate);

            Console.WriteLine($"\nWith British culture:");
            Console.WriteLine($"  {formatted2}");

            // Using InvariantCulture for data that needs to be machine-readable
            string machineReadable = string.Format(CultureInfo.InvariantCulture,
                "{0}|{1:yyyy-MM-dd}|{2}", orderNumber, orderDate, orderTotal);
            Console.WriteLine($"\nMachine-readable format (InvariantCulture):");
            Console.WriteLine($"  {machineReadable}");

            // Modern string interpolation - more readable but culture considerations apply
            string interpolated = $"Order #{orderNumber} for {customerName} totaling {orderTotal:C} was placed on {orderDate:d}";
            Console.WriteLine($"\nString interpolation (uses current culture):");
            Console.WriteLine($"  {interpolated}");

            // Advanced composite formatting scenarios
            Console.WriteLine("\nAdvanced formatting scenarios:");
            
            // Alignment and padding - crucial for reports and tables
            string[] products = { "Laptop", "Mouse", "Keyboard" };
            decimal[] prices = { 999.99m, 29.99m, 149.99m };

            Console.WriteLine("Product listing with alignment:");
            Console.WriteLine($"{"Product",-12} {"Price",8}");
            Console.WriteLine(new string('-', 20));
            
            for (int i = 0; i < products.Length; i++)
            {
                // Left-align product name, right-align price
                Console.WriteLine($"{products[i],-12} {prices[i],8:C}");
            }

            // Conditional formatting with format strings
            Console.WriteLine("\nConditional formatting:");
            int[] quantities = { -5, 0, 10, 100 };
            
            foreach (int qty in quantities)
            {
                // Different format for positive, negative, and zero values
                string status = string.Format("Quantity: {0:#;(#);'Out of Stock'}", qty);
                Console.WriteLine($"  {status}");
            }

            Console.WriteLine();
        }

        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("9. REAL-WORLD SCENARIOS - PUTTING IT ALL TOGETHER");
            Console.WriteLine("==================================================");

            // These scenarios demonstrate practical applications you'll encounter
            // in enterprise software development

            // Scenario 1: Processing CSV data with culture-aware parsing
            Console.WriteLine("Scenario 1: Processing international CSV sales data");
            string[] csvLines = {
                "2024-05-01,Alice,1234.56,USD",
                "2024-05-02,Bob,2.345,67,EUR",  // European format with comma decimal
                "2024-05-03,Charlie,invalid_amount,USD",
                "2024-05-04,David,(150.00),USD"  // Negative amount in parentheses
            };

            foreach (string line in csvLines)
            {
                string[] parts = line.Split(',');
                Console.WriteLine($"\nProcessing: {line}");

                // Parse date using exact format for consistency
                if (DateTime.TryParseExact(parts[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out DateTime saleDate))
                {
                    Console.WriteLine($"  Date: {saleDate:MMMM dd, yyyy}");
                }
                else
                {
                    Console.WriteLine($"  Invalid date format");
                    continue;
                }

                // Intelligent amount parsing - try different number styles
                decimal amount;
                bool parsed = false;
                
                // Try standard format first
                if (decimal.TryParse(parts[2], NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
                {
                    parsed = true;
                }
                // Try accounting format (parentheses for negative)
                else if (decimal.TryParse(parts[2], NumberStyles.Number | NumberStyles.AllowParentheses, CultureInfo.GetCultureInfo("en-US"), out amount))
                {
                    parsed = true;
                }
                // Try European format
                else if (decimal.TryParse(parts[2], NumberStyles.Number, CultureInfo.GetCultureInfo("de-DE"), out amount))
                {
                    parsed = true;
                }

                if (parsed)
                {
                    string currency = parts[3];
                    Console.WriteLine($"  Sale: {parts[1]} - {amount:N2} {currency}");
                    
                    // Format for different regions
                    if (currency == "EUR")
                    {
                        Console.WriteLine($"    European format: {amount.ToString("C", CultureInfo.GetCultureInfo("de-DE"))}");
                    }
                    else
                    {
                        Console.WriteLine($"    US format: {amount:C}");
                    }
                }
                else
                {
                    Console.WriteLine($"  Invalid amount: {parts[2]}");
                }
            }

            // Scenario 2: Configuration file processing with type inference
            Console.WriteLine("\n\nScenario 2: Smart configuration file processing");
            var configEntries = new Dictionary<string, string>
            {
                {"timeout", "30"},
                {"retryCount", "5"},
                {"enableLogging", "true"},
                {"maxFileSize", "10.5"},
                {"serverUrl", "https://api.example.com"},
                {"connectionString", "Server=localhost;Database=test"},
                {"percentage", "0.15"},
                {"negativeBudget", "(1000.00)"}
            };

            Console.WriteLine("Processing configuration entries with type detection:");
            foreach (var entry in configEntries)
            {
                Console.WriteLine($"\n  {entry.Key}: \"{entry.Value}\"");
                
                // Try parsing in order of specificity
                if (bool.TryParse(entry.Value, out bool boolValue))
                {
                    Console.WriteLine($"    -> Boolean: {boolValue}");
                }
                else if (int.TryParse(entry.Value, out int intValue))
                {
                    Console.WriteLine($"    -> Integer: {intValue:N0}");
                }
                else if (decimal.TryParse(entry.Value, NumberStyles.Number | NumberStyles.AllowParentheses, 
                    CultureInfo.InvariantCulture, out decimal decimalValue))
                {
                    Console.WriteLine($"    -> Decimal: {decimalValue:N2}");
                }
                else if (Uri.TryCreate(entry.Value, UriKind.Absolute, out Uri? uriValue))
                {
                    Console.WriteLine($"    -> URL: {uriValue}");
                }
                else
                {
                    Console.WriteLine($"    -> String: {entry.Value}");
                }
            }

            // Scenario 3: Multi-language report generation
            Console.WriteLine("\n\nScenario 3: Multi-language financial report");
            decimal revenue = 1234567.89m;
            DateTime reportDate = DateTime.Now;
            
            var reportCultures = new[]
            {
                ("United States", "en-US"),
                ("United Kingdom", "en-GB"),
                ("Germany", "de-DE"),
                ("France", "fr-FR"),
                ("Japan", "ja-JP")
            };

            Console.WriteLine($"Financial Report - Revenue: {revenue}");
            Console.WriteLine("Localized for different regions:\n");
            
            foreach (var (country, cultureName) in reportCultures)
            {
                try
                {
                    CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
                    string localizedReport = string.Format(culture,
                        "{0}: Revenue {1:C} as of {2:D}",
                        country, revenue, reportDate);
                    Console.WriteLine($"  {localizedReport}");
                }
                catch (CultureNotFoundException)
                {
                    Console.WriteLine($"  {country}: Culture not available");
                }
            }

            // Scenario 4: Log parsing with multiple timestamp formats
            Console.WriteLine("\n\nScenario 4: Robust log file timestamp parsing");
            string[] logEntries = {
                "2024-05-29 14:30:15.123 [INFO] Application started",
                "29/05/2024 14:30:16 [WARN] Configuration file missing",
                "May 29, 2024 2:30:17 PM [ERROR] Database connection failed",
                "2024-05-29T14:30:18Z [DEBUG] UTC timestamp",
                "invalid_timestamp [ERROR] This should fail gracefully"
            };

            // Define possible timestamp formats in order of preference
            string[] timestampFormats = {
                "yyyy-MM-dd HH:mm:ss.fff",     // ISO with milliseconds
                "dd/MM/yyyy HH:mm:ss",          // European format
                "MMMM dd, yyyy h:mm:ss tt",     // US long format
                "yyyy-MM-ddTHH:mm:ssZ"          // ISO UTC format
            };

            foreach (string logEntry in logEntries)
            {
                Console.WriteLine($"\nParsing: {logEntry}");
                
                bool timestampParsed = false;
                string[] logParts = logEntry.Split(' ');
                
                // Try different combinations of date/time parts
                for (int partCount = 1; partCount <= Math.Min(3, logParts.Length); partCount++)
                {
                    string timestampPart = string.Join(" ", logParts.Take(partCount));
                    
                    foreach (string format in timestampFormats)
                    {
                        if (DateTime.TryParseExact(timestampPart, format, CultureInfo.InvariantCulture, 
                            DateTimeStyles.None, out DateTime timestamp))
                        {
                            Console.WriteLine($"  ✓ Parsed with format '{format}': {timestamp:yyyy-MM-dd HH:mm:ss.fff}");
                            
                            // Extract and display log message
                            string logMessage = string.Join(" ", logParts.Skip(partCount));
                            Console.WriteLine($"    Message: {logMessage}");
                            
                            timestampParsed = true;
                            break;
                        }
                    }
                    
                    if (timestampParsed) break;
                }

                if (!timestampParsed)
                {
                    Console.WriteLine($"  ✗ Could not parse timestamp - treating as plain text");
                }
            }

            // Scenario 5: Data export with format preservation
            Console.WriteLine("\n\nScenario 5: Data export with format preservation");
            
            var exportData = new[]
            {
                new { Name = "John Doe", Age = 35, Salary = 85000.50m, StartDate = new DateTime(2020, 3, 15) },
                new { Name = "Jane Smith", Age = 28, Salary = 92500.75m, StartDate = new DateTime(2021, 8, 22) },
                new { Name = "Bob Johnson", Age = 42, Salary = 105000.00m, StartDate = new DateTime(2019, 1, 10) }
            };

            Console.WriteLine("Exporting employee data in different formats:");
            
            // CSV format (invariant culture for data integrity)
            Console.WriteLine("\nCSV Format (machine-readable):");
            Console.WriteLine("Name,Age,Salary,StartDate");
            foreach (var employee in exportData)
            {
                string csvLine = string.Format(CultureInfo.InvariantCulture,
                    "{0},{1},{2:F2},{3:yyyy-MM-dd}",
                    employee.Name, employee.Age, employee.Salary, employee.StartDate);
                Console.WriteLine(csvLine);
            }

            // Human-readable format (current culture)
            Console.WriteLine("\nHuman-Readable Report:");
            foreach (var employee in exportData)
            {
                Console.WriteLine($"Employee: {employee.Name}");
                Console.WriteLine($"  Age: {employee.Age} years");
                Console.WriteLine($"  Salary: {employee.Salary:C}");
                Console.WriteLine($"  Start Date: {employee.StartDate:D}");
                Console.WriteLine();
            }

            Console.WriteLine("Key Takeaways:");
            Console.WriteLine("1. Always use InvariantCulture for data storage and exchange");
            Console.WriteLine("2. Use specific cultures for user interface display");
            Console.WriteLine("3. TryParse methods prevent exceptions and improve performance");
            Console.WriteLine("4. Custom format providers give ultimate control when needed");
            Console.WriteLine("5. Consider NumberStyles and DateTimeStyles for flexible parsing");

            Console.WriteLine();
        }

        static void DemonstrateCustomFormatProvider()
        {
            Console.WriteLine("8. CUSTOM FORMAT PROVIDERS - ULTIMATE CONTROL");
            Console.WriteLine("===============================================");

            // Creating custom format providers by implementing IFormatProvider and ICustomFormatter
            // This allows complete control over how objects are formatted
            
            double number = -123.45;
            IFormatProvider wordyProvider = new WordyFormatProvider();
            
            Console.WriteLine("Using custom WordyFormatProvider:");
            Console.WriteLine("This converts digits to words - useful for checks, invoices, etc.");
            
            // Custom format providers only work with composite format strings
            Console.WriteLine(string.Format(wordyProvider, "{0:C} in words is {0:W}", number));
            Console.WriteLine(string.Format(wordyProvider, "Temperature: {0:W} degrees", 32.5));
            Console.WriteLine(string.Format(wordyProvider, "Count: {0:W} items", 789));

            // Demonstrating fallback to parent provider for standard formats
            Console.WriteLine($"\nStandard formats still work:");
            Console.WriteLine(string.Format(wordyProvider, "Standard currency: {0:C}", number));
            Console.WriteLine(string.Format(wordyProvider, "Standard number: {0:N2}", number));

            Console.WriteLine();
        }

        // Custom format provider implementation - converts numbers to word representation
        public class WordyFormatProvider : IFormatProvider, ICustomFormatter
        {
            private readonly IFormatProvider _parent;

            public WordyFormatProvider() : this(CultureInfo.CurrentCulture) { }

            public WordyFormatProvider(IFormatProvider parent)
            {
                _parent = parent;
            }

            public object? GetFormat(Type? formatType)
            {
                // Return this object if ICustomFormatter is requested
                if (formatType == typeof(ICustomFormatter)) return this;
                return null;
            }

            public string Format(string? format, object? arg, IFormatProvider? formatProvider)
            {
                // If the format isn't "W" (our custom format), defer to the parent provider
                if (arg == null || format != "W")
                    return string.Format(_parent, "{0:" + format + "}", arg);

                // Custom logic for converting number digits to words
                StringBuilder result = new StringBuilder();
                
                // Use invariant culture for consistent digit representation
                string digitList = string.Format(CultureInfo.InvariantCulture, "{0}", arg);

                foreach (char digit in digitList)
                {
                    switch (digit)
                    {
                        case '0': result.Append("zero "); break;
                        case '1': result.Append("one "); break;
                        case '2': result.Append("two "); break;
                        case '3': result.Append("three "); break;
                        case '4': result.Append("four "); break;
                        case '5': result.Append("five "); break;
                        case '6': result.Append("six "); break;
                        case '7': result.Append("seven "); break;
                        case '8': result.Append("eight "); break;
                        case '9': result.Append("nine "); break;
                        case '.': result.Append("point "); break;
                        case ',': result.Append("comma "); break;
                        case '-': result.Append("minus "); break;
                        default: result.Append(digit + " "); break;
                    }
                }
                
                return result.ToString().Trim();
            }
        }
    }
}
