# Unit Testing with NUnit - Prime Number Service

A comprehensive demonstration of Test-Driven Development (TDD) using NUnit framework in .NET, featuring a complete prime number service with extensive test coverage.

## Project Overview

This project showcases **Test-Driven Development (TDD)** principles by implementing a `PrimeService` class with three core methods:

- **`IsPrime(int candidate)`** - Determines if a number is prime
- **`FindPrimesUpTo(int limit)`** - Finds all prime numbers up to a given limit
- **`GetNextPrime(int number)`** - Finds the next prime number after a given number

## Project Structure

```
unit-testing-using-nunit/
├── unit-testing-using-nunit.sln          # Solution file
├── README.md                              # This documentation
├── PrimeService/                          # Main library project
│   ├── PrimeService.csproj               # Library project file
│   └── PrimeService.cs                   # Implementation with efficient algorithms
└── PrimeService.Tests/                   # Test project
    ├── PrimeService.Tests.csproj         # Test project file (NUnit packages)
    ├── UnitTest1.cs                      # Basic demonstration test
    ├── PrimeService_IsPrimeShould.cs     # 31 tests for IsPrime method
    ├── PrimeService_FindPrimesUpToShould.cs  # 17 tests for FindPrimesUpTo method
    └── PrimeService_GetNextPrimeShould.cs    # 30 tests for GetNextPrime method
```

## Test Coverage

The project includes **78 comprehensive unit tests** covering:

### IsPrime Method Tests (31 tests)
- Edge cases (negative numbers, 0, 1, 2)
- Known prime numbers (2, 3, 5, 7, 11, 13, 17, 19, 23, 97, 101)
- Known composite numbers (4, 6, 8, 9, 10, 12, 15, 21, 25, 100)
- Large number performance testing
- Parametrized test cases for efficiency

### FindPrimesUpTo Method Tests (17 tests)
- Edge cases (negative, 0, 1, 2)
- Various limit values (3, 10, 20, 100)
- Array correctness and sorting
- Performance with large limits (100,000)
- Parametrized test cases
- Prime inclusion/exclusion logic

### GetNextPrime Method Tests (30 tests)
- Edge cases (negative, 0, 1)
- Sequential prime verification
- Large number handling
- Performance testing
- Parametrized test cases for various inputs
- Comprehensive coverage of prime sequences

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio Code, Visual Studio, or any .NET-compatible IDE

### Running the Project

1. **Clone/Download the project**
   ```bash
   cd "path/to/unit-testing-using-nunit"
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run all tests**
   ```bash
   dotnet test
   ```

5. **Run tests with detailed output**
   ```bash
   dotnet test --verbosity normal
   ```

6. **Run tests with coverage (optional)**
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

## TDD Process Demonstrated

This project follows the classic **Red-Green-Refactor** TDD cycle:

### 1. Red Phase (Failed Tests)
- Created comprehensive test cases first
- All tests initially failed with `NotImplementedException`
- Tests defined the expected behavior and API

### 2. Green Phase (Passing Tests)
- Implemented efficient algorithms to make all tests pass
- **IsPrime**: Optimized trial division with √n limit
- **FindPrimesUpTo**: Sieve of Eratosthenes algorithm
- **GetNextPrime**: Sequential prime finding with overflow protection

### 3. Refactor Phase (Optimize)
- Code is already well-optimized with efficient algorithms
- Clear documentation and proper error handling
- Follows SOLID principles and clean code practices

## Implementation Highlights

### Efficient Algorithms Used

1. **Prime Checking (IsPrime)**
   - O(√n) time complexity
   - Special handling for 2 (only even prime)
   - Only checks odd divisors after 2

2. **Sieve of Eratosthenes (FindPrimesUpTo)**
   - O(n log log n) time complexity
   - Memory efficient boolean array
   - Optimized marking starting from i²

3. **Sequential Prime Finding (GetNextPrime)**
   - Reuses optimized IsPrime method
   - Overflow protection for large numbers
   - Handles all edge cases properly

## Learning Objectives

This project demonstrates:

1. **Test-Driven Development (TDD)**
   - Writing tests before implementation
   - Red-Green-Refactor cycle
   - Test-first thinking

2. **NUnit Testing Framework**
   - Test attributes (`[Test]`, `[TestCase]`, `[SetUp]`)
   - Assertions and constraint model
   - Parametrized tests
   - Test organization and naming

3. **Unit Testing Best Practices**
   - AAA pattern (Arrange, Act, Assert)
   - Descriptive test names
   - Edge case testing
   - Performance testing
   - Test isolation

4. **Clean Code Principles**
   - Single Responsibility Principle
   - Clear method names and documentation
   - Proper error handling
   - Efficient algorithms

## Key Testing Concepts Covered

- **Edge Case Testing**: Negative numbers, zero, one, two
- **Boundary Testing**: Testing limits and edge values
- **Parametrized Tests**: Testing multiple inputs efficiently
- **Performance Testing**: Ensuring reasonable execution times
- **Error Handling**: Testing exception scenarios
- **Collection Testing**: Verifying array results and properties

## Performance Characteristics

- **IsPrime**: Handles large numbers (up to ~10⁶) efficiently
- **FindPrimesUpTo**: Can find all primes up to 100,000 in reasonable time
- **GetNextPrime**: Fast sequential prime finding with overflow protection

## Technologies Used

- **.NET 8.0**: Latest LTS version
- **NUnit 4.2.2**: Modern testing framework
- **NUnit3TestAdapter**: Test discovery and execution
- **Microsoft.NET.Test.Sdk**: Test platform
- **C#**: Primary programming language

## Notes

This project serves as an excellent reference for:
- Learning TDD methodology
- Understanding NUnit framework features
- Implementing efficient mathematical algorithms
- Writing comprehensive unit tests
- Following .NET testing best practices

