using System;

namespace WorkingWithEnums
{
    /// <summary>
    /// Comprehensive demonstration of working with Enums in C#
    /// This project covers all fundamental enum operations, conversions, and advanced techniques
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Working with Enums in C# ===");
            Console.WriteLine("This demonstration covers enum conversions, string representations, flags, and advanced techniques\n");

            // Execute all demonstration methods
            DemonstrateBasicEnumOperations();
            DemonstrateEnumToIntegralConversions();
            DemonstrateGenericEnumConversions();
            DemonstrateEnumToStringConversions();
            DemonstrateIntegralToEnumConversions();
            DemonstrateStringToEnumParsing();
            DemonstrateEnumeratingEnumValues();
            DemonstrateFlagsEnums();
            DemonstrateAdvancedFlagsOperations();
            DemonstrateEnumLimitations();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== End of Enum Demonstration ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region 1. Basic Enum Operations
        /// <summary>
        /// Simple enum for basic operations demonstration
        /// Notice how we assign specific values to maintain control over the underlying integers
        /// </summary>
        public enum Priority
        {
            Low = 1,
            Medium = 2,
            High = 3,
            Critical = 4
        }

        /// <summary>
        /// Shows basic enum usage and the relationship between enum names and their underlying values
        /// This is the foundation - understanding that enums are essentially named integers
        /// </summary>
        static void DemonstrateBasicEnumOperations()
        {
            Console.WriteLine("1. === Basic Enum Operations ===");
            
            // Creating enum variables - this is how you'll use enums 90% of the time
            Priority taskPriority = Priority.High;
            Priority bugPriority = Priority.Critical;
            
            Console.WriteLine($"Task priority: {taskPriority}");
            Console.WriteLine($"Bug priority: {bugPriority}");
            
            // Comparing enums - works just like you'd expect
            if (bugPriority > taskPriority)
            {
                Console.WriteLine("Bug has higher priority than task");
            }
            
            // Enum values can be used in switch statements - very common pattern
            string priorityDescription = taskPriority switch
            {
                Priority.Low => "Can wait for next sprint",
                Priority.Medium => "Should be done this week",
                Priority.High => "Needs attention soon",
                Priority.Critical => "Drop everything and fix this!",
                _ => "Unknown priority"
            };
            
            Console.WriteLine($"Priority description: {priorityDescription}");
            Console.WriteLine();
        }
        #endregion

        #region 2. Enum to Integral Conversions
        /// <summary>
        /// Flags enum for demonstrating bitwise operations
        /// The [Flags] attribute tells everyone this enum is designed for combining values
        /// </summary>
        [Flags]
        public enum BorderSides 
        { 
            None = 0,
            Left = 1, 
            Right = 2, 
            Top = 4, 
            Bottom = 8,
            // Pre-defined combinations for convenience
            LeftRight = Left | Right,
            TopBottom = Top | Bottom,
            All = Left | Right | Top | Bottom
        }

        /// <summary>
        /// Converting enums to their underlying integer values
        /// This is straightforward but important to understand for debugging and data storage
        /// </summary>
        static void DemonstrateEnumToIntegralConversions()
        {
            Console.WriteLine("2. === Enum to Integral Conversions ===");
            
            // Basic conversion - explicit cast required because it's a narrowing conversion
            BorderSides side = BorderSides.Top;
            int sideValue = (int)side;
            
            Console.WriteLine($"BorderSides.Top as integer: {sideValue}");
            
            // With flags, you can see how bitwise OR creates combined values
            BorderSides combined = BorderSides.Left | BorderSides.Right;
            int combinedValue = (int)combined;
            
            Console.WriteLine($"Left | Right = {combinedValue} (1 + 2 = 3)");
            
            // When working with different underlying types, cast accordingly
            Priority priority = Priority.Critical;
            int priorityValue = (int)priority;
            
            Console.WriteLine($"Priority.Critical as integer: {priorityValue}");
            
            // You can also cast to other integral types
            byte priorityAsByte = (byte)priority;
            long priorityAsLong = (long)priority;
            
            Console.WriteLine($"Priority.Critical as byte: {priorityAsByte}");
            Console.WriteLine($"Priority.Critical as long: {priorityAsLong}");
            
            Console.WriteLine();
        }
        #endregion

        #region 3. Generic Enum Conversions
        /// <summary>
        /// Enum with different underlying type for demonstration
        /// Sometimes you need byte or long instead of int to save memory or handle large values
        /// </summary>
        public enum FileSize : long
        {
            Empty = 0,
            Small = 1024,                    // 1 KB
            Medium = 1048576,                // 1 MB
            Large = 1073741824,              // 1 GB
            Huge = 1099511627776             // 1 TB
        }

