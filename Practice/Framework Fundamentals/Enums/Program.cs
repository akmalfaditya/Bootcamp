using System;

namespace WorkingWithEnums
{
    /// <summary>
    /// Advanced Concepts and Utility Methods in .NET Enums
    /// This comprehensive demonstration explores the System.Enum type and its powerful capabilities
    /// covering type unification, static utility methods, and runtime behavior
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Advanced Concepts and Utility Methods in .NET Enums ===");
            Console.WriteLine("Exploring the System.Enum type and its powerful capabilities\n");

            // Part 1: System.Enum fundamentals
            DemonstrateSystemEnumTypeUnification();
            DemonstrateSystemEnumUtilityMethods();
            
            // Part 2: Comprehensive enum conversions
            DemonstrateBasicEnumOperations();
            DemonstrateEnumToIntegralConversions();
            DemonstrateGenericEnumConversions();
            DemonstrateEnumToStringConversions();
            DemonstrateIntegralToEnumConversions();
            DemonstrateStringToEnumParsing();
            DemonstrateEnumeratingEnumValues();
            
            // Part 3: Advanced scenarios
            DemonstrateFlagsEnums();
            DemonstrateAdvancedFlagsOperations();
            DemonstrateEnumUnderTheHood();
            DemonstrateBoxingBehavior();
            DemonstrateEnumLimitations();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== End of Enum Demonstration ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region 0. System.Enum Type Unification
        /// <summary>
        /// Simple enums for demonstrating System.Enum type unification
        /// Notice how different enum types can all be treated as System.Enum
        /// </summary>
        public enum Nut { Walnut, Hazelnut, Macadamia }
        public enum Size { Small, Medium, Large }

        /// <summary>
        /// Demonstrates how System.Enum serves as a common base type for all enum types
        /// This allows writing generic methods that can operate on any enum
        /// </summary>
        static void DemonstrateSystemEnumTypeUnification()
        {
            Console.WriteLine("0a. === System.Enum Type Unification ===");
            
            // Any enum member can be treated as a System.Enum instance
            // This enables polymorphic behavior with different enum types
            Display(Nut.Macadamia);    // Different enum types
            Display(Size.Large);       // can be passed to the same method
            Display(Priority.Critical);

            // The beauty of type unification - one method handles all enum types
            Console.WriteLine("\nDemonstrating generic enum operations:");
            ShowEnumInfo(Nut.Walnut);
            ShowEnumInfo(Size.Medium);
            ShowEnumInfo(Priority.High);
            
            Console.WriteLine();
        }

        /// <summary>
        /// Generic method that can accept any enum type due to System.Enum unification
        /// This is the power of having a common base type for all enums
        /// </summary>
        static void Display(Enum value)
        {
            // At runtime, we can access both the type name and the value
            Console.WriteLine($"  {value.GetType().Name}.{value.ToString()}");
        }

        /// <summary>
        /// Another example of working with System.Enum as a unified type
        /// Shows how you can write utilities that work with any enum
        /// </summary>
        static void ShowEnumInfo(Enum enumValue)
        {
            Type enumType = enumValue.GetType();
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            
            Console.WriteLine($"  Enum: {enumType.Name}, Value: {enumValue}, " +
                            $"Underlying Type: {underlyingType.Name}, " +
                            $"Integral Value: {Convert.ToInt32(enumValue)}");
        }

