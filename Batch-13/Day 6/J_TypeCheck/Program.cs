//type checker
class Calculator {
	public object Add(object a, object b) {
		if( a is int && b is int) {
			return (int)a + (int) b;
		}
		if( a is string && b is string) {
			return (string)a + (string) b;
		}
		return null;
	}
}
class Program {
 	static void Main() {
		int a = 3;
		int b = 2;
		Calculator calc = new();
		int result = (int) calc.Add(3,2);
		result.Dump();
		string result2 = (string) calc.Add("string","string");
		result2.Dump();
	}
}	
class Car {}