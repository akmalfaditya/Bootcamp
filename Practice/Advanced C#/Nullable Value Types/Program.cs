using System;

namespace NullableValueTypesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Comprehensive Guide to Nullable Value Types in C# ===\n");

            // Welcome message - let's explore why nullable value types exist
            Console.WriteLine("Welcome to our deep dive into nullable value types!");
            Console.WriteLine("We'll cover everything from basic concepts to advanced scenarios.\n");

            // Let's start with the fundamentals and work our way up
            DemonstrateTheBasicProblem();
            BasicNullableTypesDemo();
            ExploreNullableStructInternals();
            ImplicitExplicitConversionsDemo();
            BoxingUnboxingDemo();
            OperatorLiftingDemo();
            EqualityOperatorsDemo();
            RelationalOperatorsDemo();
            ArithmeticOperatorsDemo();
            BooleanLogicalOperatorsDemo();
            MixingNullableNonNullableDemo();
            NullCoalescingOperatorDemo();
            CompareWithMagicValues();
            RealWorldScenarioDemo();

            Console.WriteLine("\nThat's a wrap! You now understand nullable value types completely.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region Basic Nullable Types

        static void BasicNullableTypesDemo()
        {
            Console.WriteLine("2. BASIC NULLABLE TYPES IN ACTION");
            Console.WriteLine("==================================");

            // Here's the magic - adding ? to any value type makes it nullable
            Console.WriteLine("The ? syntax is your gateway to nullable value types:");

            // Regular value types have default values, never null
            int regularInt = default;  // 0
            bool regularBool = default;  // false
            DateTime regularDate = default;  // 1/1/0001 12:00:00 AM
            
            Console.WriteLine($"Regular int default: {regularInt}");
            Console.WriteLine($"Regular bool default: {regularBool}");
            Console.WriteLine($"Regular DateTime default: {regularDate}");

            Console.WriteLine("\nNow watch what happens when we add the magic '?' symbol:");

            // Nullable value types CAN represent the absence of a value
            int? nullableInt = null;
            bool? nullableBool = null;
            DateTime? nullableDate = null;

            Console.WriteLine($"Nullable int: {nullableInt?.ToString() ?? "null"}");
            Console.WriteLine($"Nullable bool: {nullableBool?.ToString() ?? "null"}");
            Console.WriteLine($"Nullable DateTime: {nullableDate?.ToString() ?? "null"}");

            // Let's verify they're actually null
            Console.WriteLine($"\nVerifying null status:");
            Console.WriteLine($"nullableInt == null: {nullableInt == null}");
            Console.WriteLine($"nullableBool == null: {nullableBool == null}");
            Console.WriteLine($"nullableDate == null: {nullableDate == null}");

            // Now let's assign some actual values and see the difference
            nullableInt = 42;
            nullableBool = true;
            nullableDate = new DateTime(2024, 12, 25);

            Console.WriteLine($"\nAfter assigning real values:");
            Console.WriteLine($"Nullable int: {nullableInt}");
            Console.WriteLine($"Nullable bool: {nullableBool}");
            Console.WriteLine($"Nullable DateTime: {nullableDate}");

            // They're no longer null
            Console.WriteLine($"\nNull status after assignment:");
            Console.WriteLine($"nullableInt == null: {nullableInt == null}");
            Console.WriteLine($"nullableBool == null: {nullableBool == null}");
            Console.WriteLine($"nullableDate == null: {nullableDate == null}");

            Console.WriteLine();
        }

        #endregion

        #region Nullable Struct Internals

        static void ExploreNullableStructInternals()
        {
            Console.WriteLine("3. EXPLORING THE NULLABLE<T> STRUCT INTERNALS");
            Console.WriteLine("=============================================");

            // Let's peek under the hood - int? is really Nullable<int>
            Console.WriteLine("Under the hood, int? is actually System.Nullable<int>");
            
            // These two declarations are equivalent
            Nullable<int> explicitNullable = new Nullable<int>(100);
            int? shorthandNullable = 100;

            Console.WriteLine($"Explicit Nullable<int>: {explicitNullable}");
            Console.WriteLine($"Shorthand int?: {shorthandNullable}");
            Console.WriteLine($"Are they the same type? {explicitNullable.GetType() == shorthandNullable.GetType()}");

            // The struct has two key properties: HasValue and Value
            int? testValue = 50;
            
            Console.WriteLine($"\nExamining the internal structure:");
            Console.WriteLine($"testValue = {testValue}");
            Console.WriteLine($"testValue.HasValue = {testValue.HasValue}");
            
            if (testValue.HasValue)
            {
                Console.WriteLine($"testValue.Value = {testValue.Value}");
            }

            // Let's see what happens when we set it to null
            testValue = null;
            Console.WriteLine($"\nAfter setting to null:");
            Console.WriteLine($"testValue = {testValue}");
            Console.WriteLine($"testValue.HasValue = {testValue.HasValue}");

            // Here's the dangerous part - accessing Value when HasValue is false
            Console.WriteLine("\nDemonstrating the danger of accessing Value when null:");
            try
            {
#pragma warning disable CS8629 // Nullable value type may be null
                int dangerousAccess = testValue.Value; // This will throw InvalidOperationException!
#pragma warning restore CS8629
                Console.WriteLine($"This won't print: {dangerousAccess}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            // Safe alternatives for getting values
            Console.WriteLine("\nSafe ways to extract values:");
            Console.WriteLine($"GetValueOrDefault(): {testValue.GetValueOrDefault()}");
            Console.WriteLine($"GetValueOrDefault(999): {testValue.GetValueOrDefault(999)}");

            // Default value behavior
            int? defaultNullable = default;
            Console.WriteLine($"\nDefault value of int?: {defaultNullable}");
            Console.WriteLine($"Default HasValue: {defaultNullable.HasValue}");

            Console.WriteLine();
        }

        #endregion

        #region Implicit and Explicit Conversions

        static void ImplicitExplicitConversionsDemo()
        {
            Console.WriteLine("4. UNDERSTANDING NULLABLE CONVERSIONS");
            Console.WriteLine("=====================================");

            Console.WriteLine("One of the beautiful things about nullable types is how they handle conversions.");
            Console.WriteLine("Let's see the rules in action...\n");

            // RULE 1: T to T? is always implicit (safe)
            Console.WriteLine("RULE 1: Regular value type → Nullable type (IMPLICIT - always safe)");
            int regularInt = 25;
            int? nullableFromRegular = regularInt;  // No cast needed!
            
            Console.WriteLine($"Regular int: {regularInt}");
            Console.WriteLine($"Implicitly converted to nullable: {nullableFromRegular}");
            Console.WriteLine("Why is this safe? Because a regular value always has a value!\n");

            // RULE 2: T? to T requires explicit conversion (dangerous)
            Console.WriteLine("RULE 2: Nullable type → Regular value type (EXPLICIT - potentially dangerous)");
            int? nullableInt = 75;
            int backToRegular = (int)nullableInt;  // Explicit cast required
            
            Console.WriteLine($"Nullable int: {nullableInt}");
            Console.WriteLine($"Explicitly converted back to regular: {backToRegular}");
            Console.WriteLine("Why explicit? Because the nullable might be null!\n");

            // The dangerous scenario - null to regular type
            Console.WriteLine("The DANGEROUS scenario - converting null to regular type:");
            int? nullValue = null;
            try
            {
#pragma warning disable CS8629 // Nullable value type may be null
                int willThrow = (int)nullValue;  // This explodes!
#pragma warning restore CS8629
                Console.WriteLine($"This won't print: {willThrow}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"💥 Boom! {ex.Message}");
            }

            // SAFE ALTERNATIVES
            Console.WriteLine("\nSAFE alternatives for nullable → regular conversions:");
            
            // Option 1: Check HasValue first
            Console.WriteLine("Option 1: Check HasValue first");
            if (nullValue.HasValue)
            {
                int safeConversion = (int)nullValue;
                Console.WriteLine($"Safe conversion: {safeConversion}");
            }
            else
            {
                Console.WriteLine("Cannot convert - value is null, avoiding the explosion!");
            }

            // Option 2: Use GetValueOrDefault
            Console.WriteLine("\nOption 2: Use GetValueOrDefault");
            int defaultValue = nullValue.GetValueOrDefault(-1);
            Console.WriteLine($"Using GetValueOrDefault(-1): {defaultValue}");

            // Option 3: Use null-coalescing operator (we'll cover this more later)
            Console.WriteLine("\nOption 3: Use null-coalescing operator (??)");
            int coalescedValue = nullValue ?? -999;
            Console.WriteLine($"Using ?? operator: {coalescedValue}");

            Console.WriteLine();
        }

        #endregion

        #region Boxing and Unboxing

        static void BoxingUnboxingDemo()
        {
            Console.WriteLine("4. BOXING AND UNBOXING DEMONSTRATION");
            Console.WriteLine("====================================");

            // Boxing nullable types - the boxed object contains the underlying value, not the nullable wrapper
            int? nullableValue = 123;
            object boxedValue = nullableValue;  // Boxing
            
            Console.WriteLine($"Original nullable: {nullableValue}");
            Console.WriteLine($"Boxed object: {boxedValue}");
            Console.WriteLine($"Boxed type: {boxedValue.GetType().Name}");

            // Boxing a null nullable type results in null reference
            int? nullNullable = null;
            object? boxedNull = nullNullable;
            
            Console.WriteLine($"Null nullable: {nullNullable}");
            Console.WriteLine($"Boxed null: {boxedNull}");
            Console.WriteLine($"Boxed null == null: {boxedNull == null}");

            // Unboxing back to nullable
            int? unboxedValue = (int?)boxedValue;
            Console.WriteLine($"Unboxed back to nullable: {unboxedValue}");

            // Safe unboxing using 'as' operator
            object stringObject = "Not a number";
            int? safeUnbox = stringObject as int?;
            
            Console.WriteLine($"Safe unboxing from string: {safeUnbox}");
            Console.WriteLine($"Safe unboxing HasValue: {safeUnbox.HasValue}");

            // Direct unboxing to regular value type
            object anotherBoxed = 456;
            int directUnbox = (int)anotherBoxed;
            Console.WriteLine($"Direct unboxing: {directUnbox}");

            Console.WriteLine();
        }

        #endregion

        #region Operator Lifting

        static void OperatorLiftingDemo()
        {
            Console.WriteLine("6. OPERATOR LIFTING - THE MAGIC BEHIND THE SCENES");
            Console.WriteLine("==================================================");

            Console.WriteLine("Here's something cool: you can use regular operators (+, -, <, >, ==) with nullable types!");
            Console.WriteLine("This works because of 'operator lifting' - the compiler does magic for us.\n");

            int? a = 10;
            int? b = 20;
            int? c = null;

            Console.WriteLine($"Our test values: a = {a}, b = {b}, c = {c}");

            Console.WriteLine("\nArithmetic operations with valid values work as expected:");
            Console.WriteLine($"a + b = {a + b}");     // Lifted addition
            Console.WriteLine($"b - a = {b - a}");     // Lifted subtraction  
            Console.WriteLine($"a * 2 = {a * 2}");     // Mixed nullable/regular

            Console.WriteLine("\nBut here's where it gets interesting - operations with null:");
            Console.WriteLine($"a + c = {a + c}");     // 10 + null = null
            Console.WriteLine($"c * b = {c * b}");     // null * 20 = null
            Console.WriteLine($"c / 5 = {c / 5}");     // null / 5 = null

            Console.WriteLine("\nThe rule: If ANY operand is null, arithmetic result is null");
            Console.WriteLine("(This is similar to SQL's NULL behavior)\n");

            Console.WriteLine("Comparison operations have their own rules:");
            Console.WriteLine($"a < b = {a < b}");     // 10 < 20 = true
            Console.WriteLine($"a > b = {a > b}");     // 10 > 20 = false

            Console.WriteLine("\nBut comparisons with null are always false:");
            Console.WriteLine($"a < c = {a < c}");     // 10 < null = false
            Console.WriteLine($"c > b = {c > b}");     // null > 20 = false
            Console.WriteLine($"c < 5 = {c < 5}");     // null < 5 = false

            Console.WriteLine("\nKey insight: null is incomparable - neither greater, less, nor equal to anything");
            Console.WriteLine("(except for equality, where null == null is true)");

            Console.WriteLine();
        }

        #endregion

        #region Equality Operators

        static void EqualityOperatorsDemo()
        {
            Console.WriteLine("6. EQUALITY OPERATORS DEMONSTRATION");
            Console.WriteLine("===================================");

            int? x = 5;
            int? y = 5;
            int? z = 10;
            int? nullValue1 = null;
            int? nullValue2 = null;

            Console.WriteLine($"x = {x}, y = {y}, z = {z}");
            Console.WriteLine($"nullValue1 = {nullValue1}, nullValue2 = {nullValue2}");

            // Equality with same values
            Console.WriteLine($"x == y: {x == y}");  // True
            Console.WriteLine($"x != z: {x != z}");  // True

            // Equality with null
            Console.WriteLine($"x == null: {x == null}");  // False
            Console.WriteLine($"nullValue1 == null: {nullValue1 == null}");  // True

            // Two nulls are equal
            Console.WriteLine($"nullValue1 == nullValue2: {nullValue1 == nullValue2}");  // True

            // Mixed comparisons
            Console.WriteLine($"x == nullValue1: {x == nullValue1}");  // False

            // Comparing with regular value types
            int regularInt = 5;
            Console.WriteLine($"x == regularInt: {x == regularInt}");  // True (implicit conversion)

            Console.WriteLine();
        }

        #endregion

        #region Relational Operators

        static void RelationalOperatorsDemo()
        {
            Console.WriteLine("7. RELATIONAL OPERATORS DEMONSTRATION");
            Console.WriteLine("=====================================");

            int? a = 10;
            int? b = 20;
            int? nullValue = null;

            Console.WriteLine($"a = {a}, b = {b}, nullValue = {nullValue}");

            // Normal comparisons
            Console.WriteLine($"a < b: {a < b}");    // True
            Console.WriteLine($"a > b: {a > b}");    // False
            Console.WriteLine($"a <= 10: {a <= 10}");  // True (comparing with literal)
            Console.WriteLine($"b >= a: {b >= a}");  // True

            // Comparisons involving null always return false
            Console.WriteLine($"a < nullValue: {a < nullValue}");     // False
            Console.WriteLine($"nullValue < a: {nullValue < a}");     // False
            Console.WriteLine($"nullValue > b: {nullValue > b}");     // False
            Console.WriteLine($"nullValue <= 50: {nullValue <= 50}");  // False (comparing null with literal)

            Console.WriteLine("\nKey insight: Any relational comparison with null returns false");
            Console.WriteLine("This is different from equality, where null == null is true");

            Console.WriteLine();
        }

        #endregion

        #region Arithmetic Operators

        static void ArithmeticOperatorsDemo()
        {
            Console.WriteLine("8. ARITHMETIC OPERATORS DEMONSTRATION");
            Console.WriteLine("=====================================");

            int? a = 15;
            int? b = 5;
            int? nullValue = null;

            Console.WriteLine($"a = {a}, b = {b}, nullValue = {nullValue}");

            // Normal arithmetic operations
            Console.WriteLine($"a + b = {a + b}");    // 20
            Console.WriteLine($"a - b = {a - b}");    // 10
            Console.WriteLine($"a * b = {a * b}");    // 75
            Console.WriteLine($"a / b = {a / b}");    // 3

            // Arithmetic with null propagates null
            Console.WriteLine($"a + nullValue = {a + nullValue}");      // null
            Console.WriteLine($"nullValue - b = {nullValue - b}");      // null
            Console.WriteLine($"nullValue * a = {nullValue * a}");      // null
            Console.WriteLine($"nullValue / b = {nullValue / b}");      // null

            // Chained operations
            int? result = a + b * nullValue;
            Console.WriteLine($"a + b * nullValue = {result}");         // null

            Console.WriteLine("\nKey insight: Any arithmetic operation with null results in null");
            Console.WriteLine("This is similar to SQL's NULL behavior");

            Console.WriteLine();
        }

        #endregion

        #region Mixing Nullable and Non-Nullable

        static void MixingNullableNonNullableDemo()
        {
            Console.WriteLine("9. MIXING NULLABLE AND NON-NULLABLE DEMONSTRATION");
            Console.WriteLine("=================================================");

            int? nullableInt = null;
            int regularInt = 10;
            double regularDouble = 3.14;

            Console.WriteLine($"nullableInt = {nullableInt}");
            Console.WriteLine($"regularInt = {regularInt}");
            Console.WriteLine($"regularDouble = {regularDouble}");

            // Mixing in arithmetic - regular types are implicitly converted to nullable
            int? result1 = nullableInt + regularInt;     // null + 10 = null
            int? result2 = regularInt + nullableInt;     // 10 + null = null
            
            Console.WriteLine($"nullableInt + regularInt = {result1}");
            Console.WriteLine($"regularInt + nullableInt = {result2}");

            // When both operands are non-null
            nullableInt = 5;
            int? result3 = nullableInt + regularInt;     // 5 + 10 = 15
            
            Console.WriteLine($"After setting nullableInt = 5:");
            Console.WriteLine($"nullableInt + regularInt = {result3}");

            // Mixed type operations
            double? mixedResult = nullableInt * regularDouble;  // 5 * 3.14 = 15.7
            Console.WriteLine($"nullableInt * regularDouble = {mixedResult}");

            // Assignment scenarios
            int? fromRegular = regularInt;       // Implicit conversion
            // int toRegular = nullableInt;      // This would require explicit cast
            
            if (nullableInt.HasValue)
            {
                int safeAssignment = (int)nullableInt;
                Console.WriteLine($"Safe assignment: {safeAssignment}");
            }

            Console.WriteLine();
        }

        #endregion

        #region Boolean Logical Operators

        static void BooleanLogicalOperatorsDemo()
        {
            Console.WriteLine("10. BOOLEAN LOGICAL OPERATORS DEMONSTRATION");
            Console.WriteLine("===========================================");

            bool? n = null;
            bool? f = false;
            bool? t = true;

            Console.WriteLine($"n = {n}, f = {f}, t = {t}");
            Console.WriteLine("\nTesting logical OR (|) operator:");

            // OR operations - null is treated as "unknown"
            Console.WriteLine($"n | n = {n | n}");        // null | null = null
            Console.WriteLine($"n | f = {n | f}");        // null | false = null
            Console.WriteLine($"n | t = {n | t}");        // null | true = true (because true OR anything is true)
            Console.WriteLine($"f | n = {f | n}");        // false | null = null
            Console.WriteLine($"t | n = {t | n}");        // true | null = true

            Console.WriteLine("\nTesting logical AND (&) operator:");
            
            // AND operations
            Console.WriteLine($"n & n = {n & n}");        // null & null = null
            Console.WriteLine($"n & f = {n & f}");        // null & false = false (because false AND anything is false)
            Console.WriteLine($"n & t = {n & t}");        // null & true = null
            Console.WriteLine($"f & n = {f & n}");        // false & null = false
            Console.WriteLine($"t & n = {t & n}");        // true & null = null

            Console.WriteLine("\nKey insights:");
            Console.WriteLine("- true OR anything = true (even null)");
            Console.WriteLine("- false AND anything = false (even null)");
            Console.WriteLine("- Other combinations with null remain null");

            // Practical example
            Console.WriteLine("\nPractical example - user permissions:");
            bool? hasAdminRights = null;  // Unknown
            bool? hasReadAccess = true;
            bool? hasWriteAccess = false;

            bool? canPerformAction = hasAdminRights | (hasReadAccess & hasWriteAccess);
            Console.WriteLine($"Can perform action: {canPerformAction}");  // false

            Console.WriteLine();
        }

        #endregion

        #region Null-Coalescing Operator

        static void NullCoalescingOperatorDemo()
        {
            Console.WriteLine("11. NULL-COALESCING OPERATOR (??) DEMONSTRATION");
            Console.WriteLine("===============================================");

            int? nullableValue = null;
            int? anotherValue = 42;

            // Basic null-coalescing
            int result1 = nullableValue ?? 100;
            int result2 = anotherValue ?? 100;

            Console.WriteLine($"nullableValue = {nullableValue}");
            Console.WriteLine($"anotherValue = {anotherValue}");
            Console.WriteLine($"nullableValue ?? 100 = {result1}");
            Console.WriteLine($"anotherValue ?? 100 = {result2}");

            // Chaining null-coalescing operators
            int? first = null;
            int? second = null;
            int? third = 999;
            int? fourth = 888;

            int chainedResult = first ?? second ?? third ?? fourth ?? 0;
            Console.WriteLine($"\nChaining example:");
            Console.WriteLine($"first = {first}, second = {second}, third = {third}, fourth = {fourth}");
            Console.WriteLine($"first ?? second ?? third ?? fourth ?? 0 = {chainedResult}");

            // Null-coalescing with different types
            string? nullString = null;
            string defaultString = nullString ?? "Default Value";
            Console.WriteLine($"\nWith strings:");
            Console.WriteLine($"nullString ?? \"Default Value\" = \"{defaultString}\"");

            // Practical examples
            Console.WriteLine("\nPractical examples:");
            
            // Configuration values
            int? configTimeout = GetConfigTimeout(); // Might return null
            int actualTimeout = configTimeout ?? 30; // Default to 30 seconds
            Console.WriteLine($"Configuration timeout: {configTimeout}");
            Console.WriteLine($"Actual timeout used: {actualTimeout} seconds");

            // User input validation
            int? userAge = GetUserAge(); // Might be null if invalid
            string ageDisplay = $"Age: {userAge?.ToString() ?? "Not specified"}";
            Console.WriteLine($"User age display: {ageDisplay}");

            // Null-coalescing assignment (C# 8.0+)
            int? score = null;
            score ??= 0; // Assign 0 if score is null
            Console.WriteLine($"Score after ??= operator: {score}");

            Console.WriteLine();
        }

        // Helper methods for practical examples
        static int? GetConfigTimeout()
        {
            // Simulate reading from config file that might not have this value
            return null;
        }

        static int? GetUserAge()
        {
            // Simulate user input that might be invalid
            return null;
        }

        #endregion

        #region Comparing with Magic Values

        static void CompareWithMagicValues()
        {
            Console.WriteLine("13. NULLABLE TYPES VS MAGIC VALUES COMPARISON");
            Console.WriteLine("==============================================");

            Console.WriteLine("Before nullable types, developers used 'magic values' to represent null:");
            Console.WriteLine("This approach had serious limitations...\n");

            // Example of the old "magic value" approach
            Console.WriteLine("OLD APPROACH - Magic Values:");
            Console.WriteLine("=============================");

            // String.IndexOf returns -1 if character not found (magic value approach)
            string text = "Hello World";
            int indexOfZ = text.IndexOf('z');  // Returns -1 (magic value for "not found")
            int indexOfH = text.IndexOf('H');  // Returns 0 (actual index)

            Console.WriteLine($"Looking for 'z' in '{text}': {indexOfZ}");
            Console.WriteLine($"Looking for 'H' in '{text}': {indexOfH}");

            // Problems with magic values:
            Console.WriteLine("\nProblems with magic values:");
            Console.WriteLine("1. Inconsistency - each type uses different 'null' representations");
            Console.WriteLine("2. Collision risk - magic value might be valid data");
            Console.WriteLine("3. Silent errors - forgetting to check leads to bugs");
            Console.WriteLine("4. Not type-safe - 'null' state not captured in type system");

            // Demonstrating inconsistency
            int notFoundIndex = -1;        // String operations use -1
            DateTime invalidDate = DateTime.MinValue;  // Date operations might use MinValue
            double invalidNumber = double.NaN;         // Math operations might use NaN

            Console.WriteLine($"\nInconsistent magic values:");
            Console.WriteLine($"String 'not found': {notFoundIndex}");
            Console.WriteLine($"Invalid date: {invalidDate}");
            Console.WriteLine($"Invalid number: {invalidNumber}");

            Console.WriteLine("\nNEW APPROACH - Nullable Types:");
            Console.WriteLine("===============================");

            // Modern nullable approach provides consistency
            int? findIndex = FindCharacterIndex(text, 'z');  // Returns null if not found
            int? findValidIndex = FindCharacterIndex(text, 'H');  // Returns actual index

            Console.WriteLine($"Looking for 'z': {findIndex?.ToString() ?? "Not found"}");
            Console.WriteLine($"Looking for 'H': {findValidIndex?.ToString() ?? "Not found"}");

            Console.WriteLine("\nBenefits of nullable types:");
            Console.WriteLine("1. Consistent pattern across all value types");
            Console.WriteLine("2. No collision with valid data");
            Console.WriteLine("3. Compile-time checking for null handling");
            Console.WriteLine("4. Type-safe - null state is part of the type system");
            Console.WriteLine("5. Clear intent - T? explicitly means 'might be null'");

            // Demonstrating type safety
            if (findIndex.HasValue)
            {
                Console.WriteLine($"Character found at index: {findIndex.Value}");
            }
            else
            {
                Console.WriteLine("Character not found - no ambiguity!");
            }

            Console.WriteLine();
        }

        // Helper method demonstrating the nullable approach
        static int? FindCharacterIndex(string text, char character)
        {
            int index = text.IndexOf(character);
            return index >= 0 ? index : null;  // Return null instead of -1
        }

        #endregion

        #region Real World Scenario

        static void RealWorldScenarioDemo()
        {
            Console.WriteLine("14. REAL WORLD SCENARIO - EMPLOYEE MANAGEMENT SYSTEM");
            Console.WriteLine("======================================================");

            Console.WriteLine("Let's see nullable types in action with a practical employee management system.");
            Console.WriteLine("This demonstrates all the concepts we've learned in a real-world context.\n");

            var employees = new[]
            {
                new Employee("John Doe", 30, 75000.50m),
                new Employee("Jane Smith", null, 82000.00m),  // Age is private/unknown
                new Employee("Bob Wilson", 45, null),          // Salary is confidential
                new Employee("Alice Brown", null, null),       // Both unknown (new hire?)
                new Employee("Charlie Davis", 28, 45000.00m)
            };

            Console.WriteLine("Employee Management System - Complete Employee List:");
            Console.WriteLine("====================================================");

            foreach (var emp in employees)
            {
                emp.DisplayInfo();
                Console.WriteLine();
            }

            // Demonstrate nullable arithmetic in action
            Console.WriteLine("BONUS CALCULATION DEMO:");
            Console.WriteLine("=======================");
            decimal? bonusPercentage = 5.5m; // 5.5% bonus

            foreach (var emp in employees)
            {
                decimal? bonus = emp.CalculateBonus(bonusPercentage);
                string bonusDisplay = bonus?.ToString("C") ?? "Cannot calculate (salary unknown)";
                Console.WriteLine($"{emp.Name}: Bonus = {bonusDisplay}");
            }

            Console.WriteLine("\nRETIREMENT ELIGIBILITY CHECK:");
            Console.WriteLine("==============================");
            foreach (var emp in employees)
            {
                bool eligible = emp.IsRetirementEligible();
                string status = eligible ? "Eligible for retirement" : "Not eligible (or age unknown)";
                Console.WriteLine($"{emp.Name}: {status}");
            }

            // Statistical analysis with nullable values
            Console.WriteLine("\nSTATISTICAL ANALYSIS:");
            Console.WriteLine("=====================");

            var stats = EmployeeStatistics.Calculate(employees);
            Console.WriteLine($"Total employees: {stats.TotalEmployees}");
            Console.WriteLine($"Employees with known age: {stats.EmployeesWithAge}");
            Console.WriteLine($"Employees with known salary: {stats.EmployeesWithSalary}");
            Console.WriteLine($"Average age: {stats.AverageAge?.ToString("F1") ?? "Cannot calculate (insufficient data)"}");
            Console.WriteLine($"Average salary: {stats.AverageSalary?.ToString("C") ?? "Cannot calculate (insufficient data)"}");
            Console.WriteLine($"Total payroll: {stats.TotalPayroll?.ToString("C") ?? "Cannot calculate (some salaries unknown)"}");

            Console.WriteLine("\nKey Takeaways from this real-world example:");
            Console.WriteLine("===========================================");
            Console.WriteLine("1. Nullable types elegantly handle missing/unknown data");
            Console.WriteLine("2. Operations propagate null appropriately (bonus calculation)");
            Console.WriteLine("3. GetValueOrDefault provides safe fallbacks (retirement eligibility)");
            Console.WriteLine("4. Null-coalescing operators create user-friendly displays");
            Console.WriteLine("5. Statistical calculations handle partial data gracefully");

            Console.WriteLine();
        }

        #endregion

        #region Demonstrating the Basic Problem

        static void DemonstrateTheBasicProblem()
        {
            Console.WriteLine("1. THE FUNDAMENTAL PROBLEM WITH VALUE TYPES");
            Console.WriteLine("============================================");

            // Reference types can naturally represent "no value" with null
            string? someText = null;  // Perfectly valid - means "no text"
            object? someObject = null;  // Also valid - means "no object"
            
            Console.WriteLine($"Reference type (string): {someText ?? "null"}");
            Console.WriteLine($"Reference type (object): {someObject ?? "null"}");

            // But value types always contain some value - they can't be "empty"
            int regularInt = default;    // This gives us 0, not "nothing"
            bool regularBool = default;  // This gives us false, not "unknown"
            DateTime regularDate = default;  // This gives us 1/1/0001, not "no date"

            Console.WriteLine($"Value type (int): {regularInt}");
            Console.WriteLine($"Value type (bool): {regularBool}");
            Console.WriteLine($"Value type (DateTime): {regularDate}");

            Console.WriteLine("\nThe Problem:");
            Console.WriteLine("- What if we need to represent 'unknown age' in a Person class?");
            Console.WriteLine("- What if a database column allows NULL for an integer field?");
            Console.WriteLine("- What if a user hasn't provided their birth date yet?");
            
            // This won't compile - demonstrates the problem
            // int impossibleInt = null;  // Compile error!
            
            Console.WriteLine("\nSolution: Nullable Value Types!");
            Console.WriteLine("Now we can have int?, bool?, DateTime? etc.\n");
        }

        #endregion
    }

    #region Real World Classes

    public class Employee
    {
        public string Name { get; }
        public int? Age { get; }           // Nullable - age might be private/unknown
        public decimal? Salary { get; }    // Nullable - salary might be confidential

        public Employee(string name, int? age, decimal? salary)
        {
            Name = name;
            Age = age;
            Salary = salary;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Employee: {Name}");
            
            // Using null-coalescing for display
            string ageDisplay = Age?.ToString() ?? "Unknown";
            string salaryDisplay = Salary?.ToString("C") ?? "Confidential";
            
            Console.WriteLine($"  Age: {ageDisplay}");
            Console.WriteLine($"  Salary: {salaryDisplay}");

            // Determine benefits eligibility (requires both age and salary)
            bool? eligibleForBenefits = IsEligibleForBenefits();
            string eligibilityStatus = eligibleForBenefits?.ToString() ?? "Cannot determine";
            Console.WriteLine($"  Benefits eligible: {eligibilityStatus}");
        }

        public bool? IsEligibleForBenefits()
        {
            // Need both age and salary to determine eligibility
            if (!Age.HasValue || !Salary.HasValue)
                return null; // Cannot determine without complete information

            // Eligible if age >= 21 and salary >= 40000
            return Age >= 21 && Salary >= 40000;
        }

        public decimal? CalculateBonus(decimal? bonusPercentage)
        {
            // Using nullable arithmetic - if any value is null, result is null
            return Salary * (bonusPercentage / 100);
        }

        public bool IsRetirementEligible()
        {
            // Using GetValueOrDefault for safe comparison
            return Age.GetValueOrDefault(0) >= 65;
        }
    }

    public class EmployeeStatistics
    {
        public int TotalEmployees { get; set; }
        public int EmployeesWithAge { get; set; }
        public int EmployeesWithSalary { get; set; }
        public double? AverageAge { get; set; }
        public decimal? AverageSalary { get; set; }
        public decimal? TotalPayroll { get; set; }

        public static EmployeeStatistics Calculate(Employee[] employees)
        {
            var stats = new EmployeeStatistics
            {
                TotalEmployees = employees.Length
            };

            // Calculate statistics for non-null values only
            var knownAges = new System.Collections.Generic.List<int>();
            var knownSalaries = new System.Collections.Generic.List<decimal>();
            decimal? totalPayroll = 0;
            bool allSalariesKnown = true;

            foreach (var emp in employees)
            {
                if (emp.Age.HasValue)
                {
                    knownAges.Add(emp.Age.Value);
                    stats.EmployeesWithAge++;
                }

                if (emp.Salary.HasValue)
                {
                    knownSalaries.Add(emp.Salary.Value);
                    stats.EmployeesWithSalary++;
                    totalPayroll += emp.Salary.Value;
                }
                else
                {
                    allSalariesKnown = false;
                }
            }

            // Calculate averages (nullable results)
            stats.AverageAge = knownAges.Count > 0 ? knownAges.Average() : null;
            stats.AverageSalary = knownSalaries.Count > 0 ? knownSalaries.Average() : null;
            stats.TotalPayroll = allSalariesKnown ? totalPayroll : null;

            return stats;
        }
    }

    // Extension method to calculate average for decimal collections
    public static class Extensions
    {
        public static double Average(this System.Collections.Generic.List<int> values)
        {
            return values.Sum() / (double)values.Count;
        }

        public static decimal Average(this System.Collections.Generic.List<decimal> values)
        {
            return values.Sum() / values.Count;
        }

        public static int Sum(this System.Collections.Generic.List<int> values)
        {
            int sum = 0;
            foreach (var value in values)
                sum += value;
            return sum;
        }

        public static decimal Sum(this System.Collections.Generic.List<decimal> values)
        {
            decimal sum = 0;
            foreach (var value in values)
                sum += value;
            return sum;
        }
    }

    #endregion
}
