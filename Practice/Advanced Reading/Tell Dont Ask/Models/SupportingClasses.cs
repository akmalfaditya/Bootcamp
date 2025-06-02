namespace Tell_Dont_Ask.Models;

/// <summary>
/// Simple alarm system for our monitoring examples
/// This class demonstrates proper encapsulation - it knows how to warn,
/// and callers just tell it to warn without needing to know implementation details
/// </summary>
public class Alarm
{
    private readonly List<string> _warnings = new();
    
    /// <summary>
    /// Tell the alarm to warn about something
    /// Notice: we're not asking the alarm for its state, we're telling it what to do
    /// </summary>
    public void Warn(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        var fullMessage = $"[{timestamp}] ALARM: {message}";
        
        Console.WriteLine($"🚨 {fullMessage}");
        _warnings.Add(fullMessage);
    }
    
    /// <summary>
    /// Sometimes we do need to query for information - that's okay!
    /// The key is that this doesn't expose internal implementation details
    /// </summary>
    public int GetWarningCount() => _warnings.Count;
    
    /// <summary>
    /// Clear all warnings - again, we're telling the alarm what to do
    /// </summary>
    public void ClearWarnings()
    {
        _warnings.Clear();
        Console.WriteLine("🔇 Alarm warnings cleared");
    }
}

/// <summary>
/// Product model for our shopping cart examples
/// Simple data class - not everything needs complex behavior
/// </summary>
public class Product
{
    public string Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public string Category { get; }
    
    public Product(string id, string name, decimal price, string category = "General")
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }
    
    public override string ToString() => $"{Name} (${Price:F2})";
}

/// <summary>
/// Shopping cart item - combines a product with quantity
/// Notice how it calculates its own subtotal instead of exposing quantity for external calculation
/// </summary>
public class CartItem
{
    public Product Product { get; }
    public int Quantity { get; set; } // Made public set for bad examples - normally would be private
    
    public CartItem(Product product, int quantity = 1)
    {
        Product = product;
        Quantity = quantity;
    }
    
    /// <summary>
    /// Tell the item to increase its quantity
    /// The item handles the logic internally
    /// </summary>
    public void IncreaseQuantity(int amount = 1)
    {
        if (amount <= 0) 
            throw new ArgumentException("Amount must be positive");
            
        Quantity += amount;
    }
    
    /// <summary>
    /// The item knows how to calculate its own subtotal
    /// We don't need to ask for quantity and price separately
    /// </summary>
    public decimal GetSubtotal() => Product.Price * Quantity;
    
    public override string ToString() => 
        $"{Quantity}x {Product.Name} = ${GetSubtotal():F2}";
}

/// <summary>
/// User credentials for security examples
/// Encapsulates password validation logic
/// </summary>
public class UserCredentials
{
    public string Username { get; }
    private readonly string _passwordHash;
    
    public UserCredentials(string username, string password)
    {
        Username = username;
        _passwordHash = HashPassword(password); // In real life, use proper hashing
    }
    
    /// <summary>
    /// Tell the credentials to verify a password
    /// We don't expose the hash, we just ask "is this password correct?"
    /// </summary>
    public bool VerifyPassword(string password)
    {
        return HashPassword(password) == _passwordHash;
    }
    
    private string HashPassword(string password)
    {
        // Simplified hashing for demo - use BCrypt or similar in real apps
        return password.GetHashCode().ToString();
    }
}

/// <summary>
/// Access level enumeration for security system
/// </summary>
public enum AccessLevel
{
    Guest = 1,
    Basic = 2,
    Employee = 3,
    Manager = 4,
    Admin = 5
}

/// <summary>
/// Represents a user in our security system
/// Encapsulates access level logic
/// </summary>
public class SecurityUser
{
    public string Name { get; }
    public string Username { get; }
    public AccessLevel AccessLevel { get; }
    public DateTime LastLogin { get; private set; }
      public SecurityUser(string username, AccessLevel accessLevel, string? name = null)
    {
        Username = username;
        Name = name ?? username;
        AccessLevel = accessLevel;
        LastLogin = DateTime.MinValue;
    }
    
    /// <summary>
    /// Tell the user they've logged in
    /// The user updates their own state
    /// </summary>
    public void RecordLogin()
    {
        LastLogin = DateTime.Now;
    }
    
    /// <summary>
    /// Ask the user if they can access something
    /// This is a query, but it encapsulates the access logic
    /// </summary>
    public bool CanAccess(AccessLevel requiredLevel)
    {
        return AccessLevel >= requiredLevel;
    }
    
    public override string ToString() => $"{Name} ({AccessLevel})";
}
