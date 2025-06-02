namespace SOLID_and_Kiss_Principles.Models;

/// <summary>
/// Base bird class for demonstrating Liskov Substitution Principle
/// Notice: we removed the Fly() method because not all birds can fly
/// This follows LSP - any subclass should be substitutable for the base class
/// </summary>
public abstract class Bird
{
    public string Name { get; set; } = string.Empty;

    protected Bird(string name)
    {
        Name = name;
    }

    // All birds can eat - this is a safe common behavior
    public virtual void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }

    // All birds make sounds - but each differently
    public abstract void MakeSound();
}

/// <summary>
/// Flyable interface - only birds that can fly implement this
/// This is much cleaner than forcing all birds to have a Fly() method
/// </summary>
public interface IFlyable
{
    void Fly();
}

/// <summary>
/// Swimmable interface - for birds that can swim
/// Interface segregation at work!
/// </summary>
public interface ISwimmable
{
    void Swim();
}

/// <summary>
/// Sparrow can fly - simple and clean
/// </summary>
public class Sparrow : Bird, IFlyable
{
    public Sparrow() : base("Sparrow") { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} chirps: Tweet tweet!");
    }

    public void Fly()
    {
        Console.WriteLine($"{Name} is flying gracefully");
    }
}

/// <summary>
/// Penguin can't fly but can swim
/// No more throwing exceptions or violating LSP!
/// </summary>
public class Penguin : Bird, ISwimmable
{
    public Penguin() : base("Penguin") { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} makes noise: Honk honk!");
    }

    public void Swim()
    {
        Console.WriteLine($"{Name} is swimming like a torpedo");
    }
}

/// <summary>
/// Duck is versatile - can both fly and swim
/// Shows how interfaces can be combined
/// </summary>
public class Duck : Bird, IFlyable, ISwimmable
{
    public Duck() : base("Duck") { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} quacks: Quack quack!");
    }

    public void Fly()
    {
        Console.WriteLine($"{Name} is flying over the pond");
    }

    public void Swim()
    {
        Console.WriteLine($"{Name} is swimming on the water surface");
    }
}
