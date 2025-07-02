using System;

namespace Classes
{
    /// <summary>
    /// Person class demonstrating primary constructors (C# 12 feature)
    /// Primary constructors provide a concise way to declare constructor parameters
    /// The parameters become available throughout the class body
    /// Think of it as a shorthand for common constructor + field patterns
    /// </summary>
    /// <param name="firstName">Person's first name</param>
    /// <param name="lastName">Person's last name</param>
    public class Person(string firstName, string lastName)
    {
        // The parameters firstName and lastName are available throughout the class!
        // No need to declare separate fields or write constructor code
        
        /// <summary>
        /// Property using primary constructor parameters
        /// You can use the parameters directly in property definitions
        /// </summary>
        public string FullName => $"{firstName} {lastName}";

        /// <summary>
        /// Field initialized using primary constructor parameters
        /// The parameters can be used to initialize fields
        /// </summary>
        private readonly string _initials = $"{(firstName.Length > 0 ? firstName[0] : 'X')}.{(lastName.Length > 0 ? lastName[0] : 'X')}.";

        /// <summary>
        /// Auto-property for age (not part of primary constructor)
        /// You can still have regular properties alongside primary constructor parameters
        /// </summary>
        public int Age { get; set; } = 0;

        /// <summary>
        /// Auto-property for email
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// Property that uses primary constructor parameters in getter
        /// </summary>
        public string Initials => _initials;

        /// <summary>
        /// Property that validates and normalizes the first name
        /// You can create properties that work with the primary constructor parameters
        /// </summary>
        public string FirstName 
        { 
            get => firstName;
            // Note: You cannot set primary constructor parameters after construction
            // They are effectively readonly
        }

        /// <summary>
        /// Property for last name
        /// </summary>
        public string LastName 
        { 
            get => lastName;
        }

        /// <summary>
        /// Method using primary constructor parameters
        /// The parameters are available in all methods
        /// </summary>
        public void Introduce()
        {
            Console.WriteLine($"  👋 Hello! I'm {firstName} {lastName}");
            if (Age > 0)
            {
                Console.WriteLine($"  🎂 I'm {Age} years old");
            }
            if (!string.IsNullOrEmpty(Email))
            {
                Console.WriteLine($"  📧 You can reach me at {Email}");
            }
        }

        /// <summary>
        /// Method to update age
        /// </summary>
        /// <param name="newAge">New age value</param>
        public Person SetAge(int newAge)
        {
            if (newAge >= 0)
            {
                Age = newAge;
                Console.WriteLine($"  📅 {firstName}'s age updated to {Age}");
            }
            else
            {
                Console.WriteLine($"  ⚠️ Invalid age: {newAge}. Age must be non-negative.");
            }
            return this; // Return this for method chaining
        }

        /// <summary>
        /// Method to update email
        /// </summary>
        /// <param name="email">Email address</param>
        public Person SetEmail(string email)
        {
            Email = email ?? "";
            Console.WriteLine($"  📧 {firstName}'s email updated to: {Email}");
            return this; // Return this for method chaining
        }

        /// <summary>
        /// Method that demonstrates using primary constructor parameters in logic
        /// </summary>
        /// <param name="otherPerson">Another person to compare with</param>
        /// <returns>True if they have the same last name</returns>
        public bool HasSameLastNameAs(Person otherPerson)
        {
            if (otherPerson == null)
                return false;
                
            // Using lastName parameter from primary constructor and property from other instance
            bool sameLastName = string.Equals(lastName, otherPerson.LastName, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"  👥 {firstName} {lastName} and {otherPerson.FirstName} {otherPerson.LastName} {(sameLastName ? "have the same" : "have different")} last names");
            return sameLastName;
        }

        /// <summary>
        /// Method to create a formal name
        /// </summary>
        /// <param name="title">Title to add (Mr., Ms., Dr., etc.)</param>
        /// <returns>Formal name with title</returns>
        public string GetFormalName(string title = "")
        {
            if (string.IsNullOrEmpty(title))
                return $"{firstName} {lastName}";
            
            return $"{title} {firstName} {lastName}";
        }

        /// <summary>
        /// Method to display person information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  📋 Person Information:");
            Console.WriteLine($"      Full Name: {FullName}");
            Console.WriteLine($"      First Name: {firstName}");  // Using primary constructor parameter
            Console.WriteLine($"      Last Name: {lastName}");    // Using primary constructor parameter
            Console.WriteLine($"      Initials: {Initials}");
            Console.WriteLine($"      Age: {(Age > 0 ? Age.ToString() : "Not specified")}");
            Console.WriteLine($"      Email: {(string.IsNullOrEmpty(Email) ? "Not provided" : Email)}");
        }

        /// <summary>
        /// Method to print person information (alias for DisplayInfo)
        /// </summary>
        public void PrintInfo()
        {
            DisplayInfo();
        }

        /// <summary>
        /// Override ToString using primary constructor parameters
        /// </summary>
        /// <returns>String representation of the person</returns>
        public override string ToString()
        {
            // Primary constructor parameters available here too!
            return $"{firstName} {lastName} ({Age} years old)";
        }

        /// <summary>
        /// Static method to demonstrate creating Person with primary constructor
        /// </summary>
        /// <param name="fullName">Full name to parse</param>
        /// <returns>New Person instance</returns>
        public static Person ParseFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return new Person("Unknown", "Person");
            
            string[] parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length == 1)
                return new Person(parts[0], "");
            
            if (parts.Length >= 2)
                return new Person(parts[0], parts[parts.Length - 1]); // First and last name
                
            return new Person("Unknown", "Person");
        }

        /// <summary>
        /// Demonstrate primary constructor features
        /// </summary>
        public static void DemonstratePrimaryConstructor()
        {
            Console.WriteLine($"  🏗️ Demonstrating Primary Constructor Features:");
            
            // Creating instances with primary constructor
            var person1 = new Person("Alice", "Johnson");
            var person2 = new Person("Bob", "Johnson");
            
            person1.SetAge(30);
            person1.SetEmail("alice@example.com");
            
            person2.SetAge(25);
            person2.SetEmail("bob@example.com");
            
            person1.Introduce();
            person2.Introduce();
            
            person1.HasSameLastNameAs(person2);
            
            Console.WriteLine($"  🎓 Formal name: {person1.GetFormalName("Dr.")}");
            
            // Parse full name
            var person3 = Person.ParseFullName("Charlie Brown");
            person3.DisplayInfo();
        }
    }
}
