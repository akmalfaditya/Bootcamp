# Tell, Don't Ask - Practice Exercises

## 🎯 Exercise Instructions

These exercises are designed to help you practice identifying and refactoring "Ask" patterns into "Tell" patterns. Start with the simpler exercises and work your way up.

---

## 📚 Exercise 1: Student Grade Calculator (Beginner)

### Current Code (Ask Approach)
```csharp
public class Student 
{
    public string Name { get; set; }
    public List<int> Grades { get; set; } = new();
    public bool IsHonorStudent { get; set; }
}

// External usage
public class GradeProcessor 
{
    public void ProcessStudent(Student student) 
    {
        if (student.Grades.Count == 0) 
        {
            Console.WriteLine($"{student.Name}: No grades available");
            return;
        }
        
        double average = student.Grades.Average();
        student.IsHonorStudent = average >= 90;
        
        if (student.IsHonorStudent) 
        {
            Console.WriteLine($"{student.Name}: Honor Student (Average: {average:F1})");
        } 
        else if (average >= 70) 
        {
            Console.WriteLine($"{student.Name}: Passing (Average: {average:F1})");
        } 
        else 
        {
            Console.WriteLine($"{student.Name}: Needs Improvement (Average: {average:F1})");
        }
    }
}
```

### Your Task
Refactor this to use the Tell approach. The Student class should:
- Handle its own grade calculations
- Determine its own honor status
- Provide meaningful operations instead of exposing data
- Generate its own status reports

### Hints
- Add methods like `AddGrade()`, `CalculateAverage()`, `GetAcademicStatus()`
- Move the decision logic into the Student class
- Make the Grades list private
- Consider what operations a Student should be able to perform

---

## 🏦 Exercise 2: Library Book System (Intermediate)

### Current Code (Ask Approach)
```csharp
public class Book 
{
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsCheckedOut { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public string CheckedOutBy { get; set; }
    public int MaxLoanDays { get; set; } = 14;
}

public class LibrarySystem 
{
    public bool CheckOutBook(Book book, string memberName) 
    {
        if (book.IsCheckedOut) 
        {
            Console.WriteLine($"Book '{book.Title}' is already checked out");
            return false;
        }
        
        book.IsCheckedOut = true;
        book.CheckOutDate = DateTime.Now;
        book.CheckedOutBy = memberName;
        Console.WriteLine($"Book '{book.Title}' checked out to {memberName}");
        return true;
    }
    
    public bool ReturnBook(Book book) 
    {
        if (!book.IsCheckedOut) 
        {
            Console.WriteLine($"Book '{book.Title}' is not checked out");
            return false;
        }
        
        var daysBorrowed = (DateTime.Now - book.CheckOutDate.Value).Days;
        if (daysBorrowed > book.MaxLoanDays) 
        {
            var overdueDays = daysBorrowed - book.MaxLoanDays;
            Console.WriteLine($"Book '{book.Title}' returned {overdueDays} days late!");
        }
        
        book.IsCheckedOut = false;
        book.CheckOutDate = null;
        book.CheckedOutBy = null;
        Console.WriteLine($"Book '{book.Title}' returned successfully");
        return true;
    }
    
    public void CheckOverdueBooks(List<Book> books) 
    {
        foreach (var book in books) 
        {
            if (book.IsCheckedOut && book.CheckOutDate.HasValue) 
            {
                var daysBorrowed = (DateTime.Now - book.CheckOutDate.Value).Days;
                if (daysBorrowed > book.MaxLoanDays) 
                {
                    Console.WriteLine($"OVERDUE: '{book.Title}' by {book.CheckedOutBy}");
                }
            }
        }
    }
}
```

### Your Task
Refactor this to use the Tell approach. The Book class should:
- Handle its own checkout/return logic
- Calculate if it's overdue
- Validate checkout/return operations
- Provide status information without exposing internal state

### Questions to Consider
- What operations should a Book be able to perform?
- How can the Book protect its own business rules?
- What information should the Book provide vs. what should it keep private?
- How can you eliminate the external manipulation of Book state?

---

## 🏪 Exercise 3: Inventory Management System (Advanced)

