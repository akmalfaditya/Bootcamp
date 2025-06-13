using Microsoft.EntityFrameworkCore;
using FluentValidationMVC.Data;
using FluentValidationMVC.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidationMVC.Models;
using FluentValidationMVC.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This is where we configure dependency injection - a key concept in modern web development
// We're telling the application how to create instances of our services when they're needed

// Add Entity Framework and configure SQLite database
// Connection string points to a local SQLite database file
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=studentmanagement.db"));

// Register our business layer service
// AddScoped means one instance per HTTP request - perfect for database operations
// This allows controllers to receive IStudentService through constructor injection
builder.Services.AddScoped<IStudentService, StudentService>();

// Add MVC services
builder.Services.AddControllersWithViews();

// Enable FluentValidation - this is the key integration step from the training material
// AddFluentValidationAutoValidation integrates FluentValidation with ASP.NET Core's validation pipeline
// This means validation will automatically occur during model binding
builder.Services.AddFluentValidationAutoValidation();

// Enable client-side validation adapters for FluentValidation
// This allows FluentValidation rules to work with jQuery validation on the client side
// Requires jQuery and jQuery Validation to be included in your views
builder.Services.AddFluentValidationClientsideAdapters();

// Register all our FluentValidation validators
// Each validator must be registered for dependency injection
// AddTransient is appropriate for validators as they're stateless and lightweight
// We're focusing on Student and Grade models for this demo
builder.Services.AddTransient<IValidator<Student>, StudentValidator>();
builder.Services.AddTransient<IValidator<Grade>, GradeValidator>();

var app = builder.Build();

// Apply database migrations automatically
// This is a more professional approach than EnsureCreated()
// Migrations provide version control for your database schema
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Apply any pending migrations
    // This will create the database if it doesn't exist and apply all migrations
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
