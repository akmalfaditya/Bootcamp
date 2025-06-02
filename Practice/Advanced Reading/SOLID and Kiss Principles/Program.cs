using SOLID_and_Kiss_Principles.BadExamples;
using SOLID_and_Kiss_Principles.GoodExamples;
using SOLID_and_Kiss_Principles.Interfaces;
using SOLID_and_Kiss_Principles.Models;
using SOLID_and_Kiss_Principles.Tests;

namespace SOLID_and_Kiss_Principles;

/// <summary>
/// Welcome to the SOLID & KISS Principles Demo!
/// This program shows you the difference between good and bad code design
/// 
/// We'll demonstrate each principle with real examples that you can run and see
/// Pay attention to the comments - they explain what's happening and why
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🚀 SOLID & KISS Principles Demo");
        Console.WriteLine("=====================================");
        Console.WriteLine();
        
        // Let's see each principle in action
        DemonstrateSOLIDPrinciples();
        
        Console.WriteLine();
        Console.WriteLine("Press any key to continue to KISS principle demo...");
        Console.ReadKey();
        Console.Clear();
          DemonstrateKISSPrinciple();
        
        Console.WriteLine();
        Console.WriteLine("Press any key to see example tests...");
        Console.ReadKey();
        Console.Clear();
        
        // Show how good design makes testing easier
        ExampleTests.RunExampleTests();
        
        Console.WriteLine();
        Console.WriteLine("🎉 Demo completed! You've seen SOLID and KISS principles in action.");
        Console.WriteLine("Remember: These principles make your code easier to maintain, test, and extend.");
        Console.WriteLine();
        Console.WriteLine("📚 Check out QUICK_REFERENCE.md for a cheat sheet");
        Console.WriteLine("🏋️ Try the exercises in EXERCISES.md to practice");
    }
    
    static void DemonstrateSOLIDPrinciples()
    {
        Console.WriteLine("📚 SOLID PRINCIPLES DEMONSTRATION");
        Console.WriteLine("==================================");
        
        // 1. Single Responsibility Principle
        DemonstrateSRP();
        
        // 2. Open/Closed Principle  
        DemonstrateOCP();
        
        // 3. Liskov Substitution Principle
        DemonstrateLSP();
        
        // 4. Interface Segregation Principle
        DemonstrateISP();
        
        // 5. Dependency Inversion Principle
        DemonstrateDIP();
    }
    
    static void DemonstrateSRP()
    {
        Console.WriteLine("\n🎯 1. SINGLE RESPONSIBILITY PRINCIPLE (SRP)");
        Console.WriteLine("Each class should have only ONE reason to change");
        Console.WriteLine("------------------------------------------------");
        
        Console.WriteLine("\n❌ BAD: One class doing everything");
        try
        {
            var badService = new BadUserService();
            badService.RegisterUser("john@example.com", "password123");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        Console.WriteLine("\n✅ GOOD: Separated responsibilities");
        var logger = new ConsoleLogger();
        var emailService = new EmailService(logger);
        var userRepository = new UserRepository(logger);
        var goodService = new UserService(emailService, userRepository, logger);
        
        goodService.RegisterUser("jane@example.com", "securepass456");
        
        Console.WriteLine("\n💡 Notice how the good example separates email, logging, and database concerns!");
    }
    
    static void DemonstrateOCP()
    {
        Console.WriteLine("\n🔓 2. OPEN/CLOSED PRINCIPLE (OCP)");
        Console.WriteLine("Open for extension, closed for modification");
        Console.WriteLine("------------------------------------------");
        
        Console.WriteLine("\n❌ BAD: Adding new shapes requires modifying AreaCalculator");
        var badCalculator = new BadAreaCalculator();
        var badShapes = new object[] 
        { 
            new BadRectangle { Width = 5, Height = 3 },
            new BadCircle { Radius = 2 }
        };
        
        var badTotal = badCalculator.CalculateTotalArea(badShapes);
        Console.WriteLine($"Bad calculator total: {badTotal}");
        
        Console.WriteLine("\n✅ GOOD: Can add new shapes without changing AreaCalculator");
        var logger = new ConsoleLogger();
        var goodCalculator = new AreaCalculator(logger);
        var goodShapes = new Shape[]
        {
            new Rectangle(5, 3),
            new Circle(2),
            new Triangle(4, 6) // New shape added without changing AreaCalculator!
        };
        
        var goodTotal = goodCalculator.CalculateTotalArea(goodShapes);
        
        Console.WriteLine("\n💡 Adding Triangle didn't require changing the AreaCalculator class!");
    }
    
    static void DemonstrateLSP()
    {
        Console.WriteLine("\n🔄 3. LISKOV SUBSTITUTION PRINCIPLE (LSP)");
        Console.WriteLine("Subclasses should be substitutable for their base classes");
        Console.WriteLine("--------------------------------------------------------");
        
        Console.WriteLine("\n❌ BAD: Penguin breaks the contract of Bird");
        try
        {
            BadBird badBird1 = new BadSparrow();
            badBird1.Fly(); // Works fine
            
            BadBird badBird2 = new BadPenguin();
            badBird2.Fly(); // Throws exception - breaks LSP!
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with bad design: {ex.Message}");
        }
        
        Console.WriteLine("\n✅ GOOD: Proper abstraction with interfaces");
        // All birds can eat and make sounds
        Bird bird1 = new Sparrow();
        Bird bird2 = new Penguin();
        Bird bird3 = new Duck();
        
        bird1.Eat();
        bird1.MakeSound();
        bird2.Eat();
        bird2.MakeSound();
        bird3.Eat();
        bird3.MakeSound();
        
        // Only birds that can fly implement IFlyable
        if (bird1 is IFlyable flyingBird1) flyingBird1.Fly();
        if (bird2 is IFlyable flyingBird2) flyingBird2.Fly(); // Won't execute - penguin can't fly
        if (bird3 is IFlyable flyingBird3) flyingBird3.Fly();
        
        // Only birds that can swim implement ISwimmable
        if (bird2 is ISwimmable swimmingBird) swimmingBird.Swim();
        if (bird3 is ISwimmable swimmingBird2) swimmingBird2.Swim();
        
        Console.WriteLine("\n💡 No exceptions thrown! Each bird behaves correctly for its type.");
    }
    
    static void DemonstrateISP()
    {
        Console.WriteLine("\n🧩 4. INTERFACE SEGREGATION PRINCIPLE (ISP)");
        Console.WriteLine("Don't force classes to implement interfaces they don't use");
        Console.WriteLine("----------------------------------------------------------");
        
        Console.WriteLine("\n❌ BAD: Manager forced to implement worker methods");
        try
        {
            var badManager = new BadManager();
            badManager.AssignTask(); // Works
            badManager.WorkOnTask(); // Throws exception - managers don't work on tasks!
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with bad design: {ex.Message}");
        }
        
        Console.WriteLine("\n✅ GOOD: Focused interfaces");
        var logger = new ConsoleLogger();
        var notificationService = new NotificationService(logger);
        
        // Manager only implements ITaskManager
        var manager = new Manager(notificationService, logger);
        manager.AssignTask("Build new feature", "developer@company.com");
        manager.CreateSubTask("Build new feature", "Write unit tests");
        manager.ReviewTask("Build new feature");
        
        // TeamLead implements both ITaskManager and IWorker
        var teamLead = new TeamLead(notificationService, logger);
        teamLead.AssignTask("Fix bug", "junior@company.com");
        teamLead.WorkOnTask("Code review task"); // TeamLead can also work
        teamLead.ReportProgress("Code review task", 75);
        teamLead.CompleteTask("Code review task");
        
        Console.WriteLine("\n💡 Each role implements only the interfaces it needs!");
    }
    
    static void DemonstrateDIP()
    {
        Console.WriteLine("\n🔗 5. DEPENDENCY INVERSION PRINCIPLE (DIP)");
        Console.WriteLine("Depend on abstractions, not concrete implementations");
        Console.WriteLine("----------------------------------------------------");
        
        Console.WriteLine("\n❌ BAD: Direct dependencies on concrete classes");
        var badOrderService = new BadOrderService();
        var testOrder = new Order("customer@example.com");
        testOrder.Items.Add(new OrderItem("Laptop", 1, 999.99m));
        badOrderService.ProcessOrder(testOrder);
        
        Console.WriteLine("\n✅ GOOD: Dependencies injected through interfaces");
        var logger = new ConsoleLogger();
        var emailService = new EmailService(logger);
        var userRepository = new UserRepository(logger);
        var priceCalculator = new PriceCalculator(logger);
        
        // All dependencies are injected - easy to test and change
        var goodOrder = new Order("customer@example.com");
        goodOrder.Items.Add(new OrderItem("Laptop", 1, 999.99m));
        goodOrder.Items.Add(new OrderItem("Mouse", 2, 29.99m));
        
        var total = priceCalculator.CalculateTotal(goodOrder.Items);
        var discountedTotal = priceCalculator.ApplyDiscount(total, 10); // 10% discount
        var finalTotal = priceCalculator.ApplyTax(discountedTotal, 8.5m); // 8.5% tax
        
        goodOrder.TotalAmount = finalTotal;
        emailService.SendOrderConfirmation(goodOrder.CustomerEmail, goodOrder.Id.ToString());
        
        Console.WriteLine($"Final order total: ${finalTotal:F2}");
        Console.WriteLine("\n💡 Easy to swap implementations for testing or different environments!");
    }
    
    static void DemonstrateKISSPrinciple()
    {
        Console.WriteLine("🎯 KISS PRINCIPLE DEMONSTRATION");
        Console.WriteLine("===============================");
        Console.WriteLine("Keep It Simple, Stupid - Simplicity is the ultimate sophistication");
        
        Console.WriteLine("\n❌ BAD: Over-engineered price calculation");
        var badCalculator = new BadPriceCalculator();
        
        // Look at all these parameters - way too complex!
        var badPrice = badCalculator.CalculatePrice(
            quantity: 2, 
            pricePerItem: 100m, 
            applyDiscount: true, 
            isTaxable: true, 
            isVip: true, 
            isWeekend: false, 
            isHoliday: false, 
            customerType: "premium"
        );
        Console.WriteLine($"Bad calculator result: ${badPrice:F2}");
        Console.WriteLine("^ Too many parameters, complex logic, hard to understand and maintain!");
        
        Console.WriteLine("\n✅ GOOD: Simple, focused methods");
        var logger = new ConsoleLogger();
        var goodCalculator = new PriceCalculator(logger);
        
        // Simple, clear steps
        var items = new List<OrderItem>
        {
            new OrderItem("Widget A", 2, 100m),
            new OrderItem("Widget B", 1, 50m)
        };
        
        var baseTotal = goodCalculator.CalculateTotal(items);
        var discountedTotal = goodCalculator.ApplyDiscount(baseTotal, 15); // 15% discount
        var finalTotal = goodCalculator.ApplyTax(discountedTotal, 10); // 10% tax
        
        Console.WriteLine($"Good calculator result: ${finalTotal:F2}");
        Console.WriteLine("^ Simple methods, easy to understand, easy to test, easy to maintain!");
        
        Console.WriteLine("\n💡 KISS Principle Benefits:");
        Console.WriteLine("  • Easier to understand and maintain");
        Console.WriteLine("  • Fewer bugs");
        Console.WriteLine("  • Easier to test");
        Console.WriteLine("  • Faster development");
        Console.WriteLine("  • Better collaboration");
    }
}
