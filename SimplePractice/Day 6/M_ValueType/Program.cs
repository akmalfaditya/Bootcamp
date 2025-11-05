//Value type
class Program {
	static void Main() {
		int a = 3;
		Adder add = new();
		add.Add(a);
		Console.WriteLine(a);
	}
}
class Adder {
	public int Add(int x) {
		return x++;
	}
}