using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Repository;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
	public DbSet<Category> Categories { get; set; }
	
}
