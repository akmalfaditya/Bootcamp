abstract class Engine { 
	public string type;
	public void Start() {}
	public void GenerateRotation() {
		Console.WriteLine("Generate rotation...");
	}
}
class Electric : Engine {
}
class Program {
	static void Main() {
		Electric electric = new Electric();
	}
}