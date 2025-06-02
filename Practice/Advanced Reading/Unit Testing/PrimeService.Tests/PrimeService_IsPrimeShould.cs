/*
 * PrimeService_IsPrimeShould.cs - Unit Tests for IsPrime Method
 * 
 * This file demonstrates comprehensive unit testing practices using NUnit.
 * We follow the naming convention: [ClassName]_[MethodName]Should
 * 
 * Key concepts demonstrated:
 * - Test-Driven Development (TDD) approach
 * - Arrange-Act-Assert pattern
 * - Test case parameterization
 * - Test setup and teardown
 * - Descriptive test names that explain the scenario
 * 
 * Remember: Good tests are like documentation - they should clearly explain
 * what the code is supposed to do in different scenarios.
 */

using NUnit.Framework;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    /// <summary>
    /// Test fixture for testing the IsPrime method of PrimeService
    /// 
    /// [TestFixture] tells NUnit that this class contains tests.
    /// We organize tests by the method we're testing to keep things clean.
    /// </summary>
    [TestFixture]    public class PrimeService_IsPrimeShould
    {
        // This is our "System Under Test" (SUT)
        // We create a fresh instance for each test to ensure test isolation
        private Prime.Services.PrimeService _primeService;

        /// <summary>
        /// SetUp method runs before each individual test
        /// This ensures each test starts with a clean state
        /// Think of it as "preparing the stage" for each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Create a new instance for each test
            // This prevents tests from affecting each other
            _primeService = new Prime.Services.PrimeService();
        }

        /// <summary>
        /// TearDown runs after each test (if you need cleanup)
        /// For this simple example, we don't need it, but it's good to know about
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // In more complex scenarios, you might need to:
            // - Close database connections
            // - Delete temporary files
            // - Reset static state
            // For our simple service, no cleanup needed
        }

        #region Edge Cases and Invalid Inputs

        /// <summary>
        /// Test that demonstrates TDD - this will fail initially
        /// We start with the simplest case: 1 is not prime
        /// 
        /// Test naming convention: [Method]_[Scenario]_[ExpectedResult]
        /// This makes it crystal clear what we're testing
        /// </summary>
        [Test]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            // Arrange: Set up the test data
            int input = 1;
            
            // Act: Execute the method we're testing
            // Note: This will initially throw NotImplementedException
            var result = _primeService.IsPrime(input);
            
            // Assert: Verify the result is what we expect
            Assert.That(result, Is.False, "1 should not be prime by mathematical definition");
        }

        /// <summary>
        /// Using TestCase attribute to test multiple similar scenarios
        /// This is more efficient than writing separate tests for each value
        /// All values less than 2 should return false
        /// </summary>
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void IsPrime_ValuesLessThan2_ReturnFalse(int value)
        {
            // Act
            var result = _primeService.IsPrime(value);
            
            // Assert
            Assert.That(result, Is.False, $"{value} should not be prime (values less than 2 are not prime)");
        }

        /// <summary>
        /// Test with extremely large negative numbers
        /// Good tests cover edge cases that might break your code
        /// </summary>
        [Test]
        public void IsPrime_LargeNegativeNumber_ReturnFalse()
        {
            // Arrange
            int input = int.MinValue;
            
            // Act
            var result = _primeService.IsPrime(input);
            
            // Assert
            Assert.That(result, Is.False, "Large negative numbers should not be prime");
        }

        #endregion

        #region Known Prime Numbers

        /// <summary>
        /// Test the smallest prime number
        /// 2 is the first and only even prime number
        /// </summary>
        [Test]
        public void IsPrime_InputIs2_ReturnTrue()
        {
            // Arrange
            int input = 2;
            
            // Act
            var result = _primeService.IsPrime(input);
            
            // Assert
            Assert.That(result, Is.True, "2 is the smallest prime number");
        }

        /// <summary>
        /// Test several known prime numbers using TestCase
        /// This ensures our algorithm works correctly for various primes
        /// </summary>
        [TestCase(2)]   // Smallest prime
        [TestCase(3)]   // Smallest odd prime
        [TestCase(5)]   
        [TestCase(7)]   
        [TestCase(11)]  
        [TestCase(13)]  
        [TestCase(17)]  
        [TestCase(19)]  
        [TestCase(23)]  
        [TestCase(97)]  // Larger two-digit prime
        [TestCase(101)] // Three-digit prime
        public void IsPrime_KnownPrimeNumbers_ReturnTrue(int primeNumber)
        {
            // Act
            var result = _primeService.IsPrime(primeNumber);
            
            // Assert
            Assert.That(result, Is.True, $"{primeNumber} should be identified as prime");
        }

        #endregion

        #region Known Composite Numbers

        /// <summary>
        /// Test that 4 (first composite number) is correctly identified
        /// </summary>
        [Test]
        public void IsPrime_InputIs4_ReturnFalse()
        {
            // Arrange
            int input = 4;
            
            // Act
            var result = _primeService.IsPrime(input);
            
            // Assert
            Assert.That(result, Is.False, "4 is composite (2 × 2), not prime");
        }

        /// <summary>
        /// Test various composite numbers
        /// These should all return false
        /// </summary>
        [TestCase(4)]   // 2²
        [TestCase(6)]   // 2 × 3
        [TestCase(8)]   // 2³
        [TestCase(9)]   // 3²
        [TestCase(10)]  // 2 × 5
        [TestCase(12)]  // 2² × 3
        [TestCase(15)]  // 3 × 5
        [TestCase(21)]  // 3 × 7
        [TestCase(25)]  // 5²
        [TestCase(100)] // 2² × 5²
        public void IsPrime_KnownCompositeNumbers_ReturnFalse(int compositeNumber)
        {
            // Act
            var result = _primeService.IsPrime(compositeNumber);
            
            // Assert
            Assert.That(result, Is.False, $"{compositeNumber} should be identified as composite (not prime)");
        }

        #endregion

        #region Performance and Large Numbers

        /// <summary>
        /// Test with larger numbers to ensure the algorithm is reasonably efficient
        /// This also tests the correctness for larger primes
        /// </summary>
        [TestCase(982451653)] // Large known prime
        [TestCase(982451654)] // Large composite (982451653 + 1)
        public void IsPrime_LargeNumbers_WorksCorrectly(int largeNumber)
        {
            // For performance testing, you might want to measure execution time
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Act
            var result = _primeService.IsPrime(largeNumber);
            
            stopwatch.Stop();
            
            // Assert the result based on known values
            if (largeNumber == 982451653)
            {
                Assert.That(result, Is.True, "982451653 should be prime");
            }
            else
            {
                Assert.That(result, Is.False, "982451654 should not be prime");
            }
            
            // Optional: Assert performance (this might fail on slow machines)
            // Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(1000), 
            //     "Prime check should complete within reasonable time");
        }

        #endregion

        #region Alternative Assert Syntax Examples

        /// <summary>
        /// This test demonstrates different ways to write assertions in NUnit
        /// All of these are equivalent - use the style your team prefers
        /// </summary>
        [Test]
        public void IsPrime_DemonstrateAssertSyntax_InputIs3()
        {
            // Act
            var result = _primeService.IsPrime(3);
            
            // Different ways to assert the same thing:
            
            // Classic Assert syntax (older style)
            Assert.IsTrue(result, "3 should be prime - Classic syntax");
            
            // Constraint-based syntax (newer, more readable)
            Assert.That(result, Is.True, "3 should be prime - Constraint syntax");
            
            // Fluent syntax (most modern)
            Assert.That(result, Is.EqualTo(true), "3 should be prime - Fluent syntax");
            
            // You can also use simple boolean checks
            Assert.That(result, "3 should be prime - Simple boolean check");
        }

        #endregion
    }
}
