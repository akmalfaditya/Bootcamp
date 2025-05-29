namespace Generics
{
    /// <summary>
    /// Think of generics as "type placeholders" - you write code once, and it works with any type
    /// It's like having a universal tool that adapts to whatever you're working with
    /// No more copy-pasting the same logic for different types!
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Generics: Write Once, Use Everywhere ===\n");

            // Let's explore generics step by step - from basic to advanced
            DemonstrateBasicGenericTypes();
            DemonstrateGenericMethods();
            DemonstrateGenericConstraints();
            DemonstrateVarianceInGenerics();
            DemonstrateRealWorldScenarios();

            Console.WriteLine("\n=== Generics Demo Complete! ===");
            Console.ReadKey();
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
        /// Generic constraints - adding rules to make generics more powerful
        /// Sometimes you need your generic type to have certain capabilities
        /// Constraints let you say "T must be comparable" or "T must have a parameterless constructor"
        /// </summary>
        static void DemonstrateGenericConstraints()
        {
            Console.WriteLine("3. Generic Constraints - Adding Rules to Generics:");
            
            // Comparable constraint - types that can be compared
            Console.WriteLine("⚖️ Comparable Constraint (where T : IComparable<T>):");
            
            int maxInt = ConstrainedGenerics.GetMaximum(15, 25);
            string maxString = ConstrainedGenerics.GetMaximum("apple", "zebra");
            DateTime maxDate = ConstrainedGenerics.GetMaximum(DateTime.Now, DateTime.Now.AddDays(1));
            
            Console.WriteLine($"Max of 15 and 25: {maxInt}");
            Console.WriteLine($"Max of 'apple' and 'zebra': {maxString}");
            Console.WriteLine($"Max date: {maxDate:yyyy-MM-dd}");
            
            // Class constraint - reference types only
            Console.WriteLine("\n🏗️ Class Constraint (where T : class):");
            var personRepository = new Repository<Person>();
            personRepository.Add(new Person("Charlie", 35));
            personRepository.Add(new Person("Diana", 28));
            personRepository.ShowAll();
            
            // New constraint - types with parameterless constructor
            Console.WriteLine("\n🆕 New Constraint (where T : new()):");
            var factory = new GenericFactory<Person>();
            Person newPerson = factory.CreateInstance();
            Console.WriteLine($"Created new person: {newPerson}");
            
            // Multiple constraints
            Console.WriteLine("\n🔗 Multiple Constraints:");
            var manager = new AdvancedManager<Employee>();
            manager.ProcessEmployee(new Employee("Eve", "Developer"));
            
            Console.WriteLine("✅ Constraints make generics more powerful by ensuring type capabilities!");
            Console.WriteLine();
        }

        /// <summary>
        /// Variance in generics - covariance and contravariance
        /// This is advanced stuff that makes generic interfaces more flexible
        /// Covariance (out): you can assign more specific to more general
        /// Contravariance (in): you can assign more general to more specific
        /// </summary>
        static void DemonstrateVarianceInGenerics()
        {
            Console.WriteLine("4. Variance in Generics - Flexible Type Relationships:");
            
            // Covariance demonstration
            Console.WriteLine("📤 Covariance (out keyword) - Output positions:");
            
            // Create specific collections
            var dogProducer = new AnimalProducer<Dog>();
            var catProducer = new AnimalProducer<Cat>();
            
            // Covariance allows assignment to more general type
            IAnimalProducer<Animal> animalProducer1 = dogProducer;  // Legal!
            IAnimalProducer<Animal> animalProducer2 = catProducer;  // Legal!
            
            Animal dog = animalProducer1.Produce();
            Animal cat = animalProducer2.Produce();
            
            Console.WriteLine($"Produced: {dog.Name} (Type: {dog.GetType().Name})");
            Console.WriteLine($"Produced: {cat.Name} (Type: {cat.GetType().Name})");
            
            // Contravariance demonstration
            Console.WriteLine("\n📥 Contravariance (in keyword) - Input positions:");
            
            var animalConsumer = new AnimalConsumer<Animal>();
            
            // Contravariance allows assignment to more specific type
            IAnimalConsumer<Dog> dogConsumer = animalConsumer;  // Legal!
            IAnimalConsumer<Cat> catConsumer = animalConsumer;  // Legal!
            
            dogConsumer.Consume(new Dog("Buddy"));
            catConsumer.Consume(new Cat("Whiskers"));
            
            // Built-in variance examples
            Console.WriteLine("\n🏗️ Built-in Variance Examples:");
            
            IEnumerable<string> strings = new List<string> { "hello", "world" };
            IEnumerable<object> objects = strings;  // Covariance in action!
            
            foreach (object obj in objects)
            {
                Console.WriteLine($"Object: {obj}");
            }
            
            Console.WriteLine("✅ Variance makes generic interfaces more flexible and powerful!");
            Console.WriteLine();
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
    }
}
