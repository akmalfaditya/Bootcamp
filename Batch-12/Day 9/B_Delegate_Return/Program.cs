//Delegate Return
public delegate int MyDelegate(int a, int b);
class Program {
	static void Main() {
		Calculator calc = new();
		MyDelegate del = calc.Add;
		del += calc.Sub;
		del += calc.Mul;
		
		int result = del.Invoke(5, 3);
		Console.WriteLine(result);
	}
}
class Calculator {
	public int Add(int a, int b) {
		return  a + b;
	}
	public int Sub(int a, int b) {
		return a - b;
	}
	public int Mul(int a, int b) {
		return a * b;
	}
}