## Practice Exercises

Want to solidify your understanding? Try these hands-on exercises!

### Exercise 1: Fix the SRP Violation
Look at this class that violates SRP:

```csharp
public class ReportService
{
    public void GenerateReport(List<Sale> sales)
    {
        // Calculate totals
        var total = sales.Sum(s => s.Amount);
        
        // Format data
        var formattedData = FormatSalesData(sales);
        
        // Send email
        var emailBody = $"Report: {formattedData}";
        SendEmail("manager@company.com", emailBody);
        
        // Save to file
        File.WriteAllText("report.txt", formattedData);
        
        // Log activity
        Console.WriteLine($"Report generated at {DateTime.Now}");
    }
}
```

**Your Task**: Split this into separate classes following SRP. Think about what responsibilities this class has.

### Exercise 2: Apply OCP
You have this discount calculator:

```csharp
public class DiscountCalculator
{
    public decimal Calculate(decimal amount, string customerType)
    {
        if (customerType == "Regular")
            return amount * 0.95m; // 5% discount
        else if (customerType == "VIP") 
            return amount * 0.9m;  // 10% discount
        else if (customerType == "Premium")
            return amount * 0.8m;  // 20% discount
        else
            return amount;
    }
}
```

**Your Task**: Refactor this to follow OCP so you can add new customer types without modifying the calculator.

### Exercise 3: Fix LSP Violation
This code violates LSP:

```csharp
public class Vehicle
{
    public virtual void StartEngine() 
    { 
        Console.WriteLine("Engine started"); 
    }
}

public class Bicycle : Vehicle
{
    public override void StartEngine()
    {
        throw new NotSupportedException("Bicycles don't have engines!");
    }
}
```

**Your Task**: Redesign this hierarchy to follow LSP.

### Exercise 4: Apply ISP
This interface is too fat:

```csharp
public interface IEmployee
{
    void Work();
    void GetPaid();
    void ManageTeam();
    void WriteCode();
    void ReviewCode();
    void ConductMeetings();
}
```

**Your Task**: Break this into smaller, focused interfaces.

### Exercise 5: Apply DIP
This class violates DIP:

```csharp
public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        var emailSender = new EmailSender();
        var paymentProcessor = new CreditCardProcessor();
        var inventory = new DatabaseInventory();
        
        paymentProcessor.ProcessPayment(order.Total);
        inventory.UpdateStock(order.Items);
        emailSender.SendConfirmation(order.CustomerEmail);
    }
}
```

**Your Task**: Refactor to use dependency injection.

### Bonus Challenge: Build a Simple System

Create a simple library management system that demonstrates all SOLID principles:

**Requirements**:
- Books can be borrowed and returned
- Different user types (Student, Faculty, Guest) have different borrowing limits
- Send notifications when books are due
- Generate reports of overdue books
- Support different notification methods (Email, SMS)

**Your Task**: Design this system following all SOLID and KISS principles.

---
