using System;

namespace NullableValueTypesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Nullable Value Types in C# - Complete Demonstration ===\n");

            // Run all demonstrations
            BasicNullableTypesDemo();
            NullableStructInternalsDemo();
            ImplicitExplicitConversionsDemo();
            BoxingUnboxingDemo();
            OperatorLiftingDemo();
            EqualityOperatorsDemo();
            RelationalOperatorsDemo();
            ArithmeticOperatorsDemo();
            MixingNullableNonNullableDemo();
            BooleanLogicalOperatorsDemo();
            NullCoalescingOperatorDemo();
            RealWorldScenarioDemo();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Nullable Types

        static void BasicNullableTypesDemo()
        {
            Console.WriteLine("1. BASIC NULLABLE TYPES DEMONSTRATION");
            Console.WriteLine("=====================================");

            // Regular value types cannot hold null
            int regularInt = 0;  // Default value, not null
            bool regularBool = false;  // Default value, not null
            
            Console.WriteLine($"Regular int default: {regularInt}");
            Console.WriteLine($"Regular bool default: {regularBool}");

            // Nullable value types CAN hold null
            int? nullableInt = null;
            bool? nullableBool = null;
            double? nullableDouble = null;

            Console.WriteLine($"Nullable int: {nullableInt}");
            Console.WriteLine($"Nullable bool: {nullableBool}");
            Console.WriteLine($"Nullable double: {nullableDouble}");

            // Check if they're null
            Console.WriteLine($"nullableInt == null: {nullableInt == null}");
            Console.WriteLine($"nullableBool == null: {nullableBool == null}");

            // Assign actual values
            nullableInt = 42;
            nullableBool = true;
            nullableDouble = 3.14159;

            Console.WriteLine($"\nAfter assigning values:");
            Console.WriteLine($"Nullable int: {nullableInt}");
            Console.WriteLine($"Nullable bool: {nullableBool}");
            Console.WriteLine($"Nullable double: {nullableDouble}");

            Console.WriteLine();
        }

        #endregion

        #region Nullable Struct Internals

        static void NullableStructInternalsDemo()
        {
            Console.WriteLine("2. NULLABLE STRUCT INTERNALS DEMONSTRATION");
            Console.WriteLine("==========================================");

            // Behind the scenes, int? is actually Nullable<int>
            Nullable<int> explicitNullable = new Nullable<int>(100);
            int? shorthandNullable = 100;

            Console.WriteLine($"Explicit Nullable<int>: {explicitNullable}");
            Console.WriteLine($"Shorthand int?: {shorthandNullable}");

            // Working with HasValue and Value properties
            int? testValue = 50;
            
            Console.WriteLine($"\nTesting HasValue and Value:");
            Console.WriteLine($"testValue: {testValue}");
            Console.WriteLine($"testValue.HasValue: {testValue.HasValue}");
            
            if (testValue.HasValue)
            {
                Console.WriteLine($"testValue.Value: {testValue.Value}");
            }

            // Now test with null
            testValue = null;
            Console.WriteLine($"\nAfter setting to null:");
            Console.WriteLine($"testValue: {testValue}");
            Console.WriteLine($"testValue.HasValue: {testValue.HasValue}");

            // Accessing Value when HasValue is false would throw an exception
            try
            {
                int dangerousAccess = testValue.Value; // This will throw!
                Console.WriteLine($"This won't print: {dangerousAccess}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            // Safe ways to get values
            Console.WriteLine($"GetValueOrDefault(): {testValue.GetValueOrDefault()}");
            Console.WriteLine($"GetValueOrDefault(999): {testValue.GetValueOrDefault(999)}");

            Console.WriteLine();
        }

        #endregion

        #region Implicit and Explicit Conversions

        static void ImplicitExplicitConversionsDemo()
        {
            Console.WriteLine("3. IMPLICIT AND EXPLICIT CONVERSIONS DEMONSTRATION");
            Console.WriteLine("==================================================");

            // Implicit conversion from T to T? (always safe)
            int regularInt = 25;
            int? nullableFromRegular = regularInt;  // Implicit conversion
            
            Console.WriteLine($"Regular int: {regularInt}");
            Console.WriteLine($"Converted to nullable: {nullableFromRegular}");

            // Explicit conversion from T? to T (can be dangerous)
            int? nullableInt = 75;
            int backToRegular = (int)nullableInt;  // Explicit cast required
            
            Console.WriteLine($"Nullable int: {nullableInt}");
            Console.WriteLine($"Converted back to regular: {backToRegular}");

            // Dangerous explicit conversion - this will throw!
            int? nullValue = null;
            try
            {
                int willThrow = (int)nullValue;  // InvalidOperationException!
                Console.WriteLine($"This won't print: {willThrow}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Conversion failed: {ex.Message}");
            }

            // Safe way to convert back
            Console.WriteLine("\nSafe conversion patterns:");
            if (nullValue.HasValue)
            {
                int safeConversion = (int)nullValue;
                Console.WriteLine($"Safe conversion: {safeConversion}");
            }
            else
            {
                Console.WriteLine("Cannot convert - value is null");
            }

            // Using GetValueOrDefault for safe conversion
            int defaultValue = nullValue.GetValueOrDefault(-1);
            Console.WriteLine($"Using GetValueOrDefault: {defaultValue}");

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
            object boxedNull = nullNullable;
            
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
            Console.WriteLine("5. OPERATOR LIFTING DEMONSTRATION");
            Console.WriteLine("=================================");

            Console.WriteLine("Operator lifting allows regular operators to work with nullable types");
            Console.WriteLine("If any operand is null, special rules apply...\n");

            int? a = 10;
            int? b = 20;
            int? c = null;

            Console.WriteLine($"a = {a}, b = {b}, c = {c}");

            // Arithmetic operations with valid values
            Console.WriteLine($"a + b = {a + b}");
            Console.WriteLine($"b - a = {b - a}");
            Console.WriteLine($"a * 2 = {a * 2}");

            // Arithmetic operations with null (result is null)
            Console.WriteLine($"a + c = {a + c}");
            Console.WriteLine($"c * b = {c * b}");

            // Comparison operations
            Console.WriteLine($"a < b = {a < b}");
            Console.WriteLine($"a > b = {a > b}");

            // Comparison with null (result is false for relational operators)
            Console.WriteLine($"a < c = {a < c}");
            Console.WriteLine($"c > b = {c > b}");

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
            Console.WriteLine($"a <= a: {a <= a}");  // True
            Console.WriteLine($"b >= a: {b >= a}");  // True

            // Comparisons involving null always return false
            Console.WriteLine($"a < nullValue: {a < nullValue}");     // False
            Console.WriteLine($"nullValue < a: {nullValue < a}");     // False
            Console.WriteLine($"nullValue > b: {nullValue > b}");     // False
            Console.WriteLine($"nullValue <= nullValue: {nullValue <= nullValue}");  // False

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
            string nullString = null;
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

        #region Real World Scenario

        static void RealWorldScenarioDemo()
        {
            Console.WriteLine("12. REAL WORLD SCENARIO - EMPLOYEE MANAGEMENT SYSTEM");
            Console.WriteLine("=====================================================");

            var employees = new[]
            {
                new Employee("John Doe", 30, 75000.50m),
                new Employee("Jane Smith", null, 82000.00m),  // Age unknown
                new Employee("Bob Wilson", 45, null),          // Salary confidential
                new Employee("Alice Brown", null, null)        // Both unknown
            };

            Console.WriteLine("Employee Management System:");
            Console.WriteLine("===========================");

            foreach (var emp in employees)
            {
                emp.DisplayInfo();
                Console.WriteLine();
            }

            // Statistical analysis with nullable values
            Console.WriteLine("Statistical Analysis:");
            Console.WriteLine("====================");

            var stats = EmployeeStatistics.Calculate(employees);
            Console.WriteLine($"Total employees: {stats.TotalEmployees}");
            Console.WriteLine($"Employees with known age: {stats.EmployeesWithAge}");
            Console.WriteLine($"Employees with known salary: {stats.EmployeesWithSalary}");
            Console.WriteLine($"Average age: {stats.AverageAge?.ToString("F1") ?? "Unknown"}");
            Console.WriteLine($"Average salary: {stats.AverageSalary?.ToString("C") ?? "Confidential"}");

            Console.WriteLine();
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

        public static EmployeeStatistics Calculate(Employee[] employees)
        {
            var stats = new EmployeeStatistics
            {
                TotalEmployees = employees.Length
            };

            // Calculate statistics for non-null values only
            var knownAges = new System.Collections.Generic.List<int>();
            var knownSalaries = new System.Collections.Generic.List<decimal>();

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
                }
            }

            // Calculate averages (nullable results)
            stats.AverageAge = knownAges.Count > 0 ? knownAges.Average() : null;
            stats.AverageSalary = knownSalaries.Count > 0 ? knownSalaries.Average() : null;

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
