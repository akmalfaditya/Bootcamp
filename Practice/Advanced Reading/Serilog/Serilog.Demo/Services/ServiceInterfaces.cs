using Serilog.Demo.Models;

namespace Serilog.Demo.Services;

/// <summary>
/// Service interfaces for our demo application
/// These define the contracts for our business logic services
/// </summary>
/// 
public interface IUserService
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(User user);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task<bool> DeactivateUserAsync(int userId);
}

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderRequest request);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    Task<List<Order>> GetUserOrdersAsync(int userId, int page, int pageSize);
    Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    Task<bool> CancelOrderAsync(int orderId, string reason);
    Task<decimal> CalculateOrderTotalAsync(List<OrderItemRequest> items);
}

public interface IPaymentService
{
    Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest request);
    Task<Payment?> GetPaymentByIdAsync(int paymentId);
    Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId);
    Task<Payment> RefundPaymentAsync(int paymentId, decimal amount, string reason);
    Task<Payment?> ProcessRefundAsync(int paymentId, string reason);
    Task<bool> ValidatePaymentAmountAsync(decimal amount);
}
