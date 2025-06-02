using SOLID_and_Kiss_Principles.Interfaces;
using SOLID_and_Kiss_Principles.Models;

namespace SOLID_and_Kiss_Principles.GoodExamples;

/// <summary>
/// GOOD EXAMPLE - Follows Single Responsibility Principle
/// This class ONLY handles user registration logic
/// All other concerns are delegated to appropriate services
/// </summary>
public class UserService
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;
    
    // Dependency injection - follows Dependency Inversion Principle
    public UserService(IEmailService emailService, IUserRepository userRepository, ILogger logger)
    {
        _emailService = emailService;
        _userRepository = userRepository;
        _logger = logger;
    }
    
    // Clean, focused method with single responsibility
    public void RegisterUser(string email, string password)
    {
        try
        {
            // Validate input
            if (!_emailService.ValidateEmail(email))
                throw new ArgumentException("Invalid email format");
                
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters");
                
            // Check if user already exists
            if (_userRepository.UserExists(email))
                throw new InvalidOperationException("User already exists");
            
            // Create and save user
            var user = new User(email, password);
            _userRepository.SaveUser(user);
            
            // Send welcome email
            _emailService.SendWelcomeEmail(email);
            
            // Log the registration
            _logger.LogInfo($"User registered successfully: {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to register user {email}: {ex.Message}");
            throw;
        }
    }
}

/// <summary>
/// GOOD EXAMPLE - Email service with single responsibility
/// Only handles email-related operations
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger _logger;
    
    public EmailService(ILogger logger)
    {
        _logger = logger;
    }
    
    public bool ValidateEmail(string email)
    {
        // Simple but effective validation - KISS principle
        return !string.IsNullOrWhiteSpace(email) && 
               email.Contains("@") && 
               email.Contains(".") &&
               email.IndexOf("@") < email.LastIndexOf(".");
    }
    
    public void SendWelcomeEmail(string email)
    {
        // In real life, this would use an email service like SendGrid
        Console.WriteLine($"📧 Sending welcome email to: {email}");
        _logger.LogInfo($"Welcome email sent to {email}");
    }
    
    public void SendOrderConfirmation(string email, string orderId)
    {
        Console.WriteLine($"📧 Sending order confirmation to: {email} for order: {orderId}");
        _logger.LogInfo($"Order confirmation sent to {email}");
    }
}

/// <summary>
/// GOOD EXAMPLE - User repository with single responsibility  
/// Only handles user data operations
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new(); // In-memory storage for demo
    private readonly ILogger _logger;
    
    public UserRepository(ILogger logger)
    {
        _logger = logger;
    }
    
    public void SaveUser(User user)
    {
        _users.Add(user);
        _logger.LogInfo($"User saved: {user.Email}");
    }
    
    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
    
    public bool UserExists(string email)
    {
        return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
}

/// <summary>
/// GOOD EXAMPLE - Simple logger implementation
/// Focused on logging only - KISS principle
/// </summary>
public class ConsoleLogger : ILogger
{
    public void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
    }
    
    public void LogError(string message)
    {
        Console.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss} - {message}");
    }
    
    public void LogError(Exception exception)
    {
        Console.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss} - {exception.Message}");
    }
}

/// <summary>
/// GOOD EXAMPLE - Follows Open/Closed Principle
/// Can handle any shape without modification
/// Open for extension (new shapes), closed for modification
/// </summary>
public class AreaCalculator
{
    private readonly ILogger _logger;
    
    public AreaCalculator(ILogger logger)
    {
        _logger = logger;
    }
    
    // This method never needs to change when we add new shapes!
    public double CalculateTotalArea(Shape[] shapes)
    {
        double totalArea = 0;
        
        foreach (var shape in shapes)
        {
            var area = shape.Area();
            totalArea += area;
            
            _logger.LogInfo($"Calculated area for {shape.GetShapeInfo()}");
        }
        
        _logger.LogInfo($"Total area of all shapes: {totalArea:F2}");
        return totalArea;
    }
}

/// <summary>
/// GOOD EXAMPLE - Manager that follows Interface Segregation Principle
/// Only implements interfaces relevant to management tasks
/// </summary>
public class Manager : ITaskManager
{
    private readonly INotificationService _notificationService;
    private readonly ILogger _logger;
    
    public Manager(INotificationService notificationService, ILogger logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }
    
    public void AssignTask(string taskName, string assignee)
    {
        Console.WriteLine($"👔 Manager assigning '{taskName}' to {assignee}");
        _notificationService.SendTaskAssignment(assignee, taskName);
        _logger.LogInfo($"Task '{taskName}' assigned to {assignee}");
    }
    