        /// <summary>
        /// Generic methods for working with any enum type
        /// This is useful when you're building utilities or frameworks
        /// </summary>
        static void DemonstrateGenericEnumConversions()
        {
            Console.WriteLine("3. === Generic Enum Conversions ===");
            
            // Using our generic utility method with different enum types
            Console.WriteLine($"BorderSides.Top integral value: {GetIntegralValue(BorderSides.Top)}");
            Console.WriteLine($"Priority.High integral value: {GetIntegralValue(Priority.High)}");
            Console.WriteLine($"FileSize.Large integral value: {GetIntegralValue(FileSize.Large)}");
            
            // The boxed version preserves the original type
            object boxedBorderSide = GetBoxedIntegralValue(BorderSides.Top);
            object boxedFileSize = GetBoxedIntegralValue(FileSize.Large);
            
            Console.WriteLine($"BorderSides.Top boxed type: {boxedBorderSide.GetType()}");
            Console.WriteLine($"FileSize.Large boxed type: {boxedFileSize.GetType()}");
            
            // Demonstrating why the boxed version is better
            Console.WriteLine($"FileSize.Large as long: {(long)boxedFileSize:N0}");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Generic utility method that works with any enum
        /// This approach requires knowing the target type, but it's simple and efficient
        /// </summary>
        static int GetIntegralValue(Enum anyEnum)
        {
            // Double cast: Enum -> object -> int
            // This works for most enums but assumes underlying type is int or smaller
            return (int)(object)anyEnum;
        }
        
        /// <summary>
        /// More robust generic method that preserves the original integral type
        /// Use this when you need to handle enums with different underlying types properly
        /// </summary>
        static object GetBoxedIntegralValue(Enum anyEnum)
        {
            // Get the actual underlying type of the enum
            Type integralType = Enum.GetUnderlyingType(anyEnum.GetType());
            
            // Convert to that specific type, preserving the original precision
            return Convert.ChangeType(anyEnum, integralType);
        }
        #endregion

        #region 4. Enum to String Conversions
        /// <summary>
        /// Demonstrates various ways to convert enums to strings
        /// String conversion is crucial for UI display, logging, and serialization
        /// </summary>
        static void DemonstrateEnumToStringConversions()
        {
            Console.WriteLine("4. === Enum to String Conversions ===");
            
            BorderSides side = BorderSides.Top;
            Priority priority = Priority.High;
            
            // Default ToString() - gives you the name, which is usually what you want
            Console.WriteLine($"Default ToString(): {side.ToString()}");
            
            // Format specifiers give you different representations
            Console.WriteLine($"Name format (G): {side.ToString("G")}");           // Same as default
            Console.WriteLine($"Decimal format (D): {side.ToString("D")}");        // Underlying value
            Console.WriteLine($"Hexadecimal format (X): {side.ToString("X")}");    // Hex value
            Console.WriteLine($"Flags format (F): {side.ToString("F")}");          // Smart flags display
            
            // With combined flags, the output becomes more interesting
            BorderSides combined = BorderSides.Left | BorderSides.Right;
            Console.WriteLine($"\nCombined flags: {combined}");
            Console.WriteLine($"Combined as decimal: {combined.ToString("D")}");
            Console.WriteLine($"Combined as hex: {combined.ToString("X")}");
            Console.WriteLine($"Combined with flags format: {combined.ToString("F")}");
            
            // Using Enum.Format for the same results
            Console.WriteLine($"\nUsing Enum.Format:");
            Console.WriteLine($"Enum.Format name: {Enum.Format(typeof(BorderSides), side, "G")}");
            Console.WriteLine($"Enum.Format decimal: {Enum.Format(typeof(BorderSides), side, "D")}");
            
            // Custom formatting for display purposes
            string userFriendlyPriority = FormatPriorityForDisplay(priority);
            Console.WriteLine($"User-friendly priority: {userFriendlyPriority}");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Helper method showing how you might format enums for user display
        /// In real applications, you often want more descriptive text than the enum name
        /// </summary>
        static string FormatPriorityForDisplay(Priority priority)
        {
            return priority switch
            {
                Priority.Low => "⚪ Low Priority",
                Priority.Medium => "🟡 Medium Priority", 
                Priority.High => "🟠 High Priority",
                Priority.Critical => "🔴 Critical Priority",
                _ => "❓ Unknown Priority"
            };
        }
        #endregion

        #region 5. Integral to Enum Conversions
        /// <summary>
        /// Converting integers back to enum values
        /// This is common when reading from databases, config files, or APIs
        /// </summary>
        static void DemonstrateIntegralToEnumConversions()
        {
            Console.WriteLine("5. === Integral to Enum Conversions ===");
            
            // Basic casting from int to enum
            int priorityValue = 3;
            Priority priority = (Priority)priorityValue;
            Console.WriteLine($"Integer {priorityValue} as Priority: {priority}");
            
            // Using Enum.ToObject for dynamic conversion
            object borderSideObj = Enum.ToObject(typeof(BorderSides), 3);
            Console.WriteLine($"Integer 3 as BorderSides object: {borderSideObj}");
            
            // Cast the object back to the specific enum type
            BorderSides borderSide = (BorderSides)borderSideObj;
            Console.WriteLine($"Cast back to BorderSides: {borderSide}");
            
            // Working with combined flag values
            int combinedValue = 5; // 1 + 4 = Left + Top
            BorderSides combinedSides = (BorderSides)combinedValue;
            Console.WriteLine($"Integer {combinedValue} as combined BorderSides: {combinedSides}");
            
            // Be careful with invalid values - this won't throw an exception!
            int invalidValue = 999;
            Priority invalidPriority = (Priority)invalidValue;
            Console.WriteLine($"Invalid value {invalidValue} as Priority: {invalidPriority}");
            Console.WriteLine($"Is {invalidPriority} a defined Priority? {Enum.IsDefined(typeof(Priority), invalidPriority)}");
            
            // Safe conversion with validation
            if (TryConvertToPriority(2, out Priority safePriority))
            {
                Console.WriteLine($"Safe conversion successful: {safePriority}");
            }
            
            if (!TryConvertToPriority(999, out Priority invalidSafePriority))
            {
                Console.WriteLine("Safe conversion failed for invalid value 999");
            }
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Safe conversion utility that validates the integer value
        /// Use this pattern when you're not sure if the integer represents a valid enum value
        /// </summary>
        static bool TryConvertToPriority(int value, out Priority priority)
        {
            if (Enum.IsDefined(typeof(Priority), value))
            {
                priority = (Priority)value;
                return true;
            }
            
            priority = default;
            return false;
        }
        #endregion

        #region 6. String to Enum Parsing
        /// <summary>
        /// Parsing strings back into enum values
        /// Essential for configuration files, user input, and API parameters
        /// </summary>
        static void DemonstrateStringToEnumParsing()
        {
            Console.WriteLine("6. === String to Enum Parsing ===");
            
            // Basic parsing - case sensitive by default
            string priorityText = "High";
            Priority parsedPriority = (Priority)Enum.Parse(typeof(Priority), priorityText);
            Console.WriteLine($"Parsed '{priorityText}' to Priority: {parsedPriority}");
            
            // Case-insensitive parsing
            string lowercaseText = "medium";
            Priority caseInsensitivePriority = (Priority)Enum.Parse(typeof(Priority), lowercaseText, true);
            Console.WriteLine($"Parsed '{lowercaseText}' (case-insensitive) to Priority: {caseInsensitivePriority}");
            
            // Parsing combined flag values
            string combinedText = "Left, Right";
            BorderSides combinedSides = (BorderSides)Enum.Parse(typeof(BorderSides), combinedText);
            Console.WriteLine($"Parsed '{combinedText}' to BorderSides: {combinedSides}");
            
            // Safe parsing with TryParse - much better for user input
            Console.WriteLine("\nSafe parsing examples:");
            
            string[] testInputs = { "Critical", "invalid", "LOW", "Left,Top", "" };
            
            foreach (string input in testInputs)
            {
                if (Enum.TryParse<Priority>(input, true, out Priority result))
                {
                    Console.WriteLine($"✓ Successfully parsed '{input}' as Priority: {result}");
                }
                else if (Enum.TryParse<BorderSides>(input, true, out BorderSides borderResult))
                {
                    Console.WriteLine($"✓ Successfully parsed '{input}' as BorderSides: {borderResult}");
                }
                else
                {
                    Console.WriteLine($"✗ Failed to parse '{input}' as any known enum");
                }
            }
            
            // Generic parsing utility
            if (TryParseEnum<Priority>("high", out Priority genericResult))
            {
                Console.WriteLine($"Generic parse successful: {genericResult}");
            }
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Generic utility for parsing any enum type
        /// This is handy when building configuration systems or APIs
        /// </summary>
        static bool TryParseEnum<T>(string value, out T result) where T : struct, Enum
        {
            return Enum.TryParse<T>(value, true, out result);
        }
        #endregion

        #region 7. Enumerating Enum Values
        /// <summary>
        /// Status enum for demonstration
        /// </summary>
        public enum TaskStatus
        {
            NotStarted,
            InProgress,
            Testing,
            CodeReview,
            Done,
            Cancelled
        }

        /// <summary>
        /// Getting all values and names from an enum
        /// Useful for building UI dropdowns, validation, and reflection-based operations
        /// </summary>
        static void DemonstrateEnumeratingEnumValues()
        {
            Console.WriteLine("7. === Enumerating Enum Values ===");
            
            // Get all values - returns Array, so you need to cast
            Console.WriteLine("All Priority values:");
            foreach (Priority priority in Enum.GetValues<Priority>())
            {
                Console.WriteLine($"  {priority} = {(int)priority}");
            }
            
            // Alternative way using the older method
            Console.WriteLine("\nAll TaskStatus values (older syntax):");
            foreach (TaskStatus status in Enum.GetValues(typeof(TaskStatus)))
            {
                Console.WriteLine($"  {status} = {(int)status}");
            }
            
            // Get all names as strings
            Console.WriteLine("\nAll BorderSides names:");
            string[] borderNames = Enum.GetNames<BorderSides>();
            foreach (string name in borderNames)
            {
                Console.WriteLine($"  {name}");
            }
            
            // Practical example: Building a dropdown list
            Console.WriteLine("\nBuilding UI dropdown data:");
            var priorityOptions = BuildDropdownOptions<Priority>();
            foreach (var option in priorityOptions)
            {
                Console.WriteLine($"  Value: {option.Value}, Display: {option.Text}");
            }
            
            // Count and validate enum members
            int priorityCount = Enum.GetValues<Priority>().Length;
            Console.WriteLine($"\nTotal Priority enum members: {priorityCount}");
            
            // Check if a value is defined
            Console.WriteLine($"Is Priority value 3 defined? {Enum.IsDefined(typeof(Priority), 3)}");
            Console.WriteLine($"Is Priority value 99 defined? {Enum.IsDefined(typeof(Priority), 99)}");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Utility method for building dropdown/select list options from any enum
        /// This is a common pattern in web applications and desktop UIs
        /// </summary>
        static List<(int Value, string Text)> BuildDropdownOptions<T>() where T : struct, Enum
        {
            var options = new List<(int Value, string Text)>();
            
            foreach (T enumValue in Enum.GetValues<T>())
            {
                int value = Convert.ToInt32(enumValue);
                string text = enumValue.ToString();
                options.Add((value, text));
            }
            
            return options;
        }
        #endregion

        #region 8. Flags Enums
        /// <summary>
        /// File permissions enum using flags pattern
        /// This is a classic example of how flags are used in real systems
        /// </summary>
        [Flags]
        public enum FilePermissions
        {
            None = 0,
            Read = 1,
            Write = 2,
            Execute = 4,
            Delete = 8,
            // Common combinations
            ReadWrite = Read | Write,
            FullControl = Read | Write | Execute | Delete
        }

        /// <summary>
        /// Basic flags operations
        /// This demonstrates the core concept of flags - combining multiple values into one
        /// </summary>
        static void DemonstrateFlagsEnums()
        {
            Console.WriteLine("8. === Flags Enums ===");
            
            // Combining flags with bitwise OR
            FilePermissions permissions = FilePermissions.Read | FilePermissions.Write;
            Console.WriteLine($"Combined permissions: {permissions}");
            Console.WriteLine($"Combined permissions value: {(int)permissions}");
            
            // Checking if a specific flag is set
            bool canRead = (permissions & FilePermissions.Read) != 0;
            bool canExecute = (permissions & FilePermissions.Execute) != 0;
            
            Console.WriteLine($"Can read: {canRead}");
            Console.WriteLine($"Can execute: {canExecute}");
            
            // Adding a flag
            permissions |= FilePermissions.Execute;
            Console.WriteLine($"After adding Execute: {permissions}");
            
            // Removing a flag
            permissions &= ~FilePermissions.Write;
            Console.WriteLine($"After removing Write: {permissions}");
            
            // Toggling a flag
            permissions ^= FilePermissions.Delete;
            Console.WriteLine($"After toggling Delete: {permissions}");
            
            // Working with pre-defined combinations
            FilePermissions fullControl = FilePermissions.FullControl;
            Console.WriteLine($"Full control permissions: {fullControl}");
            Console.WriteLine($"Full control value: {(int)fullControl}");
            
            // The power of flags: storing multiple boolean values in a single integer
            ShowPermissionAnalysis(FilePermissions.Read | FilePermissions.Execute);
            ShowPermissionAnalysis(FilePermissions.FullControl);
            ShowPermissionAnalysis(FilePermissions.None);
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Utility method to analyze flag permissions
        /// This shows a common pattern for working with flags in business logic
        /// </summary>
        static void ShowPermissionAnalysis(FilePermissions permissions)
        {
            Console.WriteLine($"\nAnalyzing permissions: {permissions} (value: {(int)permissions})");
            
            string[] checks = 
            {
                $"  Read: {permissions.HasFlag(FilePermissions.Read)}",
                $"  Write: {permissions.HasFlag(FilePermissions.Write)}",
                $"  Execute: {permissions.HasFlag(FilePermissions.Execute)}",
                $"  Delete: {permissions.HasFlag(FilePermissions.Delete)}"
            };
            
            foreach (string check in checks)
            {
                Console.WriteLine(check);
            }
        }
        #endregion

        #region 9. Advanced Flags Operations
        /// <summary>
        /// Day of week flags for more complex scenarios
        /// </summary>
        [Flags]
        public enum DaysOfWeek
        {
            None = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 4,
            Thursday = 8,
            Friday = 16,
            Saturday = 32,
            Sunday = 64,
            // Useful combinations
            Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
            Weekend = Saturday | Sunday,
            All = Weekdays | Weekend
        }

        /// <summary>
        /// Advanced flags operations and patterns
        /// This shows more sophisticated ways to work with flags in real applications
        /// </summary>
        static void DemonstrateAdvancedFlagsOperations()
        {
            Console.WriteLine("9. === Advanced Flags Operations ===");
            
            // Complex flag combinations
            DaysOfWeek workDays = DaysOfWeek.Weekdays;
            DaysOfWeek meetingDays = DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday;
            
            Console.WriteLine($"Work days: {workDays}");
            Console.WriteLine($"Meeting days: {meetingDays}");
            
            // Finding overlaps
            DaysOfWeek overlap = workDays & meetingDays;
            Console.WriteLine($"Days that are both work and meeting days: {overlap}");
            
            // Finding differences
            DaysOfWeek nonMeetingWorkDays = workDays & ~meetingDays;
            Console.WriteLine($"Work days without meetings: {nonMeetingWorkDays}");
            
            // Getting individual flags from a combined value
            Console.WriteLine("\nIndividual days in work schedule:");
            foreach (DaysOfWeek day in GetIndividualFlags(workDays))
            {
                Console.WriteLine($"  {day}");
            }
            
            // Counting set flags
            int workDayCount = CountSetFlags(workDays);
            Console.WriteLine($"Number of work days: {workDayCount}");
            
            // Validating flag combinations
            Console.WriteLine($"Is Monday a work day? {IsValidWorkDay(DaysOfWeek.Monday, workDays)}");
            Console.WriteLine($"Is Saturday a work day? {IsValidWorkDay(DaysOfWeek.Saturday, workDays)}");
            
            // Converting flags to collections for easier processing
            List<DaysOfWeek> meetingDaysList = GetFlagsAsList(meetingDays);
            Console.WriteLine($"Meeting days as list: [{string.Join(", ", meetingDaysList)}]");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Utility to extract individual flags from a combined flags value
        /// This is useful when you need to process each flag separately
        /// </summary>
        static IEnumerable<DaysOfWeek> GetIndividualFlags(DaysOfWeek combinedFlags)
        {
            foreach (DaysOfWeek day in Enum.GetValues<DaysOfWeek>())
            {
                // Skip None and composite values, only return individual days
                if (day != DaysOfWeek.None && combinedFlags.HasFlag(day) && IsPowerOfTwo((int)day))
                {
                    yield return day;
                }
            }
        }
        
        /// <summary>
        /// Count how many individual flags are set
        /// </summary>
        static int CountSetFlags(DaysOfWeek flags)
        {
            int count = 0;
            int value = (int)flags;
            
            // Brian Kernighan's algorithm for counting set bits
            while (value != 0)
            {
                count++;
                value &= value - 1; // Clear the lowest set bit
            }
            
            return count;
        }
        
        /// <summary>
        /// Check if a specific day is included in a work schedule
        /// </summary>
        static bool IsValidWorkDay(DaysOfWeek day, DaysOfWeek workSchedule)
        {
            return workSchedule.HasFlag(day);
        }
        
        /// <summary>
        /// Convert flags enum to a list for easier processing
        /// </summary>
        static List<DaysOfWeek> GetFlagsAsList(DaysOfWeek flags)
        {
            return GetIndividualFlags(flags).ToList();
        }
        
        /// <summary>
        /// Helper method to check if a number is a power of 2
        /// </summary>
        static bool IsPowerOfTwo(int x)
        {
            return x > 0 && (x & (x - 1)) == 0;
        }
        #endregion

        #region 10. Enum Limitations
        /// <summary>
        /// Demonstrates the limitations and potential pitfalls of enums
        /// Understanding these is crucial for writing robust code
        /// </summary>
        static void DemonstrateEnumLimitations()
        {
            Console.WriteLine("10. === Enum Limitations ===");
            
            // Limitation 1: No strong runtime type safety
            BorderSides side = BorderSides.Left;
            Console.WriteLine($"Initial side: {side} (value: {(int)side})");
            
            // This compiles and runs but probably isn't what you intended
            side += 1234;
            Console.WriteLine($"After adding 1234: {side} (value: {(int)side})");
            Console.WriteLine($"Is this a defined BorderSides value? {Enum.IsDefined(typeof(BorderSides), side)}");
            
            // Limitation 2: Invalid values don't throw exceptions
            Priority invalidPriority = (Priority)999;
            Console.WriteLine($"Invalid priority (999): {invalidPriority}");
            Console.WriteLine($"ToString() still works: '{invalidPriority.ToString()}'");
            
            // Limitation 3: Flags can have unexpected combinations
            DaysOfWeek invalidDays = (DaysOfWeek)255; // All bits set
            Console.WriteLine($"Invalid days combination: {invalidDays}");
            
            // Best practices for handling these limitations
            Console.WriteLine("\nBest practices:");
            
            // Always validate when converting from external sources
            if (TryParseEnum<Priority>("Medium", out Priority validPriority))
            {
                Console.WriteLine($"✓ Valid priority parsed: {validPriority}");
            }
            
            // Use IsDefined for validation
            int userInput = 2;
            if (Enum.IsDefined(typeof(Priority), userInput))
            {
                Priority safePriority = (Priority)userInput;
                Console.WriteLine($"✓ Valid priority from user input: {safePriority}");
            }
            else
            {
                Console.WriteLine($"✗ Invalid priority value from user: {userInput}");
            }
            
            // For flags, validate against known combinations
            DaysOfWeek userDays = DaysOfWeek.Monday | DaysOfWeek.Wednesday;
            if (IsValidDaysCombination(userDays))
            {
                Console.WriteLine($"✓ Valid days combination: {userDays}");
            }
            
            // Be careful with arithmetic operations
            Console.WriteLine("\nArithmetic operation dangers:");
            Priority p1 = Priority.Medium;
            Priority p2 = Priority.High;
            
            // This might not make logical sense but it compiles
            Priority arithmeticResult = (Priority)((int)p1 + (int)p2);
            Console.WriteLine($"Priority.Medium + Priority.High = {arithmeticResult} (value: {(int)arithmeticResult})");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Validate that a days combination only uses defined individual values
        /// </summary>
        static bool IsValidDaysCombination(DaysOfWeek days)
        {
            // Get all valid individual day values
            DaysOfWeek validDays = DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday |
                                  DaysOfWeek.Thursday | DaysOfWeek.Friday | DaysOfWeek.Saturday | DaysOfWeek.Sunday;
            
            // Check if the combination only uses valid bits
            return (days & ~validDays) == 0;
        }
        #endregion

        #region 11. Real-World Scenarios
        /// <summary>
        /// Log level enum for logging systems
        /// </summary>
        public enum LogLevel
        {
            Trace = 0,
            Debug = 1,
            Information = 2,
            Warning = 3,
            Error = 4,
            Critical = 5
        }

        /// <summary>
        /// HTTP status codes enum (partial)
        /// </summary>
        public enum HttpStatusCode
        {
            OK = 200,
            Created = 201,
            BadRequest = 400,
            Unauthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            InternalServerError = 500
        }

        /// <summary>
        /// Feature flags for application configuration
        /// </summary>
        [Flags]
        public enum FeatureFlags
        {
            None = 0,
            DarkMode = 1,
            ExperimentalUI = 2,
            AdvancedReporting = 4,
            BetaFeatures = 8,
            DebugMode = 16,
            // Combinations for different user types
            StandardUser = DarkMode,
            PowerUser = DarkMode | AdvancedReporting,
            Developer = DarkMode | ExperimentalUI | BetaFeatures | DebugMode,
            All = DarkMode | ExperimentalUI | AdvancedReporting | BetaFeatures | DebugMode
        }

        /// <summary>
        /// Real-world scenarios demonstrating practical enum usage
        /// These examples show how enums solve actual business problems
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("11. === Real-World Scenarios ===");
            
            // Scenario 1: Logging system
            Console.WriteLine("Scenario 1: Logging System");
            SimulateLogging();
            
            // Scenario 2: HTTP API responses
            Console.WriteLine("\nScenario 2: HTTP API Response Handling");
            SimulateApiResponses();
            
            // Scenario 3: Feature flags
            Console.WriteLine("\nScenario 3: Feature Flag Management");
            SimulateFeatureFlags();
            
            // Scenario 4: State machine
            Console.WriteLine("\nScenario 4: Order Processing State Machine");
            SimulateOrderProcessing();
            
            // Scenario 5: Configuration settings
            Console.WriteLine("\nScenario 5: Application Configuration");
            SimulateApplicationConfig();
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Simulates a logging system using enums for log levels
        /// </summary>
        static void SimulateLogging()
        {
            LogLevel currentLogLevel = LogLevel.Information;
            
            // Simulate various log messages
            LogMessage(LogLevel.Trace, "Entering method CalculateTotal()", currentLogLevel);
            LogMessage(LogLevel.Debug, "User ID: 12345, Cart items: 3", currentLogLevel);
            LogMessage(LogLevel.Information, "Order created successfully", currentLogLevel);
            LogMessage(LogLevel.Warning, "Stock running low for item XYZ", currentLogLevel);
            LogMessage(LogLevel.Error, "Payment gateway timeout", currentLogLevel);
            LogMessage(LogLevel.Critical, "Database connection lost", currentLogLevel);
        }
        
        static void LogMessage(LogLevel level, string message, LogLevel minimumLevel)
        {
            // Only log if the level is at or above the minimum
            if (level >= minimumLevel)
            {
                string levelText = level.ToString().ToUpper();
                Console.WriteLine($"  [{levelText}] {message}");
            }
        }
        
        /// <summary>
        /// Simulates handling HTTP API responses
        /// </summary>
        static void SimulateApiResponses()
        {
            HttpStatusCode[] responses = 
            {
                HttpStatusCode.OK,
                HttpStatusCode.NotFound,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.InternalServerError
            };
            
            foreach (HttpStatusCode status in responses)
            {
                string message = GetHttpStatusMessage(status);
                bool isSuccess = IsSuccessStatusCode(status);
                Console.WriteLine($"  Status {(int)status} ({status}): {message} [Success: {isSuccess}]");
            }
        }
        
        static string GetHttpStatusMessage(HttpStatusCode status)
        {
            return status switch
            {
                HttpStatusCode.OK => "Request successful",
                HttpStatusCode.Created => "Resource created successfully",
                HttpStatusCode.BadRequest => "Invalid request data",
                HttpStatusCode.Unauthorized => "Authentication required",
                HttpStatusCode.Forbidden => "Access denied",
                HttpStatusCode.NotFound => "Resource not found",
                HttpStatusCode.InternalServerError => "Server error occurred",
                _ => "Unknown status"
            };
        }
        
        static bool IsSuccessStatusCode(HttpStatusCode status)
        {
            int code = (int)status;
            return code >= 200 && code < 300;
        }
        
        /// <summary>
        /// Simulates feature flag management
        /// </summary>
        static void SimulateFeatureFlags()
        {
            // Different user types with different feature access
            FeatureFlags guestUser = FeatureFlags.None;
            FeatureFlags standardUser = FeatureFlags.StandardUser;
            FeatureFlags powerUser = FeatureFlags.PowerUser;
            FeatureFlags developer = FeatureFlags.Developer;
            
            Console.WriteLine("  Feature access by user type:");
            ShowFeatureAccess("Guest", guestUser);
            ShowFeatureAccess("Standard User", standardUser);
            ShowFeatureAccess("Power User", powerUser);
            ShowFeatureAccess("Developer", developer);
        }
        
        static void ShowFeatureAccess(string userType, FeatureFlags flags)
        {
            Console.WriteLine($"    {userType}:");
            Console.WriteLine($"      Dark Mode: {flags.HasFlag(FeatureFlags.DarkMode)}");
            Console.WriteLine($"      Experimental UI: {flags.HasFlag(FeatureFlags.ExperimentalUI)}");
            Console.WriteLine($"      Advanced Reporting: {flags.HasFlag(FeatureFlags.AdvancedReporting)}");
            Console.WriteLine($"      Beta Features: {flags.HasFlag(FeatureFlags.BetaFeatures)}");
            Console.WriteLine($"      Debug Mode: {flags.HasFlag(FeatureFlags.DebugMode)}");
        }
        
        /// <summary>
        /// Order status for state machine example
        /// </summary>
        public enum OrderStatus
        {
            Pending,
            Confirmed,
            Processing,
            Shipped,
            Delivered,
            Cancelled,
            Returned
        }
        
        /// <summary>
        /// Simulates an order processing state machine
        /// </summary>
        static void SimulateOrderProcessing()
        {
            OrderStatus currentStatus = OrderStatus.Pending;
            
            Console.WriteLine($"  Starting order processing. Initial status: {currentStatus}");
            
            // Simulate order progression
            OrderStatus[] progressionPath = 
            {
                OrderStatus.Confirmed,
                OrderStatus.Processing,
                OrderStatus.Shipped,
                OrderStatus.Delivered
            };
            
            foreach (OrderStatus nextStatus in progressionPath)
            {
                if (CanTransitionTo(currentStatus, nextStatus))
                {
                    currentStatus = nextStatus;
                    Console.WriteLine($"  ✓ Transitioned to: {currentStatus}");
                }
                else
                {
                    Console.WriteLine($"  ✗ Cannot transition from {currentStatus} to {nextStatus}");
                }
            }
        }
        
        static bool CanTransitionTo(OrderStatus from, OrderStatus to)
        {
            // Define valid state transitions
            return (from, to) switch
            {
                (OrderStatus.Pending, OrderStatus.Confirmed) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Confirmed, OrderStatus.Processing) => true,
                (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Processing, OrderStatus.Cancelled) => true,
                (OrderStatus.Shipped, OrderStatus.Delivered) => true,
                (OrderStatus.Shipped, OrderStatus.Returned) => true,
                (OrderStatus.Delivered, OrderStatus.Returned) => true,
                _ => false
            };
        }
        
        /// <summary>
        /// Application configuration categories
        /// </summary>
        public enum ConfigCategory
        {
            Database,
            Logging,
            Security,
            Performance,
            UserInterface
        }
        
        /// <summary>
        /// Simulates application configuration management
        /// </summary>
        static void SimulateApplicationConfig()
        {
            // Simulate loading configuration by category
            foreach (ConfigCategory category in Enum.GetValues<ConfigCategory>())
            {
                var settings = LoadConfigurationSettings(category);
                Console.WriteLine($"  {category} settings: {settings.Count} items loaded");
                
                foreach (var setting in settings.Take(2)) // Show first 2 settings
                {
                    Console.WriteLine($"    {setting.Key}: {setting.Value}");
                }
            }
        }
        
        static Dictionary<string, string> LoadConfigurationSettings(ConfigCategory category)
        {
            // Simulate loading different types of configuration
            return category switch
            {
                ConfigCategory.Database => new Dictionary<string, string>
                {
                    ["ConnectionString"] = "Server=localhost;Database=MyApp",
                    ["CommandTimeout"] = "30",
                    ["PoolSize"] = "100"
                },
                ConfigCategory.Logging => new Dictionary<string, string>
                {
                    ["Level"] = "Information",
                    ["FilePath"] = "logs/app.log",
                    ["MaxFileSize"] = "10MB"
                },
                ConfigCategory.Security => new Dictionary<string, string>
                {
                    ["JwtSecret"] = "***HIDDEN***",
                    ["TokenExpiry"] = "60",
                    ["RequireHttps"] = "true"
                },
                ConfigCategory.Performance => new Dictionary<string, string>
                {
                    ["CacheExpiry"] = "300",
                    ["MaxConcurrentRequests"] = "100",
                    ["EnableCompression"] = "true"
                },
                ConfigCategory.UserInterface => new Dictionary<string, string>
                {
                    ["Theme"] = "Light",
                    ["Language"] = "en-US",
                    ["PageSize"] = "25"
                },
                _ => new Dictionary<string, string>()
            };
        }
        #endregion
    }
}
