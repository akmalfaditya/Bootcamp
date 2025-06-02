/*
 * PrimeService - A Simple Prime Number Service
 * 
 * This class demonstrates basic functionality that we'll test using NUnit.
 * The purpose is to show how to write testable code and then create 
 * comprehensive unit tests for it.
 * 
 * In real development, you'd implement the logic first, but for learning 
 * Test-Driven Development (TDD), we start with a failing implementation
 * and let the tests drive our design.
 */

using System;

namespace Prime.Services
{
    /// <summary>
    /// Service class for prime number operations
    /// This is our "System Under Test" (SUT) - the thing we're going to test
    /// </summary>
    public class PrimeService
    {
        /// <summary>
        /// Determines if a given number is prime
        /// 
        /// A prime number is a natural number greater than 1 that has no positive 
        /// divisors other than 1 and itself. The first few prime numbers are 
        /// 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47...
        /// </summary>        /// <param name="candidate">The number to test for primality</param>
        /// <returns>True if the number is prime, false otherwise</returns>
        public bool IsPrime(int candidate)
        {
            // Handle edge cases - numbers less than 2 are not prime
            if (candidate < 2)
                return false;
            
            // 2 is the only even prime number
            if (candidate == 2)
                return true;
            
            // All other even numbers are not prime
            if (candidate % 2 == 0)
                return false;

            // Small prime optimizations
            if (candidate == 3) return true;
            if (candidate % 3 == 0) return false;

            // Check for factors from 5 to sqrt(candidate)
            // Use 6k±1 optimization: all primes > 3 are of the form 6k±1
            int limit = (int)Math.Sqrt(candidate);
            for (int divisor = 5; divisor <= limit; divisor += 6)
            {
                if (candidate % divisor == 0 || candidate % (divisor + 2) == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Finds all prime numbers up to a given limit using the Sieve of Eratosthenes
        /// This gives us another method to test and demonstrates testing methods that return collections
        /// </summary>
        /// <param name="limit">Find all primes up to this number (inclusive)</param>
        /// <returns>Array of prime numbers up to the limit</returns>
        public int[] FindPrimesUpTo(int limit)
        {
            if (limit < 2)
                return new int[0]; // Return empty array for invalid input

            // Sieve of Eratosthenes algorithm
            bool[] isPrime = new bool[limit + 1];
            
            // Initialize all numbers as potentially prime
            for (int i = 2; i <= limit; i++)
            {
                isPrime[i] = true;
            }

            // Mark non-primes
            for (int i = 2; i * i <= limit; i++)
            {
                if (isPrime[i])
                {
                    // Mark all multiples of i as non-prime
                    for (int j = i * i; j <= limit; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            // Count primes to determine array size
            int primeCount = 0;
            for (int i = 2; i <= limit; i++)
            {
                if (isPrime[i]) primeCount++;
            }

            // Create result array
            int[] primes = new int[primeCount];
            int index = 0;
            for (int i = 2; i <= limit; i++)
            {
                if (isPrime[i])
                {
                    primes[index++] = i;
                }
            }

            return primes;
        }

        /// <summary>
        /// Gets the next prime number after the given number
        /// Demonstrates testing methods with more complex logic
        /// </summary>
        /// <param name="number">Starting number</param>
        /// <returns>The next prime number after the input</returns>
        public int GetNextPrime(int number)
        {
            // Handle edge cases
            if (number < 1)
                return 2; // First prime number
            
            if (number == 1)
                return 2;

            // Start searching from the next number
            int candidate = number + 1;
            
            // Keep searching until we find a prime
            while (!IsPrime(candidate))
            {
                candidate++;
                
                // Prevent infinite loops for very large numbers
                if (candidate < 0) // Integer overflow
                    throw new OverflowException("No next prime found due to integer overflow");
            }

            return candidate;
        }
    }
}
