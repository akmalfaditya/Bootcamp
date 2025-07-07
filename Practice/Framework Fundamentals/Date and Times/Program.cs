using System.Globalization;
using System.Xml;

// Date and Time Handling Demonstration
// This project covers all essential concepts for working with dates and times in C#
// These are immutable structs in the System namespace - once created, their values cannot change

namespace DateAndTimeHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== DATE AND TIME HANDLING DEMONSTRATION ===\n");

            // Walk through each concept systematically, following the training material structure
            DemonstrateTimeSpanBasics();
            DemonstrateTimeSpanOperations();
            DemonstrateTimeSpanConversion();
            DemonstrateDateTimeBasics();
            DemonstrateDateTimeVsDateTimeOffset();
            DemonstrateDateTimeConstructionAndConversion();
            DemonstrateDateTimeOperations();
            DemonstrateDateTimeFormatting();
            DemonstrateNullableDateTimes();
            DemonstrateDateOnlyAndTimeOnly();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== END OF DEMONSTRATION ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DemonstrateTimeSpanBasics()
        {
            Console.WriteLine("1. TIMESPAN BASICS - REPRESENTING TIME INTERVALS");
            Console.WriteLine("================================================");
            
            // TimeSpan represents a duration or interval of time
            // Resolution: 100 nanoseconds (ns) - very precise timing
            // Range: Approximately 10 million days, can be positive or negative
            
            Console.WriteLine("TimeSpan fundamentals:");
            Console.WriteLine($"  Resolution: 100 nanoseconds (1 tick)");
            Console.WriteLine($"  Range: ±{TimeSpan.MaxValue.TotalDays:N0} days approximately");
            Console.WriteLine($"  MinValue: {TimeSpan.MinValue}");
            Console.WriteLine($"  MaxValue: {TimeSpan.MaxValue}");

            // Constructor patterns - specify days, hours, minutes, seconds, milliseconds
            TimeSpan workingHours = new TimeSpan(8, 0, 0); // hours, minutes, seconds
            TimeSpan projectDuration = new TimeSpan(30, 8, 30, 0); // days, hours, minutes, seconds
            TimeSpan preciseInterval = new TimeSpan(5, 14, 30, 45, 500); // days, hours, minutes, seconds, milliseconds

            Console.WriteLine($"\nConstructor examples:");
            Console.WriteLine($"  Working day: {workingHours}");
            Console.WriteLine($"  Project duration: {projectDuration} ({projectDuration.TotalDays} days)");
            Console.WriteLine($"  Precise interval: {preciseInterval}");

            // Ticks constructor - each tick = 100 nanoseconds
            long ticksInSecond = TimeSpan.TicksPerSecond;
            TimeSpan oneSecond = new TimeSpan(ticksInSecond);
            Console.WriteLine($"  Ticks per second: {ticksInSecond:N0}");
            Console.WriteLine($"  One second from ticks: {oneSecond}");

            // Static From... methods - convenient for single units
            TimeSpan fromDays = TimeSpan.FromDays(2.5);
            TimeSpan fromHours = TimeSpan.FromHours(2.5);
            TimeSpan fromMinutes = TimeSpan.FromMinutes(90);
            TimeSpan fromSeconds = TimeSpan.FromSeconds(3600);
            TimeSpan fromMilliseconds = TimeSpan.FromMilliseconds(5000);
            TimeSpan fromMicroseconds = TimeSpan.FromMicroseconds(1000000);

            Console.WriteLine($"\nStatic factory methods:");
            Console.WriteLine($"  2.5 days: {fromDays}");
            Console.WriteLine($"  2.5 hours: {fromHours}");
            Console.WriteLine($"  90 minutes: {fromMinutes}");
            Console.WriteLine($"  3600 seconds: {fromSeconds}");
            Console.WriteLine($"  5000 milliseconds: {fromMilliseconds}");
            Console.WriteLine($"  1,000,000 microseconds: {fromMicroseconds}");

            // Negative TimeSpan - represents time going backwards
            TimeSpan negativeSpan = TimeSpan.FromHours(-2.5);
            Console.WriteLine($"  Negative span: {negativeSpan}");

            // Creating from DateTime subtraction - very common in practice
            DateTime start = new DateTime(2024, 1, 15, 9, 0, 0);
            DateTime end = new DateTime(2024, 1, 15, 17, 30, 0);
            TimeSpan workDuration = end - start;
            
            Console.WriteLine($"\nFrom DateTime subtraction:");
            Console.WriteLine($"  Start: {start:HH:mm}");
            Console.WriteLine($"  End: {end:HH:mm}");
            Console.WriteLine($"  Duration: {workDuration}");

            Console.WriteLine();
        }

        static void DemonstrateTimeSpanOperations()
        {
            Console.WriteLine("2. TIMESPAN OPERATIONS - MATH WITH TIME");
            Console.WriteLine("=======================================");

            // TimeSpan overloads operators <, >, +, and - for intuitive calculations
            TimeSpan duration1 = TimeSpan.FromHours(2);
            TimeSpan duration2 = TimeSpan.FromMinutes(30);
            TimeSpan totalDuration = duration1 + duration2;
            
            Console.WriteLine("Basic arithmetic:");
            Console.WriteLine($"  {duration1} + {duration2} = {totalDuration}");

            TimeSpan plannedTime = TimeSpan.FromHours(10);
            TimeSpan actualTime = TimeSpan.FromHours(8.5);
            TimeSpan timeDifference = plannedTime - actualTime;
            Console.WriteLine($"  {plannedTime} - {actualTime} = {timeDifference}");

            // Comparison operators
            Console.WriteLine($"  {duration1} > {duration2}? {duration1 > duration2}");
            Console.WriteLine($"  {actualTime} < {plannedTime}? {actualTime < plannedTime}");

            // Working with TimeSpan properties - Integer vs Total properties
            TimeSpan complexTime = TimeSpan.FromDays(10) - TimeSpan.FromSeconds(1); // Nearly 10 days
            
            Console.WriteLine($"\nProperty demonstration with {complexTime}:");
            Console.WriteLine("Integer properties (components only):");
            Console.WriteLine($"  Days: {complexTime.Days}");
            Console.WriteLine($"  Hours: {complexTime.Hours}");
            Console.WriteLine($"  Minutes: {complexTime.Minutes}");
            Console.WriteLine($"  Seconds: {complexTime.Seconds}");
            Console.WriteLine($"  Milliseconds: {complexTime.Milliseconds}");

            Console.WriteLine("Total properties (entire span as double):");
            Console.WriteLine($"  TotalDays: {complexTime.TotalDays}");
            Console.WriteLine($"  TotalHours: {complexTime.TotalHours:F2}");
            Console.WriteLine($"  TotalMinutes: {complexTime.TotalMinutes:F2}");
            Console.WriteLine($"  TotalSeconds: {complexTime.TotalSeconds:F2}");
            Console.WriteLine($"  TotalMilliseconds: {complexTime.TotalMilliseconds:F2}");

            // Practical example: Marathon time analysis
            TimeSpan marathonTime = new TimeSpan(3, 25, 30); // 3 hours, 25 minutes, 30 seconds
            Console.WriteLine($"\nMarathon analysis for time {marathonTime}:");
            Console.WriteLine($"  Finished in {marathonTime.Hours} hours and {marathonTime.Minutes} minutes");
            Console.WriteLine($"  Total time: {marathonTime.TotalMinutes:F1} minutes");
            Console.WriteLine($"  Average pace per mile: {marathonTime.TotalMinutes / 26.2:F2} minutes/mile");

            // TimeSpan as time of day
            DateTime currentTime = DateTime.Now;
            TimeSpan timeOfDay = currentTime.TimeOfDay;
            Console.WriteLine($"\nTime of day example:");
            Console.WriteLine($"  Current time: {currentTime}");
            Console.WriteLine($"  Time since midnight: {timeOfDay}");
            Console.WriteLine($"  Hours elapsed today: {timeOfDay.TotalHours:F2}");

            // Demonstrating the range and precision
            TimeSpan maxSpan = TimeSpan.MaxValue;
            TimeSpan minSpan = TimeSpan.MinValue;
            TimeSpan oneTick = new TimeSpan(1); // 1 tick = 100 nanoseconds

            Console.WriteLine($"\nTimeSpan limits and precision:");
            Console.WriteLine($"  Maximum span: {maxSpan.TotalDays:N0} days");
            Console.WriteLine($"  Minimum span: {minSpan.TotalDays:N0} days");
            Console.WriteLine($"  One tick duration: {oneTick.TotalMilliseconds * 1000000} nanoseconds");

            Console.WriteLine();
        }

        static void DemonstrateTimeSpanConversion()
        {
            Console.WriteLine("2.5 TIMESPAN CONVERSION - STRING AND XML HANDLING");
            Console.WriteLine("=================================================");

            // Converting TimeSpan to string representation
            TimeSpan duration = new TimeSpan(2, 30, 45); // 2 hours, 30 minutes, 45 seconds
            Console.WriteLine($"TimeSpan value: {duration}");
            Console.WriteLine($"ToString() result: '{duration.ToString()}'");

            // Parsing TimeSpan from strings - essential for configuration files
            string timeString1 = "02:30:45";        // Standard format
            string timeString2 = "1.12:30:45";      // Days.Hours:Minutes:Seconds
            string timeString3 = "1:23:45:30.500";  // Hours:Minutes:Seconds.Milliseconds

            if (TimeSpan.TryParse(timeString1, out TimeSpan parsed1))
                Console.WriteLine($"Parsed '{timeString1}' as: {parsed1}");

            if (TimeSpan.TryParse(timeString2, out TimeSpan parsed2))
                Console.WriteLine($"Parsed '{timeString2}' as: {parsed2} ({parsed2.TotalHours:F2} total hours)");

            if (TimeSpan.TryParse(timeString3, out TimeSpan parsed3))
                Console.WriteLine($"Parsed '{timeString3}' as: {parsed3}");

            // Using Parse method (throws exception on failure)
            try
            {
                TimeSpan exactParse = TimeSpan.Parse("00:45:30");
                Console.WriteLine($"Parse() result: {exactParse}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Parse error: {ex.Message}");
            }

            // XML conversion - important for web services and configuration
            TimeSpan xmlDuration = TimeSpan.FromHours(2.5);
            string xmlString = XmlConvert.ToString(xmlDuration);
            Console.WriteLine($"XML representation: '{xmlString}'");
            
            TimeSpan fromXml = XmlConvert.ToTimeSpan(xmlString);
            Console.WriteLine($"Converted back from XML: {fromXml}");

            // Default value demonstration
            TimeSpan defaultValue = default(TimeSpan);
            Console.WriteLine($"Default TimeSpan value: {defaultValue}");
            Console.WriteLine($"TimeSpan.Zero: {TimeSpan.Zero}");
            Console.WriteLine($"Are they equal? {defaultValue == TimeSpan.Zero}");

            // TimeOfDay example - treating TimeSpan as time since midnight
            DateTime now = DateTime.Now;
            TimeSpan timeOfDay = now.TimeOfDay;
            Console.WriteLine($"Current time of day: {timeOfDay}");
            Console.WriteLine($"Hours since midnight: {timeOfDay.TotalHours:F2}");

            Console.WriteLine();
        }

        static void DemonstrateDateTimeBasics()
        {
            Console.WriteLine("3. DATETIME BASICS - SPECIFIC POINTS IN TIME");
            Console.WriteLine("============================================");

            // Creating DateTime objects - different approaches for different needs
            DateTime newYear = new DateTime(2024, 1, 1); // Midnight on New Year
            DateTime meeting = new DateTime(2024, 5, 29, 14, 30, 0); // Today at 2:30 PM
            DateTime precise = new DateTime(2024, 5, 29, 14, 30, 15, 500); // Include milliseconds

            Console.WriteLine($"New Year: {newYear}");
            Console.WriteLine($"Meeting time: {meeting}");
            Console.WriteLine($"Precise timestamp: {precise}");

            // Getting current date and time - essential for logging and timestamps
            DateTime now = DateTime.Now; // Local time
            DateTime utcNow = DateTime.UtcNow; // UTC time - better for distributed systems
            DateTime today = DateTime.Today; // Today at midnight

            Console.WriteLine($"Current local time: {now}");
            Console.WriteLine($"Current UTC time: {utcNow}");
            Console.WriteLine($"Today (midnight): {today}");

            // Working with DateTime components
            Console.WriteLine($"Current year: {now.Year}");
            Console.WriteLine($"Current month: {now.Month} ({now:MMMM})");
            Console.WriteLine($"Day of week: {now.DayOfWeek}");
            Console.WriteLine($"Day of year: {now.DayOfYear}");

            // DateTime kind - important for timezone handling
            DateTime local = new DateTime(2024, 5, 29, 12, 0, 0, DateTimeKind.Local);
            DateTime utc = new DateTime(2024, 5, 29, 12, 0, 0, DateTimeKind.Utc);
            DateTime unspecified = new DateTime(2024, 5, 29, 12, 0, 0, DateTimeKind.Unspecified);

            Console.WriteLine($"Local time: {local} (Kind: {local.Kind})");
            Console.WriteLine($"UTC time: {utc} (Kind: {utc.Kind})");
            Console.WriteLine($"Unspecified: {unspecified} (Kind: {unspecified.Kind})");

            Console.WriteLine();
        }

        static void DemonstrateDateTimeVsDateTimeOffset()
        {
            Console.WriteLine("4. DATETIME VS DATETIMEOFFSET - TIMEZONE AWARENESS");
            Console.WriteLine("==================================================");
            
            // Key difference: how they handle equality and timezone information
            Console.WriteLine("DateTime - three-state DateTimeKind flag:");
            
            DateTime localTime = new DateTime(2024, 5, 29, 15, 30, 0, DateTimeKind.Local);
            DateTime utcTime = new DateTime(2024, 5, 29, 8, 30, 0, DateTimeKind.Utc);      // Same moment as local (assuming +7 offset)
            DateTime unspecified = new DateTime(2024, 5, 29, 15, 30, 0, DateTimeKind.Unspecified);

            Console.WriteLine($"  Local: {localTime} (Kind: {localTime.Kind})");
            Console.WriteLine($"  UTC: {utcTime} (Kind: {utcTime.Kind})");
            Console.WriteLine($"  Unspecified: {unspecified} (Kind: {unspecified.Kind})");

            // DateTime equality ignores DateTimeKind - can cause timezone bugs!
            Console.WriteLine($"\nDateTime equality problems:");
            Console.WriteLine($"  Local == Unspecified? {localTime == unspecified} (same components, different meaning!)");
            Console.WriteLine($"  UTC != Local? {utcTime != localTime} (same moment, different representation!)");

            Console.WriteLine("\nDateTimeOffset - explicit UTC offset storage:");
            
            // DateTimeOffset stores time + explicit offset from UTC
            DateTimeOffset jakartaTime = new DateTimeOffset(2024, 5, 29, 15, 30, 0, TimeSpan.FromHours(7));
            DateTimeOffset newYorkTime = new DateTimeOffset(2024, 5, 29, 4, 30, 0, TimeSpan.FromHours(-4));
            DateTimeOffset londonTime = new DateTimeOffset(2024, 5, 29, 9, 30, 0, TimeSpan.FromHours(1));
            DateTimeOffset utcTimeOffset = new DateTimeOffset(2024, 5, 29, 8, 30, 0, TimeSpan.Zero);

            Console.WriteLine($"  Jakarta (+7): {jakartaTime}");
            Console.WriteLine($"  New York (-4): {newYorkTime}");
            Console.WriteLine($"  London (+1): {londonTime}");
            Console.WriteLine($"  UTC (0): {utcTimeOffset}");

            // DateTimeOffset equality considers absolute time - much safer!
            Console.WriteLine($"\nDateTimeOffset equality benefits:");
            Console.WriteLine($"  Jakarta == New York? {jakartaTime == newYorkTime} (same absolute moment)");
            Console.WriteLine($"  Jakarta == London? {jakartaTime == londonTime} (same absolute moment)");
            Console.WriteLine($"  All represent UTC: {jakartaTime.UtcDateTime}");

            // Current time examples
            DateTime nowLocal = DateTime.Now;
            DateTime nowUtc = DateTime.UtcNow;
            DateTimeOffset nowWithOffset = DateTimeOffset.Now;
            DateTimeOffset nowUtcWithOffset = DateTimeOffset.UtcNow;
            
            Console.WriteLine($"\nCurrent time comparison:");
            Console.WriteLine($"  DateTime.Now: {nowLocal}");
            Console.WriteLine($"  DateTime.UtcNow: {nowUtc}");
            Console.WriteLine($"  DateTimeOffset.Now: {nowWithOffset}");
            Console.WriteLine($"  DateTimeOffset.UtcNow: {nowUtcWithOffset}");

            // Demonstrating the advantage of DateTimeOffset for global applications
            Console.WriteLine($"\nWhy DateTimeOffset is better for distributed systems:");
            
            // Scenario: User logs in from different timezones
            var loginEvents = new[]
            {
                new { User = "Alice", LoginTime = new DateTimeOffset(2024, 5, 29, 9, 0, 0, TimeSpan.FromHours(-8)) }, // Pacific
                new { User = "Bob", LoginTime = new DateTimeOffset(2024, 5, 29, 12, 0, 0, TimeSpan.FromHours(-5)) },   // Eastern  
                new { User = "Carol", LoginTime = new DateTimeOffset(2024, 5, 29, 18, 0, 0, TimeSpan.FromHours(1)) },  // Central Europe
            };

            Console.WriteLine("  Global login events (all at the same UTC moment):");
            foreach (var evt in loginEvents)
            {
                Console.WriteLine($"    {evt.User}: {evt.LoginTime} (UTC: {evt.LoginTime.UtcDateTime:HH:mm})");
            }

            // All login times are actually the same moment!
            bool allSameTime = loginEvents[0].LoginTime == loginEvents[1].LoginTime && 
                              loginEvents[1].LoginTime == loginEvents[2].LoginTime;
            Console.WriteLine($"    All logged in at same moment: {allSameTime}");

            // Recommendation summary
            Console.WriteLine($"\nRecommendations:");
            Console.WriteLine($"  • Use DateTimeOffset for: APIs, databases, distributed systems, logging");
            Console.WriteLine($"  • Use DateTime for: Local UI times, relative scheduling ('3 AM next Sunday')");
            Console.WriteLine($"  • DateTimeOffset prevents DST and timezone conversion bugs");
            Console.WriteLine($"  • DateTime is simpler for single-timezone applications");

            Console.WriteLine();
        }

        static void DemonstrateDateTimeConstructionAndConversion()
        {
            Console.WriteLine("4.5 DATETIME CONSTRUCTION & CONVERSION - DETAILED MECHANICS");
            Console.WriteLine("===========================================================");

            // DateTime construction with ticks (100-nanosecond intervals from 01/01/0001)
            long ticks = DateTime.Now.Ticks;
            DateTime fromTicks = new DateTime(ticks);
            Console.WriteLine($"Current ticks: {ticks:N0}");
            Console.WriteLine($"DateTime from ticks: {fromTicks}");

            // Static Parse methods - essential for user input and data import
            string dateString = "2024-05-29";
            string dateTimeString = "2024-05-29 14:30:00";
            string isoString = "2024-05-29T14:30:00.000Z";

            if (DateTime.TryParse(dateString, out DateTime parsedDate))
                Console.WriteLine($"TryParse '{dateString}': {parsedDate}");

            // ParseExact requires specific format - safer for known formats
            if (DateTime.TryParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out DateTime exactParsed))
                Console.WriteLine($"ParseExact result: {exactParsed}");

            // ISO 8601 format parsing
            if (DateTime.TryParse(isoString, out DateTime isoParsed))
                Console.WriteLine($"ISO parsed: {isoParsed} (Kind: {isoParsed.Kind})");

            // DateTimeOffset construction examples
            Console.WriteLine("\nDateTimeOffset construction:");
            
            // From components with offset
            DateTimeOffset specificOffset = new DateTimeOffset(2024, 5, 29, 14, 30, 0, TimeSpan.FromHours(7));
            Console.WriteLine($"Specific offset: {specificOffset}");

            // From DateTime with inferred offset
            DateTime localTime = new DateTime(2024, 5, 29, 14, 30, 0, DateTimeKind.Local);
            DateTime utcTime = new DateTime(2024, 5, 29, 14, 30, 0, DateTimeKind.Utc);
            DateTime unspecifiedTime = new DateTime(2024, 5, 29, 14, 30, 0, DateTimeKind.Unspecified);

            DateTimeOffset fromLocal = new DateTimeOffset(localTime);
            DateTimeOffset fromUtc = new DateTimeOffset(utcTime);
            DateTimeOffset fromUnspecified = new DateTimeOffset(unspecifiedTime);

            Console.WriteLine($"From Local DateTime: {fromLocal}");
            Console.WriteLine($"From UTC DateTime: {fromUtc}");
            Console.WriteLine($"From Unspecified: {fromUnspecified}");

            // Converting between DateTimeOffset and DateTime
            Console.WriteLine("\nConversion between DateTimeOffset and DateTime:");
            
            DateTimeOffset sampleOffset = DateTimeOffset.Now;
            DateTime utcFromOffset = sampleOffset.UtcDateTime;
            DateTime localFromOffset = sampleOffset.LocalDateTime;
            DateTime unspecifiedFromOffset = sampleOffset.DateTime;

            Console.WriteLine($"Original DateTimeOffset: {sampleOffset}");
            Console.WriteLine($"UtcDateTime (Kind: {utcFromOffset.Kind}): {utcFromOffset}");
            Console.WriteLine($"LocalDateTime (Kind: {localFromOffset.Kind}): {localFromOffset}");
            Console.WriteLine($"DateTime (Kind: {unspecifiedFromOffset.Kind}): {unspecifiedFromOffset}");

            // Demonstrating DateTimeKind importance
            Console.WriteLine("\nDateTimeKind comparison behavior:");
            
            DateTime kind1 = new DateTime(2024, 5, 29, 12, 0, 0, DateTimeKind.Local);
            DateTime kind2 = new DateTime(2024, 5, 29, 12, 0, 0, DateTimeKind.Utc);
            DateTime kind3 = new DateTime(2024, 5, 29, 12, 0, 0, DateTimeKind.Unspecified);

            Console.WriteLine($"Local time: {kind1} (Kind: {kind1.Kind})");
            Console.WriteLine($"UTC time: {kind2} (Kind: {kind2.Kind})");
            Console.WriteLine($"Unspecified: {kind3} (Kind: {kind3.Kind})");
            
            // DateTime equality ignores Kind - this can cause timezone issues!
            Console.WriteLine($"Local == UTC? {kind1 == kind2} (both represent different actual moments!)");
            Console.WriteLine($"Local == Unspecified? {kind1 == kind3}");

            // DateTimeOffset equality comparison
            DateTimeOffset offset1 = new DateTimeOffset(2024, 5, 29, 15, 0, 0, TimeSpan.FromHours(7));  // Jakarta
            DateTimeOffset offset2 = new DateTimeOffset(2024, 5, 29, 8, 0, 0, TimeSpan.FromHours(0));   // UTC
            
            Console.WriteLine($"\nDateTimeOffset comparison:");
            Console.WriteLine($"Jakarta time: {offset1}");
            Console.WriteLine($"UTC time: {offset2}");
            Console.WriteLine($"Are they equal? {offset1 == offset2} (same absolute moment)");

            Console.WriteLine();
        }

        static void DemonstrateDateTimeOperations()
        {
            Console.WriteLine("5. DATETIME OPERATIONS - MANIPULATING DATES");
            Console.WriteLine("============================================");

            DateTime startDate = new DateTime(2024, 1, 15, 9, 0, 0);
            Console.WriteLine($"Starting date: {startDate}");

            // Adding time intervals - very common in business logic
            DateTime deadline = startDate.AddDays(30);
            DateTime reminder = deadline.AddDays(-7);
            DateTime urgentReminder = deadline.AddHours(-24);

            Console.WriteLine($"Project deadline: {deadline}");
            Console.WriteLine($"First reminder: {reminder}");
            Console.WriteLine($"Urgent reminder: {urgentReminder}");

            // Adding different time units
            DateTime scheduleExample = startDate
                .AddYears(1)
                .AddMonths(2)
                .AddDays(15)
                .AddHours(3)
                .AddMinutes(30);

            Console.WriteLine($"Complex schedule: {scheduleExample}");

            // Calculating business days (excluding weekends)
            DateTime businessStart = new DateTime(2024, 5, 27); // Monday
            int businessDaysToAdd = 10;
            DateTime businessEnd = AddBusinessDays(businessStart, businessDaysToAdd);

            Console.WriteLine($"Business days calculation:");
            Console.WriteLine($"  Start: {businessStart:yyyy-MM-dd} ({businessStart.DayOfWeek})");
            Console.WriteLine($"  Add {businessDaysToAdd} business days");
            Console.WriteLine($"  End: {businessEnd:yyyy-MM-dd} ({businessEnd.DayOfWeek})");

            // Finding the next occurrence of a specific day
            DateTime nextFriday = GetNextWeekday(DateTime.Today, DayOfWeek.Friday);
            Console.WriteLine($"Next Friday: {nextFriday:yyyy-MM-dd}");

            Console.WriteLine();
        }

        static void DemonstrateDateTimeFormatting()
        {
            Console.WriteLine("6. DATETIME FORMATTING - DISPLAYING AND PARSING DATES");
            Console.WriteLine("======================================================");

            DateTime sampleDate = new DateTime(2024, 5, 29, 14, 30, 45, 123);

            // Standard format strings - influenced by OS regional settings
            Console.WriteLine("Standard formats (culture-dependent):");
            Console.WriteLine($"  Short date (d): {sampleDate:d}");
            Console.WriteLine($"  Long date (D): {sampleDate:D}");
            Console.WriteLine($"  Short time (t): {sampleDate:t}");
            Console.WriteLine($"  Long time (T): {sampleDate:T}");
            Console.WriteLine($"  Full date/time (F): {sampleDate:F}");
            Console.WriteLine($"  General (G): {sampleDate:G}");
            Console.WriteLine($"  Round-trip (O): {sampleDate:O}"); // Critical for reliable parsing!

            // Round-trip format is crucial for data storage and transmission
            Console.WriteLine("\nRound-trip format importance:");
            string roundTripString = sampleDate.ToString("O");
            DateTime parsedBack = DateTime.Parse(roundTripString);
            Console.WriteLine($"  Original: {sampleDate}");
            Console.WriteLine($"  Round-trip string: '{roundTripString}'");
            Console.WriteLine($"  Parsed back: {parsedBack}");
            Console.WriteLine($"  Values equal: {sampleDate == parsedBack}");

            // Custom format strings - essential for APIs and databases
            Console.WriteLine("\nCustom formats for different purposes:");
            Console.WriteLine($"  Database: {sampleDate:yyyy-MM-dd HH:mm:ss.fff}");
            Console.WriteLine($"  ISO 8601: {sampleDate:yyyy-MM-ddTHH:mm:ss.fffZ}");
            Console.WriteLine($"  User display: {sampleDate:dddd, MMMM dd, yyyy 'at' h:mm tt}");
            Console.WriteLine($"  File naming: {sampleDate:yyyyMMdd_HHmmss}");
            Console.WriteLine($"  Log format: {sampleDate:[yyyy-MM-dd HH:mm:ss.fff]}");

            // Culture-specific formatting - critical for international applications
            CultureInfo usCulture = new CultureInfo("en-US");
            CultureInfo ukCulture = new CultureInfo("en-GB");
            CultureInfo germanCulture = new CultureInfo("de-DE");
            CultureInfo indonesianCulture = new CultureInfo("id-ID");

            Console.WriteLine("\nCultural formatting differences:");
            Console.WriteLine($"  US (en-US): {sampleDate.ToString("F", usCulture)}");
            Console.WriteLine($"  UK (en-GB): {sampleDate.ToString("F", ukCulture)}");
            Console.WriteLine($"  German (de-DE): {sampleDate.ToString("F", germanCulture)}");
            Console.WriteLine($"  Indonesian (id-ID): {sampleDate.ToString("F", indonesianCulture)}");

            // Parsing with different approaches
            Console.WriteLine("\nParsing strategies:");

            // Parsing dates from strings - essential for user input
            string dateString = "2024-05-29";
            string timeString = "14:30:45";
            string fullString = "May 29, 2024 2:30 PM";

            if (DateTime.TryParse(dateString, out DateTime parsedDate))
                Console.WriteLine($"  TryParse '{dateString}': {parsedDate}");

            if (DateTime.TryParseExact(timeString, "HH:mm:ss", null, DateTimeStyles.None, out DateTime parsedTime))
                Console.WriteLine($"  TryParseExact '{timeString}': {parsedTime:T}");

            if (DateTime.TryParse(fullString, out DateTime parsedFull))
                Console.WriteLine($"  TryParse '{fullString}': {parsedFull}");

            // Demonstrating parsing pitfalls without format specification
            Console.WriteLine("\nParsing pitfall demonstration:");
            string ambiguousDate = "01/02/2024"; // Is this Jan 2 or Feb 1?
            
            DateTime usInterpretation = DateTime.Parse(ambiguousDate, usCulture);
            DateTime ukInterpretation = DateTime.Parse(ambiguousDate, ukCulture);
            
            Console.WriteLine($"  '{ambiguousDate}' in US culture: {usInterpretation:yyyy-MM-dd} (MM/dd/yyyy)");
            Console.WriteLine($"  '{ambiguousDate}' in UK culture: {ukInterpretation:yyyy-MM-dd} (dd/MM/yyyy)");
            Console.WriteLine("  Solution: Use unambiguous formats like 'yyyy-MM-dd' or round-trip 'O'");

            // Safe parsing with specific format
            string safeFormat = "yyyy-MM-dd HH:mm:ss";
            string safeString = sampleDate.ToString(safeFormat);
            Console.WriteLine($"\n  Safe format '{safeFormat}': '{safeString}'");
            
            if (DateTime.TryParseExact(safeString, safeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime safeParsed))
                Console.WriteLine($"  Safely parsed: {safeParsed}");

            Console.WriteLine();
        }

        static void DemonstrateDateOnlyAndTimeOnly()
        {
            Console.WriteLine("8. DATEONLY AND TIMEONLY - SPECIALIZED TYPES (.NET 6+)");
            Console.WriteLine("=======================================================");
            
            // Introduced in .NET 6 to provide more specific and type-safe representations
            // DateOnly: just a date (year, month, day) without time component
            // TimeOnly: just a time of day without date component
            // Both lack DateTimeKind and have no concept of Local or UTC
            
            Console.WriteLine("DateOnly benefits - avoids DateTime pitfalls:");
            
            // DateOnly prevents bugs where DateTime intended as "just a date" 
            // accidentally gets non-zero time, causing equality comparisons to fail
            DateOnly birthday = new DateOnly(1990, 6, 15);
            DateOnly holiday = new DateOnly(2024, 12, 25);
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            Console.WriteLine($"  Birthday: {birthday}");
            Console.WriteLine($"  Christmas: {holiday}");
            Console.WriteLine($"  Today: {today}");

            // Safe date comparisons - no time component to worry about
            DateOnly date1 = new DateOnly(2024, 5, 29);
            DateOnly date2 = new DateOnly(2024, 5, 29);
            Console.WriteLine($"  Date equality works perfectly: {date1} == {date2} is {date1 == date2}");

            // Calculate age using DateOnly - more appropriate than DateTime
            int age = today.Year - birthday.Year;
            if (today < birthday.AddYears(age))
                age--;
            Console.WriteLine($"  Current age: {age} years");

            // Working with date arithmetic
            DateOnly nextBirthday = birthday.AddYears(today.Year - birthday.Year);
            if (nextBirthday < today)
                nextBirthday = nextBirthday.AddYears(1);
            
            int daysUntilBirthday = nextBirthday.DayNumber - today.DayNumber;
            Console.WriteLine($"  Days until next birthday: {daysUntilBirthday}");

            Console.WriteLine("\nTimeOnly benefits - ideal for schedules and recurring times:");
            
            // TimeOnly - perfect for store hours, alarms, recurring daily schedules
            TimeOnly storeOpen = new TimeOnly(9, 0);
            TimeOnly lunchStart = new TimeOnly(12, 0);
            TimeOnly lunchEnd = new TimeOnly(13, 0);
            TimeOnly storeClose = new TimeOnly(17, 30);
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);

            Console.WriteLine($"  Store opens: {storeOpen}");
            Console.WriteLine($"  Lunch break: {lunchStart} - {lunchEnd}");
            Console.WriteLine($"  Store closes: {storeClose}");
            Console.WriteLine($"  Current time: {currentTime}");

            // Business logic with TimeOnly
            bool isOpen = currentTime >= storeOpen && currentTime <= storeClose && 
                         !(currentTime >= lunchStart && currentTime < lunchEnd);
            Console.WriteLine($"  Store is currently: {(isOpen ? "OPEN" : "CLOSED")}");

            // TimeOnly arithmetic
            TimeOnly meetingStart = new TimeOnly(14, 0);
            TimeSpan meetingDuration = TimeSpan.FromMinutes(90);
            TimeOnly meetingEnd = meetingStart.Add(meetingDuration);

            Console.WriteLine($"\n  Meeting schedule:");
            Console.WriteLine($"    Start: {meetingStart}");
            Console.WriteLine($"    Duration: {meetingDuration}");
            Console.WriteLine($"    End: {meetingEnd}");

            // Combining DateOnly and TimeOnly when needed
            DateOnly eventDate = new DateOnly(2024, 6, 15);
            TimeOnly eventTime = new TimeOnly(14, 30);
            DateTime fullEventDateTime = eventDate.ToDateTime(eventTime);
            
            Console.WriteLine($"\n  Combining DateOnly and TimeOnly:");
            Console.WriteLine($"    Event date: {eventDate}");
            Console.WriteLine($"    Event time: {eventTime}");
            Console.WriteLine($"    Full DateTime: {fullEventDateTime}");

            // Practical example: Recurring weekly schedule
            Console.WriteLine("\n  Weekly class schedule (TimeOnly for daily patterns):");
            var classSchedule = new[]
            {
                new { Day = "Monday", Time = new TimeOnly(9, 0), Subject = "Mathematics" },
                new { Day = "Wednesday", Time = new TimeOnly(11, 0), Subject = "Physics" },
                new { Day = "Friday", Time = new TimeOnly(14, 0), Subject = "Chemistry" }
            };

            foreach (var cls in classSchedule)
            {
                Console.WriteLine($"    {cls.Day} at {cls.Time}: {cls.Subject}");
            }

            Console.WriteLine();
        }

        static void DemonstrateNullableDateTimes()
        {
            Console.WriteLine("7.5 NULLABLE DATETIME VALUES - HANDLING MISSING DATES");
            Console.WriteLine("======================================================");

            // DateTime and DateTimeOffset are structs (value types), so not intrinsically nullable
            // Two approaches to represent "null" or missing dates

            Console.WriteLine("Approach 1: Nullable<DateTime> (Recommended)");
            
            // Using nullable types - the safe and recommended approach
            DateTime? nullableDate = null;
            DateTime? validDate = new DateTime(2024, 5, 29);
            DateTimeOffset? nullableOffset = null;
            DateTimeOffset? validOffset = DateTimeOffset.Now;

            Console.WriteLine($"Nullable DateTime (null): {nullableDate?.ToString() ?? "NULL"}");
            Console.WriteLine($"Nullable DateTime (valid): {validDate}");
            Console.WriteLine($"Nullable DateTimeOffset (null): {nullableOffset?.ToString() ?? "NULL"}");
            Console.WriteLine($"Nullable DateTimeOffset (valid): {validOffset}");

            // Checking for null values
            if (nullableDate.HasValue)
                Console.WriteLine($"Date has value: {nullableDate.Value}");
            else
                Console.WriteLine("Date is null - no value assigned");

            // Getting value with fallback
            DateTime dateWithFallback = validDate ?? DateTime.Today;
            Console.WriteLine($"Date with fallback: {dateWithFallback}");

            // Practical example: Optional expiration date
            DateTime? subscriptionExpiry = null; // Lifetime subscription
            DateTime? trialExpiry = DateTime.Today.AddDays(30);

            Console.WriteLine($"\nSubscription scenarios:");
            Console.WriteLine($"Lifetime subscription expiry: {subscriptionExpiry?.ToString("yyyy-MM-dd") ?? "Never"}");
            Console.WriteLine($"Trial expiry: {trialExpiry?.ToString("yyyy-MM-dd") ?? "Never"}");

            // Check if subscription is expired
            bool isLifetimeExpired = subscriptionExpiry.HasValue && subscriptionExpiry.Value < DateTime.Today;
            bool isTrialExpired = trialExpiry.HasValue && trialExpiry.Value < DateTime.Today;
            
            Console.WriteLine($"Lifetime expired: {isLifetimeExpired}");
            Console.WriteLine($"Trial expired: {isTrialExpired}");

            Console.WriteLine("\nApproach 2: Using MinValue (Use with caution)");
            
            // Using default/MinValue - can cause issues with timezone conversions
            DateTime defaultDateTime = default(DateTime);
            DateTime minDateTime = DateTime.MinValue;
            DateTimeOffset defaultOffset = default(DateTimeOffset);
            DateTimeOffset minOffset = DateTimeOffset.MinValue;

            Console.WriteLine($"Default DateTime: {defaultDateTime}");
            Console.WriteLine($"DateTime.MinValue: {minDateTime}");
            Console.WriteLine($"Default DateTimeOffset: {defaultOffset}");
            Console.WriteLine($"DateTimeOffset.MinValue: {minOffset}");

            // Demonstrating MinValue conversion issue
            Console.WriteLine("\nMinValue conversion warning:");
            try
            {
                DateTime minValueLocal = DateTime.MinValue;
                Console.WriteLine($"MinValue before conversion: {minValueLocal}");
                
                // This might change the value when converting timezones!
                DateTime converted = minValueLocal.ToUniversalTime();
                Console.WriteLine($"After ToUniversalTime(): {converted}");
                Console.WriteLine($"Values equal? {minValueLocal == converted}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Conversion error: {ex.Message}");
            }

            // Safe pattern for checking "null" dates
            Console.WriteLine("\nSafe null checking patterns:");
            
            DateTime suspiciousDate = DateTime.MinValue;
            DateTime? safeSuspiciousDate = suspiciousDate == DateTime.MinValue ? null : suspiciousDate;
            
            Console.WriteLine($"Original date: {suspiciousDate}");
            Console.WriteLine($"Safe nullable: {safeSuspiciousDate?.ToString() ?? "NULL"}");

            Console.WriteLine();
        }

        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("8. REAL-WORLD SCENARIOS - PRACTICAL APPLICATIONS");
            Console.WriteLine("================================================");

            // Scenario 1: Event scheduling system
            Console.WriteLine("Scenario 1: Global conference scheduling");
            
            DateTimeOffset conferenceStart = new DateTimeOffset(2024, 9, 15, 9, 0, 0, TimeSpan.FromHours(-7)); // Pacific Time
            TimeSpan sessionDuration = TimeSpan.FromMinutes(45);
            TimeSpan breakDuration = TimeSpan.FromMinutes(15);

            Console.WriteLine($"Conference starts: {conferenceStart}");
            
            // Schedule multiple sessions
            for (int i = 1; i <= 4; i++)
            {
                DateTimeOffset sessionStart = conferenceStart.Add(TimeSpan.FromMinutes((i - 1) * 60));
                DateTimeOffset sessionEnd = sessionStart.Add(sessionDuration);
                
                Console.WriteLine($"  Session {i}: {sessionStart:HH:mm} - {sessionEnd:HH:mm} (Pacific Time)");
            }

            // Scenario 2: SLA monitoring
            Console.WriteLine("\nScenario 2: Service Level Agreement monitoring");
            
            DateTime serviceStart = DateTime.UtcNow;
            TimeSpan slaTarget = TimeSpan.FromHours(4); // 4-hour SLA
            DateTime slaDeadline = serviceStart.Add(slaTarget);

            Console.WriteLine($"Service request: {serviceStart:yyyy-MM-dd HH:mm:ss} UTC");
            Console.WriteLine($"SLA deadline: {slaDeadline:yyyy-MM-dd HH:mm:ss} UTC");
            
            // Simulate service completion
            DateTime serviceComplete = serviceStart.AddHours(3.5);
            TimeSpan actualDuration = serviceComplete - serviceStart;
            bool slaMetric = actualDuration <= slaTarget;

            Console.WriteLine($"Service completed: {serviceComplete:yyyy-MM-dd HH:mm:ss} UTC");
            Console.WriteLine($"Duration: {actualDuration}");
            Console.WriteLine($"SLA met: {(slaMetric ? "YES" : "NO")}");

            // Scenario 3: Recurring appointment system
            Console.WriteLine("\nScenario 3: Weekly recurring meetings");
            
            DateOnly startDate = new DateOnly(2024, 6, 3); // First Monday
            TimeOnly meetingTime = new TimeOnly(10, 0);
            
            Console.WriteLine("Next 5 weekly meetings:");
            for (int week = 0; week < 5; week++)
            {
                DateOnly meetingDate = startDate.AddDays(week * 7);
                DateTime fullDateTime = meetingDate.ToDateTime(meetingTime);
                Console.WriteLine($"  Week {week + 1}: {fullDateTime:dddd, MMMM dd, yyyy} at {meetingTime}");
            }

            // Scenario 4: Age calculation and birthday tracking
            Console.WriteLine("\nScenario 4: Employee birthday system");
            
            var employees = new[]
            {
                new { Name = "Alice", Birthday = new DateOnly(1985, 3, 15) },
                new { Name = "Bob", Birthday = new DateOnly(1990, 8, 22) },
                new { Name = "Carol", Birthday = new DateOnly(1988, 11, 7) }
            };

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);
            
            foreach (var employee in employees)
            {
                int employeeAge = CalculateAge(employee.Birthday, currentDate);
                DateOnly nextBirthday = GetNextBirthday(employee.Birthday, currentDate);
                int daysUntilBirthday = nextBirthday.DayNumber - currentDate.DayNumber;
                
                Console.WriteLine($"  {employee.Name}: {employeeAge} years old, birthday in {daysUntilBirthday} days");
            }

            Console.WriteLine();
        }

        // Helper methods for real-world scenarios
        static DateTime AddBusinessDays(DateTime startDate, int businessDays)
        {
            DateTime result = startDate;
            int daysAdded = 0;

            while (daysAdded < businessDays)
            {
                result = result.AddDays(1);
                if (result.DayOfWeek != DayOfWeek.Saturday && result.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysAdded++;
                }
            }

            return result;
        }

        static DateTime GetNextWeekday(DateTime startDate, DayOfWeek targetDay)
        {
            int daysUntilTarget = ((int)targetDay - (int)startDate.DayOfWeek + 7) % 7;
            if (daysUntilTarget == 0) daysUntilTarget = 7; // If it's the same day, get next week
            return startDate.AddDays(daysUntilTarget);
        }

        static int CalculateAge(DateOnly birthDate, DateOnly currentDate)
        {
            int age = currentDate.Year - birthDate.Year;
            if (currentDate < birthDate.AddYears(age))
                age--;
            return age;
        }

        static DateOnly GetNextBirthday(DateOnly birthDate, DateOnly currentDate)
        {
            DateOnly nextBirthday = new DateOnly(currentDate.Year, birthDate.Month, birthDate.Day);
            if (nextBirthday <= currentDate)
                nextBirthday = nextBirthday.AddYears(1);
            return nextBirthday;
        }
    }
}
