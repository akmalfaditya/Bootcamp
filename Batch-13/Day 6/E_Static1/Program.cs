static class Calculator {
	public static int Add(int a, int b) {
		return a + b;
	}
}

class Program {
	static void Main() {
		int result = Calculator.Add(3,4);
	}
}