    public void CreateSubTask(string parentTask, string subTask)
    {
        Console.WriteLine($"👔 Manager creating subtask '{subTask}' under '{parentTask}'");
        _logger.LogInfo($"Subtask '{subTask}' created under '{parentTask}'");
    }
    
    public void ReviewTask(string taskName)
    {
        Console.WriteLine($"👔 Manager reviewing task: '{taskName}'");
        _logger.LogInfo($"Task reviewed: '{taskName}'");
    }
}

/// <summary>
/// GOOD EXAMPLE - TeamLead that implements both management and work interfaces
/// Shows how ISP allows flexible implementations
/// </summary>
public class TeamLead : ITaskManager, IWorker
{
    private readonly INotificationService _notificationService;
    private readonly ILogger _logger;
    
    public TeamLead(INotificationService notificationService, ILogger logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }
    
    // Management responsibilities
    public void AssignTask(string taskName, string assignee)
    {
        Console.WriteLine($"👨‍💼 TeamLead assigning '{taskName}' to {assignee}");
        _notificationService.SendTaskAssignment(assignee, taskName);
        _logger.LogInfo($"Task '{taskName}' assigned to {assignee}");
    }
    
    public void CreateSubTask(string parentTask, string subTask)
    {
        Console.WriteLine($"👨‍💼 TeamLead creating subtask '{subTask}' under '{parentTask}'");
        _logger.LogInfo($"Subtask '{subTask}' created under '{parentTask}'");
    }
    
    public void ReviewTask(string taskName)
    {
        Console.WriteLine($"👨‍💼 TeamLead reviewing task: '{taskName}'");
        _logger.LogInfo($"Task reviewed: '{taskName}'");
    }
    
    // Work responsibilities - TeamLead can also work on tasks
    public void WorkOnTask(string taskName)
    {
        Console.WriteLine($"👨‍💼 TeamLead working on task: '{taskName}'");
        _logger.LogInfo($"TeamLead started working on '{taskName}'");
    }
    
    public void CompleteTask(string taskName)
    {
        Console.WriteLine($"👨‍💼 TeamLead completed task: '{taskName}'");
        _notificationService.SendTaskCompletion("manager@company.com", taskName);
        _logger.LogInfo($"Task completed: '{taskName}'");
    }
    
    public void ReportProgress(string taskName, int progressPercentage)
    {
        Console.WriteLine($"👨‍💼 TeamLead reports {progressPercentage}% progress on '{taskName}'");
        _logger.LogInfo($"Progress reported: {progressPercentage}% on '{taskName}'");
    }
}

/// <summary>
/// GOOD EXAMPLE - Simple notification service
/// Follows KISS - does what it needs to do, nothing more
/// </summary>
public class NotificationService : INotificationService
{
    private readonly ILogger _logger;
    
    public NotificationService(ILogger logger)
    {
        _logger = logger;
    }
    
    public void SendTaskAssignment(string recipient, string taskName)
    {
        Console.WriteLine($"📬 Notification sent to {recipient}: You've been assigned '{taskName}'");
        _logger.LogInfo($"Task assignment notification sent to {recipient}");
    }
    
    public void SendTaskCompletion(string recipient, string taskName)
    {
        Console.WriteLine($"📬 Notification sent to {recipient}: Task '{taskName}' has been completed");
        _logger.LogInfo($"Task completion notification sent to {recipient}");
    }
}

/// <summary>
/// GOOD EXAMPLE - Simple price calculator following KISS principle
/// Clean, focused methods that are easy to understand and test
/// </summary>
public class PriceCalculator : IPriceCalculator
{
    private readonly ILogger _logger;
    
    public PriceCalculator(ILogger logger)
    {
        _logger = logger;
    }
    
    // Simple, focused method - KISS principle
    public decimal CalculateTotal(List<OrderItem> items)
    {
        decimal total = items.Sum(item => item.TotalPrice);
        _logger.LogInfo($"Base total calculated: ${total:F2}");
        return total;
    }
    
    // Simple discount calculation
    public decimal ApplyDiscount(decimal total, decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100");
            
        decimal discountAmount = total * (discountPercentage / 100);
        decimal discountedTotal = total - discountAmount;
        
        _logger.LogInfo($"Applied {discountPercentage}% discount: ${discountAmount:F2}");
        return discountedTotal;
    }
    
    // Simple tax calculation
    public decimal ApplyTax(decimal total, decimal taxRate)
    {
        if (taxRate < 0) 
            throw new ArgumentException("Tax rate cannot be negative");
            
        decimal taxAmount = total * (taxRate / 100);
        decimal totalWithTax = total + taxAmount;
        
        _logger.LogInfo($"Applied {taxRate}% tax: ${taxAmount:F2}");
        return totalWithTax;
    }
}
