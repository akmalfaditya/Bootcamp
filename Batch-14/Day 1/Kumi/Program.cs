class Cat 
{
	//camelCase => attribute/variable
	//isAlive
	
	//PascalCase => function/class
	//Eat
	//Sleep

	//variable/attribute
	public bool manja;
	public string colour;
	public bool isMale;
	
	//function/behaviour
	public void Poop() 
	{
		Console.WriteLine("Poop");
	}
	public void Eat() 
	{
		Console.WriteLine("Eat");
	}
}

class Program 
{
	static void Main() 
	{
		Cat kumi = new Cat();
		kumi.manja = true;
		kumi.colour = "Brown";
		kumi.isMale = false;
		Console.WriteLine(kumi.manja);
		Console.WriteLine(kumi.colour);
		Console.WriteLine(kumi.isMale);

		kumi.Poop();
		kumi.Eat();

		Cat john = new Cat();
		john.manja = true;
		john.colour = "White";
		john.isMale = true;
		Console.WriteLine(kumi.manja);
		Console.WriteLine(kumi.colour);
		Console.WriteLine(kumi.isMale);

	}
}