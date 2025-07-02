using System;

namespace Classes
{
    /// <summary>
    /// Panda class demonstrating the 'this' keyword
    /// The 'this' keyword refers to the current instance of the class
    /// It's useful for disambiguation and method chaining
    /// </summary>
    public class Panda
    {
        private string _name;
        private int _age;
        private double _weight;
        private bool _isHappy;
        private Panda? _mate;

        /// <summary>
        /// Constructor using 'this' to avoid parameter name conflicts
        /// Without 'this', you'd need different parameter names
        /// </summary>
        /// <param name="name">Panda's name</param>
        /// <param name="age">Panda's age</param>
        /// <param name="weight">Panda's weight in kg</param>
        public Panda(string name, int age = 2, double weight = 100.0)
        {
            // 'this.name' refers to the field, 'name' refers to the parameter
            this._name = name ?? "Unnamed Panda";
            this._age = age;
            this._weight = weight;
            this._isHappy = true;
            
            Console.WriteLine($"  🐼 Created panda: {this._name}, age {this._age}, weight {this._weight}kg");
        }

        /// <summary>
        /// Overloaded constructor - demonstrates constructor chaining with 'this'
        /// </summary>
        /// <param name="name">Panda's name</param>
        public Panda(string name) : this(name, 2, 100.0)
        {
            Console.WriteLine($"  🐼 Used chained constructor for {this._name}");
        }

        /// <summary>
        /// Properties demonstrating 'this' keyword usage
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine($"  ⚠️ Invalid name provided, keeping current name: {this._name}");
                    return;
                }
                this._name = value;
                Console.WriteLine($"  ✏️ Panda renamed to: {this._name}");
            }
        }

        public int Age
        {
            get { return this._age; }
            set 
            { 
                if (value < 0)
                {
                    Console.WriteLine($"  ⚠️ Age cannot be negative, keeping current age: {this._age}");
                    return;
                }
                this._age = value;
            }
        }

        public double Weight
        {
            get { return this._weight; }
            set 
            { 
                if (value <= 0)
                {
                    Console.WriteLine($"  ⚠️ Weight must be positive, keeping current weight: {this._weight}kg");
                    return;
                }
                this._weight = value;
                Console.WriteLine($"  📏 {this._name}'s weight updated to {this._weight}kg");
            }
        }

        public bool IsHappy
        {
            get { return this._isHappy; }
            private set { this._isHappy = value; }
        }

        /// <summary>
        /// Property for panda's mate
        /// </summary>
        public Panda? Mate
        {
            get { return this._mate; }
        }

        /// <summary>
        /// Method demonstrating fluent interface using 'this' for method chaining
        /// Returns 'this' to allow chaining: panda.Eat().Sleep().Play()
        /// </summary>
        /// <param name="food">Food to eat</param>
        /// <returns>This panda instance for chaining</returns>
        public Panda Eat(string food = "bamboo")
        {
            Console.WriteLine($"  🎋 {this._name} is eating {food}");
            this._isHappy = true;
            this._weight += 0.5; // Gain a bit of weight
            
            // Return 'this' to enable method chaining
            return this;
        }

        /// <summary>
        /// Another chainable method
        /// </summary>
        /// <returns>This panda instance for chaining</returns>
        public Panda Sleep()
        {
            Console.WriteLine($"  😴 {this._name} is taking a nap");
            this._isHappy = true;
            return this; // Enable chaining
        }

        /// <summary>
        /// Another chainable method
        /// </summary>
        /// <returns>This panda instance for chaining</returns>
        public Panda Play()
        {
            Console.WriteLine($"  🎮 {this._name} is playing and having fun!");
            this._isHappy = true;
            this._weight -= 0.2; // Lose a bit of weight from exercise
            return this; // Enable chaining
        }

        /// <summary>
        /// Method to set mate
        /// </summary>
        /// <param name="otherPanda">Another panda to be mate with</param>
        public void SetMate(Panda otherPanda)
        {
            if (otherPanda == null)
            {
                Console.WriteLine($"  ❌ {this._name} cannot have a null mate");
                return;
            }

            if (otherPanda == this) // Using 'this' for comparison
            {
                Console.WriteLine($"  😅 {this._name} cannot be mate with themselves!");
                return;
            }

            this._mate = otherPanda;
            otherPanda._mate = this; // Set reciprocal relationship
            Console.WriteLine($"  💕 {this._name} and {otherPanda._name} are now mates!");
        }

        /// <summary>
        /// Method that takes another Panda parameter
        /// Uses 'this' to compare with the other panda
        /// </summary>
        /// <param name="otherPanda">Another panda to compare with</param>
        /// <returns>True if this panda is older</returns>
        public bool IsOlderThan(Panda otherPanda)
        {
            if (otherPanda == null)
            {
                Console.WriteLine($"  ❓ {this._name} cannot compare age with a null panda");
                return false;
            }
            
            bool isOlder = this._age > otherPanda._age;
            Console.WriteLine($"  📊 {this._name} (age {this._age}) is {(isOlder ? "older than" : "not older than")} {otherPanda._name} (age {otherPanda._age})");
            return isOlder;
        }

        /// <summary>
        /// Method to introduce this panda to another
        /// </summary>
        /// <param name="otherPanda">Another panda to meet</param>
        public void MeetPanda(Panda otherPanda)
        {
            if (otherPanda == null)
            {
                Console.WriteLine($"  😕 {this._name} cannot meet a null panda");
                return;
            }
            
            if (ReferenceEquals(this, otherPanda))
            {
                Console.WriteLine($"  🤔 {this._name} cannot meet themselves!");
                return;
            }
            
            Console.WriteLine($"  👋 {this._name} meets {otherPanda._name}!");
            this._isHappy = true;
            otherPanda._isHappy = true;
        }

        /// <summary>
        /// Static method to demonstrate 'this' cannot be used in static context
        /// Static methods belong to the type, not to any instance
        /// </summary>
        /// <param name="panda">Panda to get info about</param>
        /// <returns>Species info</returns>
        public static string GetSpeciesInfo(Panda panda)
        {
            // Note: Cannot use 'this' here because this is a static method
            string info = "🐼 Giant Pandas are native to China";
            if (panda != null)
            {
                info += $"\n   {panda._name} is a beautiful example of this species!";
            }
            return info;
        }

        /// <summary>
        /// Method to display panda information
        /// Uses 'this' to refer to current instance fields
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  📋 Panda Info:");
            Console.WriteLine($"      Name: {this._name}");
            Console.WriteLine($"      Age: {this._age} years");
            Console.WriteLine($"      Weight: {this._weight}kg");
            Console.WriteLine($"      Happy: {(this._isHappy ? "Yes" : "No")}");
        }

        /// <summary>
        /// Override ToString using 'this' keyword
        /// </summary>
        /// <returns>String representation of this panda</returns>
        public override string ToString()
        {
            return $"{this._name} the Panda (Age: {this._age}, Weight: {this._weight}kg, Happy: {this._isHappy})";
        }

        /// <summary>
        /// Demonstrate method chaining with 'this'
        /// </summary>
        public void DemonstrateChaining()
        {
            Console.WriteLine($"  🔗 Demonstrating method chaining for {this._name}:");
            
            // This beautiful chain is possible because each method returns 'this'
            this.Eat("fresh bamboo")
                .Sleep()
                .Play()
                .Eat("bamboo shoots");
                
            Console.WriteLine($"  ✅ Chaining complete! {this._name} is now {(this._isHappy ? "happy" : "not happy")}");
        }
    }
}
