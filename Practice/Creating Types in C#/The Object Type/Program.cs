using System;
using System.Collections.Generic;

namespace TheObjectType
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== THE OBJECT TYPE: Foundation of C# Type System ===");
            Console.WriteLine("Everything in C# inherits from System.Object");
            Console.WriteLine("This creates a unified type system with incredible flexibility\n");

            // Step through each core concept systematically
            Console.WriteLine("Press any key to start exploring...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateUniversalBaseClass();
            Console.WriteLine("\nPress any key for Stack implementation...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateStackWithObjectType();
            Console.WriteLine("\nPress any key for Boxing and Unboxing...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateBoxingAndUnboxing();
            Console.WriteLine("\nPress any key for Type Checking...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateTypeChecking();
            Console.WriteLine("\nPress any key for GetType and typeof...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateGetTypeAndTypeof();
            Console.WriteLine("\nPress any key for ToString method...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateToStringMethod();
            Console.WriteLine("\nPress any key for Object Members...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateObjectMembers();
            Console.WriteLine("\nPress any key for Real-World Examples...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== TRAINING COMPLETE ===");
            Console.WriteLine("You now understand how object type unifies the entire C# type system!");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates that every type in C# derives from object
        /// This is the foundation that enables polymorphism and type flexibility
        /// </summary>
        static void DemonstrateUniversalBaseClass()
        {
            Console.WriteLine("=== 1. OBJECT AS UNIVERSAL BASE CLASS ===");
            Console.WriteLine("Every type in C# inherits from System.Object (alias: object)");
            Console.WriteLine("This creates a unified type system\n");

            // Value types can be treated as objects
            object boxedInt = 42;
            object boxedDouble = 3.14;
            object boxedBool = true;
            object boxedChar = 'A';

            // Reference types are naturally objects
            object stringObj = "Hello World";
            object arrayObj = new int[] { 1, 2, 3 };
            object listObj = new List<string> { "C#", "Java", "Python" };

            // Custom types also inherit from object
            object personObj = new Person("John", 30);

            Console.WriteLine("All these different types stored as 'object':");
            Console.WriteLine($"Integer (value type): {boxedInt} - Type: {boxedInt.GetType().Name}");
            Console.WriteLine($"Double (value type): {boxedDouble} - Type: {boxedDouble.GetType().Name}");
            Console.WriteLine($"Boolean (value type): {boxedBool} - Type: {boxedBool.GetType().Name}");
            Console.WriteLine($"Character (value type): {boxedChar} - Type: {boxedChar.GetType().Name}");
            Console.WriteLine($"String (reference type): {stringObj} - Type: {stringObj.GetType().Name}");
            Console.WriteLine($"Array (reference type): Length {((int[])arrayObj).Length} - Type: {arrayObj.GetType().Name}");
            Console.WriteLine($"List (reference type): Count {((List<string>)listObj).Count} - Type: {listObj.GetType().Name}");
            Console.WriteLine($"Custom class: {personObj} - Type: {personObj.GetType().Name}");

            Console.WriteLine("\nThe power: We can store ANY type in object variables!");
            Console.WriteLine("This enables generic collections, polymorphism, and flexible APIs");
        }

        /// <summary>
        /// Shows how object type enables creation of general-purpose data structures
        /// Before generics, this was the primary way to create flexible containers
        /// </summary>
        static void DemonstrateStackWithObjectType()
        {
            Console.WriteLine("=== 2. STACK WITH OBJECT TYPE ===");
            Console.WriteLine("Before generics, all collections used object as storage type");
            Console.WriteLine("This allowed storing mixed types in the same container\n");

            var stack = new SimpleStack();

            // Push different types onto the stack
            Console.WriteLine("Pushing various types onto the stack:");
            stack.Push("sausage");                      // string
            stack.Push(3);                              // int (will be boxed)
            stack.Push(true);                           // bool (will be boxed)
            stack.Push(new Person("Alice", 25));        // custom object
            stack.Push(new DateTime(2023, 12, 25));     // struct (will be boxed)

            Console.WriteLine($"\nStack now contains {stack.Count} items");
            stack.DisplayContents();

            // Pop items back (note: casting required)
            Console.WriteLine("\nPopping items (demonstrating downcasting):");
            DateTime date = (DateTime)stack.Pop();      // Cast back to DateTime
            Person person = (Person)stack.Pop();        // Cast back to Person
            bool flag = (bool)stack.Pop();              // Cast back to bool
            int number = (int)stack.Pop();              // Cast back to int
            string text = (string)stack.Pop();          // Cast back to string

            Console.WriteLine($"Retrieved: {text}, {number}, {flag}, {person}, {date:yyyy-MM-dd}");
            Console.WriteLine("\nKey lesson: object enables type flexibility but requires explicit casting");
        }

        /// <summary>
        /// Comprehensive demonstration of boxing and unboxing
        /// Critical concept for understanding value type behavior with object
        /// </summary>
        static void DemonstrateBoxingAndUnboxing()
        {
            Console.WriteLine("=== 3. BOXING AND UNBOXING ===");
            Console.WriteLine("Boxing = value type → reference type (object)");
            Console.WriteLine("Unboxing = reference type (object) → value type\n");

            // Boxing demonstration
            Console.WriteLine("BOXING EXAMPLES:");
            int originalValue = 42;
            Console.WriteLine($"Original value: {originalValue} (stored on stack)");

            // Implicit boxing - value type to object
            object boxedValue = originalValue;  // Boxing occurs here!
            Console.WriteLine($"After boxing: {boxedValue} (now stored on heap as object)");
            Console.WriteLine($"Are they equal? {originalValue.Equals(boxedValue)}"); // True - same value
            Console.WriteLine("Boxing creates a separate object on the heap"); // Explanation instead of showing reference inequality
            
            // Demonstrate that boxing creates a copy
            originalValue = 99;
            Console.WriteLine($"Original changed to: {originalValue}");
            Console.WriteLine($"Boxed value remains: {boxedValue} (independent copy)");

            Console.WriteLine("\nUNBOXING EXAMPLES:");
            // Explicit unboxing - object back to value type
            int unboxedValue = (int)boxedValue;  // Unboxing occurs here!
            Console.WriteLine($"Unboxed value: {unboxedValue}");

            // Unboxing type safety - must match exactly
            Console.WriteLine("\nType safety in unboxing:");
            object boxedInt = 123;
            try
            {
                int correctCast = (int)boxedInt;        // Works - exact type match
                Console.WriteLine($"Correct cast: {correctCast}");
                
                // This would fail - can't unbox int as long directly
                // long wrongCast = (long)boxedInt;     // InvalidCastException!
                
                // But this works - unbox then convert
                long validConversion = (long)(int)boxedInt;
                Console.WriteLine($"Valid conversion: {validConversion}");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Cast error: {ex.Message}");
            }

            // Performance implications
            Console.WriteLine("\nPERFORMANCE CONSIDERATIONS:");
            Console.WriteLine("Boxing/unboxing has overhead - allocation + copying");
            Console.WriteLine("Avoid in performance-critical loops");
            Console.WriteLine("Generics (List<T>) solve this by avoiding boxing");

            // ToString() and boxing
            Console.WriteLine("\nTOSTRING AND BOXING:");
            int directValue = 456;
            string result1 = directValue.ToString();     // No boxing
            object boxedValue2 = directValue;
            string result2 = boxedValue2?.ToString() ?? "null";     // No additional boxing
            Console.WriteLine($"Both results identical: {result1 == result2}");
        }

        /// <summary>
        /// Shows the difference between compile-time and runtime type checking
        /// Demonstrates when and why runtime type validation occurs
        /// </summary>
        static void DemonstrateTypeChecking()
        {
            Console.WriteLine("=== 4. STATIC vs RUNTIME TYPE CHECKING ===");
            Console.WriteLine("C# checks types at compile-time AND runtime for safety\n");

            // Compile-time type checking
            Console.WriteLine("COMPILE-TIME CHECKING:");
            Console.WriteLine("// int x = \"hello\";  // Compiler error - type mismatch");
            Console.WriteLine("// string s = 123;    // Compiler error - no implicit conversion");

            // Runtime type checking with objects
            Console.WriteLine("\nRUNTIME CHECKING (with object variables):");
            object obj1 = "Hello World";
            object obj2 = 42;
            object obj3 = new Person("Bob", 30);

            Console.WriteLine("Testing different runtime casts:");

            // Safe cast - will work
            try
            {
                string str = (string)obj1;  // Runtime checks: is obj1 really a string?
                Console.WriteLine($"✓ Successfully cast to string: {str}");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"✗ Cast failed: {ex.Message}");
            }

            // Unsafe cast - will fail
            try
            {
                int num = (int)obj1;        // Runtime checks: is obj1 really an int?
                Console.WriteLine($"This won't print: {num}");
            }
            catch (InvalidCastException)
            {
                Console.WriteLine($"✗ Expected failure: Cannot cast string to int");
            }

            // Safe ways to check types
            Console.WriteLine("\nSAFE TYPE CHECKING:");
            
            // Using 'is' operator
            if (obj2 is int)
            {
                int safeNum = (int)obj2;
                Console.WriteLine($"✓ obj2 is an int: {safeNum}");
            }

            // Using 'as' operator (returns null if cast fails)
            string? asString = obj2 as string;
            if (asString != null)
            {
                Console.WriteLine($"obj2 as string: {asString}");
            }
            else
            {
                Console.WriteLine("✓ obj2 is not a string (as operator returned null)");
            }

            // Pattern matching (modern C#)
            Console.WriteLine("\nPATTERN MATCHING:");
            if (obj3 is Person p)
            {
                Console.WriteLine($"✓ obj3 is a Person: {p.Name}, {p.Age}");
            }

            Console.WriteLine("\nKey insight: Runtime checking enables safe downcasting");
        }

        /// <summary>
        /// Demonstrates GetType() vs typeof() for type introspection
        /// Shows how to examine types at runtime vs compile-time
        /// </summary>
        static void DemonstrateGetTypeAndTypeof()
        {
            Console.WriteLine("=== 5. GETTYPE() vs TYPEOF() ===");
            Console.WriteLine("Two ways to get Type information in C#\n");

            var person = new Person("Alice", 30);
            var product = new Product("Laptop", 999.99m);
            int number = 42;

            Console.WriteLine("GetType() - Runtime type of instances:");
            Console.WriteLine($"person.GetType(): {person.GetType().Name}");
            Console.WriteLine($"product.GetType(): {product.GetType().Name}");
            Console.WriteLine($"number.GetType(): {number.GetType().Name}");

            Console.WriteLine("\ntypeof() - Compile-time type information:");
            Console.WriteLine($"typeof(Person): {typeof(Person).Name}");
            Console.WriteLine($"typeof(Product): {typeof(Product).Name}");
            Console.WriteLine($"typeof(int): {typeof(int).Name}");
            Console.WriteLine($"typeof(object): {typeof(object).Name}");

            Console.WriteLine("\nType comparison:");
            Console.WriteLine($"person.GetType() == typeof(Person): {person.GetType() == typeof(Person)}");
            Console.WriteLine($"number.GetType() == typeof(int): {number.GetType() == typeof(int)}");

            Console.WriteLine($"Person inherits from object: {typeof(object).IsAssignableFrom(typeof(Person))}");
            Console.WriteLine($"int inherits from object: {typeof(object).IsAssignableFrom(typeof(int))}");

            // Detailed type information
            Console.WriteLine("\nDetailed type information:");
            Type personType = typeof(Person);
            Console.WriteLine($"Full name: {personType.FullName}");
            Console.WriteLine($"Namespace: {personType.Namespace}");
            Console.WriteLine($"Assembly: {personType.Assembly.GetName().Name}");
            Console.WriteLine($"Base type: {personType.BaseType?.Name}");
            Console.WriteLine($"Is class: {personType.IsClass}");
            Console.WriteLine($"Is value type: {personType.IsValueType}");

            Console.WriteLine("\nPractical use: Type-based logic");
            object[] mixed = { person, product, number, "hello", true };
            foreach (object item in mixed)
            {
                Console.WriteLine($"Type: {item.GetType().Name}, Value: {item}");
            }
        }

        /// <summary>
        /// Demonstrates ToString() override and its importance
        /// Shows default behavior vs custom implementations
        /// </summary>
        static void DemonstrateToStringMethod()
        {
            Console.WriteLine("=== 6. TOSTRING() METHOD ===");
            Console.WriteLine("Every object has ToString() - but the quality varies!\n");

            // Built-in types have good ToString() implementations
            Console.WriteLine("Built-in types with good ToString():");
            Console.WriteLine($"int: {42.ToString()}");
            Console.WriteLine($"double: {3.14159.ToString()}");
            Console.WriteLine($"bool: {true.ToString()}");
            Console.WriteLine($"DateTime: {DateTime.Now.ToString()}");

            // Custom classes - before and after ToString() override
            var person = new Person("Bob", 35);
            var product = new Product("Laptop", 999.99m);

            Console.WriteLine("\nCustom classes with ToString() override:");
            Console.WriteLine($"Person: {person}");        // Calls overridden ToString()
            Console.WriteLine($"Product: {product}");      // Calls overridden ToString()

            // Demonstrate classes without ToString() override
            var pandaWithout = new PandaWithoutToString { Name = "Ping" };
            var pandaWith = new PandaWithToString { Name = "Pong" };

            Console.WriteLine("\nComparison - with vs without ToString() override:");
            Console.WriteLine($"Without override: {pandaWithout}");  // Shows type name
            Console.WriteLine($"With override: {pandaWith}");        // Shows meaningful info

            // ToString() is called automatically in many places
            Console.WriteLine("\nAutomatic ToString() calls:");
            Console.WriteLine($"String interpolation: {person}");
            Console.WriteLine($"String concatenation: " + person);
            Console.WriteLine("Console.WriteLine: " + person);

            Console.WriteLine("\nBest practice: Always override ToString() for better debugging!");
        }

        /// <summary>
        /// Explores the key members inherited from System.Object
        /// Shows practical usage of Equals, GetHashCode, ReferenceEquals, etc.
        /// </summary>
        static void DemonstrateObjectMembers()
        {
            Console.WriteLine("=== 7. OBJECT MEMBERS ===");
            Console.WriteLine("All types inherit these important members from object\n");

            var person1 = new Person("Charlie", 25);
            var person2 = new Person("Charlie", 25);
            var person3 = person1;  // Same reference

            // Equals() method
            Console.WriteLine("Equals() method:");
            Console.WriteLine($"person1.Equals(person2): {person1.Equals(person2)}");  // True (overridden)
            Console.WriteLine($"person1.Equals(person3): {person1.Equals(person3)}");  // True (same ref)
            Console.WriteLine($"person1.Equals(\"not a person\"): {person1.Equals("not a person")}");  // False

            // Static Object.Equals() - null-safe
            Console.WriteLine("\nStatic Object.Equals() (null-safe):");
            Person? nullPerson = null;
            Console.WriteLine($"Object.Equals(person1, person2): {Object.Equals(person1, person2)}");
            Console.WriteLine($"Object.Equals(person1, nullPerson): {Object.Equals(person1, nullPerson)}");
            Console.WriteLine($"Object.Equals(nullPerson, nullPerson): {Object.Equals(nullPerson, nullPerson)}");

            // ReferenceEquals() - checks for same object instance
            Console.WriteLine("\nReferenceEquals() - identity comparison:");
            Console.WriteLine($"ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)}");  // False
            Console.WriteLine($"ReferenceEquals(person1, person3): {ReferenceEquals(person1, person3)}");  // True

            // GetHashCode() method
            Console.WriteLine("\nGetHashCode() method:");
            Console.WriteLine($"person1.GetHashCode(): {person1.GetHashCode()}");
            Console.WriteLine($"person2.GetHashCode(): {person2.GetHashCode()}");
            Console.WriteLine($"Equal objects have same hash: {person1.GetHashCode() == person2.GetHashCode()}");

            // MemberwiseClone() demonstration
            Console.WriteLine("\nMemberwiseClone() - shallow copy:");
            var clonedPerson = person1.CreateShallowCopy();
            Console.WriteLine($"Original: {person1}");
            Console.WriteLine($"Clone: {clonedPerson}");
            Console.WriteLine($"Are they the same reference? {ReferenceEquals(person1, clonedPerson)}");
            Console.WriteLine($"Are they equal? {person1.Equals(clonedPerson)}");

            Console.WriteLine("\nThese methods work together to enable:");
            Console.WriteLine("- Hash tables (Dictionary, HashSet)");
            Console.WriteLine("- Equality comparisons");
            Console.WriteLine("- Object copying");
            Console.WriteLine("- Collection membership testing");
        }

        /// <summary>
        /// Shows real-world applications of object type concepts
        /// Demonstrates practical scenarios where these concepts matter
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("=== 8. REAL-WORLD SCENARIOS ===");
            Console.WriteLine("Where object type knowledge matters in practice\n");

            // Scenario 1: Configuration management
            Console.WriteLine("SCENARIO 1: Configuration Management");
            var config = new ConfigurationManager();
            config.SetValue("server.port", 8080);
            config.SetValue("server.host", "localhost");
            config.SetValue("debug.enabled", true);
            config.SetValue("timeout.seconds", 30.5);

            Console.WriteLine("Configuration values (all stored as object):");
            Console.WriteLine($"Port: {config.GetValue<object>("server.port")} ({config.GetValue<object>("server.port")?.GetType().Name})");
            Console.WriteLine($"Host: {config.GetValue<object>("server.host")} ({config.GetValue<object>("server.host")?.GetType().Name})");
            Console.WriteLine($"Debug: {config.GetValue<object>("debug.enabled")} ({config.GetValue<object>("debug.enabled")?.GetType().Name})");
            Console.WriteLine($"Timeout: {config.GetValue<object>("timeout.seconds")} ({config.GetValue<object>("timeout.seconds")?.GetType().Name})");

            // Scenario 2: Logging system
            Console.WriteLine("\nSCENARIO 2: Logging System");
            var logger = new SimpleLogger();
            logger.Log("Application started");
            logger.Log(404);
            logger.Log(new Exception("Something went wrong"));
            logger.Log(new { UserId = 123, Action = "Login" });
            
            Console.WriteLine("All logged messages:");
            logger.DisplayLogs();

            // Scenario 3: Before generics - ArrayList equivalent
            Console.WriteLine("\nSCENARIO 3: Pre-Generics Collections (Historical)");
            var legacyList = new List<object>();  // Simulating old ArrayList
            legacyList.Add("Text data");
            legacyList.Add(42);
            legacyList.Add(new Person("Dave", 40));
            legacyList.Add(DateTime.Now);

            Console.WriteLine("Mixed-type collection (like old ArrayList):");
            foreach (object item in legacyList)
            {
                // This is why generics were needed - no type safety!
                Console.WriteLine($"  {item} ({item.GetType().Name})");
            }

            // Scenario 4: API that accepts any type
            Console.WriteLine("\nSCENARIO 4: Generic API Parameters");
            ProcessData("String data");
            ProcessData(12345);
            ProcessData(new Person("Emma", 28));
            ProcessData(new[] { 1, 2, 3 });

            Console.WriteLine("\nKey takeaways:");
            Console.WriteLine("- object enables flexible APIs");
            Console.WriteLine("- Boxing/unboxing affects performance");
            Console.WriteLine("- Type checking ensures safety");
            Console.WriteLine("- Generics often provide better solutions");
            Console.WriteLine("- Understanding object helps debug type issues");
        }

        /// <summary>
        /// Helper method for demonstrating flexible API design
        /// Shows how object parameter enables accepting any type
        /// </summary>
        static void ProcessData(object data)
        {
            if (data == null)
            {
                Console.WriteLine("Received null data");
                return;
            }

            Type dataType = data.GetType();
            Console.WriteLine($"Processing {dataType.Name}: {data}");

            // Type-specific processing
            switch (data)
            {
                case string s:
                    Console.WriteLine($"  String length: {s.Length}");
                    break;
                case int i:
                    Console.WriteLine($"  Integer value: {i:N0}");
                    break;
                case Person p:
                    Console.WriteLine($"  Person details: {p.Name}, {p.Age} years old");
                    break;
                case Array arr:
                    Console.WriteLine($"  Array with {arr.Length} elements");
                    break;
                default:
                    Console.WriteLine($"  General object of type {dataType.Name}");
                    break;
            }
        }
    }
}
