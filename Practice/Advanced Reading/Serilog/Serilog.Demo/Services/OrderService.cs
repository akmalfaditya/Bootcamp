using Serilog.Demo.Models;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Serilog.Demo.Services;

/// <summary>
/// Order service implementation showcasing advanced Serilog features:
/// - Performance monitoring with timing measurements
/// - Complex object logging with destructuring
/// - Business workflow logging
/// - Error correlation with transaction IDs
/// - Contextual enrichment for order processing
/// </summary>
public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private static readonly List<Order> _orders = new();
    private static int _nextOrderId = 1;

    public OrderService(ILogger<OrderService> logger)
    {
        _logger = logger;
        _logger.LogInformation("OrderService initialized. Ready to process orders");
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        // Generate a correlation ID for this order creation process
        // This helps track all log entries related to this specific operation
        var correlationId = Guid.NewGuid().ToString("N")[..8];
        
        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("UserId", request.UserId))
        {
            _logger.LogInformation("Starting order creation process for user {UserId} with {ItemCount} items", 
                request.UserId, request.Items.Count);

            // Start performance monitoring
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Validate the request
                await ValidateOrderRequest(request);

                // Calculate totals
                var totalAmount = await CalculateOrderTotalAsync(request.Items);
                
                // Create the order entity
                var order = new Order
                {
                    OrderId = _nextOrderId++,
                    UserId = request.UserId,
                    TotalAmount = totalAmount,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    ShippingAddress = request.ShippingAddress,
                    Items = request.Items.Select(item => new OrderItem
                    {
                        OrderItemId = Random.Shared.Next(1000, 9999),
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList()
                };

                // Simulate database save
                await Task.Delay(Random.Shared.Next(100, 300));
                _orders.Add(order);

                stopwatch.Stop();

                // Log successful creation with comprehensive details
                // Notice the use of {@Order} for destructuring the entire object
                _logger.LogInformation("Order created successfully in {ElapsedMs}ms: {@OrderSummary}", 
                    stopwatch.ElapsedMilliseconds,
                    new {
                        order.OrderId,
                        order.UserId,
                        order.TotalAmount,
                        order.ItemCount,
                        order.Status,
                        ProcessingTime = stopwatch.ElapsedMilliseconds
                    });

                // Log business metrics
                _logger.LogInformation("Order creation metrics: Total value {TotalAmount:C}, " +
                    "Items {ItemCount}, Processing time {ProcessingTimeMs}ms", 
                    totalAmount, order.ItemCount, stopwatch.ElapsedMilliseconds);

                return order;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                // Log the failure with full context
                _logger.LogError(ex, "Order creation failed after {ElapsedMs}ms for user {UserId}. " +
                    "Error: {ErrorMessage}. Request: {@OrderRequest}", 
                    stopwatch.ElapsedMilliseconds, request.UserId, ex.Message, 
                    new { request.UserId, ItemCount = request.Items.Count, request.ShippingAddress });
                
                throw;
            }
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        using (LogContext.PushProperty("OrderId", orderId))
        {
            _logger.LogDebug("Retrieving order {OrderId}", orderId);

            await Task.Delay(Random.Shared.Next(10, 50));

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order != null)
            {
                _logger.LogInformation("Retrieved order {OrderId} for user {UserId} with status {Status}", 
                    orderId, order.UserId, order.Status);
            }
            else
            {
                _logger.LogWarning("Order {OrderId} not found", orderId);
            }

            return order;
        }
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
        using (LogContext.PushProperty("UserId", userId))
        {
            _logger.LogInformation("Retrieving all orders for user {UserId}", userId);

            await Task.Delay(Random.Shared.Next(20, 100));

            var userOrders = _orders.Where(o => o.UserId == userId).ToList();

            // Log summary statistics
            if (userOrders.Any())
            {
                var totalValue = userOrders.Sum(o => o.TotalAmount);
                var statusBreakdown = userOrders.GroupBy(o => o.Status)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count());

                _logger.LogInformation("Found {OrderCount} orders for user {UserId}. " +
                    "Total value: {TotalValue:C}. Status breakdown: {@StatusBreakdown}", 
                    userOrders.Count, userId, totalValue, statusBreakdown);
            }
            else
            {
                _logger.LogInformation("No orders found for user {UserId}", userId);
            }

            return userOrders;
        }
    }

    public async Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        using (LogContext.PushProperty("OrderId", orderId))
        {
            _logger.LogInformation("Updating order {OrderId} status to {NewStatus}", orderId, status);

            var order = await GetOrderByIdAsync(orderId);
            
            if (order == null)
            {
                var message = $"Cannot update status: Order {orderId} not found";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var previousStatus = order.Status;
            
            // Business rule validation
            if (!IsValidStatusTransition(previousStatus, status))
            {
                var message = $"Invalid status transition from {previousStatus} to {status}";
                _logger.LogError("Status update failed for order {OrderId}: {ErrorMessage}", 
                    orderId, message);
                throw new InvalidOperationException(message);
            }

            // Simulate database update
            await Task.Delay(Random.Shared.Next(50, 150));

            order.Status = status;
            if (status == OrderStatus.Delivered)
            {
                order.CompletedAt = DateTime.UtcNow;
            }

            // Log the business event with before/after states
            _logger.LogInformation("Order {OrderId} status updated from {PreviousStatus} to {NewStatus} " +
                "for user {UserId}", orderId, previousStatus, status, order.UserId);

            // Log completion if order is delivered
            if (status == OrderStatus.Delivered && order.CompletedAt.HasValue)
            {
                var fulfillmentTime = order.CompletedAt.Value - order.CreatedAt;
                _logger.LogInformation("Order {OrderId} fulfilled in {FulfillmentDays} days " +
                    "({FulfillmentHours:F1} hours)", 
                    orderId, fulfillmentTime.Days, fulfillmentTime.TotalHours);
            }

            return order;
        }
    }

    public async Task<bool> CancelOrderAsync(int orderId, string reason)
    {
        using (LogContext.PushProperty("OrderId", orderId))
        using (LogContext.PushProperty("CancellationReason", reason))
        {
            _logger.LogInformation("Attempting to cancel order {OrderId}. Reason: {Reason}", 
                orderId, reason);

            try
            {
                var order = await GetOrderByIdAsync(orderId);
                
                if (order == null)
                {
                    _logger.LogWarning("Cannot cancel order {OrderId}: Order not found", orderId);
                    return false;
                }

                // Business rules for cancellation
                if (order.Status == OrderStatus.Delivered)
                {
                    _logger.LogWarning("Cannot cancel order {OrderId}: Order already delivered", orderId);
                    return false;
                }

                if (order.Status == OrderStatus.Cancelled)
                {
                    _logger.LogInformation("Order {OrderId} is already cancelled", orderId);
                    return true;
                }

                // Perform cancellation
                await UpdateOrderStatusAsync(orderId, OrderStatus.Cancelled);

                // Log business impact
                _logger.LogInformation("Order {OrderId} cancelled successfully. " +
                    "Revenue impact: {LostRevenue:C}", orderId, order.TotalAmount);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cancelling order {OrderId}: {ErrorMessage}", 
                    orderId, ex.Message);
                return false;
            }
        }
    }

    public async Task<decimal> CalculateOrderTotalAsync(List<OrderItemRequest> items)
    {
        _logger.LogDebug("Calculating total for {ItemCount} items", items.Count);

        // Simulate complex calculation
        await Task.Delay(Random.Shared.Next(10, 50));

        var total = items.Sum(item => item.Quantity * item.UnitPrice);

        // Log calculation details for audit purposes
        _logger.LogDebug("Order total calculated: {Total:C} from {ItemCount} items. " +
            "Breakdown: {@ItemBreakdown}", 
            total, items.Count, 
            items.Select(i => new { i.ProductName, i.Quantity, i.UnitPrice, LineTotal = i.Quantity * i.UnitPrice }));

        return total;
    }

    private async Task ValidateOrderRequest(CreateOrderRequest request)
    {
        _logger.LogDebug("Validating order request for user {UserId}", request.UserId);

        if (request.UserId <= 0)
        {
            var message = "Invalid user ID";
            _logger.LogError("Order validation failed: {ValidationError}", message);
            throw new ArgumentException(message);
        }

        if (!request.Items.Any())
        {
            var message = "Order must contain at least one item";
            _logger.LogError("Order validation failed: {ValidationError}", message);
            throw new ArgumentException(message);
        }

        if (string.IsNullOrWhiteSpace(request.ShippingAddress))
        {
            var message = "Shipping address is required";
            _logger.LogError("Order validation failed: {ValidationError}", message);
            throw new ArgumentException(message);
        }

        // Simulate validation processing time
        await Task.Delay(Random.Shared.Next(10, 30));

        _logger.LogDebug("Order request validation completed successfully");
    }

    public async Task<List<Order>> GetUserOrdersAsync(int userId, int page, int pageSize)
    {
        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("Page", page))
        using (LogContext.PushProperty("PageSize", pageSize))
        {
            _logger.LogDebug("Retrieving orders for user {UserId}, page {Page}, pageSize {PageSize}",
                userId, page, pageSize);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Simulate database query with pagination
                await Task.Delay(Random.Shared.Next(50, 150));

                // Mock implementation - in real app this would query the database
                var mockOrders = new List<Order>
                {
                    new Order
                    {
                        OrderId = 1,
                        UserId = userId,
                        TotalAmount = 99.99m,
                        Status = OrderStatus.Delivered,
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        CompletedAt = DateTime.UtcNow.AddDays(-8),
                        Items = new List<OrderItem>
                        {
                            new OrderItem { OrderItemId = 1, ProductName = "Sample Product", Quantity = 1, UnitPrice = 99.99m }
                        },
                        ShippingAddress = "123 Test Street"
                    }
                };

                // Apply pagination
                var skip = (page - 1) * pageSize;
                var paginatedOrders = mockOrders.Skip(skip).Take(pageSize).ToList();

                stopwatch.Stop();

                // Log successful retrieval with performance metrics
                using (LogContext.PushProperty("OperationName", "GetUserOrders"))
                using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                {
                    _logger.LogInformation("Retrieved {OrderCount} orders for user {UserId} in {Duration}ms",
                        paginatedOrders.Count, userId, stopwatch.ElapsedMilliseconds);
                }

                return paginatedOrders;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                _logger.LogError(ex, "Failed to retrieve orders for user {UserId} after {ElapsedMs}ms",
                    userId, stopwatch.ElapsedMilliseconds);
                
                throw;
            }
        }
    }

    private static bool IsValidStatusTransition(OrderStatus from, OrderStatus to)
    {
        // Define valid state transitions
        return (from, to) switch
        {
            (OrderStatus.Pending, OrderStatus.Processing) => true,
            (OrderStatus.Pending, OrderStatus.Cancelled) => true,
            (OrderStatus.Processing, OrderStatus.Shipped) => true,
            (OrderStatus.Processing, OrderStatus.Cancelled) => true,
            (OrderStatus.Shipped, OrderStatus.Delivered) => true,
            (OrderStatus.Delivered, OrderStatus.Refunded) => true,
            _ => false
        };
    }
}