        /// <summary>
        /// Demonstrates the static utility methods provided by System.Enum
        /// These methods are the real workhorses for enum manipulation
        /// </summary>
        static void DemonstrateSystemEnumUtilityMethods()
        {
            Console.WriteLine("0b. === System.Enum Static Utility Methods ===");
            
            // Method 1: Enum.GetValues() - get all enum members
            Console.WriteLine("GetValues() demonstration:");
            Array nutValues = Enum.GetValues(typeof(Nut));
            foreach (Nut nut in nutValues)
            {
                Console.WriteLine($"  {nut} = {(int)nut}");
            }
            
            // Method 2: Enum.GetNames() - get all enum names as strings
            Console.WriteLine("\nGetNames() demonstration:");
            string[] sizeNames = Enum.GetNames(typeof(Size));
            Console.WriteLine($"  Size enum names: [{string.Join(", ", sizeNames)}]");
            
            // Method 3: Enum.IsDefined() - check if a value is valid
            Console.WriteLine("\nIsDefined() demonstration:");
            Console.WriteLine($"  Is Nut value 1 defined? {Enum.IsDefined(typeof(Nut), 1)}");
            Console.WriteLine($"  Is Nut value 99 defined? {Enum.IsDefined(typeof(Nut), 99)}");
            Console.WriteLine($"  Is 'Medium' a valid Size name? {Enum.IsDefined(typeof(Size), "Medium")}");
            
            // Method 4: Enum.Parse() - convert strings to enum values
            Console.WriteLine("\nParse() demonstration:");
            Nut parsedNut = (Nut)Enum.Parse(typeof(Nut), "Hazelnut");
            Console.WriteLine($"  Parsed 'Hazelnut' to: {parsedNut}");
            
            // Method 5: Enum.ToObject() - convert integral values to enum instances
            Console.WriteLine("\nToObject() demonstration:");
            object nutObject = Enum.ToObject(typeof(Nut), 2);
            Console.WriteLine($"  Integer 2 as Nut: {nutObject}");
            
            // Method 6: Enum.GetUnderlyingType() - get the underlying integral type
            Console.WriteLine("\nGetUnderlyingType() demonstration:");
            Type nutUnderlyingType = Enum.GetUnderlyingType(typeof(Nut));
            Type priorityUnderlyingType = Enum.GetUnderlyingType(typeof(Priority));
            Console.WriteLine($"  Nut underlying type: {nutUnderlyingType.Name}");
            Console.WriteLine($"  Priority underlying type: {priorityUnderlyingType.Name}");
            
            // Method 7: Enum.Format() - format enum values with format specifiers
            Console.WriteLine("\nFormat() demonstration:");
            Size size = Size.Large;
            Console.WriteLine($"  Size.Large formatted as 'G': {Enum.Format(typeof(Size), size, "G")}");
            Console.WriteLine($"  Size.Large formatted as 'D': {Enum.Format(typeof(Size), size, "D")}");
            Console.WriteLine($"  Size.Large formatted as 'X': {Enum.Format(typeof(Size), size, "X")}");
            
            Console.WriteLine();
        }
        #endregion

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
        /// Generic methods for working with any enum type - demonstrates multiple approaches
        /// from the .NET Framework for handling different underlying integral types
        /// </summary>
        static void DemonstrateGenericEnumConversions()
        {
            Console.WriteLine("3. === Generic Enum Conversions ===");
            
            Console.WriteLine("When you need to work with System.Enum instances where the exact");
            Console.WriteLine("enum type might not be known at compile time:\n");
            
            // Approach 1: Simple cast (assumes int underlying type)
            Console.WriteLine("Approach 1: Simple cast to int (risky for non-int enums)");
            try
            {
                Console.WriteLine($"  BorderSides.Top: {GetIntegralValueSimple(BorderSides.Top)}");
                Console.WriteLine($"  Priority.High: {GetIntegralValueSimple(Priority.High)}");
                Console.WriteLine($"  FileSize.Small: {GetIntegralValueSimple(FileSize.Small)}");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"  ✗ Cast error with FileSize: {ex.Message.Split('.')[0]}");
                Console.WriteLine("     (FileSize has 'long' underlying type, can't cast to int)");
            }
            
            // Approach 2: Using Convert.ToDecimal() - safe for all integral types
            Console.WriteLine("\nApproach 2: Convert.ToDecimal() - safe for all integral types");
            Console.WriteLine($"  BorderSides.Top: {GetAnyIntegralValue(BorderSides.Top)}");
            Console.WriteLine($"  Priority.High: {GetAnyIntegralValue(Priority.High)}");
            Console.WriteLine($"  FileSize.Large: {GetAnyIntegralValue(FileSize.Large):N0}");
            
            // Approach 3: Dynamic type detection with Convert.ChangeType()
            Console.WriteLine("\nApproach 3: Dynamic type detection - preserves original type");
            object borderValue = GetBoxedIntegralValue(BorderSides.Top);
            object fileSizeValue = GetBoxedIntegralValue(FileSize.Large);
            
            Console.WriteLine($"  BorderSides.Top: {borderValue} (Type: {borderValue.GetType().Name})");
            Console.WriteLine($"  FileSize.Large: {fileSizeValue:N0} (Type: {fileSizeValue.GetType().Name})");
            
