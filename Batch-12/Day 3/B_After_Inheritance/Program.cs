//After Inheritance
class Animal {
	public int age;
	public string name;
	public string gender;
	public string race;
	
	public void Eat() {}
	public void Poop() {}
}
class Cat : Animal {
	public string moustache;
	public void Meow() {}
}
class Dog : Animal {
	public string moustache;
	public void Bark() {}
}
class Bird : Animal {
	public string wing;
	public void Fly() {}
}