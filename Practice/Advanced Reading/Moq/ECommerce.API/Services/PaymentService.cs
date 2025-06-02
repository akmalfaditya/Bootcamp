using ECommerce.API.Models;

namespace ECommerce.API.Services
{
    /// <summary>
    /// Payment processing service that handles card validation and payment processing
    /// This service coordinates with external payment providers and shipping services
    /// We'll partially mock this in tests to isolate external dependencies
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IShipmentService _shipmentService;

        public PaymentService(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        /// <summary>
        /// Main payment processing method
        /// Validates card details, processes payment, and initiates shipping
        /// Contains multiple validation rules that we need to test
        /// </summary>
        public string ChargeAndShip(Order order)
        {
            // Validation rule: Amount must be positive
            if (order.Card.Amount <= 0)
            {
                return "Amount Not Valid";
            }            // Card validation rules
            if (order.Card == null)
                return "Payment card is required";

            // Check if card is expired
            if (order.Card.ValidTo < DateTime.Now)
                return "Card Expired";

            // Validate card number length (simplified validation)
            // Real applications would use more sophisticated card validation
            if (order.Card.CardNumber.Length < 16)
                return "CardNumber Not Valid";

            // Process the actual payment through external service
            bool paymentSuccess = MakePayment(order.Card);

            if (paymentSuccess)
            {
                // If payment succeeds, initiate shipping
                var shipment = _shipmentService.Ship(order.Address);
                if (shipment != null)
                    return "Item Shipped";
                else
                    return "Something went wrong with the shipment!!!";
            }
            else
            {
                return "Payment Failed";
            }
        }

        /// <summary>
        /// Simulates external payment service call
        /// In real implementation, this would call a payment gateway API
        /// We mark this as virtual so we can mock it in tests
        /// </summary>
        public virtual bool MakePayment(Card card)
        {
            // This would normally make an HTTP call to a payment provider
            // For demo purposes, we'll return true to simulate successful payment
            // In tests, we'll mock this method to control the return value
            return true;
        }
    }
}
