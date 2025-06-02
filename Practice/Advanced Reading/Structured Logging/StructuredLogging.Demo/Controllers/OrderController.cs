using Microsoft.AspNetCore.Mvc;
using StructuredLogging.Demo.Models;
using StructuredLogging.Demo.Services;

namespace StructuredLogging.Demo.Controllers
{
    /// <summary>
    /// Order controller demonstrating structured logging in business operations
    /// This controller showcases how to log complex business workflows,
    /// performance metrics, and transaction-related events
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// Create order endpoint demonstrating structured logging for business transactions
        /// This method shows how to log complex business operations with timing and context
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            // Start measuring request processing time for performance monitoring
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            
            // Log the incoming business operation with comprehensive context
            _logger.LogInformation("Order creation request received. UserId: {UserId}, ItemCount: {ItemCount}, " +
                "EstimatedTotal: {EstimatedTotal:C}, ShippingMethod: {ShippingMethod}, " +
                "PaymentMethod: {PaymentMethod}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                request.UserId, request.Items.Count, 
                request.Items.Sum(i => i.UnitPrice * i.Quantity),
                request.ShippingMethod, request.PaymentMethod, clientIp, correlationId);

            try
            {                // Validate request data
                if (!ModelState.IsValid)
                {
                    // Log validation failures with detailed field information
                    var validationErrors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .Select(x => new { Field = x.Key, Errors = x.Value?.Errors.Select(e => e.ErrorMessage) });
                    
                    _logger.LogWarning("Order creation failed: Invalid request data. UserId: {UserId}, " +
                        "ValidationErrors: {@ValidationErrors}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        request.UserId, validationErrors, clientIp, correlationId);
                    
                    return BadRequest(ModelState);
                }

                // Call the service layer to create the order
                var order = await _orderService.CreateOrderAsync(request);
                
                stopwatch.Stop();

                // Log successful order creation with business metrics
                _logger.LogInformation("Order created successfully. OrderNumber: {OrderNumber} (ID: {OrderId}), " +
                    "UserId: {UserId}, TotalAmount: {TotalAmount:C}, ItemCount: {ItemCount}, " +
                    "Status: {OrderStatus}, ProcessingTime: {ProcessingTimeMs}ms, " +
                    "IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    order.OrderNumber, order.Id, order.UserId, order.TotalAmount, 
                    order.Items.Count, order.Status, stopwatch.ElapsedMilliseconds,
                    clientIp, correlationId);

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                stopwatch.Stop();
                
                // Log business validation failures
                _logger.LogWarning("Order creation failed: Business validation error. UserId: {UserId}, " +
                    "Error: {ErrorMessage}, ProcessingTime: {ProcessingTimeMs}ms, " +
                    "IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    request.UserId, ex.Message, stopwatch.ElapsedMilliseconds, clientIp, correlationId);
                
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                // Log system errors with full context
                _logger.LogError(ex, "Order creation failed: System error. UserId: {UserId}, " +
                    "ProcessingTime: {ProcessingTimeMs}ms, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    request.UserId, stopwatch.ElapsedMilliseconds, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get order by ID with structured logging for data retrieval
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var correlationId = Guid.NewGuid().ToString();
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            _logger.LogInformation("Get order request received. OrderId: {OrderId}, " +
                "IP: {ClientIp}, CorrelationId: {CorrelationId}", id, clientIp, correlationId);

            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                
                if (order != null)
                {
                    // Log successful data retrieval with order context
                    _logger.LogInformation("Order retrieved successfully. OrderNumber: {OrderNumber} (ID: {OrderId}), " +
                        "UserId: {UserId}, Status: {OrderStatus}, TotalAmount: {TotalAmount:C}, " +
                        "IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        order.OrderNumber, order.Id, order.UserId, order.Status, 
                        order.TotalAmount, clientIp, correlationId);
                    
                    return Ok(order);
                }
                else
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}, " +
                        "IP: {ClientIp}, CorrelationId: {CorrelationId}", id, clientIp, correlationId);
                    
                    return NotFound(new { message = "Order not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order. OrderId: {OrderId}, " +
                    "IP: {ClientIp}, CorrelationId: {CorrelationId}", id, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get orders by user ID with structured logging for user-specific queries
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByUser(int userId)
        {
            var correlationId = Guid.NewGuid().ToString();
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            _logger.LogInformation("Get user orders request received. UserId: {UserId}, " +
                "IP: {ClientIp}, CorrelationId: {CorrelationId}", userId, clientIp, correlationId);

            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                
                // Log retrieval results with aggregated metrics
                _logger.LogInformation("User orders retrieved successfully. UserId: {UserId}, " +
                    "OrderCount: {OrderCount}, TotalValue: {TotalValue:C}, " +
                    "IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    userId, orders.Count, orders.Sum(o => o.TotalAmount), clientIp, correlationId);
                
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user orders. UserId: {UserId}, " +
                    "IP: {ClientIp}, CorrelationId: {CorrelationId}", userId, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Update order status endpoint demonstrating structured logging for state changes
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            _logger.LogInformation("Update order status request received. OrderId: {OrderId}, " +
                "NewStatus: {NewStatus}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                id, request.Status, clientIp, correlationId);

            try
            {
                var success = await _orderService.UpdateOrderStatusAsync(id, request.Status);
                
                if (success)
                {
                    _logger.LogInformation("Order status updated successfully. OrderId: {OrderId}, " +
                        "NewStatus: {NewStatus}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        id, request.Status, clientIp, correlationId);
                    
                    return Ok(new { message = "Order status updated successfully" });
                }
                else
                {
                    _logger.LogWarning("Order status update failed. OrderId: {OrderId}, " +
                        "RequestedStatus: {RequestedStatus}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        id, request.Status, clientIp, correlationId);
                    
                    return NotFound(new { message = "Order not found or update failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status. OrderId: {OrderId}, " +
                    "RequestedStatus: {RequestedStatus}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    id, request.Status, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Process payment endpoint demonstrating structured logging for financial operations
        /// </summary>
        [HttpPost("{id}/payment")]
        public async Task<ActionResult> ProcessPayment(int id, [FromBody] ProcessPaymentRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            // Log payment processing request (be careful not to log sensitive payment data)
            _logger.LogInformation("Payment processing request received. OrderId: {OrderId}, " +
                "TransactionId: {TransactionId}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                id, request.TransactionId, clientIp, correlationId);

            try
            {
                var success = await _orderService.ProcessPaymentAsync(id, request.TransactionId);
                
                if (success)
                {
                    _logger.LogInformation("Payment processed successfully. OrderId: {OrderId}, " +
                        "TransactionId: {TransactionId}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        id, request.TransactionId, clientIp, correlationId);
                    
                    return Ok(new { message = "Payment processed successfully" });
                }
                else
                {
                    _logger.LogError("Payment processing failed. OrderId: {OrderId}, " +
                        "TransactionId: {TransactionId}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        id, request.TransactionId, clientIp, correlationId);
                    
                    return BadRequest(new { message = "Payment processing failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment processing error. OrderId: {OrderId}, " +
                    "TransactionId: {TransactionId}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    id, request.TransactionId, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Ship order endpoint demonstrating structured logging for fulfillment operations
        /// </summary>
        [HttpPost("{id}/ship")]
        public async Task<ActionResult> ShipOrder(int id, [FromBody] ShipOrderRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            _logger.LogInformation("Ship order request received. OrderId: {OrderId}, " +
                "TrackingNumber: {TrackingNumber}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                id, request.TrackingNumber, clientIp, correlationId);

            try
            {
                var success = await _orderService.ShipOrderAsync(id, request.TrackingNumber);
                
                if (success)
                {
                    _logger.LogInformation("Order shipped successfully. OrderId: {OrderId}, " +
                        "TrackingNumber: {TrackingNumber}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        id, request.TrackingNumber, clientIp, correlationId);
                    
                    return Ok(new { message = "Order shipped successfully" });
                }
                else
                {
                    _logger.LogWarning("Order shipping failed. OrderId: {OrderId}, " +
                        "TrackingNumber: {TrackingNumber}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                        id, request.TrackingNumber, clientIp, correlationId);
                    
                    return BadRequest(new { message = "Order shipping failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order shipping error. OrderId: {OrderId}, " +
                    "TrackingNumber: {TrackingNumber}, IP: {ClientIp}, CorrelationId: {CorrelationId}",
                    id, request.TrackingNumber, clientIp, correlationId);
                
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }

    /// <summary>
    /// Request models for order operations
    /// </summary>
    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }

    public class ProcessPaymentRequest
    {
        public string TransactionId { get; set; } = string.Empty;
    }

    public class ShipOrderRequest
    {
        public string TrackingNumber { get; set; } = string.Empty;
    }
}