            // Approach 4: Using ToString("D") for string representation
            Console.WriteLine("\nApproach 4: ToString(\"D\") - direct integral value as string");
            Console.WriteLine($"  BorderSides.Top: '{GetIntegralValueAsString(BorderSides.Top)}'");
            Console.WriteLine($"  FileSize.Large: '{GetIntegralValueAsString(FileSize.Large)}'");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Simple cast approach - works but dangerous for non-int underlying types
        /// This would crash if the underlying type is long and the value exceeds int range
        /// </summary>
        static int GetIntegralValueSimple(Enum anyEnum)
        {
            return (int)(object)anyEnum; // This could overflow!
        }
        
        /// <summary>
        /// Safe approach using Convert.ToDecimal() - all integral types can convert to decimal
        /// without loss of information, including ulong values
        /// </summary>
        static decimal GetAnyIntegralValue(Enum anyEnum)
        {
            return Convert.ToDecimal(anyEnum);
        }
        
        /// <summary>
        /// Dynamic approach that determines the enum's underlying type and converts accordingly
        /// This preserves the original integral type without value conversion
        /// </summary>
        static object GetBoxedIntegralValue(Enum anyEnum)
        {
            Type integralType = Enum.GetUnderlyingType(anyEnum.GetType());
            // Note: This doesn't perform value conversion; it re-boxes the same 
            // integral value into the correct type's "clothing"
            return Convert.ChangeType(anyEnum, integralType);
        }
        
        /// <summary>
        /// String representation approach using format specifier
        /// Useful for custom serialization scenarios
        /// </summary>
        static string GetIntegralValueAsString(Enum anyEnum)
        {
            return anyEnum.ToString("D"); // "D" formats as decimal integral value
        }
        #endregion

