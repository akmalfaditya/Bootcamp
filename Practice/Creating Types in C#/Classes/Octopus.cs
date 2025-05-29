using System;

namespace Classes
{
    /// <summary>
    /// Demonstrates different types of fields:
    /// - Instance fields (regular fields)
    /// - Static fields (shared across all instances)
    /// - Readonly fields (can only be set in constructor)
    /// </summary>
    public class Octopus
    {
        // Instance field - each octopus has its own name
        public string Name;
        
        // Static field - shared by ALL octopuses
        // No matter how many octopus objects you create, there's only one Legs field
        public static int Legs = 8;
        
        // Readonly field - can only be assigned in constructor or at declaration
        // Once set, it cannot be changed during the object's lifetime
        public readonly int Age;
        
        // Private field - only accessible within this class
        // This is good practice for encapsulation
        private readonly DateTime birthDate;

        /// <summary>
        /// Constructor that initializes the octopus
        /// Notice how we can set readonly fields here
        /// </summary>
        /// <param name="name">The octopus's name</param>
        public Octopus(string name)
        {
            Name = name;
            Age = 5; // All octopuses start at age 5 for this demo
            birthDate = DateTime.Now.AddYears(-Age);
            
            Console.WriteLine($"Created octopus: {name} with {Legs} legs");
        }

        /// <summary>
        /// Method to get birth date (accessing private field)
        /// </summary>
        /// <returns>When this octopus was born</returns>
        public DateTime GetBirthDate()
        {
            return birthDate;
        }

        /// <summary>
        /// Static method - belongs to the class, not any specific instance
        /// You call this like: Octopus.GetSpeciesInfo()
        /// </summary>
        /// <returns>General information about octopuses</returns>
        public static string GetSpeciesInfo()
        {
            return $"Octopuses are marine animals with {Legs} legs and high intelligence.";
        }
    }
}
