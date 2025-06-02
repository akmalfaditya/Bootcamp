using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Models;
using ECommerce.API.Services;

namespace ECommerce.API.Controllers
{
    /// <summary>
    /// Cart controller that handles checkout operations
    /// This is our API entry point that we'll test through integration tests
    /// The controller is kept thin - it just delegates to the cart service
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Main checkout endpoint that processes customer orders
        /// Accepts an order object and returns the processing result
        /// This is the method that ties together all our business logic
        /// </summary>
        [HttpPost("checkout")]
        public ActionResult<string> CheckOut(Order order)
        {
            // Basic null check - in production you'd want more robust validation
            if (order == null)
            {
                return BadRequest("Order cannot be null");
            }

            try
            {
                // Delegate to the cart service for all business logic
                // This keeps the controller thin and focused on HTTP concerns
                var result = _cartService.ValidateCart(order);
                return Ok(result);
            }            catch (Exception)
            {
                // In production, you'd want proper logging here
                // For demo purposes, we'll return a generic error message
                return StatusCode(500, "An error occurred while processing your order");
            }
        }

        /// <summary>
        /// Health check endpoint to verify the service is running
        /// Useful for monitoring and testing purposes
        /// </summary>
        [HttpGet("health")]
        public ActionResult<string> Health()
        {
            return Ok("Cart service is healthy");
        }
    }
}
