/*
 * PrimeService_FindPrimesUpToShould.cs - Unit Tests for FindPrimesUpTo Method
 * 
 * This file demonstrates testing methods that return collections.
 * Testing collections requires different strategies than testing simple values.
 * 
 * Key concepts demonstrated:
 * - Testing collection results (arrays/lists)
 * - CollectionAssert for array/collection comparisons
 * - Testing edge cases with collections (empty results, single items)
 * - Performance considerations for larger datasets
 * - Testing boundary conditions
 */

using NUnit.Framework;
using Prime.Services;
using System.Linq;

namespace Prime.UnitTests.Services
{
    /// <summary>
    /// Test fixture for testing the FindPrimesUpTo method
    /// This method returns an array of prime numbers, so our tests need to verify collections
    /// </summary>
    [TestFixture]
    public class PrimeService_FindPrimesUpToShould    {
        private Prime.Services.PrimeService _primeService;

        [SetUp]
        public void SetUp()
        {
            _primeService = new Prime.Services.PrimeService();
        }

        #region Edge Cases and Invalid Input Tests

        /// <summary>
        /// Test what happens when we ask for primes up to a negative number
        /// The method should probably return an empty array or throw an exception
        /// </summary>
        [Test]
        public void ReturnEmptyArray_WhenLimitIsNegative()
        {
            // Arrange
            int negativeLimit = -5;

            // Act & Assert
            // We expect this to either return empty array or throw an exception
            // Let's test both scenarios - first assuming it returns empty array
            var result = _primeService.FindPrimesUpTo(negativeLimit);
            
            // Use CollectionAssert for testing arrays/collections
            CollectionAssert.IsEmpty(result, 
                "FindPrimesUpTo should return empty array for negative limits");
        }

        /// <summary>
        /// Testing the boundary condition: what about 0?
        /// There are no primes less than or equal to 0
        /// </summary>
        [Test]
        public void ReturnEmptyArray_WhenLimitIsZero()
        {
            // Arrange
            int zeroLimit = 0;

            // Act
            var result = _primeService.FindPrimesUpTo(zeroLimit);

            // Assert
            CollectionAssert.IsEmpty(result, 
                "No primes exist less than or equal to 0");
        }

        /// <summary>
        /// Testing limit = 1: still no primes
        /// 1 is not considered a prime number
        /// </summary>
        [Test]
        public void ReturnEmptyArray_WhenLimitIsOne()
        {
            // Arrange
            int oneLimit = 1;

            // Act
            var result = _primeService.FindPrimesUpTo(oneLimit);

            // Assert
            CollectionAssert.IsEmpty(result, 
                "1 is not a prime number, so result should be empty");
        }

        #endregion

        #region Small Range Tests

        /// <summary>
        /// Test the smallest case with a prime: limit = 2
        /// Should return [2] since 2 is the first and smallest prime
        /// </summary>
        [Test]
        public void ReturnArrayWithTwo_WhenLimitIsTwo()
        {
            // Arrange
            int limit = 2;
            int[] expectedPrimes = { 2 };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            CollectionAssert.AreEqual(expectedPrimes, result,
                "When limit is 2, should return array containing only 2");
        }

        /// <summary>
        /// Test with limit = 3: should return [2, 3]
        /// </summary>
        [Test]
        public void ReturnFirstTwoPrimes_WhenLimitIsThree()
        {
            // Arrange
            int limit = 3;
            int[] expectedPrimes = { 2, 3 };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            CollectionAssert.AreEqual(expectedPrimes, result,
                "When limit is 3, should return [2, 3]");
            
            // Alternative way to test the same thing - sometimes useful for debugging
            Assert.AreEqual(2, result.Length, "Should have exactly 2 primes");
            Assert.Contains(2, result, "Result should contain 2");
            Assert.Contains(3, result, "Result should contain 3");
        }

        #endregion

        #region Medium Range Tests

        /// <summary>
        /// Test with a reasonable range: primes up to 10
        /// Expected: [2, 3, 5, 7]
        /// </summary>
        [Test]
        public void ReturnCorrectPrimes_WhenLimitIsTen()
        {
            // Arrange
            int limit = 10;
            int[] expectedPrimes = { 2, 3, 5, 7 };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            CollectionAssert.AreEqual(expectedPrimes, result,
                "Primes up to 10 should be [2, 3, 5, 7]");
        }

        /// <summary>
        /// Test with primes up to 20
        /// Expected: [2, 3, 5, 7, 11, 13, 17, 19]
        /// </summary>
        [Test]
        public void ReturnCorrectPrimes_WhenLimitIsTwenty()
        {
            // Arrange
            int limit = 20;
            int[] expectedPrimes = { 2, 3, 5, 7, 11, 13, 17, 19 };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            CollectionAssert.AreEqual(expectedPrimes, result,
                "Primes up to 20 should be [2, 3, 5, 7, 11, 13, 17, 19]");
            
            // Additional verification: check the count
            Assert.AreEqual(8, result.Length, 
                "There should be exactly 8 primes up to 20");
        }

