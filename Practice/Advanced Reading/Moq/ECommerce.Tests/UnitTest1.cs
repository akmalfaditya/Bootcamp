using NUnit.Framework;
using Moq;
using ECommerce.API.Services;
using ECommerce.API.Models;

namespace ECommerce.Tests
{
    /// <summary>
    /// Comprehensive test suite for CartService demonstrating NUnit and Moq usage
    /// This class shows real-world testing scenarios including:
    /// - Setting up mocks for external dependencies
    /// - Testing business logic validation rules
    /// - Using parameterized tests for multiple scenarios
    /// - Verifying mock interactions
    /// </summary>
    [TestFixture]
    public class CartServiceTests
    {
        // These are our test dependencies - the main objects we'll work with
        private ICartService _cartService;
        private Mock<PaymentService> _paymentServiceMock;
        private Mock<IShipmentService> _shipmentServiceMock;

        /// <summary>
        /// SetUp method runs before each test
        /// Here we configure our mocks and create the service under test
        /// This ensures each test starts with a clean slate
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Create a mock for ShipmentService since it's entirely external
            // We don't want our tests making real shipping API calls
            _shipmentServiceMock = new Mock<IShipmentService>();

            // Create a partial mock of PaymentService
            // We want to test most of its logic but mock the MakePayment method
            // The constructor parameter is the mocked shipment service
            _paymentServiceMock = new Mock<PaymentService>(_shipmentServiceMock.Object);

            // Create the actual CartService - this is what we're testing
            // We inject the mocked payment service to control its behavior
            _cartService = new CartService(_paymentServiceMock.Object);
        }

        /// <summary>
        /// Test for empty cart validation
        /// Business rule: Cart must contain at least one item
        /// </summary>
        [Test]
        public void ValidateCart_EmptyCart_ReturnsInvalidCart()
        {
            // Arrange - Create an order with no cart items
            var order = new Order
            {
                CartItems = new List<CartItem>(), // Empty cart
                Card = CreateValidCard(),
                Address = new AddressInfo()
            };

            // Act - Call the method we're testing
            var result = _cartService.ValidateCart(order);

            // Assert - Verify the expected result
            Assert.AreEqual("Invalid Cart", result);
        }

        /// <summary>
        /// Test for invalid quantity validation
        /// Business rule: Quantity must be between 1 and 10
        /// </summary>
        [Test]
        public void ValidateCart_InvalidQuantity_ReturnsInvalidQuantity()
        {
            // Arrange - Create an order with invalid quantity
            var order = new Order
            {
                CartItems = new List<CartItem>
                {
                    new CartItem { ProductId = "1001", Quantity = 15, Price = 100 } // Over limit
                },
                Card = CreateValidCard(),
                Address = new AddressInfo()
            };

            // Act
            var result = _cartService.ValidateCart(order);

            // Assert
            Assert.AreEqual("Invalid Product Quantity", result);
        }

