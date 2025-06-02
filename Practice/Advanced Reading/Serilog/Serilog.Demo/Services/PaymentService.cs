using Serilog.Demo.Models;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Serilog.Demo.Services;

/// <summary>
/// Payment service demonstrating critical logging scenarios:
/// - Financial transaction logging with audit trails
/// - Security logging for payment processing
/// - Error handling with detailed context
/// - Performance monitoring for payment operations
/// - Sensitive data handling (what NOT to log)
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;
    private static readonly List<Payment> _payments = new();
    private static int _nextPaymentId = 1;

    public PaymentService(ILogger<PaymentService> logger)
    {
        _logger = logger;
        _logger.LogInformation("PaymentService initialized for transaction processing");
    }

    public async Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        // Generate transaction ID for correlation across all payment-related logs
        var transactionId = Guid.NewGuid().ToString("N")[..12].ToUpper();
        
        using (LogContext.PushProperty("TransactionId", transactionId))
        using (LogContext.PushProperty("OrderId", request.OrderId))
        using (LogContext.PushProperty("PaymentMethod", request.Method))
        {
            _logger.LogInformation("Starting payment processing for order {OrderId}. " +
                "Amount: {Amount:C}, Method: {PaymentMethod}, Transaction: {TransactionId}", 
                request.OrderId, request.Amount, request.Method, transactionId);

            var stopwatch = Stopwatch.StartNew();
            Payment payment = null!;

            try
            {
                // Validate payment request
                await ValidatePaymentRequest(request);

                // Create payment record
                payment = new Payment
                {
                    PaymentId = _nextPaymentId++,
                    OrderId = request.OrderId,
                    Amount = request.Amount,
                    Method = request.Method,
                    Status = PaymentStatus.Processing,
                    CreatedAt = DateTime.UtcNow,
                    TransactionId = transactionId
                };

                _payments.Add(payment);

                // Log payment initiation (critical for audit trail)
                _logger.LogInformation("Payment {PaymentId} created and processing initiated. " +
                    "Transaction {TransactionId} for order {OrderId}", 
                    payment.PaymentId, transactionId, request.OrderId);

                // Simulate payment processing with external provider
                var processingResult = await ProcessWithPaymentProvider(payment);

                stopwatch.Stop();

                if (processingResult.Success)
                {
                    payment.Status = PaymentStatus.Completed;
                    payment.ProcessedAt = DateTime.UtcNow;

                    // SUCCESS: Log with comprehensive audit information
                    _logger.LogInformation("Payment processed successfully. " +
                        "Payment {PaymentId}, Transaction {TransactionId}, Order {OrderId}, " +
                        "Amount {Amount:C}, Method {PaymentMethod}, Duration {ProcessingTimeMs}ms", 
                        payment.PaymentId, transactionId, request.OrderId, 
                        request.Amount, request.Method, stopwatch.ElapsedMilliseconds);

                    // Log business metrics for monitoring
                    _logger.LogInformation("Payment metrics: {PaymentMethod} transaction completed in {ProcessingTimeMs}ms", 
                        request.Method, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    payment.Status = PaymentStatus.Failed;
                    payment.ErrorMessage = processingResult.ErrorMessage;

                    // FAILURE: Log with error details but maintain security
                    _logger.LogError("Payment processing failed. " +
                        "Payment {PaymentId}, Transaction {TransactionId}, Order {OrderId}, " +
                        "Amount {Amount:C}, Method {PaymentMethod}, Error: {ErrorMessage}, " +
                        "Duration {ProcessingTimeMs}ms", 
                        payment.PaymentId, transactionId, request.OrderId, 
                        request.Amount, request.Method, processingResult.ErrorMessage, 
                        stopwatch.ElapsedMilliseconds);

                    throw new InvalidOperationException($"Payment failed: {processingResult.ErrorMessage}");
                }

                return payment;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // Update payment status if payment object was created
                if (payment != null)
                {
                    payment.Status = PaymentStatus.Failed;
                    payment.ErrorMessage = ex.Message;
                }

                // Critical error logging for payment failures
                _logger.LogError(ex, "Payment processing exception occurred. " +
                    "Transaction {TransactionId}, Order {OrderId}, Amount {Amount:C}, " +
                    "Method {PaymentMethod}, Duration {ProcessingTimeMs}ms, Error: {ErrorMessage}", 
                    transactionId, request.OrderId, request.Amount, request.Method, 
                    stopwatch.ElapsedMilliseconds, ex.Message);

                throw;
            }
        }
    }

    public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
    {
        using (LogContext.PushProperty("PaymentId", paymentId))
        {
            _logger.LogDebug("Retrieving payment {PaymentId}", paymentId);

            await Task.Delay(Random.Shared.Next(10, 30));

            var payment = _payments.FirstOrDefault(p => p.PaymentId == paymentId);

            if (payment != null)
            {
                _logger.LogInformation("Retrieved payment {PaymentId} for order {OrderId} " +
                    "with status {PaymentStatus}", paymentId, payment.OrderId, payment.Status);
            }
            else
            {
                _logger.LogWarning("Payment {PaymentId} not found", paymentId);
            }

            return payment;
        }
    }

    public async Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId)
    {
        using (LogContext.PushProperty("OrderId", orderId))
        {
            _logger.LogInformation("Retrieving all payments for order {OrderId}", orderId);

            await Task.Delay(Random.Shared.Next(15, 50));

            var orderPayments = _payments.Where(p => p.OrderId == orderId).ToList();

            if (orderPayments.Any())
            {
                var totalAmount = orderPayments.Where(p => p.Status == PaymentStatus.Completed)
                                               .Sum(p => p.Amount);
                
                var statusSummary = orderPayments.GroupBy(p => p.Status)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count());

                _logger.LogInformation("Found {PaymentCount} payments for order {OrderId}. " +
                    "Total successful amount: {TotalAmount:C}. Status summary: {@StatusSummary}", 
                    orderPayments.Count, orderId, totalAmount, statusSummary);
            }
            else
            {
                _logger.LogInformation("No payments found for order {OrderId}", orderId);
            }

            return orderPayments;
        }
    }

    public async Task<Payment> RefundPaymentAsync(int paymentId, decimal amount, string reason)
    {
        using (LogContext.PushProperty("PaymentId", paymentId))
        using (LogContext.PushProperty("RefundAmount", amount))
        using (LogContext.PushProperty("RefundReason", reason))
        {
            _logger.LogInformation("Processing refund for payment {PaymentId}. " +
                "Amount: {RefundAmount:C}, Reason: {RefundReason}", 
                paymentId, amount, reason);

            var originalPayment = await GetPaymentByIdAsync(paymentId);
            
            if (originalPayment == null)
            {
                var message = $"Cannot process refund: Payment {paymentId} not found";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            if (originalPayment.Status != PaymentStatus.Completed)
            {
                var message = $"Cannot refund payment {paymentId}: Payment not in completed status";
                _logger.LogError("Refund failed for payment {PaymentId}: {ErrorMessage}", 
                    paymentId, message);
                throw new InvalidOperationException(message);
            }

            if (amount > originalPayment.Amount)
            {
                var message = $"Refund amount {amount:C} exceeds original payment amount {originalPayment.Amount:C}";
                _logger.LogError("Refund validation failed for payment {PaymentId}: {ErrorMessage}", 
                    paymentId, message);
                throw new ArgumentException(message);
            }

            // Create refund transaction
            var refundTransactionId = Guid.NewGuid().ToString("N")[..12].ToUpper();
            
            var refundPayment = new Payment
            {
                PaymentId = _nextPaymentId++,
                OrderId = originalPayment.OrderId,
                Amount = -amount, // Negative amount for refund
                Method = originalPayment.Method,
                Status = PaymentStatus.Processing,
                CreatedAt = DateTime.UtcNow,
                TransactionId = refundTransactionId
            };

            _payments.Add(refundPayment);

            try
            {
                // Simulate refund processing
                await Task.Delay(Random.Shared.Next(200, 500));

                // Simulate refund success (90% success rate)
                if (Random.Shared.NextDouble() > 0.1)
                {
                    refundPayment.Status = PaymentStatus.Completed;
                    refundPayment.ProcessedAt = DateTime.UtcNow;
                    originalPayment.Status = PaymentStatus.Refunded;

                    _logger.LogInformation("Refund processed successfully. " +
                        "Original payment {OriginalPaymentId}, Refund payment {RefundPaymentId}, " +
                        "Refund transaction {RefundTransactionId}, Amount {RefundAmount:C}, " +
                        "Reason: {RefundReason}", 
                        paymentId, refundPayment.PaymentId, refundTransactionId, amount, reason);
                }
                else
                {
                    refundPayment.Status = PaymentStatus.Failed;
                    refundPayment.ErrorMessage = "Refund processing failed at payment provider";

                    _logger.LogError("Refund processing failed. " +
                        "Payment {PaymentId}, Transaction {RefundTransactionId}, " +
                        "Amount {RefundAmount:C}, Error: {ErrorMessage}", 
                        paymentId, refundTransactionId, amount, refundPayment.ErrorMessage);

                    throw new InvalidOperationException(refundPayment.ErrorMessage);
                }

                return refundPayment;
            }
            catch (Exception ex)
            {
                refundPayment.Status = PaymentStatus.Failed;
                refundPayment.ErrorMessage = ex.Message;

                _logger.LogError(ex, "Refund processing exception. " +
                    "Payment {PaymentId}, Transaction {RefundTransactionId}, " +
                    "Amount {RefundAmount:C}, Error: {ErrorMessage}", 
                    paymentId, refundTransactionId, amount, ex.Message);

                throw;
            }
        }
    }

    public async Task<bool> ValidatePaymentAmountAsync(decimal amount)
    {
        _logger.LogDebug("Validating payment amount {Amount:C}", amount);

        await Task.Delay(Random.Shared.Next(5, 15));

        if (amount <= 0)
        {
            _logger.LogWarning("Payment validation failed: Amount {Amount:C} must be positive", amount);
            return false;
        }

        if (amount > 10000) // Business rule: max transaction amount
        {
            _logger.LogWarning("Payment validation failed: Amount {Amount:C} exceeds maximum limit", amount);
            return false;
        }

        _logger.LogDebug("Payment amount {Amount:C} validation passed", amount);
        return true;
    }

    private async Task ValidatePaymentRequest(ProcessPaymentRequest request)
    {
        _logger.LogDebug("Validating payment request for order {OrderId}", request.OrderId);

        if (request.OrderId <= 0)
        {
            var message = "Invalid order ID";
            _logger.LogError("Payment validation failed: {ValidationError}", message);
            throw new ArgumentException(message);
        }

        var isValidAmount = await ValidatePaymentAmountAsync(request.Amount);
        if (!isValidAmount)
        {
            var message = $"Invalid payment amount: {request.Amount:C}";
            _logger.LogError("Payment validation failed: {ValidationError}", message);
            throw new ArgumentException(message);
        }

        _logger.LogDebug("Payment request validation completed successfully");
    }

    private async Task<(bool Success, string? ErrorMessage)> ProcessWithPaymentProvider(Payment payment)
    {
        _logger.LogDebug("Processing payment {PaymentId} with external provider. " +
            "Method: {PaymentMethod}, Amount: {Amount:C}", 
            payment.PaymentId, payment.Method, payment.Amount);

        // Simulate external payment provider call
        await Task.Delay(Random.Shared.Next(500, 2000));

        // Simulate various outcomes based on payment method and amount
        var successRate = payment.Method switch
        {
            PaymentMethod.CreditCard => 0.95,
            PaymentMethod.DebitCard => 0.92,
            PaymentMethod.PayPal => 0.98,
            PaymentMethod.BankTransfer => 0.85,
            _ => 0.90
        };

        var isSuccess = Random.Shared.NextDouble() < successRate;

        if (isSuccess)
        {
            _logger.LogDebug("External payment provider returned success for payment {PaymentId}", 
                payment.PaymentId);
            return (true, null);
        }
        else
        {
            var errorMessage = GenerateRandomErrorMessage();
            _logger.LogWarning("External payment provider returned error for payment {PaymentId}: {ErrorMessage}", 
                payment.PaymentId, errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<Payment?> ProcessRefundAsync(int paymentId, string reason)
    {
        using (LogContext.PushProperty("PaymentId", paymentId))
        using (LogContext.PushProperty("RefundReason", reason))
        {
            _logger.LogInformation("Starting refund process for payment {PaymentId}, Reason: {RefundReason}",
                paymentId, reason);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Validate the refund request
                if (string.IsNullOrWhiteSpace(reason))
                {
                    var message = "Refund reason is required";
                    _logger.LogError("Refund validation failed: {ValidationError}", message);
                    throw new ArgumentException(message);
                }

                // Simulate finding the original payment
                await Task.Delay(Random.Shared.Next(50, 100));

                // Mock payment lookup - in real app this would query the database
                var originalPayment = new Payment
                {
                    PaymentId = paymentId,
                    OrderId = 1,
                    Amount = 99.99m,
                    Method = PaymentMethod.CreditCard,
                    Status = PaymentStatus.Completed,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    ProcessedAt = DateTime.UtcNow.AddDays(-5),
                    TransactionId = Guid.NewGuid().ToString()
                };

                // Validate that the payment can be refunded
                if (originalPayment.Status != PaymentStatus.Completed)
                {
                    var message = $"Payment {paymentId} cannot be refunded. Current status: {originalPayment.Status}";
                    _logger.LogWarning("Refund rejected: {RejectReason}", message);
                    throw new InvalidOperationException(message);
                }

                // Process the refund with external provider
                _logger.LogInformation("Processing refund with payment provider for payment {PaymentId}", paymentId);
                await Task.Delay(Random.Shared.Next(1000, 3000)); // Simulate provider call

                // Create refunded payment record
                var refundedPayment = new Payment
                {
                    PaymentId = originalPayment.PaymentId,
                    OrderId = originalPayment.OrderId,
                    Amount = originalPayment.Amount,
                    Method = originalPayment.Method,
                    Status = PaymentStatus.Refunded,
                    CreatedAt = originalPayment.CreatedAt,
                    ProcessedAt = DateTime.UtcNow,
                    TransactionId = originalPayment.TransactionId
                };

                stopwatch.Stop();

                // Log successful refund with audit trail
                using (LogContext.PushProperty("AuditEvent", "RefundProcessed"))
                using (LogContext.PushProperty("OperationName", "ProcessRefund"))
                using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                {
                    _logger.LogWarning("AUDIT: Refund processed successfully for payment {PaymentId}, " +
                        "Amount: {Amount:C}, Reason: {RefundReason}, Duration: {Duration}ms",
                        paymentId, refundedPayment.Amount, reason, stopwatch.ElapsedMilliseconds);
                }

                return refundedPayment;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                _logger.LogError(ex, "Refund processing failed for payment {PaymentId} after {ElapsedMs}ms. Reason: {RefundReason}",
                    paymentId, stopwatch.ElapsedMilliseconds, reason);
                
                throw;
            }
        }
    }

    private static string GenerateRandomErrorMessage()
    {
        var errors = new[]
        {
            "Insufficient funds",
            "Card expired",
            "Transaction declined by bank",
            "Invalid card number",
            "Network timeout",
            "Provider temporarily unavailable",
            "Security check failed"
        };

        return errors[Random.Shared.Next(errors.Length)];
    }
}
