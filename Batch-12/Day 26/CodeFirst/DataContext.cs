using Microsoft.EntityFrameworkCore;

namespace CodeFirst;

public class DataContext : DbContext
{
	public DbSet<Employee> Employees { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("FileName=./MyDb.db");
	}
}
