namespace StructuredLogging.Demo.Models
{
    /// <summary>
    /// Order model representing an e-commerce order
    /// This model demonstrates structured logging in business operations
    /// including order processing, status changes, and payment tracking
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        
        // Shipping information for structured logging examples
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingMethod { get; set; } = string.Empty;
        public string? TrackingNumber { get; set; }
        
        // Payment information
        public string PaymentMethod { get; set; } = string.Empty;
        public string? PaymentTransactionId { get; set; }
        
        // Items collection - this will help us demonstrate structured logging with collections
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        
        /// <summary>
        /// Calculate total items count for logging purposes
        /// </summary>
        public int TotalItemsCount => Items?.Sum(i => i.Quantity) ?? 0;
    }

    /// <summary>
    /// Order status enumeration for demonstrating structured logging with enums
    /// This helps us track order lifecycle events with proper context
    /// </summary>
    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        PaymentConfirmed = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Refunded = 6
    }

    /// <summary>
    /// Individual order item for demonstrating structured logging with nested objects
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
