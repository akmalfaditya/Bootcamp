using System;

namespace Classes
{
    /// <summary>
    /// Welcome to C# Classes - the heart of object-oriented programming!
    /// Think of classes as blueprints for creating objects. Just like an architect's blueprint
    /// defines how to build a house, a class defines how to create and work with objects.
    /// 
    /// This demo covers everything from basic class concepts to advanced features.
    /// We'll build practical examples that you might actually use in real projects.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Classes: Building Blocks of Object-Oriented Programming ===\n");

            // Start with the fundamentals - what makes a class tick
            DemonstrateBasicClasses();
            
            // Fields - data containers
            DemonstrateFields();
            
            // Constants - immutable values
            DemonstrateConstants();
            
            // Methods - behavior and functionality
            DemonstrateMethods();
            
            // Constructors - how objects come to life
            DemonstrateConstructors();
            
            // Properties - the elegant way to control data access
            DemonstrateProperties();
            
            // Indexers - making your objects work like arrays
            DemonstrateIndexers();
            
            // Object initializers and deconstructors
            DemonstrateObjectFeatures();
            
            // Static classes and members
            DemonstrateStaticFeatures();
            
            // Primary constructors (C# 12 feature)
            DemonstratePrimaryConstructors();
            
            // Partial classes and methods
            DemonstratePartialClasses();
            
            // The 'this' keyword and references
            DemonstrateThisKeyword();

