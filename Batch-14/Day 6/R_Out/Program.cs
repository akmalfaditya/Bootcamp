//Out
class Program {
	static void Main() {
		int a;
		Adder add = new();
		add.Add(out a);
		Console.WriteLine(a);
	}
}
class Adder {
	public void Add(out int x) {
		x = 3;
	}
}