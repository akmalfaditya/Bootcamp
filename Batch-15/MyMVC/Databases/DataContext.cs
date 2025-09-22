using Microsoft.EntityFrameworkCore;
using MyMVC.Models;

namespace MyMVC.Databases;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
}
