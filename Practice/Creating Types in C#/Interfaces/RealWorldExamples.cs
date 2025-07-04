using System;

namespace Interfaces
{
    /// <summary>
    /// Class vs Interface Design - The Big Picture
    /// This is the fundamental design question: when do you use a class, and when an interface?
    /// 
    /// The rule of thumb:
    /// - Use classes for "is a" relationships (shared implementation and identity)
    /// - Use interfaces for "can do" relationships (shared capabilities)
    /// </summary>

    #region The Problem: Single Inheritance Limitation

    // If these were all classes, we'd have a problem:
    // An Eagle can't inherit from Bird AND FlyingCreature AND Carnivore
    // C# only allows single inheritance from classes

    #endregion

    #region The Solution: Interfaces for Capabilities

    // Base classes for shared implementation and identity ("is a")
    public abstract class Animal
    {
        public abstract string Sound { get; }
        public abstract void Move();
    }

    public abstract class Bird : Animal
    {
        public override void Move() => Console.WriteLine("Flying or walking...");
    }

    public abstract class Insect : Animal
    {
        public override void Move() => Console.WriteLine("Crawling or flying...");
    }

    // Interfaces for capabilities ("can do")
    // Different creatures implement these in completely different ways
    public interface IFlyingCreature
    {
        void Fly();
    }

    public interface ICarnivore
    {
        void Hunt();
    }

    #endregion

    #region Concrete Implementations

    /// <summary>
    /// Ostrich is a Bird, but it can't fly or hunt (in this model)
    /// Shows inheritance without interfaces
    /// </summary>
    public class Ostrich : Bird
    {
        public override string Sound => "Boom!";
    }

    /// <summary>
    /// Eagle is a Bird AND can fly AND can hunt
    /// Perfect example of class + multiple interfaces
    /// </summary>
    public class Eagle : Bird, IFlyingCreature, ICarnivore
    {
        public override string Sound => "Screech!";

        public void Fly()
        {
            Console.WriteLine("Eagle soars majestically through the sky");
        }

        public void Hunt()
        {
            Console.WriteLine("Eagle dives down to catch prey with razor-sharp talons");
        }
    }

    /// <summary>
    /// Bee is an Insect and can fly
    /// Flies completely differently from an Eagle!
    /// </summary>
    public class Bee : Insect, IFlyingCreature
    {
        public override string Sound => "Buzz!";

        public void Fly()
        {
            Console.WriteLine("Bee buzzes rapidly between flowers");
        }
    }

    /// <summary>
    /// Flea is an Insect and is a carnivore (sort of!)
    /// Hunts in a completely different way from an Eagle
    /// </summary>
    public class Flea : Insect, ICarnivore
    {
        public override string Sound => "*silence*";

        public void Hunt()
        {
            Console.WriteLine("Flea jumps onto unsuspecting host");
        }
    }

    #endregion

    /// <summary>
    /// Demo showing the power of this design
    /// </summary>
    public static class ClassVsInterfaceDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Class vs Interface Design Demo ===\n");

            // Create our menagerie
            var animals = new Animal[] 
            { 
                new Ostrich(), 
                new Eagle(), 
                new Bee(), 
                new Flea() 
            };

            Console.WriteLine("Processing all animals...\n");

            foreach (var animal in animals)
            {
                Console.WriteLine($"=== {animal.GetType().Name} ===");
                
                // All animals share base behavior through inheritance
                Console.WriteLine($"Sound: {animal.Sound}");
                animal.Move();

                // Check for flying capability using interface
                if (animal is IFlyingCreature flyer)
                {
                    Console.Write("Flying ability: ");
                    flyer.Fly();
                }

                // Check for hunting capability using interface
                if (animal is ICarnivore hunter)
                {
                    Console.Write("Hunting ability: ");
                    hunter.Hunt();
                }

                Console.WriteLine(); // Blank line
            }

            Console.WriteLine("Key insights:");
            Console.WriteLine("- All animals share identity and basic behavior via class inheritance");
            Console.WriteLine("- Each animal can have unique capabilities via interface implementation");
            Console.WriteLine("- Same interface, completely different implementations");
            Console.WriteLine("- This design is flexible and easily extensible");
        }
    }
}
