using Microsoft.AspNetCore.Mvc;
using MyMVC.Databases;
using MyMVC.Models;

namespace MyMVC.Controllers;

public class CategoryController : Controller
{
	private readonly DataContext _db;
	public CategoryController(DataContext db) 
	{
		_db = db;
	}
	public IActionResult Index() 
	{
		List<Category> categories = _db.Categories.ToList();
		return View(categories);
	}
	public IActionResult Create() 
	{
		return View();
	}
	[HttpPost]
	public IActionResult Create(Category category) 
	{
		bool status = _db.Categories.Any(cat => cat.CategoryName == category.CategoryName);
		if(status) 
		{
			TempData["Error"] = "Category already exist";
			return RedirectToAction("Index");
		}
		_db.Categories.Add(category);
		_db.SaveChanges();
		TempData["Success"] = "Create Category success";
		return RedirectToAction("Index");
	}
	public IActionResult Update(int? id) 
	{
		if(id is null) 
		{
			return NotFound();
		}
		Category category = _db.Categories.Find(id);
		if(category is null) 
		{
			return NotFound();
		}
		return View(category);
	}
	[HttpPost]
	public IActionResult Update(Category category) 
	{
		_db.Categories.Update(category);
		_db.SaveChanges();
		return RedirectToAction("Index");
	}
	public IActionResult Delete(int? id) 
	{
		if(id is null) 
		{
			return NotFound();
		}
		Category category = _db.Categories.Find(id);
		if(category is null) 
		{
			return NotFound();
		}
		return View(category);
	}
	[HttpPost]
	public IActionResult Delete(Category category) 
	{
		_db.Categories.Remove(category);
		_db.SaveChanges();
		return RedirectToAction("Index");
	}
}
