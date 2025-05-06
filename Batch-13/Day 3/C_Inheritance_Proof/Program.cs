//Proof inheritance
class Animal {
	public int age;
	public string name;
	public string gender;
	public string race;
	
	public void Eat() {
		Console.WriteLine("Eat");
	}
	public void Poop() {
		Console.WriteLine("Poop");
	}
}
class Cat : Animal {
	public string moustache;
	public void Meow() {}
}
class Program {
	static void Main() {
		Cat cat = new Cat();
		cat.Eat();
		cat.Poop();
	}
}