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

## Creating the Project from Scratch

Follow these steps to build the Structured Logging Demo project from the ground up:

### Step 1: Create Project Structure

1. **Create the solution directory and navigate to it**
   ```powershell
   mkdir StructuredLogging.Demo
   cd StructuredLogging.Demo
   ```

2. **Create a new Web API project**
   ```powershell
   dotnet new webapi --name StructuredLogging.Demo --framework net8.0
   cd StructuredLogging.Demo
   ```

3. **Install required NuGet packages**
   ```powershell
   # Serilog packages
   dotnet add package Serilog.AspNetCore --version 8.0.0
   dotnet add package Serilog.Sinks.Console --version 5.0.1
   dotnet add package Serilog.Sinks.File --version 5.0.0
   dotnet add package Serilog.Enrichers.Environment --version 2.3.0
   dotnet add package Serilog.Enrichers.Process --version 2.0.2
   dotnet add package Serilog.Enrichers.Thread --version 3.1.0
   
   # NLog packages
   dotnet add package NLog --version 5.5.0
   dotnet add package NLog.Web.AspNetCore --version 5.5.0
   
   # Additional packages for enhanced functionality
   dotnet add package Microsoft.Extensions.Logging --version 8.0.0
   ```

### Step 2: Create Domain Models

1. **Create the Models directory**
   ```powershell
   mkdir Models
   ```

2. **Create User.cs with user-related models**
   ```csharp
   namespace StructuredLogging.Demo.Models
   {
       public class User
       {
           public int Id { get; set; }
           public string Username { get; set; } = string.Empty;
           public string Email { get; set; } = string.Empty;
           public string FirstName { get; set; } = string.Empty;
           public string LastName { get; set; } = string.Empty;
           public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
           public bool IsActive { get; set; } = true;
       }

       public class LoginRequest
       {
           public string Username { get; set; } = string.Empty;
           public string Password { get; set; } = string.Empty;
       }

       public class LoginResponse
       {
           public bool Success { get; set; }
           public string Message { get; set; } = string.Empty;
           public User? User { get; set; }
           public string? Token { get; set; }
       }

       public class CreateUserRequest
       {
           public string Username { get; set; } = string.Empty;
           public string Email { get; set; } = string.Empty;
           public string FirstName { get; set; } = string.Empty;
           public string LastName { get; set; } = string.Empty;
           public string Password { get; set; } = string.Empty;
       }
   }
   ```

3. **Create Order.cs with order-related models**
   ```csharp
   namespace StructuredLogging.Demo.Models
   {
       public class Order
       {
           public int Id { get; set; }
           public int UserId { get; set; }
           public decimal TotalAmount { get; set; }
           public OrderStatus Status { get; set; } = OrderStatus.Pending;
           public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
           public DateTime? PaidAt { get; set; }
           public DateTime? ShippedAt { get; set; }
           public List<OrderItem> Items { get; set; } = new List<OrderItem>();
       }

       public class OrderItem
       {
           public int Id { get; set; }
           public string ProductName { get; set; } = string.Empty;
           public decimal Price { get; set; }
           public int Quantity { get; set; }
           public decimal Subtotal => Price * Quantity;
       }

       public enum OrderStatus
       {
           Pending,
           Paid,
           Shipped,
           Delivered,
           Cancelled
       }

       public class CreateOrderRequest
       {
           public int UserId { get; set; }
           public List<OrderItem> Items { get; set; } = new List<OrderItem>();
       }

       public class PaymentRequest
       {
           public string PaymentMethod { get; set; } = string.Empty;
           public string PaymentDetails { get; set; } = string.Empty;
       }

       public class ShippingRequest
       {
           public string ShippingAddress { get; set; } = string.Empty;
           public string ShippingMethod { get; set; } = string.Empty;
       }
   }
   ```

4. **Create RequestModels.cs for common API models**
   ```csharp
   namespace StructuredLogging.Demo.Models
   {
       public class ApiResponse<T>
       {
           public bool Success { get; set; }
           public string Message { get; set; } = string.Empty;
           public T? Data { get; set; }
           public string? CorrelationId { get; set; }
       }

       public class PaginatedRequest
       {
           public int Skip { get; set; } = 0;
           public int Take { get; set; } = 10;
           public string Filter { get; set; } = string.Empty;
       }

       public class PaginatedResponse<T>
       {
           public List<T> Items { get; set; } = new List<T>();
           public int TotalCount { get; set; }
           public int Skip { get; set; }
           public int Take { get; set; }
       }
   }
   ```

### Step 3: Create Service Interfaces

1. **Create the Services directory**
   ```powershell
   mkdir Services
   ```

2. **Create IServices.cs with service contracts**
   ```csharp
   using StructuredLogging.Demo.Models;

   namespace StructuredLogging.Demo.Services
   {
       public interface IUserService
       {
           Task<LoginResponse> LoginAsync(LoginRequest request, string clientIp);
           Task<User> CreateUserAsync(CreateUserRequest request);
           Task<User?> GetUserByIdAsync(int id);
           Task<PaginatedResponse<User>> GetUsersAsync(PaginatedRequest request);
       }

       public interface IOrderService
       {
           Task<Order> CreateOrderAsync(CreateOrderRequest request);
           Task<Order?> GetOrderByIdAsync(int id);
           Task<PaginatedResponse<Order>> GetOrdersByUserIdAsync(int userId, PaginatedRequest request);
           Task<bool> ProcessPaymentAsync(int orderId, PaymentRequest request);
           Task<bool> ProcessShippingAsync(int orderId, ShippingRequest request);
       }
   }
   ```

