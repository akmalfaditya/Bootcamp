using System.Diagnostics;

class Program 
{
	static void Main() 
	{
		Console.WriteLine("Program starting");
		Calculator calculator = new();
		int result = calculator.Add(3, 5);
		Console.WriteLine(result);
	}
}
class Calculator 
{
	public int Add(int a, int b) => a * b;
}