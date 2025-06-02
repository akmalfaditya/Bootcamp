using ECommerce.API.Models;

namespace ECommerce.API.Services
{
    /// <summary>
    /// Interface for cart validation and order processing
    /// This defines the contract for our main business logic
    /// </summary>
    public interface ICartService
    {
        string ValidateCart(Order order);
    }

    /// <summary>
    /// Interface for payment processing operations
    /// Abstracts away payment gateway integration
    /// </summary>
    public interface IPaymentService
    {
        string ChargeAndShip(Order order);
        bool MakePayment(Card card);
    }

    /// <summary>
    /// Interface for shipment operations
    /// Abstracts away shipping provider integration
    /// </summary>
    public interface IShipmentService
    {
        ShipmentDetails? Ship(AddressInfo address);
    }
}
