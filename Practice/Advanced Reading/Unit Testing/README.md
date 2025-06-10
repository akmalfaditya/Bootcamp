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

## Creating the Project from Scratch

Follow these steps to build the Unit Testing project from the ground up using Test-Driven Development principles:

### Step 1: Create Solution and Project Structure

1. **Create the solution directory and navigate to it**
   ```powershell
   mkdir unit-testing-using-nunit
   cd unit-testing-using-nunit
   ```

2. **Create a new solution**
   ```powershell
   dotnet new sln --name unit-testing-using-nunit
   ```

3. **Create the class library project**
   ```powershell
   dotnet new classlib --name PrimeService --framework net8.0
   ```

4. **Create the test project**
   ```powershell
   dotnet new nunit --name PrimeService.Tests --framework net8.0
   ```

5. **Add projects to the solution**
   ```powershell
   dotnet sln add PrimeService/PrimeService.csproj
   dotnet sln add PrimeService.Tests/PrimeService.Tests.csproj
   ```

6. **Add project reference from test project to library project**
   ```powershell
   cd PrimeService.Tests
   dotnet add reference ../PrimeService/PrimeService.csproj
   cd ..
   ```

### Step 2: Configure Test Project Dependencies

1. **Navigate to the test project directory**
   ```powershell
   cd PrimeService.Tests
   ```

2. **Ensure NUnit packages are properly installed**
   ```powershell
   dotnet add package NUnit --version 4.2.2
   dotnet add package NUnit3TestAdapter --version 4.6.0
   dotnet add package Microsoft.NET.Test.Sdk --version 17.11.1
   ```

3. **Navigate back to solution root**
   ```powershell
   cd ..
   ```

### Step 3: Define the Service Interface (TDD Red Phase)

1. **Create the initial PrimeService class with method signatures**
   ```powershell
   # Navigate to the library project
   cd PrimeService
   ```

2. **Replace Class1.cs with PrimeService.cs containing method stubs**:
   ```csharp
   namespace PrimeService
   {
       public class PrimeService
       {
           public bool IsPrime(int candidate)
           {
               throw new NotImplementedException("Method not implemented yet");
           }

           public int[] FindPrimesUpTo(int limit)
           {
               throw new NotImplementedException("Method not implemented yet");
           }

           public int GetNextPrime(int number)
           {
               throw new NotImplementedException("Method not implemented yet");
           }
       }
   }
   ```

3. **Delete the default Class1.cs file**
   ```powershell
   Remove-Item Class1.cs
   ```

### Step 4: Write Comprehensive Tests First (TDD Red Phase)

1. **Navigate to the test project**
   ```powershell
   cd ../PrimeService.Tests
   ```

2. **Create test file for IsPrime method (PrimeService_IsPrimeShould.cs)**:
   ```csharp
   using NUnit.Framework;

   namespace PrimeService.Tests
   {
       [TestFixture]
       public class PrimeService_IsPrimeShould
       {
           private PrimeService _primeService;

           [SetUp]
           public void SetUp()
           {
               _primeService = new PrimeService();
           }

           [Test]
           public void IsPrime_InputIs1_ReturnFalse()
           {
               bool result = _primeService.IsPrime(1);
               Assert.That(result, Is.False, "1 should not be considered prime");
           }

           [Test]
           public void IsPrime_InputIs2_ReturnTrue()
           {
               bool result = _primeService.IsPrime(2);
               Assert.That(result, Is.True, "2 should be considered prime");
           }

           [TestCase(3)]
           [TestCase(5)]
           [TestCase(7)]
           [TestCase(11)]
           [TestCase(13)]
           [TestCase(17)]
           [TestCase(19)]
           [TestCase(23)]
           [TestCase(97)]
           [TestCase(101)]
           public void IsPrime_InputIsKnownPrime_ReturnTrue(int candidate)
           {
               bool result = _primeService.IsPrime(candidate);
               Assert.That(result, Is.True, $"{candidate} should be considered prime");
           }

           [TestCase(4)]
           [TestCase(6)]
           [TestCase(8)]
           [TestCase(9)]
           [TestCase(10)]
           [TestCase(12)]
           [TestCase(15)]
           [TestCase(21)]
           [TestCase(25)]
           [TestCase(100)]
           public void IsPrime_InputIsKnownComposite_ReturnFalse(int candidate)
           {
               bool result = _primeService.IsPrime(candidate);
               Assert.That(result, Is.False, $"{candidate} should not be considered prime");
           }

           [TestCase(-1)]
           [TestCase(-5)]
           [TestCase(-100)]
           public void IsPrime_InputIsNegative_ReturnFalse(int candidate)
           {
               bool result = _primeService.IsPrime(candidate);
               Assert.That(result, Is.False, "Negative numbers should not be considered prime");
           }

           [Test]
           public void IsPrime_InputIs0_ReturnFalse()
           {
               bool result = _primeService.IsPrime(0);
               Assert.That(result, Is.False, "0 should not be considered prime");
           }
       }
   }
   ```

