using System;

namespace Classes
{
    /// <summary>
    /// Bunny class demonstrating object initializers
    /// Object initializers provide a clean way to set multiple properties during object creation
    /// They're syntactic sugar that makes code more readable and expressive
    /// </summary>
    public class Bunny
    {
        /// <summary>
        /// Auto-implemented properties for bunny characteristics
        /// Object initializers work great with these
        /// </summary>
        public string Name { get; set; } = "";
        public string Color { get; set; } = "White";
        public int Age { get; set; } = 1;
        public double Weight { get; set; } = 2.5; // in kg
        public bool IsFriendly { get; set; } = true;
        public string FavoriteFood { get; set; } = "Carrots";
        public bool LikesCarrots { get; set; } = true;
        public bool LikesHumans { get; set; } = true;

        /// <summary>
        /// Property with init-only setter - can be set during initialization but not after
        /// This is perfect for data that shouldn't change after object creation
        /// </summary>
        public DateTime BirthDate { get; init; } = DateTime.Now;

        /// <summary>
        /// Computed property based on other properties
        /// </summary>
        public string Description => $"{Color} bunny named {Name}, age {Age}, loves {FavoriteFood}";

        /// <summary>
        /// Default constructor
        /// </summary>
        public Bunny()
        {
            Console.WriteLine($"  🐰 Created a bunny!");
        }

        /// <summary>
        /// Constructor with name parameter
        /// </summary>
        /// <param name="name">The bunny's name</param>
        public Bunny(string name) : this()
        {
            Name = name;
            Console.WriteLine($"  🐰 Created bunny named {name}");
        }

        /// <summary>
        /// Method to make the bunny hop
        /// </summary>
        public void Hop()
        {
            Console.WriteLine($"  🦘 {Name} hops around happily!");
        }

        /// <summary>
        /// Method to feed the bunny
        /// </summary>
        /// <param name="food">Food to give</param>
        public void Feed(string food)
        {
            if (food.ToLower() == FavoriteFood.ToLower())
            {
                Console.WriteLine($"  😋 {Name} loves the {food}! *munch munch*");
            }
            else
            {
                Console.WriteLine($"  🤔 {Name} sniffs the {food} but prefers {FavoriteFood}");
            }
        }

        /// <summary>
        /// Method to pet the bunny
        /// </summary>
        public void Pet()
        {
            if (IsFriendly)
            {
                Console.WriteLine($"  😊 {Name} enjoys being petted and purrs softly");
            }
            else
            {
                Console.WriteLine($"  😤 {Name} is not in the mood for petting right now");
            }
        }

        /// <summary>
        /// Method to display bunny information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  📋 Bunny Info:");
            Console.WriteLine($"      Name: {Name}");
            Console.WriteLine($"      Color: {Color}");
            Console.WriteLine($"      Age: {Age} years old");
            Console.WriteLine($"      Weight: {Weight} kg");
            Console.WriteLine($"      Friendly: {(IsFriendly ? "Yes" : "No")}");
            Console.WriteLine($"      Favorite Food: {FavoriteFood}");
            Console.WriteLine($"      Birth Date: {BirthDate:yyyy-MM-dd}");
        }

        /// <summary>
        /// Override ToString for nice string representation
        /// </summary>
        /// <returns>String description of the bunny</returns>
        public override string ToString()
        {
            return Description;
        }
    }
}
