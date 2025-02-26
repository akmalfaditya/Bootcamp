//object
class Calculator {
	public object Add(object a, object b) {
		return (int)a+(int)b;
	}
}
class Program {
 	static void Main() {
		int a = 3;
		int b = 2;
		Calculator calc = new();
		calc.Add(3,2);
		//calc.Add(new Car() , new Car() );
	}
}	
class Car {}