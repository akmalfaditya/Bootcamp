# ECommerce Testing Demo

A comprehensive demonstration of unit testing with **NUnit** and **Moq** in ASP.NET Core, showcasing best practices for testing business logic in an e-commerce cart checkout system.

## Project Overview

This project demonstrates essential testing concepts through a realistic e-commerce scenario:
- **Domain**: Cart checkout system with payment processing and shipment handling
- **Testing Framework**: NUnit for test structure and assertions
- **Mocking Framework**: Moq for isolating dependencies
- **Architecture**: Clean separation with services, interfaces, and dependency injection

## Project Structure

```
ECommerce.Testing.Demo/
├── ECommerce.API/                 # Main Web API project
│   ├── Models/                    # Domain models
│   │   └── Order.cs              # Order, CartItem, Card, etc.
│   ├── Services/                  # Business logic layer
│   │   ├── Interfaces.cs         # Service contracts
│   │   ├── CartService.cs        # Core cart business logic
│   │   ├── PaymentService.cs     # Payment processing
│   │   └── ShipmentService.cs    # Shipping coordination
│   ├── Controllers/              # API endpoints
│   │   └── CartController.cs     # Cart checkout endpoint
│   └── Program.cs                # DI configuration
└── ECommerce.Tests/              # Unit test project
    └── UnitTest1.cs              # Comprehensive test suite
```

## Key Testing Concepts Demonstrated

### 1. **Mock Setup and Verification**
```csharp
// Setting up mock behavior
_mockPaymentService.Setup(x => x.MakePayment(It.IsAny<decimal>(), It.IsAny<Card>()))
               .Returns(true);

// Verifying interactions
_mockPaymentService.Verify(x => x.MakePayment(expectedAmount, It.IsAny<Card>()), Times.Once);
```

### 2. **Parameterized Testing**
Uses `[TestCase]` attributes to test multiple scenarios efficiently:
- Invalid payment amounts
- Card validation edge cases
- Quantity limit violations
- Payment and shipment failures

### 3. **Isolation Through Mocking**
- **External Dependencies**: Payment gateway simulation
- **Infrastructure Services**: Shipping provider integration
- **Clean Testing**: Each test focuses on specific business logic

### 4. **Real-World Business Rules**
- Cart validation (non-empty, quantity limits)
- Payment validation (amount, card details)
- Order state management
- Error handling and recovery

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Running the Project

1. **Restore Dependencies**
```bash
dotnet restore
```

2. **Build the Solution**
```bash
dotnet build
```

3. **Run the API**
```bash
cd ECommerce.API
dotnet run
```

4. **Run the Tests**
```bash
cd ECommerce.Tests
dotnet test
```

## Creating the Project from Scratch

Follow these steps to build the ECommerce Testing Demo project from the ground up:

### Step 1: Create Solution and Project Structure

1. **Create the solution directory and navigate to it**
```bash
mkdir ECommerce.Testing.Demo
cd ECommerce.Testing.Demo
```

2. **Create a new solution**
```bash
dotnet new sln --name ECommerce.Testing.Demo
```

3. **Create the Web API project**
```bash
dotnet new webapi --name ECommerce.API --framework net8.0
```

4. **Create the test project**
```bash
dotnet new nunit --name ECommerce.Tests --framework net8.0
```

5. **Add projects to the solution**
```bash
dotnet sln add ECommerce.API/ECommerce.API.csproj
dotnet sln add ECommerce.Tests/ECommerce.Tests.csproj
```

### Step 2: Configure Project Dependencies

1. **Add project reference from test project to API project**
```bash
cd ECommerce.Tests
dotnet add reference ../ECommerce.API/ECommerce.API.csproj
```

2. **Install NuGet packages for the test project**
```bash
dotnet add package Moq --version 4.20.69
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.0
```

3. **Navigate back to solution root**
```bash
cd ..
```

### Step 3: Create Domain Models

1. **Create the Models directory in ECommerce.API**
```bash
mkdir ECommerce.API/Models
```

