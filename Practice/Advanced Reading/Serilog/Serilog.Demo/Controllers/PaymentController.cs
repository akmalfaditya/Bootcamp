using Microsoft.AspNetCore.Mvc;
using Serilog.Demo.Models;
using Serilog.Demo.Services;
using Serilog;
using Serilog.Context;

namespace Serilog.Demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Demonstrates financial transaction logging with security considerations
    /// This endpoint shows how to handle sensitive payment data in logs
    /// </summary>
    [HttpPost("process")]
    public async Task<ActionResult<ApiResponse<Payment>>> ProcessPayment([FromBody] ProcessPaymentRequest request)
    {
        // Generate unique transaction ID for tracking
        var transactionId = Guid.NewGuid().ToString();
        
        using (LogContext.PushProperty("TransactionId", transactionId))
        using (LogContext.PushProperty("OrderId", request.OrderId))
        using (LogContext.PushProperty("PaymentMethod", request.Method))
        {
            // IMPORTANT: Never log the full payment request as it may contain sensitive data
            _logger.LogInformation("Payment processing initiated for order {OrderId} using {PaymentMethod}, Amount: {Amount:C}, TransactionId: {TransactionId}",
                request.OrderId, request.Method, request.Amount, transactionId);

            try
            {
                // Input validation with detailed logging
                if (request.Amount <= 0)
                {
                    _logger.LogWarning("Payment rejected: Invalid amount {Amount} for order {OrderId}", 
                        request.Amount, request.OrderId);
                    
                    return BadRequest(new ApiResponse<Payment>
                    {
                        Success = false,
                        Message = "Payment amount must be greater than zero",
                        Errors = new List<string> { "Invalid amount" }
                    });
                }

                if (request.Amount > 10000) // Business rule: maximum transaction limit
                {
                    _logger.LogWarning("Payment rejected: Amount {Amount:C} exceeds limit for order {OrderId}",
                        request.Amount, request.OrderId);
                    
                    return BadRequest(new ApiResponse<Payment>
                    {
                        Success = false,
                        Message = "Payment amount exceeds maximum limit of $10,000",
                        Errors = new List<string> { "Amount exceeds limit" }
                    });
                }

                // Log the attempt before processing
                _logger.LogInformation("Attempting to process payment of {Amount:C} for order {OrderId} via {PaymentMethod}",
                    request.Amount, request.OrderId, request.Method);

                // Process payment through service layer
                var payment = await _paymentService.ProcessPaymentAsync(request);

                // Log successful completion with audit trail
                _logger.LogInformation("Payment {PaymentId} processed successfully for order {OrderId}. Status: {PaymentStatus}, Amount: {Amount:C}",
                    payment.PaymentId, payment.OrderId, payment.Status, payment.Amount);

                // For financial transactions, also log to audit trail
                using (LogContext.PushProperty("AuditEvent", "PaymentCompleted"))
                {
                    _logger.LogInformation("AUDIT: Payment transaction completed - PaymentId: {PaymentId}, OrderId: {OrderId}, Amount: {Amount:C}, Method: {PaymentMethod}, TransactionId: {TransactionId}",
                        payment.PaymentId, payment.OrderId, payment.Amount, payment.Method, transactionId);
                }

                return Ok(new ApiResponse<Payment>
                {
                    Success = true,
                    Data = payment,
                    Message = "Payment processed successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                // Business logic failures - these might be due to insufficient funds, expired cards, etc.
                _logger.LogWarning(ex, "Payment processing failed for order {OrderId}: {ErrorMessage}. TransactionId: {TransactionId}",
                    request.OrderId, ex.Message, transactionId);

                return BadRequest(new ApiResponse<Payment>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Critical payment processing errors - need immediate attention
                _logger.LogError(ex, "CRITICAL: Payment processing error for order {OrderId}, Amount: {Amount:C}. TransactionId: {TransactionId}",
                    request.OrderId, request.Amount, transactionId);

                // For financial operations, also create an incident log
                using (LogContext.PushProperty("Severity", "Critical"))
                using (LogContext.PushProperty("IncidentType", "PaymentProcessingFailure"))
                {
                    _logger.LogError("INCIDENT: Critical payment processing failure - TransactionId: {TransactionId}, OrderId: {OrderId}",
                        transactionId, request.OrderId);
                }

                return StatusCode(500, new ApiResponse<Payment>
                {
                    Success = false,
                    Message = "Payment processing is temporarily unavailable. Please try again later."
                });
            }
        }
    }

    /// <summary>
    /// Demonstrates refund processing with compliance logging
    /// Shows how to handle financial reversals with proper audit trails
    /// </summary>
    [HttpPost("refund/{paymentId}")]
    public async Task<ActionResult<ApiResponse<Payment>>> ProcessRefund(int paymentId, [FromBody] string reason)
    {
        var refundId = Guid.NewGuid().ToString();
        
        using (LogContext.PushProperty("PaymentId", paymentId))
        using (LogContext.PushProperty("RefundId", refundId))
        using (LogContext.PushProperty("RefundReason", reason))
        {
            _logger.LogInformation("Refund process initiated for payment {PaymentId}, Reason: {RefundReason}, RefundId: {RefundId}",
                paymentId, reason, refundId);

            try
            {
                if (string.IsNullOrWhiteSpace(reason))
                {
                    _logger.LogWarning("Refund rejected: No reason provided for payment {PaymentId}", paymentId);
                    
                    return BadRequest(new ApiResponse<Payment>
                    {
                        Success = false,
                        Message = "Refund reason is required"
                    });
                }

                var refundedPayment = await _paymentService.ProcessRefundAsync(paymentId, reason);

                if (refundedPayment == null)
                {
                    _logger.LogWarning("Refund failed: Payment {PaymentId} not found or not eligible for refund", paymentId);
                    
                    return NotFound(new ApiResponse<Payment>
                    {
                        Success = false,
                        Message = "Payment not found or not eligible for refund"
                    });
                }

                // Critical audit logging for financial reversals
                using (LogContext.PushProperty("AuditEvent", "RefundProcessed"))
                {
                    _logger.LogWarning("AUDIT: Refund processed - PaymentId: {PaymentId}, Amount: {Amount:C}, Reason: {RefundReason}, RefundId: {RefundId}",
                        refundedPayment.PaymentId, refundedPayment.Amount, reason, refundId);
                }

                _logger.LogInformation("Refund completed successfully for payment {PaymentId}, Amount: {Amount:C}",
                    refundedPayment.PaymentId, refundedPayment.Amount);

                return Ok(new ApiResponse<Payment>
                {
                    Success = true,
                    Data = refundedPayment,
                    Message = "Refund processed successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Refund processing failed for payment {PaymentId}: {ErrorMessage}",
                    paymentId, ex.Message);

                return BadRequest(new ApiResponse<Payment>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CRITICAL: Refund processing error for payment {PaymentId}. RefundId: {RefundId}",
                    paymentId, refundId);

                // Critical incident for refund failures
                using (LogContext.PushProperty("Severity", "Critical"))
                using (LogContext.PushProperty("IncidentType", "RefundProcessingFailure"))
                {
                    _logger.LogError("INCIDENT: Critical refund processing failure - RefundId: {RefundId}, PaymentId: {PaymentId}",
                        refundId, paymentId);
                }

                return StatusCode(500, new ApiResponse<Payment>
                {
                    Success = false,
                    Message = "Refund processing is temporarily unavailable"
                });
            }
        }
    }

    /// <summary>
    /// Demonstrates payment status inquiry logging
    /// Shows how to log read operations for financial data
    /// </summary>
    [HttpGet("{paymentId}")]
    public async Task<ActionResult<ApiResponse<Payment>>> GetPayment(int paymentId)
    {
        using (LogContext.PushProperty("PaymentId", paymentId))
        {
            _logger.LogDebug("Payment inquiry for PaymentId: {PaymentId}", paymentId);

            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(paymentId);

                if (payment == null)
                {
                    _logger.LogInformation("Payment inquiry: Payment {PaymentId} not found", paymentId);
                    
                    return NotFound(new ApiResponse<Payment>
                    {
                        Success = false,
                        Message = $"Payment {paymentId} not found"
                    });
                }

                // Log successful retrieval (but don't log sensitive financial details at INFO level)
                _logger.LogDebug("Payment {PaymentId} retrieved successfully for order {OrderId}",
                    payment.PaymentId, payment.OrderId);

                return Ok(new ApiResponse<Payment>
                {
                    Success = true,
                    Data = payment,
                    Message = "Payment retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment {PaymentId}", paymentId);

                return StatusCode(500, new ApiResponse<Payment>
                {
                    Success = false,
                    Message = "An error occurred while retrieving payment information"
                });
            }
        }
    }

    /// <summary>
    /// Demonstrates order payment history logging
    /// Shows how to log aggregated financial data queries
    /// </summary>
    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<ApiResponse<List<Payment>>>> GetOrderPayments(int orderId)
    {
        using (LogContext.PushProperty("OrderId", orderId))
        {
            _logger.LogDebug("Retrieving payment history for order {OrderId}", orderId);

            try
            {
                var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);

                _logger.LogInformation("Retrieved {PaymentCount} payments for order {OrderId}",
                    payments.Count, orderId);

                // For financial reporting, log aggregate data
                if (payments.Any())
                {
                    var totalPaid = payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount);
                    var totalRefunded = payments.Where(p => p.Status == PaymentStatus.Refunded).Sum(p => p.Amount);
                    
                    using (LogContext.PushProperty("TotalPaid", totalPaid))
                    using (LogContext.PushProperty("TotalRefunded", totalRefunded))
                    {
                        _logger.LogInformation("Payment summary for order {OrderId}: TotalPaid: {TotalPaid:C}, TotalRefunded: {TotalRefunded:C}",
                            orderId, totalPaid, totalRefunded);
                    }
                }

                return Ok(new ApiResponse<List<Payment>>
                {
                    Success = true,
                    Data = payments,
                    Message = $"Retrieved {payments.Count} payments"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payments for order {OrderId}", orderId);

                return StatusCode(500, new ApiResponse<List<Payment>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving payment history"
                });
            }
        }
    }
}
