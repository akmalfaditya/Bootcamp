//Constructor
class Calculator {
	// no constructor?
	// parameterless constructor
	public Calculator() { 
		Console.WriteLine("Calculator instance created");
	}
}

class Program {
	static void Main() {
		for(int i=0; i < 10; i++) {
			Calculator calculator = new Calculator();
		}
	}
}