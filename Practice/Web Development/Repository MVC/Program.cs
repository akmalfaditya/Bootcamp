using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using RepositoryMVC.Services;
using RepositoryMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This is where we configure dependency injection - a key concept in modern web development
// We're telling the application how to create instances of our services when they're needed

// Add Entity Framework and configure SQLite database
// Connection string points to a local SQLite database file
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=studentmanagement.db"));

// Register Repository Pattern Components
// The Repository Pattern provides abstraction between business logic and data access
// This makes our application more testable, maintainable, and loosely coupled

// Register the Unit of Work - This coordinates all repository operations
// Scoped lifetime ensures one instance per HTTP request
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register individual repositories for direct injection if needed
// These are also scoped to match the Unit of Work lifetime
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();

// Register generic repository factory for entities without specialized repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register our business layer service - Updated to use Repository Pattern
// The service layer now depends on repositories instead of direct DbContext access
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
