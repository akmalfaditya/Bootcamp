using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.File;

// Early initialization of Serilog - this is critical for startup logging
// We want to capture any startup errors before the full configuration is loaded
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog as the logging provider
    // This replaces the default .NET logging with Serilog throughout the application
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .WriteTo.File("logs/application-.log", 
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30,
            shared: true,
            flushToDiskInterval: TimeSpan.FromSeconds(1))
        .WriteTo.File(new CompactJsonFormatter(), "logs/application-json-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30));

    // Add services to the container.
    builder.Services.AddControllers();
    
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();    // Register our demo services for dependency injection
    // These services will demonstrate structured logging in action
    builder.Services.AddScoped<Serilog.Demo.Services.IOrderService, Serilog.Demo.Services.OrderService>();
    builder.Services.AddScoped<Serilog.Demo.Services.IUserService, Serilog.Demo.Services.UserService>();
    builder.Services.AddScoped<Serilog.Demo.Services.IPaymentService, Serilog.Demo.Services.PaymentService>();

    var app = builder.Build();

    // Add Serilog request logging middleware
    // This captures HTTP request/response information in structured logs
    app.UseSerilogRequestLogging(options =>
    {
        // Customize the message template
        options.MessageTemplate = "Handled {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        
        // Emit debug-level events instead of the defaults
        options.GetLevel = (httpContext, elapsed, ex) => ex != null
            ? LogEventLevel.Error 
            : httpContext.Response.StatusCode > 499 
                ? LogEventLevel.Error 
                : LogEventLevel.Information;
          // Attach additional properties to the request completion event
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            var userAgent = httpContext.Request.Headers.UserAgent.FirstOrDefault();
            diagnosticContext.Set("UserAgent", userAgent ?? "Unknown");
        };
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("Application configured successfully, starting server...");
    
    app.Run();
}
catch (Exception ex)
{
    // This is crucial - any startup exceptions need to be logged
    // Without this, you might never know why your application failed to start
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    // Ensure all logs are flushed before the application shuts down
    // This is especially important for file-based logging
    Log.CloseAndFlush();
}