            Console.WriteLine("\n=== Classes Demo Complete! ===");
            Console.WriteLine("You've seen the core building blocks of C# object-oriented programming.");
            Console.ReadKey();
        }

        /// <summary>
        /// Basic class creation and instantiation
        /// Shows how classes work as blueprints for objects
        /// </summary>
        static void DemonstrateBasicClasses()
        {
            Console.WriteLine("1. Basic Classes - The Foundation:");
            
            // Create employee instances from our Employee class blueprint
            var employee = new Employee("Alice Johnson", 28);
            var manager = new Employee("Bob Smith", 35);
            
            Console.WriteLine($"  Employee: {employee.Name}, Age: {employee.Age}");
            Console.WriteLine($"  Manager: {manager.Name}, Age: {manager.Age}");
            
            // Show how each object has its own data
            employee.CelebrateBirthday();
            Console.WriteLine($"  After birthday: {employee.Name} is now {employee.Age}");
            Console.WriteLine($"  Manager's age unchanged: {manager.Age}");
            
            Console.WriteLine("✅ Each object has its own copy of instance data\n");
        }

        /// <summary>
        /// Field types and behaviors
        /// Covers instance fields, static fields, readonly, and initialization
        /// </summary>
        static void DemonstrateFields()
        {
            Console.WriteLine("2. Fields - Data Containers:");
            
            // Instance fields - each object gets its own copy
            var octopus1 = new Octopus("Oscar");
            var octopus2 = new Octopus("Ollie");
            
            Console.WriteLine($"  Octopus 1: {octopus1.Name}, Age: {octopus1.Age}");
            Console.WriteLine($"  Octopus 2: {octopus2.Name}, Age: {octopus2.Age}");
            
            // Static fields - shared across ALL instances
            Console.WriteLine($"  All octopuses have {Octopus.Legs} legs (static field)");
            Console.WriteLine($"  Total octopuses created: {Octopus.TotalCreated}");
            
            // Readonly fields
            Console.WriteLine($"  Octopus 1 ID (readonly): {octopus1.Id}");
            Console.WriteLine($"  Octopus 2 ID (readonly): {octopus2.Id}");
            
            Console.WriteLine("✅ Instance fields are per-object, static fields are per-type\n");
        }

        /// <summary>
        /// Constants vs static readonly fields
        /// When to use which and their compilation differences
        /// </summary>
        static void DemonstrateConstants()
        {
            Console.WriteLine("3. Constants vs Static Readonly:");
            
            // Constants - compile-time values, baked into consuming assemblies
            Console.WriteLine($"  PI (const): {MathConstants.PI}");
            Console.WriteLine($"  Speed of Light (const): {MathConstants.SPEED_OF_LIGHT:E} m/s");
            
            // Static readonly - runtime values, can be different each run
            Console.WriteLine($"  App Start Time (static readonly): {MathConstants.ApplicationStartTime}");
            Console.WriteLine($"  Random Seed (static readonly): {MathConstants.RandomSeed}");
            
            // Local constants
            const int LOCAL_MAX = 100;
            Console.WriteLine($"  Local constant: {LOCAL_MAX}");
            
            Console.WriteLine("✅ Use const for truly constant values, static readonly for runtime constants\n");
        }

        /// <summary>
        /// Methods in all their forms
        /// Regular methods, expression-bodied, local methods, overloading
        /// </summary>
        static void DemonstrateMethods()
        {
            Console.WriteLine("4. Methods - Object Behaviors:");
            
            var math = new MathOperations();
            
            // Regular methods
            Console.WriteLine($"  Addition: 15 + 25 = {math.Add(15, 25)}");
            Console.WriteLine($"  Subtraction: 50 - 12 = {math.Subtract(50, 12)}");
            
            // Expression-bodied methods
            Console.WriteLine($"  Multiplication: 8 × 7 = {math.Multiply(8, 7)}");
            Console.WriteLine($"  Division: 100 ÷ 4 = {math.Divide(100, 4)}");
            
            // Method with complex logic and local methods
            Console.WriteLine($"  Factorial of 5: {math.CalculateFactorial(5)}");
            
            // Method overloading
            Console.WriteLine($"  Power (int): 2^8 = {math.Power(2, 8)}");
            Console.WriteLine($"  Power (double): 2.5^3 = {math.Power(2.5, 3):F2}");
            
            Console.WriteLine("✅ Methods define what objects can do\n");
        }

        /// <summary>
        /// Constructor variations and overloading
        /// Shows different ways to initialize objects
        /// </summary>
        static void DemonstrateConstructors()
        {
            Console.WriteLine("5. Constructors - Object Birth:");
            
            // Different constructor overloads
            var car1 = new Car("Toyota");  // Only make
            var car2 = new Car("Honda", "Civic");  // Make and model
            var car3 = new Car("BMW", "X5", 2023);  // All parameters
            
            Console.WriteLine($"  Car 1: {car1.Make} {car1.Model} ({car1.Year})");
            Console.WriteLine($"  Car 2: {car2.Make} {car2.Model} ({car2.Year})");
            Console.WriteLine($"  Car 3: {car3.Make} {car3.Model} ({car3.Year})");
            
            // Show constructor chaining in action
            var luxuryCar = new Car("Mercedes", "S-Class", 2024);
            Console.WriteLine($"  Luxury car: {luxuryCar}");
            
            Console.WriteLine("✅ Constructors can be overloaded and chained for flexible initialization\n");
        }

        /// <summary>
        /// Properties in detail
        /// Manual properties, automatic properties, validation, expression-bodied
        /// </summary>
        static void DemonstrateProperties()
        {
            Console.WriteLine("6. Properties - Controlled Data Access:");
            
            var stock = new Stock();
            
            // Automatic properties
            stock.Symbol = "MSFT";
            stock.CompanyName = "Microsoft Corporation";
            Console.WriteLine($"  Stock: {stock.Symbol} - {stock.CompanyName}");
            
            // Manual property with validation
            stock.CurrentPrice = 350.75m;
            Console.WriteLine($"  Current price: ${stock.CurrentPrice:F2}");
            
            // Try invalid price (will be rejected)
            try
            {
                stock.CurrentPrice = -10;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ❌ {ex.Message}");
            }
            
            // Calculated property
            stock.SharesOwned = 100;
            Console.WriteLine($"  Shares owned: {stock.SharesOwned}");
            Console.WriteLine($"  Total value (calculated): ${stock.TotalValue:F2}");
            
            // Read-only property
            Console.WriteLine($"  Created on: {stock.CreatedDate:yyyy-MM-dd HH:mm:ss}");
            
            Console.WriteLine("✅ Properties provide controlled access with validation and calculation capabilities\n");
        }

        /// <summary>
        /// Indexers for array-like access
        /// Making objects behave like collections
        /// </summary>
        static void DemonstrateIndexers()
        {
            Console.WriteLine("7. Indexers - Array-like Access:");
            
            var sentence = new Sentence();
            
            // Access words by index
            Console.WriteLine($"  Original: \"{sentence}\"");
            Console.WriteLine($"  Word 0: '{sentence[0]}'");
            Console.WriteLine($"  Word 2: '{sentence[2]}'");
            
            // Modify words using indexer
            sentence[3] = "kangaroo";
            Console.WriteLine($"  After changing word 3: \"{sentence}\"");
            
            // Try invalid index
            try
            {
                string word = sentence[10];
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"  ❌ {ex.Message}");
            }
            
            Console.WriteLine("✅ Indexers make objects behave like arrays or dictionaries\n");
        }

        /// <summary>
        /// Object initializers and deconstructors
        /// Modern C# object creation and destruction patterns
        /// </summary>
        static void DemonstrateObjectFeatures()
        {
            Console.WriteLine("8. Object Initializers & Deconstructors:");
            
            // Object initializer syntax
            var bunny = new Bunny
            {
                Name = "Fluffy",
                LikesCarrots = true,
                LikesHumans = false
            };
            
            Console.WriteLine($"  Created bunny: {bunny.Name}");
            Console.WriteLine($"  Likes carrots: {bunny.LikesCarrots}");
            Console.WriteLine($"  Likes humans: {bunny.LikesHumans}");
            
            // Deconstructor usage
            var rectangle = new Rectangle(5.0f, 3.0f);
            (float width, float height) = rectangle;  // Deconstruction
            Console.WriteLine($"  Rectangle deconstructed: width={width}, height={height}");
            
            // Mixed deconstruction
            var (w, h) = new Rectangle(8.0f, 6.0f);
            Console.WriteLine($"  Another rectangle: width={w}, height={h}");
            
            Console.WriteLine("✅ Object initializers and deconstructors provide elegant object creation/extraction\n");
        }

        /// <summary>
        /// Static classes and members
        /// Utility classes and shared functionality
        /// </summary>
        static void DemonstrateStaticFeatures()
        {
            Console.WriteLine("9. Static Classes & Members:");
            
            // Static class usage - cannot be instantiated
            double result = MathUtilities.Add(15.7, 24.3);
            Console.WriteLine($"  Static method: 15.7 + 24.3 = {result}");
            
            double sqrt = MathUtilities.SquareRoot(144);
            Console.WriteLine($"  Square root of 144 = {sqrt}");
            
            bool isPrime = MathUtilities.IsPrime(17);
            Console.WriteLine($"  Is 17 prime? {isPrime}");
            
            // Static constructor in action (already ran when class was first accessed)
            Console.WriteLine($"  Math utilities initialized: {MathUtilities.IsInitialized}");
            
            // Mixed static and instance members
            var employee = new Employee("Charlie Brown", 25);
            Console.WriteLine($"  Employee count: {Employee.TotalEmployees}");
            employee.Work();
            Console.WriteLine($"  After work, total work hours: {Employee.TotalWorkHours}");
            
            Console.WriteLine("✅ Static members belong to the type, not instances\n");
        }

        /// <summary>
        /// Primary constructors (C# 12 feature)
        /// Simplified constructor syntax
        /// </summary>
        static void DemonstratePrimaryConstructors()
        {
            Console.WriteLine("10. Primary Constructors (C# 12):");
            
            // Primary constructor creates clean, concise syntax
            var person = new Person("Diana Prince", "Wonder Woman");
            person.PrintInfo();
            
            var point = new Point(10, 20);
            Console.WriteLine($"  Point coordinates: {point}");
            Console.WriteLine($"  Distance from origin: {point.DistanceFromOrigin:F2}");
            
            Console.WriteLine("✅ Primary constructors reduce boilerplate for simple initialization\n");
        }

        /// <summary>
        /// Partial classes and methods
        /// Code organization and extensibility
        /// </summary>
        static void DemonstratePartialClasses()
        {
            Console.WriteLine("11. Partial Classes & Methods:");
            
            // Partial class that's split across multiple files
            var paymentForm = new PaymentForm();
            
            // Call methods from different parts of the partial class
            paymentForm.ProcessPayment(250.00m);
            paymentForm.GenerateReceipt();
            
            // Partial methods in action
            paymentForm.CompleteTransaction(150.75m);
            
            Console.WriteLine("✅ Partial classes allow splitting implementation across files\n");
        }

        /// <summary>
        /// The 'this' keyword and object references
        /// Self-reference and disambiguation
        /// </summary>
        static void DemonstrateThisKeyword()
        {
            Console.WriteLine("12. The 'this' Keyword:");
            
            var panda1 = new Panda("Po");
            var panda2 = new Panda("Mei");
            
            Console.WriteLine($"  Created pandas: {panda1.Name} and {panda2.Name}");
            
            // Use 'this' to set up relationships
            panda1.SetMate(panda2);
            Console.WriteLine($"  {panda1.Name}'s mate: {panda1.Mate?.Name ?? "None"}");
            Console.WriteLine($"  {panda2.Name}'s mate: {panda2.Mate?.Name ?? "None"}");
            
            // Chain method calls using 'this'
            var person = new Person("John", "Doe")
                .SetAge(30)
                .SetEmail("john.doe@email.com");
            
            person.DisplayInfo();
            
            Console.WriteLine("✅ 'this' refers to the current instance and enables method chaining\n");
        }
    }
}
