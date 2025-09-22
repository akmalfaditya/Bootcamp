//Anonymous Method / Lambda Expression
class Program {
	static void Main() {
		var add = (int a, int b) => a+b;
		int result = add(3,4);
		result.Dump();
		add(5,6).Dump();
	}
}
class Adder {
	public int Add(int a,int b) {
		return a + b;
	}
}
//======================
class Program {
	static void Main() {
		Action<string> print = (string message) => Console.WriteLine(message);
		print("message");
	}
}
//class Printer {
//	public void Print(string message) {
//		Console.WriteLine(message);
//	}
//}
//======================
class Program {
	static void Main() {
		Func<int> printer = () => 3;
		printer().Dump();
	}
}
//class Printer {
//	public int Print() {
//		return 3;
//	}
//}
//======================
class Program {
	static void Main() {
		var print = () => Console.WriteLine(3);
		print();
	}
}
//class Printer {
//	public void Print() {
//		Console.WriteLine(3);
//	}
//}
