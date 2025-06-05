# Entity Framework Migrations Guide

This guide explains how to work with Entity Framework Core migrations in the ProductCatalogAPI project.

## What are Migrations?

Entity Framework migrations are a way to version control your database schema. They allow you to:
- Track changes to your data model over time
- Apply database changes incrementally
- Rollback to previous database versions
- Deploy database changes consistently across environments

## Prerequisites

- .NET 8.0 SDK installed
- Entity Framework Core Tools installed (already included in this project)

## Common Migration Commands

### Check Current Migration Status
```bash
# See which migrations have been applied
dotnet ef migrations list

# See the current database schema
dotnet ef database update --verbose
```

### Creating New Migrations

When you modify your models (Product.cs, Category.cs), create a new migration:

```bash
# Create a new migration
dotnet ef migrations add [MigrationName]

# Examples:
dotnet ef migrations add AddProductImageUrl
dotnet ef migrations add UpdateCategoryConstraints
dotnet ef migrations add AddUserTable
```

### Applying Migrations

```bash
# Apply all pending migrations to the database
dotnet ef database update

# Apply migrations up to a specific migration
dotnet ef database update [MigrationName]

# Apply migrations with detailed logging
dotnet ef database update --verbose
```

### Removing Migrations

```bash
# Remove the last migration (if not applied to database)
dotnet ef migrations remove

# Remove multiple migrations (work backwards)
dotnet ef migrations remove
dotnet ef migrations remove
```

### Rolling Back Migrations

```bash
# Rollback to a previous migration
dotnet ef database update [PreviousMigrationName]

# Rollback to initial state (no migrations)
dotnet ef database update 0
```

## Development Workflow

### 1. Modify Your Models
When you need to change the database schema:

```csharp
public class Product
{
    // Existing properties...
    
    // New property added
    public string ImageUrl { get; set; } = string.Empty;
    
    // Modified property
    [StringLength(200)] // Changed from 100
    public string Name { get; set; } = string.Empty;
}
```

### 2. Create Migration
```bash
dotnet ef migrations add AddImageUrlToProduct
```

### 3. Review Generated Migration
Check the generated migration file in `Migrations/` folder:

```csharp
public partial class AddImageUrlToProduct : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ImageUrl",
            table: "Products",
            type: "TEXT",
            nullable: false,
            defaultValue: "");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ImageUrl",
            table: "Products");
    }
}
```

### 4. Apply Migration
```bash
dotnet ef database update
```

## Important Notes

### Seed Data Considerations
- Always use static dates in seed data (avoid `DateTime.Now`)
- Use consistent data across environments
- Be careful with auto-increment IDs in seed data

### Migration Best Practices
- **Always backup** your database before applying migrations in production
- **Test migrations** in a development environment first
- **Use descriptive names** for migrations (e.g., `AddUserAuthenticationTable`)
- **Don't modify** existing migration files after they've been applied
- **Review generated SQL** to ensure it matches your expectations

### Production Deployment
For production environments:

```bash
# Generate SQL scripts instead of direct database updates
dotnet ef migrations script

# Generate script from specific migration to latest
dotnet ef migrations script FromMigrationName

# Generate script for specific migration range
dotnet ef migrations script FromMigration ToMigration
```

## Troubleshooting

### Migration Errors

**Error: "Migration 'X' has already been applied"**
```bash
# Force remove the migration record
dotnet ef database update [PreviousMigrationName] --force
dotnet ef migrations remove
```

**Error: "Model changes detected"**
- Usually caused by dynamic values in seed data
- Use static dates and values in HasData() calls

**Error: "No migrations found"**
```bash
# Ensure you're in the correct directory
cd ProductCatalogAPI

# Check if migrations folder exists
ls Migrations/
```

### Database Issues

**Reset Database Completely**
```bash
# Delete database file
rm ProductCatalog.db

# Remove all migrations
dotnet ef migrations remove
# (repeat until no migrations left)

# Create fresh migration
dotnet ef migrations add InitialCreate

# Apply migration
dotnet ef database update
```

## Project Structure After Migrations

```
ProductCatalogAPI/
├── Migrations/
│   ├── 20240101000000_InitialCreate.cs
│   ├── 20240101000000_InitialCreate.Designer.cs
│   ├── 20240102000000_AddImageUrlToProduct.cs
│   ├── 20240102000000_AddImageUrlToProduct.Designer.cs
│   └── ProductCatalogDbContextModelSnapshot.cs
├── ProductCatalog.db (SQLite database file)
└── ...
```

## Learning Outcomes

By working with migrations, you'll understand:
- Database version control concepts
- Schema evolution strategies
- Deployment best practices
- Data migration patterns
- Rollback strategies

## Additional Resources

- [EF Core Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Migration Commands Reference](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- [Deployment Strategies](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing)

---

Remember: Migrations are your database's version control system. Use them wisely! 🎉
