# Date and Times

## Learning Objectives

By learningthis project, you will:
- **Master Time Intervals**: Use TimeSpan for durations, delays, and time calculations
- **Handle DateTime Types**: Work with DateTime, DateTimeOffset, and timezone conversions
- **Format Date/Time**: Display dates and times in various formats for different cultures
- **Parse Date Strings**: Convert user input and external data to DateTime objects safely
- **Use DateOnly/TimeOnly**: Leverage .NET 6+ types for date-only and time-only scenarios
- **Manage Timezones**: Handle global applications with proper timezone awareness
- **Build Real-World Features**: Create schedulers, timers, logging systems, and business logic

## Core Concepts

### TimeSpan - Representing Time Intervals
TimeSpan represents a duration of time - like a stopwatch measurement. Use it for elapsed time, delays, timeouts, and intervals.

```csharp
// Different ways to create TimeSpan
TimeSpan workingDay = new TimeSpan(8, 0, 0);        // 8 hours
TimeSpan meeting = TimeSpan.FromMinutes(45);        // 45 minutes
TimeSpan timeout = TimeSpan.FromSeconds(30);        // 30 seconds
TimeSpan project = TimeSpan.FromDays(14);           // 2 weeks

// Properties for accessing different units
Console.WriteLine($"Total hours: {workingDay.TotalHours}");      // 8.0
Console.WriteLine($"Total minutes: {meeting.TotalMinutes}");     // 45.0
Console.WriteLine($"Days: {project.Days}");                     // 14
```

### DateTime vs DateTimeOffset
Understanding the difference is crucial for global applications:

```csharp
// DateTime - local or unspecified timezone context
DateTime localTime = DateTime.Now;           // Local machine time
DateTime utcTime = DateTime.UtcNow;         // UTC time
DateTime unspecified = new DateTime(2024, 5, 29, 14, 30, 0); // No timezone info

// DateTimeOffset - timezone-aware, always includes offset information
DateTimeOffset nowWithOffset = DateTimeOffset.Now;        // Local time with offset
DateTimeOffset utcWithOffset = DateTimeOffset.UtcNow;     // UTC with +00:00 offset
DateTimeOffset specificTime = new DateTimeOffset(2024, 5, 29, 14, 30, 0, TimeSpan.FromHours(-5)); // EST
```

### DateOnly and TimeOnly (.NET 6+)
For scenarios where you only need date or time components:

```csharp
// DateOnly - perfect for birthdays, deadlines, calendar events
DateOnly birthday = new DateOnly(1990, 6, 15);
DateOnly today = DateOnly.FromDateTime(DateTime.Now);
DateOnly deadline = today.AddDays(30);

// TimeOnly - perfect for business hours, appointments, schedules
TimeOnly opening = new TimeOnly(9, 0);        // 9:00 AM
TimeOnly closing = new TimeOnly(17, 30);      // 5:30 PM
TimeOnly appointment = TimeOnly.FromTimeSpan(TimeSpan.FromHours(14.5)); // 2:30 PM
```

## Key Features

### 1. **TimeSpan Operations and Calculations**
```csharp
public static class TimeCalculator
{
    public static TimeSpan CalculateWorkingHours(TimeSpan start, TimeSpan end, TimeSpan lunchBreak)
    {
        TimeSpan totalTime = end - start;
        return totalTime - lunchBreak;
    }
    
    public static TimeSpan CalculateProjectDuration(DateTime startDate, DateTime endDate)
    {
        return endDate - startDate;
    }
    
    public static bool IsWithinBusinessHours(TimeOnly currentTime, TimeOnly start, TimeOnly end)
    {
        return currentTime >= start && currentTime <= end;
    }
    
    // Calculate elapsed time with high precision
    public static TimeSpan MeasureExecutionTime(Action operation)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        operation();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}

// Usage examples
TimeSpan workingHours = TimeCalculator.CalculateWorkingHours(
    TimeSpan.FromHours(9),      // 9:00 AM
    TimeSpan.FromHours(17),     // 5:00 PM  
    TimeSpan.FromMinutes(60)    // 1 hour lunch
); // Result: 7 hours
```

