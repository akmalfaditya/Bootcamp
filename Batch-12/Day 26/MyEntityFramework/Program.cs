using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MyEntityFramework;

class Program
{
	static void Main()
	{
		using (Northwind northwind = new())
		{
			//Connection Test
			bool status = northwind.Database.CanConnect();
			if (status == false)
			{
				return;
			}
			Console.WriteLine(status);

			//Read All Category
			// ReadAllCategory(northwind);

			//Create Category
			// CreateCategory(northwind);
			// ReadAllCategory(northwind);

			//Delete Category
			// DeleteCategory(northwind);
			// ReadAllCategory(northwind);

			//Update
			// UpdateCategory(northwind);

			//Search/Filter
			List<Category> categories = northwind.Categories.Where(a => a.CategoryName.Contains("o")).Include(a => a.Products).ToList();
			foreach (var cat in categories) 
			{
				Console.WriteLine($"{cat.CategoryId} : {cat.CategoryName} = {cat.Description}");
				Console.WriteLine("\tTotal Product = " + cat.Products.Count());
				foreach(var pr in cat.Products) 
				{
					Console.WriteLine($"{pr.ProductId} : {pr.ProductName}");
				}
			}

			//Search/Filter to Get Id
			// Category category = northwind.Categories.Where(c => Regex.IsMatch(c.CategoryName, "^Con.*s$")).FirstOrDefault();
			// if (category is null)
			// {
			// 	Console.WriteLine("No Category Found");
			// }
			// else 
			// {
			// 	Console.WriteLine(category.CategoryName);
			// }
		}
	}

	private static void UpdateCategory(Northwind northwind)
	{
		int updateId = 10;
		Category updateCategory = northwind.Categories.Find(updateId);
		updateCategory.CategoryName = "Bowo";
		updateCategory.Description = "Tiktok";
		northwind.Categories.Update(updateCategory);
		northwind.SaveChanges();
	}

	private static void DeleteCategory(Northwind northwind)
	{
		int deleteId = 9;
		Category deleteCategory = northwind.Categories.Find(deleteId);
		northwind.Categories.Remove(deleteCategory);
		northwind.SaveChanges();
	}

	private static void ReadAllCategory(Northwind northwind)
	{
		List<Category> categories = northwind.Categories.ToList();
		foreach (Category cat in categories)
		{
			Console.WriteLine($"{cat.CategoryId} : {cat.CategoryName} = {cat.Description}");
		}
	}
	private static void CreateCategory(Northwind northwind)
	{
		Category category = new Category();
		category.CategoryName = "Wakil President";
		category.Description = "Tanya bapak aja";
		northwind.Categories.Add(category);
		northwind.SaveChanges();
	}
}