2. **Create Order.cs file** containing:
   - `Order` class with properties: Id, Items, PaymentCard, BillingAddress, ShippingAddress, TotalAmount, Status
   - `CartItem` class with properties: ProductName, Price, Quantity
   - `Card` class with properties: CardNumber, ExpiryDate, CVV
   - `Address` class with properties: Street, City, PostalCode, Country
   - `OrderStatus` enum with values: Pending, Confirmed, Shipped, Delivered

### Step 4: Create Service Interfaces

1. **Create the Services directory in ECommerce.API**
```bash
mkdir ECommerce.API/Services
```

2. **Create Interfaces.cs file** defining:
   - `ICartService` interface with `CheckoutCart` method
   - `IPaymentService` interface with `MakePayment` method
   - `IShipmentService` interface with `ArrangeShipment` method

### Step 5: Implement Service Classes

1. **Create CartService.cs** implementing:
   - Cart validation logic (non-empty cart, quantity limits)
   - Order creation and management
   - Coordination with payment and shipment services
   - Business rule enforcement

2. **Create PaymentService.cs** implementing:
   - Payment processing simulation
   - Card validation (16-digit numbers, future expiry dates)
   - Amount validation (positive values)

3. **Create ShipmentService.cs** implementing:
   - Shipping arrangement simulation
   - Address validation
   - Shipping coordination logic

### Step 6: Create API Controller

1. **Create the Controllers directory in ECommerce.API**
```bash
mkdir ECommerce.API/Controllers
```

2. **Create CartController.cs** implementing:
   - POST endpoint for cart checkout
   - Request model validation
   - Error handling and response formatting
   - Dependency injection of cart service

### Step 7: Configure Dependency Injection

1. **Update Program.cs** to register:
   - Service interfaces and implementations
   - Scoped lifetime for services
   - API controller configuration
   - Swagger documentation

### Step 8: Create Comprehensive Test Suite

1. **Set up the test class structure in UnitTest1.cs**:
   - Create a test class named `CartServiceTests`
   - Add using statements for NUnit, Moq, and project namespaces
   - Declare private fields for the service under test and mock dependencies

2. **Create test setup method**:
   - Add `[SetUp]` attribute to initialize method
   - Initialize mock objects for `IPaymentService` and `IShipmentService`
   - Create instance of `CartService` with mock dependencies
   - Prepare common test data (valid cart items, payment cards, addresses)

3. **Implement cart validation tests**:
   - **Empty Cart Test**: Verify exception thrown when cart has no items
   - **Quantity Limit Test**: Test maximum 10 items per product validation using `[TestCase]`
   - **Null Cart Test**: Ensure proper handling of null cart parameter

4. **Create payment validation tests**:
   - **Invalid Amount Tests**: Use parameterized tests for negative and zero amounts
   - **Card Number Validation**: Test various invalid card number formats (too short, too long, non-numeric)
   - **Expiry Date Validation**: Test past dates and invalid date formats

5. **Implement mock behavior setup tests**:
   - **Payment Service Mock**: Set up `MakePayment` method to return true/false
   - **Shipment Service Mock**: Configure `ArrangeShipment` method behavior
   - **Conditional Mocking**: Create different mock responses based on input parameters

6. **Create integration flow tests**:
   - **Successful Checkout Test**: Test complete flow with valid inputs
   - **Payment Failure Test**: Verify handling when payment service returns false
   - **Shipment Failure Test**: Test behavior when shipment arrangement fails

7. **Add mock verification tests**:
   - **Payment Method Calls**: Verify `MakePayment` called with correct parameters using `Times.Once`
   - **Shipment Method Calls**: Ensure `ArrangeShipment` invoked appropriately
   - **No Calls Verification**: Verify methods not called in failure scenarios

8. **Implement parameterized test cases**:
   - Use `[TestCase]` attributes for multiple validation scenarios
   - Test edge cases with different data combinations
   - Include both valid and invalid parameter sets

9. **Create assertion patterns**:
   - **Exception Testing**: Use `Assert.Throws<Exception>()` for error scenarios
   - **Result Validation**: Verify returned order properties and status
   - **State Verification**: Check object state changes after operations

10. **Add comprehensive test coverage**:
    - **Business Rule Tests**: Validate all cart and payment business rules
    - **Error Message Tests**: Verify specific error messages for different failures
    - **Boundary Tests**: Test limits and edge cases for quantities and amounts