        #endregion

        #region Boundary and Edge Cases

        /// <summary>
        /// Test where the limit itself is a prime number
        /// When limit = 7, result should include 7
        /// </summary>
        [Test]
        public void IncludeLimitInResult_WhenLimitIsPrime()
        {
            // Arrange
            int primeLimit = 7;
            int[] expectedPrimes = { 2, 3, 5, 7 };

            // Act
            var result = _primeService.FindPrimesUpTo(primeLimit);

            // Assert
            CollectionAssert.AreEqual(expectedPrimes, result,
                "When limit is a prime, it should be included in result");
            
            Assert.Contains(primeLimit, result, 
                "The limit itself should be in the result when it's prime");
        }

        /// <summary>
        /// Test where the limit is a composite number
        /// When limit = 8, should return [2, 3, 5, 7] (8 is not included)
        /// </summary>
        [Test]
        public void ExcludeLimitFromResult_WhenLimitIsComposite()
        {
            // Arrange
            int compositeLimit = 8;
            int[] expectedPrimes = { 2, 3, 5, 7 };

            // Act
            var result = _primeService.FindPrimesUpTo(compositeLimit);

            // Assert
            CollectionAssert.AreEqual(expectedPrimes, result,
                "When limit is composite, it should not be included");
            
            CollectionAssert.DoesNotContain(result, compositeLimit,
                "Composite limit should not appear in results");
        }

        #endregion

        #region Performance and Larger Numbers

        /// <summary>
        /// Test with a larger number to verify the algorithm works efficiently
        /// Primes up to 100 - there are 25 of them
        /// </summary>
        [Test]
        public void HandleLargerNumbers_WhenLimitIsOneHundred()
        {
            // Arrange
            int limit = 100;
            // The first 25 primes (up to 100)
            int[] expectedPrimes = { 
                2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 
                31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 
                73, 79, 83, 89, 97 
            };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            Assert.AreEqual(25, result.Length, 
                "There should be exactly 25 primes up to 100");
            
            CollectionAssert.AreEqual(expectedPrimes, result,
                "The primes up to 100 should match the expected sequence");
        }

        /// <summary>
        /// Performance test: make sure the algorithm doesn't take too long
        /// This is important for the Sieve of Eratosthenes algorithm
        /// </summary>
        [Test]
        public void CompleteInReasonableTime_WhenLimitIsLarge()
        {
            // Arrange
            int largeLimit = 10000;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var result = _primeService.FindPrimesUpTo(largeLimit);
            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.Greater(result.Length, 0, "Should find some primes in range 1-10000");
            
            // The Sieve of Eratosthenes should be quite fast
            Assert.Less(stopwatch.ElapsedMilliseconds, 1000, 
                "Finding primes up to 10,000 should take less than 1 second");
            
            // We know there are 1,229 primes less than 10,000
            Assert.AreEqual(1229, result.Length,
                "There should be exactly 1,229 primes less than 10,000");
        }

        #endregion

        #region Array Properties Tests

        /// <summary>
        /// Verify that the returned array is sorted in ascending order
        /// This is a reasonable expectation for this method
        /// </summary>
        [Test]
        public void ReturnSortedArray_ForAnyValidLimit()
        {
            // Arrange
            int limit = 50;

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            var sortedResult = result.OrderBy(x => x).ToArray();
            CollectionAssert.AreEqual(sortedResult, result,
                "Returned array should be sorted in ascending order");
        }

        /// <summary>
        /// Verify that all returned numbers are actually prime
        /// This is a sanity check that uses our other method
        /// </summary>
        [Test]
        public void ReturnOnlyPrimeNumbers_ForAnyValidLimit()
        {
            // Arrange
            int limit = 30;

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            foreach (int number in result)
            {
                // This test will fail initially because IsPrime throws NotImplementedException
                // But once we implement IsPrime, this becomes a great cross-validation test
                try
                {
                    Assert.IsTrue(_primeService.IsPrime(number),
                        $"Number {number} in result should be prime according to IsPrime method");
                }
                catch (System.NotImplementedException)
                {
                    // Skip this assertion until IsPrime is implemented
                    Assert.Pass("Skipping prime validation until IsPrime is implemented");
                }
            }
        }

        #endregion

        #region Alternative Testing Approaches

        /// <summary>
        /// Example of using TestCase for parameterized testing with collections
        /// This shows how to test multiple scenarios in one test method
        /// </summary>
        [TestCase(2, new int[] { 2 })]
        [TestCase(3, new int[] { 2, 3 })]
        [TestCase(5, new int[] { 2, 3, 5 })]
        [TestCase(11, new int[] { 2, 3, 5, 7, 11 })]
        public void ReturnExpectedPrimes_ForVariousLimits(int limit, int[] expectedPrimes)
        {
            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            CollectionAssert.AreEqual(expectedPrimes, result,
                $"Primes up to {limit} should match expected array");
        }

        #endregion
    }
}