        /// <summary>
        /// Parameterized test demonstrating multiple test scenarios
        /// This is the main test method from the material - it shows how to test
        /// different combinations of inputs and expected outcomes
        /// </summary>
        [Test]
        [TestCase(-1, "4041000011114567", true, 1, true, true, "Amount Not Valid")]  // Invalid amount
        [TestCase(10, "404100001111456", true, 2, true, true, "CardNumber Not Valid")] // Invalid card number
        [TestCase(12, "4041000011114561", false, 3, true, true, "Card Expired")] // Expired card
        [TestCase(11, "40410000111145610", true, 11, true, true, "Invalid Product Quantity")] // Invalid quantity
        [TestCase(5, "40410000111145610", true, 9, false, true, "Payment Failed")] // Payment fails
        [TestCase(8, "40410000111145610", true, 9, true, false, "Something went wrong with the shipment!!!")] // Shipment fails
        [TestCase(4, "40410000111145610", true, 9, true, true, "Item Shipped")] // Success case
        public void CartService_ValidateCart_HandlesAllScenarios(
            double amount, 
            string cardNumber, 
            bool validDate, 
            int quantity,
            bool paymentSuccess, 
            bool shipmentSuccess, 
            string expectedResult)
        {
            // Arrange - Build the test data based on parameters
            var card = new Card
            {
                CardNumber = cardNumber,
                ValidTo = validDate ? DateTime.Now.AddDays(10) : DateTime.Now.AddDays(-10),
                Name = "Test Customer",
                Amount = amount
            };

            var address = new AddressInfo
            {
                Street = "123 Test St",
                City = "Test City",
                State = "Test State",
                ZipCode = "12345"
            };

            var cartItems = new List<CartItem> 
            { 
                new CartItem 
                { 
                    ProductId = "1001", 
                    Quantity = quantity, 
                    Price = 100,
                    ProductName = "Test Product"
                } 
            };

            var order = new Order 
            { 
                Address = address, 
                CartItems = cartItems, 
                Card = card 
            };

            // Configure the shipment service mock
            // Return a shipment object for success, null for failure
            var shipmentDetails = shipmentSuccess ? new ShipmentDetails 
            {
                TrackingNumber = "TEST123",
                EstimatedDelivery = DateTime.Now.AddDays(3),
                Carrier = "Test Carrier"
            } : null;

            _shipmentServiceMock
                .Setup(x => x.Ship(It.IsAny<AddressInfo>()))
                .Returns(shipmentDetails);

            // Configure the payment service mock
            // CallBase = true means call the real methods except for what we explicitly mock
            _paymentServiceMock.CallBase = true;
            _paymentServiceMock
                .Setup(x => x.MakePayment(It.IsAny<Card>()))
                .Returns(paymentSuccess);

            // Act - Call the method under test
            var result = _cartService.ValidateCart(order);

            // Verify that our mocks were called (optional but good practice)
            if (quantity >= 1 && quantity <= 10 && order.CartItems.Count > 0)
            {
                // These verifications only make sense if we got past cart validation
                _paymentServiceMock.Verify(); // Verify all setups were called
                _shipmentServiceMock.Verify(); // Verify all setups were called
            }

            // Assert - Check the result matches what we expect
            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// Test specifically for card number validation
        /// Shows how to test individual validation rules
        /// </summary>
        [Test]
        public void ValidateCart_ShortCardNumber_ReturnsCardNumberNotValid()
        {
            // Arrange
            var order = CreateValidOrder();
            order.Card.CardNumber = "123456789"; // Too short

            // Configure mocks for this scenario
            _paymentServiceMock.CallBase = true;

            // Act
            var result = _cartService.ValidateCart(order);

            // Assert
            Assert.AreEqual("CardNumber Not Valid", result);
        }

        /// <summary>
        /// Test for expired card scenario
        /// </summary>
        [Test]
        public void ValidateCart_ExpiredCard_ReturnsCardExpired()
        {
            // Arrange
            var order = CreateValidOrder();
            order.Card.ValidTo = DateTime.Now.AddDays(-30); // Expired 30 days ago

            // Configure mocks
            _paymentServiceMock.CallBase = true;

            // Act
            var result = _cartService.ValidateCart(order);

            // Assert
            Assert.AreEqual("Card Expired", result);
        }

        /// <summary>
        /// Test the successful flow end-to-end
        /// This verifies that when everything goes right, we get the expected result
        /// </summary>
        [Test]
        public void ValidateCart_ValidOrder_ReturnsItemShipped()
        {
            // Arrange
            var order = CreateValidOrder();

            // Mock successful shipment
            _shipmentServiceMock
                .Setup(x => x.Ship(It.IsAny<AddressInfo>()))
                .Returns(new ShipmentDetails 
                { 
                    TrackingNumber = "SUCCESS123",
                    EstimatedDelivery = DateTime.Now.AddDays(5),
                    Carrier = "Express Shipping"
                });

            // Mock successful payment
            _paymentServiceMock.CallBase = true;
            _paymentServiceMock
                .Setup(x => x.MakePayment(It.IsAny<Card>()))
                .Returns(true);

            // Act
            var result = _cartService.ValidateCart(order);

            // Assert
            Assert.AreEqual("Item Shipped", result);

            // Verify that the shipment service was called exactly once
            _shipmentServiceMock.Verify(x => x.Ship(It.IsAny<AddressInfo>()), Times.Once);
            
            // Verify that the payment service was called exactly once
            _paymentServiceMock.Verify(x => x.MakePayment(It.IsAny<Card>()), Times.Once);
        }

        /// <summary>
        /// Helper method to create a valid order for testing
        /// This reduces code duplication in our tests
        /// </summary>
        private Order CreateValidOrder()
        {
            return new Order
            {
                CartItems = new List<CartItem>
                {
                    new CartItem 
                    { 
                        ProductId = "1001", 
                        Quantity = 2, 
                        Price = 25.99m,
                        ProductName = "Test Product"
                    }
                },
                Card = CreateValidCard(),
                Address = new AddressInfo
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "CA",
                    ZipCode = "90210",
                    Country = "USA"
                }
            };
        }

        /// <summary>
        /// Helper method to create a valid card for testing
        /// </summary>
        private Card CreateValidCard()
        {
            return new Card
            {
                CardNumber = "4041000011114567", // Valid length
                Name = "John Doe",
                ValidTo = DateTime.Now.AddYears(2), // Valid for 2 more years
                Amount = 51.98, // Positive amount
                CVV = "123"
            };
        }
    }
}