### 2. **DateTime Formatting and Parsing**
```csharp
public static class DateTimeHelper
{
    // Standard format strings
    public static string FormatStandard(DateTime dateTime)
    {
        return $"""
            Short Date: {dateTime:d}           // 5/29/2024
            Long Date: {dateTime:D}            // Wednesday, May 29, 2024
            Short Time: {dateTime:t}           // 2:30 PM
            Long Time: {dateTime:T}            // 2:30:45 PM
            ISO 8601: {dateTime:O}             // 2024-05-29T14:30:45.0000000
            RFC 1123: {dateTime:R}             // Wed, 29 May 2024 14:30:45 GMT
            """;
    }
    
    // Custom format strings
    public static string FormatCustom(DateTime dateTime)
    {
        return $"""
            Custom: {dateTime:yyyy-MM-dd HH:mm:ss}        // 2024-05-29 14:30:45
            US Format: {dateTime:MM/dd/yyyy h:mm tt}      // 05/29/2024 2:30 PM
            European: {dateTime:dd.MM.yyyy HH:mm}         // 29.05.2024 14:30
            File Safe: {dateTime:yyyy-MM-dd_HH-mm-ss}     // 2024-05-29_14-30-45
            """;
    }
    
    // Safe parsing with multiple formats
    public static DateTime? TryParseMultipleFormats(string input)
    {
        string[] formats = {
            "yyyy-MM-dd",
            "MM/dd/yyyy", 
            "dd/MM/yyyy",
            "yyyy-MM-dd HH:mm:ss",
            "MM/dd/yyyy h:mm tt",
            "O" // ISO 8601
        };
        
        foreach (string format in formats)
        {
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }
        
        // Try standard parsing as fallback
        return DateTime.TryParse(input, out DateTime fallback) ? fallback : null;
    }
}
```

### 3. **Timezone Management**
```csharp
public static class TimezoneManager
{
    public static DateTimeOffset ConvertToTimezone(DateTimeOffset source, string timezoneId)
    {
        TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        return TimeZoneInfo.ConvertTime(source, targetZone);
    }
    
    public static string FormatForMultipleTimezones(DateTimeOffset dateTime)
    {
        var timezones = new[]
        {
            ("UTC", "UTC"),
            ("Eastern", "Eastern Standard Time"),
            ("Pacific", "Pacific Standard Time"),
            ("Central Europe", "Central European Standard Time"),
            ("Tokyo", "Tokyo Standard Time")
        };
        
        var results = new List<string>();
        foreach (var (name, id) in timezones)
        {
            try
            {
                var converted = ConvertToTimezone(dateTime, id);
                results.Add($"{name}: {converted:yyyy-MM-dd HH:mm zzz}");
            }
            catch (TimeZoneNotFoundException)
            {
                results.Add($"{name}: Timezone not found");
            }
        }
        
        return string.Join("\n", results);
    }
    
    public static bool IsInDaylightSavingTime(DateTime dateTime, string timezoneId)
    {
        TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        return timezone.IsDaylightSavingTime(dateTime);
    }
}
```

### 4. **Business Calendar Operations**
```csharp
public static class BusinessCalendar
{
    private static readonly HashSet<DayOfWeek> weekends = new() 
    { 
        DayOfWeek.Saturday, 
        DayOfWeek.Sunday 
    };
    
    public static bool IsBusinessDay(DateOnly date)
    {
        return !weekends.Contains(date.DayOfWeek);
    }
    
    public static DateOnly AddBusinessDays(DateOnly startDate, int businessDays)
    {
        DateOnly current = startDate;
        int daysAdded = 0;
        
        while (daysAdded < businessDays)
        {
            current = current.AddDays(1);
            if (IsBusinessDay(current))
            {
                daysAdded++;
            }
        }
        
        return current;
    }
    
    public static int CountBusinessDays(DateOnly start, DateOnly end)
    {
        int count = 0;
        DateOnly current = start;
        
        while (current <= end)
        {
            if (IsBusinessDay(current))
            {
                count++;
            }
            current = current.AddDays(1);
        }
        
        return count;
    }
    
    public static IEnumerable<DateOnly> GetBusinessDaysInMonth(int year, int month)
    {
        var firstDay = new DateOnly(year, month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);
        
        for (DateOnly current = firstDay; current <= lastDay; current = current.AddDays(1))
        {
            if (IsBusinessDay(current))
            {
                yield return current;
            }
        }
    }
}
```

