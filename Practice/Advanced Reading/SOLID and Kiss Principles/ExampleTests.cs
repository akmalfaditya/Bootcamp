using SOLID_and_Kiss_Principles.GoodExamples;
using SOLID_and_Kiss_Principles.Interfaces;
using SOLID_and_Kiss_Principles.Models;

namespace SOLID_and_Kiss_Principles.Tests;

/// <summary>
/// Example unit tests showing how SOLID principles make testing easier
/// 
/// Notice how we can easily mock dependencies because we used interfaces
/// This is one of the biggest benefits of following SOLID principles
/// </summary>
public static class ExampleTests
{
    /// <summary>
    /// Run some basic tests to show how testable our design is
    /// In a real project, you'd use a testing framework like xUnit or NUnit
    /// </summary>
    public static void RunExampleTests()
    {
        Console.WriteLine("🧪 RUNNING EXAMPLE TESTS");
        Console.WriteLine("========================");
        Console.WriteLine("These show how SOLID principles make testing easier\n");
        
        TestEmailValidation();
        TestPriceCalculation();
        TestShapeAreas();
        TestUserRegistration();
        
        Console.WriteLine("✅ All tests passed! Notice how easy it was to test isolated functionality.");
    }
    
    static void TestEmailValidation()
    {
        Console.WriteLine("Testing email validation...");
        
        var mockLogger = new MockLogger();
        var emailService = new EmailService(mockLogger);
        
        // Test valid emails
        assert(emailService.ValidateEmail("test@example.com"), "Should accept valid email");
        assert(emailService.ValidateEmail("user.name@domain.co.uk"), "Should accept email with dots");
        
        // Test invalid emails
        assert(!emailService.ValidateEmail("invalid-email"), "Should reject email without @");
        assert(!emailService.ValidateEmail("@domain.com"), "Should reject email without username");
        assert(!emailService.ValidateEmail("user@"), "Should reject email without domain");
        
        Console.WriteLine("✅ Email validation tests passed");
    }
    
    static void TestPriceCalculation()
    {
        Console.WriteLine("Testing price calculations...");
        
        var mockLogger = new MockLogger();
        var calculator = new PriceCalculator(mockLogger);
        
        var items = new List<OrderItem>
        {
            new OrderItem("Item1", 2, 10.00m),
            new OrderItem("Item2", 1, 5.00m)
        };
        
        var total = calculator.CalculateTotal(items);
        assert(total == 25.00m, $"Expected 25.00, got {total}");
        
        var discounted = calculator.ApplyDiscount(total, 10);
        assert(discounted == 22.50m, $"Expected 22.50 after 10% discount, got {discounted}");
        
        var withTax = calculator.ApplyTax(discounted, 8);
        assert(withTax == 24.30m, $"Expected 24.30 after 8% tax, got {withTax}");
        
        Console.WriteLine("✅ Price calculation tests passed");
    }
    
    static void TestShapeAreas()
    {
        Console.WriteLine("Testing shape area calculations...");
        
        var rectangle = new Rectangle(5, 4);
        assert(rectangle.Area() == 20, $"Rectangle area should be 20, got {rectangle.Area()}");
        
        var circle = new Circle(3);
        var expectedArea = Math.PI * 9; // π * r²
        assert(Math.Abs(circle.Area() - expectedArea) < 0.001, "Circle area calculation failed");
        
        var triangle = new Triangle(6, 4);
        assert(triangle.Area() == 12, $"Triangle area should be 12, got {triangle.Area()}");
        
        Console.WriteLine("✅ Shape area tests passed");
    }
    
    static void TestUserRegistration()
    {
        Console.WriteLine("Testing user registration...");
        
        var mockLogger = new MockLogger();
        var mockEmailService = new MockEmailService();
        var mockUserRepository = new MockUserRepository();
        
        var userService = new UserService(mockEmailService, mockUserRepository, mockLogger);
        
        // Test successful registration
        userService.RegisterUser("newuser@example.com", "password123");
        assert(mockUserRepository.WasSaveCalled, "User should have been saved");
        assert(mockEmailService.WasWelcomeEmailSent, "Welcome email should have been sent");
        
        Console.WriteLine("✅ User registration tests passed");
    }
    
    /// <summary>
    /// Simple assertion helper for our demo tests
    /// </summary>
    static void assert(bool condition, string message)
    {
        if (!condition)
            throw new Exception($"Test failed: {message}");
    }
}

/// <summary>
/// Mock implementations for testing
/// These replace real services during testing so we can:
/// 1. Test in isolation
/// 2. Verify interactions
/// 3. Control behavior
/// </summary>

public class MockLogger : ILogger
{
    public List<string> InfoMessages { get; } = new();
    public List<string> ErrorMessages { get; } = new();
    
    public void LogInfo(string message) => InfoMessages.Add(message);
    public void LogError(string message) => ErrorMessages.Add(message);
    public void LogError(Exception exception) => ErrorMessages.Add(exception.Message);
}

public class MockEmailService : IEmailService
{
    public bool WasWelcomeEmailSent { get; private set; }
    public bool WasOrderConfirmationSent { get; private set; }
    
    public bool ValidateEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
    
    public void SendWelcomeEmail(string email)
    {
        WasWelcomeEmailSent = true;
    }
    
    public void SendOrderConfirmation(string email, string orderId)
    {
        WasOrderConfirmationSent = true;
    }
}

public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    public bool WasSaveCalled { get; private set; }
    
    public void SaveUser(User user)
    {
        _users.Add(user);
        WasSaveCalled = true;
    }
    
    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }
    
    public bool UserExists(string email)
    {
        return _users.Any(u => u.Email == email);
    }
}

/*
 * 💡 Testing Benefits from SOLID Principles:
 * 
 * 1. SRP: Each class has focused responsibility, easy to test one thing
 * 2. OCP: Can test new implementations without changing existing tests
 * 3. LSP: All implementations of interface behave consistently
 * 4. ISP: Can mock only the interfaces we need for each test
 * 5. DIP: Can inject mock dependencies to isolate code under test
 * 
 * This is why SOLID principles are so valuable - they make your code testable!
 */
