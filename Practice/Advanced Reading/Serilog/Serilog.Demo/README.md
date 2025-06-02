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
