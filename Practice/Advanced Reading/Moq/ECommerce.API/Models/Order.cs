namespace ECommerce.API.Models
{
    /// <summary>
    /// Represents a customer order containing cart items, payment card, and shipping address
    /// This is our main domain model that flows through the checkout process
    /// </summary>
    public class Order
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public Card Card { get; set; } = new Card();
        public AddressInfo Address { get; set; } = new AddressInfo();
    }

    /// <summary>
    /// Individual item in the shopping cart
    /// Contains product details and quantity information
    /// </summary>
    public class CartItem
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Payment card information
    /// Contains all the data needed for payment processing
    /// Note: In real applications, sensitive card data should be tokenized
    /// </summary>
    public class Card
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime ValidTo { get; set; }
        public double Amount { get; set; }
        public string CVV { get; set; } = string.Empty;
    }

    /// <summary>
    /// Customer's shipping address information
    /// Used for both billing and shipping purposes in this demo
    /// </summary>
    public class AddressInfo
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    /// <summary>
    /// Shipment acknowledgment details returned by shipping service
    /// Contains tracking information and delivery estimates
    /// </summary>
    public class ShipmentDetails
    {
        public string TrackingNumber { get; set; } = string.Empty;
        public DateTime EstimatedDelivery { get; set; }
        public string Carrier { get; set; } = string.Empty;
        public string Status { get; set; } = "Processing";
    }
}
