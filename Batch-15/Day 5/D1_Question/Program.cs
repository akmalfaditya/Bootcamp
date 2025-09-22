abstract class Engine { 
	public string type = "type";
	public abstract void Start();
	public void GenerateRotation() {
		Console.WriteLine("Generate rotation...");
	}
}
class Electric : Engine {
	public override void Start() {
		Console.WriteLine("Electric Start()");
	}
}
class Program {
	static void Main() {
		Electric electric = new Electric();
		Console.WriteLine(electric.type);
	}
}