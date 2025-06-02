using StructuredLogging.Demo.Models;

namespace StructuredLogging.Demo.Services
{
    /// <summary>
    /// Order service implementation demonstrating structured logging in business operations
    /// This class showcases how to log complex business workflows with proper context,
    /// performance tracking, and error handling using structured logging patterns
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IUserService _userService;
        
        // In-memory storage for demonstration purposes
        private readonly List<Order> _orders;
        private static int _nextOrderId = 1;

        public OrderService(ILogger<OrderService> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _orders = new List<Order>();
        }

        /// <summary>
        /// Create a new order with comprehensive structured logging
        /// This method demonstrates logging complex business operations with timing,
        /// validation, and detailed context information
        /// </summary>
        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            // Start timing the operation for performance logging
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Generate unique order number for tracking
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{_nextOrderId:D6}";
            
            // Log the start of order creation with comprehensive context
            _logger.LogInformation("Order creation started: {OrderNumber} for User {UserId}. Items: {ItemCount}, Total estimated: {EstimatedTotal}",
                orderNumber, request.UserId, request.Items.Count, request.Items.Sum(i => i.UnitPrice * i.Quantity));

            try
            {
                // Validate user exists
                var user = await _userService.GetUserByIdAsync(request.UserId);
                if (user == null)
                {
                    // Log validation failure with context
                    _logger.LogWarning("Order creation failed: User {UserId} not found for order {OrderNumber}",
                        request.UserId, orderNumber);
                    
                    throw new ArgumentException($"User with ID {request.UserId} not found");
                }

                // Log user validation success with user context
                _logger.LogDebug("User validation successful for order {OrderNumber}: {Username} (ID: {UserId}, Role: {Role})",
                    orderNumber, user.Username, user.Id, user.Role);

                // Validate order items
                if (!request.Items.Any())
                {
                    _logger.LogWarning("Order creation failed: No items provided for order {OrderNumber} by user {Username} (ID: {UserId})",
                        orderNumber, user.Username, user.Id);
                    
                    throw new ArgumentException("Order must contain at least one item");
                }

                // Log item validation details
                foreach (var item in request.Items)
                {
                    _logger.LogDebug("Order item validation for {OrderNumber}: {ProductSku} - {ProductName}, Qty: {Quantity}, Price: {UnitPrice}",
                        orderNumber, item.ProductSku, item.ProductName, item.Quantity, item.UnitPrice);
                    
                    if (item.Quantity <= 0)
                    {
                        _logger.LogError("Invalid quantity for product {ProductSku} in order {OrderNumber}: {Quantity}",
                            item.ProductSku, orderNumber, item.Quantity);
                        
                        throw new ArgumentException($"Invalid quantity for product {item.ProductSku}");
                    }
                }

                // Create the order
                var order = new Order
                {
                    Id = _nextOrderId++,
                    OrderNumber = orderNumber,
                    UserId = request.UserId,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    ShippingAddress = request.ShippingAddress,
                    ShippingMethod = request.ShippingMethod,
                    PaymentMethod = request.PaymentMethod,
                    Items = request.Items.Select(item => new OrderItem
                    {
                        OrderId = _nextOrderId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList()
                };

                // Calculate total amount
                order.TotalAmount = order.Items.Sum(item => item.TotalPrice);
                
                // Save order
                _orders.Add(order);
                
                stopwatch.Stop();

                // Log successful order creation with comprehensive business context
                _logger.LogInformation("Order created successfully: {OrderNumber} for user {Username} (ID: {UserId}). " +
                    "Status: {OrderStatus}, Items: {ItemCount}, Total: {TotalAmount:C}, " +
                    "Shipping: {ShippingMethod}, Payment: {PaymentMethod}, Processing time: {ProcessingTimeMs}ms",
                    order.OrderNumber, user.Username, order.UserId, order.Status, 
                    order.Items.Count, order.TotalAmount, order.ShippingMethod, 
                    order.PaymentMethod, stopwatch.ElapsedMilliseconds);

                // Log individual items for detailed tracking
                foreach (var item in order.Items)
                {
                    _logger.LogDebug("Order item created for {OrderNumber}: {ProductSku} - {ProductName}, " +
                        "Qty: {Quantity}, Unit Price: {UnitPrice:C}, Total: {TotalPrice:C}",
                        order.OrderNumber, item.ProductSku, item.ProductName, 
                        item.Quantity, item.UnitPrice, item.TotalPrice);
                }

                return order;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                // Log order creation failure with full context and timing
                _logger.LogError(ex, "Order creation failed for {OrderNumber}, User: {UserId}, " +
                    "Processing time: {ProcessingTimeMs}ms. Error: {ErrorMessage}",
                    orderNumber, request.UserId, stopwatch.ElapsedMilliseconds, ex.Message);
                
                throw;
            }
        }

        /// <summary>
        /// Get order by ID with structured logging for data retrieval
        /// </summary>
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            _logger.LogDebug("Retrieving order by ID: {OrderId}", orderId);

            try
            {
                await Task.Delay(25); // Simulate database access
                
                var order = _orders.FirstOrDefault(o => o.Id == orderId);
                
                if (order != null)
                {
                    _logger.LogDebug("Order retrieved: {OrderNumber} (ID: {OrderId}) for user {UserId}, " +
                        "Status: {OrderStatus}, Total: {TotalAmount:C}",
                        order.OrderNumber, order.Id, order.UserId, order.Status, order.TotalAmount);
                }
                else
                {
                    _logger.LogWarning("Order not found: {OrderId}", orderId);
                }

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order: {OrderId}", orderId);
                throw;
            }
        }