### Current Code (Ask Approach)
```csharp
public class InventoryItem 
{
    public string ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public int MinimumStock { get; set; }
    public int MaximumStock { get; set; }
    public decimal UnitPrice { get; set; }
    public List<StockMovement> Movements { get; set; } = new();
    public DateTime LastRestocked { get; set; }
}

public class StockMovement 
{
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; } // "IN", "OUT", "ADJUSTMENT"
    public string Reason { get; set; }
}

public class InventoryManager 
{
    public bool ProcessSale(InventoryItem item, int requestedQuantity) 
    {
        if (item.Quantity < requestedQuantity) 
        {
            Console.WriteLine($"Insufficient stock for {item.Name}. Available: {item.Quantity}");
            return false;
        }
        
        item.Quantity -= requestedQuantity;
        item.Movements.Add(new StockMovement 
        {
            Date = DateTime.Now,
            Quantity = -requestedQuantity,
            Type = "OUT",
            Reason = "Sale"
        });
        
        if (item.Quantity <= item.MinimumStock) 
        {
            Console.WriteLine($"WARNING: {item.Name} is below minimum stock level!");
        }
        
        return true;
    }
    
    public void RestockItem(InventoryItem item, int quantity) 
    {
        if (item.Quantity + quantity > item.MaximumStock) 
        {
            Console.WriteLine($"Cannot restock {item.Name}. Would exceed maximum stock level.");
            return;
        }
        
        item.Quantity += quantity;
        item.LastRestocked = DateTime.Now;
        item.Movements.Add(new StockMovement 
        {
            Date = DateTime.Now,
            Quantity = quantity,
            Type = "IN",
            Reason = "Restock"
        });
        
        Console.WriteLine($"Restocked {item.Name}. New quantity: {item.Quantity}");
    }
    
    public void GenerateStockReport(List<InventoryItem> items) 
    {
        Console.WriteLine("INVENTORY REPORT");
        Console.WriteLine("================");
        
        foreach (var item in items) 
        {
            string status = "OK";
            if (item.Quantity <= item.MinimumStock) 
            {
                status = "LOW STOCK";
            } 
            else if (item.Quantity >= item.MaximumStock) 
            {
                status = "OVERSTOCKED";
            }
            
            decimal totalValue = item.Quantity * item.UnitPrice;
            var daysSinceRestock = (DateTime.Now - item.LastRestocked).Days;
            
            Console.WriteLine($"{item.Name}: {item.Quantity} units ({status}) - Value: {totalValue:C} - Last restocked: {daysSinceRestock} days ago");
        }
    }
}
```

### Your Task
This is a complex refactoring exercise. Transform this into a Tell-based design where:
- InventoryItem handles its own stock operations
- Business rules are encapsulated within the item
- External code tells items what to do rather than manipulating their state
- The InventoryManager becomes a coordinator rather than a manipulator

### Advanced Challenges
1. How do you handle the stock movement logging?
2. How do you provide reporting information without exposing internal collections?
3. How do you handle validation and business rules?
4. How do you maintain the ability to generate reports across multiple items?

### Hints
- Consider what operations an InventoryItem should support
- Think about what information it needs to provide for reporting
- Consider how to handle cross-cutting concerns like logging
- Remember that some coordination logic may still belong in a manager class

---

## 🎮 Exercise 4: Game Character System (Creative Challenge)

### Your Task
Design a game character system from scratch using the Tell, Don't Ask principle. Your system should include:

- Character classes (Warrior, Mage, Archer, etc.)
- Combat mechanics
- Inventory management
- Experience and leveling
- Equipment effects

### Requirements
- Characters should handle their own combat calculations
- Inventory should manage itself
- Equipment should affect character stats automatically
- Experience and leveling should be self-managed
- No external code should directly manipulate character state

### Design Questions
- How do characters interact with each other in combat?
- How does equipment modify character abilities?
- How do you handle character progression?
- What information do characters need to expose for the game UI?

---

## 🔍 Solution Guidelines

### For Each Exercise, Ask Yourself:

1. **What data is being accessed from outside the object?**
   - This data probably belongs inside methods

2. **What decisions are being made by external code?**
   - These decisions should move into the object

3. **What business rules are scattered across multiple places?**
   - Centralize these rules in the appropriate object

4. **What operations are being performed on the object's data?**
   - Turn these into methods on the object

5. **What would break if the internal structure changed?**
   - Design interfaces that are stable regardless of internals

### Success Criteria

Your refactored code should have:
- ✅ Objects that actively manage their own state
- ✅ Business logic encapsulated within relevant objects
- ✅ Minimal public getters/setters
- ✅ Methods that capture business intent
- ✅ Objects that can protect their own invariants
- ✅ Reduced coupling between classes
- ✅ Easier unit testing capabilities

### Common Pitfalls to Avoid

- ❌ Creating getters/setters for every field "just in case"
- ❌ Making everything private without providing useful operations
- ❌ Creating methods that just wrap property access
- ❌ Forgetting that some information still needs to be accessible
- ❌ Over-engineering simple scenarios

---

## 🚀 Next Steps

After completing these exercises:

1. **Review existing code** in your projects for Ask patterns
2. **Practice identifying** Tell opportunities in real codebases
3. **Start small** with refactoring - don't try to change everything at once
4. **Focus on high-value areas** where business logic is scattered
5. **Get feedback** from other developers on your designs

Remember: The goal is more maintainable, robust code - not perfect encapsulation at any cost!
