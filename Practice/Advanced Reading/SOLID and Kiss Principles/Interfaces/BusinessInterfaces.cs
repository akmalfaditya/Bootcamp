namespace SOLID_and_Kiss_Principles.Interfaces;

/// <summary>
/// Email service interface - focused on email operations only
/// Single Responsibility: handles email-related tasks
/// Dependency Inversion: allows different email implementations
/// </summary>
public interface IEmailService
{
    bool ValidateEmail(string email);
    void SendWelcomeEmail(string email);
    void SendOrderConfirmation(string email, string orderId);
}

/// <summary>
/// User repository interface - handles user data persistence
/// SRP: only deals with user data storage/retrieval
/// DIP: abstracts away the actual storage mechanism
/// </summary>
public interface IUserRepository
{
    void SaveUser(Models.User user);
    Models.User? GetUserByEmail(string email);
    bool UserExists(string email);
}

/// <summary>
/// Logger interface - simple and focused
/// ISP: only what's needed for logging
/// </summary>
public interface ILogger
{
    void LogInfo(string message);
    void LogError(string message);
    void LogError(Exception exception);
}

/// <summary>
/// Price calculator interface - handles pricing logic
/// SRP: focused only on price calculations
/// OCP: can be extended with new pricing strategies
/// </summary>
public interface IPriceCalculator
{
    decimal CalculateTotal(List<Models.OrderItem> items);
    decimal ApplyDiscount(decimal total, decimal discountPercentage);
    decimal ApplyTax(decimal total, decimal taxRate);
}