### 5. **Date Range and Period Calculations**
```csharp
public record DateRange(DateOnly Start, DateOnly End)
{
    public int DurationInDays => End.DayNumber - Start.DayNumber + 1;
    
    public bool Contains(DateOnly date) => date >= Start && date <= End;
    
    public bool Overlaps(DateRange other) => Start <= other.End && End >= other.Start;
    
    public DateRange? Intersection(DateRange other)
    {
        DateOnly start = DateOnly.FromDayNumber(Math.Max(Start.DayNumber, other.Start.DayNumber));
        DateOnly end = DateOnly.FromDayNumber(Math.Min(End.DayNumber, other.End.DayNumber));
        
        return start <= end ? new DateRange(start, end) : null;
    }
    
    public IEnumerable<DateOnly> GetDates()
    {
        for (DateOnly current = Start; current <= End; current = current.AddDays(1))
        {
            yield return current;
        }
    }
}

public static class AgeCalculator
{
    public static int CalculateAge(DateOnly birthDate, DateOnly currentDate)
    {
        int age = currentDate.Year - birthDate.Year;
        
        // Subtract a year if birthday hasn't occurred this year yet
        if (currentDate < birthDate.AddYears(age))
        {
            age--;
        }
        
        return age;
    }
    
    public static TimeSpan CalculatePreciseAge(DateTime birthDate, DateTime currentDate)
    {
        return currentDate - birthDate;
    }
    
    public static (int years, int months, int days) CalculateDetailedAge(DateOnly birthDate, DateOnly currentDate)
    {
        int years = currentDate.Year - birthDate.Year;
        int months = currentDate.Month - birthDate.Month;
        int days = currentDate.Day - birthDate.Day;
        
        if (days < 0)
        {
            months--;
            days += DateTime.DaysInMonth(currentDate.Year, currentDate.Month == 1 ? 12 : currentDate.Month - 1);
        }
        
        if (months < 0)
        {
            years--;
            months += 12;
        }
        
        return (years, months, days);
    }
}
```

### 6. **Scheduling and Recurring Events**
```csharp
public class EventScheduler
{
    public record ScheduledEvent(string Name, DateTime StartTime, TimeSpan Duration);
    
    private readonly List<ScheduledEvent> events = new();
    
    public void ScheduleEvent(string name, DateTime startTime, TimeSpan duration)
    {
        var newEvent = new ScheduledEvent(name, startTime, duration);
        
        // Check for conflicts
        if (HasConflict(newEvent))
        {
            throw new InvalidOperationException($"Event '{name}' conflicts with existing events");
        }
        
        events.Add(newEvent);
    }
    
    public bool HasConflict(ScheduledEvent newEvent)
    {
        DateTime newEventEnd = newEvent.StartTime + newEvent.Duration;
        
        return events.Any(existingEvent =>
        {
            DateTime existingEnd = existingEvent.StartTime + existingEvent.Duration;
            return newEvent.StartTime < existingEnd && newEventEnd > existingEvent.StartTime;
        });
    }
    
    public IEnumerable<ScheduledEvent> GetEventsForDay(DateOnly date)
    {
        return events.Where(e => DateOnly.FromDateTime(e.StartTime) == date);
    }
    
    public IEnumerable<DateTime> GenerateRecurringDates(DateTime start, TimeSpan interval, int count)
    {
        DateTime current = start;
        for (int i = 0; i < count; i++)
        {
            yield return current;
            current += interval;
        }
    }
}
```

## Tips

### DateTime Best Practices
- **Use DateTimeOffset for applications**: Provides timezone context for global applications
- **Store UTC in databases**: Convert to local time for display only
- **Be careful with DateTime.Now**: Consider using DateTimeOffset.Now for timezone awareness
- **Use DateOnly/TimeOnly when appropriate**: More semantic and prevents time-related bugs

### TimeSpan Optimization
- **Cache frequently used TimeSpans**: Create static readonly instances for common durations
- **Use appropriate precision**: Don't use milliseconds when seconds suffice
- **Consider performance**: TimeSpan operations are fast, but avoid unnecessary calculations in loops

### Formatting and Parsing
- **Always specify culture**: Use CultureInfo for consistent formatting across different locales
- **Use TryParse methods**: Safer than Parse() methods, avoids exceptions
- **Test edge cases**: Leap years, month boundaries, daylight saving time transitions

## Real-World Applications

### Logging System with Timestamps
```csharp
public class Logger
{
    private readonly string logFile;
    
    public Logger(string logFile) => this.logFile = logFile;
    
    public void Log(string message, LogLevel level = LogLevel.Info)
    {
        string timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
        string logEntry = $"[{timestamp}] [{level}] {message}";
        
        File.AppendAllText(logFile, logEntry + Environment.NewLine);
    }
    
    public IEnumerable<string> GetLogsForDateRange(DateOnly start, DateOnly end)
    {
        return File.ReadAllLines(logFile)
            .Where(line => IsInDateRange(line, start, end));
    }
    
    private bool IsInDateRange(string logLine, DateOnly start, DateOnly end)
    {
        // Extract timestamp from log line and check if it falls within range
        // Implementation would parse the timestamp and compare dates
        return true; // Simplified for example
    }
}
```

