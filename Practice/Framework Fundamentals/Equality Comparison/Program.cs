using System;
using System.Collections.Generic;

namespace EqualityComparison
{
    /// <summary>
    /// Comprehensive demonstration of equality comparison in C#
    /// This project covers value vs referential equality, various comparison methods, and custom implementations
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Equality Comparison in C# ===");
            Console.WriteLine("This demonstration covers all aspects of equality comparison in .NET\n");

            // Execute all demonstration methods
            DemonstrateValueVsReferentialEquality();
            DemonstrateOperatorEquality();
            DemonstrateEqualsMethod();
            DemonstrateObjectEqualsStatic();
            DemonstrateNullHandling();
            DemonstrateStringEquality();
            DemonstrateWhenEqualsAndOperatorDiffer();
            DemonstrateCustomValueTypeEquality();
            DemonstrateCustomReferenceTypeEquality();
            DemonstrateIEquatableInterface();
            DemonstrateHashCodeConsiderations();
            DemonstrateModernRecords();
            DemonstrateRealWorldScenarios();
            DemonstrateModernRecords();

            Console.WriteLine("\n=== End of Equality Comparison Demonstration ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region 1. Value vs Referential Equality
        /// <summary>
        /// Basic class for demonstrating referential equality
        /// Two instances with same data are still different objects in memory
        /// </summary>
        public class SimpleClass
        {
            public int Value { get; set; }
            
            public SimpleClass(int value)
            {
                Value = value;
            }
        }

        /// <summary>
        /// Demonstrates the fundamental difference between value and referential equality
        /// This is the foundation you need to understand before diving into complex scenarios
        /// </summary>
        static void DemonstrateValueVsReferentialEquality()
        {
            Console.WriteLine("1. === Value vs Referential Equality ===");
            
            // Value types always use value equality
            Console.WriteLine("Value types (value equality):");
            int x = 5, y = 5;
            Console.WriteLine($"int x = 5, y = 5");
            Console.WriteLine($"x == y: {x == y}"); // True - same values
            Console.WriteLine($"x.Equals(y): {x.Equals(y)}"); // True - same values
            
            // DateTime is also a value type
            DateTime dt1 = new DateTime(2023, 1, 1);
            DateTime dt2 = new DateTime(2023, 1, 1);
            Console.WriteLine($"\nDateTime comparison:");
            Console.WriteLine($"dt1 == dt2: {dt1 == dt2}"); // True - same moment
            Console.WriteLine($"dt1.Equals(dt2): {dt1.Equals(dt2)}"); // True - same moment
            
            // DateTimeOffset demonstrates complex value equality
            var dto1 = new DateTimeOffset(2023, 1, 1, 1, 0, 0, TimeSpan.FromHours(8));  // 1 AM UTC+8
            var dto2 = new DateTimeOffset(2023, 1, 1, 2, 0, 0, TimeSpan.FromHours(9));  // 2 AM UTC+9
            Console.WriteLine($"\nDateTimeOffset (same moment, different zones):");
            Console.WriteLine($"dto1: {dto1}");
            Console.WriteLine($"dto2: {dto2}");
            Console.WriteLine($"dto1 == dto2: {dto1 == dto2}"); // True - same moment in time!
            
            // Reference types use referential equality by default
            Console.WriteLine("\nReference types (referential equality):");
            SimpleClass obj1 = new SimpleClass(5);
            SimpleClass obj2 = new SimpleClass(5);
            Console.WriteLine($"obj1.Value = {obj1.Value}, obj2.Value = {obj2.Value}");
            Console.WriteLine($"obj1 == obj2: {obj1 == obj2}"); // False - different objects
            Console.WriteLine($"obj1.Equals(obj2): {obj1.Equals(obj2)}"); // False - different objects
            
            // Same reference = equal
            SimpleClass obj3 = obj1;
            Console.WriteLine($"obj3 = obj1 (same reference)");
            Console.WriteLine($"obj1 == obj3: {obj1 == obj3}"); // True - same object reference
            Console.WriteLine($"ReferenceEquals(obj1, obj3): {ReferenceEquals(obj1, obj3)}"); // True
            
            // Demonstrate the pitfall with boxing
            Console.WriteLine("\nBoxing pitfall with == operator:");
            object boxedX = 5;
            object boxedY = 5;
            Console.WriteLine($"object boxedX = 5, boxedY = 5");
            Console.WriteLine($"boxedX == boxedY: {boxedX == boxedY}"); // False - different boxed objects!
            Console.WriteLine($"boxedX.Equals(boxedY): {boxedX.Equals(boxedY)}"); // True - Int32.Equals called
            
            Console.WriteLine();
        }
        #endregion

        #region 2. Operator Equality (== and !=)
        /// <summary>
        /// Demonstrates how == and != operators work differently for various types
        /// The key insight: these operators are resolved at compile time based on the declared type
        /// </summary>
        static void DemonstrateOperatorEquality()
        {
            Console.WriteLine("2. === Operator Equality (== and !=) ===");
            
            // With primitive types, == works as expected
            Console.WriteLine("Primitive types:");
            int a = 10, b = 10;
            Console.WriteLine($"int a = 10, b = 10");
            Console.WriteLine($"a == b: {a == b}"); // True
            Console.WriteLine($"a != b: {a != b}"); // False
            
            // Boxing can cause unexpected results with ==
            Console.WriteLine("\nBoxing complications:");
            object boxedA = 5;
            object boxedB = 5;
            Console.WriteLine($"object boxedA = 5, boxedB = 5");
            Console.WriteLine($"boxedA == boxedB: {boxedA == boxedB}"); // False! Different boxed objects
            Console.WriteLine($"(int)boxedA == (int)boxedB: {(int)boxedA == (int)boxedB}"); // True - unboxed comparison
            
            // String is special - it overrides == for value equality
            Console.WriteLine("\nString equality (overridden == operator):");
            string str1 = "Hello";
            string str2 = "Hello";
            string str3 = "Hel" + "lo"; // Constructed at runtime
            Console.WriteLine($"str1: \"{str1}\"");
            Console.WriteLine($"str2: \"{str2}\"");
            Console.WriteLine($"str3: \"{str3}\" (constructed)");
            Console.WriteLine($"str1 == str2: {str1 == str2}"); // True
            Console.WriteLine($"str1 == str3: {str1 == str3}"); // True - value equality
            Console.WriteLine($"ReferenceEquals(str1, str2): {ReferenceEquals(str1, str2)}"); // True (interning)
            Console.WriteLine($"ReferenceEquals(str1, str3): {ReferenceEquals(str1, str3)}"); // Depends on interning
            
            // Uri also overrides == for value equality
            Console.WriteLine("\nUri equality (overridden == operator):");
            Uri uri1 = new Uri("https://example.com");
            Uri uri2 = new Uri("https://example.com");
            Console.WriteLine($"uri1 == uri2: {uri1 == uri2}"); // True - value equality
            Console.WriteLine($"ReferenceEquals(uri1, uri2): {ReferenceEquals(uri1, uri2)}"); // False - different objects
            
            // Demonstrating the compile-time nature of ==
            Console.WriteLine("\nCompile-time resolution of ==:");
            object objUri1 = new Uri("https://example.com");
            object objUri2 = new Uri("https://example.com");
            Console.WriteLine($"Same URIs cast to object:");
            Console.WriteLine($"objUri1 == objUri2: {objUri1 == objUri2}"); // False! Uses object's ==
            Console.WriteLine($"((Uri)objUri1) == ((Uri)objUri2): {((Uri)objUri1) == ((Uri)objUri2)}"); // True - uses Uri's ==
            
            Console.WriteLine();
        }
        #endregion

        #region 3. The Virtual Equals Method
        /// <summary>
        /// Demonstrates the Equals method behavior across different scenarios
        /// Unlike ==, Equals is resolved at runtime and can be overridden
        /// </summary>
        static void DemonstrateEqualsMethod()
        {
            Console.WriteLine("3. === The Virtual Equals Method ===");
            
            // Equals works well with boxed value types
            Console.WriteLine("Equals with boxed value types:");
            object boxedInt1 = 42;
            object boxedInt2 = 42;
            Console.WriteLine($"object boxedInt1 = 42, boxedInt2 = 42");
            Console.WriteLine($"boxedInt1 == boxedInt2: {boxedInt1 == boxedInt2}"); // False
            Console.WriteLine($"boxedInt1.Equals(boxedInt2): {boxedInt1.Equals(boxedInt2)}"); // True!
            
            // Equals is runtime-resolved, so it works correctly with polymorphism
            Console.WriteLine("\nEquals with different declared types:");
            object objString1 = "test";
            string actualString = "test";
            Console.WriteLine($"object objString1 = \"test\"");
            Console.WriteLine($"string actualString = \"test\"");
            Console.WriteLine($"objString1.Equals(actualString): {objString1.Equals(actualString)}"); // True
            Console.WriteLine($"actualString.Equals(objString1): {actualString.Equals(objString1)}"); // True
            
            // DateTime Equals behavior
            Console.WriteLine("\nDateTime Equals behavior:");
            DateTime date1 = new DateTime(2023, 12, 25);
            DateTime date2 = new DateTime(2023, 12, 25);
            object boxedDate = new DateTime(2023, 12, 25);
            
            Console.WriteLine($"date1.Equals(date2): {date1.Equals(date2)}"); // True
            Console.WriteLine($"date1.Equals(boxedDate): {date1.Equals(boxedDate)}"); // True
            Console.WriteLine($"boxedDate.Equals(date1): {boxedDate.Equals(date1)}"); // True
            
            // Reference type Equals (default behavior)
            Console.WriteLine("\nReference type Equals (default behavior):");
            SimpleClass ref1 = new SimpleClass(100);
            SimpleClass ref2 = new SimpleClass(100);
            SimpleClass ref3 = ref1;
            
            Console.WriteLine($"ref1.Equals(ref2): {ref1.Equals(ref2)}"); // False - different objects
            Console.WriteLine($"ref1.Equals(ref3): {ref1.Equals(ref3)}"); // True - same reference
            
            // Type safety with Equals
            Console.WriteLine("\nType safety with Equals:");
            Console.WriteLine($"date1.Equals(\"not a date\"): {date1.Equals("not a date")}"); // False
            Console.WriteLine($"42.Equals(\"42\"): {42.Equals("42")}"); // False
            
            Console.WriteLine();
        }
        #endregion

        #region 4. Object.Equals Static Method
        /// <summary>
        /// Demonstrates the null-safe Object.Equals static method
        /// This is your go-to method when dealing with potentially null values
        /// </summary>
        static void DemonstrateObjectEqualsStatic()
        {
            Console.WriteLine("4. === Object.Equals Static Method ===");
            
            // Object.Equals handles nulls gracefully
            Console.WriteLine("Null-safe comparisons:");
            
            object obj1 = 42;
            object obj2 = 42;
            object? obj3 = null;
            object? obj4 = null;
            
            Console.WriteLine($"object obj1 = 42, obj2 = 42, obj3 = null, obj4 = null");
            Console.WriteLine($"Object.Equals(obj1, obj2): {Object.Equals(obj1, obj2)}"); // True
            Console.WriteLine($"Object.Equals(obj1, obj3): {Object.Equals(obj1, obj3)}"); // False
            Console.WriteLine($"Object.Equals(obj3, obj4): {Object.Equals(obj3, obj4)}"); // True - both null
            
            // Comparison with manual null checking
            Console.WriteLine("\nManual null checking vs Object.Equals:");
            
            string str1 = "hello";
            string? str2 = null;
            string? str3 = null;
            
            // Manual approach (verbose and error-prone)
            bool manualResult1 = AreEqualManual(str1, str2);
            bool manualResult2 = AreEqualManual(str2, str3);
            
            // Object.Equals approach (clean and safe)
            bool staticResult1 = Object.Equals(str1, str2);
            bool staticResult2 = Object.Equals(str2, str3);
            
            Console.WriteLine($"str1 = \"hello\", str2 = null, str3 = null");
            Console.WriteLine($"Manual comparison str1 vs str2: {manualResult1}");
            Console.WriteLine($"Object.Equals(str1, str2): {staticResult1}");
            Console.WriteLine($"Manual comparison str2 vs str3: {manualResult2}");
            Console.WriteLine($"Object.Equals(str2, str3): {staticResult2}");
            
            // Works with value types too
            Console.WriteLine("\nWith value types:");
            int? nullable1 = 10;
            int? nullable2 = 10;
            int? nullable3 = null;
            
            Console.WriteLine($"int? nullable1 = 10, nullable2 = 10, nullable3 = null");
            Console.WriteLine($"Object.Equals(nullable1, nullable2): {Object.Equals(nullable1, nullable2)}"); // True
            Console.WriteLine($"Object.Equals(nullable1, nullable3): {Object.Equals(nullable1, nullable3)}"); // False
            Console.WriteLine($"Object.Equals(nullable3, null): {Object.Equals(nullable3, null)}"); // True
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Manual null-safe equality comparison - shows why Object.Equals is better
        /// </summary>
        static bool AreEqualManual(object? obj1, object? obj2)
        {
            if (obj1 == null) return obj2 == null;
            return obj1.Equals(obj2);
        }
        #endregion

        #region 5. Null Handling in Equality
        /// <summary>
        /// Comprehensive demonstration of null handling in equality comparisons
        /// Null handling is a common source of bugs, so understanding this is crucial
        /// </summary>
        static void DemonstrateNullHandling()
        {
            Console.WriteLine("5. === Null Handling in Equality ===");
            
            // The classic null reference exception trap
            Console.WriteLine("Null reference exception scenarios:");
            
            string? nullString = null;
            string validString = "test";
            
            // This would throw NullReferenceException:
            // nullString.Equals(validString) 
            
            Console.WriteLine("Safe null handling approaches:");
            
            // Approach 1: Check for null first
            bool result1 = nullString != null && nullString.Equals(validString);
            Console.WriteLine($"nullString != null && nullString.Equals(validString): {result1}");
            
            // Approach 2: Use Object.Equals (recommended)
            bool result2 = Object.Equals(nullString, validString);
            Console.WriteLine($"Object.Equals(nullString, validString): {result2}");
            
            // Approach 3: Call Equals on the non-null object (if you know which one)
            bool result3 = validString.Equals(nullString);
            Console.WriteLine($"validString.Equals(nullString): {result3}");
            
            // Demonstrating all null combinations
            Console.WriteLine("\nAll null combination scenarios:");
            
            object obj1 = "value";
            object obj2 = "value";
            object? obj3 = null;
            object? obj4 = null;
            
            var comparisons = new[]
            {
                (obj1, obj2, "value vs value"),
                (obj1, obj3, "value vs null"),
                (obj3, obj1, "null vs value"),
                (obj3, obj4, "null vs null")
            };
            
            foreach (var (o1, o2, description) in comparisons)
            {
                bool equalsResult = Object.Equals(o1, o2);
                bool operatorResult = o1 == o2;
                
                Console.WriteLine($"{description}:");
                Console.WriteLine($"  Object.Equals: {equalsResult}");
                Console.WriteLine($"  == operator: {operatorResult}");
            }
            
            // Nullable value types
            Console.WriteLine("\nNullable value types:");
            
            int? nullableInt1 = 5;
            int? nullableInt2 = 5;
            int? nullableInt3 = null;
            
            Console.WriteLine($"nullable int comparisons:");
            Console.WriteLine($"5 == 5: {nullableInt1 == nullableInt2}"); // True
            Console.WriteLine($"5 == null: {nullableInt1 == nullableInt3}"); // False
            Console.WriteLine($"null == null: {nullableInt3 == null}"); // True
            
            Console.WriteLine();
        }
        #endregion

        #region 6. String Equality Special Cases
        /// <summary>
        /// Strings have special equality behavior due to interning and cultural considerations
        /// Understanding string equality is essential for text processing applications
        /// </summary>
        static void DemonstrateStringEquality()
        {
            Console.WriteLine("6. === String Equality Special Cases ===");
            
            // String interning effects
            Console.WriteLine("String interning:");
            
            string literal1 = "Hello World";
            string literal2 = "Hello World";
            string constructed = "Hello" + " " + "World";
            string runtime = string.Concat("Hello", " ", "World");
            
            Console.WriteLine($"literal1: \"{literal1}\"");
            Console.WriteLine($"literal2: \"{literal2}\"");
            Console.WriteLine($"constructed: \"{constructed}\"");
            Console.WriteLine($"runtime: \"{runtime}\"");
            
            Console.WriteLine($"literal1 == literal2: {literal1 == literal2}"); // True
            Console.WriteLine($"literal1 == constructed: {literal1 == constructed}"); // True
            Console.WriteLine($"literal1 == runtime: {literal1 == runtime}"); // True
            
            Console.WriteLine($"ReferenceEquals(literal1, literal2): {ReferenceEquals(literal1, literal2)}"); // True (interned)
            Console.WriteLine($"ReferenceEquals(literal1, constructed): {ReferenceEquals(literal1, constructed)}"); // True (interned)
            Console.WriteLine($"ReferenceEquals(literal1, runtime): {ReferenceEquals(literal1, runtime)}"); // False (not interned)
            
            // Different string comparison methods
            Console.WriteLine("\nString comparison methods:");
            
            string upper = "HELLO";
            string lower = "hello";
            
            Console.WriteLine($"Comparing \"HELLO\" vs \"hello\":");
            Console.WriteLine($"upper == lower: {upper == lower}"); // False
            Console.WriteLine($"upper.Equals(lower): {upper.Equals(lower)}"); // False
            Console.WriteLine($"upper.Equals(lower, StringComparison.OrdinalIgnoreCase): {upper.Equals(lower, StringComparison.OrdinalIgnoreCase)}"); // True
            Console.WriteLine($"string.Equals(upper, lower, StringComparison.OrdinalIgnoreCase): {string.Equals(upper, lower, StringComparison.OrdinalIgnoreCase)}"); // True
            
            // Cultural string comparisons
            Console.WriteLine("\nCultural considerations:");
            
            string turkishI = "İstanbul"; // Turkish capital İ
            string regularI = "Istanbul";  // Regular I
            
            Console.WriteLine($"Turkish İ vs regular I:");
            Console.WriteLine($"turkishI.Equals(regularI): {turkishI.Equals(regularI)}"); // False
            Console.WriteLine($"turkishI.Equals(regularI, StringComparison.InvariantCultureIgnoreCase): {turkishI.Equals(regularI, StringComparison.InvariantCultureIgnoreCase)}"); // False (correct)
            
            // Empty string vs null
            Console.WriteLine("\nEmpty string vs null:");
            
            string emptyString = "";
            string? nullString = null;
            string whiteSpace = " ";
            
            Console.WriteLine($"emptyString == nullString: {emptyString == nullString}"); // False
            Console.WriteLine($"string.IsNullOrEmpty(emptyString): {string.IsNullOrEmpty(emptyString)}"); // True
            Console.WriteLine($"string.IsNullOrEmpty(nullString): {string.IsNullOrEmpty(nullString)}"); // True
            Console.WriteLine($"string.IsNullOrWhiteSpace(whiteSpace): {string.IsNullOrWhiteSpace(whiteSpace)}"); // True
            
            Console.WriteLine();
        }
        #endregion

        #region 6.5. When Equals and == Differ
        /// <summary>
        /// Demonstrates cases where Equals and == operators behave differently
        /// This is important to understand for edge cases and special types
        /// </summary>
        static void DemonstrateWhenEqualsAndOperatorDiffer()
        {
            Console.WriteLine("6.5. === When Equals and == Differ ===");
            
            // double.NaN example - the classic case
            Console.WriteLine("double.NaN behavior:");
            double nan = double.NaN;
            Console.WriteLine($"double.NaN == double.NaN: {nan == nan}"); // False - IEEE 754 spec
            Console.WriteLine($"double.NaN.Equals(double.NaN): {nan.Equals(nan)}"); // True - reflexive requirement
            Console.WriteLine("Reason: == follows IEEE 754 math rules, Equals must be reflexive for collections to work");
            
            // Another NaN example with variables
            double x = double.NaN;
            double y = double.NaN;
            Console.WriteLine($"x == y (both NaN): {x == y}"); // False
            Console.WriteLine($"x.Equals(y) (both NaN): {x.Equals(y)}"); // True
            
            // StringBuilder example
            Console.WriteLine("\nStringBuilder behavior:");
            var sb1 = new System.Text.StringBuilder("Hello World");
            var sb2 = new System.Text.StringBuilder("Hello World");
            
            Console.WriteLine($"sb1 == sb2: {sb1 == sb2}"); // False - referential equality
            Console.WriteLine($"sb1.Equals(sb2): {sb1.Equals(sb2)}"); // True - value equality
            Console.WriteLine("Reason: StringBuilder overrides Equals for value comparison but keeps == as reference comparison");
            
            // Demonstrating why this matters for collections
            Console.WriteLine("\nWhy this matters for collections:");
            
            // With double.NaN in collections
            var nanSet = new HashSet<double> { double.NaN };
            Console.WriteLine($"HashSet<double> contains NaN: {nanSet.Contains(double.NaN)}"); // True - uses Equals
            
            // With StringBuilder in collections
            var sbSet = new HashSet<System.Text.StringBuilder> { sb1 };
            Console.WriteLine($"HashSet<StringBuilder> contains sb2: {sbSet.Contains(sb2)}"); // True - uses Equals
            
            // Demonstrating the reflexive requirement
            Console.WriteLine("\nReflexive requirement demonstration:");
            var nanDict = new Dictionary<double, string> { { double.NaN, "Not a Number" } };
            Console.WriteLine($"Dictionary lookup with NaN key: {nanDict.ContainsKey(double.NaN)}"); // True - must work
            
            Console.WriteLine();
        }
        #endregion

        #region 7. Custom Value Type Equality
        /// <summary>
        /// Custom struct implementing proper equality comparison
        /// This shows the complete pattern for value types with IEquatable<T>
        /// </summary>
        public struct Area : IEquatable<Area>
        {
            public readonly int Width;
            public readonly int Height;
            
            public Area(int width, int height)
            {
                // Store dimensions in normalized order for consistent equality
                Width = Math.Min(width, height);
                Height = Math.Max(width, height);
            }
            
            // Strongly-typed Equals for performance
            public bool Equals(Area other)
            {
                return Width == other.Width && Height == other.Height;
            }
            
            // Override object.Equals for polymorphic scenarios
            public override bool Equals(object? obj)
            {
                return obj is Area other && Equals(other);
            }
            
            // GetHashCode must be consistent with Equals
            public override int GetHashCode()
            {
                return HashCode.Combine(Width, Height);
            }
            
            // Operator overloads for convenience
            public static bool operator ==(Area left, Area right)
            {
                return left.Equals(right);
            }
            
            public static bool operator !=(Area left, Area right)
            {
                return !(left == right);
            }
            
            public override string ToString()
            {
                return $"{Width}x{Height}";
            }
        }

        /// <summary>
        /// Demonstrates custom equality implementation for value types
        /// This is the gold standard for implementing equality in structs
        /// </summary>
        static void DemonstrateCustomValueTypeEquality()
        {
            Console.WriteLine("7. === Custom Value Type Equality ===");
            
            // Creating Area instances
            Area area1 = new Area(5, 10);
            Area area2 = new Area(10, 5);  // Should be equal (dimensions swapped)
            Area area3 = new Area(3, 7);
            Area area4 = new Area(5, 10);
            
            Console.WriteLine($"area1: {area1}");
            Console.WriteLine($"area2: {area2} (dimensions swapped)");
            Console.WriteLine($"area3: {area3}");
            Console.WriteLine($"area4: {area4}");
            
            // Testing equality methods
            Console.WriteLine("\nEquality comparisons:");
            Console.WriteLine($"area1 == area2: {area1 == area2}"); // True - same area
            Console.WriteLine($"area1 == area3: {area1 == area3}"); // False - different area
            Console.WriteLine($"area1 == area4: {area1 == area4}"); // True - identical
            
            Console.WriteLine($"area1.Equals(area2): {area1.Equals(area2)}"); // True
            Console.WriteLine($"area1.Equals(area3): {area1.Equals(area3)}"); // False
            
            // Testing with boxing
            Console.WriteLine("\nBoxed equality:");
            object boxedArea1 = area1;
            object boxedArea2 = area2;
            
            Console.WriteLine($"boxedArea1.Equals(boxedArea2): {boxedArea1.Equals(boxedArea2)}"); // True
            Console.WriteLine($"area1.Equals(boxedArea2): {area1.Equals(boxedArea2)}"); // True
            
            // Hash code consistency
            Console.WriteLine("\nHash code consistency:");
            Console.WriteLine($"area1.GetHashCode(): {area1.GetHashCode()}");
            Console.WriteLine($"area2.GetHashCode(): {area2.GetHashCode()}"); // Should be same
            Console.WriteLine($"area3.GetHashCode(): {area3.GetHashCode()}"); // Should be different
            
            // Using in collections
            Console.WriteLine("\nUsing in collections:");
            var areaSet = new HashSet<Area> { area1, area2, area3, area4 };
            Console.WriteLine($"HashSet count (should be 2): {areaSet.Count}"); // area1==area2==area4, so only 2 unique
            
            var areaDictionary = new Dictionary<Area, string>
            {
                [area1] = "First area",
                [area2] = "Second area (same as first)",
                [area3] = "Third area"
            };
            Console.WriteLine($"Dictionary count (should be 2): {areaDictionary.Count}");
            Console.WriteLine($"Value for area2: {areaDictionary[area2]}"); // Should be "Second area"
            
            Console.WriteLine();
        }
        #endregion

        #region 8. Custom Reference Type Equality
        /// <summary>
        /// Custom class implementing value-based equality instead of reference equality
        /// This is useful for entity classes where logical equality matters more than object identity
        /// </summary>
        public class Person : IEquatable<Person>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            
            public Person(string firstName, string lastName, DateTime dateOfBirth)
            {
                FirstName = firstName;
                LastName = lastName;
                DateOfBirth = dateOfBirth;
            }
            
            // Strongly-typed Equals
            public bool Equals(Person? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                
                return string.Equals(FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(LastName, other.LastName, StringComparison.OrdinalIgnoreCase) &&
                       DateOfBirth.Date == other.DateOfBirth.Date; // Compare only date part
            }
            
            public override bool Equals(object? obj)
            {
                return obj is Person other && Equals(other);
            }
            
            public override int GetHashCode()
            {
                return HashCode.Combine(
                    FirstName?.ToLowerInvariant(),
                    LastName?.ToLowerInvariant(),
                    DateOfBirth.Date);
            }
            
            // Operator overloads
            public static bool operator ==(Person? left, Person? right)
            {
                if (ReferenceEquals(left, right)) return true;
                if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
                return left.Equals(right);
            }
            
            public static bool operator !=(Person? left, Person? right)
            {
                return !(left == right);
            }
            
            public override string ToString()
            {
                return $"{FirstName} {LastName} (born {DateOfBirth:yyyy-MM-dd})";
            }
        }

        /// <summary>
        /// Demonstrates custom equality for reference types
        /// Shows how to override default referential equality with value-based equality
        /// </summary>
        static void DemonstrateCustomReferenceTypeEquality()
        {
            Console.WriteLine("8. === Custom Reference Type Equality ===");
            
            // Creating Person instances
            Person person1 = new Person("John", "Doe", new DateTime(1990, 5, 15));
            Person person2 = new Person("john", "doe", new DateTime(1990, 5, 15, 14, 30, 0)); // Same person, different case and time
            Person person3 = new Person("Jane", "Doe", new DateTime(1990, 5, 15));
            Person person4 = person1; // Same reference
            
            Console.WriteLine($"person1: {person1}");
            Console.WriteLine($"person2: {person2} (different case, includes time)");
            Console.WriteLine($"person3: {person3}");
            Console.WriteLine($"person4: same reference as person1");
            
            // Reference equality vs value equality
            Console.WriteLine("\nReference vs Value equality:");
            Console.WriteLine($"ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)}"); // False
            Console.WriteLine($"person1 == person2: {person1 == person2}"); // True - value equality
            Console.WriteLine($"person1.Equals(person2): {person1.Equals(person2)}"); // True
            
            Console.WriteLine($"ReferenceEquals(person1, person4): {ReferenceEquals(person1, person4)}"); // True
            Console.WriteLine($"person1 == person4: {person1 == person4}"); // True
            
            // Different persons
            Console.WriteLine($"person1 == person3: {person1 == person3}"); // False - different first name
            
            // Null handling
            Console.WriteLine("\nNull handling:");
            Person? nullPerson = null;
            Console.WriteLine($"person1 == null: {person1 == null}"); // False
            Console.WriteLine($"null == person1: {null == person1}"); // False
            Console.WriteLine($"nullPerson == null: {nullPerson == null}"); // True
            
            // Hash code consistency
            Console.WriteLine("\nHash code consistency:");
            Console.WriteLine($"person1.GetHashCode(): {person1.GetHashCode()}");
            Console.WriteLine($"person2.GetHashCode(): {person2.GetHashCode()}"); // Should be same
            Console.WriteLine($"person3.GetHashCode(): {person3.GetHashCode()}"); // Should be different
            
            // Using in collections
            Console.WriteLine("\nUsing in collections:");
            var personSet = new HashSet<Person> { person1, person2, person3 };
            Console.WriteLine($"HashSet count (should be 2): {personSet.Count}"); // person1 == person2
            
            var personDict = new Dictionary<Person, string>
            {
                [person1] = "First entry",
                [person2] = "Second entry (overwrites first)",
                [person3] = "Third entry"
            };
            Console.WriteLine($"Dictionary count (should be 2): {personDict.Count}");
            Console.WriteLine($"Value for person1: {personDict[person1]}"); // Should be "Second entry"
            
            Console.WriteLine();
        }
        #endregion

        #region 9. IEquatable<T> Interface Deep Dive
        /// <summary>
        /// Performance comparison struct to demonstrate IEquatable<T> benefits
        /// </summary>
        public struct PerformanceTest : IEquatable<PerformanceTest>
        {
            public int Value1 { get; }
            public int Value2 { get; }
            public int Value3 { get; }
            
            public PerformanceTest(int value1, int value2, int value3)
            {
                Value1 = value1;
                Value2 = value2;
                Value3 = value3;
            }
            
            // IEquatable<T> implementation - no boxing
            public bool Equals(PerformanceTest other)
            {
                return Value1 == other.Value1 && Value2 == other.Value2 && Value3 == other.Value3;
            }
            
            // Object.Equals override - handles boxing scenarios
            public override bool Equals(object? obj)
            {
                return obj is PerformanceTest other && Equals(other);
            }
            
            public override int GetHashCode()
            {
                return HashCode.Combine(Value1, Value2, Value3);
            }
        }

        /// <summary>
        /// Struct without IEquatable<T> for comparison
        /// </summary>
        public struct WithoutIEquatable
        {
            public int Value1 { get; }
            public int Value2 { get; }
            public int Value3 { get; }
            
            public WithoutIEquatable(int value1, int value2, int value3)
            {
                Value1 = value1;
                Value2 = value2;
                Value3 = value3;
            }
            
            public override bool Equals(object? obj)
            {
                return obj is WithoutIEquatable other && 
                       Value1 == other.Value1 && 
                       Value2 == other.Value2 && 
                       Value3 == other.Value3;
            }
            
            public override int GetHashCode()
            {
                return HashCode.Combine(Value1, Value2, Value3);
            }
        }

        /// <summary>
        /// Deep dive into IEquatable<T> interface and its benefits
        /// This shows why implementing IEquatable<T> is important for performance
        /// </summary>
        static void DemonstrateIEquatableInterface()
        {
            Console.WriteLine("9. === IEquatable<T> Interface Deep Dive ===");
            
            // Creating test instances
            var test1 = new PerformanceTest(1, 2, 3);
            var test2 = new PerformanceTest(1, 2, 3);
            var test3 = new PerformanceTest(4, 5, 6);
            
            var without1 = new WithoutIEquatable(1, 2, 3);
            var without2 = new WithoutIEquatable(1, 2, 3);
            
            Console.WriteLine("IEquatable<T> vs Object.Equals:");
            
            // Direct comparison - uses IEquatable<T>.Equals (fast)
            Console.WriteLine($"test1.Equals(test2): {test1.Equals(test2)}"); // No boxing
            
            // Boxed comparison - falls back to Object.Equals
            object boxedTest1 = test1;
            object boxedTest2 = test2;
            Console.WriteLine($"boxedTest1.Equals(boxedTest2): {boxedTest1.Equals(boxedTest2)}"); // Boxing occurred
            
            // Generic collections benefit from IEquatable<T>
            Console.WriteLine("\nGeneric collections performance:");
            
            var listWithIEquatable = new List<PerformanceTest> { test1, test2, test3 };
            var listWithoutIEquatable = new List<WithoutIEquatable> { without1, without2 };
            
            // These use IEquatable<T>.Equals - no boxing
            bool containsResult1 = listWithIEquatable.Contains(test1);
            int indexResult1 = listWithIEquatable.IndexOf(test2);
            
            // These use Object.Equals - boxing occurs
            bool containsResult2 = listWithoutIEquatable.Contains(without1);
            int indexResult2 = listWithoutIEquatable.IndexOf(without2);
            
            Console.WriteLine($"List<PerformanceTest>.Contains: {containsResult1}");
            Console.WriteLine($"List<PerformanceTest>.IndexOf: {indexResult1}");
            Console.WriteLine($"List<WithoutIEquatable>.Contains: {containsResult2}");
            Console.WriteLine($"List<WithoutIEquatable>.IndexOf: {indexResult2}");
            
            // Dictionary performance
            Console.WriteLine("\nDictionary operations:");
            
            var dictWithIEquatable = new Dictionary<PerformanceTest, string>
            {
                [test1] = "Value 1",
                [test3] = "Value 3"
            };
            
            var dictWithoutIEquatable = new Dictionary<WithoutIEquatable, string>
            {
                [without1] = "Value 1"
            };
            
            // Fast lookups with IEquatable<T>
            bool hasKey1 = dictWithIEquatable.ContainsKey(test2); // Uses IEquatable<T>
            bool hasKey2 = dictWithoutIEquatable.ContainsKey(without2); // Uses Object.Equals
            
            Console.WriteLine($"Dictionary<PerformanceTest>.ContainsKey: {hasKey1}");
            Console.WriteLine($"Dictionary<WithoutIEquatable>.ContainsKey: {hasKey2}");
            
            // Demonstrating type safety
            Console.WriteLine("\nType safety benefits:");
            
            // This won't compile - type safe
            // test1.Equals("string"); // Compile error
            
            // This compiles but will return false - not type safe
            bool objectEqualsResult = test1.Equals("string");
            Console.WriteLine($"test1.Equals(\"string\") via object.Equals: {objectEqualsResult}");
            
            // Generic constraint ensures type safety
            bool genericResult = AreEqualGeneric(test1, test2);
            Console.WriteLine($"Generic type-safe comparison: {genericResult}");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Generic method demonstrating type-safe equality comparison
        /// </summary>
        static bool AreEqualGeneric<T>(T item1, T item2) where T : IEquatable<T>
        {
            return item1.Equals(item2);
        }
        #endregion

        #region 10. Hash Code Considerations
        /// <summary>
        /// Class demonstrating proper hash code implementation
        /// </summary>
        public class Employee : IEquatable<Employee>
        {
            public int EmployeeId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Department { get; set; }
            
            public Employee(int id, string firstName, string lastName, string department)
            {
                EmployeeId = id;
                FirstName = firstName;
                LastName = lastName;
                Department = department;
            }
            
            public bool Equals(Employee? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                
                // Equality based on EmployeeId only (business key)
                return EmployeeId == other.EmployeeId;
            }
            
            public override bool Equals(object? obj)
            {
                return obj is Employee other && Equals(other);
            }
            
            public override int GetHashCode()
            {
                // Hash code must be based on the same fields used in Equals
                return EmployeeId.GetHashCode();
            }
            
            public override string ToString()
            {
                return $"Employee {EmployeeId}: {FirstName} {LastName} ({Department})";
            }
        }

        /// <summary>
        /// Bad example - inconsistent hash code implementation
        /// </summary>
        public class BadEmployee : IEquatable<BadEmployee>
        {
            public int EmployeeId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            
            public BadEmployee(int id, string firstName, string lastName)
            {
                EmployeeId = id;
                FirstName = firstName;
                LastName = lastName;
            }
            
            public bool Equals(BadEmployee? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return EmployeeId == other.EmployeeId; // Equals based on ID
            }
            
            public override bool Equals(object? obj)
            {
                return obj is BadEmployee other && Equals(other);
            }
            
            // BAD: Hash code based on different field than Equals!
            public override int GetHashCode()
            {
                return HashCode.Combine(FirstName, LastName); // Hash based on name
            }
        }

        /// <summary>
        /// Demonstrates the importance of consistent GetHashCode implementation
        /// GetHashCode must be consistent with Equals for hash-based collections to work properly
        /// </summary>
        static void DemonstrateHashCodeConsiderations()
        {
            Console.WriteLine("10. === Hash Code Considerations ===");
            
            // Good implementation
            Console.WriteLine("Good implementation (consistent Equals and GetHashCode):");
            
            var emp1 = new Employee(123, "John", "Doe", "IT");
            var emp2 = new Employee(123, "John", "Smith", "HR"); // Same ID, different details
            var emp3 = new Employee(456, "John", "Doe", "IT");   // Different ID, same details
            
            Console.WriteLine($"emp1: {emp1}");
            Console.WriteLine($"emp2: {emp2}");
            Console.WriteLine($"emp3: {emp3}");
            
            Console.WriteLine($"emp1.Equals(emp2): {emp1.Equals(emp2)}"); // True - same ID
            Console.WriteLine($"emp1.Equals(emp3): {emp1.Equals(emp3)}"); // False - different ID
            
            Console.WriteLine($"emp1.GetHashCode(): {emp1.GetHashCode()}");
            Console.WriteLine($"emp2.GetHashCode(): {emp2.GetHashCode()}"); // Same as emp1
            Console.WriteLine($"emp3.GetHashCode(): {emp3.GetHashCode()}"); // Different
            
            // Test with HashSet
            var employeeSet = new HashSet<Employee> { emp1, emp2, emp3 };
            Console.WriteLine($"HashSet count (should be 2): {employeeSet.Count}"); // emp1 == emp2
            
            // Test with Dictionary
            var employeeDict = new Dictionary<Employee, string>
            {
                [emp1] = "First entry",
                [emp2] = "Second entry (should overwrite)",
                [emp3] = "Third entry"
            };
            Console.WriteLine($"Dictionary count (should be 2): {employeeDict.Count}");
            Console.WriteLine($"Value for emp1: {employeeDict[emp1]}"); // Should be "Second entry"
            
            // Bad implementation
            Console.WriteLine("\nBad implementation (inconsistent Equals and GetHashCode):");
            
            var badEmp1 = new BadEmployee(123, "John", "Doe");
            var badEmp2 = new BadEmployee(123, "Jane", "Smith"); // Same ID, different name
            
            Console.WriteLine($"badEmp1: {badEmp1}");
            Console.WriteLine($"badEmp2: {badEmp2}");
            
            Console.WriteLine($"badEmp1.Equals(badEmp2): {badEmp1.Equals(badEmp2)}"); // True - same ID
            Console.WriteLine($"badEmp1.GetHashCode(): {badEmp1.GetHashCode()}");
            Console.WriteLine($"badEmp2.GetHashCode(): {badEmp2.GetHashCode()}"); // Different! This breaks hash collections
            
            // This will cause problems in hash-based collections
            var badEmployeeSet = new HashSet<BadEmployee> { badEmp1 };
            Console.WriteLine($"BadEmployee HashSet initially contains badEmp1: {badEmployeeSet.Contains(badEmp1)}"); // True
            
            // Add the "equal" employee
            badEmployeeSet.Add(badEmp2);
            Console.WriteLine($"HashSet count after adding equal employee: {badEmployeeSet.Count}"); // 2! Should be 1
            
            // Lookups may fail due to hash code inconsistency
            Console.WriteLine($"HashSet contains badEmp1 after adding badEmp2: {badEmployeeSet.Contains(badEmp1)}"); // May be false!
            
            // Hash code rules
            Console.WriteLine("\nHash code implementation rules:");
            Console.WriteLine("1. If obj1.Equals(obj2), then obj1.GetHashCode() == obj2.GetHashCode()");
            Console.WriteLine("2. Hash code should not change during object lifetime");
            Console.WriteLine("3. Hash code should be fast to compute");
            Console.WriteLine("4. Hash code should distribute values evenly");
            Console.WriteLine("5. Use only immutable fields in hash code calculation");
            
            Console.WriteLine();
        }
        #endregion

        #region 11. Real-World Scenarios
        /// <summary>
        /// Entity class representing a database record
        /// </summary>
        public class Customer : IEquatable<Customer>
        {
            public int CustomerId { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime LastModified { get; set; }
            
            public Customer(int customerId, string email, string firstName, string lastName)
            {
                CustomerId = customerId;
                Email = email;
                FirstName = firstName;
                LastName = lastName;
                CreatedDate = DateTime.Now;
                LastModified = DateTime.Now;
            }
            
            // Equality based on business key (CustomerId)
            public bool Equals(Customer? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return CustomerId == other.CustomerId;
            }
            
            public override bool Equals(object? obj)
            {
                return obj is Customer other && Equals(other);
            }
            
            // Hash code based on immutable business key
            public override int GetHashCode()
            {
                return CustomerId.GetHashCode();
            }
            
            public override string ToString()
            {
                return $"Customer {CustomerId}: {FirstName} {LastName} ({Email})";
            }
        }

        /// <summary>
        /// Value object for address comparison
        /// </summary>
        public readonly struct Address : IEquatable<Address>
        {
            public string Street { get; }
            public string City { get; }
            public string PostalCode { get; }
            public string Country { get; }
            
            public Address(string street, string city, string postalCode, string country)
            {
                Street = street ?? "";
                City = city ?? "";
                PostalCode = postalCode ?? "";
                Country = country ?? "";
            }
            
            public bool Equals(Address other)
            {
                return string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(PostalCode, other.PostalCode, StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase);
            }
            
            public override bool Equals(object? obj)
            {
                return obj is Address other && Equals(other);
            }
            
            public override int GetHashCode()
            {
                return HashCode.Combine(
                    Street?.ToLowerInvariant(),
                    City?.ToLowerInvariant(),
                    PostalCode?.ToLowerInvariant(),
                    Country?.ToLowerInvariant());
            }
            
            public static bool operator ==(Address left, Address right)
            {
                return left.Equals(right);
            }
            
            public static bool operator !=(Address left, Address right)
            {
                return !(left == right);
            }
            
            public override string ToString()
            {
                return $"{Street}, {City} {PostalCode}, {Country}";
            }
        }

        /// <summary>
        /// Demonstrates real-world equality scenarios
        /// These examples show how equality is used in practical applications
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("11. === Real-World Scenarios ===");
            
            // Scenario 1: Entity tracking in data access layer
            Console.WriteLine("Scenario 1: Entity tracking (ORM simulation):");
            
            var customer1 = new Customer(1001, "john@example.com", "John", "Doe");
            var customer2 = new Customer(1001, "john.doe@example.com", "Jonathan", "Doe"); // Same ID, different details
            var customer3 = new Customer(1002, "jane@example.com", "Jane", "Smith");
            
            // Simulate entity cache
            var entityCache = new HashSet<Customer> { customer1 };
            
            Console.WriteLine($"Initial cache contains: {customer1}");
            Console.WriteLine($"Attempting to add customer with same ID: {customer2}");
            
            bool wasAdded = entityCache.Add(customer2);
            Console.WriteLine($"Was customer2 added? {wasAdded}"); // False - already exists
            Console.WriteLine($"Cache count: {entityCache.Count}"); // Still 1
            
            // Update scenario
            if (entityCache.TryGetValue(customer2, out Customer? existingCustomer))
            {
                Console.WriteLine($"Found existing customer: {existingCustomer}");
                Console.WriteLine("Updating customer details...");
                existingCustomer.Email = customer2.Email;
                existingCustomer.FirstName = customer2.FirstName;
                existingCustomer.LastModified = DateTime.Now;
            }
            
            // Scenario 2: Value object comparison
            Console.WriteLine("\nScenario 2: Address comparison (value objects):");
            
            var address1 = new Address("123 Main St", "New York", "10001", "USA");
            var address2 = new Address("123 main st", "new york", "10001", "usa"); // Different case
            var address3 = new Address("456 Oak Ave", "Boston", "02101", "USA");
            
            Console.WriteLine($"address1: {address1}");
            Console.WriteLine($"address2: {address2} (different case)");
            Console.WriteLine($"address3: {address3}");
            
            Console.WriteLine($"address1 == address2: {address1 == address2}"); // True - case insensitive
            Console.WriteLine($"address1 == address3: {address1 == address3}"); // False
            
            // Using addresses as dictionary keys
            var deliveryInstructions = new Dictionary<Address, string>
            {
                [address1] = "Leave at front door",
                [address3] = "Ring doorbell twice"
            };
            
            // This should find the existing entry despite case differences
            if (deliveryInstructions.TryGetValue(address2, out string? instructions))
            {
                Console.WriteLine($"Found delivery instructions for address2: {instructions}");
            }
            
            // Scenario 3: Collection operations
            Console.WriteLine("\nScenario 3: Collection deduplication:");
            
            var customers = new List<Customer>
            {
                new Customer(1001, "john@example.com", "John", "Doe"),
                new Customer(1002, "jane@example.com", "Jane", "Smith"),
                new Customer(1001, "john.doe@company.com", "John", "Doe"), // Duplicate ID
                new Customer(1003, "bob@example.com", "Bob", "Johnson"),
                new Customer(1002, "jane.smith@company.com", "Jane", "Smith") // Duplicate ID
            };
            
            Console.WriteLine($"Original customer list count: {customers.Count}");
            
            // Remove duplicates using HashSet
            var uniqueCustomers = new HashSet<Customer>(customers);
            Console.WriteLine($"Unique customers count: {uniqueCustomers.Count}");
            
            // Find duplicates
            var duplicates = customers.GroupBy(c => c)
                                   .Where(g => g.Count() > 1)
                                   .Select(g => g.Key);
            
            Console.WriteLine("Duplicate customers found:");
            foreach (var duplicate in duplicates)
            {
                Console.WriteLine($"  {duplicate}");
            }
            
            // Scenario 4: Performance comparison
            Console.WriteLine("\nScenario 4: Performance considerations:");
            
            // Large collection operations
            var largeCustomerList = new List<Customer>();
            for (int i = 1; i <= 1000; i++)
            {
                largeCustomerList.Add(new Customer(i, $"user{i}@example.com", $"User{i}", "Test"));
            }
            
            var searchCustomer = new Customer(500, "different@email.com", "Different", "Name");
            
            // This uses IEquatable<Customer> for efficient comparison
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            bool found = largeCustomerList.Contains(searchCustomer);
            stopwatch.Stop();
            
            Console.WriteLine($"Contains operation on 1000 customers: {found}");
            Console.WriteLine($"Time taken: {stopwatch.ElapsedTicks} ticks");
            
            // HashSet lookup is much faster for large collections
            var customerHashSet = new HashSet<Customer>(largeCustomerList);
            
            stopwatch.Restart();
            bool foundInHashSet = customerHashSet.Contains(searchCustomer);
            stopwatch.Stop();
            
            Console.WriteLine($"HashSet lookup result: {foundInHashSet}");
            Console.WriteLine($"HashSet lookup time: {stopwatch.ElapsedTicks} ticks");
            
            Console.WriteLine();
        }
        #endregion

        #region 12. Modern C# Records (C# 9+)
        /// <summary>
        /// Records provide automatic equality implementation based on structural equality
        /// This demonstrates the modern approach to value-based equality
        /// </summary>
        public record ProductRecord(string Name, decimal Price, string Category)
        {
            // Records automatically implement:
            // - Equals(object? obj)
            // - Equals(ProductRecord? other)
            // - GetHashCode()
            // - == and != operators
            // - IEquatable<ProductRecord>
            
            // You can override the default equality if needed
            public virtual bool Equals(ProductRecord? other)
            {
                // Custom equality - ignore case for name and category
                return other is not null &&
                       string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
                       Price == other.Price &&
                       string.Equals(Category, other.Category, StringComparison.OrdinalIgnoreCase);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(
                    Name?.ToLowerInvariant(),
                    Price,
                    Category?.ToLowerInvariant());
            }
        }

        /// <summary>
        /// Regular record without custom equality - uses automatic structural equality
        /// </summary>
        public record PersonRecord(string FirstName, string LastName, int Age);

        /// <summary>
        /// Demonstrates modern C# records and their automatic equality implementation
        /// Records are perfect for value objects and DTOs
        /// </summary>
        static void DemonstrateModernRecords()
        {
            Console.WriteLine("12. === Modern C# Records (C# 9+) ===");
            
            // Basic record equality
            Console.WriteLine("Basic record equality:");
            var person1 = new PersonRecord("John", "Doe", 30);
            var person2 = new PersonRecord("John", "Doe", 30);
            var person3 = new PersonRecord("Jane", "Doe", 30);
            
            Console.WriteLine($"person1: {person1}");
            Console.WriteLine($"person2: {person2}");
            Console.WriteLine($"person3: {person3}");
            
            Console.WriteLine($"person1 == person2: {person1 == person2}"); // True - same values
            Console.WriteLine($"person1 == person3: {person1 == person3}"); // False - different name
            Console.WriteLine($"person1.Equals(person2): {person1.Equals(person2)}"); // True
            Console.WriteLine($"ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)}"); // False
            
            // Record with custom equality
            Console.WriteLine("\nRecord with custom equality:");
            var product1 = new ProductRecord("iPhone", 999.99m, "Electronics");
            var product2 = new ProductRecord("iphone", 999.99m, "electronics"); // Different case
            var product3 = new ProductRecord("iPhone", 1099.99m, "Electronics"); // Different price
            
            Console.WriteLine($"product1: {product1}");
            Console.WriteLine($"product2: {product2} (different case)");
            Console.WriteLine($"product3: {product3} (different price)");
            
            Console.WriteLine($"product1 == product2: {product1 == product2}"); // True - custom equality ignores case
            Console.WriteLine($"product1 == product3: {product1 == product3}"); // False - different price
            
            // Records in collections
            Console.WriteLine("\nRecords in collections:");
            var personSet = new HashSet<PersonRecord> { person1, person2, person3 };
            Console.WriteLine($"PersonRecord HashSet count: {personSet.Count}"); // 2 - person1 == person2
            
            var productSet = new HashSet<ProductRecord> { product1, product2, product3 };
            Console.WriteLine($"ProductRecord HashSet count: {productSet.Count}"); // 2 - product1 == product2
            
            // Record mutation (with expression)
            Console.WriteLine("\nRecord mutation with 'with' expression:");
            var updatedPerson = person1 with { Age = 31 };
            Console.WriteLine($"Original: {person1}");
            Console.WriteLine($"Updated: {updatedPerson}");
            Console.WriteLine($"Are they equal? {person1 == updatedPerson}"); // False - different age
            
            // Hash codes
            Console.WriteLine("\nHash code consistency:");
            Console.WriteLine($"person1.GetHashCode(): {person1.GetHashCode()}");
            Console.WriteLine($"person2.GetHashCode(): {person2.GetHashCode()}"); // Same
            Console.WriteLine($"product1.GetHashCode(): {product1.GetHashCode()}");
            Console.WriteLine($"product2.GetHashCode(): {product2.GetHashCode()}"); // Same due to custom implementation
            
            Console.WriteLine();
        }
        #endregion
    }
}
