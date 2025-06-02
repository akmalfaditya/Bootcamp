namespace StructuredLogging.Demo.Models
{
    /// <summary>
    /// Login request model used for authentication demonstrations
    /// This helps us show structured logging for security events
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        
        // Additional fields for demonstrating structured logging context
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }

    /// <summary>
    /// Login result for demonstrating structured logging outcomes
    /// </summary>
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? User { get; set; }
        public string? SessionId { get; set; }
        public DateTime LoginTime { get; set; }
        
        // These properties help us demonstrate structured logging with security context
        public LoginFailureReason? FailureReason { get; set; }
        public int FailedAttemptCount { get; set; }
    }

    /// <summary>
    /// Enumeration for login failure reasons - great for structured logging categories
    /// </summary>
    public enum LoginFailureReason
    {
        InvalidCredentials,
        AccountLocked,
        AccountDisabled,
        TooManyAttempts,
        SystemError
    }

    /// <summary>
    /// Order creation request model for demonstrating structured logging in business operations
    /// </summary>
    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingMethod { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }

    /// <summary>
    /// Order item request for the order creation process
    /// </summary>
    public class OrderItemRequest
    {
        public string ProductSku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