### Meeting Scheduler
```csharp
public class MeetingScheduler
{
    public record Meeting(string Title, DateTimeOffset Start, TimeSpan Duration, string[] Attendees);
    
    private readonly List<Meeting> meetings = new();
    
    public bool ScheduleMeeting(string title, DateTimeOffset start, TimeSpan duration, string[] attendees)
    {
        // Check if all attendees are available
        if (!AreAttendeesAvailable(attendees, start, duration))
        {
            return false;
        }
        
        meetings.Add(new Meeting(title, start, duration, attendees));
        return true;
    }
    
    public IEnumerable<Meeting> GetMeetingsForWeek(DateOnly weekStart)
    {
        DateOnly weekEnd = weekStart.AddDays(6);
        
        return meetings.Where(m =>
        {
            DateOnly meetingDate = DateOnly.FromDateTime(m.Start.DateTime);
            return meetingDate >= weekStart && meetingDate <= weekEnd;
        });
    }
    
    private bool AreAttendeesAvailable(string[] attendees, DateTimeOffset start, TimeSpan duration)
    {
        DateTimeOffset end = start + duration;
        
        return !meetings.Any(existing =>
        {
            DateTimeOffset existingEnd = existing.Start + existing.Duration;
            bool timeOverlaps = start < existingEnd && end > existing.Start;
            bool hasCommonAttendee = existing.Attendees.Intersect(attendees).Any();
            
            return timeOverlaps && hasCommonAttendee;
        });
    }
}
```

### Performance Monitor
```csharp
public class PerformanceMonitor
{
    private readonly Dictionary<string, List<TimeSpan>> executionTimes = new();
    
    public T MeasureExecution<T>(string operationName, Func<T> operation)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        T result = operation();
        stopwatch.Stop();
        
        RecordExecutionTime(operationName, stopwatch.Elapsed);
        return result;
    }
    
    public void RecordExecutionTime(string operationName, TimeSpan duration)
    {
        if (!executionTimes.ContainsKey(operationName))
        {
            executionTimes[operationName] = new List<TimeSpan>();
        }
        
        executionTimes[operationName].Add(duration);
    }
    
    public PerformanceStats GetStats(string operationName)
    {
        if (!executionTimes.ContainsKey(operationName))
        {
            return new PerformanceStats();
        }
        
        var times = executionTimes[operationName];
        return new PerformanceStats
        {
            Count = times.Count,
            Average = TimeSpan.FromTicks((long)times.Average(t => t.Ticks)),
            Min = times.Min(),
            Max = times.Max(),
            Total = TimeSpan.FromTicks(times.Sum(t => t.Ticks))
        };
    }
}

public record PerformanceStats
{
    public int Count { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public TimeSpan Total { get; init; }
}
```


##  Industry Impact

Date and time handling is critical for:

**Financial Trading**: Precise timestamps for transactions, market opening/closing times
**Healthcare Systems**: Appointment scheduling, medication timing, patient record timestamps
**E-commerce**: Order processing, delivery scheduling, time-limited promotions
**Gaming**: Event scheduling, leaderboard resets, time-based gameplay mechanics
**Enterprise Software**: Reporting periods, audit trails, SLA tracking, business hours logic

## Integration with Modern C#

**Pattern Matching with DateTime (C# 7+)**:
```csharp
string GetTimeOfDay(TimeOnly time) => time switch
{
    var t when t < new TimeOnly(12, 0) => "Morning",
    var t when t < new TimeOnly(17, 0) => "Afternoon", 
    var t when t < new TimeOnly(21, 0) => "Evening",
    _ => "Night"
};
```

**Record Types for Time Periods (C# 9+)**:
```csharp
public record TimePeriod(DateTimeOffset Start, DateTimeOffset End)
{
    public TimeSpan Duration => End - Start;
    public bool Contains(DateTimeOffset dateTime) => dateTime >= Start && dateTime <= End;
}
```

**Target-Typed New Expressions (C# 9+)**:
```csharp
TimeSpan workingHours = new(8, 0, 0);  // Shorter syntax
DateOnly today = new(2024, 5, 29);     // More concise
```