### Step 4: Implement Service Classes

1. **Create UserService.cs with comprehensive logging**
   ```csharp
   using StructuredLogging.Demo.Models;
   using System.Diagnostics;

   namespace StructuredLogging.Demo.Services
   {
       public class UserService : IUserService
       {
           private readonly ILogger<UserService> _logger;
           private static readonly List<User> _users = new List<User>();
           private static int _nextId = 1;

           public UserService(ILogger<UserService> logger)
           {
               _logger = logger;
           }

           public async Task<LoginResponse> LoginAsync(LoginRequest request, string clientIp)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("User login attempt for {Username} from {ClientIP} at {Timestamp}. Correlation ID: {CorrelationId}",
                   request.Username, clientIp, DateTime.UtcNow, correlationId);

               try
               {
                   // Simulate async operation
                   await Task.Delay(100);

                   var user = _users.FirstOrDefault(u => u.Username == request.Username && u.IsActive);
                   
                   if (user == null)
                   {
                       _logger.LogWarning("Login failed for {Username} from {ClientIP} - User not found. Correlation ID: {CorrelationId}",
                           request.Username, clientIp, correlationId);
                       
                       return new LoginResponse
                       {
                           Success = false,
                           Message = "Invalid username or password"
                       };
                   }

                   // Simulate password validation (never log actual passwords)
                   if (request.Password != "password123")
                   {
                       _logger.LogWarning("Login failed for {Username} from {ClientIP} - Invalid password. Correlation ID: {CorrelationId}",
                           request.Username, clientIp, correlationId);
                       
                       return new LoginResponse
                       {
                           Success = false,
                           Message = "Invalid username or password"
                       };
                   }

                   stopwatch.Stop();
                   
                   _logger.LogInformation("User login successful for {Username} from {ClientIP} in {ElapsedMilliseconds}ms. User ID: {UserId}, Correlation ID: {CorrelationId}",
                       request.Username, clientIp, stopwatch.ElapsedMilliseconds, user.Id, correlationId);

                   return new LoginResponse
                   {
                       Success = true,
                       Message = "Login successful",
                       User = user,
                       Token = $"mock-token-{user.Id}-{correlationId}"
                   };
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Unexpected error during login for {Username} from {ClientIP}. Correlation ID: {CorrelationId}",
                       request.Username, clientIp, correlationId);
                   
                   return new LoginResponse
                   {
                       Success = false,
                       Message = "An error occurred during login"
                   };
               }
           }

           public async Task<User> CreateUserAsync(CreateUserRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("Creating new user with username {Username} and email {Email}. Correlation ID: {CorrelationId}",
                   request.Username, request.Email, correlationId);

               try
               {
                   // Simulate async operation
                   await Task.Delay(200);

                   // Check for existing user
                   if (_users.Any(u => u.Username == request.Username))
                   {
                       _logger.LogWarning("User creation failed - Username {Username} already exists. Correlation ID: {CorrelationId}",
                           request.Username, correlationId);
                       throw new InvalidOperationException("Username already exists");
                   }

                   if (_users.Any(u => u.Email == request.Email))
                   {
                       _logger.LogWarning("User creation failed - Email {Email} already exists. Correlation ID: {CorrelationId}",
                           request.Email, correlationId);
                       throw new InvalidOperationException("Email already exists");
                   }

                   var user = new User
                   {
                       Id = _nextId++,
                       Username = request.Username,
                       Email = request.Email,
                       FirstName = request.FirstName,
                       LastName = request.LastName,
                       CreatedAt = DateTime.UtcNow,
                       IsActive = true
                   };

                   _users.Add(user);
                   stopwatch.Stop();

                   _logger.LogInformation("User created successfully with ID {UserId} for username {Username} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                       user.Id, user.Username, stopwatch.ElapsedMilliseconds, correlationId);

                   return user;
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error creating user with username {Username}. Correlation ID: {CorrelationId}",
                       request.Username, correlationId);
                   throw;
               }
           }

           public async Task<User?> GetUserByIdAsync(int id)
           {
               _logger.LogDebug("Retrieving user with ID {UserId}", id);

               try
               {
                   await Task.Delay(50); // Simulate async operation
                   
                   var user = _users.FirstOrDefault(u => u.Id == id);
                   
                   if (user == null)
                   {
                       _logger.LogWarning("User with ID {UserId} not found", id);
                   }
                   else
                   {
                       _logger.LogDebug("User with ID {UserId} retrieved successfully. Username: {Username}",
                           id, user.Username);
                   }

                   return user;
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
                   throw;
               }
           }

           public async Task<PaginatedResponse<User>> GetUsersAsync(PaginatedRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               
               _logger.LogDebug("Querying users with parameters: Skip={Skip}, Take={Take}, Filter={Filter}. Correlation ID: {CorrelationId}",
                   request.Skip, request.Take, request.Filter, correlationId);

               try
               {
                   await Task.Delay(100); // Simulate async operation

                   var filteredUsers = _users.AsQueryable();

                   if (!string.IsNullOrEmpty(request.Filter))
                   {
                       filteredUsers = filteredUsers.Where(u => 
                           u.Username.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                           u.Email.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                           u.FirstName.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                           u.LastName.Contains(request.Filter, StringComparison.OrdinalIgnoreCase));
                   }

                   var totalCount = filteredUsers.Count();
                   var users = filteredUsers.Skip(request.Skip).Take(request.Take).ToList();

                   _logger.LogInformation("Users query completed: {TotalCount} total users, {ReturnedCount} returned. Correlation ID: {CorrelationId}",
                       totalCount, users.Count, correlationId);

                   return new PaginatedResponse<User>
                   {
                       Items = users,
                       TotalCount = totalCount,
                       Skip = request.Skip,
                       Take = request.Take
                   };
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error querying users with filter {Filter}. Correlation ID: {CorrelationId}",
                       request.Filter, correlationId);
                   throw;
               }
           }
       }
   }
   ```

