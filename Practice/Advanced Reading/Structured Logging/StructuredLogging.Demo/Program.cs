using Serilog;
using StructuredLogging.Demo.Services;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

// =================================================================
// STRUCTURED LOGGING CONFIGURATION - SERILOG APPROACH
// =================================================================
// This section demonstrates how to configure Serilog for structured logging
// Serilog provides excellent structured logging capabilities with multiple output sinks

// Configure Serilog as the logging provider
// This replaces the default .NET Core logging with Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    // Console sink with structured output - great for development
    .WriteTo.Console(outputTemplate: 
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] " +
        "{Message:lj} {Properties:j}{NewLine}{Exception}")
    
    // File sink with JSON format - perfect for log aggregation tools
    .WriteTo.File("logs/application-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] " +
                       "{Message:lj} {Properties:j}{NewLine}{Exception}")
    
    // File sink with JSON format for structured data analysis
    .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(), "logs/application-json-.log",
        rollingInterval: RollingInterval.Day)
    
    // Enrich logs with additional context information
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .CreateLogger();

// Replace default logging with Serilog
builder.Host.UseSerilog();

// =================================================================
// SERVICE REGISTRATION
// =================================================================
// Register application services for dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Add framework services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =================================================================
// MIDDLEWARE PIPELINE CONFIGURATION
// =================================================================
// Configure the HTTP request pipeline with structured logging context

// Development environment configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Log startup information with structured data
    Log.Information("Application starting in {Environment} mode at {StartupTime}", 
        app.Environment.EnvironmentName, DateTime.UtcNow);
}

// Add request logging middleware to capture HTTP context
app.UseSerilogRequestLogging(options =>
{
    // Customize the request logging to include additional context
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {        // Add additional structured properties to each request log
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown");
        diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
        
        // Add correlation ID for tracing requests across services
        var correlationId = Guid.NewGuid().ToString();
        httpContext.Items["CorrelationId"] = correlationId;
        diagnosticContext.Set("CorrelationId", correlationId);
    };
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// =================================================================
// APPLICATION STARTUP LOGGING
// =================================================================
try
{
    Log.Information("Starting up the Structured Logging Demo application");
    Log.Information("Application configured with {ServiceCount} registered services", 
        builder.Services.Count);
    
    app.Run();
}
catch (Exception ex)
{
    // Ensure any startup errors are captured in structured format
    Log.Fatal(ex, "Application terminated unexpectedly during startup");
    throw;
}
finally
{
    // Ensure logs are flushed before application shutdown
    Log.CloseAndFlush();
}