3. **Create test file for FindPrimesUpTo method (PrimeService_FindPrimesUpToShould.cs)**:
   ```csharp
   using NUnit.Framework;

   namespace PrimeService.Tests
   {
       [TestFixture]
       public class PrimeService_FindPrimesUpToShould
       {
           private PrimeService _primeService;

           [SetUp]
           public void SetUp()
           {
               _primeService = new PrimeService();
           }

           [Test]
           public void FindPrimesUpTo_InputIs2_ReturnArray2()
           {
               int[] result = _primeService.FindPrimesUpTo(2);
               int[] expected = { 2 };
               Assert.That(result, Is.EqualTo(expected));
           }

           [Test]
           public void FindPrimesUpTo_InputIs10_ReturnCorrectPrimes()
           {
               int[] result = _primeService.FindPrimesUpTo(10);
               int[] expected = { 2, 3, 5, 7 };
               Assert.That(result, Is.EqualTo(expected));
           }

           [Test]
           public void FindPrimesUpTo_InputIs20_ReturnCorrectPrimes()
           {
               int[] result = _primeService.FindPrimesUpTo(20);
               int[] expected = { 2, 3, 5, 7, 11, 13, 17, 19 };
               Assert.That(result, Is.EqualTo(expected));
           }

           [TestCase(-1)]
           [TestCase(-5)]
           [TestCase(0)]
           [TestCase(1)]
           public void FindPrimesUpTo_InputIsInvalidRange_ReturnEmptyArray(int limit)
           {
               int[] result = _primeService.FindPrimesUpTo(limit);
               Assert.That(result, Is.Empty);
           }

           [Test]
           public void FindPrimesUpTo_InputIs100_ReturnCorrectCount()
           {
               int[] result = _primeService.FindPrimesUpTo(100);
               Assert.That(result.Length, Is.EqualTo(25), "There should be 25 primes up to 100");
           }
       }
   }
   ```

4. **Create test file for GetNextPrime method (PrimeService_GetNextPrimeShould.cs)**:
   ```csharp
   using NUnit.Framework;

   namespace PrimeService.Tests
   {
       [TestFixture]
       public class PrimeService_GetNextPrimeShould
       {
           private PrimeService _primeService;

           [SetUp]
           public void SetUp()
           {
               _primeService = new PrimeService();
           }

           [Test]
           public void GetNextPrime_InputIs1_Return2()
           {
               int result = _primeService.GetNextPrime(1);
               Assert.That(result, Is.EqualTo(2));
           }

           [Test]
           public void GetNextPrime_InputIs2_Return3()
           {
               int result = _primeService.GetNextPrime(2);
               Assert.That(result, Is.EqualTo(3));
           }

           [TestCase(3, 5)]
           [TestCase(5, 7)]
           [TestCase(7, 11)]
           [TestCase(11, 13)]
           [TestCase(13, 17)]
           [TestCase(17, 19)]
           [TestCase(19, 23)]
           public void GetNextPrime_InputIsKnownPrime_ReturnNextPrime(int input, int expected)
           {
               int result = _primeService.GetNextPrime(input);
               Assert.That(result, Is.EqualTo(expected));
           }

           [TestCase(4, 5)]
           [TestCase(6, 7)]
           [TestCase(8, 11)]
           [TestCase(9, 11)]
           [TestCase(10, 11)]
           public void GetNextPrime_InputIsComposite_ReturnNextPrime(int input, int expected)
           {
               int result = _primeService.GetNextPrime(input);
               Assert.That(result, Is.EqualTo(expected));
           }

           [TestCase(-1)]
           [TestCase(-5)]
           [TestCase(0)]
           public void GetNextPrime_InputIsNegativeOrZero_Return2(int input)
           {
               int result = _primeService.GetNextPrime(input);
               Assert.That(result, Is.EqualTo(2));
           }
       }
   }
   ```

### Step 5: Run Tests to Confirm Red Phase

1. **Build the solution to check for compilation errors**
   ```powershell
   cd ..
   dotnet build
   ```

2. **Run tests to confirm they all fail (Red phase)**
   ```powershell
   dotnet test
   ```

   All tests should fail with `NotImplementedException` messages.

### Step 6: Implement the Service Methods (TDD Green Phase)

1. **Navigate to the library project**
   ```powershell
   cd PrimeService
   ```

