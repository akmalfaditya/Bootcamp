using System;
using System.Collections.Generic;

namespace Generics
{
    /// <summary>
    /// Base animal class for variance demonstrations
    /// We'll use inheritance to show how covariance and contravariance work
    /// </summary>
    public abstract class Animal
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        
        /// <summary>
        /// Calculate age in years based on birth date
        /// </summary>
        public int Age => DateTime.Now.Year - BirthDate.Year;

        protected Animal(string name)
        {
            Name = name;
            BirthDate = DateTime.Now.AddYears(-2); // Default to 2 years old
        }

        public abstract void MakeSound();
        
        public virtual void Eat()
        {
            Console.WriteLine($"{Name} is eating...");
        }

        public override string ToString()
        {
            return $"{Name} the {GetType().Name}";
        }
    }

    /// <summary>
    /// Dog class - more specific than Animal
    /// </summary>
    public class Dog : Animal
    {
        public string Breed { get; set; }

        public Dog() : this("Unnamed Dog") { }

        public Dog(string name, string breed = "Mixed") : base(name)
        {
            Breed = breed;
        }

        public Dog(string name, int age, string breed = "Mixed") : base(name)
        {
            Breed = breed;
            BirthDate = DateTime.Now.AddYears(-age);
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Woof! Woof!");
        }

        public void Fetch()
        {
            Console.WriteLine($"{Name} is fetching the ball!");
        }

        public override string ToString()
        {
            return $"{Name} the {Breed} Dog";
        }
    }

    /// <summary>
    /// Cat class - also more specific than Animal
    /// </summary>
    public class Cat : Animal
    {
        public bool IsIndoor { get; set; }

        public Cat() : this("Unnamed Cat") { }

        public Cat(string name, bool isIndoor = true) : base(name)
        {
            IsIndoor = isIndoor;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Meow! Meow!");
        }

        public void Purr()
        {
            Console.WriteLine($"{Name} is purring contentedly...");
        }

        public override string ToString()
        {
            return $"{Name} the {(IsIndoor ? "Indoor" : "Outdoor")} Cat";
        }
    }

    /// <summary>
    /// Covariant interface - uses 'out' keyword
    /// This means T can only appear in OUTPUT positions (return values)
    /// Allows assignment of IAnimalProducer<Dog> to IAnimalProducer<Animal>
    /// Think: "A dog producer IS AN animal producer" ✓
    /// </summary>
    /// <typeparam name="T">Type parameter marked as covariant</typeparam>
    public interface IAnimalProducer<out T> where T : Animal
    {
        /// <summary>
        /// Produce an animal - T is in output position, so covariance works
        /// </summary>
        T Produce();

        /// <summary>
        /// Get information about what this producer creates
        /// </summary>
        string GetProducerInfo();

        /// <summary>
        /// Check if this producer can create animals
        /// </summary>
        bool CanProduce { get; }

        // NOTE: We CAN'T have methods like void Consume(T animal) here
        // because T would be in an input position, violating covariance rules
    }

    /// <summary>
    /// Contravariant interface - uses 'in' keyword  
    /// This means T can only appear in INPUT positions (parameters)
    /// Allows assignment of IAnimalConsumer<Animal> to IAnimalConsumer<Dog>
    /// Think: "An animal consumer CAN CONSUME dogs" ✓
    /// </summary>
    /// <typeparam name="T">Type parameter marked as contravariant</typeparam>
    public interface IAnimalConsumer<in T> where T : Animal
    {
        /// <summary>
        /// Consume an animal - T is in input position, so contravariance works
        /// </summary>
        void Consume(T animal);

        /// <summary>
        /// Feed an animal - another input position
        /// </summary>
        void Feed(T animal, string food);

        /// <summary>
        /// Check if this consumer can handle a specific animal type
        /// We can't return T here - that would be an output position!
        /// </summary>
        bool CanHandle(Type animalType);

        // NOTE: We CAN'T have methods like T Produce() here
        // because T would be in an output position, violating contravariance rules
    }

    /// <summary>
    /// Concrete implementation of the covariant producer interface
    /// This class can produce animals of type T
    /// </summary>
    /// <typeparam name="T">Type of animal this producer creates</typeparam>
    public class AnimalProducer<T> : IAnimalProducer<T> where T : Animal, new()
    {
        private readonly string producerName;
        private int animalCount = 0;

        public AnimalProducer()
        {
            producerName = $"{typeof(T).Name} Producer";
        }

        public bool CanProduce => true;

        /// <summary>
        /// Produce a new animal of type T
        /// Uses the 'new()' constraint to create instances
        /// </summary>
        /// <returns>New animal instance</returns>
        public T Produce()
        {
            animalCount++;
            T animal = new T();
            
            // Try to set a unique name if possible
            animal.Name = $"{typeof(T).Name}_{animalCount:D3}";
            
            Console.WriteLine($"  🏭 {producerName} produced: {animal}");
            return animal;
        }

        public string GetProducerInfo()
        {
            return $"{producerName} - Total produced: {animalCount}";
        }
    }

    /// <summary>
    /// Concrete implementation of the contravariant consumer interface
    /// This class can consume animals of type T (and more specific types due to contravariance)
    /// </summary>
    /// <typeparam name="T">Type of animal this consumer can handle</typeparam>
    public class AnimalConsumer<T> : IAnimalConsumer<T> where T : Animal
    {
        private readonly string consumerName;
        private readonly List<string> consumedAnimals = new List<string>();

        public AnimalConsumer()
        {
            consumerName = $"{typeof(T).Name} Consumer";
        }

        /// <summary>
        /// Consume an animal - the contravariant method
        /// </summary>
        /// <param name="animal">Animal to consume</param>
        public void Consume(T animal)
        {
            Console.WriteLine($"  🍽️ {consumerName} is consuming: {animal}");
            consumedAnimals.Add(animal.ToString());
            
            // Make the animal perform its behavior before being consumed
            animal.MakeSound();
            animal.Eat();
        }

        /// <summary>
        /// Feed an animal before consuming
        /// </summary>
        /// <param name="animal">Animal to feed</param>
        /// <param name="food">Type of food</param>
        public void Feed(T animal, string food)
        {
            Console.WriteLine($"  🥣 {consumerName} is feeding {animal} some {food}");
            animal.Eat();
        }

        /// <summary>
        /// Check if this consumer can handle a specific animal type
        /// </summary>
        /// <param name="animalType">Type to check</param>
        /// <returns>True if this consumer can handle the type</returns>
        public bool CanHandle(Type animalType)
        {
            return typeof(T).IsAssignableFrom(animalType);
        }

        /// <summary>
        /// Show consumption history
        /// </summary>
        public void ShowConsumptionHistory()
        {
            Console.WriteLine($"  {consumerName} consumption history:");
            foreach (string animal in consumedAnimals)
            {
                Console.WriteLine($"    - {animal}");
            }
        }
    }

    /// <summary>
    /// Demonstrates variance with built-in .NET interfaces
    /// Shows how covariance and contravariance work with common interfaces
    /// </summary>
    public static class BuiltInVarianceExamples
    {
        /// <summary>
        /// Demonstrate covariance with IEnumerable<T>
        /// IEnumerable<T> is covariant because it's declared as IEnumerable<out T>
        /// </summary>
        public static void DemonstrateIEnumerableCovariance()
        {
            Console.WriteLine("  📚 IEnumerable<T> Covariance:");

            // Create specific collections
            List<Dog> dogs = new List<Dog>
            {
                new Dog("Buddy", "Golden Retriever"),
                new Dog("Max", "German Shepherd")
            };

            List<Cat> cats = new List<Cat>
            {
                new Cat("Whiskers", true),
                new Cat("Shadow", false)
            };

            // Covariance allows this assignment!
            IEnumerable<Animal> dogAnimals = dogs;  // Dogs are Animals
            IEnumerable<Animal> catAnimals = cats;  // Cats are Animals

            Console.WriteLine("    Dogs as Animals:");
            foreach (Animal animal in dogAnimals)
            {
                Console.WriteLine($"      {animal}");
                animal.MakeSound();
            }

            Console.WriteLine("    Cats as Animals:");
            foreach (Animal animal in catAnimals)
            {
                Console.WriteLine($"      {animal}");
                animal.MakeSound();
            }
        }

        /// <summary>
        /// Demonstrate contravariance with Action<T>
        /// Action<T> is contravariant because it's declared as Action<in T>
        /// </summary>
        public static void DemonstrateActionContravariance()
        {
            Console.WriteLine("\n  🎬 Action<T> Contravariance:");

            // Create an action that works with any Animal
            Action<Animal> animalAction = animal =>
            {
                Console.WriteLine($"    Processing animal: {animal}");
                animal.MakeSound();
            };

            // Contravariance allows assignment to more specific types!
            Action<Dog> dogAction = animalAction;  // Animal action can handle Dogs
            Action<Cat> catAction = animalAction;  // Animal action can handle Cats

            // Use the actions
            Dog myDog = new Dog("Rex", "Labrador");
            Cat myCat = new Cat("Fluffy", true);

            Console.WriteLine("    Using contravariant actions:");
            dogAction(myDog);  // Works because Action<Animal> can handle Dog
            catAction(myCat);  // Works because Action<Animal> can handle Cat
        }

        /// <summary>
        /// Demonstrate contravariance with IComparer<T>
        /// IComparer<T> is contravariant because it's declared as IComparer<in T>
        /// </summary>
        public static void DemonstrateIComparerContravariance()
        {
            Console.WriteLine("\n  ⚖️ IComparer<T> Contravariance:");

            // Create a comparer for Animals (by name)
            IComparer<Animal> animalComparer = Comparer<Animal>.Create((a1, a2) =>
                string.Compare(a1.Name, a2.Name, StringComparison.OrdinalIgnoreCase));

            // Contravariance allows using Animal comparer for Dogs!
            IComparer<Dog> dogComparer = animalComparer;

            List<Dog> dogList = new List<Dog>
            {
                new Dog("Zeus", "Great Dane"),
                new Dog("Ace", "Border Collie"),
                new Dog("Bruno", "Bulldog")
            };

            Console.WriteLine("    Dogs before sorting:");
            dogList.ForEach(dog => Console.WriteLine($"      {dog}"));

            // Sort using the contravariant comparer
            dogList.Sort(dogComparer);

            Console.WriteLine("    Dogs after sorting (using Animal comparer):");
            dogList.ForEach(dog => Console.WriteLine($"      {dog}"));
        }
    }

    /// <summary>
    /// Advanced variance scenarios and edge cases
    /// Shows more complex situations where variance applies
    /// </summary>
    public static class AdvancedVarianceScenarios
    {
        /// <summary>
        /// Delegate variance examples
        /// Shows how variance works with custom delegates
        /// </summary>
        public static void DemonstrateDelegateVariance()
        {
            Console.WriteLine("  🎯 Delegate Variance:");

            // Covariant delegate (return type)
            Func<Dog> dogFactory = () => new Dog("Factory Dog", "Factory Breed");
            Func<Animal> animalFactory = dogFactory;  // Covariance!

            Animal factoryAnimal = animalFactory();
            Console.WriteLine($"    Factory produced: {factoryAnimal}");

            // Contravariant delegate (parameter type)
            Action<Animal> animalHandler = animal =>
            {
                Console.WriteLine($"    Handling: {animal}");
                animal.MakeSound();
            };

            Action<Dog> dogHandler = animalHandler;  // Contravariance!
            dogHandler(new Dog("Handled Dog", "Handled Breed"));
        }

        /// <summary>
        /// Show variance limitations and compile errors
        /// Demonstrates what you CAN'T do with variance
        /// </summary>
        public static void ShowVarianceLimitations()
        {
            Console.WriteLine("\n  ⚠️ Variance Limitations:");

            // These would cause compile errors if uncommented:

            // ILLEGAL: Can't assign List<Dog> to List<Animal>
            // List<Animal> is NOT covariant because List<T> allows both input and output
            // List<Animal> animals = new List<Dog>();  // Compile error!

            // ILLEGAL: Can't add Cat to a List<Dog> even if assigned to List<Animal>
            // This is why List<T> can't be covariant - it would break type safety

            Console.WriteLine("    ✓ List<T> is invariant - no variance allowed");
            Console.WriteLine("    ✓ IList<T> is invariant - has both input and output operations");
            Console.WriteLine("    ✓ Only interfaces with pure input (in) or pure output (out) can be variant");

            // Show the correct way to work with collections
            List<Dog> dogs = new List<Dog> { new Dog("Safe Dog") };
            List<Animal> animals = new List<Animal>(dogs);  // Copy, not assignment
            Console.WriteLine($"    Copied {dogs.Count} dogs to animal list safely");
        }

        /// <summary>
        /// Generic method variance examples
        /// Shows how variance affects generic method calls
        /// </summary>
        public static void DemonstrateGenericMethodVariance()
        {
            Console.WriteLine("\n  🔧 Generic Method Variance:");

            // Generic method that accepts IEnumerable<T> (covariant)
            static void ProcessAnimals<T>(IEnumerable<T> animals) where T : Animal
            {
                Console.WriteLine($"    Processing {typeof(T).Name} collection:");
                foreach (T animal in animals)
                {
                    Console.WriteLine($"      - {animal}");
                    animal.MakeSound();
                }
            }

            List<Dog> dogs = new List<Dog>
            {
                new Dog("Method Dog 1"),
                new Dog("Method Dog 2")
            };

            // This works because of covariance in IEnumerable<T>
            ProcessAnimals<Animal>(dogs);  // Dogs treated as Animals

            // Type inference also works
            ProcessAnimals(dogs);  // Compiler infers T as Dog
        }
    }
}
