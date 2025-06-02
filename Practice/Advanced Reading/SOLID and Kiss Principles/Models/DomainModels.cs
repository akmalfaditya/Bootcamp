namespace SOLID_and_Kiss_Principles.Models;

/// <summary>
/// Basic user model - keeping it simple, just what we need
/// Notice how this follows KISS - no over-engineering here
/// </summary>
public class User
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public User(string email, string password)
    {
        Email = email;
        Password = password;
        CreatedAt = DateTime.Now;
        IsActive = true;
    }
}

/// <summary>
/// A simple order model for our e-commerce demo
/// Again, keeping it simple - only essential properties
/// </summary>
public class Order
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public Order(string customerEmail)
    {
        CustomerEmail = customerEmail;
        OrderDate = DateTime.Now;
        Items = new List<OrderItem>();
    }
}

/// <summary>
/// Order item - simple and focused
/// </summary>
public class OrderItem
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }

    public decimal TotalPrice => Quantity * PricePerItem;

    public OrderItem(string productName, int quantity, decimal pricePerItem)
    {
        ProductName = productName;
        Quantity = quantity;
        PricePerItem = pricePerItem;
    }
}
