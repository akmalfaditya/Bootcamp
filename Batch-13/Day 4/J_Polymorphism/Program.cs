//Overriding
class Animal {
	public string name;
	public int age;
	public void Eat() {
		Console.WriteLine("Eat");
	}
	public virtual void MakeSound() {
		Console.WriteLine("...");
	}
}
class Cat : Animal {
	public override void MakeSound() {
		Console.WriteLine("Meow!");
	}
}
class Ant : Animal {}
class Program {
	static void Main() {
		Animal animal = new();
		animal.MakeSound(); //...
		Cat cat = new();
		cat.MakeSound(); //Meow
		Ant ant = new();
		ant.MakeSound(); //...
	}
}