        #region 4. Enum to String Conversions
        /// <summary>
        /// Comprehensive demonstration of enum to string conversions
        /// Shows both ToString() and Enum.Format() methods with all format specifiers
        /// </summary>
        static void DemonstrateEnumToStringConversions()
        {
            Console.WriteLine("4. === Enum to String Conversions ===");
            
            Console.WriteLine("Enums can be represented as strings in different ways:\n");
            
            BorderSides side = BorderSides.Top;
            Priority priority = Priority.High;
            
            Console.WriteLine("1. Using ToString() with format specifiers:");
            Console.WriteLine($"   Default (no format): {side.ToString()}");
            Console.WriteLine($"   General format (G):  {side.ToString("G")}");    // Same as default
            Console.WriteLine($"   Decimal format (D):  {side.ToString("D")}");    // Underlying integral value
            Console.WriteLine($"   Hex format (X):      {side.ToString("X")}");    // Hexadecimal value
            Console.WriteLine($"   Flags format (F):    {side.ToString("F")}");    // Smart flags display
            
            Console.WriteLine("\n2. Using Enum.Format() static method:");
            Console.WriteLine($"   Enum.Format with 'G': {Enum.Format(typeof(BorderSides), side, "G")}");
            Console.WriteLine($"   Enum.Format with 'D': {Enum.Format(typeof(BorderSides), side, "D")}");
            Console.WriteLine($"   Enum.Format with 'X': {Enum.Format(typeof(BorderSides), side, "X")}");
            Console.WriteLine($"   Enum.Format with 'F': {Enum.Format(typeof(BorderSides), side, "F")}");
            
            // Demonstrate with combined flags - this is where format specifiers really shine
            Console.WriteLine("\n3. Combined flags show the power of different formats:");
            BorderSides combined = BorderSides.Left | BorderSides.Right;
            Console.WriteLine($"   Combined value: Left | Right");
            Console.WriteLine($"   General (G): {combined.ToString("G")}");     // "Left, Right"
            Console.WriteLine($"   Decimal (D): {combined.ToString("D")}");     // "3" (1 + 2)
            Console.WriteLine($"   Hex (X):     {combined.ToString("X")}");     // "00000003"
            Console.WriteLine($"   Flags (F):   {combined.ToString("F")}");     // "Left, Right" (even without [Flags])
            
            Console.WriteLine("\n4. Format behavior differences:");
            Console.WriteLine("   - 'G' and 'F' are similar but 'F' ensures flag combination display");
            Console.WriteLine("   - 'D' gives you the raw integral value for storage/serialization");
            Console.WriteLine("   - 'X' is useful for debugging bit patterns in flags");
            
            // Custom formatting for display purposes
            Console.WriteLine("\n5. Custom formatting for user interfaces:");
            string userFriendlyPriority = FormatPriorityForDisplay(priority);
            Console.WriteLine($"   User-friendly priority: {userFriendlyPriority}");
            
            // Demonstrate with different underlying types
            Console.WriteLine("\n6. Format behavior with different underlying types:");
            FileSize largeFile = FileSize.Large;
            Console.WriteLine($"   FileSize.Large (long): G='{largeFile:G}', D='{largeFile:D}', X='{largeFile:X}'");
            
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
        /// Converting integers back to enum values using various approaches
        /// Demonstrates Enum.ToObject() as the dynamic equivalent of explicit casting
        /// </summary>
        static void DemonstrateIntegralToEnumConversions()
        {
            Console.WriteLine("5. === Integral to Enum Conversions ===");
            
            Console.WriteLine("Two main approaches for converting integers to enums:\n");
            
            // Approach 1: Direct explicit casting (when you know the enum type at compile time)
            Console.WriteLine("1. Direct explicit casting (compile-time known type):");
            int priorityValue = 3;
            Priority priority = (Priority)priorityValue;
            Console.WriteLine($"   (Priority){priorityValue} = {priority}");
            
            int borderValue = 3; // This represents Left | Right (1 + 2)
            BorderSides borderSide = (BorderSides)borderValue;
            Console.WriteLine($"   (BorderSides){borderValue} = {borderSide}");
            
            // Approach 2: Enum.ToObject() for dynamic conversion
            Console.WriteLine("\n2. Enum.ToObject() - dynamic conversion:");
            Console.WriteLine("   This is the dynamic equivalent of explicit casting");
            
            object borderObj = Enum.ToObject(typeof(BorderSides), 3);
            Console.WriteLine($"   Enum.ToObject(typeof(BorderSides), 3) = {borderObj}");
            Console.WriteLine($"   Return type: {borderObj.GetType().Name}");
            
            // Cast back to specific enum type
            BorderSides typedBorder = (BorderSides)borderObj;
            Console.WriteLine($"   Cast back to BorderSides: {typedBorder}");
            
            // ToObject is overloaded for different integral types
            Console.WriteLine("\n3. ToObject() overloads for different integral types:");
            object fromByte = Enum.ToObject(typeof(Priority), (byte)2);
            object fromLong = Enum.ToObject(typeof(FileSize), 1073741824L);
            object fromUInt = Enum.ToObject(typeof(BorderSides), 5u);
            
            Console.WriteLine($"   From byte: {fromByte}");
            Console.WriteLine($"   From long: {fromLong}");
            Console.WriteLine($"   From uint: {fromUInt}");
            
            // Demonstrate equivalence
            Console.WriteLine("\n4. Equivalence demonstration:");
            BorderSides directCast = (BorderSides)3;
            BorderSides viaToObject = (BorderSides)Enum.ToObject(typeof(BorderSides), 3);
            Console.WriteLine($"   Direct cast: {directCast}");
            Console.WriteLine($"   Via ToObject: {viaToObject}");
            Console.WriteLine($"   Are equal: {directCast == viaToObject}");
            
            // Important: No validation occurs!
            Console.WriteLine("\n5. No automatic validation (be careful!):");
            int invalidValue = 999;
            Priority invalidPriority = (Priority)invalidValue;
            object invalidViaToObject = Enum.ToObject(typeof(Priority), invalidValue);
            
            Console.WriteLine($"   Invalid cast (999): {invalidPriority}");
            Console.WriteLine($"   Invalid ToObject (999): {invalidViaToObject}");
            Console.WriteLine($"   Is 999 a defined Priority? {Enum.IsDefined(typeof(Priority), invalidValue)}");
            
            // Safe conversion patterns
            Console.WriteLine("\n6. Safe conversion with validation:");
            if (TryConvertToPriority(2, out Priority safePriority))
            {
                Console.WriteLine($"   ✓ Safe conversion of 2: {safePriority}");
            }
            
            if (!TryConvertToPriority(999, out Priority _))
            {
                Console.WriteLine($"   ✗ Safe conversion rejected 999");
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
        /// Parsing strings back into enum values using Enum.Parse() and TryParse()
        /// Essential for configuration files, user input, and API parameters
        /// </summary>
        static void DemonstrateStringToEnumParsing()
        {
            Console.WriteLine("6. === String to Enum Parsing ===");
            
            Console.WriteLine("Converting string representations back to enum instances:\n");
            
            // Basic parsing with Enum.Parse() - requires Type and string
            Console.WriteLine("1. Enum.Parse() - basic usage:");
            string priorityText = "High";
            Priority parsedPriority = (Priority)Enum.Parse(typeof(Priority), priorityText);
            Console.WriteLine($"   Enum.Parse(typeof(Priority), \"{priorityText}\") = {parsedPriority}");
            
            // Case-insensitive parsing with optional third parameter
            Console.WriteLine("\n2. Case-insensitive parsing:");
            string lowercaseText = "medium";
            Priority caseInsensitivePriority = (Priority)Enum.Parse(typeof(Priority), lowercaseText, true);
            Console.WriteLine($"   Enum.Parse(typeof(Priority), \"{lowercaseText}\", true) = {caseInsensitivePriority}");
            
            // Parsing combined flag values - works with comma-separated names
            Console.WriteLine("\n3. Parsing combined flag values:");
            string combinedText = "Left, Right";
            BorderSides combinedSides = (BorderSides)Enum.Parse(typeof(BorderSides), combinedText);
            Console.WriteLine($"   Enum.Parse(typeof(BorderSides), \"{combinedText}\") = {combinedSides}");
            Console.WriteLine($"   Resulting value: {(int)combinedSides}");
            
            // Alternative flag combination syntax
            string alternativeFormat = "Left,Top"; // No spaces
            BorderSides altCombined = (BorderSides)Enum.Parse(typeof(BorderSides), alternativeFormat);
            Console.WriteLine($"   Enum.Parse(typeof(BorderSides), \"{alternativeFormat}\") = {altCombined}");
            
            // Parsing throws FormatException for invalid input
            Console.WriteLine("\n4. Error handling with Enum.Parse():");
            try
            {
                Priority invalid = (Priority)Enum.Parse(typeof(Priority), "InvalidValue");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"   ✗ FormatException: {ex.Message}");
            }
            
            // Safe parsing with TryParse - much better approach
            Console.WriteLine("\n5. Safe parsing with Enum.TryParse<T>():");
            
            string[] testInputs = { "Critical", "invalid", "LOW", "Left,Top", "", "medium" };
            
            foreach (string input in testInputs)
            {
                if (Enum.TryParse<Priority>(input, true, out Priority result))
                {
                    Console.WriteLine($"   ✓ Successfully parsed '{input}' as Priority: {result}");
                }
                else if (Enum.TryParse<BorderSides>(input, true, out BorderSides borderResult))
                {
                    Console.WriteLine($"   ✓ Successfully parsed '{input}' as BorderSides: {borderResult}");
                }
                else
                {
                    Console.WriteLine($"   ✗ Failed to parse '{input}' as any known enum");
                }
            }
            
            // Generic parsing utility demonstration
            Console.WriteLine("\n6. Generic parsing utility:");
            if (TryParseEnum<Priority>("high", out Priority genericResult))
            {
                Console.WriteLine($"   Generic parse of 'high': {genericResult}");
            }
            
            if (TryParseEnum<BorderSides>("none", out BorderSides borderGeneric))
            {
                Console.WriteLine($"   Generic parse of 'none': {borderGeneric}");
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
        /// Demonstrates Enum.GetValues() and Enum.GetNames() static methods
        /// These methods use reflection and cache results for efficiency
        /// </summary>
        static void DemonstrateEnumeratingEnumValues()
        {
            Console.WriteLine("7. === Enumerating Enum Values ===");
            
            Console.WriteLine("System.Enum provides static methods to retrieve collections of enum members:\n");
            
            // Enum.GetValues() returns an array containing all enum members
            Console.WriteLine("1. Enum.GetValues() - returns all enum instances:");
            Console.WriteLine("   foreach (Enum value in Enum.GetValues(typeof(BorderSides)))");
            
            foreach (Enum value in Enum.GetValues(typeof(BorderSides)))
            {
                Console.WriteLine($"      {value} = {Convert.ToInt32(value)}");
            }
            
            // Modern generic version (C# 7.3+)
            Console.WriteLine("\n2. Generic GetValues<T>() - modern approach:");
            Console.WriteLine("   foreach (Priority priority in Enum.GetValues<Priority>())");
            
            foreach (Priority priority in Enum.GetValues<Priority>())
            {
                Console.WriteLine($"      {priority} = {(int)priority}");
            }
            
            // Enum.GetNames() returns string array of all member names
            Console.WriteLine("\n3. Enum.GetNames() - returns member names as strings:");
            string[] taskStatusNames = Enum.GetNames(typeof(TaskStatus));
            Console.WriteLine($"   TaskStatus names: [{string.Join(", ", taskStatusNames)}]");
            
            // Modern generic version
            Console.WriteLine("\n4. Generic GetNames<T>() approach:");
            string[] borderNames = Enum.GetNames<BorderSides>();
            Console.WriteLine($"   BorderSides names: [{string.Join(", ", borderNames)}]");
            
            // Important note about flags enums and composite values
            Console.WriteLine("\n5. Behavior with [Flags] enums:");
            Console.WriteLine("   GetValues() returns ALL members, including composite ones:");
            
            foreach (DaysOfWeek day in Enum.GetValues<DaysOfWeek>())
            {
                bool isComposite = !IsPowerOfTwo((int)day) && day != DaysOfWeek.None;
                string type = isComposite ? "(composite)" : "(individual)";
                Console.WriteLine($"      {day} = {(int)day} {type}");
            }
            
            // Practical applications
            Console.WriteLine("\n6. Practical applications:");
            
            // Building dropdown/select options
            Console.WriteLine("   Building UI dropdown data:");
            var priorityOptions = BuildDropdownOptions<Priority>();
            foreach (var option in priorityOptions.Take(3))
            {
                Console.WriteLine($"      Value: {option.Value}, Display: {option.Text}");
            }
            
            // Validation and counting
            int priorityCount = Enum.GetValues<Priority>().Length;
            Console.WriteLine($"\n   Total Priority enum members: {priorityCount}");
            
            // Using IsDefined for validation
            Console.WriteLine($"   Is Priority value 3 defined? {Enum.IsDefined(typeof(Priority), 3)}");
            Console.WriteLine($"   Is Priority value 99 defined? {Enum.IsDefined(typeof(Priority), 99)}");
            Console.WriteLine($"   Is 'High' a valid Priority name? {Enum.IsDefined(typeof(Priority), "High")}");
            
            // Performance note
            Console.WriteLine("\n7. Performance consideration:");
            Console.WriteLine("   These methods use reflection and cache results for efficiency");
            Console.WriteLine("   First call is slower, subsequent calls use cached data");
            
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
        /// Demonstrates the key limitations of enums and their runtime behavior
        /// Shows why enums provide static type safety but not strong runtime type safety
        /// </summary>
        static void DemonstrateEnumLimitations()
        {
            Console.WriteLine("14. === Enum Limitations and Runtime Behavior ===");
            
            Console.WriteLine("Understanding enum limitations is crucial for writing robust code:\n");
            
            // Key insight: Type safety is enforced by compiler, not CLR
            Console.WriteLine("1. Static vs Strong Type Safety:");
            Console.WriteLine("   Enums provide STATIC type safety (compile-time) but not STRONG type safety (runtime)");
            
            BorderSides side = BorderSides.Left;
            Console.WriteLine($"   Initial side: {side} (value: {(int)side})");
            
            // This compiles and runs - compiler allows arithmetic on underlying type
            Console.WriteLine("\n2. Arithmetic operations that probably don't make sense:");
            side += 1234;  // No compile-time or runtime error!
            Console.WriteLine($"   After side += 1234: {side} (value: {(int)side})");
            Console.WriteLine($"   Is this a defined BorderSides? {Enum.IsDefined(typeof(BorderSides), side)}");
            Console.WriteLine($"   ToString() still works: '{side.ToString()}'");
            
            // CLR doesn't validate enum values
            Console.WriteLine("\n3. CLR doesn't validate enum values:");
            Priority invalidPriority = (Priority)999;
            Console.WriteLine($"   (Priority)999 = {invalidPriority}");
            Console.WriteLine($"   Type: {invalidPriority.GetType().Name}");
            Console.WriteLine($"   ToString(): '{invalidPriority.ToString()}'");
            Console.WriteLine($"   GetHashCode(): {invalidPriority.GetHashCode()}");
            
            // Enum values are just integers at runtime
            Console.WriteLine("\n4. Runtime perspective - enums are just integers:");
            Console.WriteLine("   When unboxed, CLR treats enum identically to its underlying integral value");
            
            int intValue = 5;
            BorderSides enumValue = (BorderSides)intValue;
            Console.WriteLine($"   int value: {intValue}");
            Console.WriteLine($"   enum value: {enumValue}");
            Console.WriteLine($"   (int)enumValue: {(int)enumValue}");
            Console.WriteLine($"   Are they equal? {intValue == (int)enumValue}");
            
            // Flags can have unexpected combinations
            Console.WriteLine("\n5. Flags can have unexpected combinations:");
            DaysOfWeek invalidDays = (DaysOfWeek)255; // All bits set
            Console.WriteLine($"   (DaysOfWeek)255 = {invalidDays}");
            Console.WriteLine($"   This represents: {Convert.ToString(255, 2).PadLeft(8, '0')} in binary");
            
            // Comparison operations can be misleading
            Console.WriteLine("\n6. Comparison operations might not make logical sense:");
            Priority p1 = Priority.Medium;
            Priority p2 = Priority.High;
            Priority result = (Priority)((int)p1 + (int)p2);
            
            Console.WriteLine($"   Priority.Medium + Priority.High = {result} (value: {(int)result})");
            Console.WriteLine($"   Does this make logical sense? Probably not!");
            
            // Best practices for handling limitations
            Console.WriteLine("\n7. Best practices for robust enum handling:");
            
            // Always validate external input
            Console.WriteLine("   a) Always validate when converting from external sources:");
            if (TryParseEnum<Priority>("Medium", out Priority validPriority))
            {
                Console.WriteLine($"      ✓ Valid priority: {validPriority}");
            }
            
            // Use IsDefined for validation
            Console.WriteLine("   b) Use IsDefined for validation:");
            int userInput = 2;
            if (Enum.IsDefined(typeof(Priority), userInput))
            {
                Priority safePriority = (Priority)userInput;
                Console.WriteLine($"      ✓ Valid priority from input: {safePriority}");
            }
            
            // For flags, validate against known combinations
            Console.WriteLine("   c) For flags, validate against known combinations:");
            DaysOfWeek userDays = DaysOfWeek.Monday | DaysOfWeek.Wednesday;
            if (IsValidDaysCombination(userDays))
            {
                Console.WriteLine($"      ✓ Valid days combination: {userDays}");
            }
            
            // Avoid arithmetic operations
            Console.WriteLine("   d) Avoid arithmetic operations on enum values:");
            Console.WriteLine("      Use explicit comparisons and logical operations instead");
            
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

        #region How Enums Work Under the Hood
        /// <summary>
        /// Demonstrates the runtime behavior of enums and how the CLR treats them
        /// Shows the difference between static type safety and runtime behavior
        /// </summary>
        static void DemonstrateEnumUnderTheHood()
        {
            Console.WriteLine("12. === How Enums Work Under the Hood ===");
            
            Console.WriteLine("Understanding enum behavior at the CLR level:\n");
            
            // Static type safety vs runtime behavior
            Console.WriteLine("1. Static Type Safety (enforced by compiler):");
            BorderSides validSide = BorderSides.Left;
            Console.WriteLine($"   Valid assignment: {validSide}");
            
            // The following would cause compile error (uncomment to see):
            // BorderSides invalidAssignment = 999; // Compiler error
            
            Console.WriteLine("\n2. Runtime Behavior (CLR treats enum as integral value):");
            BorderSides runtimeSide = BorderSides.Left;
            
            // This compiles and runs - no runtime validation!
            runtimeSide += 1234;
            Console.WriteLine($"   After adding 1234: {runtimeSide} (value: {(int)runtimeSide})");
            Console.WriteLine($"   Is this a valid BorderSides? {Enum.IsDefined(typeof(BorderSides), runtimeSide)}");
            
            // CLR doesn't care about enum semantics - it's just an int
            Console.WriteLine("\n3. CLR perspective - enums are just integers:");
            int intValue = 5;
            BorderSides fromInt = (BorderSides)intValue;
            Console.WriteLine($"   Integer 5 as BorderSides: {fromInt}");
            Console.WriteLine($"   ToString() still works: '{fromInt.ToString()}'");
            
            // Demonstrate that enum definitions are essentially static fields
            Console.WriteLine("\n4. Enum definitions at runtime:");
            Type borderType = typeof(BorderSides);
            Console.WriteLine($"   BorderSides base type: {borderType.BaseType?.Name}");
            Console.WriteLine($"   Is value type: {borderType.IsValueType}");
            Console.WriteLine($"   Is enum: {borderType.IsEnum}");
            Console.WriteLine($"   Underlying type: {Enum.GetUnderlyingType(borderType).Name}");
            
            // Show how performance mirrors integral constants
            Console.WriteLine("\n5. Performance characteristics:");
            MeasureEnumPerformance();
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Simple performance demonstration showing enums behave like integers
        /// </summary>
        static void MeasureEnumPerformance()
        {
            const int iterations = 1000000;
            
            // Enum operations
            var sw = System.Diagnostics.Stopwatch.StartNew();
            BorderSides side = BorderSides.Left;
            for (int i = 0; i < iterations; i++)
            {
                side = (BorderSides)((int)side + 1);
            }
            sw.Stop();
            Console.WriteLine($"   Enum operations: {sw.ElapsedMilliseconds}ms for {iterations:N0} operations");
            
            // Equivalent int operations
            sw.Restart();
            int intSide = 1;
            for (int i = 0; i < iterations; i++)
            {
                intSide = intSide + 1;
            }
            sw.Stop();
            Console.WriteLine($"   Int operations:  {sw.ElapsedMilliseconds}ms for {iterations:N0} operations");
            Console.WriteLine("   (Performance is nearly identical)");
        }
        
        /// <summary>
        /// Demonstrates the boxing behavior that enables ToString() and GetType() on enums
        /// This explains why ToString() returns "Right" instead of "2"
        /// </summary>
        static void DemonstrateBoxingBehavior()
        {
            Console.WriteLine("13. === Boxing Behavior and Virtual Method Calls ===");
            
            Console.WriteLine("Why does BorderSides.Right.ToString() print 'Right' instead of '2'?\n");
            
            BorderSides side = BorderSides.Right;
            
            Console.WriteLine("1. When calling virtual methods, C# implicitly boxes the enum:");
            Console.WriteLine($"   side.ToString(): '{side.ToString()}'  // Boxing occurs here");
            Console.WriteLine($"   side.GetType().Name: '{side.GetType().Name}'  // And here");
            
            Console.WriteLine("\n2. Boxing wraps the integral value with type information:");
            object boxedSide = side; // Explicit boxing
            Console.WriteLine($"   Boxed enum: {boxedSide}");
            Console.WriteLine($"   Boxed type: {boxedSide.GetType().FullName}");
            Console.WriteLine($"   Underlying value: {(int)(BorderSides)boxedSide}");
            
            Console.WriteLine("\n3. Without boxing, you get the raw integral value:");
            Console.WriteLine($"   ((int)side).ToString(): '{((int)side).ToString()}'");
            Console.WriteLine($"   ((int)side).GetType().Name: '{((int)side).GetType().Name}'");
            
            Console.WriteLine("\n4. Comparison of boxed vs unboxed behavior:");
            DemonstrateBoxedVsUnboxed(BorderSides.Left | BorderSides.Right);
            
            Console.WriteLine("\n5. This is compiler magic, not CLR behavior:");
            Console.WriteLine("   The CLR doesn't know about enum semantics");
            Console.WriteLine("   C# compiler inserts boxing when needed for virtual calls");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Helper method to show the difference between boxed and unboxed enum behavior
        /// </summary>
        static void DemonstrateBoxedVsUnboxed(BorderSides sides)
        {
            int integralValue = (int)sides;
            object boxedEnum = sides;
            
            Console.WriteLine($"   Original enum: {sides}");
            Console.WriteLine($"   As integer: {integralValue}");
            Console.WriteLine($"   Boxed enum: {boxedEnum}");
            Console.WriteLine($"   boxedEnum.ToString(): '{boxedEnum.ToString()}'");
            Console.WriteLine($"   integralValue.ToString(): '{integralValue.ToString()}'");
        }
        #endregion
    }
}
