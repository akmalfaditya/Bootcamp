using System.Net.Mail;
using SOLID_and_Kiss_Principles.Models;

namespace SOLID_and_Kiss_Principles.BadExamples;

/// <summary>
/// BAD EXAMPLE - Violates Single Responsibility Principle
/// This class does WAY too much:
/// 1. User registration
/// 2. Email validation  
/// 3. Sending emails
/// 4. Database operations
/// 
/// Problem: If email logic changes, we have to modify this class
/// Problem: If validation rules change, we have to modify this class  
/// Problem: Hard to test individual pieces
/// Problem: Hard to reuse email functionality elsewhere
/// </summary>
public class BadUserService
{
    // Look at all these different responsibilities in one class!
    
    public void RegisterUser(string email, string password)
    {
        // Validation logic - should be separate
        if (!ValidateEmail(email))
            throw new ArgumentException("Invalid email format");
            
        if (string.IsNullOrEmpty(password) || password.Length < 6)
            throw new ArgumentException("Password too weak");
            
        // User creation logic
        var user = new User(email, password);
        
        // Database logic - should be separate
        SaveUserToDatabase(user);
        
        // Email logic - should be separate  
        SendWelcomeEmail(email);
        
        // Logging logic - should be separate
        LogUserRegistration(email);
    }
    
    // Email validation mixed with user logic
    private bool ValidateEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
    
    // Database operations mixed in
    private void SaveUserToDatabase(User user)
    {
        // Simulated database save
        Console.WriteLine($"Saving user {user.Email} to database...");
        // In real life, this would be database code
    }
    
    // Email sending mixed in
    private void SendWelcomeEmail(string email)
    {
        // Email logic - should be in email service
        Console.WriteLine($"Sending welcome email to {email}");
        // In real life, this would be SMTP code
    }
    
    // Logging mixed in
    private void LogUserRegistration(string email)
    {
        Console.WriteLine($"User registered: {email} at {DateTime.Now}");
    }
}

/// <summary>
/// BAD EXAMPLE - Violates Open/Closed Principle
/// Every time we add a new shape, we have to modify this class
/// This makes the code fragile and harder to maintain
/// </summary>
public class BadAreaCalculator
{
    // Look how we have to modify this method for each new shape
    public double CalculateTotalArea(object[] shapes)
    {
        double totalArea = 0;
        
        foreach (var shape in shapes)
        {
            // Adding new shapes requires modifying this method
            // This violates Open/Closed Principle
            if (shape is BadRectangle rectangle)
            {
                totalArea += rectangle.Width * rectangle.Height;
            }
            else if (shape is BadCircle circle)
            {
                totalArea += Math.PI * circle.Radius * circle.Radius;
            }
            // What if we want to add Triangle? We'd have to modify this!
            // else if (shape is Triangle triangle) { ... }
            else
            {
                throw new ArgumentException($"Unknown shape type: {shape.GetType()}");
            }
        }
        
        return totalArea;
    }
}

// Bad shape classes - no common interface or base class
public class BadRectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
}

public class BadCircle  
{
    public double Radius { get; set; }
}

/// <summary>
/// BAD EXAMPLE - Violates Liskov Substitution Principle
/// Penguin can't fly, but inherits from Bird which has Fly()
/// This breaks the substitution principle
/// </summary>
public class BadBird
{
    public virtual void Fly()
    {
        Console.WriteLine("Bird is flying");
    }
    
    public virtual void Eat()
    {
        Console.WriteLine("Bird is eating");
    }
}

public class BadSparrow : BadBird
{
    public override void Fly()
    {
        Console.WriteLine("Sparrow flies fast");
    }
}

// THIS IS THE PROBLEM - Penguin can't fly!
public class BadPenguin : BadBird
{
    public override void Fly()
    {
        // This breaks LSP - we can't substitute Penguin for Bird safely
        throw new NotSupportedException("Penguins can't fly!");
    }
}

/// <summary>
/// BAD EXAMPLE - Violates Interface Segregation Principle
/// Forces classes to implement methods they don't need
/// </summary>
public interface IBadLead
{
    void AssignTask();
    void CreateSubTask();
    void WorkOnTask();     // Managers shouldn't need this!
    void ReviewCode();     // Not all leads do code review!
    void ConductMeeting(); // What if someone doesn't conduct meetings?
}

// Manager forced to implement work methods
public class BadManager : IBadLead
{
    public void AssignTask()
    {
        Console.WriteLine("Manager assigning task");
    }
    
    public void CreateSubTask()
    {
        Console.WriteLine("Manager creating subtask");
    }
    
    // Forced to implement this even though managers don't work on tasks
    public void WorkOnTask()
    {
        throw new NotImplementedException("Managers don't work on tasks!");
    }
    
    // Forced to implement this too
    public void ReviewCode()
    {
        throw new NotImplementedException("Not all managers review code!");
    }
    
    public void ConductMeeting()
    {
        Console.WriteLine("Manager conducting meeting");
    }
}

/// <summary>
/// BAD EXAMPLE - Violates Dependency Inversion Principle
/// Directly depends on concrete classes instead of abstractions
/// Hard to test, hard to change implementations
/// </summary>
public class BadOrderService
{
    // Direct dependencies on concrete classes - BAD!
    private BadEmailSender _emailSender;
    private BadFileLogger _logger;
    private BadDatabaseRepository _repository;
    
    public BadOrderService()
    {
        // Creating dependencies directly - tightly coupled!
        _emailSender = new BadEmailSender();
        _logger = new BadFileLogger();
        _repository = new BadDatabaseRepository();
    }
    
    public void ProcessOrder(Order order)
    {
        // What if we want to use a different logger? Email sender? Database?
        // We'd have to modify this class!
        
        _repository.SaveOrder(order);
        _emailSender.SendOrderConfirmation(order.CustomerEmail);
        _logger.Log($"Order processed: {order.Id}");
    }
}

// Concrete classes that are tightly coupled
public class BadEmailSender
{
    public void SendOrderConfirmation(string email)
    {
        Console.WriteLine($"Sending email to {email}");
    }
}

public class BadFileLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"Logging to file: {message}");
    }
}

public class BadDatabaseRepository
{
    public void SaveOrder(Order order)
    {
        Console.WriteLine($"Saving order {order.Id} to database");
    }
}

/// <summary>
/// BAD EXAMPLE - Violates KISS Principle
/// Over-engineered solution for a simple calculation
/// </summary>
public class BadPriceCalculator
{
    // Way too complex for simple calculations!
    public decimal CalculatePrice(int quantity, decimal pricePerItem, 
        bool applyDiscount = false, bool isTaxable = false, 
        bool isVip = false, bool isWeekend = false, 
        bool isHoliday = false, string customerType = "regular")
    {
        decimal basePrice = quantity * pricePerItem;
        
        // This is getting ridiculous...
        if (applyDiscount)
        {
            if (isVip)
            {
                if (customerType == "premium")
                {
                    if (isWeekend)
                        basePrice *= 0.8m; // 20% discount
                    else if (isHoliday)
                        basePrice *= 0.75m; // 25% discount
                    else
                        basePrice *= 0.85m; // 15% discount
                }
                else
                {
                    basePrice *= 0.9m; // 10% discount
                }
            }
            else
            {
                basePrice *= 0.95m; // 5% discount
            }
        }
        
        if (isTaxable)
        {
            if (customerType == "business")
                basePrice *= 1.15m; // 15% tax
            else
                basePrice *= 1.1m; // 10% tax
        }
        
        return basePrice;
    }
}
