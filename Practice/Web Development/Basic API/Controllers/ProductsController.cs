using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Services;

namespace ProductCatalogAPI.Controllers
{
    /// <summary>
    /// RESTful Web API Controller for Product operations.
    /// This controller demonstrates all the REST principles and HTTP verbs.
    /// Each action corresponds to a specific HTTP operation that clients can call.
    /// 
    /// Base route: /api/products
    /// This follows REST conventions where the URL represents the resource (products).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Constructor injection of the product service.
        /// This is the dependency injection pattern in action - the controller depends on
        /// the service interface, not the concrete implementation.
        /// </summary>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// GET /api/products
        /// Gets all products, with optional filtering.
        /// 
        /// Query parameters:
        /// - activeOnly: if true, returns only active products
        /// - categoryId: filter by category
        /// - search: search term for product names/descriptions
        /// 
        /// This demonstrates how REST APIs can provide flexible querying capabilities.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSummaryDto>>> GetProducts(
            [FromQuery] bool activeOnly = true,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? search = null)
        {
            try
            {
                IEnumerable<ProductSummaryDto> products;

                // Handle different query scenarios
                if (!string.IsNullOrWhiteSpace(search))
                {
                    // Search takes precedence over other filters
                    products = await _productService.SearchProductsAsync(search);
                }
                else if (categoryId.HasValue)
                {
                    // Filter by category
                    products = await _productService.GetProductsByCategoryAsync(categoryId.Value);
                }
                else if (activeOnly)
                {
                    // Default: get active products only
                    products = await _productService.GetActiveProductsAsync();
                }
                else
                {
                    // Get all products (including inactive)
                    products = await _productService.GetAllProductsAsync();
                }

                // Return 200 OK with the products
                // The framework automatically serializes this to JSON
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception in a real application
                // Return 500 Internal Server Error
                return StatusCode(500, new { message = "An error occurred while retrieving products", error = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/products/{id}
        /// Gets a specific product by its ID.
        /// 
        /// This demonstrates the REST pattern for retrieving individual resources.
        /// Returns 404 if the product doesn't exist, 200 if found.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                
                if (product == null)
                {
                    // Return 404 Not Found with a helpful message
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                // Return 200 OK with the product details
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the product", error = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/products
        /// Creates a new product.
        /// 
        /// The request body should contain a CreateProductDto with the product information.
        /// This demonstrates the REST POST operation for creating new resources.
        /// Returns 201 Created with the new product details and a Location header.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            // Model validation happens automatically due to the data annotations on the DTO
            if (!ModelState.IsValid)
            {
                // Return 400 Bad Request with validation errors
                return BadRequest(ModelState);
            }

            try
            {
                var createdProduct = await _productService.CreateProductAsync(createProductDto);

                // Return 201 Created with the location of the new resource
                // This follows REST conventions - the client gets the ID and location of the created resource
                return CreatedAtAction(
                    nameof(GetProduct),
                    new { id = createdProduct.Id },
                    createdProduct);
            }
            catch (ArgumentException ex)
            {
                // Business logic errors (like invalid category) return 400 Bad Request
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the product", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/products/{id}
        /// Updates an existing product.
        /// 
        /// PUT is idempotent - calling it multiple times with the same data has the same effect.
        /// The request body should contain an UpdateProductDto with the updated information.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, updateProductDto);

                if (updatedProduct == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                // Return 200 OK with the updated product
                return Ok(updatedProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the product", error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/products/{id}
        /// Performs a hard delete of a product.
        /// 
        /// In most production systems, you'd use soft delete instead.
        /// Returns 204 No Content if successful, 404 if the product doesn't exist.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleted = await _productService.DeleteProductAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                // Return 204 No Content - the resource has been successfully deleted
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the product", error = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/products/{id}/deactivate
        /// Performs a soft delete by marking the product as inactive.
        /// 
        /// This is often preferred over hard deletes in production systems
        /// because it preserves historical data and relationships.
        /// </summary>
        [HttpPost("{id}/deactivate")]
        public async Task<ActionResult> DeactivateProduct(int id)
        {
            try
            {
                var deactivated = await _productService.DeactivateProductAsync(id);

                if (!deactivated)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                return Ok(new { message = $"Product with ID {id} has been deactivated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deactivating the product", error = ex.Message });
            }
        }
    }
}
