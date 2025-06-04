using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Data
{
    /// <summary>
    /// Our main database context. This is the bridge between our C# objects and the SQLite database.
    /// In a Web API, this is crucial because multiple clients will be accessing the same data simultaneously.
    /// EF Core handles the complexity of SQL generation, connection management, and object mapping.
    /// </summary>
    public class ProductCatalogDbContext : DbContext
    {
        /// <summary>
        /// Constructor that accepts DbContextOptions.
        /// This follows the dependency injection pattern, allowing us to configure the database
        /// connection in Program.cs and inject it here.
        /// </summary>
        public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet for Products. This represents the Products table in our database.
        /// Each DbSet corresponds to a table, and EF Core will generate the appropriate SQL
        /// when we query these properties.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// DbSet for Categories. This will be the Categories table.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// OnModelCreating is where we configure the database schema.
        /// This is called when EF Core is building the model, either for migrations
        /// or when connecting to the database.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                // Set up the relationship between Product and Category
                // This tells EF Core that each Product has one Category (via CategoryId)
                // and each Category can have many Products
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent accidental cascading deletes

                // Configure decimal precision for Price to avoid floating-point issues
                // This is important for financial data - we want exactly 2 decimal places
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");

                // Add an index on CategoryId for better query performance
                // Since we'll often filter products by category, this improves API response times
                entity.HasIndex(p => p.CategoryId)
                      .HasDatabaseName("IX_Products_CategoryId");

                // Add an index on IsActive for soft delete queries
                entity.HasIndex(p => p.IsActive)
                      .HasDatabaseName("IX_Products_IsActive");
            });

            // Configure the Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                // Add unique constraint on category name
                // This prevents duplicate categories and enforces business rules at the database level
                entity.HasIndex(c => c.Name)
                      .IsUnique()
                      .HasDatabaseName("IX_Categories_Name_Unique");

                // Index for IsActive to improve queries filtering active categories
                entity.HasIndex(c => c.IsActive)
                      .HasDatabaseName("IX_Categories_IsActive");
            });

            // Seed initial data
            // This is useful for testing and ensuring the API has some data to work with
            SeedData(modelBuilder);
        }        /// <summary>
        /// Seeds the database with initial data.
        /// This is essential for a Web API demo - it gives us data to test with immediately.
        /// In production, you might load this from configuration files or external sources.
        /// IMPORTANT: Using static dates to avoid EF Core migration warnings about dynamic values.
        /// </summary>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Using static dates to avoid EF Core migration issues with dynamic DateTime.UtcNow
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            // Seed Categories first (since Products depend on them)
            var categories = new[]
            {
                new Category
                {
                    Id = 1,
                    Name = "Electronics",
                    Description = "Electronic devices and gadgets",
                    CreatedDate = baseDate.AddDays(1),
                    IsActive = true
                },
                new Category
                {
                    Id = 2,
                    Name = "Books",
                    Description = "Books, magazines, and reading materials",
                    CreatedDate = baseDate.AddDays(2),
                    IsActive = true
                },
                new Category
                {
                    Id = 3,
                    Name = "Clothing",
                    Description = "Apparel and fashion items",
                    CreatedDate = baseDate.AddDays(3),
                    IsActive = true
                },
                new Category
                {
                    Id = 4,
                    Name = "Home & Garden",
                    Description = "Home improvement and gardening supplies",
                    CreatedDate = baseDate.AddDays(4),
                    IsActive = true
                }
            };

            modelBuilder.Entity<Category>().HasData(categories);            // Seed Products
            var products = new[]
            {
                new Product
                {
                    Id = 1,
                    Name = "Smartphone X1",
                    Description = "Latest smartphone with advanced features",
                    Price = 699.99m,
                    StockQuantity = 50,
                    CategoryId = 1,
                    CreatedDate = baseDate.AddDays(10),
                    LastModifiedDate = baseDate.AddDays(10),
                    IsActive = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Laptop Pro 15",
                    Description = "High-performance laptop for professionals",
                    Price = 1299.99m,
                    StockQuantity = 25,
                    CategoryId = 1,
                    CreatedDate = baseDate.AddDays(11),
                    LastModifiedDate = baseDate.AddDays(11),
                    IsActive = true
                },
                new Product
                {
                    Id = 3,
                    Name = "Programming Fundamentals",
                    Description = "Learn the basics of programming",
                    Price = 49.99m,
                    StockQuantity = 100,
                    CategoryId = 2,
                    CreatedDate = baseDate.AddDays(12),
                    LastModifiedDate = baseDate.AddDays(12),
                    IsActive = true
                },
                new Product
                {
                    Id = 4,
                    Name = "Web Development Mastery",
                    Description = "Advanced web development techniques",
                    Price = 79.99m,
                    StockQuantity = 75,
                    CategoryId = 2,
                    CreatedDate = baseDate.AddDays(13),
                    LastModifiedDate = baseDate.AddDays(13),
                    IsActive = true
                },
                new Product
                {
                    Id = 5,
                    Name = "Cotton T-Shirt",
                    Description = "Comfortable cotton t-shirt",
                    Price = 19.99m,
                    StockQuantity = 200,
                    CategoryId = 3,
                    CreatedDate = baseDate.AddDays(14),
                    LastModifiedDate = baseDate.AddDays(14),
                    IsActive = true
                },
                new Product
                {
                    Id = 6,
                    Name = "Garden Tools Set",
                    Description = "Complete set of gardening tools",
                    Price = 89.99m,
                    StockQuantity = 30,
                    CategoryId = 4,
                    CreatedDate = baseDate.AddDays(15),
                    LastModifiedDate = baseDate.AddDays(15),
                    IsActive = true
                }
            };

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