2. **Implement the complete PrimeService class with efficient algorithms**:
   ```csharp
   namespace PrimeService
   {
       public class PrimeService
       {
           /// <summary>
           /// Determines if a number is prime using optimized trial division
           /// Time complexity: O(√n)
           /// </summary>
           public bool IsPrime(int candidate)
           {
               if (candidate < 2)
                   return false;

               if (candidate == 2)
                   return true;

               if (candidate % 2 == 0)
                   return false;

               var boundary = (int)Math.Floor(Math.Sqrt(candidate));

               for (int i = 3; i <= boundary; i += 2)
               {
                   if (candidate % i == 0)
                       return false;
               }

               return true;
           }

           /// <summary>
           /// Finds all prime numbers up to a given limit using Sieve of Eratosthenes
           /// Time complexity: O(n log log n)
           /// </summary>
           public int[] FindPrimesUpTo(int limit)
           {
               if (limit < 2)
                   return new int[0];

               bool[] isPrime = new bool[limit + 1];
               for (int i = 2; i <= limit; i++)
                   isPrime[i] = true;

               for (int i = 2; i * i <= limit; i++)
               {
                   if (isPrime[i])
                   {
                       for (int j = i * i; j <= limit; j += i)
                           isPrime[j] = false;
                   }
               }

               var primes = new List<int>();
               for (int i = 2; i <= limit; i++)
               {
                   if (isPrime[i])
                       primes.Add(i);
               }

               return primes.ToArray();
           }

           /// <summary>
           /// Gets the next prime number after the given number
           /// Uses the optimized IsPrime method for checking
           /// </summary>
           public int GetNextPrime(int number)
           {
               if (number < 2)
                   return 2;

               int candidate = number + 1;

               while (candidate <= int.MaxValue)
               {
                   if (IsPrime(candidate))
                       return candidate;

                   candidate++;

                   // Prevent infinite loops near int.MaxValue
                   if (candidate < 0) // Overflow occurred
                       throw new OverflowException("No prime found within integer range");
               }

               throw new OverflowException("No prime found within integer range");
           }
       }
   }
   ```

### Step 7: Run Tests to Confirm Green Phase

1. **Navigate back to solution root**
   ```powershell
   cd ..
   ```

2. **Build the solution**
   ```powershell
   dotnet build
   ```

3. **Run all tests to confirm they pass (Green phase)**
   ```powershell
   dotnet test --verbosity normal
   ```

   All tests should now pass successfully.

### Step 8: Add Performance and Edge Case Tests (TDD Refactor Phase)

1. **Add additional comprehensive test cases to each test file**
2. **Add performance benchmarks**
3. **Test edge cases and boundary conditions**
4. **Verify algorithm efficiency**

### Step 9: Create Basic Demonstration Test

1. **Navigate to test project**
   ```powershell
   cd PrimeService.Tests
   ```

2. **Update UnitTest1.cs with basic demonstration**:
   ```csharp
   using NUnit.Framework;

   namespace PrimeService.Tests
   {
       public class Tests
       {
           private PrimeService _primeService;

           [SetUp]
           public void Setup()
           {
               _primeService = new PrimeService();
           }

           [Test]
           public void Test1()
           {
               Assert.Pass();
           }

           [Test]
           public void IsPrime_BasicTest()
           {
               // Arrange
               int number = 7;

               // Act
               bool result = _primeService.IsPrime(number);

               // Assert
               Assert.That(result, Is.True);
           }
       }
   }
   ```

### Step 10: Final Verification and Documentation

1. **Run final test suite**
   ```powershell
   cd ..
   dotnet test --verbosity normal
   ```

2. **Generate test coverage report (optional)**
   ```powershell
   dotnet test --collect:"XPlat Code Coverage"
   ```

3. **Build in release mode**
   ```powershell
   dotnet build --configuration Release
   ```

4. **Verify project structure**
   ```powershell
   tree /f
   ```

### Implementation Guidelines

When implementing each component, ensure:

- **Test-Driven Development**: Always write tests before implementation
- **Red-Green-Refactor**: Follow the TDD cycle strictly
- **Comprehensive Coverage**: Test edge cases, boundaries, and performance
- **Clean Code**: Use meaningful names and proper documentation
- **Efficient Algorithms**: Implement optimized mathematical algorithms
- **Error Handling**: Properly handle edge cases and invalid inputs

### Testing Best Practices Applied

- **AAA Pattern**: Arrange, Act, Assert in every test
- **Descriptive Names**: Test method names clearly describe the scenario
- **Single Responsibility**: Each test validates one specific behavior
- **Test Independence**: Tests do not depend on each other
- **Parameterized Tests**: Use TestCase for testing multiple inputs efficiently
- **Performance Testing**: Verify algorithms handle large inputs reasonably

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

