using System;

namespace TheObjectType
{
    /// <summary>
    /// Person class demonstrating proper ToString(), Equals(), and GetHashCode() overrides
    /// This shows how to make your custom objects play nicely with the object system
    /// </summary>
    public class Person
    {
        // Properties that define our person
        public string Name { get; set; }
        public int Age { get; set; }
        
        /// <summary>
        /// Constructor for creating a person
        /// </summary>
        /// <param name="name">Person's name</param>
        /// <param name="age">Person's age</param>
        public Person(string name, int age)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Age = age;
        }
        
        /// <summary>
        /// Override ToString() to provide meaningful string representation
        /// This gets called automatically by Console.WriteLine, string interpolation, etc.
        /// </summary>
        /// <returns>Human-readable description of the person</returns>
        public override string ToString()
        {
            return $"{Name} (Age: {Age})";
        }
        
        /// <summary>
        /// Override Equals() to define what makes two Person objects equal
        /// Important: if you override Equals, you MUST also override GetHashCode!
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        /// <returns>True if objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            // Quick checks first
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            
            // Cast and compare properties
            Person other = (Person)obj;
            return Name == other.Name && Age == other.Age;
        }
        
        /// <summary>
        /// Override GetHashCode() to work with hash-based collections
        /// Objects that are equal must have the same hash code!
        /// </summary>
        /// <returns>Hash code for this object</returns>
        public override int GetHashCode()
        {
            // Combine hash codes of all relevant properties
            return HashCode.Combine(Name, Age);
        }
        
        /// <summary>
        /// Optional: implement == operator for convenience
        /// </summary>
        public static bool operator ==(Person left, Person right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }
        
        /// <summary>
        /// Optional: implement != operator for consistency
        /// </summary>
        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }
    }
    
    /// <summary>
    /// Product class demonstrating different ToString() style
    /// Shows how different classes can have different string representations
    /// </summary>
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        public Product(string name, decimal price)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
        }
        
        /// <summary>
        /// Product's ToString() shows price formatting
        /// Demonstrates how each class can customize its string representation
        /// </summary>
        public override string ToString()
        {
            return $"{Name} - ${Price:F2}";
        }
        
        /// <summary>
        /// Simple equality based on name and price
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Product other)
            {
                return Name == other.Name && Price == other.Price;
            }
            return false;
        }
        
        /// <summary>
        /// Hash code implementation for Product
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Price);
        }
    }
    
    /// <summary>
    /// Animal class demonstrating inheritance from object
    /// Every class in C# implicitly inherits from System.Object
    /// </summary>
    public class Animal
    {
        public string Species { get; set; }
        public string Sound { get; set; }
        
        public Animal(string species, string sound)
        {
            Species = species;
            Sound = sound;
        }
        
        /// <summary>
        /// Animal's custom ToString() implementation
        /// </summary>
        public override string ToString()
        {
            return $"{Species} says '{Sound}'";
        }
        
        /// <summary>
        /// Method to demonstrate polymorphic behavior
        /// </summary>
        public virtual void MakeSound()
        {
            Console.WriteLine($"The {Species} goes '{Sound}'");
        }
    }
    
    /// <summary>
    /// Dog class inheriting from Animal
    /// Demonstrates inheritance chain: Dog -> Animal -> Object
    /// </summary>
    public class Dog : Animal
    {
        public string Breed { get; set; }
        
        public Dog(string breed) : base("Dog", "Woof")
        {
            Breed = breed;
        }
        
        /// <summary>
        /// Override ToString() to include breed information
        /// </summary>
        public override string ToString()
        {
            return $"{Breed} {base.ToString()}";
        }
        
        /// <summary>
        /// Override virtual method from Animal
        /// </summary>
        public override void MakeSound()
        {
            Console.WriteLine($"The {Breed} dog barks: Woof woof!");
        }
    }
}
