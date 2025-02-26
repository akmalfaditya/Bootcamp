//in
class Program {
	static void Main() {
		int a = 3;
		Adder add = new();
		add.Add(in a);
		Console.WriteLine(a);
	}
}
class Adder {
	public void Add(in int x) {
		// x++; readonly
	}
}