### Step 9: Create API Test File

1. **Create ECommerce.API.http** with sample requests for:
   - Valid checkout scenarios
   - Invalid input testing
   - Edge case validation
   - Different payload combinations

### Step 10: Build and Verify

1. **Restore all dependencies**
```bash
dotnet restore
```

2. **Build the entire solution**
```bash
dotnet build
```

3. **Run all tests to ensure functionality**
```bash
dotnet test
```

4. **Run the API to test endpoints**
```bash
cd ECommerce.API
dotnet run
```

### Implementation Guidelines

When implementing each component, ensure:

- **Clean Architecture**: Maintain clear separation between controllers, services, and models
- **Dependency Injection**: Use constructor injection for all service dependencies
- **Interface Segregation**: Keep interfaces focused and cohesive
- **Error Handling**: Implement proper exception handling and validation
- **Test Coverage**: Write tests for all business logic and edge cases
- **Documentation**: Include comprehensive XML documentation comments

## Detailed Unit Testing Implementation

### Setting Up the Test Environment

1. **Create the basic test class structure**:
```csharp
using NUnit.Framework;
using Moq;
using ECommerce.API.Services;
using ECommerce.API.Models;

namespace ECommerce.Tests
{
    [TestFixture]
    public class CartServiceTests
    {
        private CartService _cartService;
        private Mock<IPaymentService> _mockPaymentService;
        private Mock<IShipmentService> _mockShipmentService;
        
        [SetUp]
        public void Setup()
        {
            // Initialize mocks and service under test
        }
    }
}
```

2. **Initialize test dependencies in Setup method**:
```csharp
[SetUp]
public void Setup()
{
    _mockPaymentService = new Mock<IPaymentService>();
    _mockShipmentService = new Mock<IShipmentService>();
    _cartService = new CartService(_mockPaymentService.Object, _mockShipmentService.Object);
}
```

### Creating Test Data Helpers

1. **Create helper methods for test data**:
```csharp
private List<CartItem> CreateValidCartItems()
{
    return new List<CartItem>
    {
        new CartItem { ProductName = "Laptop", Price = 999.99m, Quantity = 1 },
        new CartItem { ProductName = "Mouse", Price = 25.50m, Quantity = 2 }
    };
}

private Card CreateValidCard()
{
    return new Card
    {
        CardNumber = "1234567890123456",
        ExpiryDate = "12/25",
        CVV = "123"
    };
}

private Address CreateValidAddress()
{
    return new Address
    {
        Street = "123 Main St",
        City = "Anytown",
        PostalCode = "12345",
        Country = "USA"
    };
}
```

### Implementing Validation Tests

1. **Test empty cart validation**:
```csharp
[Test]
public void CheckoutCart_EmptyCart_ThrowsArgumentException()
{
    // Arrange
    var emptyCart = new List<CartItem>();
    var card = CreateValidCard();
    var address = CreateValidAddress();

    // Act & Assert
    var ex = Assert.Throws<ArgumentException>(() => 
        _cartService.CheckoutCart(emptyCart, card, address, address));
    
    Assert.That(ex.Message, Contains.Substring("Cart cannot be empty"));
}
```

2. **Test quantity limits with parameterized tests**:
```csharp
[TestCase(11)]
[TestCase(15)]
[TestCase(20)]
public void CheckoutCart_ExceedsQuantityLimit_ThrowsArgumentException(int quantity)
{
    // Arrange
    var cartItems = new List<CartItem>
    {
        new CartItem { ProductName = "Laptop", Price = 999.99m, Quantity = quantity }
    };
    var card = CreateValidCard();
    var address = CreateValidAddress();

    // Act & Assert
    var ex = Assert.Throws<ArgumentException>(() => 
        _cartService.CheckoutCart(cartItems, card, address, address));
    
    Assert.That(ex.Message, Contains.Substring("Maximum 10 items per product"));
}
```

### Setting Up Mock Behaviors

