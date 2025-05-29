using System.Globalization;

// Date and Time Handling Demonstration
// This project covers all essential concepts for working with dates and times in C#

namespace DateAndTimeHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== DATE AND TIME HANDLING DEMONSTRATION ===\n");

            // Let's walk through each concept with real-world examples
            DemonstrateTimeSpanBasics();
            DemonstrateTimeSpanOperations();
            DemonstrateDateTimeBasics();
            DemonstrateDateTimeVsDateTimeOffset();
            DemonstrateDateTimeOperations();
            DemonstrateDateTimeFormatting();
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

            // Different ways to create TimeSpan objects
            // Think of TimeSpan as a stopwatch measurement - how long something takes
            
            // Using constructors - like setting up a timer
            TimeSpan workingHours = new TimeSpan(8, 0, 0); // 8 hours
            TimeSpan shortBreak = new TimeSpan(0, 15, 0);  // 15 minutes
            TimeSpan projectDeadline = new TimeSpan(30, 0, 0, 0); // 30 days

            Console.WriteLine($"Standard working day: {workingHours}");
            Console.WriteLine($"Coffee break duration: {shortBreak}");
            Console.WriteLine($"Project timeline: {projectDeadline.Days} days");

            // Using static factory methods - more readable for specific units
            TimeSpan meetingDuration = TimeSpan.FromMinutes(45);
            TimeSpan lunchBreak = TimeSpan.FromHours(1.5);
            TimeSpan serverTimeout = TimeSpan.FromSeconds(30);
            TimeSpan databaseBackup = TimeSpan.FromHours(2.5);

            Console.WriteLine($"Meeting length: {meetingDuration}");
            Console.WriteLine($"Lunch break: {lunchBreak}");
            Console.WriteLine($"API timeout: {serverTimeout}");
            Console.WriteLine($"Backup duration: {databaseBackup}");

            // Creating TimeSpan from DateTime subtraction - very common pattern
            DateTime projectStart = new DateTime(2024, 1, 15);
            DateTime projectEnd = new DateTime(2024, 3, 15);
            TimeSpan projectDuration = projectEnd - projectStart;
            
            Console.WriteLine($"Project started: {projectStart:yyyy-MM-dd}");
            Console.WriteLine($"Project ended: {projectEnd:yyyy-MM-dd}");
            Console.WriteLine($"Total project time: {projectDuration.TotalDays} days");

            Console.WriteLine();
        }

        static void DemonstrateTimeSpanOperations()
        {
            Console.WriteLine("2. TIMESPAN OPERATIONS - MATH WITH TIME");
            Console.WriteLine("=======================================");

            // TimeSpan arithmetic - useful for scheduling and time calculations
            TimeSpan morningWork = TimeSpan.FromHours(4);
            TimeSpan afternoonWork = TimeSpan.FromHours(4);
            TimeSpan totalWork = morningWork + afternoonWork;
            
            Console.WriteLine($"Morning work: {morningWork}");
            Console.WriteLine($"Afternoon work: {afternoonWork}");
            Console.WriteLine($"Total work time: {totalWork}");

            // Subtracting time spans
            TimeSpan plannedTime = TimeSpan.FromHours(8);
            TimeSpan actualTime = TimeSpan.FromHours(6.5);
            TimeSpan timeSaved = plannedTime - actualTime;
            
            Console.WriteLine($"Time saved: {timeSaved} ({timeSaved.TotalMinutes} minutes)");

            // Working with TimeSpan components
            TimeSpan marathon = TimeSpan.FromSeconds(10567); // Random marathon time
            Console.WriteLine($"Marathon time breakdown:");
            Console.WriteLine($"  Hours: {marathon.Hours}");
            Console.WriteLine($"  Minutes: {marathon.Minutes}");
            Console.WriteLine($"  Seconds: {marathon.Seconds}");
            Console.WriteLine($"  Total hours: {marathon.TotalHours:F2}");

            // Practical example: Calculate overtime
            TimeSpan standardWorkDay = TimeSpan.FromHours(8);
            TimeSpan actualWorkDay = TimeSpan.FromHours(10.5);
            
            if (actualWorkDay > standardWorkDay)
            {
                TimeSpan overtime = actualWorkDay - standardWorkDay;
                Console.WriteLine($"Overtime worked: {overtime} ({overtime.TotalHours:F1} hours)");
            }

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

            // DateTime - good for local applications
            DateTime localMeeting = new DateTime(2024, 5, 29, 15, 30, 0, DateTimeKind.Local);
            Console.WriteLine($"Local meeting: {localMeeting}");

            // DateTimeOffset - better for distributed systems and APIs
            // Includes timezone offset information
            DateTimeOffset globalMeeting = new DateTimeOffset(2024, 5, 29, 15, 30, 0, TimeSpan.FromHours(7)); // Jakarta time
            DateTimeOffset nyMeeting = new DateTimeOffset(2024, 5, 29, 4, 30, 0, TimeSpan.FromHours(-4)); // New York time
            DateTimeOffset londonMeeting = new DateTimeOffset(2024, 5, 29, 9, 30, 0, TimeSpan.FromHours(1)); // London time

            Console.WriteLine($"Jakarta meeting: {globalMeeting}");
            Console.WriteLine($"New York meeting: {nyMeeting}");
            Console.WriteLine($"London meeting: {londonMeeting}");

            // All these times are actually the same moment!
            Console.WriteLine($"Jakarta UTC: {globalMeeting.UtcDateTime}");
            Console.WriteLine($"New York UTC: {nyMeeting.UtcDateTime}");
            Console.WriteLine($"London UTC: {londonMeeting.UtcDateTime}");

            // Current time with offset
            DateTimeOffset nowWithOffset = DateTimeOffset.Now;
            DateTimeOffset utcNowWithOffset = DateTimeOffset.UtcNow;
            
            Console.WriteLine($"Current time with offset: {nowWithOffset}");
            Console.WriteLine($"UTC time with offset: {utcNowWithOffset}");

            // Converting between timezones
            TimeZoneInfo jakartaZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            TimeZoneInfo nyZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime utcTime = DateTime.UtcNow;
            DateTime jakartaTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, jakartaZone);
            DateTime nyTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, nyZone);

            Console.WriteLine($"Same moment in different timezones:");
            Console.WriteLine($"  UTC: {utcTime}");
            Console.WriteLine($"  Jakarta: {jakartaTime}");
            Console.WriteLine($"  New York: {nyTime}");

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
            Console.WriteLine("6. DATETIME FORMATTING - DISPLAYING DATES");
            Console.WriteLine("==========================================");

            DateTime sampleDate = new DateTime(2024, 5, 29, 14, 30, 45);

            // Standard format strings
            Console.WriteLine("Standard formats:");
            Console.WriteLine($"  Short date: {sampleDate:d}");
            Console.WriteLine($"  Long date: {sampleDate:D}");
            Console.WriteLine($"  Short time: {sampleDate:t}");
            Console.WriteLine($"  Long time: {sampleDate:T}");
            Console.WriteLine($"  Full date/time: {sampleDate:F}");
            Console.WriteLine($"  General: {sampleDate:G}");
            Console.WriteLine($"  ISO 8601: {sampleDate:O}");

            // Custom format strings - very useful for reports and user interfaces
            Console.WriteLine("\nCustom formats:");
            Console.WriteLine($"  Database format: {sampleDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  User-friendly: {sampleDate:dddd, MMMM dd, yyyy}");
            Console.WriteLine($"  File naming: {sampleDate:yyyyMMdd_HHmmss}");
            Console.WriteLine($"  API timestamp: {sampleDate:yyyy-MM-ddTHH:mm:ssZ}");

            // Culture-specific formatting
            CultureInfo usCulture = new CultureInfo("en-US");
            CultureInfo ukCulture = new CultureInfo("en-GB");
            CultureInfo indonesianCulture = new CultureInfo("id-ID");

            Console.WriteLine("\nCultural differences:");
            Console.WriteLine($"  US format: {sampleDate.ToString("d", usCulture)}");
            Console.WriteLine($"  UK format: {sampleDate.ToString("d", ukCulture)}");
            Console.WriteLine($"  Indonesian: {sampleDate.ToString("d", indonesianCulture)}");

            // Parsing dates from strings - essential for user input
            string dateString = "2024-05-29";
            string timeString = "14:30:45";
            string fullString = "May 29, 2024 2:30 PM";

            if (DateTime.TryParse(dateString, out DateTime parsedDate))
                Console.WriteLine($"Parsed date: {parsedDate}");

            if (DateTime.TryParseExact(timeString, "HH:mm:ss", null, DateTimeStyles.None, out DateTime parsedTime))
                Console.WriteLine($"Parsed time: {parsedTime:T}");

            Console.WriteLine();
        }

        static void DemonstrateDateOnlyAndTimeOnly()
        {
            Console.WriteLine("7. DATEONLY AND TIMEONLY - SPECIALIZED TYPES (.NET 6+)");
            Console.WriteLine("=======================================================");

            // DateOnly - perfect for birthdays, holidays, deadlines without time
            DateOnly birthday = new DateOnly(1990, 6, 15);
            DateOnly holiday = new DateOnly(2024, 12, 25);
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            Console.WriteLine($"Birthday: {birthday}");
            Console.WriteLine($"Christmas: {holiday}");
            Console.WriteLine($"Today: {today}");

            // Calculate age using DateOnly
            int age = today.Year - birthday.Year;
            if (today < birthday.AddYears(age))
                age--;
            Console.WriteLine($"Current age: {age} years");

            // Days until next birthday
            DateOnly nextBirthday = birthday.AddYears(today.Year - birthday.Year);
            if (nextBirthday < today)
                nextBirthday = nextBirthday.AddYears(1);
            
            int daysUntilBirthday = nextBirthday.DayNumber - today.DayNumber;
            Console.WriteLine($"Days until next birthday: {daysUntilBirthday}");

            // TimeOnly - great for schedules, store hours, alarms
            TimeOnly storeOpen = new TimeOnly(9, 0);
            TimeOnly storeLunch = new TimeOnly(12, 0);
            TimeOnly storeClose = new TimeOnly(17, 30);
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);

            Console.WriteLine($"\nStore hours:");
            Console.WriteLine($"  Opens: {storeOpen}");
            Console.WriteLine($"  Lunch: {storeLunch}");
            Console.WriteLine($"  Closes: {storeClose}");
            Console.WriteLine($"  Current time: {currentTime}");

            // Check if store is open
            bool isOpen = currentTime >= storeOpen && currentTime <= storeClose && 
                         !(currentTime >= storeLunch && currentTime <= storeLunch.AddHours(1));
            Console.WriteLine($"  Store is {(isOpen ? "OPEN" : "CLOSED")}");

            // Working with time spans in TimeOnly
            TimeOnly meetingStart = new TimeOnly(14, 0);
            TimeSpan meetingDuration = TimeSpan.FromMinutes(90);
            TimeOnly meetingEnd = meetingStart.Add(meetingDuration);

            Console.WriteLine($"\nMeeting schedule:");
            Console.WriteLine($"  Start: {meetingStart}");
            Console.WriteLine($"  Duration: {meetingDuration}");
            Console.WriteLine($"  End: {meetingEnd}");

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
