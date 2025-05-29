using System;

namespace Classes
{
    /// <summary>
    /// Basic class example showing fields and constructor
    /// This is the foundation - every class you write will have these basic elements
    /// </summary>
    public class Employee
    {
        // Public fields - directly accessible from outside
        // In real-world scenarios, you'd probably use properties instead
        public string Name;
        public int Age;

        /// <summary>
        /// Constructor - this runs when you create a new Employee object
        /// Think of it as the "setup" method for your object
        /// </summary>
        /// <param name="name">Employee's full name</param>
        /// <param name="age">Employee's age in years</param>
        public Employee(string name, int age)
        {
            // Initialize the fields with the provided values
            Name = name;
            Age = age;
            
            Console.WriteLine($"New employee created: {name}");
        }

        /// <summary>
        /// A simple method to display employee information
        /// This shows how methods work within a class
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"Employee: {Name}, Age: {Age}");
        }

        /// <summary>
        /// Method to calculate retirement years remaining
        /// Assumes retirement age of 65
        /// </summary>
        /// <returns>Years until retirement</returns>
        public int YearsUntilRetirement()
        {
            const int retirementAge = 65;
            return Math.Max(0, retirementAge - Age);
        }
    }
}
