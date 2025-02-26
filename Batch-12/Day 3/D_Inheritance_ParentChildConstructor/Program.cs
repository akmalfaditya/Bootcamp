class Animal {
	public Animal() {
		Console.WriteLine("Animal Created");
	}
}
class Cat : Animal {
	public Cat() {
		Console.WriteLine("Cat created");
	}
}
class Program {
	static void Main() {
		Animal animal = new Animal();
	}
}