2. **Create OrderService.cs with business operation logging**
   ```csharp
   using StructuredLogging.Demo.Models;
   using System.Diagnostics;

   namespace StructuredLogging.Demo.Services
   {
       public class OrderService : IOrderService
       {
           private readonly ILogger<OrderService> _logger;
           private readonly IUserService _userService;
           private static readonly List<Order> _orders = new List<Order>();
           private static int _nextId = 1;

           public OrderService(ILogger<OrderService> logger, IUserService userService)
           {
               _logger = logger;
               _userService = userService;
           }

           public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("Creating order for user {UserId} with {ItemCount} items. Correlation ID: {CorrelationId}",
                   request.UserId, request.Items.Count, correlationId);

               try
               {
                   // Validate user exists
                   var user = await _userService.GetUserByIdAsync(request.UserId);
                   if (user == null)
                   {
                       _logger.LogWarning("Order creation failed - User {UserId} not found. Correlation ID: {CorrelationId}",
                           request.UserId, correlationId);
                       throw new InvalidOperationException("User not found");
                   }

                   // Validate items
                   if (!request.Items.Any())
                   {
                       _logger.LogWarning("Order creation failed - No items provided for user {UserId}. Correlation ID: {CorrelationId}",
                           request.UserId, correlationId);
                       throw new ArgumentException("Order must contain at least one item");
                   }

                   var totalAmount = request.Items.Sum(item => item.Subtotal);

                   var order = new Order
                   {
                       Id = _nextId++,
                       UserId = request.UserId,
                       TotalAmount = totalAmount,
                       Status = OrderStatus.Pending,
                       CreatedAt = DateTime.UtcNow,
                       Items = request.Items.Select(item => new OrderItem
                       {
                           Id = item.Id,
                           ProductName = item.ProductName,
                           Price = item.Price,
                           Quantity = item.Quantity
                       }).ToList()
                   };

                   _orders.Add(order);
                   stopwatch.Stop();

                   _logger.LogInformation("Order {OrderId} created successfully for user {UserId} with total amount {TotalAmount:C} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                       order.Id, order.UserId, order.TotalAmount, stopwatch.ElapsedMilliseconds, correlationId);

                   // Log individual items for audit trail
                   foreach (var item in order.Items)
                   {
                       _logger.LogDebug("Order {OrderId} contains item: {ProductName} x{Quantity} at {Price:C} each. Correlation ID: {CorrelationId}",
                           order.Id, item.ProductName, item.Quantity, item.Price, correlationId);
                   }

                   return order;
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error creating order for user {UserId}. Correlation ID: {CorrelationId}",
                       request.UserId, correlationId);
                   throw;
               }
           }

           public async Task<Order?> GetOrderByIdAsync(int id)
           {
               _logger.LogDebug("Retrieving order with ID {OrderId}", id);

               try
               {
                   await Task.Delay(50); // Simulate async operation
                   
                   var order = _orders.FirstOrDefault(o => o.Id == id);
                   
                   if (order == null)
                   {
                       _logger.LogWarning("Order with ID {OrderId} not found", id);
                   }
                   else
                   {
                       _logger.LogDebug("Order {OrderId} retrieved successfully. Status: {OrderStatus}, Amount: {TotalAmount:C}",
                           id, order.Status, order.TotalAmount);
                   }

                   return order;
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error retrieving order with ID {OrderId}", id);
                   throw;
               }
           }

           public async Task<PaginatedResponse<Order>> GetOrdersByUserIdAsync(int userId, PaginatedRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               
               _logger.LogDebug("Querying orders for user {UserId} with parameters: Skip={Skip}, Take={Take}. Correlation ID: {CorrelationId}",
                   userId, request.Skip, request.Take, correlationId);

               try
               {
                   await Task.Delay(100); // Simulate async operation

                   var userOrders = _orders.Where(o => o.UserId == userId).ToList();
                   var totalCount = userOrders.Count;
                   var orders = userOrders.Skip(request.Skip).Take(request.Take).ToList();

                   _logger.LogInformation("Orders query completed for user {UserId}: {TotalCount} total orders, {ReturnedCount} returned. Correlation ID: {CorrelationId}",
                       userId, totalCount, orders.Count, correlationId);

                   return new PaginatedResponse<Order>
                   {
                       Items = orders,
                       TotalCount = totalCount,
                       Skip = request.Skip,
                       Take = request.Take
                   };
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error querying orders for user {UserId}. Correlation ID: {CorrelationId}",
                       userId, correlationId);
                   throw;
               }
           }

           public async Task<bool> ProcessPaymentAsync(int orderId, PaymentRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("Processing payment for order {OrderId} using method {PaymentMethod}. Correlation ID: {CorrelationId}",
                   orderId, request.PaymentMethod, correlationId);

               try
               {
                   var order = _orders.FirstOrDefault(o => o.Id == orderId);
                   if (order == null)
                   {
                       _logger.LogWarning("Payment processing failed - Order {OrderId} not found. Correlation ID: {CorrelationId}",
                           orderId, correlationId);
                       return false;
                   }

                   if (order.Status != OrderStatus.Pending)
                   {
                       _logger.LogWarning("Payment processing failed - Order {OrderId} is not in pending status. Current status: {OrderStatus}. Correlation ID: {CorrelationId}",
                           orderId, order.Status, correlationId);
                       return false;
                   }

                   // Simulate payment processing
                   await Task.Delay(500);

                   // Simulate random payment failure (10% chance)
                   var random = new Random();
                   if (random.Next(1, 11) == 1)
                   {
                       _logger.LogWarning("Payment processing failed for order {OrderId} with amount {Amount:C} - Simulated payment gateway error. Correlation ID: {CorrelationId}",
                           orderId, order.TotalAmount, correlationId);
                       return false;
                   }

                   order.Status = OrderStatus.Paid;
                   order.PaidAt = DateTime.UtcNow;
                   stopwatch.Stop();

                   _logger.LogInformation("Payment processed successfully for order {OrderId} with amount {Amount:C} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                       orderId, order.TotalAmount, stopwatch.ElapsedMilliseconds, correlationId);

                   return true;
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing payment for order {OrderId} with amount {Amount:C}. Correlation ID: {CorrelationId}",
                       orderId, request, correlationId);
                   throw;
               }
           }

           public async Task<bool> ProcessShippingAsync(int orderId, ShippingRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("Processing shipping for order {OrderId} to address {ShippingAddress} via {ShippingMethod}. Correlation ID: {CorrelationId}",
                   orderId, request.ShippingAddress, request.ShippingMethod, correlationId);

               try
               {
                   var order = _orders.FirstOrDefault(o => o.Id == orderId);
                   if (order == null)
                   {
                       _logger.LogWarning("Shipping processing failed - Order {OrderId} not found. Correlation ID: {CorrelationId}",
                           orderId, correlationId);
                       return false;
                   }

                   if (order.Status != OrderStatus.Paid)
                   {
                       _logger.LogWarning("Shipping processing failed - Order {OrderId} is not paid. Current status: {OrderStatus}. Correlation ID: {CorrelationId}",
                           orderId, order.Status, correlationId);
                       return false;
                   }

                   // Simulate shipping processing
                   await Task.Delay(300);

                   order.Status = OrderStatus.Shipped;
                   order.ShippedAt = DateTime.UtcNow;
                   stopwatch.Stop();

                   _logger.LogInformation("Shipping processed successfully for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                       orderId, stopwatch.ElapsedMilliseconds, correlationId);

                   return true;
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing shipping for order {OrderId}. Correlation ID: {CorrelationId}",
                       orderId, correlationId);
                   throw;
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

2. **Create UserController.cs with HTTP context logging**
   ```csharp
   using Microsoft.AspNetCore.Mvc;
   using StructuredLogging.Demo.Models;
   using StructuredLogging.Demo.Services;
   using System.Diagnostics;

   namespace StructuredLogging.Demo.Controllers
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
           public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();
               var clientIp = GetClientIpAddress();

               _logger.LogInformation("Login request received for username {Username} from {ClientIP}. Correlation ID: {CorrelationId}",
                   request.Username, clientIp, correlationId);

               try
               {
                   var result = await _userService.LoginAsync(request, clientIp);
                   stopwatch.Stop();

                   var response = new ApiResponse<LoginResponse>
                   {
                       Success = result.Success,
                       Message = result.Message,
                       Data = result,
                       CorrelationId = correlationId
                   };

                   if (result.Success)
                   {
                       _logger.LogInformation("Login request completed successfully for username {Username} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                           request.Username, stopwatch.ElapsedMilliseconds, correlationId);
                       return Ok(response);
                   }
                   else
                   {
                       _logger.LogWarning("Login request failed for username {Username} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                           request.Username, stopwatch.ElapsedMilliseconds, correlationId);
                       return BadRequest(response);
                   }
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing login request for username {Username}. Correlation ID: {CorrelationId}",
                       request.Username, correlationId);

                   return StatusCode(500, new ApiResponse<LoginResponse>
                   {
                       Success = false,
                       Message = "An error occurred while processing the login request",
                       CorrelationId = correlationId
                   });
               }
           }

           [HttpPost]
           public async Task<ActionResult<ApiResponse<User>>> CreateUser([FromBody] CreateUserRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("Create user request received for username {Username}. Correlation ID: {CorrelationId}",
                   request.Username, correlationId);

               try
               {
                   var user = await _userService.CreateUserAsync(request);
                   stopwatch.Stop();

                   _logger.LogInformation("Create user request completed successfully for username {Username} in {ElapsedMilliseconds}ms. User ID: {UserId}, Correlation ID: {CorrelationId}",
                       request.Username, stopwatch.ElapsedMilliseconds, user.Id, correlationId);

                   var response = new ApiResponse<User>
                   {
                       Success = true,
                       Message = "User created successfully",
                       Data = user,
                       CorrelationId = correlationId
                   };

                   return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
               }
               catch (InvalidOperationException ex)
               {
                   _logger.LogWarning(ex, "Create user request failed for username {Username} - {ErrorMessage}. Correlation ID: {CorrelationId}",
                       request.Username, ex.Message, correlationId);

                   return BadRequest(new ApiResponse<User>
                   {
                       Success = false,
                       Message = ex.Message,
                       CorrelationId = correlationId
                   });
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing create user request for username {Username}. Correlation ID: {CorrelationId}",
                       request.Username, correlationId);

                   return StatusCode(500, new ApiResponse<User>
                   {
                       Success = false,
                       Message = "An error occurred while creating the user",
                       CorrelationId = correlationId
                   });
               }
           }

           [HttpGet("{id}")]
           public async Task<ActionResult<ApiResponse<User>>> GetUser(int id)
           {
               var correlationId = Guid.NewGuid().ToString();

               _logger.LogDebug("Get user request received for ID {UserId}. Correlation ID: {CorrelationId}",
                   id, correlationId);

               try
               {
                   var user = await _userService.GetUserByIdAsync(id);

                   if (user == null)
                   {
                       _logger.LogWarning("Get user request failed - User {UserId} not found. Correlation ID: {CorrelationId}",
                           id, correlationId);

                       return NotFound(new ApiResponse<User>
                       {
                           Success = false,
                           Message = "User not found",
                           CorrelationId = correlationId
                       });
                   }

                   _logger.LogDebug("Get user request completed successfully for ID {UserId}. Correlation ID: {CorrelationId}",
                       id, correlationId);

                   return Ok(new ApiResponse<User>
                   {
                       Success = true,
                       Message = "User retrieved successfully",
                       Data = user,
                       CorrelationId = correlationId
                   });
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing get user request for ID {UserId}. Correlation ID: {CorrelationId}",
                       id, correlationId);

                   return StatusCode(500, new ApiResponse<User>
                   {
                       Success = false,
                       Message = "An error occurred while retrieving the user",
                       CorrelationId = correlationId
                   });
               }
           }

           [HttpGet]
           public async Task<ActionResult<ApiResponse<PaginatedResponse<User>>>> GetUsers([FromQuery] PaginatedRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();

               _logger.LogDebug("Get users request received with parameters: Skip={Skip}, Take={Take}, Filter={Filter}. Correlation ID: {CorrelationId}",
                   request.Skip, request.Take, request.Filter, correlationId);

               try
               {
                   var result = await _userService.GetUsersAsync(request);

                   _logger.LogDebug("Get users request completed successfully. Returned {Count} users. Correlation ID: {CorrelationId}",
                       result.Items.Count, correlationId);

                   return Ok(new ApiResponse<PaginatedResponse<User>>
                   {
                       Success = true,
                       Message = "Users retrieved successfully",
                       Data = result,
                       CorrelationId = correlationId
                   });
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing get users request. Correlation ID: {CorrelationId}",
                       correlationId);

                   return StatusCode(500, new ApiResponse<PaginatedResponse<User>>
                   {
                       Success = false,
                       Message = "An error occurred while retrieving users",
                       CorrelationId = correlationId
                   });
               }
           }

           private string GetClientIpAddress()
           {
               var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
               if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
                   ipAddress = "127.0.0.1";
               return ipAddress;
           }
       }
   }
   ```

3. **Create OrderController.cs with business operation logging**
   ```csharp
   using Microsoft.AspNetCore.Mvc;
   using StructuredLogging.Demo.Models;
   using StructuredLogging.Demo.Services;
   using System.Diagnostics;

   namespace StructuredLogging.Demo.Controllers
   {
       [ApiController]
       [Route("api/[controller]")]
       public class OrdersController : ControllerBase
       {
           private readonly ILogger<OrdersController> _logger;
           private readonly IOrderService _orderService;

           public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
           {
               _logger = logger;
               _orderService = orderService;
           }

           [HttpPost]
           public async Task<ActionResult<ApiResponse<Order>>> CreateOrder([FromBody] CreateOrderRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("Create order request received for user {UserId} with {ItemCount} items. Correlation ID: {CorrelationId}",
                   request.UserId, request.Items.Count, correlationId);

               try
               {
                   var order = await _orderService.CreateOrderAsync(request);
                   stopwatch.Stop();

                   _logger.LogInformation("Order creation completed in {ElapsedMilliseconds}ms for user {UserId}. Order ID: {OrderId}, Correlation ID: {CorrelationId}",
                       stopwatch.ElapsedMilliseconds, request.UserId, order.Id, correlationId);

                   var response = new ApiResponse<Order>
                   {
                       Success = true,
                       Message = "Order created successfully",
                       Data = order,
                       CorrelationId = correlationId
                   };

                   return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, response);
               }
               catch (InvalidOperationException ex)
               {
                   _logger.LogWarning(ex, "Create order request failed for user {UserId} - {ErrorMessage}. Correlation ID: {CorrelationId}",
                       request.UserId, ex.Message, correlationId);

                   return BadRequest(new ApiResponse<Order>
                   {
                       Success = false,
                       Message = ex.Message,
                       CorrelationId = correlationId
                   });
               }
               catch (ArgumentException ex)
               {
                   _logger.LogWarning(ex, "Create order request failed for user {UserId} - {ErrorMessage}. Correlation ID: {CorrelationId}",
                       request.UserId, ex.Message, correlationId);

                   return BadRequest(new ApiResponse<Order>
                   {
                       Success = false,
                       Message = ex.Message,
                       CorrelationId = correlationId
                   });
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing create order request for user {UserId}. Correlation ID: {CorrelationId}",
                       request.UserId, correlationId);

                   return StatusCode(500, new ApiResponse<Order>
                   {
                       Success = false,
                       Message = "An error occurred while creating the order",
                       CorrelationId = correlationId
                   });
               }
           }

           [HttpGet("{id}")]
           public async Task<ActionResult<ApiResponse<Order>>> GetOrder(int id)
           {
               var correlationId = Guid.NewGuid().ToString();

               _logger.LogDebug("Get order request received for ID {OrderId}. Correlation ID: {CorrelationId}",
                   id, correlationId);

               try
               {
                   var order = await _orderService.GetOrderByIdAsync(id);

                   if (order == null)
                   {
                       _logger.LogWarning("Get order request failed - Order {OrderId} not found. Correlation ID: {CorrelationId}",
                           id, correlationId);

                       return NotFound(new ApiResponse<Order>
                       {
                           Success = false,
                           Message = "Order not found",
                           CorrelationId = correlationId
                       });
                   }

                   _logger.LogDebug("Get order request completed successfully for ID {OrderId}. Correlation ID: {CorrelationId}",
                       id, correlationId);

                   return Ok(new ApiResponse<Order>
                   {
                       Success = true,
                       Message = "Order retrieved successfully",
                       Data = order,
                       CorrelationId = correlationId
                   });
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing get order request for ID {OrderId}. Correlation ID: {CorrelationId}",
                       id, correlationId);

                   return StatusCode(500, new ApiResponse<Order>
                   {
                       Success = false,
                       Message = "An error occurred while retrieving the order",
                       CorrelationId = correlationId
                   });
               }
           }

           [HttpGet("user/{userId}")]
           public async Task<ActionResult<ApiResponse<PaginatedResponse<Order>>>> GetOrdersByUser(int userId, [FromQuery] PaginatedRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();

               _logger.LogDebug("Get orders by user request received for user {UserId}. Correlation ID: {CorrelationId}",
                   userId, correlationId);

               try
               {
                   var result = await _orderService.GetOrdersByUserIdAsync(userId, request);

                   _logger.LogDebug("Get orders by user request completed successfully for user {UserId}. Returned {Count} orders. Correlation ID: {CorrelationId}",
                       userId, result.Items.Count, correlationId);

                   return Ok(new ApiResponse<PaginatedResponse<Order>>
                   {
                       Success = true,
                       Message = "Orders retrieved successfully",
                       Data = result,
                       CorrelationId = correlationId
                   });
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing get orders by user request for user {UserId}. Correlation ID: {CorrelationId}",
                       userId, correlationId);

                   return StatusCode(500, new ApiResponse<PaginatedResponse<Order>>
                   {
                       Success = false,
                       Message = "An error occurred while retrieving orders",
                       CorrelationId = correlationId
                   });
               }
           }

           [HttpPost("{id}/pay")]
           public async Task<ActionResult<ApiResponse<bool>>> ProcessPayment(int id, [FromBody] PaymentRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("Process payment request received for order {OrderId} using method {PaymentMethod}. Correlation ID: {CorrelationId}",
                   id, request.PaymentMethod, correlationId);

               try
               {
                   var result = await _orderService.ProcessPaymentAsync(id, request);
                   stopwatch.Stop();

                   if (result)
                   {
                       _logger.LogInformation("Payment processing completed successfully for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                           id, stopwatch.ElapsedMilliseconds, correlationId);

                       return Ok(new ApiResponse<bool>
                       {
                           Success = true,
                           Message = "Payment processed successfully",
                           Data = result,
                           CorrelationId = correlationId
                       });
                   }
                   else
                   {
                       _logger.LogWarning("Payment processing failed for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                           id, stopwatch.ElapsedMilliseconds, correlationId);

                       return BadRequest(new ApiResponse<bool>
                       {
                           Success = false,
                           Message = "Payment processing failed",
                           Data = result,
                           CorrelationId = correlationId
                       });
                   }
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing payment for order {OrderId}. Correlation ID: {CorrelationId}",
                       id, correlationId);

                   return StatusCode(500, new ApiResponse<bool>
                   {
                       Success = false,
                       Message = "An error occurred while processing payment",
                       CorrelationId = correlationId
                   });
               }
           }

           [HttpPost("{id}/ship")]
           public async Task<ActionResult<ApiResponse<bool>>> ProcessShipping(int id, [FromBody] ShippingRequest request)
           {
               var correlationId = Guid.NewGuid().ToString();
               var stopwatch = Stopwatch.StartNew();

               _logger.LogInformation("Process shipping request received for order {OrderId} to {ShippingAddress}. Correlation ID: {CorrelationId}",
                   id, request.ShippingAddress, correlationId);

               try
               {
                   var result = await _orderService.ProcessShippingAsync(id, request);
                   stopwatch.Stop();

                   if (result)
                   {
                       _logger.LogInformation("Shipping processing completed successfully for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                           id, stopwatch.ElapsedMilliseconds, correlationId);

                       return Ok(new ApiResponse<bool>
                       {
                           Success = true,
                           Message = "Shipping processed successfully",
                           Data = result,
                           CorrelationId = correlationId
                       });
                   }
                   else
                   {
                       _logger.LogWarning("Shipping processing failed for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                           id, stopwatch.ElapsedMilliseconds, correlationId);

                       return BadRequest(new ApiResponse<bool>
                       {
                           Success = false,
                           Message = "Shipping processing failed",
                           Data = result,
                           CorrelationId = correlationId
                       });
                   }
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "Error processing shipping for order {OrderId}. Correlation ID: {CorrelationId}",
                       id, correlationId);

                   return StatusCode(500, new ApiResponse<bool>
                   {
                       Success = false,
                       Message = "An error occurred while processing shipping",
                       CorrelationId = correlationId
                   });
               }
           }
       }
   }
   ```

### Step 6: Configure Logging Infrastructure

1. **Create NLog.config for NLog configuration**
   ```xml
   <?xml version="1.0" encoding="utf-8" ?>
   <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
         xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
         autoReload="true"
         internalLogLevel="Info"
         internalLogFile="logs/internal-nlog.txt">

     <!-- Enable ASP.NET Core layout renderers -->
     <extensions>
       <add assembly="NLog.Web.AspNetCore"/>
     </extensions>

     <!-- Define variables -->
     <variable name="logDirectory" value="logs" />

     <!-- Define targets -->
     <targets>
       <!-- Console target for development -->
       <target xsi:type="Console" name="console"
               layout="${longdate} [${uppercase:${level}}] [${logger}] ${message} ${exception:format=tostring}" />

       <!-- File target for general logging -->
       <target xsi:type="File" name="fileTarget"
               fileName="${logDirectory}/nlog-${shortdate}.log"
               layout="${longdate} [${uppercase:${level}}] [${logger}] ${message} ${exception:format=tostring}"
               archiveFileName="${logDirectory}/archive/nlog-{#}.log"
               archiveEvery="Day"
               archiveNumbering="Rolling"
               maxArchiveFiles="30" />

       <!-- JSON file target for structured logging -->
       <target xsi:type="File" name="jsonFileTarget"
               fileName="${logDirectory}/nlog-json-${shortdate}.log"
               archiveFileName="${logDirectory}/archive/nlog-json-{#}.log"
               archiveEvery="Day"
               archiveNumbering="Rolling"
               maxArchiveFiles="30">
         <layout xsi:type="JsonLayout" includeAllProperties="true">
           <attribute name="timestamp" layout="${longdate}" />
           <attribute name="level" layout="${level:upperCase=true}" />
           <attribute name="logger" layout="${logger}" />
           <attribute name="message" layout="${message}" />
           <attribute name="exception" layout="${exception:format=tostring}" />
           <attribute name="url" layout="${aspnet-request-url}" />
           <attribute name="action" layout="${aspnet-mvc-action}" />
           <attribute name="controller" layout="${aspnet-mvc-controller}" />
           <attribute name="userAgent" layout="${aspnet-request-useragent}" />
           <attribute name="clientIP" layout="${aspnet-request-ip}" />
           <attribute name="requestId" layout="${aspnet-TraceIdentifier}" />
         </layout>
       </target>

       <!-- Error-specific target -->
       <target xsi:type="File" name="errorFileTarget"
               fileName="${logDirectory}/nlog-errors-${shortdate}.log"
               layout="${longdate} [${uppercase:${level}}] [${logger}] ${message} ${exception:format=tostring}"
               archiveFileName="${logDirectory}/archive/nlog-errors-{#}.log"
               archiveEvery="Day"
               archiveNumbering="Rolling"
               maxArchiveFiles="30" />

       <!-- Performance-specific target -->
       <target xsi:type="File" name="performanceFileTarget"
               fileName="${logDirectory}/nlog-performance-${shortdate}.log"
               layout="${longdate} [${logger}] ${message}"
               archiveFileName="${logDirectory}/archive/nlog-performance-{#}.log"
               archiveEvery="Day"
               archiveNumbering="Rolling"
               maxArchiveFiles="30" />
     </targets>

     <!-- Define rules -->
     <rules>
       <!-- Performance logging -->
       <logger name="*" minlevel="Info" writeTo="performanceFileTarget">
         <filters>
           <when condition="contains('${message}', 'ElapsedMilliseconds')" action="Log" />
           <when condition="true" action="Ignore" />
         </filters>
       </logger>

       <!-- Error logging -->
       <logger name="*" minlevel="Error" writeTo="errorFileTarget" />

       <!-- General logging -->
       <logger name="*" minlevel="Debug" writeTo="console,fileTarget,jsonFileTarget" />

       <!-- Skip non-critical Microsoft logs and only log warnings and errors -->
       <logger name="Microsoft.*" maxlevel="Info" final="true" />
       <logger name="System.Net.Http.*" maxlevel="Info" final="true" />
     </rules>
   </nlog>
   ```

2. **Update Program.cs with comprehensive Serilog and NLog configuration**
   ```csharp
   using Serilog;
   using Serilog.Events;
   using NLog;
   using NLog.Web;
   using StructuredLogging.Demo.Services;

   // Create early logger for startup errors
   Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
       .Enrich.FromLogContext()
       .WriteTo.Console()
       .CreateBootstrapLogger();

   try
   {
       Log.Information("Starting web application");

       var builder = WebApplication.CreateBuilder(args);

       // Configure Serilog
       builder.Host.UseSerilog((context, services, configuration) =>
       {
           configuration
               .ReadFrom.Configuration(context.Configuration)
               .ReadFrom.Services(services)
               .Enrich.FromLogContext()
               .Enrich.WithMachineName()
               .Enrich.WithProcessId()
               .Enrich.WithThreadId()
               .WriteTo.Console(
                   outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
               .WriteTo.File(
                   path: "logs/application-.log",
                   rollingInterval: RollingInterval.Day,
                   outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                   retainedFileCountLimit: 30)
               .WriteTo.File(
                   path: "logs/application-json-.log",
                   rollingInterval: RollingInterval.Day,
                   formatter: new Serilog.Formatting.Compact.CompactJsonFormatter(),
                   retainedFileCountLimit: 30);
       });

       // Configure NLog
       builder.Logging.ClearProviders();
       builder.Host.UseNLog();

       // Add services to the container
       builder.Services.AddControllers();
       builder.Services.AddEndpointsApiExplorer();
       builder.Services.AddSwaggerGen(c =>
       {
           c.SwaggerDoc("v1", new() 
           { 
               Title = "Structured Logging Demo API", 
               Version = "v1",
               Description = "A comprehensive demonstration of structured logging with Serilog and NLog"
           });
       });

       // Register application services
       builder.Services.AddScoped<IUserService, UserService>();
       builder.Services.AddScoped<IOrderService, OrderService>();

       // Configure CORS for development
       builder.Services.AddCors(options =>
       {
           options.AddPolicy("AllowAll", policy =>
           {
               policy.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
           });
       });

       var app = builder.Build();

       // Configure the HTTP request pipeline
       if (app.Environment.IsDevelopment())
       {
           app.UseSwagger();
           app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "Structured Logging Demo API v1");
               c.RoutePrefix = string.Empty; // Serve Swagger UI at root
           });
       }

       app.UseHttpsRedirection();
       app.UseCors("AllowAll");
       app.UseAuthorization();

       // Add request logging middleware
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
           };
       });

       app.MapControllers();

       // Add health check endpoint
       app.MapGet("/health", () => new { 
           Status = "Healthy", 
           Timestamp = DateTime.UtcNow,
           Environment = app.Environment.EnvironmentName 
       });

       Log.Information("Application configured successfully, starting server");
       app.Run();
   }
   catch (Exception ex)
   {
       Log.Fatal(ex, "Application terminated unexpectedly");
   }
   finally
   {
       Log.CloseAndFlush();
   }
   ```

### Step 7: Create API Test Files

1. **Create api-tests.http for comprehensive API testing**
   ```http
   ### Variables
   @baseUrl = https://localhost:7000
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
     "lastName": "Doe",
     "password": "password123"
   }

   ### Create Another User
   POST {{baseUrl}}/api/users
   Content-Type: {{contentType}}

   {
     "username": "jane.smith",
     "email": "jane.smith@example.com",
     "firstName": "Jane",
     "lastName": "Smith",
     "password": "password123"
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

   ### Get User by ID
   GET {{baseUrl}}/api/users/1

   ### Get All Users
   GET {{baseUrl}}/api/users?skip=0&take=10

   ### Get Users with Filter
   GET {{baseUrl}}/api/users?skip=0&take=10&filter=john

   ### Create Order
   POST {{baseUrl}}/api/orders
   Content-Type: {{contentType}}

   {
     "userId": 1,
     "items": [
       {
         "id": 1,
         "productName": "Laptop",
         "price": 999.99,
         "quantity": 1
       },
       {
         "id": 2,
         "productName": "Mouse",
         "price": 25.50,
         "quantity": 2
       }
     ]
   }

   ### Get Order by ID
   GET {{baseUrl}}/api/orders/1

   ### Get Orders by User
   GET {{baseUrl}}/api/orders/user/1?skip=0&take=10

   ### Process Payment
   POST {{baseUrl}}/api/orders/1/pay
   Content-Type: {{contentType}}

   {
     "paymentMethod": "CreditCard",
     "paymentDetails": "****-****-****-1234"
   }

   ### Process Shipping
   POST {{baseUrl}}/api/orders/1/ship
   Content-Type: {{contentType}}

   {
     "shippingAddress": "123 Main St, Anytown, USA",
     "shippingMethod": "Standard"
   }

   ### Error Scenarios

   ### Create User with Duplicate Username
   POST {{baseUrl}}/api/users
   Content-Type: {{contentType}}

   {
     "username": "john.doe",
     "email": "another@example.com",
     "firstName": "Another",
     "lastName": "User",
     "password": "password123"
   }

   ### Create Order for Non-existent User
   POST {{baseUrl}}/api/orders
   Content-Type: {{contentType}}

   {
     "userId": 999,
     "items": [
       {
         "id": 1,
         "productName": "Product",
         "price": 10.00,
         "quantity": 1
       }
     ]
   }

   ### Create Order with No Items
   POST {{baseUrl}}/api/orders
   Content-Type: {{contentType}}

   {
     "userId": 1,
     "items": []
   }

   ### Process Payment for Non-existent Order
   POST {{baseUrl}}/api/orders/999/pay
   Content-Type: {{contentType}}

   {
     "paymentMethod": "CreditCard",
     "paymentDetails": "****-****-****-1234"
   }
   ```

### Step 8: Build and Test the Application

1. **Create logs directory**
   ```powershell
   mkdir logs
   ```

2. **Build the project**
   ```powershell
   dotnet build
   ```

3. **Run the application**
   ```powershell
   dotnet run
   ```

4. **Test the API using the HTTP file or Swagger UI**
   - Open browser and navigate to `https://localhost:7000` for Swagger UI
   - Use the `api-tests.http` file in VS Code with REST Client extension
   - Monitor log files in the `logs` directory

### Step 9: Monitor and Analyze Logs

1. **View console output** for real-time logging
2. **Check log files** in the `logs` directory:
   - `application-YYYY-MM-DD.log` (Serilog text format)
   - `application-json-YYYY-MM-DD.log` (Serilog JSON format)
   - `nlog-YYYY-MM-DD.log` (NLog text format)
   - `nlog-json-YYYY-MM-DD.log` (NLog JSON format)
   - `nlog-errors-YYYY-MM-DD.log` (Error-specific logs)
   - `nlog-performance-YYYY-MM-DD.log` (Performance logs)

### Implementation Best Practices Applied

- **Structured Properties**: Use meaningful property names and preserve data types
- **Correlation IDs**: Track requests across service boundaries
- **Performance Monitoring**: Log execution times for important operations
- **Security Logging**: Track authentication attempts and security events
- **Error Context**: Preserve full context when errors occur
- **Log Levels**: Use appropriate log levels for different scenarios
- **Configuration Flexibility**: Support multiple output formats and targets

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
