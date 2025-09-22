interface IParent {
	void Start();
}
class Parent {
	public void Start() {
		Console.WriteLine("Parent Start");
	}
}
class Child : Parent, IParent {
	public void Start() {
		Console.WriteLine("Child Start");
	}
}
class Program {
	static void Main() {
		Child child  = new Child();
		child.Start();
	}
}