using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DTOs;
using MyWebAPI.Models;
using MyWebAPI.Repository;

namespace MyWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase, IController<CategoryDTO>
{
	private IMapper _map;
	private DataContext _db;
	public CategoryController(DataContext db, IMapper map) 
	{
		_db = db;
		_map = map;
	}
	[HttpPost]
	public IActionResult Create(CategoryDTO data)
	{
		Category category = _map.Map<Category>(data);
		_db.Categories.Add(category);
		_db.SaveChanges();
		return Ok();
	}
	[HttpDelete]
	[Route("{id}")]
	public IActionResult Delete(int id)
	{
		Category? category = _db.Categories.Find(id);
		if(category is null ) {
			return NotFound();
		}
		_db.Categories.Remove(category);
		_db.SaveChanges();
		return Ok();
	}
	[HttpGet]
	public IActionResult Get()
	{
		List<Category> categories = _db.Categories.ToList();
		List<CategoryDTO> categoriesDto = _map.Map<List<CategoryDTO>>(categories);
		return Ok(categoriesDto);
	}
	[HttpGet]
	[Route("detail")]
	public IActionResult GetDetail()
	{
		List<Category> categories = _db.Categories.ToList();
		return Ok(categories);
	}
	[HttpGet]
	[Route("{id}")]
	public IActionResult Get(int id)
	{
		Category? category = _db.Categories.Find(id);
		if(category is null ) {
			return NotFound();
		}
		return Ok(category);
	}
	[HttpPut]
	[Route("{id}")]
	public IActionResult Update(int id, CategoryDTO data)
	{
		Category? category = _db.Categories.Find(id);
		if(category is null ) {
			return NotFound();
		}
		category.CategoryName = data.CategoryName;
		category.Description = data.Description;
		_db.Categories.Update(category);
		_db.SaveChanges();
		return Ok();
	}

}
