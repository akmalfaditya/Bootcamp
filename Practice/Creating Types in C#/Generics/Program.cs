using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generics
{
    /// <summary>
    /// Comprehensive Generics demonstration project
    /// 
    /// Generics provide a mechanism for writing code that operates on data of different types,
    /// but in a type-safe manner. Unlike inheritance, which expresses reusability through a 
    /// common base type, generics use a "template" approach with "placeholder" types.
    /// 
    /// This project covers ALL aspects of generics from the course material:
    /// 1. Generic Types (Stack<T> example and why generics exist)
    /// 2. Generic Methods (Swap<T> and type inference)
    /// 3. Type Parameters and their declaration rules
    /// 4. typeof and Unbound Generic Types
    /// 5. default keyword with generics
    /// 6. Generic Constraints (where clauses)
    /// 7. Subclassing Generic Types
    /// 8. Self-Referencing Generic Declarations
    /// 9. Static Data in Generic Types
    /// 10. Type Parameters and Conversions
    /// 11. Covariance and Contravariance (Variance)
    /// 
    /// Key Benefits Demonstrated:
    /// - Increased Type Safety: Errors caught at compile time, not runtime
    /// - Reduced Casting: No need for explicit downcasting
    /// - Reduced Boxing/Unboxing: Performance benefits for value types
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Generics: Complete Learning Guide ===\n");
            Console.WriteLine("Generics provide type-safe, reusable code without performance penalties");
            Console.WriteLine("Key benefits: Type safety, reduced casting, no boxing/unboxing\n");

            // Progress through all generic concepts systematically
            DemonstrateWhyGenericsExist();
            DemonstrateBasicGenericTypes();
            DemonstrateGenericMethods();
            DemonstrateTypeParameters();
            DemonstrateDefaultKeyword();
            DemonstrateGenericConstraints();
            DemonstrateSubclassingGenericTypes();
            DemonstrateSelfReferencingGenerics();
            DemonstrateStaticDataInGenerics();
            DemonstrateTypeParameterConversions();
            DemonstrateVarianceInGenerics();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== Complete Generics Guide Finished! ===");
            Console.WriteLine("Key takeaways:");
            Console.WriteLine("✓ Generics provide type safety without performance costs");
            Console.WriteLine("✓ Write once, use with any type - ultimate code reusability");
            Console.WriteLine("✓ Constraints enable calling specific methods on generic types");
            Console.WriteLine("✓ Variance enables flexible type compatibility with interfaces");
            Console.ReadKey();
        }

        /// <summary>
        /// Why generics exist - solving the fundamental problems of code reuse
        /// Before generics, developers faced a tough choice:
        /// 1. Code duplication (separate classes for each type)
        /// 2. Type unsafe object-based solutions with performance issues
        /// Generics give us the best of both worlds!
        /// </summary>
        static void DemonstrateWhyGenericsExist()
        {
            Console.WriteLine("=== WHY GENERICS EXIST ===");
            Console.WriteLine("Solving the fundamental problems of reusable code\n");

            // Problem 1: Code Duplication (Hardcoded types)
            Console.WriteLine("PROBLEM 1: Code Duplication");
            Console.WriteLine("Before generics, you needed separate classes for each type:");
            Console.WriteLine("IntStack, StringStack, DateTimeStack, PersonStack...");
            Console.WriteLine("Massive code duplication for essentially the same logic!\n");

            // Problem 2: Object-based approach issues
            Console.WriteLine("PROBLEM 2: Object-based Approach Issues");
            
            var objectStack = new ObjectStack();
            objectStack.Push(42);        // Boxing occurs here - int becomes object
            objectStack.Push("Hello");   // String is reference type, no boxing
            objectStack.Push(DateTime.Now); // Boxing occurs - DateTime becomes object
            
            // Type safety issues - this compiles but causes runtime error!
            try
            {
                objectStack.Push("not a number");
                int number = (int)objectStack.Pop(); // Runtime exception!
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Runtime error: {ex.Message}");
            }
            
            // Boxing/Unboxing performance demonstration
            Console.WriteLine("\nPerformance issues with object approach:");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Object-based approach - boxing/unboxing
            for (int i = 0; i < 100000; i++)
            {
                objectStack.Push(i);     // Boxing
                int value = (int)objectStack.Pop(); // Unboxing
            }
            stopwatch.Stop();
            long objectTime = stopwatch.ElapsedMilliseconds;
            
            // Generic approach - no boxing/unboxing
            var genericStack = new Stack<int>();
            stopwatch.Restart();
            for (int i = 0; i < 100000; i++)
            {
                genericStack.Push(i);    // No boxing
                int value = genericStack.Pop(); // No unboxing
            }
            stopwatch.Stop();
            long genericTime = stopwatch.ElapsedMilliseconds;
            
            Console.WriteLine($"Object approach: {objectTime}ms (with boxing/unboxing)");
            Console.WriteLine($"Generic approach: {genericTime}ms (no boxing/unboxing)");
            
            // SOLUTION: Generics provide the best of both worlds
            Console.WriteLine("\nSOLUTION: Generics provide the best of both worlds:");
            Console.WriteLine("✓ Single implementation (Stack<T>) works for all types");
            Console.WriteLine("✓ Compile-time type safety - errors caught early");
            Console.WriteLine("✓ No boxing/unboxing - full performance for value types");
            Console.WriteLine("✓ Clean, readable code without casts");
            
            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Generic types - the foundation of reusable code
        /// Instead of writing separate Stack classes for int, string, etc.,
        /// we write ONE generic Stack that works with ANY type
        /// </summary>
        static void DemonstrateBasicGenericTypes()
        {
            Console.WriteLine("1. Basic Generic Types - One Code, Many Types:");
            
            // Create stacks for different types - same code, different data types
            var intStack = new CustomStack<int>();
            var stringStack = new CustomStack<string>();
            var personStack = new CustomStack<Person>();
            
            // Int stack operations
            Console.WriteLine("📦 Integer Stack:");
            intStack.Push(10);
            intStack.Push(20);
            intStack.Push(30);
            intStack.ShowStackInfo();
            Console.WriteLine($"Popped: {intStack.Pop()}");
            Console.WriteLine($"Current count: {intStack.Count}");
            
            // String stack operations
            Console.WriteLine("\n📦 String Stack:");
            stringStack.Push("First");
            stringStack.Push("Second");
            stringStack.Push("Third");
            stringStack.ShowStackInfo();
            Console.WriteLine($"Popped: {stringStack.Pop()}");
            
            // Custom object stack
            Console.WriteLine("\n📦 Person Stack:");
            personStack.Push(new Person("Alice", 25));
            personStack.Push(new Person("Bob", 30));
            personStack.ShowStackInfo();
            
            Console.WriteLine("✅ Same generic class works perfectly with int, string, and custom types!");
            Console.WriteLine();
        }

        /// <summary>
        /// Generic methods - algorithms that work with any type
        /// Perfect for utility functions that should work universally
        /// The compiler is smart enough to figure out the type automatically!
        /// </summary>
        static void DemonstrateGenericMethods()
        {
            Console.WriteLine("2. Generic Methods - Universal Algorithms:");
            
            // Swapping different types - same method, different data
            Console.WriteLine("🔄 Generic Swap Method:");
            
            int x = 5, y = 10;
            Console.WriteLine($"Before swap: x={x}, y={y}");
            GenericUtilities.Swap(ref x, ref y);  // Type automatically inferred
            Console.WriteLine($"After swap: x={x}, y={y}");
            
            string first = "Hello", second = "World";
            Console.WriteLine($"Before swap: first='{first}', second='{second}'");
            GenericUtilities.Swap(ref first, ref second);
            Console.WriteLine($"After swap: first='{first}', second='{second}'");
            
            // Array operations with generics
            Console.WriteLine("\n📋 Generic Array Operations:");
            int[] numbers = { 1, 2, 3, 4, 5 };
            string[] words = { "apple", "banana", "cherry" };
            
            GenericUtilities.PrintArray(numbers);
            GenericUtilities.PrintArray(words);
            
            // Search in arrays
            Console.WriteLine($"Index of 3 in numbers: {GenericUtilities.FindIndex(numbers, 3)}");
            Console.WriteLine($"Index of 'banana' in words: {GenericUtilities.FindIndex(words, "banana")}");
            
            Console.WriteLine("✅ Generic methods eliminate code duplication across different types!");
            Console.WriteLine();
        }

        /// <summary>
        /// Type Parameters and their declaration rules
        /// Understanding how to declare and use type parameters correctly
        /// Rules about where type parameters can be introduced
        /// </summary>
        static void DemonstrateTypeParameters()
        {
            Console.WriteLine("=== TYPE PARAMETERS ===");
            Console.WriteLine("Understanding type parameter declaration and usage\n");

            // Single type parameter
            Console.WriteLine("Single type parameter (T by convention):");
            var singleGeneric = new SingleTypeParameter<string>("Hello World");
            singleGeneric.ShowType();

            // Multiple type parameters
            Console.WriteLine("\nMultiple type parameters (descriptive names with T prefix):");
            var multiGeneric = new MultipleTypeParameters<string, int>();
            multiGeneric.SetKeyValue("age", 25);
            multiGeneric.ShowPair();

            // typeof with unbound generic types
            Console.WriteLine("\ntypeof with unbound generic types:");
            Type listType = typeof(List<>);     // Unbound generic type
            Type dictType = typeof(Dictionary<,>); // Use commas for multiple parameters
            
            Console.WriteLine($"Unbound List type: {listType}");
            Console.WriteLine($"Unbound Dictionary type: {dictType}");
            
            // typeof with closed types
            Type closedListType = typeof(List<int>);
            Type closedDictType = typeof(Dictionary<string, int>);
            
            Console.WriteLine($"Closed List<int> type: {closedListType}");
            Console.WriteLine($"Closed Dictionary<string, int> type: {closedDictType}");

            // Type parameter overloading by arity
            Console.WriteLine("\nType parameter overloading by arity:");
            Console.WriteLine("Can have MyClass, MyClass<T>, MyClass<T1, T2> - all different types");
            
            var overload0 = new ArityOverloadExample();
            var overload1 = new ArityOverloadExample<int>(42);
            var overload2 = new ArityOverloadExample<string, double>("test", 3.14);
            
            overload0.ShowInfo();
            overload1.ShowInfo();
            overload2.ShowInfo();

            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// The default keyword with generic types
        /// Understanding how default(T) works with different types
        /// Essential for initializing generic fields and variables
        /// </summary>
        static void DemonstrateDefaultKeyword()
        {
            Console.WriteLine("=== DEFAULT KEYWORD WITH GENERICS ===");
            Console.WriteLine("Getting default values for generic type parameters\n");

            // default(T) with different types
            Console.WriteLine("default(T) behavior:");
            DefaultValueDemo.ShowDefaultValue<int>();        // 0
            DefaultValueDemo.ShowDefaultValue<bool>();       // false
            DefaultValueDemo.ShowDefaultValue<string>();     // null
            DefaultValueDemo.ShowDefaultValue<DateTime>();   // 01/01/0001 00:00:00
            DefaultValueDemo.ShowDefaultValue<Person>();     // null

            // Practical usage - clearing arrays
            Console.WriteLine("\nPractical usage - clearing arrays:");
            int[] numbers = { 1, 2, 3, 4, 5 };
            string[] words = { "apple", "banana", "cherry" };
            
            Console.WriteLine("Before clearing:");
            Console.WriteLine($"Numbers: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Words: [{string.Join(", ", words.Where(w => w != null))}]");
            
            DefaultValueDemo.ClearArray(numbers);
            DefaultValueDemo.ClearArray(words);
            
            Console.WriteLine("\nAfter clearing:");
            Console.WriteLine($"Numbers: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Words: [{string.Join(", ", words.Where(w => w != null))}]");

            // C# 7.1+ simplified syntax
            Console.WriteLine("\nC# 7.1+ simplified syntax:");
            DefaultValueDemo.DemonstrateSimplifiedDefault();

            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Subclassing generic types - inheritance with generics
        /// Shows how generic classes can be inherited and specialized
        /// Demonstrates open vs closed inheritance patterns
        /// </summary>
        static void DemonstrateSubclassingGenericTypes()
        {
            Console.WriteLine("=== SUBCLASSING GENERIC TYPES ===");
            Console.WriteLine("Inheritance patterns with generic types\n");

            // Pattern 1: Leave base class type parameters open
            Console.WriteLine("Pattern 1: Keep type parameters open");
            var specialIntStack = new SpecialStack<int>();
            specialIntStack.Push(10);
            specialIntStack.Push(20);
            specialIntStack.PushWithLogging(30); // Special functionality
            
            Console.WriteLine($"Special stack count: {specialIntStack.Count}");
            Console.WriteLine($"Special stack operations: {specialIntStack.OperationCount}");

            // Pattern 2: Close generic type parameters with concrete types
            Console.WriteLine("\nPattern 2: Close type parameters with concrete types");
            var intStack = new IntStack();
            intStack.Push(100);
            intStack.Push(200);
            intStack.PushMultiple(300, 400, 500); // Specialized for int
            
            Console.WriteLine($"Int stack count: {intStack.Count}");
            Console.WriteLine($"Sum of all elements: {intStack.Sum()}");

            // Mixed inheritance - some open, some closed
            Console.WriteLine("\nMixed inheritance patterns:");
            var mixedExample = new MixedInheritanceExample<string>();
            mixedExample.SetData("Hello", 42);
            mixedExample.ShowData();

            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Self-referencing generic declarations
        /// The curiously recurring template pattern (CRTP) in C#
        /// Commonly used with interfaces like IEquatable<T> and IComparable<T>
        /// </summary>
        static void DemonstrateSelfReferencingGenerics()
        {
            Console.WriteLine("=== SELF-REFERENCING GENERIC DECLARATIONS ===");
            Console.WriteLine("The curiously recurring template pattern in C#\n");

            // IEquatable<T> example - self-referencing pattern
            Console.WriteLine("IEquatable<T> - self-referencing pattern:");
            var balloon1 = new Balloon { Color = "Red", CC = 100 };
            var balloon2 = new Balloon { Color = "Red", CC = 100 };
            var balloon3 = new Balloon { Color = "Blue", CC = 150 };

            Console.WriteLine($"Balloon1 == Balloon2: {balloon1.Equals(balloon2)}"); // true
            Console.WriteLine($"Balloon1 == Balloon3: {balloon1.Equals(balloon3)}"); // false

            // IComparable<T> example
            Console.WriteLine("\nIComparable<T> - another self-referencing pattern:");
            var person1 = new ComparablePerson("Alice", 25);
            var person2 = new ComparablePerson("Bob", 30);
            var person3 = new ComparablePerson("Charlie", 20);

            var people = new List<ComparablePerson> { person2, person3, person1 };
            Console.WriteLine("Before sorting:");
            people.ForEach(p => Console.WriteLine($"  {p}"));

            people.Sort(); // Uses IComparable<T>
            Console.WriteLine("\nAfter sorting by age:");
            people.ForEach(p => Console.WriteLine($"  {p}"));

            // Custom self-referencing interface
            Console.WriteLine("\nCustom self-referencing interface:");
            var node1 = new TreeNode<string>("Root");
            var node2 = new TreeNode<string>("Child1");
            var node3 = new TreeNode<string>("Child2");

            node1.AddChild(node2);
            node1.AddChild(node3);
            
            Console.WriteLine($"Node1 can accept: {node1.CanAccept(node2)}");
            Console.WriteLine($"Tree structure: {node1.GetTreeStructure()}");

            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Static data in generic types
        /// Each closed generic type has its own static data
        /// Demonstrates how static members are per-closed-type, not per-open-type
        /// </summary>
        static void DemonstrateStaticDataInGenerics()
        {
            Console.WriteLine("=== STATIC DATA IN GENERIC TYPES ===");
            Console.WriteLine("Each closed generic type has separate static data\n");

            // Each closed type has its own static data
            Console.WriteLine("Static counters for different closed types:");
            
            Console.WriteLine($"Bob<int>.Count before: {Bob<int>.Count}");
            Console.WriteLine($"Bob<string>.Count before: {Bob<string>.Count}");
            Console.WriteLine($"Bob<double>.Count before: {Bob<double>.Count}");

            // Increment counters for different types
            Console.WriteLine($"Incrementing Bob<int>.Count: {++Bob<int>.Count}");    // 1
            Console.WriteLine($"Incrementing Bob<int>.Count: {++Bob<int>.Count}");    // 2
            Console.WriteLine($"Incrementing Bob<string>.Count: {++Bob<string>.Count}"); // 1
            Console.WriteLine($"Incrementing Bob<double>.Count: {++Bob<double>.Count}"); // 1

            // Show they are independent
            Console.WriteLine("\nFinal counts - each type maintains separate state:");
            Console.WriteLine($"Bob<int>.Count: {Bob<int>.Count}");       // 2
            Console.WriteLine($"Bob<string>.Count: {Bob<string>.Count}"); // 1
            Console.WriteLine($"Bob<double>.Count: {Bob<double>.Count}"); // 1

            // Practical example - type-specific caching
            Console.WriteLine("\nPractical example - type-specific caching:");
            
            // Each type gets its own cache
            TypeSpecificCache<User>.Add(new User(1, "john", "john@test.com"));
            TypeSpecificCache<User>.Add(new User(2, "jane", "jane@test.com"));
            TypeSpecificCache<Product>.Add(new Product(101, "Laptop", 999.99m));
            
            Console.WriteLine($"Cached users: {TypeSpecificCache<User>.Count}");
            Console.WriteLine($"Cached products: {TypeSpecificCache<Product>.Count}");
            
            // Show the cached items
            Console.WriteLine("User cache contents:");
            foreach (var user in TypeSpecificCache<User>.GetAll())
            {
                Console.WriteLine($"  {user}");
            }

            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Type parameter conversions and ambiguity resolution
        /// Demonstrates the challenges with casting generic type parameters
        /// Shows solutions using 'as' operator and object casting
        /// </summary>
        static void DemonstrateTypeParameterConversions()
        {
            Console.WriteLine("=== TYPE PARAMETER CONVERSIONS ===");
            Console.WriteLine("Handling conversion ambiguities with generic type parameters\n");

            // Problem: Direct casting with generic types
            Console.WriteLine("Problem: Direct casting ambiguity");
            Console.WriteLine("// return (StringBuilder)arg; // Compile-time error: Ambiguous conversion");
            Console.WriteLine("Compiler doesn't know if T is reference type or value type with custom conversion\n");

            // Solution 1: Using the 'as' operator
            Console.WriteLine("Solution 1: Using the 'as' operator");
            Console.WriteLine("Safe for reference and nullable conversions only");
            
            object stringBuilderObj = new StringBuilder("Hello World");
            object stringObj = "Just a string";
            object intObj = 42;

            var result1 = ConversionExamples.SafeConvertToStringBuilder(stringBuilderObj);
            var result2 = ConversionExamples.SafeConvertToStringBuilder(stringObj);
            var result3 = ConversionExamples.SafeConvertToStringBuilder(intObj);

            Console.WriteLine($"StringBuilder object: {result1?.ToString() ?? "null"}");
            Console.WriteLine($"String object: {result2?.ToString() ?? "null"}");
            Console.WriteLine($"Int object: {result3?.ToString() ?? "null"}");

            // Solution 2: Cast to object first
            Console.WriteLine("\nSolution 2: Cast to object first");
            Console.WriteLine("Resolves ambiguity by using object as intermediate type");
            
            try
            {
                var result4 = ConversionExamples.ConvertViaObject<StringBuilder>(stringBuilderObj);
                Console.WriteLine($"StringBuilder via object: {result4}");
                
                // This will throw InvalidCastException
                var result5 = ConversionExamples.ConvertViaObject<StringBuilder>(stringObj);
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Expected exception: {ex.Message}");
            }

            // Unboxing example
            Console.WriteLine("\nUnboxing with generics:");
            object boxedInt = 42;
            object boxedDouble = 3.14;
            
            int unboxedInt = ConversionExamples.UnboxValue<int>(boxedInt);
            double unboxedDouble = ConversionExamples.UnboxValue<double>(boxedDouble);
            
            Console.WriteLine($"Unboxed int: {unboxedInt}");
            Console.WriteLine($"Unboxed double: {unboxedDouble}");

            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Covariance and contravariance - advanced variance concepts
        /// How to use covariance (out) and contravariance (in) with generics
        /// Enabling flexible and powerful API designs
        /// </summary>
        static void DemonstrateVarianceInGenerics()
        {
            Console.WriteLine("=== COVARIANCE AND CONTRAVARIANCE ===");
            Console.WriteLine("Advanced variance concepts with generics\n");

            // Covariance (out) - producing values
            Console.WriteLine("Covariance (out) - producing values:");
            IProducer<Animal> animalProducer = new Producer<Dog>();
            Animal animal = animalProducer.Produce();
            Console.WriteLine($"Produced animal: {animal.Name} (Type: {animal.GetType().Name})");

            // Contravariance (in) - consuming values
            Console.WriteLine("\nContravariance (in) - consuming values:");
            IConsumer<Dog> dogConsumer = new Consumer<Animal>();
            dogConsumer.Consume(new Dog("Buddy"));

            // Built-in covariance example: IEnumerable<T>
            Console.WriteLine("\nBuilt-in covariance example: IEnumerable<T>");
            IEnumerable<Dog> dogList = new List<Dog> { new Dog("Rex"), new Dog("Fido") };
            IEnumerable<Animal> animalList = dogList;  // Covariance in action!
            
            foreach (Animal a in animalList)
            {
                Console.WriteLine($"Animal name: {a.Name} (Type: {a.GetType().Name})");
            }

            // Built-in contravariance example: IComparer<T>
            Console.WriteLine("\nBuilt-in contravariance example: IComparer<T>");
            IComparer<Animal> animalComparer = new AnimalAgeComparer();
            IComparer<Dog> dogComparer = animalComparer;  // Legal due to contravariance
            
            var dogs = new List<Dog> { new Dog("Rex", 5), new Dog("Fido", 3) };
            dogs.Sort(dogComparer); // Sorts by age using AnimalAgeComparer
            
            Console.WriteLine("Dogs sorted by age:");
            foreach (var dog in dogs)
            {
                Console.WriteLine($"  {dog.Name} (Age: {dog.Age})");
            }

            Console.WriteLine(new string('=', 70) + "\n");
        }

        /// <summary>
        /// Real-world scenarios where generics shine
        /// These are patterns you'll use in actual production code
        /// From data access to caching to event handling - generics are everywhere!
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("5. Real-World Scenarios - Generics in Action:");
            
            // Generic repository pattern
            Console.WriteLine("🗃️ Generic Repository Pattern:");
            var userRepo = new GenericRepository<User>();
            var productRepo = new GenericRepository<Product>();
            
            userRepo.Add(new User(1, "john.doe", "john@example.com"));
            userRepo.Add(new User(2, "jane.smith", "jane@example.com"));
            
            productRepo.Add(new Product(101, "Laptop", 999.99m));
            productRepo.Add(new Product(102, "Mouse", 29.99m));
            
            Console.WriteLine("Users in repository:");
            foreach (var user in userRepo.GetAll())
            {
                Console.WriteLine($"  {user}");
            }
            
            Console.WriteLine("Products in repository:");
            foreach (var product in productRepo.GetAll())
            {
                Console.WriteLine($"  {product}");
            }
            
            // Generic caching system
            Console.WriteLine("\n💾 Generic Caching System:");
            var cache = new GenericCache<string, User>();
            
            cache.Set("user:1", new User(1, "cached.user", "cached@example.com"));
            cache.Set("user:2", new User(2, "another.user", "another@example.com"));
            
            User? cachedUser = cache.Get("user:1");
            if (cachedUser != null)
            {
                Console.WriteLine($"Retrieved from cache: {cachedUser}");
            }
            
            // Generic event system
            Console.WriteLine("\n📡 Generic Event System:");
            var eventBus = new GenericEventBus();
            
            // Subscribe to different event types
            eventBus.Subscribe<UserLoggedIn>(evt => 
                Console.WriteLine($"  🔐 User logged in: {evt.Username} at {evt.Timestamp}"));
            
            eventBus.Subscribe<OrderPlaced>(evt => 
                Console.WriteLine($"  🛒 Order placed: #{evt.OrderId} for ${evt.Amount:F2}"));
            
            // Publish events
            eventBus.Publish(new UserLoggedIn("john.doe", DateTime.Now));
            eventBus.Publish(new OrderPlaced(12345, 199.99m, DateTime.Now));
            
            Console.WriteLine("✅ Generics enable powerful, reusable patterns in real applications!");
            Console.WriteLine();
        }

        /// <summary>
        /// Generic constraints - adding requirements to type parameters
        /// Constraints enable calling specific methods and operations on generic types
        /// They're like "rules" that T must follow to be used with your generic class/method
        /// </summary>
        static void DemonstrateGenericConstraints()
        {
            Console.WriteLine("=== GENERIC CONSTRAINTS ===");
            Console.WriteLine("Adding requirements to type parameters for enhanced capabilities\n");

            // IComparable<T> constraint - enables comparison operations
            Console.WriteLine("IComparable<T> constraint - enables comparison operations:");
            int maxInt = ConstrainedGenerics.GetMaximum(10, 20);
            string maxString = ConstrainedGenerics.GetMaximum("apple", "banana");
            DateTime maxDate = ConstrainedGenerics.GetMaximum(DateTime.Now, DateTime.Now.AddDays(1));
            
            Console.WriteLine($"Maximum of 10 and 20: {maxInt}");
            Console.WriteLine($"Maximum of 'apple' and 'banana': {maxString}");
            Console.WriteLine($"Maximum date: {maxDate:yyyy-MM-dd}");

            // Array operations with constraints
            Console.WriteLine("\nArray operations with IComparable constraint:");
            int[] numbers = { 5, 2, 8, 1, 9, 3 };
            Console.WriteLine($"Original array: [{string.Join(", ", numbers)}]");
            
            int min = ConstrainedGenerics.FindMinimum(numbers);
            Console.WriteLine($"Minimum value: {min}");
            
            ConstrainedGenerics.QuickSort(numbers);
            Console.WriteLine($"Sorted array: [{string.Join(", ", numbers)}]");

            // Class constraint - reference types only
            Console.WriteLine("\nClass constraint (where T : class):");
            var personRepo = new Repository<Person>();
            personRepo.Add(new Person("Alice", 25));
            personRepo.Add(new Person("Bob", 30));
            personRepo.Add(null); // This works because of class constraint
            personRepo.ShowAll();

            // New() constraint - types with parameterless constructor
            Console.WriteLine("\nNew() constraint (where T : new()):");
            var employeeFactory = new GenericFactory<Employee>();
            var newEmployee = employeeFactory.CreateInstance();
            var employees = employeeFactory.CreateInstances(3);
            Console.WriteLine($"Created {employees.Length} employees using factory");

            // Multiple constraints combined
            Console.WriteLine("\nMultiple constraints (where T : class, IComparable<T>, new()):");
            var manager = new AdvancedManager<Employee>();
            manager.ProcessEmployee(new Employee("John", "IT"));
            manager.ProcessEmployee(null!); // Will create new instance due to null check
            var maxEmployee = manager.GetMaximum();
            Console.WriteLine($"Maximum employee: {maxEmployee}");

            // Struct constraint - value types only
            Console.WriteLine("\nStruct constraint (where T : struct):");
            var intProcessor = new ValueTypeProcessor<int>();
            int? nullableInt = null;
            int processedInt = intProcessor.ProcessNullable(nullableInt);
            bool areEqual = intProcessor.AreEqual(5, 5);
            Console.WriteLine($"Are 5 and 5 equal? {areEqual}");

            // Enum constraint (C# 7.3+)
            Console.WriteLine("\nEnum constraint (where T : struct, Enum):");
            var dayValues = EnumUtilities<DayOfWeek>.GetAllValues();
            Console.WriteLine($"All DayOfWeek values: {string.Join(", ", dayValues)}");
            
            if (EnumUtilities<DayOfWeek>.TryParse("Monday", out DayOfWeek day))
            {
                Console.WriteLine($"Parsed day: {day}");
            }

            Console.WriteLine(new string('=', 70) + "\n");
        }
    }
}