        /// <summary>
        /// Get orders by user ID with structured logging for user-specific data retrieval
        /// </summary>
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            _logger.LogInformation("Retrieving orders for user: {UserId}", userId);

            try
            {
                await Task.Delay(50); // Simulate database access
                
                var orders = _orders.Where(o => o.UserId == userId).ToList();
                
                // Log retrieval results with aggregated information
                _logger.LogInformation("Orders retrieved for user {UserId}: {OrderCount} orders, " +
                    "Total value: {TotalValue:C}",
                    userId, orders.Count, orders.Sum(o => o.TotalAmount));

                // Log individual order summaries for detailed tracking
                foreach (var order in orders)
                {
                    _logger.LogDebug("User order: {OrderNumber} (ID: {OrderId}), Status: {OrderStatus}, " +
                        "Total: {TotalAmount:C}, Created: {CreatedAt}",
                        order.OrderNumber, order.Id, order.Status, order.TotalAmount, order.CreatedAt);
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for user: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Update order status with structured logging for state transitions
        /// This method demonstrates how to log business state changes with proper context
        /// </summary>
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            _logger.LogInformation("Updating order status: Order {OrderId} to {NewStatus}", orderId, status);

            try
            {
                await Task.Delay(25); // Simulate database update
                
                var order = _orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                {
                    _logger.LogWarning("Cannot update status: Order {OrderId} not found", orderId);
                    return false;
                }

                var previousStatus = order.Status;
                order.Status = status;

                // Update timestamps based on status
                switch (status)
                {
                    case OrderStatus.Processing:
                        order.ProcessedAt = DateTime.UtcNow;
                        break;
                    case OrderStatus.Shipped:
                        order.ShippedAt = DateTime.UtcNow;
                        break;
                    case OrderStatus.Delivered:
                        order.DeliveredAt = DateTime.UtcNow;
                        break;
                }

                // Log status transition with comprehensive context
                _logger.LogInformation("Order status updated: {OrderNumber} (ID: {OrderId}) " +
                    "from {PreviousStatus} to {NewStatus} for user {UserId}",
                    order.OrderNumber, order.Id, previousStatus, status, order.UserId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status: Order {OrderId} to {Status}", orderId, status);
                return false;
            }
        }

        /// <summary>
        /// Process payment with structured logging for financial operations
        /// This demonstrates logging sensitive operations with appropriate detail
        /// </summary>
        public async Task<bool> ProcessPaymentAsync(int orderId, string paymentTransactionId)
        {
            _logger.LogInformation("Processing payment for order {OrderId}, Transaction: {TransactionId}", 
                orderId, paymentTransactionId);

            try
            {
                await Task.Delay(200); // Simulate payment processing
                
                var order = _orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                {
                    _logger.LogError("Payment processing failed: Order {OrderId} not found", orderId);
                    return false;
                }

                // Simulate payment validation
                if (string.IsNullOrEmpty(paymentTransactionId))
                {
                    _logger.LogError("Payment processing failed for order {OrderNumber} (ID: {OrderId}): " +
                        "Invalid transaction ID", order.OrderNumber, order.Id);
                    return false;
                }

                // Update order with payment information
                order.PaymentTransactionId = paymentTransactionId;
                order.Status = OrderStatus.PaymentConfirmed;

                // Log successful payment with financial context (be careful not to log sensitive data)
                _logger.LogInformation("Payment processed successfully for order {OrderNumber} (ID: {OrderId}), " +
                    "User: {UserId}, Amount: {Amount:C}, Transaction: {TransactionId}, Payment Method: {PaymentMethod}",
                    order.OrderNumber, order.Id, order.UserId, order.TotalAmount, 
                    paymentTransactionId, order.PaymentMethod);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment processing error for order {OrderId}, Transaction: {TransactionId}", 
                    orderId, paymentTransactionId);
                return false;
            }
        }

        /// <summary>
        /// Ship order with structured logging for fulfillment operations
        /// </summary>
        public async Task<bool> ShipOrderAsync(int orderId, string trackingNumber)
        {
            _logger.LogInformation("Shipping order {OrderId} with tracking number {TrackingNumber}", 
                orderId, trackingNumber);

            try
            {
                await Task.Delay(100); // Simulate shipping process
                
                var order = _orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                {
                    _logger.LogError("Shipping failed: Order {OrderId} not found", orderId);
                    return false;
                }

                if (order.Status != OrderStatus.PaymentConfirmed)
                {
                    _logger.LogWarning("Shipping failed for order {OrderNumber} (ID: {OrderId}): " +
                        "Invalid status {CurrentStatus}. Expected: {ExpectedStatus}",
                        order.OrderNumber, order.Id, order.Status, OrderStatus.PaymentConfirmed);
                    return false;
                }

                // Update order with shipping information
                order.TrackingNumber = trackingNumber;
                order.Status = OrderStatus.Shipped;
                order.ShippedAt = DateTime.UtcNow;

                // Log successful shipping with logistics context
                _logger.LogInformation("Order shipped successfully: {OrderNumber} (ID: {OrderId}), " +
                    "User: {UserId}, Tracking: {TrackingNumber}, Shipping Method: {ShippingMethod}, " +
                    "Shipped Date: {ShippedAt}, Items: {ItemCount}",
                    order.OrderNumber, order.Id, order.UserId, trackingNumber, 
                    order.ShippingMethod, order.ShippedAt, order.Items.Count);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Shipping error for order {OrderId}, Tracking: {TrackingNumber}", 
                    orderId, trackingNumber);
                return false;
            }
        }
    }
}
