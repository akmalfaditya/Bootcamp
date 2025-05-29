namespace TheObjectType
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== The Object Type: Your Universal Container ===\n");

            // Let's explore each concept step by step
            DemonstrateObjectAsUniversalBase();
            DemonstrateStackImplementation();
            DemonstrateTypeUnification();
            DemonstrateBoxingAndUnboxing();
            DemonstrateBoxingWithNumericTypes();
            DemonstrateTypeChecking();
            DemonstrateGetTypeAndTypeof();
            DemonstrateToStringOverride();
            DemonstrateObjectMembers();
            
            // Advanced examples and real-world scenarios
            DemonstrateAdvancedConcepts();

            Console.WriteLine("\n=== Object Type Demo Complete! ===");
            Console.ReadKey();
        }

        /// <summary>
        /// Shows how every type in C# inherits from object
        /// This is the foundation that makes polymorphism possible
        /// </summary>
        static void DemonstrateObjectAsUniversalBase()
        {
            Console.WriteLine("1. Object as Universal Base Class:");
            
            // Everything can be stored as object - the universal container!
            object intAsObject = 42;           // int -> object
            object stringAsObject = "Hello";   // string -> object
            object arrayAsObject = new int[5]; // array -> object
            object dateAsObject = DateTime.Now; // DateTime -> object
            
            Console.WriteLine("All these are now stored as 'object':");
            Console.WriteLine($"Integer: {intAsObject}");
            Console.WriteLine($"String: {stringAsObject}");
            Console.WriteLine($"Array: {arrayAsObject}");
            Console.WriteLine($"DateTime: {dateAsObject}");
            
            // The magic: we can treat them all the same way
            object[] mixedBag = { intAsObject, stringAsObject, arrayAsObject, dateAsObject };
            Console.WriteLine("\nIterating through mixed types as objects:");
            foreach (object item in mixedBag)
            {
                Console.WriteLine($"Type: {item.GetType().Name}, Value: {item}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates a practical use case: generic stack using object
        /// Before generics (List<T>), this was how we built flexible collections
        /// </summary>
        static void DemonstrateStackImplementation()
        {
            Console.WriteLine("2. Practical Example - Generic Stack with Object:");
            
            var stack = new SimpleStack();
            
            // Push different types - the stack doesn't care what goes in!
            stack.Push("First item");     // string
            stack.Push(100);              // int
            stack.Push(DateTime.Now);     // DateTime
            stack.Push(new Person("John", 25)); // custom object
            
            Console.WriteLine("Popping items from stack:");
            
            // Pop and cast back - LIFO (Last In, First Out)
            Person person = (Person)stack.Pop();           // Cast back to Person
            DateTime date = (DateTime)stack.Pop();         // Cast back to DateTime  
            int number = (int)stack.Pop();                 // Cast back to int
            string text = (string)stack.Pop();             // Cast back to string
            
            Console.WriteLine($"Person: {person}");
            Console.WriteLine($"Date: {date:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Number: {number}");
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows how value types and reference types are unified under object
        /// This is what makes C# a truly unified type system
        /// </summary>
        static void DemonstrateTypeUnification()
        {
            Console.WriteLine("3. Type Unification - Value and Reference Types:");
            
            var stack = new SimpleStack();
            
            // Value types (stored on stack normally)
            stack.Push(42);        // int
            stack.Push(3.14);      // double
            stack.Push('A');       // char
            stack.Push(true);      // bool
            
            // Reference types (stored on heap)
            stack.Push("Hello");   // string
            stack.Push(new int[3]); // array
            
            Console.WriteLine("Mixed value and reference types in same container:");
            
            // Pop them all back - the stack treats them equally
            int[] arr = (int[])stack.Pop();
            string str = (string)stack.Pop();
            bool boolean = (bool)stack.Pop();
            char character = (char)stack.Pop();
            double pi = (double)stack.Pop();
            int answer = (int)stack.Pop();
            
            Console.WriteLine($"Array: [{string.Join(", ", arr)}]");
            Console.WriteLine($"String: {str}");
            Console.WriteLine($"Boolean: {boolean}");
            Console.WriteLine($"Character: {character}");
            Console.WriteLine($"Double: {pi}");
            Console.WriteLine($"Integer: {answer}");
            Console.WriteLine();
        }

        /// <summary>
        /// Deep dive into boxing and unboxing - the secret sauce
        /// Boxing: value type -> heap allocation -> object reference
        /// Unboxing: object reference -> extract value -> stack value
        /// </summary>
        static void DemonstrateBoxingAndUnboxing()
        {
            Console.WriteLine("4. Boxing and Unboxing Deep Dive:");
            
            // Boxing: value type gets wrapped in an object
            int originalValue = 42;
            object boxedValue = originalValue;  // Boxing happens here!
              Console.WriteLine($"Original int: {originalValue}");
            Console.WriteLine($"Boxed object: {boxedValue}");
            Console.WriteLine($"Are they the same reference? No - boxing creates a new object on the heap!");
            
            // Unboxing: extract the value back
            int unboxedValue = (int)boxedValue;  // Unboxing with explicit cast
            
            Console.WriteLine($"Unboxed int: {unboxedValue}");
            Console.WriteLine($"Values equal? {originalValue == unboxedValue}");
            
            // Watch out! Wrong type unboxing throws exception
            try
            {
                long wrongType = (long)boxedValue;  // This will fail!
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
                Console.WriteLine("Remember: unboxing must be to the EXACT same type!");
            }
            
            // Performance note: boxing creates heap allocation
            Console.WriteLine("\nPerformance tip: Boxing creates heap objects!");
            Console.WriteLine("Use generics (List<T>) instead of object collections when possible.");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows conversion scenarios with boxing/unboxing
        /// Sometimes you need to convert types during the process
        /// </summary>
        static void DemonstrateBoxingWithNumericTypes()
        {
            Console.WriteLine("5. Boxing with Numeric Conversions:");
            
            // Box a double
            object boxedDouble = 3.7;
            Console.WriteLine($"Boxed double: {boxedDouble}");
            
            // To convert to int, we need two-step process
            int convertedInt = (int)(double)boxedDouble;  // Unbox to double, then convert to int
            Console.WriteLine($"Converted to int: {convertedInt}");
            
            // This would fail - can't directly cast boxed double to int
            try
            {
                int directCast = (int)boxedDouble;  // InvalidCastException!
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Direct cast failed - need to unbox to original type first!");
            }
            
            // Demonstrate with different numeric types
            object boxedFloat = 2.5f;
            object boxedDecimal = 100.25m;
            
            // Each needs to be unboxed to its exact type before conversion
            float originalFloat = (float)boxedFloat;
            decimal originalDecimal = (decimal)boxedDecimal;
            
            Console.WriteLine($"Float: {originalFloat}, Decimal: {originalDecimal}");
            Console.WriteLine();
        }

        /// <summary>
        /// Explores static vs runtime type checking
        /// Static: compiler catches errors before runtime
        /// Runtime: CLR catches errors during execution
        /// </summary>
        static void DemonstrateTypeChecking()
        {
            Console.WriteLine("6. Static vs Runtime Type Checking:");
            
            // Static type checking (compile-time)
            Console.WriteLine("Static type checking prevents this at compile time:");
            Console.WriteLine("// int x = \"hello\";  // Compiler error!");
            
            // Runtime type checking (during execution)
            Console.WriteLine("\nRuntime type checking catches errors during execution:");
            
            object mysteryObject = "I'm actually a string";
            
            // This will fail at runtime because mysteryObject is really a string
            try
            {
                int number = (int)mysteryObject;  // Runtime exception!
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Runtime error caught: {ex.Message}");
            }
            
            // Safe ways to check types at runtime
            if (mysteryObject is string text)
            {
                Console.WriteLine($"Pattern matching success: '{text}'");
            }
            
            // Using 'as' operator for safe casting
            int? maybeNumber = mysteryObject as int?;
            if (maybeNumber == null)
            {
                Console.WriteLine("'as' operator returned null - safe casting failed");
            }
            
            Console.WriteLine("Always check types when dealing with object references!");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows how to inspect types at runtime
        /// GetType() vs typeof() - both give you type information
        /// </summary>
        static void DemonstrateGetTypeAndTypeof()
        {
            Console.WriteLine("7. Type Inspection with GetType() and typeof:");
            
            var person = new Person("Alice", 30);
            var number = 42;
            var text = "Hello World";
            
            // GetType() - called on object instances at runtime
            Console.WriteLine("Using GetType() on instances:");
            Console.WriteLine($"person.GetType(): {person.GetType().Name}");
            Console.WriteLine($"number.GetType(): {number.GetType().Name}");
            Console.WriteLine($"text.GetType(): {text.GetType().Name}");
            
            // typeof() - evaluated at compile time
            Console.WriteLine("\nUsing typeof() with type names:");
            Console.WriteLine($"typeof(Person): {typeof(Person).Name}");
            Console.WriteLine($"typeof(int): {typeof(int).Name}");
            Console.WriteLine($"typeof(string): {typeof(string).Name}");
            
            // More detailed type information
            Type personType = person.GetType();
            Console.WriteLine($"\nDetailed info for {personType.Name}:");
            Console.WriteLine($"Full name: {personType.FullName}");
            Console.WriteLine($"Namespace: {personType.Namespace}");
            Console.WriteLine($"Assembly: {personType.Assembly.GetName().Name}");
            Console.WriteLine($"Base type: {personType.BaseType?.Name ?? "None"}");
            
            // Type comparison
            Console.WriteLine($"\nType comparisons:");
            Console.WriteLine($"person.GetType() == typeof(Person): {person.GetType() == typeof(Person)}");
            
            // Check inheritance
            Console.WriteLine($"Person inherits from object: {typeof(object).IsAssignableFrom(typeof(Person))}");
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates ToString() method overriding
        /// Every object has ToString() - you can customize it!
        /// </summary>
        static void DemonstrateToStringOverride()
        {
            Console.WriteLine("8. ToString() Method Override:");
            
            // Built-in types have meaningful ToString() implementations
            int number = 42;
            double pi = 3.14159;
            DateTime now = DateTime.Now;
            
            Console.WriteLine("Built-in types ToString():");
            Console.WriteLine($"int: {number.ToString()}");
            Console.WriteLine($"double: {pi.ToString()}");
            Console.WriteLine($"DateTime: {now.ToString()}");
            
            // Custom class with overridden ToString()
            var person = new Person("Bob", 35);
            var product = new Product("Laptop", 999.99m);
            
            Console.WriteLine("\nCustom classes with overridden ToString():");
            Console.WriteLine($"Person: {person.ToString()}");
            Console.WriteLine($"Product: {product.ToString()}");
            
            // ToString() is called automatically by Console.WriteLine
            Console.WriteLine("\nAutomatic ToString() calls:");
            Console.WriteLine(person);   // Implicitly calls ToString()
            Console.WriteLine(product);  // Implicitly calls ToString()
            
            // String interpolation also uses ToString()
            Console.WriteLine($"\nString interpolation: Person is {person}, Product is {product}");
            Console.WriteLine();
        }

        /// <summary>
        /// Explores the core members that every object inherits
        /// These are the building blocks of the .NET type system
        /// </summary>
        static void DemonstrateObjectMembers()
        {
            Console.WriteLine("9. Core Object Members:");
            
            var person1 = new Person("Charlie", 25);
            var person2 = new Person("Charlie", 25);
            var person3 = person1;  // Same reference
            
            // Equals() - value equality (overridden in Person)
            Console.WriteLine("Equals() method:");
            Console.WriteLine($"person1.Equals(person2): {person1.Equals(person2)}");  // True (same values)
            Console.WriteLine($"person1.Equals(person3): {person1.Equals(person3)}");  // True (same reference)
            
            // ReferenceEquals() - reference equality
            Console.WriteLine("\nReferenceEquals() method:");
            Console.WriteLine($"ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)}");  // False
            Console.WriteLine($"ReferenceEquals(person1, person3): {ReferenceEquals(person1, person3)}");  // True
            
            // GetHashCode() - for hash-based collections
            Console.WriteLine("\nGetHashCode() method:");
            Console.WriteLine($"person1.GetHashCode(): {person1.GetHashCode()}");
            Console.WriteLine($"person2.GetHashCode(): {person2.GetHashCode()}");
            Console.WriteLine("Objects with same values should have same hash code (if Equals is true)");
            
            // GetType() - runtime type information
            Console.WriteLine($"\nGetType(): {person1.GetType().Name}");
            
            // ToString() - string representation
            Console.WriteLine($"ToString(): {person1.ToString()}");
            
            // MemberwiseClone() - shallow copy (protected, shown conceptually)
            Console.WriteLine("\nMemberwiseClone() creates shallow copies (protected method)");
            Console.WriteLine("Used internally by classes that implement ICloneable");
              Console.WriteLine("\nThese methods form the contract that every .NET object follows!");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Demonstrates advanced object concepts and real-world scenarios
        /// Shows performance implications and best practices
        /// </summary>
        static void DemonstrateAdvancedConcepts()
        {
            Console.WriteLine("10. Advanced Object Concepts:");
            
            // Performance implications
            AdvancedObjectExamples.PerformanceExample();
            
            // Common mistakes
            AdvancedObjectExamples.CommonMistakes();
            
            // Object comparison nuances
            AdvancedObjectExamples.ObjectComparison();
            
            // Advanced type checking
            AdvancedObjectExamples.AdvancedTypeChecking();
            
            // Heterogeneous collections
            AdvancedObjectExamples.HeterogeneousCollections();
            
            Console.WriteLine("\nKey takeaways:");
            Console.WriteLine("✅ Use generics (List<T>) instead of object collections when possible");
            Console.WriteLine("✅ Override ToString(), Equals(), and GetHashCode() in your custom classes");
            Console.WriteLine("✅ Be careful with boxing/unboxing - it creates heap allocations");
            Console.WriteLine("✅ Use pattern matching and 'is'/'as' for safe type checking");
            Console.WriteLine("✅ Remember: everything inherits from object - that's the power of .NET!");
            Console.WriteLine();
        }
    }
}