1. **Configure payment service mocks**:
```csharp
[Test]
public void CheckoutCart_ValidInput_CallsPaymentService()
{
    // Arrange
    var cartItems = CreateValidCartItems();
    var card = CreateValidCard();
    var address = CreateValidAddress();
    var expectedAmount = cartItems.Sum(item => item.Price * item.Quantity);

    _mockPaymentService.Setup(x => x.MakePayment(It.IsAny<decimal>(), It.IsAny<Card>()))
                      .Returns(true);
    _mockShipmentService.Setup(x => x.ArrangeShipment(It.IsAny<Address>()))
                       .Returns(true);

    // Act
    var result = _cartService.CheckoutCart(cartItems, card, address, address);

    // Assert
    _mockPaymentService.Verify(x => x.MakePayment(expectedAmount, card), Times.Once);
    Assert.That(result.Status, Is.EqualTo(OrderStatus.Confirmed));
}
```

2. **Test payment failure scenarios**:
```csharp
[Test]
public void CheckoutCart_PaymentFails_ThrowsInvalidOperationException()
{
    // Arrange
    var cartItems = CreateValidCartItems();
    var card = CreateValidCard();
    var address = CreateValidAddress();

    _mockPaymentService.Setup(x => x.MakePayment(It.IsAny<decimal>(), It.IsAny<Card>()))
                      .Returns(false);

    // Act & Assert
    var ex = Assert.Throws<InvalidOperationException>(() => 
        _cartService.CheckoutCart(cartItems, card, address, address));
    
    Assert.That(ex.Message, Contains.Substring("Payment failed"));
    
    // Verify shipment service was not called
    _mockShipmentService.Verify(x => x.ArrangeShipment(It.IsAny<Address>()), Times.Never);
}
```

### Testing Card Validation

1. **Test invalid card numbers**:
```csharp
[TestCase("123456789012345")]    // Too short
[TestCase("12345678901234567")]  // Too long
[TestCase("abcd567890123456")]   // Contains letters
[TestCase("")]                   // Empty
[TestCase(null)]                 // Null
public void CheckoutCart_InvalidCardNumber_ThrowsArgumentException(string cardNumber)
{
    // Arrange
    var cartItems = CreateValidCartItems();
    var card = new Card { CardNumber = cardNumber, ExpiryDate = "12/25", CVV = "123" };
    var address = CreateValidAddress();

    // Act & Assert
    var ex = Assert.Throws<ArgumentException>(() => 
        _cartService.CheckoutCart(cartItems, card, address, address));
    
    Assert.That(ex.Message, Contains.Substring("Invalid card number"));
}
```

2. **Test expiry date validation**:
```csharp
[TestCase("12/20")]  // Past date
[TestCase("13/25")]  // Invalid month
[TestCase("12/2025")] // Wrong format
[TestCase("invalid")] // Invalid format
public void CheckoutCart_InvalidExpiryDate_ThrowsArgumentException(string expiryDate)
{
    // Arrange
    var cartItems = CreateValidCartItems();
    var card = new Card { CardNumber = "1234567890123456", ExpiryDate = expiryDate, CVV = "123" };
    var address = CreateValidAddress();

    // Act & Assert
    var ex = Assert.Throws<ArgumentException>(() => 
        _cartService.CheckoutCart(cartItems, card, address, address));
    
    Assert.That(ex.Message, Contains.Substring("Invalid expiry date"));
}
```

### Testing Complete Workflows

1. **Test successful checkout flow**:
```csharp
[Test]
public void CheckoutCart_ValidInput_ReturnsConfirmedOrder()
{
    // Arrange
    var cartItems = CreateValidCartItems();
    var card = CreateValidCard();
    var billingAddress = CreateValidAddress();
    var shippingAddress = CreateValidAddress();
    var expectedTotal = cartItems.Sum(item => item.Price * item.Quantity);

    _mockPaymentService.Setup(x => x.MakePayment(It.IsAny<decimal>(), It.IsAny<Card>()))
                      .Returns(true);
    _mockShipmentService.Setup(x => x.ArrangeShipment(It.IsAny<Address>()))
                       .Returns(true);

    // Act
    var result = _cartService.CheckoutCart(cartItems, card, billingAddress, shippingAddress);

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Status, Is.EqualTo(OrderStatus.Confirmed));
    Assert.That(result.TotalAmount, Is.EqualTo(expectedTotal));
    Assert.That(result.Items, Has.Count.EqualTo(cartItems.Count));
    Assert.That(result.PaymentCard, Is.EqualTo(card));
    Assert.That(result.BillingAddress, Is.EqualTo(billingAddress));
    Assert.That(result.ShippingAddress, Is.EqualTo(shippingAddress));
}
```

