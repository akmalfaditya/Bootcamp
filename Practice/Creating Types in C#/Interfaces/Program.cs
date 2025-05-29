using System;

namespace Interfaces
{
    /// <summary>
    /// Think of interfaces as contracts - they tell you WHAT to do, not HOW to do it
    /// Like a job description that says "must be able to drive" but doesn't specify the car
    /// Master interfaces, and you'll write incredibly flexible, testable code!
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Interfaces: Contracts for Better Code ===\n");

            // Let's explore interfaces step by step with real examples
            DemonstrateBasicInterfaces();
            DemonstrateMultipleInterfaces();
            DemonstrateInterfaceInheritance();
            DemonstrateExplicitImplementation();
            DemonstrateReimplementation();
            DemonstrateDefaultInterfaceMembers();
            DemonstrateStaticInterfaceMembers();
            DemonstratePolymorphismWithInterfaces();
            DemonstrateRealWorldScenarios();
            DemonstrateBestPractices();

            Console.WriteLine("\n=== Interfaces Demo Complete! ===");
            Console.ReadKey();
        }

        /// <summary>
        /// Basic interfaces are like job contracts
        /// They say "if you want this job, you must be able to do these things"
        /// Perfect starting point to understand the concept
        /// </summary>
        static void DemonstrateBasicInterfaces()
        {
            Console.WriteLine("1. Basic Interfaces - The Foundation:");
            
            // Interface as contract - both shapes know what they need to do
            IShape circle = new Circle(5.0);
            IShape square = new Square(4.0);
            
            // Same interface, different implementations - that's the beauty!
            Console.WriteLine($"Circle - Area: {circle.Area():F2}, Perimeter: {circle.Perimeter():F2}");
            Console.WriteLine($"Square - Area: {square.Area():F2}, Perimeter: {square.Perimeter():F2}");
            
            // You can treat them the same way through the interface
            IShape[] shapes = { circle, square, new Triangle(3, 4, 5) };
            
            Console.WriteLine("\nCalculating all shapes through interface:");
            foreach (IShape shape in shapes)
            {
                Console.WriteLine($"Area: {shape.Area():F2}, Perimeter: {shape.Perimeter():F2}");
            }
            
            Console.WriteLine("\n✨ Key insight: Same interface, different behaviors!");
            Console.WriteLine("✅ This is polymorphism in action - one interface, many forms");
            Console.WriteLine();
        }

        /// <summary>
        /// Multiple interfaces are like having multiple skills
        /// A person can be both a driver AND a programmer AND a cook
        /// Classes can implement as many interfaces as they need
        /// </summary>
        static void DemonstrateMultipleInterfaces()
        {
            Console.WriteLine("2. Multiple Interfaces - Jack of All Trades:");
            
            // SmartDevice implements multiple interfaces - it's multi-talented!
            var smartphone = new SmartDevice("iPhone 15");
            
            // Can use it as a communication device
            ICommunicationDevice comm = smartphone;
            comm.SendMessage("Hello from interface!");
            comm.MakeCall("123-456-7890");
            
            // Can also use it as an entertainment device
            IEntertainmentDevice entertainment = smartphone;
            entertainment.PlayMusic("Bohemian Rhapsody");
            entertainment.PlayVideo("Coding Tutorial");
            
            // Or just use it directly with all capabilities
            smartphone.TakePicture();
            smartphone.BrowseInternet("github.com");
            
            Console.WriteLine("\n✨ One class, multiple contracts fulfilled!");
            Console.WriteLine("✅ This gives you incredible flexibility in design");
            Console.WriteLine("✅ Perfect for dependency injection and testing");
            Console.WriteLine();
        }

        /// <summary>
        /// Interface inheritance is like skill specialization
        /// Basic driving -> Advanced driving -> Racing
        /// Each level builds on the previous one
        /// </summary>
        static void DemonstrateInterfaceInheritance()
        {
            Console.WriteLine("3. Interface Inheritance - Building on Contracts:");
            
            // TextEditor must implement both IUndoable and IRedoable
            var editor = new TextEditor();
            
            // Can use as basic undoable
            IUndoable undoable = editor;
            undoable.Undo();
            
            // Can use as advanced redoable (which includes undo)
            IRedoable redoable = editor;
            redoable.Undo();  // From base interface
            redoable.Redo();  // From derived interface
            
            // Full functionality
            editor.Save();
            
            Console.WriteLine("\n✨ Interface inheritance creates specialized contracts!");
            Console.WriteLine("✅ Build complex behaviors from simple building blocks");
            Console.WriteLine();
        }

        /// <summary>
        /// Explicit implementation solves the "name collision" problem
        /// Like having two bosses who both want you to "report" to them
        /// You can satisfy both, just need to be explicit about which one
        /// </summary>
        static void DemonstrateExplicitImplementation()
        {
            Console.WriteLine("4. Explicit Interface Implementation - Avoiding Conflicts:");
            
            var robot = new MultiFunctionRobot();
            
            // Normal implementation (implicitly implements IWorker.Work)
            robot.Work();
            
            // Explicit implementation - need to cast to access
            ICleaningRobot cleaner = robot;
            cleaner.Work();  // Different implementation!
            
            ISecurityRobot security = robot;
            security.Work();  // Yet another implementation!
            
            // Robot can be many things depending on how you look at it
            Console.WriteLine("\nSame method name, different behaviors:");
            Console.WriteLine("- As worker: general work");
            Console.WriteLine("- As cleaner: cleaning work"); 
            Console.WriteLine("- As security: patrol work");
            
            Console.WriteLine("\n✨ Explicit implementation resolves naming conflicts!");
            Console.WriteLine("✅ One class can wear many hats effectively");
            Console.WriteLine();
        }

        /// <summary>
        /// Reimplementation is like upgrading your skills
        /// You learned to drive in a basic car, now you're driving a sports car
        /// Same license, but totally different experience
        /// </summary>
        static void DemonstrateReimplementation()
        {
            Console.WriteLine("5. Interface Reimplementation - Upgrading Behavior:");
            
            // Base implementation
            TextBox basicTextBox = new TextBox();
            basicTextBox.Undo();
            
            // Reimplemented version - same interface, enhanced behavior
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Undo();
            
            // Through interface - polymorphism still works
            IUndoable[] undoables = { basicTextBox, richTextBox };
            
            Console.WriteLine("\nThrough interface:");
            foreach (IUndoable item in undoables)
            {
                item.Undo();
            }
            
            Console.WriteLine("\n✨ Reimplementation allows behavior evolution!");
            Console.WriteLine("✅ Subclasses can provide specialized implementations");
            Console.WriteLine();
        }

        /// <summary>
        /// Default interface members (C# 8+) are like "basic training"
        /// The interface provides a default way to do something
        /// You can use it as-is or override it if you know better
        /// </summary>
        static void DemonstrateDefaultInterfaceMembers()
        {
            Console.WriteLine("6. Default Interface Members (C# 8+) - Built-in Behavior:");
            
            // FileLogger uses default implementation
            var fileLogger = new FileLogger();
            fileLogger.Log("This uses default implementation");
            
            // DatabaseLogger provides custom implementation
            var dbLogger = new DatabaseLogger();
            dbLogger.Log("This uses custom implementation");
            
            // Both work through interface
            ILogger[] loggers = { fileLogger, dbLogger };
            
            Console.WriteLine("\nBoth loggers through interface:");
            foreach (ILogger logger in loggers)
            {
                logger.Log($"Message from {logger.GetType().Name}");
            }
            
            Console.WriteLine("\n✨ Default implementations make interfaces more powerful!");
            Console.WriteLine("✅ Add new methods without breaking existing code");
            Console.WriteLine("✅ Provide sensible defaults while allowing customization");
            Console.WriteLine();
        }

        /// <summary>
        /// Static interface members (C# 11+) are like class-level contracts
        /// Instead of "instances must be able to...", it's "types must be able to..."
        /// Perfect for factory patterns and type-level operations
        /// </summary>
        static void DemonstrateStaticInterfaceMembers()
        {
            Console.WriteLine("7. Static Interface Members (C# 11+) - Type-Level Contracts:");
            
            // Static members belong to the type, not instance
            Console.WriteLine($"Product: {Product.Description}");
            Console.WriteLine($"Category: {Product.Category}");
            Console.WriteLine($"Default Priority: {Product.DefaultPriority}");
            
            Console.WriteLine($"\nService: {Service.Description}");
            Console.WriteLine($"Category: {Service.Category}");
            
            // Can use in generic constraints for type-level operations
            DisplayTypeInfo<Product>();
            DisplayTypeInfo<Service>();
            
            Console.WriteLine("\n✨ Static interface members enable type-level contracts!");
            Console.WriteLine("✅ Perfect for factory patterns and generic constraints");
            Console.WriteLine("✅ Compile-time polymorphism with types");
            Console.WriteLine();
        }

        // Generic method that requires static interface implementation
        static void DisplayTypeInfo<T>() where T : ITypeDescribable
        {
            Console.WriteLine($"Type {typeof(T).Name}: {T.Description} [{T.Category}]");
        }

        /// <summary>
        /// Polymorphism with interfaces is the real superpower
        /// Write code once, work with unlimited implementations
        /// Like a universal remote that works with any TV brand
        /// </summary>
        static void DemonstratePolymorphismWithInterfaces()
        {
            Console.WriteLine("8. Polymorphism Power - One Interface, Many Forms:");
            
            // Create different payment processors
            IPaymentProcessor[] processors = 
            {
                new CreditCardProcessor(),
                new PayPalProcessor(),
                new BankTransferProcessor(),
                new CryptocurrencyProcessor()
            };
            
            decimal amount = 100.50m;
            
            Console.WriteLine($"Processing ${amount} through different methods:");
            
            // Same code works with all processors - that's polymorphism!
            foreach (IPaymentProcessor processor in processors)
            {
                if (processor.ValidatePayment(amount))
                {
                    processor.ProcessPayment(amount);
                }
                else
                {
                    Console.WriteLine($"{processor.GetType().Name}: Payment validation failed");
                }
            }
            
            Console.WriteLine("\n✨ One loop, multiple payment methods!");
            Console.WriteLine("✅ Add new payment methods without changing existing code");
            Console.WriteLine("✅ This is the Open/Closed Principle in action");
            Console.WriteLine();
        }

        /// <summary>
        /// Real-world scenarios show why interfaces matter
        /// This is what you'll actually use in production code
        /// Repository pattern, dependency injection, testability - all rely on interfaces
        /// </summary>
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("9. Real-World Scenarios - Where Interfaces Shine:");
            
            // Repository pattern with interfaces
            Console.WriteLine("Repository Pattern:");
            IUserRepository repository = new DatabaseUserRepository();
            // Could easily switch to: new InMemoryUserRepository() or new FileUserRepository()
            
            repository.AddUser(new User { Id = 1, Name = "John Doe", Email = "john@example.com" });
            repository.AddUser(new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" });
            
            var user = repository.GetUser(1);
            Console.WriteLine($"Retrieved: {user?.Name} ({user?.Email})");
            
            var allUsers = repository.GetAllUsers();
            Console.WriteLine($"Total users: {allUsers.Count()}");
            
            // Notification system with multiple channels
            Console.WriteLine("\nNotification System:");
            INotificationService emailService = new EmailNotificationService();
            INotificationService smsService = new SmsNotificationService();
            INotificationService pushService = new PushNotificationService();
            
            var notificationManager = new NotificationManager();
            notificationManager.AddService(emailService);
            notificationManager.AddService(smsService);
            notificationManager.AddService(pushService);
            
            notificationManager.SendNotification("Welcome to our platform!", "user@example.com");
            
            Console.WriteLine("\n✨ Interfaces enable flexible, testable architectures!");
            Console.WriteLine("✅ Easy to mock for unit testing");
            Console.WriteLine("✅ Dependency injection becomes natural");
            Console.WriteLine("✅ Swap implementations without breaking code");
            Console.WriteLine();
        }

        /// <summary>
        /// Best practices learned from years of interface usage
        /// These guidelines will save you headaches and make your code maintainable
        /// </summary>
        static void DemonstrateBestPractices()
        {
            Console.WriteLine("10. Interface Best Practices - Hard-Won Wisdom:");
            
            Console.WriteLine("📋 Naming Conventions:");
            Console.WriteLine("✅ Start with 'I' (IDisposable, IComparable, IEnumerable)");
            Console.WriteLine("✅ Use adjectives ending in '-able' for capabilities");
            Console.WriteLine("✅ Use nouns for contracts (IRepository, IService)");
            
            Console.WriteLine("\n🎯 Design Guidelines:");
            Console.WriteLine("✅ Keep interfaces small and focused (Interface Segregation)");
            Console.WriteLine("✅ Define behavior, not data (no public fields)");
            Console.WriteLine("✅ Think about the contract from the client's perspective");
            Console.WriteLine("✅ Avoid marker interfaces (empty interfaces)");
            
            Console.WriteLine("\n⚡ Performance Tips:");
            Console.WriteLine("✅ Interface calls have tiny overhead vs direct calls");
            Console.WriteLine("✅ Use generic interfaces for better type safety");
            Console.WriteLine("✅ Consider async interfaces for I/O operations");
            
            Console.WriteLine("\n🧪 Testing Benefits:");
            Console.WriteLine("✅ Easy to create mock implementations");
            Console.WriteLine("✅ Test different scenarios with different implementations");
            Console.WriteLine("✅ Verify behavior contracts are maintained");
            
            Console.WriteLine("\n🔄 When to Use Interfaces vs Classes:");
            Console.WriteLine("✅ Interface: Multiple unrelated classes need same behavior");
            Console.WriteLine("✅ Interface: You want to support multiple inheritance");
            Console.WriteLine("✅ Interface: Designing for testability");
            Console.WriteLine("✅ Class: Sharing implementation code");
            Console.WriteLine("✅ Class: Representing a clear 'is-a' relationship");
            
            Console.WriteLine("\n🎪 Remember: Interfaces are about CONTRACTS, not CODE!");
            Console.WriteLine("They define WHAT must be done, not HOW it's done.");
            Console.WriteLine();
        }
    }
}
