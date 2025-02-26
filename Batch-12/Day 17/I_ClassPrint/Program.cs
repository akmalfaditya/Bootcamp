class Program 
{
	static void Main() 
	{
		Car car = new();
		car.name = "toyota";
		Console.WriteLine(car);

		object x;
	}
}
class Car 
{
	public string name;
	public override string ToString()
	{
		return name;
	}
	
}