// See https://aka.ms/new-console-template for more information
interface IAnimal
{
    void MakeSound();
}

class Animal : IAnimal
{
    public void MakeSound()
    {
        Console.WriteLine("Animal sound");
    }
}

class Dog : IAnimal
{
    public void MakeSound()
    {
        Console.WriteLine("Bark");
    }
}

class Cat : IAnimal
{
    public void MakeSound()
    {
        Console.WriteLine("Meow");
    }
}

class Program
{
    static void Main(string[] args)
    {
        IAnimal[] animals = new IAnimal[2];
        animals[0] = new Dog();
        animals[1] = new Cat();

        foreach (IAnimal animal in animals)
        {
            animal.MakeSound();
        }
    }
}

