using System;

namespace Classes
{
    /// <summary>
    /// Octopus class demonstrating different types of fields and their behaviors
    /// This class is perfect for understanding instance vs static vs readonly fields
    /// Think of it as a marine biology class in code form!
    /// </summary>
    public class Octopus
    {
        // Instance fields - each octopus gets its own copy of these
        private string _name;
        public int Age = 2;  // Field initializer - all octopuses start at age 2

        // Readonly field - can only be set during declaration or in constructor
        // Each octopus gets a unique ID that can never change after creation
        public readonly Guid Id;

        // Static fields - shared by ALL octopus instances
        // These belong to the Octopus class itself, not individual octopuses
        public static readonly int Legs = 8;     // All octopuses have 8 legs
        public static readonly int Eyes = 2;     // All octopuses have 2 eyes
        
        // Static field that changes - tracks how many octopuses we've created
        private static int _totalCreated = 0;

        // Static property to expose the counter
        public static int TotalCreated => _totalCreated;

        // Multiple field declaration - you can declare several fields of the same type together
        // These are instance fields for tracking octopus behavior
        private bool _isHungry, _isSleeping, _isHiding;

        /// <summary>
        /// Constructor - initializes a new octopus
        /// Shows how readonly fields must be set here if not set during declaration
        /// </summary>
        /// <param name="name">The octopus's name</param>
        public Octopus(string name)
        {
            _name = name ?? "Unnamed Octopus";
            Id = Guid.NewGuid();  // Set the readonly field - this can only happen here!
            
            // Set initial state
            _isHungry = true;     // New octopuses are hungry
            _isSleeping = false;
            _isHiding = false;
            
            // Increment static counter - this affects ALL octopus instances
            _totalCreated++;
            
            Console.WriteLine($"  🐙 New octopus born: {_name} (ID: {Id.ToString()[..8]}..., Total: {_totalCreated})");
        }

        /// <summary>
        /// Property for accessing the name field with validation
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Octopus must have a name!");
                _name = value;
            }
        }

        /// <summary>
        /// Property to check if octopus is hungry
        /// </summary>
        public bool IsHungry
        {
            get { return _isHungry; }
            private set { _isHungry = value; }  // Private setter - only this class can change it
        }

        /// <summary>
        /// Property to check if octopus is sleeping
        /// </summary>
        public bool IsSleeping
        {
            get { return _isSleeping; }
            private set { _isSleeping = value; }
        }

        /// <summary>
        /// Instance method - behavior specific to this octopus
        /// </summary>
        public void Feed()
        {
            if (_isHungry)
            {
                _isHungry = false;
                Console.WriteLine($"  🍤 {_name} is now well-fed and happy!");
            }
            else
            {
                Console.WriteLine($"  😋 {_name} is already full!");
            }
        }

        /// <summary>
        /// Another instance method showing octopus behavior
        /// </summary>
        public void Hide()
        {
            _isHiding = !_isHiding;
            string status = _isHiding ? "hiding in a cave" : "coming out to play";
            Console.WriteLine($"  🕳️ {_name} is now {status}");
        }

        /// <summary>
        /// Method to make the octopus sleep or wake up
        /// </summary>
        public void Sleep()
        {
            _isSleeping = !_isSleeping;
            string status = _isSleeping ? "taking a nap" : "waking up";
            Console.WriteLine($"  😴 {_name} is now {status}");
        }

        /// <summary>
        /// Static method - operates on the class level, not individual instances
        /// You call this as: Octopus.GetSpeciesInfo()
        /// </summary>
        public static void GetSpeciesInfo()
        {
            Console.WriteLine($"  📚 Octopus Species Info:");
            Console.WriteLine($"      🦵 Legs: {Legs}");
            Console.WriteLine($"      👀 Eyes: {Eyes}");
            Console.WriteLine($"      🌊 Habitat: Ocean");
            Console.WriteLine($"      🧠 Intelligence: Very High");
            Console.WriteLine($"      📊 Population in this program: {_totalCreated}");
        }

        /// <summary>
        /// Override ToString for nice string representation
        /// </summary>
        public override string ToString()
        {
            return $"{_name} the Octopus (Age: {Age}, ID: {Id.ToString()[..8]}...)";
        }

        /// <summary>
        /// Finalizer - called when garbage collector destroys this octopus
        /// </summary>
        ~Octopus()
        {
            _totalCreated--;
            Console.WriteLine($"  ⚰️ {_name} the octopus has returned to the sea (Remaining: {_totalCreated})");
        }
    }
}
