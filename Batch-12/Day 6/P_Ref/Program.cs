//Ref
class Program {
	static void Main() {
		int a = 2;
		Adder add = new();
		add.Add(ref a);
		Console.WriteLine(a);
	}
}
class Adder {
	public void Add(ref int x) {
		x++;
	}
}