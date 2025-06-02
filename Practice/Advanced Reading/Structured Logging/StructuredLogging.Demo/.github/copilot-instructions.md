# GitHub Copilot Instructions for Structured Logging Demo Project

## Project Context
This is an educational ASP.NET Core Web API project demonstrating comprehensive structured logging patterns using Serilog and NLog. The project implements a complete e-commerce system with user management and order processing to showcase real-world logging scenarios.

## Code Style and Patterns

### Logging Style
- **Always use structured logging** with named placeholders: `{PropertyName}` not string interpolation
- **Include correlation IDs** in all operations for request tracing
- **Use appropriate log levels**: Debug for development, Information for business events, Error for exceptions
- **Preserve context** across service calls and operations
- **Add performance timing** for business-critical operations using Stopwatch

### Comment Style
- Use **trainer-style comments** that explain the "why" behind logging decisions
- Focus on **educational value** - explain structured logging benefits
- Include **real-world context** about when and why to use specific patterns
- Avoid AI-like comments - write as if training a developer team

### Example Logging Patterns
```csharp
// ✅ Good - Structured with context
_logger.LogInformation("Order {OrderId} created for user {UserId} with amount {TotalAmount:C}", 
    order.Id, order.UserId, order.TotalAmount);

// ❌ Avoid - String interpolation loses structure
_logger.LogInformation($"Order {order.Id} created for user {order.UserId}");
```

## Domain Context
- **User Management**: Authentication, user creation, user retrieval
- **Order Processing**: Order creation, payment processing, shipping, order history
- **Business Rules**: Validation, security checks, business logic enforcement
- **Performance Monitoring**: Operation timing, resource usage tracking

## Architecture Principles
- **Service Layer Pattern**: Business logic in services with comprehensive logging
- **Controller Pattern**: HTTP context logging with correlation IDs
- **Dependency Injection**: All services registered and properly injected
- **Separation of Concerns**: Models, services, and controllers have distinct responsibilities

## Structured Logging Concepts to Maintain
1. **Security Event Logging**: Authentication attempts, authorization failures
2. **Business Operation Tracking**: Order lifecycle, payment processing
3. **Performance Monitoring**: Response times, resource usage
4. **Error Context Preservation**: Full exception details with business context
5. **Audit Trail Creation**: User actions, data modifications
6. **Correlation ID Usage**: Request tracing across service boundaries

## When Extending the Project
- **Add correlation IDs** to any new operations
- **Include timing measurements** for business-critical operations
- **Log security events** for any authentication/authorization code
- **Preserve business context** in all error scenarios
- **Use structured properties** that would be useful for log analysis
- **Document logging patterns** in trainer-style comments

## Technology Stack Awareness
- ASP.NET Core 8.0 Web API
- Serilog with Console, File, and JSON sinks
- NLog with multiple targets and ASP.NET Core integration
- Entity Framework patterns (even without actual database)
- Swagger/OpenAPI for API documentation

## File Organization
- `Models/`: Domain entities and DTOs with validation attributes
- `Services/`: Business logic with comprehensive structured logging
- `Controllers/`: HTTP endpoints with request/response logging
- `logs/`: Runtime-generated log files (multiple formats)
- Configuration files: `Program.cs` (Serilog), `NLog.config` (NLog)

## Educational Focus
This project is designed to teach structured logging concepts through practical examples. When suggesting code changes:
- Explain the logging pattern being demonstrated
- Show how structured data helps with debugging and monitoring
- Include real-world scenarios where this logging would be valuable
- Demonstrate both Serilog and NLog approaches when relevant
