using Microsoft.AspNetCore.Mvc;
using Serilog.Demo.Models;
using Serilog.Demo.Services;
using Serilog;
using Serilog.Context;

namespace Serilog.Demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Demonstrates comprehensive order processing logging
    /// This endpoint shows how to log business workflows with correlation IDs
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Order>>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        // Generate correlation ID for this business transaction
        var correlationId = Guid.NewGuid().ToString();
        
        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("UserId", request.UserId))
        {
            _logger.LogInformation("Order creation started for user {UserId} with {ItemCount} items, CorrelationId: {CorrelationId}",
                request.UserId, request.Items.Count, correlationId);

            try
            {
                // Validate request - this would typically be done with FluentValidation
                if (request.Items == null || !request.Items.Any())
                {
                    _logger.LogWarning("Order creation failed: No items provided for user {UserId}", request.UserId);
                    return BadRequest(new ApiResponse<Order>
                    {
                        Success = false,
                        Message = "Order must contain at least one item",
                        Errors = new List<string> { "Items list cannot be empty" }
                    });
                }

                if (string.IsNullOrWhiteSpace(request.ShippingAddress))
                {
                    _logger.LogWarning("Order creation failed: No shipping address provided for user {UserId}", request.UserId);
                    return BadRequest(new ApiResponse<Order>
                    {
                        Success = false,
                        Message = "Shipping address is required",
                        Errors = new List<string> { "ShippingAddress cannot be empty" }
                    });
                }

                // Log the business details before processing
                var totalOrderValue = request.Items.Sum(item => item.Quantity * item.UnitPrice);
                _logger.LogInformation("Processing order with total value {TotalValue:C} for user {UserId}",
                    totalOrderValue, request.UserId);

                // Create the order through our service layer
                var order = await _orderService.CreateOrderAsync(request);

                _logger.LogInformation("Order {OrderId} created successfully for user {UserId} with total {TotalAmount:C}",
                    order.OrderId, order.UserId, order.TotalAmount);

                return Ok(new ApiResponse<Order>
                {
                    Success = true,
                    Data = order,
                    Message = "Order created successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                // Business logic exceptions - these are expected and should be logged as warnings
                _logger.LogWarning(ex, "Business rule violation during order creation for user {UserId}: {ErrorMessage}",
                    request.UserId, ex.Message);

                return BadRequest(new ApiResponse<Order>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Unexpected exceptions - these need immediate attention
                _logger.LogError(ex, "Unexpected error during order creation for user {UserId}. CorrelationId: {CorrelationId}",
                    request.UserId, correlationId);

                return StatusCode(500, new ApiResponse<Order>
                {
                    Success = false,
                    Message = "An unexpected error occurred while creating the order"
                });
            }
        }
    }

    /// <summary>
    /// Demonstrates how to log data retrieval operations
    /// Shows logging patterns for read operations vs write operations
    /// </summary>
    [HttpGet("{orderId}")]
    public async Task<ActionResult<ApiResponse<Order>>> GetOrder(int orderId)
    {
        using (LogContext.PushProperty("OrderId", orderId))
        {
            _logger.LogDebug("Fetching order details for OrderId: {OrderId}", orderId);

            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);

                if (order == null)
                {
                    _logger.LogInformation("Order not found: {OrderId}", orderId);
                    return NotFound(new ApiResponse<Order>
                    {
                        Success = false,
                        Message = $"Order {orderId} not found"
                    });
                }

                _logger.LogDebug("Successfully retrieved order {OrderId} for user {UserId}",
                    order.OrderId, order.UserId);

                return Ok(new ApiResponse<Order>
                {
                    Success = true,
                    Data = order,
                    Message = "Order retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order {OrderId}", orderId);

                return StatusCode(500, new ApiResponse<Order>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the order"
                });
            }
        }
    }

    /// <summary>
    /// Demonstrates logging for status change operations
    /// Shows how to log business state transitions
    /// </summary>
    [HttpPut("{orderId}/status")]
    public async Task<ActionResult<ApiResponse<Order>>> UpdateOrderStatus(int orderId, [FromBody] OrderStatus newStatus)
    {
        using (LogContext.PushProperty("OrderId", orderId))
        using (LogContext.PushProperty("NewStatus", newStatus))
        {
            _logger.LogInformation("Attempting to update order {OrderId} status to {NewStatus}", orderId, newStatus);

            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);

                if (updatedOrder == null)
                {
                    _logger.LogWarning("Cannot update status: Order {OrderId} not found", orderId);
                    return NotFound(new ApiResponse<Order>
                    {
                        Success = false,
                        Message = $"Order {orderId} not found"
                    });
                }

                _logger.LogInformation("Order {OrderId} status successfully updated to {NewStatus}", orderId, newStatus);

                return Ok(new ApiResponse<Order>
                {
                    Success = true,
                    Data = updatedOrder,
                    Message = $"Order status updated to {newStatus}"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid status transition for order {OrderId}: {ErrorMessage}", orderId, ex.Message);

                return BadRequest(new ApiResponse<Order>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating status for order {OrderId}", orderId);

                return StatusCode(500, new ApiResponse<Order>
                {
                    Success = false,
                    Message = "An error occurred while updating the order status"
                });
            }
        }
    }

    /// <summary>
    /// Demonstrates logging for list operations with performance metrics
    /// Shows how to log query operations with filtering and pagination
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<List<Order>>>> GetUserOrders(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("Page", page))
        using (LogContext.PushProperty("PageSize", pageSize))
        {
            _logger.LogDebug("Fetching orders for user {UserId}, page {Page}, pageSize {PageSize}",
                userId, page, pageSize);

            try
            {
                var orders = await _orderService.GetUserOrdersAsync(userId, page, pageSize);

                _logger.LogInformation("Retrieved {OrderCount} orders for user {UserId}",
                    orders.Count, userId);

                return Ok(new ApiResponse<List<Order>>
                {
                    Success = true,
                    Data = orders,
                    Message = $"Retrieved {orders.Count} orders"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for user {UserId}", userId);

                return StatusCode(500, new ApiResponse<List<Order>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving orders"
                });
            }
        }
    }
}
