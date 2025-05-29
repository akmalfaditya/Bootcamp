using System;

namespace Enums
{
    /// <summary>
    /// Welcome to C# Enums bootcamp!
    /// Think of enums as a way to give meaningful names to numbers
    /// Instead of remembering "status 1 = pending, 2 = approved", just use Status.Pending!
    /// They make your code self-documenting and way less error-prone
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Enums: Named Constants Done Right ===\n");

            // Let's explore enums from basic concepts to advanced patterns
            DemonstrateBasicEnums();
            DemonstrateCustomUnderlyingTypes();
            DemonstrateExplicitValues();
            DemonstrateEnumConversions();
            DemonstrateFlagsEnums();
            DemonstrateEnumOperators();
            DemonstrateTypeSafetyIssues();
            DemonstrateRealWorldScenarios();
            DemonstrateAdvancedTechniques();
            DemonstrateBestPractices();

            Console.WriteLine("\n=== Enums Demo Complete! ===");
            Console.ReadKey();
        }

        /// <summary>
        /// Basic enums are like having a dropdown list in your code
        /// Instead of magic numbers, you get meaningful names
        /// The compiler assigns 0, 1, 2, 3... automatically
        /// </summary>
        static void DemonstrateBasicEnums()
        {
            Console.WriteLine("1. Basic Enums - No More Magic Numbers:");
            
            // Create enum variables - much clearer than using numbers!
            BorderSide leftSide = BorderSide.Left;
            BorderSide topSide = BorderSide.Top;
            
            Console.WriteLine($"Selected sides: {leftSide} and {topSide}");
            Console.WriteLine($"Left side value: {(int)leftSide}");  // Shows underlying value (0)
            Console.WriteLine($"Top side value: {(int)topSide}");    // Shows underlying value (2)
            
            // Enum comparison - super readable code
            if (leftSide == BorderSide.Left)
            {
                Console.WriteLine("✅ Yep, it's the left side!");
            }
            
            // Loop through all enum values - handy for UI dropdowns
            Console.WriteLine("\nAll border sides:");
            foreach (BorderSide side in Enum.GetValues<BorderSide>())
            {
                Console.WriteLine($"- {side} (value: {(int)side})");
            }
            
            Console.WriteLine("\n✨ See how much clearer this is than using 0, 1, 2, 3?");
            Console.WriteLine("✅ Self-documenting code that's hard to mess up");
            Console.WriteLine();
        }

        /// <summary>
        /// Sometimes you need to save memory or match external system requirements
        /// You can choose the underlying type: byte, short, int, long, etc.
        /// Default is int, but smaller types can save memory in large arrays
        /// </summary>
        static void DemonstrateCustomUnderlyingTypes()
        {
            Console.WriteLine("2. Custom Underlying Types - Memory Optimization:");
            
            // FilePermission uses byte - perfect for file system flags
            FilePermission readPerm = FilePermission.Read;
            FilePermission writePerm = FilePermission.Write;
            
            Console.WriteLine($"File permissions using byte enum:");
            Console.WriteLine($"Read: {readPerm} (value: {(byte)readPerm})");
            Console.WriteLine($"Write: {writePerm} (value: {(byte)writePerm})");
            
            // HttpStatus uses short - enough for all HTTP status codes
            HttpStatus okStatus = HttpStatus.OK;
            HttpStatus notFoundStatus = HttpStatus.NotFound;
            
            Console.WriteLine($"\nHTTP statuses using short enum:");
            Console.WriteLine($"OK: {okStatus} (value: {(short)okStatus})");
            Console.WriteLine($"Not Found: {notFoundStatus} (value: {(short)notFoundStatus})");
            
            // Show memory usage difference
            Console.WriteLine($"\nMemory usage:");
            Console.WriteLine($"- Default int enum: 4 bytes per value");
            Console.WriteLine($"- Byte enum: 1 byte per value (75% memory savings!)");
            Console.WriteLine($"- Short enum: 2 bytes per value (50% memory savings!)");
            
            Console.WriteLine("\n✨ Choose the right size for your data!");
            Console.WriteLine("✅ Small enums = less memory, especially in arrays");
            Console.WriteLine();
        }

        /// <summary>
        /// Sometimes you need specific values - maybe to match a database or API
        /// You can assign explicit values and let the rest auto-increment
        /// Great for maintaining backward compatibility
        /// </summary>
        static void DemonstrateExplicitValues()
        {
            Console.WriteLine("3. Explicit Values - Taking Control:");
            
            // Priority enum with explicit values - common in project management
            Priority lowPriority = Priority.Low;
            Priority highPriority = Priority.High;
            Priority criticalPriority = Priority.Critical;
            
            Console.WriteLine("Project priorities with explicit values:");
            Console.WriteLine($"Low: {lowPriority} (value: {(int)lowPriority})");
            Console.WriteLine($"High: {highPriority} (value: {(int)highPriority})");
            Console.WriteLine($"Critical: {criticalPriority} (value: {(int)criticalPriority})");
            
            // LogLevel with gaps - room for future levels
            LogLevel infoLevel = LogLevel.Info;
            LogLevel warningLevel = LogLevel.Warning;
            LogLevel errorLevel = LogLevel.Error;
            
            Console.WriteLine("\nLog levels with gaps for future expansion:");
            Console.WriteLine($"Info: {infoLevel} (value: {(int)infoLevel})");
            Console.WriteLine($"Warning: {warningLevel} (value: {(int)warningLevel})");
            Console.WriteLine($"Error: {errorLevel} (value: {(int)errorLevel})");
            
            // Show enum values in order
            Console.WriteLine("\nAll log levels in order:");
            var logLevels = Enum.GetValues<LogLevel>().OrderBy(x => (int)x);
            foreach (LogLevel level in logLevels)
            {
                Console.WriteLine($"- {level} = {(int)level}");
            }
            
            Console.WriteLine("\n✨ Explicit values give you total control!");
            Console.WriteLine("✅ Perfect for database IDs or API constants");
            Console.WriteLine("✅ Leave gaps for future expansion");
            Console.WriteLine();
        }

        /// <summary>
        /// Enums can be converted to and from their underlying values
        /// This is essential when working with databases, APIs, or configuration files
        /// But be careful - you can cast invalid values!
        /// </summary>
        static void DemonstrateEnumConversions()
        {
            Console.WriteLine("4. Enum Conversions - Moving Between Types:");
            
            // Convert enum to underlying value
            OrderStatus pendingStatus = OrderStatus.Pending;
            int statusValue = (int)pendingStatus;
            
            Console.WriteLine($"Order status: {pendingStatus}");
            Console.WriteLine($"As integer: {statusValue}");
            
            // Convert back from underlying value
            OrderStatus restoredStatus = (OrderStatus)statusValue;
            Console.WriteLine($"Restored from int: {restoredStatus}");
            
            // Parse from string - useful for configuration files
            string statusName = "Shipped";
            if (Enum.TryParse<OrderStatus>(statusName, out OrderStatus parsedStatus))
            {
                Console.WriteLine($"Parsed '{statusName}' to: {parsedStatus}");
            }
            
            // Convert between compatible enums (same underlying values)
            BorderSide rightBorder = BorderSide.Right;
            HorizontalAlignment rightAlign = (HorizontalAlignment)rightBorder;
            Console.WriteLine($"Border.Right ({(int)rightBorder}) → HorizontalAlignment: {rightAlign}");
            
            // Demonstrate the danger of invalid casts
            Console.WriteLine("\n⚠️  Watch out for invalid conversions:");
            OrderStatus invalidStatus = (OrderStatus)999;
            Console.WriteLine($"Invalid cast result: {invalidStatus} (value: 999)");
            
            // Safe way to check validity
            bool isValidStatus = Enum.IsDefined(typeof(OrderStatus), invalidStatus);
            Console.WriteLine($"Is 999 a valid OrderStatus? {isValidStatus}");
            
            Console.WriteLine("\n✨ Conversions are powerful but need validation!");
            Console.WriteLine("✅ Always validate when converting from external data");
            Console.WriteLine();
        }

        /// <summary>
        /// Flags enums are like checkboxes - you can select multiple options
        /// Each value must be a power of 2 so they can be combined with bitwise operations
        /// Perfect for permissions, configuration options, or feature flags
        /// </summary>
        static void DemonstrateFlagsEnums()
        {
            Console.WriteLine("5. Flags Enums - Multiple Choice Power:");
            
            // Single permission
            FilePermissions readOnly = FilePermissions.Read;
            Console.WriteLine($"Read-only permission: {readOnly}");
            
            // Combine multiple permissions using OR
            FilePermissions readWrite = FilePermissions.Read | FilePermissions.Write;
            Console.WriteLine($"Read-write permission: {readWrite}");
            
            // All permissions
            FilePermissions fullAccess = FilePermissions.Read | FilePermissions.Write | FilePermissions.Execute;
            Console.WriteLine($"Full access: {fullAccess}");
            
            // Check if specific permission is included using AND
            bool canRead = (fullAccess & FilePermissions.Read) != 0;
            bool canDelete = (fullAccess & FilePermissions.Delete) != 0;
            
            Console.WriteLine($"Can read? {canRead}");
            Console.WriteLine($"Can delete? {canDelete}");
            
            // Remove a permission using XOR
            FilePermissions noExecute = fullAccess ^ FilePermissions.Execute;
            Console.WriteLine($"Remove execute permission: {noExecute}");
            
            // Feature flags example - perfect for A/B testing!
            FeatureFlags currentFeatures = FeatureFlags.DarkMode | FeatureFlags.PushNotifications;
            Console.WriteLine($"\nCurrent features enabled: {currentFeatures}");
            
            // Check individual features
            CheckFeature(currentFeatures, FeatureFlags.DarkMode);
            CheckFeature(currentFeatures, FeatureFlags.AdvancedSearch);
            CheckFeature(currentFeatures, FeatureFlags.PushNotifications);
            
            Console.WriteLine("\n✨ Flags enums are perfect for combinations!");
            Console.WriteLine("✅ Permissions, features, configuration options");
            Console.WriteLine("✅ Efficient storage using bitwise operations");
            Console.WriteLine();
        }

        static void CheckFeature(FeatureFlags enabled, FeatureFlags feature)
        {
            bool hasFeature = (enabled & feature) != 0;
            string status = hasFeature ? "✅ Enabled" : "❌ Disabled";
            Console.WriteLine($"  {feature}: {status}");
        }

        /// <summary>
        /// Enums support comparison, arithmetic, and bitwise operators
        /// You can compare them, do math with them, and combine them
        /// This makes them incredibly flexible for different scenarios
        /// </summary>
        static void DemonstrateEnumOperators()
        {
            Console.WriteLine("6. Enum Operators - Math and Logic:");
            
            // Comparison operators - enums are ordered by their values
            Priority mediumPriority = Priority.Medium;
            Priority highPriority = Priority.High;
            
            Console.WriteLine("Priority comparisons:");
            Console.WriteLine($"Medium < High: {mediumPriority < highPriority}");
            Console.WriteLine($"High >= Medium: {highPriority >= mediumPriority}");
            
            // Arithmetic with enums - useful for incrementing/decrementing
            LogLevel currentLevel = LogLevel.Warning;
            LogLevel nextLevel = currentLevel + 10; // Add to underlying value
            
            Console.WriteLine($"\nLog level arithmetic:");
            Console.WriteLine($"Current: {currentLevel} ({(int)currentLevel})");
            Console.WriteLine($"Next: {nextLevel} ({(int)nextLevel})");
            
            // Bitwise operations with flags (we saw some earlier)
            FilePermissions perm1 = FilePermissions.Read | FilePermissions.Write;
            FilePermissions perm2 = FilePermissions.Write | FilePermissions.Execute;
            
            // Union (OR) - all permissions from both
            FilePermissions combined = perm1 | perm2;
            Console.WriteLine($"\nBitwise operations:");
            Console.WriteLine($"Perm1: {perm1}");
            Console.WriteLine($"Perm2: {perm2}");
            Console.WriteLine($"Combined: {combined}");
            
            // Intersection (AND) - only common permissions
            FilePermissions common = perm1 & perm2;
            Console.WriteLine($"Common: {common}");
            
            // Difference (XOR) - permissions in either but not both
            FilePermissions different = perm1 ^ perm2;
            Console.WriteLine($"Different: {different}");
            
            Console.WriteLine("\n✨ Enums work with all the operators you'd expect!");
            Console.WriteLine("✅ Compare, calculate, and combine with ease");
            Console.WriteLine();
        }

        /// <summary>
        /// Enums aren't bulletproof - you can cast invalid values
        /// This is a common source of bugs, so let's learn to handle it properly
        /// Always validate enum values coming from external sources!
        /// </summary>
        static void DemonstrateTypeSafetyIssues()
        {
            Console.WriteLine("7. Type Safety Issues - The Dark Side:");
            
            // The danger: casting invalid values
            Console.WriteLine("⚠️  Demonstration of enum safety issues:");
            
            // This compiles but creates an invalid enum value!
            OrderStatus invalidStatus = (OrderStatus)999;
            Console.WriteLine($"Invalid status: {invalidStatus} (shows as number: 999)");
            
            // This could crash your app if you're not careful
            try
            {
                ProcessOrder(invalidStatus);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Caught error: {ex.Message}");
            }
            
            // Safe validation using Enum.IsDefined
            Console.WriteLine("\n✅ Safe validation techniques:");
            
            int userInput = 2; // Imagine this comes from user or database
            if (Enum.IsDefined(typeof(OrderStatus), userInput))
            {
                OrderStatus safeStatus = (OrderStatus)userInput;
                Console.WriteLine($"Valid status: {safeStatus}");
            }
            else
            {
                Console.WriteLine($"Invalid status value: {userInput}");
            }
            
            // Parse from string safely
            string statusText = "InvalidStatus";
            if (Enum.TryParse<OrderStatus>(statusText, out OrderStatus parsedStatus))
            {
                Console.WriteLine($"Parsed: {parsedStatus}");
            }
            else
            {
                Console.WriteLine($"'{statusText}' is not a valid OrderStatus");
            }
            
            // Flags enum validation is trickier
            Console.WriteLine("\n🏴 Flags enum validation:");
            FilePermissions suspiciousPerms = (FilePermissions)1023; // All bits set
            
            if (IsFlagsValueValid(suspiciousPerms))
            {
                Console.WriteLine($"Valid flags combination: {suspiciousPerms}");
            }
            else
            {
                Console.WriteLine($"Invalid flags combination: {suspiciousPerms}");
            }
            
            Console.WriteLine("\n✨ Always validate enums from external sources!");
            Console.WriteLine("✅ Use Enum.IsDefined() for regular enums");
            Console.WriteLine("✅ Custom validation for flags enums");
            Console.WriteLine();
        }

        static void ProcessOrder(OrderStatus status)
        {
            // Validate enum in business logic methods
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                throw new ArgumentException($"Invalid order status: {status}");
            }
            
            Console.WriteLine($"Processing order with status: {status}");
        }

        static bool IsFlagsValueValid<T>(T flagsValue) where T : Enum
        {
            // For flags enums, check if the string representation contains only letters/commas
            // If it contains numbers, it means there are undefined bits set
            string stringValue = flagsValue.ToString();
            return !stringValue.Any(char.IsDigit);
        }

        /// <summary>
        /// Real-world scenarios where enums shine
        /// These are patterns you'll use in actual projects
        /// From simple state machines to complex configuration systems
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("8. Real-World Scenarios - Enums in Action:");
            
            // E-commerce order processing
            Console.WriteLine("📦 E-commerce Order Processing:");
            var order = new Order(1001);
            
            order.UpdateStatus(OrderStatus.Pending);
            order.UpdateStatus(OrderStatus.Processing);
            order.UpdateStatus(OrderStatus.Shipped);
            order.UpdateStatus(OrderStatus.Delivered);
            
            // User account management
            Console.WriteLine("\n👤 User Account Management:");
            var user = new UserAccount("john.doe@example.com");
            
            user.SetRole(UserRole.Member);
            user.SetPermissions(AccountPermissions.ViewProfile | AccountPermissions.EditProfile);
            user.DisplayInfo();
            
            // Game development example
            Console.WriteLine("\n🎮 Game Development:");
            var player = new GameCharacter("Hero");
            
            player.SetDirection(Direction.North);
            player.Move();
            player.SetDirection(Direction.East);
            player.Move();
            
            // Configuration system
            Console.WriteLine("\n⚙️ Application Configuration:");
            var config = new AppConfiguration();
            
            config.SetLogLevel(LogLevel.Warning);
            config.EnableFeatures(FeatureFlags.DarkMode | FeatureFlags.PushNotifications);
            config.DisplaySettings();
            
            Console.WriteLine("\n✨ Enums make complex systems simple and safe!");
            Console.WriteLine("✅ State management, permissions, configuration");
            Console.WriteLine("✅ Self-documenting code that's hard to break");
            Console.WriteLine();
        }

        /// <summary>
        /// Advanced enum techniques for power users
        /// Extensions methods, custom parsing, enum utilities
        /// These techniques will make you an enum ninja!
        /// </summary>
        static void DemonstrateAdvancedTechniques()
        {
            Console.WriteLine("9. Advanced Techniques - Enum Mastery:");
            
            // Extension methods make enums more powerful
            Console.WriteLine("🔧 Extension Methods:");
            
            OrderStatus status = OrderStatus.Processing;
            Console.WriteLine($"Status: {status}");
            Console.WriteLine($"Description: {status.GetDescription()}");
            Console.WriteLine($"Can cancel? {status.CanBeCancelled()}");
            Console.WriteLine($"Color: {status.GetDisplayColor()}");
            
            // Enum utilities for reflection-based operations
            Console.WriteLine("\n🔍 Enum Utilities:");
            
            var allStatuses = EnumUtils.GetAllValues<OrderStatus>();
            Console.WriteLine($"All order statuses: {string.Join(", ", allStatuses)}");
            
            var statusCount = EnumUtils.GetCount<OrderStatus>();
            Console.WriteLine($"Total status count: {statusCount}");
            
            // Advanced flags operations
            Console.WriteLine("\n🏴 Advanced Flags Operations:");
            
            var permissions = FilePermissions.Read | FilePermissions.Write;
            Console.WriteLine($"Original permissions: {permissions}");
            
            permissions = permissions.AddFlag(FilePermissions.Execute);
            Console.WriteLine($"After adding Execute: {permissions}");
            
            permissions = permissions.RemoveFlag(FilePermissions.Write);
            Console.WriteLine($"After removing Write: {permissions}");
            
            bool hasRead = permissions.HasFlag(FilePermissions.Read);
            Console.WriteLine($"Has Read permission: {hasRead}");
            
            // Enum parsing with fallback
            Console.WriteLine("\n📝 Safe Parsing:");
            
            string input = "InvalidStatus";
            OrderStatus fallbackStatus = OrderStatus.Pending;
            OrderStatus result = EnumUtils.ParseWithFallback(input, fallbackStatus);
            Console.WriteLine($"Parsed '{input}' with fallback: {result}");
            
            Console.WriteLine("\n✨ Advanced techniques unlock enum superpowers!");
            Console.WriteLine("✅ Extension methods for rich functionality");
            Console.WriteLine("✅ Utility classes for common operations");
            Console.WriteLine();
        }

        /// <summary>
        /// Best practices learned from years of enum usage
        /// Follow these guidelines and your enums will be maintainable and robust
        /// </summary>
        static void DemonstrateBestPractices()
        {
            Console.WriteLine("10. Best Practices - Enum Wisdom:");
            
            Console.WriteLine("📋 Naming Conventions:");
            Console.WriteLine("✅ Use PascalCase for enum types: OrderStatus, FilePermissions");
            Console.WriteLine("✅ Use singular names unless flags: Direction (not Directions)");
            Console.WriteLine("✅ Use plural for flags: FilePermissions, FeatureFlags");
            Console.WriteLine("✅ Add [Flags] attribute for combinable enums");
            
            Console.WriteLine("\n🎯 Design Guidelines:");
            Console.WriteLine("✅ Start with 0 for default/none values");
            Console.WriteLine("✅ Use powers of 2 for flags (1, 2, 4, 8, 16...)");
            Console.WriteLine("✅ Leave gaps in values for future expansion");
            Console.WriteLine("✅ Document what each value means");
            
            Console.WriteLine("\n⚡ Performance Tips:");
            Console.WriteLine("✅ Prefer smaller underlying types when possible");
            Console.WriteLine("✅ Use Enum.HasFlag() for flags checking (it's optimized)");
            Console.WriteLine("✅ Cache Enum.GetValues() results if called frequently");
            Console.WriteLine("✅ Avoid Enum.IsDefined() in hot paths (it's slow)");
            
            Console.WriteLine("\n🛡️ Safety Guidelines:");
            Console.WriteLine("✅ Always validate enums from external sources");
            Console.WriteLine("✅ Use TryParse() instead of Parse() for strings");
            Console.WriteLine("✅ Handle unknown enum values gracefully");
            Console.WriteLine("✅ Don't rely on specific underlying values in business logic");
            
            Console.WriteLine("\n🔄 When to Use Enums:");
            Console.WriteLine("✅ Fixed set of related constants");
            Console.WriteLine("✅ State machines and status tracking");
            Console.WriteLine("✅ Configuration options and flags");
            Console.WriteLine("✅ API response codes and error types");
            
            Console.WriteLine("\n❌ When NOT to Use Enums:");
            Console.WriteLine("❌ Values that change frequently");
            Console.WriteLine("❌ Large sets of data (use lookup tables)");
            Console.WriteLine("❌ String constants (use static readonly)");
            Console.WriteLine("❌ Complex relationships (use classes)");
            
            Console.WriteLine("\n🎪 Golden Rules:");
            Console.WriteLine("1. Make invalid states unrepresentable");
            Console.WriteLine("2. Use meaningful names that tell a story");
            Console.WriteLine("3. Validate external data always");
            Console.WriteLine("4. Document the business meaning, not just the code");
            Console.WriteLine();
        }
    }
}
