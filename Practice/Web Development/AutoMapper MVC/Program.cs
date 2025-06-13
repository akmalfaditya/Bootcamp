using Microsoft.EntityFrameworkCore;
using AutoMapperMVC.Data;
using AutoMapperMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This is where we configure dependency injection - a key concept in modern web development
// We're telling the application how to create instances of our services when they're needed

// Add Entity Framework and configure SQLite database
// Connection string points to a local SQLite database file
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=studentmanagement.db"));

// Register AutoMapper with all profiles in the current assembly
// This will scan for any classes that inherit from Profile and register them
// AutoMapper will handle the object-to-object mapping based on our profiles
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register our business layer service
// AddScoped means one instance per HTTP request - perfect for database operations
// This allows controllers to receive IStudentService through constructor injection
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllersWithViews();

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
