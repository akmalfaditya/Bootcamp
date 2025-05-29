using System;

namespace Classes
{
    /// <summary>
    /// Demonstrates constructor overloading - providing multiple ways to create objects
    /// This is super useful when you want flexibility in how objects are initialized
    /// </summary>
    public class Car
    {
        public string Make;
        public string Model;
        public int Year;
        public string Color;

        /// <summary>
        /// Constructor with just make - calls the more complete constructor
        /// The : this() syntax is called "constructor chaining"
        /// It's like saying "run this other constructor first, then do anything extra here"
        /// </summary>
        /// <param name="make">Car manufacturer</param>
        public Car(string make) : this(make, "Unknown")
        {
            // This constructor delegates to the two-parameter constructor
            Console.WriteLine("Created car with minimal info");
        }

        /// <summary>
        /// Constructor with make and model
        /// This one also chains to the most complete constructor
        /// </summary>
        /// <param name="make">Car manufacturer</param>
        /// <param name="model">Car model</param>
        public Car(string make, string model) : this(make, model, DateTime.Now.Year, "Not Specified")
        {
            Console.WriteLine("Created car with make and model");
        }

        /// <summary>
        /// Most complete constructor - this is where the actual initialization happens
        /// The other constructors eventually call this one
        /// </summary>
        /// <param name="make">Car manufacturer</param>
        /// <param name="model">Car model</param>
        /// <param name="year">Manufacturing year</param>
        /// <param name="color">Car color</param>
        public Car(string make, string model, int year, string color)
        {
            Make = make;
            Model = model;
            Year = year;
            Color = color;
            
            Console.WriteLine($"Car fully initialized: {year} {make} {model} ({color})");
        }

        /// <summary>
        /// Method to display car information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"{Year} {Make} {Model} - Color: {Color}");
        }

        /// <summary>
        /// Calculate approximate car age
        /// </summary>
        /// <returns>Age in years</returns>
        public int GetAge() => DateTime.Now.Year - Year;
    }
}
