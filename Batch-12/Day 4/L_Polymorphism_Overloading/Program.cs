using L_Polymorphism_Overloading;

class Program {
	static void Main() {
		Calculator calculator = new Calculator();
		int a = 1;
		int b = 2;
		int result = calculator.Add(a, b);
		Console.WriteLine(result);

		float fa = 1;
		float fb = 2;
		float fresult = calculator.Add(a, b);
		Console.WriteLine(fresult);

		string sa = "1";
		string sb = "2";
		string sresult = calculator.Add(sa, sb);
		Console.WriteLine(sresult);
	}
}