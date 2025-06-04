using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Services;

namespace ProductCatalogAPI.Controllers
{
    /// <summary>
    /// RESTful Web API Controller for Category operations.
    /// 
    /// Base route: /api/categories
    /// This demonstrates how to structure API endpoints for related but distinct resources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// GET /api/categories
        /// Gets all categories with optional filtering.
        /// 
        /// Query parameters:
        /// - activeOnly: if true, returns only active categories (default: true)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] bool activeOnly = true)
        {
            try
            {
                var categories = activeOnly
                    ? await _categoryService.GetActiveCategoriesAsync()
                    : await _categoryService.GetAllCategoriesAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving categories", error = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/categories/{id}
        /// Gets a specific category by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = $"Category with ID {id} not found" });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the category", error = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/categories/{id}/products
        /// Gets a category with all its products.
        /// 
        /// This demonstrates how to expose related data through nested routes.
        /// This endpoint returns more data, so use it judiciously.
        /// </summary>
        [HttpGet("{id}/products")]
        public async Task<ActionResult<CategoryWithProductsDto>> GetCategoryWithProducts(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryWithProductsAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = $"Category with ID {id} not found" });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the category with products", error = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/categories
        /// Creates a new category.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryDto);

                return CreatedAtAction(
                    nameof(GetCategory),
                    new { id = createdCategory.Id },
                    createdCategory);
            }
            catch (ArgumentException ex)
            {
                // Category name conflicts return 400 Bad Request
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the category", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/categories/{id}
        /// Updates an existing category.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);

                if (updatedCategory == null)
                {
                    return NotFound(new { message = $"Category with ID {id} not found" });
                }

                return Ok(updatedCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the category", error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/categories/{id}
        /// Hard deletes a category.
        /// Will fail if the category contains products.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var deleted = await _categoryService.DeleteCategoryAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = $"Category with ID {id} not found" });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Cannot delete category with products
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the category", error = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/categories/{id}/deactivate
        /// Soft deletes a category and all its products.
        /// </summary>
        [HttpPost("{id}/deactivate")]
        public async Task<ActionResult> DeactivateCategory(int id)
        {
            try
            {
                var deactivated = await _categoryService.DeactivateCategoryAsync(id);

                if (!deactivated)
                {
                    return NotFound(new { message = $"Category with ID {id} not found" });
                }

                return Ok(new { message = $"Category with ID {id} and all its products have been deactivated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deactivating the category", error = ex.Message });
            }
        }
    }
}
