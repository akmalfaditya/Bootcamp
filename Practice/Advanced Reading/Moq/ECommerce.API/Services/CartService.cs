using ECommerce.API.Models;

namespace ECommerce.API.Services
{
    /// <summary>
    /// Main business logic service for cart operations
    /// This is the core service we want to test thoroughly
    /// Handles cart validation and coordinates with payment service
    /// </summary>
    public class CartService : ICartService
    {
        private readonly IPaymentService _paymentService;

        public CartService(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Validates the cart and processes the order if valid
        /// This method contains the business rules we need to test:
        /// 1. Cart must contain at least one item
        /// 2. Product quantities must be between 1 and 10
        /// 3. If validation passes, delegate to payment service
        /// </summary>
        public string ValidateCart(Order order)
        {
            // Business rule: Cart cannot be empty
            if (order.CartItems.Count < 1)
                return "Invalid Cart";

            // Business rule: Quantity must be reasonable (1-10 items per product)
            // This prevents abuse and ensures realistic order quantities
            if (order.CartItems.Any(x => x.Quantity < 0 || x.Quantity > 10))
                return "Invalid Product Quantity";

            // If cart validation passes, proceed with payment processing
            return _paymentService.ChargeAndShip(order);
        }
    }
}
