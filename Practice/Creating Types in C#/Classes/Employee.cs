using System;

namespace Classes
{
    /// <summary>
    /// Enhanced Employee class demonstrating comprehensive class concepts
    /// This shows how a real business object might look in production code
    /// Covers fields, properties, constructors, methods, static members, and more
    /// </summary>
    public class Employee
    {
        // Private fields - the internal data of our object
        // Using _camelCase naming convention for private fields
        private string _name;
        private int _age;
        private readonly DateTime _hireDate;  // readonly - can only be set in constructor
        private static int _totalEmployees = 0;      // Static field shared by all instances
        private static int _totalWorkHours = 0;      // Tracks total work across all employees

        // Static properties to expose the counters
        public static int TotalEmployees => _totalEmployees;
        public static int TotalWorkHours => _totalWorkHours;

        /// <summary>
        /// Constructor - this runs when you create a new Employee
        /// Notice how we use 'this.' to disambiguate between parameters and fields when needed
        /// </summary>
        /// <param name="name">Employee's full name</param>
        /// <param name="age">Employee's current age</param>
        public Employee(string name, int age)
        {
            this._name = name ?? throw new ArgumentNullException(nameof(name));
            this._age = age;
            this._hireDate = DateTime.Now;  // readonly field can be set in constructor
            
            // Increment the static counter - shared across all instances
            _totalEmployees++;
            
            Console.WriteLine($"  👤 New employee hired: {name} (Total employees: {_totalEmployees})");
        }

        // Public properties to provide controlled access to private fields
        public string Name
        {
            get { return _name; }
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty");
                _name = value;
            }
        }

        public int Age
        {
            get { return _age; }
            set 
            { 
                if (value < 0 || value > 150)
                    throw new ArgumentException("Age must be between 0 and 150");
                _age = value;
            }
        }

        // Read-only property - no setter, exposes readonly field
        public DateTime HireDate => _hireDate;

        // Calculated property - derived from other data
        public int YearsOfService => DateTime.Now.Year - _hireDate.Year;

        /// <summary>
        /// Instance method - behavior that this employee can perform
        /// Shows how methods can modify object state
        /// </summary>
        public void CelebrateBirthday()
        {
            _age++;
            Console.WriteLine($"  🎂 Happy birthday {_name}! Now {_age} years old.");
        }

        /// <summary>
        /// Another instance method showing how employees work
        /// This updates the static field showing shared state across all instances
        /// </summary>
        public void Work()
        {
            int hoursWorked = 8; // Standard work day
            _totalWorkHours += hoursWorked;
            Console.WriteLine($"  💼 {_name} worked {hoursWorked} hours today");
        }

        /// <summary>
        /// Static method - belongs to the Employee class, not any specific instance
        /// You call this on the class itself: Employee.GetCompanyStats()
        /// </summary>
        public static void GetCompanyStats()
        {
            Console.WriteLine($"  📊 Company Stats:");
            Console.WriteLine($"      Total Employees: {_totalEmployees}");
            Console.WriteLine($"      Total Work Hours: {_totalWorkHours}");
            Console.WriteLine($"      Average Hours per Employee: {(_totalEmployees > 0 ? _totalWorkHours / _totalEmployees : 0)}");
        }

        /// <summary>
        /// Override ToString to provide meaningful string representation
        /// This gets called when you print the object or convert it to string
        /// </summary>
        public override string ToString()
        {
            return $"{_name} (Age: {_age}, Hired: {_hireDate:yyyy-MM-dd}, Service: {YearsOfService} years)";
        }

        /// <summary>
        /// Finalizer - called by garbage collector when object is being destroyed
        /// In real code, you rarely need these unless working with unmanaged resources
        /// </summary>
        ~Employee()
        {
            _totalEmployees--;
            // Note: Don't write to Console in real finalizers - this is just for demo
            Console.WriteLine($"  💀 Employee {_name} record finalized");
        }
    }
}
