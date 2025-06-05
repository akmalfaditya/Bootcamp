# Structured Logging Demo - ASP.NET Core Web API

A comprehensive demonstration of structured logging techniques using **Serilog** and **NLog** in an ASP.NET Core Web API project. This project showcases best practices for implementing structured logging in enterprise applications.

## Project Overview

This demo project implements a complete e-commerce API system with user management and order processing, specifically designed to demonstrate various structured logging concepts and patterns.

### Key Features Demonstrated

- **User Authentication System** - Login, user creation, and retrieval with security event logging
- **Order Management System** - Order creation, payment processing, and shipping with business operation logging
- **Comprehensive Logging Patterns** - Performance monitoring, error handling, and audit trails
- **Multiple Logging Frameworks** - Both Serilog and NLog configurations for comparison

## Architecture

```
StructuredLogging.Demo/
├── Controllers/          # API controllers with HTTP context logging
│   ├── UserController.cs     # User management endpoints
│   └── OrderController.cs    # Order processing endpoints
├── Models/              # Domain models and DTOs
│   ├── User.cs              # User entity and authentication models
│   ├── Order.cs             # Order entity and related models
│   └── RequestModels.cs     # API request/response models
├── Services/            # Business logic layer with structured logging
│   ├── IServices.cs         # Service interfaces
│   ├── UserService.cs       # User business operations
│   └── OrderService.cs      # Order business operations
├── logs/                # Generated log files (created at runtime)
├── NLog.config          # NLog configuration with multiple targets
├── Program.cs           # Application startup with Serilog configuration
└── README.md           # This documentation
```

## Structured Logging Concepts Demonstrated

### 1. **Security Event Logging**
```csharp
// Example from UserService.cs
_logger.LogInformation("User login attempt for {Username} from {ClientIP} at {Timestamp}",
    username, clientIp, DateTime.UtcNow);
```

### 2. **Performance Monitoring**
```csharp
// Example from OrderController.cs
var stopwatch = Stopwatch.StartNew();
// ... business logic ...
_logger.LogInformation("Order creation completed in {ElapsedMilliseconds}ms for {UserId}",
    stopwatch.ElapsedMilliseconds, request.UserId);
```

### 3. **Business Operation Tracking**
```csharp
// Example from OrderService.cs
_logger.LogInformation("Order {OrderId} created successfully for user {UserId} with total amount {TotalAmount:C}",
    order.Id, order.UserId, order.TotalAmount);
```

### 4. **Error Context Preservation**
```csharp
// Example error logging with full context
_logger.LogError(ex, "Payment processing failed for order {OrderId} with amount {Amount:C}. Correlation ID: {CorrelationId}",
    orderId, amount, correlationId);
```

### 5. **Data Access Logging**
```csharp
// Example from services showing data operations
_logger.LogDebug("Querying users with parameters: Skip={Skip}, Take={Take}, Filter={Filter}",
    skip, take, filter);
```

## Technology Stack

- **Framework**: ASP.NET Core 8.0 Web API
- **Logging Libraries**:
  - Serilog 4.3.0 with Console and File sinks
  - NLog 5.5.0 with Web extensions
- **Output Formats**: Console, Text Files, JSON Files
- **Development Tools**: Swagger/OpenAPI integration

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Git (for cloning)

### Installation Steps

1. **Clone or navigate to the project**:
   ```bash
   cd "c:\Users\Formulatrix\Documents\Bootcamp\Practice\Advanced Reading\Structured Logging\StructuredLogging.Demo"
   ```

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
   - Swagger UI: `https://localhost:7000/swagger`
   - API Base URL: `https://localhost:7000`

### Configuration

#### Serilog Configuration (Program.cs)
- **Console Sink**: Structured output for development
- **File Sink**: Rolling daily logs with detailed templates
- **JSON Sink**: Machine-readable logs for analysis tools
- **Context Enrichment**: Machine name, process ID, thread ID

#### NLog Configuration (NLog.config)
- **Multiple Targets**: Console, file, JSON, error-specific, performance-specific
- **ASP.NET Core Integration**: Request context, user agent, client IP
- **Flexible Rules**: Different log levels for different scenarios
- **Archive Management**: Automatic log rotation and cleanup

## API Endpoints

### User Management
- `POST /api/users/login` - User authentication
- `POST /api/users` - Create new user
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users` - List users with pagination

### Order Management
- `POST /api/orders` - Create new order
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/user/{userId}` - Get user's orders
- `POST /api/orders/{id}/pay` - Process payment
- `POST /api/orders/{id}/ship` - Process shipping

## Log Analysis Examples

### Viewing Structured Logs

1. **Console Output** (Development):
   ```
   [2024-01-15 10:30:45.123 +00:00] [INF] [UserService] User login successful for {Username="john.doe"} from {ClientIP="192.168.1.100"} {CorrelationId="abc-123-def"}
   ```

2. **JSON Output** (Production Analysis):
   ```json
   {
     "timestamp": "2024-01-15 10:30:45.123",
     "level": "INFO",
     "message": "Order created successfully",
     "properties": {
       "OrderId": "12345",
       "UserId": "67890",
       "TotalAmount": 150.75,
       "CorrelationId": "abc-123-def"
     }
   }
   ```

### Log File Locations
- **Serilog Logs**: `logs/application-YYYY-MM-DD.log`
- **Serilog JSON**: `logs/application-json-YYYY-MM-DD.log`
- **NLog Standard**: `logs/nlog-YYYY-MM-DD.log`
- **NLog JSON**: `logs/nlog-json-YYYY-MM-DD.log`
- **Error Logs**: `logs/nlog-errors-YYYY-MM-DD.log`
- **Performance Logs**: `logs/nlog-performance-YYYY-MM-DD.log`

## Best Practices Demonstrated

### 1. **Structured Properties**
- Use meaningful property names: `{Username}`, `{OrderId}`, `{CorrelationId}`
- Include context: timestamps, user IDs, request IDs
- Preserve data types: amounts as decimals, dates as DateTime

### 2. **Log Levels Usage**
- **Trace**: Detailed flow information
- **Debug**: Diagnostic information for development
- **Information**: General application flow
- **Warning**: Unexpected situations that don't stop execution
- **Error**: Error events that don't stop application
- **Critical**: Critical errors that may cause application termination

### 3. **Performance Monitoring**
- Use `Stopwatch` for accurate timing
- Log performance metrics as structured data
- Include operation context and parameters

### 4. **Security Considerations**
- Never log sensitive data (passwords, credit cards)
- Use placeholders for PII when necessary
- Include security context (IP addresses, user agents)

### 5. **Context Preservation**
- Correlation IDs for request tracing
- User context across operations
- Error context with full exception details

## 📚 Additional Resources

- [Serilog Documentation](https://serilog.net/)
- [NLog Documentation](https://nlog-project.org/)
- [ASP.NET Core Logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/)
- [Structured Logging Best Practices](https://stackify.com/what-is-structured-logging-and-why-developers-need-it/)


*This project demonstrates comprehensive structured logging patterns for enterprise ASP.NET Core applications. Each log entry tells a story with context, making debugging and monitoring much more effective.*
