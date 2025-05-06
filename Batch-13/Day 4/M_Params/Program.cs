//Params
class Calculator {
	public int Add(params int[] numbers) {
		int result = 0;
		foreach(int i in numbers) {
			result += i;
		}
		return result;
	}
}
class Program {
	static void Main() {
		Calculator calc = new Calculator();
		int result = calc.Add(2,3,4,4,5,7,8,9123123,6,8,4,6,8,78,98);
		Console.WriteLine(result);
	}
}