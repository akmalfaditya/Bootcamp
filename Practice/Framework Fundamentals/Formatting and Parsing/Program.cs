using System.Globalization;

// Formatting and Parsing Demonstration
// This project covers converting between strings and other data types in C#

namespace FormattingAndParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FORMATTING AND PARSING DEMONSTRATION ===\n");

            // Let's explore how to convert data to strings and back
            DemonstrateBasicToStringAndParse();
            DemonstrateTryParseMethod();
            DemonstrateCultureSensitiveParsing();
            DemonstrateCustomFormatProviders();
            DemonstrateNumberFormatting();
            DemonstrateDateTimeFormatting();
            DemonstrateCompositeFormatting();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== END OF DEMONSTRATION ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DemonstrateBasicToStringAndParse()
        {
            Console.WriteLine("1. BASIC TOSTRING() AND PARSE() OPERATIONS");
            Console.WriteLine("==========================================");

            // Converting different types to strings - the foundation of data display
            bool isActive = true;
            int quantity = 42;
            double price = 19.99;
            DateTime orderDate = DateTime.Now;

            Console.WriteLine("Converting values TO strings:");
            Console.WriteLine($"  Boolean: {isActive} -> \"{isActive.ToString()}\"");
            Console.WriteLine($"  Integer: {quantity} -> \"{quantity.ToString()}\"");
            Console.WriteLine($"  Double: {price} -> \"{price.ToString()}\"");
            Console.WriteLine($"  DateTime: {orderDate} -> \"{orderDate.ToString()}\"");

            // Converting strings back to original types - essential for user input
            string boolString = "True";
            string intString = "123";
            string doubleString = "45.67";
            string dateString = "2024-05-29";

            Console.WriteLine("\nConverting strings BACK to values:");
            
            try
            {
                bool parsedBool = bool.Parse(boolString);
                int parsedInt = int.Parse(intString);
                double parsedDouble = double.Parse(doubleString);
                DateTime parsedDate = DateTime.Parse(dateString);

                Console.WriteLine($"  \"{boolString}\" -> {parsedBool} (type: {parsedBool.GetType().Name})");
                Console.WriteLine($"  \"{intString}\" -> {parsedInt} (type: {parsedInt.GetType().Name})");
                Console.WriteLine($"  \"{doubleString}\" -> {parsedDouble} (type: {parsedDouble.GetType().Name})");
                Console.WriteLine($"  \"{dateString}\" -> {parsedDate} (type: {parsedDate.GetType().Name})");
            }
            catch (FormatException ex)
            {
                // This shows what happens when parsing fails
                Console.WriteLine($"  Parsing error: {ex.Message}");
            }

            // Demonstrating what happens with invalid input
            Console.WriteLine("\nWhat happens with invalid input:");
            try
            {
                int invalidNumber = int.Parse("not_a_number");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"  Trying to parse 'not_a_number' as int: {ex.Message}");
            }

            Console.WriteLine();
        }

        static void DemonstrateTryParseMethod()
        {
            Console.WriteLine("2. TRYPARSE() - SAFE PARSING WITHOUT EXCEPTIONS");
            Console.WriteLine("===============================================");

            // TryParse is safer - it returns false instead of throwing exceptions
            // This is the preferred method when dealing with user input
            
            string[] testInputs = { "123", "45.67", "not_a_number", "", "999999999999999999999" };

            Console.WriteLine("Testing various inputs with TryParse:");
            
            foreach (string input in testInputs)
            {
                Console.WriteLine($"\nTesting input: \"{input}\"");

                // Try parsing as integer
                if (int.TryParse(input, out int intResult))
                {
                    Console.WriteLine($"  ✓ Successfully parsed as int: {intResult}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Failed to parse as int");
                }

                // Try parsing as double
                if (double.TryParse(input, out double doubleResult))
                {
                    Console.WriteLine($"  ✓ Successfully parsed as double: {doubleResult}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Failed to parse as double");
                }

                // Try parsing as DateTime
                if (DateTime.TryParse(input, out DateTime dateResult))
                {
                    Console.WriteLine($"  ✓ Successfully parsed as DateTime: {dateResult:yyyy-MM-dd}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Failed to parse as DateTime");
                }
            }

            // Practical example: Validating user input
            Console.WriteLine("\nPractical example - Age validation:");
            string[] ageInputs = { "25", "0", "-5", "150", "twenty-five" };

            foreach (string ageInput in ageInputs)
            {
                if (int.TryParse(ageInput, out int age) && age >= 0 && age <= 120)
                {
                    Console.WriteLine($"  \"{ageInput}\" -> Valid age: {age}");
                }
                else
                {
                    Console.WriteLine($"  \"{ageInput}\" -> Invalid age input");
                }
            }

            Console.WriteLine();
        }

        static void DemonstrateCultureSensitiveParsing()
        {
            Console.WriteLine("3. CULTURE-SENSITIVE PARSING AND FORMATTING");
            Console.WriteLine("============================================");

            // Different cultures format numbers differently
            // This is crucial for international applications
            
            double number = 1234.56;
            DateTime date = new DateTime(2024, 5, 29, 14, 30, 0);

            // Get different culture formats
            CultureInfo usCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo germanCulture = CultureInfo.GetCultureInfo("de-DE");
            CultureInfo frenchCulture = CultureInfo.GetCultureInfo("fr-FR");
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            Console.WriteLine($"Number {number} formatted in different cultures:");
            Console.WriteLine($"  US (en-US): {number.ToString("N2", usCulture)}");
            Console.WriteLine($"  German (de-DE): {number.ToString("N2", germanCulture)}");
            Console.WriteLine($"  French (fr-FR): {number.ToString("N2", frenchCulture)}");
            Console.WriteLine($"  Invariant: {number.ToString("N2", invariantCulture)}");

            Console.WriteLine($"\nDate {date:yyyy-MM-dd HH:mm} formatted in different cultures:");
            Console.WriteLine($"  US: {date.ToString("d", usCulture)}");
            Console.WriteLine($"  German: {date.ToString("d", germanCulture)}");
            Console.WriteLine($"  French: {date.ToString("d", frenchCulture)}");

            // Parsing strings with different decimal separators
            Console.WriteLine("\nParsing culture-specific number formats:");
            
            string usNumber = "1,234.56";     // US format: comma for thousands, dot for decimal
            string germanNumber = "1.234,56"; // German format: dot for thousands, comma for decimal

            // Parse using specific cultures
            if (double.TryParse(usNumber, NumberStyles.Number, usCulture, out double usResult))
            {
                Console.WriteLine($"  US format \"{usNumber}\" -> {usResult}");
            }

            if (double.TryParse(germanNumber, NumberStyles.Number, germanCulture, out double germanResult))
            {
                Console.WriteLine($"  German format \"{germanNumber}\" -> {germanResult}");
            }

            // Using InvariantCulture for consistent behavior
            Console.WriteLine("\nUsing InvariantCulture for consistency:");
            string invariantNumber = "1234.56";
            double invariantResult = double.Parse(invariantNumber, CultureInfo.InvariantCulture);
            Console.WriteLine($"  \"{invariantNumber}\" (invariant) -> {invariantResult}");

            Console.WriteLine();
        }

        static void DemonstrateCustomFormatProviders()
        {
            Console.WriteLine("4. CUSTOM FORMAT PROVIDERS");
            Console.WriteLine("===========================");

            // Creating custom format providers to control exactly how data is displayed
            decimal amount = 1234.56m;

            // Custom NumberFormatInfo for currency formatting
            NumberFormatInfo customCurrency = new NumberFormatInfo();
            customCurrency.CurrencySymbol = "IDR ";
            customCurrency.CurrencyDecimalDigits = 0;
            customCurrency.CurrencyGroupSeparator = ".";
            customCurrency.CurrencyDecimalSeparator = ",";

            Console.WriteLine("Custom currency formatting:");
            Console.WriteLine($"  Default: {amount:C}");
            Console.WriteLine($"  Indonesian Rupiah: {amount.ToString("C", customCurrency)}");

            // Custom number grouping
            NumberFormatInfo customGrouping = new NumberFormatInfo();
            customGrouping.NumberGroupSeparator = " ";  // Space instead of comma
            customGrouping.NumberDecimalSeparator = ","; // Comma instead of dot

            int largeNumber = 1234567;
            Console.WriteLine($"\nCustom number grouping:");
            Console.WriteLine($"  Default: {largeNumber:N0}");
            Console.WriteLine($"  Space-separated: {largeNumber.ToString("N0", customGrouping)}");

            // Cloning existing format providers for modification
            Console.WriteLine("\nCloning and modifying existing format providers:");
            
            NumberFormatInfo clonedFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            clonedFormat.PercentSymbol = " percent";
            clonedFormat.PercentDecimalDigits = 1;

            double percentage = 0.1234;
            Console.WriteLine($"  Default percent: {percentage:P}");
            Console.WriteLine($"  Custom percent: {percentage.ToString("P", clonedFormat)}");

            Console.WriteLine();
        }

        static void DemonstrateNumberFormatting()
        {
            Console.WriteLine("5. NUMBER FORMATTING DEEP DIVE");
            Console.WriteLine("===============================");

            int integer = 42;
            double floating = 1234.5678;
            decimal money = 19.99m;

            // Standard numeric format strings
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

            // Custom numeric format strings
            Console.WriteLine("\nCustom format strings:");
            Console.WriteLine($"  Phone number format: {1234567890:###-###-####}");
            Console.WriteLine($"  Padded number: {42:00000}");
            Console.WriteLine($"  Conditional format: {-15:#;(#);zero}");  // positive;negative;zero

            // Working with very large and very small numbers
            Console.WriteLine("\nSpecial number scenarios:");
            double veryLarge = 1.23e15;
            double verySmall = 1.23e-8;
            
            Console.WriteLine($"  Very large: {veryLarge:E2}");
            Console.WriteLine($"  Very small: {verySmall:E2}");
            Console.WriteLine($"  Scientific: {veryLarge:0.##E+0}");

            Console.WriteLine();
        }

        static void DemonstrateDateTimeFormatting()
        {
            Console.WriteLine("6. DATETIME FORMATTING MASTERY");
            Console.WriteLine("===============================");

            DateTime now = DateTime.Now;
            DateTimeOffset nowOffset = DateTimeOffset.Now;

            // Standard DateTime format strings
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

            // Custom DateTime format strings
            Console.WriteLine("\nCustom format strings:");
            Console.WriteLine($"  Database format: {now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Readable format: {now:dddd, MMMM dd, yyyy}");
            Console.WriteLine($"  Time only: {now:HH:mm:ss}");
            Console.WriteLine($"  12-hour format: {now:h:mm:ss tt}");
            Console.WriteLine($"  ISO 8601: {now:yyyy-MM-ddTHH:mm:ssZ}");
            Console.WriteLine($"  File-safe: {now:yyyy-MM-dd_HH-mm-ss}");

            // Working with DateTimeOffset (timezone-aware)
            Console.WriteLine($"\nDateTimeOffset formatting:");
            Console.WriteLine($"  With offset: {nowOffset:yyyy-MM-dd HH:mm:ss zzz}");
            Console.WriteLine($"  UTC: {nowOffset.UtcDateTime:yyyy-MM-dd HH:mm:ss} UTC");

            // Custom DateTimeFormatInfo
            DateTimeFormatInfo customDateFormat = new DateTimeFormatInfo();
            customDateFormat.ShortDatePattern = "dd/MM/yyyy";
            customDateFormat.LongTimePattern = "HH:mm:ss";

            Console.WriteLine($"\nCustom date format: {now.ToString("G", customDateFormat)}");

            Console.WriteLine();
        }

        static void DemonstrateCompositeFormatting()
        {
            Console.WriteLine("7. COMPOSITE FORMATTING - STRING.FORMAT AND INTERPOLATION");
            Console.WriteLine("==========================================================");

            // Composite formatting with string.Format
            string customerName = "Alice Johnson";
            int orderNumber = 12345;
            decimal orderTotal = 156.78m;
            DateTime orderDate = DateTime.Now;

            // Traditional string.Format approach
            string formatted1 = string.Format(
                "Order #{0} for {1} totaling {2:C} was placed on {3:d}",
                orderNumber, customerName, orderTotal, orderDate);
            
            Console.WriteLine("string.Format example:");
            Console.WriteLine($"  {formatted1}");

            // Using format providers with string.Format
            CultureInfo britishCulture = CultureInfo.GetCultureInfo("en-GB");
            string formatted2 = string.Format(britishCulture,
                "Order #{0} for {1} totaling {2:C} was placed on {3:d}",
                orderNumber, customerName, orderTotal, orderDate);

            Console.WriteLine($"\nWith British culture:");
            Console.WriteLine($"  {formatted2}");

            // Modern string interpolation (equivalent but more readable)
            string interpolated = $"Order #{orderNumber} for {customerName} totaling {orderTotal:C} was placed on {orderDate:d}";
            Console.WriteLine($"\nString interpolation:");
            Console.WriteLine($"  {interpolated}");

            // Advanced formatting scenarios
            Console.WriteLine("\nAdvanced formatting scenarios:");
            
            // Alignment and padding
            string[] products = { "Laptop", "Mouse", "Keyboard" };
            decimal[] prices = { 999.99m, 29.99m, 149.99m };

            Console.WriteLine("Product listing with alignment:");
            Console.WriteLine($"{"Product",-12} {"Price",8}");
            Console.WriteLine(new string('-', 20));
            
            for (int i = 0; i < products.Length; i++)
            {
                Console.WriteLine($"{products[i],-12} {prices[i],8:C}");
            }

            // Conditional formatting
            Console.WriteLine("\nConditional formatting:");
            int[] quantities = { -5, 0, 10, 100 };
            
            foreach (int qty in quantities)
            {
                string status = string.Format("Quantity: {0:#;(#);'Out of Stock'}", qty);
                Console.WriteLine($"  {status}");
            }

            Console.WriteLine();
        }

        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("8. REAL-WORLD SCENARIOS");
            Console.WriteLine("========================");

            // Scenario 1: Processing CSV data
            Console.WriteLine("Scenario 1: Processing CSV sales data");
            string[] csvLines = {
                "2024-05-01,Alice,1234.56,USD",
                "2024-05-02,Bob,2345.67,EUR",
                "2024-05-03,Charlie,invalid_amount,USD"
            };

            foreach (string line in csvLines)
            {
                string[] parts = line.Split(',');
                Console.WriteLine($"\nProcessing: {line}");

                // Parse date
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

                // Parse amount
                if (decimal.TryParse(parts[2], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount))
                {
                    string currency = parts[3];
                    Console.WriteLine($"  Sale: {parts[1]} - {amount:N2} {currency}");
                }
                else
                {
                    Console.WriteLine($"  Invalid amount: {parts[2]}");
                }
            }

            // Scenario 2: Configuration file processing
            Console.WriteLine("\n\nScenario 2: Configuration file processing");
            var configEntries = new Dictionary<string, string>
            {
                {"timeout", "30"},
                {"retryCount", "5"},
                {"enableLogging", "true"},
                {"maxFileSize", "10.5"},
                {"serverUrl", "https://api.example.com"}
            };

            Console.WriteLine("Processing configuration entries:");
            foreach (var entry in configEntries)
            {
                Console.WriteLine($"\n  {entry.Key}: \"{entry.Value}\"");
                
                // Try different parsing approaches
                if (int.TryParse(entry.Value, out int intValue))
                {
                    Console.WriteLine($"    -> Integer: {intValue}");
                }
                else if (double.TryParse(entry.Value, out double doubleValue))
                {
                    Console.WriteLine($"    -> Double: {doubleValue:F2}");
                }
                else if (bool.TryParse(entry.Value, out bool boolValue))
                {
                    Console.WriteLine($"    -> Boolean: {boolValue}");
                }
                else
                {
                    Console.WriteLine($"    -> String: {entry.Value}");
                }
            }

            // Scenario 3: Internationalization example
            Console.WriteLine("\n\nScenario 3: Multi-language price formatting");
            decimal productPrice = 29.99m;
            
            var cultures = new[]
            {
                ("US", "en-US"),
                ("UK", "en-GB"),
                ("Germany", "de-DE"),
                ("Japan", "ja-JP"),
                ("Indonesia", "id-ID")
            };

            Console.WriteLine($"Product price {productPrice} in different regions:");
            foreach (var (country, cultureName) in cultures)
            {
                try
                {
                    CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
                    Console.WriteLine($"  {country}: {productPrice.ToString("C", culture)}");
                }
                catch (CultureNotFoundException)
                {
                    Console.WriteLine($"  {country}: Culture not available");
                }
            }

            // Scenario 4: Log file timestamp parsing
            Console.WriteLine("\n\nScenario 4: Log file timestamp parsing");
            string[] logEntries = {
                "2024-05-29 14:30:15.123 [INFO] Application started",
                "29/05/2024 14:30:16 [WARN] Configuration file missing",
                "May 29, 2024 2:30:17 PM [ERROR] Database connection failed",
                "invalid_timestamp [DEBUG] This should fail"
            };

            foreach (string logEntry in logEntries)
            {
                Console.WriteLine($"\nParsing: {logEntry}");
                
                // Try different timestamp formats
                string[] possibleFormats = {
                    "yyyy-MM-dd HH:mm:ss.fff",
                    "dd/MM/yyyy HH:mm:ss",
                    "MMMM dd, yyyy h:mm:ss tt"
                };

                bool parsed = false;
                foreach (string format in possibleFormats)
                {
                    string timestampPart = logEntry.Split(' ')[0] + " " + logEntry.Split(' ')[1];
                    if (logEntry.Split(' ').Length > 2 && logEntry.Split(' ')[2].Contains(":"))
                    {
                        timestampPart += " " + logEntry.Split(' ')[2];
                    }

                    if (DateTime.TryParseExact(timestampPart, format, CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, out DateTime timestamp))
                    {
                        Console.WriteLine($"  ✓ Parsed with format '{format}': {timestamp:yyyy-MM-dd HH:mm:ss}");
                        parsed = true;
                        break;
                    }
                }

                if (!parsed)
                {
                    Console.WriteLine($"  ✗ Could not parse timestamp");
                }
            }

            Console.WriteLine();
        }
    }
}
