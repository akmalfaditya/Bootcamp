using System;

namespace Classes
{
    /// <summary>
    /// Car class demonstrating constructor overloading and chaining
    /// Shows how you can create multiple ways to build the same type of object
    /// Think of it like different car dealership packages - basic, standard, or premium
    /// </summary>
    public class Car
    {
        private string _make;
        private string _model;
        private int _year;
        private double _mileage;

        /// <summary>
        /// Basic constructor - just the make (like "Toyota")
        /// This chains to the most detailed constructor using 'this'
        /// </summary>
        /// <param name="make">Car manufacturer</param>
        public Car(string make) : this(make, "Unknown Model", DateTime.Now.Year)
        {
            Console.WriteLine($"  🚗 Created basic car: {make}");
        }

        /// <summary>
        /// Intermediate constructor - make and model
        /// Also chains to the detailed constructor
        /// </summary>
        /// <param name="make">Car manufacturer</param>
        /// <param name="model">Car model</param>
        public Car(string make, string model) : this(make, model, DateTime.Now.Year)
        {
            Console.WriteLine($"  🚗 Created car: {make} {model}");
        }

        /// <summary>
        /// Detailed constructor - this is where the real work happens
        /// The other constructors all lead here via constructor chaining
        /// </summary>
        /// <param name="make">Car manufacturer</param>
        /// <param name="model">Car model</param>
        /// <param name="year">Manufacturing year</param>
        public Car(string make, string model, int year)
        {
            _make = make ?? throw new ArgumentNullException(nameof(make));
            _model = model ?? "Unknown Model";
            _year = year;
            _mileage = 0.0; // New cars start with 0 miles
            
            Console.WriteLine($"  🚗 Created detailed car: {year} {make} {model}");
        }

        // Properties for accessing the car data
        public string Make => _make;
        public string Model => _model;
        public int Year => _year;
        public double Mileage => _mileage;

        /// <summary>
        /// Method to drive the car and add mileage
        /// </summary>
        /// <param name="miles">Miles to drive</param>
        public void Drive(double miles)
        {
            if (miles > 0)
            {
                _mileage += miles;
                Console.WriteLine($"  🛣️ Drove {miles} miles. Total mileage: {_mileage:F1}");
            }
        }

        /// <summary>
        /// Method to get car information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  📋 {_year} {_make} {_model} - {_mileage:F1} miles");
        }

        /// <summary>
        /// Override ToString for nice string representation
        /// </summary>
        public override string ToString()
        {
            return $"{_year} {_make} {_model} ({_mileage:F1} miles)";
        }
    }
}
