# Serilog Demo Project - Comprehensive Structured Logging in .NET Core

This project demonstrates advanced structured logging techniques using Serilog in a .NET Core Web API application. It showcases real-world logging scenarios across different business contexts including user management, order processing, and payment handling.

## Table of Contents

- [Overview](#overview)
- [Features Demonstrated](#features-demonstrated)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Logging Concepts Covered](#logging-concepts-covered)
- [API Endpoints](#api-endpoints)
- [Log Output Examples](#log-output-examples)
- [Best Practices Demonstrated](#best-practices-demonstrated)
- [Configuration Details](#configuration-details)

## Overview

This project is designed as a comprehensive learning resource for understanding how to implement production-ready structured logging in .NET Core applications. It covers everything from basic logging setup to advanced scenarios like audit trails, performance monitoring, and security logging.

### Key Technologies Used

- **.NET 8.0** - Latest LTS version with improved performance
- **Serilog** - Structured logging framework
- **ASP.NET Core Web API** - RESTful API framework
- **Multiple Serilog Sinks** - Console, File, and filtered outputs

## Features Demonstrated

### 1. **Bootstrap Logging**
- Early startup logging to catch configuration errors
- Separate logger for initialization phase

### 2. **Multiple Sinks Configuration**
- Console output for development
- Rolling file logs for persistent storage
- Separate audit trail logs
- Error-specific log files
- Performance monitoring logs

### 3. **Request Logging Middleware**
- Automatic HTTP request/response logging
- Request timing and performance metrics
- Custom enrichment with correlation IDs

### 4. **Structured Data Logging**
- Proper use of message templates
- Strongly-typed log properties
- Context enrichment with LogContext

### 5. **Business Workflow Logging**
- Order processing workflows
- Payment transaction logging
- User authentication flows

### 6. **Security and Audit Logging**
- Authentication attempt logging
- Financial transaction audit trails
- Sensitive data handling best practices

### 7. **Performance Monitoring**
- Method execution timing
- Database operation performance
- Business process duration tracking

### 8. **Error Handling and Alerting**
- Structured exception logging
- Critical incident logging
- Business vs. technical error differentiation

## Project Structure

```
Serilog.Demo/
├── Controllers/
│   ├── UsersController.cs      # User management endpoints
│   ├── OrdersController.cs     # Order processing endpoints
│   └── PaymentController.cs    # Payment processing endpoints
├── Models/
│   └── DomainModels.cs         # Business entities and DTOs
├── Services/
│   ├── ServiceInterfaces.cs    # Service contracts
│   ├── UserService.cs          # User business logic
│   ├── OrderService.cs         # Order processing logic
│   └── PaymentService.cs       # Payment processing logic
├── Program.cs                  # Application startup and Serilog configuration
├── appsettings.json           # Serilog configuration and app settings
└── README.md                  # This documentation
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 / VS Code / JetBrains Rider

### Installation

1. **Clone or download** this project to your local machine

2. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```

5. **Access the API**:
   - Base URL: `https://localhost:7216` or `http://localhost:5216`
   - Swagger UI: `https://localhost:7216/swagger`

## Creating the Project from Scratch

Follow these steps to build the Serilog Demo project from the ground up:

### Step 1: Create Project Structure

1. **Create the solution directory and navigate to it**
   ```powershell
   mkdir Serilog.Demo
   cd Serilog.Demo
   ```

2. **Create a new Web API project**
   ```powershell
   dotnet new webapi --name Serilog.Demo --framework net8.0
   cd Serilog.Demo
   ```

3. **Install required NuGet packages**
   ```powershell
   # Core Serilog packages
   dotnet add package Serilog.AspNetCore --version 8.0.0
   dotnet add package Serilog.Sinks.Console --version 5.0.1
   dotnet add package Serilog.Sinks.File --version 5.0.0
   
   # Enrichment packages
   dotnet add package Serilog.Enrichers.Environment --version 2.3.0
   dotnet add package Serilog.Enrichers.Process --version 2.0.2
   dotnet add package Serilog.Enrichers.Thread --version 3.1.0
   
   # Expression filtering for advanced log filtering
   dotnet add package Serilog.Expressions --version 4.0.0
   dotnet add package Serilog.Filters.Expressions --version 3.0.0
   ```

### Step 2: Create Domain Models

1. **Create the Models directory**
   ```powershell
   mkdir Models
   ```

2. **Create DomainModels.cs with comprehensive business entities**
   ```csharp
   namespace Serilog.Demo.Models
   {
       public class User
       {
           public int UserId { get; set; }
           public string Username { get; set; } = string.Empty;
           public string Email { get; set; } = string.Empty;
           public string FirstName { get; set; } = string.Empty;
           public string LastName { get; set; } = string.Empty;
           public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
           public bool IsActive { get; set; } = true;
       }

       public class Order
       {
           public int OrderId { get; set; }
           public int UserId { get; set; }
           public List<OrderItem> Items { get; set; } = new List<OrderItem>();
           public decimal TotalAmount { get; set; }
           public OrderStatus Status { get; set; } = OrderStatus.Pending;
           public string ShippingAddress { get; set; } = string.Empty;
           public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
           public DateTime? CompletedAt { get; set; }
       }

       public class OrderItem
       {
           public string ProductName { get; set; } = string.Empty;
           public int Quantity { get; set; }
           public decimal UnitPrice { get; set; }
           public decimal TotalPrice => Quantity * UnitPrice;
       }

       public enum OrderStatus
       {
           Pending,
           Processing,
           Shipped,
           Delivered,
           Cancelled
       }

       public class Payment
       {
           public int PaymentId { get; set; }
           public int OrderId { get; set; }
           public decimal Amount { get; set; }
           public PaymentMethod Method { get; set; }
           public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
           public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
           public string? TransactionId { get; set; }
           public string? FailureReason { get; set; }
       }

       public enum PaymentMethod
       {
           CreditCard = 1,
           DebitCard = 2,
           PayPal = 3,
           BankTransfer = 4
       }

       public enum PaymentStatus
       {
           Pending,
           Completed,
           Failed,
           Refunded
       }

       // DTOs for API requests and responses
       public class CreateUserRequest
       {
           public string Username { get; set; } = string.Empty;
           public string Email { get; set; } = string.Empty;
           public string FirstName { get; set; } = string.Empty;
           public string LastName { get; set; } = string.Empty;
       }

       public class LoginRequest
       {
           public string Username { get; set; } = string.Empty;
           public string Password { get; set; } = string.Empty;
       }

       public class CreateOrderRequest
       {
           public int UserId { get; set; }
           public List<OrderItem> Items { get; set; } = new List<OrderItem>();
           public string ShippingAddress { get; set; } = string.Empty;
       }

       public class ProcessPaymentRequest
       {
           public int OrderId { get; set; }
           public PaymentMethod Method { get; set; }
           public decimal Amount { get; set; }
       }

       public class RefundRequest
       {
           public int PaymentId { get; set; }
           public decimal Amount { get; set; }
           public string Reason { get; set; } = string.Empty;
       }

       public class UpdateOrderStatusRequest
       {
           public OrderStatus Status { get; set; }
       }

       public class ApiResponse<T>
       {
           public bool Success { get; set; }
           public T? Data { get; set; }
           public string Message { get; set; } = string.Empty;
           public string? CorrelationId { get; set; }
       }
   }
   ```

### Step 3: Create Service Interfaces

1. **Create the Services directory**
   ```powershell
   mkdir Services
   ```

2. **Create ServiceInterfaces.cs with comprehensive service contracts**
   ```csharp
   using Serilog.Demo.Models;

   namespace Serilog.Demo.Services
   {
       public interface IUserService
       {
           Task<User?> AuthenticateAsync(string username, string password);
           Task<User> CreateUserAsync(CreateUserRequest request);
           Task<User?> GetUserByIdAsync(int userId);
           Task<User> UpdateUserAsync(int userId, CreateUserRequest request);
           Task<bool> DeleteUserAsync(int userId);
       }

       public interface IOrderService
       {
           Task<Order> CreateOrderAsync(CreateOrderRequest request);
           Task<Order?> GetOrderByIdAsync(int orderId);
           Task<List<Order>> GetOrdersByUserIdAsync(int userId);
           Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);
       }

       public interface IPaymentService
       {
           Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest request);
           Task<Payment?> GetPaymentByIdAsync(int paymentId);
           Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId);
           Task<Payment> ProcessRefundAsync(RefundRequest request);
       }
   }
   ```

### Step 4: Implement Service Classes with Comprehensive Logging

1. **Create UserService.cs with authentication and security logging**
   ```csharp
   using Serilog.Demo.Models;
   using Serilog;
   using System.Diagnostics;

   namespace Serilog.Demo.Services
   {
       public class UserService : IUserService
       {
           private readonly ILogger<UserService> _logger;
           private static readonly List<User> _users = new();
           private static int _nextUserId = 1;

           public UserService(ILogger<UserService> logger)
           {
               _logger = logger;
           }

           public async Task<User?> AuthenticateAsync(string username, string password)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];
               var stopwatch = Stopwatch.StartNew();

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Operation", "UserAuthentication"))
               {
                   _logger.LogInformation("Authentication attempt for username {Username}", username);

                   try
                   {
                       // Simulate async database call
                       await Task.Delay(100);

                       var user = _users.FirstOrDefault(u => u.Username == username && u.IsActive);

                       if (user == null)
                       {
                           _logger.LogWarning("Authentication failed - User {Username} not found", username);
                           return null;
                       }

                       // In real implementation, verify password hash
                       if (password != "password123")
                       {
                           _logger.LogWarning("Authentication failed - Invalid password for user {Username}", username);
                           return null;
                       }

                       stopwatch.Stop();
                       using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                       {
                           _logger.LogInformation("Authentication successful for user {Username} (UserId: {UserId})", 
                               username, user.UserId);
                       }

                       return user;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error during authentication for username {Username}", username);
                       throw;
                   }
               }
           }

           public async Task<User> CreateUserAsync(CreateUserRequest request)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];
               var stopwatch = Stopwatch.StartNew();

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Operation", "CreateUser"))
               {
                   _logger.LogInformation("Creating user with username {Username} and email {Email}", 
                       request.Username, request.Email);

                   try
                   {
                       // Check for existing user
                       if (_users.Any(u => u.Username == request.Username))
                       {
                           _logger.LogWarning("User creation failed - Username {Username} already exists", request.Username);
                           throw new InvalidOperationException("Username already exists");
                       }

                       if (_users.Any(u => u.Email == request.Email))
                       {
                           _logger.LogWarning("User creation failed - Email {Email} already exists", request.Email);
                           throw new InvalidOperationException("Email already exists");
                       }

                       // Simulate async database operation
                       await Task.Delay(200);

                       var user = new User
                       {
                           UserId = _nextUserId++,
                           Username = request.Username,
                           Email = request.Email,
                           FirstName = request.FirstName,
                           LastName = request.LastName,
                           CreatedAt = DateTime.UtcNow,
                           IsActive = true
                       };

                       _users.Add(user);
                       stopwatch.Stop();

                       using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                       using (LogContext.PushProperty("UserId", user.UserId))
                       {
                           _logger.LogInformation("User created successfully - UserId: {UserId}, Username: {Username}", 
                               user.UserId, user.Username);
                       }

                       return user;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error creating user with username {Username}", request.Username);
                       throw;
                   }
               }
           }

           public async Task<User?> GetUserByIdAsync(int userId)
           {
               using (LogContext.PushProperty("Operation", "GetUser"))
               {
                   _logger.LogDebug("Retrieving user with ID {UserId}", userId);

                   try
                   {
                       await Task.Delay(50);
                       var user = _users.FirstOrDefault(u => u.UserId == userId);

                       if (user == null)
                       {
                           _logger.LogWarning("User with ID {UserId} not found", userId);
                       }

                       return user;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error retrieving user with ID {UserId}", userId);
                       throw;
                   }
               }
           }

           public async Task<User> UpdateUserAsync(int userId, CreateUserRequest request)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Operation", "UpdateUser"))
               {
                   _logger.LogInformation("Updating user {UserId} with new data", userId);

                   try
                   {
                       var user = _users.FirstOrDefault(u => u.UserId == userId);
                       if (user == null)
                       {
                           _logger.LogWarning("Update failed - User {UserId} not found", userId);
                           throw new InvalidOperationException("User not found");
                       }

                       // Check for username/email conflicts with other users
                       if (_users.Any(u => u.UserId != userId && u.Username == request.Username))
                       {
                           _logger.LogWarning("Update failed - Username {Username} already exists for another user", 
                               request.Username);
                           throw new InvalidOperationException("Username already exists");
                       }

                       await Task.Delay(150);

                       user.Username = request.Username;
                       user.Email = request.Email;
                       user.FirstName = request.FirstName;
                       user.LastName = request.LastName;

                       _logger.LogInformation("User {UserId} updated successfully", userId);
                       return user;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error updating user {UserId}", userId);
                       throw;
                   }
               }
           }

           public async Task<bool> DeleteUserAsync(int userId)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Operation", "DeleteUser"))
               {
                   _logger.LogInformation("Attempting to delete user {UserId}", userId);

                   try
                   {
                       var user = _users.FirstOrDefault(u => u.UserId == userId);
                       if (user == null)
                       {
                           _logger.LogWarning("Delete failed - User {UserId} not found", userId);
                           return false;
                       }

                       await Task.Delay(100);

                       // Soft delete by marking as inactive
                       user.IsActive = false;

                       _logger.LogInformation("User {UserId} deleted successfully (soft delete)", userId);
                       return true;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error deleting user {UserId}", userId);
                       throw;
                   }
               }
           }
       }
   }
   ```

2. **Create OrderService.cs with business workflow logging**
   ```csharp
   using Serilog.Demo.Models;
   using Serilog;
   using System.Diagnostics;

   namespace Serilog.Demo.Services
   {
       public class OrderService : IOrderService
       {
           private readonly ILogger<OrderService> _logger;
           private readonly IUserService _userService;
           private static readonly List<Order> _orders = new();
           private static int _nextOrderId = 1;

           public OrderService(ILogger<OrderService> logger, IUserService userService)
           {
               _logger = logger;
               _userService = userService;
           }

           public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];
               var stopwatch = Stopwatch.StartNew();

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Operation", "CreateOrder"))
               using (LogContext.PushProperty("UserId", request.UserId))
               {
                   _logger.LogInformation("Processing order creation for user {UserId} with {ItemCount} items", 
                       request.UserId, request.Items.Count);

                   try
                   {
                       // Validate user exists
                       var user = await _userService.GetUserByIdAsync(request.UserId);
                       if (user == null)
                       {
                           _logger.LogWarning("Order creation failed - User {UserId} not found", request.UserId);
                           throw new InvalidOperationException("User not found");
                       }

                       // Validate items
                       if (!request.Items.Any())
                       {
                           _logger.LogWarning("Order creation failed - No items provided for user {UserId}", request.UserId);
                           throw new ArgumentException("Order must contain at least one item");
                       }

                       var totalAmount = request.Items.Sum(item => item.TotalPrice);

                       var order = new Order
                       {
                           OrderId = _nextOrderId++,
                           UserId = request.UserId,
                           Items = request.Items,
                           TotalAmount = totalAmount,
                           ShippingAddress = request.ShippingAddress,
                           Status = OrderStatus.Pending,
                           CreatedAt = DateTime.UtcNow
                       };

                       // Simulate database save
                       await Task.Delay(300);
                       _orders.Add(order);

                       stopwatch.Stop();

                       using (LogContext.PushProperty("OrderId", order.OrderId))
                       using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                       {
                           _logger.LogInformation("Order {OrderId} created successfully for user {UserId} with total amount {TotalAmount:C}", 
                               order.OrderId, request.UserId, order.TotalAmount);

                           // Log individual items for audit trail
                           foreach (var item in order.Items)
                           {
                               _logger.LogDebug("Order {OrderId} contains: {ProductName} x{Quantity} at {UnitPrice:C} each", 
                                   order.OrderId, item.ProductName, item.Quantity, item.UnitPrice);
                           }
                       }

                       return order;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error creating order for user {UserId}", request.UserId);
                       throw;
                   }
               }
           }

           public async Task<Order?> GetOrderByIdAsync(int orderId)
           {
               using (LogContext.PushProperty("Operation", "GetOrder"))
               {
                   _logger.LogDebug("Retrieving order {OrderId}", orderId);

                   try
                   {
                       await Task.Delay(50);
                       var order = _orders.FirstOrDefault(o => o.OrderId == orderId);

                       if (order == null)
                       {
                           _logger.LogWarning("Order {OrderId} not found", orderId);
                       }

                       return order;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error retrieving order {OrderId}", orderId);
                       throw;
                   }
               }
           }

           public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
           {
               using (LogContext.PushProperty("Operation", "GetOrdersByUser"))
               using (LogContext.PushProperty("UserId", userId))
               {
                   _logger.LogDebug("Retrieving orders for user {UserId}", userId);

                   try
                   {
                       await Task.Delay(100);
                       var orders = _orders.Where(o => o.UserId == userId).ToList();

                       _logger.LogDebug("Found {OrderCount} orders for user {UserId}", orders.Count, userId);
                       return orders;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error retrieving orders for user {UserId}", userId);
                       throw;
                   }
               }
           }

           public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Operation", "UpdateOrderStatus"))
               using (LogContext.PushProperty("OrderId", orderId))
               {
                   _logger.LogInformation("Updating order {OrderId} status to {NewStatus}", orderId, status);

                   try
                   {
                       var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
                       if (order == null)
                       {
                           _logger.LogWarning("Status update failed - Order {OrderId} not found", orderId);
                           throw new InvalidOperationException("Order not found");
                       }

                       var oldStatus = order.Status;
                       order.Status = status;

                       if (status == OrderStatus.Delivered)
                       {
                           order.CompletedAt = DateTime.UtcNow;
                       }

                       await Task.Delay(100);

                       _logger.LogInformation("Order {OrderId} status updated from {OldStatus} to {NewStatus}", 
                           orderId, oldStatus, status);

                       return order;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error updating status for order {OrderId} to {NewStatus}", orderId, status);
                       throw;
                   }
               }
           }
       }
   }
   ```

3. **Create PaymentService.cs with financial audit logging**
   ```csharp
   using Serilog.Demo.Models;
   using Serilog;
   using System.Diagnostics;

   namespace Serilog.Demo.Services
   {
       public class PaymentService : IPaymentService
       {
           private readonly ILogger<PaymentService> _logger;
           private readonly IOrderService _orderService;
           private static readonly List<Payment> _payments = new();
           private static int _nextPaymentId = 1;

           public PaymentService(ILogger<PaymentService> logger, IOrderService orderService)
           {
               _logger = logger;
               _orderService = orderService;
           }

           public async Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest request)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];
               var stopwatch = Stopwatch.StartNew();

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Operation", "ProcessPayment"))
               using (LogContext.PushProperty("OrderId", request.OrderId))
               using (LogContext.PushProperty("AuditEvent", "PaymentAttempt"))
               {
                   _logger.LogInformation("Processing payment for order {OrderId} - Amount: {Amount:C}, Method: {PaymentMethod}", 
                       request.OrderId, request.Amount, request.Method);

                   try
                   {
                       // Validate order exists
                       var order = await _orderService.GetOrderByIdAsync(request.OrderId);
                       if (order == null)
                       {
                           _logger.LogWarning("Payment failed - Order {OrderId} not found", request.OrderId);
                           throw new InvalidOperationException("Order not found");
                       }

                       // Validate amount matches order total
                       if (request.Amount != order.TotalAmount)
                       {
                           _logger.LogWarning("Payment failed - Amount mismatch for order {OrderId}. Expected: {ExpectedAmount:C}, Provided: {ProvidedAmount:C}", 
                               request.OrderId, order.TotalAmount, request.Amount);
                           throw new ArgumentException("Payment amount does not match order total");
                       }

                       // Simulate payment processing
                       await Task.Delay(500);

                       var payment = new Payment
                       {
                           PaymentId = _nextPaymentId++,
                           OrderId = request.OrderId,
                           Amount = request.Amount,
                           Method = request.Method,
                           Status = PaymentStatus.Pending,
                           ProcessedAt = DateTime.UtcNow
                       };

                       // Simulate payment gateway interaction
                       var success = await SimulatePaymentGatewayAsync(payment);

                       if (success)
                       {
                           payment.Status = PaymentStatus.Completed;
                           payment.TransactionId = $"TXN_{Guid.NewGuid().ToString("N")[..10].ToUpper()}";

                           // Update order status
                           await _orderService.UpdateOrderStatusAsync(request.OrderId, OrderStatus.Processing);
                       }
                       else
                       {
                           payment.Status = PaymentStatus.Failed;
                           payment.FailureReason = "Payment gateway declined the transaction";
                       }

                       _payments.Add(payment);
                       stopwatch.Stop();

                       using (LogContext.PushProperty("PaymentId", payment.PaymentId))
                       using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                       using (LogContext.PushProperty("AuditEvent", payment.Status == PaymentStatus.Completed ? "PaymentCompleted" : "PaymentFailed"))
                       {
                           if (payment.Status == PaymentStatus.Completed)
                           {
                               _logger.LogInformation("AUDIT: Payment {PaymentId} completed successfully for order {OrderId} - Amount: {Amount:C}, Method: {PaymentMethod}, Transaction: {TransactionId}", 
                                   payment.PaymentId, request.OrderId, payment.Amount, payment.Method, payment.TransactionId);
                           }
                           else
                           {
                               _logger.LogWarning("AUDIT: Payment {PaymentId} failed for order {OrderId} - Amount: {Amount:C}, Method: {PaymentMethod}, Reason: {FailureReason}", 
                                   payment.PaymentId, request.OrderId, payment.Amount, payment.Method, payment.FailureReason);
                           }
                       }

                       return payment;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error processing payment for order {OrderId} with amount {Amount:C}", request.OrderId, request.Amount);
                       throw;
                   }
               }
           }

           public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
           {
               using (LogContext.PushProperty("Operation", "GetPayment"))
               {
                   _logger.LogDebug("Retrieving payment {PaymentId}", paymentId);

                   try
                   {
                       await Task.Delay(50);
                       var payment = _payments.FirstOrDefault(p => p.PaymentId == paymentId);

                       if (payment == null)
                       {
                           _logger.LogWarning("Payment {PaymentId} not found", paymentId);
                       }

                       return payment;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error retrieving payment {PaymentId}", paymentId);
                       throw;
                   }
               }
           }

           public async Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId)
           {
               using (LogContext.PushProperty("Operation", "GetPaymentsByOrder"))
               using (LogContext.PushProperty("OrderId", orderId))
               {
                   _logger.LogDebug("Retrieving payments for order {OrderId}", orderId);

                   try
                   {
                       await Task.Delay(100);
                       var payments = _payments.Where(p => p.OrderId == orderId).ToList();

                       _logger.LogDebug("Found {PaymentCount} payments for order {OrderId}", payments.Count, orderId);
                       return payments;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error retrieving payments for order {OrderId}", orderId);
                       throw;
                   }
               }
           }

           public async Task<Payment> ProcessRefundAsync(RefundRequest request)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];
               var stopwatch = Stopwatch.StartNew();

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Operation", "ProcessRefund"))
               using (LogContext.PushProperty("PaymentId", request.PaymentId))
               using (LogContext.PushProperty("AuditEvent", "RefundAttempt"))
               {
                   _logger.LogInformation("Processing refund for payment {PaymentId} - Amount: {Amount:C}, Reason: {Reason}", 
                       request.PaymentId, request.Amount, request.Reason);

                   try
                   {
                       var originalPayment = _payments.FirstOrDefault(p => p.PaymentId == request.PaymentId);
                       if (originalPayment == null)
                       {
                           _logger.LogWarning("Refund failed - Payment {PaymentId} not found", request.PaymentId);
                           throw new InvalidOperationException("Payment not found");
                       }

                       if (originalPayment.Status != PaymentStatus.Completed)
                       {
                           _logger.LogWarning("Refund failed - Payment {PaymentId} is not in completed status. Current status: {PaymentStatus}", 
                               request.PaymentId, originalPayment.Status);
                           throw new InvalidOperationException("Can only refund completed payments");
                       }

                       if (request.Amount > originalPayment.Amount)
                       {
                           _logger.LogWarning("Refund failed - Refund amount {RefundAmount:C} exceeds original payment amount {OriginalAmount:C} for payment {PaymentId}", 
                               request.Amount, originalPayment.Amount, request.PaymentId);
                           throw new ArgumentException("Refund amount cannot exceed original payment amount");
                       }

                       // Simulate refund processing
                       await Task.Delay(400);

                       var refundPayment = new Payment
                       {
                           PaymentId = _nextPaymentId++,
                           OrderId = originalPayment.OrderId,
                           Amount = -request.Amount, // Negative amount for refund
                           Method = originalPayment.Method,
                           Status = PaymentStatus.Refunded,
                           ProcessedAt = DateTime.UtcNow,
                           TransactionId = $"REF_{Guid.NewGuid().ToString("N")[..10].ToUpper()}"
                       };

                       _payments.Add(refundPayment);
                       stopwatch.Stop();

                       using (LogContext.PushProperty("RefundPaymentId", refundPayment.PaymentId))
                       using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                       using (LogContext.PushProperty("AuditEvent", "RefundCompleted"))
                       {
                           _logger.LogInformation("AUDIT: Refund {RefundPaymentId} processed successfully for original payment {PaymentId} - Amount: {Amount:C}, Reason: {Reason}", 
                               refundPayment.PaymentId, request.PaymentId, request.Amount, request.Reason);
                       }

                       return refundPayment;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error processing refund for payment {PaymentId} with amount {Amount:C}", request.PaymentId, request.Amount);
                       throw;
                   }
               }
           }

           private async Task<bool> SimulatePaymentGatewayAsync(Payment payment)
           {
               using (LogContext.PushProperty("Operation", "PaymentGateway"))
               using (LogContext.PushProperty("PaymentMethod", payment.Method))
               {
                   _logger.LogDebug("Communicating with payment gateway for payment {PaymentId}", payment.PaymentId);

                   try
                   {
                       // Simulate network call to payment gateway
                       await Task.Delay(200);

                       // Simulate random failures (10% failure rate)
                       var random = new Random();
                       var success = random.Next(1, 11) != 1;

                       using (LogContext.PushProperty("Duration", 200))
                       {
                           if (success)
                           {
                               _logger.LogDebug("Payment gateway approved payment {PaymentId}", payment.PaymentId);
                           }
                           else
                           {
                               _logger.LogWarning("Payment gateway declined payment {PaymentId}", payment.PaymentId);
                           }
                       }

                       return success;
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Payment gateway communication failed for payment {PaymentId}", payment.PaymentId);
                       return false;
                   }
               }
           }
       }
   }
   ```

### Step 5: Create API Controllers

1. **Create the Controllers directory**
   ```powershell
   mkdir Controllers
   ```

2. **Create UsersController.cs with HTTP context logging**
   ```csharp
   using Microsoft.AspNetCore.Mvc;
   using Serilog.Demo.Models;
   using Serilog.Demo.Services;
   using Serilog;
   using System.Diagnostics;

   namespace Serilog.Demo.Controllers
   {
       [ApiController]
       [Route("api/[controller]")]
       public class UsersController : ControllerBase
       {
           private readonly ILogger<UsersController> _logger;
           private readonly IUserService _userService;

           public UsersController(ILogger<UsersController> logger, IUserService userService)
           {
               _logger = logger;
               _userService = userService;
           }

           [HttpPost("login")]
           public async Task<ActionResult<ApiResponse<User>>> Login([FromBody] LoginRequest request)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];
               var stopwatch = Stopwatch.StartNew();

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Endpoint", "POST /api/users/login"))
               {
                   _logger.LogInformation("User login attempt for username {Username}", request.Username);

                   try
                   {
                       var user = await _userService.AuthenticateAsync(request.Username, request.Password);
                       stopwatch.Stop();

                       using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                       {
                           if (user != null)
                           {
                               _logger.LogInformation("Login successful for username {Username} in {Duration}ms", 
                                   request.Username, stopwatch.ElapsedMilliseconds);

                               return Ok(new ApiResponse<User>
                               {
                                   Success = true,
                                   Data = user,
                                   Message = "Login successful",
                                   CorrelationId = correlationId
                               });
                           }
                           else
                           {
                               _logger.LogWarning("Login failed for username {Username} in {Duration}ms", 
                                   request.Username, stopwatch.ElapsedMilliseconds);

                               return Unauthorized(new ApiResponse<User>
                               {
                                   Success = false,
                                   Message = "Invalid username or password",
                                   CorrelationId = correlationId
                               });
                           }
                       }
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error during login attempt for username {Username}", request.Username);

                       return StatusCode(500, new ApiResponse<User>
                       {
                           Success = false,
                           Message = "An error occurred during login",
                           CorrelationId = correlationId
                       });
                   }
               }
           }

           [HttpGet("{userId}")]
           public async Task<ActionResult<ApiResponse<User>>> GetUser(int userId)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Endpoint", "GET /api/users/{userId}"))
               {
                   _logger.LogDebug("Retrieving user {UserId}", userId);

                   try
                   {
                       var user = await _userService.GetUserByIdAsync(userId);

                       if (user == null)
                       {
                           _logger.LogWarning("User {UserId} not found", userId);
                           return NotFound(new ApiResponse<User>
                           {
                               Success = false,
                               Message = "User not found",
                               CorrelationId = correlationId
                           });
                       }

                       _logger.LogDebug("User {UserId} retrieved successfully", userId);
                       return Ok(new ApiResponse<User>
                       {
                           Success = true,
                           Data = user,
                           Message = "User retrieved successfully",
                           CorrelationId = correlationId
                       });
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error retrieving user {UserId}", userId);

                       return StatusCode(500, new ApiResponse<User>
                       {
                           Success = false,
                           Message = "An error occurred while retrieving the user",
                           CorrelationId = correlationId
                       });
                   }
               }
           }

           [HttpPost]
           public async Task<ActionResult<ApiResponse<User>>> CreateUser([FromBody] CreateUserRequest request)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];
               var stopwatch = Stopwatch.StartNew();

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Endpoint", "POST /api/users"))
               {
                   _logger.LogInformation("Creating user with username {Username}", request.Username);

                   try
                   {
                       var user = await _userService.CreateUserAsync(request);
                       stopwatch.Stop();

                       using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                       {
                           _logger.LogInformation("User created successfully - UserId: {UserId}, Username: {Username} in {Duration}ms", 
                               user.UserId, user.Username, stopwatch.ElapsedMilliseconds);
                       }

                       return CreatedAtAction(nameof(GetUser), new { userId = user.UserId }, new ApiResponse<User>
                       {
                           Success = true,
                           Data = user,
                           Message = "User created successfully",
                           CorrelationId = correlationId
                       });
                   }
                   catch (InvalidOperationException ex)
                   {
                       _logger.LogWarning(ex, "User creation failed for username {Username} - {ErrorMessage}", 
                           request.Username, ex.Message);

                       return BadRequest(new ApiResponse<User>
                       {
                           Success = false,
                           Message = ex.Message,
                           CorrelationId = correlationId
                       });
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error creating user with username {Username}", request.Username);

                       return StatusCode(500, new ApiResponse<User>
                       {
                           Success = false,
                           Message = "An error occurred while creating the user",
                           CorrelationId = correlationId
                       });
                   }
               }
           }

           [HttpPut("{userId}")]
           public async Task<ActionResult<ApiResponse<User>>> UpdateUser(int userId, [FromBody] CreateUserRequest request)
           {
               var correlationId = Guid.NewGuid().ToString("N")[..8];

               using (LogContext.PushProperty("CorrelationId", correlationId))
               using (LogContext.PushProperty("Endpoint", "PUT /api/users/{userId}"))
               {
                   _logger.LogInformation("Updating user {UserId}", userId);

                   try
                   {
                       var user = await _userService.UpdateUserAsync(userId, request);

                       _logger.LogInformation("User {UserId} updated successfully", userId);
                       return Ok(new ApiResponse<User>
                       {
                           Success = true,
                           Data = user,
                           Message = "User updated successfully",
                           CorrelationId = correlationId
                       });
                   }
                   catch (InvalidOperationException ex)
                   {
                       _logger.LogWarning(ex, "User update failed for UserId {UserId} - {ErrorMessage}", userId, ex.Message);

                       return BadRequest(new ApiResponse<User>
                       {
                           Success = false,
                           Message = ex.Message,
                           CorrelationId = correlationId
                       });
                   }
                   catch (Exception ex)
                   {
                       _logger.LogError(ex, "Error updating user {UserId}", userId);

                       return StatusCode(500, new ApiResponse<User>
                       {
                           Success = false,
                           Message = "An error occurred while updating the user",
                           CorrelationId = correlationId
                       });
                   }
               }
           }
       }
   }
   ```

3. **Create OrdersController.cs and PaymentController.cs** following similar patterns with comprehensive logging

### Step 6: Configure Serilog

1. **Update appsettings.json with comprehensive Serilog configuration**
   ```json
   {
     "Serilog": {
       "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Expressions"],
       "MinimumLevel": {
         "Default": "Debug",
         "Override": {
           "Microsoft": "Information",
           "Microsoft.Hosting.Lifetime": "Information",
           "System": "Warning"
         }
       },
       "WriteTo": [
         {
           "Name": "Console",
           "Args": {
             "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
           }
         },
         {
           "Name": "File",
           "Args": {
             "path": "logs/serilog-demo-.log",
             "rollingInterval": "Day",
             "retainedFileCountLimit": 30,
             "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}"
           }
         },
         {
           "Name": "Logger",
           "Args": {
             "configureLogger": {
               "Filter": [
                 {
                   "Name": "ByIncludingOnly",
                   "Args": {
                     "expression": "@Properties['AuditEvent'] is not null"
                   }
                 }
               ],
               "WriteTo": [
                 {
                   "Name": "File",
                   "Args": {
                     "path": "logs/audit/audit-.log",
                     "rollingInterval": "Day",
                     "retainedFileCountLimit": 90,
                     "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{AuditEvent}] [{CorrelationId}] {Message:lj}{NewLine}"
                   }
                 }
               ]
             }
           }
         },
         {
           "Name": "Logger",
           "Args": {
             "configureLogger": {
               "Filter": [
                 {
                   "Name": "ByIncludingOnly",
                   "Args": {
                     "expression": "@l in ['Error', 'Fatal', 'Warning']"
                   }
                 }
               ],
               "WriteTo": [
                 {
                   "Name": "File",
                   "Args": {
                     "path": "logs/errors/error-.log",
                     "rollingInterval": "Day",
                     "retainedFileCountLimit": 90,
                     "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}"
                   }
                 }
               ]
             }
           }
         },
         {
           "Name": "Logger",
           "Args": {
             "configureLogger": {
               "Filter": [
                 {
                   "Name": "ByIncludingOnly",
                   "Args": {
                     "expression": "@Properties['Duration'] is not null"
                   }
                 }
               ],
               "WriteTo": [
                 {
                   "Name": "File",
                   "Args": {
                     "path": "logs/performance/performance-.log",
                     "rollingInterval": "Day",
                     "retainedFileCountLimit": 30,
                     "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Operation}] Duration: {Duration}ms {Message:lj}{NewLine}"
                   }
                 }
               ]
             }
           }
         }
       ],
       "Enrich": ["FromLogContext", "WithMachineName", "WithProcessId", "WithThreadId"],
       "Properties": {
         "Application": "Serilog.Demo"
       }
     }
   }
   ```

2. **Update Program.cs with bootstrap logging and comprehensive configuration**
   ```csharp
   using Serilog;
   using Serilog.Events;
   using Serilog.Context;
   using Serilog.Demo.Services;

   // Create early logger for startup errors
   Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
       .Enrich.FromLogContext()
       .WriteTo.Console()
       .CreateBootstrapLogger();

   try
   {
       Log.Information("Starting Serilog Demo application");

       var builder = WebApplication.CreateBuilder(args);

       // Configure Serilog from appsettings.json
       builder.Host.UseSerilog((context, services, configuration) =>
       {
           configuration.ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Application", "Serilog.Demo")
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
       });

       // Add services to the container
       builder.Services.AddControllers();
       builder.Services.AddEndpointsApiExplorer();
       builder.Services.AddSwaggerGen(c =>
       {
           c.SwaggerDoc("v1", new() 
           { 
               Title = "Serilog Demo API", 
               Version = "v1",
               Description = "Comprehensive structured logging demonstration with Serilog"
           });
       });

       // Register application services
       builder.Services.AddScoped<IUserService, UserService>();
       builder.Services.AddScoped<IOrderService, OrderService>();
       builder.Services.AddScoped<IPaymentService, PaymentService>();

       var app = builder.Build();

       // Configure the HTTP request pipeline
       if (app.Environment.IsDevelopment())
       {
           app.UseSwagger();
           app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "Serilog Demo API v1");
               c.RoutePrefix = string.Empty;
           });
       }

       app.UseHttpsRedirection();

       // Add request logging with enrichment
       app.UseSerilogRequestLogging(options =>
       {
           options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
           options.GetLevel = (httpContext, elapsed, ex) => ex != null
               ? LogEventLevel.Error 
               : httpContext.Response.StatusCode > 499 
                   ? LogEventLevel.Error 
                   : LogEventLevel.Information;
           
           options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
           {
               diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
               diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
               diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
               diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());
               diagnosticContext.Set("RequestId", httpContext.TraceIdentifier);
           };
       });

       app.UseAuthorization();
       app.MapControllers();

       // Add health check endpoint
       app.MapGet("/health", () => 
       {
           Log.Information("Health check endpoint accessed");
           return new { 
               Status = "Healthy", 
               Timestamp = DateTime.UtcNow,
               Environment = app.Environment.EnvironmentName,
               Application = "Serilog.Demo"
           };
       });

       Log.Information("Serilog Demo application configured successfully");
       app.Run();
   }
   catch (Exception ex)
   {
       Log.Fatal(ex, "Serilog Demo application terminated unexpectedly");
   }
   finally
   {
       Log.CloseAndFlush();
   }
   ```

### Step 7: Create API Test File

1. **Create api-tests.http for comprehensive testing**
   ```http
   ### Variables
   @baseUrl = https://localhost:7216
   @contentType = application/json

   ### Health Check
   GET {{baseUrl}}/health

   ### Create User
   POST {{baseUrl}}/api/users
   Content-Type: {{contentType}}

   {
     "username": "john.doe",
     "email": "john.doe@example.com",
     "firstName": "John",
     "lastName": "Doe"
   }

   ### User Login - Success
   POST {{baseUrl}}/api/users/login
   Content-Type: {{contentType}}

   {
     "username": "john.doe",
     "password": "password123"
   }

   ### User Login - Failure
   POST {{baseUrl}}/api/users/login
   Content-Type: {{contentType}}

   {
     "username": "john.doe",
     "password": "wrongpassword"
   }

   ### Get User
   GET {{baseUrl}}/api/users/1

   ### Create Order
   POST {{baseUrl}}/api/orders
   Content-Type: {{contentType}}

   {
     "userId": 1,
     "items": [
       {
         "productName": "Laptop",
         "quantity": 1,
         "unitPrice": 999.99
       },
       {
         "productName": "Mouse",
         "quantity": 2,
         "unitPrice": 25.50
       }
     ],
     "shippingAddress": "123 Main St, Anytown, USA"
   }

   ### Process Payment
   POST {{baseUrl}}/api/payment/process
   Content-Type: {{contentType}}

   {
     "orderId": 1,
     "method": 1,
     "amount": 1050.99
   }

   ### Process Refund
   POST {{baseUrl}}/api/payment/refund/1
   Content-Type: {{contentType}}

   {
     "paymentId": 1,
     "amount": 500.00,
     "reason": "Customer requested partial refund"
   }
   ```

### Step 8: Build and Test

1. **Create logs directory structure**
   ```powershell
   mkdir logs
   mkdir logs\audit
   mkdir logs\errors
   mkdir logs\performance
   ```

2. **Build the project**
   ```powershell
   dotnet build
   ```

3. **Run the application**
   ```powershell
   dotnet run
   ```

4. **Test using the HTTP file or Swagger UI**
   - Navigate to `https://localhost:7216` for Swagger UI
   - Use the `api-tests.http` file for testing
   - Monitor log files in the `logs` directory

### Implementation Best Practices Applied

- **Bootstrap Logging**: Early startup error capture
- **Structured Properties**: Meaningful property names with proper data types
- **Correlation IDs**: Request tracking across service boundaries
- **Performance Monitoring**: Method execution timing with context
- **Security Logging**: Authentication attempts and audit trails
- **Log Filtering**: Separate logs by type and importance
- **Context Enrichment**: Automatic property inclusion with LogContext
- **Error Handling**: Comprehensive exception logging with context

### Viewing Logs

The application will create log files in the following directories:

```
logs/
├── serilog-demo-20240101.log      # General application logs
├── audit/
│   └── audit-20240101.log         # Audit trail logs
├── errors/
│   └── error-20240101.log         # Error and warning logs
└── performance/
    └── performance-20240101.log   # Performance monitoring logs
```

## Logging Concepts Covered

### 1. **Message Templates and Structured Data**

```csharp
// ❌ String interpolation (avoid)
_logger.LogInformation($"User {user.Username} logged in at {DateTime.Now}");

// ✅ Structured logging (recommended)
_logger.LogInformation("User {Username} logged in at {LoginTime}", 
    user.Username, DateTime.UtcNow);
```

### 2. **Log Context Enrichment**

```csharp
using (LogContext.PushProperty("CorrelationId", correlationId))
using (LogContext.PushProperty("UserId", userId))
{
    _logger.LogInformation("Processing order for user {UserId}", userId);
    // All logs within this scope will include CorrelationId and UserId
}
```

### 3. **Performance Monitoring**

```csharp
var stopwatch = Stopwatch.StartNew();
try
{
    var result = await ProcessOrderAsync(order);
    return result;
}
finally
{
    stopwatch.Stop();
    using (LogContext.PushProperty("OperationName", "ProcessOrder"))
    using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
    {
        _logger.LogInformation("Order processing completed in {Duration}ms", 
            stopwatch.ElapsedMilliseconds);
    }
}
```

### 4. **Security and Audit Logging**

```csharp
// Authentication logging
_logger.LogWarning("Failed login attempt for user {Username} from IP {ClientIP}", 
    username, clientIP);

// Financial audit trail
using (LogContext.PushProperty("AuditEvent", "PaymentCompleted"))
{
    _logger.LogInformation("AUDIT: Payment {PaymentId} processed for {Amount:C}", 
        paymentId, amount);
}
```

## API Endpoints

### Users Controller
- `POST /api/users/login` - User authentication
- `GET /api/users/{userId}` - Get user details
- `POST /api/users` - Create new user
- `PUT /api/users/{userId}` - Update user information

### Orders Controller
- `POST /api/orders` - Create new order
- `GET /api/orders/{orderId}` - Get order details
- `PUT /api/orders/{orderId}/status` - Update order status
- `GET /api/orders/user/{userId}` - Get user's orders

### Payment Controller
- `POST /api/payment/process` - Process payment
- `POST /api/payment/refund/{paymentId}` - Process refund
- `GET /api/payment/{paymentId}` - Get payment details
- `GET /api/payment/order/{orderId}` - Get order payments

## Log Output Examples

### Console Output (Development)
```
[2024-01-15 10:30:45.123 +00:00] [INF] [Serilog.Demo.Controllers.UsersController] User login attempt for username "john.doe"
[2024-01-15 10:30:45.156 +00:00] [INF] [Serilog.Demo.Services.UserService] Authentication successful for user john.doe (UserId: 123)
```

### File Output (Production)
```
[2024-01-15 10:30:45.123 +00:00] [INF] [Serilog.Demo.Services.OrderService] [COR-123-456] Processing order creation for user 123 with 3 items
[2024-01-15 10:30:45.167 +00:00] [INF] [Serilog.Demo.Services.OrderService] [COR-123-456] Order 789 created successfully with total $156.78
```

### Audit Trail Output
```
[2024-01-15 10:35:22.445 +00:00] [INF] [PaymentCompleted] [COR-123-456] AUDIT: Payment 456 processed for order 789. Amount: $156.78, Method: CreditCard
[2024-01-15 10:45:15.223 +00:00] [WRN] [RefundProcessed] [COR-789-012] AUDIT: Refund processed - PaymentId: 456, Amount: $156.78, Reason: Customer request
```

### Performance Log Output
```
[2024-01-15 10:30:45.200 +00:00] [INF] [ProcessOrder] Duration: 245ms Order processing workflow completed for order 789
[2024-01-15 10:30:45.201 +00:00] [INF] [ValidatePayment] Duration: 45ms Payment validation completed for amount $156.78
```

## Best Practices Demonstrated

### 1. **Sensitive Data Handling**
- Never log passwords, credit card numbers, or personal data
- Use placeholder values for sensitive information
- Separate audit logs for compliance requirements

### 2. **Log Levels Usage**
- **Debug**: Detailed diagnostic information
- **Information**: General application flow
- **Warning**: Unexpected situations that don't stop execution
- **Error**: Errors and exceptions that need attention
- **Critical**: Serious failures requiring immediate action

### 3. **Performance Considerations**
- Use structured logging for better performance
- Avoid string interpolation in log messages
- Implement log filtering to reduce I/O overhead
- Use async logging where appropriate

### 4. **Correlation and Context**
- Include correlation IDs for request tracking
- Use LogContext for automatic property inclusion
- Enrich logs with relevant business context

### 5. **Error Handling**
- Log exceptions with full stack traces
- Include contextual information with errors
- Differentiate between business and technical errors

## Configuration Details

### appsettings.json Structure

The `appsettings.json` file demonstrates:

- **Multiple Sinks**: Console, file, audit, error, and performance logs
- **Log Filtering**: Separate logs based on properties and levels
- **Rolling Policies**: Daily rolling with size limits
- **Output Templates**: Customized formats for different purposes
- **Enrichers**: Automatic addition of machine, process, and thread information

### Key Configuration Features

1. **Separate Audit Logs**: Financial and security events
2. **Error Isolation**: Warnings and errors in separate files
3. **Performance Tracking**: Dedicated performance monitoring logs
4. **Log Retention**: Configurable retention policies
5. **Environment-Specific Settings**: Different configs for dev/prod

## Testing the Application

### Sample HTTP Requests

You can test the logging functionality using the following curl commands:

```bash
# Create a user
curl -X POST "https://localhost:7216/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","firstName":"Test","lastName":"User"}'

# User login
curl -X POST "https://localhost:7216/api/users/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"password123"}'

# Create an order
curl -X POST "https://localhost:7216/api/orders" \
  -H "Content-Type: application/json" \
  -d '{"userId":1,"items":[{"productName":"Test Product","quantity":2,"unitPrice":25.50}],"shippingAddress":"123 Test St"}'

# Process payment
curl -X POST "https://localhost:7216/api/payment/process" \
  -H "Content-Type: application/json" \
  -d '{"orderId":1,"method":1,"amount":51.00}'
```

### What to Look For

After making these requests:

1. Check the console output for real-time logs
2. Examine the log files in the `logs/` directory
3. Notice how correlation IDs track requests across services
4. Observe different log levels and contexts
5. See how audit events are separately logged

## Learning Objectives

By studying this project, you will learn:

1. **How to configure Serilog** for production use
2. **Structured logging best practices** in .NET Core
3. **Multiple sink configurations** for different log types
4. **Request correlation** and context enrichment
5. **Performance monitoring** through logging
6. **Security and audit trail** implementation
7. **Error handling and incident logging**
8. **Log filtering and organization** strategies

## Next Steps

To extend this project for learning:

1. **Add Database Logging**: Implement Entity Framework logging
2. **Add External Sinks**: Try Elasticsearch, Application Insights, or Seq
3. **Implement Log Aggregation**: Set up centralized logging
4. **Add Metrics**: Integrate with monitoring systems
5. **Health Checks**: Add logging for health check endpoints
6. **Background Services**: Add logging for background tasks

---

**Happy Logging!** This project demonstrates production-ready logging patterns that you can adapt for your own applications. Remember: good logging is essential for maintainable, observable applications.