### Advanced Mock Verification

1. **Verify specific method calls and parameters**:
```csharp
[Test]
public void CheckoutCart_ValidInput_VerifiesServiceInteractions()
{
    // Arrange
    var cartItems = CreateValidCartItems();
    var card = CreateValidCard();
    var address = CreateValidAddress();
    var expectedAmount = 1051.48m; // Calculated total

    _mockPaymentService.Setup(x => x.MakePayment(expectedAmount, card)).Returns(true);
    _mockShipmentService.Setup(x => x.ArrangeShipment(address)).Returns(true);

    // Act
    _cartService.CheckoutCart(cartItems, card, address, address);

    // Assert
    _mockPaymentService.Verify(x => x.MakePayment(expectedAmount, card), Times.Once);
    _mockShipmentService.Verify(x => x.ArrangeShipment(address), Times.Once);
    _mockPaymentService.VerifyNoOtherCalls();
    _mockShipmentService.VerifyNoOtherCalls();
}
```

### Running and Analyzing Tests

1. **Execute tests using dotnet CLI**:
```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "CartServiceTests"

# Run tests with coverage (requires coverage package)
dotnet test --collect:"XPlat Code Coverage"
```

2. **Test organization best practices**:
   - Group related tests using nested classes or categories
   - Use descriptive test names that explain the scenario
   - Follow AAA pattern (Arrange, Act, Assert) consistently
   - Keep tests independent and isolated
   - Use parameterized tests for similar scenarios with different data

### Testing the API


Use the provided `ECommerce.API.http` file or test manually:

```http
POST https://localhost:7139/api/cart/checkout
Content-Type: application/json

{
  "items": [
    {
      "productName": "Laptop",
      "price": 999.99,
      "quantity": 1
    }
  ],
  "paymentCard": {
    "cardNumber": "1234567890123456",
    "expiryDate": "12/25",
    "cvv": "123"
  },
  "billingAddress": {
    "street": "123 Main St",
    "city": "Anytown",
    "postalCode": "12345",
    "country": "USA"
  },
  "shippingAddress": {
    "street": "456 Oak Ave",
    "city": "Somewhere",
    "postalCode": "67890",
    "country": "USA"
  }
}
```

## Test Scenarios Covered

### Validation Tests
- **Empty Cart Validation**: Prevents checkout with no items
- **Quantity Limits**: Enforces maximum 10 items per product
- **Payment Amount**: Validates positive payment amounts
- **Card Number**: Ensures 16-digit card numbers
- **Expiry Date**: Validates future expiry dates

### Integration Tests
- **Payment Processing**: Simulates payment gateway interactions
- **Shipment Coordination**: Tests shipping provider integration
- **Error Handling**: Verifies proper error responses

### Success Scenarios
- **Complete Checkout Flow**: End-to-end successful order processing
- **Order State Management**: Confirms proper order status updates

## Learning Objectives

This project teaches:

1. **Unit Testing Fundamentals**
   - Test structure (Arrange, Act, Assert)
   - Test isolation and independence
   - Meaningful test names and organization

2. **Mocking with Moq**
   - Creating mock objects
   - Setting up method behavior
   - Verifying method calls
   - Using `It.IsAny<>()` for flexible matching

3. **NUnit Features**
   - `[TestCase]` for parameterized tests
   - `[SetUp]` for test initialization
   - Assertion methods and error handling
   - Test organization and naming

4. **Testing Best Practices**
   - Dependency injection for testability
   - Single responsibility in tests
   - Clear test data setup
   - Comprehensive coverage of business rules

## Code Quality Features

- **Clean Architecture**: Clear separation of concerns
- **SOLID Principles**: Dependency inversion, single responsibility
- **Comprehensive Comments**: Trainer-style explanations throughout
- **Error Handling**: Proper exception management
- **Validation Logic**: Realistic business rule enforcement

Each test demonstrates specific concepts with real-world applicability, making it an excellent reference for building robust, testable applications.

---

**Happy Testing!**
