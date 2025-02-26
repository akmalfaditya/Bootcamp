using System.Diagnostics;
using CalculatorLib;

class Program 
{
	static void Main() 
	{
		Calculator calculator = new Calculator();
		//int result = calculator.Add(3, 4);
		//Console.WriteLine(result);
		//HTTPSEND (result)
		//UI (result)

		//Take input user from Terminal
		//Console.ReadLine return string
		string inputUser = Console.ReadLine();
		string inputUser2 = Console.ReadLine();
		
		//Convert string to Int using Parse
		int inputUserInt = int.Parse(inputUser);
		int inputUser2Int = int.Parse(inputUser2);

		//Call calculator method
		int result = calculator.Add(inputUserInt, inputUser2Int);
		Console.WriteLine(result);
	}
}