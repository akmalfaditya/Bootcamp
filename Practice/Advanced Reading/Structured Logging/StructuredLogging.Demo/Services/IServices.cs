using StructuredLogging.Demo.Models;

namespace StructuredLogging.Demo.Services
{
    /// <summary>
    /// User service interface defining operations for user management
    /// These operations will be used to demonstrate structured logging patterns
    /// </summary>
    public interface IUserService
    {
        Task<LoginResult> LoginAsync(LoginRequest request);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateLastLoginAsync(int userId);
    }

    /// <summary>
    /// Order service interface for business operations
    /// These methods will showcase structured logging in complex business workflows
    /// </summary>
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> ProcessPaymentAsync(int orderId, string paymentTransactionId);
        Task<bool> ShipOrderAsync(int orderId, string trackingNumber);
    }
}
