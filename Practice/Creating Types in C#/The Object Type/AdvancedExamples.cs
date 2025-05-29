using System;
using System.Collections.Generic;

namespace TheObjectType
{
    /// <summary>
    /// Advanced examples and edge cases with the object type
    /// These examples show real-world scenarios and common pitfalls
    /// </summary>
    public static class AdvancedObjectExamples
    {
        /// <summary>
        /// Demonstrates performance implications of boxing/unboxing
        /// Boxing creates heap allocations - can impact performance in loops!
        /// </summary>
        public static void PerformanceExample()
        {
            Console.WriteLine("=== Performance Impact of Boxing ===");
            
            // BAD: Boxing in every iteration (slow!)
            var objectList = new List<object>();
            Console.WriteLine("Boxing 1000 integers (creates 1000 heap objects):");
            
            var start = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                objectList.Add(i);  // Boxing happens here!
            }
            var boxingTime = DateTime.Now - start;
            
            // GOOD: Using generics (fast!)
            var intList = new List<int>();
            Console.WriteLine("Adding 1000 integers to List<int> (no boxing):");
            
            start = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                intList.Add(i);  // No boxing - direct storage!
            }
            var genericTime = DateTime.Now - start;
            
            Console.WriteLine($"Boxing time: {boxingTime.TotalMilliseconds:F2}ms");
            Console.WriteLine($"Generic time: {genericTime.TotalMilliseconds:F2}ms");
            Console.WriteLine("Lesson: Use generics when you know the type!");
        }
        
        /// <summary>
        /// Shows common boxing/unboxing mistakes and how to avoid them
        /// </summary>
        public static void CommonMistakes()
        {
            Console.WriteLine("\n=== Common Boxing/Unboxing Mistakes ===");
            
            // Mistake 1: Wrong type unboxing
            object boxedInt = 42;
            try
            {
                long wrongUnbox = (long)boxedInt;  // WRONG!
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("❌ Can't directly cast boxed int to long");
                long correctUnbox = (long)(int)boxedInt;  // CORRECT!
                Console.WriteLine($"✅ Correct way: {correctUnbox}");
            }
            
            // Mistake 2: Nullable types boxing
            int? nullableInt = null;
            object boxedNullable = nullableInt;  // Boxes to null!
            Console.WriteLine($"Boxed nullable: {boxedNullable ?? "null"}");
            
            nullableInt = 42;
            boxedNullable = nullableInt;  // Boxes the value, not the nullable!
            int unboxedValue = (int)boxedNullable;  // Direct unbox to int works
            Console.WriteLine($"Nullable boxing result: {unboxedValue}");
            
            // Mistake 3: Enum boxing
            DayOfWeek today = DayOfWeek.Monday;
            object boxedEnum = today;  // Boxing enum
            
            // You can unbox to enum or its underlying type
            DayOfWeek unboxedEnum = (DayOfWeek)boxedEnum;
            int enumAsInt = (int)boxedEnum;
            Console.WriteLine($"Enum unboxed: {unboxedEnum}, As int: {enumAsInt}");
        }
        
        /// <summary>
        /// Demonstrates object comparison nuances
        /// Reference vs value equality can be tricky with object!
        /// </summary>
        public static void ObjectComparison()
        {
            Console.WriteLine("\n=== Object Comparison Nuances ===");
            
            // Value types - boxing creates new objects
            int value1 = 42;
            int value2 = 42;
            object obj1 = value1;  // Boxing
            object obj2 = value2;  // Another boxing
            
            Console.WriteLine($"obj1 == obj2: {obj1 == obj2}");  // False! Different objects
            Console.WriteLine($"obj1.Equals(obj2): {obj1.Equals(obj2)}");  // True! Same value
            Console.WriteLine($"ReferenceEquals(obj1, obj2): {ReferenceEquals(obj1, obj2)}");  // False!
            
            // String interning - special case
            object str1 = "Hello";
            object str2 = "Hello";
            Console.WriteLine($"\nString references equal: {ReferenceEquals(str1, str2)}");  // True! (interned)
            
            // Custom objects
            object person1 = new Person("Alice", 25);
            object person2 = new Person("Alice", 25);
            Console.WriteLine($"Person references equal: {ReferenceEquals(person1, person2)}");  // False
            Console.WriteLine($"Person values equal: {person1.Equals(person2)}");  // True (we overrode Equals)
        }
        
        /// <summary>
        /// Shows advanced type checking techniques
        /// </summary>
        public static void AdvancedTypeChecking()
        {
            Console.WriteLine("\n=== Advanced Type Checking ===");
            
            object[] mixedArray = { 42, "Hello", new Person("Bob", 30), 3.14, new Dog("Labrador") };
            
            Console.WriteLine("Analyzing mixed array:");
            foreach (object item in mixedArray)
            {
                // Multiple ways to check types
                Type itemType = item.GetType();
                
                Console.WriteLine($"\nItem: {item}");
                Console.WriteLine($"  Type: {itemType.Name}");
                Console.WriteLine($"  Full name: {itemType.FullName}");
                Console.WriteLine($"  Is value type: {itemType.IsValueType}");
                Console.WriteLine($"  Is reference type: {!itemType.IsValueType}");
                
                // Pattern matching (modern C#)
                string description = item switch
                {
                    int i => $"Integer: {i}",
                    string s => $"String with {s.Length} characters",
                    Person p => $"Person named {p.Name}",
                    Dog d => $"Dog of breed {d.Breed}",
                    double d => $"Double: {d:F2}",
                    _ => "Unknown type"
                };
                Console.WriteLine($"  Pattern match: {description}");
                
                // Type hierarchy checking
                if (typeof(Animal).IsAssignableFrom(itemType))
                {
                    Console.WriteLine($"  Inherits from Animal: Yes");
                }
            }
        }
        
        /// <summary>
        /// Demonstrates practical object collections
        /// How object was used before generics for heterogeneous collections
        /// </summary>
        public static void HeterogeneousCollections()
        {
            Console.WriteLine("\n=== Heterogeneous Collections ===");
            
            // Before generics, this was common
            var properties = new Dictionary<string, object>
            {
                ["Name"] = "John Doe",
                ["Age"] = 35,
                ["IsMarried"] = true,
                ["Salary"] = 75000.50m,
                ["Skills"] = new string[] { "C#", "SQL", "JavaScript" }
            };
            
            Console.WriteLine("Configuration/Properties dictionary:");
            foreach (var kvp in properties)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} ({kvp.Value.GetType().Name})");
            }
            
            // Safe retrieval with type checking
            if (properties["Name"] is string name)
            {
                Console.WriteLine($"\nName retrieved safely: {name}");
            }
            
            if (properties["Skills"] is string[] skills)
            {
                Console.WriteLine($"Skills: {string.Join(", ", skills)}");
            }
            
            // Modern approach would use generics or dynamic
            Console.WriteLine("\nModern alternatives:");
            Console.WriteLine("- Use Dictionary<string, T> for known types");
            Console.WriteLine("- Use dynamic for flexible typing");
            Console.WriteLine("- Use custom classes with properties");
        }
    }
}
