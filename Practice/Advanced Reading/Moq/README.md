# ECommerce Testing Demo

A comprehensive demonstration of unit testing with **NUnit** and **Moq** in ASP.NET Core, showcasing best practices for testing business logic in an e-commerce cart checkout system.

## 📋 Project Overview

This project demonstrates essential testing concepts through a realistic e-commerce scenario:
- **Domain**: Cart checkout system with payment processing and shipment handling
- **Testing Framework**: NUnit for test structure and assertions
- **Mocking Framework**: Moq for isolating dependencies
- **Architecture**: Clean separation with services, interfaces, and dependency injection

## 🏗️ Project Structure

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

## 🎯 Key Testing Concepts Demonstrated

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

## 🚀 Getting Started

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

## 🧪 Test Scenarios Covered

### ✅ Validation Tests
- **Empty Cart Validation**: Prevents checkout with no items
- **Quantity Limits**: Enforces maximum 10 items per product
- **Payment Amount**: Validates positive payment amounts
- **Card Number**: Ensures 16-digit card numbers
- **Expiry Date**: Validates future expiry dates

### ✅ Integration Tests
- **Payment Processing**: Simulates payment gateway interactions
- **Shipment Coordination**: Tests shipping provider integration
- **Error Handling**: Verifies proper error responses

### ✅ Success Scenarios
- **Complete Checkout Flow**: End-to-end successful order processing
- **Order State Management**: Confirms proper order status updates

## 📚 Learning Objectives

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

## 🔍 Code Quality Features

- **Clean Architecture**: Clear separation of concerns
- **SOLID Principles**: Dependency inversion, single responsibility
- **Comprehensive Comments**: Trainer-style explanations throughout
- **Error Handling**: Proper exception management
- **Validation Logic**: Realistic business rule enforcement

## 🎓 Training Notes

This project serves as a practical example for:
- Junior developers learning unit testing
- Teams adopting TDD practices
- Code review and quality standards
- Integration testing strategies

Each test demonstrates specific concepts with real-world applicability, making it an excellent reference for building robust, testable applications.

---

**Happy Testing!** 🧪✨
