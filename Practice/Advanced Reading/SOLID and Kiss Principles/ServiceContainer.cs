using SOLID_and_Kiss_Principles.GoodExamples;
using SOLID_and_Kiss_Principles.Interfaces;

namespace SOLID_and_Kiss_Principles;

/// <summary>
/// Simple dependency injection container for our demo
/// In real projects, you'd use Microsoft.Extensions.DependencyInjection or similar
/// 
/// This shows how DIP makes your application flexible and testable
/// You can easily swap implementations without changing business logic
/// </summary>
public static class ServiceContainer
{
    /// <summary>
    /// Configure all our dependencies in one place
    /// This is where you decide which implementations to use
    /// </summary>
    public static ServiceRegistrations CreateServices()
    {
        // Start with the logger - everything depends on it
        var logger = new ConsoleLogger();
        
        // Build up our service chain
        var emailService = new EmailService(logger);
        var userRepository = new UserRepository(logger);
        var notificationService = new NotificationService(logger);
        var priceCalculator = new PriceCalculator(logger);
        
        // Wire up business services
        var userService = new UserService(emailService, userRepository, logger);
        var areaCalculator = new AreaCalculator(logger);
        
        // Create workplace services
        var manager = new Manager(notificationService, logger);
        var teamLead = new TeamLead(notificationService, logger);
        
        return new ServiceRegistrations
        {
            Logger = logger,
            EmailService = emailService,
            UserRepository = userRepository,
            NotificationService = notificationService,
            PriceCalculator = priceCalculator,
            UserService = userService,
            AreaCalculator = areaCalculator,
            Manager = manager,
            TeamLead = teamLead
        };
    }
}

/// <summary>
/// Simple container to hold our service instances
/// In real apps, you'd use a proper DI container
/// </summary>
public class ServiceRegistrations
{
    public ILogger Logger { get; set; } = null!;
    public IEmailService EmailService { get; set; } = null!;
    public IUserRepository UserRepository { get; set; } = null!;
    public INotificationService NotificationService { get; set; } = null!;
    public IPriceCalculator PriceCalculator { get; set; } = null!;
    public UserService UserService { get; set; } = null!;
    public AreaCalculator AreaCalculator { get; set; } = null!;
    public Manager Manager { get; set; } = null!;
    public TeamLead TeamLead { get; set; } = null!;
}

/// <summary>
/// Example of how you might create different configurations
/// for different environments (Development, Testing, Production)
/// </summary>
public static class EnvironmentConfigurations
{
    /// <summary>
    /// Development configuration - might use console logging, in-memory storage
    /// </summary>
    public static ServiceRegistrations CreateDevelopmentServices()
    {
        return ServiceContainer.CreateServices(); // Same as default for this demo
    }
    
    /// <summary>
    /// Production configuration - might use file logging, database storage
    /// </summary>
    public static ServiceRegistrations CreateProductionServices()
    {
        // In real life, you'd swap out implementations here
        // For example: new FileLogger() instead of ConsoleLogger()
        // or new DatabaseUserRepository() instead of InMemoryUserRepository()
        return ServiceContainer.CreateServices();
    }
    
    /// <summary>
    /// Test configuration - might use mock services, no real email sending
    /// </summary>
    public static ServiceRegistrations CreateTestServices()
    {
        // In tests, you'd use mock implementations
        // For example: new MockEmailService() that doesn't actually send emails
        return ServiceContainer.CreateServices();
    }
}

/*
 * 💡 Key Benefits of This Approach:
 * 
 * 1. TESTABILITY: Easy to swap real services with mocks for testing
 * 2. FLEXIBILITY: Change implementations without changing business logic  
 * 3. MAINTAINABILITY: All dependencies configured in one place
 * 4. SEPARATION: Business logic doesn't know about concrete implementations
 * 5. SCALABILITY: Easy to add new services and dependencies
 * 
 * This is Dependency Inversion Principle in action!
 */
