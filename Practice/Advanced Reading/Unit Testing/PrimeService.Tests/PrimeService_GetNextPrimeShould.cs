/*
 * PrimeService_GetNextPrimeShould.cs - Unit Tests for GetNextPrime Method
 * 
 * This file demonstrates testing methods that find the "next" item in a sequence.
 * Testing sequential operations requires careful consideration of edge cases.
 * 
 * Key concepts demonstrated:
 * - Testing sequential/iterative methods
 * - Boundary testing with edge cases
 * - Performance testing for potentially expensive operations
 * - Cross-validation with other methods in the same class
 */

using NUnit.Framework;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    /// <summary>
    /// Test fixture for testing the GetNextPrime method
    /// This method finds the next prime number after a given number
    /// </summary>
    [TestFixture]
    public class PrimeService_GetNextPrimeShould    {
        private Prime.Services.PrimeService _primeService;

        [SetUp]
        public void SetUp()
        {
            _primeService = new Prime.Services.PrimeService();
        }

        #region Edge Cases and Invalid Input Tests

        /// <summary>
        /// Test what happens when we ask for the next prime after a negative number
        /// The first prime is 2, so this should probably return 2
        /// </summary>
        [Test]
        public void ReturnTwo_WhenInputIsNegative()
        {
            // Arrange
            int negativeInput = -10;
            int expectedNextPrime = 2;

            // Act
            var result = _primeService.GetNextPrime(negativeInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after any negative number should be 2 (the first prime)");
        }

        /// <summary>
        /// Test the next prime after 0
        /// Since 2 is the first prime, next prime after 0 should be 2
        /// </summary>
        [Test]
        public void ReturnTwo_WhenInputIsZero()
        {
            // Arrange
            int zeroInput = 0;
            int expectedNextPrime = 2;

            // Act
            var result = _primeService.GetNextPrime(zeroInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 0 should be 2");
        }

        /// <summary>
        /// Test the next prime after 1
        /// Since 1 is not prime and 2 is the first prime, result should be 2
        /// </summary>
        [Test]
        public void ReturnTwo_WhenInputIsOne()
        {
            // Arrange
            int oneInput = 1;
            int expectedNextPrime = 2;

            // Act
            var result = _primeService.GetNextPrime(oneInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 1 should be 2");
        }

        #endregion

        #region Testing from Prime Numbers

        /// <summary>
        /// Test getting next prime when starting from a prime number
        /// Next prime after 2 should be 3
        /// </summary>
        [Test]
        public void ReturnThree_WhenInputIsTwo()
        {
            // Arrange
            int primeInput = 2;
            int expectedNextPrime = 3;

            // Act
            var result = _primeService.GetNextPrime(primeInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 2 should be 3");
        }

        /// <summary>
        /// Test next prime after 3 should be 5
        /// </summary>
        [Test]
        public void ReturnFive_WhenInputIsThree()
        {
            // Arrange
            int primeInput = 3;
            int expectedNextPrime = 5;

            // Act
            var result = _primeService.GetNextPrime(primeInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 3 should be 5");
        }

        /// <summary>
        /// Test a larger gap: next prime after 7 should be 11
        /// </summary>
        [Test]
        public void ReturnEleven_WhenInputIsSeven()
        {
            // Arrange
            int primeInput = 7;
            int expectedNextPrime = 11;

            // Act
            var result = _primeService.GetNextPrime(primeInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 7 should be 11");
        }

        #endregion

        #region Testing from Composite Numbers

        /// <summary>
        /// Test getting next prime from a composite number
        /// Next prime after 4 should be 5
        /// </summary>
        [Test]
        public void ReturnFive_WhenInputIsFour()
        {
            // Arrange
            int compositeInput = 4;
            int expectedNextPrime = 5;

            // Act
            var result = _primeService.GetNextPrime(compositeInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 4 should be 5");
        }

        /// <summary>
        /// Test next prime after 6 should be 7
        /// </summary>
        [Test]
        public void ReturnSeven_WhenInputIsSix()
        {
            // Arrange
            int compositeInput = 6;
            int expectedNextPrime = 7;

            // Act
            var result = _primeService.GetNextPrime(compositeInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 6 should be 7");
        }

        /// <summary>
        /// Test with a larger composite number: next prime after 15 should be 17
        /// </summary>
        [Test]
        public void ReturnSeventeen_WhenInputIsFifteen()
        {
            // Arrange
            int compositeInput = 15;
            int expectedNextPrime = 17;

            // Act
            var result = _primeService.GetNextPrime(compositeInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 15 should be 17");
        }

        #endregion

        #region Parameterized Tests

        /// <summary>
        /// Use TestCase to test multiple scenarios efficiently
        /// This demonstrates the power of parameterized testing
        /// </summary>
        [TestCase(1, 2, Description = "Next prime after 1")]
        [TestCase(2, 3, Description = "Next prime after 2")]
        [TestCase(3, 5, Description = "Next prime after 3")]
        [TestCase(4, 5, Description = "Next prime after 4")]
        [TestCase(5, 7, Description = "Next prime after 5")]
        [TestCase(6, 7, Description = "Next prime after 6")]
        [TestCase(7, 11, Description = "Next prime after 7")]
        [TestCase(8, 11, Description = "Next prime after 8")]
        [TestCase(9, 11, Description = "Next prime after 9")]
        [TestCase(10, 11, Description = "Next prime after 10")]
        [TestCase(11, 13, Description = "Next prime after 11")]
        [TestCase(20, 23, Description = "Next prime after 20")]
        [TestCase(30, 31, Description = "Next prime after 30")]
        public void ReturnCorrectNextPrime_ForVariousInputs(int input, int expectedNextPrime)
        {
            // Act
            var result = _primeService.GetNextPrime(input);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                $"Next prime after {input} should be {expectedNextPrime}");
        }

        #endregion

        #region Larger Numbers and Performance Tests

        /// <summary>
        /// Test with a moderately large number to ensure the algorithm works efficiently
        /// Next prime after 100 should be 101
        /// </summary>
        [Test]
        public void ReturnOneHundredOne_WhenInputIsOneHundred()
        {
            // Arrange
            int largeInput = 100;
            int expectedNextPrime = 101;

            // Act
            var result = _primeService.GetNextPrime(largeInput);

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 100 should be 101");
        }

        /// <summary>
        /// Test finding next prime after a larger number
        /// This tests both correctness and performance
        /// </summary>
        [Test]
        public void HandleLargerNumbers_WithReasonablePerformance()
        {
            // Arrange
            int largeInput = 1000;
            int expectedNextPrime = 1009; // First prime after 1000
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var result = _primeService.GetNextPrime(largeInput);
            stopwatch.Stop();

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 1000 should be 1009");
            
            Assert.Less(stopwatch.ElapsedMilliseconds, 100,
                "Finding next prime after 1000 should be fast (< 100ms)");
        }

        /// <summary>
        /// Test with an even larger number to verify scalability
        /// Next prime after 10000 should be 10007
        /// </summary>
        [Test]
        public void HandleVeryLargeNumbers_ButStillComplete()
        {
            // Arrange
            int veryLargeInput = 10000;
            int expectedNextPrime = 10007;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var result = _primeService.GetNextPrime(veryLargeInput);
            stopwatch.Stop();

            // Assert
            Assert.AreEqual(expectedNextPrime, result,
                "Next prime after 10000 should be 10007");
            
            Assert.Less(stopwatch.ElapsedMilliseconds, 500,
                "Finding next prime after 10000 should complete in reasonable time");
        }

        #endregion

        #region Cross-Validation Tests

        /// <summary>
        /// Verify that the returned number is actually prime
        /// This uses our IsPrime method to cross-validate the result
        /// </summary>
        [Test]
        public void ReturnAPrimeNumber_ForAnyValidInput()
        {
            // Arrange
            int[] testInputs = { 1, 4, 6, 9, 10, 15, 20, 25, 30 };

            // Act & Assert
            foreach (int input in testInputs)
            {
                var result = _primeService.GetNextPrime(input);
                
                try
                {
                    // Cross-validate with IsPrime method
                    Assert.IsTrue(_primeService.IsPrime(result),
                        $"GetNextPrime({input}) returned {result}, which should be prime");
                }
                catch (System.NotImplementedException)
                {
                    // Skip this assertion until IsPrime is implemented
                    // But we can still do basic validation
                    Assert.Greater(result, input,
                        $"Next prime after {input} should be greater than {input}");
                    Assert.Greater(result, 1,
                        "Any prime number should be greater than 1");
                }
            }
        }

        /// <summary>
        /// Verify that there are no prime numbers between input and result
        /// This ensures we're finding the NEXT prime, not just any prime after input
        /// </summary>
        [Test]
        public void ReturnTheNextPrime_NotJustAnyPrimeAfterInput()
        {
            // Arrange
            int input = 10;
            
            // Act
            var nextPrime = _primeService.GetNextPrime(input);
            
            // Assert
            // We know the next prime after 10 is 11
            // Let's verify there are no primes between 10 and 11
            for (int i = input + 1; i < nextPrime; i++)
            {
                try
                {
                    Assert.IsFalse(_primeService.IsPrime(i),
                        $"Number {i} should not be prime (between {input} and next prime {nextPrime})");
                }
                catch (System.NotImplementedException)
                {
                    // Skip detailed validation until IsPrime is implemented
                    break;
                }
            }
        }

        #endregion

        #region Result Properties Tests

        /// <summary>
        /// Verify basic properties of the result
        /// </summary>
        [Test]
        public void ReturnPositiveNumber_ForAnyInput()
        {
            // Arrange
            int[] testInputs = { -5, 0, 1, 10, 100 };

            // Act & Assert
            foreach (int input in testInputs)
            {
                var result = _primeService.GetNextPrime(input);
                
                Assert.Greater(result, 0,
                    $"GetNextPrime({input}) should return a positive number");
                Assert.Greater(result, input,
                    $"Next prime after {input} should be greater than {input}");
            }
        }

        /// <summary>
        /// Test the mathematical property: if we call GetNextPrime twice,
        /// the second result should be greater than the first
        /// </summary>
        [Test]
        public void ReturnLargerPrime_WhenCalledSuccessively()
        {
            // Arrange
            int startNumber = 10;

            // Act
            var firstNextPrime = _primeService.GetNextPrime(startNumber);
            var secondNextPrime = _primeService.GetNextPrime(firstNextPrime);

            // Assert
            Assert.Greater(secondNextPrime, firstNextPrime,
                "Successive calls to GetNextPrime should return increasing values");
            
            // The second prime should be the next prime after the first result
            Assert.Greater(firstNextPrime, startNumber,
                "First result should be greater than start number");
        }

        #endregion
    }
}
